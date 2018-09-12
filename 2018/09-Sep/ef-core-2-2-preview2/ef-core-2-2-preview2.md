# Announcing Entity Framework Core 2.2 Preview 2 and the preview of the Cosmos DB provider and spatial extensions for EF Core

Today we are making EF Core 2.2 Preview 2 available, together with a preview of our data provider for Cosmos DB and new spatial extensions for our SQL Server and in-memory providers.

## Obtaining the preview

The preview bits are available on NuGet, and also as part of [ASP.NET Core 2.2 Preview 2](https://blogs.msdn.microsoft.com/webdev/2018/09/12/asp-net-core-2-2-0-preview2-now-available/) and the [.NET Core SDK 2.2 Preview 2](https://www.microsoft.com/net/download/dotnet-core/2.2), also releasing today.

If you are working on an application based on ASP.NET Core, we recommend you upgrade to ASP.NET Core 2.2 Preview 2 following the instructions in the [announcement](https://blogs.msdn.microsoft.com/webdev/2018/09/12/asp-net-core-2-2-0-preview2-now-available/).

The SQL Server and the in-memory providers are included in ASP.NET Core, but for other providers and any other type of application, you will need to install the corresponding NuGet package.
For example, to add the 2.2 Preview 2 version of the SQL Server provider in a .NET Core library or application from the command line, use:

``` console
$ dotnet add package Microsoft.EntityFrameworkCore.SqlServer -v 2.2.0-preview2-35157
```

Or from the Package Manager Console in Visual Studio:

``` console
PM> Install-Package Microsoft.EntityFrameworkCore.SqlServer -Version 2.2.0-preview2-35157
```

For more details on how to add EF Core to your projects see [our documentation on Installing Entity Framework Core](https://docs.microsoft.com/ef/core/get-started/install/).

The Cosmos DB provider and the spatial extensions ship as new separate NuGet packages. We'll explain how to get started with them in the corresponding feature descriptions.

## What is new in this preview?

As we explained in [our roadmap annoucement](https://github.com/aspnet/Announcements/issues/308) back in June, there will be a large number of bug fixes (you can see the list of issues we have fixed so far [here](https://github.com/aspnet/EntityFrameworkCore/issues?q=is%3Aissue+milestone%3A2.2.0-preview2+is%3Aclosed+label%3Aclosed-fixed), but only a relatively small number of new features in EF Core 2.2.

Here are the most salient new features:

### New EF Core provider for Cosmos DB

This new provider enables developers familiar with the EF programing model to easily target [Azure Cosmos DB](https://docs.microsoft.com/en-us/azure/cosmos-db/introduction) as an application database, with all the advantages that come with it, including global distribution, elastic scalability, "always on" availability, very low latency, and automatic indexing.

The provider targets the [SQL API in Cosmos DB](https://docs.microsoft.com/en-us/azure/cosmos-db/sql-api-introduction), and can be installed in an application issuing the following command form the command line:

``` console
$ dotnet add package Microsoft.EntityFrameworkCore.Cosmos.Sql -v 2.2.0-preview2-35157
```

Or from the Package Manager Console in Visual Studio:

``` console
PM> Install-Package Microsoft.EntityFrameworkCore.Cosmos.Sql -Version 2.2.0-preview2-35157
```

To configure a DbContext to connect to Cosmos DB, you call the UseCosmosSql() extension method. For example, the following DbContext connects to a database called "MyDocuments" on the [Cosmos DB local emulator](https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator) to store a simple blogging model:

``` csharp
public class BloggingContext : DbContext
{
  public DbSet<Blog> Blogs { get; set; }
  public DbSet<Post> Posts { get; set; }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    optionsBuilder.UseCosmosSql(
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
}
```

If you want, you can create the database programmatically, using EF Core APIs:

``` csharp
using (var context = new BloggingContext())
{
  await context.Database.EnsureCreatedAsync();
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
                Title = "Welcome to this blog!"
            },
        }
      }
    });
  await context.SaveChangesAsync();
}
```

And you can write queries using LINQ:

``` csharp
var dotNetBlog = context.Blogs.Single(b => b.Name == ".NET Blog");
```

#### Current capabilities and limitations of the Cosmos DB provider

Around a year ago, we started showing similar functionality in demos, using a Cosmos DB provider prototype we put together as a proof of concept. This helped us get some great feedback:

*   Most customers we talked to confirmed that they could see a lot of value in being able to use the EF APIs they were already familiar with to target Cosmos DB, and potentially other NoSQL databases.
*   There were specific details about how the prototype worked, that we needed to fix.
For example, our prototype mapped entities in each inheritance hierarchy to their own separate Cosmos DB collections, but because of the way [Cosmos DB pricing](https://azure.microsoft.com/en-us/pricing/details/cosmos-db/) works, this could become unnecessarily expensive.
Based on this feedback, we decided to implement a new mapping convention that by default stores all entity types defined in the DbContext in the same Cosmos DB collection, and uses a discriminator property to identify the entity type.

The preview we are releasing today, although limited in many ways, is no longer a prototype, but the actual code we plan on keeping working on and eventually shipping.
Our hope is that by releasing it early in development, we will enable many developers to play with it and provide more valuable feedback.

Here are some of the known limitations we are working on overcoming for Preview 3 and RTM.

*   No asynchronous query support: Currently, LINQ queries can only be executed synchronously.
*   Only some of the LINQ operators translatable to Cosmos DB's SQL dialect are currently translated.
*   No synchronous API support for SaveChanges(), EnsureCreated() or EsureDeleted(): you can use the asynchronous versions.
*   No auto-generated unique keys: Since entities of all types share the same collection, each entity needs to have a globally unique key value, but in Preview 2, if you use an integer Id key, you will need to set it explicitly to unique values on each added entity. This has been addressed, and in our nightly builds we now automatically generate GUID values.
*   No nesting of owned entities in documents: We are planning to use [entity ownership](https://docs.microsoft.com/en-us/ef/core/modeling/owned-entities) to decide when an entity should be serialized as part of the same JSON document as the owner. In fact we are extending the ability to specify ownership to collections in 2.2\. However this behavior hasn't been implemented yet and each entity is stored as its own document.

You can track in more detail our progress overcoming these and other limitations in [this issue on GitHub](https://github.com/aspnet/EntityFrameworkCore/issues/12086).

For anything else that you find, please report it [as a new issue](https://github.com/aspnet/EntityFrameworkCore/issues/new).

## Spatial extensions for SQL Server and in-memory

Support for exposing the spatial capabilities of databases through the mapping of spatial columns and functions is a [long-standing and popular feature request](https://github.com/aspnet/EntityFrameworkCore/issues/1100) for EF Core.
In fact, [some of this functionality has been available to you for some time](http://www.npgsql.org/efcore/mapping/nts.html) if you use the EF Core provider for PostgreSQL, Npgsql.
In EF Core 2.2, we are finally attempting to address this for the providers that we ship.

Our implementation picks the same [NetTopologySuite](https://github.com/NetTopologySuite/NetTopologySuite) library that the PostgreSQL provider uses as the source of spatial .NET types you can use in your entity properties.
NetTopologySuite is a database-agnostic spatial library that implements standard spatial functionality using .NET idioms like properties and indexers.

The extension then adds the ability to map and convert instances of these types to the column types supported by the underlying database, and usage of methods defined on these types in LINQ queries, to SQL functions supported by the underlying database.

You can install the spatial extension using the following command form the command line:

``` console
$ dotnet add package Microsoft.EntityFrameworkCore.SqlServer.NetTopologySuite -v 2.2.0-preview2-35157
```

Or from the Package Manager Console in Visual Studio:

``` console
PM> Install-Package Microsoft.EntityFrameworkCore.SqlServer.NetTopologySuite -Version 2.2.0-preview2-35157
```

Once you have installed this extension, you can enable it in your DbContext by calling the UseNetTopologySuite() method inside UseSqlServer() either in OnConfiguring() or AddDbContext().
For example:

``` csharp
public class SensorContext : DbContext
{
  public DbSet<Measurement> Measurements { get; set; }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    optionsBuilder
      .UseSqlServer(
        @"Server=(localdb)\mssqllocaldb;Database=SensorDatabase;Trusted_Connection=True;ConnectRetryCount=0",
        sqlOptions => sqlOptions.UseNetTopologySuite())
  }
}
```

Then you can start using spatial types in your model definition. In this case, we will use `NetTopologySuite.Geometries.Point` to represent the location of a measurement:

``` csharp
using NetTopologySuite.Geometries;
...
  public class Measurement
  {
      public int Id { get; set; }
      public DateTime Time { get; set; }
      public Point Location { get; set; }
      public double Temperature { get; set; }
  }
```

Once you have configured the DbContext and the model in this way, you can create the database, and start persisting spatial data:

``` csharp
using (var context = new SensorContext())
{
  context.Database.EnsureCreated();
  context.AddRange(
    new Measurement { Time = DateTime.Now, Location = new Point(0, 0), Temperature = 0.0},
    new Measurement { Time = DateTime.Now, Location = new Point(1, 1), Temperature = 0.1},
    new Measurement { Time = DateTime.Now, Location = new Point(1, 2), Temperature = 0.2},
    new Measurement { Time = DateTime.Now, Location = new Point(2, 1), Temperature = 0.3},
    new Measurement { Time = DateTime.Now, Location = new Point(2, 2), Temperature = 0.4});
  context.SaveChanges();
}
```

And once you have a database containing spatial data, you can start executing queries:

``` csharp
var currentLocation = new Point(0, 0);

var nearestMesurements =
  from m in context.Measurements
  where m.Location.Distance(currentLocation) < 2
  orderby m.Location.Distance(currentLocation) descending
  select m;

foreach (var m in nearestMeasurements)
{
    Console.WriteLine($"A temperature of {m.Temperature} was detected on {m.Time} at {m.Location}.");
}
```

This will result in the following SQL query being executed:

``` sql
SELECT [m].[Id], [m].[Location], [m].[Temperature], [m].[Time]
FROM [Measurements] AS [m]
WHERE [m].[Location].STDistance(@__currentLocation_0) < 2.0E0
ORDER BY [m].[Location].STDistance(@__currentLocation_0) DESC
```

### Current capabilities and limitations of the spatial extensions

*   It is possible to map properties of concrete types from `NetTopologySuite.Geometries` such as `Geometry`, `Point`, or `Polygon`, or interfaces from `GeoAPI.Geometries`, such as `IGeometry`, `IPoint`, `IPolygon`, etc.
*   Only SQL Server and in-memory database are supported: For in-memory it is not necessary to call UseNetTopologySuite(). SQLite will be enabled in Preview 3 via SpatiaLite.
*   EF Core Migrations does not scaffold spatial types correctly, so you currently cannot use Migrations to create the database schema or apply seed data without workarounds.
*   Mapping to Geography columns doesn't completely work yet.
*   You may find warnings saying that value converters are used. These can be ignored.
*   Reverse engineering a table that contains spatial columns into an entity isn't supported yet.

For anything else that you find, please report it [as a new issue](https://github.com/aspnet/EntityFrameworkCore/issues/new).

### Collections of owned entities

EF Core 2.2 extends the ability to express [ownership relationships](https://docs.microsoft.com/en-us/ef/core/modeling/owned-entities) to one-to-many associations. This helps constraining how entities in an owned collection can be manipulated (for example, they cannot be used without an owner) and triggers automatic behaviors such as implicit eager loading. In the case of relational database, owned collections are mapped to separate tables from the owner, just like regular one-to-many associations, but in the case of a document-oriented database such as Cosmos DB, we plan to nest owned entities (in owned collections or references) within the same JSON document as the owner. You can use the feature by invoking the new OwnsMany() API:

``` csharp
modelBuilder.Entity<Customer>().OwnsMany(c => c.Addresses);
```

### Query tags

This feature is designed to facilitate the correlation of LINQ queries in code with the corresponding generated SQL output captured in logs.

To take advantage of the feature, you _annotate_ a query using the new WithTag() API in a LINQ query.
Using the spatial query from the previous example:

``` csharp
var nearestMesurements =
    from m in context.Measurements.WithTag(@"This is my spatial query!")
    where m.Location.Distance(currentLocation) < 2.5
    orderby m.Location.Distance(currentLocation) descending
    select m;
```
This will generate the following SQL output:

``` sql
-- EFCore: (#This is my spatial query!)
SELECT [m].[Id], [m].[Location], [m].[Temperature], [m].[Time]
FROM [Measurements] AS [m]
WHERE [m].[Location].STDistance(@__currentLocation_0) < 2.5E0
ORDER BY [m].[Location].STDistance(@__currentLocation_0) DESC
```
## Provider compatibility

Although we have setup testing to make sure that existing providers will continue to work with EF Core 2.2, there might be unexpected problems, and we welcome users and provider writers to report compatibility issues on [our issue tracker](https://github.com/aspnet/EntityFrameworkCore/issues/new).

## What comes next?

We are still working in some additional features we would like to include in EF Core 2.2, like reverse engineering of database views into query types, support for spatial types with SQLite, as well as additional bug fixes.
We are planning on releasing EF Core 2.2 in the last calendar quarter of 2018.

In the meantime, our team has started working on the our next major release, EF Core 3.0, which will include, among other improvements, a significant overhaul of our LINQ implementation.

We will also soon start the work to make Entity Framework 6 compatible with .NET Core 3.0, [which was announced last may](https://blogs.msdn.microsoft.com/dotnet/2018/05/07/net-core-3-and-support-for-windows-desktop-applications/).

## Your feedback is really needed!

**We encourage you to play with the new features, and we thank you in advance for posting any feedback to [our issue tracker](https://github.com/aspnet/EntityFrameworkCore/issues/new).**

**The spatial extensions and the Cosmos DB provider in particular are very large features that expose a lot of new capabilities and APIs.
Really being able to ship these features as part of EF Core 2.2 RTM is going to depend on your valuable feedback and on our ability to use it to iterate over the design in the next few months.**
