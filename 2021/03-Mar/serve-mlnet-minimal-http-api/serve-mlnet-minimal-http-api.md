---
post_title: "Serve ML.NET Models as HTTP APIs with minimal configuration"
username: luisquintanilla
microsoft_alias: luquinta
categories: .NET, ML.NET
summary: "Learn how to serve machine learning models from HTTP APIs using minimal configuration"
desired_publication_date: '2021-03-01'
---

## Introduction

One of the most difficult tasks of building machine learning applications is deploying them to production. The ML.NET team is exploring ways to simplify the process and would like to hear your feedback.

When it comes to deploying machine learning models as web services, the bare minimum you need is a single endpoint to handle making predictions. One way to do that in .NET is using a technique known as "route-to-code." In this post, I'll show how "route-to-code" can help you quickly build highly scalable machine learning web services in about 60 lines of code!

For more information on ["route-to-code"](https://docs.microsoft.com/aspnet/core/web-api/route-to-code?view=aspnetcore-5.0), see the ASP.NET documentation.

## Install the Microsoft.Extensions.ML NuGet package

For web applications, it's recommended to use the `PredictionEnginePool` service. This is a scalable service which provides an `ObjectPool` containing `PredictionEngine` objects that use an ML.NET model to make predictions on new data. The `PredictionEnginePool` service is part of the [`Microsoft.Extensions.ML` NuGet package](https://www.nuget.org/packages/Microsoft.Extensions.ML/).

If you're using the CLI, you can use the following command to install the NuGet package.

```dotnetcli
dotnet add package Microsoft.Extensions.ML
```

### Configure the web application

A standard ASP.NET convention is to configure your application's services and request pipelines in a class called `Startup`. Because we're working with a single service and endpoint, we'll instead configure our application inside the `Program` class. Add the following code to your `Main` method.

```csharp
WebHost.CreateDefaultBuilder()
    .ConfigureServices(services => {
        // Register PredictionEnginePool service 
        services.AddPredictionEnginePool<Input,Output>()
            .FromUri("https://github.com/dotnet/samples/raw/master/machine-learning/models/sentimentanalysis/sentiment_model.zip");
    })
    .Configure(app => {
            app.UseHttpsRedirection();
    })
    .Build()
    .Run();
```

This code defines and builds the application's web host. It also registers a `PredictionEnginePool` for a model hosted on GitHub. Once registered you can use this service anywhere in your application using dependency injection.

### Define model input and output schemas

Machine learning models use patterns learned from the training process to generate predictions using new data as input. The machine learning model used in this sample analyzes sentiment from input text and categorizes it as positive or negative.

Define both of these classes in your application.

```csharp
public class Input
{
	public string SentimentText;

	[ColumnName("Label")]
	public bool Sentiment;
}

public class Output
{
	[ColumnName("PredictedLabel")]
	public bool Prediction { get; set; }

	public float Probability { get; set; }

	public float Score { get; set; }
}
```

The `Input` and `Output` classes in this case define the schema of the model's input and output respectively. Given an input value in the `SentimentText` property, the model outputs a boolean value for the `Prediction`, where zero is negative and one is positive sentiment.

### Create a handler to make predictions

To process incoming requests, you'll want to create a handler. The handler is a method that leverages the `HttpContext` to access registered services (in this case `PredictionEnginePool`), read the request, and write out a response.

```csharp
static async Task PredictHandler(HttpContext http)
{
	// Get PredictionEnginePool service
	var predEngine = http.RequestServices.GetRequiredService<PredictionEnginePool<Input,Output>>();

	// Deserialize HTTP request JSON body
	var input = await JsonSerializer.DeserializeAsync<Input>(http.Request.Body);

	// Predict using PredictionEnginePool service
	var prediction = predEngine.Predict(input);

	// Return prediction as JSON response
	await http.Response.WriteAsJsonAsync(prediction);
}
```

### Configure routes

Now that you have a handler, configure your application to route requests to your handler. Add the following code inside the `Configure` method.

```csharp
app.UseRouting();
app.UseEndpoints(endpoints => {
    // Define prediction endpoint
    endpoints.MapPost("/predict", PredictHandler);
});
```

This code maps HTTP POST requests to the `predict` endpoint and uses the `PredictHandler` previously created to process those requests.

Your final `Main` method should look like the following:

```csharp
WebHost.CreateDefaultBuilder()
    .ConfigureServices(services => {
        // Register PredictionEnginePool service 
        services.AddPredictionEnginePool<Input,Output>()
            .FromUri("https://github.com/dotnet/samples/raw/master/machine-learning/models/sentimentanalysis/sentiment_model.zip");
    })
    .Configure(app => {
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseEndpoints(endpoints => {
            // Define prediction endpoint
            endpoints.MapPost("/predict", PredictHandler);
        });
    })
    .Build()
    .Run();      
```

That's all you need to serve your machine learning model as an HTTP API!

## Test your API

To test your API, run the application and make an HTTP POST request with a JSON body containing the `SentimentText` property.

```json
{
	"SentimentText": "This is a very bad steak"
}
```

You should receive an `Output` response similar to the following:

```json
{
  "prediction": false,
  "probability": 0.5,
  "score": 0
}
```

The `false` value in `prediction` indicates that the `SentimentText` provided in the request is negative.

## Conclusion

In this post, we showed how "route-to-code" can help you quickly write a highly scalable machine learning ASP.NET web service. Try deploying your own machine learning models using the minimal "route-to-code" method and [give us feedback](https://github.com/dotnet/machinelearning-modelbuilder/issues) on how to make it better.

You can find a complete version of this application in the [ML.NET HTTP API GitHub repository](https://github.com/luisquintanilla/mlnet-http-api). In that repository you'll also find a [sample](https://github.com/luisquintanilla/mlnet-http-api/tree/api-endpoints) that uses the API Endpoints NuGet package to enable model binding and OpenAPI/Swagger.