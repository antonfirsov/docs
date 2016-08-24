The week in .NET - 8/2/2016
============================

To read last week's post, see [The week in .NET – 7/26/2016](https://blogs.msdn.microsoft.com/dotnet/2016/07/26/the-week-in-net-7262016/).

On .NET
-------

We had no show last week, and the guest for this week is still to be determined. Please check [the On .NET channel on YouTube for updates](https://www.youtube.com/channel/UCvtT19MZW8dq5Wwfu6B0oxw).

Package of the week: AForge.NET
-------------------------------

[AForge.NET](http://www.aforgenet.com/) is an open source C# framework designed for developers and researchers in the fields of Computer Vision and Artificial Intelligence - image processing, neural networks, genetic algorithms, fuzzy logic, machine learning, robotics, etc.

![Motion detection using AForge.NET](http://www.aforgenet.com/framework/features/vision/simple_background_modeling.png)

Here's the code for simple motion detection from a video feed:

```csharp
IMotionDetector detector = new MotionDetector(
    new SimpleBackgroundModelingDetector(),
    new MotionAreaHighlighting());
while (!needToStop)
{
    detector.ProcessFrame(videoFrame);
    if (detector.MotionLevel > 0.01)
    {
        // motion level is greater then 1% - fire alarm
        // make sound, start blinking, start saving video, etc.
        // ...
    }    
}
```

.NET Core App of the week: Emitter.io
-------------------------------------

[Emitter.io](https://emitter.io/) is a fast [MQTT](http://mqtt.org/) implementation that was built entirely on .NET Core, LibUV, and Docker. Naturally, a [.NET client library](https://emitter.io/develop/dotnet) is available.

Game of the Week: Overcooked
----------------------------

Chaotic couch co-op cooking game for one to four players.

In [Overcooked](http://www.ghosttowngames.com/overcooked/), you work as a team of chefs preparing, cooking and serving a variety of tasty orders, making sure to serve them before your customers storm out angry, of course! You can play solo or with up to four of your friends in both co-op and challenge game modes. Overcooked features an array of bizarre kitchens that push your co-op and coordination skills, including being located on a pirate ship, moving trucks and the bowels of a fiery underworld!

![gamescreen](https://cloud.githubusercontent.com/assets/4108756/17334147/fae2a7ce-5889-11e6-83c5-00f7ea073432.png)

Overcooked was created by Ghost Town Games using [Unity](http://unity3d.com/) and [C#](https://channel9.msdn.com/Series/C-Sharp-Fundamentals-Development-for-Absolute-Beginners). It is available August 3rd on Xbox One, Windows (via Steam) and PS4.

User group meeting of the week: Using F# to Create a Shared Logic Layer in Seattle
----------------------------------------------------------------------------------

Join the [Seattle Mobile .NET Developers group](http://www.meetup.com/SeattleMobileDevelopers/) and James Moore on Wednesday, August 3 at 6:00PM at the City University of Seattle for [a session about building a F# driven shared business logic layer for your apps](http://www.meetup.com/SeattleMobileDevelopers/events/232259640/).

.NET
----

* [Entity Framework Core 1.1 Plans](https://blogs.msdn.microsoft.com/dotnet/2016/07/29/entity-framework-core-1-1-plans/) by Rowan Miller.
* [.NET Support and Versioning](https://blogs.msdn.microsoft.com/dotnet/2016/07/26/net-support-and-versioning/) by Lee Coward.
* [Return Any (Task-Like) Type From An Async Method](http://blog.i3arnon.com/2016/07/25/arbitrary-async-returns/) by Bar Arnon.
* [Building all and current dotnet core projects in VS Code](http://blog.jonathanchannon.com/2016/07/13/building-all-current-dotnet-core-projects-vscode/) by Jonathan Channon.
* [Detect and Blur Faces with .NET Core and Face API](https://carlos.mendible.com/2016/07/30/detect-and-blur-faces-with-dotnetcore-and-face-api/) by Carlos Mendible.
* [Rider – the story so far](https://blog.jetbrains.com/dotnet/2016/07/26/rider-the-story-so-far/) by Matt Ellis.
* [.NET Core: Using existing resx resource file](http://www.lhotka.net/weblog/NETCoreUsingExistingResxResourceFile.aspx) by Rockford Lhotka.

ASP.NET
-------

* [The 201 on Building Web API with ASP.NET Core MVC (book)](https://amzn.com/1535534060) by Badrinarayanan Lakshmiraghavan.
* [Announcing WebApiContrib for ASP.NET Core](http://www.strathweb.com/2016/07/announcing-webapicontrib-for-asp-net-core/) by Filip W.
* [ASP.NET Core upgrade speedrun (video)](https://www.youtube.com/watch?v=1__LtKddvPw) by Steve Desmond.
* [Cross platform .NET, welcome to the new age! (video)](https://www.youtube.com/watch?v=lX4kmnGeu4A) by Julie Lerman.
* [Loading view components from a class library in ASP.NET Code MVC](http://aspnetmonsters.com/2016/07/2016-07-16-loading-view-components-from-a-class-library-in-asp-net-core/) by the ASP.NET Monsters.
* [Using Roles with the ASP.NET Core JWT middleware](http://www.jerriepelser.com/blog/using-roles-with-the-jwt-middleware) and [Adding parameters to the OpenID Connect Authorization URL](http://www.jerriepelser.com/blog/adding-parameters-to-openid-connect-authorization-url) by Jerrie Pelser.
* [ASP.NET Core hosting (revisited) – Part I](https://luisfsgoncalves.wordpress.com/2016/07/25/asp-net-core-hosting-revisited-part-i/) and [part II](https://luisfsgoncalves.wordpress.com/2016/07/26/asp-net-core-hosting-revisited-part-ii/) by Luís Gonçalves.
* [Securing Authentication Cookies in ASP.NET Core](https://blog.mariusschulz.com/2016/07/19/securing-authentication-cookies-in-asp-net-core) and [Generating Route URLs in ASP.NET Core MVC](https://blog.mariusschulz.com/2016/07/21/generating-route-urls-in-asp-net-core-mvc) by Marius Schulz.
* [Loading tenants from the database with SaasKit - Part 2, Caching](http://andrewlock.net/loading-tenants-from-the-database-with-saaskit-part-2-caching/) by Andrew Lock.
* [Exploring a minimal WebAPI with ASP.NET Core](http://www.hanselman.com/blog/ExploringAMinimalWebAPIWithASPNETCore.aspx) by Scott Hanselman.
* [Walkthrough: ASP.NET application performance improvements on Azure with ANTS Performance Profiler](http://www.red-gate.com/products/dotnet-development/ants-performance-profiler/resources/articles/azure-asp-net-performance-profiling) by Ben Emmett.
* [How to add MVC to your ASP.NEt Core application](https://jonhilton.net/2016/07/27/how-to-add-mvc-to-your-asp-net-core-web-application/) by Jon Hilton.
* [Authoring Nested TagHelpers in ASP.NET Core MVC](http://our.componentone.com/2016/07/28/taghelpers-authoring-nested-taghelpers-in-asp-net-core-mvc/) by Prabhakar Mishra.
* [Using PetaPoco micro-ORM with ASP.NET Core](http://www.artifextech.com/blog/using-petapoco-micro-orm-with-asp-net-core-1/) by Paul Duffy.
* [Using Sessions and HttpContext in ASP.NET Core and MVC Core](http://benjii.me/2016/07/using-sessions-and-httpcontext-in-aspnetcore-and-mvc-core/) by Ben Cull.

F#
--

* [Working with F# projects in VSCode](http://kcieslak.io/Working-with-F-Projects-In-VSCode) by Krzysztof Cieślak.
* [Approximate your spending pattern using Gradient descent in F#](https://kimsereyblog.blogspot.co.uk/2016/07/approximate-your-spending-pattern-using.html) by Kimserey Lam.
* [A on Akka.NET 1.1 with Aaron Stannard](https://www.infoq.com/news/2016/07/akka-dotnet) by Pierre-Luc Maheu.
* [Workshop Recap: Expressing Intent with F# with Tomas Petricek](https://tech.jet.com/blog/2016/07-13-workshop-recap-expressing-intent-f-tomas-petricek/) by Erich Ess and Nora Jones.
* [My first month working with F# daily](https://medium.com/@machadoiuri/my-first-month-working-with-f-daily-c8c1b9c84dd8#.yushl3584) by Iuri L Machado.
* [Struggling with Readability in Functional Programming (with Euler Problem #6 in F#)](https://jeremybytes.blogspot.co.uk/2016/07/struggling-with-readability-in.html) by Jeremy Clark.

Check out [F# Weekly](https://sergeytihon.wordpress.com/category/f-weekly/) for more great content from the F# community.

Xamarin
-------

* [Preview: Android N support preview 3](https://releases.xamarin.com/preview-android-n-support-preview-3/) by Adrian Murphy.
* [Asset Previewer](http://tirania.org/blog/archive/2016/Jul-28.html) by Miguel de Icaza.
* [Implement Custom fonts using Effects in Xamarin Forms](http://err2solution.com/2016/07/custom-fonts-using-effects-in-xamarin-forms/) by S Ravi Kumar.
* [Creating a Xamarin.Android Navigation Service for MVVM Light to use with any Activity](http://www.blogaboutxamarin.com/creating-a-xamarin-android-navigation-service-for-mvvm-light-to-use-with-any-activity/), [Using the MVVM Light ObservableRecylerAdapter with the Xamarin.Android RecylerView](http://www.blogaboutxamarin.com/using-the-mvvm-light-observablerecyleradapter-with-the-xamarin-android-recylerview/), and [Using the ObservableTableViewSource in MVVM Light V5.3 with Xamarin.iOS](http://www.blogaboutxamarin.com/using-the-observabletableviewsource-in-mvvm-light-v5-3-with-xamarin-ios/) by Richard Woollcott.

Games
---

* [Unity 5.4 is Out – Here's What's In It](http://blogs.unity3d.com/2016/07/28/unity-5-4-is-out-heres-whats-in-it/) by Alex Lian
* [Creating an Ability System With Scriptable Objects (upcoming Unity live training)](http://unity3d.com/learn/live-training/session/creating-ability-system-scriptable-objects) by Matthew-Schell
* [Character Select System with Scriptable Objects (upcoming Unity live training)](http://unity3d.com/learn/live-training/session/character-select-system-scriptable-objects) by Matthew-Schell
* [Delegates and Events in Unity](http://www.unitygeek.com/delegates-events-unity/) by Unity Geek
* [Finite State Machine For Game Developers](http://gamedevelopertips.com/finite-state-machine-game-developers/) by Marco
* [Hex Map 2: Blending Cell Colors](http://catlikecoding.com/unity/tutorials/hex-map-2/) by Catlike Coding
* [1.1 Unity Tower defense tutorial](https://www.youtube.com/watch?v=-EQKXyzdWwg) by inScope Studios
* [Unity and C# Tutorial - Lesson Three - Arrays](https://www.youtube.com/watch?v=YhMT7Eg5hWg) by Craig Hinrichs
* [C# Monogame RPG Made Easy Tutorial 2 - GameScreen](https://www.youtube.com/watch?v=CcPb0bKkpeg) by CodingMadeEasy

And this is it for this week!

Contribute to the week in .NET
------------------------------

As always, this weekly post couldn't exist without community contributions, and I'd like to thank all those who sent links and tips.

You can participate too. Did you write a great blog post, or just read one? Do you want everyone to know about an amazing new contribution or a useful library? Did you make or play a great game built on .NET?
We'd love to hear from you, and feature your contributions on future posts:

* Send an email to beleroy at Microsoft,
* [comment on this gist](https://gist.github.com/bleroy/c2d1a9d682097f25f8b82d6001b16a3c)
* Leave us a pointer in the comments section below.
* [Send Stacey (@yecats131) tips on Twitter about .NET games](https://twitter.com/yecats131).

This week's post (and future posts) also contains news I first read on [The ASP.NET Community Standup](https://blogs.msdn.microsoft.com/webdev/tag/communitystandup/), on [Weekly Xamarin](http://weeklyxamarin.com/), on [F# weekly](https://sergeytihon.wordpress.com/category/f-weekly/), on [ASP.NET Weekly](http://www.aspnetweekly.com/), and on [Chris Alcock's The Morning Brew](http://themorningbrew.net/).
