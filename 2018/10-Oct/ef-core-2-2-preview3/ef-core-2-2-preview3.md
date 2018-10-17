# Announcing Entity Framework Core 2.2 Preview 3

Today we are making EF Core 2.2 Preview 3 available, together with a new preview of our data provider for Cosmos DB and updated spatial extensions for various providers.

Preview 3 is going to be the last milestone before EF Core 2.2 RTM, so now is your last chance to try the bits and give us feedback if you want to have an impact on the quality and the shape of the APIs in this release.

Besides the new features, you can help by trying EF Core 2.2 preview 3 on applications that are using third party providers. Although we now have our own testing for this, there might be unforeseen compatibility problems, and the earlier we can detect them, the higher chances we have of addressing them before RTM.

We thank you in advance for reporting any issues your find on [our issue tracker on GitHub](https://github.com/aspnet/EntityFrameworkCore/issues/new).

## EF Core 2.2 roadmap update

EF Core 2.2 RTM is still planned to release by the end of the 2018 calendar year, alongside ASP.NET Core 2.2 and .NET Core 2.2.

However, based on a reassessment of the progress we have made so far, and on new information about the work we need to complete 2.2, we are no longer trying to include the following features in the EF Core 2.2 RTM:

- **Reverse engineering database views into query types:** [This feature](https://github.com/aspnet/EntityFrameworkCore/issues/1679) is postponed to EF Core 3.0.

- **Cosmos DB Provider:** Although we have made a lot of progress setting up the required infrastructure for document-oriented database support in EF Core, and have been steadily adding functionality to the provider, realistically we cannot arrive to a state in which we can release the provider with adequate functionality and quality in the current timeframe for 2.2.

  Overall, we have found that the work necessary to complete the provider to be more than we initially estimated. Also, ongoing evolution in Cosmos DB is leading us to frequently revisit decisions about such things as how we use the Cosmos DB SDK, whether we map all entities to a single collection by default, etc.

  We plan to maintain the focus on the provider and to continue working with the Cosmos DB team and to keep releasing previews of the provider regularly. You can expect at least one more preview by the end of this year, and RTM in 2019. We haven't decided yet if the Cosmos DB provider will release as part of EF Core 3.0 or earlier.

  A good way to keep track of our progress is [this checklist in our issue tracker](https://github.com/aspnet/EntityFrameworkCore/issues/12086).

## Obtaining the preview

The preview bits are available [on NuGet](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/2.2.0-preview3-35497), and also as part of [ASP.NET Core 2.2 Preview 3](https://blogs.msdn.microsoft.com/webdev/2018/10/17/asp-net-core-2-2-0-preview3-now-available/) and the [.NET Core SDK 2.2 Preview 3 (to-do)](https://blogs.msdn.microsoft.com/dotnet/2018/10/17/announcing-net-core-2-2-preview-3/), also releasing today. If you are want to try the preview in an application based on ASP.NET Core, we recommend you upgrade to ASP.NET Core 2.2 Preview 3.

The SQL Server and the in-memory providers are also included in ASP.NET Core, but for other providers and any other type of application, you will need to install the corresponding NuGet package.

For example, to add the 2.2 Preview 3 version of the SQL Server provider in a .NET Core library or application from the command line, use:

``` console
$ dotnet add package Microsoft.EntityFrameworkCore.SqlServer -v 2.2.0-preview3-35497
```

Or from the Package Manager Console in Visual Studio:

``` console
PM> Install-Package Microsoft.EntityFrameworkCore.SqlServer -Version 2.2.0-preview3-35497
```

For more details on how to add EF Core to your projects see [our documentation on Installing Entity Framework Core](https://docs.microsoft.com/ef/core/get-started/install/).

The Cosmos DB provider and the spatial extensions ship as new separate NuGet packages. We'll explain how to get started with them in the corresponding feature descriptions.

## What is new in this preview?

Around [69 issues have been fixed](https://github.com/aspnet/EntityFrameworkCore/issues?q=is%3Aissue+milestone%3A2.2.0-preview3+is%3Aclosed+label%3Aclosed-fixed) between since we finished [Preview 2 last month](https://blogs.msdn.microsoft.com/dotnet/2018/09/12/announcing-entity-framework-core-2-2-preview-2/). This includes product bug fixes and improvements to the new features. Specifically about the new features, the most significant changes are:

### Spatial extensions
* We have enabled spatial extensions to work with the SQLite provider using the popular [SpatiaLite](https://www.gaia-gis.it/fossil/libspatialite/index) library.
* We switched the default mapping of spatial properties on SQL Server from geometry to geography columns.
* In order to use spatial extensions correctly with preview 3, it is recommended that you use the GeometryFactory provided by NetTopologySuite instead of creating new instances directly.
* We collaborated with the NetTopologySuite team to create NetTopologySuite.IO.SqlServerBytes — a new IO module that targets .NET Standard and works directly with the SQL Server serialization format.
* We enabled reverse engineering for databases containing spatial columns. Just make sure you add the spatial extension package for your database provider before you run `Scaffold-DbContext` or `dotnet ef dbcontext scaffold`.

Here is an updated usage example:

``` csharp
// Model class
public class Friend
{
  [Key]
  public string Name { get; set; }

  [Required]
  public IPoint Location { get; set; }
}
```

``` csharp
// Program
private static void Main(string[] args)
{
     // Create spatial factory
     var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

     // Setup data in datbase
     using (var context = new MyDbContext())
     {
         context.Database.EnsureDeleted();
         context.Database.EnsureCreated();

         context.Add(
             new Friend
             {
                 Name = "Bill",
                 Location = geometryFactory.CreatePoint(new Coordinate(-122.34877, 47.6233355))
             });
         context.Add(
             new Friend
             {
                 Name = "Paul",
                 Location = geometryFactory.CreatePoint(new Coordinate(-122.3308366, 47.5978429))
             });
         context.SaveChanges();
     }

     // find nearest friends
     using (var context = new MyDbContext())
     {
         var myLocation = geometryFactory.CreatePoint(new Coordinate(-122.13345, 47.6418066));

         var nearestFriends =
             (from f in context.Friends
              orderby f.Location.Distance(myLocation) descending
              select f).Take(5);

         Console.WriteLine("Your nearest friends are:");
         foreach (var friend in nearestFriends)
         {
             Console.WriteLine($"Name: {friend.Name}.");
         }
     }
}
```

In order to use this code with SQL Server, simply install the 2.2 preview 3 version of the `Microsoft.EntityFrameworkCore.SqlServer.NetTopologySuite` NuGet package, and configure your DbContext as follows:

``` csharp
public class MyDbContext : DbContext
{
    public DbSet<Friend> Friends { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlServer(
            "Server=(localdb)\\mssqllocaldb;Database=SpatialFriends;ConnectRetryCount=0",
            b => b.UseNetTopologySuite());

    }
}
```

In order to use this code with SQLite, you can install the 2.2 preview 3 version of the  `Microsoft.EntityFrameworkCore.Sqlite.NetTopologySuite`and `Microsoft.EntityFrameworkCore.Sqlite` packages. Then you can configure the DbContext like this:

``` csharp
public class MyDbContext : DbContext
{
    public DbSet<Friend> Friends { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite(
            "Filename=SpatialFriends.db",
            x => x.UseNetTopologySuite());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // For SQLite, you need to configure reference system on column
        modelBuilder
            .Entity<Friend>()
            .Property(f => f.Location)
            .ForSqliteHasSrid(4326);
    }
}
```

Note that the spatial extension for SQLite requires the SpatiaLite library. This will be added as a dependency by the NuGet packages previously mentioned if you are on Windows, but on other systems you will need extra steps. For example:

* On MacOS: `brew install libspatialite`
* On Ubuntu or Debian Linux: `apt-get install libsqlite3-mod-spatialite`

In order to use this code with the in-memory provider, simply install the `NetTopologySuite` package, and the 2.2 preview 3 version of the `Microsoft.EntityFrameworkCore.InMemory` package. Then you can simply configure the DbContext like this:

``` csharp
public class MyDbContext : DbContext
{
    public DbSet<Friend> Friends { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseInMemoryDatabase("SpatialFriends");
    }
}
```

### Cosmos DB provider
We have made several changes and improvements since preview 2:
* The package name has been renamed to Microsoft.EntityFrameworkCore.Cosmos
* The `UseCosmosSql()` method has been renamed to `UseCosmos()`
* We now store owned entity references and collections in the same document as the owner
* Queries can now be executed asynchronously
* `SaveChanges()`, `EnsureCreated()`, and `EnsureDeleted()` can now be executed synchronously
* You no longer need to manually generate unique key values for entities
* We preserve values in non-mapped properties when we update documents
* We added a `ToContainer()` API to map entity types to a Cosmos DB container (or collection) explicitly
* We now use the name of the derived DbContext type, rather 'Unicorn' for the container or collection name we use by convention
* We enabled various existing features to work with Cosmos DB, including retrying execution strategies, and data seeding

We still have some pending work and several limitations to remove in the provider. Most of them as tracked as uncompleted tasks [on our task list](https://github.com/aspnet/EntityFrameworkCore/issues/12086). In addition to those:

* Currently, synchronous methods are much slower than the corresponding asynchronous methods
* The value of the 'id' property has to be specified for seeding
* There is currently no enforcement of uniqueness of primary keys values on entities saved by multiple instances of the DbContext

In order to use the provider, install the 2.2 preview 3 version of the `Microsoft.EntityFrameworkCore.Cosmos` package.

The following example configures the DbContext to connect to the [Cosmos DB local emulator](https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator) to store a simple blogging model:

``` csharp
public class BloggingContext : DbContext
{
  public DbSet<Blog> Blogs { get; set; }
  public DbSet<Post> Posts { get; set; }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    optionsBuilder.UseCosmos(
      "https://localhost:8081",
      "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
      "MyDocuments");
  }
}

public class Blog
{
  public int BlogId { get; set; }
  public string Name { get; set; }
  public string Url { get; set; }
  public List<Post> Posts { get; set; }
}

public class Post
{
  public int PostId { get; set; }
  public string Title { get; set; }
  public string Content { get; set; }
  public List<Tag> Tags { get; set; }
}

[Owned]
public class Tag
{
    [Key]
    public string Name { get; set; }
}

```

If you want, you can create the database programmatically, using EF Core APIs:

``` csharp
using (var context = new BloggingContext())
{
  context.Database.EnsureCreated();
}
```

Once you have connected to an existing database and you have defined your entities, you can start storing data in the database, for example:

``` csharp
using (var context = new BloggingContext())
{
  context.Blogs.Add(
    new Blog
    {
        BlogId = 1,
        Name = ".NET Blog",
        Url = "https://blogs.msdn.microsoft.com/dotnet/",
        Posts = new List<Post>
        {
            new Post
            {
                PostId = 2,
                Title = "Welcome to this blog!",
                Tags = new List<Tag>
                {
                    new Tag
                    {
                        Name = "Entity Framework Core"
                    },
                    new Tag
                    {
                        Name = ".NET Core"
                    }
                }
            },
        }
      }
    });
  context.SaveChanges();
}
```

And you can write queries using LINQ:

``` csharp
var dotNetBlog = context.Blogs.Single(b => b.Name == ".NET Blog");
```

### Query tags
* We fixed several issues with multiple calls of the API and with usage with multi-line strings
* The API was renamed to `TagWith()`

This an updated usage example:

    ``` csharp
    var nearestFriends =
        (from f in context.Friends.TagWith(@"This is my spatial query!")
        orderby f.Location.Distance(myLocation) descending
        select f).Take(5).ToList();
    ```
    This will generate the following SQL output:

    ``` sql
    -- This is my spatial query!

    SELECT TOP(@__p_1) [f].[Name], [f].[Location]
    FROM [Friends] AS [f]
    ORDER BY [f].[Location].STDistance(@__myLocation_0) DESC
    ```

### Collections of owned entities

* The main update since preview 2 is that the Cosmos DB provider now stores owned collections as part of the same document as the owner.

Here is a simple usage scenario:

``` csharp
modelBuilder.Entity<Customer>().OwnsMany(c => c.Addresses);
```
## Thank you

The EF team would like to thank everyone for the feedback and contributions. Once more, please try this preview and report any feedback on [our issue tracker](https://github.com/aspnet/EntityFrameworkCore/issues/new).
