---
post_title: MSBuild and 64-bit Visual Studio 2022
username: raines
microsoft_alias: raines
featured_image: ./msbuildx64.png
categories: .NET, Visual Studio
summary: Learn how the shift to 64-bit Visual Studio can impact your builds
desired_publication_date: 2021-11-11
---

Visual Studio’s [shift to 64-bit](https://devblogs.microsoft.com/visualstudio/visual-studio-2022/#visual-studio-2022-is-64-bit) means your builds in Visual Studio 2022 will run in a 64-bit MSBuild. This will not cause any problems for most people. However, if your build includes a task that is 32-bit only and does not correctly mark itself as a 32-bit task, your build may fail.

The best way to know if this will affect you is by testing your build with a 64-bit version of MSBuild. Visual Studio and Build Tools have included 64-bit MSBuild since Visual Studio 2013, so you can do this in your current version of Visual Studio, as well as with the Visual Studio 2022 previews.

## Background and what’s changed

MSBuild is part of both Visual Studio and the .NET SDK. The changes described here affect only the Visual Studio MSBuild and will not affect builds started through `dotnet build`.

MSBuild has both 32- and 64-bit executables. Both are installed in every copy of Visual Studio and Visual Studio Build Tools, and scripts that call `msbuild.exe` by full path can select which to use. The Developer Command Prompt for Visual Studio also sets `PATH` to include the MSBuild bin folder.

Visual Studio 2019 is the last version of Visual studio that used the 32-bit version of MSBuild for builds within Visual Studio. Since Visual Studio 2022 is now 64-bit and runs MSBuild in-process, it now runs a 64-bit version of MSBuild, including when you hit F5 or Ctrl-F5. Consistent with the change to 64-bit, the Visual Studio Developer Command Prompt now points to the 64-bit version of MSBuild in the `PATH` (the 32-bit version was on the `PATH` in earlier releases).

The 32-bit MSBuild is run by default in the Azure Pipelines [Visual Studio Build](https://docs.microsoft.com/azure/devops/pipelines/tasks/build/visual-studio-build?view=azure-devops) and [MSBuild](https://docs.microsoft.com/azure/devops/pipelines/tasks/build/msbuild?view=azure-devops) tasks. To change this, specify `msbuildArchitecture: 'x64'` in your YAML job definition. For GitHub Actions, the [setup-msbuild action](https://github.com/marketplace/actions/setup-msbuild) allows for specifying this same setting using `msbuild-architecture: 'x64'`.

## How does this affect my build?

Changes in Windows between environments may cause problems, if part of your build has a dependency on the 32-bit [filesystem](https://docs.microsoft.com/windows/win32/winprog64/file-system-redirector) or [registry](https://docs.microsoft.com/windows/win32/winprog64/registry-redirector) redirection.

Most MSBuild tasks build for `AnyCPU` and should not have a problem running in a 64-bit environment. Many tasks invoke tools via the command line (they “shell out”) and to those tools it will make no difference whether the task is 32- or 64-bit since the tool continues to run in its own process.

Some tasks p/invoke into native libraries and thus are sensitive to the architecture they run in.

If you maintain a task, you do not have to rewrite it to run in a 64-bit environment. Instead, you can mark it so that MSBuild runs it in a process with the correct bitness: the 64-bit version of MSBuild can start 32-bit tasks out of process, as the 32-bit version can start 64-bit tasks. See “Guidance for Task Owners” below.

Don’t feel alone if you encounter this—even the [Visual Studio SDK](https://docs.microsoft.com/visualstudio/extensibility/visual-studio-sdk) had issues with 64-bit MSBuild compatibility. Some of its tasks are thin wrappers over libraries written in C++ and built for 32-bit x86. We discovered this early in the Visual Studio 2022 lifecycle and the Visual Studio SDK has been updated to support 64-bit MSBuild using the techniques described below since [Microsoft.VSSDK.BuildTools 16.8.1015](https://www.nuget.org/packages/Microsoft.VSSDK.BuildTools/16.8.1015).

## Potential issues with MSBuild tasks

MSBuild tasks are .NET assemblies that add extra functionality to MSBuild. You use them in your normal build, even if you have never written one. MSBuild tasks normally run inside the parent MSBuild process.

If a task is not compatible with 64-bit MSBuild, the task may not be found or it may throw an error. Contact the task owner (such as by filing an issue in their repo), possibly referencing this breaking change blog post. If _you_ are the task owner, see “Guidance for task owners” below.

Tasks incompatible with 64-bit MSBuild may fail in a variety of ways; the most common are [MSB4018](https://docs.microsoft.com/visualstudio/msbuild/errors/msb4018) with a `System.DllNotFoundException` and [MSB4062](https://docs.microsoft.com/visualstudio/msbuild/errors/msb4062) “task could not be loaded” errors.

## Guidance for task (NuGet package) owners

You have the option of **rewriting your task to support running in a 32- or 64-bit environment** and deploying both copies of native assemblies, but this is often difficult, so you may prefer to **configure MSBuild to run your task in a 32-bit process** (even from a 64-bit build).

Because this requires starting a new process and communicating with it, it takes more time during the build than running a task in the existing MSBuild process. This is usually not a problem unless the task is called many, many times in your build.

Tasks are made available for use with [`UsingTask`](https://docs.microsoft.com/visualstudio/msbuild/usingtask-element-msbuild) elements. You can [configure the task](https://docs.microsoft.com/visualstudio/msbuild/how-to-configure-targets-and-tasks#usingtask-attributes-and-task-parameters) to indicate that it requires a specific runtime environment by specifying the `Architecture` attribute in the `UsingTask` element.

A well-specified `UsingTask` for a 32-bit-only assembly looks something like this:

```xml
<UsingTask TaskName="TaskThatNeedsX86Library"
           AssemblyFile="$(MSBuildThisFileDirectory)\ArchSpecificTasks.dll"
           Architecture="x86" />
```

This change is **backward compatible** since MSBuild has supported running tasks in a different architecture since .NET 4.5 and Visual Studio 2012. Specifying `Architecture` will not change how the task runs in a 32-bit MSBuild environment, but will cause 64-bit MSBuild to run it in a secondary 32-bit process. Because of this, we recommend making this change unconditionally and not trying to detect Visual Studio 2022 (MSBuild 17) specifically.

### Testing

We recommend testing your task in a few build scenarios to make sure your changes are working:

1. UI-driven builds in Visual Studio 2019 (to ensure that there hasn’t been a regression in that scenario).
2. Command-line builds using 32-bit `MSBuild.exe` from Visual Studio 2019 (MSBuild 16).
3. Command-line builds using 64-bit `MSBuild.exe` from Visual Studio 2019 (MSBuild 16).
4. UI-driven builds in the latest Visual Studio 2022 (this is a primary developer scenario going forward).
5. Command-line builds using 64-bit `MSBuild.exe` from Visual Studio 2022 (MSBuild 17).
6. Command-line builds using the .NET SDK (if your task supports this environment). These use `dotnet build` and should be unaffected by your changes.

### Known issues

If a task is defined in a .NET assembly compiled as 32-bit only, MSBuild will fail to load it with an error like

```text
S:\BitnessInMSBuild\ShowErrors.proj(13,5): error MSB4062: The "TaskCompiledForx86" task could not be loaded from the assembly S:\BitnessInMSBuild\TaskCompiledForx86\bin\Debug\net472\TaskCompiledForx86.dll. Could not load file or assembly 'file:///S:\BitnessInMSBuild\TaskCompiledForx86\bin\Debug\net472\TaskCompiledForx86.dll' or one of its dependencies. An attempt was made to load a program with an incorrect format. Confirm that the <UsingTask> declaration is correct, that the assembly and all its dependencies are available, and that the task contains a public class that implements Microsoft.Build.Framework.ITask.
```

_even if the `UsingTask` correctly declares an `Architecture`_. This is tracked by [dotnet/msbuild#6461](https://github.com/dotnet/msbuild/issues/6461).

You can work around this issue by recompiling your task assembly as AnyCPU and additionally specifying the `Architecture`. In that case, the MSBuild engine can _load_ the task in a 64-bit environment but it will only _execute_ it in a 32-bit process.

## Mitigation for users of tasks that they don’t own

Unfortunately, it is difficult to work around task misconfiguration if you do not control the `UsingTask`. [dotnet/msbuild#5541](https://github.com/dotnet/msbuild/issues/5541) tracks a change that would make it easier to override an incorrect `UsingTask` in your project. Please let us know if this would be useful to you in a future Visual Studio/MSBuild release.

## Next Steps

The upgrade to Visual Studio 2022 is an exciting one. 64-bit MSBuild is just one of the new features we've prepared for you and we're excited for you to try them out. When you do, be sure to leave your feedback below, we'd love to hear about your experiences with the upgrade and with 64-Bit MSBuild overall!
