The week in .NET - 11/30/2015
=============================

This is our first "The Week in .NET" post.
Our objective in writing this is to provide more regular updates about what the team and
core contributors have been working on, and to give a community heartbeat, in the form
of a list of interesting links.
Of course, if you wrote a great blog post, or just read one, if you want to show a great
new contribution, if you've written a useful library, we'd love to hear from you, and
feature it on future posts.
We have an email address that you can use to send us such tips: weekindotnet at Microsoft,
or you can [comment on this gist](https://gist.github.com/bleroy/e3263de7a4be4a6a1195) with new
links, or you can simply leave us a pointer in the comment section below.

.NET Core, .NET Framework 4.6.1, ASP.NET 5 RC, and Visual Studio Update 1 for .NET Managed Languages are here!
--------------------------------------------------------------------------------------------------------------

On November 18th, the team shipped .NET Core and ASP.NET 5 Release Candidates, supported
on Windows, OS X, and Linux.
The release is "Go Live", meaning you can deploy apps into production and call Microsoft
Support if you need help.

You can read the full details about this release here:
[Announcing .NET Core and ASP.NET 5 RC](http://blogs.msdn.com/b/dotnet/archive/2015/11/18/announcing-net-core-and-asp-net-5-rc.aspx).

ASP.NET has its own post that you can find here:
[Announcing ASP.NET 5 RC](http://blogs.msdn.com/b/webdev/archive/2015/11/18/announcing-asp-net-5-release-candidate-1.aspx).

The announcement happened at the [Connect(); // 2015 conference](https://channel9.msdn.com/Events/Visual-Studio/Connect-event-2015/).
Follow the link for videos.

Yesterday, November 30th, .NET Framework 4.6.1 shipped with significant improvements to
the development experience as well as greater reliability and performance of apps:
[.NET Framework 4.6.1 is now available!](http://blogs.msdn.com/b/dotnet/archive/2015/11/30/net-framework-4-6-1-is-now-available.aspx)

On the same day, Visual Studio Update 1 for .NET Managed Languages arrived with new IDE
features, interactive C#, new code analysis management, Visual F# improvements, and the
new F5 experience for Roslyn open source development:
[What's New in Visual Studio Update 1 for .NET Managed Languages](http://blogs.msdn.com/b/dotnet/archive/2015/11/30/what-s-new-in-visual-studio-update-1-for-net-managed-languages.aspx).

Package of the week: HTML Agility Pack
--------------------------------------

If you ever have to extract information from an HTML document, you could build a quick and
dirty parser, or you could use regular expressions, but the saner option is to use a proper
HTML parser, and then query the resulting DOM.
[HTML Agility Pack](https://www.nuget.org/packages/HtmlAgilityPack/)
provides such a parser and DOM for .NET.

[HtmlAgilityPack](https://www.nuget.org/packages/HtmlAgilityPack/)

Here's some code that uses `HtmlAgilityPack` to extract the URL of each image on this blog's
home page:

<script src="https://gist.github.com/bleroy/c5e8f2ebdbd694e0913f.js"></script>

User group of the week: TRINUG
------------------------------

TRINUG is a user group in Raleigh, NC, USA that meets on the second Wednesday of each month.
They are holding a hands-on lab tonight December 1st at 6PM to install Windows 10 on your
Raspberry Pi 2.

[F#/Analytics + IoT + Azure](http://www.meetup.com/TRINUG/events/225097782/)

Blog posts of the week
----------------------

[Marc Gravell](http://blog.marcgravell.com/) has a very detailed tutorial and explanation of
what it takes to port a .NET library to support .NET Core:
[The road to DNX - part 1](http://blog.marcgravell.com/2015/11/the-road-to-dnx-part-1.html),
[part 2](http://blog.marcgravell.com/2015/11/the-road-to-dnx-part-2.html), and
[part 3](http://blog.marcgravell.com/2015/11/the-road-to-dnxpart-3.html).

[The Register](http://www.theregister.co.uk/) explains
[why Microsoft's .NET Core is the future of its development platform](http://www.theregister.co.uk/2015/11/20/microsoft_net_core_development_platform_fork/).

Nate Barbettini gives us his [Ultimate Guide to Using Visual Studio on a Mac](https://stormpath.com/blog/ultimate-guide-to-using-visual-studio-on-a-mac/).

[Mahmut Jomaa](http://mjomaa.com/) has a tutorial on [how to upgrade an ASP.NET 5 application
from Beta 8 to RC1](http://mjomaa.com/computer-science/frameworks/asp-net-mvc/157-upgrading-your-asp-net-5-application-from-beta8-to-rc1), and [Shawn Wildermuth](http://wildermuth.com/) has [one as well](http://wildermuth.com/2015/11/18/Upgrading_ASP_NET_5_Beta_8_to_RC1).

[Armen Shimoon](http://dotnetliberty.com/) [annotated the diff between ASP.NET 5 Beta 8 and RC1](http://dotnetliberty.com/index.php/2015/11/23/asp-net-5-beta-8-to-rc1-annotated-diff/)

[Shane Boyer](http://tattoocoder.com/) explains [how to prepare for the .NET Core Command Line Interface](http://tattoocoder.com/preparing-for-dotnet-cli/).

And this is it for this week!