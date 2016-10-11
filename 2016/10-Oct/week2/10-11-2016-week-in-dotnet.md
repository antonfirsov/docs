The week in .NET - On .NET on Net Standard 2.0 - Nancy - Satellite Reign
========================================================================

To read last week's post, see [The week in .NET – On .NET on Cecil – NAudio – SpeechCentral – Hand of Fate](https://blogs.msdn.microsoft.com/dotnet/2016/10/04/the-week-in-net-on-net-on-cecil-naudio-speechcentral-hand-of-fate/).

On .NET
-------

Last week, [Immo Landwerth was on the show](https://channel9.msdn.com/Shows/On-NET/Immo-Landwerth-Net-Standard) to talk about Net Standard 2.0:

<iframe src="https://channel9.msdn.com/Shows/On-NET/Immo-Landwerth-Net-Standard/player" width="960" height="540" allowFullScreen frameBorder="0"></iframe>

This week, we'll speak with Joe Morris and Srivatsn Narayanan about the new .NET Core build system. The show is on Thursdays and begins at 10AM Pacific Time [on Channel 9](https://channel9.msdn.com/Shows/On-NET). We'll take questions on Gitter, on [the dotnet/home channel](https://gitter.im/dotnet/home) or on Twitter. Please use the `#onnet` tag. It's OK to start sending us questions in advance if you can't do it live during the show.

Package of the week: Nancy
--------------------------

[Nancy](https://github.com/NancyFx/Nancy) is a lightweight, low-ceremony, framework for building HTTP based services on .NET Framework, .NET Core, and Mono. The goal of the framework is to stay out of the way as much as possible and provide a super-duper-happy-path to all interactions.

```csharp
public class Module : NancyModule
{
    public Module()
    {
        Get("/greet/{name}", x => {
            return $"Hello {x.name}";
        });
    }
}
```

Game of the week: Satellite Reign
---------------------------------

[Satellite Reign](https://madewith.unity.com/games/satellite-reign) is a real-time strategy game set in a cyberpunk city. Command a group of four agents, using them to sneak, steal, kill and sabotage to complete your missions. Satellite Reign features an open world, multiple strategies for completing missions and agent customization that allows you to play with your style. You can enjoy single player or multiplayer co-op play, where each person controls an individual agent. 

![screenshot](https://cloud.githubusercontent.com/assets/4108756/19274294/824ecf22-8f84-11e6-8978-14f76519e300.jpg)

[Satellite Reign](https://madewith.unity.com/games/satellite-reign) was created by [5 Lives Studios](http://www.5livesstudios.com/) using [Unity](https://unity3d.com/) and [C#](https://channel9.msdn.com/Series/C-Sharp-Fundamentals-Development-for-Absolute-Beginners). It is available on Windows, Mac OS and Linux on [Steam](http://store.steampowered.com/app/268870/) and [Good Old Games](https://www.gog.com/game/satellite_reign).

User group meeting of the week: The new .NET universe in Berkeley, CA, with Beth Massi
------------------------------------------------

On Friday, October 13 at 6:45 in Berkeley, the [EastBay.NET group](http://www.meetup.com/BayNET/) invites you to [a presentation of the new .NET landscape](http://www.meetup.com/BayNET/events/234653169/) given by [Beth Massi](https://twitter.com/BethMassi).

.NET
----

* [Announcing Visual Studio "15" Preview 5](https://blogs.msdn.microsoft.com/visualstudio/2016/10/05/announcing-visual-studio-15-preview-5/) by John Montgomery.
* [Running a .NET Core app in a Docker container](https://raygun.com/blog/2016/10/net-core-docker-container/) by Callum Gavin.
* [CSProj to XProj: Supporting .NET Core using the Preview tools](http://geekswithblogs.net/mrsteve/archive/2016/10/07/csproj-to-xproj-support-dotnet-core-preview-tools.aspx) by Steve Wilkes.
* [How Buffered IO Can Ruin Performance](https://aloiskraus.wordpress.com/2016/10/09/how-buffered-io-can-ruin-performance/) by Alois Kraus.
* [What's new in Visual Studio "15" Preview 5 for Universal Windows Developers](https://blogs.msdn.microsoft.com/visualstudio/2016/10/06/whats-new-in-visual-studio-15-preview-5-for-universal-windows-developers/) by Karan Nandwani.
* [Debugging high memory usage. Part 2 - .NET Memory Profiler](http://indexoutofrange.com/Debugging-high-memory-usage.Part-2-DotNetMemoryProfiler/) by Szymon Warda.
* [Dealing with Anti-Virus False Positives](https://weblog.west-wind.com/posts/2016/Oct/05/Dealing-with-AntiVirus-False-Positives) by Rick Strahl.
* [Story of Equality in .Net - Part 6](http://developmentpassion.blogspot.co.uk/2016/10/story-of-equality-in-net-part-6.html) by Ehsan Sajjad.

ASP.NET
-------

* [How to reference an existing .NET Framework Project in an ASP.NET Core 1.0 Web App](http://www.hanselman.com/blog/HowToReferenceAnExistingNETFrameworkProjectInAnASPNETCore10WebApp.aspx) by Scott Hanselman.
* [Making ASP.NET apps first-class citizens on Google Cloud Platform](https://cloudplatform.googleblog.com/2016/08/making-ASP.NET-apps-first-class-citizens-on-Google-Cloud-Platform.html) by Chris Sells.
* [Introduction to Authorisation in ASP.NET Core](https://andrewlock.net/introduction-to-authorisation-in-asp-net-core/) by Andrew Lock.
* [ASP.NET MVC Core: HTML Encoding a JSON Request Body](https://www.stevejgordon.co.uk/asp-net-mvc-core-html-encoding-json-body) by Steve J. Gordon.
* [Adding Web API to your .NET Core application](https://jonhilton.net/2016/10/06/adding-web-api-to-your-net-core-application/) by Jon Hilton.
* [Why use Nancy?](http://codeopinion.com/why-use-nancy/) by Derek Comartin.
* [Dockerizing Nerd Dinner: Part 1, Running a Legacy ASP.NET App in a Windows Container](https://blog.sixeyed.com/dockerizing-nerd-dinner-part-1-running-a-legacy-asp-net-app-in-a-windows-container/) by Elton Stoneman.
* [Use Dapper ORM With ASP.NET Core](http://www.talkingdotnet.com/use-dapper-orm-with-asp-net-core/) by Talking Dotnet.

F#
--

* [Ionide F# 2.6.0 for VS Code is released, with CodeLens showing type information!](https://twitter.com/IonideProject/status/783359689920839680).
* [Curious case of disjoint-set](http://red-green-rewrite.github.io/2016/09/30/Curious-case-of-disjoint-set/), by Milosz Krajewski.
* [Data structures done right](http://blog.stermon.com/articles/2016/10/05/data-structures-done-right), by Ramón Soto Mathiesen.
* [Prefer records of functions to interfaces](https://medium.com/@dogwith1eye/prefer-records-of-functions-to-interfaces-d6413af4d2c3#.8863lvebz), by Matthew Doig.
* [GOTO 2016 - Exploring StackOverflow Data with F# (video)](https://www.youtube.com/watch?v=qlKZKN7il7c&feature=youtu.be&list=PLEx5khR4g7PIu7g3dXpwnGFdV69Wp-wce), by Evalina Gabasova.
* [Dynamically extending F# applications](http://kcieslak.io/Dynamically-extending-F-applications) by Krzysztof Cieslak.

Check out [F# Weekly](https://sergeytihon.wordpress.com/category/f-weekly/) for more great content from the F# community.

Xamarin
-------

* [Xamarin Beta Release: Cycle 8 Service Release 0 – Apple TLS](https://releases.xamarin.com/beta-release-cycle-8-service-release-0-apple-tls/) by Luis Aguilera.
* [Xamarin Developer Events in October](https://blog.xamarin.com/xamarin-developer-events-in-october/) by Jayme Singleton.
* [Xamarin and Visual Studio at Future Decoded](https://blog.xamarin.com/xamarin-and-visual-studio-at-future-decoded/) by Julia Black.
* [A Step-by-Step Guide to Building a Profitable Mobile Services Business Through Mobile DevOps](https://blog.xamarin.com/a-step-by-step-guide-to-building-a-profitable-mobile-services-business-through-mobile-devops/) by Francine Anthony.
* [Easier App Debugging with Xamarin Studio Run Configurations](https://blog.xamarin.com/easier-app-debugging-with-xamarin-studio-run-configurations/) and [Adding Bindable Native Views Directly to XAML](https://blog.xamarin.com/adding-bindable-native-views-directly-to-xaml/) by Pierce Boggan.
* [The Xamarin Show 4: Continuous Delivery with Josh Weber](https://channel9.msdn.com/Shows/XamarinShow/Continuous-Delivery-with-Josh-Weber) and [The Xamarin Show 5: MVVM & Data Binding with Xamarin.Forms](https://channel9.msdn.com/Shows/XamarinShow/Introduction-to-MVVM) by James Montemagno.
* [Building Beautiful Apps With Xamarin Forms](http://www.rarelyimpossible.com/blog/2016/9/26/building-beautiful-apps-with-xamarin-forms) by Rarely Impossible.
* [In-App Gestures And Mobile App User Experience](https://www.smashingmagazine.com/2016/10/in-app-gestures-and-mobile-app-user-experience/) by Nick Babich.
* [Static Initialization](https://xamarinhelp.com/static-initialization/) by Adam Pedley.
* [ReactiveUI Goodies – Observing Properties](https://janhannemann.wordpress.com/2016/10/03/reactiveui-goodies-observing-properties/) by Jan Hannemann.

Games
-----

* [Unity 5 Tutorial: How to make a climbing system like in Assassins Creed in Unity - part 10 (video)](https://www.youtube.com/watch?v=O-sE2mZpaXU) by Gamad.
* [Unity - 2D Movement (Part 5) - Building Tank Prefab (video)](https://www.youtube.com/watch?v=uvMLNkk9SeA) by Pixel Make
* [Beginning C# with Unity: Part 14: Foreach Loops (video)](https://videos.raywenderlich.com/courses/beginning-c/lessons/14) by Brian Moakley.
* [7.1 Unity Tower defense tutorial - Spawn position (video)](https://www.youtube.com/watch?v=c43OJOBjaL0&feature=youtu.be) by inScope Studios.

And this is it for this week!

Contribute to the week in .NET
------------------------------

As always, this weekly post couldn't exist without community contributions, and I'd like to thank all those who sent links and tips. The F# section is provided by Phillip Carter, the gaming section by Stacey Haffner, and the Xamarin section by Dan Rigby.

You can participate too. Did you write a great blog post, or just read one? Do you want everyone to know about an amazing new contribution or a useful library? Did you make or play a great game built on .NET?
We'd love to hear from you, and feature your contributions on future posts:

* Send an email to beleroy at Microsoft,
* [comment on this gist](https://gist.github.com/bleroy/332a9da79f99d042aa4ed0e7d1fa4f51)
* Leave us a pointer in the comments section below.
* [Send Stacey (@yecats131) tips on Twitter about .NET games](https://twitter.com/yecats131).

This week's post (and future posts) also contains news I first read on [Weekly Xamarin](http://weeklyxamarin.com/), on [F# weekly](https://sergeytihon.wordpress.com/category/f-weekly/), and on [Chris Alcock's The Morning Brew](http://themorningbrew.net/).
