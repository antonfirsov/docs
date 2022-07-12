---
post_title: .NET July 2022 Updates – .NET 6.0.7 and .NET Core 3.1.27
author1: dwhittaker
post_slug: july-2022-updates
username: dwhittaker
microsoft_alias: dwhittaker
featured_image: dotnet-bot_handybot.png
categories: .NET, Maintenance & Updates, .NET Core
summary: Check out July updates for .NET 6.0 and .NET Core 3.1
desired_publication_date: 2022-07-12
---

Today, we are releasing the [.NET July 2022 Updates](https://github.com/dotnet/announcements/issues/xxxx). These updates contain reliability and non-security improvements. [Your app may be vulnerable](https://github.com/dotnet/core/blob/main/release-notes/6.0/cve.md) if you have not deployed a recent .NET update.

You can download [6.0.7](https://dotnet.microsoft.com/download/dotnet/6.0) and [3.1.27](https://dotnet.microsoft.com/download/dotnet/3.1) versions for Windows, macOS, and Linux, for x86, x64, Arm32, and Arm64.

* Installers and binaries: [6.0.7](https://dotnet.microsoft.com/download/dotnet/6.0) | [3.1.27](https://dotnet.microsoft.com/download/dotnet/3.1)
* Release notes: [6.0.7](https://github.com/dotnet/core/blob/main/release-notes/6.0/6.0.7/6.0.7.md) | [3.1.27](https://github.com/dotnet/core/blob/main/release-notes/3.1/3.1.27/3.1.27.md) 
* [Container images](https://mcr.microsoft.com/catalog?search=dotnet/)
* Linux packages: [6.0.7](https://github.com/dotnet/core/blob/main/release-notes/6.0/install-linux.md) | [3.1.27](https://github.com/dotnet/core/blob/main/release-notes/3.1/install-linux.md) 
* [Release feedback/issue](https://github.com/dotnet/core/issues/XXXX)
* Known issues: [6.0](https://github.com/dotnet/core/blob/main/release-notes/6.0/known-issues.md) |  [3.1](https://github.com/dotnet/core/blob/main/release-notes/3.1/3.1-known-issues.md)

## Improvements

* ASP.NET Core: [6.0.7][aspnet-6-0-7]
* Runtime: [6.0.7][runtime-6-0-7]
* Winforms: [6.0.7][winforms-6-0-7] | [3.1.27][winforms-3-1-27]
* Wpf: [6.0.7][wpf-6-0-7]
* CoreFX: [3.1.27][corefx-3-1-27]

## Visual Studio

See release notes for Visual Studio compatibility for [.NET 6.0](https://github.com/dotnet/core/blob/main/release-notes/6.0/6.0.7/6.0.7.md#visual-studio-compatibility) and [.NET Core 3.1](https://github.com/dotnet/core/blob/main/release-notes/3.1/3.1.27/3.1.27.md#visual-studio-compatibility).

## .NET Core 3.1 End of life

.NET Core 3.1 will reach end of life on December 13, 2022, as described in [.NET Releases](https://github.com/dotnet/core/blob/main/releases.md) and per [.NET Release Policies](https://github.com/dotnet/core/blob/main/release-policies.md). After that time, .NET Core 3.1 patch updates will no longer be provided. We recommend that you move any .NET Core 3.1 applications and environments to .NET 6.0. It’ll be an easy upgrade in most cases.

The [.NET Releases page](https://github.com/dotnet/core/blob/main/releases.md) is the best place to look for release lifecycle information. Knowing key dates helps you make informed decisions about when to upgrade or make other changes to your software and computing environment.

[aspnet-6-0-7]: https://github.com/dotnet/aspnetcore/issues?q=milestone%3A6.0.7++is%3Aclosed+label%3Aservicing-approved+
[runtime-6-0-7]: https://github.com/dotnet/runtime/issues?q=milestone%3A6.0.7+is%3Aclosed+label%3Aservicing-approved+
[winforms-6-0-7]: https://github.com/dotnet/winforms/issues?q=milestone%3A6.0.7++is%3Aclosed+label%3Aservicing-approved+
[wpf-6-0-7]: https://github.com/dotnet/wpf/issues?q=milestone%3A6.0.7+is%3Aclosed+label%3Aservicing-approved+
[corefx-3-1-27]: https://github.com/dotnet/corefx/issues?q=milestone%3A3.1.27++is%3Aclosed+label%3Aservicing-approved+
[winforms-3-1-27]: https://github.com/dotnet/winforms/issues?q=milestone%3A3.1.27++is%3Aclosed+label%3Aservicing-approved+

