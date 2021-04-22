---
post_title: Conversation about crossgen2
username: rlander@microsoft.com
microsoft_alias: rlander@microsoft.com
categories: .NET Core, .NET
desired_publication_date: 04/22/2021
summary: Conversation with .NET engineers about crossgen2.
---

[Crossgen2](https://devblogs.microsoft.com/dotnet/announcing-net-6-preview-1/#crossgen2) is an exciting new platform addition and part of the [.NET 6](https://devblogs.microsoft.com/dotnet/announcing-net-6-preview-1/) release. It is a new tool that enables both generating and optimizing code in a new way.

The crossgen2 project is a significant effort, and is the focus of multiple engineers. I thought it might be interesting to try a more conversational approach to exploring new features. I sent a set of questions to the team. [Simon Nattress](https://github.com/nattress) offered to tell us more about crossgen2. Let's see what he said. I'll provide my own thoughts, too.

## What is crossgen for and when should it be used?

**Simon:** Crossgen is a tool that provides ahead-of-time (AOT) compilation for your code so that the need for JITing at runtime is reduced. When publishing your application, Crossgen runs the JIT over all assemblies and stores the JITted code in an extra section that can be quickly fetched at runtime. Crossgen should be used in scenarios where fast startup is important.

**Rich:** You might see [crossgen](https://github.com/dotnet/runtime/blob/main/docs/workflow/building/coreclr/crossgen.md) and [readytorun](https://github.com/dotnet/runtime/blob/main/docs/design/coreclr/botr/readytorun-overview.md) terms used interchangeably. Crossgen is a tool that generates native code (at least today) in the readytorun format. The readytorun format is primarily oriented on being compatible across assemblies, and having the same compatibility guarantee as IL, while offering the performance benefits of ahead-of-time compiled code. Starting with crossgen2, it has some other modes with other characteristics.

## Why are we making a new version of crossgen? What are our goals? 

**Simon:** Crossgen’s pedigree comes from the early .NET Framework days. Its implementation is tightly coupled with the runtime (it essentially is just the runtime and JIT attached to a PE file emitter). We are building a new version of Crossgen – Crossgen 2 – which starts with a new code base architected to be a compiler that can perform analysis and optimizations not possible with the previous version.

**Rich:** As the .NET Core project became more mature and we saw usage grow across multiple application scenarios, we realized that crossgen's limitation of only really being able to produce native code of one flavor with one set of characteristics was going to be a big problem. For example, we might want to generate code with different characteristics for Windows desktop on one hand and Linux containers on the other. The need for that level of code generation diversity is what motivated the project.

## Is crossgen -> crossgen2 similar to the native code csc -> managed Roslyn transition? How long has it been worked on?

**Simon:** The Roslyn transition to managed was not just a rewrite in a different language. It defined an analysis platform for using CSC as an API. It can be used as a compiler and as a source code analyzer in an editor. Similarly, Crossgen2 is not simply a rewrite in managed. The architecture uses a graph to drive analysis and compilation. This allows scanners, optimizers, analyzers to all work off a common representation of the assembly being compiled. The project has been worked on for 2 years – the origins of the Crossgen2 compiler began as a research project around 2016.

**Rich:** We have a lot of people on the team that primarily write C/C++ (even assembly), but most people like writing C# better and are more productive. Every release, more of the product gets moved to C# for this and other reasons.

## What are the key benefits and also the drawbacks from writing crossgen in C#?

**Simon:** Writing in C# gives us access to a rich set of .NET APIs as well as memory safety guarantees provided by using a managed language. A drawback of using C# is increased runtime when using Crossgen2 on many small assemblies at once because of the overhead of starting the runtime many times. Fortunately, we can mitigate much of that by running Crossgen2 on itself!

**Rich:** It is also super helpful being on the same team as the folks adding new capabilities to C# and .NET libraries. There is a lot of shared thinking and collaboration on low-level scenarios to enable C# to be a high-performance language. The more challenges we run into to make low-level code fast, the more we add features to fix that. It's a virtuous cycle.

## Can you describe some of the projects that are planned that are made possible with crossgen2?

**Simon:** Crossgen2 (unlike native Crossgen) allows us to analyze and compile multiple assemblies at once as a single servicing unit with extra optimizations allowed within the compile set.

**Rich:** [Version bubbles](https://github.com/dotnet/runtime/blob/main/docs/design/features/crossgen2-compilation-structure-enhancements.md) is the feature that Simon is referring to, and is one of my favorite new features. By default, readytorun code is versionable, and that's a great characteristic. I work a lot on containers and they have a key characteristic of immutability, which makes versionability  unimportant. Version bubbles trade versionability for performance. That's perfect for scenarios like containers where you'd much prefer greater performance and don't have to give anything up for it. I'm looking forward to offering more nuanced and opinionated code in scenarios where it makes sense.

**Rich:** Versionability is a big topic, but I feel the need to expand on it a little. Let's start with the [book of the runtime](https://github.com/dotnet/runtime/blob/main/docs/design/coreclr/botr/readytorun-overview.md#version-bubbles). *"When changes to managed code are made, we have to make sure that all the artifacts in a native code image only depend on information in other modules that cannot change without breaking the compatibility rules. What is interesting about this problem is that the constraints only come into play when you cross module boundaries."* Inlining is the perfect example. Methods can be inlined within the same assembly (equivalent to "module") because the method being inlined and the method it is being inlined into reside within the same compatibility boundary. You cannot update one without updating the other. If you inline across assemblies boundaries, then the original code (that was inlined) could change and then a performance optimization is now exhibiting functionally incorrect behavior. That's very bad. Version bubbles enable redefining the version boundary, but it is up to you to maintain that contract, and it isn't a .NET code generation bug if you don't.

**Rich:** Cross-compilation is another really important feature. You'll be able to produce native code for Arm64 on an x64 machine and vice versa. For example, when you want to generate Arm64 code on an x64 machine, the SDK will acquire the Arm64 RyuJIT compiled for x64 so that it will run on an x64 machine. Cross-compilation is a key tenet of the architecture.

## Could crossgen2 ever be used to target a runtime other than CoreCLR? For example, to enable the native AOT form factor?

**Simon:** Yes – much of the current Crossgen2 code is shared with the [NativeAOT](https://github.com/dotnet/runtimelab/tree/feature/NativeAOT) project which targets a different runtime. The managed type system implementation has been designed with extension points to allow for this flexibility.

## What’s with the name? What’s the name you would prefer and why?

**Simon:** Crossgen originally started life as a cross-architecture AOT code generator for Windows Phone.

**Rich:** At one point, I tried to rename the tool "genr2r", like "generator" but "r2r" at the end for "ready-to-run" but no one else was keen on that idea. At this point, I'm hoping that we'll revert to just calling the tool "crossgen" after we've dropped our use of the existing crossgen tool.

## Closing

First, thanks Simon for taking some time to tell us all about crossgen2. We also appreciate all your efforts on crossgen2. Simon has since moved to the [Cosmos DB](https://azure.microsoft.com/services/cosmos-db/) team. They use .NET, too!

While many of you will not use crossgen2 directly, you will certainly take advantage of the .NET platform being more optimized with this new tool. Going forward, crossgen2 will enables us even more options to make higher performance choices for the platform and for your code.

This post was the first one that I've posted in a conversational style. Did you like it? Should we do this again? If so, which topics should we have a conversation about next?
