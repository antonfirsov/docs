.NET Core Data Access
=====================

.NET Core has been released a few months ago, and data access libraries for most databases, both [relational](https://en.wikipedia.org/wiki/Relational_database) and [NoSQL](https://en.wikipedia.org/wiki/NoSQL) is now available. In this post, I'll detail what client libraries are available, as well as show code samples.

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

[.NET Core support is in beta](https://blogs.msdn.microsoft.com/dotnet/2016/10/19/net-core-tooling-in-visual-studio-15).

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

CouchDB
-------

What about OLE DB?
------------------

[OLE DB](https://msdn.microsoft.com/en-us/library/ms722784(v=vs.85).aspx) has been a great way to access various data sources in a uniform manner, but it was based on COM, which is a Windows-only technology, and as such was not the best fit for a cross-platform technology such as .NET Core. It is also unsupported in SQL Server versions 2014 and later.

Keeping track
-------------

More database support will come as .NET Core matures.