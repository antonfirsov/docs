---
post_title: .NET June 2023 Updates – .NET 7.0.8, .NET 6.0.19
author1: rbhanda@microsoft.com
post_slug: june-2023-updates
username: rbhanda@microsoft.com
microsoft_alias: rbhanda
featured_image: dotnet-bot_handybot.png
categories: .NET, Maintenance & Updates, .NET Core
summary: Check out latest June 2023 updates for .NET 7.0 and .NET 6.0
post_date: 2023-06-22 12:00:00
---

## Update -- June 22, 2023

We have released a second update this month to address a regression in our earlier release (June 13th). The new versions are .NET 6.0.19 and .NET 7.0.8. The regression is functional and doesn't require action on your part unless you are affected by the issue.

### Regression 
The NET 6.0.18 and 7.0.7 updates update added constraints to PFX certificate loading to fix a DoS vulnerability (CVE-2023-29331). We created a specific exception message with a link to a known issue KB https://support.microsoft.com/kb/5025823 to describe these behavioral changes.

We learned from customer reports during the week of June 13, 2023 that .NET 6.0.18 and 7.0.7 may fail to import PKCS12 blobs whose private keys are protected by a null password. Callers may non-deterministically observe a `CryptographicException` being thrown by the `X509Certificate` constructor on those runtimes. This regression was unintentional and a fix is being offered for affected applications.

Also documented at [.NET June OOB Updates][KB-Number].

### Download Update


You can download [7.0.8](https://dotnet.microsoft.com/download/dotnet/7.0) and [6.0.19](https://dotnet.microsoft.com/download/dotnet/6.0) versions for Windows, macOS, and Linux, for x86, x64, Arm32, and Arm64.

* Installers and binaries: [7.0.8](https://dotnet.microsoft.com/download/dotnet/7.0) | [6.0.19](https://dotnet.microsoft.com/download/dotnet/6.0) 
* Release notes: [7.0.8](https://github.com/dotnet/core/blob/main/release-notes/7.0/7.0.8/7.0.8.md) | [6.0.19](https://github.com/dotnet/core/blob/main/release-notes/6.0/6.0.19/6.0.19.md) | 
* [Container images](https://mcr.microsoft.com/catalog?search=dotnet/)
* Linux packages: [7.0.8](https://github.com/dotnet/core/blob/main/release-notes/7.0/install-linux.md) | [6.0.19](https://github.com/dotnet/core/blob/main/release-notes/6.0/install-linux.md)
* [Release feedback/issue](https://github.com/dotnet/core/issues/XXXX)

### Do I need to install 6.0.19 / 7.0.8?

There is no need to install these updates unless you are affected by the functional regression listed at [KB5028608](https://support.microsoft.com/kb/5028608). If you are not affected by the functional regression described above, you can safely remain on 6.0.18 / 7.0.7. 

### Is 6.0.19 / 7.0.8 a security update?

No. These updates contain no new security fixes beyond what already shipped in 6.0.18 / 7.0.7. As long as you are running at least 6.0.18 or 7.0.7, you are protected with all of the latest available security fixes.

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
[KB-Number]: https://support.microsoft.com/kb/5028608

