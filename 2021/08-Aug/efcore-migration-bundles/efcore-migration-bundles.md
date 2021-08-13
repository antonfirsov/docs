---
post_title: 'Introducing DevOps-friendly EF Core Migration Bundles'
username: jeremy-likness
microsoft_alias: jeliknes
categories: .NET Core, Entity Framework, ASP.NET
summary: EF Core's new migration bundles generate binary artifacts that you can use to deploy schema and data changes as part of your continuous delivery process.
desired_publication_date: '2021-08-16'
---
Today, the EF Core team would like to introduce you to a new feature that shipped in our latest preview release: [migration bundles](https://github.com/dotnet/efcore/issues/19693).

Business applications evolve with time. The changes are reflected in your code, your schema, and your data. Updates to code can often be deployed simply by 
replacing binary files or pointing the load balancer to new nodes. On the other hand, updates that involve changes to the database are inevitably more complex. Not only 
do the code and data need to remain in synch, but alterations to schema definitions can cause side effects that ripple across tables. The data in existing columns may need to be
transformed or even transferred while the changes are applied, and modern applications are expected to run with minimal downtime.

A major benefit of using EF Core is the ability to manage schema changes through a mechanism called _[migrations](https://docs.microsoft.com/ef/core/managing-schemas/migrations/)_. A migration is essentially a mini-snapshot of a "point-in-time" of your database model. After making modifications to your model that may change the schema, you can use the [EF Core Command Line Interface (CLI)](https://docs.microsoft.com/ef/core/cli/dotnet) to capture the snapshot in a migration. The same tool can then be used to point a migration at an existing database and deploy the data definition language (DDL) necessary to bring the database in sync with the model. 

Migrations must be able to see the database model to work. When you create or apply a migration, the projects containing the migrations and the [`DbContext`](https://docs.microsoft.com/dotnet/api/microsoft.entityframeworkcore.dbcontext) definition are compiled and inspected. At a high level, migrations function in the following way:

- When a data model change is introduced, the developer uses EF Core tools to add a corresponding migration describing the updates necessary to keep the database schema in sync. EF Core compares the current model against a snapshot of the old model to determine the differences and generates migration source files; the files can be tracked in your project's source control like any other source file.
- Once a new migration has been generated, it can be applied to a database in various ways. EF Core records all applied migrations in a special history table, allowing it to know which migrations have been applied and which haven't.

Until now, there have traditionally been three ways developers apply migrations when new versions of their software are deployed.

## Scripting

One approach is to generate SQL scripts from the migrations using the EF Core tools. The benefit of this is that the script can be inspected and modified as necessary prior to deployment. The scripts are pure SQL and can be managed and deployed independently of EF Core, whether via an automated process or the manual intervention of a database administrator (DBA). By default, scripts are specific to the migration from which they were generated and assume you have applied previous changes. They must be run in sequence or the scripts may fail and produce unexpected side effects.

A second option is to generate _idempotent_ scripts. These are scripts that check for the existing version and apply multiple migrations as needed to make the database current. There are tradeoffs to both approaches, but the idempotent option is the safest approach to have a one-size-fits-all script.

## Command line interface (CLI)

It is possible to deploy updates directly using the command line tool. This comes with risk because the changes are deployed immediately without giving you the opportunity to inspect the generated migrations. This also requires the tool dependencies (.NET SDK, your source code to compile the model and the tool itself) to be installed on the production servers.

## Application startup

It is possible to run migrations as part of your application by calling the `Database.Migrate()` method. Although this may work, the approach is problematic. In distributed systems, if multiple nodes start at the same time, they may conflict with each other and try to upgrade simultaneously or migrate against a partially updated database. Modifying the schema requires the application to have elevated permissions, but granting those permissions is a security risk. As with the CLI approach, there is no opportunity to review the SQL prior to applying it.

## Introducing migration bundles

Scripting remains a viable option for migrations. For those who choose the code approach, and to mitigate some of the risks associated with the command line and application startup approaches, the EF Core team is happy to announce the preview availability of migration bundles in [EF Core 6.0 Preview 7](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/6.0.0-preview.7.21378.4). A migration bundle performs the same actions as the command line interface:

`dotnet ef database update`

The migration bundle is a self-contained executable with everything needed to run a migration. It accepts the connection string as a parameter. It is intended to be an artifact used in continouous deployment that works with all the major tools (Docker, SSH, PowerShell, etc.). It doesn't require you to copy source code or install the .NET SDK (only the runtime) and can be integrated as a deployment step in your DevOps pipeline. This also decouples the migration activity from your main application so there are no concerns about race conditions and no need to elevate the permissions of your main app. The vision is to have first class building blocks that use migration bundles available in DevOps toolsets like Visual Studio and GitHub Actions. For now, we provide the bundle and you are responsible to run it. 

### Get started

To work with bundles, you'll need at least the preview 7 version of tools. You can install it using this command:

`dotnet tool install --global dotnet-ef --version 6.0.0-preview.7.21378.4`

Do you already have the tool installed? No problem! Simply replace `install` with `update` to upgrade:

`dotnet tool update --global dotnet-ef --version 6.0.0-preview.7.21378.4`

From the command line, in the working directory of your project that contains the EF Core migrations, use:

`dotnet ef migrations bundle`

To generate the bundle. If you prefer the Visual Studio Package Manager Console, run this command:

`Bundle-Migration`

Either option will produce an artifact named `bundle` (i.e., `bundle.exe` on Windows machines). 

By default, the bundle will look for an `appSettings.json` to find the connection string. Simply run:

`./bundle.exe` 

To deploy your migrations. If you prefer to use an environment variable instead, simply pass the connection string using the `--connection` switch. For example:

`./bundle --connection {$ENVVARWITHCONNECTION}`

Obviously (we hope), it makes sense to deploy the bundle as part of your staging and test process prior to applying it to production. 

> **Note:** Migrations are EF Core's "best guess" about the changes required based on the schema and shape of the model. You should _always_ inspect the migration before applying it and update/customize as needed. For example, if you decide to split a `Name` column to `FirstName` and `LastName`, EF Core will generate the changes to drop one column and add the other two, but you will need to customize the migration to populate the new columns with data from the old ones. 

## Your feedback is important!

There is still work to do with bundles and we need your help. There are two main ways you can contribute to this feature:

1. Visit the [migration bundles issue](https://github.com/dotnet/efcore/issues/19693) to share feature requests and design needs so that we can incorporate the features you need to be successful.
2. Try out the bundles and [file any issues](https://github.com/dotnet/efcore/issues/) that you may encounter so we can address them before the final release.

Thank you for your continued support and we look forward to your feedback!

Sincerely,

Jeremy Likness on behalf of the EF Core team.
