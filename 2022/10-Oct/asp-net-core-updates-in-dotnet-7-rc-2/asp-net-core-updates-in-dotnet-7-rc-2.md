---
post_title: ASP.NET Core updates in .NET 7 Release Candidate 2
author1: daroth@microsoft.com
post_slug: asp-net-core-updates-in-dotnet-7-rc-2
username: daroth@microsoft.com
microsoft_alias: daroth
featured_image: ./dotnet-bot_scene_space-rocks.png
categories: .NET, ASP.NET, ASP.NET Core, Blazor
summary: .NET 7 Release Candidate 2 is now available! Check out what's new in ASP.NET Core in this update.
desired_publication_date: 2022-10-11
post_date: 2022-10-11 10:00:00
---

[.NET 7 Release Candidate 2 (RC2) is now available](https://devblogs.microsoft.com/dotnet/announcing-dotnet-7-rc-2) and includes many great new improvements to ASP.NET Core.

Here's a summary of what's new in this preview release:

- Output caching improvements
- Dynamic authentication requests with msal.js
- Improved diagnostics for authentication in Blazor WebAssembly
- WebAssembly multithreading (experimental)

For more details on the ASP.NET Core work planned for .NET 7 see the full [ASP.NET Core roadmap for .NET 7](https://aka.ms/aspnet/roadmap) on GitHub.

## Get started

To get started with ASP.NET Core in .NET 7 Release Candidate 2, [install the .NET 7 SDK](https://dotnet.microsoft.com/download/dotnet/7.0).

If you're on Windows using Visual Studio, we recommend installing the latest [Visual Studio 2022 preview](https://visualstudio.com/preview). If you're on macOS, we recommend installing the latest [Visual Studio 2022 for Mac preview](https://visualstudio.microsoft.com/vs/mac/preview/).

To install the latest .NET WebAssembly build tools, run the following command from an elevated command prompt:

```console
dotnet workload install wasm-tools
```

## Upgrade an existing project

To upgrade an existing ASP.NET Core app from .NET 7 RC1 to .NET 7 RC2:

- Update all Microsoft.AspNetCore.\* package references to `7.0.0-rc.2.*`.
- Update all Microsoft.Extensions.\* package references to `7.0.0-rc.2.*`.

See also the full list of [breaking changes](https://docs.microsoft.com/dotnet/core/compatibility/7.0#aspnet-core) in ASP.NET Core for .NET 7.

## Output caching improvements

Output caching was improved in .NET 7 RC2 based on preview feedback from users.

### Binary serialization

In this version, output caching stores the content of the cached entries in a binary format. Previously it was using JSON. This new format is faster and smaller.

### Option to not vary by host

By default, cached entries are unique per HTTP domain name. A few users reported the need for cached entries to be shared across domain names. For this purpose, we introduced a new `SetVaryByHost` option:

```csharp
builder.Services.AddOutputCache(options =>
{
    options.AddBasePolicy(builder => builder.SetVaryByHost(false));
}
```

### Improved vary-by rules

Some changes were made to the API to make its usage simpler and more consistent.

For example, you can now create an `OutputCachePolicyBuilder` instance that doesn't contain the default policy, so you can specify your own logic.

We also prefixed most builder methods with `SetXxxxx` to make it obvious these methods will update an existing property of the builder:

```csharp
options.AddBasePolicy(b => b.SetVaryByQuery("*").SetLocking(true));
```

## Dynamic authentication requests with msal.js

In .NET 7 RC1 we introduced support for making dynamic authentication requests with custom parameters in Blazor WebAssembly apps. With .NET 7 RC2, this functionality is now available when using the Microsoft Identity Platform via the built-in msal.js integration. For details on making dynamic authentication requests in Blazor WebAssembly see the [Dynamic authentication requests in Blazor WebAssembly](https://devblogs.microsoft.com/dotnet/asp-net-core-updates-in-dotnet-7-rc-1/#dynamic-authentication-requests-in-blazor-webassembly) section of the .NET 7 RC1 release announcement post.

## Improved diagnostics for authentication in Blazor WebAssembly

To help diagnose authentication issues in Blazor WebAssembly apps we added detailed logging that you can enable using the following logging configuration:

```json
"Logging": {
    "LogLevel": {
        "Microsoft.AspNetCore.Components.WebAssembly.Authentication": "Debug"
    }
}
```

See [Logging configuration](https://learn.microsoft.com/aspnet/core/blazor/fundamentals/configuration?view=aspnetcore-6.0#logging-configuration) for additional details on how to configure logging in Blazor WebAssembly apps.

## WebAssembly multithreading (experimental)

The `wasm-experimental` workload now includes support for multi-threaded .NET apps on WebAssembly using [Web Workers](https://developer.mozilla.org/en-US/docs/Web/API/Web_Workers_API/Using_web_workers). This is a new .NET runtime capability. Multithreading support hasn't been integrated yet into Blazor WebAssembly apps (planned for .NET 8), but you can still try it out in preview form using the experimental WebAssembly Browser App template.

To use WebAssembly multithreading:

- Install the `wasm-experimental` workload:

    ```console
    dotnet workload install wasm-experimental
    ```

- Create a new `wasmbrowser` app:

    ```console
    dotnet new wasmbrowser
    ```

- Add the `WasmEnableThreads` property to the project file to enable threading:

    ```xml
    <PropertyGroup>
      <WasmEnableThreads>true</WasmEnableThreads>
    </PropertyGroup>
    ```

-  Add a reference to the Microsoft.NET.WebAssembly.Threading package. This package provides reference assemblies that prevent warnings about using threading APIs in the browser:

    ```console
    dotnet add package --prerelease Microsoft.NET.WebAssembly.Threading
    ```

- Create and run a new thread.

    ```csharp
    using System;
    using System.Threading;
    using System.Runtime.Versioning;
    using System.Runtime.InteropServices.JavaScript;

    [assembly:SupportedOSPlatform("browser")]

    new Thread(SecondThread).Start();
    Console.WriteLine($"Hello, Browser from the main thread {Thread.CurrentThread.ManagedThreadId}");

    static void SecondThread()
    {
        Console.WriteLine($"Hello from Thread {Thread.CurrentThread.ManagedThreadId}");
        for (int i = 0; i < 5; ++i)
        {
            Console.WriteLine($"Ping {i}");
            Thread.Sleep(1000);
        }
    }
    ```

- Run the app and browser to the app URL:

   ```console
   dotnet run
   ```

- (Optional) Publish and serve the app with a web server such as [dotnet-serve](https://www.nuget.org/packages/dotnet-serve). Note that [cross origin isolation](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/SharedArrayBuffer#security_requirements) must be enabled by serving the COOP and COEP headers

   ```console
   dotnet publish -c Release
   dotnet serve -h "Cross-Origin-Opener-Policy:same-origin" -h "Cross-Origin-Embedder-Policy:require-corp" --directory bin/Release/net7.0/browser-wasm/AppBundle/
   ```

- Open the served URL in a browser and view the output in the browser developer console. (note: Chrome, Edge or Safari; at this time Firefox is not supported until [Mozilla #1540913](https://bugzilla.mozilla.org/show_bug.cgi?id=1540913) is fixed)

    ```console
    Hello from Thread 2
    Hello, Browser from the main thread 1
    Ping 0
    Ping 1
    Ping 2
    Ping 3
    Ping 4
    ```

The completed example is available at https://github.com/lambdageek/hithread.

Notes and known issues with .NET multithreading on WebAssembly:

- By default, the app will create a pool of four Web Workers to execute .NET threads. To control the number of workers that are created at app startup, configure the `_WasmPThreadPoolSize` property (subject to change in a future release). The maximum parallelism of your app must not exceed the worker pool size.
- A `SynchronizationContext` is used on the browser thread by default.  If you do not use `ConfigureAwait(false)` when awaiting asynchronous operations, they will be scheduled on the browser thread.  Conversely, not using `ConfigureAwait` allows your asynchronous operations to return to the browser thread in order to interact with the DOM or other JS libraries that are only available on the main thread.
- Multithreading requires that [cross origin isolation](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/SharedArrayBuffer#security_requirements) is enabled by serving the COOP and COEP headers. This will restrict the functionality of the site: Enabling cross-origin isolation blocks loading cross-origin resources that you don't explicitly opt-in, and it will prevent your top-level document from being able to communicate with popup windows.
- The build properties `RunAOTCompilation` and `WasmEnableSIMD` are supported with multithreading.
- `JSImport` and `JSExport` only work from the main browser thread.
- WebSocket operations are only supported on the main browser thread.
- Debugging multithreaded code on WebAssembly is not yet supported.
- Threading does not work in Firefox yet due to [Bugzilla #1540913](https://bugzilla.mozilla.org/show_bug.cgi?id=1540913).

For more details on the progress of WebAssembly multithreading support, see https://github.com/dotnet/runtime/issues/68162.

## Give feedback

We hope you enjoy this preview release of ASP.NET Core in .NET 7. Let us know what you think about these new improvements by filing issues on [GitHub](https://github.com/dotnet/aspnetcore/issues/new).

Thanks for trying out ASP.NET Core!