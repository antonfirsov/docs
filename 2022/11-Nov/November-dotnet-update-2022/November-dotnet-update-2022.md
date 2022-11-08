---
post_title: .NET November 2022 Updates – .NET 6.0.11
author1: dwhittaker
post_slug: november-2022-updates
username: dwhittaker
microsoft_alias: dwhittaker
featured_image: dotnet-bot_handybot.png
categories: .NET, Maintenance & Updates, .NET Core
summary: Check out November updates for .NET 6.0
desired_publication_date: 2022-11-08
post_date: 2022-11-08 11:00:00
---

Today, we are releasing the [.NET November 2022 Updates](https://github.com/dotnet/announcements/issues/xxxx). These updates contain security and non-security improvements. [Your app may be vulnerable](https://github.com/dotnet/core/blob/main/release-notes/6.0/cve.md) if you have not deployed a recent .NET update.

You can download [6.0.11](https://dotnet.microsoft.com/download/dotnet/6.0) versions for Windows, macOS, and Linux, for x86, x64, Arm32, and Arm64.

* Installers and binaries: [6.0.11](https://dotnet.microsoft.com/download/dotnet/6.0)
* Release notes: [6.0.11](https://github.com/dotnet/core/blob/main/release-notes/6.0/6.0.11/6.0.11.md)
* [Container images](https://mcr.microsoft.com/catalog?search=dotnet/)
* Linux packages: [6.0.11](https://github.com/dotnet/core/blob/main/release-notes/6.0/install-linux.md)
* [Release feedback/issue](https://github.com/dotnet/core/issues/XXXX)
* Known issues: [6.0](https://github.com/dotnet/core/blob/main/release-notes/6.0/known-issues.md)

## Improvements

* ASP.NET Core: [6.0.11][aspnet-6-0-11]
* SDK: [6.0.11][sdk-6-0-11]
* Runtime: [6.0.11][runtime-6-0-11]

## Visual Studio

See release notes for Visual Studio compatibility for [.NET 6.0](https://github.com/dotnet/core/blob/main/release-notes/6.0/6.0.10/6.0.10.md#visual-studio-compatibility) and [.NET Core 3.1](https://github.com/dotnet/core/blob/main/release-notes/3.1/3.1.30/3.1.30.md#visual-studio-compatibility).

## .NET Core 3.1 End of life

[.NET Core 3.1 will reach end of life on December 13, 2022](https://devblogs.microsoft.com/dotnet/net-core-3-1-will-reach-end-of-support-on-december-13-2022/), as described in [.NET Releases](https://github.com/dotnet/core/blob/main/releases.md) and per [.NET Release Policies](https://github.com/dotnet/core/blob/main/release-policies.md). After that time, .NET Core 3.1 patch updates will no longer be provided. We recommend that you move any .NET Core 3.1 applications and environments to .NET 6.0. It’ll be an easy upgrade in most cases.
The [.NET Releases page](https://github.com/dotnet/core/blob/main/releases.md) is the best place to look for release lifecycle information. Knowing key dates helps you make informed decisions about when to upgrade or make other changes to your software and computing environment.

[aspnet-6-0-11]: https://github.com/dotnet/aspnetcore/issues?q=milestone%3A6.0.11++is%3Aclosed+label%3Aservicing-approved+
[sdk-6-0-11]: https://github.com/dotnet/sdk/issues?q=milestone%3A6.0.11+is%3Aclosed+label%3Aservicing-approved+
[runtime-6-0-11]: https://github.com/dotnet/runtime/issues?q=milestone%3A6.0.11+is%3Aclosed+label%3Aservicing-approved+
