# Announcing .NET 5 Preview 4 and our journey to one .NET

.NET 5 is the next version and future of .NET. We are continuing the journey of unifying the .NET platform, with a single framework that extends from cloud to desktop to mobile and beyond. Looking back, we took the best of .NET Framework and put that into .NET Core 3, including support for WPF and Windows Forms. As we continue the journey, we will move Xamarin and .NET web assembly to use the .NET 5 libraries, and extend the dotnet tools to target mobile and web assembly in the browser. At the same time, we'll continue to improve .NET capabilities as a leading cloud and container runtime.

You can tune in to hear Scott Hanselman and I talk about .NET 5 and beyond in our "[The Journey to One .NET" talk](https://aka.ms/ScottBuildSessions) today. 

Last year, [we laid out our vision for one .NET and .NET 5](https://devblogs.microsoft.com/dotnet/introducing-net-5/), we said we would take .NET Core and Mono/Xamarin implementations and unify them into one base class library (BCL) and toolchain (SDK). In the wake of the global health pandemic, we've had to adapt to the changing needs of our customers and provide the support needed to assist with their smooth operations. Our efforts continue to be anchored in helping our customers address their most urgent needs. As a result, we expect these features to be available in preview by November 2020, but the that unification will be truly completed with .NET 6, our Long-Term Support (LTS) release. Our vision hasn't changed, but our timeline has. 

We remain committed to one .NET platform and will deliver a quality .NET 5 release in November this year. You'll continue to see a wave of innovation happening with multiple previews on the journey to one .NET.  

We want to hear from you!  Share your feedback about .NET 5 at https://aka.ms/dotnet5_feedback_blog.  We greatly value your feedback and use it to help make decisions on the future of .NET.

## Download Preview 4

You can [download .NET 5.0 Preview 4](https://dotnet.microsoft.com/download/dotnet/5.0), for Windows, macOS, and Linux:

* [Windows and macOS installers](https://dotnet.microsoft.com/download/dotnet/5.0)
* [.zip and tar.gz files](https://dotnet.microsoft.com/download/dotnet/5.0)
* [Docker images](https://hub.docker.com/_/microsoft-dotnet)
* [Snap installer](https://snapcraft.io/dotnet-sdk)

ASP.NET Core, and EF Core are also being released today. [PowerShell has a .NET 5-based release today](https://devblogs.microsoft.com/powershell/powershell-7-1-preview-3-release/ ) and  now releases on the .NET schedule.

You need Visual Studio 2019 16.6 or later versions to use .NET 5.0. To use .NET 5.0 with Visual Studio Code, install the latest version of the [C# extension](https://code.visualstudio.com/Docs/languages/csharp). .NET 5.0 isn't yet supported with Visual Studio for Mac.

Release notes:

* [.NET 5.0 release notes](https://github.com/dotnet/core/tree/master/release-notes/5.0/preview)
* [.NET 5.0 known issues](https://github.com/dotnet/core/blob/master/release-notes/5.0/5.0-known-issues.md)
* [.NET Core 3.1 -> .NET 5.0 API diff](https://github.com/dotnet/core/tree/master/release-notes/5.0/preview/api-diff)
* [GitHub release](https://github.com/dotnet/core/releases)
* [GitHub tracking issue](https://github.com/dotnet/core/issues/4607)

## .NET 5 Highlights

Let's take a look at some of the release highlights that we expect to deliver with .NET 5, in November. Many of these changes are included, in part or in full, in Preview 4. The highlights will paint a clearer picture on the improvements you'll get to take advantage of in your development process and in production when you adopt .NET 5.

* Includes C# 9 and F# 5.
* Performance -- Improve [performance throughout the product](https://github.com/dotnet/runtime/pulls?page=2&q=is%3Apr+is%3Aclosed+label%3Atenet-performance).
   * [Regular expressions](https://devblogs.microsoft.com/dotnet/regex-performance-improvements-in-net-5/)
   * [Improve performance of `string.ToUpperInvariant`, `string.ToLowerInvariant`, `char.ToUpperInvariant`, `char.ToLowerInvariant`, and other related patterns](https://github.com/dotnet/runtime/pull/31968)
   * [Improve HTTP 1.1 performance](https://github.com/dotnet/corefx/pull/41640)
   * [Improve HTTP/2 performance](https://github.com/dotnet/runtime/pull/35694)
   * [Improve tiered compilation performance](https://github.com/dotnet/runtime/pull/32969)
   * [Improve stack prolog zeroing performance](https://github.com/dotnet/runtime/pull/32538)
   * [Improve performance of tailcalls used by F#](https://twitter.com/dsymetweets/status/1255077752149094400)
* Consistent performance: We have increased our focus on predictably consistent performance, reducing performance cliffs and outliers, with an emphasis on P95+ latency. 
   * [Improve call counting mechanism](https://github.com/dotnet/runtime/pull/32250) used by tiered JIT compilation to smooth out performance during startup
   * [Dynamic expansion of internal generic dictionary](https://github.com/dotnet/runtime/pull/32270) that eliminate performance cliffs hit by generic code 
   * [Pinned object heap](https://github.com/dotnet/runtime/pull/32283) to reduce heap fragmentation caused by pinning
   * Reduce GC pause times in specific situations, like [Array.Copy](https://github.com/dotnet/coreclr/pull/27776), [Array.Sort](https://github.com/dotnet/runtime/pull/35297) or [object unboxing](https://github.com/dotnet/runtime/pull/32353#issuecomment-586642480)  
* Single file applications -- a new [single-file publish type](https://github.com/dotnet/runtime/issues/36590) that executes your app out of a single binary (for example, can be used on read-only media).
* Windows ARM64 -- [Enable .NET to run natively on Windows ARM64](https://gist.github.com/richlander/6fd855f467036a941501e5dcaceabf0a), supporting both development scenarios and deployment of client apps on customer machines. 
* ARM64 -- [Improve ARM64 performance](https://github.com/dotnet/runtime/issues/35853) (Linux and Windows) in the JIT and BCL libraries.
* Containers -- [Reduce container image size](https://github.com/dotnet/dotnet-docker/issues/1814#issuecomment-625294750) and implement [new container APIs](https://github.com/dotnet/runtime/pull/34334) to enable .NET to stay up-to-date with container runtime evolution.
* New Target Framework -- We have adopted a [new approach for .NET TFMs](https://github.com/dotnet/designs/blob/master/accepted/2020/net5/net5.md).
* JSON APIs -- Enable easier [migration from Newtonsoft.Json to System.Text.Json](https://docs.microsoft.com/dotnet/standard/serialization/system-text-json-migrate-from-newtonsoft-how-to).

I'll share some more detailed information about some of these improvements, and where we see them headed.

### .NET 5.0 Target Framework

We are changing the approach we use for [target frameworks with .NET 5.0](https://github.com/dotnet/designs/blob/master/accepted/2020/net5/net5.md). The following two project file examples demonstrate using .NET Core 3.1 and .NET 5.0 target frameworks, by specifying the respective Target Framework Moniker (TFM). You can see a new, more compact, TFM for .NET 5.0:

.NET Core 3.0:

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>netcoreapp3.1</TargetFramework>
  </PropertyGroup>

</Project>
```

.NET 5.0:

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net5.0</TargetFramework>
  </PropertyGroup>

</Project>
```

We are making several important changes to .NET TFMs for .NET 5.0, to simplify using them, reduce concepts, and to make it easier to expose operating-system-specific APIs. 

Here is a super-quick summary:

Targeting API versions will be simpler with .NET going forward. We won't have two families of TFMs, like:`netcoreapp3.1` and `netstandard2.0`. Instead, we'll have just one, like: `net5.0`, and `net6.0`. That's because there is just one .NET implementation going forward, so there is no longer a need for .NET Standard (which made libraries compatible across multiple .NET products). You'll also be able to target operating system APIs, with a small extension to the TFM, like `net5.0-windows` and `net6.0-android`. We'll also remove the different SDKs from new project files, like `Sdk="Microsoft.NET.Sdk.WindowsDesktop"` since `net5.0-windows`, for example, will provide the same information. The biggest win is that by targeting `net5.0`, you get access to 100% of cross-platform APIs, not the subset that happened to be in .NET Standard. It will always be obvious which TFM to use (it's either portable code or OS-specific), and you'll never have to wait for APIs like `Span<T>` to be available.

Here are the details:

* `net5.0` is the new Target Framework Moniker (TFM) for .NET 5.0. 
* `net5.0` can consume `netcoreapp*` and `netstandard*` dependencies.
* `net5.0` should be thought of as replacing .NET Standard.
* `net5.0` supports [.NET Framework compatibility mode](https://docs.microsoft.com/en-us/dotnet/core/porting/third-party-deps#net-framework-compatibility-mode)
* `net5.0-windows` will be used to expose Windows-specific functionality, like Windows Forms and WPF.
* .NET 6.0 will use the same approach, with `net6.0` and will add `net6.0-ios` and `net6.0-android`.
* The OS-specific TFMs can include [OS version numbers](https://github.com/dotnet/designs/blob/master/accepted/2020/minimum-os-version/minimum-os-version.md), like `net6.0-ios14`.
* Portable APIs, like ASP.NET Core and Xamarin.Forms, will be usable with `net5.0`.

These changes are a result of thinking of [.NET Core as the future of .NET](https://devblogs.microsoft.com/dotnet/net-core-is-the-future-of-net/). We've been removing the "Core" name from various aspects of the product, including [APIs](https://github.com/dotnet/runtime/issues/33680) and [container repos](https://github.com/dotnet/dotnet-docker/issues/1765). We also saw an opportunity to further simplify .NET, by removing .NET Standard as a concept, for .NET 5.0+. [.NET Standard](https://github.com/dotnet/standard) has played a key role in establishing .NET Core, by creating a bridge with .NET Framework and Xamarin. The .NET Standard 2.0 version will remain relevant for many years, and we recommend you use it if you need to support .NET Framework. For apps and libraries that don't need to run on .NET Framework, we recommend targeting the `net5.0` TFM, which will give you access to the largest set of cross-platform APIs. For Xamarin, .NET Standard 2.0 and 2.1 remain relevant, however, once Xamarin is integrated into .NET as part of .NET 6.0, then it will switch to `net6.0` TFMs, and developers will target .NET Standard 2.0 exclusively for .NET Framework compatibility.

You likely have more questions you want answered. We'll be publishing a larger blog post on this topic before we release .NET 5.0. The following points answer some of the most obvious remaining questions:

* `netcoreapp5.0` was used in earlier previews and is no longer supported, however still works.
* Existing .NET Standard versions will work forever, and their continued use is supported.
* We don't expect to create any new `netstandard` versions. [.NET Standard 2.1](https://devblogs.microsoft.com/dotnet/announcing-net-standard-2-1/) will likely be the last version.
* There are no plans for a `net5.0-linux` TFM since we don't (yet) expose any Linux-specific APIs. Also, "Linux" is not a single uniform quantity, so it is unclear which APIs would be exposed in such a TFM. We could expose the [POSIX standard](https://en.wikipedia.org/wiki/POSIX), but then we'd call it `net5.0-posix`, and it would work on more operating systems than just Linux. However, we don't have plans for that either.
* We [do not plan to expose a TFM for web assembly](https://github.com/dotnet/runtime/issues/33328), for similar reasons as described for Linux.
* You cannot update the `TargetFrameworkVersion` in a .NET Framework project to 5.0 and expect it to become a .NET 5.0 project. It will not work. Instead, you need to [port your application to .NET Core](https://docs.microsoft.com/en-us/dotnet/core/porting/). For libraries, you can port to .NET Standard or .NET Core. We are [no longer adding .NET Framework APIs to .NET Core](https://github.com/dotnet/announcements/issues/130), so there is no need to wait to port your application to .NET Core.
* The new TFM plan is a foundational part of the [workloads project](https://github.com/dotnet/designs/blob/master/accepted/2020/workloads/workloads.md). We will add minimal support for workloads in .NET 5.0 and then implement the complete vision in .NET 6.0.

### Windows Forms Designer for .NET Core Released

Today we’re happy to announce that the Windows Forms designer for .NET Core projects is now available as a preview in Visual Studio 2019 version 16.6! We also have a newer version of the designer available in Visual Studio 16.7 Preview 1! 

To enable the designer in Visual Studio, go to Tools > Options > Environment > Preview Features and select the Use the preview Windows Forms designer for .NET Core apps option. 

The new designer supports all Windows Forms controls, except `DataGridView` and `ToolStripContainer` (coming soon). It include all other designer functionality you would expect, including: drag-and-drop, selection, move and resize, cut/copy/paste/delete of controls, integration with the Properties Window, events generation and more. Data binding and support for third party controls are coming soon.

Learn more in the [Windows Forms Designer for .NET Core Released](https://devblogs.microsoft.com/dotnet/windows-forms-designer-for-net-core-released/) post.


### Windows ARM64

.NET apps can now run natively on Windows ARM64. This follows the support we added for Linux ARM64 in .NET Core 3.0. With .NET 5.0, you can develop web and UI apps on Windows ARM64 devices, and deliver your applications to users who own [Surface Pro X](https://www.microsoft.com/en-us/p/surface-pro-x/8VDNRP2M6HHC) and similar devices. You can already run .NET Core and .NET Framework apps on Windows ARM64, but via x86 emulation. It's workable, but native ARM64 execution has much better performance.

You can download and use the .NET 5.0 SDK on ARM64 with today's preview 4 release. Currently, only Console and ASP.NET Core apps are supported. See [.NET 5.0 ARM64 tracking issue](https://gist.github.com/richlander/6fd855f467036a941501e5dcaceabf0a) to track our progress.

The `master` branch adds support for Windows Forms. This changes may make it into Preview 5, but Preview 6 for sure. You can download a `master` branch build from [dotnet/installer](https://github.com/dotnet/installer#installers-and-binaries).

At present, you need to download and expand `.zip` files for ARM64. We intend to add ARM64 MSIs for the final .NET 5 release.

We have been working closely with the [PowerShell](https://github.com/powershell/powershell) team to validate and enable PowerShell 7.1 on Windows ARM64. The team has had Windows ARM64 "experimental" builds for some time and intends to support PowerShell 7.1 on Windows ARM64, when they release. PowerShell 7.1 is built on .NET 5.0, and should be released around the same time.

The following image demonstrate the [Conway's Game of life](https://github.com/dotnet/samples/tree/master/windowsforms/Conway's-Game-of-Life/VB) VB and Windows Forms sample running on Windows ARM64.

<img width="398" alt="2020-05-15" src="https://user-images.githubusercontent.com/2608468/82086979-20f5bd00-96a4-11ea-8d73-abed8f2505fb.png">

### ARM64 Performance

We've been investing significantly in improving ARM64 performance, for over a year. We're committed to making ARM64 a high-performance platform with .NET. Platform portability and consistency have always been compelling characteristics of .NET. This includes offering great performance wherever you use .NET. With .NET Core 3.x, ARM64 has had functionality parity with x64 but was missing some key performance features and investments. We're making the first big investments in ARM64 performance in .NET 5.0.

There are several categories of improvements we're making: 

* Tune JIT optimizations for ARM64 ([example](https://github.com/dotnet/runtime/pull/35675))
* Enable and take advantage of ARM64 hardware intrinsics ([example](https://github.com/dotnet/runtime/pull/34486)).
* Adjust performance-critical algorithms in libraries for ARM64 ([example](https://github.com/dotnet/runtime/issues/34198)).

See [Improving ARM64 Performance in .NET 5.0](https://github.com/dotnet/runtime/issues/35853) to track our progress. 

[Hardware intrinsics](https://devblogs.microsoft.com/dotnet/hardware-intrinsics-in-net-core/) are a [low-level performance feature](https://github.com/dotnet/designs/blob/master/accepted/2018/platform-intrinsics.md) we added in .NET Core 3.0. At the time, we added support for x64 instructions and chips. As part of .NET 5.0, we are extending the feature to support ARM64. Just creating the intrinsics doesn't help performance. You need to use them in performance-critical code. We've [taken advantage of ARM64 intrinsics extensively in .NET libraries](https://github.com/dotnet/runtime/issues/33308) in .NET 5.0. You can also do this in your own code, although you need to be be familiar with CPU instructions to do so.

I'll explain how hardware intrinsics work with an analogy. For the most part, developers rely on types and APIs built into .NET, like `String.Split` or `HttpClient`. Those APIs often take advantage of native operating system APIs, via the [P/Invoke](https://docs.microsoft.com/dotnet/standard/native-interop/pinvoke) feature. P/Invoke enables high-performance native interop, and is used extensively in the BCL for that purpose.  You can use this same feature yourself to call native APIs. Hardware intrinsics are similar, except instead of calling operating system APIs, they enable you to directly use CPU instructions in your code. It's roughly equivalent to a .NET version of [C++ intrinsics](https://en.wikipedia.org/wiki/Intrinsic_function). Hardware intrinsics are best thought of as a CPU hardware-acceleration feature. They provide very tangible benefits, are now a key part of the performance substrate of the .NET libraries, and responsible for many of the benefits you read about in our [performance blog posts](https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-core-3-0/). In terms of comparison to C++, when .NET intrinsics are AOT-compiled into Ready-To-Run files, the intrinics have no runtime performance penalty.

Note: The Visual C++ compiler has an analogous [intrinsics feature](https://docs.microsoft.com/cpp/intrinsics/compiler-intrinsics). You can directly compare C++ to .NET hardware intrinsics, as you can see if you search for `_mm_i32gather_epi32` at [System.Runtime.Intrinsics.X86.Avx2](https://docs.microsoft.com/dotnet/api/system.runtime.intrinsics.x86.avx2), [x64 (amd64) intrinsics list](https://docs.microsoft.com/cpp/intrinsics/x64-amd64-intrinsics-list), and [Intel Intrinsics guide](https://software.intel.com/sites/landingpage/IntrinsicsGuide/#text=_mm_i32gather_epi32). You will see a lot of similarity. 

We're making our first big investments in ARM64 performance in 5.0, but will continue this effort in subsequent releases. We work directly with engineers from [ARM Holdings](https://en.wikipedia.org/wiki/Arm_Holdings) to prioritize product improvements and to design algorithms that best take advantage of the [ARMv8 ISA](https://en.wikipedia.org/wiki/ARM_architecture#ARMv8-A). Some of these improvements will accrue value to ARM32, however, we are not applying unique effort to ARM32.

Please share any performance information with us related to ARM64, either a notable improvement from 3.1 to 5.0, or performance with 5.0 that should be better.

### P95+ Latency

We see an increasing number of large internet-facing sites and services being hosted on .NET. While there is a lot of legitimate focus on the [requests per second (RPS) metric](https://twitter.com/ben_a_adams/status/1260792649625280513), we find that very few big site owners ask us about that or require millions of RPS. We hear a lot about latency, however, specifically about improving [P95 or P99 latency](https://docs.microsoft.com/en-us/azure/internet-analyzer/internet-analyzer-scorecard). Often, the number of machines or cores that are provisioned for (and biggest cost driver of) a site are chosen based on achieving a specific P95 metric, as opposed to say P50. We think of latency as being the true "money metric".

Our friends at StackOverflow do a great job of sharing data on their service. One of their engineers, Nick Craver, recently shared [improvements they saw to latency](https://twitter.com/Nick_Craver/status/1205289893674573829), as a result of moving to .NET Core:


```html
<blockquote class="twitter-tweet"><p lang="en" dir="ltr">The median page render time for questions dropped from about 21 ms (we were up a bit lately due to GC) to ~15ms.<br><br>The 95th percentile dropped from ~40ms to ~30ms (same measurement). 99th dropped from ~60ms to ~45ms.<br><br>Not too shabby, given we haven&#39;t optimized anything at all yet. <a href="https://t.co/MMHjI9wkuL">pic.twitter.com/MMHjI9wkuL</a></p>&mdash; Nick Craver (@Nick_Craver) <a href="https://twitter.com/Nick_Craver/status/1245027999034023936?ref_src=twsrc%5Etfw">March 31, 2020</a></blockquote> <script async src="https://platform.twitter.com/widgets.js" charset="utf-8"></script>
```

While you can see that we've been making good progress on latency, we're far from satisfied. In the (distant) past, we built features like [server GC](https://docs.microsoft.com/en-us/dotnet/standard/garbage-collection/workstation-server-gc) and [background GC](https://docs.microsoft.com/dotnet/standard/garbage-collection/background-gc) to improve latency, by taking advantage of course-grained CPU features like multiple-cores and threads, respectively. Those remain very important, however, we need to get a lot more creative to significantly improve latency moving forward, at least as it relates to the GC. We have started multiple projects along those lines.

Pinned objects have been a long-term challenge for GC performance, specifically because they accelerate (or cause) memory fragmentation. We've added a [new GC heap for pinned objects](https://github.com/dotnet/runtime/pull/32283). The [pinned object heap](https://github.com/dotnet/runtime/blob/master/docs/design/features/PinnedHeap.md) is based on the assumption that there are very few pinned objects in a process but that their presence causes disproportionate performance challenges. It makes sense to move pinned objects -- particularly those created by .NET libraries as an implementation detail -- to a unique area, leaving the generational GC heaps with few or no pinned objects, and with substantially higher performance as a result.

More recently, we've been attacking long-standing "hard problems" in the GC. [dotnet/runtime #2795](https://github.com/dotnet/runtime/pull/32795) applies a new approach to GC statics scanning that avoids lock contention when it is determining liveness of GC heap objects. [dotnet/runtime #25986](https://github.com/dotnet/coreclr/pull/) uses a new algorithm for balancing GC work across cores during the mark phase of garbage collection, which should increase the throughput of garbage collection with large heaps, which in turn reduces latency.

### Containers

We consider containers to be the most important cloud trend, and have been investing significantly in this modality. We are investing in containers in at least four different ways, at multiple levels of the .NET software stack.

The first is our investment in fundamentals. It's a bit odd to claim credit for these investments, since they also benefit non-containerized workloads. What might not be obvious is that more and more of the feedback we receive that influences our fundamentals investment is coming from developers who deploy containerized apps. There is a bias to containers with these investments.

We are working on making .NET perform better in containers. We heard reports about [poor performance related to a change in .NET Core 3.1](https://github.com/dotnet/runtime/issues/622) late last year (which was later reverted). We are now investigating the performance of using .NET in high-density and other configurations to help inform what we expect will be a relatively scoped set of changes that unlocks the next significant performance improvements in containers. It should be noted that [.NET Core 3.0 was a very big release for .NET and containers](https://devblogs.microsoft.com/dotnet/using-net-and-docker-together-dockercon-2019-update/), with the 3.1 issue being a small (and short-lived) blip.

We are always looking for opportunities to improve the images we publish. This includes [reducing image size](https://github.com/dotnet/dotnet-docker/issues/1814#issuecomment-625294750), but also extending the set of images we publish. We have decided to [start publishing Windows Server Core images](https://github.com/dotnet/dotnet-docker/issues/1852) based on feedback we heard on GitHub and other sources. The following is an [example Dockerfile](https://github.com/mthalman/dotnet-docker/blob/e4a2c1b8696b4b8657a775d6ee8e72d69e650a2f/5.0/runtime/windowsservercore-1909/amd64/Dockerfile) that will be used when we start publishing these images. We've made other changes that [reduce the size of Windows Server Core images](https://devblogs.microsoft.com/dotnet/we-made-windows-server-core-container-images-40-smaller/), making them more attractive to use.

Last, we are working to make it easier to work with container orchestrators and similar environments. We are adding support for [OpenTelemetry out of the box](https://github.com/dotnet/runtime/issues/31372) so that you can [capture distributed traces and metrics from your application](https://opentelemetry.io/). We are also working on a new set of experimental tools in the [dotnet/tye](https://github.com/dotnet/tye) repo that are intended to improve microservices developer productivity, both for development and deploying to a Kubernetes environment.

### Improving tiered compilation performance

We've been working on improving [tiered compilation](https://devblogs.microsoft.com/dotnet/tiered-compilation-preview-in-net-core-2-1/) for multiple releases. We continue to see it as a critical performance feature, for both startup and steady-state performance. We've made two big imporvements to tiered compilation this release.

The primary mechanism underlying tiered compilation is call counting. Once a method is called n times, the runtime asks the JIT to recompile the method at higher quality. From our earliest performance analyses, we knew that the call-counting mechanism was too slow (from a long-term standpoint), but didn't see a straightforward way to resolve that. As part of .NET 5.0, we've [improved the call counting mechanism](https://github.com/dotnet/runtime/pull/32250) used by tiered JIT compilation to smooth out performance during startup. In past releases, we've seen reports of unpredictable performance during the first 10-15s of process lifetime (mostly for web servers). That should now be resolved. Please test it and tell us what you see.

Another performance challenge we found was using tiered compilation for methods with loops. The fundamental problem is that you can have a cold method (only called once or a few times; $lt; n) with a loop that iterates a million times. A great example of this pathalogical case is the `Program.Main` method of an application. As a result, we disabled tiered compilation for methods with loops by default. Instead, we enabled applications to opt into using tiered compilation with loops. PowerShell is an application that chose to do this, after seeing high single-digit performance improvements in some scenarios.

To address methods with loops better, we implemented [on-stack replacement (OSR)](https://github.com/dotnet/runtime/pull/32969). This is similar to a feature that the Java Virtual Machines has, of the same name. [OSR](https://github.com/dotnet/runtime/blob/master/docs/design/features/OnStackReplacement.md) enables code executed by a currently running method to be re-compiled in the middle of method execution, while those methods are active "on-stack". This feature is currently experimental and opt-in (on x64).

To use OSR, multiple features must be enabled. The [PowerShell profile file](https://github.com/PowerShell/PowerShell/blob/70d9ed4d551e12eebf2985b5590c7cd6e106aaeb/src/powershell-win-core/powershell-win-core.csproj#L10) is a good starting point. You will notice that tiered compilation and all quick-jit features are enabled. In addition, you need to set `COMPlus_TC_OnStackReplacement=1` (its an environment variable).

Alternatively, you can set the following two environment variables, assuming all other settings have their default values:

* `COMPlus_TC_QuickJitForLoops=1`
* `COMPlus_TC_OnStackReplacement=1`

We do not intend to enable OSR by default in .NET 5.0 and have not yet decided if we will support it in production. Please give us any and all feedback you have on the feature. We are actively testing it now and will share more insights on it later.

### Single file applications

There are key scenarios where people want to use .NET where single-file distribution is a requirement, or at least preferred. We've been building up the key pieces that we need to enable this scenario over multiple releases, and will be including a [new single file publish type in .NET 5.0](https://github.com/dotnet/runtime/issues/36590). It's a feature we expect to continue to refine over multiple releases.

There are two aspects that make this feature expensive to build: 

* Accounting for different feature sets and constraints on Linux and Windows for loading executable content out of native resources.
* Ensuring that the debugger provides a multi-file-like experience for single-file applications.

For scoping purposes, we are supporting this feature on X64, for .NET 5.0, on Windows and Linux. It will work for ARM32/64 apps, however, we are not actively validating single files apps for the ARM architecture this release. Both [runtime-dependent and self-contained publish types](https://docs.microsoft.com/en-us/dotnet/core/deploying/) will be supported for single-file.

The experience between Windows and Linux is similar, but not the same. The differences are primarily relevant for self-contained single file applications, as described in the [Single-file publish design doc](https://github.com/dotnet/designs/blob/master/accepted/2020/single-file/design.md):

* Single-file publish Linux: `dotnet publish -r linux-x64 /p:PublishSingleFile=true`
   * Published files: `HelloWorld`, `HelloWorld.pdb`
* Single-file publish Windows: `dotnet publish -r win-x64 /p:PublishSingleFile=true`
   * Published files: `HelloWorld.exe`, `HelloWorld.pdb`, `coreclr.dll`, `clrjit.dll`, `clrcompression.dll`,  `mscordaccore.dll`

As you can see, on Windows, single-file self-contained applications require four additional files beyond the app. We were not able to include these runtime files into the single file app. We do not currently have a technical plan for hiding these extra files on Windows, even though we understand that it would be preferred. Note: `.pdb` files are required only for debugging scenarios, and `mscordaccore.dll` is required to collect crash dumps, including by [Windows Error Reporting (AKA "Watson")](https://en.wikipedia.org/wiki/Windows_Error_Reporting). 

### Improving migration from NewtonSoft.Json to System.Text.Json

We added [System.Text.Json](https://devblogs.microsoft.com/dotnet/try-the-new-system-text-json-apis/) as part of the .NET Core 3.0 release. It provides significant performance improvements over [Newtonsoft.Json](https://www.nuget.org/packages/Newtonsoft.Json), which has been the go-to Json library for .NET for many years. In some cases, it is hard to migrate to System.Text.Json, even with the [migration guide](https://docs.microsoft.com/dotnet/standard/serialization/system-text-json-migrate-from-newtonsoft-how-to) we've provided. We've been working on targeted features that enable easier migration, without giving up on the performance value proposition of System.Text.Json.

We've added the following migration features with .NET 5.0:

* [Add support for preserve references on JSON](https://github.com/dotnet/runtime/pull/655) - [Enables `ReferenceLoopHandling`](https://github.com/dotnet/runtime/issues/29900).
* [Add `JsonConstructor` and support for deserializing with parameterized ctors](https://github.com/dotnet/runtime/pull/33444) -- Adds support for immutable classes and structs to JsonSerializer.
* [Add JsonIgnoreCondition & per-property ignore logic](https://github.com/dotnet/runtime/pull/34049) - Adds support for null value handling.
* [Add JsonIncludeAttribute & support for non-public accessors](https://github.com/dotnet/runtime/pull/34675) -- Enables non-public getter usage, which is similar to the capability of the Newtonsoft.Json `JsonProperty` attribute.

At the same time, we're also improving the usability of System.Text.Json:

* [Add new System.Net.Http.Json project/namespace](https://github.com/dotnet/runtime/pull/33459) - Adds [new extension methods for HttpClient that allow serialization from/to JSON](https://github.com/dotnet/runtime/issues/32937).
* [Add copy constructor to JsonSerializerOptions](https://github.com/dotnet/runtime/pull/34725) - Enables a library of framework to manage a `JsonSerializerOptions` instance, with specific values it sets, while the type versions over time.

## WinRT Interop

We are moving to a [new model for supporting WinRT APIs as part of .NET 5.0](https://github.com/dotnet/runtime/issues/35318). This includes calling APIs (in either direction; CLR <==> WinRT), marshaling of data between the two type systems, and unification of types that are intended to be treated the same across the boundary (i.e. "projected types"; [`IEnumerable<T>'](https://docs.microsoft.com/dotnet/api/system.collections.generic.ienumerable-1) and [`IIterable<T>`](https://docs.microsoft.com/uwp/api/windows.foundation.collections.iiterable-1) are examples).

We will rely on a [new set of WinRT tools](https://github.com/microsoft/CsWinRT) provided by the WinRT team in Windows that will generate C#-based WinRT interop assemblies. We are currently working closely with that team. The tools will be delivered for .NET 5.0.

There are several benefits to the new system:

* Can be developed and improved separate from the .NET runtime.
* Symmetrical with interop systems provided for other OSes, like iOS and Android.
* Can take advantage of many other .NET features (AOT, C# features, IL linking).
* Simplifies the .NET runtime codebase.

We will be removing the existing WinRT interop system from the .NET runtime (and any other associated components) as part of .NET 5.0. This means that apps using WinRT with .NET Core 3.x will need to be rebuilt and will not run on .NET 5.0 as-is.

### Open source project improvements

We care a lot about open source, including enabling the .NET community to be productive on GitHub, and making .NET projects accessible to a large set of developers. We've been working on a variety of initiatives along those lines.

[dotnet/source-build](https://github.com/dotnet/source-build) enables building the entire .NET project/product from source with a single command. Red Hat uses this project to build the version of .NET Core that they distribute, and we work closely with them on that. [Fedora also uses source-build](https://fedoraproject.org/wiki/DotNet) to enable .NET Core in their package repositories. We want to make it straightforward for any developer, organization or company to use source-build, and are investing significantly in the project. You can follow the [.NET 5.0 source-build effort](https://github.com/dotnet/source-build/issues/1500) directly.

We started out the .NET Core project with too many GitHub repos. At its high, we had over 100 repos. That was too many to make sense to anyone, including the .NET Team. As part of the .NET 5.0 project, we decided to reduce the number of repos to a small and manageable collection. We announced our [intention to consolidate .NET repos](https://github.com/dotnet/announcements/issues/119) in August, 2019, and then provided a [final update on the plan](https://github.com/dotnet/announcements/issues/127) the following October. As part of that plan, we merged many repos together and moved almost all repos within the [dotnet org](https://github.com/dotnet). We retained repo history as part of the effort, which had some [funny side-effects](https://twitter.com/migueldeicaza/status/1219748706611798022). We continue to use the old repos for servicing the 2.1 and 3.1 product versions. [MSBuild](https://github.com/microsoft/msbuild) and [NuGet client](https://github.com/NuGet/NuGet.Client) repos remain in other orgs.

We are also working on [reducing build times](https://github.com/dotnet/arcade/blob/master/Documentation/Net5Builds.md) for most repos. Quicker build times make everyone more productive, and enable you to see PR build results quicker. This is a longer-term effort, and a theme that will repeat in subsequent releases. You can track progress at [.NET 5 Build Time Reduction Status](https://github.com/dotnet/arcade/blob/master/Documentation/Net5BuildTImeReductionStatus.md).

We largely focus on improving the product, but are more recently turning our attention to improving the .NET open source project for contributors and other participants. We recently asked for [feedback on improving the project and the experience participating in .NET repos](https://github.com/dotnet/announcements/issues/154). It is important to us that everyone feels like they have a voice (on project-related topics) on .NET project repos, that they are treated well, and that they can accomplish their goals. That doesn't mean we accept every PR or suggestion filed as an issue. As it relates to our approach, we intend to use clear language, be neutral to kind in our engagement, and [encourage contribution]((https://github.com/dotnet/runtime/issues?q=is%3Aopen+is%3Aissue+label%3Aeasy)). Are we getting this right? What would you like to see us do differently or better? Please give us your feedback on our [repo contribution experience survey](https://www.surveymonkey.com/r/ZLPVNX9?SourceRepo=dotnet-blog).

We have been asked multiple times to clarify and liberalize .NET Core licenses. We've done that, each time moving source and binary assets to the [MIT license](https://github.com/dotnet/core/blob/master/LICENSE.TXT). More recently, we've [clarified the license we use for .NET Windows builds](https://github.com/dotnet/installer/issues/7043). For most users, these changes won't matter much, but for others, they do, and we've done our best to satisfy their needs.

## New improvements in Preview 4

The following improvements are new in Preview 4 and not otherwise covered in the earlier highlights section.

### C# 9

.NET 5.0 Preview 4 includes the first preview of C# 9. The C# 9 preview includes numerous features including the first preview of Records, the first preview of top-level statements, improved pattern matching, and more. Here's a sneak peek of some of the pattern matching improvements:

```csharp
using System;

public enum LifeStage
{
    Prenatal,
    Infant,
    Toddler,
    EarlyChild,
    MiddleChild,
    Adolescent,
    EarlyAdult,
    MiddleAdult,
    LateAdult
}

public class C
{
    // "is not" patterns
    public static bool IsNotNull<T>(T item) => item is not null;
    
    // Relational patterns
    public static LifeStage LifeStageAtAge(int age) =>
        age switch
        {
            < 0 =>  LifeStage.Prenatal,
            < 2 =>  LifeStage.Infant,
            < 4 =>  LifeStage.Toddler,
            < 6 =>  LifeStage.EarlyChild,
            < 12 => LifeStage.MiddleChild,
            < 20 => LifeStage.Adolescent,
            < 40 => LifeStage.EarlyAdult,
            < 65 => LifeStage.MiddleAdult,
            _ =>    LifeStage.LateAdult,
        };
}
```

```html
<script src="https://gist.github.com/cartermp/c87120452d124c8cadceb62935bff003.js"></script>
```

You can play with this same code at [Sharplab.io](https://sharplab.io/#v2:EYLgZgpghgLgrgJwgZwLQAdYwggdsgZgB8ABAJgEYBYAKFpIIAIJc4BbRgGQEtIBlGFADmEWgG9ajKYwAKSXLCgAbADSTpASVxgouGGprTGAFQD2AE3NKcBowFEoCJQE8AwgAtuS87ekBZbktrDy8fdSkAQXNTa2QAYxZ9cMYHJ2couCUkw39AqwgMrN8pTlgC80yYWgBfWnomckZXcWSAelbGACJuZEZcUxhOxkwYbDxkZIZGEgoANkZgUxjGDWQAOQG1zKUAHmMAPgAKY0ZubDYASkYAXn3T89Pe/pg+7YBuZLaOgCUIJVhuKYFEphlgcPhJg05lxeBABMIIDD+IIRBEYBERIduHpGAirrdkkYEYxkAB3M5xdyE6QSHJGemMHaMAAMNzuSLhKIgADo5CxFKpqQymWQ2SVYfCRNytDo9MUGdImQAWMUcyU8sxBGxC+lM+a3cXIhHc1IuELeeUKxmMCiig08I1SgJa81hOlW61kVn2iVc7lRGIoBJynVGZXeu4OznG03pCpFUOKxizACsYqj6u5zvyhWyHukAH1VYbo1LSthc5bpNUPjRajQgA==).

Stay tuned for our blog post tomorrow that dives into all the details.

### F# 5

Building on the [F# 5 preview released earlier this year](https://devblogs.microsoft.com/dotnet/announcing-f-5-preview-1/), the update to F# 5 includes support for consuming Default Interface Methods (DIMs) and some big performance improvements. Here's a sneak peek at the DIMs support in F#:

```fsharp
open CSharp

// You can implement the interface via a class
type MyType() =
    member _.M() = ()

    interface MyDim

let md = MyType() :> MyDim
printfn "DIM from C#: %d" md.Z

// You can also implement it via an object expression
let md' = { new MyDim }
printfn "DIM from C# but via Object Expression: %d" md'.Z
```

```html
<script src="https://gist.github.com/cartermp/6a9eddd1fcb4f117baa93710fc9e7400.js"></script>
```

Stay tuned for a blog post tomorrow that goes over the details.

### C# Source Generators update

This release also includes an update to the [C#  Source Generators preview](https://devblogs.microsoft.com/dotnet/introducing-c-source-generators/). In addition to some bug fixes, it includes support for passing an analyzerconfig, which is essentially a list of key-value pairs, to a Source Generator. This lets your source generators work differently based on the input they recieve. For example, you may want to generate source code differently if a consuming project targets .NET Framework vs. .NET 5. Using an analyzerconfig allows you to pass information like a consuming project's TFM to allow for exactly this scenario.

### Support for ICU on Windows

We use the [ICU](http://site.icu-project.org/) library that provides Unicode and Globalization support for applications on Linux and macOS. We are now using [this same library on Windows](https://docs.microsoft.com/en-us/windows/win32/intl/international-components-for-unicode--icu-). [This change](https://github.com/dotnet/runtime/pull/34645) makes the behavior of globalization APIs such as culture-specific string comparison consistent between Windows 10 and other operating systems.


### Support for cgroup v2 (for containers)

.NET now has [support for cgroup v2](https://github.com/dotnet/runtime/pull/34334), which we expect will become an important container-related API in 2020 and beyond. Docker currently uses cgroup v1 (which is already supported by .NET). In comparison, cgroup v2 is simpler, more efficient, and more secure than cgroup v1. You can learn more about [cgroup and Docker resource limits](https://devblogs.microsoft.com/dotnet/using-net-and-docker-together-dockercon-2019-update/) from our 2019 Docker update. Linux distros and containers runtimes are in the [process of adding support for cgroup v2](https://medium.com/nttlabs/cgroup-v2-596d035be4d7). .NET 5.0 will work correctly in cgroup v2 environments once they become more common. Credit to [Omair Majid](https://github.com/omajid), who supports .NET at Red Hat.

### Reducing the size of container images

We are always looking for opportunities to make .NET container images smaller and easier to use. We made a change in Preview 4 that dramatically reduces the size of the aggregate images you pull in multi-stage-build scenarios (which is a very common pattern). We [re-based the SDK image on top of the ASP.NET image](https://github.com/dotnet/dotnet-docker/pull/1848) instead of [buildpack-deps](https://hub.docker.com/_/buildpack-deps). 

This change has the following win for multi-stage builds (example usage in [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/Dockerfile)):

Multi-stage build costs with **Ubuntu 20.04 Focal**:

| Pull Image | Before | After |
| ---------- | ------ | ----- |
| `sdk:5.0-focal`        |  268 MB | 232 MB|
| `aspnet:5.0-focal`| 64 MB | 10 KB (manifest only) |

*Net download savings*: 100 MB (-30%)

Multi-stage build costs with **Debian 10 Buster**:

| Pull Image | Before | After |
| ---------- | ------ | ----- |
| `sdk:5.0`        |  280 MB | 218 MB|
| `aspnet:5.0`| 84 MB | 4 KB (manifest only) |

*Net download savings*: 146 MB (-40%)

See [dotnet/dotnet-docker #1814](https://github.com/dotnet/dotnet-docker/issues/1814#issuecomment-625294750) for more detailed information.

This change helps multi-stage builds, where the `sdk` and the `aspnet` or `runtime` image you are targeting are the same version (we expect that this is the common case). With this change, the `aspnet` pull (for example), will be a no-op, because you will have pulled the `aspnet` layers via the initial `sdk` pull.

If you want a bit more context, keep reading. For 3.1 and prior, the SDK is based on the [buildpack-deps](https://hub.docker.com/_/buildpack-deps) image. When we started producing container images, we noticed other development platforms using buildpack-deps as the base of their tools/SDK images, so we followed the established pattern. We have specifically relied on the [`scm` layer](https://github.com/docker-library/buildpack-deps/blob/1bf287b61b2c02d8890f4806a9bfb2c7042b308d/focal/scm/Dockerfile), which includes source-control tools and is based on the [`curl` layer](https://github.com/docker-library/buildpack-deps/blob/1bf287b61b2c02d8890f4806a9bfb2c7042b308d/focal/curl/Dockerfile), which includes curl and similar network tools. That means that all those tools have been available to you in the SDK images. Unfortunately, this approach has come with a big tradeoff. Since Docker only allows for a single line of inheritance (each image can only have one parent), the `sdk` image needs to carry its own copy of ASP.NET, and Docker doesn't see the actually identical ASP.NET bytes in the `sdk` image as the same as the ones in the `aspnet` image. That situation requires a lot of wasted bytes to be stored and transferred. On the other hand, people don't want to give up using the tools provided by `buildpack-deps`. 

As a compromise position, we re-based the `sdk` on `aspnet`, added [some of the tools back](https://github.com/dotnet/dotnet-docker/pull/1848#issue-404674130), while retaining 90+% of the size savings.

This explanation is descriptive of what we did for Ubuntu. The story with Debian is more complicated, and responsible for the larger size win. In short, the Debian variants of `aspnet` and `runtime` are based on the `-slim` Debian variant, while `buildpack-deps` is based on the non-slim Debian images. That means that for multi-stage builds with Debian, that you pull Debian twice! Even the distro layer hasn't been shared until now.

We made similar changes for [Alpine and Nano Server](https://github.com/dotnet/dotnet-docker/pull/1832). There is no `buildpack-deps` image for either Alpine or Nano Server. However, the `sdk` images for Alpine and Nano Server were not previously built on top of the ASP.NET image. We fixed that. You will see significant size wins for Alpine and Nano Server as well with 5.0, for multi-stage builds.

We've known about these problems for a long time, but they had never been the next thing to go resolve. We decided that the 5.0 release was a good time to chase these size wins. Please tell us if there are any rough edges that we didn't expect.

## .NET 5.0 will switch to the `dotnet` container repo

As part of the move to ".NET" as the product name, we are now publishing .NET 5.0 Preview 4 and later images to the [`mcr.microsoft.com/dotnet`](https://hub.docker.com/_/microsoft-dotnet) family of repos, instead of [`mcr.microsoft.com/dotnet/core`](https://hub.docker.com/_/microsoft-dotnet-core). Please update your `FROM` statements and scripts accordingly. .NET Core 3.1 and 2.1 will continue to be published to [`mcr.microsoft.com/dotnet/core`](https://hub.docker.com/_/microsoft-dotnet-core).

## Closing

.NET 5.0 is shaping up to be another big foundational release, much like .NET Core 1.0, 2.0 and 3.0. It includes many new improvements that should make your applications and development process better and easier. Much of the team has been working on .NET 5 since before we released .NET Core 3.0. We've been looking forward to releasing all these improvements in a near-final form for many months, and will now watch for your feedback as you try them out.

As you can see from the product investments we've chosen, we're focused on modern scenarios, and giving you straightforward and predictable solutions that power the portfolio of applications you need, to run your business or organization. It's critical to us that you give us feedback to help us improve the features that you've read about here, but also with that you'd like to see next. As you may have seen earlier in the post, we're already deep into planning the .NET 6.0 release, so its not too early to give us future-looking feedback. 

Please Share your feedback about .NET at https://aka.ms/dotnet5_feedback_blog.
