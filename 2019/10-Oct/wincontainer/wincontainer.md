# We made Windows Server Core container images 45% smaller

Over the past year, we've been working with the Windows Server team to make [Windows Server Core container images](https://hub.docker.com/_/microsoft-windows-servercore) a lot smaller. They are now about 45% smaller! The Windows Server team has already published the new images in the [Server Core Insider Docker repo](https://hub.docker.com/_/microsoft-windows-servercore-insider), and will eventually publish them to their [stable repo](https://hub.docker.com/_/microsoft-windows-servercore) with their 20H1 release. You can check them out for yourself. I'll tell you how we did it and what you need to know to take advantage of the improvements.

Let's start with the numbers:

* Insider images are >45% smaller than 1903 images.
* Container startup into Windows PowerShell is 30-45% faster.

These measurements are based on images in the [Windows Server Core insiders Docker repo](https://hub.docker.com/_/microsoft-windows-servercore-insider). We used PowerShell as a proxy for any .NET Framework application, but also because we expect that PowerShell is used a lot in containers.

The improvements should apply to any scenario where you use Windows Server Core containers images. It should be most beneficial and noticeable for scaling applications in production, CI/CD and any other workflow that pulls images without the benefit of a Docker image cache or that has a need for faster startup.

The Windows Server and PowerShell teams also published posts about these improvements in [post name here](https://blogs.windows.com/windowsexperience/tag/windows-server/#m4BFDOal4uXl87D3.97) and [post name here](https://blogs.windows.com/windowsexperience/tag/windows-server/#m4BFDOal4uXl87D3.97). Check out these posts as well.

## A case of playing poorly with container layers

We started this project with the hypothesis that the way .NET Framework is packaged and installed does not play nicely with the way [Docker layers](https://docs.docker.com/storage/storagedriver/) work. We found that this was the case based on an investigation we did over a year ago.

As background, Docker creates a read-only layer for each command in a Dockerfile, like `FROM`, `RUN` and even `ENV`. If files are updated in multiple layers, you will end up carrying multiple copies of that file in the image even though there is only one copy in the final image layer (the one you see and use). We found that this situation was common with .NET Framework container images. This is similar to the way Git works with binaries, if you are familiar with that model. If this makes you cringe, then you are following along perfectly well.

[.NET Framework Dockerfiles are open source](https://github.com/microsoft/dotnet-framework-docker), so I will use them as examples in the rest of the post. They are also a good source of Docker-related techniques if you want to customize your own Dockerfiles further.

The [Dockerfile for .NET Framework 4.8 on Windows Server Core 2019](https://github.com/microsoft/dotnet-framework-docker/blob/master/4.8/runtime/windowsservercore-ltsc2019/Dockerfile) demonstrates the anti-pattern we're wanting to fix, of updating files multiple times in different layers, as follows:

```Dockerfile
# escape=`

FROM mcr.microsoft.com/windows/servercore:ltsc2019

# Install .NET 4.8
RUN curl -fSLo dotnet-framework-installer.exe https://download.visualstudio.microsoft.com/download/pr/7afca223-55d2-470a-8edc-6a1739ae3252/abd170b4b0ec15ad0222a809b761a036/ndp48-x86-x64-allos-enu.exe `
    && .\dotnet-framework-installer.exe /q `
    && del .\dotnet-framework-installer.exe `
    && powershell Remove-Item -Force -Recurse ${Env:TEMP}\*

# Apply latest patch
RUN curl -fSLo patch.msu http://download.windowsupdate.com/c/msdownload/update/software/updt/2019/09/windows10.0-kb4515843-x64_181da0224818b03254ff48178c3cd7f73501c9db.msu `
    && mkdir patch `
    && expand patch.msu patch -F:* `
    && del /F /Q patch.msu `
    && DISM /Online /Quiet /Add-Package /PackagePath:C:\patch\Windows10.0-kb4515843-x64.cab `
    && rmdir /S /Q patch

# ngen .NET Fx
ENV COMPLUS_NGenProtectedProcess_FeatureEnabled 0
RUN \Windows\Microsoft.NET\Framework64\v4.0.30319\ngen uninstall "Microsoft.Tpm.Commands, Version=10.0.0.0, Culture=Neutral, PublicKeyToken=31bf3856ad364e35, processorArchitecture=amd64" `
    && \Windows\Microsoft.NET\Framework64\v4.0.30319\ngen update `
    && \Windows\Microsoft.NET\Framework\v4.0.30319\ngen update
```

Lets's dive into what the Dockerfile is actually doing. The first `FROM` line pulls Windows Server Core 2019, which includes .NET Framework 4.7.2. The following `RUN` line then installs .NET Framework 4.8 on top. The middle `RUN` line services .NET Framework 4.8 with the latest patches. The last `RUN` line runs the `ngen` tool to create or update NGEN images, if needed. Many files are being updated multiple times with this series of commands. Each time a file is updated, the size of the image increases by the size of the new "duplicate" file.

In the worst case scenario, four copies of many files are created, and that doesn't account for the fact that each file has IL and NGEN variants, for x86 and x64. The size explosion starts to become apparent and is hard to fully grasp without a full accounting in a spreadsheet.

Stepping back, not all file updates are equal. For example, you can (in theory) update a 1 KB text file 500 times before it will have the same impact of updating a 500 KB file once. We found that NGEN image files were the worst offender. NGEN images are generated by `ngen.exe` (which you see used in the Dockerfile) to improve startup performance. They are also big, typically 3x larger than their associated IL files. It quickly became clear that NGEN images were going to be a primary target for a solution.

## Designing a container-friendly approach

Architecturally, we had three design characteristics that we wanted in a solution:

- There should be a single copy of each file in the .NET Framework, across all container image layers published by Microsoft.
- NGEN images that are created by default should align with default use cases.
- Maintain startup performance as container image size is reduced.

The biggest risk was the last characteristic, about maintaining startup performance, given that our primary startup performance lever -- NGEN -- was the primary target for reducing container image size. You already know how the story ends from the introduction, but let's keep digging, and look at what we did in preparation for Windows Server Core 20H1 images (what is in Insiders now).

Here's what we did in the Windows Server Core base image layer:

- Include a serviced copy of .NET Framework 4.8.
- Remove all NGEN images, except for the three most critical ones, for both x86 and x64. These images are for mscorlib.dll, System.dll and System.Core.dll.

Here's what we did in the .NET Framework runtime image layer:

- NGEN assemblies used by Windows PowerShell and ASP.NET (and no more).
- NGEN only 64-bit assemblies. The only 32-bit NGEN images are the three included in the Server Core base image.

You can see these changes in the [Dockerfile for .NET Framework 4.8 on the Windows Server Core Insider image](https://github.com/microsoft/dotnet-framework-docker/blob/windowserver-2004/4.8/runtime/windowsservercore-2004/Dockerfile), as follows:

```Dockerfile
# escape=`

FROM mcr.microsoft.com/windows/servercore/insider:10.0.19023.1

# Enable detection of running in a container
ENV DOTNET_RUNNING_IN_CONTAINER=true

RUN `
    # Ngen top of assembly graph to optimize a set of frequently used assemblies
    \Windows\Microsoft.NET\Framework64\v4.0.30319\ngen install "Microsoft.PowerShell.Utility.Activities, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    # To optimize 32-bit assemblies, uncomment the next line and add an escape character (`) to the end of the previous line
    # && \Windows\Microsoft.NET\Framework\v4.0.30319\ngen install "Microsoft.PowerShell.Utility.Activities, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
```

This Dockerfile is much simpler, but we'll still take a deeper look. The `FROM` statement  pulls the Windows Server Core Insider base image layer, which already contains the (serviced) version of the .NET Framework we want. This is why there are no subsequent `RUN` statements that download and install later or serviced .NET Framework versions. The single `RUN` statement uses `ngen` to pre-compile a curated set of assemblies that we expect will benefit most .NET applications, but only for the 64-bit version of .NET Framework.

This much more streamlined approach has the following key benefits:

- The Windows Server Core base image is now a lot smaller, and will be a massive benefit for Windows applications that don't use .NET Framework. They still contain the .NET Framework but only a much smaller set of the NGEN images as compared to the 1903 base image.
- The .NET Framework container image is also significantly smaller because it is now constructed in a way that much better aligns with the way Docker layers and images work and it contains a much smaller and curated set of NGEN images.

In terms of guidance, this new approach means that you should strongly prefer the .NET Framework runtime (or SDK) image if you are using Windows PowerShell or containerizing a .NET Framework application. It also means that it makes more sense to customize NGEN in your own Dockerfiles since the images Microsoft produces have  much fewer NGEN images to start with.

Looking back at the new .NET Framework runtime Dockerfile, you can see that the last line is commented, which would otherwise NGEN assemblies for the 32-bit .NET Framework. You should consider uncommenting this line if you run 32-bit .NET Framework applications. You would need to either copy this line to your application Dockerfile (typically as the first line after the FROM statement) or use this Dockerfile as an alternative to using the .NET Framework runtime image.

If you use your own version of this Dockerfile, then you can customize it further. For example, you could target a smaller or different set of assemblies that are specifically chosen for only your application.

## Performance of the new approach

I'm finally going to share the performance numbers! I'll explain a few more things first, to make sure you've got the right context.

Like I said earlier, our primary goal wasn't to improve the startup time of PowerShell or ASP.NET, but maintain it as we reduced the size of container images. Turns out that we did better than that, but let me ignore our achievements for a moment to make a point. If you are not familar with containers, it may not be obvious how valuable achieving that goal really is. I'll explain it.

For many scenarios, image size ends up being a dominant startup cost because images need to be pulled across a network as part of `docker run`. This is the case for scenarios where the container environment doesn't maintain an image cache (more common than you might think).

If the value here still isn't popping, I'll try an anology (I really love analogies). Imagine you and I are racing two cars on a track. I'm racing a white one with a red maple leaf ... OK, OK, the color doesn't matter! The gun goes off and fans are expecting us to start tearing down the track. Instead of hitting the gas pedals, we jump out of the cars and first fill up our gas tanks, then jump back in and finally start moving forward to do the job we were paid to do (race the cars!). That's basically what `docker run` has to do if you don't have a local copy of an image.

With this improvement in place, we still have to jump out of the cars when the gun goes up, but your car tank is now half the size it was before, so filling it is much quicker, HOWEVER the car still goes the same speed and distance. Unfortunately for me, you win the race because I'm stuck with the older version of the car! Unlucky me.

I'm going to stretch this analogy a little further. I said that your tank fills up in half the time now, but still goes the same speed and distance. It turns out that we managed to make the car go faster, too, and it can still go just as far. Sounds awesome! That's basically the same thing we achieved.

OK, back to reality ... let's look at the actual results we saw, as measured in our performance lab.

|            | 1903 | 1903-FX | Insider | Insider-FX|
| ---------- | ---- | ------- | ---- | ------ |
| Size compressed (GB)  | 2.11 | 2.18 | 1.13 | 1.19 |
| Size uncompressed (GB)  | 5.03 | 5.29 | 2.63 | 2.89 |
| Container launch (s) | 6.7 | 6.67 | 4.68 | 3.61 |
| PowerShell launch (s) | 0.64 | 0.13 | 0.73 | 0.15 |

Legend:

* **1903:** mcr.microsoft.com/windows/servercore:1903
* **1903-FX:** mcr.microsoft.com/dotnet/framework/runtime:4.8-20191008-windowsservercore-1903
* **Insider:** mcr.microsoft.com/windows/servercore/insider:10.0.19023.1
* **Insider-FX:** image built from [runtime Dockerfile](https://github.com/microsoft/dotnet-framework-docker/blob/windowserver-2004/4.8/runtime/windowsservercore-2004/Dockerfile)
* **Size compressed (GB)** -- this is the size of an image, in gigabytes, within a registry and when you pull it (AKA "over the wire").
* **Size uncompressed (GB)** -- this is the size of an image, in gigabytes, on disk after you pull it. It is uncompressed so that it is fast to run.
* **Container launch (s)** -- This is the time it takes, in seconds, to launch a container, into PowerShell. It is equivalent to: `docker run --rm  DOCKERIMAGE powershell exit`.
* **PowerShell launch (s)** -- This the time it takes, in seconds, to launch PowerShell within a running container. It is equivalent to: `powershell exit`.

I'll give you the value-oriented summary of what those numbers are actually telling us.

For the **Windows Server Core Insider base image**:

* The compressed Insider image is 46% smaller than the 1903 base image.
* The uncompressed Insider image is 47% smaller than the 1903 base image.
* Container startup into Windows PowerShell is 30% faster, when using the Insider image compared to the 1903 base image.
* Windows PowerShell startup within a running container is slower with the Insider image than the 1903 base image, by 100ms (15%) on our hardware.

For the **.NET Framework runtime image**, based on the new Windows Server Core Insider base image:

* The compressed .NET Framework runtime image is 45% smaller than the 1903 runtime image.
* The uncompressed .NET Framework runtime image is 45% smaller than the 1903 runtime image.
* Container startup into Windows PowerShell is 45% faster, using the .NET Framework runtime image compared to the 1903 runtime image.
* Windows PowerShell startup within a running container is slower with the Insider-based runtime image than the 1903 runtime image, by 20ms (15%) on our hardware.
* We specifically measured the benefit of not including 32-bit images in the runtime image. It is 70MB in the compressed image and 300 MB in the uncompressed image.

Note: We expect these numbers to change before the Windows Server 20H1 release, either a little better or a little worse, but not far off what I've described here.

If you are interested in the details or reproducing these numbers yourself, the following list details the measurements we made and some of our methodology.

* Size compressed: [Retrieving Docker Image Sizes
](https://gist.github.com/MichaelSimons/fb588539dcefd9b5fdf45ba04c302db6)
* Size uncompressed: `docker images`
* Container launch (run from the host, in PowerShell): 
    ```powershell
    $a = @(); 1..5 | % { $a += (measure-command { docker run --rm  DOCKERIMAGE powershell exit }).TotalSeconds }; $a
    ```
* PowerShell launch (run from inside the container, in PowerShell): 
    ```powershell
    $a = @(); 1..5 | % { $a += (measure-command { powershell exit } ).TotalSeconds } ; $a
    ```

Note: All launch measurements listed are the average of the middle 3 of 5 test runs.

PowerShell launch is run from within PowerShell. This approach could be viewed as a weak test methodology. Instead, it is a practical pattern for what we are measuring, which is the reduction of JIT time. The second PowerShell instance will be in a second process. There is some benefit from launching PowerShell from PowerShell because read-only pages will be shared across processes. JITed code is written to read-write pages, which are not shared across process boundaries, such that the actual code execution of PowerShell will be unique in both processes and sensitive to the need to JIT at startup. As a result, the difference in startup numbers is primarily due to the reduction in JIT compilation required during startup. That also explains why we are only measuring `powershell exit` (we are only targeting startup for the scenario). Feel free to measure this and other scenarios and give us your feedback. We'd appreciate that.

We haven't yet started measuring the performance improvement to the [.NET Framework SDK image](https://hub.docker.com/_/microsoft-dotnet-framework-sdk/). We expect to see size and container startup improvements for that image, too. You can see an early version of the [.NET Framework SDK image Dockerfile](https://github.com/microsoft/dotnet-framework-docker/blob/windowserver-2004/4.8/sdk/windowsservercore-2004/Dockerfile) that you can see and test.

## Forward-looking Guidance

Starting with the next version of Windows Server, we have the following guidance for Windows container users:

* If you are using .NET Framework applications with Windows containers, including Windows PowerShell, use a [.NET Framework image](https://hub.docker.com/_/microsoft-dotnet-framework).
* If you are not using .NET, use the [Windows Server Core base image](https://hub.docker.com/_/microsoft-windows-servercore), or another image derived from it.
* If you need better startup performance than the .NET Framework runtime image has to offer, we recommend creating your own images with your own profile of NGEN images. This is considered a supported scenario, and doesn't disqualify you from getting support from Microsoft.

## Closing

A lot of our effort on Docker containers has been [focused on .NET Core](https://devblogs.microsoft.com/dotnet/using-net-and-docker-together-dockercon-2019-update/), however, we have been looking for opportunities to improve the experience for .NET Framework users as well. This post describes such an improvement. Please tell us about other pain points for using .NET Framework in containers. We'd be interested in talking with you if you are using .NET Framework containers in production to learn more about what is working well and what isn't.

Please give us feedback as you start adopting the new Windows Server Core container images. We intend to produce .NET Framework images for the next version of Windows Server Core as soon as 20H1 images are available in the [Windows Docker repo](https://hub.docker.com/_/microsoft-windows-servercore).
