The week in .NET - On .NET on CoreRT & .NET Native - 
============================

To read last week's post, see [The week in .NET – .NET Foundation – Serilog – Super Dungeon Bros](https://blogs.msdn.microsoft.com/dotnet/2016/11/01/the-week-in-net-net-foundation-serilog-super-dungeon-bros/).

On .NET
-------

Last week, [Mei-Chin Tsai and Jan Kotas were on the show](https://channel9.msdn.com/Shows/On-NET/Mei-Chin-Tsai--Jan-Kotas-CoreRT--NET-Native):

<iframe src="https://channel9.msdn.com/Shows/On-NET/Mei-Chin-Tsai--Jan-Kotas-CoreRT--NET-Native/player" width="960" height="540" allowFullScreen frameBorder="0"></iframe>

This week, we won't be streaming live, but we'll be taking advantage of the presence of many [MVPs](https://mvp.microsoft.com/en-us) on campus for the MVP Summit to speak with as many of them as possible in the form of short 10-15 minute interviews.

Package of the week: Enums.NET
------------------------------

[Enums.NET](https://github.com/TylerBrinkley/Enums.NET) is a high-performance type-safe .NET enum utility library which caches enum members' name, value, and attributes and provides many operations as C# extension methods for ease of use. It is available as a [NuGet Package](https://www.nuget.org/packages/Enums.NET/) and is compatible with .NET Framework 2.0+ and .NET Standard 1.0+.

```csharp
foreach (var member in Enums.GetEnumMembers<NumericOperator>())
{
    NumericOperator value = member.Value;
    string name = member.Name;
    Console.WriteLine($"{name}: {value.AsString(EnumFormat.DecimalValue)}");
}
Console.WriteLine($@"{nameof(NumericOperator)} has
{Enums.GetEnumMembers<NumericOperator>().Count()} members and
{Enums.GetEnumMembers<NumericOperator>(excludeDuplicates: true).Count()} distinct members.");
```

![Comparing the performance of Enums.NET with System.Enum](https://github.com/TylerBrinkley/Enums.NET/raw/master/Doc/performance.png)

User group meeting of the week: Intro to HoloLens Development with Unity and UWP in Sterling, VA
------------------------------------------------

[Microsoft Maniacs](http://www.meetup.com/Microsoft-Maniacs/) are holding [a meeting in Sterling, VA on Wednesday, November 9 about HoloLens development using Unity and UWP](http://www.meetup.com/Microsoft-Maniacs/events/233182952/).

.NET
----

* [C# NumberFormat Sections](https://weblog.west-wind.com/posts/2016/Nov/03/C-NumberFormat-Sections) by Rick Strahl.
* [The "Internet of Stranger Things" Wall, Part 1 – Introduction and Remote Wiring](https://blogs.windows.com/buildingapps/2016/10/31/the-internet-of-stranger-things-wall-part-1-introduction-and-remote-wiring/) by Pete Brown.
* [Custom Project File for Building Nuget Packages](http://www.dotnetcatch.com/2016/10/23/custom-project-file-for-building-nuget-packages/) by Robb Schiefer.
* [Why an HTML5-Compliant .NET is Important](http://blog.developers.win/2016/11/why-an-html5-compliant-net-is-important/) by Mike-EEE.
* [Paket-like NuGet with MSBuild](http://www.bricelam.net/2016/10/23/paket-like-nuget-with-msbuild.html) by Brice Lambson.
* [.NET Standard based Windows Service support for .NET](https://github.com/dasMulli/dotnet-win32-service) by Martin Andreas Ullrich.
* [Mapping to Getter-only Properties with EF Core](https://csharp.christiannagel.com/2016/11/07/efcorefields/) by Christian Nagel.
* [Introducing Markdown Monster - a new Markdown Editor](https://weblog.west-wind.com/posts/2016/Nov/04/Introducing-Markdown-Monster-a-new-Markdown-Editor) by Rick Strahl. [Markdown Monster is a WPF application](https://github.com/RickStrahl/MarkdownMonster), and I'm using it to write this post.
* [.NET Document Databases with Marten](http://dontcodetired.com/blog/post/NET-Document-Databases-with-Marten) by Jason Roberts.
* [Learn .NET Core by example (or micro example) – Part II](https://jonhilton.net/2016/11/03/learn-dot-net-core-by-example-part-ii/) by Jon Hilton.
* [High performance field clobbering](https://ayende.com/blog/176002/high-performance-field-clobbering) and [HTTP benchmark and pipelining](https://ayende.com/blog/176001/http-benchmark-and-pipelining) by Ayende Rahien.
* [ASP.NET Core RESTful Web API versioning made easy](http://www.hanselman.com/blog/ASPNETCoreRESTfulWebAPIVersioningMadeEasy.aspx) and [The mystery of dotnet watch and 'Microsoft.NETCore.App', version '1.1.0-preview1-001100-00' was not found](http://www.hanselman.com/blog/TheMysteryOfDotnetWatchAndMicrosoftNETCoreAppVersion110preview100110000WasNotFound.aspx) by Scott Hanselman.
* [Install .NET Core on Mint 18 or Elementary OS](http://hudosvibe.net/post/install-.net-core-on-mint-18-or-elementary-os) by Hrvoje Hudoletnjak.
* [Lazy async initialization for expiring objects](http://www.strathweb.com/2016/11/lazy-async-initialization-for-expiring-objects/) by Filip W.
* [Streaming API in Seq 3.4](https://nblumhardt.com/2016/10/seq-streaming-api/) by Nicholas Blumhardt.
* [.NET Core + RabbitMQ = RawRabbit](http://piotrgankiewicz.com/2016/10/31/net-core-rabbitmq-rawrabbit/) by Piotr Gankiewicz.

ASP.NET
-------

* [Bearer Token Authentication in ASP.NET Core](https://blogs.msdn.microsoft.com/webdev/2016/10/27/bearer-token-authentication-in-asp-net-core/) by Mike Rousos.
* [ASP.NET Core Budgeting App Series (Part 5 FINALE) - A basic UI with React, enabling CORS, and my opinion of ASP.NET Core so far](http://makingoutwith.net/2016/budgeting-app-with-aspnet-core-part-5/) by Joe Petrakovich.
* [Adding Cache-Control headers to Static Files in ASP.NET Core](https://andrewlock.net/adding-cache-control-headers-to-static-files-in-asp-net-core/) by Andrew Lock.
* [ASP.NET Core: Using view injection](http://gunnarpeipman.com/2016/11/aspnet-core-view-injection/) by Gunnar Peipman.
* [Step by step: Scale ASP.NET Core with Docker Swarm](https://carlos.mendible.com/2016/10/30/step-by-step-scale-asp-net-core-with-docker-swarm/) by Carlos Mendible.

F#
--

* [Join the F# Slack Team](http://fsharp.org/guides/slack/)
* [LINQPad Dump for F#](http://markheath.net/post/linqpad-dump-for-f), by Mark Heath
* [F# and .NET Core with SDK Development Group #1](https://www.youtube.com/watch?v=JtTGrh6aIi0), presented by Enrico Sada
* [Case Study: Writing Microservices with F#](http://www.codemag.com/Article/1611071), by Rachel Reese
* [TDD Kata in F#/C# using FsCheck](https://mnie.github.io/2016-10-26-TDDKataInFSharpAndCSharpUsingFsCheck/), by Michał Niegrzybowski

Check out [F# Weekly](https://sergeytihon.wordpress.com/category/f-weekly/) for more great content from the F# community.

Xamarin
-------

* [Xamarin Beta Release: Cycle 8 Service Release 1 RC Builds](https://releases.xamarin.com/beta-release-cycle-8-service-release-1-rc-builds/) and [Xamarin Preview: iOS Simulator (For Windows) update 6](https://releases.xamarin.com/preview-ios-simulator-for-windows-update-6/) by Adrian Murphy.
* [Xamarin Developer Events in November](https://blog.xamarin.com/xamarin-events-for-november-2016/) by Jayme Singleton.
* [Live Webinar | Scale Your Mobile Quality: Industry Benchmarks and Testing Best Practices](https://blog.xamarin.com/live-webinar-scale-your-mobile-quality-industry-benchmarks-and-testing-best-practices/) by Courtney Witmer.
* [Mobile Leaders Podcast | Productivity and Delight in Enterprise Apps with Josh Clark of Big Medium](https://blog.xamarin.com/mobile-leaders-podcast-productivity-and-delight-in-enterprise-apps-with-josh-clark-of-big-medium/) by Anusha Sethuraman.
* [Adding the Microsoft Graph to Your Xamarin.Forms Mobile Apps](https://blog.xamarin.com/using-graph-sdk-xamarin-forms-mobile-apps/) by Mayur Tendulkar
.
* [Xamarin Podcast: What’s New in Xamarin.Forms 2.3.3](https://blog.xamarin.com/podcast-whats-new-in-xamarin-forms-2-3-3/) by Pierce Boggan.
* [Say Hello to Siri with SiriKit](https://blog.xamarin.com/say-hello-siri-sirikit/) by Mike James.
* [The Xamarin Show: Grab a Snack Pack!](https://blog.xamarin.com/the-xamarin-show-grab-a-snack-pack/) by Cody Beyer.
* [The Xamarin Show 8: Microsoft Graph with Simon Jäger](https://channel9.msdn.com/Shows/XamarinShow/Microsoft-Graph-with-Simon-Jager) and [Cross Platform Photos with Media Plugin](http://motzcod.es/post/152529381397/cross-platform-photos-with-media-plugin) by James Montemagno.
* [Operation Separation](https://xamarinhelp.com/operation-separation/), [Introduction to UrhoSharp in Xamarin Forms](https://xamarinhelp.com/introduction-urhosharp-xamarin-forms/), and [UrhoSharp 3D Moving Object](https://xamarinhelp.com/urhosharp-3d-moving-object/) by Adam Pedley.
* [Xamarin Quick Tip – Changing the UISegmentedControl Text Color](http://blog.falafel.com/xamarin-ios-uisegmentedcontrol-text-color/) and [Xamarin Quick Tip : Identifying if a device is an iPad or an iPhone using UserInterfaceIdiom](http://blog.falafel.com/xamarin-ios-userinterfaceidiom/) by Carey Payette.
* [The Golden Rules Of Bottom Navigation Design](https://www.smashingmagazine.com/2016/11/the-golden-rules-of-mobile-navigation-design/) by Nick Babich.
* [Be more awesome with MFractor for Xamarin Studio](http://www.michaelridland.com/xamarin/be-more-awesome-with-mfractor-for-xamarin-studio/) by Michael Ridland.
* [ReactiveUI Goodies – Merge](https://janhannemann.wordpress.com/2016/10/31/reactiveui-goodies-merge/) by Jan Hannemann.
* [Announcing Azure Storage Client Library GA for Xamarin](https://blogs.msdn.microsoft.com/windowsazurestorage/2016/10/31/cross-post-announcing-azure-storage-client-library-ga-for-xamarin/) by Windows Azure Storage.
* [Prism for Xamarin.Forms 6.3 Preview with Improved Template Pack](http://brianlagunas.com/prism-for-xamarin-forms-6-3-preview-with-improved-template-pack/) by Brian Lagunas.
* [Banish Compiler Directives From Shared Projects!](https://codemilltech.com/banish-compiler-directives-from-shared-projects/) by Matthew Soucoup.

Azure
-----

* [Using Azure DocumentDB and ASP.NET Core for extreme NoSQL performance](https://auth0.com/blog/documentdb-with-aspnetcore/) by Matías Quaranta.

And this is it for this week!

Contribute to the week in .NET
------------------------------

As always, this weekly post couldn't exist without community contributions, and I'd like to thank all those who sent links and tips. The F# section is provided by [Phillip Carter](https://twitter.com/_cartermp), the gaming section by [Stacey Haffner](https://twitter.com/yecats131), and the Xamarin section by [Dan Rigby](https://twitter.com/DanRigby).

You can participate too. Did you write a great blog post, or just read one? Do you want everyone to know about an amazing new contribution or a useful library? Did you make or play a great game built on .NET?
We'd love to hear from you, and feature your contributions on future posts:

* Send an email to beleroy at Microsoft,
* [comment on this gist](https://gist.github.com/bleroy/8f5ef8208edd591bac94a84265e15a32)
* Leave us a pointer in the comments section below.
* [Send Stacey (@yecats131) tips on Twitter about .NET games](https://twitter.com/yecats131).

This week's post (and future posts) also contains news I first read on [The ASP.NET Community Standup](https://blogs.msdn.microsoft.com/webdev/tag/communitystandup/), on [Weekly Xamarin](http://weeklyxamarin.com/), on [F# weekly](https://sergeytihon.wordpress.com/category/f-weekly/), and on [Chris Alcock's The Morning Brew](http://themorningbrew.net/).
