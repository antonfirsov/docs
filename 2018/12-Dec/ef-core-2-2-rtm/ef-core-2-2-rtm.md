# Announcing Entity Framework Core 2.2

Today we're making the final version of EF Core 2.2 available, alongside [ASP.NET Core 2.2](https://aka.ms/aspnetcore22announce) and [.NET Core 2.2](https://aka.ms/netcore22announce). This is the latest release of our open-source and cross-platform object-database mapping technology.  

EF Core 2.2 RTM includes more than a hundred bug fixes and a few new features:  

## Spatial data support

Spatial data can be used to represent the physical location and shape of objects.
Many databases can natively store, index, and query spatial data.
Common scenarios include querying for objects within a given distance, and testing if a polygon contains a given location.
EF Core 2.2 now supports working with spatial data from various databases using types from the [NetTopologySuite](https://github.com/NetTopologySuite/NetTopologySuite) (NTS) library.

Spatial data support is implemented as a series of provider-specific extension packages.
Each of these packages contributes mappings for NTS types and methods, and the corresponding spatial types and functions in the database.
Such provider extensions are now available for [SQL Server](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer.NetTopologySuite/), [SQLite](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite.NetTopologySuite/), and [PostgreSQL](https://www.nuget.org/packages/Npgsql.EntityFrameworkCore.PostgreSQL.NetTopologySuite/) (from the [Npgsql project](http://www.npgsql.org/)).
Spatial types can be used directly with the [EF Core in-memory provider](https://docs.microsoft.com/en-us/ef/core/providers/in-memory/) without additional extensions.

Once the provider extension is installed, you can add properties of supported types to your entities. For example:

``` csharp
using NetTopologySuite.Geometries;

namespace MyApp
{
  public class Friend
  {
    [Key]
    public string Name { get; set; }

    [Required]
    public Point Location { get; set; }
  }
}
```

You can then persist entities with spatial data:

``` csharp
using (var context = new MyDbContext())
{
    context.Add(
        new Friend
        {
            Name = "Bill",
            Location = new Point(-122.34877, 47.6233355) {SRID = 4326 }
        });
    context.SaveChanges();
}
```
And you can execute database queries based on spatial data and operations:

``` csharp
  var nearestFriends =
      (from f in context.Friends
      orderby f.Location.Distance(myLocation) descending
      select f).Take(5).ToList();
```

For more information on this feature, see the [spatial data documentation](https://docs.microsoft.com/ef/core/modeling/spatial).

## Collections of owned entities

EF Core 2.0 added the ability to model ownership in one-to-one associations.
EF Core 2.2 extends the ability to express ownership to one-to-many associations.
Ownership helps constrain how entities are used.

For example, owned entities:
- Can only ever appear on navigation properties of other entity types.
- Are automatically loaded, and can only be tracked by a DbContext alongside their owner.

In relational databases, owned collections are mapped to separate tables from the owner, just like regular one-to-many associations.
But in document-oriented databases, we plan to nest owned entities (in owned collections or references) within the same document as the owner.

You can use the feature by calling the new OwnsMany() API:

``` csharp
modelBuilder.Entity<Customer>().OwnsMany(c => c.Addresses);
```

For more information, see the [updated owned entities documentation](https://docs.microsoft.com/ef/core/modeling/owned-entities#collections-of-owned-types).

## Query tags

This feature simplifies the correlation of LINQ queries in code with generated SQL queries captured in logs.

To take advantage of query tags, you annotate a LINQ query using the new TagWith() method.
Using the spatial query from a previous example:

``` csharp
  var nearestFriends =
      (from f in context.Friends.TagWith(@"This is my spatial query!")
      orderby f.Location.Distance(myLocation) descending
      select f).Take(5).ToList();
```

This LINQ query will produce the following SQL output:

``` sql
-- This is my spatial query!

SELECT TOP(@__p_1) [f].[Name], [f].[Location]
FROM [Friends] AS [f]
ORDER BY [f].[Location].STDistance(@__myLocation_0) DESC
```

For more information, see the [query tags documentation](https://docs.microsoft.com/ef/core/querying/tags).

## Getting EF Core 2.2

The EF Core NuGet packages are available [on the NuGet Gallery](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/2.2.0), and also as part of [ASP.NET Core 2.2](https://aka.ms/aspnetcore22announce) and the [new .NET Core SDK](https://dotnet.microsoft.com/download/dotnet-core/2.2).

If you want to use EF Core in an application based on ASP.NET Core, we recommend that first you upgrade your application to [ASP.NET Core 2.2](https://aka.ms/aspnetcore22announce).

In general, the best way to use EF Core in an application is to install the corresponding NuGet package for the provider your application will use. For example, to add the 2.2 version of the SQL Server provider in a .NET Core project from the command line, use:

``` console
$ dotnet add package Microsoft.EntityFrameworkCore.SqlServer -v 2.2.0
```

Or from the Package Manager Console in Visual Studio:

``` console
PM> Install-Package Microsoft.EntityFrameworkCore.SqlServer -Version 2.2.0
```

For more information on how to add EF Core to your projects, see [our documentation on Installing Entity Framework Core](https://docs.microsoft.com/ef/core/get-started/install/).

## Compatibility with EF Core 2.1

We spent much time and effort making sure that EF Core 2.2 is backwards compatible with existing EF Core 2.1 providers, and that updating an application to build on EF Core 2.2 won't cause compatibility issues. We expect most upgrades to be smooth, however if you find any unexpected issues, please report them to our [issue tracker](https://github.com/aspnet/EntityFrameworkCore/issues/new).

There is one known change in EF Core 2.2 that could require minor updates in application code. Read the description of the following issue for more details:

- *[#13986](https://github.com/aspnet/EntityFrameworkCore/issues/13986) Type configured as both owned entity and regular entity requires a primary key to be defined after upgrading from 2.1 to 2.2*

We intend to maintain [a list of issues that may require adjustments to existing code](
https://github.com/aspnet/EntityFrameworkCore/issues?utf8=%E2%9C%93&q=is%3Aissue+label%3Aconsider-for-release-notes+milestone%3A2.2.0) on our issue tracker.

## What's next: EF Core 3.0

With EF Core 2.2 out the door, our main focus is now EF Core 3.0. There are several details of the next major release that we still need to figure out, but here are some of the main themes we know so far:

- **LINQ improvements**: LINQ enables you to write database queries without leaving your language of choice, taking advantage of rich type information to get IntelliSense and compile-time type checking. But LINQ also enables you to write an unlimited number of complicated queries, and that has always been a huge challenge for LINQ providers. In the first few versions of EF Core, we solved that in part by figuring out what portions of a query could be translated to SQL, and then by allowing the rest of the query to execute in memory on the client. This client-side execution can be desirable in some situations, but in many other cases it can result in inefficient queries that may not identified until an application is deployed to production. In EF Core 3.0, we are planning to make profound changes to how our LINQ implementation works, and how we test it. The goals are to make it more robust (for example, to avoid breaking queries in patch releases), to be able to translate more expressions correctly into SQL, to generate efficient queries in more cases, and to prevent inefficient queries from going undetected.

- **Cosmos DB support**: We're working on a Cosmos DB provider for EF Core, to enable developers familiar with the EF programing model to easily target Azure Cosmos DB as an application database. The goal is to make some of the advantages of Cosmos DB, like global distribution, “always on” availability, elastic scalability, and low latency, even more accessible to .NET developers. The provider will enable most EF Core features, like automatic change tracking, LINQ, and value conversions, against the SQL API in Cosmos DB. We started this effort before EF Core 2.2, and [we have made some preview versions of the provider available](https://blogs.msdn.microsoft.com/dotnet/2018/10/17/announcing-entity-framework-core-2-2-preview-3/). The new plan is to continue developing the provider alongside EF Core 3.0.   

- **C# 8.0 support**: We want our customers to take advantage some of the [new features coming in C# 8.0](https://blogs.msdn.microsoft.com/dotnet/2018/11/12/building-c-8-0/) like async streams (including await for each) and nullable reference types while using EF Core.

- **Reverse engineering database views into query types:** In EF Core 2.1, we added support for query types, which can represent data that can be read from the database, but cannot be updated. Query types are a great fit for mapping database views, so in EF Core 3.0, we would like to automate the creation of query types for database views.

- **Property bag entities**: This feature is about enabling entities that store data in indexed properties instead of regular properties, and also about being able to use instances of the same .NET class (potentially something as simple as a `Dictionary<string, object>`) to represent different entity types in the same EF Core model. This feature is a stepping stone to support many-to-many relationships without a join entity, which is one of the most requested improvements for EF Core.

- **EF 6.3 on .NET Core**: We understand that many existing applications use previous versions of EF, and that porting them to EF Core only to take advantage of .NET Core can sometimes require a significant effort. For that reason, we will be adapting the next version of EF 6 to run on .NET Core 3.0. We are doing this to facilitate porting existing applications with minimal changes. There are going to be some limitations (for example, it will require new providers, spatial support with SQL Server won't be enabled), and there are no new features planned for EF 6.

## Thank you

The EF team would like to thank everyone for all the community feedback and contributions that went into EF Core 2.2. Once more, you can report any new issues you find on [our issue tracker](https://github.com/aspnet/EntityFrameworkCore/issues/new).
