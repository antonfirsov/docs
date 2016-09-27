The week in .NET - On .NET on Orchard 2 - Mocking on Core - 
============================

To read last week's post, see [The week in .NET: On .NET with Steeltoe – C# Functional Extensions – Firewatch](https://blogs.msdn.microsoft.com/dotnet/2016/09/20/the-week-in-net-on-net-with-steeltoe-c-functional-extensions-firewatch/).

On .NET
-------

Last week, [Sébastien Ros was on the show](xx) to talk about [Orchard 2](https://github.com/orchardcms/orchard2):

<iframe src="https://channel9.msdn.com/Shows/On-NET/Sbastien-Ros-Orchard-2/player" width="560" height="315" allowFullScreen frameBorder="0"></iframe>

This week, we'll speak with [JB Evain](http://evain.net/) about his work on the [Visual Studio 2015 Tools](https://visualstudiogallery.msdn.microsoft.com/8d26236e-4a64-4d64-8486-7df95156aba9) for [Unity](https://unity3d.com/),  and maybe also [Cecil](https://github.com/jbevain/cecil). The show begins at 12PM Pacific Time (note that's 2 hours later than usual) [on Channel 9](https://channel9.msdn.com/Shows/On-NET). We'll take questions on Gitter, on [the dotnet/home channel](https://gitter.im/dotnet/home). Please use the `#onnet` tag. It's OK to start sending us questions in advance if you can't do it live during the show.

Mocking on .NET Core
--------------------

Three major .NET mocking frameworks now have official pre-releases with .NET Standard support:

* [FakeItEasy](https://ci.appveyor.com/nuget/FakeItEasy) (nuget feed from AppVeyor CI builds)
* [Moq](https://www.nuget.org/packages/Moq/4.6.38-alpha). Note that there is another "moq.netcore" package from the ASP.NET team's MyGet feed. It is an obsolete private fork meant to unblock testing in the early days before Moq had releases that support .NET Standard. Consumers of the "moq.netcore" package should switch to use the latest official Moq package.
* [NSubstitute](https://www.nuget.org/packages/NSubstitute/2.0.0-rc)

Xamarin App of the Week: SpeechCentral
--------------------------------------

Speech Central is an iOS app that lets you enjoy the Internet with the screen off by using the headphones or Bluetooth hands-free for reading aloud and issuing commands. Keep up with the news while you're commuting or jogging.

The application is written with Xamarin.iOS.

Package of the week: xx
----------------------------------



```csharp
```

Xamarin App of the week: xx
-----------------------------------


Game of the week: xx
-----------------------------------


User group meeting of the week: xx
------------------------------------------------



.NET
----

* Immo's post
* [Get started with VS Code using C# and .NET Core](https://channel9.msdn.com/Blogs/dotnet/Get-started-with-VS-Code-using-CSharp-and-NET-Core) by Kendra Havens.
* [Deal with Swallowed Exceptions Magically with IL Weaving](https://buildplease.com/pages/ilweaving/) by Nick Chamberlain.

ASP.NET
-------

* [xx](xx) by xx.

F#
--

* [F# and ASP.NET Core (video)](https://www.youtube.com/watch?v=zYi4ev6ll0Y), by Enrico Sada via Community for F#.
* [F# in the Real World (video)](https://vimeo.com/183301783), by Yan Cui
* [Xando: Down the rabbit hole of CQRS and Event Sourcing](http://alxandr.me/2016/09/19/xando-pt-1), by alxandr
* [Absolute layout and relative layout Xamarin Forms](https://kimsereyblog.blogspot.com.by/2016/09/absolute-layout-and-relative-layout.html), by Kimserey Lam
* [Using F# and Canopy for UI Testing](http://www.jeremybellows.com/blog/Using-Fsharp-and-Canopy-for-UI-Testing), by Jeremy Bellows
* [Learning F#, ASP.NET Core, and Polymer](https://github.com/joeaudette/playground/blob/master/spa-stack/README.md) by Joe Audette.

Check out [F# Weekly](https://sergeytihon.wordpress.com/category/f-weekly/) for more great content from the F# community.

Xamarin
-------

* [Scaling from Side Project to 200,000+ Downloads with Xamarin and Microsoft Azure](https://blog.xamarin.com/scaling-from-side-project-to-200000-downloads-with-xamarin-and-microsoft-azure/) by Courtney Witmer.
* [Start Building Azure-Connected Apps with the Xamarin Shopping Demo App](https://blog.xamarin.com/start-building-azure-connected-apps-with-the-xamarin-shopping-demo-app/) by Mike James.
* [Xamarin Around the World with Xamarin Dev Days](https://blog.xamarin.com/xamarin-around-the-world-with-xamarin-dev-days/) by Jayme Singleton.
* [New iOS 10 Privacy Permission Settings](https://blog.xamarin.com/new-ios-10-privacy-permission-settings/), [The Xamarin Show 2 - Continuous Integration with Simina Pasat](https://channel9.msdn.com/Shows/XamarinShow/Continuous-Integration-with-Simina-Pasat), and [The Xamarin Show - Snack Pack 1: Android Emulators](https://channel9.msdn.com/Shows/XamarinShow/Snack-Pack-1-Android-Emulators) by James Montemagno.
* [Preview: iOS Simulator (for Windows) Update 4](https://releases.xamarin.com/preview-ios-simulator-for-windows-update-4/) by Adrian Murphy.
* [NuGet Support in Xamarin Studio 6.1](http://lastexitcode.com/blog/2016/09/17/NuGetSupportInXamarinStudio6-1/) by Matt Ward.
* [Xamarin Forms Triggers vs Behaviors vs Effects](https://xamarinhelp.com/xamarin-forms-triggers-behaviors-effects/) by Adam Pedley.
* [Hololens app with UrhoSharp : Introduction – Part 1](http://blog.lordinaire.fr/2016/09/xamarin-build-hololens-apps-with-urhosharp-part-1/) by Maxime Frappat.
* [Hololens – Xamarin, URHO and an Spatial Mapping sample (with 2 more lines of code it became a Shooting Game)](https://elbruno.com/2016/09/19/hololens-xamarin-urho-and-an-spatial-mapping-sample-with-2-more-lines-of-code-it-became-a-shooting-game/) by Bruno Capuano.
* [The Thumb Zone: Designing For Mobile Users](https://www.smashingmagazine.com/2016/09/the-thumb-zone-designing-for-mobile-users) by Samantha Ingram.
* [Fix for UITest crashing after Xamarin Studio update to 6.1 (build 5441) fails with SetUp : System.InvalidOperationException](http://www.xradapp.com/fix-for-uitest-crashing-after-xamarin-studio-update-to-6-1-build-5441-fails-with-setup-system-invalidoperationexception/) by Mark J Radacz.
* [Yet Another Podcast #164 – Azure Mobile Apps with Chris Risner](http://jesseliberty.com/2016/09/19/yet-another-podcast-164-azure-mobile-apps-with-chris-risner/) by Jesse Liberty.

Games
-----

* [xx](xx) by xx.


And this is it for this week!

Contribute to the week in .NET
------------------------------

As always, this weekly post couldn't exist without community contributions, and I'd like to thank all those who sent links and tips. The F# section is provided by Phillip Carter, the gaming section by Stacey Haffner, and the Xamarin section by Dan Rigby.

You can participate too. Did you write a great blog post, or just read one? Do you want everyone to know about an amazing new contribution or a useful library? Did you make or play a great game built on .NET?
We'd love to hear from you, and feature your contributions on future posts:

* Send an email to beleroy at Microsoft,
* [comment on this gist](https://gist.github.com/bleroy/dab191a0bfc1909b777aed2b3fdb7f09)
* Leave us a pointer in the comments section below.
* [Send Stacey (@yecats131) tips on Twitter about .NET games](https://twitter.com/yecats131).

This week's post (and future posts) also contains news I first read on [The ASP.NET Community Standup](https://blogs.msdn.microsoft.com/webdev/tag/communitystandup/), on [Weekly Xamarin](http://weeklyxamarin.com/), on [F# weekly](https://sergeytihon.wordpress.com/category/f-weekly/), on [ASP.NET Weekly](http://www.aspnetweekly.com/), and on [Chris Alcock's The Morning Brew](http://themorningbrew.net/).
