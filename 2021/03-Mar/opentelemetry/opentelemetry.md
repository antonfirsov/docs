---
post_title: "OpenTelemetry .NET reaches v1.0"
username: shirhatti
microsoft_alias: soshir
categories: .NET
summary: "Learn how to get started with OpenTelemetry .NET"
desired_publication_date: '2021-03-18'
---

As modern application environments are polyglot, distributed, and increasingly complex, observing your application to identify and react to failures has become challenging. In early 2019, two popular instrumentation projects, OpenTracing and OpenCensus, merged to create [OpenTelemetry](https://opentelemetry.io), a new standard for observability telemetry.

## What is OpenTelemetry?

By standardizing how different applications and frameworks collect and emit observability telemetry, OpenTelemetry aims to solve some of the challenges posed by these environments.
The OpenTelemetry project is comprised of:
- A vendor-neutral specification for observability telemetry (distributed tracing, metrics, etc.)
- API packages that implement the public interfaces used for instrumentation
- SDKs used by applications to configure instrumentation and interfaces for plugin authors to write exporters
- Exporters that enable you to send your data to the telemetry backend of your choice

This rich ecosystem of components has led to OpenTelemetry becoming ubiquitous in the observability telemetry space. Under the stewardship of the [Cloud Native Computing Foundation](https://www.cncf.io/), it already has a vibrant ecosystem of contributors and maintainers and is contributed to and used by many large companies. If you aren't interested in OpenTelemetry yet, you soon will be because of the following benefits that it provides:

### Interoperable

Since different languages implement the same shared specification including the wire protocol for propagating observability telemetry, you can monitor your distributed application with complete interoperability. For example, you can have a frontend service in .NET call into a backend service in Node.js, and track your distributed operation across both these services.

### Vendor neutral

As the collection of observability telemetry becomes standardized by OpenTelemetry, you can choose your telemetry backend without having to change your instrumentation code. You're free to pick the platform that offers you the most effective analysis of your data.

Multiple major instrumentation and Application Performance Monitoring (APM) vendors have publicly expressed that OpenTelemetry is a key part of their monitoring solution. It is exciting to have multiple vendors implementations already available that are based on OpenTelemetry.

- [New Relic’s OpenTelemetry .NET Offering Reaches V1.0](https://blog.newrelic.com/product-news/new-relic-opentelemetry-net/)
- [Pre-release Azure Monitor OpenTelemetry Exporter](https://www.nuget.org/packages/Azure.Monitor.OpenTelemetry.Exporter/1.0.0-beta.2)

In addition to the SaaS offerings for observability telemetry, OpenTelemetry also works well with existing open source distributed tracing tools such as [Jaeger](https://www.jaegertracing.io/) and [Zipkin](https://zipkin.io/).

### Future proof

OpenTelemetry aims to build instrumentation into libraries and frameworks moving forward. When newer libraries and frameworks emerge, you can easily monitor them using shared instrumentation libraries.

## What’s new in OpenTelemetry?

In February 2021, the [OpenTelemetry specification reached v1.0](https://medium.com/opentelemetry/opentelemetry-specification-v1-0-0-tracing-edition-72dd08936978). With the v1.0 specification, OpenTelemetry implementations are now offering stability guarantees for distributed tracing. Shortly after the stabilization of the specification, OpenTelemetry .NET, the canonical distribution of the OpenTelemetry SDK implementation in .NET, also [announced their v1.0](https://medium.com/opentelemetry/opentelemetry-net-reaches-v1-0-e7c5e975fd44) release which includes the following:

- The v1.0 release of [OpenTelemetry .NET APIs](https://www.nuget.org/packages/OpenTelemetry.Api/): Tracing API, Baggage API, Context API and Propagators API.
- [An SDK](https://www.nuget.org/packages/OpenTelemetry/) providing controls for sampling, processing, and export.
- Exporters to [Jaeger](https://www.nuget.org/packages/OpenTelemetry.Exporter.Jaeger/), [Zipkin](https://www.nuget.org/packages/OpenTelemetry.Exporter.Zipkin/) and the [OpenTelemetry Protocol (OTLP)](https://www.nuget.org/packages/OpenTelemetry.Exporter.OpenTelemetryProtocol/)
- Documentation, which includes [samples](https://github.com/open-telemetry/opentelemetry-dotnet/tree/main/examples), [getting started guides](https://github.com/open-telemetry/opentelemetry-dotnet/tree/main/docs/trace/getting-started) and guides for [ plugin authors](https://github.com/open-telemetry/opentelemetry-specification/blob/main/specification/glossary.md#plugin-author) to [ extend the SDK](https://github.com/open-telemetry/opentelemetry-dotnet/tree/main/docs/trace/extending-the-sdk)

## Getting started with OpenTelemetry

OpenTelemetry helps solving concerns ranging from emitting observability data, processing of the data, and controlling how the data is exported. Despite the cross-cutting nature of these concerns, care has been taken to separate them.

### Application Developers
If you are an application developer, you can configure your application to use the OpenTelemetry .NET SDK and its instrumentation packages to collect distributed tracing.

Here's an example that shows you how to add instrumentation to a console application using the [OpenTelemetry](https://www.nuget.org/packages/OpenTelemetry/) and [OpenTelemetry.Exporter.Console](https://www.nuget.org/packages/OpenTelemetry.Exporter.Console/) packages.

```csharp
public class Program
{
    public static void Main()
    {
        using var tracerProvider = Sdk.CreateTracerProviderBuilder()
            .SetSampler(new AlwaysOnSampler())
            // Add more libraries
            .AddSource("Sample")
            // Add more exporters
            .AddConsoleExporter()
            .Build();

        // ...
        // Rest of application
        // ...

    }
}
```

The OpenTelemetry SDK also integrates with the hosted application model used by ASP.NET Core (and the worker service). Here's an example that shows you how to instrument an ASP.NET Core application using the [OpenTelemetry](https://www.nuget.org/packages/OpenTelemetry/), [OpenTelemetry.Extensions.Hosting](https://www.nuget.org/packages/OpenTelemetry.Extensions.Hosting/1.0.0-rc2) and [OpenTelemetry.Exporter.Zipkin](https://www.nuget.org/packages/OpenTelemetry.Exporter.Zipkin/) packages.

```csharp
public class Startup
{
    public Startup(IConfiguration configuration) => Configuration = configuration;

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddOpenTelemetryTracing((builder) => builder
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddZipkinExporter(zipkinOptions =>
            {
                zipkinOptions.Endpoint = new Uri(Configuration.GetConnectionString("zipkin"));
            }));
    }
}
```

### Library Authors
> If you are not a library author, you can skip this section. Popular libraries in the .NET ecosystem (ASP.NET Core, grpc-dotnet, SQLClient) are already instrumented and ready for to you consume observability telemetry.

The [System.Diagnostics APIs](https://docs.microsoft.com/dotnet/api/system.diagnostics) in .NET contain an implementation of the OpenTelemetry API specification. In addition to these APIs shipping as part of .NET 5, they are also available on NuGet as part of the [System.Diagnostics.DiagnosticsSource](https://www.nuget.org/packages/System.Diagnostics.DiagnosticSource/5.0.1) package and supported on all supported version of .NET including .NET Framework 4.5+ and .NET Core 2.1+. This is the only package reference you require to add distributed tracing support to your library.

Here's an example that shows how you might instrument your library:

```csharp
class Program
{
    static ActivitySource s_source = new ActivitySource("Sample");

    static async Task Main(string[] args)
    {
        await DoSomeWork();
        Console.WriteLine("Example work done");
    }

    static async Task DoSomeWork()
    {
        using (Activity? activity = s_source.StartActivity("SomeWork"))
        {
            await StepOne();
            await StepTwo();
        }
    }

    static async Task StepOne()
    {
        using (Activity? activity = s_source.StartActivity("StepOne"))
        {
            await Task.Delay(500);
        }
    }

    static async Task StepTwo()
    {
        using (Activity? activity = s_source.StartActivity("StepTwo"))
        {
            await Task.Delay(1000);
        }
    }
}
```

### Analyzing tracing data

Once you've exported your distributed tracing data, you can analyze it to view your recorded traces and the causal links between all the spans that comprise these traces. I've included a screenshot below of the simple application we just saw in the Zipkin UI where we can see the spans **_stepone_** and **_steptwo_** parented under the **_somework_** span.

![Screenshot of distributed trace in Zipkin](https://user-images.githubusercontent.com/4734691/111587032-e571c680-877e-11eb-8213-9c9e87a313d8.png)
TODO:


## Feedback

We're super excited to continue to improving the observability of all applications built on .NET and OpenTelemetry is a giant stride for us in that direction. We would love to hear your feedback! Let us know in comments below or join the rapidly growing OpenTelemetry .NET community.

You can join the contributors on the [opentelemetry-dotnet](https://github.com/open-telemetry/opentelemetry-dotnet) repo, join the [OpenTelemetry .NET channel on the CNCF Slack instance](https://app.slack.com/client/T08PSQ7BQ/C01N3BC2W7Q) (create a CNCF Slack account [here](http://slack.cncf.io/)), or participate in the [weekly community meetings](https://github.com/open-telemetry/opentelemetry-dotnet#contributing).
