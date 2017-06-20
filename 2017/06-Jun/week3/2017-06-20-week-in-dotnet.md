---
title: 'The week in .NET - .NET Architecture: Microservices & Containers, On .NET with Omer Raviv on OzCode, Sprache'
keywords: Week in .NET, community, .NET
weblogName: .NET Blog
postId: 12105
---
Previous posts:

* [On .NET with Mattias Karlsson on Cake, Topshelf](https://blogs.msdn.microsoft.com/dotnet/2017/06/14/the-week-in-net-on-net-with-mattias-karlsson-on-cake-topshelf/)
* [On .NET with Brett Morrison, DateTime Extensions](https://blogs.msdn.microsoft.com/dotnet/2017/06/06/the-week-in-net-on-net-with-brett-morrison-datetime-extensions/)
* [Open XML SDK, Adventure Time](https://blogs.msdn.microsoft.com/dotnet/2017/05/30/the-week-in-net-open-xml-sdk-adventure-time/)
 
## .NET Architecture – Microservices & Containers

We recently added an [Architecture Guidance page](https://www.microsoft.com/net/learn/architecture) on the .NET Website. This new page pulls together architecture related resources for building different types of applications with .NET. We’ll be adding and updating content as it becomes available.

The Microservices & Containers section of the page provides some great resources for building and deploying microservices with .NET. [Cesar De la Torre](https://twitter.com/cesardelatorre) has authored two eBooks, one on designing and building microservices, and the other on deploying and maintaining them in production. You’ll also find the eShopOnContainers sample application, that shows all the concepts in action.

Visit the [Architecture Guidance page](https://www.microsoft.com/net/learn/architecture) to get these resources, and more.


## On .NET: Omer Raviv - OzCode

During the Microsoft Build conference, we recorded [interviews with some of the attendees](https://channel9.msdn.com/Shows/On-NET/Omer-Raviv-OzCode). This week, we're publishing the one we did with Omer Raviv, with a fantastic demo of [OzCode](https://oz-code.com/), a debugging extension for Visual Studio that makes it a lot easier to debug Linq expressions, displays variable values right in the code editor, and much, much more. This is recommended watching for anyone who spends a lot of time in the debugger.

<iframe src="https://channel9.msdn.com/Shows/On-NET/Omer-Raviv-OzCode/player" width="960" height="540" allowFullScreen frameBorder="0"></iframe>

## Package of the week: Sprache

Parsing text is one of the most common tasks in computer science. Whether you need to build a small DSL, use an exotic data format, or use formatted input from your users, you'll need to describe how to transform text into a data structure.

There are fully-featured toolsets such as [ANTLR](http://www.antlr.org/) to perform such tasks, and many language grammars are defined with ANTLR, but for smaller parsing needs, they can be massively overkill.

[Sprache](https://github.com/sprache/Sprache) is a lightweight package to build simple parsers in C#. You can stop using regular expressions right now, and start building maintainable parsers instead ;)

```csharp
Parser<string> identifier =
    from leading in Parse.WhiteSpace.Many()
    from first in Parse.Letter.Once()
    from rest in Parse.LetterOrDigit.Many()
    from trailing in Parse.WhiteSpace.Many()
    select new string(first.Concat(rest).ToArray());

var id = identifier.Parse(" abc123  ");

Assert.AreEqual("abc123", id);
```

* [Sprache on GitHub](https://github.com/sprache/Sprache)
* [Sprache on NuGet](https://www.nuget.org/packages/Sprache)
* [Building a DSL using Sprache](https://nblumhardt.com/2010/01/building-an-external-dsl-in-c/) by Nicholas Blumhardt.

## Meetup of the week: F# for your day job in Durham, NC

If you've read a little about F# but haven't given it a serious try yourself, you might get the impression that it's good for people writing massively distributed systems or machine learning algorithms, but doesn't help with the kind of code you trudge through from 9 to 5. In fact, F# excels at expressing clean, simple solutions to the everyday problems encountered when developing business applications.

[TRINUG.NET](https://www.meetup.com/TRINUG/) [welcomes you on Wednesday, June 21 at 6:00PM](https://www.meetup.com/TRINUG/events/240556969/) to give you a tour of the distinctive features of F#, discovering each one by starting with a realistic example.

## .NET

* [Microsoft .NET Framework 4.7 is available on Windows Update, WSUS, and MU Catalog](https://blogs.msdn.microsoft.com/dotnet/2017/06/13/microsoft-net-framework-4-7-is-available-on-windows-update-wsus-and-mu-catalog/) by Jamshed Damkewala.
* [How the .NET Runtime loads a Type](http://mattwarren.org/2017/06/15/How-the-.NET-Rutime-loads-a-Type/) by Matt Warren.
* [Rider EAP 23: .NET Core debugger is back, Code Cleanup, and more](https://blog.jetbrains.com/dotnet/2017/06/16/rider-eap-23-net-core-debugger-back-code-cleanup/) by Jura Gorohovsky.
* [Automate Testing and Running Apps with dotnet watch](http://ardalis.com/automate-testing-and-running-apps-with-dotnet-watch) by Steve Smith.
* [One year of running the Windsor-Essex .NET Developers](https://codeopinion.com/one-year-of-running-a-windsor-essex-net-developers/) by Derek Comartin.
* [From Java to .Net Core, Part 2: Types](https://developers.redhat.com/blog/2017/06/15/from-java-to-net-core-part-2-types/) by Yev Bronshteyn.
* [AutoMapper 6.1.0 released](https://jimmybogard.com/automapper-6-1-0-released/) by Jimmy Bogard.
* [Consuming REST Services from Your Mobile Application Using Swagger and AutoRest](https://visualstudiomagazine.com/articles/2017/04/01/consuming-rest-services.aspx) by Nick Randolph.
* [.Net Core .csproj – Automatically Packaging README.txt](https://www.bengribaudo.com/blog/2017/06/19/3635/net-core-csproj-automatically-packaging-readme-txt) by Ben Gribaudo.
* [To Heap or not to Heap; That’s the Large Object Question?](https://www.codeproject.com/Articles/1191534/To-Heap-or-not-to-Heap-That-s-the-Large-Object-Que) by Doug Duerner.
* [Create a Free Private NuGet Server with Continuous Deployment using VSTS](https://www.devtrends.co.uk/blog/create-a-free-private-nuget-server-with-continuous-deployment-using-vsts) by Paul Hiles.
* [How to reference a .NET Core library in WinForms - Or, .NET Standard Explained](https://www.hanselman.com/blog/HowToReferenceANETCoreLibraryInWinFormsOrNETStandardExplained.aspx) by Scott Hanselman.

## ASP.NET

* [Automatically validating anti-forgery tokens in ASP.NET Core with the AutoValidateAntiforgeryTokenAttribute](https://andrewlock.net/automatically-validating-anti-forgery-tokens-in-asp-net-core-with-the-autovalidateantiforgerytokenattribute/) by Andrew Lock.
* [Minimal ASP.NET Core Web API project](https://www.meziantou.net/2017/06/19/minimal-asp-net-core-web-api-project) by Gérald Barré.
* [Docker for .NET Developers (Part 3) Why we started using Docker with ASP.NET Core](https://www.stevejgordon.co.uk/docker-for-dotnet-developers-part-3) and [Docker for .NET Developers (Part 4) Working with docker-compose and multiple ASP.NET Core microservices](https://www.stevejgordon.co.uk/docker-for-dotnet-developers-part-4) by Steve Gordon.
* [Resolving ASP.NET Core Startup class from the DI container](https://www.strathweb.com/2017/06/resolving-asp-net-core-startup-class-from-the-di-container/) by Filip W.
* [Integrating Microsoft Identity Authorization into a Menu System](https://www.danylkoweb.com/Blog/integrating-microsoft-identity-authorization-into-a-menu-system-IV) by Jonathan Danylko.
* [Middleware in ASP.NET Core – Handling requests](https://codingblast.com/asp-net-core-middleware/) by Ibrahim Šuta.
* [From 0 to 100 with this ASP.NET Core/AngularX Project Template](https://channel9.msdn.com/coding4fun/blog/From-0-to-100-with-this-ASPNET-CoreAngularX-Project-Template?WT.mc_id=DX_MVP4025064) by Greg Duncan.
* [Exploring GraphQL and creating a GraphQL endpoint in ASP.NET Core](http://asp.net-hacker.rocks/2017/05/29/graphql-and-aspnetcore.html) by Jürgen Gutsch.
* [Using Consul for Health Checks with ASP.NET Core](http://cecilphillip.com/using-consul-for-health-checks-with-asp-net-core/) by Cecil Phillip.
* [ASP.NET Core deployment using Docker, Nginx and Ubuntu Server](http://piotrgankiewicz.com/2017/06/12/asp-net-core-deployment-using-docker-nginx-and-ubuntu-server/) by Piotr Gankiewicz.
* [Options for Configuring ASP.NET Core Application Settings](http://rion.io/2017/06/10/options-for-configuring-asp-net-core-application-settings/) by Rion Williams.
* [Michael Ballhaus (MCPD + SCJP)](http://geekswithblogs.net/ballhaus/archive/2017/06/14/swaggerapi.aspx) by Michael Ballhaus.
* [10 Things To Know About In-Memory Caching In ASP.NET Core](http://www.binaryintellect.net/articles/a7d9edfd-1f86-45f8-a668-64cc86d8e248.aspx) by Bipin Joshi.

## C#

* [C# 7.x and 8.0: Uncertainty and Awesomeness](https://www.erikheemskerk.nl/c-sharp-7-2-and-8-0-uncertainty-awesomeness/) by Erik Heemskerk.
* [My Favorite C# 7 Feature: More expression-bodied members](http://motzcod.es/post/161630386432/my-favorite-c-7-feature-more-expression-bodied) by James Montemagno.
* [Practical C# – Extern Alias in C#](http://www.andreaangella.com/2017/06/practical-csharp-extern-alias/) by Andrea Angella.

## F#

* [Building Roguelike in F#](https://www.reaktor.com/blog/building-roguelike-in-f/) by Simo Raman.
* [Options in F#](https://dotnetcodr.com/2017/06/17/options-in-f/) by Andras Nemes.
* [Pulling state from F# Mailboxes](http://www.codingwithsam.com/pulling-state-from-f-mailboxes/) by Sam Williams.
* [The Foundations of Functional Concurrency is 37% off!](http://freecontent.manning.com/the-foundations-of-functional-concurrency/) by Manning and Riccardo Terrell.
* [When to Unit Test in F#](https://cockneycoder.wordpress.com/2017/06/12/when-to-unit-test-in-f/) by Isaac Abraham.

There is more content available this week in [F# Weekly](https://sergeytihon.wordpress.com/category/f-weekly/). If you want to see more F# awesomeness, please check it out!

## VB

* [Visual Basic and Cross-Platform: Mobile Apps with VB, Xamarin, and .NET Standard!](https://blogs.msdn.microsoft.com/vbteam/2017/06/13/visual-basic-and-cross-platform-mobile-apps-with-vb-xamarin-and-net-standard/) by Klaus Löffelmann.
* [Calling Web Services with HttpWebRequest, WebClient and HttpClient](https://visualstudiomagazine.com/articles/2017/06/01/calling-web-services.aspx) by Peter Vogel.
* [Viva, Visual Basic! Or, Does VB Have a Future?](https://visualstudiomagazine.com/articles/2017/06/13/visual-basic-future.aspx) by Michael Domingo.

## Xamarin

* [Stable Release: 15.2.3 Xamarin.VS Servicing Release](https://releases.xamarin.com/stable-release-15-2-3-xamarin-vs-servicing-release/) by Bri Brothers.
* [Xamarin Forms, the MVVMLight Toolkit and I: navigation and modal pages](https://msicc.net/xamarin-forms-the-mvvmlight-toolkit-and-i-navigation-and-modal-pages/) by Marco Siccardi.
* [Visual States in Xamarin.Forms using BuildIt.Forms](https://nicksnettravels.builttoroam.com/post/2017/06/10/Visual-States-in-XamarinForms-using-BuildItForms.aspx) by Nick Randolph.
* [Xamarin.Forms Visual States with View Models](https://nicksnettravels.builttoroam.com/post/2017/06/10/XamarinForms-Visual-States-with-View-Models.aspx) by Nick Randolph.
* [Chat Bot with Xamarin Forms](https://xamarinhelp.com/chat-bot-xamarin-forms/) by Adam Pedley.
* [Is Xamarin Forms Making Traditional Xamarin Obsolete?](https://xamarinhelp.com/xamarin-forms-making-traditional-xamarin-obsolete/) by Adam Pedley.
* [Xamarin Forms ChatBot Application using the Microsoft Bot Framework](http://xamarininterviewquestion.blogspot.com/2017/06/xamarin-forms-chatbot-application-using.html) by Suthahar J.
* [Boosting Your Productivity with MFractor](https://blog.verslu.is/tools/boosting-productivity-mfractor/) by Gerald Versluis.
* [5 Ways to Boost Xamarin.Forms App Startup Time](https://blog.xamarin.com/5-ways-boost-xamarin-forms-app-startup-time/) by David Ortinau.
* [Adding Face Tracking and Live Recognition to your Android App](https://blog.xamarin.com/adding-face-tracking-live-recognition-android-app/) by Nish Anil.
* [Demystifying Build Configurations](https://blog.xamarin.com/demystifying-build-configurations/) by Jon Goldberger.
* [Episode 25: Unity Game Development with Visual Studio for Mac with Jb Evain](https://channel9.msdn.com/Shows/XamarinShow/Episode-25-Unity-Game-Development-with-Visual-Studio-for-Mac-with-Jb-Evain) by The Xamarin Show.
* [Growing Your First Xamarin.Forms Mac App](https://codemilltech.com/growing-your-first-xamarin-forms-mac-app/) by Matthew Soucoup.
* [Things I Think Are Cool: Custom XAML Markup Extensions](https://codemilltech.com/things-i-think-are-cool-xaml-markup-extensions/) by Matthew Soucoup.
* [Embedding Xamarin.Forms into a Xamarin Native App #TheFuture](http://motzcod.es/post/161785997897/embedding-xamarinforms-into-a-xamarin-native-app) by James Montemagno.

## Azure

* [Azure via C# – Working with Azure Queues](http://www.andreaangella.com/2017/06/azure-via-csharp-azure-queues/) by Andrea Angella.
* [Writing an Azure Function in C# to create an ICS Calendar File](http://michaelcrump.net/building-an-ics-for-azure-functions-webinar/) by Michael Crump.
* [Creating ASP.NET Web App In Azure](http://www.c-sharpcorner.com/article/creating-asp-net-web-app-in-azure/) by Viral Jain.
* [AppVeyor – Continuous Delivery to Azure for your .NET Core Applications](https://dotnetcore.gaprogman.com/2017/06/15/appveyor-continuous-delivery-for-your-net-core-applications/) by Jamie Taylor.

## UWP

* [Adding Fluent Design Acrylic Material to UWP via Xamarin.Forms.](https://nicksnettravels.builttoroam.com/post/2017/06/11/Adding-Fluent-Design-Acrylic-Material-to-UWP-via-XamarinForms.aspx) by Nick Randolph.
* [iBeacons and UWP](https://channel9.msdn.com/coding4fun/blog/iBeacons-and-UWP) by Channel 9.
* [Setting up a HoloLens project with the HoloToolkit](http://dotnetbyexample.blogspot.com/2017/06/setting-up-hololens-project-with.html) by Joost van Schaik.

## Data

* [Introducing DataReaderAdapter: Adds AsDataReader()/AsDataReaderOfObjects() to IEnumerable<T>](https://www.bengribaudo.com/blog/2017/06/16/3612/introducing-datareaderadapter) by Ben Gribaudo.

## Game development

* [Getting Started with Duality - Part 1](https://channel9.msdn.com/Shows/dotGAME/Getting-Started-with-Duality--Part-1) by Stacey Haffner.
* [Attributes - Unity Tips](https://youtu.be/fxHOfV3yM9o) by GameGrind.
* [Unity 5.6 Tutorial: How to create a Maze Generator - part 1](https://youtu.be/lge09eYxVWE) by Gamad.
* [TDD in Unity - Heart Based Health System - Part 1](https://youtu.be/R1aO4Tmw3zA) by Infallible Code.
* [Real Time Strategy in Unity - AI Implementation : Economy](https://youtu.be/WiWD5SEVSBU) by Unit02Games.


And this is it for this week!

## Contribute to the week in .NET

As always, this weekly post couldn't exist without community contributions, and I'd like to thank all those who sent links and tips. The F# section is provided by [Phillip Carter](https://twitter.com/_cartermp), the gaming section by [Stacey Haffner](https://twitter.com/yecats131), the Xamarin section by [Dan Rigby](https://twitter.com/DanRigby), and the Azure and UWP section by [Michael Crump](http://twitter.com/mbcrump).

You can participate too. Did you write a great blog post, or just read one? Do you want everyone to know about an amazing new contribution or a useful library? Did you make or play a great game built on .NET?
We'd love to hear from you, and feature your contributions on future posts. Please [add your posts](https://weekindotnet.azurewebsites.net), it takes only a second.

We pick the articles based on the following criteria: the posts must be about .NET, they must have been published this week, and they must be original contents. Publication in Week in .NET is not an endorsement from Microsoft or the authors of this post.

This week's post (and future posts) also contains news I first read on [The ASP.NET Community Standup](https://blogs.msdn.microsoft.com/webdev/tag/communitystandup/), on [Weekly Xamarin](http://weeklyxamarin.com/), on [F# weekly](https://sergeytihon.wordpress.com/category/f-weekly/), and on [The Morning Brew](http://themorningbrew.net/).