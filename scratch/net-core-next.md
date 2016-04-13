.NET Core Commandline Tools and Native Compilation
==================================================

We've been working on some exciting improvements to .NET Core. .NET Core will include a new set of command-line tools and the ability to native compile apps with .NET Native! Over the last few months, we realized that the lowest-level .NET Core experience needed to be better, based on your feedback and to enable new scenarios. 

New experiences:

- Acquire "dotnet" from your favorite package manager or with a user-friendly installer.
- Compile and manage .NET Core projects with simple, consistent and standalone tools.
- Native compile .NET Core apps for fast startup and easy deployment (single file apps).

New .NET Core components:

- Standalone SDK-like tools, including the new higher-level "dotnet" driver tool.
- .NET Native compiler and related tools.
- CoreRT runtime, optimized for native apps (AKA AOT).

These components are on GitHub in the [cli](https://github.com/dotnet/cli) and [corert](https://github.com/dotnet/corert) repos.

We also announced some big improvements to .NET Core, for RC2. You can [try out](http://dotnet.github.io/getting-started/) the new tools now. We'd appreciate your feedback on these changes. We're a few months away from the RC2 release.

You can [try out the new .NET Core experience](http://dotnet.github.io/getting-started/) now. It's all open source and early versions are available on GitHub. Take a look at the [.NET Core Roadmap](https://github.com/dotnet/core/blob/master/roadmap.md). It includes the schedule for the major features and scenarios.

Enabling .NET Native for .NET Core Apps
---------------------------------------

We've seen significant startup and throughput benefits of native compilation for UWP apps, using .NET Native. It's a great scenario addition to .NET Core apps on Windows, OS X and Linux. Today, many native apps and tools benefit from being compiled by a C++ compiler, and not as much by being written in C++. .NET Native brings much of the performance and all of the deployment benefits of native compilation, while retaining your ability to write in your favorite .NET programming language.

.NET Native is a native toolchain that compiles [IL byte code](https://en.wikipedia.org/wiki/Common_Intermediate_Language) to machine code (e.g. X64 instructions). By default, .NET Native uses RyuJIT as an ahead-of-time (AOT) compiler, the same one that CoreCLR uses as a just-in-time (JIT) compiler. It can also be used with other compilers, such as [LLILC](https://github.com/dotnet/llilc), UTC for UWP apps and [IL to CPP](https://github.com/dotnet/corert/tree/master/src/ILCompiler.Compiler/src/CppCodeGen) (an IL to textual C++ compiler we've built as a prototype).

In addition to the toolchain, .NET Native apps require a small runtime component to provide services such as garbage collection. This runtime is called [CoreRT](https://github.com/dotnet/corert). It's a .NET Core runtime that is optimized for AOT scenarios. One can think of it as a "managed native runtime!"

To start, we are targeting native executables (AKA "console apps"). Over time, we'll extend that to include ASP.NET 5 apps. You can continue to use CoreCLR for your .NET Core apps. It remains a great option if native compilation isn't critical for your needs. CoreCLR will also provide a superior debugging experience until we add debugging support to CoreRT.

.NET Native offers great benefits that are critical for many apps. 

- The native compiler generates a *SINGLE FILE*, including the app, managed dependencies and CoreRT.
- Native compiled apps startup faster since they execute already compiled code. They don't need to generate machine code at runtime nor load a JIT compiler.
- Native compiled apps can use an optimizing compiler, resulting in faster throughput from higher quality code (C++ compiler optimizations). Both the LLILLC and IL to CPP compilers rely on optimizing compiers.

These benefits open up some new scenarios for .NET developers

- Copy a single file executable from one machine and run on another (of the same kind) without first installing .NET.
- Create and run a docker image that contains a single file executable (e.g. one file in addition to Ubuntu 14.0.4).

At RC2, we intend for .NET Native to be at beta quality. The rest of .NET Core, including the new standalone tools, will be at RC quality.

.NET Core Tools
---------------

We've been using [DNX](http://blogs.msdn.com/b/dotnet/archive/2015/04/29/net-announcements-at-build-2015.aspx#dnx) for all .NET Core scenarios for nearly two years. It provides a lot of great experiences, but doesn't have great "pay for play" characteristics. DNX is a big leap from  building the [CoreCLR](https://github.com/dotnet/coreclr) and [CoreFX](https://github.com/dotnet/corefx) repos and wanting to build an app with a simple environment. In fact, one of the open source contributors to CoreCLR said: "I can build CoreCLR, but I don't know how to build 'Hello World'." We cannot have that!

.NET Core will have three new components: a set of standalone command-line (CLI) tools, a shared framework and a set of runtime services. These components will replace DNX and are essentially DNX split in three parts. The [CLI tools](https://github.com/dotnet/cli) will implement key scenarios, such as compilation and NuGet package management. The "dotnet" tool is the entry-point tool that you will use in most cases. It provides higher-level commands, often using multiple tools together to complete a task. It's a convenience wrapper over the other tools, which can also be used directly.

The DNX services will be offered as a hosting option in your app. You can opt to use a host that offers one or more of these services, like file change watching or NuGet package servicing. You can also opt to use a shared framework, to ease deployment of dependencies and for performance reasons. Some of this is still being designed and isn't yet implemented.

ASP.NET 5 will transition to the new tools for RC2. The ASP.NET team is already in progress. Both the .NET Core and ASP.NET teams are working closely to enable a smooth transition from DNX to "dotnet" tools.

The new tools are really easy to use. They offer a set of commands for compiling apps and libraries and other typical tasks. You can get a sense of using the tools from the examples below.

**dotnet run**

`dotnet run` provides the same experience `dnx run` does. It compiles and runs your app with one step. 



