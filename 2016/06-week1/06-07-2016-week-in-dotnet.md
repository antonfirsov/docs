The week in .NET - 06/07/2016
============================

To read last week's post, see [The week in .NET – 5/31/2016](https://blogs.msdn.microsoft.com/dotnet/2016/05/31/the-week-in-net-5312016/).

DotNetConf 7-9 June
-------------------

Are you ready to rediscover .NET? Well, dotnetConf is back!

![DotNetConf](../05-week5/DotNetConf.PNG)

Immerse yourself in the world of .NET and join our live stream for 3 days of free online content June 7 - 9 featuring speakers from the .NET Community and Microsoft product teams. Watch and ask questions after each session for a live Q&A. The live stream will be broadcasted on [Channel9](http://channel9.msdn.com/). 

There's never been a better time to be a .NET developer. Learn to develop for web, mobile, desktop, games, services, libraries and more for a variety of platforms and devices all with .NET! We'll have presentations on .NET Core and ASP.NET Core, C#, F#, Roslyn, Visual Studio, Xamarin, and much more. Take a look at our lineup of great [speakers and sessions](https://channel9.msdn.com/Events/dotnetConf/2016). We'll have keynotes from Miguel de Icaza, Scott Hunter, and Scott Hanselman and a lot of great content from our community.

For more information, check out [our website](http://www.dotnetconf.net/) and stay tuned to [#dotnetconf](https://twitter.com/search?q=%23dotNetConf) & [@dotnet](https://twitter.com/dotnet) on Twitter. 

See you on the live stream! 

On.NET
------

Last week on the show, we had Lucian Wischik, Program Manager on the Managed Languages team at Microsoft, and concurrency expert.

<iframe width="560" height="315" src="https://www.youtube.com/embed/5MnfrL7gfEs" frameborder="0" allowfullscreen></iframe>

This week, [we speak with Daniel Egloff](https://www.youtube.com/watch?v=QJ93BMCo_XI) about using the GPU in .NET.

Package of the week: Markdig
----------------------------------

There are quite a few markdown libraries for .NET. [Markdig](https://github.com/lunet-io/markdig) is a fully extensible implementation of the [CommonMark](http://commonmark.org/) standard with excellent performance, both in terms of speed, and GC pressure.

```csharp
var result = Markdown.ToHtml("This is a text with some *emphasis*");
```

Xamarin App of the week: Expensya
---------------------------------

Expensya is a leading expense reporting app in Europe, Africa and Australia. Expensya is implemented in C# and runs on Azure, so when building the mobile app, they naturally decided to use Xamarin, that allows a high rate of code sharing, reuse of the team skills, cross-platform and client-server unit tests that validate the business logic end to end, and native-level performance. Thanks to Xamarin, Expensya mobile apps shipped on the three main platforms, in just a few months.

![Expensya](http://www.expensya.com/Images/Welcome/demo-en.gif)

User group meeting of the week: HoloLens Development in Florida
---------------------------------------------------------------

The [Florida .NET user group](http://www.fladotnet.com/) hosts [a meeting on HoloLens Development with Unity and .NET](http://www.fladotnet.com/Reg.aspx?EventID=810) on Wednesday, June 8 at 6:30PM at Octagon Technology Staffing@AXIS in Ft Lauderdale, FL.

.NET
----

* [Inline IL ASM in C# with Roslyn](http://xoofx.com/blog/2016/05/25/inline-il-asm-in-csharp-with-roslyn/) by Alexandre Mutel.
* [Strings and the CLR - a Special Relationship](http://mattwarren.org/2016/05/31/Strings-and-the-CLR-a-Special-Relationship/) by Matt Warren.
* [Use project.lock.json to troubleshoot dotnet restore problems](https://andrewlock.net/use-project-lock-json-to-troubleshoot-dotnet-restore-problems/) by Andrew Lock.
* [Windows 10 Anniversary Update Preview–Composition and the CompositionBackdropBrush](https://mtaulty.com/2016/06/06/windows-10-anniversary-update-preview-composition-and-the-compositionbackdropbrush/) by Mike Taulty.
* [ASP.NET Core – problems and fixes](https://devblog.dymel.pl/2016/06/06/asp-net-core-problems-fixes/) by Michal Dymel.
* [MSBuild Structured Log: record and visualize your builds](http://www.hanselman.com/blog/MSBuildStructuredLogRecordAndVisualizeYourBuilds.aspx) by Scott Hanselman.
* [Storing C# app settings with JSON](http://piotrgankiewicz.com/2016/06/06/storing-c-app-settings-with-json/) by Piotr Gankiewicz.
* [Async Programming : Unit Testing Asynchronous Code](https://msdn.microsoft.com/en-us/magazine/dn818493.aspx) by Stephen Cleary.

ASP.NET
-------

* [Publishing and Running ASP.NET Core Applications with IIS](http://weblog.west-wind.com/posts/2016/Jun/06/Publishing-and-Running-ASPNET-Core-Applications-with-IIS) by Rick Strahl.
* [Introduction to integration testing with xUnit and TestServer in ASP.NET Core](https://andrewlock.net/introduction-to-integration-testing-with-xunit-and-testserver-in-asp-net-core/) by Andrew Lock.
* [Authorizing your .NET Core MVC6 API requests with OpenIddict and Identity](http://kerryritter.com/authorizing-your-net-core-mvc6-api-requests-with-openiddict-and-identity/) by Kerry Ritter.
* [Cloudscribe.Web.Localization - more flexible localization for ASP.NET Core](https://github.com/joeaudette/cloudscribe.Web.Localization) by Joe Audette.
* [ASP.NET Core RC2 (migration guide)](https://ievangelist.github.io/blog/migrating-to-rc2/) by David Pine.

F#
--

* [Using XAML in F# Xamarin Forms - A Screencast](http://www.wintellect.com/devcenter/jwood/using-xaml-f-xamarin-forms-screencast), by Jonathan Wood.
* [Custom error handling and logging in Suave](https://dusted.codes/custom-error-handling-and-logging-in-suave), by Dustin Moris Gorski.
* [Upcoming F# events - learn Suave, FsLab & more!](http://tomasp.net/blog/2016/fsharp-events/), by Tomas Petricek.
* [Fable: Super Fable Mario (Mario clone using HTML5 canvas)](http://fsprojects.github.io/Fable/samples/mario/index.html).
​
Check out [F# Weekly](https://sergeytihon.wordpress.com/category/f-weekly/) for more great content from the F# community.

Xamarin
-------

* [Join the Xamarin Team for dotNetConf](https://blog.xamarin.com/join-the-xamarin-team-for-dotnetconf/) by Joseph Hill.
* [Xamarin DevOps with VSTS - Setup a Cross Platform Build Agent on OSX](http://www.blogaboutxamarin.com/xamarin-devops-with-vsts-setup-a-cross-platform-build-agent-on-osx/), and [Xamarin DevOps with VSTS - Setup a Cross Platform Build Agent on Windows] (http://www.blogaboutxamarin.com/xamarin-devops-with-vsts-setup-a-cross-platform-build-agent-on-windows/) by Richard Woollcott.
* [ASP.NET Core 1.0 RC2 support in Xamarin Studio](http://lastexitcode.com/blog/2016/06/05/AspNetCoreRC2SupportInXamarinStudio/) by Matt Ward.
* [Watch Kent Boogaart code WorkoutWotch from start to end](https://www.youtube.com/playlist?list=PLwqdWBgwaokVPpdPOOJ-GTGiHcqoG5alU).
* [Xamarin DevOps with VSTS - Getting Started](http://www.thexamarinjournal.com/xamarin-dev-ops-with-vsts-getting-started/) by Richard Woollcott.
* [Xamarin Forms UI Snippets](http://snppts.io/latest).


And this is it for this week!

Contribute to the week in .NET
------------------------------

As always, this weekly post couldn't exist without community contributions, and I'd like to thank all those who sent links and tips.

You can participate too. Did you write a great blog post, or just read one? Do you want everyone to know about an amazing new contribution or a useful library? Did you make or play a great game built on .NET?
We'd love to hear from you, and feature your contributions on future posts:

* Send an email to beleroy at Microsoft,
* [comment on this gist](xx)
* Leave us a pointer in the comments section below.
* [Send Stacey (@yecats131) tips on Twitter about .NET games](https://twitter.com/yecats131).

This week's post (and future posts) also contains news I first read on [The ASP.NET Community Standup](https://blogs.msdn.microsoft.com/webdev/tag/communitystandup/), on [Weekly Xamarin](http://weeklyxamarin.com/), on [F# weekly](https://sergeytihon.wordpress.com/category/f-weekly/), on [ASP.NET Weekly](http://www.aspnetweekly.com/), and on [Chris Alcock's The Morning Brew](http://themorningbrew.net/).
