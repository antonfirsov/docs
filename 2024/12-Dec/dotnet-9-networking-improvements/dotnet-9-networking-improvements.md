---
post_title: .NET 9 Networking Improvements
author1: mapichov
author2: knatalia
post_slug: dotnet-9-networking-improvements
microsoft_alias: mapichov
featured_image: dotnet-bot.png
categories: .NET, Networking
tags: .net 9, http, http-client-factory, net-security
summary: Introducing new networking features in .NET 9 including HTTP space, HttpClientFactory, security and more!
post_date: 2024-12-05 10:05:00
---

As in the previous years, we publish a blog post about new interesting changes in networking space with new [.NET release](https://devblogs.microsoft.com/dotnet/announcing-dotnet-9/). This year, we're introducing changes in [HTTP](#http) space, new [`HttpClientFactory`](#httpclientfactory) APIs, .NET framework compatibility improvements and more.

## HTTP

### Connection Pooling

In this release, we've made two impactful performance improvements in HTTP connection pooling.

The first one is the support of multiple HTTP/3 connections. Using more than one HTTP/3 connection to the peer is discouraged by the [RFC 9114](https://datatracker.ietf.org/doc/html/rfc9114#section-3.3) since the connection can multiplex multiple parallel requests. However, in certain scenarios like server-to-server, one connection might become a bottleneck even with the multiplexing. We've seen such limitations with HTTP/2 ([dotnet/runtime#35088](https://github.com/dotnet/runtime/issues/35088)), which has the same concept of multiplexing over one connection. For the same reasons ([dotnet/runtime#51775](https://github.com/dotnet/runtime/issues/51775)), we decided to implement multiple connection support for HTTP/3 ([dotnet/runtime#101535](https://github.com/dotnet/runtime/issues/101535)).

The implementation itself tries to closely match the behavior of HTTP/2 multiple connections. Which at the moment always prefer to saturate existing connections with as many requests as allowed by the peer before opening a new one. Note that this is an implementation detail and the behavior might change in the future.

As a result, [our benchmarks](https://github.com/aspnet/Benchmarks/tree/main/src/BenchmarksApps/HttpClientBenchmarks) showed a non-trivial increase in RPS (requests per seconds), comparison for 10,000 parallel requests:

| client                      | single HTTP/3 connection              | multiple HTTP/3 connections           |
| --------------------------- | ------------------------------------- | ------------------------------------- |
| Max CPU Usage (%)           | 35                                    | 92                                    |
| Max Cores usage (%)         | 971                                   | 2,572                                 |
| Max Working Set (MB)        | 3,810                                 | 6,491                                 |
| Max Private Memory (MB)     | 4,415                                 | 7,228                                 |
| Processor Count             | 28                                    | 28                                    |
| First request duration (ms) | 519                                   | 594                                   |
| Requests                    | 345,446                               | 4,325,325                             |
| Mean RPS                    | 23,069                                | 288,664                               |

This feature can be turned on via a property on [`SocketsHttpHandler`](https://learn.microsoft.com/dotnet/api/system.net.http.socketshttphandler.enablemultiplehttp3connections):
```c#
var client = new HttpClient(new SocketsHttpHandler()
{
    EnableMultipleHttp3Connections = true
});
```


The second change is fixing the lock contention in HTTP 1.1 connection pooling ([dotnet/runtime#70098](https://github.com/dotnet/runtime/issues/70098)). The HTTP 1.1 connection pool used a single lock to manage the list of connections and the queue of pending requests. With high throughput scenarios on machines with high number of CPU cores, this lock became a bottleneck. In the fix ([dotnet/runtime#99364](https://github.com/dotnet/runtime/pull/99364)), the ordinary list with a lock was replaced by a concurrent collection instead. Specifically by [`ConcurrentStack`](https://learn.microsoft.com/dotnet/api/system.collections.concurrent.concurrentstack-1) as it preserves the observable behavior when requests are handled by the newest available connection - to be able to collect older connections on their configured [lifetime](https://learn.microsoft.com/dotnet/api/system.net.http.socketshttphandler.pooledconnectionlifetime). As a result, the throughput of HTTP 1.1 requests in our benchmarks increased by more than 30%:
| client             | .net 8.0    | .net 9.0     | increase |
| ------------------ | ----------- | ------------ | -------- |
| Requests           |  80,028,791 |  107,128,778 |  +33.86% |
| Mean RPS           |     666,886 |      892,749 |  +33.87% |


### Proxy Auto Update on Windows

One of the main pain points when debugging HTTP traffic with .NET applications was that the application didn't react to changes in Windows proxy settings ([dotnet/runtime#70098](https://github.com/dotnet/runtime/issues/46910)). The proxy settings was initialized once per process and it wasn't possible to refresh it in any reasonable manner until the application got restarted. Properties like [`HttpClient.DefaultProxy`](https://learn.microsoft.com/dotnet/api/system.net.http.httpclient.defaultproxy) would return the same instance upon repeated access and never re-fetch the settings. As a result, tools like Fiddler, that set themselves as system proxy to listen for the traffic, weren't able to capture traffic from already running processes. This was mitigated in [dotnet/runtime#103364](https://github.com/dotnet/runtime/pull/103364), where the `HttpClient.DefaultProxy` is always initialized to an instance of Windows proxy that listens for registry changes and reloads the proxy settings when notified. Now the following code:
```c#
while (true)
{
    using var resp = await client.GetAsync("https://httpbin.org/");
    Console.WriteLine(HttpClient.DefaultProxy.GetProxy(new Uri("https://httpbin.org/"))?.ToString() ?? "null");
    await Task.Delay(1_000);
}
```
will produce output like this:
```text
null
// After Fiddler's "System Proxy" is turned on.
http://127.0.0.1:8866/
```

Note that this change applies only for Windows as it has a unique concept of [machine wide proxy settings](https://support.microsoft.com/windows/use-a-proxy-server-in-windows-03096c53-0554-4ffe-b6ab-8b1deee8dae1). Whereas Linux and other UNIX based systems allow setting up proxy via environment variables, which cannot be changed during process lifetime.

### Community contributions

In HTTP space, there were two community contributions we'd like to call out. One was an addition of missing [`HttpContent.LoadIntoBufferAsync`](https://learn.microsoft.com/dotnet/api/system.net.http.httpcontent.loadintobufferasync) overloads taking in [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) parameter as is a standard for async methods. The API proposal ([dotnet/runtime#102659](https://github.com/dotnet/runtime/issues/102659)) was prepared by [andrewhickman-aveva](https://github.com/andrewhickman-aveva) and the implementation ([dotnet/runtime#103991](https://github.com/dotnet/runtime/pull/103991)) was done by [manandre](https://github.com/manandre).

The other change tries to alleviate a unit discrepancy in units of `MaxResponseHeadersLength` property on [`SocketsHttpHandler`](https://learn.microsoft.com/dotnet/api/system.net.http.socketshttphandler) and [`HttpClientHandler`](https://learn.microsoft.com/dotnet/api/system.net.http.httpclienthandler) ([dotnet/runtime#75137](https://github.com/dotnet/runtime/issues/75137)). All the other size and length properties are interpreted as being in bytes, this one is interpreted as kilobytes. And since the actual behavior cannot be changed due to backward compatibility, the problem was solved by implementing an analyzer ([dotnet/roslyn-analyzers#6796](https://github.com/dotnet/roslyn-analyzers/pull/6796)). The analyzer tries to make sure the user is aware that the value provided is interpreted as kilobytes and will warn if the usage suggests otherwise. If the value is higher than a certain threshold, it will look like this:
![Analyzer Warning for MaxResponseHeadersLength](analyzer_warning.png "Analyzer Warning for MaxResponseHeadersLength")
The analyzer was implemented by [amiru3f](https://github.com/amiru3f).


// Natalia
## WebSockets
- https://github.com/dotnet/runtime/pull/105841

// Mana
## .NET Framework Compatibility
- HttpWebRequest (https://github.com/dotnet/runtime/pull/95001, https://github.com/dotnet/runtime/pull/94664, https://github.com/dotnet/runtime/pull/97537)
- WebRequest impersonation (https://github.com/dotnet/runtime/pull/102038)
- ServicePoint obsoletion (https://github.com/dotnet/runtime/pull/103456)
- AuthenticationManager obsoletion (https://github.com/dotnet/runtime/pull/93171) community

// Anton
## Metrics / Diagnostics / Redaction
- https://github.com/dotnet/runtime/pull/103922
- ...

// Natalia
## HttpClientFactory

// Mana
## QUIC
- public API surface (https://github.com/dotnet/runtime/pull/104227)
- APIs for multiple connections (https://github.com/dotnet/runtime/pull/101531)
- new connection options (https://github.com/dotnet/runtime/pull/94211)
- connection TLS details (https://github.com/dotnet/runtime/pull/84976, https://github.com/dotnet/runtime/pull/106391)
- perf callback in TP thread (https://github.com/dotnet/runtime/pull/98361)
- perf configuration cache (https://github.com/dotnet/runtime/pull/99371)

## Security

### SSLKEYLOGFILE Support

The most upvoted issue in the security space was to support logging of pre-master secret ([dotnet/runtime#37915](https://github.com/dotnet/runtime/issues/37915)). The logged secret can be used by packet capturing tool Wireshark to decrypt the traffic. It's useful diagnostics tool when investigation networking issues. Moreover, the same functionality is provided by browsers like Firefox (via [NSS](https://udn.realityripple.com/docs/Mozilla/Projects/NSS/Key_Log_Format)) and Chrome and command line HTTP tools like [cURL](https://everything.curl.dev/usingcurl/tls/sslkeylogfile).

We have implemented this feature for both [`SslStream`](https://learn.microsoft.com/dotnet/api/system.net.security.sslstream) and [`QuicConnection`](https://learn.microsoft.com/dotnet/api/system.net.quic.quicconnection0). For the former, the functionality is limited to the platforms on which we use OpenSSL as a security backend (in the terms of officially released .NET runtime it means on Linux). For the later, it is supported everywhere, regardless of the security backend. The difference is that QUIC is implemented in userspace, but TLS on Windows is implemented in a separate, privileged process. This means that SChannel needs to support exporting the encryption secrets to applications in order to make userspace QUIC implementations possible, but will not do the same for TLS due to security concerns ([dotnet/runtime#94843](https://github.com/dotnet/runtime/issues/94843)).

This feature exposes security secrets and relying solely on an environmental variable could unintentionally leak them. For that reason, we've decided to introduce an additional `AppContext` switch necessary to enable the feature ([dotnet/runtime#100665](https://github.com/dotnet/runtime/pull/100665)). It requires the user to prove the ownership of the application by either setting it programmatically in the code:
```c#
AppContext.SetSwitch("System.Net.EnableSslKeyLogging", true);
```
or by changing the `{appname}.runtimeconfig.json` next to the application:
```c#
{
  "runtimeOptions": {
    "configProperties": {
      "System.Net.EnableSslKeyLogging": true
    }
  }
}
```

The last thing is to set up an environmental variable `SSLKEYLOGFILE` and run the application:

```sh
export SSLKEYLOGFILE=~/keylogfile

./<appname>
```

At this point, `~/keylogfile` will contain pre-master secrets that can be used by Wireshark to decrypt the traffic, see [TLS Using the (Pre)-Master-Secret](https://wiki.wireshark.org/TLS#using-the-pre-master-secret) documentation.


### TLS Resume with Client Certificate

TLS resume enables reusing previously stored TLS data to re-establish connection to previously connected server. It can save round-trips during the handshake as well as CPU processing. This feature is a native part of Windows SChannel, therefore it's implicitly used  by .NET on Windows platforms. However, on Linux platforms where we use OpenSSL as a security backend, enabling caching and re-using TLS data is more involved. We firstly introduces the support in .NET 7, [TLS Resume](https://devblogs.microsoft.com/dotnet/dotnet-7-networking-improvements/#performance). It has it's own limitations that in general are not present on Windows. One such limitations was that it was not supported for sessions using mutual authentication by providing a client certificate ([dotnet/runtime#94561](https://github.com/dotnet/runtime/issues/94561)). This has been fixed in .NET 9 ([dotnet/runtime#102656](https://github.com/dotnet/runtime/pull/102656)) and works if one these conditions is:
- [`ClientCertificateContext`](https://learn.microsoft.com/dotnet/api/system.net.security.sslclientauthenticationoptions.clientcertificatecontext)
- [`LocalCertificateSelectionCallback`](https://learn.microsoft.com/dotnet/api/system.net.security.sslclientauthenticationoptions.localcertificateselectioncallback) returns non-null certificate on the first call
- [`ClientCertificates`](https://learn.microsoft.com/dotnet/api/system.net.security.sslclientauthenticationoptions.clientcertificates0) collection has at least one certificate with private key

### Negotiate API Integrity Checks

In .NET 7, we added [`NegotiateAuthentication`](https://learn.microsoft.com/dotnet/api/system.net.security.negotiateauthentication) APIs, [Negotiate API](https://devblogs.microsoft.com/dotnet/dotnet-7-networking-improvements/#negotiate-api). The original implementation's goal was to remove access via reflection to the internals of `NTAuthentication`. However, that proposal was missing functions to generate and verify message integrity codes from [RFC 2743](https://datatracker.ietf.org/doc/html/rfc2743). They are usually implemented as cryptographic signing operation with a negotiated key. The API was proposed in [dotnet/runtime#86950](https://github.com/dotnet/runtime/issues/86950) and implemented in [dotnet/runtime#96712](https://github.com/dotnet/runtime/pull/96712) and ss with the original change, all the work from the API proposal to the implementation was done by a community contributor [filipnavara](https://github.com/filipnavara).

// Mana
## Networking Primitives
- IEquatable on Uri (https://github.com/dotnet/runtime/pull/103511)
- More MediaTypeNames (https://github.com/dotnet/runtime/pull/103575)
- SSE parser as OOB (https://github.com/dotnet/runtime/pull/102238)
- Uri’s got span-based zero-alloc Escape/UnescapeDataString APIs (https://github.com/dotnet/runtime/pull/98074)
- new media types (https://github.com/dotnet/runtime/pull/103575)
