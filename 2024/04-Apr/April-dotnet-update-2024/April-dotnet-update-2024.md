---
post_title: .NET April 2024 Updates – .NET 8.0.4, 7.0.18, .NET 6.0.29
author1: rbhanda@microsoft.com
post_slug: 1pril-2024-updates
username: rbhanda@microsoft.com
microsoft_alias: rbhanda
featured_image: April-2024-updates.jpg
categories: .NET, Maintenance & Updates, .NET Core
summary: Check out latest April 2024 updates for .NET 8.0, .NET 7.0, and .NET 6.0.
post_date: 2024-04-09 11:00:00
---

Today, we are releasing the [.NET April 2024 Updates](https://github.com/dotnet/announcements/issues/302). These updates contain security and non-security improvements. [Your app may be vulnerable](https://github.com/dotnet/core/blob/main/release-notes/6.0/cve.md) if you have not deployed a recent .NET update.

You can download [8.0.4](https://dotnet.microsoft.com/download/dotnet/8.0), [7.0.18](https://dotnet.microsoft.com/download/dotnet/7.0) and, [6.0.29](https://dotnet.microsoft.com/download/dotnet/6.0) versions for Windows, macOS, and Linux, for x86, x64, Arm32, and Arm64.


* Installers and binaries: [8.0.4](https://dotnet.microsoft.com/download/dotnet/8.0) |[7.0.18](https://dotnet.microsoft.com/download/dotnet/7.0) | [6.0.29](https://dotnet.microsoft.com/download/dotnet/6.0) 
* Release notes: [8.0.4](https://github.com/dotnet/core/blob/main/release-notes/8.0/8.0.4/8.0.4.md) | [7.0.18](https://github.com/dotnet/core/blob/main/release-notes/7.0/7.0.18/7.0.18.md) | [6.0.29](https://github.com/dotnet/core/blob/main/release-notes/6.0/6.0.29/6.0.29.md)
* [Container images](https://mcr.microsoft.com/catalog?search=dotnet/)
* Linux packages: [8.0.4](https://github.com/dotnet/core/blob/main/release-notes/8.0/install-linux.md) | [7.0.18](https://github.com/dotnet/core/blob/main/release-notes/7.0/install-linux.md) | [6.0.29](https://github.com/dotnet/core/blob/main/release-notes/6.0/install-linux.md)
* [Release feedback/issue](https://github.com/dotnet/core/issues/9263)
* Known issues: [8.0](https://github.com/dotnet/core/blob/main/release-notes/8.0/known-issues.md) | [7.0](https://github.com/dotnet/core/blob/main/release-notes/7.0/known-issues.md) | [6.0](https://github.com/dotnet/core/blob/main/release-notes/6.0/known-issues.md)



### Windows Package Manager CLI (winget)

You can now install .NET updates using the Windows Package Manager CLI (winget):

* To install the .NET 8 runtime: ```winget install dotnet-runtime-8```

* To install the .NET 8 SDK: ```winget install dotnet-sdk-8```

* To update an existing installation: ```winget upgrade```

See [Install with Windows Package Manager (winget)](https://learn.microsoft.com/dotnet/core/install/windows?tabs=net80#install-with-windows-package-manager-winget) for more information.


## Improvements
* ASP.NET Core: [8.0.4][aspnet-8-0-4] | [6.0.29][aspnet-6-0-29]
* Entity Framework Core: [8.0.4][efcore-8-0-4] 
* Runtime: [8.0.4][runtime-8-0-4] | [7.0.18][runtime-7-0-18] | [6.0.29][runtime-6-0-29]
* SDK: [8.0.4][sdk-8-0-4]

## Security

[CVE-2024-21409 | .NET Elevation of Privilege Vulnerability](https://msrc.microsoft.com/update-guide/vulnerability/CVE-2024-21409)

Microsoft is releasing this security advisory to provide information about a vulnerability in .NET 6.0, .NET 7.0 ,and .NET 8.0. This advisory also provides guidance on what developers can do to update their applications to remove this vulnerability.

A use-after-free vulnerability exists in WPF which may result in Elevation of Privilege when viewing untrusted documents.

## Visual Studio

See release notes for Visual Studio compatibility for [.NET 8.0](https://github.com/dotnet/core/blob/main/release-notes/8.0/8.0.4/8.0.4.md#visual-studio-compatibility), [.NET 7.0](https://github.com/dotnet/core/blob/main/release-notes/7.0/7.0.18/7.0.18.md#visual-studio-compatibility) and, [.NET 6.0](https://github.com/dotnet/core/blob/main/release-notes/6.0/6.0.29/6.0.29.md#visual-studio-compatibility).

[efcore-6-0-29]: https://github.com/dotnet/efcore/issues?q=milestone%3A6.0.29+is%3Aclosed+label%3Aservicing-approved+
[efcore-7-0-18]: https://github.com/dotnet/efcore/issues?q=milestone%3A7.0.18+is%3Aclosed+label%3Aservicing-approved+
[efcore-8-0-4]: https://github.com/dotnet/efcore/issues?q=milestone%3A8.0.4+is%3Aclosed+label%3Aservicing-approved+
[sdk-7-0-18]: https://github.com/dotnet/sdk/issues?q=milestone%3A7.0.18+is%3Aclosed+label%3Aservicing-approved+
[sdk-6-0-29]: https://github.com/dotnet/sdk/issues?q=milestone%3A7.0.18+is%3Aclosed+label%3Aservicing-approved+
[sdk-8-0-4]: https://github.com/dotnet/sdk/issues?q=milestone%3A8.0.4+is%3Aclosed+label%3Aservicing-approved+
[runtime-7-0-18]: https://github.com/dotnet/runtime/issues?q=milestone%3A7.0.18+is%3Aclosed+label%3Aservicing-approved+
[runtime-6-0-29]: https://github.com/dotnet/runtime/issues?q=milestone%3A6.0.29+is%3Aclosed+label%3Aservicing-approved+
[runtime-8-0-4]: https://github.com/dotnet/runtime/issues?q=milestone%3A8.0.4+is%3Aclosed+label%3Aservicing-approved+
[roslyn-8-0-4]: https://github.com/dotnet/roslyn-analyzers/issues?q=milestone%3A8.0.4xx+is%3Aclosed+label%3Aservicing-approved+
[aspnet-6-0-29]: https://github.com/dotnet/aspnetcore/issues?q=milestone%3A6.0.29+is%3Aclosed+label%3Aservicing-approved+
[aspnet-7-0-18]: https://github.com/dotnet/aspnetcore/issues?q=milestone%3A7.0.18+is%3Aclosed+label%3Aservicing-approved+
[aspnet-8-0-4]: https://github.com/dotnet/aspnetcore/issues?q=milestone%3A8.0.4+is%3Aclosed+label%3Aservicing-approved+
[linker-7-0-18]: https://github.com/dotnet/linker/issues?q=milestone%3A7.0.18+is%3Aclosed+label%3Aservicing-approved+
[winforms-8-0-4]: https://github.com/dotnet/winforms/issues?q=milestone%3A8.0.4+is%3Aclosed+label%3Aservicing-approved+
[winforms-7-0-18]: https://github.com/dotnet/winforms/issues?q=milestone%3A7.0.18+is%3Aclosed+label%3Aservicing-approved+
[winforms-6-0-29]: https://github.com/dotnet/winforms/issues?q=milestone%3A6.0.29+is%3Aclosed+label%3Aservicing-approved+
[wpf-8-0-4]: https://github.com/dotnet/wpf/issues?q=milestone%3A8.0.4+is%3Aclosed+label%3Aservicing-approved+
[wpf-7-0-18]: https://github.com/dotnet/wpf/issues?q=milestone%3A7.0.18+is%3Aclosed+label%3Aservicing-approved+
[wpf-6-0-29]: https://github.com/dotnet/wpf/issues?q=milestone%3A6.0.29+is%3Aclosed+label%3Aservicing-approved+
[templating-7-0-18]: https://github.com/dotnet/templating/issues?q=milestone%3A7.0.18+is%3Aclosed+label%3Aservicing-approved+
