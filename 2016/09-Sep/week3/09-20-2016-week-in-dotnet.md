The week in .NET: On .NET with Steeltoe - C# Functional Extensions
==================================================================

To read last week's post, see [The week in .NET: .NET Core 1.0.1 – On .NET with Peachpie – Avalonia – Folk Tale](https://blogs.msdn.microsoft.com/dotnet/2016/09/14/the-week-in-net-net-core-1-0-1-on-net-with-peachpie-avalonia-folk-tale/).

On .NET
-------

Last week, [we had David Morhovich and Zach Brown on the show to talk about Steeltoe](https://channel9.msdn.com/Shows/On-NET/David-Morhovich-and-Zach-Brown-Steeltoe):

<iframe src="https://channel9.msdn.com/Shows/On-NET/David-Morhovich-and-Zach-Brown-Steeltoe/player" width="560" height="315" allowFullScreen frameBorder="0"></iframe>

This week, we'll speak with [Sébastien Ros](http://sebastienros.com/) about [Orchard 2](https://github.com/orchardcms/orchard2), the new version of the .NET CMS that can run on .NET Core. The show begins at 10AM Pacific Time [on Channel 9](https://channel9.msdn.com/Shows/On-NET). We'll take questions on Gitter, on [the dotnet/home channel](https://gitter.im/dotnet/home). Please use the `#onnet` tag. It's OK to start sending us questions in advance if you can't do it live during the show.

Package of the week: C# Functional Extensions
---------------------------------------------

[C# functional extensions](https://github.com/louthy/language-ext) is a project that adds common functional patterns such as tuples, option types, monads, easier Lambda declaration (`var add = fun( (int x, int y) => x + y );`), or even pattern matching. It also implements "Erlang-like" concurrency, which is based on the concept of agents communicating through messages.

```csharp
// Log process
var logger = spawn<string>("logger", Console.WriteLine);

// Ping process
ping = spawn<string>("ping", msg =>
{
    tell(logger, msg);
    tell(pong, "ping", TimeSpan.FromMilliseconds(100));
});

// Pong process
pong = spawn<string>("pong", msg =>
{
    tell(logger, msg);
    tell(ping, "pong", TimeSpan.FromMilliseconds(100));
});

// Trigger
tell(pong, "start");
```

User group meeting of the week: PerfView and Warden in Wrocław, Poland
----------------------------------------------------------------------

Tonight at 6:30 in Wrocław, Poland, [the Wrocław .NET User Group](http://www.meetup.com/wrocnet/) has [a meeting](http://www.meetup.com/wrocnet/events/232697053/) where Michal Malecki and Piotr Gankiewicz will talk about PerfView and Warden.

.NET
----

* [Self-contained .NET Core Applications](http://www.hanselman.com/blog/SelfcontainedNETCoreApplications.aspx) by Scott Hanselman.
* [Marten 1.0 is Here!](https://jeremydmiller.com/2016/09/14/marten-1-0-is-here/) by Jeremy D. Miller.
* [Authenticode Signing Service and Client](https://oren.codes/2016/09/12/authenticode-signing-service-and-client/) by Oren Novotny.
* [Creating Beautiful Effects for UWP](https://blogs.windows.com/buildingapps/2016/09/12/creating-beautiful-effects-for-uwp/#zdYK2PhlbZc1R6EE.97) by Michael Crump.
* [Subverting .NET Type Safety with 'System.Runtime.CompilerServices.Unsafe'](http://mattwarren.org/2016/09/14/Subverting-.NET-Type-Safety-with-System.Runtime.CompilerServices.Unsafe/) (NSFW in a completely new sense of the term) and [Compact strings in the CLR](http://mattwarren.org/2016/09/19/Compact-strings-in-the-CLR/) by Matt Warren.
* [Use HiLo to generate keys with Entity Framework Core](http://www.talkingdotnet.com/use-hilo-to-generate-keys-with-entity-framework-core/) by Talking Dotnet.
* [Asynchronous Programming - Exception Handling](https://github.com/jbe2277/waf/wiki/Exception-Handling) by jbe2277.
* [A first look at Entity Framework Core with SQLite database on Ubuntu Linux](http://vmtri.com/a-first-look-at-entity-framework-core-with-sqlite-database-on-ubuntu-linux/) by Võ Minh Trí.
* [Using PeerFinder from Console: Wi-Fi Direct data transfer in C#](http://blog.plasticscm.com/2016/09/using-peerfinder-from-console-wi-fi.html) by PlasticSCM.
* [Graph Database with Neo4j and a .NET Client](http://thenewstack.io/graph-database-neo4j-net-client/) by Chris Skardon and Michael Hunger.

ASP.NET
-------

* [Custom ASP.NET Core Middleware Example](https://blogs.msdn.microsoft.com/dotnet/2016/09/19/custom-asp-net-core-middleware-example/) by Mike Rousos.
* [Peachpie: What Difference Does the 'Core' Make?](http://www.peachpie.io/2016/09/coretesting.html) by Benjamin Fistein.
* [Configuring environment specific services in ASP.NET Core - Part 2](http://andrewlock.net/configuring-environment-specific-services-in-asp-net-core-part-2/), [HTML minification using WebMarkupMin in ASP.NET Core](http://andrewlock.net/html-minification-using-webmarkupmin-in-asp-net-core/), and [Viewing what's changed in ASP.NET Core 1.0.1](http://andrewlock.net/viewing-whats-changed-in-asp-net-core-1-0-1/) by Andrew Lock.
* [ASP.NET Core with Angular2 - tutorial](https://devblog.dymel.pl/2016/09/08/aspnet-core-with-angular2-tutorial/) by Michał Dymel.
* [Step by step: Serilog with ASP.NET Core](https://carlos.mendible.com/2016/09/19/step-step-serilog-asp-net-core/) and [.NET Core and Microsoft Bot Framework](https://carlos.mendible.com/2016/09/11/netcore-and-microsoft-bot-framework/) by Carlos Mendible.
* [AWS DynamoDB on .NET Core: Getting Started](http://dotnetliberty.com/index.php/2016/09/19/aws-dynamodb-on-net-core-getting-started/) by Armen Shimoon.
* [Bending ASP.NET Core MVC To Your Will](http://jameschambers.com/2016/09/Bending-ASP-NET-MVC-Core-To-Your-Will/) by James Chambers.
* [Include user properties in IdentityServer4 with ASP.NET Identity](http://dkbe.ch/post/include-user-properties-in-identityserver4-with-asp-net-core-identity) by David Keller.
* [Full Server logout with IdentityServer4 and OpenID Connect Implicit Flow](https://damienbod.com/2016/09/16/full-server-logout-with-identityserver4-and-openid-connect-implicit-flow/) by Damien Bowden.
* [Getting started with EF Core + Sqlite + Asp.Net core Web api on Mac](https://wannabeegeek.com/2016/09/09/getting-started-with-ef-core-sqlite-asp-net-core-web-api-on-mac/) by Swaminathan Vetri.
* [Real-World CQRS/ES with ASP.NET and Redis Part 3 - The Read Model](https://www.exceptionnotfound.net/real-world-cqrs-es-with-asp-net-and-redis-part-3-the-read-model/), [Part 4 - Creating the APIs](https://www.exceptionnotfound.net/real-world-cqrs-es-with-asp-net-and-redis-part-4-creating-the-apis/), and [Part 5 - Running the APIs](https://www.exceptionnotfound.net/real-world-cqrs-es-with-asp-net-and-redis-part-5-running-the-apis/) by Matthew Jones.

F#
--

* [Exploring StackOverflow [with F#]](http://evelinag.com/ExploringStackOverflow/#/), by Evelina Gabasova.
* [Langston's Ant in F#](https://technicalitee.blogspot.com.by/2016/09/langtons-ant-in-f.html), by Ram.
* [Programming UrhoSharp with F#](https://developer.xamarin.com/guides/cross-platform/urho/fsharp-and-urhosharp/), by Xamarin.
* [Use apply and carry on](http://red-green-rewrite.github.io/2016/09/14/Use-apply-and-carry-on/), by Milosz Krajewski.
* [F# Series: Building Real-World Applications in F#](https://medium.com/@odytrice/f-series-building-real-world-applications-in-f-b45b62ac653d#.7yzhd17dx), by Ody Mbegbu.

Check out [F# Weekly](https://sergeytihon.wordpress.com/category/f-weekly/) for more great content from the F# community.

Xamarin
-------

* [Major Updates: iOS 10, Android Nougat, and Other Tasty Bits](https://blog.xamarin.com/major-updates-ios-10-android-nougat-and-other-tasty-bits/) by Miguel de Icaza.
* [Xamarin Stable Release: Cycle 8 with iOS 10 and Xcode 8 Support](https://releases.xamarin.com/stable-release-cycle-8-w-ios-10-and-xcode8-support/) and [Xamarin Preview: Xamarin Profiler 0.34.1](https://releases.xamarin.com/preview-xamarin-profiler-0-34-1/) by Adrian Murphy.
* [Testing iOS 10 in Xamarin Test Cloud](https://blog.xamarin.com/testing-ios-10-in-xamarin-test-cloud/) by John Lago.
* [Introducing the Azure Track in Xamarin University](https://blog.xamarin.com/introducing-the-azure-track-in-xamarin-university/) by Bryan Costanich.
* [Preparing for Native Library Linking Changes in Android N](https://blog.xamarin.com/preparing-for-native-library-linking-changes-in-android-n/) and [Preparing Machines for Xamarin Cycle 8 / iOS10 / Android N](http://motzcod.es/post/150380059392/preparing-machines-for-xamarin-cycle) by James Montemagno.
* [Xamarin Podcast: iOS 10, iPhone 7, Apple Watch Series 2, and more!](https://blog.xamarin.com/podcast-ios-10-iphone-7-apple-watch-series-2-and-more/) by Pierce Boggan.
* [Yet Another Podcast #163: James Montemagno and Xamarin Cycle 8](http://jesseliberty.com/2016/09/13/yet-another-podcast-163-james-montemagno-and-xamarin-cycle-8/) by Jesse Liberty.
* [VSTS September Extensions Roundup – App Stores!](https://blogs.msdn.microsoft.com/visualstudioalm/2016/09/13/team-services-september-extensions-roundup-app-stores/) by Joe Bourne.
* [Access a Control Method from a ViewModel](https://xamarinhelp.com/access-a-control-method-from-a-viewmodel/) by Adam Pedley.
* [Now Xamarin Devs can create Hololens Apps!](https://elbruno.com/2016/09/13/hololens-now-xamarin-devs-can-create-hololens-apps/) by Bruno Capuano.
* [LoaderViewXamarin - Loading animations for Android TextView and ImageView elements](https://github.com/martijn00/LoaderViewXamarin) by Martijn van Dijk.

Azure
-----

* [Serving Static Content From Azure Storage: Content Delivery Network Setup](https://www.dougv.com/2016/09/serving-static-content-azure-storage-content-delivery-network-setup/) by Doug Vanderweide.

Games
-----

* [MonoGame – Building multi-platform solutions](http://darkgenesis.zenithmoon.com/monogame-building-multi-platform-solutions/) by Simon Jackson.


And this is it for this week!

Contribute to the week in .NET
------------------------------

As always, this weekly post couldn't exist without community contributions, and I'd like to thank all those who sent links and tips. The F# section is provided by Phillip Carter, the gaming section by Stacey Haffner, and the Xamarin section by Dan Rigby.

You can participate too. Did you write a great blog post, or just read one? Do you want everyone to know about an amazing new contribution or a useful library? Did you make or play a great game built on .NET?
We'd love to hear from you, and feature your contributions on future posts:

* Send an email to beleroy at Microsoft,
* [comment on this gist](https://gist.github.com/bleroy/3459f8817743bffaa26a5dd58682fe9e)
* Leave us a pointer in the comments section below.
* [Send Stacey (@yecats131) tips on Twitter about .NET games](https://twitter.com/yecats131).

This week's post (and future posts) also contains news I first read on [The ASP.NET Community Standup](https://blogs.msdn.microsoft.com/webdev/tag/communitystandup/), on [Weekly Xamarin](http://weeklyxamarin.com/), on [F# weekly](https://sergeytihon.wordpress.com/category/f-weekly/), on [ASP.NET Weekly](http://www.aspnetweekly.com/), and on [Chris Alcock's The Morning Brew](http://themorningbrew.net/).
