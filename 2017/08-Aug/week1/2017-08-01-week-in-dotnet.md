---
title: The week in .NET - Nuke, Warden.NET
keywords: Week in .NET, community, .NET
weblogName: .NET Blog
dontInferFeaturedImage: true
---
Previous posts:

* [MIST, F# in NYC, and links](https://blogs.msdn.microsoft.com/dotnet/2017/07/26/the-week-in-net-mist-f-in-nyc-and-links/)
* [Command Line Parser Library, .NET South East](https://blogs.msdn.microsoft.com/dotnet/2017/07/18/the-week-in-net-command-line-parser-library-net-south-east/)
* [Links!](https://blogs.msdn.microsoft.com/dotnet/2017/07/11/the-week-in-net-links-2/)

## Tool of the week: Nuke

[Nuke](http://www.nuke.build/) is a cross-platform build automation system with C# DSL, much like [Cake](http://cakebuild.net/). It features auto-completion, refactoring, and navigation in all IDEs. Nuke avoids complexity, and integrates as a normal project.

```csharp
[Parameter] string MyGetApiKey;

Target Publish => _ => _
        .Requires(() => MyGetApiKey)
        .OnlyWhen(() => IsServerBuild)
        .DependsOn(Pack)
        .Executes(() => GlobFiles(OutputDirectory / "packages", "*.nupkg")
                .ForEach(x => NuGetPush(s => s
                        .SetTargetPath(x)
                        .SetVerbosity(NuGetVerbosity.Detailed)
                        .SetApiKey(MyGetApiKey)
                        .SetSource("https://www.myget.org/F/nukebuild/api/v2/package"))));
```

* [The Nuke web site](http://www.nuke.build/)

## Package of the week: Warden.NET

The System.Diagnostics.Process class while useful does not have a concept for parent applications; while Windows itself does track parents, it does not track grandparents and processes can quickly become orphaned. Which is where Warden comes in. Warden.NET is a simple to use library for managing processes and their states.

```csharp
WardenManager.Initialize();
var wardenTest = await WardenProcess.Start("notepad.exe", string.Empty, ProcessTypes.Win32);
if (wardenTest != null)
{
    wardenTest.OnStateChange += delegate (object sender, StateEventArgs args)
    {
        Console.WriteLine($"---\nName: {wardenTest.Name}\nId: {wardenTest.Id}\nstate changed to {args.State}\n---");
    };
}
```

* [Tracking and Managing Processes on Windows with Warden.NET](https://blog.rainway.io/tracking-and-managing-processes-on-windows-be9d95602b54)
* [Warden.NET on GitHub](https://github.com/RainwayApp/warden/)
* [Warden.NET on Nuget](https://www.nuget.org/packages/Warden/)

## User group meeting of the week: .NET day in Bangalore

[The .NET Bangalore user group](https://www.meetup.com/DotNetBLR/) has [a full day event on Saturday, August 5](https://www.meetup.com/DotNetBLR/events/236111765/) with a great agenda: ES6 for .NET developers, .NET Core WebAPI, ASP.NET Core, and AI with .NET.

## .NET

* [Testing a Cake Addin](https://codeopinion.com/testing-a-cake-addin/) by Derek Comartin.
* [Using .NET Core 2 to read serial data from an Arduino UNO over USB](https://jeremylindsayni.wordpress.com/2017/07/31/using-net-core-2-to-read-serial-data-from-an-arduino-uno-over-usb/) by Jeremy Lindsay.
* [.NET Application Architecture Guidance](https://blogs.msdn.microsoft.com/dotnet/2017/07/26/the-new-net-application-architecture-guidance/) by Cesar de la Torre.
* [Top 5 .NET Exceptions](https://blogs.technet.microsoft.com/uktechnet/2017/07/25/top-5-net-exceptions/) by Liam Westley.
* [Building a query parser over a weekendPart II](https://ayende.com/blog/179137/building-a-query-parser-over-a-weekend-part-ii?Key=859f407c-8753-4bd0-9f9c-359eab8484c7) by Ayende Rahien.
* [Reflecting on performance testing](http://aakinshin.net/blog/post/reflecting-on-performance-testing/) by Andrey Akinshin.

## ASP.NET

* [Creating Web API With ASP.NET Core Using Visual Studio Code](http://www.c-sharpcorner.com/article/creating-web-api-with-asp-net-core-using-visual-studio-code/) by Ahmed Abdi.
* [In ASP.NET Core 1.1](http://www.jerriepelser.com/blog/accessing-tokens-aspnet-core-2/) by Jerrie Pelser.
* [Customising ASP.NET Core Identity EF Core naming conventions for PostgreSQL](https://andrewlock.net/customising-asp-net-core-identity-ef-core-naming-conventions-for-postgresql/) by Andrew Lock.
* [Introducing Support for Brotli Compression](https://blogs.msdn.microsoft.com/dotnet/2017/07/27/introducing-support-for-brotli-compression/) by Denys Tsomenko.
* [In-Memory ASP.NET Core Integration Tests with TestServer](https://visualstudiomagazine.com/articles/2017/07/01/testserver.aspx) by Jason Roberts.
* [Peachpie - Open Source PHP Compiler to .NET and WordPress under ASP.NET Core](https://www.hanselman.com/blog/PeachpieOpenSourcePHPCompilerToNETAndWordPressUnderASPNETCore.aspx) by Scott Hanselman.
* [ASP.NET Core MVC – Custom Tag Helpers](https://codingblast.com/asp-net-core-mvc-custom-tag-helpers/) by Ibrahim Šuta.
* [ASP.NET Core Razor Pages – Handler Methods](https://codingblast.com/asp-net-core-razor-pages-handlers/) by Ibrahim Šuta.
* [Redis InMemory Cache in ASP.net MVC Core](https://garywoodfine.com/redis-inmemory-cache-asp-net-mvc-core/) by Gary Woodfine.
* [Building a scheduled task in ASP.NET Core/Standard 2.0](https://blog.maartenballiauw.be/post/2017/08/01/building-a-scheduled-cache-updater-in-aspnet-core-2.html) by Maarten Balliauw.
* [Run ASP.NET Core on OpenShift](https://carlos.mendible.com/2017/07/26/run-asp-net-core-on-openshift/) by Carlos Mendible.
* [Validating user with cookie authentication in ASP.NET Core 2](https://www.meziantou.net/2017/07/20/validating-user-with-cookie-authentication-in-asp-net-core-2) by Gérald Barré.

## C#

* [Practical C# – Implementing Equality](http://www.andreaangella.com/2017/07/practical-csharp-implementing-equality/) by Andrea Angella.
* [If there is no difference between two options, choose the one that is easier to debug](https://blogs.msdn.microsoft.com/oldnewthing/20170725-00/?p=96676) by Raymond Chen.
* [Trying to set a readonly auto-property value externally (plus, a little BenchmarkDotNet)](http://www.productiverage.com/trying-to-set-a-readonly-autoproperty-value-externally-plus-a-little-benchmarkdotnet) by Dan Roberts.
* [Michael James - Developer](http://blog.mjjames.co.uk/2017/07/its-little-things.html) by Michael James.
* [Directly throw Exception as an Expression – Throw expressions in C# 7.0](http://dailydotnettips.com/2017/07/31/directly-throw-exception-as-an-expression-throw-expressions-in-c-7-0/) by Abhijit Jana.
* [Perusing C# 7.1](http://davidpine.net/blog/csharp-seven-dot-one/) by David Pine.
* [Suppress "Use 'throw' expression" suggestion](https://www.meziantou.net/2017/07/31/suppress-use-throw-expression-suggestion) by Gérald Barré.

## F#

* [F# Tutorial](https://www.youtube.com/watch?v=c7eNDJN758U&feature=youtu.be) by Derek Banas.
* [Experimenting with Partial Application](http://geekeh.com/experimenting-with-partial-application/) by Shane Charles.
* [Getting started with F# and .NET Core](http://julienblanchard.com/2017/getting-started-with-fsharp-and-dotnet-core/) by UNKNOWN.
* [Does it make sense to invest into the stock market at all-time highs? Answered with F# on .NET Core](http://kalapos.net/Blog/ShowPost/BackTestingWithFSharp) by Gergely Kalapos.
* [When to use a Discriminated Union vs Record Type in F#](http://www.mindbodysouldeveloper.com/2017/07/17/when-to-use-a-discriminated-union-vs-record-type-in-f-sharp/) by Jose Gonzalez.
* [Two Tetromino Tetris with Fable and F#](http://www.prigrammer.com/?p=489) by Tom Prior.

There is more content available this week in [F# Weekly](https://sergeytihon.wordpress.com/category/f-weekly/). If you want to see more F# awesomeness, please check it out!

## Xamarin

* [Xamarin.Android Proguard Notes](http://www.jon-douglas.com/2017/07/20/proguard-notes/) by Jon Douglas.
* [Xamarin.Tip – Borderless Picker](https://alexdunn.org/2017/07/24/xamarin-tip-borderless-picker/) by Alex Dunn.
* [Xamarin.Tip – Borderless DatePicker](https://alexdunn.org/2017/07/25/xamarin-tip-borderless-datepicker/) by Alex Dunn.
* [Stop toggling IsBusy with NotifyTask!](http://nmilcoff.com/2017/07/10/stop-toggling-isbusy-with-notifytask/) by Nico.
* [A Beginners Guide for Contributing to Xamarin.Forms](https://blog.xamarin.com/?p=32646&preview=1&_ppp=fc271eb8e4) by David Ortinau.
* [Building the Xamarin.Forms NuGet](https://blog.xamarin.com/building-xamarin-forms-nuget/) by David Ortinau.
* [Installing Visual Studio 2017 Made Easy](https://blog.xamarin.com/installing-visual-studio-2017-made-easy/) by Mayur Tendulkar.
* [Tracking Memory Leaks In Xamarin With The Profiler – Part 2](https://xamarinhelp.com/tracking-memory-leaks-xamarin-profiler-part-2/) by Adam Pedley.
* [Prism in Xamarin Forms Step by Step (Part. 2)](https://xamgirl.com/prism-in-xamarin-forms-step-by-step-part-2/) by Charlin Agramonte.
* [Announcing MvvmCross 5.1!](https://www.mvvmcross.com/mvvmcross-51-release/) by MvvmCross.
* [Show Live Updates to Your Xamarin Grids](https://www.infragistics.com/community/blogs/infragistics/archive/2017/07/20/live-updates-to-the-xamarin-grid.aspx) by Infragistics.

## Azure

* [Calling a downstream web API from a web API using Azure AD](https://azure.microsoft.com/en-us/resources/samples/active-directory-dotnet-webapi-onbehalfof/) by Jean-Marc Prieur.
* [Run Console Apps on Azure Functions](https://azure.microsoft.com/en-us/resources/samples/functions-dotnet-migrating-console-apps/) by Azure Team.
* [Is Your Serverless Application Testable? – Azure Logic Apps](https://blog.kloud.com.au/2017/07/24/is-your-serverless-application-testable-azure-logic-apps/) by Justin Yoo.

## UWP

* [How to Restart your App Programmatically](https://blogs.windows.com/buildingapps/2017/07/28/restart-app-programmatically/#oqVq2U7hhCPrDkU0.97) by Andrew Whitechapel.
* [Creating a geographical map on the floor in your Hololens / Windows MR app](http://dotnetbyexample.blogspot.com/2017/07/create-geographical-map-on-floor-in.html) by Joost van Schaik.
* [Text-To-Speech with Windows 10 Iot Core & UWP on Raspberry Pi](http://www.tozon.info/blog/post/2017/07/28/Text-To-Speech-with-Windows-10-Iot-Core-UWP-on-Raspberry-Pi) by Andrej Tozon.
* [Login to your UWP with your MSA](http://blog.jerrynixon.com/2017/07/login-to-your-uwp-with-your-msa.html) by Jerry Nixon.

And this is it for this week!

## Contribute to the week in .NET

As always, this weekly post couldn't exist without community contributions, and I'd like to thank all those who sent links and tips. The F# section is provided by [Phillip Carter](https://twitter.com/_cartermp), the gaming section by [Stacey Haffner](https://twitter.com/yecats131), the Xamarin section by [Dan Rigby](https://twitter.com/DanRigby), and the Azure and UWP section by [Michael Crump](http://twitter.com/mbcrump).

You can participate too. Did you write a great blog post, or just read one? Do you want everyone to know about an amazing new contribution or a useful library? Did you make or play a great game built on .NET?
We'd love to hear from you, and feature your contributions on future posts. Please [add your posts](https://weekindotnet.azurewebsites.net), it takes only a second.

We pick the articles based on the following criteria: the posts must be about .NET, they must have been published this week, and they must be original contents. Publication in Week in .NET is not an endorsement from Microsoft or the authors of this post.

This week's post (and future posts) also contains news I first read on [The ASP.NET Community Standup](https://blogs.msdn.microsoft.com/webdev/tag/communitystandup/), on [Weekly Xamarin](http://weeklyxamarin.com/), on [F# weekly](https://sergeytihon.wordpress.com/category/f-weekly/), and on [The Morning Brew](http://themorningbrew.net/).