The week in .NET - On .NET with Sidarth Gupta on Tizen, Happy birthday .NET with Bertrand Le Roy, JSON.NET 10
=============================================================================================================

Previous posts:

* [Happy birthday .NET with Mads Torgersen, Coypu](https://blogs.msdn.microsoft.com/dotnet/2017/03/21/the-week-in-net-happy-birthday-net-with-mads-torgersen-coypu/)
* [On .NET with Scott Hunter, On .NET with Matt Watson, MessagePack](https://blogs.msdn.microsoft.com/dotnet/2017/03/14/the-week-in-net-on-net-with-scott-hunter-on-net-with-matt-watson-messagepack/)
* [Visual Studio 2017, .NET Core SDK, F# 4.1, On .NET with Phillip Carter, Happy Birthday from John Shewchuk, FNA, Pyre](https://blogs.msdn.microsoft.com/dotnet/2017/03/08/the-week-in-net-visual-studio-2017-net-core-sdk-f-4-1-on-net-with-phillip-carter-happy-birthday-from-john-shewchuk-pyre/)

On .NET
-------

Last week, we spoke with Sidarth Gupta from Samsung about [Tizen](https://www.tizen.org/). Tizen is Samsung's open source OS that runs on TVs, watches, phones, and other devices. The development platform for Tizen is built on .NET Core and Xamarin Forms.

<iframe src="https://channel9.msdn.com/Shows/On-NET/Sidarth-Gupta-Tizen/player" width="960" height="540" allowFullScreen frameBorder="0"></iframe>

This week, we'll talk about [SonarLint](http://www.sonarlint.org/visualstudio/index.html) with Tamás Vajk. We'll take questions on Gitter, on [the dotnet/home channel](https://gitter.im/dotnet/home) and on Twitter. Please use the `#onnet` tag. It's OK to start sending us questions in advance if you can't do it live during the show.

Happy birthday .NET!
--------------------

Bertrand Le Roy started using .NET with the first public betas, then proceeded to write a CMS with it. That's when he moved to the US and joined the ASP.NET team that was working on ASP.NET 2.0 at the time. He's worked on ASP.NET Ajax (including UpdatePanel, yes), co-founded the Orchard CMS project, and is now working as a Program Manager on .NET Core. He sometimes speaks of himself in the third person.

<iframe src="https://channel9.msdn.com/Blogs/funkyonex/Happy-Birthday-NET-with-Bertrand-LeRoy/player" width="960" height="540" allowFullScreen frameBorder="0"></iframe>

Package of the week: JSON.NET 10
--------------------------------

[I've featured JSON.NET before](https://blogs.msdn.microsoft.com/dotnet/2016/01/12/the-week-in-net-1122016/), and [it's the #1 package on NuGet](https://www.nuget.org/packages) with more than 50 million downloads. [Version 10 is out](http://james.newtonking.com/archive/2017/03/21/json-net-10-0-release-1-async-performance-documentation-and-more), however, with new features that make it a particularly exciting release. Specifically, the library now has full `async` support.

```csharp
JArray largeJson;

// read asynchronously from a file
using (FileStream asyncFileStream = new FileStream(@"large.json", FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true))
{
    largeJson = await JArray.LoadAsync(new JsonTextReader(new StreamReader(asyncFileStream)));
}

JToken user = largeJson.SelectToken("$[?(@.name == 'Woodard Caldwell')]");
user["isActive"] = false;

// write asynchronously to a file
using (FileStream asyncFileStream = new FileStream(@"large.json", FileMode.Open, FileAccess.Write, FileShare.Write, 4096, true))
{
    await largeJson.WriteToAsync(new JsonTextWriter(new StreamWriter(asyncFileStream)));
}
```

Download [JSON.NET 10 on NuGet.org](https://www.nuget.org/packages/Newtonsoft.Json/).

Meetups of the week: game development with Unity for Windows 10 in NYC
----------------------------------------------------------------------

[Tonight at 6PM at the Microsoft Reactor in NYC](https://www.meetup.com/MMADNYC/events/238648860/), [the Microsoft makers and app developers group](https://www.meetup.com/MMADNYC/) holds a meeting where you'll learn about game development on Windows 10 using [Unity](https://unity3d.com/).

.NET
----

* [A Hitchhikers Guide to the CoreCLR Source Code](http://mattwarren.org/2017/03/23/Hitchhikers-Guide-to-the-CoreCLR-Source-Code/) by Matt Warren.
* [Command Line: Using dotnet watch test for continuous testing with .NET Core 1.0 and XUnit.net](https://www.hanselman.com/blog/CommandLineUsingDotnetWatchTestForContinuousTestingWithNETCore10AndXUnitnet.aspx) by Scott Hanselman.
* [Visual Studio 2017 can automatically recommend NuGet packages for unknown types](https://www.hanselman.com/blog/VisualStudio2017CanAutomaticallyRecommendNuGetPackagesForUnknownTypes.aspx) by Scott hanselman.
* [Fast Dictionary and struct generic arguments](https://ayende.com/blog/177377/fast-dictionary-and-struct-generic-arguments) by Ayende Rahien.
* [ClrMD Part 2 – From ClrRuntime to ClrHeap or how to traverse the managed heap](http://labs.criteo.com/2017/03/clrmd-part-2-clrruntime-clrheap-traverse-managed-heap/) by CriteoLabs.
* [NuGet Versioning Hell](http://isolineltd.com/blog/2017/03/23/NuGet-Versioning-Hell) by Ivan Gavryliuk.
* [Dynamically generating classes in runtime](https://nikolalukovic.com/programming/NET-Dynamically-generating-classes-in-runtime.html) by Nikola Lukovic.
* [Introductie van .NET Core](http://www.dotnetflix.com/player/46), [.NET Standard](http://www.dotnetflix.com/player/47), and [Developer experience - deel 1 van 2](http://www.dotnetflix.com/player/48) (Dutch) by Sander Molenkamp and Edwin van Wijk.

ASP.NET
-------

* [Preventing mass assignment or over posting in ASP.NET Core](https://andrewlock.net/preventing-mass-assignment-or-over-posting-in-asp-net-core/) by Andrew Lock.
* [Step by step: Running ASP.NET Core on Raspberry Pi](https://carlos.mendible.com/2017/03/21/step-by-step-running-aspnet-core-on-raspberry-pi/), and [Raspberry Pi: Run ASP.NET Core on Startup](https://carlos.mendible.com/2017/03/26/raspberry-pi-run-aspnet-core-on-startup/) by Carlos Mendible.
* [MVC 5 encrypt parameters](http://msprogrammer.serviciipeweb.ro/2017/03/20/mvc-5-encrypt-parameters/) by Andrei Ignat.

C#
--

* [Why .NET Core Made C# Your Next Programming Language to Learn](https://stackify.com/net-core-csharp-next-programming-language/) by Matt Watson.
* [Exploring C# 7](https://docs.microsoft.com/en-us/dotnet/articles/project-json) by David Pine.
* [Deconstructors for non-tuple types in C# 7.0](https://andrewlock.net/deconstructors-for-non-tuple-types-in-c-7-0/) by Andrew Lock.

F#
--

* [Exploring StackOverflow Data](https://www.youtube.com/watch?v=VU1ObHfDNPQ), by Evalina Gabasova
* [Visualizing Olympic Medals with F# and Fable](https://www.youtube.com/watch?v=C4xzEudljWE), by Tomas Petricek
* [Building a MUD with F# and Akka.NET – Part One](https://www.seventeencups.net/building-a-mud-with-f-sharp-and-akka-net-part-one/), by Joe Clay
* [Why OO Matters (in F#)](https://eiriktsarpalis.wordpress.com/2017/03/20/why-oo-matters-in-f/), by Eirik Tsarpalis
* [Answering “What’s for lunch?” using Azure Functions, F# and Slack](https://martinand.net/2017/03/20/what-is-for-lunch/), by  Martin Andersen
* [Examining the F# Programming Language](http://insights.dice.com/2017/03/20/examining-f-programming-language/?utm_campaign=shareaholic&utm_medium=twitter&utm_source=socialnetwork), by David Bolton
* [Visual F# Attributions](https://github.com/Microsoft/visualfsharp/blob/master/attributions.md)

New F# Language Suggestions:

- [New type: constrained type](https://github.com/fsharp/fslang-suggestions/issues/553)
- [Flow based null check analysis for `[<AllowNullLiteralAttribute>]` types and alike](https://github.com/fsharp/fslang-suggestions/issues/552)

Check out [F# Weekly](https://sergeytihon.wordpress.com/category/f-weekly/) for more great content from the F# community.

Xamarin
-------

* [Xamarin Beta Release: 15.1 Beta Preview 1](https://releases.xamarin.com/beta-release-15-1-beta-preview-1/) by Bri Brothers.
* [Xamarin Stable Release: Xamarin Workbooks & Inspector 1.2.0](https://releases.xamarin.com/stable-release-xamarin-workbooks-inspector/) by Bri Brothers.
* [Xamarin Technical Bulletin: Updating Xamarin Visual Studio 2017 & Side-by-Side](https://releases.xamarin.com/technical-bulletin-updating-xamarin-visual-studio-2017-side-by-side/) by Dominic Nahous.
* [Introducing the Kimono Designer for SkiaSharp](https://blog.xamarin.com/introducing-the-kimono-designer-for-skiasharp/) by Kevin Mullins.
* [Xamarin University Webinar Recording | Building Your First Android App with Xamarin for Visual Studio](https://blog.xamarin.com/xamarin-university-webinar-recording-building-your-first-android-app-with-xamarin-for-visual-studio/) by Courtney Witmer.
* [Play Audio and Video with the MediaManager Plugin for Xamarin](https://blog.xamarin.com/play-audio-and-video-with-the-mediamanager-plugin-for-xamarin/) by Martijn van Dijk.
* [Organize a Xamarin Dev Days!](https://blog.xamarin.com/organize-your-xamarin-dev-days/) by Jayme Singleton.
* [Catch Up on Visual Studio 2017 and Visual Studio for Mac with Channel 9](https://blog.xamarin.com/catch-up-on-vs2017-visual-studio-for-mac-channel-9/) by James Montemagno.
* [Introduction to Game Development with MonoGame](https://developer.xamarin.com/guides/cross-platform/game_development/monogame/introduction/) by Xamarin.
* [New HockeySDK releases for Xamarin and Unity](https://www.hockeyapp.net/blog/2017/03/22/HockeySDK-Xamarin-4-1-2.html) by HockeyApp.
* [Behind the Scenes: How Chefs for Seniors uses Xamarin, HockeyApp & Azure App Service to Power its Daily Operations](https://channel9.msdn.com/Blogs/DevRadio/DR1719) by DevRadio.
* [Episode 19: MonoGame - Write Once, Play Everywhere with Dean Ellis](https://channel9.msdn.com/Shows/XamarinShow/Episode-19-MonoGame-Write-Once-Play-Everywhere-with-Dean-Ellis) by The Xamarin Show.
* [Cleaning Up Space on Your Xamarin Development Machine](http://motzcod.es/post/158519702967/cleanup-up-space-xamarin-dev-machine) by James Montemagno.
* [Xamarin.Forms Layout Challenges – Great Places](http://www.kymphillpotts.com/xamarin-forms-layout-challenges-great-places/) by Kym Phillpotts.
* [Connecting To A Remote Database in Xamarin Forms](https://xamarinhelp.com/connecting-remote-database-xamarin-forms/) by Adam Pedley.
* [UISleuth – Visually Inspect Your Xamarin Forms Application](https://xamarinhelp.com/uisleuth-visually-inspect-xamarin-forms-application/) by Adam Pedley.
* [Xamarin Forms Binding](https://xamarinhelp.com/xamarin-forms-binding/) by Adam Pedley.
* [Xamarin.Tips – Xamarin.Forms Android Custom TableView Section Titles](https://alexdunn.org/2017/03/21/xamarin-tips-xamarin-forms-android-custom-tableview-section-titles/) by Alex Dunn.
* [Xamarin.Tips – Xamarin.Forms iOS Custom TableView Section Titles](https://alexdunn.org/2017/03/21/xamarin-tips-xamarin-forms-ios-custom-tableview-section-titles/) by Alex Dunn.
* [Xamarin.Tips – Android Shadows on Transparent Views](https://alexdunn.org/2017/03/22/xamarin-tips-android-shadows-on-transparent-views/) by Alex Dunn.
* [Xamarin.Tips – Changing a TableView’s Separator Color](https://alexdunn.org/2017/03/23/xamarin-tips-changing-a-tableviews-separator-color/) by Alex Dunn.
* [Xamarin.Forms: Grouping data with Tabbed page](https://almirvuk.blogspot.com/2017/03/xamarinforms-grouping-data-with-tabbed.html) by Almir Vuk.
* [Xamarin.Forms -- Caching for the ListView](https://visualstudiomagazine.com/articles/2017/03/01/xamarinforms-ios-android-mobile-visual-studio.aspx) by Wallace McClure.

UWP
----

* [Setting a custom User-Agent in the UWP WebView control](https://www.pedrolamas.com/2017/03/21/setting-a-custom-user-agent-in-the-uwp-webview-control/) by Pedro Lamas.

Azure
-----


Data
----

* [Quick Start EF Core Videos on Channel 9](http://thedatafarm.com/uncategorized/quick-start-ef-core-videos-on-channel-9/), [EF Core Quick Starts: ASP.NET Core in Visual Studio 2017 (video)](https://channel9.msdn.com/Blogs/MVP-VisualStudio-Dev/EF-Core-Quick-Starts-ASPNET-Core-in-Visual-Studio-2017) and [EF Core Quick Starts: Full .NET in Visual Studio 2015 (video)](https://channel9.msdn.com/Blogs/MVP-VisualStudio-Dev/EF-Core-Quick-Starts-Full-NET-in-Visual-Studio-2015) by Julie Lerman.

Game development
----------------


And this is it for this week!

Contribute to the week in .NET
------------------------------

As always, this weekly post couldn't exist without community contributions, and I'd like to thank all those who sent links and tips. The F# section is provided by [Phillip Carter](https://twitter.com/_cartermp), the gaming section by [Stacey Haffner](https://twitter.com/yecats131), and the Xamarin section by [Dan Rigby](https://twitter.com/DanRigby), and the UWP section by [Michael Crump](http://twitter.com/mbcrump).

You can participate too. Did you write a great blog post, or just read one? Do you want everyone to know about an amazing new contribution or a useful library? Did you make or play a great game built on .NET?
We'd love to hear from you, and feature your contributions on future posts:

* Send an email to beleroy at Microsoft,
* [comment on this gist](https://gist.github.com/bleroy/c44c40fe36ffc3ad1b512ac550e1bb88)
* Leave us a pointer in the comments section below.
* [Send Stacey (@yecats131) tips on Twitter about .NET games](https://twitter.com/yecats131).

This week's post (and future posts) also contains news I first read on [The ASP.NET Community Standup](https://blogs.msdn.microsoft.com/webdev/tag/communitystandup/), on [Weekly Xamarin](http://weeklyxamarin.com/), on [F# weekly](https://sergeytihon.wordpress.com/category/f-weekly/), and on [Chris Alcock's The Morning Brew](http://themorningbrew.net/).
