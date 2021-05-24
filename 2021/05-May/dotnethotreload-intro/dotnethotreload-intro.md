---
post_title: 'Introducing the .NET Hot Reload experience for editing code at runtime'
username: DmitryLyalin
microsoft_alias: dmitryly
featured_image: 2021/05-May/dotnethotreload-intro/dotnetblog_hotreload_hero.png
categories: .NET, .NET Core, .NET Framework
summary: Introducing the new .NET Hot Reload user experience for editing managed code at runtime, now available through the Visual Studio 2019 debugger and dotnet watch.
desired_publication_date: 05/25/2021
---

Today, we are excited to introduce you to the availability of the .NET Hot Reload experience in Visual Studio 2019 version 16.11 (Preview 1) and through the `dotnet watch` command-line tooling in .NET 6 (Preview 4). In the rest of this blog post, we’d like this opportunity to walk you through what is .NET Hot Reload, how you can get started using this feature, what our vision is for future planned improvements and clarity on what type of edits and languages are currently supported.

## What is .NET Hot Reload?

With Hot Reload you can now modify your apps managed source code while the application is running, without the need to manually pause or hit a breakpoint. Simply make a supported change while your app is running and in our new Visual Studio experience use the “apply code changes” button to apply your edits.

![.NET Hot Reload, apply code changes in Visual Studio 2019](2021/05-May/dotnethotreload-intro/dotnetHotReloadVS2019version16_11.gif)

Hot Reload works with many of your existing and upcoming project types such as WPF, Windows Forms, .NET MAUI previews, ASP.NET Core apps code-behind, Console applications, WinUI 3 (managed debugger required) and many others. This support is very broad with the core experience working with any project that is powered by .NET Framework or CoreCLR runtimes. 

With Hot Reload our goal is to make this experience available no matter how you prefer to launch your app. With today’s release you can now use this experience through the fully integrated Visual Studio debugger experience or the `dotnet watch` command-line tool, with more options to come in later releases.

## Getting Started

To get started you have the option of either using Visual Studio’s newest preview release or our .NET 6 Preview 4, instructions below.

**Visual Studio:**

To try Hot Reload in Visual Studio when using the debugger:

* Download and install Visual Studio 2019 version 16.11 Preview 1
* Open a supported project type, for example a WPF app
* Launch the app with the debugger attached through F5 (make sure that “enable native code debugging” is disabled in debugger settings/debug launch profile)
* Open a C# code file with some code that can be re-executed through the running apps user interface (example: code-behind of a button or a ViewModel’s command) or something that is being triggered at an interval through a timer and change the code
* Apply the code changes using the new `apply code changes` (ALT-F10) button in your Visual Studio toolbar (next to the `Continue` button). Note that saving the files is not required when using Visual Studio, giving you the flexibility to quickly change code and keep going.

If the change you made is supported your app will now be patched while its running with your new logic and you should see the changes in your app’s behavior the next time the updated code is re-executed by either your action or by something like a timer triggering the code.

You can also continue to use other debugger features such as breakpoints, Edit & Continue, XAML Hot Reload, etc. Everything that you are used to today should fully work in parallel with .NET Hot Reload. If something does not work for you, please let us know!

**CLI**

To try Hot Reload from the command-line when launching your app using dotnet watch:

* Install .NET 6 Preview 4
* Update your existing ASP.NET Core project to target .NET 6
* Add the "hotReloadProfile": "aspnetcore" property to your apps launch profile in launchSettings.json. 

Example of Properties/launchSettings.json:

```json
{
  "profiles": {
    "dotnet": {
      "commandName": "Project",
      "hotReloadProfile": "aspnetcore"
    }
  }
}
```

* Run the project using `dotnet watch` and look at the output as it should indicate that hot reload is enabled
* Make a supported code change to your apps managed source code and save the file to apply

Just like with the Visual Studio experience your new logic should now be applied and you should see the changes in your app’s behavior the next time the updated code is re-executed.

You can also use this approach with your Blazor WebAssembly projects by modifying the "blazorwasm" hot reload profile and following similar steps above. You can even try it with a Windows Forms or other types of projects powered by CoreCLR, just manually add a file named launchSettings.json under the Properties folder with the above example contents.

This experience is still in development, and we are tracking future improvement that will make it easier to use dotnet watch to Hot Reload all types of .NET Core apps without launchSettings.json files, but this is a limitation in the current release.

## Best in Visual Studio 2022 & .NET 6

With today’s release this is just a preview of our full vision of Hot Reload for .NET developers. While some capability is being made available in early .NET 6 previews and in Visual Studio 2019, for the full power of this feature we are targeting .NET 6 (and future releases of .NET) and Visual Studio 2022 as the set of frameworks and tooling that will have the most complete and optimized experience.

To give you a glance into what type of features we plan to deliver in future previews and our final release here are some examples:

* **.NET Multi-platform App UI (.NET MAUI):** With .NET 6 Preview 4 developers building .NET MAUI applications can now use .NET Hot Reload with projects targeting WinUI 3. In future releases we’re bringing .NET Hot Reload support to iOS, Android, and Mac Catalyst scenarios.
* **Razor Pages:** In future releases Hot Reload and Edit and Continue (EnC) will be supported for editing Razor for websites or Blazor apps targeting .NET 6 or higher 
* **No Debugger Required in Visual Studio:** In future release of Visual Studio 2022 we are working to add support to use Hot Reload without needing the debugger, this means developers will be able to launch their apps using CTRL-F5 and still use Hot Reload to patch their running app
* **Reducing the number of unsupported changes:** In future releases of Visual Studio 2022 and .NET 6+ we’re planning work across multiple teams that will reduce the number of unsupported edits at runtime 
* **Optimizing frameworks to work best with Hot Reload:** In .NET 6 we’re investigating how certain frameworks can be improved to better support Hot Reload. Examples of this in the future will include improvements to ASP.NET Core, .NET MAUI and other frameworks where tweaks and optimization will make Hot Reload changes more useful in more situations.

While the above are our current plans, note that plans can change based on customer feedback and schedule.

## Supported/Unsupported changes and languages
No matter how you use .NET Hot Reload please be aware that some changes are not supported at runtime and will prompt you with a `rude edit` dialog and require you to restart your app in order to apply. We're still working on the feature and the documentation to detail what edits are supported. For now, start by reviewing our [existing list of Edit and Continue (EnC) equivalent capabilities](https://github.com/dotnet/roslyn/blob/main/docs/wiki/EnC-Supported-Edits.md). Since Hot Reload is powered by EnC this will give you a good starting point for better understanding this new feature. For details see: [EnC documentation](https://docs.microsoft.com/visualstudio/debugger/edit-and-continue?view=vs-2019).

Also, while the above examples are specifically mentioning C#, Visual Basic is also supported in various situations when running under Visual Studio’s debugger. F# is currently not supported in .NET 6 but we are planning to support in a future release based on customer feedback.

## Your feedback matters
In this early preview release, we want to acknowledge that **there will be bugs**. Sometimes if you try to apply a change it might silently fail, or your app might crash, etc. If you do encounter any problems, please take a moment to report issues to us, as only with your feedback can we ensure that critical problems are resolved, and future decisions are prioritized based on your input. 

To reach us please use the [Visual Studio feedback mechanism](https://docs.microsoft.com/visualstudio/ide/feedback-options?view=vs-2019).
