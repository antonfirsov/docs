---
title: The week in .NET - Open XML SDK, Adventure Time
keywords: Week in .NET, .NET, community
weblogName: .NET Blog
postId: 10625
---
Previous posts:

* [.NET poster, Happy Birthday .NET with Jan Kotas, Skyworld](https://blogs.msdn.microsoft.com/dotnet/2017/05/23/the-week-in-net-net-poster-happy-birthday-net-with-jan-kotas-skyworld/)
* [Microsoft Build 2017, .NET Core 2.0 preview 1, For the King](https://blogs.msdn.microsoft.com/dotnet/2017/05/16/the-week-in-net-microsoft-build-2017-net-core-2-0-preview-1-for-the-king/)
* [Microsoft Build 2017, .NET Core 2.0 status, Happy birthday .NET with Eilon Lipton, On .NET with Alfonso García-Caro on Fable, Stanford CoreNLP](https://blogs.msdn.microsoft.com/dotnet/2017/05/09/the-week-in-net-microsoft-build-2017-net-core-2-0-status-happy-birthday-net-with-eilon-lipton-on-net-with-alfonso-garca-caro-on-fable-stanford-corenlp/)
 
Package of the week: Open XML SDK
---------------------------------

The Open XML SDK provides open-source libraries for working with Word, Excel, and PowerPoint documents. It supports scenarios such as high-performance generation of word-processing documents, spreadsheets, and presentations, high fidelity conversion of Word documents to HTML, extraction of data from XLSX, and document modification.

```csharp
using (var doc = WordprocessingDocument.Open(strDoc, true))
{
    var p = new Paragraph(new Run(new Text(
        "This is some text in a run in a paragraph.")));
    
    doc.MainDocumentPart.Document.Body.AppendChild(p);
}
```

* [Source code](https://github.com/officedev/open-xml-sdk)
* [MyGet feed](https://dotnet.myget.org/gallery/open-xml-sdk)

Game of the Week - Adventure Time: Magic Man's Head Games
---------------------------------------------------------

[Adventure Time: Magic Man's Head Games](http://www.turbo-button.com/games/adventuretime) is a virtual reality platformer. After you've been magically transformed into a giant balloon, you must work with Finn and Jake to become normal again. Help them fight baddies, rescue friends and traverse dangerous lands as you chase Magic Man across Ooo.  

![Adventure Time: Magic Man's Head Games](Adventuretime.jpg)

[Adventure Time: Magic Man's Head Games](http://www.turbo-button.com/games/adventuretime) was created by [Turbo Button](http://www.turbo-button.com) using [C#](https://channel9.msdn.com/Series/C-Sharp-Fundamentals-Development-for-Absolute-Beginners) and [Unity](unity3d.com). It is available for the Samsung Gear VR and on [Steam](http://store.steampowered.com/app/412790/Adventure_Time_Magic_Mans_Head_Games/) for Oculus Rift and HTC Vive.

Meetup of the week: up and running with ASP.NET MVC Core in Edmunton, AB
------------------------------------------------------------------------

On [Wednesday, May 31 at 6PM, the Edmunton .NET User Group has an "up and running with ASP.NET MVC Core" session](https://www.meetup.com/Edmonton-NET-User-Group/events/233981332/).

## .NET

* [.NET Framework May 2017 Cumulative Quality Update for Windows 10](https://blogs.msdn.microsoft.com/dotnet/2017/05/26/net-framework-may-2017-cumulative-quality-update-for-windows-10/) by Rich Lander.
* [Using .NET and Docker Together](https://blogs.msdn.microsoft.com/dotnet/2017/05/25/using-net-and-docker-together/) by Rich Lander.
* [Integration testing with .NET Core](https://medium.com/@dmitriy.litichevskiy/integration-testing-with-net-core-e82e0d923794) by Dmitriy Litichevskiy.
* [An Early Look at Multi-Tenancy in Marten 2.0](https://jeremydmiller.com/2017/05/22/an-early-look-at-multi-tenancy-in-marten-2-0/) by Jeremy D. Miller.
* [Message Handlers in the new Jasper Service Bus](https://jeremydmiller.com/2017/05/25/message-handlers-in-the-new-jasper-service-bus/) by Jeremy D. Miller.
* [Actor model and using of Akka.NET](https://rubikscode.net/2017/05/28/actor-model-and-using-of-akka-net/) by Rubik’s Code.
* [.NET Framework setup verification tool, cleanup tool and detection sample code now support .NET Framework 4.7](https://blogs.msdn.microsoft.com/astebner/2017/05/26/net-framework-setup-verification-tool-cleanup-tool-and-detection-sample-code-now-support-net-framework-4-7/) by Aaron Stebner.
* [Managed object internals, Part 1. Layout](https://blogs.msdn.microsoft.com/seteplia/2017/05/26/managed-object-internals-part-1-layout/) by Sergey Teplyakov.

## ASP.NET

* [Access the JWT bearer token when using the JWT middleware in ASP.NET Core](http://www.jerriepelser.com/blog/aspnetcore-jwt-saving-bearer-token-as-claim/) by Jerrie Pelser.
* [Overriding the NameClaimType when using the ASP.NET Core OpenID Connect middleware](http://www.jerriepelser.com/blog/overriding-name-claimtype-aspnetcore-oidc/) by Jerrie Pelser.
* [Start with Elasticssearch, Kibana and ASP.NET Core](https://carlos.mendible.com/2017/05/30/start_with_elasticsearch_kibana_and_aspnet_core/) by Carlos Mendible.
* [Self Descriptive HTTP API in ASP.NET Core](https://codeopinion.com/self-descriptive-http-api-in-asp-net-core-hateoas/) by Derek Comartin.
* [ASP.NET Core 2.0 Configuration and Razor Pages](https://dotnetcore.gaprogman.com/2017/05/25/asp-net-core-2-0-configuration-and-razor-pages/) by Jamie Taylor.
* [How to use multiple hosting environments on the same machine in ASP.NET Core](https://andrewlock.net/how-to-use-multiple-hosting-environments-on-the-same-machine-in-asp-net-core/) by Andrew Lock.
* [Using Razor Pages to simplify basic actions in ASP.NET Core 2.0 preview 1](https://andrewlock.net/using-razorpages-to-simplify-basic-actions-in-asp-net-core-2-0-preview-1/) by Andrew Lock.
* [Creating simple shoutbox using ASP.NET Core Razor Pages](http://gunnarpeipman.com/2017/05/razor-pages-shoutbox/) by Gunnar Peipman.
* [ASP.NET Core Sample Image Resizing Service](http://sikorsky.pro/en/blog/aspnet-core-image-resizing-service) by Dmitry Sikorsky.
* [Precompiling razor views](https://www.meziantou.net/2017/05/29/asp-net-core-precompiling-razor-views) by Gérald Barré.
* [Exploiting Partial and RenderPartial in ASP.NET MVC](https://visualstudiomagazine.com/articles/2017/05/26/tip-partial-renderpartial-aspnet-mvc.aspx) by Peter Vogel.
* [Handling 404 Not Found in Asp.Net Core](https://www.devtrends.co.uk/blog/handling-404-not-found-in-asp.net-core) and [Handling errors in an ASP.NET Core Web API](https://www.devtrends.co.uk/blog/handling-errors-in-asp.net-core-web-api) by DevTrends.
* [ASP.NET Core Correlation IDsWriting a basic middleware library to enable correlation IDs on ASP.NET Core](https://www.stevejgordon.co.uk/asp-net-core-correlation-ids) by Steve Gordon.

## C#

* [C# Local Functions](https://garywoodfine.com/c-local-functions/) by Gary Woodfine.
* [C# 7 Series, Part 2: Async Main](https://blogs.msdn.microsoft.com/mazhou/2017/05/30/c-7-series-part-2-async-main/) by mazhou.
* [Lowering in the C# Compiler (and what happens when you misuse it)](http://mattwarren.org/2017/05/25/Lowering-in-the-C-Compiler/) by Matt Warren.

## F#

* [F# Weekly #22, 2017 with 2017 F# survey results](https://sergeytihon.com/category/f-weekly/) by Sergey Tihon.
* [Agile Experiments in Machine Learning with F# - Mathias Brandewinder](http://www.channel64.net/2017/05/agile-experiments-in-machine-learning.html) by channel64.
* [Xamarin Forms: MvvmCross with F#](http://www.codingwithsam.com/xamarin-forms-mvvmcross-with-f/) by sam.
* [Introducing F# intro](http://blog.2mas.xyz/introducing-f-intro/) by Tomas Jansson.
* [F# Works 2017 Survey Results](https://docs.google.com/forms/d/e/1FAIpQLSeZ1EJe1pfztiUudIclcMU1lV-vXCUlGqECPQMZxFD6Q0zGoA/viewanalytics).
* [Lambda expressions in F#](https://dotnetcodr.com/2017/05/27/lambda-expressions-in-f/) by Andras Nemes.
* [Feeding a function result into a pattern matching lambda expression in F#](https://dotnetcodr.com/2017/05/28/feeding-a-function-result-into-a-pattern-matching-lambda-expression-in-f/) by Andras Nemes.

## VB

* [Introduction to (Live) Unit Testing in Visual Basic…](https://blogs.msdn.microsoft.com/vbteam/2017/05/21/introduction-to-live-unit-testing-in-visual-basic/) by Klaus Löffelmann.

## Xamarin

* [Xamarin University’s free webinar series: Learn mobile development from experts](https://blogs.msdn.microsoft.com/visualstudio/2017/05/24/xamarin-universitys-free-webinar-series-learn-mobile-development-from-experts/) by Mark Smith.
* [Xamarin.Tip – MvvmLight Code Snippets for Visual Studio for Mac](https://alexdunn.org/2017/05/25/xamarin-tip-mvvmlight-code-snippets-for-visual-studio-for-mac/) by Alex Dunn.
* [What good is Xamarin.Forms?](https://borntolearn.mslearn.net/b/mva/posts/what-good-is-xamarin-forms) by Matthew Calder.
* [All The Ways To Have Multiple Solutions Open In VS For Mac](https://codemilltech.com/things-i-think-are-cool-all-the-ways-to-have-multiple-solutions-open-in-vs-for-mac/) by Matthew Soucoup.
* [Dynamically binding RESX Resources in Xamarin Forms](http://blog.pieeatingninjas.be/2017/05/20/dynamically-binding-resx-resources-in-xamarin-forms/) by Pieter Nijs.
* [Yet Another Podcast #171 – MFractor](http://jesseliberty.com/2017/05/25/yet-another-podcast-171-mfractor/) by Jesse Liberty.
* [Securing Google Play In-App Purchases for Xamarin with Azure Functions](http://jonathanpeppers.com/Blog/securing-google-play-in-app-purchases-for-xamarin-with-azure-functions) by Jonathan Peppers.
* [Intelligent Bot in a Native iOS and Android app](http://www.colbylwilliams.com/2017/05/18/intelligent-bot-in-a-native-ios-and-android-app.html) by Colby Williams.
* [Change the font type of a NavigationPage Title in Xamarin Forms](http://www.devprotocol.com/change-the-font-type-of-a-navigationpage-title-in-xamarin-forms/) by Jan Tourlamain.
* [Deep Dive into SkiaSharp with Xamarin.Forms](https://blog.xamarin.com/deep-dive-skiasharp-xamarin-forms/) by Charles Petzold.
* [Staying Up-to-Date in Visual Studio 2017 with the Xamarin Updater](https://blog.xamarin.com/staying-date-visual-studio-2017-xamarin-updater/) by Pierce Boggan.
* [TrainerRoad Helps Cyclists Increase Performance with Five-Star Apps](https://blog.xamarin.com/trainerroad-helps-cyclists-increase-performance-five-star-apps/) by Lacey Butler.
* [Using Local Notifications in Xamarin.Mac](https://blog.xamarin.com/using-local-notifications-xamarin-mac/) by Adam Hartley.
* [Introducing MFractor for Visual Studio Mac](https://www.mfractor.com/blogs/news/introducing-mfractor-for-visual-studio-mac) by Matthew Robbins.
* [Preview your HockeyApp apps in Mobile Center](https://www.hockeyapp.net/blog/2017/05/18/preview-your-hockeyapp-apps-in-mobile-center.html) by HockeyApp Team.
* [Rebuilding the Xamarin.Forms Button with Visual States and Control Templates](https://nicksnettravels.builttoroam.com/post/2017/05/23/Rebuilding-the-XamarinForms-Button-with-Visual-States-and-Control-Templates.aspx) by Nick Randolph.
* [Styling Pages and Controls in Xamarin Forms using Visual States](https://nicksnettravels.builttoroam.com/post/2017/05/23/Styling-Pages-and-Controls-in-Xamarin-Forms-using-Visual-States.aspx) by Nick Randolph.
* [Xamarin Stable Release: 15.2.2 Servicing Release](https://releases.xamarin.com/stable-release-15-2-2/) by Bri Brothers.
* [Stable Release: 15.2 Hotfix Xamarin Android (Mac Only)](https://releases.xamarin.com/stable-release-15-2-hotfix-xamarin-android-mac-only/) by Adrian Murphy.
* [Architecting a Large Xamarin Forms App](https://xamarinhelp.com/architecting-large-xamarin-forms-app/) by Adam Pedley.
* [Container, Region and Stack Navigation in Exrin](https://xamarinhelp.com/container-region-stack-navigation-exrin/) by Adam Pedley.
* [Continuous Integration and Deployment for Xamarin Apps](https://xamarinhelp.com/continuous-integration-deployment-xamarin-apps/) by Adam Pedley.
* [HoloLens with Xamarin UrhoSharp](https://xamarinhelp.com/hololens-xamarin-urhosharp/) by Adam Pedley.
* [Running Xamarin UITests Locally](https://xamarinhelp.com/running-xamarin-uitests-locally/) by Adam Pedley.
* [Xamarin Forms LayoutOption Differences](https://xamarinhelp.com/xamarin-forms-layoutoption-differences/) by Adam Pedley.

## Azure

* [Comparing Data Stores](https://developingdane.com/comparing-data-stores/) by Dane Vinson.

## UWP

* [Windows Template Studio](https://csharp.christiannagel.com/2017/05/24/windowstemplatestudio/) by Christian Nagel.
* [See What’s New with Windows Ink in the Windows 10 Creators Update](https://blogs.windows.com/buildingapps/2017/05/23/see-whats-new-windows-ink-creators-update/#lA7p9Oj8AxWZeBi6.97) by Jerry Koh.
* [UWP and the evolution of touch development](https://blogs.windows.com/buildingapps/2017/05/25/uwp-evolution-touch-development/#1MI02dEYDgYo0fuW.97) by Windows Apps Team.

## Data

* [Dealing With Optimistic Concurrency Control Collisions](https://jimmybogard.com/dealing-with-optimistic-concurrency-control-collisions/) by Jimmy Bogard.
* [5 Ways To Manage Database Schema Changes in 2017 (in .NET)](https://surfingthecode.com/2017/05/5-ways-to-manage-database-schema-changes-in-dotnet/) by Alexander Tsvetkov.

And this is it for this week!

Contribute to the week in .NET
------------------------------

As always, this weekly post couldn't exist without community contributions, and I'd like to thank all those who sent links and tips. The F# section is provided by [Phillip Carter](https://twitter.com/_cartermp), the gaming section by [Stacey Haffner](https://twitter.com/yecats131), the Xamarin section by [Dan Rigby](https://twitter.com/DanRigby), and the UWP section by [Michael Crump](http://twitter.com/mbcrump).

You can participate too. Did you write a great blog post, or just read one? Do you want everyone to know about an amazing new contribution or a useful library? Did you make or play a great game built on .NET?
We'd love to hear from you, and feature your contributions on future posts. Please [add your posts](https://weekindotnet.azurewebsites.net), it takes only a second.

This week's post (and future posts) also contains news I first read on [The ASP.NET Community Standup](https://blogs.msdn.microsoft.com/webdev/tag/communitystandup/), on [Weekly Xamarin](http://weeklyxamarin.com/), on [F# weekly](https://sergeytihon.wordpress.com/category/f-weekly/), and on [The Morning Brew](http://themorningbrew.net/).