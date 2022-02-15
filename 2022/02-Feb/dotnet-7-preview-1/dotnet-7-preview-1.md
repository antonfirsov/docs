---
post_title: Announcing .NET 7 Preview 1
username: jeremy_likness
microsoft_alias: jeliknes
categories: .NET
featured_image: dotnet7-preview1.jpg
desired_publication_date: 2/17/2022
summary: .NET 7 Preview 1 is now available and is the start of a major .NET release, focused on cloud native, app modernization, containers, and many other improvements.
---

Today, we are excited to announce the next milestone in the history of .NET. [While celebrating the community and 20 years of innovation](https://dotnet.microsoft.com), .NET 7 Preview 1 marks the first step forward towards the next 20 years of .NET.

.NET 7 builds on the foundation established by .NET 6, which includes a unified set of base libraries, runtime, and SDK, a simplified development experience, and high developer productivity. Major areas of focus for .NET 7 include improved support for cloud native scenarios, tools to make it easier to upgrade legacy projects, and simplifying the developer experience by making it easier to work with containers.

.NET 7 Preview 1 includes annotations to APIs to support nullability, ongoing JIT compiler optimizations, new APIs, and support for more hot reload scenarios.

Releases of .NET include products, libraries, runtime, and tooling, and represent a collaboration across multiple teams inside and outside Microsoft. The broader themes covered in this blog post do not encompass all of the key scenarios and investments for .NET 7. They represent large areas but are just a part of all the important work going into .NET 7. We plan to make broad investments in ASP.NET Core, Blazor, EF Core, WinForms, WPF, and other platforms. You can learn more about these areas by reading the product roadmaps:

* [ASP.NET Core 7 and Blazor Roadmap](https://github.com/dotnet/aspnetcore/issues/39504)
* [EF 7 Roadmap](https://docs.microsoft.com/ef/core/what-is-new/ef-core-7.0/plan)
* [ML.NET](https://github.com/dotnet/machinelearning/blob/main/ROADMAP.md)
* [.NET MAUI](https://github.com/dotnet/maui/wiki/Roadmap)
* [WinForms](https://github.com/dotnet/winforms/blob/main/docs/roadmap.md)
* [WPF](https://github.com/dotnet/wpf/blob/main/roadmap.md)
* [NuGet](https://github.com/NuGet/Home/issues/11571)
* [Roslyn](https://github.com/dotnet/roslyn/blob/main/docs/Language%20Feature%20Status.md)
* [Runtime](https://github.com/dotnet/core/blob/main/roadmap.md)

You can [download .NET 7 Preview 1](https://dotnet.microsoft.com/download/dotnet/7.0), for Windows, macOS, and Linux.

* [Installers and binaries](https://dotnet.microsoft.com/download/dotnet/7.0)
* [Container images](https://hub.docker.com/_/microsoft-dotnet)
* [Linux packages](https://github.com/dotnet/core/blob/master/release-notes/7.0/)
* [Release notes](https://github.com/dotnet/core/tree/master/release-notes/7.0)
* [Known issues](https://github.com/dotnet/core/blob/master/release-notes/7.0/7.0-known-issues.md)
* [GitHub issue tracker](https://github.com/dotnet/core/issues)

.NET 7 has been tested with Visual Studio 17.1 Preview 5 and will be coming in a future Visual Studio for Mac preview. We recommend you use those builds if you want to try .NET 7 with Visual Studio family products.

## Cloud Native

Cloud native apps are built from the ground up to take advantage of modern, web-based resources such as [database services](https://docs.microsoft.com/azure/?product=databases) and [hosted containers](https://docs.microsoft.com/azure/?product=containers). The cloud native architecture can improve scale in large applications by creating autonomous subsystems (commonly referred to as _[microservices](https://docs.microsoft.com/dotnet/architecture/microservices/)_) that are deployed and scale independently from other areas of the application while lowering costs in the long term. The microservices architecture is a popular approach because it's flexible and designed to evolve and scale to limits that are difficult to achieve in a monolithic architecture.

.NET 7 will make it easier to build cloud native apps by exploring improvements to the developer experience, such as:

* Simplifying the setup and configuration necessary to implement secure authentication and authorization
* Improving the performance of application startup and runtime execution.

We'll continue to make investments in [Microsoft Orleans](https://dotnet.github.io/orleans/), a .NET cross-platform framework for building distributed applications that has been referred to as "distributed .NET." We'll continue to enhance the comprehensive [documentation for Microsoft Orleans](https://docs.microsoft.com/dotnet/orleans/) and make it easier to use and implement by improving integration of Orleans with existing cloud services like [Azure App Services](https://docs.microsoft.com/azure/app-service/) and [Azure Container Apps](https://docs.microsoft.com/azure/container-apps/overview).

## Modernize .NET apps

Since the release of .NET 6, developers have been upgrading their applications to take advantage of new performance gains, productivity features like minimal APIs and hot reload, new runtime and C# language innovations and the availability of a mature ecosystem of libraries and tools. In .NET 7, we'll continue to enable you to bring your existing .NET apps forward to the latest .NET platforms and technologies. More analyzers, code fixers, and support for additional app types in the [.NET Upgrade Assistant](https://dotnet.microsoft.com/platform/upgrade-assistant) will help you confidently upgrade even more of your application portfolio and spend less time on the repetitive tasks involved in upgrading.

We also know that each of the .NET platforms (ASP.NET, WinForms, WPF, etc.) have their own unique challenges with modernization and may be lacking functionality that you need as a developer or support in the platform itself. For some .NET platforms like WCF, there may not be a clear direction for you. We'll focus on offering appropriate guidance, documentation, and tooling to make these .NET platforms more seamless to upgrade to.

## Containers

Containers are the preferred way to deploy cloud native apps and microservices for many companies today. Relying on containers presents several challenges including managing compliance, building and publishing images, securing images, and streamlining the size and performance of images. We believe that there is an opportunity to create a better experience with .NET containers.

To help customers face these challenges, we plan to make significant improvements to .NET development with containers in .NET 7. For example, we'll explore building containers as part of `msbuild` instead of relying on Docker desktop and the Docker CLI. This will remove Docker as a prerequisite so build environments only need to install the .NET SDK to build container images. We plan to enhance telemetry to improve the observability of containers. We'll also focus on making our container images smaller, faster, and more secure while we explore highly requested models such as rootless and distroless.

## Support

.NET 7 is a **Current** release, meaning it will receive free support and patches for 18 months from the release date. It's important to note that the quality of all releases is the same. The only difference is the length of support. For more about .NET support policies, see the [.NET and .NET Core official support policy](https://dotnet.microsoft.com/platform/support/policy/dotnet-core).

## Breaking changes

You can find the most recent list of breaking changes in .NET 7 by reading the [Breaking changes in .NET 7](https://docs.microsoft.com/dotnet/core/compatibility/7.0) document. It lists breaking changes by area and release with links to detailed explanations.

To see what breaking changes are proposed but still under review, follow the [Proposed .NET Breaking Changes GitHub issue](https://github.com/dotnet/core/issues/7131).

## Preview 1

The following features are now available in the Preview 1 release.

### Nullable annotations for Microsoft.Extensions

We have been making progress on annotating the Microsoft.Extensions.* libraries for nullability. In .NET 7 Preview 1, the following libraries have been annotated for nullability:

* Microsoft.Extensions.DependencyInjection.Abstractions
* Microsoft.Extensions.Logging.Abstractions
* Microsoft.Extensions.Primitives
* Microsoft.Extensions.FileSystemGlobbing
* Microsoft.Extensions.DependencyModel
* Microsoft.Extensions.Configuration.Abstractions
* Microsoft.Extensions.FileProviders.Abstractions
* Microsoft.Extensions.FileProviders.Physical
* Microsoft.Extensions.Configuration
* Microsoft.Extensions.Configuration.Binder
* Microsoft.Extensions.Configuration.CommandLine
* Microsoft.Extensions.Configuration.EnvironmentVariables
* Microsoft.Extensions.Configuration.FileExtensions
* Microsoft.Extensions.Configuration.Ini
* Microsoft.Extensions.Configuration.Json

By the time .NET 7 is released, we plan on annotating all the Microsoft.Extensions.* libraries for nullability. You can see the remaining libraries, and follow the progress at [dotnet/runtime#43605](https://github.com/dotnet/runtime/issues/43605).

A huge thank you to @maxkoshevoi who has been contributing the bulk of this effort. Without @maxkoshevoi's help, we wouldn't be nearly as far as we are.

### Observability

Continue improving the tracing APIs:

* Adding the overload to `ActivityContext.TryParse` allows parsing and creating an `ActivityContext` object including if the activity context was propagated from a remote parent ([related issue](https://github.com/dotnet/runtime/issues/42575)).
* Adding the method `Activity.IsStopped()` to indicate whether the `Activity` object is stopped([related issue](https://github.com/dotnet/runtime/issues/63353)).

### CodeGen

Community PRs (many thanks to JIT community contributors!!)

From @am11

* [Convert some old style intrinsics to NamedIntrinsic runtime#62271](https://github.com/dotnet/runtime/pull/62271)

From @anthonycanino

* [Add additional binary operations into the RangeCheck analysis. runtime#61662](https://github.com/dotnet/runtime/pull/61662)

From @SeanWoo

* [[JIT] [Issue: 61620] Optimizing ARM64 for *x = dblCns; runtime#61847](https://github.com/dotnet/runtime/pull/61847)

From @SingleAccretion

* [Tune floating-point CSEs live across a call better runtime#63903](https://github.com/dotnet/runtime/pull/63903)
* [Update hash of the new CSE when resizing runtime#61984](https://github.com/dotnet/runtime/pull/61984)
* [Rewrite selection for fields and always normalize SIMD types in VN runtime#61370](https://github.com/dotnet/runtime/pull/61370)
* [Add documentation on how VN numbers memory runtime#60476](https://github.com/dotnet/runtime/pull/60476)
* [Improve value numbering for casts runtime#59841](https://github.com/dotnet/runtime/pull/59841)
* [Address-expose locals under complex local addresses in block morphing runtime#63100](https://github.com/dotnet/runtime/pull/63100)
* [Handle embedded assignments in copy propagation runtime#63447](https://github.com/dotnet/runtime/pull/63447)
* [Exception sets: debug checker & fixes runtime#63539](https://github.com/dotnet/runtime/pull/63539)
* [Implement the "moffset" encoding size optimization in emitOutputAM runtime#62896](https://github.com/dotnet/runtime/pull/62896)
* [Compress operand kinds array and optimize OperIsLocal runtime#63253](https://github.com/dotnet/runtime/pull/63253)
* [Make gtHasRef pay attention to LCL_FLD nodes runtime#62568](https://github.com/dotnet/runtime/pull/62568)
* [Enable global constant propagation for GT_LCL_FLD runtime#61209](https://github.com/dotnet/runtime/pull/61209)
* [Enable global constant propagation for small types runtime#57726](https://github.com/dotnet/runtime/pull/57726)
* [Properly type primary selectors in fgMemoryVNForLoopSideEffects runtime#60505](https://github.com/dotnet/runtime/pull/60505)

From @RalfKornmannEnvision

* [CoreRT support for ARM64&Unix runtime#41023](https://github.com/dotnet/runtime/pull/41023)

From @weilinwa

* [Optimize FMA codegen base on the overwritten runtime#58196](https://github.com/dotnet/runtime/pull/58196)

#### Dynamic PGO

* [OSR support for Arm64](https://github.com/dotnet/runtime/pull/62831)
* [JIT: support OSR for synchronized methods](https://github.com/dotnet/runtime/pull/61712)
* [JIT: handle interaction of OSR, PGO, and tail calls](https://github.com/dotnet/runtime/pull/62263)
* [Add 2009 Jit Architecture Plan (excerpts)](https://github.com/dotnet/runtime/pull/60939)
* [JIT: limited version of forward substitution for some relops](https://github.com/dotnet/runtime/pull/61023)
* [JIT: save generics context for late devirtualization](https://github.com/dotnet/runtime/pull/63420)

#### Arm64

* [Arm64: Memory barrier improvements](https://github.com/dotnet/runtime/pull/62895)
![Arm64: Memory barrier improvements](https://user-images.githubusercontent.com/63486087/150900463-6065f6ba-114f-4a1d-ba5f-355d02b05d1a.png)
* [Use SIMD operations in InitBlkUnroll/CopyBlkUnroll and increase unroll limit up to 128 bytes](https://github.com/dotnet/runtime/pull/61030)
* [[Arm64] Keep unrolling InitBlock and CopyBlock up to 128 bytes](https://github.com/dotnet/runtime/pull/63422)
* ['cmeq' and 'fcmeq' Vector64<T>.Zero/Vector128<T>.Zero ARM64 containment optimizations](https://github.com/dotnet/runtime/pull/62933)
* [[arm64] JIT: X % 2 == 0 -> X & 1 == 0](https://github.com/dotnet/runtime/pull/62399)
* [[arm64] JIT: Add with sign/zero extend](https://github.com/dotnet/runtime/pull/61549)
* [[arm64] JIT: Enable CSE/hoisting for "arrayBase + elementOffset"](https://github.com/dotnet/runtime/pull/61293)
* [[arm64] JIT: Fold "A * B + C" to MADD/MSUB](https://github.com/dotnet/runtime/pull/61037)

#### Loop Optimizations

* [Generalize loop pre-header creation and loop hoisting](https://github.com/dotnet/runtime/pull/62560)
* [Loop refactoring and commenting improvements](https://github.com/dotnet/runtime/pull/61496)

#### General Optimizations

* [Accelerate additional cross platform hardware intrinsics](https://github.com/dotnet/runtime/pull/61649)
* [Implement Narrow and Widen using SIMDAsHWIntrinsic](https://github.com/dotnet/runtime/pull/60094)
* [Add IsKnownConstant jit helper and optimize 'str == ""' with str.StartsWith('c')](https://github.com/dotnet/runtime/pull/63734)
* [Allow JIT to keep HFA/HVA in the registers when passing them as argument/returning values](https://github.com/dotnet/runtime/pull/62623)
* [Enable support for nint/nuint for Vector64/128/256<T>](https://github.com/dotnet/runtime/pull/63329)
* [Adding support for X86Base.Pause() and ArmBase.Yield()](https://github.com/dotnet/runtime/pull/61065)
* [Use preferred region from PAL for JIT reloc hints](https://github.com/dotnet/runtime/pull/60747)
* [Support fast tailcalls in R2R](https://github.com/dotnet/runtime/pull/56669)
* [Allow contained indirections in tailcalls on x64](https://github.com/dotnet/runtime/pull/58686)
* [Optimize indirection cell call sequences more generally](https://github.com/dotnet/runtime/pull/59602)
* [Avoid additional local created for delegate invocations](https://github.com/dotnet/runtime/pull/63796)

### Interop: p/Invoke code generation

We integrated the p/invoke source generator that was prototyped in .NET 6 into dotnet/runtime and have been converting the runtime libraries to use it. This means the converted p/invokes are AOT-compatible and no longer require an IL stub to be generated at runtime.

We intend to make the p/invoke source generator available for use outside the runtime in the future. You can follow our remaining work in [dotnet/runtime#60595](https://github.com/dotnet/runtime/issues/60595).

### New APIs in System.Text.Json

System.Text.Json ships with a couple of minor quality-of-life enhancements:

* Developers now have access to the default `JsonSerializerOptions` singleton used internally by System.Text.Json ([related issue](https://github.com/dotnet/runtime/pull/61434)).
* Add a `JsonWriterOptions.MaxDepth` property and ensure this value flows from the equivalent `JsonSerializerOptions.MaxDepth` property on serialization ([related issue](https://github.com/dotnet/runtime/pull/61608)).
* Add `Patch` methods to `System.Net.Http.Json` ([related issue](https://github.com/dotnet/runtime/issues/60531)).

### Hot Reload in Mono

The following edits are now allowed in C# hot reload for Blazor WebAssembly and .NET for iOS and Android ([related issue](https://github.com/dotnet/runtime/pull/63513)):
* Adding static lambdas to existing methods
* Adding lambdas that capture this to existing methods that already have at least one lambda that captures this
* Adding new static or non-virtual instance methods to existing classes
* Adding new static fields to existing classes
* Adding new classes

Known issues:
* Instance fields in newly added classes are not supported
* Newly added methods and fields in existing or new classes are not visible to reflection

You can follow our progress in [dotnet/runtime#57365](https://github.com/dotnet/runtime/issues/57365)

## Targeting .NET 7

To target .NET 7, you need to use a .NET 7 Target Framework Moniker (TFM) in your project file. For example:

```xml
<TargetFramework>net7.0</TargetFramework>
```

The full set of .NET 7 TFMs, including operating-specific ones follows.

* `net7.0`
* `net7.0-android`
* `net7.0-ios`
* `net7.0-maccatalyst`
* `net7.0-macos`
* `net7.0-tvos`
* `net7.0-windows`

We expect that upgrading from .NET 6 to .NET 7 should be straightforward. Please report any breaking changes that you discover in the process of testing existing apps with .NET 7.

## Closing

A global and diverse team of engineers at Microsoft in collaboration with a highly engaged community of developers are building .NET 7. The broad .NET community, including everyone from students and hobbyists to open source contributors and enterprise customers, is at the heart of .NET and proposes new ideas and code changes regularly that drive the .NET ecosystem forward. We appreciate and [thank you](https://dotnet.microsoft.com/thanks) for your support, contributions and insights and will continue to work with and contribute alongside the .NET open source community through the .NET Foundation.

Welcome to .NET 7.
