---
post_title: Improving multi-platform container support
author1: rlander@microsoft.com
post_slug: improving-multiplatform-container-support
username: rlander@microsoft.com
microsoft_alias: rlander
featured_image: dotnet-bot_cloudapps.png
categories: .NET, .NET Core, Containers, Linux
tags: Linux, containers
summary: Learn how to build containers for multiple architectures, including building containers on Apple M1 machines for an x64 cloud.
desired_publication_date: 2023-03-29
post_date: 2023-03-29 10:05:00
---

> This post was updated on April 15, 2024 to reflect the latest releases, fix broken links, and add illustrative images.

Docker makes it straightforward to build container images that target a specific hardware architecture (Arm and x64). This is particularly useful if you have an Arm64 M1/M2/M3 Apple Mac and want to build containers for an x64 cloud service. We are encouraging a new pattern for building container images for multiple architectures. It has better performance and integrates with the [BuildKit](https://www.docker.com/blog/how-to-rapidly-build-multi-architecture-images-with-buildx/) [build model](https://docs.docker.com/build/buildkit/).

This post is focused on building container images with `docker build`. You can also [build container images directly with the SDK](https://devblogs.microsoft.com/dotnet/streamline-container-build-dotnet-8/) but the pattern discussed in this post isn't required or relevant if you build container images with `dotnet publish`.

## Problem to solve

Many users build x64 .NET container images on Arm64 hardware. Doing that can result in the .NET SDK running in an emulated environment, which doesn't work well. You can see in the following screenshot that `dotnet restore` is taking a very long time to run when run with emulation. The pattern described in this post avoids this problem.

![dotnet restore hangs when run with processor architecture emulation](./docker-build-emulation.png)

## Solution

You can run `docker build` with a multi-platform friendly pattern. It has two aspects.

Use the `--platform` switch with `docker build`, which you already would be doing to target an alternate architecture:

```bash
docker build --platform linux/amd64 -t app .
```

Use `$BUILDPLATFORM` and `$TARGETARCH` environment variables in your [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/822166bbf88e23cbbbba6fedc3f56b99276a1e96/samples/aspnetapp/Dockerfile) to get the SDK to run natively and cross-compile/publish.

```Dockerfile
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
```

and

```Dockerfile
RUN dotnet publish -a $TARGETARCH
```

This pattern can be used with any .NET version. In particular, we added support for `-a` to support Docker `$TARGETARCH` values (which are really Golang processor architecture values and different from .NET ones) in the .NET 8 SDK ([dotnet/sdk #30762](https://github.com/dotnet/sdk/pull/30762)).

The rest of the post will go into much more detail.

## Multi-platform build

There are really two different scenarios at play, which I'm calling "multi-platform".

- Build a container image for a specific architecture (different than your machine).
- Build multiple container images at once, for multiple architectures.

Everything we're going to look at applies to both of these scenarios.

Let's start with the Docker [multi-platform model](https://docs.docker.com/build/building/multi-platform/). We'll assume that the user is using Apple Arm64 hardware. In fact, I'm writing this all on a MacBook Air M1 laptop, which will make it easy for me to demonstrate the behavior.

Docker has a `--platform` switch that you can use to control the output of your images. I'm going to show you how this system works using Alpine, to make the explanation as simple as possible.

Here's a simple Dockerfile to test the behavior. The `alpine` tag is a multi-platform tag (more on that shortly).

```dockerfile
FROM alpine
```

Let's build it and validate the results. Again, I'm on an Arm64 machine.

```bash
% docker build -t image .
% docker inspect image -f "{{.Os}}/{{.Architecture}}" 
linux/arm64
```

We have an Arm64 image, as expected.

We can target x64 by using the `--platform` switch.

```bash
% docker build -t image --platform linux/amd64 .
% docker inspect image -f "{{.Os}}/{{.Architecture}}"
linux/amd64
```

Now we have an x64 image. If we run the image, we can see that it is x64 -- via the `x86_64` string -- but there is a warning due to the image (which will be [emulated](https://www.qemu.org/)) and the implicit platform not matching.

```bash
% docker run --rm image uname -a
WARNING: The requested image's platform (linux/amd64) does not match the detected host platform (linux/arm64/v8) and no specific platform was requested
Linux 188008b850d3 5.15.49-linuxkit #1 SMP PREEMPT Tue Sep 13 07:51:32 UTC 2022 x86_64 Linux
```

`--platform` can be used to get rid of that error message.

```bash
% docker run --rm --platform linux/amd64 image uname -a
Linux 40b8d36a90f8 5.15.49-linuxkit #1 SMP PREEMPT Tue Sep 13 07:51:32 UTC 2022 x86_64 Linux
```

The `--platform` switch is a bit subtle. I'll explain.

The first take-away is that `--platform` is always set, implicitly as your machine platform or an explicitly specified one.

The bigger point is that `--platform` affects (A) multi-platform tags (like `alpine`) and (B) the configuration of the final image.

We can see that `alpine` is a multi-platform tag, supporting several architectures:

```bash
% docker manifest inspect alpine | grep architecture
            "architecture": "amd64",
            "architecture": "arm",
            "architecture": "arm",
            "architecture": "arm64",
            "architecture": "386",
            "architecture": "ppc64le",
            "architecture": "s390x",
```

The platform value (implicitly or explicitly specified) is used to pick which of those architecture-specific images to pull. Each one of those is a distinct image with its own separately compiled copy of Alpine Linux, all referenced from the `alpine` tag.

The Alpine team also publishes architecture-specific images, such as [`arm64v8/alpine`](https://hub.docker.com/r/arm64v8/alpine/).

```bash
% docker manifest inspect -v arm64v8/alpine | grep architecture
			"architecture": "arm64",
```

We can still use the `--platform` switch with this tag, but we'll get incorrect results.

```bash
% echo "FROM arm64v8/alpine" > Dockerfile
% docker build -t image .
% docker inspect image -f "{{.Os}}/{{.Architecture}}"
linux/arm64
% docker build -t image --platform linux/amd64 .
% docker inspect image -f "{{.Os}}/{{.Architecture}}"
linux/amd64
% docker run --rm -it image uname -a
WARNING: The requested image's platform (linux/amd64) does not match the detected host platform (linux/arm64/v8) and no specific platform was requested
Linux d9abfaec07a1 5.15.49-linuxkit #1 SMP PREEMPT Tue Sep 13 07:51:32 UTC 2022 aarch64 Linux
```

If you look closely, you'll see that this image is misconfigured. It is an `aarch64` image but is marked as `linux/amd64`. That's not very helpful.

In case you missed it in bash output, our Dockerfile now looks like the following.

```dockerfile
FROM arm64v8/alpine
```

You can safely and correctly use the `--platform` tag with Dockerfiles that have a mix of multi-platform and platform-specific tags, however, the final stage must reference a multi-platform image. In our updated Dockerfile, we have one `FROM` statement with a platform-specific tag, so using the `--platform` switch is user error and expected to result in a bad image.

We're almost done on background context. Docker also defines a bunch of `ARG` values, as you can see in the following [Dockerfile](https://gist.github.com/richlander/70cde3f0176d36862af80c41722acd47).

```dockerfile
FROM alpine

ARG TARGETPLATFORM
ARG TARGETOS
ARG TARGETARCH
ARG TARGETVARIANT
ARG BUILDPLATFORM
ARG BUILDOS
ARG BUILDARCH
ARG BUILDVARIANT
RUN echo "Building on $BUILDPLATFORM, targeting $TARGETPLATFORM"
RUN echo "Building on ${BUILDOS} and ${BUILDARCH} with optional variant ${BUILDVARIANT}"
RUN echo "Targeting ${TARGETOS} and ${TARGETARCH} with optional variant ${TARGETVARIANT}"
```

We can build the Dockerfile targeting x64 (on Arm64).

```bash
% docker build -t image --platform linux/amd64 .
[+] Building 1.7s (8/8) FINISHED                                                
 => [internal] load build definition from Dockerfile                       0.0s
 => => transferring dockerfile: 428B                                       0.0s
 => [internal] load .dockerignore                                          0.0s
 => => transferring context: 2B                                            0.0s
 => [internal] load metadata for docker.io/library/alpine:latest           0.0s
 => [1/4] FROM docker.io/library/alpine                                    0.0s
 => [2/4] RUN echo "Building on linux/arm64, targeting linux/amd64"        0.2s
 => [3/4] RUN echo "Building on linux and arm64 with optional variant "    0.3s
 => [4/4] RUN echo "Targeting linux and amd64 with optional variant "      0.3s
 => exporting to image                                                     0.0s
 => => exporting layers                                                    0.0s
 => => writing image sha256:c70637a6942e07f3a00eb03c1c6bf1cd8a709e91cd627  0.0s
 => => naming to docker.io/library/image
```

Make sure to pay close attention to those `RUN echo` lines.

These environment variables will prove useful when we look at our solution for .NET, in the next section.

## Build .NET images for any architecture

We're now going to look at how to build .NET images using similar techniques to what we just looked at with Alpine.

There are a few characteristics that we want:

- A single Dockerfile should work for multiple architectures.
- The result should be optimal.
- The SDK should always run with the native architecture, both because it is faster and because [.NET doesn't support QEMU](https://github.com/dotnet/core/blob/main/release-notes/6.0/supported-os.md#qemu).

We'll use [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/samples/aspnetapp/Dockerfile). This Dockerfile has been updated for .NET 8, both enabling [non-root hosting](https://devblogs.microsoft.com/dotnet/securing-containers-with-rootless/) and multi-platform targeting.

There are a few lines in this Dockerfile that do something special that help us achieve those characteristics.

```dockerfile
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
```

The Dockerfile format enables specifying the `--platform` switch for a `FROM` statement and to use a built-in `ARG` to provide the value. In this case, we're saying that the `$BUILDPLATFORM` (AKA the local machine architecture) should always be used. On an Arm64 machine, this will always be Arm64, even if targeting x64.

This pattern doesn't actually require .NET 8. It works with any .NET version. In fact, it will work with any multi-platform tag, like for Node.js or Java. It's just a Docker feature.

```dockerfile
RUN dotnet restore -a $TARGETARCH

# copy everything else and build app
COPY aspnetapp/. .
RUN dotnet publish -a $TARGETARCH --no-restore -o /app
```

We're now restoring and publishing the app for the target architecture, using another built-in argument.

Underneath the covers, the SDK is creating a Runtime ID (RID). That's the same as specifying `-r` argument. The `-a` argument is a shortcut for that. We recommend publishing apps as [RID-specific](https://learn.microsoft.com/dotnet/core/deploying/deploy-with-cli#native-dependencies) in containers. Some NuGet packages include multiple copies of native dependencies. By [publish an app as RID-specific](https://gist.github.com/richlander/4c07862f6434944f6eee1815237dabe1), you will get just one copy of those dependencies (reducing container size), matching your target environment.

If you are building an Alpine image and use this pattern, make sure that use an Alpine-based SDK, so that `linux-musl` will be inferred when you use `-a` to specify the architecture. Read [dotnet/sdk #30369](https://github.com/dotnet/sdk/issues/30369) for more information.

You might be used to seeing `-c Release` in Dockerfiles. We've made that the default for `dotnet publish` with .NET 8, making it optional. We've been working on improving .NET SDK defaults.

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0
```

The second (and last) `FROM` statement is a multi-platform tag. It will be affected by the `--platform` switch (from `docker build`). That's what we want.

Let's try it out.

```bash
% pwd
/Users/rich/git/dotnet-docker/samples/aspnetapp
% docker build --pull -t aspnetapp .
% docker inspect aspnetapp -f "{{.Os}}/{{.Architecture}}" 
linux/arm64
```

We see an Arm64 image.

Let's try targeting x64.

```bash
% docker build --pull -t aspnetapp --platform linux/amd64 .
% docker inspect aspnetapp -f "{{.Os}}/{{.Architecture}}"
linux/amd64
% docker run --rm --platform linux/amd64 --entrypoint bash aspnetapp -c "uname -a" 
Linux 70dd5f0635cd 6.6.16-linuxkit #1 SMP Fri Feb 16 11:54:02 UTC 2024 x86_64 GNU/Linux
```

That looks great. We have an x64 image.

You can try this pattern for .NET 8 apps, but also .NET 6 and 7 apps, too. The .NET 8 SDK can build apps for those earlier versions with this same approach.

## Build multi-platform images with `docker buildx`

[Buildx](https://www.docker.com/blog/how-to-rapidly-build-multi-architecture-images-with-buildx/) is a nice addition to Docker tools. I think of it as "full BuildKit". For our purposes, it enables specifying multiple platforms to build at once and to package them all up as a multi-platform tag. It will even push them to your registry, all with a single command.

We first need to setup buildx.

```bash
% docker buildx create
whimsical_sanderson
```

We can now build a multi-platform image for our app.

```bash
% docker buildx build --pull -t aspnetapp --platform linux/arm64,linux/arm,linux/amd64 .
```

You may see this error:

```bash
ERROR: Multi-platform build is not supported for the docker driver.
Switch to a different driver, or turn on the containerd image store, and try again.
```

Enable the [containerd image store](https://docs.docker.com/desktop/containerd/) to resolve that.

![Enable containerd image store in Docker Desktop](./containerd-image-store.png)

Here, we're building for three architectures. In some environments, you can also specify just the architectures as a short-hand, avoiding repeating "linux".

With that command, you may the following warning.

```bash
WARNING: No output specified with docker-container driver. Build result will only remain in the build cache. To push result image into registry use --push or to load image into docker use --load
```

If you want to push your image to a registry, you need to add the `--push` argument and use a fully-specified registry name for the `-t` argument.

Let's try `--push` (with my private registry; you'll need to switch to your own).

```bash
$ docker buildx build --pull --push -t maplesugar.azurecr.io/aspnetapp --platform linux/arm64,linux/arm,linux/amd64 .
```

That command pushed 3 images and 1 tag to the registry.

I can now try pulling the image on my Apple laptop. It would work the same on my Raspberry Pi.

```bash
$ docker pull maplesugar.azurecr.io/aspnetapp
$ docker run --rm --name aspnetapp -d -p 8000:8080 maplesugar.azurecr.io/aspnetapp
08968dcce418db4d6f746bfa3a5f2afdcf66570bc8a726c4f5a4859e8666e354
$ curl http://localhost:8000/Environment
{"runtimeVersion":".NET 8.0.4","osVersion":"Debian GNU/Linux 12 (bookworm)","osArchitecture":"Arm64","user":"app","processorCount":8,"totalAvailableMemoryBytes":4113563648,"memoryLimit":0,"memoryUsage":32227328,"hostName":"7a2b079005a5"}
% docker exec aspnetapp uname -a
Linux 7a2b079005a5 6.6.22-linuxkit #1 SMP Fri Mar 29 12:21:27 UTC 2024 aarch64 GNU/Linux
% docker kill aspnetapp
aspnetapp
```

I'll now try the same image on an x64 machine.

```bash
$ docker run --rm --name aspnetapp -d -p 8000:8080 maplesugar.azurecr.io/aspnetapp
6dac425acc325da1c085608d503d6c884610cfa5b2a7dd93575f20355daec1a2
$ curl http://localhost:8000/Environment
{"runtimeVersion":".NET 8.0.4","osVersion":"Debian GNU/Linux 12 (bookworm)","osArchitecture":"X64","user":"app","processorCount":8,"totalAvailableMemoryBytes":67382321152,"memoryLimit":0,"memoryUsage":31629312,"hostName":"78fed92fd353"}
$ docker exec aspnetapp uname -a
Linux 78fed92fd353 6.8.0-22-generic #22-Ubuntu SMP PREEMPT_DYNAMIC Thu Apr  4 22:30:32 UTC 2024 x86_64 GNU/Linux
$ docker kill aspnetapp
aspnetapp
```

The primary difference is the value of the `osArchitecture` property in the JSON returned from the endpoint. We're about to demonstrate that the `maplesugar.azurecr.io/aspnetapp` tag we pushed is indeed a multi-platform image.

## Summary

This new Dockerfile pattern for .NET apps makes it simpler, easier, and faster to build containers for whichever architecture you want on whichever machine you want. We've had [past difficulties with x64 emulation with QEMU](https://github.com/dotnet/dotnet-docker/discussions/3848). This approach side-steps QEMU completely, which avoid the problems we've had and improves performance at the same time.

We wonder how teams will use the multi-platform container capability. Will you target multiple architectures at the developer desktop or also in CI? In particular, we wonder if teams will push both Arm64 and x64 container images to their registry even if only one of those architectures is needed by a cloud container service. Will pushing multiple architectures help developers investigate production issues when their local machine doesn't match the architecture of the cloud service? Please tell us if you have plans or existing practices on that topic.

We hope that this new pattern works for you, that it makes it easier to target multiple platforms and that you appreciate having `docker build --platform` and `docker buildx build --platform` as more straightforward options.
