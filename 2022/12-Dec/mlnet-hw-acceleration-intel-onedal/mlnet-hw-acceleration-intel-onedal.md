---
post_title: Accelerate ML.NET training with Intel oneDAL
username: 'luquinta@microsoft.com'
author1: 'luquinta@microsoft.com'
microsoft_alias: 'luquinta'
featured_image: ./mlnet-icon.png
categories: ML.NET, Machine Learning, AI Machine Learning, .NET
summary: The first preview release of ML.NET 3.0 brings training hardware acceleration improvements powered by Intel oneDAL.
desired_publication_date: '2022-12-20'
post_date: 2022-12-20 09:04:00
---

[ML.NET](https://dot.net/ml) is an open-source, cross-platform machine learning framework for .NET developers that enables integration of custom machine learning models into .NET apps.

Just over a month ago, we released ML.NET 2.0. Thank you for trying it out and giving us feedback.

We're not stopping there though and are excited to introduce the first preview release of ML.NET 3.0. This release brings several hardware acceleration improvements that allow you to make the most out of your compute resources during training.

Install the latest [ML.NET 3.0 preview version](https://aka.ms/mlnet-3-preview1) to try out the latest improvements powered by Intel oneDAL and give us feedback.

## What is Intel oneAPI Data Analytics Library (oneDAL)

[Intel oneAPI Data Analytics Library](https://www.intel.com/content/www/us/en/developer/tools/oneapi/onedal.html) is a library that helps speed up data analysis by providing highly optimized algorithmic building blocks for all stages of the data analytics and machine learning process.

oneDAL makes use of the SIMD extensions in 64-bit architectures, which are featured in Intel and AMD CPUs

## oneDAL components in ML.NET

oneDAL integrates into ML.NET by accelerating existing trainers during training. Currently, the following ML.NET trainers provide oneDAL support.

| Trainer | Machine Learning Task |
| --- | --- |
| Ordinary Least Squares | Regression |
| L-BGFS | Classification |
| FastTree | Regression & Classification |
| FastForest | Regression & Classification |

## Get started with oneDAL in ML.NET

1. Install the latest [`Microsoft.ML` 3.0 preview version](https://aka.ms/mlnet-3-preview1).

    ```bash
    dotnet add package Microsoft.ML --prerelease
    ```

    If you're using `OLS` or `FastTree`, you'll have to install additional packages

    ```bash
    # Ordinary Least Squares (OLS)
    dotnet add package Microsoft.ML.Mkl.Components --prerelease
    
    # FastTree
    dotnet add package Microsoft.ML.FastTree --prerelease
    ```

1. Set the `MLNET_BACKEND` environment variable to `ONEDAL`. If you're using one of the trainers supported by oneDAL, there are no code changes required.
1. Create a pipeline using one of the oneDAL-supported ML.NET trainers. In this sample, it's using `LbfgsLogisticRegression` to train a binary classification model to predict oneDAL support.  

    ```csharp
    // Initialize MLContext
    var ctx = new MLContext();
    
    // Define data
    var trainingData = new [] 
    {
        new {Arch="ARM", Trainer="LightGBM", oneDALSupport=false},
        new {Arch="x86", Trainer="FastTree", oneDALSupport=true},
        new {Arch="x86", Trainer="LbfgsLogisticRegression", oneDALSupport=true},
        new {Arch="ARM", Trainer="FastTree", oneDALSupport=false}
    };
    
    // Load data into IDataView
    var trainingDv = ctx.Data.LoadFromEnumerable(trainingData);
    
    // Define data processing pipeline & trainer
    var pipeline = 
        ctx.Transforms.Categorical.OneHotEncoding(new [] {
                new InputOutputColumnPair("ArchEncoded", "Arch"),
                new InputOutputColumnPair("TrainerEncoded", "Trainer")})
            .Append(ctx.Transforms.Concatenate("Features", "ArchEncoded", "TrainerEncoded"))
            .Append(ctx.BinaryClassification.Trainers.LbfgsLogisticRegression(labelColumnName:"oneDALSupport"));
    
    // Train model
    var model = pipeline.Fit(trainingDv)
    ```

1. Train your model.

For a more complete example, see this [sample](https://aka.ms/onedal-sample). 

## What's next?

We're just getting started with ML.NET 3.0 development and are excited about the improvements and new capabilities we're looking to enable. For more details, see the [ML.NET roadmap](https://aka.ms/mlnet-roadmap).

## Thank you

We are extremely grateful to our Intel partners as none of these improvements would be possible without them.

## Get started and resources

Learn more about ML.NET, Model Builder, and the ML.NET CLI in [Microsoft Docs](https://docs.microsoft.com/dotnet/machine-learning/).

If you run into any issues, feature requests, or feedback, please file an issue in the [ML.NET repo](https://github.com/dotnet/machinelearning/issues).

Join the [ML.NET Community Discord](https://aka.ms/virtual-mlnet-community-discord) or #machine-learning channel on the [.NET Development Discord](https://aka.ms/dotnet-discord).

Tune in to the [Machine Learning .NET Community Standup](https://dotnet.microsoft.com/live/community-standup) every other Wednesday at 10am Pacific Time.
