---
post_title: 'Announcing .NET MAUI in .NET 8 Preview 5'
author1: davidortinau
post_slug: announcing-dotnet-maui-in-dotnet-8-preview-5
microsoft_alias: daortin
featured_image: dotnet-maui-dotnet8p5.png
categories: .NET, .NET MAUI
summary: .NET MAUI makes its way to .NET 8 Preview 5 and is full of new quality improvements for developers across the entire framework. In addition, we are introducing NuGet packages even greater flexibility going forward.
tags: .net 8, .net maui
post_date: 2023-06-13 10:07:00
---

Hello everyone! I'm thrilled to announce that .NET MAUI in .NET 8 Preview 5 is now available with lots of bug fixes and performance improvements for cross-platform app development. In this post, I'll summarize some of the most important changes in this release and show you how to update your .NET MAUI project to use this release.

## What's fixed and improved in .NET MAUI

Preview 5 is another quality-focused update that fixes many issues and enhances the performance of the framework, including:

- **iOS Keyboard Scrolling:** ContentInsets were added to improve the scrolling behavior of the iOS keyboard. [#14371](https://github.com/dotnet/maui/pull/14371)
- **Test Improvements:** Various improvements and fixes were made to the tests, including the removal of the skip attribute from a test related to SearchBarHandler. [#14852](https://github.com/dotnet/maui/pull/14852)
- **Performance Enhancements:** Performance improvements were made to the {Binding} mechanism and the layout performance of labels on Android. [#14830](https://github.com/dotnet/maui/pull/14830), [#14933](https://github.com/dotnet/maui/pull/14933), [#14980](https://github.com/dotnet/maui/pull/14980)
- **Bug Fixes:** Several bug fixes were implemented, addressing issues such as gestures in Label Spans, Entry issues with the keyboard, Label truncation on iOS, CollectionView issues, ContentView RTL, and more. [#14410](https://github.com/dotnet/maui/pull/14410), [#14382](https://github.com/dotnet/maui/pull/14382), [#14453](https://github.com/dotnet/maui/pull/14453), [#14391](https://github.com/dotnet/maui/pull/14391), [#11763](https://github.com/dotnet/maui/pull/11763), [#15114](https://github.com/dotnet/maui/pull/15114), [#12909](https://github.com/dotnet/maui/pull/12909)

These are just some of the highlights of this release. For a complete list of changes, please check out the [release notes](https://github.com/dotnet/maui/releases).

## How to update

Visual Studio 2022 on Windows now includes .NET 8 previews and the .NET MAUI preview workload. Download the latest preview version 17.7 Preview 2, select the .NET Multi-platform App UI workload, and then check the optional component ".NET MAUI (.NET 8 Preview)".

![Visual Studio installer checkbox for .NET MAUI and .NET 8 previews](vs17.7-p2-installer.png)

If you are on macOS, you may download the [.NET 8 preview 5 installer](https://dotnet.microsoft.com/download/dotnet/8.0), and then install .NET MAUI from the command line:

```bash
dotnet workload install maui
```

To verify that everything is installed correctly, you can run the following command:

```bash
dotnet --list-sdks
```

You should see something like this:

```text
8.0.100-preview.5.23303.2 [C:\Program Files\dotnet\sdk]
```

And to verify you have the correct .NET MAUI workload, run the command:

```bash
dotnet workload list
```

You should see something like this:

```text
Installed Workload Id      Manifest Version                            Installation Source
-----------------------------------------------------------------------------------------------------------
maui-windows               8.0.0-preview.5.8529/8.0.100-preview.5      VS 17.5.33627.172, VS 17.7.33808.371
maui-maccatalyst           8.0.0-preview.5.8529/8.0.100-preview.5      VS 17.5.33627.172, VS 17.7.33808.371
maccatalyst                16.4.8525-net8-p5/8.0.100-preview.5         VS 17.5.33627.172, VS 17.7.33808.371
maui-ios                   8.0.0-preview.5.8529/8.0.100-preview.5      VS 17.5.33627.172, VS 17.7.33808.371
ios                        16.4.8525-net8-p5/8.0.100-preview.5         VS 17.5.33627.172, VS 17.7.33808.371
maui-android               8.0.0-preview.5.8529/8.0.100-preview.5      VS 17.5.33627.172, VS 17.7.33808.371
android                    34.0.0-preview.5.312/8.0.100-preview.5      VS 17.5.33627.172, VS 17.7.33808.371
```


### Updating existing projects

To update your .NET MAUI project to use 8.0.0-preview.5.8506, you first need to install the .NET 8 Preview 5 SDK and then update the .NET MAUI NuGet packages in your project. You can use Visual Studio or Visual Studio for Mac to manage your NuGet packages or edit your project file manually.


To use Visual Studio or Visual Studio for Mac, right-click on your project and select "Manage NuGet Packages..." then select the "Include prerelease" option and search for Microsoft.Maui.* packages. Update all the packages to version 8.0.0-preview.5.8506. To edit your project file manually, open it in a text editor and find the `ItemGroup` element that contains the `PackageReference` elements for Microsoft.Maui.* packages. Update all the Version attributes to 8.0.0-preview.5.8506.


Your project file should have an `ItemGroup` that looks something like this:

```xml
<ItemGroup>
	<PackageReference Include="Microsoft.Maui.Controls" Version="$(MauiVersion)" />
	<PackageReference Include="Microsoft.Maui.Controls.Compatibility" Version="$(MauiVersion)" />
	<PackageReference Include="Microsoft.Extensions.Logging.Debug" Version="8.0.0-preview.5.23280.8" />
</ItemGroup>
```

Now you can build and run your project using Visual Studio or Visual Studio for Mac or use the dotnet build command with the -t:Run target:

```bash
dotnet build -t:Run -f net8-android MyMauiApp.csproj
```

This will build and launch your app on an Android emulator or device.

## Feedback Welcome

We appreciate your feedback and contributions to .NET MAUI. You can [report issues](https://github.com/dotnet/maui/issues/new/choose), [suggest features](https://github.com/dotnet/maui/issues/new?assignees=&labels=proposal%2Fopen%2Ct%2Fenhancement&projects=&template=feature-request.yml), or [submit pull requests](https://github.com/dotnet/maui/blob/main/.github/CONTRIBUTING.md) on our GitHub repository. You can also join our Discord server or follow us on Twitter to stay in touch with the latest news and updates.

Thank you for your support and happy coding!