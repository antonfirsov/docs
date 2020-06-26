# Announcing .NET 5.0 Preview 6

Today, we're releasing .NET 5.0 Preview 6. It contains a small set of new features and performance improvements.  The [.NET 5.0 Preview 4 post](https://devblogs.microsoft.com/dotnet/announcing-net-5-preview-4-and-our-journey-to-one-net/) covers what we are planning to deliver with .NET 5.0. Most of the features are now in the product, but some are not yet in their final state. We expect that the release will be feature-complete with Preview 8.

You can [download .NET 5.0 Preview 6](https://dotnet.microsoft.com/download/dotnet/5.0), for Windows, macOS, and Linux:

* [Windows and macOS installers](https://dotnet.microsoft.com/download/dotnet/5.0)
* [Binaries](https://dotnet.microsoft.com/download/dotnet/5.0)
* [Docker images](https://hub.docker.com/_/microsoft-dotnet)
* [Snap installer](https://snapcraft.io/dotnet-sdk)

ASP.NET Core and EF Core are also being released today.

You need to use [Visual Studio 2019 16.7](https://visualstudio.microsoft.com/vs/preview/) to use .NET 5.0. .NET 5.0 is now supported with [Visual Studio for Mac Preview](https://visualstudio.microsoft.com/). Install the latest version of the [C# extension](https://code.visualstudio.com/Docs/languages/csharp), to use .NET 5.0 with [Visual Studio Code](https://visualstudio.microsoft.com/). 

Release notes:

* [.NET 5.0 release notes](https://github.com/dotnet/core/tree/master/release-notes/5.0)
* [.NET 5.0 known issues](https://github.com/dotnet/core/blob/master/release-notes/5.0/5.0-known-issues.md)
* [.NET 5.0 Runtime epics](https://github.com/dotnet/runtime/issues/37269)
* GitHub release
* GitHub tracking issue

## Windows ARM64 Update

We announced support for Windows ARM64 as part of [Preview 4](https://devblogs.microsoft.com/dotnet/announcing-net-5-preview-4-and-our-journey-to-one-net/). At that time, we had only enabled console and ASP.NET Core apps on Windows ARM64. The Preview 6 SDK now includes support for Windows Forms. That means you can build and run Windows Forms apps on Windows ARM64 devices, just like you would on x64. We're still working on adding support for WPF on Windows ARM64.

You can see a [sample Windows Forms app](https://github.com/richlander/testapps/tree/master/versioninfo-windowsforms/versioninfo) running on an ARM64 laptop, shown below.

<img width="573" alt="2020-05-13" src="https://user-images.githubusercontent.com/2608468/85644370-9497cc00-b64b-11ea-83ae-fe41a2248119.png">

Visual Studio .NET remote debugger support for Windows ARM64 is expected with Visual Studio 16.7. We expect Visual Studio Code .NET remote debugger support to follow soon after that. To avoid confusion, this support is referring to running Visual Studio or Visual Studio Code on an x64 machine, and remote attaching to a running .NET application on a Windows ARM64 machine. In addition, [Visual Studio Code is adding support for ARM64](https://github.com/microsoft/vscode/issues/98587). We will support the C# extension and the .NET debugger running within the Windows ARM64 version of Visual Studio Code, however, I don't have dates to share yet for that.

## Windows Forms

Visual Basic users are used to being able to enforce that their applications are single-instanced (one instance launched at a time). This behavior is now available via [WindowsFormsApplicationBase.IsSingleInstance](https://github.com/dotnet/winforms/pull/3200). Here's a [great explanation of this behavior from Scott Hanselman](https://www.hanselman.com/blog/TheWeeklySourceCode31SingleInstanceWinFormsAndMicrosoftVisualBasicdll.aspx). Credit: [@paul1956](https://github.com/paul1956) 

The team added [Collapse Support to ListViewGroup](https://github.com/dotnet/winforms/issues/3067). This [change](https://github.com/dotnet/winforms/pull/3155) makes it easier to manage a form with multiple `ListViewGroups`. Credit: [@lonitra](https://github.com/lonitra) (.NET Team intern).

You can see the result shown below.

![collapse2](https://user-images.githubusercontent.com/30007367/80524343-4e243a80-8944-11ea-8749-4529c305ae1c.gif)

## RyuJIT code quality improvements

The RyuJIT team continues to land really important improvements, preview after preview. They didn't dissapoint with Preview 6. Let's take a look:

* General improvements
  *	[Struct handling improvements](https://github.com/dotnet/runtime/pull/36146)
  *	[Optimization to remove redundant zero initializations](https://github.com/dotnet/runtime/pull/36918)
* [ARM64 hardware intrinsics](https://github.com/dotnet/runtime/issues/33308) implementation progress
  * [Implement Duplicate and DuplicateSelectedScalar](https://github.com/dotnet/runtime/pull/36144)
  * [ASIMD Shift Intrinsics](https://github.com/dotnet/runtime/pull/36830)
  * [Polynomial Multiply Long Intrinsics](https://github.com/dotnet/runtime/pull/36853)
  * [Optimize Vector64 and Vector128.Create methods](https://github.com/dotnet/runtime/pull/36267)
  * [Optimize ToScalar() and GetElement() to use arm64 intrinsic](https://github.com/dotnet/runtime/pull/36156)
  * [Optimize ToVector128, ToVector128Unsafe and Vector128.GetLower()](https://github.com/dotnet/runtime/pull/36732)
* ARM64 generated code improvements: greatly reduced ARM64 code size
  * [Optimize call indirect for R2R, Arm and Arm64 scenarios](https://github.com/dotnet/runtime/pull/35675)
  * [Optimize virtual call stub for R2R and JIT](https://github.com/dotnet/runtime/pull/36817) 

## Single file apps

We've been continuing to improve [Support Single-File Apps in .NET 5 ](https://github.com/dotnet/runtime/issues/36590). Our goal is to enable publishing an app as one file (obviously), for Windows, macOS and Linux. We're almost there. When we last talked about single file, with [Preview 4](https://devblogs.microsoft.com/dotnet/announcing-net-5-preview-4-and-our-journey-to-one-net/), I mentioned that Windows "single file" apps required a few extra runtime files. We added a [new option to include native binaries and any additional content](https://github.com/dotnet/designs/blob/master/accepted/2020/single-file/design.md#build-system-interface) (like images) in the single-file. These files will be extracted upon first launch. Apps that target Linux and macOS don't need to use this option for native runtime binaries, unless they want to use it for media or other content.

Current limitations:

* On Linux, the singlefilehost with runtime components linked in is still to be implemented. Therefore, the runtime native binaries will be published as separate files (similar to Windows experience). [#37119](https://github.com/dotnet/runtime/issues/37119) , [#38304](https://github.com/dotnet/runtime/issues/38304)
* On Linux, ready-to-run assemblies embedded in a bundle are loaded like IL assemblies.  [#38061](https://github.com/dotnet/runtime/issues/38061)

## Native hosted application

Over the years, we've seen a variety of hosting models for .NET in native applications. [@rseanhall](https://github.com/rseanhall) proposed and implemented [a novel new model for doing that](https://github.com/dotnet/runtime/issues/35465), which takes advantage of all the built-in application functionality offered by the .NET application hosting layer (specifically loading dependencies), while enabling a custom entrypoint to be called from native code. That's perfect for a lot of scenarios, and that one can imagine becoming popular with developers that host .NET components from native applications. That didn't exist before. Thanks for the contribution, [@rseanhall](https://github.com/rseanhall).

Two primary PRs:

* [Enable calling get_runtime_delegate from app context](https://github.com/dotnet/runtime/pull/37473)
* [Implement hdt_get_function_pointer](https://github.com/dotnet/runtime/pull/37696)

## [Breaking change] Removal of built-in WinRT support in .NET 5.0  

[Windows Runtime (WinRT)](https://blogs.windows.com/windowsdeveloper/2019/04/30/calling-windows-10-apis-from-a-desktop-application-just-got-easier/) is the technology and [ABI](https://en.wikipedia.org/wiki/Application_binary_interface) that new APIs are exposed with in Windows. You can call those APIs via .NET code, similar to how you would with C++. Support for WinRT interop was added in .NET Core 3.0, as part of adding support for Windows desktop client frameworks (Windows Forms and WPF). 

More recently, we've been working closely with the Windows team to change and improve the way that WinRT interop works with .NET. We have replaced the built-in WinRT support with the [C#/WinRT](https://docs.microsoft.com/windows/uwp/csharp-winrt/) tool chain, provided by the Windows team, in .NET 5.0. This [change in WinRT interop is a breaking change](https://github.com/dotnet/runtime/issues/37672), and .NET Core 3.x apps that use WinRT will need to be recompiled. We will provide more infromation on this in coming previews.

The benefits are called out in [Support WinRT APIs in .NET 5](https://github.com/dotnet/runtime/issues/35318):

* WinRT interop can be developed and improved separate from the .NET runtime.
* Makes WinRT interop symmetrical with interop systems provided for other operating systems, like iOS and Android.
* Can take advantage of many other .NET features (AOT, C# features, IL linking).
* Simplifies the .NET runtime codebase (removes 60k lines of code).

For more details, see the official docs issue at https://github.com/dotnet/docs/issues/18875. To see all breaking changes (in dotnet/runtime) in the release, check out the [.NET 5.0 breaking change query](https://github.com/dotnet/runtime/issues?q=is%3Aopen+is%3Aissue+label%3Abreaking-change+milestone%3A5.0).

## Platform support

We've updated our [.NET 5 - Supported OS versions](https://github.com/dotnet/core/blob/master/release-notes/5.0/5.0-supported-os.md) page to capture our latest plans for platform support for .NET 5.0. Please tell us what you think. What are we missing?

We realized that the package manager and container support we offer isn't listed on that page. That should be fixed. We plan to add that information before we release .NET 5.0.

## Closing

We're now past the half-way point in this release cycle. In fact, we're starting to (in our parlance) "close down the release". If you are watching our repos closely, you'll see that we're starting to [manage the milestones of issues](https://github.com/dotnet/runtime/issues/38286) more carefully. Having worked on multiple .NET releases now, I can tell you that this is a great time. It's time to claim victory on the set of features we've built, and to polish them to the point that you are happy using them. That's what we're doing  now, in our home workplaces.
