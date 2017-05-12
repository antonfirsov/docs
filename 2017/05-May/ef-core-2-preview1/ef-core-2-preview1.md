# Announcing EF Core 2.0 Preview 1

This week we made Entity Framework Core 2.0 Preview 1 available.

Entity Framework Core (EF Core) is a lightweight, extensible, and cross-platform version of Entity Framework. EF Core follows the same release cycle as .NET Core but can be used in multiple .NET platforms, including .NET Core 2.0 and .NET Framework 4.6 or newer.

## Installing or upgrading to 2.0 Preview 1

Applications based on ASP.NET Core 2.0 Preview 1 can already use EF Core 2.0 Preview 1. Also, existing ASP.NET Core applications can upgrade EF Core by upgrading to the 2.0 Preview 1 version of the ASP.NET Core meta-package and removing any references to older EF Core runtime packages. 

Other applications can upgrade by installing a 2.0 Preview 1-compatible version of the EF Core provider. E.g. to install the SQL Server provider:

```
PM> install-package Microsoft.EntityFrameworkCore.SqlServer -Prerelease -Version 2.0.0-preview1-final
```
Or to upgrade:

```
PM> update-package Microsoft.EntityFrameworkCore.SqlServer -Prerelease -Version 2.0.0-preview1-final
```

If you are using a third party database provider, then check to see if they have released an update that depends on 2.0.0-preview1-final. If they have, then just upgrade to the new version. If not, then you will not be able to upgrade since version 2.0 contains several breaking changes and 1.* providers are not expected to work with it.

### Upgrading tooling packages

If your project references any of the tooling and design packages, then be sure to upgrade those too, e.g. in order to use the `dotnet` command line tools, your application's CSPROJ file should contain the following:

``` xml
<ItemGroup>
  <DotNetCliToolReference Include="Microsoft.EntityFrameworkCore.Tools.DotNet" Version="2.0.0-preview1-final" />
</ItemGroup>
```

The Package Manager Console EF Core commands can be upgraded by issuing the following command:

```
PM> update-package Microsoft.EntityFrameworkCore.Tools -Prerelease -Version 2.0.0-preview1-final
```

> NOTE: There is a known issue that can prevent EF Core tooling and ASP.NET Scaffolding functionality from working with the default ASP.NET Core 2.0 Preview 1 project templates. The recommended workaround is to modify the following code in `Program.cs`:

Change:
``` C#
public static IWebHost BuildWebHost(string[] args) =>
    WebHost.CreateDefaultBuilder(args)
        .UseStartup<Startup>()
        .Build();
```

To:
``` C#
public static IWebHost BuildWebHost(string[] args) =>
    WebHost.CreateDefaultBuilder(args)
        .UseStartup<Startup>()
        .UseDefaultServiceProvider(options => options.ValidateScopes = false)
        .Build();
```

There are more details about this and other issue in the [list of known issues for ASP.NET Core 2.0 Preview 1 tooling](https://github.com/aspnet/Tooling/blob/master/known-issues-vs2017-preview.md).

### Visual Studio requirements

Accessing .NET Core tooling in Visual Studio requires Visual Studio 2017 15.3 Preview, Visual Studio for Mac or Visual Studio Code. More information about this and download links can be found in the [.NET Core 2.0 Preview 1 announcement](https://blogs.msdn.microsoft.com/dotnet/2017/05/10/announcing-net-core-2-0-preview-1/).

## What is new in 2.0 Preview 1

Besides new features, we have been focusing on fundamental improvements to existing functionality: we have made our LINQ implementation more robust and efficient, applied major simplifications of EF Core's provider model and its utilization of dependency injection, taken a significant number of community contributions, and fixed more than a hundred bugs reported by customers.

On the tooling side, we updated our command line and Package Manager Console tools to support new application initialization patterns used in ASP.NET Core 2.0.

From the perspective of new features, we have made some progress closing the functionality gap between EF Core and EF6 as well as implemented new capabilities only available in the Core version. For the complete set of enhancements and bug fixes included in EF Core 2.0 Preview 1, see [the release notes](https://github.com/aspnet/EntityFramework/releases/tag/rel%2F2.0.0-preview1).

Here are details of some of the salient new features:

### Improved LINQ translation

We have spent a lot of time and effort on improving our LINQ translations and query execution. There are multiple scenarios in which EF Core 2.0 is more efficient than 1.0 or 1.1, e.g. cases in which the latter would:

- Unnecessarily create nested subqueries
- Switch prematurely to client-side evaluation
- Retrieve all columns of a table when only a few were requested
- Translate a single LINQ query into N+1 queries, sometimes without appropriate filters

### EF.Functions.Like()

We have added the EF.Functions property which can be used by EF Core or providers to define methods that map to database functions or operators so that those can be invoked in LINQ queries. The first example of such a method is `Like()` and is also included in Preview 1.

``` C#
var aCustomers =
    from c in context.Customers
    where EF.Functions.Like(c.Name, "a%");
    select c;
```

Note that `Like()` comes with an in-memory implementation, which can be  handy when working against an in-memory database or when evaluation of the predicate needs to occur con the client side.

### Owned entities and table splitting

In version 2.0 we are extending EF Core to allow mapping types that do not posses their own identity and for which instances can only be tracked and referenced as dependents of instances of other entity types. This is an evolution of the way complex types work in EF6 and previous versions.

In Preview 1 the feature is still not complete: Post-Preview 1 (e.g. using our current nightly builds) for the example below, the properties of both `Customer` and `Address` are mapped by convention to columns of the same table. In the Preview 1 bits, however table splitting is not enabled and these are still mapped to separate tables:

``` C#
modelBuilder.Entity<Customer>()
    .OwnsOne(c => c.WorkAddress);

public class Customer
{
    public int CustomerId { get; set; }
    public Address WorkAddress { get; set; }
}

public class Address
{
    public string Line { get; set; }
    public string PostalOrZipCode { get; set; }
    public string StateOrProvince { get; set; }
    public string CityOrTown { get; internal set; }
}
```

### Global Query Filters

The new version also address a common need to apply "vertical filters" when querying for data of specific entity types.

These filters are defined in the EF Core model and can reference properties in entities as well as be parameterized by capturing custom members of the derived `DbContext` class:

``` C#
public class BloggingContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }

    public int TenantId {get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Post>()
            .HasQueryFilter(p => !p.IsDeleted &&
                  p.TenantId == this.TenantId );
    }
}
```

Filters are applied automatically when queries retrieve data of specific types directly as well as through navigation properties, e.g. using the `Include()` method.

Filters can also be disabled in individual LINQ queries using the `IgnoreQueryFilters()` operator.

### DbContext Pooling

The basic pattern for using EF Core in an ASP.NET Core application usually involves registering a custom `DbContext` type into the dependency injection system and later obtaining instances of that type through constructor parameters in controllers. This means a new instance of the `DbContext` is created for each requests.

In version 2.0 Preview 1 we are introducing a new way to register custom `DbContext` types in dependency injection which transparently introduces a pool of reusable `DbContext` instances:

``` C#
services.AddDbContextPool<BloggingContext>(
    options => options.UseSqlServer(connectionString));
```

If this method is used, at the time a `DbContext` instance is requested by a controller we will first check if there is an instance available in the pool. Once the request processing finalizes, any state on the instance is reset and the instance is itself returned to the pool.

This is conceptually similar to how connection pooling operates in ADO.NET providers and has the advantage of saving some of the cost of initialization of `DbContext` instance.

The new method introduces a few limitations on what can be done in the `OnConfiguring()` method of the `DbContext` but it can be adopted by many ASP.NET Core applications to obtain a performance boost.

### Manual compiled queries

Manual or explicitly compiled query APIs have been available in previous versions of EF and also in LINQ to SQL to allow applications to cache the translation of queries so that they can be computed only once and executed many times.

Although in general EF Core can automatically compile and cache queries based on a hashed representation of the query expressions, this mechanism can be used to obtain a small performance gain by bypassing the computation of the hash and the cache lookup, allowing the application to use an already compiled query through the invocation of a delegate.

```C#
private static Func<CustomerContext, int> _customerById =
    EF.CompileQuery((CustomerContext db, int id) =>
        db.Customers
            .Include(c => c.Address)
            .Single(c => c.Id == id));

...

using (var db = new CustomerContext())
{
   var customer = _customerById(db, 147);
}
```

## What is next after Preview 1

Besides addressing important feedback from customers using this preview, we will continue fixing bugs and finalizing a few remaining 2.0 features, such as the overhaul of EF Core's logging and diagnostics infrastructure and the work to integrate that into Azure Application Insights. The [roadmap for EF Core 2.0](https://github.com/aspnet/EntityFramework/wiki/Roadmap#ef-core-20) has been update to reflect current status and plans.

## Thank you!

We want to take the opportunity to thank all the members of the .NET developer community who with their feedback and code contributions are helping us make EF Core 2.0 a better release. By their GitHub handles: [@BladeWise](https://github.com/BladeWise), [@ErikEJ](https://github.com/ErikEJ),  [@fitzchak](https://github.com/fitzchak), [@IvanKishchenko](https://github.com/IvanKishchenko), [@laskoviymishka](https://github.com/laskoviymishka), [@lecaillon](https://github.com/lecaillon), [@MicahZoltu](https://github.com/MicahZoltu), [@multiarc](https://github.com/multiarc), [@NickCraver](https://github.com/NickCraver), [@pmiddleton](https://github.com/pmiddleton), [@roji](https://github.com/roji), [@rpawlaszek](https://github.com/rpawlaszek), [@searus](https://github.com/searus), [@tinchou](https://github.com/tinchou), [@tuespetre](https://github.com/tuespetre), and many more.
