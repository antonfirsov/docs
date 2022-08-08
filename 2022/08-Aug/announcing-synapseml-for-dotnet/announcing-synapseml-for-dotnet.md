---
post_title: Announcing SynapseML for .NET - Large Scale ML with a Simple API
author1: serenaruan
author2: marhamil
post_slug: announcing-synapseml-for-dotnet
username: serenaruan
microsoft_alias: serenaruan
featured_image: synapseml_dotnet_blog_card.jpg
categories: AI Machine Learning
summary: Announcing the release of hundreds of new distributed machine learning models and capabilities for .NET as part of the SynapseML distributed ML library.
desired_publication_date: 2022-08-09
---

Today, we are excited to announce the release of a new set of .NET APIs for massively scalable machine learning as part of the v0.10 release of SynapseML. This allows you to author, train, and use any SynapseML model from C#, F#, or other languages in the .NET family with our .NET for Apache Spark language bindings. In particular, with SynapseML developers can build scalable and intelligent systems for solving challenges in domains such as:

| <!-- --> | <!-- -->           	|
|----------------------------	|--------------------------------------------	|
|[Deep Learning](https://microsoft.github.io/SynapseML/docs/features/onnx/about/)           	| [Model Interpretability](https://microsoft.github.io/SynapseML/docs/features/responsible_ai/Model%20Interpretation%20on%20Spark/) 	|
| [Computer vision](https://microsoft.github.io/SynapseML/docs/features/opencv/OpenCV%20-%20Pipeline%20Image%20Transformations/)            	| [Reinforcement learning and personalization](https://microsoft.github.io/SynapseML/docs/features/vw/Vowpal%20Wabbit%20-%20Overview/) 	|
| [Anomaly Detection](https://microsoft.github.io/SynapseML/docs/features/isolation_forest/IsolationForest%20-%20Multivariate%20Anomaly%20Detection/)          	| [Search and retrieval](https://microsoft.github.io/SynapseML/docs/features/other/AzureSearchIndex%20-%20Met%20Artworks/)                       	|
| [Form and face recognition](https://microsoft.github.io/SynapseML/docs/features/cognitive_services/CognitiveServices%20-%20Create%20a%20Multilingual%20Search%20Engine%20from%20Forms/)  	| [Speech processing](https://microsoft.github.io/SynapseML/docs/features/cognitive_services/CognitiveServices%20-%20Overview/#speech-to-text-sample)                          	|
| [Gradient boosting](https://microsoft.github.io/SynapseML/docs/features/lightgbm/about/)          	| [Text analytics](https://microsoft.github.io/SynapseML/docs/features/cognitive_services/CognitiveServices%20-%20Overview/#text-analytics-sample)                             	|
| [Microservice orchestration](https://microsoft.github.io/SynapseML/docs/features/cognitive_services/CognitiveServices%20-%20Overview/#arbitrary-web-apis) 	| [Translation](https://microsoft.github.io/SynapseML/docs/features/cognitive_services/CognitiveServices%20-%20Create%20a%20Multilingual%20Search%20Engine%20from%20Forms/)                                	|

In this article first we'll dive deep into the SynapseML library and the Apache Spark distributed computing framework. Next, we'll explore how to get started using SynapseML from .NET. Finally, we'll describe our automated code generation system for mapping Scala-based Apache Spark APIs into .NET so that the .NET ecosystem has parity with the official Apache Spark APIs.

## Distributed Machine Learning with SynapseML

![SynapseML](https://mmlspark.blob.core.windows.net/graphics/dotnetblog/synapseml_overview.svg)

Writing fault-tolerant distributed programs is a complex and error-prone process. For example, consider the distributed evaluation of a deep network. The first step is to send a multi-GB model to hundreds of worker machines without overwhelming the network. Then, data readers must coordinate to ensure that all data is queued for processing and that GPUs are at full capacity. If new computers join or leave the cluster, new worker machines must receive copies of the model, and data readers need to adapt to share work with the new machines and re-compute lost work. Finally, progress must be tracked to ensure resources are properly freed.

Frameworks like Horovod can manage this, but if a teammate wants to compare against a different ML framework, such as LightGBM, XGBoost, or SparkML, it requires a new environment and cluster. Moreover, these training systems aren’t designed to serve or deploy models, so separate inference and streaming architectures are required.

SynapseML simplifies this experience by unifying many different ML learning frameworks with a single API that is scalable, data- and language-agnostic, and that works for batch, streaming, and serving applications. It’s designed to help developers focus on the high-level structure of their data and tasks, not the implementation details and idiosyncrasies of different ML ecosystems and databases.

SynapseML's unified API standardizes many of today’s tools, frameworks, and algorithms, streamlining the distributed ML experience available across many common programming languages. This enables developers to quickly compose disparate ML frameworks for use cases that require more than one framework, such as web-supervised learning, search engine creation, and many others. It can also train and evaluate models on single-node, multi-node, and elastically resizable clusters of computers, so developers can scale up their work without wasting resources.

The SynapseML API is available in several different programming languages, and its API abstracts over a wide variety of databases, file systems, and cloud data stores to simplify experiments no matter where data is located.

SynapseML is built on the [Apache Spark](https://spark.apache.org/) distributed computing framework and its .NET bindings are built on top of the [.NET for Apache Spark project](https://dotnet.microsoft.com/apps/data/spark). For detailed information on the SynapseML .NET APIs please see our [.NET API documentation](https://mmlspark.blob.core.windows.net/docs/0.10.0/dotnet/namespaces.html).



## Getting Started in .NET

SynapseML's .NET bindings are made available in a custom NuGet feed. To access this feed, add the feed as a nuget source by running the following dotnet CLI commands in the terminal:

```powershell
dotnet nuget add source https://mmlspark.blob.core.windows.net/synapsemlnuget/index.json -n SynapseMLFeed
```

SynapseML's .NET bindings are split into several sub-projects. To install all of the subprojects, run the following lines:

```powershell
dotnet add package SynapseML.Core --version 0.10.0
dotnet add package SynapseML.Cognitive --version 0.10.0
dotnet add package SynapseML.DeepLearning --version 0.10.0
dotnet add package SynapseML.Lightgbm --version 0.10.0
dotnet add package SynapseML.Opencv --version 0.10.0
dotnet add package SynapseML.Vw --version 0.10.0
```

>**Note**: SynapseML v0.10.0 depends on Microsoft.Spark v2.1.1.
>SynapseML v0.10.0 targets framework netstandard2.1.

Afterwards, you can use SynapseML's APIs for distributed machine learning on Spark via .NET.


## A .NET example using LightGBMClassifier in SynapseML

>**Note**: Follow our [installation guide](https://microsoft.github.io/SynapseML/docs/reference/dotnet-setup/#installation) to install prerequisites.

To create your first SynapseML .NET application we will create a console app with the following command:

```powershell
dotnet new console -o SynapseMLApp
cd SynapseMLApp
```

Next, we install the nuget packages required for this demo by running following command:

```powershell
dotnet add package Microsoft.Spark --version 2.1.1
dotnet add package SynapseML.Lightgbm --version 0.10.0
dotnet add package SynapseML.Core --version 0.10.0
```

>**Note**: This example uses Microsoft.Spark 2.1.1.
Please add corresponding SynapseML subpackages for different APIs.


With SynapseML installed we can now build our application. Next, update your console application with the code below:

```csharp
using Synapse.ML.Lightgbm;
using Synapse.ML.Featurize;
using Microsoft.Spark.Sql;
using Microsoft.Spark.Sql.Types;

// Create Spark session
SparkSession spark = SparkSession
    .Builder()
    .AppName("LightGBMExample")
    .GetOrCreate();

// Load Data
DataFrame df = spark.Read()
    .Option("inferSchema", true)
    .Parquet("wasbs://publicwasb@mmlspark.blob.core.windows.net/AdultCensusIncome.parquet")
    .Limit(2000);

var featureColumns = new string[] {"age", "workclass", "fnlwgt", "education", "education-num",
    "marital-status", "occupation", "relationship", "race", "sex", "capital-gain",
    "capital-loss", "hours-per-week", "native-country"};

// Transform features
var featurize = new Featurize()
    .SetOutputCol("features")
    .SetInputCols(featureColumns)
    .SetOneHotEncodeCategoricals(true)
    .SetNumFeatures(14);

var featurizedDf = featurize
    .Fit(df)
    .Transform(df)
    .WithColumn("label", Functions.When(Functions.Col("income").Contains("<"), 0.0).Otherwise(1.0));

DataFrame[] dfs = featurizedDf.RandomSplit(new double[] {0.75, 0.25}, 123);
var trainDf = dfs[0];
var testDf = dfs[1];

// Create LightGBMClassifier
var lightGBMClassifier = new LightGBMClassifier()
    .SetFeaturesCol("features")
    .SetRawPredictionCol("rawPrediction")
    .SetObjective("binary")
    .SetNumLeaves(30)
    .SetNumIterations(200)
    .SetLabelCol("label")
    .SetLeafPredictionCol("leafPrediction")
    .SetFeaturesShapCol("featuresShap");

// Fit the model
var lightGBMClassificationModel = lightGBMClassifier.Fit(trainDf);

// Apply transformation and displayresults
lightGBMClassificationModel.Transform(testDf).Show(50);

// Stop Spark session
spark.Stop();
```

Run `dotnet build` to build the project. Then navigate to build output directory, and run following command:

```powershell
spark-submit --class org.apache.spark.deploy.dotnet.DotnetRunner --packages com.microsoft.azure:synapseml_2.12:0.10.0,org.apache.hadoop:hadoop-azure:3.3.1 --master local microsoft-spark-3-2_2.12-2.1.1.jar dotnet SynapseMLApp.dll
```

> **Note**: Here we added two packages, `synapseml_2.12` for SynapseML's scala source, and `hadoop-azure` for supporting reading files from adls.

Once the program terminates, you will see the following expected output:

```text
+---+---------+------+-------------+-------------+--------------+------------------+---------------+-------------------+-------+------------+------------+--------------+--------------+------+--------------------+-----+--------------------+--------------------+----------+--------------------+--------------------+
|age|workclass|fnlwgt|    education|education-num|marital-status|        occupation|   relationship|               race|    sex|capital-gain|capital-loss|hours-per-week|native-country|income|            features|label|       rawPrediction|         probability|prediction|      leafPrediction|        featuresShap|
+---+---------+------+-------------+-------------+--------------+------------------+---------------+-------------------+-------+------------+------------+--------------+--------------+------+--------------------+-----+--------------------+--------------------+----------+--------------------+--------------------+
| 17|        ?|634226|         10th|            6| Never-married|                 ?|      Own-child|              White| Female|           0|           0|          17.0| United-States| <=50K|(61,[7,9,11,15,20...|  0.0|[9.37122343731523...|[0.99991486808581...|       0.0|[0.0,0.0,0.0,0.0,...|[-0.0560742274706...|
| 17|  Private| 73145|          9th|            5| Never-married|      Craft-repair|      Own-child|              White| Female|           0|           0|          16.0| United-States| <=50K|(61,[7,9,11,15,17...|  0.0|[12.7512760001880...|[0.99999710138899...|       0.0|[0.0,0.0,0.0,0.0,...|[-0.1657810433238...|
| 17|  Private|150106|         10th|            6| Never-married|             Sales|      Own-child|              White| Female|           0|           0|          20.0| United-States| <=50K|(61,[5,9,11,15,17...|  0.0|[12.7676985938038...|[0.99999714860282...|       0.0|[0.0,0.0,0.0,0.0,...|[-0.1276877355292...|
| 17|  Private|151141|         11th|            7| Never-married| Handlers-cleaners|      Own-child|              White|   Male|           0|           0|          15.0| United-States| <=50K|(61,[8,9,11,15,17...|  0.0|[12.1656242513070...|[0.99999479363924...|       0.0|[0.0,0.0,0.0,0.0,...|[-0.1279828578119...|
| 17|  Private|327127|         11th|            7| Never-married|  Transport-moving|      Own-child|              White|   Male|           0|           0|          20.0| United-States| <=50K|(61,[1,9,11,15,17...|  0.0|[12.9962776686392...|[0.99999773124636...|       0.0|[0.0,0.0,0.0,0.0,...|[-0.1164691543415...|
| 18|        ?|171088| Some-college|           10| Never-married|                 ?|      Own-child|              White| Female|           0|           0|          40.0| United-States| <=50K|(61,[7,9,11,15,20...|  0.0|[12.9400428266629...|[0.99999760000817...|       0.0|[0.0,0.0,0.0,0.0,...|[-0.1554829578661...|
| 18|  Private|115839|         12th|            8| Never-married|      Adm-clerical|  Not-in-family|              White| Female|           0|           0|          30.0| United-States| <=50K|(61,[0,9,11,15,17...|  0.0|[11.8393032168619...|[0.99999278472630...|       0.0|[0.0,0.0,0.0,0.0,...|[0.44080835709189...|
| 18|  Private|133055|      HS-grad|            9| Never-married|     Other-service|      Own-child|              White| Female|           0|           0|          30.0| United-States| <=50K|(61,[3,9,11,15,17...|  0.0|[11.5747235180479...|[0.99999059936124...|       0.0|[0.0,0.0,0.0,0.0,...|[-0.1415862541824...|
| 18|  Private|169745|      7th-8th|            4| Never-married|     Other-service|      Own-child|              White| Female|           0|           0|          40.0| United-States| <=50K|(61,[3,9,11,15,17...|  0.0|[11.8316427733613...|[0.99999272924226...|       0.0|[0.0,0.0,0.0,0.0,...|[-0.1527378526573...|
| 18|  Private|177648|      HS-grad|            9| Never-married|             Sales|      Own-child|              White| Female|           0|           0|          25.0| United-States| <=50K|(61,[5,9,11,15,17...|  0.0|[10.0820248199174...|[0.99995817710510...|       0.0|[0.0,0.0,0.0,0.0,...|[-0.1151843103241...|
| 18|  Private|188241|         11th|            7| Never-married|     Other-service|      Own-child|              White|   Male|           0|           0|          16.0| United-States| <=50K|(61,[3,9,11,15,17...|  0.0|[10.4049945509280...|[0.99996972005153...|       0.0|[0.0,0.0,0.0,0.0,...|[-0.1356854966291...|
| 18|  Private|200603|      HS-grad|            9| Never-married|      Adm-clerical| Other-relative|              White| Female|           0|           0|          30.0| United-States| <=50K|(61,[0,9,11,15,17...|  0.0|[12.1354343020828...|[0.99999463406365...|       0.0|[0.0,0.0,0.0,0.0,...|[0.53241098695335...|
| 18|  Private|210026|         10th|            6| Never-married|     Other-service| Other-relative|              White| Female|           0|           0|          40.0| United-States| <=50K|(61,[3,9,11,15,17...|  0.0|[12.3692360082180...|[0.99999575275599...|       0.0|[0.0,0.0,0.0,0.0,...|[-0.1275208795564...|
| 18|  Private|447882| Some-college|           10| Never-married|      Adm-clerical|  Not-in-family|              White| Female|           0|           0|          20.0| United-States| <=50K|(61,[0,9,11,15,17...|  0.0|[10.2514945786032...|[0.99996469655062...|       0.0|[0.0,0.0,0.0,0.0,...|[0.36497782752201...|
| 19|        ?|242001| Some-college|           10| Never-married|                 ?|      Own-child|              White| Female|           0|           0|          40.0| United-States| <=50K|(61,[7,9,11,15,20...|  0.0|[13.9439986622060...|[0.99999912057674...|       0.0|[0.0,0.0,0.0,0.0,...|[-0.1265631737386...|
| 19|  Private| 63814| Some-college|           10| Never-married|      Adm-clerical|  Not-in-family|              White| Female|           0|           0|          18.0| United-States| <=50K|(61,[0,9,11,15,17...|  0.0|[10.2057742895673...|[0.99996304506073...|       0.0|[0.0,0.0,0.0,0.0,...|[0.77645146059597...|
| 19|  Private| 83930|      HS-grad|            9| Never-married|     Other-service|      Own-child|              White| Female|           0|           0|          20.0| United-States| <=50K|(61,[3,9,11,15,17...|  0.0|[10.4771335467356...|[0.99997182742919...|       0.0|[0.0,0.0,0.0,0.0,...|[-0.1625827100973...|
| 19|  Private| 86150|         11th|            7| Never-married|             Sales|      Own-child| Asian-Pac-Islander| Female|           0|           0|          19.0|   Philippines| <=50K|(61,[5,9,14,15,17...|  0.0|[12.0241839747799...|[0.99999400263272...|       0.0|[0.0,0.0,0.0,0.0,...|[-0.1532111483051...|
| 19|  Private|189574|      HS-grad|            9| Never-married|     Other-service|  Not-in-family|              White| Female|           0|           0|          30.0| United-States| <=50K|(61,[3,9,11,15,17...|  0.0|[9.53742673004733...|[0.99992790305091...|       0.0|[0.0,0.0,0.0,0.0,...|[-0.0988907054317...|
| 19|  Private|219742| Some-college|           10| Never-married|     Other-service|      Own-child|              White| Female|           0|           0|          15.0| United-States| <=50K|(61,[3,9,11,15,17...|  0.0|[12.8625329757574...|[0.99999740658642...|       0.0|[0.0,0.0,0.0,0.0,...|[-0.1922327651359...|
+---+---------+------+-------------+-------------+--------------+------------------+---------------+-------------------+-------+------------+------------+--------------+--------------+------+--------------------+-----+--------------------+--------------------+----------+--------------------+--------------------+
```

And just like that you have trained a distributed LightGBM model using the SynapseML .NET bindings! For more usage examples, please see our [.NET setup guide with a cognitive service example](https://microsoft.github.io/SynapseML/docs/reference/dotnet-setup/). Furthermore you can check out our [.NET API docs](https://mmlspark.blob.core.windows.net/docs/0.10.0/dotnet/annotated.html) for a full list of machine learning classes and functionalities available.

## Automating the .NET for Apache Spark Integration

In the past, building language bindings for Apache Spark models has been a manual and labor-intensive process. Surprisingly PySpark, SparklyR, and .NET for Apache Spark hand-write their SparkML APIs. We here at SynapseML are way too lazy to put in this kind of legwork, and nobody wants to be saddled with that maintenance burden. To allow the team to sit back and drink margaritas all day, we have created an automated SparkML binding generation system. This system automatically translates SparkML and SynapseML APIs into Python, R, and .NET.

Soon, we hope to contribute this autogeneration back to the [.NET for Apache Spark repo](https://github.com/dotnet/spark/tree/main/src/csharp/Microsoft.Spark/ML/Feature) so that all of the remaining SparkML classes are supported in the core library. To this end, we have redesigned the fundamental classes used in ML area, including `Params`, `PipelineStage`, `Transformer`, `Estimator`, `Model`, and `Evaluator` to better align with Scala's class hierarchy.

Our code generation system can also be used from within your own Scala projects by extending the `DotnetWrappable` trait in your Scala SparkML model. Calling the inherited method`makeDotnetFile` with autogenerate a Spark.NET class for you. Examples of our [generated SparkML classes](https://github.com/microsoft/SynapseML/tree/master/core/src/main/dotnet/src/org/apache/spark/ml) can be found on GitHub, and keep your eyes open for more integrations between the SynapseML and Spark.NET ecosystem in the months to come!

## Conclusion

Today we announced the release of hundreds of new distributed machine learning models and capabilities for the C# and F# languages as part of the SynapseML distributed ML library. These announcements allow .NET developers to train, run, explain, and evaluate deep learning models, intelligent services, gradient boosted trees, reinforcement learning systems and many other capabilities while enjoying the .NET language and framework. Furthermore, this integration is completely automated, so every update to the SynapseML library will bring more models and capabilities to the .NET ecosystem without costing developers weeks to implement. We encourage the .NET community to give the APIs a try in their next application, and to  feel empowered to reach out to the SynapseML team to let us know how you like them!

## Acknowledgements

Huge thanks to the amazing folks on the .NET for Apache Spark team who made this work possible. Cheers to Niharika Dutta, Andrew Fogarty, Mark Niehaus, Steve Suh, and Tom Finley for their comments, PR feedback, and help teaching us the ways of the .NET developer. 

## Learn More

* [Full installation guidance](https://microsoft.github.io/SynapseML/docs/reference/dotnet-setup/#installation)
* [SynapseML Homepage](https://microsoft.github.io/SynapseML/)
* [SynapseML GitHub](https://github.com/microsoft/SynapseML)
* [SynapseML v0.10.0 Release](https://github.com/microsoft/SynapseML/releases/tag/v0.10.0)
* [Spark.NET on Github](https://github.com/dotnet/spark)
