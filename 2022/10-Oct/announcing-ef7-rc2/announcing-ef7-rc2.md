---
post_title: 'Announcing Entity Framework Core 7 RC2: JSON Columns'
author1: avickers
post_slug: announcing-ef7-release-candidate-2
microsoft_alias: avickers
featured_image: ef7rc2.png
categories: .NET, .NET Core, Entity Framework
summary: Announcing EF Core 7 (EF7) RC2 featuring JSON column mapping
desired_publication_date: 2022-10-11
post_date: 2022-10-11 10:00:00
---

Entity Framework Core 7 (EF7) RC2 has shipped! The RC2 release contains all features planned for the EF7 GA release. This includes:

- [Mapping to SQL Server JSON Columns](https://learn.microsoft.com/ef/core/what-is-new/ef-core-7.0/whatsnew#json-columns)
- [ExecuteUpdate and ExecuteDelete (Bulk updates)](https://learn.microsoft.com/ef/core/what-is-new/ef-core-7.0/whatsnew#executeupdate-and-executedelete-bulk-updates)
- [Improved performance for SaveChanges](https://learn.microsoft.com/ef/core/what-is-new/ef-core-7.0/whatsnew#faster-savechanges)
- [Table-per-concrete-type (TPC) inheritance mapping](https://learn.microsoft.com/ef/core/what-is-new/ef-core-7.0/whatsnew#table-per-concrete-type-tpc-inheritance-mapping)
- [Custom Reverse Engineering Templates for Database First](https://learn.microsoft.com/ef/core/what-is-new/ef-core-7.0/whatsnew#custom-reverse-engineering-templates)
- [Customizable model building conventions](https://learn.microsoft.com/ef/core/what-is-new/ef-core-7.0/whatsnew#model-building-conventions)
- [Stored procedure mapping for inserts, updates, and deletes](https://learn.microsoft.com/ef/core/what-is-new/ef-core-7.0/whatsnew#stored-procedure-mapping)
- [New and improved interceptors and events](https://learn.microsoft.com/ef/core/what-is-new/ef-core-7.0/whatsnew#new-and-improved-interceptors-and-events)
- [Query enhancements, including more GroupBy and GroupJoin translations](https://learn.microsoft.com/ef/core/what-is-new/ef-core-7.0/whatsnew#query-enhancements)

RC2 contains more than [80 fixed issues over RC1](https://github.com/dotnet/efcore/issues?q=is%3Aissue+milestone%3A7.0.0-rc2+is%3Aclosed). In total, EF7 contains more than 350 fixed issues over EF Core 6.0, as well as [significant documentation updates](https://github.com/dotnet/EntityFramework.Docs/issues?q=is%3Aopen+is%3Aissue+milestone%3A7.0.0). Full details can be found on the [dotnet/efcore GitHub Releases page](https://github.com/dotnet/efcore/releases).

In this blog post, well take a deep dive into one of the most hotly anticipated features in EF7--the ability to map .NET aggregates to JSON documents stored in relational database columns.

## Mapping to JSON Columns

Most relational databases support columns that contain JSON documents. The JSON in these columns can be drilled into with queries. This allows, for example, filtering and sorting by the elements of the documents, as well as projection of elements out of the documents into results. JSON columns allow relational databases to take on some of the characteristics of document databases, creating a useful hybrid between the two.

EF7 contains provider-agnostic support for JSON columns, with an implementation for SQL Server. This support allows mapping of aggregates built from .NET types to JSON documents. Normal LINQ queries can be used on the aggregates, and these will be translated to the appropriate query constructs needed to drill into the JSON. EF7 also supports updating and saving changes to the JSON documents.

> [!NOTE]
> SQLite support for JSON is [planned for post EF7](https://github.com/dotnet/efcore/issues/28816). The [PostgreSQL](https://github.com/npgsql/efcore.pg) and [Pomelo MySQL](https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql) providers already contain some support for JSON columns. We will be working with the authors of those providers to align JSON support across all providers.

### Mapping to JSON columns

In EF Core, aggregate types are defined using `OwnsOne` and `OwnsMany`. For example, consider the aggregate `ContactDetails` type from the sample model shown at the end of this post.

```csharp
public class ContactDetails
{
    public Address Address { get; set; } = null!;
    public string? Phone { get; set; }
}

public class Address
{
    public Address(string street, string city, string postcode, string country)
    {
        Street = street;
        City = city;
        Postcode = postcode;
        Country = country;
    }

    public string Street { get; set; }
    public string City { get; set; }
    public string Postcode { get; set; }
    public string Country { get; set; }
}
```

This can then be used in an "owner" entity type, for example, to store the contact details of an author:

```csharp
public class Author
{
    public Author(string name)
    {
        Name = name;
    }

    public int Id { get; private set; }
    public string Name { get; set; }
    public ContactDetails Contact { get; set; } = null!;
    public List<Post> Posts { get; } = new();
}
```

The aggregate type is configured  in `OnModelCreating` using `OwnsOne`:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Author>().OwnsOne(
        author => author.Contact, ownedNavigationBuilder =>
        {
            ownedNavigationBuilder.OwnsOne(contactDetails => contactDetails.Address);
        });
}
```

> [!TIP]
> The code shown in this post comes from the [samples on GitHub](https://github.com/dotnet/EntityFramework.Docs) for the [What's New in EF7](https://learn.microsoft.com/ef/core/what-is-new/ef-core-7.0/whatsnew) documentation. Specifically, this code comes from [JsonColumnsSample.cs](https://github.com/dotnet/EntityFramework.Docs/tree/main/samples/core/Miscellaneous/NewInEFCore7/JsonColumnsSample.cs).

By default, EF Core relational database providers map aggregate types like this to the same table as the owning entity type. That is, each property of the `ContactDetails` and `Address` classes are mapped to a column in the `Authors` table.

Some saved authors with contact details will look like this:

**Authors**

| Id  | Name             | Contact\_Address\_Street | Contact\_Address\_City | Contact\_Address\_Postcode | Contact\_Address\_Country | Contact\_Phone |
|:----|:-----------------|:-------------------------|:-----------------------|:---------------------------|:--------------------------|:---------------|
| 1   | Maddy Montaquila | 1 Main St                | Camberwick Green       | CW1 5ZH                    | UK                        | 01632 12345    |
| 2   | Jeremy Likness   | 2 Main St                | Chigley                | CW1 5ZH                    | UK                        | 01632 12346    |
| 3   | Daniel Roth      | 3 Main St                | Camberwick Green       | CW1 5ZH                    | UK                        | 01632 12347    |
| 4   | Arthur Vickers   | 15a Main St              | Chigley                | CW1 5ZH                    | United Kingdom            | 01632 22345    |
| 5   | Brice Lambson    | 4 Main St                | Chigley                | CW1 5ZH                    | UK                        | 01632 12349    |

If desired, each entity type making up the aggregate can be mapped to its own table instead:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Author>().OwnsOne(
        author => author.Contact, ownedNavigationBuilder =>
        {
            ownedNavigationBuilder.ToTable("Contacts");
            ownedNavigationBuilder.OwnsOne(contactDetails => contactDetails.Address, ownedOwnedNavigationBuilder =>
            {
                ownedOwnedNavigationBuilder.ToTable("Addresses");
            });
        });
}
```

The same data is then stored across three tables:

**Authors**

| Id  | Name             |
|:----|:-----------------|
| 1   | Maddy Montaquila |
| 2   | Jeremy Likness   |
| 3   | Daniel Roth      |
| 4   | Arthur Vickers   |
| 5   | Brice Lambson    |

**Contacts**

| AuthorId | Phone       |
|:---------|:------------|
| 1        | 01632 12345 |
| 2        | 01632 12346 |
| 3        | 01632 12347 |
| 4        | 01632 22345 |
| 5        | 01632 12349 |

**Addresses**

| ContactDetailsAuthorId | Street      | City             | Postcode | Country        |
|:-----------------------|:------------|:-----------------|:---------|:---------------|
| 1                      | 1 Main St   | Camberwick Green | CW1 5ZH  | UK             |
| 2                      | 2 Main St   | Chigley          | CW1 5ZH  | UK             |
| 3                      | 3 Main St   | Camberwick Green | CW1 5ZH  | UK             |
| 4                      | 15a Main St | Chigley          | CW1 5ZH  | United Kingdom |
| 5                      | 4 Main St   | Chigley          | CW1 5ZH  | UK             |

Now, for the interesting part. In EF7, the `ContactDetails` aggregate can be mapped to a JSON column. This requires just one call to `ToJson()` when configuring the aggregate type:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Author>().OwnsOne(
        author => author.Contact, ownedNavigationBuilder =>
        {
            ownedNavigationBuilder.ToJson();
            ownedNavigationBuilder.OwnsOne(contactDetails => contactDetails.Address);
        });
}
```

The `Authors` table will now contain a JSON column for `ContactDetails` populated with a JSON document for each author:

**Authors**

| Id  | Name             | Contact                                                                                                                                                                                                                                                                                             |
|:----|:-----------------|:----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| 1   | Maddy Montaquila | {<br/>&nbsp;&nbsp;"Phone":"01632 12345",<br/>&nbsp;&nbsp;"Address": {<br/>&nbsp;&nbsp;&nbsp;&nbsp;"City":"Camberwick Green",<br/>&nbsp;&nbsp;&nbsp;&nbsp;"Country":"UK",<br/>&nbsp;&nbsp;&nbsp;&nbsp;"Postcode":"CW1 5ZH",<br/>&nbsp;&nbsp;&nbsp;&nbsp;"Street":"1 Main St"<br/>&nbsp;&nbsp;}<br/>} |
| 2   | Jeremy Likness   | {<br/>&nbsp;&nbsp;"Phone":"01632 12346",<br/>&nbsp;&nbsp;"Address": {<br/>&nbsp;&nbsp;&nbsp;&nbsp;"City":"Chigley",<br/>&nbsp;&nbsp;&nbsp;&nbsp;"Country":"UK",<br/>&nbsp;&nbsp;&nbsp;&nbsp;"Postcode":"CH1 5ZH",<br/>&nbsp;&nbsp;&nbsp;&nbsp;"Street":"2 Main St"<br/>&nbsp;&nbsp;}<br/>}          |
| 3   | Daniel Roth      | {<br/>&nbsp;&nbsp;"Phone":"01632 12347",<br/>&nbsp;&nbsp;"Address": {<br/>&nbsp;&nbsp;&nbsp;&nbsp;"City":"Camberwick Green",<br/>&nbsp;&nbsp;&nbsp;&nbsp;"Country":"UK",<br/>&nbsp;&nbsp;&nbsp;&nbsp;"Postcode":"CW1 5ZH",<br/>&nbsp;&nbsp;&nbsp;&nbsp;"Street":"3 Main St"<br/>&nbsp;&nbsp;}<br/>} |
| 4   | Arthur Vickers   | {<br/>&nbsp;&nbsp;"Phone":"01632 12348",<br/>&nbsp;&nbsp;"Address": {<br/>&nbsp;&nbsp;&nbsp;&nbsp;"City":"Chigley",<br/>&nbsp;&nbsp;&nbsp;&nbsp;"Country":"UK",<br/>&nbsp;&nbsp;&nbsp;&nbsp;"Postcode":"CH1 5ZH",<br/>&nbsp;&nbsp;&nbsp;&nbsp;"Street":"15a Main St"<br/>&nbsp;&nbsp;}<br/>}        |
| 5   | Brice Lambson    | {<br/>&nbsp;&nbsp;"Phone":"01632 12349",<br/>&nbsp;&nbsp;"Address": {<br/>&nbsp;&nbsp;&nbsp;&nbsp;"City":"Chigley",<br/>&nbsp;&nbsp;&nbsp;&nbsp;"Country":"UK",<br/>&nbsp;&nbsp;&nbsp;&nbsp;"Postcode":"CH1 5ZH",<br/>&nbsp;&nbsp;&nbsp;&nbsp;"Street":"4 Main St"<br/>&nbsp;&nbsp;}<br/>}          |

> [!TIP]
> This use of aggregates is very similar to the way JSON documents are mapped when using the EF Core provider for Azure Cosmos DB. JSON columns bring the capabilities of using EF Core against document databases to documents embedded in a relational database.

The JSON documents shown above are very simple, but this mapping capability can also be used with more complex document structures. For example, consider another aggregate type from our sample model, used to represent metadata about a post:

```csharp
public class PostMetadata
{
    public PostMetadata(int views)
    {
        Views = views;
    }

    public int Views { get; set; }
    public List<SearchTerm> TopSearches { get; } = new();
    public List<Visits> TopGeographies { get; } = new();
    public List<PostUpdate> Updates { get; } = new();
}

public class SearchTerm
{
    public SearchTerm(string term, int count)
    {
        Term = term;
        Count = count;
    }

    public string Term { get; private set; }
    public int Count { get; private set; }
}

public class Visits
{
    public Visits(double latitude, double longitude, int count)
    {
        Latitude = latitude;
        Longitude = longitude;
        Count = count;
    }

    public double Latitude { get; private set; }
    public double Longitude { get; private set; }
    public int Count { get; private set; }
    public List<string>? Browsers { get; set; }
}

public class PostUpdate
{
    public PostUpdate(IPAddress postedFrom, DateTime updatedOn)
    {
        PostedFrom = postedFrom;
        UpdatedOn = updatedOn;
    }

    public IPAddress PostedFrom { get; private set; }
    public string? UpdatedBy { get; init; }
    public DateTime UpdatedOn { get; private set; }
    public List<Commit> Commits { get; } = new();
}

public class Commit
{
    public Commit(DateTime committedOn, string comment)
    {
        CommittedOn = committedOn;
        Comment = comment;
    }

    public DateTime CommittedOn { get; private set; }
    public string Comment { get; set; }
}
```

This aggregate type contains several nested types and collections. Calls to `OwnsOne` and `OwnsMany` are used to map this aggregate type:

```csharp
modelBuilder.Entity<Post>().OwnsOne(
    post => post.Metadata, ownedNavigationBuilder =>
    {
        ownedNavigationBuilder.ToJson();
        ownedNavigationBuilder.OwnsMany(metadata => metadata.TopSearches);
        ownedNavigationBuilder.OwnsMany(metadata => metadata.TopGeographies);
        ownedNavigationBuilder.OwnsMany(
            metadata => metadata.Updates,
            ownedOwnedNavigationBuilder => ownedOwnedNavigationBuilder.OwnsMany(update => update.Commits));
    });
```

> [!TIP]
> `ToJson` is only needed on the aggregate root to map the entire aggregate to a JSON document.

With this mapping, EF7 can create and query into a complex JSON document like this:

```json
{
  "Views": 5085,
  "TopGeographies": [
    {
      "Browsers": "Firefox, Netscape",
      "Count": 924,
      "Latitude": 110.793,
      "Longitude": 39.2431
    },
    {
      "Browsers": "Firefox, Netscape",
      "Count": 885,
      "Latitude": 133.793,
      "Longitude": 45.2431
    }
  ],
  "TopSearches": [
    {
      "Count": 9359,
      "Term": "Search #1"
    }
  ],
  "Updates": [
    {
      "PostedFrom": "127.0.0.1",
      "UpdatedBy": "Admin",
      "UpdatedOn": "1996-02-17T19:24:29.5429092Z",
      "Commits": []
    },
    {
      "PostedFrom": "127.0.0.1",
      "UpdatedBy": "Admin",
      "UpdatedOn": "2019-11-24T19:24:29.5429093Z",
      "Commits": [
        {
          "Comment": "Commit #1",
          "CommittedOn": "2022-08-21T00:00:00+01:00"
        }
      ]
    },
    {
      "PostedFrom": "127.0.0.1",
      "UpdatedBy": "Admin",
      "UpdatedOn": "1997-05-28T19:24:29.5429097Z",
      "Commits": [
        {
          "Comment": "Commit #1",
          "CommittedOn": "2022-08-21T00:00:00+01:00"
        },
        {
          "Comment": "Commit #2",
          "CommittedOn": "2022-08-21T00:00:00+01:00"
        }
      ]
    }
  ]
}
```

### Queries into JSON columns

Queries into JSON columns work just the same as querying into any other aggregate type in EF Core. That is, just use LINQ! Here are some examples.

A query for all authors that live in Chigley:

```csharp
var authorsInChigley = await context.Authors
    .Where(author => author.Contact.Address.City == "Chigley")
    .ToListAsync();
```

This query generates the following SQL when using SQL Server:

```sql
      SELECT [a].[Id], [a].[Name], JSON_QUERY([a].[Contact],'$')
      FROM [Authors] AS [a]
      WHERE CAST(JSON_VALUE([a].[Contact],'$.Address.City') AS nvarchar(max)) = N'Chigley'
```

Notice the use of `JSON_VALUE` to get the `City` from the `Address` inside the JSON document.

`Select` can be used to extract and project elements from the JSON document:

```csharp
var postcodesInChigley = await context.Authors
    .Where(author => author.Contact.Address.City == "Chigley")
    .Select(author => author.Contact.Address.Postcode)
    .ToListAsync();
```

This query generates the following SQL:

```sql
SELECT CAST(JSON_VALUE([a].[Contact],'$.Address.Postcode') AS nvarchar(max))
FROM [Authors] AS [a]
WHERE CAST(JSON_VALUE([a].[Contact],'$.Address.City') AS nvarchar(max)) = N'Chigley'
```

Here's an example that does a bit more in the filter and projection, and also orders by the phone number in the JSON document:

```csharp
var orderedAddresses = await context.Authors
    .Where(
        author => (author.Contact.Address.City == "Chigley"
                   && author.Contact.Phone != null)
                  || author.Name.StartsWith("D"))
    .OrderBy(author => author.Contact.Phone)
    .Select(
        author => author.Name + " (" + author.Contact.Address.Street
                  + ", " + author.Contact.Address.City
                  + " " + author.Contact.Address.Postcode + ")")
    .ToListAsync();
```

This query generates the following SQL:

```sql
SELECT (((((([a].[Name] + N' (') + CAST(JSON_VALUE([a].[Contact],'$.Address.Street') AS nvarchar(max))) + N', ') + CAST(JSON_VALUE([a].[Contact],'$.Address.City') AS nvarchar(max))) + N' ') + CAST(JSON_VALUE([a].[Contact],'$.Address.Postcode') AS nvarchar(max))) + N')'
FROM [Authors] AS [a]
WHERE (CAST(JSON_VALUE([a].[Contact],'$.Address.City') AS nvarchar(max)) = N'Chigley' AND CAST(JSON_VALUE([a].[Contact],'$.Phone') AS nvarchar(max)) IS NOT NULL) OR ([a].[Name] LIKE N'D%')
ORDER BY CAST(JSON_VALUE([a].[Contact],'$.Phone') AS nvarchar(max))
```

And when the JSON document contains collections, then these can be projected out in the results:

```csharp
var postsWithViews = await context.Posts.Where(post => post.Metadata!.Views > 3000)
    .AsNoTracking()
    .Select(
        post => new
        {
            post.Author!.Name,
            post.Metadata!.Views,
            Searches = post.Metadata.TopSearches,
            Commits = post.Metadata.Updates
        })
    .ToListAsync();
```

This query generates the following SQL:

```sql
SELECT [a].[Name], CAST(JSON_VALUE([p].[Metadata],'$.Views') AS int), JSON_QUERY([p].[Metadata],'$.TopSearches'), [p].[Id], JSON_QUERY([p].[Metadata],'$.Updates')
FROM [Posts] AS [p]
LEFT JOIN [Authors] AS [a] ON [p].[AuthorId] = [a].[Id]
WHERE CAST(JSON_VALUE([p].[Metadata],'$.Views') AS int) > 3000
```

> [!TIP]
> Consider creating indexes to improve query performance in JSON documents. For example, see [Index Json data](https://learn.microsoft.com/sql/relational-databases/json/index-json-data) when using SQL Server.

### Updating JSON columns

[`SaveChanges` and `SaveChangesAsync`](https://learn.microsoft.com/ef/core/saving/basic) work in the normal way to make updates a JSON column. For extensive changes, the entire document will be updated. For example, replacing most of the `Contact` document for an author:

```csharp
var jeremy = await context.Authors.SingleAsync(author => author.Name.StartsWith("Jeremy"));

jeremy.Contact = new() { Address = new("2 Riverside", "Trimbridge", "TB1 5ZS", "UK"), Phone = "01632 88346" };

await context.SaveChangesAsync();
```

In this case, the entire new document is passed as a parameter:

```text
info: 8/30/2022 20:21:24.392 RelationalEventId.CommandExecuted[20101] (Microsoft.EntityFrameworkCore.Database.Command)
      Executed DbCommand (2ms) [Parameters=[@p0='{"Phone":"01632 88346","Address":{"City":"Trimbridge","Country":"UK","Postcode":"TB1 5ZS","Street":"2 Riverside"}}' (Nullable = false) (Size = 114), @p1='2'], CommandType='Text', CommandTimeout='30']
```

Which is then used in the `UPDATE` SQL:

```sql
SET IMPLICIT_TRANSACTIONS OFF;
SET NOCOUNT ON;
UPDATE [Authors] SET [Contact] = @p0
OUTPUT 1
WHERE [Id] = @p1;
```

However, if only a sub-document is changed, then EF Core will use a `JSON_MODIFY` command to update only the sub-document. For example, changing the `Address` inside a `Contact` document:

```csharp
var brice = await context.Authors.SingleAsync(author => author.Name.StartsWith("Brice"));

brice.Contact.Address = new("4 Riverside", "Trimbridge", "TB1 5ZS", "UK");

await context.SaveChangesAsync();
```

Generates the following parameters:

```text
info: 10/2/2022 15:51:15.895 RelationalEventId.CommandExecuted[20101] (Microsoft.EntityFrameworkCore.Database.Command)
      Executed DbCommand (2ms) [Parameters=[@p0='{"City":"Trimbridge","Country":"UK","Postcode":"TB1 5ZS","Street":"4 Riverside"}' (Nullable = false) (Size = 80), @p1='5'], CommandType='Text', CommandTimeout='30']
```
Which is used in the `UPDATE` via a `JSON_MODIFY` call:

```sql
SET IMPLICIT_TRANSACTIONS OFF;
SET NOCOUNT ON;
UPDATE [Authors] SET [Contact] = JSON_MODIFY([Contact], 'strict $.Address', JSON_QUERY(@p0))
OUTPUT 1
WHERE [Id] = @p1;
```

Finally, if only a single property is changed, then EF Core will again use a "JSON_MODIFY" command, this time to patch only the changed property value. For example:

```csharp
var arthur = await context.Authors.SingleAsync(author => author.Name.StartsWith("Arthur"));

arthur.Contact.Address.Country = "United Kingdom";

await context.SaveChangesAsync();
```

Generates the following parameters:

```text
info: 10/2/2022 15:54:05.112 RelationalEventId.CommandExecuted[20101] (Microsoft.EntityFrameworkCore.Database.Command)
      Executed DbCommand (2ms) [Parameters=[@p0='["United Kingdom"]' (Nullable = false) (Size = 18), @p1='4'], CommandType='Text', CommandTimeout='30']
```

Which are again used with a `JSON_MODIFY`:

```sql
SET IMPLICIT_TRANSACTIONS OFF;
SET NOCOUNT ON;
UPDATE [Authors] SET [Contact] = JSON_MODIFY([Contact], 'strict $.Address.Country', JSON_VALUE(@p0, '$[0]'))
OUTPUT 1
WHERE [Id] = @p1;
```

### Limitations

The JSON support in EF7 lays the groundwork for fully-featured cross-provider JSON column support in future releases. However, the EF7 schedule means that some important features did not make it into EF7. These include:

| Feature                                                                                   | Tracking GitHub Issue                                   |
|-------------------------------------------------------------------------------------------|---------------------------------------------------------|
| JSON column support for SQLite.                                                           | [#28816](https://github.com/dotnet/efcore/issues/28816) |
| Map spatial types to JSON.                                                                | [#28811](https://github.com/dotnet/efcore/issues/28811) |
| Map collections of primitive types to JSON.                                               | [#28688](https://github.com/dotnet/efcore/issues/28688) |
| Support JSON columns when using TPT or TPC inheritance mapping.                           | [#28443](https://github.com/dotnet/efcore/issues/28443) |
| Support more complex queries, such as querying into collections, that require `jsonpath`. | [#28616](https://github.com/dotnet/efcore/issues/28616) |
| Mapping attribute (aka data annotation) for property mapped to JSON column.               | [#28933](https://github.com/dotnet/efcore/issues/28933) |
| Configure serialization options for JSON columns.                                         | [#28043](https://github.com/dotnet/efcore/issues/28043) |

We plan to implement these features in future releases. Make sure to vote for the appropriate GitHub issue for any feature that is particularly important to you. 

### The model

As noted above, the code shown in this post comes from the [EF Core samples on GitHub](https://github.com/dotnet/EntityFramework.Docs). The entity types used in this post are shown below.

```csharp
public class Blog
{
    public Blog(string name)
    {
        Name = name;
    }

    public int Id { get; private set; }
    public string Name { get; set; }
    public List<Post> Posts { get; } = new();
}

public class Post
{
    public Post(string title, string content, DateTime publishedOn)
    {
        Title = title;
        Content = content;
        PublishedOn = publishedOn;
    }

    public int Id { get; private set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime PublishedOn { get; set; }
    public Blog Blog { get; set; } = null!;
    public List<Tag> Tags { get; } = new();
    public Author? Author { get; set; }
    public PostMetadata? Metadata { get; set; }
}

public class FeaturedPost : Post
{
    public FeaturedPost(string title, string content, DateTime publishedOn, string promoText)
        : base(title, content, publishedOn)
    {
        PromoText = promoText;
    }

    public string PromoText { get; set; }
}

public class Tag
{
    public Tag(string id, string text)
    {
        Id = id;
        Text = text;
    }

    public string Id { get; private set; }
    public string Text { get; set; }
    public List<Post> Posts { get; } = new();
}

public class Author
{
    public Author(string name)
    {
        Name = name;
    }

    public int Id { get; private set; }
    public string Name { get; set; }
    public ContactDetails Contact { get; set; } = null!;
    public List<Post> Posts { get; } = new();
}

public class ContactDetails
{
    public Address Address { get; set; } = null!;
    public string? Phone { get; set; }
}

public class Address
{
    public Address(string street, string city, string postcode, string country)
    {
        Street = street;
        City = city;
        Postcode = postcode;
        Country = country;
    }

    public string Street { get; set; }
    public string City { get; set; }
    public string Postcode { get; set; }
    public string Country { get; set; }
}

public class PostMetadata
{
    public PostMetadata(int views)
    {
        Views = views;
    }

    public int Views { get; set; }
    public List<SearchTerm> TopSearches { get; } = new();
    public List<Visits> TopGeographies { get; } = new();
    public List<PostUpdate> Updates { get; } = new();
}

public class SearchTerm
{
    public SearchTerm(string term, int count)
    {
        Term = term;
        Count = count;
    }

    public string Term { get; private set; }
    public int Count { get; private set; }
}

public class Visits
{
    public Visits(double latitude, double longitude, int count)
    {
        Latitude = latitude;
        Longitude = longitude;
        Count = count;
    }

    public double Latitude { get; private set; }
    public double Longitude { get; private set; }
    public int Count { get; private set; }
    public List<string>? Browsers { get; set; }
}

public class PostUpdate
{
    public PostUpdate(IPAddress postedFrom, DateTime updatedOn)
    {
        PostedFrom = postedFrom;
        UpdatedOn = updatedOn;
    }

    public IPAddress PostedFrom { get; private set; }
    public string? UpdatedBy { get; init; }
    public DateTime UpdatedOn { get; private set; }
    public List<Commit> Commits { get; } = new();
}

public class Commit
{
    public Commit(DateTime committedOn, string comment)
    {
        CommittedOn = committedOn;
        Comment = comment;
    }

    public DateTime CommittedOn { get; private set; }
    public string Comment { get; set; }
}
```

### Summary

EF Core 7.0 (EF7) adds support for mapping aggregate types to JSON documents stored in "JSON columns" of a relational database. This allows relational databases to directly store documents while retaining the overall relational structure of the data. EF7 contains provider-agnostic support for JSON columns, with an implementation for SQL Server.  The JSON in these columns can queried using LINQ, allowing filtering and sorting by the elements of the documents, as well as projection of elements out of the documents into results. In addition, EF7 supports element-level change tracking of the documents and partial updates for only the changed elements when `SaveChanges` is called.

## EF7 Prerequisites

- EF7 targets .NET 6, which means it can be used on .NET 6 (LTS) or .NET 7.
- EF7 will not run on .NET Framework.

EF7 is the successor to EF Core 6.0, not to be confused with [EF6](https://github.com/dotnet/ef6). If you are considering upgrading from EF6, please read our guide to [port from EF6 to EF Core](https://docs.microsoft.com/ef/efcore-and-ef6/porting/).

## How to get EF7 RC2

EF7 is distributed exclusively as a set of NuGet packages. For example, to add the SQL Server provider to your project, you can use the following command using the dotnet tool:

```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 7.0.0-rc.2.22472.11
```

This following table links to the RC2 versions of the EF Core packages and describes what they are used for.

| **Package**                                                                                                                                                             | **Purpose**                                                                          |
|:------------------------------------------------------------------------------------------------------------------------------------------------------------------------|:-------------------------------------------------------------------------------------|
| [Microsoft.EntityFrameworkCore](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/7.0.0-rc.2.22472.11)                                                       | The main EF Core package                                                             |
| [Microsoft.EntityFrameworkCore.SqlServer](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer/7.0.0-rc.2.22472.11)                                   | Database provider for Microsoft SQL Server and SQL Azure                             |
| [Microsoft.EntityFrameworkCore.SqlServer.NetTopologySuite](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer.NetTopologySuite/7.0.0-rc.2.22472.11) | SQL Server support for spatial types                                                 |
| [Microsoft.EntityFrameworkCore.Sqlite](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite/7.0.0-rc.2.22472.11)                                         | Database provider for SQLite that includes the native binary for the database engine |
| [Microsoft.EntityFrameworkCore.Sqlite.Core](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite.Core/7.0.0-rc.2.22472.11)                               | Database provider for SQLite _without_ a packaged native binary                      |
| [Microsoft.EntityFrameworkCore.Sqlite.NetTopologySuite](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite.NetTopologySuite/7.0.0-rc.2.22472.11)       | SQLite support for spatial types                                                     |
| [Microsoft.EntityFrameworkCore.Cosmos](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Cosmos/7.0.0-rc.2.22472.11)                                         | Database provider for Azure Cosmos DB                                                |
| [Microsoft.EntityFrameworkCore.InMemory](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.InMemory/7.0.0-rc.2.22472.11)                                     | The in-memory database provider                                                      |
| [Microsoft.EntityFrameworkCore.Tools](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Tools/7.0.0-rc.2.22472.11)                                           | Visual Studio Package Manager Console commands for migrations and scaffolding        |
| [dotnet-ef](https://www.nuget.org/packages/dotnet-ef/7.0.0-rc.2.22472.11)                                                                                               | `dotnet` command line tools for migrations and scaffolding                           |
| [Microsoft.EntityFrameworkCore.Design](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Design/7.0.0-rc.2.22472.11)                                         | Shared design-time components for EF Core tools                                      |
| [Microsoft.EntityFrameworkCore.Proxies](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Proxies/7.0.0-rc.2.22472.11)                                       | Lazy-loading and change-tracking proxies                                             |
| [Microsoft.EntityFrameworkCore.Abstractions](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Abstractions/7.0.0-rc.2.22472.11)                             | Decoupled EF Core abstractions                                                       |
| [Microsoft.EntityFrameworkCore.Relational](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Relational/7.0.0-rc.2.22472.11)                                 | Shared EF Core components for relational database providers                          |
| [Microsoft.EntityFrameworkCore.Analyzers](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Analyzers/7.0.0-rc.2.22472.11)                                   | C# analyzers for EF Core                                                             |

The 7.0 RC2 release of the `Microsoft.Data.Sqlite.Core` [ADO.NET](https://docs.microsoft.com/dotnet/framework/data/adonet/ado-net-overview) provider is also [available from NuGet](https://www.nuget.org/packages/Microsoft.Data.Sqlite.Core/7.0.0-rc.2.22472.11).

## Installing the EF7 Command Line Interface (CLI)

Before you can execute EF7 Core migration or scaffolding commands, you'll have to install the CLI package as either a global or local tool.

To install the preview tool globally, install with:

```bash
dotnet tool install --global dotnet-ef --version 7.0.0-rc.2.22472.11 
```

If you already have the tool installed, you can upgrade it with the following command:

```bash
dotnet tool update --global dotnet-ef --version 7.0.0-rc.2.22472.11 
```

This new version of `dotnet-ef` is supported for projects that use older versions of the EF Core runtime.

## Daily builds

EF7 previews and RC releases are aligned with .NET 7 previews, which tend to lag behind the latest work on EF7. Consider using the [daily builds](https://github.com/aspnet/AspNetCore/blob/master/docs/DailyBuilds.md) instead to get the most up-to-date EF7 features and bug fixes.

## The .NET Data Community Standup

The .NET data team is now live streaming every other Wednesday at 10am Pacific Time, 1pm Eastern Time, or 18:00 UTC. Join the stream learn and ask questions about the many .NET Data related topics.

- [Watch our YouTube playlist](https://aka.ms/efstandups) of previous shows
- [Visit the .NET Community Standup](https://live.dot.net) page to preview upcoming shows
- [Submit your ideas](https://github.com/dotnet/efcore/issues/22700) for a guest, product, demo, or other content to cover

## Documentation and Feedback

The starting point for all EF Core documentation is [docs.microsoft.com/ef/](https://docs.microsoft.com/ef/).

Please file issues found and any other feedback on the [dotnet/efcore GitHub repo](https://github.com/dotnet/efcore).

## Helpful Links

The following links are provided for easy reference and access.

- [EF Core Community Standup Playlist: https://aka.ms/efstandups](https://aka.ms/efstandups)
- [Main documentation: https://aka.ms/efdocs](https://aka.ms/efdocs)
- [Issues and feature requests for EF Core: https://aka.ms/efcorefeedback](https://aka.ms/efcorefeedback)
- [Entity Framework Roadmap: https://aka.ms/efroadmap](https://aka.ms/efroadmap)
- [Bi-weekly updates: https://github.com/dotnet/efcore/issues/27185](https://github.com/dotnet/efcore/issues/27185)

## Thank you from the team

A big thank you from the EF team to everyone who has used and contributed to EF over the years!

Welcome to EF7.
