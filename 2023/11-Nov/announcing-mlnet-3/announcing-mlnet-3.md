---
post_title: Announcing ML.NET 3.0
author1: 'jeffhandley'
microsoft_alias: 'jeffhand'
featured_image: ./announcing-mlnet-3.png
categories: ML.NET, Machine Learning, AI Machine Learning, .NET
summary: Announcing ML.NET 3.0, with improvements for deep learning, DataFrame, performance, and more!
desired_publication_date: '2023-11-27'
post_date: 2023-11-27 16:00:00
---

[ML.NET](https://dot.net/ml) is an open-source, cross-platform machine learning framework for .NET developers that enables integration of custom machine learning models into .NET applications. [ML.NET version 3.0](https://www.nuget.org/packages/Microsoft.ML/3.0.0) is now released, with lots of new features and enhancements!

Deep Learning scenarios were substantially expanded in this release with new capabilities in Object Detection, Named Entity Recognition, and Question Answering. That's all possible because of integrations and interoperability with TorchSharp and ONNX models. We've also updated our integration with LightGBM to the latest version.

Data processing scenarios are greatly improved with a long list of enhancements and bug fixes to `DataFrame`, as well as new `IDataView` interoperability features. The important steps of loading, inspecting, transforming, and visualizing your data are much more powerful.

While this post highlights several aspects of the ML.NET 3.0 release, the full list of updates is available in the [release notes](https://github.com/dotnet/machinelearning/blob/main/docs/release-notes/3.0/release-3.0.0.md).

## Deep Learning

Over the past year, we've all witnessed an acceleration of growth in deep learning scenarios and capabilities. With ML.NET 3.0, you can leverage many of these advancements within your .NET applications.

### Object Detection

Object detection is a computer vision problem. While closely related to image classification, object detection performs image classification at a more granular scale. Object detection both locates and categorizes entities within images. It's best to use object detection when images contain multiple objects of different types.

We announced [Object Detection in ML.NET Model Builder](https://devblogs.microsoft.com/dotnet/object-detection-ml-dotnet-model-builder/) earlier this year. Those capabilities are built on top of the TorchSharp-powered Object Detection APIs introduced in ML.NET 3.0 ([PR #6605](https://github.com/dotnet/machinelearning/pull/6605)).

Under the covers, the Object Detection API leverages some of the latest techniques from Microsoft Research and is backed by a Transformer-based neural network architecture built with TorchSharp. For more details on the underlying model, see the [Searching the Space of Vision Transformer](https://arxiv.org/pdf/2111.14725.pdf) paper.

Object Detection is included in the [Microsoft.ML.TorchSharp 3.0.0 package](https://www.nuget.org/packages/Microsoft.ML.TorchSharp/3.0.0) within the `Microsoft.ML.TorchSharp` and `Microsoft.ML.TorchSharp.AutoFormerV2` namespaces. Read the [Object Detection in ML.NET Model Builder](https://devblogs.microsoft.com/dotnet/object-detection-ml-dotnet-model-builder/) blog post for an in-depth look.

```c#
    var chain = new EstimatorChain<ITransformer>();

    var filteredPipeline = chain.Append(
            mlContext.Transforms.Text.TokenizeIntoWords(labelColumnName, separators: [',']),
            TransformerScope.Training
        )
        .Append(
            mlContext.Transforms.Conversion.MapValueToKey(labelColumnName),
            TransformerScope.Training
        )
        .Append(
            mlContext.Transforms.Text.TokenizeIntoWords(boundingBoxColumnName, separators: [',']),
            TransformerScope.Training
        )
        .Append(
            mlContext.Transforms.Conversion.ConvertType(boundingBoxColumnName),
            TransformerScope.Training
        )
        .Append(mlContext.Transforms.LoadImages("Image", imageFolder, "ImagePath"))
        .Append(
            mlContext.MulticlassClassification.Trainers.ObjectDetection(
                labelColumnName, predictedLabelColumnName, scoreColumnName,
                boundingBoxColumnName, predictedBoundingBoxColumnName,
                imageColumnName, maxEpoch
            )
        )
        .Append(mlContext.Transforms.Conversion.MapKeyToValue(predictedLabelColumnName));

    var options = new ObjectDetectionTrainer.Options()
    {
        LabelColumnName = labelColumnName,
        BoundingBoxColumnName = boundingBoxColumnName,
        ScoreThreshold = .5,
        MaxEpoch = maxEpoch,
        LogEveryNStep = 1,
    };

    var pipeline = mlContext.Transforms.Text.TokenizeIntoWords(labelColumnName, separators: [','])
        .Append(mlContext.Transforms.Conversion.MapValueToKey(labelColumnName))
        .Append(mlContext.Transforms.Text.TokenizeIntoWords(boundingBoxColumnName, separators: [',']))
        .Append(mlContext.Transforms.Conversion.ConvertType(boundingBoxColumnName))
        .Append(mlContext.Transforms.LoadImages("Image", imageFolder, "ImagePath"))
        .Append(mlContext.MulticlassClassification.Trainers.ObjectDetection(options))
        .Append(mlContext.Transforms.Conversion.MapKeyToValue(predictedLabelColumnName));

    var model = pipeline.Fit(data);
    var idv = model.Transform(data);

    var metrics = ML.MulticlassClassification.EvaluateObjectDetection(
        idv, idv.Schema[2], idv.Schema[boundingBoxColumnName], idv.Schema[predictedLabelColumnName],
        idv.Schema[predictedBoundingBoxColumnName], idv.Schema[scoreColumnName]
    );
```

### Named Entity Recognition and Question Answering

Natural Language Processing is one of the most common ML needs in software. Two of the most substantial areas of advancement in NLP have been Question Answering (QA) and Named Entity Recognition (NER). Both of these scenarios are unlocked in ML.NET 3.0 by building on top of the existing TorchSharp RoBERTa text classification features introduced in ML.NET 2.0.

Both the NER and QA trainers are included in the [Microsoft.ML.TorchSharp 3.0.0 package](https://www.nuget.org/packages/Microsoft.ML.TorchSharp/3.0.0) and the `Microsoft.ML.TorchSharp` namespace.

```c#
    // QA trainer
    var chain = new EstimatorChain<ITransformer>();
    var estimatorQA = chain.Append(mlContext.MulticlassClassification.Trainers.QuestionAnswer(
        contextColumnName, questionColumnName, trainingAnswerColumnName,
        answerIndexColumnName, predictedAnswerColumnName, scoreColumnName,
        topK, batchSize, maxEpochs, architecture, validationSet
    ));

    // NER trainer
    var estimatorNER = chain.Append(mlContext.Transforms.Conversion.MapValueToKey("Label", keyData))
        .Append(mlContext.MulticlassClassification.Trainers.NameEntityRecognition(
            labelColumnName, outputColumnName, sentence1ColumnName,
            batchSize, maxEpochs, architecture, validationSet
        ))
        .Append(mlContext.Transforms.Conversion.MapKeyToValue(outputColumn));
```

## Intel oneDAL Training Acceleration

Shortly after we released ML.NET 2.0, we announced training hardware acceleration powered by Intel oneDAL as part of the first preview of ML.NET 3.0. Intel oneDAL ([Intel oneAPI Data Analytics Library](https://www.intel.com/content/www/us/en/developer/tools/oneapi/onedal.html)) is a library that helps speed up data analysis by providing highly optimized algorithmic building blocks for all stages of the data analytics and machine learning process. Intel oneDAL makes use of the SIMD extensions in 64-bit architectures, which are featured in Intel and AMD CPUs.

Refer back to the [Accelerate ML.NET training with Intel oneDAL](https://devblogs.microsoft.com/dotnet/accelerate-ml-net-training-with-intel-onedal/) blog post for more on this feature set.

## Automated Machine Learning (AutoML)

Automated Machine Learning (AutoML) automates the process of applying machine learning to data. AutoML powers experiences like those found in Model Builder and the ML.NET CLI.

With ML.NET 3.0, the AutoML experience gained several new capabilities. The AutoML Sweeper now supports Sentence Similarity, Question Answering, and Object Detection. Community member [Antti "Andy" Törrönen (@torronen)](https://github.com/torronen) implemented a sampling key column name (`SamplingKeyColumnName`) that can be used with `SetDataset` to more easily set the sampling key name. The `AutoZero` tuner can now be used in `BinaryClassification` experiments. The maximum number of models used for an experiment can be specified through `ExperimentSettings.MaxModel`.

Thanks to community member [Andras Fuchs (@andrasfuchs)](https://github.com/andrasfuchs), continuous resource monitoring is available through `AutoML.IMonitor`. This allows monitoring of memory demand, virtual memory usage, and remaining disk space. With that monitoring, long-running experiments can be controlled through a custom `IMonitor` implementation to avoid crashes and failed trials.

## DataFrame

This release includes a long list of notable updates to `DataFrame`, many of which were completed by a community member, [Aleksei Smirnov (@asmirnov82)](https://github.com/asmirnov82). We appreciate Aleksei's contributions and we're sure you will too!

To enable more `IDataView` <-> `DataFrame` conversions, support for both `String` and `VBuffer` column types have been added. `String` values are handled as `ReadOnlyMemory<char>` , and the `VBufferDataFrameColumn<T>` column type supports all backing primitives. Columns can now store more than 2 Gb of data as well, with the previous limitation being removed. Apache Arrow `Date64` column data is recognized now too.

Data loading scenarios for `DataFrame` are expanded in ML.NET 3.0. Data can now be imported from and exported to SQL databases thanks to community member, [Andrei Faber (@andrei-faber)](https://github.com/andrei-faber). This is accomplished using ADO.NET, which supports a large number of SQL-compatible databases. As part of this implementation, it also became possible to load data from any `IEnumerable` collection and export data to `System.Data.DataTable`. Data from one `DataFrame` can now be appended into another `DataFrame` when their column names match, relaxing a previous constraint on column ordering. Comma-separated data loaded through `DataFrame.LoadCsv` can now handle duplicate column names too, with the option to rename duplicate columns.

There were many other enhancements and fixes to `DataFrame` too. Arithmetic performance was improved in column cloning and binary comparison scenarios. Null value handling was improved while performing arithmetic operations, requiring fewer steps of transforming and cleaning data. There were even debugger improvements that produce more readable output for columns with long names.

## Tensor Primitives Integration

Tensor Primitives is short for `System.Numerics.Tensors.TensorPrimitives`, a new set of APIs that introduce support for tensor operations. As part of .NET 8, our team released a new [System.Numerics.Tensors](https://www.nuget.org/packages/System.Numerics.Tensors/8.0.0) package that introduced Tensor Primitives. The Tensor Primitives APIs are the next step in the evolution of Numerics for AI in .NET, building on the momentum of hardware intrinsics and Generic Math.

While the integration with Tensor Primitives is purely an implementation detail that doesn't affect the public surface area of ML.NET, it brings some notable performance improvements. The following benchmark results illustrate the gains while targeting .NET 8.

|          Method | arrayLength | Mean - Original | Mean - New | % Faster |
|---------------: |-----------: |---------------: |-----------:|---------:|
|      AddScalarU |         512 |  25.30 ns       |  20.32 ns  | 25%      |
|           Scale |         512 |  19.91 ns       |  19.29 ns  |  3%      |
|       ScaleSrcU |         512 |  27.58 ns       |  20.74 ns  | 33%      |
|       ScaleAddU |         512 |  28.46 ns       |  29.05 ns  | ---      |
|       AddScaleU |         512 |  29.74 ns       |  28.59 ns  |  4%      |
|      AddScaleSU |         512 | 345.92 ns       | 327.68 ns  |  6%      |
|   AddScaleCopyU |         512 |  34.01 ns       |  27.03 ns  | 26%      |
|            AddU |         512 |  29.80 ns       |  26.71 ns  | 12%      |
|           AddSU |         512 | 325.32 ns       | 349.46 ns  | ---      |
| MulElementWiseU |         512 |  33.92 ns       |  27.29 ns  | 24%      |
|             Sum |         512 |  36.57 ns       |  34.34 ns  |  6%      |
|          SumSqU |         512 |  37.50 ns       |  39.34 ns  | -5%      |
|      SumSqDiffU |         512 |  41.23 ns       |  43.38 ns  | ---      |
|         SumAbsU |         512 |  43.74 ns       |  39.27 ns  | 11%      |
|     SumAbsDiffU |         512 |  47.23 ns       |  37.48 ns  | 26%      |
|         MaxAbsU |         512 |  42.30 ns       |  43.26 ns  | ---      |
|     MaxAbsDiffU |         512 |  46.94 ns       |  47.73 ns  | ---      |
|            DotU |         512 |  50.34 ns       |  43.20 ns  | 17%      |
|           DotSU |         512 | 212.19 ns       | 213.18 ns  | ---      |
|           Dist2 |         512 |  55.48 ns       |  47.43 ns  | 17%      |

More details and the .NET Framework benchmark results are included in the [dotnet/machinelearning#6875](https://github.com/dotnet/machinelearning/pull/6875) pull request that introduced this integration.

Beyond these performance gains, we also used this integration opportunity as a means for testing the API shape, usability, functionality, and correctness of the `TensorPrimitives` APIs. Proving that the APIs could satisfy the ML.NET scenarios was a valuable step toward bringing the System.Numerics.Tensors package out of preview with a stable 8.0.0 version.

## What's Next

With the .NET 8 and ML.NET 3.0 releases completed, we are working on our plans for .NET 9 and ML.NET 4.0. Much sooner than that though, you can expect Model Builder and the ML.NET CLI to be updated to consume the ML.NET 3.0 release.

We know we will continue expanding deep learning scenarios and integrations, and we know we will keep making enhancements to DataFrame. We will keep expanding the APIs available in System.Numerics.Tensors and integrating them into ML.NET. Stay tuned for more detailed ML.NET 4.0 plans.

## Get started and resources

Learn more about ML.NET, Model Builder, and the ML.NET CLI at [Microsoft Learn](https://learn.microsoft.com/dotnet/machine-learning/).

If you run into any issues, feature requests, or feedback, please file an issue in the [ML.NET repo](https://github.com/dotnet/machinelearning).

Join the [ML.NET Community Discord](https://aka.ms/virtual-mlnet-community-discord) or #machine-learning channel on the [.NET Development Discord](https://aka.ms/dotnet-discord).

Tune in to the [Machine Learning .NET Community Standup](https://dotnet.microsoft.com/live/community-standup) every other Wednesday at 10am Pacific Time.
