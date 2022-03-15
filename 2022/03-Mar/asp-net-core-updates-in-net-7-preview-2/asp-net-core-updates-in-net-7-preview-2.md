---
post_title: ASP.NET Core updates in .NET 7 Preview 2
username: daroth@microsoft.com
microsoft_alias: daroth
featured_image: ./dotnet-bot_scene_chef.png
categories: .NET, ASP.NET Core, Blazor
summary: .NET 7 Preview 2 is now available! Check out what's new in ASP.NET Core in this update.
desired_publication_date: 2022-03-15
---

[.NET 7 Preview 2 is now available](https://devblogs.microsoft.com/dotnet/announcing-dotnet-7-preview-2) and includes many great new improvements to ASP.NET Core.

Here's a summary of what's new in this preview release:

- Infer API controller action parameters that come from services
- Dependency injection for SignalR hub methods
- Provide endpoint descriptions and summaries for minimal APIs
- Binding arrays and StringValues from headers and query strings in minimal APIs
- Customize the cookie consent value

For more details on the ASP.NET Core work planned for .NET 7 see the full [ASP.NET Core roadmap for .NET 7](https://aka.ms/aspnet/roadmap) on GitHub.

## Get started

To get started with ASP.NET Core in .NET 7 Preview 2, [install the .NET 7 SDK](https://dotnet.microsoft.com/download/dotnet/7.0).

If you're on Windows using Visual Studio, we recommend installing the latest [Visual Studio 2022 preview](https://visualstudio.com/preview). Visual Studio for Mac support for .NET 7 previews isn't available yet but is coming soon.

To install the latest .NET WebAssembly build tools, run the following command from an elevated command prompt:

```console
dotnet workload install wasm-tools
```

## Upgrade an existing project

To upgrade an existing ASP.NET Core app from .NET 7 Preview 1 to .NET 7 Preview 2:

- Update all Microsoft.AspNetCore.\* package references to `7.0.0-preview.2.*`.
- Update all Microsoft.Extensions.\* package references to `7.0.0-preview.2.*`.

See also the full list of [breaking changes](https://docs.microsoft.com/dotnet/core/compatibility/7.0#aspnet-core) in ASP.NET Core for .NET 7.

## Infer API controller action parameters that come from services

Parameter binding for API controller actions now binds parameters through dependency injection when the type is configured as a service. This means it's no longer required to explicitly apply the `[FromServices]` attribute to a parameter.

``` csharp
Services.AddScoped<SomeCustomType>();

[Route("[controller]")]
[ApiController]
public class MyController : ControllerBase
{
    // Both actions will bound the SomeCustomType from the DI container
    public ActionResult GetWithAttribute([FromServices]SomeCustomType service) => Ok();
    public ActionResult Get(SomeCustomType service) => Ok();
}
```

You can disable the feature by setting `DisableImplicitFromServicesParameters`:

``` csharp
Services.Configure<ApiBehaviorOptions>(options =>
{
     options.DisableImplicitFromServicesParameters = true;
})
```

## Dependency injection for SignalR hub methods

SignalR hub methods now support injecting services through dependency injection (DI).

```csharp
Services.AddScoped<SomeCustomType>();

public class MyHub : Hub
{
    // SomeCustomType comes from DI by default now
    public Task Method(string text, SomeCustomType type) => Task.CompletedTask;
}
```

You can disable the feature by setting `DisableImplicitFromServicesParameters`:

```csharp
services.AddSignalR(options =>
{
    options.DisableImplicitFromServicesParameters = true;
});
```

To explicitly mark a parameter to be bound from configured services, use the `[FromServices]` attribute:

```csharp
public class MyHub : Hub
{
    public Task Method(string arguments, [FromServices] SomeCustomType type);
}
```

## Provide endpoint descriptions and summaries for minimal APIs

Minimal APIs now support annotating operations with descriptions and summaries used for OpenAPI spec generation. You can set these descriptions and summaries for route handlers in your minimal API apps using an extension methods:

```csharp
app.MapGet("/hello", () => ...)
  .WithDescription("Sends a request to the backend HelloService to process a greeting request.");
```

Or set the description or summary via attributes on the route handler delegate:

```csharp
app.MapGet("/hello", [EndpointSummary("Sends a Hello request to the backend")]() => ...)
```

## Binding arrays and StringValues from headers and query strings in minimal APIs

With this release, you can now bind values from HTTPS headers and query strings to arrays of primitive types, string arrays, or `StringValues`:

```csharp
// Bind query string values to a primitive type array
// GET  /tags?q=1&q=2&q=3
app.MapGet("/tags", (int[] q) => $"tag1: {q[0]} , tag2: {q[1]}, tag3: {q[2]}")

// Bind to a string array
// GET /tags?names=john&names=jack&names=jane
app.MapGet("/tags", (string[] names) => $"tag1: {names[0]} , tag2: {names[1]}, tag3: {names[2]}")

// Bind to StringValues
// GET /tags?names=john&names=jack&names=jane
app.MapGet("/tags", (StringValues names) => $"tag1: {names[0]} , tag2: {names[1]}, tag3: {names[2]}")
```

You can also bind query strings or header values to an array of a complex type as long as the type has `TryParse` implementation as demonstrated in the example below.

```csharp
// Bind to aan array of a complex type
// GET /tags?tags=trendy&tags=hot&tags=spicy
app.MapGet("/tags", (Tag[] tags) =>
{
    return Results.Ok(tags);
});

...

class Tag 
{
    public string? TagName { get; init; }

    public static bool TryParse(string? tagName, out Tag tag)
    {
        if (tagName is null) 
        {
            tag = default;
            return false;
        }

        tag = new Tag { TagName = tagName };
        return true;
    }
}
```

## Customize the cookie consent value

You can now specify the value used to track if the user consented to the cookie use policy using the new `CookiePolicyOptions.ConsentCookieValue` property.

Thank you [@daviddesmet](https://github.com/daviddesmet) for contributing this improvement!

## Request for feedback on shadow copying for IIS

In .NET 6 we added experimental support for shadow copying app assemblies to the ASP.NET Core Module (ANCM) for IIS. When an ASP.NET Core app is running on Windows, the binaries are locked so that they cannot be modified or replaced. You can stop the app by deploying an [app offline file](https://docs.microsoft.com/aspnet/core/host-and-deploy/app-offline), but sometimes doing so is inconvenient or impossible. Shadow copying enables the app assemblies to be updated while the app is running by making a copy of the assemblies.

You can enable shadow copying by customizing the ANCM handler settings in *web.config*:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <system.webServer>
    <handlers>
      <remove name="aspNetCore"/>
      <add name="aspNetCore" path="*" verb="*" modules="AspNetCoreModuleV2" resourceType="Unspecified"/>
    </handlers>
    <aspNetCore processPath="%LAUNCHER_PATH%" arguments="%LAUNCHER_ARGS%" stdoutLogEnabled="false" stdoutLogFile=".\logs\stdout">
      <handlerSettings>
        <handlerSetting name="experimentalEnableShadowCopy" value="true" />
        <handlerSetting name="shadowCopyDirectory" value="../ShadowCopyDirectory/" />
      </handlerSettings>
    </aspNetCore>
  </system.webServer>
</configuration>
```

We're investigating making shadow copying in IIS a feature of ASP.NET Core in .NET 7, and we're seeking additional feedback on whether the feature satisfies user requirements. If you deploy ASP.NET Core to IIS, please give shadow copying a try and [share with us your feedback on GitHub](https://github.com/dotnet/AspNetCore.Docs/issues/23733).

## Give feedback

We hope you enjoy this preview release of ASP.NET Core in .NET 7. Let us know what you think about these new improvements by filing issues on [GitHub](https://github.com/dotnet/aspnetcore/issues/new).

Thanks for trying out ASP.NET Core!
