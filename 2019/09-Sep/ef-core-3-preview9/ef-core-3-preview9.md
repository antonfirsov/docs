# Announcing Entity Framework Core 3.0 Preview 9 and Entity Framework 6.3 Preview 9

The Preview 9 versions of the [EF Core 3.0 package](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/3.0.0-preview9.19423.6) and the [EF 6.3 package](https://www.nuget.org/packages/EntityFramework/6.3.0-preview9-19423-04) are now available for download from [nuget.org](https://www.nuget.org/).

**These are the last planned previews before we release the final versions later this month. We have almost completely stopped making changes to the product code, but we are still actively monitoring feedback for any important bugs that may be reported. So please install the previews to validate that all the functionality required by your applications is available and works correctly, and report any issues you find to either the [EF Core issue tracker](https://github.com/aspnet/EntityFrameworkCore/issues/new) or the [EF 6 issue tracker](https://github.com/aspnet/EntityFramework6/issues/new) on GitHub.** 

**Even if we may not be able to fix many more issues in EF Core 3.0 at this point, we'll consider important bugs and regressions for the upcoming 3.1 release.**

## What's new in EF Core 3.0 Preview 9

Besides [all the other improvements in EF Core 3.0](https://docs.microsoft.com/ef/core/what-is-new/ef-core-3.0/features), preview 9 includes [fixes for more than 100 issues](https://github.com/aspnet/EntityFrameworkCore/issues?q=is%3Aissue+milestone%3A3.0.0-preview9+is%3Aclosed) that we resolved since Preview 8. Here are a few highlights:

- Support for translating queries that project a single result form a collection using window functions (issue [#10001](https://github.com/aspnet/EntityFrameworkCore/issues/10001)) 
  
- Support for translating queries with constants or parameters in the GROUP BY key (issue [#14152](https://github.com/aspnet/EntityFrameworkCore/issues/14152)).

- Improvements to our thread concurrency detection logic to reduce false positives (issue [#14534](https://github.com/aspnet/EntityFrameworkCore/issues/14534)).

### Consider installing daily builds

In Preview 9 the functionality of the in-memory provider is still very limited, but this is already fixed in our daily builds. In fact, [we have already fixed more than 20](https://github.com/aspnet/EntityFrameworkCore/milestone/42?closed=1) that aren't included in Preview 9, and we may still fix a few more before RTM.

Detailed instructions to install daily builds, including the necessary NuGet feeds, can be found in the [How to get daily builds of ASP.NET Core](https://github.com/aspnet/AspNetCore/blob/master/docs/DailyBuilds.md) article.

### Common workarounds for LINQ queries

The LINQ implementation in EF Core 3.0 is designed to work very differently from the one used in previous versions of EF Core. For this reason, you are likely to run into issues with LINQ queries, especially when upgrading existing applications. Here are some workarounds that might help you get things working:

- **Try a [daily build](https://github.com/aspnet/AspNetCore/blob/master/docs/DailyBuilds.md)** (as previously mentioned) to confirm that you aren't hitting an issue that has already been fixed.

- **Switch to client evaluation explicitly:** If your query filters data based on an expression that cannot be translated to SQL, you may need to switch to client evaluation explicitly by inserting a call to either `AsEnumerable()`, `AsAsyncEnumerable()`, `ToList()`, or `ToListAsync()` in the middle of the query. For example, the following query will no longer work in EF Core 3.0 because one of the predicates in the _where_ clause requires client evaluation:
  
   ``` csharp
   var specialCustomers = context.Customers
     .Where(c => c.Name.StartsWith(n) && IsSpecialCustomer(c));
   ```
   
   But if you know it is reasonable to process part of the filter on the client, you can rewrite the query as:

   ``` csharp
   var specialCustomers = context.Customers
     .Where(c => c.Name.StartsWith(n))
     .AsEnumerable() // Start using LINQ to Objects (switch to client evaluation)
     .Where(c => IsSpecialCustomer(c));
   ```
   
   Remember that this is by-design: In EF Core 3.0, [LINQ operations that cannot be translated to SQL are no longer automatically evaluated on the client](https://docs.microsoft.com/ef/core/what-is-new/ef-core-3.0/breaking-changes#linq-queries-are-no-longer-evaluated-on-the-client).

- **Use raw SQL queries:** If some expression in your LINQ query is not translated correctly (or at all) to SQL, but you know what translation you would want to have generated, you may be able to work around the issue by executing your own SQL statement using the `FromSqlRaw()` or `FromSqlInterpolated()` methods. 
   
   Also make sure an issue exists in [our issue tracker on GitHub](https://github.com/aspnet/EntityFrameworkCore/issues) to support the translation of the specific expression.

### Breaking changes

All breaking changes in this release are listed in the [Breaking changes in EF Core 3.0](https://docs.microsoft.com/ef/core/what-is-new/ef-core-3.0/breaking-changes) article. We keep the list up to date on every preview, with the most impactful changes near the top of the list, to make it easier for you to react. 

### Obtaining the Preview 9 packages

EF Core 3.0 is distributed exclusively as NuGet packages. As usual, add or upgrade the runtime to Preview 9 via the NuGet user interface, the Package Manager Console in Visual Studio, or via the `dotnet add package` command. In all cases, include the option to allow installing pre-release versions. For example, you can execute the following command to install the SQL Server provider:

``` console
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 3.0.0-*
```

With .NET Core 3.0, the `dotnet ef` command-line tool is no longer included in the .NET Core SDK. Before you can execute EF Core migration or scaffolding commands, you'll have to install it as either a global or local tool. Due to limitations in `dotnet tool install`, installing preview tools requires specifying at least part of the preview version on the installation command. For example, to install the 3.0 Preview 9 version of `dotnet ef` as a global tool, execute the following command:

``` console
dotnet tool install --global dotnet-ef --version 3.0.0-* 
```

## What's new in EF 6.3 Preview 9

All of the work planned for the EF 6.3 package has been completed. We are now focused on monitoring your feedback and fixing any important bugs that may be reported.

As with EF Core, any bug fixes that happened after we branched for Preview 9 are available in [our daily builds](https://github.com/aspnet/AspNetCore/blob/master/docs/DailyBuilds.md).

### How to work with EDMX files in .NET Core projects
On the tooling side, we plan to release an updated EF6 designer in an upcoming update of Visual Studio 2019 which will work with projects that target .NET Core (tracked in issue [#883](https://github.com/aspnet/EntityFramework6/issues/883)).

Until this new version of the designer is available, we recommend that you work with your EDMX files inside projects that target .NET Framework. You can then add the EDMX file and the generated classes for the entities and the DbContext as linked files to a .NET Core 3.0 or .NET Standard 2.1 project in the same solution. For example, the project file for the .NET Core project can include the linked files like this:

``` csproj
  <ItemGroup>
    <EntityDeploy Include="..\EdmxDesignHost\Entities.edmx" Link="Model\Entities.edmx" />
    <Compile Include="..\EdmxDesignHost\Entities.Context.cs" Link="Model\Entities.Context.cs" />
    <Compile Include="..\EdmxDesignHost\Thing.cs" Link="Model\Thing.cs" />
    <Compile Include="..\EdmxDesignHost\Person.cs" Link="Model\Person.cs" />
  </ItemGroup>
```
Note that the EDMX file is linked with the `EntityDeploy` build action. This is a special MSBuild task (now included in the EF 6.3 package) that takes care of adding the EF model into the target assembly as embedded resources (or copying it as files in the output folder, depending on the setting on the Metadata Artifact Processing setting in the EDMX). For more details on how to get this set up, see our [EDMX .NET Core sample](https://aka.ms/EdmxDotNetCoreSample).

You can choose to copy the files instead of linking them, but keep in mind that due to a bug in current builds of Visual Studio, copying the files from the .NET Framework project to the .NET Core project within Solution Explorer may cause hangs, so it is better to copy the files from the command line.

### Feedback requested: Should we build a dotnet ef6 tool?

We are also seeking feedback and possible contributions to enable a cross-platform command line experience for migrations commands, similar to `dotnet ef` but for EF6 (tracked in issue [#1053](https://github.com/aspnet/EntityFramework6/issues/1053)). If you would like to see this happen, or if you would like to contribute to it, please vote or comment on the issue.

## Weekly status updates

If you'd like to track our progress more closely, we now publish weekly status updates to [GitHub](https://github.com/aspnet/EntityFrameworkCore/issues/15403). We also post these status updates to our Twitter account, [@efmagicunicorns](https://twitter.com/efmagicunicorns).  

## Thank you

Once more, thank you for trying our preview bits, and for all the bug reports and contributions that will help make EF Core 3.0 and EF 6.3 much better releases.
