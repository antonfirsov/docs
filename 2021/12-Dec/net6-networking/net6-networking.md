---
post_title: '.NET 6 Networking Improvements'
username: mapichov
microsoft_alias: mapichov
categories: .NET, .NET Core, Networking
featured_image: dotnet-bot.png
summary: Introducing new networking features in .NET 6.
desired_publication_date: '2021-12-13'
---

With each new release of .NET we like to publish a blog post highlighting some of the changes and improvements for  networking. In this post, I am pleased to talk about the changes in **.NET 6**.

The previous version of this post is [.NET 5 networking improvements](https://devblogs.microsoft.com/dotnet/net-5-new-networking-improvements/).

## HTTP
### HTTP/2 Window Scaling
With the emerging popularity of HTTP/2 and gRPC, our customers discovered that `SocketsHttpHandler`'s HTTP/2 download speed wasn't on par with other implementations when connected to geographically distant servers with significant network delay. On links with a high [bandwidth-delay product](https://en.wikipedia.org/wiki/Bandwidth-delay_product), some users reported 5x-10x differences compared to other implementations which were able to utilize the link's physical bandwidth. To give an example: in one of our benchmarks curl was able to reach the maximum 10 Mbit/s rate of a specific cross-atlantic link, while `SocketsHttpHanlder` speed topped at 2.5 Mbit/s. Amongst other things, this was heavily impacting gRPC streaming scenarios.

The root cause of the issue turned out to be the fixed-size HTTP/2 receive window, with it's 64KB size being too small to keep the network busy when `WINDOW_UPDATE` frames are received with a high delay, meaning that HTTP/2's own flow control mechanism was stalling the network link.

We considered "cheap" options to solve this, such as defining a large fixed-size window - which could result in unnecessarily high memory footprint -, or to ask the user to manually configure the receive window based on empirical observations. None of these seemed to be satisfactory, so we decided to implement an automatic window sizing algorithm similar to the one in TCP or QUIC ([dotnet/runtime#54755](https://github.com/dotnet/runtime/pull/54755)).

This turned out to work well, lifting download speeds close to their theoretical maximum. However, since HTTP/2 PING frames are used to determine the round-trip time of the HTTP/2 connection, we had to be very careful to avoid triggering the server's [PING flood](https://cve.mitre.org/cgi-bin/cvename.cgi?name=CVE-2019-9512) protection mechanisms. We implemented an algorithm that should work well with [gRPC](https://github.com/grpc/proposal/blob/master/A8-client-side-keepalive.md#server-enforcement) and existing HTTP servers, but we wanted to make sure we have an escape path in case something goes wrong. Dynamic window sizing - and consequentially PING frames - can be turned off by setting the `System.Net.SocketsHttpHandler.Http2FlowControl.DisableDynamicWindowSizing` `AppContext` switch to `true`. If this ever becomes necessary, there's still a way to fix throughput issues by assigning a higher value to [`SocketsHttpHandler.InitialHttp2StreamWindowSize`](https://docs.microsoft.com/dotnet/api/system.net.http.socketshttphandler.initialhttp2streamwindowsize).

### HTTP/3 and QUIC
In .NET 5, we released an experimental implementation of [QUIC and HTTP/3](https://devblogs.microsoft.com/dotnet/net-5-new-networking-improvements/#http-3). It was limited only to Insider builds of Windows and there was quite a bit of ceremony to get it working.

In .NET 6, we have significantly simplified the setup.

- On Windows, we ship [MsQuic](https://github.com/microsoft/msquic) library as part of the runtime, so there's no need to download or reference anything external. The only limitation is that Windows 11 or Windows Server 2022 is required. This is due to TLS 1.3 support for QUIC in SChannel that is not available in earlier Windows versions.
- On Linux, we publish MsQuic as a standard Linux package `libmsquic` (deb and rpm) in [Microsoft Package Repository](https://docs.microsoft.com/windows-server/administration/linux-package-repository-for-microsoft-software). The reason for not bundling MsQuic with runtime on Linux is that we ship `libmsquic` with [QuicTLS](https://github.com/quictls/openssl), a fork of OpenSSL providing necessary TLS APIs. And since we bundle QuicTLS with MsQuic, we need to be able to do security patches outside of the normal .NET release schedule.

We have also greatly improved stability and implemented a lot of missing features with around [90 issues closed](https://github.com/dotnet/runtime/issues?q=is%3Aissue+project%3Adotnet%2Fruntime%2F2+is%3Aclosed+milestone%3A6.0.0+) in .NET 6 milestone.

HTTP/3 uses QUIC rather than TCP as its transport layer. Our .NET implementation of QUIC protocol is a managed layer built on top of MsQuic, in the `System.Net.Quic` library. QUIC is a general purpose protocol, that can be used for multiple scenarios, not just HTTP/3, but is new and only recently ratified in [RFC 9000](https://www.rfc-editor.org/rfc/rfc9000.html).
We didn't have enough confidence that the current shape of API would stand the test of time, and be suitable for use by other protocols, so we decided to keep it private in this release. As a result, .NET 6 contains the QUIC protocol implementation, but doesn't expose it. It's only used internally for HTTP/3 in `HttpClient` and in Kestrel server.

Despite putting a lot of effort in bug squishing in this release, we still don't think the HTTP/3 quality is fully production ready. And since any HTTP request could inadvertently be upgraded to HTTP/3 via [`Alt-Svc` header](https://www.ietf.org/archive/id/draft-ietf-quic-http-34.html#name-http-alternative-services) and start failing, we've opted to keep HTTP/3 capability disabled by default in this release. In `HttpClient`, it's hidden behind `System.Net.SocketsHttpHandler.Http3Support` `AppContext` switch.


All the details how to set everything up have already been described in our previous articles: [`HttpClient`](https://devblogs.microsoft.com/dotnet/http-3-support-in-dotnet-6/) and [Kestrel](https://docs.microsoft.com/aspnet/core/fundamentals/servers/kestrel/http3?view=aspnetcore-6.0). On Linux, acquire `libmsquic` package, on Windows, make sure the OS version is at least 10.0.20145.1000. Then, you only need to enable HTTP/3 support and set the `HttpClient` to use HTTP/3:
```C#
using System.Net;

// Set this switch programmatically or in csproj:
// <RuntimeHostConfigurationOption Include="System.Net.SocketsHttpHandler.Http3Support" Value="true" />
AppContext.SetSwitch("System.Net.SocketsHttpHandler.Http3Support", true);

// Set up the client to request HTTP/3.
var client = new HttpClient()
{
    DefaultRequestVersion = HttpVersion.Version30,
    DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrHigher,
};
var resp = await client.GetAsync("https://<http3 endpoint>");

// Print the response version.
Console.WriteLine($"status: {resp.StatusCode}, version: {resp.Version}");
```

We encourage you to try HTTP/3 out! And in case you encounter any problem, please file an issue in [dotnet/runtime](https://github.com/dotnet/runtime/issues).

### HTTP Retry Logic
.NET 6 changes the HTTP request retry logic to be based on a fixed retry count limit (see [dotnet/runtime#48758](https://github.com/dotnet/runtime/pull/48758)).

Previously, .NET 5 disallowed request retries on connection failure when the failure occurred on a "new" connection (one that hadn't been used for previous requests). We did this mainly to ensure that the retry logic doesn't end up in an infinite loop. This was suboptimal and specifically problematic for HTTP/2 connections (see [dotnet/runtime#44669](https://github.com/dotnet/runtime/issues/44669)). On the other hand, .NET 5 was too lenient about allowing retries in many cases, which wasn't fully compliant with [RFC 2616](https://datatracker.ietf.org/doc/html/rfc2616#section-8.1.4). For example, we were retrying on arbitrary exceptions, e.g. on IO timeouts, even though the user was explicitly setting this timeout and presumably wanted to fail (not retry) the request when the timeout was exceeded.

.NET 6 retry logic will work regardless of the request being the first one on a connection. It introduces the retry limit which is currently set to 5. In future, we might consider adjusting it or making it configurable, if desired.

To comply better with the RFC, requests are now only retriable when we believe that the server is attempting to gracefully tear down a connection -- that is, when we receive EOF before any other response data for HTTP/1.1, or receive GOAWAY for HTTP/2.

The downside to more conservative retry behavior of .NET 6 is that the failures previously masked by lenient retry policy will start to be visible to users. For example, if a server tears down idle connections in a non-graceful way (by sending TCP RST packet), the requests, that are failed because of RST, will not be automatically retried. This was briefly touched upon in the [AAD article about migration to .NET 6](https://devblogs.microsoft.com/dotnet/azure-active-directorys-gateway-is-on-net-6-0/#learnings). The workaround would be to set client's idle timeout ([`SocketsHttpHandler.PooledConnectionIdleTimeout`](https://docs.microsoft.com/dotnet/api/system.net.http.socketshttphandler.pooledconnectionidletimeout?view=net-6.0)) to 50-75% of server's idle timeout (if it's known). That way a request should never get caught in the race with the server closing the connection as idle -- `HttpClient` will scavenge it sooner. Another approach would be to implement a custom retry policy outside of  `HttpClient`. That would also allow to tweak the retry policy and heuristics, for example, if some generally non-idempotent requests could be retried depending on a specific server's logic and implementation.

### SOCKS Proxy Support
SOCKS proxy support has been a long standing issue ([dotnet/runtime#17740](https://github.com/dotnet/runtime/issues/17740)) that has finally been implemented by a community contributor [@huoyaoyuan](https://github.com/huoyaoyuan). We have written about this addition in [.NET 6 Preview 5 blog post](https://devblogs.microsoft.com/dotnet/announcing-net-6-preview-5/#libraries-socks-proxy-support). The change added support for SOCKS4, SOCKS4a and SOCKS5 proxies.

SOCKS proxy is a very versatile tool. For example, it can provide a similar functionality to VPN. Most notably SOCKS proxy is used to access [Tor](https://www.torproject.org/) network.

To configure `HttpClient` to use SOCKS proxy, you only need to use the `socks` scheme when defining the proxy[^socks]:
```C#
var client = new HttpClient(new SocketsHttpHandler()
{
    // Specify the whole Uri (schema, host and port) as one string or use Uri directly.
    Proxy = new WebProxy("socks5://127.0.0.1:9050")
});

var content = await client.GetStringAsync("https://check.torproject.org/");
Console.WriteLine(content);
```
This example assumes you're running `tor` instance on your computer. If the request succeeds, you should be able to find "**Congratulations. This browser is configured to use Tor.**" in the response content.

[^socks]: In the original blog post, we've made a mistake and used a wrong overload of [`WebProxy` constructor](https://docs.microsoft.com/dotnet/api/system.net.webproxy.-ctor?view=net-6.0#System_Net_WebProxy__ctor_System_String_System_Int32_). It expects only host name in the first argument and cannot be used with any other proxy type than HTTP. We've also fixed this particular constructor behavior inconsistency for .NET 7 ([dotnet/runtime#62338](https://github.com/dotnet/runtime/pull/62338)).

### WinHTTP
`WinHttpHandler` is a wrapper over WinHTTP, therefore the feature set is gated on the functionality in WinHTTP. In this release, there are a couple of additions that expose or enable WinHttp features for HTTP/2. They are parts of a larger effort ([dotnet/core#5713](https://github.com/dotnet/core/issues/5713)) to enable users to use [gRPC .NET](https://github.com/grpc/grpc-dotnet) on .NET Framework. The goal is to enable a smoother transition from WCF to gRPC on .NET Framework and then to gRPC on .NET Core / .NET 5+.

- Trailing headers ([dotnet/runtime#44778](https://github.com/dotnet/runtime/issues/44778)).
  - For .NET Core 3.1 / .NET 5 and higher, trailing headers are exposed in [`HttpResponseMessage.TrailingHeaders`](https://docs.microsoft.com/dotnet/api/system.net.http.httpresponsemessage.trailingheaders?view=net-6.0).
  - For .NET Framework, they are exposed in `HttpRequestMessage.Properties["__ResponseTrailers"]` since there is no such property as `TrailingHeaders` on .NET Framework.

- Bidirectional streaming ([dotnet/runtime#44784](https://github.com/dotnet/runtime/issues/44784)). This change is completely seamless and `WinHttpHandler` will automatically allow bidirectional streaming when appropriate, i.e. when request content doesn't have a known length and when underlying WinHTTP supports it.

- [TCP keep-alive](https://datatracker.ietf.org/doc/html/rfc1122#section-4.2.3.6)  configuration. TCP keep-alive is used to keep an idle connection open and to prevent nodes in the middle, like proxies and firewalls, from dropping the connection sooner than the client expects. In .NET 6, we have added 3 new properties to `WinHttpHandler` to configure it:
```C#
public class WinHttpHandler
{
    // Controls whether TCP keep-alive is getting send or not.
    public bool TcpKeepAliveEnabled { get; set; }
    // Delay to the first keep-alive packet during inactivity.
    public TimeSpan TcpKeepAliveTime { get; set; }
    // Interval for subsequent keep-alive packets during inactivity.
    public TimeSpan TcpKeepAliveInterval { get; set; }
}
```
  These properties correspond to WinHTTP [`tcp_keepalive`](https://docs.microsoft.com/windows/win32/winsock/sio-keepalive-vals) structure.

- Use of TLS 1.3 with `WinHttpHandler` ([dotnet/runtime#58590](https://github.com/dotnet/runtime/pull/58590)). This feature is transparent to the user, the only thing needed is Windows support.

### Other HTTP Changes
Many of the HTTP changes in .NET 6 have already been talked about in Stephen Toub's extensive article about [performance](https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-6/#networking), but there are few of them that are worth repeating.

- Refactored connection pooling in `SocketsHttpHandler` ([runtime/dotnet#44818](https://github.com/dotnet/runtime/issues/44818)). The new approach allows us to always process requests on whichever connection that becomes available first, whether that's a newly established one or one that became ready to handle the request in the meantime. While previously, in the case all connections where busy when the request came, we'd start opening a new connection and keep the request waiting for it. This change applies to HTTP/1.1 as well as HTTP/2 with [`EnableMultipleHttp2Connections`](https://docs.microsoft.com/dotnet/api/system.net.http.socketshttphandler.enablemultiplehttp2connections?view=net-6.0) turned on.

- Added non-validated enumeration of HTTP headers ([runtime/dotnet#35126](https://github.com/dotnet/runtime/issues/35126)). The change adds a new API [`HttpHeaders.NonValidated`](https://docs.microsoft.com/dotnet/api/system.net.http.headers.httpheaders.nonvalidated?view=net-6.0) to the header collection. It allows to inspect headers as they were received (without being sanitized) and it also skips all the parsing and validation logic, saving not just CPU cycles but also allocations.

- Optimized HPack Huffman decoding ([dotnet/runtime#43603](https://github.com/dotnet/runtime/pull/43603)). HPack is a header (de)compression format for HTTP/2 [RFC 7541](https://datatracker.ietf.org/doc/html/rfc7541). From our micro benchmarks, this optimization reduced the time taken for decoding to about 0.35 of the orignal one ([dotnet/runtime#1506](https://github.com/dotnet/runtime/issues/1506#issuecomment-705860700)).

- Introduced [`ZLibStream`](https://docs.microsoft.com/dotnet/api/system.io.compression.zlibstream?view=net-6.0). Originally, we didn't expect zlib envelope in deflate compressed content data ([dotnet/runtime#38022](https://github.com/dotnet/runtime/issues/38022)), which [RFC 2616](https://www.rfc-editor.org/rfc/rfc2616.html#section-3.5) defines as zlib format with deflate compression. Once we fixed that, another issue occurred since not all servers put the zlib envelope in place. So we introduced a mechanism to detect the format and use an appropriate type of stream ([dotnet/runtime#57862](https://github.com/dotnet/runtime/pull/57862)).

- Added cookies enumeration. Until .NET 6, there wasn't a way to enumerate all cookies in [`CookieContainer`](https://docs.microsoft.com/dotnet/api/system.net.cookiecontainer?view=net-6.0). You needed to know their domain names to get them. Moreover, there wasn't a way to get list of domains for which there are any cookies. People were using ugly hacks to access the cookies ([dotnet/runtime#44094](https://github.com/dotnet/runtime/issues/44094#issuecomment-820262017)). So we introduced a new API [`CookieContainer.GetAllCookies`](https://docs.microsoft.com/dotnet/api/system.net.cookiecontainer.getallcookies?view=net-6.0) to list all the cookies in the container ([dotnet/runtime#44094](https://github.com/dotnet/runtime/issues/44094)).

## Sockets
### Handle port exhaustion by utilizing auto-reuse port range on Windows
When opening concurrent HTTP/1.1 connections on a large scale, you may notice that new connection attempts start failing after some time. On Windows, this happens typically around ~16K concurrent connections, with socket error 10055 (`WSAENOBUFS`) as an internal `SocketException` message.
Normally, the network stack chooses a port that isn’t already bound to another socket, meaning that the maximum number of simultaneusly open connections is limited by the [dynamic port range](https://docs.microsoft.com/windows/client-management/troubleshoot-tcpip-port-exhaust#default-dynamic-port-range-for-tcpip).
This is a configurable range, typically defaulting to `49152-65535` and a theoretical limit of 2<sup>16</sup>=65536 ports, since a port is a 16 bit number.

To address this problem for the case when remote endpoints differ in IP addresses and/or ports, Windows [introduced](https://support.microsoft.com/topic/reliability-and-scalability-improvements-in-tcp-ip-for-windows-8-1-and-windows-server-2012-r2-82b226f4-cadc-7676-67db-2195516b7956) a feature called **auto-reuse port range** back in the Windows 8.1 times. .NET framework exposed the related socket option `SO_REUSE_UNICASTPORT` through an opt-in property [`ServicePointManager.ReusePort`](https://docs.microsoft.com/dotnet/api/system.net.servicepointmanager.reuseport), but this property bacame a no-op API on .NET Core / .NET 5+. Instead, in [dotnet/runtime#48219](https://github.com/dotnet/runtime/issues/54903) we enabled `SO_REUSE_UNICASTPORT` for all outgoing asynchronous `Socket` connections on .NET 6+, allowing ports to be reused between connections as long as:
- The connection’s full 4-tuple of (local port, local address, remote port, remote address) is unique.
- The auto-reuse port range is configured on the machine.

You can set the auto-reuse port range with the following PowerShell cmdlet:

```powershell
Set-NetTCPSetting -SettingName InternetCustom `
                  -AutoReusePortRangeStartPort <start-port> `
                  -AutoReusePortRangeNumberOfPorts <number-of-ports>
```

A reboot is required for the setting to take effect.

From the authors of the Windows feature:

> Due to sticky backward-compatibility problems, the auto-reuse port range must be used exclusively for outbound connections using this special logic. That means if the auto-reuse port range is configured to overlap with a well-known listen port (port 80, for instance), then an attempt to bind a listening socket to that port will fail. Also, if the auto-reuse port range fully covers the regular ephemeral port range, then normal wildcard binds will fail. Normally, choosing an auto-reuse range that is a strict subset of the default ephemeral port range will avoid problems. But the admin must still be careful, because some applications use large port numbers inside the ephemeral port range as "well-known" port numbers.

### An option to globally disable IPv6
Since .NET 5, we are using [DualMode](https://docs.microsoft.com/dotnet/api/system.net.sockets.socket.dualmode) sockets in `SocketsHttpHandler`.
This allows us to handle IPv4 traffic from an IPv6 socket, and is considered to be a favorable practice by [RFC 1933](https://tools.ietf.org/html/rfc1933).
On the other hand, we had several reports from users having trouble connecting through VPN tunnels which do not support IPv6 and/or dual-stack sockets properly.
To mitigate these issues and other potential problems with IPv6,
[dotnet/runtime#55012](https://github.com/dotnet/runtime/pull/55012) implemented a switch to disable IPv6 globally for the entire .NET 6 process.

You can now set the environment variable `DOTNET_SYSTEM_NET_DISABLEIPV6` to `1` or the `System.Net.DisableIPv6` [runtime configuration setting](https://docs.microsoft.com/dotnet/core/run-time-config/) to `true` if you experience similar problems and decide to address them by disabling IPv6.

### New Span- and Task-based overloads in `System.Net.Sockets`
With the help of the community, we managed to bring `Socket` and related types close to API-complete in terms of `Span`, `Task`, and cancellation support.
The complete API-diff is way too long to include into this blog post, you can find it in this [dotnet/core document](https://github.com/dotnet/core/blob/main/release-notes/6.0/api-diff/.Net/6.0.0_System.Net.Sockets.md).
We would like to thank [@gfoidl](https://github.com/gfoidl), [@ovebastiansen](https://github.com/ovebastiansen) and [@PJB3005](https://github.com/PJB3005) for their contributions!

## Security
In .NET 6, we have made two smaller changes worth mentioning in networking security space.

### Delayed Client Negotiation
This is a server-side `SslStream` function. It is used when the server decides that it needs to renegotiate encryption for already established connection. For example, when client accesses a resource that needs a client certificate that hasn't been initially provided.

The new `SslStream` method looks like this:
```C#
public virtual Task NegotiateClientCertificateAsync(CancellationToken cancellationToken = default);
```

The implementation uses two different TLS features depending on the TLS version. For TLS up to 1.2 inclusively, TLS renegotiation is used ([RFC 5746](https://www.rfc-editor.org/rfc/rfc5746.html)). For TLS 1.3, post handshake authentication extension is used ([RFC 8446](https://datatracker.ietf.org/doc/html/rfc8446#section-4.2.6)). Those two feature are abstracted in SChannel [`AcceptSecurityContext`](https://docs.microsoft.com/windows/win32/secauthn/acceptsecuritycontext--schannel) function. Thus, delayed client negotiation is fully supported on Windows. Unfortunately, with OpenSSL the story is different and therefore the support is limited to TLS renegotiation, i.e. TLS up to 1.2, on Linux. Moreover, MacOS is not supported at all since its security layer doesn't provide either of those. We are fully invested in closing this platform gap in .NET 7.

Note that neither TLS renegotiation nor post handshake authentication extension are allowed with HTTP/2 ([RFC 8740](https://datatracker.ietf.org/doc/html/rfc8740)) since it multiplexes multiple requests over one connection.

### Impersonation Improvement

This is Windows only feature where single process can have threads running under different users via [`WindowsIdentity.RunImpersonatedAsync`](https://docs.microsoft.com/dotnet/api/system.security.principal.windowsidentity.runimpersonatedasync?view=net-6.0). We did not behave well in two situation which we fixed in .NET 6. The first one was when doing asynchronous name resolution ([dotnet/runtime#47435](https://github.com/dotnet/runtime/pull/47435)). And the other was when sending HTTP requests where we would not honor the impersonated user ([dotnet/runtime#58033](https://github.com/dotnet/runtime/issues/58033)).

## Diagnostics
We got many questions, complaints and bug reports about the default behavior of [`HttpClient`](https://docs.microsoft.com/dotnet/api/system.net.http.httpclient?view=net-6.0) in regards to [`Activity`](https://docs.microsoft.com/dotnet/api/system.diagnostics.activity?view=net-6.0) creation ([dotnet/runtime#41072](https://github.com/dotnet/runtime/issues/41072)) and automatic trace header injection ([dotnet/runtime#35337](https://github.com/dotnet/runtime/issues/35337)). These problems were even more pronounced in ASP.NET Core projects where an `Activity` is created automatically, inadvertently turning on `DiagnosticsHandler`, which is part of `HttpClient` handler chain. Moreover, `DiagnosticsHandler` is an internal class without any configuration exposed via `HttpClient` thus forcing users to come up with hacky workarounds to control the behavior ([dotnet/runtime#31862](https://github.com/dotnet/runtime/issues/31862)) or just to turn it completely off ([dotnet/runtime#35337-comment](https://github.com/dotnet/runtime/issues/35337#issuecomment-864293752)).

All of these issues were addressed in .NET 6 ([dotnet/runtime#55392](https://github.com/dotnet/runtime/pull/55392)). The header injection can now be controlled with [`DistributedContextPropagator`](https://docs.microsoft.com/dotnet/api/system.diagnostics.distributedcontextpropagator?view=net-6.0). It can either be done globally via [`DistributedContextPropagator.Current`](https://docs.microsoft.com/dotnet/api/system.diagnostics.distributedcontextpropagator.current?view=net-6.0#System_Diagnostics_DistributedContextPropagator_Current) or per `HttpClient`/`SocketsHttpHandler` with [`SocketsHttpHandler.ActivityHeadersPropagator`](https://docs.microsoft.com/dotnet/api/system.net.http.socketshttphandler.activityheaderspropagator?view=net-6.0#System_Net_Http_SocketsHttpHandler_ActivityHeadersPropagator). We have also prepared some of the most asked for implementations:
- [`NoOutputPropagator`](https://docs.microsoft.com/dotnet/api/system.diagnostics.distributedcontextpropagator.createnooutputpropagator?view=net-6.0#System_Diagnostics_DistributedContextPropagator_CreateNoOutputPropagator) to suppress trace header injection.
- [`PassThroughPropagator`](https://docs.microsoft.com/dotnet/api/system.diagnostics.distributedcontextpropagator.createpassthroughpropagator?view=net-6.0) to inject a trace header with the value from the root `Activity`, i.e. act transparently and send the same header value as was received by the application.

For more granular control over the header injection, a custom `DistributedContextPropagator` can be provided. For example, one for skipping exactly the one layer emitted by `DiagnosticsHandler` (credits to [@MihaZupan](https://github.com/MihaZupan)):
```C#
public sealed class SkipHttpClientActivityPropagator : DistributedContextPropagator
{
    private readonly DistributedContextPropagator _originalPropagator = Current;

    public override IReadOnlyCollection<string> Fields => _originalPropagator.Fields;

    public override void Inject(Activity? activity, object? carrier, PropagatorSetterCallback? setter)
    {
        if (activity?.OperationName == "System.Net.Http.HttpRequestOut")
        {
            activity = activity.Parent;
        }

        _originalPropagator.Inject(activity, carrier, setter);
    }

    public override void ExtractTraceIdAndState(object? carrier, PropagatorGetterCallback? getter, out string? traceId, out string? traceState) =>
        _originalPropagator.ExtractTraceIdAndState(carrier, getter, out traceId, out traceState);

    public override IEnumerable<KeyValuePair<string, string?>>? ExtractBaggage(object? carrier, PropagatorGetterCallback? getter) =>
        _originalPropagator.ExtractBaggage(carrier, getter);
}
```

And finally, to pull this all together, set up `ActivityHeadersPropagator`:
```C#
// Set up headers propagator for this client.
var client = new HttpClient(new SocketsHttpHandler() {
    // -> Turns off activity creation as well as header injection
    // ActivityHeadersPropagator = null

    // -> Activity gets created but no trace header is injected
    // ActivityHeadersPropagator = DistributedContextPropagator.CreateNoOutputPropagator()

    // -> Activity gets created, trace header gets injected and contains "root" activity id
    // ActivityHeadersPropagator = DistributedContextPropagator.CreatePassThroughPropagator()

    // -> Activity gets created, trace header gets injected and contains "parent" activity id
    // ActivityHeadersPropagator = new SkipHttpClientActivityPropagator()

    // -> Activity gets created, trace header gets injected and contains "System.Net.Http.HttpRequestOut" activity id
    // Same as not setting ActivityHeadersPropagator at all.
    // ActivityHeadersPropagator = DistributedContextPropagator.CreateDefaultPropagator()
});

// If you want the see the order of activities created, add ActivityListener.
ActivitySource.AddActivityListener(new ActivityListener()
{
    ShouldListenTo = (activitySource) => true,
    ActivityStarted = activity => Console.WriteLine($"Start {activity.DisplayName}{activity.Id}"),
    ActivityStopped = activity => Console.WriteLine($"Stop {activity.DisplayName}{activity.Id}")
});

// Set up activities, at least two layers to show all the differences.
using Activity root = new Activity("root");
// Header format can be overridden, default is W3C, see https://www.w3.org/TR/trace-context/).
// root.SetIdFormat(ActivityIdFormat.Hierarchical);
root.Start();
using Activity parent = new Activity("parent");
// parent.SetIdFormat(ActivityIdFormat.Hierarchical);
parent.Start();

var request = new HttpRequestMessage(HttpMethod.Get, "https://www.microsoft.com");

using var response = await client.SendAsync(request);
Console.WriteLine($"Request: {request}"); // Print the request to see the injected header.
```

## URI
`HttpClient` uses `System.Uri` which does validation and canonicalization per [RFC 3986](https://datatracker.ietf.org/doc/html/rfc3986) and modifies some of the URIs in a way that might break their end-customers. For example, larger services or SDKs might need to pass a URI transparently from their source (e.g. Kestrel) to `HttpClient`, which was impossible in .NET 5 (see [dotnet/runtime#52628](https://github.com/dotnet/runtime/issues/52628), [dotnet/runtime#58057](https://github.com/dotnet/runtime/issues/58057)).

.NET 6 is introducing a new API flag [`UriCreationOptions.DangerousDisablePathAndQueryCanonicalization`](https://docs.microsoft.com/dotnet/api/system.uricreationoptions.dangerousdisablepathandquerycanonicalization?view=net-6.0) (see [dotnet/runtime#59274](https://github.com/dotnet/runtime/pull/59274)) which will allow the user to disable any canonicalization on URI and take it "as is".

Setting `DangerousDisablePathAndQueryCanonicalization` means no validation and no transformation of the input will take place past the authority. As a side effect, `Uri` instances created with this option do not support `Uri.Fragment`s -- it will always be empty. Moreover, `Uri.GetComponents(UriComponents, UriFormat)` may not be used for `UriComponents.Path` or `UriComponents.Query` and will throw `InvalidOperationException`.

Be aware that disabling canonicalization also means that reserved characters will not be escaped (e.g. space characters will not be changed to `%20`), which may corrupt the HTTP request and makes the application subject to request smuggling. Only set this option if you have ensured that the URI string is already sanitized.

```c#
var uriString = "http://localhost/path%4A?query%4A#/foo";

var options = new UriCreationOptions { DangerousDisablePathAndQueryCanonicalization = true };
var uri = new Uri(uriString, options);
Console.WriteLine(uri); // outputs "http://localhost/path%4A?query%4A#/foo"
Console.WriteLine(uri.AbsolutePath); // outputs "/path%4A"
Console.WriteLine(uri.Query); // outputs "?query%4A#/foo"
Console.WriteLine(uri.PathAndQuery); // outputs "/path%4A?query%4A#/foo"
Console.WriteLine(uri.Fragment); // outputs an empty string

var canonicalUri = new Uri(uriString);
Console.WriteLine(canonicalUri.PathAndQuery); // outputs "/pathJ?queryJ"
Console.WriteLine(canonicalUri.Fragment); // outputs "#/foo"
```
Note that the API is part of a larger API surface we designed for .NET 7 (see [dotnet/runtime#59099](https://github.com/dotnet/runtime/issues/59099)).


## Final Notes
This is not an exhaustive list of all the networking changes that happened in .NET 6. We tried to pick the most interesting ones or the ones with the biggest impact. If you find any errors in the networking stack, do not hesitate to contact us. You can find us under [`dotnet/ncl`](https://github.com/orgs/dotnet/teams/ncl) alias on GitHub.

Also, I'd like to thank my co-authors:
- [@antonfirsov](https://github.com/antonfirsov) who wrote [HTTP/2 Windows Scaling](#http2-window-scaling) and [Sockets](#sockets).
- [@CarnaViire](https://github.com/CarnaViire) who wrote [HTTP Retry Logic](#http-retry-logic) and [URI](#uri).