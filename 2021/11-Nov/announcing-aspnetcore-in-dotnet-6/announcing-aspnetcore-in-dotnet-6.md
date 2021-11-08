---
post_title: Announcing ASP.NET Core in .NET 6
username: daroth@microsoft.com
microsoft_alias: daroth
featured_image: dotnet-bot_asp.net.png
categories: ASP.NET, ASP.NET Core, Azure, Blazor
summary: .NET 6 is now available! Check out all the new features and improvements in ASP.NET Core in .NET 6.
desired_publication_date: 2021-11-08
---

.NET 6 is now [released](https://devblogs.microsoft.com/dotnet/announcing-dotnet-6)! ASP.NET Core in .NET 6 includes everything you need to build rich modern web UI and powerful back-end services.

## What's new?

Here's a sampling of the great new features and improvements in ASP.NET Core for .NET 6:

- **[Hot reload](https://docs.microsoft.com/aspnet/core/blazor/tooling#net-hot-reload)**: Apply changes to Razor, C#, and CSS source files into your running app during development without the need to rebuild and restart the app.
- **Minimal APIs**: Create a new ASP.NET Core app with just a few lines of code using the latest C# features and a simplified hosting model.
- **Async streaming**: Asynchronously stream data from the server without any need for buffering.
- **IAsyncDisposable**: Support for `IAsyncDisposable` on controllers, page models, and view components.
- **Bootstrap 5.1**: ASP.NET Core now comes with integrated Bootstrap 5.1 support.
- **Null-state analysis**: All ASP.NET Core templates now have C# null-state analysis enabled by default.
- **[CSS isolation for pages and views](https://docs.microsoft.com/aspnet/core/razor-pages/#css-isolation)**: Scope CSS styles to specific pages or views using *.cshtml.css* files.
- **[JavaScript modules](https://docs.microsoft.com/aspnet/core/blazor/javascript-interoperability/#load-a-script-from-an-external-javascript-file-js-collocated-with-a-component)**: Place related JavaScript modules alongside pages, views, and components using *.cshtml.js* and *.razor.js* files.
- **Blazor improvements**:
  - [Render components from JavaScript](https://docs.microsoft.com/aspnet/core/blazor/components/#render-razor-components-from-javascript)
  - [Preserve prerendered state](https://docs.microsoft.com/aspnet/core/blazor/components/prerendering-and-integration#preserve-prerendered-state)
  - [Error boundaries](https://docs.microsoft.com/aspnet/core/blazor/fundamentals/handle-errors#error-boundaries)
  - [Custom event args](https://docs.microsoft.com/aspnet/core/blazor/components/event-handling#custom-event-arguments)
  - [Infer generic type parameters from ancestor components](https://docs.microsoft.com/aspnet/core/blazor/components/templated-components#infer-generic-types-based-on-ancestor-components)
  - [Required component parameters](https://docs.microsoft.com/aspnet/core/blazor/components/#component-parameters)
  - [Handle query string parameters](https://docs.microsoft.com/aspnet/core/blazor/fundamentals/routing#query-strings)
  - [Control HTML head content](https://docs.microsoft.com/aspnet/core/blazor/components/control-head-content)
  - [JavaScript initializers](https://docs.microsoft.com/aspnet/core/blazor/fundamentals/startup#javascript-initializers)
  - [Dynamically render components](https://docs.microsoft.com/aspnet/core/blazor/components/dynamiccomponent)
- **[.NET WebAssembly build tools](https://docs.microsoft.com/aspnet/core/blazor/host-and-deploy/webassembly#ahead-of-time-aot-compilation)**: Ahead-of-time (AOT) compilation for Blazor WebAssembly apps, as well as support for runtime relinking and native dependencies.
- **Single-page apps**: Built-in support for Angular 12 and React 17 based on a flexible template pattern that can be used with other popular frontend JavaScript frameworks.
- **Socket control**: More control over socket creation and handling.
- **Strongly-typed headers**: Accessing HTTP headers in a strongly-typed way.
- **[HTTP](https://docs.microsoft.com/aspnet/core/fundamentals/http-logging) & [W3C logging](https://docs.microsoft.com/aspnet/core/fundamentals/w3c-logger)**: Log HTTP traffic, and log using the W3C Extended Log File Format.
- **[HTTP/3 (Preview)](https://docs.microsoft.com/aspnet/core/fundamentals/servers/kestrel/http3)**: Preview of server support for HTTP/3 based on the new QUIC transport.

For a full list of everything that's new in ASP.NET Core in .NET 6, check out the [ASP.NET Core in .NET 6 release notes](https://docs.microsoft.com/aspnet/core/release-notes/aspnetcore-6.0).

## Get started

To get started with ASP.NET Core in .NET 6, [install the .NET 6 SDK](https://dotnet.microsoft.com/download). .NET 6 is also included with Visual Studio 2022. Mac users should use the latest Visual Studio 2022 for Mac preview.

## Upgrade an existing project

To upgrade an existing ASP.NET Core app from .NET 5 to .NET 6, follow the steps in [Migrate from ASP.NET Core 5.0 to 6.0](https://docs.microsoft.com/aspnet/core/migration/50-to-60)

To upgrade an existing ASP.NET Core app from .NET 6 RC2 to .NET 6, update all package references to `6.0.0`.

That's it! You should be all set to enjoy the benefits of .NET 6.

See also the full list of [breaking changes](https://docs.microsoft.com/dotnet/core/compatibility/6.0) in ASP.NET Core for .NET 6.

## Join us for the .NET 6 release at .NET Conf 2021

Come celebrate with us and learn all about the .NET 6 release at [.NET Conf 2021](https://dotnetconf.net), a FREE, three day virtual developer event with over 80 sessions featuring speakers from the .NET team and the broader .NET community. The conference starts tomorrow and goes from November 9-11. We hope you can join us!

## Azure Functions, Web Apps, and Static Web Apps support .NET 6

Azure App Service teams have been working around the clock to make sure .NET 6 is supported across your favorite Azure PaaS services like Web Apps and Functions, so you don't have to wait to deploy. At the time of this post, .NET 6 is being actively deployed to the worldwide network of servers and configured to build and run .NET 6 apps.

For more information on the various Azure services and their roll-out of supporting .NET 6, which should conclude by the end of this week, see the following links:

- Azure Functions now supports [running serverless functions in .NET 6](https://go.microsoft.com/fwlink/?linkid=2178604)
- The [App Service .NET 6 GA Announcement](https://go.microsoft.com/fwlink/?linkid=2178304) has information and details for ASP.NET Core developers excited to get going with .NET 6 today. One thing to note - if you're already running a .NET 6 preview build on App Service, your app will be auto-updated on the first restart once the .NET 6 runtime and SDK are deployed to your region - you don't have to rebuild or do anything, it'll just update if you're not running a self-contained app.
- Azure Static Web Apps now supports [full-stack .NET 6 applications with Blazor WebAssembly frontends and Azure Function APIs](https://go.microsoft.com/fwlink/?linkid=2178605)

We're sure you'll enjoy the immediate availability of Azure App Service to run your ASP.NET Core and Serverless .NET apps. And don't forget to try using [Azure Container Apps](https://azure.microsoft.com/services/container-apps/) if you're keen on building some background microservices using the [.NET Core Worker Service](https://docs.microsoft.com/dotnet/core/extensions/workers) template, too.

## Thank you!

Thank you to everyone in the community who helped make this release of .NET 6 possible! This release represents the culmination of many GitHub issues, pull requests, design feedback comments and documentation updates contributed by many members of the .NET community. We couldn't have made it to this point without you!

We hope you enjoy this release of ASP.NET Core in .NET 6. We're eager to hear about your experiences building with it. Let us know about your creative efforts on [GitHub](https://github.com/dotnet/aspnetcore/issues).

Thanks again, and happy coding!
