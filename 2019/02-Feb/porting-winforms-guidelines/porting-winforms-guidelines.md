# How to port WinForms applications to .NET Core 3.0

In this post, I will describe how to port a WinForms application from .NET
Framework to .NET Core. I will also show how you can keep using the WinForms
Designer in Visual Studio even though it is under development and is not yet
available for .NET Core projects.

## About the sample

For this post, I'll be using a [Memory-style][memory-game] board game
application. It contains a WinForms UI (`MemoryGame.exe`, targeting .NET
Framework 4.5) and a class library with the game logic
(`MemoryGame.Logic.dll`, also targeting .NET Framework 4.5). I'll be porting the
application project to .NET Core 3 and the class library to .NET Standard 2.0.
Using .NET Standard instead of .NET Core allows me to reuse the game logic to
provide the application for other platforms, such as iOS, Android or the web.

You can either watch Scott Hunter and me doing the conversion in this video, or
you can follow the step-by-step instructions below. Of course, you don't
get a penalty for doing both.

**ToDo: link to the video.**

## Step-by-step process

**Note**: The steps below are similar for porting both Windows Forms and WPF 
applications.

Before porting the application to .NET Core 3, I need to do some preparation
first.

## Preparing to port

1. **Install [.NET Core 3][core-installation]** and Update Visual Studio to 2019
   Preview version (Visual Studio 2017 will only support up to .NET Core 2.2).
   
1. **Start from a working solution**. Ensure the solution opens, builds, and
   runs without any issues.

1. **Run [.NET Portability Analyzer][api-port]** to determine if there are any
   APIs your application depends on that are missing from .NET Core. If there
   are, you have a few options:
    * Remove unsupported APIs or replace them with those that are included in
      .NET Core
    * Split your project into two projects, one that only contains usages of
      APIs that are available in .NET Core, the other contains the rest. This
      way, you get the best of both worlds without losing any features for your
      existing .NET Framework users; you'll only have to migrate the first
      project.

1. **Replace `packages.config` with `PackageReference`**. If your project uses
   NuGet packages, you will need to add the same NuGet packages to the new .NET
   Core project. .NET Core projects support only `PackageReference` for adding
   NuGet packages. To move your NuGet references from `packages.config` to your
   project file, right-click on `packages.config` -> **Migrate packages.config
   to PackageReference...**.

   You can learn more about this migration in our [docs][pkg-config].
   **TODO: Recommend updating NuGet packages early on to .NET Standard versions**
   **TODO: Add a note about creating a branch in source control (or create a new .csproj)**

1. **Migrate to the SDK-style .csproj file**. To move my application to .NET
   Core, I need to change my project file to SDK-style format because the old
   format does not support .NET Core. Besides, the SDK-style format is much
   leaner and easier to work with.

   You can either change SDK-style format by hand or using a third-party tool
   [CsprojToVs2017][sdk-tool]. After using the tool you still might need to
   delete some reference by hand, for example.

   ```xml
   <Reference Include="System.Data.DataSetExtensions" />
   <Reference Include="Microsoft.CSharp" />
   <Reference Include="System.Net.Http" />
   ```

   I will migrate by hand by just replacing all the content of `.csproj` file
   with the following lines.

   ```xml
   <Project Sdk="Microsoft.NET.Sdk">
     <PropertyGroup>
       <TargetFramework>net472</TargetFramework>
       <GenerateAssemblyInfo>false</GenerateAssemblyInfo>
     </PropertyGroup>
   </Project>
   ```

   If you are referencing NuGet packages or other projects in your old `.csproj` file, you'll need to add the references to the new `.csproj` file too, for example.

   NuGet package reference

   ```xml
   <PackageReference Include="Microsoft.Windows.Compatibility" Version="2.0.1" />
   ```

   Project reference

   ```xml
   <ProjectReference Include="..\MatchingGame.Core\MatchingGame.Core.csproj" />
    ```

   After you've migrated to the new SDK-style format, ensure your project builds and runs successfully.

## Porting the application

* **Retarget class library to .NET Standard** (optional).  To do so, just
   change the `TargetFramework` property to `netstandard2.0`:

   ```xml
   <Project Sdk="Microsoft.NET.Sdk">
     <PropertyGroup>
       <TargetFramework>nestandard2.0</TargetFramework>
     </PropertyGroup>
   </Project>
   ```


1. **Move from .NET Framework to .NET Standard or .NET Core**. After
   successfully converting my library to SDK-style format now I can retarget
   it. However, in my case I want my class library to target .NET Standard
   instead of .NET Core. That way, it will be accessible from any .NET
   implementation if I decide to ship the game to other platforms (such as iOS,
   Android, or Web Assembly). To do so, in the project file I am replacing

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

   I got an error "The name 'Registry' does not exist in the current context"
   so I will add **Microsoft.Windows.Compatibility** NuGet
   package to my project. After installing the package errors disappear.

1. **Install API Analyzer**. [API Analyzer][api-analyzer], available as a NuGet
   package **Microsoft.DotNet.Analyzers.Compatibility**, will prompt you with
   warnings when you are using deprecated APIs or APIs that are not supported
   across all platforms (Windows, Linux, macOS). If you added Compatibility Pack
   I recommend to add API Analyzer as well to keep track of all non
   cross-platform APIs.

   At this point I am done with the class library migration to .NET Standard.
   If you have multiple projects referencing each other, migrate them
   "bottom-up" starting with the project that has no dependencies on other
   projects.

1. **Add .NET Core Windows Forms project**. Add a new .NET Core 3.0 WinForms
   project to the solution. Visual Studio templates for desktop projects are
   under development, for now I will use the console.
   **TODO Project Templates in VS**
   ```cli
   dotnet new winforms -o <path-to-your-solution>\MatchingGame.Core\
   ```

   After the new WinForms .NET Core project is created, add it to your solution.

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
   WinForms project since they are linked.

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
[compat-pack]: https://docs.microsoft.com/en-us/dotnet/core/porting/windows-compat-pack
[wcf-supported]: https://github.com/dotnet/wcf/blob/master/release-notes/SupportedFeatures-v2.1.0.md
[api-analyzer]:https://blogs.msdn.microsoft.com/dotnet/2017/10/31/introducing-api-analyzer/
[compat-pack]:https://blogs.msdn.microsoft.com/dotnet/2017/11/16/announcing-the-windows-compatibility-pack-for-net-core/
[winforms]:https://github.com/dotnet/winforms
[winforms-samples]:https://github.com/dotnet/samples/tree/master/windowsforms
