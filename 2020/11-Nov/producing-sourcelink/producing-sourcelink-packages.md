---
post_title: 'Producing Packagess with Source Link'
username: cnov
featured_image: path/relative/to/your/post/image.png
categories: .NET, .NET Core, Debugging
summary: 
---

In our last post, we showed you how you can debug into the framework and dependencies that was produced with Source Link. In this post, we'll show you how to add Source Link to your projects. This is beneficial both for public and internal projects.

## How Source Link Works

At its most basic, Source Link generates a JSON file that maps raw source code locations to the source files in the build. This is most commonly an HTTPS URL to a file on GitHub, GitLab, Azure Repos, or Bitbucket. The JSON file gets embedded in the debug symbols, which the debugger uses to download the file on-demand. For non-public repos, this does not automatically grant anyone access to the source; the debugger would need to authenticate so existing permissions are maintained.

## Using Source Link

Source Link is distributed as [a set of NuGet packages](https://www.nuget.org/packages?q=Microsoft.SourceLink), one per repository host. It is intended to be used as a private reference (it is used during the build, it does not require runtime consumers to have a dependency on it). 