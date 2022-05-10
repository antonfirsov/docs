---
post_title: 'Announcing Entity Framework 7 Preview 4'
username: jeremy-likness
microsoft_alias: jeliknes
desired_publication_date: 5/10/2022
featured_image: ef7preview4.jpg
categories: .NET, Entity Framework
summary: Announcing the release of EF7 Preview 4 and DDD-friendly value converters for value generators.
---

Today, the .NET data team announces the fourth preview release of 
[EF Core 7.0 (EF7)](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/7.0.0-preview.4.22229.2). 
In addition to bug fixes and foundation work for larger features, this preview includes updates to ensure that [converters and comparers are handled by type mapping](https://github.com/dotnet/efcore/issues/17962) 
and to support [using converters with value generators](https://github.com/dotnet/efcore/issues/11597). Be sure to read the [full plan for EF7](https://docs.microsoft.com/ef/core/what-is-new/ef-core-7.0/plan) to learn what's on the roadmap.

## Domain-driven design and "guarded" keys

One pattern from domain-driven design (DDD) is the concept of using value objects as keys. In other words, instead of declaring a key like this:

```csharp
public class Thing
{
  public int Id { get; set; }
}
```

It might look like this instead:

```csharp
public class Thing
{
  public ThingKey Id { get; set; }
}

public class ThingKey
{
  public ThingKey(Func<int> generator) => Id = generator();  
  public ThingKey(int id) => Id = id;
  public int Id { get; private set; }
}
```

There are several advantages to doing this. The first and perhaps most obvious is that it hides the implementation details of the key. If you decide that you'd prefer to
consume the limitless resource of GUIDs instead the more limited and boringly sequential set of available integers, your refactoring will be much easier thanks to the buffer the key class provides. 
You are also able to guard the key from bad data
with validations so someone doesn't accidentally assign a negative value. Speaking of accidents, have you ever passed the wrong id to a method that takes a key? The
compiler doesn't care because it's all ints ... or bytes ... or 1s and 0s ... but if you're using strongly-typed keys, you will be forced to provide the key that, *cough "Dad
Joke" *cough* fits. Otherwise, the compiler will complain.

Whether or not you use this approach is of course up to you, but before preview 4 the choice was limited in EF Core. The most obvious way to translate from an entity to
a scalar is to use a converter, but EF Core would throw an exception if you tried to assign a converter to a property flagged for value generation. That constraint
has been lifted and more importantly, the code updated to ensure this case is handled properly. Thanks to the 81 of you who upvoted this issue, it was prioritized
and is now ready for you to try out. Let us know if you're using this feature and how it works for you!

## Prerequisites 

- EF7 currently targets .NET 6. This will likely be updated to .NET 7 as we near the release. 
- EF7 will not run on .NET Framework.

EF7 is the successor to EF Core 6.0, not to be confused with [EF6](https://github.com/dotnet/ef6). If you are considering upgrading from EF6, please read our guide to [port from EF6 to EF Core](https://docs.microsoft.com/ef/efcore-and-ef6/porting/).

---

## How to get EF7 previews

EF7 is distributed exclusively as a set of NuGet packages.
For example, to add the SQL Server provider to your project, you can use the following command using the dotnet tool:

```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 7.0.0-preview.4.22229.2
```

This following table links to the preview 1 versions of the EF Core packages and describes what they are used for.

|**Package**    |**Purpose**      |
|--------------:|:----------------|
|[Microsoft.EntityFrameworkCore](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/7.0.0-preview.4.22229.2)|The main EF Core package that is independent of specific database providers|
|[Microsoft.EntityFrameworkCore.SqlServer](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer/7.0.0-preview.4.22229.2)|Database provider for Microsoft SQL Server and SQL Azure|
|[Microsoft.EntityFrameworkCore.SqlServer.NetTopologySuite](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer.NetTopologySuite/7.0.0-preview.4.22229.2)|SQL Server support for spatial types|
|[Microsoft.EntityFrameworkCore.Sqlite](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite/7.0.0-preview.4.22229.2)|Database provider for SQLite that includes the native binary for the database engine|
|[Microsoft.EntityFrameworkCore.Sqlite.Core](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite.Core/7.0.0-preview.4.22229.2)|Database provider for SQLite _without_ a packaged native binary|
|[Microsoft.EntityFrameworkCore.Sqlite.NetTopologySuite](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite.NetTopologySuite/7.0.0-preview.4.22229.2)|SQLite support for spatial types|
|[Microsoft.EntityFrameworkCore.Cosmos](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Cosmos/7.0.0-preview.4.22229.2)|Database provider for Azure Cosmos DB|
|[Microsoft.EntityFrameworkCore.InMemory](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.InMemory/7.0.0-preview.4.22229.2)|The in-memory database provider|
|[Microsoft.EntityFrameworkCore.Tools](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Tools/7.0.0-preview.4.22229.2)|EF Core PowerShell commands for the Visual Studio Package Manager Console; use this to integrate tools like [scaffolding](https://docs.microsoft.com/ef/core/managing-schemas/scaffolding) and [migrations](https://docs.microsoft.com/ef/core/managing-schemas/migrations/) with Visual Studio|
|[Microsoft.EntityFrameworkCore.Design](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Design/7.0.0-preview.4.22229.2)|Shared design-time components for EF Core tools|
|[Microsoft.EntityFrameworkCore.Proxies](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Proxies/7.0.0-preview.4.22229.2)|Lazy-loading and change-tracking proxies|
|[Microsoft.EntityFrameworkCore.Abstractions](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Abstractions/7.0.0-preview.4.22229.2)|Decoupled EF Core abstractions; use this for features like extended data annotations defined by EF Core|
|[Microsoft.EntityFrameworkCore.Relational](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Relational/7.0.0-preview.4.22229.2)|Shared EF Core components for relational database providers|
|[Microsoft.EntityFrameworkCore.Analyzers](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Analyzers/7.0.0-preview.4.22229.2)|C# analyzers for EF Core|

We also published the 7.0 preview 1 release of the [Microsoft.Data.Sqlite.Core](https://www.nuget.org/packages/Microsoft.Data.Sqlite.Core/7.0.0-preview.4.22229.2) provider for [ADO.NET](https://docs.microsoft.com/dotnet/framework/data/adonet/ado-net-overview).

## Installing the EF7 Command Line Interface (CLI)

Before you can execute EF7 Core migration or scaffolding commands, you'll have to install the CLI package as either a global or local tool.

To install the preview tool globally, install with:

```bash
dotnet tool install --global dotnet-ef --version 7.0.0-preview.4.22229.2 
```

If you already have the tool installed, you can upgrade it with the following command:

```bash
dotnet tool update --global dotnet-ef --version 7.0.0-preview.4.22229.2 
```

It's possible to use this new version of the EF7 CLI with projects that use older versions of the EF Core runtime.

## Daily builds

EF7 previews are aligned with .NET 7 previews. These previews tend to lag behind the latest work on EF7. Consider using the [daily builds](https://github.com/aspnet/AspNetCore/blob/master/docs/DailyBuilds.md) instead to get the most up-to-date EF7 features and bug fixes.

As with the previews, the daily builds require .NET 6.

## The .NET Data Community Standup

The .NET data team is now live streaming every other Wednesday at 10am Pacific Time, 1pm Eastern Time, or 17:00 UTC. Join the stream to ask questions about the data-related topic of your choice, including the latest preview release. 

- [Watch our YouTube playlist](https://aka.ms/efstandups) of previous shows
- [Visit the .NET Community Standup](https://dotnet.microsoft.com/platform/community/standup) page to preview upcoming shows
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
