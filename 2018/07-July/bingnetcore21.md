# Bing.com runs on .NET Core 2.1!

Bing.com is a cloud service that runs on thousands of servers spanning many datacenters across the globe. Bing servers handle thousands of users' queries every second from consumers around the world doing searches through their browsers, from our partners using the [Microsoft Cognitive Services](https://azure.microsoft.com/en-us/services/cognitive-services) APIs, and from the personal digital assistant, [Cortana](https://www.microsoft.com/en-us/cortana). Our users demand both relevancy and speed in those results, thus performance and reliability are key components in running a successful cloud service such as Bing.

Bing's front-end stack is written predominantly in managed code layered in an MVC pattern. Most of the business logic code is written as data models in C#, and the view logic is written in [Razor](https://github.com/aspnet/Razor). This layer is responsible for transforming the search result data (encoded as [Microsoft Bond](https://github.com/Microsoft/Bond)) to HTML that is then compressed and sent to the browser. As owners of that front-end platform at Bing, we consider developer productivity and feature agility as additional key components in our definition of success. Hundreds of developers rely on this platform to get their features to production, and they expect it to run like clockwork.

Since its beginning, Bing.com has run on the .NET Framework, but it recently transitioned to running on .NET Core. The main reasons driving Bing.com's adoption of .NET Core are performance (a.k.a serving latency), support for side-by-side and app-local installation independent of the machine-wide installation (or lack thereof) and [ReadyToRun images](https://github.com/dotnet/coreclr/blob/master/Documentation/botr/readytorun-overview.md). In anticipation of those improvements, we started an effort to make the code portable across .NET implementations, rather than relying on libraries only available on Windows and only with the .NET Framework. The team started the effort with .NET Standard 1.x, but the reduced API surface caused non-trivial complications for our code migrations. With the 20,000+ APIs that returned with [.NET Standard 2.0](https://docs.microsoft.com/en-us/dotnet/standard/whats-new/whats-new-in-dotnet-standard?tabs=csharp#whats-new-in-the-net-standard-20), all that changed, and we were able to quickly shift gears from code modifications to testing. After squashing a few bugs, we were ready to deploy .NET Core to production.

## ReadyToRun Images

Managed applications often can have poor startup performance as methods first have to be JIT compiled to machine code. .NET Framework has a precompilation technology, `NGEN`. However, `NGEN` requires the precompilation step to occur on the machine on which the code will execute. For Bing, that would mean `NGENing` on thousands of machines. This coupled with an aggressive deployment cycle would result in significant serving capacity reduction as the application gets precompiled on the web-serving machines. Furthermore, running `NGEN` requires administrative privileges, which are often unavailable or heavily scrutinized in a datacenter setting. On .NET Core, the `crossgen` tool allows the code to be precompiled as a pre-deployment step, such as in the build lab, and the images deployed to production are *Ready To Run*!

## Performance ##

.NET Core 2.1 has made major performance improvements in virtually all areas of the runtime and libraries; a great treatise is available on a [previous post in the blog](https://blogs.msdn.microsoft.com/dotnet/2018/04/18/performance-improvements-in-net-core-2-1).

Our production data resonates with the significant performance improvements in .NET Core 2.1 (as compared to both .NET Core 2.0 and .NET Framework 4.7.2). The graph below tracks our internal server latency over the last few months. The Y axis is the latency (actual values omitted), and the final precipitous drop (on June 2) is the deployment of .NET Core 2.1! That is a **34% improvement**, all thanks to the hard work of the .NET community!

![](bingnetcoreimprovement.png)

The following changes in .NET Core 2.1 are the highlights of this phenomenal improvement for our workload. They're presented in decreasing order of impact.

1. [Vectorization of `string.Equals`](https://github.com/dotnet/coreclr/pull/16994) (@jkotas) & [`string.IndexOf`/`LastIndexOf`](https://github.com/dotnet/coreclr/pull/16392) (@eerhardt)

Whichever way you slice it, HTML rendering and manipulation are string-heavy workloads. String comparisons and indexing operations are major components of that. Vectorization of these operations is the single biggest contributor to the performance improvement we've measured.

2. [Devirtualization Support for `EqualityComparer<T>.Default`](https://github.com/dotnet/coreclr/pull/14125) (@AndyAyersMS)

One of our major software components is a heavy user of `Dictionary<int/long, V>`, which indirectly benefits from the intrinsic recognition work that was done in the JIT to make [`Dictionary<K, V>` amenable to that optimization](https://github.com/dotnet/coreclr/pull/15419) (@benaadams)

3. [Software Write Watch for Concurrent GC](https://github.com/dotnet/coreclr/pull/16516) (@Maoni0 and @kouvel)

This led to reduction in CPU usage in our application. Prior to .NET Core 2.1, the write-watch on Windows x64 (and on the .NET Framework) was implemented using Windows APIs that had a different performance trade-off. This new implementation relies on a JIT Write Barrier, which intuitively increases the cost of a reference store, but that cost is amortized and not noticed in our workload. This improvement is now also available on the .NET Framework via [May 2018 Security and Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2018/05/08/net-framework-may-2018-security-and-quality-rollup)

4. [Methods with calli are now inline-able](https://github.com/dotnet/coreclr/pull/13756) (@AndyAyersMS and @mjsabby)

We use `ldftn` + `calli` in lieu of delegates (which incur an object allocation) in performance-critical pieces of our code where there is a need to call a managed method indirectly. This change allowed method bodies with a `calli` instruction to be eligible for inlining. Our dependency injection framework generates such methods.

5. [Improve performance of string.IndexOfAny for 2 & 3 char searches](https://github.com/dotnet/coreclr/pull/13219) (@bbowyersmyth)

A common operation in a front-end stack is search for ':', '/', '/' in a string to delimit portions of a URL. This special-casing improvement was beneficial throughout the codebase.

In addition to the runtime changes, .NET Core 2.1 also brought [Brotli](https://github.com/google/brotli) support to the .NET Library ecosystem. Bing.com uses this capability to dynamically compress the content and deliver a fast user experience! As more users upgrade their browsers to modern verions a growing percentage of Bing.com users will receive Brotli-compressed content.

## Runtime Agility ##

Finally, the ability to have an xcopy version of the runtime inside our application means we're able to adopt newer versions of the runtime at a much faster pace.  In fact, if you peek at the graph above we took the .NET Core 2.1 update worldwide in a regular application deployment on June 2, which is **two days** after it was released!

This was possible because we were running our continuous integration (CI) pipeline with .NET Core's daily CI builds testing functionality and performance all the way through the release.

We're excited about the future and are collaborating closely with the .NET team to help them qualify their future updates! The .NET Core team is excited because of our large catalog of functional tests and an additional large codebase to measure real-world performance improvements on, as well as our commitment to providing both Bing.com users fast results and our own developers working with the latest software and tools.
