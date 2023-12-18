---
post_title: Hardware Intrinsics in .NET 8
author1: tagoo@microsoft.com
post_slug: dotnet-8-hardware-intrinsics
microsoft_alias: tagoo
featured_image: hardware-intrinsics-in-net8.png
categories: .NET, C#
summary: .NET 8 includes significant improvements to the Hardware Intrinsics feature.
tags: .net 8
post_date: 2023-12-11 13:15:00
---

.NET has a long history of providing access to additional hardware functionality via APIs that are intrinsically understood by the JIT compiler. This started on .NET Framework back in 2014 and expanded with the introduction of .NET Core 3.0 in 2019. Since then, the runtime has iteratively provided more APIs and taken better advantage of this in each release.

As a brief overview:
* 2014 - .NET 4.5.2 - First APIs exposed in the `System.Numerics` namespace
  * Introduces `Vector<T>`
  * Introduces `Vector2`, `Vector3`, `Vector4`, `Matrix4x4`, `Quaternion`, and `Plane`
  * 64-bit only
  * See also: https://devblogs.microsoft.com/dotnet/the-jit-finally-proposed-jit-and-simd-are-getting-married/
* 2019 - .NET Core 3.0 - First APIs exposed in the `System.Runtime.Intrinsics` namespace
  * Introduces `Vector128<T>` and `Vector256<T>`
  * Introduces `Sse`, `Sse2`, `Sse3`, `Ssse3`, `Sse41`, `Sse42`, `Avx`, `Avx2`, `Fma`, `Bmi1`, `Bmi2`, `Lzcnt`, `Popcnt`, `Aes`, and `Pclmul` for `x86`/`x64`
  * 32-bit and 64-bit support
  * See also: https://devblogs.microsoft.com/dotnet/hardware-intrinsics-in-net-core/
* 2020 - .NET 5 - Arm support added to the `System.Runtime.Intrinsics` namespace
  * Introduces `Vector64<T>`
  * Introduces `AdvSimd`, `ArmBase`, `Dp`, `Rdm`, `Aes`, `Crc32`, `Sha1`, and `Sha256` for `Arm`/`Arm64`
  * Introduces `X86Base` for `x86`/`x64`
  * See also: https://devblogs.microsoft.com/dotnet/announcing-net-5-0-preview-7/
* 2021 - .NET 6 - Codegen and infrastructure improvements
  * Introduces `AvxVnni` for `x86`/`x64`
  * Rewrites the `System.Numerics` implementation to use `System.Runtime.Intrinsics`
  * See also: https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-6/
* 2022 - .NET 7 - Support for writing cross platform algorithms
  * Introduces significant new functionality on the `Vector64<T>`, `Vector128<T>`, and `Vector256<T>` types that works across platforms
  * Introduces `X86Serialize` for `x86`/`x64`
  * Brings the API surface exposed by the above vector types and `Vector<T>` to a parity
  * See also: https://devblogs.microsoft.com/dotnet/performance_improvements_in_net_7/
* 2023 - .NET 8 - [Wasm](https://webassembly.org/) support and AVX-512
  * Introduces `PackedSimd` and `WasmBase` for `Wasm`
  * Introduces `Vector512<T>`
  * Introduces `Avx512F`, `Avx512BW`, `Avx512CD`, `Avx512DQ`, and `Avx512Vbmi` for `x86`/`x64`
  * See also: The rest of this blog post

Because of this work, with every release .NET libraries and applications gain more power to take advantage of the underlying hardware. In this post I'll cover in depth what we introduced in .NET 8 and the type of functionality it enables.

## WebAssembly Support

[WebAssembly](https://devblogs.microsoft.com/dotnet/extending-web-assembly-to-the-cloud/), or Wasm for short, is essentially code that runs in your browser and which allows a much higher performance profile than typical interpreted scripting support. As a platform, Wasm has started providing underlying SIMD (Single Instruction, Multiple Data) support so that core algorithms can be accelerated and .NET has correspondingly opted to expose support for this functionality via hardware intrinsics.

This support is very similar to the foundations that other platforms provide and so we won't go into it in significant detail. Rather, you can simply expect that your existing cross platform algorithms using `Vector128<T>` will implicitly light up where supported. If you want to take more direct advantage of functionality that is unique to Wasm, then you can explicitly use the APIs exposed by the `PackedSimd` and `WasmBase` classes in the `System.Runtime.Intrinsics.Wasm` namespace.

## AVX-512 Support

AVX-512 is a new feature set provided for x86 and x64 computers. It brings along with it a large set of new instructions and hardware functionality that wasn't previously available including support for 16 additional SIMD registers, dedicated masking, and operating on 512-bits of data at a time. Access to this functionality requires a relatively new processor, namely it requires Skylake-X or newer from Intel and Zen4 or newer from AMD. Because of this, the number of users that can take advantage of this new functionality is smaller, but the improvements it can bring to that hardware are still significant and make it worthwhile to support for data heavy workloads. Additionally, the JIT will opportunistically utilize these instructions for existing SIMD code where it determines benefit to exist. Some examples include:
* using `vpternlog` instead of `and, andn, or` when a bitwise conditional select is done (`Vector128.ConditionalSelect`)
* using the EVEX encoding to fit more operations into less bytes of code, such as for embedded broadcasts (`x + Vector128.Create(5)`)
* using newer instructions where support now exists with AVX-512, such as for full-width shuffling and many `long`/`ulong` (`Int64`/`UInt64`) operations
* there were other improvements, that are not listed here, as well and you can expect even more to be added over time
  * some cases such as `Vector<T>` allowing scaling to 512-bits were not completed in .NET 8

In order to support the new vector size of 512-bits, .NET introduced the `Vector512<T>` type. This exposes the same general API surface as the other fixed-sized vector types such as `Vector256<T>`. It likewise continues exposing the `Vector512.IsHardwareAccelerated` property that allows you to determine whether the general logic should be accelerated in the hardware or if it will end up emulating the behavior via a software fallback.

Vector512 is accelerated with AVX-512 by default on Ice Lake and newer hardware (and thus `Vector512.IsHardwareAccelerated` reports `true`), where AVX-512 instructions do not cause the CPU to significantly downclock; where-as utilizing AVX-512 instructions can cause more significant downclocking on Skylake-X, Cascade Lake, and Cooper Lake based hardware (see also `2.5.3 Skylake Server Power Management` in the `Intel® 64 and IA-32 Architectures Optimization Reference Manual: Volume 1`). While this is ultimately beneficial for large workloads, it can negatively impact other smaller workloads and as such we default to reporting `false` for `Vector512.IsHardwareAccelerated` on these platforms. `Avx512F.IsSupported` will still report true and the underlying implementation of `Vector512` will still utilize `AVX-512` instructions if called directly. This allows workloads to take advantage of the functionality where they know it to be explicitly beneficial without accidentally causing a negative impact to others.

### Special Thanks

This functionality was made possible via significant contributions by our friends at Intel. The .NET team and Intel have collaborated many times over the years and this continued by us working together on the overall design and implementation, allowing the AVX-512 support to land in .NET 8.

There was also a great deal of input and validation from the .NET community that helped achieve success and make the release all the better.

If you would like to contribute or provide input, please join us in the [dotnet/runtime](https://github.com/dotnet/runtime) repos on GitHub, and tune into API Review on the [.NET Foundation YouTube](https://www.youtube.com/@NETFoundation/streams) channel by following our [schedule](https://apireview.net/schedule) where you can see us discuss new additions to the .NET Libraries and even provide your own input via the chat channel.

### It's not just 512-bits?

Contrary to the name, AVX-512 is not just about 512-bit support. The additional registers, masking support, embedded rounding or broadcast support, and new instructions also all exist for 128-bit and 256-bit vectors. This means that your existing workloads can implicitly get better and you can take explicit advantage of newer functionality where such implicit light up is not possible.

When SSE was first introduced in 1999 on the Intel Pentium III, it provided 8 registers each 128-bits in length. These registers were known as `xmm0` through `xmm7`. When the x64 platform was later introduced in 2003 on the AMD Athlon 64, it then provided 8 additional registers that were accessible to 64-bit code. These registers were named `xmm8` through `xmm15`. This initial support used a simple encoding scheme that worked in a very similar manner to the general purpose instructions and only allowed for 2 registers to be specified. For something like addition which requires 2 inputs, this meant that one of the registers acted as both an input and an output. This meant that if your input and output needed to be different, you needed 2 instructions to complete the operation. Effectively, your `z = x + y` would become `z = x; z += y`. At the high level these behave the same, but at the low level there is 2 rather than 1 step to make it happen.

This was then further expanded in 2011 when Intel introduced AVX on the Sandy Bridge based processors by expanding the support to 256-bits. These newer registers were named `ymm0` through `ymm15`, with only registers up to `ymm7` being accessible to 32-bit code. This also introduced a new encoding known as `VEX` (Vector Extensions) which allowed for 3 registers to be encoded. This meant that you could encode `z = x + y` directly and didn't have to break it into 2 separate steps.

AVX-512 was then introduced by Intel in 2017 with the Skylake-X based processors. This expanded that support to 512-bits and named the registers `zmm0` through `zmm15`. It also introduced 16 new registers, aptly named `zmm16` through `zmm31` and which also have `xmm16-xmm31` and `ymm16-ymm31` variants. As with the previous cases, only registers up to `zmm7` are accessible to 32-bit code. It introduced 8 new registers, named `k0` through `k7`, designed to support "masking" and another new encoding named `EVEX` (Enhanced Vector Extensions) which allows all this new information to be expressed. The EVEX encoding also has other features that allow more common information and operations to be expressed in a more compact fashion. This can help decrease code size while improving performance.

### What new instructions exist?

There is a lot of new functionality, far too much to cover everything in this blog post. But some of the most notable new instructions provide things like:
* Support for doing operations like `Abs`, `Max`, `Min`, and shifting on 64-bit integers - previously this functionality had to be emulated using multiple instructions
* Support for doing conversions between unsigned integers and floating-point types
* Support for working with floating-point edge cases
* Support for fully rearranging the elements in a vector or multiple vectors
* Support for doing 2 bitwise operations in a single instruction

The 64-bit integer support is notable because it means working with 64-bit data doesn't need to use a slower or alternative code sequence to support the same functionality. It makes it much easier to just write your code and expect it to behave the same regardless of the underlying data type you're working with.

The floating-point to unsigned integer conversion support is notable for similar reasons. Converting from `double` to `long` required a single instruction, but converting from `double` to `ulong` required many instructions. With AVX-512 this becomes a single instruction and allows users to get the expected performance when working with unsigned data. This can be common in various image processing or Machine Learning scenarios.

The expanded support for floating-point data is one of my favorite features of AVX-512. Some examples include the ability to extract the unbiased exponent (`Avx512F.GetExponent`) or the normalized mantissa (`Avx512F.GetMantissa`), to round a floating-point value to a specific number of fraction bits (`Avx512F.RoundScale`), to multiply a value by 2^x (`Avx512F.Scale`, known in C as `scalebn`), to perform `Min`, `Max`, `MinMagnitude`, and `MaxMagnitude` with correct handling of `+0` and `-0` (`Avx512DQ.Range`), and even to perform reductions which are useful when handling large values for trigonometric functions like `Sin` or `Cos` (`Avx512DQ.Reduce`).

However, one of my personal favorites is an instruction named `vfixupimm` (`Avx512F.Fixup`). At a high level, this instruction lets you detect a number of input edge cases and "fixup" the output to be one of the common outputs and to do this per element. This can massively improve the performance of some algorithms and greatly reduces the amount of handling required. The way it works is it takes 4 inputs known as `left`, `right`, `table`, and `control`. It first does a classification of the floating-point value in `right` and determines if it is `QNaN` (0), `SNaN` (1), `+/-0` (2), `+1` (3), `-Infinity` (4), `+Infinity` (5), `Negative` (6), or `Positive` (7). It then uses that to read `4` bits from `table` (`QNaN` being `0`, reads bits `0..3`; `Negative` being `6` reads bits `24..27`). The value of those 4 bits in `table` then determines what the result will be. The possible results (per element) are:

| Bit Pattern | Definition                                   |
| ----------- | -------------------------------------------- |
| 0b0000      | left[i]                                      |
| 0b0001      | right[i]                                     |
| 0b0010      | QNaN(right[i])                               |
| 0b0011      | QNaN                                         |
| 0b0100      | -Infinity                                    |
| 0b0101      | +Infinity                                    |
| 0b0110      | IsNegative(right[i]) ? -Infinity : +Infinity |
| 0b0111      | -0.0                                         |
| 0b1000      | +0.0                                         |
| 0b1001      | -1.0                                         |
| 0b1010      | +1.0                                         |
| 0b1011      | +0.5                                         |
| 0b1100      | +90.0                                        |
| 0b1101      | PI / 2                                       |
| 0b1110      | MaxValue                                     |
| 0b1111      | MinValue

With SSE there was some support for rearranging the data in a vector. Say, for example, you had `0, 1, 2, 3` and you wanted it ordered `3, 1, 2, 0`. With the introduction of AVX and the expansion to 256-bits, this support was likewise expanded. However, due to how the instructions operated you'd actually do the same 128-bit operation twice. This made it simple to expand existing algorithms to 256-bits since you effectively are just doing the same thing twice. However, it made working with other algorithms more difficult when you actually needed to consider the entire vector cohesively. There were some instructions that let you rearrange the data across the entire 256-bit vector, but they were often limited either in how the data could be rearranged or in the types they supported (full shuffle of byte elements is a notable example of missing support). AVX-512 has many of the same considerations for its expanded 512-bit support. However, it also introduces new instructions that fill the gap and now let you fully rearrange the elements for any size of element.

Finally, one of my other personal favorites is an instruction named `vpternlog` (`Avx512F.TernaryLogic`). This instruction lets you take any 2 bitwise operations and combine them, so they can be executed in a single instruction. For example, you can do `(a & b) | c`. The way it works is that it takes 4 inputs, `a`, `b`, `c`, and `control`. You then have 3 keys to remember: `A: 0xF0`, `B: 0xCC`, `C: 0xAA`. In order to represent the operation desired, you simply build the `control` by performing that operation on those keys. So, if you wanted to simply return `a`, you'd use `0xF0`. If you wanted to do `a & b`, you'd use `(byte)(0xF0 & 0xCC)`. If you wanted to do `(a & b) | c`, then it is `(byte)((0xF0 & 0xCC) | 0xAA`. There are 256 different operations possible in total, with the basic building blocks being those keys and the following bitwise operations:

| Operation | Definition |
| --------- | ---------- |
| not       | ~x         |
| and       |  x & y     |
| nand      | ~x & y     |
| or        |  x | y     |
| nor       | ~x | y     |
| xor       |  x ^ y     |
| xnor      | ~x ^ y     |

There are then some special operations that are also supported given the above basic operations and which can expand even further.

| Operation          | Definition                                                                           |
| ------------------ | ------------------------------------------------------------------------------------ |
| false              | Bit pattern of 0x00                                                                  |
| true               | Bit pattern of 0xFF                                                                  |
| major              | Returns 0 if two or more input bits are 0, returns 1 if two or more input bits are 1 |
| minor              | Returns 0 if two or more input bits are 1, returns 1 if two or more input bits are 0 |
| conditional select | Logically `(x & y) | (~x & z)`, which works since it is `(x and y) or (x nand y)`    |

In .NET 8 we didn't complete the support to implicitly recognize and fold these patterns to emit `vpternlog`. We expect that to debut in .NET 9.

### What is masking support?

At the simplest level, writing vectorized code involves using SIMD to do the same basic operation on `Count` different elements of type `T` in a single instruction. This works very nicely when the same operation needs to be done to all data. However, not all data is necessarily uniform and sometimes you need to handle particular inputs differently. For example, you may want to do a different operation for positive vs negative numbers. You may need to return a different result if the user has passed in `NaN`, and so on. When writing regular code, you would normally handle this with a branch and this works very nicely. When writing vectorized code, however, such branches break the ability to use SIMD instructions since you have to handle each element independently. .NET takes advantage of this in various locations, including the new `TensorPrimitives` APIs where it allows us to handle trailing data that would otherwise not fit into a full vector.

The typical solution for this is to write "branch-free" code. One of the simplest ways to do this is to compute both answers and then use bitwise operations to pick the correct answer. You can think of this a lot like a ternary condition `cond ? result1 : result2`. In order to support this in SIMD, there exists an API named `ConditionalSelect` which takes a mask and both results. The mask is also a vector, but its values are typically either `AllBitsSet` or `Zero`. When you have this pattern, then the implementation of `ConditionalSelect` is effectively `(cond & result1) | (~cond & result2)`. What this breaks down to doing is taking bits from `result1` where the corresponding bit in `cond` is `1` and otherwise taking the corresponding bit from `result2` (when the bit in `cond` is `0`). So if you wanted to convert all negative values to `0`, you would have something like `(x < 0) ? 0 : x` for regular code and `Vector128.ConditionalSelect(Vector128.LessThan(x, Vector128.Zero), Vector128.Zero, x)` for vectorized code. It's a bit more verbose, but can also provide significant performance improvement.

When hardware first started having SIMD support, you would have to support this masking very literally by doing 3 instructions: `and, nand, or`. As newer hardware came out, more optimized versions were added that allowed you to do this in a single instruction, such as `blendv` on x86/x64 and `bsl` on Arm64. AVX-512 then took this a step further by introducing dedicated hardware support for expressing masks and tracking them in registers (the previously mentioned `k0-k7`). It then provided additional support for allowing this masking to be done as part of almost any other operation. So rather than having to specify `vcmpltps; vblendvps; vaddps` (compare, mask, then add), you could instead encode that mask directly as part of the addition (and thus emit `vcmpltps; vaddps` instead). This allows the hardware to represent more operations in less space, improving code density, and to better take advantage of the intended behavior.

Notably we do not directly expose a 1-to-1 concept with the underlying hardware for masking here. Rather, the JIT continues taking and returning a regular vector for comparison results and does the relevant pattern recognition and subequent opportunistic lightup of masking features based on this. This allows the exposed API surface to be significantly smaller (over 3000 fewer APIs), for existing code to largely "just work" and take advantage of the newer hardware support without explicit action, and for users wanting to support AVX-512 to not have to learn new concepts or write code in a new way.

### What about examples of using AVX-512 in practice?

AVX-512 can be used to accelerate all of the same scenarios as SSE or AVX based scenarios. An easy way to identify where the .NET Libraries are already using this acceleration is to search for the places we're calling `Vector512.IsHardwareAccelerated`, this can be done using [source.dot.net](https://source.dot.net/#System.Private.CoreLib/src/libraries/System.Private.CoreLib/src/System/Runtime/Intrinsics/Vector512.cs,576d90eecb8bc9d4,references).

We've accelerated cases such as:
* System.Collections.BitArray - [creation](https://source.dot.net/#System.Collections/System/Collections/BitArray.cs,137), [bitwise and](https://source.dot.net/#System.Collections/System/Collections/BitArray.cs,137), [bitwise or](https://source.dot.net/#System.Collections/System/Collections/BitArray.cs,137), [bitwise xor](https://source.dot.net/#System.Collections/System/Collections/BitArray.cs,137), [bitwise not](https://source.dot.net/#System.Collections/System/Collections/BitArray.cs,137)
* System.Linq.Enumerable - [Max and Min](https://source.dot.net/#System.Linq/System/Linq/MaxMin.cs,70)
* System.Buffers.Text.Base64 - [Decoding](https://source.dot.net/#System.Private.CoreLib/src/libraries/System.Private.CoreLib/src/System/Buffers/Text/Base64Decoder.cs,72), [Encoding](https://source.dot.net/#System.Private.CoreLib/src/libraries/System.Private.CoreLib/src/System/Buffers/Text/Base64Encoder.cs,71)
* System.String - [Equals, IgnoreCase](https://source.dot.net/#System.Private.CoreLib/src/libraries/System.Private.CoreLib/src/System/Globalization/Ordinal.cs,166)
* System.Span - [IndexOf](https://source.dot.net/#System.Private.CoreLib/src/libraries/System.Private.CoreLib/src/System/SpanHelpers.Packed.cs,117), [IndexOfAny](https://source.dot.net/#System.Private.CoreLib/src/libraries/System.Private.CoreLib/src/System/SpanHelpers.Packed.cs,117), [IndexOfAnyInRange](https://source.dot.net/#System.Private.CoreLib/src/libraries/System.Private.CoreLib/src/System/SpanHelpers.Packed.cs,117), [SequenceEqual](https://source.dot.net/#System.Private.CoreLib/src/libraries/System.Private.CoreLib/src/System/SpanHelpers.Byte.cs,66), [Reverse](https://source.dot.net/#System.Private.CoreLib/src/libraries/System.Private.CoreLib/src/System/SpanHelpers.Byte.cs,66), [Contains](https://source.dot.net/#System.Private.CoreLib/src/libraries/System.Private.CoreLib/src/System/SpanHelpers.Packed.cs,117), etc

There are other examples throughout the .NET libraries and general .NET ecosystem, far too many to list and cover. These include, but are not limited to, scenarios such as color conversions, image processing, machine learning, text transcoding, JSON parsing, software rendering, ray tracing, game acceleration, and much more.

### What's next?

We plan to continue to improve the hardware intrinsics support in .NET when and where it makes sense. Please note the following items are forward thinking and speculative. The list is not complete and we provide no guarantees any of these features will land or when they will ship if they do.

Some of the items on our longer term roadmap include the following:
* `SVE` and SVE2 for Arm64
* `AVX10` for x86/x64
* Allowing `Vector<T>` to implicitly expand to 512-bits
* An `ISimdVector<TSelf, T>` interface to allow better reuse of SIMD logic
* An analyzer to help encourage users to use the cross-platform APIs where the semantics are identical (use `x + y` instead of `Sse.Add(x, y)`)
* An analyzer to recognize patterns that may have more optimal alternatives (do `value + value` instead of `value * 2` or `Sse.UnpackHigh(value, value)` instead of `Sse.Shuffle(value, value, 0b11_11_10_10)`
* Additional explicit usage of hardware intrinsics in various .NET APIs
* Additional cross-platform APIs to help abstract common operation
  * getting the index of the first/last match in a mask
  * getting the number of matches in a mask
  * determining if any matches exist
  * allowing non-deterministic behavior for cases like `Shuffle` or `ConditionalSelect`
    * these APIs have a well-defined behavior on all platforms today, such as `Shuffle` treating any out of range index as zeroing the destination element
    * the new APIs, such as `ShuffleUnsafe`, would instead allow different behavior for out of range indices
	* for such a scenario, Arm64 would have the same behavior, while x64 only has the same behavior if the most-significant bit is set
* Additional pattern recognition for cases like
  * embedded masking (AVX512, AVX10, SVE/SVE2)
  * combined bitwise-operations (`vpternlog` on AVX512)
  * limited JIT time constant folding opportunities

If you're looking to use hardware intrinsics in .NET, we encourage you to try out the APIs available in the [System.Runtime.Intrinsics namespace](https://learn.microsoft.com/dotnet/api/system.runtime.intrinsics?view=net-8.0), log [API suggestions](https://github.com/dotnet/runtime/issues/new?assignees=&labels=api-suggestion&projects=&template=02_api_proposal.yml&title=%5BAPI+Proposal%5D%3A+) for functionality you feel is missing or could be improved, and to engage in our preview releases to try out the functionality before it ships so you can help make each release better than the last!
