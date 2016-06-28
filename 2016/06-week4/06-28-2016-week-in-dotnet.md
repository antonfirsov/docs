The week in .NET - 6/28/2016
============================

To read last week's post, see [The week in .NET – 6/21/2016](https://blogs.msdn.microsoft.com/dotnet/2016/06/21/the-week-in-net-6212016/).

We shipped!
-----------

We are excited to announce the release of .NET Core 1.0, [ASP.NET Core 1.0](https://blogs.msdn.microsoft.com/webdev/2016/06/27/announcing-asp-net-core-1-0/) and [Entity Framework Core 1.0](https://blogs.msdn.microsoft.com/dotnet/2016/06/27/entity-framework-core-1-0-0-available), available on Windows, OS X and Linux! .NET Core is a cross-platform, open source, and modular .NET platform for creating modern web apps, microservices, libraries and console applications.

This release includes the .NET Core runtime, libraries and tools and the ASP.NET Core libraries. We are also releasing Visual Studio and Visual Studio Code extensions that enable you to create .NET Core projects. You can get started at [https://dot.net/core](https://dot.net/core).

The Visual Studio team also released [Visual Studio 2015 Update 3](https://blogs.msdn.microsoft.com/visualstudio/2016/06/27/visual-studio-2015-update-3-and-net-core-1-0-available-now/) today. You need that release to build .NET Core apps in Visual Studio.

Finally, we invite you to check out [the new .NET Core documentation web site](https://docs.microsoft.com/en-us/dotnet/articles/core/index), [the ASP.NET Core documentation web site](https://docs.asp.net/en/latest/), and [our great new interactive C# tutorial](https://www.microsoft.com/net/tutorials/csharp/getting-started). 

On .NET
-------

Last week, we had Jeremy Kuhne on the show to talk about long path support in .NET.

<iframe width="560" height="315" src="https://www.youtube.com/embed/ZppqEMegCAA" frameborder="0" allowfullscreen></iframe>

This week, we'll have [a special show with Scott Hunter](https://www.youtube.com/watch?v=Ora3MZi3ius) to talk about the .NET Core 1.0 release. As usual, we'll take questions from the audience.

Package of the week: MyTested.AspNetCore.Mvc - Fluent testing framework for ASP.NET Core MVC
--------------------------------------------------------------------------------------------

Even though ASP.NET MVC has been designed to be testable, any help writing tests for routes and controllers is useful. The [MyTested.AspNetCore](https://mytestedasp.net/) library makes writing such tests easy and fun.

Here's how you'd test that a route is calling into the right controller action, with the correct model:

```csharp
MyMvc
    .Routes()
    .ShouldMap(request => request
        .WithLocation("/My/Action/1")
        .WithMethod(HttpMethod.Post)
        .WithAuthenticatedUser()
        .WithAntiForgeryToken()
        .WithJsonBody(new
        {
            Integer = 1,
            String = "Text"
        }))
    .To<MyController>(c => c.Action(1, new MyModel
    {
        Integer = 1,
        String = "Text"
    }))
    .AndAlso()
    .ToValidModelState();
```

And here's how you'd test a controller action:

```csharp
MyMvc
    .Controller<MvcController>()
    .WithOptions(options => options
        .For<AppSettings>(settings => settings.Cache = true))
    .WithSession(session => session
        .WithEntry("Session", "SessionValue"))
    .WithDbContext(db => db.WithEntities(entities => entities
        .AddRange(SampleDataProvider.GetModels())))
    .Calling(c => c.SomeAction())
    .ShouldHave()
    .MemoryCache(cache => cache
        .ContainingEntry(entry => entry
            .WithKey("CacheEntry")
            .WithSlidingExpiration(TimeSpan.FromMinutes(10))))
    .AndAlso()
    .ShouldReturn()
    .View()
    .WithModelOfType<ResponseModel>()
    .Passing(m =>
    {
        Assert.AreEqual(1, m.Id);
        Assert.AreEqual("Some property value", m.SomeProperty);
    });
```

Game of the week: xx
-----------------------------------


User group meeting of the week: Building Smart Apps with Microsoft Cognitive Services in Boston
-----------------------------------------------------------------------------------------------

On Thursday, June 30 at 6:15PM, the [Boston Mobile C# Developers group](http://www.meetup.com/bostonmobiledev/) is [hosting a meeting](http://www.meetup.com/bostonmobiledev/events/231715511/) where you’ll learn how to build smarter apps that can analyze images and text, perform speech recognition, linguistic analysis, provide recommendations, and more – all with just a few lines of code. 

.NET
----

* [Announcing .NET Core 1.0](https://blogs.msdn.microsoft.com/dotnet/2016/06/27/announcing-net-core-1-0/) by Rich Lander.
* Wired: [Microsoft Says It's in Love With Linux. Now It's Finally Proving It](http://www.wired.com/2016/06/microsofts-open-source-love-affair-reaches-new-heights/) by Klint Finley.
* Ars Technica: [.NET Core 1.0 released, now officially supported by Red Hat](http://arstechnica.com/information-technology/2016/06/net-core-1-0-released-now-officially-supported-by-red-hat/) by Peter Bright.
* [Reactive and Interactive Extensions for .NET 3.0](https://github.com/Reactive-Extensions/Rx.NET/releases/tag/v3.0.0) by Oren Novotny.
* [NDepend – the king of code metrics](http://piotrgankiewicz.com/2016/06/27/ndepend-the-king-of-code-metrics/) by Piotr Gankiewicz.
* [Portable- is dead, long live NetStandard](https://oren.codes/2016/06/23/portable-is-dead-long-live-netstandard/) by Oren Novotny.
* [More on Inking with Wet Ink & Custom Rulers](https://mtaulty.com/2016/06/21/windows-10-anniversary-update-more-on-inking-with-wet-ink/) by Mike Taulty.
* [Cross Platform Time Zone Handling for ASP.NET Core](http://www.joeaudette.com/cross-platform-time-zone-handling-for-aspnet-core.aspx) by Joe Audette.

ASP.NET
-------

* [Announcing ASP.NET Core 1.0](https://blogs.msdn.microsoft.com/webdev/2016/06/27/announcing-asp-net-core-1-0/) by Jeffrey T. Fritz.
* [Adding a Custom Inline Route Constraint in ASP.NET Core 1.0](http://www.hanselman.com/blog/AddingACustomInlineRouteConstraintInASPNETCore10.aspx) by Scott Hanselman.
* [Reloading strongly typed Options on file changes in ASP.NET Core RC2](http://andrewlock.net/reloading-strongly-typed-options-when-appsettings-change-in-asp-net-core-rc2/) by Andrew Lock.
* [Building REST APIs using ASP.NET Core and Entity Framework Core](https://chsakell.com/2016/06/23/rest-apis-using-asp-net-core-and-entity-framework-core/) by Christos Sakell.
* [Basics of Middleware in ASP NET Core (video)](https://www.youtube.com/watch?v=B2WftX-etVo) by ProCoder.

F#
--

* [F# gotchas for C# developers](http://dobegin.com/fsharp-gotchas-for-csharp-devs/) by Daniel Lazarenko.
* [F# Implementation of The Elm Architecture](http://anthonylloyd.github.io/blog/2016/06/20/fsharp-elm-part1) by Anthony Lloyd.

Xamarin
-------

* [Xamarin DevOps with VSTS - Deploying To Devices From HockeyApp](http://www.thexamarinjournal.com/xamarin-dev-ops-with-vsts-deploying-to-devices-from-hockeyapp-2/) by Richard Woollcott.
* [Merged Dictionaries with Xamarin Forms](https://xamarinhelp.com/merged-dictionaries-xamarin-forms/), [App Discovery and Deep Linking Series](https://xamarinhelp.com/app-discovery-deep-linking-series/), and [Contributing to Xamarin Forms](https://xamarinhelp.com/contributing-xamarin-forms/) by Adam Pedley.

Games
-----



And this is it for this week!

Contribute to the week in .NET
------------------------------

As always, this weekly post couldn't exist without community contributions, and I'd like to thank all those who sent links and tips.

You can participate too. Did you write a great blog post, or just read one? Do you want everyone to know about an amazing new contribution or a useful library? Did you make or play a great game built on .NET?
We'd love to hear from you, and feature your contributions on future posts:

* Send an email to beleroy at Microsoft,
* [comment on this gist](https://gist.github.com/bleroy/23823a0c50588a44e9ff02b191df983b)
* Leave us a pointer in the comments section below.
* [Send Stacey (@yecats131) tips on Twitter about .NET games](https://twitter.com/yecats131).

This week's post (and future posts) also contains news I first read on [The ASP.NET Community Standup](https://blogs.msdn.microsoft.com/webdev/tag/communitystandup/), on [Weekly Xamarin](http://weeklyxamarin.com/), on [F# weekly](https://sergeytihon.wordpress.com/category/f-weekly/), on [ASP.NET Weekly](http://www.aspnetweekly.com/), and on [Chris Alcock's The Morning Brew](http://themorningbrew.net/).
