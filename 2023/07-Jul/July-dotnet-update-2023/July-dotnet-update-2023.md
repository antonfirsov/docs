---
post_title: .NET July 2023 Updates – .NET 7.0.9, .NET 6.0.20
author1: rbhanda@microsoft.com
post_slug: July-2023-updates
username: rbhanda@microsoft.com
microsoft_alias: rbhanda
featured_image: dotnet-bot_handybot.png
categories: .NET, Maintenance & Updates, .NET Core
summary: Check out latest July 2023 updates for .NET 7.0 and .NET 6.0
post_date: 2023-07-11 10:00:00
---


Today, we are releasing the [.NET July 2023 Updates](https://github.com/dotnet/announcements/issues/262). These updates contain security and non-security improvements. [Your app may be vulnerable](https://github.com/dotnet/core/blob/main/release-notes/6.0/cve.md) if you have not deployed a recent .NET update.

You can download [7.0.9](https://dotnet.microsoft.com/download/dotnet/7.0) and [6.0.20](https://dotnet.microsoft.com/download/dotnet/6.0) versions for Windows, macOS, and Linux, for x86, x64, Arm32, and Arm64.


* Installers and binaries: [7.0.9](https://dotnet.microsoft.com/download/dotnet/7.0) | [6.0.20](https://dotnet.microsoft.com/download/dotnet/6.0) 
* Release notes: [7.0.9](https://github.com/dotnet/core/blob/main/release-notes/7.0/7.0.9/7.0.9.md) | [6.0.20](https://github.com/dotnet/core/blob/main/release-notes/6.0/6.0.20/6.0.20.md) | 
* [Container images](https://mcr.microsoft.com/catalog?search=dotnet/)
* Linux packages: [7.0.9](https://github.com/dotnet/core/blob/main/release-notes/7.0/install-linux.md) | [6.0.20](https://github.com/dotnet/core/blob/main/release-notes/6.0/install-linux.md)
* [Release feedback/issue](https://github.com/dotnet/core/issues/8611)
* Known issues: [7.0](https://github.com/dotnet/core/blob/main/release-notes/7.0/known-issues.md) | [6.0](https://github.com/dotnet/core/blob/main/release-notes/6.0/known-issues.md)

### Windows Package Manager CLI (winget)

You can now install .NET updates using the Windows Package Manager CLI (winget):

* To install the .NET 7 runtime: ```winget install dotnet-runtime-7```

* To install the .NET 7 SDK: ```winget install dotnet-sdk-7```

* To update an existing installation: ```winget upgrade```

See [Install with Windows Package Manager (winget)](https://learn.microsoft.com/dotnet/core/install/windows?tabs=net70#install-with-windows-package-manager-winget) for more information.

## Improvements

* ASP.NET Core: [7.0.9][aspnet-7-0-9] | [6.0.20][aspnet-6-0-20]
* Entity Framework Core: [7.0.9][efcore-7-0-9]
* Runtime: [7.0.9][runtime-7-0-9] | [6.0.20][runtime-6-0-20]
* Winforms: [7.0.9][winforms-7-0-9] | [6.0.20][winforms-6-0-20]
* WPF: [7.0.9][wpf-7-0-9] | [6.0.20][wpf-6-0-20]

## Security

[CVE-2023-33127 - .NET Remote Code Execution Vulnerability](https://msrc.microsoft.com/update-guide/vulnerability/CVE-2023-33127)

Microsoft is releasing this security advisory to provide information about a vulnerability in .NET 7.0 and .NET 6.0. This advisory also provides guidance on what developers can do to update their applications to remove this vulnerability.

A vulnerability exists in .NET applications where the diagnostic server can be exploited to achieve cross-session/cross-user elevation of privilege (EoP) and code execution.

[CVE-2023-33170 - .NET Security Feature Bypass Vulnerability](https://msrc.microsoft.com/update-guide/vulnerability/CVE-2023-33170)

Microsoft is releasing this security advisory to provide information about a vulnerability in ASP.NET  Core 2.1 and above. This advisory also provides guidance on what developers can do to update their applications to remove this vulnerability.

A vulnerability exists in ASP.NET Core applications where account lockout maximum failed attempts may not be immediately updated, allowing an attacker to try more passwords.


## Visual Studio

See release notes for Visual Studio compatibility for [.NET 7.0](https://github.com/dotnet/core/blob/main/release-notes/7.0/7.0.9/7.0.9.md#visual-studio-compatibility) and [.NET 6.0](https://github.com/dotnet/core/blob/main/release-notes/6.0/6.0.20/6.0.20.md#visual-studio-compatibility).


[efcore-7-0-9]: https://github.com/dotnet/efcore/issues?q=milestone%3A7.0.9+is%3Aclosed+label%3Aservicing-approved+
[sdk-7-0-9]: https://github.com/dotnet/sdk/issues?q=milestone%3A7.0.9+is%3Aclosed+label%3Aservicing-approved+
[runtime-7-0-9]: https://github.com/dotnet/runtime/issues?q=milestone%3A7.0.9+is%3Aclosed+label%3Aservicing-approved+
[runtime-6-0-20]: https://github.com/dotnet/runtime/issues?q=milestone%3A6.0.20+is%3Aclosed+label%3Aservicing-approved+
[aspnet-6-0-20]: https://github.com/dotnet/aspnetcore/issues?q=milestone%3A6.0.20+is%3Aclosed+label%3Aservicing-approved+
[aspnet-7-0-9]: https://github.com/dotnet/aspnetcore/issues?q=milestone%3A7.0.9+is%3Aclosed+label%3Aservicing-approved+
[linker-7-0-9]: https://github.com/dotnet/linker/issues?q=milestone%3A7.0.9+is%3Aclosed+label%3Aservicing-approved+
[winforms-7-0-9]: https://github.com/dotnet/winforms/issues?q=milestone%3A7.0.9+is%3Aclosed+label%3Aservicing-approved+
[winforms-6-0-20]: https://github.com/dotnet/winforms/issues?q=milestone%3A6.0.20+is%3Aclosed+label%3Aservicing-approved+
[wpf-7-0-9]: https://github.com/dotnet/wpf/issues?q=milestone%3A7.0.9+is%3Aclosed+label%3Aservicing-approved+
[wpf-6-0-20]: https://github.com/dotnet/wpf/issues?q=milestone%3A6.0.20+is%3Aclosed+label%3Aservicing-approved+
[templating-7-0-9]: https://github.com/dotnet/templating/issues?q=milestone%3A7.0.9+is%3Aclosed+label%3Aservicing-approved+
[KB-Number]: https://support.microsoft.com/kb/5028608

