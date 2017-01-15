# What's new for the .NET Native Compiler and Runtime in Visual Studio 2017 RC
We are happy to announce the .NET Native 1.6 RC release as part of the [Visual Studio 2017 RC update](). The .NET Native 1.6 RC release includes many enhancements to performance, fixes numerous customer reported issues and is now available as a NuGet package!

Unlike the beta release in November, this update does have Windows Store support, enabling you to release your apps targeting .NET Native 1.6. It's important to note that .NET Native 1.6 is not currently supported in Visual Studio 2015 Update 3 or below. 

## How to Get .NET Native 1.6 RC
We've recently added NuGet support for .NET Native in Visual Studio 2017 RC! This means that you are no longer tied to the .NET Native version that is included inbox with Visual Studio, but instead can now select the version that you want to use. .NET Native will now be included with the Microsoft.NETCore.UniversalWindowsPlatform NuGet package, starting with version 5.3.0. This means that when you upgrade the Microsoft.NETCore.UniversalWindowsPlatform package to version 5.3.0 and above, you are also opting into a specific version of .NET Native. The most recent update to Visual Studio 2017 RC comes with .NET Native 1.4 packaged in the installer, making it the default version. This is the same version that is included in Visual Studio 2015 Update 3, with the addition of a few servicing fixes. 

You can upgrade your Universal Windows Application project to .NET Native 1.6 RC directly by upgrading the Microsoft.NETCore.UniversalWindowsPlatform NuGet package to 5.3.0. Once complete, whenever you build/debug in release builds, Visual Studio 2017 RC will automatically start using .NET Native 1.6 RC.

Here are the steps:

1. Right click on the project and select **Manage NuGet Packages...**
2. Select the **Microsoft.NETCore.UniversalWindowsPlatform** NuGet package.
3. Change the version to **5.3.0**.
4. Click the **Update** button.

![NuGet Screenshot](vsNugetUpdate.PNG)

If you do not see version 5.3.0 listed, make sure that the Package Source is set to **nuget.org**.

You can revert back to .NET Native 1.4 at any time by rolling back the Microsoft.NETCore.UniversalWindowsPlatform NuGet package from 5.3.0 to any earlier version such as 5.2.2 or 5.1.0. This can be done by following the same steps outlined above with the desired version.

## What's New in .NET Native 1.6 RC
.NET Native 1.6 RC contains lots of great improvements, including addressing over 100 customer reported issues!

### Hardware-accelerated System.Numerics.Vectors
We've updated .NET Native's [System.Numerics.Vectors](https://blogs.msdn.microsoft.com/dotnet/2014/11/05/using-system-numerics-vector-for-graphics-programming/) support to be hardware-accelerated on all .NET Native platforms (x86, x64 using 128-bit SSE2 and ARM32 using 128-bit NEON)! The addition of this support can provide significant performance improvements when utilizing .NET Native 1.6.  

Here's a sample project rendering with .NET Native 1.4:

![SIMD_1.6](SIMD_1.4.gif)

Here's the same project rendering with .NET Native 1.6:

![SIMD_1.6](SIMD_1.6.gif)

As you can see, .NET Native 1.4 takes 64 seconds to render, as opposed to .NET Native 1.6 which takes just 1.9 seconds! 

### General Improvements 
We have made many general improvements and addressed over 100 customer reported issues! 

Here are some of the general improvements: 

* You can now inspect static fields that contain the `ThreadStatic` attribute.
* We've began building the Shared Library package on x64 with profile-guided optimizations which reduces the package size and improves startup time for x64 native apps. This change brings x64 to parity with x86 and ARM32.
* We've integrated .NET Native garbage collector with Windows Runtime MemoryManager API to properly calculate memory load factor in UWP applications.
* We've reduced compile times for applications that contain large and/or complex methods by ~25% in certain scenarios. 
* Up to 400% performance improvement in reverse p/invoke, and 135% performance improvement when accessing Windows Runtime objects in certain scenarios.
* We've made improvements to the reflection stack and metadata formats that resulted in up to 300% performance improvements in some customer scenarios.
* We've made improvements to delegate invocation that can reduce code size and give up to 7% faster performance.
* We've also made many other code quality improvements which improved startup times, better steady-state performance, less memory usage and smaller app size.

Here are some of the more common customer reported issues that we fixed:

* We've resolved an issue that sometimes resulted in a 1300 error when submitting a package to the store after upgrading / cherry-picking .NET Core assembly packages. 
* We've resolved an issue that caused a memory leak when interacting with certain Windows Runtime objects in a different process.
* We've significantly reduced global lock contention when accessing Windows Runtime objects from multiple threads
* We've resolved an issue that resulted in queries not executing properly in Entity Framework when enabling .NET Native. ([GitHub #6381](https://github.com/aspnet/EntityFramework/issues/6381))
* We've resolved an issue with System.Linq.Expressions that resulted in unsupressable error messages. ([GitHub #5088](https://github.com/dotnet/corefx/issues/5088))
* .NET Native will now show a warning if you have a native DLL in a different CPU architecture than the application being built. This is a common mistake that results in the application not being able to launch.

### Known issues

* .NET native does not currently support portable PDBs. When debugging managed components with portable PDBs in application compiled with .NET native, you may have trouble setting breakpoints, stepping in, and/or inspecting variables of related types in those components. You can delete the files from the local package directory (users\userName\.nuget\packages) to workaround the warning. This change was also made in the servicing update for .NET Native 1.4 in the latest update to Visual Studio 2017 RC. Earlier versions of .NET native may incorrectly throw `OutOfMemoryException` and crash during build when consuming portable PDBs. Please send feedback to us at dotnetnative@microsoft.com if you encounter an issue with portable PDBs in Visual Studio 2015 Update 3.

## Provide Feedback
We want to thank everyone for your feedback as it has been instrumental! Please continue to send questions, suggestions and feedback to us at dotnetnative@microsoft.com.
