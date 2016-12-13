The week in .NET - Visual Studio 2017 RC updated, On .NET with Stephen Cleary and Luis Valencia
============================

To read last week's post, see [The week in .NET – On .NET on MyGet – FlexViewer – I Expect You To Die](https://blogs.msdn.microsoft.com/dotnet/2016/12/06/the-week-in-net-on-net-on-myget-flexviewer-i-expect-you-to-die/).

Visual Studio 2017 RC updated
-----------------------------

Yesterday, [Visual Studio 2017 RC got an update](https://blogs.msdn.microsoft.com/visualstudio/2016/12/12/updating-visual-studio-2017-release-candidate/), with further improvements to the `csproj` format. You can read all the .NET Core and `csproj` details in [Updating Visual Studio 2017 RC – .NET Core Tooling improvements](https://blogs.msdn.microsoft.com/dotnet/2016/12/12/updating-visual-studio-2017-rc-net-core-tooling-improvements/), and the ASP.NET changes in [New Updates to Web Tools in Visual Studio 2017 RC](https://blogs.msdn.microsoft.com/webdev/2016/12/12/new-updates-to-web-tools-in-visual-studio-2017-rc/).

On .NET
-------

Last week, I published the first two of our MVP Summit interviews.

[Stephen Cleary talked about his AsyncEx library](https://channel9.msdn.com/Shows/On-NET/Stephen-Cleary-AsyncFx):

<iframe src="https://channel9.msdn.com/Shows/On-NET/Stephen-Cleary-AsyncFx/player" width="960" height="540" allowFullScreen frameBorder="0"></iframe>

[Luis Valencia showed his IoT work with sensor data aggregation using Azure](https://channel9.msdn.com/Shows/On-NET/Luis-Valencia-IoT-sensors-and-Azure):

<iframe src="https://channel9.msdn.com/Shows/On-NET/Luis-Valencia-IoT-sensors-and-Azure/player" width="960" height="540" allowFullScreen frameBorder="0"></iframe>

This week, we'll be back in the studio to speak with Immo Landwerth, Karel Zikmund, and Wes Haggard about the way the .NET team manages the .NET Core open source projects and repositories. The show is on Thursdays and begins at 10AM Pacific Time [on Channel 9](https://channel9.msdn.com/Shows/On-NET). We'll take questions on Gitter, on [the dotnet/home channel](https://gitter.im/dotnet/home) and on Twitter. Please use the `#onnet` tag. It's OK to start sending us questions in advance if you can't do it live during the show.

App of the week: Ulterius
-------------------------

[Ulterius](https://ulterius.io/) is [a complete remote computer access solution](https://blog.ulterius.io/meet-ulterius/) in your browser. It features hardware and process monitoring and management, remote shells (cmd, PowerShell, and bash), file system access, scheduling, webcam access, and remote desktop.

![Ulterius' remote desktop feature](https://i.imgur.com/kCnnjmi.png)

[Ulterius is open source](https://github.com/ulterius) and built with .NET.

Package of the week: Inferno
----------------------------

[Inferno](http://securitydriven.net/inferno/) is a modern, open-source, general-purpose .NET crypto library, that has been [professionally audited](http://securitydriven.net/inferno/SDI-01-report.pdf). It comes to us from [Stan Drapkin](https://twitter.com/sdrapkin), the author of [Security-drive .NET](http://securitydriven.net/).

```csharp
using (var encryptedStream = new FileStream(encryptedFilename, FileMode.Open))
using (var decryptedStream = new FileStream(decryptedFilename, FileMode.Create))
using (var decTransform = new EtM_DecryptTransform(key: key))
{
    using (var cryptoStream = new CryptoStream(encryptedStream, decTransform, CryptoStreamMode.Read))
        cryptoStream.CopyTo(decryptedStream);

    if (!decTransform.IsComplete) throw new Exception("Not all blocks are decrypted.");
}
```

User group meeting of the week: LTS LoGeek Night in Wrocław, Poland
-------------------------------------------------------------------

[LoGeek Night](http://career.luxoft.com/lts/logeek/poland/) is a full day of presentations and discussions in a relaxed atmosphere as hot pizza and cold beer are served. LoGeek Night is on Thursday, December 15, at the Wędrówki Pub in Wrocław.

The presentations include:

* Łukasz Pyrzyk: .NET Core and Open Source in 2017.
* Ivan Koshelev: Advanced queries in LINQ, IQueryable and Expression Trees with examples in Entity Framework 6.
* Andrey Gordienkov: Transitive dependecies: they are not who we think they are.

.NET
----

* [Updating Visual Studio 2017 RC – .NET Core Tooling improvements](https://blogs.msdn.microsoft.com/dotnet/2016/12/12/updating-visual-studio-2017-rc-net-core-tooling-improvements/) by Joe Morris and David Carmona.
* [.NET Standard (video playlist)](https://www.youtube.com/watch?v=YI4MurjfMn8&list=PLRAdsfhKI4OWx321A_pr-7HhRNk7wOLLY) by Immo Landwerth.
* [Implementing low level trie](https://ayende.com/blog/176065/implementing-low-level-trie-part-i) and [Writing my own synchronization primitive ReaderWriterLock](https://ayende.com/blog/176226/writing-my-own-synchronization-primitive-readerwriterlock) by Ayende Rahien.
* [Orleans 2.0 Tech Preview supporting .NET Core](https://blogs.msdn.microsoft.com/orleans/2016/12/05/orleans-20-tech-preview-net-core/) by Julian Dominguez.
* [How to calculate 17 billion similarities](http://indexoutofrange.com/How-to-calculate-17-billion-similarities/) by Szymon Warda.
* [The best log viewer in the universe](http://stackify.com/best-log-viewer-prefix/) by Stackify.
* [Free eBook: Containerized Docker Application Lifecycle with Microsoft Platform and Tools](https://buildazure.com/2016/12/07/free-ebook-containerized-docker-application-lifecycle-with-microsoft-platform-and-tools/) by Cesar de la Torre.
* [What is this race condition that the OpenMutex documentation is trying to warn me about?](https://blogs.msdn.microsoft.com/oldnewthing/20161208-00/?p=94885) by Raymond Chen.

ASP.NET
-------

* [New Updates to Web Tools in Visual Studio 2017 RC](https://blogs.msdn.microsoft.com/webdev/2016/12/12/new-updates-to-web-tools-in-visual-studio-2017-rc/) by Daniel Roth.
* [Getting started with My Tested ASP.NET Core MVC in less than 15 minutes](http://docs.mytestedasp.net/tutorial/intro.html).
* [In-memory testing using ASP.NET Core](http://josephwoodward.co.uk/2016/12/in-memory-testing-using-asp-net-core) by Joseph Woodward.
* [Getting started with OLAP for ASP.NET MVC](http://our.componentone.com/2016/12/06/understanding-olap-for-asp-net-mvc/) by Prabhakar Mishra.
* [Localization.SqlLocalizer: SQL Localization for ASP.NET Core supporting all EF Core providers.](http://localizationsqllocalizer.readthedocs.io/en/latest/index.html) by Damien Bod.
* [Debug ASP.NET Core on Docker with Visual Studio Code](https://carlos.mendible.com/2016/12/11/debug-asp-net-core-on-docker-with-visual-studio-code/) by Carlos Mendible.
* [Applying the RouteDataRequest CultureProvider globally with middleware as filters](http://andrewlock.net/applying-the-routedatarequest-cultureprovider-globally-with-middleware-as-filters/) by Andrew Lock.
* [Exploring Wyam - a .NET Static Site Content Generator](http://www.hanselman.com/blog/ExploringWyamANETStaticSiteContentGenerator.aspx) by Scott Hanselman.
* [Fat Controller CQRS Diet: Vertical Slices](http://codeopinion.com/fat-controller-cqrs-diet-vertical-slices/) by Derek Comartin.
* [Migration to ASP.NET Core: Considerations and Strategies](http://developer.telerik.com/topics/net/migration-asp-net-core-considerations-strategies/) by Scott Addie.
* [Scaffolding ASP.Net Core MVC](https://www.codeproject.com/Articles/1160127/Scaffolding-ASP-Net-Core-MVC) by Shashangka Shekhar.
* [Fun with the HttpClient pipeline](http://www.thomaslevesque.com/2016/12/08/fun-with-the-httpclient-pipeline/) by Thomas Levesque.
* [Basic Steps to Migrate HTTP Handlers and HTTP Modules to ASP.NET Core Middleware](https://www.codeproject.com/Articles/1159640/Basic-Steps-to-Migrate-HTTP-Handlers-and-HTTP-Modu) by Srinivasa Dinesh Parupalli.
* [Build a REST API for your Mobile Apps with ASP.NET Core](https://stormpath.com/blog/rest-api-mobile-dotnet-core) by Laura Rodriguez.
* [Introducing the ASP.Net Async OutputCache Module](https://blogs.msdn.microsoft.com/webdev/2016/12/05/introducing-the-asp-net-async-outputcache-module/) by lanlanlee2008.
* [Managing Cookie Lifetime with ASP.NET Core OAuth 2.0 providers](http://www.jerriepelser.com/blog/managing-session-lifetime-aspnet-core-oauth-providers/) by Jerrie Pelser.

F#
--

* [Build your own chatbot therapist in F#](http://evelinag.com/eliza/#/) by Evalina Gabasova.
* [A gentle introduction to programming networked services on linux](https://github.com/haf/linux-intro-course) by Henrik Feldt.
* [RdKafka for F# Microservices](https://j-alexander.github.io/entry/2016/12/08/rdkafka-for-fsharp-microservices) by Jonathan Leaver.
* [Asterik Game in F# and WPF](http://markheath.net/post/asterisk-fsharp) by Mark Heath.
* [How F# delighted this newbie while experimenting with distributed systems](https://hussam.github.io/fsadvent16/) by Hussam Abu-Libdeh.

Check out the [F# Advent Calendar](https://sergeytihon.wordpress.com/2016/10/23/f-advent-calendar-in-english-2016/) for loads of great F# blog posts for the month of December.

Check out [F# Weekly](https://sergeytihon.wordpress.com/category/f-weekly/) for more great content from the F# community.

Xamarin
-------

* [Bindable Native Views in XAML – With Commands!?!](https://codemilltech.com/bindable-native-views-in-xaml-with-commands/) by Matthew Soucoup.
* [Xamarin Alpha Preview 4: Cycle 9](https://releases.xamarin.com/alpha-preview-4-cycle-9/) & [Preview 2: Visual Studio for Mac](https://releases.xamarin.com/preview-2-visual-studio-for-mac/) by Adrian Murphy.
* [Join us for the Xamarin Dev Days Live Virtual Event](https://blog.xamarin.com/join-us-for-the-xamarin-dev-day-live-virtual-event/) by James Montemagno.
* [Webinar Recording | Get Started with Xamarin and Microsoft Azure](https://blog.xamarin.com/webinar-recording-get-started-with-xamarin-and-microsoft-azure/) by Courtney Witmer.
* [Optimizing Android Apps for Multi-Window Mode](https://blog.xamarin.com/optimizing-android-apps-for-multi-window-mode/) by James Montemagno.
* [Google Awareness API for Android: Query and React to Signals](https://blog.xamarin.com/google-awareness-api-for-android-query-and-react-to-signals/) by James Montemagno.
* [Vehicle Smart Solves an Everyday Problem with Five-Star Xamarin Apps](https://blog.xamarin.com/vehicle-smart-solves-an-everyday-problem-with-five-star-xamarin-apps/) by Lacey Butler.
* [Xamarin + Universal Windows Platform](https://developer.xamarin.com/guides/cross-platform/windows/universal/) & [Introducing Visual Studio for Mac](https://developer.xamarin.com/guides/cross-platform/visual-studio-mac/) by Xamarin.
* [The Xamarin Show 12: MVVM Light and Xamarin with Laurent Bugnion](https://channel9.msdn.com/Shows/XamarinShow/The-Xamarin-Show-12-MVVM-Light-and-Xamarin-with-Laurent-Bugnion) by James Montemagno.
* [Setting Up Android x86 HAXM Emulators](http://motzcod.es/post/154096351292/setting-up-android-x86-haxm-emulators) by James Montemagno.
* [Xamarin.Android - Where Do These Permissions Come From?](http://www.jon-douglas.com/2016/12/05/xamarin-android-where-do-these-permissions-come-from/) by Jon Douglas.
* [Introduction To Xamarin Workbooks](https://xamarinhelp.com/introduction-xamarin-workbooks/) by Adam Pedley.
* [Best Practices For Animated Progress Indicators](https://www.smashingmagazine.com/2016/12/best-practices-for-animated-progress-indicators/) by Nick Babich.
* [Putting Aid on the Map with Help from Urban Refuge](https://channel9.msdn.com/Blogs/DevRadio/DR1707) by Jerry Nixon.
* [X-Platform Development With Xamarin.Forms & F#](http://trelford.com/blog/post/XamarinForms.aspx) by Phillip Trelford.
* [Xamarin.iOS - How to get the mime type of a file](http://blog.thomaslebrun.net/2016/12/xamarin-ios-how-to-get-the-mime-type-of-a-file/) by Thomas Lebrun.
* [Xamarin.iOS - How to pre-calculate the size of a text, depending of its content](http://blog.thomaslebrun.net/2016/12/xamarin-ios-how-to-pre-calculate-the-size-of-a-text-depending-of-its-content/) by Thomas Lebrun.
* [Xamarin Forms: Customizing the Synfusion Kanban Control is as simple as 1-2-3](https://inquisitorjax.blogspot.com/2016/12/xamarin-forms-customizing-synfusion.html) by Malcolm Jack.
* [Caliburn.Micro 3.0.2 released](http://caliburnmicro.com/announcements/3.0.2) by Caliburn.Micro Team.

Azure
-----

* [Understanding the Azure App Service Editor](https://kencenerelli.wordpress.com/2016/12/10/understanding-the-azure-app-service-editor/) by Ken Cenerelli.

Data
----

* [Integration Testing with Entity Framework Core (video)](https://aspnetmonsters.com/2016/12/monsters-weekly/ep84/) by ASP.NET Monsters.

Games
-----

And this is it for this week!

Contribute to the week in .NET
------------------------------

As always, this weekly post couldn't exist without community contributions, and I'd like to thank all those who sent links and tips. The F# section is provided by [Phillip Carter](https://twitter.com/_cartermp), the gaming section by [Stacey Haffner](https://twitter.com/yecats131), and the Xamarin section by [Dan Rigby](https://twitter.com/DanRigby).

You can participate too. Did you write a great blog post, or just read one? Do you want everyone to know about an amazing new contribution or a useful library? Did you make or play a great game built on .NET?
We'd love to hear from you, and feature your contributions on future posts:

* Send an email to beleroy at Microsoft,
* [comment on this gist](https://gist.github.com/bleroy/28d6b742a1ebadcec8553608429d3a6e)
* Leave us a pointer in the comments section below.
* [Send Stacey (@yecats131) tips on Twitter about .NET games](https://twitter.com/yecats131).

This week's post (and future posts) also contains news I first read on [The ASP.NET Community Standup](https://blogs.msdn.microsoft.com/webdev/tag/communitystandup/), on [Weekly Xamarin](http://weeklyxamarin.com/), on [F# weekly](https://sergeytihon.wordpress.com/category/f-weekly/), and on [Chris Alcock's The Morning Brew](http://themorningbrew.net/).
