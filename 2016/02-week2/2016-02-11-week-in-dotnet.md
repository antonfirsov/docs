The week in .NET - 2/11/2016
============================

This issue of the Week in .NET is slightly late, and I apologize for that.
I was visiting some customers in California for the first half of the week, and they've kept me very busy.
I'm writing this post on the plane to Seattle.
While I was in LA,
[I also visited the LADOTNET user group, where I talked about .NET Core, C# 6, and the future of C#](http://www.meetup.com/LADOTNET/events/228290762/).
You can find links to my slides in the .NET section below.

To read last week's post, see [The week in .NET – 2/2/2016](https://blogs.msdn.microsoft.com/dotnet/2016/02/02/the-week-in-net-222016/).

On.NET
------

Last week on On .NET, [we had Scott Hunter](https://www.youtube.com/watch?v=g2a4W6Q7aRw),
who is the new director of Program Management for .NET, in other words my grand-boss.
Apparently I didn't say anything deserving firing, so we'll be back this week with another show.

We'll be live on [Friday 10AM Pacific Time](https://www.youtube.com/watch?v=BEvn9aI6rd0),
instead of our usual Thursday time.
I'm happy to announce that our guest for this week is Aaron Stannard.
We'll talk about [Akka.NET](http://getakka.net), the actor framework for .NET, and about Aaron's other projects
[NBench](https://github.com/petabridge/NBench) and [DotNetty](https://github.com/Azure/DotNetty).

Package of the week: Polly
--------------------------

In a world of increasingly distributed applications, exception handling is not always the most
convenient way of handling transient errors and the flow associated with them.
For instance, if you're communicating with a distant service, you may want to implement a retry
policy in case it fails.
[Polly](https://github.com/App-vNext/Polly) provides a fluent API that easily expresses such policies.

```csharp
await Policy
  .Handle<TimeoutException>()
  .Or<HttpException>(ex => ex.WebEventCode == WebEventCodes.RuntimeErrorRequestAbort)
  .WaitAndRetryAsync(new[] {1.Seconds(), 5.Seconds()})
  .ExecuteAsync(() => DoSomethingAsync());
```

Tool of the week: DotNetAPIs
----------------------------

[DotNetAPIs](http://dotnetapis.com/) is an extremely impressive web site that acts as an
API documentation aggregator and search engine for a boatload of .NET APIs and libraries.
The way it can be so exhaustive is by analyzing all NuGet packages, and extracting their
built-in XML documentation.
It's a great, and very useful idea.
An essential new tool for all .NET developers.

User group of the week: Baltimore Software Patterns Practice
------------------------------------------------------------

Claudio Sanchez is going to talk at the Baltimore Software Patterns Practice group on
[Tuesday, February 16 at 7:00PM](http://www.meetup.com/Baltimore-Software-Patterns-Practices/events/228542097/)
about Slack`-driven development.

.NET
----

* [Porting to .NET Core](https://blogs.msdn.microsoft.com/dotnet/2016/02/10/porting-to-net-core/)
  by Immo Landwerth.
* [Joe Duffy continues his fascinating series on Midori with a great discussion on error patterns](http://joeduffyblog.com/2016/02/07/the-error-model/).
* [What I've learned about .NET Native](https://blog.rendle.io/what-ive-learned-about-dotnet-native/) by Mark Rendle.
* [.NET Core (LADOTNET presentation)](http://www.slideshare.net/BertrandLeRoy/net-core).
* [C# Today and Tomorrow (LADOTNET presentation)](http://www.slideshare.net/BertrandLeRoy/c-today-and-tomorrow),
  a presentation I shamelessly stole from Mads Torgersen.
* [Learn how to use the Windows Event Log via C#](http://automatetheplanet.com/windows-event-log-tips/)
  by Anton Angelov.
* [Project.json all the things!](https://oren.codes/2016/02/08/project-json-all-the-things/)
  by Oren Novotny.
* [FormatFilter and MediaTypeMappings in ASP.NET Core 1.0 MVC](http://www.strathweb.com/2016/02/formatfilter-and-mediatypemappings-in-asp-net-core-1-0-mvc/)
  by Filip W.

ASP.NET
-------

* [The Ultimate Guide To Unit Testing in ASP.NET MVC](http://www.danylkoweb.com//Blog/the-ultimate-guide-to-unit-testing-in-aspnet-mvc-E2)
  by Jonathan Danylko.
* [A run around the new ASP.NET Data Protection & Authorization Stacks (video)](https://vimeo.com/153102690)
  by Barry Dorrans.
* [Configuring Redis as the ASP.NET Core session store](http://www.hossambarakat.net/2016/02/03/configuring-redis-as-asp-net-core-1-0-session-store/)
  by Hossam Barakat.
* [Release management using VSTS](https://codesnob.wordpress.com/2016/02/04/release-management-using-vsts/)
  by Alton CrossleyASP.NET.
* [A simple authentication library for .NET Core, because sometimes less is more](https://github.com/joeaudette/cloudscribe.Web.SimpleAuth)
  by Joe Audette.
* [Preventing sensistive data exposure in ASP.NET Part 1](http://lockmedown.com/preventing-sensitive-data-exposure-aspnet-part1/)
  and [part 2](http://lockmedown.com/preventing-sensitive-data-exposure-aspnet-part2/)
  by Max R McCarty.
* [Great series on multi-tenancy with ASP.NET MVC](http://benfoster.io/blog/tagged/multi-tenancy)
  by Ben Foster.
* [Inline image tag helper](http://adventuresinwebprogramming.blogspot.co.uk/2016/02/inline-image-taghelper.html)
  by Rich Hosek.

F#
--

* [The Jet Engine We Built in 2015](http://techgroup.jet.com/blog/2016/02-05-the-jet-engine-we-built-in-2015/index.html), by Louie Bacaj.
* [Ten Tips for Productive F# Scripting](http://brandewinder.com/2016/02/06/10-fsharp-scripting-tips/), by Mathias Brandewinder.
* [A Cheatsheet for F#'s DSL-friendly Features](https://github.com/dungpa/dsls-in-action-fsharp/blob/master/DSLCheatsheet.md), by Anh-Dung Phan.
* [Building a Poker Bot: Card Recognition](http://mikhail.io/2016/02/building-a-poker-bot-card-recognition/), by Mikhail Shilkov.
* [How to Keep the Domain Pure When Logic Depends on the Current Date](http://www.taimila.com/blog/fsharp-pure-time-dependent-domain/), by Lauri Taimila.
* [F# for Beginners](https://sachabarbs.wordpress.com/1406-2/), by Sascha Barbs.

Check out [F# Weekly](https://sergeytihon.wordpress.com/category/f-weekly/) for more great content from the F# community.

Games
-----

* [Visual Studio Tools for Unity 2.2](https://blogs.msdn.microsoft.com/visualstudio/2016/02/04/visual-studio-tools-for-unity-2-2/), by Jb Evain.
* [Valve Brings SteamVR to Unity](http://blogs.unity3d.com/2016/02/10/valve-brings-steamvr-to-the-unity-technologies-platform/), by JP Hawkins.
* [Keynote from the Vision Summit 2016](https://www.youtube.com/watch?v=2hYDtxCtzdA).

### Global Game Jam 2016 Submission

Become an explorer who has encountered a small tribe in [Cannibroth](http://globalgamejam.org/2016/games/cannibroth).
The tribe only communicates through dance and you must respond with the proper moves or be tossed into the pot and turned into carrot food! 

<img alt="Cannibroth" src="https://camo.githubusercontent.com/72ad3c8fd24c3634eb665eb5c62dee80c3d12d69/687474703a2f2f62726574687564736f6e2e636f6d2f67616d6573662f696d616765732f63616e6e6962726f74682e706e67" style="width:500px;max-width:100%"/>

And this is it for this week!

Contribute to the week in .NET
------------------------------

As always, this weekly post couldn't exist without community contributions,
and I'd like to thank all those who sent links and tips.
You can participate too. Did you write a great blog post, or just read one?
Do you want everyone to know about an amazing new contribution or a useful library?
We'd love to hear from you, and feature your contributions on future posts:

* Send an email to beleroy at Microsoft,
* [comment on this gist](https://gist.github.com/bleroy/bb2852c686befec4f35a)
* Leave us a pointer in the comments section below.

This week's post (and future posts) also contains news I first read on
[ASP.NET's community spotlight](http://www.asp.net/),
on [F# weekly](https://sergeytihon.wordpress.com/category/f-weekly/),
on [ASP.NET Weekly](http://www.aspnetweekly.com/),
on [Dirk Strauss' The Daily Six Pack](http://www.dirkstrauss.com/the-daily-six-pack/),
and on [Chris Alcock's The Morning Brew](http://themorningbrew.net/).
