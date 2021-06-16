---
post_title: Conversation about networking
username: rlander@microsoft.com
microsoft_alias: rlander@microsoft.com
categories: .NET
desired_publication_date: 06/16/2021
summary: Conversation with .NET engineers about networking in .NET. They share their perspective on network protocols, the differences and benefits between HTTP 1 through 3, the architecture of the .NET networking stack, why client and server implementations and APIs are different, and how the focus on the YARP reverse proxy has been a great stress test for the networking implementation and performance.
---

Pretty much every app is network-connected and depends on fast and reliable networking to provide its intended experience. This includes both client- and server-side networking, and even both within the same app. There is also a lot of new development and diversity of requirements across networking protocols and systems. The networking team focuses on delivering high-performance APIs, primarily for HTTP, that you can use to write highly-scalable apps.

We’re using the [conversation format](https://devblogs.microsoft.com/dotnet/category/conversations/) again, this time with runtime engineers who work on networking and related topics.

* [David Fowler](https://github.com/davidfowl)
* [Geoff Kizer](https://github.com/geoffkizer)
* [Karel Zikmund](https://github.com/karelz)
* [Sam Spencer](https://github.com/samsp-msft)
* [Stephen Toub](https://github.com/stephentoub)

## Let's start by describing the networking protocols that .NET supports out of the box.

**Karel:** HTTP 1.1, HTTP/2, WebSockets

**Sam:** Sockets, HTTP, WebSockets.

**Geoffrey:** TCP/IP, HTTP (1.1 and 2, including WebSockets), SSL, FTP, SMTP, as well as support for NT authentication across these.

**Stephen:** The lowest layer of .NET's networking support is sockets. Everything else is built on top of that. `HttpClient` and Kestrel provide HTTP/1.x, HTTP/2.0, and (coming in .NET 6) HTTP/3.0. SmtpClient provides support for SMTP. `WebSocket` (both client and server) provides websockets. TLS/SSL is provided by `SslStream`. FTP is supported by `FtpClient`.  `NegotiateStream` uses the Negotiate protocol.

**David:** We also have some custom higher level protocols like SignalR’s RPC protocol and gRPC.

**Geoffrey:** Yeah, gRPC should be included.

## What are some networking protocols that .NET doesn't support out of the box that are commonly used, like MQTT. Should we fix that?

**Geoffrey:** MQTT -- probably. AMQP -- probably not.

**Karel:** DNS (direct APIs) - we should support it

**Sam:** should **we** fix it (MQTT)? Probably not.

**Stephen:** There are also some protocols we "support" but the implementations/designs are old and we generally recommend using alternatives in the ecosystem, e.g. FTP and SMTP.

**David:** Only the most pervasive ones like HTTP and DNS.

**Sam:** Having a DNS client would be good.

**Stephen:** We have a DNS client, but it's limited in the protocols it supports, and it's implemented as a wrapper for the underlying operating system.

**Geoffrey:** I agree we should be considering our own DNS client, but I'm not sure that's relevant to this since we already support it today.

**Sam:** mDNS, upnp.

## HTTP is hugely popular. Describe the architecture of our HTTP stack at a high-level, including cross-platform support.

**Stephen:** Client and server are a bit different here, but fundamentally everything is built on top of some transport layer, with HTTP then layered on top of that. For `HttpClient`, it sits on top of `System.IO.Stream` and defaults to an implementation that uses Sockets but that supports a different Stream being plugged in. For Kestrel, it has a more robust model for plugging in custom transports, again defaulting to one based on `Socket`.

**Geoffrey:** Our HTTP stack is built on top of our cross-platform Sockets capabilities (`Socket` class) and our cross-platform SSL/TLS implementation (`SslStream`). `HttpClient` provides client support, and Kestrel/ASP.NET provide server support. Both support HTTP 1.1 and 2.0, and in the future 3.0.

**David:** There’s a server stack and a client stack. The client stack is exposed via `HttpClient` APIs and the server is exposed via ASP.NET Core. The server stack is a bit more complicated because it supports multiple implementations out of the box, [HTTP.sys](https://docs.microsoft.com/aspnet/core/fundamentals/servers/httpsys), [IIS](https://docs.microsoft.com/aspnet/core/host-and-deploy/iis) (our windows web server), and [kestrel](https://docs.microsoft.com/aspnet/core/fundamentals/servers/kestrel) (a socket based server implementation).

## Zoom in a layer deeper, for the client API.

**Karel:** `HttpClient` is pluggable - defaults to socket-based implementation [SocketsHttpHandler](https://devblogs.microsoft.com/dotnet/announcing-net-core-2-1/#networking-performance-improvements). There is bunch of platform specific Handlers, which leverage OS HTTP stacks (Android, iOS, [WinHttpHandler](https://docs.microsoft.com/dotnet/api/system.net.http.winhttphandler) for Windows, `BrowserHttpHandler` for WASM). Platform handlers can leverage OS capabilities that sockets cannot (e.g. transfer of connections between networks on Android, iOS).

**Stephen:** [`HttpClient`](https://docs.microsoft.com/dotnet/api/system.net.http.httpclient) is a wrapper for an [`HttpMessageHandler`](https://docs.microsoft.com/dotnet/api/system.net.http.httpmessagehandler) (an abstract base class). By default, `HttpClient` uses `SocketsHttpHandler`, which derives from `HttpMessageHandler` and is built on top of `System.Net.Sockets` (but supports replacing that with an arbitrary `System.IO.Stream`) and `System.Net.Security.SslStream`, and provides support for HTTP/1.x, HTTP/2.0, and (coming soon) HTTP/3.0. Other `HttpMessageHandler` implementations can be provided; for example, we produce another, `WinHttpHandler`, which wraps the Windows [`winhttp`](https://docs.microsoft.com/windows/win32/winhttp/about-winhttp) component. Historically, we've had additional implementations, including one based on [curl](https://curl.se/), but over time have opted for doing as much as possible in managed code rather than wrapping native components, except at the lowest layers where it's a necessity or where we've opted to for security reasons.

**Geoffrey:** `HttpClient` is the main class, but it sits on top of an `HttpMessageHandler `, which provides the bulk of the protocol implementation. `SocketsHttpHandler` is the default handler on Windows/Mac/Linux; we also provide platform specific handlers for other plats, e.g. `BrowserHttpHandler` for Blazor, etc. `HttpMessageHandler` is replaceable and multiple can be composed together. An `HttpMessageHandler` is just something that can take an `HttpRequestMessage` (representing the request) and return an `HttpResponseMessage` (representing the response). These two objects allow you to set/get HTTP verb, headers, and content for the request and response.

## Same, for the server API.

**David:** The server stack has a couple of layers involved with different responsibilities. The [`IServer`](https://docs.microsoft.com/dotnet/api/microsoft.aspnetcore.hosting.server.iserver) is the core of the stack and decides how requests are handled, the middleware pipeline that is where cross cutting concerns and application logic runs (things like routing ASP.NET WebAPI/MVC/Razor Pages and also things like). We ship 3 IServer implementations out of the box, IIS, Http.sys and Kestrel. Kestrel is the server implementation that is fully implemented in managed code, down to the transport layer (`System.Net.Sockets`).

## Why do the server and client APIs need to diverge so much?

**Sam:** Because their usage is very different. On the server it's also mixed with an app model, so its not just a protocol, but also the infrastructure for the apps.

**Geoffrey:** They don't need to, necessarily. But our priority has been to provide a great experience for each, while also minimizing API churn.

**Stephen:** In some ways, they don't... certain concepts could be modeled in the same way, and haven't in certain cases simply because, historically, they were designed by different groups of people with differing goals, and as a result differences have naturally crept in to the APIs. In other ways, usage is different, e.g. whether a request is initiated vs received, and that leads to differences in the APIs.

**Sam:** From a YARP perspective, unifying the API for things like headers so they can just be passed off as a collection would be very useful.

**Karel:** Both sides solve different problems - client cares about DNS resolution, proxies. Server cares about customization of rate limiting, security of requests coming from rouge clients.

**Geoffrey:** Building a "converged" API that does a good job of addressing both client and server scenarios is a lot harder than building different APIs for each that do so.

**David:** As for why client and server differ? We attempted to merge them in .NET Framework and it caused some confusion. It's hard to distinguish what "middleware" was for client and which one was for server. There were also some fundamental differences in the programming model on the server side that we made a decision on very early (mutable context, vs request/response) that doesn't gel with the current client model.

## YARP presumably uses both client and server APIs at once. Is it a good stress case for client and server APIs and for streaming APIs, too? Who do you see adopting YARP?

**Sam:** Yes, [YARP](https://github.com/microsoft/reverse-proxy) is a good stress case, and benchmarks show that we are behind the competition especially for larger payloads.

**Karel:** YARP is good stress case for both sides, because it is expected to have high-throughput deployments which very few real-world apps achieve. And none of them will be as minimal as YARP.

YARP will be adopted by folks who need reverse proxy component in their stack -- protecting their end-points from the wild internet, need to load balance, A/B testing, etc.

**Geoffrey:** Sure, it's a pretty good stress case. It has a few unique issues it needs to deal with that most customer scenarios probably won't care about, but that's the exception and not the rule.

I suspect people will adopt YARP who are using (or considering using) other reverse proxy components, but prefer YARP because of it's integration with our platform/framework generally.

**Sam:** Internal infrastructure teams are adopting YARP due to the extensibility, and support for HTTP/2. Http/2 makes implementing your own proxy significantly more difficult than 1.1.

**Geoffrey:** I doubt anyone actually starts from "I want YARP" -- I expect they start from "I want a reverse proxy" and then consider their options from there. But I could be wrong.

**Stephen:** "Stress" could have multiple meanings here. From a scalability / reliability perspective, you can throw tons of requests at it and stress both the server and client APIs as part of it, rather than requiring separate load tests for each individually. "Stress" could also mean how the APIs fit together, from a usability perspective, which gets back to the previous question about the divergence. Whether for scalability, reliability, or usability, YARP is helping us to evolve our APIs and implementations to better meet the needs of cloud native services.

**David:** YARP is a great stress test for both client and server and one of the main goals is to flesh out bottlenecks and drive improvements into the platform as a result of a real end to end product. We see lots of internal teams at Microsoft using it as their reverse proxy in modern micro-service architectures as well as third parties looking for an easy to use extensible proxy.

## Describe the .NET networking APIs relative to Java NIO or Netty.

**Geoffrey:** Our model is based on async IO operations, and composing these operations together using async methods and Task-based coordination. The Netty model is based on I/O events, which are similar in some ways to I/O operations, but do not provide the same level of power and composability that async and await in C# do.

**David:** Java NIO exposes an event loop model while .NET APIs are what Geoffrey said. But I would refrain from drawing any conclusions based on that.

## There has been an effort to move more of the .NET networking stack to C#. What are the benefits of that?

**David:** It's already in C#!

**Karel:** C# implementation has advantage of cross-platform support (where C# is, our networking stack is) -- we do not have to write adapter to each platform specifically (see PlatformHandlers above). It brings consistency across the platforms.

It also allows us to fine-tune performance in unified way across all platforms.

**David:** We like things in C# for several reasons:

* More portable in the face of new platforms.
* Consistent behavior.
* More eyes from the community. The .NET community likes to code using C#.

**Geoffrey:** Several. A big one is performance. We found that relying on external HTTP components like `WinHttp` or `libcurl` was an impediment to optimizing our HTTP performance. It's too high of a level of abstraction to get good performance. With our C# implementations, we don't hit this performance bottleneck, and as a result our implementation is much faster. And we integrated better with CLR infrastructure like the threadpool, which also helps for performance and memory usage.

Another benefit is consistency across platforms. With different implementations, there were lots of subtle (and some not so subtle) differences in behavior across platforms, and feature support was not always consistent. With a C# implementation we can provide a high level of consistency across platforms.

Finally, it's one less external dependency for us (or customers) to have to worry about, which is always good.

Building on our own `Socket` and `SslStream` components has helped make those components better, and should given customers confidence that you can build high-performance, quality software using these building blocks.

**David:** Unfortunately we still have hard OS dependencies for `SSLStream` and Crypto related to networking.

## HTTP/2 support was recently added. What's the big advantage over HTTP 1.1? When should you use that? Is it enabled by default for ASP.NET?

**Stephen:** HTTP/2 is required for certain protocols like gRPC; arguably its biggest advantage (beyond touted aspects like multiplexing many requests onto the same TCP connection, which is more relevant for browsers) is enabling access to such protocols.

**Sam:** HTTP/2 can make better use of each connection, so you have less overhead for establishing TLS. Its also the basis for gRPC.

**Karel:** Establishing any HTTP connection takes several round-trips between client and server. Also it takes time to adjust for throughput between them.

HTTP/2 has the power of sharing single connection for multiple requests in parallel, so you are doing these delays and throughput optimizations just once instead of multiple times for multiple HTTP 1.1 connections

**Geoffrey:** HTTP/2 shines brightest when used in the browser, because it has several improvements that reduce latency and thus page load time. If you're doing server-to-server communication, where connections are typically long lived, the benefits are less. You should see some small benefits from multiplexing (reducing the number of connections needed) and from the binary encoding (reducing request/response size on the wire).

## Same thing with HTTP/3 / QUIC.

**Sam:** The biggest benefits for QUIC are for mobile clients switching between networks - it enables a connection to persist across the network changes.

QUIC fixes some of the problems with HTTP/2's head of line blocking.

**Karel:** HTTP/3 is great for devices - it can transfer connections between networks (e.g. Wi-Fi -> LTE). One big advantage against HTTP/2 is that it behaves better on lossy networks -- if you lose some packets, HTTP/2 will block processing of all requests until the packet is retransmitted. HTTP/3 will block only one of the parallel requests that belongs to the lost packet.

**Sam:** QUICs requirement of UDP means that its likely to not work as well with network infrastructure until it catches up.

**Geoffrey:** HTTP3 is a better HTTP2; the basic design is similar, but it further optimizes connection establishment and data transfer by combining multiplexing, SSL, and reliable connection management into a new low-level transport protocol (QUIC). It also solves a new problem introduced in HTTP2 -- "head of line" blocking. If you're considering moving to HTTP2 but aren't sure it's worth the trouble, you may want to skip it and wait for HTTP3, which is coming soon.

## Were HTTP/2 and HTTP/3 implemented primarily in C#, like HTTP 1.x?

**Karel:** Yes for HTTP/2, no for HTTP/3 (yet - we have a prototype though).

**Sam:** Http/3 is C#, MSQuic is C/C++.

**Geoffrey:** Yes, HTTP/2 and HTTP/3 are implemented entirely in C#. The QUIC protocol, which HTTP3 relies on, is not -- we depend on the MSQuic native library to provide QUIC protocol support.

**Stephen:** Yes. The HTTP protocol implementations are all managed, for HTTP/1.x, 2.0, and 3.0. For 1.x and 2.0, they sit on top of Socket, which in turn P/Invokes into the underlying operating system. For 3.0, they sit on top of a QuicConnection, which is implemented as P/Invokes into the native msquic library today.

## Are we moving to a model where each of these HTTP versions will be common, both on the internet and in .NET code? Perhaps HTTP/2 will go away, and HTTP 1.1 and HTTP/3 will be pervasive?

**Stephen:** HTTP/1.1 is going to be around for a long time. HTTP/3.0 is brand new, but it seems likely it's here to stay. HTTP/2.0... jury's out

**Geoffrey:** It's too early to say for sure. We can confidently say that HTTP/1.1 will be around for a long, long time -- it's the easiest and most broadly implemented way to do HTTP. Bluntly: if HTTP3 succeeds it will supplant HTTP2 entirely; if not, well.... then it won't, and HTTP2 will stick around.

**Sam:** I think HTTP/3 will take longer for adoption due to the requirement for QUIC. Other library infrastructure like OpenSSL does not yet work with QUIC, so it will take time for the ecosystem to shake out.

## Will there be an HTTP/4? If so, do you have a sense of which existing problems it might solve, or that you'd hope it would solve?


## There is a lot of focus in the industry to move users to HTTPS and to modern and safe TLS versions. What is the .NET team doing to encourage that?

**Stephen:** For TLS, the main thing we do is stay up-to-date in support for TLS versions, and also defer to the operating system for what versions/ciphers/etc to allow by default. We also implement features meant to help developers remain safe by default.

**Karel:** We provide guidance to rely on OS settings of safe TLS versions.

We let users decide on HTTP vs. HTTPS, though we recommend HTTPS more.

**Geoffrey:** First, we provide a first-class implementation of `SslStream` that's easy to use and high performance, supports the latest protocols like TLS 1.3, etc, etc.

Beyond this, we've worked to update our default SSL settings to ensure they are modern and provide good protection by default.

## Performance has been a huge focus for networking. What are the primary changes that have been made to improve performance?

**Karel:** Primary changes for perf were optimizations in Sockets, how we use I/O, and `ThreadPool`.

**Geoffrey:** It's really hard to identify "primary" changes that improve performance. That's not how improving performance works, usually. It's a process, where we are continually evaluating our current performance and looking for opportunities to improve, and harvesting wins of 2 or 3% here or there. It's the commitment to performance overall that has allowed us to improve so much, not any one or two specific changes.

## Same, but forward looking.


## What does networking look like in a Wasm environment, both browser- and non-browser based?

## Closing

Networking is only going to become more important over time. The approaches we've taken, of basing the architecture on low-level building blocks and then implementing the rest of the protocol in C#, and then of building higher level systems like [Yarp](https://github.com/microsoft/reverse-proxy) that stress everything underneath has paid dividends and will continue to in future. One expects that Yarp performance will continue to be a focus for the team and that investments in that scenario will provide general benefits that many of you will enjoy.

Thanks again to Stephen, Sam, Karel, Geoffrey and David for sharing your insights and context on networking. It was a great [conversation](https://devblogs.microsoft.com/dotnet/category/conversations/).
