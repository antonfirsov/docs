---
post_title: Build client web assets with your Razor Class Library
username: daroth@microsoft.com
microsoft_alias: daroth
featured_image: ./dotnet-compatible-with-other-tools.png
categories: ASP.NET, ASP.NET Core
summary: Learn how to integrate a client build process using tools like npm and webpack into the build of your Razor Class Library.
desired_publication_date: 2021-11-01
---

Razor Class Libraries are .NET class libraries setup with the Razor SDK to enable building Razor files (.razor, cshtml) and to include client web assets (.js, .css, images, etc.). Client web assets often need to be built or processed using tools like npm and webpack. But integrating a client web asset build pipeline into the build process of a Razor Class Library can be challenging. It's error prone, and sometimes results in fragile solutions, like relying on the ordering of tasks in the build process that can change between releases.

In this post we'll show you how to integrate npm and webpack into the build process for your Razor Class Library. For folks who want a solution that just works, we'll look at the new experimental [Microsoft.AspNetCore.ClientAssets](https://www.nuget.org/packages/Microsoft.AspNetCore.ClientAssets/) package that handles this integration for you automatically provided you have the right tools installed. We'll also walk through all the details of how to set this up using MSBuild for folks that want to be able to control and customize the process. While the focus of this post is npm and webpack, the solution that we provide here can easily be adapted to other tools like yarn, rollup, etc.

## Adding a client web assets build process

There are two things that we want to happen as part of the build process to integrate handling of client web assets:

1. Restore client web assets managed by other package managers (npm, yarn).
1. Build/generate client web assets and include the results as part of the build output as static web assets.

In our examples, we'll put all the client web asset sources into an *assets* folder within the root of the project.

For the output of the build process we have a couple of options:

* Put the outputs in the *dist* subfolder. This is common in many tools and makes it easy to glance at the output. However, if you're using Git (or some other version control system), you may be forced to add an entry into the *.gitignore* file to avoid accidentally adding the outputs to source control, and it makes cleaning the workspace harder.
* Put the outputs in the *obj* folder. This is more inline with how builds work in .NET and doesn't require any additional setup. We'll use this approach.

Ideally, we want these steps to be incremental, meaning that they only run when any of the inputs have changed and don't run when everything is up to date. For example, we only want to run `npm install` when the packages are not available locally or when the *package.json* file has been updated.

## Using the MicrosoftAspNetCore.ClientAssets package

All of this build logic can be included in a NuGet package and reused across multiple projects. In fact, that's what we've done in the experimental [Microsoft.AspNetCore.ClientAssets](https://nuget.org/packages/Microsoft.AspNetCore.ClientAssets) package, which will automatically add support for integrating npm, webpack, or other tools to your build pipeline.

To use the MicrosoftAspNetCore.ClientAssets package:

- Add a package reference to Microsoft.AspNetCore.ClientAssets from your project.
- Add an *assets* folder with a *package.json* file.
- In *assets/package.json* specify your dependencies and defines the `build:Debug` and `build:Release` scripts you want to run for Debug and Release builds.

That's it! You're read to restore and build your client web assets. You can find a sample that shows using this package to build TypeScript files in a Razor Class Library here: https://github.com/aspnet/AspLabs/tree/main/src/ClientAssets/samples.

We think this package will fit the needs of most users, but give it a try and let us know what you think by submitting any issues you find on [GitHub](https://github.com/dotnet/aspnetcore/issues/new). If for some reason this doesn't work for a tool you are using, you can add the MSBuild logic directly to your project and update and adapt it to fit your needs. We'll cover that next.

## Add a client web assets build process using MSBuild

The MicrosoftAspNetCore.ClientAssets package provides custom MSBuild targets to add a client web assets build process to your project. In this section, we'll look at how these targets work, so that you can customize them.

### Integrating `npm install` with Razor Class Libraries

To restore client web assets from npm, we'll create a task to invoke `npm install`. MSBuild tasks allow you to specify `Inputs` and `Outputs`, which are used to determine if the target needs to run by examining whether an input is newer than one of the outputs. Since we're putting all our client sources in the *assets* folder, we can use the *assets\package.json* file as an input to determine if we need to run the `npm install` command or not. When `npm install` is run, a *node_modules\.package-lock.json* file is created, which we can use for the output. If *package.json* is changed, it will be newer than *node_modules\.package-lock.json*, which will trigger a new install the next time the build is triggered. 

The complete target is displayed below:

```xml
<Target Name="NpmInstall" Inputs="assets\package.json" Outputs="assets\node_modules\.package-lock.json">
  <Message Importance="high" Text="Running npm install..." />
  <Exec Command="npm install" WorkingDirectory="assets" />
</Target>
```

### Running an npm command and collecting the outputs as static web assets

Next we'll create a target that runs an npm command and collects the outputs as static web assets. In our example, we'll create an `NpmRunBuild` target that runs the `build:$(Configuration)` script in *package.json* with every build (where `$(Configuration)` is Release or Debug).

The `NpmRunBuild` target will do the following:

* Run the given npm command.
* Collect the outputs from the command.
* Add the outputs as `Content` items, and link them to where they would be in the *wwwroot* folder.
* Write a file to disk with the list of generated files to use as an output for incrementalism.

We'll start by defining the `NpmRunBuild` target so that it runs after `NpmInstall` but before `AssignTargetPaths`. The target executes the specified npm command, and the command output is directed to *$(IntermediateOutputPath)\npm*, which normally points to *obj\Debug|Release\net6.0*.

```xml
<Target Name="NpmRunBuild" DependsOnTargets="NpmInstall" BeforeTargets="AssignTargetPaths">
  <Exec Command="npm run build:$(Configuration) -- -o $(IntermediateOutputPath)\npm" WorkingDirectory="assets" />
</Target>
```

Next we'll add the build outputs as `Content` items and link them to the *wwwroot* folder. We can do so by declaring three item groups after invoking the `Exec` task as follows:

```xml
<ItemGroup>
  <_NpmOutput Include="$(IntermediateOutputPath)\npm\**" />
  <Content Include="@(_NpmOutput)" Link="wwwroot\%(_NpmOutput.RecursiveDir)\%(_NpmOutput.FileName)%(_NpmOutput.Extension)" />
  <FileWrites Include="@(_NpmOutput)" />
</ItemGroup>
```

The `_NpmOutput` item group captures all the files in the output content. We then include these items as `Content` and link them to the *wwwroot* folder. Finally, we add these items to the list of `FileWrites` so they can be cleaned up property.

The last step is to make this target run only when the sources are changed. To do so, we need to declare `Inputs` and `Outputs` on the target in the same way we did for the `NpmInstall` target. MSBuild will check the outputs, and execute the target in two situations:

* The outputs don't exist.
* Any outputs are older than any of the inputs.

Given that we don't know what the outputs of running the npm command are going to be, we'll generate a file with the list of output files. The output file listing will always have a well-known file name (*npmbuild.complete.txt*), so we'll be able to use it as the output for our target. We can produce the output file listing like this:

```xml
<WriteLinesToFile File="$(IntermediateOutputPath)npmbuild.complete.txt" Lines="@(_NpmOutput)" />
```

Since we're generating a file, we also need to add it to the list of `FileWrites` as we did with the other outputs:

```xml
<FileWrites Include="$(IntermediateOutputPath)npmbuild.complete.txt" />
```

For the inputs, we can define an item group to capture them as follows:

```xml
<ItemGroup>
  <NpmBuildInputs Include="assets\**" Exclude="assets\node_modules\**" />
</ItemGroup>
```

With all that in place, we can define the inputs and outputs on our `NpmRunBuild` target as follows:

```xml
<ItemGroup>
  <NpmBuildInputs Include="assets\**" Exclude="assets\node_modules\**" />
</ItemGroup>

<Target Name="NpmRunBuild" DependsOnTargets="NpmInstall" BeforeTargets="AssignTargetPaths" Inputs="@(NpmBuildInputs)" Outputs="$(IntermediateOutputPath)npmbuild.complete.txt">
  <Exec Command="$(NpmCommand) -- -o $(IntermediateOutputPath)\npm" WorkingDirectory="assets" />

  <ItemGroup>
    <_NpmOutput Include="$(IntermediateOutputPath)\npm\**"></_NpmOutput>
    <Content Include="@(_NpmOutput)" Link="wwwroot\%(_NpmOutput.RecursiveDir)\%(_NpmOutput.FileName)%(_NpmOutput.Extension)" />
    <FileWrites Include="@(_NpmOutput)" />
    <FileWrites Include="$(IntermediateOutputPath)npmbuild.complete.txt" />
  </ItemGroup>

  <WriteLinesToFile File="$(IntermediateOutputPath)npmbuild.complete.txt" Lines="@(_NpmOutput)" />
</Target>
```

This target will only run when you change something in your sources, like your TypeScript files or *package.json*.

### Adding scripts in *package.json*

With these targets the build pipeline will invoke `npm install` and `npm run build:Debug` or `npm run build:Release` when necessary, passing the output folder as the `-o` parameter. We need to define these scripts in our *package.json* to invoke the appropriate tool. To use webpack, the scripts look like this:

```json
"scripts": {
  "build:Debug": "webpack --mode development",
  "build:Release": "webpack --mode production"
}
```

The parameters passed from the `Exec` task to `npm run` will transparently flow into the invoked command and override the default output path.

### Reusing the MSBuild code

We've seen in detail how to put together all the pieces. However, we've been very strict in what command to run, where the files should be located, and so on. Next we'll generalize the solution by using MSBuild to parameterize the process. Here's how:

```xml
<PropertyGroup>
  <ClientAssetsDirectory Condition="'$(ClientAssetsDirectory)' == ''">assets\</ClientAssetsDirectory>
  <ClientAssetsRestoreInputs Condition="'$(ClientAssetsRestoreInputs)' == ''">$(ClientAssetsDirectory)package-lock.json;$(ClientAssetsDirectory)package.json<ClientAssetsRestoreInputs>
  <ClientAssetsRestoreOutputs Condition="'$(ClientAssetsRestoreOutputs)' == ''">$(ClientAssetsDirectory)node_modules\.package-lock.json<ClientAssetsRestoreOutputs>
  <ClientAssetsRestoreCommand Condition="'$(ClientAssetsRestoreCommand)' == ''">npm install<ClientAssetsRestoreCommand>
  <ClientAssetsBuildCommand Condition="'$(ClientAssetsBuildCommand)' == ''">npm run build:$(Configuration)<ClientAssetsBuildCommand>
  <ClientAssetsBuildOutputParameter Condition="'$(ClientAssetsBuildOutputParameter)' == ''">-o<ClientAssetsBuildOutputParameter>
  <ClientAssetsRestoreInputs>$(MSBuildProjectFile);$(ClientAssetsRestoreInputs)</ClientAssetsRestoreInputs>
</PropertyGroup>

<ItemGroup>
  <ClientAssetsInputs Include="$(ClientAssetsDirectory)**" Exclude="$(DefaultItemExcludes)" />
</ItemGroup>

<Target Name="NpmInstall" Inputs="$(ClientAssetsRestoreInputs)" Outputs="$(ClientAssetsRestoreOutputs)">
  <Message Importance="high" Text="Running $(ClientAssetsRestoreCommand)..." />
  <Exec Command="$(ClientAssetsRestoreCommand)" WorkingDirectory="$(ClientAssetsDirectory)" />
</Target>

<Target Name="NpmRunBuild" DependsOnTargets="NpmInstall" BeforeTargets="AssignTargetPaths" Inputs="@(ClientAssetsInputs)" Outputs="$(IntermediateOutputPath)clientassetsbuild.complete.txt">
  <Exec Command="$(ClientAssetsBuildCommand) -- $(ClientAssetsBuildOutputParameter) $(IntermediateOutputPath)\clientassets" WorkingDirectory="$(ClientAssetsDirectory)" />

  <ItemGroup>
    <_ClientAssetsBuildOutput Include="$(IntermediateOutputPath)\clientassets\**"></_ClientAssetsBuildOutput>
    <Content Include="@(_ClientAssetsBuildOutput)" Link="wwwroot\%(_ClientAssetsBuildOutput.RecursiveDir)\%(_ClientAssetsBuildOutput.FileName)%(_ClientAssetsBuildOutput.Extension)" />
    <FileWrites Include="@(_ClientAssetsBuildOutput)" />
    <FileWrites Include="$(IntermediateOutputPath)clientassetsbuild.complete.txt" />
  </ItemGroup>

  <WriteLinesToFile File="$(IntermediateOutputPath)clientassetsbuild.complete.txt" Lines="@(_ClientAssetsBuildOutput)" />
</Target>
```

With the changes we made above, we can adapt the build process by changing the values for the different properties in our project file. For example, we can specify that our client web assets are in the *js* folder instead of the *assets*, or change the list of default exclusions:

```xml
<PropertyGroup>
  <ClientAssetsDirectory>js\</ClientAssetsDirectory>
  <DefaultItemExcludes>$(ClientAssetsDirectory)\dist\**</DefaultItemExcludes>
</PropertyGroup>
```

## Summary

Adding a client web assets build process to your .NET projects is now easy using the new experimental [Microsoft.AspNetCore.ClientAssets](https://www.nuget.org/packages/Microsoft.AspNetCore.ClientAssets/) package. Or you can create a custom build process using similar MSBuild logic to fit your needs. We hope you'll give this functionality a try and let us know what you think on [GitHub](https://github.com/dotnet/aspnetcore/issues/new).

Happy coding!
