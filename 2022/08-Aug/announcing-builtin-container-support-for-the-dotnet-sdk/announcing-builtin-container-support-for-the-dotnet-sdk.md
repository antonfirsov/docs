---
post_title: Announcing built-in container support for the .NET SDK
author1: chethusk
post_slug: announcing-builtin-container-support-for-the-dotnet-sdk
username: chethusk
microsoft_alias: chethusk
featured_image: sdk-container-quickstart.png
categories: .NET, .NET Core, ASP.NET Core, Containers, Docker
summary: Describes the new container image building support in the .NET 7 SDK.
desired_publication_date: 2022-08-25
---

Containers have become one of the easiest ways to distribute and run a wide suite of applications and services in the cloud. The .NET Runtime was [hardened for containers](https://devblogs.microsoft.com/dotnet/announcing-net-core-3-0/#docker-and-cgroup-limits) years ago. You can now create containerized versions of your applications with just `dotnet publish`. Container images are now a supported output type of the .NET SDK.

## TL;DR

Before we go into the details of how this works, I want to show what a zero-to-running containerized ASP.NET Core application looks like:

> Note: you must have Docker installed and running for this sample to work
> Note: for this initial release, only Linux containers are supported.

```shell
# create a new project and move to its directory
dotnet new mvc -n my-awesome-container-app
cd my-awesome-container-app

# add a reference to a (temporary) package that creates the container
dotnet add package Microsoft.NET.Build.Containers

# publish your project for linux-x64
dotnet publish --os linux --arch x64 -p:PublishProfile=DefaultContainer

# run your app using the new container
docker run -it --rm -p 5010:80 my-awesome-container-app:1.0.0
```

Now you can go to `http://localhost:5010` and you should see the sample MVC application, rendered in all its glory.

## Motivation

Containers are an excellent way to bundle and ship applications. A popular way to build container images is through a Dockerfile - a special file that describes how to create and configure a container image. Let's look at one of our samples for an [ASP.NET Core application's Dockerfile](https://github.com/dotnet/dotnet-docker/blob/1ea6550437f5e6f5150792444bc26aa25e9773da/samples/aspnetapp/Dockerfile) here:

```dockerfile
# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY aspnetapp/*.csproj .
RUN dotnet restore --use-current-runtime  

# copy everything else and build app
COPY aspnetapp/. .
RUN dotnet publish -c Release -o /app --use-current-runtime --self-contained false --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "aspnetapp.dll"]
```

This project uses a [multi-stage build](https://docs.docker.com/develop/develop-images/multistage-build/) to both build and run the app. It uses the 7.0 SDK image to restore the application's dependencies, publish the application to a folder, and finally create the final runtime image from that directory.

This Dockerfile works very well, but there are a few caveats to it that aren't immediately apparent, which arise from the concept of a Docker [build context](https://docs.docker.com/engine/reference/commandline/build/#description). The build context is a the set of files that are accessible inside of a Dockerfile, and is often (though not always) the same directory as the Dockerfile. If you have a Dockerfile located beside your project file, but your project file is underneath a solution root, it's very easy for your Docker build context to not include configuration files like Directory.Packages.props or NuGet.config that would be included in a regular `dotnet build`. You would have this same situation with any hierarchical configuration model, like EditorConfig or repository-local git configurations.

This mismatch between the explicitly-defined Docker build context and the .NET build process was one of the driving motivators for this feature. All of the information required to build an image is present in a standard `dotnet build`, we just needed to figure out the right way to represent that data in a way that container runtimes like Docker could use.

### How does it work

A container image is made of two primary parts: some JSON configuration containing metadata about how the image can be run, and a list of tarball archives that represent the file system.  In .NET 7, we added several APIs to the .NET Runtime for [handling TAR files and streams](https://devblogs.microsoft.com/dotnet/announcing-dotnet-7-preview-4/#added-new-tar-apis), and that opened the door to manipulating container images programmatically.

This approach has been demonstrated very successfully in projects like [Jib](https://github.com/GoogleContainerTools/jib) in the Java ecosystem, [Ko](https://github.com/google/ko) for Go, and even in .NET with projects like [konet](https://github.com/lippertmarkus/konet). It's obvious that a simple tool-driven approach to generate container images is becoming more popular.

We built the .NET SDK solution with the following goals:

* Provide seamless integration with existing build logic, to prevent the kinds of context gaps mentioned earlier
* Implement in C#, to take advantage of our own tool and benefit from .NET runtime performance improvements
* Integrated into the .NET SDK, so that it is straightforward for the .NET team to improve and service in the existing .NET SDK processes

## Customization

Those of you with experience writing Dockerfiles are probably wondering where all of the complexity of the Dockerfile has gone. Where is the `FROM` base image declared? What tags are used? Many aspects of the generated image are customizable by you through the use of MSBuild properties and items (you can read about that in detail [at the documentation](https://aka.ms/dotnet/containers/customization)), but we've shipped a few defaults to make getting started easier.

### Base Image

The base image defines which functionality you will have available and which OS and version you will be using. The following base images are automatically chosen:

* for ASP.NET Core apps, `mcr.microsoft.com/dotnet/aspnet`
* for self-contained apps, `mcr.microsoft.com/dotnet/runtime-deps`
* for all other apps, `mcr.microsoft.com/dotnet/runtime`

In all cases, the tag used for the base image is the version portion of the TargetFramework that has been chosen for the application - for example, a TargetFramework of `net7.0` would result in a tag of `7.0` being used. These simple-version tags are based on Debian distributions of linux - if you want to use a different supported distro like Alpine or Ubuntu then you would need to manually specify that tag (see the `ContainerBaseImage` example below).

We think that the choice of defaulting to the Debian-based versions of the runtime images is the best choice for broad compatibility with most applications' needs. Of course, you are free to choose any base image for your containers. Simply set the `ContainerBaseImage` property to any image name and we'll do the rest. For example, maybe you want to run your web application on Alpine Linux. That would look like this:

```xml
<ContainerBaseImage>mcr.microsoft.com/dotnet/aspnet:7.0-alpine</ContainerBaseImage>
```

### Image name and version

The name of your image will be based on the `AssemblyName` of your project by default. If that's not a good fit for you, the name of the image can be explicitly set by using the `ContainerImageName` property.

An image also needs a tag, so we default to the value of the `Version` property. This defaults to `1.0.0`, but you are free to customize this in any way and we'll make use of it. This fits especially well with automated versioning schemes like [GitVersioning](https://github.com/dotnet/Nerdbank.GitVersioning), where the version is derived from some configuration data in the repository.  This helps ensure that you always have a unique version of your application to run. Unique version tags are especially important for images, because pushing an image with the same tag to a registry will overwrite the image that was previously stored as that tag.

### Everything else

Many other properties can be set on an image, like custom Entrypoints, Environment Variables, and even arbitrary Labels (which are often used for tracking metadata like SDK version, or who built the image). In addition, images are often pushed to destination registries by specifying a different base registry. In subsequent versions of the .NET SDK we plan to add support for these capabilities in a similar manner as described above - all controlled through MSBuild project properties.

## When should you use this

### Local development

If you need a container for local development, now you can have one with a single command. `dotnet publish --os linux --arch x64 -p:PublishProfile=DefaultContainer` will make a Debug-configuration container image named after your project. Some users have made this even simpler by putting these properties into a Directory.Build.props and simply running `dotnet publish`:

```xml
<Project>
    <PropertyGroup>
        <RuntimeIdentifier>linux-x64</RuntimeIdentifier>
        <PublishProfile>DefaultContainer<PublishProfile>
    </PropertyGroup>
</Project>
```

You could also use other SDK and MSBuild features like response files or PublishProfiles to create groups of these properties for easier use.

### CI Pipelines

Overall, building container images with the SDK should integrate seamlessly into your existing build processes. A sample of a minimal GitHub Actions workflow to containerize an app is just about 30 lines of configuration:

```yml
name: Containerize ASP.NET Core application

on: [push]

jobs:
  build:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      - name: Setup .NET SDK
        uses: actions/setup-dotnet@v2
      # login to our registry so we can push the image
      - name: Docker Login
        uses: actions-hub/docker/login@master
        env:
          DOCKER_REGISTRY_URL: sdkcontainerdemo.azurecr.io
          DOCKER_USERNAME: ${{ secrets.ACR_USERNAME }}
          DOCKER_PASSWORD: ${{ secrets.ACR_PAT }}
      # build the app for the linux-x64 runtime identifier
      - name: Build
        run: dotnet build --os linux --arch x64 --configuration Release
        env:
          GITHUB_USERNAME: baronfel
          GITHUB_TOKEN: ${{ secrets.CONTAINER_PUSH_PAT }}
      # Package the app into a linux-x64 container based on the dotnet/aspnet image
      - name: Publish
        run: dotnet publish --os linux --arch x64 --configuration Release -p:PublishProfile=DefaultContainer
      # Because we don't yet support pushing to authenticated registries, we have to use docker to
      # tag and push the image. In the future neither of these steps will be required!
      - name: Tag built container with Azure Container Registry url
        uses: actions-hub/docker/cli@master
        with:
          args: tag sdk-container-demo:1.0.0 sdkcontainerdemo.azurecr.io/baronfel/sdk-container-demo:latest
      - name: Push built container to Azure Container Registry
        uses: actions-hub/docker/cli@master
        with:
          args: push sdkcontainerdemo.azurecr.io/baronfel/sdk-container-demo:latest
```

This is part of an example repo called [baronfel/sdk-container-demo](https://github.com/baronfel/sdk-container-demo) made to demonstrate this end-to-end process.

There is one major scenario that we won't be able to handle at all with SDK-built containers, though.

### RUN commands

There's no way of performing RUN commands with the .NET SDK. These commands are often used to install some OS packages or create a new OS user, or any number of arbitrary things. If you would like to keep using the SDK container building feature, you can instead create a custom base image with these changes and then using this base image as the `ContainerBaseImage` as described above.

As an example, let's say I'm working on a library that needs the libxml2 library installed on the host in order to do some custom XML processing. I could write a Dockerfile like this to capture this native dependency:

```Dockerfile
FROM mcr.microsoft.com/dotnet/runtime:7.0

RUN apt-get update && \
        apt-get install -y libxml2 && \
        rm -rf /var/lib/apt/lists/*
```

Now, I can build, tag, and push this image to my registry and use it as a base for my application:

```shell
docker build -f Dockerfile . -t registry.contoso.com/my-base-image:1.0.0
docker push registry.contoso.com/my-base-image:1.0.0
```

In the project file of my application I'd set the `ContainerBaseImage` property to this new image:

```xml
<Project>
  <PropertyGroup>
    ...
    <ContainerBaseImage>registry.contoso.com/my-base-image:1.0.0</ContainerBaseImage>
    ...
  </PropertyGroup>
</Project>
```

## Temporary gaps

This is the initial preview of SDK-built container images, so there are some features that we haven't yet been able to deliver.

### Windows images and non-x64 architectures

We have focused on the Linux-x64 image deployment scenario for this initial release. Windows images and other architectures are key scenarios we plan to support for the full release, so watch out for new developments there.

### Pushing to remote registries

We have not yet implemented support for authentication, which is critical for many users. This is one of our highest priority items. In the interim, we suggest pushing to your local Docker daemon, then using `docker tag` and `docker push` to push the generated images to your destination. The sample GitHub Action above shows how this can be done.

### Some image customization

Several image metadata customizations have not been implemented yet. This includes custom Entrypoints, environment variables, and custom user/group information. A full list can be seen on [dotnet/sdk-container-builds](https://aka.ms/dotnet/containers/customization#unsupported-properties).

## Next Steps

Over the release candidate phases of the .NET 7 release we will be adding new image metadata, support for pushing images to remote registries, and support for Windows images. You can track that progress at the [milestone](https://github.com/dotnet/sdk-container-builds/issues?q=is%3Aopen+is%3Aissue+milestone%3A7.0.100-rc1) we've created for this.

We also plan on integrating this work directly into the SDK throughout the release. When that happens, we'll be releasing a 'final' version of the package on NuGet that will warn you of the change, and ask you to remove the Package entirely from your project.

If you are interested in cutting-edge builds of the tool, we also have a [GitHub package feed](https://github.com/dotnet/sdk-container-builds/packages/1588543) where you can opt into trying the latest work.

If you encounter issues using the SDK container image builder, [let us know](https://github.com/dotnet/sdk-container-builds/issues/new) and we'll do our best to help you out.

## Closing thoughts

We would love for those of you building Linux containers to try building them with the .NET SDK. I've personally had a blast trying them out locally - I had a good time going to some of my demo repositories and containerizing them with a single command, and I hope you all feel the same!

Happy image building!
