---
post_title: ASP.NET Core updates in .NET 7 Preview 7
author1: daroth@microsoft.com
post_slug: asp-net-core-updates-in-dotnet-7-preview-7
username: daroth@microsoft.com
microsoft_alias: daroth
featured_image: ./dotnet-bot_scene_limo-driver.png
categories: .NET, ASP.NET, ASP.NET Core
summary: .NET 7 Preview 7 is now available! Check out what's new in ASP.NET Core in this update.
desired_publication_date: 2022-08-09
---

[.NET 7 Preview 7 is now available](https://devblogs.microsoft.com/dotnet/announcing-dotnet-7-preview-7) and includes many great new improvements to ASP.NET Core.

Here's a summary of what's new in this preview release:

- New Blazor WebAssembly loading page
- Blazor data binding get/set/after modifiers
- Blazor virtualization improvements
- Pass state using `NavigationManager`
- Additional `System.Security.Cryptography` support on WebAssembly
- Updated Angular and React templates
- gRPC JSON transcoding performance
- Authentication will use single scheme as `DefaultScheme`
- `IFormFile`/`IFormFileCollection` support for authenticated requests in minimal APIs
- New problem details service
- Diagnostics middleware updates
- New `HttpResults` interfaces

For more details on the ASP.NET Core work planned for .NET 7 see the full [ASP.NET Core roadmap for .NET 7](https://aka.ms/aspnet/roadmap) on GitHub.

## Get started

To get started with ASP.NET Core in .NET 7 Preview 7, [install the .NET 7 SDK](https://dotnet.microsoft.com/download/dotnet/7.0).

If you're on Windows using Visual Studio, we recommend installing the latest [Visual Studio 2022 preview](https://visualstudio.com/preview). If you're on macOS, we recommend installing the latest [Visual Studio 2022 for Mac preview](https://visualstudio.microsoft.com/vs/mac/preview/).

To install the latest .NET WebAssembly build tools, run the following command from an elevated command prompt:

```console
dotnet workload install wasm-tools
```

> Note: Building .NET 6 Blazor projects with the .NET 7 SDK and the .NET 7 WebAssembly build tools is currently not supported. This will be addressed in a future .NET 7 update: [dotnet/runtime#65211](https://github.com/dotnet/runtime/issues/65211).

## Upgrade an existing project

To upgrade an existing ASP.NET Core app from .NET 7 Preview 6 to .NET 7 Preview 7:

- Update all Microsoft.AspNetCore.\* package references to `7.0.0-preview.7.*`.
- Update all Microsoft.Extensions.\* package references to `7.0.0-preview.7.*`.

See also the full list of [breaking changes](https://docs.microsoft.com/dotnet/core/compatibility/7.0#aspnet-core) in ASP.NET Core for .NET 7.

## New Blazor loading page

The Blazor WebAssembly project template has a new loading UI that shows the progress of loading the app.

![Blazor WebAssembly loading screen](blazor-wasm-loading-screen.gif)

The new loading screen is implemented with HTML and CSS in the Blazor WebAssembly template using two new CSS custom properties (variables) provided by Blazor WebAssembly:

- `--blazor-load-percentage`: The percentage of app files loaded.
- `--blazor-load-percentage-text`: The percentage of app files loaded rounded to the nearest whole number.

Using these new CSS variables, you can create a custom loading UI that matches the styling of your own Blazor WebAssembly apps.

## Blazor data binding get/set/after modifiers

Blazor provides a powerful [data binding](https://docs.microsoft.com/aspnet/core/blazor/components/data-binding) feature for creating two-way bindings between UI elements or component parameters with .NET objects. In .NET 7 you can now easily run async logic after a binding event has completed using the new `@bind:after` modifier:

```razor
<input @bind="searchText" @bind:after="PerformSearch" />

@code {
    string searchText;

    async Task PerformSearch()
    {
        // ... do something asynchronously with 'searchText' ...
    }
}
```

In this example the `PerformSearch` async method will run automatically after any changes to the search text are detected.

It's also now easier to setup binding for component parameters. Components can support two-way data binding by defining a pair of parameters for the value and for a callback that is called when the value changes. The new `@bind:get` and `@bind:set` modifiers now make it trivial to create a component parameters that binds to an underlying UI element:

```razor
<input @bind:get="Value" @bind:set="ValueChanged" />

@code {
    [Parameter] public TValue Value { get; set; }
    [Parameter] public EventCallback<TValue> ValueChanged { get; set; }
}
```

The `@bind:get` and `@bind:set` modifiers are always used together. The `@bind:get` modifier specifies the value to bind to and the `@bind:set` modifier specifies a callback that is called when the value changes.

## Blazor virtualization improvements

Blazor's `Virtualize` component renders a spacer element to define the vertical height of the scroll region. By default it uses a `div` element like this:

```html
<div style="height: 12345px"></div>
```

However, in some cases the parent element might not allow child `div` elements. For example, the parent element might be a `tbody`, which only allows child `tr` elements. For these cases you can now use the new `SpacerElement` parameter to configure the spacer element that `Virtualize` uses:

```razor
<tbody>
  <Virtualize SpacerElement="tr">...</Virtualize>
</tbody>
```

## Pass state using `NavigationManager`

You can now pass state when navigating in Blazor apps using the `NavigationManager`.

```csharp
navigationManager.NavigateTo("/orders", new NavigationOptions { HistoryEntryState = "My state" });
```

This mechanism allows for simple communication between different pages. The specified state is pushed onto the browser's history stack so that it can be accessed later using either the `NavigationManager.HistoryEntryState` property or the `LocationChangedEventArgs.HistoryEntryState` property when listening for location changed events.

## Additional `System.Security.Cryptography` support on WebAssembly

.NET 6 supported the SHA family of hashing algorithms when running on WebAssembly. .NET 7 enables more cryptographic algorithms by taking advantage of [SubtleCrypto](https://developer.mozilla.org/docs/Web/API/SubtleCrypto) when possible, and falling back to a .NET implementation when SubtleCrypto can't be used. In .NET 7 Preview 7 the following algorithms are now supported on WebAssembly:

- SHA1, SHA256, SHA384, SHA512
- HMACSHA1, HMACSHA256, HMACSHA384, HMACSHA512
- Aes (only CBC mode is supported)
- Rfc2898DeriveBytes (PBKDF2)
- HKDF

## Updated Angular and React templates

We updated the Angular project template to Angular 14 and the React project template to React 18.2.

## gRPC JSON transcoding performance

[gRPC JSON transcoding](https://devblogs.microsoft.com/dotnet/announcing-grpc-json-transcoding-for-dotnet/) is a new feature in .NET 7 for turning gRPC APIs into RESTful APIs.

.NET 7 Preview 7 improves performance and memory usage when serializing messages. gRPC JSON transcoding serializes gRPC messages to a [standardize JSON format](https://developers.google.com/protocol-buffers/docs/proto3#json). Before Preview 7, transcoding required a custom `JsonConverter` to customize JSON serialization. This release replaces the `JsonConverter` with [System.Text.Json's new contract customization feature](https://github.com/dotnet/runtime/issues/63686).

The benchmark results below compare serializing gRPC messages before and after using contract customization:

| Method                       | Mean     | Ratio | Allocated |
|------------------------------|----------|-------|-----------|
| SerializeMessage_Converter   | 386.7 ns | 1.00  | 160 B     |
| SerializeMessage_Contract    | 213.3 ns | 0.55  | 80 B      |
| DeserializeMessage_Converter | 296.0 ns | 1.00  | 304 B     |
| DeserializeMessage_Contract  | 167.6 ns | 0.57  | 224 B     |

A custom contract and System.Text.Json's high-performance serializer dramatically improves performance and allocations.

## Authentication will use single scheme as `DefaultScheme`

As part of the work to simplify authentication, when there is only a single authentication scheme registered, it will automatically be used as the `DefaultScheme`, which eliminates the need to specify the `DefaultScheme` in `AddAuthentication()` in this case. This behavior can be disabled via `AppContext.SetSwitch("Microsoft.AspNetCore.Authentication.SuppressAutoDefaultScheme")`

## `IFormFile`/`IFormFileCollection` support for authenticated requests in minimal APIs

In .NET 7 Preview 1 introduced support for [handling file upload in minimal APIs](https://devblogs.microsoft.com/dotnet/asp-net-core-updates-in-net-7-preview-1/#iformfile-and-iformfilecollection-support) using `IFormFile` and `IFormFileCollection`. .NET Preview 7 adds support for authenticated file upload requests to minimal APIs using an `Authorization` header, a client certificate, or a cookie header.

There is no built-in support for anti-forgery in minimal APIs. However, it can be [implemented using the `IAntiforgery` service](https://docs.microsoft.com/aspnet/core/security/anti-request-forgery?view=aspnetcore-7.0#antiforgery-with-minimal-apis).

## New problem details service

.NET 7 Preview 7 introduces a new problem details service based on the `IProblemDetailsService` interface for generating consistent [problem details](https://www.rfc-editor.org/rfc/rfc7807) responses in your app.

To add the problem details service, use the `AddProblemDetails` extension method on `IServiceCollection`.

```csharp
builder.Services.AddProblemDetails();
```

You can then write a problem details response from any layer in your app by calling `IProblemDetailsService.WriteAsync`. For example, here's how you can generate a problem details response from middleware:

``` csharp
httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

if (context.RequestServices.GetService<IProblemDetailsService>() is { } problemDetailsService)
{
    return problemDetailsService.WriteAsync(new { HttpContext = httpContext });
}

return ValueTask.CompletedTask;
```

You can customize problem details responses generated by the service (including autogenerated responses for API controllers) using `ProblemDetailsOptions`:

``` csharp
builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = (context) => 
    {
       context.ProblemDetails.Extensions.Add("my-extension", new { Property = "value" });
    };
});
```

In addition, you can create your own `IProblemDetailsWriter` implementation for advanced customizations:

``` csharp
public class CustomWriter : IProblemDetailsWriter
{
    // Indicates that only responses with StatusCode == 400
    // will be handled by this writer. All others will be
    // handled by different registered writers if available.
    public bool CanWrite(ProblemDetailsContext context) 
        => context.HttpContext.Response.StatusCode == 400;

    public Task WriteAsync(ProblemDetailsContext context)
    {
        //Additional customizations

        // Write to the response
        context.HttpResponse.Response.WriteAsJsonAsync(context.ProblemDetails);
    }        
}
```

Register any `IProblemDetailsWriter` implementations before the call to the `AddProblemDetails` method:

```csharp
builder.Services.AddSingleton<IProblemDetailsWriter, CustomWriter>();
builder.Services.AddProblemDetails();
```

## Diagnostics middleware updates

The following middleware were updated to generated problem details HTTP responses when the new problem details service (`IProblemDetailsService`) is registered:

- `ExceptionHandlerMiddleware`: Generates a problem details response when a custom handler is not defined, unless not accepted by the client.
- `StatusCodePagesMiddleware`: Generates a problem details response by default, unless not accepted by the client.
- `DeveloperExceptionPageMiddleware`: Generate a problem details response when `text/html` is not accepted, unless not accepted by the client.

The following sample configures the app to generate a problem details response for all HTTP client and server error responses that do not have a body content yet:

``` csharp
var builder = WebApplication.CreateBuilder(args);

// Add services to the containers
builder.Services.AddControllers();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler();
app.UseStatusCodePages();

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.MapControllers();
app.Run();
```

## New `HttpResults` interfaces

In [.NET 7 Preview 3](https://devblogs.microsoft.com/dotnet/asp-net-core-updates-in-dotnet-7-preview-3/#improved-unit-testability-for-minimal-route-handlers) the types implementing `IResult` in ASP.NET Core were made public.  Now, in this preview, we're introducing new interfaces, in the namespace `Microsoft.AspNetCore.Http.Http`, to describe the `IResult` types:

- `Microsoft.AspNetCore.Http.IContentTypeHttpResult`
- `Microsoft.AspNetCore.Http.IFileHttpResult`
- `Microsoft.AspNetCore.Http.INestedHttpResult`
- `Microsoft.AspNetCore.Http.IStatusCodeHttpResult`
- `Microsoft.AspNetCore.Http.IValueHttpResult`
- `Microsoft.AspNetCore.Http.IValueHttpResult<TValue>`

With these interfaces you now have a more generalized way to detect the `IResult` type at runtime, which is a common pattern in filter implementations.

``` csharp
app.MapGet("/weatherforecast", (int days) =>
{
    if (days <= 0)
    {
        return Results.BadRequest();
    }

    var forecast = Enumerable.Range(1, days.Value).Select(index =>
       new WeatherForecast (DateTime.Now.AddDays(index), Random.Shared.Next(-20, 55), "Cool"))
        .ToArray();
    return Results.Ok(forecast);
}).
AddEndpointFilter(async (context,next) =>
{
    var result = await next(context);

    return result switch
    {
        IValueHttpResult<WeatherForecast[]> weatherForecastResult => new WeatherHttpResult(weatherForecastResult.Value),
        _ => result
    };
});

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
```

## Give feedback

We hope you enjoy this preview release of ASP.NET Core in .NET 7. Let us know what you think about these new improvements by filing issues on [GitHub](https://github.com/dotnet/aspnetcore/issues/new).

Thanks for trying out ASP.NET Core!
