---
post_title: .NET 8 Networking Improvements
author1: mapichov
author2: knatalia
post_slug: dotnet-8-networking-improvements
microsoft_alias: mapichov
featured_image: dotnet8-networking-improvments.png
categories: .NET, Networking
tags: .net 8, http, metrics, http, sockets
summary: Introducing new networking features in .NET 8 including HTTP space, metrics, sockets and more!
post_date: 2023-12-07 10:05:00
---

It has become a tradition to publish a blog post about new interesting changes in networking space with new [.NET release](https://devblogs.microsoft.com/dotnet/announcing-dotnet-8/). This year, we'd like to introduce changes in [HTTP](#http) space, newly added [metrics](#metrics), new [`HttpClientFactory`](#httpclientfactory) APIs and more.

## HTTP

### Metrics

.NET 8 adds built-in HTTP Metrics to both ASP.NET Core and `HttpClient` using the [System.Diagnostics.Metrics API](https://learn.microsoft.com/dotnet/core/diagnostics/metrics) that was introduced in .NET 6. Both the Metrics APIs and the semantics of the new built-in metrics were designed in close cooperation with OpenTelemetry, making sure the new metrics are consistent with the standard and work well with popular tools like [Prometheus](https://prometheus.io/) and [Grafana](https://grafana.com/).

The `System.Diagnostics.Metrics` API introduces many new features which were missing from the EventCounters. These features are utilized extensively by the new built-in metrics, resulting in a wider functionality achieved by a simpler and more elegant set of instruments. To list a couple of examples:
- [Histograms](https://learn.microsoft.com/dotnet/api/system.diagnostics.metrics.meter.createhistogram?view=net-8.0) enable us to report the durations, eg. the request duration ([`http.client.request.duration`](https://learn.microsoft.com/dotnet/core/diagnostics/built-in-metrics-system-net#instrument-httpclientrequestduration)) or the connection duration ([`http.client.connection.duration`](https://learn.microsoft.com/dotnet/core/diagnostics/built-in-metrics-system-net#instrument-httpclientconnectionduration)). These are new metrics without EventCounter counterparts.
- [Multi-dimensionality](https://learn.microsoft.com/dotnet/core/diagnostics/metrics-instrumentation#multi-dimensional-metrics) allows us to attach tags (a.k.a. attributes or labels) to measurements, meaning that we can report information like `server.address` (identifies the [URI origin](https://www.rfc-editor.org/rfc/rfc9110.html#name-uri-origin)), or `error.type` (describes the error reason if a request fails) together with the measurements. Multi-dimensionality also enables simplification: to report the number of open HTTP connections `SocketsHttpHandler` uses 3 EventCounters: [`http11-connections-current-total`, `http20-connections-current-total` and `http30-connections-current-total`](https://learn.microsoft.com/dotnet/core/diagnostics/available-counters#systemnethttp-counters), while the Metrics equivalent of these counters is a single instrument, [`http.client.open_connections`](https://learn.microsoft.com/dotnet/core/diagnostics/built-in-metrics-system-net#instrument-httpclientopen_connections) where the HTTP version is reported using the `network.protocol.version` tag. 
- To help use cases where the built in tags aren't sufficient to categorize outgoing HTTP requests, the `http.client.request.duration` metric supports the injection of user-defined tags. This is called [enrichment](https://learn.microsoft.com/dotnet/fundamentals/networking/telemetry/metrics#enrichment).
- [`IMeterFactory` integration](https://learn.microsoft.com/dotnet/fundamentals/networking/telemetry/metrics#imeterfactory-and-ihttpclientfactory-integration) enables the isolation of the [`Meter`](https://learn.microsoft.com/dotnet/api/system.diagnostics.metrics.meter) instances used to emit the HTTP metrics, making it easier to write tests that run validation against the built-in measurements, and enabling the parallel execution of such tests.
- While this is not specific to the built-in networking metrics, it's worth to mention that the collection APIs in `System.Digangostics.Metrics` are also more advanced: they are strongly typed and more performant, and allow multiple simultaneous listeners and listener access to unaggregated measurements.

These advantages together result in better, richer metrics which can be collected more efficiently by 3rd party tools like Prometheus.
Thanks to the flexibility of [PromQL (Prometheus Query Language)](https://prometheus.io/docs/prometheus/latest/querying/basics/), which allows creating sophisticated queries against the multi-dimensional metrics collected from the .NET networking stack, users can now get insights about the status and health of `HttpClient` and `SocketsHttpHandler` instances on a level that was not previously possible.

On the downside, we should mention that only the `System.Net.Http` and the `System.Net.NameResolution` components are instrumented using `System.Diagnostics.Metrics` in .NET 8, meaning that you still need to use EventCounters to extract counters from the lower levels of the stack such as `System.Net.Sockets`.
While all built-in EventCounters which existed in previous versions are still supported, the .NET team doesn't expect to make substantial new investments into EventCounters, and new built-in instrumentation will be added using `System.Diagnostics.Metrics` in future versions.

For more information about using built-in HTTP metrics, please read our tutorial on [Networking metrics in .NET](https://learn.microsoft.com/dotnet/fundamentals/networking/telemetry/metrics). It includes examples about collection and reporting using Prometheus and Grafana, and also demonstrates how to enrich and test built-in HTTP metrics. For a comprehensive list of built-in instruments see the docs for [System.Net metrics](https://learn.microsoft.com/dotnet/core/diagnostics/built-in-metrics-system-net). In case you are more interested about the server-side, please read the docs on [ASP.NET Core metrics](https://learn.microsoft.com/aspnet/core/log-mon/metrics/metrics).

### Extended Telemetry

Apart from the new metrics, the existing `EventSource` based telemetry events introduced in [.NET 5](https://devblogs.microsoft.com/dotnet/net-5-new-networking-improvements/#telemetry) were augmented with more information about HTTP connections ([dotnet/runtime#88853](https://github.com/dotnet/runtime/pull/88853)):

```diff
- ConnectionEstablished(byte versionMajor, byte versionMinor)
+ ConnectionEstablished(byte versionMajor, byte versionMinor, long connectionId, string scheme, string host, int port, string? remoteAddress)

- ConnectionClosed(byte versionMajor, byte versionMinor)
+ ConnectionClosed(byte versionMajor, byte versionMinor, long connectionId)

- RequestHeadersStart()
+ RequestHeadersStart(long connectionId)
```

Now, when a new connection is established, the event logs its `connectionId` together with its scheme, port, and peer IP address. This enables correlating requests and responses with connections via the `RequestHeadersStart` event - which occurs when a request is associated to a pooled connection and starts being processed - which also logs the associated `connectionId`. This is especially valuable in diagnostic scenarios where users want to see the IP addresses of the servers their HTTP requests are served by, which was the main motivation behind the addition ([dotnet/runtime#63159](https://github.com/dotnet/runtime/issues/63159)).

The events can be consumed in many ways, see [Networking telemetry in .NET - Events](https://learn.microsoft.com/dotnet/fundamentals/networking/telemetry/events). But for the sake of in-process enhanced logging, a custom [`EventListener`](https://learn.microsoft.com/dotnet/api/system.diagnostics.tracing.eventlistener?view=net-8.0) can be used to correlate the request/response pair with the connection data:

```csharp
using IPLoggingListener ipLoggingListener = new();
using HttpClient client = new();

// Send requests in parallel.
await Parallel.ForAsync(0, 1000, async (i, ct) =>
{
    // Initialize the async local so that it can be populated by "RequestHeadersStart" event handler.
    RequestInfo info = RequestInfo.Current;
    using var response = await client.GetAsync("https://testserver");
    Console.WriteLine($"Response {response.StatusCode} handled by connection {info.ConnectionId}. Remote IP: {info.RemoteAddress}");

    // Process response...
});

internal sealed class RequestInfo
{
    private static readonly AsyncLocal<RequestInfo> _asyncLocal = new();
    public static RequestInfo Current => _asyncLocal.Value ??= new();

    public string? RemoteAddress;
    public long ConnectionId;
}

internal sealed class IPLoggingListener : EventListener
{
    private static readonly ConcurrentDictionary<long, string> s_connection2Endpoint = new ConcurrentDictionary<long, string>();

    // EventId corresponds to [Event(eventId)] attribute argument and the payload indices correspond to the event method argument order.

    // See: https://github.com/dotnet/runtime/blob/a6e4834d53ac591a4b3d4a213a8928ad685f7ad8/src/libraries/System.Net.Http/src/System/Net/Http/HttpTelemetry.cs#L100-L101
    private const int ConnectionEstablished_EventId = 4;
    private const int ConnectionEstablished_ConnectionIdIndex = 2;
    private const int ConnectionEstablished_RemoteAddressIndex = 6;

    // See: https://github.com/dotnet/runtime/blob/a6e4834d53ac591a4b3d4a213a8928ad685f7ad8/src/libraries/System.Net.Http/src/System/Net/Http/HttpTelemetry.cs#L106-L107
    private const int ConnectionClosed_EventId = 5;
    private const int ConnectionClosed_ConnectionIdIndex = 2;

    // See: https://github.com/dotnet/runtime/blob/a6e4834d53ac591a4b3d4a213a8928ad685f7ad8/src/libraries/System.Net.Http/src/System/Net/Http/HttpTelemetry.cs#L118-L119
    private const int RequestHeadersStart_EventId = 7;
    private const int RequestHeadersStart_ConnectionIdIndex = 0;

    protected override void OnEventSourceCreated(EventSource eventSource)
    {
        if (eventSource.Name == "System.Net.Http")
        {
            EnableEvents(eventSource, EventLevel.LogAlways);
        }
    }

    protected override void OnEventWritten(EventWrittenEventArgs eventData)
    {
        ReadOnlyCollection<object?>? payload = eventData.Payload;
        if (payload == null) return;

        switch (eventData.EventId)
        {
            case ConnectionEstablished_EventId:
                // Remember the connection data.
                long connectionId = (long)payload[ConnectionEstablished_ConnectionIdIndex]!;
                string? remoteAddress = (string?)payload[ConnectionEstablished_RemoteAddressIndex];
                if (remoteAddress != null)
                {
                    Console.WriteLine($"Connection {connectionId} established to {remoteAddress}");
                    s_connection2Endpoint.TryAdd(connectionId, remoteAddress);
                }
                break;
            case ConnectionClosed_EventId:
                connectionId = (long)payload[ConnectionClosed_ConnectionIdIndex]!;
                s_connection2Endpoint.TryRemove(connectionId, out _);
                break;
            case RequestHeadersStart_EventId:
                // Populate the async local RequestInfo with data from "ConnectionEstablished" event.
                connectionId = (long)payload[RequestHeadersStart_ConnectionIdIndex]!;
                if (s_connection2Endpoint.TryGetValue(connectionId, out remoteAddress))
                {
                    RequestInfo.Current.RemoteAddress = remoteAddress;
                    RequestInfo.Current.ConnectionId = connectionId;
                }
                break;
        }
    }
}
```

Additionally, the `Redirect` event has been extended to include the redirect URI:

```diff
-void Redirect();
+void Redirect(string redirectUri);
```

### HTTP Error Codes

One of the diagnostics problems of `HttpClient` was that in case of an exception it was not easy to distinguish _programmatically_ the exact root cause for the error. The only way to differentiate many of these was to parse the exception message from [`HttpRequestException`](https://learn.microsoft.com/dotnet/api/system.net.http.httprequestexception?view=net-8.0). Moreover, other HTTP implementations, like WinHTTP with [`ERROR_WINHTTP_*`](https://learn.microsoft.com/windows/win32/winhttp/error-messages) error codes, offer such functionality in the form of either numerical codes or enumerations. So .NET 8 introduces a similar enumeration and provides it in exceptions thrown from HTTP processing, which are:
- [`HttpRequestException`](https://learn.microsoft.com/dotnet/api/system.net.http.httprequestexception?view=net-8.0) for request processing up until receiving response headers.
- [`HttpIOException`](https://learn.microsoft.com/dotnet/api/system.net.http.httpioexception?view=net-8.0) for response content reading.

The design of [`HttpRequestError`](https://learn.microsoft.com/dotnet/api/system.net.http.httprequesterror?view=net-8.0) enum and how it's plugged into HTTP exceptions is described in [dotnet/runtime#76644](https://github.com/dotnet/runtime/issues/76644) API proposal.

Now, a consumer of an `HttpClient` methods can handle specific internal errors much more easily and reliably:

```csharp
using HttpClient httpClient = new();

// Handling problems with the server:
try
{
    using HttpResponseMessage response = await httpClient.GetAsync("https://testserver", HttpCompletionOption.ResponseHeadersRead);
    using Stream responseStream = await response.Content.ReadAsStreamAsync();
    // Process responseStream ...
}
catch (HttpRequestException e) when (e.HttpRequestError == HttpRequestError.NameResolutionError)
{
    Console.WriteLine($"Unknown host: {e}");
    // --> Try different hostname.
}
catch (HttpRequestException e) when (e.HttpRequestError == HttpRequestError.ConnectionError)
{
    Console.WriteLine($"Server unreachable: {e}");
    // --> Try different server.
}
catch (HttpIOException e) when (e.HttpRequestError == HttpRequestError.InvalidResponse)
{
    Console.WriteLine($"Mangled responses: {e}");
    // --> Block list server.
}

// Handling problems with HTTP version selection:
try
{
    using HttpResponseMessage response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, "https://testserver")
    {
        Version = HttpVersion.Version20,
        VersionPolicy = HttpVersionPolicy.RequestVersionExact
    }, HttpCompletionOption.ResponseHeadersRead);
    using Stream responseStream = await response.Content.ReadAsStreamAsync();
    // Process responseStream ...
}
catch (HttpRequestException e) when (e.HttpRequestError == HttpRequestError.VersionNegotiationError)
{
    Console.WriteLine($"HTTP version is not supported: {e}");
    // Try with different HTTP version.
}

```

### HTTPS Proxy Support

One of the highly requested features that got implemented with this release is support for HTTPS proxies ([dotnet/runtime#31113](https://github.com/dotnet/runtime/issues/31113)). It's possible now to use proxies serving requests over HTTPS, meaning the connection _to_ the proxy is secure. That doesn't say anything about the request itself _from_ the proxy, which can still be both HTTP or HTTPS. In case of a plain text HTTP request, the connection to an HTTPS proxy is secure (over HTTPS), followed by plain text request from proxy to the destination. And in case of an HTTPS request (proxy tunnel), the initial `CONNECT` request to open the tunnel will be sent over secured channel (HTTPS) to the proxy, followed by the HTTPS request from the proxy to the destination through the tunnel.

To take advantage of the feature, all that's needed is to use an HTTPS scheme when setting up the proxy:


```csharp
using HttpClient client = new HttpClient(new SocketsHttpHandler()
{
    Proxy = new WebProxy("https://proxy.address:12345")
});

using HttpResponseMessage response = await client.GetAsync("https://httpbin.org/");
```

## HttpClientFactory

.NET 8 extends the ways in which you can configure [`HttpClientFactory`](https://learn.microsoft.com/dotnet/core/extensions/httpclient-factory), including client defaults, custom logging, and simplified `SocketsHttpHandler` configuration. The APIs are implemented in the `Microsoft.Extensions.Http` package which is available on NuGet and includes support for .NET Standard 2.0. Therefore, this functionality is usable for customers not just on .NET 8, but on all versions of .NET, including .NET Framework (the only exception are the `SocketsHttpHandler` related APIs that are only available for .NET 5+).

### Set Up Defaults For All Clients

.NET 8 adds the ability to set default configuration that would be used for all `HttpClient`s created by `HttpClientFactory` ([dotnet/runtime#87914](https://github.com/dotnet/runtime/issues/87914)). This is useful when all or most of the registered clients contain the same subset of the configuration.

Consider an example where two named clients are defined, and they both need `MyAuthHandler` in their message handlers chain.

```csharp
services.AddHttpClient("consoto", c => c.BaseAddress = new Uri("https://consoto.com/"))
    .AddHttpMessageHandler<MyAuthHandler>();

services.AddHttpClient("github", c => c.BaseAddress = new Uri("https://github.com/"))
    .AddHttpMessageHandler<MyAuthHandler>();
```

To extract the common part, you can now use the [`ConfigureHttpClientDefaults`](https://learn.microsoft.com/dotnet/api/microsoft.extensions.dependencyinjection.httpclientfactoryservicecollectionextensions.configurehttpclientdefaults) method:

```csharp
services.ConfigureHttpClientDefaults(b => b.AddHttpMessageHandler<MyAuthHandler>());

// both clients will have MyAuthHandler added by default
services.AddHttpClient("consoto", c => c.BaseAddress = new Uri("https://consoto.com/"));
services.AddHttpClient("github", c => c.BaseAddress = new Uri("https://github.com/"));
```

All [`IHttpClientBuilder`](https://learn.microsoft.com/dotnet/api/microsoft.extensions.dependencyinjection.ihttpclientbuilder) extension methods that are used with `AddHttpClient`, can be used within `ConfigureHttpClientDefaults` as well.

The default configuration (`ConfigureHttpClientDefaults`) is applied to all clients _before_ the client-specific (`AddHttpClient`) configurations; their relative position in the registration doesn't matter. `ConfigureHttpClientDefaults` can be registered multiple times, in this case the configurations will be applied one-by-one in the order of registration. Any part of the configuration can be overridden or modified in the client-specific configurations, for example, you can set additional settings to `HttpClient` object or to a primary handler, remove previously added additional handler, etc.

Note that since 8.0, [`ConfigureHttpMessageHandlerBuilder`](https://learn.microsoft.com/dotnet/api/microsoft.extensions.dependencyinjection.httpclientbuilderextensions.configurehttpmessagehandlerbuilder) method is _deprecated_. You should use [`ConfigurePrimaryHttpMessageHandler(Action<HttpMessageHandler,IServiceProvider>)`](https://learn.microsoft.com/dotnet/api/microsoft.extensions.dependencyinjection.httpclientbuilderextensions.configureprimaryhttpmessagehandler?view=dotnet-plat-ext-8.0#microsoft-extensions-dependencyinjection-httpclientbuilderextensions-configureprimaryhttpmessagehandler(microsoft-extensions-dependencyinjection-ihttpclientbuilder-system-action((system-net-http-httpmessagehandler-system-iserviceprovider)))) or [`ConfigureAdditionalHttpMessageHandlers`](https://learn.microsoft.com/dotnet/api/microsoft.extensions.dependencyinjection.httpclientbuilderextensions.configureadditionalhttpmessagehandlers) methods instead, to modify a previously configured primary handler or a list of additional handlers respectively.

```csharp
// by default, adds User-Agent header, uses HttpClientHandler with UseCookies=false
// as a primary handler, and adds MyAuthHandler to all clients
services.ConfigureHttpClientDefaults(b =>
    b.ConfigureHttpClient(c => c.DefaultRequestHeaders.UserAgent.ParseAdd("HttpClient/8.0"))
     .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler() { UseCookies = false })
     .AddHttpMessageHandler<MyAuthHandler>());

// HttpClient will have both User-Agent (from defaults) and BaseAddress set
// + client will have UseCookies=false and MyAuthHandler from defaults
services.AddHttpClient("modify-http-client", c => c.BaseAddress = new Uri("https://httpbin.org/"))

// primary handler will have both UseCookies=false (from defaults) and MaxConnectionsPerServer set
// + client will have User-Agent and MyAuthHandler from defaults
services.AddHttpClient("modify-primary-handler")
    .ConfigurePrimaryHandler((h, _) => ((HttpClientHandler)h).MaxConnectionsPerServer = 1);

// MyWrappingHandler will be inserted at the top of the handlers chain
// + client will have User-Agent, UseCookies=false and MyAuthHandler from defaults
services.AddHttpClient("insert-handler-into-chain"))
    .ConfigureAdditionalHttpMessageHandlers((handlers, _) =>
        handlers.Insert(0, new MyWrappingHandler());

// MyAuthHandler (initially from defaults) will be removed from the handler chain
// + client will still have User-Agent and UseCookies=false from defaults
services.AddHttpClient("remove-handler-from-chain"))
    .ConfigureAdditionalHttpMessageHandlers((handlers, _) =>
        handlers.Remove(handlers.Single(h => h is MyAuthHandler)));
```

### Modify HttpClient Logging

Customizing (or even simply turning off) `HttpClientFactory` logging was one of the long-requested features ([dotnet/runtime#77312](https://github.com/dotnet/runtime/issues/77312)).

#### Old Logging Overview

Default ("old") logging added by `HttpClientFactory` is quite verbose and emits 8 log messages per request:
1. _Start notification_ with request URI -- before propagating through the delegating handler pipeline;
2. _Request headers_ -- before handler pipeline;
3. _Start notification_ with request URI -- after handler pipeline;
4. _Request headers_ -- after handler pipeline;
5. _Stop notification_ with elapsed time -- before propagating the response back through the delegating handler pipeline;
6. _Response headers_ -- before propagating the response back;
7. _Stop notification_ with elapsed time -- after propagating the response back;
8. _Response headers_ -- after propagating the response back.

This can be illustrated with the diagram below. In this and the following diagrams, `*` and `[...]` denote a logging event (in the default implementation, a log message being written into `ILogger`), and `-->` symbolizes data flow through the _Application_ and _Transport_ layers.


```fsharp
  Request -->
*   [Start notification]    // "Start processing HTTP request ..." (1)
*   [Request headers]       // "Request Headers: ..." (2)
      --> Additional Handler #1 -->
        --> .... -->
          --> Additional Handler #N -->
*           [Start notification]    // "Sending HTTP request ..." (3)
*           [Request headers]       // "Request Headers: ..." (4)
                --> Primary Handler -->
                      --------Transport--layer------->
                                          // Server sends response
                      <-------Transport--layer--------
                <-- Primary Handler <--
*           [Stop notification]    // "Received HTTP response ..." (5)
*           [Response headers]     // "Response Headers: ..." (6)
          <-- Additional Handler #N <--
        <-- .... <--
      <-- Additional Handler #1 <--
*   [Stop notification]    // "End processing HTTP request ..." (7)
*   [Response headers]     // "Response Headers: ..." (8)
  Response <--
```

Console output of the default `HttpClientFactory` logging looks like this:

```csharp
var client = _httpClientFactory.CreateClient();
await client.GetAsync("https://httpbin.org/get");
```

```Output
info: System.Net.Http.HttpClient.test.LogicalHandler[100]
      Start processing HTTP request GET https://httpbin.org/get
trce: System.Net.Http.HttpClient.test.LogicalHandler[102]
      Request Headers:
      ....
info: System.Net.Http.HttpClient.test.ClientHandler[100]
      Sending HTTP request GET https://httpbin.org/get
trce: System.Net.Http.HttpClient.test.ClientHandler[102]
      Request Headers:
      ....
info: System.Net.Http.HttpClient.test.ClientHandler[101]
      Received HTTP response headers after 581.2898ms - 200
trce: System.Net.Http.HttpClient.test.ClientHandler[103]
      Response Headers:
      ....
info: System.Net.Http.HttpClient.test.LogicalHandler[101]
      End processing HTTP request after 618.9736ms - 200
trce: System.Net.Http.HttpClient.test.LogicalHandler[103]
      Response Headers:
      ....
```

Note that in order to see `Trace` level messages, you need to opt-in to that in the global logging configuration file or via [`SetMinimumLevel(LogLevel.Trace)`](https://learn.microsoft.com/dotnet/api/microsoft.extensions.logging.loggingbuilderextensions.setminimumlevel?view=dotnet-plat-ext-8.0). But even considering only `Informational` messages, "old" logging still has 4 messages per request.

To remove the default (or previously added) logging, you can use the new [`RemoveAllLoggers()`](https://learn.microsoft.com/dotnet/api/microsoft.extensions.dependencyinjection.httpclientbuilderextensions.removeallloggers) extension method. It is especially powerful combined with the `ConfigureHttpClientDefaults` API described in the [Set Up Defaults For All Clients](#set-up-defaults-for-all-clients) section above. This way, you can remove the "old" logging for all of the clients in one line:

```csharp
services.ConfigureHttpClientDefaults(b => b.RemoveAllLoggers()); // remove HttpClientFactory default logging for all clients
```

If you ever need to bring back the "old" logging, e.g. for a specific client, you can do so using [`AddDefaultLogger()`](https://learn.microsoft.com/dotnet/api/microsoft.extensions.dependencyinjection.httpclientbuilderextensions.adddefaultlogger).

#### Add Custom Logging

In addition to the ability to remove the "old" logging, new `HttpClientFactory` APIs also allow you to fully customize the logging. You can specify what and how will be logged when `HttpClient` starts a request, receives a response or throws an exception.

You can add several custom loggers alongside, if you choose to do so -- for example, both Console and ETW loggers, or both ["wrapping" and "not wrapping"](#wrapping-and-not-wrapping-loggers) loggers. Because of its additive nature, you might need to explicitly remove the default "old" logging beforehand.

To add custom logging, you need to implement the [`IHttpClientLogger`](https://learn.microsoft.com/dotnet/api/microsoft.extensions.http.logging.ihttpclientlogger) interface and then add the custom logger to your client with [`AddLogger`](https://learn.microsoft.com/dotnet/api/microsoft.extensions.dependencyinjection.httpclientbuilderextensions.addlogger). Note that logging implementation _should not_ throw any Exceptions, otherwise it can disrupt the request execution.

Registration:

```csharp
services.AddSingleton<SimpleConsoleLogger>(); // register the logger in DI

services.AddHttpClient("foo") // add a client
    .RemoveAllLoggers() // remove previous logging
    .AddLogger<SimpleConsoleLogger>(); // add the custom logger
```

Sample logger implementation:

```csharp
// outputs one line per request to console
public class SimpleConsoleLogger : IHttpClientLogger
{
    public object? LogRequestStart(HttpRequestMessage request) => null;

    public void LogRequestStop(object? ctx, HttpRequestMessage request, HttpResponseMessage response, TimeSpan elapsed)
        => Console.WriteLine($"{request.Method} {request.RequestUri?.AbsoluteUri} - {(int)response.StatusCode} {response.StatusCode} in {elapsed.TotalMilliseconds}ms");

    public void LogRequestFailed(object? ctx, HttpRequestMessage request, HttpResponseMessage? response, Exception e, TimeSpan elapsed)
        => Console.WriteLine($"{request.Method} {request.RequestUri?.AbsoluteUri} - Exception {e.GetType().FullName}: {e.Message}");
}
```

Sample output:

```csharp
var client = _httpClientFactory.CreateClient("foo");
await client.GetAsync("https://httpbin.org/get");
await client.PostAsync("https://httpbin.org/post", new ByteArrayContent(new byte[] { 42 }));
await client.GetAsync("http://httpbin.org/status/500");
await client.GetAsync("http://localhost:1234");
```
```Output
GET https://httpbin.org/get - 200 OK in 393.2039ms
POST https://httpbin.org/post - 200 OK in 95.524ms
GET https://httpbin.org/status/500 - 500 InternalServerError in 99.5025ms
GET http://localhost:1234/ - Exception System.Net.Http.HttpRequestException: No connection could be made because the target machine actively refused it. (localhost:1234)
```

#### Request Context Object

A context object can be used to match the [`LogRequestStart`](https://learn.microsoft.com/dotnet/api/microsoft.extensions.http.logging.ihttpclientlogger.logrequeststart) call with the corresponding [`LogRequestStop`](https://learn.microsoft.com/dotnet/api/microsoft.extensions.http.logging.ihttpclientlogger.logrequeststop) call to pass data from one to the other. Context object is produced by `LogRequestStart` and then passed back to `LogRequestStop`. This can be a property bag or any other object that holds the necessary data.

If a context object is not needed, implementation can return `null` from `LogRequestStart`.

The following example shows how context object can be used to pass a custom request identifier.

```csharp
public class RequestIdLogger : IHttpClientLogger
{
    private readonly ILogger _log;

    public RequestIdLogger(ILogger<RequestIdLogger> log)
    {
        _log = log;
    }

    private static readonly Action<ILogger, Guid, string?, Exception?> _requestStart =
        LoggerMessage.Define<Guid, string?>(
            LogLevel.Information,
            EventIds.RequestStart,
            "Request Id={RequestId} ({Host}) started");

    private static readonly Action<ILogger, Guid, double, Exception?> _requestStop =
        LoggerMessage.Define<Guid, double>(
            LogLevel.Information,
            EventIds.RequestStop,
            "Request Id={RequestId} succeeded in {elapsed}ms");

    private static readonly Action<ILogger, Guid, Exception?> _requestFailed =
        LoggerMessage.Define<Guid>(
            LogLevel.Error,
            EventIds.RequestFailed,
            "Request Id={RequestId} FAILED");

    public object? LogRequestStart(HttpRequestMessage request)
    {
        var ctx = new Context(Guid.NewGuid());
        _requestStart(_log, ctx.RequestId, request.RequestUri?.Host, null);
        return ctx;
    }

    public void LogRequestStop(object? ctx, HttpRequestMessage request, HttpResponseMessage response, TimeSpan elapsed)
        => _requestStop(_log, ((Context)ctx!).RequestId, elapsed.TotalMilliseconds, null);

    public void LogRequestFailed(object? ctx, HttpRequestMessage request, HttpResponseMessage? response, Exception e, TimeSpan elapsed)
        => _requestFailed(_log, ((Context)ctx!).RequestId, null);

    public static class EventIds
    {
        public static readonly EventId RequestStart = new(1, "RequestStart");
        public static readonly EventId RequestStop = new(2, "RequestStop");
        public static readonly EventId RequestFailed = new(3, "RequestFailed");
    }

    record Context(Guid RequestId);
}
```

```Output
info: RequestIdLogger[1]
      Request Id=d0d63b84-cd67-4d21-ae9a-b63d26dfde50 (httpbin.org) started
info: RequestIdLogger[2]
      Request Id=d0d63b84-cd67-4d21-ae9a-b63d26dfde50 succeeded in 530.1664ms
info: RequestIdLogger[1]
      Request Id=09403213-dd3a-4101-88e8-db8ab19e1eeb (httpbin.org) started
info: RequestIdLogger[2]
      Request Id=09403213-dd3a-4101-88e8-db8ab19e1eeb succeeded in 83.2484ms
info: RequestIdLogger[1]
      Request Id=254e49bd-f640-4c56-b62f-5de678eca129 (httpbin.org) started
info: RequestIdLogger[2]
      Request Id=254e49bd-f640-4c56-b62f-5de678eca129 succeeded in 162.7776ms
info: RequestIdLogger[1]
      Request Id=e25ccb08-b97e-400d-b42b-b09d6c42adec (localhost) started
fail: RequestIdLogger[3]
      Request Id=e25ccb08-b97e-400d-b42b-b09d6c42adec FAILED
```

#### Avoid Reading From Content Streams

If you intend to read and log, for example, request and response Content, please be aware that it can potentially have an _adverse  side effect_ on the end user experience and cause bugs. For example, request content might become consumed before it was sent, or response content of a huge size might end up being buffered in memory. In addition, before .NET 7, accessing headers was not thread-safe and might lead to errors and unexpected behavior.

#### Use Async Logging With Caution

We expect that the synchronous `IHttpClientLogger` interface would be suitable for the vast majority of the custom logging use cases. It is advised to refrain from using async in logging for performance reasons. However, in case async access within the logging is strictly required, you can implement the asynchronous version [`IHttpClientAsyncLogger`](https://learn.microsoft.com/dotnet/api/microsoft.extensions.http.logging.ihttpclientasynclogger). It derives from `IHttpClientLogger`, so it can use the same `AddLogger` API for registration.

Note that in that case sync counterparts of the logging methods should be implemented as well, especially if the implementation is a part of a library targeting .NET Standard or .NET 5+. Sync counterparts are called from sync `HttpClient.Send` methods; even if .NET Standard surface doesn’t include them, .NET Standard library can be used in a .NET 5+ application, so the end users would have the access to sync `HttpClient.Send` methods.

#### Wrapping And Not Wrapping Loggers

When you add the logger, you may explicitly set `wrapHandlersPipeline` parameter to specify whether the logger would be
-	_wrapping_ the handlers pipeline (added to the top of the pipeline, corresponding to the messages no. 1, 2, 7 and 8 in the [Old Logging Overview](#old-logging-overview) section above)

```fsharp
  Request -->
*   [LogRequestStart()]                // wrapHandlersPipeline=TRUE
      --> Additional Handlers #1..N -->    // handlers pipeline
          --> Primary Handler -->
                --------Transport--layer--------
          <-- Primary Handler <--
      <-- Additional Handlers #N..1 <--    // handlers pipeline
*   [LogRequestStop()]                 // wrapHandlersPipeline=TRUE
  Response <--
```
-	or, _not wrapping_ the handlers pipeline (added to the bottom, corresponding to the messages no. 3, 4, 5 and 6 in the [Old Logging Overview](#old-logging-overview) section above).


```fsharp
  Request -->
    --> Additional Handlers #1..N --> // handlers pipeline
*     [LogRequestStart()]             // wrapHandlersPipeline=FALSE
          --> Primary Handler -->
                --------Transport--layer--------
          <-- Primary Handler <--
*     [LogRequestStop()]              // wrapHandlersPipeline=FALSE
    <-- Additional Handlers #N..1 <-- // handlers pipeline
  Response <--
```

By default, loggers are added as _not wrapping_.

The difference between wrapping and not wrapping the pipeline is most prominent in case of a retrying handler being added to the pipeline (e.g. Polly or some custom implementation of retries). In that case, a wrapping logger (on the top) would log a message about a single successful request, and the elapsed time logged would be the total time from the user initiating the request to them receiving a response. A non-wrapping logger (on the bottom) would log each of the retry iterations, with first ones potentially logging an exception or an unsuccessful status code, and the last one logging the success. The elapsed time in each case would be time spent purely within the primary handler (the one actually sending the request on the wire, e.g. `HttpClientHandler`).

This can be illustrated with the following diagrams:

- _Wrapping_ case (`wrapHandlersPipeline=TRUE`)

```fsharp
  Request -->
*   [LogRequestStart()]
        --> Additional Handlers #1..(N-1) -->
            --> Retry Handler -->
              --> //1
                  --> Primary Handler -->
                  <-- "503 Service Unavailable" <--
              --> //2
                  --> Primary Handler ->
                  <-- "503 Service Unavailable" <--
              --> //3
                  --> Primary Handler -->
                  <-- "200 OK" <--
            <-- Retry Handler <--
        <-- Additional Handlers #(N-1)..1 <--
*   [LogRequestStop()]
  Response <--
```

```txt
info: Example.CustomLogger.Wrapping[1]
      GET https://consoto.com/
info: Example.CustomLogger.Wrapping[2]
      200 OK - 809.2135ms
```

- _Not wrapping_ case (`wrapHandlersPipeline=FALSE`)

```fsharp
  Request -->
    --> Additional Handlers #1..(N-1) -->
        --> Retry Handler -->
          --> //1
*           [LogRequestStart()]
                --> Primary Handler -->
                <-- "503 Service Unavailable" <--
*           [LogRequestStop()]
          --> //2
*           [LogRequestStart()]
                --> Primary Handler -->
                <-- "503 Service Unavailable" <--
*           [LogRequestStop()]
          --> //3
*           [LogRequestStart()]
                --> Primary Handler -->
                <-- "200 OK" <--
*           [LogRequestStop()]
        <-- Retry Handler <--
    <-- Additional Handlers #(N-1)..1 <--
  Response <--
```

```txt
info: Example.CustomLogger.NotWrapping[1]
      GET https://consoto.com/
info: Example.CustomLogger.NotWrapping[2]
      503 Service Unavailable - 98.613ms
info: Example.CustomLogger.NotWrapping[1]
      GET https://consoto.com/
info: Example.CustomLogger.NotWrapping[2]
      503 Service Unavailable - 96.1932ms
info: Example.CustomLogger.NotWrapping[1]
      GET https://consoto.com/
info: Example.CustomLogger.NotWrapping[2]
      200 OK - 579.2133ms
```

### Simplified SocketsHttpHandler Configuration

.NET 8 adds more convenient and fluent way to use `SocketsHttpHandler` as a primary handler in `HttpClientFactory` ([dotnet/runtime#84075](https://github.com/dotnet/runtime/issues/84075)).

You can set and configure `SocketsHttpHandler` with the [`UseSocketsHttpHandler`](https://learn.microsoft.com/dotnet/api/microsoft.extensions.dependencyinjection.httpclientbuilderextensions.usesocketshttphandler) method. You can use `IConfiguration` to set `SocketsHttpHandler` properties from a config file, or you can configure it from the code, or you can combine both of the approaches.

Note that, when applying `IConfiguration` to the `SocketsHttpHandler`, only properties of SocketsHttpHandler of type `bool`, `int`, `Enum`, or `TimeSpan` are parsed. All unmatched properties in `IConfiguration` are _ignored_. Configuration is parsed only once upon registration and is _not_ reloaded, so the handler will not reflect any config file changes until the application is restarted.

```csharp
// sets up properties on the handler directly
services.AddHttpClient("foo")
    .UseSocketsHttpHandler((h, _) => h.UseCookies = false);

// uses a builder to combine approaches
services.AddHttpClient("bar")
    .UseSocketsHttpHandler(b =>
        b.Configure(config.GetSection($"HttpClient:bar")) // loads simple properties from config
         .Configure((h, _) => // sets up SslOptions in code
         {
            h.SslOptions.RemoteCertificateValidationCallback = delegate { return true; };
         });
    );
```

```json
{
  "HttpClient": {
    "bar": {
      "AllowAutoRedirect": true,
      "UseCookies": false,
      "ConnectTimeout": "00:00:05"
    }
  }
}
```

## QUIC

### OpenSSL 3 Support

Most of the current Linux distributions adopted OpenSSL 3 in their recent releases:
- Debian 12+: [Bookworm OpenSSL](https://packages.debian.org/bookworm/openssl)
- Ubuntu 22+: [Jammy OpenSSL](https://packages.ubuntu.com/jammy/openssl)
- Fedora 37+: [Fedora OpenSSL](https://packages.fedoraproject.org/pkgs/openssl/openssl/)
- OpenSUSE: [Tumbleweed OpenSSL](https://software.opensuse.org/package/openssl)
- AlmaLinux 9+: [AlmaLinux 9 package repository](http://repo.almalinux.org/almalinux/9/BaseOS/x86_64/os/Packages/)

.NET 8's QUIC support is ready for that ([dotnet/runtime#81801](https://github.com/dotnet/runtime/issues/81801)).

The first step to achieve that was to make sure that [MsQuic](https://github.com/microsoft/msquic), the QUIC implementation used underneath `System.Net.Quic`, can work with OpenSSL 3+. This work happened in MsQuic repository [microsoft/msquic#2039](https://github.com/microsoft/msquic/issues/2039). The next step was to make sure that `libmsquic` package is built and published with a corresponding dependency on the default OpenSSL version for the particular distribution and version. For example Debian distribution:
- Debian 11 [`libmsquic`](https://packages.microsoft.com/debian/11/prod/pool/main/libm/libmsquic/) depends on OpenSSL 1.1
- Debian 12 [`libmsquic`](https://packages.microsoft.com/debian/12/prod/pool/main/libm/libmsquic/) depends on OpenSSL 3

The last step was to make sure that the right versions of MsQuic and OpenSSL are being tested and that the tests have coverage across the breadth of .NET supported distributions.

### Exceptions

After publishing QUIC APIs in .NET 7 (as [preview feature](https://github.com/dotnet/designs/blob/main/accepted/2021/preview-features/preview-features.md)), we received several issues about exceptions:
- [dotnet/runtime#78751](https://github.com/dotnet/runtime/issues/78751): `QuicConnection.ConnectAsync` raises `SocketException` when host not found
- [dotnet/runtime#78096](https://github.com/dotnet/runtime/issues/78096): QuicListener AcceptConnectionAsync and OperationCanceledException
- [dotnet/runtime#75115](https://github.com/dotnet/runtime/issues/75115): QuicListener.AcceptConnectionAsync rethrowing exceptions

In .NET 8, `System.Net.Quic` exceptions behavior was completely revised in [dotnet/runtime#82262](https://github.com/dotnet/runtime/issues/82262) and the above mentioned issues were addressed.

One of the main goals of the revision was to make sure that the exceptions behavior in `System.Net.Quic` is as consistent as possible across the whole namespace. In general, the current behavior can be summarized as follows:
- `QuicException`: all errors specific to the QUIC protocol or related to its processing.
  - Connection closed either locally or by the peer.
  - Connection idled out from inactivity.
  - Stream aborted either locally or by the peer.
  - Other errors described in [`QuicError`](https://learn.microsoft.com/dotnet/api/system.net.quic.quicerror?view=net-8.0)
- `SocketException`: for network problem such as network conditions, name resolution or user errors.
  - Address is already in use.
  - Target host cannot be reached.
  - Specified address is invalid.
  - Host name cannot be resolved.
- `AuthenticationException`: for all TLS related issues. The goal is to have similar behavior as [`SslStream`](https://learn.microsoft.com/dotnet/api/system.net.security.sslstream?view=net-8.0).
  - Certificate related errors.
  - ALPN negotiation errors.
  - User cancellation during handshake.
- `ArgumentException`: when supplied [`QuicConnectionOptions`](https://learn.microsoft.com/dotnet/api/system.net.quic.quicconnectionoptions?view=net-8.0) or [`QuicListenerOptions`](https://learn.microsoft.com/dotnet/api/system.net.quic.quiclisteneroptions?view=net-8.0) are invalid.
  - Provided stream limits as not within the range of 0-65535.
  - Omitting mandatory properties like: [`DefaultCloseErrorCode`](https://learn.microsoft.com/dotnet/api/system.net.quic.quicconnectionoptions.defaultcloseerrorcode?view=net-8.0#system-net-quic-quicconnectionoptions-defaultcloseerrorcode) or [`DefaultStreamErrorCode`](https://learn.microsoft.com/dotnet/api/system.net.quic.quicconnectionoptions.defaultstreamerrorcode?view=net-8.0#system-net-quic-quicconnectionoptions-defaultstreamerrorcode).
  - Not specifying [`ClientAuthenticationOptions`](https://learn.microsoft.com/dotnet/api/system.net.quic.quicclientconnectionoptions.clientauthenticationoptions?view=net-8.0) or [`ServerAuthenticationOptions`](https://learn.microsoft.com/dotnet/api/system.net.quic.quicserverconnectionoptions.serverauthenticationoptions?view=net-8.0).
- `OperationCanceledException`: whenever [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken?view=net-8.0) gets fired cancellation.
- `ObjectDisposedException`: whenever calling a method on an already disposed object.

Note that the examples mentioned above are not exhaustive.

Apart from changing the behavior, [`QuicException`](https://learn.microsoft.com/dotnet/api/system.net.quic.quicexception?view=net-8.0) was changed as well. One of those changes was adjusting [`QuicError`](https://learn.microsoft.com/dotnet/api/system.net.quic.quicerror?view=net-8.0) enum values. Items that are now covered by `SocketException` were removed and a new value for user callback errors was added ([dotnet/runtime#87259](
https://github.com/dotnet/runtime/issues/87259)). The newly added `CallbackError` is used to distinguish exceptions thrown by [`QuicListenerOptions.ConnectionOptionsCallback`](https://learn.microsoft.com/dotnet/api/system.net.quic.quiclisteneroptions.connectionoptionscallback?view=net-8.0#system-net-quic-quiclisteneroptions-connectionoptionscallback) from `System.Net.Quic` originating ones ([dotnet/runtime#88614](https://github.com/dotnet/runtime/pull/88614)). So, if the user code throws for example `ArgumentException`, [`QuicListener.AcceptConnectionAsync`](https://learn.microsoft.com/dotnet/api/system.net.quic.quiclistener.acceptconnectionasync?view=net-8.0) will wrap it in [`QuicException`](https://learn.microsoft.com/dotnet/api/system.net.quic.quicexception?view=net-8.0) with [`QuicError`](https://learn.microsoft.com/dotnet/api/system.net.quic.quicexception.quicerror?view=net-8.0) set to `CallbackError` and the inner exception will contain the original user-thrown one. It can be used like this:

```csharp
await using var listener = await QuicListener.ListenAsync(new QuicListenerOptions
{
    // ...
    ConnectionOptionsCallback = (con, hello, token) =>
    {
        if (blockedServers.Contains(hello.ServerName))
        {
            throw new ArgumentException($"Connection attempt from forbidden server: '{hello.ServerName}'.", nameof(hello));
        }

        return ValueTask.FromResult(new QuicServerConnectionOptions
        {
            // ...
        });
    },
});
// ...
try
{
    await listener.AcceptConnectionAsync();
}
catch (QuicException ex) when (ex.QuicError == QuicError.CallbackError && ex.InnerException is ArgumentException)
{
    Console.WriteLine($"Blocked connection attempt from forbidden server: {ex.InnerException.Message}");
}

```

The last change in exception space was adding transport error code into `QuicException` ([dotnet/runtime#88550](https://github.com/dotnet/runtime/pull/88550)). Transport error codes are defined by [RFC 9000 Transport Error Codes](https://www.rfc-editor.org/rfc/rfc9000.html#name-transport-error-codes) and they were already available to `System.Net.Quic` from [MsQuic](https://github.com/microsoft/msquic), they just weren't exposed publicly. So a new nullable property was added to `QuicException`: [`TransportErrorCode`](https://learn.microsoft.com/dotnet/api/system.net.quic.quicexception.transporterrorcode?view=net-8.0). We'd like to thank a community contributor [AlexRadch](https://github.com/AlexRadch) who implemented this change in [dotnet/runtime#88614](https://github.com/dotnet/runtime/pull/88550).

## Sockets

The most impactful change done in socket space was significantly lowering allocations for connection-less (UDP) sockets ([dotnet/runtime#30797](https://github.com/dotnet/runtime/issues/30797)). One of the biggest contributors to allocations when working with UDP sockets was allocating a new `EndPoint` object (and supporting allocations like `IPAddress`) on each call of [`Socket.ReceiveFrom`](https://learn.microsoft.com/dotnet/api/system.net.sockets.socket.receivefrom?view=net-8.0). To mitigate this, a set of new APIs working with [`SocketAddress`](https://learn.microsoft.com/dotnet/api/system.net.socketaddress?view=net-8.0) instead ([dotnet/runtime#87397](https://github.com/dotnet/runtime/issues/87397)) was introduced. `SocketAddress` internally holds the IP address as an array of bytes in the platform dependent form so that it can be passed to the operating system calls directly. Therefore, no copies of the IP address data need to be done before calling native socket functions.

Moreover, the newly added [`ReceiveFrom`](https://learn.microsoft.com/dotnet/api/system.net.sockets.socket.receivefrom?view=net-8.0#system-net-sockets-socket-receivefrom(system-span((system-byte))-system-net-sockets-socketflags-system-net-socketaddress)) and [`ReceiveFromAsync`](https://learn.microsoft.com/dotnet/api/system.net.sockets.socket.receivefromasync?view=net-8.0#system-net-sockets-socket-receivefromasync(system-memory((system-byte))-system-net-sockets-socketflags-system-net-socketaddress-system-threading-cancellationtoken)) overloads do not instantiate a new `IPEndPoint` on each call but rather mutate the provided `receivedAddress` parameter in place. All this together can be used to make UDP socket code more efficient:

```csharp
// Same initialization code as before, no change here.
Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
byte[] message = Encoding.UTF8.GetBytes("Hello world!");
byte[] buffer = new byte[1024];
IPEndPoint endpoint = new IPEndPoint(IPAddress.Loopback, 12345);
server.Bind(endpoint);

// --------
// Original code that would allocate IPEndPoint for each ReceiveFromAsync:
Task<SocketReceiveFromResult> receiveTaskOrig = server.ReceiveFromAsync(buffer, SocketFlags.None, endpoint);
await client.SendToAsync(message, SocketFlags.None, endpoint);
SocketReceiveFromResult resultOrig = await receiveTaskOrig;

Console.WriteLine(Encoding.UTF8.GetString(buffer, 0, result.ReceivedBytes) + " from " + result.RemoteEndPoint);
// Prints:
// Hello world! from 127.0.0.1:59769

// --------
// New variables that can be re-used for subsequent calls:
SocketAddress receivedAddress = endpoint.Serialize();
SocketAddress targetAddress = endpoint.Serialize();

// New code that will mutate provided SocketAddress for each ReceiveFromAsync:
ValueTask<int> receiveTaskNew = server.ReceiveFromAsync(buffer, SocketFlags.None, receivedAddress);
await client.SendToAsync(message, SocketFlags.None, targetAddress);
var length = await receiveTaskNew;

Console.WriteLine(Encoding.UTF8.GetString(buffer, 0, length) + " from " + receivedAddress);
// Prints:
// Hello world! from InterNetwork:16:{233,121,127,0,0,1,0,0,0,0,0,0,0,0}
```

On top of that, work with `SocketAddress` was improved in [dotnet/runtime#86872](https://github.com/dotnet/runtime/issues/86872). `SocketAddress` now has several additional members that make it more useful on its own:
- getter [`Buffer`](https://learn.microsoft.com/dotnet/api/system.net.socketaddress.buffer?view=net-8.0): to access the whole underlying address buffer.
- setter [`Size`](https://learn.microsoft.com/dotnet/api/system.net.socketaddress.size?view=net-8.0): to be able to adjust the above mentioned buffer size (only to a smaller size).
- static [`GetMaximumAddressSize`](https://learn.microsoft.com/dotnet/api/system.net.socketaddress.getmaximumaddresssize?view=net-8.0): to get the necessary buffer size based on the address type.
- interface [`IEquatable<SocketAddress>`](https://learn.microsoft.com/dotnet/api/system.net.socketaddress.equals?view=net-8.0#system-net-socketaddress-equals(system-net-socketaddress)): `SocketAddress` can be used to differentiate peers with whom the socket communicates, for example as a key in a dictionary (this is not a new functionality, it just makes it callable via the interface).

And lastly, some of the internally made copies of the IP address data were removed to make it more performant.

## Networking Primitives

### MIME Types

Adding missing MIME types was one of the most upvoted issues in the networking space ([dotnet/runtime#1489](https://github.com/dotnet/runtime/issues/1489)). This was a mostly community-driven change that lead to the [dotnet/runtime#85807](https://github.com/dotnet/runtime/issues/85807) API proposal. As this addition needed to go through the API review process, it was necessary to make sure that the added types are relevant and follow the specification ([IANA Media Types](https://www.iana.org/assignments/media-types/media-types.xhtml)). For this preparatory work, we'd like to thank community contributors [Bilal-io](https://github.com/Bilal-io) and [mmarinchenko](https://github.com/mmarinchenko).

### IPNetwork

Another new API addition in .NET 8 is the new type `IPNetwork` ([dotnet/runtime#79946](https://github.com/dotnet/runtime/issues/79946)). The struct allows specifying classless IP sub-networks as defined in [RFC 4632](https://datatracker.ietf.org/doc/html/rfc4632). For example:
- `127.0.0.0/8` for a class-less definition corresponding to a class A subnet.
- `42.42.128.0/17` for a class-less subnet of 2<sup>15</sup> addresses.
- `2a01:110:8012::/100` for IPv6 subnet of 2<sup>28</sup> addresses.

The new API offers construction either from an `IPAddress` and prefix length with the [constructor](https://learn.microsoft.com/dotnet/api/system.net.ipnetwork.-ctor?view=net-8.0#system-net-ipnetwork-ctor(system-net-ipaddress-system-int32)) or by parsing from string via [`TryParse`](https://learn.microsoft.com/dotnet/api/system.net.ipnetwork.tryparse?view=net-8.0) or [`Parse`](https://learn.microsoft.com/dotnet/api/system.net.ipnetwork.parse?view=net-8.0). On top of that, it allows checking if an `IPAddress` belongs to the subnet with [`Contains`](https://learn.microsoft.com/dotnet/api/system.net.ipnetwork.contains?view=net-8.0) method. Sample usage can look like this:

```csharp
// IPv4 with manual construction.
IPNetwork ipNet = new IPNetwork(new IPAddress(new byte[] { 127, 0, 0, 0 }), 8);
IPAddress ip1 = new IPAddress(new byte[] { 255, 0, 0, 1 });
IPAddress ip2 = new IPAddress(new byte[] { 127, 0, 0, 10 });
Console.WriteLine($"{ip1} {(ipNet.Contains(ip1) ? "belongs" : "doesn't belong")} to {ipNet}");
Console.WriteLine($"{ip2} {(ipNet.Contains(ip2) ? "belongs" : "doesn't belong")} to {ipNet}");
// Prints:
// 255.0.0.1 doesn't belong to 127.0.0.0/8
// 127.0.0.10 belongs to 127.0.0.0/8

// IPv6 with parsing.
IPNetwork ipNet = IPNetwork.Parse("2a01:110:8012::/96");
IPAddress ip1 = IPAddress.Parse("2a01:110:8012::1742:4244");
IPAddress ip2 = IPAddress.Parse("2a01:110:8012:1010:914e:2451:16ff:ffff");
Console.WriteLine($"{ip1} {(ipNet.Contains(ip1) ? "belongs" : "doesn't belong")} to {ipNet}");
Console.WriteLine($"{ip2} {(ipNet.Contains(ip2) ? "belongs" : "doesn't belong")} to {ipNet}");
// Prints:
// 2a01:110:8012::1742:4244 belongs to 2a01:110:8012::/96
// 2a01:110:8012:1010:914e:2451:16ff:ffff doesn't belong to 2a01:110:8012::/96
```

Note that this type should not be confused with the [`Microsoft.AspNetCore.HttpOverrides.IPNetwork`](https://learn.microsoft.com/dotnet/api/microsoft.aspnetcore.httpoverrides.ipnetwork) class that existed in ASP.NET Core since 1.0. We expect that ASP.NET APIs will eventually migrate to the new `System.Net.IPNetwork` type ([dotnet/aspnetcore#46157](https://github.com/dotnet/aspnetcore/issues/46157)).

## Final Notes

The topics chosen for this blog post are not an exhaustive list of all changes done in .NET 8, just the ones we think might be the most interesting to read about. If you're more interested in performance improvements, you should check out [networking section](https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-8/#networking) in Stephen's huge performance blog post. And if you have any questions or find any bugs, you can reach out to us in [dotnet/runtime](https://github.com/dotnet/runtime/issues) repository.

Lastly, I'd like to thank my co-authors:
- [@antonfirsov](https://github.com/antonfirsov) who wrote [Metrics](#metrics).
- [@CarnaViire](https://github.com/CarnaViire) who wrote [HttpClientFactory](#httpclientfactory).
