The week in .NET - 2/23/2016
============================

To read last week's post, see [The week in .NET - 2/17/2016](https://blogs.msdn.microsoft.com/dotnet/2016/02/17/the-week-in-net-2172016/).

On.NET
------

Last week, [Joe Duffy was on the show](https://www.youtube.com/watch?v=WuqrfuJLbgk) to talk about [Midori](http://joeduffyblog.com/2015/11/03/blogging-about-midori/). This week, because of a cancellation, we don't have a guest yet. We may skip the show for the week, or we may announce who our guest will be later. Stay tuned...

Package of the week: RestSharp
------------------------------

Interacting with REST resources may in theory be just a matter of using standard HTTP, but in practice, there is still ceremony involved around serialization, and authentication. [RestSharp](http://restsharp.org/) is a library that facilitates REST interaction from .NET code.

Here's how you'd query a hypothetical directory service for information about employees:

```csharp
var client = new RestClient("http://microsoft.com");
var request = new RestRequest("people/{alias}");
var alias = "beleroy";
request.AddUrlSegment("alias", alias);
var person = await client.ExecuteGetTaskAsync<Person>(request).Data;
Console.WriteLine($"{alias} stands for {person.FirstName} {person.LastName}.");
```

User group of the week: Seattle Mobile .NET Developers
------------------------------------------------------

On [Wednesday, February 24 at 6:00, our own Stacey Haffner will give an introduction to game programming with Unity](http://www.meetup.com/SeattleMobileDevelopers/events/228554453/) at the [Seattle Mobile .NET Developers](http://www.meetup.com/SeattleMobileDevelopers/) meeting.

.NET
----

* [An update on ASP.NET Core and .NET Core](https://blogs.msdn.microsoft.com/webdev/2016/02/01/an-update-on-asp-net-core-and-net-core/) by Jeffrey T. Fritz.
* [SQLite Code First (GitHub) by Marc Sallin](https://github.com/msallin/SQLiteCodeFirst) is a library that enables EF code first to work with SQLite databases.
* [Async-Friendly Stack Trace (GitHub)](https://github.com/aelij/AsyncFriendlyStackTrace) by Eli Arbel.
* [Moq on .NET Core](http://dotnetliberty.com/index.php/2016/02/22/moq-on-net-core/) by Armen Shimoon.

ASP.NET
-------

* [ASP.NET Core – 2300% More Requests Served Per Second](http://www.ageofascent.com/asp-net-core-exeeds-1-15-million-requests-12-6-gbps/) by Ben Adams.
* [Getting ASP.NET Core running on Ubuntu](http://www.mcrook.com/2016/02/getting-mvc-6-and-net-core-running-on.html) by Michael Crook.
* [ASP.NET Core 1.0 – Create web application using Yeoman and Visual Studio Code](http://www.mithunvp.com/asp-net-core-visual-studio-code-yeoman/) by Mithun Pattankar.
* [How to send email from ASP.NET Core with MailKit](http://stevejgordon.co.uk/how-to-send-emails-in-asp-net-core-1-0)
  by Steve Gordon.
* [Writing custom middleware in ASP.NET Core](http://www.exceptionnotfound.net/writing-custom-middleware-in-asp-net-core-1-0/) by Matthew P Jones.
* [View Components in ASP.NET Core](http://www.mikesdotnetting.com/article/294/view-components-in-asp-net-core-mvc) by Mike Brind.

F#
--

* fsharpConf 2016 is live on Channel 9 on March 4th. [Check out the lineup of speakers!](http://fsharpconf.com/)
* [Ionide and the State of F# Open Source Development](https://www.youtube.com/watch?v=JWe1pOeh84U&feature=youtu.be), by Krzysztof Cieślak.
* [Building Concurrent, Fault-tolerant, Scalable Applications in F# Using Akka.NET](https://www.youtube.com/watch?v=gwWS1e0f-L0&feature=youtu.be), by Riccardo Terrell.
* [Domain Modeling with Types](https://www.youtube.com/watch?v=970nkg60lHs), by Ryan Riley.
* [Managing RabbitMQ Messages with F# and Akka.NET](http://miles.no/blogg/managing-rabbitmq-messages-with-f-and-akkanet), by Vagif Abilov.
* [Benchmarking IEnumerables in F# - Seq.timed](http://latkin.org/blog/2016/02/08/benchmarking-ienumerables-in-f-seq-timed/), by Lincoln Atkinson.
* [Types + Properties = Software: Designing with Types](http://blog.ploeh.dk/2016/02/10/types-properties-software-designing-with-types/), by Mark Seemann.
* [F# Will Solve Your Everyday Problem Without a Headache](http://blog.2mas.xyz/fsharp-will-solve-your-everyday-problem-without-a-headache/), by Tomas Jansson.

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
* [comment on this gist](https://gist.github.com/bleroy/4412310994d1a03255e6)
* Leave us a pointer in the comments section below.
* [Send Stacey (@yecats131) tips on Twitter about .NET games](https://twitter.com/yecats131).

This week's post (and future posts) also contains news I first read on
[ASP.NET's community spotlight](http://www.asp.net/),
on [F# weekly](https://sergeytihon.wordpress.com/category/f-weekly/),
on [ASP.NET Weekly](http://www.aspnetweekly.com/),
on [Dirk Strauss' The Daily Six Pack](http://www.dirkstrauss.com/the-daily-six-pack/),
and on [Chris Alcock's The Morning Brew](http://themorningbrew.net/).
