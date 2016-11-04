.NET Core Data Access
=====================

.NET Core was released a few months ago, and data access libraries for most databases, both [relational](https://en.wikipedia.org/wiki/Relational_database) and [NoSQL](https://en.wikipedia.org/wiki/NoSQL) are now available. In this post, I'll detail what client libraries are available, as well as show code samples for each of them.

EF Core
-------

[Entity Framework](https://github.com/aspnet/EntityFramework6) is Microsoft's [Objet-Relational Mapper](https://en.wikipedia.org/wiki/Object-relational_mapping) for .NET, and as such is one of the most-used data access technologies for .NET. [EF Core](https://docs.efproject.net/en/latest/), released simultaneously with .NET Core, is a lightweight and extensible version of Entity Framework that works on both .NET Core and .NET Framework. It has [support](https://docs.efproject.net/en/latest/providers/index.html) for [Microsoft SQL Server](https://docs.efproject.net/en/latest/providers/sql-server/index.html), [SQLLite](https://docs.efproject.net/en/latest/providers/sqlite/index.html), [PostgreSQL](http://www.npgsql.org/), [MySQL](https://docs.efproject.net/en/latest/providers/mysql/index.html), [Microsoft SQL Server Compact Edition](https://docs.efproject.net/en/latest/providers/sql-compact/index.html), [DB2](https://docs.efproject.net/en/latest/providers/ibm/index.html), with more, such as [Oracle](https://docs.efproject.net/en/latest/providers/oracle/index.html), to come.

What follows is an example of EF Core code accessing a blog's database. [The full tutorial can be found on the EF documentation site](https://docs.efproject.net/en/latest/platforms/netcore/new-db-sqlite.html).

```csharp
using (var db = new BloggingContext())
{
    db.Blogs.Add(new Blog { Url = "http://blogs.msdn.com/adonet" });
    var count = db.SaveChanges();
    Console.WriteLine($"{count} records saved to database");

    Console.WriteLine();
    Console.WriteLine("All blogs in database:");
    foreach (var blog in db.Blogs)
    {
        Console.WriteLine($" - {blog.Url}");
    }
}
```

Dapper
------

Dapper is a micro-ORM built and maintained by StackExchange engineers. It focuses on performance, and can map the results of a query to a strongly-typed list, or to dynamic objects. [.NET Core support is currently in beta](https://blogs.msdn.microsoft.com/dotnet/2016/10/19/net-core-tooling-in-visual-studio-15).

```csharp
var sql = @"
select * from Customers where CustomerId = @id
select * from Orders where CustomerId = @id";

using (var multi = connection.QueryMultiple(sql, new {id=selectedId}))
{
   var customer = multi.Read<Customer>().Single();
   var orders = multi.Read<Order>().ToList();
   // ...
} 
```

SQL Server
----------

The Microsoft SQL Server client library is built into .NET Core. You don't have to use an ORM, and can instead go directly to the metal and talk to a SQL Server instance or to an Azure SQL database using the same APIs from the `System.Data.SqlClient` package.

```csharp
using (var connection = new SqlConnection("Server=tcp:YourServer,1433;Initial Catalog=YourDatabase;Persist Security Info=True;"))
{
    var command = new SqlCommand("SELECT TOP 10 Id, Name, Price FROM Products ORDER BY Price", connection);
    connection.Open();
    using (var reader = command.ExecuteReader())
    {
        while (reader.Read())
        {
            Console.WriteLine($"{reader[0]}:{reader[1]} ${reader[2]}");
        }
    }
}
```

PostgreSQL
----------

[PostgreSQL](https://www.postgresql.org/) is an open source relational database with a devoted following. The [Npgsql client library supports .NET Core](http://www.npgsql.org/doc/coreclr.html).

```csharp
using (var conn = new NpgsqlConnection("Host=myserver;Username=mylogin;Password=******;Database=music"))
{
    conn.Open();
    using (var cmd = new NpgsqlCommand())
    {
        cmd.Connection = conn;

        cmd.CommandText = "SELECT name FROM artists";
        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                Console.WriteLine(reader.GetString(0));
            }
        }
    }
}
```

MySQL
-----

[MySQL](https://www.mysql.com/) is one of the most commonly used relational databases on the market, and it's open source. [Support for .NET Core is now available](http://insidemysql.com/mysql-connector-net-for-net-core-1-0/), both through [EF Core](https://docs.efproject.net/en/latest/providers/mysql/index.html) and directly through [the MySQL Connector for .NET Core](https://www.nuget.org/packages/MySql.Data/).

```csharp
using (var connection = new MySqlConnection
    {
        ConnectionString = "server=localhost;user id=root;password=******;persistsecurityinfo=True;port=3305;database=music"
    }) {
    connection.Open();
    var command = new MySqlCommand("SELECT * FROM music.category;", connection);

    using (MySqlDataReader reader =  command.ExecuteReader())
    {
        while (reader.Read())
        {
            Console.WriteLine(
                $"{reader["category_id"]}: {reader["name"]} {reader["last_update"]}");
        }
    }
}
```

SQLite
-------

[SQLite](http://sqlite.org/) is a self-contained, embedded relational database that is released in the public domain. SQLite is lightweight (less than 1MB), cross-platform, and is extremely easy to embed and deploy with an application, which explains how it quietly became the most widely deployed database in the world. It's commonly used as an application file format.

You can use [SQLite with EF Core](https://docs.efproject.net/en/latest/providers/sqlite/index.html), or you can talk to a SQLite database directly using the [Microsoft.Data.Sqlite](https://github.com/aspnet/Microsoft.Data.Sqlite/releases) library that is maintained by the ASP.NET team.

```csharp
using (var connection = new SqliteConnection("Filename=" + path"))
{
    connection.Open();

    using (var reader = connection.ExecuteReader("SELECT Name FROM Person;"))
    {
        while (reader.Read())
        {
            Console.WriteLine($"Hello {reader.GetString(0)}!"));
        }
    }
}
```

DB2
---

Pending... Waiting for additional info from IBM.

MongoDB
-------

[MongoDB](https://www.mongodb.com/) is a document database with [an official .NET driver that supports .NET Core](https://mongodb.github.io/mongo-csharp-driver/).

```csharp
var client = new MongoClient("mongodb://localhost:27017");
var database = client.GetDatabase("commerce");
BsonClassMap.RegisterClassMap<Person>();
var customers = database.GetCollection<Person>("customer").AsQueryable();

var query = from c in customers
            where c.Age > 21
            select c;
```

RavenDB
-------

[RavenDB](https://ravendb.net/) is a document database that is not only compatible with .NET Core, it's also built with it.

```csharp
using (IDocumentStore store = new DocumentStore
{
    Url = "http://localhost:8080/",
    DefaultDatabase = "Northwind"
})
{
    store.Initialize();

    using (IDocumentSession session = store.OpenSession())
    {
        IList<Product> results = session
            .Query<Product>()
            .Where(x => x.UnitsInStock > 10)
            .Skip(5)
            .Take(10)
            .ToList();
    }
}
```

Redis
-----

[Redis](http://redis.io/) is one of the most popular key-value stores.

[StackExchange.Redis](https://www.nuget.org/packages/StackExchange.Redis/) is a high performance Redis client that is maintained by the [StackExchange](http://stackexchange.com/) team.

```csharp
var redis = ConnectionMultiplexer.Connect("localhost");
var db = redis.GetDatabase();
var value = await db.StringGetAsync("mykey");
Console.WriteLine($"mykey: {value}");
```

[ServiceStack](https://servicestack.net/) has [its own Redis client library](https://www.nuget.org/packages/ServiceStack.Redis/), that is compatible with .NET Core, like the rest of ServiceStack.

```csharp
var clientsManager = container.Resolve<IRedisClientsManager>();
using (IRedisClient redis = clientsManager.GetClient())
{
    var redisTodos = redis.As<Todo>();
    var todo = redisTodos.GetById(1);
    Console.WriteLine($"Need to {todo.Content}.");
}
```

Cassandra
---------

[Apache Cassandra](http://cassandra.apache.org/) is a highly scalable and fault-tolerant NoSQL database. [DataStax](https://www.nuget.org/packages/CassandraCSharpDriver/) is a C# driver for Cassandra with built-in support for mapping Cassandra data to CLR objects. The latest version is compatible with .NET Core.

```csharp
var cluster = Cluster.Builder()
    .AddContactPoint("127.0.0.1")
    .Build();
using (var session = cluster.Connect())
{
    session.UserDefinedTypes.Define(
      UdtMap.For<Address>()
        .Map(a => a.Street, "street")
        .Map(a => a.City, "city")
        .Map(a => a.ZipCode, "zip_code")
        .Map(a => a.Phones, "phones")
    );
    var query = new SimpleStatement("SELECT id, name, address FROM users where id = ?", userId);
    var rs = await session.ExecuteAsync(query);
    var row = rs.First();
    var userAddress = row.GetValue<Address>("address");
    Console.WriteLine("user lives on {0} Street", userAddress.Street);
}
```

CouchBase
---------

[CouchBase](http://www.couchbase.com/nosql-databases/couchbase-server) is an open source document database that is popular in mobile applications. [The offical Couchbase client library](https://www.nuget.org/packages/CouchbaseNetClient/2.4.0-dp2) is compatible with .NET Core.

```csharp
using (var bucket = Cluster.OpenBucket())
{
    var get = bucket.GetDocument<dynamic>(documentId);
    document = get.Document;
    Console.WriteLine($"{document.Id}: {document.Content.name}");
}
```

CouchDB
-------

What about OLE DB?
------------------

[OLE DB](https://msdn.microsoft.com/en-us/library/ms722784(v=vs.85).aspx) has been a great way to access various data sources in a uniform manner, but it was based on COM, which is a Windows-only technology, and as such was not the best fit for a cross-platform technology such as .NET Core. It is also unsupported in SQL Server versions 2014 and later. For those reasons, OLE DB won't be supported by .NET Core.

Keeping track
-------------

More database support for .NET Core will no doubt become available in the future, and we'll make sure to highlight new client libraries in our [Week in .NET posts]() as they get announced. In the meantime, I hope this post helps get you started with .NET Core application development.