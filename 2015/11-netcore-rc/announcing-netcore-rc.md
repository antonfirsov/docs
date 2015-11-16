Announcing .NET Core and ASP.NET 5 RC
=====================================

Today, we are announcing .NET Core and ASP.NET 5 Release Candidate 1, supported on Windows, OS X and Linux. This release is "Go Live", meaning you can deploy apps into production and call Microsoft Support if you need help. Please check out the [Announcing ASP.NET 5 RC1 blog post](http://blogs.msdn.com/b/webdev) to learn more about the updates to ASP.NET 5.

The best way to get the RC is to go to the [Get ASP.NET](http://get.asp.net) site. It has everything you need: downloads, instructions and samples. If you already have one of the betas installed, you can upgrade your environment to RC1 from the command-line (link to document w/those intructions).

We also have exciting news to share about changes that will come with .NET Core RC2.

- New commandline tools to compile and manage .NET Core projects.
- Native compilation of .NET Core apps, on Windows, OS X and Linux, with .NET Native and the new CoreRT AOT-optimized .NET Core runtime.
- CoreRT, .NET Native and the new commandline tools are open source on GitHub.
- Packages and installers for the supported operating systems.

The team will be hanging out in the [.NET Foundation Forums](http://forums.dotnetfoundation.org) to answer your questions about the RC or anything else about .NET Core and ASP.NET 5. We're here to help!

.NET Core and ASP.NET 5 RC
==========================

.NET Core and ASP.NET 5 are now RC. They are ready for you to start building web apps and services with ASP.NET 5. You can build apps and services that scale, that work on-prem and in the Cloud and that can be run on Windows, Linux and OS X. .NET apps are portable. You can take an app running on Windows and move it to Linux, or vice-versa, without code modification. That's a lot of flexibility in the way you build, deploy and manage apps.

We've done several beta [releases](https://github.com/aspnet/Home/releases) since the [Preview release](http://blogs.msdn.com/b/dotnet/archive/2015/04/29/net-announcements-at-build-2015.aspx) earlier in the year. We've added many features and now have Linux and OS X implementations in place. The following are some of the key features that we've added since Preview:

**ASP.NET 5**

- feature 1
- feature 2

**.NET Core**

- feature 1
- feature 2

Key Feature 1
-------------

Description

Key Feature 2
-------------

Description

Re-Introducing .NET Core
========================

We've been shipping betas of .NET Core for nearly two years. Over the last few months, we realized that the lowest-level .NET Core experience needed to be improved, based on your feedback and to enable new scenarios. These changes are planned for RC2, although we want to tell you about them now. The initial changes are [already on GitHub](https://github.com/dotnet/cli), so you can [try them out now](http://dotnet.github.io/getting-started/).

Take a look at the [.NET Core Roadmap](https://github.com/dotnet/core/blob/master/roadmap.md). It includes the schedule and the major features and scenarios.

We'll be relying on your feedback to validate if the proposed experience is better ... we've got a Twitter poll (they only run for 24hrs) waiting for your feedback: link.

What's changing? What's new? This is all RC2.
---------------------------------------------

For the last couple years, we've been working on two important projects concurrently: cross-platform .NET Core for ASP.NET 5 and .NET Native for Windows 10 UWP. It occured to us many times that the native compilation technology (AKA [AOT](https://en.wikipedia.org/wiki/Ahead-of-time_compilation)) we built for UWP would be very useful for other kinds of applications. We've also heard a lot of customer feedback telling us the same thing.

We're extending .NET Native to support .NET Core apps on Windows, Linux and OS X. To start, we are targeting native executables (AKA "console apps"). Over time, we'll extend that to include ASP.NET 5 apps. You can continue to use CoreCLR for your .NET Core apps. It remains a great option if native compilation isn't critical for your needs.

We've been using [DNX](http://blogs.msdn.com/b/dotnet/archive/2015/04/29/net-announcements-at-build-2015.aspx#dnx) for all .NET Core scenarios for nearly two years. It provides a lot of great experiences, but doesn't have great "pay for play" characteristics. DNX is a big leap from  building the [CoreCLR](https://github.com/dotnet/coreclr) and [CoreFX](https://github.com/dotnet/corefx) repos and wanting to build an app with a simple environment. In fact, one of the open source contributors to CoreCLR said: "I can build CoreCLR, but I don't know how to build 'Hello World'." We cannot have that!

.NET Core will have three new components: a set of standalone tools, a shared framework and a set of runtime services. These components will replace DNX and are essentially DNX split in three parts. The tools will implement key scenarios, such as compilation and NuGet package management. The "dotnet" tool is the entry-point tool that you will use in most cases. It provides higher-level commands, often using multiple tools together to complete a task. It's a convenience wrapper over the other tools, which can also be used directly.

At RC2, we intend for .NET Native to be at beta quality. The rest of .NET Core, including the new standalone tools, will be at RC quality.

Enabling .NET Native for .NET Core Apps
---------------------------------------

We've seen significant startup and throughput benefits of native compilation for UWP apps. It's a great scenario addition to .NET Core apps on Windows, OS X and Linux. Today, many native apps and tools benefit from being compiled by a C++ compiler, and not as much by being written in C++. .NET Native brings the much of the performance and deployment benefits of native compilation, while retaining your ability to write in your favorite .NET programming language.

.NET Native is a native toolchain that compiles [IL byte code](https://en.wikipedia.org/wiki/Common_Intermediate_Language) to machine code (e.g. X64 instructions). By default, .NET Native uses RyuJIT as an ahead-of-time (AOT) compiler, the same one that CoreCLR uses as a just-in-time (JIT) compiler. It can also be used with other compilers, such as [LLILC](https://github.com/dotnet/llilc) and [IL to CPP](https://github.com/dotnet/corert/tree/master/src/ILCompiler.Compiler/src/CppCodeGen), an IL to textual C++ compiler we've built as a prototype.

In addition to the toolchain, .NET Native apps require a small runtime component to provide services such as garbage collection. This runtime is called [CoreRT](https://github.com/dotnet/corert). It's a .NET Core runtime that is optimized for AOT scenarios. One can think of it as a "managed native runtime!"

.NET Native offers great benefits that are critical for many apps. 

- The native compiler generates a *SINGLE FILE*, including the app, managed dependencies and CoreRT.
- Native compiled apps startup faster since they execute already compiled code. They don't need to generate machine code at runtime nor load a JIT compiler.
- Native compiled apps can use an optimizing compiler, resulting in faster throughput from higher quality code (C++ compiler optimizations). Both the LLILLC and IL to CPP compilers rely on optimizing compiers.

These benefits open up some new scenarios for .NET developers

- Copy a single file executable from one machine and run on another (of the same kind) without first installing .NET.
- Create and run a docker image that contains a single file executable (e.g. one file in addition to Ubuntu 14.0.4).

.NET Core Tools
---------------



, providing more choice on the experience you want and the dependencies you want for your app. The tools will compile apps and   which are in the [cli repo](https://github.com/dotnet/cli). It will use a shared framework for some scenarios, which are optional. Last, the DNX services will be offered as a hosting option in your app. You can opt to use a host that offers one or more of these services, like file change watching or NuGet package servicing. Some of this is still being designed and isn't yet implemented.

