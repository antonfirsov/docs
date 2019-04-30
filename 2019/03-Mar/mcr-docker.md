# .NET Core Container Images now Published to Microsoft Container Registry

We are now publishing .NET Core container images to [Microsoft Container Registry (MCR)](https://azure.microsoft.com/en-us/blog/microsoft-syndicates-container-catalog/). We have also made other changes to the images we publish, described in this post.

**Important:** You will need to change `FROM` statements in `Dockerfile` files and `docker pull` commands as a result of these changes. 3.0 references need to be changed now. Most 1.x and 2.x usages can be changed over time. The new tag scheme is decribed in this post and are provided at the [microsoft-dotnet-core](https://hub.docker.com/_/microsoft-dotnet-core/) repo, our new home on Docker Hub.

Summary of changes:

* .NET Core images are now published to Microsoft Container Registry.
* Updates will continue to be published to Docker Hub, for .NET Core 1.x and 2.x.
* .NET Core 3.0 will only be published to MCR.
* Nano Server 2016 images are no longer supported or published.

## Microsoft Container Registry (MCR)

Microsoft teams are now publishing container images to [MCR](https://azure.microsoft.com/en-us/blog/microsoft-syndicates-container-catalog/). There are two key reasons for this change:

* We can establish MCR as the official source of  Microsoft-provided container images, and then more easily promote and syndicate those images to multiple container services, like Docker Hub and Red Hat OpenShift.
* We can use Microsoft Azure as a global content distribution network (CDN) for delivering Microsoft-provided container images from locations closer to you. This means your container images pulls will be faster and have improved reliability in many cases.

From an architectural perspective, MCR is a globally replicated service that handles image manifest requests. It uses the [Azure CDN](https://azure.microsoft.com/en-us/services/cdn/) service for image layer requests. This separation isn't observable with `docker pull`, but it is easy to see when you [inspect .NET Core images with curl](https://gist.github.com/richlander/2793148e78074b91f810ba1dcdd1fd51). The use of globally replicated resources helps to demonstrate our commitment to providing a great experience for container users across the world.

## Continuing to Support Docker Hub

We will continue to maintain [Docker Hub repo pages](https://hub.docker.com/_/microsoft-dotnet-core/) so that you can discover and learn about [.NET Core images](https://hub.docker.com/_/microsoft-dotnet-core/). The Docker Hub website URLs you've used for Microsoft repos will continue to work, and forward to updated locations on Docker Hub.

You will use and see MCR as the storage backend for Microsoft container images, BUT the primary way you learn about Microsoft container images and tags will be through a container hub or website, which for many users will continue to be Docker Hub.

Existing Docker Hub images will be maintained as-is. In fact, we will continue to update the existing microsoft/dotnet repo, as described later in this post.

## .NET Core Images are on MCR

We [started publishing images to MCR](https://github.com/dotnet/dotnet-docker/issues/935) in February 2019, starting with the [.NET Core "nightly" repo](https://hub.docker.com/_/microsoft-dotnet-core-nightly). In early March, we moved the [.NET Core repo](https://hub.docker.com/_/microsoft-dotnet-core) as well.

On Docker Hub, we had one very large repo that served up four image types for four operating system distributions and three CPU types. This broad set of tags made for very long tag names and even longer README files. We decided to take this opportunity to [re-factor .NET Core into multiple repos](https://hub.docker.com/_/microsoft-dotnet-core), one for each image type. We also added a "product repo" that groups all of our repos together.

The new repos follow:

* [.NET Core product repo (no images)](https://hub.docker.com/_/microsoft-dotnet-core/)
* [.NET Core SDK](https://hub.docker.com/_/microsoft-dotnet-core-sdk/)
* [ASP.NET Core runtime](https://hub.docker.com/_/microsoft-dotnet-core-aspnet/)
* [.NET Core Runtime](https://hub.docker.com/_/microsoft-dotnet-core-runtime/) 
* [.NET Core Runtime dependencies](https://hub.docker.com/_/microsoft-dotnet-core-runtimes-deps/)

Note: [.NET Core 2.0 is end-of-life (EOL)](https://devblogs.microsoft.com/dotnet/net-core-2-0-will-reach-end-of-life-on-september-1-2018/) therefore it will not be available on MCR, only on Docker Hub, as unsupported images. You'll want to move to [.NET Core 2.1 which is a Long-term Support (LTS) release](https://dotnet.microsoft.com/download/archives).

## Update .NET Core Image Tags

The following examples show you what the new `docker pull` tag strings look like for .NET Core. They are shown as `docker pull`, but the same strings need to be used in `Dockerfile` files for `FROM` statements.

These examples all target .NET Core 2.1, but the same pattern is used across any supported .NET Core version:

* **SDK:** `docker pull mcr.microsoft.com/dotnet/core/sdk:2.1`
* **ASP.NET Core Runtime:** `docker pull mcr.microsoft.com/dotnet/core/aspnet:2.1`
* **.NET Core Runtime:** `docker pull mcr.microsoft.com/dotnet/core/runtime:2.1`
* **.NET Core Runtime Dependencies:** `docker pull mcr.microsoft.com/dotnet/core/runtime-deps:2.1`

The following example demonstrates what a FROM statement looks like for the new MCR repos, using the `dotnet/core/sdk` repo as an example:

```Dockerfile
FROM mcr.microsoft.com/dotnet/core/sdk:2.1
```

If you use Alpine, for example, the tags are easily extended to include Alpine, using the `dotnet/core/runtime` repo as an example:

```Dockerfile
FROM mcr.microsoft.com/dotnet/core/runtime:2.1-alpine
```

You can look at the [.NET Core Docker Samples](https://github.com/dotnet/dotnet-docker/blob/master/samples/README.md) to see how the tag strings are used in practice.

## Continued Support for Docker Hub

We've been publishing images to Docker Hub for three or four years. There are likely thousands (if not millions) of scripts and [Dockerfiles](https://github.com/search?l=Dockerfile&q=%22FROM+microsoft%2Fdotnet%22&type=Code) that have been written that expect .NET container images on Docker Hub. As stated above, those artifacts will continue to work as-is.

We publish multiple forms of tags that provide varying levels of convenience and consistency. These differences pivot on the degree to which version numbers are specified, from completely specified to not present at all. The following example tags demonstrate the various forms of tags, from least- to most-specific:

* `latest`
* `2.2-runtime`
* `2.1.6-sdk`

We will continue publishing images for the first two tags forms (version-less and two-part versions) for the supported lifetime of the associated versions. We will not publish any new three-part versions (like the last example) to Docker Hub, but only to MCR. We expect that most scripts and Dockerfile files use either of the first two forms of tags, or are manually updated to adopt three-part tags on some regular cadence. If they are manually updated, they can be manually updated to pull images from MCR.

## .NET Core 3.0 Images

The move to MCR is happening part-way through the .NET Core 3.0 release, which gave us the option of making .NET Core 3.0 MCR-only. This makes our approach for MCR for .NET Core 3.0 different than the other supported versions. We initially published .NET Core 3.0 Preview images to Docker Hub. Starting with [.NET Core 3.0 Preview 3](https://devblogs.microsoft.com/dotnet/announcing-net-core-3-preview-3/), .NET Core 3.0 images will only be published to MCR. It is important that .NET Core 3.0 users transition to MCR soon.

The following are examples of .NET Core 3.0 tag strings, to help you move to MCR:

* **SDK:** `docker pull mcr.microsoft.com/dotnet/core/sdk:3.0`
* **ASP.NET Core Runtime:** `docker pull mcr.microsoft.com/dotnet/core/aspnet:3.0`
* **.NET Core Runtime:** `docker pull mcr.microsoft.com/dotnet/core/runtime:3.0`
* **.NET Core Runtime Dependencies:** `docker pull mcr.microsoft.com/dotnet/core/runtime-deps:3.0`

The following example demonstrates what a FROM statement looks like for .NET Core 3.0 on MCR, using the `dotnet/core/runtime` repo as an example:

```Dockerfile
FROM mcr.microsoft.com/dotnet/core/runtime:3.0
```

.NET Core 3.0 Preview 1 and Preview 2 images will remain available on Docker Hub, for three-part version tags. For Preview 1 and Preview 2, we also published two-part version tags, like `3.0-sdk` and `3.0-runtime`. We were concerned that some users would see those two-part version tags for .NET Core 3.0 on Docker Hub and believe that those were supported images and would be updated in the future. They will not be. To mitigate that, we deleted two-part version tags for 3.0 on Docker Hub. This approach enables us to clearly communicate during the preview period that everyone needs to move to MCR for 3.0 images as soon as possible. We appologize if this change negatively affected you.

Visual Studio 2019 Previews use the two-part 3.0 tags that were deleted. Users must update their `Dockerfile` files to ensure that their projects build correctly. We have provided a [sample Dockerfile](https://gist.github.com/richlander/4be82f104e5323cca8493eb35a3ea773) that provides the correct `FROM` statements for .NET Core 3.0 ASP.NET Core projects in Visual Studio 2019.

## Nano Server 2016 Images

[Nano Server 2016 is no longer supported](https://support.microsoft.com/en-us/help/4043450/products-reaching-end-of-support-for-2018) by the Windows team and they are no longer publishing updated container images for that version. As a result, we have stopped publishing Nano Server 2016 images to Docker Hub and MCR.

This affects .NET Core image tags in a few different ways:

* Manifest (AKA "[multi-arch](https://github.com/dotnet/announcements/issues/14)") tags no longer include an entry for Nano Server 2016. That means that manifest tags like `2.1-sdk` will no longer work on Windows Server 2016, Nano Server 2016, or Windows 10 1607. If you need to still use Nano Server 2016 based images (even though they are no longer supported), you will need to use tags that include the Windows version (these are non-manifest tags), for example `mcr.microsoft.com/dotnet/core/runtime:2.1-nanoserver-sac2016`.
* .NET Core 2.x and 3.0 images are supported and available for all supported versions of Nano Server starting with version 1709.  This means that the 2.x and 3.0 manifest tags can be used on Windows 10, version 1709+, and Windows Server, version 1709+. You can also use non-manifest tags for those versions, too.
* We only produce Nano Server, version 1809 images for .NET Core 1.x. Previously, we only produced Nano Server, version 2016 images for .NET Core 1.x. You would have used either a manifest tag (like `1.1-runtime` or `1.1`) or a `nanoserver-sac2016` tag to pull those images.  You can pull the new .NET Core 1.x Nano Server, version 1809 images by either using a manifest tag or a `nanoserver-1809` tag.  These tags are only supported on Windows 10, version 1809 and Windows Server 2019

.NET Core images for Nano Server 2016 are still available on Docker Hub and MCR and will not be deleted. You can continue to use them but they are not supported and will not get new updates. If you need to do this and previously used manifest tags, like `1.1-sdk`, you can now use the following MCR tags (Docker Hub variants are similar):

- `2.2.2-nanoserver-sac2016`, `2.2-nanoserver-sac2016`
- `2.1.8-nanoserver-sac2016`, `2.1-nanoserver-sac2016`
- `1.1.11-nanoserver-sac2016`, `1.1-nanoserver-sac2016`
- `1.0.14-nanoserver-sac2016`, `1.0-nanoserver-sac2016`

Please note that [.NET Core 1.x will go out of support on June 27, 2019](https://devblogs.microsoft.com/dotnet/net-core-1-0-and-1-1-will-reach-end-of-life-on-june-27-2019). We recommend that .NET Core 1.x users move to .NET Core 2.1.

## At DockerCon 2019

We are sending a few team members to [DockerCon 2019](https://www.docker.com/dockercon/). Contact us @ dotnet@microsoft.com if you'd like to meet up and talk how you use .NET and Docker together. We'd love to hear about your approach and any challenges that you face, or changes you'd like us to make.

We've been attending DockerCon for a few years now and always enjoy the show. It is a good opportunity to learn the new ways people are using containers and also what new features are coming. As an example, we're still waiting for official support for [BuildKit](https://github.com/moby/buildkit). It is the feature we most want to see become part of the default feature set of Docker.

## Closing

We are continuing to improve the experience using .NET Core container images from Microsoft. Publishing .NET Core container images to MCR will be an improvement since MCR is globally replicated.

.NET Framework container images are not yet available on MCR. We will be moving them to MCR shortly.

See [Using .NET and Docker Together](https://devblogs.microsoft.com/dotnet/using-net-and-docker-together-dockercon-2018-update/) if you want to learn more about using Docker.
