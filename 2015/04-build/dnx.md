.NET Execution Environment (DNX)
================================

The [.NET Execution Environment (DNX)](https://github.com/aspnet/dnx) is a new development and execution environment across multiple platforms (Windows, OS X, Linux, ...), multiple CPUs (x86, x64, ...) and across different .NET flavors (.NET Framework, .NET Core and Mono). It provides a consistent experience across that breadth of options, making your job significantly easier.

DNX provides the following benefits:

- Straightforward acquisition and management of DNX versions.
- Compile and launch apps from source or as NuGet packages.
- Simple and effective dependency management and acquisition.
- Support for multiple .NET runtimes, OSes and CPUs.
- Supports ASP.NET 5 and .NET Core console app workloads.
- Enables debugging from source for referenced packages.

DNX is a general .NET Core concept and facility. It's the easiest way to acquire and use the new open source and cross-platform version of .NET. It started life in the ASP.NET 5 project and has gone through several renames over the past few months (was called 'KRE' before), but that's purely historical.

DNX Tools and Concepts
----------------------

There are several pieces to DNX:

- DNX (distribution): A distribution (a NuGet package) of the components that are the implementation of the new environment. 
	- The .NET Core DNX distribution includes CoreCLR and the base parts of CoreFX.
	- The .NET Framework and Mono DNX distribution only contain the DNX components.
- DNVM: A tool for aquiring and managing DNX distributions. Not part of DNX itself, since plays an admistrator role to DNX. 
- DNU: A NuGet client for DNX. NuGet.exe is not used.
- DNX (commandline tool): A eponymously named tool that controls variou app operationals, primarly launching.

Using DNX
---------

The team has been using DNX as the easy way to both acquire and use .NET Core for development and testing. It's great to use, particularly for the scenario where you SSH into a terminal on Linux and want to quickly acquire a version of .NET and start using it within a minute.

In a typical workflow, you do the following (each of which is a simple command):

- Acquire DNVM.
- Acquire desired runtime flavor (e.g. X64 .NET Core for OS X) with DNVM.
- Write or git clone app source, using DNX concepts (e.g. project.json).
- Restore packages for app, using DNU (part of the DNX distribution).
- Launch app from source: `dnx . run`.

You can learn how to try DNX yourself, with the following instructions:

- [ASP.NET 5 apps](https://github.com/aspnet/home)
- [.NET Core console apps](https://github.com/dotnet/coreclr/blob/master/README.md#get-net-core)


