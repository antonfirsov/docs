# Announcing Entity Framework Core 5.0 Preview 6

Today, the Entity Framework Core team announces the sixth preview release of [EF Core 5.0](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/5.0.0-TBDVersion). This release includes split queries for related collections, a new "index" attribute, improved exceptions related to query translations, IP address mapping, exposing transaction id for correlation, and much more. 

## Prerequisites 

**EF Core 5.0 will _not_ run on .NET Standard 2.0 platforms, including .NET Framework.**

The previews of EF Core 5.0 require [.NET Standard 2.1](https://github.com/dotnet/standard/blob/master/docs/versions/netstandard2.1.md). This means that EF Core 5.0 will run on .NET Core 3.1 and does _not_ require .NET 5. To summarize: EF Core 5.0 runs on [platforms that support .NET Standard 2.1](https://docs.microsoft.com/dotnet/standard/net-standard#net-implementation-support).

**The plan is to maintain .NET Standard 2.1 compatibility through the final release.** 

## Be a part of .NET 5

The .NET documentation team is [reorganizing .NET content](https://github.com/dotnet/docs/issues/18923) to better match the workloads you build with .NET. This includes a new [.NET Data landing page](https://github.com/dotnet/docs/issues/19029) that will link out to data-related topics ranging from EF Core to APIs, Big Data, and Machine learning. The planning and execution will be done completely in the open on GitHub. This is your opportunity to help shape the hierarchy and content to best fit your needs as a .NET developer. We look forward to your contributions! 
 
---

## How to get EF Core 5.0 previews

EF Core is distributed exclusively as a set of NuGet packages.
For example, to add the SQL Server provider to your project, you can use the following command using the dotnet tool:

```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 5.0.0-TBDVersion
```

This following table links to the preview 6 versions of the EF Core packages and describes what they are used for.

|**Package**    |**Purpose**      |
|--------------:|:----------------|
|[Microsoft.EntityFrameworkCore](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/5.0.0-TBDVersion)|The main EF Core package that is independent of specific database providers|
|[Microsoft.EntityFrameworkCore.SqlServer](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer/5.0.0-TBDVersion)|Database provider for Microsoft SQL Server and SQL Azure|
|[Microsoft.EntityFrameworkCore.SqlServer.NetTopologySuite](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer.NetTopologySuite/5.0.0-TBDVersion)|SQL Server support for spatial types|
|[Microsoft.EntityFrameworkCore.Sqlite](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite/5.0.0-TBDVersion)|Database provider for SQLite that includes the native binary for the database engine|
|[Microsoft.EntityFrameworkCore.Sqlite.Core](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite.Core/5.0.0-TBDVersion)|Database provider for SQLite _without_ a packaged native binary|
|[Microsoft.EntityFrameworkCore.Sqlite.NetTopologySuite](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite.NetTopologySuite/5.0.0-TBDVersion)|SQLite support for spatial types|
|[Microsoft.EntityFrameworkCore.Cosmos](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Cosmos/5.0.0-TBDVersion)|Database provider for Azure Cosmos DB|
|[Microsoft.EntityFrameworkCore.InMemory](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.InMemory/5.0.0-TBDVersion)|The in-memory database provider|
|[Microsoft.EntityFrameworkCore.Tools](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Tools/5.0.0-TBDVersion)|EF Core PowerShell commands for the Visual Studio Package Manager Console; use this to integrate tools like [scaffolding](https://docs.microsoft.com/ef/core/managing-schemas/scaffolding) and [migrations](https://docs.microsoft.com/ef/core/managing-schemas/migrations/) with Visual Studio|
|[Microsoft.EntityFrameworkCore.Design](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Design/5.0.0-TBDVersion)|Shared design-time components for EF Core tools|
|[Microsoft.EntityFrameworkCore.Proxies](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Proxies/5.0.0-TBDVersion)|Lazy-loading and change-tracking proxies|
|[Microsoft.EntityFrameworkCore.Abstractions](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Abstractions/5.0.0-TBDVersion)|Decoupled EF Core abstractions; use this for features like extended data annotations defined by EF Core|
|[Microsoft.EntityFrameworkCore.Relational](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Relational/5.0.0-TBDVersion)|Shared EF Core components for relational database providers|
|[Microsoft.EntityFrameworkCore.Analyzers](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Analyzers/5.0.0-TBDVersion)|C# analyzers for EF Core|

We also published the 5.0 preview 6 release of the [Microsoft.Data.Sqlite.Core](https://www.nuget.org/packages/Microsoft.Data.Sqlite.Core/5.0.0-TBDVersion) provider for [ADO.NET](https://docs.microsoft.com/dotnet/framework/data/adonet/ado-net-overview).

## Installing the EF Core Command Line Interface (CLI)

As with EF Core 3.0 and 3.1, the EF Core CLI is no longer included in the .NET Core SDK. Before you can execute EF Core migration or scaffolding commands, you'll have to install this package as either a global or local tool.

![dotnet-ef](https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/05/efcore-preview4-scaled.jpg)

To install the preview tool globally, first uninstall any existing version with:

```bash
dotnet tool uninstall --global dotnet-ef
```

Then install with:

```bash
dotnet tool install --global dotnet-ef --version 5.0.0-TBDVersion
```

It's possible to use this new version of the EF Core CLI with projects that use older versions of the EF Core runtime.

---

## What's New in EF Core 5 Preview 6

We maintain documentation covering [new features introduced into each preview](https://docs.microsoft.com/ef/core/what-is-new/ef-core-5.0/whatsnew).

Some of the highlights from preview 6 are called out below. This preview also includes several bug fixes.

### Split queries for related collections

Starting with EF Core 3.0, EF Core always generates a single SQL query for each LINQ query. This ensures consistency of the data returned within the constraints of the transaction mode in use. However, this can become very slow when the query uses `Include` or a projection to bring back multiple related collections.

EF Core 5.0 now allows a single LINQ query including related collections to be split into multiple SQL queries. This can significantly improve performance, but can result in inconsistency in the results returned if the data changes between the two queries. Transactions can be used to mitigate this, at least within the constraints of the transaction mode in use.

#### Split queries with Include

For example, consider a query that pulls in two levels of related collections using `Include`:

```CSharp
var artists = context.Artists
    .Include(e => e.Albums).ThenInclude(e => e.Tags)
    .ToList();
```

By default, EF Core will generate the following SQL when using the SQLite provider:

```sql
SELECT "a"."Id", "a"."Name", "t0"."Id", "t0"."ArtistId", "t0"."Title", "t0"."Id0", "t0"."AlbumId", "t0"."Name"
FROM "Artists" AS "a"
LEFT JOIN (
    SELECT "a0"."Id", "a0"."ArtistId", "a0"."Title", "t"."Id" AS "Id0", "t"."AlbumId", "t"."Name"
    FROM "Album" AS "a0"
    LEFT JOIN "Tag" AS "t" ON "a0"."Id" = "t"."AlbumId"
) AS "t0" ON "a"."Id" = "t0"."ArtistId"
ORDER BY "a"."Id", "t0"."Id", "t0"."Id0"
```

The new `AsSplitQuery` API can be used to change this behavior. For example:

```CSharp
var artists = context.Artists
    .AsSplitQuery()
    .Include(e => e.Albums).ThenInclude(e => e.Tags)
    .ToList();
```

AsSplitQuery is available for all relational database providers and can be used anywhere in the query, just like AsNoTracking. EF Core will now generate the following three SQL queries:

```sql
SELECT "a"."Id", "a"."Name"
FROM "Artists" AS "a"
ORDER BY "a"."Id"

SELECT "a0"."Id", "a0"."ArtistId", "a0"."Title", "a"."Id"
FROM "Artists" AS "a"
INNER JOIN "Album" AS "a0" ON "a"."Id" = "a0"."ArtistId"
ORDER BY "a"."Id", "a0"."Id"

SELECT "t"."Id", "t"."AlbumId", "t"."Name", "a"."Id", "a0"."Id"
FROM "Artists" AS "a"
INNER JOIN "Album" AS "a0" ON "a"."Id" = "a0"."ArtistId"
INNER JOIN "Tag" AS "t" ON "a0"."Id" = "t"."AlbumId"
ORDER BY "a"."Id", "a0"."Id"
```

All operations on the query root are supported. This includes OrderBy/Skip/Take, Join operations, FirstOrDefault and similar single result selecting operations.

Note that filtered Includes with OrderBy/Skip/Take are not supported in preview 6, but are available in the daily builds and will be included in preview 7.

#### Split queries with collection projections

`AsSplitQuery` can also be used when collections are loaded in projections. For example:

```CSharp
context.Artists
    .AsSplitQuery()
    .Select(e => new
    {
        Artist = e,
        Albums = e.Albums,
    }).ToList();
```

This LINQ query generates the following two SQL queries when using the SQLite provider:

```sql
SELECT "a"."Id", "a"."Name"
FROM "Artists" AS "a"
ORDER BY "a"."Id"

SELECT "a0"."Id", "a0"."ArtistId", "a0"."Title", "a"."Id"
FROM "Artists" AS "a"
INNER JOIN "Album" AS "a0" ON "a"."Id" = "a0"."ArtistId"
ORDER BY "a"."Id"
```

Note that only materialization of the collection is supported. Any composition after `e.Albums` in above case won't result in a split query. Improvements in this area are tracked by [#21234](https://github.com/dotnet/efcore/issues/21234).

### IndexAttribute

The new IndexAttribute can be placed on an entity type to specify an index for a single column. For example:

```CSharp
[Index(nameof(FullName), IsUnique = true)]
public class User
{
    public int Id { get; set; }
    
    [MaxLength(128)]
    public string FullName { get; set; }
}
```

For SQL Server, Migrations will then generate the following SQL:

```sql
CREATE UNIQUE INDEX [IX_Users_FullName]
    ON [Users] ([FullName])
    WHERE [FullName] IS NOT NULL;
```

IndexAttribute can also be used to specify an index spanning multiple columns. For example:

```CSharp
[Index(nameof(FirstName), nameof(LastName), IsUnique = true)]
public class User
{
    public int Id { get; set; }
    
    [MaxLength(64)]
    public string FirstName { get; set; }

    [MaxLength(64)]
    public string LastName { get; set; }
}
```

For SQL Server, this results in:

```sql
CREATE UNIQUE INDEX [IX_Users_FirstName_LastName]
    ON [Users] ([FirstName], [LastName])
    WHERE [FirstName] IS NOT NULL AND [LastName] IS NOT NULL;
```

Documentation is tracked by issue [#2407](https://github.com/dotnet/EntityFramework.Docs/issues/2407).

### Improved query translation exceptions

We are continuing to improve the exception messages generated when query translation fails. For example, this query uses the unmapped property `IsSigned`:

```CSharp
var artists = context.Artists.Where(e => e.IsSigned).ToList();
```

EF Core will throw the following exception indicating that translation failed because `IsSigned` is not mapped:

> Unhandled exception. System.InvalidOperationException: The LINQ expression 'DbSet<Artist>()
>    .Where(a => a.IsSigned)' could not be translated. Additional information: Translation of member 'IsSigned' on entity type 'Artist' failed. Possibly the specified member is not mapped. Either rewrite the query in a form that can be translated, or switch to client evaluation explicitly by inserting a call to either AsEnumerable(), AsAsyncEnumerable(), ToList(), or ToListAsync(). See https://go.microsoft.com/fwlink/?linkid=2101038 for more information.

Similarly, better exception messages are now generated when attempting to translate string comparisons with culture-dependent semantics. For example, this query attempts to use `StringComparison.CurrentCulture`:

```CSharp
var artists = context.Artists
    .Where(e => e.Name.Equals("The Unicorns", StringComparison.CurrentCulture))
    .ToList();
```

EF Core will now throw the following exception:

> Unhandled exception. System.InvalidOperationException: The LINQ expression 'DbSet<Artist>()
>      .Where(a => a.Name.Equals(
>          value: "The Unicorns", 
>          comparisonType: CurrentCulture))' could not be translated. Additional information: Translation of 'string.Equals' method which takes 'StringComparison' argument is not supported. See https://go.microsoft.com/fwlink/?linkid=2129535 for more information. Either rewrite the query in a form that can be translated, or switch to client evaluation explicitly by inserting a call to either AsEnumerable(), AsAsyncEnumerable(), ToList(), or ToListAsync(). See https://go.microsoft.com/fwlink/?linkid=2101038 for more information.

### Specify transaction ID

This feature was contributed from the community by [@Marusyk](https://github.com/Marusyk). Many thanks for the contribution!

EF Core exposes a transaction ID for correlation of transactions across calls. This is typically set by EF Core when a transaction is started. If the application starts the transaction instead, then this feature allows the application to explicitly set the transaction ID so it is correlated correctly everywhere it is used. For example:

```CSharp
using (context.Database.UseTransaction(myTransaction, myId))
{
   ...
}
```

### IPAddress mapping

This feature was contributed from the community by [@ralmsdeveloper](https://github.com/ralmsdeveloper). Many thanks for the contribution!

The standard .NET [IPAddress class](https://docs.microsoft.com/dotnet/api/system.net.ipaddress) is now automatically mapped to a string column for databases that do not already have native support. For example, consider mapping this entity type:

```CSharp
public class Host
{
    public int Id { get; set; }
    public IPAddress Address { get; set; }
}
```

On SQL Server, this will result in Migrations creating the following table:

```sql
CREATE TABLE [Host] (
    [Id] int NOT NULL,
    [Address] nvarchar(45) NULL,
    CONSTRAINT [PK_Host] PRIMARY KEY ([Id]));
``` 

Entities can then be added in the normal way:

```CSharp
context.AddRange(
    new Host { Address = IPAddress.Parse("127.0.0.1")},
    new Host { Address = IPAddress.Parse("0000:0000:0000:0000:0000:0000:0000:0001")});
``` 

And the resulting SQL will insert the normalized IPv4 or IPv6 address:

```sql
Executed DbCommand (14ms) [Parameters=[@p0='1', @p1='127.0.0.1' (Size = 45), @p2='2', @p3='::1' (Size = 45)], CommandType='Text', CommandTimeout='30']
      SET NOCOUNT ON;
      INSERT INTO [Host] ([Id], [Address])
      VALUES (@p0, @p1), (@p2, @p3);
```

### Exclude OnConfiguring when scaffolding

When a DbContext is scaffolded from an existing database, EF Core by default creates an OnConfiguring overload with a connection string so that the context is immediately usable. However, this is not useful if you already have a partial class with OnConfiguring, or if you are configuring the context some other way.

To address this, the scaffolding commands can now be instructed to ommit generation of OnConfiguring. For example:

```
dotnet ef dbcontext scaffold "Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Chinook" Microsoft.EntityFrameworkCore.SqlServer --no-onconfiguring
```

Or in the Package Manager Console:

```
Scaffold-DbContext 'Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Chinook' Microsoft.EntityFrameworkCore.SqlServer -NoOnConfiguring 
``` 

Note that we recommend using [a named connection string and secure storage like User Secrets](https://docs.microsoft.com/ef/core/managing-schemas/scaffolding?tabs=vs#configuration-and-user-secrets).

### Translations for FirstOrDefault on strings

This feature was contributed from the community by [@dvoreckyaa](https://github.com/dvoreckyaa). Many thanks for the contribution!

FirstOrDefault and similar operators for characters in strings are now translated. For example, this LINQ query:

```CSharp
context.Customers.Where(c => c.ContactName.FirstOrDefault() == 'A').ToList();
```

Will be translated to the following SQL when using SQL Server:

```sql
SELECT [c].[Id], [c].[ContactName]
FROM [Customer] AS [c]
WHERE SUBSTRING([c].[ContactName], 1, 1) = N'A'
```

### Simplify case blocks

EF Core now generates better queries with CASE blocks. For example, this LINQ query: 

```CSharp
context.Weapons
    .OrderBy(w => w.Name.CompareTo("Marcus' Lancer") == 0)
    .ThenBy(w => w.Id)
```

Was on SQL Server formally translated to:

```sql
SELECT [w].[Id], [w].[AmmunitionType], [w].[IsAutomatic], [w].[Name], [w].[OwnerFullName], [w].[SynergyWithId]
FROM [Weapons] AS [w]
ORDER BY CASE
    WHEN (CASE
        WHEN [w].[Name] = N'Marcus'' Lancer' THEN 0
        WHEN [w].[Name] > N'Marcus'' Lancer' THEN 1
        WHEN [w].[Name] < N'Marcus'' Lancer' THEN -1
    END = 0) AND CASE
        WHEN [w].[Name] = N'Marcus'' Lancer' THEN 0
        WHEN [w].[Name] > N'Marcus'' Lancer' THEN 1
        WHEN [w].[Name] < N'Marcus'' Lancer' THEN -1
    END IS NOT NULL THEN CAST(1 AS bit)
    ELSE CAST(0 AS bit)
END, [w].[Id]");
``` 

But is now translated to:

```sql
SELECT [w].[Id], [w].[AmmunitionType], [w].[IsAutomatic], [w].[Name], [w].[OwnerFullName], [w].[SynergyWithId]
FROM [Weapons] AS [w]
ORDER BY CASE
    WHEN ([w].[Name] = N'Marcus'' Lancer') AND [w].[Name] IS NOT NULL THEN CAST(1 AS bit)
    ELSE CAST(0 AS bit)
END, [w].[Id]");
``` 

---

## Daily builds

EF Core previews are aligned with .NET 5 previews. These previews tend to lag behind the latest work on EF Core. Consider using the [daily builds](https://github.com/aspnet/AspNetCore/blob/master/docs/DailyBuilds.md) instead to get the most up-to-date EF Core features and bug fixes.

As with the previews, the daily builds do not require .NET 5; they can be used with GA/RTM release of .NET Core 3.1.

---

## The EF Core Community Standup

The EF Core team is now live streaming every other Wednesday at 10am Pacific Time, 1pm Eastern Time, or 17:00 UTC. Join the stream to ask questions about the EF Core topic of your choice, including the latest preview release. 

* [Visit the .NET Community Standup](https://dotnet.microsoft.com/platform/community/standup) page to preview upcoming shows and view recordings from past shows
* [Suggest a guest or project, including your own](https://github.com/dotnet/efcore/discussions/21371) by posting to the linked discussion
* You can also [request an EF Core demo](https://github.com/dotnet/efcore/discussions/21192)

## Documentation and feedback

The starting point for all EF Core documentation is [docs.microsoft.com/ef/](https://docs.microsoft.com/ef/).

Please file issues found and any other feedback on the [dotnet/efcore GitHub repo](https://github.com/dotnet/efcore).

---

## Helpful Short Links

The following short links are provided for easy reference and access.

Main documentation:
https://aka.ms/efdocs

Issues and feature requests for EF Core:
https://aka.ms/efcorefeedback

Entity Framework Roadmap:
https://aka.ms/efroadmap

What's new in EF Core 5.x?
https://aka.ms/efcore5

---

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
<td><a href="https://github.com/lajones"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/efteam_lajones.jpeg" alt="lajones" width=200px><br>lajones</a></td>
<td><a href="https://github.com/maumar"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/efteam_maumar.jpeg" alt="maumar" width=200px><br>Maurycy Markowski</a></td>
<td><a href="https://github.com/roji"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/efteam_roji-1-300x300.png" alt="roji" width=200px><br>Shay Rojansky</a></td>
<td><a href="https://github.com/smitpatel"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/efteam_smitpatel.png" alt="smitpatel" width=200px><br>Smit Patel</a>
</td>
</tr>

</table>

---

## Thank you to our contributors!

A big thank you to the following community members who have already contributed code or documentation to the EF Core 5 release! (List is in chronological order of first contribution to EF Core 5).

<table>
<tr>
<td><a href="https://github.com/aevitas"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_aevitas.jpeg" alt="aevitas" width=200px><br>aevitas</a></td>
<td><a href="https://github.com/alaatm"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_alaatm.png" alt="alaatm" width=200px><br>Alaa Masoud</a></td>
<td><a href="https://github.com/aleksandar-manukov"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_aleksandar-manukov.png" alt="aleksandar-manukov" width=200px><br>Aleksandar Manukov</a></td>
<td><a href="https://github.com/amrbadawy"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_amrbadawy.jpeg" alt="amrbadawy" width=200px><br>Amr Badawy</a>
</td>
</tr>

<tr>
<td><a href="https://github.com/AnthonyMonterrosa"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_AnthonyMonterrosa.jpeg" alt="AnthonyMonterrosa" width=200px><br>Anthony Monterrosa</a></td>
<td><a href="https://github.com/bbrandt"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_bbrandt.jpeg" alt="bbrandt" width=200px><br>Ben Brandt</a></td>
<td><a href="https://github.com/benmccallum"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_benmccallum.jpeg" alt="benmccallum" width=200px><br>Ben McCallum</a></td>
<td><a href="https://github.com/ccjx"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_ccjx.png" alt="ccjx" width=200px><br>Clarence Cai</a>
</td>
</tr>

<tr>
<td><a href="https://github.com/CGijbels"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_CGijbels.jpeg" alt="CGijbels" width=200px><br>Christophe Gijbels</a></td>
<td><a href="https://github.com/cincuranet"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_cincuranet.png" alt="cincuranet" width=200px><br>Jiri Cincura</a></td>
<td><a href="https://github.com/Costo"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_Costo.jpeg" alt="Costo" width=200px><br>Vincent Costel</a></td>
<td><a href="https://github.com/dshuvaev"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_dshuvaev.jpeg" alt="dshuvaev" width=200px><br>Dmitry Shuvaev</a>
</td>
</tr>

<tr>
<td><a href="https://github.com/EricStG"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_EricStG.jpeg" alt="EricStG" width=200px><br>Eric St-Georges</a></td>
<td><a href="https://github.com/ErikEJ"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_ErikEJ.jpeg" alt="ErikEJ" width=200px><br>Erik Ejlskov Jensen</a></td>
<td><a href="https://github.com/gravbox"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_gravbox.png" alt="gravbox" width=200px><br>Christopher Davis</a></td>
<td><a href="https://github.com/ivaylokenov"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_ivaylokenov.jpeg" alt="ivaylokenov" width=200px><br>Ivaylo Kenov</a>
</td>
</tr>

<tr>
<td><a href="https://github.com/jfoshee"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_jfoshee.png" alt="jfoshee" width=200px><br>Jacob Foshee</a></td>
<td><a href="https://github.com/jmzagorski"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_jmzagorski.png" alt="jmzagorski" width=200px><br>Jeremy Zagorski</a></td>
<td><a href="https://github.com/jviau"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_jviau.jpeg" alt="jviau" width=200px><br>Jacob Viau</a></td>
<td><a href="https://github.com/knom"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_knom.png" alt="knom" width=200px><br>Max K.</a>
</td>
</tr>

<tr>
<td><a href="https://github.com/lohoris-crane"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_lohoris-crane.jpeg" alt="lohoris-crane" width=200px><br>lohoris-crane</a></td>
<td><a href="https://github.com/loic-sharma"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_loic-sharma.jpeg" alt="loic-sharma" width=200px><br>Loïc Sharma</a></td>
<td><a href="https://github.com/lokalmatador"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_lokalmatador.jpeg" alt="lokalmatador" width=200px><br>lokalmatador</a></td>
<td><a href="https://github.com/mariusGundersen"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_mariusGundersen.png" alt="mariusGundersen" width=200px><br>Marius Gundersen</a>
</td>
</tr>

<tr>
<td><a href="https://github.com/Marusyk"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_Marusyk.jpeg" alt="Marusyk" width=200px><br>Roman Marusyk</a></td>
<td><a href="https://github.com/matthiaslischka"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_matthiaslischka.jpeg" alt="matthiaslischka" width=200px><br>Matthias Lischka</a></td>
<td><a href="https://github.com/MaxG117"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_MaxG117.png" alt="MaxG117" width=200px><br>MaxG117</a></td>
<td><a href="https://github.com/MHDuke"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_MHDuke.jpeg" alt="MHDuke" width=200px><br>MHDuke</a>
</td>
</tr>

<tr>
<td><a href="https://github.com/mikes-gh"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_mikes-gh.png" alt="mikes-gh" width=200px><br>Mike Surcouf</a></td>
<td><a href="https://github.com/Muppets"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_Muppets.jpeg" alt="Muppets" width=200px><br>Neil Bostrom</a></td>
<td><a href="https://github.com/nmichels"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_nmichels.jpeg" alt="nmichels" width=200px><br>Nícolas Michels</a></td>
<td><a href="https://github.com/OOberoi"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_OOberoi.jpeg" alt="OOberoi" width=200px><br>Obi Oberoi</a></td>
</td>
</tr>

<tr>
<td><a href="https://github.com/orionstudt"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_orionstudt.jpeg" alt="orionstudt" width=200px><br>Josh Studt</a></td>
<td><a href="https://github.com/ozantopal"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_ozantopal.jpeg" alt="ozantopal" width=200px><br>Ozan Topal</a></td>
<td><a href="https://github.com/pmiddleton"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_pmiddleton.jpeg" alt="pmiddleton" width=200px><br>Paul Middleton</a></td>
<td><a href="https://github.com/prog-rajkamal"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_prog-rajkamal.jpeg" alt="prog-rajkamal" width=200px><br>Raj</a>
</td>
</tr>

<tr>
<td><a href="https://github.com/ptjhuang"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_ptjhuang.png" alt="ptjhuang" width=200px><br>Peter Huang</a></td>
<td><a href="https://github.com/ralmsdeveloper"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_ralmsdeveloper.png" alt="ralmsdeveloper" width=200px><br>Rafael Almeida Santos</a></td>
<td><a href="https://github.com/redoz"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_redoz.png" alt="redoz" width=200px><br>Patrik Husfloen</a></td>
<td><a href="https://github.com/rmarskell"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_rmarskell.jpeg" alt="rmarskell" width=200px><br>Richard Marskell</a>
</td>
</tr>

<tr>
<td><a href="https://github.com/sguitardude"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_sguitardude.jpeg" alt="sguitardude" width=200px><br>sguitardude</a></td>
<td><a href="https://github.com/SimpleSamples"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_SimpleSamples.png" alt="SimpleSamples" width=200px><br>Sam Hobbs</a></td>
<td><a href="https://github.com/svengeance"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_svengeance.png" alt="svengeance" width=200px><br>Sven</a></td>
<td><a href="https://github.com/VladDragnea"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_VladDragnea.jpeg" alt="VladDragnea" width=200px><br>Vlad</a>
</td>
</tr>

<tr>
<td><a href="https://github.com/vslee"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_vslee.png" alt="vslee" width=200px><br>vslee</a></td>
<td><a href="https://github.com/WeihanLi"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_WeihanLi.jpeg" alt="WeihanLi" width=200px><br>liweihan</a></td>
<td><a href="https://github.com/Youssef1313"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_Youssef1313.jpeg" alt="Youssef1313" width=200px><br>Youssef Victor</a></td>
<td><a href="https://github.com/1iveowl"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/1iveowl.jpg" alt="1iveowl" width=200px><br>1iveowl</a>
</td>
</tr>

<tr>
<td><a href="https://github.com/thomaslevesque"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_thomaslevesque.png" alt="thomaslevesque" width=200px><br>Thomas Levesque</a></td>
<td><a href="https://github.com/akovac35"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_akovac35.png" alt="akovac35" width=200px><br>Aleksander Kovač</a></td>
<td><a href="https://github.com/leotsarev"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_leotsarev.jpg" alt="leotsarev" width=200px><br>Leonid Tsarev</a></td>
<td><a href="https://github.com/kostat"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_kostat.jpg" alt="kostat" width=200px><br>Konstantin Triger</a></td>
</tr>

<tr>
<td><a href="https://github.com/sungam3r"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/04/contributor_sungam3r.png" alt="sungam3r" width=200px><br>Ivan Maximov</a></td>
<td><a href="https://github.com/dzmitry-lahoda"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/04/contributor_dzmitry-lahoda.jpg" alt="dzmitry-lahoda" width=200px><br>Dzmitry Lahoda</a></td>
<td><a href="https://github.com/Logerfo"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/04/contributor_logerfo.jpg" alt="Logerfo" width=200px><br>Bruno Logerfo</a></td>
<td><a href="https://github.com/witheej"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/05/contributor_witheej.jpg" alt="witheej" width=200px><br>Josh Withee</td>
</tr>

<tr>
<td><a href="https://github.com/FransBouma"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/05/contributor_FransBouma.png" alt="FransBouma" width=200px><br>Frans Bouma</td>
<td><a href="https://github.com/IGx89"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/05/contributor_IGx89.png" alt="IGx89" width=200px><br>Matthew Lieder</td>
<td><a href="https://github.com/paulomorgado"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/05/contributor_paulomorgado.jpg" alt="paulomorgado" width=200px><br>Paulo Morgado</td>
<td><a href="https://github.com/mderriey"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/05/contributor_mderriey.jpg" alt="mderriey" width=200px><br>Mickaël Derriey</td>
</tr>

<tr>
<td><a href="https://github.com/LaurenceJKing"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/05/contributor_LaurenceJKing.jpg" alt="LaurenceJKing" width=200px><br>Laurence King</td>
<td><a href="https://github.com/oskarj"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/05/contributor_oskarj.png" alt="oskarj" width=200px><br>Oskar Josefsson</td>
<td><a href="https://github.com/bdebaere"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/05/contributor_bdebaere.jpg" alt="bdebaere" width=200px><br>bdebaere</a></td>
<td><a href="https://github.com/BhargaviAnnadevara-MSFT"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/05/contributor_BhargaviAnnadevara-MSFT.jpg" alt="BhargaviAnnadevara-MSFT" width=200px><br>Bhargavi Annadevara</a></td>
</tr>

<tr>
 <td><a href="https://github.com/AlexanderTaeschner"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/05/contributor_AlexanderTaeschner.png" alt="AlexanderTaeschner" width=200px><br>Alexander Täschner</a></td>
 <td><a href="https://github.com/Jesse-Hufstetler"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/05/contributor_Jesse-Hufstetler.png" alt="Jesse-Hufstetler" width=200px><br>Jesse Hufstetler</a></td>
 <td><a href="https://github.com/ivarlovlie"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/06/contributor_ivarlovlie.png" alt="ivarlovlie" width=200px><br>Ivar Løvlie</a></td>
 <td><a href="https://github.com/cucoreanu"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/06/contributor_cucoreanu.jpg" alt="cucoreanu" width=200px><br>cucoreanu</a></td>
 </tr>

<tr>
<td><a href="https://github.com/serpent5"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/06/contributor_serpent5.png" alt="serpent5" width=200px><br>Kirk Larkin</a></td>
 <td><a href="https://github.com/sdanyliv"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/06/contributor_sdanyliv.jpg" alt="sdanyliv" width=200px><br>Svyatoslav Danyliv</a></td>
 <td><a href="https://github.com/twenzel"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/06/contributor_twenzel.jpg" alt="twenzel" width=200px><br>Toni Wenzel</a></td>
 <td><a href="https://github.com/manvydasu"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/06/contributor_manvydasu.png" alt="manvydasu" width=200px><br>manvydasu</a></td> 
</tr>

<tr> 
<td><a href="https://github.com/brandongregoryscott"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/05/contributor_AlexanderTaeschner.png" alt="brandongregoryscott" width=200px><br>Brandon Scott</a></td>
 <td><a href="https://github.com/uncheckederror"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/06/contributor_uncheckederror.jpg" alt="uncheckederror" width=200px><br>Thomas Ryan</a></td>
 <td><a href="https://github.com/rocke97"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/06/contributor_rocke97.jpg" alt="rocke97" width=200px><br>Aaron Gunther</a></td>
 <td>&nbsp;</td>
</tr>

</table>
