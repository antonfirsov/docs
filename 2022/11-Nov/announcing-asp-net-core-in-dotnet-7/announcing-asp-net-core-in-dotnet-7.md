---
post_title: Announcing ASP.NET Core in .NET 7
author1: danroth27
post_slug: announcing-asp-net-core-in-dotnet-7
username: danroth27
microsoft_alias: daroth
featured_image: ./3d-dotnet-bot-jetpack.png
categories: .NET, ASP.NET, ASP.NET Core, Blazor
tags: .net 7
post_date: 2022-11-08 08:59:50
summary: .NET 7 is now available! Check out all the new features and improvements in ASP.NET Core in .NET 7.
desired_publication_date: 2022-11-08
---

.NET 7 is now [released](https://devblogs.microsoft.com/dotnet/announcing-dotnet-7)! ASP.NET Core in .NET 7 includes everything you need to build rich modern web UI and powerful back-end services.

## What's new?

Here's a sampling of the great new features and improvements in ASP.NET Core for .NET 7:

- **Servers and runtime**
    - **[Rating limiting](https://learn.microsoft.com/aspnet/core/performance/rate-limit?view=aspnetcore-7.0)**: Limit the rate of handled requests using flexible endpoint configuration and policies.
    - **[Output caching](https://learn.microsoft.com/aspnet/core/performance/caching/output?view=aspnetcore-7.0)**: Configure caching for responses to more efficiently handle requests.
    - **[Request decompression](https://learn.microsoft.com/aspnet/core/fundamentals/middleware/request-decompression?view=aspnetcore-7.0)**: Accept requests with compressed content.
    - **[HTTP/3](https://learn.microsoft.com/aspnet/core?view=aspnetcore-7.0)**: Built-in support for HTTP/3, the latest HTTP version based on the new QUIC multiplexed transport protocol.
    - **[WebSockets over HTTP/2](https://learn.microsoft.com/aspnet/core/fundamentals/websockets?view=aspnetcore-7.0#http2-websockets-support)**: Use WebSockets over HTTP/2 connections.
    - **[WebTransport (experimental)](https://devblogs.microsoft.com/dotnet/experimental-webtransport-over-http-3-support-in-kestrel/)**: Create streams and data grams over HTTP/3 with experimental support for [WebTransport](https://datatracker.ietf.org/doc/html/draft-ietf-webtrans-http3/).
- **Minimal APIs**
    - **[Endpoint filters](https://learn.microsoft.com/aspnet/core/fundamentals/minimal-apis/min-api-filters?view=aspnetcore-7.0)**: Use endpoint filters to run cross-cutting code before or after a route handler.
    - **[Typed results](https://learn.microsoft.com/aspnet/core/fundamentals/minimal-apis?view=aspnetcore-7.0#typed-results)**: Return strongly typed results from minimal APIs.
    - **[Route groups](https://learn.microsoft.com/aspnet/core/fundamentals/minimal-apis?view=aspnetcore-7.0#route-groups)**: Organize groups of endpoints with a common prefix
- **gRPC**
    - **[JSON transcoding](https://learn.microsoft.com/aspnet/core/grpc/json-transcoding?view=aspnetcore-7.0)**: Expand the reach of your gRPC services by also exposing them as JSON-based APIs
    - **[OpenAPI with JSON transcoding (experimenal)](https://learn.microsoft.com/aspnet/core/grpc/json-transcoding-openapi?view=aspnetcore-7.0)**: Use experimental support for generating OpenAPI specs for your gRPC JSON transcoded services.
    - **[gRPC health checks](https://learn.microsoft.com/aspnet/core/grpc/health-checks?view=aspnetcore-7.0)**: Report and check the health of gRPC server apps.
    - **[gRPC client `AddCallCredentials`](https://learn.microsoft.com/aspnet/core/grpc/authn-and-authz?view=aspnetcore-7.0#bearer-token-with-grpc-client-factory)**: Create clients that send authorized requests using bearer tokens.
- **SignalR**
    - **[Client results](https://learn.microsoft.com/aspnet/core/signalr/hubs?view=aspnetcore-7.0#client-results)**: Return client results to the server in response to requests from the server.
- **MVC**
    - **[Nullable view and page models](https://learn.microsoft.com/aspnet/core/release-notes/aspnetcore-7.0?view=aspnetcore-7.0#support-for-nullable-models-in-mvc-views-and-razor-pages)**: Nullable page and view models are now supported to improve the experience when using null state checking.
- **Blazor**
    - **[Custom elements](https://learn.microsoft.com/aspnet/core/blazor/components/?view=aspnetcore-7.0#blazor-custom-elements)**: Build standard HTML custom elements with Blazor to integrate Blazor components with any JavaScript-based app.
    - **[Handle location changing events](https://learn.microsoft.com/aspnet/core/blazor/fundamentals/routing?view=aspnetcore-7.0#handleprevent-location-changes)**: Intercept location changing events to create custom user experiences when navigating.
    - **[Bind after/get/set modifiers](https://learn.microsoft.com/aspnet/core/blazor/components/data-binding?view=aspnetcore-7.0)**: Run async logic after data binding and independently control how data binding gets and sets the data.
    - **[Dynamic authentication requests](https://learn.microsoft.com/aspnet/core/blazor/security/webassembly/additional-scenarios?view=aspnetcore-7.0#custom-authentication-request-scenarios)**: Create dynamic authentication requests at runtime with custom parameters to handle advanced authentication scenarios in Blazor WebAssembly apps.
    - **[Improved JavaScript interop on WebAssembly](https://learn.microsoft.com/aspnet/core/blazor/javascript-interoperability/import-export-interop?view=aspnetcore-7.0)**: Optimize JavaScript interop call when running on WebAssembly using the new `[JSImport]`/`[JSExport]` support.
    - **[WebAssembly SIMD & exception handling](https://learn.microsoft.com/aspnet/core/blazor/tooling?view=aspnetcore-7.0&pivots=windows#net-webassembly-build-tools-1)**: Improve performance with .NET WebAssembly ahead-of-time (AOT) compilation using WebAssembly SIMD and exception handling support.

For a full list of everything that's new in ASP.NET Core in .NET 7, check out the [ASP.NET Core in .NET 7 release notes](https://learn.microsoft.com/aspnet/core/release-notes/aspnetcore-7.0).

## Get started

To get started with ASP.NET Core in .NET 7, [install the .NET 7 SDK](https://dotnet.microsoft.com/download). .NET 7 is also included with Visual Studio 2022. Mac users should use the latest Visual Studio 2022 for Mac preview.

## Upgrade an existing project

To upgrade an existing ASP.NET Core app from .NET 6 to .NET 7, follow the steps in [Migrate from ASP.NET Core 6.0 to 7.0](https://learn.microsoft.com/aspnet/core/migration/60-70)

To upgrade an existing ASP.NET Core app from .NET 7 RC2 to .NET 7, update all ASP.NET Core package references to `7.0.0`.

That's it! You should be all set to enjoy the benefits of .NET 7.

See also the full list of [breaking changes](https://learn.microsoft.com/dotnet/core/compatibility/7.0) in ASP.NET Core for .NET 7.

## Join us for the .NET 7 release at .NET Conf 2022

Come celebrate with us and learn all about the .NET 7 release at [.NET Conf 2022](https://dotnetconf.net), a FREE, three day virtual developer event with over 80 sessions featuring speakers from the .NET team and the broader .NET community. The conference starts TODAY and goes from November 8-10. We hope you can join us!

## .NET 7 on Azure

.NET 7 is already deployed and ready to be used across your favorite Azure services, like [Azure App Service](https://go.microsoft.com/fwlink/?linkid=2214434), [Azure Functions](https://go.microsoft.com/fwlink/?linkid=2214834), and [Azure Static Web Apps](https://aka.ms/swa-dotnet7-ga). Get started building with .NET 7 on Azure today!

## Thank you!

Thank you to everyone in the community who helped make this release of .NET 7 possible! This release represents the culmination of many GitHub issues, pull requests, design feedback comments and documentation updates contributed by many members of the .NET community. We couldn't have made it to this point without you!

We hope you enjoy this release of ASP.NET Core in .NET 7. We're eager to hear about your experiences building with it. Let us know about any feedback you have on this release on [GitHub](https://github.com/dotnet/aspnetcore/issues).

Thanks again, and happy coding!
