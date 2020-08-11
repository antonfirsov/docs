# August ML.NET API and Tooling Updates 

We recently released ML.NET 1.5 and 1.5.1 as well as new versions of Model Builder and the ML.NET CLI. These releases include numerous bug fixes and enhancements, as well as a new features for anomaly detection and time series data, improvements to the TextLoader, local GPU training for image classification in Model Builder, and more.

In this post
1.	New algorithms and features for anomaly detection and time series data
2.	AutoML for ranking scenario
3.	Updates to TextLoader
4.	Model Builder local GPU training for image classification
5.	Feedback button in Model Builder
6.	Thanks to our contributors
7.	Feedback
8.	Get started & Resources

## What’s new with the ML.NET API?

### New algorithms and features for anomaly detection and time series data
The 1.5 update added a new anomaly detection algorithm called [DetectEntireAnomalyBySrCnn](https://docs.microsoft.com/dotnet/api/microsoft.ml.timeseriescatalog.detectentireanomalybysrcnn?view=ml-dotnet) which allows you to detect anomalies for an entire dataset at once; this is in contrast to the existing [DetectAnomalyBySrCnn](https://docs.microsoft.com/dotnet/api/microsoft.ml.timeseriescatalog.detectanomalybysrcnn?view=ml-dotnet) algorithm, which streams parts of the dataset at once and examines a window around points to find anomalies.

This new algorithm is faster than the DetectAnomalyBySrCnn algorithm and can work on arbitrarily-sized datasets since it can train on batches of a fixed size, but it also consumes more memory since it loads the entire dataset in memory. You can use the new DetectEntireAnomalyBySrCnn algorithm if you have all your data on hand. However, if your time series data is streaming, you don’t have all your data on hand, or your data is too large to fit in memory, you can still use the previous DetectAnomalyBySrCnn algorithm. Here is an example using the new DetectEntireAnomalyBySrCnn algorithm.

This update also added [root cause detection](https://github.com/dotnet/machinelearning/blob/master/src/Microsoft.ML.TimeSeries/RootCauseAnalyzer.cs), which is an explainability feature that identifies which inputs caused an anomaly. For example, say you have housing data for Seattle, and one of the house listings shows an abnormally high price (e.g. an anomaly) on August 6. Using root cause detection, you may find that the neighborhood and property type are the contributing factors to the abnormally high price. The 1.5.1 update also added the ability for you to define a threshold for root cause analysis which can influence which features are chosen as root causes. Here is an example of root cause analysis.

The 1.5.1 update also added new capabilities for working with time series data, including seasonality detection and being able to de-seasonalize seasonal data prior to anomaly detection. For example, say you had sales data from the past 5 years, and you noticed that sales always go up in the holiday months. Normally, this spike in sales would be counted as an anomaly, but now you can use ML.NET’s seasonality detection feature to identify this yearly occurrence and normalize the data against the seasonality before your anomaly detection analysis so that it does not show up as an anomaly.

### AutoML for ranking scenario
While ML.NET has supported the [ranking scenario](https://github.com/dotnet/machinelearning-samples/tree/master/samples/csharp/getting-started/Ranking_Web) for a while, it is now also supported by local AutoML. This means that you don’t have to worry about selecting an algorithm or manually tuning algorithm settings; instead, you can simply choose the ranking scenario and input your data, and AutoML will give you the best model based on your inputs.
Currently, you can use the AutoML.NET API for this ranking scenario, but we are working on adding AutoML ranking to our tooling (Model Builder in Visual Studio and the ML.NET CLI) as well.

### Updates to TextLoader
The 1.5 update also improved the TextLoader experience, which includes adding the following features:
-  Enabling the TextLoader to accept new lines in quoted fields.
-  Adding escapeChar support.
-  Adding public generic methods to the TextLoader catalog that accept Options objects.
-  Adding decimal marker option in the TextLoader.

You can see more changes to ML.NET in the [1.5](https://github.com/dotnet/machinelearning/blob/master/docs/release-notes/1.5.0/release-1.5.0.md) and [1.5.1](https://github.com/dotnet/machinelearning/blob/master/docs/release-notes/1.5.1/release-1.5.1.md) release notes.

## What’s new with ML.NET tooling?

### Model Builder local GPU training for image classification
You can now utilize your local GPU for faster Image Classification training via Model Builder in Visual Studio.

We tested local training with a dataset of ~77K images; comparing CPU with GPU, we got the following results:
-  CPU Training: 4h 19m 10s
-  GPU Training: 38m 57s

When you open Model Builder and select the Image Classification scenario, you will now see a 3rd option for Local GPU training (in addition to Local CPU training and Azure training).

After selecting Local (GPU) as your training environment, you can check to see if your machine is compatible for GPU training right in the Model Builder UI.

![GPU compatability requirements](gpu-compatability.png)
 
Compatibility requirements include:
1.	Installing the ML.NET Model Builder GPU Support extension in the Visual Studio Marketplace or in the Extensions Manager in VS.
2.	A CUDA-compatible GPU.
3.	Installing CUDA v10.0 (make sure you get v.10.0, and not any newer version – you can’t have multiple version of CUDA installed).
4.	Installing cuDNN v7.6.4 for CUDA 10.0 (you can’t have multiple versions of cuDNN installed).
Currently, Model Builder can check that you have a CUDA-compatible GPU as well as make sure you have the GPU extension installed, but it can’t yet check that you have the correct versions of CUDA and cuDNN (we are working to add this compatibility check in a future release).

Don’t have a CUDA-compatible GPU but still want faster training? You can train in Azure, either by selecting the Azure training environment in Model Builder to utilize Azure ML or by creating an Azure VM with GPU and using Model Builder’s local GPU option for training.

You can read more about how to set up GPU training in the [ML.NET Docs](https://aka.ms/vsmbgpu).

### Feedback button in Model Builder
It’s now even easier to open GitHub issues for Model Builder. We’ve added a Feedback button so that you can start to file bugs or suggest features from the UI in Visual Studio.

Selecting “Report a bug” or “Suggest a feature” will open up GitHub in your browser with the corresponding template to fill out.

![Model Builder feedback button](mb-feedback.png)

## Thanks to our contributors
For these updates, we had help from some other teams at Microsoft!

Thanks to [Klaus Marius Hansen](https://github.com/klausmh) and [Lisa Hua](https://github.com/lisahua) from the PowerBI team and [Shakira Sun](https://github.com/suxi-ms) and [Meng Ai](https://github.com/mengaims) from the Kensho team for all of your contributions!

## Feedback
We would love to hear your feedback!

If you run into any issues, please let us know by creating an issue in our GitHub repos (or use the new Feedback button in Model Builder!):
-  ML.NET API:
http://www.github.com/dotnet/machinelearning 
-  ML.NET Tooling (Model Builder & ML.NET CLI): http://www.github.com/dotnet/machinelearning-modelbuilder

## Get Started & Resources
Get started with ML.NET in this [tutorial](https://dotnet.microsoft.com/learn/ml-dotnet/get-started-tutorial/intro).

Learn more about ML.NET and Model Builder in [Microsoft Docs](https://aka.ms/mlnet-docs).

