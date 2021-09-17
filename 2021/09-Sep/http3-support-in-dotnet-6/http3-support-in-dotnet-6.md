---
post_title: HTTP/3 support in .NET 6
username: sam.spencer@live.com
microsoft_alias: samsp
categories: .NET Core, ASP.NET, Networking
summary: See how to use HTTP/3 in your apps built using .NET 6.
desired_publication_date: 2021-09-16
---

.NET 6 includes preview support for HTTP/3:

- In Kestrel, HTTP.Sys & IIS for ASP.NET for server scenarios
- In HttpClient to make outbound requests
- For gRPC

## What is HTTP/3 and why is support important?

HTTP through version 1.1 was a relatively simple protocol, open a TCP connection, send a set of headers over clear text and then receive the response. Requests can be pipelined over the same connection, but each has to be handled in order. TLS adds some additional complications, and a couple of round-trips to initiate a connection, but once established, HTTP is used the same way over the secure channel.

As many sites have moved to require TLS encryption, and you can only serve a request at a time per connection with HTTP/1.1, performance of web pages that typically require downloading multiple resources (scripts, images, CSS, fonts, etc.) was being limited as multiple connections are needed, each of which has high setup costs.

HTTP/2 solved the problem by changing to become a binary protocol using a framing concept to enable multiple requests to be handled at the same time over the same connection. The setup costs for TLS could be paid once, and then all the requests can be interleaved over that single connection.

That's all great, except we have all gone mobile and much of the access is now from phones and tablets using Wi-Fi and cellular connections which can be unreliable. Although HTTP/2 enables multiple streams, they all go over a connection which is TLS encrypted, so if a TCP packet is lost all of the streams are blocked until the data can be recovered. This is known as the head of line blocking problem.

HTTP/3 solves these problems by using a new underlying connection protocol called QUIC. QUIC uses UDP and has TLS built in, so it's faster to establish connections as the TLS handshake occurs as part of the connection. Each frame of data is independently encrypted so it no longer has the head of line blocking in the case of packet loss. Unlike TCP a QUIC connection is independent of the IP address, so mobile clients can roam between wifi and cellular networks, keeping the same logical connection, continuing long downloads etc.

Amongst the metrics that sites and services use, tracking the latency of the worst connections using P90, P95 or P99 is common. HTTP/3 is already proving to have a positive impact on these numbers, improving the experience for users with the worst connections, for example [Facebook](https://engineering.fb.com/2020/10/21/networking-traffic/how-facebook-is-bringing-quic-to-billions/), [Snapchat](https://eng.snap.com/quic-at-snap/) & [Google Cloud](https://cloudplatform.googleblog.com/2018/06/Introducing-QUIC-support-for-HTTPS-load-balancing.html).

## QUIC Support in .NET

[QUIC](https://techcommunity.microsoft.com/t5/networking-blog/what-s-quic/ba-p/2683367) is designed as a base layer for HTTP/3 but it can also be used by other protocols. It is designed to work well for mobile with the ability to handle network changes, and have good recovery if packet loss occurs.

.NET uses the [MSQuic](https://github.com/microsoft/msquic) library for its QUIC implementation. This is an open source, cross platform library from the Windows networking team. For packaging reasons it is included in .NET 6 for Windows, and as a separate package for Linux.

A key difference with QUIC is that TLS encryption is built in, and so the connection establishment includes the TLS handshake. This means that the TLS library used needs to provide APIs to enable this type of handshake. For Windows, the APIs are included in SChannel / Bcrypt.dll. For Linux it's a bit more complicated – OpenSSL which is used by .NET and most other software on Linux does not yet include these APIs. The OpenSSL team has been heads down working on OpenSSL 3.0, which has a hard deadline to be submitted for FIPS 140-2 certification, so was unable to add that support directly in OpenSSL 3.0.

This created a problem for us, and many others working on QUIC and HTTP/3. To provide a stopgap solution until OpenSSL can include API support for QUIC handshakes, Microsoft has partnered with Akamai to create a fork of OpenSSL – [QuicTLS](https://github.com/QuicTLS/openssl) – that provides the APIs to enable the QUIC handshake. Unlike other forks which have diverged over time from OpenSSL, QuicTLS provides a minimal delta over mainline OpenSSL, and is kept in sync with the upstream.

The MSQuic package for Linux is statically linked with QuicTLS, and so does not need a separate download and managing multiple variants of the OpenSSL library. This also means that when mainline OpenSSL includes QUIC APIs, the package will be updated to use those instead.

In .NET 6, we are not exposing the [.NET QUIC APIs](https://github.com/dotnet/runtime/tree/main/src/libraries/System.Net.Quic), the goal is to make them public in .NET 7. QUIC can be used like a TCP socket and is not specific to HTTP/3 so we expect other protocols to be built on QUIC over time, such as [SMB over QUIC](https://techcommunity.microsoft.com/t5/storage-at-microsoft/smb-over-quic-is-now-in-public-preview/ba-p/2482964).

## HTTP/3 Support in .NET 6

At the time of publishing this post, the RFC for HTTP/3 is not yet finalized, and so can still change. We have included HTTP/3 in .NET 6 so that customers can start experimenting with it, but it is a preview feature for .NET 6 - this is because it does not meet the quality standards of the rest of .NET 6. There may be rough edges, and there needs to be broader testing with other servers & clients to ensure compatibility, especially in the edge cases.

### Prerequisites

To use HTTP/3 the prerequisite versions of MSQuic and its TLS dependencies need to be installed.

**Windows**

MsQuic is installed as part of .NET 6, but it needs an updated version of Schannel SSP which provides the TLS API, this is supplied with recent releases of the OS.

- Windows 11 Build 22000 or later, or Server 2022 RTM

Windows 11 builds are currently only available to [Windows Insiders](https://insider.windows.com/).

**Linux**

On Linux, libmsquic is published via Microsoft official Linux package repository packages.microsoft.com. In order to consume it, it must be added manually. See [Linux Software Repository for Microsoft Products](https://docs.microsoft.com/windows-server/administration/linux-package-repository-for-microsoft-software). After configuring the package feed, it can be installed via the package manager for your distro, for example, on Ubuntu:

```shell
sudo apt install libmsquic
```

### Kestrel Server

Server support is included in Kestrel. Preview features need to be enabled using the following project property:

```xml
<PropertyGroup>
  <EnablePreviewFeatures>True</EnablePreviewFeatures>
</PropertyGroup>
```

And then set in the listener options, for example:

```C#
public static async Task Main(string[] args)
{
  var builder = WebApplication.CreateBuilder(args);
  builder.WebHost.ConfigureKestrel((context, options) =>
  {
    options.Listen(IPAddress.Any, 5001, listenOptions =>
    {
      // Use HTTP/3
      listenOptions.Protocols = HttpProtocols.Http1AndHttp2AndHttp3;
      listenOptions.UseHttps();
    });
  });
}
```

For more details, see [Use HTTP/3 with the ASP.NET Core Kestrel web server](https://docs.microsoft.com/aspnet/core/fundamentals/servers/kestrel/http3?view=aspnetcore-6.0)

### HTTP/3 Client

HttpClient has been updated to include support for HTTP/3, but it needs to be enabled with a runtime flag. Include the following in the project file to enable HTTP/3 with HttpClient:

```xml
<ItemGroup>
  <RuntimeHostConfigurationOption Include="System.Net.SocketsHttpHandler.Http3Support" Value="true" />
</ItemGroup>
```

HTTP/3 needs to be specified as the version for the request:

```c#
// See https://aka.ms/new-console-template for more information
using System.Net;

var client = new HttpClient();
client.DefaultRequestVersion = HttpVersion.Version30;
client.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionExact;

var resp = await client.GetAsync("https://localhost:5001/");
var body = await resp.Content.ReadAsStringAsync();

Console.WriteLine($"status: {resp.StatusCode}, version: {resp.Version}, body: {body.Substring(0, Math.Min(100, body.Length))}");
```

## HTTP/3 via HTTP.sys & IIS

On Windows Server 2022, Http.sys supports HTTP/3 when it's enabled with a registry key, and TLS 1.3 must be enabled (default). This is independent of ASP.NET's support for HTTP/3, as the HTTP protocol is handled by HTTP.sys in this configuration – so it applies to not just ASP.NET but any content or services served by HTTP.sys. For more details see this [blog post](https://techcommunity.microsoft.com/t5/networking-blog/enabling-http-3-support-on-windows-server-2022/ba-p/2676880) from the Windows networking team.

## gRPC with HTTP/3
gRPC is a RPC mechanism using the protobuf serialization format. gRPC typically uses HTTP/2 as its transport. HTTP/3 uses the same semantics, so there is little change required to make it work. gRPC over HTTP/3 is not yet a standard, and is [proposed](https://github.com/grpc/proposal/pull/256) by the .NET team.

The following code is based on the [greeter sample](https://github.com/grpc/grpc/tree/master/examples/csharp/Helloworld), with the [hello world proto](https://github.com/grpc/grpc/tree/master/examples/protos).

The client and server projects require the same respective preview feature enablement in their projects as the samples further above.

**ASP.NET Server**
```c#
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.WebHost.ConfigureKestrel((context, options) =>
{
  options.Listen(IPAddress.Any, 5001, listenOptions =>
  {
    listenOptions.Protocols = HttpProtocols.Http3;
    listenOptions.UseHttps();
  });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
app.UseDeveloperExceptionPage();
}

app.MapGrpcService<GreeterService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
```

**Client**
```c#
using Grpc.Net.Client;
using GrpcService1;
using System.Net;

var httpClient = new HttpClient();
httpClient.DefaultRequestVersion = HttpVersion.Version30;
httpClient.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionExact;

var channel = GrpcChannel.ForAddress("https://localhost:5001", new GrpcChannelOptions() { HttpClient = httpClient });
var client = new Greeter.GreeterClient(channel);

var response = await client.SayHelloAsync(
new HelloRequest { Name = "World" });

Console.WriteLine(response.Message);
```

## macOS support

.NET 6 does not include support for HTTP/3 on macOS, primarily because of a lack of a QUIC compatible TLS API. .NET uses SecureTransport on macOS for its TLS implementation, which does not yet include TLS APIs to support QUIC handshake. While we could use OpenSSL, we felt it better to not introduce an additional dependency which is not integrated with the cert management of the OS.

## Going Forward

We will be investing further in QUIC and HTTP/3 in .NET 7, so expect to see updated functionality in the previews.
