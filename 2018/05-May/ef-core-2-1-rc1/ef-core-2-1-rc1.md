# Announcing Entity Framework Core 2.1 Release Candidate 1
Today, we are excited to announce that the first release candidate of EF Core 2.1 is available, alongside [.NET Core 2.1 RC 1](https://blogs.msdn.microsoft.com/dotnet/2018/05/07/announcing-net-core-2-1-rc-1/) and [ASP.NET Core 2.1 RC 1](https://blogs.msdn.microsoft.com/webdev/2018/05/07/asp-net-core-2-1-0-rc1-now-available/), for broad testing, and now also for production use!

## Go live support

EF Core 2.1 RC1 is a "go live" release, which means once you test that your application works correctly with RC1, you can use it in production and obtain support from Microsoft, but you should still update to the final stable release once it's available.

Go live support for EF Core RC1 extends to the base functionality in EF Core and to the providers that are developed as part of the Entity Framework Core project, like the SQL Server, SQLite, and in-memory database providers. If you are using any other providers, we recommend you verify what level of support you can get from their corresponding developers.

## Changes since Preview 2

For the full details on what has changed since EF Core 2.0, look at the [What's New section](https://docs.microsoft.com/ef/core/what-is-new/ef-core-2.1) of our documentation. The main new features are:

*   _GroupBy_ translation
*   Lazy loading
*   Parameters in entity constructors
*   Value conversion
*   Query types
*   Data seeding
*   _System.Transactions_ support

We have been stabilizing the product since Preview 2, therefore there are no new features in RC1. In fact, all changes in RC1 are either bug fixes or very small functional or performance improvements on existing features.

You can get an up-to-date list of bug fixes and small enhancements using [this issue tracker query](https://github.com/aspnet/EntityFrameworkCore/issues?q=is:issue+milestone:2.1.0-rc1+label:closed-fixed). Some of the small RC1 enhancements worth mentioning are:

*   [#9295](https://github.com/aspnet/EntityFrameworkCore/issues/9295) List.Exists is now translated to SQL. Thank you [Massimiliano Donini](https://github.com/ilmax) for your contribution!
*   [#9347](https://github.com/aspnet/EntityFrameworkCore/issues/9347) Several performance improvements in model building
*   [#11570](https://github.com/aspnet/EntityFrameworkCore/issues/11570) DbContext scaffolding now generates a constructor that allows injection of _DbContextOptions<TContext>_

## Obtaining the bits

The new bits are available [in NuGet as individual packages](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/2.1.0-rc1-final), as part of the [ASP.NET Core 2.1 RC1 metapackage](https://www.nuget.org/packages/Microsoft.AspNetCore.App/2.1.0-rc1-final) and in the [.NET Core 2.1 RC1 SDK](https://www.microsoft.com/net/download/dotnet-core/sdk-2.1.300-rc1), also released today.

The recommended way to obtain the new packages for ASP.NET Core applications is through the installation of the new SDK, rather than only updating the packages. See the details in the [get started section of the ASP.NET Core announcement](https://blogs.msdn.microsoft.com/webdev/2018/05/07/asp-net-core-2-1-0-rc1-now-available/#get-started).

For other applications, either the SDK can be installed or the packages can be updated using the _dotnet_ command line tool or NuGet.

EF Core 2.1 RC1 and the corresponding versions of the SQL Server and in-memory database providers are included in the ASP.NET Core metapackage. Therefore, if your application is an ASP.NET Core application and you are using one of these providers, you don't need additional upgrade steps.

If you’re using one of the database providers developed as part of the Entity Framework Core project (for example, SQL Server, SQLite or In-Memory), you can install EF Core 2.1 RC1 bits by simply installing the latest version of a provider. For example, using _dotnet_ on the command-line:

``` console
$ dotnet add package Microsoft.EntityFrameworkCore.Sqlite -V 2.1.0-rc1-final
```

If you’re using another EF Core 2.0-compatible relational database provider, it's recommended that in order to obtain all the newest EF Core bits, you add a direct reference to the base relational provider in your application, for example:

``` console
$ dotnet add package Microsoft.EntityFrameworkCore.Relational -V 2.1.0-rc1-final
```

**When updating packages, make sure that all EF Core packages are updated to the RC1 version. Mixing EF Core or infrastructure packages from older .NET Core versions (including previous 2.1 preview bits) will likely cause errors.**

## Provider compatibility

As we mentioned in previous announcements, some of the new features in 2.1, such as value conversions, require an updated database provider. However, it was our original goal that existing providers developed for EF Core 2.0 would be compatible with EF Core 2.1 as long as you didn't try to use the new features.

In practice, testing has shown that some of the EF Core 2.0 providers are not going to be compatible with 2.1. Also, there have been changes in the code necessary to support the new features since Preview 1 and Preview 2. Therefore, we recommend that you use a provider that has been updated for EF Core 2.1 RC1.

We have news that some of these updated providers will be available within days of this announcement. Others may take longer.

We have been working and will continue to work with provider writers to make sure we identify and address any issues with the upgrade. In the particular case of [Pomelo.EntityFrameworkCore.MySql](https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql), we are actively working with the developers to help them get it ready for 2.1.

If you experience any new incompatibility, please report it by [creating an issue in our GitHub repository](https://github.com/aspnet/EntityFrameworkCore/issues/new).

## What's next

We expect to ship the final version of EF Core 2.1 in the first half of 2018, as planned. We're now getting very close to the finish line!

In the meantime, planning for the next versions of EF Core after 2.1 is ongoing. Stay tuned for upcoming announcements in this blog.

## Thank you!

As always, the entire Entity Framework team wants to express our deep gratitude to everyone who has helped in making this release better by trying early builds, providing feedback, reporting bugs, and contributing code.

Please try EF Core 2.1 RC1, and keep posting any new feedback to [our issue tracker](https://github.com/aspnet/EntityFrameworkCore/issues/new)!
