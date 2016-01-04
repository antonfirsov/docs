The week in .NET - 1/5/2015
===========================

We're back for a new year of .NET!
For this first week of 2016, we're off to a great start with two packages and lots
of interesting posts.

As always, this weekly post couldn't exist without community contributions,
and I'd like to thank all those who sent links and tips.
You can participate too. Did you write a great blog post, or just read one?
Do you want everyone to know about an amazing new contribution or a useful library?
We'd love to hear from you, and feature your contributions on future posts:

* Send an email to beleroy at Microsoft,
* [comment on this gist](https://gist.github.com/bleroy/b509d5071434ae58c9e2)
* Leave us a pointer in the comments section below.

This week's post (and future posts) also contains news I first read on
[ASP.NET's community spotlight](http://www.asp.net/),
on [F# weekly](https://sergeytihon.wordpress.com/category/f-weekly/),
on [ASP.NET Weekly](http://www.aspnetweekly.com/),
and on [Chris Alcock's The Morning Brew](http://themorningbrew.net/).

To read last week's post, see [The week in .NET - 12/29/2015](http://blogs.msdn.com/b/dotnet/archive/2015/12/29/the-week-in-net-12-29-2015.aspx).

Package of the week #1: ReactiveUI
----------------------------------

ReactiveUI uses [Reactive Extensions](http://reactivex.io/) for .NET to create testable UI
that are compatible with Xamarin (iOS, Android, and Mac), WPF, Windows Forms, UWP, etc.

Here's an example that illustrates how Reactive programming is very appropriate for UI, by
showing how to throttle change events on a search textbox before sending queries to a search
service:

```csharp
WhenAnyValue(x => x.SearchQuery)
    .Throttle(TimeSpan.FromSeconds(1), RxApp.MainThreadScheduler)
    .InvokeCommand(this, x => x.Search);
```

You can see this sample code in context on [the ReactiveUI site](http://reactiveui.net/).

The ReactiveUI community is very open to contributions: you can check out
[what's up for grabs](https://github.com/reactiveui/ReactiveUI/labels/up-for-grabs),
and they also have
[a tag for first-timer only issues](https://github.com/reactiveui/ReactiveUI/labels/first-timers-only).

Package of the week #2: TypedRouting
------------------------------------

If you think declaring ASP.NET MVC routes involves too many "magic strings", you're going
to love [TypedRouting](https://github.com/ivaylokenov/AspNet.Mvc.TypedRouting).

Here's how you would declare a `GET` route to `HomeController.Index(int)`:

```csharp
routes.Get("MyRoute/{id}", route => route.ToAction<HomeController>(a => a.Index(With.Any<int>())));
```

In this example, both the controller name and the action method names are checked at compile-time.
Even the signature of the method can be checked during compilation, and will be flagged as an error
if it changes and becomes incompatible with its usage.

The library doesn't stop at adding routes, and also provides a range of utilities to use the routes.
For example, you can get a link using the route defined above:

```csharp
urlHelper.Action<HomeController>(c => c.Index(1));
```

User group of the week: New England Microsoft Developers
--------------------------------------------------------

This week, the [New England Microsoft Developers group](http://www.meetup.com/NE-MSFT-Devs/)
has John Pelak talking about
[Azure ML - machine learning for all of us](http://www.meetup.com/NE-MSFT-Devs/events/227682400/),
on Thursday, January 7, from 6:30PM to 8:30PM, in Burlington, MA.

.NET
----

* [Fast shared array, buffer, and ring buffer for .NET IPC with memory mapped files](http://spazzarama.com/2015/12/31/fast-shared-array-buffer-and-circular-buffer-ring-buffer-for-dotnet-ipc-with-memory-mapped-files/)
  by Justin Stenning.
* [Introducing NBench - an automated performance testing framework](https://petabridge.com/blog/introduction-to-nbench/)
  by Petabridge.
* [Do one thing and do it well](https://blog.rendle.io/do-one-thing-and-do-it-well/) by Mark Rendle.

ASP.NET
-------

* [Complex custom tag helpers in MVC 6](http://www.davepaquette.com/archive/2015/12/28/complex-custom-tag-helpers-in-mvc-6.aspx)
  by Dave Paquette.
* [How to unit test ASP.NET MVC 6 ModelState](http://dotnetliberty.com/index.php/2016/01/04/how-to-unit-test-asp-net-5-mvc-6-modelstate/),
  and [Fast ASP.NET Integration](http://dotnetliberty.com/index.php/2015/12/31/fast-asp-net-5-integration-testing-with-xunit/)
  by Armen Shimoon.
  
F#
--

Start your New Year off right with the epic conclusion of the 2015 [F# Advent Calendar in English](https://sergeytihon.wordpress.com/2015/10/25/f-advent-calendar-in-english-2015/):
* [Welcome to 2016 - A Call to Action](http://foundation.fsharp.org/call_to_action), by The F# Software Foundation.
* [Visualizing F# Advent Calendar Contributors](http://www.pirrmann.net/visualizing-f-advent-calendar-contributors/), by Pierre Irrmann.
* [Implementing API Gateway in F# Using Rx and Suave](http://blog.tamizhvendan.in/blog/2015/12/29/implementing-api-gateway-in-f-number-using-rx-and-suave/), by Tamizh Vendan.
* [Happy New Year 2016 Around the World](http://tomasp.net/blog/2015/happy-new-year-tweets/), by Tomas Petricek.

Check out [F# Weekly](https://sergeytihon.wordpress.com/category/f-weekly/) for more great content from the F# community.

And this is it for this week!