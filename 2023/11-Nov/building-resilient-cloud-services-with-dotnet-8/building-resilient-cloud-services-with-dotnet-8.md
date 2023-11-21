---
post_title: Building resilient cloud services with .NET 8
author1: martintomka
post_slug: building-resilient-cloud-services-with-dotnet-8
microsoft_alias: martintomka
featured_image: dotnet8_blog_image.png
categories: .NET, .NET Core, .NET Fundamentals, ASP.NET Core, C#, Networking
tags: .NET, HTTP, resilience, IHttpClientFactory, cloud, scale, Polly, HttpClient, C#
summary: How to integrate resilience and into HTTP Client
post_date: 2023-11-28 10:05:00
---

Building resilient apps is a fundamental requirement for cloud development. With .NET 8, we've made substantial advancements to simplify the integration of resilience into your applications. We're excited to introduce the [`Microsoft.Extensions.Http.Resilience`](https://www.nuget.org/packages/Microsoft.Extensions.Http.Resilience) and [`Microsoft.Extensions.Resilience`](https://www.nuget.org/packages/Microsoft.Extensions.Resilience) packages to the .NET ecosystem. These new libraries are based on the [Polly](https://github.com/App-vNext/Polly) library, a widely recognized open-source project.

## TL;DR

To use the new HTTP resilience APIs, install the package from the command line:

```dotnetcli
dotnet add package Microsoft.Extensions.Http.Resilience
```

Or add it directly in the C# project file:

```xml
<ItemGroup>
  <PackageReference Include="Microsoft.Extensions.Http.Resilience" />
</ItemGroup>
```

You can now use the recommended `AddStandardResilienceHandler` extension on the `IHttpClientBuilder`:

```csharp
var services = new ServiceCollection();

services.AddHttpClient("my-client")
        .AddStandardResilienceHandler(options =>
        {
            // Configure standard resilience options here
        });
```

Example above uses `AddStandardResilienceHandler` to add a pipeline (rate limiter, total timeout, retry, circuit breaker, attempt timeout) of resilience strategies to the HTTP client. See [standard resilience pipeline](#standard-resilience-pipeline) section to learn more.

A more real-world example would rely on hosting, such as that described in the [.NET Generic Host](https://learn.microsoft.com/dotnet/core/extensions/generic-host) article. Using the [Microsoft.Extensions.Hosting](https://www.nuget.org/packages/Microsoft.Extensions.Hosting) NuGet package, the above example becomes:

``` csharp
using Http.Resilience.Example;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
IServiceCollection services = builder.Services;

services.AddHttpClient("my-client")
        .AddStandardResilienceHandler(options =>
        {
            // Configure standard resilience options here
        });

// Use the client
var host = builder.Build();
var httpClient = host.Services
    .GetRequiredService<IHttpClientFactory>()
    .CreateClient("my-client");

// Make resilient HTTP request
HttpResponseMessage response = await httpClient.GetAsync("https://jsonplaceholder.typicode.com/comments");
```

> To use the registered `my-client`, resolve `IHttpClientFactory` from dependency injection and use the `CreateClient` method to create an HTTP client.

For advanced scenarios, the APIs support building your own custom HTTP resilience pipeline:

```csharp
services.AddHttpClient("my-client")
        .AddResilienceHandler("my-pipeline", builder =>
        {
            // Refer to https://www.pollydocs.org/strategies/retry.html#defaults for retry defaults
            builder.AddRetry(new HttpRetryStrategyOptions
            {
                MaxRetryAttempts = 4,
                Delay = TimeSpan.FromSeconds(2),
                BackoffType = DelayBackoffType.Exponential
            });

            // Refer to https://www.pollydocs.org/strategies/timeout.html#defaults for timeout defaults
            builder.AddTimeout(TimeSpan.FromSeconds(5));
        });
```

## Why resilience is important

Services often rely on the HTTP protocol to make remote requests. These requests can occasionally fail due to network issues or problems on the server side. If not addressed properly, these failures can impact the service's availability. As your service integrates more remote dependencies, the likelihood of cascading failures, where failures in one system trigger failures in others, increases. Learn more about [cascading failures](https://en.wikipedia.org/wiki/Cascading_failure).

To bolster the resilience of your service, especially concerning outgoing HTTP requests, you can employ several strategies:

- **Set timeouts for outgoing requests.** After a certain period, it's more efficient to cancel the request than to continue waiting.
- **Retry transient failures.** Some requests might fail due to temporary network glitches or fleeting server-side errors. Instead of letting the operation fail, consider retrying the request.
- **Pause communication during remote service outages.** If a remote service is temporarily unavailable, it may be wise to halt all communication temporarily and resume once the service is back online.
- **Establish fallback actions.** Design actions to execute when primary operations fail.

You can use any of these resilience strategies individually or combine them for outgoing HTTP requests to achieve the best results.

## .NET resilience and Polly

When discussing resilience in the .NET ecosystem, one cannot overlook the [Polly library](https://github.com/App-vNext/Polly). For years, it has been the go-to resilience solution for the .NET community. Microsoft teamed up with the Polly community to develop the new Polly v8 version, which now forms the backbone of our latest resilience libraries. The `Microsoft.Extensions.Http.Resilience` package provides a concise, HTTP-focused layer atop the Polly library, embracing many of Polly's newest features. For a comprehensive look at the Polly library, please visit [pollydocs.org](https://www.pollydocs.org/).

To use the resilience strategies mentioned earlier, there's no additional steps required. The Polly library readily offers a variety of [resilience strategies](https://www.pollydocs.org/strategies) and even lets you combine them into [resilience pipelines](https://www.pollydocs.org/pipelines).

Here's a simple example illustrating how to define a resilience pipeline that integrates both timeouts and retries:

``` csharp
ResiliencePipeline<HttpResponseMessage> pipeline = new ResiliencePipelineBuilder<HttpResponseMessage>()
    // For retry defaults, see: https://www.pollydocs.org/strategies/retry.html#defaults 
    .AddRetry(new RetryStrategyOptions<HttpResponseMessage>
    {
        MaxRetryAttempts = 4,
        Delay = TimeSpan.FromSeconds(2),
        BackoffType = DelayBackoffType.Exponential
    })
    // For timeout defaults, see: https://www.pollydocs.org/strategies/timeout.html#defaults 
    .AddTimeout(TimeSpan.FromSeconds(5))
    .Build();

// Using the pipeline
await pipeline.ExecuteAsync(
    async cancellationToken => await httpClient.GetAsync("https://jsonplaceholder.typicode.com"), cancellationToken);
```

In the above example, we use the `ResiliencePipelineBuilder<HttpResponseMessage>` to create a pipeline. This pipeline can then support the execution of any user-defined callbacks yielding `HttpResponseMessage` results.

## Resilience journey

Microsoft operates some of the world's largest services. Polly is widely admired and used across many of these services. In our endeavor to unify services, we developed internal libraries that incorporate Polly, enhancing it with features like telemetry, dependency injection support, and options-based configuration. Recognizing that the broader .NET community could benefit from these enhancements, we initiated a dialogue with Polly's maintainers. After presenting a demo integrating our improvements directly into Polly, it became evident that while these changes would enhance the Polly codebase, they would require a completely new API. This evolution culminated in the release of Polly v8, now [officially available](https://www.thepollyproject.org/2023/09/28/polly-v8-officially-released/).

With Polly v8, a new [Polly.Core](https://www.nuget.org/packages/Polly.Core) package was launched, preserving the legacy API within the [Polly](https://www.nuget.org/packages/Polly) package. The `Polly.Core` package stands independently, offering minimal dependencies and retaining all the previous version's features while introducing advancements like built-in telemetry, a zero-allocation API, unified execution, and a fluent syntax. Furthermore, the [Polly.Extensions](https://www.nuget.org/packages/Polly.Extensions) package now provides seamless integration with [`IServiceCollection`](https://learn.microsoft.com/dotnet/core/extensions/dependency-injection) and facilitates the export of Polly's telemetry events into [.NET metrics](https://www.pollydocs.org/advanced/telemetry.html#metrics).

The new HTTP resilience packages are build upon the foundations of Polly, presenting the .NET community with dedicated and refined HTTP-based resilience APIs.

### Resilience packages

- [`Microsoft.Extensions.Resilience`](https://www.nuget.org/packages/Microsoft.Extensions.Resilience): This package provides a minimal set of APIs. Its primary purpose is to enrich [Polly's metrics](https://www.pollydocs.org/advanced/telemetry.html#metrics) using the `AddResilienceEnricher` extension for `IServiceCollection`. For more details, refer to the [Resilience docs](https://learn.microsoft.com/dotnet/core/resilience).
- [`Microsoft.Extensions.Http.Resilience`](https://www.nuget.org/packages/Microsoft.Extensions.Http.Resilience): This package offers HTTP-specific APIs that integrate with Polly v8 and `IHttpClientFactory`. It is the successor to the `Microsoft.Extensions.Http.Polly` package and is recommended for all new projects. See the section below for more information.
- [`Microsoft.Extensions.Http.Polly`](https://www.nuget.org/packages/Microsoft.Extensions.Http.Polly): This package integrates older versions of Polly with [`HttpClient`](https://learn.microsoft.com/dotnet/fundamentals/networking/http/httpclient) and [`IHttpClientFactory`](https://learn.microsoft.com/dotnet/core/extensions/httpclient-factory).

## Resilient HTTP requests

To execute resilient HTTP requests, first install the [`Microsoft.Extensions.Http.Resilience`](https://www.nuget.org/packages/Microsoft.Extensions.Http.Resilience) package. This package exposes the following extensions for [`IHttpClientBuilder`](https://learn.microsoft.com/dotnet/core/extensions/httpclient-factory):

- **`AddStandardResilienceHandler`**: This adds a resilience handler with a standard resilience pipeline suitable for most scenarios.
  
- **`AddStandardHedgingHandler`**: This introduces a resilience handler with a standard hedging pipeline, which supports requests to multiple endpoints.
  
- **`AddResilienceHandler`**: This incorporates a resilience handler that allows configuration of resilience strategies in the resilience pipeline.

For further information, visit the official [Build Resilient HTTP Apps](https://learn.microsoft.com/dotnet/core/resilience/http-resilience) documentation.

## Standard resilience pipeline

The standard pipeline is the recommended resilience API to use. It combines five Polly strategies to form a resilience pipeline suitable for most scenarios. The standard pipeline contains the following strategies executed in the order below:

| Order | Strategy | Description |
|--:|--|--|
| **1** | Rate limiter | The rate limiter pipeline limits the maximum number of concurrent requests being sent to the dependency. |
| **2** | Total request timeout | The total request timeout pipeline applies an overall timeout to the execution, ensuring that the request, including retry attempts, doesn't exceed the configured limit. |
| **3** | Retry | The retry pipeline retries the request in case the dependency is slow or returns a transient error. |
| **4** | Circuit breaker | The circuit breaker blocks the execution if too many direct failures or timeouts are detected. |
| **5** | Attempt timeout | The attempt timeout pipeline limits each request attempt duration and throws if it's exceeded. |

To integrate the standard pipeline, use the `AddStandardResilienceHandler` extension for `IHttpClientBuilder`:

``` csharp
// Add standard resilience handler to the HTTP client
services.AddHttpClient("my-client")
        .AddStandardResilienceHandler();
```

> Invoking `AddStandardResilienceHandler` returns an `IHttpStandardResiliencePipelineBuilder` instance which exposes additional extensions for configuring the standard pipeline.

The previous example uses default options where for both retries and circuit breaker strategies, the following outcomes are handled:

- Any status code *500* or above.
- *429* (Too Many Requests).
- *408* (Request Timeout).
- Exceptions: `HttpRequestException` and `TimeoutRejectedException`.

Customizing the standard pipeline options when calling `AddStandardResilienceHandler` is also possible:

``` csharp
services
    .AddHttpClient("my-client")
    .AddStandardResilienceHandler(options =>
    {
        // Customize retry
        options.Retry.ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
            .Handle<TimeoutRejectedException>()
            .Handle<HttpRequestException>()
            .HandleResult(response => response.StatusCode == HttpStatusCode.InternalServerError);
        options.Retry.MaxRetryAttempts = 5;

        // Customize attempt timeout
        options.AttemptTimeout.Timeout = TimeSpan.FromSeconds(2);
    });
```

Alternatively, configure the options with the `Configure` extensions:

``` csharp
// Retrieve the IConfiguration
IConfiguration configuration = ...; 

services
    .AddHttpClient("my-client")
    .AddStandardResilienceHandler()
    // Configure the standard options with IConfiguration
    .Configure(configuration.GetSection("my-section"))
    // Or using callbacks
    .Configure(options => 
    {
        // Additional configuration
    });
```

The previous example:

- Employs `Configure` that takes `IConfiguration` to bind options from *my-section*.
- Uses another `Configure` showcasing the support for configuration chaining.

> The standard resilience handler supports dynamic reloading of options. If the `configuration` changes, the resilience pipeline dynamically refreshes, using the new configuration for request handling. This enhancement is enabled by the [dynamic reloads](https://www.pollydocs.org/advanced/dependency-injection.html#dynamic-reloads) feature of the Polly library.

## Standard hedging pipeline

This is a new addition to the resilience family. The standard hedging handler is similar to the standard resilience handler. However, instead of using a retry strategy, this pipeline employs a hedging strategy. Introduced in Polly v8, the hedging strategy aims to improve request latency by issuing multiple concurrent requests. See Polly's official [hedging strategy documentation](https://www.pollydocs.org/strategies/hedging.html) for more details.

The standard hedging pipeline consists of the following strategies:

| Order | Strategy | Description |
|--:|--|--|
| **1** | Total request timeout | The total request timeout pipeline applies an overall timeout to the execution, ensuring that the request, including hedging attempts, doesn't exceed the configured limit. |
| **2** | Hedging | The hedging strategy executes the requests against multiple endpoints in case the dependency is slow or returns a transient error. Routing is options, by default it just hedges the URL provided by the original <xref:System.Net.Http.HttpRequestMessage>. |
| **3** | Rate limiter (per endpoint) | The rate limiter pipeline limits the maximum number of concurrent requests being sent to the dependency. |
| **4** | Circuit breaker (per endpoint) | The circuit breaker blocks the execution if too many direct failures or timeouts are detected. |
| **5** | Attempt timeout (per endpoint) | The attempt timeout pipeline limits each request attempt duration and throws if it's exceeded. |

To use standard hedging:

``` csharp
services.AddHttpClient("my-client")
        .AddStandardHedgingHandler();
```

By default, hedging sends another request if no response is received within 2 seconds. It then waits for the quickest request to complete.

Standard hedging uses a pool of circuit breakers to ensure that requests aren't sent to unhealthy endpoints. Typically, the pool selection is based on the URL authority (scheme + host + port). In the provided example, no routing is defined for hedging, so all requests go to the URL specified in the request message.

To customize the selection of circuit breakers, use the `SelectPipelineBy` extension method:

``` csharp
services.AddHttpClient("my-client")
        .AddStandardHedgingHandler()
        .SelectPipelineBy(serviceProvider => request => request.RequestUri.Host);
```

`SelectPipelineBy` requires a factory that, when invoked, returns a function to retrieve a string from `HttpRequestMessage`. This string is then used to pool the circuit breakers.

Like the standard pipeline, you can also configure standard hedging options:

``` csharp
services
    .AddHttpClient("my-client")
    .AddStandardHedgingHandler()
    .Configure(configuration.GetSection("my-section"))
    .Configure(options => 
    {
        options.Hedging.MaxHedgedAttempts = 3;
        options.Hedging.Delay = TimeSpan.FromSeconds(1);
    });
```

In the example above:

- `Configure` binds options from *my-section* using `IConfiguration`.
- Another `Configure` demonstrates configuration chaining and adjusts the maximum number of hedged attempts to 3. This means the hedging strategy can make up to 4 requests to a remote endpoint with a 1-second delay between them.

### Standard hedging and routing

One powerful feature of the standard hedging pipeline is its ability to configure URL routes for requests. This enables hedging to send requests to different endpoints in case some are unresponsive or unhealthy, as demonstrated below:

``` csharp
services
    .AddHttpClient("my-client")
    .AddStandardHedgingHandler(routingBuilder =>
    {
        routingBuilder.ConfigureOrderedGroups(options =>
        {
            options.Groups.Add(new UriEndpointGroup
            {
                Endpoints =
                {
                    new() { Uri = new("https://example.net/api/a"), Weight = 95 },
                    new() { Uri = new("https://example.net/api/b"), Weight = 5 },
                }
            });

            options.Groups.Add(new UriEndpointGroup
            {
                Endpoints =
                {
                    new() { Uri = new("https://example.net/api/c"), Weight = 95 },
                    new() { Uri = new("https://example.net/api/d"), Weight = 5 },
                }
            });            
        });
    });
```

In this example:

- `AddStandardHedgingHandler` introduces standard hedging.
- Routes for hedging are set up within `AddStandardHedgingHandler` using the `ConfigureOrderedGroups` extension.
- Two groups with multiple endpoints are added. With ordered groups, every request selects a single endpoint from each group sequentially. After exhausting all groups, hedging stops, even if `MaxHedgedAttempts` is not met.
- The `Weight` property indicates the probability of selecting that endpoint. In the example above, there is a *95%* chance of selecting the endpoint `https://example.net/api/a` and a *5%* chance for the `https://example.net/api/b` endpoint.

> While the example employs ordered groups via `ConfigureOrderedGroups`, the API also offers `ConfigureWeightedGroups`, which permits group selection based on weight.

Visit the official [customize hedging handler route selection](https://learn.microsoft.com/dotnet/core/resilience/http-resilience?tabs=dotnet-cli#customize-hedging-handler-route-selection) documentation to learn more about routing in hedging.

### Standard hedging and unavailable endpoints

Consider this configuration:

``` csharp
services
    .AddHttpClient("my-client", client =>
    {
        client.Timeout = TimeSpan.FromSeconds(10);
        client.BaseAddress = new Uri("https://example.net");
    })
    .AddStandardHedgingHandler(routingBuilder =>
    {
        routingBuilder.ConfigureOrderedGroups(options =>
        {
            options.Groups.Add(new UriEndpointGroup
            {
                Endpoints = [new() { Uri = new("https://jsonplaceholder.typicode.com:999") }] // Unavailable endpoint
            });

            options.Groups.Add(new UriEndpointGroup
            {
                Endpoints = [new() { Uri = new("https://jsonplaceholder.typicode.com") }]
            });
        });
    })
    .Configure(options => 
    {
        options.Endpoint.CircuitBreaker.MinimumThroughput = 5;
        options.Endpoint.CircuitBreaker.SamplingDuration = TimeSpan.FromSeconds(5);
        options.Endpoint.Timeout.Timeout = TimeSpan.FromSeconds(1);
    });
```

Note the inaccessible URL `https://jsonplaceholder.typicode.com:999` representing an unhealthy endpoint.

Using the above-configured HTTP client:

``` csharp
// Create the client
HttpClient client = services.BuildServiceProvider()
    .GetRequiredService<IHttpClientFactory>()
    .CreateClient("my-client");

// Use the client
for (int i = 0; i < 10; i++)
{
    var watch = Stopwatch.StartNew();
    await client.GetStringAsync("posts");
    Console.WriteLine("{0}: {1}ms", i + 1, watch.ElapsedMilliseconds);
}
```

Results in the following console output:

``` bash
1: 1396ms
2: 1024ms
3: 1015ms
4: 1028ms
5: 1015ms
6: 14ms
7: 11ms
8: 15ms
9: 15ms
10: 13ms
```

Initial attempts are lengthy since hedging tries to obtain a response from the unavailable endpoint. After a duration aligning with the circuit breaker's `SamplingDuration`, the pipeline identifies the malfunctioning endpoint and opens its circuit breaker. When the circuit breaker is open, attempts to reach the first endpoint instantly fail, prompting an immediate request to a secondary endpoint, hence the faster subsequent attempts.

See [circuit breaker state diagram](https://www.pollydocs.org/strategies/circuit-breaker.html#state-diagram) to learn more about states of circuit breaker and the state transitions.

> In situations like the one described above, after a certain duration, you might observe increased latency for a single request. In terms of circuit breakers, this is a probing request checking endpoint health. Probing requests might exhibit increased latencies. If probing is successful, the circuit breaker is closed, allowing hedging to reconnect with the primary endpoint.

## Custom resilience pipeline

There are scenarios where the standard resilience or standard hedging pipeline might not be suitable. In such cases, you have APIs that allow you to build your own HTTP-based resilience pipeline. These APIs integrate seamlessly with the Polly library and support all built-in resilience strategies.

To add a custom resilience pipeline, use the `AddResilienceHandler` extension for `IHttpClientBuilder`:

```csharp
services
    .AddHttpClient("my-client")
    .AddResilienceHandler("custom-pipeline", builder =>
    {
        builder
            .AddRetry(new HttpRetryStrategyOptions())
            .AddTimeout(new HttpTimeoutStrategyOptions());
    });
```

In the example above:

- A new named *my-client* HTTP Client is registered.
- `AddResilienceHandler` is called to add a resilience handler that uses a resilience pipeline, configured by calling extensions on `builder`.
- Both retry and timeout strategies are added to the pipeline.

### Custom resilience pipeline and dynamic reloads

The custom resilience pipeline supports dynamic reloads, i.e., it transparently refreshes the pipeline whenever the options are changed. To enable dynamic reloads, the example above can be rewritten as:

``` csharp
// 1. Define options that represent the custom pipeline
public class CustomPipelineOptions
{
    [Required]
    public HttpRetryStrategyOptions Retry { get; set; } = new();

    [Required]
    public HttpTimeoutStrategyOptions Timeout { get; set; } = new();
}

// 2. Build a configuration that dynamically reloads
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

// 3. Bind named CustomPipelineOptions from configuration
var services = new ServiceCollection();
services.AddLogging(b => b.AddConsole().SetMinimumLevel(LogLevel.Debug));
services.Configure<CustomPipelineOptions>("custom-pipeline", configuration.GetRequiredSection("CustomPipeline"));

// 4. Define the HTTP pipeline
services
    .AddHttpClient("my-client", client => client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com"))
    .AddResilienceHandler("custom-pipeline", (builder, context) =>
    {
        // Enable dynamic reloads of this pipeline whenever the named CustomPipelineOptions change
        context.EnableReloads<CustomPipelineOptions>("custom-pipeline");

        // Retrieve the named options
        var options = context.GetOptions<CustomPipelineOptions>("custom-pipeline");

        builder
            .AddRetry(options.Retry)
            .AddTimeout(options.Timeout);
    });
```

The `appsettings.json` file looks similar to:

``` json
{
    "CustomPipeline": {
        "Retry": {
            "ShouldRetryAfterHeader": false,
            "MaxRetryAttempts": 3,
            "BackoffType": 2,
            "UseJitter": true,
            "Delay": "00:00:02",
          },
          "Timeout": {
            "Timeout": "00:00:30",
          }
    }
}
```

To test the reloads, run the application and use the defined HTTP pipeline:

``` csharp
HttpClient client = services.BuildServiceProvider()
    .GetRequiredService<IHttpClientFactory>()
    .CreateClient("my-client");

await client.GetStringAsync("posts");
Console.ReadLine();
```

Now, whenever you modify the `appsettings.json`, the pipeline is dynamically reloaded. You should see the following Polly event in the console:

``` bash
info: Polly[0]
      Resilience event occurred. EventName: 'OnReload', Source: 'my-client-custom-pipeline//(null)', Operation Key: '', Result: ''
```

> Dynamic reloads are automatically enabled for both the standard resilience pipeline and the standard hedging pipeline.

Visit the official [dynamic reload](https://learn.microsoft.com/dotnet/core/resilience/http-resilience#dynamic-reload) documentation to learn more.

### HTTP resilience options

The `Microsoft.Extensions.Http.Resilience` library defines HTTP-specific options tailored for HTTP scenarios. These options extend those defined in the Polly library and modify some of the defaults. Specifically, the `ShouldHandle` predicate of reactive strategies has been updated to retry on specific HTTP errors.

Below is a table showing the HTTP-specific options:

| Options | Base Options | Notes |
|--:|--|--|
| `HttpRetryStrategyOptions` | [`RetryStrategyOptions<HttpResponseMessage>`](https://www.pollydocs.org/strategies/retry#defaults) | Some defaults have been changed.  <sup>1</sup> |
| `HttpCircuitBreakerStrategyOptions` | [`CircuitBreakerStrategyOptions<HttpResponseMessage>`](https://www.pollydocs.org/strategies/circuit-breaker#defaults) | Some defaults have been changed. <sup>2</sup> |
| `HttpHedgingStrategyOptions` | [`HedgingStrategyOptions<HttpResponseMessage>`](https://www.pollydocs.org/strategies/hedging#defaults) | Some defaults have been changed. <sup>3</sup>  |
| `HttpRateLimiterStrategyOptions` | [`RateLimiterStrategyOptions`](https://www.pollydocs.org/strategies/rate-limiter#defaults) | Defaults remain unchanged. |
| `HttpTimeoutStrategyOptions` | [`TimeoutStrategyOptions`](https://www.pollydocs.org/strategies/timeout#defaults) | Defaults remain unchanged. |

<sup>1</sup> The `HttpRetryStrategyOptions` uses an exponential backoff type with jitter and handles the *Retry-After* header automatically. The `ShouldHandle` predicate addresses any status code of *500* or above, as well as *429* and *408* status codes. Additionally, both `HttpRequestException` and `TimeoutRejectedException` are retried.

<sup>2</sup> The circuit breaker's `ShouldHandle` predicate addresses any status code of *500* or above, as well as *429* and *408* status codes. Moreover, `HttpRequestException` and `TimeoutRejectedException` are handled.

<sup>3</sup> The `ShouldHandle` predicate for hedging addresses any status code of *500* or above, as well as *429* and *408* status codes. Additionally, `HttpRequestException` and `TimeoutRejectedException` are hedged.

## Performance

The new resilience APIs are built on Polly v8, which was designed from the ground up to support zero-allocations and enhanced performance. To illustrate these improvements, let's compare the performance of the standard resilience pipeline in Polly v7 to that in Polly v8:

```markdown
|                    Method |     Mean |     Error |    StdDev | Ratio |   Gen0 | Allocated | Alloc Ratio |
|-------------------------- |---------:|----------:|----------:|------:|-------:|----------:|------------:|
| StandardPipeline_Polly_V7 | 3.236 us | 0.0130 us | 0.0187 us |  1.00 | 0.1488 |    3816 B |        1.00 |
| StandardPipeline_Polly_V8 | 3.104 us | 0.0237 us | 0.0317 us |  0.96 | 0.0381 |    1008 B |        0.26 |
```

While the execution time is marginally faster, the APIs built on Polly v8 use almost 4x less memory.

## Summary

The .NET 8 introduces new `Microsoft.Extensions.Http.Resilience` and `Microsoft.Extensions.Resilience` packages, built on the Polly library. These new packages allow developers to integrate resilience strategies seamlessly into the HTTP pipeline. Collaboration with the Polly community led to the development of Polly v8, which offers improved performance, built-in telemetry, and a fluent syntax. These advancements simplify the integration of resilience for developers while ensuring efficiency and reduced memory usage.
