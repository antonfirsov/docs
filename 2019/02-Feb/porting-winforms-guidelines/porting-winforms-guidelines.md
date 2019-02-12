# How to port desktop applications to .NET Core 3.0

In this post, I will describe how to port a desktop application from .NET
Framework to .NET Core. I picked a WinForms application as an example but steps for WPF applications are similar. I will also show how you can keep using the WinForms
Designer in Visual Studio even though it is under development and is not yet
available for .NET Core projects.

## About the sample

For this post, I'll be using a [Memory-style][memory-game] board game
application. It contains a WinForms UI (`MemoryGame.exe`)
Framework 4.5) and a class library with the game logic
(`MemoryGame.Logic.dll`), both targeting .NET Framework 4.5. I'll be porting the
application project to .NET Core 3.0 and the class library to .NET Standard 2.0.
Using .NET Standard instead of .NET Core allows me to reuse the game logic to
provide the application for other platforms, such as iOS, Android or the web.

You can either watch Scott Hunter and me doing the conversion in this video, or
you can follow the step-by-step instructions below. Of course, you don't
get a penalty for doing both.

**ToDo: link to the video.**

## Step-by-step process

>I suggest doing migration in a separate branch or, if you're not using version
>control, creating a copy of your project so you have a clean state to go back
>to if necessary.

Before porting the application to .NET Core 3, I need to do some preparation
first.

## Preparing to port

1. **Install [.NET Core 3][core-installation]** and Update Visual Studio to 2019
   Preview version (Visual Studio 2017 will only support up to .NET Core 2.2).

1. **Start from a working solution**. Ensure the solution opens, builds, and
   runs without any issues.

1. **Update NuGet packages**. It is always a good practice to use the latest
   versions of NuGet packages before any migration. If your application is
   referencing any NuGet packages, update them to the latest version. Ensure
   your application builds successfully. In case of any NuGet errors, downgrade
   the version and find the latest one that doesn't break the application.

1. **Run [.NET Portability Analyzer][api-port]** to determine if there are any
   APIs your application depends on that are missing from .NET Core. In case
   there are, you need to refactor your code to avoid dependencies on not
   supported in .NET Core APIs. Sometimes it's possible to find an alternative
   APIs that provide needed functionality. Another option would be to move all
   code containing .NET Framework-only APIs to a separate project and leave that
   project on .NET Framework while migrating the rest to .NET Core.

1. **Replace `packages.config` with `PackageReference`**. If your project uses
   NuGet packages, you will need to add the same NuGet packages to the new .NET
   Core project. .NET Core projects support only `PackageReference` for adding
   NuGet packages. To move your NuGet references from `packages.config` to your
   project file, right-click on `packages.config` -> **Migrate packages.config
   to PackageReference...**.

   You can learn more about this migration in our [docs][pkg-config].

1. **Ensure your dependencies are supported in .NET Standard**. If your project
   has any references, we recommend to check if they are supported in .NET
   Standard before performing any porting. The easiest way to check is to create
   a new .NET Core project with the same references and make sure it builds.
    1. Create new console application targeting .NET Core 3.
    1. In the project file copy all references from the old project, for example:

       NuGet package reference

       ```xml
       <PackageReference Include="Microsoft.Windows.Compatibility" Version="2.0.1" />
       ```

       Project reference

       ```xml
       <ProjectReference Include="..\MatchingGame.Core\MatchingGame.Core.csproj" />
        ```
    1. Build.

       > If you get any NuGet restore errors, some of the packages you are
       referencing probably support only .NET Framework. In that case you can
       use the `Contact` form on the [NuGet gallery][nuget-org] and let the
       author know that you'd be interested in seeing the package being updated
       to .NET Standard.

1. **Migrate to the SDK-style .csproj file**. To move my application to .NET
   Core, I need to change my project file to SDK-style format because the old
   format does not support .NET Core. Besides, the SDK-style format is much
   leaner and easier to work with. Simply replace your current `.csproj` file
   with the `.csproj` file from the project you created on the step above.

   If you did not have any dependencies and haven't performed the step above
   (like in my case), just replace the content of your `.csproj` file with the
   following:

   ```xml
   <Project Sdk="Microsoft.NET.Sdk">
     <PropertyGroup>
       <TargetFramework>net472</TargetFramework>
     </PropertyGroup>
   </Project>
   ```

   There is also a third-party tool [CsprojToVs2017][sdk-tool] that can perform
   the conversion for you, but after using it you still might need to delete
   some reference by hand, for example those.

   ```xml
   <Reference Include="System.Data.DataSetExtensions" />
   <Reference Include="Microsoft.CSharp" />
   <Reference Include="System.Net.Http" />
   ```

## Porting class library

1. **Move from .NET Framework to .NET Standard or .NET Core**. After
   successfully converting my library to SDK-style format now I can retarget it.
   In my case I want my class library to target [.NET Standard][standard] instead of .NET
   Core. That way, it will be accessible from any .NET implementation if I
   decide to ship the game to other platforms (such as iOS, Android, or Web
   Assembly). To do so, in the project file I am replacing

    ```xml
   <TargetFramework>net472</TargetFramework>
   ```

   with

   ```xml
   <TargetFramework>netstandard2.0</TargetFramework>
   ```

    Build your application. Now you might get some errors if you were using APIs
    that are not included in .NET Standard. If you did not get any errors with
    your application, you can skip next two steps.

1. **Add Compatibility Pack if needed**. Some APIs that are not included in .NET
   Standard are available in [Compatibility Pack][compat-pack]. If you got
   errors on the previous step, you can check if [Compatibility
   Pack][compat-pack] can help.

   I got an error "The name 'Registry' does not exist in the current context" so
   I will add **Microsoft.Windows.Compatibility** NuGet package to my project.
   After installing the package errors disappear.

1. **Install API Analyzer**. [API Analyzer][api-analyzer], available as a NuGet
   package **Microsoft.DotNet.Analyzers.Compatibility**, will prompt you with
   warnings when you are using deprecated APIs or APIs that are not supported
   across all platforms (Windows, Linux, macOS). If you added Compatibility Pack
   I recommend to add API Analyzer as well to keep track of all non
   cross-platform APIs.

   At this point I am done with the class library migration to .NET Standard. If
   you have multiple projects referencing each other, migrate them "bottom-up"
   starting with the project that has no dependencies on other projects.

   In my example I also have a user interface project `MemoryGame.exe`, so now I
   will perform similar steps to migrate it to .NET Core.

## Porting UI

1. **Add .NET Core Windows Forms project**. Add a new .NET Core 3.0 WinForms
   project to the solution. At the moment we did the demo Visual Studio
   templates for desktop projects were under development, so I used the console.

   ```cli
   dotnet new winforms -o <path-to-your-solution>\MatchingGame.Core\
   ```

   After the new WinForms .NET Core project was created I added it to my
   solution.

   Once the WinForms templates for .NET Core are added to Visual Studio, you can
   create a new WinForms project by just right clicking on the solution ->
   **Add** -> **New Project...**.

1. **Link projects**. Delete all files from the new WinForms project (right now
   it contains  the generic Hello World code). Link all files from your existing
   .NET Framework WinForms project to the .NET Core 3.0 WinForms project by
   adding following to the `.csproj` file.

    ```xml
    <ItemGroup>
        <Compile Include="..\<Your .NET Framework Project Name>\**\*.cs" />
        <EmbeddedResource Include="..\<Your .NET Framework Project Name>\**\*.resx" />
    </ItemGroup>
    ```

1. **Align default namespace and assembly name**. Since you're linking to
   designer generated files (for example, `Resources.Designer.cs`) you generally
   want to make sure that the .NET Core version of your application uses the
   same namespace and the same assembly name. Copy the following settings from
   your .NET Framework project:

   ```xml
   <PropertyGroup>
       <RootNamespace><!-- (Your default namespace) --></RootNamespace>
       <AssemblyName><!-- (Your assembly name) --></AssemblyName>
   </PropertyGroup>
   ```

1. **Disable `AssemblyInfo` generation**. In the new style projects
   `AssemblyInfo` is generated automatically by default. At the same time the
   `AssemblyInfo` file from the old WinForms project will be copied to the new
   project too, because I linked all files `**\*.cs` in the previous step. That
   will result in duplication of `AssemblyInfo`. To avoid it in
   `MatchingGame.Core` project file I set `GenerateAssemblyInfo` to false.

   ```xml
   <GenerateAssemblyInfo>false</GenerateAssemblyInfo>
   ```

1. **Run new project**. Set your new .NET Core project as StartUp Project and
   run it. Make sure everything works.

1. **Copy or leave linked**. Now instead of linking the files, you can actually
   copy them from the old .NET Framework WinForms project to the new .NET Core
   3.0 WinForms project. After that you can get rid of the old project.

## Using WinForms Designer for .NET Core projects

As I mentioned above, WinForms Designer for .NET Core projects is not yet
available in Visual Studio. However there are ways to work around it.

1. You can keep your files linked (by just not performing the step above) and
   copy them when the Designer support is available. This way you can modify the
   files in your old .NET Framework WinForms project using WinForms Designer.
   And the changes will be automatically transferred to the new .NET Core
   WinForms project since they're linked.

1. You can have two project files in your WinForms project: the old `.csproj`
   file from the old .NET Framework WinForms project and the new `.csproj` file
   from the new .NET Core  WinForms project. You'll just have to unload and
   reload the project with corresponding project file depending on whether you
   want to use the WinForms Designer or not.

## See also
[WinForms repo][winforms]

[WinForms samples][winforms-samples]

[memory-game]: https://en.wikipedia.org/wiki/Concentration_(game)
[core-installation]: https://dotnet.microsoft.com/download
[api-port]: https://blogs.msdn.microsoft.com/dotnet/2018/08/08/are-your-windows-forms-and-wpf-applications-ready-for-net-core-3-0/
[pkg-config]: https://docs.microsoft.com/en-us/nuget/reference/migrate-packages-config-to-package-reference
[sdk-tool]:https://github.com/hvanbakel/CsprojToVs2017
[compat-pack]: https://docs.microsoft.com/dotnet/core/porting/windows-compat-pack
[wcf-supported]: https://github.com/dotnet/wcf/blob/master/release-notes/SupportedFeatures-v2.1.0.md
[api-analyzer]:https://blogs.msdn.microsoft.com/dotnet/2017/10/31/introducing-api-analyzer/
[compat-pack]:https://blogs.msdn.microsoft.com/dotnet/2017/11/16/announcing-the-windows-compatibility-pack-for-net-core/
[winforms]:https://github.com/dotnet/winforms
[winforms-samples]:https://github.com/dotnet/samples/tree/master/windowsforms
[nuget-org]: https://www.nuget.org/
[standard]: https://docs.microsoft.com/en-us/dotnet/standard/net-standard
