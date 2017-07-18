---
title: The week in .NET - Command Line Parser Library, .NET South East
keywords: Week in .NET, community, .NET
weblogName: .NET Blog
dontInferFeaturedImage: true
---
Previous posts:

* [Links!](https://blogs.msdn.microsoft.com/dotnet/2017/07/11/the-week-in-net-links-2/)
* [Links!](https://blogs.msdn.microsoft.com/dotnet/2017/07/04/the-week-in-net-links/)
* [.NET Conf, Material Design in XAML Toolkit](https://blogs.msdn.microsoft.com/dotnet/2017/06/27/the-week-in-net-net-conf-material-design-in-xaml-toolkit/)

## Package of the week: Command Line Parser Library

Command-line applications typically accept arguments and options, and expose a `--help` page describing them. Parsing those arguments and options is a repetitive task that .NET provides little help for out of the box, beyond the `string[] args` argument to `main`. [Giacomo Stelluti Scala](https://github.com/gsscoder)'s [Command Line Parser Library](https://github.com/gsscoder/commandline) offers CLR applications a clean and concise API for manipulating command line arguments and related tasks, such as defining switches, options and verb commands. It allows you to display a help screen with a high degree of customization and a simple way to report syntax errors to the end user. Everything that is boring and repetitive about parsing command line arguments is delegated to the library, letting developers concentrate on core logic. It's written in C# and doesn't depend on other packages. It's also freindly to F# and VB developers.

First, define the options the application expects:

```csharp
class Options {
  [Option('r', "read", Required = true,
    HelpText = "Input files to be processed.")]
  public IEnumerable<string> InputFiles { get; set; }

  // Omitting long name, default --verbose
  [Option(
    HelpText = "Prints all messages to standard output.")]
  public bool Verbose { get; set; }

  [Option(Default = "中文",
    HelpText = "Content language.")]
  public string Language { get; set; }

  [Value(0, MetaName = "offset",
    HelpText = "File offset.")]
  public long? Offset { get; set; }
}
```

Then consume them:

```csharp
static int Main(string[] args) {
  var options = new Options();
  var isValid = CommandLine.Parser.Default.ParseArgumentsStrict(args, options);
```

* [Command Line Parser Library on GitHub](https://github.com/gsscoder/commandline)
* [Command Line Parser Library on Nuget](https://www.nuget.org/packages/CommandLineParser/)

## User group of the week: .NET South East (Brighton, UK)

User groups are an essential part of the .NET Community, and none of them has existed forever. The creation of a new user group is an occasion for celebration. Steve Gordon, a regular of this column, did just that, and created the .NET South East user group in Brighton in the United Kingdom. I really like that Steve didn't stop there, and went on explaining why and how he created the group in [a great blog post](https://www.stevejgordon.co.uk/announcing-dotnet-south-east-user-group). Hopefully, this can inspire others in the .NET community to create their own group: if there's no user group in your area, but you know the users are there, just go ahead and fill that void!

* [Announcing .NET South East,a new Brighton based .NET User Group](https://www.stevejgordon.co.uk/announcing-dotnet-south-east-user-group), by Steve Gordon.

## .NET

* [Docker for .NET Developers (Part 7) - Setting up Amazon EC2 Container Registry](https://www.stevejgordon.co.uk/docker-for-net-developers-part-7) by Steve Gordon.
* [.NET Core Design Reviews: System.IO.Pipelines](https://www.youtube.com/watch?v=IXg58zMsPug) by Immo Landwerth.
* [Calling a custom executable from Cake using StartProcess and ProcessSettings](https://jeremylindsayni.wordpress.com/2017/07/17/calling-a-custom-executable-from-cake-using-startprocess-and-processsettings/) by Jeremy Lindsay.
* [How to build a .NET Core Project with VS Code ?](https://joffreykern.github.io/blog/how-to-build-dotnet-core-project-with-vs-code) by Joffrey Kern.
* [Good citizenship - logging from .NET libraries](https://nblumhardt.com/2017/07/library-logging/) by UNKNOWN.
* [Dotnet new templates for AWS Lambda and Raspberry Pi](https://carlos.mendible.com/2017/07/18/dotnet-new-templates-for-aws-lambda-and-raspberry-pi/) by Carlos Mendible.
* [dotnet CLI – how to update a NuGet package and add a new NuGet package](https://codingblast.com/update-nuget-package-dotnet-cli/) by Ibrahim Šuta.
* [.NET Framework July 2017 Security and Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2017/07/11/net-framework-july-2017-security-and-quality-rollup/) by Rich Lander.
* [Samsung Releases 4th Preview of Visual Studio Tools for Tizen including support for .NET Core 2.0 Preview](https://blogs.msdn.microsoft.com/visualstudio/2017/07/11/samsung-releases-4th-preview-of-visual-studio-tools-for-tizen-including-support-for-net-core-2-0-preview/) by Scott Hunter.
* [Introducing Unity 2017](https://blogs.unity3d.com/2017/07/11/introducing-unity-2017/) by Alex Lian.
* [Introduction to MSBuild in .NET Core](https://channel9.msdn.com/Shows/Code-Conversations/Introduction-to-MSBuild-in-NET-Core-with-Nate-McMaster) by Nate McMaster.
* [Live coding an Alexa Smart Home skill from scratch in C#](http://damianblog.com/2017/06/05/live-coding-an-alexa-smart-home-skill-from-scratch-in-csharp/) by Damian Mehers.
* [Making any call to a function of an object thread safe](http://msprogrammer.serviciipeweb.ro/2017/07/10/making-any-call-to-a-function-of-an-object-thread-safe/) by Andrei Ignat.
* [Use this helper CLI for switching .NET Core SDK versions](http://reynders.co/use-this-helper-cli-for-switching-net-core-sdk-versions/) by Fanie Reynders.
* [.NET Core on Circle CI 2.0 using Docker and Cake](https://adamhathcock.blog/2017/07/12/net-core-on-circle-ci-2-0-using-docker-and-cake/) by Adam Hathcock.

## ASP.NET

* [Injecting content into your head or body tags via dependency injection using ITagHelperComponent](http://josephwoodward.co.uk/2017/07/injecting-javascript-into-views-using-itaghelpercomponent) by Joseph Woodward.
* [An Angular 4 .Net Core Application Updater](http://lightswitchhelpwebsite.com/Blog/tabid/61/EntryId/4309/An-Angular-4-Net-Core-Application-Updater.aspx) by Michael Washington.
* [Creating custom password validators for ASP.NET Core Identity](https://andrewlock.net/creating-custom-password-validators-for-asp-net-core-identity-2/) by Andrew Lock.
* [Minimal ASPNET Core Web API](https://ardalis.com/minimal-aspnet-core-web-api) by Steve Smith.
* [Building Reusable UI Components in ASP.NET Core](http://developer.telerik.com/topics/net/building-reusable-ui-components-in-asp-net-core/) by Scott Addie.
* [Set User identity and IsAuthenticated in ASP.NET MVC Core controller tests](http://hudosvibe.net/post/mock-user-identity-in-asp.net-mvc-core-controller-tests) by Hrvoje Hudoletnjak.
* [Setting up Webpack in ASP.NET Web Forms](http://cecilphillip.com/setting-up-webpack-in-asp-net-web-forms/) by Cecil Phillip.
* [How to get the correct Request.Url when behind a load balancer](http://codeclimber.net.nz/archive/2017/07/14/how-to-get-the-correct-requesturl-when-behind-a-load-balancer/) by Simone Chiaretta.
* [Development time IIS support for ASP.NET Core Applications](https://blogs.msdn.microsoft.com/webdev/2017/07/13/development-time-iis-support-for-asp-net-core-applications/) by Sourabh Shirhatti.
* [Adding an external Microsoft login to IdentityServer4](https://damienbod.com/2017/07/11/adding-an-external-microsoft-login-to-identityserver4/) by Damien Bowden.
* [Implementing Two-factor authentication with IdentityServer4 and Twilio](https://damienbod.com/2017/07/14/implementing-two-factor-authentication-with-identityserver4-and-twilio/) by Damien Bowden.
* [ASP.NET Core MVC – Form Tag Helpers](https://codingblast.com/asp-net-core-mvc-form-tag-helpers/) by Ibrahim Šuta.
* [Use brotli compression with ASP.NET Core](https://www.meziantou.net/2017/07/17/use-brotli-compression-with-asp-net-core) by Gérald Barré.
* [Anti-Forgery Validation in ASP.NET Core](https://www.red-gate.com/simple-talk/dotnet/asp-net/anti-forgery-validation-asp-net-core/) by Dino Esposito.
* [A guide to caching in ASP.NET Core](https://www.devtrends.co.uk/blog/a-guide-to-caching-in-asp.net-core) by Paul Hiles.
* [ASP.NET Core Demystified - Model Binding in MVC](https://www.exceptionnotfound.net/asp-net-core-demystified-model-binding-in-mvc/) by Matthew Jones.
* [Test Automation Using Atata: Handle Confirmation Popups](https://www.codeproject.com/Articles/1194445/Test-Automation-Using-Atata-Confirmation-Popups) by Yevgeniy Shunevych.
* [Blazor Brings .NET Back to the Browser](https://www.infoq.com/news/2017/07/Blazor) by InfoQ.

## C#

* [Revisions to previous discussion of the implementation of anonymous methods in C#](https://blogs.msdn.microsoft.com/oldnewthing/20170717-00/?p=96625) by Raymond Chen.
* [Join Null Check with Assignment](https://colinmackay.scot/2017/07/16/join-null-check-with-assignment/) by Colin Angus Mackay.
* [Pattern Matching In C#](https://l-paathshaala.com/2017/07/10/pattern-matching-in-c/) by Sandeep Shekhawat.
* [Ref, Ref Return and Ref Local In C#](https://l-paathshaala.com/2017/07/17/ref-ref-return-and-ref-local-in-c/) by Sandeep Shekhawat.
* [Using Span<T>](http://adamsitnik.com/Span/) by Adam Sitnik.
* [Using C# 7.1](http://blog.monstuff.com/archives/2017/07/using-Csharp-7.1.html) by Julien Couvreur.
* [Reviewing ResinPart I](https://ayende.com/blog/178945/reviewing-resin-part-i?key=457d59ed93ed44cebbb25ace8f71d561) by Ayende Rahien.
* [Reviewing ResinPart II](https://ayende.com/blog/178946/reviewing-resin-part-ii?Key=154eea77-6556-4292-94f5-0838c0174e89&utm_source=feedburner&utm_medium=feed&utm_campaign=Feed%3A+AyendeRahien+%28Ayende+%40+Rahien%29) by Ayende Rahien.
* [C# Developers: Stop Calling .Result](http://motzcod.es/post/162870742532/c-sharp-developers-stop-calling-dot-result) by James Montemagno.
* [Practical C# – Async Main in C# 7.1](http://www.andreaangella.com/2017/07/async-main/) by Andrea Angella.
* [All you need to know to master C# 7](http://www.andreaangella.com/2017/07/master-csharp7/) by Andrea Angella.
* [Practical C# – Generalized Async Return Types in C# 7](http://www.andreaangella.com/2017/07/practical-csharp-generalized-async-return-types/) by Andrea Angella.
* [Practical C# – Readonly Auto Properties](http://www.andreaangella.com/2017/07/practical-csharp-readonly-auto-properties/) by Andrea Angella.

## F#

* [Kami 2 Solver in F# - Part 2](http://completely-unique.com/posts/kami2-solver-part-2) by Chris Smith.
* [How does OO look in F#?](https://medium.com/@edgarsanchezg/f-is-a-first-functional-language-but-being-multi-paradigm-it-can-handle-classes-objects-fd9ede7a2cc9) by Edgar Sánchez.
* [From JavaScript to Functional Web Development (part 2)](https://medium.com/@iSetr/from-javascript-to-functional-web-development-part-2-33b2a9388935) by Sandor Szaloki.
* [Intro your website in WebSharper](https://medium.com/@iSetr/intro-your-website-in-websharper-296817af84e0) by Sandor Szaloki.
* [A gentle introduction to functional programming for web programmers using F#](https://blogs.msdn.microsoft.com/uk_faculty_connection/2017/07/12/a-gentle-introduction-to-functional-programming-for-web-programmers-using-f/) by Lee Stott.
* [Microsoft Reiterates its Support of F#](https://www.infoq.com/news/2017/07/microsoft-fsharp-build?utm_campaign=infoq_content&utm_source=twitter&utm_medium=feed&utm_term=dotnet) by InfoQ.
* [Introduction to F# with Nikhil Barthwal](https://vimeo.com/169456334) by Nikhil Barthwal via NYC F# meetup.

There is more content available this week in [F# Weekly](https://sergeytihon.wordpress.com/category/f-weekly/). If you want to see more F# awesomeness, please check it out!

## Xamarin

* [Visual Studio Tools for Tizen](http://gunnarpeipman.com/2017/07/tizen/) by Gunnar Peipman.
* [Xamarin forms Tabbed page – UWP with images](http://depblog.weblogs.us/2017/07/12/xamarin-forms-tabbed-page-uwp-with-images/) by Glenn Versweyveld.
* [The Xamarin Live Player Unpacked](http://developer.telerik.com/products/ui-for-xamarin/xamarin-live-player-unpacked/) by Sam Basu.
* [Generate pdf documents for iOS, Android & Windows UWP using Xamarin Forms and Syncfusion Essentials–Part 2](http://inquisitorjax.blogspot.com/2017/07/generate-pdf-documents-for-ios-android.html) by Malcolm Jack.
* [Mobile Cross Platform Image manipulation with Xamarin (Android / iOS / Windows UWP)](http://inquisitorjax.blogspot.com/2017/07/mobile-cross-platform-image.html) by Malcolm Jack.
* [Learning Xamarin.Forms – Part 2: MVVM](http://jesseliberty.com/2017/07/06/learning-xamarin-forms-part-2-mvvm/) by Jesse Liberty.
* [Bind to Xamarin Picker but only update value after hitting Done on iOS](http://blog.pieeatingninjas.be/2017/07/06/bind-to-xamarin-picker-but-only-update-value-after-hitting-done-on-ios/) by Pieter Nijs.
* [Mocking and Unit Testing the Xamarin.Forms Application class](http://brianlagunas.com/mocking-and-unit-testing-the-xamarin-forms-application-class/) by Brian Lagunas.
* [Xamarin.Basics – Ad Hoc iOS Builds, Part 2a: Publishing in HockeyApp](https://alexdunn.org/2017/07/13/xamarin-basics-ad-hoc-ios-builds-part-2a-publishing-in-hockeyapp/) by Alex Dunn.
* [Difference between Xamarin.Forms and Xamarin Traditional](https://almirvuk.blogspot.com/2017/07/difference-between-xamarinforms-and.html) by Almir Vuk.
* [Getting fancy with UIView anchors and state changes](http://xleon.net/xamarin/ios/nslayoutconstraint/autolayout/getting-fancy-with-uiview-anchors-and-state-changes.html) by Diego Ponce de León.
* [UIStackView magic](http://xleon.net/xamarin/ios/uistackview-magic.html) by Diego Ponce de León.
* [Beta Release: 15.3 Preview 4](https://releases.xamarin.com/beta-release-15-3-preview-4/) by Bri Brothers.
* [Step by step tracking down a macOS Beta regression](https://medium.com/@donblas/step-by-step-tracking-down-a-macos-beta-regression-39fe29b254de) by Chris Hamons.
* [Episode 26: Monetizing Mobile Apps with Ads](https://channel9.msdn.com/Shows/XamarinShow/Episode-26-Monetizing-Mobile-Apps-with-Ads) by The Xamarin Show.
* [Things I Think Are Cool: Behaviors Library](https://codemilltech.com/things-i-think-are-cool-behaviors-library/) by Matthew Soucoup.
* [XAML Markup Extensions](https://codemilltech.com/xaml-markup-extensions/) by Matthew Soucoup.
* [Mastering the Android Support Libraries](https://blog.xamarin.com/mastering-android-support-libraries/) by James Montemagno.
* [Xamarin Podcast: App Monetization, .NET Standard, Azure Cosmos DB, and more!](https://blog.xamarin.com/podcast-app-monetization-net-standard-azure-cosmos-db/) by Pierce Boggan.
* [Urban Refuge’s Refugee Aid Mobile Apps Turn Research into Action](https://blog.xamarin.com/urban-refuges-refugee-aid-mobile-apps-turn-research-action/) by Lacey Butler.
* [Prism in Xamarin Forms Step by Step (Part. 1)](https://xamgirl.com/prism-in-xamarin-forms-step-by-step-part-1/) by Charlin Agramonte.

## Azure

* [How to Store Secrets in Azure Key Vault Using .NET Core](https://www.humankode.com/asp-net-core/how-to-store-secrets-in-azure-key-vault-using-net-core) by humankode.
* [Monitoring the Nuget feed using Azure Functions](https://blogs.msdn.microsoft.com/brandonh/2017/07/17/monitoring-the-nuget-feed-using-azure-functions/) by Brandon H.
* [Azure Data Lake authentication options for .NET](https://azure.microsoft.com/en-us/resources/samples/data-lake-analytics-dotnet-auth-options/) by Matthew Hicks.
* [Media Services: Integrating Azure Media Services with Azure Functions and Logic Apps](https://azure.microsoft.com/en-us/resources/samples/media-services-dotnet-functions-integration/) by John Deutscher.
* [Service Fabric .NET Quickstart](https://azure.microsoft.com/en-us/resources/samples/service-fabric-dotnet-quickstart/) by Mikkel Hegnhoj.

## UWP

* [Windows 10 SDK Preview Build 16232 Released](https://blogs.windows.com/buildingapps/2017/07/12/windows-10-sdk-preview-build-16232-released/#eytmIyZ3atxZuMq8.97) by Clint Rutkas.
* [Announcing Babylon.js 3.0](https://blogs.windows.com/buildingapps/2017/07/12/announcing-babylon-js-3-0/#B86CuisPPwiV6bvx.97) by David Catuhe.
* [The UWP Community Toolkit is 1 1/2! (v1.5)](https://channel9.msdn.com/coding4fun/blog/The-UWP-Community-Toolkit-is-1-12-v15) by Channel 9.
* [Building a dynamic floating clickable menu for HoloLens/Windows MR](http://dotnetbyexample.blogspot.com/2017/07/building-dynamic-floating-clickable.html) by Joost van Schaik.

## Data

* [Reviewing Resin Part III](https://ayende.com/blog/178947/reviewing-resin-part-iii?Key=c73f964b-561b-4bfa-ab80-a72624d4e568) by Ayende Rahien.

## Game development

* [[Video] 12.7 Unity Tower defense tutorial - Buying upgrades](https://youtu.be/-Qvmvro9BmI) by inScope Studios.
* [[Video] Basic Level Design with Unity Terrain Generation](https://youtu.be/vdcdqEgV6-8) by Infallible Code.
* [Calling all game devs: The Dream.Build.Play 2017 Challenge is Here!](https://blogs.windows.com/buildingapps/2017/07/12/calling-game-devs-dream-build-play-2017-challenge/#1YS5DZdpOSUDru9Q.97) by Andrew Parsons.

And this is it for this week!

## Contribute to the week in .NET

As always, this weekly post couldn't exist without community contributions, and I'd like to thank all those who sent links and tips. The F# section is provided by [Phillip Carter](https://twitter.com/_cartermp), the gaming section by [Stacey Haffner](https://twitter.com/yecats131), the Xamarin section by [Dan Rigby](https://twitter.com/DanRigby), and the Azure and UWP section by [Michael Crump](http://twitter.com/mbcrump).

You can participate too. Did you write a great blog post, or just read one? Do you want everyone to know about an amazing new contribution or a useful library? Did you make or play a great game built on .NET?
We'd love to hear from you, and feature your contributions on future posts. Please [add your posts](https://weekindotnet.azurewebsites.net), it takes only a second.

We pick the articles based on the following criteria: the posts must be about .NET, they must have been published this week, and they must be original contents. Publication in Week in .NET is not an endorsement from Microsoft or the authors of this post.

This week's post (and future posts) also contains news I first read on [The ASP.NET Community Standup](https://blogs.msdn.microsoft.com/webdev/tag/communitystandup/), on [Weekly Xamarin](http://weeklyxamarin.com/), on [F# weekly](https://sergeytihon.wordpress.com/category/f-weekly/), and on [The Morning Brew](http://themorningbrew.net/).