---
post_title: .NET December 2022 Updates – .NET 7.0.1, .NET 6.0.12, .NET Core 3.1.32
author1: dwhittaker
post_slug: december-2022-updates
username: dwhittaker
microsoft_alias: dwhittaker
featured_image: dotnet-bot_handybot.png
categories: .NET, Maintenance & Updates, .NET Core
summary: Check out December updates for .NET 7.0., .NET 6.0, and .NET Core 3.1
desired_publication_date: 2022-12-13
post_date: 2022-12-13 10:00:00
---

Today, we are releasing the [.NET December 2022 Updates](https://github.com/dotnet/announcements/issues/xxxx). These updates contain security and non-security improvements. [Your app may be vulnerable](https://github.com/dotnet/core/blob/main/release-notes/6.0/cve.md) if you have not deployed a recent .NET update.

You can download [7.0.1](https://dotnet.microsoft.com/download/dotnet/7.0), [6.0.12](https://dotnet.microsoft.com/download/dotnet/6.0), and [3.1.32](https://dotnet.microsoft.com/download/dotnet/3.1) versions for Windows, macOS, and Linux, for x86, x64, Arm32, and Arm64.

* Installers and binaries: [7.0.1](https://dotnet.microsoft.com/download/dotnet/7.0) | [6.0.12](https://dotnet.microsoft.com/download/dotnet/6.0) | [3.1.32](https://dotnet.microsoft.com/download/dotnet/3.1)
* Release notes: [7.0.1](https://github.com/dotnet/core/blob/main/release-notes/7.0/7.0.1/7.0.1.md) | [6.0.12](https://github.com/dotnet/core/blob/main/release-notes/6.0/6.0.12/6.0.12.md) | [3.1.32](https://github.com/dotnet/core/blob/main/release-notes/3.1/3.1.32/3.1.32.md)
* [Container images](https://mcr.microsoft.com/catalog?search=dotnet/)
* Linux packages: [7.0.1](https://github.com/dotnet/core/blob/main/release-notes/7.0/install-linux.md) | [6.0.12](https://github.com/dotnet/core/blob/main/release-notes/6.0/install-linux.md) | [3.1.32](https://github.com/dotnet/core/blob/main/release-notes/3.1/install-linux.md)
* [Release feedback/issue](https://github.com/dotnet/core/issues/XXXX)
* Known issues: [7.0](https://github.com/dotnet/core/blob/main/release-notes/7.0/known-issues.md) | [6.0](https://github.com/dotnet/core/blob/main/release-notes/6.0/known-issues.md) |  [3.1](https://github.com/dotnet/core/blob/main/release-notes/3.1/3.1-known-issues.md)

## Improvements

* ASP.NET Core: [7.0.1][aspnet-7-0-1] | [6.0.12][aspnet-6-0-12] | [3.1.32][aspnet-3-1-32]
* Installer: [7.0.1][installer-7-0-1]
* SDK: [7.0.1][sdk-7-0-1] | [6.0.12][sdk-6-0-12]
* Runtime: [7.0.1][runtime-7-0-1] | [6.0.12][runtime-6-0-12]
* Templating: [7.0.1][templating-7-0-1]
* Winforms: [7.0.1][winforms-7-0-1]

## Security
[CVE-2022-41089 - .NET Remote Code Execution Vulnerability](https://msrc.microsoft.com/update-guide/vulnerability/CVE-2022-41089)

Microsoft is releasing this security advisory to provide information about a vulnerability in .NET Core 3.1, .NET 6.0., and .NET 7.0. This advisory also provides guidance on what developers can do to update their applications to remove this vulnerability.

A remote code execution vulnerability exists in .NET Core 3.1, .NET 6.0, and .NET 7.0, where a malicious actor could cause a user to run arbitrary code as a result of parsing maliciously crafted xps files.

## Visual Studio

See release notes for Visual Studio compatibility for [.NET 7.0](https://github.com/dotnet/core/blob/main/release-notes/7.0/7.0.1/7.0.1.md#visual-studio-compatibility), [.NET 6.0](https://github.com/dotnet/core/blob/main/release-notes/6.0/6.0.12/6.0.12.md#visual-studio-compatibility) and [.NET Core 3.1](https://github.com/dotnet/core/blob/main/release-notes/3.1/3.1.32/3.1.32.md#visual-studio-compatibility).

## .NET Core 3.1 End of life

[.NET Core 3.1 will reach end of life on December 13, 2022](https://devblogs.microsoft.com/dotnet/net-core-3-1-will-reach-end-of-support-on-december-13-2022/), as described in [.NET Releases](https://github.com/dotnet/core/blob/main/releases.md) and per [.NET Release Policies](https://github.com/dotnet/core/blob/main/release-policies.md). After that time, .NET Core 3.1 patch updates will no longer be provided. We recommend that you move any .NET Core 3.1 applications and environments to .NET 6.0. It’ll be an easy upgrade in most cases.
The [.NET Releases page](https://github.com/dotnet/core/blob/main/releases.md) is the best place to look for release lifecycle information. Knowing key dates helps you make informed decisions about when to upgrade or make other changes to your software and computing environment.

[aspnet-7-0-1]: https://github.com/dotnet/aspnetcore/issues?q=milestone%3A7.0.1++is%3Aclosed+label%3Aservicing-approved+
[aspnet-6-0-12]: https://github.com/dotnet/aspnetcore/issues?q=milestone%3A6.0.12++is%3Aclosed+label%3Aservicing-approved+
[aspnet-3-1-32]: https://github.com/dotnet/aspnetcore/issues?q=milestone%3A3.1.32++is%3Aclosed+label%3Aservicing-approved+
[installer-7-0-1]: https://github.com/dotnet/installer/issues?q=milestone%3A7.0.1+is%3Aclosed+label%3Aservicing-approved+
[sdk-7-0-1]: https://github.com/dotnet/sdk/issues?q=milestone%3A7.0.1+is%3Aclosed+label%3Aservicing-approved+
[sdk-6-0-12]: https://github.com/dotnet/sdk/issues?q=milestone%3A6.0.12+is%3Aclosed+label%3Aservicing-approved+
[runtime-7-0-1]: https://github.com/dotnet/runtime/issues?q=milestone%3A7.0.1+is%3Aclosed+label%3Aservicing-approved+
[runtime-6-0-12]: https://github.com/dotnet/runtime/issues?q=milestone%3A6.0.12+is%3Aclosed+label%3Aservicing-approved+
[templating-7-0-1]: https://github.com/dotnet/templating/issues?q=milestone%3A7.0.1+is%3Aclosed+label%3Aservicing-approved+
[winforms-7-0-1]: https://github.com/dotnet/winforms/issues?q=milestone%3A7.0.1+is%3Aclosed+label%3Aservicing-approved+
