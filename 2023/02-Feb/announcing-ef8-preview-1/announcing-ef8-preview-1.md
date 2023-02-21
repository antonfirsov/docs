---
post_title: 'EF Core 8 Preview 1: Raw, lazy, and on-time'
author1: avickers
post_slug: announcing-ef8-preview-1
microsoft_alias: avickers
featured_image: ef8p1.png
categories: .NET, Entity Framework
summary: Announcing Entity Framework Core 8 (EF8)Preview 1 with raw SQL queries, lazy-loading, DateOnly/TimeOnly and more!
desired_publication_date: 2023-02-21
tags: .net 8, ef core, efcore, ef8, entity framework
post_date: 2023-02-21 10:00:00
---

The first preview of Entity Framework Core (EF Core) 8 is [available on NuGet today](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/8.0.0-preview.1.23111.4)!

## Basic information

EF Core 8, or just EF8, is the successor to EF Core 7, and is scheduled for release in November 2023, at the same time as .NET 8.

EF8 currently targets .NET 6. This will likely be updated to .NET 8 as we near release.

EF8 will align with .NET 8 as a long-term support (LTS) release. See the [.NET support policy](https://dotnet.microsoft.com/platform/support/policy/dotnet-core) for more information.

## EF8 themes

Before looking at the new features in preview 1, let's take a quick look at the plan for large investments in data access for .NET 8. These investments are covered by five themes:

### Highly requested features

- [JSON columns](https://learn.microsoft.com/ef/core/what-is-new/ef-core-8.0/plan#json-columns)
  Build on EF7 JSON support to further power the document/relational hybrid pattern.
- [Value objects](https://learn.microsoft.com/ef/core/what-is-new/ef-core-8.0/plan#value-objects)
  Applications can use DDD-style value objects in EF models.
- [SQL queries for unmapped types](https://learn.microsoft.com/ef/core/what-is-new/ef-core-8.0/plan#sql-queries-for-unmapped-types)
  Applications can execute more types of SQL query without dropping down to ADO.NET or using third-party libraries.

### Cloud native and devices

- [AOT and trimming with EF Core](https://learn.microsoft.com/ef/core/what-is-new/ef-core-8.0/plan#aot-and-trimming-with-ef-core)
  Small, fast-starting EF Core applications with no dynamic code generation.
- [AOT and trimming for ADO.NET](https://learn.microsoft.com/ef/core/what-is-new/ef-core-8.0/plan#aot-and-trimming-for-adonet)
  Low-level data access can be used in cloud native applications.

### Performance

- [Woodstar](https://learn.microsoft.com/ef/core/what-is-new/ef-core-8.0/plan#woodstar)
  Fast, fully managed access to SQL Server and Azure SQL for .NET applications.

### Visual Tooling

- [First-class T4 templates in Visual Studio](https://learn.microsoft.com/ef/core/what-is-new/ef-core-8.0/plan#first-class-t4-templates-in-visual-studio)
  Leverage T4 templating across multiple areas in Visual Studio.
- [EF Core Database First in Visual Studio](https://learn.microsoft.com/ef/core/what-is-new/ef-core-8.0/plan#ef-core-database-first-in-visual-studio)
  Out-of-the-box Database First tooling in Visual Studio.

### Developer experience

- [Make EF Core better](https://learn.microsoft.com/ef/core/what-is-new/ef-core-8.0/plan#theme-developer-experience)
  Improve the developer experience be making many small improvements to EF Core

### Find out more and give feedback

Your feedback on planning is important. Please comment on [GitHub Issue #29853](https://github.com/dotnet/efcore/issues/29853) with any feedback or general suggestions about the plan. The best way to indicate the importance of an issue is to vote (👍) for that [issue on GitHub](https://github.com/dotnet/efcore/issues). This data will then feed into the [planning process](xref:core/what-is-new/release-planning) for the next release.

## New in EF8 preview 1

The following sections give an overview of three particular enhancements available in EF8 preview 1: Raw SQL queries for unmapped types, lazy-loading for no-tracking queries, and `DateOnly`/`TimeOnly` support for SQL Server/Azure SQL. In total, EF8 preview 1 ships with [more than 60 improvements, both bug fixes and enhancements](https://github.com/dotnet/efcore/issues?q=is%3Aissue+milestone%3A8.0.0-preview1+is%3Aclosed).

> **TIP**
> Full details of all new EF8 features can be found in the [What's New in EF8](https://learn.microsoft.com/ef/core/what-is-new/ef-core-8.0/whatsnew) documentation. All the code is available in [runnable samples on GitHub](https://github.com/dotnet/EntityFramework.Docs).

### Raw SQL queries for unmapped types

EF7 introduced [raw SQL queries returning scalar types](https://learn.microsoft.com/ef/core/querying/sql-queries#querying-scalar-non-entity-types). This is enhanced in EF8 to include raw SQL queries returning any mappable CLR type, without including that type in the EF model.

> **TIP**
> The code shown here comes from [RawSqlSample.cs](https://github.com/dotnet/EntityFramework.Docs/tree/main/samples/core/Miscellaneous/NewInEFCore8/RawSqlSample.cs).

Queries using unmapped types are executed using [SqlQuery](https://learn.microsoft.com/dotnet/api/microsoft.entityframeworkcore.relationaldatabasefacadeextensions.sqlquery) or [SqlQueryRaw](https://learn.microsoft.com/dotnet/api/microsoft.entityframeworkcore.relationaldatabasefacadeextensions.sqlqueryraw). The former uses string interpolation to parameterize the query, which helps ensure that all non-constant values are parameterized. For example, consider the following database table:

```sql
CREATE TABLE [Posts] (
    [Id] int NOT NULL IDENTITY,
    [Title] nvarchar(max) NOT NULL,
    [Content] nvarchar(max) NOT NULL,
    [PublishedOn] date NOT NULL,
    [BlogId] int NOT NULL,
);
```

`SqlQuery` can be used to query this table and return instances of a `BlogPost` type with properties corresponding to the columns in the table:

For example:

```csharp
public class BlogPost
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateOnly PublishedOn { get; set; }
    public int BlogId { get; set; }
}
```

For example:

```csharp
var start = new DateOnly(2022, 1, 1);
var end = new DateOnly(2023, 1, 1);
var postsIn2022 =
    await context.Database
        .SqlQuery<BlogPost>($"SELECT * FROM Posts as p WHERE p.PublishedOn >= {start} AND p.PublishedOn < {end}")
        .ToListAsync();
```

When using SQL Server, this query is parameterized and executed as:

```sql
SELECT * FROM Posts as p WHERE p.PublishedOn >= @p0 AND p.PublishedOn < @p1
```

The type used for query results can contain common mapping constructs supported by EF Core, such as parameterized constructors and mapping attributes. For example:

```csharp
public class BlogPost
{
    public BlogPost(string blogTitle, string content, DateOnly publishedOn)
    {
        BlogTitle = blogTitle;
        Content = content;
        PublishedOn = publishedOn;
    }

    public int Id { get; private set; }

    [Column("Title")]
    public string BlogTitle { get; set; }

    public string Content { get; set; }
    public DateOnly PublishedOn { get; set; }
    public int BlogId { get; set; }
}
```

> **NOTE**
> Types used in this way do not have keys defined and cannot have relationships to other types. Types with relationships must be mapped in the model.

One nice feature of `SqlQuery` is that it returns an `IQueryable` which can be composed on using LINQ. For example, a 'Where' clause can be added to the query above:

```csharp
var summariesIn2022 =
    await context.Database.SqlQuery<PostSummary>(
            @$"SELECT b.Name AS BlogName, p.Title AS PostTitle, p.PublishedOn
               FROM Posts AS p
               INNER JOIN Blogs AS b ON p.BlogId = b.Id")
        .Where(p => p.PublishedOn >= cutoffDate && p.PublishedOn < end)
        .ToListAsync();
```

This is executed as:

```sql
SELECT [n].[BlogName], [n].[PostTitle], [n].[PublishedOn]
FROM (
         SELECT b.Name AS BlogName, p.Title AS PostTitle, p.PublishedOn
         FROM Posts AS p
                  INNER JOIN Blogs AS b ON p.BlogId = b.Id
     ) AS [n]
WHERE [n].[PublishedOn] >= @__cutoffDate_1 AND [n].[PublishedOn] < @__end_2
```

At this point it is worth remembering that all of the above can be done completely in LINQ without the need to write any SQL. This includes returning instances of an unmapped type like `PostSummary`. For example, the preceding query can be written in LINQ as:

```csharp
var summariesByLinq =
    await context.Posts.Select(
            p => new PostSummary
            {
                BlogName = p.Blog.Name,
                PostTitle = p.Title,
                PublishedOn = p.PublishedOn,
            })
        .Where(p => p.PublishedOn >= start && p.PublishedOn < end)
        .ToListAsync();
```

Which translates to much cleaner SQL:

```sql
SELECT [b].[Name] AS [BlogName], [p].[Title] AS [PostTitle], [p].[PublishedOn]
FROM [Posts] AS [p]
INNER JOIN [Blogs] AS [b] ON [p].[BlogId] = [b].[Id]
WHERE [p].[PublishedOn] >= @__start_0 AND [p].[PublishedOn] < @__end_1
```

> **TIP**
> EF is able to generate cleaner SQL when it is responsible for the entire query than it is when composing over user-supplied SQL because, in the former case, the full semantics of the query are available to EF.

So far, all the queries have been executed directly against tables. `SqlQuery` can also be used to return results from a view without mapping the view type in the EF model. For example:

```csharp
var summariesFromView =
    await context.Database.SqlQuery<PostSummary>(
            @$"SELECT * FROM PostAndBlogSummariesView")
        .Where(p => p.PublishedOn >= cutoffDate && p.PublishedOn < end)
        .ToListAsync();
```

Likewise, `SqlQuery` can be used for the results of a function:

```csharp
var summariesFromFunc =
    await context.Database.SqlQuery<PostSummary>(
            @$"SELECT * FROM GetPostsPublishedAfter({cutoffDate})")
        .Where(p => p.PublishedOn < end)
        .ToListAsync();
```

The returned `IQueryable` can be composed upon when it is the result of a view or function, just like it can be for the result of a table query. Stored procedures can be also be executed using `SqlQuery`, but most databases do not support further composition. For example:

```csharp
var summariesFromStoredProc =
    await context.Database.SqlQuery<PostSummary>(
            @$"exec GetRecentPostSummariesProc")
        .ToListAsync();
```

### Lazy-loading for no-tracking queries

EF8 adds support for [lazy-loading of navigations](https://learn.microsoft.com/ef/core/querying/related-data/lazy) on entities that are not being tracked by the `DbContext`. This means a no-tracking query can be followed by lazy-loading of navigations on the entities returned by the no-tracking query.

> **TIP**
> The code for the lazy-loading examples shown below comes from [LazyLoadingSample.cs](https://github.com/dotnet/EntityFramework.Docs/tree/main/samples/core/Miscellaneous/NewInEFCore8/LazyLoadingSample.cs).

For example, consider a no-tracking query for blogs:

```csharp
var blogs = await context.Blogs.AsNoTracking().ToListAsync();
```

If `Blog.Posts` is configured for lazy-loading, for example, using lazy-loading proxies, then accessing `Posts` will cause it to load from the database:

```csharp
Console.WriteLine();
Console.Write("Choose a blog: ");
if (int.TryParse(ReadLine(), out var blogId))
{
    Console.WriteLine("Posts:");
    foreach (var post in blogs[blogId - 1].Posts)
    {
        Console.WriteLine($"  {post.Title}");
    }
}
```

EF8 also reports whether or not a given navigation is loaded for entities not tracked by the context. For example:

```csharp
foreach (var blog in blogs)
{
    if (context.Entry(blog).Collection(e => e.Posts).IsLoaded)
    {
        Console.WriteLine($" Posts for blog '{blog.Name}' are loaded.");
    }
}
```

There are a few important considerations when using lazy-loading in this way:

- Lazy-loading will only succeed until the `DbContext` used to query the entity is disposed.
- Entities queried in this way a reference to their `DbContext`, even though they are not tracked by it. Care should be taken to avoid memory leaks if the entity instances will have long lifetimes.
- Explicitly detaching the entity by setting its state to `EntityState.Detached` severs the reference to the `DbContext` and lazy-loading will no longer work.
- Remember that all lazy-loading uses synchronous I/O, since there is no way to access a property in an asynchronous manner.

Lazy-loading from untracked entities works for both [lazy-loading proxies](https://learn.microsoft.com/ef/core/querying/related-data/lazy#lazy-loading-with-proxies) and [lazy-loading without proxies](https://learn.microsoft.com/ef/core/querying/related-data/lazy#lazy-loading-without-proxies).

### DateOnly/TimeOnly supported on SQL Server

The [System.DateOnly](https://learn.microsoft.com/dotnet/api/system.dateonly) and [System.TimeOnly](https://learn.microsoft.com/dotnet/api/system.timeonly) types were introduced in .NET 6 and have been supported for several database providers (e.g. SQLite, MySQL, and PostgreSQL) since their introduction. For SQL Server, the recent release of a [Microsoft.Data.SqlClient](https://www.nuget.org/packages/Microsoft.Data.SqlClient/) package targeting .NET 6 has allowed [ErikEJ to add support for these types at the ADO.NET level](https://github.com/dotnet/SqlClient/pull/1813). This in turn paved the way for support in EF8 for `DateOnly` and `TimeOnly` as properties in entity types.

> **TIP**
> `DateOnly` and `TimeOnly` can be used in EF Core 6 and 7 using the [ErikEJ.EntityFrameworkCore.SqlServer.DateOnlyTimeOnly](https://www.nuget.org/packages/ErikEJ.EntityFrameworkCore.SqlServer.DateOnlyTimeOnly) community package from [@ErikEJ](https://github.com/ErikEJ).

For example, consider the following EF model for British schools:

```csharp
public class School
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public DateOnly Founded { get; set; }
    public List<Term> Terms { get; } = new();
    public List<OpeningHours> OpeningHours { get; } = new();
}

public class Term
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public DateOnly FirstDay { get; set; }
    public DateOnly LastDay { get; set; }
    public School School { get; set; } = null!;
}

[Owned]
public class OpeningHours
{
    public OpeningHours(DayOfWeek dayOfWeek, TimeOnly? opensAt, TimeOnly? closesAt)
    {
        DayOfWeek = dayOfWeek;
        OpensAt = opensAt;
        ClosesAt = closesAt;
    }

    public DayOfWeek DayOfWeek { get; private set; }
    public TimeOnly? OpensAt { get; set; }
    public TimeOnly? ClosesAt { get; set; }
}
```

> **TIP**
> The code shown here comes from [DateOnlyTimeOnlySample.cs](https://github.com/dotnet/EntityFramework.Docs/tree/main/samples/core/Miscellaneous/NewInEFCore8/DateOnlyTimeOnlySample.cs).

> **NOTE**
> This model represents only British schools and stores times as local (GMT) times. Handling different timezones would complicate this code significantly. Note that using `DateTimeOffset` would not help here, since opening and closing times have different offsets depending whether daylight saving time is active or not.

These entity types map to the following tables when using SQL Server or Azure SQL. Notice that the `DateOnly` properties map to `date` columns, and the `TimeOnly` properties map to `time` columns.

```sql
CREATE TABLE [Schools] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [Founded] date NOT NULL,
    CONSTRAINT [PK_Schools] PRIMARY KEY ([Id]));

CREATE TABLE [OpeningHours] (
    [SchoolId] int NOT NULL,
    [Id] int NOT NULL IDENTITY,
    [DayOfWeek] int NOT NULL,
    [OpensAt] time NULL,
    [ClosesAt] time NULL,
    CONSTRAINT [PK_OpeningHours] PRIMARY KEY ([SchoolId], [Id]),
    CONSTRAINT [FK_OpeningHours_Schools_SchoolId] FOREIGN KEY ([SchoolId]) REFERENCES [Schools] ([Id]) ON DELETE CASCADE);

CREATE TABLE [Term] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [FirstDay] date NOT NULL,
    [LastDay] date NOT NULL,
    [SchoolId] int NOT NULL,
    CONSTRAINT [PK_Term] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Term_Schools_SchoolId] FOREIGN KEY ([SchoolId]) REFERENCES [Schools] ([Id]) ON DELETE CASCADE);
```

Queries using `DateOnly` and `TimeOnly` work in the expected manner. For example, the following LINQ query finds schools that are currently open:

```csharp
openSchools = await context.Schools
    .Where(
        s => s.Terms.Any(
                 t => t.FirstDay <= today
                      && t.LastDay >= today)
             && s.OpeningHours.Any(
                 o => o.DayOfWeek == dayOfWeek
                      && o.OpensAt < time && o.ClosesAt >= time))
    .ToListAsync();
```

This query translates to the following SQL, as shown by calling [ToQueryString()](https://learn.microsoft.com/dotnet/api/microsoft.entityframeworkcore.entityframeworkqueryableextensions.toquerystring) on the IQueryable object:

```sql
DECLARE @__today_0 date = '2023-02-07';
DECLARE @__dayOfWeek_1 int = 2;
DECLARE @__time_2 time = '19:53:40.4798052';

SELECT [s].[Id], [s].[Founded], [s].[Name], [o0].[SchoolId], [o0].[Id], [o0].[ClosesAt], [o0].[DayOfWeek], [o0].[OpensAt]
FROM [Schools] AS [s]
LEFT JOIN [OpeningHours] AS [o0] ON [s].[Id] = [o0].[SchoolId]
WHERE EXISTS (
    SELECT 1
    FROM [Term] AS [t]
    WHERE [s].[Id] = [t].[SchoolId] AND [t].[FirstDay] <= @__today_0 AND [t].[LastDay] >= @__today_0) AND EXISTS (
    SELECT 1
    FROM [OpeningHours] AS [o]
    WHERE [s].[Id] = [o].[SchoolId] AND [o].[DayOfWeek] = @__dayOfWeek_1 AND [o].[OpensAt] < @__time_2 AND [o].[ClosesAt] >= @__time_2)
ORDER BY [s].[Id], [o0].[SchoolId]
```

## How to get EF8 preview 1

EF8 is distributed exclusively as a set of NuGet packages. For example, to add the SQL Server provider to your project, you can use the following command using the dotnet tool:

```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.0-preview.1.23111.4
```

## Installing the EF8 Command Line Interface (CLI)

The `dotnet-ef` tool must be installed before executing EF8 Core migration or scaffolding commands.

To install the tool globally, use:

```bash
dotnet tool install --global dotnet-ef --version 8.0.0-preview.1.23111.4
```

If you already have the tool installed, you can upgrade it with the following command:

```bash
dotnet tool update --global dotnet-ef --version 8.0.0-preview.1.23111.4
```

## The .NET Data Community Standup

The .NET data access team is now live streaming every other Wednesday at 10am Pacific Time, 1pm Eastern Time, or 18:00 UTC. Join the stream learn and ask questions about many .NET Data related topics.

- [Watch our YouTube playlist](https://aka.ms/efstandups) of previous shows
- [Visit the .NET Community Standup](https://live.dot.net) page to preview upcoming shows
- [Submit your ideas](https://github.com/dotnet/efcore/issues/22700) for a guest, product, demo, or other content to cover

## Documentation and Feedback

The starting point for all EF Core documentation is [docs.microsoft.com/ef/](https://docs.microsoft.com/ef/). Please file issues found and any other feedback on the [dotnet/efcore GitHub repo](https://github.com/dotnet/efcore).

## Helpful Links

The following links are provided for easy reference and access.

- EF Core Community Standup Playlist: [aka.ms/efstandups](https://aka.ms/efstandups)
- Main documentation: [aka.ms/efdocs](https://aka.ms/efdocs)
- What's New in EF Core 8: [aka.ms/ef7-new](https://aka.ms/ef7-new)
- What's New in EF Core 7: [aka.ms/ef8-new](https://aka.ms/ef8-new)
- Issues and feature requests for EF Core: [github.com/dotnet/efcore/issues](https://github.com/dotnet/efcore/issues)
- Entity Framework Roadmap: [aka.ms/efroadmap](https://aka.ms/efroadmap)
- Bi-weekly updates: [aka.ms/ef-news](https://aka.ms/ef-news)
