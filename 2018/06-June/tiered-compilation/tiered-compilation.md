# Tiered Compilation Preview in .NET Core 2.1 #

_Date_ by [Noah Falk](https://github.com/noahfalk)

If you are a fan of performance there has been a lot great news lately, such as [Performance Improvements in .NET Core 2.1](https://blogs.msdn.microsoft.com/dotnet/2018/04/11/announcing-net-core-2-1-preview-2/) and [Announcing .Net Core 2.1 Preview 2](https://blogs.msdn.microsoft.com/dotnet/2018/04/11/announcing-net-core-2-1-preview-2/), but the show isn't over yet. Tiered compilation is a significant new performance feature that we are making available as a preview for anyone to try out, starting in the RTM release of .NET Core 2.1. In many scenarios that we have tested applications both start faster and run faster at steady state. All it needs from you is a smidgen of curiosity, a project that runs on .NET Core 2.1, and a trivial change to your environment variables or project file to enable it. In the rest of this post I'll cover what it is, how you use it, and why it is the hidden gem of the 2.1 release! (I might be a tad biased)

## What is Tiered Compilation? ##

Since the beginnings of .NET Framework, every method in your code was compiled once*. However there are tradeoffs to be made when deciding how to do that compilation that will affect the performance of your application. For example the JIT could do very aggressive optimization and get great steady-state performance, but optimizing code well is not a quick endeavor so your application would start very slowly. Alternately the JIT could use very simple compilation algorithms that run quickly so your application starts fast, but the code quality would be much worse and steady-state application throughput would suffer. .NET has always tried to take a balanced approach that would do a reasonable job at both startup and steady-state performance, but using a single compilation means compromise was required.

The Tiered Compilation feature changes the premise by allowing .NET to have multiple compilations for the same method that can be hot-swapped at runtime. This separates the decision making so that we can pick a technique that is best for startup, pick a second technique that is best for steady-state and then deliver great performance on both. In .Net Core 2.1 this is what Tiered Compilation is doing for your application if you opt-in: 

  - **Faster application startup time** - When an application starts it waits for some code to JIT. Tiered compilation asks the JIT to generate the initial compilation very quickly, sacrificing code quality optimization if needed. Afterwards, if the method is called frequently, more optimized code is generated on a background thread and the initial code is replaced to preserve the application's steady state performance.

  - **Faster steady-state performance** - For a typical .NET Core application most of the framework code will load from pre-compiled (ReadyToRun) images. This is great for startup, but the pre-compiled images have versioning constraints and CPU instruction constraints that prohibit some types of optimization. For any methods in these images that are called frequently Tiered Compilation requests the JIT to create optimized code on a background thread that will replace the pre-compiled version.

## Faster? How much faster? ##

We'd love to hear how it does for your apps, but here are a few of the examples we tested it with:

[startup\_picture\_here]

[steady\_state\_picture\_here]

We also evaluated on a variety of different ASP.Net performance benchmarks. For each benchmark we compiled it several different ways for comparison purposes:
  
  - **Default** - This is the behavior you get if you call 'dotnet run' or publish the application using a framework dependent deployment. In this mode most framework and ASP.Net code is precompiled and the JIT does a moderate amount of optimization for any remaining code that is not pre-compiled.

  - **MinOpts** - This mode uses all the same precompiled code as Default but the JIT runs as quickly as possible producing less optimized code. This mode starts fast but has poor steady-state performance.

  - **Jitted** - This mode avoids (nearly) all the precompiled code and JITs instead with a moderate amount of optimization. This mode starts very slowly but has good steady-state performance because of the better quality of the jitted code.

  - **Tiered compilation** - This mode uses the same precompiled images as Default and enables the tiered compilation feature. It has most of the startup benefits from MinOpts as well as the steady-state benefits of Jitted.

Because there are many scenarios with different timescales, each scenario was normalized using the Default compilation strategy as the baseline. Startup time is shown on the x-axis of the scatterplot as the ratio of Default\_Mode startup time / Tested\_Mode startup time. Steady-state performance is shown on the y-axis, as a ratio Tested\_Mode requests per second / Default\_Mode requests per second. In short, bigger numbers are better and a dot at (1.25, 1.5) means that compilation mode started 25% faster and served requests 50% faster than default.


[asp.net picture here]

How will your apps fare? It is much easier to measure than to predict but we can offer a few broad rules of thumb.

1) The startup improvements apply primarily to reduced time jitting managed code. You can use tools such as [PerfView](https://github.com/Microsoft/perfview) to determine how much time your app spends doing this. In our testing time spent jitting would often decrease by about 35%.

2) The steady-state improvements apply primarily to applications that are CPU bound where a non-trivial amount of the hot code is coming from the .NET or ASP.Net libraries. Profilers such as [PerfView](https://github.com/Microsoft/perfview) can help you determine if your app is in this category.

## Trying it out ##

A small disclaimer, the feature is still a preview. We've done a lot of testing on it but haven't enabled the feature by default because we want to gather feedback and continue to make adjustments. Turning it on might not make your app faster or you might run into other rough edges we've missed. We are here to help if you encounter issues and you can always disable it easily. You can turn this on in production if you like, but we strongly suggest testing it out beforehand.

There are several ways to opt-in, all of which have the same effect:

  - If you build the application yourself using .Net 2.1 SDK - Add the MSBuild property <TieredCompilation\>true<\\TieredCompilation\> to the default property group in your project file. For example:
  
    <Project Sdk="Microsoft.NET.Sdk">
      <PropertyGroup>
        <OutputType>Exe</OutputType>
        <TargetFramework>netcoreapp2.1</TargetFramework>
        **<TieredCompilation>true<\TieredCompilation>**
      </PropertyGroup>
    </Project>

  - If you run an application that has already been built, edit runtimeconfig.json to add System.Runtime.TieredCompilation=true to the configProperties. For example:

    {
      "runtimeOptions": {
        "configProperties": {
          "System.Runtime.TieredCompilation": true,
      },
      "framework": {
        ...
      }
    }

  - If you run an application and don't want to modify any files, set the environment variable COMPlus_TieredCompilation=1

## Is it working? ##

TODO: Thinking we should export this section from the blog to a separate trouble shooting guide in github and only include a link

Hopefully you get the easy case, the app runs faster immediately, slam dunk!

Sometimes though software development is a little more challenging. Fear not, we are still in this together. A few things to check:

1) Make sure the managed assemblies were compiled for Release, not Debug. Tiered Compilation automatically disables itself on debug builds.
2) Make sure Tiered Compilation really is enabled. Given this is a performance feature with no exposed API confirming this is a little subtle. 

TODO: see if we can offer an easier diagnosis via PerfView, perhaps with a natural lead in to the performance diagnostics

 One way to bend the runtime to your will is to use a native debugger such as Visual Studio. Set a breakpoint at coreclr!ThePreStub (you don't need source for this and symbols are published to Microsoft's public symbol server). Once at the breakpoint enter "coreclr.dll!g_pConfig->fTieredCompilation" in the watch window. If the value is 'true' then Tiered Compilation is enabled.

TODO: add some content about diagnosing whether the app is expected to get faster on tiered compilation or this is just an app that tiered compilation can't do much for.

## Getting Technical ##

TODO: Thinking it might be better to move content in this section to a follow up blog post and expand on it? My concern is that it may break the flow for less technical customers and have them wander off without reaching the conclusion. A second post delayed a few weeks may also offer a 2nd chance to interest the audience if they missed the initial post and could have pretty high nerdiness appeal.

## Where do we go from here? ##

Tiered Compilation creates a variety of possibilities we could continue to capitalize on well into the future. Historically our JIT has been somewhat constrained to investing in techniques relatively near the middle ground positions .NET has always needed. Now that the runtime can capitalize on more extreme tradeoff positions we've got the leeway and the incentive to push the boundaries, both to make compilation faster and to generate higher quality code. With runtime hot-swapping of code .NET could start doing more detailed profiling and then use the runtime feedback to do even better optimization (profile guided optimization). Such techniques can allow code generators to out-perform even the best static optimizers that don't have access to profile data. Or there are yet other options such as dynamic deoptimization for better diagnostics, collectible code to reduce memory usage, and hot patching for instrumentation or servicing. For now our most immediate goal remains a bit closer to the ground - make sure the functionality in the preview works well, respond to your feedback, and finish this first iteration of the work.


## Wrapping Up ##

We hope Tiered Compilation gives your applications the same great improvements as the benchmarks and we know there is even more untapped potential yet to come. Please give it a try and then [come visit on github](https://github.com/dotnet/coreclr/issues/4331) to give us your feedback, discuss, ask questions, and maybe even contribute some code of your own. Thanks!