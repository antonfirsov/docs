# Entity Framework 2.1 Roadmap
As mentioned in the [announcement of the .NET Core 2.1 roadmap](https://blogs.msdn.microsoft.com/dotnet/2018/02/02/net-core-2-1-roadmap/) earlier today, at this point we know the overall shape of our next release and we have decided on a general schedule for it. As we approach the release of our first preview later this month, we also wanted to expand on what we have planned for Entity Framework Core 2.1.

## New features
Although EF Core 2.1 is a minor release that builds over the foundational 2.0, it introduces significant new capabilities:

-	__Lazy loading:__ EF Core now contains the necessary building blocks for anyone to write entity classes that can load their navigation properties on demand. We have also created a new package, Microsoft.EntityFrameworkCore.Proxies, that leverages those building blocks to produce lazy loading proxy classes based on minimally modified entity classes. In order to use these lazy loading proxies, you only need navigation properties in your entities to be virtual.

-	__Parameters in entity constructors:__ as one of the required building blocks for lazy loading, we enabled the creation of entities that take parameters in their constructor. You can use parameters to inject property values, lazy loading delegates, and services.

-	__Value conversions:__ Until now, EF Core could only map properties of types natively supported by the underlying database provider. Values were copied back and forth between columns and properties without any transformation. Starting with EF Core 2.1, value conversions can be applied to transform the values obtained from columns before they are applied to properties, and vice versa. We have a number of conversions that can be applied by convention as necessary, as well as an explicit configuration API that allows registering delegates for the conversions between columns and properties. Some of the application of this feature are:

    - Storing enums as strings
    - Mapping unsigned integers with SQL Server
    - Transparent encryption and decryption of property values


-	__LINQ GroupBy translation:__ Before EF Core 2.1, the GroupBy LINQ operator would always be evaluated in memory. We now support translating it to the SQL GROUP BY clause in most common cases.

-	__Data Seeding:__ With the new release it will be possible to provide initial data to populate a database. Unlike in EF6, in EF Core, seeding data is associated to an entity type as part of the model configuration. Then EF Core migrations can automatically compute what insert, update or delete operations need to be applied when upgrading the database to a new version of the model.

-	__Query types:__ An EF Core model can now include query types. Unlike entity types, query types do not have keys defined on them and cannot be inserted, deleted or updated (i.e. they are read-only), but they can be returned directly by queries. Some of the usage scenarios for query types are:

    - Mapping to views without primary keys
    - Mapping to tables without primary keys
    - Mapping to queries defined in the model
    - Serving as the return type for FromSql() queries


-	__Include for derived types:__ It will be now possible to specify navigation properties only defined in derived types when writing expressions for the Include() methods. The syntax looks like this:

   ``` csharp
   var query = context.People.Include(p => ((Student)p).School);
   ```
-	__System.Transactions support:__ We have added the ability to work with System.Transactions features such as TransactionScope. This will work on both .NET Framework and .NET Core when using database providers that support it.

## Other improvements and new initiatives
Besides the major new features included in 2.1, we have made numerous smaller improvements and we have fixed more than a hundred product bugs. We also made progress on the following areas:

-	__Optimization of correlated subqueries:__ We have improved our query translation to avoid executing N + 1 SQL queries in many common scenarios in which a root query is joined with a correlated subquery.

-	__Column ordering in migrations:__ Based on customer feedback, we have updated migrations to initially generate columns for tables in the same order as properties are declared in classes.

-	__Cosmos DB provider preview:__ We have been developing an EF Core provider for the DocumentDB API in Cosmos DB. This is the first document database provider we have produced, and the learnings from this exercise are going to inform improvements in the design of the subsequent release after 2.1. The current plan is to publish an early preview of the Cosmos DB provider in the 2.1 timeframe.

-	__Sample Oracle provider for EF Core:__ We have produced a sample EF Core provider for Oracle databases. The purpose of the project is not to produce an EF Core provider owned by Microsoft, but to:

    1. Help us identify gaps in EF Core’s relational and base functionality which we need to address in order to better support Oracle.
    2. Help jumpstart the development of other Oracle providers for EF Core either by Oracle or third parties.    


  Note that currently our sample is based on the latest available ADO.NET provider from Oracle, which only supports .NET Framework. As soon as an ADO.NET provider for .NET Core is made available, we will consider updating the sample to use it.

## What's next

We will be releasing the first preview of EF Core 2.1, including all the features mentioned above, later this month. After that, we intend to release additional previews monthly, and a final release in the first half of 2018.

A big thank you to everyone that uses EF Core, and to everyone who has helped make the 2.1 release better by providing feedback, reporting bugs, and contributing code.
