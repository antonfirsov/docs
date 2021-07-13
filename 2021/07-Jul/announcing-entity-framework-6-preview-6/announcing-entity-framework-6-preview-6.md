---
post_title: 'Announcing Entity Framework Core 6.0 Preview 6: Configure Conventions'
username: jeremy-likness
microsoft_alias: jeliknes
categories: .NET Core, Entity Framework, ASP.NET
summary: Learn about configure conventions and other new features that are available in EF Core 6.0 Preview 6.
desired_publication_date: '2021-07-14'
---
Today, the Entity Framework Core team announces the sixth preview release of EF Core 6.0. The team continues work on the features _you_ helped prioritize. In addition to adding new capabilities, we are closing the gap between EF Core and EF6. [Issue #24106](https://github.com/dotnet/efcore/issues/24106) shares details about the differences and our roadmap to eliminate it. We are making progress on features like [migrations bundles](https://github.com/dotnet/efcore/issues/19693) and [implicit ownership in the Azure Cosmos DB provider](https://github.com/dotnet/efcore/issues/24803) but are not quite ready for you to test them yet. However, many updates are available right now when you download preview 6 (see the end of this blog post for details on how).

> 💡 Starting with preview 6, EF Core 6 targets the .NET 6 [Target Framework Moniker (TFM)](https://docs.microsoft.com/dotnet/standard/frameworks).

A few highlights for this release include:

- Support for 64-bit identity seed values
- Support for new BCL `DateOnly` and `TimeOnly` structs for SQLite
- Uniquify and validate check constraint names
- **Pre-convention model configuration**
- The items on [this list](https://github.com/dotnet/efcore/issues?q=is%3Aissue+sort%3Aupdated-desc+milestone%3A6.0.0-preview6+is%3Aclosed)

This release supports **pre-convention model configuration**. Doing an initial discovery of the model based only on `DbSet` roots was a reasonable approach when it was easy to distinguish likely scalar properties from likely navigation properties. However, as we allow more and more types to be mapped, it has become increasingly problematic to...

- Exclude a type as an entity type and therefore avoid trying to bring it and all its properties into the model
- Revert a type from being an entity type when a value converter is found or the type is ignored

This results in both bugs and additional overhead. This release, we focused on finding ways to enhance model building so that it can more efficiently figure out what is an entity type and what is not. For example, assume you always store string data as byte arrays. Instead of configuring every single entity, you can use the `ConfigureConventions` override. It looks like this:

```csharp
protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
  {
      configurationBuilder.Properties<string>()
          .HaveConversion<byte[]>()
          .HaveMaxLength(255);

      configurationBuilder.IgnoreAny<INonPersisted>();
  }
```


> **We want to hear from you!** If you and/or your team are using the Entity Framework Designer (the visual editor for EDMX files in Visual Studio) we'd like to understand how you use the designer and what might be stopping you from migrating to the latest code base. Give us feedback by commenting on [issue #25248](https://github.com/dotnet/efcore/issues/25248).

## How to get EF Core 6.0 previews

EF Core is distributed exclusively as a set of NuGet packages. For example, to add the SQL Server provider to your project, you can use the following command using the dotnet tool:

```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 6.0.0-preview.6.?????.?
```

This following table links to the preview 6 versions of the EF Core 6.0 packages and describes what they are used for.

|**Package**    |**Purpose**      |
|--------------:|:----------------|
|[Microsoft.EntityFrameworkCore](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/6.0.0-preview.6.?????.?)|The main EF Core package that is independent of specific database providers|
|[Microsoft.EntityFrameworkCore.SqlServer](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer/6.0.0-preview.6.?????.?)|Database provider for Microsoft SQL Server and SQL Azure|
|[Microsoft.EntityFrameworkCore.SqlServer.NetTopologySuite](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer.NetTopologySuite/6.0.0-preview.6.?????.?)|SQL Server support for spatial types|
|[Microsoft.EntityFrameworkCore.Sqlite](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite/6.0.0-preview.6.?????.?)|Database provider for SQLite that includes the native binary for the database engine|
|[Microsoft.EntityFrameworkCore.Sqlite.Core](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite.Core/6.0.0-preview.6.?????.?)|Database provider for SQLite _without_ a packaged native binary|
|[Microsoft.EntityFrameworkCore.Sqlite.NetTopologySuite](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite.NetTopologySuite/6.0.0-preview.6.?????.?)|SQLite support for spatial types|
|[Microsoft.EntityFrameworkCore.Cosmos](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Cosmos/6.0.0-preview.6.?????.?)|Database provider for Azure Cosmos DB|
|[Microsoft.EntityFrameworkCore.InMemory](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.InMemory/6.0.0-preview.6.?????.?)|The in-memory database provider|
|[Microsoft.EntityFrameworkCore.Tools](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Tools/6.0.0-preview.6.?????.?)|EF Core PowerShell commands for the Visual Studio Package Manager Console; use this to integrate tools like [scaffolding](https://docs.microsoft.com/ef/core/managing-schemas/scaffolding) and [migrations](https://docs.microsoft.com/ef/core/managing-schemas/migrations/) with Visual Studio|
|[Microsoft.EntityFrameworkCore.Design](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Design/6.0.0-preview.6.?????.?)|Shared design-time components for EF Core tools|
|[Microsoft.EntityFrameworkCore.Proxies](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Proxies/6.0.0-preview.6.?????.?)|Lazy-loading and change-tracking proxies|
|[Microsoft.EntityFrameworkCore.Abstractions](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Abstractions/6.0.0-preview.6.?????.?)|Decoupled EF Core abstractions; use this for features like extended data annotations defined by EF Core|
|[Microsoft.EntityFrameworkCore.Relational](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Relational/6.0.0-preview.6.?????.?)|Shared EF Core components for relational database providers|
|[Microsoft.EntityFrameworkCore.Analyzers](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Analyzers/6.0.0-preview.6.?????.?)|C# analyzers for EF Core|

We also published the 6.0 preview 6 release of the [Microsoft.Data.Sqlite.Core](https://www.nuget.org/packages/Microsoft.Data.Sqlite.Core/6.0.0-preview.6.?????.?) provider for [ADO.NET](https://docs.microsoft.com/dotnet/framework/data/adonet/ado-net-overview).

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

## Thank you to our contributors!

We are grateful to our amazing community of contributors. Our success is founded upon the shoulders of your efforts and feedback. If you are interested in contributing but not sure how or would like help, please reach out to us! We want to help you succeed. We would like to publicly acknowledge and thank these contributors for investing in the success of EF Core 6.0.

| | | | |
|:--:|:--:|:--:|:--:|
|**[AkinSabriCam](https://github.com/AkinSabriCam)**|**[alexernest](https://github.com/alexernest)**|**[alexpotter10](https://github.com/alexpotter10)**|**[Ali-YousefiTelori](https://github.com/Ali-YousefiTelori)**|
|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3157)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3225)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3143)|[#1](https://github.com/dotnet/efcore/pull/23946), [#2](https://github.com/dotnet/efcore/pull/23946)|
| | | | |
**[AlirezaRezaeeIR](https://github.com/AlirezaRezaeeIR)**|**[andrejs86](https://github.com/andrejs86)**|**[AndrewKitu](https://github.com/AndrewKitu)**|**[ardalis](https://github.com/ardalis)**|
[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3232)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3182)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3070)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3091)|
| | | | |
**[bartoszluka](https://github.com/bartoszluka)**|**[CaringDev](https://github.com/CaringDev)**|**[carlreid](https://github.com/carlreid)**|**[carlreinke](https://github.com/carlreinke)**|
[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3317)|[#1](https://github.com/dotnet/efcore/pull/23585), [#2](https://github.com/dotnet/efcore/pull/23585)|[#1](https://github.com/dotnet/efcore/pull/24498), [#2](https://github.com/dotnet/efcore/pull/24498)|[#1](https://github.com/dotnet/efcore/pull/23694), [#2](https://github.com/dotnet/efcore/pull/23694)|
| | | | |
**[cgrevil](https://github.com/cgrevil)**|**[cgrimes01](https://github.com/cgrimes01)**|**[cincuranet](https://github.com/cincuranet)**|**[cuperman007](https://github.com/cuperman007)**|
[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3154)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3038)|[#1](https://github.com/dotnet/efcore/pull/24234), [#2](https://github.com/dotnet/efcore/pull/24234), [#3](https://github.com/dotnet/EntityFramework.Docs/pull/2714), [#4](https://github.com/dotnet/EntityFramework.Docs/pull/3185)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2574)|
| | | | |
**[dan-giddins](https://github.com/dan-giddins)**|**[dannyjacosta](https://github.com/dannyjacosta)**|**[dennisseders](https://github.com/dennisseders)**|**[DickBaker](https://github.com/DickBaker)**|
[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2910)|[#1](https://github.com/dotnet/efcore/pull/24839), [#2](https://github.com/dotnet/efcore/pull/24839)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2839), [#2](https://github.com/dotnet/EntityFramework.Docs/pull/2845), [#3](https://github.com/dotnet/EntityFramework.Docs/pull/2848), [#4](https://github.com/dotnet/EntityFramework.Docs/pull/2987), [#5](https://github.com/dotnet/EntityFramework.Docs/pull/2997), [#6](https://github.com/dotnet/EntityFramework.Docs/pull/3007)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2990)|
| | | | |
**[ErikEJ](https://github.com/ErikEJ)**|**[fagnercarvalho](https://github.com/fagnercarvalho)**|**[FarshanAhamed](https://github.com/FarshanAhamed)**|**[filipnavara](https://github.com/filipnavara)**|
[#1](https://github.com/dotnet/efcore/pull/22900), [#2](https://github.com/dotnet/efcore/pull/22900), [#3](https://github.com/dotnet/efcore/pull/22937), [#4](https://github.com/dotnet/efcore/pull/22937), [#5](https://github.com/dotnet/efcore/pull/22938), [#6](https://github.com/dotnet/efcore/pull/22938), [#7](https://github.com/dotnet/efcore/pull/24937), [#8](https://github.com/dotnet/efcore/pull/24937), [#9](https://github.com/dotnet/EntityFramework.Docs/pull/2897), [#10](https://github.com/dotnet/EntityFramework.Docs/pull/2984), [#11](https://github.com/dotnet/EntityFramework.Docs/pull/3187), [#12](https://github.com/dotnet/EntityFramework.Docs/pull/3197), [#13](https://github.com/dotnet/EntityFramework.Docs/pull/3230), [#14](https://github.com/dotnet/EntityFramework.Docs/pull/3257), [#15](https://github.com/dotnet/EntityFramework.Docs/pull/3303), [#16](https://github.com/dotnet/EntityFramework.Docs/pull/3312)|[#1](https://github.com/dotnet/efcore/pull/23094), [#2](https://github.com/dotnet/efcore/pull/23094)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3181)|[#1](https://github.com/dotnet/efcore/pull/23591), [#2](https://github.com/dotnet/efcore/pull/23591)|
| | | | |
**[garyng](https://github.com/garyng)**|**[Geoff1900](https://github.com/Geoff1900)**|**[gfoidl](https://github.com/gfoidl)**|**[gieseanw](https://github.com/gieseanw)**|
[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3045), [#2](https://github.com/dotnet/EntityFramework.Docs/pull/3046), [#3](https://github.com/dotnet/EntityFramework.Docs/pull/3047)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3025)|[#1](https://github.com/dotnet/efcore/pull/22923), [#2](https://github.com/dotnet/efcore/pull/22923)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3262)|
| | | | |
**[Giorgi](https://github.com/Giorgi)**|**[GitHubPang](https://github.com/GitHubPang)**|**[gurustron](https://github.com/gurustron)**|**[hez2010](https://github.com/hez2010)**|
[#1](https://github.com/dotnet/efcore/pull/24147), [#2](https://github.com/dotnet/efcore/pull/24147), [#3](https://github.com/dotnet/EntityFramework.Docs/pull/3106), [#4](https://github.com/dotnet/EntityFramework.Docs/pull/3107)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3097), [#2](https://github.com/dotnet/EntityFramework.Docs/pull/3315)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3010)|[#1](https://github.com/dotnet/efcore/pull/24211), [#2](https://github.com/dotnet/efcore/pull/24211)|
| | | | |
**[HSchwichtenberg](https://github.com/HSchwichtenberg)**|**[jaliyaudagedara](https://github.com/jaliyaudagedara)**|**[jantlee](https://github.com/jantlee)**|**[jeremycook](https://github.com/jeremycook)**|
[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2894), [#2](https://github.com/dotnet/EntityFramework.Docs/pull/3256)|[#1](https://github.com/dotnet/efcore/pull/24499), [#2](https://github.com/dotnet/efcore/pull/24499)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2786)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2827)|
| | | | |
**[jing8956](https://github.com/jing8956)**|**[joakimriedel](https://github.com/joakimriedel)**|**[joaopgrassi](https://github.com/joaopgrassi)**|**[joelmandell](https://github.com/joelmandell)**|
[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3146)|[#1](https://github.com/dotnet/efcore/pull/23437), [#2](https://github.com/dotnet/efcore/pull/23437), [#3](https://github.com/dotnet/EntityFramework.Docs/pull/3321)|[#1](https://github.com/dotnet/efcore/pull/22849), [#2](https://github.com/dotnet/efcore/pull/22849)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3305)|
| | | | |
**[JohnMHigginsPMC](https://github.com/JohnMHigginsPMC)**|**[JonasSamuelsson](https://github.com/JonasSamuelsson)**|**[josemiltonsampaio](https://github.com/josemiltonsampaio)**|**[KaloyanIT](https://github.com/KaloyanIT)**|
[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3313)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3309)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2927)|[#1](https://github.com/dotnet/efcore/pull/23563), [#2](https://github.com/dotnet/efcore/pull/23563), [#3](https://github.com/dotnet/efcore/pull/23666), [#4](https://github.com/dotnet/efcore/pull/23666)|
| | | | |
**[khalidabuhakmeh](https://github.com/khalidabuhakmeh)**|**[khellang](https://github.com/khellang)**|**[koenbeuk](https://github.com/koenbeuk)**|**[kotpal](https://github.com/kotpal)**|
[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2858), [#2](https://github.com/dotnet/EntityFramework.Docs/pull/2962)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2982)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2921), [#2](https://github.com/dotnet/EntityFramework.Docs/pull/3283)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2763)|
| | | | |
**[larsholm](https://github.com/larsholm)**|**[lauxjpn](https://github.com/lauxjpn)**|**[leonardoporro](https://github.com/leonardoporro)**|**[lexkazakov](https://github.com/lexkazakov)**|
[#1](https://github.com/dotnet/efcore/pull/24624), [#2](https://github.com/dotnet/efcore/pull/24624)|[#1](https://github.com/dotnet/efcore/pull/24806), [#2](https://github.com/dotnet/efcore/pull/24806), [#3](https://github.com/dotnet/efcore/pull/24819), [#4](https://github.com/dotnet/efcore/pull/24819), [#5](https://github.com/dotnet/efcore/pull/25128), [#6](https://github.com/dotnet/efcore/pull/25128)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2883)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3191)|
| | | | |
**[mariuz](https://github.com/mariuz)**|**[marodev](https://github.com/marodev)**|**[martincostello](https://github.com/martincostello)**|**[MartinWestminster](https://github.com/MartinWestminster)**|
[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3124)|[#1](https://github.com/dotnet/efcore/pull/24951), [#2](https://github.com/dotnet/efcore/pull/24951)|[#1](https://github.com/dotnet/efcore/pull/25021), [#2](https://github.com/dotnet/efcore/pull/25021), [#3](https://github.com/dotnet/efcore/pull/25041), [#4](https://github.com/dotnet/efcore/pull/25041)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3118)|
| | | | |
**[Marusyk](https://github.com/Marusyk)**|**[massytech](https://github.com/massytech)**|**[MattKomorcec](https://github.com/MattKomorcec)**|**[MaxG117](https://github.com/MaxG117)**|
[#1](https://github.com/dotnet/efcore/pull/23039), [#2](https://github.com/dotnet/efcore/pull/23039), [#3](https://github.com/dotnet/efcore/pull/24016), [#4](https://github.com/dotnet/efcore/pull/24016), [#5](https://github.com/dotnet/efcore/pull/24203), [#6](https://github.com/dotnet/efcore/pull/24203), [#7](https://github.com/dotnet/efcore/pull/24204), [#8](https://github.com/dotnet/efcore/pull/24204), [#9](https://github.com/dotnet/efcore/pull/24247), [#10](https://github.com/dotnet/efcore/pull/24247), [#11](https://github.com/dotnet/efcore/pull/24284), [#12](https://github.com/dotnet/efcore/pull/24284), [#13](https://github.com/dotnet/efcore/pull/24286), [#14](https://github.com/dotnet/efcore/pull/24286), [#15](https://github.com/dotnet/efcore/pull/24323), [#16](https://github.com/dotnet/efcore/pull/24323), [#17](https://github.com/dotnet/efcore/pull/24463), [#18](https://github.com/dotnet/efcore/pull/24463)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2490)|[#1](https://github.com/dotnet/efcore/pull/24728), [#2](https://github.com/dotnet/efcore/pull/24728)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2898)|
| | | | |
**[mefateah](https://github.com/mefateah)**|**[meggima](https://github.com/meggima)**|**[mguinness](https://github.com/mguinness)**|**[michalczerwinski](https://github.com/michalczerwinski)**|
[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3065)|[#1](https://github.com/dotnet/efcore/pull/23605), [#2](https://github.com/dotnet/efcore/pull/23605)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3227)|[#1](https://github.com/dotnet/efcore/pull/24793), [#2](https://github.com/dotnet/efcore/pull/24793), [#3](https://github.com/dotnet/efcore/pull/24814), [#4](https://github.com/dotnet/efcore/pull/24814), [#5](https://github.com/dotnet/efcore/pull/24887), [#6](https://github.com/dotnet/efcore/pull/24887)|
| | | | |
**[mrlife](https://github.com/mrlife)**|**[msawczyn](https://github.com/msawczyn)**|**[MSDN-WhiteKnight](https://github.com/MSDN-WhiteKnight)**|**[natashanikolic](https://github.com/natashanikolic)**|
[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3094), [#2](https://github.com/dotnet/EntityFramework.Docs/pull/3128), [#3](https://github.com/dotnet/EntityFramework.Docs/pull/3129), [#4](https://github.com/dotnet/EntityFramework.Docs/pull/3132)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2917)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2887)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2920)|
| | | | |
**[nmichels](https://github.com/nmichels)**|**[nschonni](https://github.com/nschonni)**|**[ntovas](https://github.com/ntovas)**|**[OKTAYKIR](https://github.com/OKTAYKIR)**|
[#1](https://github.com/dotnet/efcore/pull/23091), [#2](https://github.com/dotnet/efcore/pull/23091)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2775), [#2](https://github.com/dotnet/EntityFramework.Docs/pull/2776), [#3](https://github.com/dotnet/EntityFramework.Docs/pull/2779), [#4](https://github.com/dotnet/EntityFramework.Docs/pull/2780)|[#1](https://github.com/dotnet/efcore/pull/24829), [#2](https://github.com/dotnet/efcore/pull/24829)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3145)|
| | | | |
**[OOberoi](https://github.com/OOberoi)**|**[Oxyrus](https://github.com/Oxyrus)**|**[pkellner](https://github.com/pkellner)**|**[ptupitsyn](https://github.com/ptupitsyn)**|
[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3163), [#2](https://github.com/dotnet/EntityFramework.Docs/pull/3319)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3110)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2954)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3202)|
| | | | |
**[ralmsdeveloper](https://github.com/ralmsdeveloper)**|**[RaymondHuy](https://github.com/RaymondHuy)**|**[riscie](https://github.com/riscie)**|**[seekingtheoptimal](https://github.com/seekingtheoptimal)**|
[#1](https://github.com/dotnet/efcore/pull/19473), [#2](https://github.com/dotnet/efcore/pull/19473)|[#1](https://github.com/dotnet/efcore/pull/22514), [#2](https://github.com/dotnet/efcore/pull/22514), [#3](https://github.com/dotnet/efcore/pull/23145), [#4](https://github.com/dotnet/efcore/pull/23145), [#5](https://github.com/dotnet/efcore/pull/23232), [#6](https://github.com/dotnet/efcore/pull/23232), [#7](https://github.com/dotnet/efcore/pull/23424), [#8](https://github.com/dotnet/efcore/pull/23424)|[#1](https://github.com/dotnet/efcore/pull/20792), [#2](https://github.com/dotnet/efcore/pull/20792)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2473)|
| | | | |
**[SergerGood](https://github.com/SergerGood)**|**[Shirasho](https://github.com/Shirasho)**|**[SimonCropp](https://github.com/SimonCropp)**|**[smagurauskas](https://github.com/smagurauskas)**|
[#1](https://github.com/dotnet/efcore/pull/24750), [#2](https://github.com/dotnet/efcore/pull/24750), [#3](https://github.com/dotnet/efcore/pull/24751), [#4](https://github.com/dotnet/efcore/pull/24751), [#5](https://github.com/dotnet/efcore/pull/24752), [#6](https://github.com/dotnet/efcore/pull/24752), [#7](https://github.com/dotnet/efcore/pull/24753), [#8](https://github.com/dotnet/efcore/pull/24753), [#9](https://github.com/dotnet/efcore/pull/24755), [#10](https://github.com/dotnet/efcore/pull/24755)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2988)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/2957), [#2](https://github.com/dotnet/EntityFramework.Docs/pull/2959)|[#1](https://github.com/dotnet/efcore/pull/23933), [#2](https://github.com/dotnet/efcore/pull/23933)|
| | | | |
**[stevendarby](https://github.com/stevendarby)**|**[Strepto](https://github.com/Strepto)**|**[taha-ghadirian](https://github.com/taha-ghadirian)**|**[teo-tsirpanis](https://github.com/teo-tsirpanis)**|
[#1](https://github.com/dotnet/efcore/pull/24746), [#2](https://github.com/dotnet/efcore/pull/24746)|[#1](https://github.com/dotnet/efcore/pull/24141), [#2](https://github.com/dotnet/efcore/pull/24141)|[#1](https://github.com/dotnet/efcore/pull/25085), [#2](https://github.com/dotnet/efcore/pull/25085)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3123), [#2](https://github.com/dotnet/EntityFramework.Docs/pull/3268)|
| | | | |
**[the-wazz](https://github.com/the-wazz)**|**[thiggins1990](https://github.com/thiggins1990)**|**[tkp1n](https://github.com/tkp1n)**|**[umitkavala](https://github.com/umitkavala)**|
[#1](https://github.com/dotnet/efcore/pull/23551), [#2](https://github.com/dotnet/efcore/pull/23551)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3219)|[#1](https://github.com/dotnet/efcore/pull/23014), [#2](https://github.com/dotnet/efcore/pull/23014)|[#1](https://github.com/dotnet/efcore/pull/23322), [#2](https://github.com/dotnet/efcore/pull/23322), [#3](https://github.com/dotnet/efcore/pull/23562), [#4](https://github.com/dotnet/efcore/pull/23562), [#5](https://github.com/dotnet/efcore/pull/24856), [#6](https://github.com/dotnet/efcore/pull/24856)|
| | | | |
**[uncheckederror](https://github.com/uncheckederror)**|**[Varorbc](https://github.com/Varorbc)**|**[vincent1405](https://github.com/vincent1405)**|**[vonzshik](https://github.com/vonzshik)**|
[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3168)|[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3141)|[#1](https://github.com/dotnet/efcore/pull/24020), [#2](https://github.com/dotnet/efcore/pull/24020)|[#1](https://github.com/dotnet/efcore/pull/24775), [#2](https://github.com/dotnet/efcore/pull/24775), [#3](https://github.com/dotnet/efcore/pull/24778), [#4](https://github.com/dotnet/efcore/pull/24778), [#5](https://github.com/dotnet/efcore/pull/24863), [#6](https://github.com/dotnet/efcore/pull/24863)|
| | | | |
**[vytotas](https://github.com/vytotas)**|**[wdesgardin](https://github.com/wdesgardin)**|**[wmeints](https://github.com/wmeints)**|**[yesmey](https://github.com/yesmey)**|
[#1](https://github.com/dotnet/EntityFramework.Docs/pull/3158)|[#1](https://github.com/dotnet/efcore/pull/24588), [#2](https://github.com/dotnet/efcore/pull/24588)|[#1](https://github.com/dotnet/efcore/pull/23873), [#2](https://github.com/dotnet/efcore/pull/23873)|[#1](https://github.com/dotnet/efcore/pull/24111), [#2](https://github.com/dotnet/efcore/pull/24111), [#3](https://github.com/dotnet/efcore/pull/24155), [#4](https://github.com/dotnet/efcore/pull/24155), [#5](https://github.com/dotnet/efcore/pull/24160), [#6](https://github.com/dotnet/efcore/pull/24160)|
| | | | |
