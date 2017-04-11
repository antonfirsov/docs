The week in .NET - .NET Framework 4.7, reference documentation, On .NET on modular ASP.NET, Happy birthday .NET with Immo Landwerth, JustAssembly
=============================================

Previous posts:

* [On .NET on SonarLint and SonarQube, Happy birthday .NET with Dan Fernandez, nopCommerce, Steve Gordon](https://blogs.msdn.microsoft.com/dotnet/2017/04/04/the-week-in-net-on-net-on-sonarlint-and-sonarqube-happy-birthday-net-with-dan-fernandez-nopcommerce-steve-gordon/)
* [On .NET with Sidharth Gupta on Tizen, Happy birthday .NET with Bertrand Le Roy, JSON.NET 10, Gunnar Peipman](https://blogs.msdn.microsoft.com/dotnet/2017/03/28/the-week-in-net-on-net-with-sidarth-gupta-on-tizen-happy-birthday-net-with-bertrand-le-roy-json-net-10-gunnar-peipman/)
* [Happy birthday .NET with Mads Torgersen, Coypu](https://blogs.msdn.microsoft.com/dotnet/2017/03/21/the-week-in-net-happy-birthday-net-with-mads-torgersen-coypu/)

.NET Framework 4.7
------------------

This week, we announced the release of the .NET Framework 4.7. We’ve added support for targeting the .NET Framework 4.7 in [Visual Studio 2017, also updated today](https://blogs.msdn.microsoft.com/visualstudio/2017/04/05/visual-studio-2017-update/).

The .NET Framework 4.7 includes improvements in several areas:

* High DPI support for Windows Forms applications on Windows 10
* Touch support for WPF applications on Windows 10
* Enhanced cryptography support
* Performance and reliability improvements

You can see the complete list of improvements and the API diff in the [.NET Framework 4.7 release notes](https://github.com/Microsoft/dotnet/tree/master/releases/net47/README.md).

Read the blog post: [Announcing the .NET Framework 4.7](https://blogs.msdn.microsoft.com/dotnet/2017/04/05/announcing-the-net-framework-4-7/) by Rich Lander.

New .NET reference documentation
--------------------------------

Almost a year ago, we piloted the .NET Core reference documentation on [docs.microsoft.com](https://docs.microsoft.com/en-us/dotnet/articles/core/). Today we are happy to announce our unified [.NET API reference experience](https://docs.microsoft.com/dotnet/api). We understand that developer productivity is key - from a hobbyist developer, to a startup, to an enterprise. With that in mind, we partnered closely with the Xamarin team to standardize how we document, discover, and navigate .NET APIs at Microsoft.

* [Announcing a unified .NET reference experience on docs.microsoft.com](https://docs.microsoft.com/en-us/teamblog/announcing-unified-dotnet-experience-on-docs) by Jeff Sandquist.
* [The new .NET reference documentation](https://docs.microsoft.com/dotnet/api)

On .NET
-------

Last week, [Sébastien Ros was back on the show](https://channel9.msdn.com/Shows/On-NET/Sbastien-Ros-Modular-ASPNET-apps) to demo the fantastic support for modularity that was built for [Orchard Core](https://github.com/orchardcms/orchard2), that can now be used in any ASP.NET Core application:

<iframe src="https://channel9.msdn.com/Shows/On-NET/Sbastien-Ros-Modular-ASPNET-apps/player" width="960" height="540" allowFullScreen frameBorder="0"></iframe>

Happy birthday .NET with Immo Landwerth
---------------------------------------

Back in February we threw a party for the 15th anniversary of .NET. We caught up with Immo Landwerth, a program manager on the .NET team at Microsoft, who joined Microsoft in 2010. He tells us about his journey from being a customer using .NET to an employee and the cultural changes he's witnessed as .NET has moved to open source.

<iframe src="https://channel9.msdn.com/Blogs/funkyonex/Happy-Birthday-NET-with-Immo-Landwerth/player" width="960" height="540" allowFullScreen frameBorder="0"></iframe>

Tool of the week: JustAssembly
------------------------------

This week, [Telerik](http://www.telerik.com/) introduced [JustAssembly](http://www.telerik.com/justassembly), a free utility tool that compares two .NET assemblies and shows the differences in each assembly code line by line.

![JustAssembly](http://d585tldpucybw.cloudfront.net/sfimages/default-source/blogs/2017/justassembly2.png)

Read Stefan Stefanov's [blog post introducing the tool](http://www.telerik.com/blogs/ensure-version-compatibility-with-justassembly).

Meetups of the week: VS 2017, AppInsights, and IoT in Adelaide
--------------------------------------------------------------

The [Adelaide .NET User Group](https://www.meetup.com/Adelaide-dotNET/) holds a Visual Studio 2017 [launch event on April 12 at 5:30PM](https://www.meetup.com/Adelaide-dotNET/events/237629165/) with a talk from Paul Usher on AppInsight and another on IoT with Jack Ni.

.NET
----

* [.NET Framework April 2017 Monthly Rollup](https://blogs.msdn.microsoft.com/dotnet/2017/04/11/net-framework-april-2017-monthly-rollup/) by Rich Lander.
* [Introducing Windows Forms HDPI Improvements in .NET Framework 4.7](https://channel9.msdn.com/Blogs/dotnet/Introducing-Windows-Forms-HDPI-Improvements-in-NET-Framework-47) by Stacey Haffner and Merrie McGaw.
* [Contributing to Open-Source - My first Roslyn pull request - Getting the environment ready](https://blog.decayingcode.com/post/contributing-to-open-source-my-first-roslyn-pull-request-getting-the-environment-ready/) and [Fixing the bug](https://blog.decayingcode.com/post/contributing-to-open-source-my-first-roslyn-pull-request-fixing-the-bug/) by Maxime Rouiller.
* [MSTest V2 is open source](https://blogs.msdn.microsoft.com/visualstudioalm/2017/04/05/mstest-v2-is-open-source/) by Pratap Lakshman.
* [Installing Ubuntu 16.04 on a Raspberry Pi 3, installing .NET Core 2, and running a sample .NET Core 2 app](https://jeremylindsayni.wordpress.com/2017/04/02/installing-ubuntu-16-04-on-a-raspberry-pi-3-installing-net-core-2-and-running-a-sample-net-core-2-app/) by Jeremy Lindsay.
* [Building a realtime server backend using the Orleans Actor system, Dotnet Core and Server-side Redux](https://medium.com/@MaartenSikkema/using-dotnet-core-orleans-redux-and-websockets-to-build-a-scalable-realtime-back-end-cd0b65ec6b4d) by Maarten Sikkema.
* [Extending .NET CLI with custom tools - dotnet init initializes your NuGet package](https://blog.maartenballiauw.be/post/2017/04/10/extending-dotnet-cli-with-custom-tools.html) by Maarten Balliauw.
* [RyuJIT and the never-ending ThreadAbortException](http://labs.criteo.com/2017/04/ryujit-never-ending-threadabortexception/) by Christophe Nasarre and Kevin Gosse.
* [Webinar recording: Exploring .NET’s memory management](Why VB2017 only supports consuming ref returning methods) by Maarten Balliauw.
* [Automapper for .NET Core](http://developer.telerik.com/content-types/podcast/automapper-net-core/) by Jimmy Bogard.
* [Implementing OpenID Implicit Flow using OpenIddict and Angular](https://damienbod.com/2017/04/11/implementing-openid-implicit-flow-using-openiddict-and-angular/) by Damien Bowden.

ASP.NET
-------

* [Getting started with ASP.NET Core](https://andrewlock.net/getting-started-with-asp-net-core/) by Andrew Lock.
* [JWT Validation and Authorization in ASP.NET Core](https://blogs.msdn.microsoft.com/webdev/2017/04/06/jwt-validation-and-authorization-in-asp-net-core/) by Jeffrey T. Fritz.
* [Using Consul for Service Discovery with ASP.NET Core](http://cecilphillip.com/using-consul-for-service-discovery-with-asp-net-core) by Cecil Phillip.
* [Adding favicons to your ASP.NET Core website with Real Favicon Generator](https://andrewlock.net/adding-favicons-to-your-asp-net-core-website-with-realfavicongenerator/) by Andrew Lock.
* [How to troubleshoot: “An error occurred while starting the application” in ASP.NET Core on IIS](https://scottsauber.com/2017/04/10/how-to-troubleshoot-an-error-occurred-while-starting-the-application-in-asp-net-core-on-iis/) by Scott Sauber.
* [Running multiple independent ASP.NET Core pipelines side by side in the same application](https://www.strathweb.com/2017/04/running-multiple-independent-asp-net-core-pipelines-side-by-side-in-the-same-application/) by Filip W.
* [ASP.NET Core Lazy Command Pattern](http://rehansaeed.com/asp-net-core-lazy-command-pattern/) by Muhammad Rehan Saeed.
* [Alba 1.0 – Recipes for ASP.Net Core Integration Testing](https://jeremydmiller.com/2017/04/05/alba-1-0-recipes-for-asp-net-core-integration-testing/) by Jeremy D. Miller.
* [HTTP/2 with Server Push proof of concept for ASP.NET Core HttpSysServer](http://www.tpeczek.com/2017/04/http2-with-server-push-proof-of-concept.html) by Tomasz Pęczek.
* [Hosted ASP.NET Core builds with AppVeyor (video)](https://aspnetmonsters.com/2017/04/monsters-weekly/ep97/) by the ASP.NET Monsters.

C#
--

* [Micro-Benchmarking the Three Ways to Cast Safely](https://www.danielcrabtree.com/blog/164/c-sharp-7-micro-benchmarking-the-three-ways-to-cast-safely) by Daniel Crabtree.
* [Is Operator Patterns - You won't need 'as' as often](https://www.danielcrabtree.com/blog/152/c-sharp-7-is-operator-patterns-you-wont-need-as-as-often) by Daniel Crabtree.
* [Dissecting Local Functions to understand how they capture local variables](https://www.danielcrabtree.com/blog/73/c-sharp-7-dissecting-local-functions-to-understand-how-they-capture-local-variables) by Daniel Crabtree.
* [Local Functions are Funcs too](https://www.danielcrabtree.com/blog/84/c-sharp-7-local-functions-are-funcs-too) by Daniel Crabtree.
* [Ref Returns, Ref Locals, and how to use them](https://www.danielcrabtree.com/blog/128/c-sharp-7-ref-returns-ref-locals-and-how-to-use-them) by Daniel Crabtree.
* [Try catch, finally throw — or Exception Handling 101 for .NET](https://medium.com/@alexyakunin/try-catch-finally-throw-or-exception-handling-101-9f824136b21b) by Alex Yakunin.

F#
--

* [Getting Started with .NET Core using F# (video)](https://www.youtube.com/watch?v=2xG31sUsCdc&feature=youtu.be).
* [Building a security testing service with F# (video)](https://www.youtube.com/watch?v=ZVvcWIjbbhk&feature=youtu.be), by William Blum.
* [Using F# to write serverless Azure functions](https://blogs.msdn.microsoft.com/uk_faculty_connection/2017/03/24/using-f-to-write-serverless-azure-functions), by Lee Stott.
* [Gram Schmidt in FSharp](http://jeremybellows.com/blog/Gram-Schmidt-in-FSharp), by Jeremy Bellows.
* [Slack TypeProvider](http://rflechner.github.io/SlackTypeProvider/#/), by Flechner Romain.
* [A Reusable ApiController Adapter](http://blog.ploeh.dk/2017/03/30/a-reusable-apicontroller-adapter/), by Mark Seemann.
* [Creating an Azure Functions solution diagram](http://brandewinder.com/2017/04/01/azure-function-app-diagram/), by Mathias Brandewinder.

New F# Language Suggestions:

- [Implicit interface implementation from an object expression](https://github.com/fsharp/fslang-suggestions/issues/555)
- [Implement `[<StructuralEquality>]` and `[<StructuralComparison>]` for simple class types](https://github.com/fsharp/fslang-suggestions/issues/554)

Check out [F# Weekly](https://sergeytihon.wordpress.com/category/f-weekly/) for more great content from the F# community.

VB
--

* [Why VB2017 only supports consuming ref returning methods](https://blogs.msdn.microsoft.com/vbteam/2017/04/06/why-vb2017-only-supports-consuming-ref-returning-methods/) by Anthony D. Green.

Xamarin
-------

* [Stable Release: 15.1](https://releases.xamarin.com/stable-release-15-1/) by Bri Brothers.
* [Stable Release: Xamarin.Forms 2.3.4.224](https://releases.xamarin.com/stable-release-xamarin-forms-2-3-4-224/) by David Ortinau.
* [Announcing Xamarin.Forms Stable Release 2.3.4](https://blog.xamarin.com/announcing-xamarin-forms-stable-release-2-3-4/) by David Ortinau.
* [Announcing Xamarin’s Xcode 8.3 Support](https://blog.xamarin.com/announcing-xamarins-xcode-8-3-support/) by Miguel de Icaza.
* [Securing Web Requests with TLS 1.2](https://blog.xamarin.com/securing-web-requests-with-tls-1-2/) by James Montemagno.
* [Spring into April with Xamarin Developer Events](https://blog.xamarin.com/spring-april-xamarin-developer-events/) by  Jayme Singleton.
* [Snack Pack 9: Installing Xamarin for Visual Studio 2017](https://channel9.msdn.com/Shows/XamarinShow/Snack-Pack-9-Installing-Xamarin-for-Visual-Studio-2017) by The Xamarin Show.
* [Xamarin.Controls – Xamarin.Forms PinView](https://alexdunn.org/2017/03/30/xamarin-controls-xamarin-forms-pinview/) by Alex Dunn.
* [Xamarin.Tips – Restrict the Length of Your Entry Text](https://alexdunn.org/2017/03/30/xamarin-tips-restrict-the-length-of-your-entry-text/) by Alex Dunn.
* [Xamarin.Tips – Sending Authorized SignalR Requests](https://alexdunn.org/2017/03/31/xamarin-tips-sending-authorized-signalr-requests/) by Alex Dunn.
* [Launching A Mobile App Via A URI Scheme](https://xamarinhelp.com/launching-mobile-app-via-uri-scheme/) by Adam Pedley.
* [MFractor – Productivity tools for Xamarin Studio](https://xamarinhelp.com/mfractor-productivity-tools-xamarin-studio/) by Adam Pedley.
* [Persist Whatever You Want With Xamarin.Forms](https://codemilltech.com/persist-whatever-you-want-with-xamarin-forms/) by Matthew Soucoup.
* [Set alternate app icons in iOS using Xamarin](http://www.marcofolio.net/xamarin/set_alternate_app_icons_in_ios_using_xamarin.html) by Marco Kuiper.
* [Validate input in Xamarin.Forms using INotifyDataErrorInfo, Behaviors, Effects, and Prism](http://www.davidezordan.net/blog/?p=8093) by Davide Zordan.
* [Solution: Xamarin.Forms Intellisense not working](http://bsubramanyamraju.blogspot.com/2017/04/solution-xamarinforms-intellisense-is.html) by Subramanyam Raju.

Azure
-----

* [DocumentDB Transactions from .NET](https://codeopinion.com/documentdb-transactions-from-net/) by Derek Comartin.
* [Blue Green Deployments with Azure - The Simple way](http://faesel.com/Blog/Post?postId=0fd2703c-33e5-423b-a791-bc32cbddf044) by Faesel Saeed.

UWP
----

* [Announcing UWP Community Toolkit 1.4](https://blogs.windows.com/buildingapps/2017/04/03/announcing-uwp-community-toolkit-1-4/) by David Catuhe.
* [ICYMI – Your weekly TL;DR](http://blogs.windows.com/buildingapps/2017/04/07/icymi-weekly-tldr-9/) By the Windows Apps Team.
* [Managing Windows IoT Core devices with Azure IoT Hub](https://blogs.windows.com/buildingapps/2017/04/07/managing-windows-iot-core-devices-azure-iot-hub/) By Artur Laksberg.
* [New Share Experience in Windows 10 Creators Update](https://blogs.windows.com/buildingapps/2017/04/06/new-share-experience-windows-10-creators-update/) By Juan Sebastian Oviedo.
* [Updating your tooling for Windows 10 Creators Update](http://blogs.windows.com/buildingapps/2017/04/05/updating-tooling-windows-10-creators-update/) By Clint Rutkas.
* [Windows 10 Creators Update and Creators Update SDK are Released](http://blogs.windows.com/buildingapps/2017/04/05/windows-10-creators-update-creators-update-sdk-released/) By Kevin Gallo.

Data
----

* [Step by step: Couchbase with .Net Core](https://carlos.mendible.com/2017/04/10/step-by-step-couchbase-with-net-core/) by Carlos Mendible.
* [Using ElasticSearch, Kibana, ASP.NET Core and Docker to Discover and Visualize data](http://www.dotnetcurry.com/aspnet/1354/elastic-search-kibana-in-docker-dotnet-core-app) by Daniel Jimenez Garcia.
* [Avoid Lazy Loading Entities in ASP.NET Applications](http://ardalis.com/avoid-lazy-loading-entities-in-asp-net-applications) by Steve Smith.

Game Development
----------------

* [Ludum Dare 38 Theme Slaughter](https://ldjam.com/events/ludum-dare/38/theme).
* [[MineCraft] It's time to discover... Marketplace!](https://minecraft.net/en-us/article/its-time-discover-marketplace).
* [How Firewatch’s UI enhances immersion](https://medium.com/the-cube/how-firewatchs-ui-enhances-immersion-18feddbc7857) by Abhishek Iyer.
* [The Job Simulator Postmortem](https://youtu.be/G5a5VIdjiJA) by lexander Schwartz and Devin Reimer
* ['Make me think, make me move': New Doom's deceptively simple design](http://www.gamasutra.com/view/news/295254/Make_me_think_make_me_move_New_Dooms_deceptively_simple_design.php) by Kris Graft.
* [11.5 Unity Tower defense tutorial - Frost and Storm debuffs](https://youtu.be/HKVnQ-Dkemw) by inScope Studios.
* [[Unity 5.5] Tutorial: How to create a Pickable Object](https://youtu.be/bi8Tm80qs5M) by Gamad.
* [Getting Started with MonoGame using 2D](https://youtu.be/6inkDfpUxAU) by  Simon "Darkside" Jackson.

And this is it for this week!

Contribute to the week in .NET
------------------------------

As always, this weekly post couldn't exist without community contributions, and I'd like to thank all those who sent links and tips. The F# section is provided by [Phillip Carter](https://twitter.com/_cartermp), the gaming section by [Stacey Haffner](https://twitter.com/yecats131), and the Xamarin section by [Dan Rigby](https://twitter.com/DanRigby), and the UWP section by [Michael Crump](http://twitter.com/mbcrump).

You can participate too. Did you write a great blog post, or just read one? Do you want everyone to know about an amazing new contribution or a useful library? Did you make or play a great game built on .NET?
We'd love to hear from you, and feature your contributions on future posts:

* Send an email to beleroy at Microsoft,
* [comment on this gist](https://gist.github.com/bleroy/5c7df3460c97cb4f78113e3a0ecd04d6)
* Leave us a pointer in the comments section below.
* [Send Stacey (@yecats131) tips on Twitter about .NET games](https://twitter.com/yecats131).

This week's post (and future posts) also contains news I first read on [The ASP.NET Community Standup](https://blogs.msdn.microsoft.com/webdev/tag/communitystandup/), on [Weekly Xamarin](http://weeklyxamarin.com/), on [F# weekly](https://sergeytihon.wordpress.com/category/f-weekly/), and on [The Morning Brew](http://themorningbrew.net/).
