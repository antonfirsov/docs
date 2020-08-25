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

You need the latest version of [Visual Studio](https://visualstudio.microsoft.com) (including [Visual Studio for Mac)](https://visualstudio.microsoft.com/vs/mac/) to use .NET 5.0. The [Visual Studio Code](https://code.visualstudio.com/) [C# extension](https://code.visualstudio.com/docs/languages/dotnet) now supports C# 9.

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
   * [Web Assembly (wasm)](https://github.com/dotnet/runtime/issues/38367) -- build wasm apps and deploy in the browser, with Blazor and Mono.

   The [.NET 5.0 Runtime Epics](https://github.com/dotnet/runtime/issues/37269) provide a more detailed set of highlights dedicated to the runtime and libraries.

## Unified platform vision

Last year, we shared a [broad vision of a singular .NET stack and ecosystem](https://devblogs.microsoft.com/dotnet/introducing-net-5/). We're happy to report that we now have the foundation in place to deliver on that vision, in .NET 6.0. We started the 5.0 release with separate [CoreCLR](https://github.com/dotnet/coreclr), [CoreFX](https://github.com/dotnet/corefx), and [Mono](https://github.com/mono/mono) repos, with significant duplication across them. We ended the release with the unified [runtime](https://github.com/dotnet/runtime) repo, which includes the CoreCLR and Mono runtimes and the .NET libraries. We now have one repository for the runtime, libraries, and other low-level components of the .NET platform, which we will move forward in lock-step together in future releases. The benefits of this change are much higher compatibility between the various .NET app types and maintaining and improving just one libraries code base. 

A first example of moving forward with this vision is our work with [Web Assembly](https://webassembly.org/). Blazor Webassembly in .NET 5.0 uses the Mono runtime and the .NET Libraries (all the System.* libraries). This is a change from [Blazor 3.2](https://devblogs.microsoft.com/aspnet/blazor-webassembly-3-2-0-now-available/), which used the Mono runtime and Mono libraries. The change we made with Web assembly, to use the .NET libraries, is a down-payment on the broader vision. We expect to deliver the rest of the vision, largely focused on Xamarin (iOS and Android), with .NET 6.0. We had intended to deliver support for Xamarin apps with 5.0, but the global pandemic caused us to pause that work for a release. 

Looking forward, our fundamental investments will go into the runtime repo, for .NET 6.0 and beyond. We intend to use CoreCLR for desktop, IoT, and server workloads and Mono for mobile and web assembly. We'll continue to optimize the .NET libraries to deliver a first-class experience across all of those workload types.

We'll continue to support and service .NET Framework in Windows and Windows Server. We release patches for .NET Framework nearly [every month](https://github.com/dotnet/announcements/labels/Monthly-Update), including in [container images](https://hub.docker.com/_/microsoft-dotnet-framework). We'll continue this model going forward, and support .NET Framework with each new version of Windows and Windows Server.

Let's switch to looking at what's new in the 5.0 release.

## Application deployment

After writing or updating an application, you need to [deploy it](https://docs.microsoft.com/dotnet/core/deploying/) for your users to take advantage of. This might be to a web server, a cloud service, or client machine, and might be the result of a CI/CD flow using a service like [Azure DevOps](https://docs.microsoft.com/azure/devops/pipelines/ecosystems/dotnet-core) or [GitHub Actions](https://github.com/actions/setup-dotnet).

We strive to provide first-class deployment capabilities that naturally align with the application types. For .NET 5.0, we focused on improving single file applications, reducing container size for docker multi-stage builds, and providing better support for deploying ClickOnce applications with .NET Core.

Best doc to get started: [.NET application deployment](https://docs.microsoft.com/dotnet/core/deploying/)

### Single file applications

[Single file applications](https://github.com/dotnet/runtime/issues/36590) are published and deployed as a single file. The app and its dependencies are all included within that file. When the app is run, the dependencies are loaded directly from that file into memory. There is no performance penalty with this approach. When combined with assembly trimming and ahead-of-time compilation, single file apps are smaller and startup quickly.

Single file apps can be either framework-dependent or self-contained. Framework-dependent single file apps can be very small, by relying on a globally-installed .NET runtime. Self-contained single-file apps are larger (due to carrying the runtime), but do not require installation of the .NET runtime as an installation pre-step and will just work as a result. In general, framework-dependent is good for development and enterprise environments, while self-contained is often a better choice for ISVs.

We produced a version of single-file apps with .NET Core 3.1. It packages binaries into a single file for deployment and then unpacks those files to a temporary directory to load and execute them. There may be some scenarios where this approach is better, but we expect that the solution we've built for 5.0 will be preferred and a welcome improvement.

We had multiple hurdles to overcome to create a true single-file solution. We had to create a more sophisticated bundler, teach the runtime to load assemblies out of binary resources, and make the debugger compatible with memory-mapped assemblies. We also ran into some hurdles that we could not clear.

On all platforms, we have a component called "apphost". This is the file that becomes you executable, for example `myapp.exe` on Windows or `./myapp` on Unix-based platforms. For single file apps we created a new host we call "superhost". It has the same role as the regular apphost, but also includes a statically-linked copy of the runtime. The superhost is a fundamental design point of our our single file approach. This model is the one we use on Linux. We were not able to implement this approach on Windows or macOS, due to various operating system constraints. We do not have a superhost on Windows or macOS. On those operating systems, the native runtime binaries (~3 of them) sit beside the single file app. We will revisit this situation in .NET 6.0, however, we expect the problems we ran into to remain challenging. 

The following table demonstrates what you can expect for both runtime-dependent and self-contained applications with 5.0. It compares 3.1 to 5.0, for the console template and [BlazingPizza.Server](https://github.com/dotnet-presentations/blazor-workshop/tree/master/src/BlazingPizza.Server) app.

> Editors note: I need to re-run these scenarios and update the numbers.

- 3.1 console template (Linux x64)
   - Runtime-dependent: 96k
   - Self-contained: 36MB
- 5.0 console template (Linux x64)
   - Runtime-dependent: 215k
   - Self-contained: 29MB
- 3.1 BlazingPizza.Server 
   - Runtime-dependent: 27MB
   - Self-contained: 80MB
- 5.0 BlazingPizza.Server
   - Runtime-dependent: 24MB
   - Self-contained: 67MB

Notes:

- Framework-dependent publish: dotnet publish -r linux-x64 --self-contained false /p:PublishSingleFile=true
- Self-contained publish: dotnet publish -r linux-x64 --self-contained true /p:PublishSingleFile=true /p:PublishTrimmed=true
- Assembly trimmng isn't supported for framework-dependent apps.

You can also configure single file publishing with a project file.

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net5.0</TargetFramework>
    <!-- Enable single file -->
    <PublishSingleFile>true</PublishSingleFile>
    <!-- Determine self-contained or framework-dependent -->
    <SelfContained>true</SelfContained>
    <!-- Enable use of assemby trimming - only supported for self-contained apps -->
    <PublishTrimmed>true</PublishTrimmed>
    <!-- Enable AOT compilation -->
    <PublishReadyToRun>true</PublishReadyToRun>
  </PropertyGroup>

</Project>
```

Notes:

* Apps are OS and architecture-specific. You need to publish for each configuration (Windows x64, Linux x64, Windows ARM64, ...).
* Configuration files (like `*.runtimeconfig.json`) are included in the single file. You can place an additional config file beside the single file, if needed (good for testing).
* `.pdb` files are not included in the single file by default. You can enable PDB embedding with the `<DebugType>embed</DebugType>` property.

We've seen a lot of comments on previous preview posts asking about the relationship between single file apps and ahead of time (AOT) compilation. AOT is a spectrum. The ready-to-run code that `dotnet publish` generates (when you set `PublishReadyToRun` to true) is an example of AOT. When you publish ready-to-run images, the build generates machine code for you, ahead of time, instead of the JIT doing it at runtime. I think most people will accept this as a definition of AOT. However, many people mean something more specific when they say AOT. They want a solution that has the following characteristics: no IL present (for size and obfuscation reasons), a JIT is (at most) optional, and binary size is as small as it can be. We use the term "native AOT" to describe that point on the AOT spectrum. The single file solution we have in .NET 5.0 doesn't satisfy this definition of AOT. It's a big step forward, but it isn't "native AOT". We recently published a [survey on Native AOT](https://github.com/dotnet/runtime/issues/40430) to get more feedback on that modality. We're looking through the results now and will include them in our 6.0 planning effort. 

## More Features

## Closing
