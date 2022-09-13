---
post_title: .NET September 2022 Updates – .NET 6.0.9 and .NET Core 3.1.29
author1: dwhittaker
post_slug: september-2022-updates
username: dwhittaker
microsoft_alias: dwhittaker
featured_image: dotnet-bot_handybot.png
categories: .NET, Maintenance & Updates, .NET Core
summary: Check out September updates for .NET 6.0 and .NET Core 3.1
desired_publication_date: 2022-09-13
---

Today, we are releasing the [.NET September 2022 Updates](https://github.com/dotnet/announcements/issues/xxxx). These updates contain security and non-security improvements. [Your app may be vulnerable](https://github.com/dotnet/core/blob/main/release-notes/6.0/cve.md) if you have not deployed a recent .NET update.

You can download [6.0.9](https://dotnet.microsoft.com/download/dotnet/6.0) and [3.1.29](https://dotnet.microsoft.com/download/dotnet/3.1) versions for Windows, macOS, and Linux, for x86, x64, Arm32, and Arm64.

* Installers and binaries: [6.0.9](https://dotnet.microsoft.com/download/dotnet/6.0) | [3.1.28](https://dotnet.microsoft.com/download/dotnet/3.1)
* Release notes: [6.0.9](https://github.com/dotnet/core/blob/main/release-notes/6.0/6.0.9/6.0.9.md) | [3.1.29](https://github.com/dotnet/core/blob/main/release-notes/3.1/3.1.29/3.1.29.md)
* [Container images](https://mcr.microsoft.com/catalog?search=dotnet/)
* Linux packages: [6.0.9](https://github.com/dotnet/core/blob/main/release-notes/6.0/install-linux.md) | [3.1.29](https://github.com/dotnet/core/blob/main/release-notes/3.1/install-linux.md)
* [Release feedback/issue](https://github.com/dotnet/core/issues/XXXX)
* Known issues: [6.0](https://github.com/dotnet/core/blob/main/release-notes/6.0/known-issues.md) |  [3.1](https://github.com/dotnet/core/blob/main/release-notes/3.1/3.1-known-issues.md)

## Improvements

* ASP.NET Core: [6.0.9][aspnet-6-0-9]
* EFcore: [6.0.9][efcore-6-0-9]
* Runtime: [6.0.9][runtime-6-0-9]
* Winforms: [6.0.9][winforms-6-0-9]
* Wpf: [6.0.9][wpf-6-0-9]

## Security

[CVE 2022-38013: .NET Denial of Service Vulnerability](https://msrc.microsoft.com/update-guide/vulnerability/CVE-2022-38013)

Microsoft is releasing this security advisory to provide information about a vulnerability in .NET 6 and .NET Core 3.1. This advisory also provides guidance on what developers can do to update their applications to remove this vulnerability.

A denial of service vulnerability exists in ASP.NET  Core 3.1 and .NET 6.0 where a malicious client could cause a stack overflow which may result in a denial of service attack when an attacker sends a customized payload that is parsed during model binding.

## Visual Studio

See release notes for Visual Studio compatibility for [.NET 6.0](https://github.com/dotnet/core/blob/main/release-notes/6.0/6.0.9/6.0.9.md#visual-studio-compatibility) and [.NET Core 3.1](https://github.com/dotnet/core/blob/main/release-notes/3.1/3.1.29/3.1.29.md#visual-studio-compatibility).

## .NET Core 3.1 End of life

[.NET Core 3.1 will reach end of life on December 13, 2022](https://devblogs.microsoft.com/dotnet/net-core-3-1-will-reach-end-of-support-on-december-13-2022/), as described in [.NET Releases](https://github.com/dotnet/core/blob/main/releases.md) and per [.NET Release Policies](https://github.com/dotnet/core/blob/main/release-policies.md). After that time, .NET Core 3.1 patch updates will no longer be provided. We recommend that you move any .NET Core 3.1 applications and environments to .NET 6.0. It’ll be an easy upgrade in most cases.
The [.NET Releases page](https://github.com/dotnet/core/blob/main/releases.md) is the best place to look for release lifecycle information. Knowing key dates helps you make informed decisions about when to upgrade or make other changes to your software and computing environment.


[aspnet-6-0-9]: https://github.com/dotnet/aspnetcore/issues?q=milestone%3A6.0.9++is%3Aclosed+label%3Aservicing-approved+
[efcore-6-0-9]: https://github.com/dotnet/efcore/issues?q=milestone%3A6.0.9++is%3Aclosed+label%3Aservicing-approved+
[runtime-6-0-9]: https://github.com/dotnet/runtime/issues?q=milestone%3A6.0.9+is%3Aclosed+label%3Aservicing-approved+
[winforms-6-0-9]: https://github.com/dotnet/winforms/issues?q=milestone%3A6.0.9++is%3Aclosed+label%3Aservicing-approved+
[wpf-6-0-9]: https://github.com/dotnet/wpf/issues?q=milestone%3A6.0.9+is%3Aclosed+label%3Aservicing-approved+
