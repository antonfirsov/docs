# What's new for the .NET Native Compiler and Runtime in Visual Studio 2017 RC
We are happy to announce the .NET Native 1.6 RC release as part of the [Visual Studio 2017 RC update](). The .NET Native 1.6 RC release includes many enhancements to performance, fixes several customer reported issues and now has NuGet support!

Unlike the beta release in November, this update does have Windows Store support, enabling you to release your apps targeting .NET Native 1.6. It's important to note that .NET Native 1.6 will not be availble in Visual Studio 2015 Update 3 or below. 

## How to Get .NET Native 1.6 RC
We've recently added NuGet support for .NET Native! This means that you are no longer tied to the .NET Native version that is included inbox with Visual Studio, but instead can now select the version that you want to use. The most recent update to Visual Studio 2017 RC comes with .NET Native 1.4 packaged in the installer. This is the same version that is included in Visual Studio 2015 Update 3, with the addition of a few servicing fixes.

You can upgrade your Universal Windows Application project to .NET Native 1.6 RC directly by using NuGet. Here are the steps:

1. Right click on the project and select **Manage NuGet Packages...**
2. Change the version to **5.3.0**.
3. Click the **Update** button.

![NuGet Screenshot](vsNugetUpdate.PNG)

## What's New in .NET Native 1.6 RC
.NET Native 1.6 RC contains lots of great improvements, including addressing over 100 customer reported issues!

Here are some of the general improvements:

* You can now use hadware-accellerated System.Numerics on all target platforms (x86, x64 using 128-bit SSE2 and ARM32 using 128-bit NEON).
* You can now debug methods that contain the `ThreadStatic` attribute.
* We've reduced the compile time for applications that contain large and/or complex methods by ~25%. 
* We've made improvements to delegate invocation which reduces the code size, resulting in up to 7% faster performance.
* We've began building the Shared Library package on x64 with profile-guided optimizations which reduces the package size and improves startup time for x64 native apps. This change brings x64 to parity with x86 and ARM32.
* You'll now see a warning thrown when using portable PDB files. If encountered, delete the files from <WHERE?>. This change was also made in the servicing update for .NET Native 1.4 in Visual Studio 2017. 
* We've made serveral code quality improvements which result in improved startup times, better steady-state performance, less memory usage and smaller app size.

Here are some of the more common customer reported issues that we fixed:

* Resolved an issue that caused some customers to receieve a 1300 error when submitting their package to the store after upgrading .NET Native versions. 
* Resolved an issue that caused a memory leak when using the `CoReleaseMarshalData` function.
* Fixed a global lock issue with multiple threads competing for WinRT factory lock and interface lock.
* Fixed an issue that resulted in queries not executing properly in Entity Framework when enabling .NET Native. ([GitHub #6381](https://github.com/aspnet/EntityFramework/issues/6381))
* Fixed an issue with System.Linq.Expressions that resulted in unsupressable error messages. ([GitHub #5088](https://github.com/dotnet/corefx/issues/5088))

## Provide Feedback
We want to thank everyone for your feedback as it has been instrumental! Please continue to send questions, suggestions and feedback to us at dotnetnative@microsoft.com. 