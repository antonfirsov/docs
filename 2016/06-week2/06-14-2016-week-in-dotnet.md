The week in .NET - 6/14/2016
============================

To read last week's post, see [The week in .NET – 06/07/2016](https://blogs.msdn.microsoft.com/dotnet/2016/06/07/the-week-in-net-06072016/).

On .NET
-------

Last week, we spoke with Daniel Egloff from [QuantAlea](http://quantalea.com/) about their very cool GPU technology that enables C# and F# code to be compiled for modern GPUs, thus unlocking massively parallel computations with performance that is out of reach of regular CPUs.

<iframe width="560" height="315" src="https://www.youtube-nocookie.com/embed/QJ93BMCo_XI?rel=0" frameborder="0" allowfullscreen></iframe>

This week, we speak with [Pratap Lakshman](https://www.youtube.com/watch?v=TmLOLCAp1N8) about MS Test.

Package of the week: ELMAH
--------------------------

[ELMAH](https://code.google.com/p/elmah/) (Error Logging Modules and Handlers) is an application-wide error logging facility that is completely pluggable. It can be dynamically added to a running ASP.NET web application, or even all ASP.NET web applications on a machine, without any need for re-compilation or re-deployment.

Once ELMAH has been dropped into a running web application and configured appropriately, you get the following facilities without changing a single line of your code: logging of nearly all unhandled exceptions, a web page to remotely view the entire log of recoded exceptions, a web page to remotely view the full details of any one logged exception, including colored stack traces, in many cases, you can review the original yellow screen of death that ASP.NET generated for a given exception, even with customErrors mode turned off, an e-mail notification of each error at the time it occurs, and an RSS feed of the last 15 errors from the log.

![ELMAH](http://www.tigraine.at/wp-content/uploads/2009/04/image2.png)

Game of the Week: Endless Space
-------------------------------

[Endless Space](http://madewith.unity.com/games/endless-space) is a 4X turn-based strategy game and the winner of the 2013 Unity Golden Cube and Community Choice awards. In Endless Space, players choose between expanding, exploring, exploiting or exterminating as they control their civilization while racing to colonize space and dominate the Dust market. Players can select one of eight civilizations and explore hundreds of star systems and planets. There is no lack of content or replayability in Endless Space, as you are able to control the game's scope and generate random galaxies each time you choose to play.   

![gamescreen](https://cloud.githubusercontent.com/assets/4108756/16045083/ce0606d8-31fb-11e6-8dc2-7b665d29c0b1.PNG)

Endless Space was created by [Amplitude Studios](http://madewith.unity.com/profiles/amplitude-studios) using [Unity](http://unity3d.com/) and [C#](https://channel9.msdn.com/Series/C-Sharp-Fundamentals-Development-for-Absolute-Beginners). It is currently available on Mac and Windows. More information can be found on their [Made With Unity](http://madewith.unity.com/games/endless-space) page.

User group meeting of the week: Introduction to ASP.NET Core 1.0 and the dotnet CLI in NYC
------------------------------------------------------------------------------------------

Next Monday, June 20 at 6:30, Maria Naggaga Nakanwagi will be at the [New York Pluralsight Study Group](http://www.meetup.com/NYPluralsightStudy/) for [an introduction to ASP.NET Core and the .NET CLI](http://www.meetup.com/NYPluralsightStudy/events/231877444/).

.NET
----

* [Visual Studio 2015 Update 3 RC](https://blogs.msdn.microsoft.com/visualstudio/2016/06/07/visual-studio-2015-update-3-rc/) by John Montgomery.
* [A dotnetConf 2016 recap with links to all the talks](https://blogs.msdn.microsoft.com/dotnet/2016/06/09/thank-you-for-watching-dotnetconf-2016/) by Beth Massi.
* [Announcing a new .NET and ASP.NET Core Bug Bounty](https://blogs.msdn.microsoft.com/webdev/2016/06/07/announcing-a-new-net-and-asp-net-core-bug-bounty/) by Barry Dorrans.
* [Cake joins the .NET Foundation](http://cakebuild.net/blog/2016/06/cake-joins-dotnetfoundation), and [Cake v0.13.0 released](http://cakebuild.net/blog/2016/06/cake-v0-13-0-released).
* [.NET Platform Standard and the magic of "imports"](https://blogs.infosupport.com/net-platform-standard-and-the-magic-of-imports/) by Jonathan Mezach.
* [Announcing MSTest Framework support for .NET Core RC2 / ASP.NET Core RC2](https://blogs.msdn.microsoft.com/visualstudioalm/2016/05/30/announcing-mstest-framework-support-for-net-core-rc2-asp-net-core-rc2/) by Pratap Lakshman.
* [Implementing a Markdown Engine for .NET](http://xoofx.com/blog/2016/06/13/implementing-a-markdown-processor-for-dotnet/) by Alexandre Mutel.
* [Designer Support for EF Core via Devart](http://thedatafarm.com/data-access/designer-support-for-ef-core-via-devart/)by Julie Lerman.
* [Publishing your first .NET Core NuGet package with AppVeyor and MyGet](https://andrewlock.net/publishing-your-first-nuget-package-with-appveyor-and-myget/), and [Adding Travis CI builds to a .NET Core app](https://andrewlock.net/adding-travis-ci-to-a-net-core-app/) by Andrew Lock.
* [Announcing Scripty, an alternative to T4 for compile-time code generation using the power of Roslyn scripting](http://daveaglick.com/posts/announcing-scripty) by Dave Glick.
* [A Peek into .NET Open Source Contributions](https://blogs.msdn.microsoft.com/webdev/2016/06/10/a-peek-into-net-open-source-contributions/) by Jeffrey T. Fritz.
* [Maybe null is not an option](http://www.westerndevs.com/Fsharp/Functional-programming/maybe-null-is-not-an-option/) by Amir Barylko.
* [Structured logging concepts in .NET part 1](http://nblumhardt.com/2016/06/structured-logging-concepts-in-net-series-1/) and [part 2](https://nblumhardt.com/2016/06/events-and-levels-structured-logging-concepts-in-net-2/) by Nicholas Blumhardt.
* [Tracking down a performance hit](https://codeblog.jonskeet.uk/2016/06/09/tracking-down-a-performance-hit/) by Jon Skeet.

ASP.NET
-------

* [Deploying Docker containers running ASP.NET Core RC2 to Microsoft Azure Cloud](http://laurentkempe.com/2016/06/08/Deploying-Docker-containers-running-ASPNET-Core-RC2-to-Microsoft-Azure-Cloud/index.html) by Laurent Kempé.
* [ASP.NET Core: No more worries about checking in secrets](http://www.jerriepelser.com/blog/aspnet-core-no-more-worries-about-checking-in-secrets) by Jerrie Pelser.
* [Setup ASP.NET Core 1.0 debugging on Ubuntu 16.04](http://zablo.net/blog/post/run-and-debug-asp-net-core-rc2-ubuntu-16-04) by Marcin Zabłocki.
* [Running ASP.NET Core 1.0-RC2 in Docker](https://www.sesispla.net/blog/language/en/2016/05/running-asp-net-core-1-0-rc2-in-docker/) by Sergio Sisternes.

F#
--

* [Mastering .NET Machine Learning (book)](https://www.packtpub.com/big-data-and-business-intelligence/mastering-net-machine-learning) by Jamie Dixon.
* [F# for Machine Learning Essentials (book)](https://www.packtpub.com/big-data-and-business-intelligence/f-machine-learning) by Sudipta Mukherjee.
* [F# in the real world](http://www.slideshare.net/theburningmonk/f-in-the-real-world-ndc/8-why_F), by Yan Cui
* [F# the most highly paid tech worldwide in 2016, but it’s not just for finance.](https://fsharp.tv/gazettes/f-the-most-highly-paid-tech-worldwide-in-2016/) - FSharp TV
* [F# Gotchas for C# Developers](http://dobegin.com/fsharp-gotchas-for-csharp-devs/), by Daniel Lazarenko
* [Suave CoreCLR Sample](https://github.com/SuaveIO/Suave-CoreCLR-sample), a sample Suave.io hello world using .NET Core

Check out [F# Weekly](https://sergeytihon.wordpress.com/category/f-weekly/) for more great content from the F# community.

Xamarin
-------

* [.NET Conf Day 2 Keynote (video)](https://channel9.msdn.com/Events/dotnetConf/2016/NET-Conf-Day-2-Keynote-Miguel-de-Icaza) by Miguel de Icaza.
* [Xamarin for Visual Studio 4.1 Preview](https://developer.xamarin.com/releases/vs/xamarin.vs_99/xamarin.vs_99.0/).
* [Integrating MixPanel Analytics into your Xamarin.iOS app](http://tech.justgiving.com/2016/06/08/integrating-mixpanel-analytics-into-your-xamarin-ios-app/) by Mark Gibaud.
* [A detailed look at what's new in Xamarin.iOS for Visual Studio](https://xamarinhelp.com/new-xamarin-ios-visual-studio/) by Adam Pedley.
* [Fun with Expressions](http://blog.robmikh.com/xaml/uwp/composition/2016/04/28/fun-with-expressions.html) by Robert Mikhayelyan.
* [Global resources in Xamarin.Forms](http://jesseliberty.com/2016/05/19/global-resources-in-xamarin-forms/) by Jesse Liberty.

Games
-----

* [How to Make a Game Like Bomberman](https://www.raywenderlich.com/125559/make-game-like-bomberman) by Eric Van de Kerckhove.
* [Tutorial: Save And Load system - How to save unity stuff in one file part #1 (Video)](https://www.youtube.com/watch?v=30gE2M8SCi0&feature=youtu.be) by Gamad.
* [Introducing C# Developers to Building Games with Unity - For the Hobby Developer (Video)](https://channel9.msdn.com/events/dotnetConf/2016/Introducing-C-Developers-to-Building-Games-with-Unity-For-the-Hobby-Developer) by Stacey Haffner.

And this is it for this week!

Contribute to the week in .NET
------------------------------

As always, this weekly post couldn't exist without community contributions, and I'd like to thank all those who sent links and tips.

You can participate too. Did you write a great blog post, or just read one? Do you want everyone to know about an amazing new contribution or a useful library? Did you make or play a great game built on .NET?
We'd love to hear from you, and feature your contributions on future posts:

* Send an email to beleroy at Microsoft,
* [comment on this gist](https://gist.github.com/bleroy/cd10bebc745f23d3dc6c333c9d81cc23)
* Leave us a pointer in the comments section below.
* [Send Stacey (@yecats131) tips on Twitter about .NET games](https://twitter.com/yecats131).

This week's post (and future posts) also contains news I first read on [The ASP.NET Community Standup](https://blogs.msdn.microsoft.com/webdev/tag/communitystandup/), on [Weekly Xamarin](http://weeklyxamarin.com/), on [F# weekly](https://sergeytihon.wordpress.com/category/f-weekly/), on [ASP.NET Weekly](http://www.aspnetweekly.com/), and on [Chris Alcock's The Morning Brew](http://themorningbrew.net/).
