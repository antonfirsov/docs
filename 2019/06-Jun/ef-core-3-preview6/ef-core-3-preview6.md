# Announcing Entity Framework Core 3.0 Preview 6 and Entity Framework 6.3 Preview 6

New previews of the next versions of [EF Core](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer/3.0.0-preview6.19304.10) and [EF 6](https://www.nuget.org/packages/EntityFramework/6.3.0-preview6-19304-03) are now available on [NuGet.Org](https://www.nuget.org/).

## What is new in EF Core 3.0 Preview 6

In recent months, a lot of our efforts have been focused on a new LINQ implementation for EF Core 3.0. Although the work isn't complete and a lot of the intended functionality hasn't been enabled, before preview 6 we reached a point in which we couldn't make more progress without integrating the new implementation into the codebase in the main branch. 

### Query changes

> **IMPORTANT: Although as always, we want to encourage you to experiment with our preview bits in a controlled environment and to provide feedback, preview 6 has significant limitations in the LINQ implementation that we expect to affect any application that performs all but the most trivial queries. Given this, we want to explicitly recommend you against trying to update any applications you have in production to this preview.**
 
While some of the limitations are caused by intentional breaking changes, a lot more are temporary issues we expect to fix before RTM. 

These are some of the main things to know:

- **Temporary limitation: In-memory database and Cosmos DB providers aren't functional in this preview:** In the initial phase of the switch to the new implementation, we have prioritized getting our relational providers working. Functionality with in-memory database and Cosmos DB providers is broken, and we recommend you skip preview 6 if you have code that depends on these providers. We expect to gradually restore functionality in subsequent previews.  

- **Temporary limitation: Several areas of query translation aren't working with relational databases:** Queries that use any of these constructs will very likely fail to translate correctly or execute: 
  - Owned types
  - Collections reference on projections
  - GroupBy operator
  - Equality comparisons between entities 
  - Query tags 
  - Global query filters
  
- **Intentional breaking change: LINQ queries are no longer evaluated on the client:** This is actually one of the main motivations we had for building a new LINQ implementation into EF Core 3.0. Before this version of EF Core, expressions in the query that could not be translated for SQL would be automatically evaluated on the client, regardless of their location in the query. This contributed to hard to predict performance issues, especially when expressions used in predicates were not translated and large amounts of data ended up being filtered on the client, and also caused compatibility problems every time we introduced new translation capabilities. In the new implementation, we only support evaluating on the client expressions in the top-level projection of the query. Read [the full description of this breaking change](https://docs.microsoft.com/core/what-is-new/ef-core-3.0/breaking-changes#linq-queries-are-no-longer-evaluated-on-the-client) in our documentation. 

- **Intentional breaking change: Existing FromSql overloads has been renamed to FromSqlRaw and FromSqlInterpolated, and can only be used at the root of queries:** Read [more details about it](https://docs.microsoft.com/ef/core/what-is-new/ef-core-3.0/breaking-changes#fromsql-executesql-and-executesqlasync-have-been-renamed) in the breaking change documentation. 

If you run into any issues that you don't see in this list or in the [list of bugs already tracked for 3.0](https://github.com/aspnet/EntityFrameworkCore/milestone/42), please [report it on GitHub](https://github.com/aspnet/EntityFrameworkCore/issues/new).

If you hit limitations only in a few queries, here are some workarounds that you might be able to use to get things working:

- Switch explicitly to client evaluation if you need to filter data based on an expression that cannot be translated to SQL, using the `AsEnumerable()` or `ToList()` extension methods. For example, the following query will no longer work in EF Core 3.0 because one of the predicates in the where clause requires client evaluation:
   ``` csharp
   var specialCustomers = context.Customers
     .Where(c => c.Name.StartsWith(n) && IsSpecialCustomer(c));
   ```
   But if you know it is ok to perform part of the filtering on the client, you can rewrite this as:  
   ``` csharp
   var specialCustomers = context.Customers
     .Where(c => c.Name.StartsWith(n))
     .AsEnumerable()
     .Where(c => IsSpecialCustomer(c));
   ```

- Use the new `FromSqlRaw()` or `FromSqlInterpolated()` methods to provide your own SQL translations for anything that isn't yet supported.

- Skip preview 6 and stay on preview 5 until more issues are fixed, or try our [nightly builds](https://github.com/aspnet/AspNetCore/blob/master/docs/DailyBuilds.md). 

### Switch to Microsoft.Data.SqlClient

As [announced recently](https://devblogs.microsoft.com/dotnet/introducing-the-new-microsoftdatasqlclient/), development of the ADO.NET provider for SQL Server has moved to this new package. The EF Core provider for SQL Server now uses the new package to connect to SQL Server databases.

Please continue to create issues for the EF Core layer (for example SQL generation) on the [EF Core issue tracker](https://github.com/aspnet/EntityFrameworkCore/issues). Any issues related to SQL Server database connectivity are better reported to the [SqlClient issue tracker](https://github.com/dotnet/SqlClient/issues).  

### DbContext scaffolding improvements
We now have support for:
- Scaffolding entities without keys.
- Scaffolding entities from database views.
- Scaffolding `DbContext` from an Azure SQL Data Warehouse.
- A new `dotnet ef dbcontext script` command to generate the SQL script equivalent to calling `EnsureCreated()`.
- New Package Manager Console command functionality in `Get-DbContext` for listing `DbContext` types available in an application.
  
Thank you very much [@ErickEJ](https://github.com/ErikEJ) for all these contributions! You rock! 

## What's new in EF 6.3 Preview 6

Here are some of the main changes since preview 5:

- **We automatically locate System.Data.SqlClient when it isn’t registered with DbProviderFactories:** This means it's no longer necessary to register SqlClient as a workaround before you start using EF.

- **EF 6 tests are passing on .NET Core:** We made infrastructure updates necessary to get our existing tests running on .NET Core. This has allowed us to identify and fix problems in the product code as well as an issue that was fixed in .NET Core 3.0 Preview 6. There is a known issue with the translation of String methods like StartsWith, EndsWith and Contains that wasn't fixed on time for Preview 6. 

## What's next 

For upcoming previews and the rest of the EF Core 3.0 release, our main focus will be on restoring and improving the query functionality using our new LINQ implementation.

We are also working to enable more aspects of the experience you would expect from using EF 6.3 on .NET Core. For example, we are making additional changes to simplify working when an application .config file isn't present, enabling embedding metadata from an EDMX into the compilation output on .NET Core, and reimplementing the EF migration commands in a way that is compatible with .NET Core.

## Weekly status updates

If you'd like to track our progress more closely, we now publish weekly status updates to [GitHub](https://github.com/aspnet/EntityFrameworkCore/issues/15403). We also post these status updates to our Twitter account, [@efmagicunicorns](https://twitter.com/efmagicunicorns).  

## Thank you

As always, thank you for trying our preview bits, and thanks to all the contributors that helped in this preview!
