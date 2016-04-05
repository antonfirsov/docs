The week in .NET - 4/5/2016
===========================

To read last week's post, see [The week in .NET – 3/29/2016](https://blogs.msdn.microsoft.com/dotnet/2016/03/29/the-week-in-net-3292016/).

On.NET
------

Last week was a thrilling one for the .NET community, with lots of exciting announcements made at [the Build conference](https://channel9.msdn.com/Events/Build/2016). We decided to move the show to avoid being in direct competition with Build's live stream. On Monday, [we recorded a show with John Kemnetz on the new experimental support for C# debugging in VS Code](https://www.youtube.com/watch?v=OjNbBOjcLRk). We tried something a little different this time: instead of our usual one hour discussion, we did a short 20 minute live screencast. Let us know what you think in the comments. This week, [Beth Massi and Maria Naggaga are taking over the show](https://www.youtube.com/watch?v=hd6BQPfBzz0).

Package of the week: Autofac
----------------------------

With dependency injection baked right into ASP.NET Core, it's more important than ever to master this technique that can reduce coupling in your applications. ASP.NET provides a minimal container, but is also open for optional replacement containers. [Autofac](https://github.com/autofac/Autofac) is a popular [IoC](https://en.wikipedia.org/wiki/Inversion_of_control) container that is [now compatible with .NET Core](http://autofac.readthedocs.org/en/latest/integration/aspnetcore.html).

The following code shows how you'd register an implementation of an `IOutput` interface from the initialization phase of your application, so that it can later be resolved from other components.

```csharp
public IServiceProvider ConfigureServices(IServiceCollection services)
{
    services.AddMvc();

    var builder = new ContainerBuilder();

    builder.RegisterType<ConsoleOutput>().As<IOutput>();
    builder.Populate(services);
    var container = builder.Build();

    return container.Resolve<IServiceProvider>();
}
```

You can then inject an `IOutput` into your controllers and services:

```csharp
public class MyController : Controller
{
    private IOutput _output;
    
    public MyController(IOutput output)
    {
        _output = output;
    }
    
    public IActionResult DoSomething()
    {
        _output.Write("I did something.");
        return View();
    }
}
```

This is a simple use case for Autofac, but check-out [the documentation](http://autofac.readthedocs.org/en/latest/index.html) for more elaborate examples. We also have quite a few dependency injection articles this week; check-out the ASP.NET section.

Interestingly, Autofac is the IoC container used in the [Orchard CMS](*http://orchardproject.net/).

Wellington, New Zealand Code Camp
---------------------------------

On Saturday April 9th 2016 come and join with some of New Zealand's top technical speakers for [a free one day conference](http://www.nichesoftware.co.nz/codecamp.html).

.NET
----

* [Xamarin for Everyone](https://blog.xamarin.com/xamarin-for-all/) by Nat Friedman.
* [Announcing the .NET Framework 4.6.2 Preview](https://blogs.msdn.microsoft.com/dotnet/2016/03/30/announcing-the-net-framework-4-6-2-preview/) by Stacey Haffner.
* [.NET at Build 2016 – Open, Cross-platform and FREE](https://blogs.msdn.microsoft.com/dotnet/2016/04/01/net-at-build-2016-open-cross-platform-and-free/) by Rich Lander.
* [What’s New for C# and VB in Visual Studio](https://blogs.msdn.microsoft.com/dotnet/2016/04/02/whats-new-for-c-and-vb-in-visual-studio/) by Kasey Uhlenhuth.
* [Mobile App Development made easy with Visual Studio and Xamarin](https://blogs.msdn.microsoft.com/visualstudio/2016/03/31/mobile-app-development-made-easy-with-visual-studio-and-xamarin/) by John Montgomery.
* [Mono Relicensed MIT](http://www.mono-project.com/news/2016/03/31/mono-relicensed-mit/) by Miguel de Icaza.
* [A Vision For Visual Studio 2015: Take on Dependencies; stay Productive](https://blogs.msdn.microsoft.com/visualstudio/2016/04/01/visual-studio-2015-take-on-dependencies-stay-productive/) by Michael C. Fanning and Joe Morris.
* [Faster, Leaner, Focused on Your Development Needs: The New Visual Studio Installer](https://blogs.msdn.microsoft.com/visualstudio/2016/04/01/faster-leaner-visual-studio-installer/) by Tim Sneath.
* [IConfiguration in .NetCore](http://www.ryansouthgate.com/2016/03/23/iconfiguration-in-netcore/) by Ryan Southgate.
* [Refactoring Essentials 4.0 Comes With Roslyn Code Converter](http://community.sharpdevelop.net/blogs/christophwille/archive/2016/04/01/refactoring-essentials-4-0-comes-with-roslyn-code-converter.aspx) by Christoph Wille.
* [Visualization and comparison of sorting algorithms in C#](http://www.codeproject.com/Articles/1087568/Visualization-and-Comparison-of-sorting-algorith) by Mark Monnin.
* [Reduce Coupling: free your code and your tests](http://jonhilton.net/2016/03/29/coupling-tests-production/) by Jon Hilton.

ASP.NET
-------

* [Dependency Injection in ASP.NET Core](https://blogs.msdn.microsoft.com/webdev/2016/03/28/dependency-injection-in-asp-net-core/) by Jeffrey T. Fritz.
* [Setting Up Dependency Injection in Web API with StructureMap](http://www.exceptionnotfound.net/setting-up-dependency-injection-in-web-api-with-structuremap/) by Matthew Jones.
* [The Subtle Perils of Controller Dependency Injection in ASP.NET Core MVC](http://www.strathweb.com/2016/03/the-subtle-perils-of-controller-dependency-injection-in-asp-net-core-mvc/) by Filip W.
* [Docker and ASP.NET Core (video)](http://wildermuth.com/2016/03/28/Docker_and_ASP_NET_Core_A_Webcast) by Shawn Wildermuth.
* [ASP.NET Core Password Options and Custom Validators](http://www.elanderson.net/2016/03/asp-net-core-password-options-and-custom-validators/) by Eric L. Anderson.
* [Building Advanced Tag Helpers (video)](http://aspnetmonsters.com/2016/03/monsters-weekly%5Cep19/) by the ASP.NET Monsters.

F#
--

* Try F# on .NET Core!
  * Watch [Getting Started with F# on .NET Core](https://channel9.msdn.com/Events/Build/2016/T661), by David Stephens.
  * Read [this Getting Started guide on Github](https://github.com/enricosada/fsharp-dotnet-cli-samples/wiki/Getting-Started) for more details.
* [Beyond Lists](http://www.infoq.com/presentations/data-structure-lists), by Phil Trelford.
* [Functional Architecture](https://vimeo.com/161131920), by Mark Seemann.
* [Microservices Chaos Testing at Jet](http://www.infoq.com/presentations/jet-microservices-testing), by Rachel Reese.

Check out [F# Weekly](https://sergeytihon.wordpress.com/category/f-weekly/) for more great content from the F# community.
Games
---

* [Unity Joins the .NET Foundation](http://blogs.unity3d.com/2016/04/01/unity-joins-the-net-foundation/) - by Jonathan Chambers.
* [Visual Studio Tools for Unity (Video)](https://channel9.msdn.com/Shows/Visual-Studio-Toolbox/Visual-Studio-Tools-for-Unity) - Jb Evain and Robert Green.

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
* [comment on this gist](xx)
* Leave us a pointer in the comments section below.
* [Send Stacey (@yecats131) tips on Twitter about .NET games](https://twitter.com/yecats131).

This week's post (and future posts) also contains news I first read on
[ASP.NET's community spotlight](http://www.asp.net/),
on [F# weekly](https://sergeytihon.wordpress.com/category/f-weekly/),
on [ASP.NET Weekly](http://www.aspnetweekly.com/),
and on [Chris Alcock's The Morning Brew](http://themorningbrew.net/).
