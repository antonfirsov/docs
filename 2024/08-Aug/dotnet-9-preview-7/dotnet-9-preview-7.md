---
post_title: .NET 9 Preview 7 is now available!
author1: dotnet
post_slug: dotnet-9-preview-7
microsoft_alias: jamont
featured_image: dotnet9p7.jpg
categories: .NET, .NET MAUI, ASP.NET Core, Blazor, C#
tags: .net 9, featured-preview
ai_note: hide
summary: Try out the latest features in .NET 9 Preview 7 across the .NET runtime, SDK, libraries, ASP.NET Core, Blazor, C#, .NET MAUI, and more!
post_date: 2024-08-15 14:00:00
---

It's a great time to check out the latest .NET 9 Preview! We just shipped our **seventh** preview release, adding to some major enhancements across the .NET Runtime, SDK, libraries, C#, ASP.NET Core, Blazor, and .NET MAUI. Check out the full release notes linked below and get started today.

[cta-button align="center" text="Download .NET 9 Preview 7" url="https://dotnet.microsoft.com/download/dotnet/9.0" color="#5C2D91"]

This release contains the following improvements.

## **📚Libraries**

- [Removal of `BinaryFormatter` is complete](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview7/libraries.md#removal-of-binaryformatter-is-complete)
- [Enumerate over `ReadOnlySpan<char>.Split()` segments](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview7/libraries.md#enumerate-over-readonlyspancharsplit-segments)
- [`Debug.Assert` now reports assert condition, by default.](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview7/libraries.md#debugassert-now-reports-assert-condition-by-default)
- [Compression APIs now use `zlib-ng`](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview7/libraries.md#compression-apis-now-use-zlib-ng)
- [`Guid.CreateVersion7` enables creating GUIDs with a natural sort order](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview7/libraries.md#guidcreateversion7-enables-creating-guids-with-a-natural-sort-order)
- [`Interlocked.CompareExchange` for more types](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview7/libraries.md#interlockedcompareexchange-for-more-types)
- [AES-GCM and ChaChaPoly1305 algorithms enabled for iOS/tvOS/MacCatalyst](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview7/libraries.md#aes-gcm-and-chachapoly1305-algorithms-enabled-for-iostvosmaccatalyst)
- [Changes to X.509 Certificate Loading](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview7/libraries.md#changes-to-x509-certificate-loading)
- [Support for XPS documents from XPS virtual printer](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview7/libraries.md#support-for-xps-documents-from-xps-virtual-printer)
- [Marking `Tensor<T>` as `Experimental`](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview7/libraries.md#marking-tensort-as-experimental)
- Full [release notes](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview7/libraries.md)

## **⏱️Runtime**

- [ARM64 SVE Support](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview7/runtime.md#arm64-sve-support)
- [Post-Indexed Addressing on ARM64](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview7/runtime.md#post-indexed-addressing-on-arm64)
- [Strength Reduction in Loops](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview7/runtime.md#strength-reduction-in-loops)
- [Object Stack Allocation for Boxes](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview7/runtime.md#object-stack-allocation-for-boxes)
- [GC Dynamic Adaptation To Application Sizes](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview7/runtime.md#gc-dynamic-adaptation-to-application-sizes)
- Full [release notes](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview7/runtime.md)

## **C/#**

- [Prioritize better overloads with `OverloadResolutionPriority` attribute](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview7/csharp.md#prioritize-better-overloads-with-overloadresolutionpriority-attribute)
- Full [release notes](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview7/csharp.md)

## **🛠️ SDK**

- [Container publishing improvements for insecure registries](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview7/sdk.md#container-publishing-improvements-for-insecure-registries)
- [More consistent environment variables for container publishing](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview7/sdk.md#more-consistent-environment-variables-for-container-publishing)
- [Introduction of Workload Sets for more control over workloads](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview7/sdk.md#introduction-of-workload-sets-for-more-control-over-workloads)
- [Mitigating analyzer mismatch issues aka 'torn SDK'](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview7/sdk.md#mitigating-analyzer-mismatch-issues-aka-torn-sdk)
- Full [release notes](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview7/sdk.md)

## **🌐 ASP.NET Core**

- [SignalR supports trimming and Native AOT](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview7/aspnetcore.md#signalr-supports-trimming-and-native-aot)
- [Microsoft.AspNetCore.OpenApi supports trimming and Native AOT](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview7/aspnetcore.md#microsoftaspnetcoreopenapi-supports-trimming-and-native-aot)
- [Improvements to transformer registration APIs in Microsoft.AspNetCore.OpenApi](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview7/aspnetcore.md#improvements-to-transformer-registration-apis-in-microsoftaspnetcoreopenapi)
- [Call `ProducesProblem` and `ProducesValidationProblem` on route groups](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview7/aspnetcore.md#call-producesproblem-and-producesvalidationproblem-on-route-groups)
- [Construct `Problem` and `ValidationProblem` result types with `IEnumerable<KeyValuePair<string, object?>>` values](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview7/aspnetcore.md#construct-problem-and-validationproblem-result-types-with-ienumerablekeyvaluepairstring-object-values)
- [`OpenIdConnectHandler` support for Pushed Authorization Requests (PAR)](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview7/aspnetcore.md#openidconnecthandler-support-for-pushed-authorization-requests-par)
- [Data Protection support for deleting keys](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview7/aspnetcore.md#data-protection-support-for-deleting-keys)
- [Customize Kestrel named pipe endpoints](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview7/aspnetcore.md#customize-kestrel-named-pipe-endpoints)
- [Improved Kestrel connection metrics](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview7/aspnetcore.md#improved-kestrel-connection-metrics)
- [Opt-out of HTTP metrics on certain endpoints and requests](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview7/aspnetcore.md#opt-out-of-http-metrics-on-certain-endpoints-and-requests)
- [`ExceptionHandlerMiddleware` option to choose the status code based on the exception](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview7/aspnetcore.md#exceptionhandlermiddleware-option-to-choose-the-status-code-based-on-the-exception)
- Full [release notes](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview7/aspnetcore.md)

## **📱 .NET MAUI**

- [Introduction of `HybridWebview`](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview7/dotnetmaui.md#introduction-of-hybridwebview)
- [New `TitleBar` Control and `Window.TitleBar` for Windows](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview7/dotnetmaui.md#new-titlebar-control-and-windowtitlebar-for-windows)
- [`CollectionView` & `CarouselView` improvements with a new opt-in handler for iOS and Mac Catalyst](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview7/dotnetmaui.md#collectionview--carouselview-improvements-with-a-new-opt-in-handler-for-ios-and-mac-catalyst)
- [Ability to bring a `Window` to the foregrond with `ActivateWindow`](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview7/dotnetmaui.md#activatewindow-added-to-bring-a-window-to-foreground)
- [`BackButtonBehavior` `OneWay` binding mode](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview7/dotnetmaui.md#backbuttonbehavior-oneway-binding-mode)
- [`BlazorWebView` backward compatibility host address](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview7/dotnetmaui.md#blazorwebview-backward-compatibility-host-address)
- [Native Embedding improvements](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview7/dotnetmaui.md#native-embedding-improvements)
- [`MainPage` is Obsolete](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview7/dotnetmaui.md#mainpage-is-obsolete)
- [New Handler Disconnect Policy](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview7/dotnetmaui.md#new-handler-disconnect-policy)
- [New `ProcessTerminated` event on `WebView` Control](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview7/dotnetmaui.md#new-processterminated-event-on-webview-control)
- [New lifecycle methods for remote notifications on iOS & Mac Catalyst](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview7/dotnetmaui.md#new-lifecycle-methods-for-remote-notifications-on-ios--mac-catalyst)
- [Xcode Sync for CLI and Visual Studio Code](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview7/dotnetmaui.md#xcode-sync-for-cli-and-visual-studio-code)
- Full [release notes](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview7/dotnetmaui.md)

## 🚀 Get started

To get started with .NET 9, [install the .NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0).

If you're on Windows using Visual Studio, we recommend installing the latest [Visual Studio 2022 preview](https://visualstudio.microsoft.com/vs/preview/). .NET 9 can now be installed directly through the Visual Studio installer starting with Visual Studio 2022 17.12 Preview 1.

You can also use Visual Studio Code and the [C# Dev Kit](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit) extension with .NET 9.

## 📢 Team Announcements & Discussions

The team has been making [monthly announcements](https://github.com/dotnet/core/discussions/9448) alongside full [release notes](https://github.com/dotnet/core/tree/main/release-notes/9.0) on the [dotnet/core GitHub Discussions](https://github.com/dotnet/core/discussions/categories/news) and has seen great engagement and feedback from the community. We will continue to post each new release on GitHub, but as we get closer to launch this November alongside [.NET Conf 2024 (save the date today!)](https://www.dotnetconf.net/), we wanted to cross-post our release details on the .NET blog. 

- [.NET MAUI](https://github.com/dotnet/maui/discussions/24219)
- [ASP.NET Core](https://github.com/dotnet/aspnetcore/discussions/57312)
- [Libraries & Runtime](https://github.com/dotnet/runtime/discussions/106350)
- [Source-build](https://github.com/dotnet/source-build/discussions/4551)

## 🔔 Stay up-to-date with .NET 9

You can stay up-to-date with all the features of .NET 9 with:

- [What's new in .NET 9](https://learn.microsoft.com/dotnet/core/whats-new/dotnet-9/overview)
- [What's new in ASP.NET Core](https://learn.microsoft.com/aspnet/core/release-notes/aspnetcore-9.0)
- [What's new in .NET MAUI](https://learn.microsoft.com/dotnet/maui/whats-new/dotnet-9)
- [What's new in EF Core](https://learn.microsoft.com/ef/core/what-is-new/ef-core-9.0/whatsnew)
- [Breaking Changes in .NET 9](https://learn.microsoft.com/dotnet/core/compatibility/9.0)
- [.NET 9 Releases](https://github.com/dotnet/core/blob/main/release-notes/9.0/README.md)

Additionally, be sure to subscribe to the GitHub Discussions [RSS news feed](https://github.com/dotnet/core/discussions/categories/news.atom) for all release announcements.  

We want your feedback, so head over to the [.NET 9 Preview 7 GitHub Discussion](https://github.com/dotnet/core/discussions/9448) to discuss features and give feedback for this release.