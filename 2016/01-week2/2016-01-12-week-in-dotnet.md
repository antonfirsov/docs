The week in .NET - 1/12/2015
============================

To read last week's post, see [The week in .NET - 1/5/2015](http://blogs.msdn.com/b/dotnet/archive/2016/01/05/the-week-in-net-1-5-2015.aspx).

On.NET
------

Last week, we had [Mads Torgersen on the show](https://www.youtube.com/watch?v=pwdxfY2Y2Ow),
to talk about language design in general, and C# in particular.
This week, we'll talk to Jonathan Chambers and Lucas Meijer from the [Unity 3D](http://unity3d.com/)
team about game engines, and using .NET to target iOS, Android, or even the Web.
Please send me your questions ahead of time, or attend the show and ask them on the chat.
Tune in on [Thursday, at 10:00AM PST](https://www.youtube.com/watch?v=B0yWmVL8hF0)  to view the show live!

Package of the week #1: JSON.NET 8
----------------------------------

[JSON.NET](https://github.com/JamesNK/Newtonsoft.Json) needs no introduction, as it is the recommended library
to serialize and deserialize JSON in .NET.
James Newton-King [just released version 8.0](http://james.newtonking.com/archive/2015/12/20/json-net-8-0-release-1-allocations-and-bug-fixes)
with lots of bug fixes, and great performance improvements.
To improve perf on an already very fast library, James introduced new ways in which a JSON.NET
user can avoid memory allocations.
Instead of allocating new buffers as needed, the new code uses pools of buffers:

```csharp
IList<int> value;
 
var serializer = new JsonSerializer();
using (var reader = new JsonTextReader(new StringReader(@"[1,2,3,4]")))
{
    // reader will get buffer from array pool
    reader.ArrayPool = JsonArrayPool.Instance;
 
    value = serializer.Deserialize<IList<int>>(reader);
}
```

The new feature is still somewhat experimental, and for now, you'll need to provide
your own implementation of `IArrayPool` (a sample is provided on
[the announcement post](http://james.newtonking.com/archive/2015/12/20/json-net-8-0-release-1-allocations-and-bug-fixes)).
In future versions, there will be a built-in implementation.

Package of the week #2: Colorful.Console
----------------------------------------

Console applications are cool, but they can be even cooler with some added color.
[Colorful.Console](http://colorfulconsole.com/) is a drop-in replacement for
`System.Console` that adds some additional APIs that make it super-simple to
write in color.
But that's not all: it also contains a super-cool API that can transform text into
ASCII art:

```csharp
FigletFont font = FigletFont.Load("chunky.flf");
Figlet figlet = new Figlet(font);

Console.WriteLine(figlet.ToAscii("Belvedere"), ColorTranslator.FromHtml("#8AFFEF"));
Console.WriteLine(figlet.ToAscii("ice"), ColorTranslator.FromHtml("#FAD6FF"));
Console.WriteLine(figlet.ToAscii("cream."), ColorTranslator.FromHtml("#B8DBFF"));
```

![Belvedere ice cream formatted as ASCII art](http://colorfulconsole.com/images/ascii_x2.png)

User group of the week: Adelaide .NET User Group
------------------------------------------------

On [Wednesday, January 13 in Adelaide](http://www.meetup.com/Adelaide-dotNET/events/227689829/)
(yep, in Autralia), David Gardiner will present on IntelliTest and other .NET unit testing tools.

.NET
----

* In [a great series of questions and answers](https://stackoverflow.com/questions/34611919/how-to-package-a-portable-net-library-targeting-net-core),
  Sander shows [how to package a .NET library using various compilation targets, including .NET Core](https://stackoverflow.com/questions/34611919/how-to-package-a-portable-net-library-targeting-net-core)
* [Debugging and Profiling in Visual Studio 2015](https://www.simple-talk.com/dotnet/visual-studio/debugging-and-profiling-in-visual-studio-2015/)
  by Manuel Meyer.
* Tony Sneed recounts his journey to open source in [How open source changed my life](https://github.com/JimBobSquarePants/ImageProcessor/).
* [To base(), or not to base(), that is the question](http://codeblog.jonskeet.uk/2016/01/08/to-base-or-not-to-base-that-is-the-question/)
  by Jon Skeet.
* [Evolution of C#](http://www.kunal-chowdhury.com/2016/01/csharp-basics.html)
  by Kunal Chowdhury.
* [Functional Microservices](http://www.dotnetrocks.com/default.aspx?ShowNum=1240),
  the .NET Rocks show with Rachel Reese.

ASP.NET
-------

* [Goodbye child actions, hello view components](http://www.davepaquette.com/archive/2016/01/02/goodbye-child-actions-hello-view-components.aspx)
  by Dave Paquette.
* [Best practices for private config data and connection strings in configuration in ASP.NET and Azure](http://www.hanselman.com/blog/BestPracticesForPrivateConfigDataAndConnectionStringsInConfigurationInASPNETAndAzure.aspx)
  by Scott Hanselman.
* [Real time translated chat with ASP.NET, Microsoft Translator and IP Messaging](https://www.twilio.com/blog/2015/12/hola-ip-messaging-real-time-translated-chat-with-asp-net-microsoft-translator-and-ip-messaging.html)
  by Devin Rader.
* [Building APIs with MVC 6 and OAuth (video)](https://www.youtube.com/watch?v=vqcAVic4Ej0)
  by Filip Ekberg.
* [How to take an ASP.NET MVC web site down for maintenance](https://www.simple-talk.com/dotnet/asp.net/how-to-take-an-asp.net-mvc-web-site-down-for-maintenance/)
  by Jon smith.
* [Experiments with Entity Framework 7 and ASp.NET MVC 6](http://damienbod.com/2016/01/07/experiments-with-entity-framework-7-and-asp-net-5-mvc-6/)
  by Damien Bod.

F#
--

Great progress has been made to add .NET Core support to the Visual F# compiler.
The compiler and F# Interactive now run on CoreCLR on Windows, OS X, and Linux, but there's still plenty of work left.
To track the progress of the project and find ways to contribute, check out
[the status page on Github](https://github.com/Microsoft/visualfsharp/wiki/F%23-for-CoreCLR---Status).

* [Lean and Functional Programming](https://vimeo.com/album/3452190/video/131189623#t=2m04s),
  by Bryan Hunter.
* [Visualizing F# Advent Calendar Contributors](http://www.pirrmann.net/visualizing-f-advent-calendar-contributors/),
  by Pierre Irrmann.
* [Reconciling Stack Traces with Computation Expressions](https://eiriktsarpalis.wordpress.com/2015/12/27/reconciling-stacktraces-with-computation-expressions/),
  by Eirik Tsarpalis
* [F# Presentations from CodeMash 2016](http://blogs.teamb.com/craigstuntz/2015/11/09/38883/),
  by Craig Stuntz

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
* [comment on this gist](https://gist.github.com/bleroy/8935596f8518e41281d2)
* Leave us a pointer in the comments section below.

This week's post (and future posts) also contains news I first read on
[ASP.NET's community spotlight](http://www.asp.net/),
on [F# weekly](https://sergeytihon.wordpress.com/category/f-weekly/),
on [ASP.NET Weekly](http://www.aspnetweekly.com/),
on [Dirk Strauss' The Daily Six Pack](http://www.dirkstrauss.com/the-daily-six-pack/),
and on [Chris Alcock's The Morning Brew](http://themorningbrew.net/).
