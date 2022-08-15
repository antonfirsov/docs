---
post_title: .NET 6 is now in Ubuntu 22.04
author1: rlander@microsoft.com
post_slug: dotnet-6-is-now-in-ubuntu-2204
username: rlander@microsoft.com
microsoft_alias: rlander
featured_image: dotnet-canonical.png
categories: .NET, ASP.NET Core, Containers, Support
summary: Summary of your post, shown on the home page next to the featured image
desired_publication_date: 2022-08-16
---

[.NET 6](https://devblogs.microsoft.com/dotnet/announcing-net-6/) is now [included in Ubuntu 22.04 (Jammy)](https://ubuntu.com/blog/install-dotnet-on-ubuntu) and can be installed with just `apt install dotnet6`. This is a major improvement and simplification for Ubuntu users. It's also a new reason to upgrade to [Ubuntu 22.04](https://ubuntu.com/blog/ubuntu-22-04-lts-released).

Here's the commands to install the [.NET 6 SDK on Ubuntu 22.04](https://packages.ubuntu.com/jammy-updates/dotnet6):

```bash
sudo apt update
sudo apt install dotnet6
```

We're also announcing that [.NET 6 is available with Chiseled Ubuntu Containers](https://github.com/ubuntu-rocks/dotnet). Our friends at [Canonical](https://canonical.com/) have developed a new [chisel](https://github.com/canonical/chisel) approach for making ultra-small container images. We're very excited about it. The Chiseled Ubuntu image is `100MB` smaller than the Ubuntu images you've been using until now!

Here's the command to pull the new ASP.NET Chiseled image:

```bash
docker pull mcr.microsoft.com/dotnet/nightly/aspnet:6.0-jammy-chiseled
```

We also have updated our [dotnetapp](https://github.com/dotnet/dotnet-docker/blob/main/samples/dotnetapp/Dockerfile.chiseled) and [aspnetapp](https://github.com/dotnet/dotnet-docker/blob/main/samples/aspnetapp/Dockerfile.chiseled) so that you can try out .NET with Chiseled Ubuntu Containers.

These new container images significantly improve security posture:

- Ultra-small images (reduced size and attack surface)
- No package manager (avoids a whole class of attacks)
- No shell (avoids a whole class of attacks)
- Non-root (avoids a whole class of attacks)

To top that off, Canonical and Microsoft are committed to working together to ensure that new .NET releases are available with new Ubuntu releases and that they work well together. This includes security updates and secure delivery of container images.

We're really excited that .NET 6 is available in Ubuntu 22.04 and that Canonical chose to work with us as their launch partner for Chiseled Ubuntu images. This is what Canonical had to say about the project.

> “Ubuntu now has an end-to-end story from development to production with ultra-small supported container images, starting with the .NET platform”, said Valentin Viennot, Product Manager at Canonical. “We think it’s a huge improvement for both our communities; collaborating with the .NET team at Microsoft has enabled us to go above and beyond”.

## Canonical and Microsoft

Several months ago, folks at Canonical and Microsoft started working together with the goal of making Ubuntu an even better environment for .NET devs.

We had two main goals in mind:

- Simplify using .NET on Ubuntu.
- Shorten the supply chain between Canonical and Microsoft.

We've known for years that many .NET devs use Ubuntu. After we got talking, it became obvious that there was a fair bit we could do to make that experience better. Let me tell you what we've delivered.

## .NET in APT

You can now install [.NET 6 with APT](https://ubuntu.com/blog/install-dotnet-on-ubuntu), built by Canonical via [source-build](https://github.com/dotnet/source-build). These packages are available with Ubuntu 22.04 (Jammy) and later. It's a great reason to upgrade to [Jammy](https://ubuntu.com/blog/ubuntu-22-04-lts-released)!

> Note: Please checkout this [advisory on using `packages.microsoft.com` on Ubuntu 22.04](https://github.com/dotnet/core/issues/7699) now that .NET 6 is included in Ubuntu.

There are [multiple packages](https://packages.ubuntu.com/search?suite=default&section=all&arch=any&keywords=dotnet&searchon=names):

- [`dotnet6`](https://packages.ubuntu.com/jammy-updates/dotnet6) -- The .NET 6 SDK (short name).
- [`dotnet-sdk-6.0`](https://packages.ubuntu.com/jammy-updates/dotnet-sdk-6.0) -- Same as above (long name).
- [`aspnet-runtime-6.0`](https://packages.ubuntu.com/jammy-updates/aspnetcore-runtime-6.0) -- ASP.NET Core
- [`dotnet-runtime-6.0`](https://packages.ubuntu.com/jammy-updates/dotnet-runtime-6.0) -- .NET Runtime

I'll show you how to install these images using Docker (same model applies elsewhere):

```bash
rich@kamloops:~$ docker run --rm -it ubuntu:jammy
root@7d4dfca0ef55:/# apt update && apt install -y dotnet6
root@7d4dfca0ef55:/# dotnet --version
6.0.108
```

Canonical and Microsoft will be working together to ensure that these packages are updated on the monthly .NET team release schedule. This includes Microsoft sharing [CVE information](https://github.com/dotnet/core/blob/main/release-notes/6.0/cve.md) (descriptions and code) with Canonical ahead of public releases. Similarly, Canonical will share security information in the other direction.

Note: We're currently missing Arm64 builds. Those will be coming soon. Both companies are strong proponents of Arm64.

Note: [.NET SDK workloads](https://github.com/dotnet/designs/blob/main/accepted/2020/workloads/workloads.md) are not available in packages (for any Linux distro). Also, the .NET MAUI workloads isn't supported on Linux.

## .NET in Chiseled Ubuntu Containers

You can now use [.NET in Chiseled Ubuntu Containers](https://github.com/ubuntu-rocks/dotnet). Chiseling delivers the smallest container footprint while still being the Ubuntu you know and trust. It is similar to conventional [distroless](https://hackernoon.com/distroless-containers-hype-or-true-value-2rfl3wat), with  a tool that is customized for slicing `.deb` packages.

We're offering three layers of Chiseled Ubuntu container images, for Arm64 and x64:

- `mcr.microsoft.com/dotnet/nightly/runtime-deps:6.0-jammy-chiseled`
- `mcr.microsoft.com/dotnet/nightly/runtime:6.0-jammy-chiseled`
- `mcr.microsoft.com/dotnet/nightly/aspnet:6.0-jammy-chiseled`

Note: The images will be offered in our `nightly` repos while the chiseled offering is in preview. We'll make another announcement when they are supported in production. It will be sometime this year, but we haven't picked a timeframe, since we've been focused on basic enablement.

Let's take a look at the size win. All of the following sizes are uncompressed (on-disk, not registry/wire size).

First, the `runtime-deps` layer.

- Ubuntu 22.04 (Jammy): `112MB`
- Chiseled Ubuntu 22.04 (Jammy): `12.9MB`

And on the other end of the spectrum, the `aspnet` layer.

- Ubuntu 22.04 (Jammy): `213MB`
- Chiseled Ubuntu 22.04 (Jammy): `104MB`

That's a truly amazing difference! The folks at Canonical have figured out how to drop 100MB of binaries and other content from these images. When we first started talking, we had no idea we'd be talking about this large of a difference!

Close readers will notice that chiseled `aspnet` is smaller than the existing `runtime-deps` layer. That's shockingly good.

It's reasonable to ask what [Alpine](https://alpinelinux.org/) looks like. It's a newer distro designed to be super small and componentized from the start. Alpine is `9.84MB` for `runtime-deps:6.0-alpine` and `100MB` for `aspnet:6.0-alpine`. Those are impressive numbers, again uncompressed. That's the key reason why Alpine is so popular (and why we've published .NET images for it for years).

Alpine is great (and we're also friends with those folks), but it isn't for everyone and every app since it uses [musl](http://musl.libc.org/), which is a different (and incompatible) `libc` variant. That's only important if your app includes native libraries. If it doesn't (and most .NET apps don't), you don't need to worry about this detail. The .NET product itself is happy running with either `musl` or [`glibc`](https://www.gnu.org/software/libc/) and every PR on [dotnet/runtime](https://github.com/dotnet/runtime) tests for both.

Putting this in perspective, this is really great news if you use Ubuntu for development and always wished for a small Ubuntu to deliver into production. You now have a straightforward path from dev box to cloud without any distro-compatibility surprises. It's amazing (and quite surprising) to see Ubuntu in the same ballpark as Alpine. Kudos to the Canonical folks on a great engineering accomplishment.

It's also worth mentioning that [Chainguard](https://www.chainguard.dev/) is looking at [minimal container images towards a secure future](https://blog.chainguard.dev/minimal-container-images-towards-a-more-secure-future/). That project is run out of the [distroless](https://github.com/distroless) GitHub org. We're watching that project and glad to see more interest in small and more secure container images. We believe that minimal + non-root container images are the future.

Like our [Alpine images](https://github.com/dotnet/dotnet-docker/blob/1297d21bbf695bcb87580bea2ccefdced894eeeb/src/runtime-deps/3.1/alpine3.16/amd64/Dockerfile#L19-L20), we've chosen not to include [ICU](https://icu.unicode.org/). It would likely double the size of the image. That means that we've enabled [globalization invariant mode](https://github.com/dotnet/runtime/blob/main/docs/design/features/globalization-invariant-mode.md). For some apps, that's fine, and the size win is great. For others, it is a deal breaker. We may need to adjust this part of the plan depending on the feedback. We've [documented the pattern](https://github.com/ubuntu-rocks/dotnet/issues/21) to add ICU into your images.

Let me demo these images a bit to drive the point home on how (intentionally) limited these images are.

```bash
% docker run --rm mcr.microsoft.com/dotnet/nightly/runtime-deps:6.0-jammy-chiseled-amd64
docker: Error response from daemon: No command specified.
See 'docker run --help'.
```

Let's try again.

```bash
% docker run --rm mcr.microsoft.com/dotnet/nightly/runtime-deps:6.0-jammy-chiseled-amd64 bash
docker: Error response from daemon: failed to create shim task: OCI runtime create failed: runc create failed: unable to start container process: exec: "bash": executable file not found in $PATH: unknown.
```

Huh? What's up? They don't work! That's the point. These are appliance-like container images. They are stripped down to the minimum. They are only intended to do what you design them to do. That's the aspect that makes them more secure. If this experience is uncomfortable, you can always use the regular Ubuntu images. We'll continue to offer them. They are not going away.

For the `runtime` and `aspnet` images, we decided to use `dotnet --info` as the `ENTRYPOINT` to make the experience a little more friendly and useful.

```bash
% docker run --rm mcr.microsoft.com/dotnet/nightly/runtime:6.0-jammy-chiseled-amd64      
Host (useful for support):
  Version: 6.0.5
  Commit:  70ae3df4a6

.NET SDKs installed:
  No SDKs were found.

.NET runtimes installed:
  Microsoft.NETCore.App 6.0.5 [/usr/share/dotnet/shared/Microsoft.NETCore.App]

To install additional .NET runtimes or SDKs:
  https://aka.ms/dotnet-download
```

For the moment, the images are in preview. You will see this space changing quickly. We're starting with x64 images, but will soon have Arm64 images and multi-arch manifests.

We're not offering a chiseled SDK image. It wasn't obvious that there was a strong need. In fact, a chiseled SDK image could be hard to use for some scenarios. You can continue to use the existing Jammy SDK image: `mcr.microsoft.com/dotnet/sdk:6.0-jammy`. If there is a need for a chiseled SDK image, we'll be happy to reconsider.

## Using chiseled container images

For most apps, there won't be any different in using these new container images, in terms of what your `Dockerfile` looks like.

We made updated our samples to use these new containers images:

- [dotnetapp](https://github.com/dotnet/dotnet-docker/blob/main/samples/dotnetapp/Dockerfile.chiseled)
- [aspnetapp](https://github.com/dotnet/dotnet-docker/blob/main/samples/aspnetapp/Dockerfile.chiseled)

I'll show you how easy this is with [dotnetapp](https://github.com/dotnet/dotnet-docker/blob/main/samples/dotnetapp/Dockerfile.chiseled).

The Dockerfile is barely different.

```dockerfile
@@ -0,0 +1,17 @@
# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:7.0-jammy AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.csproj .
RUN dotnet restore --use-current-runtime

# copy and publish app and libraries
COPY . .
RUN dotnet publish -c Release -o /app --use-current-runtime --self-contained false --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/nightly/runtime:7.0-jammy-chiseled
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "dotnetapp.dll"]
```

Only the final `FROM` statement is notably different from our standard [Ubuntu Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/samples/dotnetapp/Dockerfile.ubuntu-x64).

I'll now build the sample:

```bash
rich@MacBook-Air-2 dotnetapp % pwd
/Users/rich/git/dotnet-docker/samples/dotnetapp
rich@MacBook-Air-2 dotnetapp % docker build -t dotnetapp-chiseled -f Dockerfile.chiseled .
rich@MacBook-Air-2 dotnetapp % docker images | grep dotnetapp-chiseled
dotnetapp-chiseled                 latest      bf7e125bd182   20 seconds ago   90.5MB
```

Note: I didn't use any .NET trimming features. Certainly, this image could be made smaller.

Let's see what it in action:

```bash
rich@MacBook-Air-2 dotnetapp % docker run --rm dotnetapp-chiseled
         42
         42              ,d                             ,d
         42              42                             42
 ,adPPYb,42  ,adPPYba, MM42MMM 8b,dPPYba,   ,adPPYba, MM42MMM
a8"    `Y42 a8"     "8a  42    42P'   `"8a a8P_____42   42
8b       42 8b       d8  42    42       42 8PP"""""""   42
"8a,   ,d42 "8a,   ,a8"  42,   42       42 "8b,   ,aa   42,
 `"8bbdP"Y8  `"YbbdP"'   "Y428 42       42  `"Ybbd8"'   "Y428

.NET 7.0.0-preview.7.22375.6
Linux 5.10.104-linuxkit #1 SMP PREEMPT Thu Mar 17 17:05:54 UTC 2022

OSArchitecture: Arm64
ProcessorCount: 4
TotalAvailableMemoryBytes: 3.83 GiB
```

And then, let's try to break in:

```bash
rich@MacBook-Air-2 dotnetapp % docker run --rm --entrypoint bash dotnetapp-chiseled
docker: Error response from daemon: failed to create shim task: OCI runtime create failed: runc create failed: unable to start container process: exec: "bash": executable file not found in $PATH: unknown.
rich@MacBook-Air-2 dotnetapp % docker run --rm --entrypoint apt  dotnetapp-chiseled install -y bash
docker: Error response from daemon: failed to create shim task: OCI runtime create failed: runc create failed: unable to start container process: exec: "apt": executable file not found in $PATH: unknown.
```

My "red team" skills are failing me.

I'll now describe chiseled images in a bit more detail now that you've seen them in action.

## Chiseled Ubuntu Containers

Chiseled Ubuntu Containers are the Canonical take on the distroless concept, originally [popularized by Google](https://github.com/GoogleContainerTools/distroless). With the original implementation, a distro is stripped bare and only necessary packages are installed. Chiseling takes this one step forward by [installing only the directories and files in each package that are necessary](https://github.com/dotnet/dotnet-docker/blob/fd39ea3eece8c8653348a717446ac41e93633784/src/runtime-deps/6.0/jammy-chiseled/arm64v8/Dockerfile#L29-L36).

The other challenge with the original implementation was that it wasn't necessarily supported by any party. Chiseled Ubuntu Containers are a first-class Canonical deliverable. That means you can use ultra-small container images and be supported as a Canonical customer.

Hats off to Google for getting us all started down this path.

As stated earlier, there is a lot of value to this approach:

- Ultra-small images (reduced size and attack surface)
- No package manager (avoids a whole class of attacks)
- No shell (avoids a whole class of attacks)

Chiseled Ubuntu Containers are currently in preview. We'll make a separate announcement when they are stable and supported in production.

## Non-root images

We've [configured all of the new .NET Chiseled Ubuntu Containers](https://github.com/dotnet/dotnet-docker/blob/fd39ea3eece8c8653348a717446ac41e93633784/src/runtime-deps/6.0/jammy-chiseled/arm64v8/Dockerfile#L13-L26) with a [non-root](https://stackoverflow.com/questions/59840450/rootless-docker-image) user. The images do not include the `root` user or include root-elevating commands like `sudo` or `su`. That means that it is not possible to exercise capabilities and operations that require `root`.

Non-root images are an additional security mitigation beyond removing a shell (like `bash`). Non-root images are logically separate and complementary to running a [daemon as rootless](https://docs.docker.com/engine/security/rootless/). Every [reduction in privilege helps](https://seclists.org/oss-sec/2019/q1/119).

If you need access to privileged resources, you can add the `root` user within your `Dockerfile`. You are not prevented from that, but then that's a specific security decision you'd be making.

Chiseled images are appliance-like and are not general-purpose. We felt that they offered us an opportunity to [finally deliver non-root images](https://github.com/dotnet/dotnet-docker/issues/2249). That's informing our policy going forward. Appliance-like images will be delivered as non-root and general-purpose ones will be delivered as per the policy of the base image (which might be configured with the `root` user). However, this project with Canonical has inspired us to look at a middle-ground option, of [offering non-root-capable images](https://github.com/dotnet/designs/pull/271).

## Secure supply chain

Canonical already has secure processes in place for directly delivering Ubuntu Virtual Machine images to Azure for customers to use. It occurred to us that Canonical could do the same thing with the Ubuntu container base images that we use to build Ubuntu-based .NET images (regular and Chiseled). That's what we're now using, instead of pulling from Docker Hub. We now have what's effectively a zero-distance supply chain for all Canonical assets with known custody/provenance throughout.

We're doing something similar with sharing [CVE](https://www.cve.org/) fixes. We have a shared private [virtual mono repo](https://github.com/dotnet/source-build/issues/2956) for sharing monthly patches. It's also shared with Red Hat. It means we can work together on getting the correct fixes in place at the right time in a coordinated way.

.NET container images are not yet signed, but that's coming relatively soon. We're regularly working to improve our security-focused capabilities.

## Support

Canonical and Microsoft have been working together to give you a better experience. This includes support. You can report issues in the familiar .NET repos like  [dotnet/core](https://github.com/dotnet/core) and [dotnet/runtime](https://github.com/dotnet/runtime). If you want commercial support, you should [start with Canonical support](https://ubuntu.com/security/docker-images). Canonical is the best position to support Ubuntu packages. Canonical may contact Microsoft to assist with resolving issues, as needed.

Security researchers that find vulnerabilities in Canonical-provided .NET packages are still eligible for the [Microsoft .NET Bounty Program](https://www.microsoft.com/msrc/bounty-dot-net-core).

Microsoft continues to maintain .NET packages in its [packages.microsoft.com](https://docs.microsoft.com/dotnet/core/install/linux) feed for Ubuntu and we intend to continue that going forward. For most users, we recommend using the `dotnet6` packages that come with Ubuntu Jammy+. That's what I'll be doing. It's also the same guidance we have for Red Hat users.

> Note: Please checkout this [advisory on using `packages.microsoft.com` on Ubuntu 22.04](https://github.com/dotnet/core/issues/7699) now that .NET 6 is included in Ubuntu.

There are two main reasons to continue to use the Microsoft packages:

- You specifically want .NET builds from Microsoft, not any other vendor.
- The Microsoft packages target later .NET SDK feature bands (like `6.0.4xx`) while source-build tracks `6.0.1xx`. That's more relevant for Windows users, but might be important for some Linux users.

The new packages are available for .NET 6+ and Ubuntu 22.04+. Previous .NET and Ubuntu versions are not supported (with the new packages). You must use the existing `packages.microsoft.com` feed to use .NET on earlier Ubuntu versions. Separately, earlier .NET versions are not supported on Ubuntu 22.04 because they do not support OpenSSL v3.

## What's Next?

We have identified a number of [opportunities to make it easier for Canonical to consume .NET source](https://github.com/dotnet/source-build/issues/2911). We're going to focus on those in the immediate term. These improvements will also benefit other users who build and distribute .NET from source.

We recently setup a distro-maintainer group for .NET. Canonical is a member of that group. We have already started discussing [potential source-build improvements](https://github.com/dotnet/source-build/issues/2911) within that forum. Other distros (that build .NET from source) are welcome to join. Contact dotnet@microsoft.com for more information.

Canonical is starting out with support for x64 and will quickly add .NET packages for Arm64. It's an exciting time in the industry with multiple mainline chip architectures to support. Ubuntu and .NET both have a long history of supporting multiple architectures.

## Closing

.NET has been open source for just over 5 years now. A partnership with Canonical was felt out of grasp during the early days of our project on GitHub. We've learned a lot about how to structure an OSS project so that it is a candidate for inclusion in a Linux distro. This is thanks to our other partners who have taught us a lot, particularly Fedora and Red Hat. Looking back, it is easy to see that open source, trust, and industry relationships are even more important now than they were when we started. We're excited and honored to be working with Canonical.
