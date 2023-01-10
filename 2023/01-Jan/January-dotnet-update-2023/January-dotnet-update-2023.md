---
post_title: .NET January 2023 Updates – .NET 7.0.2, .NET 6.0.13
author1: rbhanda
post_slug: january-2023-updates
username: rbhanda
microsoft_alias: rbhanda
featured_image: dotnet-bot_handybot.png
categories: .NET, Maintenance & Updates, .NET Core
summary: Check out January 2023 updates for .NET 7.0. and .NET 6.0
desired_publication_date: 2023-01-10
post_date: 2023-01-10 10:00:00
---

Today, we are releasing the [.NET January 2023 Updates](https://github.com/dotnet/announcements/issues/xxxx). These updates contain security and non-security improvements. [Your app may be vulnerable](https://github.com/dotnet/core/blob/main/release-notes/6.0/cve.md) if you have not deployed a recent .NET update.

You can download [7.0.2](https://dotnet.microsoft.com/download/dotnet/7.0) and [6.0.13](https://dotnet.microsoft.com/download/dotnet/6.0) versions for Windows, macOS, and Linux, for x86, x64, Arm32, and Arm64.

* Installers and binaries: [7.0.2](https://dotnet.microsoft.com/download/dotnet/7.0) | [6.0.13](https://dotnet.microsoft.com/download/dotnet/6.0) 
* Release notes: [7.0.2](https://github.com/dotnet/core/blob/main/release-notes/7.0/7.0.2/7.0.2.md) | [6.0.13](https://github.com/dotnet/core/blob/main/release-notes/6.0/6.0.13/6.0.13.md) | 
* [Container images](https://mcr.microsoft.com/catalog?search=dotnet/)
* Linux packages: [7.0.2](https://github.com/dotnet/core/blob/main/release-notes/7.0/install-linux.md) | [6.0.13](https://github.com/dotnet/core/blob/main/release-notes/6.0/install-linux.md)
* [Release feedback/issue](https://github.com/dotnet/core/issues/XXXX)
* Known issues: [7.0](https://github.com/dotnet/core/blob/main/release-notes/7.0/known-issues.md) | [6.0](https://github.com/dotnet/core/blob/main/release-notes/6.0/known-issues.md)

* Efcore: [7.0.2][efcore-7-0-2]
* Runtime: [7.0.2][runtime-7-0-2] | [6.0.13][runtime-6-0-13]
* SDK: [6.0.13][sdk-6-0-13]

## Security

[CVE-2023-21538 - .NET Denial of Service Vulnerability](https://msrc.microsoft.com/update-guide/vulnerability/CVE-2023-21538)

Microsoft is releasing this security advisory to provide information about a vulnerability in.NET 6.0.. This advisory also provides guidance on what developers can do to update their applications to remove this vulnerability.

A denial of service vulnerability exists in .NET 6.0 where a malicious client could cause a stack overflow which may result in a denial of service attack when an attacker sends an invalid request to an exposed endpoint.

## Visual Studio

See release notes for Visual Studio compatibility for [.NET 7.0](https://github.com/dotnet/core/blob/main/release-notes/7.0/7.0.2/7.0.2.md#visual-studio-compatibility) and [.NET 6.0](https://github.com/dotnet/core/blob/main/release-notes/6.0/6.0.13/6.0.13.md#visual-studio-compatibility).


[efcore-7-0-2]: https://github.com/dotnet/efcore/issues?q=milestone%3A7.0.2+is%3Aclosed+label%3Aservicing-approved+
[sdk-6-0-13]: https://github.com/dotnet/sdk/issues?q=milestone%3A6.0.13+is%3Aclosed+label%3Aservicing-approved+
[runtime-7-0-2]: https://github.com/dotnet/runtime/issues?q=milestone%3A7.0.2+is%3Aclosed+label%3Aservicing-approved+
[runtime-6-0-13]: https://github.com/dotnet/runtime/issues?q=milestone%3A6.0.13+is%3Aclosed+label%3Aservicing-approved+

