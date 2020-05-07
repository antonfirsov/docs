# Announcing .NET 5 Preview 4 and our Journey to one .NET

.NET 5 is the next version of .NET where we continue the journey of unifying the .NET platform. We’ve taken the best of .NET Framework and put that into .NET Core 3 with the addition of Windows desktop support with WPF and Windows Forms. As we continue the journey, our vision for one .NET is to take .NET Core and Mono/Xamarin and create a unified set of libraries and tools as well as expand on the support for cloud native and modern web development.  

Last year, we laid out our vision for one .NET and .NET 5, we said we would take .NET Core and Mono/Xamarin implementations and unify them into one base class library (BCL) and toolchain (SDK). In the wake of the global health pandemic, we have had to adapt to the changing needs of our customers and provide the support needed to assist with smooth operations. Our efforts continue to be anchored in helping our customers address their most urgent needs. As a result, we expect these features to be available in preview by Nov 2020 but the unification will be truly completed with .NET 6, our Long-Term Support (LTS) release. Our vision has not changed, but our timeline has. 

.NET 5 will have several cloud & web investments, such as smaller, faster, single file EXEs that use less memory which are appropriate for microservices and containerized applications across operating systems. We will continue to build on the work we have done.

## Download Preview 4

We are still committed to one .NET platform and delivering a quality .NET 5 release to our millions of users in November this year. You will continue to see a wave of innovation happening with multiple previews along the way on the journey to one .NET.  

You can [download .NET 5.0 Preview 4](https://dotnet.microsoft.com/download/dotnet-core/5.0), for Windows, macOS, and Linux:

* [MSIs, PKGs and zips](https://dotnet.microsoft.com/download/dotnet-core/5.0)
* [Docker images](https://hub.docker.com/_/microsoft-dotnet-core)
* [Snap installer](https://snapcraft.io/dotnet-sdk)

ASP.NET Core and EF Core are also being released today.

You need to use Visual Studio 2019 16.6 to use .NET 5.0. Install the latest version of the [C# extension](https://code.visualstudio.com/Docs/languages/csharp), to use .NET 5.0 with Visual Studio Code. .NET 5.0 isn't yet supported with Visual Studio for Mac.

Release notes:

* .NET 5.0 release notes
* .NET 5.0 known issues
* .NET Core 3.1 -> .NET 5.0 API diff
* GitHub release
* GitHub tracking issue

Let's look at some of the improvements in Preview 4.

## Windows ARM64

We are adding support for .NET to run natively on Windows ARM64. This is in addition, to Linux ARM64, which we've supported since .NET Core 3.0. With .NET 5.0, you can develop cloud and UI apps on Windows ARM64 devices, and deliver your applications to users who own [Surface Pro X](https://www.microsoft.com/en-us/p/surface-pro-x/8VDNRP2M6HHC) and similar devices. You can currently run .NET Core and .NET Framework apps on Windows ARM64, but via x86 emulation. It's workable, but not the same as a native app.

You can download and use the .NET 5.0 SDK on ARM64 with today's release. At present, you need to download and expand a zip, and it doesn't yet include Windows Forms or WPF. We're working on filling the gaps so that using .NET on Windows Forms on ARM64 is just like x64. We intend to backport the same functionality to .NET Core 3.1.

You can following our progress at [.NET 5.0 ARM64 tracking issue](https://gist.github.com/tommcdon/6a250a1caa621892a14ea42bf1f87b4a). We're also working to [improve ARM64 performance](https://github.com/dotnet/runtime/issues/35853), generally.

## BCL

Placeholder

1.	Strongly typed JSON API on HttpClient (David C)
2.	System.Text.JSON features (Layomi)
3.	Open telemetry (Tarek) – is there a consumable chunk in P4?

## Containers -- container images

We are always looking for opportunities to make .NET container images smaller and easier to use. We made a change in Preview 4 that dramatically reduces the size of the aggregate images you pull in multi-stage-build scenarios (which is a very common pattern). We [re-based the SDK image on top of the ASP.NET image](https://github.com/dotnet/dotnet-docker/pull/1848) instead of [buildpack-deps](https://hub.docker.com/_/buildpack-deps). 

This change has the following win, for multi-stage builds (example usage in [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/Dockerfile)):

Ubuntu 20.04 -- Focal

| Pull Image | Before | After |
| ---------- | ------ | ----- |
| `sdk:5.0-focal`        |  268MB | 232 MB|
| `aspnet:5.0-focal`| 64MB | 10 KB (manifest only) |

*Net download savings*: 100 MB (-30%)

Debian 10 -- Buster

| Pull Image | Before | After |
| ---------- | ------ | ----- |
| `sdk:5.0`        |  280MB | 218 MB|
| `aspnet:5.0`| 84MB | 4 KB (manifest only) |

*Net download savings*: 146 MB (-40%)

This change primarily helps multi-stage builds, where the `sdk` and the `aspnet` or `runtime` image you are targeting are the same version (we expect that this is the common case). With this change, the `aspnet` pull (for example), will be a no-op, because you will have pulled the `aspnet` layers via the initial `sdk` pull.

If you want a bit more context, keep reading. For 3.1 and prior, the SDK is based on the [buildpack-deps](https://hub.docker.com/_/buildpack-deps) image. When we started producing container images, we noticed other development platforms using buildpack-deps as the base of their tools/SDK images, so we followed the established pattern. We have specifically relied on the ['scm' layer](https://github.com/docker-library/buildpack-deps/blob/1bf287b61b2c02d8890f4806a9bfb2c7042b308d/focal/scm/Dockerfile), which includes source-control tools, and is based on the [`curl` layer](https://github.com/docker-library/buildpack-deps/blob/1bf287b61b2c02d8890f4806a9bfb2c7042b308d/focal/curl/Dockerfile), which includes curl and similar network tools. That means that all those tools have been available to you in the SDK images. Unfortuantely, since Docker only allows for a single line of inheritance (each image can only have one parent), that means that the `sdk` image needs to carry its own copy of ASP.NET, and Docker doesn't see the actually identical ASP.NET bytes in the `sdk` image as the same as the ones in the `aspnet` image. That presents a lot of waste. As a middle ground, we added [some of the tools back](https://github.com/dotnet/dotnet-docker/pull/1848#issue-404674130), which is a very small cost relative to the approach we were previously using.

This explanation is descriptive of what we did for Ubuntu. The story with Debian is more complicated, and the reason for the larger size win. In short, the Debian variants of `aspnet` and `runtime` are based on the `-slim` Debian variant, while `buildpack-deps` is based on the non-slim Debian images. That means that for muti-stage builds with Debian, that you pull Debian twice! Even the distro layer hasn't been shared.

We made similar changes for [Alpine and Nano Server](https://github.com/dotnet/dotnet-docker/pull/1832). There is no `buildpack-deps` for either Alpine or Nano Server. However, the `sdk` images for Alpine and Nano Server were not previously built on top of the ASP.NET image. We fixed that. You will see significant size wins for Alpine and Nano Server as well, for multi-stage builds.

We've known about these problems for a long time, but they were never been the next thing to go resolve. We decided that the 5.0 release was a good time to chase these size wins. To be honest, we were surprised with how large they were, and wish we had made the changes earlier. For the most part, we expect these changes to be a huge win. Please tell us if there are any rough edges that we didn't expect.


## Containers -- Repo change

As part of the move to ".NET" as the product name, we are now pubishing .NET 5.0 and later images to the [`mcr.microsoft.com/dotnet`](https://hub.docker.com/_/microsoft-dotnet) family of repos, instead of [`mcr.microsoft.com/dotnet/core`](https://hub.docker.com/_/microsoft-dotnet-core). Please update your `FROM` statements and scripts accordingly. 


## Containers -- cgroup v2

.NET now has support for cgroup v2, which is we expect will become an important container-related API in 2020 and beyond. Docker currently uses cgroup v1. In comparison, cgroup v2 is simpler, more efficient and more secure than cgroup v1. You can learn more about [cgroup and Docker resource limits](https://devblogs.microsoft.com/dotnet/using-net-and-docker-together-dockercon-2019-update/) from our 2019 Docker update. Linux distros and containers runtimes are in the [process of adding support for cgroup v2](https://medium.com/nttlabs/cgroup-v2-596d035be4d7). .NET 5.0 will work correctly in cgroup v2 environments, once they become more common. Credit to [Omair Majid](https://github.com/omajid), who supports .NET at Red Hat.

## Closing

Text here.