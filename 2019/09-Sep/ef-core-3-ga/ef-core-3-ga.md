We are extremely excited to announce the general availability of [EF Core 3.0][1]and [EF 6.3][2] on [nuget.org][3].

The final versions of [.NET Core 3.0][4] and [ASP.NET Core 3.0][5] are also available now.

## How to get EF Core 3.0 {#how-to-get-ef-core-3-0}

EF Core 3.0 is distributed exclusively as a set of NuGet packages. For example, to add the SQL Server provider to your project, you can use the following command using the `dotnet` tool:

<pre><code class="highlight-text-shell-session">dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 3.0.0</code></pre>

When upgrading applications that target older versions of ASP.NET Core to 3.0, you also have to add the EF Core packages as an explicit dependency.

Also starting in 3.0, the `dotnet ef` command-line tool is no longer included in the .NET Core SDK. Before you can execute EF Core migration or scaffolding commands, you’ll have to install this package as either a global or local tool. To install the final version of our 3.0.0 tool as a global tool, use the following command:

<pre><code class="highlight-text-shell-session">dotnet tool install --global dotnet-ef --version 3.0.0</code></pre>

Specifying the version in the command is now optional. If you omit it, `dotnet tool install` will automatically install the latest stable version, which is right now 3.0.0.

It's possible to use this new version of `dotnet ef` with projects that use older versions of the EF Core runtime. However, older versions of the tool will not work with EF Core 3.0.

## What's new in EF Core 3.0 {#what-s-new-in-ef-core-3-0}

Including major features, minor enhancements, and bug fixes, EF Core 3.0 contains more than 600 product improvements. Here are some of the most important ones:

### LINQ overhaul {#linq-overhaul}

We rearchitected our LINQ provider to enable translating more query patterns into SQL, generating efficient queries in more cases, and preventing inefficient queries from going undetected. The new LINQ provider is the foundation over which we'll be able to offer new query capabilities and performance improvements in future releases, without breaking existing applications and data providers.

#### Restricted client evaluation {#restricted-client-evaluation}

The most important design change has to do with how we handle LINQ expressions that cannot be converted to parameters or translated to SQL.

In previous versions, EF Core identified what portions of a query could be translated to SQL, and executed the rest of the query on the client. This type of client-side execution is desirable in some situations, but in many other cases it can result in inefficient queries.

For example, if EF Core 2.2 couldn't translate a predicate in a `Where()` call, it executed an SQL statement without a filter, transferred all the rows from the database, and then filtered them in-memory:

<pre><code class="highlight-source-cs">var specialCustomers = 
  context.Customers
    .Where(c =&gt; c.Name.StartsWith(n) && IsSpecialCustomer(c));</code></pre>

That may be acceptable if the database contains a small number of rows but can result in significant performance issues or even application failure if the database contains a large number or rows.

In EF Core 3.0, we've restricted client evaluation to only happen on the top-level projection (essentially, the last call to `Select()`). When EF Core 3.0 detects expressions that can't be translated anywhere else in the query, it throws a runtime exception.

To evaluate a predicate condition on the client as in the previous example, developers now need to explicitly switch evaluation of the query to LINQ to Objects:

<pre><code class="highlight-source-cs">var specialCustomers =
  context.Customers
    .Where(c =&gt; c.Name.StartsWith(n)) 
    .AsEnumerable() // switches to LINQ to Objects
    .Where(c =&gt; IsSpecialCustomer(c));</code></pre>

See the [breaking changes documentation][6] for more details about how this can affect existing applications.

#### Single SQL statement per LINQ query {#single-sql-statement-per-linq-query}

Another aspect of the design that changed significantly in 3.0 is that we now always generate a single SQL statement per LINQ query. In previous versions, we used to generate multiple SQL statements in certain cases, like to translate `Include()` calls on collection navigation properties and to translate queries that followed certain patterns with subqueries. Although this was in some cases convenient, and for `Include()` it even helped avoid sending redundant data over the wire, the implementation was complex, it resulted in some extremely inefficient behaviors (N+1 queries), and there was situations in which the data returned across multiple queries could be inconsistent.

Similarly to client evaluation, if EF Core 3.0 can't translate a LINQ query into a single SQL statement, it throws a runtime exception. But we made EF Core capable of translating many of the common patterns that used to generate multiple queries to a single query with JOINs.

### Cosmos DB support {#cosmos-db-support}

The Cosmos DB provider for EF Core enables developers familiar with the EF programing model to easily target Azure Cosmos DB as an application database. The goal is to make some of the advantages of Cosmos DB, like global distribution, "always on" availability, elastic scalability, and low latency, even more accessible to .NET developers. The provider enables most EF Core features, like automatic change tracking, LINQ, and value conversions, against the SQL API in Cosmos DB.

See the [Cosmos DB provider documentation][7] for more details.

### C# 8.0 support {#csharp-8-0-support}

EF Core 3.0 takes advantage of a couple of the [new features in C# 8.0][8]:

#### Asynchronous streams {#asynchronous-streams}

Asynchronous query results are now exposed using the new standard `IAsyncEnumerable<T>` interface and can be consumed using `await foreach`.

<pre><code class="highlight-source-cs">var orders = 
  from o in context.Orders
  where o.Status == OrderStatus.Pending
  select o;

await foreach(var o in orders)
{
  Process(o);
}</code></pre>

See the [asynchronous streams in the C# documentation][9] for more details.

#### Nullable reference types {#nullable-reference-types}

When this new feature is enabled in your code, EF Core examines the nullability of reference type properties and applies it to corresponding columns and relationships in the database: properties of non-nullable references types are treated as if they had the `[Required]` data annotation attribute.

For example, in the following class, properties marked as of type `string?` will be configured as optional, whereas `string` will be configured as required:

<pre><code class="highlight-source-cs">public class Customer
{
  public int Id { get; set; }
  public string FirstName { get; set; }
  public string LastName { get; set; }
  public string? MiddleName { get; set; }
}</code></pre>

See [nullable reference types in the C# documentation][10] for more details.

### Interception of database operations {#interception-of-database-operations}

The new interception API in EF Core 3.0 allows providing custom logic to be invoked automatically whenever low-level database operations occur as part of the normal operation of EF Core. For example, when opening connections, committing transactions, or executing commands.

Similarly to the interception features that existed in EF 6, interceptors allow you to intercept operations before or after they happen. When you intercept them before they happen, you are allowed to by-pass execution and supply alternate results from the interception logic.

For example, to manipulate command text, you can create an `IDbCommandInterceptor`:

<pre><code class="highlight-source-cs">public class HintCommandInterceptor : DbCommandInterceptor
{
  public override InterceptionResult ReaderExecuting(
    DbCommand command, 
    CommandEventData eventData, 
    InterceptionResult result)
  {
    // Manipulate the command text, etc. here...
    command.CommandText += " OPTION (OPTIMIZE FOR UNKNOWN)";
    return result;
  }
}</code></pre>

And register it with your `DbContext`:

<pre><code class="highlight-source-cs">services.AddDbContext(b =&gt; b
  .UseSqlServer(connectionString)
  .AddInterceptors(new HintCommandInterceptor()));</code></pre>

### Reverse engineering of database views {#reverse-engineering-of-database-views}

Query types, which represent data that can be read from the database but not updated, have been renamed to [keyless entity types][11]. As they are an excellent fit for mapping database views in most scenarios, EF Core now automatically creates keyless entity types when reverse engineering database views.

For example, using the [dotnet ef command-line tool][12] you can type:

<pre><code class="highlight-text-shell-session">dotnet ef dbcontext scaffold "Server=(localdb)\mssqllocaldb;Database=Blogging;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer</code></pre>

And the tool will now automatically scaffold types for views and tables without keys:

<pre><code class="highlight-source-cs">protected override void OnModelCreating(ModelBuilder modelBuilder)
{
  modelBuilder.Entity&lt;Names&gt;(entity =&gt;
  {
    entity.HasNoKey();
    entity.ToView("Names");
  });

  modelBuilder.Entity&lt;Things&gt;(entity =&gt;
  {
    entity.HasNoKey();
  });
}</code></pre>

### Dependent entities sharing a table with principal are now optional {#dependent-entities-sharing-a-table-with-principal-are-now-optional}

Starting with EF Core 3.0, if `OrderDetails` is owned by `Order` or explicitly mapped to the same table, it will be possible to add an `Order` without an `OrderDetails` and all of the `OrderDetails` properties, except the primary key will be mapped to nullable columns.

When querying, EF Core will set `OrderDetails` to `null` if any of its required properties doesn't have a value, or if it has no required properties besides the primary key and all properties are `null`.

<pre><code class="highlight-source-cs">public class Order
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public OrderDetails Details { get; set; }
}

[Owned]
public class OrderDetails
{
    public int Id { get; set; }
    public string ShippingAddress { get; set; }
}</code></pre>

## What's new in EF 6.3 {#what-s-new-in-ef-6-3}

We understand that many existing applications use previous versions of EF, and that porting them to EF Core only to take advantage of .NET Core can require a significant effort. For that reason, we decided to port the newest version of EF 6 to run on .NET Core 3.0. The developer community also contributed to this release with several bug fixes and enhancements.

Here are some of the most notable improvements:

*   Support for .NET Core 3.0 
    *   The EF 6.3 runtime package now targets .NET Standard 2.1 in addition to .NET Framework 4.0 and 4.5.
    *   The migration commands have been rewritten to execute out of process and work with SDK-style projects.
*   Support for SQL Server hierarchyid
*   Improved compatibility with Roslyn and NuGet PackageReference
*   Added the `ef6.exe` utility for enabling, adding, scripting, and applying migrations from assemblies. This replaces `migrate.exe`

There are certain limitations when using EF 6.3 in .NET Core. For example:

*   Data providers need to be also ported to .NET Core. We only ported the SQL Server provider, which is included in the EF 6.3 package.
*   Spatial support won't be enabled with SQL Server because the spatial types aren't enabled to work with .NET Core.
*   There's currently no support for using the EF designer directly on .NET Core or .NET Standard projects.

For more details on the EF 6.3 release, and a workaround to the latter limitation, see [What's new in EF 6.3][13] in the product's documentation.

## What's next: EF Core 3.1 {#what-s-next-ef-core-3-1}

The EF team is now focused on the EF Core 3.1 release, which is planned for later this year, and on making sure that the documentation for EF Core 3.0 is complete.

EF Core 3.1 will be a [long-term support (LTS) release][14], which means it will be supported for at least 3 years. Hence the focus is on stabilizing and fixing bugs rather than adding new features and risky changes. We recommend that you adopt .NET Core 3.0 today and then adopt 3.1 when it becomes available. There won't be breaking changes between these two releases.

The full set of issues fixed in 3.1 can be seen in [our issue tracker][15]. Here are some worth mentioning:

*   Fixes and improvements for issues recently found in the Cosmos DB provider 
*   Fixes and improvements for issues recently found in the new LINQ implementation 
*   Lots of regressions tests added for issues verified as fixed in 3.0
*   Test stability improvements
*   Code cleanup

The first preview of EF Core 3.1 will be available very soon.

## Thank you {#thank-you}

If you either sent code contributions or feedback for any of our preview releases, thanks a lot! You helped make EF Core 3.0 and EF 6.3 significantly better!

We hope everyone will now enjoy the results.

 [1]: https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/3.0.0
 [2]: https://www.nuget.org/packages/EntityFramework/6.3.0
 [3]: https://nuget.org
 [4]: https://devblogs.microsoft.com/dotnet/announcing-net-core-3-0/
 [5]: https://devblogs.microsoft.com/aspnet/asp-net-core-and-blazor-updates-in-net-core-3-0/
 [6]: https://docs.microsoft.com/ef/core/what-is-new/ef-core-3.0/breaking-changes#linq-queries-are-no-longer-evaluated-on-the-client
 [7]: https://docs.microsoft.com/ef/core/providers/cosmos/index
 [8]: https://docs.microsoft.com/dotnet/csharp/whats-new/csharp-8
 [9]: https://docs.microsoft.com/dotnet/csharp/whats-new/csharp-8#asynchronous-streams
 [10]: https://docs.microsoft.com/dotnet/csharp/whats-new/csharp-8#nullable-reference-types
 [11]: https://docs.microsoft.com/ef/core/modeling/keyless-entity-types
 [12]: https://docs.microsoft.com/ef/core/miscellaneous/cli/dotnet
 [13]: https://docs.microsoft.com/ef/ef6/what-is-new/#ef-630
 [14]: https://dotnet.microsoft.com/platform/support/policy/dotnet-core
 [15]: https://github.com/aspnet/EntityFrameworkCore/issues?utf8=%E2%9C%93&q=is%3Aissue+is%3Aopen+is%3Aissue+milestone%3A3.1.0+label%3Aclosed-fixed
