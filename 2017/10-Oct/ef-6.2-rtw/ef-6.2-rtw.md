# Entity Framework 6.2 runtime released

Today we announce the availability of EF 6.2 runtime in [NuGet.org](https://www.nuget.org/packages/EntityFramework/6.2.0).

Entity Framework (EF) is Microsoft's traditional object/relational mapper (O/RM) for .NET Framework. To understand the difference between EF6 and EF Core, please refer to [our documentation](https://docs.microsoft.com/en-us/ef/efcore-and-ef6/).

## How to obtain the new package

You can install EF 6.2 either using the "Manage NuGet Packages" option in Visual Studio or by issuing commands in the "NuGet Package Manager Console".
For example, to upgrade an existing project to use EF 6.2, you can run:
``` console
PM> Update-Package EntityFramework -Version 6.2.0
```

Or to  install the package in a new project:

``` console
PM> Install-Package EntityFramework -Version 6.2.0
```
## What is new in EF 6.2?

While most of the focus of the EF team is nowadays on adding new features and improvements to [EF Core](https://docs.microsoft.com/en-us/ef/core/index), we plan to keep fixing important bugs, implementing small improvements, and incorporating community contributions in the EF 6 codebase.

To a great extent, it is thanks to the efforts our community of open source contributors that EF 6.2 includes numerous [bugs fixes](https://github.com/aspnet/entityframework6/issues?utf8=%E2%9C%93&q=is%3Aissue%20milestone%3A6.2.0%20is%3Aclosed%20label%3Aclosed-fixed%20-label%3Aarea-tools%20label%3Atype-bug) and [product enhancements](https://github.com/aspnet/entityframework6/issues?utf8=%E2%9C%93&q=is%3Aissue%20milestone%3A6.2.0%20is%3Aclosed%20label%3Aclosed-fixed%20-label%3Aarea-tools%20label%3Atype-enhancement%20).

The most important changes were already detailed in the [beta 1 announcement](https://blogs.msdn.microsoft.com/dotnet/2017/05/23/announcing-ef-6-2-beta-1/) last May. Here is a brief list of the most important changes affecting the runtime:

- Reduce start up time by loading finished code first models from a persistent cache [#275](https://github.com/aspnet/EntityFramework6/issues/275)
- Fluent API to define indexes [#274](https://github.com/aspnet/EntityFramework6/issues/274)
- DbFunctions.Like() to enable writing LINQ queries that translate to LIKE in SQL [#241](https://github.com/aspnet/EntityFramework6/issues/241)
- Migrate.exe should support -script option [#240](https://github.com/aspnet/EntityFramework6/issues/240)
- EF6 does not work with primary key from sequence [#165](https://github.com/aspnet/EntityFramework6/issues/165)
- Update error numbers for SQL Azure Execution Strategy [#83](https://github.com/aspnet/EntityFramework6/issues/83)
- Bug: Retrying queries or SQL commands fails with "The SqlParameter is already contained by another SqlParameterCollection" [#81](https://github.com/aspnet/EntityFramework6/issues/81)
- Bug: Evaluation of DbQuery.ToString() frequently times out in the debugger [#73](https://github.com/aspnet/EntityFramework6/issues/73)

## Where are the EF 6.2 tools for Visual Studio?

We decided that we still needed to complete [some work](https://github.com/aspnet/EntityFramework6/issues?q=is%3Aopen+is%3Aissue+milestone%3A6.2.0+label%3Aarea-tools) on the tools before we could declare them "final".
However, it did not make sense to delay the release of the runtime packages:
- Since the release of the beta last May, we have received enough feedback to validate the release, and only minor adjustments have been necessary.
- The EF 6.2 runtime is fully compatible with released versions of our Visual Studio tools. You should only need to manually upgrade the NuGet packages in your applications.

Once we finish the work on the tools, we will include them in an upgrade of Visual Studio 2017 as well as in downloadable installers for previous versions.

## The future of EF Power Tools

Last July, one of our main contributors, [Erik Ejlskov Jensen](https://github.com/ErikEJ), decided to pursue future development of the EF Power Tools, creating his own fork labeled "Community Edition". The source code is available at https://github.com/ErikEJ/EntityFramework6PowerTools and the latest released version of the Visual Studio extension is available in the [Visual Studio Marketplace](https://marketplace.visualstudio.com/items?itemName=ErikEJ.EntityFramework6PowerToolsCommunityEdition). We encourage you to try this version if you enjoy the features provided by the project, especially if you need them on newer versions of Visual Studio.

## What is next?

Besides releasing the 6.2 versions of the Visual Studio tools, we have a few more things planned for EF6 in the short and medium term. Among others:

- Porting the EF 6 documentation to the [Microsoft Docs platform](https://docs.microsoft.com/en-us/ef/ef6/)
- Improve our support for [NuGet's PackageReference](https://docs.microsoft.com/en-us/nuget/consume-packages/package-references-in-project-files)

As always, you are welcome to provide feedback as well as to contribute in any way in our [GitHub project](https://github.com/aspnet/entityframework6/).

## Thank you!

Once more, we would like to take the chance to thank the members of the .NET development community who contributed to this release, in particular those who made code contributions (in alphabetical order, by they GitHub aliases): [@axelheer](https://github.com/axelheer), [@bengutt](https://github.com/bengutt), [@brandondahler](https://github.com/brandondahler), [@ErikEJ](https://github.com/ErikEJ), [@jcchalte](https://github.com/jcchalte), [@j-Edge](https://github.com/j-Edge), [@julielerman](https://github.com/julielerman), [@montanehamilton](https://github.com/montanehamilton), [@Sebazzz](https://github.com/Sebazzz).
