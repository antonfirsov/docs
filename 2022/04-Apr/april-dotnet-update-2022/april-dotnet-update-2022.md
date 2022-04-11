---
post_title: .NET April 2022 Updates – .NET 6.0.4, .NET 5.0.16 and, .NET 3.1.24
username: dwhittaker@microsoft.com
microsoft_alias: dwhittaker
featured_image: dotnet-bot_handybot.png
categories: .NET
summary: Check out April updates for .NET 6.0, .NET 5.0, and .NET Core 3.1
desired_publication_date: 2022-04-12
---

Today, we are releasing the [.NET April 2022 Updates](https://github.com/dotnet/announcements/issues/xxxx). These updates contain reliability and non-security improvements. See the individual release notes for details on updated packages.

You can download [6.0.4](https://dotnet.microsoft.com/download/dotnet/6.0),  [5.0.16](https://dotnet.microsoft.com/download/dotnet/5.0) and, [3.1.24](https://dotnet.microsoft.com/download/dotnet/3.1) versions for Windows, macOS, and Linux, for x86, x64, Arm32, and Arm64.

* Installers and binaries: [6.0.4](https://dotnet.microsoft.com/download/dotnet/6.0) | [5.0.16](https://dotnet.microsoft.com/download/dotnet/5.0) | [3.1.24](https://dotnet.microsoft.com/download/dotnet/3.1)
* Release notes: [6.0.4](https://github.com/dotnet/core/blob/main/release-notes/6.0/6.0.4/6.0.4.md) | [5.0.16](https://github.com/dotnet/core/blob/main/release-notes/5.0/5.0.16/5.0.16.md) | [3.1.24](https://github.com/dotnet/core/blob/main/release-notes/3.1/3.1.24/3.1.24.md) 
* [Container images](https://hub.docker.com/_/microsoft-dotnet)
* Linux packages: [6.0.4](https://github.com/dotnet/core/blob/main/release-notes/6.0/install-linux.md) | [5.0.16](https://github.com/dotnet/core/blob/main/release-notes/5.0/install-linux.md) | [3.1.24](https://github.com/dotnet/core/blob/main/release-notes/3.1/install-linux.md) 
* [Release feedback/issue](https://github.com/dotnet/core/issues/XXXX)
* Known issues: [6.0](https://github.com/dotnet/core/blob/main/release-notes/6.0/known-issues.md) | [5.0](https://github.com/dotnet/core/blob/main/release-notes/5.0/5.0-known-issues.md) |  [3.1](https://github.com/dotnet/core/blob/main/release-notes/3.1/3.1-known-issues.md)

Additionally, starting this month, we will be making updates for .NET Core available for server operating systems via Microsoft Update (MU) on an opt-in basis. If you do not want to have your server operating systems updated automatically for you **no action** is required. If on the other hand you do want to leverage this for your servers please review our blog post on [automatic updates for server operating systems](https://devblogs.microsoft.com/dotnet/server-os-can-opt-in-to-au/).

## Improvements

* ASP.NET Core: [6.0.4][aspnet-6-0-4]
* Installer: [6.0.4][installer-6-0-4]
* MSBuild: [6.0.4][msbuild-6-0-4]
* Runtime: [6.0.4][runtime-6-0-4]
* Templating: [6.0.4][templating-6-0-4]
* Winforms: [6.0.4][winforms-6-0-4]
* Winforms: [5.0.16][winforms-5-0-16]
* CoreCLR: [3.1.24][coreclr-3-1-24]
* Corefx: [3.1.24][corefx-3-1-24]
* Winforms: [3.1.24][winforms-3-1-24]

## Visual Studio

See release notes for Visual Studio compatibility for [.NET 6.0](https://github.com/dotnet/core/blob/main/release-notes/6.0/6.0.4/6.0.4.md#visual-studio-compatibility), [.NET 5.0](https://github.com/dotnet/core/blob/main/release-notes/5.0/5.0.16/5.0.16.md#visual-studio-compatibility) and, [.NET Core 3.1](https://github.com/dotnet/core/blob/main/release-notes/3.1/3.1.24/3.1.24.md#visual-studio-compatibility).

## OS Lifecycle Update

We will officially support Ubuntu 22.04 starting with May 2022 servicing updates. 

We are also aware of an Open SSL error on Arm32 architecture and are actively working to address. This issue is being tracked [here](https://github.com/dotnet/runtime/issues/66310).

[aspnet-6-0-4]: https://github.com/dotnet/aspnetcore/issues?q=milestone%3A6.0.4++is%3Aclosed+label%3Aservicing-approved+
[winforms-6-0-4]: https://github.com/dotnet/winforms/issues?q=milestone%3A6.0.4+is%3Aclosed+label%3Aservicing-approved+
[runtime-6-0-4]: https://github.com/dotnet/runtime/issues?q=milestone%3A6.0.4+is%3Aclosed+label%3Aservicing-approved+
[installer-6-0-4]: https://github.com/dotnet/installer/issues?q=milestone%3A6.0.4+is%3Aclosed+label%3Aservicing-approved+
[msbuild-6-0-4]: https://github.com/dotnet/msbuild/issues?q=milestone%3A6.0.4+is%3Aclosed+label%3Aservicing-approved+
[templating-6-0-4]: https://github.com/dotnet/templating/issues?q=milestone%3A6.0.4+is%3Aclosed+label%3Aservicing-approved+
[winforms-5-0-16]: https://github.com/dotnet/winforms/issues?q=milestone%3A5.0.16+is%3Aclosed+label%3Aservicing-approved+
[wpf-5-0-16]: https://github.com/dotnet/wpf/issues?q=milestone%3A5.0.16+is%3Aclosed+label%3Aservicing-approved+
[coreclr-3-1-24]: https://github.com/dotnet/coreclr/issues?q=milestone%3A3.1.24+is%3Aclosed+label%3Aservicing-approved+
[corefx-3-1-24]: https://github.com/dotnet/corefx/issues?q=milestone%3A3.1.24+is%3Aclosed+label%3Aservicing-approved+
[winforms-3-1-24]: https://github.com/dotnet/winforms/issues?q=milestone%3A3.1.24+is%3Aclosed+label%3Aservicing-approved+
[wpf-3-1-24]: https://github.com/dotnet/wpf/issues?q=milestone%3A3.1.24+is%3Aclosed+label%3Aservicing-approved+
