# Announcing the Open Sourcing of MSBuild

Today we are pleased to announce that MSBuild is now available on [GitHub](http://github.com/microsoft/msbuild) and we are contributing it to the [.NET Foundation](http://www.dotnetfoundation.org/)! The Microsoft Build Engine (MSBuild) is a platform for building applications. By invoking msbuild.exe on your project or solution file, you can orchestrate and build products in environments where Visual Studio isn't installed. For instance, MSBuild is used to build the [.NET Core Libraries](https://github.com/dotnet/corefx) and [.NET Core Runtime](https://github.com/dotnet/coreclr) open source projects.

![](github.png)

The MSBuild sources we're publishing today are closely aligned with the version we will ship with Visual Studio 2015. You may notice a few differences as this is our first attempt at a standalone build, but you should see those discrepancies reduced over time. And keep in mind that for now, you'll need to have Visual Studio 2015 Preview or the Microsoft Build Tools 2015 Preview installed before you customize and build your own projects.

We will be adding Linux and Mac support soon (perhaps with your help!) so you can use MSBuild to build the open source .NET projects on your preferred platform. We'll initially start with Mono and look to port the code to run on .NET Core. But we're really just getting started on our ports. We wanted to open up the code first, so that we could all enjoy the cross-platform journey from the outset.

### Walkthrough

**Build our Source Tree**

The first scenario you might want to try is building our source tree and then using that output to build it again. To do this, you will need to have Visual Studio 2015 installed on your machine.

git clone [https://github.com/dotnet/msbuild.git](https://github.com/dotnet/msbuild.git)

cd msbuild

build.cmd

RebuildWithLocalMSBuild.cmd

This will create a folder bin\Windows\_NT\MSBuild in your source tree which you can use to build anything you want, including MSBuild.

&nbsp;

**Build a Console App**

To build a console app, you'll first want to run the BuildAndCopy.cmd script we included in the root folder of the source. This will build the sources and create a copy of your build output with everything you need to do a manage project build. To do this and build a simple .NET Core console application, try the following:

BuildAndCopy.cmd bin\Windows\_NT\MSBuild true

cd ..\

git clone https://github.com/dotnet/corefxlab

cd .\corefxlab\demos\CoreClrConsoleApplications\HelloWorld

nuget restore

..\..\..\..\msbuild\bin\Windows\_NT\MSBuild\MSBuild.exe -- Change this as needed to match your MSBuild source location.

.\bin\Debug\HelloWorld.exe

# Summary

MSBuild is the default build engine for Visual Studio and the .NET community on the Windows platform. Through open sourcing MSBuild we are responding to community [feedback](http://forums.dotnetfoundation.org/t/compiling-net-core-code-on-linux-os-x/302/3) and we intend to make it the best choice for .NET developers on the Linux and Mac platforms.

You can learn about the opportunities to get involved [here](https://github.com/microsoft/msbuild/wiki/Contributing%20Code). We look forward to your comments and hearing from you on the [.NET Foundation Forums](http://forums.dotnetfoundation.org/)!

&nbsp;

Tags: MSBuild, open source, announcement

&nbsp;

&nbsp;