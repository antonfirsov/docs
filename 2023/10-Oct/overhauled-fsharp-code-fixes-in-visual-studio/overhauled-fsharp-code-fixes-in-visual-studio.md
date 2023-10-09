---
post_title: Overhauled F# code fixes in Visual Studio
author1: psemkin
post_slug: overhauled-fsharp-code-fixes-in-visual-studio
microsoft_alias: psemkin
featured_image: fsharp-codefix-in-vs.png
categories: F#, Visual Studio
tags: f#, visual studio, code fixes, developer experience
summary: F# code fixes are now more performant, correct, and testable.
post_date: 2023-10-11 10:05:00
---

This summer, the F# code fixes in Visual Studio underwent significant updates. We addressed a few dozens of bugs and regressions, sped things up and brought an easy way to improve, test and create code fixes. Here, we'll share a few details on what has changed and give you tips on how you can contribute to our mission of boosting developer productivity.

## Code fixes in F#

Code fixes fall under [Quick Actions](https://learn.microsoft.com/visualstudio/ide/quick-actions) available in the light bulb menu. They are triggered by diagnostics - errors, warnings, or informational messages. Each diagnostic has an ID and a location, often indicated by a squiggly line. Code fixes aim to automatically resolve these diagnostics. Here is the `Add missing 'fun' keyword` [code fix](https://github.com/dotnet/fsharp/blob/f46963104630b5623f439ba09bde9c02f4d45d2b/vsintegration/src/FSharp.Editor/CodeFixes/AddMissingFunKeyword.fs) in action:

[video src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2023/10/code-fix-demo.mp4"]

At present, Visual Studio offers over 30 F# code fixes. These include tasks like removing redundant code, converting C# constructs to F#, and adding missing import directives.

## How can they be improved?

Compiler diagnostics can relate to a wide variety of situations. Take the `FS0010` error as an example: it is raised for any unexpected symbol, be it an operator, keyword, or a misplaced brace. Challenges emerge when a code fix is suitable for certain instances but might lead to undesirable source modifications in others. Our primary objective has been to ensure code fixes are precise and don't mislead developers.

And as usual, there are performance opportunities. Recently, we incorporated [IcedTasks](https://github.com/TheAngryByrd/IcedTasks) - an amazing library created by our external contributor, [@TheAngryByrd](https://github.com/TheAngryByrd). Among other benefits, the library helps reduce memory consumption and improves the management of canceled user actions. With this change, we transitioned from `async` to `task` expressions in code fixes. This shift is in line with our [general recommendation](https://learn.microsoft.com/dotnet/fsharp/tutorials/async#core-concepts) when interoperating with .NET libraries.

## How can they be tested?

Traditionally, much of Visual Studio's F# functionality, including code fixes, relied on manual testing — an approach neither convenient nor sustainable. Fortunately, we've transitioned this year, implementing a framework tailored for unit testing code fixes.

The framework allows to specify the examples of broken code and the desired fixed code, and specify instances when code fixes should remain inactive (negative testing). This promises to ease the life of contributors dedicated to refining the F# editor.

<pre><code class="language-fsharp">
[<Fact>]
let ``Fixes FS0010 for missing fun keyword`` () =
    let code =
        """
let getEvens numbers =
    numbers
    |> Seq.filter (x -> x % 2 = 0)
"""

    let expected =
        Some
            {
                Message = "Add missing 'fun' keyword"
                FixedCode =
                    """
let getEvens numbers =
    numbers
    |> Seq.filter (fun x -> x % 2 = 0)
"""
            }

    let actual =  AddMissingFunKeywordCodeFixProvider() |> tryFix code Auto

    Assert.Equal(expected, actual)
</code></pre>

## Get involved!

While we've resolved numerous issues, some are still there 🐞. Many code fixes can be further improved in the ways described above. And of course, the more code fixes the better - we have a lot of ideas for them and we welcome fresh perspectives. All of this is tracked in [this ticket](https://github.com/dotnet/fsharp/issues/15408) and some tasks we consider as [good first issues](https://github.com/dotnet/fsharp/issues?q=is%3Aopen+label%3AArea-LangService-CodeFixes+label%3A%22good+first+issue%22).

Additionally, we've crafted [practical guidelines](https://github.com/dotnet/fsharp/blob/1492e70e6d99931ce5133819ebaf4caaadf8da54/vsintegration/src/FSharp.Editor/CodeFixes/CodeFixes.md) on enhancing, testing, and developing F# code fixes. We are looking forward to your contributions!