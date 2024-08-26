---
post_title: Introducing the Pinecone .NET SDK
author1: luquinta@microsoft.com
post_slug: introducing-pinecone-dotnet-sdk
microsoft_alias: luquinta
featured_image: introducing-pinecone-dotnet-sdk.jpg
categories: .NET, AI
tags: ai,pinecone,vectordb
ai_note: hide
summary: Get started building AI applications in .NET using the Pinecone Vector DB and the Pinecone .NET SDK. 
post_date: 2024-08-27 13:05:00
---

The AI ecosystem in .NET is constantly growing. Today, we're excited to announce the newest member of the community: Pinecone.

In this post, we introduce the Pinecone .NET SDK and show how you can quickly get started building AI applications with it.

## What is Pinecone?

Pinecone is a robust vector database designed to efficiently handle and query large-scale vector data. With Pinecone, engineers and data scientists can effortlessly build vector-based AI applications that require efficient similarity search and ranking.

To learn more about Pinecone, visit their [website](https://www.pinecone.io/).  

## What is a vector database?

Building AI applications requires efficient vector data processing.  

A vector database indexes and stores embedding vectors for fast retrieval and similarity search.

Embeddings are numerical representations of data such as text, images, and audio. They capture semantic meaning and relationships, making them essential in AI applications.

Due to the complexity of vector embeddings, you need a database designed specifically for handling this data type.

To learn more, see the [Embeddings in .NET](https://learn.microsoft.com/dotnet/ai/conceptual/embeddings) and [Vector Databases for .NET](https://learn.microsoft.com/dotnet/ai/conceptual/vector-databases) documentation.

## Get started with the Pinecone .NET SDK

Getting started with Pinecone in .NET is easy:

1. Set up your Pinecone account and database, if you haven’t already, and create an API key. For more details, see the [documentation](https://docs.pinecone.io/guides/get-started/quickstart).
1. Download the [Pinecone .NET SDK](https://aka.ms/pinecone-dotnet-sdk) from NuGet.
1. After downloading the SDK, connect the .NET client to your Pinecone database:

    ```csharp
    using Pinecone;

    var pinecone = new PineconeClient("PINECONE_API_KEY");
    ```

## Create an index

An index is the high-level structure that stores vector data in Pinecone.

An index serves queries over vectors it contains; and does other vector operations over its contents. For more details, wee the [Pinecone index documentation](https://docs.pinecone.io/guides/indexes/understanding-indexes).  

```csharp
var createIndexResponse = await pinecone.CreateIndexAsync(new CreateIndexRequest
 {
 	Name = “example_index”indexName,
 	Dimension = 3,
 	Metric = CreateIndexRequestMetric.Cosine,
 	Spec = new ServerlessIndexSpec
 	{
 	Serverless = new ServerlessSpec
 	{
     	Cloud = ServerlessSpecCloud.Azure,
     	Region = "eastus2"
 	}
 	}
 });
```

## Add records

To start adding records your data store:

```csharp
var upsertResponse = await index.UpsertAsync(new UpsertRequest {
 	Vectors = new[]
 	{
     	new Vector
     	{
         	Id = "v1",
         	Values = new[] { 0.1f, 0.2f, 0.3f }
     	},
     	new Vector
     	{
         	Id = "v2",
         	Values = new[] { 0.4f, 0.5f, 0.6f }
     	},
     	new Vector
     	{
         	Id = "v3",
         	Values = new[] { 0.7f, 0.8f, 0.9f }
     	}
 	}
 });

await Task.Delay(10000);
```

## Query records

Once you have records in your data store, you can query them:

```csharp
var queryResponse = await index.QueryAsync(
   new QueryRequest
   {
       Id = "v1",
       TopK = 1,
       IncludeValues = true,
   });
```

## Conclusion

We can't wait to see what you build. Try out the [Pinecone .NET SDK](https://aka.ms/pinecone-dotnet-sdk) and give us feedback!