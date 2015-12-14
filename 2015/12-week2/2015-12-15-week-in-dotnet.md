The week in .NET - 12/15/2015
=============================

We have some great contents and news for you this week.
I can't emphasize enough that this weekly post couldn't exist without community contributions,
and I'd like to thank all those who sent links and tips.
Keep them coming!
In a way, this is an open source blog post series ;)

The first piece of news I want to share is that the posts are being translated to Korean
by [Euni Kim](https://www.facebook.com/jugan.net).
The first post can be found [here](http://blogs.msdn.com/b/eva/archive/2015/12/11/2015-12-1.aspx).
If you'd like to do the same and translate the posts to your language, and add news from your
local communities, you are more than welcome to do so.
Please drop me a line, and I'll link to your translations from here.

The second news is that we're also starting a live YouTube show that we're calling
"[On.NET](https://www.youtube.com/channel/UCvtT19MZW8dq5Wwfu6B0oxw)",
where we'll discuss current .NET events and topics with a guest every week.
Our first guest will be [Miguel de Icaza](https://en.wikipedia.org/wiki/Miguel_de_Icaza),
creator of Gnome, Mono, founder of [Xamarin](https://xamarin.com),
recipient of the FSF Award for the Advancement of Free Software, MIT Innovator of the Year 1999,
and one of Time magazine's top 100 innovators of the 21st century. Wow.

You can join the live stream on Thursday, December 17, at 10AM Pacific Time, 1PM Eastern Time at
this address: <https://www.youtube.com/watch?v=6p6-FMZRiAc>

You can also subscribe to the channel here: <https://www.youtube.com/channel/UCvtT19MZW8dq5Wwfu6B0oxw>

If you have questions for Miguel, you'll be able to ask them during the event, or you can
send them to me in advance at beleroy at microsoft.

As always, huge thank yous to all who sent messages of encouragement and contributions.
If you would like to participate too, if you wrote a great blog post, or just read one,
if you want to show an amazing new contribution, if you've written a useful library,
we'd love to hear from you, and feature it on future posts.
You can send me an email to beleroy at Microsoft,
or you can [comment on this gist](https://gist.github.com/bleroy/5360f02aaad026eb0b7e) with new
links, or you can simply leave us a pointer in the comment section below.

This week's post (and future posts) also contains news I first read on
[ASP.NET's community spotlight](http://www.asp.net/),
and from [Chris Alcock's The Morning Brew](http://themorningbrew.net/),
two other great sources for .NET news.

Package of the week #1: {m}brace the cloud
------------------------------------------

[MBrace](http://mbrace.io/) is a little more than a library: it's a DSL for F# that integrates
cloud programming into the language.
I like to think about it as async/await for the cloud.

Here's a small sample that reads from a queue in the cloud:

<script src="https://gist.github.com/bleroy/926452c406d2fcff327a.js"></script>

You can see this and other samples in context in this tutorial:
[Using Cloud Queues](http://mbrace.io/starterkit/HandsOnTutorial/8-using-cloud-queues.html)

Package of the week #2: Humanizer
---------------------------------

[Dannief](https://gist.github.com/dannief) sent a tip about [Humanizer](https://github.com/Humanizr/Humanizer),
a library with dozens of methods that translate strings and objects back and forth between
computer representations and all the messy, messy representations of human languages.

For example, here's how you can truncate a string while respecting word boundaries:

```csharp
"Long text to truncate".Truncate(10)
```

This code will output the string `"Long text…"`.

Another, more elaborate example, is this bit of code that produces a human-readable list from
a collection of objects:

<script src="https://gist.github.com/bleroy/66c03b69e5678376810c.js"></script>

User group of the week: Boston Mobile C# Developers
---------------------------------------------------

The [Boston Mobile C# Developers](http://www.meetup.com/bostonmobiledev/) group has a
[Xamarin 4 event](http://www.meetup.com/bostonmobiledev/events/226979784/)
on Thursday, December 17, at 6:00PM where you will learn how to build great native
iOS and Android apps with C#.

If you want to see what user group meetings are happening in your area, try clicking
[here](http://blogs.msdn.com/b/dotnet/p/dotnetusergroups.aspx)...

.NET News
---------

* Omar Khudeira from Pagea Engineering makes a great case for
  [Why .NET?](http://engineering.gopangea.com/2015/12/10/why-dot-net.html).
* Ron Petrusha recaps this year of
  [making .NET open source](http://blogs.msdn.com/b/visualstudio/archive/2015/12/10/the-net-journey-recapping-the-last-year.aspx),
  and so does Matt Warren from an external perspective in
  [Open Source .NET – 1 year later](http://mattwarren.org/2015/12/08/open-source-net-1-year-later/).
* David Ebbo shows how to
  [call the Azure ARM API using plain REST](http://blog.davidebbo.com/2015/12/calling-arm-using-plain-rest.html).
* The ever-interesting Eric Lippert has a great series about arbitrary precision mathematical
  types in [The dedoublifier, part one](http://ericlippert.com/2015/11/30/the-dedoublifier-part-one/),
  [part 2](http://ericlippert.com/2015/12/03/the-dedoublifier-part-two/),
  [part 3](http://ericlippert.com/2015/12/07/the-dedoublifier-part-three/),
  and [part 4](http://ericlippert.com/2015/12/10/the-dedoublifier-part-four/).

ASP.NET 5
---------

* Mahesh Sabnis shows
  [how to build an ASP.NET MVC 6 & EF 7 application](http://www.dotnetcurry.com/aspnet-mvc/1215/building-aspnet-mvc-6-entity-framework-7-app-using-aspnet-5).
* Armen Shimoon posts on using faking in order to defer design decisions in
  [ASP.NET 5 Web API: faking it while making it](http://dotnetliberty.com/index.php/2015/12/07/asp-net-5-web-api-faking-it-while-making-it/).

F#
--

* Jamie Dixon explains [why you don't need a PhD to do F#](https://medium.com/@jamiedixon/progressive-f-tutorials-london-2015-795d76c027da#.67vl10l5w).
* Phillip Trelford has interesting posts on using type providers to create a better IntelliSense experience:
  [Disinherited Types](http://trelford.com/blog/post/Disinherited.aspx),
  and [MSDNify Types](http://trelford.com/blog/post/MSDNify.aspx).

The F# community is writing a new blog post daily for this year's
[F# Advent Calendar in English](https://sergeytihon.wordpress.com/2015/10/25/f-advent-calendar-in-english-2015/).
Here are some of the most recent posts:

* [F#, .NET and the Open Source Situation](https://cockneycoder.wordpress.com/2015/12/08/f-net-and-the-open-source-situation/), by Isaac Abraham.
* [A Quick Look at F# in Visual Studio Code](http://www.wintellect.com/devcenter/jwood/a-quick-look-at-f-in-visual-studio-code), by Jonathan Wood.
* [Christmas Trees in WPF using FSharp.ViewModule](http://reedcopsey.com/2015/12/09/christmas-trees-in-wpf-using-fsharp-viewmodule/), by Reed Copsey, Jr..
* [The Trips and Traps of Creating a Generative Type Provider in F#](https://medium.com/@haumohio/the-trips-and-traps-of-creating-a-generative-type-provider-in-f-75162d99622c#.258c8w748), by Peter Bayne.
* [Algo Trading with F# and GPUs,](http://blog.quantalea.com/?p=8391) by Daniel Egloff.
* [Making Busy Progress in F#](http://teadrivendev.github.io/2015/12/11/making-progress-fsharp/), by @TeaDrivenDev.
* [Providing Value with Trivial Abstraction in F#](http://reidev275.azurewebsites.net/providing-value-with-trivial-abstraction-in-f/), by Reid Evans.
* [What’s New in F# 4.0 in Visual Studio 2015](http://fsharpmonologue.blogspot.co.id/2015/12/whats-new-in-f-40-in-visual-studio-2015.html), by Eriawan Kusumawardho.
* [Solving the Santa Claus Problem in F#](http://www.rickyterrell.com/?p=68), by Riccardo Terrell.
* [Learn the Machine!](https://lenadroid.github.io/posts/machine-learning-fsharp-accorddotnet.html), by @lenadroid.

Check out [F# Weekly](https://sergeytihon.wordpress.com/category/f-weekly/) for more great
content from the F# community.

And this is it for this week!