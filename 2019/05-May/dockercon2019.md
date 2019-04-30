# Using .NET and Docker Together – DockerCon 2019 Update

[DockerCon 2019](https://www.docker.com/dockercon/) is being held this week, in San Francisco. We posted a [DockerCon 2018 update](https://devblogs.microsoft.com/dotnet/using-net-and-docker-together-dockercon-2018-update/) last year, and it is time to share how we've improved the experience of using .NET and Docker together over the last year.

We have a group of .NET Core team members attending the conference again this year. Please reach out @ dotnet@microsoft.com if you want to meetup.

Most of our effort to improve the .NET Core Docker experience in the last year has been focused on .NET Core 3.0. This is the first release in which we've made substantive runtime changes to make CoreCLR much more efficient, honor Docker resource limits better by default, and offer more configuration for you to tweak.

We are invested in making .NET Core a true container runtime. In past releases, we thought of .NET Core as container friendly. We are now hardening the runtime to make it container-aware and function efficiently in low-memory environments.

## Allocate less memory and fewer GC heaps by default

The most foundational change we made is to reduce the memory that CoreCLR uses by default. If you think of the Docker limit as the denominator, then baseline memory usage is the numerator that you start with. It is critical to reduce that value to enable smaller memory limits. That's exactly what we've done with .NET Core 3.0.

We reduced the minimal generation 0 GC allocation budget to better align with modern processor cache sizes and cache hierarchy. We found that the initial allocation size was unnecessarily large and could be significantly reduced without any perceivable loss of performance. In [workloads we measured](https://twitter.com/DamianEdwards/status/1093981272362254336), we found tens of percentage points of improvements.

There's a new policy for determining how many GC heaps to create. This is most important on machines were a low memory limit is set, but no CPU limit is set on a machine with many CPU cores. The GC now reserves a memory segment with a minimum size of 16 MB per heap. This limits the number of heaps the GC will create. For example, if you set a 160 MB memory limit on a 48-core machine, you don't want 48 GC heaps created. That means that if you set a 160 MB limit, then only 10 GC heaps will be created. If CPU limits are not set, applications can still take advantage of all the cores on the machine.

We know that some developers use the workstation GC as a means of limiting GC allocations, with a possible reduction in throughput. With this new policy in place, we hope that you do not need to enable workstation GC with docker workloads.

Both changes -- reducing generating 0 initial allocation size and defining a new GC heap minimum -- results in lower memory usage by default and makes the default .NET Core configuration better in more cases.

## Support for Docker Memory Limits

There are really two scenarios for memory limits:

* setting an arbitrary memory limit (like say 750 MB)
* setting a low memory limit (like say 75 MB)

In either case, you want your application to run reliably over time. Obviously, if you limit an application to run in less than 75 MB of memory, it needs to be capable of doing that. A container-hardened runtime is not a magic runtime! You need to model memory requirements in terms of both steady-state and per-request memory usage. An application that requires a 70 MB cache has to accommodate that.

[Docker resource limits](https://docs.docker.com/config/containers/resource_constraints/) are built on top of [cgroups](https://en.wikipedia.org/wiki/Cgroups), which is a Linux kernel capability. From a runtime perspective, we need to target cgroup primitives.

The following summary describes the new [.NET Core 3.0 behavior when cgroup limits are set](https://github.com/dotnet/designs/blob/master/accepted/support-for-memory-limits.md):

* Default GC heap size: maximum of `20 MB` or `75%` of the cgroup memory limit on the container
* Minimum reserved segment size per GC heap is `16 MB`, which will reduce the number of heaps created on machines with a large number of cores and small memory limits

Though CGroups are a Linux concept, Job objects on Windows are a similar concept, and the runtime honors memory limits on Windows in the same way.

Over the last few releases, we have put a lot of effort into improving how .NET Core performs on the [TechEmpower Benchmarks](https://www.techempower.com/benchmarks/). With .NET Core 3.0, we found ways to significantly improve the performance and reduce the memory used by a large margin. We now run the TechEmpower plaintext benchmark in a container limited to about 150 MB, while servicing millions of requests per second. This enables us to validate memory limited scenarios every day. If the container OOMs, then that means we need to determine why the scenario is using more memory than we expect.

Note: [Process APIs report inconsistent results in containers](https://github.com/dotnet/corefx/issues/36086). We do not recommend relying on these APIs for containerized apps. We are working on resolving these issues. Please let us know if you rely on these APIs.

## Support for Docker CPU Limits

CPU can also be limited; however, it is more nuanced on how it affects your application.

Docker limits enable setting CPU limits as a decimal value. The runtime doesn't have this concept, dealing only in whole integers for CPU cores. Previously, the runtime used simple rounding to calculate the correct value. That approach leads the runtime to take advantage of less CPU than requested, leading to CPU underutilization.

In the case where `--cpus` is set to a value (for example, `1.499999999`) that is close but not close enough to being rounded up to the next integer value, the runtime would previously round that value down (in this case, to `1`). In practice, rounding up is better.

By changing the runtime policy to aggressively round up CPU values, the runtime augments the pressure on the OS thread scheduler, but even in the worst case scenario (`--cpus=1.000000001` — previously rounded down to `1`, now rounded to `2`), we have not observed any overutilization of the CPU leading to performance degradation.

Unlike with the memory example, it is OK if the runtime thinks it has access to more CPU than it does. It just results on a higher reliance on the OS scheduler to correctly schedule work.

The next step is ensuring that the thread pool honors CPU limits. Part of the algorithm of the thread pool is computing CPU busy time, which is, in part, a function of available CPUs. By taking CPU limits into account when computing CPU busy time, we avoid various heuristics of the thread pool competing with each other: one trying to allocate more threads to increase the CPU busy time, and the other one trying to allocate less threads because adding more threads doesn’t improve the throughput.

Server GC is enabled by default for ASP.NET Core apps (it isn't for console apps), because it enables high throughput and reduces contention across cores. When a process is limited to a single processor, the runtime automatically switches to workstation GC. Even if you explicitly specify the use of server GC, the workstation GC will always be used in single core environments.

## Adding PowerShell to .NET Core SDK container Images

[PowerShell Core](https://github.com/powershell/powershell) has been [added to the .NET Core SDK Docker container images](https://github.com/dotnet/dotnet-docker/issues/1069), per [requests from the community](https://github.com/dotnet/dotnet-docker/issues/360). PowerShell Core is a cross-platform (Windows, Linux, and macOS) automation and configuration tool/framework that works well with your existing tools and is optimized for dealing with structured data (e.g. JSON, CSV, XML, etc.), REST APIs, and object models. It includes a command-line shell, an associated scripting language and a framework for processing cmdlets.

PowerShell Core is released as a self-contained application by default. We converted it to a framework-dependent application for this case. That means that the size cost is relatively low, and there is only one copy of the .NET Core runtime in the image to service.

You can try out PowerShell Core, as part of the .NET Core SDK container image, by running the following Docker command:

```console
docker run --rm mcr.microsoft.com/dotnet/core/sdk:3.0 pwsh -c Write-Host "Hello Powershell"
```

There are two main scenarios that having PowerShell inside the .NET Core SDK container image enables, which were not otherwise possible:

* Write .NET Core application Dockerfiles with PowerShell syntax, for any OS.
* Write .NET Core application/library build logic that can be easily containerized.

Example syntax for launching PowerShell for a volume-mounted containerized build:

* `docker run -it -v c:\myrepo:/myrepo -w /myrepo mcr.microsoft.com/dotnet/core/sdk:3.0 pwsh build.ps1`
* `docker run -it -v c:\myrepo:/myrepo -w /myrepo mcr.microsoft.com/dotnet/core/sdk:3.0 ./build.ps1`

Note: For the second example to work, on Linux, the .ps1 file needs to have the following pattern, and needs to be formatted with Unix (LF) not Windows (CRLF) line endings:

```powershell
#!/usr/bin/env pwsh
Write-Host "test"
```

If you are new to PowerShell, we recommend reviewing the [PowerShell getting started documentation](https://github.com/PowerShell/PowerShell/tree/master/docs/learning-powershell).

Note: PowerShell Core is now available as part of [.NET Core 3.0 SDK container images](https://hub.docker.com/_/microsoft-dotnet-core-sdk/). It is not part of the [.NET Core 3.0 SDK](https://dotnet.microsoft.com/download/dotnet-core/3.0).

## .NET Core Images now available via Microsoft Container Registry

Microsoft teams are now publishing container images to the Microsoft Container Registry (MCR). There are two primary reasons for this change:

* Syndicate Microsoft-provided container images to multiple registries, like Docker Hub and Red Hat.
* Use Microsoft Azure as a global CDN for delivering Microsoft-provided container images.

On the .NET team, we are now publishing all [.NET Core images to MCR](https://hub.docker.com/_/microsoft-dotnet-core). As you can see from these links (if you click on them), we continue to have “home pages” on Docker Hub. We intend for that to continue indefinitely. MCR does not offer such pages, but relies on public registries, like Docker Hub, to provide users with image-related information.

The links to our old repos, such as [microsoft/dotnet](https://hub.docker.com/r/microsoft/dotnet), now forward to the new locations. The images that existed at those locations still exists and will not be deleted.

We will continue servicing the floating tags in the old repos for the supported life of the various .NET Core versions. For example, `2.1-sdk`, `2.2-runtime`, and latest are examples of floating tags that will be serviced. A three-part version tag, like `2.1.2-sdk`, will not be serviced, which was already the case.

.NET Core 3.0 will only be published to MCR.

For example, the correct tag string to pull the 3.0 SDK image now looks like the following:

```console
mcr.microsoft.com/dotnet/core/sdk:3.0
```

The correct tag string to pull the 2.1 runtime image now looks like the following:

```console
mcr.microsoft.com/dotnet/core/runtime:2.1
```

The new MCR strings are used with both `docker pull` and in Dockerfile `FROM` statements.

## Platform matrix and support

With .NET Core, we try to support a broad set of distros and versions. For example, with Ubuntu, we support versions 16.04 and later. With containers, it's too expensive and confusing for us to support the full matrix of options. In practice, we produce images for each distro's tip version or tip LTS version.

We have found that each distribution has a unique approach to releasing, schedule and end-of life (EOL). That prevents us from defining a one-size-fits-all policy that we could document. Instead, we found it was easier to document our policy for each distro.

* Alpine -- support tip and retain support for one quarter (3 months) after a new version is released. Right now, 3.9 is tip and we'll stop producing 3.8 images in a month or two.
* Debian -- support one Debian version per .NET Core version, whichever Debian version is the latest when a given .NET Core version ships. This is also the default Linux image used for a given multi-arch tag. For .NET Core 3.0, we intend to [publish Debian 10 based images](https://github.com/dotnet/dotnet-docker/issues/1079). We produce Debian 9 based images for .NET Core 2.1 and 2.2, and Debian 8 images for earlier .NET Core versions.
* Ubuntu -- support one Ubuntu version per .NET Core version, whichever Ubuntu version is the latest LTS version when a given .NET Core version ships. Today, we support Ubuntu 18.04 for all supported .NET Core versions. When 20.04 is released, we will start publishing images based on it, for the latest .NET Core version at that time. In addition, as we get closer to a new Ubuntu LTS versions, we will [start supporting non-LTS Ubuntu versions](https://github.com/dotnet/dotnet-docker/issues/1081) a means of validating the new LTS versions.

For Windows, we support all supported Nano Server versions with each .NET Core version. In short, we support the cross-product of Nano Server and .NET Core versions.

## ARM Architecture

We are in the process of adding support for ARM64 on Linux with .NET Core 3.0, complementing the ARM32 and X64 support already in place. This will enable .NET Core to be used in even more environments.

We were excited to see that [ARM32 images were added for Alpine](https://hub.docker.com/r/arm32v7/alpine). We have been wanting to see that for a couple years. We are hoping to start [publishing .NET Core for Alpine on ARM32](https://github.com/dotnet/dotnet-docker/issues/1059) after .NET Core 3.0 is released, possibly as part of a .NET Core 3.1 release. Please tell us if this scenario is important to you.

## Closing

Containers are a major focus for .NET Core, as we hope is evident from all the changes we've made. As always, we are reliant on your feedback to direct future efforts.

We've done our best to target obvious and fundamental behavior in the runtime. We'll need to look at specific scenarios in order to further optimize the runtime. Please tell us about yours. We're happy to spend some time with you to learn more about how you are using .NET Core and Docker together.

Enjoy the conference (if you are attending)!
