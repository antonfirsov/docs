---
post_title: .NET August 2022 Updates – .NET 6.0.8 and .NET Core 3.1.28
author1: dwhittaker
post_slug: august-2022-updates
username: dwhittaker
microsoft_alias: dwhittaker
featured_image: dotnet-bot_handybot.png
categories: .NET, Maintenance & Updates, .NET Core
summary: Check out July updates for .NET 6.0 and .NET Core 3.1
desired_publication_date: 2022-08-09
---

Today, we are releasing the [.NET August 2022 Updates](https://github.com/dotnet/announcements/issues/xxxx). These updates contain security and non-security improvements. [Your app may be vulnerable](https://github.com/dotnet/core/blob/main/release-notes/6.0/cve.md) if you have not deployed a recent .NET update.

You can download [6.0.8](https://dotnet.microsoft.com/download/dotnet/6.0) and [3.1.28](https://dotnet.microsoft.com/download/dotnet/3.1) versions for Windows, macOS, and Linux, for x86, x64, Arm32, and Arm64.

* Installers and binaries: [6.0.8](https://dotnet.microsoft.com/download/dotnet/6.0) | [3.1.28](https://dotnet.microsoft.com/download/dotnet/3.1)
* Release notes: [6.0.8](https://github.com/dotnet/core/blob/main/release-notes/6.0/6.0.8/6.0.8.md) | [3.1.28](https://github.com/dotnet/core/blob/main/release-notes/3.1/3.1.28/3.1.28.md)
* [Container images](https://mcr.microsoft.com/catalog?search=dotnet/)
* Linux packages: [6.0.8](https://github.com/dotnet/core/blob/main/release-notes/6.0/install-linux.md) | [3.1.28](https://github.com/dotnet/core/blob/main/release-notes/3.1/install-linux.md)
* [Release feedback/issue](https://github.com/dotnet/core/issues/XXXX)
* Known issues: [6.0](https://github.com/dotnet/core/blob/main/release-notes/6.0/known-issues.md) |  [3.1](https://github.com/dotnet/core/blob/main/release-notes/3.1/3.1-known-issues.md)

## Improvements

* Runtime: [6.0.8][runtime-6-0-8]

## Security

[CVE 2022-34716: .NET Information Disclosure Vulnerability](https://msrc.microsoft.com/update-guide/vulnerability/CVE-2022-34716)

Microsoft is releasing this security advisory to provide information about a vulnerability in .NET 6 and .NET Core 3.1. This advisory also provides guidance on what developers can do to update their applications to remove this vulnerability.

An information disclosure vulnerability exists in .NET 6 and .NET Core 3.1 that could lead to unauthorized access of priviledged information.

## Visual Studio

See release notes for Visual Studio compatibility for [.NET 6.0](https://github.com/dotnet/core/blob/main/release-notes/6.0/6.0.8/6.0.8.md#visual-studio-compatibility) and [.NET Core 3.1](https://github.com/dotnet/core/blob/main/release-notes/3.1/3.1.28/3.1.28.md#visual-studio-compatibility).

## .NET Core 3.1 End of life

[.NET Core 3.1 will reach end of life on December 13, 2022](https://devblogs.microsoft.com/dotnet/net-core-3-1-will-reach-end-of-support-on-december-13-2022/), as described in [.NET Releases](https://github.com/dotnet/core/blob/main/releases.md) and per [.NET Release Policies](https://github.com/dotnet/core/blob/main/release-policies.md). After that time, .NET Core 3.1 patch updates will no longer be provided. We recommend that you move any .NET Core 3.1 applications and environments to .NET 6.0. It’ll be an easy upgrade in most cases.

The [.NET Releases page](https://github.com/dotnet/core/blob/main/releases.md) is the best place to look for release lifecycle information. Knowing key dates helps you make informed decisions about when to upgrade or make other changes to your software and computing environment.

[runtime-6-0-8]: https://github.com/dotnet/runtime/issues?q=milestone%3A6.0.8+is%3Aclosed+label%3Aservicing-approved+
