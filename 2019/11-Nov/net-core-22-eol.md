[.NET Core 2.2][1] was released on December 4, 2018. As a non-LTS ("Current") release, it is supported for three months after the next release. [.NET Core 3.0][2] was released on September 23, 2019. As a result, .NET Core 2.2 is supported until December 23, 2019.

After that time, .NET Core patch updates will no longer include updated packages of container images for .NET Core 2.2. You should plan your upgrade from .NET Core 2.2 now.

[.NET Core 3.1][8] released December 3, 2019 as a [Long-term support release][5]. As a result, .NET Core 3.0, released September 23, 2019 is supported until March 23, 2020.

## Upgrade to .NET Core 3.1 {#upgrade-to-net-core-3-1}

The supported upgrade path from .NET Core 2.2 is via .NET Core 3.1. Migrating from 2.2 to 3.1 is straightforward; update the project file to use 3.1 rather than 2.2. The first document below illustrates the process from 2.0 to 2.1. ASP.NET Core 2.2 to 3.1 has additional considerations detailed in the second document.

*   [Migrate from .NET Core 2.0 to 2.1][3]
*   [Migrate from ASP.NET Core 2.2 to 3.0][4]

## Microsoft Support Policy {#microsoft-support-policy}

Microsoft has a [published support policy][6] for .NET Core. It includes policies for two release types: LTS and Current.

*   LTS releases include features and components that have been stabilized, requiring few updates over a longer support release lifetime. These releases are a good choice for hosting applications that you do not intend to update often.
*   Current releases include features and components that are new and may undergo future change based on feedback. These releases are a good choice for applications in active development, giving you access to the latest features and improvements. You need to upgrade to later .NET Core releases more often to stay in support.

Both types of releases receive critical fixes throughout their lifecycle, for security, reliability, or to add support for new operating system versions. You must stay up-to-date with the latest patches to qualify for support.

See [.NET Core Supported OS Lifecycle Policy][7] to learn about Windows, macOS and Linux versions that are supported for each .NET Core release.

 [1]: https://devblogs.microsoft.com/dotnet/announcing-net-core-2-2/
 [2]: https://devblogs.microsoft.com/dotnet/announcing-net-core-3-0/
 [3]: https://docs.microsoft.com/dotnet/core/migration/20-21
 [4]: https://docs.microsoft.com/en-us/aspnet/core/migration/22-to-30?view=aspnetcore-3.0
 [5]: https://devblogs.microsoft.com/dotnet/introducing-net-5/
 [6]: https://github.com/dotnet/core/blob/master/microsoft-support.md
 [7]: https://github.com/dotnet/core/blob/master/os-lifecycle-policy.md
 [8]: https://devblogs.microsoft.com/dotnet/announcing-net-core-3-1/
