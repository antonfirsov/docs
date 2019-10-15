# Announcing .NET Core 3.1 Preview 1

Today, we're announcing [.NET Core 3.1 Preview 1](https://dotnet.microsoft.com/download/dotnet-core/3.1). .NET Core 3.1 will be a small release focused on key improvements in Blazor and Windows desktop, the two big additions in [.NET Core 3.0.](https://devblogs.microsoft.com/dotnet/announcing-net-core-3-0/). It will be a long term support (LTS) release with an expected final ship date of December 2019.

You can [download .NET Core 3.1 Preview 1](https://dotnet.microsoft.com/download/dotnet-core/3.1) on Windows, macOS, and Linux.

* [.NET Core 3.0 SDK and Runtime](https://dotnet.microsoft.com/download/dotnet-core/3.1)
* [Docker images](https://hub.docker.com/_/microsoft-dotnet-core)

ASP.NET Core and EF Core are also releasing updates today.

Visual Studio 16.4 Preview 2 and Visual Studio for Mac 8.4 Preview 2 are also releasing today. They are required updates to use .NET Core 3.1 Preview 1. Visual Studio 16.4 includes .NET Core 3.1, so just updating Visual Studio will give you both releases.

Details:

* [.NET Core 3.1 release notes](https://github.com/dotnet/core/tree/master/release-notes/3.1)
* [API diff](https://github.com/dotnet/core/blob/master/release-notes/3.1/preview/api-diff/preview1/3.0-preview8.md)
* [GitHub release](https://github.com/dotnet/core/releases/tag/v3.1.0-preview1)

## Improvements

There are several targeted improvements planned for .NET Core 3.1. The following improvements are not available yet, but are expected in later previews.

In .NET Core 3.0, the [.NET Core Desktop Runtime Installer](https://dotnet.microsoft.com/download/dotnet-core/3.0/runtime) (includes WPF and Windows Forms) does not install the .NET Core Runtime (includes CoreFX and CoreCLR) for you. We will be changing that, so that the Desktop Runtime installer is self-sufficient. The Runtime and Hosting Bundle, for server scenarios, already works this way.

Managed C++ was a promised component of the .NET Core 3.0 release. It was delivered, given that it is a required dependency of WPF, but a developer experence in Visual Studio and the SDK was missing. We will be delivering that experience with .NET Core 3.1 and Visual Studio 16.4. Managed C++ is only supported on Windows.

macOS 10.15 Catalina includes a new security requirement, that applications must be [notarized](https://developer.apple.com/documentation/xcode/notarizing_your_app_before_distribution). We will be satisfying these requirements for the .NET Core SDK, for .NET Core 3.1 and all other supported .NET Core releases. If you are using .NET Core to deliver macOS applications, we would appreciate working with you on notarization requirements.

## Closing

The primary goal of .NET Core 3.1 is to polish the features and scenarios we delivered in .NET Core 3.0. .NET Core 3.1 will be a [long term support (LTS)](https://dotnet.microsoft.com/platform/support/policy/dotnet-core) release, supported for at least 3 years.

Please install and test .NET Core 3.1 Preview 1 and give us feedback. It is not yet supported or recommended for use in production.

If you missed it, check out the [.NET Core 3.0 announcement](https://devblogs.microsoft.com/dotnet/announcing-net-core-3-0-preview-7/) from last month.
