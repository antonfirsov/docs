---
post_title: .NET May 2022 Updates – .NET 6.0.5, .NET 5.0.17 and, .NET Core 3.1.25
username: dwhittaker@microsoft.com
microsoft_alias: dwhittaker
featured_image: dotnet-bot_handybot.png
categories: .NET, Maintenance & Updates, .NET Core
summary: Check out May updates for .NET 6.0, .NET 5.0, and .NET Core 3.1
desired_publication_date: 2022-05-10
---

Today, we are releasing the [.NET May 2022 Updates](https://github.com/dotnet/announcements/issues/xxxx). These updates contain security and non-security improvements. See the individual release notes for details on updated packages.

You can download [6.0.5](https://dotnet.microsoft.com/download/dotnet/6.0),  [5.0.17](https://dotnet.microsoft.com/download/dotnet/5.0) and, [3.1.25](https://dotnet.microsoft.com/download/dotnet/3.1) versions for Windows, macOS, and Linux, for x86, x64, Arm32, and Arm64.

* Installers and binaries: [6.0.5](https://dotnet.microsoft.com/download/dotnet/6.0) | [5.0.17](https://dotnet.microsoft.com/download/dotnet/5.0) | [3.1.25](https://dotnet.microsoft.com/download/dotnet/3.1)
* Release notes: [6.0.5](https://github.com/dotnet/core/blob/main/release-notes/6.0/6.0.5/6.0.5.md) | [5.0.17](https://github.com/dotnet/core/blob/main/release-notes/5.0/5.0.17/5.0.17.md) | [3.1.25](https://github.com/dotnet/core/blob/main/release-notes/3.1/3.1.25/3.1.25.md) 
* [Container images](https://hub.docker.com/_/microsoft-dotnet)
* Linux packages: [6.0.5](https://github.com/dotnet/core/blob/main/release-notes/6.0/install-linux.md) | [5.0.17](https://github.com/dotnet/core/blob/main/release-notes/5.0/install-linux.md) | [3.1.25](https://github.com/dotnet/core/blob/main/release-notes/3.1/install-linux.md) 
* [Release feedback/issue](https://github.com/dotnet/core/issues/XXXX)
* Known issues: [6.0](https://github.com/dotnet/core/blob/main/release-notes/6.0/known-issues.md) | [5.0](https://github.com/dotnet/core/blob/main/release-notes/5.0/5.0-known-issues.md) |  [3.1](https://github.com/dotnet/core/blob/main/release-notes/3.1/3.1-known-issues.md)

## [.NET 5.0 Out Of Support Starting May 10, 2022](https://devblogs.microsoft.com/dotnet/dotnet-5-end-of-support-update/) 
May 2022 is the last update for .NET 5.0 and Microsoft will no longer provide servicing updates, including security fixes or technical support. Please update the version of .NET you are using to a supported version (.NET 6.0) in order to continue to receive updates.

## Improvements

* ASP.NET Core: [6.0.5][aspnet-6-0-5]
* Runtime: [6.0.5][runtime-6-0-5]
* Wpf: [6.0.5][wpf-6-0-5]

### Security
 [CVE 2022-29117: .NET Denial of Service Vulnerability](https://msrc.microsoft.com/update-guide/vulnerability/CVE-2022-29117)

Microsoft is releasing this security advisory to provide information about a vulnerability in .NET 6.0, .NET 5.0 and .NET Core 3.1. This advisory also provides guidance on what developers can do to update their applications to remove this vulnerability.

A vulnerability exists in .NET 6.0, .NET 5.0 and .NET core 3.1 where a malicious client can manipulate cookies and cause a Denial of Service.

 [CVE 2022-23267: .NET Denial of Service Vulnerability](https://msrc.microsoft.com/update-guide/vulnerability/CVE-2022-23267)

Microsoft is releasing this security advisory to provide information about a vulnerability in .NET 6.0, .NET 5.0 and .NET Core 3.1. This advisory also provides guidance on what developers can do to update their applications to remove this vulnerability.

A vulnerability exists in .NET 6.0, .NET 5.0 and .NET Core 3.1 where a malicious client can cause a Denial of Service via excess memory allocations through HttpClient.

 [CVE 2022-29145: .NET Denial of Service Vulnerability](https://msrc.microsoft.com/update-guide/vulnerability/CVE-2022-29145)

Microsoft is releasing this security advisory to provide information about a vulnerability in .NET 6.0, .NET 5.0 and .NET Core 3.1. This advisory also provides guidance on what developers can do to update their applications to remove this vulnerability.

A vulnerability exists in .NET 6.0, .NET 5.0 and .NET Core 3.1 where a malicious client can can cause a denial of service when HTML forms are parsed.

## Visual Studio

See release notes for Visual Studio compatibility for [.NET 6.0](https://github.com/dotnet/core/blob/main/release-notes/6.0/6.0.5/6.0.5.md#visual-studio-compatibility), [.NET 5.0](https://github.com/dotnet/core/blob/main/release-notes/5.0/5.0.17/5.0.17.md#visual-studio-compatibility) and, [.NET Core 3.1](https://github.com/dotnet/core/blob/main/release-notes/3.1/3.1.25/3.1.25.md#visual-studio-compatibility).

## OS Lifecycle Update

Ubuntu 22.04 is now supported with the .NET 6.0.5, .NET 5.0.17, .NET Core 3.1.25 update. The operating system support pages for [.NET 6.0](https://github.com/dotnet/core/blob/master/release-notes/6.0/6.0-supported-os.md), [.NET 5.0](https://github.com/dotnet/core/blob/main/release-notes/5.0/5.0-supported-os.md), and [.NET Core 3.1](https://github.com/dotnet/core/blob/master/release-notes/3.1/3.1-supported-os.md) have been updated to reflect that.

We are also aware of an Open SSL error on Arm32 architecture and are actively working to address. This issue is being tracked [here](https://github.com/dotnet/runtime/issues/66310).

[aspnet-6-0-5]: https://github.com/dotnet/aspnetcore/issues?q=milestone%3A6.0.5++is%3Aclosed+label%3Aservicing-approved+
[runtime-6-0-5]: https://github.com/dotnet/runtime/issues?q=milestone%3A6.0.5+is%3Aclosed+label%3Aservicing-approved+
[wpf-6-0-5]: https://github.com/dotnet/wpf/issues?q=milestone%3A6.0.5+is%3Aclosed+label%3Aservicing-approved+

