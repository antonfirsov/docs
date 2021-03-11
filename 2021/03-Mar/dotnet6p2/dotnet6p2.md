---
post_title: Announcing .NET 6 Preview 2
username: rlander@microsoft.com
microsoft_alias: rlander@microsoft.com
categories: .NET Core, .NET
desired_publication_date: 3/11/2021
summary: .NET 6 Preview 2 is now available.
---

Today, we are glad to release .NET 6 Preview 2. It includes new APIs and runtime performance improvements. It also includes builds for Apple Silicon, which were missing for Preview 1. After the [announcement of the overall .NET 6 release](https://devblogs.microsoft.com/dotnet/announcing-net-6-preview-1/), we're now back to regularly scheduled monthly previews until the final release in November. You will see new features included in each preview that deliver on [.NET 6 themes, epics and user stories](https://themesof.net/). These themes offer improvements to every .NET application, including for server/cloud, desktop and mobile.

You can [download .NET 6 Preview 2](https://dotnet.microsoft.com/download/dotnet/6.0), for Windows, macOS, and Linux.

* [Installers and binaries](https://dotnet.microsoft.com/download/dotnet/6.0)
* [Container images](https://hub.docker.com/_/microsoft-dotnet)
* [Linux packages](https://github.com/dotnet/core/blob/master/release-notes/6.0/)
* [Release notes](https://github.com/dotnet/core/tree/master/release-notes/6.0)
* [Known issues](https://github.com/dotnet/core/blob/master/release-notes/6.0/6.0-known-issues.md)
* [GitHub issue tracker](https://github.com/dotnet/core/issues)

See the ASP.NET Core and EF Core posts for more detail on what’s new for web and data access scenarios.

.NET 6 has been tested with Visual Studio 16.9 Preview 4 and Visual Studio for Mac 8.9. We recommend you use those builds if you want to try .NET 6.

In this post, two of my team members will cover the [.NET 6 themes](https://themesof.net/) that they are working on, specifically [Improve inner-loop performance for .NET developers](https://github.com/dotnet/core/issues/5510) and [.NET has a great client app development experience](https://github.com/dotnet/core/issues/5423). I covered [Improve startup and throughput using runtime execution information (PGO)](https://github.com/dotnet/core/issues/5491) in the [.NET 6 Preview 1 post](https://devblogs.microsoft.com/dotnet/announcing-net-6-preview-1/#theme-improve-startup-and-throughput-using-runtime-execution-information-pgo).

## Support

.NET 6 will be will be released in November 2021, and will be supported for three years, as a [Long Term Support (LTS) release](https://github.com/dotnet/core/blob/master/release-policies.md). The [platform matrix](https://github.com/dotnet/core/blob/master/release-notes/6.0/6.0-supported-os.md) has been significantly expanded compared to .NET 5.

The additions are:

- Android.
- iOS.
- Mac and Mac Catalyst, for x64 and Apple Silicon (AKA "M1").
- Windows Arm64 (specifically Windows Desktop).

.NET 6 Debian container images are based on Debian 11 ("bullseye"), which is [currently in testing](https://wiki.debian.org/DebianBullseye).

## Theme -- Improve .NET Inner Loop Performance

[Stephen Toub](https://github.com/stephentoub) is the author of this theme section. He's part of the team working on the [inner loop theme](https://github.com/dotnet/core/issues/5510), and is sharing his team's goals and plans.

Performance is top of mind for all .NET releases.  Over the past several versions, a lot of effort has gone into improving throughput, reducing memory consumption, and other factors that impact the steady-state performance of an application.  For .NET 6, we’re also focused on a few additional aspects, several of which are highlighted as part of one of our .NET 6 themes: [improving inner-loop performance for .NET developers](https://github.com/dotnet/core/issues/5510). Just as we’ve focused on ensuring that apps and services have the best possible performance, we also want to ensure that developers are able to be as productive as possible, optimizing the tools and workflows that are frequently used as part of their “inner loop” (meaning the processes they use repeatedly as part of making code changes, building, and testing).

Some of these efforts look very similar to traditional throughput efforts of the past, but instead of focusing on steady-state performance, they’re focused on startup performance, of the runtimes, the app models, the `dotnet` CLI, MSBuild, etc., as well as the end-to-end performance of the tools (especially for smaller solutions, as historically they haven’t received as much attention).  The mindset involved in such optimizations is often quite different from the mindset used when optimizing for steady-state throughput.  For steady-state work, you might focus on caching values that could be reused in the future, but often for startup performance you’re focused on operations that may only be invoked once and where the costs for that first invocation matter.  The work involved here does, however, look like many other performance efforts, with a typical profile-analyze-fix loop: you profile the relevant area of the application you want to optimize, you analyze the resulting data to look for the top offenders and bottlenecks, you come up with fixes for them, and then you start the process over to find the next impactful items.  We’re still early in the .NET 6 development cycle, but we’ve already been successful in trimming overheads from key areas involved in developers’ inner loops, focusing on various `dotnet` commands like `new`, `build`, and `run`.  Example improvements thus far include fixing places tools were unexpectedly JIT’ing (https://github.com/dotnet/installer/pull/9635), avoiding expensive logging-related work happening even when logging wasn’t enabled (https://github.com/dotnet/aspnetcore/pull/27956), optimizing globbing in MSBuild (https://github.com/dotnet/msbuild/pull/6151), changing the ASP.NET Razor compiler to use a Roslyn source generator in order to avoid an extra compilation process (https://github.com/dotnet/sdk/pull/15756), and changing the .NET host to use file access patterns less likely to trigger antivirus tooling on the machine (https://github.com/dotnet/runtime/pull/48774).

Of course, one of the best performance optimizations is one that avoids the need for work to be done entirely, and that is the focus of the other half of this .NET 6 theme: .NET hot reload. Hot reload will improve developer productivity, across all supported operating systems and hardware platforms, by enabling code to be edited while an app is running, even without a debugger attached.  With many kinds of edits, no restart will be required: the developer will save their changes, and the new version of the code will be applied to the executing process.  Upon wanting to make a change to the app or service, rather than needing to stop it and go through the typical inner loop cycle of making the change, building, running, and getting back to the point in the app or service that triggered the need for the change in the first place, that whole cycle can be skipped for many kinds of edits. The work involved in enabling this spans the .NET release, requiring investments in the runtimes (both coreclr and mono), the C# Roslyn compiler, the app models (e.g. Blazor, .NET MAUI), and the developer tools (e.g. CLI, Visual Studio), but it is expected to fundamentally improve the way .NET developers go about writing their apps and services.

## Theme: .NET has a great client app development experience

[Maddy Leger](https://github.com/maddyleger1) is the author of this theme section. She is part of the team working on the [client app development theme](https://github.com/dotnet/core/issues/5423), and is sharing her team's goals and plans.

One of the most exciting parts of .NET 6 is mobile development, which is currently offered as the separate Xamarin product. Over time, we've been making Xamarin more similar to mainline .NET. It's now time to deliver a fully unified mobile product for .NET. With .NET 6, iOS, Android, and macOS development will be integrated into the .NET SDK experience and use the .NET libraries. For the past couple years, we’ve been working to fold Mono into .NET, so developers can leverage the strengths of both runtimes without having to switch BCLs or target different .NET versions. In .NET 5, we moved Blazor WebAssembly over, and are using that same model for Xamarin. .NET 6 is the culmination of this unification effort, encompassing a key Epic of the theme - [Xamarin developers can upgrade to and use the latest .NET SDKs for their existing apps](https://github.com/dotnet/xamarin/issues/2).

Now that all your .NET apps will run on the same libraries, we want to increase the amount of code you share across desktop and mobile platforms. Xamarin.Forms, Xamarin’s cross-platform UI framework, is evolving into [.NET Multi-platform App UI](https://github.com/dotnet/maui), enablin you to easily write apps for iOS, Android, Windows, and macOS with the same codebase. .NET MAUI is shipping as part of .NET 6 along with a bunch of performance and tooling improvements like .NET/C# Hot Reload, more shared resources and code across different platforms, and better page rendering performance with a more flexible set of UI controls. You can follow the overall Epic: [Xamarin/.NET MAUI developers have improved app performance and share more code with .NET 6](https://github.com/dotnet/xamarin/issues/21).

.NET MAUI isn’t just for client app developers. Thanks to the refactored control set and the ability to run on the .NET 6 libraries, your existing Blazor apps will run natively on Windows and macOS via .NET MAUI. You’ll be able to seamlessly weave in native controls and functionality alongside your Blazor codebase, including platform specific functionality. See the [Preview 1 blog for some screenshots of “Blazor desktop” in action](https://devblogs.microsoft.com/dotnet/announcing-net-6-preview-1/#blazor-desktop-apps).  

The final epic we’re focused on in this theme is about packaging, deploying, and releasing your cross-platform client apps. Because there are so many developers/target platforms/ways to develop your apps, there are a lot of different app packages you have to distribute at the end of the day. Especially with Blazor desktop, we want to make that experience as seamless as possible. We’re looking at strategies for improving release and versioning both locally and in the cloud as part of the epic Desktop developers can package, distribute, release, and update their applications to multiple desktop platforms and architectures.

To summarize, in .NET 6 you’ll be able to: 

- Build iOS, Android, and macOS apps using the .NET libraries.
- Create iOS, Android, Windows, and macOS apps from the same codebase using .NET MAUI.
- Share more code and resources across platforms (such as images, app icons/manifests, and more).
- Run your Blazor web apps natively on the macOS and Windows.
- Easily package and distribute your apps for all the target frameworks you include.

We’ve already made a lot of progress and will continue to add more features into each .NET 6 Preview. For more info, you can follow the [].NET MAUI repo](http://github.com/dotnet/maui), which is kept up to date with progress reports.

The remainder of the post is dedicated to features that are new in [Preview 2](https://github.com/dotnet/core/issues/5889).

## .NET Multi-platform App UI

We have added .NET MAUI and single project developer experiences for Android, iOS, and Mac Catalyst.

### Mac Catalyst SDK and target framework moniker for compiling .NET MAUI apps for macOS desktop

```xml
<TargetFrameworks>net6.0-android;net6.0-ios</TargetFrameworks>
<TargetFrameworks Condition=" '$(OS)' != 'Windows_NT' ">$(TargetFrameworks);net6.0-maccatalyst</TargetFrameworks>
```

### A single, multi-targeted application project

![single, multi-targeted application project](https://user-images.githubusercontent.com/41873/110378427-36651a80-801b-11eb-8707-ae3f7109ff16.png)


### Shared fonts, images, and app icons

Fonts and images can be placed in one location in your solution and .NET MAUI will enable them to natively work on all platforms you target. These are tracked in your *.csproj as `SharedImage` and `SharedFont`.

```xml
<ItemGroup>
    <SharedImage Include="appicon.svg" ForegroundFile="appiconfg.svg" IsAppIcon="true" />
    <SharedFont Include="Resources\Fonts\ionicons.ttf" />
  </ItemGroup>
```

Both accept wildcards to include all files within a location.

```xml
<ItemGroup>
    <SharedImage Include="appicon.svg" ForegroundFile="appiconfg.svg" IsAppIcon="true" />
    <SharedImage Include="Resources\Images\*" />
    <SharedFont Include="Resources\Fonts\*" />
  </ItemGroup>
```

### MauiApp with Host Builder for bootstrapping your app

We have extensions for configuring services, fonts, and compatibility renderers for migrating Xamarin.Forms projects. `IWindow` has been introduced for future multi-window support coming in a future release.  

```csharp
public class Application : MauiApp
{
    public override IAppHostBuilder CreateBuilder() => 
        base.CreateBuilder()
            .RegisterCompatibilityRenderers()
            .ConfigureServices((ctx, services) =>
            {
                services.AddTransient<MainPage>();
                services.AddTransient<IWindow, MainWindow>();
            })
            .ConfigureFonts((hostingContext, fonts) =>
            {
                fonts.AddFont("ionicons.ttf", "IonIcons");
            });

    public override IWindow CreateWindow(IActivationState state)
    {
        Microsoft.Maui.Controls.Compatibility.Forms.Init(state);
        return Services.GetService<IWindow>();
    }
}
```

### New Control Handlers

We have introduced the first controls and properties that implement a new handler approach. These include partial implementations of Button, Label, and Entry, Slider, and Switch.

macOS:
<img width="766" alt="Screen Shot 2021-03-08 at 10 57 44 AM" src="https://user-images.githubusercontent.com/41873/110378734-8d6aef80-801b-11eb-95fa-25155b9216ce.png">

iOS:
<img width="502" alt="Screen Shot 2021-03-08 at 2 39 34 PM" src="https://user-images.githubusercontent.com/41873/110379248-33b6f500-801c-11eb-8ef5-b0193d26a70e.png">


Android:
![Android emulator screenshot of .NET MAUI controls](https://user-images.githubusercontent.com/41873/110379091-fce0df00-801b-11eb-9bf3-976174650f1b.png)

### Updates to Mobile SDKs

Android:
* Android X libraries are now available for .NET 6 and the default dependency for Android apps

iOS:
* Windows developers can use the Remote iOS Simulator
* VS Mac on Windows can connect to the remote Mac build host
* AOT enables build and deploy to physical iOS hardware

## .NET Libraries

The following APIs and improvements have been added to the .NET libraries.

### System.Text.Json - ReferenceHandler.IgnoreCycles

[`JsonSerializer` (System.Text.Json)](https://docs.microsoft.com/dotnet/api/system.text.json.jsonserializer) now supports the ability to [ignore cycles when serializing an object graph](https://github.com/dotnet/runtime/issues/40099). The `ReferenceHandler.IgnoreCycles` option has similar behavior as Newtonsoft.Json [`ReferenceLoopHandling.Ignore`](https://www.newtonsoft.com/json/help/html/ReferenceLoopHandlingIgnore.htm). One key difference is that the System.Text.Json implementation replaces reference loops with the `null` JSON token instead of ignoring the object reference.

You can see the behavior of `ReferenceHandler.IgnoreCycles` in the following example. In this case, the `Next` property is serialized as `null` since it otherwise creates a cycle.

```csharp
class Node
{
    public string Description { get; set; }
    public object Next { get; set; }
}

void Test()
{
    var node = new Node { Description = "Node 1" };
    node.Next = node;
    
    var opts = new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.IgnoreCycles };
    
    string json = JsonSerializer.Serialize(node, opts);
    Console.WriteLine(json); // Prints {"Description":"Node 1","Next":null}
}
```

### PriorityQueue

`PriorityQueue<TElement, TPriority>` (System.Collections.Generic) is a new collection that enables adding new items with a value and a priority. On dequeue the PriorityQueue returns the element with the lowest priority value. You can think of this new collection as similar to `Queue<T>` but that each enqueued element has a priority value that affects the behavior of dequeue.

The following sample demonstrates the behavior of `PriorityQueue<string, int>`.

```csharp
// creates a priority queue of strings with integer priorities
var pq = new PriorityQueue<string, int>();

// enqueue elements with associated priorities
pq.Enqueue("A", 3);
pq.Enqueue("B", 1);
pq.Enqueue("C", 2);
pq.Enqueue("D", 3);

pq.Dequeue(); // returns "B"
pq.Dequeue(); // returns "C"
pq.Dequeue(); // either "A" or "D", stability is not guaranteed.
```

Credit to community member [Patryk Golebiowski](https://github.com/pgolebiowski) for contributing [the implementation](https://github.com/dotnet/runtime/pull/46009).

### Better parsing of standard numeric formats

We've improved the parser for the standard numeric types, specifically for `.ToString` and `.TryFormat`. They will provide better results when precision > 99 decimal places is specified. Also, the parser now better supports trailing zeros in the `Parse` method.

The following examples demonstrate before and after behavior.

- `32.ToString("C100")` -> `C132`
   - .NET 6: `$32.0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000`
   - .NET 5: We had an artificial limitation in the formatting code to only handle a precision of <= 99. For precision >= 100, we instead interpreted the input as a custom format.
- `32.ToString("H99")` -> throw a `FormatException`
   -  .NET 6: throws a FormatException
   - This is correct behavior, but is called here to contrast with the next example.
- `32.ToString("H100")` -> `H132`
   - .NET 6: throw a FormatException
   - .NET 5: `H` is an invalid format specifier. So, we should've thrown a `FormatException`. Instead, our incorrect behavior of interpreting precision >= 100 as custom formats meant we returned wrong values.
- `double.Parse("9007199254740997.0")` -> `9007199254740998`
   - .NET 6: `9007199254740996`.
   - .NET 5: `9007199254740997.0` is not exactly representable in the IEEE 754 format. With our current rounding scheme, the correct return value should have been `9007199254740996`. However, the last `.0` portion of the input was forcing the parser to incorrectly round the result and return `9007199254740998`. 

Changes:

- https://github.com/dotnet/runtime/issues/46827
- https://github.com/dotnet/docs/pull/23046
- https://github.com/dotnet/docs/issues/22458
- 
### SignalR - Nullable annotations

The [ASP.NET Core SignalR Client](https://www.nuget.org/packages/Microsoft.AspNetCore.SignalR.Client/) package has been [annotated for nullability](https://github.com/dotnet/aspnetcore/pull/29219). This means that the C# compiler will provide appropriate feedback, when you [enable nullability](https://docs.microsoft.com/dotnet/csharp/language-reference/builtin-types/nullable-value-types), based on your handling of nulls in SignalR APIs. The SignalR server has already been updated for nullability in 5.0, but a few modifications were made in 6.0. You can track ASP.NET Core support for nullable annotations at [dotnet/aspnetcore #27389](https://github.com/dotnet/aspnetcore/issues/27389).

Nullable annotations were a big focus of the .NET Core 3.x and .NET 5 releases. All of the .NET libraries (AKA "base class libraries") were annotated as part of those releases. The [System.Device.Gpio](https://www.nuget.org/packages/System.Device.Gpio/) package was also annotated as part of the .NET 5 release.

You need to add `<Nullable>enable</Nullable>` in your project file to use the nullable annotations.

## Runtime

The following improvements have been made in (or related to) the .NET runtime.

## Framework Assemblies are compiled with Crossgen2

All of the .NET libraries are now compiled with [crossgen 2](https://devblogs.microsoft.com/dotnet/announcing-net-6-preview-1/#crossgen2), across all supported operating systems and architectures. This includes all libraries in the `Microsoft.NETCore.App` directory, but not the other frameworks, such as ASP.NET or Windows Desktop. Those frameworks will be transitioned to crossgen 2 in Previews 3 and/or 4.

Crossgen 2 itself isn't intended to improve performance. As I said in the [preview 1 post](https://devblogs.microsoft.com/dotnet/announcing-net-6-preview-1/#crossgen2), the purpose of Crossgen 2 is to enable new performance features like [PGO](https://devblogs.microsoft.com/dotnet/announcing-net-6-preview-1/#theme-improve-startup-and-throughput-using-runtime-execution-information-pgo). That said, Crossgen 2 has delivered modest size-on-disk improvements based on some targeted size optimizations, as you can see in the following comparison. Main point is that moving to Crossgen 2 does not incur any up-front regressions. It is effectively neutral, as promised.

```console
Size [MB] FullName
--------- --------
64.22     C:\Program Files\dotnet\shared\Microsoft.NETCore.App\5.0.3
63.31     C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.0-preview.1.21102.12
63.00     C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.0-preview.2.21118.6
```

### Profile guided optimization

[Profile guided optimization](https://devblogs.microsoft.com/dotnet/announcing-net-6-preview-1/#theme-improve-startup-and-throughput-using-runtime-execution-information-pgo) enables us to generate code that is optimal based on a variety of characteristics. We are building both static and [dynamic PGO](https://github.com/dotnet/runtime/issues/43618) variations.

The following improvements have been made in Preview 2:

- Allow CSE & hoisting of vtable lookups for the indirections -- [dotnet/runtime #47808](https://github.com/dotnet/runtime/pull/47808)
- Block counts in tiered compilation -- [dotnet/runtime #13672](https://github.com/dotnet/runtime/issues/13672)
- Allow Inlinee profile scale-up -- [dotnet/runtime #48280](https://github.com/dotnet/runtime/pull/48280)
- Efficient profiling scheme (e.g., spanning tree with efficient edge instrumentation) -- [dotnet/runtime #46882](https://github.com/dotnet/runtime/issues/46882), [dotnet/runtime #47509](https://github.com/dotnet/runtime/pull/47509), [dotnet/runtime #47476](https://github.com/dotnet/runtime/pull/47476), [dotnet/runtime #47072](https://github.com/dotnet/runtime/pull/47072), [dotnet/runtime #47597](https://github.com/dotnet/runtime/pull/47597), [dotnet/runtime #47723](https://github.com/dotnet/runtime/pull/47723), [dotnet/runtime #47876](https://github.com/dotnet/runtime/pull/47876), [dotnet/runtime #47959](https://github.com/dotnet/runtime/pull/47959)

### JIT improvements

The following improvements were made to optimize the code generated by the JIT.

- Not aligning cloned loops -- [dotnet/runtime #48090](https://github.com/dotnet/runtime/pull/48090)
- MultiplyHigh intrinsics (`smulh`/`umulh`) -- [dotnet/runtime #47362](https://github.com/dotnet/runtime/pull/47362)

The first improvement is the latest result of a project to [stabilize performance measurements](https://github.com/dotnet/runtime/issues/43227). You can read recent analyses ([January 26, 2021](https://github.com/dotnet/runtime/issues/43227#issuecomment-767967603); [February 3, 2021](https://github.com/dotnet/runtime/issues/43227#issuecomment-772914110)) that provide an in-depth explanation of our progress.

The second improvement is specific to Arm64. We continue to [improve the performance of the code that the JIT generates for Arm64](https://github.com/dotnet/runtime/issues/43629), in collaboration with Arm engineers.

## Closing

We're back to the regular monthly previews, with .NET 6. Please give .NET 6 Preview 2 a spin in your environment, in a [container](https://hub.docker.com/_/microsoft-dotnet-sdk), a virtual machine or on a test machine. We want your feedback to help us improve these new features before we release the final version in November. We expect that the majority of the release should be done by July; we'll focus on quality after that. That gives you a sense of the windows we have for feedback. The earlier you can give feedback, the better.

You can expect to see more in [.NET 6](https://devblogs.microsoft.com/dotnet/announcing-net-6-preview-1) for [mobile](https://devblogs.microsoft.com/dotnet/announcing-net-6-preview-1/#net-multi-platform-app-ui) and [Blazor Desktop](https://devblogs.microsoft.com/dotnet/announcing-net-6-preview-1/#blazor-desktop-apps) in subsequent previews. We're still in the process of building those features.
