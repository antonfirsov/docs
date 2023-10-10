---
post_title: .NET October 2023 Updates – .NET 7.0.12, .NET 6.0.23
author1: rbhanda@microsoft.com
post_slug: October-2023-updates
username: rbhanda@microsoft.com
microsoft_alias: rbhanda
featured_image: dotnet-bot_handybot.png
categories: .NET, Maintenance & Updates, .NET Core
summary: Check out latest October 2023 updates for .NET 7.0 and .NET 6.0
post_date: 2023-10-10 11:00:00
---


Today, we are releasing the [.NET October 2023 Updates](https://github.com/dotnet/announcements/issues/276). These updates contain security and non-security improvements. [Your app may be vulnerable](https://github.com/dotnet/core/blob/main/release-notes/6.0/cve.md) if you have not deployed a recent .NET update.

You can download [7.0.12](https://dotnet.microsoft.com/download/dotnet/7.0) and [6.0.23](https://dotnet.microsoft.com/download/dotnet/6.0) versions for Windows, macOS, and Linux, for x86, x64, Arm32, and Arm64.


* Installers and binaries: [7.0.12](https://dotnet.microsoft.com/download/dotnet/7.0) | [6.0.23](https://dotnet.microsoft.com/download/dotnet/6.0) 
* Release notes: [7.0.12](https://github.com/dotnet/core/blob/main/release-notes/7.0/7.0.12/7.0.12.md) | [6.0.23](https://github.com/dotnet/core/blob/main/release-notes/6.0/6.0.23/6.0.23.md)
* [Container images](https://mcr.microsoft.com/catalog?search=dotnet/)
* Linux packages: [7.0.12](https://github.com/dotnet/core/blob/main/release-notes/7.0/install-linux.md) | [6.0.23](https://github.com/dotnet/core/blob/main/release-notes/6.0/install-linux.md)
* [Release feedback/issue](https://github.com/dotnet/core/issues/8827)
* Known issues: [7.0](https://github.com/dotnet/core/blob/main/release-notes/7.0/known-issues.md) | [6.0](https://github.com/dotnet/core/blob/main/release-notes/6.0/known-issues.md)

### Windows Package Manager CLI (winget)

You can now install .NET updates using the Windows Package Manager CLI (winget):

* To install the .NET 7 runtime: ```winget install dotnet-runtime-7```

* To install the .NET 7 SDK: ```winget install dotnet-sdk-7```

* To update an existing installation: ```winget upgrade```

See [Install with Windows Package Manager (winget)](https://learn.microsoft.com/dotnet/core/install/windows?tabs=net70#install-with-windows-package-manager-winget) for more information.


## Security

[CVE-2023-44487 - .NET Denial of Service Vulnerability](https://msrc.microsoft.com/update-guide/vulnerability/CVE-2023-44487)

Microsoft is releasing this security advisory to provide information about a vulnerability in .NET 8.0 RC1, .NET 7.0 ,and .NET 6.0. This advisory also provides guidance on what developers can do to update their applications to address this vulnerability. A patch for this vulnerability (nicknamed "Rapid Reset") is being released in coordination with other industry partners.

A vulnerability exists in the ASP.NET  Core Kestrel web server where a malicious client may flood the server with specially crafted HTTP/2 requests, causing denial of service.

[CVE-2023-38171 - .NET Denial of Service Vulnerability](https://msrc.microsoft.com/update-guide/vulnerability/CVE-2023-38171)

Microsoft is releasing this security advisory to provide information about a vulnerability in .NET 7.0 and .NET 8.0 RC1. This advisory also provides guidance on what developers can do to update their applications to address this vulnerability.

A null pointer vulnerability exists in MsQuic.dll which may lead to Denial of Service. This issue only affects Windows systems.

[CVE-2023-36435 - .NET Denial of Service Vulnerability](https://msrc.microsoft.com/update-guide/vulnerability/CVE-2023-36435)

Microsoft is releasing this security advisory to provide information about a vulnerability in .NET 7.0 and .NET 8.0 RC1. This advisory also provides guidance on what developers can do to update their applications to address this vulnerability.

A memory leak vulnerability exists in MsQuic.dll which may lead to Denial of Service. This issue only affects Windows systems.

## Visual Studio

See release notes for Visual Studio compatibility for [.NET 7.0](https://github.com/dotnet/core/blob/main/release-notes/7.0/7.0.12/7.0.12.md#visual-studio-compatibility) and [.NET 6.0](https://github.com/dotnet/core/blob/main/release-notes/6.0/6.0.23/6.0.23.md#visual-studio-compatibility).

[efcore-6-0-22]: https://github.com/dotnet/efcore/issues?q=milestone%3A6.0.23+is%3Aclosed+label%3Aservicing-approved+
[efcore-7-0-11]: https://github.com/dotnet/efcore/issues?q=milestone%3A7.0.12+is%3Aclosed+label%3Aservicing-approved+
[sdk-7-0-11]: https://github.com/dotnet/sdk/issues?q=milestone%3A7.0.12+is%3Aclosed+label%3Aservicing-approved+
[sdk-6-0-22]: https://github.com/dotnet/sdk/issues?q=milestone%3A7.0.12+is%3Aclosed+label%3Aservicing-approved+
[runtime-7-0-11]: https://github.com/dotnet/runtime/issues?q=milestone%3A7.0.12+is%3Aclosed+label%3Aservicing-approved+
[runtime-6-0-22]: https://github.com/dotnet/runtime/issues?q=milestone%3A6.0.23+is%3Aclosed+label%3Aservicing-approved+
[aspnet-6-0-22]: https://github.com/dotnet/aspnetcore/issues?q=milestone%3A6.0.23+is%3Aclosed+label%3Aservicing-approved+
[aspnet-7-0-11]: https://github.com/dotnet/aspnetcore/issues?q=milestone%3A7.0.12+is%3Aclosed+label%3Aservicing-approved+
[linker-7-0-11]: https://github.com/dotnet/linker/issues?q=milestone%3A7.0.12+is%3Aclosed+label%3Aservicing-approved+
[winforms-7-0-11]: https://github.com/dotnet/winforms/issues?q=milestone%3A7.0.12+is%3Aclosed+label%3Aservicing-approved+
[winforms-6-0-22]: https://github.com/dotnet/winforms/issues?q=milestone%3A6.0.23+is%3Aclosed+label%3Aservicing-approved+
[wpf-7-0-11]: https://github.com/dotnet/wpf/issues?q=milestone%3A7.0.12+is%3Aclosed+label%3Aservicing-approved+
[wpf-6-0-22]: https://github.com/dotnet/wpf/issues?q=milestone%3A6.0.23+is%3Aclosed+label%3Aservicing-approved+
[templating-7-0-11]: https://github.com/dotnet/templating/issues?q=milestone%3A7.0.12+is%3Aclosed+label%3Aservicing-approved+


