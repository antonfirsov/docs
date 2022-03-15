---
post_title: Announcing .NET 7 Preview 2 - The New, 'New' Experience
username: angelosp
microsoft_alias: angelpe
categories: .NET
featured_image: dotnet7-preview2.jpg
desired_publication_date: 2022-03-15
summary: .NET 7 Preview 2 is now available with enhancements to RegEx source generators, progress moving NativeAOT into the runtime, and a major set of improvements to the 'dotnet new' CLI experience.
---

Today, we are glad to release .NET 7 Preview 2. The second preview of .NET 7 includes enhancements to RegEx source generators, progress moving NativeAOT from experimental status into the runtime, and a major set of improvements to the "dotnet new" CLI experience. The bits are available for you to grab *right now* and start experimenting with new features like:

- Build a specialized RegEx pattern matching engine using source generators at compile-time rather than slower methods at runtime.
- Take advantage of SDK improvements that provide an entirely new, streamlined tab completion experience to explore templates and parameters when running `dotnet new`.
- Don't trim your excitement, just your apps in preparation to try out NativeAOT with your own innovative solutions.

You can [download .NET 7 Preview 2](https://dotnet.microsoft.com/download/dotnet/7.0), for Windows, macOS, and Linux.

* [Installers and binaries](https://dotnet.microsoft.com/download/dotnet/7.0)
* [Container images](https://hub.docker.com/_/microsoft-dotnet)
* [Linux packages](https://github.com/dotnet/core/blob/master/release-notes/7.0/)
* [Release notes](https://github.com/dotnet/core/tree/master/release-notes/7.0)
* [Known issues](https://github.com/dotnet/core/blob/main/release-notes/7.0/known-issues.md)
* [GitHub issue tracker](https://github.com/dotnet/core/issues)

.NET 7 Preview 2 has been tested with Visual Studio 17.2 Preview 1. We recommend you use the [preview channel builds](https://visualstudio.com/preview) if you want to try .NET 7 with Visual Studio family products. Visual Studio for Mac support for .NET 7 previews isn’t available yet but is coming soon.

## Preview 2

The following features are now available in the Preview 2 release.

### Introducing the new Regex Source Generator

https://github.com/dotnet/runtime/issues/44676

Have you ever wished you had all of the great benefits that come from having a specialized Regex engine that is optimized for your particular pattern, without the overhead of building this engine at runtime? We are excited to announce the new Regex Source Generator which was included in Preview 1. It brings all of the performance benefits from our compiled engine without the startup cost, and it has additional benefits, like providing a great debugging experience as well as being trimming-friendly. If your pattern is known at compile-time, then the new regex source generator is the way to go.

In order to start using it, you only need to turn the containing type into a `partial` one, and declare a new partial method with the `RegexGenerator` attribute that will return the optimized `Regex` object, and that's it! The source generator will fill the implementation of that method for you, and will get updated automatically as you make changes to your pattern or to the additional options that you pass in. Here is an example:

**Before**

```c#
public class Foo
{
  public Regex regex = new Regex(@"abc|def", RegexOptions.IgnoreCase);

  public bool Bar(string input)
  {
    bool isMatch = regex.IsMatch(input);
    // ..
  }
}
```

**After**

```c#
public partial class Foo  // <-- Make the class a partial class
{
  [RegexGenerator(@"abc|def", RegexOptions.IgnoreCase)] // <-- Add the RegexGenerator attribute and pass in your pattern and options
  public static partial Regex MyRegex(); //  <-- Declare the partial method, which will be implemented by the source generator

  public bool Bar(string input)
  {
    bool isMatch = MyRegex().IsMatch(input); // <-- Use the generated engine by invoking the partial method.
    // ..
  }
}
```

And that's it. Please try it out and let us know if you have any feedback.

### CodeGen

Community PRs (Many thanks to JIT community contributors!!)

From @sandreenko

* [[Jit] Delete Statement::m_compilerAdded . runtime#64506](https://github.com/dotnet/runtime/pull/64506 )

From @SingleAccretion

* [Delete GT_DYN_BLK runtime#63026](https://github.com/dotnet/runtime/pull/63026)
* [Address-expose locals under complex local addresses in block morphing runtime#63100](https://github.com/dotnet/runtime/pull/63100 )
* [Refactor optimizing morph for commutative operations runtime#63251](https://github.com/dotnet/runtime/pull/63251 )
* [Preserve OBJ/BLK on the RHS of ASG runtime#63268](https://github.com/dotnet/runtime/pull/63268 )
* [Handle embedded assignments in copy propagation runtime#63447](https://github.com/dotnet/runtime/pull/63447 )
* [Do not set GTF_NO_CSE for sources of block copies runtime#63462](https://github.com/dotnet/runtime/pull/63462 )
* [Exception sets: debug checker & fixes runtime#63539](https://github.com/dotnet/runtime/pull/63539 )
* [Stop using CLS_VAR for "boxed statics" runtime#63845](https://github.com/dotnet/runtime/pull/63845)
* [Tune floating-point CSEs live across a call better runtime#63903](https://github.com/dotnet/runtime/pull/63903 )
* [Reverse ASG(CLS_VAR, ...) runtime#63957](https://github.com/dotnet/runtime/pull/63957)
* [Fix invalid threading of nodes in rationalization runtime#64012](https://github.com/dotnet/runtime/pull/64012)
* [Add the exception set for ObjGetType runtime#64106](https://github.com/dotnet/runtime/pull/64106)
* [Improve fgValueNumberBlockAssignment runtime#64110](https://github.com/dotnet/runtime/pull/64110)
* [Commutative morph optimizations runtime#64122](https://github.com/dotnet/runtime/pull/64122)
* [Fix unique VNs for ADDRs runtime#64230](https://github.com/dotnet/runtime/pull/64230 )
* [Copy propagation tweaking runtime#64378](https://github.com/dotnet/runtime/pull/64378)
* [Introduce GenTreeDebugOperKind runtime#64498](https://github.com/dotnet/runtime/pull/64498 )
* [Add support for TYP_BYREF LCL_FLDs to VN runtime#64501](https://github.com/dotnet/runtime/pull/64501 )
* [Do not add NRE sets for non-null addresses runtime#64607](https://github.com/dotnet/runtime/pull/64607)
* [Take into account zero-offset field sequences when propagating locals runtime#64701](https://github.com/dotnet/runtime/pull/64701 )
* [Another size estimate fix for movs runtime#64826](https://github.com/dotnet/runtime/pull/64826 )
* [Mark promoted SIMD locals used by HWIs as DNER runtime#64855](https://github.com/dotnet/runtime/pull/64855 )
* [Propagate exception sets for assignments runtime#64882](https://github.com/dotnet/runtime/pull/64882 )
* [Account for HWI stores in LIR side effects code runtime#65079](https://github.com/dotnet/runtime/pull/65079 )

From @Wraith2

* [Recognize BLSR in "x & (x-1)" Add blsr runtime#63545](https://github.com/dotnet/runtime/pull/63545 )
* [Add xarch andn runtime#64350](https://github.com/dotnet/runtime/pull/64350 )

### Dynamic PGO

* [JIT: simple forward substitution pass runtime#63720](https://github.com/dotnet/runtime/pull/63720 )

### Arm64

* [Local heap optimizations on Arm64 runtime#64481](https://github.com/dotnet/runtime/pull/64481 )
* [Couple optimization to MultiRegStoreLoc runtime#64857](https://github.com/dotnet/runtime/pull/64857 )
* [Implement LoadPairVector64 and LoadPairVector128 runtime#64864](https://github.com/dotnet/runtime/pull/64864 )
* ['cmeq' and 'fcmeq' Vector64<T>.Zero/Vector128<T>.Zero ARM64 containment optimizations runtime#62933](https://github.com/dotnet/runtime/pull/62933)
* [Increase arm32/arm64 maximum instruction group size runtime#65153](https://github.com/dotnet/runtime/pull/65153 )
* [Prefer "mov reg, wzr" over "mov reg, #0" runtime#64740](https://github.com/dotnet/runtime/pull/64740)
* [Biggen GC Gen0 for Apple M1 Fix LLC cache issue on Apple M1 runtime#64576](https://github.com/dotnet/runtime/pull/64576)
* [Optimize full memory barriers around volatile reads/writes ARM64: Avoid LEA for volatile IND runtime#64354](https://github.com/dotnet/runtime/pull/64354 )
* [Optimize Math.Round(x, MidpointRounding.AwayFromZero/ToEven) runtime#64016](https://github.com/dotnet/runtime/pull/64016 )

### General Optimizations

* [Security: Add JIT support for control-flow guard on x64 and arm64 runtime#63763](https://github.com/dotnet/runtime/pull/63763)
* [Testing: Spmi replay asmdiffs mac os arm64 runtime#64119](https://github.com/dotnet/runtime/pull/64119 )

### Observability

* [Support Metrics UpDownCounter instrument runtime#63648](https://github.com/dotnet/runtime/issues/63648)

```C#
static Meter s_meter = new Meter("MyLibrary.Queues", "1.0.0");
static UpDownCounter<int> s_queueSize = s_meter.CreateUpDownCounter<int>("Queue-Size");
static ObservableUpDownCounter<int> s_pullQueueSize = s_meter.CreateObservableUpDownCounter<int>("Queue-Size", () => s_pullQueueSize);

...

s_queueSize.Add(10);
s_queueSize.Add(-2);
```

### Logging source generator improvements

* [Logging source generator should gracefully fail when special parameters incorrectly get passed as template parameter runtime#64310](https://github.com/dotnet/runtime/issues/64310)
* [Logging Source Generator fails to compile when in parameter modifier is present runtime#62644](https://github.com/dotnet/runtime/issues/62644)
* [Logging Source Generator fails to compile using keyword parameters with @ prefixes runtime#60968](https://github.com/dotnet/runtime/issues/60968)
* [Logging Source Generator fails ungracefully with overloaded methods runtime#61814](https://github.com/dotnet/runtime/issues/61814)
* [Logging Source Generator fails to compile due to CS0246 and CS0265 errors if type for generic constraint is in a different namespace runtime#58550](https://github.com/dotnet/runtime/issues/58550)

### SDK Improvements

[[Epic] New CLI parser + tab completion #2191](https://github.com/dotnet/templating/issues/2191)

**`dotnet new` syntax and tab completion**

For `7.0.100-preview2`, the `dotnet new` command has been given a more consistent and intuitive interface for many of the subcommands that users already use. In addition, support for tab completion of template options and arguments has been massively updated, now giving rapid feedback on valid arguments and options as the user types.

Here's the new help output as an example:

```C#
❯ dotnet new --help
Description:
  Template Instantiation Commands for .NET CLI.

Usage:
  dotnet new [<template-short-name> [<template-args>...]] [options]
  dotnet new [command] [options]

Arguments:
  <template-short-name>  A short name of the template to create.
  <template-args>        Template specific options to use.

Options:
  -?, -h, --help  Show command line help.

Commands:
  install <package>       Installs a template package.
  uninstall <package>     Uninstalls a template package.
  update                  Checks the currently installed template packages for update, and install the updates.
  search <template-name>  Searches for the templates on NuGet.org.
  list <template-name>    Lists templates containing the specified template name. If no name is specified, lists all templates.
```

**New Command Names**

Specifically, all of the _commands_ in this help output no longer have the `--` prefix that they do today. This is more in line with what users expect from subcommands in a CLI application. The old versions (`--install`, etc) are still available to prevent breaking user scripts, but we hope to add obsoletion warnings to those commands in the future to encourage migration.

**Tab Completion**

The `dotnet` CLI has supported tab completion for quite a while on popular shells like PowerShell, bash, zsh, and fish (for instructions on how to enable that, see [How to enable TAB completion for the .NET CLI](https://docs.microsoft.com/dotnet/core/tools/enable-tab-autocomplete)). It's up to individual `dotnet` commands to implement meaningful completions, however. For .NET 7, the `new` command learned how to provide tab completion for
* Available template names (in `dotnet new <template-short-name>`)

```shell
❯ dotnet new angular
angular              grpc                 razor                viewstart            worker               -h
blazorserver         mstest               razorclasslib        web                  wpf                  /?
blazorwasm           mvc                  razorcomponent       webapi               wpfcustomcontrollib  /h
classlib             nugetconfig          react                webapp               wpflib               install
console              nunit                reactredux           webconfig            wpfusercontrollib    list
editorconfig         nunit-test           sln                  winforms             xunit                search
gitignore            page                 tool-manifest        winformscontrollib   --help               uninstall
globaljson           proto                viewimports          winformslib          -?                   update
```

* Template options (the list of template options in the `web` template)

```shell
❯ dotnet new web --dry-run
--dry-run                  --language                 --output                   -lang
--exclude-launch-settings  --name                     --type                     -n
--force                    --no-https                 -?                         -o
--framework                --no-restore               -f                         /?
--help                     --no-update-check          -h                         /h
```

* Allowed values for those template options (choice values on an choice template argument)

```shell
❯ dotnet new blazorserver --auth Individual
Individual     IndividualB2C  MultiOrg       None           SingleOrg      Windows
```

There are a few known gaps in completion - for example, `--language` doesn't suggest installed language values.

**Future work**

In future previews, we plan to continue filling gaps left by this transition, as well as make enabling completions either automatic or as simple as a single command that the user can execute. We hope that this will make improvements in tab completion across the entire `dotnet` CLI more broadly used by the community!

**What's next**

`dotnet new` users - go enable tab completion and try it for your templating use! Template authors - try out tab completion for the options on your templates and make sure you're delivering the experiences you want your users to have. Everyone - raise any issues you find on the [dotnet/templating](https://github.com/dotnet/templating) repo and help us make .NET 7 the best release for `dotnet new` ever!

### NativeAOT Update

We previously announced that we're [moving the NativeAOT project](https://github.com/dotnet/runtime/issues/61231) out of experimental status and into mainline development in .NET 7. Over the past few months, we've been heads down doing the coding to move NativeAOT out of the experimental [dotnet/runtimelab repo](https://github.com/dotnet/runtimelab) and into the [dotnet/runtime](https://github.com/dotnet/runtime) repo. That work has now been completed, but we have yet to add first-class support in the dotnet SDK for publishing projects with NativeAOT. We hope to have that work done shortly, so you can try out NativeAOT with your apps. In the meantime, please try [trimming your app](https://docs.microsoft.com/dotnet/core/deploying/trimming/trim-self-contained) and ensure there are no trim warnings. Trimming is a requirement of NativeAOT. If you own any libraries there are also instructions for [preparing libraries for trimming](https://docs.microsoft.com/dotnet/core/deploying/trimming/prepare-libraries-for-trimming).

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

We appreciate and [thank you](https://dotnet.microsoft.com/thanks) for your all your support and contributions to .NET. Please give .NET 7 Preview 2 a try and tell us what you think!
