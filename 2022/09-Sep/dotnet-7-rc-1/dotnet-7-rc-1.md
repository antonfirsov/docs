---
post_title: Announcing .NET 7 Release Candidate 1
microsoft_alias: jeliknes
author1: jeremy_likness
post_slug: announcing-dotnet-7-rc-1
categories: .NET
featured_image: dotnet7rc1.png
desired_publication_date: 2022-09-14
summary: .NET 7 Release Candidate 1 is the first of two release candidates that developers can now use in production. This post recaps major features included in the fastest .NET version to date.
---

Today we announce .NET 7 Release Candidate 1. This is the first of two release candidates (RC) for .NET 7 that are supported in production.

You can [download .NET 7 Release Candidate 1](https://dotnet.microsoft.com/download/dotnet/7.0), for Windows, macOS, and Linux.

* [Installers and binaries](https://dotnet.microsoft.com/download/dotnet/7.0)
* [Container images](https://mcr.microsoft.com/catalog?search=dotnet/)
* [Linux packages](https://github.com/dotnet/core/blob/master/release-notes/7.0/)
* [Release notes](https://github.com/dotnet/core/tree/master/release-notes/7.0)
* [Known issues](https://github.com/dotnet/core/blob/main/release-notes/7.0/known-issues.md)
* [GitHub issue tracker](https://github.com/dotnet/core/issues)

.NET 7 Release Candidate 1 has been tested with Visual Studio 17.4 Preview 2. We recommend you use the [preview channel builds](https://visualstudio.com/preview) if you want to try .NET 7 with Visual Studio family products. If you're on macOS, we recommend using the latest [Visual Studio 2022 for Mac preview](https://visualstudio.microsoft.com/vs/mac/preview/).

Don't forget about [.NET Conf 2022](https://dotnetconf.net). Join us November 8-10, 2022 to celebrate the .NET 7 release!

In this blog, we'll highlight the core themes of .NET 7 and provide you with resources to dive deeper into the details.

For a more detailed recap of all the features and improvements included in .NET 7 Release Candidate 1, check out the previous .NET 7 Preview blog posts:

- [Announcing .NET 7 Preview 1](https://devblogs.microsoft.com/dotnet/announcing-net-7-preview-1/)
- [Announcing .NET 7 Preview 2](https://devblogs.microsoft.com/dotnet/announcing-dotnet-7-preview-2/)
- [Announcing .NET 7 Preview 3](https://devblogs.microsoft.com/dotnet/announcing-dotnet-7-preview-3/)
- [Announcing .NET 7 Preview 4](https://devblogs.microsoft.com/dotnet/announcing-dotnet-7-preview-4/)
- [Announcing .NET 7 Preview 5](https://devblogs.microsoft.com/dotnet/announcing-dotnet-7-preview-5/)
- [Announcing .NET 7 Preview 6](https://devblogs.microsoft.com/dotnet/announcing-dotnet-7-preview-5/)
- [Announcing .NET 7 Preview 7](https://devblogs.microsoft.com/dotnet/announcing-dotnet-7-preview-7/)

## .NET MAUI
.NET Multi-platform App UI (MAUI) unifies Android, iOS, macOS, and Windows APIs into a single API so you can write one app that runs natively on many platforms. .NET MAUI enables you to deliver the best app experiences designed specifically by each platform (Android, iOS, macOS, Windows, and Tizen) while enabling you to craft consistent brand experience through rich styling and graphics. Out of the box, each platform looks and behaves the way it should without any additional widgets or styling required.

As part of .NET 7, .NET MAUI provides a single project that handles  multi-targeting across devices and their platforms. To learn more about the productivity improvements, tooling, and performance enhancements check out these resources:

- [Introducing .NET MAUI – One Codebase, Many Platforms](https://devblogs.microsoft.com/dotnet/introducing-dotnet-maui-one-codebase-many-platforms/)
- [Productivity comes to .NET MAUI in Visual Studio 2022](https://devblogs.microsoft.com/dotnet/dotnet-maui-visualstudio-2022-release/)
- [Performance Improvements in .NET MAUI](https://devblogs.microsoft.com/dotnet/performance-improvements-in-dotnet-maui/)
- [.NET Conf Focus on MAUI - That's a wrap!](https://devblogs.microsoft.com/dotnet/dotnet-conf-focus-on-maui-recap/)

## Cloud Native
A set of best practices for building your apps for and in the cloud to take advantage of resilience, scalability, efficiency, and velocity.

.NET is a great choice to build cloud native apps. To learn more about cloud native features and improvements in .NET 7, check out these resources:

- [Announcing built-in container support for the .NET SDK](https://devblogs.microsoft.com/dotnet/announcing-builtin-container-support-for-the-dotnet-sdk/)
- [Announcing gRPC JSON transcoding for .NET](https://devblogs.microsoft.com/dotnet/announcing-grpc-json-transcoding-for-dotnet/)
- [.NET 7 comes to Azure Functions & Visual Studio 2022](https://devblogs.microsoft.com/dotnet/dotnet-7-comes-to-azure-functions/)

## ARM64
ARM provides the best of both worlds; a small chip with a giant leap of exceptional performance and unbelievable power efficiency.

.NET helps you build applications to run on ARM devices. For more information on how fast .NET 7 runs on ARM64, check out these resources:

- [Arm64 Performance Improvements in .NET 7](https://devblogs.microsoft.com/dotnet/arm64-performance-improvements-in-dotnet-7/)

## Modernization
Being on a modern version of .NET lets you take advantage of lightning fast performance with a plethora of new features improving your developer quality of life.

To make the upgrade experience as seamless as possible, the .NET Upgrade Assistant provides you with a guided step-by-step experience to modernize your .NET applications by analyzing and upgrading your project files, code files, and dependencies.

For more information on how .NET 7 helps you modernize your applications, check out these resources:

- [Incremental ASP.NET to ASP.NET Core Migration](https://devblogs.microsoft.com/dotnet/incremental-asp-net-to-asp-net-core-migration/)
- [Migrating from ASP.NET to ASP.NET Core in Visual Studio](https://devblogs.microsoft.com/dotnet/introducing-project-migrations-visual-studio-extension/)

## Performance
.NET is fast. .NET 7 is the fastest .NET yet. Over a thousand performance-impacting improvements went into .NET 7 impacting reflection, On Stack Replacement(OSR), start-up time, Native AOT, loop optimizations and many other areas.

For more information on why .NET 7 is the fastest version yet, check out these resources:

- [Performance Improvements in .NET 7](https://devblogs.microsoft.com/dotnet/performance_improvements_in_net_7/)
- [Regular Expression Improvements in .NET 7](https://devblogs.microsoft.com/dotnet/regular-expression-improvements-in-dotnet-7/)

## Contributor spotlight: Filip Navara

A huge "Thank you" goes out to all our community members. We deeply appreciate your thoughtful contributions. We asked contributor [@filipnavara](https://github.com/filipnavara) to share his thoughts.

![Filip Navara](./filip_navara.jpeg)

In Filip's own words:

I started playing with computers as a kid. While visiting my grandpa, I often saw him doing his work in BASIC. He was writing factory automation software, and I inherited my love for all things technical from him. DOS was the standard system back then, and Borland dominated the programming tools. I wanted to understand how programming works and to learn it. Stubbornly I refused all his advice and had to learn everything myself by trial and error. It was foolish, but seeing the little programs come to life was fun.

Gradually I started programming in different languages, exploring the internet and then the world of open source. I mostly enjoyed coding on low-level software such as compilers, operating systems, or system emulators. In my spare time during high school, I contributed to projects like Wine, ReactOS, QEMU, Binutils, and the MinGW compiler tool set.

When the first version of .NET Framework came out, I was immediately intrigued. It promised the simplicity of Delphi that I was familiar with, and the C# language was genuinely fun to learn. The timing was perfect since I joined my friends to start a small project to develop an email client application, and we all agreed to build it in .NET. That application, eM Client, kept me occupied throughout my university studies. It is still my current project to this day; even though the team has grown, my responsibilities have shifted, and we've got a lot of very talented programmers to relieve me of my duties.

Open sourcing of .NET was a big blessing for us and made many things easier. Nowadays, I can focus more on side projects, and contributing to .NET is a natural choice. It allows me to use my knowledge to the full extent, from the low-level details of hardware, and operating system internals, to the high-level frameworks that our email application builds on.

The open code allowed me to drive a project to port WinForms framework to macOS (based on Mono code but using Cocoa native controls in many places). I started contributing more when .NET 5 unification plan started taking place. It was always a pain point for us that different platforms like Xamarin.Mac and Mono lagged in the supported features behind the .NET we used on Windows. Initially, I started to fill the gaps in Mono base class library, which was already sharing some code with .NET Core. I realized that this game of catching up might not be the best solution, so I started exploring other options like running Xamarin.Mac on CoreCLR. It happened to take place just days before the first MonoVM (Mono runtime in .NET 5+) commits were written. Once I realized what was happening, I jumped on board that plan. All that work happened hidden in the open on GitHub, with an official announcement coming a few months later at the Build conference. It was thrilling to see the progress, to build my own Xamarin runtime build that ran on this early unified MonoVM runtime, that showed the first UI. Eventually, it started even our email client application. It was indeed a game changer for us. With the old .NET Framework, we could not use the new features when they were released. It took years before the deployment of new releases caught up. Now I was in the opposite situation, running bleeding edge bits before everyone else!

This work on the runtime unification has now successfully concluded, and we released our application with the latest .NET 6 bits to our customers. However, many places in .NET can still be improved, and I enjoy working with the people on the .NET team. I try to drive at least one minor feature for each release. For .NET 6, I focused on getting the iOS cryptography stack working. For .NET 7, I took a stab at a niche API for handling Negotiate/Kerberos/NTLM authentication with the great help of the networking team. While it's not a very attractive or visible feature, it was long-term technical debt. The code was lacking in the unit and functional tests; ASP.NET accessed the internals through reflection, which is not NativeAOT friendly; and most importantly, library authors had to use complicated ways to get around the lack of simple public API.

I sincerely hope to contribute more in the future, and I enjoy seeing other contributors finding their areas of interest and making the whole platform better for everyone!

## Support

.NET 7 is a **Short Term Support (STS)** release, meaning it will receive free support and patches for 18 months from the release date. It's important to note that the quality of all releases is the same. The only difference is the length of support. For more about .NET support policies, see the [.NET and .NET Core official support policy](https://dotnet.microsoft.com/platform/support/policy/dotnet-core).

We recently changed the "Current" name to "Short Term Support (STS)". We're in the [process of rolling out that change](https://github.com/dotnet/core/pull/7517).

## Roadmaps

Releases of .NET include products, libraries, runtime, and tooling, and represent a collaboration across multiple teams inside and outside Microsoft. You can learn more about these areas by reading the product roadmaps:

* [ASP.NET Core 7 and Blazor Roadmap](https://github.com/dotnet/aspnetcore/issues/39504)
* [EF 7 Roadmap](https://docs.microsoft.com/ef/core/what-is-new/ef-core-7.0/plan)
* [ML.NET](https://github.com/dotnet/machinelearning/blob/main/ROADMAP.md)
* [.NET MAUI](https://github.com/dotnet/maui/wiki/Roadmap)
* [WinForms](https://github.com/dotnet/winforms/blob/main/docs/roadmap.md)
* [WPF](https://github.com/dotnet/wpf/blob/main/roadmap.md)
* [NuGet](https://github.com/NuGet/Home/issues/11571)
* [Roslyn](https://github.com/dotnet/roslyn/blob/main/docs/Language%20Feature%20Status.md)
* [Runtime](https://github.com/dotnet/core/blob/main/roadmap.md)

## Closing

We appreciate and [thank you](https://dotnet.microsoft.com/thanks) for your all your support and contributions to .NET. Please [give .NET 7 Release Candidate 1 a try](https://dotnet.microsoft.com/download/dotnet/7.0) and tell us what you think!
