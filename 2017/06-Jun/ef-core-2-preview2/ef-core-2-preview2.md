# Announcing EF Core 2.0 Preview 2

Today we are making Entity Framework Core 2.0 Preview 2 available.

Entity Framework Core (EF Core) is a lightweight, extensible, and cross-platform version of Entity Framework. EF Core follows the same release cycle as .NET Core but can be used in multiple .NET platforms, including .NET Core 2.0 and .NET Framework 4.6.1 or newer.

## Installing or upgrading to 2.0 Preview 2

### .NET Standard 2.0 requisite

From Preview 2 and beyond, EF Core 2.0 targets .NET Standard 2.0 and therefore requires .NET platforms that support it:

- If the application targets .NET Core, make sure that you install [.NET Core SDK 2.0.0 Preview 2](https://www.microsoft.com/net/core/preview)
- The minimum version of .NET Framework supported is 4.6.1
- For UWP applications this means that they cannot use EF Core 2.0 until a new release of UWP with support for .NET Standard 2.0 is released later this year. Existing UWP applications should continue to target EF Core 1.
- Mono and Xamarin versions with official support for .NET Standard 2.0 will also be available at a later date. In the meanwhile we encourage [creating new issues](https://github.com/aspnet/entityframework/issues/new) for anything that doesn't work as expected

See [our announcement](https://github.com/aspnet/Announcements/issues/246) for more details about the decision to target .NET Standard 2.0.

### Installing new runtime packages

In general, it should be possible for to use Preview 2 by installing a 2.0 Preview 2-compatible version of an EF Core data provider.

E.g. to install or upgrade the SQL Server provider from the command line in a .NET Core application for cross-platform development:

``` console
dotnet add package Microsoft.EntityFrameworkCore.SqlServer -v 2.0.0-preview2-final
```

For any type of application using Visual Studio's Package Manager Console:

``` console
PM> install-package Microsoft.EntityFrameworkCore.SqlServer -Version 2.0.0-preview2-final
```

Or to upgrade:

``` console
PM> update-package Microsoft.EntityFrameworkCore.SqlServer -Version 2.0.0-preview2-final
```

If you are using a third party database provider, then check to see if they have released an update that is compatible with 2.0.0-preview2-final. If they have, then just upgrade to the new version. If not, then you will not be able to upgrade since version 2.0 Preview 2 contains several breaking changes and existing providers are not expected to work with it.

Applications targeting ASP.NET Core 2.0 Preview 2 can use EF Core 2.0 Preview 2 without additional dependencies besides third party data providers.

Existing ASP.NET Core applications need to be upgrade to ASP.NET Core 2.0 Preview 2.

Any references to older EF Core packages may need to be removed manually.

### Installing new tooling packages

If application targets .NET Core, we recommend following the instructions to install any pre-requisite updates to the development tools for the platform of your choice in the [.NET Core 2.0 Preview 2 announcement post](https://blogs.msdn.microsoft.com/dotnet/2017/06/28/announcing-net-core-2-0-preview-2/)

If your project references any of the tooling and design packages, then be sure to upgrade those too, e.g. in order to use the `dotnet ef` command line tools, your application's CSPROJ file should contain the following:

``` xml
<ItemGroup>
  <DotNetCliToolReference
      Include="Microsoft.EntityFrameworkCore.Tools.DotNet"
      Version="2.0.0-preview2-final" />
</ItemGroup>
```

The Package Manager Console EF Core commands can be upgraded by issuing the following command:

``` console
PM> update-package Microsoft.EntityFrameworkCore.Tools -Prerelease -Version 2.0.0-preview2-final
```

## What is new in 2.0 Preview 2

Besides the improvements already contained in 2.0 Preview 1 and described in [our previous announcement](https://blogs.msdn.microsoft.com/dotnet/2017/05/12/announcing-ef-core-2-0-preview-1/), here are details of some of the salient new features in Preview 2:

### String interpolation in FromSql and ExecuteSqlCommand

Preview 2 adds a feature that leverages C# 6.0 interpolated strings to build SQL at runtime in a way that helps avoid common mistakes that can lead to SQL injection. Here is an example:

``` csharp
var city = "London";
var contactTitle = "Sales Representative";

using (var context = CreateContext())
{
    context.Customers
       .FromSql($@"
           SELECT *
           FROM Customers
           WHERE City = {city}
               AND ContactTitle = {contactTitle}")
       .ToArray();
}
```
The query contains two variables embedded in the SQL format string. EF Core will produce the following SQL:

```sql
@p0='London' (Size = 4000)
@p1='Sales Representative' (Size = 4000)

SELECT *
FROM Customers
WHERE City = @p0
    AND ContactTitle = @p1
```

### Automatic table splitting for owned entity types

In Preview 1 we already announced owned entity types as a way to create types that share the identity of their owner entity. In Preview 2 we have completed and refined the feature so that by convention owned types will share the table with their owner. E.g. the following model only one table is created:

``` csharp
modelBuilder.Entity<Order>().OwnsOne(
    p => p.OrderDetails,
    cb =>
    {
        cb.OwnsOne(c => c.BillingAddress);
        cb.OwnsOne(c => c.ShippingAddress);
    });

public class Order
{
    public int Id { get; set; }
    public OrderDetails OrderDetails { get; set; }
}

public class OrderDetails
{
    public Address BillingAddress { get; set; }
    public Address ShippingAddress { get; set; }
}

public class Address
{
    public string Street { get; set; }
    public string City { get; set; }
}
```

### Database scalar function mapping
Preview 2 includes an important contribution from [Paul Middleton](https://github.com/pmiddleton) which enables mapping database scalar functions to method stubs so that they can be used in LINQ queries and translated to SQL.

The API is still in flux after Preview 2 but here is a brief description of how the feature can be used:

Declare a static method on your `DbContext` and annotate it with `DbFunctionAttribute`:

``` csharp
public class BloggingContext : DbContext
{
    [DbFunction]
    public static int PostReadCount(int blogId)
    {
        throw new Exception();
    }
}
```

Methods like this are automatically registered. Once a method has been registered you can use it anywhere in your queries:

``` csharp
var query =
    from p in context.Posts
    where BloggingContext.PostReadCount(p.Id) > 5
    select p;
```

A few things to note:

- By convention the name of the method is used as the name of a function (in this case a user defined function) when generating the SQL, but you can override the name and schema during method registration
- Currently only scalar functions are supported
- You must create the mapped function in the database, e.g. EF Core migrations will not take care of creating it

## Breaking changes in 2.0

We have taken the opportunity to significantly refine our existing API and behaviors in 2.0. There are a few improvements that can require modifying existing application code, although we believe that for the majority of applications the impact will be low, in most cases requiring just recompilation and minimal guided changes to replace obsolete APIs.

While we are putting together a more complete announcement of the breaking changes, we wanted to highlight some of the important types of changes here:

### Provider and extensions compatibility
__EF Core 2.0 will not be compatible with existing providers.__ Unlike the standard application development API, the impact on providers surface will be significant and we are happy to work with provider writers to help them through the transition. The best way to contact us if you are working on moving an existing provider and need help, is to [create a new issue](https://github.com/aspnet/entityframework/issues/new) on our GitHub repository.

There are also changes in advanced API areas such as metadata and service interfaces, which might affect libraries that extend EF Core. Once again, we are happy to work through the transition with extension authors.

### Binary compatibility
__EF Core 2.0 won't be binary compatible with EF Core 1__, which means that all code that depends on EF Core needs to at least be recompiled to work with 2.0.

In fact, many changes we made break existing compiled code but don't require changes to source code, just recompilation. A canonical example of this is when we updated of our `Include` APIs to support collection navigation properties that are of type `IEnumerable<T>` rather than `ICollection<T>` in 1.1. We realized such  binary breaking changes was not acceptable in a point release so we brought back binary compatible methods in 1.1.1. But now that 2.0 has new target requirements it requires libraries and application code to be recompiled anyway, and the old version of the API based on `ICollection<T>` has been removed.

Other examples are types we moved to more commonly used namespaces, e.g. the `DeleteBehavior` enum was moved to the main  `Microsoft.EntityFrameworkCore` namespace.

### Obsolete APIs
Examples of these are the `UseInMemoryDatabase` overload that does not take a name for the database, and the replacement of `IDbContextFactory` with `IDesignTimeDbContextFactory` in a different namespace.

While APIs marked as obsolete can be removed in future releases, and their usage will cause compile time warnings, they will continue to work for this release.

### Obsolete packages
As part of the refactoring of our provider model, in Preview 2 we have merged the functionality previously contained in provider's design-time packages into the main (and now only) provider package. E.g. there won't be any more updates of `Microsoft.EntityFrameworkCore.SqlServer.Design` and any reference to it should be removed.

### Behavioral changes
An example of this is the adjustments to support the new patterns for design-time discovery of `DbContext` types for ASP.NET Core 2.0 applications based on `Program.BuildWebHost()` and the removal of the support for the 1.x pattern based only on `Startup.ConfigureServices()`.

Another good example is that starting with Preview 2 we throw an exception any time we find an unnecessary call to `Include()` in a query.

E.g. the following query now throws, since `Include()` is not necessary to make the `Where` clause work, and we cannot use it to load `Product` instances alongside the results, because the `Select` clause changed the results so that the query no longer returns instances of `Order`:

``` csharp
var pids = context.Orders
    .Include(o => o.Product)
    .Where(o => o.Product.Name == "Baked Beans")
    .Select(o =>o.ProductId)
    .ToList();
```
The exception can be avoided by either removing the unnecessary `Include()` call or by configuring this particular case to log a warning instead of causing an error in either `OnConfiguring` or the `AddDbContext` call:

``` csharp
optionsBuilder.ConfigureWarnings(
    w => w.Log(CoreEventId.IncludeIgnoredWarning));
```

## What is next after Preview 2

At this point in the release cycle, we are focusing most of our time an effort on fixing bugs and making only a few final API adjustments. We encourage you to continue providing feedback, in particular about your experience using the new features. Once more, the best way to give us feedback is to [create a new issue](https://github.com/aspnet/entityframework/issues/new) in out GitHub repository.

## Thank you!

We want to take the opportunity to thank all the members of the .NET developer community who with their feedback are helping make EF Core 2.0 a better in release, in particular those who made code contributions in Preview 2:
Paul Middleton ([@pmiddleton](https://github.com/pmiddleton)) and Erik Ejlskov Jensen ([@ErikEJ](https://github.com/ErikEJ)).
