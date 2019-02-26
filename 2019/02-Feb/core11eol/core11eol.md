# .NET Core 1.0 and 1.1 will reach End of Life on June 27, 2019

[.NET Core 1.0](https://blogs.msdn.microsoft.com/dotnet/2016/06/27/announcing-net-core-1-0/) was released on June 27, 2016 and [.NET Core 1.1](https://blogs.msdn.microsoft.com/dotnet/2016/11/16/announcing-net-core-1-1/) was released on November 16, 2016. As an [LTS release](https://github.com/dotnet/core/blob/master/microsoft-support.md#release-types), .NET Core 1.0 is supported for three years. .NET Core 1.1 fits into the same support timeframe as .NET Core 1.0. .NET Core 1.0 and 1.1 will reach end of life and go out of support on June 27, 2019, three years after the initial .NET Core 1.0 release.

After June 27, 2019, .NET Core patch updates will no longer include updated packages or container images for .NET Core 1.0 and 1.1. You should plan your upgrade from .NET Core 1.x to 2.1 or 2.2 now.

# Upgrade to .NET Core 2.1

The supported upgrade path for .NET Core 1.x applications is via .NET Core 2.1 or 2.2. Instructions for upgrading can be found in the following documents:

* Migrate from .NET Core 1.x to 2.1 (need docs to link to)
* Migrate from ASP.NET Core 1.x to 2.1 (need docs to link to)

.NET Core 2.1 is a long-term support (LTS) release. We recommend that you make .NET Core 2.1 your new standard for .NET Core development, particularly for apps that are not updated often.

## Microsoft Support Policy

Microsoft has a published [support policy for .NET Core](https://dotnet.microsoft.com/platform/support-policy/dotnet-core). It includes policies for two release types: LTS and Current. 

.NET Core 1.0, 1.1 and 2.1 are LTS releases. .NET Core 2.0 and 2.2 are Current releases.

* **LTS** releases are designed for long-term support. They included features and components that have been stabilized, requiring few updates over a longer support release lifetime. These releases are a good choice for hosting applications that you do not intend to update.
* **Current** releases include new features that may undergo future change based on feedback. These releases are a good choice for applications in active development, giving you access to the latest features and improvements. You need to upgrade to later .NET Core releases more often to stay in support.

Both types of releases receive critical fixes throughout their lifecycle, for security, reliability, or to add support for new operating system versions. You must stay up to date with the latest patches to qualify for support.

The [.NET Core Supported OS Lifecycle Policy](https://github.com/dotnet/core/blob/master/os-lifecycle-policy.md) defines which Windows, macOS and Linux versions are supported for each .NET Core release.
