---
post_title: 'The Plan for Entity Framework Core 6.0'
username: jeremy-likness
microsoft_alias: jeliknes
featured_image: efcore6.png
categories: .NET, .NET Core, Entity Framework
summary: This blog post details the roadmap for Entity Framework Core 6.0.
desired_publication_date: 2021-01-13
---

Today we are excited to share with you [the plan for Entity Framework Core 6.0](https://docs.microsoft.com/ef/core/what-is-new/ef-core-6.0/plan).

![EF Core 6.0](./efcore6.png)

This plan brings together input from many stakeholders and outlines where and how we intend to invest for the Entity Framework (EF Core) 6.0 release. 
This plan is not set-in-stone and will evolve as we work on the release based on what we learn. This learning includes feedback from people like you, 
so please let us know what you think!

> **IMPORTANT**
> This plan is not a commitment. It is a starting point that will evolve as we learn more. Some things not currently planned for 6.0 may get pulled in. 
Some things currently planned for 6.0 may get punted out.

## General information

EF Core 6.0 is the next release after EF Core 5.0 and is currently scheduled for November 2021 at the same time as .NET 6. EF Core 6.0 will align with .NET 6 as 
a [long-term support (LTS) release](https://dotnet.microsoft.com/platform/support/policy/dotnet-core).

EF Core 6.0 will likely target .NET 6 when released. It is unlikely to support any .NET Standard version. It will not run on .NET Framework. 
See [the future of .NET Standard](https://devblogs.microsoft.com/dotnet/the-future-of-net-standard/) for more information.

## Themes

### Highly requested features

As always, a major input into the [planning process](https://docs.microsoft.com/ef/core/what-is-new/release-planning) comes from the [voting (👍) for features on GitHub](https://github.com/dotnet/efcore/issues?q=is%3Aissue+is%3Aopen+sort%3Areactions-%2B1-desc). For EF Core 6.0 we plan to work on the following highly requested features:

- SQL Server temporal tables
  - Allow temporal tables to be created via Migrations, as well as allowing access to historical data through LINQ queries.
- JSON columns
  - Introduce common patterns for JSON support that can be implemented by any database provider.
  - JSON column support will be implemented for SQL Server and SQLite. (Note that the PostgreSQL and MySQL providers already support JSON columns.)
- `ColumnAttribute.Order`
  - Allow arbitrary ordering of columns when _creating a table_ with Migrations or `EnsureCreated`.

### Performance

While EF Core is generally faster than EF6, there are still areas where significant improvements in performance are possible. We plan to tackle several of these areas in EF Core 6.0, while also improving our perf infrastructure and testing.

- Performance infrastructure and new tests
  - Improve the infrastructure for performance tests as well as adding new tests and fixing low-hanging fruit.
- Compiled models
  - Compiled models will improve startup performance, as well as having generally better performance when accessing the model.
- TechEmpower Fortunes
  - We plan to match Dapper performance on the TechEmpower Fortunes benchmark. (This is a significant challenge which will likely not be fully achieved. Nevertheless, we will get as close as we can.)
- Linker/AOT
  - We will continue investigating in making EF Core work better with linkers and AOT. We do not expect to fully close the gap in the 6.0 timeframe, but we hope to make significant progress.

### Migrations and deployment

Following on from the [investigations done for EF Core 5.0](https://docs.microsoft.com/ef/core/what-is-new/ef-core-5.0/plan#migrations-and-deployment-experience), we plan to introduce improved support for managing migrations and deploying databases. This includes two major areas:

- Migrations bundles
  - Migrations bundles will provide a simple, robust mechanism for deploying EF Core migrations.
- Managing migrations
  - Wwe plan to improve the tools and project/assembly management for EF Core migrations.

### Improve existing features and fix bugs

- EF6 query parity
  - We plan to close the query gap to EF6 and make supported EF Core queries a true superset of supported EF6 queries.
- Value objects
  - We plan to introduce a better experience focused on the needs of value objects in domain-driven design.
  - This approach will be based on value converters rather than owned entities which have proved not to be a good fit.
- Cosmos database provider
  - We are actively gathering feedback on which improvements to make to the Cosmos provider in EF Core 6.0; please make sure to vote (👍) for the Cosmos features that you need.
- Expose model building conventions to applications
  - Model building conventions are currently controlled by the database provider. In EF Core 6.0, we intend to allow applications to hook into and change these conventions.
- Zero bug balance (ZBB)
  - We plan to fix all outstanding non-blocked bugs during the EF Core 6.0 timeframe.
- Miscellaneous smaller features
  - Split query for non-navigation collections
  - Detect simple join tables in reverse engineering and create many-to-many relationships
  - Complete full/free-text search on SQLite and SQL Server
  - SQL Server Spatial Indexes
  - Mechanism/API to specify a default conversion for any property of a given type in the model
  - Use the new batching API from ADO.NET

### .NET integration

The EF Core team also works on several related but independent .NET Data technologies. In particular, we plan to work on:

- Enhancements to `System.Data`
  - Implementation of the new batching API
  - Continued work with other .NET teams and the community to understand and evolve ADO.NET
  - Standardize on `DiagnosticSource` for tracing in `System.Data` components
- Enhancements to `Microsoft.Data.Sqlite`
  - Connection pooling 
  - Prepared statements
- Nullable reference types
  - We will annotate the EF Core code to use [nullable reference types](https://docs.microsoft.com/dotnet/csharp/nullable-references).

### Experiments and investigations

The EF team is planning to invest time during the EF Core 6.0 timeframe experimenting and investigating in two areas. This is a learning process and as such no concrete deliverables are planned for the 6.0 release.

- SqlServer.Core
  - An experiment in collaboration with the community to determine what potential there is modern .NET features in a highly performant SQL Server driver.
- GraphQL
  - We plan to investigate the space and collaborate with the community to find ways to improve the experience of using [GraphQL](https://graphql.org/) with .NET.
https://docs.microsoft.com/ef/core/what-is-new/ef-core-6.0/plan
## Find out more

This post is a brief summary of the [full EF Core 6.0 Plan](https://docs.microsoft.com/ef/core/what-is-new/ef-core-6.0/plan). Please see the full plan for more information. 

## Suggestions

Your feedback on planning is important. The best way to indicate the importance of an issue is to vote (👍) for that [issue on GitHub](https://github.com/dotnet/efcore/issues). 
This data will then feed into the [planning process](https://docs.microsoft.com/ef/core/what-is-new/release-planning) for the next release.

In addition, please comment on this post if you believe we are missing something that is critical for EF Core 6.0, or are focusing on the wrong areas.
