# Announcing .NET Core 3.0 Preview 6

Today, we are announcing [.NET Core 3.0 Preview 6](https://dotnet.microsoft.com/download/dotnet-core/3.0). It includes updates for compiling assemblies for improved startup, optimizing applications for size with linker and EventPipe improvements. We've also released new Docker images for Alpine on ARM64.

[Download .NET Core 3.0 Preview 6](https://dotnet.microsoft.com/download/dotnet-core/3.0) right now on Windows, macOS and Linux.

Release notes have been published at dotnet/core. An [API diff between Preview 5 and 6](https://github.com/dotnet/core/blob/master/release-notes/3.0/preview/api-diff/preview6/3.0-preview6.md) is also available.

ASP.NET Core and EF Core are also releasing updates today.

If you missed it, check out the improvements we released in [.NET Core 3.0 Preview 5](https://devblogs.microsoft.com/dotnet/announcing-net-core-3-0-preview-5/), from last month.


## WPF and Windows Forms update

The WPF team has now completed [publishing most of the WPF codebase to GitHub](https://github.com/dotnet/wpf/issues/738). In fact, they just [published source for fifteen assemblies](https://github.com/dotnet/wpf/pull/720). For anyone familiar with WPF, the assembly names should be very familiar.

In some cases, tests are still on the backlog to get published at or before 3.0 GA. That said, the presence of all of this code should enable the WPF community to fully participate in making changes across WPF. It is obvious from reading some of the GitHub issues that the community has its own backlog that it has been waiting to realize. Dark theme, maybe?

## Alpine Docker images

Docker images are now available for both .NET Core and ASP.NET Core on ARM64. They were previously only available for x64.

The following images can be used in a `Dockerfile`, or with `docker pull`, as demonstrated below:

* `docker pull mcr.microsoft.com/dotnet/core/runtime:3.0-alpine-arm64v8`
* `docker pull mcr.microsoft.com/dotnet/core/aspnet:3.0-alpine-arm64v8`

## Event Pipe improvements

Event Pipe now supports multiple sessions. This means that you can consume events with EventListener in-proc and simultaneously have out-of-process event pipe clients.

New Perf Counters added:

* % Time in GC
* Gen 0 Heap Size
* Gen 1 Heap Size
* Gen 2 Heap Size
* LOH Heap Size
* Allocation Rate
* Number of assemblies loaded
* Number of ThreadPool Threads
* Monitor Lock Contention Rate
* ThreadPool Work Items Queue
* ThreadPool Completed Work Items Rate

Profiler attach is now implemented using the same Event Pipe infrastructure.

See [Playing with counters](https://twitter.com/davidfowl/status/1135355693634949121) from David Fowler to get an idea of what you can do with event pipe to perform your own performance investigations or just monitor application status.

See [dotnet-counters](https://github.com/dotnet/diagnostics/blob/master/documentation/dotnet-counters-instructions.md) to install the dotnet-counters tool.

## Optimize your .NET Core apps with ReadyToRun images

You can improve the startup time of your .NET Core application by compiling your application assemblies as ReadyToRun (R2R) format. R2R is a form of ahead-of-time (AOT) compilation. 

R2R binaries improve startup performance by reducing the amount of work the JIT needs to do as your application is loading. The binaries contain similar native code as what the JIT would produce, giving the JIT a bit of a vacation when performance matters most (at startup). R2R binaries are larger because they contain both intermediate language (IL) code, which is still needed for some scenarios, and the native version of the same code, to improve startup.

R2R is supported with .NET Core 3.0. It cannot be used with earlier versions of .NET Core.

### Sample performance numbers
The following are performance numbers collected using a [sample WPF application](https://github.com/ridomin/msix-catalog). The application was published as self-contained and did not use the assembly linker (covered later this post).

IL-only Application:

* Startup time: 1.9 seconds
* Memory usage: 69.1 MB
* Application size: 150 MB

With ReadyToRun images:

* Startup time: 1.3 seconds.
* Memory usage: 55.7 MB
* Application size: 156 MB

## ReadyToRun images, explained

You can R2R compile both libraries and application binaries. At present, libraries can only be R2R compiled as part of an application, not for delivery as a NuGet package. We'd like more feedback on whether that scenario is important.

AOT compiling assemblies has been available as a concept with .NET for a long time, going back to the [.NET Framework and NGEN](https://docs.microsoft.com/dotnet/framework/tools/ngen-exe-native-image-generator). NGEN has a key drawback, which is that compilation must be done on client machines, using the NGEN tool. It isn't possible to generate NGEN images as part of your application build.

Enter .NET Core. It comes with [crossgen](https://github.com/dotnet/coreclr/blob/master/Documentation/building/crossgen.md), which produces native images in a newer format called [ReadyToRun](https://github.com/dotnet/coreclr/blob/master/Documentation/botr/readytorun-overview.md). The name describes its primary value proposition, which is that these native images can be built as part of your build and are "ready to run" without any additional work on client machines. That's a major improvement, and also an important win for climate change.

In terms of compatibility, ReadyToRun images are similar to IL assemblies, with some key differences. 

* IL assemblies contain just [IL code](https://en.wikipedia.org/wiki/Common_Intermediate_Language). They can run on any runtime that supports the given target framework for that assembly. For example a `netstandard2.0` assembly can run on .NET Framework 4.6+ and .NET Core 2.0+, on any supported operating system (Windows, macOS, Linux) and architecture (Intel, ARM, 32-bit, 64-bit).
* R2R assemblies contain IL and native code. They are compiled for a specific minimum .NET Core runtime version and runtime environment (RID). For example, a `netstandard2.0` assembly might be R2R compiled for .NET Core 3.0 and Linux x64. It will only be usable in that or a compatible configuration (like .NET Core 3.1 or .NET Core 5.0, on Linux x64), because it contains native code that is only usable in that runtime environment.

### Instructions

The ReadyToRun compilation is a publish-only, opt-in feature. We’ve released a preview version of it with .NET Core 3.0 Preview 5.

To enable the ReadyToRun compilation, you have to:

* Set the `PublishReadyToRun` property to `true`.
* Publish using an explicit `RuntimeIdentifier`.

Note: When the application assemblies get compiled, the native code produced is platform and architecture specific (which is why you have to specify a valid RuntimeIdentifier when publishing).

Here’s an example:

 
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>netcoreapp3.0</TargetFramework>
    <PublishReadyToRun>true</PublishReadyToRun>
  </PropertyGroup>
</Project>
```
 
And publish using the following command:

```console
dotnet publish -r win-x64 -c Release
```

Note: The `RuntimeIdentifier` can also be set in the project file. 

Note: ReadyToRun is currently only supported for [self-contained apps](https://docs.microsoft.com/dotnet/core/deploying/). It will be enabled for [framework-dependent apps](https://docs.microsoft.com/dotnet/core/deploying/) in a later preview.
 
Native symbol generation can be enabled by setting the `PublishReadyToRunEmitSymbols` property to `true` in your project. You do not need to generate native symbols for debugging purposes. These symbols are only useful for profiling purposes.

The SDK currently supports a way to exclude certain assemblies from being compiled into ReadyToRun images. This could be useful for cases when certain assemblies do not really need to be optimized for performance. This can help reduce the size of the application. It could also be a useful workaround for cases where the ReadyToRun compiler fails to compile a certain assembly. Exclusion is done using the PublishReadyToRunExclude item group. Example:

 
```xml
<ItemGroup>
  <PublishReadyToRunExclude Include="FilenameOfAssemblyToExclude.dll" />
</ItemGroup>
```
 
## Cross platform/architecture compilations

The ReadyToRun compiler doesn't currently support cross-targeting. You need to compile on a given target. For example, if you want R2R images for Windows x64, you need to run the publish command on that environment.

Exceptions to this:

* Windows x64 can be used to compiles Windows ARM32, ARM64, and x86 images.
* Windows x86 can be used to compile Windows ARM32 images.
* Linux x64 can be used to compile Linux ARM32 and ARM64 images.

## Assembly linking

The .NET core 3.0 SDK comes with a tool that can reduce the size of apps by analyzing IL and trimming unused assemblies.

With .NET Core, it has always been possible to publish self-contained apps that include everything needed to run your code, without requiring .NET to be installed on the deployment target. In some cases, the app only requires a small subset of the framework to function and could potentially be made much smaller by including only the used libraries.

We use the [IL linker](https://github.com/mono/linker) to scan the IL of your application to detect which code is actually required, and then trim unused framework libraries. This can significantly reduce the size of some apps. Typically, small tool-like console apps benefit the most as they tend to use fairly small subsets of the framework and are usually more amenable to trimming.

To use this tool, set `PublishTrimmed=true` in your project and publish a self-contained app:

```
dotnet publish -r <rid> -c Release
```

The publish output will include a subset of the framework libraries, depending on what the application code calls. For a helloworld app, the linker reduces the size from ~68MB to ~28MB.

Applications or frameworks (including ASP.NET Core and WPF) that use reflection or related dynamic features will often break when trimmed, because the linker doesn't know about this dynamic behavior and usually can't determine which framework types will be required for reflection at run time. To trim such apps, you need to tell the linker about any types needed by reflection in your code, and in any packages or frameworks that you depend on. Be sure to test your apps after trimming.

For more information about the IL Linker, see the [documentation](https://aka.ms/dotnet-illink), or visit the [mono/linker]( https://github.com/mono/linker) repo.

Note: In previous versions of .NET Core, [ILLink.Tasks](https://dotnet.myget.org/feed/dotnet-core/package/nuget/Illink.Tasks) was shipped as an external NuGet package and provided much of the same functionality. It is no longer supported - please update to the latest 3.0 SDK and try the new experience!

## Using the Linker and ReadToRun Together

The linker and ReadyToRun compiler can be used for the same application. In general, the linker makes your application smaller, and then the ready-to-run compiler will make it a bit larger again, but with a significant performance win. It is worth testing in various configurations to understand the impact of each option.

Note: [dotnet/sdk #3257](https://github.com/dotnet/sdk/issues/3257) prevents the linker and ReadyToRun from being used together for WPF and Windows Forms applications. We are working on fixing that as part of the .NET Core 3.0 release.

## Native Hosting sample

The team recently posted a [Native Hosting sample](https://github.com/dotnet/samples/tree/master/core/hosting/HostWithHostFxr). It demonstrates a best practice approach for hosting .NET Core in a native application.

As part of .NET Core 3.0, we now expose general functionality to .NET Core native hosts that was previously only available to .NET Core managed applications through the officially provided .NET Core hosts. The functionality is primarily related to assembly loading. This functionality should make it easier to produce native hosts that can take advantage of the full feature set of .NET Core.

## Closing

Please try out the new features. Please file issues for the bugs or any challenging experiences you find. We want the feedback! You can file feature requests, too, but they likely will need to wait to get implemented until the next release at this point.

We are now getting very close to being feature complete for .NET Core 3.0, and are now transitioning the focus of the team to the quality of the release. We've got a few months of bug fixing and performance work ahead. We'll appreciate your feedback as we work through that process, too.

On that note, we will soon be switching the `master` branches on .NET Core repos to the next major release, likely at or shortly after the Preview 7 release (July).

Thanks for trying out .NET Core 3.0 previews. We appreciate your help. At this point, we're focused on getting a final release in your hands.
