# Staying up-to-date with .NET Container Images

This post describes the container images that we produce and update for you, that you can use with Docker, Kubernetes and other systems. When you are using .NET and Docker together, you are probably using the official .NET container images from Microsoft. We've made many improvements over the last year to the .NET images that make it easier for you to containerize .NET applications.

Last week during [DockerCon 2018](https://2018.dockercon.com/), I posted an update about [Using .NET and Docker Together](https://blogs.msdn.microsoft.com/dotnet/2018/06/13/using-net-and-docker-together-dockercon-2018-update/). It demonstrates how you can use Docker with .NET, for production, development and testing. Those scenarios are all based on the .NET container images on Docker Hub. 

## Faster Software Delivery

Docker is a game changer for acquiring and using .NET updates. Think back to just a few years ago. You would download the latest .NET Framework as an MSI installer package on Windows and not need to download it again until we shipped the next version. Fast forward to today. We push updated container images to [Docker Hub](https://hub.docker.com) multiple times a month. Every time you pull .NET images, you are getting updated software, an update to .NET and/or the underlying operating system, either Windows or Linux. 

This new model of software delivery is much faster and creates a much stronger connection between software producer and consumer. It also gives you more control, but requires a bit more knowledge on how you acquire software, through Docker. It is important that you understand the Docker repos and tags that the software provider -- in this case Microsoft -- uses so that you get the exact versions and updates you want. This post is intended to provide you with the information you need to select the best versions of .NET images and tags for your needs.

## Official images from Docker

Docker maintains [official images](https://docs.docker.com/docker-hub/official_repos/), for operating systems and application platforms. These images are maintained by a combination of Docker, community developers and the operating system or application platform maintainers who are expert in Docker and the given technology (like [Alpine](https://hub.docker.com/_/alpine/)).

Official images are:

* Correctly and optimally configured.
* Regularly maintained.
* Can be shared (in memory) with other applications.

.NET images are built using [official images](https://hub.docker.com/official/). We build on top of [Alpine](https://hub.docker.com/_/alpine/), [Debian](https://hub.docker.com/_/debian/), and [Ubuntu](https://hub.docker.com/_/ubuntu/) for x64 and ARM. By using official images, we leave the cost and complexity of regularly updating operating system base images and packages like OpenSSL, for example, to the developers that are closest to those technologies. Instead, our build system is configured to rebuild and retest .NET images whenever the official images that we use are updated. That all happens automatically. Using that approach, we're able to offer .NET Core on multiple Linux distros at low cost and release updates to you within hours. There are also memory savings. A combination of .NET, Java, and Node.js apps run on the same host machine with the latest official Debian image, for example, will share the Debian base image in memory.

## .NET Images from Microsoft

.NET images are not part of the Docker official images because they are maintained solely by Microsoft. Similar to Docker official images, we have a team of folks maintaining .NET images that are expert in both .NET and Docker. This results in the same benefits as described for Docker official images, above.

We maintain .NET images with the following model:

* Push same-day image updates, when a new .NET version or base operating system image is released
* Push images to Docker Hub only after successful validation in our VSTS CI system
* Produce images that match the .NET version available in Visual Studio
* Make pre-release software available for early feedback and use on [microsoft/dotnet-nightly](https://hub.docker.com/r/microsoft/dotnet-nightly/)

We rely on Docker official maintainers to produce quality images in a timely manner so that our images are always up-to-date. We know you rely on us to do the same thing for .NET. We also know that many of you automatically rebuild your images, and the applications contained within them, when a new .NET image is made available. It is very important that this process works well, enabling your applications to always be running on the latest patched version of .NET and the rest of the software stack you have chosen to use. This is part of how we work together to ensure that .NET applications are highly secure and reliable in production.

## .NET Docker Hub Repos

Docker Hub is a great service that stores the world's public container images. When we first started pushing images to Docker Hub, we created fine-grained repositories. Many fine-grained repos has its advantages, but discoverability is not one of them. We heard feedback that it was hard to find .NET images. To help with that, we reduced the number of repos we use. The current set of .NET Docker Hub repos follows:

.NET Core repos:

* [microsoft/dotnet](https://hub.docker.com/r/microsoft/dotnet/) - includes .NET Core runtime, sdk, and ASP.NET Core images.
* [microsoft/aspnetcore](https://hub.docker.com/r/microsoft/dotnet/) - includes ASP.NET Core runtime images for .NET Core 2.0 and earlier versions. Use [microsoft/dotnet](https://hub.docker.com/r/microsoft/dotnet/) for .NET Core 2.1 and later.
* [microsoft/aspnetcore-build](https://hub.docker.com/r/microsoft/aspnetcore-build/) - Includes ASP.NET Core SDK and node.js for .NET Core 2.0 and earlier versions. Use [microsoft/dotnet](https://hub.docker.com/r/microsoft/dotnet/) for .NET Core 2.1 and later. See [aspnet/announcements #298](https://github.com/aspnet/Announcements/issues/298).

.NET Framework repos:

* [microsoft/dotnet-framework](https://hub.docker.com/r/microsoft/dotnet-framework/) - includes .NET Framework runtime and sdk images.
* [microsoft/aspnet](https://hub.docker.com/r/microsoft/aspnet/) - includes ASP.NET runtime images, for ASP.NET Web Forms and MVC, configured for IIS.
* [microsoft/wcf](https://hub.docker.com/r/microsoft/wcf/) - includes WCF runtime images configured for IIS.
* [microsoft/iis](https://hub.docker.com/r/microsoft/iis/) - includes IIS on top of the Windows Server Core base image. Works for but not optimized for .NET Framework applications. The [microsoft/aspnet](https://hub.docker.com/r/microsoft/aspnet/) and [microsoft/wcf](https://hub.docker.com/r/microsoft/wcf/) repos are recommended, instead.

Deprecated repositories

* [microsoft/dotnet-framework-build](https://hub.docker.com/r/microsoft/dotnet-framework-build/) - Includes the .NET Framework SDK. SDK images are now available at [microsoft/dotnet-framework](https://hub.docker.com/r/microsoft/dotnet-framework/).

## .NET Image Tags

The image tag you use in your `Dockerfile` is perhaps the most important artifact within your various Docker-related assets. The tag defines the underlying software that you expect to be using when you type `docker build` and that your app will be running on in production. The tag gives you a lot of control over the images you pull, but can also be a source of pain if the tag you are using doesn't align with your needs.

There are four main options for tags, from most general to most specific:

* **latest version** -- The `latest` tag aligns with the default version of the software available from a repository (for whatever the repository maintainer considers the default). When you don't specify a tag, your request pulls the `latest` tag. From one pull to the next, you might get software and/or the underlying operating system that has been updated by a major version. For example, when we shipped .NET Core 2.0, the latest tag (for Linux) jumped from .NET Core 1.1 to 2.0 and from Debian 8 to 9. Latest is great for experimentation, but not a good choice for anything else. You don't want your application to use `latest` in an automated build.
* **minor version** -- A major.minor tag, such as `2.0-runtime` or `2.1-sdk`, locks you to a specific family of updates of software. These example tags will receive only .NET Core 2.0 or .NET Core 2.1 updates, respectively. You can expect to get patch updates to the software and the underlying operating system with the use of `docker build --pull`. We recommend this form of tag for most cases. It balances the competing concerns of ease of use and the risk of updates. The `4.7.2-sdk` .NET Framework tag, for example, matches this tag style.
* **patch version** -- A major.minor.patch tag, such as `2.0.7-runtime` locks you to a specific patch version of software. This is great from a predictability standpoint, but you need to update your `Dockerfile` every time you want to update to a new patch version. That's a lot of work and requires quick action if you need to deploy one of our security updates for multiple applications. The `4.7.2-sdk-20180523-windowsservercore-1803` .NET Framework tag, for example, matches this tag style. We do not update the .NET contents in patch version images, but we may push new images for the tag due to underlying base image changes. As a result, do not consider patch version tags to be immutable.
* **digest** -- You can [reference an image digest directly](https://docs.docker.com/engine/reference/builder/#from). This approach gives you the most predictability. Tags can and are overwritten with new images while digests cannot be. Tags can also be deleted. We recommend using digests in the case an application breaks due to an image update and you need to go back to a "last known good" image. It can be challenging to determine the digest for an image you are no longer using. You can add logging into your image building infrastructure to collect this information as an insurance policy for unforeseen breakage.

Tags are a contract on the .NET version you want and the degree of change you expect. We do our best to satisfy that contract each time we ship. We do two main things to produce quality container images: CI validation and code-review. CI validation runs on several operating systems with each pull request to .NET repos. This level of pre-validation provides us with confidence on the quality of the Docker images we push to Docker Hub. 

For each major and minor .NET version, we may take a new major operating system version dependency. As I mentioned earlier, we adopted Debian 9 as the base image for .NET Core 2.0.  We stayed with Debian 9 for .NET Core 2.1, since Debian 10 (AKA "Buster") has not been released. Once we adopt an underlying operating system major version, we will not change it for the life of that given .NET release. 

Each distro has its own model for patches. For .NET patches, we will adopt minor Debian releases (Debian 9.3 -> 9.4), for example. If you look at .NET Core Dockerfiles, you can see the dependency on various Linux operating systems, such as [Debian](https://github.com/dotnet/dotnet-docker/blob/master/2.1/runtime-deps/stretch-slim/amd64/Dockerfile) and [Ubuntu](https://github.com/dotnet/dotnet-docker/blob/master/2.1/runtime-deps/bionic/amd64/Dockerfile). We make decisions that make sense within the context for the Windows and Linux operating systems that we support and the community that uses them.

Windows versioning with Docker works differently than Linux, as does the way multi-arch manifest tags work. In short, when you pull a .NET Core or .NET Framework image on Windows, you will get an image that matches the host Windows version, if you use the mult-arch tags we expose (more on that later). If you want a different version, you need to use the specific tag for that Windows version. Some Azure services, like [Azure Container Instances (ACI)](https://azure.microsoft.com/en-us/services/container-instances/) [only support Windows Server 2016](https://docs.microsoft.com/en-us/azure/container-instances/container-instances-troubleshooting#image-version-not-supported) (at the time of writing). If you are targeting ACI, you need to use a Windows Server 2016 tag, such as [4.7.2-runtime-windowsservercore-ltsc2016](https://hub.docker.com/r/microsoft/dotnet-framework/) or [2.1-aspnetcore-runtime-nanoserver-sac2016](https://hub.docker.com/r/microsoft/dotnet/), for .NET Framework and ASP.NET Core respectively.

## .NET Core Tag Scheme

There are multiple kinds of images in the [microsoft/dotnet](https://hub.docker.com/r/microsoft/dotnet/) repo:

* **sdk** -- .NET Core SDK images, which include the .NET Core CLI, the .NET Core runtime and ASP.NET Core.
* **aspnetcore-runtime** -- ASP.NET Core images, which include the .NET Core runtime and ASP.NET Core.
* **runtime** -- .NET Core runtime images, which include the .NET Core runtime.
* **runtime-deps** -- .NET Core runtime dependency images, which include only the dependencies of .NET Core and not .NET Core itself. This image is intended for [self-contained applications](https://docs.microsoft.com/dotnet/core/deploying/) and is only offered for Linux. For Windows, you can use the operating system base image directly for self-contained applications, since all .NET Core dependencies are satisfied by it.

We produce Docker images for the following operating systems:

* Windows Nano Server 2016+
* Debian 8+
* Alpine 3.7
* Ubuntu 18.04

.NET Core supports multiple chips:

* x64
* ARM32v7

Note: [ARM64v8 images](https://github.com/dotnet/dotnet-docker/pull/509) will be made available at a later time, possibly with .NET Core 3.0.

.NET Core tags follow a scheme, which describes the different combinations of kinds of images, operating systems and chips that .NET Core supports.

```
[version]-[kind]-[os]-[chip]
```

Note: This scheme is new with .NET Core 2.1. Earlier versions use a similar but slightly different scheme.

The following .NET Core 2.1 tags are examples of this scheme:

* 2.1.300-sdk-alpine3.6
* 2.1.0-aspnetcore-runtime-stretch-slim
* 2.1.0-runtime-nanoserver-1803
* 2.1.0-runtime-deps-bionic-arm32v7

Note: You might notice that some of these tags use strange names. "bionic" and "stretch" are the version names for Ubuntu 18.04 and Debian 9, respectively. They are the tag names used by the [ubuntu](https://hub.docker.com/_/ubuntu/) and [debian](https://hub.docker.com/_/debian/) repos, respectively. "stretch-slim" is a smaller variant of "stretch". We use smaller images when they are available. "nanoserver-1803" represents the Spring 2018 update of Windows Nano Server. "arm32v7" describes a 32-bit ARM-based image. [ARMv7](https://en.wikipedia.org/wiki/ARM_architecture#32-bit_architecture) is a 32-bit instruction set defined by [ARM Holdings](https://arm.com). 

They are also short-forms of .NET Core tags. The short-forms are *short* in two different ways. They use two part version numbers and skip the operating system. In most cases, the short-form tags are the ones you want to use, because they are simpler, are serviced, and are [multi-arch](https://blog.docker.com/2017/09/docker-official-images-now-multi-platform/) so are portable across operating systems. 

We recommend you use the following short-forms of .NET Core tags in your Dockerfiles:

* 2.1-sdk
* 2.1-aspnetcore
* 2.1-runtime
* 2.1-runtime-deps

As discussed above, some Azure services only support Windows Server 2016 (not Windows Server, version 1709+). If you use one of those, you may not be able to use short tags unless you happen to only build images on Windows Server 2016.

## .NET Framework Tag Scheme

There are multiple kinds of images in the [microsoft/dotnet-framework](https://hub.docker.com/r/microsoft/dotnet-framework/) repo:

* **sdk** -- .NET Framework SDK images, which include the .NET Framework runtime and SDK.
* **runtime** -- .NET Framework runtime images, which include the .NET Framework runtime.

We produce Docker images for the following Windows versions:

* Windows Server Core, version 1803
* Windows Server Core, version 1709
* Windows Server Core 2016

.NET Framework tags follow a scheme, which describes the different combinations of kinds of images, operating systems and chips that .NET Core supports:

[version]-[kind]-[timestamp]-[os]

The .NET Framework version number doesn't use the major.minor.patch scheme. The third part of the version number does not represent a patch version. As a result, we added a timestamp in the tag to create unique tag names. Like .NET Core, we recommend that you use short tag names.

The following .NET Framework tags are examples of this scheme:

* 4.7.2-sdk-20180523-windowsservercore-1803
* 4.7.2-runtime-20180523-windowsservercore-1709
* 3.5-sdk-20180523-windowsservercore-ltsc2016

They are also short-forms of .NET Framework tags. The short-forms are *short* in two different ways. They skip the timestamp and skip the operating system. In most cases, those are the tags you will want to use, because they are simpler, are serviced, and are [multi-arch](https://blog.docker.com/2017/09/docker-official-images-now-multi-platform/) so are portable across Windows versions. 

We recommend you use the following short-form of tags in your Dockerfiles, using .NET Framework 4.7.2 and 3.5 as examples:

* 4.7.2-sdk
* 4.7.2-runtime
* 3.5-sdk
* 3.5-runtime

As discussed above, some Azure services only support Windows Server 2016 (not Windows Server, version 1709+). If you use one of those, you may not be able to use short tags unless you happen to only build images on Windows Server 2016.

The [microsoft/aspnet](https://hub.docker.com/r/microsoft/aspnet/) and [microsoft/wcf](https://hub.docker.com/r/microsoft/wcf/) using a variant of this tag scheme and may move to this scheme in the future.

## Security Updates and Vulnerability Scanning

As you've probably picked up at this point, we update .NET images regularly so that you have the latest .NET and operating system patches available. For Windows, our image updates are similar in nature to the regular "Patch Tuesday" releases that the Windows team makes available on Windows Update. In fact, we update our Windows-based images with the latest Windows patches every Patch Tuesday. You don't run Windows Update in a container. You rebuild with the latest container images.

The update experience is more nuanced on Linux. We support multiple Linux distros that can be updated at any time. There is no specific schedule. In addition, there is usually a set of published vulnerabilities (AKA [CVEs](https://cve.mitre.org/)) that are unpatched, where no fix is available. This situation isn't specific to using Linux in containers but using Linux generally.

We have had customers ask us why .NET Core Debian-based images fail their vulnerability scans. I use [anchore.io](https://anchore.io/) for scanning and have validated the same scans that customers shared with us. The vulnerabilities are coming from the base images we use.

You can look at the same scans I look at:

* [Alpine `latest`](https://anchore.io/image/dockerhub/3fd9065eaf02feaf94d68376da52541925650b81698c53c6824d92ff63f98353?repo=library%2Falpine&tag=latest#security)
* [Debian `latest`](https://anchore.io/image/dockerhub/8626492fecd368469e92258dfcafe055f636cb9cbc321a5865a98a0a6c99b8dd?repo=library%2Fdebian&tag=latest#security)
* [Ubuntu `latst`](https://anchore.io/image/dockerhub/113a43faa1382a7404681f1b9af2f0d70b182c569aab71db497e33fa59ed87e6?repo=library%2Fubuntu&tag=latest#security)

One of observations is that the scan results change significantly over time for Debian and Ubuntu. For Alpine, they remain stable, with few vulnerabilities reported.

We have three approaches to this challenge:

* Rebuild and republish .NET images on the latest Linux distro patch updates immediately.
* Support the latest distro major versions as soon as they are available. We have observed that the latest distros are often patched quicker.
* Support Alpine, which is much smaller, so has fewer components that can have vulnerabilities.

These three approaches give you a lot of choice. We recommend you use .NET Core images that use the latest version of your chosen Linux distro. If you have a deeper concern about vulnerabilities on Linux, use the latest patch version of the [.NET Core 2.1 Alpine images](https://anchore.io/image/dockerhub/14aa89d7e9d9bf8f1634ddc249fefa353c51b9b579dc88c8ad38eadb0b287d22?repo=microsoft%2Fdotnet&tag=2.1-runtime-alpine3.7#security). If you are still unhappy with the situation, then consider using our Nano Server images. 

## Using Pre-release Images

We maintain a pre-release Docker Hub repository, [dotnet-nightly]([microsoft/dotnet](https://hub.docker.com/r/microsoft/dotnet-nightly/)). Nightly builds of .NET Core 2.1 images were available in that repo before .NET Core 2.1 shipped. There are nightly builds of the .NET Core 1.x and 2.x servicing branches in that repo currently. Before long, you'll see nightly builds of .NET Core 2.2 and 3.0 that you can test.

We also offer .NET Core with pre-release versions of Linux distros. We offered Ubuntu 18.04 (AKA "bionic") before it was released. We currently offer .NET Core images with pre-release versions of Debian 10 (AKA "buster") and the Alpine edge branch.

## Closing

We want to make it easy and intuitive for you to use the official .NET images that we produce, for Windows and Linux. Docker provides a great software delivery system that makes it easy to stay up-to-date. We hope that you have the right information, from this post, to configure your Dockerfiles and build system to use the tag style that provides you with the servicing characteristics and image consistency that makes sense for your environment.

We will continue to make changes to .NET container images as we receive feedback and as the Docker feature set changes. We post regular updates at [dotnet/announcements](https://github.com/dotnet/announcements/labels/Docker). "watch" that repo to keep up with the latest updates.

If you are new to Docker, check out [Using .NET and Docker Together](https://blogs.msdn.microsoft.com/dotnet/2018/06/13/using-net-and-docker-together-dockercon-2018-update/). It explains how to use .NET with Docker in a variety of scenarios. We also offer samples for both [.NET Core](https://github.com/dotnet/dotnet-docker/blob/master/samples/README.md) and [.NET Framework](https://github.com/Microsoft/dotnet-framework-docker/blob/master/samples/README.md) that are easy to use and learn from.

Tell us how you are using .NET and Docker together in the comments. We're interested to hear about the way you use .NET with containers and how you would like to see it improved.
