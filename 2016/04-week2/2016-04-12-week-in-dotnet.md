The week in .NET - 4/12/2016
============================

To read last week's post, see [The week in .NET – 4/5/2016](https://blogs.msdn.microsoft.com/dotnet/2016/04/05/the-week-in-net-452016/).

On.NET
------

We had to reschedule last week's show, but [we'll be back this week with a new episode](https://www.youtube.com/watch?v=hDDd_Pjtbx8) with [PlayFab](https://playfab.com/) to talk about how the cloud can help make awesome games.

Project of the week: Math.NET Numerics
--------------------------------------

[Math.NET](http://www.mathdotnet.com/) is an open source initiative to implement mathematical computation in .NET. It includes numerical computing, symbolic algebra, geometry, and signal processing. This week, we'll look at [numerical computing](http://numerics.mathdotnet.com/).

Here's how you can define matrices and vectors, manipulate them, and perform operations between them:

```csharp
var M = Matrix<double>.Build;
var V = Vector<double>.Build;
var m = M.DenseOfArray(new[,] {{ 1.0,  2.0, 1.0},
                               {-2.0, -3.0, 1.0},
                               { 3.0,  5.0, 0.0}});
var v = V.Dense(3, i => i*i);

var transformed = m.Transpose() * v;
```

Notice how the vector's coordinates are defined with an expression rather than plain numerical values.

You can also solve equation systems, do statistics, probability, and even fit curves. It really is a mathematical treasure trove.

It's especially nice that the packages come with examples in C#, F#, and VB.

User group meeting of the week: Building a Robot Army & Encryption with .NET and SQL 2016
-----------------------------------------------------------------------------------------

The [Adelaide .NET User Group](http://www.meetup.com/Adelaide-dotNET/) has [a double feature on Wednesday, April 13 at 5:30 PM](http://www.meetup.com/Adelaide-dotNET/events/229525766/) where you'll learn both how to build a robot army, and about data encryption using .NET and SQL Server.

Control of the week: SideDrawer for UWP and Xamarin
---------------------------------------------------

Telerik's [SideDrawer control](http://www.telerik.com/xamarin-ui/sidedrawer) is a nice implementation for a slide-out navigation UI for modern mobile applications. It works with Xamarin and UWP, which enables it to target Android, iOS, and Windows.

![SideDrawer](http://d585tldpucybw.cloudfront.net/sfimages/default-source/productsimages/ui-for-xamarin/ProductItems/xamarin-sidedrawer.png?sfvrsn=1)

.NET
----

* It's now possible to keep an eye on .NET issues on Github from Twitter: [Core CLR](https://twitter.com/coreclrissues), [Core FX](https://twitter.com/corefxissues) and [Core FX Lab](https://twitter.com/corefxlabissues), [Roslyn](https://twitter.com/roslynissues), [ASP.NET](https://twitter.com/aspnetissues), and [Entity Framework](https://twitter.com/efissues).
* [Moving to Cake (C# Make)](http://laurentkempe.com/2016/04/05/Moving-to-Cake-CSharp-Make/) by Laurent Kempé.
* [Nick Landry on .NET Framework and .NET Core (video)](https://channel9.msdn.com/Blogs/Technology-and-Friends/tf420) by Nick Landry and David Giard.
* [Survey Report: Who is the .NET Developer of 2016?](http://www.telerik.com/blogs/survey-report-the-dotnet-developer-of-2016) by Nora Georgieva.
* [Visual Studio “15”: Installing Just What You Need](https://blogs.msdn.microsoft.com/visualstudio/2016/04/05/visual-studio-15-installing-just-what-you-need/) by Cathy Sullivan.
* Robert Sundström started [a nice minimalist HTTP Listener for .NET Core and UWP](https://github.com/robertsundstrom/HttpListener) that will actually work on a Raspberry Pi running Windows 10 for IoT.
* [Detecting and Solving Memory Problems in .NET (free E-Book)](http://blog.jetbrains.com/dotnet/2016/04/04/detecting-and-solving-memory-problems-in-net-ebook/) by Alexey Totin.
* [Automatically create and publish a NuGet package using VSTS](https://technologies.live/2016/04/01/automatically-create-and-publish-a-nuget-package/) by Mauricio Avilés Diaz.
* [What I think is and is not better about .Net OSS these days](https://jeremydmiller.com/2016/04/07/what-i-think-is-and-is-not-better-about-net-oss-these-days/), and [its follow-up post](https://jeremydmiller.com/2016/04/11/a-quick-followup-to-my-opinions-on-net-oss/) by Jeremy D. Miller.
* [Biometric Authentication with Microsoft Passport](http://developer.telerik.com/featured/powering-apps-microsoft-passport/) and [The Xamarin Promise – Realized!](http://developer.telerik.com/featured/xamarin-promise-realized/) by Sam Basu.
* [Idempotent Aggregates](http://codeopinion.com/idempotent-aggregates/) by Derek Comartin.
* [Closure-based State: C#](http://blogs.tedneward.com/patterns/ClosureBasedState-CSharp/) by Ted Neward.

ASP.NET
-------

Get the latest ASP.NET news directly from the team with [the ASP.NET Community Standup](https://www.youtube.com/playlist?list=PL0M0zPgJ3HSftTAAHttA3JQU4vOjXFquF). There are also [transcripts on the .NET Web Development and Tools Blog](https://blogs.msdn.microsoft.com/webdev/tag/communitystandup/).

* [The New Configuration Model in ASP.NET Core](http://developer.telerik.com/featured/new-configuration-model-asp-net-core/) by Julio Avellaneda.
* [Inline Route Constraints in ASP.NET Core MVC](https://blog.mariusschulz.com/2016/03/31/inline-route-constraints-in-asp-net-core-mvc) by Marius Schulz.
* [Creating Dynamic PDFs in ASP.NET MVC using iTextSharp](http://www.danylkoweb.com/Blog/creating-dynamic-pdfs-in-aspnet-mvc-using-itextsharp-EV) by Jonathan Danylko.
* [Understanding The Role of Startup.cs File In ASP.NET Core Project](http://www.c-sharpcorner.com/article/understanding-the-role-of-startup-cs-file-in-Asp-Net-core/) by Sunny Sharma.
* [Configuring ASP.NET and IIS Request Length for POST Data](http://weblog.west-wind.com/posts/2016/Apr/06/Configuring-ASPNET-and-IIS-Request-Length-for-POST-Data) by Rick Strahl.
* [Encryption and Decryption in ASP.NET Core](http://www.mikesdotnetting.com/article/295/encryption-and-decryption-in-asp-net-core) by Mike Brind.
* [Performance competition is a good thing](https://www.techempower.com/blog/2016/02/24/performance-competition-is-a-good-thing/) by TechEmpower.
* [Developing in Docker Containers](https://blogs.msdn.microsoft.com/stevelasker/2016/02/18/f5-developing-in-docker-containers-version-0-10-of-docker-tools-for-visual-studio/) by Steve Lasker.
* [Secure file download using Identity Server 4, Angular 2, and ASP.NET Core](http://damienbod.com/2016/03/14/secure-file-download-using-identityserver4-angular2-and-asp-net-core/) by Damien Bod.
* [Emails using Mailgun in ASP.NET Core](http://www.elanderson.net/2016/02/emails-using-mailgun-in-asp-net-core/) by Eric L. Anderson.
* [IdentityServer 4 on Docker](https://ankitbko.github.io/2016/03/IdentityServer4-on-Docker/) by Ankit Sinha.
* [Ensure your ASP.NET actions aren't missing authorization with unit tests](http://blogs.lessthandot.com/index.php/webdev/asp-net-ensure-your-actions-arent-missing-authorization-with-unit-tests/) by Eli Weinstock-Herman.
* [Predefined Namespaces And Custom Base View Page in ASP.NET Core 1.0 MVC](http://www.strathweb.com/2016/04/predefined-namespaces-and-custom-base-view-page-in-asp-net-core-1-0-mvc/) by Filip W.

F#
--

* The F# Software Foundation has launched the [F# Speakers Program](http://foundation.fsharp.org/speakers_program_launch)
* [From Community to Cloud with F#](https://vimeo.com/162061772?ref=tw-share), by Don Syme
* [Designing with Capabilities for Fun and Profit](https://vimeo.com/162209391), by Scott Wlaschin
* [Hosting Suave in the Azure App Service](https://cockneycoder.wordpress.com/2016/04/08/hosting-suave-in-the-azure-app-service/), by Isaac Abraham
* [Deploying an F# Web Application with Suave](https://www.youtube.com/watch?v=JgAY7BVzUD8), by Tomas Petricek
* [Hopac: Getting Started with Jobs](https://neoeinstein.github.io/blog/2016/04-08-hopac-getting-started-with-jobs/index.html), by Marcus Griep

Check out [F# Weekly](https://sergeytihon.wordpress.com/category/f-weekly/) for more great content from the F# community.

Games
-----

* [Developing The New Input System Together With You](http://blogs.unity3d.com/2016/04/12/developing-the-new-input-system-together-with-you/), by Rune Skovbo Johansen.
* [Basic Unity Tutorial for Steam VR & Vive (Setting up HMD and controllers) - Video](https://www.youtube.com/watch?v=LZTctk19sx8), by Sean Lee.

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
* [comment on this gist](https://gist.github.com/bleroy/74ec154ddecd9608b83009f18315681d)
* Leave us a pointer in the comments section below.
* [Send Stacey (@yecats131) tips on Twitter about .NET games](https://twitter.com/yecats131).

This week's post (and future posts) also contains news I first read on
[The ASP.NET Community Standup](https://blogs.msdn.microsoft.com/webdev/tag/communitystandup/),
on [F# weekly](https://sergeytihon.wordpress.com/category/f-weekly/),
on [ASP.NET Weekly](http://www.aspnetweekly.com/),
and on [Chris Alcock's The Morning Brew](http://themorningbrew.net/).
