The week in .NET - 3/29/2016
============================

To read last week's post, see [The week in .NET – 3/22/2016](https://blogs.msdn.microsoft.com/dotnet/2016/03/22/the-week-in-net-3222016/).

Build conference
----------------

The [Build conference](https://build.microsoft.com/) is this week! Stay tuned to this blog for announcements. You can find [the list of sessions on Channel 9](https://channel9.msdn.com/Events/Build/2016), which is also where you'll be able to watch the videos.

On.NET
------

Last week on On .NET, we had [Richard Kiene from Joyent](https://www.youtube.com/watch?v=v5YUoeFCoe8) to talk about running .NET Core on the Joyent cloud. This week, we'll speak with [John Kemnetz](https://www.youtube.com/watch?v=OjNbBOjcLRk) about the C# extension for VS Code. For the first time on the show, we'll have some demos this week.

Project of the week: Foundatio
------------------------------

[Foundatio](https://github.com/exceptionless/Foundatio) is an open-source set of building blocks for loosely-coupled, distributed apps. It defines a set of interfaces for caching, queues, distributed locks, messaging, jobs, file storage, metrics, and logging. Each interface is implemented by in-memory, Redis, and Azure implementations.

Here's for example how you can enqueue and dequeue a simple work item:

```csharp
public class DoerOfThings {
    private IQueue<SimpleWorkItem> _queue;
    
    public DoerOfThings(IQueue<SimpleWorkItem> queue) {
        _queue = queue;
    }
    
    public async Task EnqueueForLaterWork(SimpleWorkItem item) {
        await _queue.EnqueAsync(item);
    }
    
    public async Task DoSomeWork() {
        var item = await _queue.DequeueAsync();
        // Do work with that item...
    }
}
```

Control of the week: Telerik's ListView for UWP
-----------------------------------------------

[Telerik's ListView control for UWP](http://docs.telerik.com/windows-universal/controls/radlistview/listview-overview) is a great and versatile component that can display responsive lists of items, featuring virtualization, animations, grouping, sorting, and filtering.

<img src="http://d585tldpucybw.cloudfront.net/sfimages/default-source/productsimages/universal-windows-platform/listview-controls.png?sfvrsn=3" alt="Telerik's ListView for UWP" style="width:500px;max-width:100%;"/>


User group meeting of the week: .NET Core at TRINUG.NET
-------------------------------------------------------

[TRINUG.NET](http://www.meetup.com/TRINUG/) has [a meeting in Raleigh, NC, on Wednesday, March 30 at 6:00PM](http://www.meetup.com/TRINUG/events/229590894/) where you'll see how to setup a Linux host, install .NET Core on it, then play with Entity Framework, connect to a MySql server, and run a Web application utilizing all that.

.NET
----

* [On the efficiency of ValueTask](https://blog.i3arnon.com/2015/11/30/valuetask/) by Bar Arnon.
* [Parallelism on a Single Core – SIMD with C#](http://instil.co/2016/03/21/parallelism-on-a-single-core-simd-with-c/) by Eoin Mullan.
* [Why Microsoft Open Source (podcast)](http://developer.telerik.com/topics/web-development/microsoft-open-source/) with Matt Millican.
* [Cross-Platform Messaging for iOS, Android, and Windows](https://blog.xamarin.com/cross-platform-messaging-for-ios-android-and-windows/) by Pierce Boggan.
* [How to create a multi architecture NuGet Package from a UWP class library](http://msicc.net/?p=4442) by Marco Siccardi.

ASP.NET
-------

* [Creating Step-wise Forms with ASP.NET MVC and Kendo UI](http://developer.telerik.com/featured/step-wise-forms-with-asp-net-mvc-and-kendo-ui/) by Ed Charbeneau.
* [Custom Model Binder in ASP.NET MVC](http://www.dotnetcurry.com/aspnet-mvc/1261/custom-model-binder-aspnet-mvc) by Mahesh Sabnis.
* [Dependency Injection Conditional Registration in ASP.NET Core](http://www.elanderson.net/2016/03/dependency-injection-conditional-registration-in-asp-net-core/) by Eric L. Anderson.
* [Social TagHelpers for ASP.NET Core](http://rehansaeed.com/social-taghelpers-for-asp-net-core/) by Muhammad Rehan Saeed.
* [Subresource Integrity TagHelper Using ASP.NET Core part 1](http://rehansaeed.com/subresource-integrity-taghelper-using-asp-net-core/) and [part 2](http://rehansaeed.com/subresource-integrity-taghelper-using-asp-net-core-part-2/) by Muhammad Rehan Saeed.
* [Structured logging with Serilog in ASP.NET Core (video)](http://aspnetmonsters.com/2016/03/monsters-weekly%5Cep17/) by the ASP.NET Monsters.

F#
--

* [F#, Azure, and the Web](https://www.youtube.com/watch?v=DZLSkWHLFII), by Isaac Abraham
* [Creating Visual Studio Code Plugins with F# and Fable](http://kcieslak.io/Creating-VS-Code-plugins-with-F-and-Fable/), by Krzysztof Cieslak
* [Kaggle Home Depot Competition Notes: Features](http://brandewinder.com/2016/03/26/kaggle-home-depot-features/), by Mathias Brandewinder
* [Property-Based Testing in the Real World - Or How I Made My Package Manager Suck Less](http://www.navision-blog.de/blog/2016/03/21/property-based-testing-in-the-real-world/), by Steffen Forkmann
* [Sentiment Analysis in F#](https://automatagears.com/articles/sentiment-analysis-in-fsharp/), by Stephen Ireland
* [History and Philosophy of Types](https://www.youtube.com/watch?v=ITIsxWqduE4), by Tomas Petricek
* [A Brief History of Programming Languages](https://www.youtube.com/watch?v=3Z27n_ReTI8), by Andrea Magnorsky

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
* [comment on this gist](https://gist.github.com/bleroy/5a4d55aee8b302bb6ffa)
* Leave us a pointer in the comments section below.
* [Send Stacey (@yecats131) tips on Twitter about .NET games](https://twitter.com/yecats131).

This week's post (and future posts) also contains news I first read on
[ASP.NET's community spotlight](http://www.asp.net/),
on [F# weekly](https://sergeytihon.wordpress.com/category/f-weekly/),
on [ASP.NET Weekly](http://www.aspnetweekly.com/),
and on [Chris Alcock's The Morning Brew](http://themorningbrew.net/).
