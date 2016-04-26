The week in .NET - 4/26/2016
============================

To read last week's post, see [The week in .NET – 4/19/2016](https://blogs.msdn.microsoft.com/dotnet/2016/04/19/the-week-in-net-4192016/).

On.NET
------

Last week on the show, [we had Burke Holland and Sam Basu](https://www.youtube.com/watch?v=7E7JyvBIGKs) from [Telerik](http://www.telerik.com/). This week, [we'll speak with Benjamin Fistein and Jakub Míšek](https://www.youtube.com/watch?v=ZjN9kREzPMs) about [Peachpie](http://www.peachpie.io/), a PHP compiler built with Roslyn to target .NET.

Package of the week: Live-Charts for WinForms and WPF
-----------------------------------------------------

[Live-Charts](https://github.com/beto-rodriguez/Live-Charts) is a fun charting library that is fast enough that it can upate charts automatically from user actions and data changes. It's written entirely in C#, is under the MIT license, and works with WinForms and WPF. [The documentation](http://lvcharts.net/#/examples/v1/intro?path=intro) is very detailed, and contains useful animated examples.

![A basic line chart](http://lvcharts.net/App/Examples/Images/basicline.gif)

```csharp
Series = new SeriesCollection();

//create some LineSeries
var charlesSeries = new LineSeries
{
    Title = "Charles",
    Values = new ChartValues<double> {10, 5, 7, 5, 7, 8}
};
var jamesSeries = new LineSeries
{
    Title = "James",
    Values = new ChartValues<double> { 5, 6, 9, 10, 11, 9 }
};

//add series to SeriesCollection
Series.Add(charlesSeries);
Series.Add(jamesSeries);
```

```xaml
<lvc:LineChart LegendLocation="Right" Series="{Binding Series}" >
  <lvc:LineChart.AxisY>
    <lvc:Axis Title="Sold Items" />
  </lvc:LineChart.AxisY>
  <lvc:LineChart.AxisX>
    <lvc:Axis Title="Month"
              Labels="Jan, Feb , Mar, Apr, May, Jun" />
  </lvc:LineChart.AxisX>
</lvc:LineChart>
```

Control of the week: RadGridView for WPF
----------------------------------------

[Telerik's RadGridView for WPF](http://www.telerik.com/products/wpf/gridview.aspx) is a very complete grid control with built-in UI virtualization and LINQ querying. It also supports grouping, validation, merged cells, and custom templates.

![RadGridView](http://d585tldpucybw.cloudfront.net/sfimages/default-source/productsimages/WPF/Controls/GridView/1_wpf_complete-datagrid-experience66723ba9cdf64e46b0c703efa2278a47.png?sfvrsn=3)

Xamarin App of the week: Crédito Agrícola
-----------------------------------------

[Crédito Agrícola](http://www.creditoagricola.pt/CAI) is one of the largest banks in Portugal with 1.2 million customers. To create a native enterprise banking app, they turned to Xamarin after costly attempts in platform-specific languages. Now Crédito Agrícola provides better service for its most important customers through secure mobile payment authorizations and expense approvals.

![Crédito Agrícola](http://www.creditoagricola.pt/NR/rdonlyres/C6B39366-961D-431E-9350-6109CE27CFE9/0/consulta04.jpg?1461621035769)

User group meeting of the week: Designing Distributed Applications at Outbox
----------------------------------------------------------------------------

[Malisa Ncube](http://malisancube.com/speaking-at-geeknight-at-outbox/) will be [speaking about distributed applications on Tuesday, April 26 at 6:00PM](http://outbox.co.ug/events/geeknight-april-2016-designing-distributed-applications) at the Soliz House, Plot 23 Lumumba Ave, Kampala, Uganda.

.NET
----

* [.NET Goodness at BUILD 2016 – .NET ALL THE THINGS!](https://blogs.msdn.microsoft.com/bethmassi/2016/04/15/net-goodness-at-build-2016-net-all-the-things/) by Beth Massi.
* [Introducing the Microsoft .NET Framework Repair Tool Version 1.3](https://blogs.msdn.microsoft.com/dotnet/2016/04/19/introducing-the-microsoft-net-framework-repair-tool-version-1-3/) by Stacey Haffner.
* [An anthology of .NET's little wonders and pitfalls](http://geekswithblogs.net/BlackRabbitCoder/archive/2016/04/19/c.net-little-wonders-anthology.aspx), by James Michael Hare.
* [JetBrains joins the .NET Foundation](http://blog.jetbrains.com/dotnet/2016/04/18/jetbrains-joins-the-net-foundation/) by Hadi Hariri.
* [Edit and continue part 1](https://joshvarty.wordpress.com/2016/04/18/edit-and-continue-part-1-introduction/), and [part 2](https://joshvarty.wordpress.com/2016/04/21/edit-and-continue-part-2-roslyn/) by Josh Varty.
* [Test driving C# 7 features in Visual Studio “15” Preview](http://www.thomaslevesque.com/2016/04/16/test-driving-c-7-features-in-visual-studio-15-preview/) by Thomas Levesque.
* [CodeRush for Roslyn 1.0.11 (preview) is available](https://community.devexpress.com/blogs/markmiller/archive/2016/04/19/coderush-for-roslyn-1-0-11-preview-is-available.aspx) by Mark Miller.
* [The design of RavenDB 4.0 over the wire protocol](https://ayende.com/blog/173890/the-design-of-ravendb-4-0-over-the-wire-protocol) by Ayende Rahien.
* [Bot Builder Dialogs](http://mayoster.blogspot.co.uk/2016/04/bot-builder-dialogs.html) by Joe Mayo.
* [Exploiting the ConcurrentDictionary in asynchronous applications](https://visualstudiomagazine.com/articles/2016/04/01/concurrentdictionary.aspx) by Peter Vogel.
* [Build a microservice with Service Fabric on Windows Server](http://www.codeproject.com/Articles/1094778/Build-a-Microservice-with-Service-Fabric-on-Window) by Shawn1Xu.
* [Static code analysis and more with MONO-CECIL](http://blog.goyello.com/2016/04/21/static-code-analysis-with-mono-cecil/) by Patryk Borowa.
* [Why Azure REST API-s and how to prepare for using them?](http://gunnarpeipman.com/2016/04/why-azure-rest-api-s-and-how-to-prepare-for-using-them/) by Gunnar Peipman.
* [Refactoring Essentials now with a Roslyn Code Converter](https://channel9.msdn.com/coding4fun/blog/Refactoring-Essentials-now-with-a-Roslyn-Code-Converter) by Greg Duncan.
* [Properly Throwing & Rethrowing Exceptions](https://dotnettips.wordpress.com/2016/04/15/ask-dotnetdave-properly-throwing-exceptions/) by David McCarter.

ASP.NET
-------

* [Notes from the ASP.NET Community Standup – April 19, 2016](https://blogs.msdn.microsoft.com/webdev/2016/04/21/notes-from-the-asp-net-community-standup-april-19-2016/) by Jeffrey T. Fritz.
* [WhereYouAt Demo from Build 2016: Demo Flow and UI (video)](https://blogs.msdn.microsoft.com/webdev/2016/04/07/whereyouat-demobuild-2016-demo-flow-ui/) by Maria Naggaga, Scott Hanselman, Steve Lasker, and Glenn Condron.
* [Cross-Domain Cookie with Legacy Applications](http://www.danylkoweb.com/Blog/cross-domain-cookie-with-legacy-applications-F8) by Jonathan Danylko.
* [Kestrel as a Static Server for Angular](http://www.tattoocoder.com/kestrel-as-a-static-server-for-angular/) by Shayne Boyer.
* [Filters](https://docs.asp.net/en/latest/mvc/controllers/filters.html) by the ASP.NET team.
* [Setting up ASP.NET Core debugging in VS Code](http://tattoocoder.azurewebsites.net/setting-up-asp-net-core-debugging-in-vs-code/) by Shayne Boyer.
* [Implementing an ASP.NET Core RC1 Logging Provider](http://wildermuth.com/2016/04/22/Implementing-an-ASP-NET-Core-RC1-Logging-Provider) by Shawn Wildermuth.
* [How we did authorization in FubuMVC, and what I’d do differently today](https://jeremydmiller.com/2016/04/19/how-we-did-authorization-in-fubumvc-and-what-id-do-differently-today/) by Jeremy D. Miller.

Games
---

* [Particle System Modules - FAQ](http://blogs.unity3d.com/2016/04/20/particle-system-modules-faq/), by Karl Jones.
* [Game From Scratch C# tutorial in CRYENGINE V (Video)](https://www.youtube.com/watch?v=4u-_a41trHY), by James Brady. 

### Game of the Week: Shiftlings

[Shiftlings](https://madewith.unity.com/games/shiftlings) is a puzzle/platformer style game created by Rock Pocket Games using [Unity](http://unity3d.com/) and [C#](https://channel9.msdn.com/Series/C-Sharp-Fundamentals-Development-for-Absolute-Beginners). Shiftlings starts out showing two adorable alien space janitors, one of whom wanders off and drinks "the fizziest drink in the universe". As a result, he promptly blows up and up like Violet in Willy Wonka. Players navigate both of the conjoined janitors, avoiding traps and fixing problems which get progressively more difficult with each level.  

Shiftlings is available on Xbox One, Playstation 4, Wii U and Steam. More information can be found on the [Made With Unity](https://madewith.unity.com/games/shiftlings) page.

![image](https://cloud.githubusercontent.com/assets/4108756/14821745/bdc014f4-0b80-11e6-8e02-31e0cc8b220b.png)

And this is it for this week!

Contribute to the week in .NET
------------------------------

As always, this weekly post couldn't exist without community contributions,
and I'd like to thank all those who sent links and tips.
You can participate too. Did you write a great blog post, or just read one?
Do you want everyone to know about an amazing new contribution or a useful library?
Did you make or play a great game built on .NET?
We'd love to hear from you, and feature your contributions on future posts:

* Send an email to beleroy at Microsoft,
* [comment on this gist](https://gist.github.com/bleroy/92a112150a2eaa7c556a6d33c6e53a48)
* Leave us a pointer in the comments section below.
* [Send Stacey (@yecats131) tips on Twitter about .NET games](https://twitter.com/yecats131).

This week's post (and future posts) also contains news I first read on
[The ASP.NET Community Standup](https://blogs.msdn.microsoft.com/webdev/tag/communitystandup/),
on [F# weekly](https://sergeytihon.wordpress.com/category/f-weekly/),
on [ASP.NET Weekly](http://www.aspnetweekly.com/),
and on [Chris Alcock's The Morning Brew](http://themorningbrew.net/).
