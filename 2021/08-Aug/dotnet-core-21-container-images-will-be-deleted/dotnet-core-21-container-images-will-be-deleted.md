---
post_title: .NET Core 2.1 Container Images will be deleted
username: rlander@microsoft.com
microsoft_alias: rlander
categories: .NET, .NET Core
summary: Summary of your post, shown on the home page next to the featured image
desired_publication_date: 2021-08-19
---

Starting on August 21st, .NET Core 2.1 Docker container images will no longer be available on Docker Hub, but exclusively on Microsoft Container Registry (MCR). This change was previously announced with [dotnet/dotnet-docker #2848](https://github.com/dotnet/dotnet-docker/issues/2848). If you are reliant on .NET Core 2.1 images on Docker Hub, you should switch to using MCR immediately. Please reach out at dotnet@microsoft.com if this change is a problem for you / your organization.

We started [publishing .NET images to MCR](https://devblogs.microsoft.com/dotnet/net-core-container-images-now-published-to-microsoft-container-registry/) in early 2019, including for .NET Core 2.1. .NET Core 3 and later versions were published exclusively to MCR. The benefits of MCR are discussed in the referenced post.

[.NET Core 2.1 will go out of support](https://devblogs.microsoft.com/dotnet/net-core-2-1-will-reach-end-of-support-on-august-21-2021/) on August 21st. .NET Core 2.1 images will remain available on MCR. However, you are encouraged to immediately move to a later .NET version since .NET Core 2.1 will no longer be supported starting on the 21st. .NET Core 2.1 was [supported for three years](https://github.com/dotnet/core/blob/main/releases.md) starting in [August 2018](https://devblogs.microsoft.com/dotnet/net-core-august-2018-update/) after being [released in May of that year](https://devblogs.microsoft.com/dotnet/announcing-net-core-2-1/).

The rest of the post demonstrates the changes that you should make to pull .NET container images from MCR instead of Docker Hub.

## Pulling images from MCR

[.NET images](https://hub.docker.com/_/microsoft-dotnet) are available on MCR from the following repos:

- `mcr.microsoft.com/dotnet/runtime-deps`
- `mcr.microsoft.com/dotnet/runtime`
- `mcr.microsoft.com/dotnet/aspnet`
- `mcr.microsoft.com/dotnet/sdk`

You need to make the following transformations to pull .NET Core 2.1 images from MCR instead of Docker Hub.

For the SDK:

```Dockerfile
microsoft/dotnet:2.1-sdk -> mcr.microsoft.com/dotnet/sdk:2.1
microsoft/dotnet:2-sdk -> mcr.microsoft.com/dotnet/sdk:2.1
microsoft/dotnet:2.1-sdk-stretch -> mcr.microsoft.com/dotnet/sdk:2.1-stretch
microsoft/dotnet:2.1-sdk-stretch-arm32v7 -> mcr.microsoft.com/dotnet/sdk:2.1-stretch-arm32v7
microsoft/dotnet:2.1-sdk-nanoserver-1809 -> mcr.microsoft.com/dotnet/sdk:2.1-nanoserver-1809
microsoft/dotnet:2.1-sdk-alpine -> mcr.microsoft.com/dotnet/sdk:2.1-alpine
microsoft/dotnet:2.1-sdk-bionic -> mcr.microsoft.com/dotnet/sdk:2.1-bionic
microsoft/dotnet:2.1-sdk-bionic-arm32v7 -> mcr.microsoft.com/dotnet/sdk:2.1-bionic-arm32v7
microsoft/dotnet:latest -> mcr.microsoft.com/dotnet/sdk:2.1
```

For the ASP.NET Core:

```Dockerfile
microsoft/dotnet:2.1-aspnetcore-runtime -> mcr.microsoft.com/dotnet/aspnet:2.1
microsoft/dotnet:2-aspnetcore-runtime-> mcr.microsoft.com/dotnet/aspnet:2.1
microsoft/dotnet:2.1-aspnetcore-runtime-stretch-slim -> mcr.microsoft.com/dotnet/aspnet:2.1-stretch-slim
microsoft/dotnet:2.1-aspnetcore-runtime-stretch-slim-arm32v7 -> mcr.microsoft.com/dotnet/aspnet:2.1-stretch-slim-arm32v7
microsoft/dotnet:2.1-aspnetcore-runtime-nanoserver-1809 -> mcr.microsoft.com/dotnet/aspnet:2.1-nanoserver-1809
microsoft/dotnet:2.1-aspnetcore-runtime-alpine -> mcr.microsoft.com/dotnet/aspnet:2.1-alpine
microsoft/dotnet:2.1-aspnetcore-runtime-bionic -> mcr.microsoft.com/dotnet/aspnet:2.1-bionic
microsoft/dotnet:2.1-aspnetcore-runtime-bionic-arm32v7 -> mcr.microsoft.com/dotnet/aspnet:2.1-bionic-arm32v7
microsoft/dotnet:aspnetcore-runtime-> mcr.microsoft.com/dotnet/aspnet:2.1
```

For .NET Runtime:

```Dockerfile
microsoft/dotnet:2.1-runtime -> mcr.microsoft.com/dotnet/runtime:2.1
microsoft/dotnet:2-runtime -> mcr.microsoft.com/dotnet/runtime:2.1
microsoft/dotnet:2.1-runtime-stretch-slim -> mcr.microsoft.com/dotnet/runtime:2.1-stretch-slim
microsoft/dotnet:2.1-runtime-stretch-slim-arm32v7 -> mcr.microsoft.com/dotnet/runtime:2.1-stretch-slim-arm32v7
microsoft/dotnet:2.1-runtime-nanoserver-1809 -> mcr.microsoft.com/dotnet/runtime:2.1-nanoserver-1809
microsoft/dotnet:2.1-runtime-alpine -> mcr.microsoft.com/dotnet/runtime:2.1-alpine
microsoft/dotnet:2.1-runtime-bionic -> mcr.microsoft.com/dotnet/runtime:2.1-bionic
microsoft/dotnet:2.1-runtime-bionic-arm32v7 -> mcr.microsoft.com/dotnet/runtime:2.1-bionic-arm32v7
microsoft/dotnet:runtime -> mcr.microsoft.com/dotnet/runtime:2.1
```

For .NET Runtime dependencies:

```Dockerfile
microsoft/dotnet:2.1-runtime-deps -> mcr.microsoft.com/dotnet/runtime-deps:2.1
microsoft/dotnet:2.1-runtime-deps-stretch-slim -> mcr.microsoft.com/dotnet/runtime-deps:2.1-stretch-slim
microsoft/dotnet:2.1-runtime-deps-stretch-slim-arm32v7 -> mcr.microsoft.com/dotnet/runtime-deps:2.1-stretch-slim-arm32v7
microsoft/dotnet:2.1-runtime-deps-alpine -> mcr.microsoft.com/dotnet/runtime-deps:2.1-alpine
microsoft/dotnet:2.1-runtime-deps-bionic -> mcr.microsoft.com/dotnet/runtime-deps:2.1-bionic
microsoft/dotnet:2.1-runtime-deps-bionic-arm32v7 -> mcr.microsoft.com/dotnet/runtime-deps:2.1-bionic-arm32v7
microsoft/dotnet:runtime-deps -> mcr.microsoft.com/dotnet/runtime-deps:2.1
```

## Summary

We have been working closely with Docker Inc. for multiple years to transition Microsoft container images to Microsoft Container Registry. Given the popularity of .NET images, we chose the .NET Core 2.1 end-of-support date as the final date for hosting .NET images on Docker Hub. As stated earlier, .NET Core 2.1 images have been available on MCR since 2019 and .NET Core 3 and later images versions have been exclusively available on MCR. Please move to MCR for pulling all .NET container images and to a [supported .NET version](https://github.com/dotnet/core/blob/main/releases.md).
