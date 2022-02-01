---
post_title: .NET February 2022 Updates – 6.0.2 and 5.0.14
username: rbhanda@microsoft.com
microsoft_alias: rbhanda
featured_image: dotnet-bot_handybot.png
categories: .NET
summary: Check out February updates for .NET 6.0 and .NET 5.0
desired_publication_date: 2021-12-14
---

Today, we are releasing the [.NET February 2022 Updates](https://github.com/dotnet/announcements/issues/XXX). These updates contain reliability and security improvements. See the individual release notes for details on updated packages.

You can download [6.0.2](https://dotnet.microsoft.com/download/dotnet/6.0) and [5.0.14](https://dotnet.microsoft.com/download/dotnet/5.0) versions for Windows, macOS, and Linux, for x86, x64, Arm32, and Arm64.

* Installers and binaries: [6.0.2](https://dotnet.microsoft.com/download/dotnet/6.0) | [5.0.14](https://dotnet.microsoft.com/download/dotnet/5.0) 
* Release notes: [6.0.2](https://github.com/dotnet/core/blob/main/release-notes/6.0/6.0.2/6.0.2.md) | [5.0.14](https://github.com/dotnet/core/blob/main/release-notes/5.0/5.0.14/5.0.14.md) 
* [Container images](https://hub.docker.com/_/microsoft-dotnet)
* Linux packages: [6.0.2](https://github.com/dotnet/core/blob/main/release-notes/6.0/install-linux.md) | [5.0.14](https://github.com/dotnet/core/blob/main/release-notes/5.0/install-linux.md) 
* [Release feedback/issue](https://github.com/dotnet/core/issues/XXXX)
* Known issues: [6.0](https://github.com/dotnet/core/blob/main/release-notes/6.0/known-issues.md) | [5.0](https://github.com/dotnet/core/blob/main/release-notes/5.0/5.0-known-issues.md)

## Improvements 
* ASP.NET Core: [6.0.2][aspnet-6-0-2] 
* EF Core: [6.0.2][efcore-6-0-2]
* Runtime: [6.0.2][runtime-6-0-2]
* Winforms: [6.0.2][winform-6-0-2] 


### Security
 [CVE-2022-21986: ASP.NET Core Denial of Service Vulnerability](https://msrc.microsoft.com/update-guide/vulnerability/CVE-2022-21986)
 
Microsoft is releasing this security advisory to provide information about a vulnerability in .NET 6.0 and .NET 5.0. This advisory also provides guidance on what developers can do to update their applications to remove this vulnerability.

A Denial of Service vulnerability exists in .NET 5.0 and .NET 6.0 where Kestrel overpooling of HTTP/2 and HTTP/3 request headers may lead to denial of service.

## Deployment Update

Customers that have opted to receive .NET Core updates via the Microsoft Update channel will be offered updates to the Hosting Bundle starting with the December 2021 update. Updates for other .NET Core bundles (.NET Core Runtime, ASP.NET Core Runtime, Windows Desktop Runtime, and SDK) have been offered via Microsoft Update to customers that opt in since December 2020. See this [blog post](https://devblogs.microsoft.com/dotnet/net-core-updates-coming-to-microsoft-update) for more information.


## Visual Studio

See release notes for Visual Studio compatibility for [.NET 6.0](https://github.com/dotnet/core/blob/main/release-notes/6.0/6.0.2/6.0.2.md#visual-studio-compatibility) and [.NET 5.0](https://github.com/dotnet/core/blob/main/release-notes/5.0/5.0.14/5.0.14.md#visual-studio-compatibility).



[aspnet-6-0-2]: https://github.com/dotnet/aspnetcore/issues?q=milestone%3A6.0.2++is%3Aclosed+label%3Aservicing-approved+
[winform-6-0-2]: https://github.com/dotnet/winforms/issues?q=milestone%3A6.0.2+is%3Aclosed+label%3Aservicing-approved+
[runtime-6-0-2]: https://github.com/dotnet/runtime/issues?q=milestone%3A6.0.2+is%3Aclosed+label%3Aservicing-approved+
[efcore-6-0-2]: https://github.com/dotnet/efcore/issues?q=milestone%3A6.0.2+is%3Aclosed+label%3Aservicing-approved+
