---
post_title: Introducing the .NET Upgrade Assistant Preview
username: cathys@microsoft.com
microsoft_alias: cathys@microsoft.com
categories: .NET 
desired_publication_date: 3/2/2021
summary: Move your .NET Framework applications to .NET 5 at your own pace with confidence. 
---

Today we're excited to introduce a tool we've been working on to help you upgrade your .NET Framework-based applications to .NET 5 called the [.NET Upgrade Assistant](https://aka.ms/dotnet-upgrade-assistant). The .NET Upgrade Assistant is a .NET global command-line tool that gives you a guided experience for incrementally upgrading your applications. It brings an existing tool called [try-convert](https://github.com/dotnet/try-convert) together with step-by-step instructions and recommendations. This means you can instead focus your upgrading time on the more specific changes to your application and the application model rather than on the repetitive tasks across the projects in your solution.

Currently in preview, the .NET Upgrade Assistant determines which projects need to be upgraded and recommends the order they should be upgraded in. The tool automates more tasks in the upgrade process that you would normally do manually like upgrading to the new SDK-style project format, re-targeting them to .NET 5 and updating NuGet package dependencies. The tool also provides you with recommendations and fixes for project files, configuration, and source code. The difference between this tool and others is that you can see the recommendations every step of the way and you can choose what and how your code is upgraded.

The tool supports multiple project types. It's ready to help you upgrade your ASP.NET MVC, Windows Forms, and WPF apps, in addition to console apps and libraries. We'll add more project types in later previews.

[cta-button align='center' text='Get started with the .NET Upgrade Assistant' url='https://aka.ms/dotnet-upgrade-assistant'  color='#D83B01']

## Why did we build this?

We heard from customers that they want to upgrade, but it's currently too time consuming with our current set of porting tools, particularly for large ASP.NET applications. Companies typically want to move to .NET Core / .NET 5 so they can host applications on Linux and benefit from performance gains to cut hosting and compute costs. When companies are planning their modernization strategies, often the costing is started by taking one application through the upgrade process as a proof of concept. If that takes too long, then the cost of upgrading everything is too high. The .NET Upgrade Assistant aims to cut the time considerably and automate away more of the manual work.

We worked closely with Optimizely (Episerver) to upgrade their large ASP.NET application portfolio and built this tool with their close collaboration. Their solutions include content management, e-commerce and digital marketing platforms. With their help we refined how the .NET Upgrade Assistant works and added extensibility points so that they could hook their own upgrade guidance and code patterns into the assistant. This makes it easier for their customers to also upgrade their solutions as well. You can [read more on their website](https://aka.ms/dotnet-upgrade-assistant-episerver).

## How does it work?

We recommend first becoming familiar with the [overall upgrading process](https://docs.microsoft.com/dotnet/core/porting/) and [determine upgrade feasibility](https://github.com/dotnet/upgrade-assistant#determining-upgrade-feasibility). Once you've done that, you're ready to [install](https://github.com/dotnet/upgrade-assistant#installation) the .NET Upgrade Assistant and to start upgrading your projects to .NET 5. If you point it at a solution, it will analyze your dependencies and determine the order of projects to upgrade. Here's what it will do.

1. **Backup your projects.** Ensuring you have a backup (and your code is in source control), means you can easily rollback to a working state if you need to at any point. Remember, once the tool is finished running, it will be partially upgraded to .NET 5. You'll still need to make some manual changes.

2. **Update the projects to be SDK-style projects.** .NET 5 uses a simplified and different project file format compared to .NET Framework. This step uses the [try-convert](https://github.com/dotnet/try-convert) tool that was installed as part of the pre-requisites to move all your projects over to the new [SDK-style project](https://docs.microsoft.com/dotnet/core/project-sdk/overview) format.

    Prior to the .NET Upgrade Assistant, you would have had to determine yourself, the best order to upgrade your projects based on their dependencies. Imagine you had 20 projects in a solution. You would have had to build a dependency graph to understand the dependencies between the projects, figure out the best order to upgrade them, and then run the `try-convert` tool against each project. The tool, particularly at this step, saves you so much time and effort, especially if you have very large solutions with a lot of projects in it.

3. **Update the target framework (Update TFM).** At this step, the tool will re-target your projects to target .NET 5.0. Read more about [target frameworks](https://docs.microsoft.com/dotnet/standard/frameworks).

4. **Update NuGet packages in your projects.** Here, NuGet package dependencies are updated to versions that are compatible with .NET 5.0. There are a few rules that are applied to make these updates:

    - It removes package references that appear to be transitive so that only top-level dependencies are included in the csproj.
    - If a referenced NuGet package isn\'t compatible with the target NET version and a newer version of the NuGet package is, the package automatically updates the version to the first major version that will work.
    - It will replace NuGet references based on specific replacement packages listed in configuration files.

     Additional packages are now added and referenced, such as the [Windows Compatibility Pack](https://docs.microsoft.com/dotnet/core/porting/windows-compat-pack) for Windows apps. For ASP.NET Framework apps, some packages are removed that aren't need anymore (e.g. structuremap.web and Autofac.Mvc5). The .NET Upgrade Assistant analyzer package is added here too, which will help fix specific parts in your code that aren't compatible in .NET 5 in the following steps. These analyzers are specific to project types and provide you with compile time feedback for anything the tool couldn't update.

5. **Add template files.** For some application models like ASP.NET apps, common template files (like `Program.cs` and `Startup.cs`) are missing in older framework app versions, so they are added at this step and simple updates are also made based on recognized `web.config` or `app.config` values.

6. **Update C\# source.** Now that all your projects have been upgraded to .NET 5 and references upgraded when the tool could help, it's time to make some specific fixes to the C\# code. Right now, the tool offers a set of ASP.NET analyzers, which will apply fixes for known patterns that where in .NET Framework that have .NET 5 equivalents. You can pick and choose the analyzers you want to run on your code or run all of them to catch any usages of older .NET Framework patterns. Some examples for ASP.NET include:

    - Apply fix for AM0001: ASP.NET Core projects should not reference ASP.NET namespaces
    - Apply fix for AM0002: HtmlString types should be replaced with Microsoft.AspNetCore.Html.HtmlString
    - Apply fix for AM0003: ActionResult types should come from the Microsoft.AspNetCore.Mvc namespace

    The team will add analyzers for more project types in upcoming previews.

7. **Move to next project.** If your solution includes multiple projects, this is where you'd move on to working on the next project that was recommended in the beginning.

Remember, that the tool is not intended to completely upgrade .NET Framework applications to .NET 5. Manual changes will still need to be made because these require knowledge of the app and how it's intended to function. We recommend that once you're finished in the tool, you head back to Visual Studio and attempt to build your code. Look for warnings in the Error List window of Visual Studio to help you identify other parts
of your code that might not be supported in .NET 5. You should also check any NuGet packages that might have been updated as sometimes there are breaking changes and you may need to revert to an older version.

You can find [documentation and samples](https://github.com/dotnet/upgrade-assistant/wiki) on using the
.NET Upgrade Assistant in the repo. Also, you can watch my session from [.NET Conf: Focus on Windows](https://focus.dotnetconf.net/) last week where I walked through the process with Windows desktop apps.

<iframe width="560" height="315" src="https://www.youtube.com/embed/E0_yKL8nzUk?start=45" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe>

## Give us feedback

The .NET Upgrade Assistant is open source on GitHub and we're currently taking feedback. We'll be open for code contributions very soon. Head on over to <https://github.com/dotnet/upgrade-assistant> to try out the tool and engage with the team.

Happy upgrading!
