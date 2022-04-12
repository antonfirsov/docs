---
post_title: Announcing .NET 7 Preview 3
username: jondouglas
microsoft_alias: jodou
categories: .NET
featured_image: dotnet7-preview3.jpg
desired_publication_date: 2022-04-12
summary: .NET 7 Preview 3 is now available with enhancements to observability, startup times, codegen, GC regions, native AOT compilation, and more.
---

Today, we are glad to release .NET 7 Preview 3. The third preview of .NET 7 includes enhancements to observability, startup times, codegen, GC regions, native AOT compilation, and more. The bits are available for you to grab *right now* and start experimenting with new features like:

- Native AOT
- Default GC regions
- ASP.NET Core startup time improvements

You can [download .NET 7 Preview 3](https://dotnet.microsoft.com/download/dotnet/7.0), for Windows, macOS, and Linux.

* [Installers and binaries](https://dotnet.microsoft.com/download/dotnet/7.0)
* [Container images](https://hub.docker.com/_/microsoft-dotnet)
* [Linux packages](https://github.com/dotnet/core/blob/master/release-notes/7.0/)
* [Release notes](https://github.com/dotnet/core/tree/master/release-notes/7.0)
* [Known issues](https://github.com/dotnet/core/blob/main/release-notes/7.0/known-issues.md)
* [GitHub issue tracker](https://github.com/dotnet/core/issues)

.NET 7 Preview 3 has been tested with Visual Studio 17.2 Preview 3. We recommend you use the [preview channel builds](https://visualstudio.com/preview) if you want to try .NET 7 with Visual Studio family products. Visual Studio for Mac support for .NET 7 previews isn’t available yet but is coming soon. Now, let's get into some of the latest updates in this release.

### Faster, Lighter Apps with Native AOT
    
In the [.NET 7 Preview 2 blog post](https://devblogs.microsoft.com/dotnet/announcing-dotnet-7-preview-2/#nativeaot-update), we announced that the Native AOT project has been moved out of experimental status and into mainline development in .NET 7 in the dotnet/runtime repo. We know that many of you have been eagerly awaiting updates from the team on what’s coming for Native AOT, and we have a couple of new updates for you for Preview 3. 

If you want details about Native AOT, or to jump in and get started with it, the [repo docs](https://github.com/dotnet/runtime/blob/main/src/coreclr/nativeaot/docs/compiling.md) are the best place for that. 

We also recognize that some of you might not be familiar with what Native AOT is, so we wanted to share a quick overview of it with you. 

#### What is Native AOT?

Ahead-of-time (AOT) compilation refers to an umbrella of technologies which generate code at application build time, instead of run-time. AOT is not new to .NET. Today we ship [ReadyToRun](https://docs.microsoft.com/dotnet/core/deploying/ready-to-run) for client and server scenarios, and Mono AOT for mobile and WASM. Native AOT brings full native pre-compilation to .NET desktop client and server scenarios. Native AOT is not replacing these existing technologies, rather it's offering a new set of capabilities that unlocks new form factors. 

Existing AOT-compiled .NET assemblies contain platform-specific data structures and native code to frontload work typically done at runtime. Precompiling these artifacts saves time at startup (e.g. ReadyToRun), and enables access to no-JIT platforms (e.g. iOS). If precompiled artifacts are not present, .NET either falls back to JIT or interpretation (depending on the platform). 

Native AOT is similar to .NET’s existing AOT technologies, but it produces only native artifacts. In fact, the Native AOT runtime does not know how to read the .NET assembly file formats - everything is platform-native. The executable file format parsing is fully handled by the underlying operating system. 

The main advantage of Native AOT is in startup time, memory usage, accessing to restricted platforms (no JIT allowed), and smaller size on disk. Applications start running the moment the operating system pages in them into memory. The data structures are optimized for running AOT generated code, not for compiling new code at runtime. This is similar to how languages like Go, Swift, and Rust compile. Native AOT is best suited for environments where startup time matters the most. Targeting Native AOT has stricter requirements than general .NET Core/5+ applications and libraries. Native AOT forbids emitting new code at runtime (e.g. Reflection.Emit), and loading new .NET assemblies at runtime (eg. plug-in models).  

#### Prepare your apps for Native AOT

For .NET 7 we are targeting console apps and [native libraries](https://github.com/dotnet/samples/blob/main/core/nativeaot/NativeLibrary/README.md) as the primary scenario for Native AOT. Application developers and library authors can now take advantage of Native AOT by ensuring that their applications are trimmable. Since trimming is a requirement for Native AOT compilation, [preparing your applications and libraries](https://docs.microsoft.com/dotnet/core/deploying/trimming/trim-self-contained) now for trimming will help them get ready for Native AOT as well. If you are an author of any .NET libraries, following the [“Trimming libraries” instructions](https://docs.microsoft.com/dotnet/core/deploying/trimming/prepare-libraries-for-trimming) specifically will help you prepare your libraries for trimming and Native AOT. 

One of the apps that we're planning to ship in .NET 7 compiled with Native AOT is the crossgen tool. Crossgen is part of the .NET SDK. It's the CoreCLR AOT compiler that produces ReadyToRun executables. Crossgen is written in C# and we currently ship it compiled with itself as a ReadyToRun app (it's turtles all the way down!). We're already seeing some very promising numbers in terms of compilation speed and size. Crossgen benefits heavily from Native AOT because it's a short-lived process and the startup overhead dominates the overall execution time: 

|  Scenario           | ReadyToRun  | NativeAOT  |
|---------------------|-------------|------------|
| Compile CoreLib     | 4182 ms     | 3512 ms    |
| Compile HelloWorld  | 185 ms      | 49 ms      |

| Configuration  | Size     |
|----------------|----------|
| ReadyToRun     | 34.8 MB  |
| NativeAOT      | 17.6 MB  |

Looking ahead, Native AOT compatibility will be improved over the next few versions of .NET, however there will always be reasons to prefer JIT for many scenarios. We will also add first-class support in the dotnet SDK for publishing projects with Native AOT.

### Observability

.NET 7 continues to evolve support for the cloud native OpenTelemetry specification. Preview 3 adds support for specification updates [#988](https://github.com/open-telemetry/opentelemetry-specification/pull/988) and [#1708](https://github.com/open-telemetry/opentelemetry-dotnet/issues/1708) that make the trace state mutable for samplers.

- [Allow Trace Samplers to Modify the Activity Trace State](https://github.com/dotnet/runtime/pull/65530)

```C#
    //  ActivityListener Sampling callback
    listener.Sample = (ref ActivityCreationOptions<ActivityContext> activityOptions) =>
    {
        activityOptions = activityOptions with { TraceState = "rojo=00f067aa0ba902b7" };
        return ActivitySamplingResult.AllDataAndRecorded;
    };
```

### System.Composition.Hosting

The latest Managed Extensibility Framework gets a slight update to align with the previous version APIs. The new APIs allow adding a single object instance to the [System.Composition.Hosting container](https://docs.microsoft.com/dotnet/api/system.composition.hosting.containerconfiguration?view=dotnet-plat-ext-6.0). Similar to the functionality provided in the legacy interfaces [System.ComponentModel.Composition.Hosting](https://docs.microsoft.com/dotnet/api/system.componentmodel.composition.hosting?view=dotnet-plat-ext-6.0) with the API  [ComposeExportedValue<T>(CompositionContainer, T)](https://docs.microsoft.com/dotnet/api/system.componentmodel.composition.attributedmodelservices.composeexportedvalue?view=dotnet-plat-ext-6.0#system-componentmodel-composition-attributedmodelservices-composeexportedvalue-1(system-componentmodel-composition-hosting-compositioncontainer-0))

[Proposal: Inject existing object into MEF2](https://github.com/dotnet/runtime/issues/29400)

```C#
namespace System.Composition.Hosting
{
    public class ContainerConfiguration
    {
        public ContainerConfiguration WithExport<TExport>(TExport exportedInstance);
        public ContainerConfiguration WithExport<TExport>(TExport exportedInstance, string contractName = null, IDictionary<string, object> metadata = null);

        public ContainerConfiguration WithExport(Type contractType, object exportedInstance);
        public ContainerConfiguration WithExport(Type contractType, object exportedInstance, string contractName = null, IDictionary<string, object> metadata = null);
    }
}
```
    
### Startup time improvements with Write-Xor-Execute enabled
    
Performance continues to be a major focus for .NET 7. The [dotnet/runtime#65738 PR](https://github.com/dotnet/runtime/pull/65738) reimplemented the precode and call counting stubs (tiered compilation helper stubs) to significantly reduce number of post-creation modifications of executable code in the runtime. This resulted in 10-15% startup time improvements.

As a bonus, this change also resulted in steady state performance improvements (upto 8%) in some microbenchmarks and some ASPNet Benchmarks even without Write-Xor-Execute enabled.

However, there are few regressions resulting from that change too (without Write-Xor-Execute enabled) that will be addressed in the upcoming preview releases. These were observed in the Orchard and Fortunes benchmarks on Intel processors only.
    
### CodeGen

Thanks in a large part to community contributors, Preview 3 features several optimizations and bug fixes to code generation and just-in time (JIT) compilation. Here's an overview of the changes that are available today.

#### Community PRs 

These pull requests were all initiated by community contributors.
    
##### From @clamp03
    
- [Enable Fast Tail Call Optimization for ARM32](https://github.com/dotnet/runtime/pull/66282)
    
##### From @SkiFoD
    
- [Optimize "X & 1 == 1" to "X & 1" (#61412)](https://github.com/dotnet/runtime/pull/62818)
    
##### From @sandreenko
    
- [[crossgen2] Promote single byref aot](https://github.com/dotnet/runtime/pull/65682)
    
##### From @SingleAccretion
    
- [Fix missing zero-offset sequences and add checking](https://github.com/dotnet/runtime/pull/64805)
- [Handle direct addresses for statics in IsFieldAddr](https://github.com/dotnet/runtime/pull/64846)
- [Do not number partial definitions and ARGPLACE nodes](https://github.com/dotnet/runtime/pull/64898)
- [Use SSA def descriptors in copy propagation](https://github.com/dotnet/runtime/pull/65250)
- [ZeroObj assertions](https://github.com/dotnet/runtime/pull/65257)
- [Deduplicate some HWI codegen code](https://github.com/dotnet/runtime/pull/65302)
- [Use push for 8/12 byte struct args on x86](https://github.com/dotnet/runtime/pull/65387)
- [Do not set GLOB_REF for invariant indirections](https://github.com/dotnet/runtime/pull/65709)
- [Do not value number locals on the LHS](https://github.com/dotnet/runtime/pull/65902)
- [Keep the volatility of CLS_VARs in rationalization](https://github.com/dotnet/runtime/pull/65919) 
- [Slightly more aggressive ASG reversal](https://github.com/dotnet/runtime/pull/65920)
- [Fix copy propagation](https://github.com/dotnet/runtime/pull/66070)
- [Count OBJ/BLK as memory uses](https://github.com/dotnet/runtime/pull/66135)
- [Delete compUnsafeCastUsed](https://github.com/dotnet/runtime/pull/66204) 
- [Fix a couple issues with GTF_GLOB_REF setting](https://github.com/dotnet/runtime/pull/66247) 
- [Do not create small constants while morphing cascading addition](https://github.com/dotnet/runtime/pull/66270)
- [Do not propagate RHS flags in block morphing](https://github.com/dotnet/runtime/pull/66291) 
- [Stop generating CLS_VAR for 64 bit targets](https://github.com/dotnet/runtime/pull/66298) 
- [A better fix for #66242](https://github.com/dotnet/runtime/pull/66335)
- [Do not assume containment](https://github.com/dotnet/runtime/pull/66385)
- [Some small copy propagation changes](https://github.com/dotnet/runtime/pull/66582)
    
##### From @trympet
    
- [Treat TZCNT/POPCNT/LZCNT as never negative](https://github.com/dotnet/runtime/pull/64951)
    
##### From @Wraith2
    
- [Add xarch blsi](https://github.com/dotnet/runtime/pull/66193)
- [Add flags checks to BMI1 intrinsic lowering](https://github.com/dotnet/runtime/pull/66736)

### Dynamic PGO
    
- [PGO: Profile isinst/castclass in order to optimize them in Tier 1](https://github.com/dotnet/runtime/pull/65460)

### Arm64
    
- [Morph Vector.Create(0) to Vector.Zero](https://github.com/dotnet/runtime/pull/63821)
- [Allow constant propagation of Vector.Zero.](https://github.com/dotnet/runtime/pull/65028)
- [Optimize Arm64 comparison instructions: cmle, cmlt, fcmle, fcmlt](https://github.com/dotnet/runtime/pull/64783) 
- [Better addressing mode for floating point on arm64](https://github.com/dotnet/runtime/pull/65468)
- [Optimize a % b](https://github.com/dotnet/runtime/pull/65535)
- [JIT: Faster vector == Vector128.Zero on arm64](https://github.com/dotnet/runtime/pull/65632)

### Loop Optimizations
    
- [Loop Cloning](https://github.com/dotnet/runtime/pull/66257) improved the duration of single invocation by 21% for System.Collections.Tests.Perf_BitArray.BitArrayLeftShift(Size: 512):
![image](https://user-images.githubusercontent.com/63486087/161346652-4964b4f8-7bb1-4c7f-acec-abd70d1dde43.png)

### General Optimizations
    
- [Eliminate extra copy of struct from a callee that was returned in Hidden Buffer](https://github.com/dotnet/runtime/pull/64130)
- [Unroll String.Equals and str.StartsWith for constant strings](https://github.com/dotnet/runtime/pull/65288)
- [Extend Equals/StartsWith auto-vectorization for OrdinalIgnoreCase](https://github.com/dotnet/runtime/pull/66095) 
- [movzx optimization after setcc shows 0.03 ~ 0.16 % code size reduction](https://github.com/dotnet/runtime/pull/66245)
    
### GC Regions Enabled by default
    
With Preview 3, regions functionality which should help with memory utilization for high throughput applications has been enabled by default. The functionality is now enabled for all Platforms except MacOS and NativeAOT (which would be enabled in the future). More details are available in this issue: https://github.com/dotnet/runtime/issues/43844

We expect some working set increases for smaller applications due to how regions are initially allocated. If you notice any functional or performance differences please create an issue within the runtime repo. 
    
### Cryptography: Generating X.500 names more robustly

This change simplifies working with certificates by introducing a class that provides more clarity for parsing X.500 names.
    
[Make it safer and easier to build an X500DistinguishedName](https://github.com/dotnet/runtime/issues/44738)

Classically, anyone wanting to build a X.500 name (such as for creating test certificates with the `CertificateRequest` class did so with string manipulation, either via a simple literal or with string formatting, e.g.

```C#
request = new CertificateRequest($"CN={subjectName},OU=Test,O=""Fabrikam, Inc.""", ...);
```

This is generally fine, except for when `subjectName` contains a comma, quote, or anything else that has an influence on the parser.  To address that, we added the `X500DistinguishedNameBuilder` class.  Because every method only operates on a single relative distinguished name (RDN), there's no ambiguity in parsing.  As a bonus, since the RDN identifiers are expanded, you no longer have to guess what "CN" stands for ("Common Name").

```C#
X500DistinguishedNameBuilder nameBuilder = new();
nameBuilder.AddCommonName(subjectName);
nameBuilder.AddOrganizationalUnitName("Test");
nameBuilder.AddOrganizationName("Fabrikam, Inc.");

request = new CertificateRequest(nameBuilder.Build(), ...);
```

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

## Support

.NET 7 is a **Current** release, meaning it will receive free support and patches for 18 months from the release date. It's important to note that the quality of all releases is the same. The only difference is the length of support. For more about .NET support policies, see the [.NET and .NET Core official support policy](https://dotnet.microsoft.com/platform/support/policy/dotnet-core).

## Breaking changes

You can find the most recent list of breaking changes in .NET 7 by reading the [Breaking changes in .NET 7](https://docs.microsoft.com/dotnet/core/compatibility/7.0) document. It lists breaking changes by area and release with links to detailed explanations.

To see what breaking changes are proposed but still under review, follow the [Proposed .NET Breaking Changes GitHub issue](https://github.com/dotnet/core/issues/7131).

## Roadmaps

Releases of .NET include products, libraries, runtime, and tooling, and represent a collaboration across multiple teams inside and outside Microsoft. You can learn more about these areas by reading the product roadmaps:

* [ASP.NET Core 7 and Blazor Roadmap](https://github.com/dotnet/aspnetcore/issues/39504)
* [EF 7 Roadmap](https://docs.microsoft.com/ef/core/what-is-new/ef-core-7.0/plan)
* [ML.NET](https://github.com/dotnet/machinelearning/blob/main/ROADMAP.md)
* [.NET MAUI](https://github.com/dotnet/maui/wiki/Roadmap)
* [WinForms](https://github.com/dotnet/winforms/blob/main/docs/roadmap.md)
* [WPF](https://github.com/dotnet/wpf/blob/main/roadmap.md)
* [NuGet](https://github.com/NuGet/Home/issues/11571)
* [Roslyn](https://github.com/dotnet/roslyn/blob/main/docs/Language%20Feature%20Status.md)
* [Runtime](https://github.com/dotnet/core/blob/main/roadmap.md)

## Closing

We appreciate and [thank you](https://dotnet.microsoft.com/thanks) for your all your support and contributions to .NET. Please [give .NET 7 Preview 3 a try](https://dotnet.microsoft.com/download/dotnet/7.0) and tell us what you think!
