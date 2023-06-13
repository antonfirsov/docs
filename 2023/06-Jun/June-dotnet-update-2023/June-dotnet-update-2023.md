---
post_title: .NET June 2023 Updates – .NET 7.0.7, .NET 6.0.18
author1: rbhanda@microsoft.com
post_slug: june-2023-updates
username: rbhanda@microsoft.com
microsoft_alias: rbhanda
featured_image: dotnet-bot_handybot.png
categories: .NET, Maintenance & Updates, .NET Core
summary: Check out June 2023 updates for .NET 7.0 and .NET 6.0
post_date: 2023-06-13 12:00:00
---

Today, we are releasing the [.NET June 2023 Updates](https://github.com/dotnet/announcements/issues/xxxx). These updates contain security and non-security improvements. [Your app may be vulnerable](https://github.com/dotnet/core/blob/main/release-notes/6.0/cve.md) if you have not deployed a recent .NET update.

You can download [7.0.7](https://dotnet.microsoft.com/download/dotnet/7.0) and [6.0.18](https://dotnet.microsoft.com/download/dotnet/6.0) versions for Windows, macOS, and Linux, for x86, x64, Arm32, and Arm64.


* Installers and binaries: [7.0.7](https://dotnet.microsoft.com/download/dotnet/7.0) | [6.0.18](https://dotnet.microsoft.com/download/dotnet/6.0) 
* Release notes: [7.0.7](https://github.com/dotnet/core/blob/main/release-notes/7.0/7.0.7/7.0.7.md) | [6.0.18](https://github.com/dotnet/core/blob/main/release-notes/6.0/6.0.18/6.0.18.md) | 
* [Container images](https://mcr.microsoft.com/catalog?search=dotnet/)
* Linux packages: [7.0.7](https://github.com/dotnet/core/blob/main/release-notes/7.0/install-linux.md) | [6.0.18](https://github.com/dotnet/core/blob/main/release-notes/6.0/install-linux.md)
* [Release feedback/issue](https://github.com/dotnet/core/issues/XXXX)
* Known issues: [7.0](https://github.com/dotnet/core/blob/main/release-notes/7.0/known-issues.md) | [6.0](https://github.com/dotnet/core/blob/main/release-notes/6.0/known-issues.md)

### Windows Package Manager CLI (winget)

You can now install .NET updates using the Windows Package Manager CLI (winget):

* To install the .NET 7 runtime: ```winget install dotnet-runtime-7```

* To install the .NET 7 SDK: ```winget install dotnet-sdk-7```

* To update an existing installation: ```winget upgrade```

See [Install with Windows Package Manager (winget)](https://learn.microsoft.com/dotnet/core/install/windows?tabs=net70#install-with-windows-package-manager-winget) for more information.

## Improvements

* ASP.NET Core: [7.0.7][aspnet-7-0-7] | [6.0.18][aspnet-6-0-18]
* Entity Framework Core: [7.0.7][efcore-7-0-7]
* Runtime: [7.0.7][runtime-7-0-7] | [6.0.18][runtime-6-0-18]
* Winforms: [7.0.7][winforms-7-0-7] | [6.0.18][winforms-6-0-18]
* WPF: [7.0.7][wpf-7-0-7] | [6.0.18][wpf-6-0-18]

## Security

[CVE-2023-24895 - .NET Remote Code Execution Vulnerability](https://msrc.microsoft.com/update-guide/vulnerability/CVE-2023-24895)

Microsoft is releasing this security advisory to provide information about a vulnerability in .NET 7.0 and .NET 6.0. This advisory also provides guidance on what developers can do to update their applications to remove this vulnerability.

A vulnerability exists in how WPF for .NET handles certain XAML Frame elements which may result in remote code execution.

[CVE-2023-24897 - .NET Remote Code Execution Vulnerability](https://msrc.microsoft.com/update-guide/vulnerability/CVE-2023-24897)

Microsoft is releasing this security advisory to provide information about a vulnerability in .NET 7.0 and .NET 6.0. This advisory also provides guidance on what developers can do to update their applications to remove this vulnerability.

A vulnerability exists in how .NET reads debugging symbols, where reading a malicious symbols file may result in remote code execution.

[CVE-2023-24936 - .NET Elevation of Privilege Vulnerability](https://msrc.microsoft.com/update-guide/vulnerability/CVE-2023-24936)

Microsoft is releasing this security advisory to provide information about a vulnerability in .NET 7.0 and .NET 6.0. This advisory also provides guidance on what developers can do to update their applications to remove this vulnerability.

A vulnerability exists in .NET when deserializing a DataSet or DataTable from XML which may result in elevation of privileges.

[CVE-2023-29331 -  .NET Denial of Service Vulnerability](https://msrc.microsoft.com/update-guide/vulnerability/CVE-2023-29331)

Microsoft is releasing this security advisory to provide information about a vulnerability in .NET 7.0 and .NET 6.0. This advisory also provides guidance on what developers can do to update their applications to remove this vulnerability.

A vulnerability exists in .NET when processing X.509 certificates that may result in Denial of Service.

[CVE-2023-29337 -  NuGet Client Remote Code Execution Vulnerability](https://msrc.microsoft.com/update-guide/vulnerability/CVE-2023-29337)

Microsoft is releasing this security advisory to provide information about a vulnerability in .NET and NuGet on Linux. This advisory also provides guidance on what developers can do to update their applications to remove this vulnerability.

A vulnerability exists in nuget where a potential race condition that can lead to a symlink attack

[CVE-2023-32032 -  .NET Denial of Service Vulnerability](https://msrc.microsoft.com/update-guide/vulnerability/CVE-2023-32032)

Microsoft is releasing this security advisory to provide information about a vulnerability in .NET 7.0 and .NET 6.0. This advisory also provides guidance on what developers can do to update their applications to remove this vulnerability.

A vulnerability exists in .NET using extracting the contents of a Tar file which may result in elevation of privileges.

[CVE-2023-33126 -  .NET Denial of Service Vulnerability](https://msrc.microsoft.com/update-guide/vulnerability/CVE-2023-33126)

Microsoft is releasing this security advisory to provide information about a vulnerability in .NET 7.0 and .NET 6.0. This advisory also provides guidance on what developers can do to update their applications to remove this vulnerability.

A vulnerability exists in .NET during crash and stack trace scenarios that could lead to loading arbitrary binaries.

[CVE-2023-33128 -  .NET Denial of Service Vulnerability](https://msrc.microsoft.com/update-guide/vulnerability/CVE-2023-33128)

Microsoft is releasing this security advisory to provide information about a vulnerability in .NET 7.0 and .NET 6.0. This advisory also provides guidance on what developers can do to update their applications to remove this vulnerability.

A vulnerability exists in .NET source generator for P/Invokes that can lead to generated code freeing uninitialized memory and crashing.

[CVE-2023-33135 -  .NET Denial of Service Vulnerability](https://msrc.microsoft.com/update-guide/vulnerability/CVE-2023-33135)

Microsoft is releasing this security advisory to provide information about a vulnerability in .NET 7.0 and .NET 6.0. This advisory also provides guidance on what developers can do to update their applications to remove this vulnerability.

A vulnerability exists in the .NET SDK during tool restore which can lead to an elevation of privilege.

## Visual Studio

See release notes for Visual Studio compatibility for [.NET 7.0](https://github.com/dotnet/core/blob/main/release-notes/7.0/7.0.7/7.0.7.md#visual-studio-compatibility) and [.NET 6.0](https://github.com/dotnet/core/blob/main/release-notes/6.0/6.0.18/6.0.18.md#visual-studio-compatibility).


[efcore-7-0-7]: https://github.com/dotnet/efcore/issues?q=milestone%3A7.0.7+is%3Aclosed+label%3Aservicing-approved+
[sdk-7-0-7]: https://github.com/dotnet/sdk/issues?q=milestone%3A7.0.7+is%3Aclosed+label%3Aservicing-approved+
[runtime-7-0-7]: https://github.com/dotnet/runtime/issues?q=milestone%3A7.0.7+is%3Aclosed+label%3Aservicing-approved+
[runtime-6-0-18]: https://github.com/dotnet/runtime/issues?q=milestone%3A6.0.18+is%3Aclosed+label%3Aservicing-approved+
[aspnet-6-0-18]: https://github.com/dotnet/aspnetcore/issues?q=milestone%3A6.0.18+is%3Aclosed+label%3Aservicing-approved+
[aspnet-7-0-7]: https://github.com/dotnet/aspnetcore/issues?q=milestone%3A7.0.7+is%3Aclosed+label%3Aservicing-approved+
[linker-7-0-7]: https://github.com/dotnet/linker/issues?q=milestone%3A7.0.7+is%3Aclosed+label%3Aservicing-approved+
[winforms-7-0-7]: https://github.com/dotnet/winforms/issues?q=milestone%3A7.0.7+is%3Aclosed+label%3Aservicing-approved+
[winforms-6-0-18]: https://github.com/dotnet/winforms/issues?q=milestone%3A6.0.18+is%3Aclosed+label%3Aservicing-approved+
[wpf-7-0-7]: https://github.com/dotnet/wpf/issues?q=milestone%3A7.0.7+is%3Aclosed+label%3Aservicing-approved+
[wpf-6-0-18]: https://github.com/dotnet/wpf/issues?q=milestone%3A6.0.18+is%3Aclosed+label%3Aservicing-approved+
[templating-7-0-7]: https://github.com/dotnet/templating/issues?q=milestone%3A7.0.7+is%3Aclosed+label%3Aservicing-approved+

