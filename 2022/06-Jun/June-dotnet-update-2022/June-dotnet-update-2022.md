---
post_title: .NET June 2022 Updates – .NET 6.0.6 and .NET Core 3.1.26
author1: dwhittaker
post_slug: june-2022-updates
microsoft_alias: dwhittaker
featured_image: dotnet-bot_handybot.png
categories: .NET, Maintenance & Updates, .NET Core
summary: Check out May updates for .NET 6.0 and .NET Core 3.1
desired_publication_date: 2022-06-14
---

Today, we are releasing the [.NET June 2022 Updates](https://github.com/dotnet/announcements/issues/224). These updates contain security and non-security improvements. [Your app may be vulnerable](https://github.com/dotnet/core/blob/main/release-notes/6.0/cve.md) if you have not deployed the latest .NET update.

You can download [6.0.6](https://dotnet.microsoft.com/download/dotnet/6.0) and [3.1.26](https://dotnet.microsoft.com/download/dotnet/3.1) versions for Windows, macOS, and Linux, for x86, x64, Arm32, and Arm64.

* Installers and binaries: [6.0.6](https://dotnet.microsoft.com/download/dotnet/6.0) | [3.1.26](https://dotnet.microsoft.com/download/dotnet/3.1)
* Release notes: [6.0.6](https://github.com/dotnet/core/blob/main/release-notes/6.0/6.0.6/6.0.6.md) | [3.1.26](https://github.com/dotnet/core/blob/main/release-notes/3.1/3.1.26/3.1.26.md) 
* [Container images](https://hub.docker.com/_/microsoft-dotnet)
* Linux packages: [6.0.6](https://github.com/dotnet/core/blob/main/release-notes/6.0/install-linux.md) | [3.1.26](https://github.com/dotnet/core/blob/main/release-notes/3.1/install-linux.md) 
* [Release feedback/issue](https://github.com/dotnet/core/issues/7536)
* Known issues: [6.0](https://github.com/dotnet/core/blob/main/release-notes/6.0/known-issues.md) |  [3.1](https://github.com/dotnet/core/blob/main/release-notes/3.1/3.1-known-issues.md)

## Improvements

* ASP.NET Core: [6.0.6][aspnet-6-0-6]
* EFcore: [6.0.6][efcore-6-0-6]
* Runtime: [6.0.6][runtime-6-0-6]
* Winforms: [6.0.6][winforms-6-0-6]
* Wpf: [6.0.6][wpf-6-0-6]
* CoreCLR: [3.1.26][coreclr-3-1-26]

## Security

[CVE 2022-30184: .NET Information Disclosure Vulnerability](https://msrc.microsoft.com/update-guide/vulnerability/CVE-2022-30184)

Microsoft is releasing this security advisory to provide information about a vulnerability in .NET 6 and .NET Core 3.1. This advisory also provides guidance on what developers can do to update their applications to remove this vulnerability.

A vulnerability exists in .NET 6 and .NET Core 3.1 within Nuget where a credential leak can occur.

## Visual Studio

See release notes for Visual Studio compatibility for [.NET 6.0](https://github.com/dotnet/core/blob/main/release-notes/6.0/6.0.6/6.0.6.md#visual-studio-compatibility) and [.NET Core 3.1](https://github.com/dotnet/core/blob/main/release-notes/3.1/3.1.26/3.1.26.md#visual-studio-compatibility).

## .NET 5 is Out of Support

[.NET 5 is now out of support](https://devblogs.microsoft.com/dotnet/dotnet-5-end-of-support-update/). Starting with the June 2022 servicing update for Visual Studio 2019 16.11 and Visual Studio 2019 16.9, the .NET 5 component in Visual Studio will be marked out of support and made optional (not installed by default). This means that workloads in Visual Studio may be installed without installing .NET 5 and customers can continue to target .NET Core 3.1 and .NET Framework in a supported configuration. Note that existing installations won’t be affected and any previously installed workload and component will remain installed until the component or workload is unselected in Visual Studio setup. While it's possible for you to re-select this optional component in Visual Studio and re-install this, we strongly recommend you use .NET 6 with Visual Studio 2022 to build apps that run on a supported .NET runtime.

## .NET Core 3.1 Out of Support on December 13, 2022

.NET Core 3.1 will be out of support on December 13, 2022, as described in [.NET Releases](https://github.com/dotnet/core/blob/main/releases.md) and per [.NET Release Policies](https://github.com/dotnet/core/blob/main/release-policies.md). After that time, .NET Core 3.1 patch updates will no longer be provided. We recommend that you move any .NET Core 3.1 applications and environments to .NET 6.

The [.NET Releases page](https://github.com/dotnet/core/blob/main/releases.md) is the best place to look for release lifecycle information. Knowing key dates helps you make informed decisions about when to upgrade or make other changes to your software and computing environment.

[aspnet-6-0-6]: https://github.com/dotnet/aspnetcore/issues?q=milestone%3A6.0.6++is%3Aclosed+label%3Aservicing-approved+
[efcore-6-0-6]: https://github.com/dotnet/efcore/issues?q=milestone%3A6.0.6++is%3Aclosed+label%3Aservicing-approved+
[runtime-6-0-6]: https://github.com/dotnet/runtime/issues?q=milestone%3A6.0.6+is%3Aclosed+label%3Aservicing-approved+
[winforms-6-0-6]: https://github.com/dotnet/efcore/issues?q=milestone%3A6.0.6++is%3Aclosed+label%3Aservicing-approved+
[wpf-6-0-6]: https://github.com/dotnet/wpf/issues?q=milestone%3A6.0.6+is%3Aclosed+label%3Aservicing-approved+
[coreclr-3-1-26]: https://github.com/dotnet/efcore/issues?q=milestone%3A3.1.26++is%3Aclosed+label%3Aservicing-approved+

