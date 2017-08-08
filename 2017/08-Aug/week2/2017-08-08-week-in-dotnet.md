---
title: The week in .NET - Rezoom.SQL, Protobuf in Orlando, and links!
keywords: Week in .NET, community, .NET
weblogName: .NET Blog
dontInferFeaturedImage: true
---
Previous posts:

* [Nuke, Warden.NET, .NET in Bangalore](https://blogs.msdn.microsoft.com/dotnet/2017/08/01/the-week-in-net-nuke-warden-net-net-in-bangalore-and-links/)
* [MIST, F# in NYC](https://blogs.msdn.microsoft.com/dotnet/2017/07/26/the-week-in-net-mist-f-in-nyc-and-links/)
* [Command Line Parser Library, .NET South East](https://blogs.msdn.microsoft.com/dotnet/2017/07/18/the-week-in-net-command-line-parser-library-net-south-east/)

## Tool of the week: Rezoom.SQL

[Rezoom.SQL](https://github.com/rspeele/Rezoom.SQL) is an F# ORM for SQL databases.

It integrates with the F# compiler via a generative type provider to statically typecheck its own dialect of SQL. It knows how to translate this SQL dialect to various backends. Currently it supports SQLite, SQL Server, and PostgreSQL.

The type provider makes it fast and easy to write SQL statements, run them, and consume their results from your F# code with full type safety. You don't need to install any editor extensions or custom tooling, just add a NuGet package and you're off and running writing code like this:.

![Building a type from a SQL statement](https://github.com/rspeele/Rezoom.SQL/raw/master/doc/ReadmeResources/Queries.gif)

```fsharp
type ListUsers = SQL<"""
    select * from Users
""">

let showUsers() =
    use context = new ConnectionContext()
    let users = ListUsers.Command().Execute(context)
    printfn "There are %d users." users.Count
    for user in users do
        printfn "User ID %d's email is %s..." user.Id user.Email
        match user.Name with
        | None -> printfn "  and they don't have a name."
        | Some name -> printfn "  and their name is %s." name
```

* [Rezoom.SQL on GitHub](https://github.com/rspeele/Rezoom.SQL)
* [Rezoom.SQL on NuGet](https://www.nuget.org/packages/Rezoom.SQL.Provider/)


## User group meeting of the week: Protobuf in Orlando

Protocol Buffers is a method of serializing structured data. It is useful in developing programs to communicate with each other over a wire or for storing data. Join [the Orlando .NET User Group](https://www.meetup.com/ONETUG/) [on Thursday, August 10 at 6:00PM](https://www.meetup.com/ONETUG/events/241115418/) to learn how to apply Protobuf in .NET.

## .NET

* [Welcome to the .NET Framework 4.7.1 Early Access!](https://blogs.msdn.microsoft.com/dotnet/2017/08/07/welcome-to-the-net-framework-4-7-1-early-access/) by Preeti Krishna.
* [.NET Framework July 2017 Cumulative Quality Update for Windows 10](https://blogs.msdn.microsoft.com/dotnet/2017/08/01/net-framework-july-2017-cumulative-quality-update-for-windows-10/) by Rich Lander.
* [Visual Studio Toolbox: .NET Core 2.0 – Preview 2](https://blogs.msdn.microsoft.com/robertgreen/2017/08/04/visual-studio-toolbox-net-core-2-0-preview-2/) by Rich Lander.
* [Visual Studio for Mac](https://blogs.msdn.microsoft.com/uk_faculty_connection/2017/08/02/visual-studio-for-mac-2/) by Lee Stott.
* [Visual Studio for Mac version 7.2 Alpha Preview](https://blogs.msdn.microsoft.com/visualstudio/2017/08/02/visual-studio-for-mac-version-7-2-alpha-preview/) by Miguel de Icaza.
* [Rider 2017.1 – JetBrains .NET IDE hits RTM](https://blog.jetbrains.com/dotnet/2017/08/03/rider-2017-1-jetbrains-net-ide-hits-rtm/) by Jura Gorohovsky.
* [Introduction to .NET Docker Images](https://channel9.msdn.com/Shows/Code-Conversations/Introduction-to-NET-Docker-Images-with-Kendra-Havens) by Kendra Havens.
* [Microservices and Docker containers: Architecture, Patterns and Development guidance](https://blogs.msdn.microsoft.com/dotnet/2017/08/02/microservices-and-docker-containers-architecture-patterns-and-development-guidance/) by Cesar de la Torre.
* [How we did (and did not) improve performance and efficiency in Marten 2.0](https://jeremydmiller.com/2017/08/01/how-we-did-and-did-not-improve-performance-and-efficiency-in-marten-2-0/) by Jeremy D Miller.
* [Using .NET Core 2 on Raspbian Jessie to read serial data from an Arduino](https://jeremylindsayni.wordpress.com/2017/08/07/using-net-core-2-on-raspbian-jessie-to-read-serial-data-from-an-arduino/) by Jeremy Lindsay.
* [AutoMapper joins the .NET Foundation](https://jimmybogard.com/automapper-joins-the-net-foundation/) by Jimmy Bogard.
* [Converting a Library to NetStandard](https://codeopinion.com/converting-a-library-to-netstandard/) by Derek Comartin.
* [Porting to .Net Standard 2.0 Introduction](http://blog.softwarepotential.com/porting-to-net-standard-2-0-introduction/), [Part 1: Project Setup](http://blog.softwarepotential.com/porting-to-net-standard-2-0-part-1/), and [Part 2: Porting MEF 1.0 to MEF 2.0 on .Net Core](http://blog.softwarepotential.com/porting-to-net-standard-2-0-part-2-porting-mef-1-0-to-mef-2-0-on-net-core/) by Siobhan Connell.
* [ClrMD Part 6 – Manipulate memory structures like real objects](http://labs.criteo.com/2017/08/clrmd-part-6-manipulate-memory-structures-like-real-objects/) by Christophe Nasarre & Kevin Gosse.
* [How did I reduce the docker image of SimplCommerce from 900M to 145M using multi-stage builds](http://thienn.com/aspnetcore-docker-multi-stage-builds-simplcommerce/) by Thien Nguyen.
* [Our experience with using third-party libraries](https://blog.ndepend.com/experience-using-third-party-libraries/) by Patrick Smacchia.

## ASP.NET

* [ASP.NET Core and 502 Bad Gateway Response](https://blogs.msdn.microsoft.com/wushuai/2017/08/04/asp-net-core-and-502-bad-gateway-response/) by Wu Shuai.
* [Getting Started With ASP.NET Core MVC](http://l-knowtech.com/2017/08/03/getting-started-asp-net-core-mvc/) by Sandeep Shekhawat.
* [How to format response data as XML or JSON, based on the request URL in ASP.NET Core](https://andrewlock.net/formatting-response-data-as-xml-or-json-based-on-the-url-in-asp-net-core/) by Andrew Lock.
* [Structured Logging with Application Insights](https://aspnetmonsters.com/2017/08/monsters-weekly/ep102/) by ASP.NET Monsters.
* [Custom response caching in ASP.NET Core (with cache invalidation)](https://www.devtrends.co.uk/blog/custom-response-caching-in-asp.net-core-with-cache-invalidation) by Paul Hiles.
* [Develop Locally with HTTPS, Self-Signed Certificates and ASP.NET Core](https://www.humankode.com/asp-net-core/develop-locally-with-https-self-signed-certificates-and-asp-net-core) by Carlo van Wyk.
* [ASP.NET Core SignalR – Simple chat](https://codingblast.com/asp-net-core-signalr-simple-chat/) by CodingBlast.
* [.NET Core Middleware – OWASP Headers Part 2 – Configuration](https://dotnetcore.gaprogman.com/2017/08/03/net-core-middleware-configuration-options/) by Jamie Taylor.
* [Converting C# enums to JavaScript](http://gunnarpeipman.com/2017/08/enum-to-javascript/) by Gunnar Peipman.
* [Creating Themes for ASP.NET Web Core](https://www.poppastring.com/blog/CreatingThemesForASPNETWebCore.aspx) by Mark Downie.
* [Why, When and How to use Redis in ASP.NET MVC Core](https://garywoodfine.com/why-when-and-how-to-use-redis-in-asp-net-mvc-core/) by Gary Woodfine.
* [ASP.NET Core, response compression, response buffering and subtle difference between .NET Framework and .NET Core](https://www.tpeczek.com/2017/08/aspnet-core-response-compression.html) by Tomasz Pęczek.

## C#

* [Practical C# – SelectMany in LINQ](http://www.andreaangella.com/2017/08/practical-c-implementing-equality/) by Andrea Angella.
* [Practical C# – Named and Optional Parameters](http://www.andreaangella.com/2017/08/practical-csharp-optional-parameters/) by Andrea Angella.
* [A look at the internals of 'boxing' in the CLR](http://mattwarren.org/2017/08/02/A-look-at-the-internals-of-boxing-in-the-CLR/) by Matt Warren.
* [Declare Out variable right at the point – Out variable in C# 7.0](http://dailydotnettips.com/2017/08/01/declare-out-variable-right-at-the-point-out-variable-in-c-7-0/) by Abhijit Jana.
* [Using Visual Studio Code for C# (.NET Core) development](http://www.jerriepelser.com/blog/using-vscode-for-csharp-development/) by Jerrie Pelser.

## F#

* [F# Hacking - Fable, Ionide, FSAC.](https://www.youtube.com/watch?v=yTrcY8jGFmg) by Krzyzstof Cieslak.
* [Hey F#, load me this CSV file into a table!](https://medium.com/@edgarsanchezg/hey-f-load-me-this-csv-file-into-a-table-14c60a3b0842) by Edgar Sánchez.
* [Exoplanet Exploration with F# and MongoDb: Part 1](https://medium.com/@mukund.sharma92/exoplanet-exploration-with-f-and-mongodb-part-1-3a20d7a3e32e) by Moko Sharma.
* [Class-less Coding - Minimalist C# and Why F# and Function Programming Has Some Advantages](https://www.codeproject.com/Articles/1200375/Class-less-Coding-Minimalist-Csharp-and-Why-Fsharp) by Marc Clifton.
* [Extending folds for trees (F#)](https://blainne.github.io/2017/07/23/two-acc-cata/) by Grzegorz Sławecki.
* [GUI animations: async/await to F#’s Async](http://www.codingwithsam.com/gui-animations-asyncawait-to-fs-async/) by Sam WIlliams.

There is more content available this week in [F# Weekly](https://sergeytihon.wordpress.com/category/f-weekly/). If you want to see more F# awesomeness, please check it out!

## Xamarin

* [Exrin MVVM Operations – Write Less Code](https://xamarinhelp.com/exrin-mvvm-operations-write-less-code/) by Adam Pedley.
* [Mobile Database Bi-Directional Synchronization with a REST API](https://xamarinhelp.com/mobile-database-bi-directional-synchronization-rest-api/) by Adam Pedley.
* [Prism in Xamarin Forms Step by Step (Part. 3)](https://xamgirl.com/prism-in-xamarin-forms-step-by-step-part-3/) by Charlin Agramonte.
* [Learning Xamarin.Forms – Part 5: ListView](http://jesseliberty.com/2017/07/26/learning-xamarin-forms-part-5-listview/) by Jesse Liberty.
* [Transient Fault Handling in Xamarin.Forms using Polly](http://www.davidbritch.com/2017/07/transient-fault-handling-in.html) by David Britch.
* [Transient Fault Handling in Xamarin.Forms](http://www.davidbritch.com/2017/07/transient-fault-handling-in-xamarinforms.html) by David Britch.
* [MSBuild Basics](http://www.jon-douglas.com/2017/07/26/msbuild-basics/) by Jon Douglas.
* [Being Reactive](http://www.jon-douglas.com/2017/08/01/being-more-reactive/) by Jon Douglas.
* [LiveXAML for Xamarin Forms](http://www.livexaml.com/) by LiveXAML.
* [Xamarin.Tip – Borderless TimePicker](https://alexdunn.org/2017/07/26/xamarin-tip-borderless-timepicker/) by Alex Dunn.
* [Xamarin.Tip – BottomNavigationView in Xamarin.Android](https://alexdunn.org/2017/07/26/xamarin-tip-bottomnavigationview-in-xamarin-android/) by Alex Dunn.
* [Xamarin.Tip – Borderless Editor](https://alexdunn.org/2017/07/27/xamarin-tip-borderless-editor/) by Alex Dunn.
* [Xamarin.Tip – Playing Audio Through the Earpiece in iOS](https://alexdunn.org/2017/07/27/xamarin-tip-playing-audio-through-the-earpiece-in-ios/) by Alex Dunn.
* [Xamarin.Tip – Borderless Inputs](https://alexdunn.org/2017/07/28/xamarin-tip-borderless-inputs/) by Alex Dunn.
* [Xamarin.Tip – Playing Audio Through the Earpiece on Android](https://alexdunn.org/2017/07/31/xamarin-tip-playing-audio-through-the-earpiece-on-android/) by Alex Dunn.
* [Xamarin.University – Guest Lecture Available for Free!](https://alexdunn.org/2017/08/01/xamarin-university-guest-lecture-available-for-free/) by Alex Dunn.
* [Join Us for Upcoming Xamarin University Guest Lectures](https://blog.xamarin.com/join-us-upcoming-xamarin-university-guest-lectures/) by Rob Gibbens.
* [Xamarin Podcast: Building Apps with iOS 11, Visual Studio for Mac, and Mobile Center](https://blog.xamarin.com/podcast-building-apps-ios-11-visual-studio-mac-mobile-center/) by Pierce Boggan.
* [Pragma Delivers High Performance Apps Field Service Employees Love](https://blog.xamarin.com/pragma-delivers-high-performance-apps-field-service-employees-love/) by Lacey Butler.
* [Xamarin Events in August](https://blog.xamarin.com/xamarin-events-august/) by Jayme Singleton.
* [Working UrhoSharp with Xamarin Workbooks](https://channel9.msdn.com/coding4fun/blog/Working-UrhoSharp-with-Xamarin-Workbooks) by Greg Duncan.
* [Building Your First .NET Core App in Visual Studio for Mac](https://channel9.msdn.com/Shows/XamarinShow/Snack-Pack-17-Building-Your-First-NET-Core-App-in-Visual-Studio-for-Mac) by The Xamarin Show.
* [Xamarin Beta Release: 15.3 Preview 6](https://releases.xamarin.com/beta-release-15-3-preview-6/) by Bri Brothers.
* [Xamarin Preview: Xcode 9 beta 4, iOS 11, macOS 10.13 support – Preview 2](https://releases.xamarin.com/preview-xcode-9-beta-4-ios-11-macos-10-13-support-preview-2/) by Bri Brothers.
* [A Filtered View of Core Image](https://visualstudiomagazine.com/articles/2017/07/01/core-image.aspx) by Wallace McClure.
* [Powershell and Azure on MacOS](https://www.devprotocol.com/powershell-and-azure-on-macos/) by Jan Tourlamain.

## Azure

* [Read Azure Service Health Activity Logs with .NET Core](https://carlos.mendible.com/2017/08/04/read-azure-service-health-activity-logs-with-net-core/) by Carlos Mendible.
* [Change feed: Event Sourcing with Cosmos DB](https://azure.microsoft.com/en-us/blog/introducing-the-azure-cosmosdb-change-feed-processor-library/) by Judy Hanwen Shen.
* [.NET Native App accessing Web Service that calls a downstream Web API with Conditional Access](https://azure.microsoft.com/en-us/resources/samples/active-directory-dotnet-webapi-onbehalfof-ca/) by Jean-Marc Prieur.
* [Get started with Azure Data Catalog](https://azure.microsoft.com/en-us/resources/samples/data-catalog-dotnet-get-started/) by Derrick VanArnam.
* [Read and write from EventHubs using a hybrid .NET/Java Storm topology](https://azure.microsoft.com/en-us/resources/samples/hdinsight-dotnet-java-storm-eventhub/) by Larry Franks.
* [Copy blobs into an Azure Media Services asset](https://azure.microsoft.com/en-us/resources/samples/media-services-dotnet-copy-blob-into-asset/) by Julia Kornich.
* [ASP.NET and ASP.NET Core Application Restarts on Azure App Service.](https://blogs.msdn.microsoft.com/waws/2017/08/02/asp-net-and-asp-net-core-application-restarts-on-azure-app-service/) by Finbar Ryan.

## UWP

* [UWP Tip #4 - UWP Community Toolkit - Part 2, Consuming the Services](http://www.uwpapp.tips/2017/08/uwp-tip-4-uwp-community-toolkit-part-2.html) by Alvin Ashcraft.
* [#WINDOWS10 – UWP Community Toolkit 1.5. New BlueTooth LE features, new controls and more …](https://elbruno.com/2017/07/28/windows10-uwp-community-toolkit-1-5-new-bluetooth-le-features-new-controls-and-more/) by elbruno.
* [#Hololens – Lunar Module, new sample App with some very cool PreFabs for motion controllers](https://elbruno.com/2017/07/31/hololens-lunar-module-new-sample-app-with-some-very-cool-prefabs-for-motion-controllers/) by elbruno.
* [#Hololens – Tutorial to use Buttons, Dialogs and more with #MRDesignLab (#HoloToolkit ++)](https://elbruno.com/2017/08/02/hololens-tutorial-to-use-buttons-dialogs-and-more-with-mrdesignlab-holotoolkit/) by elbruno.
* [Configure your app to start at log-in](https://blogs.windows.com/buildingapps/2017/08/01/configure-app-start-log/#Vgjb2XtDMcbhtqhe.97) by Andrew Whitechapel.
* [Creating Materials and Lights in the Visual Layer](https://blogs.windows.com/buildingapps/2017/08/04/creating-materials-lights-visual-layer/#tETvMdUFRhg1s5Q3.97) by Windows UI Team.
* [Exploring Windows Mixed Reality, switching between 2D / 3D and embedding Web Views](http://www.davidezordan.net/blog/?p=8183) by Davide Zordan.

## Data

* [Using SQL Server Query Hints with Entity Framework](https://www.red-gate.com/simple-talk/dotnet/net-development/using-sql-server-query-hints-entity-framework/) by Dennes Torres.

## Game development

* [Unity 2017.1 feature spotlight: Playable API](https://blogs.unity3d.com/2017/08/02/unity-2017-1-feature-spotlight-playable-api/) by Pierre Paul Giroux.
* [Nested Coroutines in Unity](http://www.alanzucconi.com/2017/02/15/nested-coroutines-in-unity/) by Alan Zucconi.
* [Unity- How to make a 3d runner part 4? - Spawning algorithm](https://youtu.be/EGc3kOkOn_o) by Unity Hour.
* [13.5 Unity Tower Defense Tutorial - Final](https://youtu.be/ZQhRo3TmcrY) by inScope Studios.
* [Using Streaming Assets in Unity](https://www.raywenderlich.com/165809/using-streaming-assets-unity) by Mark Placzek.

And this is it for this week!

## Contribute to the week in .NET

As always, this weekly post couldn't exist without community contributions, and I'd like to thank all those who sent links and tips. The F# section is provided by [Phillip Carter](https://twitter.com/_cartermp), the gaming section by [Stacey Haffner](https://twitter.com/yecats131), the Xamarin section by [Dan Rigby](https://twitter.com/DanRigby), and the Azure and UWP section by [Michael Crump](http://twitter.com/mbcrump).

You can participate too. Did you write a great blog post, or just read one? Do you want everyone to know about an amazing new contribution or a useful library? Did you make or play a great game built on .NET?
We'd love to hear from you, and feature your contributions on future posts. Please [add your posts](https://weekindotnet.azurewebsites.net), it takes only a second.

We pick the articles based on the following criteria: the posts must be about .NET, they must have been published this week, and they must be original contents. Publication in Week in .NET is not an endorsement from Microsoft or the authors of this post.

This week's post (and future posts) also contains news I first read on [The ASP.NET Community Standup](https://blogs.msdn.microsoft.com/webdev/tag/communitystandup/), on [Weekly Xamarin](http://weeklyxamarin.com/), on [F# weekly](https://sergeytihon.wordpress.com/category/f-weekly/), and on [The Morning Brew](http://themorningbrew.net/).