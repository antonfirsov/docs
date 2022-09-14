---
post_title: 'Announcing Entity Framework 7 Release Candidate 1'
author1: jeremy-likness
post_slug: announcing-ef7-rc1
microsoft_alias: jeliknes
featured_image: EF7RC1.png
categories: .NET, .NET Core, Entity Framework
summary: Announcing EF7 Release Candidate 1
desired_publication_date: 2022-09-14
---

Entity Framework Core 7 (EF7) Release Candidate 1 has shipped! The team focused on addressing defects, minor enhancements, and putting the finishing touches on features.

See the [full list of EF7 RC1 changes](https://github.com/dotnet/efcore/issues?q=milestone%3A7.0.0-rc1) on GitHub.

For a detailed look at what's new in EF7, with working samples, check out our newly updated [What's New in EF7](https://docs.microsoft.com/ef/core/what-is-new/ef-core-7.0/whatsnew) documentation. You can also read the feature deep dives in our previous blog posts:

- [EF7 Preview 7 - Interceptors](https://devblogs.microsoft.com/dotnet/announcing-ef7-preview7-entity-framework/)
- [EF7 Preview 6 - Performance](https://devblogs.microsoft.com/dotnet/announcing-ef-core-7-preview6-performance-optimizations/)
- [EF7 Preview 5 - Table-per-Concrete Type (TPC)](https://devblogs.microsoft.com/dotnet/category/dotnet-core/)
- [EF7 Preview 4 - DDD-friendly converters](https://devblogs.microsoft.com/dotnet/announcing-entity-framework-7-preview-4/)
- [EF7 Preview 3 - customizable database-first scaffolding templates](https://devblogs.microsoft.com/dotnet/announcing-entity-framework-7-preview-3/)
- [EF7 Preview 1 - the beginning](https://devblogs.microsoft.com/dotnet/announcing-entity-framework-7-preview-1/)

## EF7 Prerequisites

- EF7 targets .NET 6, which means it can be used on .NET 6 (LTS) or .NET 7.
- EF7 will not run on .NET Framework.

EF7 is the successor to EF Core 6.0, not to be confused with [EF6](https://github.com/dotnet/ef6). If you are considering upgrading from EF6, please read our guide to [port from EF6 to EF Core](https://docs.microsoft.com/ef/efcore-and-ef6/porting/).

## How to get EF7 RC1

EF7 is distributed exclusively as a set of NuGet packages. For example, to add the SQL Server provider to your project, you can use the following command using the dotnet tool:

```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 7.0.0-rc.1.22426.7
```

This following table links to the RC1 versions of the EF Core packages and describes what they are used for.

|**Package**    |**Purpose**      |
|:--------------|:----------------|
|[Microsoft.EntityFrameworkCore](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/7.0.0-rc.1.22426.7)|The main EF Core package that is independent of specific database providers|
|[Microsoft.EntityFrameworkCore.SqlServer](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer/7.0.0-rc.1.22426.7)|Database provider for Microsoft SQL Server and SQL Azure|
|[Microsoft.EntityFrameworkCore.SqlServer.NetTopologySuite](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer.NetTopologySuite/7.0.0-rc.1.22426.7)|SQL Server support for spatial types|
|[Microsoft.EntityFrameworkCore.Sqlite](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite/7.0.0-rc.1.22426.7)|Database provider for SQLite that includes the native binary for the database engine|
|[Microsoft.EntityFrameworkCore.Sqlite.Core](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite.Core/7.0.0-rc.1.22426.7)|Database provider for SQLite _without_ a packaged native binary|
|[Microsoft.EntityFrameworkCore.Sqlite.NetTopologySuite](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite.NetTopologySuite/7.0.0-rc.1.22426.7)|SQLite support for spatial types|
|[Microsoft.EntityFrameworkCore.Cosmos](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Cosmos/7.0.0-rc.1.22426.7)|Database provider for Azure Cosmos DB|
|[Microsoft.EntityFrameworkCore.InMemory](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.InMemory/7.0.0-rc.1.22426.7)|The in-memory database provider|
|[Microsoft.EntityFrameworkCore.Tools](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Tools/7.0.0-rc.1.22426.7)|EF Core PowerShell commands for the Visual Studio Package Manager Console; use this to integrate tools like [scaffolding](https://docs.microsoft.com/ef/core/managing-schemas/scaffolding) and [migrations](https://docs.microsoft.com/ef/core/managing-schemas/migrations/) with Visual Studio|
|[Microsoft.EntityFrameworkCore.Design](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Design/7.0.0-rc.1.22426.7)|Shared design-time components for EF Core tools|
|[Microsoft.EntityFrameworkCore.Proxies](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Proxies/7.0.0-rc.1.22426.7)|Lazy-loading and change-tracking proxies|
|[Microsoft.EntityFrameworkCore.Abstractions](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Abstractions/7.0.0-rc.1.22426.7)|Decoupled EF Core abstractions; use this for features like extended data annotations defined by EF Core|
|[Microsoft.EntityFrameworkCore.Relational](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Relational/7.0.0-rc.1.22426.7)|Shared EF Core components for relational database providers|
|[Microsoft.EntityFrameworkCore.Analyzers](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Analyzers/7.0.0-rc.1.22426.7)|C# analyzers for EF Core|

We also published the release candidate 1 of the [Microsoft.Data.Sqlite.Core](https://www.nuget.org/packages/Microsoft.Data.Sqlite.Core/7.0.0-rc.1.22426.7) provider for [ADO.NET](https://docs.microsoft.com/dotnet/framework/data/adonet/ado-net-overview).

## Installing the EF7 Command Line Interface (CLI)

Before you can execute EF7 Core migration or scaffolding commands, you'll have to install the CLI package as either a global or local tool.

To install the RC tool globally, install with:

```bash
dotnet tool install --global dotnet-ef --version 7.0.0-rc.1.22426.7 
```

If you already have the tool installed, you can upgrade it with the following command:

```bash
dotnet tool update --global dotnet-ef --version 7.0.0-rc.1.22426.7 
```

It's possible to use this new version of the EF7 CLI with projects that use older versions of the EF Core runtime.

## Daily builds

EF7 release candidates are aligned with .NET 7 release candidates. These releases tend to lag behind the latest work on EF7. Consider using the [daily builds](https://github.com/aspnet/AspNetCore/blob/master/docs/DailyBuilds.md) instead to get the most up-to-date EF7 features and bug fixes.

As with the release candidates, the daily builds require .NET 6.

## The .NET Data Community Standup

The .NET data team is now live streaming every other Wednesday at 10am Pacific Time, 1pm Eastern Time, or 17:00 UTC. Join the stream to ask questions about the data-related topic of your choice, including the latest release candidate.

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
