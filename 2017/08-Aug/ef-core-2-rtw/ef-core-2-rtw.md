# Announcing Entity Framework Core 2.0

Today we are releasing the final version of Entity Framework Core 2.0, alongside [.NET Core 2.0](https://aka.ms/dotnetcore2announce) and [ASP.NET Core 2.0](https://aka.ms/aspnetcore2announce).

Entity Framework (EF) Core is the lightweight, extensible, and cross-platform version of Entity Framework, the popular Object/Relational Mapping (O/RM) framework for .NET.

## Getting the bits

You can start using EF Core 2.0 today by installing an EF Core 2.0-compatible database provider NuGet package in your applications. E.g. to install the SQL Server provider from the command line in a .NET Core 2.0 application:

```console
$ dotnet add package Microsoft.EntityFrameworkCore.SqlServer -V 2.0.0
```

Or from the Package Manager Console in Visual Studio 2017:

```console
PM> Install-Package Microsoft.EntityFrameworkCore.SqlServer -Version 2.0.0
```

Note that the SQL Server, SQLite, and in-memory database providers for EF Core 2.0 are already included in the ASP.NET Core 2.0 meta-package. Therefore if you are creating an ASP.NET Core 2.0 application, these steps won't be necessary.

Check [our documentation](https://docs.microsoft.com/ef/core/get-started/) for more detailed installation and upgrade instructions as well as tutorials on using EF Core with different kinds of applications.

## What is new in this version

Here are some of the most salient new features in EF Core 2.0:

### .NET Standard 2.0

EF Core now targets the new [.NET Standard 2.0](https://aka.ms/netstandard2announce). The latter defines a shared surface area of over 32,000 APIs that works across .NET Framework, .NET Core, Mono, Xamarin and soon, the Universal Windows Platform. With .NET Standard 2.0, developers can reuse their code and skills on a wide range of platforms, application types and devices.

See our [platform support documentation](https://docs.microsoft.com/ef/core/platforms) for detailed guidance on using EF Core 2.0 on each platform.

### Improved LINQ translation

Queries are more efficient in EF Core 2.0 in multiple scenarios. As an example, we increased the number of patterns that can be translated to SQL, so many queries that triggered client-side evaluation in previous versions will no longer do it in 2.0.

### Like query operator

You can now use `EF.Functions.Like()` in a LINQ query and it will be translated to LIKE in SQL or evaluated in memory if necessary. E.g. the following query:

```csharp
var customers =
    from c in context.Customers
    where EF.Functions.Like(c.Name, "a%");
    select c;
```
Is translated like this:

```sql
SELECT [c].[Id], [c].[Name]
FROM [Customers] AS [c]
WHERE [c].[Name] LIKE N'a%';
```

### Owned entities and Table Splitting

You can now define "owned" or "child" entities which group properties within other entities, very similar to how complex types used to work in EF6, but with the ability to contain reference navigation properties. In combination with table splitting, owned types allow these two entities to be automatically mapped to a single `Customer` table:

```csharp
public class Customer
{
    public int Id { get; set; }
    public string Name {get; set;}
    public PhysicalAddress Address { get; set; }
}

public class PhysicalAddress
{
    public string StreetAddress { get; set; }
    public Location Location { get; set; }
}

...

modelBuilder.Entity<Customer>()
    .OwnsOne(c => c.Address);
```

### Global query filters

You can now specify filters in the model that are applied automatically to all entities of a type in all queries executed on the `DbContext`. E.g. given this code in `OnModelCreating`:

```csharp
modelBuilder.Entity<Post>()
    .HasQueryFilter(p => !p.IsDeleted);
```

This query will only return posts that are not marked as deleted:

```csharp
var blog = context.Blogs
    .Include(b => b.Posts)
    .FirstOrDefault(b => b.Id == id);
```

### DbContext Pooling

Many ASP.NET Core applications can now obtain a performance boost by configuring the service registration of their `DbContext` types to use a pool of pre-created instances, avoiding the cost of creating new instance for every request:

```csharp
services.AddDbContextPool<BloggingContext>(
    options => options.UseSqlServer(connectionString));
```

### String interpolation in raw SQL methods

The following SQL query using C# string interpolation syntax now gets correctly parameterized:

```csharp
var city = "Redmond";

using (var context = CreateContext())
{
    context.Customers.FromSql($@"
        SELECT *
        FROM Customers
        WHERE City = {city}");
}
```

This will create a parameter `@p0` with a value of `'Redmond'` and SQL that looks like this:

```sql
SELECT *
FROM Customers
WHERE City = @p0
```

### And more

We have added a few more features such as **explicitly compiled queries**, **self-contained entity configurations in code first** and **database scalar function mapping** (thanks to [Paul Middleton](https://github.com/pmiddleton) for a great contribution!), and we have fixed lots of bugs.

Please check the overview of the [new features](https://docs.microsoft.com/ef/core/what-is-new/) in our documentation.

## Next steps

We are already hard at work on the [next version of EF Core](https://github.com/aspnet/EntityFramework/issues?q=is%3Aopen+is%3Aissue+milestone%3A2.1.0) and also [finishing up EF 6.2](https://github.com/aspnet/EntityFramework6/issues?q=is%3Aopen+is%3Aissue+milestone%3A6.2.0)

As always, we welcome your contributions!

## Many Thanks

We would like to take the chance to reiterate our gratitude to everyone contributing to our project, in particular, to external contributors who made code submissions to EF Core in 2.0. By their GitHub aliases: [@BladeWise](https://github.com/BladeWise), [@ErikEJ](https://github.com/ErikEJ), [@fitzchak](https://github.com/fitzchak), [@IvanKishchenko](https://github.com/IvanKishchenko), [@laskoviymishka](https://github.com/laskoviymishka), [@lecaillon](https://github.com/lecaillon), [@MicahZoltu](https://github.com/MicahZoltu), [@multiarc](https://github.com/multiarc), [@NickCraver](https://github.com/NickCraver), [@pmiddleton](https://github.com/pmiddleton), [@roji](https://github.com/roji), [@rpawlaszek](https://github.com/rpawlaszek), [@searus](https://github.com/searus), [@tinchou](https://github.com/tinchou), [@tuespetre](https://github.com/tuespetre).
