---
post_title: Resilience and chaos engineering
author1: martintomka
post_slug: resilience-and-chaos-engineering
microsoft_alias: martintomka
featured_image: chaos-engineering-featured.jpg
categories: .NET, .NET Core, .NET Fundamentals, ASP.NET Core, C#, Networking
tags: .NET, HTTP, resilience, IHttpClientFactory, cloud, scale, Polly, HttpClient, C#, chaos, chaos engineering
summary: Chaos engineering with HTTP clients and Polly library
post_date: 2024-02-09 10:05:00
---

This article introduces the concept of resilience and chaos engineering in .NET applications using the Polly library, highlighting new features that enable chaos engineering. It provides a practical guide on integrating chaos strategies within HTTP clients and showcases how to configure resilience pipelines for improved fault tolerance.

## TL;DR

As of version 8.3.0, the [Polly library](https://www.pollydocs.org) now supports [chaos engineering](https://www.pollydocs.org/chaos). This update allows you to use the following chaos strategies:

- **Fault**: Introduces faults (exceptions) into your system.
- **Outcome**: Injects fake outcomes (results or exceptions) in your system.
- **Latency**: Adds delay to executions before calls are executed.
- **Behavior**: Enables the injection of *any* additional behavior before a call is made.

> Chaos engineering for .NET was initially introduced in the [Simmy library](https://github.com/Polly-Contrib/Simmy). In version 8 of Polly, we collaborated with the creator of Simmy to integrate the Simmy library directly into Polly.

To use the new chaos strategies in a HTTP client, add the following packages to your C# project:

```xml
<ItemGroup>
  <PackageReference Include="Microsoft.Extensions.Http.Resilience" />
  <!-- This is required until Microsoft.Extensions.Http.Resilience updates the version of Polly it depends on. -->
  <PackageReference Include="Polly.Extensions" />
</ItemGroup>
```

You can now use the new chaos strategies when setting up the resilience pipeline:

```csharp
services
    .AddHttpClient("my-client")
    .AddResilienceHandler("my-pipeline", (ResiliencePipelineBuilder<HttpResponseMessage> builder) => 
    {
        // Start with configuring standard resilience strategies
        builder
            .AddConcurrencyLimiter(10, 100)
            .AddRetry(new RetryStrategyOptions<HttpResponseMessage> { /* configuration options */ })
            .AddCircuitBreaker(new CircuitBreakerStrategyOptions<HttpResponseMessage> { /* configuration options */ })
            .AddTimeout(TimeSpan.FromSeconds(5));

        // Next, configure chaos strategies to introduce controlled disruptions.
        // Place these after the standard resilience strategies.

        // Inject chaos into 2% of invocations
        const double InjectionRate = 0.02;

        builder
            .AddChaosLatency(InjectionRate, TimeSpan.FromMinutes(1)) // Introduce a delay as chaos latency
            .AddChaosFault(InjectionRate, () => new InvalidOperationException("Chaos strategy injection!")) // Introduce a fault as chaos
            .AddChaosOutcome(InjectionRate, () => new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError)) // Simulate an outcome as chaos
            .AddChaosBehavior(0.001, cancellationToken => RestartRedisAsync(cancellationToken)); // Introduce a specific behavior as chaos            
    });
```

In the example above:

- The [`IHttpClientFactory`](https://learn.microsoft.com/dotnet/core/extensions/httpclient-factory) pattern is used to register the `my-client` HTTP client.
- The `AddResilienceHandler` extension method is used to set up Polly's resilience pipeline. For more information on resilience with the Polly library, refer to [Building resilient cloud services with .NET 8](https://devblogs.microsoft.com/dotnet/building-resilient-cloud-services-with-dotnet-8/).
- Both resilience and chaos strategies are configured by calling extension methods on top of `builder` instance.

## About chaos engineering

Chaos engineering is a practice that involves testing a system by introducing disturbances or unexpected conditions. The goal is to gain confidence in the system's ability to remain stable and reliable under challenging circumstances in a live production environment.

There are numerous resources available for those interested in exploring chaos engineering further:

- [Chaos engineering on Wikipedia](https://en.wikipedia.org/wiki/Chaos_engineering): This link provides an overview of chaos engineering, including its basic concepts, history, and associated tools.
- [Chaos engineering, the history, principles, and practices](https://www.gremlin.com/community/tutorials/chaos-engineering-the-history-principles-and-practice/): An in-depth article by [Gremlin](https://www.gremlin.com/chaos-engineering/), a platform specializing in chaos engineering, covering the topic's history, key principles, and practical applications.
- [Understanding chaos engineering and resilience](https://learn.microsoft.com/azure/chaos-studio/chaos-studio-chaos-engineering-overview): A beginner's guide to chaos engineering within the framework of [Azure Chaos Studio](https://learn.microsoft.com/azure/chaos-studio/chaos-studio-overview), a service designed to evaluate and enhance the resilience of cloud applications and services through chaos engineering.

However, this blog post will not delve into the intricacies of chaos engineering. Instead, it highlights how to use the Polly library to inject chaos into our systems practically. We will focus on *in-process* chaos injection, meaning we introduce chaos directly into your process. We won't cover other external methods, such as restarting virtual machines, simulating high CPU usage, or creating low-memory conditions, in this article.

## Scenario

We're building a simple web service that provides a list of TODOs. This service gets the TODOs by talking to the `https://jsonplaceholder.typicode.com/todos` endpoint using an HTTP client. To test how well our service can handle problems, we will introduce chaos into the HTTP communication. Then, we'll use resilience strategies to mitigate these issues, making sure that the service remains reliable for users.

You'll learn how to use the Polly API to control the amount of chaos injected. This technique allows us to apply chaos selectively, such as only in certain environments (like Development or Production) or to specific users. This way, we can ensure stability while still testing our service's robustness.

## Concepts

The Polly library's chaos strategies have several key properties in common:

| Property                 | Default Value | Description |
|--------------------------|---------------|-------------|
| `InjectionRate`          | 0.001         | This is a decimal value between 0 and 1. It represents the chance that chaos will be introduced. For example, a rate of 0.2 means there's a 20% chance for chaos on each call; 0.01 means a 1% chance; and 1 means chaos will occur on every call. |
| `InjectionRateGenerator` | `null`        | This function decides the rate of chaos for each specific instance, with values ranging from 0 to 1. |
| `Enabled`                | `true`        | This indicates whether chaos injection is currently active. |
| `EnabledGenerator`       | `null`        | This function determines if the chaos strategy should be activated for each specific instance. |

By adjusting the `InjectionRate`, you can control the amount of chaos injected into the system. The `EnabledGenerator` lets you dynamically enable or disable the chaos injection. This means you can turn the chaos on or off under specific conditions, offering flexibility in testing and resilience planning.

## Practical example

In the next sections, we'll walk through the development of a simple web service. This process includes introducing chaos into the system, then exploring how you can dynamically manage the level of chaos introduced. Lastly, we'll demonstrate how to use resilience strategies to mitigate the chaos.

### Creating the Project

To start, we'll create a new project using the console. Follow these steps:

**Create a new project**: Open a new console window and run the following command that creates a new web project named *Chaos*:

```bash
dotnet new web -o Chaos
```

This command creates a new directory called `Chaos` with a basic web project setup.

**Modify `Program.cs` File**: Next, open the `Program.cs` file in your newly created project and replace its contents with the following code:

```csharp
var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

var httpClientBuilder = builder.Services.AddHttpClient<TodosClient>(client => client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com"));

var app = builder.Build();
app.MapGet("/", (TodosClient client, CancellationToken cancellationToken) => client.GetTodosAsync(cancellationToken));
app.Run();
```

The `TodosClient` and `Todo` is defined as:

```csharp
public class TodosClient(HttpClient client)
{
    public async Task<IEnumerable<TodoModel>> GetTodosAsync(CancellationToken cancellationToken)
    {
        return await client.GetFromJsonAsync<IEnumerable<TodoModel>>("/todos", cancellationToken) ?? [];
    }
}

public record TodoModel(
    [property: JsonPropertyName("id")] int Id, 
    [property: JsonPropertyName("title")] string Title);
```

The code above does the following:

- Utilizes the `IHttpClientFactory` to configure a typed `TodosClient` that targets a specified endpoint.
- Injects the `TodosClient` into the request handler to fetch a list of todos from the remote endpoint.

**Run the application**: After setting up your project and the `TodosClient`, run the application. You should be able to retrieve and display a list of todos by accessing the root endpoint.

### Injecting chaos

In this section, we'll introduce chaos to our HTTP client to observe its impact on our web service.

**Adding resilience libraries**: First, update your project file to include necessary dependencies for resilience and chaos handling:

```xml
<ItemGroup>
    <PackageReference Include="Microsoft.Extensions.Http.Resilience" Version="8.0.0" />
    <PackageReference Include="Polly.Core" Version="8.3.0" />
</ItemGroup>
```

> **Note**: We're including `Polly.Core` directly, even though `Microsoft.Extensions.Http.Resilience` already references it. This ensures we use the latest version of Polly that includes chaos strategies. Once `Microsoft.Extensions.Http.Resilience` is updated to incorporate the latest `Polly.Core`, this direct reference will no longer be necessary.

**Injecting chaos into the HTTP Client**: Next, enhance the HTTP client setup in your code to use the `AddResilienceHandler` for integrating chaos strategies:

```csharp
var httpClientBuilder = builder.Services.AddHttpClient<TodosClient>(client => client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com"));

// New code below
httpClientBuilder.AddResilienceHandler("chaos", (ResiliencePipelineBuilder<HttpResponseMessage> builder) => 
{
    // Set the chaos injection rate to 5%
    const double InjectionRate = 0.05;

    builder
        .AddChaosLatency(InjectionRate, TimeSpan.FromSeconds(5)) // Add latency to simulate network delays
        .AddChaosFault(InjectionRate, () => new InvalidOperationException("Chaos strategy injection!")) // Inject faults to simulate system errors
        .AddChaosOutcome(InjectionRate, () => new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError)); // Simulate server errors
});
```

This change accomplishes the following:

- Applies the `AddResilienceHandler` extension method to introduce a `chaos` resilience pipeline to our HTTP client.
- Utilizes various Polly extension methods within the callback to integrate different types of chaos (latency, faults, and error outcomes) into our HTTP calls.

**Running the application**: With the chaos strategies in place, running the application and attempting to retrieve TODOs will now result in random issues. These issues, while artificially induced at a 5% rate, mimic real-world scenarios where dependencies may be unstable, leading to occasional disruptions.

### Dynamically injecting chaos

In the previous section, we introduced chaos into our HTTP client, but with limited control over the chaos injection's timing and intensity. The Polly library, however, offers powerful APIs that allow for precise control over when and how chaos is introduced. These capabilities enable several scenarios:

- **Environment-specific chaos**: Inject chaos only in certain environments, such as Testing or Production, to assess resilience without affecting all users.
- **User or tenant chaos**: Introduce chaos specifically for certain users or tenants, which can be useful for testing resilience in multi-tenant applications.
- **Dynamic chaos intensity**: Adjust the amount of chaos injected based on the environment or specific users, allowing for more refined testing.
- **Selective request chaos**: Choose specific requests or APIs for chaos injection, enabling focused testing on particular system parts.

To implement these scenarios, we can use the `EnabledGenerator` and `InjectionRateGenerator` properties available across all Polly chaos strategies. Let's explore how to apply these in practice.

**Create `IChaosManager` abstraction**: First, we'll create an `IChaosManager` interface to encapsulate our chaos injection logic. This interface might look like this:

```csharp
public interface IChaosManager
{
    ValueTask<bool> IsChaosEnabledAsync(ResilienceContext context);

    ValueTask<double> GetInjectionRateAsync(ResilienceContext context);
}
```

This interface allows us to dynamically determine if chaos should be enabled and at what rate, with the flexibility to make these decisions asynchronously, such as by fetching configuration settings from a remote source.

**Incorporating the `IChaosManager` interface**: We're going to update our approach for defining chaos strategies by utilizing the `IChaosManager`. This will allow us to make dynamic decisions about when to inject chaos.

```csharp
// Updated code below
httpClientBuilder.AddResilienceHandler("chaos", (builder, context) => 
{
    // Get IChaosManager from dependency injection
    var chaosManager = context.ServiceProvider.GetRequiredService<IChaosManager>();

    builder
        .AddChaosLatency(new ChaosLatencyStrategyOptions
        {
            EnabledGenerator = args => chaosManager.IsChaosEnabledAsync(args.Context),
            InjectionRateGenerator = args => chaosManager.GetInjectionRateAsync(args.Context),
            Latency = TimeSpan.FromSeconds(5)
        })
        .AddChaosFault(new ChaosFaultStrategyOptions
        {
            EnabledGenerator = args => chaosManager.IsChaosEnabledAsync(args.Context),
            InjectionRateGenerator = args => chaosManager.GetInjectionRateAsync(args.Context),
            FaultGenerator = new FaultGenerator().AddException(() => new InvalidOperationException("Chaos strategy injection!"))
        })
        .AddChaosOutcome(new ChaosOutcomeStrategyOptions<HttpResponseMessage>
        {
            EnabledGenerator = args => chaosManager.IsChaosEnabledAsync(args.Context),
            InjectionRateGenerator = args => chaosManager.GetInjectionRateAsync(args.Context),
            OutcomeGenerator = new OutcomeGenerator<HttpResponseMessage>().AddResult(() => new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError))
        })           
});
```

Let's break down the changes:

- We're using a different `AddResilienceHandler` method that takes a `context`. This `context` gives us access to `IServiceProvider`, allowing us to retrieve additional services needed for our chaos setup.
- `IChaosManager` is obtained and utilized for setting up chaos strategies.
- Rather than using simple chaos methods, we're opting for options-based extensions. This grants complete control over the chaos functionalities, including the ability to specify conditions for when chaos should be enabled and how frequently. This includes access to both `EnabledGenerator` and `InjectionRateGenerator` properties.
- The introduction of `FaultGenerator` and `OutcomeGenerator<HttpResponseMessage>` enables us to define specific faults (errors) and outcomes that chaos strategies will inject. These APIs also allow the creation of a variety of faults and the assignment of different probabilities to each, influencing how likely each fault is to occur. For further details, refer to [PollyDocs: generating outcomes](https://www.pollydocs.org/chaos/outcome#generating-outcomes) and [PollyDocs: generating faults](https://www.pollydocs.org/chaos/fault#generating-faults).

### Implementing the `IChaosManager`

Before we dive into the implementation of the chaos manager, let's outline how it behaves during chaos injection:

- In testing environments, chaos is always active, with the injection rate of *5%*.
- In production environments, chaos is enabled exclusively for test users, with an injection rate of *3%*.

> **Note:** For this example, we'll simplify user identification. We'll identify users based on the presence of the `user=<user-name>` query string, without delving into the complexities of user identity.

**`ChaosManager` implementation**: To meet the specified requirements, here's how we implement the `ChaosManager`:

```csharp
internal class ChaosManager(IWebHostEnvironment environment, IHttpContextAccessor contextAccessor) : IChaosManager
{
    private const string UserQueryParam = "user";

    private const string TestUser = "test";

    public ValueTask<bool> IsChaosEnabledAsync(ResilienceContext context)
    {
        if (environment.IsDevelopment())
        {
            return ValueTask.FromResult(true);
        }

        // This condition is demonstrative and not recommended to use in real apps.
        if (environment.IsProduction() &&
            contextAccessor.HttpContext is {} httpContext && 
            httpContext.Request.Query.TryGetValue(UserQueryParam, out var values) &&
            values == TestUser)
        {
            // Enable chaos for 'test' user even in production 
            return ValueTask.FromResult(true);
        }

        return ValueTask.FromResult(false);
    }

    public ValueTask<double> GetInjectionRateAsync(ResilienceContext context)
    {
        if (environment.IsDevelopment())
        {
            return ValueTask.FromResult(0.05);
        }

        if (environment.IsProduction())
        {
            return ValueTask.FromResult(0.03);
        }
        
        return ValueTask.FromResult(0.0);
    }
}
```

**Integrating `IChaosManager` with `IServiceCollection`**: Update **Program.cs** to include `IChaosManager` in the Dependency Injection (DI) container.

```csharp
var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.TryAddSingleton<IChaosManager, ChaosManager>(); // <-- Add this line
services.AddHttpContextAccessor(); // <-- Add this line
```

**Run the application**: Start the application to see how it behaves. Experiment by [changing the environment setting](https://learn.microsoft.com/aspnet/core/fundamentals/environments) to observe the differences in how the application responds to chaos in development versus production settings.

### Use resilience strategies to fix the chaos

In this section we will use resilience strategies to mitigate the chaos. First, let's recount what type of chaos is injected:

- Latency of 5 seconds.
- Injection of `InvalidOperationException` exception.
- Injection of `HttpResponseMessage` with `HttpStatusCode.InternalServerError`.

We can use the following resilience strategies to mitigate the chaos.

- [Timeout Strategy](https://www.pollydocs.org/strategies/timeout): Cancels overly long requests.
- [Retry Strategy](https://www.pollydocs.org/strategies/retry): Retries exceptions and invalid responses.
- [Circuit Breaker Strategy](https://www.pollydocs.org/strategies/circuit-breaker): If the failure rate exceeds a certain threshold, it's better to stop all communications temporarily. This gives the dependency a chance to recover while conserving resources of our service.

> For more information on various resilience strategies offered by the Polly library, please visit [PollyDocs: Resilience strategies](https://www.pollydocs.org/strategies).

To add the resilience of the HTTP client, you have two options:

- [**Custom Resilience Handler**](https://learn.microsoft.com/dotnet/core/resilience/http-resilience?tabs=dotnet-cli#add-custom-resilience-handlers): This option allows you complete control over the resilience strategies added to the client.
- [**Standard Resilience Handler**](https://learn.microsoft.com/dotnet/core/resilience/http-resilience?tabs=dotnet-cli#add-standard-resilience-handler): A pre-defined standard resilience handler designed to meet the needs of most situations encountered in the real applications and used across many production applications.

For our purposes, the chaos caused by the chaos strategies mentioned earlier can be effectively managed using the standard resilience handler. Generally, it's advisable to use the standard handler unless you find it doesn't meet your specific needs. Here's how you configure the HTTP client with standard resilience:

```csharp
var httpClientBuilder = builder.Services.AddHttpClient<TodosClient>(client => client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com"));

// Add and configure the standard resilience above the chaos handler
httpClientBuilder
    .AddStandardResilienceHandler()
    .Configure(options => 
    {
        // Update attempt timeout to 1 second
        options.AttemptTimeout.Timeout = TimeSpan.FromSeconds(1);
        
        // Update circuit breaker to handle transient errors and InvalidOperationException
        options.CircuitBreaker.ShouldHandle = args => args.Outcome switch
        {
            {} outcome when HttpClientResiliencePredicates.IsTransient(outcome) => PredicateResult.True(),
            { Exception: InvalidOperationException } => PredicateResult.True(),
            _ => PredicateResult.False()
        };

        // Update retry strategy to handle transient errors and InvalidOperationException
        options.Retry.ShouldHandle = args => args.Outcome switch
        {
            {} outcome when HttpClientResiliencePredicates.IsTransient(outcome) => PredicateResult.True(),
            { Exception: InvalidOperationException } => PredicateResult.True(),
            _ => PredicateResult.False()
        };
    });

httpClientBuilder.AddResilienceHandler("chaos", (builder, context) => { /* Chaos configuration omitted for clarity */ });
```

The previous example:

- Adds a standard resilience handler using `AddStandardResilienceHandler`. It's crucial to place the standard handler before the chaos handler to effectively handle any introduced chaos.
- Uses the `Configure` extension method to modify the resilience strategy configuration.
- Sets the attempt timeout to 1 second. Timeout duration may differ for each dependency. In our cases, endpoint calls are quick. If they exceed 1 second, it suggests an issue, and it's advisable to cancel and retry.
- Updates the `ShouldHandle` predicate for both retry and circuit breaker strategies. Here, we employ switch expressions for error handling. The `HttpClientResiliencePredicates.IsTransient` function is utilized to retry on typical transient errors, like HTTP status codes of 500 and above or `HttpRequestException`. We also need to handle `InvalidOperationException`, as it's not covered by the `HttpClientResiliencePredicates.IsTransient` function.

**Running the application**: With resilience integrated into the HTTP pipeline, try to run the application and observe its behavior. Errors should no longer appear, thanks to the chaos being effectively countered by the standard resilience handler.

## Telemetry

Polly offers [comprehensive telemetry](https://www.pollydocs.org/advanced/telemetry) by default, providing robust tools to track the use of resilience strategies. This telemetry includes both logs and metrics.

### Logs

Start the application and pay attention to the log events generated by Polly. Look for "chaos events" such as `Chaos.OnLatency` or `Chaos.OnOutcome` marked with an `Information` severity level:

```bash
info: Polly[0]
      Resilience event occurred. EventName: 'Chaos.OnOutcome', Source: 'TodosClient-chaos//Chaos.Outcome', Operation Key: '', Result: ''
info: Polly[0]
      Resilience event occurred. EventName: 'Chaos.OnLatency', Source: 'TodosClient-chaos//Chaos.Latency', Operation Key: '', Result: ''      
```

Resilience events like `OnRetry` or `OnTimeout` triggered by resilience strategies are categorized with higher severity levels, such as `Warning` or `Error`. These indicate unusual activity in the system:

```bash
fail: Polly[0]
      Resilience event occurred. EventName: 'OnTimeout', Source: 'TodosClient-standard//Standard-AttemptTimeout', Operation Key: '', Result: ''
warn: Polly[0]
      Resilience event occurred. EventName: 'OnRetry', Source: 'TodosClient-standard//Standard-Retry', Operation Key: '', Result: '500'      
```

### Metrics

Polly provides the following instruments that are emitted under **Polly** metric name. These help you monitor your application's health:

- `resilience.polly.strategy.events`: Triggered when a resilience event occurs.
- `resilience.polly.strategy.attempt.duration`: Measures how long execution attempts take, relevant for `Retry` and `Hedging` strategies.
- `resilience.polly.pipeline.duration`: Tracks the total time taken by resilience pipelines.

For more information on Polly's metrics, visit [Polly Docs: Metrics](https://www.pollydocs.org/advanced/telemetry.html#metrics).

To see these metrics in action, let's use the [`dotnet counters`](https://learn.microsoft.com/dotnet/core/diagnostics/metrics-collection#view-metrics-with-dotnet-counters) tool:

1. Start the application using the `dotnet run` command.
1. Open new terminal window and execute the following command to begin tracking the `Polly` metrics for our `Chaos` app:

```bash
dotnet counters monitor -n Chaos Polly
```

Finally, access the application root API several times until resilience events start appearing in the console. In the terminal where `dotnet counters` is active you should see the following output:

```bash
[Polly]
    resilience.polly.pipeline.duration (ms)
        error.type=200,event.name=PipelineExecuted,event.severit         130.25
        error.type=200,event.name=PipelineExecuted,event.severit         130.25
        error.type=200,event.name=PipelineExecuted,event.severit         130.25
        error.type=200,event.name=PipelineExecuted,event.severit         133.75
        error.type=200,event.name=PipelineExecuted,event.severit         133.75
        error.type=200,event.name=PipelineExecuted,event.severit         133.75
        error.type=500,event.name=PipelineExecuted,event.severit           2.363
        error.type=500,event.name=PipelineExecuted,event.severit           2.363
        error.type=500,event.name=PipelineExecuted,event.severit           2.363
        event.name=PipelineExecuted,event.severity=Information,e         752
        event.name=PipelineExecuted,event.severity=Information,e         752
        event.name=PipelineExecuted,event.severity=Information,e         752
    resilience.polly.strategy.attempt.duration (ms)
        attempt.handled=False,attempt.number=0,error.type=200,ev         130.5
        attempt.handled=False,attempt.number=0,error.type=200,ev         130.5
        attempt.handled=False,attempt.number=0,error.type=200,ev         130.5
        attempt.handled=False,attempt.number=1,error.type=200,ev          98.125
        attempt.handled=False,attempt.number=1,error.type=200,ev          98.125
        attempt.handled=False,attempt.number=1,error.type=200,ev          98.125
        attempt.handled=True,attempt.number=0,error.type=500,eve           2.422
        attempt.handled=True,attempt.number=0,error.type=500,eve           2.422
        attempt.handled=True,attempt.number=0,error.type=500,eve           2.422
    resilience.polly.strategy.events (Count / 1 sec)
        error.type=500,event.name=OnRetry,event.severity=Warning           0
        event.name=Chaos.OnOutcome,event.severity=Information,pi           0
        event.name=dummy,event.severity=Error,pipeline.instance=           0
        event.name=PipelineExecuting,event.severity=Debug,pipeli           0
        event.name=PipelineExecuting,event.severity=Debug,pipeli           0
```

In practical applications, it's beneficial to use Polly metrics for creating dashboards and monitoring tools that monitor your application's resilience. The article on [.NET observability with OpenTelemetry](https://learn.microsoft.com/dotnet/core/diagnostics/observability-with-otel) provides a solid starting point for this process.

## Summary

This article explores the chaos engineering features available in Polly version `8.3.0` and later. It walks you through developing a web application that communicates with a remote dependency. By utilizing Polly's chaos engineering capabilities, we can introduce controlled chaos into HTTP client communications and then implement resilience strategies to counteract the chaos. Applying this approach to production applications enables you to proactively address issues affecting your application's resilience. Instead of waiting for unforeseen problems, use chaos engineering to simulate and prepare for them.

You can check out the full example in the [`Polly/Samples/Chaos`](https://github.com/App-vNext/Polly/tree/main/samples/Chaos) folder on GitHub.
