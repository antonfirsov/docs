The week in .NET - 8/16/2016
============================

To read last week's post, see [The week in .NET – 8/9/2016](https://blogs.msdn.microsoft.com/dotnet/2016/08/09/the-week-in-net-892016/).

On .NET
-------

Last week, we had [Pablo Santos and Francisco Monteverde to talk about PlasticSCM](https://www.youtube.com/watch?v=wPNKyC5sbac), a version control system with interesting features such as semantic merge and digital asset management. If you think version control is done and that Git is the end of it, you should check out the video, and prepare to be surprised:

<iframe width="560" height="315" src="https://www.youtube.com/embed/wPNKyC5sbac" frameborder="0" allowfullscreen></iframe>

This week, we'll have [Lucas Meijer from Unity 3D](https://www.youtube.com/watch?v=lWbP4bHEGLQ).

Package of the week: Orleans
----------------------------

[Orleans](http://dotnet.github.io/orleans/) is a framework that provides a straightforward approach to building distributed high-scale computing applications, without the need to learn and apply complex concurrency or other scaling patterns. It was created by Microsoft Research and designed for use in the cloud.

Orleans has been used extensively in Microsoft Azure by several Microsoft product groups, most notably by 343 Industries as a platform for all of Halo 4 and Halo 5 cloud services, as well as by a growing number of other companies.

Here's an example of an actor in Orleans:

```csharp
//an example of a Grain (actor) Interface
public interface IPlayerGrain : IGrainWithGuidKey
{
  Task<IGameGrain> GetCurrentGame();
  Task JoinGame(IGameGrain game);
  Task LeaveGame(IGameGrain game);
}

//an example of a Grain class implementing a Grain Interface
public class PlayerGrain : Grain, IPlayerGrain
{
    private IGameGrain currentGame;

    // Game the player is currently in. May be null.
    public Task<IGameGrain> GetCurrentGame()
    {
       return Task.FromResult(currentGame);
    }

    // Game grain calls this method to notify that the player has joined the game.
    public Task JoinGame(IGameGrain game)
    {
       currentGame = game;
       Console.WriteLine("Player {0} joined game {1}", this.GetPrimaryKey(), game.GetPrimaryKey());
       return TaskDone.Done;
    }

   // Game grain calls this method to notify that the player has left the game.
   public Task LeaveGame(IGameGrain game)
   {
       currentGame = null;
       Console.WriteLine("Player {0} left game {1}", this.GetPrimaryKey(), game.GetPrimaryKey());
       return TaskDone.Done;
   }
}
```

And here's how client code could use it:

```csharp
//construct the grain reference of a specific player
var player = GrainClient.GrainFactory.GetGrain<IPlayerGrain>(playerId);

//Invoking a grain method asynchronously
await player.JoinGame(this);
players.Add(playerId);
```

User group meeting of the week: .NET Bangalore eigth meetup
-----------------------------------------------------------

Join the [.NET Bangalore user group](http://www.meetup.com/DotNetBLR/) for [a full day of .NET goodness on Saturday, August 20](http://www.meetup.com/DotNetBLR/events/227882870/), with talks about UWP, Azure App Insights, SignalR, and .NET Core.

.NET
----

* [Introducing the .NET Framework Monthly Rollup](https://blogs.msdn.microsoft.com/dotnet/2016/08/15/introducing-the-net-framework-monthly-rollup) by Stacey Haffner.
* [Announcing NuGet 3.5 RC](http://blog.nuget.org/20160811/Announcing-NuGet-3.5-RC.html) by Harikrishna Menon.
* [How to avoid recursion](http://metacoding.azurewebsites.net/2016/08/16/how-to-avoid-recursion/) by Matthieu Mezil.
* [I tell you, that thing is a bona fide ZEBRA, or a tale of being utterly stupid](https://ayende.com/blog/174947/i-tell-you-that-thing-is-a-bona-fide-zebra-or-a-tale-of-being-utterly-stupid) and [Exceptional costs, Part II](https://ayende.com/blog/175010/digging-into-the-coreclr-exceptional-costs-part-ii) by Ayende Rahien.
* [Building a Producer Consumer Queue with TPL Dataflow](https://jeremydmiller.com/2016/08/09/building-a-producer-consumer-queue-with-tpl-dataflow/) and [Health Monitoring and Task Reassignment in our Service Bus Applications](https://jeremydmiller.com/2016/08/11/using-the-bully-algorithm-in-our-service-bus-applications/) by Jeremy D Miller.
* [MSTest V2 - First impressions](http://blog.drorhelper.com/2016/08/mstest-v2-first-impressions.html) by Dror Helper.
* [Retrieving Performance Counter from a remote PC using C#](http://www.productiverage.com/retrieving-performance-counter-from-a-remote-pc-using-c-sharp) by Productive Rage.
* [Couchbase .NET SDK 2.3.5 now available with .NET Core support](http://blog.couchbase.com/2016/august/couchbase-.net-sdk-2.3.5-now-available) by Jeff Morris.
* [Application Insights & Semantic Logging for Service Fabric Microservices](http://www.medic-consulting.com/2016/08/12/Application-Insights-and-Semantic-Logging-for-Service-Fabric-Microservices/) by Andrej Medic.

ASP.NET
-------

* [Debug Dockerized .NET Core Apps with VS Code](http://www.bloggedbychris.com/2016/08/03/debug-dockerized-net-core-apps-code/) by Chris Myers.
* [Introduction to Authentication with ASP.NET Core](http://andrewlock.net/introduction-to-authentication-with-asp-net-core/) and [Access services inside ConfigureServices using IConfigureOptions in ASP.NET Core](http://andrewlock.net/access-services-inside-options-and-startup-using-configureoptions/) by Andrew Lock.
* [Practical Permissions-based Authorization in ASP.NET Core MVC](http://benjamincollins.com/blog/practical-permission-based-authorization-in-asp-net-core/) by Ben Collins.
* [Real-World ASP.NET Core MVC Filters](https://msdn.microsoft.com/en-us/magazine/mt767699.aspx) by Steve Smith.
* [Using Semantic UI with ASP.NET Core](http://www.khalidabuhakmeh.com/using-semantic-ui-with-asp-net-core) and [Strongly Typed Configuration Settings in ASP.NET Core Part II](http://rimdev.io/strongly-typed-configuration-settings-in-asp-net-core-part-ii/) by Khalid Abuhakmeh.
* [Global Routes for ASP.NET Core MVC](http://benjii.me/2016/08/global-routes-for-asp-net-core-mvc/) by Ben Cull.
* [Add Swagger to ASP.NET Core Web API](http://www.talkingdotnet.com/add-swagger-to-asp-net-core-web-api/) by Talking Dotnet.
* [Should I Use ASP.NET Core or MVC 5?](http://www.jeffreyfritz.com/2016/08/should-i-use-asp-net-core-or-mvc-5/) by Jeffrey T Fritz.
* [WebAPIContrib.Core](https://channel9.msdn.com/coding4fun/blog/WebAPIContribCore) by Greg Duncan.

F#
--

* [Jet.com, an F# and Azure startup, sells for 3 Billion Dollars](https://techcrunch.com/2016/08/07/walmart-buys-jet-com-for-3-billion/)
* [How to Parse a Git Log with FParsec](http://blog.leifbattermann.de/2016/08/11/how-to-parse-a-git-log-with-fparsec/), by Leif Batterman
* [Understanding Xamarin Forms Data Bindings with F#](https://kimsereyblog.blogspot.com.by/2016/08/understand-xamarin-forms-data-bindings.html), by Kimserey Lam
* [Experiment with F# for Data Visualization of the Olympics](http://rio2016.thegamma.net), by Tomas Petricek
* [A F# Akka.NET actor example for pub-sub pattern with NATS server](http://carstenj.io/2016/07/01/docker-nats.io-akka.net-fsharp.html), by Сarsten Jørgensen

Check out [F# Weekly](https://sergeytihon.wordpress.com/category/f-weekly/) for more great content from the F# community.

Xamarin
-------

* [Xamarin Dev Days: More Dates & More Cities!](https://blog.xamarin.com/xamarin-dev-days-more-dates-more-cities/) by Jayme Singleton.
* [Preview: iOS 10 / Xcode 8 / Sierra Support Update 2](https://releases.xamarin.com/preview-ios-10-xcode-8-sierra-support-update-2/) and [Preview: iOS Simulator (For Windows) update 3](https://releases.xamarin.com/preview-ios-simulator-for-windows-update-3/) by Adrian Murphy.
* [Authenticating Mobile Apps with Azure Active Directory B2C](https://blog.xamarin.com/authenticating-mobile-apps-with-azure-active-directory-b2c/) and [Performing OCR for iOS, Android, and Windows with Microsoft Cognitive Services](https://blog.xamarin.com/performing-ocr-for-ios-android-and-windows-with-microsoft-cognitive-services/) by Pierce Boggan.
* [Declarative & implicit animations Library for Xamarin Forms](https://github.com/OliveTreeBible/Xamarin.Transitions) by Olive Tree.
* [Announcing Cake.Raygun](https://ghuntley.com/archive/2016/08/09/announcing-cake-raygun/) by Geoffrey Huntley.
* [Interacting with Siri on Xamarin in iOS 10](https://xamarinhelp.com/interacting-siri-xamarin/) by Adam Pedley.
* [Formatted number entry](http://thatcsharpguy.com/post/formatted-number-entry/) by Antonio Feregrino Bolaños.
* [Creating Animations with Xamarin.Forms](https://blog.xamarin.com/creating-animations-with-xamarin-forms/) by David Britch.
* [Composable Customizations with Xamarin.Forms](https://visualstudiomagazine.com/articles/2016/08/01/composable-customizations.aspx) by Greg Shackles.
* [Installing a PCL into netstandard Libraries ](http://motzcod.es/post/148657853472/installing-a-pcl-into-netstandard-libraries) by James Montemagno.
* [Using the ContainerView to Transition between Views - aka More Fragments in Xamarin.iOS](http://www.blogaboutxamarin.com/using-the-containerview-to-transition-between-views-aka-more-fragments-in-xamarin-ios/) by Richard Woollcott.

And this is it for this week!

Contribute to the week in .NET
------------------------------

As always, this weekly post couldn't exist without community contributions, and I'd like to thank all those who sent links and tips.

You can participate too. Did you write a great blog post, or just read one? Do you want everyone to know about an amazing new contribution or a useful library? Did you make or play a great game built on .NET?
We'd love to hear from you, and feature your contributions on future posts:

* Send an email to beleroy at Microsoft,
* [comment on this gist](https://gist.github.com/bleroy/2dca9ef30cf0da8d4ebf92dfb9ec07ff)
* Leave us a pointer in the comments section below.
* [Send Stacey (@yecats131) tips on Twitter about .NET games](https://twitter.com/yecats131).

This week's post (and future posts) also contains news I first read on [The ASP.NET Community Standup](https://blogs.msdn.microsoft.com/webdev/tag/communitystandup/), on [Weekly Xamarin](http://weeklyxamarin.com/), on [F# weekly](https://sergeytihon.wordpress.com/category/f-weekly/), on [ASP.NET Weekly](http://www.aspnetweekly.com/), and on [Chris Alcock's The Morning Brew](http://themorningbrew.net/).
