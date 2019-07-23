# Announcing .NET Core 3.0 Preview 7

Today, we are announcing [.NET Core 3.0 Preview 7](https://dotnet.microsoft.com/download/dotnet-core/3.0). We've transitioned from creating new features to polishing the release. Expect a singular focus on quality for the remaining preview releases.

[Download .NET Core 3.0 Preview 7](https://dotnet.microsoft.com/download/dotnet-core/3.0) right now on Windows, macOS and Linux.

ASP.NET Core and EF Core are also releasing updates today.

Details:

* [.NET Core 3.0 release notes](https://github.com/dotnet/core/tree/master/release-notes/3.0)
* [API diff](https://github.com/dotnet/core/blob/master/release-notes/3.0/preview/api-diff/preview6/3.0-preview7.md)
* [GitHub release](https://github.com/dotnet/core/releases/tag/v3.0.0-preview7)

The [Microsoft .NET Site](https://dotnet.microsoft.com/) has been updated to .NET Core 3.0 Preview 7. It's been running successfully on Preview 7 for over two weeks, on [Azure WebApps](https://azure.microsoft.com/en-us/services/app-service/web/) (as a self-contained app). We will likely move the site to Preview 8 builds in a couple of weeks.

If you missed it, check out the improvements we released in [.NET Core 3.0 Preview 6](https://devblogs.microsoft.com/dotnet/announcing-net-core-3-0-preview-6/), from last month.

## Go Live

NET Core 3.0 Preview 7 is supported by Microsoft and can be used in production. We strongly recommend that you test your app running on Preview 7 before deploying Preview 7 into production. If you find an issue with .NET Core 3.0, please file a GitHub issue and/or contact Microsoft support.

We intend to make very few changes after Preview 7 for most APIs. Notable exceptions are: WPF,Windows Forms, Blazor and Entity Framework. Any breaking changes after Preview 7 will be documented.

We are working to ensure a high degree of compatibility with .NET Core 1.x and 2.x apps, making it straightforward to upgrade existing apps to .NET Core 3.0.

## .NET Core SDK Size Improvements

The .NET Core SDK is significantly smaller with .NET Core 3.0. The primary reason is that we changed the way we construct the SDK, by moving to purpose-built “packs” of various kinds (reference assemblies, frameworks, templates). In previous versions (including .NET Core 2.2), we constructed the SDK from NuGet packages, which included many artifacts that were not required and wasted a lot of space.

.NET Core 3.0 SDK Size (size change in brackets)

| Operating System | Installer Size (change) | On-disk Size (change)   |
| ---------------- | ----------------------- | ----------------------- |
| Windows          | 164MB (-440KB; 0%)      | 441MB (-968MB; -68.7%)  |
| Linux            | 115MB (-55MB; -32%)     | 332MB (-1068MB; -76.2%) |
| macOS            | 118MB (-51MB; -30%)     | 337MB (-1063MB; -75.9%) |

The size improvements for Linux and macOS are dramatic. The improvement for Windows is smaller because we have added WPF and Windows Forms as part of .NET Core 3.0. It’s amazing that we added WPF and Windows Forms in 3.0 and the installer is still (a little bit) smaller.

You can see the same benefit with [.NET Core SDK Docker images](https://hub.docker.com/_/microsoft-dotnet-core-sdk) (here, limited to x64 Debian and Alpine).

| Distro | 2.2 Size | 3.0 Size |
| ------ | -------- | -------- |
| Debian | 1.74GB   | 706MB    |
| Alpine | 1.48GB   | 422MB    |

You can see how we calculated these file sizes in [.NET Core 3.0 SDK Size Improvements](https://gist.github.com/richlander/9dbb7cf0a9a53bfd161903ba4f20a1f6). Detailed instructions are provided so that you can run the same tests in your own environment.

## Closing

The .NET Core 3.0 release is coming close to completion, and the team is now solely focused on quality now that we're no longer building new features. Please tell us about any issues you find, ideally as quickly as possible. We want to get as many fixes in as possible before we ship the final 3.0 release.

We recommend that you start planning to adopt .NET Core 3.0. This recommendation is stronger if you are using containers. The [3.0 improvements for containers](https://devblogs.microsoft.com/dotnet/using-net-and-docker-together-dockercon-2019-update/) are critical for anyone using docker resource limits directly or via an orchestrator.
