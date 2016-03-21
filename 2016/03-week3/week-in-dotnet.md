The week in .NET - 3/15/2016
============================

To read last week's post, see [The week in .NET - 3/8/2016](https://blogs.msdn.microsoft.com/dotnet/2016/03/08/the-week-in-net-382016/).

On.NET
------

Last week, we had [Nick Craver on the show to talk about StackOverflow](https://www.youtube.com/watch?v=DJn8-Psznsw). It was lots of fun, and I highly recommend watching if you're interested in getting an inside look at how StackOverflow is being maintained and kept a high-performance site. This week, I won't host the show, and instead, we'll have [Beth Massi, Maria Naggaga Nakanwagi, and Kasey Uhlenhuth](https://www.youtube.com/watch?v=hd6BQPfBzz0) take over.

Package of the week: Quartz.NET
----------------------------------

Scheduling background jobs in an application can be tricky business. [Quartz.NET](http://www.quartz-scheduler.net/) takes care of the details for you: it manages thread pools, schedules and triggers jobs, all from a simple fluent API:

```csharp
// Prepare a job
public class HelloJob : IJob
{
    public void Execute(IJobExecutionContext context)
    {
        Console.WriteLine("Greetings from HelloJob!");
    }
}

// Tie a job to the HelloJob class
IJobDetail job = JobBuilder.Create<HelloJob>()
    .WithIdentity("job1", "group1")
    .Build();

// Prepare a trigger to run the job every 10 seconds
ITrigger trigger = TriggerBuilder.Create()
    .WithIdentity("trigger1", "group1")
    .StartNow()
    .WithSimpleSchedule(x => x
        .WithIntervalInSeconds(10)
        .RepeatForever())
    .Build();

// Tell quartz to schedule the job using our trigger
scheduler.ScheduleJob(job, trigger);
```

User group of the week: Florida.net
-----------------------------------

[Florida.net](http://www.fladotnet.com/) has an [open hack night with breakout sessions on Wednesday March 3 at 7:00PM](http://www.fladotnet.com/Reg.aspx?EventID=803).

.NET
----

* [Experimental .NET Core Debugging in VS Code](https://blogs.msdn.microsoft.com/visualstudioalm/2016/03/10/experimental-net-core-debugging-in-vs-code/) by Daniel Meixner.
* [Roslyn scripting on CoreCLR (.NET CLI and DNX) and in memory assemblies](http://www.strathweb.com/2016/03/roslyn-scripting-on-coreclr-net-cli-and-dnx-and-in-memory-assemblies/) by Filip W.
* [A cross-platform interactive C# script editor](http://www.jayway.com/2016/03/09/interactive-c-script-editor-built-electron-edgejs/) by Christian Jacobsen.
* [Hamburger controls for UWP](https://www.pedrolamas.com/2016/03/07/cimbalino-toolkit-hamburger-controls-for-uwp/) by Pedro Lamas.
* [Using the Project Oxford Emotion API in C# and JavaScript](http://blogs.msdn.com/b/martinkearn/archive/2016/03/07/using-the-project-oxford-emotion-api-in-c-and-javascript.aspx) by Martin Kearn.

ASP.NET
-------

* [How to Create a Custom Action Filter in ASP.NET MVC](http://www.infragistics.com/community/blogs/dhananjay_kumar/archive/2016/03/04/how-to-create-a-custom-action-filter-in-asp-net-mvc.aspx) by Dhananjay Kumar.
* [Multi-tenant middleware pipelines in ASP.NET Core](http://benfoster.io/blog/aspnet-core-multi-tenant-middleware-pipelines) by Ben Foster.
* [What is middleware anyway?](http://aspnetmonsters.com/2016/03/2016-02-28-what-is-middleware-anyway/) by Simon Timms.
* [ASP.NET 5 on Nano Server](http://docs.asp.net/en/latest/tutorials/nano-server.html) by Sourabh Shirhatti.
* [Use IdentityServer in SwaggerUI to consume a secured ASP.Net WebAPI](http://danielwertheim.se/use-identityserver-in-swaggerui-to-consume-a-secured-asp-net-webapi/) by Daniel Wertheim.
* [Custom Validation in ASP.NET Web API with FluentValidation](http://www.exceptionnotfound.net/custom-validation-in-asp-net-web-api-with-fluentvalidation/) by Matthew Jones.
* [Understanding ASP.NET Performance for Reading Incoming Data](http://stackify.com/understanding-asp-net-performance-for-reading-incoming-data/) by Matt Watson.

F#
--

* [F# with .NET Core and CLI](https://www.youtube.com/watch?v=_0Q-Q2UeyP0), by Enrico Sada
* [Path to F# and NGO Fraudbuster](https://www.youtube.com/watch?v=nZwQ9JVl-d8&feature=youtu.be), by Jacqueline Homan
* [On.NET: F# at Jet.com](https://blogs.msdn.microsoft.com/dotnet/2016/03/08/on-net-332016-rachel-reese-on-f-at-jet-com/), by Rachel Reese
* [Converting a DSL to Executable F# Code On-the-Fly, Part 2](http://brandewinder.com/2016/03/06/converting-dsl-to-fsharp-code-part-2/), by Mathias Brandewinder
* [GPUs and Domain Specific Languages for Life Insurance Modeling](http://blog.quantalea.com/?p=9321), by Daniel Egloff

Check out [F# Weekly](https://sergeytihon.wordpress.com/category/f-weekly/) for more great content from the F# community.

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
* [comment on this gist](https://gist.github.com/bleroy/ef4ea653fe7d56813d1a)
* Leave us a pointer in the comments section below.
* [Send Stacey (@yecats131) tips on Twitter about .NET games](https://twitter.com/yecats131).

This week's post (and future posts) also contains news I first read on
[ASP.NET's community spotlight](http://www.asp.net/),
on [F# weekly](https://sergeytihon.wordpress.com/category/f-weekly/),
on [ASP.NET Weekly](http://www.aspnetweekly.com/),
and on [Chris Alcock's The Morning Brew](http://themorningbrew.net/).
