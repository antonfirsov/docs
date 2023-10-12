---
post_title: Performance Improvements in ASP.NET Core 8
author1: brecon
post_slug: performance-improvements-in-aspnet-core-8
microsoft_alias: brecon
featured_image: asp-perf-8.jpg
categories: .NET, ASP.NET, ASP.NET Core
tags: asp.net core, performance, .net 8
summary: Performance improvements in ASP.NET Core 8
post_date: 2023-10-16 10:05:00
---

ASP.NET Core 8 and .NET 8 bring many exciting performance improvements. In this blog post, we will highlight some of the enhancements made in ASP.NET Core and show you how they can boost your web app's speed and efficiency. This is a continuation of last year's post on [Performance improvements in ASP.NET Core 7](https://devblogs.microsoft.com/dotnet/performance-improvements-in-aspnet-core-7/). And, of course, it continues to be inspired by [Performance Improvements in .NET 8](https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-8). Many of those improvements either indirectly or directly improve the performance of ASP.NET Core as well.

## Benchmarking Setup

We will use [BenchmarkDotNet](https://github.com/dotnet/benchmarkdotnet) for many of the examples in this blog post.

To setup a benchmarking project:
1. Create a new console app (`dotnet new console`)
2. Add a Nuget reference to BenchmarkDotnet (`dotnet add package BenchmarkDotnet`) version 0.13.8+
3. Change Program.cs to `var summary = BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run();`
4. Add the benchmarking code snippet below that you want to run
5. Run `dotnet run -c Release` and enter the number of the benchmark you want to run when prompted

Some of the benchmarks test internal types, and a self-contained benchmark cannot be written. In those cases we'll either reference numbers that are gotten by running the benchmarks in the repository (and link to the code in the repository), or we'll provide a simplified example to showcase what the improvement is doing.

There are also some cases where we will reference our end-to-end benchmarks which are public at https://aka.ms/aspnet/benchmarks. Although we only display the last few months of data so that the page will load in a reasonable amount of time.

## Servers

We have 3 server implementations in ASP.NET Core; Kestrel, Http.Sys, and IIS. The latter two are only usable on Windows and share a lot of code. Server performance is extremely important because it's what processes incoming requests and forwards them to your application code. The faster we can process a request, the faster you can start running application code.

### Kestrel

Header parsing is one of the first parts of processing done by a server for every request. Which means the performance is critical to allow requests to reach your application code as fast as possible.

In Kestrel we read bytes off the connection into a `System.IO.Pipelines.Pipe` which is essentially a list of `byte[]`s. When parsing headers we are reading from that list of `byte[]`s and have two different code paths. One for when the full header is inside a single `byte[]` and another for when a header is split across multiple `byte[]`s.

[dotnet/aspnetcore#45044](https://github.com/dotnet/aspnetcore/pull/45044) updated the second (slower) code path to avoid allocating a `byte[]` when parsing the header, as well as optimizes our `SequenceReader` usage to mostly use the underlying `ReadOnlySequence<byte>` which can be faster in some cases.

This resulted in a ~18% performance improvement for multi-span headers as well as making it allocation free which helps reduce GC pressure. The following microbenchmark is using internal types in Kestrel and isn't easy to isolate as a minimal sample. For those interested it is located [with the Kestrel source code](https://github.com/dotnet/aspnetcore/blob/8ad057426fa6a27cd648b05684afddab9d97d3d9/src/Servers/Kestrel/perf/Microbenchmarks/HttpParserBenchmark.cs#L73) and was run before and after the change.

|                 Method |     Mean |    Error |   StdDev |   Median |        Op/s | Gen 0 | Gen 1 | Gen 2 | Allocated |
|----------------------- |---------:|---------:|---------:|---------:|------------:|------:|------:|------:|----------:|
| MultispanUnicodeHeader - Before | 573.8 ns | 18.61 ns | 54.87 ns | 552.9 ns | 1,742,893.2 | - | - | - | 48 B |
| MultispanUnicodeHeader - After | 484.9 ns | 11.75 ns | 33.90 ns | 471.2 ns | 2,062,450.8 | - | - | - | - |

Below is an allocation profile of an end-to-end benchmark we run on our CI showing the different with this change. We reduced the `byte[]` allocations of the scenario by 73%. From 7.8GB to 2GB (during the lifetime of the benchmark run).
![shows byte[] allocations before and after change](multi-header-alloc.png)

[dotnet/aspnetcore#48368](https://github.com/dotnet/aspnetcore/pull/48368) replaced some internal custom vectorized code for ascii comparison checks with the new `Ascii` class in .NET 8. This allowed us to remove ~400 lines of code and take advantage of improvements like AVX512 and ARM AdvSIMD that are implemented in the `Ascii` code that we didn't have in Kestrel's implementation.

### Http.Sys

Near the end of 7.0 we removed some [extra thread pool dispatching in Kestrel](https://github.com/dotnet/aspnetcore/pull/43449) that improved performance significantly. More details are in [last years performance post](https://devblogs.microsoft.com/dotnet/performance-improvements-in-aspnet-core-7/#general-server). At the beginning of 8.0 we made similar changes to the Http.Sys server in [dotnet/aspnetcore#44409](https://github.com/dotnet/aspnetcore/pull/44409). This improved our [Json end to end benchmark](https://github.com/aspnet/Benchmarks/blob/e3095f4021fef7171bb3ae86616b9156df39b7bd/src/Benchmarks/Middleware/JsonMiddleware.cs) by 11% from ~469k to ~522k RPS.

Another change we made affects large responses especially in higher latency connections. [dotnet/aspnetcore#47776](https://github.com/dotnet/aspnetcore/pull/47776) adds an on-by-default option to enable Kernel-mode response buffering. This allows application writes to be buffered in the OS layer regardless of whether the client connection has acked previous writes or not, and then the OS can optimize sending the data by parallelizing writes and/or sending larger chunks of data at a time. The benefits are clear when using connections with higher latency.

To show a specific example we hosted a server in Sweden and a client in West Coast USA to create some latency in the connection. The following server code was used:
```csproj
var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseHttpSys(options =>
{
    options.UrlPrefixes.Add("http://+:12345");
    options.Authentication.Schemes = AuthenticationSchemes.None;
    options.Authentication.AllowAnonymous = true;
    options.EnableKernelResponseBuffering = true; // <-- new setting in 8.0
});

var app = builder.Build();

app.UseRouting();

app.MapGet("/file", () =>
{
    return TypedResults.File(File.Open("pathToLargeFile", FileMode.Open, FileAccess.Read));
});

app.Run();
```
The latency was around 200ms (round-trip) between client and server and the server was responding to client requests with a 212MB file.
When setting `HttpSysOptions.EnableKernelResponseBuffering` to false the file download took ~11 minutes. And when setting it to true it took ~30 seconds to download the file. That's a massive improvement, ~22x faster in this specific scenario!

More details on how response buffering works can be found in [this blog post](https://learn.microsoft.com/archive/blogs/wndp/buffering-in-http-sys).

[dotnet/aspnetcore#44561](https://github.com/dotnet/aspnetcore/pull/44561) refactors the internals of response writing in Http.Sys to remove a bunch of `GCHandle` allocations and conveniently removes a `List<GCHandle>` that was used to track handles for freeing. It does this by allocating and writing directly to `NativeMemory` when writing headers. By not pinning managed memory we are reducing GC pressure and helping reduce heap fragmentation. A downside is that we need to be extra careful to free the memory because the allocations are no longer tracked by the GC.
Running a simple web app and tracking GCHandle usage shows that in 7.0 a small response with 4 headers was using 8 `GCHandle`s per request, and when adding more headers it was using 2 more `GCHandle`s per header. In 8.0 the same app was using 4 `GCHandle`s per request, regardless of the number of headers.

[dotnet/aspnetcore#45156](https://github.com/dotnet/aspnetcore/pull/45156) by [@ladeak](https://github.com/ladeak) improved the implementation of `HttpContext.Request.Headers.Keys` and `HttpContext.Request.Headers.Count` in Http.Sys, which is also the same implementation used by IIS so double win. Before, those properties had generic implementations that used `IEnumerable` and linq expressions. Now they manually count and minimize allocations, making accessing `Count` completely allocation free.
This benchmark uses internal types, so I'll link to [the microbenchmark source](https://github.com/dotnet/aspnetcore/blob/8279d3f8ee47d5e249ef12a99e4d1cacfd5209ec/src/Servers/HttpSys/perf/Microbenchmarks/RequestHeaderBenchmarks.cs#L12) instead of providing a standalone microbenchmark.

Before:
|            Method |       Mean |    Error |   StdDev |        Op/s |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|------------------ |-----------:|---------:|---------:|------------:|-------:|------:|------:|----------:|
| CountSingleHeader |   381.3 ns |  5.63 ns |  4.99 ns | 2,622,896.1 | 0.0010 |     - |     - |     176 B |
| CountLargeHeaders | 3,293.4 ns | 16.77 ns | 14.86 ns |   303,639.9 | 0.0534 |     - |     - |   9,032 B |
|  KeysSingleHeader |   483.5 ns |  1.76 ns |  1.56 ns | 2,068,299.5 | 0.0019 |     - |     - |     344 B |
|  KeysLargeHeaders | 3,559.4 ns | 13.61 ns | 12.06 ns |   280,947.4 | 0.0572 |     - |     - |   9,648 B |

After:
|            Method |       Mean |   Error |  StdDev |        Op/s |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|------------------ |-----------:|--------:|--------:|------------:|-------:|------:|------:|----------:|
| CountSingleHeader |   249.1 ns | 3.80 ns | 3.56 ns | 4,014,316.0 |      - |     - |     - |         - |
| CountLargeHeaders |   278.3 ns | 1.81 ns | 1.69 ns | 3,593,059.3 |      - |     - |     - |         - |
|  KeysSingleHeader |   506.6 ns | 3.00 ns | 2.66 ns | 1,974,125.9 |      - |     - |     - |      32 B |
|  KeysLargeHeaders | 1,314.6 ns | 6.28 ns | 5.87 ns |   760,689.5 | 0.0172 |     - |     - |   2,776 B |

## Native AOT

[Native AOT](https://learn.microsoft.com/dotnet/core/deploying/native-aot/) was first introduced in .NET 7 and only worked with console applications and a limited number of libraries.
In .NET 8.0 we've improved the number of libraries that are supported in Native AOT as well as [added support for ASP.NET Core](https://learn.microsoft.com/aspnet/core/fundamentals/native-aot?view=aspnetcore-8.0#aspnet-core-and-native-aot-compatibility) applications. AOT apps can have minimized disk footprint, reduced startup times, and reduced memory demand. But before we talk about AOT more and show some numbers, we should talk about a prerequisite, trimming.

Starting in .NET 6 [trimming applications](https://learn.microsoft.com/dotnet/core/deploying/trimming/trim-self-contained) became a fully supported feature. Enabling this feature with `<PublishTrimmed>true</PublishTrimmed>` in your `.csproj` enables the trimmer to run during publish and remove code your application isn't using. This can result in smaller deployed application sizes, useful in scenarios where you are running on memory constrained devices. Trimming isn't free though, libraries might need to annotate types and method calls to tell the trimmer about code being used that the trimmer can't determine, otherwise the trimmer might trim away code you're relying on and your app won't run as expected. The trimmer will [raise warnings](https://learn.microsoft.com/dotnet/core/deploying/trimming/fixing-warnings) when it sees code that might not be compatible with trimming. Until .NET 8 the `<TrimMode>` property for publishing web apps was set to `partial`. This meant that only assemblies that explicitly stated they supported trimming would be trimmed. Now in 8.0, `full` is used for `<TrimMode>` which means all assemblies used by the app will be trimmed. These settings are documented in the [trimming options docs](https://learn.microsoft.com/dotnet/core/deploying/trimming/trimming-options#trimming-granularity).

In .NET 6 and .NET 7 a lot of libraries weren't compatible with trimming yet, notably ASP.NET Core libraries. If you tried to publish a simple ASP.NET Core app in 7.0 you would get a bunch of trimmer warnings because most of ASP.NET Core didn't support trimming yet.

The following is an ASP.NET Core app to show trimming in net7.0 vs. net8.0. All the numbers are for a windows publish.
```xml
<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <TargetFrameworks>net7.0;net8.0</TargetFrameworks>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>

</Project>
```

```csharp
// dotnet publish --self-contained --runtime win-x64 --framework net7.0 -p:PublishTrimmed=true -p:PublishSingleFile=true --configuration Release
var app = WebApplication.Create();

app.Run((c) => c.Response.WriteAsync("hello world"));

app.Run();
```

|    TFM |   Trimmed | Warnings |   App Size |  Publish duration |
|------- |----------:|---------:|-----------:|------------------:|
| net7.0 |     false |        0 |     88.4MB |           3.9 sec |
| net8.0 |     false |        0 |     90.9MB |           3.9 sec |
| net7.0 |      true |       16 |     28.9MB |          16.4 sec |
| net8.0 |      true |        0 |     17.3MB |          10.8 sec |

In addition to no more warnings when publishing trimmed in net8.0, the app size is smaller because we've annotated more libraries so the linker can find more code that isn't being used by the app. Part of annotating the libraries involved analyzing what code is being kept by the trimmer and changing code to improve what can be trimmed. You can see numerous PRs to help this effort; [dotnet/aspnetcore#47567](https://github.com/dotnet/aspnetcore/issues/47567), [dotnet/aspnetcore#47454](https://github.com/dotnet/aspnetcore/pull/47454), [dotnet/aspnetcore#46082](https://github.com/dotnet/aspnetcore/pull/46082), [dotnet/aspnetcore#46015](https://github.com/dotnet/aspnetcore/pull/46015), [dotnet/aspnetcore#45906](https://github.com/dotnet/aspnetcore/pull/45906), [dotnet/aspnetcore#46020](https://github.com/dotnet/aspnetcore/pull/46020), and many more.

The `Publish duration` field was calculated using the `Measure-Command` in powershell (and deleting /bin/ and /obj/ between every run). As you can see, enabling trimming can increase the publish time because the trimmer has to analyze the whole program to see what it can remove, which isn't a free operation.

We also introduced two smaller versions of `WebApplication` if you want even smaller apps via `CreateSlimBuilder` and `CreateEmptyBuilder`.
Changing the previous app to use `CreateSlimBuilder`:
```csharp
// dotnet publish --self-contained --runtime win-x64 --framework net8.0 -p:PublishTrimmed=true -p:PublishSingleFile=true --configuration Release
var builder = WebApplication.CreateSlimBuilder(args);
var app = builder.Create();

app.Run((c) => c.Response.WriteAsync("hello world"));

app.Run();
```
will result in an app size of 15.5MB.
And then going one step further with `CreateEmptyBuilder`:
```csharp
// dotnet publish --self-contained --runtime win-x64 --framework net8.0 -p:PublishTrimmed=true -p:PublishSingleFile=true --configuration Release
var builder = WebApplication.CreateEmptyBuilder(new WebApplicationOptions()
{
    Args = args
});
var app = builder.Create();

app.Run((c) => c.Response.WriteAsync("hello world"));

app.Run();
```
will result in an app size of 13.7MB, although in this case the app won't work because there is no server implementation registered. So if we add Kestrel via `builder.WebHost.UseKestrelCore();` the app size becomes 15MB.

|    TFM |      Builder |   App Size |
|------- |-------------:|-----------:|
| net8.0 |       Create |     17.3MB |
| net8.0 |         Slim |     15.5MB |
| net8.0 |        Empty |     13.7MB |
| net8.0 | Empty+Server |     15.0MB |

Note that both these APIs are available starting in 8.0 and remove a lot of defaults so it's more pay for play.

Now that we've taken a small look at trimming and seen that 8.0 has more trim compatible libraries, let's take a look at Native AOT. Just like with trimming, if your app/library isn't compatible with Native AOT you'll get warnings when building for Native AOT and there are [additional limitations](https://learn.microsoft.com/dotnet/core/deploying/native-aot/?tabs=net8plus%2Cwindows#limitations-of-native-aot-deployment) to what works in Native AOT.

Using the same app as before, we'll enable Native AOT by adding `<PublishAot>true</PublishAot>` to our csproj.

|    TFM |   AOT |   App Size |  Publish duration |
|------- |------:|-----------:|------------------:|
| net7.0 | false |     88.4MB |           3.9 sec |
| net8.0 | false |     90.9MB |           3.9 sec |
| net7.0 |  true |       40MB |          71.7 sec |
| net8.0 |  true |     12.6MB |          22.7 sec |

And just like with trimming, we can test the `WebApplication` APIs that have less defaults enabled.

|    TFM |      Builder |   App Size |
|------- |-------------:|-----------:|
| net8.0 |       Create |     12.6MB |
| net8.0 |         Slim |      8.8MB |
| net8.0 |        Empty |      5.7MB |
| net8.0 | Empty+Server |      7.8MB |

That's pretty cool! A small net8.0 app is 90.9MB and when published as Native AOT it's 12.6MB, or as low as 7.8MB (assuming we want a server, which we probably do).

Now let's take a look at some other performance characteristics of a Native AOT app; startup speed, memory usage, and RPS.
In order to properly show E2E benchmark numbers we need to use a multi-machine setup so that the server and client processes don't steal CPU from each other and we don't have random processes running like you would for a local machine. I'll be using our internal benchmarking infrastructure that makes use of the benchmarking tool [crank](https://github.com/dotnet/crank/) and our aspnet-citrine-win and aspnet-citrine-lin machines for server and load respectively. Both machine specs are described in our benchmarks [readme](https://github.com/aspnet/Benchmarks/blob/a0577e58595b665d8e33a765991e6db71ab4ba9e/scenarios/README.md#profiles). And finally, I'll be using an [application](https://github.com/aspnet/Benchmarks/tree/a0577e58595b665d8e33a765991e6db71ab4ba9e/src/BenchmarksApps/BasicMinimalApi) that uses Minimal APIs to return a json payload. This app uses the Slim builder we showed earlier as well as sets `<InvariantGlobalization>true</InvariantGlobalization>` in the csproj.

If we run the app without any extra settings:
> crank --config https://raw.githubusercontent.com/aspnet/Benchmarks/main/scenarios/goldilocks.benchmarks.yml --config https://raw.githubusercontent.com/aspnet/Benchmarks/main/build/ci.profile.yml --config https://raw.githubusercontent.com/aspnet/Benchmarks/main/scenarios/steadystate.profile.yml --scenario basicminimalapivanilla --profile intel-win-app --profile intel-lin-load --application.framework net8.0 --application.options.collectCounters true

This gives us a ~293ms startup time, 444MB working set, and ~762k RPS.

If we run the same app but publish it as Native AOT:
> crank --config https://raw.githubusercontent.com/aspnet/Benchmarks/main/scenarios/goldilocks.benchmarks.yml --config https://raw.githubusercontent.com/aspnet/Benchmarks/main/build/ci.profile.yml --config https://raw.githubusercontent.com/aspnet/Benchmarks/main/scenarios/steadystate.profile.yml --scenario basicminimalapipublishaot --profile intel-win-app --profile intel-lin-load --application.framework net8.0 --application.options.collectCounters true

We get ~67ms startup time, 56MB working set, and ~681k RPS.
That's ~77% faster startup speed, ~87% lower working set, and ~12% lower RPS. The startup speed is expected because the app has already been optimized, meaning there is no JIT running to start optimizing code. Also, in non-Native AOT apps, because startup methods are likely only called once, tiered compilation will never run on the startup methods so they won't be as optimized as they could be, but in NativeAOT the startup method will be fully optimized. The working set is a bit surprising, it is lower because Native AOT apps by default run with the new Dynamic Adaptation To Application Sizes (DATAS) GC. This GC setting tries to maintain a balance between throughput and overall memory usage, which we can see it doing with an ~87% lower working set at the cost of some RPS. You can read more about the new GC setting in [Maoni0's blog](https://maoni0.medium.com/dynamically-adapting-to-application-sizes-2d72fcb6f1ea).

Let's also compare the Native AOT vs. non-Native AOT apps with the Server GC. So we'll add `--application.environmentVariables DOTNET_GCDynamicAdaptationMode=0` when running the Native AOT app.

This time we get ~64ms startup time, 403MB working set, and ~730k RPS. The startup time is still extremely fast because changing the GC doesn't affect that, our working set is closer to the non-Native AOT app but smaller due in part to not having the JIT compiler loaded and running, and our RPS is closer to the non-Native AOT app because we're using the Server GC which optimizes throughput more than memory usage.

|    AOT |     GC |  Startup | Working Set |   RPS |
|------- |-------:|---------:|------------:|------:|
|  false | Server |    293ms |       444MB |  762k |
|  false |  DATAS |    303ms |        77MB |  739k |
|   true | Server |     64ms |       403MB |  730k |
|   true |  DATAS |     67ms |        56MB |  681k |

Non-Native AOT apps have the JIT optimizing code while it's running, and starting in .NET 8 the JIT by default will make use of dynamic PGO, this is a really cool feature that Native AOT isn't able to benefit from and is one reason non-Native AOT apps can have more throughput than Native AOT apps. You can read more about dynamic PGO in the [.NET 8 performance blog](https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-8/#tiering-and-dynamic-pgo).

If you're willing to trade some publish size for potentially more optimized code you can pass `/p:OptimizationPreference=Speed` when building and publishing your Native AOT app. When we do this for our benchmark app (with Server GC) we get a publish size of 9.5MB instead of 8.9MB and 745k RPS instead of 730k.

The app we've been using makes use of Minimal APIs which by default isn't trim friendly. It does a lot of reflection and dynamic code generation that isn't statically analyzable so the trimmer isn't able to safely trim the app. So why don't we see warnings when we Native AOT publish this app? Because we wrote a source-generator called Request Delegate Generator (RDG) that replaces your `MapGet`, `MapPost`, etc. methods with trim friendly code. This source-generator is automatically used for ASP.NET Core apps when trimming/aot publishing. Which leads us into the next section where we dive into RDG.

### Request Delegate Generator

The Request Delegate Generator (RDG) is a [source-generator](https://learn.microsoft.com/dotnet/csharp/roslyn-sdk/source-generators-overview) created to make [Minimal APIs](https://learn.microsoft.com/aspnet/core/fundamentals/minimal-apis/overview) trimmer and Native AOT friendly. Without RDG, using Minimal APIs will result in many warnings and your app likely won't work as expected. Here is a quick example to show an endpoint that will result in an exception when using Native AOT without RDG but will work with RDG enabled (or when not using Native AOT).

```csharp
app.MapGet("/", (Bindable b) => "Hello world!");

public class Bindable
{
    public static ValueTask<Bindable?> BindAsync(HttpContext context, ParameterInfo parameter)
    {
        return new ValueTask<Bindable?>(new Bindable());
    }
}
```

This app throws when you send a `GET` request to `/` because the `Bindable.BindAsync` method is referenced via reflection and so the trimmer can't statically figure out that the method is being used and will remove it. Minimal APIs then sees the `MapGet` call as needing a request body which isn't allowed by default for `GET` calls.
Besides fixings warnings and making the app work as expected in Native AOT, we get improved first response time and reduced publish size.

Without RDG, the first time a request is made to the app is when all the expression trees are generated for all endpoints in the application. Because RDG generates the source for an endpoint at compile time, there is no expression tree generation needed, the code for a specific endpoint is already available and can execute immediately.

If we take the [app used earlier](https://github.com/aspnet/Benchmarks/tree/a0577e58595b665d8e33a765991e6db71ab4ba9e/src/BenchmarksApps/BasicMinimalApi) for benchmarking AOT and look at time to first request we get ~187ms when not running as AOT and without RDG. We then get ~130ms when we enable RDG. When publishing as AOT, the time to first request is ~60ms regardless of using RDG. But this app only has 2 endpoints, so let's add 1000 more endpoints and see the difference!

2 Routes:
|    AOT |   RDG | First Request | Publish Size |
|------- |------:|--------------:|-------------:|
|  false | false |         187ms |         97MB |
|  false |  true |         130ms |         97MB |
|   true | false |          60ms |      11.15MB |
|   true |  true |          60ms |       8.89MB |

1002 Routes:
|    AOT |   RDG | First Request | Publish Size |
|------- |------:|--------------:|-------------:|
|  false | false |        1082ms |         97MB |
|  false |  true |         176ms |         97MB |
|   true | false |         157ms |      11.15MB |
|   true |  true |          84ms |       8.89MB |

### Runtime APIs

In this section we'll be looking at changes that mainly involve updating to use new APIs introduced in .NET 8 in the [Base Class Library (BCL)](https://learn.microsoft.com/dotnet/standard/glossary#bcl).

#### SearchValues

[dotnet/aspnetcore#45300](https://github.com/dotnet/aspnetcore/pull/45300) by [@gfoidl](https://github.com/gfoidl), [dotnet/aspnetcore#47459](https://github.com/dotnet/aspnetcore/pull/47459), [dotnet/aspnetcore#49114](https://github.com/dotnet/aspnetcore/pull/49114), and [dotnet/aspnetcore#49117](https://github.com/dotnet/aspnetcore/pull/49117) all make use of the new `SearchValues` type which lets these code paths take advantage of optimized search implementations for the specific values being searched for. The `SearchValues` section of the [.NET 8 performance blog](https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-8/#searchvalues) explains more details about the different search algorithms used and why this type is so cool!

#### Spans

[dotnet/aspnetcore#46098](https://github.com/dotnet/aspnetcore/pull/46098) makes use of the new `MemoryExtensions.Split(ReadOnlySpan<char> source, Span<Range> destination, char separator)` method. This allows certain cases of `string.Split(...)` to be replaced with a non-allocating version. This saves the `string[]` allocation as well as the individual `string` allocations for the items in the `string[]`. More details on this new API can be seen in the [.NET 8 Performance Post span section](https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-8/#spans).

#### FrozenDictionary

Another new type introduced is `FrozenDictionary`. This allows constructing a dictionary optimized for read operations at the cost of slower construction.

[dotnet/aspnetcore#49714](https://github.com/dotnet/aspnetcore/pull/49714) switches a `Dictionary` in routing to use `FrozenDictionary`. This dictionary is used when routing an http request to the appropriate endpoint which is almost every request to an application. The following tables show the cost of creating the dictionary vs. frozen dictionary, and then the cost of using a dictionary vs. frozen dictionary respectively. You can see that constructing a `FrozenDictionary` can be up to 13x slower, but the overall time is still in the micro second range (1/1000th of a millisecond) and the `FrozenDictionary` is only constructed once for the app. What we all like to see is that the per operation performance of using `FrozenDictionary` is 2.5x-3.5x faster than a `Dictionary`!

```csharp
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class JumpTableMultipleEntryBenchmark
{
    private string[] _strings;
    private int[] _segments;

    private JumpTable _dictionary;
    private JumpTable _frozenDictionary;
    private List<(string text, int _)> _entries;

    [Params(1000)]
    public int NumRoutes;

    [GlobalSetup]
    public void Setup()
    {
        _strings = GetStrings(1000);
        _segments = new int[1000];

        for (var i = 0; i < _strings.Length; i++)
        {
            _segments[i] = _strings[i].Length;
        }

        var samples = new int[NumRoutes];
        for (var i = 0; i < samples.Length; i++)
        {
            samples[i] = i * (_strings.Length / NumRoutes);
        }

        _entries = new List<(string text, int _)>();
        for (var i = 0; i < samples.Length; i++)
        {
            _entries.Add((_strings[samples[i]], i));
        }

        _dictionary = new DictionaryJumpTable(0, -1, _entries.ToArray());
        _frozenDictionary = new FrozenDictionaryJumpTable(0, -1, _entries.ToArray());
    }

    [BenchmarkCategory("GetDestination"), Benchmark(Baseline = true, OperationsPerInvoke = 1000)]
    public int Dictionary()
    {
        var strings = _strings;
        var segments = _segments;

        var destination = 0;
        for (var i = 0; i < strings.Length; i++)
        {
            destination = _dictionary.GetDestination(strings[i], segments[i]);
        }

        return destination;
    }

    [BenchmarkCategory("GetDestination"), Benchmark(OperationsPerInvoke = 1000)]
    public int FrozenDictionary()
    {
        var strings = _strings;
        var segments = _segments;

        var destination = 0;
        for (var i = 0; i < strings.Length; i++)
        {
            destination = _frozenDictionary.GetDestination(strings[i], segments[i]);
        }

        return destination;
    }

    [BenchmarkCategory("Create"), Benchmark(Baseline = true)]
    public JumpTable CreateDictionaryJumpTable() => new DictionaryJumpTable(0, -1, _entries.ToArray());

    [BenchmarkCategory("Create"), Benchmark]
    public JumpTable CreateFrozenDictionaryJumpTable() => new FrozenDictionaryJumpTable(0, -1, _entries.ToArray());

    private static string[] GetStrings(int count)
    {
        var strings = new string[count];
        for (var i = 0; i < count; i++)
        {
            var guid = Guid.NewGuid().ToString();

            // Between 5 and 36 characters
            var text = guid.Substring(0, Math.Max(5, Math.Min(i, 36)));
            if (char.IsDigit(text[0]))
            {
                // Convert first character to a letter.
                text = ((char)(text[0] + ('G' - '0'))) + text.Substring(1);
            }

            if (i % 2 == 0)
            {
                // Lowercase half of them
                text = text.ToLowerInvariant();
            }

            strings[i] = text;
        }

        return strings;
    }
}

public abstract class JumpTable
{
    public abstract int GetDestination(string path, int segmentLength);
}

internal sealed class DictionaryJumpTable : JumpTable
{
    private readonly int _defaultDestination;
    private readonly int _exitDestination;
    private readonly Dictionary<string, int> _dictionary;

    public DictionaryJumpTable(
        int defaultDestination,
        int exitDestination,
        (string text, int destination)[] entries)
    {
        _defaultDestination = defaultDestination;
        _exitDestination = exitDestination;

        _dictionary = entries.ToDictionary(e => e.text, e => e.destination, StringComparer.OrdinalIgnoreCase);
    }

    public override int GetDestination(string path, int segmentLength)
    {
        if (segmentLength == 0)
        {
            return _exitDestination;
        }

        var text = path.Substring(0, segmentLength);
        if (_dictionary.TryGetValue(text, out var destination))
        {
            return destination;
        }

        return _defaultDestination;
    }
}

internal sealed class FrozenDictionaryJumpTable : JumpTable
{
    private readonly int _defaultDestination;
    private readonly int _exitDestination;
    private readonly FrozenDictionary<string, int> _dictionary;

    public FrozenDictionaryJumpTable(
        int defaultDestination,
        int exitDestination,
        (string text, int destination)[] entries)
    {
        _defaultDestination = defaultDestination;
        _exitDestination = exitDestination;

        _dictionary = entries.ToFrozenDictionary(e => e.text, e => e.destination, StringComparer.OrdinalIgnoreCase);
    }

    public override int GetDestination(string path, int segmentLength)
    {
        if (segmentLength == 0)
        {
            return _exitDestination;
        }

        var text = path.Substring(0, segmentLength);
        if (_dictionary.TryGetValue(text, out var destination))
        {
            return destination;
        }

        return _defaultDestination;
    }
}
```
|                          Method | NumRoutes |           Mean |         Error |        StdDev | Ratio | RatioSD |
|-------------------------------- |---------- |---------------:|--------------:|--------------:|------:|--------:|
|       CreateDictionaryJumpTable |        25 |     735.797 ns |     8.5503 ns |     7.5797 ns |  1.00 |    0.00 |
| CreateFrozenDictionaryJumpTable |        25 |   4,677.927 ns |    80.4279 ns |    71.2972 ns |  6.36 |    0.11 |
|                                 |           |                |               |               |       |         |
|       CreateDictionaryJumpTable |        50 |   1,433.309 ns |    19.4435 ns |    17.2362 ns |  1.00 |    0.00 |
| CreateFrozenDictionaryJumpTable |        50 |  10,065.905 ns |   188.7031 ns |   176.5130 ns |  7.03 |    0.12 |
|                                 |           |                |               |               |       |         |
|       CreateDictionaryJumpTable |       100 |   2,712.224 ns |    46.0878 ns |    53.0747 ns |  1.00 |    0.00 |
| CreateFrozenDictionaryJumpTable |       100 |  28,397.809 ns |   358.2159 ns |   335.0754 ns | 10.46 |    0.20 |
|                                 |           |                |               |               |       |         |
|       CreateDictionaryJumpTable |      1000 |  28,279.153 ns |   424.3761 ns |   354.3733 ns |  1.00 |    0.00 |
| CreateFrozenDictionaryJumpTable |      1000 | 313,515.684 ns | 6,148.5162 ns | 8,208.0925 ns | 11.26 |    0.33 |
|                                 |           |                |               |               |       |         |
|                      Dictionary |        25 |      21.428 ns |     0.1816 ns |     0.1516 ns |  1.00 |    0.00 |
|                FrozenDictionary |        25 |       7.137 ns |     0.0588 ns |     0.0521 ns |  0.33 |    0.00 |
|                                 |           |                |               |               |       |         |
|                      Dictionary |        50 |      21.630 ns |     0.1978 ns |     0.1851 ns |  1.00 |    0.00 |
|                FrozenDictionary |        50 |       7.476 ns |     0.0874 ns |     0.0818 ns |  0.35 |    0.00 |
|                                 |           |                |               |               |       |         |
|                      Dictionary |       100 |      23.508 ns |     0.3498 ns |     0.3272 ns |  1.00 |    0.00 |
|                FrozenDictionary |       100 |       7.123 ns |     0.0840 ns |     0.0745 ns |  0.30 |    0.00 |
|                                 |           |                |               |               |       |         |
|                      Dictionary |      1000 |      23.761 ns |     0.2360 ns |     0.2207 ns |  1.00 |    0.00 |
|                FrozenDictionary |      1000 |       8.516 ns |     0.1508 ns |     0.1337 ns |  0.36 |    0.01 |

### Other

This section is a compilation of changes that enhance performance but do not fall under any of the preceding categories.

#### Regex

As part of the AOT effort, we noticed the regex created in `RegexRouteConstraint` (see [route constraints](https://learn.microsoft.com/aspnet/core/fundamentals/routing?view=aspnetcore-7.0#route-constraints) for more info) was adding ~1MB to the published app size. This is because the route constraints are dynamic (application code defines them) and we were using the `Regex` constructor that accepts `RegexOptions`. This meant the trimmer has to keep around all regex code that could potentially be used, including the `NonBacktracking` engine which keeps ~.8MB of code. By adding `RegexOptions.Compiled` the trimmer can now see that the `NonBacktracking` code will not be used and it can reduce the application size by ~.8MB. Additionally, using compiled regexes is faster than using the interpreted regex. The quick fix was to just add `RegexOptions.Compiled` when creating the `Regex`. The problem is that this slows down app startup because we resolve constraints when starting the app and compiled regexes are slower to construct.

[dotnet/aspnetcore#46323](https://github.com/dotnet/aspnetcore/pull/46323) fixes this by lazily initializing the regexes so app startup is actually faster than 7.0 when we weren't using compiled regexes. It also added caching to the route constraints which means if you share constraints in multiple routes you will save allocations by sharing constraints across routes.

Running a microbenchmark for the route builder to measure startup performance shows an almost 450% improvement when using 1000 routes due to no longer initializing the regexes.
The benchmark lives [in the dotnet/aspnetcore repo](https://github.com/dotnet/aspnetcore/blob/8279d3f8ee47d5e249ef12a99e4d1cacfd5209ec/src/Http/Routing/perf/Microbenchmarks/Matching/CreateMatcherRegexConstraintBenchmark.cs#L29). It has a lot of setup code and would be a bit long to put in this post.

**Before with interpreted regexes:**
| Method |     Mean |     Error |    StdDev |  Op/s |   Gen 0 |  Gen 1 | Gen 2 | Allocated |
|------- |---------:|----------:|----------:|------:|--------:|-------:|------:|----------:|
|  Build | 6.739 ms | 0.0801 ms | 0.0710 ms | 148.4 | 15.6250 |      - |     - |      7 MB |

**After with compiled and lazy regexes:**
| Method |     Mean |     Error |    StdDev |  Op/s |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|------- |---------:|----------:|----------:|------:|-------:|-------:|------:|----------:|
|  Build | 1.521 ms | 0.0238 ms | 0.0211 ms | 657.2 | 5.8594 | 1.9531 |     - |      2 MB |

Another Regex improvement came from [dotnet/aspnetcore#44770](https://github.com/dotnet/aspnetcore/pull/44770) which switched a Regex usage in routing to use the [Regex Source Generator](https://learn.microsoft.com/dotnet/standard/base-types/regular-expression-source-generators). This moves the cost of compiling the Regex to build time, as well as resulting in faster Regex code due to optimizations the source generator takes advantage of that the in-process Regex compiler does not.

We'll show a simplified example that demonstrates using the generated regex vs. the compiled regex.

```csharp
public partial class AlphaRegex
{
    static Regex Net7Constraint = new Regex(
            @"^[a-z]*$",
            RegexOptions.CultureInvariant | RegexOptions.Compiled | RegexOptions.IgnoreCase,
            TimeSpan.FromSeconds(10));

    static Regex Net8Constraint = GetAlphaRouteRegex();

    [GeneratedRegex(@"^[A-Za-z]*$")]
    private static partial Regex GetAlphaRouteRegex();

    [Benchmark(Baseline = true)]
    public bool CompiledRegex()
    {
        return Net7Constraint.IsMatch("Administration") && Net7Constraint.IsMatch("US");
    }

    [Benchmark]
    public bool SourceGenRegex()
    {
        return Net8Constraint.IsMatch("Administration") && Net8Constraint.IsMatch("US");
    }
}
```

|         Method |     Mean |    Error |   StdDev | Ratio |
|--------------- |---------:|---------:|---------:|------:|
|  CompiledRegex | 86.92 ns | 0.572 ns | 0.447 ns |  1.00 |
| SourceGenRegex | 57.81 ns | 0.860 ns | 0.805 ns |  0.66 |

#### Analyzers

Analyzers are useful for pointing out issues in code that can be hard to convey in API signatures, suggesting code patterns that are more readable, and they can also suggest more performant ways to write code. [dotnet/aspnetcore#44799](https://github.com/dotnet/aspnetcore/pull/44799) and [dotnet/aspnetcore#44791](https://github.com/dotnet/aspnetcore/pull/44791) both from [@martincostello](https://github.com/martincostello) enabled [CA1854](https://learn.microsoft.com/dotnet/fundamentals/code-analysis/quality-rules/ca1854) which helps avoid 2 dictionary lookups when only 1 is needed, and [dotnet/aspnetcore#44269](https://github.com/dotnet/aspnetcore/pull/44269) enables a bunch of analyzers many of which help use more performant APIs and are described in more detail in last years [.NET 7 Performance Post](https://devblogs.microsoft.com/dotnet/performance_improvements_in_net_7/#analyzers).

I would encourage developers who are interested in performance in their own products to checkout [performance focused analyzers](https://learn.microsoft.com/dotnet/fundamentals/code-analysis/quality-rules/performance-warnings) which contains a list of many analyzers that will help avoid easy to fix performance issues.

#### StringBuilder

`StringBuilder` is an extremely useful class for constructing a string when you either can't precompute the size of the string to create or want an easy way to construct a string without the complications involved with using `string.Create(...)`.

`StringBuilder` comes with a lot of helpful methods as well as a custom implementation of an [InterpolatedStringHandler](https://learn.microsoft.com/dotnet/csharp/whats-new/tutorials/interpolated-string-handler). What this means is that you can "create" strings to add to the `StringBuilder` without actually allocating the string. For example, previously you might have written `stringBuilder.Append(FormattableString.Invariant($"{key} = {value}"));`. This would have allocated a string via `FormattableString.Invariant(...)` then put it in the `StringBuilder`s internal `char[]` buffer, making the string a temporary allocation. Instead you can write `stringBuilder.Append(CultureInfo.InvariantCulture, $"{key} = {value}");`. This also looks like it would allocate a string via `$"{key} = {value}"`, but because `StringBuilder` has a custom `InterpolatedStringHandler` the string isn't actually allocated and instead is written directly to the internal `char[]`.

[dotnet/aspnetcore#44691](https://github.com/dotnet/aspnetcore/pull/44691) fixes some usage patterns with `StringBuilder` to avoid allocations as well as makes use of the `InterpolatedStringHandler` overload(s).
One specific example was taking a `byte[]` and converting it into a string in hexidecimal format so we could send it as a query string.
```csharp
[MemoryDiagnoser]
public class AppendBenchmark
{
    private byte[] _b = new byte[30];

    [GlobalSetup]
    public void Setup()
    {
        RandomNumberGenerator.Fill(_b);
    }

    [Benchmark]
    public string AppendToString()
    {
        var sb = new StringBuilder();
        foreach (var b in _b)
        {
            sb.Append(b.ToString("x2", CultureInfo.InvariantCulture));
        }
        return sb.ToString();
    }

    [Benchmark]
    public string AppendInterpolated()
    {
        var sb = new StringBuilder();
        foreach (var b in _b)
        {
            sb.Append(CultureInfo.InvariantCulture, $"{b:x2}");
        }
        return sb.ToString();
    }
}
```

|             Method |     Mean |    Error |   StdDev |   Gen0 | Allocated |
|------------------- |---------:|---------:|---------:|-------:|----------:|
|     AppendToString | 748.7 ns | 12.33 ns | 10.93 ns | 0.1841 |    1448 B |
| AppendInterpolated | 739.7 ns |  9.29 ns |  8.23 ns | 0.0620 |     488 B |

## Summary

Thanks for reading! [Try out .NET 8](https://dotnet.microsoft.com/download/dotnet/8.0) and let us know how your app's performance has changed! We are always looking for feedback on how to improve the product and look forward to your contributions, be it an issue report or a PR.
If you want more performance goodness, you can read the [Performance Improvements in .NET 8](https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-8/) post. Also, take a look at [Developer Stories](https://devblogs.microsoft.com/dotnet/category/developer-stories/) which showcases multiple teams at Microsoft migrating from .NET Framework to .NET or to newer versions of .NET and seeing major performance and operating cost wins.