The week in .NET - 12/22/2015
=============================

Last week, we had a great time with Miguel de Icaza on [the first episode](https://www.youtube.com/watch?v=6p6-FMZRiAc)
of our new live YouTube show [On.NET](https://www.youtube.com/channel/UCvtT19MZW8dq5Wwfu6B0oxw).
On future episodes of the show, we want to have guests who represent every facet of the .NET community.
If there's somebody you'd like to see on the show, please let us know!

We won't have a show this week or next week because of the holidays, but will resume on the first week of January.
The Week in .NET post series continues uninterrupted.

This week, we're adding a new gaming section to the post.
.NET is increasingly present in the game development community, thanks in no small part to [Unity](http://unity3d.com/).
The new section will showcase games built with .NET, demonstrating one of the coolest area of application
for our favorite platform.

As always, this weekly post couldn't exist without community contributions,
and I'd like to thank all those who sent links and tips.
You can participate too. Did you write a great blog post, or just read one?
Do you want everyone to know about an amazing new contribution or a useful library?
We'd love to hear from you, and feature your contributions on future posts:

* Send an email to beleroy at Microsoft,
* [comment on this gist](https://gist.github.com/bleroy/d3a3e3ca9644d622535d)
* Leave us a pointer in the comments section below.

This week's post (and future posts) also contains news I first read on
[ASP.NET's community spotlight](http://www.asp.net/),
on [F# weekly](https://sergeytihon.wordpress.com/category/f-weekly/),
on [ASP.NET Weekly](http://www.aspnetweekly.com/),
and on [Chris Alcock's The Morning Brew](http://themorningbrew.net/).

Package of the week: Dapper-dot-net
-----------------------------------

You've got a lot of options if you're in the market for a .NET object-relational mapper.
[Dapper](http://stackexchange.github.io/dapper-dot-net/), maintained by the Stack Overflow team,
is one of a few mappers that take the approach of remaining as simple as possible, which enables
it to remain very close to a hand-coded `SqlDataReader` in terms of performance,
while keeping a strongly-typed API.

```csharp
public class Dog
{
    public int Id { get; set; }
    public int? Age { get; set; }
    public string Name { get; set; }
    public float? Weight { get; set; }
}            

var dogs = connection.Query<Dog>("select Age = @Age", new { Age = 4 });

var howManyDogs = dogs.Count();
var firstDogAge = dogs.First().Age;
```

If Dapper wasn't impressive in itself, there is [a whole ecosystem of extensions](https://www.nuget.org/packages?q=dapper)
for it.
It's also one of the top downloads on [nuget.org](https://nuget.org).

User group of the week: Seattle Web App Dev Meetup
--------------------------------------------------

Paul Litwin will speak at the [Seattle Web App Developers Group](http://www.meetup.com/Seattle-Web-App-Developers-Group/)
about [ASP.NET 5 on Thursday, January 14](http://www.meetup.com/Seattle-Web-App-Developers-Group/events/226280408/).

If you want to see what user group meetings are happening in your area, try clicking
[here](http://blogs.msdn.com/b/dotnet/p/dotnetusergroups.aspx)...

If you are a member of a user group, and would like your next event to appear here,
please leave us a note.

.NET News
---------

* [Getting started with .NET and Docker](https://blogs.msdn.microsoft.com/mvpawardprogram/2015/12/15/getting-started-with-net-and-docker/)
  by Elton Stoneman.
* [Raw .NET Data Access / ORM Fetch benchmarks](https://weblogs.asp.net/fbouma/raw-net-data-access-orm-fetch-benchmarks-of-16-dec-2015) by Frans Bouma

.NET Games
----------

For our first gaming section, we are showing some of the games that were built for the
[Ludum Dare](http://ludumdare.com/compo/) programming contest.

* [Rocks, Maps, Scissors](http://ludumdare.com/compo/ludum-dare-34/?action=preview&uid=15279)
  is an interesting modern variation over the classic rock, paper, scissors game.
* [Tile Breaker Evolution](http://ludumdare.com/compo/ludum-dare-34/?action=preview&uid=63289)
  is a super-hard game where you guide a ball by adding obstacles for it to bounce against.
* [Mobsferatu](http://ludumdare.com/compo/ludum-dare-34/?action=preview&uid=59414)
  asks you to gather and guide an angry mob to seek revenge against Nosferatu.
* [Growth Industries](http://ludumdare.com/compo/ludum-dare-34/?action=preview&uid=24965)
  will require a touch-screen (not a mouse, definitely), and a lot of coordination.

C#
--

* [New C# REPL and scripting capabilities](http://bretstateham.com/new-c-repl-and-scripting-capabilities/)
  by Bret Stateham
* [Async Linq to objects over MongoDB](http://blog.i3arnon.com/2015/12/16/async-linq-to-objects-over-mongodb/)
  by Bar Arnon
* Phil Haack asks the super-important question of [to String or to string](http://haacked.com/archive/2015/12/16/to-string-or-not/),
  and boy, did that generate a lot of comments.

F#
--

The F# community is writing a new blog post daily for this year’s [F# Advent Calendar in English](https://sergeytihon.wordpress.com/2015/10/25/f-advent-calendar-in-english-2015/). Lots of great new posts to check out this week!
* [F#, Event Sourcing, and CQRS Tutorial...and Agents](http://blog.2mas.xyz/fsharp-event-sourcing-and-cqrs-tutorial-and-agents/), by Tomas Jansson
* [Building a Hypermedia REST API with F# and Suave.io](http://www.casquete.es/building-an-hypermedia-rest-api-with-fsharp-and-suave-io/), by Alex Casquete
* [The Star Wars Social Network](http://evelinag.com/blog/2015/12-15-star-wars-social-network/index.html#.Vm_sTfl96wU), by Evelina Gabasova
* [Pseudocode-Driven Development with F#](http://stachu.net/blog/post?postId=6), by Stachu Korick
* [Advent of Code F# - Day 16](http://theburningmonk.com/2015/12/advent-of-code-f-day-16/), by Yan Cui
* [A Mixed-Paradigm Recipe for Exposing Native Code](https://pblasucci.wordpress.com/2015/12/15/advent-drm-adt/), by Paulmichael Blasucci
* [1729](http://kunjan.in/2015/12/1729/), by Kunjan Dalal
* [Ukulele Fun for Xmas!](http://thinkbeforecoding.com/post/2015/12/17/Ukulele-Fun-for-XMas-%21), by Jérémie Chassaing
* [Using F# for Scientific Instrument Control](https://medium.com/@ant_pt/using-f-for-scientific-instrument-control-b1ef04d20da0#.pmmatrcuk), by Anton Tcholakov
* [REST vs CQRS: The Trigger Problem](http://hawkins6423.github.io/), by Matt Hawkins
* [Angels from the Realms of Glory](http://blog.mavnn.co.uk/angels-from-the-realms-of-glory/), by Michael Newton
* [Let it Snow! A Basic Particle System in F# and WPF](http://stevenpemberton.net/blog/2015/12/19/Let-it-snow-FSharp-Advent-2015/), by Steven Pemberton
* [Developing Mobile Apps at the Speed of Light](http://jmgomez.me/advent-calendar-developing-mobile-apps-at-the-spee/), by Juan Gómez
* [Reactive Messaging Patterns with F# and Akka.NET](http://jorgef.github.io/fsharpreactivepatterns/), by Jorge Fioranelli
 
Check out [F# Weekly](https://sergeytihon.wordpress.com/category/f-weekly/) for more great content from the F# community.

ASP.NET
-------

* [Tag Helpers in ASP.NET MVC 6](https://blog.mariusschulz.com/2015/12/14/tag-helpers-in-asp-net-mvc-6), by Marius Schulz
* [ASP.NET 5 MVC 6 API Documentation using Swashbuckle Swagger](http://damienbod.com/2015/12/13/asp-net-5-mvc-6-api-documentation-using-swagger/), by damienbod
* [What I Learned After A Week of Visual Studio Code and ASP.NET](http://www.khalidabuhakmeh.com/what-i-learned-after-a-week-of-visual-studio-code-and-asp-net-5), by Khalid Abuhakmeh
* [Disabling cryptographic protocols for PCI compliance](http://johnlouros.com/blog/disabling-cryptographic-protocols-for-pci-compliance)
  by John Louros
* Many ASP.NET web sites are using Bootstrap, so now is probably a great time to start learning about
  [Bootstrap 4](http://www.developerdrive.com/2015/12/what-you-need-to-know-about-bootstrap-4/).

And this is it for this week!