---
post_title: Announcing .NET 8 RC2
author1: jondouglas
post_slug: announcing-dotnet-8-rc2
microsoft_alias: jodou
featured_image: dotnet-8-rc2.png
categories: .NET, .NET Core
tags: .net 8, featured-preview
summary: ".NET 8 RC2 is now available with new NuGet package READMEs for .NET packages, simple CLI-based project evaluation for MSBuild, publishing containers to tar.gz archives, and Tensor Primitives for .NET."
post_date: 2023-10-10 10:05:00
---

.NET 8 RC2 is now available. This is our last release candidate. This release includes new NuGet package READMEs for .NET packages, simple CLI-based project evaluation for MSBuild, publishing containers to tar.gz archives, and Tensor Primitives for .NET.

The dates for [.NET Conf 2023](https://dotnetconf.net/) have been announced! Join us November 14-16, 2023 to celebrate the .NET 8 release!

[Download .NET 8 RC2](https://dotnet.microsoft.com/download/dotnet/8.0) for Linux, macOS, and Windows.

* [Installers and binaries](https://dotnet.microsoft.com/download/dotnet/8.0)
* [Container images](https://hub.docker.com/_/microsoft-dotnet-aspnet/)
* [Release notes](https://github.com/dotnet/core/tree/main/release-notes/8.0)
* [Breaking changes](https://learn.microsoft.com/dotnet/core/compatibility/8.0?toc=%2Fdotnet%2Ffundamentals%2Ftoc.json&bc=%2Fdotnet%2Fbreadcrumb%2Ftoc.json)
* [Known issues](https://github.com/dotnet/core/blob/main/release-notes/8.0/known-issues.md)
* [GitHub issue tracker](https://github.com/dotnet/core/issues)

> [!IMPORTANT]
> .NET SDK 8.0.100-rc.2 _must_ be used with Visual Studio 17.8 Preview 3 due to a dependency error with Razor tooling. See [this SDK Announcement](https://github.com/dotnet/sdk/issues/35810) for more details.

There are several exciting posts you should check out as well:

- [ASP.NET Core Updates in .NET 8 RC2](https://devblogs.microsoft.com/dotnet/asp-net-core-updates-in-dotnet-8-rc-2)
- [.NET MAUI Updates in .NET 8 RC2](https://devblogs.microsoft.com/dotnet/announcing-dotnet-maui-in-dotnet-8-rc-2)
- [Visual Studio 2022 17.8 Preview 3](https://aka.ms/vs/v178P3)
- [Entity Framework Updates in .NET 8 RC2](https://devblogs.microsoft.com/dotnet/announcing-ef8-rc2)
- [What's New in .NET 8](https://learn.microsoft.com/dotnet/core/whats-new/dotnet-8) describes all the new features in .NET 8. For a broader view of the platform, read [Why .NET?](https://devblogs.microsoft.com/dotnet/why-dotnet/).

## New package READMEs for .NET Libraries

One of the top customer problems that package consumers face is lack of documentation. As such, we are driving an effort to increase the adoption and quality of NuGet package READMEs.

The README file is an essential part of your package as it provides important information to users and helps them understand what the package is and what it does quickly. Also, README is the first things for users when they view your package on NuGet.org and soon other tooling. It is crucial for package authors to write and include high-quality READMEs for their packages.

Now there are READMEs for the following Microsoft packages:

| Package | Status |
| :--- | :---: |
| Microsoft.Extensions.DependencyInjection | :white_check_mark: |
| Microsoft.Extensions.Logging | :white_check_mark:|
| Microsoft.Extensions.DependencyInjection.Abstractions | :white_check_mark: |
| Microsoft.Extensions.Hosting | :white_check_mark: |
| Microsoft.Extensions.Hosting.WindowsServices | :white_check_mark: |
| Microsoft.Extensions.Logging.Abstractions | :white_check_mark:|
| Microsoft.Extensions.Http | :white_check_mark: |
| System.IO.Ports | :white_check_mark: |
| System.Data.OleDb | :white_check_mark: |
| Microsoft.Extensions.Options | :white_check_mark:|
| System.Management | :white_check_mark: |
| Microsoft.Extensions.Options.ConfigurationExtensions |:white_check_mark:|
| Microsoft.Extensions.Caching.Memory | :white_check_mark: |
| Microsoft.Extensions.Logging.Console |:white_check_mark:|
| Microsoft.Extensions.Hosting.Abstractions | :white_check_mark: |
| System.Text.Encoding.CodePages |:white_check_mark: |
| Microsoft.Bcl.AsyncInterfaces | :white_check_mark: |
| System.DirectoryServices.AccountManagement | :white_check_mark: |
| System.Speech | :white_check_mark: |
| System.DirectoryServices | :white_check_mark: |
| Microsoft.Extensions.Logging.Debug |:white_check_mark: |
| System.Net.Http.Json | :white_check_mark: |
| System.Data.Odbc | :white_check_mark: |
| Microsoft.Extensions.Primitives |:white_check_mark: |
| Microsoft.Bcl.Numerics (new package) | :white_check_mark:  |
| Microsoft.Bcl.TimeProvider (new package) |:white_check_mark: |
| Microsoft.Extensions.Configuration |:white_check_mark: |
| Microsoft.Extensions.Configuration.Abstractions |:white_check_mark: |
| Microsoft.Extensions.Configuration.Binder |:white_check_mark: |
| Microsoft.Extensions.Logging.EventLog |:white_check_mark: |
| System.Diagnostics.EventLog |:white_check_mark: |
| System.Net.Http.WinHttpHandler |:white_check_mark: |
| System.Text.Json |:white_check_mark: |
| System.Threading.Channels |:white_check_mark: |

## MSBuild: Simple CLI-based project evaluation

* [Issue](https://github.com/dotnet/msbuild/issues/3911#issuecomment-1478468822)
* [PR](https://github.com/dotnet/msbuild/pull/9138)

MSBuild is a very powerful platform, and is great at integrating data and tools from other ecosystems into its view of the world.  It's historically not been so good at making the data that it has available to the broader worlds of scripting and tools.  In the past, tools authors would have to do things like injecting their own MSBuild code to read certain Properties, Items, or Target Outputs into files, and then parse those files. This is work that is very error prone. In .NET 8, the MSBuild team has shipped a new feature that makes it easier to incorporate data from MSBuild into your scripts or tools. Let's take a look at a sample to see what I mean.

```shell
>dotnet publish --getProperty:OutputPath
bin\Release\net8.0\
```

In this simple example, we've used the `--getProperty` flag to request the value of the `OutputPath` property after the `publish` command has run, and MSBuild has written that single property's value to the standard out of my terminal. This is very useful for tooling like CI pipelines - where before you might have hard-coded an output path, now you can be data-driven! Let's look at a more complex example, where we fetch multiple properties. In this example we're publishing a Container Image for a project using the .NET SDK, and we want to use some properties of the container that are generated during that process:

```shell
>dotnet publish -p PublishProfile=DefaultContainer --getProperty:GeneratedContainerDigest --getProperty:GeneratedContainerConfiguration
{
  "Properties": {
    "GeneratedContainerDigest": "sha256:ef880a503bbabcb84bbb6a1aa9b41b36dc1ba08352e7cd91c0993646675174c4",
    "GeneratedContainerConfiguration": "{\u0022config\u0022:{\u0022ExposedPorts\u0022:{\u00228080/tcp\u0022:{}},\u0022Labels\u0022:{\u0022org.opencontainers.image.created\u0022:\u00222023-10-02T18:20:01.6356453Z\u0022,\u0022org.opencontainers.artifact.created\u0022:\u00222023-10-02T18:20:01.6356453Z\u0022,\u0022org.opencontainers.artifact.description\u0022:\u0022A project that demonstrates publishing to various container registries using just\\r\\n      the .NET SDK\u0022,\u0022org.opencontainers.image.description\u0022:\u0022A project that demonstrates publishing to various container registries using just\\r\\n      the .NET SDK\u0022,\u0022org.opencontainers.image.authors\u0022:\u0022Chet Husk\u0022,\u0022org.opencontainers.image.url\u0022:\u0022https://github.com/baronfel/sdk-container-demo\u0022,\u0022org.opencontainers.image.documentation\u0022:\u0022https://github.com/baronfel/sdk-container-demo\u0022,\u0022org.opencontainers.image.version\u0022:\u00221.0.0\u0022,\u0022org.opencontainers.image.licenses\u0022:\u0022MIT\u0022,\u0022org.opencontainers.image.title\u0022:\u0022.NET SDK 7 Container Demo\u0022,\u0022org.opencontainers.image.base.name\u0022:\u0022mcr.microsoft.com/dotnet/aspnet:8.0.0-rc.1\u0022},\u0022Env\u0022:[\u0022PATH=/usr/local/sbin:/usr/local/bin:/usr/sbin:/usr/bin:/sbin:/bin\u0022,\u0022APP_UID=1654\u0022,\u0022ASPNETCORE_HTTP_PORTS=8080\u0022,\u0022DOTNET_RUNNING_IN_CONTAINER=true\u0022,\u0022DOTNET_VERSION=8.0.0-rc.1.23419.4\u0022,\u0022ASPNET_VERSION=8.0.0-rc.1.23421.29\u0022],\u0022WorkingDir\u0022:\u0022/app\u0022,\u0022Entrypoint\u0022:[\u0022dotnet\u0022,\u0022sdk-container-demo.dll\u0022],\u0022User\u0022:\u00221654\u0022},\u0022created\u0022:\u00222023-10-02T18:20:04.6914310Z\u0022,\u0022rootfs\u0022:{\u0022type\u0022:\u0022layers\u0022,\u0022diff_ids\u0022:[\u0022sha256:d310e774110ab038b30c6a5f7b7f7dd527dbe527854496bd30194b9ee6ea496e\u0022,\u0022sha256:379caff0dd639afb033e114cb8da17c334a36d0c6c01bb4cf5f2d1a811968742\u0022,\u0022sha256:80627b9413613b9703eec6adc7a3a751ac1b7571041c77899456345f823ef63a\u0022,\u0022sha256:6231bc2ccd45860760c09a50d6d059aa4b6aa357e41e6d06f4394f24176f203d\u0022,\u0022sha256:56fa0b94fd5e406124a3d070ec79998698ddea2e635bf53bbf106dc86aeaa240\u0022,\u0022sha256:272eedde5582036a7f26fe5d069f4ba328ba7a5c6be30f6dcbee9838224df148\u0022,\u0022sha256:4b8ab71658cccccfaf8979b1025f3ed1b12e936a448dcd13b9ab4f7709f31357\u0022]},\u0022architecture\u0022:\u0022amd64\u0022,\u0022os\u0022:\u0022linux\u0022,\u0022history\u0022:[{\u0022created\u0022:\u00222023-09-20T04:55:40.8154909Z\u0022,\u0022created_by\u0022:\u0022/bin/sh -c #(nop) ADD file:a1398394375faab8dd9e1e8d584eea96c750fb57ae4ffd2b14624f1cf263561b in / \u0022},{\u0022created\u0022:\u00222023-09-20T04:55:41.1203677Z\u0022,\u0022created_by\u0022:\u0022/bin/sh -c #(nop)  CMD [\\u0022bash\\u0022]\u0022,\u0022empty_layer\u0022:true},{\u0022comment\u0022:\u0022buildkit.dockerfile.v0\u0022,\u0022created\u0022:\u00222023-09-20T12:13:34.5265068Z\u0022,\u0022created_by\u0022:\u0022ENV APP_UID=1654 ASPNETCORE_HTTP_PORTS=8080 DOTNET_RUNNING_IN_CONTAINER=true\u0022,\u0022empty_layer\u0022:true},{\u0022comment\u0022:\u0022buildkit.dockerfile.v0\u0022,\u0022created\u0022:\u00222023-09-20T12:13:34.5265068Z\u0022,\u0022created_by\u0022:\u0022RUN /bin/sh -c apt-get update     \\u0026\\u0026 apt-get install -y --no-install-recommends         ca-certificates                 libc6         libgcc-s1         libicu72         libssl3         libstdc\\u002B\\u002B6         tzdata         zlib1g     \\u0026\\u0026 rm -rf /var/lib/apt/lists/* # buildkit\u0022},{\u0022comment\u0022:\u0022buildkit.dockerfile.v0\u0022,\u0022created\u0022:\u00222023-09-20T12:13:34.8536814Z\u0022,\u0022created_by\u0022:\u0022RUN /bin/sh -c groupadd         --gid=$APP_UID         app     \\u0026\\u0026 useradd -l         --uid=$APP_UID         --gid=$APP_UID         --create-home         app # buildkit\u0022},{\u0022comment\u0022:\u0022buildkit.dockerfile.v0\u0022,\u0022created\u0022:\u00222023-09-20T12:13:36.8764982Z\u0022,\u0022created_by\u0022:\u0022ENV DOTNET_VERSION=8.0.0-rc.1.23419.4\u0022,\u0022empty_layer\u0022:true},{\u0022comment\u0022:\u0022buildkit.dockerfile.v0\u0022,\u0022created\u0022:\u00222023-09-20T12:13:36.8764982Z\u0022,\u0022created_by\u0022:\u0022COPY /dotnet /usr/share/dotnet # buildkit\u0022},{\u0022comment\u0022:\u0022buildkit.dockerfile.v0\u0022,\u0022created\u0022:\u00222023-09-20T12:13:37.1287440Z\u0022,\u0022created_by\u0022:\u0022RUN /bin/sh -c ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet # buildkit\u0022},{\u0022comment\u0022:\u0022buildkit.dockerfile.v0\u0022,\u0022created\u0022:\u00222023-09-20T12:13:39.8264990Z\u0022,\u0022created_by\u0022:\u0022ENV ASPNET_VERSION=8.0.0-rc.1.23421.29\u0022,\u0022empty_layer\u0022:true},{\u0022comment\u0022:\u0022buildkit.dockerfile.v0\u0022,\u0022created\u0022:\u00222023-09-20T12:13:39.8264990Z\u0022,\u0022created_by\u0022:\u0022COPY /shared/Microsoft.AspNetCore.App /usr/share/dotnet/shared/Microsoft.AspNetCore.App # buildkit\u0022},{\u0022author\u0022:\u0022.NET SDK\u0022,\u0022created\u0022:\u00222023-10-02T18:20:04.6914069Z\u0022,\u0022created_by\u0022:\u0022.NET SDK Container Tooling, version 8.0.100-dev\u0022}]}"
  }
}
```

Here we've requested two properties, and received a JSON object with a `Properties` property containing the two properties we requested (both of which MSBuild has stored as strings). From here, we can use any JSON tooling for the CLI or the language of our choice to parse this data and use it.

The same behavior holds for MSBuild Items, which can be requested by Item Type using the `--getItem:<ITEMTYPE>` flag. MSBuild will return these as JSON objects in an array under a key of the Item name, all under the `"Items"` key, like in this example where we request all of the Image Tags published for an image:

```shell
>dotnet publish -p PublishProfile=DefaultContainer --getItem:ContainerImageTags
{
  "Items": {
    "ContainerImageTags": [
      {
        "Identity": "latest",
		.... other MSBuild Item Metadata elided ....
    ]
  }
}
```

Finally, we also support retrieving the outputs of a given Target that's been run using `--getTargetResults:<TARGETNAME>`. These outputs are returned under the `TargetResults` property, like in this example where we get the final image name used by a generated SDK Container Image:

```shell
>dotnet publish -p PublishProfile=DefaultContainer --getTargetResult:ComputeContainerBaseImage
{
  "TargetResults": {
    "ComputeContainerBaseImage": {
      "Result": "Success",
      "Items": [
        {
          "Identity": "mcr.microsoft.com/dotnet/aspnet:8.0.0-rc.1",
          .... other MSBuild Item Metadata elided ....
        }
      ]
    }
  }
}
```

We think this will really make it easier for MSBuild to integrate into other tooling, so we're excited to see what you all do with it!

## SDK Container Publish: Publish to tar.gz archive

* [Issue](https://github.com/dotnet/sdk-container-builds/issues/283)
* [PR](https://github.com/dotnet/sdk/pull/35151)

The SDK has been able to publish containers to local container tools like Docker and Podman, as well as Remote Container Registries like Azure Container Registry, Amazon's Elastic Container Registry, and Docker Hub for quite some time now. However, not all workflows are so straightforward. Some teams may prefer to run scanning tools over their images before pushing them, for example. To help support these workflows, community member @Danielku15 implemented an awesome new feature for the SDK Container Publish tooling - the ability to create a container directly as a tar.gz archive. Once this archive is made, it can be moved, scanned, or loaded into a local Docker toolchain - whatever your needs may be! Let's take a look at how simple it is:

```shell
>dotnet publish -p PublishProfile=DefaultContainer -p ContainerArchiveOutputPath=./images/sdk-container-demo.tar.gz
MSBuild version 17.8.0+6cdef4241 for .NET
  Determining projects to restore...
  All projects are up-to-date for restore.
C:\Program Files\dotnet\sdk\8.0.100-rc.2.23477.19\Sdks\Microsoft.NET.Sdk\targets\Microsoft.NET.RuntimeIdentifierInference.targets(311,5): message NETSDK1057: You are using a preview version of .NET. See: http
s://aka.ms/dotnet-support-policy [D:\code\sdk-container-demo\src\sdk-container-demo\sdk-container-demo.csproj]
  sdk-container-demo -> D:\code\sdk-container-demo\src\sdk-container-demo\bin\Release\net8.0\sdk-container-demo.dll
  sdk-container-demo -> D:\code\sdk-container-demo\src\sdk-container-demo\bin\Release\net8.0\publish\
  Building image 'sdk-container-demo' with tags 'latest' on top of base image 'mcr.microsoft.com/dotnet/aspnet:8.0.0-rc.1'.
  Pushed image 'sdk-container-demo:latest' to local archive at 'D:\code\sdk-container-demo\src\sdk-container-demo\images\sdk-container-demo.tar.gz'.
```

Here I've added one new property to my publish command: `ContainerArchiveOutputPath`. When this property is specified, instead of pushing the image to Docker or to a remote registry, the tooling instead created the file I chose. In this case I specified a file name explicitly, but if you want you can also just specify a folder to push the image into. If you do that, the generated tar.gz will be named after the image (so in this example the generated name would have been `sdk-container-demo.tar.gz`).

Once you have a tar.gz, you can move it wherever you need, or simply load it into Docker using `docker load`:

```shell
>docker load --input .\images\sdk-container-demo.tar.gz
5d6d1d62da4b: Loading layer [==================================================>]  9.442MB/9.442MB
Loaded image: sdk-container-demo:latest
```

Just like that my image is available for me to run via Docker. We hope this new capability enables new workflows for more teams - let us know what you think at [our repo](https://github.com/dotnet/sdk-container-builds)!

## Introducing Tensor Primitives for .NET

- [Issue](https://github.com/dotnet/runtime/issues/89639)
- [PR](https://github.com/dotnet/runtime/issues/92219)

Over the past few years, .NET has made significant refinements to numeric support in an effort to optimize data-intensive workloads like those of AI and Machine Learning.

Starting with the addition of hardware intrinsics in .NET Core 3, we provided .NET developers with access to hardware specific instructions. This enabled .NET applications to utilize modern hardware capabilities like vector instructions which are critical to AI workloads. 

The next step in this effort was Generic Math, which was introduced in .NET 6 as a preview feature and marked stable in .NET 7. Generic Math provides more type-safe simplified numeric operations. This feature eliminates the need for creating numerous nearly identical implementations to cater to different numeric types, thus simplifying the code and making it more maintainable.

Tensor Primitives is the next step in the evolution of Numerics for AI in .NET by building on top of hardware intrinsics and Generic Math, and this set of features is included in .NET 8 starting with RC2.

Tensor Primitives is short for `System.Numerics.Tensors.TensorPrimitives`, a new set of APIs which introduce support for tensor operations. The `TensorPrimitives` APIs are delivered through a standalone `System.Numerics.Tensors` NuGet package. The contents of this package supplant the previous `System.Numerics.Tensors` package, which had only shipped as preview. With .NET 8, `System.Numerics.Tensors` will be marked stable.

For more details, see the [Future of Numerics and AI proposal](https://github.com/dotnet/runtime/issues/89639).

### Semantic search example

AI workloads, such as semantic search or Retrieval Augmented Generation (RAG), enhance the natural language capabilities of Large Language Models like ChatGPT by incorporating relevant data into prompts. In these tasks, operations on vectors, such as cosine similarity, are essential for identifying the most relevant data to answer a question.

Let's imagine you have a movie database which contains movie titles and embeddings. [Embeddings](https://learn.microsoft.com/azure/ai-services/openai/concepts/understand-embeddings) are beyond the scope of this post, but they're ways of encoding semantic information as an array of numbers. In this case, they represent a brief synopsis of these movie plots and they've been precomputed. However, if you'd like you can use models like those from [Azure OpenAI to generate embeddings](https://learn.microsoft.com/azure/ai-services/openai/how-to/embeddings?tabs=csharp).

```csharp
var movies = new [] {
    new {Title="The Lion King", Embedding= new [] {0.10022575f, -0.23998135f}},
    new {Title="Inception", Embedding= new [] {0.10327095f, 0.2563685f}},
    new {Title="Toy Story", Embedding= new [] {0.095857024f, -0.201278f}},
    new {Title="Pulp Function", Embedding= new [] {0.106827796f, 0.21676421f}},
    new {Title="Shrek", Embedding= new [] {0.09568083f, -0.21177962f}}
};
```

Let's say that you wanted to search for family-friendly movies using the search term "A movie that's fun for the whole family". The embedding for that query might look as follows. 

```csharp
var queryEmbedding = new[] {0.12217915f, -0.034832448f };
```

At this point, to see which movies in the database more closely match my query, I can compute the distance using a distance function like cosine similarity.
 
#### Before Tensor Primitives

Before `TensorPrimitives`, if you needed to apply any operations such as cosine similarity to your tensor-shaped data, you had two options:

1. Use external dependencies
2. Implement your own operations

##### Using external dependencies

The easiest way for you to use any of these operations was to use existing libraries such as TorchSharp, TensorFlow.NET, Math.NET Numerics, ML.NET, or Semantic Kernel. If the application you were building heavily relied on the functionality provided by those libraries, it made sense to add them to your project. However, if all you needed was a handful of methods from those libraries, taking a dependency on those libraries introduced additional overhead. 

```csharp
using TorchSharp;
using static TorchSharp.torch.nn.functional;

var top3MoviesTorchSharp = 
    movies
        .Select(movie => 
            new {
                Title=movie.Title, 
                Embedding=movie.Embedding, 
                Similarity=cosine_similarity(torch.tensor(queryEmbedding), torch.tensor(movie.Embedding), 0L).item<float>()})
        .OrderByDescending(movies => movies.Similarity)
        .Take(3);
```

This is what the code might look like for measuring the similarity of my query to my movie collection. After sorting by most similar (higher is more similar) and taking the top 3 movies most likely to be "family-friendly", the output is the following:

| Title | Similarity |
| --- | --- | 
| Toy Story | 0.66102695 |
| Shrek | 0.6457999 |
| The Lion King | 0.62360466 |

As you can see from this example, movies such as Pulp Fiction and Inception are not part of the results because they're not considered "family-friendly" movies. 

While TorchSharp makes it easy for you to calculate cosine similarity, adding it as a dependency for this one function may not make sense. 

##### Writing your own implementation

If you didn't want to take on another external dependency just to use a few operations in your application, the other alternative was to implement your own. This is what many of the libraries mentioned previously have done. While this is a viable path that offers you the most control over your code, this meant you were writing framework-level code rather than focusing on the competitive advantages of your application or business. Most likely your code might've been a naive implementation of the operation which didn't take full advantage of the runtime's capabilities for hardware optimizations.  

```csharp
public float CosineSimilarityCustom(ReadOnlySpan<float> vector1, ReadOnlySpan<float> vector2)
{
    if (vector1.Length != vector2.Length)
        throw new ArgumentException("Vectors must have the same length");

    float dotProduct = 0f;
    float magnitude1 = 0f;
    float magnitude2 = 0f;

    for (int i = 0; i < vector1.Length; i++)
    {
        dotProduct += vector1[i] * vector2[i];
        magnitude1 += vector1[i] * vector1[i];
        magnitude2 += vector2[i] * vector2[i];
    }

    magnitude1 = MathF.Sqrt(magnitude1);
    magnitude2 = MathF.Sqrt(magnitude2);

    if (magnitude1 == 0 || magnitude2 == 0)
        return 0;  // handle the case where one or both vectors have zero magnitude

    return dotProduct / (magnitude1 * magnitude2);
}
```

This is what a custom implementation of cosine similarity might look like. You'd then use this as follows. 

```csharp
var top3MoviesCustom = 
    movies
        .Select(movie => 
            new {
                Title=movie.Title, 
                Embedding=movie.Embedding, 
                Similarity=CosineSimilarityCustom(queryEmbedding, movie.Embedding)})
        .OrderByDescending(movies => movies.Similarity)
        .Take(3);
```

The results of this code are the same as those from the TorchSharp example. While there are no external dependencies in this case, cosine similarity now becomes a set of framework-level code you need to maintain. 

#### After Tensor Primitives

`TensorPrimitives` simplifies these choices. If you just need a handful of operations, you don't need to take on an external dependency into your project. 

```csharp
using System.Numerics.Tensors;

var top3MoviesTensorPrimitives = 
    movies
        .Select(movie => 
            new {
                Title=movie.Title, 
                Embedding=movie.Embedding, 
                Similarity=TensorPrimitives.CosineSimilarity(queryEmbedding, movie.Embedding)})
        .OrderByDescending(movies => movies.Similarity)
        .Take(3);
```

Similarly, for libraries who make use of these operations such as Semantic Kernel and ML.NET, they can replace many of their existing implementation with TensorPrimitives so they can focus on their competitive advantages and deliver features faster. 

### Current State

Currently, Tensor Primitives provides vectorized implementations for operations like:

- CosineSimilarity
- SoftMax
- Sigmoid
- Tanh
- Sinh
- Norm (L2)
- SumOfSquares
- ProductOfSums

For a complete list of operations, see [Tensor Primitives work items tracking issue](https://github.com/dotnet/runtime/issues/92219).

### Suggested Usage

In initial tests, we've observed that the performance efficiencies this package introduces are more noticeable on tensors with a large number of elements. 

### What's next?

Over the next few months, we plan to:

- Provide more comprehensive coverage of tensor operations.
- Replace existing implementations of these operations in libraries like ML.NET and Semantic Kernel with the Tensor Primitives implementations.

### Get started with Tensor Primitives today

In your .NET project, add a reference to the latest `System.Numeric.Tensors` NuGet package.

```dotnet
dotnet add package System.Numeric.Tensors --prerelease
```

Give us feedback and file any issues in the [dotnet/runtime](https://github.com/dotnet/runtime) repo.

## Community Contributor

![Florian Verdonck](community-spotlight.jpg)

I’m Florian Verdonck and I live in [Flanders Fields](https://www.britishlegion.org.uk/get-involved/remembrance/about-remembrance/in-flanders-field) in Belgium.  

My first programming experience was at the age of 18. [VB.NET](http://vb.net/) was my first programming language and near the end of my higher education, I favoured C# as my go-to language. I quickly took an interest in web development – without specializing in front-end or back-end development. As my career has progressed, I’ve always been eager to learn new software and practices. After getting the hang of C# and object-oriented programming, I learned about function programming during a [local meetup](https://www.meetup.com/socratesbe/) and had to try it out. 

Exploring functional programming was quite an eye-opener for me. It broadened my perspective on many things in the software industry such as how working too long with a popular programming language can make you oblivious to what else is out there. And why it is important to come out of your comfort zone from time to time. 

Exploring F# to learn about functional programming was a logical step for me as I had a strong familiarity with .NET. I liked how concise F# code can be and how there is no need to go fully functional from day one, you can start with what you know and gradually apply new techniques as you learn them. F# also has a treasure glove of small features and hidden gems which makes it unique. I was in love.  

As F# was my first indentation-based programming language, it took me some time to adjust to the syntax. One thing that was missing for me, coming as I did from other languages, was a code formatter.  

I value code style consistency when I’m about to add new code to a shared codebase, and always do my best to be considerate of my predecessors. When I started, F# didn’t have any out-of-the-box solution to the problem of multiple collaborators, sometimes with their own styles, that can’t share code because it simply isn’t formatted in a universally understood fashion. That is why I began contributing to the [Fantomas project](https://fsprojects.github.io/fantomas/), as a part of the [F# Foundation mentorship](https://fsharp.org/mentorship/) program. My mentor, [Anthony Lloyd](https://anthonylloyd.github.io/), and I revived the project after it had lost its main maintainer.   

Contributing to Fantomas has been instrumental in my career. Not only was it great for some real-world F# work experience, but it also led me to pursue other interests like public speaking, product ownership and community building. Maintaining Fantomas was my contribution to the F# ecosystem and, in the process, I found personal meaning and a sense of belonging.  

My enthusiasm for Fantomas had its practical side, I was working on it in my spare time as most individual open-source maintainers do, and it still wasn’t really a reliable or stable tool at the time. Frustrating bugs would still lurk behind every corner, adoption was still limited and there were upstream shortages in the F# compiler (Fantomas uses the parser from the F# compiler internally). 

What elevated Fantomas was the support of a London fintech firm called G-Research. The open-source division of G-Research saw potential in the project and decided to sponsor me financially to work on Fantomas. This made it possible to formally work on Fantomas on a weekly basis. 

G-Research’s sponsorship granted me the time to tackle more challenging problems in the codebase and led me to start contributing to the F# compiler. With the help of [Chet Husk](https://github.com/baronfel), I was able to land [my first pull request](https://github.com/dotnet/fsharp/pull/10769) where we addressed a lack of information after parsing. Once I got over that initial mental roadblock of contributing to the compiler, I started doing it again and again. The F# team has been appreciative and helpful to all my contributions, and I encourage everyone to contribute!  

Last year, I joined the open-source team at G-Research full-time. Not to work just on Fantomas, but to tackle bigger problems in the F# compiler and tooling. It is a firm willing to actively contribute back to the software and tools it uses. I’m very fortunate to have the privilege to work on these interesting projects and get the opportunity to work on the very heart of the F# ecosystem.

## Importance of Feedback

Lastly, we want to highlight the importance of your feedback through these .NET 8 preview and RC releases. Your feedback is instrumental in helping shape new .NET experiences. Here are a few examples to illustrate the importance of your voice:

- **ASP.NET Core.** [Reconsidered enabling HTTP/3 by default in .NET 8.](https://github.com/dotnet/aspnetcore/issues/50131)
- **Blazor.** [Changed their entire plan around project structure and TFMs for Blazor WebAssembly.](https://github.com/dotnet/aspnetcore/issues/49079)
- **Runtime.** [Lit up newer ARM64 hardware capabilities](https://github.com/dotnet/runtime/issues/89937) and [updated Marshal.QueryInterface.](https://github.com/dotnet/runtime/issues/91981)
- **SDK.** [Improved simplified output paths](https://devblogs.microsoft.com/dotnet/announcing-dotnet-8-preview-4/#sdk-simplified-output-path-updates) and polished the terminal logger. 
![Modern Terminal Logger](./modernbuildoutputdiagnostics.gif)
- **NuGet.** [Improved the design of the new package vulnerability auditing feature.](https://github.com/NuGet/Home/pull/12310)

## Summary

In the journey towards the release of .NET 8, we extend our heartfelt gratitude to all the passionate .NET developers around the world who stepped up to explore and test the previews and release candidates. Your dedication to the .NET ecosystem has been invaluable, and your feedback has played a pivotal role in ensuring the reliability and robustness of this latest version. Thank you for being an essential part of this exciting journey, and we can't wait to see what you'll bring to life with .NET 8.
