---
post_title: A Comprehensive Guide to Port from EF6 to EF Core
username: jeremylikness
microsoft_alias: jeliknes
featured_image: porting.png
categories: .NET Core, Entity Framework
summary: Get detailed guidance for porting your EF6 apps to EF Core.
desired_publication_date: 2022-01-03
---

EF6 was officially released nearly a decade ago in October 2013. The next major Entity Framework release was in June of 2016 when EF Core 1.0 was introduced as a complete rewrite for the modern .NET Core platform. One final, major update to EF6 happened with the release of a new version compatible with .NET Core. The latest version, capable of running on both the .NET Framework and .NET Core, was intended to make it easier for customers to transition to the .NET Core platform. With the option of EF6 on .NET Core, customers can move their existing EF6 codebase onto .NET Core first before migrating their EF6 code to EF Core. Although the initial versions of EF Core lacked support for many popular EF6 capabilities, the gap has narrowed significantly with every release. 

There are now more reasons than ever before to make the EF Core upgrade. With EF Core, you can:

- Take advantage of dozens of features exclusive to EF Core, like support for constructors with parameters, mapped types with no keys, alternate keys, simple logging configuration, minimal API support, split queries and "shadow" properties. For the full list of features compared to EF6, see [Compare EF6 and EF Core](https://docs.microsoft.com/ef/efcore-and-ef6/).
- Centralize business logic and support scenarios like multi-tenancy and "soft delete" with global query filters.
- Give your apps a performance boost by taking advantage of the ongoing performance improvements to EF Core, including the 92% improvement in requests per second for EF Core 6 on .NET 6 compared to EF Core 5 on .NET 5 based on the industry standard TechEmpower Fortunes benchmark.
- Connect to a variety of different database backends from Sqlite and MySQL to SQL Server and Azure Cosmos DB.

The Entity Framework team understands that porting your apps from EF6 to EF Core is not always straightforward. To make it easier to successfully plan and execute the upgrade to EF Core, we completely rewrote our guidance for [Porting from EF6 to EF Core](https://docs.microsoft.com/ef/efcore-and-ef6/porting/).

The new guide recognizes that there are many approaches to using and integrating EF6 that must be addressed for a port to be successful. For example, there are different steps provided for customers who prefer to reverse engineer their database compared to customers who follow a code-first approach. The guide breaks down differences in support for connection strings, explains how model building and change tracking work in EF Core, and lays out the general steps you'll follow to port your code.

Did you know it is possible to run EF6 and EF Core side-by-side, and that you can port sections of your app over time rather than planning for an "all-or-nothing" transition?
Are you aware that you can visualize your models despite the lack of EDMX designer support and even manage migrations from within Visual Studio using free open source community tools?
Do you understand the differences in how inheritance is configured and used between versions?

The answers to these questions and many more are all available in the comprehensive guide to [Porting from EF6 to EF Core](https://docs.microsoft.com/ef/efcore-and-ef6/porting/). 

We are here to help. If you encounter an issue during your upgrade, find issues in the guide or have questions and/or suggestions, simply [file an issue](https://github.com/dotnet/EntityFramework.Docs/issues/new) in our [documentation repo](https://github.com/dotnet/EntityFramework.Docs) and the Entity Framework team will respond!
