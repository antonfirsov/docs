The week in .NET - .NET Core, ASP.NET Core, EF Core 1.1 - Docker - Xenko
========================================================================

To read last week's post, see [The week in .NET – Mitch Muenster – Stateless](https://blogs.msdn.microsoft.com/dotnet/2016/11/15/the-week-in-net-mitch-muenster-stateless/).

.NET Core, ASP.NET Core, EF Core 1.1
------------------------------------

This week, at the [Connect(); // 2016](https://connectevent.microsoft.com/) event, we made a number of announcements, including [Visual Studio 2017 RC](https://blogs.msdn.microsoft.com/visualstudio/2016/11/16/visual-studio-2017-rc/), [Visual Studio for Mac Preview](https://blogs.msdn.microsoft.com/visualstudio/2016/11/16/visual-studio-for-mac/), [SQL Server on Linux Preview](https://blogs.technet.microsoft.com/dataplatforminsider/2016/11/16/announcing-sql-server-on-linux-public-preview-first-preview-of-next-release-of-sql-server/), [.NET Core 1.1](https://blogs.msdn.microsoft.com/dotnet/2016/11/16/announcing-net-core-1-1/), [Entity Framework Core 1.1](https://blogs.msdn.microsoft.com/dotnet/2016/11/16/announcing-entity-framework-core-1-1/), and [ASP.NET Core 1.1](https://blogs.msdn.microsoft.com/webdev/2016/11/16/announcing-asp-net-core-1-1/). Check out the announcement posts for all the details.

On .NET
-------

Last week, [Michael Friis and Glenn Condron were on the show](https://channel9.msdn.com/Shows/On-NET/Michael-Friis-Docker) to talk about Docker and .NET:

<iframe src="https://channel9.msdn.com/Shows/On-NET/Michael-Friis-Docker/player" width="960" height="540" allowFullScreen frameBorder="0"></iframe>

This week, we'll speak with Chad Z. Hower a.k.a. Kudzu to talk about [Cosmos](https://github.com/CosmosOS/Cosmos#c-open-source-managed-operating-system), an operating system "construction kit" built with the IL2CPU compiler, C#, and X#, a homebrew language that's part of the project. This week, because of the Tanksgiving week-end, the show is exceptionally on Wednesday, and begins at 10AM Pacific Time [on YouTube](https://www.youtube.com/watch?v=wgEBo-u19Wc). We'll take questions on the video's built-in chat.

Package of the week: mahapps.metro
----------------------------------

[mahapps.metro](http://mahapps.com/) is a toolkit to build Metro-style WPF applications that's used by [the Xamarin Inspector](https://developer.xamarin.com/guides/cross-platform/inspector/), [Xamarin Workbooks](https://developer.xamarin.com/guides/cross-platform/workbooks/) and [Markdown Monster](https://markdownmonster.west-wind.com/).

![mahapps.metro](https://github.com/MahApps/MahApps.Metro/raw/develop/docs/main_demo_window.png)

Tool of the Week: Xenko
-----------------------

[Xenko](http://xenko.com/) is an open-source C# game engine that comes with a full toolchain and development environment.

![Xenko Game Studio](http://xenko.com/images/top/editor_game.jpg)

Xenko's new [Script Editor Preview](http://xenko.com/blog/script-editor/) brings complete integration of script code in the Game Studio.

<video autoplay="" loop="" class="responsive-video" poster="../../images/blog/script_editor/create_script_gamestudio.jpg">
   <source src="../../images/blog/script_editor/create_script_gamestudio.mp4" type="video/mp4">
</video>

User group meeting of the week: Cognitive Services, AI as an API in Burlington, MA
------------------------------------------------

The [New England Microsoft Developers group](https://www.meetup.com/NE-MSFT-Devs/) holds [a meeting on Thursday, December 1 at 6:30PM in Burlington, MA](https://www.meetup.com/NE-MSFT-Devs/events/235253966/) about [Microsoft Cognitive Services](https://www.microsoft.com/cognitive-services) and how they give easy access to sentiment analysis, speech recognition, picture and video tagging, search, spell check, and more.

.NET
----

* [Announcing .NET Core 1.1](https://blogs.msdn.microsoft.com/dotnet/2016/11/16/announcing-net-core-1-1/) by Rich Lander.
* [Announcing Entity Framework Core 1.1](https://blogs.msdn.microsoft.com/dotnet/2016/11/16/announcing-entity-framework-core-1-1/) by Rowan Miller.
* [Announcing .NET Core Tools MSBuild "alpha"](https://blogs.msdn.microsoft.com/dotnet/2016/11/16/announcing-net-core-tools-msbuild-alpha/) by Rich Lander.
* [Live Unit Testing in Visual Studio 2017 RC](https://blogs.msdn.microsoft.com/visualstudio/2016/11/18/live-unit-testing-visual-studio-2017-rc/) by Joe Morris.
* [Put a .NET Core App in a Container with the new Docker Tools for Visual Studio](https://blogs.msdn.microsoft.com/webdev/2016/11/16/new-docker-tools-for-visual-studio/) by Jeffrey T. Fritz.
* [Lucene.NET status update (October '16)](http://code972.com/blog/2016/10/99-lucene-net-status-update-october-16) by Itamar Syn-Hershko.
* [Xamarin.Forms and .NET Core are the future for Tizen and a great new opportunity for .NET developers](http://www.maherjendoubi.io/xamarin-forms-and-net-core-are-the-future-for-tizen-and-a-great-new-opportunity-for-net-developers-2/) by Maher Jendoubi.
* [Waf DotNetPad has been updated for C# 7 and VB 15](https://jbe2277.github.io/dotnetpad/).
* [Making ConcurrentDictionary GetOrAdd thread safe using Lazy](https://andrewlock.net/making-getoradd-on-concurrentdictionary-thread-safe-using-lazy/) by Andrew Lock.
* [Concurrent conditional deletes](https://blog.scooletz.com/2016/11/16/concurrent-conditional-deletes/) by Szymon Kulec 'Scooletz'.
* [Refactor That Code! Get those database calls out of your controllers](http://makingoutwith.net/2016/refactoring-db-calls-out-of-controllers/) by Joe Petrakovich.
* [Troubleshooting Installing .NET Core 1.1 RTM on macOS](http://tattoocoder.com/troubleshooting-installing-net-core-1-1-rtm-on-osx/) by Shayne Boyer.
* [Using .NET Core Configuration with legacy projects](http://benfoster.io/blog/net-core-configuration-legacy-projects) by Ben Foster.
* [Fat Controller CQRS Diet: Command Pipeline](http://codeopinion.com/fat-controller-cqrs-diet-command-pipeline/) by Derek Comartin.
* [I like my performance unsafely](https://ayende.com/blog/176038/making-code-faster-i-like-my-performance-unsafely) by Ayende Rahien.
* [Extending the dotnet core cli: introducing dotnet-prop](http://codeclimber.net.nz/archive/2016/11/16/Extending-the-dotnet-core-cli-introducing-dotnet-prop.aspx) by Simone Chiaretta.
* [The semantics of ILogger.BeginScope()](https://nblumhardt.com/2016/11/ilogger-beginscope/) by Nicholas Blumhardt.

ASP.NET
-------

* [Framework Benchmarks Round 13](https://www.techempower.com/blog/2016/11/16/framework-benchmarks-round-13/) by TechEmpower.
* [Extending Identity in IdentityServer4 to manage users in ASP.NET Core](https://damienbod.com/2016/11/18/extending-identity-in-identityserver4-to-manage-users-in-asp-net-core/) by Damien Bod.
* [Step by step: Expose ASP.NET Core over HTTPS with Docker](https://carlos.mendible.com/2016/11/06/step-by-step-expose-asp-net-core-over-https-with-docker/) by Carlos Mendible.
* [Troubleshooting ASP.NET Core 1.1.0 install problems](https://andrewlock.net/troubleshooting-asp-net-core-1-1-0-install-problems/) by Andrew Lock.
* [Start using Dependency Injection with ASP.NET Core](https://jonhilton.net/2016/11/17/start-using-dependency-injection-asp-net-core/) by Jon Hilton.
* [Dockerizing Nerd Dinner: Part 2, Connecting ASP.NET to SQL Server](https://blog.sixeyed.com/dockerizing-nerd-dinner-part-2-connecting-asp-net-to-sql-server/) by Elton Stoneman.
* [The Advanced Uses of Razor Views in ASP.NET MVC](https://www.simple-talk.com/dotnet/asp-net/advanced-uses-razor-views-asp-net-mvc/) by Dino Esposito.

F#
--

* [What's new in F# 4.1](https://channel9.msdn.com/Events/Connect/2016/118), by Phillip Carter
* [F# Suave app on dotnet core on Kubernetes on Google Cloud](http://blog.2mas.xyz/fsharp-suave-app-on-dotnet-core-on-kubernetes-on-google-cloud/), by Tomas Jansson
* [Using Blobs in Azure Functions with F#](http://markheath.net/post/using-blobs-in-azure-functions-with-fsharp), by Mark Heath
* [F# and .NET Core preview3 (msbuild fsproj vs2017rc) is wip but ok](https://github.com/dotnet/netcorecli-fsc/wiki/.NET-Core-SDK-preview3)
* [Let's Play with Azure Functions](http://lukemerrett.com/lets-play-with-azure-functions/), by  Luke Merrett

New F# language proposals:

* [Shorthand notations for the if expressions](https://github.com/fsharp/fslang-suggestions/issues/519)
* [Reverse selection and slice operators](https://github.com/fsharp/fslang-suggestions/issues/518)
* [Allow Getters and Setters for Primitive Types To Validate and Coerce Values](https://github.com/fsharp/fslang-suggestions/issues/517)
* [Allow Getters and Setters for Record Field Validation](https://github.com/fsharp/fslang-suggestions/issues/516)

Check out [F# Weekly](https://sergeytihon.wordpress.com/category/f-weekly/) for more great content from the F# community.

Xamarin
-------

* [Connect(); Keynote Releases](https://releases.xamarin.com/connect-keynote-releases/) by Adrian Murphy.
* [Microsoft Connect(); 2016 Recap](https://blog.xamarin.com/microsoft-connect-2016-recap/) by Joseph Hill.
* [Introducing Visual Studio Mobile Center (Preview)](https://blogs.msdn.microsoft.com/visualstudio/2016/11/16/visual-studio-mobile-center/) by Nat Friedman.
* [Announcing the new Visual Studio for Mac](https://blogs.msdn.microsoft.com/visualstudio/2016/11/16/visual-studio-for-mac/) by Miguel de Icaza.
* [Xamarin Test Cloud Announcements at Microsoft Connect();](https://blog.xamarin.com/xamarin-test-cloud-announcements-at-microsoft-connect/) by Justin Raczak.
* [The Next Generation of HockeyApp](https://www.hockeyapp.net/blog/2016/11/16/mobile-center-next-generation-hockeyapp.html) by HockeyApp Team.
* [Live Webinar | Get Started with Xamarin and Microsoft Azure](https://blog.xamarin.com/live-webinar-get-started-with-xamarin-and-microsoft-azure/) & [Webinar Recording | Scale Your Mobile Quality: Industry Benchmarks and Testing Best Practices](https://blog.xamarin.com/webinar-recording-scale-your-mobile-quality-industry-benchmarks-and-testing-best-practices/) by Courtney Witmer.
* [Android Nougat Quick Setting Tiles](https://blog.xamarin.com/android-nougat-quick-setting-tiles/) & [Getting Started With & Learning C#](http://motzcod.es/post/153221330862/getting-started-with-csharp) by James Montemagno.
* [App Shortcuts in Xamarin on Android 7.1](http://redth.codes/app-shortcuts-in-xamarin-on-android-7-1/) by Jonathan Dick.
* [Cognitive Services - Seeing the World with Xamarin and Microsoft Computer Vision APIs](https://msdn.microsoft.com/magazine/mt788620) by Alessandro Del Sole.
* [What’s Coming in Xamarin.Forms 2.3](http://www.apcurium.com/whats-coming-xamarin-forms-2-3/) by Mathieu Savaria.
* [Platform Specifics – Xamarin.Forms 2.3.3 Look Ahead](https://codemilltech.com/platform-specifics-xamarin-forms-2-3-3-look-ahead/) by Matthew Soucoup.
* [Platform Specifics in Xamarin Forms](https://xamarinhelp.com/platform-specifics-xamarin-forms/) by Adam Pedley.
* [Talk with Rui Marinho XLabs Founder and Xamarin.Forms Developer](http://www.michaelridland.com/xamarin/talk-with-rui-marinho-xlabs-founder-and-xamarin-forms-developer/) by Michael Ridland.
* [Firebase Cloud Messaging in Xamarin.Android](http://blog.ostebaronen.dk/2016/11/firebase-cloud-messaging-in.html) by Tomasz Cielecki.
* [Xamarin User Interface Testing for Android Apps](http://blog.falafel.com/xamarin-user-interface-testing-for-android-apps/) & [Mobile Testing in the Xamarin Test Cloud](http://blog.falafel.com/mobile-testing-in-the-xamarin-test-cloud/) by Noel Rice.
* [ReactiveUI v7.0.0 released](https://ghuntley.com/archive/2016/11/12/reactiveui-version-7-0-0-released/) by Geoffrey Huntley.
* [Bindable Native Views – Xamarin.Forms 2.3.3 Look Ahead](https://codemilltech.com/bindable-native-views-xamarin-forms-2-3-3-look-ahead/) by Matthew Soucoup.

Azure
-----

* [The DocumentDB client API now supports .NET Core](https://docs.microsoft.com/en-us/azure/documentdb/documentdb-sdk-dotnet-core).
* [Announcing general availability of Azure Functions](https://azure.microsoft.com/en-us/blog/announcing-general-availability-of-azure-functions/) by Yochay Kiriaty
* [How to deploy to Azure Functions using GitHub](http://jameschambers.com/2016/11/deploy-functions-from-github/), [How to organize types in your Azure Function scripts](http://jameschambers.com/2016/11/How-to-organize-types-in-your-scripts/), and [Fan out workloads in Azure Function Apps](http://jameschambers.com/2016/11/Fan-out-workloads-in-Azure-Function-Apps/) by James Chambers.

Games
-----

* [Visual Studio Tools for Unity 3 Preview](https://blogs.msdn.microsoft.com/visualstudio/2016/11/17/visual-studio-tools-for-unity-3-preview/) by Jb Evain.

And this is it for this week!

Contribute to the week in .NET
------------------------------

As always, this weekly post couldn't exist without community contributions, and I'd like to thank all those who sent links and tips. The F# section is provided by [Phillip Carter](https://twitter.com/_cartermp), the gaming section by [Stacey Haffner](https://twitter.com/yecats131), and the Xamarin section by [Dan Rigby](https://twitter.com/DanRigby).

You can participate too. Did you write a great blog post, or just read one? Do you want everyone to know about an amazing new contribution or a useful library? Did you make or play a great game built on .NET?
We'd love to hear from you, and feature your contributions on future posts:

* Send an email to beleroy at Microsoft,
* [comment on this gist](https://gist.github.com/bleroy/098c007e7e8d0abda8bad08e8b372178)
* Leave us a pointer in the comments section below.
* [Send Stacey (@yecats131) tips on Twitter about .NET games](https://twitter.com/yecats131).

This week's post (and future posts) also contains news I first read on [The ASP.NET Community Standup](https://blogs.msdn.microsoft.com/webdev/tag/communitystandup/), on [Weekly Xamarin](http://weeklyxamarin.com/), on [F# weekly](https://sergeytihon.wordpress.com/category/f-weekly/), and on [Chris Alcock's The Morning Brew](http://themorningbrew.net/).
