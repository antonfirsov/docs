Porting from Entity Framework 6 to Entity Framework Core
========================================================
Introduction
------------
Entity Framework Core is a lightweight and extensible version of the Entity Framework data access technology which is cross-platform and supports multiple database providers. You can find a comparison of EF Core vs. EF 6 under the [Entity Framework documentation] (https://docs.efproject.net/en/latest/efcore-vs-ef6/index.html).

When porting an application from Entity Framework 6 to Entity Framework Core, you may encounter features that existed in EF 6 but no longer exist in EF Core, or are not yet implemented. For many of those features, the functionality can still be achieved. This article discusses three features: seeding, custom conventions and interceptors.
Seeding
-------
Entity Framework 6 allows overriding of `Seed()` method as part of using migrations, in `DbMigrationsConfiguration<TContext>.Seed()`, or one of the following methods as part of the database initialization:
* `DropCreateDatabaseIfModelChanges<TContext>.Seed()` 
* `DropCreateDatabaseAlways<TContext>.Seed()`
* `CreateDatabaseIfNotExists<TContext>.Seed()`

EF Core does not provide these API’s, and the database initializers no longer exist in EF Core.
The recommendation from the Entity Framework team is to use `context.Database.Migrate()` if using migrations, or `context.Database.EnsureCreated()/EnsureDeleted()` if not using migrations.
The patterns for seeding the database in EF Core are discussed on GitHub, in the Entity Framework Core repository, issue [3070](https://github.com/aspnet/EntityFramework/issues/3070).
The recommendation from the EF team is to run the seeding code within a service scope in `Startup.Configure()`:
```C#
using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>() 	
  .CreateScope())
   {
          var context = serviceScope.ServiceProvider.GetService<DbContext>();       
          …
   }
```
This pattern for seeding is used in the [MusicStore](https://github.com/aspnet/MusicStore) sample.
Custom Conventions
------------------
In Entity Framework 6 we can create custom configurations of properties and tables by using model based conventions. For example, the following code in EF 6 creates a convention to throw an exception when the column name is longer than 30 characters:
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
Interceptors
------------
Entity Framework 6 provides the ability to intercept a context using `IDbCommandInterceptor`. Interceptors let you to get into the pipeline just before and just after a query or command is sent to the database.
Entity Framework Core doesn’t have any interceptors yet. The functionality can be achieved by accessing internal services, in a similar way with the example described above for the model validator.
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
The approaches described above, which override low-level components of Entity Framework Core, should not be considered as long term solutions. The API’s for accessing internal services may change in the future releases, and there is the risk that the application will break when updated to a new version of Entity Framework Core.

