# Announcing Entity Framework Core 3.0 Preview 7 and Entity Framework 6.3 Preview 7

Today, we are making new previews of [EF Core 3.0](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/3.0.0-preview7.19362.6) and [EF 6.3](https://www.nuget.org/packages/EntityFramework/) available on [nuget.org](https://www.nuget.org/).

[.NET Core 3.0 Preview 7](https://devblogs.microsoft.com/dotnet/announcing-net-core-3-0-preview-7/) and [ASP.NET Core 3.0 Preview 7](https://devblogs.microsoft.com/aspnet/asp-net-core-and-blazor-updates-in-net-core-3-0-preview-7/) were also made available today.

We encourage you to install these previews to try the new features, and to validate that all the functionality required by your applications is available and works correctly. As always, we hope you'll report any issues you find on GitHub.

## What is new in EF Core 3.0 Preview 7

### Query improvements
In this preview, we have made a lot of progress towards the completion of our new LINQ implementation. For example, the translation of GroupBy, auto-include of owned types, and query tags are now functional again. Also, for the first time in EF Core, we now support SQL translation of LINQ set operators like Union, Concat, Intersect, and Except.

You can still expect some common query functionality to be missing or broken in this preview. Examples of this are the explicitly compiled query API, and global query filters. But all the major pieces should be in place by preview 8, and after that, our focus will be on improving the quality of the release.

If you run into any issues that you don't see in this list or in the [list of bugs already tracked for 3.0](https://github.com/aspnet/EntityFrameworkCore/issues?q=is%3Aopen+is%3Aissue+milestone%3A3.0.0), please [report it on GitHub](https://github.com/aspnet/EntityFrameworkCore/issues/new).

There are some common workarounds that you might be able to use to get things working:

- Remember that by-design, [LINQ operations that cannot be translated to SQL are no longer automatically evaluated on the client](https://docs.microsoft.com/ef/core/what-is-new/ef-core-3.0/breaking-changes#linq-queries-are-no-longer-evaluated-on-the-client) in EF Core 3.0. You may need to switch explicitly to client evaluation if you need to filter data based on an expression that cannot be translated to SQL, using the `AsEnumerable()` or `ToList()` extension methods. For example, the following query will no longer work in EF Core 3.0 because one of the predicates in the where clause requires client evaluation:
   ``` csharp
   var specialCustomers = context.Customers
     .Where(c => c.Name.StartsWith(n) && IsSpecialCustomer(c));
   ```
   But if you know it is ok to perform part of the filtering on the client, you can rewrite this as:  
   ``` csharp
   var specialCustomers = context.Customers
     .Where(c => c.Name.StartsWith(n))
     .AsEnumerable() // Start using LINQ to Objects (switch to client evaluation)
     .Where(c => IsSpecialCustomer(c));
   ```

- Use the new `FromSqlRaw()` or `FromSqlInterpolated()` methods to provide your own SQL translations for anything that isn't yet supported.

- Use a nightly build (as detailed below) to verify if the feature is already fixed.

### Other major new features

Besides the improved query implementation, preview 7 includes a new API for the interception of database operations. This is very similar to the interception feature that existed in EF 6. It enables writing simple logic that is invoked automatically by EF Core whenever, for example, a database connection is opened, a transaction is committed, or a query is executed. Interceptors usually allow you to intercept operations before or after they happen. When you intercept them before they happen, you're allowed to bypass execution and supply alternate results from the interception logic.

For example, to manipulate command text, create an `IDbCommandInterceptor`:

```C#
public class MyCommandInterceptor : DbCommandInterceptor
{
    public override InterceptionResult<DbDataReader>? ReaderExecuting(
        DbCommand command, 
        CommandEventData eventData, 
        InterceptionResult<DbDataReader>? result)
    {
        // Manipulate the command text, etc. here...
        command.CommandText = command.CommandText...

        return result;
    }
}
```

and register it with your `DbContext`:

```C#
services.AddDbContext<BlogContext>(b =>
    b.UseSqlServer(connectionString)
     .AddInterceptors(new MyCommandInterceptor()));
```

### Postponed features 

At the same time, some of the features that we had originally planned for 3.0 have been postponed to future release so that we can focus on finishing what is already in flight with good quality. One significant example of this is **support for property bag entities** (issue [#13610](https://github.com/aspnet/EntityFrameworkCore/issues/13610)) and the **ability to ignore parts of the model for migrations** (issue [#2725](https://github.com/aspnet/EntityFrameworkCore/issues/2725)). 

### Getting the preview 7 runtime and tooling

EF Core 3.0 is distributed exclusively as NuGet packages. As usual, add or upgrade the runtime to preview 7 via the NuGet user interface, the Package Manager Console in Visual Studio, or via the `dotnet add package` command. In all cases, include the option to allow installing pre-release versions.

In 3.0, the `dotnet ef` CLI tool no longer ships as part of the .NET Core SDK, so before you can execute migration or scaffolding commands, it is necessary to install it as either a global tool or local tool. Due to limitations in the `dotnet` CLI tooling, installing preview tools requires specifying at least part of the preview version on the installation command, for example to install `dotnet ef` 3.0 preview as a global tool, you can run:

``` console
$ dotnet tool install --global dotnet-ef --version 3.0.0-* 
```

### Breaking changes

All breaking changes in this release are [listed in our documentation](https://docs.microsoft.com/ef/core/what-is-new/ef-core-3.0/breaking-changes) to make it easier for you to react. We keep the list up to date on every preview, with the most impactful changes near the top of the list. For example, the top most change is the fact that [LINQ operations that cannot be translated to SQL are no longer automatically evaluated on the client](https://docs.microsoft.com/ef/core/what-is-new/ef-core-3.0/breaking-changes#linq-queries-are-no-longer-evaluated-on-the-client).

### Nightly builds

Given the state of EF Core 3.0 is evolving rapidly, we recommend switching to nightly build of preview 8, for example to try fixes that aren't included in preview 7. Detailed instructions can be found [here](https://github.com/aspnet/AspNetCore/blob/master/docs/DailyBuilds.md).   

## What's new in EF 6.3 Preview 7

In this preview we have completed the majority of the work necessary for the EF 6.3 package to work on .NET Core project, and with NuGet PackageReference in all types of projects. 

In fact, there are only three main issues still tracked for the EF 6.3 timeframe:

1. **Migration commands for the NuGet Package Manager Console that works on .NET Core projects (issue [#231](https://github.com/aspnet/EntityFramework6/issues/231)):** This is now completed and available in nightly builds. It will be part of preview 8.

2. **An updated EF6 designer for Visual Studio that can work with new project files and in projects that target .NET Core (issue [#883](https://github.com/aspnet/EntityFramework6/issues/883)):** This work hasn't started yet and is planned for an upcoming update to Visual Studio 2019. In the meantime, we recommend that you work with your EDMX files inside projects that target .NET Framework and then copy the final version of your EDMX to the .NET Core project. 

3. **A cross-platform command line experience for migrations commands, similar to `dotnet ef` but for EF6 (issue [#1053](https://github.com/aspnet/EntityFramework6/issues/1053)):** This is work we are considering doing but haven't committed to it yet. If you would like to see this happen, or if you would like to contribute to it, please send us feedback [on the issue](https://github.com/aspnet/EntityFramework6/issues/1053).

If you find any unexpected issues with EF 6.3 preview 7, please report it to [our GitHub issue tracker](https://github.com/aspnet/entityframework6/issues/new).

## Weekly status updates

If you'd like to track our progress more closely, we now publish weekly status updates to [GitHub](https://github.com/aspnet/EntityFrameworkCore/issues/15403). We also post these status updates to our Twitter account, [@efmagicunicorns](https://twitter.com/efmagicunicorns).  

## Thank you

As always, thank you for trying our preview bits, and thanks to all the contributors that helped in this preview!
