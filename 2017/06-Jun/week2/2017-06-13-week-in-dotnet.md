---
title: The week in .NET - On .NET with Mattias Karlsson on Cake, Topshelf
keywords: Week in .NET, .NET, community
weblogName: .NET Blog
postId: 10875
---
Previous posts:

* [On .NET with Brett Morrison, DateTime Extensions](https://blogs.msdn.microsoft.com/dotnet/2017/06/06/the-week-in-net-on-net-with-brett-morrison-datetime-extensions/)
* [Open XML SDK, Adventure Time](https://blogs.msdn.microsoft.com/dotnet/2017/05/30/the-week-in-net-open-xml-sdk-adventure-time/)
* [.NET poster, Happy Birthday .NET with Jan Kotas, Skyworld](https://blogs.msdn.microsoft.com/dotnet/2017/05/23/the-week-in-net-net-poster-happy-birthday-net-with-jan-kotas-skyworld/)


On .NET: Mattias Karlsson - Cake
--------------------------------

During the Microsoft Build conference, we recorded [interviews with some of the attendees](https://channel9.msdn.com/Shows/On-NET/Mattias-Karlsson-Cake). [Mattias Karlsson](https://github.com/devlead) is a core contributor on [Cake](http://cakebuild.net/), the cross-platform build automation system with a C# DSL.

<iframe src="https://channel9.msdn.com/Shows/On-NET/Mattias-Karlsson-Cake/player" width="960" height="540" allowFullScreen frameBorder="0"></iframe>

* [Cake](http://cakebuild.net/)
* [Mattias on GitHub](https://github.com/devlead)


Package of the week: Topshelf
-----------------------------

[Topshelf](http://topshelf-project.com/) is a framework for hosting services written using .NET. The creation of services is simplified by allowing developers to create an easy to debug console application that can also be installed as a service.

```csharp
public class TownCrier
{
    readonly Timer _timer;
    public TownCrier()
    {
        _timer = new Timer(1000) {AutoReset = true};
        _timer.Elapsed += (sender, eventArgs) =>
            Console.WriteLine(@"It is {DateTime.Now} and all is well");
    }
    public void Start() { _timer.Start(); }
    public void Stop() { _timer.Stop(); }
}

public class Program
{
    public static void Main()
    {
        HostFactory.Run(x =>
        {
            x.Service<TownCrier>(s =>
            {
               s.ConstructUsing(name=> new TownCrier());
               s.WhenStarted(tc => tc.Start());
               s.WhenStopped(tc => tc.Stop());
            });
            x.RunAsLocalSystem();

            x.SetDescription("Sample Topshelf Host");
            x.SetDisplayName("TownCrier");
            x.SetServiceName("TownCrier");
        });
    }
}
```

* [Topshelf web site](http://topshelf-project.com/)
* [Topshelf on NuGet](https://www.nuget.org/packages/Topshelf/)
* [Topshelf source code](https://github.com/Topshelf/Topshelf)
* [Topshelf documentation](https://topshelf.readthedocs.io/en/latest/index.html)

Meetup of the week: The Dockering of .NET with Cecil Phillip in Miami, FL
------------------------------------------------------------------------

The June meeting of [dotNet Miami](https://www.meetup.com/dotNetMiami/) will have .NET expert and Away From The Keyboard host  Cecil Phillip talking about marrying Docker and .NET. Cecil will review the basics of Docker and how we can add containers to our applications.

[Thursday, June 15, 2017 6:30 PM in Miami, FL](https://www.meetup.com/dotNetMiami/events/240608029/)

## .NET

* [Performance Improvements in .NET Core](https://blogs.msdn.microsoft.com/dotnet/2017/06/07/performance-improvements-in-net-core/) by Stephen Toub.
* [Measuring Performance Improvements in .NET Core with BenchmarkDotNet (Part 1)](http://aakinshin.net/blog/post/stephen-toub-benchmarks-part1/) by Andrey Akinshin.
* [.NET Core 2 and Visual Studio 2017 Preview 2](https://blogs.msdn.microsoft.com/dotnet/2017/06/12/10735/) by Lee Coward.
* [.NET Fringe: A Great Role Model for Community Oriented Conferences](https://blogs.msdn.microsoft.com/dotnet/2017/06/12/net-fringe/) by Immo Landwerth.
* [.NET and Docker](https://www.hanselman.com/blog/NETAndDocker.aspx) by Scott Hanselman.
* [Trying .NET Core on Linux with just a tarball (without apt-get)](https://www.hanselman.com/blog/CommentView.aspx?guid=67479F6E-F601-42C3-9E97-02036D170CFD#4dfeb315-5d0f-4917-b2b0-7124e930f387) by Scott Hanselman.
* [Continuously publishing Nuget package to MyGet using VSTS](https://www.meziantou.net/2017/06/12/continuously-publishing-nuget-package-to-myget-using-vsts) by Gérald Barré.
* [Porting a .NET Framework library to .NET Core](https://www.codeproject.com/Articles/1190475/Porting-a-NET-Framework-library-to-NET-Core) by Harsh Gupta 9.
* [Creating your first shared library in .NET Core](https://www.devtrends.co.uk/blog/creating-your-first-shared-library-in-.net-core) by Paul Hiles.
* [Contributing to the .NET Core SDK source code for the first time, and how OSS helped me](https://jeremylindsayni.wordpress.com/2017/06/10/contributing-to-the-net-core-sdk-source-code-for-the-first-time/) by Jeremy Lindsay.
* [5 Reasons You Should Stop Using System.Drawing from ASP.NET](http://photosauce.net/blog/post/5-reasons-you-should-stop-using-systemdrawing-from-aspnet) by Clinton Ingram.
* [First Foray into .NET Core 2.0](http://thedatafarm.com/data-access/first-foray-into-net-core-2-0/) by Julie Lerman.
* [Testavior: A Happy Solution To Test Your ASP.NET Core Applications](http://geeklearning.io/testavior-a-happy-solution-to-test-your-asp-net-core-applications/) by Arnaud.
* [Getting Started with PowerShell Core on Windows, Mac, and Linux](https://blogs.msdn.microsoft.com/powershell/2017/06/09/getting-started-with-powershell-core-on-windows-mac-and-linux/) by Ashley McGlone.
* [Logging with Log4Net and Common Logging](https://blog.couchbase.com/logging-log4net-common-logging/) by Matthew Groves.
* [Implementing a scheduler for your orchestrations](https://blog.scooletz.com/2017/06/08/implementing-a-scheduler-for-your-orchestrations/) by Szymon Kulec 'Scooletz'.

## ASP.NET

* [Defining custom logging messages with LoggerMessage.Define in ASP.NET Core](https://andrewlock.net/defining-custom-logging-messages-with-loggermessage-define-in-asp-net-core/) by Andrew Lock.
* [Defining ASP.NET Core Controller action constraint to match the correct action](https://blogs.msdn.microsoft.com/premier_developer/2017/06/05/defining-asp-net-core-controller-action-constraint-to-match-the-correct-action/) by Pam Lahoud.
* [Excluding the node_modules folder when publishing ASP.NET projects.](http://cecilphillip.com/excluding-node_modules-folder-from-publishing/) by Cecil Phillip.
* [Simplest Possible ASP.NET Core Web Application in Docker for Windows](http://iamnotmyself.com/2017/05/07/simplest-possible-asp-net-core-web-application-in-docker-for-windows/) by Bobby Johnson.
* [Theming in ASP.NET Core](http://www.hishambinateya.com/theming-in-asp.net-core) by Hisham Bin Ateya.
* [Keep your ASP.NET Core application’s secrets safe during development](https://jonhilton.net/2017/06/07/keep-your-asp-dot-net-application-secrets-safe/) by Jon Hilton.
* [Self Descriptive HTTP API in ASP.NET Core: Hypermedia Clients](https://codeopinion.com/self-descriptive-http-api-in-asp-net-core-hypermedia-clients/) by Derek Comartin.
* [OpenID Connect Session Management using an Angular application and IdentityServer4](https://damienbod.com/2017/06/11/openid-connect-session-management-an-angular-application-using-identityserver4/) by Damien Bowden.
* [Building the Ultimate RestSharp Client in ASP.NET and C#](https://www.exceptionnotfound.net/building-the-ultimate-restsharp-client-in-asp-net-and-csharp/) by Matthew Jones.
* [ASP.NET Core POCO Controllers](https://www.simple-talk.com/dotnet/asp-net/control-controller-asp-net-mvc/) by Dino Esposito.
* [Feeding Server Timing API from ASP.NET Core](https://www.tpeczek.com/2017/06/feeding-server-timing-api-from-aspnet.html) by Tomasz Pęczek.

## C#

* [C# 7 Features with Mads Torgersen](https://channel9.msdn.com/Shows/Code-Conversations/C-7-Features-with-Mads-Torgersen) by Mads Torgersen, Maria Naggaga, and Jon Galloway.
* [An Early Look at C# 7.1: Part 1](https://www.infoq.com/news/2017/06/CSharp-7.1-a?utm_campaign=infoq_content&utm_source=infoq&utm_medium=feed&utm_term=global) by InfoQ.
* [C# 7.2 and 8.0 Roadmap](https://www.infoq.com/news/2017/06/CSharp-7.2) by InfoQ.
* [C# 7 Series, Part 3: Default Literals](https://blogs.msdn.microsoft.com/mazhou/2017/06/06/c-7-series-part-3-default-literals/) by Mark Zhou.
* [Disambiguate method trick based on generic constraint](http://metacoding.azurewebsites.net/2017/06/08/disambiguous-methods-trick-based-on-generic-constraint/) by Matthieu Mezil.
* [Top 20 Recommended Microsoft Build 2017 Sessions for C# Developers](https://www.alvinashcraft.com/2017/06/07/top-20-recommended-microsoft-build-2017-sessions-for-c-developers/) by Alvin Ashcraft.
* [Null checking allocations and mass refactoring with Resharper](https://surfingthecode.com/2017/06/null-checking-allocations-and-mass-refactoring-with-resharper/) by Alexander Tsvetkov.
* [Practical C# – LINQ Set Operations](http://www.andreaangella.com/2017/06/practical-csharp-linq-set-operations/) by Andrea Angella.

## F#

* [Kami 2 Solver in F# - Part 1](https://completely-unique-view-blog.appspot.com/posts/kami2-solver-part-1) by Chris Smith.
* [Minimalistic Live Testing Fable Apps With QUnit](https://medium.com/@zaid.naom/minimalistic-live-testing-fable-apps-with-qunit-b9d9f2f64725) by Zaid Ajaj.
* [Parallel Programming with F# and Hopac](https://www.youtube.com/watch?v=bKpRrCssAWM) by Natallia Dzenisenka.
* [Productive Web Applications (F#)](https://www.youtube.com/watch?v=X76iVWa-0e0) by Jeremy Abbot.

There is more content available this week in [F# Weekly](https://sergeytihon.wordpress.com/category/f-weekly/). If you want to see more F# awesomeness, please check it out!

## VB

* [Building web API apps on ASP.NET Core 2 and VB.NET](http://gunnarpeipman.com/2017/06/aspnet-core2-vbnet-weather-api/) by Gunnar Peipman.

## Xamarin

* [Enterprise Application Patterns using Xamarin.Forms](http://www.davidbritch.com/2017/06/enterprise-application-patterns-using.html) by David Britch.
* [Enterprise Apps Made Easy with New Authentication & Microsoft Graph Libraries](https://blog.xamarin.com/enterprise-apps-made-easy-updated-libraries-apis/) by Mayur Tendulka.
* [Unleashed: Embedding Xamarin.Forms in Xamarin Native](https://blog.xamarin.com/unleashed-embedding-xamarin-forms-in-xamarin-native/) by David Ortinau.
* [WWDC 2017 Recap for iOS Developers](https://blog.xamarin.com/wwdc-2017-recap-for-ios-developers/) by Pierce Boggan.
* [Xamarin Developer Events This June](https://blog.xamarin.com/xamarin-developer-events-june/) by Jayme Singleton.
* [Episode 24: Xamarin Live Player](https://channel9.msdn.com/Shows/XamarinShow/Episode-24-Xamarin-Live-Player) by The Xamarin Show.
* [Getting started with MonoGame using XML](https://darkgenesis.zenithmoon.com/getting-started-with-monogame-using-xml/) by Simon Jackson.
* [Creating Custom Controls with Bindable Properties in Xamarin.Forms](https://mindofai.github.io/Creating-Custom-Controls-with-Bindable-Properties-in-Xamarin.Forms/) by Bryan Anthony Garcia.
* [Xamarin Forms, the MVVMLight Toolkit and I: Dependecy Injection](https://msicc.net/xamarin-forms-the-mvvmlight-toolkit-and-i-dependecy-injection/) by Marco Siccardi.
* [Alpha Release: Xamarin Workbooks & Inspector 1.3.0-alpha2](https://releases.xamarin.com/alpha-release-xamarin-workbooks-inspector-1-3-0-alpha2/) by Bri Brothers.
* [Common issues in the Xamarin 15.2.2 release being tracked by the Xamarin team](https://releases.xamarin.com/common-issues-in-the-xamarin-15-2-2-release-being-tracked-by-the-xamarin-team/) by Brendan Zagaeski.
* [Optimize memory usage in Xamarin apps](https://www.chipsncookies.com/2017/optimize-memory-usage-in-xamarin-apps/) by Samuel Debruyn.
* [Mobile Center plugin for fastlane](https://www.hockeyapp.net/blog/2017/06/06/mobile-center-fastlane.html) by Mobile Center Team.
* [Zero to Build: Create new Xamarin apps in minutes with AppMap](https://www.infragistics.com/community/blogs/infragistics/archive/2017/06/08/zero-to-build-creating-new-apps-in-minutes-with-appmap.aspx) by Infragistics.
* [Can't Start Android Emulator on macOS? It's Probably Conflicting with Docker](https://www.junian.net/2017/06/start-android-emulator-with-docker-on-macos.html) by Junian Triajianto.
* [Xamarin and DevOps: Setting up your iOS CI](https://www.thewissen.io/xamarin-devops-ios-ci/) by Steven Thewissen.
* [Xamarin and DevOps: Versioning your app](https://www.thewissen.io/xamarin-devops-versioning/) by Steven Thewissen.
* [Getting Started with Xamarin Forms for Mac Preview](http://err2solution.com/2017/05/getting-started-with-xamarin-forms-for-mac-preview/) by S Ravi Kumar.
* [Realm Mobile Database with Xamarin Forms Step By Step Guide](http://err2solution.com/2017/06/realm-mobile-database-with-xamarin-forms-step-by-step-guide/) by S Ravi Kumar.
* [NuGet Support in Visual Studio for Mac 7.0](http://lastexitcode.com/blog/2017/06/04/NuGetSupportInVisualStudioMac7-0/) by Matt Ward.
* [Xamarin.Forms & PaintCode](http://thatcsharpguy.com/post/custom-renderer-paint-code-en/) by Antonio Feregrino Bolaños.

## Azure

* [Azure via C# – Delete Azure Blobs](http://www.andreaangella.com/2017/06/azure-via-csharp-delete-azure-blobs/) by Andrea Angella.
* [Azure via C# – Download Azure Blobs](http://www.andreaangella.com/2017/06/azure-via-csharp-download-azure-blobs/) by Andrea Angella.* [Upgrade your .Net Core Service Fabric Microservices from VS 2015 to VS 2017](http://www.medic-consulting.com/2017/06/07/Upgrade-your-Net-Core-Service-Fabric-Microservices-from-VS-2015-to-VS-2017/) by Andrej Medic.
* [Architecting Azure Functions: Function Timeouts and Work Fan-Out with Queues](http://dontcodetired.com/blog/post/Architecting-Azure-Functions-Function-Timeouts-and-Work-Fan-Out-with-Queues) by Jason Roberts.
* [Remote debug your Azure App Service 2017 including ASP.NET Core](https://blogs.msdn.microsoft.com/benjaminperkins/2017/06/06/remote-debug-your-azure-app-service-2017-including-asp-net-core/) by Benjamin Perkins.

## UWP

* [Native Ads in Microsoft Advertising SDK](https://blogs.windows.com/buildingapps/2017/06/05/native-ads-microsoft-advertising-sdk/) by Vivek Mohan.
* [Using color fonts for beautiful text and icons](https://blogs.windows.com/buildingapps/2017/06/06/using-color-fonts-beautiful-text-icons/) by Rick Manning.

## Data

* [Reduce Overhead When Retrieving Objects with Entity Framework](https://visualstudiomagazine.com/articles/2017/06/01/reduce-overhead.aspx) by Peter Vogel.
* [LLBLGen Pro for .NET and .NET Core - Database Entity Modeling with any ORM](https://www.hanselman.com/blog/LLBLGenProForNETAndNETCoreDatabaseEntityModelingWithAnyORM.aspx) by Scott Hanselman.

## Game development

* [Advanced VR Mechanics With Unity and the HTC Vive – Part 2](https://www.raywenderlich.com/159610/advanced-vr-mechanics-unity-htc-vive-part-2) by Eric Van de Kerckhove.
* [Getting started with MonoGame using XML](http://www.gamasutra.com/blogs/SimonJackson/20170602/299228/Getting_started_with_MonoGame_using_XML.php) by Simon Jackson.
* [Sid Meier and Bruce Shelley's postmortem of Civilization](http://www.gamasutra.com/view/news/299588/Video_Sid_Meier_and_Bruce_Shelleys_postmortem_of_Civilization.php) by Sid Meier and Bruce Shelley.
* [Getting started with Mixer Interactivity in Unity](https://channel9.msdn.com/Shows/dotGAME/Getting-started-with-Mixer-Interactivity) by Stacey Haffner.

And this is it for this week!

Contribute to the week in .NET
------------------------------

As always, this weekly post couldn't exist without community contributions, and I'd like to thank all those who sent links and tips. The F# section is provided by [Phillip Carter](https://twitter.com/_cartermp), the gaming section by [Stacey Haffner](https://twitter.com/yecats131), the Xamarin section by [Dan Rigby](https://twitter.com/DanRigby), and the Azure and UWP section by [Michael Crump](http://twitter.com/mbcrump).

You can participate too. Did you write a great blog post, or just read one? Do you want everyone to know about an amazing new contribution or a useful library? Did you make or play a great game built on .NET?
We'd love to hear from you, and feature your contributions on future posts. Please [add your posts](https://weekindotnet.azurewebsites.net), it takes only a second.

We pick the articles based on the following criteria: the posts must be about .NET, they must have been published this week, and they must be original contents. Publication in Week in .NET is not an endorsement from Microsoft or the authors of this post.

This week's post (and future posts) also contains news I first read on [The ASP.NET Community Standup](https://blogs.msdn.microsoft.com/webdev/tag/communitystandup/), on [Weekly Xamarin](http://weeklyxamarin.com/), on [F# weekly](https://sergeytihon.wordpress.com/category/f-weekly/), and on [The Morning Brew](http://themorningbrew.net/).