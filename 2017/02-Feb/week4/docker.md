.NET and Docker
===============

Many developers I talk to are either using Docker actively or planning to adopt containers in their environment. Containers are an important trend in our industry and .NET is part of that. [Microsoft and Docker](https://weblogs.asp.net/scottgu/docker-and-microsoft-integrating-docker-with-windows-server-and-microsoft-azure) have been working together so that you'll have a great experience using Docker with .NET apps.

The Docker ecosystem started as a Linux technology. You can use a similar Docker workflow with Linux and .NET Core as you may have used with other development platforms. The .NET team publishes Debian images to the [microsoft/dotnet](https://hub.docker.com/r/microsoft/dotnet/) repository on Docker Hub frequently.

The Windows team recently released Windows Server 2016 and updates to Windows 10 that enable a [container experience on Windows](https://docs.microsoft.com/virtualization/windowscontainers/quick-start/). You can now use both [.NET Core](https://hub.docker.com/r/microsoft/dotnet/) and [.NET Framework](https://hub.docker.com/r/microsoft/dotnet-framework/) with Windows containers. 

These options give you a lot of choices in the way you build and package your .NET applications with Docker. This post describes some of those options and provides information on how to get started, even if you are completely new to Docker.

Why Containers?
---------------

There are multiple reasons why developers have been moving to containers to model, build, package, test and distribute their applications.

- **Consistent:** Containers include the application and all of its dependencies. The application executes the same code, regardless of computer, environment or cloud.
- **Lightweight:** Containers start quickly and use a minimal amount of RAM by using a minimal abstraction over the host operating system and sharing common resources across containers.
- **Sharing:** Containers are easy to share via [Docker Hub](https://hub.docker.com/), the [Docker Store](https://store.docker.com/), and private Docker registries, such as the [Azure Container Registry](https://azure.microsoft.com/services/container-registry/).

There are plenty of other reasons why containers are catching on. The ones listed above are the key ones.

Imagine just a few years ago telling someone that you care so much about consistency that you always ship your preferred operating system with your app. At least in the Windows ecosystem, you would have gotten some strange looks. Yet, that's exactly the model Docker uses.

As an example on the .NET Team, we realized that we needed to add test coverage for containers. Instead of "testing the container scenario", we decided to move significant parts of our engineering infrastructure to containers to get the same benefits listed above. This approach has provided us with the double benefit of developing high confidence in running .NET in containers and making our overall process more efficient and cheaper. 

Scenarios for .NET Applications
-------------------------------

The most obvious scenario for using Docker and .NET applications is for production deployment and hosting. It turns out that production is just one scenario and the other ones are equally useful. These scenarios are not specific to .NET, but should apply to most developer platforms.

- **Low friction install** -- You can try out .NET without installing anything on your machine. Just download a Docker image with .NET in it.
- **Develop in a container** -- You can develop in a consistent environment, making development and production environments very similar (avoiding issues like global state on developer machines). Visual Studio Docker Tools even enable you to start a container directly from Visual Studio.
- **Test in a container** -- You can test in a container, reducing failures due to incorrectly configured environments or other changes left behind from the last test.
- **Build in a container** -- You can build code in a container, avoiding the need to correctly configure shared build machines for multiple environments but instead move to a "BYOC" (bring your own container) approach.
- **Deployment in test, stage, production and other environments** -- You can deploy an image through all of your environments, reducing failures due to differences in configuration, typically only changing the behavior of the image via external configuration (for example, injected environment variables).

How to get started
------------------

You can get started using .NET Docker containers right now, on Windows, macOS or Linux. First, you need a Docker client. The best place to get that is [Docker.com](https://www.docker.com/community-edition). If you are on Windows, we recommend [Docker for Windows](https://docs.docker.com/docker-for-windows/install/) (Stable channel). It supports both Windows and Linux containers.

If you are new to Docker, I recommend that you check out the [Get Started with Docker](https://docs.docker.com/get-started/) section in the Docker documentation. That's where I started. The instructions use Linux containers, but don't let that scare you off if you are not a Linux user. You can find instructions for using the Docker client on your OS as well as guides that focus on general Docker concepts and mechanisms.

Next, try the samples we created for using .NET with Docker. They should help you get started with .NET and Docker in your environment. We created two sets of samples since there are key differences between .NET Core and .NET Framework that require different Dockerfiles and other artifacts.

- [.NET Core + Docker samples](https://github.com/dotnet/dotnet-docker-samples)
- [.NET Framework + Docker samples](https://github.com/microsoft/dotnet-framework-docker-samples)

To make trying these images even easier, we pushed a couple of the images to Docker Hub.

- [.NET Core sample images](https://hub.docker.com/r/microsoft/dotnet-samples/)
- [.NET Framework sample images](https://hub.docker.com/r/microsoft/dotnet-framework-samples/)

You can try .NET on your machine without installing anything (except Docker), by using the sample images. That said, I think trying the samples locally is still the best idea to fully experience and evaluate .NET with Docker.

As an example, for .NET Core, try the following Linux image (if using Docker for Windows be sure to switch to Linux Containers):

```console
docker run microsoft/dotnet-samples
```

.NET in Docker Hub
-----------------

One of the most important aspects of using .NET with Docker is relying on the .NET base images that the .NET Team provides. There are at least four reasons why using the .NET base images is a good idea:

- The .NET Team makes them so that you don't have to.
- The .NET Team updates them regularly, for both big and small releases and security updates.
- Docker shares the memory of common images when more than one application uses them on the same machine. The images have to be the same to be shared.
- Docker scans images for security vulnerabilities, giving you more information about your environment.

We publish our Docker images in a few different repositories on [Docker Hub](https://hub.docker.com/). It's important to segment images so that they are easier to find, both on the Docker Hub website as well as with the `docker search` command.

- [microsoft/dotnet](https://hub.docker.com/r/microsoft/dotnet/) -- .NET Core Runtime and SDK images for Linux and Nano Server.
- [microsoft/aspnetcore](https://hub.docker.com/r/microsoft/aspnetcore/) -- ASP.NET Core images for Linux and Nano Server.
- [microsoft/aspnetcore-build](https://hub.docker.com/r/microsoft/aspnetcore-build/) -- ASP.NET Core images for Linux and Nano Server, intended for building apps.
- [microsoft/dotnet-framework](https://hub.docker.com/r/microsoft/dotnet-framework/) -- .NET Framework 3.5 and 4.6.2 images for Windows Server Core.
- [microsoft/aspnet](https://hub.docker.com/r/microsoft/aspnet/) -- .NET Framework 3.5 and 4.6.2 ASP.NET images for Windows Server Core.

We also publish samples to Docker Hub so that you can more easily try out the product.

- [microsoft/dotnet-samples](https://hub.docker.com/r/microsoft/dotnet-samples/) -- .NET Core samples.
- [microsoft/dotnet-framework-samples](https://hub.docker.com/r/microsoft/dotnet-framework-samples/) -- .NET Framework samples.

The instructions to use the base images and the samples are available on Docker Hub at the links provided above. If you are new to the Docker layering concept we suggest reading Docker's [Understand images, containers, and storage drivers](https://docs.docker.com/engine/userguide/storagedriver/imagesandcontainers/) documentation.

Defining Docker images
----------------------

Docker images (including the .NET ones) are defined by a (relatively) simple file written in the [Dockerfile format](https://docs.docker.com/engine/reference/builder/). You can find links to each of the Dockerfile files in the Docker Hub repositories provided above. 

These files define the set of images that we provide, their size, contents and other characteristics. In many cases, we've studied what other platforms have done with their images and have tried to follow industry norms. In other cases, we've chosen to do something that works best for .NET apps that may or may not map to other platforms. In many cases Docker employees create the initial images for other platforms, so our liberal "copying" of patterns we see on Docker Hub is really just following Docker's lead. As the Docker norm changes, we'll do our best to follow it with the .NET images.

You'll quickly see that the Dockerfile source is stored on GitHub. You can follow the changes we make to the images, see why we're making them and participate in that conversation if you'd like.

Docker Image Tag-ing
---------------------

Docker images have cryptic IDs (for example, d99acb94e777) for identification by default. Since that's not super helpful for humans, Docker images can be given tags. That's usually a friendly name that describes what the image is for, such as "hello-world-app". That model works great locally on your machine. 

On Docker Hub, the repository name becomes the name for the image and the tags are used to disambiguate images within the repository, by version or other aspect.

.NET Core Tags
--------------

For .NET Core, [tags](https://hub.docker.com/r/microsoft/dotnet/tags/) are used to describe image differences on the following 3 axes:

- .NET Core Version - .NET Core 1.0, 1.1 and 2.0 (at the time of writing).
- .NET Core distribution - .NET Core Runtime, .NET Core SDK, .NET Core dependencies only
- Operating System base image - Windows and Linux

One of the key choices for the dotnet repository was the behavior of the `latest` tag. We made the choice that `latest` would always point to the latest .NET Core SDK version. For example, `latest` will be updated to point to the .NET Core SDK 2.0 when it ships as RTM. 

The alternative would would have been mapping `latest` to the .NET Core Runtime. We felt that the SDK is the best image to start with and that its easier to refine your choices once you've have some experience with the larger SDK image.

We recently changed [.NET Core Docker images to use multi-arch based tags](https://github.com/dotnet/announcements/issues/14). This change reduces the number of places you need to consider the third axis above. It means that your Dockerfile files no longer have to define which operating system that you are targeting.

.NET Framework Tags
-------------------

For .NET Framework, [tags](https://hub.docker.com/r/microsoft/dotnet-framework/tags/) are used to describe image differences on just 1 axis:

- .NET Framework version -- 3.5, 4.6.2

The `latest` tag maps to the highest .NET Framework version. `latest` will be updated to point to a `4.7` image when it becomes available.

Segmenting repositories
-----------------------

There is no hard and fast rule that I've seen on how to structure repositories. I'd say that a good rule of thumb is the following:

> `docker pull` on a repository should provide a meaningful and intuitive image. The other images in the repository should pivot on a narrow set of additional concepts relative to that default image.

The more tags you have in your repository, the more likely it is that some of the images should be in another repository. This is something that we are discussing, including with Docker. We'd like to split the .NET Core repository into two or three repositories to make it easier to use. Unfortunately, there are no good mechanisms to discover other related repositories. We're hoping this gets fixed to enable us more freedom around repository factoring.

Update Model
------------

The .NET images are updated quite often, sometimes for [security updates](https://blogs.msdn.microsoft.com/dotnet/2017/05/09/net-core-may-2017-update/). You can download these updates by performing a `docker pull`. `docker build` does not request updates from the server.

For Windows images, the .NET Dockerfile definitions rely on a specific base image. You can see that in the first line of the [microsoft/dotnet:1.0-runtime-nanoserver](https://github.com/dotnet/dotnet-docker/blob/master/1.0/nanoserver/runtime/Dockerfile) Dockerfile definition, included below:

```
FROM microsoft/nanoserver:10.0.14393.1066
```

On "patch Tuesdays" (the second Tuesday of each month), the Windows Team will typically release patches and update their base images. This will result in updated .NET images.

For Linux images, the .NET Dockerfile definitions rely on a more generic base image that will change over time. Currently, .NET Core targets Debian Jessie and rolls forward with updates to Jessie (for example 8.6 -> 8.7). We believe that this choice is reasonably safe and avoids significantly more complication in the .NET Core Docker images. You can see this policy in the first line of the [microsoft/dotnet:1.0-runtime-deps](https://github.com/dotnet/dotnet-docker/blob/master/1.0/runtime-deps/jessie/Dockerfile) Dockerfile definition, included below:

```
FROM debian:jessie
```

The [Debian repo](https://hub.docker.com/_/debian/) includes Debian Stretch (Debian 9) images. It's the next version after Debian Jessie (Debian 8). We will not automatically roll forward .NET Core tags that use Debian Jessie to Debian Stretch but will create a new tag for it. This is similar to what other platforms do.

Docker offers a service called AutoBuild that rebuilds higher-level images when base images change. The .NET Core images make use of this. For example, the [microsoft/dotnet:1.0-sdk](https://github.com/dotnet/dotnet-docker/blob/master/1.0/debian/sdk/Dockerfile) image is automatically rebuilt when the underlying [debian:jesse](https://hub.docker.com/_/debian/)image is rebuilt.

Migrating .NET Framework Applications to Containers
-------------------------------------------

Wouldn't it be exciting if there was a tool to convert VHDs to Docker containers?

At [DockerCon 2017](http://2017.dockercon.com/), there was **much excitement** around a tool designed for migrating Windows VMs to containers called [Image2Docker](https://github.com/docker/communitytools-image2docker-win). This tool is maintained by Docker on GitHub.

The Image2Docker Powershell module can be used with VHD, VHDX, or WIM image files and generate a high fidelity `Dockerfile` that builds a Docker image. The premise is that this tool will help you build a Docker image that is the same as your VHD but provides the benefits of Docker. This tool won't build the prettiest `Dockerfile` but it is a fantastic starting place for folks eager to migrate their .NET Framework applications to containers! 

You can view the [Image2Docker DockerCon talk](https://www.youtube.com/watch?v=YVfiK72Il5A) or read Docker's [Convert ASP.NET Web Servers To Docker with ImageDocker](https://blog.docker.com/2016/12/convert-asp-net-web-servers-docker-image2docker/) to learn more.

Note: You will need [Docker for Windows](https://www.docker.com/community-edition) installed to run Image2Docker. 

Closing
-------

As a team, we've been focussed on enabling .NET with Docker for about two years now. Our approach to Docker has changed a lot in that time, in large part due to adapting to the quick pace of change in the Docker ecosystem. We've also adopted Docker more ourselves. We'll continue to adapt our approach to ensure that .NET is one of the best platforms for containerized applications.

I'd like to share a "shout out" to our friends at Docker. We've been working with many of them with the goal of making .NET and Docker work great together. The Docker folks have also been very receptive when we've suggested improvements that we think would make life easier for .NET developers. Multi-arch tags and multi-stage builds are great examples of new scenarios that we've had the benefit of participating in early.

Please give .NET a try with Docker. We'd appreciating hearing about your experiences. In fact, we're interesting in finding some customers that have large Docker deployments with .NET. We want to learn from your experiences and apply that in the next batch of .NET and Docker improvements. Please provide your feedback in the comments below or drop us a line at dotnet@microsoft.com.
