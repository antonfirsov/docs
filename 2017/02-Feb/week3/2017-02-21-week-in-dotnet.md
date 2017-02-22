The week in .NET - On .NET with Beth Massi
================================================================================

Previous posts:
* [On .NET with Phil Haack, Readline](https://blogs.msdn.microsoft.com/dotnet/2017/02/14/the-week-in-net-on-net-with-phil-haack-readline/).
* [On .NET on Docker and new Core tooling, Benchmark.NET, Magicka](https://blogs.msdn.microsoft.com/dotnet/2017/02/07/the-week-in-net-on-net-on-docker-and-new-core-tooling-benchmark-net-magicka/).
* [On .NET on public speaking, ndepend, CrazyCore, The Perils of Man](https://blogs.msdn.microsoft.com/dotnet/2017/01/31/the-week-in-net-on-net-on-public-speaking-ndepend-crazycore-the-perils-of-man/).

.NET Foundation
---------------

The [.NET Foundation](https://dotnetfoundation.org/) has [a new Executive Director, Jon Galloway](https://dotnetfoundation.org/blog/welcoming-jon-galloway-as-the-new-executive-director-of-the-net-foundation). Jon replaces Martin Woodward.

On .NET
-------

In [last week's episode](https://channel9.msdn.com/Shows/On-NET/Beth-Massi-Happy-Anniversary-NET), we're speaking with [Beth Massi](https://twitter.com/bethmassi) to celebrate .NET's 15th anniversary:

<iframe src="https://channel9.msdn.com/Shows/On-NET/Beth-Massi-Happy-Anniversary-NET/player" width="960" height="540" allowFullScreen frameBorder="0"></iframe>

This week, [Eric Mellino](https://github.com/mellinoe) will be on the show to demo [CrazyCore](https://github.com/mellinoe/CrazyCore), a game engine written on .NET Core. We'll stream live [on Channel 9](https://channel9.msdn.com/Shows/On-NET). We'll take questions on [Gitter's dotnet/home channel](https://gitter.im/dotnet/home) and on Twitter. Please use the `#onnet` tag. It's OK to start sending us questions in advance if you can't do it live during the shows.

Package of the week: NeinLinq
-----------------------------

[NeinLinq](https://github.com/axelheer/nein-linq) provides helpful extensions for using LINQ providers such as Entity Framework that support only a subset of .NET functions, reusing functions, rewriting queries, even making them null-safe, and building dynamic queries using translatable predicates and selectors.

Here's an example of a Linq expression that uses a custom function that would otherwise get rejected as not translatable:

```csharp
[InjectLambda]
public static string LimitText(this string value, int maxLength)
{
    if (value != null && value.Length > maxLength)
        return value.Substring(0, maxLength);
    return value;
}

public static Expression<Func<string, int, string>> LimitText()
{
    return (v, l) => v != null && v.Length > l ? v.Substring(0, l) : v;
}
```

```csharp
from d in data.ToInjectable()
select new
{
    Id = d.Id,
    Value = d.Name.LimitText(10)
}
```

User group meeting of the week: Unit Testing in Edmonton, AB
------------------------------------------------------------

The [Edmonton .NET user group](https://www.meetup.com/Edmonton-NET-User-Group/) is meeting on [Wednesday at 6:00PM for a session on unit testing](https://www.meetup.com/Edmonton-NET-User-Group/events/236547165/).

.NET
----

* [How to evaluate info you read on garbage collectors](https://blogs.msdn.microsoft.com/maoni/2017/02/18/how-to-evaluate-info-you-read-on-garbage-collectors/) by Maoni Stephens.
* [A common execution path optimization](https://blogs.msdn.microsoft.com/seteplia/2017/02/21/a-common-execution-path-optimization/) by Sergey Teplyakov.
* [Storyteller 4.0 is Out!](https://jeremydmiller.com/2017/02/15/storyteller-4-0-is-out/) by Jeremy D. Miller.
* [The Issue With Scoped Async Synchronization Constructs](http://blog.i3arnon.com/2017/02/21/task-wrapper/) by Bar Arnon.
* [C# 7.0 – Pattern Matching](https://csharp.christiannagel.com/2017/02/15/patternmatching/) by Christian Nagel.
* [How to create a dotnet new project template in .NET Core](http://pioneercode.com/post/how-to-create-a-dot-net-new-project-template-in-dot-net-core) by Chad Ramos.
* [65535 interfaces ought to be enough for anybody](http://aakinshin.net/en/blog/dotnet/mono-and-65535interfaces/) by Andrey Akinhin.
* [State of the union: ReSharper C# 7 and VB.NET 15 support](https://blog.jetbrains.com/dotnet/2017/02/17/state-union-resharper-c-7-vb-net-15-support/) by Maarten Balliauw.
* [.NET Core Ecosystem - My thoughts](http://malisancubeblog.azurewebsites.net/net-core-ecosystem-my-thoughts/) by Malisa Ncube.
* [Migrating an existing .NET Core to csproj](http://michaelcrump.net/part6-aspnetcore/) by Michael Crump.

ASP.NET
-------

I'm at the [Orchard Harvest conference](http://orchardharvest.org/) this week, watching some awesome talk from kickass speakers such as [Sébastien Ros](http://orchardharvest.org/sessions/building-modules-for-orchard-core-cms), [Taylor Mullen](http://orchardharvest.org/sessions/what-s-new-in-asp-net-mvc-core-2-0), [Nick Mayne](http://orchardharvest.org/sessions/what-is-orchard-core-saas-framework), and others. I'll be talking tomorrow about [.NET Core, .NET Standard 2.0, and C# 7](http://orchardharvest.org/sessions/what-s-up-with-net-core-2-0-and-c-7). I've also been [live-blogging the whole thing](https://weblogs.asp.net/bleroy/Tags/Harvest). All the talks are recorded and will be available soon.

* [Overriding ASP.NET Core Framework-Provided Services](http://davidpine.net/blog/overriding-default-di/) by David Pine.
* [Understanding your middleware pipeline with the Middleware Analysis package](https://andrewlock.net/understanding-your-middleware-pipeline-with-the-middleware-analysis-package/) by Andrew Lock.
* [Let's Try WCF Self-Hosted Services in a Container](https://blogs.msdn.microsoft.com/webdev/2017/02/20/lets-try-wcf-self-hosted-services-in-a-container/) by Jeffrey T. Fritz.
* [Enhanced Selenium WebDriver Tests with the New Improved C# 6.0](https://automatetheplanet.com/selenium-webdriver-tests-csharp-six/) by Anton Angelov.
* [ASP.NET Core CSRF defence with Antiforgery](http://www.dotnetcurry.com/aspnet/1343/aspnet-core-csrf-antiforgery-token) by Daniel Jimenez Garcia.
* [Minify CSS and JavaScript files with Visual Studio and ASP.NET Core](https://www.softfluent.com/blog/dev/2017/01/20/Minify-CSS-and-JavaScript-files-with-Visual-Studio-and-ASP-NET-Core) by Gérald Barré.
* [Real time ASP.NET Core](https://radu-matei.github.io/blog/real-time-aspnet-core/) by Radu Matei.
* [Enabling Cross-Origin Requests In ASP.NET Core](http://www.c-sharpcorner.com/article/enabling-cross-origin-requests-in-asp-net-core/) by Jignesh Trivedi.
* [AttributeAuthorization with Custom Roles in ASP.NET Core](https://www.codeproject.com/Articles/1171299/AttributeAuthorization-with-Custom-Roles-in-ASP-NE) by Mosti16.
* [ASP.NET Core logging: what still works and what changed?](https://stackify.com/asp-net-core-logging-what-changed/) by Matt Watson.
* [Returning raw JSON data in Web API with Marten](https://visualstudiomagazine.com/articles/2017/02/01/returning-raw-json.aspx) by Jason roberts.
* [HTTP/2 Server Push and ASP.NET MVC - Cache Digest](http://tpeczek.blogspot.co.uk/2017/01/http2-server-push-and-aspnet-mvc-cache.html) by Tomasz Pęczek.

F#
--

* [Easy start with F# in Visual Studio Code](https://medium.com/@equisept/easy-start-with-f-in-visual-studio-code-fbf609166a0d#.rrw7k8a4d) by Equisept.
* [Thirteen Ways of Looking at a Turtle](https://www.youtube.com/watch?v=AG3KuqDbmhM), by Scott Wlaschin.
* [Particle Filter in F#](http://www.taumuon.co.uk/2017/02/particle-filter-in-f.html), by Gary Evans
* [Use JS local storage with ListModel with WebSharper UI.Next in F#](https://kimsereyblog.blogspot.com.by/2017/02/use-local-storage-with-listmodel-with.html), by Kimsery Lam.
* [F# – Mathematical expressiveness](http://blog.stermon.com/articles/2017/02/13/fsharp-mathematical-expressiveness-of-fsharp), by Ramón Soto Mathiesen.
* [Discussion: Autocompletion Behavior](https://github.com/Microsoft/visualfsharp/issues/2432).

New F# Language Suggestions:

- [Allow simple arithmetic in number literals](https://github.com/fsharp/fslang-suggestions/issues/539).
- [Erased union types (like Typescript union types)](https://github.com/fsharp/fslang-suggestions/issues/538).
- [Provide predefined deconstructor for F# records](https://github.com/fsharp/fslang-suggestions/issues/537).

Check out [F# Weekly](https://sergeytihon.wordpress.com/category/f-weekly/) for more great content from the F# community.

Xamarin
-------

* [Xamarin Release Candidate: Cycle 9 RC Refresh](https://releases.xamarin.com/release-candidate-cycle-9-rc6-refresh/) by Bri Brothers.
* [Building Android Apps with Entity Framework](https://blog.xamarin.com/building-android-apps-with-entity-framework/) by Jon Douglas.
* [Mobile Center Webinar Recordings - Ship Mobile Apps Faster and Give Your Apps an Instant Cloud Backend](https://blog.xamarin.com/mobile-center-webinar-recordings-ship-mobile-apps-faster-and-give-your-apps-an-instant-cloud-backend/) by Courtney Witmer.
* [Bring Stunning Animations to Your Apps with Lottie](https://blog.xamarin.com/bring-stunning-animations-to-your-apps-with-lottie/) by Martijn van Dijk.
* [Realtime Databases with the Realm Mobile Platform](https://blog.xamarin.com/shared-drawing-with-the-realm-mobile-platform/) by Andy Dent.
* [Consumable In-App Purchases](https://blog.xamarin.com/consumable-in-app-purchases/) by James Montemagno.
* [The Xamarin Show 15: Appium Mobile Automation with Glenn Wester](https://channel9.msdn.com/Shows/XamarinShow/The-Xamarin-Show-15-Appium-Mobile-Automation-with-Glenn-Wester) by James Montemagno.
* [Create cross-device experiences with the Project Rome SDK for Android](https://blogs.msdn.microsoft.com/jgalasyn/2017/02/09/create-cross-device-experiences-with-the-project-rome-sdk-for-android/) by Jim Galasyn.
* [Entity Framework Core with Xamarin Forms](https://xamarinhelp.com/entity-framework-core-xamarin-forms/), [Patterns for Referencing Dependencies in Cross Platform Development](https://xamarinhelp.com/patterns-referencing-dependencies-cross-platform-development/), & [Visual Studio 2017, .NET Standard and Xamarin](https://xamarinhelp.com/visual-studio-2017-net-standard-xamarin/) by Adam Pedley.
* [Sending Files to a Xamarin.Forms App – Part 1: iOS](https://codemilltech.com/sending-files-to-a-xamarin-forms-app-part-1-ios/) and [part 2: Android](https://codemilltech.com/sending-files-to-a-xamarin-forms-app-part-2-android/) by Matthew Soucoup.
* [Calligraphy with MvvmCross](http://smstuebe.de/2017/02/12/mvvmcross-calligraphy/) by Sven-Michael Stübe.
* [Call an Azure AD protected API in Xamarin/UWP apps](http://timothelariviere.com/2017/02/13/call-an-azure-ad-protected-api-in-xamarinuwp-apps/) by Timothé Larivière.
* [Designer Support for FloatLabeledEntry](http://gregshackles.com/designer-support-for-floatlabeledentry/) by Greg Shackles.
* [Xamarin.Android Continuous Integration with Visual Studio Team Services](https://alexdunn.org/2017/02/10/xamarin-android-continuous-integration-with-visual-studio-team-service/) by Alex Dunn.
* [Using an AutomationId with a Cell in Xamarin.Forms](https://windingroadway.blogspot.com/2017/02/using-automationid-with-cell-in.html) by Kevin Ford.
* [Update on the many flavors of HttpClient](http://kerry.lothrop.de/httpclient-flavors-update/) by Kerry W. Lothrop.
* [Unable to start Build 4.3.0.664 agent. when building iOS apps](http://davidyardy.com/archive/unable-to-start-build-430664-agent-when-building-ios-apps/) by David Yardy.
* [To Use Or Not To Use: Touch Gesture Controls For Mobile Interfaces](https://www.smashingmagazine.com/2017/02/touch-gesture-controls-mobile-interfaces/) by Kyle Sanders.

UWP
----

* [Cognitive Services APIs: Vision](https://blogs.windows.com/buildingapps/2017/02/13/cognitive-services-apis-vision/) by Windows Apps Team.
* [Implementing a type converter in UWP XAML](http://timheuer.com/blog/archive/2017/02/15/implement-type-converter-uwp-winrt-windows-10-xaml.aspx) by Tim Heuer
* [#Hololens – #SpectatorView, time to use my Credit Card again!](https://elbruno.com/2017/02/15/hololens-spectatorview-time-to-use-my-credit-card-again/) by El Bruno
* [Cognitive Services APIs: Speech](https://blogs.windows.com/buildingapps/2017/02/14/cognitive-services-apis-speech/) by Windows Apps Team.
* [Cognitive Services APIs: Language](https://blogs.windows.com/buildingapps/2017/02/15/cognitive-services-apis-language/) by Windows Apps Team.
* [Cognitive Services APIs: Knowledge](https://blogs.windows.com/buildingapps/2017/02/16/cognitive-services-apis-knowledge/) by Windows Apps Team.
* [Cognitive Services APIs: Search](https://blogs.windows.com/buildingapps/2017/02/17/cognitive-service-api-search/) by Windows Apps Team.

Azure
-----

* [Running ASP.NET Core applications in Azure App Service](https://shellmonger.com/2017/02/16/running-asp-net-core-applications-in-azure-app-service/) by Adrian Hall.
* [Logging To Application Insights In Azure Functions](http://geekswithblogs.net/tmurphy/archive/2017/02/16/logging-to-application-insights-in-azure-functions.aspx) by Tim Murphy.
* [Optimistic Concurrency in DocumentDB](https://codeopinion.com/documentdb-optimistic-concurrency/) by Derek Comartin.

Games
-----

* [The MonoGame Game Loop (just like the XNA Game Loop)](http://geekswithblogs.net/cwilliams/archive/2017/02/13/237027.aspx) and [Putting a Sprite onscreen](http://geekswithblogs.net/cwilliams/archive/2017/02/16/242270.aspx) by Chris G. Williams.

And this is it for this week!

Contribute to the week in .NET
------------------------------

As always, this weekly post couldn't exist without community contributions, and I'd like to thank all those who sent links and tips. The F# section is provided by [Phillip Carter](https://twitter.com/_cartermp), the gaming section by [Stacey Haffner](https://twitter.com/yecats131), and the Xamarin section by [Dan Rigby](https://twitter.com/DanRigby), and the UWP section by [Michael Crump](http://twitter.com/mbcrump).

You can participate too. Did you write a great blog post, or just read one? Do you want everyone to know about an amazing new contribution or a useful library? Did you make or play a great game built on .NET?
We'd love to hear from you, and feature your contributions on future posts:

* Send an email to beleroy at Microsoft,
* [comment on this gist](https://gist.github.com/bleroy/d34a36769b00113c63238b010ec4832d)
* Leave us a pointer in the comments section below.
* [Send Stacey (@yecats131) tips on Twitter about .NET games](https://twitter.com/yecats131).

This week's post (and future posts) also contains news I first read on [The ASP.NET Community Standup](https://blogs.msdn.microsoft.com/webdev/tag/communitystandup/), on [Weekly Xamarin](http://weeklyxamarin.com/), on [F# weekly](https://sergeytihon.wordpress.com/category/f-weekly/), and on [Chris Alcock's The Morning Brew](http://themorningbrew.net/).
