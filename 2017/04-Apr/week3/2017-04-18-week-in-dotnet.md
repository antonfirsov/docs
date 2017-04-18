The week in .NET - 
=============================================

Previous posts:

* [.NET Framework 4.7, reference documentation, On .NET on modular ASP.NET, Happy birthday .NET with Immo Landwerth, JustAssembly](https://blogs.msdn.microsoft.com/dotnet/2017/04/11/the-week-in-net-net-framework-4-7-reference-documentation-on-net-on-modular-asp-net-happy-birthday-net-with-immo-landwerth-justassembly/)
* [On .NET on SonarLint and SonarQube, Happy birthday .NET with Dan Fernandez, nopCommerce, Steve Gordon](https://blogs.msdn.microsoft.com/dotnet/2017/04/04/the-week-in-net-on-net-on-sonarlint-and-sonarqube-happy-birthday-net-with-dan-fernandez-nopcommerce-steve-gordon/)
* [On .NET with Sidharth Gupta on Tizen, Happy birthday .NET with Bertrand Le Roy, JSON.NET 10, Gunnar Peipman](https://blogs.msdn.microsoft.com/dotnet/2017/03/28/the-week-in-net-on-net-with-sidarth-gupta-on-tizen-happy-birthday-net-with-bertrand-le-roy-json-net-10-gunnar-peipman/)

On .NET
-------

This week on the show, we'll speak with [Don Schenck](https://twitter.com/DonSchenck) about [Red Hat](https://www.redhat.com/en). We'll take questions on Gitter, on [the dotnet/home channel](https://gitter.im/dotnet/home) and on Twitter. Please use the `#onnet` tag. It's OK to start sending us questions in advance if you can't do it live during the show.

Happy birthday .NET with 
---------------------------------------


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

* [An interesting (if unsuccessful) look into predicting horse races via machine learning with F#](https://medium.com/@ThisisZone/an-interesting-if-unsuccessful-look-into-predicting-horse-races-via-machine-learning-with-f-7563090c7582) by Zone.

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
