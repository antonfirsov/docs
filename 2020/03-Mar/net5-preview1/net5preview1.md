# Announcing .NET 5.0 Preview 1

At the end of last year, we shipped .NET Core 3.0 and 3.1. These versions added the desktop app models Windows Forms (WinForms) and WPF, ASP.NET Blazor for building single page applications and gRPC for cross-platform, contract-based messaging. We also added templates for building services, rich generation of client code for talking to gRPC, REST API services, and a lot more. We’re excited to see that .NET Core 3 has become the fastest adopted version of .NET ever and we’ve gained another million more users in just the last year. 
 
[We also communicated](https://devblogs.microsoft.com/dotnet/net-core-is-the-future-of-net/) with these releases that this would conclude the porting of the app models from .NET Framework. With .NET Core 3, we have ported all of the most used app models as well as introduced newer cross- platform frameworks to replace the ones we did not port. 
 
As we look forward to the next major release, .NET 5, we will continue to unify .NET into a single platform by including our .NET mobile device app model (Xamarin) in .NET 5. .NET 5 will include ASP.NET Core, Entity Framework Core, WinForms, WPF, Xamarin and ML.NET. For the first time, the entire platform will use a unified BCL (Base Class Libraries) for all the app models. Having a version 5 that is higher than both .NET Core and .NET Framework also makes it clear that .NET 5 is the future of .NET, which is a single unified platform for building any type of application. 
 
We have said this many times, but we will reiterate again; .NET Core and then .NET 5 is the .NET you should build all your NEW applications with. .NET Framework will remain supported as long as Windows itself is supported. We will continue to provide security and bug fixes and keep the networking and crypto API’s up to date. It will remain safe and supported to keep your older applications on .NET Framework. 

## Install .NET 5.0 Preview 1
 Today we are releasing the first preview of .NET 5, which is scheduled to GA (General Availability) later this year in November. 

* .NET 5.0 Preview 1 SDK
* .NET 5.0 Preview 1 Runtime

## High-level goals for .NET 5.0 

Let me highlight some of the high-level goals for .NET 5:

* Unified .NET SDK experience:
   * Single BCL (Base Class Library) across all .NET 5 applications. Today Xamarin applications use the Mono BCL but will move to use the.NET Core BCL, improving compatibility across our application models. 
   * Mobile development (Xamarin) is integrated into .NET 5. This means the .NET SDK will support mobile. For example, you can use “dotnet new XamarinForms” to create a mobile application.
* Native Applications supporting multiple platforms: Single Device project that supports an application that can work across multiple devices for example Window Desktop, Microsoft Duo (Android), and iOS using the native controls supported on those platforms.
* Web Applications supporting multiple platforms: Single Blazor project that supports an application that can work in browsers, on mobile devices and as a native desktop application (Windows 10x for example)
* Cloud Native Applications: High performance, single file (.exe) <50MB microservices and support for building multiple project (API, web front ends, containers) both locally and in the cloud.
* Continuous Improvements, such as: faster algorithms in the BCL, better support for containers in the runtime, support for HTTP3.

Today’s first preview does not contain all the work to support these high-level goals yet, but we will continue to announce more capabilities and features in future previews.

## Improvements in Preview 1

The following improvements are in Preview 1:

### Regular expression performance improvements

We've invested in significant improvements to the Regex engine. On many of the expressions we've tried, these improvements routinely result in throughput improvements of 3-6x, and in some cases, much more. We have a blog post coming shortly that will describe these improvements in much more detail.

### Code quality improvements in RyuJIT

Every release includes a set of performance improvements to the code that the JIT generates. We refer to these type of improvements as “CQ” or code quality. In most cases, these improvements also apply to the code generated for ready-to-run images.

The following improvements are in preview 1:
* [Improvements for null check folding](https://github.com/dotnet/runtime/pull/1735) – Remove the need to generate null checks in more cases by observing more patterns where null checks are provably unnecessary.
* [Tuned common subexpression evaluation (CSE)](https://github.com/dotnet/runtime/pull/1463) – The JIT looks for and collapses duplicate expressions that only need to be evaluated once (wiki).
* [Optimizing "constant_string".Length](https://github.com/dotnet/runtime/issues/5310) – Optimizing this pattern and collapsing the code down to the correct integer value.
* [JIT: build basic block pred lists before morph](https://github.com/dotnet/runtime/pull/1309) – Re-order phases in the JIT to allow key optimizations to be used earlier, resulting in better code quality and less work for the following phases, which increases JIT throughput (“TP” in the referenced PR).

### Assembly load diagnostics added to event pipe

We have added assembly load information to event pipe. This improvement is the start of making similar diagnostics functionality available as is part of .NET Framework with the [Fusion Log Viewer](https://docs.microsoft.com/dotnet/framework/tools/fuslogvw-exe-assembly-binding-log-viewer). You can now use [dotnet-trace]( https://docs.microsoft.com/dotnet/core/diagnostics/dotnet-trace) to collect this information, using the following command:

```console
dotnet-trace collect --providers Microsoft-Windows-DotNETRuntime:4:4 --process-id [process ID]
```

The workflow is described in [Trace Assembly Loading with Event Pipe]( https://github.com/richlander/testapps/tree/master/trace-assembly-loading). You can see assembly loading information for a simple test app.

![Trace assemblies -- loads](trace-assemblies-loads.png)
 
### Event pipe profiler APIs

Event pipe is a new subsystem and API that we [added in .NET Core 2.2](https://devblogs.microsoft.com/dotnet/announcing-net-core-2-2/) to make it possible to [perform performance and other diagnostic investigations](https://github.com/dotnet/runtime/blob/master/docs/project/linux-performance-tracing.md) on any operating system. In .NET 5.0, the event pipe has been extended to enable profilers to write event pipe events. This scenario is critical for instrumenting profilers that previously relied on ETW to monitor application behavior and performance.

### GitHub repo consolidation

As part of the .NET 5 release, we [reduced the number of GitHub repos](https://github.com/dotnet/runtime/issues/13688) we use to build and package .NET. Repo boundaries have a significant impact on many aspects of a project, including builds and issue management. With .NET Core 1.0, we had over 100 repos across ASP.NET, EF and .NET Core. With this latest release, we can now count the primary repos on one hand. We also moved nearly all repos to the dotnet org. 

Check out the new, consolidated, repos:

* [dotnet/runtime](https://github.com/dotnet/runtime) (was dotnet/corefx, dotnet/coreclr, and dotnet/core-setup)
* [dotnet/aspnetcore](https://github.com/dotnet/aspnetcore) (was several repos in the aspnet org)
* [dotnet/sdk](https://github.com/dotnet/sdk) (was dotnet/sdk, dotnet/cli)

## Closing

We hope that you are excited about the work that is happening with .NET 5! The best way to prepare for .NET 5 is to move all of our .NET Core applications to 3.1- we will make the transition from .NET Core 3.1 to .NET 5 as painless as possible. And if you are still building applications on .NET Framework, please feel safe leaving those applications on .NET Framework but think of using .NET Core 3.1 for all your new applications. There are lots of exciting things coming to .NET!
