---
post_title: Conversation about ready to run
username: rlander@microsoft.com
microsoft_alias: rlander@microsoft.com
categories: .NET Core, .NET
desired_publication_date: 05/20/2021
summary: Conversation with .NET engineers about the ready to run executable format.
---

[Ready to run (R2R)](https://docs.microsoft.com/dotnet/core/deploying/ready-to-run) is the [native executable code format](https://github.com/dotnet/runtime/blob/main/docs/design/coreclr/botr/readytorun-overview.md) for .NET. R2R code is produced using the [crossgen tool](https://devblogs.microsoft.com/dotnet/conversation-about-crossgen2/).

We're using the [conversation format](https://devblogs.microsoft.com/dotnet/category/conversations/) again, this time with three of the engineers who work on ready to run code generation.

* [Jan Kotas](https://github.com/jkotas)
* [Jan Vorlíček](https://github.com/janvorli)
* [Tomáš Rylek](https://github.com/trylek)

## What is the problem that ready to run solves. What’s the problem we’d have without it?

**Tomas:** Right now I think it's mostly startup perf without versioning fragility. In the longer run I hope us to shoot for runtime perf too in a more aggressive manner.

**Tomas:** In other words, we're leveraging the native code cache produced by the Crossgen compiler for offloading the JIT-heavy startup phase. Version resiliency constraints sometimes force us to produce suboptimal code though so the expectation is that after startup the code gets rejitted for better steady state perf.

**Jan K:** R2R is a binary format for AOT compiled .NET code. We would not have AOT for .NET without it. That would not work work well since we know that at least some form of AOT is needed for most [.NET runtime form factors](https://github.com/dotnet/designs/blob/main/accepted/2020/form-factors.md). There is simply too much code in typical .NET apps to JIT or interpret it all.

**Tomas:** In fact I reviewed some of these questions and [Simon's answers](https://devblogs.microsoft.com/dotnet/conversation-about-crossgen2/) before and one thing that I think might merit better clarification is the relationship between R2R and full native AOT, it seems to me that Simon's responses touched on that and maybe we can shine more light on that.

## What’s with the name? Why is the format called “ready to run”?

**Jan V:** It means that the code is ready to run right away without jitting.

**Tomas:** I believe this alludes to the version resiliency - as the name suggests, the code should "run anywhere".

**Tomas:** That's probably true too, but even if the versioning constraints are not met, it should still run even though it would drop the R2R image and just use the runtime JIT.

**Jan K:** We needed a name for the file format and Ready to Run sounded like a pretty good name. Vance Morrison came up with this name. It describes the idea behind the file format well: something that can run right away.

**Tomas:** But I guess JanK was the closest to the birth of the name so he probably knows best what was its original intent

## What are the strengths of the format?

**Tomas:** Version resiliency combined with native code for better perf, that's a crucial difference in comparison with previous technologies like NGEN or pure JIT.

**Tomas:** This is a bit more technical detail but in comparison with NGEN the format has also much cleaner design.

**Jan V:** I would say the version resiliency and future extensibility via versioning are the important strengths.

**Tomas:** It is also quite flexible so that it supports a broad range of options with regard to partial vs. full compilation, extensibility, the future option to include multiple versions of compiled functions supporting various processor extensions and the like.

**Tomas:** Among others, it's flexible enough to support future switch-over to OS-native executable images.

**Jan K:** The key characteristics of the file format is version resiliency. It means that updating a dependency in compatible way won't invalidate the AOT compiled code. It makes it behave similar to IL ([ECMA-335](https://github.com/dotnet/runtime/blob/main/docs/project/dotnet-standards.md)) file format when composing and servicing applications that is very flexible and easy to understand. Previous versions of the .NET AOT file formats - such as the file format used by NGen on .NET Framework - did not have this property.

## What are the weaknesses of the format?

**Tomas:** Well, we try to keep fixing the ones we're finding. One downside of the current way it's embedded in the images for single-input compilation is the fact that it basically hangs off the MSIL header. That's what we changed for [composite images](https://github.com/dotnet/runtime/blob/main/docs/design/features/readytorun-composite-format-design.md) to decouple these two.

**Tomas:** It doesn't yet support variant compilations of a single function that would be optimized for various extended instruction sets but it's not super hard to add.

**Jan V:** The fact that it is based on a windows exec file format. We were discussing moving to support unix native formats (ELF, mach-o) too in the past.

**Jan K:** The version resiliency comes with performance penalty caused by, for example, limited inlining. It means the R2R compiled code typically runs a bit slower than JIT compiled code.

**Tomas:** The version resiliency concept and automatic switch-over to runtime JIT upon a mismatch also means that a given R2R app can silently experience a perf degradation that may be hard to understand for an end user.

## What are the observable differences between ready to run and IL?

**Jan V:** Startup performance is better with R2R, file size is larger with it as it contains the native code in addition to the IL.

**Tomas:** For single-file compilations, in theory the only observable aspect should be perf. Large version bubble R2R images can blow up at runtime with a fail fast if someone messes up the dependent assembly versions.

**Tomas:** And the larger size of course as JanV says.

**Jan V:** Also, R2R file is currently target architecture specific.

**Jan K:** Ready to run file format can include number of optional sections. One of these sections can be the IL. Ready to run file file is typically the IL with extra stuff added.

## For someone not familiar with code generation, can you compare ready to run code with the most optimal code? Like how does compiled C++ code work relative to ready-to-run?

**Tomas:** I think this is a pair of orthogonal questions - one is comparing "R2R resilient JIT" to "runtime JIT" and the other is "managed vs. native source code compilation quality". The former is in a way much easier to answer as the latter would require a deep dive into the runtime, typesystem and memory management differences.

**Tomas:** For "R2R resilient JIT" vs. "runtime JIT", I think we've already touched on many aspects of this difference - the inability to inline or use fixed field offsets across the versioning boundary, the inability to optimize for the exact CPU which may support various instruction set extensions.

**Jan V:** Due to the version resiliency, there is an inherent  performance penalty, as Jan K said in the comment on the weaknesses e.g. when using data types from assemblies that are out of the version bubble.

**Jan V:** There is also a benefit compared to the most optimal compiled C++ code - the ability to improve the generated code at runtime using JIT, e.g. by optimizing for the specific CPU architecture or to inline code from other assemblies that were not known at the compilation time.

**Tomas:** That is also true - in general I've been reluctant to go too far w.r.t. "comparing C# with C++" as I believe a million such comparisons must have already been made. But if it's desirable, it's probably worth a new meeting or chat akin to this one.

**Jan K:** The version resilient R2R code has to use extra indirections in number of places. For example, it is not safe to assume that fields have fixed offsets in version resilient ready to run code and the offset has to be fetched via indirection at runtime. C++ does not try to deal with version resiliency. The field offsets are offset fixed in C++ compiled programs.

**Jan K:** An example that is much closer to ready to run is Objective C. Objective C is built as a version resilient system and the patterns used by version resilient R2R code and Objective C code are quite similar. For example, the field accesses in compiled Objective C code require an extra indirection, very much like in R2R code.

**Jan K:** BTW: Objective C version resiliency is what enables a large fraction of APIs in Apple's operating systems to be exposed as Objective C.

**Tomas:** I guess we could split that discussion into blocks on typesystem / class shapes, GC vs. native memory allocation, generics vs. templates, managed code peanut butter, multiple inheritance vs. interface dispatch etc.

## How does R2R compare to full AOT, as exists with the Native AOT project? Similar answer?

**Tomas:** As discussed before, the R2R format is very flexible and supports a range of products ranging from full JIT to full AOT. Currently R2R is certainly not full AOT nor is it intended to be as full native AOT is always limited by the absence of JIT that's required for e.g. Reflection.Emit or for compiling regexes.

**Tomas:** I should probably rather say that current Crossgen / Crossgen2 is not full AOT, not the R2R format in principle.

**Jan K:** We would like to get to a point where both general purpose .NET runtime and NativeAOT use same core file format for AOT code, and just differ in the optional sections that they add. We are not there yet.

## There are multiple other projects right now, like enabling newer SIMD instructions, and PGO, compositive images, and version bubbles. Are they causing the format to change a lot?

## Outside of these low-level runtime features, are there other reasons why ready to run would need to change, like due to supporting Arm64?

## Does the .NET team own the ready to run format? Is there a council that works on that, like one would imagine changing the PNG or FLAC formats?

## Could the ready to run format be used by another dev platform, like Java?

## Is there a point down the round where a new executable format might be needed?

## Closing

Ready to run and the technologies associated with it are good examples of the topics that .NET runtime engineers focus on to make your applications work well. As you can see from the answers, delivering both the fastest code and versionability are at odds, at least on the surface. The team has been finding opportunities to satisfy both needs, including offering the new optional composite mode, which disables versionability. That's great for scenarios that are immutable by construction, like containers.

Thanks again to Tomas, Jan, and Jan for sharing your insights and context on ready to run. It was a great [conversation](https://devblogs.microsoft.com/dotnet/category/conversations/).
