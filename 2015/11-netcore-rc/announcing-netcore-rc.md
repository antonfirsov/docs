Announcing .NET Core and ASP.NET 5 RC
=====================================

Today, we are announcing .NET Core and ASP.NET 5 Release Candidate 1, supported on Windows, OS X and Linux. This release is "Go Live", meaning you can deploy apps into production and call Microsoft Support if you need help. Please check out the [Announcing ASP.NET 5 RC1 blog post](http://blogs.msdn.com/b/webdev) to learn more about the updates to ASP.NET 5.

The best way to get the RC is to go to the [http://get.asp.net](http://get.asp.net) site. It has everything you need: downloads, instructions and samples. If you already have one of the betas installed, you can upgrade your environment to RC1 from the command-line (link to document w/those intructions).

We also have exciting news to share about what's coming next for .NET Core, specifically new commandline tools and .NET Native. Check out the updated [.NET Core Roadmap](https://github.com/dotnet/core/blob/master/roadmap.md) for more details.

The team will be hanging out in the [.NET Foundation Forums](http://forums.dotnetfoundation.org) to answer your questions about the RC or anything else about .NET Core and ASP.NET 5. We're here to help!

.NET Core and ASP.NET 5 RC
==========================

.NET Core and ASP.NET 5 are now RC. They are ready for you to start building web apps and services with ASP.NET 5. You can build apps and services that scale, that work on-prem and in the Cloud and that can be run on Windows, Linux and OS X. .NET apps are portable. You can take an app running on Windows and move it to Linux, or vice-versa, without code modification. That's a lot of flexibility in the way you build, deploy and manage apps.

For RC, we've added many features and now have largely feature complete Linux and OS X implementations in place. You can read [Announcing ASP.NET 5 RC](http://blogs.msdn.com/b/webdev) to learn about ASP.NET 5 RC in detail. You can also check out the release notes to see a list of the product changes and a milestone of commits: [ASP.NET](https://github.com/aspnet/Home/releases), [.NET Core](https://github.com/dotnet/core/releases).  The following are some of the key features that we've added since Preview:

**ASP.NET 5 Visual Studio Experience**

- Integrating Bootstrap snipets
- Updated Bower package UI
- MVC scaffolding is enable for ASP.NET 5 projects

**ASP.NET 5 Runtime**

- Transparent DNX app hosting model
- Configurable webroot folder
- Strong-named framework assemblies

**.NET Core Runtime and BCL**

- Removal of MaxPath restriction.
- RyuJIT now supported on Linux and OS X, including JIT and crossgen
- [Support for LLDB and SOS on Linux](https://github.com/dotnet/coreclr/blob/master/Documentation/building/debugging-instructions.md#debugging-coreclr-on-linux)
- Integration of exception handling with debugger and crash dumps
- GC/thread suspension for Linux and OSX
- Native eventing support via LTTNG for Linux

**.NET Core Libraries**

- [Cross platform SQL Client](https://gist.github.com/cartermp/3c826e1d15577268eda6)
- [Azure Libraries](http://aka.ms/azmgmt-dnx-rc1-pre)

Feature Complete
----------------

We often say, "shipping is a feature". Getting to "feature complete" is a very important step towards shipping a final version. 

.NET Core and ASP.NET 5 are now largely feature complete on Linux, OS X and Windows. For the most part, we have transitioned our focus to performance, reliability and overal quality. We still have a few features left to build, but that is now the exception. On that note, Exceptions are fully implemented!

People often ask us about baseline performance. Linux performance was much slower than our Windows implementation at first. The team has done a great job getting the Linux and OS X implementations to be at parity with the existing Windows implementation. The team hasn't really started more serious performance work. That's still coming.

Explicit DNX app hosting model
------------------------------

DNX has included a host that provides a variety of services to your app. You opt into using this host when you launch your app with `dnx run`, `dnx web` or any other dnx launch commands. The way that the host was called and wired up into your app was a bit *magic*. Some scenarios require providing a different host or doing work before the host initialized your app. There are lots of reasons why you might want a bit more control.

Starting with RC1, the host is now an explicit part of your app. This approach removes the magic, making it more obvious to you how your app functions on top of the platform. In addition, you now have more control. By default, the host is called through the following code, using the new C# 6 [expression method bodies](https://github.com/dotnet/roslyn/wiki/New-Language-Features-in-C%23-6#expression-bodies-on-method-like-members) syntax.

<script src="https://gist.github.com/richlander/938232197287f833a4b2.js"></script>

You can also call the host the *old fashioned way*. This approach is important if you want to do work before calling the host. You shouldn't do work after the host.

<script src="https://gist.github.com/richlander/f002ec4ab673eb47f4fb.js"></script>


You can see a larger example in [ASP.NET sample](https://github.com/aspnet/Home/tree/dev/samples/latest/HelloMvc) in the Home repo. 

RyuJIT now supported on Linux and OS X
--------------------------------------

RyuJIT can generate code for Windows, Linux and OS X. Even though RyuJIT can generate machine code for X64 on Windows, it needed to be taught how to generate code for the same chip on Linux and OS X. The [calling convention](https://github.com/dotnet/coreclr/pull/1806
) and other aspects are different on different OSes. We've made [changes](https://github.com/dotnet/coreclr/pull/427) to accomodate that. 

RyuJIT is also the compiler that is used for CrossGen, the ahead-of-time compiler for CoreCLR. It performs the same task as NGEN for the .NET Framework on Windows. CrossGen had to be taught to generate native images for Linux and OS X. CrossGen produces [PE](https://en.wikipedia.org/wiki/Portable_Executable) images on all OSes, since that's the executable binary format CoreCLR knows how to load. It generates [PDB](https://en.wikipedia.org/wiki/Program_database) images on Windows and [Textual Symbol Tables](https://github.com/dotnet/coreclr/pull/2033) on Linux and OS X in order to integrate with platform-specific debugging tools. CrossGen is currently [broken on Linux](https://github.com/aspnet/dnx/issues/3114) and will be fixed.

Long file names (AKA "MaxPath")
-------------------------------

Windows has had a "MaxPath" restriction on the length of filenames, including directories. The limit is around 260 characters. That's probably fine for most photo collections, but can be a problem for things like developer tools. The .NET Framework, being born on Windows, inhereted this same limitation.

Fast forward to now. The MaxPath restriction has been removed from .NET Core. You can use long filenames in .NET Core apps. Windows still has limited support for long filenames, however the .NET Core APIs won't get in your way of operating on whatever files you want to create. 

Cross Platorm SQL Client
------------------------

An initial version of SqlClient is now working on .NET Core. The major update to SQL client was to move to the .NET Core networking libraries instead of the native Windows-only library that it uses on .NET Framework. By moving the networking logic to C# and .NET Core APIs, the SqlClient library is now able to run anywhere that .NET Core runs. 

SqlClient is not yet feature complete, but it does work on Windows, OS X and Linux. To use SqlClient on OS X or Linux, Multiple Active Result Sets (MARS) must be disabled in the connection string (it is enabled by default). Set it to false by including `MultipleActiveResultSets=False;`, such as in thef ollowing example.

	connectionString="Data Source=my-address,my-port;Network Library=DBMSSOCN;Initial Catalog=mydataBase;MultipleActiveResulSets=False;User Id=myUserName; Password=myPassword;"

Additionally, connectivity to SQL Server is currently only possible via TCP. Names Pipes, Shared Memory, and LocalDB support are not complete yet.

For more information, see this [gist](https://gist.github.com/cartermp/3c826e1d15577268eda6).

Go-Live Support
===============

.NET Core 5 and ASP.NET 5 Release Candidates include Go-Live support. You can engage with the [.NET Core CLR](https://github.com/dotnet/coreclr), [.NET Core FX](https://github.com/dotnet/corefx) and [ASP.NET 5](https://github.com/aspnet) teams directly on GitHub, or, if you encounter issues that are preventing you from deploying in a production environment, please contact [Microsoft Product Support](http://support.microsoft.com/oas/default.aspx?prid=15873). Support is English only and U.S. business hours only (M-F 6a-6p PST), 1 business day response.

Closing
=======

We have released .NET Core and ASP.NET 5 RC1. You can use these in production and call on Microsoft for support via the regular Microsoft Support channels. [Get ASP.NET](http://get.asp.net), read the [ASP.NET 5 RC1 announcement](http://blogs.msdn.com/b/webdev), learn from [ASP.NET Docs](http://docs.asp.net) and play with some simple [ASP.NET samples](https://github.com/aspnet/Home/tree/dev/samples). Get started with RC1!

The team will be monitoring StackOverlow ([asp.net](http://stackoverflow.com/questions/tagged/asp.net) and [.net](http://stackoverflow.com/questions/tagged/.net) tags) and the [.NET Foundation Forums](http://forums.dotnetfoundation.org) for questions.
