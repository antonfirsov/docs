---
post_title: 'Announcing Entity Framework 7 Preview 7: Interceptors!'
author1: avickers
author2: jeremy-likness
post_slug: announcing-ef7-preview7
microsoft_alias: jeliknes
featured_image: ef7preview7.png
categories: .NET, .NET Core, Entity Framework
summary: Announcing EF7 Preview 7 with new and improved interceptors
desired_publication_date: 2022-08-09
---

Entity Framework Core 7 (EF7) Preview 7 has shipped with support for several new interceptors, as well as improvements to existing interceptors. This blog post will cover these changes, but Preview 7 also contains several additional enhancements, including:

- [Unidirectional many-to-many relationships](https://github.com/dotnet/efcore/issues/3864)
- [Configure columns for properties mapped to multiple tables using TPT, TPC, and entity splitting](https://github.com/dotnet/efcore/issues/28195)
- [Translate statistics aggregate functions on SQL Server](https://github.com/dotnet/efcore/issues/28104)
- [Filtered Include for hidden navigations](https://github.com/dotnet/efcore/issues/27493)
- [Make it easier to pass cancellation tokens to FindAsync](https://github.com/dotnet/efcore/issues/22667)
- [Translate string.Join and string.Concat](https://github.com/dotnet/efcore/issues/2981)

See the [full list of EF7 Preview 7 changes](https://github.com/dotnet/efcore/issues?q=is%3Aissue+milestone%3A7.0.0-preview7+is%3Aclosed) on GitHub.

## Interceptors

EF Core interceptors enable interception, modification, and/or suppression of EF Core operations. This includes low-level database operations such as executing a command, as well as higher-level operations, such as calls to `SaveChanges`. Interceptors support async operations and the ability to modify or suppress operations, making them more powerful than traditional events, logging, and diagnostics. 

Interceptors are registered when configuring a DbContext instance, either in `OnConfiguring` or `AddDbContext`. For example:

```csharp
public class ExampleContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.AddInterceptors(new TaggedQueryCommandInterceptor());
}
```

> [!Note]
> Code for all the examples in this post is [available on GitHub](https://github.com/ajcvickers/EF7Interceptors).

### New and improved interceptors in EF7

EF7 includes the following enhancements to interceptors:

- Interception for [creating and populating new entity instances](https://github.com/dotnet/efcore/issues/15911) (aka "materialization")
- Interception to [modify the LINQ expression tree](https://github.com/dotnet/efcore/issues/28505) before a query is compiled
- Interception for [optimistic concurrency handling](https://github.com/dotnet/efcore/issues/28315) (`DbUpdateConcurrencyException`)
- Interception for [connections _before_ checking if the connection string has been set](https://github.com/dotnet/efcore/issues/23085)
- Interception for when EF Core has [finished consuming a result set](https://github.com/dotnet/efcore/issues/23535), but before that result set is closed
- Interception for [creation of a `DbConnection` by EF Core](https://github.com/dotnet/efcore/issues/23087)
- Interception for [`DbCommand` after it has been initialized](https://github.com/dotnet/efcore/issues/17261)

In addition, EF7 includes new traditional .NET events for:

- When an [entity is about to be tracked or change state](https://github.com/dotnet/efcore/issues/27093), but before it is actually tracked or change state
- Before and after EF Core [detects changes to entities and properties](https://github.com/dotnet/efcore/issues/26506) (aka `DetectChanges` interception)

The following sections show some examples of using these new interception capabilities.

### Simple actions on entity creation

The new [IMaterializationInterceptor](https://github.com/dotnet/efcore/blob/main/src/EFCore/Diagnostics/IMaterializationInterceptor.cs) supports interception before and after an entity instance is created, and before and after properties of that instance are initialized. The interceptor can change or replace the entity instance at each point. This allows:

- Setting unmapped properties or calling methods needed for validation, computed values, or flags
- Using a factory to create instances
- Creating a different entity instance than EF would normally create, such as an instance from a cache, or of a proxy type
- Injecting services into an entity instance

For example, imagine that we want to keep track of the time that an entity was retrieved from the database, perhaps so it can be displayed to a user editing the data. To accomplish this, we first define an interface:

```csharp
public interface IHasRetrieved
{
    DateTime Retrieved { get; set; }
}
```

Using an interface is common with interceptors since it allows the same interceptor to work with many different entity types. For example:

```csharp
public class Customer : IHasRetrieved
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? PhoneNumber { get; set; }

    [NotMapped]
    public DateTime Retrieved { get; set; }
}
```

Notice that the `[NotMapped]` attribute is used to indicate that this property is used only while working with the entity, and should not be persisted to the database.

The interceptor must then implement the appropriate method from `IMaterializationInterceptor` and set the time retrieved:

```csharp
public class SetRetrievedInterceptor : IMaterializationInterceptor
{
    public object InitializedInstance(MaterializationInterceptionData materializationData, object instance)
    {
        if (instance is IHasRetrieved hasRetrieved)
        {
            hasRetrieved.Retrieved = DateTime.UtcNow;
        }
        
        return instance;
    }
}
```

An instance of this interceptor is registered when configuring the `DbContext`:

```csharp

public class CustomerContext : DbContext
{
    private static readonly SetRetrievedInterceptor _setRetrievedInterceptor = new();
    
    public DbSet<Customer> Customers => Set<Customer>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        => optionsBuilder
            .AddInterceptors(_setRetrievedInterceptor)
            .UseSqlite("Data Source = customers.db");
}
```
> [!TIP]
> This interceptor is stateless, which is common, so a single instance is created and shared between all `DbContext` instances.

Now, whenever a `Customer` is queried from the database, the `Retrieved` property will be set automatically. For example:

```csharp
using (var context = new CustomerContext())
{
    var customer = context.Customers.Single(e => e.Name == "Alice");
    Console.WriteLine($"Customer '{customer.Name}' was retrieved at '{customer.Retrieved.ToLocalTime()}'");
}
```

Produces output:

```bash
Customer 'Alice' was retrieved at '8/6/2022 7:50:25 PM'
```

### LINQ expression tree interception

EF Core makes use of .NET LINQ queries. This typically involves using the C#, VB, or F# compiler to build an expression tree which is then translated by EF Core into the appropriate SQL. For example, consider a method that returns a page of customers:

```csharp
List<Customer> GetPageOfCustomers(string sortProperty, int page)
{
    using var context = new CustomerContext();
    
    return context.Customers
        .OrderBy(e => EF.Property<object>(e, sortProperty))
        .Skip(page * 20).Take(20).ToList();
}
```

> [!TIP]
> This query uses the [`EF.Property`](https://docs.microsoft.com/dotnet/api/microsoft.entityframeworkcore.ef.property) method to specify the property to sort by. This allows the application to dynamically pass in the property name, easily allowing sorting by any property of the entity type.

This will work fine as long as the property used for sorting always returns a stable ordering. But this may not always be the case. For example, the LINQ query above generates the following on SQLite when ordering by `Customer.City`:

```sql
SELECT "c"."Id", "c"."City", "c"."Name", "c"."PhoneNumber"
FROM "Customers" AS "c"
ORDER BY "c"."City"
LIMIT @__p_1 OFFSET @__p_0
```

Uf there are multiple customers with the same `City`, then the ordering of this query is not stable. This could lead to missing or duplicate results as the user pages through the data.

A common way to fix this problem is to perform a secondary sorting by primary key. However, rather than manually adding this to every query, we can intercept the query expression tree and add the secondary ordering dynamically whenever an `OrderBy` is encountered. To facilitate this, we will again use an interface, this time for any entity that has an integer primary key:

```csharp
public interface IHasIntKey
{
    int Id { get; }
}
```

This interface is implemented by the entity types of interest:

```csharp
public class Customer : IHasIntKey
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? City { get; set; }
    public string? PhoneNumber { get; set; }
}
```

We then need an interceptor that implements [`IQueryExpressionInterceptor`](https://github.com/dotnet/efcore/blob/main/src/EFCore/Diagnostics/IQueryExpressionInterceptor.cs):

```csharp
public class KeyOrderingExpressionInterceptor : IQueryExpressionInterceptor
{
    public Expression ProcessingQuery(Expression queryExpression, QueryExpressionEventData eventData)
        => new KeyOrderingExpressionVisitor().Visit(queryExpression);

    private class KeyOrderingExpressionVisitor : ExpressionVisitor
    {
        private static readonly MethodInfo ThenByMethod
            = typeof(Queryable).GetMethods()
                .Single(m => m.Name == nameof(Queryable.ThenBy) && m.GetParameters().Length == 2);
        
        protected override Expression VisitMethodCall(MethodCallExpression? methodCallExpression)
        {
            var methodInfo = methodCallExpression!.Method;
            if (methodInfo.DeclaringType == typeof(Queryable)
                && methodInfo.Name == nameof(Queryable.OrderBy)
                && methodInfo.GetParameters().Length == 2)
            {
                var sourceType = methodCallExpression.Type.GetGenericArguments()[0];
                if (typeof(IHasIntKey).IsAssignableFrom(sourceType))
                {
                    var lambdaExpression = (LambdaExpression)((UnaryExpression)methodCallExpression.Arguments[1]).Operand;
                    var entityParameterExpression = lambdaExpression.Parameters[0];

                    return Expression.Call(
                        ThenByMethod.MakeGenericMethod(
                            sourceType,
                            typeof(int)),
                        methodCallExpression,
                        Expression.Lambda(
                            typeof(Func<,>).MakeGenericType(entityParameterExpression.Type, typeof(int)),
                            Expression.Property(entityParameterExpression, nameof(IHasIntKey.Id)),
                            entityParameterExpression));
                }
            }
            
            return base.VisitMethodCall(methodCallExpression);
        }
    }
}
```

This probably looks pretty complicated--and it is! Working with expression trees is typically not easy. Let's look at what's happening:

- Fundamentally, the interceptor encapsulates an [`ExpressionVisitor`](https://docs.microsoft.com/dotnet/api/system.linq.expressions.expressionvisitor). The visitor overrides `VisitMethodCall`, which will be called whenever there is a call to a method in the query expression tree.
- The visitor checks whether or not this is a call to the [`Queryable.OrderBy`](https://docs.microsoft.com/dotnet/api/system.linq.queryable.orderby) method we are interested in.
- If it is, then the visitor further checks when the generic method call is for a type that implements our `IHasIntKey` interface.
- At this point we know that the method call is of the form `OrderBy(e => ...)`. We extract the lambda expression from this call and get the parameter used in that expression--that is, the `e`.
- We now build a new `MethodCallExpression` using the [`Expression.Call`](https://docs.microsoft.com/dotnet/api/system.linq.expressions.expression.call) builder method. In this case, the method being called is `ThenBy(e => e.Id)`. We build this using the parameter extracted above and a property access to the `Id` property of the `IHasIntKey` interface.
- The input into this call is the original `OrderBy(e => ...)`, and so the end result is an expression for `OrderBy(e => ...).ThenBy(e => e.Id)`.
- This modified expression is returned from the visitor, which means the LINQ query has now been appropriately modified to include a `ThenBy` call.
- EF Core continues and compiles this query expression into the appropriate SQL for the database being used.

This interceptor is registered in the same way as we did for the first example. Executing `GetPageOfCustomers` now generates the following SQL:

```sql
SELECT "c"."Id", "c"."City", "c"."Name", "c"."PhoneNumber"
FROM "Customers" AS "c"
ORDER BY "c"."City", "c"."Id"
LIMIT @__p_1 OFFSET @__p_0
```

This will now always produce a stable ordering, even if there are multiple customers with the same `City`.

Phew! That's a lot of code to make a simple change to a query. And even worse, it might not even work for all queries. It is notoriously difficult write an expression visitor that recognizes all the query shapes it should, and none of the ones it should not. For example, this will likely not work if the ordering is done in a subquery.

This brings us to a critical point about interceptors--always ask yourself if there is an easier way of doing what you want. Interceptors are powerful, but it's easy to get things wrong. They are, as the saying goes, an easy way to shoot yourself in the foot.

For example, imagine if we instead changed our `GetPageOfCustomers` method like so:

```csharp
List<Customer> GetPageOfCustomers2(string sortProperty, int page)
{
    using var context = new CustomerContext();
    
    return context.Customers
        .OrderBy(e => EF.Property<object>(e, sortProperty))
        .ThenBy(e => e.Id)
        .Skip(page * 20).Take(20).ToList();
}
```

In this case the `ThenBy` is simply added to the query. Yes, it may need to be done separately to every query, but it's simple, easy to understand, and will always work.

### Optimistic concurrency interception

EF Core supports the [optimistic concurrency pattern](https://docs.microsoft.com/ef/core/saving/concurrency) by checking that the number of rows actually affected by an update or delete is the same as the number of rows expected to be affected. This is often coupled with a concurrency token; that is, a column value that will only match its expected value if the row has not been updated since the expected value was read.+

EF signals a violation of optimistic concurrency by throwing a [`DbUpdateConcurrencyException`](https://docs.microsoft.com/dotnet/api/microsoft.entityframeworkcore.dbupdateconcurrencyexception). In EF7 [`ISaveChangesInterceptor`](https://github.com/dotnet/efcore/blob/main/src/EFCore/Diagnostics/ISaveChangesInterceptor.cs) has new methods `ThrowingConcurrencyException` and `ThrowingConcurrencyExceptionAsync` that are called before the `DbUpdateConcurrencyException` is thrown. These interception points allow the exception to be suppressed, possibly coupled with async database changes to resolve the violation.

For example, if two requests attempt to delete the same entity at almost the same time, then the second delete may fail because the row in the database no longer exists. This may be fine--the end result is that the entity has been deleted anyway. The following interceptor demonstrates how this can be done:

```csharp
public class SuppressDeleteConcurrencyInterceptor : ISaveChangesInterceptor
{
    public InterceptionResult ThrowingConcurrencyException(
        ConcurrencyExceptionEventData eventData,
        InterceptionResult result)
    {
        if (eventData.Entries.All(e => e.State == EntityState.Deleted))
        {
            Console.WriteLine("Suppressing Concurrency violation for command:");
            Console.WriteLine(((RelationalConcurrencyExceptionEventData) eventData).Command.CommandText);

            return InterceptionResult.Suppress();
        }
        
        return result;
    }

    public ValueTask<InterceptionResult> ThrowingConcurrencyExceptionAsync(
        ConcurrencyExceptionEventData eventData,
        InterceptionResult result,
        CancellationToken cancellationToken = default)
        => new(ThrowingConcurrencyException(eventData, result));
}
```

There are several things worth noting about this interceptor:

- Both the synchronous and asynchronous interception methods are implemented. This is important if the application may call either `SaveChanges` or `SaveChangesAsync`. However, if all application code is async, then only `ThrowingConcurrencyExceptionAsync` needs to be implemented. Likewise, if the application never uses synchronous database methods, then only `ThrowingConcurrencyException` needs to be implemented. This is generally true for all interceptors with sync and async methods. (It might be worthwhile implementing the method your application does not use to throw, just in case some sync/async code creeps in.)
- The interceptor has access to [`EntityEntry`](https://docs.microsoft.com/dotnet/api/microsoft.entityframeworkcore.changetracking.entityentry-1) objects for the entities being saved. In this case, this is used to check whether or not the concurrency violation is happening for a delete operation.
- If the application is using a relational database provider, then the [`ConcurrencyExceptionEventData`](https://github.com/dotnet/efcore/blob/main/src/EFCore/Diagnostics/ConcurrencyExceptionEventData.cs) object can be cast to a [`RelationalConcurrencyExceptionEventData`](https://github.com/dotnet/efcore/blob/main/src/EFCore.Relational/Diagnostics/RelationalConcurrencyExceptionEventData.cs) object. This provides additional, relational-specific information about the database operation being performed. In this case, the relation command text is printed to the console.
- Returning `InterceptionResult.Suppress()` tells EF Core to suppress the action it was about to take--in this case, throwing the `DbUpdateConcurrencyException`. This is one of the most powerful features of interceptors to _change the behavior if EF Core_, rather than just observing what EF Core is doing.

### Summary

EF Core 7.0 (EF7) adds several new interceptors and enhances the behavior of already existing interceptors. Interceptors allow application react to what EF is doing in a similar way to events. However, interceptors also allow the behavior of EF to be changed by suppressing or replacing the action that EF was going to perform, or by modifying or replacing the result of that action. Where appropriate, interceptors can perform async database access, allowing complete database commands to be changed by an interceptor.

For more information of EF Core interceptors, see [_Interceptors_](https://docs.microsoft.com/ef/core/logging-events-diagnostics/interceptors) in the EF Core documentation.

Also, the EF Team showed some additional in-depth interceptor examples in an episode of the .NET Data Community Standup--available to [watch now on YouTube](https://youtu.be/EJs3sSetr2Q).

Finally, don't forget that all the code in this post is [available on GitHub](https://github.com/ajcvickers/EF7Interceptors).

## EF7 Prerequisites

- EF7 targets .NET 6, which means it can be used on .NET 6 (LTS) or .NET 7.
- EF7 will not run on .NET Framework.

EF7 is the successor to EF Core 6.0, not to be confused with [EF6](https://github.com/dotnet/ef6). If you are considering upgrading from EF6, please read our guide to [port from EF6 to EF Core](https://docs.microsoft.com/ef/efcore-and-ef6/porting/).

## How to get EF7 previews

EF7 is distributed exclusively as a set of NuGet packages. For example, to add the SQL Server provider to your project, you can use the following command using the dotnet tool:

```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 7.0.0-preview.7.22376.2
```

This following table links to the preview 5 versions of the EF Core packages and describes what they are used for.

|**Package**    |**Purpose**      |
|--------------:|:----------------|
|[Microsoft.EntityFrameworkCore](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/7.0.0-preview.7.22376.2)|The main EF Core package that is independent of specific database providers|
|[Microsoft.EntityFrameworkCore.SqlServer](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer/7.0.0-preview.7.22376.2)|Database provider for Microsoft SQL Server and SQL Azure|
|[Microsoft.EntityFrameworkCore.SqlServer.NetTopologySuite](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer.NetTopologySuite/7.0.0-preview.7.22376.2)|SQL Server support for spatial types|
|[Microsoft.EntityFrameworkCore.Sqlite](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite/7.0.0-preview.7.22376.2)|Database provider for SQLite that includes the native binary for the database engine|
|[Microsoft.EntityFrameworkCore.Sqlite.Core](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite.Core/7.0.0-preview.7.22376.2)|Database provider for SQLite _without_ a packaged native binary|
|[Microsoft.EntityFrameworkCore.Sqlite.NetTopologySuite](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite.NetTopologySuite/7.0.0-preview.7.22376.2)|SQLite support for spatial types|
|[Microsoft.EntityFrameworkCore.Cosmos](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Cosmos/7.0.0-preview.7.22376.2)|Database provider for Azure Cosmos DB|
|[Microsoft.EntityFrameworkCore.InMemory](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.InMemory/7.0.0-preview.7.22376.2)|The in-memory database provider|
|[Microsoft.EntityFrameworkCore.Tools](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Tools/7.0.0-preview.7.22376.2)|EF Core PowerShell commands for the Visual Studio Package Manager Console; use this to integrate tools like [scaffolding](https://docs.microsoft.com/ef/core/managing-schemas/scaffolding) and [migrations](https://docs.microsoft.com/ef/core/managing-schemas/migrations/) with Visual Studio|
|[Microsoft.EntityFrameworkCore.Design](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Design/7.0.0-preview.7.22376.2)|Shared design-time components for EF Core tools|
|[Microsoft.EntityFrameworkCore.Proxies](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Proxies/7.0.0-preview.7.22376.2)|Lazy-loading and change-tracking proxies|
|[Microsoft.EntityFrameworkCore.Abstractions](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Abstractions/7.0.0-preview.7.22376.2)|Decoupled EF Core abstractions; use this for features like extended data annotations defined by EF Core|
|[Microsoft.EntityFrameworkCore.Relational](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Relational/7.0.0-preview.7.22376.2)|Shared EF Core components for relational database providers|
|[Microsoft.EntityFrameworkCore.Analyzers](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Analyzers/7.0.0-preview.7.22376.2)|C# analyzers for EF Core|

We also published the 7.0 preview 1 release of the [Microsoft.Data.Sqlite.Core](https://www.nuget.org/packages/Microsoft.Data.Sqlite.Core/7.0.0-preview.7.22376.2) provider for [ADO.NET](https://docs.microsoft.com/dotnet/framework/data/adonet/ado-net-overview).

## Installing the EF7 Command Line Interface (CLI)

Before you can execute EF7 Core migration or scaffolding commands, you'll have to install the CLI package as either a global or local tool.

To install the preview tool globally, install with:

```bash
dotnet tool install --global dotnet-ef --version 7.0.0-preview.7.22376.2 
```

If you already have the tool installed, you can upgrade it with the following command:

```bash
dotnet tool update --global dotnet-ef --version 7.0.0-preview.7.22376.2 
```

It's possible to use this new version of the EF7 CLI with projects that use older versions of the EF Core runtime.

## Daily builds

EF7 previews are aligned with .NET 7 previews. These previews tend to lag behind the latest work on EF7. Consider using the [daily builds](https://github.com/aspnet/AspNetCore/blob/master/docs/DailyBuilds.md) instead to get the most up-to-date EF7 features and bug fixes.

As with the previews, the daily builds require .NET 6.

## The .NET Data Community Standup

The .NET data team is now live streaming every other Wednesday at 10am Pacific Time, 1pm Eastern Time, or 17:00 UTC. Join the stream to ask questions about the data-related topic of your choice, including the latest preview release.

- [Watch our YouTube playlist](https://aka.ms/efstandups) of previous shows
- [Visit the .NET Community Standup](https://live.dot.net) page to preview upcoming shows
- [Submit your ideas](https://github.com/dotnet/efcore/issues/22700) for a guest, product, demo, or other content to cover

## Documentation and Feedback

The starting point for all EF Core documentation is [docs.microsoft.com/ef/](https://docs.microsoft.com/ef/).

Please file issues found and any other feedback on the [dotnet/efcore GitHub repo](https://github.com/dotnet/efcore).

## Helpful Links

The following links are provided for easy reference and access.

- [EF Core Community Standup Playlist: https://aka.ms/efstandups](https://aka.ms/efstandups)
- [Main documentation: https://aka.ms/efdocs](https://aka.ms/efdocs)
- [Issues and feature requests for EF Core: https://aka.ms/efcorefeedback](https://aka.ms/efcorefeedback)
- [Entity Framework Roadmap: https://aka.ms/efroadmap](https://aka.ms/efroadmap)
- [Bi-weekly updates: https://github.com/dotnet/efcore/issues/27185](https://github.com/dotnet/efcore/issues/27185)

## Thank you from the team

A big thank you from the EF team to everyone who has used and contributed to EF over the years!

Welcome to EF7.
