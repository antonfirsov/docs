---
post_title: .NET March 2022 Updates – .NET 6.0.3, .NET 5.0.15 and, .NET 3.1.23
username: rbhanda@microsoft.com
microsoft_alias: rbhanda
featured_image: dotnet-bot_handybot.png
categories: .NET
summary: Check out March updates for .NET 6.0 and .NET 5.0
desired_publication_date: 2022-03-08
---

Today, we are releasing the [.NET March 2022 Updates](https://github.com/dotnet/announcements/issues/210). These updates contain reliability and security improvements. See the individual release notes for details on updated packages.

You can download [6.0.3](https://dotnet.microsoft.com/download/dotnet/6.0),  [5.0.15](https://dotnet.microsoft.com/download/dotnet/5.0) and, [3.1.23](https://dotnet.microsoft.com/download/dotnet/3.1) versions for Windows, macOS, and Linux, for x86, x64, Arm32, and Arm64.

* Installers and binaries: [6.0.3](https://dotnet.microsoft.com/download/dotnet/6.0) | [5.0.15](https://dotnet.microsoft.com/download/dotnet/5.0) | [3.1.23](https://dotnet.microsoft.com/download/dotnet/3.1)
* Release notes: [6.0.3](https://github.com/dotnet/core/blob/main/release-notes/6.0/6.0.3/6.0.3.md) | [5.0.15](https://github.com/dotnet/core/blob/main/release-notes/5.0/5.0.15/5.0.15.md) | [5.0.15](https://github.com/dotnet/core/blob/main/release-notes/3.1/3.0.23/3.1.23.md) 
* [Container images](https://hub.docker.com/_/microsoft-dotnet)
* Linux packages: [6.0.3](https://github.com/dotnet/core/blob/main/release-notes/6.0/install-linux.md) | [5.0.15](https://github.com/dotnet/core/blob/main/release-notes/5.0/install-linux.md) | [3.1.23](https://github.com/dotnet/core/blob/main/release-notes/3.1/install-linux.md) 
* [Release feedback/issue](https://github.com/dotnet/core/issues/XXXX)
* Known issues: [6.0](https://github.com/dotnet/core/blob/main/release-notes/6.0/known-issues.md) | [5.0](https://github.com/dotnet/core/blob/main/release-notes/5.0/5.0-known-issues.md) |  [3.1](https://github.com/dotnet/core/blob/main/release-notes/3.1/3.1-known-issues.md)

## Improvements 
* ASP.NET Core: [6.0.3][aspnet-6-0-3] 
* EF Core: [6.0.3][efcore-6-0-3]
* Runtime: [6.0.3][runtime-6-0-3]
* Winforms: [6.0.3][winform-6-0-3] 
* WPF: [6.0.3][wpf-6-0-3] 
* WPF: [5.0.15][wpf-5-0-15] 

### Security
 [CVE-2020-8927: .NET Remote Code Execution Vulnerability](https://msrc.microsoft.com/update-guide/vulnerability/CVE-2020-8927)

Microsoft is releasing this security advisory to provide information about a vulnerability in .NET 5.0 and .NET Core 3.1. This advisory also provides guidance on what developers can do to update their applications to remove this vulnerability.

A vulnerability exists in .NET 5.0 and .NET Core 3.1 where a buffer overflow exists in the Brotli library versions prior to 1.0.8.

 [CVE-2022-24464: .NET Denial of Service Vulnerability](https://msrc.microsoft.com/update-guide/vulnerability/CVE-2022-24464)

Microsoft is releasing this security advisory to provide information about a vulnerability in .NET 6.0, .NET 5.0, and .NET CORE 3.1. This advisory also provides guidance on what developers can do to update their applications to remove this vulnerability.

Microsoft is aware of a Denial of Service vulnerability, which exists in .NET 6.0, .NET 5.0, and .NET CORE 3.1 when parsing certain types of http form requests.

 [CVE-2022-24512: .NET Remote Code Execution Vulnerability](https://msrc.microsoft.com/update-guide/vulnerability/CVE-2022-24512)

Microsoft is releasing this security advisory to provide information about a vulnerability in .NET 6.0, .NET 5.0, and .NET Core 3.1. This advisory also provides guidance on what developers can do to update their applications to remove this vulnerability.

A Remote Code Execution vulnerability exists in .NET 6.0, .NET 5.0, and .NET Core 3.1 where a stack buffer overrun occurs in .NET Double Parse routine.


## Visual Studio

See release notes for Visual Studio compatibility for [.NET 6.0](https://github.com/dotnet/core/blob/main/release-notes/6.0/6.0.3/6.0.3.md#visual-studio-compatibility), [.NET 5.0](https://github.com/dotnet/core/blob/main/release-notes/5.0/5.0.15/5.0.15.md#visual-studio-compatibility) and, [.NET Core 3.1](https://github.com/dotnet/core/blob/main/release-notes/3.1/3.1.23/3.1.23.md#visual-studio-compatibility).



[aspnet-6-0-3]: https://github.com/dotnet/aspnetcore/issues?q=milestone%3A6.0.3++is%3Aclosed+label%3Aservicing-approved+
[winform-6-0-3]: https://github.com/dotnet/winforms/issues?q=milestone%3A6.0.3+is%3Aclosed+label%3Aservicing-approved+
[runtime-6-0-3]: https://github.com/dotnet/runtime/issues?q=milestone%3A6.0.3+is%3Aclosed+label%3Aservicing-approved+
[efcore-6-0-3]: https://github.com/dotnet/efcore/issues?q=milestone%3A6.0.3+is%3Aclosed+label%3Aservicing-approved+
[wpf-6-0-3]: https://github.com/dotnet/wpf/issues?q=milestone%3A6.0.3+is%3Aclosed+label%3Aservicing-approved+

[wpf-5-0-15]: https://github.com/dotnet/wpf/issues?q=milestone%3A5.0.15+is%3Aclosed+label%3Aservicing-approved+

