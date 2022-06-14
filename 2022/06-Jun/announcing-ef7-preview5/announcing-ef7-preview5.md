---
post_title: 'Announcing Entity Framework 7 Preview 5'
author1: jeremy-likness
author2: avickers
post_slug: announcing-ef7-preview5
microsoft_alias: jeliknes
featured_image: ef7preview5.png
categories: .NET, .NET Core, Entity Framework
summary: Announcing EF7 Preview 5 with completed support for Table-per-Concrete Type (TPC).
desired_publication_date: 2022-06-14
---

Entity Framework 7 (EF7) Preview 5 has shipped with support for Table-per-Concrete type (TPC) mapping. This blog post will focus on TPC. There are several other enhancements included in Preview 5, such as:

- [Support for AT TIME ZONE in SQL Server](https://github.com/dotnet/efcore/issues/26199)
- Updates to command and connection interception ([#23087](https://github.com/dotnet/efcore/issues/23087), [#23085](https://github.com/dotnet/efcore/issues/23085), [#17261](https://github.com/dotnet/efcore/issues/17261))
- Addition of the [delete behavior attribute](https://github.com/dotnet/efcore/issues/9621)

Read the [full list of EF7 Preview 5 enhancements](https://github.com/dotnet/efcore/issues?q=is%3Aissue+milestone%3A7.0.0-preview5+is%3Aclosed+label%3Atype-enhancement).

## Table-per-concrete-type (TPC) mapping

By default, EF Core maps an inheritance hierarchy of .NET types to a single database table. This is known as the table-per-hierarchy (TPH) mapping strategy. EF Core 5.0 introduced the table-per-type (TPT) strategy, which supports mapping each .NET type to a different database table. In EF Core 7.0 preview 5.0, we are excited to introduce the table-per-concrete-type (TPC) strategy. TPC also maps .NET types to different tables, but in a way that addresses some common performance issues with the TPT strategy.

In this post, we'll start by describing the structure of TPH, TPT, and TPC mappings, then look at how these strategies can be configured in EF Core, and finally discuss the pros and cons of each approach.

### Mapping inheritance hierarchies

Consider the following object-oriented domain model:

```csharp
public abstract class Animal
{
    public int Id { get; set; }
    public string Species { get; set; }
}

public class FarmAnimal : Animal
{
    public decimal Value { get; set; }
}

public class Pet : Animal
{
    public string Name { get; set; }
}

public class Cat : Pet
{
    public string EducationLevel { get; set; }
}

public class Dog : Pet
{
    public string FavoriteToy { get; set; }
}
```

If we are to retrieve some `Animal` object from the database, then we must know which type of animal it is. We don't want to save a cat and then read it back as a dog, or vice versa. (I can tell you from experience that dogs generally don't like to be treated as cats, and cats _certainly_ don't like to be treated as dogs!) So this means the type of animal--that is the actual class used when the animal was created in C#--must be saved to the database in some form.

Further, different information is associated with each `Animal` object depending on its type. For example, in our model, a farm animal has some monetary value but no name, while pets are priceless and named.

Inheritance mapping strategies (TPH, TPT, or TPC) define how this object-oriented type information and type-specific information are saved into a relational database, where inheritance is not a natural concept.

#### The TPH strategy

With the TPH strategy, a single table is created for all types in the hierarchy--hence the name "table-per-hierarchy". This table contains a special column containing a "discriminator value", which indicates the type of the object saved in each row. In addition, a column is created for every property of every type in the hierarchy. For example:

```sql
CREATE TABLE [Animals] (
    [Id] int NOT NULL IDENTITY,
    [Species] nvarchar(max) NOT NULL,
    [Discriminator] nvarchar(max) NOT NULL,
    [Value] decimal(18,2) NULL,
    [Name] nvarchar(max) NULL,
    [EducationLevel] nvarchar(max) NULL,
    [FavoriteToy] nvarchar(max) NULL,
    CONSTRAINT [PK_Animals] PRIMARY KEY ([Id])
);
```

Saving two cats, a dog, and a sheep to this table results in the following:

| Id | Species | Discriminator | Value | Name | EducationLevel | FavoriteToy |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- |
| 1 | Felis catus | Cat | NULL | Alice | MBA | NULL |
| 2 | Felis catus | Cat | NULL | Mac | BA | NULL |
| 3 | Canis familiaris | Dog | NULL | Toast | NULL | Mr. Squirrel |
| 4 | Ovis aries | FarmAnimal | 100.00 | NULL | NULL | NULL |

Notice that:

- The value in the `Discriminator` column indicates the type of C# object saved
- There is a column for every property in the hierarchy
- If the property does not exist for the type of the object saved, then the value in the database for that column is null

> [!NOTE]
> The TPH strategy requires that database columns be nullable for any property not defined in the root type of the hierarchy, even if that property is required. It is possible to create a database constraint for these columns to ensure the value is non-null whenever an instance with that property is saved, but this is not done automatically by EF Core. See [Issue #20931 on the EF Core GitHub repo](https://github.com/dotnet/efcore/issues/20931) for more information.

#### The TPT strategy

With the TPT strategy, a different table is created for every type in the hierarchy--hence the name "table-per-type". The table itself is used to determine the type of the object saved, and each table contains only columns for the properties of that type. For example:

```sql
CREATE TABLE [Animals] (
    [Id] int NOT NULL IDENTITY,
    [Species] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Animals] PRIMARY KEY ([Id])
);

CREATE TABLE [FarmAnimals] (
    [Id] int NOT NULL,
    [Value] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_FarmAnimals] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_FarmAnimals_Animals_Id] FOREIGN KEY ([Id]) REFERENCES [Animals] ([Id])
);

CREATE TABLE [Pets] (
    [Id] int NOT NULL,
    [Name] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Pets] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Pets_Animals_Id] FOREIGN KEY ([Id]) REFERENCES [Animals] ([Id])
);

CREATE TABLE [Cats] (
    [Id] int NOT NULL,
    [EducationLevel] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Cats] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Cats_Pets_Id] FOREIGN KEY ([Id]) REFERENCES [Pets] ([Id])
);

CREATE TABLE [Dogs] (
    [Id] int NOT NULL,
    [FavoriteToy] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Dogs] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Dogs_Pets_Id] FOREIGN KEY ([Id]) REFERENCES [Pets] ([Id])
);
```

Saving the same data into this database results in the following:

Animals table:

| Id | Species |
| :--- | :--- |
| 1 | Felis catus |
| 2 | Felis catus |
| 3 | Canis familiaris |
| 4 | Ovis aries |

FarmAnimals table:

| Id | Value |
| :--- | :--- |
| 4 | 100.00 |

Pets table:

| Id | Name |
| :--- | :--- |
| 1 | Alice |
| 2 | Mac |
| 3 | Toast |

Cats table:

| Id | EducationLevel |
| :--- | :--- |
| 1 | MBA |
| 2 | BA |

Dogs table:

| Id | FavoriteToy |
| :--- | :--- |
| 3 | Mr. Squirrel |

Notice that the data is saved in a normalized form, but that this means the information for a single object is spread across multiple tables.

#### TPC mapping

The TPC strategy is similar to the TPT strategy except that a different table is created for every concrete type in the hierarchy, but tables are not created for abstract types--hence the name "table-per-concrete-type". As with TPT, the table itself indicates the type of the object saved. However, unlike TPT mapping, each table contains columns for every property in the concrete type _and its base types_. For example:

```sql
CREATE TABLE [FarmAnimals] (
    [Id] int NOT NULL DEFAULT (NEXT VALUE FOR [AnimalIds]),
    [Species] nvarchar(max) NOT NULL,
    [Value] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_FarmAnimals] PRIMARY KEY ([Id])
);

CREATE TABLE [Pets] (
    [Id] int NOT NULL DEFAULT (NEXT VALUE FOR [AnimalIds]),
    [Species] nvarchar(max) NOT NULL,
    [Name] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Pets] PRIMARY KEY ([Id])
);

CREATE TABLE [Cats] (
    [Id] int NOT NULL DEFAULT (NEXT VALUE FOR [AnimalIds]),
    [Species] nvarchar(max) NOT NULL,
    [Name] nvarchar(max) NOT NULL,
    [EducationLevel] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Cats] PRIMARY KEY ([Id])
);

CREATE TABLE [Dogs] (
    [Id] int NOT NULL DEFAULT (NEXT VALUE FOR [AnimalIds]),
    [Species] nvarchar(max) NOT NULL,
    [Name] nvarchar(max) NOT NULL,
    [FavoriteToy] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Dogs] PRIMARY KEY ([Id])
);
```

Notice that:

- There is no table for `Animal`, since it is an abstract type in the object model. Remember that C# does not allow instances of abstract types, and there is therefore no situation where one will be saved to the database.
- The mapping of properties in base types is repeated for each concrete type--for example, every table has a `Species` column, and both `Cats` and `Dogs` have a `Name` column.

Saving the same data into this database results in the following:

FarmAnimals table:

| Id | Species | Value |
| :--- | :--- | :--- |
| 4 | Ovis aries | 100.00 |

Pets table:

| Id | Species | Name |
| :--- | :--- | :--- |

Cats table:

| Id | Species | Name | EducationLevel |
| :--- | :--- | :--- | :--- |
| 1 | Felis catus | Alice | MBA |
| 2 | Felis catus | Mac | BA |

Dogs table:

| Id | Species | Name | FavoriteToy |
| :--- | :--- | :--- | :--- |
| 3 | Canis familiaris | Toast | Mr. Squirrel |

Notice that, unlike with TPT mapping, all the information for a single object is contained in a single table.

### Configuring inheritance mappings in EF Core

When mapping an inheritance hierarchy, all types in the hierarchy must be explicitly included in the model. This can be done by creating a `DbSet` property for the type on your `DbContext`:

```csharp
public DbSet<Animal> Animals { get; set; }
public DbSet<Pet> Pets { get; set; }
public DbSet<Cat> Cats { get; set; }
public DbSet<Dog> Dogs { get; set; }
public DbSet<FarmAnimal> FarmAnimals { get; set; }
```

Or by using the `Entity` method in `OnModelCreating`:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Animal>();
    modelBuilder.Entity<Pet>();
    modelBuilder.Entity<Cat>();
    modelBuilder.Entity<Dog>();
    modelBuilder.Entity<FarmAnimal>();
}
```

> [!NOTE]
> This is different from the legacy EF6 behavior, where derived types of mapped base types would sometimes be automatically discovered.

Nothing else needs to be done to map the hierarchy as TPH, since it is the default strategy. However, you can make this explicit by calling `UseTphMappingStrategy` on the base type of the hierarchy. For example:

```csharp
modelBuilder.Entity<Animal>().UseTphMappingStrategy();
```

To use TPT instead, change this to `UseTptMappingStrategy`. For example:

```csharp
modelBuilder.Entity<Animal>().UseTptMappingStrategy();
```

Likewise, `UseTpcMappingStrategy` is used to configure TPC:

```csharp
modelBuilder.Entity<Animal>().UseTpcMappingStrategy();
```

In each case, the table name to use for each type can be configured using the `ToTable` builder method, or the `[Table]` attribute. However, this is only valid on types that are mapped to a table for the strategy being used. For example, the following code specifies the table names for TPC mapping:

```csharp
modelBuilder.Entity<Pet>().ToTable("Pets");
modelBuilder.Entity<Cat>().ToTable("Cats");
modelBuilder.Entity<Dog>().ToTable("Dogs");
modelBuilder.Entity<FarmAnimal>().ToTable("FarmAnimals");
```

No table name can be specified for `Animal` because it is not mapped to its own table when using the TPC strategy. Conversely, when using the TPH strategy, only the base type (`Animal`) can be given a table name.

> [!NOTE]
> If multiple types in a hierarchy are given different table names, but no mapping strategy is explicitly specified, then the TPT strategy is used. This was the normal way to configure TPT prior to EF7.

### Primary keys

The inheritance mapping strategy chosen has consequences for how primary key values are generated and managed. Keys in TPH are easy, since each entity instance is represented by a single row in a single table. Any kind of key value generation can be used, and no additional constraints are needed.

For the TPT strategy, there is always a row in the table mapped to the base type of the hierarchy. Any kind of key generation can be used on this row. The keys for other tables are linked to this table using foreign key constraints. For example:

```sql
CREATE TABLE [FarmAnimals] (
    [Id] int NOT NULL,
    [Value] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_FarmAnimals] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_FarmAnimals_Animals_Id] FOREIGN KEY ([Id]) REFERENCES [Animals] ([Id])
);
```

This ensures that same primary key value is used for a given entity in every table of the hierarchy.

This gets a bit more complicated when the TPC strategy is used. First, it's important to understand that EF Core requires that all entities in a hierarchy must have a unique key value, even if the entities have different types. So, using our example model, a `Dog` cannot have the same `Id` key value as a `Cat`. Second, unlike TPT, there is no common table that can act as the single place where key values live and can be generated. This means a simple `Identity` column cannot be used.

For databases that support sequences, this key values can be generated by using a single sequence and referencing it in the default constraint for every table. This is the strategy used in the TPC tables shown above, where each table has the following:

```sql
[Id] int NOT NULL DEFAULT (NEXT VALUE FOR [AnimalIds])
```

`AnimalIds` is a sequence created by EF Core migrations. The following model building code sets this up for SQL Server:

```csharp
modelBuilder.HasSequence<int>("AnimalIds");

modelBuilder.Entity<Animal>()
    .UseTpcMappingStrategy()
    .Property(e => e.Id).HasDefaultValueSql("NEXT VALUE FOR [AnimalIds]");
```

The syntax for the default constraint may be different for over database systems.

### Pros and cons of mapping strategies

All of the above may be interesting, but how do you decide which strategy to use? 

#### TPH

In almost all cases, TPH mapping works just fine, which is why it is the default. People are often concerned that the table can become very wide, with many columns only sparsely populated. While this can be true, it is rarely a problem with modern database systems. Query performance with TPH is always very good, mainly because no matter what query you write, only one table is ever needed to return results.

The most important performance differences of the different strategies stem from the SQL needed for different types of common query. To illustrate this, we will run the same three LINQ queries with each of the strategies. These queries are:

1. A query that returns entities of all types in the hierarchy:

```csharp
context.Animals.Where(a => a.Species.StartsWith("F")).ToList();
```

2. A query that returns entities from a subset of types in the hierarchy:

```csharp
context.Pets.Where(a => a.Species.StartsWith("F")).ToList();
```

3. A query that returns only entities from a single leaf type in the hierarchy:

```csharp
context.Cats.Where(a => a.Species.StartsWith("F")).ToList();
```

When using TPH, the SQL generated is simple and efficient in all cases. To help improve the performance of these queries, consider defining an index for faster filtering on the discriminator. In some scenarios this can a source of query slowdown compared to a table scan. However, note that SQL Server avoids using an index if it isn't highly selective. For example, for 5 discriminator values over 1 million rows, SQL server prefers a table scan. Note that adding an index will also slows down updates, which may or may not be important.

Finally, if your database system supports it, then consider using sparse columns when the majority of rows will be null for that column.

1. All types:

```sql
SELECT [a].[Id], [a].[Discriminator], [a].[Species], [a].[Value], [a].[Name], [a].[EducationLevel], [a].[FavoriteToy]
FROM [Animals] AS [a]
WHERE [a].[Species] LIKE N'F%'
```

2. Subset of types:

```sql
SELECT [a].[Id], [a].[Discriminator], [a].[Species], [a].[Name], [a].[EducationLevel], [a].[FavoriteToy]
FROM [Animals] AS [a]
WHERE [a].[Discriminator] IN (N'Pet', N'Cat', N'Dog') AND ([a].[Species] LIKE N'F%')
```

3. Leaf type:

```sql
SELECT [a].[Id], [a].[Discriminator], [a].[Species], [a].[Name], [a].[EducationLevel]
FROM [Animals] AS [a]
WHERE [a].[Discriminator] = N'Cat' AND ([a].[Species] LIKE N'F%')
```

#### TPT

The TPT strategy is rarely a good choice. It is mainly used when it is considered important that the data is stored in a normalized form, which is in turn often the case for legacy existing databases or databases managed independently from the application development team.

The main issue with the TPT strategy is that almost all queries involve joining multiple tables because the data for any given entity instance is split across multiple tables.

Using the same queries again, we can see that querying for entities of all types requires all five tables to be joined: 

```sql
SELECT [a].[Id], [a].[Species], [f].[Value], [p].[Name], [c].[EducationLevel], [d].[FavoriteToy], CASE
    WHEN [d].[Id] IS NOT NULL THEN N'Dog'
    WHEN [c].[Id] IS NOT NULL THEN N'Cat'
    WHEN [p].[Id] IS NOT NULL THEN N'Pet'
    WHEN [f].[Id] IS NOT NULL THEN N'FarmAnimal'
END AS [Discriminator]
FROM [Animals] AS [a]
    LEFT JOIN [FarmAnimals] AS [f] ON [a].[Id] = [f].[Id]
    LEFT JOIN [Pets] AS [p] ON [a].[Id] = [p].[Id]
    LEFT JOIN [Cats] AS [c] ON [a].[Id] = [c].[Id]
    LEFT JOIN [Dogs] AS [d] ON [a].[Id] = [d].[Id]
WHERE [a].[Species] LIKE N'F%'
```

> [!NOTE]
> EF Core uses "discriminator synthesis" to determine which table the data comes from, and hence the correct type to use. This works because the LEFT JOIN returns nulls for the dependent ID column (the "sub-tables") which aren't the correct type. So for a dog, `[d].[Id]` will be non-null, and all the other (concrete) IDs will be null.

Querying for entities of a subset of types still requires that the base table be joined, resulting in four tables being used:

```sql
SELECT [a].[Id], [a].[Species], [p].[Name], [c].[EducationLevel], [d].[FavoriteToy], CASE
    WHEN [d].[Id] IS NOT NULL THEN N'Dog'
    WHEN [c].[Id] IS NOT NULL THEN N'Cat'
    WHEN [p].[Id] IS NOT NULL THEN N'Pet'
END AS [Discriminator]
FROM [Animals] AS [a]
    LEFT JOIN [Pets] AS [p] ON [a].[Id] = [p].[Id]
    LEFT JOIN [Cats] AS [c] ON [a].[Id] = [c].[Id]
    LEFT JOIN [Dogs] AS [d] ON [a].[Id] = [d].[Id]
WHERE ([d].[Id] IS NOT NULL OR [c].[Id] IS NOT NULL OR [p].[Id] IS NOT NULL) AND ([a].[Species] LIKE N'F%')
```

And even querying for entities of just a single leaf type requires the tables for all the types that the leaf type derives from:

```sql
SELECT [a].[Id], [a].[Species], [p].[Name], [c].[EducationLevel], CASE
    WHEN [c].[Id] IS NOT NULL THEN N'Cat'
END AS [Discriminator]
FROM [Animals] AS [a]
    LEFT JOIN [Pets] AS [p] ON [a].[Id] = [p].[Id]
    LEFT JOIN [Cats] AS [c] ON [a].[Id] = [c].[Id]
WHERE [c].[Id] IS NOT NULL AND ([a].[Species] LIKE N'F%')
```

#### TPC

The TPC strategy is an improvement over TPT because it ensures that the information for a given entity instance is always stored in a single table. This means the TPC strategy can be useful when the mapped hierarchy is large and has many concrete (usually leaf) types, each with a large number of properties, and where only a small subset of types are used in most queries.

Using the same LINQ queries again, the SQL needed when querying for entities of all types is better than it was for TPT, since it requires one fewer table in the query. This is because there is no table for the abstract base type. In addition, `UNION ALL` is used instead of the `LEFT JOIN` needed for TPT. `UNION ALL` does not need to perform any matching between rows or de-duplication of rows, which makes it more efficient that the joins used in TPT queries. 

All that being said, when compared to the SQL for TPH, the SQL for TPC in this case is still not great:

```sql
SELECT [t].[Id], [t].[Species], [t].[Value], [t].[Name], [t].[EducationLevel], [t].[FavoriteToy], [t].[Discriminator]
FROM (
    SELECT [f].[Id], [f].[Species], [f].[Value], NULL AS [Name], NULL AS [EducationLevel], NULL AS [FavoriteToy], N'FarmAnimal' AS [Discriminator]
    FROM [FarmAnimals] AS [f]
    UNION ALL
    SELECT [p].[Id], [p].[Species], NULL AS [Value], [p].[Name], NULL AS [EducationLevel], NULL AS [FavoriteToy], N'Pet' AS [Discriminator]
    FROM [Pets] AS [p]
    UNION ALL
    SELECT [c].[Id], [c].[Species], NULL AS [Value], [c].[Name], [c].[EducationLevel], NULL AS [FavoriteToy], N'Cat' AS [Discriminator]
    FROM [Cats] AS [c]
    UNION ALL
    SELECT [d].[Id], [d].[Species], NULL AS [Value], [d].[Name], NULL AS [EducationLevel], [d].[FavoriteToy], N'Dog' AS [Discriminator]
    FROM [Dogs] AS [d]
) AS [t]
WHERE [t].[Species] LIKE N'F%'
```

This is again the case when querying for entities of a subset of types:

```sql
SELECT [t].[Id], [t].[Species], [t].[Name], [t].[EducationLevel], [t].[FavoriteToy], [t].[Discriminator]
FROM (
    SELECT [p].[Id], [p].[Species], [p].[Name], NULL AS [EducationLevel], NULL AS [FavoriteToy], N'Pet' AS [Discriminator]
    FROM [Pets] AS [p]
    UNION ALL
    SELECT [c].[Id], [c].[Species], [c].[Name], [c].[EducationLevel], NULL AS [FavoriteToy], N'Cat' AS [Discriminator]
    FROM [Cats] AS [c]
    UNION ALL
    SELECT [d].[Id], [d].[Species], [d].[Name], NULL AS [EducationLevel], [d].[FavoriteToy], N'Dog' AS [Discriminator]
    FROM [Dogs] AS [d]
) AS [t]
WHERE [t].[Species] IS NOT NULL AND ([t].[Species] LIKE N'F%')
```

But TPC is _much better_ than TPT when querying for entities of a single leaf type, since all the information for those entities comes from a single table: 

```sql
SELECT [c].[Id], [c].[Species], [c].[Name], [c].[EducationLevel]
FROM [Cats] AS [c]
WHERE [c].[Species] LIKE N'F%'
```

These types of queries for single leaf types is where TPC really excels. 

### Guidance

In summary, the guidance for which mapping strategy to use is quite simple:

- If your code will mostly query for entities of a single leaf type, then use TPC. This is because: 
  - The storage requirements are smaller, since there are no null columns and no discriminator.
  - No index is ever needed on the discriminator column, which would slow down updates and possibly also queries. An index may not be needed when using TPH either, but that depends on various factors.
- If your code will mostly query for entities of many types, such as writing queries against the base type, then lean towards TPH.
  - If your database system supports it (e.g. SQL Server), then consider using sparse columns for columns that will be rarely populated.
- Use TPT only if constrained to do so by external factors.

## Prerequisites 

- EF7 currently targets .NET 6. 
- EF7 will not run on .NET Framework.

EF7 is the successor to EF Core 6.0, not to be confused with [EF6](https://github.com/dotnet/ef6). If you are considering upgrading from EF6, please read our guide to [port from EF6 to EF Core](https://docs.microsoft.com/ef/efcore-and-ef6/porting/).

## How to get EF7 previews

EF7 is distributed exclusively as a set of NuGet packages.
For example, to add the SQL Server provider to your project, you can use the following command using the dotnet tool:

```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 7.0.0-preview.5.22302.2
```

This following table links to the preview 5 versions of the EF Core packages and describes what they are used for.

|**Package**    |**Purpose**      |
|--------------:|:----------------|
|[Microsoft.EntityFrameworkCore](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/7.0.0-preview.5.22302.2)|The main EF Core package that is independent of specific database providers|
|[Microsoft.EntityFrameworkCore.SqlServer](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer/7.0.0-preview.5.22302.2)|Database provider for Microsoft SQL Server and SQL Azure|
|[Microsoft.EntityFrameworkCore.SqlServer.NetTopologySuite](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer.NetTopologySuite/7.0.0-preview.5.22302.2)|SQL Server support for spatial types|
|[Microsoft.EntityFrameworkCore.Sqlite](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite/7.0.0-preview.5.22302.2)|Database provider for SQLite that includes the native binary for the database engine|
|[Microsoft.EntityFrameworkCore.Sqlite.Core](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite.Core/7.0.0-preview.5.22302.2)|Database provider for SQLite _without_ a packaged native binary|
|[Microsoft.EntityFrameworkCore.Sqlite.NetTopologySuite](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite.NetTopologySuite/7.0.0-preview.5.22302.2)|SQLite support for spatial types|
|[Microsoft.EntityFrameworkCore.Cosmos](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Cosmos/7.0.0-preview.5.22302.2)|Database provider for Azure Cosmos DB|
|[Microsoft.EntityFrameworkCore.InMemory](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.InMemory/7.0.0-preview.5.22302.2)|The in-memory database provider|
|[Microsoft.EntityFrameworkCore.Tools](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Tools/7.0.0-preview.5.22302.2)|EF Core PowerShell commands for the Visual Studio Package Manager Console; use this to integrate tools like [scaffolding](https://docs.microsoft.com/ef/core/managing-schemas/scaffolding) and [migrations](https://docs.microsoft.com/ef/core/managing-schemas/migrations/) with Visual Studio|
|[Microsoft.EntityFrameworkCore.Design](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Design/7.0.0-preview.5.22302.2)|Shared design-time components for EF Core tools|
|[Microsoft.EntityFrameworkCore.Proxies](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Proxies/7.0.0-preview.5.22302.2)|Lazy-loading and change-tracking proxies|
|[Microsoft.EntityFrameworkCore.Abstractions](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Abstractions/7.0.0-preview.5.22302.2)|Decoupled EF Core abstractions; use this for features like extended data annotations defined by EF Core|
|[Microsoft.EntityFrameworkCore.Relational](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Relational/7.0.0-preview.5.22302.2)|Shared EF Core components for relational database providers|
|[Microsoft.EntityFrameworkCore.Analyzers](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Analyzers/7.0.0-preview.5.22302.2)|C# analyzers for EF Core|

We also published the 7.0 preview 1 release of the [Microsoft.Data.Sqlite.Core](https://www.nuget.org/packages/Microsoft.Data.Sqlite.Core/7.0.0-preview.5.22302.2) provider for [ADO.NET](https://docs.microsoft.com/dotnet/framework/data/adonet/ado-net-overview).

## Installing the EF7 Command Line Interface (CLI)

Before you can execute EF7 Core migration or scaffolding commands, you'll have to install the CLI package as either a global or local tool.

To install the preview tool globally, install with:

```bash
dotnet tool install --global dotnet-ef --version 7.0.0-preview.5.22302.2 
```

If you already have the tool installed, you can upgrade it with the following command:

```bash
dotnet tool update --global dotnet-ef --version 7.0.0-preview.5.22302.2 
```

It's possible to use this new version of the EF7 CLI with projects that use older versions of the EF Core runtime.

## Daily builds

EF7 previews are aligned with .NET 7 previews. These previews tend to lag behind the latest work on EF7. Consider using the [daily builds](https://github.com/aspnet/AspNetCore/blob/master/docs/DailyBuilds.md) instead to get the most up-to-date EF7 features and bug fixes.

As with the previews, the daily builds require .NET 6.

## The .NET Data Community Standup

The .NET data team is now live streaming every other Wednesday at 10am Pacific Time, 1pm Eastern Time, or 17:00 UTC. Join the stream to ask questions about the data-related topic of your choice, including the latest preview release. 

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
