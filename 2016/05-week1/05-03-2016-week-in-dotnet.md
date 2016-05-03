The week in .NET - 5/3/2016
===========================

To read last week's post, see [The week in .NET – 4/27/2016](https://blogs.msdn.microsoft.com/dotnet/2016/04/26/the-week-in-net-4272016/).

Evolve conference
-----------------

[Xamarin Evolve](https://evolve.xamarin.com/), the largest cross-platform mobile event in the world, happened last week. The .NET team was there to celebrate all things Xamarin with our good friends, and now colleagues. [All the sessions can be watched on YouTube](https://www.youtube.com/watch?v=jgXCB51e4ak&list=PLM75ZaNQS_Fb7I6E9MDnMgwW1GGZIijf_), with [an incredible cast of speakers that includes Steve Wozniak and Grant Imahara](https://evolve.xamarin.com/#speakers).

On.NET
------

Last week on the show, [we spoke with Benjamin Fistein and Jakub Míšek](https://www.youtube.com/watch?v=ZjN9kREzPMs) about [Peachpie, a PHP compiler for .NET built on Roslyn](http://www.peachpie.io/).

Package of the week: Flurl
--------------------------

[Flurl](http://tmenier.github.io/Flurl/fluent-http/) is a fun library that makes it super-easy to query remote HTTP resources. Here's for example how you'd query a remote API with OAuth:

```csharp
dynamic data = await "http://someapi.com/api"
    .WithOAuthBearerToken("myToken")
    .PostUrlEncodedAsync(new {param: 42})
    .ReceiveJson();
```

User group meeting of the week: Seattle - Xamarin Evolve 2016 Redux!
--------------------------------------------------------------------

[Tonight, Tuesday May 3 at 6:00PM, at City University of Seattle, Rich Lander and Frank Krueger will help you catch up on all the amazing stuff that was shown last week in Orlando](http://www.meetup.com/SeattleMobileDevelopers/events/230745445/). The meeting will be hosted by [the Seattle Mobile .NET Developers group](http://www.meetup.com/SeattleMobileDevelopers/).

.NET
----

* [Welcoming Xamarin to the .NET Foundation](http://www.dotnetfoundation.org/blog/welcoming-xamarin-to-the-net-foundation) by the .NET Foundation.
* [News from Xamarin Evolve: What’s next for Visual Studio and Xamarin](https://blogs.msdn.microsoft.com/visualstudio/2016/04/27/xamarin-evolve-whats-next-for-visual-studio-and-xamarin/) by Amanda Silver.
* [The .NET of Tomorrow](http://developer.telerik.com/featured/the-net-of-tomorrow/) by Ed Charbeneau.
* [Exploring Visual Studio "15" Preview and Playing with C# 7](https://blog.cdemi.io/exploring-visual-studio-15-preview-and-playing-with-c-7/) by Christopher Demicoli.
* [Free C# 6 e-book](http://dontcodetired.com/blog/post/Free-eBook-C-60-Whate28099s-New-Quick-Start-Complete.aspx) by Jason Roberts.
* [Thank You For Your Pull Request](http://haacked.com/archive/2016/04/28/thank-you/) by Phil Haack.
* [NoDb - a "no database" file system storage for .NET Core/ASP.NET Core](https://github.com/joeaudette/NoDb) by Joe Audette.
* [Choosing The Right Collection](http://www.codeproject.com/Articles/1095822/Choosing-The-Right-Collection) by Arthur Minduca.
* [An interesting open source C# & VB code editor built on Roslyn and SharpDevelop's Avalon Edit](http://jbe2277.github.io/dotnetpad) by jbe2277.
* [Execute Raw SQL in Entity Framework Core](http://www.elanderson.net/2016/04/execute-raw-sql-in-entity-framework-core/) by Eric L. Anderson.
* [Jenkins C# API Library for Triggering Builds](http://automatetheplanet.com/jenkins-csharp-api-triggering-builds/) by Anton Angelov.
* [Building strongly typed application configuration utility with Roslyn](http://www.strathweb.com/2016/04/building-strongly-typed-application-configuration-utility-with-roslyn/) by Filip W.
* [Anatomy of a Low Impact Visual Studio Install](https://blogs.msdn.microsoft.com/visualstudio/2016/04/25/anatomy-of-a-low-impact-visual-studio-install/) by Art Leonard.
* [Background Tasks in .NET](http://codeopinion.com/background-tasks/) by Derek Comartin.

ASP.NET
-------

* [Learn how to upgrade an ASP.NET Core application from RC1 to RC2 with Damien, Scott, and Jon during last week's ASP.NET community standup (video)](https://blogs.msdn.microsoft.com/webdev/2016/04/28/notes-from-the-asp-net-community-standup-april-26-2016/)
  by Jeffrey T. Fritz.
* [A development workflow with Docker and .NET Core](https://www.jayway.com/2016/04/22/search-effective-workflow-docker-net-core/) by Christian Jacobsen.
* [Fritz's 10 minute tips: DI in ASP.NET Core (video)](http://www.jeffreyfritz.com/2016/04/fritzs-10-minute-tips/) by Jeffrey T. Fritz.
* [Deploying ASP.NET Core RC1 to Azure App Services](https://wildermuth.com/2016/04/29/Deploying-ASP-NET-Core-RC1-to-Azure-App-Services) by Shawn Wildermuth.
* [GitHub authentication with ASP.NET Core (video)](http://aspnetmonsters.com/2016/04/github-authentication-asp-net-core/) by the ASP.NET Monsters.

F#
--

* [F# Survey 2016 results](http://fsharpworks.com/survey.html), by fsharpWorks.
* [Nearly Everything You Ever Wanted to Know About F# Active Patterns but were Afraid to Ask](https://www.youtube.com/watch?v=AQ-8_8hfmGE&feature=youtu.be), by Paulmichael Blasucci.
* [Unfrying Your Brain with F#](http://www.infoq.com/presentations/F-sharp-patterns), by Andrea Magnorsky.
* [Interview: The Good and Bad of Microservices with F#](http://www.infoq.com/interviews/rachel-reese-microservices-fsharp-qcon-london-2016), with Rachel Reese.
* [Make Failure Great Again: A Small Journey into the F# Compiler](http://www.navision-blog.de/blog/2016/04/25/make-failure-great-again-a-small-journey-into-the-f-compiler/), by Steffen Forkmann.
* [Starting Xamarin Android Application Development with F#](http://marisks.net/2016/04/19/starting-xamarin-android-application-development-with-fsharp/), by Māris Krivtežs.

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
* [comment on this gist](https://gist.github.com/bleroy/dba6bddb38bf4ba82c0c489e66cfcbb0)
* Leave us a pointer in the comments section below.
* [Send Stacey (@yecats131) tips on Twitter about .NET games](https://twitter.com/yecats131).

This week's post (and future posts) also contains news I first read on
[The ASP.NET Community Standup](https://blogs.msdn.microsoft.com/webdev/tag/communitystandup/),
on [F# weekly](https://sergeytihon.wordpress.com/category/f-weekly/),
on [ASP.NET Weekly](http://www.aspnetweekly.com/),
and on [Chris Alcock's The Morning Brew](http://themorningbrew.net/).
