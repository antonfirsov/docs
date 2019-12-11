# Announcing .NET Core 3.1

We’re excited to announce the release of .NET Core 3.1. It's really just a small set of fixes and refinements over [.NET Core 3.0](https://devblogs.microsoft.com/dotnet/announcing-net-core-3-0/), which we released just over two months ago. The most important feature is that .NET Core 3.1 is an [long-term supported (LTS) release](https://dotnet.microsoft.com/platform/support/policy/dotnet-core) and will be supported for three years. As we've done in the past, we wanted to take our time before releasing the next LTS release. The extra two months (after .NET Core 3.0) allowed us to select and implement the right set of improvements over what was already a very stable base. .NET Core 3.1 is now ready to be used wherever your imagination or business need takes it.

You can [download .NET Core 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1), for Windows, macOS, and Linux:

* [.NET Core 3.1 SDK and Runtime](https://dotnet.microsoft.com/download/dotnet-core/3.1)
* [Docker container images](https://hub.docker.com/_/microsoft-dotnet-core)
* [Snap installer](https://snapcraft.io/dotnet-sdk)

ASP.NET Core and EF Core are also being released today.

Visual Studio 2019 16.4 was also released today and includes .NET Core 3.1. It is a required update to use .NET Core 3.1 with Visual Studio. For Visual Studio 2019 users, we recommend simply updating Visual Studio to 16.4 and instead of separately downloading .NET Core 3.1.

Visual Studio for Mac also supports and includes .NET Core 3.1, in the Visual Studio for Mac 8.4 Preview channel. You will need to opt into the Preview channel to use .NET Core 3.1.

Release notes:

* .NET Core 3.0 release notes
* .NET Core 2.2 -> 3.0 API diff
* .NET Core 3.0 contributor list
* GitHub release
* GitHub issue for .NET Core 3.0 issues

The changes in .NET Core 3.1 were primarily focussed on Blazor and [Windows Desktop](https://devblogs.microsoft.com/dotnet/net-core-3-for-windows-desktop/), the two new and large additions in .NET Core 3.0. This includes support for C++/CLI, which has been a regular request for developers targeting Windows.

Before we take a look at what's new in .NET Core 3.1, let's take a quick look at the key improvements in [.NET Core 3.0](https://devblogs.microsoft.com/dotnet/announcing-net-core-3-0/), which is the bulk of what's important to consider for .NET Core 3.1.

## Recap of .NET Core 3.0 Improvements

The following key improvements were delivered in .NET Core 3.0. We've already heard from [developers of big sites](https://twitter.com/Nick_Craver/status/1190217625017696256) that it is working super well for them.

* **.NET Core 3.0 is already battle-tested** by being hosted for months at [dot.net](https://dotnet.microsoft.com/) and on [Bing.com](https://www.bing.com/version). Many other Microsoft teams will soon be deploying large workloads on .NET Core 3.1 in production.
* **Performance is greatly improved** across many components and is described in detail at [Performance Improvements in .NET Core 3.0](https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-core-3-0/) and [Hardware Intrinsics in .NET Core](https://devblogs.microsoft.com/dotnet/hardware-intrinsics-in-net-core/).
* **C# 8** add async streams, range/index, [more patterns](https://devblogs.microsoft.com/dotnet/do-more-with-patterns-in-c-8-0/), and [nullable reference types](https://devblogs.microsoft.com/dotnet/embracing-nullable-reference-types/). Nullable enables you to directly target the flaws in code that lead to `NullReferenceException`. The lowest layer of the framework libraries has been annotated, so that you know when to expect `null`.
* **F# 4.7** focuses on making some thing easier with implicit `yield` expressions and some syntax relaxations. It also includes support for `LangVersion`, and ships with `nameof` and opening of static classes in preview. The F# Core Library now also targets .NET Standard 2.0. You can read more at [Announcing F# 4.7](https://devblogs.microsoft.com/dotnet/announcing-f-4-7/).
* **.NET Standard 2.1** increases the set of types you can use in code that can be used woth both .NET Core and Xamarin.  [.NET Standard 2.1](https://devblogs.microsoft.com/dotnet/announcing-net-standard-2-1/) includes types since .NET Core 2.1.
* **Windows Desktop** apps are now supported with .NET Core, for both [Windows Forms](https://github.com/dotnet/winforms) and [WPF](https://github.com/dotnet/wpf) (and [open source](https://devblogs.microsoft.com/dotnet/announcing-net-core-3-preview-1-and-open-sourcing-windows-desktop-frameworks/)). The WPF designer is part of Visual Studio 2019. The [Windows Forms designer](https://devblogs.microsoft.com/dotnet/introducing-net-core-windows-forms-designer-preview-1/) is in preview and available as a download.
* **.NET Core apps now have executables** by default. In past releases, apps needed to be launched via the `dotnet` command, like `dotnet myapp.dll`. Apps can now be launched with an app-specific executable, like `myapp` or `./myapp`, depending on the operating system.
* **High performance JSON APIs** have been added, for reader/writer, object model and serialization scenarios. These APIs were built from scratch on top of `Span<T>` and use UTF8 under the covers instead of UTF16 (like `string`). These APIs minimize allocations, resulting in faster performance, and much less work for the garbage collector. See [Try the new System.Text.Json APIs](https://devblogs.microsoft.com/dotnet/try-the-new-system-text-json-apis/).
* **The garbage collector uses less memory** by default, often a lot less. This improvement is very beneficial for scenarios where many applications are hosted on the same server. The garbage collector has also been updated to make better use of large numbers of cores, on machines with >64 cores. See [Making CPU configuration better for GC on machines with > 64 CPUs](https://devblogs.microsoft.com/dotnet/making-cpu-configuration-better-for-gc-on-machines-with-64-cpus/).
* **.NET Core has been hardened for Docker** to enable .NET applications to work predictably and efficiently in containers. The garbage collector and thread pool have been updated to work much better when a container has been configured for limited memory or CPU. .NET Core docker images are smaller, particularly the SDK image. See: [Running with Server GC in a Small Container Scenario Part 0](https://devblogs.microsoft.com/dotnet/running-with-server-gc-in-a-small-container-scenario-part-0/), [Running with Server GC in a Small Container Scenario Part 1 – Hard Limit for the GC Heap](https://devblogs.microsoft.com/dotnet/running-with-server-gc-in-a-small-container-scenario-part-1-hard-limit-for-the-gc-heap/) and [Using .NET and Docker Together – DockerCon 2019 Update](https://devblogs.microsoft.com/dotnet/using-net-and-docker-together-dockercon-2019-update/).
* **Raspberry Pi and ARM chips** are now supported to enable IoT development, including with the remote Visual Studio debugger. You can deploy apps that listen to sensors, and print messages or images on a display, all using the new GPIO APIs. ASP.NET can be used to expose data as an API or as a site that enables configuring an IoT device.

## Platform support

.NET Core 3.1 is supported on the following operating systems:

* Alpine: 3.9+
* Debian: 9+
* openSUSE: 42.3+
* Fedora: 26+
* Ubuntu: 16.04+
* RHEL: 6+
* SLES: 12+
* macOS: 10.13+
* Windows Client: 7, 8.1, 10 (1607+)
* Windows Server: 2012 R2 SP1+

Note: Windows Forms and WPF apps are only functional and supported on Windows.

Chip support follows:

* x64 on Windows, macOS, and Linux
* x86 on Windows
* ARM32 on Windows and Linux
* ARM64 on Linux (kernel 4.14+)

Note: Please ensure that .NET Core 3.1 ARM64 deployments use Linux kernel 4.14 version or later. For example, Ubuntu 18.04 satisfies this requirement, but 16.04 does not.

## Windows Forms Controls Removal

The following Windows Forms controls have been removed from .NET Core 3.1:

* DataGrid
* ToolBar
* ContextMenu
* Menu
* MainMenu
* MenuItem

These controls were replaced with more powerful controls in .NET Framework 2.0, back in 2005. They have not been available by default in the Visual Studio Designer Toolbox for many years. As a result, we decided to remove these controls and focus only on the new ones.

The following replacements are recommended:

| Old Control (API) 	| Recommended Replacement                  	| Other associated APIs removed    	|
|-------------------	|------------------------------------------	|----------------------------------	|
| DataGrid          	| DataGridView                             	| DataGridCell, DataGridRow, DataGridTableCollection, DataGridColumnCollection, DataGridTableStyle, DataGridColumnStyle, DataGridLineStyle, DataGridParentRowsLabel, DataGridParentRowsLabelStyle, DataGridBoolColumn, DataGridTextBox, GridColumnStylesCollection, GridTableStylesCollection, HitTestType        	|
| ToolBar           	| ToolStrip                                	| ToolBarAppearance             	|
| ToolBarButton        	| ToolStripButton                          	| ToolBarButtonClickEventArgs, ToolBarButtonClickEventHandler, ToolBarButtonStyle, ToolBarTextAlign             	|
| ContextMenu       	| ContextMenuStrip                         	|                                  	|
| Menu              	| ToolStripDropDown, ToolstripDropDownMenu 	| MenuItemCollection              	|
| MainMenu          	| MenuStrip                                	|                                  	|
| MenuItem          	| ToolstripMenuItem                        	|                                  	|


Yes, this is an unfortunate breaking change. You will see build breaks if you are using the controls we removed in your applications. Also, if you open .NET Core 3.0 applications in the latest versions of the .NET Core Windows Forms designer, you will see errors if you are using these controls.

We recommend you update your applications to .NET Core 3.1 and move to the alternative controls. Replacing the controls is a straight-forward process, essentially "find and replace".

First, we should have made these changes before we released .NET Core 3.0, and we appologize for that. We try to avoid late changes, and even more for breaking changes, and it pains us to make this one.

As we got further into the Windows Forms designer project, we realized that these controls were not aligned with creating modern applications and should never have been part of the .NET Core port of Windows Forms. We also saw that they would require more time from us to support than made sense. 

Our goal is to continue to improve Windows Forms for high DPI, accessibility, and reliability, and this late change was required to enable us to focus on delivering that.

## C++/CLI

We added support for creating C++/CLI (AKA "managed C++") components that can be used with .NET Core 3.0+, in Visual Studio 2019 16.4. You need to install the “Desktop development with C++” workload and the “C++/CLI support” component in order to use C++/CLI.

This component adds a couple templates that you can use:

* CLR Class Library (.NET Core)
* CLR Empty Project (.NET Core)

If you cannot find them, just search for them in the New Project dialog.

C++/CLI is only enabled on Windows. You cannot use C++/CLI components targeted for .NET Framework with .NET Core or vice versa.

## Closing

We recommend moving to .NET Core 3.1 as soon as you can. It is a great release (largely due to 3.0) that brings improvements to so many aspects of .NET Core. It is also a [long term support (LTS) release](https://dotnet.microsoft.com/platform/support/policy/dotnet-core), and will be supported for three years.

Life cycle update:

* .NET Core 3.0 will reach end-of-life three months from today, on March 3, 2020. 
* .NET Core 2.2 will each end of life on December 23rd.
* .NET Core 2.1 will be supported until August 2021 (it is also an LTS release).

The following .NET Core posts are recommended reading to learn more about what you get with .NET Core 3.1 and other projects we've been working on.

**Fundamentals**

* [Floating-Point Parsing and Formatting improvements in .NET Core 3.0](https://devblogs.microsoft.com/dotnet/floating-point-parsing-and-formatting-improvements-in-net-core-3-0/)
* [Default implementations in interfaces](https://devblogs.microsoft.com/dotnet/default-implementations-in-interfaces/)
* [Introducing the new Microsoft.Data.SqlClient](https://devblogs.microsoft.com/dotnet/introducing-the-new-microsoftdatasqlclient/)
* [Introducing diagnostics improvements in .NET Core 3.0](https://devblogs.microsoft.com/dotnet/introducing-diagnostics-improvements-in-net-core-3-0/)
* [Understanding the Whys, Whats, and Whens of ValueTask](https://devblogs.microsoft.com/dotnet/understanding-the-whys-whats-and-whens-of-valuetask/)

**Desktop**

* [.NET Core 3 for Windows Desktop](https://devblogs.microsoft.com/dotnet/net-core-3-for-windows-desktop/)
* [Continuously deploy and monitor your UWP, WPF, and Windows Forms app with App Center](https://devblogs.microsoft.com/dotnet/continuously-deploy-and-monitor-your-uwp-wpf-and-windows-forms-app-with-app-center/)
* [Introducing .NET Core Windows Forms Designer Preview 1](https://devblogs.microsoft.com/dotnet/introducing-net-core-windows-forms-designer-preview-1/)
* [Migrating a Sample WPF App to .NET Core 3](https://devblogs.microsoft.com/dotnet/migrating-a-sample-wpf-app-to-net-core-3-part-1/)
* [Porting desktop apps to .NET Core](https://devblogs.microsoft.com/dotnet/porting-desktop-apps-to-net-core/)


**ASP.NET**

* [Improvements in .NET Core 3.0 for troubleshooting and monitoring distributed apps](https://devblogs.microsoft.com/aspnet/improvements-in-net-core-3-0-for-troubleshooting-and-monitoring-distributed-apps/)
* [Blazor Server in .NET Core 3.0 scenarios and performance](https://devblogs.microsoft.com/aspnet/blazor-server-in-net-core-3-0-scenarios-and-performance/)
* [Supporting the community with WF and WCF OSS projects](https://devblogs.microsoft.com/dotnet/supporting-the-community-with-wf-and-wcf-oss-projects/)
* [Upcoming SameSite Cookie Changes in ASP.NET and ASP.NET Core](https://devblogs.microsoft.com/aspnet/upcoming-samesite-cookie-changes-in-asp-net-and-asp-net-core/)
* [Re-reading ASP.Net Core request bodies with EnableBuffering()](https://devblogs.microsoft.com/aspnet/re-reading-asp-net-core-request-bodies-with-enablebuffering/)
* [gRPC vs HTTP APIs](https://devblogs.microsoft.com/aspnet/grpc-vs-http-apis/)
* [Collecting .NET Core Linux Container CPU Traces from a Sidecar Container](https://devblogs.microsoft.com/dotnet/collecting-net-core-linux-container-cpu-traces-from-a-sidecar-container/)

**General**

* [How the .NET Team uses Azure Pipelines to produce Docker Images](https://devblogs.microsoft.com/dotnet/how-the-net-team-uses-azure-pipelines-to-produce-docker-images/)
* [The Evolving Infrastructure of .NET Core](https://devblogs.microsoft.com/dotnet/the-evolving-infrastructure-of-net-core/)
* [Update on .NET Standard adoption](https://devblogs.microsoft.com/dotnet/update-on-net-standard-adoption/)
* [.NET Core is the Future of .NET ](https://devblogs.microsoft.com/dotnet/net-core-is-the-future-of-net/)
* [.NET Core and systemd](https://devblogs.microsoft.com/dotnet/net-core-and-systemd/)
* [.NET Core Workers as Windows Services](https://devblogs.microsoft.com/aspnet/net-core-workers-as-windows-services/)
