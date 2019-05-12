# Announcing .NET Core 3.0 Preview 5

Today, we are announcing .NET Core 3.0 Preview 5. It includes a new Json serializer, support for publishing single file executables, an update to runtime roll-forward, and changes in the BCL. If you missed it, check out the improvements we released in [.NET Core 3.0 Preview 4](https://devblogs.microsoft.com/dotnet/announcing-net-core-3-preview-4/), from last month.

[Download .NET Core 3.0 Preview 5](https://dotnet.microsoft.com/download/dotnet-core/3.0) right now on Windows, macOS and Linux.

ASP.NET Core and EF Core are also releasing updates today.

## WPF and Windows Forms Update

You should see a startup performance improvement for WPF and Windows Forms. WPF and Windows Forms assemblies are now ahead-of-time compiled, with crossgen. We have seen multiple reports from the community that startup performance is significantly improved between Preview 4 and Preview 5. 

We published more code for [WPF](https://github.com/dotnet/wpf) as part of [.NET Core 3.0 Preview 4](https://devblogs.microsoft.com/dotnet/announcing-net-core-3-preview-4/). We expect to complete publishing WPF by Preview 7.

## Publishing Single EXEs

You can now publish a single-file executable with `dotnet publish`. This form of single EXE is effectively a self-extracting executable. It contains all dependencies, including native dependencies, as resources. At startup, it copies all dependencies to a temp directory, and loads them for there. It only needs to unpack dependencies once. After that, startup is fast, without any penalty.

You can enable this publishing option by adding the `PublishSingleFile` property to your project file or by adding a new switch on the commandline.

To produce a self-contained single EXE application, in this case for 64-bit Windows:

```console
dotnet publish -r win10-x64 /p:PublishSingleFile=true
```

Single EXE applications must be architecture specific. As a result, a runtime identifier must be specified.

See [Single file bundler](https://github.com/dotnet/core-setup/pull/5286) for more information.

Assembly trimmer, ahead-of-time compilation (via crossgen) and single file bundling are all new features in .NET Core 3.0 that can be used together or separately. Expect to hear more about these three features in future previews.

We expect that some of you will prefer single exe provided by an ahead-of-time compiler, as opposed to the self-extracting-executable approach that we are providing in .NET Core 3.0. The ahead-of-time compiler approach will be provided as part of the .NET 5 release.

## Introducing the JSON Serializer (and an update to the writer)

### JSON Serializer

The new JSON serializer layers on top of the high-performance `Utf8JsonReader` and `Utf8JsonWriter`. It deserializes objects from JSON and serializes objects to JSON. Memory allocations are kept minimal and includes support for reading and writing JSON with `Stream` asynchronously.

To get started, use the `JsonSerializer` class in the `System.Text.Json.Serialization` namespace. See the [documentation](https://github.com/dotnet/corefx/blob/master/src/System.Text.Json/docs/SerializerProgrammingModel.md) for information and samples. The feature set is currently being extended for future previews.

### Utf8JsonWriter Design Change

Based on feedback around usability and reliability, we made a design change to the `Utf8JsonWriter` that was added in preview2. The writer is now a regular class, rather than a ref struct, and implements `IDisposable`. This allows us to add support for writing to streams directly. Furthermore, we removed `JsonWriterState` and now the `JsonWriterOptions` need to be passed-in directly to the `Utf8JsonWriter`, which maintains its own state. To help offset the allocation, the `Utf8JsonWriter` has a new `Reset` API that lets you reset its state and re-use the writer. We also added a built-in `IBufferWriter<T>` implementation called `ArrayBufferWriter<T>` that can be used with the `Utf8JsonWriter`. Here's a code snippet that highlights the writer changes:

```C#
// New, built-in IBufferWriter<byte> that's backed by a grow-able array
var arrayBufferWriter = new ArrayBufferWriter<byte>();

// Utf8JsonWriter is now IDisposable
using (var writer = new Utf8JsonWriter(arrayBufferWriter, new JsonWriterOptions { Indented = true }))
{

   // Write some JSON using existing WriteX() APIs.

   writer.Flush(); // There is no isFinalBlock bool parameter anymore
}
```

You can read more about the design change [here](https://gist.github.com/ahsonkhan/f6f30656717548212693e5eaa49cece5).

## Index and Range

In the previous preview, the framework supported `Index` and `Range` by providing overloads of common operations, such as indexers and methods like `Substring`, that accepted `Index` and `Range` values. Based on feedback of early adopters, we decided to simplify this by letting the compiler call the existing indexers instead. The [Index and Range Changes document](https://github.com/dotnet/csharplang/blob/master/proposals/index-range-changes.md) has more details on how this works but the basic idea is that the compiler is able to call an `int` based indexer by extracting the offset from the given `Index` value. This means that indexing using `Index` will now work on all types that provide an indexer and have a `Count` or `Length` property. For `Range`, the compiler usually cannot use an existing indexer because those only return singular values. However, the compiler will now allow indexing using `Range` when the type either provides an indexer that accepts `Range` or if there is a method called `Slice`. This enables you to make indexing using `Range` also work on interfaces and types you don't control by providing an extension method.

Existing code that uses these indexers will continue to compile and work as expected, as demonstrated by the following code.

```C#
string s = "0123456789";
char lastChar = s[^1]; // lastChar = '9'
string startFromIndex2 = s[2..]; // startFromIndex2 = "23456789"
```

The following `String` methods have been removed:

```C#
public String Substring(Index startIndex);
public String Substring(Range range);
```

Any code uses that uses these `String` methods will need to be updated to use the indexers instead

```C#
string substring = s[^10..]; // Replaces s.Substring(^10);
string substring = s[2..8];   // Replaces s.Substring(2..8);
```

The following `Range` method previously returned `OffsetAndLength`:

```C#
public Range.OffsetAndLength GetOffsetAndLength(int length);
```

It will now simply return a tuple instead:

```C#
public ValueTuple<int, int> GetOffsetAndLength(int length);
```

The following code sample will continue to compile and run as before:

```C#
(int offset, int length) = range.GetOffsetAndLength(20);
```

## Introducing the new SqlClient

SqlClient is the data provider you use to access SQL Server and Azure SQL Database, either through one of the popular .NET O/RMs, like EF Core or Dapper, or directly using the ADO.NET APIs.

For many years, SqlClient shipped as part of the System.Data.dll assembly in .NET Framework. Any time taking advantage of new SQL Server features required changes on SqlClient, we had to wait until the next opportunity to update .NET Framework in Windows. While this used to work somewhat acceptably, with new SQL Server features still shipping regularly, new feature development moving to .NET Core, and the change in focus of .NET Framework towards stability, it made more sense to take the development of SqlClient out-of-band.

Enter [Microsoft.Data.SqlClient]( https://www.nuget.org/packages/Microsoft.Data.SqlClient/), a new version of SqlClient that you can add as a NuGet package on both .NET Framework and .NET Core (including .NET Core 3.0) applications, today launching in preview.

### What is new in Microsoft.Data.SqlClient?

Lack of support for Always Encrypted on .NET Core has been a major pain point, and we are very happy to address it in this preview.

We are also making other two new features available on both .NET Framework or .NET Core:
- Data Classification 
- UTF-8 support 

We currently plan to release these and other improvements in Microsoft.Data.SqlClient in a similar timeframe as .NET Core 3.0.  

### What does this mean for System.Data.SqlClient?

System.Data.SqlClient will still be supported and receive important security updates, so there is no need to move if your application works well with it. But if you want to take advantage of any new features, you should consider upgrading to Microsoft.Data.SqlClient. The process should be straightforward for many applications: just install the package, and update the SqlClient namespace in your code. In some other cases, changes to configuration or updated versions of O/RMs that depend on the new SqlClient will be reuqired.  

Stay tuned in this blog for a post containing many more details about the new SqlClient.

## New Japanese Era (Reiwa)

On May 1st, 2019, Japan started a new era called [Reiwa](https://en.wikipedia.org/wiki/Reiwa). Software that has support for Japanese calendars, like .NET Core, must be updated to accommodate Reiwa. .NET Core and .NET Framework have been updated and correctly handle Japanese date formatting and parsing with the new era. 

.NET relies on operating system or other updates to correctly process Reiwa dates. If you or your customers are using Windows, download the latest updates for your Windows version. If running macOS or Linux, download and install [ICU version 64.2]( http://site.icu-project.org/download/64), which has support the new Japanese era. 

[Handling a new era in the Japanese calendar in .NET blog](https://devblogs.microsoft.com/dotnet/handling-a-new-era-in-the-japanese-calendar-in-net/) has more information about the changes done in the .NET to support the new Japanese era.

## Hardware Intrinsic API changes

The `Avx2.ConvertToVector256*` methods were changed to return a signed, rather than unsigned type. This puts them inline with the `Sse41.ConvertToVector128*` methods and the corresponding native intrinsics. As an example, `Vector256<ushort> ConvertToVector256UInt16(Vector128<byte>)` is now `Vector256<short> ConvertToVector256Int16(Vector128<byte>)`.

The `Sse41/Avx.ConvertToVector128/256*` methods were split into those that take a `Vector128/256<T>` and those that take a `T*`. As an example, `ConvertToVector256Int16(Vector128<byte>)` now also has a `ConvertToVector256Int16(byte*)` overload. This was done because the underlying instruction which takes an address does a partial vector read (rather than a full vector read or a scalar read). This meant we were not able to always emit the optimal instruction coding when the user had to do a read from memory. This split allows the user to explicitly select the addressing form of the instruction when needed (such as when you don't already have a `Vector128<T>`).

The `FloatComparisonMode` enum entries and the `Sse`/`Sse2.Compare` methods were renamed to clarify that the operation is ordered/unordered and not the inputs. They were also reordered to be more consistent across the SSE and AVX implementations. An example is that `Sse.CompareEqualOrderedScalar` is now `Sse.CompareScalarOrderedEqual`. Likewise, for the AVX versions, `Avx.CompareScalar(left, right, FloatComparisonMode.OrderedEqualNonSignalling)` is now `Avx.CompareScalar(left, right, FloatComparisonMode.EqualOrderedNonSignalling)`.

## .NET Core runtime roll-forward policy update

The .NET Core runtime, actually the runtime binder, now enables major-version roll-forward as an opt-in policy. The runtime binder already enables roll-forward on patch and minor versions as a default policy. We never intend to enable major-version roll-forward as a default policy, however, it is an important for some scenarios. 

We also believe that it is important to expose a comprehensive set of runtime binding configuration options to give you the control you need.

There is a new know called `RollForward`, which accepts the following values:

* `LatestPatch` -- Roll forward to the highest patch version. This disables minor version roll forward.
* `Minor` -- Roll forward to the lowest higher minor version, if requested minor version is missing. If the requested minor version is present, then the LatestPatch policy is used. This is the default policy.
* `Major` -- Roll forward to lowest higher major version, and lowest minor version, if requested major version is missing. If the requested major version is present, then the Minor policy is used.
* `LatestMinor` -- Roll forward to highest minor version, even if requested minor version is present.
* `LatestMajor` -- Roll forward to highest major and highest minor version, even if requested major is present.
* `Disable` -- Do not roll forward. Only bind to specified version. This policy is not recommended for general use since it disable the ability to roll-forward to the latest patches. It is only recommended for testing.

See [Runtime Binding Behavior](https://github.com/dotnet/designs/blob/master/accepted/runtime-binding.md) and [dotnet/core-setup #5691](https://github.com/dotnet/core-setup/pull/5691) for more information.

## Reducing the size of the .NET Core Runtime Docker Images

We reduced the size of the runtime by about 10 MB by using a feature we call "partial crossgen".

By default, when we ahead-of-time compile an assembly, we compile all methods. These native compiled methods increase the size of an assembly, sometimes by a lot (the cost is quite variable). In many cases, a subset, sometimes a small subset, of methods are used at startup. That means that cost and benefit and can be asymmetric. Partial crossgen enables us to pre-compile only the methods that matter.

To enable this outcome, we run several .NET Core applications and collect data about which methods are called. We call this process "training". The training data is called "IBC", and is used as an input to crossgen to determine which methods to compile.

This process is only useful if we train the product with representative applications. Otherwise, it can hurt startup. At present, we are targeting making Docker container images for Linux smaller. As a result, it's only the .NET Core runtime build for Linux that is smaller and where we used partial crossgen. That enables us to train .NET Core with a smaller set of applications, because the scenario is relatively narrow. Our training has been focused on the .NET Core SDK (for example, running `dotnet build` and `dotnet test`), ASP.NET Core applications and PowerShell.

We will likely expand the use of partial crossgen in future releases.

## AssemblyLoadContext Updates

We are continuing to improve AssemblyLoadContext. We aim to make simple plug-in models to work without much effort (or code) on your part, and to enable complex plug-in models to be possible. In Preview 5, we enabled implicit type and assembly loading via Type.GetType when the caller is not the application, like a serializer, for example.

See the [AssemblyLoadContext.CurrentContextualReflectionContext design document](https://github.com/dotnet/coreclr/blob/master/Documentation/design-docs/AssemblyLoadContext.ContextualReflection.md) for more information.

## COM-callable managed components

You can now create COM-callable managed components, on Windows. This capability is critical to use .NET Core with COM add-in models, and also to provide parity with .NET Framework.

With .NET Framework, we used `mscoree.dll` as the COM server. With .NET Core, we provide a native launcher dll that gets added to the component `bin` directory when you build your COM component.

See [COM Server Demo](https://github.com/dotnet/samples/blob/staging/core/extensions/COMServerDemo/ReadMe.md) to try out this new capability.

## GC Large page support

[Large Pages](https://docs.microsoft.com/en-us/windows/desktop/Memory/large-page-support) (also known as [Huge Pages](https://wiki.debian.org/Hugepages) on Linux) is a feature where the operating system is able to establish memory regions larger than the native page size (often 4K) to improve performance of the application requesting these large pages.

When a virtual-to-physical address translation occurs, a cache called the [Translation lookaside buffer (TLB)](https://en.wikipedia.org/wiki/Translation_lookaside_buffer) is first consulted (often in parallel) to check if a physical translation for the virtual address being accessed is available to avoid doing a page-table walk which can be expensive. Each large-page translation uses a single translation buffer inside the CPU. The size of this buffer is typically three orders of magnitude larger than the native page size; this increases the efficiency of the translation buffer, which can increase performance for frequently accessed memory.

The GC can now be configured with the [GCLargePages](https://github.com/dotnet/coreclr/blob/master/src/inc/clrconfigvalues.h#L326) as an opt-in feature to choose to [allocate large pages on Windows](https://github.com/dotnet/coreclr/pull/23251). Using large pages reduces TLB misses therefore can potentially increase application performance. It does, however, come with some [limitations](https://docs.microsoft.com/en-us/windows/desktop/Memory/large-page-support).

## Closing

Thanks for trying out .NET Core 3.0. Please continue to give us feedback, either in the comments or on GitHub. We actively looking for reports and will continue to make changes based on your feedback.

Take a look at the [.NET Core 3.0 Preview 1](https://blogs.msdn.microsoft.com/dotnet/2018/12/04/announcing-net-core-3-preview-1-and-open-sourcing-windows-desktop-frameworks/), [Preview 2](https://devblogs.microsoft.com/dotnet/announcing-net-core-3-preview-2/), [Preview 3](https://devblogs.microsoft.com/dotnet/announcing-net-core-3-preview-3/) and [Preview 4](https://devblogs.microsoft.com/dotnet/announcing-net-core-3-preview-4/) posts if you missed those. With this post, they describe the complete set of new capabilities that have been added so far with the .NET Core 3.0 release.
