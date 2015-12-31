The week in .NET - 12/29/2015
=============================

We have a short post this week, as many people are on vacation, including me.
Unsurprisingly, no .NET user group has events for this week, so we'll skip the section this time.

As always, this weekly post couldn't exist without community contributions,
and I'd like to thank all those who sent links and tips.
You can participate too. Did you write a great blog post, or just read one?
Do you want everyone to know about an amazing new contribution or a useful library?
We'd love to hear from you, and feature your contributions on future posts:

* Send an email to beleroy at Microsoft,
* [comment on this gist](https://gist.github.com/bleroy/8662c1ee819d5e46ad04)
* Leave us a pointer in the comments section below.

This week's post (and future posts) also contains news I first read on
[ASP.NET's community spotlight](http://www.asp.net/),
on [F# weekly](https://sergeytihon.wordpress.com/category/f-weekly/),
on [ASP.NET Weekly](http://www.aspnetweekly.com/),
and on [Chris Alcock's The Morning Brew](http://themorningbrew.net/).

To read last week's post, see [The week in .NET - 12/29/2015](http://blogs.msdn.com/b/dotnet/archive/2015/12/22/the-week-in-net-12-22-2015.aspx).

Package of the week: ImageProcessor
-----------------------------------

Server-side image processing is important.
You might need to dynamically generate image thumbnails to make a web site more reactive, or add
watermarks on the fly, or create black-and-white versions of the photos in a gallery.
In all those scenarios, you'll need image processing, you'll need it to be fast, with a good
image quality, and in a concurrency-friendly package, because you're running on the server.

James South's [ImageProcessor](http://imageprocessor.org/) is one of the .NET libraries you can use.
It's open source, lightweight, fast, simple, multi-threading-friendly, and extensible.
It also works on .NET Core (look: no GDI+ dependency).

Here's how you'd resize a JPEG image stream in memory to be 150 pixels wide, maintaining its aspect ratio,
then send it to the output stream: 

```csharp
byte[] photoBytes = File.ReadAllBytes(file);
ISupportedImageFormat format = new JpegFormat { Quality = 70 };
Size size = new Size(150, 0)
using (MemoryStream inStream = new MemoryStream(photoBytes))
{
    using (MemoryStream outStream = new MemoryStream())
    {
        using (ImageFactory imageFactory = new ImageFactory(preserveExifData:true))
        {
            imageFactory.Load(inStream)
                        .Resize(size)
                        .Format(format)
                        .Save(outStream);
        }
    }
}
```

Check out [the documentation for ImageFactory](http://imageprocessor.org/imageprocessor/imagefactory/)
and its awesome list of methods for more exciting treatments you can apply to your images.

.NET News
---------

* [Exploring the new dotnet command-line interface](http://www.hanselman.com/blog/ExploringTheNewNETDotnetCommandLineInterfaceCLI.aspx),
  by Scott Hanselman.
* [Mono's Cooperative Mode for SGen GC](http://tirania.org/blog/archive/2015/Dec-22.html), by Miguel de Icaza.
* [Cross Platform Build Automation with VSTS / TFS 2015](https://channel9.msdn.com/Events/APAC-Influencer-Hero-2015/Singapore-Influencer-Showcase/01-Punit-Ganshani-DevOps-Build-Automation-with-VSTS--TFS-2015),
  by Punit Ganshani.
* [Packaging Libraries with NuGet](https://channel9.msdn.com/Events/APAC-Influencer-Hero-2015/Singapore-Influencer-Showcase/01-Punit-Ganshani-Packaging-your-libraries-with-NuGet),
  by Punit Ganshani.


ASP.NET
-------

* [Publishing a ASP.NET 5 Web-Application to IIS Locally](http://blogs.msdn.com/b/abhinaba/archive/2015/12/22/publishing-a-asp-net-5-web-application-to-iis-locally.aspx)
  by Abhinaba Basu.
  
F#
--

The F# community is writing a new blog post daily for this year’s [F# Advent Calendar in English](https://sergeytihon.wordpress.com/2015/10/25/f-advent-calendar-in-english-2015/).
Lots of great new posts to check out this week!

* [Automatic Re-build and Background Tasks for Suave.io Websites](http://www.navision-blog.de/blog/2015/12/21/adding-background-tasks-to-suave-io-websites/), by Steffen Forkmann.
* [Property-based Testing XSLT](http://theimowski.com/blog/2015/12-21-property-based-testing-xslt/index.html), by Tomasz Heimowski.
* [Hacking Together @wbfacts, a World Bank Twitter Bot](http://www.clear-lines.com/blog/post/hacking-together-wbfacts-a-World-Bank-Twitter-Bot.aspx), by Mathias Brandewinder.
* [F# Advent Calendar 2015](http://www.cylentware.com/blog/post/F-Advent-Calendar-2015), by Chad Boyer.
* [Some Fun with Lambda Calculus](http://gettingsharper.de/2015/12/23/f-advent-2015-some-fun-with-lambda-calculus/), by Carsten König.
* [Getting Started with SignalR Using F# and OWIN](http://troykershaw.com/blog/getting-started-with-signalr-fsharp-owin/), by Troy Kershaw.
* [Comparing Trees, Functionally](http://syntacticsalt.com/2015/12/24/comparing-trees-functionally/), by Matthew Sottile.
* [Designing for Problems Too Big to Test](http://blogs.teamb.com/craigstuntz/2015/12/23/38890/), by Craig Stuntz.
* [Monogame Snowflakes](http://soulfiremage.github.io/Advent2015.html), by Richard Griffiths.
* [F# Powered Real-time Dashboard](http://coding.fitness/f-powered-realtime-dashboard/), by Louie Bacaj.
* [WebSharper - A Year in Review](http://websharper.com/blog-entry/4665/websharper-a-year-in-review), by Adam Granicz.
* [F#, Minecraft, and a Raspberry Pi](http://www.chrisdobby.com/?p=34), by Chris Dobson.
* [Generating Markov Text from YouTube Comments](http://taylorwood.github.io/2015/12/27/youtube-comment-markov.html), by Taylor Wood.
* [Twitter Local](https://indy9000.github.io/twitter-local.html), by Indy Garcia.
 
Check out [F# Weekly](https://sergeytihon.wordpress.com/category/f-weekly/) for more great content from the F# community.

And this is it for this week!