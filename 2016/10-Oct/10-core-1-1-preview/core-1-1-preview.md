Announcing .NET Core 1.1 Preview 1
==================================

We're excited to announce the .NET Core 1.1 Preview 1 release today. It includes support for additional Linux distributions, has many updates and is the first *Current* release. I will describe all of these changes below. The release is a preview release and is intended as an early look at the release. It is not "Go Live" and is not yet recommended for production workloads.

You can download the release now:

- [Windows x64](https://go.microsoft.com/fwlink/?LinkID=831453)
- [Windows x86](https://go.microsoft.com/fwlink/?LinkID=831458)
- [macOS x64](https://go.microsoft.com/fwlink/?LinkID=831445)
- [Linux x64][preview-download]

You can see the full set of .NET Core 1.1 downloads on the [.NET Core Preview Download](https://github.com/dotnet/core/blob/master/release-notes/preview-download.md) page. 

.NET Core 1.1 Preview 1 Docker images are also available on Docker Hub in the [microsoft/dotnet](https://dockerhub.com/r/microsoft/dotnet) repo. You can see the try the new Docker image with the [dotnetapp-preview sample][dotnetapp-preview] in the [.NET Core Docker Samples repository](https://github.com/dotnet/dotnet-docker-samples/).

You can find the existing .NET Core 1.0 releases on the [dot.net/core](https://dot.net/core) page. .NET Core 1.1 will also be listed on that page once it is shipped as a stable release. 

Improvements
------------

The .NET Core 1.1 release is the first 1.x minor update. Its primary product theme is adding support for new operating system distributions.

### Operating System Distributions

Support for the following distributions was added:

- Fedora 24
- Linux Mint 18
- OpenSUSE 42.1
- macOS 10.12
- Windows Server 2016

You can see the full set of supported distributions in the [.NET Core 1.1 Preview 1 release notes](https://github.com/dotnet/core/blob/master/release-notes/1.1/1.1.0-preview1.md).

### APIs

1380 APIs were added in this release. You can see the complete set in the [API Difference .NET Core App 1.0 (ref) vs .NET Core App 1.1 (ref)](https://github.com/dotnet/core/blob/master/release-notes/1.1/1.0-1.1-api-diff/1.0-1.1-api-diff.md) document. 

APIs were added to enable specific scenarios. There was no specific theme to the API additions. 

No new .NET Standard version was created. [.NET Standard 2.0](https://blogs.msdn.microsoft.com/dotnet/2016/09/26/introducing-net-standard/) support is still coming.

### Fixes

Many specific product changes were made. You can look at the full set of [.NET Core 1.1 Preview 1 Commits](https://github.com/dotnet/core/blob/master/release-notes/1.1/1.1-preview1-commits.md) to learn more.

The previously announced MSBuild and CSProj changes are not part of this release, but are still coming.

Adopting .NET Core 1.1 Preview 1
--------------------------------

.NET Core 1.1 Preview 1 is a safe and easy install. It works the same way as .NET Core 1.0. There are a few things you will want to know about using it.

## Side-by-side Install with .NET Core 1.0

.NET Core 1.1 Preview 1 installs side-by-side with .NET Core 1.0. .NET Core 1.0 applications will continue to use the .NET Core 1.0 runtime. The .NET Core 1.0 environment is designed to be almost completely unaware that a later minor or major release is also installed. 

There is only one command -- `dotnet new` -- that will change as a result of installing .NET Core 1.1. `dotnet new` will create new projects that require .NET Core 1.1 Preview 1, as opposed to .NET Core 1.0. As a result, you may want to avoid installing it on a machine where you are doing .NET Core 1.0-based development with the command line tools. If you are on Windows and use Visual Studio for creating new projects, and not `dotnet new`, then installing .NET Core 1.1 is fine to do.

We would appreciate [feedback](https://github.com/dotnet/core/issues/305) on this design choice. The current design is that `dotnet new` will create new projects for the latest .NET Core version installed. If you don't think that's the right choice, tell us what you would like to see.

### Trying it out

You can start by installing [.NET Core 1.1 Preview](https://github.com/dotnet/core/blob/master/release-notes/1.1/1.1.0-preview1.md). After that, you can use the .NET Core tools just like you have with .NET Core 1.0. Try the following set of commands to create, build and run a .NET Core 1.1 Preview 1 application:

```console
dotnet new
dotnet restore
dotnet run
```

You can take a look at the [dotnetapp-preview][dotnetapp-preview] sample to try a .NET Core 1.1 Preview 1 application, with our without Docker.

### Upgrading Existing .NET Core 1.0 Projects

You can upgrade existing .NET Core projects from using .NET Core 1.0 to .NET Core 1.1 Preview 1. I will show you the new project.json file that the updated `dotnet new` now produces. It's the best way to see the new version values that you need to copy/paste into your existing project.json files. There are no automated tools to upgrade projects to later .NET Core versions.

The default .NET Core 1.1 Preview 1 project.json file follows:

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
          "version": "1.1.0-preview1-001100-00"
        }
      },
      "imports": "dnxcore50"
    }
  }
}
```

This project.json file is very similar to what your .NET Core 1.0 project.json looks like, with the exception of the `netcoreapp1.1` and `1.1.0-preview1-001100-00` target framework and meta-package version strings, respectively. 

You can use the following substitutions to help you update project.json files that you want to move temporarily or permanently to .NET Core 1.1.

- Update the `netcoreapp1.0` target framework to `netcoreapp1.1`.
- Update the `Microsoft.NETCore.App` package version from 1.0.x (for example, `1.0.0` or `1.0.1`) to `1.1.0-preview1-001100-00`. 

You can also just write `1.1.0-preview1-*` as a short-hand, skipping the build-specific information. It works and enables you to more easily move forward with .NET Core 1.1 nightly builds if you adopt those. You will want to change the metapackage version to `1.1.0` when .NET Core 1.1 ships as a stable release. The target framework version will not change. It is set for the lifetime of .NET Core 1.1.

### Upgrading Existing .NET Standard Projects

.NET Standard projects can also be updated, to use [.NET Standard 1.6 Preview 1](https://www.nuget.org/packages/NETStandard.Library/1.6.1-preview1-24530-04). Unlike, .NET Core, targeting a newer .NET Standard version isn't expected to give you a change in behavior, only new APIs.  

It's useful to try out a new .NET Standard version to try out new APIs, but there is no reason to move to a later .NET Standard version unless you need to use those new APIs. Moving to a later .NET Standard version results in dropping support for older operating system versions, so it is best to use the earliest .NET Standard version you can make due with, resulting in greater breadth for your library.

I will show you the new project.json file that the `dotnet new -t lib` now produces.
The default .NET Standard 1.1 Preview 1 project.json file follows:

```json
{
  "version": "1.0.0-*",
  "buildOptions": {
    "debugType": "portable"
  },
  "dependencies": {},
  "frameworks": {
    "netstandard1.6": {
      "dependencies": {
        "NETStandard.Library": "1.6.1-preview1-24530-04"
      }
    }
  }
}
```

<script src="https://gist.github.com/richlander/bbc5bff1782f7d5d9c7f848567637bd2.js"></script>

This project.json file is very similar to the project.json that the .NET Core 1.0 tools product, with the exception of the `netstandard1.6` and `1.6.1-preview1-24530-04` target framework and meta-package version strings, respectively. 

You can use the following substitutions to help you update project.json files that you want to move temporarily or permanently to .NET Core 1.1.

- Update the `netcoreapp1.x` (for example, `netstandard1.4`) target framework to `netstandard1.6`.
- Update the `NETStandard.Library` package version from 1.x (for example, 1.5) to `1.6.1-preview1-24530-04`. 

You can also just write `1.6.1-preview1-*` as a short-hand, skipping the build-specific information. It works and enables you to more easily move forward with .NET Core 1.1 nightly builds if you adopt those. You will want to change the metapackage version to `1.6.1` when .NET Standard 1.6.1 ships as a stable release. The target framework version will not change. It is set for the lifetime of .NET Core 1.1.

### Upgrading to .NET Core 1.1 Preview 1 Docker Images

.NET Core 1.1 Preview 1 images have been published to the [microsoft/dotnet](https://dockerhub.com/r/microsoft/dotnet) repo. There are two new tags for .NET Core 1.1, for the .NET Core 1.1 Preview 1 SDK and Runtime images, respectively: `1.0.0-preview2.1-sdk`, `1.1.0-core`.

The `latest` and other versionless tags have *not* been updated to point to .NET Core 1.1, but are still pointing to .NET Core 1.0. As a note, we are still deciding if the versionless tags should always point to LTS releases (see explanation below) or if it is OK for them to point to Current releases. Our thinking is that they should only ever point to LTS releases, leaving Current as opt-in. We'd appreciate your feedback on that.

You can try the new Docker images with the [dotnetapp-preview sample][dotnetapp-preview] in the [.NET Core Docker Samples repository](https://github.com/dotnet/dotnet-docker-samples/). The other samples can be easily modified to also exercise the .NET Core 1.1 Preview 1 images, following the project.json upgrade instructions I gave you above.

"Current" Release
-----------------

We announced in July that we would be adopting a [dual-train strategy for .NET Core releases](https://blogs.msdn.microsoft.com/dotnet/2016/07/26/net-support-and-versioning/). At the time, we called the two different product trains "LTS" and "FTS". Those release terms have since been renamed to "Long Term Support (LTS)" and "Current Release". This is similar to what other platforms do, like Red Hat Enterprise Linux, Ubuntu and Node.js. In fact, we adopted "Current" since it was already in use and already had the meaning that we wanted.

We call different releases "trains" since it is easy to apply the train (the vehicles on metal tracks) analogy to software releases. You can make references to "trains running on schedule" and there being an opportunity to "catch the next train". I'm sure you can come up with more of these in the comments. 

There is more to it, though. The LTS (slow) and Current (fast) trains define different releases cadences, different expectations on the kinds of changes that are acceptable in updates and different support timeframes. Based on our experience with the .NET Framework where we only ever had one train, we wanted to have more flexibility in releases and be able to better serve different customers with different expectations of us.   

We ship LTS releases after in-depth and lengthy testing, significant customer adoption (before being named LTS) and a high degree of stability. The goal is to update LTS releases as little as possible, only for security, significant reliability, performance issues and the rare important feature addition. They are supported for up to three years. Our more conservative customers tell us "I love this plan!". They would love zero changes if we could make that happen, although they realize that's not quite realistic.

Current releases are the ones we are actively working on *currently*. .NET Core 1.1 is such a release. We do our major feature work in these releases and also add support for new operating system distributions. These releases are stable but are also much faster moving, so require more testing when you adopt them. They are also only supported for three months after the next Current release ships. To stay on a supported version, you need to move to the next Current release before the three months passes.

Once we're happy with a series of Current releases and have had enough feedback, we label the next release as LTS and then repeat the whole process again. This could happen after few or many Current releases in a row. It depends a lot on the feedback we are hearing.

Support for some new operating system distributions will get added in LTS releases too, but that will be done on an exception basis. Windows Server 2016 and macOS Sierra are examples where that happened.

Please take a look at the [.NET Support and Versioning](https://blogs.msdn.microsoft.com/dotnet/2016/07/26/net-support-and-versioning/) blog post for more information.

Versioning, Filenames and Docker Tags
-------------------------------------

If you've worked on a significant project with lots of users and releases, you'll probably know that product naming and versioning is suprisingly hard. The .NET Core project doesn't escape this problem. In fact, it seems to embrace it, having chosen version strings that are not nearly as intuitive we could like. This section of the blog post hopefully provides you with a decoder ring on those versions, which we really should have shared earlier.

There are two distributions of .NET Core: a Runtime, and an SDK that includes the Runtime and some Tools. Easy so far. The primary issue is that the SDK distribution is the most popular distribution, but doesn't share the same versioning scheme as the Runtime. The challenge is that we primarily talk about the product in terms of Runtime versioning (including this blog post), while the SDK is versioned in terms of the Tools it carries. There are a variety of reasons why we chose to do that. That's the context.

.NET Core installers, Docker images and and project.json files carry version numbers that you need to use and reason about. It can be challenging selecting and/or writing the right thing because some of these strings look suprisingly similar, but mean different things.

Here are the key versions and what they mean, in prose English.

- `1.0.0-preview2-sdk` - Refers to the .NET Core 1.0 SDK, which includes a stable 1.0 Runtime and preview 1.0 Tools. This is the second preview release of the .NET Core Tools.
- `1.0.0-preview2.1-sdk` - Refers to the .NET Core 1.1 SDK, which includes a preview 1.1 Runtime and preview 1.0 Tools. It's called `preview2.1` because it's a dot release for the Tools relative to `preview2`, even though it comes with a new Runtime.
- `1.1.0-preview1` -- Refers to the first preview of the .NET Core 1.1 Runtime.

We intend to ship the final 1.0 version of the .NET Core Tools next year. This situation should get better. It will enable us to ship a `1.0.0-sdk` release, with no `preview`. The SDK and Runtime versions still won't match. We're discussing what to do about that. We'd like the Tools to be able to version faster than the Runtime, however, we may opt to get the version numbers to artificially be the same from time to time to make Runtimes and SDKs easier to match up, including the branding terms we use in blog posts.

Closing
-------

Please try out the .NET Core 1.1 Preview 1 release. We want to hear your [.NET Core 1.1 Preview 1 feedback](https://github.com/dotnet/core/issues/305) as we get ready to ship the final version of .NET Core 1.1. For those of you installing .NET Core on one of the newly supported operating system distributions, we want your feedback even more to help us scout out those releases more deeply.

Thanks to everyone for trying out and adopting .NET Core. We appreciate all of the feedback, product contributions and the general energy around the project. Thanks! 

[preview-download]: https://github.com/dotnet/core/blob/master/release-notes/preview-download.md
[relnotes]: https://github.com/dotnet/core/blob/master/release-notes/1.1/1.1.0-preview1.md 
[dotnetapp-preview]:https://github.com/dotnet/dotnet-docker-samples/tree/master/dotnetapp-preview
[feedback]: https://github.com/dotnet/core/issues/305
