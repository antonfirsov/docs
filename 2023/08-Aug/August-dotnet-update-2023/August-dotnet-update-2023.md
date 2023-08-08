---
post_title: .NET August 2023 Updates – .NET 7.0.10, .NET 6.0.21
author1: rbhanda@microsoft.com
post_slug: August-2023-updates
username: rbhanda@microsoft.com
microsoft_alias: rbhanda
featured_image: dotnet-bot_handybot.png
categories: .NET, Maintenance & Updates, .NET Core
summary: Check out latest August 2023 updates for .NET 7.0 and .NET 6.0
post_date: 2023-08-08 10:00:00
---


Today, we are releasing the [.NET August 2023 Updates](https://github.com/dotnet/announcements/issues/xxx). These updates contain security and non-security improvements. [Your app may be vulnerable](https://github.com/dotnet/core/blob/main/release-notes/6.0/cve.md) if you have not deployed a recent .NET update.

You can download [7.0.10](https://dotnet.microsoft.com/download/dotnet/7.0) and [6.0.21](https://dotnet.microsoft.com/download/dotnet/6.0) versions for Windows, macOS, and Linux, for x86, x64, Arm32, and Arm64.


* Installers and binaries: [7.0.10](https://dotnet.microsoft.com/download/dotnet/7.0) | [6.0.21](https://dotnet.microsoft.com/download/dotnet/6.0) 
* Release notes: [7.0.10](https://github.com/dotnet/core/blob/main/release-notes/7.0/7.0.10/7.0.10.md) | [6.0.21](https://github.com/dotnet/core/blob/main/release-notes/6.0/6.0.21/6.0.21.md)
* [Container images](https://mcr.microsoft.com/catalog?search=dotnet/)
* Linux packages: [7.0.10](https://github.com/dotnet/core/blob/main/release-notes/7.0/install-linux.md) | [6.0.21](https://github.com/dotnet/core/blob/main/release-notes/6.0/install-linux.md)
* [Release feedback/issue](https://github.com/dotnet/core/issues/8673)
* Known issues: [7.0](https://github.com/dotnet/core/blob/main/release-notes/7.0/known-issues.md) | [6.0](https://github.com/dotnet/core/blob/main/release-notes/6.0/known-issues.md)

### Windows Package Manager CLI (winget)

You can now install .NET updates using the Windows Package Manager CLI (winget):

* To install the .NET 7 runtime: ```winget install dotnet-runtime-7```

* To install the .NET 7 SDK: ```winget install dotnet-sdk-7```

* To update an existing installation: ```winget upgrade```

See [Install with Windows Package Manager (winget)](https://learn.microsoft.com/dotnet/core/install/windows?tabs=net70#install-with-windows-package-manager-winget) for more information.

## Improvements

* ASP.NET Core: [7.0.10][aspnet-7-0-10] | [6.0.21][aspnet-6-0-21]
* Entity Framework Core: [7.0.10][efcore-7-0-10] | [6.0.21][efcore-6-0-21]
* Runtime: [7.0.10][runtime-7-0-10] | [6.0.21][runtime-6-0-21]
* SDK: [7.0.10][sdk-7-0-10]
* WPF: [7.0.10][wpf-7-0-10] 

## Security

[CVE-2023-38178 - .NET Denial of Service Vulnerability](https://msrc.microsoft.com/update-guide/vulnerability/CVE-2023-38178)

Microsoft is releasing this security advisory to provide information about a vulnerability in ASP.NET Core 2.1 and .NET 7.0. This advisory also provides guidance on what developers can do to update their applications to remove this vulnerability.

A vulnerability exists in .NET Kestrel where a malicious client can bypass QUIC stream limit in HTTP/3 in both ASP.NET  and .NET runtimes resulting in denial of service.

[CVE-2023-35390 - .NET Remote Code Execution Vulnerability](https://msrc.microsoft.com/update-guide/vulnerability/CVE-2023-35390)

Microsoft is releasing this security advisory to provide information about a vulnerability in .NET 7.0 and .NET 6.0. This advisory also provides guidance on what developers can do to update their applications to remove this vulnerability.

A vulnerability exists when some dotnet commands are used in directories with weaker permissions which can result in remote code execution.

[CVE-2023-38180 - .NET Denial of Service Vulnerability](https://msrc.microsoft.com/update-guide/vulnerability/CVE-2023-38180)

Microsoft is releasing this security advisory to provide information about a vulnerability in ASP.NET Core 2.1, .NET 6.0, and .NET 7.0. This advisory also provides guidance on what developers can do to update their applications to remove this vulnerability.

A vulnerability exists in Kestrel where, on detecting a potentially malicious client, Kestrel will sometimes fail to disconnect it, resulting in denial of service.

[CVE-2023-35391 - .NET Information Disclosure Vulnerability](https://msrc.microsoft.com/update-guide/vulnerability/CVE-2023-35391)

Microsoft is releasing this security advisory to provide information about a vulnerability in ASP.NET core 2.1, .NET 6.0 and, .NET 7.0. This advisory also provides guidance on what developers can do to update their applications to remove this vulnerability.

A vulnerability exists in ASP.NET Core 2.1, .NET 6.0 and, .NET 7.0 applications using SignalR when redis backplane use might result in information disclosure.


## Visual Studio

See release notes for Visual Studio compatibility for [.NET 7.0](https://github.com/dotnet/core/blob/main/release-notes/7.0/7.0.10/7.0.10.md#visual-studio-compatibility) and [.NET 6.0](https://github.com/dotnet/core/blob/main/release-notes/6.0/6.0.21/6.0.21.md#visual-studio-compatibility).

[efcore-6-0-21]: https://github.com/dotnet/efcore/issues?q=milestone%3A6.0.21+is%3Aclosed+label%3Aservicing-approved+
[efcore-7-0-10]: https://github.com/dotnet/efcore/issues?q=milestone%3A7.0.10+is%3Aclosed+label%3Aservicing-approved+
[sdk-7-0-10]: https://github.com/dotnet/sdk/issues?q=milestone%3A7.0.10+is%3Aclosed+label%3Aservicing-approved+
[runtime-7-0-10]: https://github.com/dotnet/runtime/issues?q=milestone%3A7.0.10+is%3Aclosed+label%3Aservicing-approved+
[runtime-6-0-21]: https://github.com/dotnet/runtime/issues?q=milestone%3A6.0.21+is%3Aclosed+label%3Aservicing-approved+
[aspnet-6-0-21]: https://github.com/dotnet/aspnetcore/issues?q=milestone%3A6.0.21+is%3Aclosed+label%3Aservicing-approved+
[aspnet-7-0-10]: https://github.com/dotnet/aspnetcore/issues?q=milestone%3A7.0.10+is%3Aclosed+label%3Aservicing-approved+
[linker-7-0-10]: https://github.com/dotnet/linker/issues?q=milestone%3A7.0.10+is%3Aclosed+label%3Aservicing-approved+
[winforms-7-0-10]: https://github.com/dotnet/winforms/issues?q=milestone%3A7.0.10+is%3Aclosed+label%3Aservicing-approved+
[winforms-6-0-21]: https://github.com/dotnet/winforms/issues?q=milestone%3A6.0.21+is%3Aclosed+label%3Aservicing-approved+
[wpf-7-0-10]: https://github.com/dotnet/wpf/issues?q=milestone%3A7.0.10+is%3Aclosed+label%3Aservicing-approved+
[wpf-6-0-21]: https://github.com/dotnet/wpf/issues?q=milestone%3A6.0.21+is%3Aclosed+label%3Aservicing-approved+
[templating-7-0-10]: https://github.com/dotnet/templating/issues?q=milestone%3A7.0.10+is%3Aclosed+label%3Aservicing-approved+
[KB-Number]: https://support.microsoft.com/kb/5028608

