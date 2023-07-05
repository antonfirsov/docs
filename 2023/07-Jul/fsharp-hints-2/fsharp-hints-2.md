---
post_title: Improved F# hints in Visual Studio
author1: psemkin
post_slug: improved-fsharp-hints-in-visual-studio
username: psemkin
microsoft_alias: psemkin
featured_image: featured.png
categories: .NET, F#, Visual Studio
tags: developer experience, F#, hints, visual studio
summary: We're introducing new F# hints and enhancing existing ones.
post_date: 2023-07-05 10:05:00
---

A few months ago, we introduced a preview of [F# hints](https://devblogs.microsoft.com/dotnet/fsharp-inline-hints-visual-studio/) - the type and parameter name hints. Since then, we've fine-tuned them, added return type hints, and incorporated tooltips for all of them.

Explore the entire experience here:
[video src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2023/07/demo.mp4"]

<details>
<summary>Code</summary>

```fsharp
type Song = {
    Artist: string
    Title: string
}

type Playlist(songs) =

    member _.Add(artist, title) =
        { Artist = artist; Title = title } :: songs

    member _.Shuffle() =
        Algorithms.randomize songs

```

</details>

## Overview

In this code, you can spot type hints, return type hints, and parameter name hints.

![an image with all hints shown in the code](all-hints.png)

Note that all hints now feature tooltips:

![an image showing a tooltip for a parameter name hint](tooltips-for-hints.png)

Also, we refrain from displaying hints for certain obvious scenarios:

![an image demonstrating a parameter name hint not shown when it coincides with the argument name](hints-not-shown.png)

## Enabling the Hints 

These hints remain in preview and off by default.

You can configure each of them separately in options (Go to Tools -> Options -> Text Editor -> Advanced):

![an image showing Visual Studio settings for the hints](hints-in-visual-studio-options.png)

## Looking Forward and Getting Involved

In the long run, we aim to implement a hotkey for toggling hints, make them less intrusive, and include signature hints. You can find the full roadmap [in this issue](https://github.com/dotnet/fsharp/issues/14157), and all related tickets are available using [this query](https://github.com/dotnet/fsharp/labels/Area-LangService-Hints). Many of them are [good first issues](https://github.com/dotnet/fsharp/issues?q=is%3Aopen+is%3Aissue+label%3A%22good+first+issue%22), and we warmly welcome any contributions!