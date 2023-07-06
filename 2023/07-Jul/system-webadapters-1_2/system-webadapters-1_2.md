---
post_title: Introducing System.Web Adapters v1.2 with new APIs and scenarios
author1: twsouthwick
post_slug: systemweb-adapters-1_2
username: twsouthwick
microsoft_alias: tasou
featured_image: dotnet-bot_scene_archaeologists.png
categories: .NET, .NET Core, .NET Framework, ASP.NET, ASP.NET Core
tags: migration, system.web, adapters, asp.net
summary: Introducing the release of System.Web adapters v1.2 which introduces new APIs, better Blazor support, A/B testing of migrated endpoints, and more.
desired_publication_date: 2023-07-10
post_date: 2023-07-10 10:05:00
---

Today, we're releasing an update to the [System.Web Adapters](https://www.nuget.org/packages/Microsoft.AspNetCore.SystemWebAdapters) that simplify upgrading from ASP.NET to ASP.NET Core. This release brings a number of fixes as well as new scenarios that we'll explore in this post.

## `IHttpModule` support and emulation in ASP.NET Core

One of the scenarios this release enables is a way to run custom `HttpApplication` and managed `IHttpModule` implementations in the ASP.NET Core pipeline. Ideally, these would be refactored to middleware in ASP.NET Core, but we've seen instances where this can be a blocker to migration. This new support allows more shared code to be migrated to ASP.NET Core, although there may be some behavior differences that cannot be handled in the ASP.NET Core pipeline.

You add `HttpApplication` and `IHttpModule` implementations using the System.Web adapter builder:

```csharp
using System.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSystemWebAdapters()
    // Without the generic argument, a default HttpApplication will be used
    .AddHttpApplication<MyApp>(options =>
    {
        // Size of pool for HttpApplication instances. Should be what the expected concurrent requests will be
        options.PoolSize = 10;

        // Register a module by name - without the name, it will default to the type name
        options.RegisterModule<MyModule>("MyModule");
    });

var app = builder.Build();

app.UseSystemWebAdapters();

app.Run();

class MyApp : HttpApplication
{
    protected void Application_Start()
    {
        ...
    }
}

internal sealed class MyModule : IHttpModule
{
    public void Init(HttpApplication app)
    {
        application.AuthorizeRequest += (s, e)
        {
            ...
        }

        application.BeginRequest += (s, e) =>
        {
            ...
        }

        application.EndRequest += (s, e) =>
        {
            ...
        }
    }

    public void Dispose()
    {
    }
}
```

Some things to keep in mind while using this feature:

- Simple modules (especially those with only a single event), should be migrated to middleware instead using the System.Web adapters to share code as needed.
- In order to have the authorization and authentication related events run when expected, additional middleware should be manually inserted by calling `UseAuthenticationEvents()` and `UseAuthorizationEvents()`. If this is not done, the middleware will be automatically inserted when `UseSystemWebAdapters()` is called, which may cause these events to fire at unexpected times:

```diff
    var app = builder.Build();

    app.UseAuthentication();
+   app.UseAuthenticationEvents();
    app.UseAuthorization();
+   app.UseAuthorizationEvents();
    app.UseSystemWebAdapters();
    
    app.Run();
```

- The events are fired in the order they were in ASP.NET, but some of the state of the request may not be quite the same due to underlying differences in the frameworks. We're not aware at this moment of major differences, but please file issues at [dotnet/systemweb-adapters](https://github.com/dotnet/systemweb-adapters) if you find any.
- If `HttpApplication.GetVaryByCustomString(...)` was customized and expected, it may be hooked up to the [output caching](https://learn.microsoft.com/aspnet/core/performance/caching/output) availabe in .NET 7 and later via some provided extension methods. See the [module sample](https://github.com/dotnet/systemweb-adapters/blob/main/samples/Modules/ModulesCore/Program.cs) for examples on how to set this up.
- `HttpContext.Error` and other exception related `HttpContext` APIs are now hooked up to be used as expected to control any errors that occurs while invoking the events.

## Custom session key serializers

When using the System.Web adapters you can customize the serialization of session values using the `ISessionKeySerializer` interface. With this release you can now register multiple implementations of `ISessionKeySerializer`, and the adapters will iterate through all of them to identify how to serialize a given key. Previous versions would only use the latest registered serializer, which made it difficult to compose different, independent serializers. Now we attempt to use each registered serializer until one succeeds. Null values, including `Nullable<>` values, can now be serialized.

The example below demonstrates how to customize the serialization of session values using multiple `ISessionKeySerializer` implementations:

```csharp
using Microsoft.AspNetCore.SystemWebAdapters;
using Microsoft.AspNetCore.SystemWebAdapters.SessionState.Serialization;

using HttpContext = System.Web.HttpContext;
using HttpContextCore = Microsoft.AspNetCore.Http.HttpContext;

internal static class SessionExampleExtensions
{
    private const string SessionKey = "array";

    public static ISystemWebAdapterBuilder AddCustomSerialization(this ISystemWebAdapterBuilder builder)
    {
        builder.Services.AddSingleton<ISessionKeySerializer>(new ByteArraySerializer(SessionKey));
        return builder.AddJsonSessionSerializer(options =>
        {
            options.RegisterKey<int>("callCount");
        });
    }

    public static void MapSessionExample(this RouteGroupBuilder builder)
    {
        builder.RequireSystemWebAdapterSession();

        builder.MapGet("/custom", (HttpContextCore ctx) =>
        {
            return GetValue(ctx);

            static object? GetValue(HttpContext context)
            {
                if (context.Session![SessionKey] is { } existing)
                {
                    return existing;
                }

                var temp = new byte[] { 1, 2, 3 };
                context.Session[SessionKey] = temp;
                return temp;
            }
        });

        builder.MapPost("/custom", async (HttpContextCore ctx) =>
        {
            using var ms = new MemoryStream();
            await ctx.Request.Body.CopyToAsync(ms);

            SetValue(ctx, ms.ToArray());

            static void SetValue(HttpContext context, byte[] data)
                => context.Session![SessionKey] = data;
        });

        builder.MapGet("/count", (HttpContextCore ctx) =>
        {
            var context = (HttpContext)ctx;

            if (context.Session!["callCount"] is not int count)
            {
                count = 0;
            }

            context.Session!["callCount"] = ++count;

            return $"This endpoint has been hit {count} time(s) this session";
        });
    }

    /// <summary>
    /// This is an example of a custom <see cref="ISessionKeySerializer"/> that takes a key name and expects the value to be a byte array.
    /// </summary>
    private sealed class ByteArraySerializer : ISessionKeySerializer
    {
        private readonly string _key;

        public ByteArraySerializer(string key)
        {
            _key = key;
        }

        public bool TryDeserialize(string key, byte[] bytes, out object? obj)
        {
            if (string.Equals(_key, key, StringComparison.Ordinal))
            {
                obj = bytes;
                return true;
            }

            obj = null;
            return false;
        }

        public bool TrySerialize(string key, object? value, out byte[] bytes)
        {
            if (string.Equals(_key, key, StringComparison.Ordinal) && value is byte[] valueBytes)
            {
                bytes = valueBytes;
                return true;
            }

            bytes = Array.Empty<byte>();
            return false;
        }
    }
}
```

## IHtmlString support

We've added `System.Web.IHtmlString` support to .NET 8 to enable scenarios where people may be relying on it for `System.Web.HtmlUtility` behavior. As part of this, the adapters now contain `System.Web.HtmlString`, as well as a .NET Standard 2.0 `System.Web.IHtmlString` to facillitate usage in migration scenarios. `IHtmlString` currently forwards on framework to the in box version, and when .NET 8 is released will forward to that one as well allowing seamless use of the type in upgrade scenarios.

## Additional APIs

A number of additional APIs have been added:

- `IHttpModule`, `HttpApplication`, `HttpApplicationState` and other module related types
- Additional overloads of `HttpContext.RewritePath`
- Expansion of the `HttpContextBase`, `HttpRequestBase`, and `HttpResponseBase` types
- `HttpRequest.FilePath` and `HttpContext.PathInfo` is now supported via the `HttpContext.RewritePath`

We want to thank [Steven De Kock](https://github.com/sdekock), [Ruiyang Li](https://github.com/Elderry), [Clounea](https://github.com/Clounea), and [Cynthia MacLeod](https://github.com/CZEMacLeod) for their contributions to this release!

## Incremental migration guidance

As part of this release, we're also updating some guidance around [incremental migration](https://learn.microsoft.com/aspnet/core/migration/inc/overview). Some of the key areas are:

### Improved Blazor fallback routing

Blazor apps typically use a fallback route that routes any requests to the root of the app so they can be handled by client-side routing. This makes it difficult to use Blazor for incremental migration because YARP doesn't get a chance to proxy unhandled requests. In .NET 8 the routing support in Blazor is getting improved to handle this situation better, but for .NET 6 & 7 we now have [guidance](https://learn.microsoft.com/aspnet/core/migration/inc/blazor) on how to refine the Blazor fallback route so that it works with incremental migration.

### Incremental ASP.NET Web Forms migration

Upgrading from ASP.NET Web Forms to ASP.NET Core is challenging because ASP.NET Core doesn't support the Web Forms programming model. You can incrementally upgrade .aspx pages to ASP.NET Core, but you'll need to reimplement the UI rendering logic using a supported ASP.NET Core framework, like Razor Pages or Blazor.

With .NET 7 you can now incrementally replace Web Forms controls on a page with Blazor components using the new custom elements support. If using the incremental migration approach with YARP, [Razor components](https://learn.microsoft.com/aspnet/core/blazor/components/js-spa-frameworks) may be used to incrementally migrate Web Forms controls to Blazor controls and place them on `.aspx` pages instead.

For an example of this, see the [sample](https://github.com/dotnet/systemweb-adapters/tree/main/samples/WebFormsToBlazor) in the `dotnet/systemweb-adapters` repo.

### A/B Testing of Migrated Endpoints

As we worked with customers to try out the migration recommendations, a common thread emerged as to how to validate endpoints. We've added some [docs](https://learn.microsoft.com/aspnet/core/migration/inc/abtesting) on how to disable endpoints at runtime to fallback to the ASP.NET application. This can be used in cases where you want to A/B test for a given population, or if you decide you're not happy with the migrated implementation.

## Summary

Release v1.2 of the System.Web adapters brings some new features and bug fixes, including support for simpler migration of `IHttpModule` implementations. Please engage with us at https://github.com/dotnet/systemweb-adapters - we welcome any issues you face and/or PRs to help move it forward!

## Additional links

- [GitHub project](https://github.com/dotnet/systemweb-adapters)
- [Incremental migration samples](https://github.com/dotnet/systemweb-adapters/tree/main/samples)
- [Migrate from ASP.NET to ASP.NET Core docs](https://docs.microsoft.com/aspnet/core/migration/proper-to-2x)
