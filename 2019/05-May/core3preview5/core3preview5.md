# Announcing .NET Core 3 Preview 5

Today, we are announcing .NET Core 3.0 Preview 5. It includes [list features here] . If you missed it, check out the improvements we released in [.NET Core 3.0 Preview 4](https://devblogs.microsoft.com/dotnet/announcing-net-core-3-preview-4/) just last month.

[Download .NET Core 3 Preview 5](https://dotnet.microsoft.com/download/dotnet-core/3.0) right now on Windows, macOS and Linux.

## GC Large page support

[Large Pages](https://docs.microsoft.com/en-us/windows/desktop/Memory/large-page-support) (a.k.a [Huge Pages](https://wiki.debian.org/Hugepages) on Linux) is a feature where the operating system is able to establish memory regions larger than the native page size (often 4K) to improve performance of the application requesting these large pages.

When a virtual-to-physical address translation occurs, a cache called the [Translation lookaside buffer (TLB)](https://en.wikipedia.org/wiki/Translation_lookaside_buffer) is first consulted (often in parallel) to check if a physical translation for the virtual address being accessed is available to avoid doing a page-table walk which can be expensive. Each large-page translation uses a single translation buffer inside the CPU. The size of this buffer is typically three orders of magnitude larger than the native page size; this increases the efficiency of the translation buffer, which can increase performance for frequently accessed memory.

The GC can now be configured with the [GCLargePages](https://github.com/dotnet/coreclr/blob/master/src/inc/clrconfigvalues.h#L326) opt-in feature to choose to [allocate large pages on Windows](https://github.com/dotnet/coreclr/pull/23251). Using large pages reduces TLB misses therefore can potentially increase application perf in general, however, it has its own set of [limitations](https://docs.microsoft.com/en-us/windows/desktop/Memory/large-page-support), such as:

1. it requires contiguous physical space to back up a virtual large page they are hard to come by. So we ask you to specify us the heap size (conveniently with the GCHeapHardLimit config we added in preview 2) and GC can grab all of it upfront – if we wait it’s very likely we won’t be able to find more large pages.
2. you’ll need to give the [SeLockMemoryPrivilege permission](https://github.com/dotnet/core-eng/issues/5776) to even the administrators group.

The current support does come with the following caveats:

1. The Linux support isn’t implemented yet, but we are working on adding it – it’s a matter of adding large pages in the Linux PAL implementation.
2. The bigger caveat is due to the combination of the restriction in large pages on Windows and how GCHeapHardLimit was implemented. On Windows you cannot reserve and then commit large pages (ie, you have to reserve and commit at the same time). And the implementation of GCHeapHardLimit required us to reserve 3x of the limit which is not a problem on 64-bit since it’s just reserve. But with large pages you have to commit so right now it commits 3x of the limit you specify. We will be checking in a change soon that makes this go to 2x (2x is because we do not know how the large and small object distribution will be on the heap). We will be looking at further size reduction in the coming months.

## Hardware Intrinsic API changes

The `Avx2.ConvertToVector256*` methods were changed to return a signed, rather than unsigned type. This puts them inline with the `Sse41.ConvertToVector128*` methods and the corresponding native intrinsics. As an example, `Vector256<ushort> ConvertToVector256UInt16(Vector128<byte>)` is now `Vector256<short> ConvertToVector256Int16(Vector128<byte>)`.

The `Sse41/Avx.ConvertToVector128/256*` methods were split into those that take a `Vector128/256<T>` and those that take a `T*`. As an example, `ConvertToVector256Int16(Vector128<byte>)` now also has a `ConvertToVector256Int16(byte*)` overload. This was done because the underlying instruction which takes an address does a partial vector read (rather than a full vector read or a scalar read). This meant we were not able to always emit the optimal instruction coding when the user had to do a read from memory. This split, allows the user to explicitly select the addressing form of the instruction when needed (such as when you don't already have a `Vector128<T>`).

The `FloatComparisonMode` enum entries and the `Sse`/`Sse2.Compare` methods were renamed to clarify that the operation is ordered/unordered and not the inputs. They were also reordered to be more consistent across the SSE and AVX implementations. An example is that `Sse.CompareEqualOrderedScalar` is now `Sse.CompareScalarOrderedEqual`. Likewise, for the AVX versions, `Avx.CompareScalar(left, right, FloatComparisonMode.OrderedEqualNonSignalling)` is now `Avx.CompareScalar(left, right, FloatComparisonMode.EqualOrderedNonSignalling)`.

## JSON Serializer
The new JSON serializer layers on top of the high-performance `Utf8JsonReader` and `Utf8JsonWriter`. It deserializes objects from JSON and serializes objects to JSON. Memory allocations are kept minimal and includes support for reading and writing JSON with `Stream` asynchronously.

To get started, use the `JsonSerializer` class in the `System.Text.Json.Serialization` namespace. See the [documentation](https://github.com/dotnet/corefx/blob/master/src/System.Text.Json/docs/SerializerProgrammingModel.md) for information and samples. The feature set is currently being extended for future previews.

## Closing

Thanks for trying out .NET Core 3.0. Please continue to give us feedback, either in the comments or on GitHub. We are listening carefully and will continue to make changes based on your feedback.

Take a look at the [.NET Core 3.0 Preview 1](https://blogs.msdn.microsoft.com/dotnet/2018/12/04/announcing-net-core-3-preview-1-and-open-sourcing-windows-desktop-frameworks/), [Preview 2](https://devblogs.microsoft.com/dotnet/announcing-net-core-3-preview-2/), [Preview 3](https://devblogs.microsoft.com/dotnet/announcing-net-core-3-preview-3/) and [Preview 4](https://devblogs.microsoft.com/dotnet/announcing-net-core-3-preview-4/) posts if you missed those. With this post, they describe the complete set of new capabilities that have been added so far with the .NET Core 3.0 release.
