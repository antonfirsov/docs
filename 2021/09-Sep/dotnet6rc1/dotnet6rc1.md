---
post_title: Announcing .NET 6 Release Candidate 1
username: rlander@microsoft.com
microsoft_alias: rlander
categories: .NET Core, .NET
desired_publication_date: 09/14/2021
summary: .NET 6 Preview RC1 is now available.
---

We are happy to release .NET 6 Release Candidate 1. It is the first of two "go live" release candidate releases that are supported in production. For the last month or so, the team has been focused exclusively on quality improvements that resolve functional or performance issues in new features or regressions in existing ones. 

You can [download .NET 6 Release Candidate 1](https://dotnet.microsoft.com/download/dotnet/6.0) for Linux, macOS, and Windows.

* [Installers and binaries](https://dotnet.microsoft.com/download/dotnet/6.0)
* [Container images](https://hub.docker.com/_/microsoft-dotnet)
* [Linux packages](https://github.com/dotnet/core/blob/main/release-notes/6.0/install-linux.md)
* [Release notes](https://github.com/dotnet/core/blob/main/release-notes/6.0/README.md)
* [API diff](https://github.com/dotnet/core/tree/main/release-notes/6.0/preview/api-diff/preview7)
* [Known issues](https://github.com/dotnet/core/blob/main/release-notes/6.0/known-issues.md)
* [GitHub issue tracker](https://github.com/dotnet/core/issues/6141)

See the [.NET MAUI](https://devblogs.microsoft.com/dotnet), [ASP.NET Core](https://devblogs.microsoft.com/aspnet/), and [EF Core](https://devblogs.microsoft.com/dotnet) posts for more detail on what’s new for client, web, and data access application scenarios.

We're at that fun part of the cycle where we support the new release in production. We genuinely encourage it. In the last post, I suggested that folks email us at dotnet@microsoft.com to ask for guidance on how to approach that. A bunch of businesses reached out wanting to explore what they should do. The offer is still open. We'd love to hit two or three dozen early adopters and are happy to help you through the process. It's pretty straightforward.

.NET 6 RC1 has been tested and is supported with [Visual Studio 2022 Preview 2](https://visualstudio.microsoft.com/vs/preview/vs2022/). Visual Studio 2022 enables you to leverage the Visual Studio tools developed for .NET 6 such as development in .NET MAUI, Hot Reload for C# apps, new Web Live Preview for WebForms, and other performance improvements in your IDE experience.

Support for .NET 6 RC1 is coming soon in Visual Studio 2022 for Mac Preview 1, which is currently available as a private preview.

Check out the new [conversations posts](https://devblogs.microsoft.com/dotnet/category/conversations/) for in-depth engineer-to-engineer discussions on the latest .NET features.

The rest of the post is dedicated to foundational features in .NET 6. In each release, we take on a few projects that take multiple years to complete and that (by definition) do not deliver their full value for some time. Given that these features have not come to their full fruition, you'll notice a bias in this post to what we're likely to do with these features in .NET 7 and beyond.

## Source build

[Source build](https://github.com/dotnet/source-build) is a scenario and also infrastructure that we've been working on in collaboration with Red Hat since before shipping .NET Core 1.0. Several years later, we're very close to delivering a fully automated version of it. For Red Hat Enterprise Linux (RHEL) .NET users, this [capability is a big deal](https://www.redhat.com/en/about/development-model). Red Hat tells us that .NET has grown to become an important developer platform for their ecosystem. Nice!

Clearly, .NET source code can be built into binaries. Developers do that every day after cloning a repo from the [dotnet org](https://github.com/dotnet). That's not really what this is about.

The [gold standard](https://wiki.debian.org/Packaging/SourcePackage) for Linux distros is to build open source code using compilers and toolchains that are part of the distro archive. That works for the .NET runtime (written in C++), but not for any of the code written in C#. For C# code, we use a two-pass build mechanism to satisfy distro requirements. It's a bit complicated, but it's important to understand the flow.

Red Hat builds .NET SDK source using the Microsoft build of the .NET SDK (#1) to produce a pure open source build of the SDK (#2). After that, the same SDK source code is built again using this fresh build of the SDK (#2) to produce a provably open source SDK (#3). This final SDK (#3) is then made available to RHEL users. After that, Red Hat can use this same SDK (#3) to build new .NET versions and no longer needs to use the Microsoft SDK to build monthly updates.

That process may be surprising and confusing. Open source distros need to be built by open source tools. This pattern ensures that the Microsoft build of the SDK isn't required, either by intention or accident. There is a higher bar, as a developer platform, to being included in a distro than just using a compatible license. The source build project has enabled .NET to meet that bar.

The deliverable for source build is a source tarball. The source tarball contains all the source for the SDK (for a given release). From there, Red Hat (or another organization) can build their own version of the SDK. Red Hat policy requires using a built from source toolchain to produce a binary tar ball, which is why they use a two-pass methodology. But this two-pass method is not required for source build itself.

It is common in the Linux ecosystem to have both source and binary packages or tarballs available for a given component. We already had binary tarballs available and now have source tarballs as well. That makes .NET match the standard component pattern.

The big improvement in .NET 6 is that the source tarball is a now a product of our build. It used to require significant manual effort to produce, which also resulted in significant latency delivering the source tarball to Red Hat. Neither party was happy about that.

We've been working closely with Red Hat on this project for five+ years. It has succeeded, in no small part, due to the efforts of the excellent Red Hat engineers we've had the pleasure of working with. Other distros and organizations will benefit from their efforts.

As a side note, source build is a big step towards [reproducible builds](https://wiki.debian.org/ReproducibleBuilds), which we also strongly believe in. The .NET SDK and C# compiler have significant reproducible build capabilities. There are some specific technical issues that still need to be resolved for full reproducibility. Surprisingly, a major remaining issue is using stable compression algorithms for compressed content in assemblies.

## Profile-guided optimization (PGO)

[Profile Guided Optimization (PGO)](https://devblogs.microsoft.com/dotnet/conversation-about-pgo/) is an important capability of most developer platforms. It is based on the assumption that the code executed as part of startup is often uniform and that higher-level performance can be delivered by exploiting that.

There are lots of things you can do with PGO, such as:

- Compile startup code at higher-quality.
- Reduce binary size by compiling low-use code at lower-quality (or not at all).
- Re-arrange application binaries such that code used at startup is co-located near the start of the file.

.NET has used PGO in various forms for twenty years. The system we initially developed was both proprietary and (very) difficult to use. It was so difficult to use that very few other teams at Microsoft used it even though it could have provided significant benefit. With .NET 6, we decided to rebuild the PGO system from scratch. This was motivated in large part by [crossgen2](https://devblogs.microsoft.com/dotnet/conversation-about-crossgen2/) as the new enabling technology.  

There are several aspects to enabling a world-class PGO system (at least in our view):

- Easy-to-use training tools that collect PGO data from applications, on the developer desktop and/or in production.
- Straightforward integration of PGO data in the application and library build flow.
- Tools that process PGO data in various ways (differencing and transforming).
- Human- and source-control-friendly text format for PGO data.
- Static PGO data can be used by a dynamic PGO system to establish initial insight.

In .NET 6, we focused on building the foundation that can enable those and other experiences. In this release, we just got back to what we had before. The runtime libraries are compiled to [ready-to-run format](https://devblogs.microsoft.com/dotnet/conversation-about-ready-to-run/) optimized with (the new form of) PGO data. This is all enabled with crossgen2. At present, we haven't enabled anyone else to use PGO to optimize apps. That's what will be coming next with .NET 7.

## Dynamic PGO

[Dynamic PGO](https://github.com/dotnet/runtime/issues/43618) is the mirror image of the static PGO system I just described. Where static PGO is integrated with crossgen2, dynamic PGO is integrated with RyuJIT. Where static PGO requires a separate training activity and using special tools, dynamic PGO is automatic and uses the running application to collect relevant data. Dynamic PGO is similar to a [tracing JIT](https://en.wikipedia.org/wiki/Tracing_just-in-time_compilation).

Dynamic PGO is currently opt-in, with the following environment variables set.

- `DOTNET_TieredPGO=1`
- `DOTNET_TC_QuickJitForLoops=1`

The [Performance Improvements in .NET 6](https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-6/#jit) post does a great job of demonstrating how dynamic PGO improves performance.

[Tiered compilation (TC)](https://devblogs.microsoft.com/dotnet/tiered-compilation-preview-in-net-core-2-1/) has similar characteristics to dynamic PGO. In fact, dynamic PGO can be thought of as tiered compilation v2. TC provides a lot of benefit, but is unsophisticated in multiple dimensions and can be greatly improved. It's brains for a scarecrow.

Perhaps the most interesting capability of dynamic PGO is devirtualization. The cost of method calls can be described like this: interface > non-interface virtual > non-virtual. If we can transform an interface method call into a non-virtual call, then that's a significant performance improvement. That's super hard in the general case, since it is very difficult to know statically which classes implement a given interface. If it is done wrong, the program will (hopefully) crash. Dynamic PGO can do this correctly and efficiently.

RyuJIT can now generate code using the "guarded devirtualization" compiler pattern. I'll explain how that works. Dynamic PGO collects data on the actual classes that satisfy an interface in some part of a method signature at runtime. If there is a strong bias to one class, it can tell RyuJIT to generate code that prefers that class and use direct method calls in terms of that specific class. As suggested, direct calls are much faster. If, in the unexpected case, that the object is of a different class, then execution will jump to slower code that uses interface dispatch. This pattern preserves correctness, isn't much slower in the unexpected case, and is much faster in the expected typical case. This dual-mode system is called guarded since the faster devirtualized code is only executed after a successful type check.

There are other capabilities that we can imagine implementing. For example, some combination of Crossgen2 and Dynamic PGO can learn how to sparsely compile methods based on usage (don't initially compile rarely taken if/else blocks). Another idea is that Crossgen2 can communicate (via some weighting) which methods are most likely to benefit from higher tiers of compilation at runtime.

## Crossgen2

I've discussed crossgen2 multiple times now, both in this post and previously. Crossgen2 is a major step forward for ahead-of-time or pre-compilation for the platform. There are several aspects to this that lay the foundation for future investments and capabilities. It's non-obvious but crossgen2 may be the most promising foundational feature of the release. I'll try to explain why I'm so excited about it.

The most important aspect is that crossgen2 has a design goal of being a standalone compiler. Crossgen1 was a separate build of the runtime with just the components required to enable code generation. That approach was a giant hack and problematic for a dozen different reasons. It got the job done, but that's it.

As a result of being a standalone compiler, it could be written in any language. Naturally, we chose C#, but it could have equally been written in Rust or JavaScript. It just needs to be able to load a given build of RyuJIT as a plug-in and communicate with it with the prescribed protocol.

Similarly, the standalone nature enables it to be cross-targeting. It can target ARM64 from x64, Linux from Windows, or .NET 6 from .NET 7.

I'll cover a few of the scenarios that we're immediately interested in enabling, after .NET 6. From here on, I'll just use "crossgen", but I mean "crossgen2".

By default, [ready-to-run (R2R) code](https://github.com/dotnet/runtime/blob/main/docs/design/coreclr/botr/readytorun-overview.md) has the same version-ability as IL. That's objectively the right default. If you are not sure what the implications of that are, that's demonstrating we chose the right default.  In .NET 6, we added a new mode that extends the version boundary from a single assembly to a group of assemblies. We call that a "version bubble". There are two primary capabilities that version bubbles enable: inlining of methods and cross-assembly generic instantiations (like `List<string>` if `List<T>` and `string` were in different assemblies). The former enables us to generate higher-quality code and the latter enables actually generating R2R code where we otherwise have to rely on the JIT. This feature delivers double-digit startup benefits in our tests. The only downside is that version bubbles typically generate more code as a result. That's where the next capability can help.

Today, crossgen generates R2R code for all methods in an assembly, including the runtime and SDK. That's very wasteful since probably at least half of them would be best left for jitting at runtime (if they are needed at all). Crossgen has had the capability for partial compilation for a long time, and we've even used it. In .NET Core 3.0, we used it to remove about 10MB from the runtime distribution on Linux. Sadly, that configuration got lost at some point and we're now carrying that extra 10MB around. For .NET 7, we're going to take another crack at this, and will hopefully identify a lot more than 10MB of R2R code to no longer generate (which naturally has benefits beyond just size reduction).

[Vector or SIMD instructions](https://github.com/dotnet/designs/pull/173) are significantly exploited in .NET libraries and are critical to delivering high performance. By default, crossgen uses an old version (SSE2) of these instructions and relies on tiered compilation to generate the best SIMD instructions for a given machine. That works but isn't great for modern hardware and is particularly problematic for short-running serverless applications. Crossgen enables specifying a modern and better SIMD instruction set like AVX2 (for Intel and AMD).  We plan to switch to producing ready-to-run images for AVX2 with this new capability for .NET 7. This capability isn't currently relevant for Arm64 hardware, with NEON being universally and the best instructions available. When SVE and SVE2 become common place, we'll need to deploy a similar model for Arm64.

Whatever is the most optimal crossgen configuration, that's how we'll deliver container images. We see containers as our most legacy-free distribution type and want to better exploit that. We see lots of opportunity for "fully optimized by default" for containers.

## Security mitigations

We published a [security roadmap](https://github.com/dotnet/designs/blob/main/accepted/2021/runtime-security-mitigations.md) earlier this year to provide more insight on how we are approaching industry standard security techniques and hardware features. The roadmap is also intended to be a conversation, particularly if you've got a viewpoint you want to share on these topics.

We added preview support for two key security mitigations this release: [CET](https://github.com/dotnet/runtime/blob/release/6.0/docs/design/features/intel-cet-dotnet6.md), and W^X. We intend to enable them by default in .NET 7.

### CET

Intel's [Control-flow Enforcement Technology (CET)](https://newsroom.intel.com/editorials/intel-cet-answers-call-protect-common-malware-threats/) is a security feature available in some newer Intel and AMD processors. It adds capabilities to the hardware that protect against some common types of attacks involving control-flow hijacking. With CET shadow stacks, the processor and operating system can track the control flow of calls and returns in a thread in the shadow stack in addition to the data stack, and detect unintended changes to the control flow. The shadow stack is protected from application code memory accesses and helps to defend against attacks involving return-oriented programming (ROP).

See [.NET 6 compatibility with Intel CET shadow stacks (early preview on Windows)](https://github.com/dotnet/runtime/blob/release/6.0/docs/design/features/intel-cet-dotnet6.md) for more details and instructions on enabling CET.

### W^X

[W^X](https://en.wikipedia.org/wiki/W%5EX) is one of the most fundamental mitigations. It blocks the simplest attack path by disallowing memory pages to be writeable and executable at the same time. The lack of this mitigation has resulted in us not considering more advanced mitigations, since they could be bypassed by the lack of this capability. With W^X in place, we will be adding other complementary mitigations, like CET.

Apple has made the W^X mandatory for future versions of macOS desktop operating system as part of Apple Silicon transition. It motivated us to schedule implementation of this mitigation for .NET 6, on all supported operating systems. Our principle is to treat all supported operating systems equally with respect to security, where possible. W^X is available all operating systems with .NET 6 but only enabled by default on Apple Silicon. It will be enabled on all operating systems for .NET 7.

## HTTP/3

[HTTP/3](https://en.wikipedia.org/wiki/HTTP/3) is a new HTTP version. It is in preview with .NET 6. HTTP/3 solves existing functional and performance challenges with past HTTP versions by using a new underlying connection protocol called QUIC. QUIC uses UDP and has TLS built in, so its faster to establish connections as the TLS handshake occurs as part of the connection. Each frame of data is independently encrypted so the protocol no longer has the head of line blocking challenge in the case of packet loss. Unlike TCP a QUIC connection is independent of the IP address, so mobile clients can roam between wifi and cellular networks, keeping the same logical connection, and can continuing long downloads.

At the current time, the RFC for HTTP/3 is not yet finalized, and so can still change. We have included HTTP/3 in .NET 6 so that you can start experimenting with it. It is a preview feature, and so is unsupported. There may be rough edges, and there needs to be broader testing with other servers & clients to ensure compatibility.

.NET 6 does not include support for HTTP/3 on macOS, primarily because of a lack of a QUIC-compatible TLS API. .NET uses SecureTransport on MacOS for its TLS implementation, which does not yet include TLS APIs to support QUIC handshake.

A deep-dive blog post on HTTP/3 in .NET 6 will soon be published.

## SDK workloads

[SDK workloads](https://github.com/dotnet/designs/blob/main/accepted/2020/workloads/workloads.md) is a new capability that enables us to add new major capabilities to .NET without growing the SDK. That's what we've done for .NET MAUI, Android, iOS, and WebAssembly. We haven't measured all of the new workloads together, but it's easy to guess that they would sum to at least the size of the SDK as-is. Without workloads, you'd probably be unhappy about the size of the SDK.

In future releases, we intend to remove more components and make them optional, including ASP.NET and the Windows Desktop. In the end, one can imagine the SDK containing only MSBuild, NuGet, the language compilers and workload acquisition functionality. We very much want to cater to a broad .NET ecosystem and to deliver just the software you need to get your particular job done. You can see how this model would be much better for CI scenarios, enabling us to deliver a bespoke set of components for the specific code being built.

## Contributor showcase

We’re coming to the end of the release. We thought we’d take a few moments to showcase some community contributors who have been significant contributions. We covered two contributors in the [.NET 6 Preview 7](https://devblogs.microsoft.com/dotnet/announcing-net-6-preview-7/#contributor-showcase) post and want to highlight another in this post.

The text is written in the contributor’s own words.

### Theodore Tsirpanis (@teo-tsirpanis)

<img src="https://avatars.githubusercontent.com/u/12659251" width="250px"/>

*My name is [Theodore Tsirpanis](https://github.com/teo-tsirpanis), and I am from Thessaloniki, Greece. In
less than a month, I will begin my senior (fourth and final) year as an
undergraduate student at the Department of Applied Informatics of the
University of Macedonia. Besides maintaining some projects of my own
(mostly developer-facing tools and libraries), I have been contributing
to various open-source projects on GitHub for quite some time. What I
like the most about open-source is that the soonest you find a bug or a
performance improvement opportunity, you can very quickly act upon it
yourself.*

*My journey with .NET also started quite some time ago. My first
programming language was Pascal but it didn't take too long to discover
C# and later F#, and marvel at the sheer amount of technological
artisanship that permeates the .NET ecosystem. I am always eager to read
a new blog post and spend more time than I remember randomly strolling
around the libraries' source code using ILSpy, improving my coding
skills in the process. My enjoyment of writing highly performant code,
and the impact of contributing to such a large project is what motivated
me to contribute to the .NET libraries. The team members are very
responsive and have the same passion for code quality and performance as
I do. I am very glad to have played a part in making .NET a great piece
of software, and I look forward to contributing even more in the future.*

## Closing

.NET 6 has a lot of new features and capabilities that are for the here-and-now, most of which have been explored in all the preview  and also in the upcoming .NET 6 posts. At the same time, it's inspiring to see the new features in .NET 6 that will lay the foundation for what's coming next. These are big-bet features that will push the platform forward in both obvious and non-obvious ways.

For the first several releases, the team needed to focus on establishing .NET Core as a functional and holistic open source and cross-platform development system. Next, we focused on unifying the platform with Xamarin and Mono. You can see that we're departing from those critical but more simply enabling projects. It's great to see the platform again expand in terms of fundamental runtime capabilities, and there's much more to come along those lines.

Thanks for being a .NET developer.
