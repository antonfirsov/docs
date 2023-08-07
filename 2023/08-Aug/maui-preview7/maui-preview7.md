---
post_title: 'Announcing .NET MAUI in .NET 8 Preview 7: Accelerators'
author1: davidortinau
post_slug: announcing-dotnet-maui-in-dotnet-8-preview-7
microsoft_alias: daortin
featured_image: dotnet-maui-dotnet8p7.png
categories: .NET, .NET MAUI
summary: .NET MAUI in .NET 8 Preview 7 has more new quality improvements for developers, experimental new AOT for iOS, and exciting new Visual Studio support.
tags: .net 8, .net maui
post_date: 2023-08-08 10:07:00
---

.NET MAUI is now available in .NET 8 Preview 7 introducing keyboard accelerators and more fixes and improvements.

> A new .NET 7 Service Release is also available today. See the [release notes](https://github.com/dotnet/maui/releases) for full details. We are currently focusing on .NET 8 quality which means only the most critical fixes will be released for .NET 7. Once .NET 8 ships GA, we will reevaluate the requirements for which fixes are included in service releases.

## Desktop Keyboard Accelerators

Keyboard accelerators enable you to assign keyboard shortcuts to any menu item, whether visible or not, and attached to any UI element. In this example, the page has a window level menu:

```xaml
<ContentPage.MenuBarItems>
    <MenuBarItem Text="File">
        <MenuFlyoutItem Text="Preferences"
            Command="{Binding PreferencesCommand}
        />
    </MenuBarItem>
    <MenuBarItem Text="Products">
        <MenuFlyoutItem 
            x:Name="AddProductMenu"
            Text="Add Product"
            Command="{Binding AddProductCommand}"
        />
        <MenuBarItem Text="Add Product Category"/>
    </MenuBarItem>
</ContentPage.MenuBarItems>
```

In the C# partial class I can then add the shortcut I want to trigger the menu item, just as if I had clicked or tapped on it.

```csharp
MenuItem.SetAccelerator(AddProductMenu, Accelerator.FromString("Ctrl+Shft+A"));
```

Now when those keys are triggered, the `AddProductCommand` fires.

//video here

## What's fixed and improved in .NET MAUI

Here are the main themes from the release notes with the associated pull request numbers and links:

1. **Memory Leak Resolutions**: 
   - Several memory leak issues were addressed in various UI controls, including Border, Editor, and Entry on different platforms, such as iOS, Android, and Windows. These fixes ensure improved memory management and application stability. [#15946](https://github.com/dotnet/maui/pull/15946), [#15614](https://github.com/dotnet/maui/pull/15614), [#16045](https://github.com/dotnet/maui/pull/16045), [#16101](https://github.com/dotnet/maui/pull/16101), [#16348](https://github.com/dotnet/maui/pull/16348), [#16349](https://github.com/dotnet/maui/pull/16349)

2. **Enhanced UI Control Functionality**: 
   - UI controls like Border, WebView, and Entry received updates to their behavior, performance, and customization options on different platforms (iOS, Android, Windows). These enhancements contribute to a more user-friendly and feature-rich experience. [#14740](https://github.com/dotnet/maui/pull/14740), [#15881](https://github.com/dotnet/maui/pull/15881), [#15585](https://github.com/dotnet/maui/pull/15585), [#14846](https://github.com/dotnet/maui/pull/14846), [#16215](https://github.com/dotnet/maui/pull/16215), [#15458](https://github.com/dotnet/maui/pull/15458), [#16270](https://github.com/dotnet/maui/pull/16270)

3. **Platform-Specific Improvements**: 
   - Each major platform (iOS, Android, Windows) saw targeted improvements, ranging from memory leak fixes to performance enhancements, ensuring that the app runs smoothly and efficiently across diverse environments. [#15734](https://github.com/dotnet/maui/pull/15734), [#16145](https://github.com/dotnet/maui/pull/16145), [#16032](https://github.com/dotnet/maui/pull/16032)

4. **Bug Fixes and Refinements**: 
   - Several bugs, ranging from appearance issues (Shell TabBar) to functionality (SelectedItemChanged in ListView), were resolved across different platforms. These fixes contribute to a more polished and error-free application. [#16128](https://github.com/dotnet/maui/pull/16128), [#16241](https://github.com/dotnet/maui/pull/16241), [#16275](https://github.com/dotnet/maui/pull/16275), [#14663](https://github.com/dotnet/maui/pull/14663), [#16057](https://github.com/dotnet/maui/pull/16057), [#16116](https://github.com/dotnet/maui/pull/16116), [#16174](https://github.com/dotnet/maui/pull/16174), [#16248](https://github.com/dotnet/maui/pull/16248), [#15099](https://github.com/dotnet/maui/pull/15099), [#15459](https://github.com/dotnet/maui/pull/15459)

5. **Input and Interaction Enhancements**: 
   - Improvements were made to user input and interaction features, such as cursor preservation in text boxes, menu key accelerators, and InputTransparent behavior permutations. These updates enhance user engagement and application usability. [#15799](https://github.com/dotnet/maui/pull/15799), [#15835](https://github.com/dotnet/maui/pull/15835)


## How to update

Visual Studio 2022 on Windows now includes .NET 8 previews and the .NET MAUI preview workload. Download the latest preview version (17.8 Preview 1), select the .NET Multi-platform App UI workload, and then check the optional component ".NET MAUI (.NET 8 Preview)".

![Visual Studio installer checkbox for .NET MAUI and .NET 8 previews](vs17.7-p2-installer.png)

If you are on macOS, you can now develop using Visual Studio for Mac after enabling the preview feature for .NET 8 in Preferences and installing .NET 8 preview 7 from the installer.

![Enable .NET 8 in Visual Studio 2022 for Mac](vsm_enable_net8.png)

Download the [.NET 8 preview 7 installer](https://dotnet.microsoft.com/download/dotnet/8.0), and then install .NET MAUI from the command line:

```bash
dotnet workload install maui
```

## Feedback Welcome

We appreciate your feedback and contributions to .NET MAUI. You can [report issues](https://github.com/dotnet/maui/issues/new/choose), [suggest features](https://github.com/dotnet/maui/issues/new?assignees=&labels=proposal%2Fopen%2Ct%2Fenhancement&projects=&template=feature-request.yml), or [submit pull requests](https://github.com/dotnet/maui/blob/main/.github/CONTRIBUTING.md) on our GitHub repository. You can also join our Discord server or follow us on Twitter to stay in touch with the latest news and updates.

Thank you for your support and happy coding!