# A new SqlClient

Those of you who have been following .NET development closely have very likely seen Scott Hunter's latest blog post [.NET Core is the Future of .NET](https://devblogs.microsoft.com/dotnet/net-core-is-the-future-of-net/). The change in focus of .NET Framework towards stability and new feature development moving to .NET Core means SQL Server needs to change in order to continue to provide the latest SQL features to .NET developers in the same timely manner that we have done in the past.
 
System.Data.SqlClient is the ADO.NET provider you use to access SQL Server or Azure SQL Databases. Historically SQL has used System.Data.SqlClient in .NET Framework as the starting point for client-side development when proving our new SQL features and then propagating those designs to other drivers. We would still like to do this going forward but at the same time add those same new features in .NET Core, too. 

Right now, we have two code bases and two different ways SqlClient is delivered to an application. In .NET Framework, versions are installed globally in Windows. In .NET Core, an application can pick a specific SqlClient version and ship with that. Wouldn't it be nice if the .NET Core model of SqlClient delivery worked for Framework, too?
 
We couldn't just ship a new package that replaces System.Data.SqlClient. That would conflict with what lives inside .NET Framework now. That brings us to our chosen solution...

## Introducing Microsoft.Data.SqlClient
 
The Microsoft.Data.SqlClient package, which is [now available in preview on NuGet](https://www.nuget.org/packages/Microsoft.Data.SqlClient/1.0.19123.2-Preview), will be the new flagship data access driver for SQL Server. 

This new package supports both .NET Framework and .NET Core. Creating a new SqlClient in a new namespace allows both the old System.Data.SqlClient and new Microsoft.Data.SqlClient to live side-by-side. While not automatic, there is a pretty straightforward path for applications to move from the old to the new. Simply add a NuGet dependency on Microsoft.Data.SqlClient and update any using references or qualified references.

In keeping with our plans to accelerate feature delivery in this new model, we are happy to offer support for two new SQL Server features on both .NET Framework and .NET Core, along with bug fixes and performance improvements:
 
- Data Classification - Available in Azure SQL Database and Microsoft SQL Server 2019 since CTP 2.0.
- UTF-8 support - Available in Microsoft SQL Server SQL Server 2019 since CTP 2.3.

Likewise, we have updated the .NET Core version of the provider with the long awaited support for Always Encrypted, including support for Enclaves:

- Always Encrypted is available in Microsoft SQL Server 2016 and higher. 
- Enclave support was introduced in Microsoft SQL Server 2019 CTP 2.0.

The binaries in the new package are based on the same code from System.Data.SqlClient in .NET Core and .NET Framework. This means there are multiple binaries in the package. In addition to the different binaries you would expect for supporting different operating systems, there are different binaries when you target .NET Framework versus when you target .NET Core. There was no magic code merge behind the scenes: we still have divergent code bases from .NET Framework and .NET Core (for now). This also means we still have divergent feature support between SqlClient targeting .NET Framework and SqlClient targeting .NET Core. If you want to migrate from .NET Framework to .NET Core but you are blocked because .NET Core does not yet support a feature, the first preview release may not change that. But our top priority is bringing feature parity across those targets. 

## What is the roadmap for Microsoft.Data.SqlClient?

Our roadmap has more frequent releases lighting up features in Core as fast as we can implement them. The long-term goal is a single code base and it will come to that over time, but the immediate need is feature support in SqlClient on .NET Core, so that is what we are focusing on, while still being able to deliver new SQL features to .NET Framework applications, too.

|                     Feature Roadmap                      |               Engineering Roadmap               |
| -------------------------------------------------------- | ----------------------------------------------- |
| -	Azure Active Directory authentication providers (Core) | - Merge .NET Framework and .NET Core code bases |
| - Active Directory Password                              | - Open source assembly                          |
| - Managed Service Identity                               | - Move to GitHub                                |
| - Active Directory  Integrated                           |                                                 |

While we do not have dates for the above features, our goal is to have multiple releases throughout 2019. We anticipate Microsoft.Data.SqlClient moving from Preview to GA sometime prior to the RTM release of both SQL Server 2019 and .NET Core 3.0. 

## What does this mean for System.Data.SqlClient?
 
It means the development focus has changed. We have no intention of dropping support for System.Data.SqlClient any time soon. It will remain as-is and we will fix important bugs and security issues as they arise. If you have a typical application that doesn't use any of the newest SQL features, then you will still be well served by a stable and reliable System.Data.SqlClient for many years. 

However, Microsoft.Data.SqlClient will be the only place we will be implementing new features going forward. We would encourage you to evaluate your needs and options and choose the right time for you to migrate your application or library from System.Data.SqlClient to Microsoft.Data.SqlClient.

## Closing

We would like to encourage you to try the preview bits by installing the Microsoft.Data.SqlClient package. We want to hear from you! Although we haven't finished preparing the source code for publishing, you can already use the issue tracker at https://github.com/dotnet/SqlClient to report any issues. 

Keep in mind that object-relational mappers such as EF Core, EF 6, or Dapper, and  other non-Microsoft libraries, haven't yet made the transition to the new provider, so you won't be able to use the new feature through any of these libraries. Updated versions of EF Core with support for Microsoft.Data.SqlClient are expected in an upcoming preview.  

We also encourage you to visit our [Frequently Asked Questions](https://github.com/dotnet/SqlClient/wiki/Frequently-Asked-Questions) and [Release Notes](https://github.com/dotnet/SqlClient/blob/master/release-notes/1.0/1.0.0.md) pages in our GitHub repos. They contain additional information about the features available and our plans for the release. 

_This post was written by Vicky Harp, Program Manager on SqlClient and SQL Server Tools._
