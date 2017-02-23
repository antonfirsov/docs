.NET and Docker
===============

Many developers I talk to are either using Docker actively or planning to adopt containers in their environment. Containers are an important trend in our industry and .NET is now part of that. [Microsoft and Docker](https://weblogs.asp.net/scottgu/docker-and-microsoft-integrating-docker-with-windows-server-and-microsoft-azure) have been working together so that you'll have a great experience using Docker with .NET apps.

The Docker ecosystem started as a Linux technology. Now, you can use .NET Core with Debian Linux images, with a similar workflow and set of supported scenarios as other development platforms.

The Windows team recently released Windows Server 2016 and updates to Windows 10 that enable [Windows containers](https://docs.microsoft.com/en-us/virtualization/windowscontainers/quick-start/). You can now use both .NET Core and .NET Framework with Windows containers. 

These options give you a lot of choice in the way you build and package your .NET applications with Docker. This post describes some of those options and provides information on how to get started, even if you are completely new to Docker.

Why Containers?
---------------

There are multiple reasons why developers have been moving to containers to model, build, package, test and distribute their applications.

- **Consistent:** Containers include the application and all of its dependencies. The application executes the same code, regardless of computer, environment or cloud.
- **Lightweight:** Containers start quickly and use a minimal amount of RAM by using a minimal abstraction over the host operating system and sharing common resources across containers.
- **Sharing:** Containers are easy to share via [Docker Hub](https://hub.docker.com/), the [Docker Store](https://store.docker.com/), and private Docker registries.

There are plenty of other reasons why containers are catching on. The ones listed above are the key ones. 

Imagine just a few years ago telling someone that you care so much about consistency that you always ship your preferred operating system with your app. At least in the Windows ecosystem, you would have gotten some strange looks. Yet, that's exactly the model Docker uses.

As an example on the .NET Team, we realized that we needed to add test coverage for containers. Instead of "testing the container scenario", we decided to move significant parts of our engineering infrastructure to containers to get the same benefits listed above. This approach has provided us with the double benefit of developing high confidence on running .NET in containers and making our overall process more efficient and cheaper. 

Scenarios for .NET Applications
-------------------------------

The most obvious scenario for using Docker and .NET applications is for production deployment and hosting. It turns out that production is just one of the scenarios and the other ones are equally useful. These scenarios are not really specific to .NET, but apply to most developer platforms.

- **Low friction install** -- You can try out .NET without installing anything on your machine. Just download a Docker image with .NET in it.
- **Develop in a container** -- You can develop in a consistent environment, making development and production environments very similar (avoiding issues like global state on developer machines). Visual Studio Docker Tools even enable you to start a container directly from Visual Studio.
- **Test in a container** -- You can test in a container, reducing failures due to incorrectly configured environments or other changes left behind from the last test.
- **Build in a container** -- You can build code in a container, avoiding the need to correctly configure shared build machines for multiple environments but instead move to a "BYOC" (bring your own container) approach.
- **Deployment in test, stage, production and other environments** -- You can deploy a read-only image through all of your environments, reducing failures due to differences in configuration, typically only changing the behavior of the image via external configuration (for example, injected environment variables).

How to get started
------------------

You can get started using .NET docker containers right now, on Windows, macOS or Linux. First, you need a Docker client. The best place to get that is [Docker.com](https://www.docker.com/products/docker). If you are on Windows, we recommend [Docker for Windows](https://docs.docker.com/docker-for-windows/install/). It supports both Windows and Linux containers.

If you are new to Docker, I recommend that you check out the [Get Started with Docker](https://docs.docker.com/engine/getstarted/) section in the Docker documentation. That's where I started. The instructions use Linux containers, but don't let that scare you off if you are not a Linux user. The instructions are general and focus on general Docker concepts and mechanisms.

I'm a big fan of the [Docker Whalesay](https://docs.docker.com/engine/getstarted/step_three/) image that is used in the Docker gettng started instructiosn, so wanted something similar for .NET. We already had a [dotnet-bot](https://github.com/dotnet-bot) sample that does almost the same thing, so I only needed to package it up as a Docker image. I hope you like it!

Once you have basic knowledge of Docker, then try the samples we created for using .NET with Docker. They should help you get started with .NET and Docker in your environment. We created two sets of samples since there are key differences between .NET Core and .NET Framework that require different Dockerfiles and other artifacts.

- [.NET Core + Docker samples](https://github.com/dotnet/dotnet-docker-samples)
- [.NET Framework + Docker samples](https://github.com/microsoft/dotnet-framework-docker-samples)

To make trying these images even easier, we provisioned a couple of the images to DockerHub.

- [.NET Core sample images](https://hub.docker.com/r/microsoft/dotnet-samples/)
- [.NET Framework sample images](https://hub.docker.com/r/microsoft/dotnet-framework-samples/)

You can try .NET on your machine without installing anything (except Docker), by using the sample images. That said, I think trying the samples locally is still the best idea to fully experience and evaluate .NET with Docker.

As an example, for .NET Core, try the following Linux image (even on Windows):

```console
docker run microsoft/dotnet-samples

```

.NET in DockerHub
-----------------

One of the most important aspects of using .NET with Docker is relying on the .NET base images that the .NET Team provides. There are at least four reasons why using the .NET base images is a good idea:

- The .NET Team makes them so that you don't have to.
- The .NET Team updates them regularly, for both big and small releases and security updates.
- Docker shares the memory of common images when more than one application uses them on the same machine. The images have to be the same to be shared.
- Docker scans official images by default for security vulnerabilities, giving you more information about your environment.

Note: The .NET images are not yet "official" at the time of writing, but we hope they will be deemed official soon.

We publish our Docker images in a few different repositories on [Docker Hub](https://hub.docker.com/). It's important to segment images so that they are easier to find, both on the Docker Hub website as well as with the `docker search` command.

- [microsoft/dotnet](https://hub.docker.com/r/microsoft/dotnet/) -- .NET Core Runtime and SDK images for Linux and Nano server.
- [microsoft/aspnetcore](https://hub.docker.com/r/microsoft/aspnetcore/) -- ASP.NET Core images for Linux.
- [microsoft/aspnetcore-build](https://hub.docker.com/r/microsoft/aspnetcore-build/) -- ASP.NET Core images for Linux, intending for building apps.
- [microsoft/dotnet-framework](https://hub.docker.com/r/microsoft/dotnet-framework/) -- .NET Framework 3.5 and 4.6.2 images for Windows Server Core.
- [microsoft/aspnet](https://hub.docker.com/r/microsoft/aspnet/) -- .NET Framework 3.5 and 4.6.2 ASP.NET images for Windows Server Core.

We also publish samples to Docker Hub so that you can more easily try out the product.

- [microsoft/dotnet-samples](https://hub.docker.com/r/microsoft/dotnet-samples/) -- .NET Core samples.
- [microsoft/dotnet-framework-samples](https://hub.docker.com/r/microsoft/dotnet-framework-samples/) -- .NET Framework samples.

The instructions to use the base images and the samples are provided on Docker Hub at the links provided above.

Defining Docker images
----------------------

Docker images (including the .NET ones) are defined by a (relatively) simple file written in the [Dockerfile](https://docs.docker.com/engine/reference/builder/) format. You can find links to each of the Dockerfile files in the Docker Hub repositories provided above. 

These files define the set of images that we provide, their size, contents and other characteristics. In many cases, we've studied what other platforms have done with their images and have tried to follow that industry norm. In other cases, we've chosen to do something that works best for .NET apps that may or may not map to other platforms. In many cases Docker employees create the initial images for other platforms, so our liberal "copying" of patterns we see on Docker Hub is really just following Docker's lead. As the Docker norm changes, we'll do our best to follow it with the .NET images.

You'll quickly see that the Dockerfile source is stored on GitHub. You can follow the changes we make the images, see why we're making them and participate in that conversation if you'd like.

Docker Image Tag-ing
---------------------

Docker images have cryptic IDs (for example, d99acb94e777) as their primary means of identification by default. Since that's not super helpful for humans, Docker images can also be given tags. That's usually a friendly name that describes what the image is for, such as "hello-world-app". That model works great locally on your machine. On Docker Hub, the repository name becomes the name for the image and the tags are used to disambiguate images within the repository, by version or other aspect.

For .NET Core, tags are used to describe image differences on the following 3 axes:

- .NET Core Track - either LTS (1.0.3) or Current (1.1)
- .NET Core distribution - .NET Core Runtime, .NET Core SDK, .NET Core dependencies only
    - Temporarily we also have tags for MSBuild versus project.json tooling
- Operating System base image and version - Windows and Linux

For .NET Framework, tags are used to describe image differences on the following 2 axis:

- .NET Framework version -- 3.5, 4.6.2
- Operating system base image version -- Windows Server Core

.NET Core Tags
--------------

The .NET Core tags were recently changed significantly. One of the tags in a Docker repo is usually labeled `latest`. This `latest` image always has the very latest releases of a product. Our `latest` image has now changed from being project.json to MSBuild-based following our MSBuild development direction. You can still use [tags](https://hub.docker.com/r/microsoft/dotnet/tags/) even after they are updated and taken off the .NET Docker repo.

Segmenting repositories
-----------------------

There is no hard and fast rule that I've seen on how to structure repositories. I'd say that a good rule of thumb is the following:

> Docker pull on a repository should give a meaningful and intuitive image. The other images in the repository should pivot on a narrow set of additional concepts relative to that default image.

The more tags you have in your repository, the more likely it is that some of the images should be in another repository. This is something that we are discussing, including with Docker. We'd like to split the .NET Core repository into two or three repositories to make it easier to use. Unfortunately, there are no good mechanisms to discover other related repositories. We're hoping this gets fixed to enable us more freedom around repository factoring.

Update Model
------------

The .NET images may be updated quite often. You will want to opt into some of those updates and others you will get automatically (if you re-pull).

For Windows images, the .NET Dockerfile definitions rely on a specific base image that we don't expect will ever change. You can see that in the first line of the [microsoft/dotnet:1.0-runtime-nanoserver](https://github.com/dotnet/dotnet-docker/blob/master/1.0/nanoserver/runtime/Dockerfile) Dockerfile definition, included below:

```
FROM microsoft/nanoserver:10.0.14393.693
```

On "patch tuesdays" (the second tuesday of each month), the Windows Team will typically release patches and update their base images.

For Linux images, the .NET Dockerfile definitions rely on a more generic base image that will change over time. Currently, .NET Core targets Debian Jessie and rolls forward with updates to Jessie (for example 8.6 -> 8.7). We believe that this choice is reasonably safe and avoids significantly more complication in the .NET Core Docker images. We will not automatically roll forward to Debian Stretch but will create a new tag for it. This is similar to what other platforms do. You can see this policy in the first line of the [microsoft/dotnet:1.0-runtime-deps](https://github.com/dotnet/dotnet-docker/blob/master/1.0/debian/runtime-deps/Dockerfile) Dockerfile definition, included below:

```
FROM debian:jessie
```

Docker offers a service called AutoBuild that rebuilds higher-level images when base images change. The [microsoft/dotnet:1.0.0-preview2-sdk](https://github.com/dotnet/dotnet-docker/blob/master/1.0.0-preview2/debian/Dockerfile) image is automatically rebuilt when the underlying [debian:jesse](https://hub.docker.com/_/debian/) image is updated.
