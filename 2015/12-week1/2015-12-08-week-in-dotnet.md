The week in .NET - 12/08/2015
=============================

Welcome to the second "The Week in .NET" post.
It was great to see how well the first post was received.
Thanks to all who sent messages of encouragement and contributions to this week's post.
If you would like to participate too, if you wrote a great blog post, or just read one,
if you want to show an amazing new contribution, if you've written a useful library,
we'd love to hear from you, and feature it on future posts.
You can send me an email to beleroy at Microsoft,
or you can [comment on this gist](https://gist.github.com/bleroy/857ffb9796295606734e) with new
links, or you can simply leave us a pointer in the comment section below.

Package of the week #0: AngleSharp
----------------------------------

Let's start by talking a little bit about the section in last week's post that generated the
most comments: the package of the week.
Last week, I chose `HtmlAgilityPack` because it's a package that I've used a lot in the past,
and that is one of the most downloaded on NuGet.
Several of you mentioned that they were using [AngleSharp](https://www.nuget.org/packages/AngleSharp/)
in lieu of `HtmlAgilityPack`, specifically for its more modern approach, and because it's being
more actively maintained.
Indeed, it uses CSS queries instead of XPath, and is a joy to use.

Here's last week's sample code, but using `AngleSharp` instead of `HtmlAgilityPack`:

<script src="https://gist.github.com/bleroy/ca1499fee30581358e9e"></script>

Package of the week #1: Bogus
-----------------------------

[Brian Chavez](https://github.com/bchavez) sent a tip about his [Bogus](https://www.nuget.org/packages/Bogus/)
package, which creates fake data that you can use in your test cases.
The library is a lot of fun to use, and can generate a boatload of different data types, from
localized Lorem Ipsum text to images, with a result that always looks like real data from real users.

Here's how you can generate fake user data, for example:

<script src="https://gist.github.com/bleroy/d596de64caeeab352795"></script>

News from Core
--------------

[Joe Audette](https://github.com/joeaudette) sent in a tip telling us that
[Mailkit now supports .NET Core](https://github.com/jstedfast/MailKit/issues/212).
This is great news as it means that it's now super-easy to send mail from a .NET Core app.

Brian Chavez also manages the [.NET driver for RethinkDB](https://www.nuget.org/packages/RethinkDb.Driver),
which is now compatible with .NET Core.

F# news
-------

Every week from now on, we're going to relay a selection of news from the very active F# community.
Thanks to Sergey Tihon and David Stephens for providing these links.

* [Krzysztof Cieślak announces F# Support in Visual Studio Code with Ionide](http://blogs.msdn.com/b/dotnet/archive/2015/12/03/guest-post-announcing-f-support-in-visual-studio-code-with-ionide.aspx)
* [The F# community is writing a new blog post daily for this year's F# Advent Calendar](https://sergeytihon.wordpress.com/2015/10/25/f-advent-calendar-in-english-2015/)
* [Thirteen Ways of Looking at a Turtle Part 1](http://fsharpforfunandprofit.com/posts/13-ways-of-looking-at-a-turtle/) and
  [Part 2](http://fsharpforfunandprofit.com/posts/13-ways-of-looking-at-a-turtle-2/), by Scott Wlaschin
* [No. 1 at Christmas, by Sean Trelford](https://seantrelfordblog.wordpress.com/2015/12/05/no1s/)
* [Computation Expressions and Microphones, by Andrea Magnorsky](http://www.roundcrisis.com/2015/12/06/Computation-expressions-in-practice/)
* [F# 2015 Advent Cookies, by Christopher Atkins](http://blog.christopher-atkins.com/fsharp-advent-2015/)
* [Application Contracts with Swagger-powered APIs for .NET, by Sergey Tihon](https://sergeytihon.wordpress.com/2015/12/07/application-contracts-with-swagger-powered-apis-for-net-or-why-swaggerprovider/)
* [What’s the Time, Mr. Wolf?, by Aaron Powell](http://www.aaron-powell.com/posts/2015-12-07-whats-the-time-mr-wolf.html)

User group of the week: dotNET Miami
------------------------------------

dotNet Miami is a user group in Miami, FL, USA.
On Thursday, December 10th, at 6:30PM, they have an intro-level session about
[F# and database design fundamentals](http://communitymegaphone.com/ShowEvent.aspx?EventID=7390).

Blog posts of the week
----------------------

* [Bar Arnon writes about the efficiency of `ValueTask<T>`](http://blog.i3arnon.com/2015/11/30/valuetask/)
* [Leif Battermann wrote a mind-bending article about porting monadic parsers to C#](http://blog.leifbattermann.de/2015/11/23/functional-monadic-parsers-ported-to-c/)
* [Two years old, but still 100% relevant, Lucian Wischik's seven-part video of tips for Async](https://channel9.msdn.com/Series/Three-Essential-Tips-for-Async)

And this is it for this week!