---
post_title: .NET Janaury 2024 Updates – .NET 8.0.1, 7.0.15, .NET 6.0.26
author1: rbhanda@microsoft.com
post_slug: janaury-2024-updates
username: rbhanda@microsoft.com
microsoft_alias: rbhanda
featured_image: dotnet-january-2024-updates.jpg
categories: .NET, Maintenance & Updates, .NET Core
summary: Check out latest Janaury 2024 updates for .NET 7.0 and .NET 6.0
post_date: 2024-01-09 10:00:00
---

Today, we are releasing the [.NET January 2024 Updates](https://github.com/dotnet/announcements/issues/289). These updates contain security and non-security improvements. [Your app may be vulnerable](https://github.com/dotnet/core/blob/main/release-notes/6.0/cve.md) if you have not deployed a recent .NET update.

You can download [8.0.1](https://dotnet.microsoft.com/download/dotnet/8.0), [7.0.15](https://dotnet.microsoft.com/download/dotnet/7.0) and, [6.0.26](https://dotnet.microsoft.com/download/dotnet/6.0) versions for Windows, macOS, and Linux, for x86, x64, Arm32, and Arm64.


* Installers and binaries: [8.0.1](https://dotnet.microsoft.com/download/dotnet/8.0) |[7.0.15](https://dotnet.microsoft.com/download/dotnet/7.0) | [6.0.26](https://dotnet.microsoft.com/download/dotnet/6.0) 
* Release notes: [8.0.11](https://github.com/dotnet/core/blob/main/release-notes/8.0/8.0.1/8.0.1.md) | [7.0.15](https://github.com/dotnet/core/blob/main/release-notes/7.0/7.0.15/7.0.15.md) | [6.0.26](https://github.com/dotnet/core/blob/main/release-notes/6.0/6.0.26/6.0.26.md)
* [Container images](https://mcr.microsoft.com/catalog?search=dotnet/)
* Linux packages: [8.0.1](https://github.com/dotnet/core/blob/main/release-notes/8.0/install-linux.md) | [7.0.15](https://github.com/dotnet/core/blob/main/release-notes/7.0/install-linux.md) | [6.0.26](https://github.com/dotnet/core/blob/main/release-notes/6.0/install-linux.md)
* [Release feedback/issue](https://github.com/dotnet/core/issues/9052)
* Known issues: [8.0](https://github.com/dotnet/core/blob/main/release-notes/8.0/known-issues.md) | [7.0](https://github.com/dotnet/core/blob/main/release-notes/7.0/known-issues.md) | [6.0](https://github.com/dotnet/core/blob/main/release-notes/6.0/known-issues.md)



### Windows Package Manager CLI (winget)

You can now install .NET updates using the Windows Package Manager CLI (winget):

* To install the .NET 8 runtime: ```winget install dotnet-runtime-8```

* To install the .NET 8 SDK: ```winget install dotnet-sdk-8```

* To update an existing installation: ```winget upgrade```

See [Install with Windows Package Manager (winget)](https://learn.microsoft.com/dotnet/core/install/windows?tabs=net80#install-with-windows-package-manager-winget) for more information.


## Improvements
* ASP.NET Core: [8.0.1][aspnet-8-0-1] | [7.0.15][aspnet-7-0-15] | [6.0.26][aspnet-6-0-26]
* Entity Framework Core: [8.0.1][efcore-8-0-1] 
* Roslyn-Analysers: [8.0.1][roslyn-8-0-1]
* Runtime: [8.0.1][runtime-8-0-1] | [7.0.15][runtime-7-0-15] | [6.0.26][runtime-6-0-26]
* SDK: [8.0.1][sdk-8-0-1]
* WPF: [8.0.1][wpf-8-0-1] 

## Security


[CVE-2024-0056 - Microsoft.Data.SqlClient and System.Data.SqlClient SQL Data provider Information Disclosure Vulnerability](https://msrc.microsoft.com/update-guide/vulnerability/CVE-2024-0056)

Microsoft is releasing this security advisory to provide information about a vulnerability in .NET's System.Data.SqlClient and Microsoft.Data.SqlClient NuGet Packages. This advisory also provides guidance on what developers can do to update their applications to address this vulnerability.

A vulnerability exists in the Microsoft.Data.SqlClient and System.Data.SqlClient SQL Data provider where an attackercan perform an AiTM (adversary-in-the-middle) attack between the SQL client and the SQL server. This may allow the attacker to steal authentication credentials intended for the database server, even if the connection is established over an encrypted channel like TLS.

[CVE-2024-0057- .NET Security Feature bypass Vulnerability](https://msrc.microsoft.com/update-guide/vulnerability/CVE-2023-36049)

Microsoft is releasing this security advisory to provide information about a vulnerability in .NET 6.0, .NET 7.0 and .NET 8.0 . This advisory also provides guidance on what developers can do to update their applications to address this vulnerability.

A security feature bypass vulnerability exists when Microsoft .NET Framework-based applications use X.509 chain building APIs but do not completely validate the X.509 certificate due to a logic flaw. An attacker could present an arbitrary untrusted certificate with malformed signatures, triggering a bug in the framework. The framework will correctly report that X.509 chain building failed, but it will return an incorrect reason code for the failure. Applications which utilize this reason code to make their own chain building trust decisions may inadvertently treat this scenario as a successful chain build. This could allow an adversary to subvert the app's typical authentication logic.

[CVE-2024-21319 - .NET Denial of Service Vulnerability](https://msrc.microsoft.com/update-guide/vulnerability/CVE-2024-21319)

Microsoft is releasing this security advisory to provide information about a vulnerability in the ASP.NET Core project templates. This advisory also provides guidance on what developers can do to update their applications to address this vulnerability.

A Denial of Service vulnerability exists in ASP.NET Core project templates which utilize JWT-based authentication tokens. This vulnerability allows an unauthenticated client to consume arbitrarily large amounts of server memory, potentially triggering an out-of-memory condition on the server and making the server no longer able to respond to legitimate requests. 

## Visual Studio

See release notes for Visual Studio compatibility for [.NET 8.0](https://github.com/dotnet/core/blob/main/release-notes/8.0/8.0.1/8.0.1.md#visual-studio-compatibility), [.NET 7.0](https://github.com/dotnet/core/blob/main/release-notes/7.0/7.0.15/7.0.15.md#visual-studio-compatibility) and, [.NET 6.0](https://github.com/dotnet/core/blob/main/release-notes/6.0/6.0.26/6.0.26.md#visual-studio-compatibility).

[efcore-6-0-26]: https://github.com/dotnet/efcore/issues?q=milestone%3A6.0.26+is%3Aclosed+label%3Aservicing-approved+
[efcore-7-0-15]: https://github.com/dotnet/efcore/issues?q=milestone%3A7.0.15+is%3Aclosed+label%3Aservicing-approved+
[efcore-8-0-1]: https://github.com/dotnet/efcore/issues?q=milestone%3A8.0.1+is%3Aclosed+label%3Aservicing-approved+
[sdk-7-0-15]: https://github.com/dotnet/sdk/issues?q=milestone%3A7.0.15+is%3Aclosed+label%3Aservicing-approved+
[sdk-6-0-26]: https://github.com/dotnet/sdk/issues?q=milestone%3A7.0.15+is%3Aclosed+label%3Aservicing-approved+
[sdk-8-0-1]: https://github.com/dotnet/sdk/issues?q=milestone%3A8.0.1+is%3Aclosed+label%3Aservicing-approved+
[runtime-7-0-15]: https://github.com/dotnet/runtime/issues?q=milestone%3A7.0.15+is%3Aclosed+label%3Aservicing-approved+
[runtime-6-0-26]: https://github.com/dotnet/runtime/issues?q=milestone%3A6.0.26+is%3Aclosed+label%3Aservicing-approved+
[runtime-8-0-1]: https://github.com/dotnet/runtime/issues?q=milestone%3A8.0.1+is%3Aclosed+label%3Aservicing-approved+
[roslyn-8-0-1]: https://github.com/dotnet/roslyn-analyzers/issues?q=milestone%3A8.0.1xx+is%3Aclosed+label%3Aservicing-approved+
[aspnet-6-0-26]: https://github.com/dotnet/aspnetcore/issues?q=milestone%3A6.0.26+is%3Aclosed+label%3Aservicing-approved+
[aspnet-7-0-15]: https://github.com/dotnet/aspnetcore/issues?q=milestone%3A7.0.15+is%3Aclosed+label%3Aservicing-approved+
[aspnet-8-0-1]: https://github.com/dotnet/aspnetcore/issues?q=milestone%3A8.0.1+is%3Aclosed+label%3Aservicing-approved+
[linker-7-0-15]: https://github.com/dotnet/linker/issues?q=milestone%3A7.0.15+is%3Aclosed+label%3Aservicing-approved+
[winforms-8-0-1]: https://github.com/dotnet/winforms/issues?q=milestone%3A8.0.1+is%3Aclosed+label%3Aservicing-approved+
[winforms-7-0-15]: https://github.com/dotnet/winforms/issues?q=milestone%3A7.0.15+is%3Aclosed+label%3Aservicing-approved+
[winforms-6-0-26]: https://github.com/dotnet/winforms/issues?q=milestone%3A6.0.26+is%3Aclosed+label%3Aservicing-approved+
[wpf-8-0-1]: https://github.com/dotnet/wpf/issues?q=milestone%3A8.0.1+is%3Aclosed+label%3Aservicing-approved+
[wpf-7-0-15]: https://github.com/dotnet/wpf/issues?q=milestone%3A7.0.15+is%3Aclosed+label%3Aservicing-approved+
[wpf-6-0-26]: https://github.com/dotnet/wpf/issues?q=milestone%3A6.0.26+is%3Aclosed+label%3Aservicing-approved+
[templating-7-0-15]: https://github.com/dotnet/templating/issues?q=milestone%3A7.0.15+is%3Aclosed+label%3Aservicing-approved+