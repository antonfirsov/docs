The week in .NET - Happy birthday .NET with Robin Cole, TinyORM, 911 Operator
=============================================================================

Previous posts:

* [.NET Framework 4.7, reference documentation, On .NET on modular ASP.NET, Happy birthday .NET with Immo Landwerth, JustAssembly](https://blogs.msdn.microsoft.com/dotnet/2017/04/11/the-week-in-net-net-framework-4-7-reference-documentation-on-net-on-modular-asp-net-happy-birthday-net-with-immo-landwerth-justassembly/)
* [On .NET on SonarLint and SonarQube, Happy birthday .NET with Dan Fernandez, nopCommerce, Steve Gordon](https://blogs.msdn.microsoft.com/dotnet/2017/04/04/the-week-in-net-on-net-on-sonarlint-and-sonarqube-happy-birthday-net-with-dan-fernandez-nopcommerce-steve-gordon/)
* [On .NET with Sidharth Gupta on Tizen, Happy birthday .NET with Bertrand Le Roy, JSON.NET 10, Gunnar Peipman](https://blogs.msdn.microsoft.com/dotnet/2017/03/28/the-week-in-net-on-net-with-sidarth-gupta-on-tizen-happy-birthday-net-with-bertrand-le-roy-json-net-10-gunnar-peipman/)

On .NET
-------

This week on the show, we'll speak with [Don Schenck](https://twitter.com/DonSchenck) about [Red Hat](https://www.redhat.com/en). We'll take questions on Gitter, on [the dotnet/home channel](https://gitter.im/dotnet/home) and on Twitter. Please use the `#onnet` tag. It's OK to start sending us questions in advance if you can't do it live during the show.

Happy birthday .NET with Robin Cole
-----------------------------------

In February we got together with many Microsoft alumni and current employees for a huge .NET Birthday bash. We spoke to Robin Cole, who joined Microsoft in 2005 working on many projects including Expression and Visual Studio. In this quick interview, she shares her thoughts on developers and designers and exciting future ahead.

<iframe src="https://channel9.msdn.com/Blogs/funkyonex/Happy-Birthday-NET-with-Robin-Cole/player" width="960" height="540" allowFullScreen frameBorder="0"></iframe>

Package of the week: TinyORM
----------------------------

[TinyORM](https://github.com/sdrapkin/SecurityDriven.TinyORM/wiki/Quick-start) is a new micro-ORM for .NET that automates connection and transaction management, that is simple and easy to use correctly.

```csharp
public class POCO
{
    public int Answer { get; set; }
}

//...
var db = DbContext.CreateDbContext(connectionString:
  "Data Source=.\\SQL2012;Initial Catalog=tempdb;Integrated Security=True;Pooling=true;Max Pool Size=3000;");
var ids = await db.QueryAsync("select [Answer] = object_id from sys.objects;");
var pocoArray = ids.ToObjectArray<POCO>();
foreach (var poco in pocoArray)
{
    Console.WriteLine(poco.Answer);
}
```

* [NuGet](https://www.nuget.org/packages/TinyORM)
* [GitHub](https://github.com/sdrapkin/SecurityDriven.TinyORM)

Game of the Week: 911 Operator
------------------------------

[911 Operator](http://jutsugames.com/911/) is an indie simulation game. Ever wanted to see what it was like to be a 911 operator? Well, now you can! In 911 Operator, you'll manage emergency lines by answering incoming calls and reacting appropriately. Give first aid instructions, dispatch emergency respondents or even choose to ignore the call which could very well be from a prankster. In 911 Operator, you can play in any city of the world by using Free Play mode to download real maps, which of course includes real addresses, streets and emergency infrastructure.

![911 Operator](https://cloud.githubusercontent.com/assets/4108756/25139964/a638f22c-2413-11e7-8955-c9d14a12c1ce.jpg)

[911 Operator](http://jutsugames.com/911/) was created by [Jutsu Games](http://jutsugames.com/) using [C#](https://channel9.msdn.com/Series/C-Sharp-Fundamentals-Development-for-Absolute-Beginners) and [Unity](unity3d.com). It is available on [Steam](http://store.steampowered.com/app/503560/) for PC, Mac and Linux.

Meetup of the week: Global Azure Bootcamp in Miami, FL
------------------------------------------------------

The [dotnetmiami](https://www.meetup.com/dotNetMiami/) user group hosts their [Global Azure Bootcamp this Saturday at 9:00AM in Miami](https://www.meetup.com/dotNetMiami/events/238312720/).

.NET
----

* [Visual Studio for Mac to the Cloud and Beyond](https://blogs.msdn.microsoft.com/visualstudio/2017/04/12/visual-studio-for-mac-to-the-cloud-and-beyond/) by Miguel de Icaza.
* [Creating and editing solution files with the .NET CLI](https://andrewlock.net/creating-and-editing-solution-files-with-the-net-cli/) by Andrew Lock.
* [Running .NET Core Apps under Windows Subsystem for Linux (Bash for Windows)](https://weblog.west-wind.com/posts/2017/Apr/13/Running-NET-Core-Apps-under-Windows-Subsystem-for-Linux-Bash-for-Windows) by Rick Strahl.
* [Simple and elegant microservices authentication using JWT](http://blog.hovland.xyz/2017-04-09-simple-and-elegant-microservices-authentication-using-JWT/) by Tor Hovland.
* [.NET Core Overview](https://blogs.msdn.microsoft.com/premier_developer/2017/04/12/net-core-overview/) by Premier Developer.
* [Casting to IEnumerable<T> is Two Orders of Magnitude Slower](https://www.danielcrabtree.com/blog/191/casting-to-ienumerable-t-is-two-orders-of-magnitude-slower) by Daniel Crabtree.
* [How to Create .NET Core Windows Services with Visual Studio 2017](https://stackify.com/creating-net-core-windows-services/) by Matt Watson.
* [Which version of .NET Core is where?](http://www.donovanbrown.com/post/Which-version-of-NET-Core-is-where) by Donovan Brown.

ASP.NET
-------

* [ASP.NET Core MVC app running on Raspberry Pi](http://laurentkempe.com/2017/04/14/ASPNET-Core-MVC-app-running-on-raspberry-pi/) by Laurent Kempé.
* [ASP.NET Core IdentityServer4 Resource Owner Password Flow with custom UserRepository](https://damienbod.com/2017/04/14/asp-net-core-identityserver4-resource-owner-password-flow-with-custom-userrepository/) by Damien Bowden.
* [ASP.NET Core Web Servers: Kestrel vs IIS Feature Comparison and Why You Need Both](https://stackify.com/kestrel-web-server-asp-net-core-kestrel-vs-iis/) by Matt Watson.
* [How to troubleshoot: “An error occurred while starting the application” in ASP.NET Core on IIS](https://scottsauber.com/2017/04/10/how-to-troubleshoot-an-error-occurred-while-starting-the-application-in-asp-net-core-on-iis/) by Scott Sauber.

C#
--

* [Method overload resolution in C# 6.0: an interesting bug story](http://codewithstyle.info/method-overload-resolution-in-c-6-0-an-interesting-bug-story/) by Milosz Piechocki.

F#
--

* [Happy F# Day!](https://fsharpforfunandprofit.com/posts/happy-fsharp-day-2/), by Scott Wlaschin
* [Visual F# Tools - Visual Studio Toolbox](https://channel9.msdn.com/Shows/Visual-Studio-Toolbox/Visual-F-Tools)
* [Art and Neural Networking in F#](https://skillsmatter.com/skillscasts/9727-art-and-neural-network-with-f-sharp), by Robert Pickering
* [Using F#, Azure Functions, Fable and Shell Scripts](https://skillsmatter.com/skillscasts/10049-lightning-talk-session-using-f-sharp-azure-functions-fable-and-shell-scripts), by Mark Gray
* [Playing nice together: how to use F# in a brownfield project](https://skillsmatter.com/skillscasts/9888-playing-nice-together-how-to-use-f-sharp-in-a-brownfield-project), by Gien Verschatse
* [Contributing to Visual F# in 2017](http://blog.ctaggart.com/2017/04/contributing-to-visual-f-in-2017.html), by Cameron Taggert
* [Freya at FSharpX 2017](https://freya.io/blog/2017/04/10/fsharpx.html), by marcus Griep
* [Basic implementations of 5 graph data structures in F#](https://znprojects.blogspot.com.by/2017/04/aoc-2016-day-2-alternate-solutions.html), by znProjects
* [An interesting (if unsuccessful) look into predicting horse races via machine learning with F#](https://medium.com/@ThisisZone/an-interesting-if-unsuccessful-look-into-predicting-horse-races-via-machine-learning-with-f-7563090c7582) by Zone.

New F# language SUggestions:

* [Add Map.merge](https://github.com/fsharp/fslang-suggestions/issues/560)
* [Provide an Async conversion function for `structural awaiters`](https://github.com/fsharp/fslang-suggestions/issues/559)

There was a major F# conference two weeks ago, F# eXchange.  You can view all of the talks online [here](https://skillsmatter.com/conferences/8053-f-sharp-exchange-2017#skillscasts).  If you wish to see all the new and exciting areas where F# is going, please watch them.  They're entirely free.

Check out [F# Weekly](https://sergeytihon.wordpress.com/category/f-weekly/) for more great content from the F# community.

VB
--

* [Controlling Process Flow with the Template Method Pattern](https://visualstudiomagazine.com/articles/2017/04/01/template-method-pattern.aspx) by Peter Vogel.

Xamarin
-------

Microsoft Engineering is offering a limited number of technical sessions to help your team build better apps faster, and avoid the common pitfalls in going mobile. The [Go Mobile Tech Workshops](https://www.xamarin.com/go-mobile-tech-workshop) are dedicated sessions for your team covering everything from your technology stack and architecture to the latest in Visual Studio 2017 and DevOps best practices. These workshops help your team get ahead with current projects and prepare for what is coming next in app development.

Apply [here](https://www.xamarin.com/go-mobile-tech-workshop).

* [Xamarin Alpha Release: 15.2 Alpha Preview 2](https://releases.xamarin.com/alpha-release-15-2-alpha-preview-2/) by Bri Brothers.
* [Preview 7: Visual Studio for Mac](https://releases.xamarin.com/preview-7-visual-studio-for-mac/) by Bri Brothers.
* [Stable Release: Xamarin Workbooks & Inspector 1.2.1](https://releases.xamarin.com/stable-release-xamarin-workbooks-inspector-1-2-1/) by Bri Brothers.
* [Service Release: Xamarin.Forms 2.3.4.231](https://releases.xamarin.com/service-release-xamarin-forms-2-3-4-231/) by David Ortinau.
* [Pre-release: Xamarin.Forms 2.3.5.233](https://releases.xamarin.com/pre-release-xamarin-forms-2-3-5-233/) by David Ortinau.
* [Make Your Xamarin.Forms Apps Even Better (and Faster!)](https://blog.xamarin.com/make-xamarin-forms-apps-even-better-faster/) by David Ortinau.
* [Podcast: Xamarin.Forms 2.3.4 & Beyond](https://blog.xamarin.com/podcast-xamarin-forms-2-3-4-beyond/) by Pierce Boggan.
* [Xamarin Documentation Goes Multilingual!](https://blog.xamarin.com/xamarin-documentation-goes-multilingual/) by Joel Martinez.
* [New Xamarin Dev Day Cities!](https://blog.xamarin.com/new-xamarin-dev-day-cities/) by Jayme Singleton.
* [Building Your First iOS App and Connected Apps with Microsoft Azure](https://blog.xamarin.com/xamarin-university-webinar-recordings-building-first-ios-app-connected-apps-microsoft-azure/) by Courtney Witmer.
* [Live Webinar | Getting the Most Out of Xamarin.Forms for Visual Studio 2017](https://blog.xamarin.com/live-webinar-getting-xamarin-forms-visual-studio-2017/) by David Ortinau.
* [Displaying Data with macOS Table Views](https://blog.xamarin.com/displaying-data-macos-table-views/) by Adam Hartley.
* [Visual Studio for Mac to the Cloud and Beyond](https://blogs.msdn.microsoft.com/visualstudio/2017/04/12/visual-studio-for-mac-to-the-cloud-and-beyond/) by Miguel de Icaza.
* [Unit Testing Plugins for Xamarin](http://motzcod.es/post/159267241302/unit-testing-plugins-for-xamarin) by James Montemagno.
* [Device.OS is Obsolete in Xamarin.Forms… What to do?!?!](http://motzcod.es/post/159463651162/device-os-xamarin-forms-obsolete-runtime-os) by James Montemagno.
* [Using Visual Studio Mobile Center with a VSTS Code Repo](http://motzcod.es/post/159472912002/vs-mobile-center-builds-with-vsts-repo) by James Montemagno.
* [The Xamarin Show - Episode 21: Deploying Realm Object Server to an Azure Linux VM](https://channel9.msdn.com/Shows/XamarinShow/Episode-21-Deploying-Realm-Object-Server-to-an-Azure-Linux-VM) by James Montemagno.
* [Xamarin.Android Linker Tricks Part 1 - Bitdiffer](http://www.jon-douglas.com/2017/04/13/linker-bitdiffer/) by Jon Douglas.
* [Xamarin.Tips – iOS Bar Background Images in Xamarin.Forms](https://alexdunn.org/2017/04/07/xamarin-tips-ios-bar-background-images-in-xamarin-forms/) by Alex Dunn.

Azure
-----

* [Orchestrating processes with full recoverability](https://blog.scooletz.com/2017/04/13/orchestrating-your-processes-with-durable-task/) by Szymon Kulec.
* [Use Application Insights in a desktop application](https://www.meziantou.net/2017/03/29/use-application-insights-in-a-desktop-application) by Gérald Barré.
* [Azure Functions Access-Control-Allow-Credentials with CORS](https://blogs.msdn.microsoft.com/benjaminperkins/2017/04/12/azure-functions-access-control-allow-credentials-with-cors/) by Benjamin Perkins.
* [Introducing: Serverless C# with Azure Functions](https://visualstudiomagazine.com/articles/2017/04/01/azure-functions.aspx) by Jason Roberts.

UWP
----

* [Windows 10 Creators Update: What's new in Bash/WSL & Windows Console](https://blogs.msdn.microsoft.com/commandline/2017/04/11/windows-10-creators-update-whats-new-in-bashwsl-windows-console/) by Rich Turner.
* [ICYMI – Your weekly TL;DR](http://blogs.windows.com/buildingapps/2017/04/14/icymi-weekly-tldr-10/) By Windows Apps Team.
* [COM Server and OLE Document support for the Desktop Bridge](http://blogs.windows.com/buildingapps/2017/04/13/com-server-ole-document-support-desktop-bridge/) By Adam Braden.
* [Monetizing your app: Advertisement placement](https://blogs.windows.com/buildingapps/2017/04/10/monetizing-app-advertisement-placement/) By Kiran Bangalore.

Game Development
----------------

* [Inventory and Store System - Part 5.1 (Creating the Player Inventory)](https://channel9.msdn.com/Shows/dotGAME/Inventory-and-Store-System-Part-51-Creating-the-Player-Inventory) by Stacey Haffner.
* [Microsoft Launches Xbox Academy, Free Xbox And PC Game Development Classes](https://www.gamespot.com/articles/microsoft-launches-xbox-academy-free-xbox-and-pc-g/1100-6449259/).
* [Inside the next Xbox: Project Scorpio and its brand-new dev kit](http://www.gamasutra.com/view/news/295800/Inside_the_next_Xbox_Project_Scorpio_and_its_brandnew_dev_kit.php) by Alex Wawro.
* [CRYENGINE 5.3.4 is now available for download](https://www.cryengine.com/news/cryengine-534-is-now-available-for-download).
* [Is Horizon's UI Design good? - The UI Show](https://youtu.be/rpFmD0YgyhA) by The UI Show.
* [[Unity 5.5] Tutorial: How to create depth of field in unity (like in GTA)](https://youtu.be/xlpHmxNtiT8) by Gamad.
* [Unity - Loading a JSON Collection](https://youtu.be/M-r4l-OcZtw) by FirstGearGames.
* [Asset Bundles vs. Resources: A Memory Showdown](https://blogs.unity3d.com/2017/04/12/asset-bundles-vs-resources-a-memory-showdown/) by Ryan Caltabiano.
* [(Unity) 2017.1.0 Beta 1 is available!](https://forum.unity3d.com/threads/2017-1-0-beta-1-is-available.466064/) by Charles_Beauchemin.

And this is it for this week!

Contribute to the week in .NET
------------------------------

As always, this weekly post couldn't exist without community contributions, and I'd like to thank all those who sent links and tips. The F# section is provided by [Phillip Carter](https://twitter.com/_cartermp), the gaming section by [Stacey Haffner](https://twitter.com/yecats131), the Xamarin section by [Dan Rigby](https://twitter.com/DanRigby), and the UWP section by [Michael Crump](http://twitter.com/mbcrump).

You can participate too. Did you write a great blog post, or just read one? Do you want everyone to know about an amazing new contribution or a useful library? Did you make or play a great game built on .NET?
We'd love to hear from you, and feature your contributions on future posts:

* Send an email to beleroy at Microsoft,
* [Comment on this gist](https://gist.github.com/bleroy/ba8835cee4d60422f3d053deea104563)
* Leave us a pointer in the comments section below.
* [Send Stacey (@yecats131) tips on Twitter about .NET games](https://twitter.com/yecats131).

This week's post (and future posts) also contains news I first read on [The ASP.NET Community Standup](https://blogs.msdn.microsoft.com/webdev/tag/communitystandup/), on [Weekly Xamarin](http://weeklyxamarin.com/), on [F# weekly](https://sergeytihon.wordpress.com/category/f-weekly/), and on [The Morning Brew](http://themorningbrew.net/).
