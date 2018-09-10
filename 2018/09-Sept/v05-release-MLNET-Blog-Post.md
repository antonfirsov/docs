# Announcing ML.NET 0.5

It’s been a few months already since we [released ML.NET 0.1 at //Build 2018](https://blogs.msdn.microsoft.com/dotnet/2018/05/07/introducing-ml-net-cross-platform-proven-and-open-source-machine-learning-framework/), a cross-platform, open source machine learning framework for .NET developers. While we’re evolving through new preview releases, we are getting great feedback and would like to thank the community for your engagement as we continue to develop ML.NET together in the open. 

Today we are happy to announce the latest version: **ML.NET 0.5**. In this release we are adding **[TensorFlow](https://www.tensorflow.org/) model scoring** as a **transform** to ML.NET. This enables using an existing TensorFlow model within an ML.NET experiment. In this release we are also addressing a variety of issues and feedback we received from the community. We welcome feedback and contributions to the conversation: relevant issues can be found [here](https://github.com/dotnet/machinelearning/projects/4).

As part of the upcoming road in ML.NET, we really want your feedback on making ML.NET easier to use. We are working on a new API which improves flexibility and ease of use. When the new API is ready and good enough, we plan to deprecate the current “pipeline” API. Because this will be a significant change we want to share our proposals for the multiple API options and comparisons at the end of this blog post and start an open discussion with you where you can provide your feedback and help shape the long-term API for ML.NET.

This blog post provides details about the following topics in ML.NET:

* Added a TensorFlow model scoring transform (TensorFlowTransform) to ML.NET v0.5

* New API proposal exploration for you to provide feedback for upcoming versions


## Added a TensorFlow model scoring transform (TensorFlowTransform)

[TensorFlow](https://www.tensorflow.org/) is a popular machine learning
      toolkit that enables training deep neural networks (and general numeric
      computations).

`TensorFlowTransform` enables taking an existing TensorFlow model, either
      trained by you or downloaded from somewhere else, and get the scores
      from the model in ML.NET.

The implementation of this transform is based on code from [TensorFlowSharp](https://github.com/migueldeicaza/TensorFlowSharp).

Through ML.NET you can now vey easily use TensorFlow models for scoring by simply using the ML.NET NuGet packages in your .NET Core or .NET Framework apps, as shown in the diagram.

![TensorFlow-ML.NET application diagram](v05-release-MLNET-Blog-Post-IMAGES/TensorFlow-MLNET-NuGet-App-Diagram.png)

In the following code snippet you can see how you can use the TensorFlow transform in the ML.NET pipeline:

```cs

// ... Additional transformations in the pipeline code

pipeline.Add(new TensorFlowScorer()
{
    ModelFile = "model/tensorflow_inception_graph.pb",   // Example using the Inception v3 TensorFlow model
    InputColumns = new[] { "input" },                    // Name of input in the TensorFlow model
    OutputColumn = "softmax2_pre_activation"             // Name of output in the TensorFlow model
});

// ... Additional code specifying a learner and training process for the ML.NET model

```

The code example above uses the pre-trained TensorFlow model named *Inception v3*, available [here](https://storage.googleapis.com/download.tensorflow.org/models/inception5h.zip). 

In next releases, we will add functionality in ML.NET to enable identifying the expected inputs and outputs of TensorFlow models. For now, you can use the TensorFlow APIs or a tool like [Netron](https://github.com/lutzroeder/Netron) to explore the TensorFlow model.

If you open the previous sample TensorFlow model file (`tensorflow_inception_graph.pb`) with [Netron](https://github.com/lutzroeder/Netron) and explore the model's graph, you can see how it correlates the `InputColumn` with the node's `input` at the begining of the graph:

![TensorFlow model's input in graph](v05-release-MLNET-Blog-Post-IMAGES/Input-Node-TF-Model.png)

And how the `OutputColumn` correlates with `softmax2_pre_activation` node's output almost at the end of the graph.

![TensorFlow model's input in graph](v05-release-MLNET-Blog-Post-IMAGES/Output-Node-TF-Model.png)

*Important note:* For now (when using the "pipeline" API), these scores can only be used within a `LearningPipeline` as inputs (numeric vectors)  to a learner like a classifier learner. However, with the upcoming new ML.NET APIs, the scores from the TensorFlow model will be directly accessible, so you could simply score with the TensorFlow model without needing to add any additional learner and train process.

You can find [here](https://github.com/dotnet/machinelearning/blob/6ac380a4d3f44ee7b015461f74c4298b0ed5184b/test/Microsoft.ML.Tests/Scenarios/TensorflowTests.cs) an additional and complete code example using the `TensorFlowTransform` with the existing `LearningPipeline` API.



## Explore the upcoming new ML.NET API and provide feedback

As mentioned at the begining of this blog post, we are really looking forward to get your feedback on the new API we're creating while crafting ML.NET. This evolution in ML.NET offers much more flexible capabilities than what current "pipeline" API offers. The "pipeline" API will be deprecated when this new API is ready and good enough.  


### Why ML.NET is switching from the "pipeline" API to a new API?

As part of crafting process in the preview versions (remember that ML.NET is still in early previews), we've been getting feedback about the "pipeline" API and discovered quite a few limitations we need to address by creating a more flexible API.

Specifically, new capabilities provided by the new API which are not possible with the "pipeline" API are the following: 

- *Strongly-typed API*: This new API takes advantage of C# capabilities and offers an Strongly-typed API in ML.NET so errors can be discovered in compilation time while being able to better use Intellisense in the editors. 

- *Better flexibility:* You now have a decomposalbe tran and predict process. No more rigid and linear pipeline execution. With the new API you can run certain code and then the execution can be forked so those multiple paths can re-use the initial common execution. For example, you can share the same transforms execution and transformed data by multiple learners and trainings. 
This ne API is based on the new `Estimators`, shown in the code below in this blog post. 

- *Improved usability:* Direct call to the APIs from your code, no more scaffolding or insolation layer creating an obscure separation between what the user/developer writes and the internal APIs. Entrypoints are no longer mandatory. 

- *Possible to simply score with TensorFlow models.* Thanks to the mentioned flexibility in the API, you can also simply load a TensorFlow model and score by using it without needing to add any additional learner and training process.

- *Better visibility of the transformed data:* You can now better visibility of the data while applying transformers.

As sample code to discuss about, the code snippet below shows how the transforms and trainign process of the "GitHub issues labelers" sample app can be implemented with the new API in ML.NET.

**New API in ML.NET:**

```cs

        public static async Task BuildAndTrainModelToClassifyGithubIssues()
        {
            var env = new TlcEnvironment(new SysRandom(0), verbose: true);

            string dataPath = "corefx-issues-train.tsv";

            // Create reader with specific schema. 
            // string :ID, string: Area, string:Title, string:Description
            var reader = TextLoader.CreateReader(env, ctx =>
                                          (area: ctx.LoadText(1),
                                            title: ctx.LoadText(2),
                                            description: ctx.LoadText(3),
                                            dataPath,
                                            useHeader: true));

            var estimator = reader.MakeEstimator()
                .Append(row => (
                    // Convert string label to key. 
                    label: row.area.Dictionarize(),
                    // Featurizes 'description'
                    description: row.description.FeaturizeText(),
                    // Featurizes 'title'
                    title: row.title.FeaturizeText()))
                .Append(row => (
                    // Concatenate the two features into a vector.
                    features: row.description.ConcatWith(r.title),
                    // Preserve the label
                    label: row.label))
                .Append(row => r.label.PredictSdcaMultiClass(row.features));

            // Read the data
            var data = reader.Read(dataPath);

            // Fit the data
            var model = estimator.Fit(data);

            string modelPath = "github-Model.zip";

            // Save the ML.NET model into a .ZIP file
            await model.WriteAsync(modelPath);
        }

        public static async Task PredictGithubIssues()
        {
            ClassifyGithubIssues();

            // Read model from an ML.NET .ZIP model file
            var model = await PredictionModel.ReadAsync(ModelPath);
            var predictor = model.MakePredictionFunction<IssueInput, IssuePrediction>();

            // This prediction will classify this particular issue in a type such as "EF and Database access"
            var prediction = predictor.PredictSdcaMultiClass(new IssueInput
                {
                    Title = "Sample issue related to Entity Framework", 
                    Description = "When using Entity FRamework Core I'm experiencing database connection failures when running queries or transactions. Looks like it could be related to transient faults in network communication agains the Azure SQL Database..."
                });
        }

```

You can compare that with the old "pipeline" API where you don't have that flexibility because the pipeline execution is not decomposable but is a linear execution.

**Old "pipeline" API:**

```cs
        public static async Task BuildAndTrainModelToClassifyGithubIssuesAsync()
        {
            var pipeline = new LearningPipeline();

            pipeline.Add(new TextLoader(DataPath).CreateFrom<GitHubIssue>(useHeader: true));

            pipeline.Add(new Dictionarizer(("Area", "Label")));

            pipeline.Add(new TextFeaturizer("Title", "Title"));

            pipeline.Add(new TextFeaturizer("Description", "Description"));
            
            pipeline.Add(new ColumnConcatenator("Features", "Title", "Description"));

            pipeline.Add(new StochasticDualCoordinateAscentClassifier());
            pipeline.Add(new PredictedLabelColumnOriginalValueConverter() { PredictedLabelColumn = "PredictedLabel" });

            Console.WriteLine("=============== Training model ===============");

            var model = pipeline.Train<GitHubIssue, GitHubIssuePrediction>();

            await model.WriteAsync(ModelPath);

            Console.WriteLine("=============== End training ===============");
            Console.WriteLine("The model is saved to {0}", ModelPath);
        }
```

For instance, with the "pipeline" API you can see how that code is fully linear and you cannot decompose it in multiple pieces so you could re-use part of its execution like you can when using the new `Estimators`.

Because this will be a significant change we want to share our proposals and start an open discussion with you where you can provide your feedback and help shape the long-term API for ML.NET.


## Provide your feedback on the new API

Feel free to provide your feedback as comments in this blog post or even better, do it in this specially made place for gathering feedback on te new API:

Get involved: http://aka.ms/newapifeedback

## Get started!

If you haven’t already, try out ML.NET you can [get started here](https://www.microsoft.com/net/learn/apps/machine-learning-and-ai/ml-dotnet/get-started/windows).  We look forward to your feedback and welcome you to file issues with any suggestions or enhancements in the GitHub repo.

https://github.com/dotnet/machinelearning



*This blog was authored by Cesar de la Torre, Gal Oshri, John Alexander and Ankit Asthana*

Thanks,

The ML.NET Team


