---
post_title: .NET and .NET Framework July 2024 servicing releases updates
author1: taraoverfield
author2: rbhanda@microsoft.com
post_slug: dotnet-and-dotnet-framework-july-2024-servicing-updates
microsoft_alias: tarao
featured_image: dotnet-updates-july-2024.jpg
categories: .NET, .NET Framework, Maintenance & Updates
tags: .NET, .NET Framework
summary: A recap of the latest servicing updates for .NET and .NET Framework for July 2024.
post_date: 2024-07-09 10:05:00
---

[alert type="important" heading="Use release notes for downloading July updates"]
Please use the release note links in this post to download the July updates. The [download page](https://dotnet.microsoft.com/download/dotnet) on the .NET website has not yet been updated for July. We are actively working to update the .NET website.
[/alert]

Welcome to our new combined .NET servicing updates for July 2024. To help streamline and help you keep up to date with the latest service releases we have decided to combine our update posts around both .NET & .NET Framework so you can find all the information in one convenient location on the blog. Don't forget that you can find updates about .NET previews on [GitHub](https://github.com/dotnet/core/discussions/categories/news), specifically for .NET 9. Let's get into the latest release of .NET & .NET Framework, here is a quick overview of what's new in these releases:

 - [Security Improvements](#security-improvements)
 - [.NET updates](#net-july-2024-updates)
 - [.NET Framework updates](#net-framework-july-2024-updates)

## Security improvements

This month you will find two CVEs that have been fixed this month:

| CVE # | Title | Applies to |
| ----- | ---- | ---------- |
| [CVE-2024-30105](https://msrc.microsoft.com/update-guide/vulnerability/CVE-2024-30105) | .NET Denial of Service Vulnerability | .NET 8.0 |
| [CVE-2024-35264](https://msrc.microsoft.com/update-guide/vulnerability/CVE-2024-35264) | .NET Remote Code Execution Vulnerability| .NET 8.0 | 
| [CVE-2024-38081](https://msrc.microsoft.com/update-guide/vulnerability/CVE-2024-38081) | .NET Remote Code Execution Vulnerability| .NET 6.0, .NET Framework 2.0, 3.0, 3.5, 4.6.2, 4.7, 4.7.1, 4.8, 4.8.1  | 
| [CVE-2024-38095](https://msrc.microsoft.com/update-guide/vulnerability/CVE-2024-38095) | .NET Remote Code Execution Vulnerability| .NET 8.0, .NET 6.0 | 


> Note: There are no new security improvements for .NET Framework this release.


## .NET July 2024 Updates

Below you will find a details list of everything from the .NET release for July 2024 including .NET 6.0.32 and .NET 8.0.7:

|  | .NET 6.0 | .NET 7.0 | .NET 8.0 |
|----------|----------|----------|----------|
| Release Notes    | [6.0.32](https://github.com/dotnet/core/blob/main/release-notes/6.0/6.0.32/6.0.32.md)  | [8.0.7](https://github.com/dotnet/core/blob/main/release-notes/8.0/8.0.7/8.0.7.md)   |
| Installers and binaries    | [6.0.32](https://dotnet.microsoft.com/download/dotnet/6.0)   | [8.0.7](https://dotnet.microsoft.com/download/dotnet/8.0)  |
| Container Images    | [images](https://mcr.microsoft.com/catalog?search=dotnet/)   | [images](https://mcr.microsoft.com/catalog?search=dotnet/)   | [images](https://mcr.microsoft.com/catalog?search=dotnet/)  
| Linux packages    | [6.0.32](https://github.com/dotnet/core/blob/main/release-notes/6.0/install-linux.md) | [8.0.7](https://github.com/dotnet/core/blob/main/release-notes/8.0/install-linux.md)   | 
| Known Issues    | [6.0](https://github.com/dotnet/core/blob/main/release-notes/6.0/known-issues.md)   | [8.0](https://github.com/dotnet/core/blob/main/release-notes/8.0/known-issues.md)   | 

### .NET Improvements
* .NET MAUI: [8.0.70][maui-8-0-7
* ASP.NET Core: [8.0.7][aspnet-8-0-7] 
* Entity Framework Core: [8.0.7][efcore-8-0-7] 
* Runtime: [8.0.7][runtime-8-0-7] | [6.0.32][runtime-6-0-32]
* SDK: [8.0.7][sdk-8-0-7]

Share feedback about this release in the [Release feedback issue](https://github.com/dotnet/core/issues/xxxx). 

## .NET Framework July 2024 Updates

This month, there are security and non-security updates in these releases, be sure to browse our [release notes for .NET Framework](https://learn.microsoft.com/dotnet/framework/release-notes/2024/07-09-july-security-and-quality-rollup) for more details. 


## See you next month

Let us know what you think of these new combined service release blogs as we continue to iterate to bring you the latest news and updates for .NET.

[efcore-6-0-32]: https://github.com/dotnet/efcore/issues?q=milestone%3A6.0.32+is%3Aclosed+label%3Aservicing-approved+
[efcore-8-0-7]: https://github.com/dotnet/efcore/issues?q=milestone%3A8.0.7+is%3Aclosed+label%3Aservicing-approved+
[sdk-6-0-32]: https://github.com/dotnet/sdk/issues?q=milestone%3A7.0.16+is%3Aclosed+label%3Aservicing-approved+
[sdk-8-0-7]: https://github.com/dotnet/sdk/issues?q=milestone%3A8.0.7+is%3Aclosed+label%3Aservicing-approved+
[runtime-6-0-32]: https://github.com/dotnet/runtime/issues?q=milestone%3A6.0.32+is%3Aclosed+label%3Aservicing-approved+
[runtime-8-0-7]: https://github.com/dotnet/runtime/issues?q=milestone%3A8.0.7+is%3Aclosed+label%3Aservicing-approved+
[roslyn-8-0-7]: https://github.com/dotnet/roslyn-analyzers/issues?q=milestone%3A8.0.7xx+is%3Aclosed+label%3Aservicing-approved+
[aspnet-6-0-32]: https://github.com/dotnet/aspnetcore/issues?q=milestone%3A6.0.32+is%3Aclosed+label%3Aservicing-approved+
[maui-8-0-7]: https://github.com/dotnet/maui/releases/tag/8.0.70
[aspnet-8-0-7]: https://github.com/dotnet/aspnetcore/issues?q=milestone%3A8.0.7+is%3Aclosed+label%3Aservicing-approved+
[winforms-8-0-7]: https://github.com/dotnet/winforms/issues?q=milestone%3A8.0.7+is%3Aclosed+label%3Aservicing-approved+
[winforms-6-0-32]: https://github.com/dotnet/winforms/issues?q=milestone%3A6.0.32+is%3Aclosed+label%3Aservicing-approved+
[wpf-8-0-7]: https://github.com/dotnet/wpf/issues?q=milestone%3A8.0.7+is%3Aclosed+label%3Aservicing-approved+
[wpf-6-0-32]: https://github.com/dotnet/wpf/issues?q=milestone%3A6.0.32+is%3Aclosed+label%3Aservicing-approved+
[templating-7-0-15]: https://github.com/dotnet/templating/issues?q=milestone%3A7.0.16+is%3Aclosed+label%3Aservicing-approved+
