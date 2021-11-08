---
post_title: 'F# 6 is officially here!'
username: 'kathleen.a.dollard'
microsoft_alias: 'kdollard'
featured_image: fsharp512.png
categories:  F#, .NET, .NET Core
summary: F# 6 is released as part of .NET 6. You'll find new task based async, pipeline debugging, and tons of other features!
desired_publication_date: 2021-11-09
---

F# 6 is now released as part of .NET 6. You'll find new task based async, pipeline debugging, and many features to make your code simpler and more performant.

You can find the full list of features in the [RC2 blog post on F# 6](https://devblogs.microsoft.com/dotnet/whats-new-in-fsharp-6/) and in this [What's new in F# 6 article](https://docs.microsoft.com/dotnet/fsharp/whats-new/fsharp-6).

F# 6 is included with Visual Studio 2022 and .NET SDK 6.0.100. You can find out how to install Visual Studio or the .NET SDK in the announcements for [Visual Studio 2022](https://devblogs.microsoft.com/visualstudio/visual-studio-2022-now-available/) and [.NET 6.0](https://devblogs.microsoft.com/dotnet/announcing-net-6/).

## Template simplification

We've simplified the Console template for F#. The new template is:

```f#
// For more information see https://aka.ms/fsharp-console-apps
printfn "Hello from F#"
```

The new template demonstrates a simple way to create an application and the change is consistent with the simplification of other .NET templates.

## Updated Hello World tutorial with Next Steps video

If you're new to F#, a great place to start is the [F# Tutorial - Hello World in 5 minutes](https://dotnet.microsoft.com/learn/languages/fsharp-hello-world-tutorial). This tutorial has been updated for F# 6 and now includes a link to [Luis Quintanilla's great F# 101 Series](https://aka.ms/fsharp-101) on the Next Steps page.

## Documentation updates

Since our post in October, we’ve done extensive work on the [F# documentation](https://docs.microsoft.com/dotnet/fsharp/) with updated and new content. It's been reorganized to make it easier to find what you are looking for. You'll find:

* Better left pane navigation.
* New features are included, like [F# 6 tasks](https://docs.microsoft.com/dotnet/fsharp/language-reference/task-expressions).
* Reorganized Language Guide Overview, which is now organized by the principle areas of F#.
* Top level links to pages on:
  * [F# for web development](https://docs.microsoft.com/dotnet/fsharp/scenarios/web-development).
  * [F# Machine Learning Tools](https://docs.microsoft.com/dotnet/fsharp/scenarios/machine-learning).
  * [Deploying and Managing Azure Resources with F#](https://docs.microsoft.com/dotnet/fsharp/using-fsharp-on-azure/deploying-and-managing).
  * [Using Apache Spark with F# on Azure](https://docs.microsoft.com/dotnet/fsharp/using-fsharp-on-azure/apache-spark).
* Separate pages for individual tools, making them easier to find.

Documentation is an open-source community effort for F# and .NET. We appreciate both your contributions and your suggestions. If you click the Feedback > This page button at the bottom of a page, it will open an issue you can track in the repo. If you press the pencil icon in the upper right of the page header to edit, you can suggest changes or add content to the document. This works by navigating to the document on GitHub, where you need to press a second pencil button. When you do that, you'll be in edit mode. When you finish your changes, scroll to the bottom, leave us a quick note such as why you made the change, and press the Commit Changes button. This creates a new branch and PR. Of course, if you have a general comment, feel free to open an issue in the [.NET docs repo](https://github.com/dotnet/docs). Working from the page helps by helping us find exactly what you're referring to when it's feedback on a page. If you'd like to contribute, check out the [tracking issue for F# tooling improvements](https://github.com/dotnet/docs/issues/26813)

## Tooling

The F# community has been busy updating tools for F# 6, .NET 6, and Visual Studio 2022.

### Fantomas

Fantomas, the F# code formatting tool for F#, is now available for Visual Studio 2022. Search for “fantomas” in the [Visual Studio Marketplace](https://marketplace.visualstudio.com/items?itemName=asti.fantomas-vs) or in Extensions > Manage Extensions > Online > Search.

Alternatively, you can install Fantomas as a .NET command-line tool with [these instructions](https://github.com/fsprojects/fantomas/blob/master/docs/Documentation.md#fantomas-how-to-use).  

Fantomas keeps its default formatting consistent with the current [F# Style Guide](https://docs.microsoft.com/dotnet/fsharp/style-guide/), and thus default formatting can change between Fantomas versions. For example, F# 6 introduced some indentation changes. The Visual Studio extension is currently pinned to a specific Fantomas version. If you are also using Fantomas a command line tool (such as in CI), you can get conflicting formatting messages. [This problem is being tracked](https://github.com/fsprojects/fantomas-for-vs/issues/21). In the meantime, consider choosing between Visual Studio and the command line tool, and using only one of these approaches to avoid these conflicting messages.

### Ionide

Ionide now supports F# 6 features and syntax. You can update your project to .NET 6 right now and everything should work great. If you encounter any issues, report them in the [Ionide repo](https://github.com/ionide/ionide-vscode-fsharp).

## Summary

With F# 6, the language continues to deliver on its goals since its inception - succinct, robust, performant code that you can trust for practical coding, whether for personal use, start-ups or the enterprise.   We love working together with the F# community to advance the language and its tooling, and encourage you to take F# 6 for a spin,  and to join us in advancing it further!

Thank you so much for all you do!

- Kathleen Dollard (PM for the .NET Languages) and Don Syme (F# Lead Designer)
