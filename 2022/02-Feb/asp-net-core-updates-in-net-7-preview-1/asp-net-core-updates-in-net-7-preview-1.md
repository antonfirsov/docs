---
post_title: ASP.NET Core updates in .NET 7 Preview 1
username: daroth@microsoft.com
microsoft_alias: daroth
featured_image: ./dotnet-bot_scene_surveying.png
categories: .NET, ASP.NET Core, Blazor
summary: .NET 7 Preview 1 is now available! Check out what's new in ASP.NET Core in this update and learn about the roadmap for ASP.NET Core in .NET 7.
desired_publication_date: 2022-02-17
---

[.NET 7 Preview 1 is now available!](https://devblogs.microsoft.com/dotnet/announcing-net-7-preview-1). This is the first preview of the next major version of .NET, which will include the next wave of innovations for web development with ASP.NET Core.

In .NET 7 we plan to make broad investments across ASP.NET Core. Below are some of the areas we plan to focus on:

- **Performance**: .NET 6 contained many [performance improvements for ASP.NET Core](https://devblogs.microsoft.com/dotnet/performance-improvements-in-aspnet-core-6/), and we'll do work to make ASP.NET Core even faster and more efficient in .NET 7.
- **HTTP/3**: HTTP/3 support shipped as a preview feature in .NET 6. For .NET 7, we want to finish it and make it a supported feature that's enabled by default. In future previews, you can expect to see advanced TLS features and more performance improvements in our HTTP/3 support.
- **Minimal APIs**: Add support for endpoint filters and route grouping as core primitives for minimal APIs. Also simplify authentication and authorization configurations for APIs in general.
- **gRPC**: We're investing in gRPC JSON transcoding. This feature allows gRPC services to be called like RESTful HTTP APIs with JSON requests and responses.
- **SignalR**: Add support for strongly-typed clients and returning results from client invocations.
- **Razor**: We'll make various improvements to the Razor compiler to improve performance, resiliency, and to facilitate improved tooling.
- **Blazor**: After finishing Blazor Hybrid support for .NET MAUI, WPF, and Windows Forms, we'll make broad improvements to Blazor including:
    - New .NET WebAssembly capabilities: mixed-mode AOT, multithreading, web crypto.
    - Enhanced Hot Reload support.
    - Data binding improvements.
    - More flexible prerendering.
    - More control over the lifecycle of Blazor Server circuits.
    - Improved support for micro frontends.
- **MVC**: Improvements to endpoint routing, link generation, and parameter binding.
- **Orleans**: The ASP.NET Core and Orleans teams are investigating ways to further align and integrate the Orleans distributed programming model with ASP.NET Core. Orleans 4 will ship alongside .NET 7 and focuses on simplicity, maintainability, and performance, including human readable stream identities and a new optimized, version-tolerant serializer.

For more details on the specific ASP.NET Core work planned for .NET 7 see the full [ASP.NET Core roadmap for .NET 7](https://aka.ms/aspnet/roadmap) on GitHub.

.NET 7 Preview 1 is the first of many .NET 7 preview releases in preparation for the .NET 7 release in November 2022. 

Here's a summary of what's new in this preview release:

- Minimal API improvements:
    - `IFormFile` and `IFormFileCollection` Support
    - Bind the request body as a `Stream` or `PipeReader`
    - JSON options configuration
- SignalR client source generator
- Support for nullable models in MVC views and Razor Pages
- Use JSON property names in validation errors
- Improved console output for `dotnet watch`
- Configure `dotnet watch` to always restart for rude edits
- Use dependency injection in a `ValidationAttribute`
- Faster header parsing and writing
- gRPC JSON transcoding

### Get started

To get started with ASP.NET Core in .NET 7 Preview 1, [install the .NET 7 SDK](https://dotnet.microsoft.com/download/dotnet/7.0).

If you're on Windows using Visual Studio, we recommend installing the latest [Visual Studio 2022 preview](https://visualstudio.com/preview). Visual Studio for Mac support for .NET 7 previews isn't available yet but is coming soon.

To install the latest .NET WebAssembly build tools, run the following command from an elevated command prompt:

```console
dotnet workload install wasm-tools
```

### Upgrade an existing project

To upgrade an existing ASP.NET Core app from .NET 6 to .NET 7 Preview 1:

- Update the target framework for your app to `net7.0`.
- Update all Microsoft.AspNetCore.\* package references to `7.0.0-preview.1.*`.
- Update all Microsoft.Extensions.\* package references to `7.0.0-preview.1.*`.

See also the full list of [breaking changes](https://docs.microsoft.com/dotnet/core/compatibility/7.0#aspnet-core) in ASP.NET Core for .NET 7.

## Minimal API improvements

### `IFormFile` and `IFormFileCollection` Support

You can now handle file uploads in minimal APIs using `IFormFile` and `IFormFileCollection`: 

```csharp
app.MapPost("/upload", async(IFormFile file) =>
{
    using var stream = System.IO.File.OpenWrite("upload.txt");
    await file.CopyToAsync(stream); 
});
```

```csharp
app.MapPost("/upload", async (IFormFileCollection myFiles) => { ... });
```

Using this feature with authentication requires anti-forgery support, which isn't yet implemented. [Anti-forgery support for minimal APIs](https://github.com/dotnet/aspnetcore/issues/38630) is on our roadmap for .NET 7. Binding to `IFormFile` or `IFormFileCollection` when the request contains an `Authorization` header with a bearer token is currently disabled. This limitation will be addressed as soon as we complete the work on anti-forgery support.

Thanks to @martincostello for contributing this feature.

#### Bind the request body as a `Stream` or `PipeReader`

You can now bind the request body as a `Stream` or `PipeReader` to efficiently support scenarios where the user has to ingest data and either store it to a blob storage or enqueue the data to a queue provider (Azure Queue, etc.) for later processing by a worker or cloud function. The following example shows how to use the new binding:

```csharp
app.MapPost("v1/feeds", async (QueueClient queueClient, Stream body, CancellationToken cancellationToken) =>
{
    await queueClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken);
    await queueClient.SendMessageAsync(new BinaryData(body), cancellationToken: cancellationToken);
});
```

When using the `Stream` or `PipeReader` there are a few things to take into consideration: 

- When ingesting data, the `Stream` will be the same object as `HttpRequest.Body`.
- The request body isn't buffered by default. After the body is read, it's not rewindable (you can't read the stream multiple times).
- The `Stream/PipeReader` are not usable outside of the minimal action handler as the underlying buffers will be disposed and/or reused.
    
### JSON options configuration

We're introducing a new and cleaner API, `ConfigureRouteHandlerJsonOptions`, to configure JSON options for minimal API endpoints. This new API avoids confusion with `Microsoft.AspNetCore.Mvc.JsonOptions`.

```csharp
var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureRouteHandlerJsonOptions(options =>
{
    //Ignore Cycles
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; 
});    
```

## SignalR client source generator

We've added a new client source generator for SignalR thanks to a contribution by @mehmetakbulut.

The SignalR client source generator generates strongly-typed sending and receiving code based on interfaces that you define. You can reuse the same interfaces from [strongly-typed SignalR hubs](https://docs.microsoft.com/aspnet/core/signalr/hubs#strongly-typed-hubs) on the client in place of the loosely-typed `.On("methodName", ...)` methods. Similarly, your hub can implement an interface for its methods and the client can use that same interface to call the hub methods.

To use the SignalR client source generator:

* Add a reference to the [Microsoft.AspNetCore.SignalR.Client.SourceGenerator](https://nuget.org/packages/Microsoft.AspNetCore.SignalR.Client.SourceGenerator) package.
* Add a `HubServerProxyAttribute` and `HubClientProxyAttribute` class to your project (this part of the design will likely change in future previews):

```csharp
[AttributeUsage(AttributeTargets.Method)]
internal class HubServerProxyAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Method)]
internal class HubClientProxyAttribute : Attribute
{
}
```

* Add a static partial class to your project and write static partial methods with the `[HubClientProxy]` and `[HubServerProxy]` attributes

```csharp
internal static partial class MyCustomExtensions
{
    [HubClientProxy]
    public static partial IDisposable ClientRegistration<T>(this HubConnection connection, T provider);

    [HubServerProxy]
    public static partial T ServerProxy<T>(this HubConnection connection);
}
```

* Use the partial methods from your code!

```csharp
public interface IServerHub
{
    Task SendMessage(string message);
    Task<int> Echo(int i);
}

public interface IClient
{
    Task ReceiveMessage(string message);
}

public class Client : IClient
{
    // Equivalent to HubConnection.On("ReceiveMessage", (message) => {});
    Task ReceiveMessage(string message)
    {
        return Task.CompletedTask;
    }
}

HubConnection connection = new HubConnectionBuilder().WithUrl("...").Build();
var stronglyTypedConnection = connection.ServerProxy<IServerHub>();
var registrations = connection.ClientRegistration<IClient>(new Client());

await stronglyTypedConnection.SendMessage("Hello world");
var echo = await stronglyTypedConnection.Echo(10);
```

## Support for nullable models in MVC views and Razor Pages

We enabled defining a nullable page or view model to improve the experience when using null state checking with ASP.NET Core apps:

```razor
@model Product?
```

## Use JSON property names in validation errors

When model validation produces a `ModelErrorDictionary` it will by default use the property name as the error key (`"MyClass.PropertyName"`). Model property names are generally an implementation detail, which can make them difficult to handle from single-page apps. You can now configure validation to use the corresponding JSON property names instead with the new `SystemTextJsonValidationMetadataProvider` (or `NewtonsoftJsonValidationMetadataProvider` when using Json.NET).

```csharp
services.AddControllers(options =>
{
    options.ModelMetadataDetailsProviders.Add(new SystemTextJsonValidationMetadataProvider())
});
```

## Improved console output for `dotnet watch`

We cleaned up the console output from `dotnet watch` to better align with the log out of ASP.NET Core and to stand out with 😮emojis😍.

Here's an example of what the new output looks like:

```console
C:\BlazorApp> dotnet watch
dotnet watch 🔥 Hot reload enabled. For a list of supported edits, see https://aka.ms/dotnet/hot-reload.
  💡 Press "Ctrl + R" to restart.
dotnet watch 🔧 Building...
  Determining projects to restore...
  All projects are up-to-date for restore.
  You are using a preview version of .NET. See: https://aka.ms/dotnet-support-policy
  BlazorApp -> C:\Users\daroth\Desktop\BlazorApp\bin\Debug\net7.0\BlazorApp.dll
dotnet watch 🚀 Started
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:7148
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5041
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Development
info: Microsoft.Hosting.Lifetime[0]
      Content root path: C:\Users\daroth\Desktop\BlazorApp\
dotnet watch ⌚ File changed: .\Pages\Index.razor.
dotnet watch 🔥 Hot reload of changes succeeded.
info: Microsoft.Hosting.Lifetime[0]
      Application is shutting down...
dotnet watch 🛑 Shutdown requested. Press Ctrl+C again to force exit.
```

## Configure `dotnet watch` to always restart for rude edits

Configure `dotnet watch` to always restart without a prompt for rude edits (edits that can't be hot reloaded) by setting the `DOTNET_WATCH_RESTART_ON_RUDE_EDIT` environment variable to `true`.

## Inject services into custom validation attributes in Blazor

You can now inject services into custom validation attributes in Blazor. Blazor will setup the `ValidationContext` so it can be used as a service provider.

```csharp
public class SaladChefValidatorAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var saladChef = validationContext.GetRequiredService<SaladChef>();
        if (saladChef.ThingsYouCanPutInASalad.Contains(value.ToString()))
        {
            return ValidationResult.Success;
        }
        return new ValidationResult("You should not put that in a salad!");
    }
}

// Simple class configured as a service for dependency injection
public class SaladChef
{
    public string[] ThingsYouCanPutInASalad = { "Strawberries", "Pineapple", "Honeydew", "Watermelon", "Grapes" };
}
```

Thank you @MariovanZeist for this contribution!

## Faster header parsing and writing

We made several improvements to the performance of header parsing and writing for HTTP/2 and HTTP/3. See the following pull requests for details:

- [HTTP/2: Improve incoming header performance](https://github.com/dotnet/aspnetcore/pull/38834)
- [HTTP/3: Optimize validating and setting incoming headers](https://github.com/dotnet/aspnetcore/pull/39013)
- [HTTP headers enumerator move directly to next](https://github.com/dotnet/aspnetcore/pull/37538)

## gRPC JSON transcoding

gRPC JSON transcoding allows gRPC services to be used like a RESTful HTTP APIs. Once configured, gRPC JSON transcoding allows you to call gRPC methods with familiar HTTP concepts:

* HTTP verbs
* URL parameter binding
* JSON requests/responses

Of course gRPC can continue to be used as well. RESTful APIs for your gRPC services. No duplication!

ASP.NET Core has experimental support for this feature using a library called gRPC HTTP API. For .NET 7 we plan to make this functionality a supported part of ASP.NET Core. This functionality isn't included with .NET 7 yet, but you can try out the existing experimental packages. For more information, see the [gRPC HTTP API getting started documentation](https://github.com/aspnet/AspLabs/tree/main/src/GrpcHttpApi).

## Give feedback

We hope you enjoy this preview release of ASP.NET Core in .NET 7 and that you're as excited about about our roadmap for .NET 7 as we are! We're eager to hear about your experiences with this release and your thoughts on the roadmap. Let us know what you think by filing issues on [GitHub](https://github.com/dotnet/aspnetcore/issues/new) and commenting on the [roadmap issue](https://aka.ms/aspnet/roadmap).

Thanks for trying out ASP.NET Core!
