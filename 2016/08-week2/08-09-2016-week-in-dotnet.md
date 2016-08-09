The week in .NET - 8/9/2016
============================

To read last week's post, see [The week in .NET – 8/2/2016](https://blogs.msdn.microsoft.com/dotnet/2016/08/02/the-week-in-net-822016/).

On .NET
-------

Last week on the show, we had [Frank Krueger](https://www.youtube.com/watch?v=3LuIirxXNjk) to talk about his amazing [Continuous C# and F# IDE for the iPad](http://praeclarum.org/post/147003028753/continuous-c-and-f-ide-for-the-ipad).

<iframe width="560" height="315" src="https://www.youtube.com/embed/3LuIirxXNjk" frameborder="0" allowfullscreen></iframe>

This week, we'll speak with [Francisco Monteverde](https://www.youtube.com/watch?v=wPNKyC5sbac) about [PlasticSCM](https://www.plasticscm.com/).

Package of the week: OxyPlot
----------------------------

[OxyPlot](http://www.oxyplot.org/) is an open source and cross-platform plotting library.

Here's how you'd plot the `cos` function in a Universal Windows application:

```csharp
public class MainViewModel
{
    public MainViewModel()
    {
        this.MyModel = new PlotModel { Title = "Cosine" };
        this.MyModel.Series.Add(new FunctionSeries(Math.Cos, 0, 10, 0.1, "cos(x)"));
    }

    public PlotModel MyModel { get; private set; }
}
```

```xaml
<Page.DataContext>
    <local:MainViewModel/>
</Page.DataContext>

<Grid Background="{ThemeResource ApplicationPageBackgroundThemeBrush}">
    <oxy:PlotView Model="{Binding MyModel}"/>
</Grid>
```

![A cosine plotted in a phone emulator](http://docs.oxyplot.org/en/latest/_images/windows-universal-app-example1.png)

Tool of the week: .NET API Catalog
----------------------------------

[.NET API Catalog](http://apisof.net/catalog/System) is a new tool that makes it easy to explore .NET APIs and figure out what support exists for each API on each version of .NET: .NET Framework, .NET Standard, Mono, or even Silverlight. I particularly like the hackable URLs that enable you to easily reach any API from its long name.

The tool runs on Azure, with an in memory object model that is pulled from Azure Blob Storage. The deployment of the web site is fully automated and is happening each time a commit happens to our internal CoreFxTools Git repo, which is hosted on VSTS, without disrupting service.

User group meeting of the week: IOT RpiCar and ASP.NET Core + Docker in Bucarest, Romania
-----------------------------------------------------------------------------------------

Join the ADCES group and Victor Hurdugaci on Tuesday, August 9 at 7:00PM at the AFI PARK 2, Bucarest, Romania for a session about [IOT RpiCar and ASP.NET Core + Docker](http://www.meetup.com/Bucharest-A-D-C-E-S-Meetup/events/232773415/).

.NET
----

* [Announcing .NET Framework 4.6.2](https://blogs.msdn.microsoft.com/dotnet/2016/08/02/announcing-net-framework-4-6-2/) by Stacey Haffner.
* [Compile your changes on the fly with .NET Core Watch](https://jonhilton.net/2016/08/04/compile-your-changes-on-the-fly-with-net-core/) by Jon Hilton.
* [Entity Framework Core 1.0 - Table Valued Functions and LINQ Composition](https://cmatskas.com/entity-framework-core-1-0-table-valued-functions-and-linq-composition/) by Christos Matskas.
* [Announcing Machine.Specifications (MSpec) 0.11 for .NET Core, .NET CLI and .NET Standard](http://ivanz.com/2016/08/05/announcing-mspec-for-net-core/) by Ivan Zlatev.
* [NSubstitute 2.0.0 (RC) released with .NET Core compatibility](https://groups.google.com/forum/m/#!topic/nsubstitute/YP2TIgczBm0/discussion) by David Tchepak.
* [.NET thread-pool threads and CLR worker threads](http://geekswithblogs.net/FrostRed/archive/2016/08/04/188548.aspx) by Changhong Fu.
* [Fast Deep Copy by Expression Trees (C#)](http://www.codeproject.com/Articles/1111658/Fast-Deep-Copy-by-Expression-Trees-C-Sharp) by Frakon.
* [Throttling to improve responsiveness](https://github.com/jbe2277/waf/wiki/Throttling-to-improve-responsiveness) by jbe2277.
* [Exploring Entity Framework Core 1.0.0 RTM Changes](http://stevejgordon.co.uk/exploring-entity-framework-core-1-0-0-rtm-changes) by Steve Gordon.

ASP.NET
-------

* [ASP.NET Core Kestrel - The Need for Speed](http://www.poppastring.com/blog/ASPNETCoreKestrelTheNeedForSpeed.aspx) by Mark Downie.
* [ASP.NET Core Dependency Injection](http://wildermuth.com/2016/08/07/ASP-NET-Core-Dependency-Injection) by Shawn Wildermuth.
* [Add HTTP headers to static files in ASP.​NET Core](http://asp.net-hacker.rocks/2016/08/04/add-http-header-to-static-files-in-aspnetcore.html) by Jürgen Gutsch.
* [Integration testing your ASP.NET Core middleware using TestServer](http://josephwoodward.co.uk/2016/07/integration-testing-asp-net-core-middleware) by Joseph Woodward.
* [Simulating Latency in ASP.NET Core](https://blog.mariusschulz.com/2016/08/06/simulating-latency-in-asp-net-core) by Marius Schulz.
* [Forking the pipeline - adding tenant-specific files with SaasKit in ASP.NET Core](http://andrewlock.net/forking-the-pipeline-adding-tenant-specific-files-with-saaskit-in-asp-net-core/) by Andrew Lock.

F#
--

* [Walmart Rewrites Its E-Commerce Strategy With $3.3 Billion Deal for Jet.com](http://www.nytimes.com/2016/08/09/business/dealbook/walmart-jet-com.html) by Leslie Picker and Rachel Abrams.
* [F# in Numbers: A Look at the Annual F# Survey Results](https://www.infoq.com/articles/fsharp-community-survey-2016) by Tomas Petricek.
* [F# to Javascript with Tomas Petricek (podcast)](http://dotnetrocks.com/?show=1330).`
* [Perspectives on Clojure and F# (video)](https://channel9.msdn.com/Blogs/Charles/Emerging-Langs-Clojure-and-F) by Rich Hickey and Joe Pamer.
* [F# for Scala Developers (slides)](https://alfonsogarciacaro.github.io/fsharp-for-scala-developers/#/) by Alfonso Garcia-Caro.
* [If you're not live-codeing, you're dead-coding (video)](https://vimeo.com/131658147) by Jeremy Chassaing.
* [Fable |> React Native – Native apps with F#](http://www.navision-blog.de/blog/2016/08/06/fable-react-native/) by Steffen Forkmann.
* [Incremental construction of DFA in F#](http://www.codeproject.com/Articles/1110749/Incremental-construction-of-DFA-in-Fsharp) by Vyacheslav Chernykh.
* [Building an OData service in F# using Entity Framework and Suave](https://fsharp.tv/gazettes/building-an-odata-service-in-f-using-entity-framework-and-suave-gazette-007-2/) by Tamizh Vendan.
* [TypeShape: Practical Generic Programming in F#](https://eiriktsarpalis.wordpress.com/2016/08/05/typeshape-practical-generic-programming-in-f/) by Eirik Tsarpalis.

Xamarin
-------

* [Xamarin.Forms 2.3.1-stable](https://forums.xamarin.com/discussion/70166/xamarin-forms-2-3-1-stable) by Bryan Hunter.
* [Using Speech Recognition in iOS 10](http://gregshackles.com/using-speech-recognition-in-ios-10/) by Greg Shackles.
* [.NET Standard Library Support for Xamarin](https://blog.xamarin.com/net-standard-library-support-for-xamarin/) by James Montemagno.
* [.NET Standard Library with Xamarin Forms](https://xamarinhelp.com/dot-net-standard-pcl-xamarin-forms/) and [.NET Standard with Xamarin Forms Gotchas](https://xamarinhelp.com/net-standard-xamarin-forms-gotchas/) by Adam Pedley.
* [Integrating Azure Active Directory B2C into Xamarin Mobile App](http://www.hossambarakat.net/2016/07/07/integrating-azure-active-directory-b2c-into-xamarin-mobile-app/) by Hossam Barakat.
* [Using the ContainerView to Share Views - aka Fragments in Xamarin.iOS](http://www.blogaboutxamarin.com/using-the-containerview-to-share-views-aka-fragments-in-xamarin-ios/) by Richard Woollcott.

And this is it for this week!

Contribute to the week in .NET
------------------------------

As always, this weekly post couldn't exist without community contributions, and I'd like to thank all those who sent links and tips.

You can participate too. Did you write a great blog post, or just read one? Do you want everyone to know about an amazing new contribution or a useful library? Did you make or play a great game built on .NET?
We'd love to hear from you, and feature your contributions on future posts:

* Send an email to beleroy at Microsoft,
* [comment on this gist](https://gist.github.com/bleroy/146844abfd548e28296f9b59a57b4199)
* Leave us a pointer in the comments section below.
* [Send Stacey (@yecats131) tips on Twitter about .NET games](https://twitter.com/yecats131).

This week's post (and future posts) also contains news I first read on [The ASP.NET Community Standup](https://blogs.msdn.microsoft.com/webdev/tag/communitystandup/), on [Weekly Xamarin](http://weeklyxamarin.com/), on [F# weekly](https://sergeytihon.wordpress.com/category/f-weekly/), on [ASP.NET Weekly](http://www.aspnetweekly.com/), and on [Chris Alcock's The Morning Brew](http://themorningbrew.net/).
