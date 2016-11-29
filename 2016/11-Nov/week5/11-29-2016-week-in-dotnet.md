The week in .NET - Cosmos on On.NET, GongSolutions.WPF.DragDrop
===============================================================

To read last week's post, see [The week in .NET – .NET Core, ASP.NET Core, EF Core 1.1 – Docker – Xenko](https://blogs.msdn.microsoft.com/dotnet/2016/11/22/the-week-in-net-net-core-asp-net-core-ef-core-1-1-docker-xenko/).

On .NET
-------

Last week, Chad Z. Hower, a.k.a. Kudzu [was on the show](https://channel9.msdn.com/Shows/On-NET/Chad-Z-Hower-aka-Kudzu-Cosmos) to talk about Cosmos, a C# open source managed operating system:

<iframe src="https://channel9.msdn.com/Shows/On-NET/Chad-Z-Hower-aka-Kudzu-Cosmos/player" width="960" height="540" allowFullScreen frameBorder="0"></iframe>

This week, we'll speak with Xavier Decoster and Maarten Balliauw about [MyGet](http://myget.org/). The show is on Wednesday this week and begins at 10AM Pacific Time [on YouTube](https://www.youtube.com/watch?v=WOSngcgTikg). We'll take questions on the video's integrated chat.

Package of the week: GongSolutions.WPF.DragDrop
-----------------------------------------------

The [GongSolutions.WPF.DragDrop library](https://github.com/punker76/gong-wpf-dragdrop) is an easy to use drag & drop framework for WPF. It supports MVVM, multi-selection, and visual feedback adorners.

![WPF.DragDrop](https://github.com/punker76/gong-wpf-dragdrop/raw/dev/screenshots/DragDropSample01.gif)

```xml
<ListBox ItemsSource="{Binding Collection}"
         dd:DragDrop.IsDragSource="True"
         dd:DragDrop.IsDropTarget="True"
         dd:DragDrop.DropHandler="{Binding}" />
```

```csharp
class ExampleViewModel : IDropTarget
{
    public ObservableCollection<ExampleItemViewModel> Items;

    void IDropTarget.DragOver(IDropInfo dropInfo) {
        ExampleItemViewModel sourceItem = dropInfo.Data as ExampleItemViewModel;
        ExampleItemViewModel targetItem = dropInfo.TargetItem as ExampleItemViewModel;

        if (sourceItem != null && targetItem != null && targetItem.CanAcceptChildren) {
            dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
            dropInfo.Effects = DragDropEffects.Copy;
        }
    }

    void IDropTarget.Drop(IDropInfo dropInfo) {
        ExampleItemViewModel sourceItem = dropInfo.Data as ExampleItemViewModel;
        ExampleItemViewModel targetItem = dropInfo.TargetItem as ExampleItemViewModel;
        targetItem.Children.Add(sourceItem);
    }
}
```

Game of the week: xx
-----------------------------------


User group meeting of the week: Electrical Engineering for Programmers in NYC
-----------------------------------------------------------------------------

Tonight, Tuesday November 29 at 6:00PM at the Microsoft Reactor in NYC, the [Microsoft Makers and App Developers group](https://www.meetup.com/MMADNYC/) hold [a meeting on Electrical Engineering for programmers](https://www.meetup.com/MMADNYC/events/235331431/).

.NET
----

* [.NET Standard 2.0 - Making Sense of .NET Again](https://weblog.west-wind.com/posts/2016/Nov/23/NET-Standard-20-Making-Sense-of-NET-Again) by Rick Strahl.
* [Open Source .NET – 2 years later](http://mattwarren.org/2016/11/23/open-source-net-2-years-later/) by Matt Warren.
* [R.I.P project.json – Out with the new, in with the old](https://www.stevejgordon.co.uk/project-json-replaced-by-csproj) by Steve Gordon.
* [MVP Hackathon 2016: Cool Projects from Microsoft MVPs](https://blogs.msdn.microsoft.com/webdev/2016/11/22/mvp-hackathon-2016/) by Jeffrey T. Fritz.
* [Selenium with .NET Core](http://www.dotnetcatch.com/2016/11/23/selenium-with-net-core/) by Robb Schiefer.
* [Running a .NET User Group](http://codeopinion.com/running-a-net-user-group/) by Derek Comartin.
* [Problems with AsParallel](http://indexoutofrange.com/Problems_with_AsParallel/) by Szymon Warda.
* [Digging into the CoreCLR - Some bashing on the cost of hashing](https://ayende.com/blog/176161/digging-into-the-coreclr-some-bashing-on-the-cost-of-hashing) by Federico.
* Making code faster: [Streamlining the output](https://ayende.com/blog/176097/making-code-faster-streamlining-the-output), [That pesky dictionary](https://ayende.com/blog/176098/making-code-faster-that-pesky-dictionary), and [Micro optimizations and parallel work](https://ayende.com/blog/176129/making-code-faster-micro-optimizations-and-parallel-work) by Ayende Rahien.
* [The journey continues to Secure Pipelines, via OpenSsl](https://cetus.io/tim/Part-3-Pipelines-OpenSsl/) by Tim Seaward.
* [Enforcing Immutability in Multi-Threaded Projects with NDepend](http://www.daedtech.com/enforcing-mutability-multi-threaded-projects-ndepend/) by Erik Dietrich.
* [ALT.NET talks in Paris](http://www.maherjendoubi.io/alt-net-talks-2016-11/) by Maher Jendoubi.
* [MailBody, a library for generating email using a fluent interface](https://github.com/doxakis/MailBody) by Philip Doxakis.
* [Using ETW tracing on Windows 10 IoT Core](http://gunnarpeipman.com/2016/11/iot-etw-trace/) by Gunnar Peipman.

ASP.NET
-------

* [Custom ModelBinding in ASP.NET MVC Core](https://www.stevejgordon.co.uk/html-encode-string-aspnet-core-model-binding) by Steve Gordon.
* [Exploring Middleware as MVC Filters in ASP.NET Core 1.1](https://andrewlock.net/exploring-middleware-as-mvc-filters-in-asp-net-core-1-1/) by Andrew Lock.
* [The ASP.NET Web API 2 HTTP Message Lifecycle in 43 Easy Steps](https://www.exceptionnotfound.net/the-asp-net-web-api-2-http-message-lifecycle-in-43-easy-steps-2/) by Matthew Jones.
* [A simple full stack application with .NET Core and Angular JS](http://www.c17e.com/index.php/2016/11/25/a-simple-full-stack-application-with-net-core-and-angular-js/) by Pierre Murasso.
* [ASP.NET Web API - Keeping It Simple](https://www.codeproject.com/Articles/1157685/ASP-NET-Web-API-Keeping-It-Simple) by Kannankeril.
* [ASP.NET Core and the Enterprise Part 3: Middleware](http://odetocode.com/blogs/scott/archive/2016/11/22/asp-net-core-and-the-enterprise-part-3-middleware.aspx) by K. Scott Allen.
* [ASP.NET Core: Using third-party DI/IoC containers](http://gunnarpeipman.com/2016/11/aspnet-core-structuremap-autofac/) by Gunnar Peipman.
* [Creating a New View Engine in ASP.NET Core](http://aspnetmonsters.com/2016/11/2016-11-22-creating-a-new-view-engine-in-asp-net-core/) by David Paquette.
* [ASP.NET Core: compile once, host everywhere](https://www.codeproject.com/articles/1117251/asp-net-core-compile-once-host-everywhere) by Kornfeld Eliyahu Peter.

F#
--

Xamarin
-------

* [Introducing Workbooks & Inspector](https://blog.xamarin.com/introducing-workbooks-inspector/) by Aaron Bockover.
* [Xamarin Workbooks - The (Interactive) Future of Technical Docs](https://msdn.microsoft.com/magazine/mt790187) by Craig Dunn.
* [Visual Studio Development – Introducing Visual Studio for Mac](https://msdn.microsoft.com/magazine/mt790182) by Mikayla Hutchinson.
* [Mobile DevOps - Exploring Visual Studio Mobile Center](https://msdn.microsoft.com/magazine/mt790198) by Thomas Dohmke.
* [Live XAML Previewing with the Xamarin.Forms Previewer](https://blog.xamarin.com/live-xaml-previewing-with-the-xamarin-forms-previewer/) by Nina Vyedin.
* [Introducing Xamarin.Forms 2.3.3: Native View Declaration and Platform Specifics](https://blog.xamarin.com/introducing-xamarin-forms-2-3-3-native-view-declaration-and-platform-specifics/) by Pierce Boggan.
* [Mobile - Embedding Native Views in Your Xamarin.Forms Apps](https://msdn.microsoft.com/magazine/mt790186) by Charles Petzold.
* [Bindable Native Views – Xamarin.Forms 2.3.3 Look Ahead](https://codemilltech.com/bindable-native-views-xamarin-forms-2-3-3-look-ahead/) by Matthew Soucoup.
* [Xamarin.Forms and .NET Core are the future for Tizen and a great new opportunity for .NET developers](http://www.maherjendoubi.io/xamarin-forms-and-net-core-are-the-future-for-tizen-and-a-great-new-opportunity-for-net-developers-2/) by Maher Jendoubi.
* [Resource Files in Xamarin Forms](https://xamarinhelp.com/resource-files/) by Adam Pedley.
* [Hacking the Xamarin.Forms Layout System for Fun and Profit](http://www.michaelridland.com/xamarin/hacking-the-xamarin-forms-layout-system-for-fun-and-profit/) by Michael Ridland.
* [Xamarin and the Universal Windows Platform](https://msdn.microsoft.com/magazine/mt790185) by Tyler Whitney.
* [Scale Your Automated Mobile App Testing with Xamarin Test Cloud](https://msdn.microsoft.com/magazine/mt790199) by Justin Raczak.
* [Building Mobile UI Tests using REPL](http://blog.falafel.com/building-mobile-ui-tests-using-repl/) & [Troubleshooting Xamarin UI Tests](http://blog.falafel.com/troubleshooting-xamarin-ui-tests/) by Noel Rice.
* [Building Flexible and Efficient Xamarin Apps with GraphQL](http://gregshackles.com/building-flexible-xamarin-apps-with-graphql/) by Greg Shackles.
* [Visual Studio - Unable to launch Google Android Emulators](http://motzcod.es/post/153357758562/visual-studio-unable-to-launch-google-android) by James Montemagno.
* [Xamarin Studio Add-in: Sort and Remove unused using directives on file save](https://github.com/alexsorokoletov/XamarinStudio.SortRemoveUsings) by Alex Sorokoletov.

Azure
-----

* [Publishing ASP.NET Core 1.1 applications to Azure using git deploy](http://www.hanselman.com/blog/PublishingASPNETCore11ApplicationsToAzureUsingGitDeploy.aspx) by Scott Hanselman.
* [CSV file to API using Azure Functions](http://blogs.lessthandot.com/index.php/enterprisedev/cloud/azure/csv-file-to-api-using-azure-functions-csvaas/) by Eli Weinstock-Herman.
* [Transition your Mobile Services to App Service (video)](https://channel9.msdn.com/Shows/Azure-Friday/Transition-your-Mobile-Services-to-App-Service) by Adrian Hall and Seth Juarez.

And more Azure links on [Azure Weekly](https://buildazure.com/2016/11/21/azure-weekly-nov-21-2016/), by Chris Pietschmann.

Data
----

* [LLBLGen Pro v5.1 RTM has been released!](https://weblogs.asp.net/fbouma/llblgen-pro-v5-1-rtm-has-been-released) by Frans Bouma.
* [EF6 or EF Core? How Do I Choose? (slide deck)](http://thedatafarm.com/data-access/ef6-or-ef-core-how-do-i-choose/) by Julie Lerman.
* [Entity Framework Core and Cross-Database Support](https://blogs.msdn.microsoft.com/mvpawardprogram/2016/11/22/entity-framework-core/) by Shay Rojansky.
* [Entity Framework Core – Unit Testing](https://csharp.christiannagel.com/2016/11/22/efcoreunittesting/) by Christian Nagel.
* [Integration Testing with Entity Framework Core and SQL Server](http://www.davepaquette.com/archive/2016/11/27/integration-testing-with-entity-framework-core-and-sql-server.aspx) by Dave Paquette.

Games
-----

And this is it for this week!

Contribute to the week in .NET
------------------------------

As always, this weekly post couldn't exist without community contributions, and I'd like to thank all those who sent links and tips. The F# section is provided by [Phillip Carter](https://twitter.com/_cartermp), the gaming section by [Stacey Haffner](https://twitter.com/yecats131), and the Xamarin section by [Dan Rigby](https://twitter.com/DanRigby).

You can participate too. Did you write a great blog post, or just read one? Do you want everyone to know about an amazing new contribution or a useful library? Did you make or play a great game built on .NET?
We'd love to hear from you, and feature your contributions on future posts:

* Send an email to beleroy at Microsoft,
* [comment on this gist](xx)
* Leave us a pointer in the comments section below.
* [Send Stacey (@yecats131) tips on Twitter about .NET games](https://twitter.com/yecats131).

This week's post (and future posts) also contains news I first read on [The ASP.NET Community Standup](https://blogs.msdn.microsoft.com/webdev/tag/communitystandup/), on [Weekly Xamarin](http://weeklyxamarin.com/), on [F# weekly](https://sergeytihon.wordpress.com/category/f-weekly/), and on [Chris Alcock's The Morning Brew](http://themorningbrew.net/).
