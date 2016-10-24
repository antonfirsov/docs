Entity Framework Core (EF Core) is a lightweight, extensible, and cross-platform version of Entity Framework. Today we are making Entity Framework Core 1.1 Preview 1 available.


## Upgrading to 1.1 Preview 1

If you are using one of the database providers shipped by the EF Team (SQL Server, SQLite, and InMemory) then just upgrade your provider package.

```
PM> Update-Package Microsoft.EntityFrameworkCore.SqlServer -Pre
```

If you are using a third party database provider then check to see if they have released an update that depends on 1.1.0-preview1-final. If they have, then just upgrade to the new version. If not, then you should be able to upgrade just the EF Core relational components that they depend on. Most of the new features in 1.1 do not require changes to the database provider. We've done some testing to ensure database providers that depend on 1.0 continue to work with 1.1 Preview 1, but this testing has not been exhaustive.

```
PM> Update-Package Microsoft.EntityFrameworkCore.Relational -Pre
```

### Upgrading tooling packages

If you are using the tooling package, then be sure to upgrade that too. Note that tooling is versioned as 1.0.0-preview3-final because tooling has not reached its initial stable release (this is true of tooling across .NET Core, ASP.NET Core, and EF Core).

```
PM> Update-Package Microsoft.EntityFrameworkCore.Tools -Pre
```

If you are using ASP.NET Core then you need to update the tools section of project.json to use the new `Microsoft.EntityFrameworkCore.Tools.DotNet` package. As the design of .NET CLI Tools has progressed it has become necessary for us to separate the `dotnet ef` tools into this separate package.

```json
"tools": {
   "Microsoft.EntityFrameworkCore.Tools.DotNet": "1.0.0-preview3-final"
 },
```

## What's in 1.1 Preview 1

The 1.1 release is focused on addressing issues that prevent folks from adopting EF Core. This includes fixing bugs and adding some of the critical features that are not yet implemented in EF Core. While we've made some good progress on this, we do want to acknowledge that EF Core still isn't going to be the right choice for everyone, for more detailed info of what is implemented see our [EF Core and EF6.x comparison](https://docs.efproject.net/en/latest/efcore-vs-ef6/index.html).


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

The new `HasField(...)` method in the fluent API allows you to configure a backing field for a property. This is most commonly done when a property does not have a setter.

```c#
public class BloggingContext : DbContext
{
    ...

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog>()
            .Property(b => b.Url)
            .HasField("_theUrl");
    }
}
```

By default, EF will use the field when constructing instances of your entity during a query, or when it can't use the property (i.e. it needs to set the value but there is no property setter). You can change this via the new `UsePropertyAccessMode(...)` API.

```c#
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Blog>()
        .Property(b => b.Url)
        .HasField("_theUrl")
        .UsePropertyAccessMode(PropertyAccessMode.Field);
}
```

You can also create a property in your model that does not have a corresponding property in the entity class, but uses a field to store the data in the entity. This is different from [Shadow Properties](https://docs.efproject.net/en/latest/modeling/shadow-properties.html), where the data is stored in the change tracker. This would typically be used if the entity class uses methods to get/set values.

You can give EF the name of the field in the `Property(...)` API. If there is no property with the given name, then EF will look for a field.

```c#
public class BloggingContext : DbContext
{
    ...

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog>()
            .Property("_theUrl");
    }
}
```

You can also choose to give the property a name, other than the field name. This name is then used when creating the model, most notably it will be used for the column name that is mapped to in the database.

```c#
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Blog>()
        .Property<string>("Url")
        .HasField("_theUrl");
}

```

You can use the `EF.Property(...)` method to refer to these properties in a LINQ query.

```c#
var blogs = db.Blogs
    .OrderBy(b => EF.Property<string>(b, "Url"))
    .ToList();
```

### Explicit Loading

Explicit loading allows you to load the contents of a navigation property for an entity that is tracked by the context.

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

Connection resiliency automatically retries failed database commands. This release includes a execution strategy that is specifically tailored to SQL Server (including SQL Azure). This execution strategy is included in our SQL Server provider. It is aware of the exception types that can be retried and has sensible defaults for maximum retries, delay between retries, etc.

```c#
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    optionsBuilder
        .UseSqlServer(
            "<connection string>",
            options => options.EnableRetryOnFailure());
}
```

Other database providers may choose to add retry strategies that are tailored to their database. There is also a mechanism to register a custom execution strategy of your own.

```c#
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    optionsBuilder
        .UseMyProvider(
            "<connection string>",
            options => options.ExecutionStrategy(...));
}
```


### SQL Server memory-optimized table support

[Memory-Optimized Tables](https://msdn.microsoft.com/en-us/library/dn133165.aspx) are a feature of SQL Server. You can now specify that the table an entity is mapped to is memory-optimized. When using EF Core to create and maintain a database based on your model (either with migrations or `Database.EnsureCreated`), a memory-optimized table will be created for these entities.

```c#
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Blog>()
        .ForSqlServerIsMemoryOptimized();
}
```


### Simplified service replacement

In EF Core 1.0 it is possible to replace internal services that EF uses, but this is complicated and requires you to take control of the dependency injection container that EF uses. In 1.1 we have made this much simpler, with a `ReplaceService(...)` method that can be used when configuring the context.

```c#
public class BloggingContext : DbContext
{
    ...

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        ...

        optionsBuilder.ReplaceService<SqlServerTypeMapper, MyCustomSqlServerTypeMapper>();
    }
}
```


## What happening after 1.1 Preview 1

The stable 1.1 release will be available later this year. We aren't planning any new features between preview1 and the stable release. We will just be working on fixing critical bugs that are reported.

Our team is now turning its attention to the EF Core 1.2 and EF6.2 releases. We will share details of these releases in the near future.
