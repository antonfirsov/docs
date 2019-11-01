# NET Core 3 for Windows Desktop

## Intro

In September, we released .NET Core support for building Windows Desktop applications, 
including WPF and Windows Forms. Since then, we have been delighted to see so many 
developers share their stories of migrating desktop applications (and controls libraries) to .NET Core. 
We constantly hear stories of .NET Windows Desktop developers powering their business with WPF and Windows Forms, especially in scenarios where the desktop shines, including:
* Offline workloads
* Applications with dependencies on custom device drivers
* UI-dense forms over data applications
* Extreme low-latency requirements

This is just the beginning for Windows application development on .NET Core. Read on to 
learn more about the benefits of .NET Core for building Windows applications.

## Why Windows desktop on .NET Core?

.NET Core (and in future .NET 5 that is built on top of .NET Core) will be the future of .NET. 
.NET Framework will be supported for a long time, but new updates will be added to .NET Core (and eventually .NET 5). 
To improve Windows desktop stacks and enable .NET desktop developers to benefit from all
the updates of the future, we brought Windows Forms and WPF to .NET Core. 
They will still remain Windows-only technologies because there are tightly coupled
dependencies to Windows APIs. But .NET Core, besides being cross-platform, has many other
features that can enhance desktop applications. 

First of all, all the runtime
improvements and language features will be added only to .NET Core and in future
to .NET 5. A good example here is C# 8 that became available in .NET Core 3.0.
Besides, the .NET Core versions of Windows Forms and WPF will become a part of
the .NET 5 platform. So, by porting your application to .NET Core today you are
preparing them for .NET 5 in future.

Also, .NET Core brings deployment
flexibility for your applications with new options that are not available in
.NET Framework, such as:

* **Side-by-side deployment**. Now you can have multiple
.NET Core versions on the same machine and can choose which version each of your
apps should target.
* **Self-contained deployment**. You can deploy the .NET Core platform with your applications and become completely independent of your end
users environment – you app has everything it needs to run on any Windows
machine.
* **Single .exe files**. You can package your app and the .NET Core
platform all in one .exe file.
* **Improved runtime performance**. .NET Core has many performance optimizations compared to .NET Framework. When you think about the history of .NET Core, built initially for web and server workloads, it helps to understand if your application may see noticeable benefits from the runtime optimizations. Specifically, desktop applications with heavy dependencies on File I/O, networking, and database operations will likely see improvements to performance *for those scenarios*. Some areas where you may not notice much change are in UI rendering performance or application startup performance. 
* **Smaller app sizes**. In .Net Core 3 we  introduced a new feature called trimmer that will analyze your code and include in your self-contained deployment only those assemblies from .NET Core that are needed for your application. That way all platform parts that are not used for your case will be trimmed out.

By setting the properties `<PublishSingleFile>`,`<RuntimeIdentifier>` and `<PublishTrimmed>` in the project file you’ll be able to deploy a trimmed self-contained application as a single .exe
file as it shown in the example below.

````xml
<PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>netcoreapp3.0</TargetFramework>
    <PublishSingleFile>true</PublishSingleFile>
    <RuntimeIdentifier>win-x64</RuntimeIdentifier>
    <PublishTrimmed>true</PublishTrimmed>
</PropertyGroup>
````

## Differences between .NET Framework desktop and .NET Core desktop

While developing desktop applications, you won’t notice much difference between .NET Framework and .NET Core versions of WPF and Windows Forms. A part of our effort was to provide a functional parity between these platforms in the desktop area and enhance the .NET Core experience in future. WPF applications are fully supported on .NET Core and ready for you to use, while we are working on minor updates and improvements. For Windows Forms the runtime part is fully ported to .NET Core and the team is working on the **Windows Forms Designer**. We are planning to get it ready by summer 2020 and for now you can check out the [Preview version]() (which of course provides the limited functionality since the work on it is in progress).

### Breaking changes

There are no breaking changes in WPF and Windows Forms areas between .NET Framework and .Net Core per se, but if you were using such domains as WCF Client, Code Access Security, App Domains, Interop and Remoting, you will need to refactor your code if you want to switch to .NET Core. Also, there are changes in how you configure the .NET features. In .NET Core unlike .NET Framework you cannot use app config files, because .NET Core doesn’t have a machine.config file to define common configuration sections such as system.diagnostics, system.net, or system.servicemodel, so an app config file will fail to load if it contains any of these sections. This change affects System.Diagnostics tracing and WCF client scenarios which were commonly configured using XML configuration previously. In .NET Core you’ll need to configure them in code instead. To change behaviors without recompiling, consider setting up tracing and WCF types using values loaded from a Microsoft.Extensions.Configuration source or from appSettings.

You can find more information on differences between .NET Core and .NET Framework in the [documentation](https://docs.microsoft.com/en-us/dotnet/core/porting/net-framework-tech-unavailable).  

## Getting Started

Check out these short video tutorials:

* [Getting started with WPF on .NET Core](https://www.youtube.com/watch?v=Y4pthq_zGvI&list=PLdo4fOcmZ0oV7n106SEWwWPy4WVjpl3Fj&index=4&t=6s)
* [Getting started with Windows Forms on .NET Core](https://www.youtube.com/watch?v=a66wsCRSgDk&list=PLdo4fOcmZ0oV7n106SEWwWPy4WVjpl3Fj&index=3&t=12s)
* [Differences between .NET Core and .Net Framework and what to choose for your application](https://www.youtube.com/watch?v=BPWTdQ7rh2w&list=PLdo4fOcmZ0oV7n106SEWwWPy4WVjpl3Fj&index=2&t=67s)

## Porting from .NET Framework

As a starting point, can try out a tool we created to help automate converting your .NET Framework project(s) to .NET Core – [Try Convert](https://github.com/dotnet/try-convert).

It's important to remember that this tool is just a starting point in your journey to .NET Core. It is also not a supported Microsoft product. Although it can help you with some of the mechanical aspects of migration, it will not handle all scenarios or project types. If your solution has projects that the tool rejects or fails to convert, you’ll have to port by hand. No worries, we have plenty of tutorials on how to do it (in the end of this section).

The try-convert tool will attempt to migrate your old-style project files to the new SDK-style and retarget applicable projects to .NET Core. For your libraries we leave it up to you to make a call regarding the platform: weather you’d like to target .NET Core or .NET Standard. You can specify it in your project file by updating the value for `<TargetFramework>`.

It is a global tool that you can install on your machine, the you can call from CLI:

````cmd
C:\> try-convert -p <path to your project>
````

or

````cmd
C:\> try-convert -w <path to your solution>
````

As previously mentioned, if the try-convert tool did not work for you, here are materials on how to port your application by hand.

Videos

* [Simple porting case](https://sec.ch9.ms/ch9/beca/05683ec4-e8f8-4415-9009-046352a4beca/on.NET_porting_high.mp4)
* [Advanced porting case](https://www.youtube.com/playlist?list=PLReL099Y5nRdG-LQ6OZSPECF-eXjgFNrW)

Documentation

* [Simple porting case](https://devblogs.microsoft.com/dotnet/porting-desktop-apps-to-net-core/)
* Advanced porting case ([Part 1](https://devblogs.microsoft.com/dotnet/migrating-a-sample-wpf-app-to-net-core-3-part-1/), [Part 2](https://devblogs.microsoft.com/dotnet/migrating-a-sample-wpf-app-to-net-core-3-part-2/))
* [Overview of the porting process](https://docs.microsoft.com/en-us/dotnet/core/porting/)
