# CLI Local Tools and Tool Updates

.NET Core 3.0 introduces an additional way to install  .NET Core tools: local tools.

The `dotnet tool install` command now supports three different ways to install .NET Core tools. Local tools use a manifest which will often be in the root directory of a repository. This means the manifest is cloned along with the other repository files. When the user that cloned the directory runs `dotnet tool restore`, the correct versions of all the tools in the manifest will be placed on the machine. Local tools can also be used to specify per location versions of tools. 

.NET Core tool can be managed in three ways:

* Global tools (added in .NET Core 2.1):
  * Installed using `dotnet tool install --global <packageId>` which places an executable in a directory that is on the path by the user.
  * Are executables available from any directory for that user.
  * Have only one version installed at a time.
  * Intended for general purpose tools.
* Tool-path tools (added in .NET Core 2.1):
  * Installed using `dotnet tool install --tool-path <path> --global <packageId>` which places an executable in the specified directory.
  * Are executables accessed via normal file access such as an absolute or relative path to the executable.
  * Can have different versions installed in different directories.
  * Intended primarily for CI builds prior to availability of local tools.
* Local tools (added in .NET Core 3.0):
  * Defined in a manifest which is intended to be checked into source control.
  * All tools in the manifest are made available (restored) with a single command.
  * Are stored locally in a cache.
  * Are available only from that location and subdirectories.
  * Are dlls accessed via the `dotnet tool run` command.
  * Have a specific version defined in the manifest.
  * Intended for per repository versioning of tools and streamlining first use of a repository after cloning.

The tools created for these approaches are identical. The difference is how they are installed and used.

Three older approaches remain supported for backward compatibility: `DotNetCliToolReference` and using the `dotnet` driver to run commands. Also, MSBuild tasks and targets are an additional extensibility point used to customize the build process, but aren't covered here.

## Using local tools

Local tool installation is only available in .NET Core 3.0 and above.

Local tools are .NET Core shared framework dlls that are restored from a manifest. The intent is to make it easy for repository maintainers or organizations to provide the correct tool versions for their contributors or team members, and to have these versions not conflict between projects.

When a programmer clones a repository, they will get any tool manifest file it contains along with the other files. Running the following command ensures that the correct version of all of the tools defined in the manifest are restored into a local cache:

```bash
> dotnet tool restore
```

Manifests installed in different directories on the user's machines (such as in different local repositories) can specify different versions of the tools. Use of the local cache means that only one copy is placed on the machine for each specific version of the tool.

Local tools are accessed using the command `dotnet tool run <ToolName>` driver. The driver finds the manifest, determines the version needed, finds it in the cache, and runs it. For example, the `dotnetsay` tool prints any arguments and is accessed with:

```bash
> dotnet tool run dotnetsay Hello
Hello
```

We are looking for feedback on how well this calling approach works. You can provide feedback in the [CLI GitHub repository](https://github.com/dotnet/cli/issues).

The local tools that will be restored are contained in a manifest.

## Using global tools

Global Tools are .NET Core shared framework executables installed into a directory that is placed into your path during the installation of the .NET Core SDK. You install global tools using the commands:

```bash
> dotnet tool install dotnetsay
```

Since they are in a single location on your path, you can run the tool by just typing the name of the executable.

```bash
> dotnetsay Hello
Hello
```

You can learn more in the [documentation for .NET Core Global Tools](https://docs.microsoft.com/en-us/dotnet/core/tools/global-tools).

## Using tool-path option

Tool-path tools are .NET Core shared framework executable installed in the directory you specify. They are primarily useful for scripting and were important in supplying a specific version of a tool, such as for a build server, prior to local tools. Some teams may continue to prefer their specificity for scripting:

```bash
> dotnet tool install dotnetsay --tool-path ../tools
```

You access them like any other executable not on your path, such as by specifying them with a partial path:

```bash
> ../tools/dotnetsay Hello
```

Since tool-path tools are a variation of global tools, you can learn more in the [documentation for .NET Core Global Tools](https://docs.microsoft.com/en-us/dotnet/core/tools/global-tools).

## Choosing which approach is right for you

Global, local and tool-path tools differ in how they are installed. The same tool can be installed as a global tool, a local tool or a tool-path tool. 

|Style|Tool on path|Specific version|Availability|Calling mechanism|
|-|-|-|-|-|
|local|No|Yes|Directory location|Via dotnet: `dotnet tool run dotnetsay Hello`|
|global|Yes|No|User-wide|Direct: `dotnetsay Hello`
|tool-path|No|Yes|By specifying path|With path: Direct: `../tools/dotnetsay Hello`

Use a global tool when:

* You are installing a tool for your use.
* You're happy to have it on your path.
* You always want the latest version or a specific version.
* You want updating the tool to affect all usage.
* You want the simplest calling mechanism.

Use a local tool when:

* You want to share a set of tools for a repo or team and want easy, consistent tool installation
* You want to specify tools with their version in source control.
* You don't mind a few extra characters when calling the tool.
* You want a tool, or version of a tool, available only for specific projects.

Use a tool-path tool when:

* You are scripting and want to restrict the impact on the machine to the working directory.
* You are using tools in a build that do not need to be available to normal users of the repository.

We anticipate local tools replacing much of the usage of tool-path tools that occurred in the .NET Core 2.1 time frame. We think this will occur because the same tools are generally needed for human and scripted usage, and because we think version isolation will be sufficient for many build scenarios (the cache can be shared).

## Managing the local tool manifest

If you maintain a repository or manage a team and want a straightforward way to allow the programmers you work with to have the right tools, consider using local tools.

Local tools rely on a manifest.

1. Create a manifest file by navigating to the directory location below which you want tools available (usually your repository root) and calling:

```bash
dotnet new tool-manifest
```

2. Add the tools you would like to the manifest using:

```bash
dotnet tool install <package_id> [--version <version>]
```

3. Ensure the manifest is included in your source control repository.
1. You can update the manifest to specify other newer versions of tools by either using `dotnet tool update <package_id>` or manually editing the manifest file.
1. You can uninstall tools from the the manifest by manually editing the manifest file.

### Structure of the tool manifest file

The tool manifest is a human readable JSON file with this structure:

```json
{
    "isRoot": true,
    "tools": {
        "<NuGet_PackageId>": {
            "version": "Version",
            "commands": [
                "<ToolCommandName>"
            ]
        }
    }
}
```

You may want to edit the tool manifest file to check or change the version of a tool.

In some cases, you may want to combine multiple tool manifest files up the directory hierarchy. This may be useful in organizations that have a proscribed directory structure. To do this change "isRoot" to false. If multiple entries are found for a tool, the one closest to the point of invocation (the leaf-most) will be used.

Many packages can be included within `tools`.

When you add tools to the manifest with `dotnet tool install`, the version and tool command name are entered for you. If you edit the manifest file you must include these.

### Location of the tool manifest file

The tool manifest file is located in the `.config` folder. The `dotnet tool restore` and `dotnet tool run` commands look for a manifest file in a `.config` directory at each level of the file hierarchy. If the file is in the directory.

## Dotnet Framework Executables

Local tools, global tools and tool-path tools are all framework dependent executables, which means they are executables that depend on a version of the .NET Core runtime installed on your machine and rolls forward on normal .NET Core Runtime roll forward rules. Specifically, applications targeting .NET Core Runtime 2.1 rolls forward to 2.2, but applications targeting 2.1 and 2.2 do not roll forward to 3.0.

This currently creates a challenge for all tool authors. You can multi-target or include a runtimeconfig.json file that allows roll forward. You can find out more in the [documentation for roll forward](https://docs.microsoft.com/en-us/dotnet/core/whats-new/dotnet-core-2-1#roll-forward).

## Comments on naming

A good name for your tool will help people find and use it. A few guidelines:

* The tool command name is the name used to run the installed tool and does not need to match the package Id.
* The package Id is used to install, update and uninstall the tool.
* Neither package Ids or tool command names need to be in the format `dotnet-`.
* When a tool prefixed by `dotnet-` is installed as a global tool (or a tool-path tool placed into the user's path), it can be run using `dotnet <tool>`. These tools run the risk of colliding with future verbs in the CLI. If a verb conflicts, the verb will have precedence and the tool will only be accessible with its full tool command name (such as `dotnet-myToolName`).
* If the user attempts to install a tool with a tool command name already installed, installation will fail. The user will have to pick one of the tools.
* Establishing a unique name for your tool is often an excellent strategy.
* The NuGet namespace is finite. Consider prefacing your package Id with a branded prefix.
* We currently support only one command per tool package. 

## Check it out

I hope you'll explore .NET Core tools and the different ways to install and manage them and look forward to any feedback you have!