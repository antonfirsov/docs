# Announcing Entity Framework Core 3.0 Preview 4

Today, we are making the fourth preview of Entity Framework Core 3.0 available on [NuGet](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/3.0.0-preview4.19216.3), alongside [.NET Core 3.0 Preview 4](https://devblogs.microsoft.com/dotnet/announcing-net-core-3-preview-4/) and [ASP.NET Core 3.0 Preview 4](https://devblogs.microsoft.com/aspnet/asp-net-core-updates-in-net-core-3-0-preview-4).
We encourage you to install this preview to test the new functionality and assess the impact of the included breaking changes.

## What’s new in EF Core 3.0 Preview 4?

This preview includes more than 50 bug fixes and functional enhancements. You can query our issue tracker for the full list of issues fixed in [Preview 4](https://github.com/aspnet/EntityFrameworkCore/issues?utf8=%E2%9C%93&amp;q=is:issue+milestone:3.0.0-preview4+is:closed+label:closed-fixed+-label:area-docs+-label:area-test+-label:area-adonet-sqlite+sort:reactions-desc), as well as for the issues fixed in [Preview 3](https://github.com/aspnet/EntityFrameworkCore/issues?utf8=%E2%9C%93&amp;q=is:issue+milestone:3.0.0-preview3+is:closed+label:closed-fixed+-label:area-docs+-label:area-test+-label:area-adonet-sqlite+sort:reactions-desc), and [Preview 2](https://github.com/aspnet/EntityFrameworkCore/issues?utf8=%E2%9C%93&amp;q=is:issue+milestone:3.0.0-preview2+is:closed+label:closed-fixed+-label:area-docs+-label:area-test+-label:area-adonet-sqlite+sort:reactions-desc).

Some of the most important changes are:

### LINQ queries are no longer evaluated on the client 

The new LINQ implementation that we will introduce in an upcoming preview will not support automatic client evaluation except on the last `select` operator on your query. In preparation for this change, in preview 4 we have switched the [existing client evaluation behavior](https://docs.microsoft.com/ef/core/querying/client-eval#optional-behavior-throw-an-exception-for-client-evaluation) to throw by default.

Although it should still be possible to restore the old behavior by re-configuring client evaluation to only cause a warning, we recommend that you test your application with the new default.
If you see client evaluation exceptions, you can try modifying your queries to avoid client evaluation.
If that doesn't work, you can introduce calls to `AsEnumerable()` or `ToList()` to explicitly switch the processing of the query to the client in places where this is acceptable.
You can use raw SQL queries instead of LINQ if none of these options work for you.

See [the breaking change details](https://docs.microsoft.com/ef/core/what-is-new/ef-core-3.0/breaking-changes#linq-queries-are-no-longer-evaluated-on-the-client) to learn more about why we are making this change.

If you hit any case in which a LINQ expression isn't translated to SQL and you get a client evaluation exception instead, but you know exactly what SQL translation you expect to get, we ask you to [create an issue](https://github.com/aspnet/EntityFrameworkCore/issues/new) to help us improve EF Core.

### The EF Core runtime is no longer part of the ASP.NET Core shared framework 

_Note that this change was actually introduced in Preview 1 but was not previously announced in this blog._

The main consequence of this change is that no matter what type of application you are building or what database your application uses, you always obtain EF Core by installing the NuGet package for the EF Core provider of your choice.

In any operating system supported for .NET Core development, you can install the preview bits by installing a provider for EF Core 3.0 Preview 4.
For example, to install the SQLite provider, type this in the command line:

``` console
$ dotnet add package Microsoft.EntityFrameworkCore.Sqlite -v 3.0.0-preview4.19216.3
```

Or from the Package Manager Console in Visual Studio:

``` powershell
PM&gt; Install-Package Microsoft.EntityFrameworkCore.Sqlite -Version 3.0.0-preview4.19216.3
```

See the [breaking change details](https://docs.microsoft.com/ef/core/what-is-new/ef-core-3.0/breaking-changes#entity-framework-core-is-no-longer-part-of-the-aspnet-core-shared-framework) for more information.

### The dotnet ef tool is no longer part of the .NET Core SDK

This change allows us to ship `dotnet ef` as a regular .NET CLI tool that can be installed as either a global or local tool.For example, to be able to manage migrations or scaffold a `DbContext`, install `dotnet ef` as a global tool typing the following command:

``` console
$ dotnet tool install --global dotnet-ef --version 3.0.0-preview4.19216.3
```

For more information, see [this breaking change's details](https://docs.microsoft.com/ef/core/what-is-new/ef-core-3.0/breaking-changes#the-ef-core-command-line-tool-dotnet-ef-is-no-longer-part-of-the-net-core-sdk).

### Dependent entities sharing tables with their principal entities can now be optional 

This enables for example, owned entities mapped to the same table as the owner to be optional, which is a frequently requested improvement.

See issue [#9005](https://github.com/aspnet/EntityFrameworkCore/issues/9005) for more details.

### Key generation improvements for in-memory database 

We have made several improvements to simplify using in-memory database for unit testing.
For example, each generated property now gets incremented independently.
Also, when the database is deleted, key generation is reset and starts at 1.
Finally, if a property in an entity contains a value larger than the last value returned by the generator, the latter is bumped to start on the next available value.

For more details, see issue [#6872](https://github.com/aspnet/EntityFrameworkCore/issues/6872).

### Separate methods for working with raw SQL queries as plain strings or interpolated strings 

The existence of method overloads accepting SQL as plain strings or interpolated strings made it very hard to predict which version would be used and how parameters would be processed.
To eliminate this source of confusion, we added the new methods `FromSqlRaw`, `FromSqlInterpolated`, `ExecuteSqlRaw`, and `ExecuteSqlInterpolated`.
The existing `FromSql` and `ExecuteSqlCommand` methods are now obsolete.

See issue [#10996](https://github.com/aspnet/EntityFrameworkCore/issues/10996) for more details.

### Database connection no longer remains open until a TransactionScope is disposed

This makes EF Core work better with database providers that are optimized to work with System.Transactions, like SqlClient and Npgsql.

See [this breaking change's details](https://docs.microsoft.com/ef/core/what-is-new/ef-core-3.0/breaking-changes#database-connection-is-now-closed-if-not-used-anymore-before-the-transactionscope-has-been-completed) for more information.

### Code Analyzer that detects usage of EF Core internal APIs 

EF Core exposes some of its internal APIs in public types.
For example, all types under nested EF Core namespaces named `Internal` are considered internal APIs even if they are technically public types.
In the past, this made it easy for application developers and provider writers to unintentionally use internal APIs in their code.
With this new analyzer, using EF Core internal APIs produces a warning by default.
For example:

_EF1001: Microsoft.EntityFrameworkCore.Internal.MethodInfoExtensions is an internal API that supports the Entity Framework Core infrastructure and not subject to the same compatibility standards as public APIs. It may be changed or removed without notice in any release._

If the usage is intentional, the warning can be suppressed like any code analysis warning.

See issue [#12104](https://github.com/aspnet/EntityFrameworkCore/issues/12104) for more details.

### Improved performance of iterating over tracked entities using .Local 

We have improved the implementation so that iterating over the contents of `.Local` is around three times faster.
If you are going to iterate multiple times, you should still consider calling `ToObservableCollection()` first.

See issue [#14231](https://github.com/aspnet/EntityFrameworkCore/issues/14231) for more details.

### The concept of query types has been renamed to entities without keys 

_Note that this change was included in preview 3 but wasn’t previously announced in this blog._

Query types were introduced in EF Core 2.1 to enable reading data from database tables and views that didn't contain unique keys.
Having query types as a separate concept from entity types has proven to obscure their purpose and what makes them different.
It also led us to have to introduce some undesired redundancy and unintended inconsistencies in our APIs.
We believe the consolidation of the concepts and eventual removal of the query types API will help reduce the confusion.

See [the breaking change details](https://docs.microsoft.com/ef/core/what-is-new/ef-core-3.0/breaking-changes#query-types-are-consolidated-with-entity-types) for more information.

### New preview of the Cosmos DB provider for EF Core 

Although the work is still far from complete, we have been making progress on the provider in successive previews of EF Core 3.0.
For example, the new version takes advantage of the [new Cosmos DB SDK](https://www.nuget.org/packages/Microsoft.Azure.Cosmos/), enables customizing the names of properties used in the storage, handles value conversions, uses a deterministic approach for key generation, and allows specifying the Azure region to use.

For full details, see the [Cosmos DB provider task list](https://github.com/aspnet/EntityFrameworkCore/issues/12086) on our issue tracker.

## What’s next? 

As detailed [in our list of new features](https://docs.microsoft.com/ef/core/what-is-new/ef-core-3.0/features), in 3.0 we plan to deliver an improved LINQ implementation, Cosmos DB support, support for C# 8.0 features like nullable reference types and async collections, reverse engineering of database views, property bag entities, and a new version of EF6 that can run on .NET Core.

In the first 4 previews, we focused most of our efforts on the improvements that we believed could have the highest impact on existing applications and database providers tying to upgrade from previous versions.
Implementing these "breaking changes" and architectural improvements early on gives us more time to gather feedback and react to unanticipated issues.
But it also means that many of those important features have had to wait until later previews.
At this point, you can expect initial support for most of the 3.0 features to arrive within the next couple of preview cycles.

In order to make it easier for you to track our progress, from now on, we will be posting <a href="https://github.com/aspnet/EntityFrameworkCore/issues/15403">weekly status updates</a> on GitHub.
You can subscribe to notifications to get an email every time we post an update.

We would also like to take this opportunity to talk about important adjustments we are making moving forward:

### EF Core 3.0 will target .NET Standard 2.1 

After investigating several options, we have concluded that making EF Core 3.0 target .NET Standard 2.1 is the best path to take advantage of new features in .NET and C#, like integration with `IAsyncEnumerable&lt;T&gt;`.
This is consistent with similar decisions announced last year for ASP.NET Core 3.0 and C# 8.0, and enables us to move forward without introducing compatibility adapters that could hinder our ability to deliver great new functionality on .NET Core down the road.

Although Preview 4 still targets .NET Standard 2.0, the RTM version will require 2.1 and therefore will not be compatible with .NET implementations that only support .NET Standard 2.0, like .NET Framework.
Customers that need to run EF Core on .NET Framework, should plan to continue using EF Core 2.1 or 2.2.

### EF 6.3 will target .NET Standard 2.1, in addition to already supported .NET Framework versions 

We found that the API surface offered by .NET Standard 2.1 is adequate for supporting EF6 without removing any runtime features.
In addition to .NET Standard 2.1, the EF 6.3 NuGet package will as usual support .NET Framework 4.0 (without async support) and .NET Framework 4.5 and newer.
For the time being, we are only going to test and support EF 6.3 running on .NET Framework and .NET Core 3.0. 
But targeting .NET Standard 2.1 opens the possibility to have EF 6.3 working on other .NET implementations, as long as they fully implement the standard and don’t have other runtime limitations.

## Closing

The EF team would like to thank everyone for their feedback and their contributions to this preview.
Once more, we encourage you to try out the latest bits and submit any new feedback to [our issue tracker](https://github.com/aspnet/EntityFrameworkCore/issues/new).
