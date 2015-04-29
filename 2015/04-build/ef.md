Entity Framework
================

The Entity Framework has two separate EF versions that they are working on. EF 6.x is the mature version that everyone is using. They also have the EF 7.x version, which is the next major release and is still under active development and in preview. You can learn more by reading today's announcement post from the EF team.

Entity Framework 6.1.3
----------------------

EF6.1.3 is the latest stable version of Entity Framework and is the recommended version for production applications. EF6.1.3 is a patch release containing fixes for high priority issues that were reported on EF6.1.2. Visual Studio 2015 RC includes the RTM version of Entity Framework 6.1.3 runtime and tooling.

The EF runtime will be installed if you create a new model using the Entity Framework Tools in a project that does not already have the EF runtime installed.

The runtime is pre-installed in new ASP.NET projects, depending on the project template you select.
The EF6.1.3 Tools for Visual Studio 2015 are included to make sure you get the latest bug fixes and improvements.

Entity Framework 7 Beta 4
-------------------------

EF7 introduces some significant changes and improvements over EF6.x and therefore the pre-release phase of EF7 is much longer than other recent releases. We’ve made significant progress since our last pre-release, but if you decide to try out EF7 then please bear in mind that this preview is designed to give you an idea of what the experience will be like and there are still a number of limitations and missing features that will be addressed before RTM.

EF7 can be used in .NET Framework, .NET Core (including ASP.NET 5) and Mono apps.

Here is a rough guide to what currently works in Beta 4. Most of these features are a work-in-progress and still have limitations.

- Basic modeling including built-in conventions, table/column mapping, and relationships
- Change tracking
- LINQ queries
- Table based Insert/Update/Delete (including batching)
- Migrations and database creation/deletion
- Transactions (including automatic transactions during SaveChanges and explicit transaction APIs)
- Identity and Sequence patterns for database generated key values
- Raw SQL commands
- An early preview of reverse engineering a model from a database
- Logging
- Unique constraints including the ability to use them as keys in a relationship