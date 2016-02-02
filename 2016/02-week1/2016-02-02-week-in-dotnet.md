The week in .NET - 2/2/2016
===========================

To read last week's post, see [The week in .NET - 1/25/2016](http://blogs.msdn.com/b/dotnet/archive/2016/01/26/the-week-in-net-1-25-2016.aspx).

On.NET
------

On .NET can now be [enjoyed on Channel 9](https://channel9.msdn.com/Shows/On-NET),
in addition to [our YouTube channel](https://www.youtube.com/channel/UCvtT19MZW8dq5Wwfu6B0oxw).
A nice consequence of this, beyond the additional audience, is that we're getting
[a nice audio podcast feed](https://s.ch9.ms/Shows/On-NET/feed/mp3) out of it, which
should be useful to all of you who prefer to enjoy the show at times when you can't stare at
a screen, for example during your commute.

Last week, we had [Brian Flannery and Colin Sullivan from Apcera on the show, to talk about NATS, a high performance messaging system with a great .NET client](https://youtu.be/h3x6eY0RAr4).
You can learn more about NATS on [the NATS web site](http://nats.io).

This week, [we'll have the great pleasure of having Scott Hunter on the show](https://www.youtube.com/watch?v=g2a4W6Q7aRw).
We'll be talking about .NET, ASP.NET, shipping Core, and more...
Please note the slightly different schedule for this episode of the show: we'll be live at 11:00AM Pacific Time, instead of our usual 10:00AM.

Package of the week: AutoMapper
-------------------------------

[AutoMapper](https://github.com/AutoMapper/AutoMapper) is a simple library that makes it
trivially easy to map data between object shapes.
It is, as its author Jimmy Bogard describes it, an object to object mapper.
This is especially useful on layer boundaries, such as the UI/Domain, or the Service/Domain
boundaries.

```csharp
var config = new MapperConfiguration(cfg => cfg.CreateMap<Order, OrderDto>());
var mapper = config.CreateMapper();
OrderDto dto = mapper.Map<OrderDto>(order);
```

AutoMapper can be configured to recognize common conventions, and can use custom converters.

Tool of the week: Cake
----------------------

MS Build isn't the only way to build large .NET projects.
An alternative build automation system that should appeal to .NET developers is [Cake](http://cakebuild.net/blog/2016/01/cake-v0-8-0-released).
Cake is cross-platform, but what really sets it apart is that its build scripts are written in C#.

<img alt="Cake is a cross-platform build automation system that you script using C#." src="http://cakebuild.net/Content/img/screenshot.png" style="width:500px;max-width:100%;"/>

[Version 0.8 of Cake was just released](http://cakebuild.net/blog/2016/01/cake-v0-8-0-released).

User group of the week: New England Microsoft Developers
--------------------------------------------------------

This week, I could do some shameless auto-promotion and point you to
[my appearance at the LA .NET developers group](http://www.meetup.com/LADOTNET/events/228290762/),
but the event is sold-out, so instead I'll point to
[John Miner's talk about PowerBI at the New England Microsoft Developers meeting on Thursday, February 4, in Burlington, MA](http://www.meetup.com/NE-MSFT-Devs/events/228190990/).

.NET
----

* [.NET Framework 4.6.1 is available on Windows Update](http://blogs.msdn.com/b/dotnet/archive/2016/01/26/microsoft-net-framework-4-6-1-is-available-on-windows-update-and-wsus.aspx).
* [A brief look at the .NET portability analyzer (video)](https://channel9.msdn.com/Blogs/Seth-Juarez/A-Brief-Look-at-the-NET-Portability-Analyzer).
* [Interactive coding with C# and F# REPLs](http://www.hanselman.com/blog/InteractiveCodingWithCAndFREPLsScriptCSOrTheVisualStudioInteractiveWindow.aspx)
  by Scott Hanselman.
* [How (and why) to lobby companies to support .NET OSS](http://seankilleen.com/2016/01/how-and-why-to-lobby-for-oss/)
  by Sean Killeen.
* [What I've learned about .NET Native](https://blog.rendle.io/what-ive-learned-about-dotnet-native/)
  by Mark Rendle.
* [NBench Testing – Memory Allocations](http://www.dotnetalgorithms.com/2016/01/nbench-testing-memory-allocations/)
  by Andrea Angella.
* [Porting Microbus to .NET Core](http://www.lavinski.me/porting-microbus-to-dotnetcore/)
  by Daniel Little.
* [Generic resource leak detection with ETW and EasyHook](http://geekswithblogs.net/akraus1/archive/2016/01/30/172079.aspx)
  by Alois Kraus.

ASP.NET
-------

* Martin Kramer shows a really neat trick that I had no idea was possible in
  [JavaScript debugging in Visual Studio with Chrome](http://lostindetails.com/blog/post/JavaScript-debugging-in-VisualStudio-with-Chrome).
* [Understanding the new ASP.NET Core configuration in startup.cs](http://mikemengell.com/asp-net5/understanding-the-new-asp-net-5-configuration-in-startup-cs/)
  by Mike Mengell.
* [NGINX Reverse Proxy and Load Balancing for ASP.NET 5 Applications with Docker Compose](http://www.tugberkugurlu.com/archive/nginx-reverse-proxy-and-load-balancing-for-asp-net-5-applications-with-docker-compose)
  by Tugberk Ugurlu.
* [Isolated ASP.NET attribute routing](http://shazwazza.com/post/isolated-aspnet-attribute-routing/)
  by Shannon Deminick.
* [A practical approach to cache busting with Webpack and ASP.NET Core](http://scottaddie.com/2015/12/14/a-practical-approach-to-cache-busting-with-webpack-and-asp-net-5/)
  by Scott Addie.
* [Using subdomains in ASP.NET MVC](http://www.danylkoweb.com/Blog/using-subdomains-in-aspnet-mvc-DX)
  by Jonathan Danylko.
* [ASP.NET Core 1.0 using SQL localization](http://damienbod.com/2016/01/29/asp-net-core-1-0-using-sql-localization/)
  by Damien Bod.
* [Inline images in ASP.NET Core](https://weblogs.asp.net/ricardoperes/inline-images-in-asp-net-mvc-core)
  by Ricardo Peres.

F#
--

* [Microservices & Messaging](http://techgroup.jet.com/blog/2016/01-26-microservices-messaging/) at Jet, by Krishna Vangapandu
* [Continuously writing an iPhone app, on an iPad Pro, using F#](https://www.youtube.com/watch?v=bbSawlDetOU&feature=youtu.be), by Frank Krueger
* [Star Wars social networks: The Force Awakens](http://evelinag.com/blog/2016/01-25-social-network-force-awakens/index.html), by Evelina Gabasova
* [Interview with Henrik Feldt on Suave 1.0](http://www.infoq.com/news/2016/01/suave-interview), by Pierre-Luc Maheu
* [Building a random art bot in F#](http://theburningmonk.com/2016/01/building-a-random-arts-bot-in-fsharp/), by Yan Cui
* [Discussion on the Visual F# Github](https://github.com/Microsoft/visualfsharp/issues/913) about how to improve the Visual Studio F# editor with Roslyn Workspaces 
* [Meeting notes](http://foundation.fsharp.org/board_meeting_20151214) from the most recent F# Software Foundation board meeting

Check out [F# Weekly](https://sergeytihon.wordpress.com/category/f-weekly/) for more great content from the F# community.

Games
-----

* [Unity comes to New Nintendo DS](http://blogs.unity3d.com/2016/01/29/unity-comes-to-new-nintendo-3ds/)
  by Andrew Innes.

### Global Game Jam 2016 Submission

In [Oh God, it's Monday](http://globalgamejam.org/2016/games/oh-god-its-monday), players must find the most efficient routes for the employees to complete their tasks before time runs out for the day. If any employees run into each other during the day they will stop and talk - causing their tasks to not be completed and the round to fail. 
![02_4x3_0](https://cloud.githubusercontent.com/assets/4108756/12756896/9cfa9178-c98a-11e5-99f7-b8bf0b885eb0.jpg)

And this is it for this week!

Contribute to the week in .NET
------------------------------

As always, this weekly post couldn't exist without community contributions,
and I'd like to thank all those who sent links and tips.
You can participate too. Did you write a great blog post, or just read one?
Do you want everyone to know about an amazing new contribution or a useful library?
We'd love to hear from you, and feature your contributions on future posts:

* Send an email to beleroy at Microsoft,
* [comment on this gist](https://gist.github.com/bleroy/d24fedefa24f34c9eb3b)
* Leave us a pointer in the comments section below.

This week's post (and future posts) also contains news I first read on
[ASP.NET's community spotlight](http://www.asp.net/),
on [F# weekly](https://sergeytihon.wordpress.com/category/f-weekly/),
on [ASP.NET Weekly](http://www.aspnetweekly.com/),
on [Dirk Strauss' The Daily Six Pack](http://www.dirkstrauss.com/the-daily-six-pack/),
and on [Chris Alcock's The Morning Brew](http://themorningbrew.net/).
