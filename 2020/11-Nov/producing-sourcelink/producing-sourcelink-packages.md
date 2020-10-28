---
post_title: 'Producing Packagess with Source Link'
username: cnov
featured_image: 
categories: .NET, .NET Core, Debugging
summary: 
---

In our last post, we showed you [how you can debug into the framework and dependencies](TODO ADD LINK) that was produced with Source Link. In this post, we'll show you how to add Source Link to your projects. This is beneficial both for public and internal projects.

## How Source Link Works

At its most basic, Source Link generates a JSON file that maps raw source code locations to the source files in the build. This is most commonly an HTTPS URL to a file on GitHub, GitLab, Azure Repos, or Bitbucket. The JSON file gets embedded in the debug symbols, which the debugger uses to download the file on-demand. For non-public repos, this does not automatically grant anyone access to the source; the debugger would need to authenticate so existing permissions are maintained.

## Using Source Link

Source Link is distributed as [a set of NuGet packages](https://www.nuget.org/packages?q=Microsoft.SourceLink), one per repository host. It is intended to be used as a private reference (it is used during the build, it does not require runtime consumers to have a dependency on it). 

You can enable Source Link experience in your own .NET project by setting a few properties and adding a PackageReference to a Source Link package:

```xml
<Project Sdk="Microsoft.NET.Sdk">
 <PropertyGroup>
    <TargetFramework>netcoreapp3.1</TargetFramework>
 
    <!-- Optional: Publish the repository URL in the built .nupkg (in the NuSpec <Repository> element) -->
    <PublishRepositoryUrl>true</PublishRepositoryUrl>
 
    <!-- Optional: Embed source files that are not tracked by the source control manager in the PDB -->
    <EmbedUntrackedSources>true</EmbedUntrackedSources>
  
    <!-- Optional: Embed symbols containing Source Link in the main file (exe/dll) -->
    <DebugType>embedded</DebugType>
  </PropertyGroup>
  <ItemGroup>
    <!-- Add PackageReference specific for your source control provider (see below) --> 
  </ItemGroup>
</Project>
```

### Deterministic Builds

Deterministic builds ensure that the same binary is produced regardless of the machine building it, including paths to sources stored in the symbols. While deterministic builds have been on by default since ____, there is an extra property, `ContinuousIntegrationBuild`, to set on the build server to normalize stored file paths. These should not be enabled during local dev or the debugger won't be able to find the local source files.

Therefore, you should use your CI system's variable to set them conditionally. For Azure Pipelines, it 
looks like this

```xml
<PropertyGroup Condition="'$(TF_BUILD)' == 'true'">
  <ContinuousIntegrationBuild>true</ContinuousIntegrationBuild>
</PropertyGroup>
```

For GitHub Actions, the variable is `GITHUB_ACTIONS`, so the result would be:
```xml
<PropertyGroup Condition="'$(GITHUB_ACTIONS)' == 'true'">
  <ContinuousIntegrationBuild>true</ContinuousIntegrationBuild>
</PropertyGroup>
```

### Don't Repeat Yourself

If you have a solution that has multiple projects in it, you can extract the common properties into a [Directory.Build.props](https://docs.microsoft.com/visualstudio/msbuild/customize-your-build#directorybuildprops-and-directorybuildtargets) file at the solution directory. That way all projects have them added automatically. 

If you distribute the library via a package published to [NuGet.org](https://nuget.org), you should use [embedded pdb's[TODO FIND SOURCE TO LINK] so the debug information is always available with your library. Alternatively, you can build a [symbol package](https://docs.microsoft.com/nuget/create-packages/symbol-packages-snupkg) and publish it to [NuGet.org](https://nuget.org) as well. This will make the symbols available on [NuGet.org symbol server](https://docs.microsoft.com/nuget/create-packages/symbol-packages-snupkg#nugetorg-symbol-server), where the debugger can download it from when needed. 

## Source Control Providers
Source Link packages are currently available for the following source control providers.

> Source Link package is a development dependency, which means it is only used during build. It is therefore recommended to set `PrivateAssets` to `all` on the package reference. This prevents consuming projects of your nuget package from attempting to install Source Link.

### github.com and GitHub Enterprise

For projects hosted by [GitHub](https://github.com) or [GitHub Enterprise](https://enterprise.github.com/home) reference 
[Microsoft.SourceLink.GitHub](https://www.nuget.org/packages/Microsoft.SourceLink.GitHub) like so:

```xml
<ItemGroup>
  <PackageReference Include="Microsoft.SourceLink.GitHub" Version="1.0.0" PrivateAssets="All"/>
</ItemGroup>
```

### Azure Repos (former Visual Studio Team Services)

For projects hosted by [Azure Repos](https://azure.microsoft.com/services/devops/repos) in git repositories reference [Microsoft.SourceLink.AzureRepos.Git](https://www.nuget.org/packages/Microsoft.SourceLink.AzureRepos.Git): 

```xml
<ItemGroup>
  <PackageReference Include="Microsoft.SourceLink.AzureRepos.Git" Version="1.0.0" PrivateAssets="All"/>
</ItemGroup>
```

### Azure DevOps Server (former Team Foundation Server)

For projects hosted by on-prem [Azure DevOps Server](https://azure.microsoft.com/services/devops/server/) in git repositories reference
[Microsoft.SourceLink.AzureDevOpsServer.Git](https://www.nuget.org/packages/Microsoft.SourceLink.AzureDevOpsServer.Git) and add host configuration like so:

```xml
<ItemGroup>
  <PackageReference Include="Microsoft.SourceLink.AzureDevOpsServer.Git" Version="1.0.0" PrivateAssets="All"/>
</ItemGroup>
```

If your server is configurated with non-empty IIS [Virtual Directory](docs/TfsVirtualDirectory/README.md), specify this directory in `SourceLinkAzureDevOpsServerGitHost` item like so:

```xml
<ItemGroup>
  <SourceLinkAzureDevOpsServerGitHost Include="server-name" VirtualDirectory="tfs"/>
</ItemGroup>
```

The `Include` attribute specifies the domain and optionally the port of the server (e.g. `server-name` or `server-name:8080`).

### GitLab

For projects hosted by [GitLab](https://gitlab.com) reference [Microsoft.SourceLink.GitLab](https://www.nuget.org/packages/Microsoft.SourceLink.GitLab) package: 

```xml
<ItemGroup>
  <PackageReference Include="Microsoft.SourceLink.GitLab" Version="1.0.0" PrivateAssets="All"/>
</ItemGroup>
```

### Bitbucket

For projects in git repositories hosted on [Bitbucket.org](https://bitbucket.org) or hosted on an on-prem Bitbucket server reference [Microsoft.SourceLink.Bitbucket.Git](https://www.nuget.org/packages/Microsoft.SourceLink.Bitbucket.Git) package:

```xml
<ItemGroup>
  <PackageReference Include="Microsoft.SourceLink.Bitbucket.Git" Version="1.0.0" PrivateAssets="All"/>
</ItemGroup>
```

If your project is hosted by Bitbucket Server or Bitbucket Data Center older than version 4.7 you must specify `SourceLinkBitbucketGitHost` item group in addition to the package reference:

```xml
<ItemGroup>
  <SourceLinkBitbucketGitHost Include="bitbucket.yourdomain.com" Version="4.5"/>
</ItemGroup>
```

The item group `SourceLinkBitbucketGitHost` specifies the domain of the Bitbucket host and the version of Bitbucket.
The version is important since URL format for accessing files changes with version 4.7. By default Source Link assumes new format (version 4.7+).

### gitweb (pre-release)

For projects hosted on-prem via [gitweb](https://git-scm.com/docs/gitweb) reference [Microsoft.SourceLink.GitWeb](https://www.nuget.org/packages/Microsoft.SourceLink.GitWeb) package: 

```xml
<ItemGroup>
  <PackageReference Include="Microsoft.SourceLink.GitWeb" Version="1.1.0-beta-20204-02" PrivateAssets="All"/>
</ItemGroup>
```

## Suummary

Source Link is easy to add to your projects and we highly recommend that all projects configure it by default. 
