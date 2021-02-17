---
post_title: 'Announcing Entity Framework Core 6.0 Preview 1'
username: jeremy-likness
microsoft_alias: jeliknes
desired_publication_date: 2/19/2021
categories: .NET Core, Entity Framework, ASP.NET 
summary: Announcing the release of EF Core 6.0 Preview 1, the first preview of the new Entity Framework Core.
---

Today, the Entity Framework Core team announces the first preview release of 
[EF Core 6.0](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/6.0.0-preview.1.21102.2). 
This release includes new attributes, built-in functions, and database-specific improvements to SQLite and SQL Server capabilities.

## Prerequisites 

- EF Core 6.0 currently targets .NET 5. This will likely be updated to .NET 6 as we near the release. EF Core 6.0 does not target any .NET Standard version; for more information see [the future of .NET Standard](https://devblogs.microsoft.com/dotnet/the-future-of-net-standard/).

- EF Core 6.0 will not run on .NET Framework.

---

## How to get EF Core 6.0 previews

EF Core is distributed exclusively as a set of NuGet packages.
For example, to add the SQL Server provider to your project, you can use the following command using the dotnet tool:

```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 6.0.0-preview.1.21102.2
```

This following table links to the preview 1 versions of the EF Core packages and describes what they are used for.

|**Package**    |**Purpose**      |
|--------------:|:----------------|
|[Microsoft.EntityFrameworkCore](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/6.0.0-preview.1.21102.2)|The main EF Core package that is independent of specific database providers|
|[Microsoft.EntityFrameworkCore.SqlServer](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer/6.0.0-preview.1.21102.2)|Database provider for Microsoft SQL Server and SQL Azure|
|[Microsoft.EntityFrameworkCore.SqlServer.NetTopologySuite](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer.NetTopologySuite/6.0.0-preview.1.21102.2)|SQL Server support for spatial types|
|[Microsoft.EntityFrameworkCore.Sqlite](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite/6.0.0-preview.1.21102.2)|Database provider for SQLite that includes the native binary for the database engine|
|[Microsoft.EntityFrameworkCore.Sqlite.Core](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite.Core/6.0.0-preview.1.21102.2)|Database provider for SQLite _without_ a packaged native binary|
|[Microsoft.EntityFrameworkCore.Sqlite.NetTopologySuite](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite.NetTopologySuite/6.0.0-preview.1.21102.2)|SQLite support for spatial types|
|[Microsoft.EntityFrameworkCore.Cosmos](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Cosmos/6.0.0-preview.1.21102.2)|Database provider for Azure Cosmos DB|
|[Microsoft.EntityFrameworkCore.InMemory](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.InMemory/6.0.0-preview.1.21102.2)|The in-memory database provider|
|[Microsoft.EntityFrameworkCore.Tools](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Tools/6.0.0-preview.1.21102.2)|EF Core PowerShell commands for the Visual Studio Package Manager Console; use this to integrate tools like [scaffolding](https://docs.microsoft.com/ef/core/managing-schemas/scaffolding) and [migrations](https://docs.microsoft.com/ef/core/managing-schemas/migrations/) with Visual Studio|
|[Microsoft.EntityFrameworkCore.Design](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Design/6.0.0-preview.1.21102.2)|Shared design-time components for EF Core tools|
|[Microsoft.EntityFrameworkCore.Proxies](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Proxies/6.0.0-preview.1.21102.2)|Lazy-loading and change-tracking proxies|
|[Microsoft.EntityFrameworkCore.Abstractions](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Abstractions/6.0.0-preview.1.21102.2)|Decoupled EF Core abstractions; use this for features like extended data annotations defined by EF Core|
|[Microsoft.EntityFrameworkCore.Relational](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Relational/6.0.0-preview.1.21102.2)|Shared EF Core components for relational database providers|
|[Microsoft.EntityFrameworkCore.Analyzers](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Analyzers/6.0.0-preview.1.21102.2)|C# analyzers for EF Core|

We also published the 6.0 preview 1 release of the [Microsoft.Data.Sqlite.Core](https://www.nuget.org/packages/Microsoft.Data.Sqlite.Core/6.0.0-preview.1.21102.2) provider for [ADO.NET](https://docs.microsoft.com/dotnet/framework/data/adonet/ado-net-overview).

## Installing the EF Core Command Line Interface (CLI)

As with EF Core 3.0 and 3.1, the EF Core CLI is no longer included in the .NET Core SDK. Before you can execute EF Core migration or scaffolding commands, 
you'll have to install this package as either a global or local tool.

To install the preview tool globally, first uninstall any existing version with:

```bash
dotnet tool uninstall --global dotnet-ef
```

Then install with:

```bash
dotnet tool install --global dotnet-ef --version 6.0.0-preview.1.21102.2
```

It's possible to use this new version of the EF Core CLI with projects that use older versions of the EF Core runtime.

---

## What's New in EF Core 6 Preview 1

We maintain documentation covering [new features introduced into each preview](https://docs.microsoft.com/ef/core/what-is-new/ef-core-6.0/whatsnew).

Some of the highlights from preview 1 are called out below. This preview also includes several bug fixes.

> [!TIP]
> You can run and debug into all the preview 1 samples shown below by [downloading the sample code from GitHub](https://github.com/dotnet/EntityFramework.Docs/tree/master/samples/core/Miscellaneous/NewInEFCore6).

### UnicodeAttribute

GitHub Issue: [#19794](https://github.com/dotnet/efcore/issues/19794). This feature was contributed by [@RaymondHuy](https://github.com/RaymondHuy).

Starting with EF Core 6.0, a string property can now be mapped to a non-Unicode column using a mapping attribute _without specifying the database type directly_. For example, consider a `Book` entity type with a property for the [International Standard Book Number (ISBN)](https://en.wikipedia.org/wiki/International_Standard_Book_Number) in the form "ISBN 978-3-16-148410-0":

```csharp
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }

        [Unicode(false)]
        [MaxLength(22)]
        public string Isbn { get; set; }
    }
```

Since ISBNs cannot contain any non-unicode characters, the `Unicode` attribute will cause a non-Unicode string type to be used. In addition, `MaxLength` is used to limit the size of the database column. For example, when using SQL Server, this results in a database column of `varchar(22)`:

```sql
CREATE TABLE [Book] (
    [Id] int NOT NULL IDENTITY,
    [Title] nvarchar(max) NULL,
    [Isbn] varchar(22) NULL,
    CONSTRAINT [PK_Book] PRIMARY KEY ([Id]));
```

> [!NOTE]
> EF Core maps string properties to Unicode columns by default. `UnicodeAttribute` is ignored when the database system supports only Unicode types.

### PrecisionAttribute

GitHub Issue: [#17914](https://github.com/dotnet/efcore/issues/17914). This feature was contributed by [@RaymondHuy](https://github.com/RaymondHuy).

The precision and scale of a database column can now be configured using mapping attributes _without specifying the database type directly_. For example, consider a `Product` entity type with a decimal `Price` property:

```csharp
    public class Product
    {
        public int Id { get; set; }

        [Precision(precision: 10, scale: 2)]
        public decimal Price { get; set; }
    }
```

EF Core will map this property to a database column with precision 10 and scale 2. For example, on SQL Server:

```sql
CREATE TABLE [Product] (
    [Id] int NOT NULL IDENTITY,
    [Price] decimal(10,2) NOT NULL,
    CONSTRAINT [PK_Product] PRIMARY KEY ([Id]));
```

### EntityTypeConfigurationAttribute

GitHub Issue: [#23163](https://github.com/dotnet/efcore/issues/23163). This feature was contributed by [@KaloyanIT](https://github.com/KaloyanIT).

`IEntityTypeConfiguration` instances allow `ModelBuilder` configuration for a each entity type to be contained in its own configuration class. For example:

```csharp
public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder
            .Property(e => e.Isbn)
            .IsUnicode(false)
            .HasMaxLength(22);
    }
}
```

Normally, this configuration class must be instantiated and called into from `OnModelCreating`. For example:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    new BookConfiguration().Configure(modelBuilder.Entity<Book>());
}
```

Starting with EF Core 6.0, an `EntityTypeConfigurationAttribute` can be placed on the entity type such that EF Core can find and use appropriate configuration. For example:

```csharp
[EntityTypeConfiguration(typeof(BookConfiguration))]
public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Isbn { get; set; }
}
```

This attribute means that EF Core will use the specified `IEntityTypeConfiguration` implementation whenever the `Book` entity type is included in a model. The entity type is included in a model using one of the normal mechanisms. For example, by creating a `DbSet` property for the entity type:

```csharp
public class BooksContext : DbContext
{
    public DbSet<Book> Books { get; set; }

    //...
```

Or by registering it in `OnModelCreating`:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Book>();
}
```

> [!NOTE]
> `EntityTypeConfigurationAttribute` types will not be automatically discovered in an assembly. Entity types must be added to the model before the attribute will be discovered on that entity type.

### Translate ToString on SQLite

GitHub Issue: [#17223](https://github.com/dotnet/efcore/issues/17223). This feature was contributed by [@ralmsdeveloper](https://github.com/ralmsdeveloper).

Calls to `ToString` are now translated to SQL when using the SQLite database provider. This can be useful for text searches involving non-string columns. For example, consider a `User` entity type that stores phone numbers as numeric values:

```csharp
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public long PhoneNumber { get; set; }
    }
```

`ToString` can be used to convert the number to a string in the database. We can then use this string with a function such as `LIKE` to find numbers that match a pattern. For example, to find all numbers containing 555:

```csharp
var users = context.Users.Where(u => EF.Functions.Like(u.PhoneNumber.ToString(), "%555%")).ToList();
```

This translates to the following SQL when using a SQLite database:

```sql
SELECT COUNT(*)
FROM "Users" AS "u"
WHERE CAST("u"."PhoneNumber" AS TEXT) LIKE '%555%'
```

Note that translation of `ToString` for SQL Server is already supported in EF Core 5.0, and may also be supported by other database providers.

### EF.Functions.Random

GitHub Issue: [#16141](https://github.com/dotnet/efcore/issues/16141). This feature was contributed by [@RaymondHuy](https://github.com/RaymondHuy).

`EF.Functions.Random` maps to a database function returning a pseudo-random number between 0 and 1 exclusive. Translations have been implemented in the EF Core repo for SQL Server, SQLite, and Cosmos. For example, consider a `User` entity type with a `Popularity` property:

```csharp
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public int Popularity { get; set; }
    }
```

`Popularity` can have values from 1 to 5 inclusive. Using `EF.Functions.Random` we can write a query to return all users with a randomly chosen popularity:

```csharp
var users = context.Users.Where(u => u.Popularity == (int)(EF.Functions.Random() * 5.0) + 1).ToList();
```

This translates to the following SQL when using a SQL Server database:

```sql
SELECT [u].[Id], [u].[Popularity], [u].[Username]
FROM [Users] AS [u]
WHERE [u].[Popularity] = (CAST((RAND() * 5.0E0) AS int) + 1)
```

### Support for SQL Server sparse columns

GitHub Issue: [#8023](https://github.com/dotnet/efcore/issues/8023).

SQL Server [sparse columns](https://docs.microsoft.com/sql/relational-databases/tables/use-sparse-columns) are ordinary columns that are optimized to store null values. This can be useful when using [TPH inheritance mapping](https://docs.microsoft.com/ef/core/modeling/inheritance) where properties of a rarely used subtype will result in null column values for most rows in the table. For example, consider a `ForumModerator` class that extends from `ForumUser`:

```csharp
    public class ForumUser
    {
        public int Id { get; set; }
        public string Username { get; set; }
    }

    public class ForumModerator : ForumUser
    {
        public string ForumName { get; set; }
    }
```

There may be millions of users, with only a handful of these being moderators. This means mapping the `ForumName` as sparse might make sense here. This can now be configured using `IsSparse` in `OnModelCreating`. For example:

```csharp
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<ForumModerator>()
                .Property(e => e.ForumName)
                .IsSparse();
        }
```

EF Core migrations will then mark the column as sparse. For example:

```sql
CREATE TABLE [ForumUser] (
    [Id] int NOT NULL IDENTITY,
    [Username] nvarchar(max) NULL,
    [Discriminator] nvarchar(max) NOT NULL,
    [ForumName] nvarchar(max) SPARSE NULL,
    CONSTRAINT [PK_ForumUser] PRIMARY KEY ([Id]));
```

> [!NOTE]
> Sparse columns have limitations. Make sure to read the [SQL Server sparse columns documentation](https://docs.microsoft.com/sql/relational-databases/tables/use-sparse-columns) to ensure that sparse columns are the right choice for your scenario.

### In-memory database: validate required properties are not null

GitHub Issue: [#10613](https://github.com/dotnet/efcore/issues/10613). This feature was contributed by [@fagnercarvalho](https://github.com/fagnercarvalho).

The EF Core in-memory database will now throw an exception if an attempt is made to save a null value for a property marked as required. For example, consider a `User` type with a required `Username` property:

```csharp
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }
    }
```

Attempting to save an entity with a null `Username` will result in the following exception:

> Microsoft.EntityFrameworkCore.DbUpdateException: Required properties '{'Username'}' are missing for the instance of entity type 'User' with the key value '{Id: 1}'.

This validation can be disabled if necessary. For example:

```csharp
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .LogTo(Console.WriteLine, new[] { InMemoryEventId.ChangesSaved })
                .UseInMemoryDatabase("UserContextWithNullCheckingDisabled")
                .EnableNullabilityCheck(false);
        }
```

### Improved SQL Server translation for IsNullOrWhitespace

GitHub Issue: [#22916](https://github.com/dotnet/efcore/issues/22916). This feature was contributed by [@Marusyk](https://github.com/Marusyk).

Consider the following query:

```csharp
        var users = context.Users.Where(
            e => string.IsNullOrWhiteSpace(e.FirstName)
                 || string.IsNullOrWhiteSpace(e.LastName)).ToList();
```

Before EF Core 6.0, this was translated to the following on SQL Server:

```sql
SELECT [u].[Id], [u].[FirstName], [u].[LastName]
FROM [Users] AS [u]
WHERE ([u].[FirstName] IS NULL OR (LTRIM(RTRIM([u].[FirstName])) = N'')) OR ([u].[LastName] IS NULL OR (LTRIM(RTRIM([u].[LastName])) = N''))
```

This translation has been improved for EF Core 6.0 to:

```sql
SELECT [u].[Id], [u].[FirstName], [u].[LastName]
FROM [Users] AS [u]
WHERE ([u].[FirstName] IS NULL OR ([u].[FirstName] = N'')) OR ([u].[LastName] IS NULL OR ([u].[LastName] = N''))
```

### Database comments are scaffolded to code comments

GitHub Issue: [#19113](https://github.com/dotnet/efcore/issues/19113). This feature was contributed by [@ErikEJ](https://github.com/ErikEJ).

Comments on SQL tables and columns are now scaffolded into the entity types created when [reverse-engineering an EF Core model](https://docs.microsoft.com/ef/core/managing-schemas/scaffolding) from an existing SQL Server database. For example:

```csharp
/// <summary>
/// The Blog table.
/// </summary>
public partial class Blog
{
    /// <summary>
    /// The primary key.
    /// </summary>
    [Key]
    public int Id { get; set; }
}
```

## Microsoft.Data.Sqlite 6.0 Preview 1

> [!TIP]
> You can run and debug into all the preview 1 samples shown below by [downloading the sample code from GitHub](https://github.com/dotnet/EntityFramework.Docs/tree/master/samples/core/Miscellaneous/NewInEFCore6).

### Savepoints API

GitHub Issue: [#20228](https://github.com/dotnet/efcore/issues/20228).

We have been standardizing on [a common API for savepoints in ADO.NET providers](https://github.com/dotnet/runtime/issues/33397). Microsoft.Data.Sqlite now supports this API, including:

- `Save` to create a savepoint in the transaction
- `Rollback` to roll back to a previous savepoint
- `Release` to release a savepoint

Using a savepoint allows part of a transaction to be rolled back without rolling back the entire transaction. For example, the code below:

- Creates a transaction
- Sends an update to the database
- Creates a savepoint
- Sends another update to the database
- Rolls back to the savepoint previous created
- Commits the transaction

```csharp
        using var connection = new SqliteConnection("DataSource=test.db");
        connection.Open();

        using var transaction = connection.BeginTransaction();

        using (var command = connection.CreateCommand())
        {
            command.CommandText = @"UPDATE Users SET Username = 'ajcvickers' WHERE Id = 1";
            command.ExecuteNonQuery();
        }

        transaction.Save("MySavepoint");

        using (var command = connection.CreateCommand())
        {
            command.CommandText = @"UPDATE Users SET Username = 'wfvickers' WHERE Id = 2";
            command.ExecuteNonQuery();
        }

        transaction.Rollback("MySavepoint");

        transaction.Commit();
```

This will result in the first update being committed to the database, while the second update is not committed since the savepoint was rolled back before committing the transaction.

### Command timeout in the connection string

GitHub Issue: [#22505](https://github.com/dotnet/efcore/issues/22505). This feature was contributed by [@nmichels](https://github.com/nmichels).

ADO.NET providers support two distinct timeouts:

- The connection timeout, which determines the maximum time to wait when making a connection to the database.
- The command timeout, which determines the maximum time to wait for a command to complete executing.

The command timeout can be set from code using <xref:System.Data.Common.DbCommand.CommandTimeout?displayProperty=nameWithType>. Many providers are now also exposing this command timeout in the connection string. Microsoft.Data.Sqlite is following this trend with the `Command Timeout` connection string keyword. For example, `"Command Timeout=60;DataSource=test.db"` will use 60 seconds as the default timeout for commands created by the connection.

> [!TIP]
> Sqlite treats `Default Timeout` as a synonym for `Command Timeout` and so can be used instead if preferred.
---

## Daily builds

EF Core previews are aligned with .NET 6 previews. These previews tend to lag behind the latest work on EF Core. Consider using the [daily builds](https://github.com/aspnet/AspNetCore/blob/master/docs/DailyBuilds.md) instead to get the most up-to-date EF Core features and bug fixes.

As with the previews, the daily builds require .NET 5.

---

## The EF Core Community Standup

The EF Core team is now live streaming every other Wednesday at 10am Pacific Time, 1pm Eastern Time, or 17:00 UTC. Join the stream to ask questions about the EF Core topic of your choice, including the latest preview release. 

* [Watch our YouTube playlist](https://aka.ms/efstandups) of previous shows
* [Visit the .NET Community Standup](https://dotnet.microsoft.com/platform/community/standup) page to preview upcoming shows
* [Suggest a guest or project, including your own](https://github.com/dotnet/efcore/discussions/21371) by posting to the linked discussion
* You can also [request an EF Core demo](https://github.com/dotnet/efcore/discussions/21192)

## Documentation and Feedback

The starting point for all EF Core documentation is [docs.microsoft.com/ef/](https://docs.microsoft.com/ef/).

Please file issues found and any other feedback on the [dotnet/efcore GitHub repo](https://github.com/dotnet/efcore).

## Helpful Short Links

The following short links are provided for easy reference and access.

EF Core Community Standup Playlist:
https://aka.ms/efstandups

Main documentation:
https://aka.ms/efdocs

Issues and feature requests for EF Core:
https://aka.ms/efcorefeedback

Entity Framework Roadmap:
https://aka.ms/efroadmap

---

## Thank you from the team

A big thank you from the EF team to everyone who has used EF over the years!

<table>

<tr>
<td><a href="https://github.com/ajcvickers"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/efteam_ajcvickers.jpeg" alt="ajcvickers" width=200px><br>Arthur Vickers</a></td>
<td><a href="https://github.com/AndriySvyryd"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/efteam_AndriySvyryd.jpeg" alt="AndriySvyryd" width=200px><br>Andriy Svyryd</a></td>
<td><a href="https://github.com/bricelam"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/efteam_bricelam.jpeg" alt="" width=200px><br>Brice Lambson</a></td>
<td><a href="https://github.com/JeremyLikness"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/efteam_JeremyLikness.jpeg" alt="JeremyLikness" width=200px><br>Jeremy Likness</a>
</td>
</tr>

<tr>
<td><a href="https://github.com/maumar"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/efteam_maumar.jpeg" alt="maumar" width=200px><br>Maurycy Markowski</a></td>
<td><a href="https://github.com/roji"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/efteam_roji-1-300x300.png" alt="roji" width=200px><br>Shay Rojansky</a></td>
<td><a href="https://github.com/smitpatel"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/efteam_smitpatel.png" alt="smitpatel" width=200px><br>Smit Patel</a>
</td><td></td>
</tr>

</table>

---

## Thank you to our contributors!

| | | | |
|:--:|:--:|:--:|:--:|
|**[Ali-YousefiTelori](https://github.com/Ali-YousefiTelori)**|**[AndrewKitu](https://github.com/AndrewKitu)**|**[ardalis](https://github.com/ardalis)**|**[CaringDev](https://github.com/CaringDev)**|
|[![Ali-YousefiTelori](./Ali-YousefiTelori.jpg)](https://github.com/Ali-YousefiTelori)|[![AndrewKitu](./AndrewKitu.jpg)](https://github.com/AndrewKitu)|[![ardalis](./ardalis.jpg)](https://github.com/ardalis)|[![CaringDev](./CaringDev.png)](https://github.com/CaringDev)|
|[#1](https://github.com/dotnet/efcore/pull/23946)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3070)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3091)|[#1](https://github.com/dotnet/efcore/pull/23585)|
| | | |
**[carlreinke](https://github.com/carlreinke)**|**[cgrimes01](https://github.com/cgrimes01)**|**[cincuranet](https://github.com/cincuranet)**|**[dan-giddins](https://github.com/dan-giddins)**|
[![carlreinke](./carlreinke.png)](https://github.com/carlreinke)|[![cgrimes01](./cgrimes01.png)](https://github.com/cgrimes01)|[![cincuranet](./cincuranet.png)](https://github.com/cincuranet)|[![dan-giddins](./dan-giddins.png)](https://github.com/dan-giddins)|
[#1](https://github.com/dotnet/efcore/pull/23694)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3038)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2714)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2910)|
| | | |
**[dennisseders](https://github.com/dennisseders)**|**[DickBaker](https://github.com/DickBaker)**|**[ErikEJ](https://github.com/ErikEJ)**|**[fagnercarvalho](https://github.com/fagnercarvalho)**|
[![dennisseders](./dennisseders.png)](https://github.com/dennisseders)|[![DickBaker](./DickBaker.png)](https://github.com/DickBaker)|[![ErikEJ](./ErikEJ.jpg)](https://github.com/ErikEJ)|[![fagnercarvalho](./fagnercarvalho.jpg)](https://github.com/fagnercarvalho)|
[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2839), [#2](https://github.com/dotnet/EntityFramework.Docs/pull/2845), [#3](https://github.com/dotnet/EntityFramework.Docs/pull/2848), [#4](https://github.com/dotnet/EntityFramework.Docs/pull/2987), [#5](https://github.com/dotnet/EntityFramework.Docs/pull/2997), [#6](https://github.com/dotnet/EntityFramework.Docs/pull/3007)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2990)|[#1](https://github.com/dotnet/efcore/pull/22900), [#2](https://github.com/dotnet/efcore/pull/22937), [#3](https://github.com/dotnet/efcore/pull/22938), [#4](https://github.com/dotnet/EntityFramework.Docs/pull/2897), [#5](https://github.com/dotnet/EntityFramework.Docs/pull/2984)|[#1](https://github.com/dotnet/efcore/pull/23094)|
| | | |
**[filipnavara](https://github.com/filipnavara)**|**[garyng](https://github.com/garyng)**|**[Geoff1900](https://github.com/Geoff1900)**|**[gfoidl](https://github.com/gfoidl)**|
[![filipnavara](./filipnavara.jpg)](https://github.com/filipnavara)|[![garyng](./garyng.jpg)](https://github.com/garyng)|[![Geoff1900](./Geoff1900.png)](https://github.com/Geoff1900)|[![gfoidl](./gfoidl.jpg)](https://github.com/gfoidl)|
[#1](https://github.com/dotnet/efcore/pull/23591)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3045), [#2](https://github.com/dotnet/EntityFramework.Docs/pull/3046), [#3](https://github.com/dotnet/EntityFramework.Docs/pull/3047)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3025)|[#1](https://github.com/dotnet/efcore/pull/22923)|
| | | |
**[gurustron](https://github.com/gurustron)**|**[HSchwichtenberg](https://github.com/HSchwichtenberg)**|**[jantlee](https://github.com/jantlee)**|**[joaopgrassi](https://github.com/joaopgrassi)**|
[![gurustron](./gurustron.png)](https://github.com/gurustron)|[![HSchwichtenberg](./HSchwichtenberg.jpg)](https://github.com/HSchwichtenberg)|[![jantlee](./jantlee.jpg)](https://github.com/jantlee)|[![joaopgrassi](./joaopgrassi.jpg)](https://github.com/joaopgrassi)|
[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3010)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2894)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2786)|[#1](https://github.com/dotnet/efcore/pull/22849)|
| | | |
**[josemiltonsampaio](https://github.com/josemiltonsampaio)**|**[KaloyanIT](https://github.com/KaloyanIT)**|**[khalidabuhakmeh](https://github.com/khalidabuhakmeh)**|**[khellang](https://github.com/khellang)**|
[![josemiltonsampaio](./josemiltonsampaio.jpg)](https://github.com/josemiltonsampaio)|[![KaloyanIT](./KaloyanIT.jpg)](https://github.com/KaloyanIT)|[![khalidabuhakmeh](./khalidabuhakmeh.jpg)](https://github.com/khalidabuhakmeh)|[![khellang](./khellang.png)](https://github.com/khellang)|
[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2927)|[#1](https://github.com/dotnet/efcore/pull/23563), [#2](https://github.com/dotnet/efcore/pull/23666)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2858), [#2](https://github.com/dotnet/EntityFramework.Docs/pull/2962)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2982)|
| | | |
**[koenbeuk](https://github.com/koenbeuk)**|**[kotpal](https://github.com/kotpal)**|**[leonardoporro](https://github.com/leonardoporro)**|**[Marusyk](https://github.com/Marusyk)**|
[![koenbeuk](./koenbeuk.png)](https://github.com/koenbeuk)|[![kotpal](./kotpal.jpg)](https://github.com/kotpal)|[![leonardoporro](./leonardoporro.png)](https://github.com/leonardoporro)|[![Marusyk](./Marusyk.jpg)](https://github.com/Marusyk)|
[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2921)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2763)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2883)|[#1](https://github.com/dotnet/efcore/pull/23039), [#2](https://github.com/dotnet/efcore/pull/24016)|
| | | |
**[MaxG117](https://github.com/MaxG117)**|**[mefateah](https://github.com/mefateah)**|**[meggima](https://github.com/meggima)**|**[msawczyn](https://github.com/msawczyn)**|
[![MaxG117](./MaxG117.png)](https://github.com/MaxG117)|[![mefateah](./mefateah.png)](https://github.com/mefateah)|[![meggima](./meggima.jpg)](https://github.com/meggima)|[![msawczyn](./msawczyn.jpg)](https://github.com/msawczyn)|
[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2898)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3065)|[#1](https://github.com/dotnet/efcore/pull/23605)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2917)|
| | | |
**[MSDN-WhiteKnight](https://github.com/MSDN-WhiteKnight)**|**[natashanikolic](https://github.com/natashanikolic)**|**[nmichels](https://github.com/nmichels)**|**[nschonni](https://github.com/nschonni)**|
[![MSDN-WhiteKnight](./MSDN-WhiteKnight.png)](https://github.com/MSDN-WhiteKnight)|[![natashanikolic](./natashanikolic.png)](https://github.com/natashanikolic)|[![nmichels](./nmichels.jpg)](https://github.com/nmichels)|[![nschonni](./nschonni.jpg)](https://github.com/nschonni)|
[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2887)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2920)|[#1](https://github.com/dotnet/efcore/pull/23091)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2775), [#2](https://github.com/dotnet/EntityFramework.Docs/pull/2776), [#3](https://github.com/dotnet/EntityFramework.Docs/pull/2779), [#4](https://github.com/dotnet/EntityFramework.Docs/pull/2780)|
| | | |
**[pkellner](https://github.com/pkellner)**|**[ralmsdeveloper](https://github.com/ralmsdeveloper)**|**[RaymondHuy](https://github.com/RaymondHuy)**|**[Shirasho](https://github.com/Shirasho)**|
[![pkellner](./pkellner.jpg)](https://github.com/pkellner)|[![ralmsdeveloper](./ralmsdeveloper.jpg)](https://github.com/ralmsdeveloper)|[![RaymondHuy](./RaymondHuy.jpg)](https://github.com/RaymondHuy)|[![Shirasho](./Shirasho.jpg)](https://github.com/Shirasho)|
[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2954)|[#1](https://github.com/dotnet/efcore/pull/19473)|[#1](https://github.com/dotnet/efcore/pull/22514), [#2](https://github.com/dotnet/efcore/pull/23145), [#3](https://github.com/dotnet/efcore/pull/23232), [#4](https://github.com/dotnet/efcore/pull/23424)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2988)|
| | | |
**[SimonCropp](https://github.com/SimonCropp)**|**[the-wazz](https://github.com/the-wazz)**|**[tkp1n](https://github.com/tkp1n)**|**[Tomkaa](https://github.com/Tomkaa)**|
[![SimonCropp](./SimonCropp.jpg)](https://github.com/SimonCropp)|[![the-wazz](./the-wazz.png)](https://github.com/the-wazz)|[![tkp1n](./tkp1n.jpg)](https://github.com/tkp1n)|[![Tomkaa](./Tomkaa.png)](https://github.com/Tomkaa)|
[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2957), [#2](https://github.com/dotnet/EntityFramework.Docs/pull/2959)|[#1](https://github.com/dotnet/efcore/pull/23551)|[#1](https://github.com/dotnet/efcore/pull/23014)|[#1](https://github.com/dotnet/efcore/pull/23933)|
| | | |
**[umitkavala](https://github.com/umitkavala)**|**[vincent1405](https://github.com/vincent1405)**|**[wmeints](https://github.com/wmeints)**| |
[![umitkavala](./umitkavala.jpg)](https://github.com/umitkavala)|[![vincent1405](./vincent1405.png)](https://github.com/vincent1405)|[![wmeints](./wmeints.jpg)](https://github.com/wmeints)| |
[#1](https://github.com/dotnet/efcore/pull/23322), [#2](https://github.com/dotnet/efcore/pull/23562)|[#1](https://github.com/dotnet/efcore/pull/24020)|[#1](https://github.com/dotnet/efcore/pull/23873)| |
