The week in .NET - Mitch Muenster - Stateless
=============================================

To read last week's post, see [The week in .NET – On .NET on CoreRT and .NET Native – Enums.NET – Ylands – Markdown Monster](https://blogs.msdn.microsoft.com/dotnet/2016/11/08/the-week-in-net-on-net-on-corert-amp-net-native-enums-net-ylands-markdown-monster/).

On .NET
-------

Last week, we hosted the [MVP Summit](https://mvp.microsoft.com/summit), and instead of having a big one-hour show, we did several mini-interviews with MVPs. The first one was published on Monday. [Mitch Muenster spent 25 minutes with us talking about being a developer with autism](https://channel9.msdn.com/Shows/On-NET/Mitch-Muenster-Being-a-dev-with-autism):

<iframe src="https://channel9.msdn.com/Shows/On-NET/Mitch-Muenster-Being-a-dev-with-autism/player" width="960" height="540" allowFullScreen frameBorder="0"></iframe>

This week, we'll publish the other interviews that we recorded during the summit.

Package of the week: Stateless
------------------------------

Almost all applications implement processes that can be represented as workflows or state machines. [Stateless](https://github.com/dotnet-state-machine/stateless) is a library that enables the representation of state machine-based workflows directly in .NET Code.

[Version 3.0 of Stateless just came out, with support for .NET Core](https://nblumhardt.com/2016/11/stateless-30/).

```csharp
var phoneCall = new StateMachine<State, Trigger>(State.OffHook);

phoneCall.Configure(State.OffHook)
    .Permit(Trigger.CallDialed, State.Ringing);

phoneCall.Configure(State.Ringing)
    .Permit(Trigger.HungUp, State.OffHook)
    .Permit(Trigger.CallConnected, State.Connected);

phoneCall.Configure(State.Connected)
    .OnEntry(() => StartCallTimer())
    .OnExit(() => StopCallTimer())
    .Permit(Trigger.LeftMessage, State.OffHook)
    .Permit(Trigger.HungUp, State.OffHook)
    .Permit(Trigger.PlacedOnHold, State.OnHold);

// ...
```

User group meeting of the week: Introduction to TPL Dataflow in Boulder, CO
---------------------------------------------------------------------------

The [Boulder .NET User Group](https://www.meetup.com/Boulder-NET-User-Group/) holds [a meeting on Tuesday, November 15 at 5:45](https://www.meetup.com/Boulder-NET-User-Group/events/235464245/) on TPL Dataflow, a pattern that allows for lock-free multitasking.

.NET
----

* [.NET Core Data Access](https://blogs.msdn.microsoft.com/dotnet/2016/11/09/net-core-data-access/) by Bertrand Le Roy.
* [So you want to write an EF Core provider...](https://blog.oneunicorn.com/2016/11/11/so-you-want-to-write-an-ef-core-provider/) and [Internal code in EF Core 1.1](https://blog.oneunicorn.com/2016/11/09/internal-code-in-ef-core-1-1/) by Arthur Vickers.
* [EF Core Lets Us Finally Define NoTracking DbContexts](http://thedatafarm.com/data-access/ef-core-lets-us-finally-define-notracking-dbcontexts/) by Julie Lerman.
* [Is Entity Framework Core Production Ready?](https://jonhilton.net/2016/11/09/is-entity-framework-core-production-ready/) by Jon hilton.
* [Using dependency injection in a .Net Core console application](https://andrewlock.net/using-dependency-injection-in-a-net-core-console-application/) by Andrew Lock.
* [VLC.MediaElement for UWP, a MediaElement clone leveraging VLC](https://github.com/kakone/VLC.MediaElement) by Stéphane Mitermite.
* [Refit: The automatic type-safe REST library for .NET Core, Xamarin and .NET](https://github.com/paulcbetts/refit) by Paul Betts.
* [Efficient discriminated unions in C#7](http://xoofx.com/blog/2016/10/28/efficient-discriminated-unions-in-csharp/) by Alexandre Mutel.
* [OneOf, easy to use discriminated unions for C#](https://github.com/mcintyre321/OneOf) by Harry McIntyre.
* [Hitchhiking the HoloToolkit-Unity, Leg 2 – Input Scripts (video)](https://mtaulty.com/2016/11/11/hitchiking-the-holotoolkit-unity-leg-2/) and [Windows 10, 1607, UWP and Experimenting with the Kinect for Windows V2 Update](https://mtaulty.com/2016/11/07/windows-10-1607-uwp-and-experimenting-with-the-kinect-for-windows-v2-update/) by Mike Taulty.
* [How bwin is using SQL Server 2016 In-Memory OLTP to achieve unprecedented performance and scale](https://blogs.msdn.microsoft.com/sqlcat/2016/10/26/how-bwin-is-using-sql-server-2016-in-memory-oltp-to-achieve-unprecedented-performance-and-scale/) by Mike Weiner.
* [Fat Controller CQRS Diet: Simple Command](http://codeopinion.com/fat-controller-cqrs-diet-simple-command/) by Derek Comartin.
* [C# Wildcard Variables](http://aspnetmonsters.com/2016/11/2016-11-09-csharp-wildcards/) by Simon Timms.
* [Control the name of your .NET Core output](http://www.donovanbrown.com/post/Control-the-name-of-your-NET-Core-output) by Donovan Brown.
* [Nothing Cheesy in the "Cheese Edition" of the C# Yellow Book](https://channel9.msdn.com/coding4fun/blog/Nothing-Cheesy-in-the-Cheese-Edition-of-the-C-Yellow-Book) by Greg Duncan.
* [Responsive Applications with Asynchronous Programming](http://www.codeproject.com/Articles/1153166/Responsive-Applications-with-Asynchronous-Programm) by Dirk Strauss.
* [ZeroFormatter, an interesting approach to serialization](https://github.com/neuecc/ZeroFormatter) by Yoshifumi Kawai.
* [20 .NET and Visual Studio power tips](http://www.manuelmeyer.net/2016/10/introducing-netvisual-studio-power-tips/) by Manuel Meyer.

ASP.NET
-------

* [How to serve a static (non-MVC) website and a web API at the same time in ASP.NET Core (video)](http://makingoutwith.net/2016/how-to-serve-a-static-site-plus-a-web-api-in-aspnetcore/) by Joe Petrakovich.
* [Using MongoDB with Web API and ASP.NET Core](http://www.dotnetcurry.com/aspnet-mvc/1267/using-mongodb-nosql-database-with-aspnet-webapi-core) by Mahesh Sabnis.
* [Config transformations in ASP.NET Core](http://blog.elmah.io/config-transformations-in-aspnetcore/) by Thomas Ardal.
* [The Monsters Weekly - Episode 77 -  Internationalization Part 2 - Request Localization](http://aspnetmonsters.com/2016/11/monsters-weekly/ep77/) by the ASP.NET Monsters.

F#
--

* [Writing a search engine with Azure and F# in a weekend](https://skillsmatter.com/skillscasts/8901-f-sharpunctional-londoners-meetup#video) by Anthony Brown.
* [Introducing the F# Software Foundation Programs]https://www.infoq.com/news/2016/11/fsharp-foundation-mentorship) by Pierre-Luc Maheu & Reed Copsey, Jr.
* [F# Beginner Function Declaration Gotcha](http://markheath.net/post/fsharp-beginner-function-declaration-gotcha) by Mark Heath.
* [F# Error Handling Compared](https://medium.com/@dogwith1eye/fsharp-error-handling-compared-bef0516a449#.bjweuq5a1) by Matthew Doig.
* [Let's Play with Azure Functions](http://lukemerrett.com/lets-play-with-azure-functions/) by Luke Merrett.
* [Code as Data: Structuring business rules in F#](https://medium.com/cleartax-engineering/code-as-data-structuring-business-rules-in-f-34cf05f083a2#.rz4l0j21j) by Ankit Solanki.

Check out [F# Weekly](https://sergeytihon.wordpress.com/category/f-weekly/) for more great content from the F# community.

Xamarin
-------

* [Xamarin Alpha Preview: Cycle 9](https://releases.xamarin.com/alpha-preview-cycle-9/) & [Preview: Xamarin Inspector 0.99.1.0](https://releases.xamarin.com/preview-xamarin-inspector-0-99-1-0/) by Adrian Murphy.
* [Join us for Microsoft Connect(); Now with Virtual Training from Xamarin University](https://blog.xamarin.com/join-us-for-microsoft-connect-now-with-virtual-training-from-xamarin-university/) by Julia Black.
* [Webinar Recording | Building Better Apps with Microsoft Cognitive Services and Xamarin](https://blog.xamarin.com/webinar-recording-building-better-apps-with-microsoft-cognitive-services-and-xamarin/) by Courtney Witmer.
* [Microsoft Research Ships Intelligent Apps with the Power of C# and AI](https://blog.xamarin.com/microsoft-research-ships-intelligent-apps-with-the-power-of-c-and-ai/) by Lacey Butler.
* [Getting Started with the iOS 10 Notification Framework](https://blog.xamarin.com/getting-started-with-the-ios-10-notification-framework/) by Mike James.
* [The Xamarin Show 9: Azure Search with Liam Cavanagh](https://channel9.msdn.com/Shows/XamarinShow/Azure-Search-with-Liam-Cavanagh) by James Montemagno.
* [Sez Who? Support for iOS 10 Speech Recognition in Xamarin](https://visualstudiomagazine.com/articles/2016/10/01/xamarin-support-ios-10-speech.aspx) by Wallace McClure.
* [P/Invoking Native Calls](http://www.jon-douglas.com/2016/11/08/pinvoking-native-calls/) & [Porting Android Libraries to Xamarin.Android](http://www.jon-douglas.com/2016/10/28/porting-android-libraries-to-xamarin-android/) by Jon Douglas.
* [A Quick Guide For Designing Better Buttons](https://www.smashingmagazine.com/2016/11/a-quick-guide-for-designing-better-buttons/) by Nick Babich.
* [What Everyone Should Know About The Process Behind App Design](https://www.smashingmagazine.com/2016/11/what-everyone-should-know-about-the-process-behind-app-design/) by Michael Flarup.
* [Face detection on iOS](http://blog.ostebaronen.dk/2016/11/face-detection-on-ios.html) by Tomasz Cielecki.
* [Banish Compiler Directives From Shared Projects!](https://codemilltech.com/banish-compiler-directives-from-shared-projects/) & [Stopping the “ThisApp May Slow Down Your iPhone” Message](https://codemilltech.com/code-mill-minute-stopping-the-thisapp-may-slow-down-your-iphone-message/) by Matthew Soucoup.
* [Native Views in XAML from a PCL](https://xamarinhelp.com/native-views-xaml-pcl/), [The Advanced Navigation of Exrin in Xamarin Forms](https://xamarinhelp.com/advanced-navigation-exrin-xamarin-forms/), & [Xamarin Forms Visual Previewer](https://xamarinhelp.com/xamarin-forms-previewer/) by Adam Pedley.
* [Platform Specifics – Xamarin.Forms 2.3.3 Look Ahead](https://codemilltech.com/platform-specifics-xamarin-forms-2-3-3-look-ahead/) by Matthew Soucoup.

Azure
-----

* [Fluent API Libraries for Azure .NET SDK](https://buildazure.com/2016/11/09/fluent-api-libraries-for-azure-net-sdk/) by Build Azure.
* [Automating deployment of ASP.NET Core to Azure App Service from Linux](http://www.codeproject.com/Articles/1153874/Automating-deployment-of-ASP-NET-Core-to-Azure-App) by Afzaal Ahmad Zeeshan.
* [Serverless Computing and Workflows with Azure Functions and Microsoft Flow](http://dontcodetired.com/blog/post/Serverless-Computing-and-Workflows-with-Azure-Functions-and-Microsoft-Flow) by Jason roberts

And this is it for this week!

Contribute to the week in .NET
------------------------------

As always, this weekly post couldn't exist without community contributions, and I'd like to thank all those who sent links and tips. The F# section is provided by [Phillip Carter](https://twitter.com/_cartermp), the gaming section by [Stacey Haffner](https://twitter.com/yecats131), and the Xamarin section by [Dan Rigby](https://twitter.com/DanRigby).

You can participate too. Did you write a great blog post, or just read one? Do you want everyone to know about an amazing new contribution or a useful library? Did you make or play a great game built on .NET?
We'd love to hear from you, and feature your contributions on future posts:

* Send an email to beleroy at Microsoft,
* [comment on this gist](https://gist.github.com/bleroy/5a7beca22c38d9de215a88c5c1c43fd3)
* Leave us a pointer in the comments section below.
* [Send Stacey (@yecats131) tips on Twitter about .NET games](https://twitter.com/yecats131).

This week's post (and future posts) also contains news I first read on [The ASP.NET Community Standup](https://blogs.msdn.microsoft.com/webdev/tag/communitystandup/), on [Weekly Xamarin](http://weeklyxamarin.com/), on [F# weekly](https://sergeytihon.wordpress.com/category/f-weekly/), and on [Chris Alcock's The Morning Brew](http://themorningbrew.net/).
