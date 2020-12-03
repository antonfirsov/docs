---
post_title: 'Announcing Version 1.0 of .NET for Apache Spark'
username: jeremy-likness
featured_image: dotnet-bot.png
categories: .NET, .NET Core, Spark for .NET, Big Data
summary: Announcing the release of version 1.0 of .NET for Apache&reg; Spark&trade;, an open source package that brings .NET development to the Apache&reg; Spark&trade; platform.
---
Today, we [announce the release](#main-url-tbd) of version 1.0 of [.NET for Apache&reg; Spark&trade;](https://dot.net/spark), an open source package that brings .NET development to
the [Apache&reg; Spark&trade;](https://spark.apache.org/) platform. This release is possible due to the combined efforts of Microsoft and the open source community.
Version 1.0 includes support for .NET applications targeting [.NET Standard 2.0](https://docs.microsoft.com/dotnet/standard/whats-new/whats-new-in-dotnet-standard#whats-new-in-net-standard-20)
or later. Access to the Apache&reg; Spark&trade; DataFrame APIs (versions 2.3, 2.4 and 3.0) and the ability to write Spark SQL and create user-defined functions
(UDFs) are also included in the release.

![The .NET Bot](./dotnet-bot.png)

The following code snippet is an example of using Spark to produce a word count from a document (browse the full sample [here](https://github.com/JeremyLikness/SparkMLDocCategorization/tree/master/SparkWordsProcessor)):

```csharp
var docs = spark.Read().Option("header", true).Csv("documents.csv");
var filCol = Functions.Col("file");
var words = docs
    .Select(
        fileCol,
        // "a b c" => ["a", "b", "c"]
        Functions.Split(
            Functions.Col("words"), " ")
        .Alias("wordList"))
    // flatten into one row per word
    .Select(
        fileCol,
        // 1: ["a", "b", "c"] => 1: "a", 2: "b", 3: "c"
        Functions.Explode(
            Functions.Col("wordList"))
        .Alias("word"))
    .GroupBy(fileCol, Functions.Lower(Functions.Col("word")))
    .Count();
```

## Background

.NET for Apache&reg; Spark&trade;  launched two years ago to address increasing demand from the .NET community for an easier way to build big data applications. A recent survey confirmed the biggest motivation to use the package is to take advantage of existing .NET development skills and resources, including the enormous .NET ecosystem of existing libraries and frameworks. The team is committed to the continuous evolution of the product to integrate the latest features and keep the API current with the latest Spark versions. For more about the history of the project and key contributors, read the [full announcement](#main-url-tbd).

## Get Started

There are several options to get started. First, read the full [.NET for Apache Spark 1.0 announcement](#main-url-tbd). Then you can:

- Browse our online [.NET for Apache Spark documentation](https://docs.microsoft.com/dotnet/spark/what-is-apache-spark-dotnet)
- Take the tutorial: [Get started with .NET for Apache Spark](https://docs.microsoft.com/dotnet/spark/tutorials/get-started)
- Submit jobs to run on Azure and analyze data in real-time notebooks using [.NET for Apache Spark with Azure Synapse Analytics](https://docs.microsoft.com/azure/synapse-analytics/spark/spark-dotnet)
- Visit and consider contributing to our [open source repository](https://github.com/dotnet/spark)