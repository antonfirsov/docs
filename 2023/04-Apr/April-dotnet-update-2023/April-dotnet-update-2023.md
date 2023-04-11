---
post_title: .NET April 2023 Updates – .NET 7.0.5, .NET 6.0.16
author1: rbhanda@microsoft.com
post_slug: april-2023-updates
username: rbhanda@microsoft.com
microsoft_alias: rbhanda
featured_image: dotnet-bot_handybot.png
categories: .NET, Maintenance & Updates, .NET Core
summary: Check out April 2023 updates for .NET 7.0. and .NET 6.0
desired_publication_date: 2023-04-11
post_date: 2023-04-11 10:00:00
---

Today, we are releasing the [.NET April 2023 Updates](https://github.com/dotnet/announcements/issues/xxxx). These updates contain non-security improvements. [Your app may be vulnerable](https://github.com/dotnet/core/blob/main/release-notes/6.0/cve.md) if you have not deployed a recent .NET update.

You can download [7.0.5](https://dotnet.microsoft.com/download/dotnet/7.0) and [6.0.16](https://dotnet.microsoft.com/download/dotnet/6.0) versions for Windows, macOS, and Linux, for x86, x64, Arm32, and Arm64.


* Installers and binaries: [7.0.5](https://dotnet.microsoft.com/download/dotnet/7.0) | [6.0.16](https://dotnet.microsoft.com/download/dotnet/6.0) 
* Release notes: [7.0.5](https://github.com/dotnet/core/blob/main/release-notes/7.0/7.0.5/7.0.5.md) | [6.0.16](https://github.com/dotnet/core/blob/main/release-notes/6.0/6.0.16/6.0.16.md) | 
* [Container images](https://mcr.microsoft.com/catalog?search=dotnet/)
* Linux packages: [7.0.5](https://github.com/dotnet/core/blob/main/release-notes/7.0/install-linux.md) | [6.0.16](https://github.com/dotnet/core/blob/main/release-notes/6.0/install-linux.md)
* [Release feedback/issue](https://github.com/dotnet/core/issues/XXXX)
* Known issues: [7.0](https://github.com/dotnet/core/blob/main/release-notes/7.0/known-issues.md) | [6.0](https://github.com/dotnet/core/blob/main/release-notes/6.0/known-issues.md)

### Windows Package Manager CLI (winget)

You can now install .NET updates using the Windows Package Manager CLI (winget):

* To install the .NET 7 runtime: ```winget install dotnet-runtime-7```

* To install the .NET 7 SDK: ```winget install dotnet-sdk-7```

* To update an existing installation: ```winget upgrade```

See [Install with Windows Package Manager (winget)](https://learn.microsoft.com/dotnet/core/install/windows?tabs=net70#install-with-windows-package-manager-winget) for more information.

## Improvements

* ASP.NET Core: [7.0.5][aspnet-7-0-5]
* Entity Framework Core: [7.0.5][efcore-7-0-5]
* Linker: [7.0.5][linker-7-0-5]
* Runtime: [7.0.5][runtime-7-0-5] | [6.0.16][runtime-6-0-16]
* Templating: [7.0.5][templating-7-0-5]
* WPF: [7.0.5][wpf-7-0-5] | [6.0.16][wpf-6-0-16]

## Visual Studio

See release notes for Visual Studio compatibility for [.NET 7.0](https://github.com/dotnet/core/blob/main/release-notes/7.0/7.0.5/7.0.5.md#visual-studio-compatibility) and [.NET 6.0](https://github.com/dotnet/core/blob/main/release-notes/6.0/6.0.16/6.0.16.md#visual-studio-compatibility).


[efcore-7-0-5]: https://github.com/dotnet/efcore/issues?q=milestone%3A7.0.5+is%3Aclosed+label%3Aservicing-approved+
[sdk-7-0-5]: https://github.com/dotnet/sdk/issues?q=milestone%3A7.0.5+is%3Aclosed+label%3Aservicing-approved+
[runtime-7-0-5]: https://github.com/dotnet/runtime/issues?q=milestone%3A7.0.5+is%3Aclosed+label%3Aservicing-approved+
[runtime-6-0-16]: https://github.com/dotnet/runtime/issues?q=milestone%3A6.0.16+is%3Aclosed+label%3Aservicing-approved+
[aspnet-6-0-16]: https://github.com/dotnet/aspnetcore/issues?q=milestone%3A6.0.16+is%3Aclosed+label%3Aservicing-approved+
[aspnet-7-0-5]: https://github.com/dotnet/aspnetcore/issues?q=milestone%3A7.0.5+is%3Aclosed+label%3Aservicing-approved+
[linker-7-0-5]: https://github.com/dotnet/linker/issues?q=milestone%3A7.0.5+is%3Aclosed+label%3Aservicing-approved+
[winforms-7-0-5]: https://github.com/dotnet/winforms/issues?q=milestone%3A7.0.5+is%3Aclosed+label%3Aservicing-approved+
[wpf-7-0-5]: https://github.com/dotnet/wpf/issues?q=milestone%3A7.0.5+is%3Aclosed+label%3Aservicing-approved+
[wpf-6-0-16]: https://github.com/dotnet/wpf/issues?q=milestone%3A6.0.16+is%3Aclosed+label%3Aservicing-approved+
[templating-7-0-5]: https://github.com/dotnet/templating/issues?q=milestone%3A7.0.5+is%3Aclosed+label%3Aservicing-approved+

