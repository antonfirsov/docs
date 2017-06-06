---
title: The week in .NET - On .NET with Brett Morrison, DateTime Extensions
keywords: Week in .NET, .NET, community
weblogName: .NET Blog
---
Previous posts:

* [Open XML SDK, Adventure Time](https://blogs.msdn.microsoft.com/dotnet/2017/05/30/the-week-in-net-open-xml-sdk-adventure-time/)
* [.NET poster, Happy Birthday .NET with Jan Kotas, Skyworld](https://blogs.msdn.microsoft.com/dotnet/2017/05/23/the-week-in-net-net-poster-happy-birthday-net-with-jan-kotas-skyworld/)
* [Microsoft Build 2017, .NET Core 2.0 preview 1, For the King](https://blogs.msdn.microsoft.com/dotnet/2017/05/16/the-week-in-net-microsoft-build-2017-net-core-2-0-preview-1-for-the-king/)
 
On .NET: Brett Morrison
-----------------------

During the Build conference, I caught up with my friend Brett Morrison. Brett is an entrepreneur, executive, and hands-on developer, who has been using Microsoft products and .NET throughout his career. He founded startups, such as [Onestop](http://onestop.com/) and ememories, and also worked for [SpaceX](http://www.spacex.com/).

<iframe src="https://channel9.msdn.com/Shows/On-NET/Brett-Morrison/player" width="960" height="540" allowFullScreen frameBorder="0"></iframe>

Package of the week: DateTime Extensions
----------------------------------------

Date calculations can be tricky, but if you need to take holidays into accounts, things become really complicated. The [DateTime Extensions](https://github.com/joaomatossilva/DateTimeExtensions/) project knows holidays for two dozen cultures, and can perform calculations taking them into account.

```csharp
DateTimeCultureInfo pt_ci = new DateTimeCultureInfo("pt-PT");
DateTime startDate = new DateTime(2011, 4, 21);

//21-04-2011 - start
//22-04-2011 - holiday
//23-04-2011 - saturday
//24-04-2011 - sunday
//25-04-2011 - holiday
//26-04-2011 - end

DateTime endDate = startDate.AddWorkingDays( 1, pt_ci);
Assert.IsTrue(endDate == startDate.AddDays(5));
```

* [DateTime Extensions Web site](http://www.kspace.pt/DateTimeExtensions/)
* [DateTime Extensions on GitHub](https://github.com/joaomatossilva/DateTimeExtensions)
* [DateTime Extensions on NuGet](https://www.nuget.org/packages/DateTimeExtensions)
* [Sample web site](http://datetimeextensions.azurewebsites.net/)

Meetup of the week: Donovan Brown - Zero to DevOps and Database DevOps in Cambridge
------------------------------------------------------------------------

DevOps is about people, process and products. Getting it all right requires effort but the benefits to your organisation and customers can be huge. In this demo-heavy session, Donovan Brown will show you how to go from "zero to DevOps" like a pro. Starting from just a blank desktop, he will create a new team project and a complete pipeline. He will also deploy an ASP.NET MVC application into Azure all live and hands on.

This meetup is on [Monday, June 12, in Cambridge](https://www.meetup.com/Cambridge-NET-User-Group/events/237509818/). It's hosted by the [Cambridge .NET User Group](https://www.meetup.com/Cambridge-NET-User-Group/).


## .NET

* [.NET Core and .NET Framework Working Together, Or: The Magic of .NET Standard](https://dotnetcore.gaprogman.com/2017/06/01/net-core-and-net-framework-working-together-or-the-magic-of-net-standard/) by Jamie Taylor.
* [Announcing Reactive Extensions for .NET 4.0 Preview 1!](https://oren.codes/2017/05/27/announcing-reactive-extensions-for-net-4-0-preview-1/) by Oren Novotny.
* [The Coming .NET Renaissance](http://www.aaronstannard.com/the-coming-dotnet-reinassance/) by Aaron Stannard.
* [Choice amongst cross-platform .NET IDEs - VS Code, Visual Studio for Mac, JetBrains Rider](https://www.hanselman.com/blog/ChoiceAmongstCrossplatformNETIDEsVSCodeVisualStudioForMacJetBrainsRider.aspx) by Scott Hanselman.
* [Array Pool](https://csharp.christiannagel.com/2017/05/31/arraypool/) by Christian Nagel.
* [Creating a simple key-value logger for an object graph](https://danielwertheim.se/creating-a-simple-key-value-logger-for-an-object-graph/) by Daniel Wertheim.
* [The Fraternal Twins of Equals and GetHashCode](https://visualstudiomagazine.com/articles/2017/04/01/fraternal-twins-gethashcode-equals-csharp-vb.aspx) by Tim Patrick.
* [.NET Core Support in dotConnect Providers and LinqConnect!](https://www.devart.com/news/2017/net-core-support.html) by Devart.
* [Docker for .NET Developers (Part 1)An introduction to Docker for .NET developers](https://www.stevejgordon.co.uk/docker-dotnet-developers-part-1) by Steve Gordon.
* [Docker for .NET Developers (Part 2) Taking a look at our first dockerfile and building an image for an ASP.NET Core API service](https://www.stevejgordon.co.uk/docker-for-dotnet-developers-part-2) by Steve Gordon.
* [Using Roslyn refactorings with OmniSharp and Visual Studio Code](https://www.strathweb.com/2017/05/using-roslyn-refactorings-with-omnisharp-and-visual-studio-code/) by Filip W.
* [Refactoring dependencies with Autofac Aggregate Services](http://cecilphillip.com/autofac-aggregate-services/) by Cecil Phillip.
* [ClrMD Part 4 – What callbacks are called by my timers?](http://labs.criteo.com/2017/05/clrmd-part-4-callbacks-called-timers/) by Nasarre Christophe and Kevin Gosse.

## ASP.NET

* [IdentityServer4: New & Improved for ASP.NET Core](https://channel9.msdn.com/Events/Techorama/Techorama-2017/BRK02) by Dominick Baier.
* [Installing Asp.Net Core Docker For Windows](http://sibeeshpassion.com/installing-asp-net-core-docker-for-windows/) by Sibeesh Passion.
* [Getting Started with ASP.NET Core JavaScript Services](http://www.codingflow.net/getting-started-with-asp-net-core-javascript-services/) by Jason Taylor.
* [The Microsoft.AspNetCore.All metapackage is huge, and that's awesome, thanks to the .NET Core runtime store](https://andrewlock.net/the-microsoft-aspnetcore-all-metapackage-is-huge-and-thats-awesome-thanks-to-the-net-core-runtime-store-2/) by Andrew Lock.
* [Using ImageSharp to resize images in ASP.NET Core - Part 4: saving to disk](https://andrewlock.net/using-imagesharp-to-resize-images-in-asp-net-core-part-4-saving-to-disk/) by Andrew Lock.
* [ASP.NET Core – Logging](https://codingblast.com/asp-net-core-logging/) by CodingBlast.
* [The end of request validation](https://www.jardinesoftware.net/2017/06/01/the-end-of-request-validation/) by James Jardine.
* [Post-Redirect-Get and TempData with ASP.NET Core](https://www.meziantou.net/2017/06/05/post-redirect-get-and-tempdata-with-asp-net-core) by Gérald Barré.
* [Conditional middleware based on request in ASP.NET Core](https://www.devtrends.co.uk/blog/conditional-middleware-based-on-request-in-asp.net-core) by Paul Hiles.
* [Bypassing IIS Error Messages in ASP.NET](https://weblog.west-wind.com/posts/2017/Jun/01/Bypassing-IIS-Error-Messages-in-ASPNET) by Rick Strahl.
* [Using VS Code and ASP.NET Core?](https://wildermuth.com/2017/06/04/Using-VS-Code-and-ASP-NET-Core) by Shawn Wildermuth.
* [Owin middleware in .NET Standard for Application Insights - part 2](https://stapp.space/owin-middleware-in-net-standard-for-application-insights-part-2/) by Piotr Stapp.
* [ASP.NET Core Utils - nuget package available](https://devblog.dymel.pl/2017/05/31/asp-net-core-utlis/) by Michal Dymel.
* [Implementing a silent token renew in Angular for the OpenID Connect Implicit flow](https://damienbod.com/2017/06/02/implementing-a-silent-token-renew-in-angular-for-the-openid-connect-implicit-flow/) by Damien Bowden.

## C#

* [Practical C# – Select in LINQ](http://www.andreaangella.com/2017/05/practical-c-videos-local-functions-ref-returns-and-locals/) by Andrea Angella.
* [Practical C# – Aggregate in LINQ](http://www.andreaangella.com/2017/06/practical-csharp-aggregate-in-linq/) by Andrea Angella.
* [Practical C# – Sum in LINQ](http://www.andreaangella.com/2017/06/practical-csharp-sum-in-linq/) by Andrea Angella.
* [Practical C# – Where in LINQ](http://www.andreaangella.com/2017/06/where-in-linq/) by Andrea Angella.
* [Strong Typing: a pattern for more robust and maintainable code](https://tech.winton.com/blog/2017/06/strong-typing-a-pattern-for-more-robust-code) by Jos Hickson.

## F#

* [Introducing Fable.Remoting: Automated Type-Safe Client-Server Communication for Fable Apps](https://medium.com/@zaid.naom/introducing-fable-remoting-automated-type-safe-client-server-communication-for-fable-apps-e567454d594c) by Zaid Ajaj.
* [(Nearly) Everything You Ever Wanted to Know About F# Active Patterns](https://www.youtube.com/watch?v=I5dKFT_Z-fc&feature=youtu.be) by Hakka Labs.
* [F# Partiall-Applied Unions](https://amazingant.com/blog/2017/05/29/FSharp-Partially-Applied-Unions/) by Anthony Perez.
* [Why you should use F#](https://blogs.msdn.microsoft.com/dotnet/2017/05/31/why-you-should-use-f/) by Phillip Carter and Mads Torgersen.
* [Using Polly with F# async workflows by Mark Seemann](http://blog.ploeh.dk/2017/05/30/using-polly-with-f-async-workflows/) by Mark Seemann.
* [Encapsulation - C# vs F# vs Haskell, equivalent result](http://blog.stermon.com/articles/2017/05/31/encapsulation-csharp-vs-fsharp-vs-haskell-equivalent-result) by Ramón Soto Mathiesen.
* [Azure Functions tip: working locally with F# Scripts](http://brandewinder.com/2017/06/01/azure-functions-local-development-with-fsharp-scripts/) by Mathias Brandewinder.

There is more content available this week in [F# Weekly](https://sergeytihon.wordpress.com/category/f-weekly/).  If you want to see more F# awesomeness, please check it out!

## VB

* [Simple .NET Core application using VB.NET](http://gunnarpeipman.com/2017/06/dotnet-core-vbnet/) by Gunnar Peipman.

## Xamarin

* [Xamarin Stable Release: 15.2.2 Xamarin.Android, Xamarin.VS Hotfix](https://releases.xamarin.com/stable-release-15-2-2-xamarin-android-xamarin-vs-hotfix/) by Bri Brothers.
* [Xamarin University To Host Free Webinars in June](https://visualstudiomagazine.com/articles/2017/05/30/xamarin-university-free-webinars.aspx) by Michael Domingo.
* [New & Upcoming Xamarin Dev Days](https://blog.xamarin.com/new-upcoming-xamarin-dev-days/) by Jayme Singleton.
* [Xamarin Podcast: Previewing Xamarin.Forms 3.0](https://blog.xamarin.com/podcast-previewing-xamarin-forms-3-0/) by Pierce Boggan.
* [Introducing ELXF: A UI Framework for Concise, Maintainable & Fast Programmatic UI's for Xamarin.Forms](http://www.leerichardson.com/2017/05/introducing-easylayout-for-xamarinforms.html) by Lee Richardson.
* [Introduction to Rest Web Services in Xamarin](http://xamarinui.blogspot.com/2017/05/introduction-to-rest-web-services-in.html) by Asfend Yar.
* [Shared Components In Xamarin Forms](http://xamarinui.blogspot.com/2017/05/shared-components-in-xamarin-forms.html) by Asfend Yar.
* [View Sizing In Xamarin Forms](http://xamarinui.blogspot.com/2017/05/view-sizing-in-xamarin-forms.html) by Asfend Yar.
* [A simple page-indicator for your android view-pager](http://xleon.net/xamarin/android/a-simple-page-indicator-for-your-android-viewpager.html) by Diego Ponce de León.
* [Xamarin.Tip – Adding Dynamic Elevation to Your Xamarin.Forms Buttons](https://alexdunn.org/2017/05/30/xamarin-tips-adding-dynamic-elevation-to-your-xamarin-forms-buttons/) by Alex Dunn.
* [Xamarin.Tip – Mvvm Light and Dependency Injection](https://alexdunn.org/2017/06/01/xamarin-tips-mvvm-light-and-dependency-injection/) by Alex Dunn.
* [Snack Pack 12: Getting Started with Visual Studio for Mac](https://channel9.msdn.com/Shows/XamarinShow/Snack-Pack-12-Getting-Started-with-Visual-Studio-for-Mac) by James Montemagno.
* [Text and Icons in Master/Detail Reveal Button on iOS](https://codemilltech.com/text-and-icons-in-master-detail-pages-on-ios/) by Matthew Soucoup.
* [Things I Think Are Cool: Merge Conflict Podcast](https://codemilltech.com/things-i-think-are-cool-mergeconflict-podcast/) by Matthew Soucoup.
* [Use Camera To Take Photo In Xamarin Forms](https://xamarinhelp.com/use-camera-take-photo-xamarin-forms/) by Adam Pedley.
* [Xamarin Mobile Apps Continuous Integration and Delivery with Jenkins and HockeyApp](https://www.junian.net/2017/05/xamarin-ci-cd-with-jenkins-and-hockeyapp.html) by Junian Triajianto.
* [Trying Out Xamarin Live Player](https://mindofai.github.io/Trying-Out-Xamarin-Live-Player/) by Bryan Anthony Garcia.
* [Ambient Properties in Xamarin.Forms](https://nicksnettravels.builttoroam.com/post/2017/05/27/Ambient-Properties-in-XamarinForms.aspx) by Nick Randolph.
* [Toolbar Navigation in Xamarin Forms](https://jfarrell.net/2017/05/28/toolbar-navigation-in-xamarin-forms/) by Jason Farrell.

## Azure

* [Solve production exceptions in no time with Application Insights Snapshots](https://www.patrickvankleef.com/2017/05/31/solve-production-exceptions-in-no-time-with-application-insights-snapshots/) by Patrick van Kleef.
* [Azure SQL Data Sync Refresh](https://azure.microsoft.com/blog/azure-sql-data-sync-refresh/) by Joshua Gnanayutham.
* [Diagnose sudden changes in your app behavior with a click!](https://azure.microsoft.com/blog/diagnose-sudden-changes-in-your-app-behavior-with-a-click/) by Sharon Nakibly.
* [Streamlining Kubernetes development with Draft](https://azure.microsoft.com/blog/streamlining-kubernetes-development-with-draft/) by Gabe Monroy.
* [Azure via C# – Create Azure Blobs](http://www.andreaangella.com/2017/06/azure-via-cs-create-azure-blobs/) by Andrea Angella.
* [Building a JAMstack site with Hugo and Azure Functions](http://conductofcode.io/post/building-a-jamstack-site-with-hugo-and-azure-functions/) by Conduct of Code.

## UWP

* [UWP and the evolution of touch development](https://blogs.windows.com/buildingapps/2017/05/25/uwp-evolution-touch-development/) by Windows Apps Team.
* [Toolkits, Toolkits, Toolkits!](https://blogs.windows.com/buildingapps/2017/06/02/toolkits-toolkits-toolkits/) by Windows Apps Team.

And this is it for this week!

Contribute to the week in .NET
------------------------------

As always, this weekly post couldn't exist without community contributions, and I'd like to thank all those who sent links and tips. The F# section is provided by [Phillip Carter](https://twitter.com/_cartermp), the gaming section by [Stacey Haffner](https://twitter.com/yecats131), the Xamarin section by [Dan Rigby](https://twitter.com/DanRigby), and the Azure and UWP section by [Michael Crump](http://twitter.com/mbcrump).

You can participate too. Did you write a great blog post, or just read one? Do you want everyone to know about an amazing new contribution or a useful library? Did you make or play a great game built on .NET?
We'd love to hear from you, and feature your contributions on future posts. Please [add your posts](https://weekindotnet.azurewebsites.net), it takes only a second.

We pick the articles based on the following criteria: the posts must be about .NET, they must have been published this week, and they must be original contents. Publication in Week in .NET is not an endorsement from Microsoft or the authors of this post.

This week's post (and future posts) also contains news I first read on [The ASP.NET Community Standup](https://blogs.msdn.microsoft.com/webdev/tag/communitystandup/), on [Weekly Xamarin](http://weeklyxamarin.com/), on [F# weekly](https://sergeytihon.wordpress.com/category/f-weekly/), and on [The Morning Brew](http://themorningbrew.net/).