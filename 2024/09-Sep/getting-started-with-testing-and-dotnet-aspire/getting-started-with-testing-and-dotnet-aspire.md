---
post_title: Getting started with testing and .NET Aspire
author1: aapowell
post_slug: getting-started-with-testing-and-dotnet-aspire
microsoft_alias: aapowell
featured_image: getting-started-with-testing-and-dotnet-aspire-cover.jpg
categories: .NET, .NET Aspire
tags: .NET, .NET Aspire, Cloud Native, Testing
ai_note: hide
summary: Learn how to improve your software development process with automated testing in .NET Aspire. This post covers the basics of getting started, writing tests for distributed applications, and ensuring your services run smoothly.
post_date: 2024-09-25 10:05:00
---

Automated testing is an important part of software development, helping ensure that bugs are caught early and regression issues are prevented. In this blog post, we will explore how to get started with testing in .NET Aspire, allowing us to test scenarios across our distributed applications.

## Testing Distributed Applications

Distributed applications are inherently complex, you need to ensure components such as databases, caches, etc. are available and in the correct state. Then your application may have multiple services that need to be tested together. .NET Aspire is a great tool to help us define the environment for our application, connecting together all the services and resources, making it easy to launch our environment.

And this is equally true when it comes to end-to-end, or integration, testing of an application. We need to ensure that the database is in an expected state for a test, avoid having other tests interfere with our test, and ensure that the application is running in the correct configuration. This is something that .NET Aspire can help us with.

Thankfully, we have the .NET Aspire [`Aspire.Hosting.Testing` NuGet package](https://www.nuget.org/packages/aspire.hosting.testing) which can help us with this. Let's take a look at how we can use this package to write tests.

## Getting Started

To get started, we're going to create a new .NET Aspire Starter App project. This will create a new .NET Aspire application with the AppHost, Service Defaults, an API backend and a Blazor web frontend.

Ensure you have the .NET Aspire workload installed:

```bash
dotnet workload update
dotnet workload install aspire
```

Then create a new project using the `aspire-starter` template:

```bash
dotnet new aspire-starter --name AspireWithTesting
```

Next, we need to add a test project, and we can choose from one of three testing frameworks, MSTest, xUnit or Nunit. For this example, we'll use MSTest. This can be created using the `aspire-mstest` template:

```bash
dotnet new aspire-mstest --name AspireWithTesting.Tests
dotnet sln add AspireWithTesting.Tests
```

> Note: You can have the test project included when creating with the `aspire-starter` template by adding the `--test-framework MSTest` (or other framework) flag.

The template already references the `Aspire.Hosting.Testing` NuGet package, as well as the chosen testing framework (MSTest in this case), so the last thing we need to do is add a reference to the **AppHost** project in our Test project:

```bash
dotnet add AspireWithTesting.Tests reference AspireWithTesting.AppHost
```

## Writing Tests

We'll find there is already a stub test file, `IntegrationTest1.cs` in our test project that describes the steps above and provides an example of tests that can be written, but let's start from scratch so we can understand what's going on. Create a new file called `FrontEndTests.cs` and add the following code:

```csharp
namespace AspireWithTesting.Tests;

[TestClass]
public class FrontEndTests
{
    [TestMethod]
    public async Task CanGetIndexPage()
    {
        var appHost =
            await DistributedApplicationTestingBuilder
                    .CreateAsync<Projects.AspireWithTesting_AppHost>();
        appHost.Services.ConfigureHttpClientDefaults(clientBuilder =>
        {
            clientBuilder.AddStandardResilienceHandler();
        });

        await using var app = await appHost.BuildAsync();
        await app.StartAsync();

        var resourceNotificationService =
            app.Services.GetRequiredService<ResourceNotificationService>();
        await resourceNotificationService
            .WaitForResourceAsync("webfrontend", KnownResourceStates.Running)
            .WaitAsync(TimeSpan.FromSeconds(30));

        var httpClient = app.CreateHttpClient("webfrontend");
        var response = await httpClient.GetAsync("/");

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }
}
```

Awesome, the test is written, let's run it:

```bash
dotnet test
```

And if everything goes to plan, we should see output such as:

```plaintext
Test summary: total: 1, failed: 0, succeeded: 1, skipped: 0, duration: 0.9s
```

## Understanding the Test

Let's break down what's happening in the test:

```csharp
var appHost =
    await DistributedApplicationTestingBuilder
            .CreateAsync<Projects.AspireWithTesting_AppHost>();
appHost.Services.ConfigureHttpClientDefaults(clientBuilder =>
{
    clientBuilder.AddStandardResilienceHandler();
});

await using var app = await appHost.BuildAsync();
await app.StartAsync();
```

This first section of the test is using the **AppHost** project that defines all our resources, services, and their relationships, and then starts it up, as if we did a `dotnet run` against the project, but in a test environment which we can control some additional aspects. For example, we're injecting the `StandardResilienceHandler` into the `HttpClient` that the tests are going to use to interact with the services in the AppHost. Once the testing AppHost is configured, we can build the application, ready to be started.

```csharp
var resourceNotificationService =
    app.Services.GetRequiredService<ResourceNotificationService>();
await resourceNotificationService
    .WaitForResourceAsync("webfrontend", KnownResourceStates.Running)
    .WaitAsync(TimeSpan.FromSeconds(30));
```

Because the AppHost will be starting several different resources and services, we need to ensure that they are available to us before we try to run tests against them. After all, if the web application hasn't started and we try to resolve it, we're going to get an error. The `ResourceNotificationService` is a service that allows us to wait for a resource to be in a particular state, in this case, we're waiting for the `webfrontend` (the name we set in the AppHost) to be in the `Running` state, and we're giving it 30 seconds to do so. This pattern would need to be repeated for any other services that we're going to interact with, whether directly or indirectly.

```csharp
var httpClient = app.CreateHttpClient("webfrontend");
var response = await httpClient.GetAsync("/");

Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
```

Finally, we can request an instance of `HttpClient` from the app that has been started and provide the name of the service we want to interact with. This uses the same service discovery as the rest of the application, so we don't need to worry about the URL or port that the service is running on. We can then make a request to the service, in this case, the root of the web frontend, and check that we get a `200 OK` response, confirming that the service is running and responding as expected.

## Testing the API

Testing the API service is a very similar approach to the frontend service, but since it's returning data, we can take it a step further and assert against the data that is returned:

```csharp
using System.Net.Http.Json;

namespace AspireWithTesting.Tests;

[TestClass]
public class ApiTests
{
    [TestMethod]
    public async Task CanGetWeatherForecast()
    {
        var appHost =
            await DistributedApplicationTestingBuilder.CreateAsync<Projects.AspireWithTesting_AppHost>();
        appHost.Services.ConfigureHttpClientDefaults(clientBuilder =>
        {
            clientBuilder.AddStandardResilienceHandler();
        });

        await using var app = await appHost.BuildAsync();
        await app.StartAsync();

        var resourceNotificationService =
            app.Services.GetRequiredService<ResourceNotificationService>();
        await resourceNotificationService
                .WaitForResourceAsync("apiservice", KnownResourceStates.Running)
                .WaitAsync(TimeSpan.FromSeconds(30));

        var httpClient = app.CreateHttpClient("apiservice");
        var response = await httpClient.GetAsync("/weatherforecast");

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var forecasts = await response.Content.ReadFromJsonAsync<IEnumerable<WeatherForecast>>();
        Assert.IsNotNull(forecasts);
        Assert.AreEqual(5, forecasts.Count());
    }

    record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary);
}
```

[alert type="note" heading="Note"]Since the `WeatherForecast` `record` is private in the API project, we need to define it in the test project to be able to deserialize the JSON response.[/alert]

Once we've asserts that the API endpoint returned a `200 OK` response, we can then deserialize the JSON response into a collection of `WeatherForecast` objects and assert against the data. In this case, the data we have for our API is randomly generated so we're only asserting on the number of records returned, but if we had a database that the data came from, the test could assert against the expected data.

## Video

<iframe width="800" height="450" src="https://www.youtube.com/embed/bPIu6PZW41Q?si=I7eIqZccEJwHapEl" title="YouTube video player" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share" referrerpolicy="strict-origin-when-cross-origin" allowfullscreen></iframe>

## Summary

In this blog post, we've explored how to get started with testing in .NET Aspire, allowing us to test scenarios across our distributed applications. We've seen how to write tests for the frontend and API services, ensuring that they are running and responding as expected.

- [.NET Aspire Testing Documentation](https://learn.microsoft.com/dotnet/aspire/fundamentals/testing?pivots=mstest)
