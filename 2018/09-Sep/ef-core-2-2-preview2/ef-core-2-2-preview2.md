# Announcing Entity Framework Core 2.2 Preview 2 and the preview of the Cosmos DB provider and spatial extensions for EF Core

Today we are making EF Core 2.2 Preview 2 available, together with a preview of our data provider for Cosmos DB and spatial extensions for the EF Core SQL Server and in-memory providers.

## Obtaining the preview
The preview bits are available on NuGet, and also as part of [ASP.NET Core 2.2 Preview 2](tbd) and the [.NET Core SDK 2.2 Preview 2](tbd), also releasing today.
If you are working on an application based on ASP.NET Core, we recommend you upgrade to ASP.NET Core 2.2 Preview 2 following the recommended steps in the [announcement](tbd).
The SQL Server and the in-memory providers are included in ASP.NET Core.
For other providers, you will need to install the corresponding NuGet package as described next.

To add, for example the 2.2 Preview 2 version of the SQL Server provider in a .NET Core library or application from the command line, use:

``` console
dotnet add package Microsoft.EntityFrameworkCore.SqlServer -v 2.2.0-preview2-tbd  
```  

Or from the Package Manager Console in Visual Studio:

``` console
PM> Install-Package Microsoft.EntityFrameworkCore.SqlServer -Version 2.2.0-preview2-tbd
```

The Cosmos DB provider and the spatial extensions ship as separate new NuGet packages. We'll explain how to get started with them in the respective feature descriptions.

For a more detailed explanation see [our documentation on Installing Entity Framework Core](https://docs.microsoft.com/ef/core/get-started/install/).

## What is new in this preview?
As we explained in [our roadmap annoucement](https://github.com/aspnet/Announcements/issues/308) back in June, there will be a large number of bug fixes (you can see the list of issues we have fixed so far [here](https://github.com/aspnet/EntityFrameworkCore/issues?q=is%3Aissue+milestone%3A2.2.0+is%3Aclosed+label%3Aclosed-fixed) but only a relatively small number of major new features in EF Core 2.2.

Here are the most important new features:

### New EF Core provider for Cosmos DB
TO-DO
#### Limitations
TO-DO

## Spatial extensions for SQL Server and in-memory
TO-DO
### Limitations
TO-DO

> **Note: Spatial support and the Cosmos DB provider are large features that expose a lot of new capabilities and APIs.
In order to make sure we get them right, we need to go through an iterative process in which your feedback is critical.
On the other hand, the release schedule of EF Core 2.2 is tied other important products.
If by the time we ship EF Core 2.2 we have reasons to believe that the spatial extensions or the Cosmos DB provider aren't ready, we will consider delaying them to a later time.**

### Collections of owned entities
TO-DO

### Tagged queries
TO-DO

## Provider compatibility
Although we have setup testing to make sure that existing providers will continue to work with EF Core 2.2, there might be unexpected problems, and we welcome users and provider writers to report compatibility issues on [our issue tracker](https://github.com/aspnet/EntityFrameworkCore/issues/new).

## What comes next?

We are still working in some additional features we would like to include in EF Core 2.2, like reverse engineering of database views into query types, support for spatial types with SQLite, as well as additional bug fixes.
We are planning on releasing EF Core 2.2 in the last calendar quarter of 2018.

Our team is also working on the our next major release, EF Core 3.0, which will include significant improvements to our LINQ implementation, and making Entity Framework 6 compatible with .NET Core 3.0.  

## Thank you!

We encourage you to try the new features and we thank you in advance for posting any feedback to [our issue tracker](https://github.com/aspnet/EntityFrameworkCore/issues/new).
