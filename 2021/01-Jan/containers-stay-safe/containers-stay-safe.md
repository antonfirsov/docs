---
post_title: Staying safe with .NET containers
username: rlander@microsoft.com
microsoft_alias: rlander@microsoft.com
categories: .NET Core
desired_publication_date: 2/1/2021
summary: An update on using .NET and containers together -- how to stay safe and keep your container images up-to-date.

---

Container-based application deployment and execution has become very common. Nearly all cloud and server app developers we talk to use containers in some way. We mostly hear about public cloud use, but also IoT and have even heard of .NET containers pulled and used over satellite links on cruise ships. In the early days, we would share compelling reasons why people should consider containers. We no longer do that because containers are so widely used now. We are focused on making .NET a great container platform, and adapting as the container ecosystem evolves.

In this post, I'm going to tackle staying safe and up-to-date with containers. Doing that can be challenging and not always intuitive. This post describes our approach to helping you with that -- largely via our container image publishing model -- and with associated guidance of the images we publish. The post has a strong bias to Linux, because there is more to know and more nuance on Linux. It replaces a similar 2018 post, [Staying up-to-date with .NET Container Images](https://devblogs.microsoft.com/dotnet/staying-up-to-date-with-net-container-images/).

I decided to start 2021 with an update on .NET containers, and answer common questions we hear. I posted similar content in past years: [2017](https://devblogs.microsoft.com/dotnet/using-net-and-docker-together/), [2018](https://devblogs.microsoft.com/dotnet/using-net-and-docker-together-dockercon-2018-update/), and [2019](https://devblogs.microsoft.com/dotnet/using-net-and-docker-together-dockercon-2019-update/). This year, I'm planning on publishing a series of posts, each dedicated to a thematic slice of the container experience. I'm hoping to get some of my colleagues to post, too. These posts will cover how we've made .NET into a great container platform, but also suggestions on how you can be a great container user.

## Meet the team

> The team is responsible for maintaining and improving container image publishing infrastructure not manually publishing images. This infrastructure runs day and night, including when the team is sleeping.

I'll start by telling you a little about the team that works on the image publishing slice of our container experience. Knowing more about what we do helps you better understand the images you are using.

The container publishing team is made up of three developers -- Dan, Matt, and Michael -- and one Program Manager -- Rich (me). You can follow what we're doing in our two primary repos: [dotnet/dotnet-docker](https://github.com/dotnet/dotnet-docker), [microsoft/dotnet-framework-docker](https://github.com/microsoft/dotnet-framework-docker). We triage issues in those repos every week, and try to address everything reported or asked for in issues, discussions, or pull requests. You'll also find [Dockerfiles](https://github.com/dotnet/dotnet-docker/tree/master/src) for all .NET images, and [samples](https://github.com/dotnet/dotnet-docker/blob/master/samples/README.md) that demonstrate common ways of using them.

On the face of it, our job is easy. We produce new [container images for .NET](https://hub.docker.com/_/microsoft-dotnet) servicing and preview releases. We are not responsible for building .NET (a larger team takes care of that). We only need to write [Dockerfiles that unpack and copy .NET builds into a container image](https://github.com/dotnet/dotnet-docker/blob/d985263c5fad70046004c02b3427686256168e63/src/sdk/5.0/alpine3.12/amd64/Dockerfile#L22-L28). As is often the case, theory doesn't track closely to reality.

Container pulls are hard to count (layers vs manifest-only pulls) but it is safe to say there are ten million .NET image pulls a month. There are two things that are ever-present in our minds, as fundamental requirements of that scale. The first is that a lot of people are counting on us to deliver software that is high-quality and safe. The second is that there is an inherent diversity of needs demanded by the developers and devops professionals driving all those image pulls. The pull rate has grown to that level, in part, because we satisfy a lot of those needs. Those needs are what we continue to focus on as we consider what to do next. For example, we publish images for three Linux distros, as opposed to just one.

Much of that will come as no surprise. Less obvious is how we [manage updates for Linux distro base images](https://github.com/dotnet/docker-tools/blob/main/documentation/base-image-dependency-flow.md) that we support -- Alpine, Debian, and Ubuntu -- that we (and by extension you) rely on. It was obvious from our early container days that managing base image updates was a task for a cloud service and not people. In response, we built [significant infrastructure](https://devblogs.microsoft.com/dotnet/how-the-net-team-uses-azure-pipelines-to-produce-docker-images/) that watches for base image updates and then re-builds and re-publishes .NET images in response. This happens multiple times a month, and in rare cases, multiple times a day.

## Dockerfiles

> .NET Dockerfiles rely on versioned URLs that reference public and immutable SHA2-validated .NET builds and other resources via HTTPS.

[Dockerfiles](https://docs.docker.com/engine/reference/builder/) are a text-based recipe format for defining container images, part shell script, part declarative format, part (arguably) functional programming language. There are many positive aspects to Dockerfiles. Perhaps the the most compelling part is the concept of layers, their hash-based identity and the caching system that is built of top of those characteristics.

We know many people use our Dockerfiles to produce their own images or as a starting point to producing images that are different in some way. We endeavor to make our Dockerfiles best practice, self-consistent, and easy to use. We've always thought of the Dockerfiles and the resulting images as equally important deliverables of our team.

From very early on, we decided that the [Dockerfiles we maintain on GitHub](https://github.com/dotnet/dotnet-docker/tree/master/src) must be the true source of the images we publish. We've kept to that idea, and see it as a contract with you. This topic is both subtle and incredibly foundational. I'll explain.

Imagine you pull and inspect a .NET image (from our registry) and then rebuild the same image from the Dockerfile we've shared as its apparent source, on your own machine. You get the same result (I'll define that shortly). That's comforting. It means that using official .NET images is just a convenience as you can build them yourself. But what happens if you get a different result. That's concerning, particularly if no explanation is provided. What are you to think? Your mind races. The difference could be the result of something nefarious or accidental. Only an investigation could help answer that question, and who has time for that?

The following workflow demonstrates how to compare a registry image with a locally built one (both from the same [source Dockerfile](https://github.com/dotnet/dotnet-docker/blob/fb30bdce6e982de944d25bc07b6f5d701ec91a1e/src/sdk/5.0/alpine3.12/amd64/Dockerfile)) using the Google [container-diff](https://github.com/GoogleContainerTools/container-diff) tool:

```console
C:\>curl https://storage.googleapis.com/container-diff/latest/container-diff-windows-amd64.exe -o container-diff.exe
  % Total    % Received % Xferd  Average Speed   Time    Time     Time  Current
                                 Dload  Upload   Total   Spent    Left  Speed
100 14.6M  100 14.6M    0     0  14.6M      0  0:00:01 --:--:--  0:00:01 20.3M

C:\>git clone https://github.com/dotnet/dotnet-docker
Cloning into 'dotnet-docker'...

C:\>cd \dotnet-docker\src\sdk\5.0\alpine3.12\amd64

C:\dotnet-docker\src\sdk\5.0\alpine3.12\amd64>git pull
Already up to date.

C:\dotnet-docker\src\sdk\5.0\alpine3.12\amd64>docker pull mcr.microsoft.com/dotnet/sdk:5.0-alpine
5.0-alpine: Pulling from dotnet/sdk
Digest: sha256:fb1a43b50c7047e5f28e309268a8f5425abc9cb852124f6828dcb0e4f859a4a1
Status: Image is up to date for mcr.microsoft.com/dotnet/sdk:5.0-alpine
mcr.microsoft.com/dotnet/sdk:5.0-alpine

C:\dotnet-docker\src\sdk\5.0\alpine3.12\amd64>docker build --pull -t dotnet-sdk:5.0-alpine .
Sending build context to Docker daemon  4.096kB

<snip/>

C:\dotnet-docker\src\sdk\5.0\alpine3.12\amd64>\container-diff.exe diff mcr.microsoft.com/dotnet/sdk:5.0-alpine daemon://dotnet-sdk:5.0-alpine --type=history

<snip/>

-----History-----

Docker history lines found only in mcr.microsoft.com/dotnet/sdk:5.0-alpine: None

Docker history lines found only in dotnet-sdk:5.0-alpine: None
```

This result tells you that the Dockerfiles used to build the two images are the same. That's -- as far as I'm aware -- the most straightforward test to validate the fidelity of a registry image with its apparent source. The test passed. Comparing image digests won't work; they will never match.

Let's inspect .NET Dockerfiles one layer deeper. .NET Dockerfiles reference both [Linux](https://github.com/dotnet/dotnet-docker/blob/c0e8be8a44b47b1dcc2a5b4b2ebd92022087ac0b/src/runtime-deps/3.1/alpine3.12/amd64/Dockerfile) and [.NET](https://github.com/dotnet/dotnet-docker/blob/fb30bdce6e982de944d25bc07b6f5d701ec91a1e/src/sdk/5.0/alpine3.12/amd64/Dockerfile) resources. For Linux resources, we reference operating system tags and packages whose content will change over time. For .NET resources, we rely on publicly available Azure blob resources (that our team maintains) that don't change.

If you look at [git history on our repos](https://github.com/dotnet/dotnet-docker/commits/master), you will notice that the Linux-specific references never change, and the .NET references are updated once a month. The diffs are a direct outcome of the different patterns that I just explained between Linux and .NET resources.

There are two cases where the container-diff tool will report that the registry and local images that you are comparing are the same (in terms of Docker history lines), but will be misleading because the images are actually different. The first is that Debian, for example, has been updated and we haven't pushed a .NET update on top of it yet. As you'll soon read, there is a very small window of time where that is at all likely. The second is that a package that .NET relies on has been updated since we last built and published .NET images. This case is quite likely after even a couple days after we publish an image. We don't proactively re-build images when packages are updated, which I'll explain more later.

Sidebar: Various folks in the container ecosystem are looking at enabling deterministic images. We welcome that. See [Building deterministic Docker images with Bazel](https://blog.bazel.build/2015/07/28/docker_build.html) and [DETERMINISTIC DOCKER IMAGES WITH GO MICROSERVICES](https://www.thestrangeloop.com/2019/deterministic-docker-images-with-go-microservices.html).

Pulling this all together: since our Dockerfiles are public, and only use publicly-available resources, you can produce the same results we do to produce your own images or to validate the fidelity of the images relative to our published Dockerfiles. Transparency is a key tenet for developing trust with containers.

There is an industry effort called [secure supply chain](https://openssf.org/). It's not new, but it is as important now as ever. We are strong advocates of secure supply chain goals and principles on the .NET Team and at [Microsoft](https://openssf.org/about/governing-board/) generally. The topics that I'm touching on with transparency and verifiable artifacts are fundamental tenets of that effort.

## Pedigree and provenance

> When you use official .NET images, you place significant trust in Microsoft. That trust should be grounded in the published policies that we use to produce and distribute software.

I'll start by defining these two terms, per our use. Pedigree defines [where source code comes from](https://github.com/dotnet/dotnet-docker/blob/master/documentation/image-artifact-details.md) and its ownership (licensing). Provenance defines the manner and environment in which source code is managed and then built into binaries and made available to you. Let's focus on the layer above the operating system, which we'll call the ".NET layer".

On pedigree, our policy is to only include code (typically in binary form) that is owned and maintained by the [.NET Foundation](https://dotnetfoundation.org/) or [Microsoft](https://dotnet.microsoft.com/). The code must be licensed as [MIT](https://github.com/dotnet/runtime/blob/master/LICENSE.TXT) (or compatible) or with a [Microsoft license](https://github.com/dotnet/core/blob/master/license-information-windows.md). We scan that code for pedigree violations as a due diligence exercise. It mitigates another party (successfully) making a copyright claim on the software in the .NET layer. In some cases, there may be packages (templates would be a good example, in SDK images) that are not owned by the Microsoft or the .NET Foundation, and those are scrutinized.

Determining pedigree with Linux distros and packages can be difficult. We documented a workflow for [finding Linux legal metadata](https://github.com/microsoft/containerregistry/blob/master/legal/Linux-Legal-Metadata.md) in container images that may be helpful to you. It was produced in collaboration with the [open source programs office](https://opensource.microsoft.com/) at Microsoft.

On provenance, we build and publish .NET containers images per Microsoft security practices (including regular mandatory security training for engineers). We use [Azure DevOps Pipelines](https://devblogs.microsoft.com/dotnet/how-the-net-team-uses-azure-pipelines-to-produce-docker-images/) to build .NET images. The .NET product build also uses Pipelines. GitHub is currently working on satisfying our feature requirements so that we can adopt GitHub Actions in the future. .NET images are published to the Microsoft Container Registry (MCR). By using MCR, we extend our provenance promise all the way to, and including, the registry.

As stated earlier, our Dockerfiles rely on public resources, via HTTPS URLs. HTTPS on its own isn't good enough. It protects the transport, but not the source from tampering or accidental change. .NET Dockerfiles contain SHA2-based [content integrity checks](https://docs.microsoft.com/dotnet/standard/security/ensuring-data-integrity-with-hash-codes) for each external resource. If these checks fail, the build fails (which is good). I recall that our container build failed once due to these checks failing as a result of a failure in our release publishing. The issue was resolved immediately.

## Building images

> Our infrastructure re-builds and publishes .NET images after Linux base image updates, within twelve hours.

I just described that .NET images have a high-fidelity relationship with their published Dockerfiles. To deliver on that promise, we have to build official .NET container images with the public (unmodified) Dockerfiles. Think "clone and build", with nothing in-between. That's exactly what we do. If you look at git history, you'll notice Dockerfile pull request merges on release-day, prior to image availability. This behavior is (to some degree) demonstrating the practices I'm describing.

.NET release day -- as much as people on the team are always excited about delivering new features -- is a mundane affair. It is mundane because we can predict the day and hour of the release. The more interesting case is when Linux base images are updated. Our infrastructure checks for new images every four hours. We could check more often, but we don't believe that there is a compelling need, nor do we want to put unnecessary stress on someone else's cloud infrastructure.

Over time, we've come to the viewpoint that a twelve-hour [SLA](https://en.wikipedia.org/wiki/Service-level_agreement) for publishing Linux base image updates is the sweet spot. Certainly, it doesn't take near that long to run through the process. In fact, most of the time within that duration is intentional delay. I'll explain why.

We analyzed the logs from our infrastructure and found that some distros publish base images updates multiple times across the same day. For example, Ubuntu x64 images might be published in the morning, and Arm64 images in the afternoon, all for the same logical Ubuntu update. We've also seen cases where there have been multiple updates for the same architecture in the same day. We don't know why, and it isn't important for us to know. Our infrastructure notices these updates, but then waits to see if anything else happens across an approximately twelve hour period. There are benefits to waiting, and no real downsides.

We know that many of you have infrastructure that watches for .NET updates (just like we do with Linux distro base images). We'd rather not unnecessarily spin your infrastructure if we can avoid it. If we can collapse two updates into one across a short period, that seems like a win.

## Vulnerabilities

> Vulnerabilities, and the CVEs that track them, are one the most challenging and confusing aspects of using containers.

Security vulnerabilities are the most common topic we get asked about. This is because there is a lot of danger associated with them, but best practices in managing them are lacking. Vulnerability management is not the same on Windows and Linux, although the general landscape is the same. In this section, I'll address the topic in broad terms, and then Windows and Linux specifically.

Lifecycle of vulnerabilities: introduction -> discovery -> disclosure (as [CVEs](https://cve.mitre.org/)) -> resolution (patch distributed) -> all machines patched.

This sequence does not address a vulnerability being exercised maliciously, which can happen any time after the point of discovery, and past resolution. Malicious use isn't really part of the sequence but overlays it, and is also subject to software vendor and user behavior.

Sidebar: CVE reports are most often created for vulnerabilities that need action on the part of an end user, be it installing the latest OS update, or changing a config file. If something can be fixed automatically it might not get a CVE. A CVE can be created in response to a request by the software or hardware vendor (like Debian or Microsoft), or security researchers.

The time between each of the points in the vulnerability lifecycle is undefined, particularly between discovery and disclosure. The time gaps are primarily influenced on who initially discovers the vulnerability. If a vulnerability is discovered by someone that reports it through a vendor’s [reporting mechanisms](https://www.microsoft.com/en-us/msrc/faqs-report-an-issue) (including [bug bounties](https://www.microsoft.com/msrc/bounty-dot-net-core)) or internally by the software vendor, then the gap between discovery, correction and public disclosure is likely to be short and uneventful. Vulnerabilities may be discovered by individuals or groups that prefer not to report them to vendors for any number of reasons. In this case the timeline between discovery, disclosure and patches to address the issue is unpredictable.

In the general case, vulnerabilities don't just magically appear. There are no tricky vulnerability fairies that cause the latest build to be bad. The majority of vulnerabilities are present in system software months and years before they are disclosed as CVEs. It's a bit like diamonds and oil. They are present in the ground long before they are discovered and sold in a marketplace.

In any given operating system update, there may be a mix of unpatched and recently patched CVEs. How does one reason about that from a safety perspective? There isn't a general metric that validates that the build is acceptable to use, certainly not based on barrels or carats of CVEs. Should one focus on the most severe CVE and make a decision based on it? If there is a severe vulnerability in an operating system and it was discovered to have been present for five years, should one revert to an operating system build from five years ago? That's impractical. What about the 1000+ CVEs that have been patched since (let alone the addition of operating system features). If there is one patched high-severity CVE and five unpatched medium-severity CVEs in a given build, is that good? Builds with [unpatched CVEs present a challenge often without an immediate resolution](https://github.com/debuerreotype/docker-debian-artifacts/issues/111).

Sidebar: We do not publish the results of our operating system vulnerability scans. This is something that we'd like to do in the future. When people share the results of their security scans, we don't have official results for them to compare with. Having official and up-to-date scan results would be useful.

It's easy to get focused on operating system CVEs. There are thousands of components and subsystems in an operating system. Most apps won't exercise even a quarter of the functionality in an operating system image. Apps, however, also depend on application platforms, like .NET. Your app almost certainly exercises a larger proportion of functionality of its app platform than operating system, and will have commensurate exposure. One can imagine that given five operating system CVEs and five app platform CVEs, that it's likely that only one of the operating system CVEs and at least three of the app platform CVEs will apply to your app. I just made that up, but it's likely true enough and gets the point across.

In general, .NET CVEs should be your first concern. I'd say the same for any app platform. If you use the official .NET images, and delay consumption of them because of operating system CVEs, you may want to [re-think that](https://en.wikipedia.org/wiki/Priority_inversion).

This far-from-ideal situation provides context on why security research and bug bounties are so important. It also strongly suggests that patching CVEs is not reasonable as a singular security defence, but is a key aspect of a more general approach.

## Managing vulnerabilities

> Our approach to vulnerabilities is a healthy mixture of safety and pragmatism.

I recall a conversation with a Microsoft leader on this topic a few years ago. They asked "will you knowingly publish Linux images with CVEs?" I said "Yes." That wasn't the answer that was expected. You have to zoom out to appreciate why the policy we use is a reasonable one (if not the only one available) and why traditional practices may no longer apply.

On our team, we are presented with the same two things every month: operating system updates, and .NET security patches. We have to do something productive with those two things, and together. There are only so many ways to combine operating system and .NET updates. We need to publish new operating systems patches so that you have easy access to them, and have to pick an operating system version on which to release .NET patches. On top of that, we need to publish updated images, month over month, in a way that seems predictable to you and absolutely avoids surprises.

That leaves the serious question of how we reason and react to unpatched operating system CVEs. The only reasonable model we've found is to publish .NET updates on the latest operating system update (or "tip"). As I shared earlier, there is no good model (that we've identified) for going backwards, so we've chosen to only go forwards. We've also chosen to believe that operating system vendors make well-intentioned choices with their users' best interests in mind, and see no need to get involved in the middle of your already established relationship.

To make this concrete, we're not going to publish an older build of Windows or Linux in March than we did in February, for example. That would be very bad and completely irresponsible on our part. In fact, if you ever see stale operating system versions in .NET images, that's the [black swan event](https://en.wikipedia.org/wiki/Black_swan_theory) that should cause your security scanner's [anomaly detection](https://en.wikipedia.org/wiki/Anomaly_detection) filters to set off alarm bells because it is actually more serious than any CVE, and you should absolutely halt using our images as a result.

Pulling this all together: we do not gate operating system base image updates on security vulnerability scans. We always publish the latest updates for all operating systems. Our approach gives you flexibility. You can adopt our tip-based policy or gate image updates on some metric of your own devising. If you do not like our container-building policy, that's probably a good reason to build your own images, and not rely on ours. Since we publish Dockerfiles for .NET images, we've given you a head-start on doing so.

## Linux base images

> We see Linux as a set of continuously updated (base image) tags on a registry, as it relates to publishing .NET container images.

Linux builds and container images are produced with their own set of practices and policies. I'm not going to describe them here, and I'm not informed enough to do that with credibility. I think Linux is [great](https://cloudblogs.microsoft.com/windowsserver/2015/05/06/microsoft-loves-linux/), but that's not very descriptive on its own. It's up to you to discover what you need to know about the software you use. For this section, I'm going to focus on aspects of the Linux-based software that we distribute in .NET container images, and that my team has discovered as part of our use (as a user, just like you).

[Linux is licensed as GPL](https://github.com/torvalds/linux/blob/master/COPYING). Packages available in a package manager may be licensed with the same or different license. That means that .NET containers are to some degree GPL, which is not a license typically used at Microsoft. Our view is that this is fine and is the exact same as using Linux virtual machines. The industry seems to have come to the [same conclusion](https://opensource.com/article/18/1/containers-gpl-and-copyleft).

I already discussed most of our thinking on security vulnerabilities, but will repeat myself a bit. We publish updated .NET images for [Alpine](https://hub.docker.com/_/alpine/), [Debian](https://hub.docker.com/_/debian/), and [Ubuntu](https://hub.docker.com/_/ubuntu/) as those [official images](https://hub.docker.com/search?q=&type=image&image_filter=official) are made available on Docker Hub. Our image updates of those official images are not gated by a review of Linux distro patches. Instead, we accept official image updates from the official image maintainers (at least some of which are community volunteers) as is. We've engaged with maintainers from these three distributions to greater or lesser degrees. For example, we've talked with [Tianon Gravi]((https://github.com/tianon) who is one of the Debian image maintainers, and can start a conversation should we need to. We thank all image maintainers for their stewardship and offer them a sincere hat tip.

We know from past experience that if there was a severe issue in Linux that Microsoft is likely to be made aware of it, possibly as part of an industry-wide conversation. When [that happens](https://nvd.nist.gov/vuln/detail/CVE-2020-1967), we react very quickly, and will work weekends and holidays to do the right thing on your behalf.

Microsoft uses Linux extensively in its own operations, and has significant incentive to keep those operations safe, including ones that use .NET. That means when you use some form of Linux from Microsoft, it's also a form of Linux that Microsoft is willing to use itself. That's not a guarantee of anything, but directionally favorable.

## Linux packages

> "Why is Microsoft releasing images with vulnerabilities? Please fix this."

We get asked about vulnerability scans of .NET images frequently. The scans sometimes flag legitimate issues that require us to update a package. However, much more frequently the results are false positives. There are multiple reasons [why vulnerability scans show CVEs in images](https://github.com/docker-library/faq/#why-does-my-security-scanner-show-that-an-image-has-cves):

- The CVE [doesn't apply to your use case](https://github.com/debuerreotype/docker-debian-artifacts/issues/111#issuecomment-754739434).
- The CVE is unpatched, either generally, or in the user's given distro.
- The CVE is patched but the Linux base image has not yet been rebuilt to include the patch.
- The CVE is patched but the .NET image has not yet been rebuilt to include the patch.
- The CVE is patched but not in the (stale) .NET image you are using.

Analyzing CVEs can be a challenge. The key question is applicability to your application. Conducting this analysis requires significant insight of your application and its dependencies. On the other end of the spectrum, simply counting unpatched CVEs in an application image is usually not going to be a good metric for determining the security of your system. A single CVE could be devastating and a dozen reported in an image might be benign. It depends. If you are using Linux and security is a focus of yours, you need to become comfortable reading and determining severity and applicability of CVE reports.

I'll expand on some of the scenarios I just defined with actual real-life examples.

A user was concerned because [CVE-2020-1751](https://nvd.nist.gov/vuln/detail/CVE-2020-1751) was present in their vulnerability scan. That CVE only applies to PowerPC, which .NET doesn't support. [CVE-2018-12886](https://nvd.nist.gov/vuln/detail/CVE-2018-12886) was similarly reported to us, but is more nuanced. It only applies to Arm processors, which we do support, but the user was targeting x64, which made the CVE inapplicable for them.

A user recently asked why [CVE-2020-1971](https://security-tracker.debian.org/tracker/CVE-2020-1971) was not resolved. It was resolved in our registry, but not when the image they scanned (from their registry) was built. We recommend rebuilding images before scanning. You can also pull our images to see when they were last built. Looking at git history of our Dockerfiles is not a good source of information for Linux updates.

You can check the created date of an image in our registry with the following workflow:

```console
% docker pull mcr.microsoft.com/dotnet/sdk:5.0   
5.0: Pulling from dotnet/sdk
Digest: sha256:084344040abb10b8440e7b485c962d8ef322cbc1724841a4bdd913b20b75ec4e
Status: Image is up to date for mcr.microsoft.com/dotnet/sdk:5.0
mcr.microsoft.com/dotnet/sdk:5.0
% docker inspect --format='{{.Created}}' mcr.microsoft.com/dotnet/sdk:5.0
2021-01-30T14:33:23.522932595Z
```

The `docker history` command can be used to get the same information. It's both easier and harder to use.

Another confusing topic is how updates are released across Linux versions. For example, a CVE may be patched in Debian and not Ubuntu or in Debian 10 and not 9. If you are using Debian 9 (in this fictitious example), then you need to wait for a fix or switch to Debian 10. Per our observations, there is a bias (although not strong) to CVEs being resolved first in newer distro versions. We recommend using the latest version of a distro. We also recommend Alpine because it contains fewer packages and seems to have more limited CVE exposure due to its reduction in surface area.

Our publishing infrastructure is oriented around base image tags. However, we also install [packages](https://github.com/dotnet/dotnet-docker/blob/c0e8be8a44b47b1dcc2a5b4b2ebd92022087ac0b/src/runtime-deps/3.1/buster-slim/amd64/Dockerfile#L5-L14). .NET images are updated multiple times a month due to distro updates, particularly with Debian and Ubuntu, and also for monthly .NET updates. The latest package updates are always installed when images are rebuilt.

There are cases where our team should be re-building images due to package updates. We do not have a system in place to discover when we should rebuild container images due to patched CVEs in newer packages. We'd like a system where our publishing system auto-rebuilds images when a fix is available for a CVE that has a certain severity or higher. Instead, we rely on base image updates as the .NET image rebuild signal and install the newest package updates as a side-effect of that operation. In the general case, this doesn't matter since the base image is updated frequently. In some cases, it likely does.

As suggested earlier, if there is a severe issue, our team will likely be contacted, and we'll act on it. It's more likely that .NET images will have exposure to medium-severity package CVEs as a result of this gap. Counter-intuitively, this challenge isn't a practical problem for Debian and Ubuntu because they are typically updated multiple times a month. It is more of a problem for Alpine images because it isn't updated nearly as often due to its reduced surface area (which is what leads to its legitimately stronger security reputation).

## Linux distro policies

> We publish .NET images for distros that are commonly used in containers and that we expect will get significant use. Other combinations are possible, but DIY.

.NET has a set of [Linux distro support policies](https://github.com/dotnet/core/blob/master/os-lifecycle-policy.md) that describe distros, distro versions and architectures we support. We publish container images for a subset of those. We publish [OS Support notices](https://github.com/dotnet/core/labels/os-support) so that you can follow our activities.

[Our policies](https://github.com/dotnet/dotnet-docker/blob/master/documentation/platform-matrix-suport.md) are a function and a further refinement of each distro's release schedule and lifecycle.

When a new .NET version is released, we produce new container images for that version with the:

* Latest Alpine version.
* Latest Debian version.
* Latest Ubuntu LTS version.

Note: We published images for non-LTS Ubuntu versions at one point. They got very little use. The majority of people want to use Ubuntu LTS versions. That works great for us. We prefer producing a more narrow set of images with more even use.

When a new distro version is released:

- For Alpine, we publish new Alpine container images for all in-support .NET versions. At that point, we will announce that we will stop producing images for the oldest Alpine version (that we produce images for) in three months.
- For Debian, we publish new Debian container images for the latest .NET version, for example, .NET 5.0 (currently the latest version). For example, the `5.0` tag references Debian 10. A new opt-in tag like `5.0-bullseye` would be created, which would reference Debian 11. Later, the `6.0` tag would reference Debian 11 images, and Debian 10 images would not be provided (for 6.0).
- For Ubuntu, we publish new Ubuntu LTS container images for the latest .NET version and the latest .NET LTS version (if they differ). The next LTS version will be 22.04.

On one hand, these policies are confusing because they are so different. On the other, they are what we believe 80%+ of those distro users will be happy with. The policies make more sense if viewed from the context of that community, or at least that's our intention.

An important take-away is that you may need to be on a recent .NET version if you want to the latest distro version from the official .NET images. For example, we publish .NET Core 2.1 images on top of Debian 9. .NET Core 3.1 and .NET 5.0 are published with Debian 10. We have this policy to limit our image matrix. We also feel that if you are willing to upgrade your operating system version, you should also consider updating your .NET version. We try to make that as easy as possible.

## Windows

The Windows story around vulnerabilities is comparatively simpler. This is for two reasons: vulnerability disclosure and fixes typically coincide on the same day (Patch Tuesday), and in-support Windows versions are updated together. Patch Tuesday (second Tuesday of the month) is also easy to plan around.

We publish new .NET container images every Patch Tuesday, built on top of updated Windows base images. We work closely with the Windows team so we don't have to guess their schedule. As a result, we don't have the same twelve hour delay with Windows as we do with Linux. The Windows team typically publishes their images by 12pm Pacific Time on Patch Tuesday, and we produce our images soon after, typically on the order of a few hours.

We get very few requests from users about unpatched CVEs in Windows. If you are using the latest .NET Windows-based images, your image is patched as it relates to both .NET and Windows. You only need to think about patching once a month, for both Windows and .NET. It's that simple.

To be fair, I'm focused on CVEs that the operating system vendor has disclosed. This is a situation where Windows and Linux differ significantly. With Windows, CVEs and fixes typically coincide on the same day, and with Linux, disclosure often proceeds the fix. There are merits to both approaches. Windows and Linux are both subject to disclosures that come via other parties. In that case -- for both Windows and Linux -- CVEs will proceed fixes, and as a user, you will need to wait, possibly with no recourse. It's not hard to find [historical examples](https://heartbleed.com/).

## .NET release policies

> .NET is patched on a regular schedule, almost like clockwork.

.NET security updates are published [once a month](https://github.com/dotnet/announcements/issues?q=is%3Aissue+is%3Aopen+label%3AMonthly-Update) on Patch Tuesday, across all distribution types (containers, MSI, zips, ...), per our [Release Policies](https://github.com/dotnet/core/blob/master/release-policies.md). Updated Linux and Windows containers are released within a short release window, typically around noon Pacific Time on Patch Tuesday. If you are using the latest .NET images, your image is patched as it relates to .NET.

If for some reason, you can only rebuild your .NET container images once a month, target 6pm Pacific time every Patch Tuesday. That's not the recommended approach, but a simple and more effective policy than not rebuilding on any schedule.

## Microsoft container registry (MCR)

> We've changed where we publish container images multiple times. Sorry about that. We're certain we've found the final home, in MCR. Please update your Dockerfiles.

We publish .NET images exclusively to MCR. We work closely with the MCR team, and regularly talk about ways we can improve container user experiences.

MCR offers two primary benefits:

* We can extend our container image provenance promise all the way through to and including the container registry.
* MCR is available in most Azure regions, enabling the ability to pull .NET images in-region and also acting as a globally replicated container CDN.

.NET Core and .NET 5.0 images are published to `mcr.microsoft.com/dotnet`. That's a [recent change](https://github.com/dotnet/dotnet-docker/issues/1765). We recommend that you update your Dockerfiles and scripts to use this location. It will make adopting new releases easier. .NET 5.0 and .NET 6.0 are only published to this location. We hope that we do not have to make any more registry changes going forward. We know that they are painful for you. They are also painful for us.

I'll share a little history so that you can ensure that you are pulling images from the right place.

* Starting with .NET Core 1.0 and .NET Framework 4.6.2, we published images to Docker Hub, in the `microsoft/dotnet`, `microsoft/aspnetcore`,  `microsoft/aspnetcore-build`, `microsoft/dotnet-framework`, and `microsoft/aspnet` repos.
* In 2018, we began publishing .NET images to MCR, in the `mcr.microsoft.com/dotnet/core` and `mcr.microsoft.com/dotnet/framework` repos.
* In 2020, with the re-branding of .NET Core to ".NET" with .NET 5.0, we began publishing .NET 5.0 and in-support .NET Core images to `mcr.microsoft.com/dotnet`, removing `/core`. .NET Framework publishing has not changed.
* .NET Core 2.1 and 3.1 images are still published to `mcr.microsoft/dotnet/core`, for compatibility reasons. .NET Core 2.1 images are also published to the older Docker Hub locations -- such as `microsoft/dotnet` -- for the same reason. This dual-publishing model is in place to maintain your existing docker builds without breaking your access to images.

We recommend that you use the new and shorter `mcr.microsoft.com/dotnet` location going forward.

## Tips

Much of this post is focused on the challenging topic of CVEs. There are, however, some concrete and simple things you can do that help.

* Pull before build -- You should always [pull before build](https://github.com/dotnet/dotnet-docker/tree/master/samples/dotnetapp#build-a-net-image). It is easy to build from a stale cache (which may be missing critical patches).
* Consider using a private registry -- Using a private registry for [consuming public content](https://opencontainers.org/posts/blog/2020-10-30-consuming-public-content/) insulates you from failures cases.
* Consider using a cloud container build service -- Offload the task of building images and staying up to date to the cloud. [ACR Tasks](https://docs.microsoft.com/en-us/azure/container-registry/container-registry-tasks-overview) and [GitHub Actions](https://docs.github.com/en/actions/creating-actions/creating-a-docker-container-action) are good examples.
* Pedigree and provenance apply above the .NET layer -- If you install packages in your own layers, then you need to consider when you should update your images based on security fixes being made available for those packages.
* Container image scanning -- Scanning images is a best practice and something you should consider if you are not currently doing it.

## Closing

We've seen [.NET container](https://hub.docker.com/_/microsoft-dotnet) usage grow quickly, with image pulls now in the millions per month. Developers use containers because they have important benefits that are not provided by other solutions. They enable more deterministic and (approaching) instant-on compute, with Linux and Windows operating systems, on x64 and Arm architectures.

We've learned a lot as a team over the past five years that we've been publishing container images. This post, and the ones that will follow it, are intended to help you get the most out of using .NET containers and also to provide you with the knowledge you need to make the best choices on their use.

This post includes comparisons between Linux and Windows. The descriptions of Linux and Windows are intended to be objective, and are not influenced by an agenda (other than education), nor are they intended to get people to switch operating systems. I described the various ecosystems as I see them, having significant experience with both. If there are inaccuracies, please point those out.

It is very exciting and satisfying to see container usage be such an important part of .NET usage generally. My team has put a lot of effort into providing a great experience, and I'm glad to see so many developers and companies taking advantage of it. That said, there is still a lot to do, and we would like more feedback to help direct our future efforts.

I'll leave you with a few take-aways from the post:

- It is important to rebuild your container images frequently -- multiple times a month on Linux and once a month on Windows -- if you want to limit your exposure to CVEs.
- Developing fluency with reading [official CVE reports](https://nvd.nist.gov/) will enable you to navigate CVEs big and small and as you need to.
- Patching is not a sufficient security strategy on its own. There will be periods of time where patches will not be available.
- Policies are needed ahead-of-time (in your organization) to determine what to do when patches are not available.
- The .NET container team publishes updated images -- with operating system patches -- with a 12 hour SLA. In the case of (very) severe CVEs, we will typically publish faster.

Looking forward, .NET 6.0 is shaping up to be a great release (at least as an initial plan), including for [containers and cloud native](https://github.com/dotnet/core/issues/5397). I'll write more about some of these upcoming features and scenarios in future posts.
