The week in .NET - xx/xx/xx
============================

To read last week's post, see [The week in .NET – On .NET on Net Standard 2.0 – Nancy – Satellite Reign](https://blogs.msdn.microsoft.com/dotnet/2016/10/11/the-week-in-net-on-net-on-net-standard-2-0-nancy-satellite-reign/).

On .NET
-------

We didn't have a show last week, but we're back this week with Rowan Miller to chat about [Entity Framework Core 1.1](https://blogs.msdn.microsoft.com/dotnet/2016/07/29/entity-framework-core-1-1-plans/) and .NET. The show is on Thursdays and begins at 10AM Pacific Time [on Channel 9](https://channel9.msdn.com/Shows/On-NET). We'll take questions on Gitter, on [the dotnet/home channel](https://gitter.im/dotnet/home) and on Twitter. Please use the `#onnet` tag. It's OK to start sending us questions in advance if you can't do it live during the show.

Package of the week: Bond
-------------------------

[Bond](https://microsoft.github.io/bond/manual/bond_cs.html) is a battle-tested binary serialization format and library, similar to [Google's Protocol Buffer](https://developers.google.com/protocol-buffers/docs/csharptutorial). Bond works on Linux, OSX, and Windows, and supports C++, C#, and Python.

To work with Bond, you start by defining your schema using an [IDL](http://www.ibm.com/developerworks/webservices/library/co-corbajct3.html)-like specification.

```csharp
namespace Examples

struct Record
{
    0: string Name;
    1: vector<double> Constants;
}
```

Then, you codegen a C# library for the schema:

```
gbc c# example.bond
```

You may now use the generated library in your C# code to instantiate objects of the types defined, as well as serialize and deserialize them:

```csharp
var src = new Record
{
    Name = "FooBar",
    Constants = { 3.14, 6.28 }
};

var output = new OutputBuffer();
var writer = new CompactBinaryWriter<OutputBuffer>(output);

Serialize.To(writer, src);

var input = new InputBuffer(output.Data);
var reader = new CompactBinaryReader<InputBuffer>(input);

var dst = Deserialize<Record>.From(reader);
```

Bond also offers deep cloning and comparison for objects of compatible types defined from Bond specifications.

Conference of the week: .NET DeveloperDays October 20-21 in Warsaw
------------------------------------------------------------------

[.NET DeveloperDays](http://net.developerdays.pl/) is the biggest event in Central and Eastern Europe dedicated exclusively to application development on the .NET platform. It is designed for architects, developers, testers and project managers using .NET in their work and to those who want to improve their knowledge and skills in this field. The conference content is 100% English, making it easy for the international audience to attend. The speaker lineup includes Jon Skeet, Dino Esposito, and Ted Neward.

.NET
----

* [.NET Framework Monthly Rollups Explained](https://blogs.msdn.microsoft.com/dotnet/2016/10/11/net-framework-monthly-rollups-explained/) and [.NET Framework Monthly Rollup: October 2016](https://blogs.msdn.microsoft.com/dotnet/2016/10/11/net-framework-monthly-rollup-october-2016/) by Stacey Haffner.
* [Learning .NET Core by example](https://jonhilton.net/2016/10/12/learning-dotnet-core-by-example/) by Jon Hilton.
* [Parallel Test Execution](https://blogs.msdn.microsoft.com/visualstudioalm/2016/10/10/parallel-test-execution/) by Pratap Lakshman.
* [Porting Apache Avro into .NET Core](https://weltam.wordpress.com/2016/09/07/porting-apache-avro-into-net-core/) by Welly Tambunan.
* [Timing the time it takes to parse time part 1](https://ayende.com/blog/175713/timing-the-time-it-takes-to-parse-time-part-i) and [part 2](https://ayende.com/blog/175714/timing-the-time-it-takes-to-parse-time-part-ii?Key=f65c7092-3f17-4ded-bb30-9dd479848d5c) by Ayende Rahien.
* [Microsoft IIS Administration Preview](https://blogs.iis.net/adminapi/microsoft-iis-administration-api-preview).
* [Deal with time dependencies in Tests](https://carlos.mendible.com/2016/10/11/deal-with-time-dependencies-in-tests/) by Carlos Mendible.
* [Metric.NET: dealing with measurement units](https://github.com/milutinovici/metric) by Ivan Milutinović.
* [Stuntman now works with .NET Core](https://github.com/ritterim/stuntman).
* [MediatR Pipeline Examples](https://lostechies.com/jimmybogard/2016/10/13/mediatr-pipeline-examples/) by Jimmy Bogard.
* [Using Core class library from .NET 4.6 project](http://hudosvibe.net/post/using-core-class-library-from-.net-4.6-project) by Hrvoje Hudoletnjak.
* [Calling UWP APIs from a Desktop Application](https://mtaulty.com/2016/10/11/calling-uwp-apis-from-a-desktop-application/) by Mike Taulty.

ASP.NET
-------

* [Exploring ASP.NET Core with Docker in both Linux and Windows Containers](http://www.hanselman.com/blog/ExploringASPNETCoreWithDockerInBothLinuxAndWindowsContainers.aspx) by Scott Hanselman.
* [Custom authorization policies and requirements in ASP.NET Core](http://andrewlock.net/custom-authorisation-policies-and-requirements-in-asp-net-core/) by Andrew Lock.
* [Debugging into ASP.NET Core Source](https://www.stevejgordon.co.uk/debugging-into-asp-net-core-source) by Steve Gordon.
* [Localization & Routing in ASP.NET Core 1.0](http://en.xn--mgbz4cf.com/post/localization--routing-in-aspnet-core-10) by Hisham Bin Ateya.
* [ASP.NET Core and the Enterprise: Part 1 Frameworks](http://odetocode.com/blogs/scott/archive/2016/10/11/asp-net-core-and-the-enterprise-part-1ndashframeworks.aspx) by K. Scott Allen.

F#
--

* [Using F# on Azure](https://docs.microsoft.com/en-us/dotnet/articles/fsharp/using-fsharp-on-azure/) by Sylvan Clebsch.
* [Getting Started with F# in Visual Studio Code with Ionide](https://docs.microsoft.com/en-us/dotnet/articles/fsharp/tutorials/getting-started/getting-started-vscode) by Phillip Carter.
* [Using F# with .NET Core](https://medium.com/real-world-fsharp/using-f-with-net-core-aa6cfc9ef547#.sqxgfl6qs) by Ody Mbegbu.
* [ASP.NET Identity in an F# Web Application, Part One](https://csharptofsharp.wordpress.com/2016/10/13/asp-net-identity-in-an-f-web-application-part-one/) by Ernest Pazera.
* [Prefer Records of Functions to Interfaces](https://medium.com/@dogwith1eye/prefer-records-of-functions-to-interfaces-d6413af4d2c3#.fbxgxayqt) by Matthew Doig.

Check out [F# Weekly](https://sergeytihon.wordpress.com/category/f-weekly/) for more great content from the F# community.

Xamarin
-------

* [Xamarin Preview: Xamarin Profiler 0.38.0](https://releases.xamarin.com/preview-xamarin-profiler-0-38-0/) by Adrian Murphy.
* [Xamarin Dev Days Continue to Roll Out!](https://blog.xamarin.com/xamarin-dev-days-continue-to-roll-out/) by Jayme Singleton.
* [Mobile Leaders Podcast | Enterprise Mobility Trends with Maribel Lopez](https://blog.xamarin.com/mobile-leaders-podcast-enterprise-mobility-trends-with-maribel-lopez-where-are-things-headed/) by Anusha Sethuraman.
* [Live Webinar | Xamarin University Presents: Introduction to Xamarin with Visual Studio](https://blog.xamarin.com/live-webinar-xamarin-university-presents-introduction-to-xamarin-with-visual-studio/) by Courtney Witmer.
* [Understanding Android’s Doze Functionality](https://blog.xamarin.com/understanding-androids-doze-functionality/) and [The Xamarin Show 6: User Interface Automation with Charles Wang](https://channel9.msdn.com/Shows/XamarinShow/User-Interface-Automation-with-Charles-Wang) by James Montemagno.
* [Bringing Platform-Specific Functionality to Xamarin.Forms Apps](https://blog.xamarin.com/bringing-platform-specific-functionality-to-xamarin-forms-apps/) by Pierce Boggan.
* [Designing Card-Based User Interfaces](https://www.smashingmagazine.com/2016/10/designing-card-based-user-interfaces/) by Nick Babich.
* [Creating A Splash Screen In Xamarin Forms](https://xamarinhelp.com/creating-splash-screen-xamarin-forms/) and [Misuses Of MessagingCenter](https://xamarinhelp.com/common-misuse-messagingcenter/) by Adam Pedley.
* [ReactiveUI Goodies – Search](https://janhannemann.wordpress.com/2016/10/13/reactiveui-goodies-search/) and [ReactiveUI Goodies – ReactiveList](https://janhannemann.wordpress.com/2016/10/06/reactiveui-goodies-reactivelist/) by Jan Hannemann.
* [Semantic Versioning of Xamarin Applications](https://ghuntley.com/archive/2016/10/11/semantic-versioning-of-xamarin-applications/) by Geoffrey Huntley.
* [Templates and Extensions for Xamarin & Mono](https://visualstudiomagazine.com/articles/2016/10/01/templates-and-extensions.aspx) by Terrence Dorsey.

Azure
-----

* [Simpler Azure Management Libraries for .NET](https://azure.microsoft.com/en-us/blog/simpler-azure-management-libraries-for-net/) by Asir Selvasingh.
* [Hosting .NET Core Services on Service Fabric](https://blogs.msdn.microsoft.com/dotnet/2016/10/13/hosting-net-core-services-on-service-fabric/) by Vaijanath Angadihiremath.
* [Running Worker Roles with Docker in .NET Core](https://medium.com/@duizendnegen/running-worker-roles-with-docker-in-net-core-543b8d1c4ae7) by Pepijn Schoen.
* [ASP.NET Core 1.0 - Configure Application Insights](http://www.codeproject.com/Tips/1139662/ASP-NET-Core-Configure-Application-Insights) by Joao Sousa.
* [App Service Mobile Apps .NET Client SDK 3.0.1 release](https://azure.microsoft.com/en-us/blog/app-service-mobile-apps-net-client-sdk-3-0-1-release/) by Mimi Xu.
* [Sending a Regular SMS with Azure Functions and Twilio](http://dontcodetired.com/blog/post/Sending-a-Regular-SMS-with-Azure-Functions-and-Twilio) by Jason Roberts.

And this is it for this week!

Contribute to the week in .NET
------------------------------

As always, this weekly post couldn't exist without community contributions, and I'd like to thank all those who sent links and tips. The F# section is provided by [Phillip Carter](https://twitter.com/_cartermp), the gaming section by [Stacey Haffner](https://twitter.com/yecats131), and the Xamarin section by [Dan Rigby](http://DanRigby.com).

You can participate too. Did you write a great blog post, or just read one? Do you want everyone to know about an amazing new contribution or a useful library? Did you make or play a great game built on .NET?
We'd love to hear from you, and feature your contributions on future posts:

* Send an email to beleroy at Microsoft,
* [comment on this gist](xx)
* Leave us a pointer in the comments section below.
* [Send Stacey (@yecats131) tips on Twitter about .NET games](https://twitter.com/yecats131).

This week's post (and future posts) also contains news I first read on [The ASP.NET Community Standup](https://blogs.msdn.microsoft.com/webdev/tag/communitystandup/), on [Weekly Xamarin](http://weeklyxamarin.com/), on [F# weekly](https://sergeytihon.wordpress.com/category/f-weekly/), and on [Chris Alcock's The Morning Brew](http://themorningbrew.net/).
