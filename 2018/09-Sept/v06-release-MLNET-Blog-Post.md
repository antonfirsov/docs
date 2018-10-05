# Announcing 'Machine Learning .NET’ 0.6

Today, we’re announcing the release of **ML.NET 0.6**. We released ML.NET 0.1 at //Build 2018, ML.NET is a cross-platform, open source machine learning framework for .NET developers. If you haven’t tried ML.NET yet, here’s how you can get started!

With this ML.NET 0.6 release we largely focused on releasing the first iteration of **new ML.NET APIs** for building and consuming models. These new APIs aer more flexible and enable various new tasks and code workflow that weren't possible with the previous *LearningPipeline* APIs.

In addition we're also adding support for getting predictions (scoring) [ONNX](https://onnx.ai/) models. 

Other enhancements include improvements to ML.NET TensorFlow scoring, performance speed-up for our prediction engine, more consistency with the .NET type-system and having a model deployment compatible for serverless workloads like Azure Functions.

The new API is the most important part of this release. This new ML.NET API improves flexibility and ease of use for additional scenarios not possible before. We are starting to deprecate the current `LearningPipeline` API. Because this is a significant change we also want an open discussion where you can provide feedback and help shape the long-term API for ML.NET.

This blog post provides details about the following topics in ML.NET 0.6 release:

* [New API for building and consuming Machine Learning models](http://TBD-Set-Local-URL)
* [Ability to score pre-trained ONNX Models](http://TBD-Set-Local-URL)
* [Improvements to TensorFlow model scoring functionality](http://TBD-Set-Local-URL)
* [Performance and Type-system improvements](http://TBD-Set-Local-URL)


## New API for building and consuming a Machine Learning model

While the existing LearningPipeline API released with ML.NET 0.1 was easy to get started with, it did have a [few limitations](http://TBD/linktolimitations). Moving forward we have moved the LearningPipeline API into Microsoft.ML.Legacy namespace (e.g. [Sentiment Analysis based on Binary Classification with the LearningPipeline API](https://github.com/dotnet/machinelearning-samples/blob/master/samples/csharp/getting-started/BinaryClassification_SentimentAnalysis/Program.cs))

The new API is designed to support a wider set of scenarios and closely follows ML principles and naming from other popular ML related frameworks like Apache Spark and Scikit-Learn. 

Let’s walkthrough an example on how to build a sentiment analysis model with the new APIs and introduce the new concepts along the way.

Building an ML Model involves the following high-level steps:

![High level steps to build an ML model](v06-release-MLNET-Blog-Post-IMAGES/ml-model-high-level-steps.png)

To go through these steps with ML.NET there are essentially five main concepts with the new API, let’s take a look with them through an example like building a Sentiment Analysis model based on a binary classification task.

### Step 1: Create an ML Context 

When building a model with ML.NET you have to start first creating an ML Context or environment. This is comparable to using DbContext in Entity Framework, but of course, in a completely different domain. The environment provides you essentially a context for your ML job that can be used for exception tracking and logging. 

```cs
var env = new LocalEnvironment();
```

We are working on bringing this concept/naming closer to EF and other .NET frameworks.

### Step 2: Read your data 

One of the most important things is, as always, your data! You need to load a Dataset into the ML pipeline to be used to train your model.

In ML.NET data is similar to a SQL view. It is lazily evaluated, schematized, heterogenous. For building our sentiment analysis model this is how our sample data is going to look like:

| Toxic  (label) | Comment  (text)                                      |
|----------------|------------------------------------------------------|
| 1              | ==RUDE== Dude, you are rude …                        |
| 1              | == OK! == IM GOING TO VANDALIZE …                    |
| 0              | I also found use of the word "humanists” confusing … |
| 0              | Oooooh thank you Mr. DietLime …                      |

To read in this data you will use a data reader which is an ML.NET component. The reader takes in the environment and requires you to define the schema of your data. In this case the first column (Toxic) is of type Boolean and the "label" (meaning also the prediction) and the second column (Comment) is the feature of type text/string that we are going to use to predict the sentiment on.

```cs
var reader = TextLoader.CreateReader(env, ctx => (label: ctx.LoadBool(0),
text: ctx.LoadText(1)));
```

The schema of your data is in this case composed by a boolean column (Toxic) which is the "label" and positioned as the first column. Then, the second is a text column (Comment) which is the feature we are going to use to predict.

Note that this case, loading your training data from a file, is the easiest way to get started, but ML.NET also allows you to load data from databases or in-memory collections.

### Step 3: Transform your data 

Machine learning algorithms understand *featurized* data, so the next step is for us to transform our textual data into a format that we can use our ML algorithms on. In order to do so we need to create an estimator and use the FeaturizeText transform as shown below.

```cs
var est = reader.MakeNewEstimator().Append(row => (label: row.label,
                                                   text:row.text.FeaturizeText()));
```

### Step 4: Add your ML Learner 

Now that our text has been *featurized*, the next step then is to add a learner. In this case we will use the [SDCAClassifier learner](https://docs.microsoft.com/en-us/dotnet/api/microsoft.ml.trainers.stochasticdualcoordinateascentclassifier?view=ml-dotnet).

Adding a learner also requires us to create a context, since we are performing a binary classification ML task for our sentiment analysis we need to define an additional context. 

```cs
var bctx = new BinaryClassificationContext(env);

var est = reader.MakeNewEstimator().Append(row => (label: row.label,
                                                   text: row.text.FeaturizeText()))
                                   .Append(row => (label: row.label,
                                                   prediction:bctx.Trainers.Sdca(row.label,            
                                                                                 row.text)))
```

The learner takes in the `label`, and the *featurized* `text` as input parameters and returns a `prediction` which contains the `predictedLabel`, probability and score field triplet, as shown in the las `.Append()` code below. 

The `predictedLabel` field contains the Boolean result of the prediction. 

The probability and score provide additional metrics about the prediction being made. 

```cs
var est = reader.MakeNewEstimator().Append(row => (label: row.label,
                                                   text: row.text.FeaturizeText()))
                                   .Append(row => (label: row.label,
                                                   prediction: bctx.Trainers.Sdca(row.label, 
                                                                                  row.text)))
                                   .Append(row => (label: row.label,
                                                   prediction: row.prediction,
                                                   predictedlabel: row.prediction.predictedLabel));
```

### Step 5: Build and train your model

Once the estimator has been defined, we can go ahead and train our model using the Fit() API. This returns us back a model which we can then use for predictions.

```cs
var model = est.Fit(traindata);
```

### Step 6: Evaluate your trained model 

Now that you've created and trained the model, you need to evaluate it with a different dataset for quality assurance and validation with code similar to the following:

```cs
// Evaluate the model
var predictions = model.Transform(testdata);
var metrics = bctx.Evaluate(predictions, row => row.label, row => row.prediction);
Console.WriteLine("PredictionModel quality metrics evaluation");
Console.WriteLine("------------------------------------------");
Console.WriteLine($"Accuracy: {metrics.Accuracy:P2}");
```
Basically that codes implements the following:

* Loads the test dataset.
* Evaluates the model and create metrics.
* Shows the accuracy of the model from the metrics.



## Ability to score pre-trained ONNX Models

[ONNX](http://onnx.ai/) is an open model format that enables developers to more easily move models between different tools.

In ML.NET v0.3 we added the capability of [exporting ML.NET models to the ONNX-ML format](https://blogs.msdn.microsoft.com/dotnet/2018/07/09/announcing-ml-net-0-3/#onnx-section) so additional execution environments could run the model (such as *Windows ML*).

In this new v0.6 release, ML.NET can also act as an [ONNX backend](https://github.com/onnx/onnx/blob/master/docs/ImplementingAnOnnxBackend.md#what-is-an-onnx-backend), meaning that in ML.NET you can now score/predict trained ONNX models (ONNX standard v1.2) created somewhere else thanks to a new *transformer* and runtime for scoring ONNX models, as ilustrated in the following figure.

![Process exporting and scoring ONNX models](v06-release-MLNET-Blog-Post-IMAGES/onnx-scoring-diagram.png)

There is a large [variety of ONNX models](https://github.com/onnx/models) created and trained in [multiple frameworks](https://github.com/onnx/tutorials#onnx-tutorials) that can export models to ONNX format. Those models can be used for tasks like image classification, emotion recognition, and object detection.

The ONNX *transformer* in ML.NET enables providing some data to an existing ONNX model (such as the models above) and getting the score (prediction) from it.

The ONNX runtime in ML.NET currently supports only Windows on x64 CPU. Support for other platforms (Linux and macOS) are in the roadmap.

The way you use an ONNX model in your estimator is by simply adding it with this line of code similar to the following:

```cs
.Append(row => (row.name, softmaxout_1: row.data_0.ApplyOnnxModel(modelFile)));
```

Further example usage can be found [here](https://github.com/dotnet/machinelearning/blob/master/test/Microsoft.ML.OnnxTransformTest/OnnxTransformTests.cs#L186).

## Improvements to TensorFlow model scoring functionality

The [TensorFlow scoring transform](https://docs.microsoft.com/en-us/dotnet/api/microsoft.ml.transforms.tensorflowtransform?view=ml-dotnet) released in ML.NET 0.5 enabled using 'frozen' TensorFlow models. In ML.NET 0.6, 'saved' TensorFlow models can also be used.

An API was added to extract information about the nodes in a TensorFlow model. This can help identifying the input and output of a TensorFlow model. Example usage can be found [here](https://github.com/dotnet/machinelearning/blob/3cdd3c8b32705e91dcf46c429ee34196163af6da/src/Microsoft.ML.DnnAnalyzer/Microsoft.ML.DnnAnalyzer/DnnAnalyzer.cs).


## Performance and Type-system improvements

In v0.6 we have identified up to [~100x speedup](https://github.com/dotnet/machinelearning/issues/1013#issuecomment-426117666) in prediction engine performance for single records.

Replaced ML.NET's Dv type system with .NET's standard type system: 

* ML.NET previously had its own type system which helped it more efficiently deal with things like missing values (a common case in ML). This type system required users to work with types like DvText, DvBool, DvInt4, etc.

* This update replaces the Dv type system with .NET's standard type system to make ML.NET easier to use and to take advantage of innovation in .NET.

* One effect of this change is that only floats and doubles have missing values, represented by NaN. More information can be found [here](https://github.com/dotnet/machinelearning/issues/673).

As further improvement, you can now use additional .NET app models such as *Azure Functions* easily without convoluted workarounds, thanks to the improved approach to dependency injection.


## Additional resources

* The most important **ML.NET concepts** for understanding the new API are introduced [here](https://github.com/dotnet/machinelearning/blob/3cdd3c8b32705e91dcf46c429ee34196163af6da/docs/code/MlNetHighLevelConcepts.md).

* A **cookbook** that shows how to use these APIs for a variety of existing and new scenarios can be found [here](https://github.com/dotnet/machinelearning/blob/3cdd3c8b32705e91dcf46c429ee34196163af6da/docs/code/MlNetCookBook.md).


## Provide your feedback on the new API
![Provide feedback image with two people and a swimlane](v06-release-MLNET-Blog-Post-IMAGES/swimlane-feedback.png)

As mentioned at the begining of the blog post, the new API is a significant change, so we also want to create an open discussion where you can provide feedback and help shape the long-term API for ML.NET.

Want to get involved? Start by providing feedback at this blog post comments below or through issues at the [ML.NET GitHub repo](https://github.com/dotnet/machinelearning/issues)

## Get started!

If you haven’t already, get started with [ML.NET here](https://www.microsoft.com/net/learn/apps/machine-learning-and-ai/ml-dotnet/get-started/windows)!
 
Next, explore some other great resources:

  * Tutorials and resources at the [Microsoft Docs ML.NET Guide](https://docs.microsoft.com/en-us/dotnet/machine-learning/)
  * Code samples at the [machinelearning-samples GitHub repo](https://github.com/dotnet/machinelearning-samples)

We look forward to your feedback and welcome you to file issues with any suggestions or enhancements in the [ML.NET GitHub repo](https://github.com/dotnet/machinelearning).



*This blog was authored by Ankit Asthana and Cesar de la Torre*

Thanks,

The ML.NET Team


