---
post_title: 'EF Core 8 Preview 4: Primitive collections and improved Contains'
author1: shrojans
post_slug: announcing-ef8-preview-4
microsoft_alias: shrojans
featured_image: ef8p4.png
categories: .NET, Entity Framework
summary: Announcing Entity Framework Core 8 (EF8) Preview 4 with support for primitive collections and improved Contains
desired_publication_date: 2023-05-16
tags: .net 8, ef core, efcore, ef8, entity framework
post_date: 2023-05-16 10:00:00
---

The fourth preview of Entity Framework Core (EF Core) 8 is [available on NuGet today](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/8.0.0-preview.4.23259.3)!

## Basic information

EF Core 8, or just EF8, is the successor to EF Core 7, and is scheduled for release in November 2023, at the same time as .NET 8.

EF8 previews currently target .NET 6, and can therefore be used with either .NET 6 (LTS) or .NET 7. This will likely be updated to .NET 8 as we near release.

EF8 will align with .NET 8 as a long-term support (LTS) release. See the [.NET support policy](https://dotnet.microsoft.com/platform/support/policy/dotnet-core) for more information.

## New in EF8 Preview 4

The fourth preview version of EF Core 8.0 preview4 includes some exciting new capabilities in query translation, as well as an important performance optimization. Let's dive in!

### Translating LINQ Contains with an inline collection

In EF's quest to translate more and more LINQ queries to SQL, we sometimes encounter odd and problematic corner cases. Let's examine such a case, which also happens to be related to a highly-voted EF performance issue. Starting from something simple, imagine you have a bunch of Blogs, and want to query out two Blogs whose names you know. You could use the following LINQ query to do so:

```c#
var blogs = await context.Blogs
    .Where(b => new[] { "Blog1", "Blog2" }.Contains(b.Name))
    .ToArrayAsync();
```

This would cause the followed SQL query to be generated on SQL Server:

```sql
SELECT [b].[Id], [b].[Name]
FROM [Blogs] AS [b]
WHERE [b].[Name] IN (N'Blog1', N'Blog2')
```

Looks great! The LINQ Contains operator has a matching SQL construct - the IN expression - which provides us with a perfect translation. However, the names in this query are embedded as constants into the LINQ query - and therefore also into the SQL query, via what I'll refer to as an *inline collection* (that's the `new[] { ... }` part): the collection is specified within the query itself, in line. In many cases, we can't do that: the Blog names are sometimes available only as a variable, since we read them from some other source, possibly even from another EF LINQ query.

### Translating LINQ Contains with a parameter collection

So what happens when we try to do the same, but embedding a variable within the query instead of an inline collection?

```c#
var names = new[] { "Blog1", "Blog2" };

var blogs = await context.Blogs
    .Where(b => names.Contains(b.Name))
    .ToArrayAsync();
```

When a variable such as `names` is embedded in a query, EF usually sends it as-is via a database parameter. This works well in most cases, but for this particular case, databases simply don't support using the IN expression with a parameter. In other words, the following isn't valid SQL:

```sql
SELECT [b].[Id], [b].[Name]
FROM [Blogs] AS [b]
WHERE [b].[Name] IN @names
```

More broadly, relational databases don't really have the concept of a "list" or of a "collection"; they generally work with logically unordered, structured sets such as tables. SQL Server does allow sending [table-valued parameters](https://learn.microsoft.com/sql/relational-databases/tables/use-table-valued-parameters-database-engine), but that involves various complications which make this an inappropriate solution (e.g. the table type must be defined in advanced before querying, with its specific structure).

The one exception to this is PostgreSQL, which fully supports the concept of arrays: you can have an int array column in a table, query into it, and send an array as a parameter, just like you can with any other database type. This allows the EF PostgreSQL provider to perform the following translation:

```sql
Executed DbCommand (10ms) [Parameters=[@__names_0={ 'Blog1', 'Blog2' } (DbType = Object)], CommandType='Text', CommandTimeout='30']

SELECT b."Id", b."Name"
FROM "Blogs" AS b
WHERE b."Name" = ANY (@__names_0)
```

This is very similar to the inline collection translation above with IN, but uses the PostgreSQL-specific ANY construct, which can accept an array type. Leveraging this, we pass the array of blog names as a SQL parameter directly to ANY - that's `@__names_0` - and get the perfect translation. But what can we do for other databases, where this does not exist?

Up to now, all versions of EF have provided the following translation:

```sql
SELECT [b].[Id], [b].[Name]
FROM [Blogs] AS [b]
WHERE [b].[Name] IN (N'Blog1', N'Blog2')
```

But wait, this looks suspiciously familiar - it's the inline collection translation we saw above! And indeed, since we couldn't parameterize the array, we simply embedded its values - as constants - into the SQL query. While .NET variables in EF LINQ queries usually become SQL parameters, in this particular case the variable has disappeared, and its contents have been inserted directly into the SQL.

This has the unfortunate consequence that the SQL produced by EF varies for different array contents - a pretty abnormal situation! Usually, when you run the same LINQ query over and over again - changing only parameter values - EF sends the exact same SQL to the database. This is vital for good performance: SQL Server caches SQL, performing expensive query planning only the first time a particular SQL is seen (a similar SQL cache is implemented in the database driver for PostgreSQL). In addition, EF itself has an internal SQL cache for its queries, and this SQL variance makes caching impossible, leading to further EF overhead for each and every query.

But crucially, the negative performance impact of constantly varying SQLs goes beyond this particular query. SQL Server (and Npgsql) can only cache a certain number of SQLs; at some point, they have to get rid of old entries to avoid using too much memory. If you frequently use Contains with a variable array, each individual invocation causees valuable cache entries to be taken at the database, for SQLs that will most probably never be used (since they have the specific array values baked in). That means you're also evicting cache entries for other, important SQLs that will need to be used, and requiring them to be re-planned again and again.

In short - not great! In fact, this performance issue is the [second most highly-voted issue](https://github.com/dotnet/efcore/issues/13617) in the EF Core repo; and as with most performance problems, your application may be suffering from it without you knowing about it. We clearly need a better solution for translating the LINQ Contains operator when the collection is a parameter.

### Using OpenJson to translate parameter collections

Let's see what SQL preview4 generates for this LINQ query:

```sql
Executed DbCommand (49ms) [Parameters=[@__names_0='["Blog1","Blog2"]' (Size = 4000)], CommandType='Text', CommandTimeout='30']

SELECT [b].[Id], [b].[Name]
FROM [Blogs] AS [b]
WHERE EXISTS (
    SELECT 1
    FROM OpenJson(@__names_0) AS [n]
    WHERE [n].[value] = [b].[Name])
```

This SQL is a completely different beast indeed; but even without understanding exactly what's going on, we can already see that the blog names are passed as a parameter, represented via `@__names_0` in the SQL - similar to our PostgreSQL translation above. So how does this work?

Modern databases have built-in support for JSON; although the specifics vary from database to database, all support some basic forms of parsing and querying JSON directly in SQL. One of SQL Server's JSON capabilities is the [OpenJson](https://learn.microsoft.com/sql/t-sql/functions/openjson-transact-sql) function: this is a "table-valued function" which accepts a JSON document, and returns a standard, relational rowset from its contents. For example, the following SQL query:

```sql
SELECT * FROM OpenJson('["one", "two", "three"]');
```

Returns the following rowset:

[key] | value | type
----- | ----- | ----
0     | one   | 1
1     | two   | 1
2     | three | 2

The input JSON array has effectively been transformed into a relational "table", which can then be queried with the usual SQL operators. EF makes use of this to solve the "parameter collection" problem:

1. We convert your .NET array variable into a JSON array...
2. We send that JSON array as a simple SQL nvarchar parameter...
3. We use the OpenJson function to unpack the parameter...
4. And we use an EXISTS subquery to check if any of the elements match the Blog's name.

This achieves our goal of having a single, non-varying SQL for different values in the .NET array, and resolves the SQL caching problem. Importantly, when viewed on its own, this new translation may actually run a bit *slower* than the previous one - SQL Server can sometimes execute the previous IN translation more efficiently than it can the new translation; when exactly this happens depends on the number of elements in the array. But the crucial bit is that no matter how fast this *particular* query runs, it no longer causes *other* queries to be evicted from the SQL cache, negatively affecting your application as a whole.

> [!NOTE]
> We are looking into further optimizations for the OpenJson-based translation above - the preview4 implementation is just the first version of this feature. Stay tuned for further performance improvements in this area.

### Older versions of SQL Server

The OpenJson function was introduced in SQL Server 2016 (13.x); while that's quite an old version, it's still supported, and we don't want to break its users by relying on it. Therefore, we've introduced a general way for you to tell EF which SQL Server is being targeted - this will allow us to take advantage of newer features while preserving backwards compatibility for users on older versions. To do this, simply call the new [`UseCompatibilityLevel`] method when configuring your context options:

```c#
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder
        .UseSqlServer(@"<CONNECTION STRING>", o => o.UseCompatibilityLevel(120));
```

The `120` argument is the desired [SQL Server compatibility level](https://learn.microsoft.com/sql/t-sql/statements/alter-database-transact-sql-compatibility-level#compatibility_level--160--150--140--130--120--110--100--90--80-); `120` corresponds to SQL Server 2014 (12.x). When this is done, EF will generate the previous translation, embedding the array's contents into an IN expression.

### Queryable primitive collection columns

I could stop here, and we'd already have a nice feature, resolving a long-standing performance issue. But let's go further! The solution to Contains above supports representing a primitive collection as a JSON array, and then using that collection like any other table in the query. The above translation of Contains is just a very specific case of that - but we can do much, much more.

Let's say that each Blog is also associated to a collection of Tags. In classical relational modeling, we'd represent this as a many-to-many relationship between a Blogs table and a Tags table, using a BlogTags join table to link the two together; and EF Core supports this mapping very well ([see docs](https://learn.microsoft.com/ef/core/modeling/relationships/many-to-many)). But this traditional modeling can be a bit heavy, requiring two additional tables and JOINs, and a .NET type to wrap your simple string Tag. Let's try to look at this from a different angle.

Since EF now supports primitive collections, we can simply add a string array property to our Blog type:

```c#
public class Blog
{
    public int Id { get; set; }
    // ...
    public string[] Tags { get; set; }
}
```

This causes EF to generate the following table:

```sql
CREATE TABLE [Blogs] (
    [Id] int NOT NULL IDENTITY,
    -- ...
    [Tags] nvarchar(max) NULL,
);
```

Our new Tags properties is now mapped to a single `nvarchar(max)` property in the database. You can now add a Blog with some tags:

```c#
context.Blogs.Add(new Blog { Name = "Blog1", Tags = new[] { "Tag1", "Tag2" } });
await context.SaveChangesAsync();
```

... and EF will automatically encode your Tags .NET array as a JSON array string in the database:

```sql
Executed DbCommand (47ms) [Parameters=[@p0='foo' (Nullable = false) (Size = 4000), @p1='["Tag1","Tag2"]' (Size = 4000)], CommandType='Text', CommandTimeout='30']

INSERT INTO [Blogs] ([Name], [Tags])
OUTPUT INSERTED.[Id]
VALUES (@p0, @p1);
```

Similarly, when reading a Blog from the database, EF will automatically decode the JSON array and populate your .NET array property. That's all pretty nifty - but people have been doing this for quite some time by defining a [value converter](https://learn.microsoft.com/ef/core/modeling/value-conversions) on their array properties. In fact, our value converter documentation has an [example](https://learn.microsoft.com/ef/core/modeling/value-conversions) showing exactly this. So what's the big deal?

Just as we used a SQL EXISTS subquery to translate the LINQ Contains operator, EF now allows you to use arbitrary LINQ operators over such primitive collection columns - just as if they were regular DbSets; in other words, primitive collections are now fully queryable. For example, to find all Blogs which have a certain Tag, you can now use the following LINQ query:

```c#
var blogs = await context.Blogs
    .Where(b => b.Tags.Contains("Tag1"))
    .ToArrayAsync();
```

... which EF translates to the following:

```sql
SELECT [b].[Id], [b].[Name], [b].[Tags]
FROM [Blogs] AS [b]
WHERE EXISTS (
    SELECT 1
    FROM OpenJson([b].[Tags]) AS [t]
    WHERE [t].[value] = N'Tag1')
```

That's the exact same SQL we saw above for a parameter - but applied to a column! But let's do something fancier: what if, instead of querying for all Blogs which have a certain Tag, we want to query for Blogs which have *multiple* Tags? This can now be done with the following LINQ query:

```c#
var tags = new[] { "Tag1", "Tag2" };

var blogs = await context.Blogs
    .Where(b => b.Tags.Intersect(tags).Count() >= 2)
    .ToArrayAsync();
```

This leverages more sophisticated LINQ operators: we intersect each Blog's Tags with a parameter collection, and query out the Blogs where there are at least two matches. This translates to the following:

```sql
Executed DbCommand (48ms) [Parameters=[@__tags_0='["Tag1","Tag2"]' (Size = 4000)], CommandType='Text', CommandTimeout='30']

SELECT [b].[Id], [b].[Name], [b].[Tags]
FROM [Blogs] AS [b]
WHERE (
    SELECT COUNT(*)
    FROM (
        SELECT [t].[value]
        FROM OpenJson([b].[Tags]) AS [t] -- column collection
        INTERSECT
        SELECT [t1].[value]
        FROM OpenJson(@__tags_0) AS [t1] -- parameter collection
    ) AS [t0]) >= 2
```

That's quite a mouthful - but we're using the same basic mechanisms: we perform an intersection between the column primitive collection (`[b].[Tags]`) and the parameter primitive collection (`@__tags_0`), using OpenJson to unpack the JSON array strings into rowsets.

Let's look at one last example. Since we encode primitive collections as JSON arrays, these collections are *naturally ordered*. This is an atypical situation within relationl databases - relational sets are always logically unordered, and an ORDER BY clause must be used in order to get any deterministic ordering.

Now, a list of Tags is typically an unordered bag: we don't care which Tag comes first. But let's assume, for the sake of this example, that your Blogs' Tags are ordered, with more "important" Tags coming first. In such a situation, it may make sense to query all Blogs with a certain value as their *first* Tag:

```c#
var blogs = await context.Blogs
    .Where(b => b.Tags[0] == "Tag1")
    .ToArrayAsync();
```

This currently generates the following SQL:

```sql
SELECT [b].[Id], [b].[Name], [b].[Tags]
FROM [Blogs] AS [b]
WHERE (
    SELECT [t].[value]
    FROM OpenJson([b].[Tags]) AS [t]
    ORDER BY CAST([t].[key] AS int)
    OFFSET 0 ROWS FETCH NEXT 1 ROWS ONLY) = N'Tag1'
```

EF generates an ORDER BY clause to make sure that the JSON array's natural ordering is preserved, and then uses limits to get the first element. This over-elaborate SQL has already been improved, and later previews will generate the following tighter SQL instead:

```sql
SELECT [b].[Id], [b].[Name], [b].[Tags]
FROM [Blogs] AS [b]
WHERE JSON_VALUE([b].[Tags], '$[0]') = N'Tag1'
```

To summarize, you can now use the full range of LINQ operators on primitive collections - whether they're a column or a parameter. This opens up exciting translation possibilities for queries which were never translatable before; we're looking forward to seeing the kind of queries you'll use with this!

> [!NOTE]
> Before using JSON-based primitive collections, carefully consider indexing and query performance. Most database allow indexing at least some forms of querying into JSON documents; but arbitrary, complex queries such as the intersect above would likely not be able to use an index. In some cases, traditional relational modeling (e.g. many-to-many) may be more appropriate.

> [!NOTE]
> We mentioned above that PostgreSQL has native support for arrays, so there's no need to resort to JSON array encoding when dealing with primitive collections there. Instead, primitive array collections are (by default) mapped to arrays, and the PostgreSQL [unnest](https://www.postgresql.org/docs/current/functions-array.html#id-1.5.8.25.6.2.2.17.1.1.1) function is used to expand the native array to a rowset.

### And one last thing: queryable inline collections

We discussed columns and parameters containing primitive collections, but we left out one last type - inline collections. You may remember that we started this post with the following LINQ query:

```c#
var blogs = await context.Blogs
    .Where(b => new[] { "Blog1", "Blog2" }.Contains(b.Name))
    .ToArrayAsync();
```

The `new[] { ... }` bit in the query represents an *inline collection*. Up to now, EF supported these only in some very restricted scenarios, such as with the Contains operator. Preview 4 now brings full support for queryable inline collections, allowing you to use the full range of LINQ operators on them as well.

As an example query, let's challenge ourselves and do something a bit more complicated. The following query searches for Blogs which have at least one Tag that starts with either `a` or `b`:

```c#
var blogs = await context.Blogs
    .Where(b => new[] { "a%", "b%" }
        .Any(pattern => b.Tags.Any(tag => EF.Functions.Like(tag, pattern))))
    .ToArrayAsync();
```

Note that the inline collection of patterns - `new[] { "a%", "b%" }` - is composed over with the Any operator. This now translates to the following SQL:

```sql
SELECT [b].[Id], [b].[Name], [b].[Tags]
FROM [Blogs] AS [b]
WHERE EXISTS (
    SELECT 1
    FROM (VALUES (CAST(N'a%' AS nvarchar(max))), (N'b%')) AS [v]([Value]) -- inline collection
    WHERE EXISTS (
        SELECT 1
        FROM OpenJson([b].[Tags]) AS [t] -- column collection
        WHERE [t].[value] LIKE [v].[Value]))
```

The interesting bit is the "inline collection" line. Unlike with parameter and column collections, we don't need to resort to JSON arrays and OpenJson: SQL already has a universal mechanism for specifying inline tables via the `VALUES` expression. This completes the picture - EF now supports querying into any kind of primitive collection, be it a column, a parameter or an inline collection.

### What's supported and what's not

The fourth preview brings primitive collection support for SQL Server and SQLite; the PostgreSQL provider will also be updated to support them. However, as indicated above, this is the first wave of work on primitive collections - expect further improvements in coming versions. Specifically:

* Primitive collections inside owned JSON entities aren't supported yet.
* Certain primitive data types aren't yet supported on certain providers; this is the case with spatial types, for example.
* We may optimize the SQL around OpenJSON to make querying more efficient.

## How to get EF8 Preview 4

EF8 is distributed exclusively as a set of NuGet packages. For example, to add the SQL Server provider to your project, you can use the following command using the dotnet tool:

```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.0-preview.4.23259.3
```

## Installing the EF8 Command Line Interface (CLI)

The `dotnet-ef` tool must be installed before executing EF8 Core migration or scaffolding commands.

To install the tool globally, use:

```bash
dotnet tool install --global dotnet-ef --version 8.0.0-preview.4.23259.3
```

If you already have the tool installed, you can upgrade it with the following command:

```bash
dotnet tool update --global dotnet-ef --version 8.0.0-preview.4.23259.3
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
- Bi-weekly updates: [aka.ms/ef-news](https://aka.ms/ef-news)
