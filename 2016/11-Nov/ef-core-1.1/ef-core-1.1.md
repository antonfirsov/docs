Entity Framework Core (EF Core) is a lightweight, extensible, and cross-platform version of Entity Framework. Today we are making Entity Framework Core 1.1 available.

EF Core follows the same release cycle as .NET Core. Continuous improvements every 2 months and new features released every 6 months. This is the first feature release since 1.0.

Be sure to read the Upgrading to 1.1 section, at the end of this post, for important information about upgrading to the 1.1 release.


## What's in 1.1

The 1.1 release is focused on addressing issues that prevent folks from adopting EF Core. This includes fixing bugs (there are over 100 bug fixes included in the 1.1 release) and adding some of the critical features that are not yet implemented in EF Core. While we've made some good progress on this, we do want to acknowledge that EF Core still isn't going to be the right choice for everyone, for more detailed info of what is implemented see our [EF Core and EF6.x comparison](https://docs.microsoft.com/en-us/ef/efcore-and-ef6/index).


### Improved LINQ translation

In the 1.1 release we have made good progress improving the EF Core LINQ provider. This enables more queries to successfully execute, with more logic being evaluated in the database (rather than in memory).

### DbSet.Find

`DbSet.Find(...)` is an API that is present in EF6.x and has been one of the more common requests for EF Core. It allows you to easily query for an entity based on its primary key value. If the entity is already loaded into the context, then it is returned without querying the database.

```c#
using (var db = new BloggingContext())
{
    var blog = db.Blogs.Find(1);
}
```

### Mapping to fields

The new `HasField(...)` method in the fluent API allows you to configure a backing field for a property. This can be useful for read-only properties, or data that has Get/Set methods rather than a property. For detailed guidance, see the [Backing Fields](https://docs.microsoft.com/en-us/ef/core/modeling/backing-field) article in our documentation.

```c#
public class BloggingContext : DbContext
{
    ...

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog>()
            .Property(b => b.Url)
            .HasField("_validatedUrl");
    }
}
```

### Explicit Loading

Explicit loading allows you to load the contents of a navigation property for an entity that is tracked by the context. For more information, see the [Loading Related Data](https://docs.microsoft.com/en-us/ef/core/querying/related-data#eager-loading) article in our documentation.

```c#
using (var db = new BloggingContext())
{
    var blog = db.Blogs.Find(1);

    db.Entry(blog).Collection(b => b.Posts).Load();
    db.Entry(blog).Reference(b => b.Author).Load();
}
```

### Additional EntityEntry APIs from EF6.x

We've added the remaining `EntityEntry` APIs that were available in EF6.x. This includes `Reload()`, `GetModifiedProperties()`, `GetDatabaseValues()` etc. These APIs are most commonly accessed by calling the `DbContext.Entry(object entity)` method.

### Connection resiliency

Connection resiliency automatically retries failed database commands. The SQL Server provider includes a execution strategy that is specifically tailored to SQL Server (including SQL Azure). It is aware of the exception types that can be retried and has sensible defaults for maximum retries, delay between retries, etc. For more information, see the [Connection Resiliency](https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency) article in our documentation.

An execution strategy is specified when configuring the options for your context. This is typically in the `OnConfiguring` method of your derived context, or in `Startup.cs` for an ASP.NET Core application.

```c#
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    optionsBuilder
        .UseSqlServer(
            "<connection string>",
            options => options.EnableRetryOnFailure());
}
```

### SQL Server memory-optimized table support

[Memory-Optimized Tables](https://msdn.microsoft.com/en-us/library/dn133165.aspx) are a feature of SQL Server. You can now specify that the table an entity is mapped to is memory-optimized. For more information, see the [Memory-Optimized Tables](https://docs.microsoft.com/en-us/ef/core/providers/sql-server/memory-optimized-tables) article in our documentation.

```c#
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Blog>()
        .ForSqlServerIsMemoryOptimized();
}
```

### Simplified service replacement

In EF Core 1.0 it is possible to replace internal services that EF uses, but this is complicated and requires you to take control of the dependency injection container that EF uses. In 1.1 we have made this much simpler, with a `ReplaceService(...)` method that can be used when configuring the context. This is typically in the `OnConfiguring` method of your derived context, or in `Startup.cs` for an ASP.NET Core application.

```c#
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    optionsBuilder.UseSqlServer("<connection string>");

    optionsBuilder.ReplaceService<SqlServerTypeMapper, MyCustomSqlServerTypeMapper>();
}
```


## Upgrading to 1.1

If you are using one of the database providers shipped by the EF Team (SQL Server, SQLite, and InMemory), then just upgrade your provider package.

```
PM> Update-Package Microsoft.EntityFrameworkCore.SqlServer
```

If you are using a third party database provider, then check to see if they have released an update that depends on 1.1.0. If they have, then just upgrade to the new version. If not, then you should be able to upgrade just the EF Core relational components that they depend on. Most of the new features in 1.1 do not require changes to the database provider. We've done some testing to ensure database providers that depend on 1.0 continue to work with 1.1, but this testing has not been exhaustive.

```
PM> Update-Package Microsoft.EntityFrameworkCore.Relational
```

### Upgrading tooling packages

If you are using the tooling package, then be sure to upgrade that too. Note that tooling is versioned as 1.0.0-preview4 because tooling has not reached its initial stable release (this is true of tooling across .NET Core, ASP.NET Core, and EF Core).

```
PM> Update-Package Microsoft.EntityFrameworkCore.Tools -Pre
```

If you are using ASP.NET Core and the `dotnet ef` commands, then you need to update the tools section of project.json to use the new `Microsoft.EntityFrameworkCore.Tools.DotNet` package in place of the `Microsoft.EntityFrameworkCore.Tools` package from 1.0. As the design of .NET CLI Tools has progressed it has become necessary for us to separate the `dotnet ef` tools into this separate package.

```json
"tools": {
   "Microsoft.EntityFrameworkCore.Tools.DotNet": "1.1.0-preview4"
 },
```
