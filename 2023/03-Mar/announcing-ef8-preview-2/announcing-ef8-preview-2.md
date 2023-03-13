---
post_title: 'EF Core 8 Preview 2: Lite and familiar'
author1: avickers@microsoft.com
post_slug: announcing-ef8-preview-2
microsoft_alias: avickers
featured_image: ef8p2.png
categories: .NET, Entity Framework
summary: Announcing Entity Framework Core 8 (EF8) Preview 2 with support for JSON columns in SQLite and HierarchyId in SQL Server/Azure SQL
desired_publication_date: 2023-03-14
tags: .net 8, ef core, efcore, ef8, entity framework
post_date: 2023-03-14 10:00:00
---

The second preview of Entity Framework Core (EF Core) 8 is [available on NuGet today](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/8.0.0-preview.2.23128.3)!

## Basic information

EF Core 8, or just EF8, is the successor to EF Core 7, and is scheduled for release in November 2023, at the same time as .NET 8.

EF8 currently targets .NET 6. This will likely be updated to .NET 8 as we near release.

EF8 will align with .NET 8 as a long-term support (LTS) release. See the [.NET support policy](https://dotnet.microsoft.com/platform/support/policy/dotnet-core) for more information.

## New in EF8 Preview 2

The following sections give an overview of two exciting enhancements available in EF8 Preview 2: support for JSON columns in SQLite databases and HierarchyId in SQL Server/Azure SQL databases. EF8 Preview 2 also ships several [smaller bug fixes and enhancements](https://github.com/dotnet/efcore/issues?q=is%3Aissue+milestone%3A8.0.0-preview2+is%3Aclosed), as well as [more than 60 bug fixes and enhancements from preview 1](https://github.com/dotnet/efcore/issues?q=is%3Aissue+milestone%3A8.0.0-preview1+is%3Aclosed).

> **TIP**
> Full details of all new EF8 features can be found in the [What's New in EF8](https://learn.microsoft.com/ef/core/what-is-new/ef-core-8.0/whatsnew) documentation. All the code is available in [runnable samples on GitHub](https://github.com/dotnet/EntityFramework.Docs).

### JSON Columns for SQLite

EF7 introduced support for mapping to JSON columns when using Azure SQL/SQL Server. EF8 extends this support to SQLite databases. As for the SQL Server support, this includes

- Mapping of aggregates built from .NET types to JSON documents stored in SQLite columns
- Queries into JSON columns, such as filtering and sorting by the elements of the documents
- Queries that project elements out of the JSON document into results
- Updating and saving changes to JSON documents

The existing [documentation from What's New in EF7](https://learn.microsoft.com/ef/core/what-is-new/ef-core-7.0/whatsnew#json-columns) provides detailed information on JSON mapping, queries, and updates. This documentation now also applies to SQLite.

> **TIP**
> The code shown in the EF7 documentation has been updated to also run on SQLite can can be found in [JsonColumnsSample.cs](https://github.com/dotnet/EntityFramework.Docs/tree/main/samples/core/Miscellaneous/NewInEFCore8/JsonColumnsSample.cs).

#### Queries into JSON columns

Queries into JSON columns on SQLite use the `json_extract` function. For example, the "authors in Chigley" query from the documentation referenced above:

```csharp
var authorsInChigley = await context.Authors
    .Where(author => author.Contact.Address.City == "Chigley")
    .ToListAsync();
```

Is translated to the following SQL when using SQLite:

```sql
SELECT "a"."Id", "a"."Name", "a"."Contact"
FROM "Authors" AS "a"
WHERE json_extract("a"."Contact", '$.Address.City') = 'Chigley'
```

#### Updating JSON columns

For updates, EF uses the `json_set` function on SQLite. For example, when updating a single property in a document:

```csharp
var arthur = await context.Authors.SingleAsync(author => author.Name.StartsWith("Arthur"));

arthur.Contact.Address.Country = "United Kingdom";

await context.SaveChangesAsync();
```

EF generates the following parameters:

```text
info: 3/10/2023 10:51:33.127 RelationalEventId.CommandExecuted[20101] (Microsoft.EntityFrameworkCore.Database.Command)
      Executed DbCommand (0ms) [Parameters=[@p0='["United Kingdom"]' (Nullable = false) (Size = 18), @p1='4'], CommandType='Text', CommandTimeout='30']
```

Which use the `json_set` function on SQLite:

```sql
UPDATE "Authors" SET "Contact" = json_set("Contact", '$.Address.Country', json_extract(@p0, '$[0]'))
WHERE "Id" = @p1
RETURNING 1;
```

### SQL Server HierarchyId

Azure SQL and SQL Server have a special data type called [`hierarchyid`](https://learn.microsoft.com/sql/t-sql/data-types/hierarchyid-data-type-method-reference) that is used to store [hierarchical data](https://learn.microsoft.com/sql/relational-databases/hierarchical-data-sql-server). In this case, "hierarchical data" essentially means data that forms a tree structure, where each item can have a parent and/or children. Examples of such data are:

- An organizational structure
- A file system
- A set of tasks in a project
- A taxonomy of language terms
- A graph of links between Web pages

The database is then able to run queries against this data using its hierarchical structure. For example, a query can find ancestors and dependents of given items, or find all items at a certain depth in the hierarchy.

#### HierarchyId support in .NET and EF Core

Official support for the SQL Server `hierarchyid` type has only recently come to modern .NET platforms (i.e. ".NET Core"). This support is in the form of the [Microsoft.SqlServer.Types](https://www.nuget.org/packages/Microsoft.SqlServer.Types) NuGet package, which brings in low-level SQL Server-specific types. In this case, the low-level type is called `SqlHierarchyId`.

At the next level, a new [Microsoft.EntityFrameworkCore.SqlServer.Abstractions](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer.Abstractions) package has been introduced, which includes a higher-level `HierarchyId` type intended for use in entity types.

> **TIP**
> The `HierarchyId` type is more idiomatic to the norms of .NET than `SqlHierarchyId`, which is instead modeled after how .NET Framework types are hosted inside the SQL Server database engine.  `HierarchyId` is designed to work with EF Core, but it can also be used outside of EF Core in other applications. The `Microsoft.EntityFrameworkCore.SqlServer.Abstractions` package doesn't reference any other packages, and so has minimal impact on deployed application size and dependencies.

Use of `HierarchyId` for EF Core functionality such as queries and updates requires the [Microsoft.EntityFrameworkCore.SqlServer.HierarchyId](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer.HierarchyId) package. This package brings in `Microsoft.EntityFrameworkCore.SqlServer.Abstractions` and `Microsoft.SqlServer.Types` as transitive dependencies, and so is often the only package needed. Once the package is installed, use of `HierarchyId` is enabled by calling `UseHierarchyId` as part of the application's call to `UseSqlServer`. For example:

```csharp
options.UseSqlServer(
    connectionString,
    x => x.UseHierarchyId());
```

> **NOTE**
> Unofficial support for `hierarchyid` in EF Core has been available for many years via the [EntityFrameworkCore.SqlServer.HierarchyId](https://www.nuget.org/packages/EntityFrameworkCore.SqlServer.HierarchyId) package. This package has been maintained as a collaboration between the community and the EF team. Now that there is official support for `hierarchyid` in .NET, the code from this community package forms, with the permission of the original contributors, the basis for the official package described here. Many thanks to all those involved over the years, including [@aljones](https://github.com/aljones), [@cutig3r](https://github.com/cutig3r), [@huan086](https://github.com/huan086), [@kmataru](https://github.com/kmataru), [@mehdihaghshenas](https://github.com/mehdihaghshenas), and [@vyrotek](https://github.com/vyrotek)

#### Modeling hierarchies

The `HierarchyId` type can be used for properties of an entity type. For example, assume we want to model the paternal family tree of some fictional [halflings](https://en.wikipedia.org/wiki/Halfling). In the entity type for `Halfling`, a `HierarchyId` property can be used to locate each halfling in the family tree.

```csharp
public class Halfling
{
    public Halfling(HierarchyId pathFromPatriarch, string name, int? yearOfBirth = null)
    {
        PathFromPatriarch = pathFromPatriarch;
        Name = name;
        YearOfBirth = yearOfBirth;
    }

    public int Id { get; private set; }
    public HierarchyId PathFromPatriarch { get; set; }
    public string Name { get; set; }
    public int? YearOfBirth { get; set; }
}
```

> **TIP**
> The code shown here and in the examples below comes from [HierarchyIdSample.cs](https://github.com/dotnet/EntityFramework.Docs/tree/main/samples/core/Miscellaneous/NewInEFCore8/HierarchyIdSample.cs).

> **TIP**
> If desired, `HierarchyId` is suitable for use as a key property type.

In this case, the family tree is rooted with the patriarch of the family. Each halfling can be traced from the patriarch down the tree using its `PathFromPatriarch` property. SQL Server uses a compact binary format for these paths, but it is common to parse to and from a human-readable string representation when when working with code. In this representation, the position at each level is separated by a `/` character. For example, consider the family tree in the diagram below:

![Halfling family tree](./familytree.png)

In this tree:

- Balbo is at the root of the tree, represented by `/`.
- Balbo has five children, represented by `/1/`, `/2/`, `/3/`, `/4/`, and `/5/`.
- Balbo's first child, Mungo, also has five children, represented by `/1/1/`, `/1/2/`, `/1/3/`, `/1/4/`, and `/1/5/`. Notice that the `HierarchyId` for Balbo (`/1/`) is the prefix for all his children.
- Similarly, Balbo's third child, Ponto, has two children, represented by `/3/1/` and `/3/2/`. Again the each of these children is prefixed by the `HierarchyId` for Ponto, which is represented as `/3/`.
- And so on down the tree...

The following code inserts this family tree into a database using EF Core:

```csharp
await AddRangeAsync(
    new Halfling(HierarchyId.Parse("/"), "Balbo", 1167),
    new Halfling(HierarchyId.Parse("/1/"), "Mungo", 1207),
    new Halfling(HierarchyId.Parse("/2/"), "Pansy", 1212),
    new Halfling(HierarchyId.Parse("/3/"), "Ponto", 1216),
    new Halfling(HierarchyId.Parse("/4/"), "Largo", 1220),
    new Halfling(HierarchyId.Parse("/5/"), "Lily", 1222),
    new Halfling(HierarchyId.Parse("/1/1/"), "Bungo", 1246),
    new Halfling(HierarchyId.Parse("/1/2/"), "Belba", 1256),
    new Halfling(HierarchyId.Parse("/1/3/"), "Longo", 1260),
    new Halfling(HierarchyId.Parse("/1/4/"), "Linda", 1262),
    new Halfling(HierarchyId.Parse("/1/5/"), "Bingo", 1264),
    new Halfling(HierarchyId.Parse("/3/1/"), "Rosa", 1256),
    new Halfling(HierarchyId.Parse("/3/2/"), "Polo"),
    new Halfling(HierarchyId.Parse("/4/1/"), "Fosco", 1264),
    new Halfling(HierarchyId.Parse("/1/1/1/"), "Bilbo", 1290),
    new Halfling(HierarchyId.Parse("/1/3/1/"), "Otho", 1310),
    new Halfling(HierarchyId.Parse("/1/5/1/"), "Falco", 1303),
    new Halfling(HierarchyId.Parse("/3/2/1/"), "Posco", 1302),
    new Halfling(HierarchyId.Parse("/3/2/2/"), "Prisca", 1306),
    new Halfling(HierarchyId.Parse("/4/1/1/"), "Dora", 1302),
    new Halfling(HierarchyId.Parse("/4/1/2/"), "Drogo", 1308),
    new Halfling(HierarchyId.Parse("/4/1/3/"), "Dudo", 1311),
    new Halfling(HierarchyId.Parse("/1/3/1/1/"), "Lotho", 1310),
    new Halfling(HierarchyId.Parse("/1/5/1/1/"), "Poppy", 1344),
    new Halfling(HierarchyId.Parse("/3/2/1/1/"), "Ponto", 1346),
    new Halfling(HierarchyId.Parse("/3/2/1/2/"), "Porto", 1348),
    new Halfling(HierarchyId.Parse("/3/2/1/3/"), "Peony", 1350),
    new Halfling(HierarchyId.Parse("/4/1/2/1/"), "Frodo", 1368),
    new Halfling(HierarchyId.Parse("/4/1/3/1/"), "Daisy", 1350),
    new Halfling(HierarchyId.Parse("/3/2/1/1/1/"), "Angelica", 1381));

await SaveChangesAsync();
```

> **TIP**
> If needed, decimal values can be used to create new nodes between two existing nodes. For example, `/3/2.5/2/` goes between `/3/2/2/` and `/3/3/2/`.

#### The `HierarchyId` type

`HierarchyId` exposes several methods that can be used in LINQ queries.

| Method                                                           | Description                                                                                                                                                                |
|------------------------------------------------------------------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `GetAncestor(int n)`                                             | Gets the node `n` levels up the hierarchical tree.                                                                                                                         |
| `GetDescendant(HierarchyId? child1, HierarchyId? child2)`        | Gets the value of a descendant node that is greater than `child1` and less than `child2`.                                                                                  |
| `GetLevel()`                                                     | Gets the level of this node in the hierarchical tree.                                                                                                                      |
| `GetReparentedValue(HierarchyId? oldRoot, HierarchyId? newRoot)` | Gets a value representing the location of a new node that has a path from `newRoot` equal to the path from `oldRoot` to this, effectively moving this to the new location. |
| `IsDescendantOf(HierarchyId? parent)`                            | Gets a value indicating whether this node is a descendant of `parent`.                                                                                                     |

In addition, the operators `==`, `!=`, `<`, `<=`, `>` and `>=` can be used.

#### Example: Get entities at a given level in the tree

The following query uses `GetLevel` to return all halflings at a given level in the family tree:

```csharp
var generation = await context.Halflings.Where(halfling => halfling.PathFromPatriarch.GetLevel() == level).ToListAsync();
```

This translates to the following SQL:

```sql
SELECT [h].[Id], [h].[Name], [h].[PathFromPatriarch], [h].[YearOfBirth]
FROM [Halflings] AS [h]
WHERE [h].[PathFromPatriarch].GetLevel() = @__level_0
```

Running this in a loop we can get the halflings for every generation:

```text
Generation 0: Balbo
Generation 1: Mungo, Pansy, Ponto, Largo, Lily
Generation 2: Bungo, Belba, Longo, Linda, Bingo, Rosa, Polo, Fosco
Generation 3: Bilbo, Otho, Falco, Posco, Prisca, Dora, Drogo, Dudo
Generation 4: Lotho, Poppy, Ponto, Porto, Peony, Frodo, Daisy
Generation 5: Angelica
```

#### Example: Get the direct ancestor of an entity

The following query uses `GetAncestor` to find the direct ancestor of a halfling, given that halfling's name:

```csharp
async Task<Halfling?> FindDirectAncestor(string name)
    => await context.Halflings
        .SingleOrDefaultAsync(
            ancestor => ancestor.PathFromPatriarch == context.Halflings
                .Single(descendent => descendent.Name == name).PathFromPatriarch
                .GetAncestor(1));
```

This translates to the following SQL:

```sql
SELECT TOP(2) [h].[Id], [h].[Name], [h].[PathFromPatriarch], [h].[YearOfBirth]
FROM [Halflings] AS [h]
WHERE [h].[PathFromPatriarch] = (
    SELECT TOP(1) [h0].[PathFromPatriarch]
    FROM [Halflings] AS [h0]
    WHERE [h0].[Name] = @__name_0).GetAncestor(1)
```

Running this query for the halfling "Bilbo" returns "Bungo".

#### Example: Get the direct descendents of an entity

The following query also uses `GetAncestor`, but this time to find the direct descendents of a halfling, given that halfling's name:

```csharp
IQueryable<Halfling> FindDirectDescendents(string name)
    => context.Halflings.Where(
        descendent => descendent.PathFromPatriarch.GetAncestor(1) == context.Halflings
            .Single(ancestor => ancestor.Name == name).PathFromPatriarch);
```

This translates to the following SQL:

```sql
SELECT [h].[Id], [h].[Name], [h].[PathFromPatriarch], [h].[YearOfBirth]
FROM [Halflings] AS [h]
WHERE [h].[PathFromPatriarch].GetAncestor(1) = (
    SELECT TOP(1) [h0].[PathFromPatriarch]
    FROM [Halflings] AS [h0]
    WHERE [h0].[Name] = @__name_0)
```

Running this query for the halfling "Mungo" returns "Bungo", "Belba", "Longo", and "Linda".

#### Example: Get all ancestors of an entity

`GetAncestor` is useful for searching up or down a single level, or, indeed, a specified number of levels. On the other hand, `IsDescendantOf` is useful for finding all ancestors or dependents. For example, the following query uses `IsDescendantOf` to find the all the ancestors of a halfling, given that halfling's name:

```csharp
IQueryable<Halfling> FindAllAncestors(string name)
    => context.Halflings.Where(
            ancestor => context.Halflings
                .Single(
                    descendent =>
                        descendent.Name == name
                        && ancestor.Id != descendent.Id)
                .PathFromPatriarch.IsDescendantOf(ancestor.PathFromPatriarch))
        .OrderByDescending(ancestor => ancestor.PathFromPatriarch.GetLevel());
```

> **IMPORTANT**
> `IsDescendantOf` returns true for itself, which is why it is filtered out in the query above.

This translates to the following SQL:

```sql
SELECT [h].[Id], [h].[Name], [h].[PathFromPatriarch], [h].[YearOfBirth]
FROM [Halflings] AS [h]
WHERE (
    SELECT TOP(1) [h0].[PathFromPatriarch]
    FROM [Halflings] AS [h0]
    WHERE [h0].[Name] = @__name_0 AND [h].[Id] <> [h0].[Id]).IsDescendantOf([h].[PathFromPatriarch]) = CAST(1 AS bit)
ORDER BY [h].[PathFromPatriarch].GetLevel() DESC
```

Running this query for the halfling "Bilbo" returns "Bungo", "Mungo", and "Balbo".

#### Example: Get all descendents of an entity

The following query also uses `IsDescendantOf`, but this time to all the descendents of a halfling, given that halfling's name:

```csharp
IQueryable<Halfling> FindAllDescendents(string name)
    => context.Halflings.Where(
            descendent => descendent.PathFromPatriarch.IsDescendantOf(
                context.Halflings
                    .Single(
                        ancestor =>
                            ancestor.Name == name
                            && descendent.Id != ancestor.Id)
                    .PathFromPatriarch))
        .OrderBy(descendent => descendent.PathFromPatriarch.GetLevel());
```

This translates to the following SQL:

```sql
SELECT [h].[Id], [h].[Name], [h].[PathFromPatriarch], [h].[YearOfBirth]
FROM [Halflings] AS [h]
WHERE [h].[PathFromPatriarch].IsDescendantOf((
    SELECT TOP(1) [h0].[PathFromPatriarch]
    FROM [Halflings] AS [h0]
    WHERE [h0].[Name] = @__name_0 AND [h].[Id] <> [h0].[Id])) = CAST(1 AS bit)
ORDER BY [h].[PathFromPatriarch].GetLevel()
```

Running this query for the halfling "Mungo" returns "Bungo", "Belba", "Longo", "Linda", "Bingo", "Bilbo", "Otho", "Falco", "Lotho", and "Poppy".

#### Example: Finding a common ancestor

One of the most common questions asked about this particular family tree is, "who is the common ancestor of Frodo and Bilbo?" We can use `IsDescendantOf` to write such a query:

```csharp
async Task<Halfling?> FindCommonAncestor(Halfling first, Halfling second)
    => await context.Halflings
        .Where(
            ancestor => first.PathFromPatriarch.IsDescendantOf(ancestor.PathFromPatriarch)
                        && second.PathFromPatriarch.IsDescendantOf(ancestor.PathFromPatriarch))
        .OrderByDescending(ancestor => ancestor.PathFromPatriarch.GetLevel())
        .FirstOrDefaultAsync();
```

This translates to the following SQL:

```sql
SELECT TOP(1) [h].[Id], [h].[Name], [h].[PathFromPatriarch], [h].[YearOfBirth]
FROM [Halflings] AS [h]
WHERE @__first_PathFromPatriarch_0.IsDescendantOf([h].[PathFromPatriarch]) = CAST(1 AS bit)
  AND @__second_PathFromPatriarch_1.IsDescendantOf([h].[PathFromPatriarch]) = CAST(1 AS bit)
ORDER BY [h].[PathFromPatriarch].GetLevel() DESC
```

Running this query with "Bilbo" and "Frodo" tells us that their common ancestor is "Balbo".

#### Example: Re-parenting a sub-hierarchy

I'm sure we all remember the scandal of SR 1752 (a.k.a. "LongoGate") when DNA testing revealed that Longo was not in fact the son of Mungo, but actually the son of Ponto! One fallout from this scandal was that the family tree needed to be re-written. In particular, Longo and all his descendents needed to be re-parented from Mungo to Ponto. `GetReparentedValue` can be used to do this. For example, first "Longo" and all his descendents are queried:

```csharp
var longoAndDescendents = await context.Halflings.Where(
        descendent => descendent.PathFromPatriarch.IsDescendantOf(
            context.Halflings.Single(ancestor => ancestor.Name == "Longo").PathFromPatriarch))
    .ToListAsync();
```

Then `GetReparentedValue` is used to update the `HierarchyId` for Longo and each descendent, followed by a call to `SaveChangesAsync`:

```csharp
foreach (var descendent in longoAndDescendents)
{
    descendent.PathFromPatriarch
        = descendent.PathFromPatriarch.GetReparentedValue(
            mungo.PathFromPatriarch, ponto.PathFromPatriarch)!;
}

await context.SaveChangesAsync();
```

This results in the following database update:

```sql
SET NOCOUNT ON;
UPDATE [Halflings] SET [PathFromPatriarch] = @p0
OUTPUT 1
WHERE [Id] = @p1;
UPDATE [Halflings] SET [PathFromPatriarch] = @p2
OUTPUT 1
WHERE [Id] = @p3;
UPDATE [Halflings] SET [PathFromPatriarch] = @p4
OUTPUT 1
WHERE [Id] = @p5;
```

Using these parameters:

```text
 @p1='9',
 @p0='0x7BC0' (Nullable = false) (Size = 2) (DbType = Object),
 @p3='16',
 @p2='0x7BD6' (Nullable = false) (Size = 2) (DbType = Object),
 @p5='23',
 @p4='0x7BD6B0' (Nullable = false) (Size = 3) (DbType = Object)
 ```

> **NOTE**
> The parameters values for `HierarchyId` properties are sent to the database in their compact, binary format.

Following the update, querying for the descendents of "Mungo" returns "Bungo", "Belba", "Linda", "Bingo", "Bilbo", "Falco", and "Poppy", while querying for the descendents of "Ponto" returns "Longo", "Rosa", "Polo", "Otho", "Posco", "Prisca", "Lotho", "Ponto", "Porto", "Peony", and "Angelica".

## How to get EF8 Preview 2

EF8 is distributed exclusively as a set of NuGet packages. For example, to add the SQL Server provider to your project, you can use the following command using the dotnet tool:

```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.0-preview.2.23128.3
```

## Installing the EF8 Command Line Interface (CLI)

The `dotnet-ef` tool must be installed before executing EF8 Core migration or scaffolding commands.

To install the tool globally, use:

```bash
dotnet tool install --global dotnet-ef --version 8.0.0-preview.2.23128.3
```

If you already have the tool installed, you can upgrade it with the following command:

```bash
dotnet tool update --global dotnet-ef --version 8.0.0-preview.2.23128.3
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
