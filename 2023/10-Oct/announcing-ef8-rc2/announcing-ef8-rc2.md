---
post_title: 'EF Core 8 RC 2: Smaller features in EF8'
author1: avickers@microsoft.com
post_slug: announcing-ef8-rc2
microsoft_alias: avickers
featured_image: ef8rc2.png
categories: .NET, Entity Framework
summary: A tour through some of the smaller features release in Entity Framework Core 8 (EF8) RC 2.
desired_publication_date: 2023-10-12
tags: .net 8, ef core, efcore, ef8, entity framework
post_date: 2023-10-10 10:10:00
---

The second release candidate of Entity Framework Core (EF Core) 8 is [available on NuGet today](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/8.0.0-rc.2.23480.1)!

## Basic information

EF Core 8, or just EF8, is the successor to EF Core 7, and is scheduled for release in November 2023, at the same time as .NET 8.

EF8 requires .NET 8 and this RC 2 release should be used with the [.NET 8 RC 2 SDK](https://dotnet.microsoft.com/next).

EF8 will align with .NET 8 as a long-term support (LTS) release. See the [.NET support policy](https://dotnet.microsoft.com/platform/support/policy/dotnet-core) for more information.

## New in EF8

In this post we're going to take at a few of the smaller features included in EF8. Be sure to check out EF8 content from previous posts:

- [Complex types as value objects](https://devblogs.microsoft.com/dotnet/announcing-ef8-rc1/)
- [Primitive collections and improved Contains](https://devblogs.microsoft.com/dotnet/announcing-ef8-preview-4/)
- [Raw SQL queries for unmapped types](https://devblogs.microsoft.com/dotnet/announcing-ef8-preview-1/#raw-sql-queries-for-unmapped-types)
- [Lazy-loading for no-tracking queries](https://devblogs.microsoft.com/dotnet/announcing-ef8-preview-1/#lazy-loading-for-no-tracking-queries)
- [DateOnly/TimeOnly supported on SQL Server](https://devblogs.microsoft.com/dotnet/announcing-ef8-preview-1/#dateonly-timeonly-supported-on-sql-server)
- [SQL Server HierarchyId](https://devblogs.microsoft.com/dotnet/announcing-ef8-preview-2/#sql-server-hierarchyid)
- [JSON Columns for SQLite](https://devblogs.microsoft.com/dotnet/announcing-ef8-preview-2/#json-columns-for-sqlite)

## Sentinel values and database defaults

Databases allow columns to be configured to generate a default value if no value is provided when inserting a row. This can be represented in EF using `HasDefaultValue` for constants:

```csharp
b.Property(e => e.Status).HasDefaultValue("Hidden");
```

Or `HasDefaultValueSql` for arbitrary SQL clauses:

```csharp
b.Property(e => e.LeaseDate).HasDefaultValueSql("getutcdate()");
```

In order for EF to make use of this, it must determine when and when not to send a value for the column. By default, EF uses the CLR default as a sentinel for this. That is, when the value of `Status` or `LeaseDate` in the examples above are the CLR defaults for these types, then EF _interprets that to mean that the property has not been set_, and so does not send a value to the database. This works well for reference types--for example, if the `string` property `Status` is `null`, then EF doesn't send `null` to the database, but rather does not include any value so that the database default (`"Hidden"`) is used. Likewise, for the `DateTime` property `LeaseDate`, EF will not insert the CLR default value of `1/1/0001 12:00:00 AM`, but will instead omit this value so that database default is used.

However, in some cases the CLR default value is a valid value to insert. EF8 handles this by allowing the sentinel value for a colum to change. For example, consider an integer column configured with a database default:

```csharp
b.Property(e => e.Credits).HasDefaultValueSql(10);
```

In this case, we want the new entity to be inserted with the given number of credits, unless this is not specified, in which case 10 credits are assigned. However, this means that inserting a record with zero credits is not possible, since zero is the CLR default, and hence will cause EF to send no value. In EF8, this can be fixed by changing the sentinel for the property from zero (the CLR default) to `-1`:

```csharp
b.Property(e => e.Credits).HasDefaultValueSql(10).HasSentinel(-1);
```

EF will now only use the database default if `Credits` is set to `-1`; a value of zero will be inserted like any other amount.

Check out the [.NET Data Community Standup](https://www.youtube.com/live/GGDv_p4LAL8?si=6kbx53JjCH0OB_f6) for a more in-dept discussion on this subject including examples showing how sentinels can help prevent bugs with `enum` and `bool` values.

### Tip: using a nullable backing field

Another way to handle the same problem is to use a nullable backing field for the property. For example, instead of defining the `Credits` property as:

```csharp
public class User
{
    public int Credits { get; set; }
}
```

It can be defined as:

```csharp
public class User
{
    private int? _credits;

    public int Credits
    {
        get => _credits ?? 0;
        set => _credits = value;
    }
}
```

The backing field here will remain null _unless the property setter is actually called_. That is, the value of the backing field is a better indication of whether the property has been set or not than the CLR default of the property. This works out-of-the box with EF, since EF will use the backing field to read and write the property by default.

## Better ExecuteUpdate and ExecuteDelete

SQL commands that perform updates and deletes, such as those generated by `ExecuteUpdate` and `ExecuteDelete` methods, must target a single database table. However, in EF7, `ExecuteUpdate` and `ExecuteDelete` did not support updates accessing multiple entity types _even when the query ultimately affected a single table_. EF8 removes this limitation. For example, consider a `Customer` entity type with `CustomerInfo` owned type:

```csharp
[Table("Customers")]
public class Customer
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required CustomerInfo CustomerInfo { get; set; }
}

[Owned]
public class CustomerInfo
{
    public string? Tag { get; set; }
}
```

Both of these entity types map to the `Customers` table. However, the following bulk update fails on EF7 because it uses both entity types:

```csharp
await context.Customers
    .Where(e => e.Name == name)
    .ExecuteUpdateAsync(
        s => s.SetProperty(b => b.CustomerInfo.Tag, "Tagged")
            .SetProperty(b => b.Name, b => b.Name + "_Tagged"));
```

In EF8, this now translates to the following SQL when using Azure SQL:

```sql
UPDATE [c]
SET [c].[Name] = [c].[Name] + N'_Tagged',
    [c].[CustomerInfo_Tag] = N'Tagged'
FROM [Customers] AS [c]
WHERE [c].[Name] = @__name_0
```

Check out the [.NET Data Community Standup](https://www.youtube.com/live/GGDv_p4LAL8?si=6kbx53JjCH0OB_f6) for other examples using `Union` queries and TPT inheritance mapping.

## Better use of `IN` queries

When the Contains operator is used with a subquery, EF Core now generates better queries using SQL `IN` instead of `EXISTS`; aside from producing more readable SQL, in some cases this can result in dramatically faster queries. For example, consider the following LINQ query:

```csharp
var blogsWithPosts = await context.Blogs
    .Where(b => context.Posts.Select(p => p.BlogId).Contains(b.Id))
    .ToListAsync();
```

EF7 generates the following for PostgreSQL:

```sql
SELECT b."Id", b."Name"
      FROM "Blogs" AS b
      WHERE EXISTS (
          SELECT 1
          FROM "Posts" AS p
          WHERE p."BlogId" = b."Id")
```

Since the subquery references the external `Blogs` table (via `b."Id"`), this is a *correlated subquery*, meaning that the `Posts` subquery must be executed for each row in the `Blogs` table. In EF8, the following SQL is generated instead:

```sql
SELECT b."Id", b."Name"
      FROM "Blogs" AS b
      WHERE b."Id" IN (
          SELECT p."BlogId"
          FROM "Posts" AS p
      )
```

Since the subquery no longer references `Blogs`, it can be evaluated once, yielding massive performance improvements on most database systems. However, some database systems, most notably SQL Server, the database is able to optimize the first query to the second query so that the performance is the same.

Check out the [.NET Data Community Standup](https://www.youtube.com/live/GGDv_p4LAL8?si=6kbx53JjCH0OB_f6) for discussion and additional examples of `IN` translations.

## Numeric rowversions for SQL Azure/SQL Server

SQL Server automatic [optimistic concurrency](https://learn.microsoft.com/ef/core/saving/concurrency) is handled using [`rowversion` columns](https://learn.microsoft.com/sql/t-sql/data-types/rowversion-transact-sql). A `rowversion` is an 8-byte opaque value passed between database, client, and server. By default, SqlClient exposes `rowversion` types as `byte[]`, despite mutable reference types being a bad match for `rowversion` semantics. In EF8, it is easy instead map `rowversion` columns to `long` or `ulong` properties. For example:

```csharp
modelBuilder.Entity<Blog>()
    .Property(e => e.RowVersion)
    .HasConversion<byte[]>()
    .IsRowVersion();
```

## Parentheses elimination

Generating readable SQL is an important goal for EF Core. In EF8, the generated SQL is more readable through automatic elimination of unneeded parenthesis. For example, the following LINQ query:

```csharp
await ctx.Customers  
    .Where(c => c.Id * 3 + 2 > 0 && c.FirstName != null || c.LastName != null)  
    .ToListAsync();  
```

Translates to the following Azure SQL when using EF7:

```sql
SELECT [c].[Id], [c].[City], [c].[FirstName], [c].[LastName], [c].[Street]
FROM [Customers] AS [c]
WHERE ((([c].[Id] * 3) + 2) > 0 AND ([c].[FirstName] IS NOT NULL)) OR ([c].[LastName] IS NOT NULL)
```

Which has been improved to the following when using EF8:

```sql
SELECT [c].[Id], [c].[City], [c].[FirstName], [c].[LastName], [c].[Street]
FROM [Customers] AS [c]
WHERE ([c].[Id] * 3 + 2 > 0 AND [c].[FirstName] IS NOT NULL) OR [c].[LastName] IS NOT NULL
```
## Everything in EF8

Overall, EF8 RC 2 contains all the major feature features we intend to ship in EF8, although further tweaks and bug fixes are coming for GA. These features include:

- [Allow Multi-region or Application Preferred Regions in EF Core Cosmos](https://github.com/dotnet/efcore/issues/29424)
- [Use C# structs or classes as value objects](https://github.com/dotnet/efcore/issues/9906)
- [Support primitive collections in the compiled model](https://github.com/dotnet/efcore/issues/31489)
- [Migrations and model snapshot for primitive collections](https://github.com/dotnet/efcore/issues/31414)
- [Query: add support for projecting JSON entities that have been composed on](https://github.com/dotnet/efcore/issues/31365)
- [SQLite: Add EF.Functions.Unhex](https://github.com/dotnet/efcore/issues/31355)
- [Add type mapping APIs to customize JSON value serialization/deserialization](https://github.com/dotnet/efcore/issues/30677)
- [SQL Server Index options SortInTempDB and DataCompression](https://github.com/dotnet/efcore/issues/30408)
- [Analyzer: warn (and code fix) for use of interpolation in SQL methods accepting raw strings](https://github.com/dotnet/efcore/issues/30965)
- [Translate Contains to IN with subquery instead of EXISTS where relevant](https://github.com/dotnet/efcore/issues/30955)
- [Allow inline primitive collections with parameters, translating to VALUES](https://github.com/dotnet/efcore/issues/30732)
- [Translate DateOnly.FromDateTime](https://github.com/dotnet/efcore/issues/30708)
- [Implement JSON serialization/deserialization via Utf8JsonReader/Utf8JsonWriter](https://github.com/dotnet/efcore/issues/30604)
- [Update pattern for scaffolding column default constraints](https://github.com/dotnet/efcore/issues/13613)
- [Use IN instead of EXISTS with ExecuteDelete and entity containment](https://github.com/dotnet/efcore/issues/31386)
- [Allow ExecuteUpdate to update properties of multiple queries as long as the map to a single table](https://github.com/dotnet/efcore/issues/31406)
- [Query: add support for projecting primitive collections from JSON entities](https://github.com/dotnet/efcore/issues/31364)
- [Switch to storing enums as ints in JSON instead of strings](https://github.com/dotnet/efcore/issues/31100)
- [Translate DegreesToRadians](https://github.com/dotnet/efcore/issues/30926)
- [Metadata and type mapping support for primitive collections](https://github.com/dotnet/efcore/issues/30730)
- [JSON type representations and conversions to store types](https://github.com/dotnet/efcore/issues/30727)
- [Allow stripping away all model building code to reduce application size](https://github.com/dotnet/efcore/issues/29755)
- [Json: add support for collection of primitive types inside JSON columns](https://github.com/dotnet/efcore/issues/28688)
- [Support LINQ querying of non-primitive collections within JSON](https://github.com/dotnet/efcore/issues/28616)
- [SQLite RevEng: Sample data to determine CLR type](https://github.com/dotnet/efcore/issues/8824)
- [Allow default value check in value generation to be customized](https://github.com/dotnet/efcore/issues/701)
- [Update handling of non-nullable store-generated properties](https://github.com/dotnet/efcore/issues/15070)
- [IN() list queries are not parameterized, causing increased SQL Server CPU usage](https://github.com/dotnet/efcore/issues/13617)
- [Allow 'unsharing' connection between contexts](https://github.com/dotnet/efcore/issues/30704)
- [Remove unneeded subquery and projection when using ordering without limit/offset in set operations](https://github.com/dotnet/efcore/issues/30684)
- [Make SequentialGuidValueGenerator non-allocating](https://github.com/dotnet/efcore/issues/30610)
- [Support querying over primitive collections](https://github.com/dotnet/efcore/issues/30426)
- [JSON/Sqlite: use -> and ->> where possible when traversing JSON, rather than json_extract](https://github.com/dotnet/efcore/issues/30334)
- [Add Generic version of EntityTypeConfiguration Attribute](https://github.com/dotnet/efcore/issues/30072)
- [NativeAOT/trimming compatibility for Microsoft.Data.Sqlite](https://github.com/dotnet/efcore/issues/29725)
- [Map collections of primitive types to JSON column in relational database](https://github.com/dotnet/efcore/issues/29427)
- [Translate DateTimeOffset.ToUnixTime(Seconds|Milliseconds)](https://github.com/dotnet/efcore/issues/28925)
- [Allow pooling DbContext with singleton services](https://github.com/dotnet/efcore/issues/27752)
- [Optional RestartSequenceOperation.StartValue](https://github.com/dotnet/efcore/issues/26560)
- [Generate compiled relational model](https://github.com/dotnet/efcore/issues/24896)
- [Global query filters produce too many parameters](https://github.com/dotnet/efcore/issues/24476)
- [Optimize update path for single property JSON element](https://github.com/dotnet/efcore/issues/30410)
- [JSON columns can be used in compiled models](https://github.com/dotnet/efcore/issues/29602)
- [Unneeded parentheses removed in SQL queries ](https://github.com/dotnet/efcore/issues/26767)
- [Set operations are supported over non-entity projections with different facets](https://github.com/dotnet/efcore/issues/19129)
- [Json: add support for Sqlite provider](https://github.com/dotnet/efcore/issues/28816)
- [SQL Server: Support hierarchyid](https://github.com/dotnet/efcore/issues/365)
- [Configuration to opt out of occasionally problematic SaveChanges optimizations](https://github.com/dotnet/efcore/issues/29916)
- [Add convention types for triggers](https://github.com/dotnet/efcore/issues/28687)
- [Translate element access of a JSON array](https://github.com/dotnet/efcore/issues/28648)
- [Raw SQL queries for unmapped types](https://learn.microsoft.com/ef/core/what-is-new/ef-core-8.0/plan#sql-queries-for-unmapped-types)
- [Support the new BCL DateOnly and TimeOnly structs for SQL Server](https://github.com/dotnet/efcore/issues/24507)
- [Translate ElementAt(OrDefault)](https://github.com/dotnet/efcore/issues/17066)
- [Opt-out of lazy-loading for specific navigations](https://github.com/dotnet/efcore/issues/10787)
- [Lazy-loading for no-tracking queries](https://github.com/dotnet/efcore/issues/10042)
- [Reverse engineer Synapse and Dynamics 365 TDS](https://github.com/dotnet/efcore/issues/29121)
- [Set MaxLength on TPH discriminator property by convention](https://github.com/dotnet/efcore/issues/10691)
- [Translate ToString() on a string column](https://github.com/dotnet/efcore/issues/20839)
- [Generic overload of ConventionSetBuilder.Remove](https://github.com/dotnet/efcore/issues/29476)
- [Lookup tracked entities by primary key, alternate key, or foreign key](https://github.com/dotnet/efcore/issues/29685)
- [Allow UseSequence and HiLo on non-key properties](https://github.com/dotnet/efcore/issues/29758)
- [Pass query tracking behavior to materialization interceptor](https://github.com/dotnet/efcore/issues/29910)
- [Use case-insensitive string key comparisons on SQL Server](https://github.com/dotnet/efcore/issues/27526)
- [Allow value converters to change the DbType](https://github.com/dotnet/efcore/issues/24771)
- [Resolve application services in EF services](https://github.com/dotnet/efcore/issues/13540)
- [Numeric rowersion properties automatically convert to binary](https://github.com/dotnet/efcore/issues/12434)
- [Allow transfer of ownership of DbConnection from application to DbContext](https://github.com/dotnet/efcore/issues/24199)
- [Provide more information when 'No DbContext was found' error is generated](https://github.com/dotnet/efcore/issues/18715)

## How to get EF8 RC 2

EF8 is distributed exclusively as a set of NuGet packages. For example, to add the SQL Server provider to your project, you can use the following command using the dotnet tool:

```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.0-rc.2.23480.1
```

## Installing the EF8 Command Line Interface (CLI)

The `dotnet-ef` tool must be installed before executing EF8 Core migration or scaffolding commands.

To install the tool globally, use:

```bash
dotnet tool install --global dotnet-ef --version 8.0.0-rc.2.23480.1
```

If you already have the tool installed, you can upgrade it with the following command:

```bash
dotnet tool update --global dotnet-ef --version 8.0.0-rc.2.23480.1
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
- What's New in EF Core 8: [aka.ms/ef8-new](https://aka.ms/ef8-new)
- What's New in EF Core 7: [aka.ms/ef7-new](https://aka.ms/ef7-new)
- Issues and feature requests for EF Core: [github.com/dotnet/efcore/issues](https://github.com/dotnet/efcore/issues)
- Entity Framework Roadmap: [aka.ms/efroadmap](https://aka.ms/efroadmap)
