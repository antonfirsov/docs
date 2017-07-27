---
title: The week in .NET - 
keywords: Week in .NET, community, .NET
weblogName: .NET Blog
dontInferFeaturedImage: true
---
Previous posts:

* [Command Line Parser Library, .NET South East](https://blogs.msdn.microsoft.com/dotnet/2017/07/18/the-week-in-net-command-line-parser-library-net-south-east/)
* [Links!](https://blogs.msdn.microsoft.com/dotnet/2017/07/11/the-week-in-net-links-2/)
* [Links!](https://blogs.msdn.microsoft.com/dotnet/2017/07/04/the-week-in-net-links/)

## Package of the week: MIST

The [INotifyPropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged) interface is essential to many applications such as Windows Forms or WPF data binding. [Implementing it](https://docs.microsoft.com/en-us/dotnet/framework/winforms/how-to-implement-the-inotifypropertychanged-interface) can however be fastidious, and involves quite a lot of boilerplate code. [MIST](https://github.com/mathtone/MIST) simplifies this using IL weaving and a custom Visual Studio build task. With MIST, you can use simple auto-properties and decorate them with attributes, and get an implementation of INotifyPropertyChanged.

```csharp
[Notifier]
public class ViewModel {
    //Raises the default notification ("WillNotify")
    [Notify]
    public string WillNotify { get; set; }

    [Notify]
    public string WillAlsoNotify { get; set; }

    //Notification will not be implemented
    public string WontNotify { get; set; }

    //Raises notification for the "DefinitelyNotAlias" property 
    [Notify("DefinitelyNotAlias")]
    public string AliasNotify { get; set; }

    //Raises multiple notification events
    [Notify("WillNotify","ComplexNotify")]
    public string ComplexNotify { get; set; }

    //Notification target, any method visible to the notifying property.
    //Can be implemented in a base class.
    [NotifyTarget]
    protected void PropertyChanged(string propertyName) {
        //up to you.
    }
}
```

* [MIST on GitHub](https://github.com/mathtone/MIST)
* [MIST on Nuget](https://www.nuget.org/packages/Mathtone.MIST/)

## User group meeting of the week: hacking away at 99 problems in F# in NYC

The 99 Problems Series is a programming challenge that challenges a developer to solve problems using lists, logic, trees, and graphs.  Fellow F#er Cesar Mendoza has taken the time to code and solve most of these problems for us. They are available [here](http://www.fssnip.net/tags/ninety-nine+f%23+problems).

Bring your laptops and sense of camaraderie, because you're going to spend some of the time working through some of the problems as individuals, and some of the problems as a group.

[Hacking away at 99 problems](https://www.meetup.com/nyc-fsharp/events/241875956/) is hosted by the [New York City F# User Group](https://www.meetup.com/nyc-fsharp/) on Wednesday, July 26 at 6:30PM in New York, NY.

## .NET

* [.NET Framework July 2017 Preview of Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2017/07/24/net-framework-july-2017-preview-of-quality-rollup/) by Rich Lander.
* [Profile-guided optimization in .NET Core 2.0](https://blogs.msdn.microsoft.com/dotnet/2017/07/20/profile-guided-optimization-in-net-core-2-0/) by Daniel Podder and Bertrand Le Roy.
* [.Net Core 2.0 to extend coding optimizations to Linux](https://www.techworld.com.au/article/625141/net-core-2-0-extend-coding-optimizations-linux/) by Paul Krill.
* ["Good explanation, but now what?" - Thoughts on getting people started with .NET](https://www.arthurrump.com/2017/07/23/thoughts-on-getting-started-with-net/) by Arthur Rump.
* [How to use AppVeyor Build Cache](https://www.gep13.co.uk/blog/how-to-use-appveyor-cache) by Gary Ewan Park.
* [How to use AppVeyor Remote Desktop Connection](https://www.gep13.co.uk/blog/how-to-use-appveyor-remote-desktop-connection) by Gary Ewan Park.
* [Docker adventures - Small story about obtaining Microsoft SQL Server for Linux on my PC](https://medium.com/@dmitriy.litichevskiy/docker-adventures-1f1196166ac0) by Dmitriy Litichevskiy.
* [Running a .NET Core 2 app on Raspbian Jessie, and deploying to the Pi with Cake](https://jeremylindsayni.wordpress.com/2017/07/23/running-a-net-core-2-app-on-raspbian-jessie-and-deploying-to-the-pi-with-cake/) by Jeremy Lindsay.
* [Advanced MSBuild Extensibility](https://channel9.msdn.com/Shows/Code-Conversations/Advanced-MSBuild-Extensibility-with-Nate-McMaster) by Nate McMaster.
* [How to create a Cake Addin](https://codeopinion.com/how-to-create-cake-addin/) by Derek Comartin.
* [Configuring TeamCity to run in Docker on Linux and build .NET Core projects](http://lunarfrog.com/blog/configuring-teamcity-docker-linux-dotnetcore-builds) by Andrei Marukovich.
* [Dealing with Horrid, No-Good, Very-Bad APIs Using JSON.NET](http://trycatchfail.com/blog/post/Dealing-with-Horrid-No-Good-Very-Bad-APIs-Using-JSONNET) by Matt Honeycutt .
* [NuGet.org Gets a Facelift](http://blog.nuget.org/20170718/NuGet-Gallery-Gets-A-Facelift.html) by Jon Chu.
* [What is .NET Core?](http://cynicaldeveloper.com/blog/what-is-net-core/) by James Studdart.
* [How To Debug A .NET Core Nuget Package?](http://geeklearning.io/how-to-debug-a-net-core-nuget-package/) by Arnaud.
* [Setting up Raspian and .NET Core 2.0 on a Raspberry Pi](https://blogs.msdn.microsoft.com/david/2017/07/20/setting_up_raspian_and_dotnet_core_2_0_on_a_raspberry_pi/) by Dave The Engineer.
* [Extending MSTest V2](https://blogs.msdn.microsoft.com/devops/2017/07/18/extending-mstest-v2/) by Pratap Lakshman.
* [How can I find out how many threads are active in the CLR thread pool?](https://blogs.msdn.microsoft.com/oldnewthing/20170724-00/?p=96675) by Raymond Chen.

## ASP.NET

* [Building ASP.NET Core 2.0 preview 2 packages on AppVeyor](https://andrewlock.net/building-asp-net-core-2-0-preview-2-packages-on-appveyor/) by Andrew Lock.
* [Creating a validator to check for common passwords in ASP.NET Core Identity](https://andrewlock.net/creating-a-validator-to-check-for-common-passwords-in-asp-net-core-identity/) by Andrew Lock.
* [Customize Razor Pages Handlers](http://hishambinateya.com/customize-razor-pages-handlers) by Hisham Bin Ateya.
* [Environment Variables and Configuration in ASP.NET Core Apps](http://michaco.net/blog/EnvironmentVariablesAndConfigurationInASPNETCoreApps) by Michael Conrad.
* [ASP.NET Core MVC - Common Components/(Partial)Views across applications](http://www.ryansouthgate.com/2017/07/21/asp-net-core-mvc-common-components-view-across-applications/) by Ryan Southgate.
* [ASP.NET Core Razor Pages – Introduction](https://codingblast.com/asp-net-core-razor-pages/) by Ibrahim Šuta.
* [.NET Core Middleware – OWASP Headers Part 1](https://dotnetcore.gaprogman.com/2017/07/20/net-core-middleware-owasp-headers-part-1/) by Jamie Taylor.
* [How to Implement OPTIONS Response in ASP.NET Web API 2](https://www.codeproject.com/Articles/1197647/How-to-Implement-OPTIONS-Response-in-ASP-NET-Web-A) by Andrey Rodin.
* [Exploring the Environment Tag Helper exclude and include attributes in ASP.NET Core 2](https://scottsauber.com/2017/07/19/environment-taghelper-exclude-and-include-attributes-in-asp-net-core-2/) by Scott Sauber.
* [Advanced ASP.NET Trace Viewer – WebForms, MVC, Web API, WCF](https://stackify.com/asp-net-trace-viewer/) by Matt Watson.
* [Implementing IHostedService in ASP.NET Core 2.0Use IHostedService to run background tasks in ASP.NET Core apps](https://www.stevejgordon.co.uk/asp-net-core-2-ihostedservice) by Steve Gordon.
* [Customizing query string parameter binding in ASP.NET Core MVC](https://www.strathweb.com/2017/07/customizing-query-string-parameter-binding-in-asp-net-core-mvc/) by Filip W.
* [WebSocket per-message compression in ASP.NET Core](https://www.tpeczek.com/2017/07/websocket-per-message-compression-in.html) by Tomasz Pęczek.

## C#

* [Visual Studio Toolbox: Design Patterns: Template Method](https://blogs.msdn.microsoft.com/robertgreen/2017/07/20/visual-studio-toolbox-design-patterns-template-method/) by Robert Green.
* [Why does the assignment operator in C# evaluate left to right instead of right to left?](https://blogs.msdn.microsoft.com/oldnewthing/20170718-00/?p=96635) by Raymond Chen.
* [Crash course in async and await](https://blogs.msdn.microsoft.com/oldnewthing/20170720-00/?p=96655) by Raymond Chen.
* [Expression – Bodied Members in C# 7.0](http://dailydotnettips.com/2017/07/17/expression-bodied-members-in-c-7-0/) by Abhijit Jana.
* [Practical C# – Default Expressions in C# 7.1](http://www.andreaangella.com/2017/07/practical-csharp-default-expressions/) by Andrea Angella.
* [Local Functions – What’s the Value?](https://csharp.christiannagel.com/2017/07/25/localfunctions/) by Christian Nagel.
* [C# 6.0 draft Language Specification](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/) by BillWagner.
* [SOLID – Single Responsibility Principle With C#](https://l-paathshaala.com/2017/07/24/solid-single-responsibility-principle-with-c/) by Sandeep Shekhawat.

## F#

* [Get Started with F# as a C# developer](https://blogs.msdn.microsoft.com/dotnet/2017/07/24/get-started-with-f-as-a-c-developer/) by Phillip Carter.
* [Accessing a relational DB with F# type providers](https://medium.com/@edgarsanchezg/accessing-a-relational-db-with-f-type-providers-7263d88aa640) by Edgar Sánchez.
* [Start your graph theory website with WebSharper](https://medium.com/@iSetr/start-your-graph-theory-website-with-websharper-4c0c1b425023) by Sandor Szaloki.
* [Beerway Oriented Programming in F# ](https://medium.com/@mukund.sharma92/beerway-oriented-programming-in-f-1-7f71c2a0dcf6) by Moko Sharma.
* [Creating Visual Planetary Systems using Fable and F#](https://medium.com/@mukund.sharma92/creating-visual-planetary-systems-using-fable-and-f-de23415ca6f7) by Moko Sharma.
* [CRUDing in F# with MongoDb](https://medium.com/@mukund.sharma92/cruding-in-f-with-mongodb-e4699d1ac17e) by Moko Sharma.
* [Epic Mahabharata](https://medium.com/fuzzycloud/epic-adventure-using-f-17ac32fb539d) by Kunjan Dalal.
* [When to use a Discriminated Union vs Record Type in F#](http://www.mindbodysouldeveloper.com/2017/07/17/when-to-use-a-discriminated-union-vs-record-type-in-f-sharp/?utm_content=buffere4d09&utm_medium=social&utm_source=twitter.com&utm_campaign=buffer) by Jose Gonzalez.
* [F#, Fable and Ionide hacking - adding profiler to VSCode](https://www.youtube.com/watch?v=K-13gtMevxc) by Krzysztof Cieślak.

There is more content available this week in [F# Weekly](https://sergeytihon.wordpress.com/category/f-weekly/). If you want to see more F# awesomeness, please check it out!

## Xamarin

* [Xamarin – Build on iOS from Windows Command Prompt/MSBuild](https://martynnw.wordpress.com/2017/07/13/xamarin-build-on-ios-from-windows-command-promptmsbuild/) by Martyn Wiggins.
* [Fixing a Quirky "Xamarin.Forms ListView Bug" That Led Me Back to Basics And How Most Bugs Are Written By Users!](http://blog.mjjames.co.uk/2017/07/fixing-quirky-xamarinforms-listview-bug.html) by Michael James.
* [Snack Pack 16: Customizing Visual Studio for Mac](https://channel9.msdn.com/Shows/XamarinShow/Snack-Pack-16-Customizing-Visual-Studio-for-Mac) by The Xamarin Show.
* [The Xamarin.Forms Messaging Center Messed With Me](https://codemilltech.com/messing-with-xamarin-forms-messaging-center/) by Matthew Soucoup.
* [Xamarin Forms, the MVVMLight Toolkit and I: EventToCommandBehavior](https://msicc.net/xamarin-forms-the-mvvmlight-toolkit-and-i-eventtocommandbehavior/) by Marco Siccardi.
* [Using MvvmCross with Xamarin.Forms — Part 1](https://medium.com/@martijn00/using-mvvmcross-with-xamarin-forms-part-1-eaee5815bb8c) by Martijn van Dijk.
* [InTheHand.Forms Updates](https://peterfoot.net/2017/07/18/inthehand-forms-updates/) by Peter Freeman Foot.
* [Xamarin Preview: Xcode 9 beta 3, iOS 11, macOS 10.13 support – Preview 1](https://releases.xamarin.com/preview-xcode-9-beta-3-ios-11-macos-10-13-support-preview-1/) by Bri Brothers.
* [ReactiveUI Tutorial for Xamarin: The ViewModel](https://www.devprotocol.com/reactiveui-tutorial-for-xamarin-the-viewmodel/) by Jan Tourlamain.
* [Gorilla Player is free](https://blog.uxdivers.com/2017/07/17/gorilla-player-is-free/) by UXDivers.
* [Guitar Center’s Five-Star Xamarin.Forms Apps Bring Music to the Masses](https://blog.xamarin.com/guitar-centers-five-star-xamarin-forms-apps-bring-music-masses/) by Lacey Butler.
* [Introducing MFractor for Visual Studio for Mac](https://blog.xamarin.com/introducing-mfractor-visual-studio-mac/) by Matthew Robbins.
* [Dynamically Changing Xamarin.Forms Tab Icons When Selected](http://motzcod.es/post/162985782667/dynamically-changing-xamarin-forms-tab-icons-when-select) by James Montemagno.
* [Xamarin.Control – Xamarin.Forms MaterialEntry](https://alexdunn.org/2017/07/14/xamarin-control-xamarin-forms-materialentry/) by Alex Dunn.
* [Styles in Xamarin Forms don't work properly in UWP .NET Native - here is how to fix it](http://dotnetbyexample.blogspot.com/2017/07/styles-in-xamarin-forms-dont-work.html) by Joost van Schaik.
* [Progress indicator with Xamarin Forms](http://gunnarpeipman.com/2017/07/xamarin-forms-progress-indicator/) by Gunnar Peipman.
* [Learning Xamarin.Forms – Part 3: Navigation](http://jesseliberty.com/2017/07/14/learning-xamarin-forms-part-3-navigation/) by Jesse Liberty.
* [Learning Xamarin.Forms – Part 4: Layout and Views](http://jesseliberty.com/2017/07/19/learning-xamarin-forms-part-4-layout-and-views/) by Jesse Liberty.
* [Exrin 2.0.0 Quick Start](https://xamarinhelp.com/exrin-2-0-0-quick-start/) by Adam Pedley.
* [Exrin Inspector Preview](https://xamarinhelp.com/exrin-inspector/) by Adam Pedley.
* [Pushing the Boundaries of Xamarin with Syncfusion](https://www.syncfusion.com/blogs/post/guest-blog-pushing-the-boundaries-of-xamarin-with-syncfusion.aspx) by James Kennedy.
* [MFractor 3.1 for Visual Studio Mac out now ](https://www.mfractor.com/blogs/news/introducing-mfractor-3-1-for-visual-studio-mac) by Matthew Robbins.
* [Crash Reporting and Analytics for Xamarin](https://www.thewissen.io/crash-reporting-analytics-xamarin/) by Steven Thewissen.

## Azure

* [Calling Azure Cosmos DB Graph API from Azure Functions](https://blogs.msdn.microsoft.com/srikantan/2017/07/21/calling-azure-cosmos-db-graph-api-from-azure-functions/) by Srikantan Sankaran.
* [Opensource DevOps for C# Azure Functions](https://aboersch.com/2017/07/24/opensource-devops-for-c-azure-functions/) by Alexander Boersch.
* [Triggering an Azure Automation Runbook from anywhere](https://aboersch.com/2017/07/24/triggering-azure-automation-runbooks-from-anywhere/) by Alexander Boersch.
* [Faking Azure ID identity in ASP.NET Core unit tests](http://gunnarpeipman.com/2017/07/aspnet-core-azure-ad-unit-test/) by Gunnar Peipman.
* [Integrate Azure AD into a web application using OpenID Connect](https://azure.microsoft.com/en-us/resources/samples/active-directory-dotnet-webapp-openidconnect/) by Danny Strockis.
* [ASP.NET and SQL Database sample for Azure App Service](https://azure.microsoft.com/en-us/resources/samples/dotnet-sqldb-tutorial/) by Cephas Lin.
* [Getting Started with Azure Search using .NET](https://azure.microsoft.com/en-us/resources/samples/search-dotnet-getting-started/) by Azure Samples.
* [A lap around Azure Functions, go serverless!](https://blog.steef-jan-wiggers.com/2017/07/lap-around-azure-functions-go-serverless/) by Steef-Jan Wiggers.

## UWP

* [Deploy a UWP application to a Windows 10 device from the command line with Cake](https://jeremylindsayni.wordpress.com/2017/07/24/deploy-a-uwp-application-to-a-windows-10-device-from-the-command-line-with-cake/) by Jeremy Lindsay.
* [Working with Brushes and Content – XAML and Visual Layer Interop, Part One](https://blogs.windows.com/buildingapps/2017/07/18/working-brushes-content-xaml-visual-layer-interop-part-one/) by Windows UI Team.
* [New Lights and PropertySet Interop – XAML and Visual Layer Interop, Part Two](https://blogs.windows.com/buildingapps/2017/07/19/new-lights-propertyset-interop-xaml-visual-layer-interop-part-two/#PcIm2HwaJQw2ymxx.97) by Windows UI Team.
* [Windows Template Studio 1.2 released!](https://blogs.windows.com/buildingapps/2017/07/21/windows-template-studio-1-2-released/#ZYat4sfOILxYGQh6.97) by Clint Rutkas.

And this is it for this week!

## Contribute to the week in .NET

As always, this weekly post couldn't exist without community contributions, and I'd like to thank all those who sent links and tips. The F# section is provided by [Phillip Carter](https://twitter.com/_cartermp), the gaming section by [Stacey Haffner](https://twitter.com/yecats131), the Xamarin section by [Dan Rigby](https://twitter.com/DanRigby), and the Azure and UWP section by [Michael Crump](http://twitter.com/mbcrump).

You can participate too. Did you write a great blog post, or just read one? Do you want everyone to know about an amazing new contribution or a useful library? Did you make or play a great game built on .NET?
We'd love to hear from you, and feature your contributions on future posts. Please [add your posts](https://weekindotnet.azurewebsites.net), it takes only a second.

We pick the articles based on the following criteria: the posts must be about .NET, they must have been published this week, and they must be original contents. Publication in Week in .NET is not an endorsement from Microsoft or the authors of this post.

This week's post (and future posts) also contains news I first read on [The ASP.NET Community Standup](https://blogs.msdn.microsoft.com/webdev/tag/communitystandup/), on [Weekly Xamarin](http://weeklyxamarin.com/), on [F# weekly](https://sergeytihon.wordpress.com/category/f-weekly/), and on [The Morning Brew](http://themorningbrew.net/).