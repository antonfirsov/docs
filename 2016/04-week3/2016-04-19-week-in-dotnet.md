The week in .NET - 4/19/2016
============================

To read last week's post, see [The week in .NET – 4/12/2016](https://blogs.msdn.microsoft.com/dotnet/2016/04/12/the-week-in-net-4122016/).

On.NET
------

Last week on the show, [we looked at what PlayFab is doing to help game developers take advantage of the cloud](https://www.youtube.com/watch?v=hDDd_Pjtbx8). This week, [we'll be speaking](https://www.youtube.com/watch?v=7E7JyvBIGKs) with [Telerik](http://www.telerik.com/).

Package of the week: NUglify
----------------------------

[NUglify](https://github.com/xoofx/NUglify) is a library that can minify JavaScript and CSS. It has no dependencies, and can run on .NET Core.

```csharp
var miniJs = Uglify.Js("var x = 5; var y = 6;"); // var x=5,y=6
var miniCss = Uglify.Css("div { color: #FFFFFF; }"); // div{color:#fff}
```

Xamarin app of the week: Cinemark
---------------------------------

Cinemark is a leading theater chain in North America, with $2.7 billion in revenue and 5,600 screens. Disappointed by the hybrid app development frameworks Appcelerator and Sencha Touch, Cinemark chose a native path with Xamarin. The result is a ticketing and loyalty app worthy of one the nation's largest movie theater companies.

![Cinemark](cinemark.png)

Component of the week: SharpDevelop's WPF Designer
--------------------------------------------------

Since October 2015, the [WPF designer](https://github.com/icsharpcode/WpfDesigner/) from [SharpDevelop](http://www.icsharpcode.net/OpenSource/SD/Default.aspx) has been [a standalone component](https://www.nuget.org/packages/ICSharpCode.WpfDesigner/) that can be re-used in any application that needs to integrate a XAML designer. [That component is now free of dependencies](http://community.sharpdevelop.net/blogs/jochenkuehner/archive/2016/04/12/wpf-designer-news.aspx).

<img slt="The XAML editor" src="https://github.com/icsharpcode/WpfDesigner/raw/master/screenshot.png?raw=true" style="widht:500px;max-width:100%"/>

Game of the week: Dungeon of the Endless
----------------------------------------

[Dungeon of the Endless](http://madewith.unity.com/games/dungeon-endless) is a [roguelike](https://en.wikipedia.org/wiki/Roguelike) dungeon defense style game created by [Amplitude Studios](http://madewith.unity.com/profiles/amplitude-studios) using [Unity](http://unity3d.com/) and [C#](https://channel9.msdn.com/Series/C-Sharp-Fundamentals-Development-for-Absolute-Beginners). The gameplay is unique and blends together tower defense, squad control, RPG and roguelike elements beautifully. You'll take on the role of controlling a team of heroes who must explore a way out of their ship, which has crash landed on the planet of Auriga. Behind every door in Dungeon of the Endless lies a chance to be swarmed by condemned criminals who have no desire to work for their place back in society. Their sole purpose is to kill your heroes and/or the crystal that powers your ship. Should they succeed (and they often will), you'll be left cleaning up the pieces by creating a new team and traversing a different procedurally generated dungeon.

Dungeon of the Endless is available on Steam and iTunes. More information can be found on their [Made With Unity](http://madewith.unity.com/games/dungeon-endless) page.

<img src="https://cloud.githubusercontent.com/assets/4108756/14642984/8adbaffa-0601-11e6-9d49-723d5b9a3fa7.png" alt="Dungeon of the Endless" style="width:500px; max-width: 100%"/>

User group meeting of the week: Behaviour Driven Development with SpecFlow / What'­s new in C# with the Ottawa IT Community
----------------------------------------------------------------------------------------------------------------

[The Ottawa IT Community](http://www.meetup.com/ottawaitcommunity/) will host a [double meetup on Tuesday, April 19 on behavior-driven development with SpecFlow, and on the new C# 6 and 7 features](http://www.meetup.com/ottawaitcommunity/events/228292714/).

.NET
----

* [What’s new for the .NET Native Compiler and Runtime in Visual Studio 2015 Update 2](https://blogs.msdn.microsoft.com/dotnet/2016/04/18/whats-new-for-the-net-native-compiler-and-runtime-in-visual-studio-2015-update-2/) by Stacey Haffner and Matthew Whilden.
* [Introducing the Microsoft .NET Framework Repair Tool Version 1.3](https://blogs.msdn.microsoft.com/dotnet/2016/04/19/introducing-the-microsoft-net-framework-repair-tool-version-1-3/) by Rakesh Ranjan Singh.
* [How to host your own NuGet server and package feed](http://www.hanselman.com/blog/HowToHostYourOwnNuGetServerAndPackageFeed.aspx) by Scott Hanselman.
* [Moq on .NET Core](http://dotnetliberty.com/index.php/2016/02/22/moq-on-net-core/) by Armen Shimoon.
* [Visual Studio Code 1.0 has been released!](http://code.visualstudio.com/blogs/2016/04/14/vscode-1.0)

ASP.NET
-------

* [An update on ASP.NET Core 1.0 RC2](http://www.hanselman.com/blog/AnUpdateOnASPNETCore10RC2.aspx) by Scott Hanselman.
* [Notes from the ASP.NET Community Standup – April 12, 2016](https://blogs.msdn.microsoft.com/webdev/2016/04/14/notes-from-the-asp-net-community-standup-april-12-2016/) by Jeffrey T. Fritz.
* [Shawn Wildermuth has ported](http://wildermuth.com/2016/04/14/Welcome-to-the-New-Wildermuth-com) his [blog to .NET Core as an open source project](https://github.com/shawnwildermuth/wilderblog), and has built [RSS](https://github.com/shawnwildermuth/RssSyndication) and [XML RPC](https://github.com/shawnwildermuth/MetaWeblog) libraries along the way, that he has also open-sourced.
* [Enhancing Claims with Owin Middleware & Claims Transformation](http://www.cognim.co.uk/transforming-claims-claimsprincipal/) by Darren Hall.
* [How to perform partial resource updates with JSON Patch and ASP.NET Core](http://benfoster.io/blog/aspnet-core-json-patch-partial-api-updates) by Ben Foster.
* [ASP.NET Core custom service based on request](http://dotnetliberty.com/index.php/2016/04/11/asp-net-core-custom-service-based-on-request/) by Armen Shimoon.
* [Entity Framework Core: The Future of EF for ASP.NET Core (video)](https://channel9.msdn.com/Blogs/DevRadio/DR1644) by Chris Caldwell.
* [ASP.NET Core on Nano Server Preview](http://blog.guardrex.com/2016/04/aspnet-core-on-nano-server-preview.html) by Luke Latham.
* [Hooking up ASP.NET Core 1.0 RC1 web api with Auth0 bearer tokens](http://blog.novanet.no/hooking-up-asp-net-core-1-rc1-web-api-with-auth0-bearer-tokens/) by Hans Arne Vartdal.
* [Using Cache in ASP.NET Core 1.0 RC1](http://wildermuth.com/2016/04/14/Using-Cache-in-ASP-NET-Core-1-0-RC1) by Shawn Wildermuth.
* [Exploring Prefix: A Free ASP.NET Profiling Tool](http://www.mikesdotnetting.com/article/296/exploring-prefix-a-free-asp-net-profiling-tool) by Mike Brind.

F#
--

* [F# eXchange 2016](https://skillsmatter.com/conferences/7145-f-exchange-2016#skillscasts): More than a dozen recorded sessions from this year's conference.
* [Happy F# Day! Growing and Getting Better Each Year](http://fsharpforfunandprofit.com/posts/happy-fsharp-day-2/), by Scott Wlaschin.
* [.NET Core support has been added to the Ionide extension](https://twitter.com/k_cieslak/status/719923250583769088) for Atom and Visual Studio Code.
* [Async as Surrogate IO](http://blog.ploeh.dk/2016/04/11/async-as-surrogate-io/), by Mark Seemann.
* [Optionals](http://sidburn.github.io/blog/2016/04/11/optionals), by David Raab.
* [Functional Error Handling in F# by Example](http://blog.leifbattermann.de/2016/04/09/functional-error-handling-in-fsharp-by-example/), by Leif Battermann.

Check out [F# Weekly](https://sergeytihon.wordpress.com/category/f-weekly/) for more great content from the F# community.

And this is it for this week!

Contribute to the week in .NET
------------------------------

As always, this weekly post couldn't exist without community contributions,
and I'd like to thank all those who sent links and tips.
You can participate too. Did you write a great blog post, or just read one?
Do you want everyone to know about an amazing new contribution or a useful library?
Did you make or play a great game built on .NET?
We'd love to hear from you, and feature your contributions on future posts:

* Send an email to beleroy at Microsoft,
* [comment on this gist](xx)
* Leave us a pointer in the comments section below.
* [Send Stacey (@yecats131) tips on Twitter about .NET games](https://twitter.com/yecats131).

This week's post (and future posts) also contains news I first read on
[The ASP.NET Community Standup](https://blogs.msdn.microsoft.com/webdev/tag/communitystandup/),
on [F# weekly](https://sergeytihon.wordpress.com/category/f-weekly/),
on [ASP.NET Weekly](http://www.aspnetweekly.com/),
and on [Chris Alcock's The Morning Brew](http://themorningbrew.net/).
