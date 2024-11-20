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

// Mana
## Security

### SSLKEYLOGFILE Support
// Needs update: written last year, no need for Debug build, but we have AppContext switch instead, also works with QUIC.

The most upvoted issue in the security space was to support logging of pre-master secret ([dotnet/runtime#37915](https://github.com/dotnet/runtime/issues/37915)). The logged secret can be used by packet capturing tool Wireshark to decrypt the traffic. It's useful diagnostics tool when investigation networking issues. Moreover, the same functionality is provided by browsers like Firefox (via [NSS](https://udn.realityripple.com/docs/Mozilla/Projects/NSS/Key_Log_Format)) and Chrome and command line HTTP tools like [cURL](https://everything.curl.dev/usingcurl/tls/sslkeylogfile).

For now, we have implemented this feature for platforms on which we use OpenSSL as a security backend (in terms of officially released .NET runtime it means Linux). We track this feature for Windows in [dotnet/runtime#94843](https://github.com/dotnet/runtime/issues/94843) and we encourage upvoting the issue to let us know we should invest in doing the same for SChannel (Windows).

As this is rather advanced scenario with security implications, we decided to support it only in DEBUG builds of `System.Net.Security`. We do not officially ship DEBUG builds of .NET libraries anywhere. If you want to take advantage of this feature, you have to build it yourself. The documentation how to do that is in our [repository](https://github.com/dotnet/runtime/blob/main/docs/workflow/README.md). Note that corresponding branch of source code needs to be used, meaning [release/8.0](https://github.com/dotnet/runtime/tree/release/8.0) for .NET 8.

When you have the DEBUG build ready, you can publish your project as self contained:

```sh
dotnet publish --runtime linux-x64 --self-contained
```

Then, replace `System.Net.Security.dll` in the publish directory with the one you've built:

```sh
cp <runtime-repo>/artifacts/bin/System.Net.Security/Debug/net8.0-linux/* <project-repo>/bin/Release/net8.0/linux-x64/publish/

```

The last thing is to set up an environmental variable `SSLKEYLOGFILE` and run your program:

```sh
export SSLKEYLOGFILE=~/keylogfile

./<your-program>
```

At this point, `~/keylogfile` will contain pre-master secrets that can be used by Wireshark to decrypt the traffic, see [TLS Using the (Pre)-Master-Secret](https://wiki.wireshark.org/TLS#using-the-pre-master-secret) docs about how to configure it.

https://github.com/dotnet/runtime/pull/100665

- TLS Resume on Linux (https://github.com/dotnet/runtime/pull/102656)
- IntegrityCheck APIs (https://github.com/dotnet/runtime/pull/96712) - Filip community

// Mana
## Networking Primitives
- IEquatable on Uri (https://github.com/dotnet/runtime/pull/103511)
- More MediaTypeNames (https://github.com/dotnet/runtime/pull/103575)
- SSE parser as OOB (https://github.com/dotnet/runtime/pull/102238)
- Uri’s got span-based zero-alloc Escape/UnescapeDataString APIs (https://github.com/dotnet/runtime/pull/98074)
- new media types (https://github.com/dotnet/runtime/pull/103575)
