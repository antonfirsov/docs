---
post_title: .NET September 2023 Updates – .NET 7.0.11, .NET 6.0.22
author1: rbhanda@microsoft.com
post_slug: September-2023-updates
username: rbhanda@microsoft.com
microsoft_alias: rbhanda
featured_image: dotnet-bot_handybot.png
categories: .NET, Maintenance & Updates, .NET Core
summary: Check out latest September 2023 updates for .NET 7.0 and .NET 6.0
post_date: 2023-09-12 10:00:00
---


Today, we are releasing the [.NET September 2023 Updates](https://github.com/dotnet/announcements/issues/270). These updates contain security and non-security improvements. [Your app may be vulnerable](https://github.com/dotnet/core/blob/main/release-notes/6.0/cve.md) if you have not deployed a recent .NET update.

You can download [7.0.11](https://dotnet.microsoft.com/download/dotnet/7.0) and [6.0.22](https://dotnet.microsoft.com/download/dotnet/6.0) versions for Windows, macOS, and Linux, for x86, x64, Arm32, and Arm64.


* Installers and binaries: [7.0.11](https://dotnet.microsoft.com/download/dotnet/7.0) | [6.0.22](https://dotnet.microsoft.com/download/dotnet/6.0) 
* Release notes: [7.0.11](https://github.com/dotnet/core/blob/main/release-notes/7.0/7.0.11/7.0.11.md) | [6.0.22](https://github.com/dotnet/core/blob/main/release-notes/6.0/6.0.22/6.0.22.md)
* [Container images](https://mcr.microsoft.com/catalog?search=dotnet/)
* Linux packages: [7.0.11](https://github.com/dotnet/core/blob/main/release-notes/7.0/install-linux.md) | [6.0.22](https://github.com/dotnet/core/blob/main/release-notes/6.0/install-linux.md)
* [Release feedback/issue](https://github.com/dotnet/core/issues/8758)
* Known issues: [7.0](https://github.com/dotnet/core/blob/main/release-notes/7.0/known-issues.md) | [6.0](https://github.com/dotnet/core/blob/main/release-notes/6.0/known-issues.md)

### Windows Package Manager CLI (winget)

You can now install .NET updates using the Windows Package Manager CLI (winget):

* To install the .NET 7 runtime: ```winget install dotnet-runtime-7```

* To install the .NET 7 SDK: ```winget install dotnet-sdk-7```

* To update an existing installation: ```winget upgrade```

See [Install with Windows Package Manager (winget)](https://learn.microsoft.com/dotnet/core/install/windows?tabs=net70#install-with-windows-package-manager-winget) for more information.

## Improvements

* Runtime: [7.0.11][runtime-7-0-11] | [6.0.22][runtime-6-0-22]
* SDK: [7.0.11][sdk-7-0-11] | [6.0.22][sdk-6-0-22]


## Security

**Note:** The vulnerabilities [CVE-2023-36792]( https://www.cve.org/CVERecord?id=CVE-2023-36792), [CVE-2023-36793]( https://www.cve.org/CVERecord?id=CVE-2023-36793), [CVE-2023-36792]( https://www.cve.org/CVERecord?id=CVE-2023-36794), [CVE-2023-36796]( https://www.cve.org/CVERecord?id=CVE-2023-36796) are all resolved by a single patch. Get this update to resolve all of them.

[CVE-2023-36792 - .NET Remote Code Execution Vulnerability](https://msrc.microsoft.com/update-guide/vulnerability/CVE-2023-36792)

Microsoft is releasing this security advisory to provide information about a vulnerability in .NET 7.0 and .NET 6.0. This advisory also provides guidance on what developers can do to update their applications to remove this vulnerability.

A vulnerability exists in Microsoft.DiaSymReader.Native.amd64.dll when reading a corrupted PDB file which may lead to remote code execution. This issue only affects Windows systems.

[CVE-2023-36793 - .NET Remote Code Execution Vulnerability](https://msrc.microsoft.com/update-guide/vulnerability/CVE-2023-36793)

Microsoft is releasing this security advisory to provide information about a vulnerability in .NET 7.0 and .NET 6.0. This advisory also provides guidance on what developers can do to update their applications to remove this vulnerability.

A vulnerability exists in Microsoft.DiaSymReader.Native.amd64.dll when reading a corrupted PDB file which may lead to remote code execution. This issue only affects Windows systems.

[CVE-2023-36794 - .NET Remote Code Execution Vulnerability](https://msrc.microsoft.com/update-guide/vulnerability/CVE-2023-36794)

Microsoft is releasing this security advisory to provide information about a vulnerability in .NET 7.0 and .NET 6.0. This advisory also provides guidance on what developers can do to update their applications to remove this vulnerability.

A vulnerability exists in Microsoft.DiaSymReader.Native.amd64.dll when reading a corrupted PDB file which may lead to remote code execution. This issue only affects Windows systems.

[CVE-2023-36796 - .NET Remote Code Execution Vulnerability](https://msrc.microsoft.com/update-guide/vulnerability/CVE-2023-36796)

Microsoft is releasing this security advisory to provide information about a vulnerability in .NET 7.0 and .NET 6.0. This advisory also provides guidance on what developers can do to update their applications to remove this vulnerability.

A vulnerability exists in Microsoft.DiaSymReader.Native.amd64.dll when reading a corrupted PDB file which may lead to remote code execution. This issue only affects Windows systems.

[CVE-2023-36799 - .NET Denial of Service Vulnerability](https://msrc.microsoft.com/update-guide/vulnerability/CVE-2023-36799)

Microsoft is releasing this security advisory to provide information about a vulnerability in .NET 7.0 and .NET 6.0. This advisory also provides guidance on what developers can do to update their applications to remove this vulnerability.

A vulnerability exists in .NET where reading a maliciously crafted X.509 certificate may result in Denial of Service. This issue only affects Linux systems.

## Visual Studio

See release notes for Visual Studio compatibility for [.NET 7.0](https://github.com/dotnet/core/blob/main/release-notes/7.0/7.0.11/7.0.11.md#visual-studio-compatibility) and [.NET 6.0](https://github.com/dotnet/core/blob/main/release-notes/6.0/6.0.22/6.0.22.md#visual-studio-compatibility).

[efcore-6-0-22]: https://github.com/dotnet/efcore/issues?q=milestone%3A6.0.22+is%3Aclosed+label%3Aservicing-approved+
[efcore-7-0-11]: https://github.com/dotnet/efcore/issues?q=milestone%3A7.0.11+is%3Aclosed+label%3Aservicing-approved+
[sdk-7-0-11]: https://github.com/dotnet/sdk/issues?q=milestone%3A7.0.11+is%3Aclosed+label%3Aservicing-approved+
[sdk-6-0-22]: https://github.com/dotnet/sdk/issues?q=milestone%3A7.0.11+is%3Aclosed+label%3Aservicing-approved+
[runtime-7-0-11]: https://github.com/dotnet/runtime/issues?q=milestone%3A7.0.11+is%3Aclosed+label%3Aservicing-approved+
[runtime-6-0-22]: https://github.com/dotnet/runtime/issues?q=milestone%3A6.0.22+is%3Aclosed+label%3Aservicing-approved+
[aspnet-6-0-22]: https://github.com/dotnet/aspnetcore/issues?q=milestone%3A6.0.22+is%3Aclosed+label%3Aservicing-approved+
[aspnet-7-0-11]: https://github.com/dotnet/aspnetcore/issues?q=milestone%3A7.0.11+is%3Aclosed+label%3Aservicing-approved+
[linker-7-0-11]: https://github.com/dotnet/linker/issues?q=milestone%3A7.0.11+is%3Aclosed+label%3Aservicing-approved+
[winforms-7-0-11]: https://github.com/dotnet/winforms/issues?q=milestone%3A7.0.11+is%3Aclosed+label%3Aservicing-approved+
[winforms-6-0-22]: https://github.com/dotnet/winforms/issues?q=milestone%3A6.0.22+is%3Aclosed+label%3Aservicing-approved+
[wpf-7-0-11]: https://github.com/dotnet/wpf/issues?q=milestone%3A7.0.11+is%3Aclosed+label%3Aservicing-approved+
[wpf-6-0-22]: https://github.com/dotnet/wpf/issues?q=milestone%3A6.0.22+is%3Aclosed+label%3Aservicing-approved+
[templating-7-0-11]: https://github.com/dotnet/templating/issues?q=milestone%3A7.0.11+is%3Aclosed+label%3Aservicing-approved+


