---
post_title: .NET 9 Preview 6 is now available!
author1: dotnet
post_slug: dotnet-9-preview-6
microsoft_alias: jamont
featured_image: dotnet9p6.jpg
categories: .NET, .NET MAUI, ASP.NET Core, Blazor, C#
tags: .net 9, featured-preview
ai_note: hide
summary: Try out the latest features in .NET 9 Preview 6 across the .NET runtime, SDK, libraries, ASP.NET Core, Blazor, and more!
post_date: 2024-07-15 10:05:00
---

It's a great time to check out the latest .NET 9 Preview! We just shipped our sixth preview release, adding to some great features in the previous previews with major enhancements across the .NET Runtime, SDK, libraries, C#, and frameworks including ASP.NET Core, Blazor, and .NET MAUI. Check out the full release notes linked below and get started today.

[cta-button align="center" text="Download .NET 9 Preview 6" url="https://dotnet.microsoft.com/download/dotnet/9.0" color="#5C2D91"]

This release contains the following improvements:

**📚Libraries:**
- [Improvements to System.Numerics](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview6/libraries.md#improvements-to-systemnumerics)
- [Support Primary Constructors in Logging Source Generator](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview6/libraries.md#support-primary-constructors-in-logging-source-generator)
- [System.Text.Json](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview6/libraries.md#systemtextjson)
- [`[GeneratedRegex]` on properties](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview6/libraries.md#generatedregex-on-properties)
- [`Regex.EnumerateSplits`](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview6/libraries.md#regexenumeratesplits)
- [`OrderedDictionary<TKey, TValue>`](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview6/libraries.md#ordereddictionarytkey-tvalue)
- [`ReadOnlySet<T>`](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview6/libraries.md#readonlysett)
- [`allows ref struct` used in many places throughout the libraries](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview6/libraries.md#allows-ref-struct-used-in-many-places-throughout-the-libraries)
- [Collection lookups with spans](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview6/libraries.md#collection-lookups-with-spans)
- [More span-based APIs](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview6/libraries.md#more-span-based-apis)
- [Base64Url](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview6/libraries.md#base64url)
- [SocketsHttpHandler by default in HttpClientFactory](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview6/libraries.md#socketshttphandler-by-default-in-httpclientfactory)
- [TLS resume with client certificates on Linux](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview6/libraries.md#tls-resume-with-client-certificates-on-linux)
- [`System.Net.ServerSentEvents`](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview6/libraries.md#systemnetserversentevents)
- [Introducing the Metrics Gauge Instrument](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview6/libraries.md#introducing-the-metrics-gauge-instrument)
- Full [release notes](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview6/libraries.md)

**⏱️Runtime:**
- [ARM64 Code Generation](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview6/runtime.md#arm64-code-generation)
- [Code Layout](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview6/runtime.md#code-layout)
- [Loop Optimizations](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview6/runtime.md#loop-optimizations)
- [Reduced Address Exposure](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview6/runtime.md#reduced-address-exposure)
- [AVX10v1 Support](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview6/runtime.md#avx10v1-support)
- [Hardware Intrinsic Code Generation](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview6/runtime.md#hardware-intrinsic-code-generation)
- [Constant Folding for Floating Point and SIMD Operations](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview6/runtime.md#constant-folding-for-floating-point-and-simd-operations)
- Full [release notes](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview6/runtime.md)

**🛠️SDK**

- [NuGetAudit now raises warnings for vulnerabilities in transitive dependencies](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview6/sdk.md#nugetaudit-now-raises-warnings-for-vulnerabilities-in-transitive-dependencies)
- [`dotnet nuget why`](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview6/sdk.md#dotnet-nuget-why)
- [MSBuild BuildChecks](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview6/sdk.md#msbuild-buildchecks)
- Full [release notes](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview6/sdk.md)

You can find detailed release notes for additional features in .NET 9 Preview 6 below:

## C#

- [Partial properties](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview6/csharp.md#partial-properties)
- Full [release notes](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview6/csharp.md)

## 🌐ASP.NET Core

- [Fingerprinting of static web assets](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview6/aspnetcore.md#fingerprinting-of-static-web-assets)
- [Improved distributed tracing for SignalR](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview6/aspnetcore.md#improved-distributed-tracing-for-signalr)
- [Enhancements to Microsoft.AspNetCore.OpenAPI](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview6/aspnetcore.md#enhancements-to-microsoftaspnetcoreopenapi)
- [Analyzer to warn when `[Authorize]` is overridden by `[AllowAnymous]`](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview6/aspnetcore.md#analyzer-to-warn-when-authorize-is-overridden-by-allowanymous-from-farther-away)
- [`ComponentPlatform` renamed to `RendererInfo`](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview6/aspnetcore.md#componentplatform-renamed-to-rendererinfo)
- [Split large HTTP/2 headers across frames](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview6/aspnetcore.md#split-large-http2-headers-across-frames)
- Full [release notes](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview6/aspnetcore.md)

## 📱.NET MAUI

- Full [release notes](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview6/dotnetmaui.md)

## 🚀Get started

To get started with .NET 9, [install the .NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0).

If you're on Windows using Visual Studio, we recommend installing the latest [Visual Studio 2022 preview](https://visualstudio.microsoft.com/vs/preview/), or get started with Visual Studio Code and the [C# Dev Kit](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit) extension.


## 📢Team Announcements & Discussions

The team has been making [monthly announcements](https://github.com/dotnet/core/discussions/9234) alongside full [release notes](https://github.com/dotnet/core/tree/main/release-notes/9.0) on the [dotnet/core GitHub Discussions](https://github.com/dotnet/core/discussions/categories/news) and has seen great engagement and feedback from the community. We will continue to post each new release on GitHub, but as we get closer to launch this November alongside [.NET Conf 2024 (save the date today!)](https://www.dotnetconf.net/), we wanted to cross-post our release details on the .NET blog. 

Discuss this release with the product teams on GitHub through the GitHub discussion announcement for this release.

* [.NET 9 Preview 6 Discussion](https://github.com/dotnet/core/discussions/9392)
* [.NET Libraries & Runtime Discussion](https://github.com/dotnet/runtime/discussions/104620)
* [.NET MAUI Discussion](https://github.com/dotnet/maui/discussions/23506) 
* [ASP.NET Core Discussion](https://github.com/dotnet/aspnetcore/discussions/56690)

## 🔔Stay up to date with .NET 9

You can stay up-to-date with all the features of .NET 9 with:

* [What's new in .NET 9](https://learn.microsoft.com/dotnet/core/whats-new/dotnet-9/overview)
* [What's new in C# 13](https://learn.microsoft.com/dotnet/csharp/whats-new/csharp-13)
* [What's new in ASP.NET Core](https://learn.microsoft.com/aspnet/core/release-notes/aspnetcore-9.0)
* [What's new in .NET MAUI](https://learn.microsoft.com/dotnet/maui/whats-new/dotnet-9)
* [What's new in EF Core](https://learn.microsoft.com/ef/core/what-is-new/ef-core-9.0/whatsnew)
* [Breaking Changes in .NET 9](https://learn.microsoft.com/dotnet/core/compatibility/9.0)
* [.NET 9 Releases](https://github.com/dotnet/core/blob/main/release-notes/9.0/README.md)

Additionally, be sure to subscribe to the GitHub Discussions [RSS news feed](https://github.com/dotnet/core/discussions/categories/news.atom) for all release announcements.  

We want your feedback, so head over to the [.NET 9 Preview 6 GitHub Discussion](https://github.com/dotnet/core/discussions/9392) to discuss features and give feedback for this release.