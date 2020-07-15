# Announcing .NET 5.0

We’re excited to announce the release of .NET 5.0. It includes [many improvements](https://github.com/dotnet/runtime/issues/37269), including single file applications, smaller container images, more capable JSON APIs, a complete set of nullable reference type annotations, and support for Windows ARM64. [Performance has been greatly improved](https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-5/), in the NET libraries, in the GC, and the JIT. ARM64 was a key focus for performance investment, resulting in much better throughput and smaller binaries. .NET 5.0 includes new language versions, [C# 9](https://devblogs.microsoft.com/dotnet/welcome-to-c-9-0/) and [F# 5.0](https://devblogs.microsoft.com/dotnet/announcing-f-5-preview-1/).

ASP.NET Core and EF Core are also being released today.

You can [download .NET 5.0](https://dotnet.microsoft.com/download/dotnet/5.0), for Windows, macOS, and Linux:

* [Installers and binaries](https://dotnet.microsoft.com/download/dotnet/5.0)
* [Container images](https://hub.docker.com/_/microsoft-dotnet)
* [Snap installer](https://snapcraft.io/dotnet-sdk)
* [Release notes](https://github.com/dotnet/core/tree/master/release-notes/5.0)
* [Known issues](https://github.com/dotnet/core/blob/master/release-notes/5.0/5.0-known-issues.md)
* [GitHub issue tracker](https://github.com/dotnet/core/issues)

You need the latest version of [Visual Studio](https://visualstudio.microsoft.com) (including Visual Studio for Mac) to use .NET 5.0.

## .NET 5.0 Highlights

The following improvements are the highlights of .NET 5.0, and the ones we hope you enjoy using the most. The primary GitHub PR or issue are provided for each highlight, and the features are described in more detail later in the post.

> Editor's note: Goal for each of these topics is top 2-4 contributions. We are trying to answer the "why you should care" question and nothing more.

* [C# 9](https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-5/)
   * Top-level programs -- enable no-ceremony programs (no classes required).
   * Immutable types -- extends immutability to object initializers and property accessors, adds records (immutable value types).
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

## .NET 5.0+

Last year, we shared a broad [vision of a singled unified .NET stack and ecosystem](https://devblogs.microsoft.com/dotnet/introducing-net-5/). We're happy to report that we did much of the underlying work needed to deliver that vision. We started the release with [CoreCLR](https://github.com/dotnet/coreclr), [CoreFX](https://github.com/dotnet/corefx), and [Mono](https://github.com/mono/mono) all in separate repos, and with significant duplication across them. We ended the release with the CoreCLR and Mono runtimes and the .NET libraries all together in the [runtime](https://github.com/dotnet/runtime) repo. In particular, Mono (in the runtime repo) and CoreCLR now use the same libraries. Unfortunately, due to the global pandemic, we had to defer shipping a release of Xamarin based on this repo until .NET 6.0.

As part of .NET 5.0, we are releasing a new version of web assembly based on Mono and the .NET libraries, from the runtime repo (as opposed to the [mono](https://github.com/mono/mono) repo). This part of the release delivers on the initial vision, and proves out the model. We look forward to adding support for iOS and Android apps, based on Mono and the .NET libraries, as part of .NET 6.0.

Looking forward, most of our fundamental investment will go into the runtime repo. We intend to use CoreCLR for desktop, IoT, and server workloads and Mono for mobile and web assembly. We'll continue to optimize the .NET libraries to deliver a first-class experience across all of those workload types. This includes making the libraries more linkable. For example, we don't need algorithms that can take advantage of multiple cores with web assembly.

## Features

## Closing
