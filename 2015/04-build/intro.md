Build .NET Apps for Device and Cloud
====================================

At the Build conference today, Scott Gurthrie announced the .NET Framework 4.6 RC and Visual Studio 2015 RC. You can download and install the releases:

- .NET Framework 4.6 RC
- Visual Studio 2015 RC

As a team, we're really excited to share everything we've been working on:

- .NET Core - for device and cloud
- Windows 10 .NET UAP Apps
- ASP.NET
- .NET Framework 4.6
- Visual Studio Improvements for .NET

.NET Core - for Device and Cloud
--------------------------------

.NET Core is a new version of .NET for modern device and cloud workloads. It provides a single set of APIs for you to use for your apps. You can write and share the same code for device and cloud apps without needing to use portable libraries, shared projects or other code sharing techniques. .NET has always offered low-level code portability as a fundamental tenet, and now has a uniform API that can be used in multiple app types.

Today, the [.NET Core Framework](https://github.com/dotnet/corefx) can be used in ASP.NET 5, Windows 10 UAP and .NET Core console apps. The .NET Core API started as the API for Windows 8 Store Apps. It has since grown, both in terms of APIs exposed and to also include other scenarios such as ASP.NET 5 apps. Now, when we add new APIs to .NET Core, they are available for multiple app types at once. This approach makes better use of our engineering time and provides you with consistent API right away.

Given the need to support Windows 10 UAP apps and ASP.NET 5, these libraries are/will be supported on Windows, Linux and OS X and can run on x86, x64 and ARM CPUs. Many of the libraries are not senseitive to OS and CPU differences, whereas other have special implementations to make them function correctly in each environment.

All of the .NET Core libraries are distributed as NuGet packages. You can acquire the packages easily within Visual Studio or with one of the NuGet clients directly. The [.NET Core libraries are also open source](https://github.com/dotnet/corefx) on GitHub. You can look at the code and even make contributions.  

