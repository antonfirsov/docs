---
post_title: 'Announcing .NET MAUI in .NET 8 Release Candidate 2: More Quality'
author1: davidortinau
post_slug: announcing-dotnet-maui-in-dotnet-8-rc-2
microsoft_alias: daortin
featured_image: maui-net8-rc2.png
categories: .NET, .NET MAUI
summary: .NET MAUI in .NET 8 RC2 has still more quality improvements for developers, and we have some bonus news.
tags: .net 8, .net maui
post_date: 2023-10-10 10:07:00
---

Today, we take one step closer to .NET 8 general availability (GA) by shipping .NET MAUI in .NET 8 release candidate 2 (RC2). As with RC1, this release is covered by a go-live license so you can receive support when using it in your production applications. In this release we have focused on issues that regressed throughout the previews, and regaining some performance that was lost as we improved the reliability of hot reload, visual state manager, bindings, and app themes. 

## Quality Improvements

In addition to our narrowed focus on regressions we have also increased the battery of manual tests and automated testing. There are no breaking API changes between .NET 7 and .NET 8, so you can expect the upgrade for your applications to go smoothly in that respect. For more information about upgrading from 7 to 8, follow [this simple guide](https://github.com/dotnet/maui/wiki/Upgrading-.NET-MAUI-from-.NET-7-to-.NET-8).

Highlights from this release:

**Performance Improvements:**
Several performance improvements were made, including enhancements to the performance of ActivityExtensions.GetWindowFrame on Android, and "Setter Specificity" performance. These optimizations contribute to smoother app performance. ([#17241](https://github.com/dotnet/maui/pull/17241), [#17527](https://github.com/dotnet/maui/pull/17527), [#17364](https://github.com/dotnet/maui/pull/17364), [#17230](https://github.com/dotnet/maui/pull/17230), [#17505](https://github.com/dotnet/maui/pull/17505), [#17545](https://github.com/dotnet/maui/pull/17545)).

**UI and Control Fixes:**
Several fixes and updates were made to controls and UI elements, including fixes related to CollectionView, TabBar visibility, RoundRectangle Borders, and Android text alignment. With these your app UI is more consistent and visually accurate across platforms. ([#16870](https://github.com/dotnet/maui/pull/16870), [#17240](https://github.com/dotnet/maui/pull/17240), [#17261](https://github.com/dotnet/maui/pull/17261), [#17311](https://github.com/dotnet/maui/pull/17311), [#17353](https://github.com/dotnet/maui/pull/17353), [#17348](https://github.com/dotnet/maui/pull/17348), [#17379](https://github.com/dotnet/maui/pull/17379), [#17411](https://github.com/dotnet/maui/pull/17411), [#17430](https://github.com/dotnet/maui/pull/17430), [#17436](https://github.com/dotnet/maui/pull/17436), [#17450](https://github.com/dotnet/maui/pull/17450), [#17539](https://github.com/dotnet/maui/pull/17539), [#17594](https://github.com/dotnet/maui/pull/17594)).

**Platform-Specific Fixes:**
Several platform-specific fixes were made, including drag-and-drop functionality, tab bar appearance, and specific platform behaviors, ensuring a consistent user experience across platforms. ([#15748](https://github.com/dotnet/maui/pull/15748), [#16561](https://github.com/dotnet/maui/pull/16561), [#17567](https://github.com/dotnet/maui/pull/17567), [#17495](https://github.com/dotnet/maui/pull/17495), [#17041](https://github.com/dotnet/maui/pull/17041), [#17358](https://github.com/dotnet/maui/pull/17358), [#17524](https://github.com/dotnet/maui/pull/17524), [#17530](https://github.com/dotnet/maui/pull/17530)).

The release also includes numerous other bug fixes, enhancements, and contributions. Check out the full release notes for more details.

Additional information:
* [.NET MAUI release notes](https://github.com/dotnet/maui/releases/tag/8.0.0-rc.2.9373)
* [.NET for Android](https://github.com/xamarin/xamarin-android/releases/)
* [.NET for iOS and Mac](https://github.com/xamarin/xamarin-macios/releases/)

## Bonus: .NET 7 Service Release

Today we have also shipped [.NET MAUI service release 8](https://github.com/dotnet/maui/releases/tag/7.0.96) (version 7.0.96) for .NET 7 including select high-priority fixes for layout, memory leaks, CollectionView, safe area, and more. You can use this service release by installing .NET 8 RC2 using one of the methods below and continuing to build your applications to target .NET 7.

Alternatively, you can acquire 7.0.96 by upgrading to Visual Studio 17.7.5. 

## Bonus 2: Xcode 15 and Android API 34

Xamarin developers can now use Xcode 15 to target the latest versions (e.g iOS 17, iPad 17), and build for Android API 34 in order to be compliant with store policies. To do this, install Visual Studio 17.8 Preview 3 or the latest stable version of Visual Studio for Mac and configure your environment as usual. This does not provide newer platform APIs, but does enable existing projects to continue building while you complete your upgrades to .NET 8 and .NET MAUI regardless of the [Xamarin end-of-support date next year](https://dotnet.microsoft.com/platform/support/policy/xamarin).

## How to update

On all platforms, you can develop with .NET MAUI using Visual Studio Code. Install the [.NET MAUI extension](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.dotnet-maui) and [let us know](https://www.surveymonkey.com/r/W789CW2) how we can improve this preview experience for you in the future. 

Download the [.NET 8 RC2 installer](https://dotnet.microsoft.com/download/dotnet/8.0), and then install .NET MAUI from the command line:

```bash
dotnet workload install maui
```

Through the [retirement of Visual Studio for Mac next year](https://devblogs.microsoft.com/visualstudio/visual-studio-for-mac-retirement-announcement/) you can continue developing using Visual Studio for Mac after enabling the preview feature for .NET 8 in Preferences.

On Windows, update or install Visual Studio 2022 17.8 preview 3 to get .NET 8 RC2 with .NET MAUI (and 7.0.96). 

## Feedback Welcome

We appreciate your feedback and contributions to .NET MAUI. You can [report issues](https://github.com/dotnet/maui/issues/new/choose), [suggest features](https://github.com/dotnet/maui/issues/new?assignees=&labels=proposal%2Fopen%2Ct%2Fenhancement&projects=&template=feature-request.yml), or [submit pull requests](https://github.com/dotnet/maui/blob/main/.github/CONTRIBUTING.md) on our GitHub repository. You can also join our Discord server or follow us on Twitter to stay in touch with the latest news and updates.

Thank you to all 23 contributors (and bots) who helped put this release together!

Thank you for your support and happy coding!
