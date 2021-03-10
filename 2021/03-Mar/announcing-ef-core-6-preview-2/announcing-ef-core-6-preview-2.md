---
post_title: 'Announcing Entity Framework Core 6.0 Preview 2'
username: jeremy-likness
microsoft_alias: jeliknes
desired_publication_date: 3/11/2021
categories: .NET Core, Entity Framework, ASP.NET 
summary: Announcing the release of EF Core 6.0 Preview 2, the second preview of the new Entity Framework Core.
---

Today, the Entity Framework Core team announces the second preview release of 
[EF Core 6.0](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/6.0.0-preview.1.21102.2). 
This release includes changes to handling the synchronization context when `SaveChangesAsync` is called, smoother integration with `System.Linq.Async`, updates to string concatenation and improvements to free text search. 

## Prerequisites 

- EF Core 6.0 currently targets .NET 5. This will likely be updated to .NET 6 as we near the release. EF Core 6.0 does not target any .NET Standard version; for more information see [the future of .NET Standard](https://devblogs.microsoft.com/dotnet/the-future-of-net-standard/).

- EF Core 6.0 will not run on .NET Framework.

## How to get EF Core 6.0 previews

EF Core is distributed exclusively as a set of NuGet packages.
For example, to add the SQL Server provider to your project, you can use the following command using the `dotnet` CLI:

```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --prerelease
```

This following table links to the preview 2 versions of the EF Core packages and describes what they are used for.

|**Package**    |**Purpose**      |
|--------------:|:----------------|
|[Microsoft.EntityFrameworkCore](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/6.0.0-preview.1.21102.2)|The main EF Core package that is independent of specific database providers|
|[Microsoft.EntityFrameworkCore.SqlServer](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer/6.0.0-preview.1.21102.2)|Database provider for Microsoft SQL Server and SQL Azure|
|[Microsoft.EntityFrameworkCore.SqlServer.NetTopologySuite](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer.NetTopologySuite/6.0.0-preview.1.21102.2)|SQL Server support for spatial types|
|[Microsoft.EntityFrameworkCore.Sqlite](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite/6.0.0-preview.1.21102.2)|Database provider for SQLite that includes the native binary for the database engine|
|[Microsoft.EntityFrameworkCore.Sqlite.Core](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite.Core/6.0.0-preview.1.21102.2)|Database provider for SQLite _without_ a packaged native binary|
|[Microsoft.EntityFrameworkCore.Sqlite.NetTopologySuite](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite.NetTopologySuite/6.0.0-preview.1.21102.2)|SQLite support for spatial types|
|[Microsoft.EntityFrameworkCore.Cosmos](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Cosmos/6.0.0-preview.1.21102.2)|Database provider for Azure Cosmos DB|
|[Microsoft.EntityFrameworkCore.InMemory](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.InMemory/6.0.0-preview.1.21102.2)|The in-memory database provider|
|[Microsoft.EntityFrameworkCore.Tools](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Tools/6.0.0-preview.1.21102.2)|EF Core PowerShell commands for the Visual Studio Package Manager Console; use this to integrate tools like [scaffolding](https://docs.microsoft.com/ef/core/managing-schemas/scaffolding) and [migrations](https://docs.microsoft.com/ef/core/managing-schemas/migrations/) with Visual Studio|
|[Microsoft.EntityFrameworkCore.Design](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Design/6.0.0-preview.1.21102.2)|Shared design-time components for EF Core tools|
|[Microsoft.EntityFrameworkCore.Proxies](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Proxies/6.0.0-preview.1.21102.2)|Lazy-loading and change-tracking proxies|
|[Microsoft.EntityFrameworkCore.Abstractions](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Abstractions/6.0.0-preview.1.21102.2)|Decoupled EF Core abstractions; use this for features like extended data annotations defined by EF Core|
|[Microsoft.EntityFrameworkCore.Relational](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Relational/6.0.0-preview.1.21102.2)|Shared EF Core components for relational database providers|
|[Microsoft.EntityFrameworkCore.Analyzers](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Analyzers/6.0.0-preview.1.21102.2)|C# analyzers for EF Core|

We also published the 6.0 preview 2 release of the [Microsoft.Data.Sqlite.Core](https://www.nuget.org/packages/Microsoft.Data.Sqlite.Core/6.0.0-preview.1.21102.2) provider for [ADO.NET](https://docs.microsoft.com/dotnet/framework/data/adonet/ado-net-overview).

## Installing the EF Core Command Line Interface (CLI)

As with EF Core 3.0 and 3.1, the EF Core CLI is no longer included in the .NET Core SDK. Before you can execute EF Core migration or scaffolding commands, 
you'll have to install this package as either a global or local tool.

To install the preview tool globally, first uninstall any existing version with:

```bash
dotnet tool uninstall --global dotnet-ef
```

Then install with:

```bash
dotnet tool install --global dotnet-ef --version 6.0.0-preview.1.21102.2
```

It's possible to use this new version of the EF Core CLI with projects that use older versions of the EF Core runtime.

---

## What's New in EF Core 6 Preview 2

We maintain documentation covering [new features introduced into each preview](https://docs.microsoft.com/ef/core/what-is-new/ef-core-6.0/whatsnew).

Some of the highlights from preview 2 are called out below. This preview also includes several bug fixes.

> **TIP**
> You can run and debug into all the preview 2 samples shown below by [downloading the sample code from GitHub](https://github.com/dotnet/EntityFramework.Docs/tree/main/samples/core/Miscellaneous/NewInEFCore6).
> 

> **NOTE**
> The following fix did _not_ make it into Preview 2: [#24251](https://github.com/dotnet/efcore/issues/24251).

### Preserve synchronization context in SaveChangesAsync

GitHub Issue: [#23971](https://github.com/dotnet/efcore/issues/23971).

We [changed the EF Core code in the 5.0 release](https://github.com/dotnet/efcore/issues/10164) to set `System.Threading.Tasks.Task.ConfigureAwait` to `false` in all places where we `await` async code. This is generally a better choice for EF Core usage. However, `System.Data.Entity.DbContext.SaveChangesAsync` is a special case because EF Core will set generated values into tracked entities after the async database operation is complete. These changes may then trigger notifications which, for example, may have to run on the U.I. thread. Therefore, we are reverting this change in EF Core 6.0 for the `System.Data.Entity.DbContext.SaveChangesAsync` method only.

### Translate String.Concat with multiple arguments

GitHub Issue: [#23859](https://github.com/dotnet/efcore/issues/23859). This feature was contributed by [@wmeints](https://github.com/wmeints).

Starting with EF Core 6.0, calls to `System.String.Concat` with multiple arguments are now translated to SQL. For example, the following query:

```csharp
var shards = context.Shards
  .Where(e => string.Concat(e.Token1, e.Token2, e.Token3) != e.TokensProcessed).ToList();
```

Will be translated to the following SQL when using SQL Server:

```sql
SELECT [s].[Id], [s].[Token1], [s].[Token2], [s].[Token3], [s].[TokensProcessed]
FROM [Shards] AS [s]
WHERE ((COALESCE([s].[Token1], N'') + (COALESCE([s].[Token2], N'') + COALESCE([s].[Token3], N''))) <> [s].[TokensProcessed]) OR [s].[TokensProcessed] IS NULL
```

### Smoother integration with System.Linq.Async

GitHub Issue: [#24041](https://github.com/dotnet/efcore/issues/24041).

The [System.Linq.Async](https://www.nuget.org/packages/System.Linq.Async/) package adds client-side async LINQ processing. Using this package with previous versions of EF Core was cumbersome due to a namespace clash for the async LINQ methods. In EF Core 6.0 we have taken advantage of C# pattern matching for `System.Collections.Generic.IAsyncEnumerable` such that the exposed EF Core `Microsoft.EntityFrameworkCore.DbSet` does not need to implement the interface directly.

Note that most applications do not need to use System.Linq.Async since EF Core queries are usually fully translated on the server.

### More flexible free-text search

GitHub Issue: [#23921](https://github.com/dotnet/efcore/issues/23921).

In EF Core 6.0, we have relaxed the parameter requirements for `Microsoft.EntityFrameworkCore.SqlServerDbFunctionsExtensions.FreeText(Microsoft.EntityFrameworkCore.DbFunctions,System.String,System.String)` and `Microsoft.EntityFrameworkCore.SqlServerDbFunctionsExtensions.Contains`. This allows these functions to be used with binary columns, or with columns mapped using a value converter. For example, consider an entity type with a `Name` property defined as a value object:

```csharp
public class Customer
{
  public int Id { get; set; }

  public Name Name{ get; set; }
}

public class Name
{
  public string First { get; set; }
  public string MiddleInitial { get; set; }
  public string Last { get; set; }
}
```

This is mapped to JSON in the database:

```csharp
modelBuilder.Entity<Customer>()
  .Property(e => e.Name)
  .HasConversion(
    v => JsonSerializer.Serialize(v, null),
    v => JsonSerializer.Deserialize<Name>(v, null));
```

A query can now be executed using `Contains` or `FreeText` even though the type of the property is `Name` not `string`. For example:

```csharp
var result = context.Customers.Where(e => EF.Functions.Contains(e.Name, "Martin")).ToList();
```

This generates the following SQL, when using SQL Server:

```sql
SELECT [c].[Id], [c].[Name]
FROM [Customers] AS [c]
WHERE CONTAINS([c].[Name], N'Martin')
```

## Daily builds

EF Core previews are aligned with .NET 6 previews. These previews tend to lag behind the latest work on EF Core. Consider using the [daily builds](https://github.com/aspnet/AspNetCore/blob/master/docs/DailyBuilds.md) instead to get the most up-to-date EF Core features and bug fixes.

As with the previews, the daily builds require .NET 5.

## The EF Core Community Standup

The EF Core team is now live streaming every other Wednesday at 10am Pacific Time, 1pm Eastern Time, or 17:00 UTC. Join the stream to ask questions about the EF Core topic of your choice, including the latest preview release. 

* [Watch our YouTube playlist](https://aka.ms/efstandups) of previous shows
* [Visit the .NET Community Standup](https://live.dot.net/) page to preview upcoming shows
* [Suggest a guest or project, including your own](https://github.com/dotnet/efcore/issues/22700) by posting to the linked issue

## Documentation and Feedback

The starting point for all EF Core documentation is [docs.microsoft.com/ef/](https://docs.microsoft.com/ef/).

Please file issues found and any other feedback on the [dotnet/efcore GitHub repo](https://github.com/dotnet/efcore).

## Helpful Short Links

The following short links are provided for easy reference and access.

EF Core Community Standup Playlist:
https://aka.ms/efstandups

Main documentation:
https://aka.ms/efdocs

Issues and feature requests for EF Core:
https://aka.ms/efcorefeedback

Entity Framework Roadmap:
https://aka.ms/efroadmap

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
|**[Ali-YousefiTelori](https://github.com/Ali-YousefiTelori)**|**[AndrewKitu](https://github.com/AndrewKitu)**|**[ardalis](https://github.com/ardalis)**|**[CaringDev](https://github.com/CaringDev)**|
|[#1](https://github.com/dotnet/efcore/pull/23946), [#2](https://github.com/dotnet/efcore/pull/23946)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3070)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3091)|[#1](https://github.com/dotnet/efcore/pull/23585), [#2](https://github.com/dotnet/efcore/pull/23585)|
| | | | |
**[carlreinke](https://github.com/carlreinke)**|**[cgrimes01](https://github.com/cgrimes01)**|**[cincuranet](https://github.com/cincuranet)**|**[dan-giddins](https://github.com/dan-giddins)**|
[#1](https://github.com/dotnet/efcore/pull/23694), [#2](https://github.com/dotnet/efcore/pull/23694)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3038)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2714)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2910)|
| | | | |
**[dennisseders](https://github.com/dennisseders)**|**[DickBaker](https://github.com/DickBaker)**|**[ErikEJ](https://github.com/ErikEJ)**|**[fagnercarvalho](https://github.com/fagnercarvalho)**|
[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2839), [#2](https://github.com/dotnet/EntityFramework.Docs/pull/2845), [#3](https://github.com/dotnet/EntityFramework.Docs/pull/2848), [#4](https://github.com/dotnet/EntityFramework.Docs/pull/2987), [#5](https://github.com/dotnet/EntityFramework.Docs/pull/2997), [#6](https://github.com/dotnet/EntityFramework.Docs/pull/3007)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2990)|[#1](https://github.com/dotnet/efcore/pull/22900), [#2](https://github.com/dotnet/efcore/pull/22900), [#3](https://github.com/dotnet/efcore/pull/22937), [#4](https://github.com/dotnet/efcore/pull/22937), [#5](https://github.com/dotnet/efcore/pull/22938), [#6](https://github.com/dotnet/efcore/pull/22938), [#7](https://github.com/dotnet/EntityFramework.Docs/pull/2897), [#8](https://github.com/dotnet/EntityFramework.Docs/pull/2984)|[#1](https://github.com/dotnet/efcore/pull/23094), [#2](https://github.com/dotnet/efcore/pull/23094)|
| | | | |
**[filipnavara](https://github.com/filipnavara)**|**[garyng](https://github.com/garyng)**|**[Geoff1900](https://github.com/Geoff1900)**|**[gfoidl](https://github.com/gfoidl)**|
[#1](https://github.com/dotnet/efcore/pull/23591), [#2](https://github.com/dotnet/efcore/pull/23591)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3045), [#2](https://github.com/dotnet/EntityFramework.Docs/pull/3046), [#3](https://github.com/dotnet/EntityFramework.Docs/pull/3047)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3025)|[#1](https://github.com/dotnet/efcore/pull/22923), [#2](https://github.com/dotnet/efcore/pull/22923)|
| | | | |
**[Giorgi](https://github.com/Giorgi)**|**[GitHubPang](https://github.com/GitHubPang)**|**[gurustron](https://github.com/gurustron)**|**[hez2010](https://github.com/hez2010)**|
[#1](https://github.com/dotnet/efcore/pull/24147), [#2](https://github.com/dotnet/efcore/pull/24147), [#3](https://github.com/dotnet/EntityFramework.Docs/pull/3106), [#4](https://github.com/dotnet/EntityFramework.Docs/pull/3107)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3097)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3010)|[#1](https://github.com/dotnet/efcore/pull/24211), [#2](https://github.com/dotnet/efcore/pull/24211)|
| | | | |
**[HSchwichtenberg](https://github.com/HSchwichtenberg)**|**[jantlee](https://github.com/jantlee)**|**[joakimriedel](https://github.com/joakimriedel)**|**[joaopgrassi](https://github.com/joaopgrassi)**|
[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2894)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2786)|[#1](https://github.com/dotnet/efcore/pull/23437), [#2](https://github.com/dotnet/efcore/pull/23437)|[#1](https://github.com/dotnet/efcore/pull/22849), [#2](https://github.com/dotnet/efcore/pull/22849)|
| | | | |
**[josemiltonsampaio](https://github.com/josemiltonsampaio)**|**[KaloyanIT](https://github.com/KaloyanIT)**|**[khalidabuhakmeh](https://github.com/khalidabuhakmeh)**|**[khellang](https://github.com/khellang)**|
[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2927)|[#1](https://github.com/dotnet/efcore/pull/23563), [#2](https://github.com/dotnet/efcore/pull/23563), [#3](https://github.com/dotnet/efcore/pull/23666), [#4](https://github.com/dotnet/efcore/pull/23666)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2858), [#2](https://github.com/dotnet/EntityFramework.Docs/pull/2962)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2982)|
| | | | |
**[koenbeuk](https://github.com/koenbeuk)**|**[kotpal](https://github.com/kotpal)**|**[leonardoporro](https://github.com/leonardoporro)**|**[Marusyk](https://github.com/Marusyk)**|
[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2921)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2763)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2883)|[#1](https://github.com/dotnet/efcore/pull/23039), [#2](https://github.com/dotnet/efcore/pull/23039), [#3](https://github.com/dotnet/efcore/pull/24016), [#4](https://github.com/dotnet/efcore/pull/24016), [#5](https://github.com/dotnet/efcore/pull/24203), [#6](https://github.com/dotnet/efcore/pull/24203), [#7](https://github.com/dotnet/efcore/pull/24204), [#8](https://github.com/dotnet/efcore/pull/24204)|
| | | | |
**[MaxG117](https://github.com/MaxG117)**|**[mefateah](https://github.com/mefateah)**|**[meggima](https://github.com/meggima)**|**[mrlife](https://github.com/mrlife)**|
[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2898)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3065)|[#1](https://github.com/dotnet/efcore/pull/23605), [#2](https://github.com/dotnet/efcore/pull/23605)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3094)|
| | | | |
**[msawczyn](https://github.com/msawczyn)**|**[MSDN-WhiteKnight](https://github.com/MSDN-WhiteKnight)**|**[natashanikolic](https://github.com/natashanikolic)**|**[nmichels](https://github.com/nmichels)**|
[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2917)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2887)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2920)|[#1](https://github.com/dotnet/efcore/pull/23091), [#2](https://github.com/dotnet/efcore/pull/23091)|
| | | | |
**[nschonni](https://github.com/nschonni)**|**[Oxyrus](https://github.com/Oxyrus)**|**[pkellner](https://github.com/pkellner)**|**[ralmsdeveloper](https://github.com/ralmsdeveloper)**|
[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2775), [#2](https://github.com/dotnet/EntityFramework.Docs/pull/2776), [#3](https://github.com/dotnet/EntityFramework.Docs/pull/2779), [#4](https://github.com/dotnet/EntityFramework.Docs/pull/2780)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3110)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2954)|[#1](https://github.com/dotnet/efcore/pull/19473), [#2](https://github.com/dotnet/efcore/pull/19473)|
| | | | |
**[RaymondHuy](https://github.com/RaymondHuy)**|**[Shirasho](https://github.com/Shirasho)**|**[SimonCropp](https://github.com/SimonCropp)**|**[the-wazz](https://github.com/the-wazz)**|
[#1](https://github.com/dotnet/efcore/pull/22514), [#2](https://github.com/dotnet/efcore/pull/22514), [#3](https://github.com/dotnet/efcore/pull/23145), [#4](https://github.com/dotnet/efcore/pull/23145), [#5](https://github.com/dotnet/efcore/pull/23232), [#6](https://github.com/dotnet/efcore/pull/23232), [#7](https://github.com/dotnet/efcore/pull/23424), [#8](https://github.com/dotnet/efcore/pull/23424)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2988)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2957), [#2](https://github.com/dotnet/EntityFramework.Docs/pull/2959)|[#1](https://github.com/dotnet/efcore/pull/23551), [#2](https://github.com/dotnet/efcore/pull/23551)|
| | | | |
**[tkp1n](https://github.com/tkp1n)**|**[Tomkaa](https://github.com/Tomkaa)**|**[umitkavala](https://github.com/umitkavala)**|**[vincent1405](https://github.com/vincent1405)**|
[#1](https://github.com/dotnet/efcore/pull/23014), [#2](https://github.com/dotnet/efcore/pull/23014)|[#1](https://github.com/dotnet/efcore/pull/23933), [#2](https://github.com/dotnet/efcore/pull/23933)|[#1](https://github.com/dotnet/efcore/pull/23322), [#2](https://github.com/dotnet/efcore/pull/23322), [#3](https://github.com/dotnet/efcore/pull/23562), [#4](https://github.com/dotnet/efcore/pull/23562)|[#1](https://github.com/dotnet/efcore/pull/24020), [#2](https://github.com/dotnet/efcore/pull/24020)|
| | | | |
**[wmeints](https://github.com/wmeints)**|**[yesmey](https://github.com/yesmey)**| | |
[#1](https://github.com/dotnet/efcore/pull/23873), [#2](https://github.com/dotnet/efcore/pull/23873)|[#1](https://github.com/dotnet/efcore/pull/24111), [#2](https://github.com/dotnet/efcore/pull/24111), [#3](https://github.com/dotnet/efcore/pull/24155), [#4](https://github.com/dotnet/efcore/pull/24155)| | |
