# How to port desktop applications to .NET Core 3.0

In this post, I will describe how to port a desktop application from .NET
Framework to .NET Core. I picked a WinForms application as an example. Steps for
WPF application are similar and I'll describe what needs to be done different
for WPF as we go. I will also show how you can keep using the WinForms designer
in Visual Studio even though it is under development and is not yet available
for .NET Core projects.

## About the sample

For this post, I'll be using a [Memory-style][memory-game] board game
application. It contains a WinForms UI (`MemoryGame.exe`) and a class library
with the game logic (`MemoryGame.Logic.dll`), both targeting .NET Framework 4.5. You can download the sample [here][sample]. ToDo; add link.
I'll be porting the application project to .NET Core 3.0 and the class library
to .NET Standard 2.0. Using .NET Standard instead of .NET Core allows me to
reuse the game logic to provide the application for other platforms, such as
iOS, Android or web.

You can either watch Scott Hunter and me doing the conversion in this video, or
you can follow the step-by-step instructions below. Of course, I won't be holding it against
you, if you were to do both.

**ToDo: link to the video.**

## Step-by-step process

>I suggest doing the migration in a separate branch or, if you're not using
>version control, ~~question your life choices~~ creating a copy of your project
>so you have a clean state to go back to if necessary.

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
   the version and find the latest one that doesn't break your code.

1. **Run [.NET Portability Analyzer][api-port]** to determine if there are any
   APIs your application depends on that are missing from .NET Core. In case
   there are, you need to refactor your code to avoid dependencies on not
   supported in .NET Core APIs. Sometimes it's possible to find an alternative
   API that provides the needed functionality.

1. **Replace `packages.config` with `PackageReference`**. If your project uses
   NuGet packages, you will need to add the same NuGet packages to the new .NET
   Core project. .NET Core projects support only `PackageReference` for adding
   NuGet packages. To move your NuGet references from `packages.config` to your
   project file, right-click on `packages.config` -> **Migrate packages.config
   to PackageReference...**.

   You can learn more about this migration in our [docs][pkg-config].

## Porting main project

**Create new project**
    
1. Create new application of the same type (Console, WinForms, WPF, Class
   Library) as the application you are willing to port, targeting .NET Core 3.
   At the moment we did the demo Visual Studio templates for desktop projects
   were under development, so I used the console.

    ```cli
    dotnet new winforms -o <path-to-your-solution>\MatchingGame.Core\
    ```

1. In the project file copy all external references from the old project, for
   example:

    ```xml
    <PackageReference Include="Newtonsoft.Json" Version="9.0.1" />
    ```

1. Build. At this point if the packages you're referencing support only .NET
   Framework, you will get a [NuGet warning][nuget-warning]. If you have not
   upgraded to the latest versions of NuGet packages on the step 3, try to find
   if the latest version supporting .NET Core (.NET Standard) is available and
   upgrade. If there are no newer version, .NET Framework packages can still be
   used but you might get run-time errors if those packages have dependencies on
   APIs not supported in .NET Core. We recommend to let the author of the NuGet
   package know that you'd be interested in seeing the package being updated to
   .NET Standard. You can do it via `Contact` form on the [NuGet
   gallery][nuget-org].

### Fast way (replace existing project file)

**First, let's try the fast way to port.** Make sure you have a copy of your
current `.csproj` file, you might need to use it in future. Replace your current
`.csproj` file with the `.csproj` file from the project you created on the step
above and add in the top `<PropertyGroup>`:

```xml
<GenerateAssemblyInfo>false</GenerateAssemblyInfo>
```

Build your app. If you got no errors - congrats, you've successfully migrated
your project to .NET Core 3. For porting a dependent (UI) project see **Porting
UI** section, for using the Designer, check out **Using WinForms Designer for
.NET Core projects** section.

### Slow way (guided porting)

If you got errors (like I would with my app), it means there are more
adjustments you need to make. Instead of the fast way described here, bellow
I'll do one change at a time and give possible fixes for each issue. Steps
bellow would also help to better understand the process of the migration so if
the fast way worked for you but you're curious to learn all "whys", keep on
reading.

1. **Migrate to the SDK-style .csproj file**. To move my application to .NET
   Core, first I need to change my project file to SDK-style format because the
   old format does not support .NET Core. Besides, the SDK-style format is much
   leaner and easier to work with.

   Make sure you have a copy of your current `.csproj` file. Replace the content of your `.csproj` file with the following.

   For WinForms application:

   ```xml
   <Project Sdk="Microsoft.NET.Sdk">
     <PropertyGroup>
       <OutputType>WinExe</OutputType>
       <TargetFramework>net472</TargetFramework>
       <UseWPF>true</UseWPF>
       <GenerateAssemblyInfo>false</GenerateAssemblyInfo>
     </PropertyGroup>
   </Project>
   ```

   For WPF application:

   ```xml
   <Project Sdk="Microsoft.NET.Sdk">
     <PropertyGroup>
       <OutputType>WinExe</OutputType>
       <TargetFramework>net472</TargetFramework>
       <UseWindowsForms>true</UseWindowsForms>
       <GenerateAssemblyInfo>false</GenerateAssemblyInfo>
     </PropertyGroup>
   </Project>
   ```

   > Note that I set `<GenerateAssemblyInfo>` to `false`. In the new-style
   > projects `AssemblyInfo.cs` is generated automatically by default. So if you
   > already have `AssemblyInfo.cs` file in your project (spoiler alert: you
   > do), you need to disable auto-generation or remove the file.

   Now copy & paste all references from the old version of `.csproj` file into
   the new one. For example:

       *NuGet package reference*

       ```xml
       <PackageReference Include="Microsoft.Windows.Compatibility" Version="2.0.1" />
       ```

       *Project reference*

       ```xml
       <ProjectReference Include="..\MatchingGame.Core\MatchingGame.Core.csproj" />

   The project should build successfully since it is just a new way of writing the same thing. If you got any errors, double check your steps.

   There is also a third-party tool [CsprojToVs2017][sdk-tool] that can perform
   the conversion for you, but after using it you still might need to delete
   some reference by hand, such as:

   ```xml
   <Reference Include="System.Data.DataSetExtensions" />
   <Reference Include="Microsoft.CSharp" />
   <Reference Include="System.Net.Http" />
   ```

1. **Move from .NET Framework to .NET Standard or .NET Core**. After
   successfully converting my library to SDK-style format I'm able to retarget
   it. In my case I want my class library to target [.NET Standard][standard]
   instead of .NET Core. That way, it will be accessible from any .NET
   implementation if I decide to ship the game to other platforms (such as iOS,
   Android, or Web Assembly). To do so, replace this

    ```xml
   <TargetFramework>net472</TargetFramework>
   ```

   with

   ```xml
   <TargetFramework>netstandard2.0</TargetFramework>
   ```

    Build your application. You might get some errors if you are using APIs that
    are not included in .NET Standard. If you did not get any errors with your
    application, you can skip the next two steps.

1. **Add Compatibility Pack if needed**. Some APIs that are not included in .NET
   Standard are available in [Compatibility Pack][compat-pack]. If you got
   errors on the previous step, you can check if the [Compatibility
   Pack][compat-pack] can help.

   I got an error "The name 'Registry' does not exist in the current context" so
   I added the **Microsoft.Windows.Compatibility** NuGet package to my project.
   After installation, the error disappeared.

1. **Install API Analyzer**. [API Analyzer][api-analyzer], available as the
   NuGet package **Microsoft.DotNet.Analyzers.Compatibility**, will prompt you
   with warnings when you are using deprecated APIs or APIs that are not
   supported across all platforms (Windows, Linux, macOS). If you added the
   Compatibility Pack, I recommend adding the API Analyzer to keep track of all
   of API usages that won't work across all platforms.

   At this point, I am done with the class library migration to .NET Standard.
   If you have multiple projects referencing each other, migrate them
   "bottom-up" starting with the project that has no dependencies on other
   projects.

   In my example I also have a WinForms project `MemoryGame.exe`, so now I
   will perform similar steps to migrate that to .NET Core.

## Porting UI

1. **Add .NET Core UI project**. Add a new .NET Core 3.0 UI project to the
   solution. At this moment, the Visual Studio templates for desktop projects
   are under development, so I just used the `dotnet` CLI.

       ```cli
       dotnet new winforms -o <path-to-your-solution>\MatchingGame.Core\
       ```

    For WPF projects you'd use this:

       ```cli
       dotnet new wpf -o <path-to-your-solution>\MatchingGame.Core\
       ```

   After my new WinForms .NET Core project was created, I added it to my
   solution.

1. **Link projects**. First, delete all files from the new project (right now it
   contains the generic Hello World code). Then, link all files from your
   existing .NET Framework UI project to the .NET Core 3.0 UI project by adding
   following to the `.csproj` file.

    ```xml
    <ItemGroup>
        <Compile Include="..\<Your .NET Framework Project Name>\**\*.cs" />
        <EmbeddedResource Include="..\<Your .NET Framework Project Name>\**\*.resx" />
    </ItemGroup>
    ```

   If you have a WPF application you also need to include `.xaml` files:

   ```xml
   <ItemGroup>
     <ApplicationDefinition Include="..\WpfApp1\App.xaml" Link="App.xaml">
       <Generator>MSBuild:Compile</Generator>
     </ApplicationDefinition>
     <Compile Include="..\WpfApp1\App.xaml.cs" Link="App.xaml.cs" />
   </ItemGroup>

   <ItemGroup>
     <Page Include="..\WpfApp1\MainWindow.xaml" Link="MainWindow.xaml">
       <Generator>MSBuild:Compile</Generator>
     </Page>
     <Compile Include="..\WpfApp1\MainWindow.xaml.cs" Link="MainWindow.xaml.cs" />
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

1. **Disable `AssemblyInfo.cs` generation**. As I mentioned earlier, in the
   new-style projects, `AssemblyInfo.cs` is generated automatically by default.
   At the same time the `AssemblyInfo.cs` file from the old WinForms project
   will be copied to the new project too, because I linked all files `**\*.cs`
   in the previous step. That will result in duplication of `AssemblyInfo.cs`.
   To avoid it in `MatchingGame.Core` project file I set `GenerateAssemblyInfo`
   to `false`.

   ```xml
   <GenerateAssemblyInfo>false</GenerateAssemblyInfo>
   ```

1. **Run new project**. Set your new .NET Core project as the StartUp project
   and run it. Make sure everything works.

1. **Copy or leave linked**. Now instead of linking the files, you can actually
   copy them from the old .NET Framework UI project to the new .NET Core
   3.0 UI project. After that, you can get rid of the old project.

## Using the WinForms designer for .NET Core projects

As I mentioned above, the WinForms designer for .NET Core projects is not yet
available in Visual Studio. However there are two ways to work around it:

1. You can keep your files linked (by just not performing the step above) and
   copy them when the designer support is available. This way, you can modify
   the files in your old .NET Framework WinForms project using the designer. And
   the changes will be automatically reflected in the new .NET Core WinForms
   project -- since they're linked.

1. You can have two project files in the same directory as your WinForms
   project: the old `.csproj` file from the existing .NET Framework project and
   the new SDK-style `.csproj` file of the new .NET Core WinForms project.
   You'll just have to unload and reload the project with corresponding project
   file depending on whether you want to use the designer or not.

## Summary

In this blog post, I showed you how to port a desktop application containing multiple projects from .NET Framework to .NET Core. In typical cases, just retargeting your projects to .NET Core isn't enough. I described potential issues you might encounter and ways of addressing them. Also, I demonstrated how you can still use the WinForms designer for your ported apps while it's not yet available for .NET Core projects.

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
[nuget-warning]: https://docs.microsoft.com/en-us/nuget/reference/errors-and-warnings/nu1701
[sample]:https://github.com/
