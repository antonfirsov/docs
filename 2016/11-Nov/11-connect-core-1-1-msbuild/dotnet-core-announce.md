Announcing .NET Core 1.1
========================

We are excited to announce the release of .NET Core 1.1 RTM. You can start creating .NET Core 1.1 apps, today, in Visual Studio 2015, Visual Studio 2017 RC, Visual Studio Code and Visual Studio for the Mac.

We used the 1.1 release to achieve the following improvements:

- .NET Core: Add distros and improve performance.
- ASP.NET Core: Improve Kestrel, Azure support and productivity.
- EF Core: Azure and SQL 2016 support.

News flash: We were recently informed by the fine folks at [TechEmpower](https://aka.ms/techempower)that ASP.NET Core 1.1 with Kestrel was ranked as the fastest mainstream fullstack web framework in the TechEmpower plaintext benchmark. That's a great result, and the result of significant engineering effort.

You can see all the .NET Core changes in detail in the [.NET Core 1.1 release notes](https://github.com/dotnet/core/releases/tag/v1.1.0). It's a small delta on the [.NET Core 1.1 Preview 1](https://blogs.msdn.microsoft.com/dotnet/2016/10/25/announcing-net-core-1-1-preview-1/) release that we shipped 3 weeks ago.

### Distributions

Support for the following distributions was added:

- Linux Mint 18
- OpenSUSE 42.1
- macOS 10.12 (also added to .NET Core 1.0)
- Windows Server 2016 (also added to .NET Core 1.0)

You can see the full list of supported distributions in the [.NET Core 1.1 release notes](https://github.com/dotnet/core/tree/master/release-notes/1.1). 

### Documentation

[.NET Core documentation](https://docs.microsoft.com/dotnet) has been updated for the release and will continue to be updated. We are also in the process of making visual and content updates to the .NET Core docs to make the docs easier and more compelling to use.

The [ASP.NET Core](https://docs.microsoft.com/aspnet) and [Entity Framework](https://docs.microsoft.com/ef), [C#](https://docs.microsoft.com/dotnet/articles/csharp) and [VB](https://docs.microsoft.com/dotnet/articles/visual-basic) docs were moved to [docs.microsoft.com](https://docs.microsoft.com/) as part of this release. [F# documentation](https://docs.microsoft.com/dotnet/articles/fsharp/) was added a few months ago. 

Documentation is docs.microsoft.com](https://docs.microsoft.com/) open source. You can help us make it better by filing issues and making contributions on GitHub. Best places to start are [dotnet/docs](https://github.com/dotnet/docs/) and [aspnet/docs](https://github.com/aspnet/docs/).

### Performance

ASP.NET Core 1.1 with Kestrel was ranked as the fastest mainstream fullstack web framework in the [TechEmpower plaintext benchmark](https://aka.ms/techempower).

We adopted a performance optimization for the CoreCLR runtime called [Profile-Guided Optimization (PGO)](https://msdn.microsoft.com/library/e7k32f4k.aspx), for the .NET Core 1.1 Windows version. We've use this technique for the .NET Framework for many years, but had not yet used it for .NET Core. This improvement was not included in the earlier .NET Core 1.1 Preview 1 release.

PGO optimizes C++ compiler-generated binaries with information it records from apps it observes in our lab. We call this process "training". It's about as exciting as 6AM runs in the dark during the Winter. PGO records info about which codepaths are used in a binary and in what order. For this release, we used a simple "Hello World" app for training. 

We saw a 15% improvement with the ASP.NET [MusicStore](https://github.com/aspnet/MusicStore) app running with a PGO-optized CoreCLR in our lab and believe that these improvements will be representative to other Web applications. We hope to see greater improvements in the future as we increase the pool of apps we train with.

For Linux and macOS, we compile CoreCLR with Clang/LLVM. We intend to use the [Clang version of PGO](http://clang.llvm.org/docs/UsersManual.html#profile-guided-optimization) in the next release. Very preliminary scouting of Clang PGO suggests that we will see similar benefits.

### APIs

There are [1380 new APIs](https://github.com/dotnet/core/blob/master/release-notes/1.1/1.0-1.1-api-diff/1.0-1.1-api-diff.md) in .NET Core 1.1. Many of the new APIs were added to support the product itself, include reading [portable PDBs](https://github.com/dotnet/core/blob/master/release-notes/1.1/1.0-1.1-api-diff/1.0-1.1-api-diff_System.Reflection.PortableExecutable.md). .NET Core 1.1 supports [.NET Standard 1.6](https://www.nuget.org/packages/NETStandard.Library/1.6.0). 

[.NET Standard 2.0](https://blogs.msdn.microsoft.com/dotnet/2016/09/26/introducing-net-standard/) support is coming in an upcoming release (in 2017). It is not part of .NET Core 1.1.

Using .NET Core 1.1
-------------------

You can start by installing .NET Core 1.1. You can either install it globally using the [.NET Core 1.1 installer](https://dot.net/core) or package manager for your operating system or try it an isolated (and easily removable) environment by downloading [.NET Core as a zip](foo).

### Safe side-by-side install

You can safely globally install .NET Core 1.1 on a machine that already has .NET Core 1.0. 

The `dotnet new` command creates new templates that reference the latest runtime on the machine. This may not be desired. If not, you can hand-edit the versions in the resulting project.json to earlier version numbers. Based on feedback, we will be changing this behavior in the new version of the tools, at the same time we release the final version of Visual Studio 2017. If you do not use `dotnet new` to create new projects, but rely on Visual Studio, then you are not affected.

### Try it out

You can try .NET Core out with the command line tools, using these commands in your command prompt or terminal.

```console
dotnet new
dotnet restore
dotnet run
```

You can also try out .NET Core 1.1 with a [dotnet-bot sample](https://github.com/dotnet/dotnet-docker-samples/tree/master/dotnetapp-current) we created for using .NET Core with Docker (although you don't have to use Docker).

### Upgrading Existing .NET Core 1.0 Projects

You can upgrade existing .NET Core 1.0 projects to .NET Core 1.1. I will show you the new project.json file that the updated `dotnet new` now produces. It's the best way to see the new version values that you need to copy/paste into your existing project.json files. There are no automated tools to upgrade projects to later .NET Core versions.

The default .NET Core 1.1 project.json file follows:

```json
{
  "version": "1.0.0-*",
  "buildOptions": {
    "debugType": "portable",
    "emitEntryPoint": true
  },
  "dependencies": {},
  "frameworks": {
    "netcoreapp1.1": {
      "dependencies": {
        "Microsoft.NETCore.App": {
          "type": "platform",
          "version": "1.1.0"
        }
      },
      "imports": "dnxcore50"
    }
  }
}
```

<script src="https://gist.github.com/richlander/4cfdefbad68db0b7b9bd3d166bd7c875.js"></script>

This project.json file is very similar to what your .NET Core 1.0 project.json looks like, with the exception of the `netcoreapp1.1` and `1.1.0` target framework and meta-package version strings, respectively. 

You can use the following substitutions to help you update project.json files that you want to move temporarily or permanently to .NET Core 1.1.

- Update the `netcoreapp1.0` target framework to `netcoreapp1.1`.
- Update the `Microsoft.NETCore.App` package version from 1.0.x (for example, `1.0.0` or `1.0.1`) to `1.1.0`. 

### Upgrading .NET Standard Library Projects

There is no need to update .NET Standard Library projects.

We did publish a [NETStandard.Library 1.6.1](https://www.nuget.org/packages/NETStandard.Library/1.6.1-preview1-24530-04) meta package, however, there is no benefit in referencing it for producing libraries. The updated package has been provided as a dependency for the updated [Microsoft.NETCore.App 1.1](https://www.nuget.org/packages/Microsoft.NETCore.App/1.1.0-preview1-001100-00) metapackage. 

### Using .NET Core 1.1 Docker Images

You can use .NET Core 1.1 with Docker. You can find updated images at [microsoft/dotnet](https://hub.docker.com/r/microsoft/dotnet).

The `latest` tag has been updated to point to the .NET Core 1.1 SDK. This is a departure from our earlier plan, as discussed in the 1.1 Preview 1 post. We looked at other platforms that have Current and LTS and saw that `latest` does indeed point to the latest version. Makes sense. 

There are two new Runtime tags for .NET Core 1.1: 

- Linux: `1.1.0-runtime`
- Windows: `1.1.0-runtime-nanoserver` 

There are two new SDK tags for .NET Core 1.1: 

- Preview 2-based SDK, using project.json: `1.1.0-sdk-projectjson`
- Preview 3-based SDK, using CSProj: `1.1.0-sdk-msbuild`, .

You can try .NET Core 1.1 with the [dotnetapp-current sample][dotnetapp-current] in the [.NET Core Docker Samples repository](https://github.com/dotnet/dotnet-docker-samples/). The other samples can be easily modified to also depend the .NET Core 1.1 images, by updating both the project.json and Dockerfile files with the appropriate version strings (all of which are provided above).

Current Release
---------------

In the earlier .NET Core 1.1 blog post, I described that we have adopted the industry practice of differentiated releases, which we've called ["Long-term Support (LTS)"](https://en.wikipedia.org/wiki/Long-term_support) and "Current". .NET Core 1.1 is a Current release and also the first one. Once a given Current release has shipped, we expect very few updates, hopefully only security updates. 

We recommend that most developers adopt LTS releases. It's also the default experience we'll include in Visual Studio. We do hope that some of you adopt Current releases to give us feedback, as well. It's hard to quantify it, but we thinking an 80/20 split between LTS and Current across the entire .NET Core developer base would be about right.

Closing
-------

Please try the new .NET Core release and give us feedback. There are a lot of key improvements across .NET Core, ASP.NET Core and EF Core that will make your apps better and faster. It's the first Current release, providing you with feature faster, provided you are happy with updating .NET Core more quickly than the LTS multi-year supported releases.

To recap, the biggest changes are:

- Performance improvements, enough to make a very positive first entry on the [TechEmpower benchmarks](https://aka.ms/techempower).
- Addition of four OS distros.
- 10s of new features and 100s of bug fixes.
- Updated documentation.

Thanks to everyone who adopted .NET Core 1.0 and .NET Core 1.1 Preview 1 that gave us feedback. We appreciate all of the contribution and engagement! Please tell us what you think of the latest release.

You can start creating .NET Core 1.1 apps, today, in Visual Studio 2015, Visual Studio 2017 RC, Visual Studio Code and Visual Studio for the Mac.

[core]: https://dot.net/core
[VSRC]: https://visualstudio.com
