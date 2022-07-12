---
post_title: .NET Core 3.1 will reach End of Support on December 13, 2022
author1: dwhittaker
post_slug: net-core-3-1-will-reach-end-of-support-on-december-13-2022
username: dwhittaker
microsoft_alias: dwhittaker
featured_image: dotnet-bot_handybot.png
categories: .NET
summary: .NET Core 3.1 will reach end of support on December 13, 2022, this blog breaks down all the valuable information you need to know and how to update to .NET 6.0.
desired_publication_date: 2022-07-12
---

.NET Core 3.1 will reach end of support on December 13, 2022. After that date, Microsoft will no longer provide servicing updates or technical support for .NET Core 3.1. We recommend moving to .NET 6 as soon as possible. If you are still using .NET Core 3.1 after the end of support date, you'll need to update your app to .NET 6 or .NET 7 to remain supported and continue to receive .NET updates.

.NET Core 3.1 apps will continue to run after the end-of-support date. Nothing about them will change. However, every [security fix in .NET 6](https://github.com/dotnet/core/blob/main/release-notes/6.0/cve.md) after the end of support date is a potential documented and unpatched security vulnerability for .NET Core 3.1 apps.

You can learn more about .NET release policies:

- [.NET releases](https://github.com/dotnet/core/blob/main/releases.md)
- [.NET release policies](https://github.com/dotnet/core/blob/main/release-policies.md)

## Update your application

If your application uses NET Core 3.1, we strongly recommend you migrate your application to .NET 6 - a supported LTS version. You can download .NET 6 from the [.NET website](https://dotnet.microsoft.com/download/dotnet/6.0).

If you're an end user, we recommend reaching out to the vendor managing your software to confirm whether an updated version of the software is needed and available. The remainder of this post is applicable to software vendors and developers.

### Upgrading to .NET 6

* Open the project file (the *.csproj, *.vbproj, or *.fsproj file).
* Change `<TargetFramework>netcoreapp3.1</TargetFramework>` to `<TargetFramework>net6.0</TargetFramework>`.

You may also want to review the [.NET 6 Compatibility Guide](https://docs.microsoft.com/dotnet/core/compatibility/6.0).

### Update your development environment

In addition to the software you ship to your customers, the computer you use for development may have .NET Core 3.1 installed - either standalone or installed by Visual Studio.

You can check for stand-alone installations of .NET Core 3.1 from the command line. On a Windows computer, open a Command Prompt and go to %ProgramFiles%dotnet folder. On macOS or Linux, open a terminal window.

Then type the following command: dotnet --list-runtimes
  
![Screen Shot 2022-06-24 at 8 32 04 AM](https://user-images.githubusercontent.com/94140381/175607526-4c5f3f6b-2999-4e5f-bae4-71f2637dee4b.png)

If you use Visual Studio 2019 16.11 or 17.0 or Visual Studio 2022 17.2, then based on the workloads installed, you may also have .NET Core 3.1 installed as a required component of Visual Studio and you need to be aware of some relevant changes that are coming.

![workloads](https://user-images.githubusercontent.com/94140381/175608244-161de4ce-6078-4179-af59-e0cad8763bb6.png)

![dotnet components](https://user-images.githubusercontent.com/94140381/175608256-230e636d-7675-4c39-bafb-61212aa1948d.png)


Starting with the December 2022 servicing update for Visual Studio 2019 16.11, Visual Studio 2019 17.0, and Visual Studio 2022 17.2, the .NET Core 3.1 component in Visual Studio will be changed to out of support and optional. This means that workloads in Visual Studio may be installed without installing .NET Core 3.1. *Note* that existing installations won’t be affected and any previously installed workload and component will remain installed until the component or workload is unselected in Visual Studio setup. While it's possible for you to re-select this optional component in Visual Studio and re-install this, we strongly recommend you use .NET 6 with Visual Studio 2022 to build apps that run on a supported .NET runtime.

*Note:* If you're migrating an app to .NET 6, some breaking changes might affect you. We recommend you to go through the [compatibility check](https://docs.microsoft.com/dotnet/core/compatibility/6.0).
  
## Useful Links

* [.NET downloads](https://dotnet.microsoft.com/download/dotnet)
* [.NET Compatibility](https://docs.microsoft.com/dotnet/core/compatibility/)
* [.NET Deployment](https://docs.microsoft.com/dotnet/core/deploying/)
* [.NET Support Policy](https://dotnet.microsoft.com/platform/support/policy/dotnet-core)
