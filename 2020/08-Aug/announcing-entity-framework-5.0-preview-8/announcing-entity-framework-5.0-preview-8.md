# Announcing Entity Framework Core 5.0 Preview 8

Today, the Entity Framework Core team announces the eighth and final preview release of 
[EF Core 5.0](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/5.0.0-preview.8.20407.4). The next release will be a release candidate (RC).
This release includes table-per-type (TPT) mapping, table-valued functions, SQLite table rebuilds for migrations and much more.

```dotnetcli
                     _/\__
               ---==/    \\
         ___  ___   |.    \|\
        | __|| __|  |  )   \\\
        | _| | _|   \_/ |  //|\\
        |___||_|       /   \\\/\\

  _______ _                 _                                    
 |__   __| |               | |                                   
    | |  | |__   __ _ _ __ | | __  _   _  ___  _   _             
    | |  | '_ \ / _` | '_ \| |/ / | | | |/ _ \| | | |            
    | |  | | | | (_| | | | |   <  | |_| | (_) | |_| |  _   _   _ 
    |_|  |_| |_|\__,_|_| |_|_|\_\  \__, |\___/ \__,_| (_) (_) (_)
                                    __/ |                        
                                   |___/                         
```

**The EF Core team gives a warm thanks to the nearly 100 community contributors to EF Core 5.0.**

## Prerequisites 

**EF Core 5.0 will _not_ run on .NET Standard 2.0 platforms, including .NET Framework.**

- The previews of EF Core 5.0 require [.NET Standard 2.1](https://github.com/dotnet/standard/blob/master/docs/versions/netstandard2.1.md).
- This means that EF Core 5.0 will run on .NET Core 3.1 and does _not_ require .NET 5. 

> To summarize: EF Core 5.0 runs on [platforms that support .NET Standard 2.1](https://docs.microsoft.com/dotnet/standard/net-standard#net-implementation-support).

**The product will maintain .NET Standard 2.1 compatibility through the final release.** 
 
---

## How to get EF Core 5.0 previews

EF Core is distributed exclusively as a set of NuGet packages.
For example, to add the SQL Server provider to your project, you can use the following command using the dotnet tool:

```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 5.0.0-preview.8.20407.4
```

This following table links to the preview 8 versions of the EF Core packages and describes what they are used for.

|**Package**    |**Purpose**      |
|--------------:|:----------------|
|[Microsoft.EntityFrameworkCore](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/5.0.0-preview.8.20407.4)|The main EF Core package that is independent of specific database providers|
|[Microsoft.EntityFrameworkCore.SqlServer](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer/5.0.0-preview.8.20407.4)|Database provider for Microsoft SQL Server and SQL Azure|
|[Microsoft.EntityFrameworkCore.SqlServer.NetTopologySuite](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer.NetTopologySuite/5.0.0-preview.8.20407.4)|SQL Server support for spatial types|
|[Microsoft.EntityFrameworkCore.Sqlite](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite/5.0.0-preview.8.20407.4)|Database provider for SQLite that includes the native binary for the database engine|
|[Microsoft.EntityFrameworkCore.Sqlite.Core](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite.Core/5.0.0-preview.8.20407.4)|Database provider for SQLite _without_ a packaged native binary|
|[Microsoft.EntityFrameworkCore.Sqlite.NetTopologySuite](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite.NetTopologySuite/5.0.0-preview.8.20407.4)|SQLite support for spatial types|
|[Microsoft.EntityFrameworkCore.Cosmos](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Cosmos/5.0.0-preview.8.20407.4)|Database provider for Azure Cosmos DB|
|[Microsoft.EntityFrameworkCore.InMemory](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.InMemory/5.0.0-preview.8.20407.4)|The in-memory database provider|
|[Microsoft.EntityFrameworkCore.Tools](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Tools/5.0.0-preview.8.20407.4)|EF Core PowerShell commands for the Visual Studio Package Manager Console; use this to integrate tools like [scaffolding](https://docs.microsoft.com/ef/core/managing-schemas/scaffolding) and [migrations](https://docs.microsoft.com/ef/core/managing-schemas/migrations/) with Visual Studio|
|[Microsoft.EntityFrameworkCore.Design](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Design/5.0.0-preview.8.20407.4)|Shared design-time components for EF Core tools|
|[Microsoft.EntityFrameworkCore.Proxies](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Proxies/5.0.0-preview.8.20407.4)|Lazy-loading and change-tracking proxies|
|[Microsoft.EntityFrameworkCore.Abstractions](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Abstractions/5.0.0-preview.8.20407.4)|Decoupled EF Core abstractions; use this for features like extended data annotations defined by EF Core|
|[Microsoft.EntityFrameworkCore.Relational](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Relational/5.0.0-preview.8.20407.4)|Shared EF Core components for relational database providers|
|[Microsoft.EntityFrameworkCore.Analyzers](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Analyzers/5.0.0-preview.8.20407.4)|C# analyzers for EF Core|

We also published the 5.0 preview 8 release of the [Microsoft.Data.Sqlite.Core](https://www.nuget.org/packages/Microsoft.Data.Sqlite.Core/5.0.0-preview.8.20407.4) provider for [ADO.NET](https://docs.microsoft.com/dotnet/framework/data/adonet/ado-net-overview).

## Installing the EF Core Command Line Interface (CLI)

As with EF Core 3.0 and 3.1, the EF Core CLI is no longer included in the .NET Core SDK. Before you can execute EF Core migration or scaffolding commands, 
you'll have to install this package as either a global or local tool.

![dotnet-ef](https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/08/efpreview8.jpg)

To install the preview tool globally, first uninstall any existing version with:

```bash
dotnet tool uninstall --global dotnet-ef
```

Then install with:

```bash
dotnet tool install --global dotnet-ef --version 5.0.0-preview.8.20407.4
```

It's possible to use this new version of the EF Core CLI with projects that use older versions of the EF Core runtime.

---

## What's New in EF Core 5 Preview 8

We maintain documentation covering [new features introduced into each preview](https://docs.microsoft.com/ef/core/what-is-new/ef-core-5.0/whatsnew).

Some of the highlights from preview 8 are called out below. This preview also includes several bug fixes.

### Table-per-type (TPT) mapping

By default, EF Core maps an inheritance hierarchy of .NET types to a single database table. This is known as table-per-hierarchy (TPH) mapping. EF Core 5.0 also allows mapping each .NET type in an inheritance hierarchy to a different database table; known as table-per-type (TPT) mapping.

For example, consider this model with a mapped hierarchy:

```c#
public class Animal
{
    public int Id { get; set; }
    public string Species { get; set; }
}

public class Pet : Animal
{
    public string Name { get; set; }
}

public class Cat : Pet
{
    public string EdcuationLevel { get; set; }
}

public class Dog : Pet
{
    public string FavoriteToy { get; set; }
}
```

By default, EF Core will map this to a single table:

```sql
CREATE TABLE [Animals] (
    [Id] int NOT NULL IDENTITY,
    [Species] nvarchar(max) NULL,
    [Discriminator] nvarchar(max) NOT NULL,
    [Name] nvarchar(max) NULL,
    [EdcuationLevel] nvarchar(max) NULL,
    [FavoriteToy] nvarchar(max) NULL,
    CONSTRAINT [PK_Animals] PRIMARY KEY ([Id])
);
```

However, mapping each entity type to a different table will instead result in one table per type:

```sql
CREATE TABLE [Animals] (
    [Id] int NOT NULL IDENTITY,
    [Species] nvarchar(max) NULL,
    CONSTRAINT [PK_Animals] PRIMARY KEY ([Id])
);

CREATE TABLE [Pets] (
    [Id] int NOT NULL,
    [Name] nvarchar(max) NULL,
    CONSTRAINT [PK_Pets] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Pets_Animals_Id] FOREIGN KEY ([Id]) REFERENCES [Animals] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [Cats] (
    [Id] int NOT NULL,
    [EdcuationLevel] nvarchar(max) NULL,
    CONSTRAINT [PK_Cats] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Cats_Animals_Id] FOREIGN KEY ([Id]) REFERENCES [Animals] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Cats_Pets_Id] FOREIGN KEY ([Id]) REFERENCES [Pets] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [Dogs] (
    [Id] int NOT NULL,
    [FavoriteToy] nvarchar(max) NULL,
    CONSTRAINT [PK_Dogs] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Dogs_Animals_Id] FOREIGN KEY ([Id]) REFERENCES [Animals] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Dogs_Pets_Id] FOREIGN KEY ([Id]) REFERENCES [Pets] ([Id]) ON DELETE NO ACTION
);
```

Note that creation of the foreign key constraints shown above were added after branching the code for preview 8.

Entity types can be mapped to different tables using mapping attributes:

```c#
[Table("Animals")]
public class Animal
{
    public int Id { get; set; }
    public string Species { get; set; }
}

[Table("Pets")]
public class Pet : Animal
{
    public string Name { get; set; }
}

[Table("Cats")]
public class Cat : Pet
{
    public string EdcuationLevel { get; set; }
}

[Table("Dogs")]
public class Dog : Pet
{
    public string FavoriteToy { get; set; }
}
```

Or using `ModelBuilder` configuration:

```c#
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Animal>().ToTable("Animals");
    modelBuilder.Entity<Pet>().ToTable("Pets");
    modelBuilder.Entity<Cat>().ToTable("Cats");
    modelBuilder.Entity<Dog>().ToTable("Dogs");
}
```

Documentation is tracked by issue [#1979](https://github.com/dotnet/EntityFramework.Docs/issues/1979).

### Migrations: Rebuild SQLite tables

Compared to other database, SQLite is relatively limited in its schema manipulation capabilities. For example, dropping a column from an existing table requires that the entire table be dropped and re-created. EF Core 5.0 Migrations now supports automatic rebuilding the table for schema changes that require it.

For example, imagine we have a `Unicorns` table created for a `Unicorn` entity type:

```c#
public class Unicorn
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
}
```

```sql
CREATE TABLE "Unicorns" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Unicorns" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NULL,
    "Age" INTEGER NOT NULL
);
```

We then learn that storing the age of a unicorn is considered very rude, so let's remove that property, add a new migration, and update the database. This update will fail when using EF Core 3.1 because the column cannot be dropped. In EF Core 5.0, Migrations will instead rebuild the table:

```sql
CREATE TABLE "ef_temp_Unicorns" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Unicorns" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NULL
);

INSERT INTO "ef_temp_Unicorns" ("Id", "Name")
SELECT "Id", "Name"
FROM Unicorns;

PRAGMA foreign_keys = 0;

DROP TABLE "Unicorns";

ALTER TABLE "ef_temp_Unicorns" RENAME TO "Unicorns";

PRAGMA foreign_keys = 1;
```

Notice that:
* A temporary table is created with the desired schema for the new table
* Data is copied from the current table into the temporary table
* Foreign key enforcement is switched off
* The current table is dropped
* The temporary table is renamed to be the new table

Documentation is tracked by issue [#2583](https://github.com/dotnet/EntityFramework.Docs/issues/2583).

### Table-valued functions

This feature was contributed from the community by [@pmiddleton](https://github.com/pmiddleton). Many thanks for the contribution!

EF Core 5.0 includes first-class support for mapping .NET methods to table-valued functions (TVFs). These functions can then be used in LINQ queries where additional composition on the results of the function will also be translated to SQL.

For example, consider this TVF defined in a SQL Server database:

```sql
create FUNCTION GetReports(@employeeId int)
RETURNS @reports TABLE
(
	Name nvarchar(50) not null,
	IsDeveloper bit not null
)
AS
begin
	WITH cteEmployees AS
	(
		SELECT id, name, managerId, isDeveloper
		FROM employees
		WHERE id = @employeeId
		UNION ALL
		SELECT e.id, e.name, e.managerId, e.isDeveloper
		FROM employees e
		INNER JOIN cteEmployees cteEmp ON cteEmp.id = e.ManagerId
	)

	insert into @reports
	select name, isDeveloper
	FROM cteEmployees
	where id != @employeeId

	return
end
```

The EF Core model requires two entity types to use this TVF:
* An `Employee` type that maps to the Employees table in the normal way
* A `Report` type that matches the shape returned by the TVF

```c#
public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsDeveloper { get; set; }

    public int? ManagerId { get; set; }
    public virtual Employee Manager { get; set; }
}
```

```c#
public class Report
{
    public string Name { get; set; }
    public bool IsDeveloper { get; set; }
}
```

These types must be included in the EF Core model:

```c#
modelBuilder.Entity<Employee>();
modelBuilder.Entity(typeof(Report)).HasNoKey();
```

Notice that `Report` has no primary key and so must be configured as such.

Finally, a .NET method must be mapped to the TVF in the database. This method can be defined on the DbContext using the new `FromExpression` method:

```c#
public IQueryable<Report> GetReports(int managerId)
    => FromExpression(() => GetReports(managerId));
```

This method uses a parameter and return type that match the TVF defined above. The method is then added to the EF Core model in OnModelCreating:

```c#
modelBuilder.HasDbFunction(() => GetReports(0));
```

(Using a lambda here is an easy way to pass the `MethodInfo` to EF Core. The arguments passed to the method are ignored.)

We can now write queries that call `GetReports` and compose over the results. For example:

```c#
from e in context.Employees
from rc in context.GetReports(e.Id)
where rc.IsDeveloper == true
select new
{
  ManagerName = e.Name,
  EmployeeName = rc.Name,
})
```

On SQL Server, this translates to:

```sql
SELECT [e].[Name] AS [ManagerName], [g].[Name] AS [EmployeeName]
FROM [Employees] AS [e]
CROSS APPLY [dbo].[GetReports]([e].[Id]) AS [g]
WHERE [g].[IsDeveloper] = CAST(1 AS bit)
```

Notice that the SQL is rooted in the `Employees` table, calls `GetReports`, and then adds an additional WHERE clause on the results of the function.

### Flexible query/update mapping

EF Core 5.0 allows mapping the same entity type to different database objects. These objects may be tables, views, or functions.

For example, an entity type can be mapped to both a database view and a database table:

```c#
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder
        .Entity<Blog>()
        .ToTable("Blogs")
        .ToView("BlogsView");
}
```

By default, EF Core will then query from the view and send updates to the table. For example, executing the following code:

```c#
var blog = context.Set<Blog>().Single(e => e.Name == "One Unicorn");

blog.Name = "1unicorn2";

context.SaveChanges();
```

Results in a query against the view, and then an update to the table:

```sql
SELECT TOP(2) [b].[Id], [b].[Name], [b].[Url]
FROM [BlogsView] AS [b]
WHERE [b].[Name] = N'One Unicorn'

SET NOCOUNT ON;
UPDATE [Blogs] SET [Name] = @p0
WHERE [Id] = @p1;
SELECT @@ROWCOUNT;
```

### Context-wide split-query configuration

Split queries (see below) can now be configured as the default for any query executed by the DbContext. This configuration is only available for relational providers, and so must be specified as part of the `UseProvider` configuration. For example:

```c#
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder
        .UseSqlServer(
            Your.SqlServerConnectionString,
            b => b.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
```

Documentation is tracked by issue [#2407](https://github.com/dotnet/EntityFramework.Docs/issues/2407).

### PhysicalAddress mapping

This feature was contributed from the community by [@ralmsdeveloper](https://github.com/ralmsdeveloper). Many thanks for the contribution!

The standard .NET [PhysicalAddress class](/dotnet/api/system.net.networkinformation.physicaladdress) is now automatically mapped to a string column for databases that do not already have native support. For more information, see the examples for `IPAddress` below.

---

## Daily builds

EF Core previews are aligned with .NET 5 previews. These previews tend to lag behind the latest work on EF Core. Consider using the [daily builds](https://github.com/aspnet/AspNetCore/blob/master/docs/DailyBuilds.md) instead to get the most up-to-date EF Core features and bug fixes.

As with the previews, the daily builds do not require .NET 5; they can be used with GA/RTM release of .NET Core 3.1.

---

## Contribute to .NET 5

The .NET documentation team is [reorganizing .NET content](https://github.com/dotnet/docs/issues/18923) to better match the workloads you build with .NET. This includes a new [.NET Data landing page](https://github.com/dotnet/docs/issues/19029) that will link out to data-related topics ranging from EF Core to APIs, Big Data, and Machine learning. The planning and execution will be done completely in the open on GitHub. This is your opportunity to help shape the hierarchy and content to best fit your needs as a .NET developer. We look forward to your contributions! 

## The EF Core Community Standup

The EF Core team is now live streaming every other Wednesday at 10am Pacific Time, 1pm Eastern Time, or 17:00 UTC. Join the stream to ask questions about the EF Core topic of your choice, including the latest preview release. 

* [Visit the .NET Community Standup](https://dotnet.microsoft.com/platform/community/standup) page to preview upcoming shows and view recordings from past shows
* [Suggest a guest or project, including your own](https://github.com/dotnet/efcore/discussions/21371) by posting to the linked discussion
* You can also [request an EF Core demo](https://github.com/dotnet/efcore/discussions/21192)

## Documentation and Feedback

The starting point for all EF Core documentation is [docs.microsoft.com/ef/](https://docs.microsoft.com/ef/).

Please file issues found and any other feedback on the [dotnet/efcore GitHub repo](https://github.com/dotnet/efcore).

## Helpful Short Links

The following short links are provided for easy reference and access.

Main documentation:
https://aka.ms/efdocs

Issues and feature requests for EF Core:
https://aka.ms/efcorefeedback

Entity Framework Roadmap:
https://aka.ms/efroadmap

What's new in EF Core 5.x?
https://aka.ms/efcore5

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
<td><a href="https://github.com/lajones"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/efteam_lajones.jpeg" alt="lajones" width=200px><br>lajones</a></td>
<td><a href="https://github.com/maumar"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/efteam_maumar.jpeg" alt="maumar" width=200px><br>Maurycy Markowski</a></td>
<td><a href="https://github.com/roji"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/efteam_roji-1-300x300.png" alt="roji" width=200px><br>Shay Rojansky</a></td>
<td><a href="https://github.com/smitpatel"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/efteam_smitpatel.png" alt="smitpatel" width=200px><br>Smit Patel</a>
</td>
</tr>

</table>

---

## Thank you to our contributors!

A big thank you to the following community members who have already contributed code or documentation to the EF Core 5 release! (List is in chronological order of first contribution to EF Core 5).

<table>
<tr>
<td><a href="https://github.com/aevitas"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_aevitas.jpeg" alt="aevitas" width=200px><br>aevitas</a></td>
<td><a href="https://github.com/alaatm"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_alaatm.png" alt="alaatm" width=200px><br>Alaa Masoud</a></td>
<td><a href="https://github.com/aleksandar-manukov"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_aleksandar-manukov.png" alt="aleksandar-manukov" width=200px><br>Aleksandar Manukov</a></td>
<td><a href="https://github.com/amrbadawy"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_amrbadawy.jpeg" alt="amrbadawy" width=200px><br>Amr Badawy</a>
</td>
</tr>

<tr>
<td><a href="https://github.com/AnthonyMonterrosa"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_AnthonyMonterrosa.jpeg" alt="AnthonyMonterrosa" width=200px><br>Anthony Monterrosa</a></td>
<td><a href="https://github.com/bbrandt"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_bbrandt.jpeg" alt="bbrandt" width=200px><br>Ben Brandt</a></td>
<td><a href="https://github.com/benmccallum"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_benmccallum.jpeg" alt="benmccallum" width=200px><br>Ben McCallum</a></td>
<td><a href="https://github.com/ccjx"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_ccjx.png" alt="ccjx" width=200px><br>Clarence Cai</a>
</td>
</tr>

<tr>
<td><a href="https://github.com/CGijbels"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_CGijbels.jpeg" alt="CGijbels" width=200px><br>Christophe Gijbels</a></td>
<td><a href="https://github.com/cincuranet"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_cincuranet.png" alt="cincuranet" width=200px><br>Jiri Cincura</a></td>
<td><a href="https://github.com/Costo"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_Costo.jpeg" alt="Costo" width=200px><br>Vincent Costel</a></td>
<td><a href="https://github.com/dshuvaev"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_dshuvaev.jpeg" alt="dshuvaev" width=200px><br>Dmitry Shuvaev</a>
</td>
</tr>

<tr>
<td><a href="https://github.com/EricStG"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_EricStG.jpeg" alt="EricStG" width=200px><br>Eric St-Georges</a></td>
<td><a href="https://github.com/ErikEJ"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_ErikEJ.jpeg" alt="ErikEJ" width=200px><br>Erik Ejlskov Jensen</a></td>
<td><a href="https://github.com/gravbox"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_gravbox.png" alt="gravbox" width=200px><br>Christopher Davis</a></td>
<td><a href="https://github.com/ivaylokenov"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_ivaylokenov.jpeg" alt="ivaylokenov" width=200px><br>Ivaylo Kenov</a>
</td>
</tr>

<tr>
<td><a href="https://github.com/jfoshee"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_jfoshee.png" alt="jfoshee" width=200px><br>Jacob Foshee</a></td>
<td><a href="https://github.com/jmzagorski"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_jmzagorski.png" alt="jmzagorski" width=200px><br>Jeremy Zagorski</a></td>
<td><a href="https://github.com/jviau"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_jviau.jpeg" alt="jviau" width=200px><br>Jacob Viau</a></td>
<td><a href="https://github.com/knom"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_knom.png" alt="knom" width=200px><br>Max K.</a>
</td>
</tr>

<tr>
<td><a href="https://github.com/lohoris-crane"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_lohoris-crane.jpeg" alt="lohoris-crane" width=200px><br>lohoris-crane</a></td>
<td><a href="https://github.com/loic-sharma"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_loic-sharma.jpeg" alt="loic-sharma" width=200px><br>Loïc Sharma</a></td>
<td><a href="https://github.com/lokalmatador"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_lokalmatador.jpeg" alt="lokalmatador" width=200px><br>lokalmatador</a></td>
<td><a href="https://github.com/mariusGundersen"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_mariusGundersen.png" alt="mariusGundersen" width=200px><br>Marius Gundersen</a>
</td>
</tr>

<tr>
<td><a href="https://github.com/Marusyk"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_Marusyk.jpeg" alt="Marusyk" width=200px><br>Roman Marusyk</a></td>
<td><a href="https://github.com/matthiaslischka"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_matthiaslischka.jpeg" alt="matthiaslischka" width=200px><br>Matthias Lischka</a></td>
<td><a href="https://github.com/MaxG117"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_MaxG117.png" alt="MaxG117" width=200px><br>MaxG117</a></td>
<td><a href="https://github.com/MHDuke"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_MHDuke.jpeg" alt="MHDuke" width=200px><br>MHDuke</a>
</td>
</tr>

<tr>
<td><a href="https://github.com/mikes-gh"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_mikes-gh.png" alt="mikes-gh" width=200px><br>Mike Surcouf</a></td>
<td><a href="https://github.com/Muppets"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_Muppets.jpeg" alt="Muppets" width=200px><br>Neil Bostrom</a></td>
<td><a href="https://github.com/nmichels"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_nmichels.jpeg" alt="nmichels" width=200px><br>Nícolas Michels</a></td>
<td><a href="https://github.com/OOberoi"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_OOberoi.jpeg" alt="OOberoi" width=200px><br>Obi Oberoi</a></td>
</td>
</tr>

<tr>
<td><a href="https://github.com/orionstudt"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_orionstudt.jpeg" alt="orionstudt" width=200px><br>Josh Studt</a></td>
<td><a href="https://github.com/ozantopal"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_ozantopal.jpeg" alt="ozantopal" width=200px><br>Ozan Topal</a></td>
<td><a href="https://github.com/pmiddleton"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_pmiddleton.jpeg" alt="pmiddleton" width=200px><br>Paul Middleton</a></td>
<td><a href="https://github.com/prog-rajkamal"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_prog-rajkamal.jpeg" alt="prog-rajkamal" width=200px><br>Raj</a>
</td>
</tr>

<tr>
<td><a href="https://github.com/ptjhuang"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_ptjhuang.png" alt="ptjhuang" width=200px><br>Peter Huang</a></td>
<td><a href="https://github.com/ralmsdeveloper"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_ralmsdeveloper.png" alt="ralmsdeveloper" width=200px><br>Rafael Almeida Santos</a></td>
<td><a href="https://github.com/redoz"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_redoz.png" alt="redoz" width=200px><br>Patrik Husfloen</a></td>
<td><a href="https://github.com/rmarskell"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_rmarskell.jpeg" alt="rmarskell" width=200px><br>Richard Marskell</a>
</td>
</tr>

<tr>
<td><a href="https://github.com/sguitardude"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_sguitardude.jpeg" alt="sguitardude" width=200px><br>sguitardude</a></td>
<td><a href="https://github.com/SimpleSamples"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_SimpleSamples.png" alt="SimpleSamples" width=200px><br>Sam Hobbs</a></td>
<td><a href="https://github.com/svengeance"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_svengeance.png" alt="svengeance" width=200px><br>Sven</a></td>
<td><a href="https://github.com/VladDragnea"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_VladDragnea.jpeg" alt="VladDragnea" width=200px><br>Vlad</a>
</td>
</tr>

<tr>
<td><a href="https://github.com/vslee"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_vslee.png" alt="vslee" width=200px><br>vslee</a></td>
<td><a href="https://github.com/WeihanLi"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_WeihanLi.jpeg" alt="WeihanLi" width=200px><br>liweihan</a></td>
<td><a href="https://github.com/Youssef1313"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_Youssef1313.jpeg" alt="Youssef1313" width=200px><br>Youssef Victor</a></td>
<td><a href="https://github.com/1iveowl"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/1iveowl.jpg" alt="1iveowl" width=200px><br>1iveowl</a>
</td>
</tr>

<tr>
<td><a href="https://github.com/thomaslevesque"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_thomaslevesque.png" alt="thomaslevesque" width=200px><br>Thomas Levesque</a></td>
<td><a href="https://github.com/akovac35"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_akovac35.png" alt="akovac35" width=200px><br>Aleksander Kovač</a></td>
<td><a href="https://github.com/leotsarev"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_leotsarev.jpg" alt="leotsarev" width=200px><br>Leonid Tsarev</a></td>
<td><a href="https://github.com/kostat"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_kostat.jpg" alt="kostat" width=200px><br>Konstantin Triger</a></td>
</tr>

<tr>
<td><a href="https://github.com/sungam3r"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/04/contributor_sungam3r.png" alt="sungam3r" width=200px><br>Ivan Maximov</a></td>
<td><a href="https://github.com/dzmitry-lahoda"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/04/contributor_dzmitry-lahoda.jpg" alt="dzmitry-lahoda" width=200px><br>Dzmitry Lahoda</a></td>
<td><a href="https://github.com/Logerfo"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/04/contributor_logerfo.jpg" alt="Logerfo" width=200px><br>Bruno Logerfo</a></td>
<td><a href="https://github.com/witheej"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/05/contributor_witheej.jpg" alt="witheej" width=200px><br>Josh Withee</td>
</tr>

<tr>
<td><a href="https://github.com/FransBouma"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/05/contributor_FransBouma.png" alt="FransBouma" width=200px><br>Frans Bouma</td>
<td><a href="https://github.com/IGx89"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/05/contributor_IGx89.png" alt="IGx89" width=200px><br>Matthew Lieder</td>
<td><a href="https://github.com/paulomorgado"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/05/contributor_paulomorgado.jpg" alt="paulomorgado" width=200px><br>Paulo Morgado</td>
<td><a href="https://github.com/mderriey"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/05/contributor_mderriey.jpg" alt="mderriey" width=200px><br>Mickaël Derriey</td>
</tr>

<tr>
<td><a href="https://github.com/LaurenceJKing"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/05/contributor_LaurenceJKing.jpg" alt="LaurenceJKing" width=200px><br>Laurence King</td>
<td><a href="https://github.com/oskarj"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/05/contributor_oskarj.png" alt="oskarj" width=200px><br>Oskar Josefsson</td>
<td><a href="https://github.com/bdebaere"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/05/contributor_bdebaere.jpg" alt="bdebaere" width=200px><br>bdebaere</a></td>
<td><a href="https://github.com/BhargaviAnnadevara-MSFT"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/05/contributor_BhargaviAnnadevara-MSFT.jpg" alt="BhargaviAnnadevara-MSFT" width=200px><br>Bhargavi Annadevara</a></td>
</tr>

<tr>
 <td><a href="https://github.com/AlexanderTaeschner"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/05/contributor_AlexanderTaeschner.png" alt="AlexanderTaeschner" width=200px><br>Alexander Täschner</a></td>
 <td><a href="https://github.com/Jesse-Hufstetler"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/05/contributor_Jesse-Hufstetler.png" alt="Jesse-Hufstetler" width=200px><br>Jesse Hufstetler</a></td>
 <td><a href="https://github.com/ivarlovlie"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/06/contributor_ivarlovlie.png" alt="ivarlovlie" width=200px><br>Ivar Løvlie</a></td>
 <td><a href="https://github.com/cucoreanu"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/06/contributor_cucoreanu.jpg" alt="cucoreanu" width=200px><br>cucoreanu</a></td>
 </tr>

<tr>
<td><a href="https://github.com/serpent5"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/06/contributor_serpent5.png" alt="serpent5" width=200px><br>Kirk Larkin</a></td>
 <td><a href="https://github.com/sdanyliv"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/06/contributor_sdanyliv.jpg" alt="sdanyliv" width=200px><br>Svyatoslav Danyliv</a></td>
 <td><a href="https://github.com/twenzel"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/06/contributor_twenzel.jpg" alt="twenzel" width=200px><br>Toni Wenzel</a></td>
 <td><a href="https://github.com/manvydasu"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/06/contributor_manvydasu.png" alt="manvydasu" width=200px><br>manvydasu</a></td> 
</tr>

<tr> 
<td><a href="https://github.com/brandongregoryscott"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/05/contributor_AlexanderTaeschner.png" alt="brandongregoryscott" width=200px><br>Brandon Scott</a></td>
 <td><a href="https://github.com/uncheckederror"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/06/contributor_uncheckederror.jpg" alt="uncheckederror" width=200px><br>Thomas Ryan</a></td>
 <td><a href="https://github.com/rocke97"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/06/contributor_rocke97.jpg" alt="rocke97" width=200px><br>Aaron Gunther</a></td>
 <td><a href="https://github.com/jonlouie"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/07/contributor_jonlouie.jpg" alt="jonlouie" width=200px><br>Jon Louie</a></td>
</tr>

<tr>
 <td><a href="https://github.com/mohsinnasir"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/07/contributor_mohsinnasir.jpg" alt="mohsinnasir" width=200px><br>Mohsin Nasir</a></td>
 <td><a href="https://github.com/seekingtheoptimal"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/07/contributor_seekingtheoptimal.jpg" alt="seekingtheoptimal" width=200px><br>Bálint Szabó</a></td>
 <td><a href="https://github.com/MartinBP"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/07/contributor_MartinBP.png" alt="MartinBP" width=200px><br>Martin Boye Petersen</a></td>
 <td><a href="https://github.com/Ropouser"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/07/contributor_ropouser.png" alt="Ropouser" width=200px><br>Duje Đaković</a>< /td>
</tr>

<tr>
 <td><a href="https://github.com/codemillmatt"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/07/contributor_codemillmatt.jpg" alt="codemillmatt" width=200px><br>Matt Soucoup</a></td>
 <td><a href="https://github.com/shahabganji"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/07/contributor_shahabganji.jpg" alt="shahabganji" width=200px><br>Saeed Ganji</a></td>
 <td><a href="https://github.com/AshkanAbd"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/07/contributor_AshkanAbd.png" alt="AshkanAbd" width=200px><br>Ashkan Abd</a></td>
 <td><a href="https://github.com/ChristopherHaws"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/08/contributor_christopherhaws.png" alt="ChristopherHaws" width=200px><br>Christopher Haws</a></td>
</tr>

<tr>
 <td><a href="https://github.com/SergerGood"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/08/contributor_SergerGood.jpg" alt="SergerGood" width=200px><br>Sergei Khlebnikov</a></td>
 <td><a href="https://github.com/KaloyanIT"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/08/contributor_KaloyanIT.jpg" alt="KaloyanIT" width=200px><br>Kaloyan Kostov</a></td>
 <td><a href="https://github.com/bide45"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/08/contributor_bide45.png" alt="bide45" width=200px><br>bide45</a></td>
 <td><a href="https://github.com/jsportaro"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/08/contributor_jsportaro.png" alt="jsportaro" width=200px><br>Joseph Portaro</td>
</tr>

<tr>
 <td><a href="https://github.com/mikewodarczyk"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/08/contributor_mikewodarczyk.png" alt="mikewodarczyk" width=200px><br>Mike Wodarczyk</a></td>
 <td><a href="https://github.com/jeffsvajlenko"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/08/contributor_jeffsvajlenko.jpg" alt="jeffsvajlenko" width=200px><br>Jeffrey Svajlenko</a></td>
 <td><a href="https://github.com/vanillajonathan"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/08/contributor_vanillajonathan.png" alt="vanillajonathan" width=200px><br>Jonathan</a></td>
 <td><a href="https://github.com/m4ss1m0g"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/08/contributor_m4ss1m0g.png" alt="m4ss1m0g" width=200px><br>Massimo Giambona</a></td>
</tr>

<tr>
 <td><a href="https://github.com/Psypher9"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/08/contributor_Psypher9.png" alt="Psypher9" width=200px><br>Turner Bass</a></td>
 <td>&nbsp;</td>
 <td>&nbsp;</td>
 <td>&nbsp;</td>
</tr>

</table>
