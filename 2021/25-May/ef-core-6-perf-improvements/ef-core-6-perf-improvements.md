---
post_title: 'Announcing Entity Framework Core 6.0 Preview 4: Performance Edition'
username: shrojans
microsoft_alias: shrojans
categories: .NET Core, Entity Framework, ASP.NET
summary: Learn about some serious optimizations that have gone into Entity Framework Core 6.0, and how much faster it now performs.
desired_publication_date: '2021-05-25'
---
Today, the Entity Framework Core team announces the fourth preview release of EF Core 6.0. The main theme of this release is performance - and we'll concentrate on that below - details on getting EF Core 6.0 preview 4 are at the end of this blog post.

The short and sweet summary:

* EF Core 6.0 performance is now **70% faster** on the industry-standard TechEmpower Fortunes benchmark, compared to 5.0.
* This is the full-stack perf improvement, including improvements in the benchmark code, the .NET runtime, etc. EF Core 6.0 itself is **31% faster** executing queries.
* Heap allocations have been reduced by **43%**.

## The runtime perf push

When the EF Core team started the planning process for version 6.0, we knew it was finally time to address an important area. After several years spent delivering new EF Core features, stabilizing the product and progressively narrowing the feature gap with the old Entity Framework, we wanted to put an emphasis on performance and to see exactly where we could go. In previous work iterations, a lot of attention was given to the lower layers of the stack: on the industry-standard [TechEmpower Fortunes benchmark](https://www.techempower.com/benchmarks/#section=data-r20), .NET already scored very high, reaching 12th place overall (running on Linux against the PostgreSQL database). But while performance was always in our minds while working on EF Core, we hadn't done a proper optimization push at that level of the stack. 

When working on perf, it's usually a good idea to have a target you can work to, even if it's a somewhat arbitrary one. For 6.0, the goal we set ourselves was to get as close as possible to the performance of [Dapper](https://github.com/DapperLib/Dapper) on the Fortunes benchmark. For those unfamiliar with it, Dapper is a popular, lightweight, performance-oriented .NET object mapper maintained (and used) by the folks over at Stack Overflow; it requires you to write your own SQL and doesn't have many of the features of EF Core - it is sometimes referred to as a "micro-ORM" - but is an extremely useful data access tool. The EF Core team doesn't view Dapper as a competitor - we love it, and don't think EF Core should be the answer to every .NET data need out there. And being lightweight and performance-oriented as it is, Dapper provided us with inspiration and a number to strive to reach. 

At the end of this iteration, the gap between Dapper and EF Core in the TechEmpower Fortunes benchmark narrowed from 55% to around a little under 5%. We hope this shows that EF Core can be a good option for performance-aware applications, and that ORMs and data layers aren't necessarily "inefficient beasts" which should be avoided. It's worth mentioning that the benchmark executes a LINQ query - not raw SQL - so many of the benefits of EF Core are being preserved (e.g. statically-typed queries!) while sacrificing very little perf.

A final, general note on performance. The numbers and improvements reported in this article are for a very specific scenario (TechEmpower Fortunes), using non-tracking queries only (no change tracking, no updates); the benchmarks were executed on a high-performance, low-latency setup. Real-world application scenarios will most probably show very different results, as the runtime overhead of executing queries would be dominated by network or database I/O times. In fact, we believe that for many real-world applications, the runtime overhead added by something like EF Core is likely to play a very minor role next to other, more important factors influencing perf. Keep this in mind when thinking about your data access

With that out of the way, let's dive into some of the optimizations done for EF Core 6.0. For those interested, [the full list of improvements done in this optimization round is available](https://github.com/dotnet/efcore/issues/23611#issuecomment-778191277), along with detailed measurements.

### Pooling and recycling, DbContext and beyond

Recycling and pooling are central to good performance: they reduce the work needed to create and dispose resources, and typically lower heap allocations as well, which reduces pressure on the garbage collector. All EF Core users are familiar with the DbContext class - this is the main entry point for performing most operations; you can instantiate one (or get one from dependency injection), use it to perform a few database operations ("unit of work"), and then dispose it. While instantiating new DbContexts is fine in a typical application, the overhead of doing so in high-performance scenarios can be significant: DbContext works with a whole set of internal services - via an internal dependency injection mechanism - which coordinate together to make everything work; setting all that up takes time.

For this reason, EF Core has supported [DbContext pooling](https://docs.microsoft.com/ef/core/performance/advanced-performance-topics?tabs=with-constant#dbcontext-pooling) for quite a while. The idea is simple: when you're done with a DbContext, rather than disposing it (and all its dependent services), EF Core resets its state and then allows it to be reused later. And of course, our benchmark implementation for TechEmpower Fortunes already had this feature turned on; so... why was my profiler showing me considerable time spent creating and wiring together new DbContext instances?

In almost all cases, you want to place an upper bound on the number of instances you pool. An unbounded pool can suddenly fill up with a huge number of objects, which can take up considerably resources (memory or otherwise) and which may stick around indefinitely, depending on your pruning strategy. In the EF Core case, the default upper bound for pooled DbContext instances was 128 - going beyond that number meant falling back to instantiation and disposal. Now, while 128 should be fine for most applications - it's not common to have 128 contexts active simultaneously - it definitely wasn't enough for TechEmpower Fortunes; and hiking that number up to 1024 yielded **a 23% improvement** in benchmarkthroughput. This, of course, isn't an improvement in EF Core itself, but it did lead us to increase the default, and we will probably start emitting a warning if the upper bound is surpassed. Finally, since DbContext pooling proved to be so important in this case, we made the feature accessible to applications not using dependency injection as well. I think this is a nice example of where even a minor benchmark misconfiguration can feed into useful product improvements.

But DbContext isn't everything. When executing a query, EF Core makes use of various objects, including ADO.NET objects such as [`DbConnection`](https://docs.microsoft.com/dotnet/api/system.data.common.dbconnection), [`DbCommand`](https://docs.microsoft.com/dotnet/api/system.data.common.dbcommand) and [`DbDatareader`](https://docs.microsoft.com/dotnet/api/system.data.common.dbdatareader), and various internal objects for these. When all these instances showed high up in memory profiling, more recycling was clearly in order! As a result, each DbContext now has its own, dedicated set of instances of all these, which it reuses every time. This reusable graph of objects - rooted at the DbContext pool - extends all the way down into the PostgreSQL database provider (Npgsql), a good demonstration of an optimization that reaches across the layers of the stack. This change alone reduces the total bytes allocated for query execution by 22%.

### Logging suppression

EF Core includes a lot of extension points, which allows users to get information about - and hook into - various stages of query execution. For example, to execute the SQL query against a relational database, EF Core calls [DbCommand.ExecuteReaderAsync](https://docs.microsoft.com/dotnet/api/system.data.common.dbcommand.executereaderasync); it can log an event both before and after this call (allows users to see SQL statements before they get executed, and with their running times afterwards), write a [DiagnosticSource](https://docs.microsoft.com/dotnet/api/system.diagnostics.diagnosticsource) event, and call into a user-configured [command interceptor](https://docs.microsoft.com/ef/core/logging-events-diagnostics/interceptors#database-interception) which allows the user to manipulate the command before it gets executed. While this provides a powerful and flexible set of extension points, this doesn't come cheap: a single query execution has 7 events, each with 2 extension points (one before, one after). The cost of continuously checking whether logging is enabled or whether a DiagnosticListener is registered started showing up in profiling sessions!

One initial idea we considered was a global flag to disable all logging; this would be the simplest solution and would also provide the best performance possible. However, this approach had two drawbacks:

1. It would be a sort of high-perf opt-in: it needs to be discovered and turned on. Wherever possible, we prefer to improve EF Core for everyone - out of the box.
2. It would be all or nothing. If you, say, just want to get SQL statements logged, you can't do that without paying the price for all the other extension points as well.

The solution we ended up implementing was to check whether any sort of logging or interception is enabled, and if not, suppress logging for that event for 1 second by default. This improved benchmark throughput by around 7% - very close to the global flag solution - while at the same time bringing the perf benefit to all EF Core users, without an opt-in. If, say, a DiagnosticListener is registered at some point during program execution, it may take up to a second for events to start appearing there; that seemed like a very reasonable trade-off for the speed-up.

### Opting out of thread-safety checks

While logging suppression offered an internal optimization that's transparent to users, our third case was different.

As hopefully everyone knows, EF Core's DbContext isn't thread-safe; for one thing, it encapsulates a database connection, which itself almost never allows concurrent usage. Now, although concurrent access of a DbContext instance is a programmer bug, EF Core includes an internal thread safety mechanism, which tries to detect when this happens, and throws an informative exception. This goes a long way to help EF Core users find accidental bugs, and also to make new users aware that DbContext isn't thread-safe. This mechanism works on a best-effort basis - we made no attempt to make it detect *all* possible concurrency violations, since that would probably hurt performance in a significant way.

Now, the thread safety check mechanism itself didn't show up as significant when profiling - something else did. To support some query scenarios, this check needs to be reentrant: it's OK to start a 2nd query, as long as it's part of the 1st query. And since EF Core supports asynchronous query execution, an [`AsyncLocal`](https://docs.microsoft.com/dotnet/api/system.threading.asynclocal-1) is used to flow the locking state across the threads that participate in the query. It turned out that using this AsyncLocal caused quite a few heap allocations to occur, and reduced benchmark throughput in a considerable way.

After some discussion, we decided to introduce an opt-out flag from thread-safety checks. Unlike with logging, there is no way for EF Core to know when the checks are needed, and when they aren't; and we definitely want to prioritize reliability and easier debugging, so turning the check off by default was out of the question. Once users have tested that their application works well in production and they are confident that no concurrency bugs exist, they can choose to disable this particular protection; for our TechEmpower Fortunes benchmark, doing so yielded a 6.7% throughput improvement.

## Closing words

None of the above is a dramatic change, or a fundamental re-designing of EF Core's internal architecture. Fortunately, the EF Core query pipeline was already conceived with perf in mind: after a first expensive "compilation" when a query is first seen, EF Core caches both the query's SQL and a code-generated materializer, which is the piece of code responsible for reading results from the database and instantiating your objects from them. This means that once an application reaches steady state, the heavy lifting has already been done, and EF Core has very little work left to do; and that's how EF Core is able to perform well.

I hope the above has been an interesting read into the optimizations that have gone into EF Core 6.0, and has provided a glimpse into the internals; [the full list of optimizations is available for those who want to dive deeper](https://github.com/dotnet/efcore/issues/23611#issuecomment-778191277). To make your EF Core application perform better, please take a look at our [performance docs](https://docs.microsoft.com/ef/core/performance), including [this new guidance](https://docs.microsoft.com/ef/core/performance/advanced-performance-topics#reducing-runtime-overhead) for high-perf scenarios based on this optimization effort.

What's next? Well, performance work is never done. In addition to the above, EF Core 6.0 will also deliver other types of performance improvements, including various SQL generation improvements and [optimized models](https://github.com/dotnet/efcore/issues/1906), which should improve startup times for applications with lots of entities. We also have plans for continued future improvements, especially in areas of EF Core which weren't covered in this optimization cycle (e.g. the update pipeline, change tracking).

Finally, I'd like to thank Sébastien Ros and [the Crank performance infrastructure](https://github.com/dotnet/crank), without which this optimization work wouldn't have been possible, and the EF Core team for their patience with me making their code more convoluted.

## How to get EF Core 6.0 previews

EF Core is distributed exclusively as a set of NuGet packages. For example, to add the SQL Server provider to your project, you can use the following command using the dotnet tool:

```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 6.0.0-preview.4.?????.?
```

This following table links to the preview 4 versions of the EF Core packages and describes what they are used for.

|**Package**    |**Purpose**      |
|--------------:|:----------------|
|[Microsoft.EntityFrameworkCore](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/6.0.0-preview.4.?????.?)|The main EF Core package that is independent of specific database providers|
|[Microsoft.EntityFrameworkCore.SqlServer](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer/6.0.0-preview.4.?????.?)|Database provider for Microsoft SQL Server and SQL Azure|
|[Microsoft.EntityFrameworkCore.SqlServer.NetTopologySuite](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer.NetTopologySuite/6.0.0-preview.4.?????.?)|SQL Server support for spatial types|
|[Microsoft.EntityFrameworkCore.Sqlite](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite/6.0.0-preview.4.?????.?)|Database provider for SQLite that includes the native binary for the database engine|
|[Microsoft.EntityFrameworkCore.Sqlite.Core](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite.Core/6.0.0-preview.4.?????.?)|Database provider for SQLite _without_ a packaged native binary|
|[Microsoft.EntityFrameworkCore.Sqlite.NetTopologySuite](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite.NetTopologySuite/6.0.0-preview.4.?????.?)|SQLite support for spatial types|
|[Microsoft.EntityFrameworkCore.Cosmos](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Cosmos/6.0.0-preview.4.?????.?)|Database provider for Azure Cosmos DB|
|[Microsoft.EntityFrameworkCore.InMemory](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.InMemory/6.0.0-preview.4.?????.?)|The in-memory database provider|
|[Microsoft.EntityFrameworkCore.Tools](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Tools/6.0.0-preview.4.?????.?)|EF Core PowerShell commands for the Visual Studio Package Manager Console; use this to integrate tools like [scaffolding](https://docs.microsoft.com/ef/core/managing-schemas/scaffolding) and [migrations](https://docs.microsoft.com/ef/core/managing-schemas/migrations/) with Visual Studio|
|[Microsoft.EntityFrameworkCore.Design](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Design/6.0.0-preview.4.?????.?)|Shared design-time components for EF Core tools|
|[Microsoft.EntityFrameworkCore.Proxies](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Proxies/6.0.0-preview.4.?????.?)|Lazy-loading and change-tracking proxies|
|[Microsoft.EntityFrameworkCore.Abstractions](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Abstractions/6.0.0-preview.4.?????.?)|Decoupled EF Core abstractions; use this for features like extended data annotations defined by EF Core|
|[Microsoft.EntityFrameworkCore.Relational](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Relational/6.0.0-preview.4.?????.?)|Shared EF Core components for relational database providers|
|[Microsoft.EntityFrameworkCore.Analyzers](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Analyzers/6.0.0-preview.4.?????.?)|C# analyzers for EF Core|

We also published the 6.0 preview 4 release of the [Microsoft.Data.Sqlite.Core](https://www.nuget.org/packages/Microsoft.Data.Sqlite.Core/6.0.0-preview.4.?????.?) provider for [ADO.NET](https://docs.microsoft.com/dotnet/framework/data/adonet/ado-net-overview).

## Thank you from the team

A big thank you from the EF team to everyone who has used EF over the years!

<table>

<tr>
<td><a href="https://github.com/ajcvickers"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/efteam_ajcvickers.jpeg" alt="ajcvickers" width=200px><br>Arthur Vickers</a></td>
<td><a href="https://github.com/AndriySvyryd"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/efteam_AndriySvyryd.jpeg" alt="AndriySvyryd" width=200px><br>Andriy Svyryd</a></td>
<td><a href="https://github.com/bricelam"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/efteam_bricelam.jpeg" alt="" width=200px><br>Brice Lambson</a></td>
<td><a href="https://github.com/JeremyLikness"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/efteam_JeremyLikness.jpeg" alt="JeremyLikness" width=200px><br>Jeremy Likness</a>
</td>
</tr>

<tr>
<td><a href="https://github.com/maumar"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/efteam_maumar.jpeg" alt="maumar" width=200px><br>Maurycy Markowski</a></td>
<td><a href="https://github.com/roji"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/efteam_roji-1-300x300.png" alt="roji" width=200px><br>Shay Rojansky</a></td>
<td><a href="https://github.com/smitpatel"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/efteam_smitpatel.png" alt="smitpatel" width=200px><br>Smit Patel</a>
</td><td></td>
</tr>

</table>

## Thank you to our contributors!

We are grateful to our amazing community of contributors. Our success is founded upon the shoulders of your efforts and feedback. If you are interested in contributing but not sure how or would like help, please reach out to us! We want to help you succeed. We would like to publicly acknowledge and thank these contributors for investing in the success of EF Core 6.0.

| | | | |
|:--:|:--:|:--:|:--:|
|**[AkinSabriCam](https://github.com/AkinSabriCam)**|**[alexernest](https://github.com/alexernest)**|**[alexpotter10](https://github.com/alexpotter10)**|**[Ali-YousefiTelori](https://github.com/Ali-YousefiTelori)**|
|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3157)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3225)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3143)|[#1](https://github.com/dotnet/efcore/pull/23946), [#2](https://github.com/dotnet/efcore/pull/23946)|
| | | | |
**[alireza-rezaee](https://github.com/alireza-rezaee)**|**[andrejs86](https://github.com/andrejs86)**|**[AndrewKitu](https://github.com/AndrewKitu)**|**[ardalis](https://github.com/ardalis)**|
[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3232)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3182)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3070)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3091)|
| | | | |
**[CaringDev](https://github.com/CaringDev)**|**[carlreid](https://github.com/carlreid)**|**[carlreinke](https://github.com/carlreinke)**|**[cgrevil](https://github.com/cgrevil)**|
[#1](https://github.com/dotnet/efcore/pull/23585), [#2](https://github.com/dotnet/efcore/pull/23585)|[#1](https://github.com/dotnet/efcore/pull/24498), [#2](https://github.com/dotnet/efcore/pull/24498)|[#1](https://github.com/dotnet/efcore/pull/23694), [#2](https://github.com/dotnet/efcore/pull/23694)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3154)|
| | | | |
**[cgrimes01](https://github.com/cgrimes01)**|**[cincuranet](https://github.com/cincuranet)**|**[dan-giddins](https://github.com/dan-giddins)**|**[dennisseders](https://github.com/dennisseders)**|
[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3038)|[#1](https://github.com/dotnet/efcore/pull/24234), [#2](https://github.com/dotnet/efcore/pull/24234), [#3](https://github.com/dotnet/EntityFramework.Docs/pull/2714), [#4](https://github.com/dotnet/EntityFramework.Docs/pull/3185)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2910)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2839), [#2](https://github.com/dotnet/EntityFramework.Docs/pull/2845), [#3](https://github.com/dotnet/EntityFramework.Docs/pull/2848), [#4](https://github.com/dotnet/EntityFramework.Docs/pull/2987), [#5](https://github.com/dotnet/EntityFramework.Docs/pull/2997), [#6](https://github.com/dotnet/EntityFramework.Docs/pull/3007)|
| | | | |
**[DickBaker](https://github.com/DickBaker)**|**[ErikEJ](https://github.com/ErikEJ)**|**[fagnercarvalho](https://github.com/fagnercarvalho)**|**[FarshanAhamed](https://github.com/FarshanAhamed)**|
[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2990)|[#1](https://github.com/dotnet/efcore/pull/22900), [#2](https://github.com/dotnet/efcore/pull/22900), [#3](https://github.com/dotnet/efcore/pull/22937), [#4](https://github.com/dotnet/efcore/pull/22937), [#5](https://github.com/dotnet/efcore/pull/22938), [#6](https://github.com/dotnet/efcore/pull/22938), [#7](https://github.com/dotnet/EntityFramework.Docs/pull/2897), [#8](https://github.com/dotnet/EntityFramework.Docs/pull/2984), [#9](https://github.com/dotnet/EntityFramework.Docs/pull/3187), [#10](https://github.com/dotnet/EntityFramework.Docs/pull/3197), [#11](https://github.com/dotnet/EntityFramework.Docs/pull/3230)|[#1](https://github.com/dotnet/efcore/pull/23094), [#2](https://github.com/dotnet/efcore/pull/23094)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3181)|
| | | | |
**[filipnavara](https://github.com/filipnavara)**|**[garyng](https://github.com/garyng)**|**[Geoff1900](https://github.com/Geoff1900)**|**[gfoidl](https://github.com/gfoidl)**|
[#1](https://github.com/dotnet/efcore/pull/23591), [#2](https://github.com/dotnet/efcore/pull/23591)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3045), [#2](https://github.com/dotnet/EntityFramework.Docs/pull/3046), [#3](https://github.com/dotnet/EntityFramework.Docs/pull/3047)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3025)|[#1](https://github.com/dotnet/efcore/pull/22923), [#2](https://github.com/dotnet/efcore/pull/22923)|
| | | | |
**[Giorgi](https://github.com/Giorgi)**|**[GitHubPang](https://github.com/GitHubPang)**|**[gurustron](https://github.com/gurustron)**|**[hez2010](https://github.com/hez2010)**|
[#1](https://github.com/dotnet/efcore/pull/24147), [#2](https://github.com/dotnet/efcore/pull/24147), [#3](https://github.com/dotnet/EntityFramework.Docs/pull/3106), [#4](https://github.com/dotnet/EntityFramework.Docs/pull/3107)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3097)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3010)|[#1](https://github.com/dotnet/efcore/pull/24211), [#2](https://github.com/dotnet/efcore/pull/24211)|
| | | | |
**[HSchwichtenberg](https://github.com/HSchwichtenberg)**|**[jaliyaudagedara](https://github.com/jaliyaudagedara)**|**[jantlee](https://github.com/jantlee)**|**[jeremycook](https://github.com/jeremycook)**|
[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2894)|[#1](https://github.com/dotnet/efcore/pull/24499), [#2](https://github.com/dotnet/efcore/pull/24499)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2786)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2827)|
| | | | |
**[jing8956](https://github.com/jing8956)**|**[joakimriedel](https://github.com/joakimriedel)**|**[joaopgrassi](https://github.com/joaopgrassi)**|**[josemiltonsampaio](https://github.com/josemiltonsampaio)**|
[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3146)|[#1](https://github.com/dotnet/efcore/pull/23437), [#2](https://github.com/dotnet/efcore/pull/23437)|[#1](https://github.com/dotnet/efcore/pull/22849), [#2](https://github.com/dotnet/efcore/pull/22849)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2927)|
| | | | |
**[KaloyanIT](https://github.com/KaloyanIT)**|**[khalidabuhakmeh](https://github.com/khalidabuhakmeh)**|**[khellang](https://github.com/khellang)**|**[koenbeuk](https://github.com/koenbeuk)**|
[#1](https://github.com/dotnet/efcore/pull/23563), [#2](https://github.com/dotnet/efcore/pull/23563), [#3](https://github.com/dotnet/efcore/pull/23666), [#4](https://github.com/dotnet/efcore/pull/23666)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2858), [#2](https://github.com/dotnet/EntityFramework.Docs/pull/2962)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2982)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2921)|
| | | | |
**[kotpal](https://github.com/kotpal)**|**[larsholm](https://github.com/larsholm)**|**[lauxjpn](https://github.com/lauxjpn)**|**[leonardoporro](https://github.com/leonardoporro)**|
[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2763)|[#1](https://github.com/dotnet/efcore/pull/24624), [#2](https://github.com/dotnet/efcore/pull/24624)|[#1](https://github.com/dotnet/efcore/pull/24806), [#2](https://github.com/dotnet/efcore/pull/24806)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2883)|
| | | | |
**[lexkazakov](https://github.com/lexkazakov)**|**[mariuz](https://github.com/mariuz)**|**[MartinWestminster](https://github.com/MartinWestminster)**|**[Marusyk](https://github.com/Marusyk)**|
[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3191)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3124)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3118)|[#1](https://github.com/dotnet/efcore/pull/23039), [#2](https://github.com/dotnet/efcore/pull/23039), [#3](https://github.com/dotnet/efcore/pull/24016), [#4](https://github.com/dotnet/efcore/pull/24016), [#5](https://github.com/dotnet/efcore/pull/24203), [#6](https://github.com/dotnet/efcore/pull/24203), [#7](https://github.com/dotnet/efcore/pull/24204), [#8](https://github.com/dotnet/efcore/pull/24204), [#9](https://github.com/dotnet/efcore/pull/24247), [#10](https://github.com/dotnet/efcore/pull/24247), [#11](https://github.com/dotnet/efcore/pull/24284), [#12](https://github.com/dotnet/efcore/pull/24284), [#13](https://github.com/dotnet/efcore/pull/24286), [#14](https://github.com/dotnet/efcore/pull/24286), [#15](https://github.com/dotnet/efcore/pull/24323), [#16](https://github.com/dotnet/efcore/pull/24323)|
| | | | |
**[MattKomorcec](https://github.com/MattKomorcec)**|**[MaxG117](https://github.com/MaxG117)**|**[mefateah](https://github.com/mefateah)**|**[meggima](https://github.com/meggima)**|
[#1](https://github.com/dotnet/efcore/pull/24728), [#2](https://github.com/dotnet/efcore/pull/24728)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2898)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3065)|[#1](https://github.com/dotnet/efcore/pull/23605), [#2](https://github.com/dotnet/efcore/pull/23605)|
| | | | |
**[mguinness](https://github.com/mguinness)**|**[michalczerwinski](https://github.com/michalczerwinski)**|**[mrlife](https://github.com/mrlife)**|**[msawczyn](https://github.com/msawczyn)**|
[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3227)|[#1](https://github.com/dotnet/efcore/pull/24793), [#2](https://github.com/dotnet/efcore/pull/24793), [#3](https://github.com/dotnet/efcore/pull/24814), [#4](https://github.com/dotnet/efcore/pull/24814)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3094), [#2](https://github.com/dotnet/EntityFramework.Docs/pull/3128), [#3](https://github.com/dotnet/EntityFramework.Docs/pull/3129), [#4](https://github.com/dotnet/EntityFramework.Docs/pull/3132)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2917)|
| | | | |
**[MSDN-WhiteKnight](https://github.com/MSDN-WhiteKnight)**|**[natashanikolic](https://github.com/natashanikolic)**|**[nmichels](https://github.com/nmichels)**|**[nschonni](https://github.com/nschonni)**|
[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2887)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2920)|[#1](https://github.com/dotnet/efcore/pull/23091), [#2](https://github.com/dotnet/efcore/pull/23091)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2775), [#2](https://github.com/dotnet/EntityFramework.Docs/pull/2776), [#3](https://github.com/dotnet/EntityFramework.Docs/pull/2779), [#4](https://github.com/dotnet/EntityFramework.Docs/pull/2780)|
| | | | |
**[OKTAYKIR](https://github.com/OKTAYKIR)**|**[OOberoi](https://github.com/OOberoi)**|**[Oxyrus](https://github.com/Oxyrus)**|**[pkellner](https://github.com/pkellner)**|
[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3145)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3163)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3110)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2954)|
| | | | |
**[ptupitsyn](https://github.com/ptupitsyn)**|**[ralmsdeveloper](https://github.com/ralmsdeveloper)**|**[RaymondHuy](https://github.com/RaymondHuy)**|**[riscie](https://github.com/riscie)**|
[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3202)|[#1](https://github.com/dotnet/efcore/pull/19473), [#2](https://github.com/dotnet/efcore/pull/19473)|[#1](https://github.com/dotnet/efcore/pull/22514), [#2](https://github.com/dotnet/efcore/pull/22514), [#3](https://github.com/dotnet/efcore/pull/23145), [#4](https://github.com/dotnet/efcore/pull/23145), [#5](https://github.com/dotnet/efcore/pull/23232), [#6](https://github.com/dotnet/efcore/pull/23232), [#7](https://github.com/dotnet/efcore/pull/23424), [#8](https://github.com/dotnet/efcore/pull/23424)|[#1](https://github.com/dotnet/efcore/pull/20792), [#2](https://github.com/dotnet/efcore/pull/20792)|
| | | | |
**[SergerGood](https://github.com/SergerGood)**|**[Shirasho](https://github.com/Shirasho)**|**[SimonCropp](https://github.com/SimonCropp)**|**[stevendarby](https://github.com/stevendarby)**|
[#1](https://github.com/dotnet/efcore/pull/24750), [#2](https://github.com/dotnet/efcore/pull/24750), [#3](https://github.com/dotnet/efcore/pull/24751), [#4](https://github.com/dotnet/efcore/pull/24751), [#5](https://github.com/dotnet/efcore/pull/24752), [#6](https://github.com/dotnet/efcore/pull/24752), [#7](https://github.com/dotnet/efcore/pull/24753), [#8](https://github.com/dotnet/efcore/pull/24753), [#9](https://github.com/dotnet/efcore/pull/24755), [#10](https://github.com/dotnet/efcore/pull/24755)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2988)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2957), [#2](https://github.com/dotnet/EntityFramework.Docs/pull/2959)|[#1](https://github.com/dotnet/efcore/pull/24746), [#2](https://github.com/dotnet/efcore/pull/24746)|
| | | | |
**[Strepto](https://github.com/Strepto)**|**[teo-tsirpanis](https://github.com/teo-tsirpanis)**|**[the-wazz](https://github.com/the-wazz)**|**[tkp1n](https://github.com/tkp1n)**|
[#1](https://github.com/dotnet/efcore/pull/24141), [#2](https://github.com/dotnet/efcore/pull/24141)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3123)|[#1](https://github.com/dotnet/efcore/pull/23551), [#2](https://github.com/dotnet/efcore/pull/23551)|[#1](https://github.com/dotnet/efcore/pull/23014), [#2](https://github.com/dotnet/efcore/pull/23014)|
| | | | |
**[Tomkaa](https://github.com/Tomkaa)**|**[umitkavala](https://github.com/umitkavala)**|**[uncheckederror](https://github.com/uncheckederror)**|**[Varorbc](https://github.com/Varorbc)**|
[#1](https://github.com/dotnet/efcore/pull/23933), [#2](https://github.com/dotnet/efcore/pull/23933)|[#1](https://github.com/dotnet/efcore/pull/23322), [#2](https://github.com/dotnet/efcore/pull/23322), [#3](https://github.com/dotnet/efcore/pull/23562), [#4](https://github.com/dotnet/efcore/pull/23562)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3168)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3141)|
| | | | |
**[vincent1405](https://github.com/vincent1405)**|**[vonzshik](https://github.com/vonzshik)**|**[vytotas](https://github.com/vytotas)**|**[wdesgardin](https://github.com/wdesgardin)**|
[#1](https://github.com/dotnet/efcore/pull/24020), [#2](https://github.com/dotnet/efcore/pull/24020)|[#1](https://github.com/dotnet/efcore/pull/24775), [#2](https://github.com/dotnet/efcore/pull/24775), [#3](https://github.com/dotnet/efcore/pull/24778), [#4](https://github.com/dotnet/efcore/pull/24778)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3158)|[#1](https://github.com/dotnet/efcore/pull/24588), [#2](https://github.com/dotnet/efcore/pull/24588)|
| | | | |
**[wmeints](https://github.com/wmeints)**|**[yesmey](https://github.com/yesmey)**| | |
[#1](https://github.com/dotnet/efcore/pull/23873), [#2](https://github.com/dotnet/efcore/pull/23873)|[#1](https://github.com/dotnet/efcore/pull/24111), [#2](https://github.com/dotnet/efcore/pull/24111), [#3](https://github.com/dotnet/efcore/pull/24155), [#4](https://github.com/dotnet/efcore/pull/24155), [#5](https://github.com/dotnet/efcore/pull/24160), [#6](https://github.com/dotnet/efcore/pull/24160)| | |
