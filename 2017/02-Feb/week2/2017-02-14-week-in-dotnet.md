The week in .NET - On .NET with Phil Haack
================================================================================

Previous posts:
* [On .NET on Docker and new Core tooling, Benchmark.NET, Magicka](https://blogs.msdn.microsoft.com/dotnet/2017/02/07/the-week-in-net-on-net-on-docker-and-new-core-tooling-benchmark-net-magicka/).
* [On .NET on public speaking, ndepend, CrazyCore, The Perils of Man](https://blogs.msdn.microsoft.com/dotnet/2017/01/31/the-week-in-net-on-net-on-public-speaking-ndepend-crazycore-the-perils-of-man/).
* [On .NET with David Pine, PwdLess, Terraria](https://blogs.msdn.microsoft.com/dotnet/2017/01/18/the-week-in-net-on-net-with-david-pine-pwdless-terraria/).

Happy 15th Birthday .NET!
-------------------------

This week marks the 15th anniversary since .NET debuted to the world. On February 13th, 2002, the first version of .NET was released as part of Visual Studio.NET. [Read Beth Massi's post, featuring a new interview of Anders Hejlsberg](https://blogs.msdn.microsoft.com/dotnet/2017/02/13/happy-15th-birthday-net/).

On .NET
-------

In [this week's episode](https://channel9.msdn.com/Shows/On-NET/Phil-Haack-GitHub), we're speaking with [Phil Haack](https://twitter.com/haacked) from [GitHub](https://github.com):

<iframe src="https://channel9.msdn.com/Shows/On-NET/Phil-Haack-GitHub/player" width="960" height="540" allowFullScreen frameBorder="0"></iframe>

This week, we'll take a look back on the past 15 years of .NET with [Beth Massi](https://twitter.com/BethMassi). We'll stream live [on Channel 9](https://channel9.msdn.com/Shows/On-NET). We'll take questions on [Gitter's dotnet/home channel](https://gitter.im/dotnet/home) and on Twitter. Please use the `#onnet` tag. It's OK to start sending us questions in advance if you can't do it live during the shows.

Package of the week: Readline
-----------------------------

[Readline](https://github.com/tsolarin/readline) is one of those libraries that do one thing, and do it well. Its purpose is to implement a user prompt in your console applications, with standard keyboard shortcuts, command history, and customizable auto-complete. It is built to be a .NET implementation of [GNU Readline](https://en.wikipedia.org/wiki/GNU_Readline).

```csharp
string[] history = new string[] { "ls -a", "dotnet run", "git init" };
ReadLine.AddHistory(history);

ReadLine.AutoCompletionHandler = (t, s) =>
{
    if (t.StartsWith("git"))
        return new string[] { "init", "clone", "pull", "push" };
    else
        return null;
};

string input = ReadLine.Read("(prompt)> ");
Console.Write(input);
```

Game of the week: 
-------------------------


User group meeting of the week: Shared Security Responsibility in the Azure Cloud in Illinois
---------------------------------------------------------------

The [Chicago Azure Cloud Users Group](https://www.meetup.com/Chicagoazure/) holds [a meeting on Wednesday, February 15 at 6:00PM in Warrenville, IL](https://www.meetup.com/Chicagoazure/events/232246970/) on the Azure Shared Security Model.

.NET
----

* [Announcing .NET Core Tools Updates in VS 2017 RC](https://blogs.msdn.microsoft.com/dotnet/2017/02/07/announcing-net-core-tools-updates-in-vs-2017-rc/) by Rich Lander.
* [Welcoming Jon Galloway as the new Executive Director of the .NET Foundation](https://dotnetfoundation.org/blog/welcoming-jon-galloway-as-the-new-executive-director-of-the-net-foundation) by Martin Woodward.
* [.NET Renaissance](https://medium.com/altdotnet/net-renaissance-32f12dd72a1) by Mark Rendle.
* [Migrating existing .NET projects to SDK-based projects](https://blogs.infosupport.com/migrating-existing-projects-to-sdk-based-projects/) by Jonathan Mezach.
* [The Performance Cost of Boxing in .NET](http://www.tomdupont.net/2016/11/the-performance-cost-of-boxing-in-net.html) by Tom DuPont.

ASP.NET
-------



F#
--



Xamarin
-------

* [Xamarin Pre-release: Xamarin.Forms 2.3.4.192-pre2](https://releases.xamarin.com/pre-release-xamarin-forms-2-3-4-192-pre2/) by David Ortinau.
* [Xamarin Release Candidate: Cycle 9 RC Refresh](https://releases.xamarin.com/release-candidate-cycle-9-rc5-refresh/) by Bri Brothers.
* [Cross-Platform Drawing with SkiaSharp](https://blog.xamarin.com/drawing-with-skiasharp/) by Matthew Leibowitz.
* [Announcing Project Rome Android SDK](https://blogs.windows.com/buildingapps/2017/02/08/announcing-project-rome-android-sdk/) by Carmen Forsmann.
* [Xamarin Forms DataTemplateSelector](https://xamarinhelp.com/xamarin-forms-datatemplateselector/) & [Xamarin Forms Toolbar](https://xamarinhelp.com/xamarin-forms-toolbar/) by Adam Pedley.
* [Keeping DRY with PropertyChanged.Fody for Xamarin.Forms](https://blog.verslu.is/xamarin/keeping-dry-with-propertychanged-fody-for-xamarin-forms/) & [Working with Effects in Xamarin.Forms](https://blog.verslu.is/xamarin/xamarin-forms-xamarin/working-with-effects-in-xamarin-forms/) by Gerald Versluis.
* [Getting Device-Specific When Customizing a Xamarin Forms App](https://visualstudiomagazine.com/articles/2017/02/01/customizing-a-xamarin-forms-app.aspx) by Wallace McClure.
* [5 Helpful Xamarin.Forms Developer Tips](http://developer.telerik.com/products/ui-for-xamarin/5-helpful-xamarin-developer-tips/) by Sam Basu.
* [Xamarin.Android - Things](http://www.jon-douglas.com/2017/02/07/xamarin-android-things/) by Jon Douglas.
* [Get familiar with Xamarin Workbooks](https://mobileprogrammerblog.wordpress.com/2017/01/14/get-familiar-with-xamarin-workbooks/), [Create UI Tests with Xamarin Test Recorder](https://mobileprogrammerblog.wordpress.com/2017/02/09/create-ui-tests-with-xamarin-test-recorder/), & [Xamarin Forms with MVVM Light](https://mobileprogrammerblog.wordpress.com/2017/01/21/xamarin-forms-with-mvvm-light/) by Daniel Krzyczkowski.
* [Developing Universal/Cross-Platform Apps with MVVM – VI](https://canbilgin.wordpress.com/2017/02/04/developing-universalcross-platform-apps-with-mvvm-vi/) by Can Bilgin.
* [The Role Of Empty States In User Onboarding](https://www.smashingmagazine.com/2017/02/user-onboarding-empty-states-mobile-apps/) by Nick Babich.
* [Problem adding Microsoft Emotion API to a Xamarin app](https://blog.jayway.com/2017/02/07/problem-adding-microsoft-emotion-api-to-a-xamarin-app/) by Anders Poulsen.
* [Build action 'EmbeddedResource'](http://davidyardy.com/archive/build-action-embeddedresource/), [Visual Studio 2017–IOS Build Debug Error](http://davidyardy.com/archive/visual-studio-2017-ios-build-debug-error/), & [Xamarin: Unable to Debug Android Application](http://davidyardy.com/archive/xamarin-unable-to-debug-android-application/) by David Yardy.
* [Announcing ReactiveUI virtual community meetups](https://ghuntley.com/archive/2017/02/07/announcing-reactiveui-virtual-community-meetups/) by Geoffrey Huntley.
* [Yet Another Podcast #167 – Charles Petzold](http://jesseliberty.com/2017/02/08/yet-another-podcast-167-charles-petzold/) by Jesse Liberty.

Azure
-----


UWP
----
* [Recap Windows Developer Day: Creators Update](https://blogs.windows.com/buildingapps/2017/02/08/windows-developer-day-creators-update/) by Kevin Gallo.
* [Telerik UI for UWP Now Open Source](http://www.telerik.com/blogs/telerik-ui-for-uwp-now-open-source) by Dobrin Grancharov.
* [Using SQLite databases in UWP apps](https://blogs.windows.com/buildingapps/2017/02/06/using-sqlite-databases-uwp-apps/) by Gautam Kanumuru.
* [Announcing UWP Community Toolkit 1.3](https://blogs.windows.com/buildingapps/2017/02/10/announcing-uwp-community-toolkit-1-3/) by  David Catuhe and Giorgio Sardo.

Games
-----


And this is it for this week!

Contribute to the week in .NET
------------------------------

As always, this weekly post couldn't exist without community contributions, and I'd like to thank all those who sent links and tips. The F# section is provided by [Phillip Carter](https://twitter.com/_cartermp), the gaming section by [Stacey Haffner](https://twitter.com/yecats131), and the Xamarin section by [Dan Rigby](https://twitter.com/DanRigby), and the UWP section by [Michael Crump](http://twitter.com/mbcrump).

You can participate too. Did you write a great blog post, or just read one? Do you want everyone to know about an amazing new contribution or a useful library? Did you make or play a great game built on .NET?
We'd love to hear from you, and feature your contributions on future posts:

* Send an email to beleroy at Microsoft,
* [comment on this gist](https://gist.github.com/bleroy/ff3368ab270efd5d29a6e024c4c59879)
* Leave us a pointer in the comments section below.
* [Send Stacey (@yecats131) tips on Twitter about .NET games](https://twitter.com/yecats131).

This week's post (and future posts) also contains news I first read on [The ASP.NET Community Standup](https://blogs.msdn.microsoft.com/webdev/tag/communitystandup/), on [Weekly Xamarin](http://weeklyxamarin.com/), on [F# weekly](https://sergeytihon.wordpress.com/category/f-weekly/), and on [Chris Alcock's The Morning Brew](http://themorningbrew.net/).
