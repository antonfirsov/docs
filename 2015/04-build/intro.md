.NET Announcements at Build 2015
================================

At the Build conference today, Scott Guthrie announced the .NET Framework 4.6 RC and Visual Studio 2015 RC. He also announced important updates for Windows 10 .NET UAP apps, ASP.NET 5 and .NET Core. You can download and try out the releases now:

- [Visual Studio 2015 RC](http://go.microsoft.com/fwlink/?LinkId=517106)
- [.NET Framework 4.6 RC](http://go.microsoft.com/fwlink/?LinkId=524667)
- [.NET Core (on GitHub)](https://github.com/dotnet/core)
- [ASP.NET 5 (on GitHub)](https://github.com/aspnet/home)

As a team, we're really excited to share everything we've been working on:

- .NET Core - for device and cloud
- Windows 10 .NET UAP Apps
- ASP.NET
- .NET Framework 4.6
- Visual Studio Improvements for .NET

.NET Core - for Device and Cloud
================================

.NET Core is a new version of .NET for modern device and cloud workloads. It provides a single set of APIs for you to use for your apps. You can write and share the same code for device and cloud apps without needing to use portable libraries, shared projects or other code sharing techniques. .NET has always offered low-level code portability as a fundamental tenet, and now has a uniform API that can be used in multiple app types.

Today, the [.NET Core Framework](https://github.com/dotnet/corefx) can be used in ASP.NET 5, Windows 10 UAP and .NET Core console apps. The .NET Core API started as the API for Windows 8 Store Apps. It has since grown, both in terms of APIs exposed and to also include other scenarios such as ASP.NET 5 apps. Now, when we add new APIs to .NET Core, they are available for multiple app types at once. This approach makes better use of our engineering time and provides you with a consistent API right away.

All of the .NET Core libraries are distributed as NuGet packages. You can acquire the packages easily within Visual Studio or with one of the NuGet clients directly.

Self-contained and Efficient
----------------------------

Another major benefit of .NET Core is that it can ship as part of your app. It's a self-contained .NET runtime and framework implementation. This means that your app is composed of a set of fixed binaries, unaffected by other apps that might be updating on the same machine. Conversely, your app can be updated without affecting any other apps.

Apps often rely on many .NET Core libraries, such that a lot of self-contained apps might seem like too much of a good thing. This is particularly important for device scenarios. .NET Native solves this problem by optimizing apps to include only the code that a given app relies on. If you only rely on one type within an assembly, that's the only type that will be retained in your final app. .NET Native has other optimization that further slim down your overall app.

.NET Native currently supports only Windows 10 UAP apps, however, it is intended to fit in as a deployment option for .NET Core apps generally.

Cross-Platform
--------------

.NET Core supports Windows, OS X and Linux. You can write UAP apps on Windows 10 and ASP.NET 5 and Console apps on all of the three OSes. FreeBSD support is in progress, [led by the .NET open source community](https://github.com/dotnet/coreclr/issues?q=label%3AFreeBSD+is%3Aclosed). We expect that the community will create other OS ports. In fact, the [OS X](https://github.com/dotnet/coreclr/pull/117) port has also been community led.

.NET Core supports x86, x64 and ARM CPUs in order to support device, cloud and console app scenarios. We expect that to see more chips come online, particularly given the [LLILC](https://github.com/dotnet/llilc) LLVM integration project. LLILC will (in theory) make it possible to port .NET Core to all of the chips that LLVM supports.

Open Source
-----------

The [.NET Core](http://github.com/dotnet/core) is open source on GitHub. You can look at the code and even make contributions. We have received many great contributions over the last number of months. Thanks!

The [.NET Core Framework](https://github.com/dotnet/corefx) team are in the process of publishing all of their code on GitHub and are now over half-way done. You can check out their progress, maintained at the [CoreFX Progress](https://github.com/dotnet/corefx-progress) repo. You can also see their progress in the image below.

![CoreFX Progress](https://raw.githubusercontent.com/dotnet/corefx-progress/master/progress.png) 

