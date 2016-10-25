.NET and Docker
===============

Many people I talk to are either using Docker actively or planning to adopt containers in their environment. Containers and containerization are an important trend in our industry and .NET is now part of that. Microsoft and Docker have been working together so that you'll have a great experience using Docker with .NET apps.

The Docker ecosystem has targeted Linux. You can use .NET Core with Debian Linux images, with a similar workflow and set of supported scenarios as other development platforms.

The Windows team recently released Windows Server 2016 and updates to Windows 10 that enable Windows containers. You can now use the both .NET Core and .NET Framework with Windows containers. 

These options give you a lot of choice in the way you build and containerize your .NET applications. This post describes some of those options and provides information on how to get started, even if you are completely new to Docker.

Why Containers?
---------------

There are multiple reasons why developers have been moving to containers to model, build, package, test and distribute their applications.

- **Consistent:** Containers include the application and all of its dependencies. The application executes the same code, regardless of computer, environment or cloud.
- **Lightweight:** Containers start instantly and use a minimal amount of RAM by using a minimal abstraction over the operating system and sharing common resources across containers.
- **Sharing:** Containers are easy to share via DockerHub, Docker Store and/or private Docker registries.

There are plenty of other reasons why containers are catching on. The ones listed above are the key ones. 

As an example on the .NET Team, we realized that we needed to add test coverage for containers. Instead of "testing the container scenario", we decided to move significant parts of our engineering infrastructure to containers to get the same benefits listed above. This approach has provided us with the double benefit of developing high confidence on running .NET in containers and making our overall process more efficient.

Scenarios for .NET Applications
-------------------------------

The most obvious scenario for using Docker and .NET applications is for production deployment and hosting. This is true and it works great. It turns out that production is just one of the scenarios and the other ones are equally useful. These scenario are not really specific to .NET, but apply to most developer platforms.

- **Low friction install** -- You can try out .NET without installing anything on your machine.
- **Develop in a container** -- You can develop in a consistent environment, making development and production environments very similar (avoiding issues like global state on developer machines).
- **Test in a container** -- You can test in a containers, reducing failures due to incorrectly configured environments.
- **Build in a container** -- You can build code in a container, avoiding the need to correctly configure shared build machines for multiple environments but instead move to a "BYOC" (Bring your own container) approach.
- **Hosting in test, stage, production and other environments** -- You can deploy a read-only image through all of your environments, reducing failures due to differences in configuration, typically only changing the behavior of the image via external configuration (for example, injected environment variables).

How to get Started
------------------

You can get started using .NET docker containers right now, on Windows, macOS or Linux. First, you need a Docker client. The best place to get that is [Docker.com](https://www.docker.com/products/docker). If you are Windows, we currently recommend the [Beta channel](https://docs.docker.com/docker-for-windows/) build since it supports both Windows and Linux containers. That functionality will eventually be available in the stable channel build.

If you are new to Docker, I recommend that you check out the [Get Started with Docker](https://docs.docker.com/engine/getstarted/) section in the Docker documentation. That's where I started. It's Linux specific, but it's pretty general and you can use it on Windows (with Linux containers), macOS or Linux.

Once you have basic knowledge of Docker, then should take a look at the .NET with Docker samples that a few of us created. I'm a big fan of the [Docker Whalesay](https://docs.docker.com/engine/getstarted/step_three/) image, so decided to create something very similar. We already had a [dotnet-bot](https://github.com/dotnet-bot) sample that did almost the same thing, so only needed to package it up as a Docker image.

We went one step further with the dotnet-bot image. Instead of packaging it up as a single "hello world" image, we created multiple variants of it to demontrate the scenarios that I listed above. You may want to replicate all of these scenarios and now you have a base to start from. We have two variants of the images, for [.NET Framework](https://github.com/microsoft/dotnet-framework-docker-samples) and [.NET Core](https://github.com/dotnet/dotnet-docker-samples). There are key differences the two platforms, which require different images.

To make trying these images even easier, we provisioned some of them to DockerHub. This means that you can try .NET on your machine without installing anything (except Docker) or git cloning our samples to your machine. That said, I think trying the samples locally is still the best idea to fully experience and evaluate .NET with Docker.

.NET in DockerHub
-----------------

One of the most important aspects of using .NET with Docker is relying on the .NET base images that the .NET Team provides. There are at least four reasons why using the .NET base images is a good idea:

- The .NET Team makes them so that you don't have to.
- The .NET Team updates them for both big, small and security update releases so that you don't have to.
- Docker shares the memory of common images when more than one application uses them on the same machine.
- Docker scans official images by default for security vulnerabilities, giving you more information about your environment.

Note: The .NET images are not yet "official" at the time of writing, but we hope that this will happen soon.

We publish our Docker images in a few different repositories on [Docker Hub](https://hub.docker.com/). It's important to segment images so that they are easier to find, both on the Docker Hub website as well as with the `docker search` command.

- [microsoft/dotnet](https://hub.docker.com/r/microsoft/dotnet/) -- .NET Core images for Linux and Nano server
- [microsoft/aspnetcore](https://hub.docker.com/r/microsoft/aspnetcore/) -- ASP.NET Core images for Linux
- [microsoft/dotnet-framework](https://hub.docker.com/r/microsoft/dotnet-framework/) -- .NET Framework 3.5 and 4.6.2 images for Windows Server Core
- [microsoft/aspnet](https://hub.docker.com/r/microsoft/aspnet/) -- .NET Framework 3.5 and 4.6.2 ASP.NET images for Windows Server Core

We also publish samples to Docker Hub so that you can more easily try out the product.

- [microsoft/dotnet-samples](https://hub.docker.com/r/microsoft/dotnet-samples/)
- [microsoft/dotnet-framework-samples](https://hub.docker.com/r/microsoft/dotnet-samples/)

The instructions to use both the base images and the samples are provided on Docker Hub.

The content of these Docker Hub repositories is defined by (relatively) simple files written in the [Dockerfile](https://docs.docker.com/engine/userguide/eng-image/dockerfile_best-practices/) format. All of these files are open source and can be viewed on GitHub. The links, per Dockerfile, are listed on our the various .NET Docker Hub pages.

Docker Image Tag-ing
---------------------

Docker images have cryptic IDs (for example, d99acb94e777) as their primary means of identification by default. Since that's not super helpful for humans, Docker images can also be given tags. That's usually a friendly name that describes what the image is for, such as "hello-world-app". That model works great locally on your machine. On Docker Hub, the repository name becomes the name for the image and the tags are used to disambiguate images within the repository, by version or other aspect.

For .NET Core, tags are used to describe differences in available images on 3 axes:

- .NET Core distribution - .NET Core SDK, .NET Core Runtime, .NET Core dependencies only
- Operating System base image and version - Windows and Linux
- Docker pattern - Regular vs. ONBUILD

For .NET Framework, tags are used to describe a smaller set of differences:

- .NET Framework version -- 3.5, 4.6.2
- Operating system base image version -- Windows Server Core

The more tags you have in your repository, the more likely it is that some of the images should be in another repository. This is something that we are discussing, including with Docker. We'd like to split the .NET Core repository into two or three repositories to make it easier to use. Unfortunately, there are no good mechanisms to discover other related repositories. We're hoping this gets fixed to enable us more freedom around repository factoring.

Update Model
------------

The .NET images may be updated quite often. You will want to opt into some of those updates and others you will get automatically (if you re-pull).

For Windows images, the .NET Dockerfile definitions rely on a specific base image that we don't expect will ever change. You can see that in the first line of the [microsoft/dotnet:1.0.0-preview2-nanoserver-sdk](https://github.com/dotnet/dotnet-docker/blob/master/1.0.0-preview2/nanoserver/Dockerfile) Dockerfile definition, included below:

```
FROM microsoft/nanoserver:10.0.14393.321
```

On "patch tuesdays" (the second tuesday of each month), the Windows Team will typically release patches and update their 

https://hub.docker.com/r/microsoft/dotnet/tags/

For Linux images, the .NET Dockerfile definitions rely on a more generic base image that will change over time. Currently, .NET Core targets Debian Jessie and rolls forward with updates to Jessie (for example 8.6 -> 8.7). We believe that this choice is reasonably safe and avoids significantly more complication in the .NET Core Docker images. We will not automatically roll forward to Debian Stretch but will create a new tag for it. This is similar to what other platforms do. You can see this policy in the first line of the [microsoft/dotnet:1.0.0-preview2-sdk](https://github.com/dotnet/dotnet-docker/blob/master/1.0.0-preview2/debian/Dockerfile) Dockerfile definition, included below:

```
FROM buildpack-deps:jessie-scm
```

Docker offers a service called AutoBuild that rebuilds higher-level images when base images change. The microsoft/dotnet:1.0.0-preview2-sdk](https://github.com/dotnet/dotnet-docker/blob/master/1.0.0-preview2/debian/Dockerfile) image is automatically rebuilt when [buildpack-deps:jessie-scm](https://hub.docker.com/_/buildpack-deps/) or the underlying [debian:jesse](https://hub.docker.com/_/debian/) image is updated.

