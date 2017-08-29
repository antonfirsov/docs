---
title: The week in .NET - 
keywords: Week in .NET, community, .NET
weblogName: .NET Blog
dontInferFeaturedImage: true
---
Previous posts:

* [Project File Tools, Maira Wenzel, Mixed Reality in Miami](https://blogs.msdn.microsoft.com/dotnet/2017/08/23/the-week-in-net-project-file-tools-maira-wenzel-mixed-reality-in-miami-and-links/)
* [.NET Core 2.0, ASP.NET Core 2.0, Entity Framework 2.0, Visual Studio 2017 Update 3, enterprise Entity Framework Core in Boston](https://blogs.msdn.microsoft.com/dotnet/2017/08/15/the-week-in-net-net-core-2-0-asp-net-core-2-0-entity-framework-2-0-visual-studio-2017-update-3-enterprise-entity-framework-core-in-boston-and-links/)
* [Rezoom.SQL, Protobuf in Orlando](https://blogs.msdn.microsoft.com/dotnet/2017/08/08/the-week-in-net-rezoom-sql-protobuf-in-orlando-and-links/)

## Open-source project of the week: Let's Encrypt Azure Web App Renewer

There's a growing consensus that all web sites should transition now to be encrypted with HTTPS. This comes at a cost, and with the friction associated with acquiring and renewing certificates. The problem of cost is now addressed by [Let's Encrypt](https://letsencrypt.org/), but the friction is still there.

[Ohad Schneider](https://github.com/ohadschn) has built [Let's Encrypt WebApp Renewer, a great Azure WebJob-ready console application built in C#](https://github.com/ohadschn/letsencrypt-webapp-renewer). The application builds on an existing Azure extension, [letsencrypt-siteextension](https://github.com/sjkp/letsencrypt-siteextension), with the following advantages:

* Installs on any Web App, not necessarily the same one you're managing.
* Support for multiple Web Apps.
* Built-in email notifications.
* No eternal dependencies other than Let's Encrypt.
* Can be used in any environment.

Check out [Let's Encrypt Web App Renewer on GitHub](https://github.com/sjkp/letsencrypt-siteextension)!

## User group meeting of the week: Seattle CodeCamp 2017

Code Camps are free, one day learning events for programming professionals and students embracing a wide variety of technologies. They're also "grass roots" mini application, cloud, and mobile platform developer conferences, free of charge to attendees and open to presenters of all stripes and experience levels.

[Seattle CodeCamp 2017](https://www.meetup.com/NET-Developers-Association-Westside/events/242572767/) is On Saturday, September 9 from 8:00 AM to 5:45 PM.

## .NET

* [ClrMD Part 7 – Manipulate nested structs using dynamic](http://labs.criteo.com/2017/08/clrmd-part-7-manipulate-nested-structs-using-dynamic/) by Kevin Gosse & Christophe Nasarre.
* [Zero Garbage Collector for .NET Core](http://tooslowexception.com/zero-garbage-collector-for-net-core/) by Konrad Kokosa.
* [FiddlerCore for .NET Standard and Fiddler Orchestra—the Future of Fiddler](http://www.telerik.com/blogs/fiddlercore-for-net-standard-and-fiddler-orchestra-the-future-of-fiddler) by Tsviatko Yovtchev.
* [Automate Windows Desktop Apps with WebDriver- WinAppDriver](https://automatetheplanet.com/automate-windows-desktop-apps-winappdriver/) by Anton Angelov.
* [It looks like Ayende doesn't really like JavaScript that much](https://ayende.com/blog/179553/with-performance-test-benchmark-and-be-ready-to-back-out) by Ayende Rahien.
* [.NET Core 2.0 is Ready and Sterling Proves It!](https://blog.jeremylikness.com/https-blog-jeremylikness-com-net-core-2-0-is-ready-and-sterling-proves-it-41350afd18a9) by Jeremy Likness.
* [Visual Studio Test Platform – upcoming changes to data collectors](https://blogs.msdn.microsoft.com/devops/2017/08/24/visual-studio-test-platform-upcoming-changes-to-data-collectors/) by Pratap Lakshman.
* [How to detect on a WCF Service that a client just got disconnected?](https://blogs.msdn.microsoft.com/whereismysolution/2017/08/23/how-to-detect-on-a-wcf-service-that-a-client-just-got-disconnected/) by José Paulo Pendão.
* [.NET Core, Code Analysis and StyleCop](https://carlos.mendible.com/2017/08/24/net-core-code-analysis-and-stylecop/) by Carlos Mendible.
* [Your first serverless .NET function with OpenFaaS](https://medium.com/@rorpage/your-first-serverless-net-function-with-openfaas-27573017dedb) by Robbie Page.
* [Developers Pit .NET Core 2.0 Performance Against Java, Go](https://visualstudiomagazine.com/articles/2017/08/22/net-core-benchmarking.aspx) by David Ramel.
* [Understanding .NET Standard - An Interface Not An Implementation](https://www.danielcrabtree.com/blog/303/understanding-net-standard-an-interface-not-an-implementation) by Daniel Crabtree.

## ASP.NET

* [.NET Core Middleware – OWASP Headers Part 3 – Adding Unit Tests And More Headers](https://dotnetcore.gaprogman.com/2017/08/24/net-core-middleware-owasp-headers-part-3-finishing-what-we-started/) by Jamie Taylor.
* [Full Page Screenshots in WebDriver via Custom-built Browser Extension](https://automatetheplanet.com/build-browser-extension-full-page-screenshots/) by Anton Angelov.
* [Capture Full Page Screenshots Using WebDriver with HTML2Canvas.js](https://automatetheplanet.com/full-page-screenshots-webdriver-html2canvas/) by Anton Angelov.
* [Using CancellationTokens in ASP.NET Core MVC controllers](https://andrewlock.net/using-cancellationtokens-in-asp-net-core-mvc-controllers/) by Andrew Lock.
* [Debugging ASP.NET Core Routes](https://ardalis.com/debugging-aspnet-core-routes) by Steve Smith.
* [Consuming ASP.NET Web API REST Service In ASP.NET MVC Using HttpClient](http://www.c-sharpcorner.com/article/consuming-asp-net-web-api-rest-service-in-asp-net-mvc-using-http-client/) by Vithal Wadje.
* [Design Patterns: Asp.Net Core Web API, services, and repositories | Part 5: Repositories, the ClanRepository, and integration testing](http://www.forevolve.com/en/articles/2017/08/25/design-patterns-web-api-service-and-repository-part-5/) by Carl-Hugo Marcotte.
* [NodeServices: Where Javascript and .NET Meet Back on the Other Side](http://rion.io/2017/08/22/nodeservices-where-javascript-and-net-meet-back-on-the-other-side/) by Rion Williams.
* [First CRUD Application In ASP.NET Core MVC Using Entity Framework Core](http://l-knowtech.com/2017/08/28/first-crud-application-asp-net-core-mvc-using-entity-framework-core/) by Sandeep Shekhawat.
* [Implementing database per tenant strategy on ASP.NET Core](http://gunnarpeipman.com/2017/08/database-per-tenant/) by Gunnar Peipman.
* [Handling missing tenants in ASP.NET Core](http://gunnarpeipman.com/2017/08/missing-tenant-middleware/) by Gunnar Peipman.
* [Implementing tenant providers on ASP.NET Core](http://gunnarpeipman.com/2017/08/tenant-providers/) by Gunnar Peipman.
* [Setting a Custom Default Page in ASP.NET Core Razor Pages](https://exceptionnotfound.net/setting-a-custom-default-page-in-asp-net-core-razor-pages/) by Matthew Jones.
* [Create a CI/CD pipeline for containerized ASP.NET Core projects](https://blogs.msdn.microsoft.com/visualstudio/2017/08/22/create-a-cicd-pipeline-for-containerized-asp-net-core-projects/) by Ahmed Metwally.

## C#

* [C# Functional Programming: Simple Use Case](https://blog.jamesmichaelhickey.com/csharp-functional-programming-a-simple-use-case/) by James Hickey.
* [Lazy Async](https://codeopinion.com/lazy-async/) by Derek Comartin.
* [C# 8.0 Previewed](https://www.infoq.com/news/2017/08/CSharp-8) by Jonathan Allen.
* [Let's learn about default literal in C# 7.1](http://www.kunal-chowdhury.com/2017/08/default-literal-in-csharp-7.1.html#W0OhjyI81HwApcTa.97) by Kunal Chowdhury.
* [The Care and Feeding of Tuples in C#](https://www.red-gate.com/simple-talk/dotnet/c-programming/care-feeding-tuples-c/) by Tom Fischer.

## F#

* [There is no such thing as a free Free monad](https://finai.com/en/newsroom/there-is-no-such-thing-as-a-free-free-monad/) by  Marcin Malinowski .
* [My reasons to love F#](https://functional.works-hub.com/blog/My-reasons-to-love-F-) by Lena Hall.
* [Solving HackerRank’s Functional Challenges in F#: Solve Me First FP](https://alexatnet.com/hr-fp-solve-me-first/) by Alex Netkachov.
* [Episode 263 | Ody Mbegbu - Shipping Value](http://developeronfire.com/podcast/episode-263-ody-mbegbu-shipping-value) by Developer On Fire.
* [On the Significance of Recursion](https://eiriktsarpalis.wordpress.com/2017/08/20/on-the-significance-of-recursion/) by Eirik Tsarpalis.
* [Trying .NET Core 2.0 With F# Today](https://blog.mavnn.co.uk/trying-dotnetcore-2-dot-0-with-f-number-today/) by mavnn.

There is more content available this week in [F# Weekly](https://sergeytihon.wordpress.com/category/f-weekly/). If you want to see more F# awesomeness, please check it out!

## Xamarin

* [Snack Pack 18: Using Effects in Xamarin.Forms](https://channel9.msdn.com/Shows/XamarinShow/Snack-Pack-18-Using-Effects-in-XamarinForms) by The Xamarin Show.
* [Snack Pack 19: Serverless Compute in 5 Minutes](https://channel9.msdn.com/Shows/XamarinShow/Snack-Pack-19-Serverless-Compute-in-5-Minutes) by The Xamarin Show.
* [Introduction to UI Testing with Xamarin](https://www.jimbobbennett.io/ui-testing-your-xamarin-apps/) by Jim Bennett.
* [Integrating Xamarin Test Recorder for Mac and Xamarin Test Cloud](https://mindofai.github.io/Integrating-Xamarin-Test-Recorder-for-Mac-and-Xamarin-Test-Cloud/) by Bryan Anthony Garcia.
* [Pre-release: Xamarin.Forms 2.4.0.266-pre1](https://releases.xamarin.com/pre-release-xamarin-forms-2-4-0-266-pre1/) by David Ortinau.
* [Preview: Xcode 9 Beta 5, iOS 11, MacOS 10.13 Support – Preview 5](https://releases.xamarin.com/preview-xcode-9-beta-5-ios-11-macos-10-13-support-preview-5/) by Brendan Zagaeski.
* [Stable Release: 15.3.2 Servicing Release](https://releases.xamarin.com/stable-release-15-3-2/) by Brendan Zagaeski.
* [Technical Bulletin: Visual Studio 2017 version 15.4.0 Preview 1](https://releases.xamarin.com/technical-bulletin-visual-studio-2017-version-15-4-0-preview-1/) by Brendan Zagaeski.
* [Mobile Apps with Xamarin.Forms: Architecture and Patterns guidance](https://blogs.msdn.microsoft.com/dotnet/2017/08/25/xamarin-patterns/) by Cesar de la Torre.
* [Creating flight-mode safe Xamarin apps with Akavache](https://blog.verslu.is/xamarin/xamarin-forms-xamarin/using-akavache-xamarin/) by Gerald Versluis.
* [Fast & Simple Android Location Updates with Google Play services](https://blog.xamarin.com/fast-simple-android-location-updates-with-google-play-services/) by James Montemagno.
* [Native Android Facebook Authentication with Azure App Service](https://blog.xamarin.com/native-android-facebook-authentication-azure-app-service/) by James Montemagno.
* [Speech Central Makes it Easier to Browse the Internet Hands-Free](https://blog.xamarin.com/speech-central-makes-easier-browse-internet-hands-free/) by Lacey Butler.
* [Symbolicating iOS crashes](https://basdecort.com/2017/08/17/symbolicating-ios-crashes/) by Bas de Cort.
* [Xamarin.Tip – Read All Contacts in iOS](https://alexdunn.org/2017/08/21/xamarin-tip-read-all-contacts-in-ios/) by Alex Dunn.
* [Popped Pages in Xamarin Forms](http://www.johankarlsson.net/2017/08/popped-pages-in-xamarin-forms.html) by Johan Karlsson.
* [Being Reactive with Xamarin.Android](http://www.jon-douglas.com/2017/08/16/being-reactive-xamarin-android/) by Jon Douglas.
* [Xamarin Tools and Resources](http://www.kymphillpotts.com/xamarin-tools-and-resources/) by Kym Phillpotts.
* [.NET Core Support in Visual Studio for Mac 7.1](http://lastexitcode.com/blog/2017/08/23/NetCoreSupportInVisualStudioMac7-1/) by Matt Ward.
* [Removing BottomNavigationView’s Icon Shifting  in Xamarin.Android](http://motzcod.es/post/164336484097/remove-shifting-bottomnavigationview-android) by James Montemagno.
* [MvvmCross Updates with Martijn van Dijk](http://gonemobile.io/blog/e0056.mvvmcross.updates.with.martijn.van.dijk/) by Gone Mobile Podcast.
* [Xamarin Forms: Entry View Control](http://inquisitorjax.blogspot.com/2017/08/xamarin-forms-entry-view-control.html) by Malcolm Jack.
* [Checking out Mobile Center](https://www.thewissen.io/checking-out-mobile-center/) by Steven Thewissen.
* [Creating a good-looking Xamarin Forms UI: Twitter](https://www.thewissen.io/xamarin-forms-ui-twitter/) by Steven Thewissen.
* [Reducing App File Size In Xamarin.Forms](https://xamarinhelp.com/reducing-app-file-size-xamarin-forms/) by Adam Pedley.
* [Xamarin.Forms OnPlatform And RuntimePlatform](https://xamarinhelp.com/xamarin-forms-onplatform-runtimeplatform/) by Adam Pedley.
* [Clearable DatePicker in Xamarin Forms](https://xamgirl.com/clearable-datepicker-in-xamarin-forms/) by Charlin Agramonte.
* [Prism in Xamarin Forms Step by Step (Part. 4)](https://xamgirl.com/prism-in-xamarin-forms-step-by-step-part-4/) by Charlin Agramonte.

## Azure

* [Azure via C# – Azure Functions in C#](https://www.productivecsharp.com/2017/08/azure-via-csharp-azure-functions/) by Andrea Angella.
* [Azure for .NET Developers - Drag Tiles to customize your Azure Dashboard](http://michaelcrump.net/azure-tips-and-tricks3/) by Michael Crump.
* [Azure for .NET Developers - Customize and Pin Charts to your Azure Dashboard](http://michaelcrump.net/azure-tips-and-tricks4/) by Michael Crump.
* [Azure for .NET Developers - Custom Tile Sizes in the Azure Dashboard](http://michaelcrump.net/azure-tips-and-tricks5/) by Michael Crump.
* [Querying Azure Cosmos DB's Graph API using an Azure Function](http://www.bradygaster.com/posts/azure-cosmosdb-with-functions) by Brady Gaster.
* [Microsoft Cognitive Services Translation API with Python & C#](https://blogs.msdn.microsoft.com/uk_faculty_connection/2017/08/23/microsoft-cognitive-services-translation-api-with-python-c/) by Lee Stott.
* [Azure Data Lake DotNet Client Sample](https://azure.microsoft.com/en-us/resources/samples/data-lake-dotnet-client/) by Azure Samples.
* [Intelligent Mission Sample App for Cognitive Services for .NET ](https://azure.microsoft.com/en-us/resources/samples/gov-intelligent-mission/) by Steve Michelotti.
* [Using PlayReady and/or Widevine Dynamic Common Encryption with .NET](https://azure.microsoft.com/en-us/resources/samples/media-services-dotnet-dynamic-encryption-with-drm/) by Julia Kornich.
* [Encode and Deliver a Live Stream with Azure Media Services using .NET SDK](https://azure.microsoft.com/en-us/resources/samples/media-services-dotnet-encode-live-stream-with-ams-clear/) by Julia Kornich.

## UWP

* [UWP App Tips](http://www.uwpapp.tips/2017/08/uwp-tip-5-uwp-community-toolkit-part-3.html) by Alvin Ashcraft.
* [Windows 10 SDK Preview Build 16267 and Mobile Emulator Build 15240 Released](https://blogs.windows.com/buildingapps/2017/08/22/windows-10-sdk-preview-build-16267-mobile-emulator-build-15240-released/#2eQXWEwjLo4UpiRB.97) by Clint Rutkas.
* [UWP & .NET Standard 2.0: A preview is now available!](https://blogs.msdn.microsoft.com/dotnet/2017/08/25/uwp-net-standard-2-0-a-preview-is-now-available/) by Immo Landwerth.
* [UWP & .NET Standard 2.0: A preview is now available!](https://blogs.msdn.microsoft.com/dotnet/2017/08/25/uwp-net-standard-2-0-preview/) by Immo Landwerth.
* [API reference for Universal Windows Platform (UWP) apps](https://docs.microsoft.com/en-us/uwp/) by Windows Doc Team.

## Game development

* [Unity for Xbox One X](https://blogs.unity3d.com/2017/08/14/unity-for-xbox-one-x/) by Oli Williams.
* [Verifying the scripting docs – Fun with EditorTests](https://blogs.unity3d.com/2017/08/18/verifying-the-scripting-docs-fun-with-editortests/) by Karl Jones.
* [How To Optimise And Increase Performance In Unity Games](http://gamedevelopertips.com/increase-performance-in-unity-games/) by Marco.
* [Hex Map 20: Fog of War](http://catlikecoding.com/unity/tutorials/hex-map/part-20/) by Catlike Coding.
* [VS Code - the best IDE for Unity dev](http://koprowski.it/2017/vs-code/) by Daniel Koprowski.
* [Unity: Android Optimization Guide](http://www.gamasutra.com/blogs/NielsTiercelin/20170824/304424/Unity_Android_Optimization_Guide.php) by Niels Tiercelin.
* [Creating Tools in Unity - Brushes](https://youtu.be/-ehupJw8ArQ) by Unit02Games.
* [Unity3D Git Basics - Commits, Reverting, gitignore, and more](https://youtu.be/tNhIh3NzANc) by Unity3d College.
* [Creating Tools in Unity - Saving & Loading Tilemaps With The Inspector](https://youtu.be/UQduuM7KMN8) by Unit02Games.
* [C# Basics in Unity: Variables and Functions - Episode 1](https://youtu.be/ZikukTMbEMU) by Sykoo.

And this is it for this week!

## Contribute to the week in .NET

As always, this weekly post couldn't exist without community contributions, and I'd like to thank all those who sent links and tips. The F# section is provided by [Phillip Carter](https://twitter.com/_cartermp), the gaming section by [Stacey Haffner](https://twitter.com/yecats131), the Xamarin section by [Dan Rigby](https://twitter.com/DanRigby), and the Azure and UWP section by [Michael Crump](http://twitter.com/mbcrump).

You can participate too. Did you write a great blog post, or just read one? Do you want everyone to know about an amazing new contribution or a useful library? Did you make or play a great game built on .NET?
We'd love to hear from you, and feature your contributions on future posts. Please [add your posts](https://weekindotnet.azurewebsites.net), it takes only a second.

We pick the articles based on the following criteria: the posts must be about .NET, they must have been published this week, and they must be original contents. Publication in Week in .NET is not an endorsement from Microsoft or the authors of this post.

This week's post (and future posts) also contains news I first read on [The ASP.NET Community Standup](https://blogs.msdn.microsoft.com/webdev/tag/communitystandup/), on [Weekly Xamarin](http://weeklyxamarin.com/), on [F# weekly](https://sergeytihon.wordpress.com/category/f-weekly/), and on [The Morning Brew](http://themorningbrew.net/).