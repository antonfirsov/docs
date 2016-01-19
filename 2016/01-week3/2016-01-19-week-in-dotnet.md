The week in .NET - 1/19/2016
============================

To read last week's post, see [The week in .NET - 1/12/2016](http://blogs.msdn.com/b/dotnet/archive/2016/01/12/the-week-in-net-1-12-2015.aspx).

On.NET
------

Last week, we had [Jonathan Chambers on the show to talk about Unity](https://www.youtube.com/watch?v=B0yWmVL8hF0).
This week, we'll have [Don Syme](https://en.wikipedia.org/wiki/Don_Syme),
and [we'll talk about F#](https://www.youtube.com/watch?v=aWnmzrCvTbg).

Package of the week #1: NATS client
-----------------------------------

Microservices and IoT both require distributed architectures where a large number of endpoints
communicate fast and reliably.
NATS is a popular high-performance cloud-based messaging system that is an excellent fit for such scenarios.
The .NET client for NATS can publish over 3 million messages per second (and that's in a VM, on a developer laptop).

Here's how you'd send a simple object on the message bus, with a subject “foo”:

```csharp
using (var cnx = new ConnectionFactory().CreateEncodedConnection())
{
    cnx.Publish("foo", new Company
    {
        Name = "Apcera",
        Address = "140 New Montgomery St."
    });
}
```

Other actors can asynchronously subscribe to the same subject and process objects:

```csharp
using (var cnx = new ConnectionFactory().CreateEncodedConnection()) {
    using (cnx.SubscribeAsync("foo", (sender, args) => {
        var company = (Company)args.ReceivedObject;
        Console.WriteLine($"Name: {company.Name}, Address: {company.Address}");
    })) {
        System.Console.WriteLine("Waiting for a message...");
        Thread.Sleep(5000);
    }
}
```

You can read more about the NATS .NET client in [NATS In Microsoft .NET](http://nats.io/blog/nats-in-dotnet/).

Package of the week #2: VerbalExpressions
-----------------------------------------

Regular expressions are hard to write, read, and debug: they're a terse, but powerful DSL.
VerbalExpressions is a library that makes it possible to write regular expressions using
a more expressive syntax:

```csharp
var verbEx = new VerbalExpressions()
    .StartOfLine()
    .Then("http")
    .Maybe("s")
    .Then("://")
    .Maybe("www.")
    .AnythingBut(" ")
    .EndOfLine();

Assert.IsTrue(verbEx.Test("https://www.microsoft.com"));
```

You are still using and running regular expressions, but your code is a lot easier to understand.

VS plugin of the week: Alive
----------------------------

[Alive](https://comealive.io/) is a very cool Visual Studio extension that lets you visualize the effects
of your code as you are typing it.

<iframe width="560" height="315" src="https://www.youtube-nocookie.com/embed/PZni_54s0o4?rel=0" frameborder="0" allowfullscreen></iframe>

User group of the week: dotNet Miami
------------------------------------

Cecil Phillip is talking at the
[dotNet Miami user group on Thursday, January 21 at 6:30PM](http://communitymegaphone.com/ShowEvent.aspx?EventID=7411)
about going beyond dependency injection with Autofac.
The second talk of the night will be Camilio Sanchez on stress-free API integration.

.NET
----

* [Learn Roslyn now: the Emit API](https://joshvarty.wordpress.com/2016/01/16/learn-roslyn-now-part-16-the-emit-api/)
  by Josh Varty.
* [NBench performance testing code throughput](http://www.dotnetalgorithms.com/2016/01/nbench-performance-testing-code-throughput/)
  by Andrea Angella.
* Sébastien Ros built [a nice little VS extension](https://github.com/sebastienros/vsconstructorfield/releases)
  to help with the boilerplate associated with constructor dependency injection.
* [How to optimize JSON.NET serialization performance](http://www.tomdupont.net/2016/01/how-to-optimize-jsonnet-serialization.html)
  by Tom DuPont.
* [.NET method inlining and loops](http://www.codeproject.com/Tips/1072041/NET-Methods-Inlining-and-Loops)
  by Dmitry Orzhevsky.

ASP.NET
-------

* [What's New with ASP.NET MVC 6](https://rewards.msdn.microsoft.com/Challenge/804434af-9658-4ff1-8746-76fcfdf27132)
  by Ugo Lattanzi (video)
* [Using an ASP.NET module to debug async calls](http://blogs.msdn.com/b/webdev/archive/2015/12/29/using-asp-net-module-to-debug-async-calls.aspx)
  by Xing (Shin) Mao.
* [Setting up ASP.NET v5 (vNext) to use JWT tokens (using OpenIddict)](http://capesean.co.za/blog/asp-net-5-jwt-tokens/)
  by Capesean. [JWT Tokens](https://jwt.io/) are an open industry standard RFC 7519 method for representing
  claims securely between two parties.
* [Configuring SQL Server for session state in MVC 6](http://www.mikesdotnetting.com/Article/292/configuring-sql-server-for-session-state-in-mvc-6)
  by Mike Brind.
* [Develop ReactJS + ASP.NET Web API apps in Visual Studio 2015](http://blogs.taiga.nl/martijn/2015/12/10/develop-reactjs-asp-net-web-api-apps-in-visual-studio-2015/)
  by Martijn Boland
* [ASP.NET 5 MVC 6 file upload with Sql Server FileTable](http://damienbod.com/2015/12/05/asp-net-5-mvc-6-file-upload-with-ms-sql-server-filetable/)
  by Damien Bod
* [Running the KestrelHttpServer on Linux with CoreCLR](http://mikehadlow.blogspot.co.uk/2016/01/running-kestrelhttpserver-on-linux-with.html)
  by Mike Hadlow.

F#
--

* The .NET Core Visual F# compiler [can now bootstrap the open source F# compiler](https://github.com/Microsoft/visualfsharp/pull/850).
* Erico Sada [added preliminary support](https://twitter.com/VisualFSharp/status/684470596810358785) for F# in the new [dotnet CLI](https://github.com/dotnet/cli).
* Don Syme is working on [merging part of the F# Compiler Service](https://github.com/Microsoft/visualfsharp/pull/853) into the Visual F# compiler repo.
* [Suave](https://suave.io/), a lightweight F# web development library, has reached [v1.0](https://github.com/SuaveIO/suave/releases/tag/v1.0.0).
* [Rachel Reese discusses building functional microservices](https://www.dotnetrocks.com/?show=1240) on the .NET Rocks podcast.
* [Building Concurrent, Fault-tolerant, Scalable Applications in F# with Akka.NET](https://www.youtube.com/watch?v=gwWS1e0f-L0&list=PLE7tQUdRKcybh21_zOg8_y4f2oMKDHpUS&index=20), by Riccardo Terrell.
* [A Developer's Journey from OO to Functional](http://reidev275.github.io/ReducingDeveloperFriction/#/), by Reid Evans.
* [Designing with Capabilities](http://fsharpforfunandprofit.com/cap/), by Scott Wlaschin.
* [Freeing Your Azure Data with F# Type Providers](https://blogs.msdn.microsoft.com/mvpawardprogram/2016/01/05/freeing-your-azure-data-with-f-type-providers/), by Isaac Abraham.

Check out [F# Weekly](https://sergeytihon.wordpress.com/category/f-weekly/) for more great content from the F# community.

And this is it for this week!

Contribute to the week in .NET
------------------------------

As always, this weekly post couldn't exist without community contributions,
and I'd like to thank all those who sent links and tips.
You can participate too. Did you write a great blog post, or just read one?
Do you want everyone to know about an amazing new contribution or a useful library?
We'd love to hear from you, and feature your contributions on future posts:

* Send an email to beleroy at Microsoft,
* [comment on this gist](https://gist.github.com/bleroy/79460b60dde3fdf87adf)
* Leave us a pointer in the comments section below.

This week's post (and future posts) also contains news I first read on
[ASP.NET's community spotlight](http://www.asp.net/),
on [F# weekly](https://sergeytihon.wordpress.com/category/f-weekly/),
on [ASP.NET Weekly](http://www.aspnetweekly.com/),
on [Dirk Strauss' The Daily Six Pack](http://www.dirkstrauss.com/the-daily-six-pack/),
and on [Chris Alcock's The Morning Brew](http://themorningbrew.net/).
