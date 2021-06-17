---
post_title: Announcing .NET 6 Preview 5
username: rlander@microsoft.com
microsoft_alias: rlander@microsoft.com
categories: .NET Core, .NET
desired_publication_date: 06/15/2021
summary: .NET 6 Preview 5 is now available.
---

We are thrilled to release .NET 6 Preview 5. We're now in the second-half of the .NET 6 release, and starting to see significant features coming together. A great example is .NET SDK Workloads, which is the foundation of our [.NET unification vision](https://devblogs.microsoft.com/dotnet/announcing-net-6-preview-4/#net-platform-unification) and enables supporting more application types. Like other features, it is coming together to provide a compelling end-to-end user experience.

You can [download .NET 6 Preview 5](https://dotnet.microsoft.com/download/dotnet/6.0) for Linux, macOS, and Windows.

* [Installers and binaries](https://dotnet.microsoft.com/download/dotnet/6.0)
* [Container images](https://hub.docker.com/_/microsoft-dotnet)
* [Linux packages](https://github.com/dotnet/core/blob/main/release-notes/6.0/install-linux.md)
* [Release notes](https://github.com/dotnet/core/blob/main/release-notes/6.0/README.md)
* [API diff](https://github.com/dotnet/core/tree/main/release-notes/6.0/preview/api-diff/preview5)
* [Known issues](https://github.com/dotnet/core/blob/main/release-notes/6.0/known-issues.md)
* [GitHub issue tracker](https://github.com/dotnet/core/issues/6141)

See the [ASP.NET Core](https://devblogs.microsoft.com/aspnet/) and [EF Core](https://devblogs.microsoft.com/dotnet) posts for more detail on what’s new for web and data access scenarios.

[Visual Studio 2022 Preview 1 is also releasing today](https://aka.ms/VS2022P1) and .NET 6 Preview 5 is included in that release. .NET 6 has also been tested with [Visual Studio 16.11](https://visualstudio.microsoft.com/vs/) and [Visual Studio for Mac 8.9](https://visualstudio.microsoft.com/vs/mac/). We recommend you use those builds if you want to try .NET 6 with [Visual Studio](https://visualstudio.microsoft.com/).

Check out the new [conversations posts](https://devblogs.microsoft.com/dotnet/category/conversations/) for in-depth engineer-to-engineer discussions of the latest .NET features.

## .NET SDK: Optional Workload improvements

[SDK workloads](https://github.com/dotnet/designs/blob/main/accepted/2020/workloads/workloads.md) is a new .NET SDK feature that enables us to add support for new application types -- like [mobile](https://devblogs.microsoft.com/dotnet/announcing-net-maui-preview-4/) and [WebAssembly](https://devblogs.microsoft.com/aspnet/asp-net-core-updates-in-net-6-preview-4/#blazor-webassembly-ahead-of-time-aot-compilation) -- without increasing the size of the SDK.

The workloads feature has been updated to include `list` and `update` verbs. These new capabilities provide a sense of the expected final experience. You'll be able to quickly establish your preferred environment with a few simple commands and to keep it up-to-date over time.

* `dotnet workload list` will tell you which workloads you have installed.
* `dotnet workload update` will update all installed workloads to the newest available version.

The `update` verb queries `nuget.org` for updated workload manifests, updates local manifests, downloads new versions of the installed workloads, and then removes all old versions of a workload. This is analogous to `apt update` and `apt upgrade -y` (used on Debian-based Linux distros).

The `dotnet workload` commands operate in the context of the given SDK. Imagine you have both .NET 6 and .NET 7 installed. If you use both, the workloads commands will provide different results since the workloads will be different (at least different versions of the same workloads).

As you can see, the workloads feature is essentially a package manager for the .NET SDK. Workloads were first introduced in the [.NET 6 preview 4](https://devblogs.microsoft.com/dotnet/announcing-net-6-preview-4/#cli-install-of-net-6-sdk-optional-workloads) release.

## .NET SDK: NuGet Package Validation

[Package Validation](https://github.com/dotnet/core/issues/5700) tooling will enable NuGet library developers to validate that their packages are consistent and well-formed.

This includes:

* Validate that there are no breaking changes across versions.
* Validate that the package has the same set of publics APIs for all runtime-specific implementations.
* Determine any target-framework- or runtime- applicability gaps.

This tool is available via the [Microsoft.DotNet.PackageValidation](https://www.nuget.org/packages/Microsoft.DotNet.PackageValidation).

A post on this tool will soon be available: https://aka.ms/packageValidationPreview5.

## .NET SDK: more Roslyn Analyzers

In .NET 5, we shipped approximately 250 analyzers with the .NET SDK. Many of them already existed but were shipped out-of-band as NuGet packages. We're [adding more analyzers for .NET 6](https://github.com/dotnet/runtime/issues/43617).

By default most of the new analyzers are enabled at Info level. You can enable these analyzers at Warning level by [configuring the analysis mode](https://docs.microsoft.com/dotnet/fundamentals/code-analysis/overview#enable-additional-rules) like this: `<AnalysisMode>AllEnabledByDefault</AnalysisMode>`.

We published the set of analyzers we wanted for .NET 6 (plus some extras) and then made most of them [up-for-grabs](https://github.com/dotnet/runtime/issues?q=label%3Aup-for-grabs+label%3Acode-fixer+label%3Acode-analyzer). The community has added several implementations, including these ones in Preview 5.

| Contributor | Issue | Title |
|-------|-------|--------|
| [Newell Clark](https://github.com/NewellClark) | [dotnet/runtime #33777](https://github.com/dotnet/runtime/issues/33777) | Use span-based `string.Concat` |
| [Newell Clark](https://github.com/NewellClark)  | [dotnet/runtime #33784](https://github.com/dotnet/runtime/issues/33784) | Prefer `string.AsSpan()` over `string.Substring()` when parsing |
| [Newell Clark](https://github.com/NewellClark) | [dotnet/runtime #33789](https://github.com/dotnet/runtime/issues/33789) | Override `Stream.ReadAsync/WriteAsync` |
| [Newell Clark](https://github.com/NewellClark) | [dotnet/runtime #35343](https://github.com/dotnet/runtime/issues/35343) | Replace `Dictionary<,>.Keys.Contains` with `ContainsKey` |
| [Newell Clark](https://github.com/NewellClark) | [dotnet/runtime #45552](https://github.com/dotnet/runtime/issues/45552) | Use `String.Equals` instead of `String.Compare` |
| [Meik Tranel](https://github.com/MeikTranel) | [dotnet/runtime #47180](https://github.com/dotnet/runtime/issues/47180) | Use `String.Contains(char)` instead of `String.Contains(String)` |

Thanks [Meik Tranel](https://github.com/MeikTranel) and [Newell Clark](https://github.com/NewellClark).

## .NET SDK: Enable custom guards for Platform Compatibility Analyzer

[The CA1416 Platform Compatibility analyzer](https://docs.microsoft.com/dotnet/standard/analyzers/platform-compat-analyzer) already recognizes platform guards using the methods in OperatingSystem/RuntimeInformation, such as `OperatingSystem.IsWindows` and `OperatingSystem.IsWindowsVersionAtLeast`. However, [the analyzer does not recognize any other guard possibilities](https://github.com/dotnet/runtime/issues/44922) like the platform check result cached in a field or property, or complex platform check logic is defined in a helper method.

For [allowing custom guard possibilities](https://github.com/dotnet/runtime/issues/51541) we [added new attributes](https://github.com/dotnet/roslyn-analyzers/pull/5087) `SupportedOSPlatformGuard` and `UnsupportedOSPlatformGuard` for annotating the custom guard members with the corresponding platform name and/or version. This annotation is recognized and respected by the Platform Compatibility analyzer's flow analysis logic.

### Usage

```cs 
    [UnsupportedOSPlatformGuard("browser")] // The platform guard attribute
#if TARGET_BROWSER
    internal bool IsSupported => false;
#else
    internal bool IsSupported => true;
#endif

    [UnsupportedOSPlatform("browser")]
    void ApiNotSupportedOnBrowser() { }

    void M1()
    {
        ApiNotSupportedOnBrowser();  // Warns: This call site is reachable on all platforms.'ApiNotSupportedOnBrowser()' is unsupported on: 'browser'

        if (IsSupported)
        {
            ApiNotSupportedOnBrowser();  // Not warn
        }
    }

    [SupportedOSPlatform("Windows")]
    [SupportedOSPlatform("Linux")]
    void ApiOnlyWorkOnWindowsLinux() { }

    [SupportedOSPlatformGuard("Linux")]
    [SupportedOSPlatformGuard("Windows")]
    private readonly bool _isWindowOrLinux = OperatingSystem.IsLinux() || OperatingSystem.IsWindows();

    void M2()
    {
        ApiOnlyWorkOnWindowsLinux();  // This call site is reachable on all platforms.'ApiOnlyWorkOnWindowsLinux()' is only supported on: 'Linux', 'Windows'.

        if (_isWindowOrLinux)
        {
            ApiOnlyWorkOnWindowsLinux();  // Not warn
        }
    }
}
```

## Windows Forms: default font

You can now [set a default font for an application](https://github.com/dotnet/winforms/pull/4911) with `Application.SetDefaultFont`. The pattern you use is similar to setting high dpi or visual styles.

```diff
class Program
{
    [STAThread]
    static void Main()
    {
        Application.SetHighDpiMode(HighDpiMode.SystemAware);
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

+       Application.SetDefaultFont(new Font(new FontFamily("Microsoft Sans Serif"), 8f));

        Application.Run(new Form1());
    }
}
```

Here are two examples after setting the default font (with different fonts).

Microsoft Sans Serif, 8pt:

![image](https://user-images.githubusercontent.com/4403806/118235486-29abe980-b4d8-11eb-9a57-b7e559201a83.png)

Chiller, 12pt:

![image](https://user-images.githubusercontent.com/4403806/118235457-2153ae80-b4d8-11eb-9183-2a1cc5aa82a1.png)

The [default font was updated in .NET Core 3.0](https://github.com/dotnet/winforms/pull/656). However that change introduced a significant hurdle for some users migrating .NET Framework apps to .NET Core. This new change makes it straightforward to choose the desired font for an app and removes that migration hurdle.

## Libraries: Microsoft.Extensions

We've been improving `Microsoft.Extensions` APIs this release. In Preview 5, we've focused on hosting and dependency injection. In Preview 4, we added a [compile-time source generator for logging](https://devblogs.microsoft.com/dotnet/announcing-net-6-preview-4/#microsoft-extensions-logging-compile-time-source-generator).

Credit to Martin Björkström](https://github.com/bjorkstromm) for [dotnet/runtime #51840 (AsyncServiceScope)](https://github.com/dotnet/runtime/pull/51840).

### Hosting - ConfigureHostOptions API

We added a new ConfigureHostOptions API on IHostBuilder to make application setup simpler (e.g. configuring the shutdown timeout):

```c#
using HostBuilder host = new()
    .ConfigureHostOptions(o =>
    {
        o.ShutdownTimeout = TimeSpan.FromMinutes(10);
    })
    .Build();

host.Run();
```

Prior to Preview 5, configuring the host options was a bit more complicated:

```c#
using HostBuilder host = new()
    .ConfigureServices(services =>
    {
        services.Configure<HostOptions>(o =>
        {
            o.ShutdownTimeout = TimeSpan.FromMinutes(10);
        });
    })
    .Build();

host.Run();
```

### Dependency Injection - CreateAsyncScope APIs

You might have noticed that disposal of a service provider will throw an `InvalidOperationException` when it happens to register an IAsyncDisposable service.

The new `CreateAsyncScope` API provides a straightforward solution, as you can see in the following example:

```c#
await using (var scope = provider.CreateAsyncScope())
{
    var foo = scope.ServiceProvider.GetRequiredService<Foo>();
}
```

The following example demonstrate the existing problem case and then the previous suggested workaround.

```c#
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

await using var provider = new ServiceCollection()
        .AddScoped<Foo>()
        .BuildServiceProvider();

// This using can throw InvalidOperationException
using (var scope = provider.CreateScope())
{
    var foo = scope.ServiceProvider.GetRequiredService<Foo>();
}

class Foo : IAsyncDisposable
{
    public ValueTask DisposeAsync() => default;
}
```

You can workaround the exception by casting the returned scope to `IAsyncDisposable`.

```c#
var scope = provider.CreateScope();
var foo = scope.ServiceProvider.GetRequiredService<Foo>();
await ((IAsyncDisposable)scope).DisposeAsync();
```

`CreateAsyncScope` solves this problem, enabling you to safely use the `using` statement.

## Libraries: `JsonSerializer` Source generation

The backbone of nearly all .NET serializers is reflection. Reflection is a great capability for certain scenarios, but not as the basis of high-performance cloud-native applications (which typically (de)serialize and process a lot of JSON documents). Reflection is a problem for [startup](https://github.com/dotnet/runtime/issues/1568), memory usage, and [assembly trimming](https://github.com/dotnet/runtime/issues/45441).

The alternative to runtime reflection is compile-time [source generation](https://devblogs.microsoft.com/dotnet/new-c-source-generator-samples/). [Source generators](https://devblogs.microsoft.com/dotnet/introducing-c-source-generators/) generate C# source files that can be compiled as part of the library or application build. Generating source code at compile time can provide many benefits to .NET applications, including improved performance.

In .NET 6, we are including a new source generator as part of `System.Text.Json`. The JSON source generator works in conjunction with `JsonSerializer`, and can be configured in multiple ways. It's your decision whether you use the new source generator. It can provide the following benefits:

* Reduce start-up time
* Improve serialization throughput
* Reduce private memory usage
* Remove runtime use of `System.Reflection` and `System.Reflection.Emit`
* Allows for trim-compatible JSON serialization

For example, instead of dynamically generating methods at runtime to get and set class properties during (de)serialization using `Reflection.Emit` (which uses private memory and has start-up costs), a source generator can generate code that more simply and efficiently assigns or retrieves a value directly to/from properties, which is lightning fast.

You can try out the source generator by using the latest preview version of the [System.Text.Json NuGet package](https://www.nuget.org/packages/System.Text.Json). We are working on a [proposal](https://github.com/dotnet/designs/pull/181) for including source generators within the SDK.

### Generating optimized serialization logic

By default, the JSON source generator emits serialization logic for the given serializable types. This delivers higher performance than using the existing `JsonSerializer` methods by generating source code that uses `Utf8JsonWriter` directly. In short, source generators offer a way of giving you a different implementation at compile-time in order to make the runtime experience better.

Zooming out, `JsonSerializer` is a powerful tool which has many features (and even more coming!) that can improve the (de)serialization of .NET types from/into the JSON format. It is fast, but can have some performance overhead when only a subset of features are needed for a serialization routine. Going forward, we will update `JsonSerializer` and the new source generator together.

Given a simple type:

```cs
namespace Test
{
    internal class JsonMessage
    {
        public string Message { get; set; }
    }
}
```

The source generator can be configured to generate serialization logic for instances of the example `JsonMessage` type. Note that the class name `JsonContext` is arbitrary. You can use whichever class name you want for the generated source.

```cs
using System.Text.Json.Serialization;

namespace Test
{
    [JsonSerializable(typeof(JsonMessage)]
    internal partial class JsonContext : JsonSerializerContext
    {
    }
}
```

We have defined a [set of `JsonSerializer` features](https://github.com/dotnet/runtime/blob/e4751aee57c1089cf5aba40256a0d7b5461f691c/src/libraries/System.Text.Json/Common/JsonSerializerOptionsAttribute.cs) that are supported with the source generation mode that provides the best serialization throughput, via `JsonSerializerOptionsAttribute`. These features can be specified to the source generator ahead of time, to avoid extra checks at runtime. If the attribute is not used, then default `JsonSerializationOptions` are assumed at runtime.

As part of the build, the source generator augments the `JsonContext` partial class with the following shape:

```cs
internal partial class JsonContext : JsonSerializerContext
{
    public static JsonContext Default { get; }

    public JsonTypeInfo<JsonMessage> JsonMessage { get; }

    public JsonContext(JsonSerializerOptions options) { }

    public override JsonTypeInfo GetTypeInfo(Type type) => ...;
}
```

The serializer invocation with this mode could look like the following example. This example provides the best possible performance.

```cs
using MemoryStream ms = new();
using Utf8JsonWriter writer = new(ms);

JsonContext.Default.JsonMessage.Serialize(writer, new JsonMessage { "Hello, world!" });
writer.Flush();

// Writer contains:
// {"Message":"Hello, world!"}
```

Alternatively, you can continue to use `JsonSerializer`, and instead pass an instance of the generated code to it, with `JsonContext.Default.JsonMessage`.

```cs
JsonSerializer.Serialize(jsonMessage, JsonContext.Default.JsonMessage);
```

Here's a similar use, with a different overload.

```cs
JsonSerializer.Serialize(jsonMessage, typeof(JsonMessage), JsonContext.Default);
```

The difference between these two overloads is that the first is using the typed metadata implementation -- `JsonTypeInfo<T>` -- and the second one is using a more general untyped implementation that does type tests to determine if a typed implementation exists within the context instance. It is a little slower (due to the type tests), as a result. If there is not a source-generated implementation for a given type, then the serializer throws a `NotSupportedException`. It does not fallback to a reflection-based implementation (as an explicit design choice).

The fastest and most optimized source generation mode -- based on `Utf8JsonWriter` -- is currently only available for serialization. Similar support for deserialization -- based on `Utf8JsonReader` -- may be provided in the future depending on your feedback.

However, the source generator also emits type-metadata initialization logic that can benefit deserialization as well. To deserialize an instance of `JsonMessage` using pre-generated type metadata, you can do the following:

```cs
JsonSerializer.Deserialize(json, JsonContext.Default.JsonMessage);
```

Similar to serialization above, you might also write:

```cs
JsonSerializer.Deserialize(json, typeof(JsonMessage), JsonContext.Default);
```

### Additional notes

- Multiple types can be included for source generation via `[JsonSerializable]` on a derived, partial `JsonSerializerContext` instance, not just one.
- The source generator also supports nested object and collection members on objects, not just primitive types.

## Libraries: WebSocket Compression

Compression is important for any data transmitted over a network. [WebSockets now enable compression](https://github.com/dotnet/runtime/issues/20004). We used an implementation of `permessage-deflate` extension for WebSockets, [RFC 7692](https://tools.ietf.org/html/rfc7692). It allows compressing WebSockets message payloads using DEFLATE algorithm.

This feature was one of the top user requests for Networking on GitHub. You can follow our journey to providing that API via [API review 1](https://github.com/dotnet/runtime/issues/31088#issuecomment-754854011) and [API review 2](https://github.com/dotnet/runtime/issues/31088#issuecomment-810509834).

Credit to [Ivan Zlatanov](https://github.com/zlatanov). Thanks Ivan!

We realized that using compression together with encryption may lead to attacks, like [CRIME](https://en.wikipedia.org/wiki/CRIME) and [BREACH](https://en.wikipedia.org/wiki/BREACH). It means that a secret cannot be sent together with user-generated data in a single compression context, otherwise that secret could be extracted. To bring user's attention to these implications and help them weigh the risks, we renamed our API to `DangerousDeflateOptions`. We also added the ability to turn off compression for specific messages, so if the user would want to send a secret, they could do that securely without compression.

There was also a [follow-up](https://github.com/dotnet/runtime/pull/52022) by [Ivan](https://github.com/zlatanov) that reduced the memory footprint of the WebSocket when compression is disabled by about 27%.

Enabling the compression from the client side is easy, see the example below. However, please bear in mind that the server can negotiate the settings, e.g. request smaller window, or deny the compression completely.

```c#
var cws = new ClientWebSocket();
cws.Options.DangerousDeflateOptions = new WebSocketDeflateOptions()
{
    ClientMaxWindowBits = 10,
    ServerMaxWindowBits = 10
};
```

[WebSocket compression support for ASP.NET Core](https://github.com/dotnet/aspnetcore/issues/2715) was also recently added. It will be included in an upcoming preview.

## Libraries: Socks proxy support

[SOCKS](https://en.wikipedia.org/wiki/SOCKS) is a proxy server implementation that can process any TCP or UDP traffic, making it a very versatile system. It is a [long-standing community request](https://github.com/dotnet/runtime/issues/17740) that has been [added to .NET 6](https://github.com/dotnet/runtime/pull/48883).

This change adds support for Socks4, Socks4a, and Socks5. For example, it enables testing external connections via SSH or [connecting to the Tor network](https://en.wikipedia.org/wiki/SOCKS#Usage).

The `WebProxy` class now accepts `socks` schemes, as you can see in the following example.

```c#
var handler = new HttpClientHandler
{
    Proxy = new WebProxy("socks5://127.0.0.1", 9050)
};
var httpClient = new HttpClient(handler);
```

Credit to [Huo Yaoyuan](https://github.com/huoyaoyuan). Thanks Huo!

## Libraries: Support for OpenTelemetry Metrics

We've been [adding support for OpenTelemetry](https://devblogs.microsoft.com/dotnet/opentelemetry-net-reaches-v1-0/) for the last couple .NET versions, as part of our focus on [observability](https://devblogs.microsoft.com/dotnet/conversation-about-diagnostics/). In .NET 6, we're adding [support](https://github.com/dotnet/runtime/issues/44445) for the [OpenTelemetry Metrics API](https://github.com/open-telemetry/opentelemetry-specification/blob/main/specification/metrics/api.md). By adding support for OpenTelemetry, your apps can seamlessly interoperate with other [OpenTelemetry](https://opentelemetry.io) systems.

[System.Diagnostics.Metrics](https://github.com/dotnet/designs/blob/main/accepted/2021/System.Diagnostics/Metrics-Design.md) is the .NET implementation of the [OpenTelemetry Metrics API specification](https://github.com/open-telemetry/opentelemetry-specification/blob/main/specification/metrics/api.md). The Metrics APIs are designed explicitly for processing raw measurements, generally with the intent to produce continuous summaries of those measurements, efficiently and simultaneously.

The APIs include the `Meter` class which can be used to create instrument objects (e.g. Counter). The APIs expose four instrument classes: `Counter`, `Histogram`, `ObservableCounter`, and `ObservableGauge` to support different metrics scenarios. Also, the APIs expose the `MeterListener` class to allow listening to the instrument's recorded measurement for aggregation and grouping purposes.

The [OpenTelemetry .NET implementation](https://github.com/open-telemetry/opentelemetry-dotnet) will be extended to use these new APIs, which add support for Metrics observability scenarios.

### Library Measurement Recording Example

```C#
    Meter meter = new Meter("io.opentelemetry.contrib.mongodb", "v1.0");
    Counter<int> counter = meter.CreateCounter<int>("Requests");
    counter.Add(1);
    counter.Add(1, KeyValuePair.Create<string, object>("request", "read"));
```

### Listening Example

```C#
    MeterListener listener = new MeterListener();
    listener.InstrumentPublished = (instrument, meterListener) =>
    {
        if (instrument.Name == "Requests" && instrument.Meter.Name == "io.opentelemetry.contrib.mongodb")
        {
            meterListener.EnableMeasurementEvents(instrument, null);
        }
    };
    listener.SetMeasurementEventCallback<int>((instrument, measurement, tags, state) =>
    {
        Console.WriteLine($"Instrument: {instrument.Name} has recorded the measurement {measurement}");
    });
    listener.Start();
```

### Libraries: BigInteger Performance

[Parsing of BigIntegers](https://github.com/dotnet/runtime/pull/47842) from both decimal and hexadecimal strings has been improved. We see [improvements of up to 89%](https://github.com/DrewScoggins/performance-2/issues/5765), as demonstrated in the following chart.

![graph](https://camo.githubusercontent.com/4ebb3bde09bcf4e6217f1337fc16b4c8df80d1047ba2d18e7422f0dfce1244b2/68747470733a2f2f707673636d6475706c6f61642e626c6f622e636f72652e77696e646f77732e6e65742f6175746f66696c657265706f72742f6175746f66696c657265706f7274732f30355f31315f323032312f726566732f68656164732f6d61696e5f7836345f57696e646f777325323031302e302e31383336325f496d70726f76656d656e742f53797374656d2e4e756d65726963732e54657374732e506572665f426967496e74656765725f342e706e67)

Credit to [Joseph Da Silva](https://github.com/jfd16). Thanks Joseph!

## Libraries: `Vector<T>` now supports `nint` and `nuint`

`Vector<T>` [now supports the `nint` and `nuint` primitive types](https://github.com/dotnet/runtime/pull/50832), added in C# 9. For example, this change should make it simpler to use SIMD instructions with pointers or platform-dependent lengths.

## Libraries: Support for OpenSSL 3

.NET cryptography APIs support using [OpenSSL 3](https://wiki.openssl.org/index.php/OpenSSL_3.0) as the preferred native cryptography provider on Linux. .NET 6 will use OpenSSL 3 if it is available. Otherwise, it will use OpenSSL 1.x.

## Libraries: Add support ChaCha20/Poly1305 cryptography algorithm

The `ChaCha20Poly1305` class has been added to `System.Security.Cryptography`. In order to use the ChaCha20/Poly1305 algorithm, it must be supported by the underlying operating system. The static `IsSupported` property can be used to determine if the algorithm is supported in a given context.

* [Linux](https://github.com/dotnet/runtime/pull/52522): requires OpenSSL 1.1 or hßigher.
* [Windows](https://github.com/dotnet/runtime/pull/52030): build 20142 or higher (currently requires the Dev "insider" channel)

Credit to [Kevin Jones](https://github.com/vcsjones) for the Linux support. Thanks Kevin!

## Interop: Objective-C interoperability support

The team has been [adding Objective-C support](https://devblogs.microsoft.com/dotnet/conversation-about-net-interop/#the-team-is-working-on-adding-interop-support-for-objective-c-right-now-whats-the-purpose-of-that-project-and-whats-interesting-and-unique-about-it), with the goal of having a single [Objective-C interop implementation](hhttps://github.com/dotnet/designs/blob/main/accepted/2021/objectivec-interop.md) for .NET. Up until now, the [Objective-C](https://en.wikipedia.org/wiki/Objective-C) interop system was built around the Mono embedding API but we decided it wasn't the right approach to share across runtimes. As a result we've create a new .NET API that will enable a single Objective-C interop experience that will eventually work on both runtimes.

This new API for Objective-C interop has brought immediate support in both runtimes for [`NSAutoreleasePool`](https://developer.apple.com/documentation/foundation/nsautoreleasepool), which [enables support](https://github.com/dotnet/runtime/issues/44659) for Cocoa’s reference-counted memory management system. You can now [configure whether you want each managed thread](https://docs.microsoft.com/dotnet/core/run-time-config/threading#autoreleasepool-for-managed-threads) to have an implicit `NSAutoreleasePool`. This enables the release of Cocoa objects on a per-thread basis.

## Diagnostics (EventPipe/DiagnosticsServer) - MonoVM

A lot of [diagnostics features](https://devblogs.microsoft.com/dotnet/conversation-about-diagnostics/) have been added into MonoVM since beginning of .NET 6. This has enabled features like managed EventSource/EventListener, EventPipe and DiagnosticsServer. It has enabled using diagnostics tools like `dotnet-trace`, `dotnet-counters`, `dotnet-stacks` for apps running on mobile devices (iOS/Android) as well as desktop.

These new features opens up ability to analyse nettrace files generated by MonoVM in tools like PrefView/SpeedScope/Chromium, [dotnet-trace](https://docs.microsoft.com/dotnet/core/diagnostics/dotnet-trace#dotnet-trace-convert), or [writing custom parsers](https://github.com/dotnet/diagnostics/blob/main/documentation/diagnostics-client-library-instructions.md) using libraries like [TraceEvent](https://www.nuget.org/packages/Microsoft.Diagnostics.Tracing.TraceEvent/).

We will continue to include more features going forward, primarily focusing on SDK integration and adapting more native runtime events (`Microsoft-Windows-DotNETRuntime`) into MonoVM enabling more events in nettrace files.

The following features are now in place:

* [Share native EventPipe/DiagnosticsServer library](https://github.com/dotnet/runtime/tree/main/src/native/eventpipe) between MonoVM and CoreCLR.
* Add TCP/IP support into DiagnosticsServer and build MonoVM iOS/Android runtime packs leveraging that configuration. Needed in order to support mobile platforms.
* BCL EventSources runs on MonoVM emitting events into EventPipe.
* BCL Runtime counters emitted by `System.Diagnostics.Tracing.RuntimeEventSource` wired up on MonoVM, consumable from tools like `dotnet-counters`.
* Custom EventSources runs on MonoVM, emitting custom events into EventPipe, consumable from tools like `dotnet-trace`.
* Custom event counters runs on MonoVM, emitting custom counter events into EventPipe, consumable from tools like `dotnet-counters`.
* Sample profiler is implemented on MonoVM emitting events into EventPipe. Opens up abilities to do [CPU profiling on MonoVM](https://docs.microsoft.com/dotnet/core/diagnostics/debug-highcpu) using `dotnet-trace`.
* Implementation of `dotnet-dsrouter` diagnostics tool, enables use of existing diagnostic tooling like, `dotnet-trace`, `dotnet-counters`, `dotnet-stack` together with MonoVM running on mobile targets, without any need to change existing tooling. `dotnet-dsrouter` runs a local IPC server routing all traffic from diagnostic tooling over to DiagnosticsServer running in MonoVM on simulator/device.
* Implementation of EventPipe/DiagnosticsServer in MonoVM using [component-based architecture](https://github.com/dotnet/runtime/blob/main/docs/design/mono/components.md).
* Implementation/extension of [diagnostics environment based file session](https://github.com/dotnet/runtime/blob/main/docs/design/mono/diagnostics-tracing.md#application-running-single-file-based-eventpipe-session).

### iOS CPU sampling (SpeedScope)

The following image demonstrates part of an iOS start up CPU sampling session viewed in SpeedScope.

![speedscope-p5](https://user-images.githubusercontent.com/11529140/121526570-cd19ec80-c9f9-11eb-82d3-72a1dae4f8e7.png)

### Android CPU sampling (PerfView)

The following image demonstrates Android CPU sampling viewed in PerfView (main thread in infinite sleep).

![prefview-p5](https://user-images.githubusercontent.com/11529140/121526154-6c8aaf80-c9f9-11eb-943f-d33a99761e72.png)

## Runtime: CodeGen

The following changes have been made in RyuJIT.

### Community contributions

- Delete the unused dummyBB variable https://github.com/dotnet/runtime/pull/52155 
- Delete unused functions reading integers in big-endian format https://github.com/dotnet/runtime/pull/52154 
- Pass TYP_FLOAT to gtNewDconNode instead of creating a new scope https://github.com/dotnet/runtime/pull/51928

Thanks to [@SingleAccretion](https://github.com/SingleAccretion) for these contributions.

### Dynamic PGO https://github.com/dotnet/runtime/issues/43618

- Revise inlinee scale computations https://github.com/dotnet/runtime/pull/51593 
- Update optReachable with excluded block check https://github.com/dotnet/runtime/pull/51842 
- Generalize the branch around empty flow optimization https://github.com/dotnet/runtime/pull/51409 
- Add MCS jitflags support for the new GetLikelyClass PGO record type https://github.com/dotnet/runtime/pull/51578 
- Generalize checking for valid IR after a tail call to support crossgen2 determinism https://github.com/dotnet/runtime/pull/51903 
- More general value class devirtualization https://github.com/dotnet/runtime/pull/52210 
- Chained guarded devirtualization https://github.com/dotnet/runtime/pull/51890 

  <img src="https://user-images.githubusercontent.com/63486087/121260587-5f898700-c866-11eb-98cf-1d1ff8cc4675.png" width="600" height="400">

### JIT Loop Optimizations https://github.com/dotnet/runtime/issues/43549 
- Improved loop inversion shows good performance improvement in BenchE https://github.com/dotnet/runtime/pull/52347

  <img src="https://user-images.githubusercontent.com/63486087/121261311-6d8bd780-c867-11eb-896e-922725a5fd17.png" 
width="600" height="120">

- Scale cloned loop block weights https://github.com/dotnet/runtime/pull/51901 
- Don't recompute preds lists during loop cloning to preserve existing profile data on the edges https://github.com/dotnet/runtime/pull/51757 
- Improve DOT flow graph dumping https://github.com/dotnet/runtime/pull/52329 
- Improve loop unrolling documentation https://github.com/dotnet/runtime/pull/52099 

### LSRA https://github.com/dotnet/runtime/issues/43318 
-   Include register selection heuristics in "Allocating Registers" table https://github.com/dotnet/runtime/pull/52513 

      The diff of old vs. new table:
      
      ![image](https://user-images.githubusercontent.com/63486087/121261697-e68b2f00-c867-11eb-8472-e013523041b4.png)

### Keep Structs in Register https://github.com/dotnet/runtime/issues/43867 
- Prepare JIT backend for structs in registers https://github.com/dotnet/runtime/pull/52039 
- Liveness fix for struct enreg https://github.com/dotnet/runtime/pull/51851 
- Improve struct inits to keep ASG struct(LCL_VAR, 0) as STORE_LCL_VAR struct(0) https://github.com/dotnet/runtime/pull/52292 

### Optimizations & Debugging experience

-  Recognize and handle Vector64/128/256<T> for nint/nuint https://github.com/dotnet/runtime/pull/52016 
- Add clrjit.natvis file for better debugging experience https://github.com/dotnet/runtime/pull/52668 

    The sample visualizer for jitstd::list<RefPosition> as well as RefPosition and the decomposition of registerAssignment inside it to show all the registers:

![image](https://user-images.githubusercontent.com/63486087/121262235-a4162200-c868-11eb-8edd-53771323e836.png)

### SIMD

Inlining of certain methods involving SIMD or HWIntrinsics should now have improved codegen and performance. We saw [improvements of up to 95%](https://github.com/DrewScoggins/performance-2/issues/5581).

![graph](https://camo.githubusercontent.com/b46e9125a88e892381f8cd29fa61b144b3bfcfc107052631f0a807807a8214f0/68747470733a2f2f707673636d6475706c6f61642e626c6f622e636f72652e77696e646f77732e6e65742f6175746f66696c657265706f72742f6175746f66696c657265706f7274732f30355f30345f323032312f726566732f68656164732f6d61696e5f7836345f57696e646f777325323031302e302e31383336325f496d70726f76656d656e742f53797374656d2e4e756d65726963732e54657374732e506572665f566563746f72335f322e706e67)

## Closing

.NET 6 Preview 5 is perhaps the biggest preview yet in terms of breadth and quantity of features. You can see how much Roslyn features are affecting low-level libraries features, with source generators and analyzers. The future has truly arrived. We now have a very capable compiler toolchain that enables us to produce highly-optimized and correct code, and enables the exact same experience for your own projects.

Now is a great time to start testing .NET 6. It's still early enough for us to act on your feedback. It's hard to imagine given that while we're not shipping until [November 2021](https://www.dotnetconf.net) that the feedback window will soon narrow to high-severity issues only. The team works about one and a half previews ahead, and will soon switch to focusing primarily on quality issues. Please give .NET 6 a try if you can.

Thanks for being a .NET developer.
