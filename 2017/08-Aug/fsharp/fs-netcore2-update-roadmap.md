# Update and FAQ for F# and .NET Core

We've been working on F# and .NET Core for some time now.  F# has been supported on .NET Core since the initial release of .NET Core 1.0, and many people have enjoyed success with it as the F# open source ecosystem has begun supporting .NET Core and .NET Standard.  Most impressively, you can even use F# full-stack on .NET Core 1.0 with Fable to write the front-end and backend of a website.

Yes, you've read that correctly.  You can use F# and .NET Core 1.0 via Fable to write **full-stack** applications today!  It's stable, used in real-world production systems, and is developing its own strong, healthy community.

## Current status of F# and .NET Core 2.0

As many of you are aware, .NET Core 2.0 and .NET Standard 2.0 are on the horizon.  Strategically speaking, there is nothing more important than .NET Core for F#.  Because of this, we have focused most of our efforts on .NET Core 2.0 support.

Starting with .NET Core 2.0 Preview 3, all of the changes we have made for F# support will be in the .NET Core SDK and .NET Core CLI.  We feel that it is stable enough for you to start using it as you see fit.  We're also working with the maintainers of highly-used open source projects to ensure they work well on .NET Core 2.0.

Finally, this is the first release of F# on .NET Core that can be built from source as a part of the .NET Core SDK and the .NET Core CLI.

## The best way to use F# and .NET Core today

The best way to use F# and .NET Core today is in Visual Studio Code with the excellent Ionide-FSharp plugin.  This should come as no surprise to F# developers already using .NET Core.  It supports many things, including project scaffolding and debugging.

![Debugging F# on .NET Core with Visual Studio Code and Ionide](fs-net-core-debug.gif)

There are only three dependencies required:

1. Install [Visual Studio Code](https://code.visualstudio.com/).
2. Install the [Ionide-FSharp extension](https://marketplace.visualstudio.com/items?itemName=Ionide.Ionide-fsharp).
3. Install the [C# extension](https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp) (this is needed for the .NET Core debugger, which ships only in the C# plugin)

From there, you can configure debugging for .NET Core and debug your F# application!

## Visual Studio tooling support

Support for .NET Core and .NET Standard in Visual Studio is still a work in progress.  Visual Studio can actually load these projects today, but the experience is simply not good enough for us to declare the tooling support as shipped.  Specifically, the following will still be unsupported with Visual Studio 2017 Update 3:

* IntelliSense recognizing added files and their ordering
* IntelliSense and the project UI not recognizing project or package references
* Adding a script file from the right-click menu (fixed already, but not in the product)
* The ability to order files using the Visual Studio UI

If you load one of these projects into Visual Studio, you will likely experience these issues, even though the project will run and debug.  This is still something we're working on, and we're treating it with the highest priority now that the F# compiler is stable on .NET Core 2.0.

## FAQ

### 1. What are the release dates?

There are no release dates for .NET Core 2.0 at this time.

### 2. What about F# Interactive?

F# Interactive runs as a REPL today on .NET Core 2.0, but `#r` is thoroughly broken.  .NET Core introduces a different model for how assemblies are laid out on disk and loaded by a process such as F# Interactive.  This model breaks `#r` in F# Interactive.  Rather than add workarounds in F# Interactive, we are going to move forward with a plan to use a package manager to resolve assemblies and change the way that you use `#r` from here on out.  

In short, our goal is to move you away from specifying assemblies on disk and towards referencing a package by name and optional version.  This may look like this:

```fsharp
#r "nuget: Newtonsoft.Json" // Using NuGet
#r "paket: Newtonstoft.Json >= 9.1.0" // Using paket
```

We're still working out the design and default behavior.  It will be backwards-compatible, but we're using .NET Core as a precedent to move towards this new way to reference things in F# scripting and F# Interactive.

### 3. What about Type Providers?

Erasing Type Providers work with [this workaround](https://github.com/Microsoft/visualfsharp/issues/3303) today.  In the near future, this won't be necessary.

Generative Type Providers are still unsupported and with now workaround.  They depend on APIs which are not yet available on .NET Core.  There is no ETA.

### 4. Why is the Visual Studio tooling for .NET Core still not ready?

In summary:

1. It's more work than we initially estimated.
2. We're not comfortable with shipping broken tooling support for .NET Core in Visual Studio.

We had done initial estimates of the work needed in our language service to support .NET Core projects.  Unfortunately, we did not anticipate the amount of downlevel work required in the lower-level pieces of the project system that loads .NET Core projects.  Given our need to ensure that the F# compiler was stable and usable on .NET Core, we prioritized cross-platform workflows to allow the bulk of the F# Open Source ecosystem to successfully port to .NET Core 2.0.

The current state of the support in Visual Studio is not good enough for us to declare it shipped.  Although you can load any .NET Core SDK-based project today, and even have a good experience if the operations you perform are limited, the experience is very incomplete.  Rather than ship this support, we've made the call to declare it as shipped when that experience is good enough.

### 5. What is the bar for what you consider a good experience for .NET Core and Visual Studio tooling?

Below is what we consider to be the minimal bar for shipping this tooling support:

1. All existing .NET Core projects (1.0, 1.1, and 2.0) load, without error, in Visual Studio.
2. All .NET Core SDK projects targeting .NET Framework load, without error, in Visual Studio.
3. All existing Visual Studio features (e.g., Go to Definition, Rename, etc.) work with .NET CORE SDK-based projects.
4. There are no false negatives or false positives with IntelliSense and error reporting.
5. You can create new .NET Core and .NET Standard projects in Visual Studio, including ASP.NET Core, MSTest, xUnit, and more.
6. Performance is at least as good as projects targeting .NET Framework with "old style" `.fsproj` projects.

Although we're close, we will not ship until the above six points are met.

### 6. What about file ordering in Visual Studio?

File ordering with the Visual Studio UI requires significant downlevel work in the underlying project system code.  We're working with that team to ensure it works well, but file ordering with the Visual Studio UI is not a ship-blocker for us.  This is because project files using the .NET Core SDK are _significantly_ shorter.  In many projects, their size can drop by an order of magnitude.  The project system also does not require you to unload a project to edit the project file, which makes adjusting file order (and anything else in the project file) far easier than ever before.  We feel that these improvements result in an experience that's good enough to ship.