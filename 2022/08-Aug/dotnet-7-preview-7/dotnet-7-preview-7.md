---
post_title: Announcing .NET 7 Preview 7
microsoft_alias: jeliknes
author1: jeremy_likness
post_slug: announcing-dotnet-7-preview-7
categories: .NET
featured_image: dotnet7preview7.png
desired_publication_date: 2022-08-09
summary: .NET 7 Preview 7 is now available with improvements to System.LINQ, Unix file permissions, low-level structs, p/Invoke source generation, code generation, and websockets. 
---

Today we released .NET 7 Preview 7. This is the last preview for .NET 7 and the next version will be our first release candidate (RC). The dates for [.NET Conf 2022](https://dotnetconf.net) have been announced! Join us November 8-10, 2022 to celebrate the .NET 7 release!

Visual Studio 2022 17.3 also released today with GA support for .NET Multi-platform App UI (MAUI). Read the [.NET MAUI announcement](https://aka.ms/maui/vs-release) and tune into [.NET Conf: Focus on MAUI](https://focus.dotnetconf.net) that is live streaming now! 

This preview of .NET 7 includes improvements to System.LINQ, Unix file permissions, low-level structs, p/Invoke source generation, code generation, and websockets. 

You can [download .NET 7 Preview 7](https://dotnet.microsoft.com/download/dotnet/7.0), for Windows, macOS, and Linux.

* [Installers and binaries](https://dotnet.microsoft.com/download/dotnet/7.0)
* [Container images](https://mcr.microsoft.com/catalog?search=dotnet/)
* [Linux packages](https://github.com/dotnet/core/blob/master/release-notes/7.0/)
* [Release notes](https://github.com/dotnet/core/tree/master/release-notes/7.0)
* [Known issues](https://github.com/dotnet/core/blob/main/release-notes/7.0/known-issues.md)
* [GitHub issue tracker](https://github.com/dotnet/core/issues)

.NET 7 Preview 7 has been tested with Visual Studio 17.4 Preview 1. We recommend you use the [preview channel builds](https://visualstudio.com/preview) if you want to try .NET 7 with Visual Studio family products. If you're on macOS, we recommend using the latest [Visual Studio 2022 for Mac preview](https://visualstudio.microsoft.com/vs/mac/preview/). Now, let's get into some of the latest updates in this release.

## Simplified ordering with `System.LINQ`

[dotnet/runtime#67194](https://github.com/dotnet/runtime/issues/67194)

`System.Linq` now has the methods `Order` and `OrderDescending`, which are there to order an `IEnumerable` according to `T`.

`IQueryable` also supports this now.

> Note: This change does _not_ introduce a new language feature to `System.Linq.Expressions`.

### Usage

Previously, you had to call `OrderBy`/`OrderByDescending` by referencing the own value.

```csharp
var data = new[] { 2, 1, 3 };
var sorted = data.OrderBy(static e => e);
var sortedDesc = data.OrderByDescending(static e => e);
```

Now, you can write:

```csharp
var data = new[] { 2, 1, 3 };
var sorted = data.Order();
var sortedDesc = data.OrderDescending();
```

## Support for Unix file modes

[dotnet/runtime PR#69980](https://github.com/dotnet/runtime/pull/69980)

Previously, .NET had no built-in support for getting and setting Unix file permissions, which control which users can read, write, and execute files and directories. P/Invoking manually to syscalls isn't always easy because some are exposed differently on different distros. For example, on Ubuntu you may have to pinvoke to `__xstat`, on RedHat to `stat`, and so on. This makes a first-class .NET API important.

In Preview 7, we introduced a new enum:

```c#
public enum UnixFileMode
{
    None,
    OtherExecute, OtherWrite, OtherRead,
    GroupExecute, GroupWrite, GroupRead,
    UserExecute, UserWrite, UserRead,
     ...
}
```

and APIs `File.GetUnixFileMode` and `File.SetUnixFileMode` that get and set the file mode on either a path or a handle (file descriptors). As well as a new property on `FileInfo` and `DirectoryInfo` named `UnixFileMode`.

There is also a new overload of `Directory.CreateDirectory` and a new property on `FileStreamOptions` to allow you to create a directory or file with a particular mode in one shot. Note that when you use these, `umask` is still applied, as it would if you created the directory or file in your shell.

### Usage 

```C#
// Create a new directory with specific permissions
Directory.CreateDirectory("myDirectory", UnixFileMode.UserRead | UnixFileMode.UserWrite | UnixFileMode.UserExecute);

// Create a new file with specific permissions
FileStreamOptions options = new()
{
    Access = FileAccess.Write,
    Mode = FileMode.Create,
    UnixCreateMode =  UnixFileMode.UserRead | UnixFileMode.UserWrite,
};
using FileStream myFile = new FileStream("myFile", options);

// Get the mode of an existing file
UnixFileMode mode = File.GetUnixFileMode("myFile");

// Set the mode of an existing file
File.SetUnixFileMode("myFile", UnixFileMode.UserRead | UnixFileMode.UserWrite | UnixFileMode.UserExecute);
```

See [all new Unix File Mode APIs](https://github.com/dotnet/runtime/pull/69980/files#diff-cec8e6f471b4193246bdc0107b0dd7cbe131fb7fd189b288b37269c333d1171d).

A big thank you goes out to [@tmds](https://github.com/tmds), a long-term contributor from Red Hat, who proposed, designed, and implemented this feature.

## Low-level `struct` improvements: `ref` field support

The .NET 7 runtimes now have full support for `ref` fields within [ByRefLike](https://docs.microsoft.com/dotnet/api/system.type.isbyreflike) types (that is, `ref struct`). There was extensive language design behind this much requested feature that users can read about: [low-level struct improvements](https://github.com/dotnet/csharplang/blob/main/proposals/low-level-struct-improvements.md). With this feature, types previously requiring specialized handling in the runtimes (for example, `Span<T>` and `ReadOnlySpan<T>`), can now be fully implemented in C#.

## `LibraryImport` P/Invoke source generator

[dotnet/runtime#60595](https://github.com/dotnet/runtime/issues/60595)

The `LibraryImport` source generator is now available in a supported manner to all users. The culmination of more than 18 months this source generator is designed to be a drop-in replacement for the majority of `DllImport` uses, both in the runtime product and in user code. The .NET libraries have all adopted `LibraryImport` and have been shipping with source generated marshalling code since [.NET 7 Preview 1](https://github.com/dotnet/core/issues/7106#issuecomment-1021816362).

The source generator ships with the .NET 7 TFM and is readily available for consumption. In order to get the benefit of the source generated marshalling, replace usages of `DllImport` with `LibraryImport`. There are Analyzers and Fixers that can assist with this process.

### Usage

**Before**

```csharp
public static class Native
{
    [DllImport(nameof(Native), CharSet = CharSet.Unicode)]
    public extern static string ToLower(string str);
}
```

**After**

```csharp
public static class Native
{
    [LibraryImport(nameof(Native), StringMarshalling = StringMarshalling.Utf16)]
    public static partial string ToLower(string str);
}
```

There is an analyzer and code-fix to automatically convert your `DllImport` attributes to `LibraryImport`. For Preview 7, it is opt-in. Add `dotnet_diagnostic.SYSLIB1054.severity = suggestion` to your EditorConfig file to enable the conversion analyzer as a diagnostic.

Design documentation and details on marshalling custom types can be found under [`docs/design/libraries/LibraryImportGenerator`](https://github.com/dotnet/runtime/tree/main/docs/design/libraries/LibraryImportGenerator).

## ClientWebSocket upgrade response details

[dotnet/runtime#25918](https://github.com/dotnet/runtime/issues/25918)

`ClientWebSocket` previously did not provide any details about upgrade response. However, the information about response headers and status code might be important in both failure and success scenarios.

In case of failure, the status code can help to distinguish between retriable and non-retriable errors (server doesn't support web sockets at all vs. just a tiny transient error). Headers might also contain additional information on how to handle the situation.

The headers are also helpful even in case of a successful web socket connect, e.g., they can contain a token tied to a session, some info related to the subprotocol version, or the server can go down soon, etc.

### Usage

```C#
ClientWebSocket ws = new();
ws.Options.CollectHttpResponseDetails = true;
try
{
    await ws.ConnectAsync(uri, default);
    // success scenario
    ProcessSuccess(ws.HttpResponseHeaders);
    ws.HttpResponseHeaders = null; // clean up (if needed)
}
catch (WebSocketException)
{
    // failure scenario
    if (ws.HttpStatusCode != null)
    {
        ProcessFailure(ws.HttpStatusCode, ws.HttpResponseHeaders);
    }
}
```

## Improvements in CodeGen

Many thanks to JIT community contributors for these community PRs!

- [@MichalPetryka](https://github.com/MichalPetryka) fixed the issue [#71632](https://github.com/dotnet/runtime/issues/71632) in [PR #71633](https://github.com/dotnet/runtime/pull/71633) Make getTypeForPrimitiveValueClass treat different signs as different types.
- [@shushanhf](https://github.com/shushanhf) made [2 fixes](https://github.com/dotnet/runtime/pulls?q=is%3Apr+is%3Aclosed+label%3Aarea-CodeGen-coreclr+closed%3A2022-06-23..2022-07-11+author%3Ashushanhf+) in LoongArch64.
- [@singleaccretion](https://github.com/singleaccretion) made [20 PR contributions](https://github.com/dotnet/runtime/pulls?q=is%3Apr+is%3Aclosed+label%3Aarea-CodeGen-coreclr+closed%3A2022-06-23..2022-07-11+author%3Asingleaccretion+) during Preview 7.
- [@SkiFoD](https://github.com/SkiFoD) optimized to use Min/Max intrinsics if one of arguments is constant ([PR #69434](https://github.com/dotnet/runtime/pull/69434)).
- For **Arm664**, [@a74nh](https://github.com/a74nh) implemented the first part of [#67894](https://github.com/dotnet/runtime/pull/67894) in [PR #71616](https://github.com/dotnet/runtime/pull/71616) that generates csel and ccmp for conditional comparison and selection instructions (issue [#55364](https://github.com/dotnet/runtime/issues/55364)).

###  Loop Optimizations  

In preview 7, we made several improvements on Loop Optimizations.

- [PR #71184](https://github.com/dotnet/runtime/pull/71184) strengthens checking of the loop table for better loop integrity checks as described in [#71084](https://github.com/dotnet/runtime/issues/71084).
- [PR #71868](https://github.com/dotnet/runtime/pull/71868) Do not compact blocks around loops
- [PR #71659](https://github.com/dotnet/runtime/pull/71659) Adjust weights of blocks with profile inside loops in non-profiled methods
- [PR #71504](https://github.com/dotnet/runtime/pull/71504) Improvements to loop hoisting
- [PR #70271](https://github.com/dotnet/runtime/pull/70271) optimized multi-dimensional array access. It improved the latency by up to 67% ([Performance Link](https://github.com/dotnet/perf-autofiling-issues/issues/6721)).

In addition, Hot/Cold splitting is enabled for Exception Handling funclets in [PR #71236](https://github.com/dotnet/runtime/pull/71236).

## Contributor spotlight: Hugh Bellamy

A huge "Thank you" goes out to all our community members. We deeply appreciate your thoughtful contributions. We asked contributor [@hughbe](https://github.com/hughbe) to share his thoughts.

![Hugh Bellamy](./bellamy.jpg)

In Hugh's own words:

The first application I ever developed was as a young teen. Although "Hughser", a C# wrapping the WebBrowser component, unfortunately did not prove to win the browser wars, .NET was my first window into the world of software development. After a few years developing apps for iOS, I regained interest in .NET when roslyn, corefx and coreclr were open-sourced. I was excited by the opportunity to look inside the black box and see how the SDK I used actually worked. My first PR in November 2015 was to clean up and add tests for various methods on System.String. (Anyone who has taken a look at my contribution history is well aware that cleaning up, improving and adding tests became something of a specialty of mine!). I soon gained confidence contributing and became one of the most active contributors to .NET open source. I've now contributed to a variety of .NET Foundation projects, including corefx/coreclr/runtime, roslyn, xunit, mono, wpf and winforms. A highlight of my time contributing was definitely when I was flown out to Seattle for the Microsoft Build Conference 2019 by Microsoft to meet the team and get a shoutout at the .NET Keynote! I've also been awarded three consecutive Microsoft MVP Awards for my contributions, which I'm very proud of.

In the past, my contributions to .NET helped me secure an internship as a Junior Developer as a teenager.  I currently work as an Management Consultant specializing in Financial Services Data & Analytics. Although I have less time nowadays than I did at school and university, I still follow the .NET open source ecosystem keenly and maintain several .NET open source projects on GitHub under the username [@hughbe](https://github.com/hughbe). Coding and open source has always been a hobby of mine, but also something I've enjoyed integrating with my work. . As a Politics undergrad, I'm interested in how technology can be used in a political and social context. For example, as an intern at an NGO in Cambodia, I developed a Facebook scraping platform in .NET, parts of which I open sourced. The platform was used to provide insights into public political engagement to journalists, opposition parties and civil society. At the moment, I'm developing a prototype platform leveraging .NET to scrape public media sources, including Telegram, to assist in the gathering of Open Source Intelligence (OSINT) relating to the Ukrainian War.

Contributing to .NET has been an incredible experience - as a teenager I was remotely working with people who I would never have otherwise crossed paths with, on an almost daily basis. The team at Microsoft, as well as the rest of the open source community, are extremely accommodating, insightful and happy to be challenged. There are so many ways to get started, be it [submitting or reviewing issues](https://github.com/dotnet/runtime/issues), documentation and PRs, so [get started today](https://github.com/dotnet/runtime/blob/main/CONTRIBUTING.md)!

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

.NET 7 is a **Short Term Support (STS)** release, meaning it will receive free support and patches for 18 months from the release date. It's important to note that the quality of all releases is the same. The only difference is the length of support. For more about .NET support policies, see the [.NET and .NET Core official support policy](https://dotnet.microsoft.com/platform/support/policy/dotnet-core).

We recently changed the "Current" name to "Short Term Support (STS)". We're in the [process of rolling out that change](https://github.com/dotnet/core/pull/7517).

## Breaking changes

### Trimming and NativeAOT: All assemblies trimmed by default

To better align with user expectations and produce smaller and more efficient apps, trimming now trims all assemblies in console apps by default. This change only affects apps that are published with `PublishTrimmed=true`, and it only affects apps that had existing trim warnings. It also only affects plain .NET apps that don't use the Windows Desktop, Android, iOS, WASM, or ASP.NET SDK.

#### Previous behavior

Previously, only assemblies that were opted-in with `<IsTrimmable>true</IsTrimmable>` in the library project file were trimmed.

#### New behavior

Starting in .NET 7, trimming trims all the assemblies in the app by default. Apps that may have previously worked with `PublishTrimmed` may not work in .NET 7. However, only apps with trim warnings will be affected. If your app has no trim warnings, the change in behavior should not cause any adverse affects, and will likely decrease the app size.

For example, an app that uses `Newtonsoft.Json` or `System.Text.Json` without source generation to serialize and deserialize a type in the user project may have functioned before the change, because types in the user project were fully preserved. However, one or more trim warnings (warning codes `ILxxxx`) would have been present. Now, types in the user project are trimmed, and serialization may fail or produce unexpected results.

If your app did have trim warnings, you may see changes in behavior or exceptions. For example, an app that uses `Newtonsoft.Json` or `System.Text.Json` without source generation to serialize and deserialize a type in the user project may have functioned before the change, because types in the user project were fully preserved. However, one or more trim warnings (warning codes `ILxxxx`) would have been present. Now, types in the user project are trimmed, and serialization may fail or produce unexpected results.

#### Recommended action

The best resolution is to resolve all the trim warnings in your application. To revert to the previous behavior, set the `TrimMode` property to `partial`, which is the pre-.NET 7 behavior.

```xml
<TrimMode>partial</TrimMode>
```

The default .NET 7+ value is `full`:

```xml
<TrimMode>full</TrimMode>
```

### Other breaking changes

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

We appreciate and [thank you](https://dotnet.microsoft.com/thanks) for your all your support and contributions to .NET. Please [give .NET 7 Preview 7 a try](https://dotnet.microsoft.com/download/dotnet/7.0) and tell us what you think!
