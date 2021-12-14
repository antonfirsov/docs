---
post_title: .NET December 2021 Updates – 6.0.1, 5.0.13 and 3.1.22
username: skalaskar@microsoft.com
microsoft_alias: skalaskar
featured_image: dotnet-bot_handybot.png
categories: .NET
summary: Check out December updates for .NET 6.0, .NET 5.0, and .NET Core 3.1.22.
desired_publication_date: 2021-12-14
---

Today, we are releasing the [.NET December 2021 Updates](https://github.com/dotnet/announcements/issues/XXX). These updates contain reliability and security improvements. See the individual release notes for details on updated packages.

You can download [6.0.1](https://dotnet.microsoft.com/download/dotnet/6.0), [5.0.13](https://dotnet.microsoft.com/download/dotnet/5.0) and [3.1.22](https://dotnet.microsoft.com/download/dotnet/3.1) versions for Windows, macOS, and Linux, for x86, x64, Arm32, and Arm64.

* Installers and binaries: [6.0.1](https://dotnet.microsoft.com/download/dotnet/6.0) | [5.0.13](https://dotnet.microsoft.com/download/dotnet/5.0) | [3.1.22](https://dotnet.microsoft.com/download/dotnet/3.1)
* Release notes: [6.0.1](https://github.com/dotnet/core/blob/main/release-notes/6.0/6.0.1/6.0.1.md) | [5.0.13](https://github.com/dotnet/core/blob/main/release-notes/5.0/5.0.13/5.0.13.md) | [3.1.22](https://github.com/dotnet/core/blob/main/release-notes/3.1/3.1.22/3.1.22.md) 
* [Container images](https://hub.docker.com/_/microsoft-dotnet)
* Linux packages: [6.0.1](https://github.com/dotnet/core/blob/main/release-notes/6.0/install-linux.md) | [5.0.13](https://github.com/dotnet/core/blob/main/release-notes/5.0/install-linux.md) | [3.1.22](https://github.com/dotnet/core/blob/main/release-notes/3.1/install-linux.md) 
* [Release feedback/issue](https://github.com/dotnet/core/issues/XXXX)
* Known issues: [6.0](https://github.com/dotnet/core/blob/main/release-notes/6.0/known-issues.md) | [5.0](https://github.com/dotnet/core/blob/main/release-notes/5.0/5.0-known-issues.md) | [3.1](https://github.com/dotnet/core/blob/main/release-notes/3.1/3.1-known-issues.md) 

## Improvements 
* ASP.NET Core: [6.0.1][aspnet-6-0-1] | [5.0.13][aspnet-5-0-13] | [3.1.22][aspnet-3-1-22] 
* EF Core: [6.0.1][efcore-6-0-1]
* Runtime: [6.0.1][runtime-6-0-1] | [5.0.13][runtime-5-0-13] 
* Winforms: [6.0.1][winform-6-0-1] | [5.0.13][winform-5-0-13] 


### Security
 [CVE-2021-43877: ASP.NET Core Information Disclosure Vulnerability](https://msrc.microsoft.com/update-guide/vulnerability/CVE-2021-43877)

Microsoft is releasing this security advisory to provide information about a vulnerability in .NET and .NET Core. This advisory also provides guidance on what developers can do to update their applications to remove this vulnerability. 

An elevation of privilege vulnerability exists in ASP.NET Core Module (ANCM) that could allow elevation of privilege when .NET Core, .NET 5 and .NET 6 applications are hosted within IIS.

## Deployment Update

Customers that have opted to receive .NET Core updates via the Microsoft Update channel will be offered updates to the Hosting Bundle starting with the December 2021 update. Updates for other .NET Core bundles (.NET Core Runtime, ASP.NET Core Runtime, Windows Desktop Runtime, and SDK) have been offered via Microsoft Update to customers that opt in since December 2020. See this [blog post](https://devblogs.microsoft.com/dotnet/net-core-updates-coming-to-microsoft-update) for more information.


## Visual Studio

See release notes for Visual Studio compatibility for [.NET 6.0](https://github.com/dotnet/core/blob/main/release-notes/6.0/6.0.1/6.0.1.md#visual-studio-compatibility), [.NET 5.0](https://github.com/dotnet/core/blob/main/release-notes/5.0/5.0.13/5.0.13.md#visual-studio-compatibility), and [.NET Core 3.1](https://github.com/dotnet/core/blob/main/release-notes/3.1/3.1.22/3.1.22.md#visual-studio-compatibility).

[aspnet-3-1-22]: https://github.com/dotnet/aspnetcore/issues?q=milestone%3A3.1.22++is%3Aclosed+label%3Aservicing-approved+
[aspnet-5-0-13]: https://github.com/dotnet/aspnetcore/issues?q=milestone%3A5.0.13++is%3Aclosed+label%3Aservicing-approved+
[aspnet-6-0-1]: https://github.com/dotnet/aspnetcore/issues?q=milestone%3A6.0.1++is%3Aclosed+label%3Aservicing-approved+
[winform-5-0-13]: https://github.com/dotnet/winforms/issues?q=milestone%3A5.0.13+is%3Aclosed+label%3Aservicing-approved+
[winform-6-0-1]: https://github.com/dotnet/winforms/issues?q=milestone%3A6.0.1+is%3Aclosed+label%3Aservicing-approved+
[runtime-5-0-13]: https://github.com/dotnet/runtime/issues?q=milestone%3A5.0.13+is%3Aclosed+label%3Aservicing-approved+
[runtime-6-0-1]: https://github.com/dotnet/runtime/issues?q=milestone%3A6.0.1+is%3Aclosed+label%3Aservicing-approved+
[efcore-6-0-1]: https://github.com/dotnet/efcore/issues?q=milestone%3A6.0.1+is%3Aclosed+label%3Aservicing-approved+
