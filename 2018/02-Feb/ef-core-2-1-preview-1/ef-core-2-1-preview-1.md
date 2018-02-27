# Announcing Entity Framework Core 2.1 Preview 1
Today we are releasing the first preview of EF Core 2.1\. The new bits are available in NuGet as part of the individual packages, and as part of the ASP.NET Core meta-packages (both Microsoft.AspNetCore.All and the new Microsoft.AspNetCore.App), and included in the .NET Core SDK.

The majority of the new features of EF Core 2.1 are present and ready to try in Preview 1: We want to maximize the exposure of the new features to you, our customers, and the amount of time we will have to address your feedback before the final release. We would like to encourage you try this preview release and to let us know what you think.

## Obtaining the bits

Depending on your development environment, to install, you can use NuGet or the `dotnet` command line interface.

If you are using one of the database providers developed as part of the Entity Framework Core project (e.g. SQL Server, SQLite or In-Memory), you can install EF Core 2.1 Preview 1 by installing the latest version of the provider. For example, using `dotnet` on the command line:

``` console
$ dotnet add package Microsoft.EntityFrameworkCore.SqlServer -V 2.1.0-preview1-final
```

If you are using another EF Core 2.0-compatible relational database provider, you can get the new EF Core bits by adding the base relational provider, e.g.:

``` console
$ dotnet add package Microsoft.EntityFrameworkCore.Relational -V 2.1.0-preview1-final
```

Note that although there are a few features, such as value conversions, that require an updated database provider, existing providers developed for EF Core 2.0 should be compatible with EF Core 2.1\. If there is any incompatibility, that is [a bug we want to hear about](https://github.com/aspnet/EntityFrameworkCore/issues/new)!

Some additional providers with support for EF Core 2.1 Preview, like [Npgsql.EntityFrameworkCore.PostgreSQL](https://www.nuget.org/packages/Npgsql.EntityFrameworkCore.PostgreSQL/), will be available soon.

## New features

Besides numerous small improvements and more than a hundred product bug fixes, EF Core 2.1 includes several frequently requested new features:

### Lazy loading

EF Core now contains the necessary building blocks for anyone to author entity classes that can load their navigation properties on demand. We have also created a new package, Microsoft.EntityFrameworkCore.Proxies, that leverages those building blocks to produce lazy loading proxy classes based on minimally modified entity classes (e.g. classes with virtual navigation properties).

Read the [section on lazy loading](https://docs.microsoft.com/ef/core/querying/related-data#lazy-loading) for more information about this topic.

### Parameters in entity constructors

As one of the required building blocks for lazy loading, we enabled the creation of entities that take parameters in their constructors. You can use parameters to inject property values, lazy loading delegates, and services.

Read the [section on entity constructor with parameters](https://docs.microsoft.com/ef/core/modeling/constructors) for more information about this topic.

### Value conversions

Until now, EF Core could only map properties of types natively supported by the underlying database provider. Values were copied back and forth between columns and properties without any transformation. Starting with EF Core 2.1, value conversions can be applied to transform the values obtained from columns before they are applied to properties, and vice versa. We have a number of conversions that can be applied by convention as necessary, as well as an explicit configuration API that allows registering custom conversions between columns and properties. Some of the application of this feature are:

*   Storing enums as strings
*   Mapping unsigned integers with SQL Server
*   Automatic encryption and decryption of property values

Read the [section on value conversions](https://docs.microsoft.com/ef/core/modeling/value-conversions) for more information about this topic.

### LINQ GroupBy translation

Before version 2.1, in EF Core the GroupBy LINQ operator was always be evaluated in memory. We now support translating it to the SQL GROUP BY clause in most common cases. This example shows a query with GroupBy used to compute various aggregate functions:

``` csharp
    var query = context.Orders
        .GroupBy(o => new { o.CustomerId, o.EmployeeId })
        .Select(g => new
            {
              g.Key.CustomerId,
              g.Key.EmployeeId,
              Sum = g.Sum(o => o.Amount),
              Min = g.Min(o => o.Amount),
              Max = g.Max(o => o.Amount),
              Avg = g.Average(o => Amount)
            });
```

The corresponding SQL translation looks like this:

``` SQL
    SELECT [o].[CustomerId], [o].[EmployeeId],
        SUM([o].[Amount]), MIN([o].[Amount]), MAX([o].[Amount]), AVG([o].[Amount])
    FROM [Orders] AS [o]
    GROUP BY [o].[CustomerId], [o].[EmployeeId];
```

### Data Seeding

With the new release it will be possible to provide initial data to populate a database. Unlike in EF6, seeding data is associated to an entity type as part of the model configuration. Then EF Core migrations can automatically compute what insert, update or delete operations need to be applied when upgrading the database to a new version of the model. As an example, you can use this to configure seed data for a Post in `OnModelCreating`:

``` csharp
modelBuilder.Entity<Post>().SeedData(new Post{ Id = 1, Text = "Hello World!" });
```
Read the [section on data seeding](https://docs.microsoft.com/ef/core/modeling/data-seeding) for more information about this topic.

### Query types

An EF Core model can now include query types. Unlike entity types, query types do not have keys defined on them and cannot be inserted, deleted or updated (i.e. they are read-only), but they can be returned directly by queries. Some of the usage scenarios for query types are:

*   Mapping to views without primary keys
*   Mapping to tables without primary keys
*   Mapping to queries defined in the model
*   Serving as the return type for `FromSql()` queries

Read the [section on query types](https://docs.microsoft.com/ef/core/modeling/query-types) for more information about this topic.

### Include for derived types

It will be now possible to specify navigation properties only defined on derived types when writing expressions for the `Include` method. For the strongly typed version of `Include`, we support using either an explicit cast or the `as` operator. We also now support referencing the names of navigation property defined on derived types in the string version of `Include`:

``` csharp
var option1 = context.People.Include(p => ((Student)p).School);
var option2 = context.People.Include(p => (p as Student).School);
var option3 = context.People.Include("School");
```

### System.Transactions support

We have added the ability to work with System.Transactions features such as TransactionScope. This will work on both .NET Framework and .NET Core when using database providers that support it.

Read the [section on System.Transactions](https://docs.microsoft.com/ef/core/saving/transactions#using-systemtransactions) for more information about this topic.

### Better column ordering in initial migration

Based on customer feedback, we have updated migrations to initially generate columns for tables in the same order as properties are declared in classes. Note that EF Core cannot change order when new members are added after the initial table creation.

### Optimization of correlated subqueries

We have improved our query translation to avoid executing "N + 1" SQL queries in many common scenarios in which the usage of a navigation property in the projection leads to joining data from the root query with data from a correlated subquery. The optimization requires buffering the results form the subquery, and we require that you modify the query to opt-in the new behavior.

As an example, the following query normally gets translated into one query for Customers, plus N (where "N" is the number of customers returned) separate queries for Orders:

``` csharp
var query = context.Customers.Select(
    c => c.Orders.Where(o => o.Amount  > 100).Select(o => o.Amount));
```

By including `ToList()` in the right place, you indicate that buffering is appropriate for the Orders, which enable the optimization:

``` csharp
var query = context.Customers.Select(
    c => c.Orders.Where(o => o.Amount  > 100).Select(o => o.Amount).ToList());
```

Note that this query will be translated to only two SQL queries: One for Customers and the next one for Orders.

### OwnedAttribute

It is now possible to configure [owned entity types](https://docs.microsoft.com/ef/core/modeling/owned-entities) by simply annotating the type with `[Owned]` and then making sure the owner entity is added to the model:

``` csharp
[Owned]
public class StreetAddress
{
    public string Street { get; set; }
    public string City { get; set; }
}

public class Order
{
    public int Id { get; set; }
    public StreetAddress ShippingAddress { get; set; }
}
```

The new attribute is defined in a new package: Microsft.EntityFrameworkCore.Attributes. Our current thinking is that this package will host other EF Core-specific attributes.

Read the [section on owned entity types](https://docs.microsoft.com/ef/core/modeling/owned-entities) for more information about this topic.

## What's next

As mentioned in our [roadmap post](https://blogs.msdn.microsoft.com/dotnet/2018/02/02/entity-framework-core-2-1-roadmap/) earlier this month, we intend to release additional previews monthly, and a final release in the first half of 2018. In the meanwhile, the team has also been busy with other projects, including:

*   an EF Core provider for Cosmos DB (the current plan is to publish an early preview in the 2.1 timeframe)
*   a sample EF Core provider for Oracle databases
*   [an initiative to improve the performance of data access in .NET](https://github.com/aspnet/dataaccessperformance/), in collaboration with the community of database provider writers , and various teams across .NET

## Thank you!

As always, we want to express our gratitude to everyone that has helped making the 2.1 release better by providing feedback, reporting bugs, and contributing code.

**Please try the preview bits, and [keep the feedback coming](https://github.com/aspnet/EntityFrameworkCore/issues/new)!**
