Implementing Seeding, Custom Conventions and Interceptors in EF Core 1.0
========================================================
Introduction
------------
Entity Framework Core (EF Core) is a lightweight and extensible version of the Entity Framework (EF) data access technology which is cross-platform and supports multiple database providers. You can find a comparison of EF Core vs. EF6 under the [Entity Framework documentation] (https://docs.efproject.net/en/latest/efcore-vs-ef6/index.html).

When moving an application from EF6 to EF Core, you may encounter features that existed in EF6 but either are not present or are not yet implemented in EF Core. For many of those features, however, you can implement equivalent functionality.

Seeding
-------
With EF6 you can seed a database with initial data by overriding one of the following `Seed()` methods: 
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
       context.Database.Migrate();
       context.EnsureSeedData();
 }
```
You can find [here](https://github.com/rowanmiller/UnicornStore/blob/master/UnicornStore/src/UnicornStore/Startup.cs#L66) an example of database initialization that uses migrations, along with an implementation example of [EnsureSeedData()] (https://github.com/rowanmiller/UnicornStore/blob/master/UnicornStore/src/UnicornStore/Models/UnicornStore/UnicornStoreExtensions.cs) method.
The [MusicStore](https://github.com/aspnet/MusicStore) sample also uses this pattern for seeding.

Please note that, in general, it is recommended to apply these operations manually (rather than performing migrations and seeding automatically on startup), to avoid racing conditions when there are multiple servers, and unintentional changes.

Custom Conventions
------------------
In Entity Framework 6 we can create custom configurations of properties and tables by using model-based conventions. For example, the following code in EF6 creates a convention to throw an exception when the column name is longer than 30 characters:
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
EF Core does not provide the `IStoreModelConvention` interface; however, we can create this convention by accessing the data model inside the OnModelCreating() method:
```C#
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    foreach (var entityType in modelBuilder.Model.GetEntityTypes())
    {
        foreach (var property in entityType.GetProperties())
        {
            var columnName = property.SqlServer().ColumnName;
            if (columnName.Length > 30)
            {
                throw new InvalidOperationException("Column name is greater than 30 characters - " + columnName);
            }
        }
    }
}
```
Note that the model is not read-only and it can be modified inside the loop.

Interceptors
------------
Entity Framework 6 provides the ability to intercept a context using `IDbCommandInterceptor`. Interceptors let you to get into the pipeline just before and just after a query or command is sent to the database.
Entity Framework Core doesn’t have any interceptors yet. Similar functionality can be achieved by overriding DbContext.SaveChanges(), such as in the following example:
```C#
public override int SaveChanges(bool acceptAllChangesOnSuccess)
{
    ChangeTracker.DetectChanges();

    foreach (var entry in ChangeTracker.Entries().Where(e => e.State == EntityState.Added))
    {
        //modify entry.Entity here
    }

    ChangeTracker.AutoDetectChangesEnabled = false;
    var result = base.SaveChanges(acceptAllChangesOnSuccess);
    ChangeTracker.AutoDetectChangesEnabled = true;

    return result;
}
```
Some notes on the example above:
* The call to `ChangeTracker.DetectChanges()` is to ensure that the change tracker is aware of the changes made to the entities, e.g. if you set .Category to a new Category on an existing Product, the new Category wouldn’t be tracked until `DetectChanges()` is called or it’s added explicitly through DbSet or ChangeTracker.
* Setting `AutoDetectChangesEnabled` to false before calling the base `SaveChanges` is for performance reasons, to avoid calling `DetectChanges()` again.


Interceptors and seeding are high on the feature backlog and the Entity Framework team plans to address them in the near future.

Useful Links
---------
* [Moving an application from EF6 to EF Core](https://docs.efproject.net/en/latest/efcore-vs-ef6/porting/index.html)
* EF Core [Migrations: Seed Data](https://github.com/aspnet/EntityFramework/issues/629) GitHub issue 
* [Lifecycle Hooks] (https://github.com/aspnet/EntityFramework/issues/626) GitHub issue
* [EF Core Roadmap] (https://github.com/aspnet/EntityFramework/wiki/Roadmap)
