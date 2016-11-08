The week in .NET - .NET Foundation - Serilog - Super Dungeon Bros
=================================================================

To read last week's post, see [The week in .NET – .NET, ASP.NET, EF Core 1.1 Preview 1 – On .NET on EF Core 1.1 – Changelog – FluentValidation – Reverse: Time Collapse](https://blogs.msdn.microsoft.com/dotnet/2016/10/25/the-week-in-net-net-asp-net-ef-core-1-1-preview-1-on-net-on-ef-core-1-1-changelog-fluentvalidation-reverse-time-collapse/).

On .NET: Martin Woodward on the .NET Foundation
-----------------------------------------------

Last week, [Martin Woodward was on the show to talk about the .NET Foundation](https://channel9.msdn.com/Shows/On-NET/Martin-Woodward-NET-Foundation):

<iframe src="https://channel9.msdn.com/Shows/On-NET/Martin-Woodward-NET-Foundation/player" width="960" height="540" allowFullScreen frameBorder="0"></iframe>

This week, we'll speak with Mei-Chin Tsai and Jan Kotas about [CoreRT and .NET Native](https://msdn.microsoft.com/en-us/library/dn807190(v=vs.110).aspx) and .NET. The show is on Thursdays and begins at 10AM Pacific Time [on Channel 9](https://channel9.msdn.com/Shows/On-NET). We'll take questions on Gitter, on [the dotnet/home channel](https://gitter.im/dotnet/home) and on Twitter. Please use the `#onnet` tag. It's OK to start sending us questions in advance if you can't do it live during the show.

Package of the week: Serilog
----------------------------------

Modern applications can be complex, busy, asynchronous and distributed. This adds up to make understanding behavior and finding bugs a significant challenge. While tools for monitoring and debugging apps are always improving, [Serilog](https://serilog.net) helps by capturing log data in a form that's substantially easier for tooling to work with.

On the surface, Serilog looks like most logging libraries:

```csharp
Log.Information("Request completed in {Elapsed} ms", sw.ElapsedMilliseconds);
```

While messages can be formatted into text, Serilog uses named placeholders to capture and preserve parameters like `Elapsed` as first-class event properties:

```json
{"@t":"2016-06-07T03:44:57.853Z","@m":"Request completed in 18 ms","Elapsed":18}
```

Many of the Serilog [sinks](https://github.com/serilog/serilog/wiki/Provided-Sinks) accept data in structured formats like JSON, so searches like `Elapsed > 10` can be answered directly, without the need for regular expressions or log parsing.

Serilog is built from the ground up for distributed logging, and comes with a rich set of features for grouping, enriching and correlating log events. The project is [open source and developed by a dedicated community](https://github.com/serilog/serilog) on GitHub.

Game of the Week: Super Dungeon Bros
------------------------------------

[Super Dungeon Bros](https://madewith.unity.com/games/super-dungeon-bros) is a fast paced dungeon brawler where you can play with up to four friends. Complete quests from the Gods of Rock with heavy metal heroes Axl, Lars, Freddie and Ozzie (get it?). You and your friends must explore and fight your way through the deepest, darkest dungeons of Rökheim, searching for epic loot and the legends of fabled rock stars as you solve puzzles and destroy undead monsters. Super Dungeon Bros features cross-platform multiplayer, multiple worlds, randomly generated dungeons and a series of daily and weekly dungeon challenges.

![screenshot](https://cloud.githubusercontent.com/assets/4108756/19894765/6517b896-a00b-11e6-91a8-6efbf0850645.jpg)

[Super Dungeon Bros]( https://madewith.unity.com/games/super-dungeon-bros) is being developed by [React Games](http://www.reactgames.com/) using [Unity](https://unity3d.com/) and [C#](https://channel9.msdn.com/Series/C-Sharp-Fundamentals-Development-for-Absolute-Beginners). It is available for Xbox One, PlayStation 4 and [Steam](http://store.steampowered.com/app/306000/).

User group meeting of the week: Intro to Azure DocumentDB in Tallahassee, FL
----------------------------------------------------------------------------

On Thursday, November 3, at 6:00PM, [The Capital City .NET user Group](https://www.meetup.com/tally-dot-net/) will give [an intro to Azure DocumentDB for .NET and SQL Server Developers with Santosh Hari](https://www.meetup.com/tally-dot-net/events/233768568/). Santosh will build a simple ASP.NET MVC web app that uses C# and DocumentDB for storing data. Then he'll walk through writing queries for DocumentDB by leveraging SQL and LINQ querying skills.

.NET
----

* [Announcing NuGet 3.5 RTM](http://blog.nuget.org/20161027/Announcing-NuGet-3.5-RTM.html) by Harikrishna Menon.
* [Using dotnet watch test for continuous testing with .NET Core and XUnit.net](http://www.hanselman.com/blog/UsingDotnetWatchTestForContinuousTestingWithNETCoreAndXUnitnet.aspx) by Scott Hanselman.
* [What's new in Serilog?](https://nblumhardt.com/2016/10/october-serilog-update/) by Nicholas Blumhardt.
* [Entity Framework Core – Table per Hierarchy](https://csharp.christiannagel.com/2016/10/27/efcore_tph/) by Christian Nagel.
* [How does the 'fixed' keyword work?](http://mattwarren.org/2016/10/26/How-does-the-fixed-keyword-work/) by Matt Warren.
* [A monthly compilation of community posts](http://blog.myget.org/category/Community-news.aspx) by MyGet.
* [Using NDepend to Help Improve Architecture](https://stevedesmond.ca/blog/using-ndepend-to-help-improve-architecture) by Steve Desmond.
* [RavenDB 3.5 RTM released](https://ayende.com/blog/175937/ravendb-3-5-rtm-released) by Ayende Rahien.
* [Project.json made my life easier and it is not a joke](https://stapp.space/project-json-made-my-life-easier-and-it-is-not-a-joke/) by Piotr Stapp.
* [Implementing LUIS Routing within BotFramework](http://robinosborne.co.uk/2016/10/28/implementing-luis-routing-within-botframework/) by Robin Osborne.
* [Automating Installation Builds and Chocolatey Packaging](https://weblog.west-wind.com/posts/2016/Oct/25/Automating-Installation-Builds-and-Chocolatey-Packaging) by Rick Strahl.
* [InfoQ eMag: A Preview of C# 7 (free eBook)](https://www.infoq.com/minibooks/emag-c-sharp-preview).
* [Interception in .NET – Part 4: An Interception Framework](https://weblogs.asp.net/ricardoperes/interception-in-net-part-4-an-interception-framework) by Ricardo Peres.

ASP.NET
-------

* [Free ASP.NET Core 1.0 Training on Microsoft Virtual Academy](http://www.hanselman.com/blog/FreeASPNETCore10TrainingOnMicrosoftVirtualAcademy.aspx) by Scott Hanselman.
* [Bearer Token Authentication in ASP.NET Core](https://blogs.msdn.microsoft.com/webdev/2016/10/27/bearer-token-authentication-in-asp-net-core/) by Jeffrey T. Fritz.
* [Angular2 CLI with ASP.NET Core application - tutorial](https://devblog.dymel.pl/2016/10/25/angular2-cli-with-aspnet-core-application-tutorial/) by Michał Dymel.
* [Step by step: Scale ASP.NET Core with Docker Swarm](https://carlos.mendible.com/2016/10/30/step-by-step-scale-asp-net-core-with-docker-swarm/) by Carlos Mendible.
* [Resource-based authorisation in ASP.NET Core](https://andrewlock.net/resource-specific-authorisation-in-asp-net-core/) and [Accessing services when configuring MvcOptions in ASP.NET Core](https://andrewlock.net/accessing-services-when-configuring-mvcoptions-in-asp-net-core/) by Andrew Lock.
* [Angular2 search with ASP.NET Core and Elasticsearch](https://damienbod.com/2016/10/29/angular2-search-with-asp-net-core-and-elasticsearch/) by Damien Bowden.
* [Testing SSL in ASP.NET Core](http://wildermuth.com/2016/10/26/Testing-SSL-in-ASP-NET-Core) by Shawn Wildermuth.
* [ASP.NET Core and the Enterprise Part 2: Hosting](http://odetocode.com/blogs/scott/archive/2016/10/25/asp-net-core-and-the-enterprise-part-2-hosting.aspx) by K. Scott Allen.
* [Vertical Slice Test Fixtures for MediatR and ASP.NET Core](https://lostechies.com/jimmybogard/2016/10/24/vertical-slice-test-fixtures-for-mediatr-and-asp-net-core/) by Jimmy Bogard.
* [Run & Deploy ASP.NET Core Web Applications on Ubuntu behind Apache Server](http://www.codeproject.com/Articles/1137493/Deploy-ASP-Net-Core-Web-Applications-on-Ubuntu-Lin) by Sumit Chauhan.

F#
--

* [F# Domain Modeling](http://lukemerrett.com/fsharp-domain-modelling/), by Luke Merrett.
* [Paket 'why' command](http://theimowski.com/blog/2016/10-30-paket-why-command/index.html), by Tomasz Heimowski.
* [F# Language Suggestions are now on GitHub](https://twitter.com/dsyme/status/792331578676506624).
* [Yahtzee Scoring Kata in F#](http://markheath.net/post/yahtzee-kata-fsharp), by Mark Heath.
* [F# support on .NET Core SDK Preview 3](https://twitter.com/enricosada/status/790701057479413760?ref_src=twsrc%5Etfw).

Check out [F# Weekly](https://sergeytihon.wordpress.com/category/f-weekly/) for more great content from the F# community.

Xamarin
-------

* [Last Xamarin Dev Days of 2016](https://blog.xamarin.com/last-xamarin-dev-days-of-2016/) by Jayme Singleton.
* [The Xamarin Show Snack Pack 3: Xamarin Test Recorder for macOS](https://channel9.msdn.com/Shows/XamarinShow/Snack-Pack-3-Xamarin-Test-Recorder-for-macOS) by James Montemagno.
* [Xamarin Android 9-Patch Image Splashscreen](https://xamarinhelp.com/xamarin-android-9-patch-image-splashscreen/), [Navigating in Xamarin Forms](https://xamarinhelp.com/navigating-xamarin-forms/), and [Configuration Files In Xamarin Forms](https://xamarinhelp.com/configuration-files-xamarin-forms/) by Adam Pedley.
* [Better Navigation in Xamarin.Forms](http://jesseliberty.com/2016/10/21/better-navigation-in-xamarin-forms) by Jesse Liberty.

Azure
-----

* [Announcing Azure Storage Client Library GA for Xamarin](https://azure.microsoft.com/en-us/blog/announcing-storage-client-library-ga-for-xamarin/) by Dinesh Murthy.

Games
-----

* [Unite '16 Keynote (video)](https://www.youtube.com/watch?v=h5iBcVYluRs).
* [Introducing Holographic Emulation (video)](https://blogs.unity3d.com/2016/10/28/introducing-holographic-emulation-2/) by Peter Freese.
* [MonoGame Live #6 : XNA Sample Conversion, Localisation (video)](https://www.youtube.com/watch?v=mcafPXwG7Rg) by MonoGame.
* [Unity - 2D Movement (part 6a) - Animation : Wheels](https://www.youtube.com/watch?v=BNFLhgw_H44) and [Unity - 2D Movement (Part 6b) - Animation : Tread (video)](https://www.youtube.com/watch?v=1m6y2UdTxdY) by Pixel Make.

And this is it for this week!

Contribute to the week in .NET
------------------------------

As always, this weekly post couldn't exist without community contributions, and I'd like to thank all those who sent links and tips. The F# section is provided by [Phillip Carter](https://twitter.com/_cartermp), the gaming section by [Stacey Haffner](https://twitter.com/yecats131), and the Xamarin section by [Dan Rigby](https://twitter.com/DanRigby).

You can participate too. Did you write a great blog post, or just read one? Do you want everyone to know about an amazing new contribution or a useful library? Did you make or play a great game built on .NET?
We'd love to hear from you, and feature your contributions on future posts:

* Send an email to beleroy at Microsoft,
* [comment on this gist](https://gist.github.com/bleroy/7071a1119c6538ae46fa5cb7f2eb128b)
* Leave us a pointer in the comments section below.
* [Send Stacey (@yecats131) tips on Twitter about .NET games](https://twitter.com/yecats131).

This week's post (and future posts) also contains news I first read on [The ASP.NET Community Standup](https://blogs.msdn.microsoft.com/webdev/tag/communitystandup/), on [Weekly Xamarin](http://weeklyxamarin.com/), on [F# weekly](https://sergeytihon.wordpress.com/category/f-weekly/), and on [Chris Alcock's The Morning Brew](http://themorningbrew.net/).
