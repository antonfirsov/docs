---
post_title: What's new for gRPC in .NET 6
username: jamesnk@microsoft.com
microsoft_alias: jamesnk
featured_image: grpc-icon-color-1.png
categories: .NET, ASP.NET Core, ASP.NET
summary: 'Check out the great new features and performance improvements for gRPC in .NET 6'
desired_publication_date: 2021-12-06
---

[gRPC](https://docs.microsoft.com/aspnet/core/grpc/) is a modern, cross-platform, high-performance RPC framework. gRPC for .NET is built on top of ASP.NET Core and is our recommended way to build RPC services using .NET.

.NET 6 further improves gRPC's already great performance and adds a new range of features that make gRPC better than ever in modern cloud-native apps. In this post I'll describe these new features as well as how we are leading the industry with the first gRPC implementation to support end-to-end HTTP/3.

## gRPC client-side load balancing

Client-side load balancing is a feature that allows gRPC clients to distribute load optimally across available servers. Client-side load balancing can eliminate the need to have a proxy for load balancing. This has several benefits:

* **Improved performance.** No proxy means eliminating an additional network hop and reduced latency because RPCs are sent directly to the gRPC server.
* **Efficient use of server resources.** A load-balancing proxy must parse and then resend every HTTP request sent through it. Removing the proxy saves CPU and memory resources.
* **Simpler application architecture.** Proxy server must be set up and configured correctly. No proxy server means fewer moving parts!

Client-side load balancing is configured when a channel is created. The two components to consider when using load balancing:

* The resolver, which resolves the addresses for the channel. Resolvers support getting addresses from an external source. This is also known as service discovery.
* The load balancer, which creates connections and picks the address that a gRPC call will use.

The following code example configures a channel to use DNS service discovery with round-robin load balancing:

```csharp
var channel = GrpcChannel.ForAddress(
    "dns:///my-example-host",
    new GrpcChannelOptions
    {
        Credentials = ChannelCredentials.Insecure,
        ServiceConfig = new ServiceConfig { LoadBalancingConfigs = { new RoundRobinConfig() } }
    });
var client = new Greet.GreeterClient(channel);

var response = await client.SayHelloAsync(new HelloRequest { Name = "world" });
```

![Client-side load balancing](loadbalancing.gif "Client-side load balancing")

For more information, see [gRPC client-side load balancing](https://docs.microsoft.com/aspnet/core/grpc/loadbalancing).

## Transient fault handling with retries

gRPC calls can be interrupted by transient faults. Transient faults include:

* Momentary loss of network connectivity.
* Temporary unavailability of a service.
* Timeouts due to server load.

When a gRPC call is interrupted, the client throws an `RpcException` with details about the error. The client app must catch the exception and choose how to handle the error.

```csharp
var client = new Greeter.GreeterClient(channel);
try
{
    var response = await client.SayHelloAsync(
        new HelloRequest { Name = ".NET" });

    Console.WriteLine("From server: " + response.Message);
}
catch (RpcException ex)
{
    // Write logic to inspect the error and retry
    // if the error is from a transient fault.
}
```

Duplicating retry logic throughout an app is verbose and error-prone. Fortunately, the .NET gRPC client now has built-in support for automatic retries. Retries are centrally configured on a channel, and there are many options for customizing retry behavior using a `RetryPolicy`.

```csharp
var defaultMethodConfig = new MethodConfig
{
    Names = { MethodName.Default },
    RetryPolicy = new RetryPolicy
    {
        MaxAttempts = 5,
        InitialBackoff = TimeSpan.FromSeconds(1),
        MaxBackoff = TimeSpan.FromSeconds(5),
        BackoffMultiplier = 1.5,
        RetryableStatusCodes = { StatusCode.Unavailable }
    }
};

// Clients created with this channel will automatically retry failed calls.
var channel = GrpcChannel.ForAddress("https://localhost:5001", new GrpcChannelOptions
{
    ServiceConfig = new ServiceConfig { MethodConfigs = { defaultMethodConfig } }
});
```

For more information, see [Transient fault handling with gRPC retries](https://docs.microsoft.com/aspnet/core/grpc/retries).

## Protobuf performance

gRPC for .NET uses the Google.Protobuf package as the default serializer for messages. Protobuf is an efficient binary serialization format. Google.Protobuf is designed for performance, using code generation instead of reflection to serialize .NET objects. In .NET 5 we worked with the Protobuf team to add support for modern memory APIs such as `Span<T>`, `ReadOnlySequence<T>`, and `IBufferWriter<T>` to the serializer. The improvements in .NET 6 optimize an already fast serializer.

[protocolbuffers/protobuf#8147](https://github.com/protocolbuffers/protobuf/pull/8147) adds vectorized string serialization. SIMD instructions allow multiple characters to be processed in parallel, dramatically increasing performance when serializing certain string values.

```csharp
private string _value = new string(' ', 10080);
private byte[] _outputBuffer = new byte[10080];

[Benchmark]
public void WriteString()
{
    var span = new Span<byte>(_outputBuffer);
    WriteContext.Initialize(ref span, out WriteContext ctx);
    ctx.WriteString(_value);
    ctx.Flush();
}
```

| Method               | Google.Protobuf | Mean         | Ratio | Allocated |
| -------------------- | ----------------| ------------:| -----:| ---------:|
| WriteString          | 3.14            |     8.838 us |  1.00 |       0 B |
| WriteString          | 3.18            |     2.919 ns |  0.33 |       0 B |

[protocolbuffers/protobuf#7645](https://github.com/protocolbuffers/protobuf/pull/7645) adds a new API for creating `ByteString` instances. If you know the underlying data won't change, then use `UnsafeByteOperations.UnsafeWrap` to create a `ByteString` without copying the underlying data. This is very useful if an app works with large byte payloads and you want to reduce garbage collection frequency.

```csharp
var data = await File.ReadAllBytesAsync(@"c:\large_file.json");

// Safe but slow.
var copied = ByteString.CopyFrom(data);

// Unsafe but fast. Useful if you know data won't change.
var wrapped = UnsafeByteOperations.UnsafeWrap(data);
```

## gRPC download speeds

gRPC users reported sometimes getting slow download speeds. Our investigation discovered that HTTP/2 flow control was constraining downloads when there is latency between the client and server. The server fills the receive buffer window before the client can drain it, causing the server to pause sending data. gRPC messages are downloaded in start/stop bursts.

This is fixed in [dotnet/runtime#54755](https://github.com/dotnet/runtime/pull/54755). `HttpClient` now dynamically scales the receive buffer window. When an HTTP/2 connection is established, the client will send a ping to the server to measure latency. If there is high latency, the client automatically increases the receive buffer window, enabling a fast, continuous download.

```csharp
private GrpcChannel _channel = GrpcChannel.ForAddress(...);
private DownloadClient _client = new DownloadClient(_channel);

[Benchmark]
public Task GrpcLargeDownload() =>
    _client.DownloadLargeMessageAsync(new EmptyMessage());
```

| Method               | Runtime         | Mean        | Ratio |
| -------------------- | ----------------| -----------:| -----:|
| GrpcLargeDownload    | .NET 5.0        |      6.33 s |  1.00 |
| GrpcLargeDownload    | .NET 6.0        |      1.65 s |  0.26 |

## HTTP/3 support

gRPC on .NET now supports HTTP/3. gRPC builds on top of HTTP/3 support added to ASP.NET Core and `HttpClient` in .NET 6. For more information, see [HTTP/3 support in .NET 6](https://devblogs.microsoft.com/dotnet/http-3-support-in-dotnet-6/).

.NET is the first gRPC implementation to support end-to-end HTTP/3, and we have [submitted a gRFC](https://github.com/grpc/proposal/pull/256) for other platforms to support HTTP/3 in the future. gRPC with HTTP/3 is a [highly requested feature by the developer community](https://github.com/grpc/grpc/issues/19126), and it is exciting to see .NET leading the way in this area.

![gRPC and HTTP/3](grpchttp3.png "gRPC and HTTP/3")

## Wrapping Up

Performance is a feature of .NET and gRPC, and .NET 6 is faster than ever. New performance-orientated features like client-side load balancing and HTTP/3 mean lower latency, higher throughput, and fewer servers. It is an opportunity to save money, reduce power use and build greener cloud-native apps.

To try out the new features and get started using gRPC with .NET, the best place to start is the [Create a gRPC client and server in ASP.NET Core](https://docs.microsoft.com/aspnet/core/tutorials/grpc/grpc-start) tutorial.

We look forward to hearing about apps built with gRPC and .NET and to your future contributions in the [dotnet](https://github.com/dotnet) and [grpc](https://github.com/grpc) repos!
