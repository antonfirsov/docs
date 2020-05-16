# Announcing .NET 5 Preview 4 and our journey to one .NET

.NET 5 is the next version of .NET where we continue the journey of unifying the .NET platform. We’ve took the best of .NET Framework and put that into .NET Core 3, with the addition of Windows desktop support, including WPF and Windows Forms. As we continue the journey, our vision for one .NET is to take .NET Core and Mono/Xamarin and create a unified set of libraries and tools as well as expand on the support for cloud native and modern web development.  

I encourage you to watch "The Journey to One .NET" with Scott Hanselman and I to find out more. 

Note: Add link above.

Last year, [we laid out our vision for one .NET and .NET 5](https://devblogs.microsoft.com/dotnet/introducing-net-5/), we said we would take .NET Core and Mono/Xamarin implementations and unify them into one base class library (BCL) and toolchain (SDK). In the wake of the global health pandemic, we've had to adapt to the changing needs of our customers and provide the support needed to assist with smooth operations. Our efforts continue to be anchored in helping our customers address their most urgent needs. As a result, we expect these features to be available in preview by November 2020, but the unification will be truly completed with .NET 6, our Long-Term Support (LTS) release. Our vision hasn't changed, but our timeline has. 

.NET 5 will have several cloud & web investments, such as smaller, faster, single file EXEs that use less memory which are appropriate for microservices and containerized applications across operating systems. We'll continue to build on the work we've done so far.

We are still committed to one .NET platform and delivering a quality .NET 5 release to our millions of users in November this year. You'll continue to see a wave of innovation happening with multiple previews on the journey to one .NET.  

## Download Preview 4

You can [download .NET 5.0 Preview 4](https://dotnet.microsoft.com/download/dotnet/5.0), for Windows, macOS, and Linux:

* [MSIs, PKGs and .zip/tar.gz](https://dotnet.microsoft.com/download/dotnet/5.0)
* [Docker images](https://hub.docker.com/_/microsoft-dotnet-core)
* [Snap installer](https://snapcraft.io/dotnet-sdk)

ASP.NET Core and EF Core are also being released today.

You need Visual Studio 2019 16.6 or later versions to use .NET 5.0. To use .NET 5.0 with Visual Studio Code, install the latest version of the [C# extension](https://code.visualstudio.com/Docs/languages/csharp). .NET 5.0 isn't yet supported with Visual Studio for Mac.

Release notes:

* [.NET 5.0 release notes](https://github.com/dotnet/core/tree/master/release-notes/5.0/preview)
* [.NET 5.0 known issues](https://github.com/dotnet/core/blob/master/release-notes/5.0/5.0-known-issues.md)
* [.NET Core 3.1 -> .NET 5.0 API diff](https://github.com/dotnet/core/tree/master/release-notes/5.0/preview/api-diff)
* [GitHub release](https://github.com/dotnet/core/releases)
* [GitHub tracking issue](https://github.com/dotnet/core/issues/4607)

## .NET 5 Highlights

Let's take a look at some of the release highlights that we expect to deliver with .NET 5.0, in November. This will paint a clearer picture on the improvements you'll get to take advantage of in your development process and in production.

* Performance -- Improve [performance throughout the product](https://github.com/dotnet/runtime/pulls?page=2&q=is%3Apr+is%3Aclosed+label%3Atenet-performance) to make applications run faster and more efficiently (less cost). Examples:
   * [Regular expressions](https://devblogs.microsoft.com/dotnet/regex-performance-improvements-in-net-5/)
   * [Improve performance of `string.ToUpperInvariant`, `string.ToLowerInvariant`, `char.ToUpperInvariant`, `char.ToLowerInvariant`, and other related patterns](https://github.com/dotnet/runtime/pull/31968)
   * [Improve HTTP 1.1 performance](https://github.com/dotnet/corefx/pull/41640)
   * [Improve HTTP/2 scaling performance](https://github.com/dotnet/runtime/pull/35694)
   * [Added on-stack-replacement to improve tiered compilation performance](https://github.com/dotnet/runtime/pull/32969)
   * [Improve stack prolog zeroing performance](https://github.com/dotnet/runtime/pull/32538)
   * [Improve performance of tailcalls used by F#](https://github.com/dotnet/runtime/pull/341)
* Consistent performance: We have increased our focus on predictable consistent performance, reducing performance cliffs and outliers, with an emphasis on P95+ latency. 
   * [Improve call counting mechanism](https://github.com/dotnet/runtime/pull/32250) used by tiered JIT compilation to smooth out performance during startup
   * [Dynamic generic dictionary expansion](https://github.com/dotnet/runtime/pull/32270) that eliminate performance cliffs hit by generic code 
   * [Pinned object heap](https://github.com/dotnet/runtime/pull/32283) to reduce heap fragmentation caused by pinning
   * Reduce GC pause times in specific situations, like [GC lock contention](https://github.com/dotnet/coreclr/pull/27776), [Array.Copy](https://github.com/dotnet/coreclr/pull/27776)   
   * [Remove GC lock contention](https://github.com/dotnet/runtime/pull/32795)
* Single file applications -- a new [single-file publish type](https://github.com/dotnet/runtime/issues/36590) that [executes your app out of a single binary](https://github.com/dotnet/runtime/pull/36052) (for example, can be used on read-only media).
* Windows ARM64 -- [Enable .NET to run natively on Windows ARM64](https://gist.github.com/richlander/6fd855f467036a941501e5dcaceabf0a), supporting both development scenarios and deployment of client apps on customer machines. 
* ARM64 -- [Improve ARM64 performance](https://github.com/dotnet/runtime/issues/35853) (Linux and Windows) in the JIT and BCL libraries.
* Containers -- [Reduce container image size](https://github.com/dotnet/dotnet-docker/issues/1814#issuecomment-625294750) and implement [new container APIs](https://github.com/dotnet/runtime/pull/34334) to enable .NET to stay up-to-date with container runtime evolution.
* New Target Framework -- We have adopted a [new approach for .NET TFMs](https://github.com/dotnet/designs/blob/master/accepted/2020/net5/net5.md).
* Json APIs -- Enable easier [migration from Newtonsoft.Json to System.Text.Json](https://docs.microsoft.com/dotnet/standard/serialization/system-text-json-migrate-from-newtonsoft-how-to).

I'll share some more detailed information about some of these improvements, and where we see them headed.

### Windows ARM64

We are adding support for .NET to run natively on Windows ARM64. This is in addition, to Linux ARM64, which we've supported since .NET Core 3.0. With .NET 5.0, you can develop web and UI apps on Windows ARM64 devices, and deliver your applications to users who own [Surface Pro X](https://www.microsoft.com/en-us/p/surface-pro-x/8VDNRP2M6HHC) and similar devices. You can already run .NET Core and .NET Framework apps on Windows ARM64, but via x86 emulation. It's workable, but native ARM64 execution will have much better performance.

You can download and use the .NET 5.0 SDK on ARM64 with today's release. At present, you need to download and expand a zip, and it doesn't yet include Windows Forms or WPF. We're working on filling the gaps so that using .NET on Windows Forms on ARM64 is just like x64. We intend to backport the same functionality to .NET Core 3.1.

You can follow our progress at [.NET 5.0 ARM64 tracking issue](https://gist.github.com/tommcdon/6a250a1caa621892a14ea42bf1f87b4a). We're also working to generally [improve ARM64 performance](https://github.com/dotnet/runtime/issues/35853).

<img width="398" alt="2020-05-15" src="https://user-images.githubusercontent.com/2608468/82086979-20f5bd00-96a4-11ea-8d73-abed8f2505fb.png">

## New improvements in Preview 4


## BCL

Placeholder

1.	Strongly typed JSON API on HttpClient (David C)
2.	System.Text.JSON features (Layomi)
3.	Open telemetry (Tarek) – is there a consumable chunk in P4?


## Support for cgroup v2 (for containers)

.NET now has [support for cgroup v2](https://github.com/dotnet/runtime/pull/34334), which we expect will become an important container-related API in 2020 and beyond. Docker currently uses cgroup v1. In comparison, cgroup v2 is simpler, more efficient, and more secure than cgroup v1. You can learn more about [cgroup and Docker resource limits](https://devblogs.microsoft.com/dotnet/using-net-and-docker-together-dockercon-2019-update/) from our 2019 Docker update. Linux distros and containers runtimes are in the [process of adding support for cgroup v2](https://medium.com/nttlabs/cgroup-v2-596d035be4d7). .NET 5.0 will work correctly in cgroup v2 environments once they become more common. Credit to [Omair Majid](https://github.com/omajid), who supports .NET at Red Hat.

## Reducing the size of container images

We are always looking for opportunities to make .NET container images smaller and easier to use. We made a change in Preview 4 that dramatically reduces the size of the aggregate images you pull in multi-stage-build scenarios (which is a very common pattern). We [re-based the SDK image on top of the ASP.NET image](https://github.com/dotnet/dotnet-docker/pull/1848) instead of [buildpack-deps](https://hub.docker.com/_/buildpack-deps). 

This change has the following win for multi-stage builds (example usage in [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/Dockerfile)):

Multi-stage build costs with **Ubuntu 20.04 Focal**:

| Pull Image | Before | After |
| ---------- | ------ | ----- |
| `sdk:5.0-focal`        |  268 MB | 232 MB|
| `aspnet:5.0-focal`| 64 MB | 10 KB (manifest only) |

*Net download savings*: 100 MB (-30%)

Multi-stage build costs with **Debian 10 Buster**:

| Pull Image | Before | After |
| ---------- | ------ | ----- |
| `sdk:5.0`        |  280 MB | 218 MB|
| `aspnet:5.0`| 84 MB | 4 KB (manifest only) |

*Net download savings*: 146 MB (-40%)

See [dotnet/dotnet-docker #1814](https://github.com/dotnet/dotnet-docker/issues/1814#issuecomment-625294750) for more detailed information.

This change helps multi-stage builds, where the `sdk` and the `aspnet` or `runtime` image you are targeting are the same version (we expect that this is the common case). With this change, the `aspnet` pull (for example), will be a no-op, because you will have pulled the `aspnet` layers via the initial `sdk` pull.

If you want a bit more context, keep reading. For 3.1 and prior, the SDK is based on the [buildpack-deps](https://hub.docker.com/_/buildpack-deps) image. When we started producing container images, we noticed other development platforms using buildpack-deps as the base of their tools/SDK images, so we followed the established pattern. We have specifically relied on the [`scm` layer](https://github.com/docker-library/buildpack-deps/blob/1bf287b61b2c02d8890f4806a9bfb2c7042b308d/focal/scm/Dockerfile), which includes source-control tools and is based on the [`curl` layer](https://github.com/docker-library/buildpack-deps/blob/1bf287b61b2c02d8890f4806a9bfb2c7042b308d/focal/curl/Dockerfile), which includes curl and similar network tools. That means that all those tools have been available to you in the SDK images. Unfortunately, this approach has come with a big tradeoff. Since Docker only allows for a single line of inheritance (each image can only have one parent), the `sdk` image needs to carry its own copy of ASP.NET, and Docker doesn't see the actually identical ASP.NET bytes in the `sdk` image as the same as the ones in the `aspnet` image. That situation requires a lot of wasted bytes to be stored and transferred. On the other hand, people don't want to give up using the tools provided by `buildpack-deps`. 

As a compromise position, we re-based the `sdk` on `aspnet`, added [some of the tools back](https://github.com/dotnet/dotnet-docker/pull/1848#issue-404674130), and retained 90+% of the size savings.

This explanation is descriptive of what we did for Ubuntu. The story with Debian is more complicated, and responsible for the larger size win. In short, the Debian variants of `aspnet` and `runtime` are based on the `-slim` Debian variant, while `buildpack-deps` is based on the non-slim Debian images. That means that for multi-stage builds with Debian, that you pull Debian twice! Even the distro layer hasn't been shared until now.

We made similar changes for [Alpine and Nano Server](https://github.com/dotnet/dotnet-docker/pull/1832). There is no `buildpack-deps` image for either Alpine or Nano Server. However, the `sdk` images for Alpine and Nano Server were not previously built on top of the ASP.NET image. We fixed that. You will see significant size wins for Alpine and Nano Server as well with 5.0, for multi-stage builds.

We've known about these problems for a long time, but they had never been the next thing to go resolve. We decided that the 5.0 release was a good time to chase these size wins. To be honest, we were surprised with how large they were, and wish we had made the changes earlier. For the most part, we expect these changes to be a huge win. Please tell us if there are any rough edges that we didn't expect.


## .NET 5.0 will switch to the `dotnet` container repo

As part of the move to ".NET" as the product name, we are now publishing .NET 5.0 and later images to the [`mcr.microsoft.com/dotnet`](https://hub.docker.com/_/microsoft-dotnet) family of repos, instead of [`mcr.microsoft.com/dotnet/core`](https://hub.docker.com/_/microsoft-dotnet-core). Please update your `FROM` statements and scripts accordingly. .NET Core 3.1 and 2.1 will continue to be published to [`mcr.microsoft.com/dotnet/core`](https://hub.docker.com/_/microsoft-dotnet-core).

## Closing

Text here.
