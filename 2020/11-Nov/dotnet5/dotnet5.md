# Announcing .NET 5.0

We’re excited to announce the release of .NET 5.0. It includes [many improvements](https://github.com/dotnet/runtime/issues/37269), including single file applications, smaller container images, more capable JSON APIs, a complete set of nullable reference type annotations, and support for Windows ARM64. [Performance has been greatly improved](https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-5/), in the NET libraries, in the GC, and the JIT. ARM64 was a key focus for performance investment, resulting in much better throughput and smaller binaries. .NET 5.0 includes new language versions, [C# 9](https://devblogs.microsoft.com/dotnet/welcome-to-c-9-0/) and [F# 5.0](https://devblogs.microsoft.com/dotnet/announcing-f-5-preview-1/).

ASP.NET Core and EF Core are also being released today.

You can [download .NET 5.0](https://dotnet.microsoft.com/download/dotnet/5.0), for Windows, macOS, and Linux:

* [Windows and macOS installers](https://dotnet.microsoft.com/download/dotnet/5.0)
* [Binaries](https://dotnet.microsoft.com/download/dotnet/5.0)
* [Container images](https://hub.docker.com/_/microsoft-dotnet)
* [Snap installer](https://snapcraft.io/dotnet-sdk)

Release notes:

* [.NET 5.0 release notes](https://github.com/dotnet/core/tree/master/release-notes/5.0)
* [.NET 5.0 known issues](https://github.com/dotnet/core/blob/master/release-notes/5.0/5.0-known-issues.md)
* [.NET 5.0 Runtime epics](https://github.com/dotnet/runtime/issues/37269)
* GitHub release
* GitHub tracking issue

You need the latest Visual Studio to use .NET 5.0:

* [Visual Studio 2019 (16.7)](https://visualstudio.microsoft.com/vs/)
* [Visual Studio for Mac](https://visualstudio.microsoft.com/vs/mac/)
* [Visual Studio Code](https://code.visualstudio.com/) with the [C# extension](https://code.visualstudio.com/Docs/languages/csharp) 

## .NET 5.0 Highlights

The following improvements are the highlights of .NET 5.0, and the ones we hope you enjoy using the most. The primary GitHub PR or issue are provided for each highlight, and the features are described in more detail later in the post.

> Editor's note: Goal for each of these topics is top 2-4 contributions. We are trying to answer the "why you should care" question and nothing more.

* [C# 9](https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-5/)
   * Top-level programs -- enable no-ceremony programs (no classes required).
   * Immutability types -- extends immutability to object initializers and property accessors, adds records (immutable value types).
* [F# 5.0](https://devblogs.microsoft.com/dotnet/announcing-f-5-preview-1/)
   * Better interactive and analytical programming.
* .NET Libraries
   * New target framework (TFM) for .NET -- the new TFM is `net5` and replaces `netcoreapp` and `netstandard`.
   * JSON APIs -- Improving usability and capability of System.Text.Json.
   * Directory services -- Cross-platform support for System.DirectoryServices.Protocols.
   * Nullable reference types -- Complete set of nullable reference type annotations for .NET libraries.
* [Performance Improvements](https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-5/)
   * [Garbage collection](https://github.com/dotnet/coreclr/pull/25986) -- card stealing enables better work balance (throughput) in Server GC.
   * [Regular expressions](https://devblogs.microsoft.com/dotnet/regex-performance-improvements-in-net-5/) -- 3-6x throughput improvements in many cases.
   * [HTTP 1.1](https://github.com/dotnet/corefx/pull/41640) and [HTTP/2](https://github.com/dotnet/runtime/pull/35694) -- increase throughput with HTTP.
   * [ARM64](https://github.com/dotnet/runtime/issues/35853) -- improve throughput and size of applications targeting ARM64.
* Consistent performance ([P95+ latency](https://github.com/dotnet/runtime/issues/37534)). 
   * [Garbage collection](https://github.com/dotnet/coreclr/pull/27578) -- Reduce the cost of [suspension](https://github.com/dotnet/coreclr/pull/27729)
   * [Tiered compilation](https://github.com/dotnet/runtime/pull/32250) -- used by tiered JIT compilation to smooth out performance during startup
   * [Pinned object heap](https://github.com/dotnet/runtime/pull/32283) -- reduce heap fragmentation caused by pinning.
* Application deployment
   * [Single-file apps](https://github.com/dotnet/runtime/issues/36590) -- apps that are deployed and run as a single binary.
   * [Reduce container image size](https://github.com/dotnet/dotnet-docker/issues/1814#issuecomment-625294750) -- Optimizes container layering for large size savings for building images.
* Platforms
   * [Windows ARM64](https://github.com/dotnet/runtime/issues/36699) -- develop and deploy apps on Windows ARM64.
   * [Apple Silicon](https://github.com/dotnet/coreclr/pull/28051) -- aupport running x64 apps in the Rosetta 2 emulator.
   * [Web Assembly (wasm)](https://github.com/dotnet/runtime/issues/38367) -- build wasm apps and deploy in the browser, with Blazor and Mono.

   The [.NET 5.0 Runtime Epics](https://github.com/dotnet/runtime/issues/37269) provide a more detailed set of highlights dedicated to the runtime and libraries.

## Framing .NET 5 w/.NET Framework and Xamarin Project

## Features

## Closing

