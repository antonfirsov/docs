---
post_title: .NET November 2023 Updates – .NET 7.0.14, .NET 6.0.25
author1: rbhanda@microsoft.com
post_slug: November-2023-updates
username: rbhanda@microsoft.com
microsoft_alias: rbhanda
featured_image: dotnet-bot_handybot.png
categories: .NET, Maintenance & Updates, .NET Core
summary: Check out latest November 2023 updates for .NET 7.0 and .NET 6.0
post_date: 2023-11-14 10:00:00
---

Today, we are releasing the [.NET November 2023 Updates](https://github.com/dotnet/announcements/issues/285). These updates contain security and non-security improvements. [Your app may be vulnerable](https://github.com/dotnet/core/blob/main/release-notes/6.0/cve.md) if you have not deployed a recent .NET update.

You can download [7.0.14](https://dotnet.microsoft.com/download/dotnet/7.0) and [6.0.25](https://dotnet.microsoft.com/download/dotnet/6.0) versions for Windows, macOS, and Linux, for x86, x64, Arm32, and Arm64.


* Installers and binaries: [7.0.14](https://dotnet.microsoft.com/download/dotnet/7.0) | [6.0.25](https://dotnet.microsoft.com/download/dotnet/6.0) 
* Release notes: [7.0.14](https://github.com/dotnet/core/blob/main/release-notes/7.0/7.0.14/7.0.14.md) | [6.0.25](https://github.com/dotnet/core/blob/main/release-notes/6.0/6.0.25/6.0.25.md)
* [Container images](https://mcr.microsoft.com/catalog?search=dotnet/)
* Linux packages: [7.0.14](https://github.com/dotnet/core/blob/main/release-notes/7.0/install-linux.md) | [6.0.25](https://github.com/dotnet/core/blob/main/release-notes/6.0/install-linux.md)
* [Release feedback/issue](https://github.com/dotnet/core/issues/8910)
* Known issues: [7.0](https://github.com/dotnet/core/blob/main/release-notes/7.0/known-issues.md) | [6.0](https://github.com/dotnet/core/blob/main/release-notes/6.0/known-issues.md)

### Windows Package Manager CLI (winget)

You can now install .NET updates using the Windows Package Manager CLI (winget):

* To install the .NET 7 runtime: ```winget install dotnet-runtime-7```

* To install the .NET 7 SDK: ```winget install dotnet-sdk-7```

* To update an existing installation: ```winget upgrade```

See [Install with Windows Package Manager (winget)](https://learn.microsoft.com/dotnet/core/install/windows?tabs=net70#install-with-windows-package-manager-winget) for more information.


## Security

[CVE-2023-36038 - .NET Denial of Service Vulnerability](https://msrc.microsoft.com/update-guide/vulnerability/CVE-2023-36038)

Microsoft is releasing this security advisory to provide information about a vulnerability in ASP.NET Core 8.0 RC2. This advisory also provides guidance on what developers can do to update their applications to address this vulnerability.

A vulnerability exists in ASP.NET IIS where a remote  unauthenticated user can issue specially crafted requests to a .NET application which may result in denial of service. This vulnerability only impacts Windows OS.

[CVE-2023-36049 - .NET Elevation of Privilege Vulnerability](https://msrc.microsoft.com/update-guide/vulnerability/CVE-2023-36049)

Microsoft is releasing this security advisory to provide information about a vulnerability in .NET 6.0, .NET 7.0 and .NET 8.0 RC2. This advisory also provides guidance on what developers can do to update their applications to address this vulnerability.

An elevation of privilege vulnerability exists in .NET where untrusted URIs provided to System.Net.WebRequest.Create can be used to inject arbitrary commands to backend FTP servers.

[CVE-2023-36558 - .NET Security Feature Bypass Vulnerability](https://msrc.microsoft.com/update-guide/vulnerability/CVE-2023-36558)

Microsoft is releasing this security advisory to provide information about a vulnerability in ASP.NET Core 6.0, ASP.NET Core 7.0 and, ASP.NET Core 8.0 RC2. This advisory also provides guidance on what developers can do to update their applications to address this vulnerability.

A security feature bypass vulnerability exists in ASP.NET where an unauthenticated user is able to bypass validation on Blazor server forms which could trigger unintended actions.

## Visual Studio

See release notes for Visual Studio compatibility for [.NET 7.0](https://github.com/dotnet/core/blob/main/release-notes/7.0/7.0.14/7.0.14.md#visual-studio-compatibility) and [.NET 6.0](https://github.com/dotnet/core/blob/main/release-notes/6.0/6.0.25/6.0.25.md#visual-studio-compatibility).

[efcore-6-0-23]: https://github.com/dotnet/efcore/issues?q=milestone%3A6.0.25+is%3Aclosed+label%3Aservicing-approved+
[efcore-7-0-11]: https://github.com/dotnet/efcore/issues?q=milestone%3A7.0.14+is%3Aclosed+label%3Aservicing-approved+
[sdk-7-0-11]: https://github.com/dotnet/sdk/issues?q=milestone%3A7.0.14+is%3Aclosed+label%3Aservicing-approved+
[sdk-6-0-22]: https://github.com/dotnet/sdk/issues?q=milestone%3A7.0.14+is%3Aclosed+label%3Aservicing-approved+
[runtime-7-0-11]: https://github.com/dotnet/runtime/issues?q=milestone%3A7.0.14+is%3Aclosed+label%3Aservicing-approved+
[runtime-6-0-22]: https://github.com/dotnet/runtime/issues?q=milestone%3A6.0.25+is%3Aclosed+label%3Aservicing-approved+
[aspnet-6-0-22]: https://github.com/dotnet/aspnetcore/issues?q=milestone%3A6.0.25+is%3Aclosed+label%3Aservicing-approved+
[aspnet-7-0-11]: https://github.com/dotnet/aspnetcore/issues?q=milestone%3A7.0.14+is%3Aclosed+label%3Aservicing-approved+
[linker-7-0-11]: https://github.com/dotnet/linker/issues?q=milestone%3A7.0.14+is%3Aclosed+label%3Aservicing-approved+
[winforms-7-0-11]: https://github.com/dotnet/winforms/issues?q=milestone%3A7.0.14+is%3Aclosed+label%3Aservicing-approved+
[winforms-6-0-22]: https://github.com/dotnet/winforms/issues?q=milestone%3A6.0.25+is%3Aclosed+label%3Aservicing-approved+
[wpf-7-0-11]: https://github.com/dotnet/wpf/issues?q=milestone%3A7.0.14+is%3Aclosed+label%3Aservicing-approved+
[wpf-6-0-22]: https://github.com/dotnet/wpf/issues?q=milestone%3A6.0.25+is%3Aclosed+label%3Aservicing-approved+
[templating-7-0-11]: https://github.com/dotnet/templating/issues?q=milestone%3A7.0.14+is%3Aclosed+label%3Aservicing-approved+
[efcore-6-0-23]: https://github.com/dotnet/efcore/issues?q=milestone%3A6.0.25+is%3Aclosed+label%3Aservicing-approved+
[efcore-7-0-11]: https://github.com/dotnet/efcore/issues?q=milestone%3A7.0.14+is%3Aclosed+label%3Aservicing-approved+
[sdk-7-0-11]: https://github.com/dotnet/sdk/issues?q=milestone%3A7.0.14+is%3Aclosed+label%3Aservicing-approved+
[sdk-6-0-22]: https://github.com/dotnet/sdk/issues?q=milestone%3A7.0.14+is%3Aclosed+label%3Aservicing-approved+
[runtime-7-0-11]: https://github.com/dotnet/runtime/issues?q=milestone%3A7.0.14+is%3Aclosed+label%3Aservicing-approved+
[runtime-6-0-22]: https://github.com/dotnet/runtime/issues?q=milestone%3A6.0.25+is%3Aclosed+label%3Aservicing-approved+
[aspnet-6-0-22]: https://github.com/dotnet/aspnetcore/issues?q=milestone%3A6.0.25+is%3Aclosed+label%3Aservicing-approved+
[aspnet-7-0-11]: https://github.com/dotnet/aspnetcore/issues?q=milestone%3A7.0.14+is%3Aclosed+label%3Aservicing-approved+
[linker-7-0-11]: https://github.com/dotnet/linker/issues?q=milestone%3A7.0.14+is%3Aclosed+label%3Aservicing-approved+
[winforms-7-0-11]: https://github.com/dotnet/winforms/issues?q=milestone%3A7.0.14+is%3Aclosed+label%3Aservicing-approved+
[winforms-6-0-22]: https://github.com/dotnet/winforms/issues?q=milestone%3A6.0.25+is%3Aclosed+label%3Aservicing-approved+
[wpf-7-0-11]: https://github.com/dotnet/wpf/issues?q=milestone%3A7.0.14+is%3Aclosed+label%3Aservicing-approved+
[wpf-6-0-22]: https://github.com/dotnet/wpf/issues?q=milestone%3A6.0.25+is%3Aclosed+label%3Aservicing-approved+
[templating-7-0-11]: https://github.com/dotnet/templating/issues?q=milestone%3A7.0.14+is%3Aclosed+label%3Aservicing-approved+

