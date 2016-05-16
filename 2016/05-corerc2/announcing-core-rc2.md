Announcing .NET Core RC2
========================

Today, we are announcing the release of .NET Core RC2. You can use it to build ASP.NET Core and console apps for Windows, OS X and Linux. RC2 is a major update from the November [RC1 release](https://blogs.msdn.microsoft.com/dotnet/2015/11/18/announcing-net-core-and-asp-net-5-rc/), including new APIs, performance and reliability improvements and a new set of tools.

You can [install .NET Core 1.0 RC2](http://dot.net/core) now, on Windows, OS X and Linux. 

You can use .NET Core RC2 with a variety of editors and IDEs:

- In [Visual Studio 2015 Update 2](https://www.visualstudio.com/products/visual-studio-community-vs)
- In [Visual Studio Code](https://www.visualstudio.com/products/code-vs) with the [C# extension](https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp)
- Eventually, in your favorite [Omnisharp-enabled editor](http://www.omnisharp.net/)

You can deploy .NET Core RC2 apps:

- To Azure Websites
- On RedHat Enterprise Linux (using Software Collections)
- With Docker

Releases
========

There are multiple releases today:

- .NET Core RC2
- [ASP.NET Core RC2](https://blogs.msdn.microsoft.com/webdev/)
- .NET Core SDK Preview 1

We made major changes to the .NET Core SDK, formally called DNX, since RC1. The change was significant enough and not complete at RC2 that we opted to call that part of the release "Preview". That may sound like a concern, however, the SDK is typically only used at development time, not in production. 

.NET Core and ASP.NET Core have improved significantly since RC1. We've added features and improved performance and reliability. RC1 was "Go Live" and so is RC2. "Go Live" means you can call Microsoft Support for help with issues.

We've been working directly with a number of customers who are hosting RC1 in production today, on Windows and Linux. Thanks for taking a bet on RC1! RC2 will be even better for you.

Platform Support
================

We've been adding support for a growing number of operating systems. We started out the project with a plan to support .NET Core on Windows, OS X and a vague definition of "Linux". Close watchers of the [coreclr](https://github.com/dotnet/coreclr#build-status) and [corefx](https://github.com/dotnet/corefx#build--test-status) projects will have noticed that the platform support has been growing steadily. .NET Core RC2 is supported on the following platforms.

- Red Hat Enterprise Linux 7.2
- Centos 7.1
- Debian 8.2 (8.2, 8.3, 8.4)+
- Ubuntu 14.04 (16.04 support is coming at RTM)
- OS X 10.11
- Windows 7+ / Server 2012 R2+
- Windows Nano Server TP5

.NET Core RC2 will soon be showing up in the Red Hat Enterprise Linux software collection. You can install it with yum, following these instructions.

Ubuntu was the first distro that we supported. We heard feedback that it made more sense to start with Debian, given that it is the parent of Ubuntu and many more distros. More recently, we added support for Debian, enabling .NET Core to be used in a larger set of Debian-based distros. 

We intend .NET Core to be an open and hackable development platform. We'll publish instructions in the next couple weeks on how to test .NET Core on arbitrary distros. You can see how [Linux Mint is supported in runtimes.json](https://github.com/dotnet/corefx/blob/master/pkg/Microsoft.NETCore.Platforms/runtime.json#L323), for example.  

.NET Core Tools
===============

You typically start .NET Core development by installing the .NET Core SDK. The SDK includes enough software to build an app. The SDK gives you both the .NET Core Tools and a copy of .NET Core. As new versions of .NET Core are made available, you can download and install them, without needing to get a new version of the tools. 

Apps specify their dependence on a particular .NET Core version via the project.json project file. The tools help you acquire and use that .NET Core version. You can switch between multiple apps on your machine in Visual Studio, Visual Studio Code or at a command prompt and the .NET Core tools will always pick the right version of .NET Core to use.

You can also have multiple versions of the .NET Core tools on your machine, too, which is important for continuous integration and other scenarios. Most of the time, you will just have one copy of the tools, since doing so provides a simpler experience.

The `dotnet` Tool
-----------------

Your .NET Core experience will start with the `dotnet` tool. It exposes a set of commands for  common operations, including restoring packages, building your project and unit testing. It also includes a command to create an empty new project to make it easy to get started.

There are many tools that come with the .NET Core Tools, to enable important development scenarios. You won't see most of them since they all expose themselves through the `dotnet` tool. `dotnet` has a very simple extension model, so it's also easy to add more commands as needed.

The following list provides a partial list of the [commands](http://dotnet.github.io/docs/core-concepts/core-sdk/index.html).

- `dotnet new` - Initializes a sample console, class library or unit test C# project.
- `dotnet restore` - Restores the dependencies for a given application.
- `dotnet build` - Builds a .NET Core application.
- `dotnet publish` - Publishes a .NET portable or self-contained application.
- `dotnet run` - Runs the application from source.
- `dotnet test` - Runs tests using a test runner specified in the project.json.
- `dotnet pack` - Create a NuGet package of your code.

Comparison to DNX
-----------------

We learned a lot from our experience building [DNX](https://blogs.msdn.microsoft.com/dotnet/2015/11/18/announcing-net-core-and-asp-net-5-rc/), with RC1 and prior .NET Core releases. DNX was actually three things at once: 

- a set of tools
- a set of framework libraries
- a set of services available to apps

The tools part of DNX lines up best with the .NET Core Tools that are part of today's release.

DNX was great if you wanted all three of those things, but could be a problem if you only wanted one or two of them. This problem become obvious to us as we experimented with the [corert](https://github.com/dotnet/corert) native compilation project, which required a different set of tools than DNX provided.

DNX also relied on environment variables to set an "in use" version. That made it hard to use multiple .NET Core apps from the same command prompt.

Those challenges provided us with a good idea of what we needed to resolve for RC2.

The `dotnet` tool replaces the `dnx` and `dnu` tools that came with RC1. The `dnvm` tool doesn't have a replacement yet. That's something that might come in a later release.

.NET Core Apps 
==============

We've talked to many customers about how they want to deploy apps. We heard two main models:

- Deploy smaller apps that have a dependency on a centrally installed .NET Core version, perhaps used by multiple apps.
- Deploy larger self-contained apps that have no .NET dependencies.

Both of these [app deployment models](http://dotnet.github.io/docs/core-concepts/app-types.html) are supported and are a good choice, depending on the scenario. As of RC2, we have focussed most on the first scenario. We will continue to improve both scenarios.

Portable Apps
-------------

Portable applications are the default type in .NET Core. They require .NET Core to be installed on the targeted machine in order for them to run. This means that your application is portable between installations of .NET Core, including on multiple OSes.

This type of application will only carry its own code and dependencies that are outside of .NET Core libraries. As long as .NET Core is installed on a given machine, the app will typically work. You do not need to decide upfront which OSes your app will run on.

Self-contained Apps
-------------------

Self-contained applications contain all of their app dependencies, including the .NET Core runtime, as the application. This makes the app larger, but also makes it capable of running on any .NET Core supported platforms with the correct native dependencies, whether it has .NET Core installed or not. This makes it that much easier to deploy to the target machine, since you only deploy your application.

Since the application carries the runtime, you need to make an explicit choice which platforms your application needs to run on. For instance, if you publish a self-contained application for Windows 10, that same application will not work on OS X or Linux and vice versa. Of course, you can add or remove platforms during development at any given time.

Framework APIs
==============


