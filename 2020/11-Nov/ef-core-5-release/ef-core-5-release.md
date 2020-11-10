---
post_title: 'Announcing the Release of EF Core 5.0'
username: jeremy-likness
featured_image: ./efcore5release.png
categories: .NET Core, Entity Framework, ASP.NET 
summary: Announcing the release of EF Core 5.0, a full featured cross-platform version of Entity Framework.
---

Today, the Entity Framework team is delighted to announce the release of [EF Core 5.0](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/5.0.0). This is a general availability/release to manufacturing (GA/RTM) release that addresses final bugs identified in the previous release candidates and is ready for production.

> **‼ NOTE** The Entity Framework team is gathering feedback on next release of EF Core. Please take a moment to [fill out the survey](https://www.surveymonkey.com/r/S3M89J5)!

## What's new in EF Core 5.0

The early releases of EF Core focused on building a flexible and extensible architecture. In EF Core 3.1, the team buttoned down this architecture with some breaking changes and an overhauled query pipeline. The foundation from 3.1 enabled the team and community to deliver an astonishing set of new features for EF Core 5.0. Some of the highlights from the [81 significant enhancements](https://docs.microsoft.com/ef/core/what-is-new/ef-core-5.0/whatsnew) include:

- [Many-to-many relationship mapping](https://docs.microsoft.com/ef/core/what-is-new/ef-core-5.0/whatsnew#many-to-many)
- [Table-per-type inheritance mapping](https://docs.microsoft.com/ef/core/what-is-new/ef-core-5.0/whatsnew#table-per-type-tpt-mapping)
- [IndexAttribute to map indexes without the fluent API](https://docs.microsoft.com/ef/core/what-is-new/ef-core-5.0/whatsnew#indexattribute)
- [Database collations](https://docs.microsoft.com/ef/core/what-is-new/ef-core-5.0/whatsnew#database-collations)
- [Filtered Include](https://docs.microsoft.com/ef/core/what-is-new/ef-core-5.0/whatsnew#filtered-include)
- [Simple logging](https://docs.microsoft.com/ef/core/miscellaneous/events/simple-logging)
- [Exclude tables from migrations](https://docs.microsoft.com/ef/core/what-is-new/ef-core-5.0/whatsnew#exclude-tables-from-migrations)
- [Split queries for related collections](https://docs.microsoft.com/ef/core/what-is-new/ef-core-5.0/whatsnew#split-queries-for-related-collections)
- [Event counters](https://docs.microsoft.com/ef/core/what-is-new/ef-core-5.0/whatsnew#event-counters)
- [SaveChanges interception and events](https://docs.microsoft.com/ef/core/what-is-new/ef-core-5.0/whatsnew#savechanges-interception-and-events)
- [Required 1:1 dependents](https://docs.microsoft.com/ef/core/what-is-new/ef-core-5.0/whatsnew#required-11-dependents)
- [Migrations scripts with transactions](https://docs.microsoft.com/ef/core/what-is-new/ef-core-5.0/whatsnew#migrations-scripts-with-transactions)
- [Rebuild SQLite tables as needed in migrations](https://docs.microsoft.com/ef/core/what-is-new/ef-core-5.0/whatsnew#migrations-rebuild-sqlite-tables)
- [Mapping for table-valued functions](https://docs.microsoft.com/ef/core/what-is-new/ef-core-5.0/whatsnew#table-valued-functions)
- [DbContextFactory support for dependency injection](https://docs.microsoft.com/ef/core/what-is-new/ef-core-5.0/whatsnew#dbcontextfactory)
- [ChangeTracker.Clear to stop tracking all entities](https://docs.microsoft.com/ef/core/what-is-new/ef-core-5.0/whatsnew#reset-dbcontext-state)
- [Improved Cosmos configuration](https://docs.microsoft.com/ef/core/what-is-new/ef-core-5.0/whatsnew#cosmos-configuration)
- [Change-tracking proxies](https://docs.microsoft.com/ef/core/what-is-new/ef-core-5.0/whatsnew#change-tracking-proxies)
- [Property bags](https://docs.microsoft.com/ef/core/what-is-new/ef-core-5.0/whatsnew#property-bags)

These new features are part of a larger pool of changes:

- [Over 230 enhancements](https://github.com/dotnet/efcore/issues?q=is%3Aissue+milestone%3A5.0.0+is%3Aclosed+label%3Atype-enhacment)
- [Over 380 bug fixes](https://github.com/dotnet/efcore/issues?q=is%3Aissue+milestone%3A5.0.0+is%3Aclosed+label%3Atype-bug)
- [Over 80 cleanup and API documentation updates](https://github.com/dotnet/efcore/issues?q=is%3Aissue+milestone%3A5.0.0+is%3Aclosed+-label%3Atype-bug+-label%3Atype-enhancement+)
- [Over 120 updates to documentation pages](https://github.com/dotnet/EntityFramework.Docs/issues?q=is%3Aissue+milestone%3A5.0.0+is%3Aclosed)

## What the community is saying

With so much goodness in EF Core 5.0 it was hard to decide what to cover in the announcement blog post! The team reached out to four pillars of the EF Core community and asked them to weigh in with their favorite features in EF Core 5.0. Here's what they had to say.

> **📅 Mark your calendar** On Wednesday, November 18th at 10am Pacific Time, the EF Core team will host a special edition of the EF Core Community Standup. Our special guests from the EF Core community will join a live panel discussion about the latest features. Join us and have your EF Core 5.0 questions answered! Details will be posted to the [.NET Live](https://live.dot.net/) page.

### Julie Lerman: Simple logging

[Julie Lerman](https://twitter.com/julielerman) is a software coach, [Pluralsight author](https://www.pluralsight.com/authors/julie-lerman) and longtime user of Entity Framework.

EF Core has always integrated with the [Microsoft.Extensions.Logging](https://docs.microsoft.com/aspnet/core/fundamentals/logging/?view=aspnetcore-3.1) infrastructure. This form of logging is powerful and extensible, but requires external dependencies and configuration.  EF Core 5.0 introduces the [LogTo method](https://docs.microsoft.com/ef/core/miscellaneous/events/simple-logging) as a simple way to obtain logs while developing and debugging without installing additional dependencies.

`LogTo` is called when [configuring a DbContext instance](https://docs.microsoft.com/ef/core/miscellaneous/configuring-dbcontext). This configuration is commonly done in an override of `DbContext.OnConfiguring`. For example:

```CSharp
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder
        .EnableSensitiveDataLogging()
        .EnableDetailedErrors()
        .LogTo(Console.WriteLine);
```

Alternately, LogTo can be called as part of `AddDbContext` or when creating a `DbContextOptions` instance to pass to the `DbContext` constructor. For example:

```CSharp
serviceCollection.AddDbContext<SomeDbContext>(
    b => b.UseSqlServer(Your.ConnectionString)
        .EnableSensitiveDataLogging()
        .EnableDetailedErrors()
        .LogTo(Console.WriteLine));
```

Both examples set up logging to the console and also turn on sensitive data logging and detailed errors for the best debugging experience. This results in log output to console; for example:

```text
warn: 11/4/2020 12:59:56.097 CoreEventId.SensitiveDataLoggingEnabledWarning[10400] (Microsoft.EntityFrameworkCore.Infrastructure)
      Sensitive data logging is enabled. Log entries and exception messages may include sensitive application data; this mode should only be enabled during development.
info: 11/4/2020 12:59:56.190 CoreEventId.ContextInitialized[10403] (Microsoft.EntityFrameworkCore.Infrastructure)
      Entity Framework Core 5.0.0-rc.2.20475.6 initialized 'SomeDbContext' using provider 'Microsoft.EntityFrameworkCore.SqlServer' with options: SensitiveDataLoggingEnabled
dbug: 11/4/2020 12:59:56.221 RelationalEventId.ConnectionOpening[20000] (Microsoft.EntityFrameworkCore.Database.Connection)
      Opening connection to database 'Test' on server '(local)'.
```

See the [simple logging documentation](https://docs.microsoft.com/ef/core/miscellaneous/events/simple-logging) for more information on:

- Logging to the debug window or a file
- Log filtering
- Message contents and formatting

> "It’s a beautiful blend of the simplicity of logging in EF6 and the intelligence of .NET Core logging functionality." &mdash; Julie

### Diego Vega: Many-to-many (M2M) relationships

[Diego Vega](https://twitter.com/divega) is a principal software engineer at Microsoft on the Azure networking team. He formerly served as program manager for the EF Core team.

EF Core 5.0 supports many-to-many relationships without explicitly mapping the join table.

For example, consider these entity types:

```CSharp
public class Post
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Tag> Tags { get; set; }
}

public class Tag
{
    public int Id { get; set; }
    public string Text { get; set; }
    public ICollection<Post> Posts { get; set; }
}
```

Notice that `Post` contains a collection of `Tags`, and `Tag` contains a collection of `Posts`. EF Core 5.0 recognizes this as a many-to-many relationship by convention. This means no code is required in `OnModelCreating`:

```CSharp
public class BlogContext : DbContext
{
    public DbSet<Post> Posts { get; set; }
    public DbSet<Blog> Blogs { get; set; }
}
```

When Migrations (or `EnsureCreated`) are used to create the database, EF Core will automatically create the join table. For example, on SQL Server for this model, EF Core generates:

```sql
CREATE TABLE [Posts] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    CONSTRAINT [PK_Posts] PRIMARY KEY ([Id]));

CREATE TABLE [Tags] (
    [Id] int NOT NULL IDENTITY,
    [Text] nvarchar(max) NULL,
    CONSTRAINT [PK_Tags] PRIMARY KEY ([Id]));

CREATE TABLE [PostTag] (
    [PostsId] int NOT NULL,
    [TagsId] int NOT NULL,
    CONSTRAINT [PK_PostTag] PRIMARY KEY ([PostsId], [TagsId]),
    CONSTRAINT [FK_PostTag_Posts_PostsId] FOREIGN KEY ([PostsId]) REFERENCES [Posts] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_PostTag_Tags_TagsId] FOREIGN KEY ([TagsId]) REFERENCES [Tags] ([Id]) ON DELETE CASCADE);

CREATE INDEX [IX_PostTag_TagsId] ON [PostTag] ([TagsId]);
```

Creating and associating `Blog` and `Post` entities results in join table updates happening automatically. For example:

```CSharp
var beginnerTag = new Tag {Text = "Beginner"};
var advancedTag = new Tag {Text = "Advanced"};
var efCoreTag = new Tag {Text = "EF Core"};

context.AddRange(
    new Post {Name = "EF Core 101", Tags = new List<Tag> {beginnerTag, efCoreTag}},
    new Post {Name = "Writing an EF database provider", Tags = new List<Tag> {advancedTag, efCoreTag}},
    new Post {Name = "Savepoints in EF Core", Tags = new List<Tag> {beginnerTag, efCoreTag}});

context.SaveChanges();
```

After inserting the Posts and Tags, EF will then automatically create rows in the join table. For example, on SQL Server:

```text
Executed DbCommand (8ms) [Parameters=[@p6='1', @p7='1', @p8='1', @p9='2', @p10='2', @p11='2', @p12='2', @p13='3', @p14='3', @p15='1', @p16='3', @p17='2'], CommandType='Text', CommandTimeout='30']
      SET NOCOUNT ON;
      INSERT INTO [PostTag] ([PostsId], [TagsId])
      VALUES (@p6, @p7),
      (@p8, @p9),
      (@p10, @p11),
      (@p12, @p13),
      (@p14, @p15),
      (@p16, @p17);
```

For queries, `Include` and other query operations work just like for any other relationship. For example:

```CSharp
foreach (var post in context.Posts.Include(e => e.Tags))
{
    Console.Write($"Post \"{post.Name}\" has tags");

    foreach (var tag in post.Tags)
    {
        Console.Write($" '{tag.Text}'");
    }
}
```

The SQL generated uses the join table automatically to bring back all related Tags:

```sql
SELECT [p].[Id], [p].[Name], [t0].[PostsId], [t0].[TagsId], [t0].[Id], [t0].[Text]
FROM [Posts] AS [p]
LEFT JOIN (
    SELECT [p0].[PostsId], [p0].[TagsId], [t].[Id], [t].[Text]
    FROM [PostTag] AS [p0]
    INNER JOIN [Tags] AS [t] ON [p0].[TagsId] = [t].[Id]
) AS [t0] ON [p].[Id] = [t0].[PostsId]
ORDER BY [p].[Id], [t0].[PostsId], [t0].[TagsId], [t0].[Id]
```

Unlike EF6, EF Core allows full customization of the join table. For example, the code below configures a many-to-many relationship that also has navigations to the join entity, and in which the join entity contains a payload property:

```CSharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder
        .Entity<Post>()
        .HasMany(p => p.Tags)
        .WithMany(p => p.Posts)
        .UsingEntity<PostTag>(
            j => j.HasOne(pt => pt.Tag).WithMany().HasForeignKey(pt => pt.TagId),
            j => j.HasOne(pt => pt.Post).WithMany().HasForeignKey(pt => pt.PostId),
            j =>
            {
                j.Property(pt => pt.PublicationDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
                j.HasKey(t => new { t.PostId, t.TagId });
            });
}
```

See [configuring many-to-many relationships](https://docs.microsoft.com/ef/core/modeling/relationships?tabs=fluent-api%2Cfluent-api-simple-key%2Csimple-key#many-to-many) to learn more about more complex mappings like this.

> "I choose many-to-many because it comes with a payload of additional features." &mdash; Diego

### Jon P Smith: ChangeTracker.Clear()

[Jon P Smith](https://twitter.com/thereformedprog) is a .NET Core backend developer, architect, and the author of [Entity Framework Core in Action](https://www.manning.com/books/entity-framework-core-in-action-second-edition).

As with all EF Core releases, the team implements big features, but also many smaller usability improvements. The new `DbContext.ChangeTracker.Clear()` method is one of these improvements. It clears the EF change tracker so that all currently tracked entities are detached. For example:

```CSharp
using (var context = new BlogContext())
{
    // A normal tracking query brings entities into the context
    var postsAndTags = context.Posts.Include(e => e.Tags).ToList();
    Debug.Assert(context.ChangeTracker.Entries().Count() == 12);

    // These can then used for one or more updates
    postsAndTags.First().Name = "EF Core 102";
    context.SaveChanges();
    Debug.Assert(context.ChangeTracker.Entries().Count() == 12);

    // All tracked entities can then be detached
    context.ChangeTracker.Clear();
    Debug.Assert(context.ChangeTracker.Entries().Count() == 0);
}
```

Note that calling `ChangeTracker.Clear()` in this example should usually not be needed when using the best practice of creating a new, short-lived context instance for each unit-of-work. However, sometimes application architecture can make it hard to use a new `DbContext` for each unit-of-work. For these case, using `ChangeTracker.Clear()` is more performant and robust than mass-detaching all entities.

> "My favorite EF Core 5 feature is `ChangeTracker.Clear()`. I use this in my unit tests to clear out any database setup entity classes so that when I run my tests I know it's read new data from the database." &mdash; Jon

### Erik Ejlskov Jensen: Improved database-first scaffolding

[Erik Ejlskov Jensen](https://twitter.com/ErikEJ) is a Tech Lead at _VENZO_nxt_ and maintains the popular [EF Core Power Tools](https://github.com/ErikEJ/EFCorePowerTools) that provides reverse engineering, migrations, and model visualization for EF Core.

EF Core 5.0 includes many improvements for scaffolding (a.k.a. reverse-engineering) a `DbContext` for a database-first experience. These are features that can be both used from the command-line tools that ship with EF, or with community projects like Erik's visual EF Core Power Tools. The following sections show some examples.

#### Scaffold to custom namespaces

EFCore 5.0 supports [overriding the namespace](https://docs.microsoft.com/ef/core/managing-schemas/scaffolding?tabs=dotnet-core-cli#directories-and-namespaces) for all scaffolded classes. In addition, if desired, a different namespace can be configured for the `DbContext` class. For example:

```bash
dotnet ef dbcontext scaffold ... --namespace Your.Namespace --context-namespace Your.DbContext.Namespace
```

> **💡 TIP** We are showing the .NET command line tool here, but the same options are available in the Package Manager Console PowerShell commands.

#### Stop scaffolding the connection string

By default, EF Core includes the connection string for the scaffolded database in a call to `OnConfiguring` on the generated `DbContext`. However, this can be frustrating for applications that already configure the `DbContext` in some other way; for example, inside `AddDbContext` in an ASP.NET Core application. EF Core 5.0 supports scaffolding a `DbContext` without including a call to `OnConfiguring`. For example:

```bash
dotnet ef dbcontext scaffold ... --no-onconfiguring
```

#### Pluralization

EF Core now integrates with [Humanizer](https://github.com/Humanizr/Humanizer) to provide pluralization by default when scaffolding from a database. Putting all this together, here's an example command using custom namespaces, no `OnConfiguring`, and the default pluralization support:

```bash
dotnet ef dbcontext scaffold 'Server=(local);Database=Blogs;User Id=arthur;Password=Unicorns4All' \
 Microsoft.EntityFrameworkCore.SqlServer --namespace Blogs.Entities \
 --context-namespace Blogs.Context --no-onconfiguring --data-annotations
```

This results in the following classes being generated:

```CSharp
#nullable disable

namespace Blogs.Context
{
    public partial class BlogsContext : DbContext
    {
        public BlogsContext()
        {
        }

        public BlogsContext(DbContextOptions<BlogsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Blog> Blogs { get; set; }
        public virtual DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
```

```CSharp
#nullable disable

namespace Blogs.Entities
{
    [Index(nameof(BlogId), Name = "IX_Posts_BlogId")]
    public partial class Post
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int? BlogId { get; set; }

        [ForeignKey(nameof(BlogId))]
        [InverseProperty("Posts")]
        public virtual Blog Blog { get; set; }
    }
}
```

```CSharp
#nullable disable

namespace Blogs.Entities
{
    public partial class Blog
    {
        public Blog()
        {
            Posts = new HashSet<Post>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        [InverseProperty(nameof(Post.Blog))]
        public virtual ICollection<Post> Posts { get; set; }
    }
}
```

Notice that:

- `DbSet` property names are pluralized
- The entity types are in one namespace and the `DbContext` is in another
- There is no `OnConfiguring` method containing my connection string
- The new `IndexAttribute` is used instead of generating code in `OnModelCreating`
- `#nullable disable` is generated since EF Core does not currently reverse-engineer to nullable reference types

See the [documentation for reverse-engineering](https://docs.microsoft.com/ef/core/managing-schemas/scaffolding?tabs=dotnet-core-cli) for more information.

> "The many small feature additions to database first scaffolding are amongst my favorite features in EF Core 5: Namespace, no connection string on generated code, pluralization - all features that help improve EF Core Power Tools as well" &mdash; Erik

## Learn more

There are many good sources of information about EF Core.

- The [official documentation](https://docs.microsoft.com/ef/) contains everything from getting-started guides to advanced feature documentation
- Keep up with the latest changes in the [EF Core weekly updates](https://github.com/dotnet/efcore/issues/19549)
- The team live streams a [community standup](https://www.youtube.com/playlist?list=PL1rZQsJPBU2Ry_KbYPklhVu0JhP0kOFbj) every few weeks, including guests, news, and demos
- Ask questions on [GitHub](https://github.com/dotnet/efcore/issues) or [Stack Overflow](https://stackoverflow.com/questions/tagged/entity-framework-core)
- There are many great books, online courses, and blog posts about EF Core out there from the likes of Julie, Jon, Erik, and many others. (If you find something really good, then let us know and we'll feature it on the community standup.)

Or contact the team for any reason on [GitHub](https://github.com/dotnet/efcore) or [Twitter](https://twitter.com/i/lists/1253069921669410818).

> **‼ NOTE** Don't forget to [fill out the survey](https://www.surveymonkey.com/r/S3M89J5) and help us decide the future direction of EF Core.

## How to get EF Core 5.0

EF Core 5.0 requires a [.NET Standard 2.1](https://github.com/dotnet/standard/blob/master/docs/versions/netstandard2.1.md) platform. This means EF Core 5.0 will run on .NET Core 3.1 or .NET 5, as well as other [platforms that support .NET Standard 2.1](https://docs.microsoft.com/dotnet/standard/net-standard#net-implementation-support). EF Core 5.0 does not run on .NET Framework.

EF Core 5.0 is distributed exclusively as a set of NuGet packages. For example, to add the SQL Server provider to your project, you can use the following command using the dotnet tool:

```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 5.0.0
```

This following table links to the release versions of the EF Core packages and describes what they are used for.

|**Package**    |**Purpose**      |
|:--------------|:----------------|
|[Microsoft.EntityFrameworkCore](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/5.0.0)|The main EF Core package that is independent of specific database providers|
|[Microsoft.EntityFrameworkCore.SqlServer](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer/5.0.0)|Database provider for Microsoft SQL Server and SQL Azure|
|[Microsoft.EntityFrameworkCore.SqlServer.NetTopologySuite](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer.NetTopologySuite/5.0.0)|SQL Server support for spatial types|
|[Microsoft.EntityFrameworkCore.Sqlite](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite/5.0.0)|Database provider for SQLite that includes the native binary for the database engine|
|[Microsoft.EntityFrameworkCore.Sqlite.NetTopologySuite](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite.NetTopologySuite/5.0.0)|SQLite support for spatial types|
|[Microsoft.EntityFrameworkCore.Cosmos](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Cosmos/5.0.0)|Database provider for Azure Cosmos DB|
|[Microsoft.EntityFrameworkCore.InMemory](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.InMemory/5.0.0)|The in-memory database provider|
|[Microsoft.EntityFrameworkCore.Tools](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Tools/5.0.0)|EF Core PowerShell commands for the Visual Studio Package Manager Console; use this to integrate tools like [scaffolding](https://docs.microsoft.com/ef/core/managing-schemas/scaffolding) and [migrations](https://docs.microsoft.com/ef/core/managing-schemas/migrations/) with Visual Studio|
|[Microsoft.EntityFrameworkCore.Design](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Design/5.0.0)|Shared design-time components for EF Core tools|
|[Microsoft.EntityFrameworkCore.Proxies](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Proxies/5.0.0)|Lazy-loading and change-tracking proxies|
|[Microsoft.EntityFrameworkCore.Abstractions](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Abstractions/5.0.0)|Decoupled EF Core abstractions; use this for features like extended data annotations defined by EF Core|
|[Microsoft.EntityFrameworkCore.Relational](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Relational/5.0.0)|Shared EF Core components for relational database providers|
|[Microsoft.EntityFrameworkCore.Analyzers](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Analyzers/5.0.0)|C# analyzers for EF Core|

## Installing the EF Core Command Line Interface (CLI)

As with EF Core 3.0 and 3.1, the EF Core 5.0 CLI is no longer included in the .NET Core SDK. Before you can execute EF Core migration or scaffolding commands, you'll have to install this package as either a global or local tool.

To install the 5.0 tool globally the first time, use:

```bash
dotnet tool install --global dotnet-ef --version 5.0.0
```

If you already have the tool installed, update it with:

```bash
dotnet tool update --global dotnet-ef --version 5.0.0
```

It’s possible to use this new version of the EF Core CLI with projects that use older versions of the EF Core runtime.

## Special thanks

Development on EF Core is lead by a small team at Microsoft. However, many members of the community contributed to EF Core 5.0, either with code, documentation, or both. Many thanks from the team to everybody who helped us deliver this release!

[View the full list of contributors](https://devblogs.microsoft.com/dotnet/announcing-entity-framework-core-efcore-5-0-rc1/#thank-you-to-our-contributors-)

In addition, a special thank you this release to Diego Vega, our former program manager, who has been fundamental to the design and direction of EF Core over the years. In particular, Diego was a major contributor to the overall design for extensible many-to-many relationships.

## Conclusion

Finally, a big thank you to the entire EF community for feedback, support, and bug reports, not to mention the occasional rant! Start [using EF Core 5.0](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/5.0.0) now and continue filing bugs, voting on issues, and letting us know what you need from EF Core. If you haven't already, then be sure to [take the survey](https://www.surveymonkey.com/r/S3M89J5) and let us know where we should go next with EF Core.  

Thank you,

The EF Core Team

| | | | |
|:-:|:-:|:-:|:-:|
| ![Arthur Vickers](https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/efteam_ajcvickers.jpeg) | ![Andriy Svyryd](https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/efteam_AndriySvyryd.jpeg) | ![Brice Lambson](https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/efteam_bricelam.jpeg) | ![Jeremy Likness](https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/efteam_JeremyLikness.jpeg) |
| [Arthur Vickers](https://github.com/ajcvickers) | [Andriy Svyryd](https://github.com/AndriySvyryd) | [Brice Lambson](https://github.com/bricelam) | [Jeremy Likness](https://github.com/JeremyLikness) |
| ![Maurycy Markowski](https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/efteam_maumar.jpeg) | ![Shay Rojansky](https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/efteam_roji-1-300x300.png) | ![Smit Patel](https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2020/03/efteam_smitpatel.png)| |
| [Maurycy Markowski](https://github.com/maumar) | [Shay Rojansky](https://github.com/roji) | [Smit Patel](https://github.com/smitpatel) | |
