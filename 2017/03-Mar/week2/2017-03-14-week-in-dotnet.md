The week in .NET - On .NET with Scott Hunter, On .NET with Matt Watson, MessagePack
======================================================================================

Previous posts:

* [Visual Studio 2017, .NET Core SDK, F# 4.1, On .NET with Phillip Carter, Happy Birthday from John Shewchuk, FNA, Pyre](https://blogs.msdn.microsoft.com/dotnet/2017/03/08/the-week-in-net-visual-studio-2017-net-core-sdk-f-4-1-on-net-with-phillip-carter-happy-birthday-from-john-shewchuk-pyre/)
* [On .NET with Eric Mellino, Happy Birthday from Scott Hunter, OzCode](https://blogs.msdn.microsoft.com/dotnet/2017/02/28/the-week-in-net-on-net-with-eric-mellino-happy-birthday-from-scott-hunter-ozcode/).
* [On .NET with Beth Massi, NeinLinq](https://blogs.msdn.microsoft.com/dotnet/2017/02/22/the-week-in-net-on-net-with-beth-massi-neinlinq/).

On .NET
-------

We recorded two videos last week. In [the first one](https://channel9.msdn.com/Shows/On-NET/Scott-Hunter-NET-Core-SDK), [Scott Hunter](https://twitter.com/coolcsh) showed Visual Studio 2017 and .NET Core SDK 1.0:

<iframe src="https://channel9.msdn.com/Shows/On-NET/Scott-Hunter-NET-Core-SDK/player" width="960" height="540" allowFullScreen frameBorder="0"></iframe>

In the second video, [Matt Watson](https://stackify.com/author/mwatson/) from [Stackify](https://stackify.com/) showed us [Prefix](https://stackify.com/prefix/), a free lightweight dev tool that shows you real-time logs, errors, queries, and more, and [Retrace](https://stackify.com/retrace/), a powerful commercial solution for gathering and analysis of performance data.

<iframe src="https://channel9.msdn.com/Shows/On-NET/Matt-Watson-Prefix-and-Retrace/player" width="960" height="540" allowFullScreen frameBorder="0"></iframe>

This week, we'll have Sidarth Gupta and Hong-Seok Kim from Samsung on the show to talk about their Tizen OS and its support for .NET Core. The show won't be live this week, because of the time difference with Korea, but you may still send us your questions on [Gitter's dotnet/home channel](https://gitter.im/dotnet/home) and on Twitter. Please use the `#onnet` tag.

Happy Birthday .NET!
--------------------

We have another Happy Birthday .NET video for you this week.


Package of the week: MessagePack for C#
---------------------------------------

I've featured quite a few serialization libraries in the past. I have another one for you this week. [MessagePack](http://msgpack.org/) is an efficient binary serialization format that is supported on more than 50 development platforms. It's also used by Redis. There are at least five different implementations of MessagePack for .NET, but the one I want to showcase today is [MessagePack for C# by Yoshifumi Kawai, also known as neuecc](https://twitter.com/neuecc), who also built [ZeroFormatter](https://github.com/neuecc/ZeroFormatter/). His library supports LZ4 compression, and achieves extremely fast speeds on both serialization and deserialization, with a very compact serialized size.

First, you can define a serializable data object.

```csharp
[MessagePackObject]
public class MyClass
{
    // Key is serialization index, it is important for versioning.
    [Key(0)]
    public int Age { get; set; }

    [Key(1)]
    public string FirstName { get; set; }

    [Key(2)]
    public string LastName { get; set; }

    // public members and does not serialize target, mark IgnoreMemberttribute
    [IgnoreMember]
    public string FullName { get { return FirstName + LastName; } }
}
```

And then you can serialize and deserialize it.

```csharp
var mc = new MyClass
{
    Age = 23,
    FirstName = "Bertrand",
    LastName = "Le Roy",
};

var bytes = MessagePackSerializer.Serialize(mc);
var mc2 = MessagePackSerializer.Deserialize<MyClass>(bytes);
```

User group meeting of the week: the state of .NET in Sydney
-----------------------------------------------------------

On [Wednesday, March 15, at 6:30PM in Sydney, Australia](https://www.meetup.com/Sydney-NET-User-Group/events/238044992/), the [Sydney .NET User Group](https://www.meetup.com/Sydney-NET-User-Group/) holds [a meeting on the state of .NET, presented by Markus Egger](https://www.meetup.com/Sydney-NET-User-Group/events/238044992/).

.NET
----

* [What's New in the .NET Platform (video)](https://channel9.msdn.com/Events/Visual-Studio/Visual-Studio-2017-Launch/T103) by Scott Hunter.
* [Announcing NuGet 4.0 RTM](http://blog.nuget.org/20170308/Announcing-NuGet-4.0-RTM.html) by Anand Gaurav.
* [Optimize your productivity with .NET in Visual Studio 2017](https://blogs.msdn.microsoft.com/visualstudio/2017/03/08/optimize-your-productivity-with-net-in-visual-studio-2017-2/) by Kasey Uhlenhuth.
* [Live Unit Testing in Visual Studio 2017 Enterprise](https://blogs.msdn.microsoft.com/visualstudio/2017/03/09/live-unit-testing-in-visual-studio-2017-enterprise/) by Joe Morris and Manish Jayaswal.
* [Run To Click Debugging in Visual Studio 2017](https://blogs.msdn.microsoft.com/visualstudioalm/2017/03/07/run-to-click-debugging-in-visual-studio-2017/) by Kaycee Anderson.
* [Cake v0.18.0 released](http://cakebuild.net/blog/2017/03/cake-v0.18.0-released).
* [Getting Started with .NET Core on Mac and Linux](https://www.pluralsight.com/courses/dotnet-core-mac-linux-getting-started) by Nate Cook for Pluralsight.
* [P/invoke with unions in C#](http://yizhang82.me/pinvoke-union) by Yi Zhang.
* [Exploring .NET Core with Visual Studio 2017 and the updated CLI Tools](http://michaelcrump.net/part11-aspnetcore/) by Michael Crump.
* [Turning off Telemetry Data in .NET Core](http://michaelcrump.net/part12-aspnetcore/) by Michael Crump.

ASP.NET
-------

* [Supporting both LTS and Current releases for ASP.NET Core](https://andrewlock.net/supporting-both-lts-and-current-releases-for-asp-net-core/) by Andrew Lock.
* [What is the Microsoft.AspNetCore metapackage?](https://andrewlock.net/what-is-the-microsoft-aspnetcore-metapackage/) by Andrew Lock.
* [Testing an ASP.NET Core MVC Protobuf API using HTTPClient and xUnit](https://damienbod.com/2017/03/09/testing-an-asp-net-core-mvc-protobuf-api-using-httpclient-and-xunit/) by Damien Bowden.
* [ASP.NET Core MVC Anatomy (Part 1) – AddMvcCore](https://www.stevejgordon.co.uk/asp-net-core-mvc-anatomy-addmvccore) by Steve Gordon.
* [Fritz's 10 Minute Tips – ASP.NET Core Configuration](http://www.jeffreyfritz.com/2017/03/fritzs-10-minute-tips-asp-net-core-configuration/) by Jeffrey T. Fritz.
* [ASP.NET Core: Environment based configuring methods](http://gunnarpeipman.com/2017/03/aspnet-core-configure-environment/) by Gunnar Peipman.
* [Environment Variables in ASP.NET Core](https://codeopinion.com/environment-variables-asp-net-core/) by Derek Comartin.
* [A way too early discussion of "Jasper"](https://jeremydmiller.com/2017/03/08/a-way-too-early-discussion-of-jasper/) by Jeremy D. Miller.
* [Creating a simple blog engine](http://isolineltd.com/blog/2017/03/08/Creating-a-simple-Blog-Engine) by Ivan Gavryliuk.
* [Book: Applying Domain Driven Design with CQRS and Event Sourcing](https://buildplease.com/pages/now-what/) by Nick Chamberlain, with all examples written with ASP.NET MVC.
* [Precompiling MVC Views in ASP.NET Core with .csproj](https://scottsauber.com/2017/03/10/pre-compiling-razor-views-in-asp-net-core-with-csprojs/) by Scott Sauber.
* [Improve the security of your website using SSL and HSTS with ASP.NET Core](https://www.softfluent.com/blog/dev/2017/03/06/Improve-the-security-of-your-website-using-SSL-and-HSTS-with-ASP-NET-Core) by Gérald Barré.
* [Simple localization and language based URLs](http://gunnarpeipman.com/2017/03/aspnet-core-simple-localization/) by Gunnar Peipman.

C#
--

* [New Features in C# 7.0](https://blogs.msdn.microsoft.com/dotnet/2017/03/09/new-features-in-c-7-0/) by Mads Torgersen.
* [Exploring C# Productivity in Visual Studio 2017 (video)](https://channel9.msdn.com/Events/Visual-Studio/Visual-Studio-2017-Launch/140) by Mads Torgersen and Kasey Uhlenhuth.

F#
--

[F# eXchange 2017](https://skillsmatter.com/conferences/8053-f-sharp-exchange-2017) is in London, April 6-7. Speakers include Don Syme, Phillip Carter, Scott Wlaschin, and many others.

* [Using Elixir and F# Together - Bryan Hunter](http://www.channel64.net/2017/03/pipe-forward-using-elixir-and-f.html).
* [Some Details about Visual F# Tools in VS 2017](https://vasily-kirichenko.github.io/fsharpblog/), by Vasily Kirichenko.
* [Creating an Azure Function in F# from the ground up (Part 2)](http://brandewinder.com/2017/03/06/fsharp-azure-function-from-the-ground-up-part-2/), by Mathias Brandewinder.
* [Magic of F# Type Providers ](https://medium.com/@maximcus/magic-of-f-type-providers-225b1169c7a0#.u6tytsm6a), by Max Fedotov.
* [Creating a fully functional F# microservice part 2: Azure, FSharp.Configuration](https://mnie.github.io/2017-03-11-sentimentAppPart2/), and [part 3: Quartz.Net, Net.Mail](https://mnie.github.io/2017-03-11-sentimentAppPart3/) by Michał Niegrzybowski.
* [Contractive Functions on Streams in F#](https://medium.com/@dogwith1eye/contractive-functions-on-streams-in-f-286fca88d83f#.5bn1upjoz), by Matthew Doig.

New F# Language Suggestions:

- [List's exists2 inconsistent with Seq's exists2](https://github.com/fsharp/fslang-suggestions/issues/550)
- [Warn when an auto-property getter is confused with a default value](https://github.com/fsharp/fslang-suggestions/issues/549)

Check out [F# Weekly](https://sergeytihon.wordpress.com/category/f-weekly/) for more great content from the F# community.

Xamarin
-------

* [Visual Studio for Mac - Preview 4](https://releases.xamarin.com/preview-4-visual-studio-for-mac/) by Bri Brothers.
* [Xamarin Alpha Release: 15.1 Alpha Preview 2](https://releases.xamarin.com/alpha-release-15-1-alpha-preview-2/) by Bri Brothers.
* [Xamarin Dev Days are Coming to your City!](https://blog.xamarin.com/xamarin-dev-days-are-coming-to-your-city/) by Jayme Singleton.
* [Better Apps Faster with Visual Studio 2017 and Xamarin](https://blog.xamarin.com/better-apps-visual-studio-2017/) by Miguel de Icaza.
* [Live Webinar: Introduction to Xamarin for Visual Studio 2017](https://blog.xamarin.com/live-webinar-introduction-to-xamarin-for-visual-studio-2017/) by James Montemagno.
* [Mobile Center: Xamarin support, detailed app analytics, and more](https://blogs.msdn.microsoft.com/visualstudio/2017/03/07/mobile-center-xamarin-support-detailed-app-analytics-and-more/) by Keith Ballinger.
* [Avoid these six mobile development pitfalls](https://blogs.msdn.microsoft.com/visualstudio/2017/03/03/avoid-these-six-mobile-development-pitfalls/) by Cormac Foster.
* [Setting up Visual Studio 2017 for Xamarin Development](http://motzcod.es/post/158155898027/setting-up-vs-2017-for-xamarin-dev) by James Montemagno.
* [NuGet Support in Xamarin Studio 6.2](http://lastexitcode.com/blog/2017/03/05/NuGetSupportInXamarinStudio6-2/) by Matt Ward.
* [Xamarin Forms User Control](https://xamarinhelp.com/xamarin-forms-user-control/) by Adam Pedley.
* [Xamarin.Controls – Creating Your Own iOS Markdown UILabel](https://alexdunn.org/2017/03/03/xamarin-controls-creating-your-own-ios-markdown-uilabel/), & [Xamarin.Forms Borderless Entry](https://alexdunn.org/2017/03/08/xamarin-forms-borderless-entry/) by Alex Dunn.
* [Xamarin Plugins / .NET Standard with Martijn van Dijk and Michael Ridland](http://www.michaelridland.com/xamarin/xamarin-plugins-net-standard-with-martijn-van-dijk-and-michael-ridland/) by Michael Ridland.
* [Xamarin Forms, MVVMCross, and SkiaSharp: The Holy Trinity of Cross-Platform App Development](https://www.toptal.com/mobile/xamarin-mvvmcross-skiasharp-cross-platform) by Sylvain Gravel.
* [A Xamarin port of the usb-serial-for-android library](https://rajapet.com/2017/03/02/a-xamarin-port-of-the-usb-serial-for-android-library/) by Chris Miller.
* [Pull to Refresh Example Using Xamarin.Forms](http://15mgm15.ghost.io/2017/03/03/pull-to-refresh-example-using-xamarin-forms/) by Mario Jesús Galván Miranda.
* [Sending Files to a Xamarin.Forms App - Part 2: Android](https://codemilltech.com/sending-files-to-a-xamarin-forms-app-part-2-android/) by Matthew Soucoup.
* [Swipe to Delete and more on Xamarin.iOS](https://canbilgin.wordpress.com/2017/03/04/swipe-to-delete-and-more-on-xamarin-ios/) by Can Bilgin.
* [Xamarin Forms with Microsoft Azure](https://mobileprogrammerblog.wordpress.com/2017/03/04/xamarin-forms-with-microsoft-azure/) by Daniel Krzyczkowski.
* [Validating User Input in Xamarin.Forms](http://www.davidbritch.com/2017/03/validating-user-input-in-xamarinforms.html), & [Validating User Input in Xamarin.Forms II](http://www.davidbritch.com/2017/03/validating-user-input-in-xamarinforms-ii.html) by David Britch.
* [Forms Previewer and Custom Controls](https://peterfoot.net/2017/03/08/forms-previewer-and-custom-controls/) by Peter Foot.

UWP
----

* [Visual Studio 2017 – Now Ready for Your Windows Application Development Needs](https://blogs.windows.com/buildingapps/2017/03/07/visual-studio-2017-now-ready-windows-application-development-needs/) By Karan Nandwani.
* [Desktop Bridge: Smooth User Transition and Data Migration](https://blogs.windows.com/buildingapps/2017/03/10/desktop-bridge-smooth-user-transition-data-migration/) By Arian Ghotbi.
* [Building the Terminator Vision HUD in HoloLens](https://blogs.windows.com/buildingapps/2017/03/06/building-terminator-vision-hud-hololens/) By Windows Apps Team.
* [Playable Ads – Acquire Users Who Love to Engage with Your App](https://blogs.windows.com/buildingapps/2017/03/09/playable-ads-acquire-users-love-engage-app/) by Vikram Bodavula.
* [Dragging and Dropping Images and Files into the Web Browser Control](https://weblog.west-wind.com/posts/2017/Mar/10/Dragging-and-Dropping-Images-and-Files-into-the-Web-Browser-Control) by Rick Strahl.
* [Debugging the Web Browser Control with FireBug](https://weblog.west-wind.com/posts/2017/Mar/08/Debugging-the-Web-Browser-Control-with-FireBug) by Rick Strahl.

Data
----

* [Oracle Data Provider for .NET Support for Microsoft .NET Core](http://www.oracle.com/technetwork/topics/dotnet/tech-info/odpnet-dotnet-core-sod-3628981.pdf) by Oracle.
* [ODP.NET on Microsoft .NET Core](http://www.maherjendoubi.io/odp-net-on-microsoft-net-core/) by Maher Jendoubi.

Games
-----

* [Getting Started with MonoGame on Visual Studio 2017](https://youtu.be/zphaylhOrm0) by Simon Jackson.
* [GDC 2017 Talks](http://www.gdcvault.com/browse/gdc-17).
* [Project Tanks 1: Simple Fake-3D Wireframes](http://www.jfurness.uk/project-tanks-1-easy-fake-3d-wireframes/) by James Furness.
* [Curated #UnityTips No. 36 by Devdog March 2017](http://devdog.io/blog/2017/03/11-best-unity-tips-for-game-developers-36).
* [Fixeds, Floats and a Block Damage Effect](http://kylehalladay.com/blog/tutorial/2017/03/13/GlitchFX-In-Unity.html) by Kyle Halladay.
* [Real Time Strategy in Unity - Making Units Construct Buildings (2)](https://youtu.be/veClc0W7dic) by Unit02Games.
* [11.0 Unity Tower defense tutorial - Selling towers](https://youtu.be/x2iVy6piOUE?list=PLX-uZVK_0K_4uNwvKian1bscP9mVvOp1M) by inScope Studios.

And this is it for this week!

Contribute to the week in .NET
------------------------------

As always, this weekly post couldn't exist without community contributions, and I'd like to thank all those who sent links and tips. The F# section is provided by [Phillip Carter](https://twitter.com/_cartermp), the gaming section by [Stacey Haffner](https://twitter.com/yecats131), and the Xamarin section by [Dan Rigby](https://twitter.com/DanRigby), and the UWP section by [Michael Crump](http://twitter.com/mbcrump).

You can participate too. Did you write a great blog post, or just read one? Do you want everyone to know about an amazing new contribution or a useful library? Did you make or play a great game built on .NET?
We'd love to hear from you, and feature your contributions on future posts:

* Send an email to beleroy at Microsoft,
* [comment on this gist](https://gist.github.com/bleroy/94f6d416cd1d46d94db2a360259f5cd5)
* Leave us a pointer in the comments section below.
* [Send Stacey (@yecats131) tips on Twitter about .NET games](https://twitter.com/yecats131).

This week's post (and future posts) also contains news I first read on [The ASP.NET Community Standup](https://blogs.msdn.microsoft.com/webdev/tag/communitystandup/), on [Weekly Xamarin](http://weeklyxamarin.com/), on [F# weekly](https://sergeytihon.wordpress.com/category/f-weekly/), and on [Chris Alcock's The Morning Brew](http://themorningbrew.net/).
