# Announcing .NET 5.0 Preview 7

Today, we're releasing .NET 5.0 Preview 7. It's the second to last of the preview releases (before moving to RC). Most features should be very close to done at this point. Single file and ARM64 intrinsics are two feature areas that are taking the longest time to complete, but are on track for Preview 8. See the [.NET 5.0 Preview 4 post](https://devblogs.microsoft.com/dotnet/announcing-net-5-preview-4-and-our-journey-to-one-net/) for a broader view of the release.

ASP.NET Core and EF Core are also being released today.

You can [download .NET 5.0 Preview 7](https://dotnet.microsoft.com/download/dotnet/5.0), for Windows, macOS, and Linux:

* [Installers and binaries](https://dotnet.microsoft.com/download/dotnet/5.0)
* [Container images](https://hub.docker.com/_/microsoft-dotnet)
* [Snap installer](https://snapcraft.io/dotnet-sdk)
* [Release notes](https://github.com/dotnet/core/tree/master/release-notes/5.0)
* [Known issues](https://github.com/dotnet/core/blob/master/release-notes/5.0/5.0-known-issues.md)
* [GitHub issue tracker](https://github.com/dotnet/core/issues)


You need to use [Visual Studio 2019 16.7](https://visualstudio.microsoft.com/vs/preview/) to use .NET 5.0. .NET 5.0 is now supported with [Visual Studio for Mac](https://visualstudio.microsoft.com/vs/mac). Install the latest version of the [C# extension](https://code.visualstudio.com/Docs/languages/csharp) to use .NET 5.0 with [Visual Studio Code](https://visualstudio.microsoft.com/). 

## Performance

Stephen Toub recently posted his [Performance Improvements in .NET 5](https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-5/) post, the latest in his series. He covers ~250 performance-oriented pull requests, which reveal improvements that will even surprise people who follow .NET Core performance closely.

## System.Text.Json

We've been adding usability feature to the new JSON API. The following features are new in Preview 7 (more coming in Preview 8, too).

* [[Breaking change] Ability to ignore default values for value-type properties when serializing](https://github.com/dotnet/runtime/pull/36322/) -- can be used to reduce serialization and wire costs.
* [Ability to handle circular references when serializing](https://github.com/dotnet/runtime/pull/36829) -- API shape is now expected to be final.

## Garbage Collection (GC)

The GC now exposes detailed information of the most recent collection, via the [GC.GetGCMemoryInfo](https://github.com/dotnet/runtime/blob/6a8fd0bec119d4cb36ad40a5d2242ed7f781dd60/src/coreclr/src/System.Private.CoreLib/src/System/GC.cs#L59-L75) method. The [GCMemoryInfo](https://github.com/dotnet/runtime/blob/6072e4d3a7a2a1493f514cdf4be75a3d56580e84/src/libraries/System.Private.CoreLib/src/System/GCMemoryInfo.cs#L97) struct provides information about machine memory, heap memory and the most recent collection, or most recent collection of the kind of GC you specify - ephemeral, full blocking or background GC.

The most likely use cases for using this new API are for logging/monitoring or to indicate to a loader balancer that a machine should be taken out of rotation to request a full GC. It could also be used to avoid container hard-limits by reducing the size of caches.

Another, small but impactful change, was made to [defer the expensive `reset memory` operation to low-memory situations](https://github.com/dotnet/runtime/pull/37894). We expect these
changes in policy to lower the GC latency (and GC CPU usage in general).

## RyuJIT

RyuJIT is the assembly code generator for .NET, target both Intel and ARM chips. Most of the investment in RyuJIT is focused on performance.

* General Improvements
   * [Enable eliding some bounds checks](https://github.com/dotnet/runtime/pull/36263) -- Credit [@nathan-moore](https://github.com/nathan-moore)
   * [Optimize Enum.CompareTo after being rewritten in C#](https://github.com/dotnet/runtime/pull/37845) -- performance is now at parity with previous C++ implementation. 
   * [Improvement in register allocation for structs](https://github.com/dotnet/runtime/pull/36862) -- Enregister multireg lclVars
   * [Improvements for removal of redundant zero inits](https://github.com/dotnet/runtime/pull/37786)
   * [Tail duplication improvement](https://github.com/dotnet/runtime/pull/37038)
   * [Stack based structs copy CQ fix](https://github.com/dotnet/runtime/pull/37967)
   * [Clean up a dead field assignment after removing redundant zero initializations](https://github.com/dotnet/runtime/pull/37280)
* [ARM64 hardware intrinsics & API optimization](https://github.com/dotnet/runtime/issues/33308)
   * [Implement majority of “by element” intrinsics](https://github.com/dotnet/runtime/pull/36916)  
   * Implement fcvtxn, fcvtxn2, sqabs, sqneg, suqadd, usqadd intrinsics -- [#38010](https://github.com/dotnet/runtime/pull/38010), [#38110](https://github.com/dotnet/runtime/pull/38110)
   * [Optimize SpanHelpers.IndexOf(byte), SpanHelpers.IndexOf(char)](https://github.com/dotnet/runtime/pull/37624)
   * [Optimize SpanHelpers.IndexOfAny(byte)](https://github.com/dotnet/runtime/pull/37934)
   * [Optimize WithLower, WithUpper, Create, AsInt64, AsUInt64, AsDouble](https://github.com/dotnet/runtime/pull/37139)
   * [Optimize AsVector, AsVector128, GetUpper, As and WithElement](https://github.com/dotnet/runtime/pull/37338)
 
 ## Closing

Please tell us about your experience using Preview 7. It's not too late to share your feedback. We are getting close to the end of the release, but are actually more focused on quality now since we are largely done with feature development.

After Preview 8, we plan to release two RCs before the final release. The RCs will have "go live" licenses, meaning they'll be supported in production. On that note, we've been running the [dot.net](https://dot.net/) site on .NET 5.0 since Preview 1 (50% on 5.0; 50% on 3.1) and it has worked great.
