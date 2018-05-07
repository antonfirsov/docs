# Announcing .NET Core 2.1 RC 1

Today, we're announcing .NET Core 2.1 Release Candidate 1 (RC 1). The .NET Core 2.1 RC 1 is now ready for broad testing and for production use. Our quality, reliability, and performance testing give us confidence that the release is ready for the first set of production users. On the metrics that we can measure, .NET Core 2.1 is a large step forward from .NET Core 2.0.

ASP.NET Core 2.1 RC 1 and Entity Framework 2.1 RC 1 are also releasing today.

You can download and get started with .NET Core 2.1 RC 1, on Windows, macOS, and Linux:

* [.NET Core 2.1 RC 1 SDK](https://www.microsoft.com/net/download/dotnet-core/sdk-2.1.300-rc1) (includes the runtime)
* [.NET Core 2.1 RC 1 Runtime](https://www.microsoft.com/net/download/dotnet-core/runtime-2.1.0-rc1)

You can see complete details of the release in the [.NET Core 2.1 RC 1 release notes](https://github.com/dotnet/core/blob/master/release-notes/2.1/preview/2.1.0-rc1.md). Related instructions, known issues, and workarounds are included in releases notes.

You can develop .NET Core 2.1 apps with Visual Studio 2017 15.7, Visual Studio for Mac 7.5, or Visual Studio Code.

At this point in the release, the feature set and performance characteristics are not changing much. Look at [Announcing .NET Core 2.1 Preview 2](https://blogs.msdn.microsoft.com/dotnet/2018/04/11/announcing-net-core-2-1-preview-2/) and [Performance Improvements in .NET Core 2.1](https://blogs.msdn.microsoft.com/dotnet/2018/04/18/performance-improvements-in-net-core-2-1/) to learn about improvements in the release that are not covered in this post.

## "Go Live" Support

NET Core 2.1 RC is supported by Microsoft and can be used in production. As always, we recommend that you test your app before deploying to production. If anything seems strange, don't deploy! If all of your tests pass with .NET Core 2.1 RC and you are enthusiastic to start using the 2.1 benefits, then go ahead and deploy. Tell us about your experiences.

Another option is to adopt the .NET Core 2.1 RC SDK while continuing to target earlier .NET Core releases, like 2.0. We believe that many people do this.

## Alpine Support

[Alpine Linux](https://www.alpinelinux.org/) is a very small and security-focused Linux distro. It's also quickly becoming the [favored base image for Docker](https://hub.docker.com/r/library/alpine/). We added support for it as a preview in 2.0 after many requests for it. Since then, the requests for it have only increased.

We are adding official support for Alpine starting with this RC release. We intend to switch to Alpine 3.8 as the version we test on and support as soon as Alpine 3.8 releases (guess: June 2018).

If you want to use [.NET Core and Alpine with Docker](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/README.md#build-and-run-the-sample-for-alpine-with-docker), use the following tags:

* 2.1-sdk-alpine
* 2.1-runtime-alpine

If you are using the 2.0 Alpine images at [microsoft/dotnet](https://hub.docker.com/r/microsoft/dotnet/), switch to 2.1. We will not update the 2.0 Alpine images much longer, given that they remain in preview.

The runtime ID for Alpine was previously `alpine-3.6`. There is a now a [more generic runtime ID](https://github.com/dotnet/corefx/pull/29295/files) for Alpine and similar distros, called `linux-musl`, to support any Linux distro that uses [musl libc](https://www.musl-libc.org/). All of the other runtime IDs assume [glibc](https://www.gnu.org/software/libc/).

## ARM Support

We've had many requests for ARM, particularly for running on the [Raspberry Pi](https://www.raspberrypi.org/blog/raspberry-pi-3-model-bplus-sale-now-35/). .NET Core is now supported on Linux ARM32 distros, like [Raspbian](https://www.raspberrypi.org/downloads/) and [Ubuntu](https://www.ubuntu.com/download/server/arm). The same download links at the start of the post provide ARM downloads, too.

Note: .NET Core 2.1 is supported on Raspberry Pi 2+. It isn't supported on the Pi Zero or other devices that use an ARMv6 chip. .NET Core requires ARMv7 or ARMv8 chips, like the [ARM Cortex-A53](https://en.wikipedia.org/wiki/ARM_Cortex-A53).

If you want to use [.NET Core on ARM32 with Docker](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/dotnet-docker-arm32.md), you can use any of the following tags:

* 2.1-sdk
* 2.1-runtime
* 2.1-aspnetcore-runtime
* 2.1-sdk-stretch-slim-arm32v7
* 2.1-runtime-stretch-slim-arm32v7
* 2.1-aspnetcore-runtime-stretch-slim-arm32v7
* 2.1-sdk-bionic-arm32v7
* 2.1-runtime-bionic-arm32v7
* 2.1-aspnetcore-runtime-bionic-arm32v7

Note: The first three tags are [multi-arch](https://github.com/dotnet/announcements/issues/14).

If you are using the 2.0 ARM32 images at [microsoft/dotnet](https://hub.docker.com/r/microsoft/dotnet/), switch to 2.1. We will not update the 2.0 ARM32 images much longer, given that they remain in preview.

Our friends on the [Azure IoT Edge](https://github.com/Azure/iot-edge) team use the .NET Core Bionic ARM32 Docker images to support developers [writing C# with Edge devices](https://github.com/Azure/iot-edge/blob/master/v2/samples/azureiotedge-simulated-temperature-sensor/README.md).

If you are new to Raspberry Pi, I suggest the [awesome Pi resources at AdaFruit](https://learn.adafruit.com/category/raspberry-pi). You can [buy a Pi](https://www.adafruit.com/category/105) there, too.

We are hearing requests for ARM64. We are [working on it](https://github.com/dotnet/dotnet-docker/pull/509). For now, our best recommendation is to use our ARM32 build on ARM64. Many ARM64 chips (AKA "ARMv8") support ARM32 instructions (AKA "ARMv7"). Raspberry Pi 3+ devices have such a chip. I installed an experimental version of [ARM64 Debian](https://wiki.debian.org/RaspberryPi3) on a Pi 3 for this purpose. It works. We'll update you when our ARM64 support improves.

Major thanks to Samsung and Qualcomm for investing heavily on .NET Core ARM32 and ARM64 implementations. Please thank them, too! These contributions speak to the value of open-source.

## Docker Images

.NET Core and ASP.NET Core images have been updated for .NET Core 2.1 RC at [microsoft/dotnet](https://hub.docker.com/r/microsoft/dotnet). The [samples repo](https://hub.docker.com/r/microsoft/dotnet-samples/) has also been updated for 2.1 RC.

You can quickly try .NET Core 2.1 RC with one of our pre-built sample images:

**Console app:**

```console
docker pull microsoft/dotnet-samples:dotnetapp
docker run --rm microsoft/dotnet-samples:dotnetapp
```

**ASP.NET Core app**

```console
docker pull microsoft/dotnet-samples:aspnetapp
docker run --rm -it -p 8000:80 --name aspnetcore_sample microsoft/dotnet-samples:aspnetapp
```

You can view the site at `http://localhost:8000` on most machines. If that doesn't work check out [View ASP.NET Core app in a running container on Windows](https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/README.md#view-the-aspnet-core-app-in-a-running-container-on-windows).

## Brotli Compression

[Brotli](https://en.wikipedia.org/wiki/Brotli) is a general-purpose lossless compression algorithm that compresses data comparable to the best currently available general-purpose compression methods. It is similar in speed to deflate but offers more dense compression. The specification of the Brotli Compressed Data Format is defined in [RFC 7932](https://www.ietf.org/rfc/rfc7932.txt). The Brotli encoding is supported by most web browsers, major web servers, and some CDNs (Content Delivery Networks). The .NET Core Brotli implementation is based around the c code provided by Google at [google/brotli](https://github.com/google/brotli). Thanks, Google!

[Brotli support](https://github.com/dotnet/corefx/issues/25785) has been added to .NET Core 2.1. Operations may be completed using either the stream-based [BrotliStream](https://docs.microsoft.com/dotnet/api/system.io.compression.brotlistream?view=netcore-2.1) or the high-performance span-based [BrotliEncoder](https://docs.microsoft.com/dotnet/api/system.io.compression.brotliencoder?view=netcore-2.1)/[BrotliDecoder](https://docs.microsoft.com/dotnet/api/system.io.compression.brotlidecoder?view=netcore-2.1) classes.

The [BrotliStream](https://docs.microsoft.com/dotnet/api/system.io.compression.brotlistream?view=netcore-2.1) behavior is the same as that of [DeflateStream](https://docs.microsoft.com/dotnet/api/system.io.compression.deflatestream?view=netcore-2.1) or [GZipStream](https://docs.microsoft.com/dotnet/api/system.io.compression.gzipstream?view=netcore-2.1) to allow easily converting DeflateStream/GZipStream code to use BrotliStream.

## New Cryptography APIs

The following enhancements have been made to .NET Core cryptography APIs:

* **New SignedCms APIs** -- [System.Security.Cryptography.Pkcs.SignedCms](https://docs.microsoft.com/dotnet/api/system.security.cryptography.pkcs.signedcms?view=netcore-2.1) is now available in the [System.Security.Cryptography.Pkcs](https://www.nuget.org/packages/System.Security.Cryptography.Pkcs) package.  The .NET Core implementation is available to all .NET Core platforms and has parity with the class from .NET Framework. See: [dotnet/corefx #14197](https://github.com/dotnet/corefx/issues/14197).
* **New X509Certificate.GetCertHash overload for SHA-2** -- New overloads for [X509Certificate.GetCertHash](https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.x509certificates.x509certificate.getcerthash?view=netcore-2.1) and [X509Certificate.GetCertHashString](https://docs.microsoft.com/dotnet/api/system.security.cryptography.x509certificates.x509certificate.getcerthashstring?view=netcore-2.1) accept a hash algorithm identifier to enable callers to get certificate thumbprint values using algorithms other than SHA-1. [dotnet/corefx #16493](https://github.com/dotnet/corefx/issues/16493).
* **New [Span&lt;T&gt;-based](https://docs.microsoft.com/dotnet/api/system.span-1?view=netcore-2.1) cryptography APIs** -- Span-based APIs are available for hashing, HMAC, (cryptographic) random number generation, asymmetric signature generation, asymmetric signature processing, and RSA encryption.
* **Rfc2898DeriveBytes performance improvements** -- The implementation of [Rfc2898DeriveBytes](https://docs.microsoft.com/dotnet/api/system.security.cryptography.rfc2898derivebytes?view=netcore-2.1) (PBKDF2) is about 15% faster, based on using [Span&lt;T&gt;-based](https://docs.microsoft.com/dotnet/api/system.span-1?view=netcore-2.1). Users who benchmarked an iteration count for an amount of server time may want to update iteration count accordingly.
* **Added CryptographicOperations class** -- [CryptographicOperations.FixedTimeEquals](https://docs.microsoft.com/dotnet/api/system.security.cryptography.cryptographicoperations.fixedtimeequals?view=netcore-2.1) takes a fixed amount of time to return for any two inputs of the same length, making it suitable for use in cryptographic verification to avoid contributing to timing side-channel information.  [CryptographicOperations.ZeroMemory](https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.cryptographicoperations.zeromemory?view=netcore-2.1) is a memory clearing routine that cannot be optimized away via a write-without-subsequent-read optimization.
* **Added static RandomNumberGenerator.Fill** -- The static [RandomNumberGenerator.Fill](https://docs.microsoft.com/dotnet/api/system.security.cryptography.randomnumbergenerator.fill?view=netcore-2.1) will fill a Span with random values using the system-preferred CSPRNG, and does not require the caller to manage the lifetime of an IDisposable resource.
* **Added support for RFC 3161 cryptographic timestamps** -- New API to request, read, validate, and create TimestampToken values as defined by [RFC 3161](https://www.ietf.org/rfc/rfc3161.txt).
* **Add Unix EnvelopedCms** -- The EnvelopedCms class has been added for Linux and macOS.
* **Added ECDiffieHellman** -- Elliptic-Curve Diffie-Hellman (ECDH) is now available on .NET Core via the [ECDiffieHellman](https://docs.microsoft.com/dotnet/api/system.security.cryptography.ecdiffiehellman?view=netcore-2.1) class family with the same surface area as .NET Framework 4.7.
* **Added RSA-OAEP-SHA2 and RSA-PSS to Unix platforms** -- Starting with .NET Core 2.1 the instance provided by [RSA.Create()](https://docs.microsoft.com/dotnet/api/system.security.cryptography.rsa.create?view=netcore-2.1) can always encrypt or decrypt with OAEP using a SHA-2 digest, as well as generate or validate signatures using RSA-PSS

## .NET Core Global Tools

The .NET Core global tools feature has stabilized. The syntax that was updated in Preview 2 hasn't changed. You can try a pre-built global tool with the following example:

```console
dotnet tool install -g dotnetsay
dotnetsay
```

For global tool users, we recommend that you uninstall global tools that you installed with previous releases or just delete the `.dotnet/tools` and `dotnet/toolspkgs` directories in your user profile. The layout of the directory has changed, requiring tools to be reinstalled.

For folks that built and published tools for .NET Core Preview 1 or Preview 2, you need to rebuild and republish them with .NET Core 2.1 RC. You will need to do the same thing when we publish RTM. .NET Core applications, including tools, do not roll-forward across preview versions.

Note that .NET Core Global tools must target `netcoreapp2.1`. See the [.NET Core 2.1 RC release notes](https://github.com/dotnet/core/blob/master/release-notes/2.1/preview/2.1.0-rc1.md) for more information on Global Tools updates.

## SourceLink

[SourceLink](https://github.com/dotnet/sourcelink) is a system that enables a source debugging experiences for binaries that you either distribute or consume. It requires producers of SourceLink information and debuggers that support it. The Visual Studio debugger already supports SourceLink, starting with Visual Studio 2017 15.3. We have added support for generating SourceLink information in symbols, binaries, and NuGet packages in the .NET Core 2.1 RC SDK.

You can start producing SourceLink information by following the example at [dotnet/sourcelink](https://github.com/dotnet/sourcelink).

Our goal for the project is to enable anyone building [NuGet libraries to provide source debugging](https://github.com/dotnet/designs/blob/master/accepted/diagnostics/debugging-with-symbols-and-sources.md) for their users with almost no effort. There are a few steps left to enable the full experience, but you can get started now.

The following screenshot demonstrates debugging a NuGet package referenced by an application, with source automatically downloaded from GitHub and used by Visual Studio 2017.

![sourcelink-example](https://user-images.githubusercontent.com/2608468/39667937-10d7dabe-5076-11e8-815e-935724b3a783.PNG)

## Tiered Compilation

We've added a preview of a new and exciting capability to the runtime called tiered compilation. It's a way for the runtime to more adaptively use the Just-In-Time (JIT) compiler to get better performance.

The basic challenge for JIT compilers is that compilation time is part of the application's execution time. Producing better code usually means spending more time optimizing it. But if a given piece of code only executes once or just a few times, the compiler might spend more time optimizing it than the application would spend just running an unoptimized version.

With tiered compilation, the compiler first generates code as quickly as possible, with only minimal optimizations (first tier). Then, when it detects that certain methods are executed a lot, it produces a more optimized version of those methods (second tier) that are then used instead. The second tier compilation is performed in parallel, which removes the tension between fast compile speeds and producing optimal code. This model can be more generically called [Adaptive optimization](https://en.wikipedia.org/wiki/Adaptive_optimization).

Tiered compilation is also beneficial for long-running applications, such as web servers. We'll go into more detail in follow-on posts, but the short version is that the JIT can produce much better code than is in the pre-compiled assemblies we ship for .NET Core itself. This is mostly due to the [fragile binary interface problem](https://en.wikipedia.org/wiki/Fragile_binary_interface_problem). With tiered compilation, the JIT can use the pre-compiled code it finds for .NET Core and then JIT-compile better code for methods that get called a lot. We've seen this scenario having a large impact for the tests in our performance lab.

You can test tiered compilation with your .NET Core 2.1 RC application by setting an environment variable:

```console
COMPlus_TieredCompilation="1"
```

In the final version of .NET Core 2.1, you will be able to opt in with the [`System.Runtime.TieredCompilation` app-context switch](https://github.com/dotnet/coreclr/pull/17840). We aim to make tiered compilation the default in the future after we've had a chance to gather feedback and make any necessary improvements. We will publish more details about this new feature shortly.

## Closing

.NET Core 2.1 RC 1 is our third release on the way to the final version of 2.1. The quality is high enough now that we're happy for you to start using .NET Core 2.1 in production. We're heard a lot of positive feedback so far from folks who have tried  out the RC. We hope you have the same experience. Please share your experience with us.

We're now very close to being done with the 2.1 release. We expect to ship the final version of 2.1 in the first half of 2018. Thanks to everyone that has helped along the way.
