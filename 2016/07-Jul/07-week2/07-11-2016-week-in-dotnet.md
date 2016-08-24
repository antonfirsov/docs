The week in .NET - 7/11/2016
============================

To read the last post, see [The week in .NET – 6/28/2016](https://blogs.msdn.microsoft.com/dotnet/2016/06/28/the-week-in-net-6282016/).

On .NET
-------
Last week, we had Mukul Sabharwal on the show to talk about .NET Core usage at Bing.

<iframe width="560" height="315" src="https://www.youtube.com/embed/voT4VhDboC4" frameborder="0" allowfullscreen></iframe>

This week, we'll have Lucas Meijer from [Unity Technologies](http://unity3d.com/) on [the show](https://www.youtube.com/channel/UCvtT19MZW8dq5Wwfu6B0oxw/).

Package of the week: LinqToTwitter
----------------------------------
[LINQ to Twitter](https://github.com/JoeMayo/LinqToTwitter) is an open source library that enables querying Twitter using standard LINQ syntax. 

Here's an example of a query that returns search results where people are tweeting about LINQ to Twitter:


```csharp
var twitterCtx = new TwitterContext(...);

var searchResponse =
    await
    (from search in twitterCtx.Search
    where search.Type == SearchType.Search &&
        search.Query == "\"LINQ to Twitter\""
    select search)
    .SingleOrDefaultAsync();

if (searchResponse != null && searchResponse.Statuses != null)
    searchResponse.Statuses.ForEach(tweet =>
        Console.WriteLine(
        "User: {0}, Tweet: {1}", 
        tweet.User.ScreenNameResponse,
        tweet.Text));
```

Game of the week: Assault Android Cactus
-----------------------------------

[Assault Android Cactus](https://madewith.unity.com/games/assault-android-cactus) is a fast paced arcade style twin stick shooter. In Assault Android Cactus, players must battle waves of enemies and giant robots while avoiding having their battery fully drain. (You are androids, after all!) Players can select between nine playable androids, each of which has a distinct set of abilities that require a different play style. Assault Android Cactus supports both single player and local-player co-op modes.

![alt](http://www.witchbeam.com.au/presskit/assault_android_cactus/images/aac_008.png) 

Assault Android Cactus was created by [Witch Beam Games](https://madewith.unity.com/profiles/witch-beam-games) using [Unity](http://unity3d.com/), JavaScript and [C#](https://channel9.msdn.com/Series/C-Sharp-Fundamentals-Development-for-Absolute-Beginners). It is currently available on Windows, Mac, Linux, Wii U and PlayStation 4. More information can be found on their [Made with Unity](https://madewith.unity.com/games/assault-android-cactus) page.

User group meeting of the week: The latest on .NET and ASP.NET with Scott Hanselman
------------------------------------------------
On Wednesday, July 13 at 12:00PM, the Adelaide .NET User Group is hosting a meeting where you’ll hear an update on the progress of .NET, .NET Core and ASP.NET Core.


.NET
----

* [Where's DNVM? Safely running multiple versions of the .NET Core SDK and Tooling with global.json](http://www.hanselman.com/blog/WheresDNVMSafelyRunningMultipleVersionsOfTheNETCoreSDKAndToolingWithGlobaljson.aspx), by Scott Hanselman.
* [How the dotnet CLI tooling runs your code](http://mattwarren.org/2016/07/04/How-the-dotnet-CLI-tooling-runs-your-code/), by Matt Warren.
* [Lucene.NET 4.8 is in beta - and we need your help!](http://code972.com/blog/2016/07/98-lucene-net-4-8-is-in-beta-and-we-need-your-help)
* [Step by step: .NET Core and Entity Framework Core](https://carlos.mendible.com/2016/07/11/step-by-step-dotnet-core-and-entity-framework-core/), by Carlos Mendible.
* [Use the Desktop Bridge to Bring Your Apps to UWP - Video](https://channel9.msdn.com/Blogs/One-Dev-Minute/Use-the-Desktop-Bridge-to-Bring-Your-Apps-to-UWP), by Rosshe Keantonc.
* [LongRunning Is Useless For Task.Run With async-await](http://blog.i3arnon.com/2015/07/02/task-run-long-running/), by Bar Arnon
* [Getting started with StructureMap in ASP.NET Core](http://andrewlock.net/getting-started-with-structuremap-in-asp-net-core/), by Andrew Lock
* [Deploy a Service Fabric Cluster to Azure with .NET Framework 4.6 (ARM template)](http://www.medic-consulting.com/2016/06/23/Deploy-a-Service-Fabric-Cluster-with-support-for-net-4-6/), by Andrej Medic 
* [How to configure urls for Kestrel, WebListener and IIS express in ASP.NET Core](http://andrewlock.net/configuring-urls-with-kestrel-iis-and-iis-express-with-asp-net-core/), by Andrew Lock

ASP.NET
-------

* [Understanding Routing Precedence in ASP.NET MVC and Web API](http://www.codeproject.com/Articles/1110613/Understanding-Routing-Precedence-in-ASP-NET-MVC-an), by Rion Williams.
* [How to Master ASP.NET Core Web API Attribute Routing](http://mobilemancer.com/2016/07/06/how-to-master-asp-net-core-web-api-attribute-routing/), on Mobilemancer.
* [Adding parameters to the OpenID Connect Authorization URL](http://www.jerriepelser.com/blog/adding-parameters-to-openid-connect-authorization-url), by Jerrie Pelser.
* [Issuing and authenticating JWT tokens in ASP.NET Core WebAPI – Part I](https://goblincoding.com/2016/07/03/issuing-and-authenticating-jwt-tokens-in-asp-net-core-webapi-part-i/), by William Hallatt.
* [How to continuously deploy a ASP.​NET Core 1.0 web app to Microsoft Azure](http://asp.net-hacker.rocks/2016/07/22/deploy-aspnetcore-to-azure.html), by Jürgen Gutsch.
* [Securing ASP.NET Web API](http://code.tutsplus.com/tutorials/securing-aspnet-web-api--cms-26012), by Sovit Poudel


F#
--

* [Continuous - F# IDE for the iPad](http://continuous.codes), by Frank Krueger
* [Hacking Web Stuff with F#](https://skillsmatter.com/skillscasts/8324-hacking-web-stuff-with-f-sharp), by Phil Trelford & Tomas Petricek
* [Getting Started with F# on .NET Core](https://docs.microsoft.com/en-us/dotnet/articles/fsharp/getting-started-netcore), by Phillip Carter
* [Referencing an F# library from C# on .NET Core](http://kalapos.azurewebsites.net/referencing-an-f-library-from-c-on-net-core), by Gergely Kalapos

Check out [F# Weekly](https://sergeytihon.wordpress.com/category/f-weekly/) for more great content from the F# community.

Xamarin
-------
* [What's New in Xamarin.Forms - Video](https://channel9.msdn.com/Shows/Visual-Studio-Toolbox/Whats-New-in-XamarinForms), by James Montemagno.
* [Using Xamarin Forms with .NET Standard](https://oren.codes/2016/07/09/using-xamarin-forms-with-net-standard/), by Oren Novotny.
* [.NET Standard Library with Xamarin Forms](https://xamarinhelp.com/dot-net-standard-pcl-xamarin-forms/), by Adam Pedley.
* [Unit testing with Xamarin.Forms' DependencyService](http://arteksoftware.com/unit-testing-with-xamarin-forms-dependencyservice), by Rob Gibbens.


Games
-----

* [Unity 5 Tutorial: How to make a Crafting system like in Minecraft part 1 - Video](https://www.youtube.com/watch?v=gF0lROPs0WI), by Gamad
* [Build Your First Game with MonoGame: Getting Started](https://blog.xamarin.com/build-your-first-game-with-monogame-getting-started/), by Dean Ellis.
* [Unity 5 Swimming System and tutorial - Video](https://www.youtube.com/watch?v=SKhKJUtaMt0&feature=share), by Jay AnAm


And this is it for this week!

Contribute to the week in .NET
------------------------------

As always, this weekly post couldn't exist without community contributions, and I'd like to thank all those who sent links and tips.

You can participate too. Did you write a great blog post, or just read one? Do you want everyone to know about an amazing new contribution or a useful library? Did you make or play a great game built on .NET?
We'd love to hear from you, and feature your contributions on future posts:

* Send an email to beleroy at Microsoft,
* [comment on this gist](https://gist.github.com/staceyhaffner/9e1bac8418d1a94ca5387137cc6a1a67)
* Leave us a pointer in the comments section below.
* [Send Stacey (@yecats131) tips on Twitter about .NET games](https://twitter.com/yecats131).

This week's post (and future posts) also contains news I first read on [The ASP.NET Community Standup](https://blogs.msdn.microsoft.com/webdev/tag/communitystandup/), on [Weekly Xamarin](http://weeklyxamarin.com/), on [F# weekly](https://sergeytihon.wordpress.com/category/f-weekly/), on [ASP.NET Weekly](http://www.aspnetweekly.com/), and on [Chris Alcock's The Morning Brew](http://themorningbrew.net/).
