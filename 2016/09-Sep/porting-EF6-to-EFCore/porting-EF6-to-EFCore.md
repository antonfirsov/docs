Implementing Seeding, Custom Conventions and Interceptors when Moving from EF 6 to EF Core 1.0
========================================================
Introduction
------------
Entity Framework Core (EF Core) is a lightweight and extensible version of the Entity Framework (EF) data access technology which is cross-platform and supports multiple database providers. You can find a comparison of EF Core vs. EF 6 under the [Entity Framework documentation] (https://docs.efproject.net/en/latest/efcore-vs-ef6/index.html).

When moving an application from EF 6 to EF Core, you may encounter features that existed in EF 6 but either are not present or are not yet implemented in EF Core. For many of those features, however, you can implement equivalent functionality. This article discusses three features: 
* seeding
* custom conventions
* interceptors

Seeding
-------
With EF 6 you can seed a database with initial data by overriding one of the following `Seed()` methods: 
* [`DbMigrationsConfiguration<TContext>.Seed()`](https://msdn.microsoft.com/en-us/library/hh829453(v=vs.113).aspx)
* [`DropCreateDatabaseIfModelChanges<TContext>.Seed()`](https://msdn.microsoft.com/en-us/library/gg679410(v=vs.113).aspx) 
* [`DropCreateDatabaseAlways<TContext>.Seed()`](https://msdn.microsoft.com/en-us/library/gg679506(v=vs.113).aspx)
* [`CreateDatabaseIfNotExists<TContext>.Seed()`](https://msdn.microsoft.com/en-us/library/gg679221(v=vs.113).aspx)

EF Core does not provide similar APIs, and database initializers also no longer exist in EF Core. To seed the database, you would put the database initialization code in the application startup. If you are using migrations, call `context.Database.Migrate()`, otherwise use `context.Database.EnsureCreated()/EnsureDeleted()`.

The patterns for seeding the database are discussed in issue [3070] (https://github.com/aspnet/EntityFramework/issues/3070) in the Entity Framework Core repository on GitHub.
The recommended approach is to run the seeding code within a service scope in `Startup.Configure()`:
```C#
using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
       var context = serviceScope.ServiceProvider.GetService<MyContext>();       
       if (context.Database.EnsureCreated())
       {
           context.SeedData();
       }
 }
```
You can find [here](https://github.com/rowanmiller/UnicornStore/blob/master/UnicornStore/src/UnicornStore/Startup.cs#L66) an example of database initialization that uses migrations.
The [MusicStore](https://github.com/aspnet/MusicStore) sample also uses this pattern for seeding.

Custom Conventions
------------------
In Entity Framework 6 we can create custom configurations of properties and tables by using model-based conventions. For example, the following code in EF 6 creates a convention to throw an exception when the column name is longer than 30 characters:
```C#
public class IdentifierConvention : IStoreModelConvention<EdmProperty>
{
    public void Apply(EdmProperty item, DbModel model)
    {
         if (item.Name.Length > 30)
        {
            throw new InvalidOperationException("Column name is greater than 30 characters - " + item.Name);
        }
    }
}
```
EF Core does not provide the `IStoreModelConvention` interface; however, we can create this convention by accessing internal services (extending lower level components in EF Core). In the following example we implement a model validator which checks for very long table and column names:
```C#
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.Storage;

public class MyValidator : RelationalModelValidator
{
    const int MAX_TABLE_NAME = 30;
    const int MAX_COLUMN_NAME = 30;
    public MyValidator(
        ILogger<RelationalModelValidator> loggerFactory,
        IRelationalAnnotationProvider relationalExtensions,
        IRelationalTypeMapper typeMapper)
        : base(loggerFactory, relationalExtensions, typeMapper)
    { }

    public override void Validate(IModel model)
    {
        base.Validate(model);

        var longTables = model.GetEntityTypes()
            .Where(e => e.Relational().TableName.Length > MAX_TABLE_NAME)
            .ToList();

        if (longTables.Any())
        {
            throw new NotSupportedException(
                $"The following types are mapped to table names that exceed {MAX_TABLE_NAME} characters; "
                + string.Join(", ", longTables.Select(e => $"{e.ClrType.Name} ({e.Relational().TableName})")));
        }

        var longColumns = model.GetEntityTypes()
            .SelectMany(e => e.GetProperties())
            .Where(p => p.Relational().ColumnName.Length > MAX_COLUMN_NAME)
            .ToList();

        if (longColumns.Any())
        {
            throw new NotSupportedException(
                $"The following properties are mapped to column names that exceed {MAX_COLUMN_NAME} characters; "
                + string.Join(", ", longColumns.Select(p => $"{p.DeclaringEntityType.Name}.{p.Name} ({p.Relational().ColumnName})")));
        }
    }
}
```
Registering and using the ModelValidator created here is explained later in this article.

Interceptors
------------
Entity Framework 6 provides the ability to intercept a context using `IDbCommandInterceptor`. Interceptors let you to get into the pipeline just before and just after a query or command is sent to the database.
Entity Framework Core doesn’t have any interceptors yet. The functionality can be achieved by accessing internal services, in a similar way as the example described above for the model validator.
The following example implements `IEntityStateListener` to modify an entity just before it is added to the database:
```C#
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
public class StateListener : IEntityStateListener
{
    public void StateChanging(InternalEntityEntry entry, EntityState newState)
    {
        if (newState == EntityState.Added)
        {
            //modify entry.Entity here
        }
    }

    public void StateChanged(InternalEntityEntry entry, EntityState oldState, bool skipInitialFixup, bool fromQuery)
    {

    }
}
```
To use the StateListener and the ModelValidator in your context, create a ServiceProvider and use it in OptionsBuilder:
```C#
using Microsoft.Extensions.DependencyInjection;
public class MyContext : DbContext
{
    private static readonly IServiceProvider _serviceProvider
   = new ServiceCollection()
       .AddEntityFrameworkSqlServer()
       .AddSingleton<IEntityStateListener>(new StateListener())
       .AddScoped<RelationalModelValidator, MyValidator>()
       .BuildServiceProvider();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
   => optionsBuilder
       .UseInternalServiceProvider(_serviceProvider)
       .UseSqlServer(@"Server = (localdb)\mssqllocaldb;Database=MyDb;Trusted_Connection=True;");
```
Notes
-----
The APIs for accessing internal services may change in the future releases, and there is a risk that the application will break when updated to a new version of Entity Framework Core. The approaches described above should not be considered as long-term solutions, but as workarounds until we have a first class way of achieving the functionality.

Interceptors and seeding are high on the feature backlog and the Entity Framework team plans to address them in the near future.

Useful Links
---------
* [Moving an application from EF 6 to EF Core](https://docs.efproject.net/en/latest/efcore-vs-ef6/porting/index.html)
* EF Core [Migrations: Seed Data](https://github.com/aspnet/EntityFramework/issues/629) GitHub issue 
* [Lifecycle Hooks] (https://github.com/aspnet/EntityFramework/issues/626) GitHub issue
