# Announcing .NET 5.0 Preview 5

Today, we're releasing .NET 5.0 Preview 5. It contains a small set of new features and performance improvements.  The [.NET 5.0 Preview 4 post](https://devblogs.microsoft.com/dotnet/announcing-net-5-preview-4-and-our-journey-to-one-net/) covers what we are planning to deliver with .NET 5.0. Most of the features are now in the product, but many are not yet in their final state. We expect that the release will be very close to feature-complete by Preview 7.

You can [download .NET 5.0 Preview 5](https://dotnet.microsoft.com/download/dotnet/5.0), for Windows, macOS, and Linux:

* [Windows and macOS installers](https://dotnet.microsoft.com/download/dotnet/5.0)
* [Binaries](https://dotnet.microsoft.com/download/dotnet/5.0)
* [Docker images](https://hub.docker.com/_/microsoft-dotnet)
* [Snap installer](https://snapcraft.io/dotnet-sdk)

ASP.NET Core and EF Core are also being released today.

You need to use Visual Studio 2019 16.7 to use .NET 5.0. Install the latest version of the [C# extension](https://code.visualstudio.com/Docs/languages/csharp), to use .NET 5.0 with Visual Studio Code. .NET 5.0 isn't yet supported with Visual Studio for Mac.

Release notes:

* .NET 5.0 release notes
* .NET 5.0 known issues
* GitHub release
* GitHub tracking issue

## Following the release

It can be very hard to follow what the team is doing on GitHub, both in terms of specific features you might be interested in and understanding what the larger improvements are going to be in the next release. Even as the release blog writer, I find this difficult. To fix this problem, we put together a [.NET 5.0 Runtime epics](https://github.com/dotnet/runtime/issues/37269) issue that you can use to navigate the big investments and themes in the release. 

We consider an epic to be a collection of features that together form a step-function level improvement in .NET. If someone ever asks you "what's in .NET 5.0?" or "is there anything in .NET 5.0 that we care about?", this list of epics is a good place to start. However, it's important to understand that there are many features that aren't part of an epic and aren't captured by this issue.

Do you like these "epic" issues? Would you like to see this pattern used in more dotnet org repos?

## RyuJIT improvements

The following improvements were made to the RyuJIT JIT compiler:

* [New, much faster, portable implementation of tailcall helpers](https://github.com/dotnet/runtime/pull/341). Credit: [Jakob Botsch Nielsen](https://github.com/jakobbotsch) (.NET team intern).
* Continued ARM64 hardware intrinsics implementation progress
    * [Implement ASIMD Extract Insert ExtractVector64 ExtractVector128](https://github.com/dotnet/runtime/pull/35030) 
    * [Implement ASIMD widening, narrowing, saturating intrinsics](https://github.com/dotnet/runtime/pull/35612)
    * [Add VectorTableList and TableVectorExtension intrinsics](https://github.com/dotnet/runtime/pull/35600) -- Credit: [@TamarChristinaArm](https://github.com/TamarChristinaArm) (ARM Holdings)
    * [Add support of CreateScalarUnsafe() for arm64 intrinsic](https://github.com/dotnet/runtime/pull/34579)
    * [ARM64 intrinsic support for Vector64.Create() and Vector128.Create()](https://github.com/dotnet/runtime/pull/35590)
    * [Optimize BitOperations.PopCount() with arm64 intrinsics](https://github.com/dotnet/runtime/pull/35636)
* [Improved JIT speed in a case that was affecting regular expression compilation](https://github.com/dotnet/runtime/pull/35352)
* [Improved Intel architecture performance using new hardware intrinsics BSF/BSR](https://github.com/dotnet/runtime/pull/34550) -- Credit [@saucecontrol](https://github.com/saucecontrol)
* [Implement Vector{Size}<T>.AllBitsSet](https://github.com/dotnet/runtime/pull/33924)  -- Credit [@Gnbrkm41](https://github.com/Gnbrkm41)

## Native exports

We've had requests to enable exports for native binaries that calls into .NET code for a long time. It's a great scenario, and we're now enabling it with .NET 5.0. The building block of the feature is [hosting API support](https://github.com/dotnet/runtime/blob/3247a54a4263dc2a492b740223b6f062672f70d7/src/installer/corehost/cli/coreclr_delegates.h#L22) for [UnmanagedCallersOnlyAttribute](https://github.com/dotnet/runtime/blob/master/src/libraries/System.Private.CoreLib/src/System/Runtime/InteropServices/UnmanagedCallersOnlyAttribute.cs). 

This feature is a building-block for creating higher level experiences. [Aaron Robinson](https://github.com/AaronRobinsonMSFT), on our team, has been working on a [.NET Native Exports](https://github.com/AaronRobinsonMSFT/DNNE) project that provides a more complete experience for publishing .NET components as native libraries. We're looking for feedback on this capability to help decide if the approach should be included in the product.

The .NET Native exports project enables you to:

* Expose custom native exports.
* Doesn't require a higher-level interop technology like COM.
* Works cross-platform.

There are existing projects that enable similar scenarios, such as:

* [Unmanaged Exports](https://sites.google.com/site/robertgiesecke/Home/uploads/unmanagedexports)
* [DllExport](https://github.com/3F/DllExport)

## [Breaking change] Removal of built-in WinRT support in .NET 5.0  

Note: This change is coming in Preview 6. This is an early announcement.

[Windows Runtime (WinRT)](https://blogs.windows.com/windowsdeveloper/2019/04/30/calling-windows-10-apis-from-a-desktop-application-just-got-easier/) is the technology and [ABI](https://en.wikipedia.org/wiki/Application_binary_interface) that new APIs are exposed with in Windows. You can call those APIs via .NET code, similar to how you would with C++. Support for WinRT interop was added in .NET Core 3.0, as part of adding support for Windows desktop client frameworks (Windows Forms and WPF). 

More recently, we've been working closely with the Windows team to change and improve the way that WinRT interop works with .NET. We have replaced the built-in WinRT support with the [C#/WinRT](https://docs.microsoft.com/windows/uwp/csharp-winrt/) tool chain, provided by the Windows team, in .NET 5.0. This [change in WinRT interop is a breaking change](https://github.com/dotnet/runtime/issues/37672), and .NET Core 3.x apps that use WinRT will need to be recompiled. We will provide more infromation on this in coming previews.

The benefits are called out in [Support WinRT APIs in .NET 5](https://github.com/dotnet/runtime/issues/35318):

* WinRT interop can be developed and improved separate from the .NET runtime.
* Makes WinRT interop symmetrical with interop systems provided for other operating systems, like iOS and Android.
* Can take advantage of many other .NET features (AOT, C# features, IL linking).
* Simplifies the .NET runtime codebase (removes 60k lines of code).

For more details, see the official docs issue at https://github.com/dotnet/docs/issues/18875. To see all breaking changes (in dotnet/runtime) in the release, check out the [.NET 5.0 breaking change query](https://github.com/dotnet/runtime/issues?q=is%3Aopen+is%3Aissue+label%3Abreaking-change+milestone%3A5.0).

## Expanding System.DirectoryServices.Protocols to Linux and macOS

We've been adding cross-platform support for [System.DirectoryServices.Protocols](https://docs.microsoft.com/dotnet/api/system.directoryservices.protocols). In Preview 5, we've added [support for Linux](https://github.com/dotnet/runtime/pull/35380) and we'll add [support for macOS](https://github.com/dotnet/runtime/pull/36669) in Preview 6. Windows support was pre-existing.

[System.DirectoryServices.Protocols](https://docs.microsoft.com/dotnet/api/system.directoryservices.protocols) is a lower-level API than [System.DirectoryServices](https://docs.microsoft.com/dotnet/api/system.directoryservices), and enables (or can be used to enable) more scenarios. System.DirectoryServices includes Windows-only concepts/implementations, so it was not an obvious choice to make cross-platform. Both API-sets enable controlling and interacting with a directory service server, like [LDAP](https://en.wikipedia.org/wiki/Lightweight_Directory_Access_Protocol) or [Active Directory](https://en.wikipedia.org/wiki/Active_Directory).

## Closing
