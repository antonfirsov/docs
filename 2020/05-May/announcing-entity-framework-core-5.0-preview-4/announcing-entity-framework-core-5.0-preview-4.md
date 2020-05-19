# Announcing Entity Framework Core 5.0 Preview 4

Today we are excited to announce the fourth preview release of [EF Core 5.0](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/5.0.0-preview.4.20220.10).

The fourth previews of [.NET 5](https://devblogs.microsoft.com/dotnet/announcing-net-5-preview-4-and-our-journey-to-one-net) and ASP.NET Core 5.0 are also available now.

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

<pre class="prettyprint">
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 5.0.0-preview.4.20220.10
</pre>

The EF Core packages published today are:

* [Microsoft.EntityFrameworkCore](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/5.0.0-preview.4.20220.10) - The main EF Core package
* [Microsoft.EntityFrameworkCore.SqlServer](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer/5.0.0-preview.4.20220.10) - Database provider for Microsoft SQL Server and SQL Azure
* [Microsoft.EntityFrameworkCore.Sqlite](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite/5.0.0-preview.4.20220.10) - Database provider for SQLite
* [Microsoft.EntityFrameworkCore.Cosmos](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Cosmos/5.0.0-preview.4.20220.10) - Database provider for Azure Cosmos DB
* [Microsoft.EntityFrameworkCore.InMemory](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.InMemory/5.0.0-preview.4.20220.10) - The in-memory database provider
* [Microsoft.EntityFrameworkCore.Tools](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Tools/5.0.0-preview.4.20220.10) - EF Core PowerShell commands for the Visual Studio Package Manager Console
* [Microsoft.EntityFrameworkCore.Design](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Design/5.0.0-preview.4.20220.10) - Shared design-time components for EF Core tools
* [Microsoft.EntityFrameworkCore.SqlServer.NetTopologySuite](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer.NetTopologySuite/5.0.0-preview.4.20220.10) - SQL Server support for spatial types
* [Microsoft.EntityFrameworkCore.Sqlite.NetTopologySuite](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite.NetTopologySuite/5.0.0-preview.4.20220.10) - SQLite support for spatial types
* [Microsoft.EntityFrameworkCore.Proxies](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Proxies/5.0.0-preview.4.20220.10) - Lazy-loading and change-tracking proxies
* [Microsoft.EntityFrameworkCore.Abstractions](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Abstractions/5.0.0-preview.4.20220.10) - Decoupled EF Core abstractions
* [Microsoft.EntityFrameworkCore.Relational](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Relational/5.0.0-preview.4.20220.10) - Shared EF Core components for relational database providers
* [Microsoft.EntityFrameworkCore.Analyzers](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Analyzers/5.0.0-preview.4.20220.10) - C# analyzers for EF Core
* [Microsoft.EntityFrameworkCore.Sqlite.Core](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite.Core/5.0.0-preview.4.20220.10) - Database provider for SQLite without a packaged native binary

We have also published the 5.0 preview 4 release of the [Microsoft.Data.Sqlite.Core](https://www.nuget.org/packages/Microsoft.Data.Sqlite.Core/5.0.0-preview.4.20220.10) ADO.NET provider.

## Installing dotnet ef

As with EF Core 3.0 and 3.1, the dotnet ef command-line tool is no longer included in the .NET Core SDK. Before you can execute EF Core migration or scaffolding commands, you'll have to install this package as either a global or local tool.

![dotnet-ef](https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/05/efcore-preview4-scaled.jpg)

To install the preview tool globally, first uninstall any existing version with:

<pre class="prettyprint">
dotnet tool uninstall --global dotnet-ef
</pre>

Then install with:

<pre class="prettyprint">
dotnet tool install --global dotnet-ef --version 5.0.0-preview.4.20220.10
</pre>

It's possible to use this new version of dotnet ef with projects that use older versions of the EF Core runtime.

---

## What's new in EF Core 5 Preview 4

We maintain documentation covering [new features introduced into each preview](https://docs.microsoft.com/ef/core/what-is-new/ef-core-5.0/whatsnew).

Some of the highlights from preview 4 are called out below. This preview also includes several bug fixes.

### Configure database precision/scale in model

Precision and scale for a property can now be specified using the model builder.
For example:

```CSharp
modelBuilder
    .Entity<Blog>()
    .Property(b => b.Numeric)
    .HasPrecision(16, 4);
```

Precision and scale can still be set via the full database type, such as "decimal(16,4)". 

Documentation is tracked by issue [#527](https://github.com/dotnet/EntityFramework.Docs/issues/527).

### Specify SQL Server index fill factor

The fill factor can no be specified when creating an index on SQL Server.
For example:

```CSharp
modelBuilder
    .Entity<Customer>()
    .HasIndex(e => e.Name)
    .HasFillFactor(90);
```

Documentation is tracked by issue [#2378](https://github.com/dotnet/EntityFramework.Docs/issues/2378).

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
<td><a href="https://github.com/ajcvickers"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/efteam_ajcvickers.jpeg" alt="ajcvickers" width=200px><br>Arthur Vickers</a>
<td><a href="https://github.com/AndriySvyryd"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/efteam_AndriySvyryd.jpeg" alt="AndriySvyryd" width=200px><br>Andriy Svyryd</a>
<td><a href="https://github.com/bricelam"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/efteam_bricelam.jpeg" alt="" width=200px><br>Brice Lambson</a>
<td><a href="https://github.com/JeremyLikness"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/efteam_JeremyLikness.jpeg" alt="JeremyLikness" width=200px><br>Jeremy Likness</a>
</td>
</tr>

<tr>
<td><a href="https://github.com/lajones"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/efteam_lajones.jpeg" alt="lajones" width=200px><br>lajones</a>
<td><a href="https://github.com/maumar"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/efteam_maumar.jpeg" alt="maumar" width=200px><br>Maurycy Markowski</a>
<td><a href="https://github.com/roji"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/efteam_roji-1-300x300.png" alt="roji" width=200px><br>Shay Rojansky</a>
<td><a href="https://github.com/smitpatel"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/efteam_smitpatel.png" alt="smitpatel" width=200px><br>Smit Patel</a>
</td>
</tr>

</table>

---

## Thank you to our contributors!

A big thank you to the following community members who have already contributed code or documentation to the EF Core 5 release! (List is in chronological order of first contribution to EF Core 5).

<table>
<tr>
<td><a href="https://github.com/aevitas"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_aevitas.jpeg" alt="aevitas" width=200px><br>aevitas</a>
<td><a href="https://github.com/alaatm"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_alaatm.png" alt="alaatm" width=200px><br>Alaa Masoud</a>
<td><a href="https://github.com/aleksandar-manukov"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_aleksandar-manukov.png" alt="aleksandar-manukov" width=200px><br>Aleksandar Manukov</a>
<td><a href="https://github.com/amrbadawy"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_amrbadawy.jpeg" alt="amrbadawy" width=200px><br>Amr Badawy</a>
</td>
</tr>

<tr>
<td><a href="https://github.com/AnthonyMonterrosa"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_AnthonyMonterrosa.jpeg" alt="AnthonyMonterrosa" width=200px><br>Anthony Monterrosa</a>
<td><a href="https://github.com/bbrandt"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_bbrandt.jpeg" alt="bbrandt" width=200px><br>Ben Brandt</a>
<td><a href="https://github.com/benmccallum"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_benmccallum.jpeg" alt="benmccallum" width=200px><br>Ben McCallum</a>
<td><a href="https://github.com/ccjx"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_ccjx.png" alt="ccjx" width=200px><br>Clarence Cai</a>
</td>
</tr>

<tr>
<td><a href="https://github.com/CGijbels"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_CGijbels.jpeg" alt="CGijbels" width=200px><br>Christophe Gijbels</a>
<td><a href="https://github.com/cincuranet"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_cincuranet.png" alt="cincuranet" width=200px><br>Jiri Cincura</a>
<td><a href="https://github.com/Costo"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_Costo.jpeg" alt="Costo" width=200px><br>Vincent Costel</a>
<td><a href="https://github.com/dshuvaev"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_dshuvaev.jpeg" alt="dshuvaev" width=200px><br>Dmitry Shuvaev</a>
</td>
</tr>

<tr>
<td><a href="https://github.com/EricStG"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_EricStG.jpeg" alt="EricStG" width=200px><br>Eric St-Georges</a>
<td><a href="https://github.com/ErikEJ"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_ErikEJ.jpeg" alt="ErikEJ" width=200px><br>Erik Ejlskov Jensen</a>
<td><a href="https://github.com/gravbox"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_gravbox.png" alt="gravbox" width=200px><br>Christopher Davis</a>
<td><a href="https://github.com/ivaylokenov"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_ivaylokenov.jpeg" alt="ivaylokenov" width=200px><br>Ivaylo Kenov</a>
</td>
</tr>

<tr>
<td><a href="https://github.com/jfoshee"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_jfoshee.png" alt="jfoshee" width=200px><br>Jacob Foshee</a>
<td><a href="https://github.com/jmzagorski"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_jmzagorski.png" alt="jmzagorski" width=200px><br>Jeremy Zagorski</a>
<td><a href="https://github.com/jviau"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_jviau.jpeg" alt="jviau" width=200px><br>Jacob Viau</a>
<td><a href="https://github.com/knom"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_knom.png" alt="knom" width=200px><br>Max K.</a>
</td>
</tr>

<tr>
<td><a href="https://github.com/lohoris-crane"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_lohoris-crane.jpeg" alt="lohoris-crane" width=200px><br>lohoris-crane</a>
<td><a href="https://github.com/loic-sharma"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_loic-sharma.jpeg" alt="loic-sharma" width=200px><br>Loïc Sharma</a>
<td><a href="https://github.com/lokalmatador"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_lokalmatador.jpeg" alt="lokalmatador" width=200px><br>lokalmatador</a>
<td><a href="https://github.com/mariusGundersen"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_mariusGundersen.png" alt="mariusGundersen" width=200px><br>Marius Gundersen</a>
</td>
</tr>

<tr>
<td><a href="https://github.com/Marusyk"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_Marusyk.jpeg" alt="Marusyk" width=200px><br>Roman Marusyk</a>
<td><a href="https://github.com/matthiaslischka"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_matthiaslischka.jpeg" alt="matthiaslischka" width=200px><br>Matthias Lischka</a>
<td><a href="https://github.com/MaxG117"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_MaxG117.png" alt="MaxG117" width=200px><br>MaxG117</a>
<td><a href="https://github.com/MHDuke"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_MHDuke.jpeg" alt="MHDuke" width=200px><br>MHDuke</a>
</td>
</tr>

<tr>
<td><a href="https://github.com/mikes-gh"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_mikes-gh.png" alt="mikes-gh" width=200px><br>Mike Surcouf</a>
<td><a href="https://github.com/Muppets"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_Muppets.jpeg" alt="Muppets" width=200px><br>Neil Bostrom</a>
<td><a href="https://github.com/nmichels"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_nmichels.jpeg" alt="nmichels" width=200px><br>Nícolas Michels</a>
<td><a href="https://github.com/OOberoi"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_OOberoi.jpeg" alt="OOberoi" width=200px><br>Obi Oberoi</a>
</td>
</tr>

<tr>
<td><a href="https://github.com/orionstudt"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_orionstudt.jpeg" alt="orionstudt" width=200px><br>Josh Studt</a>
<td><a href="https://github.com/ozantopal"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_ozantopal.jpeg" alt="ozantopal" width=200px><br>Ozan Topal</a>
<td><a href="https://github.com/pmiddleton"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_pmiddleton.jpeg" alt="pmiddleton" width=200px><br>Paul Middleton</a>
<td><a href="https://github.com/prog-rajkamal"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_prog-rajkamal.jpeg" alt="prog-rajkamal" width=200px><br>Raj</a>
</td>
</tr>

<tr>
<td><a href="https://github.com/ptjhuang"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_ptjhuang.png" alt="ptjhuang" width=200px><br>Peter Huang</a>
<td><a href="https://github.com/ralmsdeveloper"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_ralmsdeveloper.png" alt="ralmsdeveloper" width=200px><br>Rafael Almeida Santos</a>
<td><a href="https://github.com/redoz"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_redoz.png" alt="redoz" width=200px><br>Patrik Husfloen</a>
<td><a href="https://github.com/rmarskell"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_rmarskell.jpeg" alt="rmarskell" width=200px><br>Richard Marskell</a>
</td>
</tr>

<tr>
<td><a href="https://github.com/sguitardude"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_sguitardude.jpeg" alt="sguitardude" width=200px><br>sguitardude</a>
<td><a href="https://github.com/SimpleSamples"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_SimpleSamples.png" alt="SimpleSamples" width=200px><br>Sam Hobbs</a>
<td><a href="https://github.com/svengeance"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_svengeance.png" alt="svengeance" width=200px><br>Sven</a>
<td><a href="https://github.com/VladDragnea"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_VladDragnea.jpeg" alt="VladDragnea" width=200px><br>Vlad</a>
</td>
</tr>

<tr>
<td><a href="https://github.com/vslee"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_vslee.png" alt="vslee" width=200px><br>vslee</a>
<td><a href="https://github.com/WeihanLi"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_WeihanLi.jpeg" alt="WeihanLi" width=200px><br>liweihan</a>
<td><a href="https://github.com/Youssef1313"><img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/contributor_Youssef1313.jpeg" alt="Youssef1313" width=200px><br>Youssef Victor</a>
</td>
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
 <td>&nbsp;</td>
 <td>&nbsp;</td>
</tr>
</table>
