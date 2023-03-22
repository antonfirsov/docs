---
post_title: Discover .NET 7 SDK Containers - Support for Authentication and Cross-architecture Builds
author1: chethusk
post_slug: updates-to-container-support-in-the-dotnet-sdk
username: chethusk
microsoft_alias: chethusk
featured_image: ./sdk-containers-7.0.200.png
categories: .NET, Cloud Native, Containers, Docker
tags: containers, .net 7, linux
summary: Learn about updates to the .NET SDK built-in containerization support in 7.0.200, including authentication to remote registries and support for multiple OS platforms.
desired_publication_date: 2023-03-23
post_date: 2023-03-23 09:05:00
---

Back in August we [announced](https://devblogs.microsoft.com/dotnet/announcing-builtin-container-support-for-the-dotnet-sdk/) support for building container images directly from the .NET CLI. Now, for the release of the .NET 7.0.200 SDK, we've expanded on that support to enable pushing to authenticated registries, targeting cross-architecture container images, and made it easier than ever to support rootless containers. We've also invested in making it easier to get started with the new container tooling. In this post we'll talk about all of these features, new to version 0.3.2 of the `Microsoft.Net.Build.Containers` package, as well as the roadmap for the future of .NET SDK container support.

If you're eager to get started, you can [skip ahead](#easier-getting-started-path).

## What's all this again?

Before we dig deep into the changes in this version, it may be helpful to review where we stand today. In .NET 7.0.100, the .NET SDK gained the ability to create container images via the `dotnet publish` command. The tools to do this do a bunch of inference based on the properties of your project and its outputs, and then create the same image that a simple Dockerfile would create. It can take a few as three commands to create a new application and publish it as an image:

```bash
dotnet new webapi
dotnet add package Microsoft.Net.Sdk.Containers
dotnet publish -c Release -r linux-x64 --no-self-contained -p PublishProfile=DefaultContainer
```

These three commands

* Create a new C# webapi project
* Add the container support package to the project
* Publish the project targeting the linux-x64 RuntimeIdentifier, using the mcr.microsoft.com/dotnet/aspnet:7.0 image as the base image, to a local Docker daemon

The intent of the tools are to reduce the concept count required to build containers and make it easier to get your code to the cloud.

If you'd like to learn more in general about containers and their benefits, there is a [Learn module](https://learn.microsoft.com/training/modules/intro-to-docker-containers/) that I can recommend that goes into detail.

## Registry Authentication

If you've worked with Docker before, you know that in order to use your images in production you typically have to _push_ them to a remote registry of some kind.  There are many different registries available, but they all share one thing in common - they require you to authenticate in order to push an image to them. In our initial release, we didn't support authenticating to remote registries, but in this release we've integrated support for the same Docker authentication protocol that all container tooling relies on - config.json and the ecosystem of Docker credential helpers.  Our tooling uses the superb [ValleySoft.DockerCredsProvider](https://github.com/mthalman/docker-creds-provider) library to seamlessly interact with your existing Docker credentials configuration, ensuring that pushing via the .NET SDK behaves exactly the same as via `docker push`. (As an aside, Matt has many helpful container-related libraries available on his [GitHub page](https://github.com/mthalman) - I blanket recommend them all for working with containers and registries!) Let's take a look at what it takes to push to GitHub Container Registry. For these next steps, I'm going to use my GitHub username (`baronfel`) and one of my demo apps (`sdk-container-demo`) as examples.

### Authenticate to the registry

To authenticate to GitHub Container Registry you'll need a GitHub Personal Access Token with the `read:packages` and `write:packages` permissions. First, login to GitHub Package Registry via the Docker CLI:

```bash
# Assume you have a GitHub Personal Access Token saved to the GITHUB_PACKAGE_PAT environment variable
echo $GITHUB_PACKAGE_PAT | docker login -n ghcr.io -u baronfel --password-stdin
```

### Publish the app as a container image

Then, publish your application as a container image to that registry using the following command. Note that the `ContainerImageName` property requires using the GitHub user name or organization name as a kind of "namespace" for the generated container image:

```bash
dotnet publish -c Release --os linux --arch x64 \
    -p PublishProfile=DefaultContainer \
    -p ContainerRegistry=ghcr.io \
    -p ContainerImageName=baronfel/sdk-container-demo
```

For me, this is a lot to type out, so I like to save the parameters to a 'Publish Profile'. You can read more about Publish Profiles [at the Learn docs](https://learn.microsoft.com/aspnet/core/host-and-deploy/visual-studio-publish-profiles?view=aspnetcore-7.0), but I think of them as a snippet of a project that I can reuse and refer to by name. To create a Publish Profile, I created a new file called `github.pubxml` in a `Properties/PublishProfiles` subdirectory of my project, and added the following contents:

```xml
<Project>
    <PropertyGroup>
        <ContainerRegistry>ghcr.io</ContainerRegistry>
        <ContainerImageName>baronfel/sdk-container-demo</ContainerImageName>
        <WebPublishMethod>Container</WebPublishMethod>
    </PropertyGroup>
</Project>
```

Now, I can publish with the following shorter command instead of the longer one from before:

```bash
dotnet publish -c Release --os linux --arch x64 -p PublishProfile=github
```

Much more readable! If you want to see a worked example, there is a repository that we've set up available at [baronfel/sdk-container-demo](https://github.com/baronfel/sdk-container-demo). This repository shows GitHub actions workflows pushing to many of the most popular container registries - if you see one that you use missing from the list, please send a PR!

Authentication in Docker is a bit detailed, so if you'd like to read more on it we've got detailed documentation about it at [Authenticating to container registries](https://github.com/dotnet/sdk-container-builds/blob/main/docs/RegistryAuthentication.md) in our repository.

As of this release we are not aware of any incompatibilities with the major container registries. I want to take the time to shout out [Norm Johanson](https://github.com/normj) from the AWS .NET SDK team, as well as his coworkers at the AWS Elastic Container Registry team, who took the time to contribute fixes for the way we handle the AWS ECR - without them it would have taken much longer! If you do encounter issues with registry communications please let us know by opening an issue at the GitHub repo!

## Cross-architecture containers

It's become very common for containers to be built for different platforms - while the default for Linux containers is x64, the rise of arm64 compute resources in cloud providers means that targeting that architecture is a requirement for container tooling. What this ends up looking like to the end user is a kind of container image called a [`manifest list`](https://docs.docker.com/registry/spec/manifest-v2-2/#manifest-list) - a container reference that brings together different platforms of the same  image under a single name and tag. Microsoft-authored container images like `mcr.microsoft.com/dotnet/runtime:7.0` are actually these manifest lists, built with support for multiple Linux and Windows architectures and versions. With this release of our container support package, we now use the `RuntimeIdentifier` passed to the publish command to help determine which specific platform should be used for your application when you use a manifest list as your base image.  In concrete terms, this means that you can target runtimes other than `linux-x64` in your publish command and the command will work, as long as your base image supports that runtime. For example, let's build a version of an application that targets arm64 Linux:

```bash
dotnet publish -c Release -r linux -a arm64 -p PublishProfile=DefaultContainer
```

That's all it takes to target your application for a different platform!  If you choose a runtime identifier that your base image doesn't support, we'll tell you the ones it _does_ support so you can update your commands.

And as always, all of this logic is completely customizable - if you specify a `ContainerBaseImage` that is very specific and points to only a single image - let's say `mcr.microsoft.com/dotnet/runtime:7.0-bullseye-slim-amd64` - then none of this code runs at all. You get exactly what you asked for.

## Improved support for rootless containers

It's more common than ever for containers to support running in a 'rootless' mode - one in which the container doesn't need additional permissions to execute. Typically this means that the container doesn't bind to ports below 1024, which require root access, and the application doesn't attempt to write to the container file system. You can read about rootless containers for .NET in more detail in the [Secure your .NET cloud apps with rootless Linux Containers](https://devblogs.microsoft.com/dotnet/securing-containers-with-rootless/) blog post.

In previous versions of the SDK Container tooling we had logic that would attempt to enforce binding to specific ports on your behalf. That logic interfered with support for rootless containers, so we've removed it in this version. Instead, if the base image you've chosen to use defines any necessary ports we support those. As always you can define your own ports via the `ContainerPort` MSBuild Item in your project file or publish profile.

With this change, container images produced via the .NET SDK are completely compliant with rootless execution.

## Easier 'getting started' path

For Web SDK projects, it's easier than ever to start building containers. Simply add the `<EnableSdkContainerSupport>true</EnableSdkContainerSupport>` property to your project file and the .NET SDK will automatically include the necessary package reference to the tooling. The tooling that ships with the 7.0.200 .NET SDK is version 0.3.2, but you can change this at any time by setting the `SdkContainerSupportPackageVersion` to another version.

If you are using Visual Studio, the publish dialogs will automatically detect and add this property for you if you choose to use the .NET SDK to publish your containers, so you just need to check in that change.

For other project types, you'll still need to add the `PackageReference` to `Microsoft.Net.Build.Containers` manually for now, but we are working on smoothing out the experience here in the future.

## Roadmap for the future

Our roadmap for the next release is centered around fit and finish items - primarily moving the container capability completely into the .NET SDK and focusing on localization and error messages (especially from the Visual Studio experience). These mark the end of what I consider the 'first phase' of the containerization effort.

Moving the containerization capability directly into the .NET SDK means two things to us:

* that it's available to all project types out of the box with no additional PackageReferences or properties required, and
* that updates to it ship on a consistent cadence, with the rest of the SDK

We hope to complete this work for the 7.0.300 release of the .NET SDK.

We also are investing in error messages for the various failure modes that can occur in the application. Currently, network errors with registries result in horrible stacktraces - we'd like to clean those up for presentation. We'd also like to do more precondition checking to fail fast with actionable errors, for instance in the case where you're pushing to a local Docker daemon but the daemon isn't yet started. We also want to take extra care to make sure that the Visual Studio experience receives the same fidelity of errors as the CLI experience. Because the Visual Studio implementation of the tooling is powered by a .NET 7 CLI application, it's easy for us to emit errors in a form that Visual Studio can't translate to the error pane. We'll fix those and ensure uniform error reporting between the CLI and Visual Studio so that all of our users get the same level of actionable error reporting.

During this time we're also investing in more integrations with partner teams, like the Docker tooling for Visual Studio and Visual Studio Code, the Oryx++ project, and the Azure Developer CLI. We'll make changes required for those tools to successfully integrate with us, and as a result you should start to see these experiences 'light up' automatically. Things like F5-debugging in Visual Studio/Visual Studio Code should just work. Running `azd up` should be seamless. Our aim is to make this tooling a natural fit for your existing workflows.  If you use other tools, please let us know so we can reach out to the authors for integrations!

After the 7.0.300 time frame, there are a number of areas we could explore. Image Indexes and authoring manifest lists in a single gesture might make it easier to autho the kinds of images that Microsoft ships for the runtime and ASP.NET Core each month. Improving our support for using local Dockerfiles as a base image would make it easier to commit to our tools without running into the limitations of a VM-less image creation model. Investing in better support for Windows Containers might make it easier for users investing in Windows to get their code onto cloud resources.  We need your help to prioritize the work! Please let us know what you'd like to see by interacting with the issues and discussions on our repo.

## Wrapping up

The 0.3.2 release of the .NET SDK Container tooling brings a host of new features like registry authentication, support for cross-architecture image builds, and support for rootless containers. The tooling is easier than ever to get started with, and is will continue to be invested into to improve the user experience.

I encourage you to take the tooling for a spin and create your own container images! Make sure to check out the docs to learn how to [Containerize an app with dotnet publish](https://learn.microsoft.com/dotnet/core/docker/publish-as-container). When you do, please reach out on the repo to let us know of any problems or features that you'd like to add.  Happy building!
