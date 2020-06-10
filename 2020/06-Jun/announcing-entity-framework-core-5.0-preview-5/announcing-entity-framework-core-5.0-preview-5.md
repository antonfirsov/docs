# Announcing Entity Framework Core 5.0 Preview 5

Today we are excited to announce the fifth preview release of [EF Core 5.0](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/5.0.0-TBDVersion).

The fifth previews of [.NET 5](https://devblogs.microsoft.com/dotnet/announcing-net-5-preview-4-and-our-journey-to-one-net) and ASP.NET Core 5.0 are also available now.

**Join us for live Q&A** at the next EF Core Community Standup on Wednesday, June 10th at 10:00am PT. For details, visit [https://live.dot.net](https://live.dot.net/).

## Prerequisites

The previews of EF Core 5.0 require .NET Standard 2.1. This means:

* EF Core 5.0 runs on .NET Core 3.1; it does not require .NET 5.
  * This may change in future previews depending on how the plan for .NET 5 evolves.
* EF Core 5.0 runs on other platforms that support [.NET Standard 2.1](https://docs.microsoft.com/ef/core/platforms/).
* EF Core 5.0 will **not** run on .NET Standard 2.0 platforms, including .NET Framework.

---

## How to get EF Core 5.0 previews

EF Core is distributed exclusively as a set of NuGet packages.
For example, to add the SQL Server provider to your project, you can use the following command using the dotnet tool:

```cmd
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 5.0.0-TBDVersion
```

The EF Core packages published today are:

* [Microsoft.EntityFrameworkCore](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/5.0.0-TBDVersion) - The main EF Core package
* [Microsoft.EntityFrameworkCore.SqlServer](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer/5.0.0-TBDVersion) - Database provider for Microsoft SQL Server and SQL Azure
* [Microsoft.EntityFrameworkCore.Sqlite](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite/5.0.0-TBDVersion) - Database provider for SQLite
* [Microsoft.EntityFrameworkCore.Cosmos](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Cosmos/5.0.0-TBDVersion) - Database provider for Azure Cosmos DB
* [Microsoft.EntityFrameworkCore.InMemory](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.InMemory/5.0.0-TBDVersion) - The in-memory database provider
* [Microsoft.EntityFrameworkCore.Tools](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Tools/5.0.0-TBDVersion) - EF Core PowerShell commands for the Visual Studio Package Manager Console
* [Microsoft.EntityFrameworkCore.Design](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Design/5.0.0-TBDVersion) - Shared design-time components for EF Core tools
* [Microsoft.EntityFrameworkCore.SqlServer.NetTopologySuite](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer.NetTopologySuite/5.0.0-TBDVersion) - SQL Server support for spatial types
* [Microsoft.EntityFrameworkCore.Sqlite.NetTopologySuite](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite.NetTopologySuite/5.0.0-TBDVersion) - SQLite support for spatial types
* [Microsoft.EntityFrameworkCore.Proxies](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Proxies/5.0.0-TBDVersion) - Lazy-loading and change-tracking proxies
* [Microsoft.EntityFrameworkCore.Abstractions](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Abstractions/5.0.0-TBDVersion) - Decoupled EF Core abstractions
* [Microsoft.EntityFrameworkCore.Relational](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Relational/5.0.0-TBDVersion) - Shared EF Core components for relational database providers
* [Microsoft.EntityFrameworkCore.Analyzers](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Analyzers/5.0.0-TBDVersion) - C# analyzers for EF Core
* [Microsoft.EntityFrameworkCore.Sqlite.Core](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite.Core/5.0.0-TBDVersion) - Database provider for SQLite without a packaged native binary

We have also published the 5.0 preview 5 release of the [Microsoft.Data.Sqlite.Core](https://www.nuget.org/packages/Microsoft.Data.Sqlite.Core/5.0.0-TBDVersion) ADO.NET provider.

## Installing dotnet ef

As with EF Core 3.0 and 3.1, the dotnet ef command-line tool is no longer included in the .NET Core SDK. Before you can execute EF Core migration or scaffolding commands, you'll have to install this package as either a global or local tool.

![dotnet-ef](https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/05/efcore-preview4-scaled.jpg)

To install the preview tool globally, first uninstall any existing version with:

```cmd
dotnet tool uninstall --global dotnet-ef
```

Then install with:

```cmd
dotnet tool install --global dotnet-ef --version 5.0.0-TBDVersion
```

It's possible to use this new version of dotnet ef with projects that use older versions of the EF Core runtime.

---

## What's new in EF Core 5 Preview 5

We maintain documentation covering [new features introduced into each preview](https://docs.microsoft.com/ef/core/what-is-new/ef-core-5.0/whatsnew).

Some of the highlights from preview 4 are called out below. This preview also includes several bug fixes.

### Database collations

The default collation for a database can now be specified in the EF model.
This will flow through to generated migrations to set the collation when the database is created.
For example:

```CSharp
modelBuilder.UseCollation("German_PhoneBook_CI_AS");
```

Migrations then generates the following to create the database on SQL Server:

```sql
CREATE DATABASE [Test]
COLLATE German_PhoneBook_CI_AS;
```

The collation to use for specific database columns can also be specified.
For example:

```CSharp
 modelBuilder
     .Entity<User>()
     .Property(e => e.Name)
     .UseCollation("German_PhoneBook_CI_AS");
```

For those not using migrations, collations are now reverse-engineered from the database when scaffolding a DbContext.

Finally, the `EF.Functions.Collate()` allows for ad-hoc queries using different collations.
For example:

```CSharp
context.Users.Single(e => EF.Functions.Collate(e.Name, "French_CI_AS") == "Jean-Michel Jarre");
```

This will generate the following query for SQL Server:

```sql
SELECT TOP(2) [u].[Id], [u].[Name]
FROM [Users] AS [u]
WHERE [u].[Name] COLLATE French_CI_AS = N'Jean-Michel Jarre'
```

Note that ad-hoc collations should be used with care as they can negatively impact database performance.

Documentation is tracked by issue [#2273](https://github.com/dotnet/EntityFramework.Docs/issues/2273).

### Flow arguments into IDesignTimeDbContextFactory

Arguments are now flowed from the command line into the `CreateDbContext` method of [IDesignTimeDbContextFactory](https://docs.microsoft.com/dotnet/api/microsoft.entityframeworkcore.design.idesigntimedbcontextfactory-1?view=efcore-3.1). 
For example, to indicate this is a dev build, a custom argument (e.g. `dev`) can passed on the command line:

```
dotnet ef migrations add two --verbose --dev
``` 

This argument will then flow into the factory, where it can be used to control how the context is created and initialized.
For example:

```CSharp
public class MyDbContextFactory : IDesignTimeDbContextFactory<SomeDbContext>
{
    public SomeDbContext CreateDbContext(string[] args) 
        => new SomeDbContext(args.Contains("--dev"));
}
```

Documentation is tracked by issue [#2419](https://github.com/dotnet/EntityFramework.Docs/issues/2419).

### No-tracking queries with identity resolution

No-tracking queries can now be configured to perform identity resolution.
For example, the following query will create a new Blog instance for each Post, even if each Blog has the same primary key. 

```CSharp
context.Posts.AsNoTracking().Include(e => e.Blog).ToList();
```

However, at the expense of usually being slightly slower and always using more memory, this query can be changed to ensure only a single Blog instance is created:

```CSharp
context.Posts.AsNoTracking().PerformIdentityResolution().Include(e => e.Blog).ToList();
```

Note that this is only useful for no-tracking queries since all tracking queries already exhibit this behavior. 
Also, following API review, the `PerformIdentityResolution` syntax will be changed.
See [#19877](https://github.com/dotnet/efcore/issues/19877#issuecomment-637371073).

Documentation is tracked by issue [#1895](https://github.com/dotnet/EntityFramework.Docs/issues/1895).

### Stored (persisted) computed columns

Most databases allow computed column values to be stored after computation.
While this takes up disk space, the computed column is calculated only once on update, instead of each time its value is retrieved.
This also allows the column to be indexed for some databases.

EF Core 5.0 allows computed columns to be configured as stored.
For example:
 
```CSharp
modelBuilder
    .Entity<User>()
    .Property(e => e.SomethingComputed)
    .HasComputedColumnSql("my sql", stored: true);
```

### SQLite computed columns

EF Core now supports computed columns in SQLite databases.

---

## Daily builds

EF Core previews are aligned with .NET 5 previews. These previews tend to lag behind the latest work on EF Core. Consider using the [daily builds](https://github.com/aspnet/AspNetCore/blob/master/docs/DailyBuilds.md) instead to get the most up-to-date EF Core features and bug fixes.

As with the previews, the daily builds do not require .NET 5; they can be used with GA/RTM release of .NET Core 3.1.

---

## Documentation and feedback

EF Core docs has a new landing page! The main page for Entity Framework documentation has been overhauled to provide you with a hub experience. We hope this new format helps you find the documentation you need faster and with fewer clicks.

The starting point for all EF Core documentation is [docs.microsoft.com/ef/](https://docs.microsoft.com/ef/).

Please file issues found and any other feedback on the [dotnet/efcore GitHub repo](https://github.com/dotnet/efcore).

---

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
 /tr>
</table>
