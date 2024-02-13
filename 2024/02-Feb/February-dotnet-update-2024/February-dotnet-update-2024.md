---
post_title: .NET February 2024 Updates – .NET 8.0.2, 7.0.16, .NET 6.0.27
author1: rbhanda@microsoft.com
post_slug: February-2024-updates
username: rbhanda@microsoft.com
microsoft_alias: rbhanda
featured_image: dotnet-february-2024-updates.jpg
categories: .NET, Maintenance & Updates, .NET Core
summary: Check out latest February 2024 updates for .NET 8.0, .NET 7.0, and .NET 6.0.
post_date: 2024-02-13 10:00:00
---

Today, we are releasing the [.NET February 2024 Updates](https://github.com/dotnet/announcements/issues/xxx). These updates contain security and non-security improvements. [Your app may be vulnerable](https://github.com/dotnet/core/blob/main/release-notes/6.0/cve.md) if you have not deployed a recent .NET update.

You can download [8.0.2](https://dotnet.microsoft.com/download/dotnet/8.0), [7.0.16](https://dotnet.microsoft.com/download/dotnet/7.0) and, [6.0.27](https://dotnet.microsoft.com/download/dotnet/6.0) versions for Windows, macOS, and Linux, for x86, x64, Arm32, and Arm64.


* Installers and binaries: [8.0.2](https://dotnet.microsoft.com/download/dotnet/8.0) |[7.0.16](https://dotnet.microsoft.com/download/dotnet/7.0) | [6.0.27](https://dotnet.microsoft.com/download/dotnet/6.0) 
* Release notes: [8.0.21](https://github.com/dotnet/core/blob/main/release-notes/8.0/8.0.2/8.0.2.md) | [7.0.16](https://github.com/dotnet/core/blob/main/release-notes/7.0/7.0.16/7.0.16.md) | [6.0.27](https://github.com/dotnet/core/blob/main/release-notes/6.0/6.0.27/6.0.27.md)
* [Container images](https://mcr.microsoft.com/catalog?search=dotnet/)
* Linux packages: [8.0.2](https://github.com/dotnet/core/blob/main/release-notes/8.0/install-linux.md) | [7.0.16](https://github.com/dotnet/core/blob/main/release-notes/7.0/install-linux.md) | [6.0.27](https://github.com/dotnet/core/blob/main/release-notes/6.0/install-linux.md)
* [Release feedback/issue](https://github.com/dotnet/core/issues/9052)
* Known issues: [8.0](https://github.com/dotnet/core/blob/main/release-notes/8.0/known-issues.md) | [7.0](https://github.com/dotnet/core/blob/main/release-notes/7.0/known-issues.md) | [6.0](https://github.com/dotnet/core/blob/main/release-notes/6.0/known-issues.md)



### Windows Package Manager CLI (winget)

You can now install .NET updates using the Windows Package Manager CLI (winget):

* To install the .NET 8 runtime: ```winget install dotnet-runtime-8```

* To install the .NET 8 SDK: ```winget install dotnet-sdk-8```

* To update an existing installation: ```winget upgrade```

See [Install with Windows Package Manager (winget)](https://learn.microsoft.com/dotnet/core/install/windows?tabs=net80#install-with-windows-package-manager-winget) for more information.


## Improvements
* ASP.NET Core: [8.0.2][aspnet-8-0-2] | [7.0.16][aspnet-7-0-15] | [6.0.27][aspnet-6-0-27]
* Entity Framework Core: [8.0.2][efcore-8-0-2] 
* Roslyn-Analysers: [8.0.2][roslyn-8-0-2]
* Runtime: [8.0.2][runtime-8-0-2] | [7.0.16][runtime-7-0-15] | [6.0.27][runtime-6-0-27]
* SDK: [8.0.2][sdk-8-0-2]

## Security


[CVE-2024-21386 - .NET Denial of Service Vulnerability](https://msrc.microsoft.com/update-guide/vulnerability/CVE-2024-21386)

Microsoft is releasing this security advisory to provide information about a vulnerability in ASP.NET 6.0, ASP.NET 7.0 and, ASP.NET 8.0 . This advisory also provides guidance on what developers can do to update their applications to address this vulnerability.

A vulnerability exists in ASP.NET applications using SignalR where a malicious client can result in a denial-of-service.

[CVE-2024-21404- .NET Denial of Service Vulnerability](https://msrc.microsoft.com/update-guide/vulnerability/CVE-2023-21404)

Microsoft is releasing this security advisory to provide information about a vulnerability in .NET 6.0, .NET 7.0 and .NET 8.0. This advisory also provides guidance on what developers can do to update their applications to remove this vulnerability.

A denial-of-service vulnerability exists in .NET with OpenSSL support when parsing X509 certificates. 

## Visual Studio

See release notes for Visual Studio compatibility for [.NET 8.0](https://github.com/dotnet/core/blob/main/release-notes/8.0/8.0.2/8.0.2.md#visual-studio-compatibility), [.NET 7.0](https://github.com/dotnet/core/blob/main/release-notes/7.0/7.0.16/7.0.16.md#visual-studio-compatibility) and, [.NET 6.0](https://github.com/dotnet/core/blob/main/release-notes/6.0/6.0.27/6.0.27.md#visual-studio-compatibility).

[efcore-6-0-27]: https://github.com/dotnet/efcore/issues?q=milestone%3A6.0.27+is%3Aclosed+label%3Aservicing-approved+
[efcore-7-0-15]: https://github.com/dotnet/efcore/issues?q=milestone%3A7.0.16+is%3Aclosed+label%3Aservicing-approved+
[efcore-8-0-2]: https://github.com/dotnet/efcore/issues?q=milestone%3A8.0.2+is%3Aclosed+label%3Aservicing-approved+
[sdk-7-0-15]: https://github.com/dotnet/sdk/issues?q=milestone%3A7.0.16+is%3Aclosed+label%3Aservicing-approved+
[sdk-6-0-27]: https://github.com/dotnet/sdk/issues?q=milestone%3A7.0.16+is%3Aclosed+label%3Aservicing-approved+
[sdk-8-0-2]: https://github.com/dotnet/sdk/issues?q=milestone%3A8.0.2+is%3Aclosed+label%3Aservicing-approved+
[runtime-7-0-15]: https://github.com/dotnet/runtime/issues?q=milestone%3A7.0.16+is%3Aclosed+label%3Aservicing-approved+
[runtime-6-0-27]: https://github.com/dotnet/runtime/issues?q=milestone%3A6.0.27+is%3Aclosed+label%3Aservicing-approved+
[runtime-8-0-2]: https://github.com/dotnet/runtime/issues?q=milestone%3A8.0.2+is%3Aclosed+label%3Aservicing-approved+
[roslyn-8-0-2]: https://github.com/dotnet/roslyn-analyzers/issues?q=milestone%3A8.0.2xx+is%3Aclosed+label%3Aservicing-approved+
[aspnet-6-0-27]: https://github.com/dotnet/aspnetcore/issues?q=milestone%3A6.0.27+is%3Aclosed+label%3Aservicing-approved+
[aspnet-7-0-15]: https://github.com/dotnet/aspnetcore/issues?q=milestone%3A7.0.16+is%3Aclosed+label%3Aservicing-approved+
[aspnet-8-0-2]: https://github.com/dotnet/aspnetcore/issues?q=milestone%3A8.0.2+is%3Aclosed+label%3Aservicing-approved+
[linker-7-0-15]: https://github.com/dotnet/linker/issues?q=milestone%3A7.0.16+is%3Aclosed+label%3Aservicing-approved+
[winforms-8-0-2]: https://github.com/dotnet/winforms/issues?q=milestone%3A8.0.2+is%3Aclosed+label%3Aservicing-approved+
[winforms-7-0-15]: https://github.com/dotnet/winforms/issues?q=milestone%3A7.0.16+is%3Aclosed+label%3Aservicing-approved+
[winforms-6-0-27]: https://github.com/dotnet/winforms/issues?q=milestone%3A6.0.27+is%3Aclosed+label%3Aservicing-approved+
[wpf-8-0-2]: https://github.com/dotnet/wpf/issues?q=milestone%3A8.0.2+is%3Aclosed+label%3Aservicing-approved+
[wpf-7-0-15]: https://github.com/dotnet/wpf/issues?q=milestone%3A7.0.16+is%3Aclosed+label%3Aservicing-approved+
[wpf-6-0-27]: https://github.com/dotnet/wpf/issues?q=milestone%3A6.0.27+is%3Aclosed+label%3Aservicing-approved+
[templating-7-0-15]: https://github.com/dotnet/templating/issues?q=milestone%3A7.0.16+is%3Aclosed+label%3Aservicing-approved+
