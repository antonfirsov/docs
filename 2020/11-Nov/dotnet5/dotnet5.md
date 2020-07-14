# Announcing .NET 5.0

We’re excited to announce the release of .NET 5.0. It includes [many improvements](https://github.com/dotnet/runtime/issues/37269), including single file applications, smaller container images, more capable JSON APIs, a complete set of nullable reference type annotations, and support for Windows ARM64. [Performance has been greatly improved](https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-5/), in the NET libraries, in the GC, and the JIT. ARM64 was a key focus for performance investment, resulting in much better throughput and smaller binaries. .NET 5.0 includes new language versions, [C# 9](https://devblogs.microsoft.com/dotnet/welcome-to-c-9-0/) and [F# 5.0](https://devblogs.microsoft.com/dotnet/announcing-f-5-preview-1/).

ASP.NET Core and EF Core are also being released today.

You can [download .NET 5.0](https://dotnet.microsoft.com/download/dotnet/5.0), for Windows, macOS, and Linux:

* [Windows and macOS installers](https://dotnet.microsoft.com/download/dotnet/5.0)
* [Binaries](https://dotnet.microsoft.com/download/dotnet/5.0)
* [Container images](https://hub.docker.com/_/microsoft-dotnet)
* [Snap installer](https://snapcraft.io/dotnet-sdk)

You need to use the latest [Visual Studio 2019](https://visualstudio.microsoft.com/vs/), [Visual Studio for Mac](https://visualstudio.microsoft.com/), or [C# extension](https://code.visualstudio.com/Docs/languages/csharp) with Visual Studio Code to use .NET 5.0. 

Release notes:

* [.NET 5.0 release notes](https://github.com/dotnet/core/tree/master/release-notes/5.0)
* [.NET 5.0 known issues](https://github.com/dotnet/core/blob/master/release-notes/5.0/5.0-known-issues.md)
* [.NET 5.0 Runtime epics](https://github.com/dotnet/runtime/issues/37269)
* GitHub release
* GitHub tracking issue

## .NET 5.0 Highlights

The following improvements are among the ones you will most likely notice and take advantage of when you adopt .NET 5.0. The primary GitHub PR or issue are provided for each highlight, and the features themselves are described in more detail later in the post.

The [.NET 5.0 Runtime Epics](https://github.com/dotnet/runtime/issues/37269) provide a more detailed set of highlights, dedicated to the runtime and libraries.

> Editor's note: Goal for each of these topics is top 2-4 contributions. We are trying to answer the "why you should care" question and nothing more.

* [C# 9](https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-5/)
   * Top-level programs that enable no-ceremony programs (no classes required).
   * Extends immutability to object initializers and property accessors and adds a new form of immutable value types called records.
* [F# 5.0](https://devblogs.microsoft.com/dotnet/announcing-f-5-preview-1/)
   * Better interactive and analytical programming.
* New APIs
   * New TFMs for .NET (waiting on Immo's post)
   * Improving usability and capability of System.Text.Json (need a link)
   * Cross-platform support for System.DirectoryServices.Protocols
* [Performance Improvements](https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-5/)
   * [Garbage collection](https://github.com/dotnet/coreclr/pull/25986)
   * [Regular expressions](https://devblogs.microsoft.com/dotnet/regex-performance-improvements-in-net-5/)
   * [HTTP 1.1](https://github.com/dotnet/corefx/pull/41640) and [HTTP/2](https://github.com/dotnet/runtime/pull/35694)
   * [ARM64](https://github.com/dotnet/runtime/issues/35853) (Linux and Windows) in the JIT and BCL libraries.
* Consistent performance ([P95+ latency](https://github.com/dotnet/runtime/issues/37534)). 
   * [Tiered compilation](https://github.com/dotnet/runtime/pull/32250) used by tiered JIT compilation to smooth out performance during startup
   * [Pinned object heap](https://github.com/dotnet/runtime/pull/32283) to reduce heap fragmentation caused by pinning
   * Reduce GC pause times in specific situations, like [Array.Copy](https://github.com/dotnet/coreclr/pull/27776), [Array.Sort](https://github.com/dotnet/runtime/pull/35297) or [object unboxing](https://github.com/dotnet/runtime/pull/32353#issuecomment-586642480)
* Application deployment
   * [Single-file apps](https://github.com/dotnet/runtime/issues/36590)
   * [Reduce container image size](https://github.com/dotnet/dotnet-docker/issues/1814#issuecomment-625294750)
* Platforms
   * [Windows ARM64](https://github.com/dotnet/runtime/issues/36699), supporting both development scenarios and deployment of client apps on customer machines.
   * [Apple Silicon](https://github.com/dotnet/coreclr/pull/28051) -- Support running x64 apps in the Rosetta 2 emulator.
   * [Web Assembly (wasm)](https://github.com/dotnet/runtime/issues/38367) -- Target the wasm with Blazor and Mono

## Framing .NET 5 w/.NET Framework and Xamarin Project

## Features

## Closing

