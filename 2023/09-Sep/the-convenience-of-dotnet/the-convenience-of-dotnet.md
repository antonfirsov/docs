---
post_title: The convenience of .NET
author1: rlander@microsoft.com
post_slug: the-convenience-of-dotnet
microsoft_alias: rlander
featured_image: convenience-of-dotnet.png
categories: .NET, Performance
tags: Convenience of .NET
summary: The .NET platform provides convenient solutions to many tasks, for developers looking for a straightforward utility function or a high degree of control crafting an algorithm.
post_date: 2023-09-25 10:05:00
---

Convenient options are available for almost every task in life, from getting a ride to the airport to writing code. Convenience is the idea that a great solution is available when you want it and that it works for you. As designers of the .NET platform, we aim to provide convenient solutions for many tasks and to improve the convenience of writing apps with each new release.

This post kicks off a new series, exploring convenient solutions to common tasks. Productivity, performance, security, and reliability are hallmark design points of the .NET platform. We described them in detail in our recent [Why .NET?](https://devblogs.microsoft.com/dotnet/why-dotnet/) post. Stephen Toub also published his annual performance post, [Performance Improvements in .NET 8](https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-8/). This post (and the ones that will follow) explores the ideas and features discussed in those other posts in terms of convenient solutions. You'll see a combination of high-level utility APIs that offer a nice balance of those design points and lower-level APIs that enable you to achieve a different balance per your needs.

The next posts go into much more detail on specific API families, with a lot of code and performance numbers, to fully explore these convenient solutions.

Let's start the series with a more general exploration of how the .NET platform delivers on convenience.

## Convenience is a spectrum

I like using the terms "convenience" and "control", to describe the two ends of the "convenience spectrum". Convenience is descriptive of the experience of writing code and control of your ability to define its behavior.

The most convenient code is compact and straightforward, often with at most a few options to vary behavior (as "choice" is itself a complexity). [`File.ReadAllText()`](https://learn.microsoft.com/dotnet/api/system.io.file.readalltext) is a good example. It returns the contents of a (text) file as a `string` that you can read and process. The lowest-level code enables a lot of flexibility, control, and performance optimization, but requires more careful use. Looking at you, [`File.OpenHandle()`](https://learn.microsoft.com/dotnet/api/system.io.file.openhandle) and [`RandomAccess.Read()`](https://learn.microsoft.com/dotnet/api/system.io.randomaccess.read), which together expose an [operating system handle](https://en.wikipedia.org/wiki/Handle_(computing)) with very little getting in your way to read through a file (as bytes) with maximum performance.

I'm going to show you a couple lists of APIs. It's OK if they don't look familiar. These APIs start with the most raw concepts and formats (the highest degree of control) and end with the most packaged and refined concepts (the most convenient).

Convenience spectrum for reading a text file:

* Most control: `File.OpenHandle` + `RandomAccess.Read`
* More convenient: `File.Open` + `FileStream.Read`
* Even more convenient: `File.OpenText` + `StreamReader.ReadLine`
* Even more convenient: `File.ReadLines` + `IEnumerable<string>`
* Even more convenient: `File.ReadAllLines` + `string[]`
* Most convenience: `File.ReadAllText` + `string`

Convenience spectrum for reading JSON text:

* Most control: `Utf8JsonReader` + `Pipelines` or `Stream`
* More convenient: `JsonDocument` + `Stream`
* Even more convenient: `JsonSerializer` + `Stream`
* Most convenient: `JsonSerializer` + `string`

Note: The APIs are listed as the primary API + their most likely companion API or type.

A key takeaway is that there is no clear break between convenient and control patterns in these lists. The end of one convenience pattern overlaps with the start of the next control pattern. One person's convenience is another's control. That's the definition of a spectrum.

## Convenience starts with choice

You might wonder why we need all these APIs. They all do the same thing, right? The first is that each of these options is the right tool for the job in different circumstances and is convenient for that circumstance. In fact, the .NET developer community consistently requests a broad sprectrum of APIs from us and we're happy to deliver them. The second is that we had to build the low-level APIs in order to make the high-level ones. It's a lot like towers of lego blocks. In theory, we could have exposed only the high-level APIs by making all the low-level ones private, but that's neither desirable nor practical in the general case.

Some developer stacks primarily expose high-level APIs that are built on native code libraries, but are missing useful lower-level APIs. The native code libraries are often written in a way that makes it impractical to expose the lower-level APIs to a managed language, so they are not. That's quite limiting. With .NET, we have a strong philosophy that the library functionality we build should be written in C#, which means that both high- and low-level APIs are available for you to use. It also means you can read the code of all the APIs you use in C# (on GitHub), like the [`File` class](https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/IO/File.cs).

Of course, there are places where we haven't exposed all of the layers; every new API we expose is something we'll have effectively forever, and requires design and direct testing and maintenance and documentation and compatibility constraints and so on.  We're thus selective in which layers we expose when, and are constantly re-evaluating whether additional support should be exposed. The previously mentioned `File.ReadAllText`, for example, has been around for many, many years, whereas the cited `RandomAccess.Read` was only recently introduced. Our long-term trend has been to make lower-level APIs available where there is a compelling case.

## Convenience enables collaboration

The .NET libraries exposes a broad set of functionality for you to use. In many cases (like with the `File` type), much of the related functionality is exposed in one place and designed to work as a larger coherent system. That means you can use more convenient APIs in one part of your code and higher-control APIs elsewhere and it can all be made to work together, nicely.

> "Hey ... I'm going to be writing this data to a `Stream` with APIs that give me the control we need for our service. You can use `StreamReader.ReadLineAsync` to read it. If that doesn't work, I'll expose an `IAsyncEnumerable<string>` for each line and you can use `await foreach` as a streaming solution. Either option works for me. I love how straightforward all of these options are. It is super easy to connect our code together and it's all super fast and convenient." -- .NET dev at ACME Solutions.

Developers working in teams can make different (and equally good) convenience choices at different layers within a larger codebase, with straightforward patterns to connect those layers.

## I'm in control

Yes, yes. The point here isn't to pick a point on this spectrum and stick to it for all the code you write. Instead, the intent is to select APIs that satify the requirements of the algorithm at hand, even if you have the skills to write more challenging code that may be better on some metric (which may or may not matter). The person who maintains your code next might not have your same skills and may (incorrectly) conclude the pattern you chose is a requirement when its not.

> We use convenient APIs in some places in .NET libraries, even though they are not the maximum speed. They makes the code small, simple and easy to understand and that can be more valuable than maximum speed.

That's what one of our architects had to say about our approach to our codebase, even in a team dedicated to high performance. We like to write [convenient code](https://github.com/dotnet/runtime/blob/8f565f39ecdee9ce68c6c7f10598a752ab51bc2e/src/libraries/Common/src/Interop/Linux/os-release/Interop.OSReleaseFile.cs#L24) whenever we can. We'd rather focus our efforts on building more features and optimizing APIs that are likely to get called in a hot loop.

The other side of the coin is that the more efficient the convenience APIs are, the more we'll be able to use them without concern in our codebase. It makes the team as a whole more efficient. We try to make convenience APIs as efficient as possible within the confines of what the shape of the API allows.

## Breaking the spectrum

There are a few cases where a single API covers the majority of use cases. This only happens when an API with a simple contract is an absolute workhorse and is required by a lot of scenarios.

The `string` class APIs are a key example. `IndexOf` and `IndexOfAny` are two favorites. We use these APIs pervasively in the .NET platform and they are used just as much by .NET developers. You can see how many [PRs have targeted those APIs](https://github.com/dotnet/runtime/pulls?q=is%3Apr+IndexOf).

Many of the `IndexOf{Any}` calls are actually on [spans](https://learn.microsoft.com/archive/msdn-magazine/2018/january/csharp-all-about-span-exploring-a-new-net-mainstay) now, rather than direct calls to `string.IndexOf{Any}`. While the spans are frequently pointing into strings, these APIs often operate on slices (after calling `string.AsSpan`, internally).

This family of APIs have been improved a lot, using multiple techniques to improve performance. For example, these APIs uses vector CPU instructions to search for search terms in a string. In .NET 8, [support for AVX512](https://github.com/dotnet/runtime/pull/86655) was added. That's not yet relevant for most hardware, however it means that `IndexOf` will be ready for newer hardware when you've got it.

We'll dive into `IndexOfAny` in much more depth in System.IO post. It's a great API.

## Closing

The .NET team has a "big tent" philosophy. We want every developer to find APIs that are approachable and suitable. If you are new to programming, we've got APIs for you. If you are more familiar with low-level APIs, we've got APIs that are likely familar.

I'm looking forward to sharing some in-depth analysis and exploration of the convenience spectrum in upcoming posts and hope that it leads to an an interesting discussion. If anything, this exercise has given me the insight on how much I appreciate the spectrum of these APIs. Perhaps our documentation should be updated to describe each topic area in terms of this specturm.

Thanks to [David Fowler](https://github.com/davidfowl), [Jan Kotas](https://github.com/jkotas), and [Stephen Toub](https://github.com/stephentoub) for their help contributing to these posts.

You can keep up to date with this series by subscribing to the [Convenience of .NET](https://devblogs.microsoft.com/dotnet/tag/convenience-of-dotnet/) tag feed in your favorite RSS reader or subscribe to the entire blog via [email below](https://devblogs.microsoft.com/dotnet/convenience-of-dotnet/#subscribe_form).
