The week in .NET - 
============================

To read last week's post, see [The week in .NET – .NET Core triage on On .NET, ShareX](https://blogs.msdn.microsoft.com/dotnet/2016/12/20/the-week-in-net-net-core-triage-on-on-net-sharex/). Next week, the post will be a little late like this week.

On .NET
-------

Last week, I published a short interview with [Steve Smith](https://channel9.msdn.com/Shows/On-NET/Steve-Smith) that was shot during the MVP Summit. We talked about ASP.NET Core and its documentation, that Steve has been contributing to, about his consulting activity, and about his Kickstarter-funded software craftsmanship calendar.

<iframe src="https://channel9.msdn.com/Shows/On-NET/Steve-Smith/player" width="960" height="540" allowFullScreen frameBorder="0"></iframe>

This week, I'll publish another MVP Summit short interview.

Package of the week: Jint
-------------------------

[Jint](https://github.com/sebastienros/jint) is a Javascript interpreter for .NET which provides full ECMA 5.1 compliance ([6.0 work is underway](https://github.com/sebastienros/jint/tree/es6)), and [can run on .NET Framework 4.5 and .NET Standard 1.3](https://www.nuget.org/packages/Jint). It's an ideal solution to provide scripting abilities to a .NET application. [RavenDB uses it](https://ayende.com/blog/160705/design-patterns-in-the-test-of-time-interpreter) to perform small transformations on document fragments, for instance. It's also commonly used as a scripting engine by games.

Running JavaScript code with Jint is as simple as spinning up an interpreter, and handing it the objects and parameters it's allowed to interact with:

```csharp
var engine = new Engine()
    .SetValue("log", new Action<object>(Console.WriteLine));

engine.Execute(@"
    function hello() { 
        log('Hello World');
    };

    hello();
    ");
```

Interoperability in Jint works both ways, with simple translations between both type systems that even include generics support:

```js
var ListOfString = System.Collections.Generic.List(System.String);
var list = new ListOfString();
list.Add('foo');
list.Add(1); // automatically converted to String
list.Count; // 2
```

Game of the Week: Blue Effect
-----------------------------

[Blue Effect](http://blue-effect.com/) is a virtual reality first-person shooter survival horror game. You are deployed to Planet Exo-277, which is populated by an alien race who wants to exterminate you. Fight waves of cruel aliens with your "Little Buddy" (a laser pistol that vaporizes anything in its path), "Enlightenment" (an orb used for lighting the path) and "Blue Effect" (a rare energy source that powers your equipment). Blue Effect also features a Hide & Seek, local multiplayer game mode. In Hide and Seek, a second player is put in control of one of the aliens via a game controller with the goal of seeking, scaring and exterminating.

![gamescreen](https://cloud.githubusercontent.com/assets/4108756/21527251/b7481ac0-ccde-11e6-9bc3-c9cd82755f27.jpg)

[Blue Effect](http://blue-effect.com/) was created [DIVR Labs](http://divrlabs.com/) using [C#](https://channel9.msdn.com/Series/C-Sharp-Fundamentals-Development-for-Absolute-Beginners) and [Unity](unity3d.com). It is currently in early access on [Steam](http://store.steampowered.com/app/522020/) for the HTC Vive and Oculus Rift.

.NET
----

* [The New and Improved Visual Studio 2017 RC: A Review](https://dzone.com/articles/visual-studio-2017-rc-review-a-look-at-whats-new-a?utm_medium=feed&utm_source=feedpress.me&utm_campaign=Feed:%20dzone%2Fwebdev) by Ollie Bannsiter.
* [Why Exceptions should be Exceptional](http://mattwarren.org/2016/12/20/Why-Exceptions-should-be-Exceptional/) by Matt Warren.
* [Decimal vs Double and Other Tips About Number Types in .NET](https://www.exceptionnotfound.net/decimal-vs-double-and-other-tips-about-number-types-in-net/) by Matthew Jones.
* [Akka.Net, Async/Await, .NET Core, Micro-ORMs and EFCore](http://www.maherjendoubi.io/alt-net-talks-2016-12-akka-net-async-await-net-core-micro-orms-and-efcore-2/) by Maher Jendoubi.
* [Making bits faster](http://indexoutofrange.com/Making-bits-faster/) and [Dividing a bit in two for performance](http://indexoutofrange.com/Divide-and-conquer-bits-for-performance/) by Szymon Warda.
* [Gotchas with HttpClient's CancelPendingRequests and Timeout in .NET](http://www.danielcrabtree.com/blog/33/gotchas-with-httpclients-cancelpendingrequests-and-timeout-in-net) by Daniel Crabtree.

ASP.NET
-------

* [Free Intermediate ASP.NET Core 1.0 Training on Microsoft Virtual Academy](http://www.hanselman.com/blog/FreeIntermediateASPNETCore10TrainingOnMicrosoftVirtualAcademy.aspx) by Scott Hanselman.
* [Redirecting unknown cultures when using the url culture provider](https://andrewlock.net/redirecting-unknown-cultures-to-the-default-culture-when-using-the-url-culture-provider/) by Andrew Lock.
* [Page redirection and URL Rewriting with ASP.NET Core](http://www.softfluent.com/blog/dev/2016/12/27/Page-redirection-and-URL-Rewriting-with-ASP-NET-Core) by Gérald Barré.
* [Your First Angular 2, ASP.NET Core Project in Visual Studio Code – Part 6](http://angularfirst.com/your-first-angular-2-asp-net-core-project-in-visual-studio-code-part-6/) by Aaron Marisi.
* [Distributed Cache using Redis and ASP.NET Core](https://www.codeproject.com/Articles/1161890/Distributed-cache-using-Redis-and-ASP-NET-Core) by Petru Faurescu.

F#
--

* [Exploring F# with .NET Core and Kestrel](https://shane.logsdon.io/posts/exploring-fsharp-with-dotnet-core-and-kestrel/) by Shane Logsdon.
* [Functional approaches to dependency injection](http://fsharpforfunandprofit.com/posts/dependency-injection-1/) by Scott Wlaschin.
* [Data structures and algorithms - helping Santa Claus find his road to San Francisco](http://jaskula.fr//blog/2016/12-19-data-structures-and-algorithms-helping-santa-claus-find-his-road-to-san-francisco/index.html#) by Tomasz Jaskula.
* [ReF#actoring: rewriting an actor in F#](https://miles.no/blogg/refactoring-rewriting-an-actor-in-f) by Vagif Abilov.
* [The Traveling Santa Problem… a Neural Network solution](http://www.rickyterrell.com/?p=97) by Riccardo Terrell.
* [About Expandable F# Compiler project](http://www.kekyo.net/2016/12/23/6305) by Kouji Matsui.
* [When Playstation meets F#, PSX |&gt; Pi](https://github.com/ChipmunkHand/ChipmunkHand/blob/master/blog/psx.md) by Ross McKinlay and Andrea McAts.
* [From Elm to Fable: trying F# In The Frontend](http://lucasmreis.github.io/blog/from-elm-to-fable/) by Lucas Reis.
* [Evaluate JsonPath Queries using FSharp.Data](https://j-alexander.github.io/entry/2016/12/23/jsonpath-queries-using-fsharpdata) by Jonathan Leaver.

Azure
-----

* [Service Bus, .NET Standard, and Open Source](https://blogs.msdn.microsoft.com/servicebus/2016/12/20/service-bus-net-standard-and-open-source/) by John Taubensee.
* [Using Azure App Service Authentication with ASP.NET (Classic) MVC Applications](https://blogs.msdn.microsoft.com/appserviceteam/2016/12/21/using-azure-app-service-authentication-with-asp-net-classic-mvc-applications/) by Adrian Hall.
* [Getting started with Azure Functions and using them within Logic Apps](http://blogs.biztalk360.com/getting-started-azure-functions-logic-apps/) by Steef-Jan Wiggers.

Xamarin
-------

* [Xamarin Stable Release: Cycle 8 Service Release 2](https://releases.xamarin.com/stable-release-cycle-8-service-release-2/) by Luis Aguilera.
* [Xamarin Alpha Preview 6: Cycle 9](https://releases.xamarin.com/alpha-preview-6-cycle-9/) by Bri Brothers.
* [Android 7.1 Developer Preview Now Available](https://blog.xamarin.com/android-7-1-developer-preview-now-available/) by Miguel de Icaza.
* [Build your Mobile Development Toolkit for 2017](https://blog.xamarin.com/build-your-mobile-development-toolkit-for-2017/) by Cormac Foster.
* [Simple and Intuitive App Shortcuts in Android 7.1](https://blog.xamarin.com/simple-and-intuitive-app-shortcuts-in-android-7-1/) by James Montemagno.
* [Xamarin Dev Days Available On-Demand](https://blog.xamarin.com/xamarin-dev-days-recap/) by Jayme Singleton.
* [The Xamarin Show Snack Pack 4: Interactive Learning with Xamarin Workbooks](https://channel9.msdn.com/Shows/XamarinShow/Snack-Pack-4-Interactive-Learning-with-Xamarin-Workbooks) by James Montemagno.
* [Going Serverless with Azure Functions: SendGrid](http://motzcod.es/post/154814819597/going-serverless-with-azure-functions-sendgrid) by James Montemagno.
* [Xamarin.Forms: Google AdMob Ads in Android](http://motzcod.es/post/154607061227/xamarinforms-google-admob-ads-in-android) & [Xamarin.Forms: Google Admob Ads in iOS](http://motzcod.es/post/154696375922/xamarinforms-google-admob-ads-in-ios) by James Montemagno.
* [Deploy the Android 7 Multi-Window Mode via Xamarin](https://visualstudiomagazine.com/articles/2016/12/01/multiwindow-mode-via-xamarin.aspx) by Wallace McClure.
* [Xamarin Forms Layout Engine, Under The Hood](https://xamarinhelp.com/xamarin-forms-layout-engine-hood/) by Adam Pedley.
* [How to Make an Android and iOS App in C# on a Mac](https://www.toptal.com/c-sharp/how-to-make-an-android-and-ios-app-in-c-on-a-mac) by Demir Selmanovic.

Data
----

* [Looking at Entity Framework Core 1.0](https://visualstudiomagazine.com/articles/2016/12/01/entity-framework-core-1_0.aspx) by Peter Vogel.

Games
-----

* [Take a look behind-the-scenes with design documents from The Legend of Zelda!](https://www.nintendo.co.uk/News/2016/December/Take-a-look-behind-the-scenes-with-design-documents-from-The-Legend-of-Zelda--1169414.html).
* [Object construction with factory method](http://brightreasongames.com/object-construction-factory-method/) by Pasquale Franzese.
* [[Unity 5] Tutorial: How to make an inventory system - part 1 (Video)](https://youtu.be/SZjWN9MsA94) by Gamad.
* [Generating Collision Meshes for a Voxel Chunk](http://www.ben-drury.co.uk/index.php/2016/12/19/generating-collision-mesh-voxel-chunk/) by Benjamin James Drury.
* [Procedural Landmass Generation (E17: texture shader)](https://youtu.be/XjH-UoyaTgs) by Sebastian Lague.

And this is it for this week!

Contribute to the week in .NET
------------------------------

As always, this weekly post couldn't exist without community contributions, and I'd like to thank all those who sent links and tips. The F# section is provided by [Phillip Carter](https://twitter.com/_cartermp), the gaming section by [Stacey Haffner](https://twitter.com/yecats131), and the Xamarin section by [Dan Rigby](https://twitter.com/DanRigby).

You can participate too. Did you write a great blog post, or just read one? Do you want everyone to know about an amazing new contribution or a useful library? Did you make or play a great game built on .NET?
We'd love to hear from you, and feature your contributions on future posts:

* Send an email to beleroy at Microsoft,
* [comment on this gist](https://gist.github.com/bleroy/848e492f494de04eacdc91ce474a331a)
* Leave us a pointer in the comments section below.
* [Send Stacey (@yecats131) tips on Twitter about .NET games](https://twitter.com/yecats131).

This week's post (and future posts) also contains news I first read on [The ASP.NET Community Standup](https://blogs.msdn.microsoft.com/webdev/tag/communitystandup/), on [Weekly Xamarin](http://weeklyxamarin.com/), on [F# weekly](https://sergeytihon.wordpress.com/category/f-weekly/), and on [Chris Alcock's The Morning Brew](http://themorningbrew.net/).
