The week in .NET - 2/17/2016
============================

To read last week's post, see [The week in .NET 2/11/2016](https://blogs.msdn.microsoft.com/dotnet/2016/02/11/the-week-in-net-2112016/).

On.NET
------

Last week, [we had Aaron Stannard on the show](https://www.youtube.com/watch?v=BEvn9aI6rd0), to talk about Akka.NET.
This week, I'm looking forward to [a chat with Joe Duffy about Midori](https://www.youtube.com/watch?v=RP26WWEqROg). 

Packages of the week: Scientist ports
-------------------------------------

No matter how carefully you are testing a refactoring, it's hard to be 100% sure your changes will work against real data and workloads until you put them in production.
[Scientist](https://github.com/github/scientist) is a very neat Ruby library built by GitHub that makes it possible to deploy refactored code alongside existing logic for the same task, run both, then log and compare the results.
This reduces the risk of deploying new code, because the old code is still running, and you can decide exactly what to do if results are inconsistent.
You also gather real-world production data about the new code without the risk of fully deploying it.

There are at least two ports of Scientist for .NET that are being worked on at the moment.
The first is a semi-official project that [Phil Haack](http://haacked.com/) from GitHub is working on: [Scientist.NET](https://github.com/haacked/scientist.net).
The second seems a litle more complete at this point in time: [Dave Zych's Schience](https://github.com/davezych/shience).

Both are still very early, but they are definitely projects to watch, and, why not, contribute to.

```csharp
var publisher = new FilePublisher(@"C:\file\path\to\results.log");

var userCanRead = Shience.New<bool>("widget-permissions")
    .Test(control: () => UserPermissions.CheckUser(currentUser), 
          candidate: () => User.Can(currentUser, Permission.Read))
    .PublishTo(publisher.Publish)
    .Execute();
```

User group of the week: Barcelona .NET Core
-------------------------------------------

Tonight Wednesday, February 17 at 6:45PM in Barcelona, join the new Barcelona .NET Core group for [an evening of demos, code, and fun](http://www.meetup.com/SeattleMobileDevelopers/events/228554453/).

.NET
----

* [A (Hitchhiker's) Guide To The .NET Core Projects on GitHub](https://blog.rendle.io/a-guide-to-the-net-projects-on-github/) by Mark Rendle.
* [Run dotnet CLI on unsupported Linux distros with docknet](https://blog.rendle.io/run-dotnet-cli-on-unsupported-linux-distros-with-docknet/) by Mark Rendle.
* [.NET Core: Introduction To Microsoft.Data.Sqlite](http://www.c-sharpcorner.com/UploadFile/ranjancse/net-co-introduction-to-microsoft-data-sqlite/) by Ranjan Dailata.
* [Weak events in .NET using Reactive Extensions](http://www.codeproject.com/Tips/1078183/Weak-events-in-NET-using-Reactive-Extensions-Rx) by Kenneth Haugland.

ASP.NET
-------

* [Our Major Minor - introducing Umbraco 7.4](http://umbraco.com/follow-us/blog-archive/2016/2/11/our-major-minor-introducing-umbraco-74/) by Niels Hartvig.
* [RESTful Web API Help Documentation using Swagger UI and Swashbuckle](http://www.codeproject.com/Articles/1078249/RESTful-Web-API-Help-Documentation-using-Swagger-U) by Sreekanth Mothukuru.
* [ASP.NET WebHooks and Slack Slash Commands](https://blogs.msdn.microsoft.com/webdev/2016/02/14/asp-net-webhooks-and-slack-slash-commands/) by Henrik F Nielsen. 
* [Authoring ASP.NET Core MVC Tag Helper](http://www.hossambarakat.net/2016/02/15/authoring-asp-net-core-mvc-tag-helper/) by Hossam Barakat.
* [Authorization policies and data protection with Identity Server 4 in ASP.NET Core](http://damienbod.com/2016/02/14/authorization-policies-and-data-protection-with-identityserver4-in-asp-net-core/) by Damien Bod.
* [ServiceStack and Razor Forms](https://visualstudiomagazine.com/articles/2016/02/01/servicestack-and-razor-forms.aspx) by Patrick Steele.
* [ASP.NET Core Identity Token Providers – Under the Hood](http://stevejgordon.co.uk/asp-net-core-identity-token-providers) by Steve Gordon.
* [AppVeyor and ASP.NET Core](http://shazwazza.com/post/appveyor-and-aspnet-core/) by Shazwazza.

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
* [comment on this gist](https://gist.github.com/bleroy/f240aa68c8489b5352e2)
* Leave us a pointer in the comments section below.
* [Send Stacey (@yecats131) tips on Twitter about .NET games](https://twitter.com/yecats131).

This week's post (and future posts) also contains news I first read on
[ASP.NET's community spotlight](http://www.asp.net/),
on [F# weekly](https://sergeytihon.wordpress.com/category/f-weekly/),
on [ASP.NET Weekly](http://www.aspnetweekly.com/),
on [Dirk Strauss' The Daily Six Pack](http://www.dirkstrauss.com/the-daily-six-pack/),
and on [Chris Alcock's The Morning Brew](http://themorningbrew.net/).
