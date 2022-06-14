---
post_title: ASP.NET Core updates in .NET 7 Preview 5
author1: daroth@microsoft.com
post_slug: asp-net-core-updates-in-dotnet-7-preview-5
username: daroth@microsoft.com
microsoft_alias: daroth
featured_image: dotnet-bot_scene_juggling.png
categories: .NET, ASP.NET Core, Blazor
summary: Summary of your post, shown on the home page next to the featured image
desired_publication_date: 2022-06-14
---

[.NET 7 Preview 5 is now available](https://devblogs.microsoft.com/dotnet/announcing-dotnet-7-preview-5) and includes many great new improvements to ASP.NET Core.

Here's a summary of what's new in this preview release:

- JWT authentication improvements & automatic authentication configuration
- Minimal API's parameter binding support for argument list simplification

For more details on the ASP.NET Core work planned for .NET 7 see the full [ASP.NET Core roadmap for .NET 7](https://aka.ms/aspnet/roadmap) on GitHub.

## Get started

To get started with ASP.NET Core in .NET 7 Preview 5, [install the .NET 7 SDK](https://dotnet.microsoft.com/download/dotnet/7.0).

If you're on Windows using Visual Studio, we recommend installing the latest [Visual Studio 2022 preview](https://visualstudio.com/preview). Visual Studio for Mac support for .NET 7 previews isn't available yet but is coming soon.

To install the latest .NET WebAssembly build tools, run the following command from an elevated command prompt:

```sh
dotnet workload install wasm-tools
```

> Note: Building .NET 6 Blazor projects with the .NET 7 SDK and the .NET 7 WebAssembly build tools is currently not supported. This will be addressed in a future .NET 7 update: [dotnet/runtime#65211](https://github.com/dotnet/runtime/issues/65211).

## Upgrade an existing project

To upgrade an existing ASP.NET Core app from .NET 7 Preview 4 to .NET 7 Preview 5:

- Update all Microsoft.AspNetCore.\* package references to `7.0.0-preview.5.*`.
- Update all Microsoft.Extensions.\* package references to `7.0.0-preview.5.*`.

See also the full list of [breaking changes](https://docs.microsoft.com/dotnet/core/compatibility/7.0#aspnet-core) in ASP.NET Core for .NET 7.

## JWT authentication improvements & automatic authentication configuration

Configuring authentication (AuthN) and authorization (AuthZ) for an ASP.NET Core app today requires numerous changes, including adding and configuring services, and adding middleware at different stages of the app startup process. We've received feedback that users find configuring authentication and authorization one of the hardest things about building APIs with ASP.NET Core. Given how critically important correctly configuring authentication and authorization are to securing web apps, we've made some improvements aimed at simplifying the most common aspects of this area for ASP.NET Core, with an initial focus on JWT bearer authentication, which is commonly used to protect web APIs.

### Simplified authentication configuration

Authentication options can now be automatically configured directly from the app's configuration system, due to the addition of a default configuration section when configuring authentication via the new `Authentication` property on `WebApplicationBuilder` like so:

```csharp
var builder = WebApplication.Create(args);

builder.Authentication.AddJwtBearer(); // <-- New top-level property for setting up authentication

var app = builder.Build();
```

This new property provides a central place in your app's code to setup authentication, providing easy access to the `AuthenticationBuilder` instance from which authentication schemes can be added and configured. Setting up authentication via this new property will also take care of automatically adding the required middleware to the request pipeline, similar to how `WebApplicationBuilder` already does this for routing.

Here's an example of an app setup to use JWT bearer authentication with two endpoints, one that requires authorization and one that doesn't:

```csharp
var builder = WebApplication.Create(args);

builder.Authentication.AddJwtBearer();

var app = builder.Build();

app.MapGet("/", () => "Hello, World!");
app.MapGet("/secret", (ClaimsPrincipal user) => $"Hello {user.Identity?.Name}. This is a secret!")
    .RequireAuthorization();

app.Run();
```

Furthermore, individual authentication schemes can have their options be automatically set from the app's configuration, making it easier to configure them between different environments (e.g. local development vs. production). For this release, only the JWT bearer scheme has been updated to support this mechanism but we'll update more authentication schemes to support this in the future.

Here's an example of an app's `appsettings.Development.json` file, updated to set authentication options via the new `"Authentication"` section:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "Authentication": {
    "DefaultScheme" : "JwtBearer",
    "Schemes": {
      "JwtBearer": {
        "Audiences": [ "http://localhost:5000", "https://localhost:5001" ],
        "ClaimsIssuer": "dotnet-user-jwts"
      }
    }
  }
}
```

Note that authentication schemes must still be added by code for them to have their options set via the new configuration section.

### Endpoint-specific authorization policies

The previous example included an endpoint definition that only allowed authenticated users to access it (`.RequireAuthorization()`). But what if the endpoint's authorization requirements are slightly more complex, e.g. only allowing users with a specific "scope" claim? A set of authorization requirements is defined in a "policy" which is normally defined globally as part of setting up authorization in the app's services and then referred to by its name when configuring the endpoint. This is good for reuse but can be difficult to discover and overly complex for some scenarios.

For cases where an authorization policy doesn't need to be shared between endpoints, you can now easily define an authorization policy directly on an endpoint via metadata like so:

```csharp
app.MapGet("/special-secret", () => "This is a special secret!")
    .RequireAuthorization(p => p.RequireClaim("scope", "myapi:secrets"));
```

Now that we have endpoints protected by JWT authentication, it would be nice to easily verify that they're configured correctly in a local development environment, without the need for a full identity and user management service. For that, we'll need something to issue JWTs to use with our app locally, which is the job of the new `dotnet user-jwts` command line tool.

### Managing development-time JWTs with `dotnet user-jwts`

If we try to access the protected endpoints from our previous examples using a tool like [Postman](https://www.postman.com/), [curl](https://curl.se/), or [`dotnet httprepl`](https://docs.microsoft.com/aspnet/core/web-api/http-repl/?view=aspnetcore-6.0&tabs=windows), we'll receive an error in the form of a response with an HTTP 401 (unauthorized) status code, indicating that the request didn't provide any authentication details and thus is unauthorized to access that resource:

```sh
MyWebApi$ curl -i http://localhost:5000
HTTP/1.1 200 OK
Content-Type: text/plain; charset=utf-8
Date: Tue, 07 Jun 2022 23:39:10 GMT
Server: Kestrel
Transfer-Encoding: chunked

Hello, World!
MyWebApi$ curl -i http://localhost:5000/secret
HTTP/1.1 401 Unauthorized
Content-Length: 0
Date: Tue, 07 Jun 2022 23:38:07 GMT
Server: Kestrel
WWW-Authenticate: Bearer
MyWebApi$
```

As the app is configured to use JWT bearer authentication, we need to provide a JWT with the request. In fully deployed systems, the JWT would typically be provided by a server acting as a Security token service (STS), perhaps in response to logging in via a set of credentials. But for the purpose of working with our API during local development, we can use the new `dotnet user-jwts` command line tool to create and manage app-specific local JWTs.

The `user-jwts` tool is similar in concept to the existing `user-secrets` tools, in that it can be used to manage values for the app that are only valid for the current user (the developer) on the current machine. In fact, the `user-jwts` tool utilizes the `user-secrets` infrastructure to manage the key that the JWTs will be signed with, ensuring it's stored safely in the user profile.

First, we have to initialize the user secrets system for our project by calling `dotnet user-secrets init` and `dotnet user-secrets list` (note this will be done for you automatically by `dotnet user-jwts` in a future preview release):

```sh
MyWebApi$ dotnet user-jwts create
Project does not contain a user secrets ID.
MyWebApi$ dotnet user-secrets init
Set UserSecretsId to 'b78e7e01-e648-421a-9ccd-e1a31dd58529' for MSBuild project 'MyWebApi.csproj'. 
MyWebApi$ dotnet user-secrets list
No secrets configured for this application.
```

Now we can use the `user-jwts` tool to create a JWT to use with our example app:

```sh
MyWebApi$ dotnet user-jwts create
New JWT saved with ID '643a8abc'.
MyWebApi$ dotnet user-jwts print 643a8abc --show-full
Found JWT with ID 'b0498b94'
{
  "Id": "b0498b94",
  "Scheme": "Bearer",
  "Name": "damia",
  "Audience": "https://localhost:7188",
  "NotBefore": "2022-06-08T00:03:31+00:00",
  "Expires": "2022-09-08T00:03:31+00:00",
  "Issued": "2022-06-08T00:03:31+00:00",
  "Token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImRhbWlhIiwic3ViIjoiZGFtaWEiLCJqdGkiOiJiMDQ5OGI5NCIsImF1ZCI6WyJodHRwczovL2xvY2FsaG9zdDo3MTg4IiwiaHR0cDovL2xvY2FsaG9zdDo1MDU2Il0sIm5iZiI6MTY1NDY0NjYxMSwiZXhwIjoxNjYyNTk1NDExLCJpYXQiOjE2NTQ2NDY2MTEsImlzcyI6ImRvdG5ldC11c2VyLWp3dHMifQ.4lS34bXQdmubMf7JIpa6kSraVPpIe9nA-2Ptni2GdMM",
  "Scopes": [],
  "Roles": [],
  "CustomClaims": {}
}
Token Header: {"alg":"HS256","typ":"JWT"}
Token Payload: {"unique_name":"damia","sub":"damia","jti":"b0498b94","aud":["https://localhost:7188","http://localhost:5056"],"nbf":1654646611,"exp":1662595411,"iat":1654646611,"iss":"dotnet-user-jwts"}
Compact Token: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImRhbWlhIiwic3ViIjoiZGFtaWEiLCJqdGkiOiJiMDQ5OGI5NCIsImF1ZCI6WyJodHRwczovL2xvY2FsaG9zdDo3MTg4IiwiaHR0cDovL2xvY2FsaG9zdDo1MDU2Il0sIm5iZiI6MTY1NDY0NjYxMSwiZXhwIjoxNjYyNTk1NDExLCJpYXQiOjE2NTQ2NDY2MTEsImlzcyI6ImRvdG5ldC11c2VyLWp3dHMifQ.4lS34bXQdmubMf7JIpa6kSraVPpIe9nA-2Ptni2GdMM
MyWebApi$ 
```

The `create` command took care of updating our project's `appsettings.Development.json` file and user secrets store with the required configuration values for the JWT bearer authentication scheme to recognize the user JWTs. The `print` command printed out the JWT value we can include in our requests' `Authorization` header to test out our protected APIs (note the `--show-full` option is required right now to retrieve the JWT value but in a future release this will be shown by default when using `user-jwt create` and `user-jwt print`).

Now let's try accessing our protected API again, this time with the JWT value created by the tool:

```sh
MyWebApi$ curl -i -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImRhbWlhIiwic3ViIjoiZGFtaWEiLCJqdGkiOiJiMDQ5OGI5NCIsImF1ZCI6WyJodHRwczovL2xvY2FsaG9zdDo3MTg4IiwiaHR0cDovL2xvY2FsaG9zdDo1MDU2Il0sIm5iZiI6MTY1NDY0NjYxMSwiZXhwIjoxNjYyNTk1NDExLCJpYXQiOjE2NTQ2NDY2MTEsImlzcyI6ImRvdG5ldC11c2VyLWp3dHMifQ.4lS34bXQdmubMf7JIpa6kSraVPpIe9nA-2Ptni2GdMM" http://localhost:5000/secret
Hello damian. This is a secret!
MyWebApi$ 
```

You can create JWTs with different claims to explore and verify your app's authorization configuration. Let's create and use a JWT with a custom user name:

```sh
MyWebApi$ dotnet user-jwts create --name MyTestUser
New JWT saved with ID '5d285409'.
MyWebApi$ dotnet user-jwts print 5d285409 --show-full
Found JWT with ID '5d285409'
{
  "Id": "5d285409",
  "Scheme": "Bearer",
  "Name": "MyTestUser",
  "Audience": "https://localhost:7188",
  "NotBefore": "2022-06-08T00:53:57+00:00",
  "Expires": "2022-09-08T00:53:57+00:00",
  "Issued": "2022-06-08T00:53:57+00:00",
  "Token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6Ik15VGVzdFVzZXIiLCJzdWIiOiJNeVRlc3RVc2VyIiwianRpIjoiNWQyODU0MDkiLCJhdWQiOlsiaHR0cHM6Ly9sb2NhbGhvc3Q6NzE4OCIsImh0dHA6Ly9sb2NhbGhvc3Q6NTA1NiJdLCJuYmYiOjE2NTQ2NDk2MzcsImV4cCI6MTY2MjU5ODQzNywiaWF0IjoxNjU0NjQ5NjM3LCJpc3MiOiJkb3RuZXQtdXNlci1qd3RzIn0.Lggk5aPZm0gRmMi180HNOt1_XDs6Fa4QsAHmaHaHPhc",
  "Scopes": [],
  "Roles": [],
  "CustomClaims": {}
}
Token Header: {"alg":"HS256","typ":"JWT"}
Token Payload: {"unique_name":"MyTestUser","sub":"MyTestUser","jti":"5d285409","aud":["https://localhost:7188","http://localhost:5056"],"nbf":1654649637,"exp":1662598437,"iat":1654649637,"iss":"dotnet-user-jwts"}
Compact Token: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6Ik15VGVzdFVzZXIiLCJzdWIiOiJNeVRlc3RVc2VyIiwianRpIjoiNWQyODU0MDkiLCJhdWQiOlsiaHR0cHM6Ly9sb2NhbGhvc3Q6NzE4OCIsImh0dHA6Ly9sb2NhbGhvc3Q6NTA1NiJdLCJuYmYiOjE2NTQ2NDk2MzcsImV4cCI6MTY2MjU5ODQzNywiaWF0IjoxNjU0NjQ5NjM3LCJpc3MiOiJkb3RuZXQtdXNlci1qd3RzIn0.Lggk5aPZm0gRmMi180HNOt1_XDs6Fa4QsAHmaHaHPhc
MyWebApi$ curl -i -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6Ik15VGVzdFVzZXIiLCJzdWIiOiJNeVRlc3RVc2VyIiwianRpIjoiNWQyODU0MDkiLCJhdWQiOlsiaHR0cHM6Ly9sb2NhbGhvc3Q6NzE4OCIsImh0dHA6Ly9sb2NhbGhvc3Q6NTA1NiJdLCJuYmYiOjE2NTQ2NDk2MzcsImV4cCI6MTY2MjU5ODQzNywiaWF0IjoxNjU0NjQ5NjM3LCJpc3MiOiJkb3RuZXQtdXNlci1qd3RzIn0.Lggk5aPZm0gRmMi180HNOt1_XDs6Fa4QsAHmaHaHPhc" http://localhost:5000/secret
Hello MyTestUser. This is a secret!
MyWebApi$ 
```

Finally, let's create a JWT with a custom claim allowing access to the most secret API in our example app:

```sh
MyWebApi$ dotnet user-jwts create --name AnotherUser --scope "myapi:secrets"
JWT for user 'AnotherUser' created with id 'e9b480cb'.
MyWebApi$ dotnet user-jwts print e9b480cb
Found JWT with ID 'e9b480cb'
{
  "Id": "e9b480cb",
  "Scheme": "Bearer",
  "Name": "AnotherUser",
  "Audience": "https://localhost:7188",
  "NotBefore": "2022-06-08T00:57:43+00:00",
  "Expires": "2022-09-08T00:57:43+00:00",
  "Issued": "2022-06-08T00:57:43+00:00",
  "Token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IkFub3RoZXJVc2VyIiwic3ViIjoiQW5vdGhlclVzZXIiLCJqdGkiOiJlOWI0ODBjYiIsInNjb3BlIjoibXlhcGk6c2VjcmV0cyIsImF1ZCI6WyJodHRwczovL2xvY2FsaG9zdDo3MTg4IiwiaHR0cDovL2xvY2FsaG9zdDo1MDU2Il0sIm5iZiI6MTY1NDY0OTg2MywiZXhwIjoxNjYyNTk4NjYzLCJpYXQiOjE2NTQ2NDk4NjMsImlzcyI6ImRvdG5ldC11c2VyLWp3dHMifQ.U88zp4my_Po0WMsZ1irVFraKTJWHIsOy8MiQ2TkmteE",
  "Scopes": [
    "myapi:secrets"
  ],
  "Roles": [],
  "CustomClaims": {}
}
Token Header: {"alg":"HS256","typ":"JWT"}
Token Payload: {"unique_name":"AnotherUser","sub":"AnotherUser","jti":"e9b480cb","scope":"myapi:secrets","aud":["https://localhost:7188","http://localhost:5056"],"nbf":1654649863,"exp":1662598663,"iat":1654649863,"iss":"dotnet-user-jwts"}
Compact Token: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IkFub3RoZXJVc2VyIiwic3ViIjoiQW5vdGhlclVzZXIiLCJqdGkiOiJlOWI0ODBjYiIsInNjb3BlIjoibXlhcGk6c2VjcmV0cyIsImF1ZCI6WyJodHRwczovL2xvY2FsaG9zdDo3MTg4IiwiaHR0cDovL2xvY2FsaG9zdDo1MDU2Il0sIm5iZiI6MTY1NDY0OTg2MywiZXhwIjoxNjYyNTk4NjYzLCJpYXQiOjE2NTQ2NDk4NjMsImlzcyI6ImRvdG5ldC11c2VyLWp3dHMifQ.U88zp4my_Po0WMsZ1irVFraKTJWHIsOy8MiQ2TkmteE
MyWebApi$ curl -i -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IkFub3RoZXJVc2VyIiwic3ViIjoiQW5vdGhlclVzZXIiLCJqdGkiOiJlOWI0ODBjYiIsInNjb3BlIjoibXlhcGk6c2VjcmV0cyIsImF1ZCI6WyJodHRwczovL2xvY2FsaG9zdDo3MTg4IiwiaHR0cDovL2xvY2FsaG9zdDo1MDU2Il0sIm5iZiI6MTY1NDY0OTg2MywiZXhwIjoxNjYyNTk4NjYzLCJpYXQiOjE2NTQ2NDk4NjMsImlzcyI6ImRvdG5ldC11c2VyLWp3dHMifQ.U88zp4my_Po0WMsZ1irVFraKTJWHIsOy8MiQ2TkmteE" http://localhost:5000/special-secret
This is a special secret!
MyWebApi$ 
```

Using the new `user-jwts` tool we've been able to verify the APIs in our example app are configured for authorization in the way we intended. The app could of course now be configured to support an actual JWT provider according to that provider's requirements.

You can explore the commands and options available on `user-jwts` with the `--help` option, e.g.:

- `dotnet user-jwts --help`
- `dotnet user-jwts create --help`

## Minimal API parameter binding for argument lists

This preview extends parameter binding for minimal APIs to support refactoring a minimal API that takes a set of parameters into one that takes a single object with top level properties that represent what were once arguments.

As an example, the following API lists all the products in a given category:

```csharp
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Requires the Microsoft.EntityFrameworkCore.InMemory package
builder.Services.AddDbContext<MyDb>(options => options.UseInMemoryDatabase("products"));
var app = builder.Build();

app.MapGet("/categories/{categoryId}/products", (int categoryId, int pageSize, int page, ILogger<Program> logger, MyDb db) =>
{
    logger.LogInformation("Getting products for page {Page}", page);
    return db.Products.Where(p => p.CategoryId == categoryId).Skip((page - 1) * pageSize).Take(pageSize);
});

app.Run();

record Product (int Id, string Name, int CategoryId);

class MyDb : DbContext
{
    public MyDb(DbContextOptions options) : base(options) { }
    public DbSet<Product> Products { get; set; }
}
```

You can now refactor your API parameters to a type and add the new attribute `AsParameters` to your parameter, like this:

```csharp
app.MapGet("/categories/{categoryId}/products", ([AsParameters] ProductRequest req) =>
{
    req.Logger.LogInformation("Getting products for page {Page}", req.Page);
    return req.Db.Products.Where(p => p.CategoryId == req.CategoryId).Skip((req.Page - 1) * req.PageSize).Take(req.PageSize);
});

record struct ProductRequest(
    int CategoryId, 
    int PageSize, 
    int Page, 
    ILogger<ProductRequest> Logger, 
    MyDb Db);
```

The [parameter binding](https://docs.microsoft.com/aspnet/core/fundamentals/minimal-apis#parameter-binding) rules will be applied to the new type's **top level properties** or **parameterized constructor parameters**. Also, the same binding attributes (`FromRoute`, `FromQuery`, `FromServices`, etc.) are supported and can be applied to them. 

Let's update the previous example to bind both `Page` and `PageSize` from the request headers instead of query string and explicitly declare where the parameters are bound from:

```csharp
// You will need to include 'using Microsoft.AspNetCore.Mvc;'
record struct ProductRequest(
    [FromRoute] int CategoryId,
    [FromHeader(Name = "PageSize")] int PageSize,
    [FromHeader(Name = "Page")] int Page,
    [FromServices] ILogger<ProductRequest> Logger, 
    [FromServices] MyDb Db);
```

Both `classes` and `structs` are supported (the usage of `structs` is recommended to avoid additional memory allocation). However, `abstract` types and `interfaces` are not supported. In our previous example, the same type (currently a `record struct`) could be define as a `class`:

```csharp
class ProductRequest
{
    [FromRoute]
    public int CategoryId { get; set; }
    [FromHeader(Name = "PageSize")]
    public int PageSize { get; set; }
    [FromHeader(Name = "Page")]
    public int Page { get; set; }
    [FromServices]
    public ILogger<ProductRequest> Logger { get; set; }
    [FromServices]
    public MyDb Db { get; set; }
}
```

The following rules are applied during the parameter binding:

**Classes**

* A public parameterless constructor will be used if present 
* A public parameterized constructor will be used if a single constructor is present and all arguments have a matching (case-insensitive) public property.
   * If a constructor parameter does not match with a property, `InvalidOperationException` will be thrown if binding is attempted.
* Throw `InvalidOperationException` when more than one parameter is declared and the parameterless constructor is not present.
* Throw `InvalidOperationException` if a suitable constructor cannot be found.

**Structs**

* A declared public parameterless constructor will always be used if present
* A public parameterized constructor will be use if a single constructor is present and all arguments have a matching (case-insensitive) public property.
   * If a constructor parameter does not match with a property, `InvalidOperationException` will be thrown if binding is attempted.
* Since `struct` always has a default constructor, the default constructor will be used if it is the only one present or more than one parameterized constructor is present.

> Note: When binding using a parameterless constructor all **public settable properties** will be bound.


## Give feedback

We hope you enjoy this preview release of ASP.NET Core in .NET 7. Let us know what you think about these new improvements by filing issues on [GitHub](https://github.com/dotnet/aspnetcore/issues/new).

Thanks for trying out ASP.NET Core!
