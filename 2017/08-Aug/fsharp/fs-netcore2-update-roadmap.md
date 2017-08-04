# F# and .NET Core Roadmap Update

Now that .NET Core 2.0 has released, we wanted to take some time to talk about F# and .NET Core.

## F# and .NET Core 1.0

F# has been supported on .NET Core since 1.0.  In the months leading up to the release of .NET Core 1.0, [Enrico Sada](https://github.com/enricosada) from the F# community worked with us to add support for F# projects in the .NET CLI.  Enrico continued to work with us and other teams to ensure quality F# support as .NET Core tools evolved from project.json to MSBuild.  We also focused on Portable PDB generation during this time so that F# was supported when debugging on .NET Core.  When Visual Studio 2017 was shipped, we declared full support for F# running on .NET Core 1.0 and 1.1.

F# and .NET Core doesn't end at what Microsoft and Enrico worked on.  The rest of the F# open source community has embraced .NET Core, porting many libraries over to it.  The most impressive of which is [Fable](http://fable.io/), which allows you to use the entire JavaScript ecosystem to write F# code that runs in the browser!

Today, F# on .NET Core 1.0 and 1.1 is used in production by many people.  We continue to see adoption on .NET Core, and believe that it will be the primary target for F# developers in the coming years, especially now that .NET Core 2.0 has been released.

## F# and .NET Core 2.0

Our top priority since the release of Visual Studio 2017 has been to ensure the quality of F# when targeting .NET Core 2.0.  Another priority was to ensure that F# could be built from source with .NET Core.  This is critically important for many Linux users, who expect to be able to build their entire development stack from source on the machines they run code on.  It can also greatly extend the reach of F# and .NET Core.  These sources will also be shipped on RedHat Enterprise Linux.

Starting with the release of .NET Core 2.0, all of the changes we have made for F# support in .NET Core 2.0 will be in-box with the .NET Core SDK and .NET Core CLI.  Additionally, we have backported in-box support for F# running on .NET Core 1.0.4 and .NET Core 1.1 in the version of the .NET Core SDK that will ship with Visual Studio 2017 Update 3.  We're also working with the maintainers of highly-used open source projects to ensure they work well on .NET Core 2.0.

## The best way to use F# and .NET Core today

The best way to use F# and .NET Core today is in Visual Studio Code with the [Ionide-FSharp extension](https://marketplace.visualstudio.com/items?itemName=Ionide.Ionide-fsharp).  This should come as no surprise to F# developers already using .NET Core today.  Ionide supports many things, including project scaffolding, IntelliSense, and debugging.

![Debugging F# on .NET Core with Visual Studio Code and Ionide](fs-net-core-debug.gif)

There are only three dependencies required to create, build, run, and debug F# applications on .NET Core today:

1. Install [Visual Studio Code](https://code.visualstudio.com/).
2. Install the [Ionide-FSharp extension](https://marketplace.visualstudio.com/items?itemName=Ionide.Ionide-fsharp).
3. Install the [C# extension](https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp) (this is needed for the .NET Core debugger, which ships only in the C# plugin)

Once you have configured your application for debugging, just like with C#, you can use F# in Visual Studio Code to do nearly anything you want with F# and .NET Core!

## Visual Studio tooling support

For long time .NET developers, a common question is "when will Visual Studio support F# .NET Core projects".  We're working hard to get this functionality ready, but unfortunately, we're not quite happy enough with the quality in the 15.3 update to announce that it's supported. While Visual Studio 2017 Update 3 is able to open the new F# .NET Core projects, build, and debug them, IntelliSense does not yet work correctly.

All .NET Core projects now share a common simplified project format that you've hopefully seen by now. This means that we share a common core component of the IDE referred to as the Common Project System (CPS). This dependency will enable us to develop at a much greater pace, rather than maintaining the large and specialized code base designed specifically for F# projects. Unfortunately, CPS does not yet support file ordering in the tree view of the Visual Studio UI.  We knew that this would prevent ordering files in the Visual Studio UI, but we did not know that this limitation also extended to the pieces of the project system which send files to the F# language service. The F# language service is what powers F# IntelliSense.  Due to receiving files out of order, our language service isn't able to correctly reflect a project's F# code, resulting in false negative error reporting and a lack of IntelliSense in certain cases.  Our work in offering stable support for .NET Core 2.0 took us beyond the window that we were able to fix this issue in CPS for the 2017 15.3 release.

## F# Interactive and Type Providers

.NET Core introduces a different model for how assemblies are laid out on disk and loaded by a process such as F# Interactive.  This model breaks `#r` in F# Interactive.  Rather than add workarounds in F# Interactive, we are going to move forward with a plan to use a package manager to resolve assemblies and change the way that you use `#r` from here on out.  

Our goal is to move you away from specifying assemblies on disk and towards referencing a package by name and optional version.  That may look like this:

```fsharp
#r "nuget: Newtonsoft.Json" // Using NuGet
#r "paket: Newtonstoft.Json >= 9.1.0" // Using paket
```

We're still working out [the design and default behavior](https://github.com/fsharp/fslang-design/issues/167).  It will be backwards-compatible, but we're using .NET Core as a precedent to move towards this new way to reference things in F# scripting and F# Interactive.

Erasing Type Providers work with [this workaround](https://github.com/Microsoft/visualfsharp/issues/3303) today.  In the near future, the workaround won't be necessary.

Generative Type Providers are still unsupported and have no workaround.  They depend on APIs which are not yet available on .NET Core.

## Conclusion

F# is stable on .NET Core, there is a great tooling experience with Visual Studio Code, and much of the F# open source ecosystem is already running on .NET Core today.  Although the entire developer experience isn't complete yet, we are working hard on it.  Each of the important remaining parts - Visual Studio support, F# Interactive, and Type Providers - have a way forward and are being worked on.  We're really excited about the forthcoming release of .NET Core 2.0, and we look forward to seeing more and more people use F# and .NET Core in the future.