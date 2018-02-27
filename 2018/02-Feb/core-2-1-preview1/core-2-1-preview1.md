# Announcing .NET Core 2.1 Preview 1

Today, we are announcing .NET Core 2.1 Preview 1. It is the first public release of .NET Core 2.1. We have great improvements that we want to share and that we would love to get your feedback on.

ASP.NET Core 2.1 Preview 1 and Entity Framework 2.1 Preview 1 are also releasing today.

You can download and get started with .NET Core 2.1 Preview 1, on Windows, macOS, and Linux:

* [.NET Core 2.0 Preview 1 SDK](https://www.microsoft.com/net/download/dotnet-core/sdk-2.1.300-preview1)
* [.NET Core 2.0 Preview 1 Runtime](https://www.microsoft.com/net/download/dotnet-core/runtime-2.1.0-preview1)

You can develop .NET Core 2.1 apps with [Visual Studio 2017 15.6 Preview 6](https://www.visualstudio.com/vs/preview/) or later, or Visual Studio Code. We expect that Visual Studio for Mac support will be added with Visual Studio 2017 15.7.

ASP.NET Core 2.1 Previews will not be auto-deployed to Azure App Service. Instead, you can opt in to using previews with just a little bit of configuration. See [some page]() for more info.

Visual Studio Team Service support for .NET Core 2.1 will come closer to RTM.

You can see complete details of the release in the .NET Core 2.1 Preview 1 release notes. Known issues and workarounds are included in the releases notes.

To everyone that helped with the release, thank you very much. We couldn’t have gotten to this spot without you and we’ll continue to need your help as we work together towards .NET Core 2.1 RTM.

Let's look at the many improvements that are part of the .NET Core 2.1 Preview 1 release. As big a release as [.NET Core 2.0](https://blogs.msdn.microsoft.com/dotnet/2017/08/14/announcing-net-core-2-0/) was, you should find multiple improvements that will want to make you upgrade.

## Themes

The .NET Core 2.1 release, across .NET Core, ASP.NET Core and EF Core is intended to improve the product across the following themes:

* Faster Build Performance
* Close gaps in ASP.NET Core and EF Core
* Improve compatibility with .NET Framework
* GDPR and Security
* Microservices and Azure
* More capable Engineering System

Some improvements for these themes are not yet in Preview 1. These themes are guiding our investments across the .NET Core 2.1 release.

## Global Tools

.NET Core now has a new deployment and extensibility mechanism for tools. This new experience is very similar to and was inspired by [NPM global tools](https://docs.npmjs.com/getting-started/installing-npm-packages-globally).

You can try out .NET Core Global Tools for yourself (after you've installed .NET Core 2.1 Preview 1) with a sample tool called [dotnetsay](https://www.nuget.org/packages/dotnetsay/) that we published, using the following commands:

```console
dotnet install tool -g dotnetsay
dotnetsay
```

```html
<script src="https://gist.github.com/richlander/84526accec70af4284b3a3c2efec1be8.js"></script>
```

Once you install `dotnetsay`, you can then use it directly by just typing `dotnetsay` in your command prompt or terminal. You can close terminal sessions, switch drives in the terminal, or reboot your machine and the command will still be there. `dotnetsay` is now in your path. Check `%USERPROFILE&\.dotnet\tools` or `~\.dotnet\tools` (as appropriate) to see the tools installation location on your machine.

You can create your own global tools by reading more about them in the [.NET Core 2.1 Release Notes]().

.NET Core tools are .NET Core console apps that are packaged and acquired as NuGet packages. By default, these tools are [framework-dependent applications](https://docs.microsoft.com/dotnet/core/deploying/) and [include all of their NuGet dependencies](https://github.com/dotnet/cli/issues/8656). This means that a given global tool will run on any operating system or chip architecture by default. You might need an existing tool on a new version of Linux. As long as .NET Core works there, you should be able to run the tool.

At present, .NET Core Tools only support global install and require the `-g` argument to be installed. We're working on various forms of local install, too, but those are not ready in preview 1.

We expect a whole new ecosystem of tools to establish itself for .NET. Some of these tools will be  specific to .NET Core development and many of them will be general in nature. The tools are deployed to NuGet feeds. By default, the `dotnet tool install` command looks for tools on NuGet.org.

If you are curious about dotnetsay, it is modeled after [docker/whalesay](https://hub.docker.com/r/docker/whalesay/), which is modeled after [cowsay](https://en.wikipedia.org/wiki/cowsay). dotnetsay is really just [dotnet-bot](https://github.com/dotnet-bot), who is one of our busiest developers!

## Build Performance Improvements

Build-time performance is much improved in .NET Core 2.1, particularly for incremental builds. These improvements apply to both `dotnet build` on the commandline and to builds in Visual Studio.  We've made improvements in the CLI tools and in MSBuild in order to make the tools deliver a much faster experience.

The following chart provides concrete numbers on the improvements that you can expect from the new release. You can see two different workloads with numbers for .NET Core 2.0, .NET Core 2.1 Preview 1 and where we aspire to land for .NET Core RTW.

![Incremental Build Improvements for .NET Core 2.1 SDK](incremental-build-core-2-1-sdk.png)

You can try building the same code yourself at [mikeharder/dotnet-cli-perf](https://github.com/mikeharder/dotnet-cli-perf/tree/master/scenarios). We'd be curious if you get similar results. Note that the improvements are for incremental builds, so you'll only see benefits after the 2nd build.

## Minor-Version Roll-forward

You will now be able to run .NET Core applications on later runtime versions, within the same major version range. For example, you will be able to run .NET Core 2.0 applications on .NET Core 2.1 or .NET Core 2.1 applications on .NET Core 2.5 (if we ever ship such a version). The roll-forward behavior is for minor versions only. For example, a .NET Core 2.x application will never automatically roll forward to .NET Core 3.0 or later.

If the expected .NET Core version is available, it is used. The roll-forward behavior is only relevant if the expected .NET Core version is not available in a given environment.

## Sockets Performance and HTTP Managed Handler

We made major improvements to sockets in .NET Core 2.1. Sockets are the basis of both outgoing and incoming networking communication. In .NET Core 2.0, the ASP.NET Kestrel Web Server and HttpClient use native sockets, not the .NET [Socket class](https://docs.microsoft.com/dotnet/api/system.net.sockets.socket). We are in the process of changing that, instead basing our higher-level networking APIs on .NET sockets.

We made three significant performance improvements for sockets (and other smaller ones) in Preview 1:

* Support for [Span&lt;T&gt;](https://docs.microsoft.com/dotnet/api/system.span-1)/Memory&lt;T&gt; in [Socket](https://docs.microsoft.com/dotnet/api/system.net.sockets.socket)/[NetworkStream](https://docs.microsoft.com/dotnet/api/system.net.sockets.networkstream)
* Improved perf on both Windows and Linux, due to a variety of fixes (e.g. reuse PreAllocatedOverlapped, cache Operation objects on Linux, improved dispatch on linux epoll notification, etc)
* Improved perf for SslStream, as well as supporting ALPN (required for HTTP2) and streamlining SslStream settings.

For HttpClient, we built a new from-the-ground-up managed [HttpClientHandler](https://docs.microsoft.com/dotnet/api/system.net.http.httpclienthandler) called [SocketHttpHandler](https://github.com/dotnet/corefx/tree/master/src/System.Net.Http/src/System/Net/Http/SocketsHttpHandler). As you can likely guess, it's a C# implementation of HttpClient based on .NET sockets and Span&lt;T&gt;.

The biggest win of SocketHttpHandler is performance. It is a lot faster than the existing implementation. There are other benefits:

* Elimination of platform dependencies on [libcurl](https://curl.haxx.se/libcurl/) (for linux) and [WinHTTP](https://msdn.microsoft.com/library/windows/desktop/aa382925.aspx) (for Windows) – simplifies both development, deployment and servicing
* Consistent behavior across platforms and platform/dependency versions.

At present, you need to enable SocketHttpHandler with an environment variable, as described at [Enabling the ManagedHandler](https://github.com/dotnet/corefx/blob/release/2.1/src/System.Net.Http/src/System/Net/Http/Managed/README.md). In Preview 2, you'll be able to `new` up the handler, as you'd expect. We are also thinking about making the new handler the default, and enable the existing native handler (based on libcurl and WinHTTP) as optional.

We are still working on moving Kestrel to use sockets. We don't have anything to share on that yet.

You might wonder how sockets improvements can be shared between incoming and outgoing scenarios, because they feel so different. It's actually pretty simple. If you're a client, you do a connect on a server.  if you're a server, you listen and wait for connections. Once that connection is established, data can flow both ways, and that's critical from a perf point of view. As a result, these socket improvements should improve both scenarios.

## Span&lt;T&gt;, Memory&lt;T&gt; and friends

We are on the verge of introducing a new set of types for using arrays and other types of memory that is much more efficient. They are included in Preview 1. Today, if you want to pass the first 1000 elements of a 10,000 element array, you need to make a copy of those 1000 elements and pass that copy to your caller. That operation is expensive in both time and space. The new Span&lt;T&gt; type enables you to provide a virtual view of that array without the time or space cost. It's also a struct, which means that there is no allocation cost either.

Span&lt;T&gt; and related types offer a uniform representation of memory from a multitude of different sources, such as arrays, stack allocation, and native code. With its slicing capabilities, it obviates the need for expensive copying and allocation in many scenarios, such as string manipulation, buffer management, etc, and provides a safe alternative to unsafe code. We expect usage of these types to start off in performance critical scenarios, but then transition to replacing arrays as the primary way of managing large blocks of data in .NET.

In terms of usage, you can create a Span&lt;T&gt; from an array:

```csharp
var arr = new byte[10];
Span<byte> bytes = arr; // Implicit cast from T[] to Span<T>
```

```html
<script src="https://gist.github.com/richlander/437976687a545e96a8665d31561dee9d.js"></script>
```

From there, you can easily and efficiently create a span to represent/point to just a subset of this array, utilizing an overload of the span’s Slice method. From there you can index into the resulting span to write and read data in the relevant portion of the original array:

```csharp
Span<byte> slicedBytes = bytes.Slice(start: 5, length: 2);
slicedBytes[0] = 42;
slicedBytes[1] = 43;
Assert.Equal(42, slicedBytes[0]);
Assert.Equal(43, slicedBytes[1]);
Assert.Equal(arr[5], slicedBytes[0]);
Assert.Equal(arr[6], slicedBytes[1]);
slicedBytes[2] = 44; // Throws IndexOutOfRangeException
bytes[2] = 45; // OK
Assert.Equal(arr[2], bytes[2]);
Assert.Equal(45, arr[2]);
```

```html
<script src="https://gist.github.com/richlander/8f307d410fb13a91398bc8fbb50c0aab.js"></script>
```

Jared Parsons gives a great introduction in his Channel 9 video [C# 7.2: Understanding Span](https://channel9.msdn.com/Events/Connect/2017/T125). Stephen Toub goes into even more detail in [C# - All About Span: Exploring a New .NET Mainstay](https://msdn.microsoft.com/en-us/magazine/mt814808.aspx).

## Windows Compatibility Pack

When you port existing code from the .NET Framework to .NET Core, you can use the [new Windows Compatibility Pack](https://blogs.msdn.microsoft.com/dotnet/2017/11/16/announcing-the-windows-compatibility-pack-for-net-core/). It provides access to an additional 20,000 APIs, compared to what is available in .NET Core. This includes System.Drawing, EventLog, WMI, Performance Counters, and Windows Services.

The following examples demonstrates accessing the Windows registry with APIs provided by the Windows Compatibility Pack.

```csharp

private static string GetLoggingPath()
{
    // Verify the code is running on Windows.
    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
    {
        using (var key = Registry.CurrentUser.OpenSubKey(@"Software\Fabrikam\AssetManagement"))
        {
            if (key?.GetValue("LoggingDirectoryPath") is string configuredPath)
                return configuredPath;
        }
    }

    // This is either not running on Windows or no logging path was configured,
    // so just use the path for non-roaming user-specific data files.
    var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    return Path.Combine(appDataPath, "Fabrikam", "AssetManagement", "Logging");
}
```

```html
<script src="https://gist.github.com/richlander/539442f699f7f45818ebe5839c66d17d.js"></script>
```

## Platform Suppport

We expect to [support the following operating system versions](https://github.com/dotnet/core/blob/master/os-lifecycle-policy.md) for .NET Core 2.1:

* Windows Client: 7, 8.1, 10 (1607+)
* Windows Server: 2008 R2 SP1+
* macOS: 10.12+
* RHEL: 7+
* Fedora: 26+
* openSUSE: 42.3+
* Debian: 8+
* Ubuntu: 14.04+
* SLES: 12+
* Alpine: 3.6+

## Docker

For Docker, we made a few changes relative to .NET Core 2.0 images at [microsoft/dotnet](https://hub.docker.com/r/microsoft/dotnet):

* Added Alpine runtime and SDK images (x64).
* Added Ubuntu Bionic/18.04, for runtime and SDK images (x64 and ARM32).
* Switched from debian:stretch to debian:stretch-slim for runtime images. (x64 and ARM32).
* Dropped debian:jessie (runtime and SDK).

These changes were based, in part, on two repeated pieces of feedback that we heard over the last six months.

* Provide smaller images, particularly for the runtime.
* Provide images with less surface area and/or more frequently updated (than Debian), from a vulnerability standpoint.

We also rewrote the [.NET Core Docker Samples](https://github.com/dotnet/dotnet-docker/blob/master/samples/README.md) instructions and code. The samples provide better instructions and implement more scenarios like unit testing and using docker registries. We hope you find the samples helpful and we plan to continue to expand them. Tell us what you'd like to see us add to make [.NET Core and Docker work better together](https://blogs.msdn.microsoft.com/dotnet/2017/05/25/using-net-and-docker-together/).

## Closing

Thanks for trying out [.NET Core 2.1 Preview 1](https://www.microsoft.com/net/download/dotnet-core/sdk-2.1.300-preview1). Please try out your existing applications with .NET Core 2.1 Preview 1. And please try out the new features that are introduced in this post. We've put a lot of effort into making them right but we need your feedback to take them across the finish line, for our final release.

Once again, thanks to everyone that contributed to the release. We appreciate all of the issues and PRs that you've contributed that have helped make this first preview release available.
