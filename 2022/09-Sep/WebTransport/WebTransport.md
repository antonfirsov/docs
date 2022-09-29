---
post_title: Experimental WebTransport over HTTP/3 support in Kestrel
username: chrross
author1: chrross
microsoft_alias: chrross
featured_image: dotnet-bot_scene_powerline.png
categories: .NET Core, ASP.NET Core
tags: http, webtransport, networking, kestrel
summary: ASP.NET Core now has experimental support for WebTransport over HTTP/3, a new secure multiplexed transport protocol for the web. Learn how to try out this new transport protocol in your app.
desired_publication_date: 2022-09-29
post_date: 2022-09-29 10:05:00
---


We’re excited to announce experimental support for the WebTransport over HTTP/3 protocol as part of .NET 7 RC1.

This feature and blog post were written by our excellent intern [Daniel Genkin](https://github.com/Daniel-Genkin-MS-2)!

## What is WebTransport

WebTransport is a new [draft specification](https://ietf-wg-webtrans.github.io/draft-ietf-webtrans-http3/draft-ietf-webtrans-http3.html) for a transport protocol similar to WebSockets that allows the usage of multiple streams per connection.

[WebSockets](https://www.rfc-editor.org/rfc/rfc6455.html) allowed upgrading a whole HTTP TCP/TLS connection to a bidirectional data stream. If you needed to open more streams you'd spend additional time and resources establishing new TCP and TLS sessions. [WebSockets over HTTP/2](https://www.rfc-editor.org/rfc/rfc8441) streamlined this by allowing multiple WebSocket streams to be established over one HTTP/2 TCP/TLS session. The downside here is that because this was still based on TCP, any packets lost from one stream would cause delays for every stream on the connection.

With the introduction of HTTP/3 and QUIC, which uses UDP rather than TCP, WebTransport can be used to establish multiple streams on one connection without them blocking each other. For example, consider an online game where the game state is transmitted on one bidirectional stream, the players' voices for the game's voice chat feature on another bidirectional stream, and the player's controls are transmitted on a unidirectional stream. With WebSockets, these would all need to be put on separate connections or squashed into a single stream. With WebTransport, you can keep all the traffic on one connection but separate them into their own streams and, if one stream were to drop network packets, the others could continue uninterrupted.

## Enabling WebTransport

You can enable WebTransport support in ASP.NET Core by setting `EnablePreviewFeatures` to `True` and adding  the following `RuntimeHostConfigurationOption` item in your project's `.csproj` file:

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">
  <PropertyGroup>
    <EnablePreviewFeatures>True</EnablePreviewFeatures>
  </PropertyGroup>

  <ItemGroup>
    <RuntimeHostConfigurationOption Include="Microsoft.AspNetCore.Server.Kestrel.Experimental.WebTransportAndH3Datagrams" Value="true" />
  </ItemGroup>
</Project>
```


## Setting up a Server

To setup a WebTransport connection, you first need to configure a web host to listen on a port over HTTP/3:
```C#
var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel((context, options) =>
{
    // Port configured for WebTransport
    options.ListenAnyIP([SOME PORT], listenOptions =>
    {
        listenOptions.UseHttps(GenerateManualCertificate());
        listenOptions.Protocols = HttpProtocols.Http1AndHttp2AndHttp3;
    });
});
var host = builder.Build();
```
WebTransport uses HTTP/3, so you must select the `listenOptions.UseHttps` setting as well as set the `listenOptions.Protocols` to include HTTP/3.

The default Kestrel development certificate cannot be used for WebTransport connections. For local testing you can use the workaround described in the [Obtaining a test certificate section](#Obtaining-a-test-certificate).

Next, we define the code that will run when Kestrel receives a connection.
```C#
host.Run(async (context) =>
{
    var feature = context.Features.GetRequiredFeature<IHttpWebTransportFeature>();
    if (!feature.IsWebTransportRequest)
    {
        return;
    }
    var session = await feature.AcceptAsync(CancellationToken.None);

    // Use WebTransport via the newly established session.
});

await host.RunAsync();
```
The `Run` method is triggered every time there is a connection request. The `IsWebTransportRequest` property on the `IHttpWebTransportFeature` indicates if the current request is a WebTransport request. Once a WebTransport request is received, calling `IHttpWebTransportFeature.AcceptAsync()` accepts the WebTransport session so you can interact with the client. This original request must be kept alive for the durration of the session or else all streams associated with that session will be closed.

Calling `await host.RunAsync()` starts the server, which can then start accepting connections. 

## Interacting with a WebTransport Session

Once the session has been established both the client and server can create streams for that session. 

This example accepts a session, waits for a bidirectional stream, reads some data, reverse it, and then writes it back to the stream.
```C#
host.Run(async (context) =>
{
    var feature = context.Features.GetRequiredFeature<IHttpWebTransportFeature>();
    if (!feature.IsWebTransportRequest)
    {
        return;
    }
    var session = await feature.AcceptAsync(CancellationToken.None);


    ConnectionContext? stream = null;
    IStreamDirectionFeature? direction = null;
    while (true)
    {
        // wait until we get a stream
        stream = await session.AcceptStreamAsync(CancellationToken.None);
        if (stream is null)
        {
            // if a stream is null, this means that the session failed to get the next one.
            // Thus, the session has ended, or some other issue has occurred. We end the
            // connection in this case.
            return;
        }

        // check that the stream is bidirectional. If yes, keep going, otherwise
        // dispose its resources and keep waiting.
        direction = stream.Features.GetRequiredFeature<IStreamDirectionFeature>();
        if (direction.CanRead && direction.CanWrite)
        {
            break;
        }
        else
        {
            await stream.DisposeAsync();
        }
    }

    var inputPipe = stream!.Transport.Input;
    var outputPipe = stream!.Transport.Output;

    // read some data from the stream into the memory
    var length = await inputPipe.AsStream().ReadAsync(memory);

    // slice to only keep the relevant parts of the memory
    var outputMemory = memory[..length];

    // do some operations on the contents of the data
    outputMemory.Span.Reverse();

    // write back the data to the stream
    await outputPipe.WriteAsync(outputMemory);
});

await host.RunAsync();
```

This example opens a new stream from the server side and then sends data.
```C#
host.Run(async (context) =>
{
    var feature = context.Features.GetRequiredFeature<IHttpWebTransportFeature>();
    if (!feature.IsWebTransportRequest)
    {
        return;
    }
    var session = await feature.AcceptAsync(CancellationToken.None);

    // open a new stream from the server to the client
    var stream = await session.OpenUnidirectionalStreamAsync(CancellationToken.None);

    if (stream is not null)
    {
        // write data to the stream
        var outputPipe = stream.Transport.Output;
        await outputPipe.WriteAsync(new Memory<byte>(new byte[] { 65, 66, 67, 68, 69 }), CancellationToken.None);
        await outputPipe.FlushAsync(CancellationToken.None);
    }
});

await host.RunAsync();
```

## Sample Apps

To showcase the functionality of the WebTransport support in Kestrel, we also created two sample apps: an [interactive](https://github.com/dotnet/aspnetcore/tree/main/src/Servers/Kestrel/samples/WebTransportInteractiveSampleApp) one and a [console-based](https://github.com/dotnet/aspnetcore/tree/main/src/Servers/Kestrel/samples/WebTransportSampleApp) one. You can run them in Visual Studio via the F5 run options.

## Obtaining a test certificate

The current Kestrel development certificate cannot be used for WebTransport connections as it does not meet the requirements needed for WebTransport over HTTP/3. You can generate a new certificate for testing via the following C# (this function will also automatically handle certificate rotation every time one expires):
```C#
static X509Certificate2 GenerateManualCertificate()
{
    X509Certificate2 cert = null;
    var store = new X509Store("KestrelWebTransportCertificates", StoreLocation.CurrentUser);
    store.Open(OpenFlags.ReadWrite);
    if (store.Certificates.Count > 0)
    {
        cert = store.Certificates[^1];

        // rotate key after it expires
        if (DateTime.Parse(cert.GetExpirationDateString(), null) < DateTimeOffset.UtcNow)
        {
            cert = null;
        }
    }
    if (cert == null)
    {
        // generate a new cert
        var now = DateTimeOffset.UtcNow;
        SubjectAlternativeNameBuilder sanBuilder = new();
        sanBuilder.AddDnsName("localhost");
        using var ec = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        CertificateRequest req = new("CN=localhost", ec, HashAlgorithmName.SHA256);
        // Adds purpose
        req.CertificateExtensions.Add(new X509EnhancedKeyUsageExtension(new OidCollection
        {
            new("1.3.6.1.5.5.7.3.1") // serverAuth
        }, false));
        // Adds usage
        req.CertificateExtensions.Add(new X509KeyUsageExtension(X509KeyUsageFlags.DigitalSignature, false));
        // Adds subject alternate names
        req.CertificateExtensions.Add(sanBuilder.Build());
        // Sign
        using var crt = req.CreateSelfSigned(now, now.AddDays(14)); // 14 days is the max duration of a certificate for this
        cert = new(crt.Export(X509ContentType.Pfx));

        // Save
        store.Add(cert);
    }
    store.Close();

    var hash = SHA256.HashData(cert.RawData);
    var certStr = Convert.ToBase64String(hash);
    Console.WriteLine($"\n\n\n\n\nCertificate: {certStr}\n\n\n\n"); // <-- you will need to put this output into the JS API call to allow the connection
    return cert;
}
// Adapted from: https://github.com/wegylexy/webtransport
```

## Going Forward

WebTransport offers exciting new possibilities for real-time app developers, making it easier and more efficient to stream multiple kinds of data back and forth with the client.

Please send us [feedback](https://github.com/dotnet/aspnetcore/issues) about how this works for your scenarios. We'll use that feedback to continue the development of this feature in .NET 8, explore integrations with frameworks like SignalR and determine when to take it out of preview.

