# Announcing Entity Framework Core 2.1 Preview 2
Today we’re releasing the second preview of EF Core 2.1, alongside [.NET Core 2.1 Preview 2](https://blogs.msdn.microsoft.com/dotnet/2018/04/11/announcing-net-core-2-1-preview-2/) and [ASP.NET Core 2.1 Preview 2](https://blogs.msdn.microsoft.com/webdev/).

Thank you so much to everyone who has tried our early builds and has helped shape this release with their feedback and code contributions!

The new preview bits are now available [in NuGet as individual packages](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/2.1.0-preview2-final), and as part of the [ASP.NET Core 2.1 Preview 2 metapackage](https://www.nuget.org/packages/Microsoft.AspNetCore.App/2.1.0-preview2-final) and in the [.NET Core 2.1 Preview 2 SDK](https://www.microsoft.com/net/download/dotnet-core/sdk-2.1.300-preview2), also released today.

## Changes since Preview 1

As we [announced in February, the first preview](https://blogs.msdn.microsoft.com/dotnet/2018/02/27/announcing-entity-framework-core-2-1-preview-1/) contained the initial implementation of all the major features we have planned for the 2.1 release: _GroupBy_ translation, lazy loading, parameters in entity constructors, value conversion, query types, data seeding, _System.Transactions_ support, among others.

For a more complete description of the new features in 2.1, see the [What's New section](https://docs.microsoft.com/ef/core/what-is-new/ef-core-2.1) in our documentation. We will keep this section up to date as we make progress.

For the second preview, we have made dozens of bug fixes, functional and performance improvements. For the complete set of issues addressed in Preview 2, see [this query](https://github.com/aspnet/EntityFrameworkCore/issues?q=is%3Aissue+milestone%3A2.1.0-preview2+label%3Aclosed-fixed) on our issue tracker.

Among all the improvements, the following are worth mentioning here:

### Refinement of the new 2.1 features

- The _GroupBy_ LINQ operator is now translated to SQL for more query patterns, including:
  - Grouping by constants or variables.
  - Grouping by scalar properties on reference navigations.
  - Grouping in combination with ordering or filtering on the grouping key or an aggregate.
  - Grouping in combination with projecting into an unmapped nominal type.

  For example, the following query can now be translated to SQL using GROUP BY and HAVING clauses:

  ``` csharp
  var activeCustomers = db.Orders
    .GroupBy(o => o.CustomerID)
    .Where(o => o.Count() > 0)
    .Select(g => new { g.Key, Count = g.Count() });
  ```

- Query types can now be used to create explicitly compiled queries and can be configured in a separate class that implements _IQueryTypeConfiguration&lt;TQuery&gt;_
- Data seeding now works with in-memory databases.  
- In general, new features like value conversions, data seeding and query types now work better in many more scenarios, and in combination with each other and with other existing features.
- New APIs have been reviewed for consistency and in some cases renamed. For example, the _SeedData_ method was renamed to _HasData_.

### Minor new features and enhancements
- The _dotnet-ef_ commands now ship as part of the .NET Core SDK, so it will no longer be necessary to use _DotNetCliToolReference_ in the project to be able to use migrations or to scaffold a DbContext from an existing database.
- A new _Microsoft.EntityFrameworkCore.Abstractions_ package contains attributes and interfaces that you can use in your projects to light up EF Core features without taking a dependency on EF Core as a whole. For example, the _[Owned]_ attribute introduced in Preview 1 was moved here.
- New _Tracked_ And _StateChanged_ events on _ChangeTracker_ can be used to write logic that reacts to entities entering the DbContext or changing their state.
- A new code analyzer is included with EF Core that detects possibly unsafe usages of our raw-SQL APIs in which parameters are not generated.
- The _dotnet ef dbcontext scaffold_ and the _Scaffold-DbContext_ commands can now use named connection strings (that is, connection strings of the form _Name=some_name_) that reference connection strings defined in configuration (including user secrets).
- Table splitting can now be used for relationships declared on derived types. This is useful for owned types.

## Obtaining the bits

EF Core 2.1 Preview 2 and the corresponding versions of the SQL Server and in-memory database providers are included in the ASP.NET Core metapackage [_Microsoft.AspNetCore.App_ 2.1 Preview 2](https://www.nuget.org/packages/Microsoft.AspNetCore.App/2.1.0-preview2-final). Therefore, if your application references the metapackage and uses these database provides, you will not need any additional installation steps.

Otherwise, depending on your development environment, in order to install the new bits in your application, you can use NuGet or the _dotnet_ command-line interface.

If you’re using one of the database providers developed as part of the Entity Framework Core project (for example, SQL Server, SQLite or In-Memory), you can install EF Core 2.1 Preview 2 by installing the latest version of the provider. For example, using _dotnet_ on the command-line:

``` console
$ dotnet add package Microsoft.EntityFrameworkCore.SqlServer -V 2.1.0-preview2-final
```

If you’re using another EF Core 2.0-compatible relational database provider, it's recommended that in order to obtain all the newest EF Core bits, you add a direct reference to the base relational provider in your application, for example:

``` console
$ dotnet add package Microsoft.EntityFrameworkCore.Relational -V 2.1.0-preview2-final
```

## Provider compatibility

Some of the new features in 2.1, such as value conversions, require an updated database provider. However it's our goal that existing providers developed for EF Core 2.0 will be compatible with EF Core 2.1, or at most require minimal updates.

We have been working and will continue to work with provider writers to make sure we identify and address any breaking changes.

If you experience any incompatibility, please report it by [creating an issue in our GitHub repository](https://github.com/aspnet/EntityFrameworkCore/issues/new).

Providers with support for new 2.1 features that are also compatible with Preview 2 will be available soon.

## What's next

As mentioned in our [roadmap post](https://blogs.msdn.microsoft.com/dotnet/2018/02/02/entity-framework-core-2-1-roadmap/) in February, we intend to release additional previews on a monthly cadence, and a final release within the next two or three months.

Regarding other ongoing projects, we still plan to release a preview of our Cosmos DB provider in the 2.1 timeframe, and a final release around the 2.2 timeframe.

## Thank you!

Again, we want to express our deep gratitude to everyone that has helped making the 2.1 release better by trying early builds, providing feedback, reporting bugs, and contributing code.

Please try the preview bits, and keep posting any new feedback to [our issue tracker](https://github.com/aspnet/EntityFrameworkCore/issues/new)!
