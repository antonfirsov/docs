---
post_title: Simplifying F# computations with the new 'while!' keyword
author1: psemkin
post_slug: simplifying-fsharp-computations-with-the-new-while-keyword
microsoft_alias: psemkin
featured_image: image.png
categories: .NET, F#
tags: .net, f#
summary: F# introduces `while!` keyword, streamlining loops in computation expressions.
post_date: 2023-09-20 10:05:00
---

In the evolving landscape of F#, the introduction of the `while!` (while-bang) keyword presents a refined approach to loops in computation expressions. Aiming to minimize boilerplate and maximize clarity, this new keyword is set to enhance the language's expressive power.

## Before `while!`

Consider a simple example: determining the latest ticket number (be it a PR or an issue) from the F# GitHub repository.

Here's a naive approach to achieve this:

```fsharp
open System
open System.Net.Http

let client = new HttpClient()
let fsharpIssuesUrl = "https://github.com/dotnet/fsharp/issues"

let doesTicketExistAsync ticketNumber = task {
    let uri = Uri $"{fsharpIssuesUrl}/{ticketNumber}"
    let! response = client.GetAsync uri
    return response.IsSuccessStatusCode
}

let goThroughFsharpTicketsAsync() = task {
    let mutable ticketNumber = 1
    let mutable keepGoing = true

    while keepGoing do
        match! doesTicketExistAsync ticketNumber with
        | true ->
            printfn $"Found a PR or issue #{ticketNumber}."
            ticketNumber <- ticketNumber + 1
        | false ->
            keepGoing <- false
            printfn $"#{ticketNumber} is not created yet."
}

goThroughFsharpTicketsAsync().Wait()
```

Due to the lack of a mechanism for an asynchronous condition in the while statement, we resort to using two mutable variables.

## With `while!`

With the advent of the `while!` keyword, specifying an asynchronous condition in loops becomes feasible.

```fsharp
open System
open System.Net.Http

let client = new HttpClient()
let fsharpIssuesUrl = "https://github.com/dotnet/fsharp/issues"

let doesTicketExistAsync ticketNumber = task {
    let uri = Uri $"{fsharpIssuesUrl}/{ticketNumber}"
    let! response = client.GetAsync uri
    return response.IsSuccessStatusCode
}

let goThroughFsharpTicketsAsync() = task {
    let mutable ticketNumber = 1

    while! doesTicketExistAsync ticketNumber do
        printfn $"Found a PR or issue #{ticketNumber}."
        ticketNumber <- ticketNumber + 1

    printfn $"#{ticketNumber} is not created yet."
}

goThroughFsharpTicketsAsync().Wait()
```

Notice the elimination of a mutable variable, a reduction in the total lines of code, and a decrease in overall cyclomatic complexity.

## A few technical details

The `while!` construct doesn't require a dedicated builder method. Instead, it invokes `.Bind` in the same manner as `let!` [does](https://learn.microsoft.com/dotnet/fsharp/language-reference/computation-expressions#let). This means that there's no additional work needed when authoring new computation expressions. If you're curious about the resulting syntax tree, you can inspect it within the tests of the feature's PR, for example [here](https://github.com/kerams/fsharp/blob/a881a62a6d56ff90e70f91a1227a568f2f09d015/tests/service/data/SyntaxTree/Expression/WhileBang%2001.fs.bsl).

Furthermore, these constructs are composable. For instance, they can be nested:

```fsharp
while! outerConditionAsync() do
    while! innerConditionAsync() do
        counter <- counter + 1
```

## Try it out!

The `while!` feature is set to be incorporated into F# 8. You can currently experiment with it using the flag `--langversion:preview`. This flag can either be passed to the dotnet fsi invocation or inserted in your .fsproj file within <OtherFlags>.

## Behind the new feature

The development of this functionality in F# is a testament to the language's vibrant and engaged community. The new keyword was wholly [added](https://github.com/dotnet/fsharp/pull/14238) by one of our external contributors who implemented the correspondent [language suggestion](https://github.com/fsharp/fslang-suggestions/issues/1038) - thanks @kerams!


By the way, at the time of writing this post, ticket #15961 is the latest. Many tickets are [good first issues](https://github.com/dotnet/fsharp/issues?q=is%3Aopen+is%3Aissue+label%3A%22good+first+issue%22) and this is your cue to contribute! Language suggestions reside [separately](https://github.com/fsharp/fslang-suggestions), and your insights there will be also very valued. 
