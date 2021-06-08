---
post_title: Conversation about containers
username: rlander@microsoft.com
microsoft_alias: rlander@microsoft.com
categories: .NET Core, .NET
desired_publication_date: 05/20/2021
summary: Conversation with .NET engineers who make .NET work great with containers.
---

[containers](https://devblogs.microsoft.com/dotnet/category/containers/) have becomes the most popular way to deploy cloud-based apps. It's also one of the most common topics that .NET web developers ask us about. We've been spending more and more effort on improving .NET for containers with each release. It's now just assumed as the primary deployment model for new features and scenarios. It's also the way our [TechEmpower](https://www.techempower.com/benchmarks/) [benchmark tests](https://github.com/TechEmpower/FrameworkBenchmarks/tree/master/frameworks/CSharp/aspnetcore) are run.

We're using the [conversation format](https://devblogs.microsoft.com/dotnet/category/conversations/) again, this time with engineers who work on improving .NET for containers and ensuring we have good end-to-end workflows.

* [Brigit Murtaugh](https://github.com/bamurtaugh)
* [David Fowler](https://github.com/davidfowl)
* [Glenn Condron](https://github.com/glennc)
* [Manish Godse](https://github.com/mangod9)
* [Maoni Stephens](https://github.com/Maoni0)
* [Michael Simons](https://github.com/MichaelSimons)
* [Rich Lander](https://github.com/richlander)

## Docker containers are popular. Why do developers and devops folks use them? Should everyone?

**Glenn:** They provide a greater level of assurance that your build output will work in your target environment then we have ever had before. In the case of .NET, this can seem negligible as we've got a pretty good track record of abstracting the underlying platform. But I have docker images with apps built using early versions of .NET Core that run perfectly today without me ever touching them. All the requirements, including the old runtime they work with, are encapsulated in that one Docker artifact.

**Manish:** Not quite sure whether developers care as much where their applications run, in contrast with devops folks who have to manage things in realtime and want isolation, density and the ability to seamlessly move workloads between VMs. Containers also provide a fast elastic way of scaling up and down as required. 

**Michael:** Containers are great for both development and app deployment scenarios.  They are a convenient means to capture the environment necessary to run either in.  They provide isolation and are scalable.  The tooling ecosystem has really flourished and makes working with containers simple and easy.

**Rich:** DevOps folks talk to us about environment promotion. That idea sums of the value of containers really well. As you promote your app from dev, to test, to staging, and eventually to production (where there may even be multiple rings), you get to count on certain aspects of containers being immutable (the files) and others being trivially easy to change (the configuration). As you promote the app through the environments, your confidence builds due to the image immutability, while the production nature of the app increases as it gets access to real data and secrets.

This model is also nicely aligned with secure supply chain since the container image isn't changed -- and shouldn't be changed -- as it goes through the environment promotion process. As image signing becomes more commonplace, this model will become standard in a lot of organizations. It's very similar to the [GitOps approach](https://www.youtube.com/watch?v=0Pp6qC7GIu8&t=60s).

## What's the difference between containers on one hand and orchstrators like Kubernetes and docker swarm on the other? Where does `docker-compose` fit in?

**Manish:** The granularity for containers is at the OS or VM level, where as orchestrators manage containers across VMs, machines and even deployments. docker-compose feels like it is more of a orchestration thing since it needs to take into account the "environment" aspects as well. But interpretation is specific to the devops team building set of applications.

**Michael:** Containers are the components involved in your system such as an app and a database.  Orchestrators define the configuration for how the containers are run such as how multiple containers communicate to each other.

**Glenn:** I think Manish and Michael have it covered. Orchestrators run containers across a set of hardware and I like to think of compose as a dev machine focused orchestrator.

**Rich:** I run [pi-hole on my network with `docker-compose`](https://github.com/pi-hole/docker-pi-hole). 

I love the simplicity of the model:

* docker-compose commands match the docker CLI, like `docker-compose pull`.
* The UX is super simple, with `docker-compose up` and `docker-compose down`.
* docker-compose comes with docker, so you don't need to install another system.

In my opinion, pi-hole doesn't need a more sophisticated system.

## Tell us about the changes that have been made to improve the container experience for .NET developers?

**Maoni:** We added job support in .NET 4.6.2, meaning GC started to specifically check if a process was running inside a job (ie, how a container is implemented on windows) so it could react to the memory pressure inside a container; however if you used Server GC with lots of cores and a small memory limit, this meant GC may not have been able to react fast enough so in .NET 3.0 we tighented this and made Server GC not throw premature OOMs for this scenario.

**Manish:** There have been many improvements in the .net container experience over the past few years. Since we are working on .net 6 currently wanted to call out the following:

1. Crossgen2: A new tool to precompile IL which replaces current crossgen. We have enabled more optimizations as part of that to improve startup, including ability to specify instruction sets, a new composite mode which compiles and all into a single binary which have shown further improvements in startup time
2. We recently improved support for windows containers based on process isolation to honor cpu limits set on them.

**Glenn:** It's not distinct to .NET developers, but the VS family that almost all .NET developers use now have some great container tooling that you should check out. For .NET specific stuff I think the other folk have it covered. Maoni's work in particular is the sort of thing that just makes .NET work better or more predictably without you even noticing. Because parts of the runtime, in this case the GC, are aware of the primitives that docker use for its isolation and constraints.

**Michael:** The size of the .NET Docker images has been reduced since they were initially released.  Part of this has come from product packaging optimizations but other significant gains have come from maximizing the number of layers shared across the .NET images - e.g. runtime, aspnet, sdk.

We also have made changes to the distros and os versions we offer official images for in response to feedback from the community.  For example Alpine was added in response to customers requesting a more secure container distro.

We recently added a `dotnet/diagnostics` image variant (`dotnet/monitor`) to offer the same diagnostics tools in containers that are useful for diagnosing .NET Core issues in other scenarios.

**Rich:** We use containers pervasively within our own engineering infrastructure. That gives us a lot of confidence that .NET works well in containers, although that just covers the basics. The next step was making changes to ensure that the runtime honors the environment, which is cgroups on Linux, and Job objects on Windows. That's what Maoni and Manish are talking about. We made [changes in .NET 6](https://github.com/dotnet/runtime/issues/53149) to complete our offering. There were some CPU-related settings that we'd missed.

Our forward looking plan is to make the container images we publish more opinionated. The official builds of .NET are intended to work on a very wide variety of hardware and operating systems. That's not changing. We can significantly optimise the container images we publish for modern environments if we we're willing to reduce the scope where those images are intended to run (think hardware made in the last five years). We're hoping that .NET 6 has the first set of changes in that model. In a world where most of the server compute is in the cloud, and climate change is threatening our way of life, it makes sense to take advantage of modern hardware and software to the greatest degree possible. That's what we're going to do.

## What's OOMKILL and is it gone for .NET apps?

**Manish:** OOMKill is the OS flavor of OutOfMemoryException. There were cases where .net wasnt good at detecting low memory conditions leading to the container exceeding its memory limits. Progressively we have improved the handling to be better aware of memory constraints for various environments.

We keep finding new cases like this one: [Memory load includes the file cache on Docker Linux · Issue #49058 · dotnet/runtime (github.com)](https://github.com/dotnet/runtime/issues/49058), where the PAL wasnt accounting for file caches leading to excessive GC-ing. Not exactly related to OOMKill but in the similar realm. 

**Maoni:** OOMKill is a concept on linux where you can specify how you want to the OS to select processes to kill when memory is tight. On Windows, when you are successful at committing memory, you are guaranteed that you can use that memory. on Linux this is not the case. there are many ways you can configure this on Linux; I often see folks just disable it.

**Rich:** In the early days of .NET Core, we saw a lot of reports of OOMKill and people were naturally unhappy. The [memory limits model](https://github.com/dotnet/designs/blob/main/accepted/2019/support-for-memory-limits.md) we [included in .NET Core 3.0](https://devblogs.microsoft.com/dotnet/using-net-and-docker-together-dockercon-2019-update/) changed that. I no longer see folks asking about this, at least not for the basic scenario. Certainly, there are always reasons why an app could be OOMKilled, but that's going to be more nuanced.

Maoni wrote some great (now historical) posts on this topic:

* [Running with Server GC in a Small Container Scenario Part 0](https://devblogs.microsoft.com/dotnet/running-with-server-gc-in-a-small-container-scenario-part-0/)
* [Running with Server GC in a Small Container Scenario Part 1 – Hard Limit for the GC Heap](https://devblogs.microsoft.com/dotnet/running-with-server-gc-in-a-small-container-scenario-part-1-hard-limit-for-the-gc-heap/)

## One of the most recent changes is enabling custom values for `Environment.ProcessorCount`. In what scenario would you recommend that?

Context: https://github.com/dotnet/runtime/issues/48094

**Manish:** ProcessorCount is used to configure a few things within the runtime plumbing, like # GC heaps etc. By limiting or increasing the count one can influence how the runtime behaves from a scaling perspective.

**David:** Lots of algorithms in .NET tune themselves according to the available processors. This includes things like concurrent data structures to the number of IO threads used by sockets and things like kestrel's IO thread queues.

It's extremely widespread in the BCL, Timers, Sockets, concurrent data structures, array pool, the list goes on.

**Maoni:** I view this as a way for users to manually influence concurrency - when you set the CPU limit on a container, the runtime will take that limit and return the # of processors calculated based on the limit, so if it's 0.1 and you are on a 10 cpu system, when various components (like the GC) go and ask for the # of processors they'd get 1. you could change the # of processors the process thinks it has access to via this to influence those components

**Rich:** There are two main ways to configure CPUs with containers. The first is by specifying `--cpus`. That's what Maoni is talking about, and there is a [rounding algorithm](https://devblogs.microsoft.com/dotnet/using-net-and-docker-together-dockercon-2019-update/#support-for-docker-cpu-limits) that uses the next whole number if a decimal is provided. The second model is with CPU affinity, which can be specified with `--cpusets`. That means you specify exactly which cores you want to use, which by extension defines the number of cores. In both cases, you may want to [tell docker one thing and your scaling algorithm another](https://github.com/dotnet/runtime/issues/622#issuecomment-564648520).

From what we have seen, there are a set of folks that want to more aggressively scale (provide a higher value) than the `--cpus` value, in particular, would allow. That's why we [enabled setting `Environment.ProcessorCount` with an environment variable](https://github.com/dotnet/runtime/issues/48094). It is very similar to [MAXPROCS in golang](https://golang.org/pkg/runtime/#GOMAXPROCS).

## If you worked at either a startup or a big bank, what would you look for in a container-friendly platform like .NET, but not necessarily .NET?

**Michael:** Security is the first thing that comes to mind.  I would look for a platform that addresses security vulnerabilities in a timely fashion.  If the platform offers Docker images, I would expect the images to get updated as part the product release, not as an afterthought.  I would also expect the Docker images to get updated within hours of any base image (e.g. distro) updates as well as anytime other components included in the images received critical security updates.

**Glenn:** Images supported by the team that makes the product. It indicates that they understand the importance of the scenarios and that you are going to get the images as fast as they can be delivered. Indicators that the team values the scenarios, i.e. features like we discussed in other answers. There are a few stacks that meet those criteria and I think they lead to the best customer experiences.

**Maoni:** Aside from things like security as Michael pointed out, and assuming the platform meets my basic functional/perf needs, I would look for a platform that gives me the best tools for doing diagnostics in a container ;).

**Manish:** Few things which are important for any platform:

* Ease to development
* Ease of Deployment
* Monitoring and Diagnosability in container environments
* Performance in container environments
* A platform which is under continuous development so any bugs/issues can be resolved in good time

**Rich:** I would look for the following:

* Publicly [talking about containers](https://devblogs.microsoft.com/dotnet/category/containers/) on a breadth of topics.
* Open and [best practice Dockerfiles](https://github.com/dotnet/dotnet-docker/tree/main/src), [samples](https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md) and [docs](https://docs.microsoft.com/dotnet/core/docker/build-container).
* [Strong performance culture](https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-5/)
* [Strong security posture](https://devblogs.microsoft.com/dotnet/staying-safe-with-dotnet-containers/)

On the last point, we get the strongest requirements from within Microsoft and the US Government. It's a rare day when a business (of any size) provides us with new security requirements.

The one clear exception is container image signing. We made a conscious decision not to support [Docker Content Trust](https://docs.docker.com/engine/security/trust/), even though we've been asked to by some customers. We are waiting for [Notary v2](https://github.com/notaryproject/nv2) and plan to support it (sign our images and validate signatures of dependent base images) when it is ready.

## Do you think of containers as primarily a deployment story or do you think devs should develop in containers, like VS Code Remote or Docker Tools for Visual Studio?

**Brigit:** With the advancement and growing popularity of dev containers, .NET dev containers provide current and potential .NET developers a lot of great options and flexibility. 

In the Visual Studio Code [Remote - Containers](https://marketplace.visualstudio.com/items?itemName=ms-vscode-remote.remote-containers) extension, we have a `Remote-Containers: Try a Development Container Sample...` command that allows users to quickly try different sample apps in dev containers. We have one for .NET, which clones https://github.com/microsoft/vscode-remote-try-dotnetcore in a [container volume](https://code.visualstudio.com/docs/remote/containers#_quick-start-open-a-git-repository-or-github-pr-in-an-isolated-container-volume). 

We have several .NET definitions in our [dev containers definitions repo](https://github.com/microsoft/vscode-dev-containers/tree/main/containers), which form the basis of definitions users can leverage in Remote - Containers and GitHub Codespaces. 

With this in mind, I think developing in containers is a great story for developers from a variety of tech stacks, including .NET. It can be helpful in scenarios where folks want to get started quickly and haven't installed .NET on their machine yet (new computer, students/devs who don't know how to install or are new to .NET), have apps compatible with specific versions of .NET or other toolsets (i.e. an app works specifically with .NET Core 3.1, but maybe I have .NET 5.0 or 2.1 on my local machine, or in use in other apps; or maybe I'm using different versions of Node.js + .NET across this and other projects).

I also believe .NET is looking at how to get students up and running - we've found dev containers can be a great resource in education, such as saving time at the beginning of the semester when students need to install new tools for their classes. We have a [blog post](https://code.visualstudio.com/blogs/2020/07/27/containers-edu) about leveraging dev containers in education and how educators have found success with dev containers.

**Rich:** If you want to learn containers or prototype with one of your apps, then Visual Studio and Visual Studio Code tools are great options. If you want to move a suite of apps to a container hosted service like [AKS](https://azure.microsoft.com/services/kubernetes-service/), then I'd suggest learning more about that service, prototyping in terms of the service and then working your way back towards your actual apps. We've seen folks prototype with our [samples apps](https://hub.docker.com/_/microsoft-dotnet-samples) for that purpose.

In terms of daily development, that's more of a toss-up in my mind. One of the great things about .NET is that it is a true cross-platform runtime and does a good job of hiding operating system differences. In most cases, you can successfully develop on one operating system and deploy to another, at least for web apps and services (which is what we're focussed on for containers).

If I want Linux (and I'm on Windows), I typically first reach for [WSL2](https://docs.microsoft.com/windows/wsl/about). That gives me a persistent file system, all the unix-style commands I want (like `time`, `xargs`, and `grep`). I'd say I split half-way between using WSL2 as a terminal session and using the WSL2 Remote feature of VS Code. Both are excellent.

I also use `zsh` on macOS a fair bit and `bash` on my Linux machine. I consider those as similar to the experience I just described with WSL2.

My next step towards fidelity with a prod environment is volume mounting source or binaries, in an SDK or runtime container, respectively. I do this frequently. It's a great way to validate that an app works with a given distro, like [Alpine Linux](https://alpinelinux.org) or with [container limits set](https://github.com/dotnet/runtime/issues/53149).

In terms of development, I rarely actually build app container images. It takes too long and provides little value outside of the other options, for development workflow.

I'm glad to see Microsoft investing so much in both Linux and container options for developers. It provides a lot of choice for developers, and enables people to be successful with their preferred workflows.

## Are we at “peak containers” yet or is there still a lot of container growth left?

**Brigit:** A lot of growth in the dev containers space - we're excited to see how folks continue to adopt them and hear their feedback.

We have a set of dev container definitions in https://github.com/microsoft/vscode-dev-containers as I mentioned above, and we collect feedback through that repo and also accept community contributions for additional definitions or updates to the current definitions. 

We're also constantly working with the community to improve the dev containers experience (Remote-Containers extension, dev containers features/workflows/properties) in https://github.com/microsoft/vscode-remote-release/issues. 

**Rich:** This is one of those typical adoption curve questions. I think that containers are completely accepted and ubiquitous at this point. At the same time, we're still a ways out from peak containers. From talking to my friends on the AKS team, there service continues to grow, which is a pretty good indication that we're not at peak containers.

Certainly, some folks are looking at using [wasm](https://bytecodealliance.org) as the next generation application deployment and execution model for cloud apps. It's possible that wasm on the server may become a reality soon, but it would take some time to slow down the momentum of containers. In terms of investments on our team, we're betting that containers remain the most popular cloud deployment model through 2025. It's hard to predict further than that.

I guess we'll really have reached peak containers when deploying software to a bare VM is something you did "back in the day".

## Containers seem like they are CI/CD 2.0 in some ways. That's good, but they are a lot to manage. What's the best way to manage CI workflow through to prod and also staying in compliance with CVE management?


## Once you get into the microservice architecture, like Kubernetes, you are into more complexity, including sidecars like Istio and dotnet-monitor. What's the flip-over point when that is worth it, and what's your guidance for getting started? Is tye the answer? It reminds me of this video.

The video: https://www.youtube.com/watch?v=y8OnoxKotPQ


## Back to .NET, my take is that .NET is the same ball park as any other competitive container platform now. Is that fair? What would we need to do in .NET to have stronger competance and capability with containers?



## What's the most important focus area going forward? Ease of use of CNCF-style technologies, hardening .NET for constrained containers, support for base image models like distroless, generating OCI images from msbuild, better container observability and diagnosability, or just moving .NET forward generally?


## Container size is such a big deal, particularly for startup in a service with no cache. Supporting Alpine was brilliant. What's the next step function in cutting size. It's needed!


## containerd seems to be growing in importance. Will that impact the average developer, or it is cloud hosting concern?



## Last question ... what am I missing if I'm using another platform? What's something great .NET brings to the table?


## Closing

One of the engineers on the team likes to say that "containers are like water". That captures our philosophy pretty well. We think of containers as being just another option for .NET apps, and do our part such that all the low-level details are taken care of. At the same time, there is complexity with using multiple containers in production, and there are industry solutions for that. Our goal is to enable you to use those systems -- like from [CNCF](https://www.cncf.io) -- with the same ease as any other cloud-oriented development platform.

Thanks again to Michael, Maoni, Manish, Glenn, David, and Brigit for sharing your insights on containers.
