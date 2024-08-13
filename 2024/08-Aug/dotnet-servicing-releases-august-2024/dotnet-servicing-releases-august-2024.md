---
post_title: .NET and .NET Framework August 2024 updates
author1: taraoverfield
author2: rbhanda@microsoft.com
post_slug: dotnet-and-dotnet-framework-august-2024-updates
microsoft_alias: tarao
featured_image: dotnet-updates-august-2024.jpg
categories: .NET, .NET Framework, Maintenance & Updates
tags: .NET, .NET Framework
summary: A recap of the updates for .NET and .NET Framework for August 2024.
post_date: 2024-08-13 10:05:00
---

Welcome to our new combined .NET servicing updates for August 2024. To help streamline and help you keep up to date with the latest service releases we have decided to combine our update posts around both .NET & .NET Framework so you can find all the information in one convenient location on the blog. Don't forget that you can find updates about .NET previews on [GitHub](https://github.com/dotnet/core/discussions/categories/news), specifically for .NET 9. Let's get into the latest release of .NET & .NET Framework, here is a quick overview of what's new in these releases:

 - [Security Improvements](#security-improvements)
 - [.NET updates](#net-August-2024-updates)
 - [.NET Framework updates](#net-framework-August-2024-updates)

## Security improvements

Two vulnerabilities have been fixed this month:

| CVE # | Title | Applies to |
| ----- | ---- | ---------- |
| [CVE-2024-38168](https://msrc.microsoft.com/update-guide/vulnerability/CVE-2024-38168) | .NET Denial of Service Vulnerability | .NET 8.0 |
| [CVE-2024-38167](https://msrc.microsoft.com/update-guide/vulnerability/CVE-2024-38167) | .NET Information Disclosure Vulnerability | .NET 8.0 | 


> Note: There are no new security updates for .NET Framework this month.


## .NET updates

The following table includes release notes and binaries for the updates.

|                 | .NET 6.0 | .NET 8.0  |
|----------|----------|----------|
| Release Notes    | [6.0.33](https://github.com/dotnet/core/blob/main/release-notes/6.0/6.0.33/6.0.33.md)  | [8.0.8](https://github.com/dotnet/core/blob/main/release-notes/8.0/8.0.8/8.0.8.md)   |
| Installers and binaries    | [6.0.33](https://dotnet.microsoft.com/download/dotnet/6.0)   | [8.0.8](https://dotnet.microsoft.com/download/dotnet/8.0)  |
| Container Images    | [images](https://mcr.microsoft.com/catalog?search=dotnet/)   | [images](https://mcr.microsoft.com/catalog?search=dotnet/) |
| Linux packages    | [6.0.33](https://github.com/dotnet/core/blob/main/release-notes/6.0/install-linux.md) | [8.0.8](https://github.com/dotnet/core/blob/main/release-notes/8.0/install-linux.md)   | 
| Known Issues    | [6.0](https://github.com/dotnet/core/blob/main/release-notes/6.0/known-issues.md)   | [8.0](https://github.com/dotnet/core/blob/main/release-notes/8.0/known-issues.md)   | 

The following table includes GitHub query links for each release.

| Component     | 8.0     | 6.0      |
|---------------|-------|-------|
| ASP.NET Core | [8.0][aspnet-8-0-8] | [6.0][aspnet-6-0-33] |
| Runtime.          | [8.0][runtime-8-0-8] | [6.0][runtime-6-0-33]
| SDK                  | [8.0][sdk-8-0-8] ||

Please use [dotnet/core #xxx](https://github.com/dotnet/core/issues/xxxx) to participate in the conversation about these updates. 

## .NET Framework August 2024 Updates

This month, there are security and non-security updates in these releases, be sure to browse our [release notes for .NET Framework](https://learn.microsoft.com/dotnet/framework/release-notes/2024/08-13-August-security-and-quality-rollup) for more details. 


[efcore-6-0-33]: https://github.com/dotnet/efcore/issues?q=milestone%3A6.0.33+is%3Aclosed+label%3Aservicing-approved+
[efcore-8-0-8]: https://github.com/dotnet/efcore/issues?q=milestone%3A8.0.8+is%3Aclosed+label%3Aservicing-approved+
[sdk-6-0-33]: https://github.com/dotnet/sdk/issues?q=milestone%3A7.0.16+is%3Aclosed+label%3Aservicing-approved+
[sdk-8-0-8]: https://github.com/dotnet/sdk/issues?q=milestone%3A8.0.8+is%3Aclosed+label%3Aservicing-approved+
[runtime-6-0-33]: https://github.com/dotnet/runtime/issues?q=milestone%3A6.0.33+is%3Aclosed+label%3Aservicing-approved+
[runtime-8-0-8]: https://github.com/dotnet/runtime/issues?q=milestone%3A8.0.8+is%3Aclosed+label%3Aservicing-approved+
[roslyn-8-0-8]: https://github.com/dotnet/roslyn-analyzers/issues?q=milestone%3A8.0.8xx+is%3Aclosed+label%3Aservicing-approved+
[aspnet-6-0-33]: https://github.com/dotnet/aspnetcore/issues?q=milestone%3A6.0.33+is%3Aclosed+label%3Aservicing-approved+
[maui-8-0-8]: https://github.com/dotnet/maui/releases/tag/8.0.80
[aspnet-8-0-8]: https://github.com/dotnet/aspnetcore/issues?q=milestone%3A8.0.8+is%3Aclosed+label%3Aservicing-approved+
[winforms-8-0-8]: https://github.com/dotnet/winforms/issues?q=milestone%3A8.0.8+is%3Aclosed+label%3Aservicing-approved+
[winforms-6-0-33]: https://github.com/dotnet/winforms/issues?q=milestone%3A6.0.33+is%3Aclosed+label%3Aservicing-approved+
[wpf-8-0-8]: https://github.com/dotnet/wpf/issues?q=milestone%3A8.0.8+is%3Aclosed+label%3Aservicing-approved+
[wpf-6-0-33]: https://github.com/dotnet/wpf/issues?q=milestone%3A6.0.33+is%3Aclosed+label%3Aservicing-approved+
[templating-7-0-15]: https://github.com/dotnet/templating/issues?q=milestone%3A7.0.16+is%3Aclosed+label%3Aservicing-approved+
