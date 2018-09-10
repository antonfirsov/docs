# Announcing EF Core 2.2 Preview 2 and the preview of the Cosmos DB provider and spatial extensions for EF Core

Today we are happy to make EF Core 2.2 Preview 2 available, together with a preview of our data provider for Cosmos DB and spatial extensions for the SQL Server and SQLite providers.

## EF Core 2.2 Preview 2
There are many bug fixes but only a relatively small number of new features planned for EF Core 2.2.
You can obtain the new packages in NuGet, as part of the [ASP.NET Core 2.2 Preview 2](tbd) or with the preview version of the [.NET Core SDK](tbd).

To add for example the 2.2 Preview 2 version of the SQL Server provider in a .NET Core library or application from the command line, use:  

``` console
dotnet add package Microsoft.EntityFrameworkCore.SqlServer -v 2.2.0-preview2-tbd  
```  

Or from the Package Manager Console in Visual Studio:

``` console
PM> Install-Package Microsoft.EntityFrameworkCore.SqlServer -Version 2.2.0-preview2-tbd
```

### What is new in this preview?

- Collections of owned entities

- Tagged queries

## New EF Core provider for Cosmos DB
TO-DO
### Get started
TO-DO
### Limitations
TO-DO

## New EF Core spatial extensions for SQL Server and SQLite
TO-DO
### Get started
TO-DO
### Limitations
TO-DO

## What comes next?

We are still working in some additional features for EF Core 2.2, like reverse engineering of views into query types, as well as additional bug fixing.

A lot of our efforts are going currently going into our next major release, EF Core 3.0, including improvements in our LINQ implementation.

As we explained in [our roadmap annoucement](https://github.com/aspnet/Announcements/issues/308) back in June, we are planning on releasing EF Core 2.2 in the last calendar quarter of 2018, and EF Core 3.0 next year.

For the Cosmos DB provider and Spatial extensions, the release dates are not currently tied to either 2.2 or 3.0, but will depend on when we believe we have achieved the quality and feature set.

## Thank you!

We want to encourage to try these bits and to thank you on advance for posting any feedback to [our issue tracker](https://github.com/aspnet/entityframeworkcore/issues/new).
