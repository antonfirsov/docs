The week in .NET: .NET Core 1.0.1 - On .NET with Peachpie - Avalonia - Folk Tale
================================================================================

To read last week's post, see [The week in .NET – 9/7/2016](https://blogs.msdn.microsoft.com/dotnet/2016/09/07/the-week-in-net-972016/#comments).

.NET Core 1.0.1 shipped!
------------------------

We shipped .NET Core 1.0.1 this morning. Check out [the announcement](xx)!

On .NET
-------

Last week, we spoke with Benjamin Fistein and Jakub Míšek about Peachpie, a PHP compiler for .NET. We've had Ben and Jakub on the show before, and they came back to show us some of the new features they've built: .NET Core compatibility, debugging in VS Code, Docker deployment, and NuGet package building and consumption.

<iframe src="https://channel9.msdn.com/Shows/On-NET/Benjamin-Fistein--Jakub-Mek-Peachpie-PHP-compiler-for-NET/player" width="560" height="315" allowFullScreen frameBorder="0"></iframe>

This week, we'll speak about [Steeltoe](http://steeltoe.io), a .NET toolkit for common microservice patterns. The show begins at 10AM Pacific Time [on Channel 9](https://channel9.msdn.com/Shows/On-NET). We'll take questions on Gitter, on [the dotnet/home channel](https://gitter.im/dotnet/home). Please use the `#onnet` tag. It's OK to start sending us questions in advance if you can't do it live during the show.

Project of the week: Avalonia
-----------------------------

[Avalonia](https://github.com/avaloniaui/avalonia) is a multi-platform UI toolkit, somewhat like WPF. It supports XAML and a flexible styling system, and runs on Windows, Linux, macOS, iOS, and Android.

<iframe width="560" height="315" src="https://www.youtube.com/embed/c_AB_XSILp0" frameborder="0" allowfullscreen></iframe>

```xml
<ListBox Items="ItemsSource">
    <ListBox.DataTemplates>
        <DataTemplate>
            <TextBlock Text="{Binding Caption}"/>
        </DataTemplate>
    </ListBox.DataTemplates>
</ListBox>
```

Game of the Week: Folk Tale
---------------------------

[Folk Tale](http://www.gamesfoundry.com/) blends the mechanics of a real time strategy with role playing elements. Players balance building a thriving village from nothing whilst exploring the vast world, making new relationships and discovering new loot. Folk Tale features random events and a dynamic story, letting you run your village the way you want while never knowing exactly how the story will unfold. Both campaign and sandbox modes can be enjoyed in addition to an in-game editor which the community can use to make their own worlds.

![game](https://cloud.githubusercontent.com/assets/4108756/18481379/d6f29d2e-7990-11e6-9256-cd843c50bd35.png)

[Folk Tale](http://www.gamesfoundry.com/) was created by Games Foundry using [Unity](https://unity3d.com/) and [C#](https://channel9.msdn.com/Series/C-Sharp-Fundamentals-Development-for-Absolute-Beginners). It is currently in early access on [Steam](http://store.steampowered.com/app/224440/) and is available for Windows and Mac OS X.

User group meeting of the week: ASP.NET Core - What you need to know to be productive on day 1, in Durham, NC
------------------------------------------------

[TRINUG](http://www.meetup.com/TRINUG/) hold a user group meeting on Wednesday, September 14 at 5:30PM about [ASP.NET Core and what you need to know to be productive on day 1](http://www.meetup.com/TRINUG/events/233282527/). Steve Michelotti will be the speaker.

.NET
----

* [The .NET Fringe 2016 conference recordings are available](https://www.youtube.com/playlist?list=PLwZVRWVJepJvam4NiKwKfR9P1aInAHev_).
* [Quick summary of what’s new in Entity Framework Core 1.0](http://www.talkingdotnet.com/summary-whats-new-entity-framework-core/) by Talking Dotnet.
* [ImageProcessor Core](http://jamessouth.me/archive/imageprocessor-core/) by James Jackson-South. We featured James' [ImageProcessor as package of the week](https://blogs.msdn.microsoft.com/dotnet/2015/12/29/the-week-in-net-12292015/) back in December last year.
* [Unobserved Exceptions](https://github.com/jbe2277/waf/wiki/Unobserved-Exceptions) by jbe2277.
* [.NET Core and Microsoft Bot Framework](https://carlos.mendible.com/2016/09/11/netcore-and-microsoft-bot-framework/) by Carlos Mendible.
* [DateTime under the hood](http://aakinshin.net/en/blog/dotnet/datetime/) and [Stopwatch under the hood](http://aakinshin.net/en/blog/dotnet/stopwatch/) by Andrey Akinshin.
* [Stop wasting time during .NET Core builds](http://donovanbrown.com/post/2016/08/28/Stop-wasting-time-during-NET-Core-builds) by Donovan Brown.
* [The Dotnet Watch Tool](http://rehansaeed.com/the-dotnet-watch-tool/) by Muhammad Rehan Saeed.
* [OneTrueError - Automated exception handling](http://www.codeproject.com/Articles/1126297/OneTrueError-Automated-exception-handling) by Jonas Gauffin.
* [Another awesome curated list of links about .NET performance](https://github.com/adamsitnik/awesome-dot-net-performance) by Adam Sitnik.

ASP.NET
-------

* [Customizing ASP.NET Core MVC: filters, constraints and conventions](https://luisfsgoncalves.wordpress.com/2016/09/10/customizing-asp-net-core-mvc-filters-constraints-and-conventions/) by Luís Gonçalves.
* [An introduction to OpenID Connect in ASP.NET Core](http://andrewlock.net/an-introduction-to-openid-connect-in-asp-net-core/), and [Configuring environment specific services for dependency injection in ASP.NET Core](http://andrewlock.net/configuring-environment-specific-services-in-asp-net-core/) by Andrew Lock.
* [Real-World CQRS/ES with ASP.NET and Redis Part 1](https://www.exceptionnotfound.net/real-world-cqrs-es-with-asp-net-and-redis-part-1-overview/), and [Part 2](https://www.exceptionnotfound.net/real-world-cqrs-es-with-asp-net-and-redis-part-2-the-write-model/) by Matthew Jones.
* [What I Learned Building with ASP.NET Core: Part 1 - Routing](http://wildermuth.com/2016/09/05/What-I-Learned-Building-with-ASP-NET-Core-Part-1---Routing) by Shawn Wildermuth.
* [A Recipe Book for DropDownLists in ASP.NET MVC](http://www.danylkoweb.com/Blog/a-recipe-book-for-dropdownlists-in-aspnet-mvc-HA) by Jonathan Danylko.
* [Using ASP.NET Core against .NET 4.6](https://jonhilton.net/2016/09/07/using-asp-net-core-against-net-4-6/) by Jon Hilton.
* [\[Controller\] and \[NonController\] attributes in ASP.NET Core MVC](http://www.strathweb.com/2016/09/controller-and-noncontroller-attributes-in-asp-net-core-mvc/) by Filip W.
* [ASP.NET Core continuous deployment with Docker Hub](https://stefanprodan.com/2016/aspnetcore-cd-pipeline-docker-hub/) by Stefan Prodan.
* [ASP.NET Core Action Arguments Validation using an ActionFilter](https://damienbod.com/2016/09/09/asp-net-core-action-arguments-validation-using-an-actionfilter/) by Damien Bod.

F#
--

* [Managing Complexity - Or "Why do you code in F#?"](http://anthonylloyd.github.io/blog/2016/09/09/managing-complexity), by Anthony Lloyd.
* [Event Sourcing is Awesome!](https://tech.jet.com/blog/2016/09-07-event-sourcing-awesome/), by Gad Berger.
* [Getting Started with Azure Functions and F#](http://gregshackles.com/getting-started-with-azure-functions-and-f/) by Greg Shackles.
* [Size of Blobs in Azure Storage Account](https://funcxz.github.io/functional%20exercises/2016/09/06/size-of-blobs-in-azure-storage-account), by Denys Kholod.
* [Recursion and Pattern Matching](http://www.jason-down.com/2016/09/02/recursion-and-pattern-matching/), by Jason Down.

Check out [F# Weekly](https://sergeytihon.wordpress.com/category/f-weekly/) for more great content from the F# community.

Azure
-----

* [Create And Publish ASP.NET WEB API In Azure](http://www.c-sharpcorner.com/article/create-and-publish-asp-net-web-api-in-azure/) by Gowtham K.

Xamarin
-------

* [Let the iOS 10-ing begin!](https://blog.xamarin.com/let-the-ios10-ing-begin/) and [Xamarin.Android 7.0 Now With More Nougat](https://blog.xamarin.com/xamarin-android-7-0-now-with-more-nougat/) by Miguel de Icaza.
* [Introducing The Xamarin Show on Channel 9](https://blog.xamarin.com/introducing-the-xamarin-show-on-channel-9/) by James Montemagno.
* [Xamarin Developer Events in September](https://blog.xamarin.com/xamarin-developer-events-in-september/) by Jayme Singleton.
* [Xamarin Beta Preview 4: Cycle 8](https://releases.xamarin.com/beta-preview-4-cycle-8/) by Adrian Murphy.
* [Free eBook: Microsoft Platform and Tools for Mobile App Development](https://buildazure.com/2016/09/08/free-ebook-microsoft-platform-and-tools-for-mobile-app-development/) by Microsoft Press.
* [Building cross-platform Xamarin.Forms apps in VSTS](https://jimblizzard.wordpress.com/2016/08/09/building-cross-platform-xamarin-forms-apps-in-vsts/) and [Android Keystore file and password in VSTS builds](https://jimblizzard.wordpress.com/2016/08/28/android-keystore-file-in-vsts-builds/) by Jim Blizzard.
* [UWP OAuth in Xamarin Forms using Xamarin.Auth](http://damianblog.com/2016/09/04/uwp-oauth-in-xamarin-forms-using-xamarin-auth/) by Damian Mehers.
* [Accessing Android Application Context outside Activity in Xamarin](http://blog.falafel.com/accessing-android-application-context-outside-activity-xamarin/) by Venkata Koppaka.
* [Navigation tab bar with colorful interactions for Xamarin Android](https://github.com/martijn00/NavigationTabBarXamarin) by Martijn van Dijk.
* [XAML Power Toys for Visual Studio 2015](https://github.com/Oceanware/XAMLPowerToys2015) by Karl Shifflett.

Games
---
* [(Unity 5) Let's Make Rust! \[Episode 28 - Inventory 6\] (video)](https://www.youtube.com/watch?v=X7SWDWaVOYQ) by Gabemeister1201.
* [Monogame - Building multi-platform solutions (video)](https://www.youtube.com/watch?v=WonVmlpPBuU) by Simon Jackson.
* [Shaders Case Study - Pixel Art Palette Swapping (video)](https://www.youtube.com/watch?v=u4Iz5AJa31Q) by Makin' Stuff Look Good.
* [Animated Metro UI Tutorial - Unity3D (video)](https://www.youtube.com/watch?v=PYrDztnGmUw) by Supermassive

And this is it for this week!

Contribute to the week in .NET
------------------------------

As always, this weekly post couldn't exist without community contributions, and I'd like to thank all those who sent links and tips. The F# section is provided by Phillip Carter, the gaming section by Stacey Haffner, and the Xamarin section by Dan Rigby.

You can participate too. Did you write a great blog post, or just read one? Do you want everyone to know about an amazing new contribution or a useful library? Did you make or play a great game built on .NET?
We'd love to hear from you, and feature your contributions on future posts:

* Send an email to beleroy at Microsoft,
* [comment on this gist](https://gist.github.com/bleroy/c5c165160a2c1f5f610e659895f9657f)
* Leave us a pointer in the comments section below.
* [Send Stacey (@yecats131) tips on Twitter about .NET games](https://twitter.com/yecats131).

This week's post (and future posts) also contains news I first read on [The ASP.NET Community Standup](https://blogs.msdn.microsoft.com/webdev/tag/communitystandup/), on [Weekly Xamarin](http://weeklyxamarin.com/), on [F# weekly](https://sergeytihon.wordpress.com/category/f-weekly/), on [ASP.NET Weekly](http://www.aspnetweekly.com/), and on [Chris Alcock's The Morning Brew](http://themorningbrew.net/).
