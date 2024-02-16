---
post_title: Our Vision for .NET 9
author1: dotnet
post_slug: our-vision-for-dotnet-9
microsoft_alias: rlander
featured_image: our-vision-for-dotnet-9.png
categories: .NET
tags: .net 9, featured-preview
summary: "Welcome to .NET 9! Learn about how we're improving .NET for all kinds of apps, with a special focus on cloud native, AI, and performance."
post_date: 2024-02-13 10:06:00
---

Welcome to .NET 9! We're at the beginning of another annual release cycle, following the successful launch of .NET 8 a few months back. We recommend that developers transition their apps to [.NET 8](https://devblogs.microsoft.com/dotnet/announcing-dotnet-8/). In this post, we'll share our initial vision for .NET 9, set to be released at .NET Conf 2024 at the end of the year. Our most important focus areas are cloud-native and intelligent app development. You can expect significant investments in performance, productivity, and security, as well as advancements across the platform.

Today, let's take a look at the .NET 9 focus areas and complementary integrations we plan to deliver in collaboration with partner teams at Microsoft. Our goal is to make .NET development more productive using Visual Studio, Visual Studio Code with the C# Dev Kit, and cloud deployments easier using Azure services. We'll continue to work closely with our industry partners, like Canonical and Red Hat, to ensure that .NET works great wherever you use it.

.NET 9 is shaping up to be another major step forward for the platform. We're delivering [.NET 9 Preview 1](https://aka.ms/dotnet/9/preview1) today and welcome your feedback on [all the new features we've delivered](https://learn.microsoft.com/dotnet/core/whats-new/dotnet-9/overview).

## Platform for Cloud-Native Developers

We've spent the last several years building out [strong cloud native fundamentals](https://devblogs.microsoft.com/dotnet/category/containers/), like  [runtime performance](https://devblogs.microsoft.com/dotnet/category/performance/) and [application monitoring](https://learn.microsoft.com/dotnet/core/diagnostics/). We will continue that effort. We're also turning our focus to delivering paved paths to popular production infrastructure and services, for example running in Kubernetes and using managed database and caching services like Redis. We will deliver those improvements at multiple layers of the .NET stack. Those capabilities all come together with [.NET Aspire](https://devblogs.microsoft.com/dotnet/category/dotnet-aspire/), which significantly reduces the cost and complexity of building cloud applications and the distance between development and production.

We've been developing Native AOT and application [trimming](https://devblogs.microsoft.com/dotnet/creating-aot-compatible-libraries/) as key tools to optimize production apps. In .NET 8, we optimized Web API applications (using the `webapiaot` template) for both trimming and AOT. In .NET 9, we are working on doing the same with other application types and improving the [DATAS GC](https://maoni0.medium.com/dynamically-adapting-to-application-sizes-2d72fcb6f1ea) for all ASP.NET Core applications.

Our Azure Container Apps partners will ensure that .NET 9 apps can be scaled to multiple instances easily within their Kubernetes-based environment. We're working with them to ensure that ephemeral data – like anti-forgery and auth tokens - are encrypted correctly using [Data Protection](https://learn.microsoft.com/aspnet/core/security/data-protection/introduction) and that rate limiting APIs are improved to ensure optimal behavior for and across each node.

The [eShop](https://github.com/dotnet/eshop) reference architecture sample app that was showcased at .NET Conf last year will be updated to take advantage of these new capabilities and deployment options as .NET 9 evolves throughout the year.

## Tools for Cloud-Native Developers

Our Visual Studio partners plan improvements that support and augment our cloud platform, Native AOT, .NET Aspire, and Azure deployment.

Native AOT code compilation requires installing and using tools that many .NET developers do not commonly use. Developers who want to cross-compile (for example, target Linux on Windows) currently rely on Docker and/or WSL2, as guided by our [documentation](https://learn.microsoft.com/dotnet/core/deploying/native-aot/cross-compile) and [samples](https://github.com/dotnet/dotnet-docker/blob/main/samples/releasesapi/README.md). Visual Studio support for AOT will expand to make Native AOT accessible to many more developers.

Visual Studio and Visual Studio Code will include new development and deployment experiences for .NET Aspire. This will include configuring components, debugging (including hot-reload) the AppHost and child processes, and fully integrating with the developer dashboard. Developers will be able to deploy their projects to Azure Container Apps, from Visual Studio, Visual Studio Code, and with Azure Developer CLI (azd).

## .NET and Artificial Intelligence

OpenAI has sparked excitement among developers by offering the opportunity to transform their applications with AI. Over the past year, Azure Open AI and .NET have been leveraged to create AI solutions, with Microsoft Copilot being the most popular. We will continue to work with customers looking for ways to use their C# skills to build this new class of apps, and to rapidly invest in our AI platform.

In .NET 8, we expanded our investment beyond ML.NET. We focused on AI workloads, invested in getting started [samples and documentation](https://learn.microsoft.com/collections/d2z1bmomeo55kr?source=learn), and collaborated with the AI ecosystem partners to deliver C# clients for vector databases like [Qdrant](https://github.com/qdrant/qdrant-dotnet) and [Milvus](https://milvus.io/docs/v2.2.x/install-csharp.md) and libraries like Semantic Kernel. Additionally, we added [TensorPrimitives for .NET](https://github.com/dotnet/runtime/issues/92219).
 
Looking ahead towards .NET 9, we are committed to making it even easier for .NET developers to integrate artificial intelligence into their existing and new applications. Developers will find great libraries and documentation for working with OpenAI and OSS models (hosted and local), and we’ll continue collaborating on Semantic Kernel, OpenAI, and Azure SDK to ensure that .NET developers have a first-class experience building intelligent applications.

We will be updating the [ChatGPT + Enterprise Data with Azure OpenAI and Cognitive Search .NET Sample](https://github.com/Azure-Samples/azure-search-openai-demo-csharp) on GitHub throughout the release.

## .NET 9 Backlog

These cloud-native and AI projects are just one part of what we'll deliver. Backlogs have been published for [.NET MAUI](https://github.com/dotnet/maui/wiki/Roadmap), [ASP.NET Core and Blazor](https://github.com/dotnet/aspnetcore/issues/51834), [C#](https://github.com/dotnet/roslyn/blob/main/docs/Language%20Feature%20Status.md#working-set), [F#](https://github.com/dotnet/core/pull/9198), and other runtime and tools components delivered in the .NET SDK. Check out the [.NET 9 Project backlog](https://github.com/dotnet/core/blob/main/roadmap.md) on GitHub for your favorite product areas and features.

We are regularly definining new features and updating progress. We'll update our backlog and the [.NET 9 release notes](https://github.com/dotnet/core/tree/main/release-notes/9.0) as we go. We also have some experiements that we're working on, which may become part of a future release.

## Try .NET 9 Preview 1

[.NET 9 Preview 1](https://aka.ms/dotnet/9/preview1) is now available for download. Going forward, we're going to publish [preview releases to GitHub Discussions](https://github.com/dotnet/core/discussions/9131). We'll tailor our .NET blog content to highlight the advantages of .NET 8, aiming to support your use of .NET 8 in production environments.

[.NET Aspire Preview 3](https://github.com/dotnet/aspire/discussions/2205) is also shipping today. This release includes UI improvements to the dashboard, and new component support including Azure OpenAI, Kafka. Oracle, MySQL, CosmosDB & Orleans.

If previews are not your thing, please take a look at the [.NET 8 release post](https://devblogs.microsoft.com/dotnet/announcing-dotnet-8/). We've heard a lot of good feedback about early .NET 8 deployments. .NET 9 should be a very easy migration from .NET 8 (and previous releases).

## Thank You

.NET is amazing because of all of you, the .NET community, who help drive .NET forward. We want to thank each and every person that has helped make this and every release fantastic by creating issues, commenting, contributing code, creating packages, joining live streams, and being active online and in their local regions.  In the [.NET 9 release notes](https://github.com/dotnet/core/tree/main/release-notes/9.0) you will find community member highlights for each release.
