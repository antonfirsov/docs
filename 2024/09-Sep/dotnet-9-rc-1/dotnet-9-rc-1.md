---
post_title: .NET 9 Release Candidate 1 is now available!
author1: dotnet
post_slug: dotnet-9-release-candidate-1-is-now-available
microsoft_alias: jogallow
featured_image: dotnet9rc1.jpg
categories: .NET, .NET MAUI, ASP.NET Core, C#
tags: .net 9, featured-preview
ai_note: hide
summary: Try out the latest features in .NET 9 Release Candidate 1 across the .NET SDK, libraries, ASP.NET Core, SignalR, .NET MAUI, and more!
post_date: 2024-09-11 13:00:00
---

.NET 9 Release Candidate 1 is now available. This is our first of two release candidates. This release includes enhanced WebSocket APIs, new compression options, advanced SignalR tracing, and updates to .NET MAUI for better text alignment, and more. Check out the full release notes linked below and get started today.

[cta-button align="center" text="Download .NET 9 Release Candidate 1" url="https://dotnet.microsoft.com/download/dotnet/9.0" color="#5C2D91"]

[alert type="note" heading="Get ready for .NET Conf!"]The dates for [.NET Conf 2024](https://dotnetconf.net/) have been announced! Join us November 12-14, 2024 to celebrate the .NET 9 release![/alert]

This release contains the following improvements.

## **📚Libraries**

* [WebSocket `Keep-Alive` Ping and Timeout APIs](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/rc1/libraries.md#websocket-keep-alive-ping-and-timeout)
* [Add ZLib, Brotli compression options](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/rc1/libraries.md#add-zlib-and-brotli-compression-options)
* [Add TarEntry.DataOffset](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/rc1/libraries.md#add-tarentrydataoffset)
* [`HttpClientFactory` no longer logs header values by default](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/rc1/libraries.md#httpclientfactory-no-longer-logs-header-values-by-default)
* [Out-of-proc Meter wildcard listening](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/rc1/libraries.md#Out-of-proc-Meter-wildcard-listening)

* Full [release notes](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/rc1/libraries.md)

## **🛠️ SDK**

* [Workload History](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/rc1/sdk.md#workload-history)

* Full [release notes](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/rc1/sdk.md)

## **🌐 ASP.NET Core**

* [Improvements to SignalR distributed tracing](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/rc1/aspnetcore.md#improvements-to-signalr-distributed-tracing)
* [Keep-alive timeout for WebSockets](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/rc1/aspnetcore.md#keep-alive-timeout-for-websockets)
* [Keyed DI in middleware](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/rc1/aspnetcore.md#keyed-di-in-middleware)
* [Override `InputNumber` type attribute](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/rc1/aspnetcore.md#override-inputnumber-type-attribute)
* [Trust the ASP.NET Core HTTPS development certificate on Linux](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/rc1/aspnetcore.md#trust-the-aspnet-core-https-development-certificate-on-linux)

* Full [release notes](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/rc1/aspnetcore.md)

## **📱 .NET MAUI**

* [Added `HorizontalTextAlignment.Justify`](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/rc1/dotnetmaui.md#added-horizontaltextalignmentjustify)

* Full [release notes](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/rc1/dotnetmaui.md)

## 🚀 Get started

To get started with .NET 9, [install the .NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0).

If you're on Windows using Visual Studio, we recommend installing the latest [Visual Studio 2022 preview](https://visualstudio.microsoft.com/vs/preview/). .NET 9 can now be installed directly through the Visual Studio installer starting with Visual Studio 2022 17.12 Preview 2.

You can also use Visual Studio Code and the [C# Dev Kit](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit) extension with .NET 9.

## 📢 Team Announcements & Discussions

The team has been making [monthly announcements](https://github.com/dotnet/core/discussions/9448) alongside full [release notes](https://github.com/dotnet/core/tree/main/release-notes/9.0) on the [dotnet/core GitHub Discussions](https://github.com/dotnet/core/discussions/categories/news) and has seen great engagement and feedback from the community. We will continue to post each new release on GitHub, but as we get closer to launch this November alongside [.NET Conf 2024 (save the date today!)](https://www.dotnetconf.net/), we wanted to cross-post our release details on the .NET blog. 

* [.NET MAUI](https://github.com/dotnet/maui/discussions/24698)
* [ASP.NET Core](https://github.com/dotnet/aspnetcore/discussions/57787)
* [Libraries & Runtime](https://github.com/dotnet/runtime/discussions/106350)

## 🔔 Stay up-to-date with .NET 9

You can stay up-to-date with all the features of .NET 9 with:

- [What's new in .NET 9](https://learn.microsoft.com/dotnet/core/whats-new/dotnet-9/overview)
- [What's new in ASP.NET Core](https://learn.microsoft.com/aspnet/core/release-notes/aspnetcore-9.0)
- [What's new in .NET MAUI](https://learn.microsoft.com/dotnet/maui/whats-new/dotnet-9)
- [What's new in EF Core](https://learn.microsoft.com/ef/core/what-is-new/ef-core-9.0/whatsnew)
- [Breaking Changes in .NET 9](https://learn.microsoft.com/dotnet/core/compatibility/9.0)
- [.NET 9 Releases](https://github.com/dotnet/core/blob/main/release-notes/9.0/README.md)

Additionally, be sure to subscribe to the GitHub Discussions [RSS news feed](https://github.com/dotnet/core/discussions/categories/news.atom) for all release announcements.  

We want your feedback, so head over to the [.NET 9 Release Candidate 1 GitHub Discussion](https://github.com/dotnet/core/discussions/9496) to discuss features and give feedback for this release.
