---
post_title: Using PostgreSQL with .NET and Entra ID
author1: aapowell
post_slug: using-postgre-sql-with-dotnet-and-entra-id
microsoft_alias: aapowell
featured_image: using-postgre-sql-with-dotnet-and-entra-id.jpg
categories: .NET, Azure
tags: .net, entra id, azure, cloud, postgres, postgresql
ai_note: hide
summary: Getting started with .NET and PostgreSQL, and using Entra ID to secure your app.
post_date: 2024-06-13 10:05:00
---

[PostgreSQL](https://www.postgresql.org/) is a powerful, open-source relational database system that is widely used in the industry. In this blog post, we will explore how to use PostgreSQL with .NET and how to secure your app with Entra ID.

## Getting started with PostgreSQL

Let's start by creating a new application which is going to use PostgreSQL as the database. There are many ways we could setup PostgreSQL: we could install it, run it in a Docker container, but for this example, I'm going to bootstrap it with .NET Aspire and the [PostgreSQL hosting component][aspire-postgres]. While this does run a Docker container behind the scenes, it means we don't need to worry about setting up Docker or managing the secrets to connect to the database.

For this, we'll use the .NET Aspire starter application:

```bash
dotnet new aspire-starter --name PostgreSQLDemo
```

This will create a new .NET Aspire application with the AppHost, Service Defaults, an API backend and a Blazor web frontend.

Next, we'll add the PostgreSQL hosting component to the AppHost project:

```bash
cd PostgreSQLDemo.AppHost
dotnet add package Aspire.Hosting.PostgreSQL
```

The final piece for our AppHost is to create the PostgreSQL resource and then add it as a reference to the API project. Open the `Program.cs` file in the AppHost project and update it to match the following:

```csharp
var builder = DistributedApplication.CreateBuilder(args);

// Create the PostgreSQL resource
var postgres= builder.AddPostgres("postgres");
var db = postgres.AddDatabase("db");

// Update the API Service project to have the PostgreSQL database as a reference
var apiService = builder.AddProject<Projects.PostgreSQLDemo_ApiService>("apiservice")
                    .WithReference(db);

builder.AddProject<Projects.PostgreSQLDemo_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();
```

There are two changes that we've made here. First, we've added the PostgreSQL resource to the AppHost project. This will create a new PostgreSQL database and user for the application, running in a container, when we launch our project. Second, we've added the database as a reference to the API project. This will allow the API project to connect to the database.

## Adding PostgreSQL to the API

Now that we are starting a PostgreSQL resource in our AppHost, we can add the necessary code to the API project to interact with the database. We'll start by adding the [Npgsql][npgsql] package to the API project, but we'll use the .NET Aspire package, which ensures that we get all the logging and telemetry that we need:

```bash
cd ../PostgreSQLDemo.ApiService
dotnet add package Aspire.Npgsql
```

> Note: If you are using EF Core, you can use the `Aspire.Npgsql.EntityFrameworkCore.PostgreSQL` package instead.

Next, we'll add Npgsql to our service collection, so we can resolve it with Dependency Injection. Open the `Program.cs` file in the API project and update it to match the following:

```csharp
builder.AddNpgsqlDataSource("db");
```

Make sure that the value you provide as the `connectionName` is the same as the name you used when you added the database reference in the AppHost project, this way the right connection string will be resolved.

With all that in place, you can now use Npgsql in your API project to interact with the PostgreSQL database.

## Securing the database in Azure with Entra ID

So far, we've focused on the local development experience. For production, we'll want to utilize Azure PostgreSQL Flexible Server for a managed instance. To enhance application security, we will employ Entra ID, creating a Managed Identity for database authentication instead of using a SQL username and password.

> Note: Setting up an Azure PostgreSQL Flexible Server and configuring it with Entra ID is beyond the scope of this article, but do check out [the docs][postgres-entra] for information on that.

Using Managed Identity differs from traditional SQL username and password authentication because it does not require a persistent password. Instead, you need to request an access token that acts in place of a persistent password, and this access token is only short lived, which means we're going to need to request a new one from time to time.

### Getting an access token

To get an access token, we'll use the `Azure.Identity` package, which is part of the Azure SDK. We'll add this package to the API project:

```bash
cd ../PostgreSQLDemo.ApiService
dotnet add package Azure.Identity
```

With the `Azure.Identity` package, we can use the `DefaultAzureCredential` class to load the Managed Identity from the environment, and then use it to request an access token. Here's an example of how you would do that:

```csharp
var credentials = new DefaultAzureCredential();
var token = await credentials.GetTokenAsync(new TokenRequestContext(["https://ossrdbms-aad.database.windows.net/.default"]), CancellationToken.None);
Console.WriteLine(token.Token);
```

That's great to get a token one-time, but we'll need to refresh it, and to do that we can leverage a feature of Npgsql, the `PeriodicPasswordProvider`.

### Using PeriodicPasswordProvider

The `PeriodicPasswordProvider` is a feature of Npgsql that allows you to provide a callback that will be called whenever Npgsql needs a password. This is perfect for our use case, as we can use it to request a new access token whenever Npgsql needs it. It will also cache that token for a period of time, so we don't need to request a new one every time.

To use the `PeriodicPasswordProvider`, we'll adjust the registration of Npgsql in the service collection to use it. Open the `Program.cs` file in the API project and update it to match the following:

```csharp
builder.AddNpgsqlDataSource("db", configureDataSourceBuilder: (dataSourceBuilder) =>
{
    if (string.IsNullOrEmpty(dataSourceBuilder.ConnectionStringBuilder.Password))
    {
        dataSourceBuilder.UsePeriodicPasswordProvider(async (_, ct) =>
        {
            var credentials = new DefaultAzureCredential();
            var token = await credentials.GetTokenAsync(new TokenRequestContext(["https://ossrdbms-aad.database.windows.net/.default"]), ct);
            return token.Token;
        }, TimeSpan.FromHours(24), TimeSpan.FromSeconds(10));
    }
});
```

Here, we're providing a `configureDataSourceBuilder` callback to the `AddNpgsqlDataSource` method, which allows us to configure the `DataSourceBuilder` that Npgsql will use. We're checking if the password is empty, which means that Npgsql is expecting a password. If so, we're setting up the `PeriodicPasswordProvider` to cache the password for 24 hours and to retry every 10 seconds in case of failure. When running locally with .NET Aspire, the password will be set, so we do not need to use the `PeriodicPasswordProvider` because we have a known and consistent password.

## Next Steps

In this blog post, we've looked at how to get started with PostgreSQL in .NET, how we can easily setup a local development experience with .NET Aspire, and how, when deployed to Azure, we can secure our application with Entra ID using the `PeriodicPasswordProvider` in Npgsql to request access tokens for Managed Identity.

- [.NET Aspire PostgreSQL component][aspire-postgres]
- [Configure Azure PostgreSQL Flexible Server with Entra][postgres-entra]
- [Npgsql documentation][npgsql]

[aspire-postgres]: https://learn.microsoft.com/dotnet/aspire/database/postgresql-component?tabs=dotnet-cli
[postgres-entra]: https://learn.microsoft.com/azure/postgresql/flexible-server/how-to-configure-sign-in-azure-ad-authentication
[npgsql]: https://www.npgsql.org/