# Announcing .NET 5.0 Preview 2

Today, we're releasing .NET 5.0 Preview 2. It contains a set of smaller features and performance improvements. We're continuing to work on the bigger features that will define the 5.0 release, some of which are starting to show up as initial designs at [dotnet/designs](https://github.com/dotnet/designs/pulls). The [.NET 5.0 Preview 1 post](https://devblogs.microsoft.com/dotnet/announcing-net-5-0-preview-1/) covers what we are planning on building for .NET 5.0. Please take a look at the post and the designs repository and share any feedback you have. And, of course, please install Preview 2, and test any workloads you can with it.

You can [download .NET 5.0 Preview 2](https://dotnet.microsoft.com/download/dotnet-core/5.0), for Windows, macOS, and Linux:

* [.NET 5.0 Preview 2 and Runtime](https://dotnet.microsoft.com/download/dotnet-core/5.0)
* [Docker images](https://hub.docker.com/_/microsoft-dotnet-core)
* [Snap installer](https://snapcraft.io/dotnet-sdk)

ASP.NET Core and EF Core are also being released today.

You need to use Visual Studio 2019 16.5 to use .NET 5.0. Install the latest version of the [C# extension](https://code.visualstudio.com/Docs/languages/csharp), to use .NET 5.0 with Visual Studio Code. Visual Studio for Mac isn't yet supported.

Release notes:

* .NET 5.0 release notes
* .NET 5.0 known issues
* .NET Core 3.1 -> .NET 5.0 API diff
* GitHub release
* GitHub tracking issue

Let's look at some of the improvements in Preview 2.

## Code quality improvements in RyuJIT

Every release includes a set of changes that improve the machine code that the JIT generates (we call this "code quality"). Better code quality means better performance. In summary, about half of the following improvements are actual new optimizations and the other half are due to changing the flow of RyuJIT to enable existing optimizations to apply to more code patterns.

* [Use xmm for stack prolog - dotnet/runtime #32538](https://github.com/dotnet/runtime/pull/32538) -- Change to x86/x64 prolog zeroing code. Improvements: [Json](https://github.com/dotnet/runtime/pull/32538#issuecomment-595503265); [TechEmpower](https://github.com/dotnet/runtime/pull/32538#issuecomment-595619687/). Credit: [Ben Adams](https://github.com/benaadams).
* [Add ValueNumbering support for GT_SIMD and GT_HWINTRINSIC tree nodes - dotnet/runtime #31834](https://github.com/dotnet/runtime/pull/31834) -- Enable the optimizer for SIMD and hardware intrinsic types.
* [Use GT_NULLCHECK for unconsumed indirections - dotnet/runtime #32641](https://github.com/dotnet/runtime/pull/32641) -- Remove redundant null checks.
* [invoke nullable box optimizations earlier - dotnet/runtime #32269](https://github.com/dotnet/runtime/pull/32269) -- Improve optimizations for `Nullable<T>`.
* [Optimize range checks for a[i & C], a[i % c] and a[(i & c1)>>c2)] patterns](https://github.com/dotnet/runtime/pull/1644) -- Improvement in range check elimination.
* [Optimize `obj.GetType() != typeof(X)` for sealed classes - dotnet/runtime #32790](https://github.com/dotnet/runtime/pull/32790) -- Improvement to type check expression.
* [Eliminate duplicate zero initializations more aggressively - dotnet/runtime #31960](https://github.com/dotnet/runtime/pull/31960) -- Better and broader approach for eliminating duplicate zero initializations.
* [Fix method and basic block flags used by early opts](https://github.com/dotnet/runtime/pull/2126) - Enables certain optimization to be used more often. For example, replacing array length with a constant now occurs much more often.

If you like this style of improvement or are an ARM64 user, you may be interested in [Vectorise BitArray for ARM64](https://github.com/dotnet/runtime/pull/33749), coming soon (already merged, but not in Preview 2). This change demonstrates a lot of focus on ARM64 in .NET 5.0.

## Garbage Collector

* [Card mark stealing - dotnet/coreclr #25986](https://github.com/dotnet/coreclr/pull/25986) -- Server GC (on different threads) can now work-steal while marking gen0/1 objects held live by older generation objects. This means that ephemeral GC pauses are shorter for scenarios where some GC threads took much longer to mark than others.
* [Introducing Pinned Object Heap - dotnet/runtime #32283](https://github.com/dotnet/runtime/pull/32283) -- Implemented part of the POH (Pinned Object Heap) feature – the part internal to GC. This new heap (essentially a peer to LOH) will allow the GC to manage pinned objects separately, and as a result avoid the negative effects of pinned objects on the generational heaps.
* [Allow allocating large object from free list while background sweeping SOH](https://github.com/dotnet/runtime/pull/2103) -- Enabled LOH allocations using the free list while BGC is sweeping SOH. Previously this was only using end of segment space on LOH. This allowed for better heap usage. 
* [Background GC suspension fixes - dotnet/coreclr #27729](https://github.com/dotnet/coreclr/pull/27729) --	Suspension fixes to reduce time for both BGC and user threads to be suspended. This reduces the total time it takes to suspend managed threads before a GC can happen. [dotnet/coreclr #27578](https://github.com/dotnet/coreclr/pull/27578) also contributes to the same outcome.
* [Fix named cgroup handling in docker](https://github.com/dotnet/runtime/pull/980) -- Added support to read limits from named cgroups. Previously we only read from the global one.

## Closing

Please take a moment to try out Preview 2, possibly in a container, a VM. We'd like your feedback on the quality of the release. There is a lot more coming, over the next several months, leading up a November release.

We're running 50% of the [.NET Website](https://dot.net/) traffic on .NET 5.0 as a test case, using Azure load balancing. We've been doing that since days after we released Preview 1. You may remember us doing something similar with .NET Core 3.0 and 3.1. By splitting the traffic 50/50, we can ensure that 5.0 gets consistently better with constant performance data available to us. This model keeps us honest, and it's also a great testing approach. We trust our most important site with ALL of our previews, in production (which we don't recommend for anyone else). That should make adopting our final release a pretty easy choice for you. The version number is in the footer of [.NET website](https://dotnet.microsoft.com/); feel free to check at any time.

Stepping back for a minute, many of you might be interested in how the the folks on the .NET Team at Microsoft are doing. We're doing well. We have lots of Teams meeting every day to organize and support one another, and are just as active on GitHub as ever. The team is close-knit, and collaborating across multiple time-zones. We're all focused on this release, and very much intending to deliver what we set out to deliver when we first defined our plans, after releasing .NET Core 3.0. Take care.
