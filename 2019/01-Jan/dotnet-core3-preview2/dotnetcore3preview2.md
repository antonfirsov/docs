# Announcing .NET Core 3 Preview 2

Today, we are announcing .NET Core 3 Preview 2. It includes new features in .NET Core 3.0 and C# 8, in addition to the [large number of new features in Preview 1](https://blogs.msdn.microsoft.com/dotnet/2018/12/04/announcing-net-core-3-preview-1-and-open-sourcing-windows-desktop-frameworks/). [ASP.NET Core 3.0 Preview 2](https://blogs.msdn.microsoft.com/webdev/) and [EF Core 3.0 Preview 2](https://blogs.msdn.microsoft.com/dotnet/) are also being released today. [C# 8 Preview 2](https://blogs.msdn.microsoft.com/dotnet/) is part of .NET Core 3 SDK, and was also released last week with [Visual Studio 2019 Preview 2](https://blogs.msdn.microsoft.com/webdev/).

[Download and get started with .NET Core 3 Preview 2](https://aka.ms/netcore3download) right now on Windows, macOS and Linux.

In case you missed it, we made some big announcements with [Preview 1](https://blogs.msdn.microsoft.com/dotnet/2018/12/04/announcing-net-core-3-preview-1-and-open-sourcing-windows-desktop-frameworks/), including adding support for [Windows Forms](https://github.com/dotnet/winforms) and [WPF](https://github.com/dotnet/wpf) with .NET Core, on Windows, and that both UI frameworks will be open source. We also announced that we would support [Entity Framework 6](https://www.nuget.org/packages/EntityFramework/) on .NET Core, which will come in a later preview. ASP.NET Core is also adding many features including Razor components.

You can see complete details of the release in the [.NET Core 3 Preview 2 release notes](https://aka.ms/netcore3releasenotes).

.NET Core 3 will be supported in Visual Studio 2019, Visual Studio for Mac and Visual Studio Code. Visual Studio 2019 Preview 2 was released last week and has support for C# 8. The Visual Studio Code C# Extension (in [pre-release channel](https://github.com/OmniSharp/omnisharp-vscode/releases/tag/v1.18.0-beta5)) was also just updated to support C# 8.

## C# 8

C# 8 is a major release of the language, as Mads describes in [Do more with patterns in C# 8.0](https://blogs.msdn.microsoft.com/dotnet/2019/01/24/do-more-with-patterns-in-c-8-0/), [Take C# 8.0 for a spin](https://blogs.msdn.microsoft.com/dotnet/2018/12/05/take-c-8-0-for-a-spin/) and [Building C# 8.0](https://blogs.msdn.microsoft.com/dotnet/2018/11/12/building-c-8-0/). In this post, I'll cover a few favorites that are new in Preview 2.

### Using Declarations

Are you tired of *using statements* that require indenting your code? No more! You can now write the following code, which attaches a *using declaration* to the scope of the current statement block and then disposes the object at the end of it.  

```csharp
static void Main(string[] args)
{
    using var options = Parse(args);
    if (options["verbose"]) { WriteLine("Logging..."); }

} // options disposed here
```

### Switch Expressions

Anyone who uses C# probably loves the idea of a *switch statement*, but not the syntax. C# 8 introduces *switch expressions*, which enable the following: terser syntax, returns a value since it is an expression, and fully integrated with pattern matching. The `switch` keyword is "infix", meaning the keyword sits between the tested value (here, that's `o`) and the list of cases, much like [expression lambdas](https://docs.microsoft.com/dotnet/csharp/programming-guide/statements-expressions-operators/lambda-expressions#expression-lambdas). The following examples use the lambda syntax for methods, which integrates well with switch expressions but isn't required.

You can see the syntax for *switch expressions* in the following example:

```csharp
static string Display(object o) => o switch
{
    Point { X: 0, Y: 0 }         => "origin",
    Point { X: var x, Y: var y } => $"({x}, {y})",
    _                            => "unknown"
};
```

There are two patterns at play in this example. `o` first matches with the `Point` *type pattern* and then with the *property pattern* inside the *{curly braces}*. The `_` describes the `discard pattern`, which is the same as `default` for *switch statements*.

You can go one step further, and rely on tuple deconstruction and parameter position, as you can see in the following example:

```csharp
static State ChangeState(State current, Transition transition, bool hasKey) =>
    (current, transition) switch
    {
        (Opened, Close)              => Closed,
        (Closed, Open)               => Opened,
        (Closed, Lock)   when hasKey => Locked,
        (Locked, Unlock) when hasKey => Closed,
        _ => throw new InvalidOperationException($"Invalid transition")
    };
```

In this example, you can see you do not need to define a variable or explicit type for each of the cases. Instead, the compiler can match the tuple being testing with the tuples defined for each of the cases.

All of these patterns enable you to write declarative code that captures your intent instead of procedural code that implements tests for it. The compiler becomes responsible for implementing that boring procedural code and is guaranteed to always do it correctly.

There will still be cases where *switch statements* will be a better choice than *switch expressions* and patterns can be used with both syntax styles.

### Pointers to Unmanaged Constructed Types

You can now take a pointer to [unmanaged constructed types](https://github.com/dotnet/csharplang/issues/1744), such as `ValueTuple<int, int>`, as long as all the elements of the generic type are unmanaged. Until now, you could only take a pointer to non-generic structs with struct-fields. Given the prevalence of generic structs in more recent .NET Core releases, this change is critical to enable performant code for common types like `ValueTask<T>`, `ValueTuple<T>` and `Span<T>`.

The following are a couple of examples of opportunities in the platform where we will likely take advantage of this feature:

**Before:** [System/Runtime/Intrinsics/Vector256.cs#L110](https://github.com/dotnet/coreclr/blob/57fd77e6f8f7f2c37cc5c3b36df3ea4f302e143b/src/System.Private.CoreLib/shared/System/Runtime/Intrinsics/Vector256.cs#L110)

```csharp
Vector256<double> SoftwareFallback(double x)
{
    var pResult = stackalloc double[4]
    {
        x,
        x,
        x,
        x,
    };

    return Unsafe.AsRef<Vector256<double>>(pResult);
}
```

**After:**

```csharp
Vector256<double> SoftwareFallback(double x)
{
    var pResult = stackalloc double[4]
    {
        x,
        x,
        x,
        x,
    };

    return *(Vector256<double>*)(pResult);
}
```

**Before:** [System/Runtime/Intrinsics/Vector256.cs#L1303](https://github.com/dotnet/coreclr/blob/57fd77e6f8f7f2c37cc5c3b36df3ea4f302e143b/src/System.Private.CoreLib/shared/System/Runtime/Intrinsics/Vector256.cs#L1303)

```csharp
Vector256<short> SoftwareFallback(short x)
{
    var result = Vector256<short>.Zero;
    Unsafe.WriteUnaligned(ref Unsafe.As<Vector256<short>, byte>(ref result), value);
    return result;
}
```

**After:**

```csharp
Vector256<int> SoftwareFallback(int x)
{
    var result = Vector256<int>.Zero;
    ((int*)(&result))[0] = x;
    return result;
}
```

### Async streams

Async streams are another major improvement in C# 8. They have been changing with each preview and require that the compiler and the framework libraries match to work correctly. You need .NET Core 3.0 Preview 2 to use async streams if you want to develop with either Visual Studio 2019 Preview 2 or the latest preview of the [C# extension for Visual Studio Code](https://github.com/OmniSharp/omnisharp-vscode/releases/tag/v1.18.0-beta5). If you are using .NET Core 3.0 Preview 2 at the command line, then everything will work as expected.

## IEEE Floating-point improvements

Floating point APIs are in the process of being updated to comply with [IEEE 754-2008 revision](https://en.wikipedia.org/wiki/IEEE_754-2008_revision). The goal of [this floating point project](https://github.com/dotnet/corefx/issues/31901) is to expose all "required" operations and ensure that they are behaviorally compliant with the IEEE spec.

Parsing and formatting fixes:

* Correctly parse and round inputs of any length.
* Correctly parse and format negative zero.
* Correctly parse Infinity and NaN by performing a case-insensitive check and allowing an optional preceding `+` where applicable.

New [Math APIs](https://github.com/dotnet/corefx/issues/31903):

* `BitIncrement/BitDecrement` -- corresponds to the `nextUp` and `nextDown` IEEE operations. They return the smallest floating-point number that compares greater or lesser than the input (respectively). For example, `Math.BitIncrement(0.0)` would return `double.Epsilon`.
* `MaxMagnitude/MinMagnitude` -- corresponds to the `maxNumMag` and `minNumMag` IEEE operations, they return the value that is greater or lesser in magnitude of the two inputs (respectively). For example, `Math.MaxMagnitude(2.0, -3.0)` would return `-3.0`.
* `ILogB` -- corresponds to the `logB` IEEE operation which returns an integral value, it returns the integral base-2 log of the input parameter. This is effectively the same as `floor(log2(x))`, but done with minimal rounding error.
* `ScaleB` -- corresponds to the `scaleB` IEEE operation which takes an integral value, it returns effectively `x * pow(2, n)`, but is done with minimal rounding error.
* `Log2` -- corresponds to the `log2` IEEE operation, it returns the base-2 logarithm. It minimizes rounding error.
* `FusedMultiplyAdd` -- corresponds to the `fma` IEEE operation, it performs a fused multiply add. That is, it does `(x * y) + z` as a single operation, there-by minimizing the rounding error. An example would be `FusedMultiplyAdd(1e308, 2.0, -1e308)` which returns `1e308`. The regular `(1e308 * 2.0) - 1e308` returns `double.PositiveInfinity`.
* `CopySign` -- corresponds to the `copySign` IEEE operation, it returns the value of `x`, but with the sign of `y`.

## .NET Platform Dependent Intrinsics

We've added APIs that allow access to certain perf-oriented CPU instructions, such as the SIMD or Bit Manipulation instruction sets. These instructions can help achieve big performance improvements in certain scenarios, such as processing data efficiently in parallel. In addition to exposing the APIs for your programs to use, we have begun using these instructions to accelerate the .NET libraries too.

The following CoreCLR PRs demonstrate a few of the intrinsics, either via implementation or use:

* [Implement simple SSE2 hardware intrinsics](https://github.com/dotnet/coreclr/pull/15585)
* [Implement the SSE hardware intrinsics](https://github.com/dotnet/coreclr/pull/15538)
* [Arm64 Base HW Intrinsics](https://github.com/dotnet/coreclr/pull/16822)
* [Use TZCNT and LZCNT for Locate{First|Last}Found{Byte|Char}](https://github.com/dotnet/coreclr/pull/21073)

For more information, take a look at [.NET Platform Dependent Intrinsics](https://github.com/dotnet/designs/blob/master/accepted/platform-intrinsics.md), which defines an approach for defining this hardware infrastructure, allowing Microsoft, chip vendors or any other company or individual to define hardware/chip APIs that should be exposed to .NET code.

## Introducing a fast in-box JSON Writer & JSON Document

Following the introduction of the JSON reader in preview1, we've added `System.Text.Json.Utf8JsonWriter` and `System.Text.Json.JsonDocument`.

### Utf8JsonWriter

The `Utf8JsonWriter` provides a high-performance, non-cached, forward-only way to write UTF-8 encoded JSON text from common .NET types like `String`, `Int32`, and `DateTime`. Like the reader, the writer is a foundational, low-level type, that can be leveraged to build custom serializers. Writing a JSON payload using the new `Utf8JsonWriter` is 30-80% faster than using the writer from `Json.NET` and does not allocate.

Here is a sample usage of the `Utf8JsonWriter` that can be used as a starting point:

```C#
static int WriteJson(IBufferWriter<byte> output, long[] extraData)
{
    var json = new Utf8JsonWriter(output, state: default);

    json.WriteStartObject();

    json.WriteNumber("age", 15, escape: false);
    json.WriteString("date", DateTime.Now);
    json.WriteString("first", "John");
    json.WriteString("last", "Smith");

    json.WriteStartArray("phoneNumbers", escape: false);
    json.WriteStringValue("425-000-1212", escape: false);
    json.WriteStringValue("425-000-1213");
    json.WriteEndArray();

    json.WriteStartObject("address");
    json.WriteString("street", "1 Microsoft Way");
    json.WriteString("city", "Redmond");
    json.WriteNumber("zip", 98052);
    json.WriteEndObject();

    json.WriteStartArray("ExtraArray");
    for (var i = 0; i < extraData.Length; i++)
    {
        json.WriteNumberValue(extraData[i]);
    }
    json.WriteEndArray();

    json.WriteEndObject();

    json.Flush(isFinalBlock: true);

    return (int)json.BytesWritten;
}
```

The `Utf8JsonWriter` accepts `IBufferWriter<byte>` as the output location to synchronously write the json data into and you, as the caller, need to provide a concrete implementation. The platform does not currently include an implementation of this interface, but we plan to provide one that is backed by a resizable byte array. That implementation would enable synchronous writes, which could then be copied to any stream (either synchronously or asynchronously). If you are writing JSON over the network and include the `System.IO.Pipelines` package, you can leverage the `Pipe`-based implementation of the interface called `PipeWriter` to skip the need to copy the JSON from an intermediary buffer to the actual output.

Here's a skeleton of an array-backed concrete implementation of `IBufferWriter<T>`:

```C#
public class ArrayBufferWriter : IBufferWriter<byte>, IDisposable
{
    private byte[] _rentedBuffer;
    private int _written;

    public ArrayBufferWriter(int initialCapacity)
    {
        // TODO: argument validation

        _rentedBuffer = ArrayPool<byte>.Shared.Rent(initialCapacity);
        _written = 0;
    }

    public void Advance(int count)
    {
        // TODO: check if disposed

        // TODO: argument validation

        _written += count;
    }

    public Memory<byte> GetMemory(int sizeHint = 0)
    {
        // TODO: check if disposed

        // TODO: argument validation

        // TODO: grow/resize the buffer as needed based on your resizing strategy

        return _rentedBuffer.AsMemory(_written);
    }

    public Span<byte> GetSpan(int sizeHint = 0)
    {
        // TODO: check if disposed

        // TODO: argument validation

        // TODO: grow/resize the buffer as needed based on your resizing strategy

        return _rentedBuffer.AsSpan(_written);
    }

    public void Dispose()
    {
        // return back to the pool
    }
}
```

You can take inspiration from this [sample implementation](https://gist.github.com/ahsonkhan/c76a1cc4dc7107537c3fdc0079a68b35).

### JsonDocument

In preview2, we've also added `System.Text.Json.JsonDocument` which was built on top of the `Utf8JsonReader`. The `JsonDocument` provides the ability to parse JSON data and build a read-only Document Object Model (DOM) that can be queried to support random access and enumeration. The JSON elements that compose the data can be accessed via the `JsonElement` type which is exposed by the `JsonDocument` as a property called `RootElement`. The `JsonElement` contains the JSON array and object enumerators along with APIs to convert JSON text to common .NET types. Parsing a typical JSON payload and accessing all its members using the `JsonDocument` is 2-3x faster than `Json.NET` with very little allocations for data that is reasonably sized (i.e. < 1 MB).

Here is a sample usage of the `JsonDocument` and `JsonElement` that can be used as a starting point:

```C#
static double ParseJson()
{
    const string json = " [ { \"name\": \"John\" }, [ \"425-000-1212\", 15 ], { \"grades\": [ 90, 80, 100, 75 ] } ]";

    double average = -1;

    using (JsonDocument doc = JsonDocument.Parse(json))
    {
        JsonElement root = doc.RootElement;
        JsonElement info = root[1];

        string phoneNumber = info[0].GetString();
        int age = info[1].GetInt32();

        JsonElement grades = root[2].GetProperty("grades");

        double sum = 0;
        foreach (JsonElement grade in grades.EnumerateArray())
        {
            sum += grade.GetInt32();
        }

        int numberOfCourses = grades.GetArrayLength();
        average = sum / numberOfCourses;
    }

    return average;
}
```

As described in our [System.Text.Json roadmap](https://github.com/dotnet/corefx/blob/master/src/System.Text.Json/roadmap/README.md), we plan to provide a [POCO](https://en.wikipedia.org/wiki/Plain_old_CLR_object) serializer and deserializer next.

## GPIO Support for Raspberry Pi

We added initial support for [GPIO](https://learn.adafruit.com/adafruits-raspberry-pi-lesson-4-gpio-setup/the-gpio-connector) with Preview 1. As part of Preview 2, we have released two NuGet packages that you can use for GPIO programming.

* [System.Device.Gpio](https://www.nuget.org/packages/System.Device.Gpio/0.1.0-prerelease.19078.2)
* [Iot.Device.Bindings](https://www.nuget.org/packages/Iot.Device.Bindings/0.1.0-prerelease.19078.2)

The GPIO Packages includes APIs for GPIO, SPI, I2C and PWM devices. The IoT bindings package includes [device bindings](https://github.com/dotnet/iot/blob/master/src/devices/README.md) for various chips and sensors, the same ones available at [dotnet/iot - src/devices](https://github.com/dotnet/iot/tree/master/src/devices).

Updated serial port APIs were announced as part of Preview 1. They are not part of these packages but are available as part of the .NET Core platform.

## Local dotnet tools

Local dotnet tools have been improved in Preview 2. Local tools are similar to dotnet global tools but are associated with a particular location on disk. This enables per-project and per-repository tooling. You can read more about them [in the .NET Core 3.0 Preview 1](https://blogs.msdn.microsoft.com/dotnet/2018/12/04/announcing-net-core-3-preview-1-and-open-sourcing-windows-desktop-frameworks/) post.

In this preview, we have added 2 new commands:

* `dotnet new tool-manifest`
* `dotnet tool list`

To add local tools, you need to add a manifest that will define the tools and versions available. The `dotnet new tool-manifest` command automates creation of the manifest required by local tools. After this file is created, you can add tools to it via `dotnet tool install <packageId>`.

The command `dotnet tool list` lists local tools and their corresponding manifest. The command `dotnet tool list -g` lists global tools.

There was a change in .NET Core Local Tools between .NET Core 3.0 Preview 1 and .NET Core 3.0 Preview 2.  If you tried out local tools in Preview 1 by running a command like `dotnet tool restore` or `dotnet tool install`, you need to delete your local tools cache folder before local tools will work correctly in Preview 2. This folder is located at:

On mac, Linux: `rm -r $HOME/.dotnet/toolResolverCache`

On Windows: `rmdir /s %USERPROFILE%\.dotnet\toolResolverCache`

If you do not delete this folder, you will receive an error.

## Assembly Unloadability

Assembly unloadability is a new capability of AssemblyLoadContext. This new feature is largely transparent from an API perspective, exposed with just a few new APIs. It enables a loader context to be unloaded, releasing all memory for instantiated types, static fields and for the assembly itself. An application should be able to load and unload assemblies via this mechanism forever without experiencing a memory leak.

We expect this new capability to be used for the following scenarios:

* Plugin scenarios where dynamic plugin loading and unloading is required. 
* Dynamically compiling, running and then flushing code. Useful for web sites, scripting engines, etc.
* Loading assemblies for introspection (like ReflectionOnlyLoad), although [MetadataLoadContext](https://blogs.msdn.microsoft.com/dotnet/2018/12/04/announcing-net-core-3-preview-1-and-open-sourcing-windows-desktop-frameworks/) (released in Preview 1) will be a better choice in many cases.

More information:

* [Design doc](https://github.com/dotnet/coreclr/pull/22166)
* [Using Unloadability doc](https://github.com/dotnet/coreclr/pull/22221)

Assembly unloading requires significant care to ensure that all references to managed objects from outside a loader context are understood and managed. When the loader context is requested to be unloaded, any outside references need to have been unreferenced so that the loader context is self-consistent only to itself.

Assembly unloadability was provided in the .NET Framework by Application Domains (AppDomains), which are not supported with .NET Core. AppDomains had both benefits and limitations compared to this new model. We consider this new loader context-based model to be more flexible and higher performance when compared to AppDomains.

## Windows Native Interop

Windows offers a rich native API, in the form of flat C APIs, COM and WinRT. We've had support for P/Invoke since .NET Core 1.0, and have been adding the ability to CoCreate COM APIs and Activate WinRT APIs as part of the .NET Core 3.0 release. We have had many requests for these capabilities, so we know that they will get a lot of use.

Late last year, we announced that we had managed to [automate Excel from .NET Core](https://twitter.com/runfaster2000/status/1053704090671185920). That was a fun moment. You can now try this same demo yourself with the [Excel demo](https://github.com/dotnet/samples/tree/master/core/extensions/ExcelDemo) sample. Under the covers, this demo is using COM interop features like NOPIA, object equivalence and custom marshallers.

Managed C++ and WinRT interop are coming in a later preview. We prioritized getting COM interop built first.

## WPF and Windows Forms

The WPF and Windows Forms teams opened up their repositories, [dotnet/wpf](https://github.com/dotnet/wpf) and [dotnet/winforms](https://github.com/dotnet/winforms), respectively, on December 4th, the same day [.NET Core 3.0 Preview 1](https://blogs.msdn.microsoft.com/dotnet/2018/12/04/announcing-net-core-3-preview-1-and-open-sourcing-windows-desktop-frameworks/) was released. Much of the last month, beyond holidays, has been spent interacting with the community, merging PRs, and responding to issues. In the background, they've been integrating WPF and Windows Forms into the .NET Core build system, including adopting the [Arcade](https://github.com/dotnet/arcade) SDK. Arcade is an MSBuild SDK that exposes functionality which is needed to build the .NET Platform. The WPF team will be publishing more of the WPF source code over the coming months.

The same teams have been making a final set of changes in .NET Framework 4.8. These same changes have also been added to the .NET Core versions of WPF and Windows Forms.

## Visual Studio support

Desktop development on .NET Core 3 requires Visual Studio 2019. We added the WPF and Windows Forms templates to the New Project Dialog to make it easier starting your new applications without using the command line.

The WPF and Windows Forms designer teams are continuing to work on an updated designer for .NET Core, which will be part of a Visual Studio 2019 Update.

### MSIX Deployment for Desktop apps

[MSIX](https://docs.microsoft.com/windows/msix/) is a new Windows app package format. It can be used to deploy .NET Core 3 desktop applications to Windows 10.

The [Windows Application Packaging Project](https://docs.microsoft.com/en-us/windows/uwp/porting/desktop-to-uwp-packaging-dot-net), available in Visual Studio 2019 preview 2, allows you to create MSIX packages with [self-contained](https://docs.microsoft.com/dotnet/core/deploying/#self-contained-deployments-scd) .NET Core applications.

>Note: The .NET Core project file must specify the supported runtimes in the `<RuntimeIdentifiers>` property:
```xml
<RuntimeIdentifiers>win-x86;win-x64</RuntimeIdentifiers>
```

## Install .NET Core 3.0 Previews on Linux with Snap

Snap is the preferred way to install and try .NET Core previews on [Linux distributions that support Snap](https://docs.snapcraft.io/installing-snapd/6735).

After configuring Snap on your system, run the following command to install the [.NET Core SDK 3.0 Preview SDK](https://snapcraft.io/dotnet-sdk).

```console
sudo snap install dotnet-sdk --beta --classic
```
 
When .NET Core in installed using the Snap package, the default .NET Core command is `dotnet-sdk.dotnet`, as opposed to just `dotnet`. The benefit of the namespaced command is that it will not conflict with a globally installed .NET Core version you may have. This command can be aliased to `dotnet` with:

```console
sudo snap alias dotnet-sdk.dotnet dotnet
```

Some distros require an additional step to enable access to the SSL certificate. See our [Linux Setup](https://github.com/dotnet/core/blob/master/Documentation/linux-setup.md) for details.

## Platform Support

.NET Core 3 will be supported on the following operating systems:

* Windows Client: 7, 8.1, 10 (1607+)
* Windows Server: 20012 R2 SP1+
* macOS: 10.12+
* RHEL: 6+
* Fedora: 26+
* Ubuntu: 16.04+
* Debian: 9+
* SLES: 12+
* openSUSE: 42.3+
* Alpine: 3.8+

Chip support follows:

* x64 on Windows, macOS, and Linux
* x86 on Windows
* ARM32 on Windows and Linux
* ARM64 on Linux

For Linux, ARM32 is supported on Debian 9+ and Ubuntu 16.04+. For ARM64, it is the same as ARM32 with the addition of Alpine 3.8. These are the same versions of those distros as is supported for X64. We made a conscious decision to make supported platforms as similar as possible between X64, ARM32 and ARM64.

Docker images for .NET Core 3.0 are available at [microsoft/dotnet on Docker Hub](https://hub.docker.com/r/microsoft/dotnet/). We are in the process of adopting [Microsoft Container Registry (MCR)](https://cloudblogs.microsoft.com/opensource/2019/01/17/improved-discovery-experience-microsoft-containers-docker-hub/). We expect that the final .NET Core 3.0 images will only be published to MCR.

## Closing

Take a look at the [.NET Core 3.0 Preview 1 post ](https://blogs.msdn.microsoft.com/dotnet/2018/12/04/announcing-net-core-3-preview-1-and-open-sourcing-windows-desktop-frameworks/) if you missed that. It includes a broader description of the overall release including the initial set of features, which are also included and improved on in Preview 2.

Thanks to everyone that installed .NET Core 3.0 Preview 1. We appreciate you trying out the new version and for your feedback. Please share any feedback you have about Preview 2.
