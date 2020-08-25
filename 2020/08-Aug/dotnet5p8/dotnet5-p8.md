# Announcing .NET 5.0 Preview 8

Today, we are releasing .NET 5.0 Preview 8. The .NET 5.0 release is now "feature complete", meaning that very nearly all features are in their final form (with the exception of bug fixes still to come). Preview 8 is, appropriately, the last preview. We plan on releasing two go-live release candidates before the final .NET 5.0 release in November. This post describes a selection of features across the .NET 5.0 release. 

We also released new versions of ASP.NET Core and EF Core today.

You can [download .NET 5.0](https://dotnet.microsoft.com/download/dotnet/5.0), for Windows, macOS, and Linux:

* [Installers and binaries](https://dotnet.microsoft.com/download/dotnet/5.0)
* [Container images](https://hub.docker.com/_/microsoft-dotnet)
* [Snap installer](https://snapcraft.io/dotnet-sdk)
* [Release notes](https://github.com/dotnet/core/tree/master/release-notes/5.0)
* [Known issues](https://github.com/dotnet/core/blob/master/release-notes/5.0/5.0-known-issues.md)
* [GitHub issue tracker](https://github.com/dotnet/core/issues)

You need the latest version of [Visual Studio](https://visualstudio.microsoft.com) (including [Visual Studio for Mac)](https://visualstudio.microsoft.com/vs/mac/) to use .NET 5.0.

.NET 5.0 includes [many improvements](https://github.com/dotnet/runtime/issues/37269), notably [single file applications](https://github.com/dotnet/runtime/issues/36590), [smaller container images](https://github.com/dotnet/dotnet-docker/issues/1814#issuecomment-625294750), more capable [JsonSerializer APIs](https://github.com/dotnet/runtime/issues/41313), a complete set of [nullable reference type annotations](https://twitter.com/terrajobst/status/1296566363880742917), and support for [Windows ARM64](https://github.com/dotnet/runtime/issues/36699). [Performance has been greatly improved](https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-5/), in the NET libraries, in the GC, and the JIT. [ARM64](https://github.com/dotnet/runtime/issues/35853) was a key focus for performance investment, resulting in much better throughput and smaller binaries. .NET 5.0 includes new language versions, [C# 9](https://devblogs.microsoft.com/dotnet/welcome-to-c-9-0/) and [F# 5.0](https://devblogs.microsoft.com/dotnet/announcing-f-5-preview-1/).

.NET 5.0 also includes support for [Web Assembly](https://webassembly.org/), using the Mono runtime and the .NET Libraries. This is the foundation of Blazor Web Assembly in .NET 5.0. This is a change from [Blazor 3.2](https://devblogs.microsoft.com/aspnet/blazor-webassembly-3-2-0-now-available/), which used the Mono runtime and Mono libraries. Last year, we presented a [vision of a unified .NET platform](https://devblogs.microsoft.com/dotnet/introducing-net-5/), with one set of libraries and tools for all .NET app types. The benefits of this change are a single development experience for .NET, much higher compatibility between the various .NET app types and maintaining and improving just one code base. The change we made with Web assembly, to use the .NET libraries, is a down-payment on that vision. We expect to deliver the rest of the vision, largely focused on Xamarin (iOS and Android), with .NET 6.0. 

Now that the release is feature-complete, let's take a look at a subset of what's coming in .NET 5.0. The post is composed of a set of thematic sections: Languages, Tools, APIs, Runtime technology and Application deployment. These sections, and their order, roughly reflect the development process and lifecycle. This will help you find the content you want if you are more interested in one development aspect than another.

## Languages

C# 9 and F# 5 are part of the .NET 5.0 release and included in the .NET 5.0 SDK. Visual Basic is also included in the 5.0 SDK. It does not include language changes, but has improvements to support the Visual Basic Application Framework on .NET Core.

[C#  Source Generators](https://devblogs.microsoft.com/dotnet/introducing-c-source-generators/) are an important new C# compiler feature that isn't technically part of C# 9 since it doesn't have any language syntax. See [New C# Source Generator Samples](https://devblogs.microsoft.com/dotnet/new-c-source-generator-samples/) to help you get started using this new feature.

### C# 9

[C# 9](https://devblogs.microsoft.com/dotnet/welcome-to-c-9-0/) is a [significant release of the language](https://github.com/dotnet/roslyn/blob/master/docs/Language%20Feature%20Status.md#c-9), focused on program simplicy, [data immutability](https://twitter.com/davidfowl/status/1283236341195501568) and more patterns. In this post, we'll stick to what might be the crowd favorites.

#### Top-level programs

[Top-level programs](https://twitter.com/davidfowl/status/1295792080216657921) offer a simpler syntax for programs, with much less ceremony. This syntax will help people learning the language in the first place. We expect the top-level program syntax to get simpler in subsequent releases, like the removal of default `using` statements.

The following is the C# 9 version of "hello world".

```csharp
using System;

Console.WriteLine("Hello World!");
```

Top-level programs can be extended to use more functionality, such as defining and calling a method or class within the same file.

```csharp
using System;
using System.Runtime.InteropServices;

Console.WriteLine("Hello World!");
FromWhom();
Show.Excitement("Top-level programs can be brief, and can grow as slowly or quickly in complexity as you'd like", 8);

void FromWhom()
{
    Console.WriteLine($"From {RuntimeInformation.FrameworkDescription}");
}

internal class Show
{
    internal static void Excitement(string message, int levelOf)
    {
        Console.Write(message);

        for (int i = 0; i < levelOf; i++)
        {
            Console.Write("!");
        }

        Console.WriteLine();
    }
}
```

This program produces the following output.

```console
[rich@taumarunui test]$ ~/dotnet/dotnet run
Hello World!
From .NET 5.0.0-preview.8
Top-level programs can be brief, and can grow as slowly or quickly in complexity as you'd like!!!!!!!!
```

#### Pattern matching

Patterns test that a value has a certain shape, and can extract information from the value when it has the matching shape. New pattern matching improvements have been added in the [last few versions of C#](https://devblogs.microsoft.com/dotnet/whats-new-in-csharp-7-0/).

I'll share two examples. The first demonstrates [property patterns](https://twitter.com/jaredpar/status/1288547233718087680). It [checks for null](https://twitter.com/a_tessenr/status/1291755101087051777) (with `is`) before comparing the `context` object to a [specific pattern](https://twitter.com/jaredpar/status/1293334174334476288).

```csharp
if (context is {IsReachable: true, Length: > 1 })
{
    Console.WriteLine(context.Name);
}
```

This is equivalent to:

```csharp
if (context is object && context.IsReachable && context.Length > 1 )
{
    Console.WriteLine(context.Name);
}
```
Also equivalent to:

```csharp
if (context?.IsReachable && context?.Length > 1 )
{
    Console.WriteLine(context.Name);
}
```

The following example uses relational patterns (like `<`, `<=`), and logical ones (like `and`, `or` and `not`). The following code produces the highway toll (as a decimal) for a delivery truck based on its gross weight. For those not familiar, `m` after a numeric literal tells the compiler than the number is a `decimal` and not a `double`.

```csharp
DeliveryTruck t when t.GrossWeightClass switch
{
    < 3000 => 8.00m,
    >= 3000 and <= 5000 => 10.00m,
    > 5000 => 15.00m,
},
```

#### Target-typed `new` expressions

[Target-typed `new` expresisons](https://github.com/dotnet/csharplang/blob/master/proposals/csharp-9.0/target-typed-new.md) is a new way to remove type duplication when constructing objects/values.

The following examples are all equivalent, with the new syntax being the middle one.

```csharp
List<string> values = new List<string>();
List<string> values = new();
var values = new List<string>();
```

Many people will prefer this new syntax over `var` (I'm guessing) for two reasons: many people read left-to-right and want the type information on the left hand side of `=`, and (likely more importantly) the left side is solely dedicated to type information and not distracted by the complexity or nuance of a particular constructor (on the right-hand side).

### F# 5

Building on [updates](https://devblogs.microsoft.com/dotnet/f-5-and-f-tools-update-for-june/) [to](https://devblogs.microsoft.com/dotnet/f-5-update-for-net-5-preview-4/) [the F# 5 preview released earlier this year](https://devblogs.microsoft.com/dotnet/announcing-f-5-preview-1/), this final update to F# 5 wraps up language support and adds two new features, Interpolated Strings and Open Type Declarations. Here's a sneak peek at what you can do with them:

#### Interpolated Strings
Interpolated strings in F# are one of the most highly-requested features. People familiar with them in C# and JavaScript (and perhaps other languages) have come to love them in those languages. In F#, we're introducing interpolated strings in an untyped fashion as they exist in other languages, but also with typed interpolated holes where the expression in an interpolated context must match a type given by a string format specifier.
```fsharp
// Basic interpolated string
let name = "Phillip"
let age = 29
let message = $"Name: {name}, Age: {age}"

// Typed interpolation
// '%s' requires the interpolation to be of type string
// '%d' requires the interpolation to be an integer
let message2 = $"Name: %s{name}, Age: %d{age}"
```

```html
<script src="https://gist.github.com/cartermp/6d226b59533ef3617c8f9d13eea20498.js"></script>
```
#### Open Type Declarations
Open Type Declarations in F# are similar to the ability to open static classes in C#, but with a slightly different syntax. They are also extended to allow opening F# specific types.
```fsharp
open type System.Math

let x = Min(1.0, 2.0)

module M =
    type DU = A | B | C

    let someOtherFunction x = x + 1

// Open only the type inside the module
open type M.DU

printfn "%A" A // Prints 'A'
```

```html
<script src="https://gist.github.com/cartermp/b2a30361927427314ab781c1bb30be98.js"></script>
```

Stay tuned for a blog post that goes over the details.

## Tools

In this post, we're going to focus on runtime diagnostic tools.

### `Microsoft.Extensions.Logging`

We've made improvements to the console log provider in the `Microsoft.Extensions.Logging` library. Developers can now implement a [custom `ConsoleFormatter`](https://github.com/dotnet/runtime/issues/34742) to exercise complete control over formatting and colorization of the console output. The formatter APIs allow for rich formatting by implementing a subset of the `VT-100` (supported by most modern terminals) escape sequences. The console logger can parse out escape sequences on unsupported terminals allowing you to author a single formatter for all terminals.

In addition to support for custom formatters, we've also added a built-in JSON formatter that emits structured JSON logs to the console.

### Dump debugging

Debugging managed code requires special knowledge of managed objects and constructs. The Data Access Component (DAC) is a subset of the runtime execution engine that has knowledge of these constructs and can access these managed objects without a runtime. Beginning in Preview 8, we've started to compile the Linux DAC against Windows. .NET Core process dumps collected on Linux can now be analyzed on Windows using WinDBG or `dotnet dump analyze`.

In Preview 8, we've also added support for capturing ELF dumps from .NET processes running on macOS. Since ELF is not the native executable (native debuggers like `lldb` will not work with these dumps) file format on macOS, we have made this an opt-in feature. To enable support for dump collection on macOS, set the environment variable `COMPlus_DbgEnableElfDumpOnMacOS=1`. The resulting dumps can be analyzed using `dotnet dump analyze`.

### Assembly load diagnostics added to event pipe

We added assembly load information to event pipe. You can think of this as a replacement to the [Fusion Log Viewer](https://docs.microsoft.com/dotnet/framework/tools/fuslogvw-exe-assembly-binding-log-viewer). You can now use [dotnet-trace]( https://docs.microsoft.com/dotnet/core/diagnostics/dotnet-trace) to collect this information, using the following command:

```console
dotnet-trace collect --providers Microsoft-Windows-DotNETRuntime:4:4 --process-id [process ID]
```

The workflow is described in [Trace Assembly Loading with Event Pipe]( https://github.com/richlander/testapps/tree/master/trace-assembly-loading).

## APIs

Many new APIs were added and improved in .NET 5.0. The following are important changes top be aware of.

### Nullable Annotations

[Nullable reference types](https://devblogs.microsoft.com/dotnet/embracing-nullable-reference-types/) was an important feature of C# 8 and .NET Core 3.0. It was released with a lot of promise, but was missing exhaustive platform annotations to make it truly useful and practical. The wait is (largely) over. The [platform is now 80% annotated](https://twitter.com/terrajobst/status/1296566363880742917) for nullability. We're looking into whether we can annotate the remaining 20% before we ship .NET 5.0 RTM. If not, we're going to finish the remaining annotations early in .NET 6.0.

The following image illustrates the progress we've made over time.

![image](https://user-images.githubusercontent.com/2608468/91111023-42b3f800-e634-11ea-8f21-8876f541f876.png)

This also means that your existing .NET Core 3.1 code might generate new diagnostics (if you have nullability enabled) when you retarget it to .NET 5.0. If that happens, you can thank us for helping you avoid nulls.

### Regular expression performance improvements

We’ve invested in [significant improvements to the Regex engine](https://devblogs.microsoft.com/dotnet/regex-performance-improvements-in-net-5/). On many of the expressions we’ve tried, these improvements routinely result in throughput improvements of 3-6x, and in some cases, much more. The changes we've made in `System.Text.RegularExpressions` have had measurable impact on our own uses, and we hope these improvements will result in measurable wins in your libraries and apps, as well.

### .NET 5.0 Target Framework

We are changing the approach we use for [target frameworks with .NET 5.0](https://github.com/dotnet/designs/blob/master/accepted/2020/net5/net5.md). The following project file demonstrate the new .NET 5.0 target framework.


```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net5.0</TargetFramework>
  </PropertyGroup>

</Project>
```

The new `net5.0` form is more compact and intuitive than the `netcoreapp3.1` style we've used until this point. In addition, we are extending the target framework to model operating systems. This change is motivated by our vision to enable targeting iOS and Android with Xamarin in .NET 6.0. 

Windows desktop APIs will only be available when targeting `net5.0-windows`. You can specify an operating system version, like `net5.0-windows7` or `net5.0-windows10.17763` ([October 2018 Update](https://en.wikipedia.org/wiki/Windows_10_version_history)). You need to target a Windows 10 version if you want to use [WinRT APIs](https://github.com/dotnet/runtime/issues/35318). 

Summary of changes:

* `net5.0` is the new Target Framework Moniker (TFM) for .NET 5.0. 
* `net5.0` combines and replaces `netcoreapp` and `netstandard` TFMs.
* `net5.0` supports [.NET Framework compatibility mode](https://docs.microsoft.com/en-us/dotnet/core/porting/third-party-deps#net-framework-compatibility-mode)
* `net5.0-windows` will be used to expose Windows-specific functionality, like Windows Forms and WPF.
* .NET 6.0 will use the same approach, with `net6.0` and will add `net6.0-ios` and `net6.0-android`.
* The OS-specific TFMs can include [OS version numbers](https://github.com/dotnet/designs/blob/master/accepted/2020/minimum-os-version/minimum-os-version.md), like `net6.0-ios14`.
* Portable APIs, like ASP.NET Core and Xamarin.Forms, will be usable with `net5.0`.

## WinRT Interop (Breaking Change)

We have moved to a [new model for supporting WinRT APIs as part of .NET 5.0](https://github.com/dotnet/runtime/issues/35318). This includes calling APIs (in either direction; CLR <==> WinRT), marshaling of data between the two type systems, and unification of types that are intended to be treated the same across the boundary (i.e. "projected types"; [`IEnumerable<T>`](https://docs.microsoft.com/dotnet/api/system.collections.generic.ienumerable-1) and [`IIterable<T>`](https://docs.microsoft.com/uwp/api/windows.foundation.collections.iiterable-1) are examples).

We will rely on a [new set of WinRT tools](https://github.com/microsoft/CsWinRT) provided by the WinRT team in Windows that will generate C#-based WinRT interop assemblies.

There are several benefits to the new WinRT interop system:

* It can be developed and improved separate from the .NET runtime.
* Symmetrical with interop systems provided for other OSes, like iOS and Android.
* Can take advantage of many other .NET features (AOT, C# features, IL linking).
* [Simplifies](https://github.com/dotnet/runtime/pull/36715) the .NET runtime codebase.

 The [existing WinRT interop system has been removed](https://github.com/dotnet/runtime/issues/37672) from the .NET runtime (and any other associated components) as part of .NET 5.0. This is a breaking change. That means that apps using WinRT with .NET Core 3.x will need to be rebuilt and will not run on .NET 5.0 as-is.

### Printing environment information

As .NET has extended support for new operating systems and chip architectures, people sometimes want a way to print environment information. We created a simple .NET tool that does this, called [dotnet-runtimeinfo](https://www.nuget.org/packages/dotnet-runtimeinfo/).

You can install and run the tool with the following commands.

```console
dotnet tool install -f dotnet-runtimeinfo
dotnet-runtimeinfo
```

The tool produces output in the following form for your environment.

```console

[rich@taumarunui ~]$ dotnet-runtimeinfo
**.NET information
Version: 5.0.0
FrameworkDescription: .NET 5.0.0-preview.8
Libraries version: 5.0.0-preview.8
Libraries hash: 977a00fb9d587a6c1292fb3af3992038ddcd3016

**Environment information
OSDescription: Linux 5.8.3-2-MANJARO-ARM #1 SMP Sat Aug 22 21:00:07 CEST 2020
OSVersion: Unix 5.8.3.2
OSArchitecture: Arm64
ProcessorCount: 6

**CGroup info**
cfs_quota_us: -1
memory.limit_in_bytes: 9223372036854771712
memory.usage_in_bytes: 1199210496
```

## Runtime Technology

Many new features were added in .NET 5.0. A small selection is described below.
 
### Event pipe profiler APIs

Event pipe is a new subsystem and API that we [added in .NET Core 2.2](https://devblogs.microsoft.com/dotnet/announcing-net-core-2-2/) to make it possible to [perform performance and other diagnostic investigations](https://github.com/dotnet/runtime/blob/master/docs/project/linux-performance-tracing.md) on any operating system. In .NET 5.0, the event pipe has been extended to enable profilers to write event pipe events. This scenario is critical for instrumenting profilers that previously relied on ETW to monitor application behavior and performance.

## Native exports

You can now export managed methods to native code. The building block of the feature is [hosting API support](https://github.com/dotnet/runtime/blob/3247a54a4263dc2a492b740223b6f062672f70d7/src/installer/corehost/cli/coreclr_delegates.h#L22) for [UnmanagedCallersOnlyAttribute](https://github.com/dotnet/runtime/blob/master/src/libraries/System.Private.CoreLib/src/System/Runtime/InteropServices/UnmanagedCallersOnlyAttribute.cs). 

[Aaron Robinson](https://github.com/AaronRobinsonMSFT), on our team, has been working on a [.NET Native Exports](https://github.com/AaronRobinsonMSFT/DNNE) project that provides a more complete experience for publishing .NET components as native libraries. We're looking for feedback on this capability to help decide if the approach should be included in the product in a later release.

There are existing projects that enable similar scenarios, such as:

* [Unmanaged Exports](https://sites.google.com/site/robertgiesecke/Home/uploads/unmanagedexports)
* [DllExport](https://github.com/3F/DllExport)

## Application deployment

After writing or updating an application, you need to [deploy it](https://docs.microsoft.com/dotnet/core/deploying/) for your users to take advantage of. In this release, we've focused on single file applications, and improve ClickOnce for .NET Core.

### Single file applications

[Single file applications](https://github.com/dotnet/runtime/issues/36590) are published and deployed as a single file. The app and its dependencies are all included within that file. When the app is run, the dependencies are loaded directly from that file into memory. There is no performance penalty with this approach. When combined with assembly trimming and ahead-of-time compilation, single file apps are smaller and startup quickly.

In .NET 5.0, single file apps are primarily focused on Linux (more on that later). They can be either framework-dependent or self-contained. Framework-dependent single file apps can be very small, by relying on a globally-installed .NET runtime. Self-contained single-file apps are larger (due to carrying the runtime), but do not require installation of the .NET runtime as an installation pre-step and will just work as a result. In general, framework-dependent is good for development and enterprise environments, while self-contained is often a better choice for ISVs.

We produced a version of single-file apps with .NET Core 3.1. It packages binaries into a single file for deployment and then unpacks those files to a temporary directory to load and execute them. There may be some scenarios where this approach is better, but we expect that the solution we've built for 5.0 will be preferred and a welcome improvement.

We had multiple hurdles to overcome to create a true single-file solution. We had to create a more sophisticated application bundler, teach the runtime to load assemblies out of binary resources, and make the debugger compatible with memory-mapped assemblies. We also ran into some hurdles that we could not clear.

On all platforms, we have a component called "apphost". This is the file that becomes your executable, for example `myapp.exe` on Windows or `./myapp` on Unix-based platforms. For single file apps we created a new host we call "superhost". It has the same role as the regular apphost, but also includes a statically-linked copy of the runtime. The superhost is a fundamental design point of our our single file approach. This model is the one we use on Linux. We were not able to implement this approach on Windows or macOS, due to various operating system constraints. We do not have a superhost on Windows or macOS. On those operating systems, the native runtime binaries (~3 of them) sit beside the single file app. We will revisit this situation in .NET 6.0, however, we expect the problems we ran into to remain challenging. 

You can use the following commands to produce single-file apps.

- Framework-dependent single-file app: 
   - `dotnet publish -r linux-x64 --self-contained false /p:PublishSingleFile=true`
- Self-contained single-file app with assembly trimming and ready to run enabled: 
   - `dotnet publish -r linux-x64 --self-contained true /p:PublishSingleFile=true /p:PublishTrimmed=true /p:PublishReadyToRun=true`

You can also configure single file publishing with a project file.

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net5.0</TargetFramework>
    <!-- Enable single file -->
    <PublishSingleFile>true</PublishSingleFile>
    <!-- Determine self-contained or framework-dependent -->
    <SelfContained>true</SelfContained>
    <!-- The OS and CPU type you are targeting -->
    <RuntimeIdentifier>linux-x64</RuntimeIdentifier>
    <!-- Enable use of assemby trimming - only supported for self-contained apps -->
    <PublishTrimmed>true</PublishTrimmed>
    <!-- Enable AOT compilation -->
    <PublishReadyToRun>true</PublishReadyToRun>
  </PropertyGroup>

</Project>
```

Notes:

* Apps are OS and architecture-specific. You need to publish for each configuration (Linux x64, Linux ARM64, Windows x64, ...).
* Configuration files (like `*.runtimeconfig.json`) are included in the single file. You can place an additional config file beside the single file, if needed (possibly for testing).
* `.pdb` files are not included in the single file by default. You can enable PDB embedding with the `<DebugType>embed</DebugType>` property.

We've seen a lot of comments on previous preview posts asking about the relationship between single file apps and ahead of time (AOT) compilation. AOT is a spectrum. The ready-to-run code that `dotnet publish` generates (when you set `PublishReadyToRun` to true) is an example of AOT. When you publish ready-to-run images, the build generates machine code for you, ahead of time, instead of the JIT doing it at runtime. Most people will likely accept this as a definition of AOT. However, many people mean something more specific when they say AOT. They want a solution that has the following characteristics: extremely fast startup, no IL present (for size and obfuscation reasons), a JIT is (at most) optional, and binary size is as small as it can be. We use the term "native AOT" to describe that point on the AOT spectrum. The single file solution we have in .NET 5.0 doesn't satisfy this definition of AOT. It's a big step forward, but it isn't "native AOT". We recently published a [survey on Native AOT](https://github.com/dotnet/runtime/issues/40430) to get more feedback on that modality. We're looking through the results now and will include them in our 6.0 planning effort. 

### Reducing the size of container images

We are always looking for opportunities to make .NET container images smaller and easier to use. We [re-based the SDK image on top of the ASP.NET image](https://github.com/dotnet/dotnet-docker/pull/1848) instead of [buildpack-deps](https://hub.docker.com/_/buildpack-deps) to dramatically reduces the size of the aggregate images you pull in multi-stage build scenarios

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

We made similar changes for [Alpine and Nano Server](https://github.com/dotnet/dotnet-docker/pull/1832). There is no `buildpack-deps` image for either Alpine or Nano Server. However, the `sdk` images for Alpine and Nano Server were not previously built on top of the ASP.NET image. We fixed that. You will see significant size wins for Alpine and Nano Server as well with 5.0, for multi-stage builds.

### ClickOnce Support

We announced a few months ago that we were going to provide [ClickOnce support for .NET Core](https://github.com/dotnet/deployment-tools/issues/9). This project is still ongoing. We hope to deliver it as part of RC2. I just wanted to share that we're still working on this project.

## Closing

"Closing" is a funny section title at this point in the release. The release is indeed closing. The team is focused on closing out all of remaining 5.0 issues and getting the final bug fixes and improvements into the release. Even the [5.0 Runtime Epics issue](https://github.com/dotnet/runtime/issues/37269#issuecomment-679638638) has been closed.

We're working on some deep-dive posts that we plan to publish before the final release on various topics. Look out for those. You can also expect an even longer post for the final release that covers a much broader set of improvements and features.

Thanks for all the support on the release and for all the contributions.
