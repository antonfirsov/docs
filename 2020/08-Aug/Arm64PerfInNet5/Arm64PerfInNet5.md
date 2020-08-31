# ARM64 performance work in .NET 5
**Kunal Pathak** (Kunal.Pathak@microsoft.com)

The .NET team has significantly improved  performance with .NET 5, both generally and for ARM64. You can check out the general improvements  in the excellent and detailed [Performance Improvements in .NET 5](https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-5/) blog by Stephen. In this post, I will describe the performance improvements we made specifically for ARM64 and show the positive impact on the benchmarks we use. I will also share some of the additional opportunities for performance improvements that we have identified and plan to address in a future release.

While we have been working on ARM64 support in RyuJIT for over five years, most of the work that was done was to ensure that we generate functionally correct ARM64 code. We spent very little time in evaluating the performance of the code  RyuJIT produced for ARM64. As part of .NET 5, our focus was to perform investigation in this area and find out any obvious issues in RyuJIT that would improve the ARM64 code quality (CQ). Since Microsoft VC++ team already support for Windows ARM64, we consulted with them to understand the CQ issues that they encountered when doing a similar exercise.

Although fixing CQ issues is crucial, sometimes its impact might not be noticeable in an application. Hence, we also wanted to make observable improvements in the performance of .NET libraries to benefit .NET applications targeted for ARM64.

Here is the outline I will use to describe our work for improving ARM64 performance on .NET 5:
- ARM64-specific optimizations in the .NET libraries.
- Evaluation of code quality produced by RyuJIT and resulting outcome.

## ARM64 hardware intrinsics in .NET libraries

In .NET Core 3.0, we introduced a new feature called ["hardware intrinsics"](https://devblogs.microsoft.com/dotnet/hardware-intrinsics-in-net-core/) which gives access to various vectorized and non-vectorized instructions that modern hardware support. .NET developers can access these instructions using set of APIs under namespace [System.Runtime.Intrinsics](https://docs.microsoft.com/en-us/dotNet/api/system.runtime.intrinsics?view=net-5.0) and [System.Runtime.Intrinsics.X86](https://docs.microsoft.com/en-us/dotnet/api/system.runtime.intrinsics.x86?view=net-5.0) for x86/x64 architecture. In .NET 5, we added around 384 APIs under [System.Runtime.Intrinsics.Arm](https://docs.microsoft.com/en-us/dotnet/api/system.runtime.intrinsics.arm?view=net-5.0) for ARM32/ARM64 architecture. This involved [implementing those APIs](https://github.com/dotnet/runtime/issues?q=is%3Aissue+label%3Aapi-approved+label%3Aarch-arm64+label%3Aarea-System.Runtime.Intrinsics+is%3Aclosed) and making RyuJIT aware of them so it can emit appropriate ARM32/ARM64 instruction. We also optimized methods of [Vector64](https://docs.microsoft.com/en-us/dotnet/api/system.runtime.intrinsics.vector64?view=net-5.0) and [Vector128](https://docs.microsoft.com/en-us/dotnet/api/system.runtime.intrinsics.vector128?view=net-5.0) that provide ways to create and manipulate [Vector64&lt;T&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.runtime.intrinsics.vector64-1?view=net-5.0) and [Vector128&lt;T&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.runtime.intrinsics.vector128-1?view=net-5.0) datatypes on which majority of the hardware intrinsic APIs operate on. If interested, refer to the sample code usage along with examples of `Vector64` and `Vector128` methods [here](https://kunalspathak.github.io/2020-08-01-Vectorization-APIs/). You can check our "hardware intrinsic" project progress [here](https://github.com/dotnet/runtime/projects/21).

-------------------------------------------------------

## Optimized .NET library code using ARM64 hardware intrinsics

In .NET Core 3.1, we optimized many critical methods of .NET library using x86/x64 intrinsics. Doing that improved the performance of such methods when ran on hardware supporting the x86/x64 intrinsic instructions. For hardware that does not support x86/x64 intrinsics such as ARM machines, .NET would fallback to the slower implementation of those methods. [dotnet/runtime#33308](https://github.com/dotnet/runtime/issues/33308) list such .NET library methods. In .NET 5, we have optimized most of these methods using ARM64 hardware intrinsics as well. So, if your code uses any of those .NET library methods, they will now see speed boost running on ARM architecture. We focused our efforts on methods that already were optimized with x86/x64 intrinsics, because those were chosen based on an earlier performance analysis (which we didn't want to duplicate/repeat) and we wanted the product to have generally similar behavior across platforms. Moving forward, we expect to use both x86/x64 and ARM64 hardware intrinsics as our default approach when we optimize .NET library methods. We still have to decide how this will affect our policy for PRs that we accept.

For each of the methods that we optimized in .NET 5, I'll show you the improvements in terms of the low-level benchmark that we used for validating our improvements. These benchmarks are far from real-world. You'll see later in the post how all of these targeted improvements combine together to greatly improve .NET on ARM64 in larger, more real-world, scenarios.

### System.Collections

`System.Collections.BitArray` methods were optimized by [@Gnbrkm41](https://github.com/Gnbrkm41) in [dotnet/runtime#33749](https://github.com/dotnet/runtime/pull/33749). The following measurements are in `nanoseconds` for [Perf_BitArray](https://github.com/dotnet/performance/blob/c86ef708fc9eea6afad9fac833c2768135a47aa0/src/benchmarks/micro/libraries/System.Collections/Perf.BitArray.cs#L12) microbenchmark.


| BitArray method      | Benchmark                        | .NET Core 3.1 | .NET 5 | Improvements |
|---------------------------|-----------------------------------------------------------------|:---------------:|:--------------:|:--------------:|
| `ctor(bool[])`       | BitArrayBoolArrayCtor(Size: 512) | 1704.68    | 215.55 | -87%          |
| `CopyTo(Array, int)` | BitArrayCopyToBoolArray(Size: 4) | 269.20    | 60.42 | -78%          |
| `CopyTo(Array, int)` | BitArrayCopyToIntArray(Size: 4)  | 87.83    | 22.24 | -75%          |
| `And(BitArray)`      | BitArrayAnd(Size: 512)           | 212.33    | 65.17 | -69%          |
| `Or(BitArray)`       | BitArrayOr(Size: 512)            | 208.82    | 64.24 | -69%          |
| `Xor(BitArray)`      | BitArrayXor(Size: 512)           | 212.34     | 67.33 | -68%          |
| `Not()`              | BitArrayNot(Size: 512)           | 152.55    | 54.47 | -64%          |
| `SetAll(bool)`       | BitArraySetAll(Size: 512)        | 108.41    | 59.71 | -45%          |
| `ctor(BitArray)`     | BitArrayBitArrayCtor(Size: 4)    | 113.39    | 74.63 | -34%          |
| `ctor(byte[])`       | BitArrayByteArrayCtor(Size: 512) | 395.87    | 356.61 | -10%          |

<p/>

### System.Numerics

`System.Numerics.BitOperations` methods were optimized in [dotnet/runtime#34486](https://github.com/dotnet/runtime/pull/34486) and [dotnet/runtime#35636](https://github.com/dotnet/runtime/pull/35636). The following measurements are in `nanoseconds` for [Perf_BitOperations](https://github.com/dotnet/performance/blob/454476401e17ed7f4d8b899ecf7661eb6cd63bad/src/benchmarks/micro/libraries/System.Numerics.BitOperations/Perf_BitOperations.cs#L14) microbenchmark.

| BitOperations method      | Benchmark                                                       | .NET Core 3.1       | .NET 5       | Improvements |
|---------------------------|-----------------------------------------------------------------|:---------------:|:--------------:|:--------------:|
| `LeadingZeroCount(uint)`  |LeadingZeroCount_uint  | 10976.5  | 1155.85 | -89%            |
| `Log2(ulong)`             |Log2_ulong             | 11550.03 | 1347.46 | -88%            |
| `TrailingZeroCount(uint)` |TrailingZeroCount_uint | 7313.95  | 1164.10 | -84%            |
| `PopCount(ulong)`         |PopCount_ulong         | 4234.18  | 1541.48 | -64%            |
| `PopCount(uint)`          |PopCount_uint          | 4233.58  | 1733.83 | -59%            |

`System.Numerics.Matrix4x4` methods were optimized in [dotnet/runtime#40054](https://github.com/dotnet/runtime/pull/40054). The following measurements are in `nanoseconds` for [Perf_Matrix4x4](https://github.com/dotnet/performance/blob/a5296dda39031ac84f40eeb5a0a136c89cde599b/src/benchmarks/micro/libraries/System.Numerics.Vectors/Perf_Matrix4x4.cs#L11) microbenchmark.

| Benchmarks                               | .NET Core 3.1 | .NET 5 | Improvements |
|------------------------------------------|---------------|--------|--------------|
| CreateScaleFromVectorWithCenterBenchmark | 29.39         | 24.84  | -15%         |
| CreateOrthographicBenchmark              | 17.14         | 11.19  | -35%         |
| CreateScaleFromScalarWithCenterBenchmark | 26.00         | 17.14  | -34%         |
| MultiplyByScalarOperatorBenchmark        | 28.45         | 22.06  | -22%         |
| TranslationBenchmark                     | 15.15         | 5.39   | -64%         |
| CreateRotationZBenchmark                 | 50.21         | 40.24  | -20%         |


The SIMD accelerated types `System.Numerics.Vector2`, `System.Numerics.Vector3` and `System.Numerics.Vector4` were optimized in [dotnet/runtime#35421](https://github.com/dotnet/runtime/pull/35421), [dotnet/runtime#36267](https://github.com/dotnet/runtime/pull/36267), [dotnet/runtime#36512](https://github.com/dotnet/runtime/pull/36512), [dotnet/runtime#36579](https://github.com/dotnet/runtime/pull/36579)and [dotnet/runtime#37882](https://github.com/dotnet/runtime/pull/37882) to use hardware intrinsics.  The following measurements are in `nanoseconds` for [Perf_Vector2](https://github.com/dotnet/performance/blob/adccb815003451dd68586516d4f25f52f3f2ebe7/src/benchmarks/micro/libraries/System.Numerics.Vectors/Perf_Vector2.cs#L12), [Perf_Vector3](https://github.com/dotnet/performance/blob/adccb815003451dd68586516d4f25f52f3f2ebe7/src/benchmarks/micro/libraries/System.Numerics.Vectors/Perf_Vector3.cs#L11) and [Perf_Vector4](https://github.com/dotnet/performance/blob/adccb815003451dd68586516d4f25f52f3f2ebe7/src/benchmarks/micro/libraries/System.Numerics.Vectors/Perf_Vector4.cs#L11) microbenchmark.


| Benchmark                              | .NET Core 3.1 | .NET 5  | Improvements |
|----------------------------------------|---------------|--------|--------------|
| Perf_Vector2.AddOperatorBenchmark      | 6.59          | 1.16   | -82%         |
| Perf_Vector2.ClampBenchmark            | 11.94         | 1.10   | -91%         |
| Perf_Vector2.DistanceBenchmark         | 6.55          | 0.70   | -89%         |
| Perf_Vector2.MinBenchmark              | 5.56          | 1.15   | -79%         |
| Perf_Vector2.SubtractFunctionBenchmark | 10.78         | 0.38   | -96%         |
| Perf_Vector3.MaxBenchmark              | 3.46          | 2.31   | -33%         |
| Perf_Vector3.MinBenchmark              | 3.97          | 0.38   | -90%         |
| Perf_Vector3.MultiplyFunctionBenchmark | 3.95          | 1.16   | -71%         |
| Perf_Vector3.MultiplyOperatorBenchmark | 4.30          | 0.77   | -82%         |
| Perf_Vector4.AddOperatorBenchmark      | 4.04          | 0.77   | -81%         |
| Perf_Vector4.ClampBenchmark            | 4.04          | 0.69   | -83%         |
| Perf_Vector4.DistanceBenchmark         | 2.12          | 0.38   | -82%         |
| Perf_Vector4.MaxBenchmark              | 6.74          | 0.38   | -94%         |
| Perf_Vector4.MultiplyFunctionBenchmark | 7.67          | 0.39   | -95%         |
| Perf_Vector4.MultiplyOperatorBenchmark | 3.47          | 0.34   | -90%         |

<p/>


### System.SpanHelpers

`System.SpanHelpers` methods were optimized in [dotnet/runtime#37624](https://github.com/dotnet/runtime/pull/37624) and [dotnet/runtime#37934](https://github.com/dotnet/runtime/pull/37934) work. The following measurements are in `nanoseconds` for [Span&lt;T&gt;.IndexOfValue](https://github.com/dotnet/performance/blob/8aed638c9ee65c034fe0cca4ea2bdc3a68d2a6b5/src/benchmarks/micro/libraries/System.Memory/Span.cs#L69) and [ReadOnlySpan.IndexOfString](https://github.com/dotnet/performance/blob/a6955b7be29e30ec3d8eba83cdf1f8d4de0ae4ff/src/benchmarks/micro/libraries/System.Memory/ReadOnlySpan.cs#L47) microbenchmark.

| Method names           | Benchmark                                                         | .NET Core 3.1         | .NET 5       | Improvements |
|------------------------------|-------------------------------------------------------------------|------------------|----------------|---------------|
| `IndexOf(char)`              | Span<Char>.IndexOfValue(Size: 512)          | 66.51  | 46.88  | -30%           |
| `IndexOf(byte)`              | Span<Byte>.IndexOfValue(Size: 512)          | 34.11  | 25.41  | -25%           |
| `IndexOf(char)`              | ReadOnlySpan.IndexOfString ()               | 172.68 | 137.76 | -20%           |
| `IndexOfAnyThreeValue(byte)` | Span<Byte>.IndexOfAnyThreeValues(Size: 512) | 71.22  | 55.92  | -21%           |

<p/>

### System.Text 
- `System.Text.ASCIIUtility`
- `System.Text.Unicode`
- `System.Text.Encodings.Web`

TODO: Numbers for `System.Text.Unicode`

TODO: Numbers for `System.Text.Encodings.Web`

<p/>

In .NET 6, we are planning to optimizing remaining methods of `System.Text.ASCIIUtility` described in [dotnet/runtime#41292](https://github.com/dotnet/runtime/issues/41292), methods of `System.Buffers` to address [dotnet/runtime#35033](https://github.com/dotnet/runtime/issues/35033) and merge the work to optimize `JsonReaderHelper.IndexOfLessThan` done by [Ben Adams](https://github.com/benaadams) in [dotnet/runtime#41097](https://github.com/dotnet/runtime/pull/41097).


TODO: Should we include sources from where data was gathered?

You can see all the measurements I have mentioned above in our performance lab run that we conducted on [8/6/2020](https://pvscmdupload.blob.core.windows.net/reports/08_06_2020/report_Daily_ca=ARM64_cb=master_co=Ubuntu1804ARM_cr=dotnetcoresdk_cc=CompliationMode=tiered-RunKind=micro_Baseline_bb=release-3.1.2xx_2020-08-06.html
), [8/10/2020](https://pvscmdupload.blob.core.windows.net/reports/08_10_2020/report_Daily_ca=ARM64_cb=master_co=Ubuntu1804ARM_cr=dotnetcoresdk_cc=CompliationMode=tiered-RunKind=micro_Baseline_bb=release-3.1.2xx_2020-08-10.html) and [8/28/2020](https://pvscmdupload.blob.core.windows.net/reports/08_28_2020/report_Daily_ca=ARM64_cb=master_co=Ubuntu1804ARM_cr=dotnetcoresdk_cc=CompliationMode=tiered-RunKind=micro_Baseline_bb=release-3.1.2xx_2020-08-28.html) to compare .NET Core 3.1 and .NET 5.

<p/>

### Details

It is probably clear at this point how impactful and important hardware intrinsics are. I want to show you more, by walking through an example. Imagine a `Test()` returns leading zero count of argument `value`.

```csharp
private int Test(uint value)
{
    return BitOperations.LeadingZeroCount(value);
}
```

Before optimization for ARM64, the code would execute the [software fallback](https://github.com/dotnet/runtime/blob/6072e4d3a7a2a1493f514cdf4be75a3d56580e84/src/libraries/System.Private.CoreLib/src/System/Numerics/BitOperations.cs#L205) of `LeadingZeroCount()`. If you see the ARM64 assembly code generated below, not only it is large, but RyuJIT had to JIT 2 methods - `Test(int)` and `Log2SoftwareFallback(int)`.

```asm
; Test(int):int

        stp     fp, lr, [sp,#-16]!
        mov     fp, sp
        cbnz    w0, M00_L00
        mov     w0, #32
        b       M00_L01
M00_L00:
        bl      System.Numerics.BitOperations:Log2SoftwareFallback(int):int
        eor     w0, w0, #31
M00_L01:
        ldp     fp, lr, [sp],#16
        ret     lr

; Total bytes of code 28, prolog size 8
; ============================================================


; System.Numerics.BitOperations:Log2SoftwareFallback(int):int

        stp     fp, lr, [sp,#-16]!
        mov     fp, sp
        lsr     w1, w0, #1
        orr     w0, w0, w1
        lsr     w1, w0, #2
        orr     w0, w0, w1
        lsr     w1, w0, #4
        orr     w0, w0, w1
        lsr     w1, w0, #8
        orr     w0, w0, w1
        lsr     w1, w0, #16
        orr     w0, w0, w1
        movz    w1, #0xacdd
        movk    w1, #0x7c4 LSL #16
        mul     w0, w0, w1
        lsr     w0, w0, #27
        sxtw    x0, w0
        movz    x1, #0xc249
        movk    x1, #0x5405 LSL #16
        movk    x1, #0x7ffc LSL #32
        ldrb    w0, [x0, x1]
        ldp     fp, lr, [sp],#16
        ret     lr

; Total bytes of code 92, prolog size 8
```

After we optimized `LeadingZeroCount()` to use ARM64 intrinsics, generated code for ARM64 is just a handful of instructions (including the crucial `clz`). In this case, RyuJIT did not even JIT `Log2SoftwareFallback(int)` method because it was not called. Thus, by doing this work, we got improvement in code quality as well as JIT throughput.


```asm
; Test(int):int

        stp     fp, lr, [sp,#-16]!
        mov     fp, sp
        clz     w0, w0
        ldp     fp, lr, [sp],#16
        ret     lr

; Total bytes of code 24, prolog size 8
```

<p/>

-------------------------------------------------------

### AOT compilation for methods having ARM64 intrinsics

In the typical case, applications are compiled to machine code at runtime using the JIT. The target machine code produced is very efficient but has the disadvantage of having to do the compilation during execution and this might add some delay during the application start-up. If the target platform is known in advance, you can create ready-to-run (R2R) native images for that target platform. This is known as ahead of time (AOT) compilation. It has the advantage of faster startup time because there is no need to produce machine code during execution. The target machine code is already present in the binary and can be run directly. AOT compiled code might be suboptimal sometimes but get replaced by optimal code eventually.

Until .NET 5, if a method (.NET library method or user defined method) had calls to ARM64 hardware intrinsic APIs (APIs under `System.Runtime.Intrinsics` and `System.Runtime.Intrinsics.Arm`), such methods were never compiled AOT and were always deferred to get compiled during runtime. This had an impact on start-up time of some .NET apps which used one of these methods in their startup code. In .NET 5, we addressed this problem in [dotnet/runtime#38060](https://github.com/dotnet/runtime/pull/38060) and now able to do the compilation of such methods AOT.

-------------------------------------------------------

## Microbenchmark analysis

Optimizing the .NET libraries with intrinsics was a straightforward step (following in the path of what we'd already done for x86/x64). An equal or more significant project was improving the quality of code that the JIT generates for ARM64. It's important to make that exercise data-oriented. We picked benchmarks that we thought would highlight underlying ARM64 CQ issues. We started with the [Microbenchmarks](https://github.com/dotnet/performance/tree/master/src/benchmarks/micro) that we maintain. There are around 1300 of these benchmarks. You can check the daily report at [https://aka.ms/dotnetperfindex](https://aka.ms/dotnetperfindex).

We compared ARM64 and x64 performance numbers for each of these benchmarks. Parity was not our goal, however, it is always useful to have a baseline to compare with, particularly to identify outliers. We then identified the benchmarks with the worst performance, and determined why that was the case. We tried using some profilers like [WPA](https://docs.microsoft.com/windows-hardware/test/wpt/windows-performance-analyzer) and [PerfView](https://github.com/microsoft/perfview) but they were not useful in this scenario. Those profilers would have pointed out the hottest method in given benchmark. But since MicroBenchmarks are tiny benchmarks with at most 1~2 methods, the hottest method that the profiler pointed was mostly the benchmark method itself. Hence, to understand the ARM64 CQ issues, we decided to just inspect the assembly code produced for a given benchmark and compare it with x64 assembly. That would help us identify basic issues in RyuJIT's ARM64 code generator.

Next, I will describe some of the issues that we found with this exercise.

### Memory barriers in ARM64

Through some of the benchmarks, we noticed  accesses of `volatile` variables in hot loop of critical methods of `System.Collections.Concurrent.ConcurrentDictionary` class. Accessing `volatile` variable for ARM64 is expensive because they introduce memory barrier instructions. I'll describe why, shortly. By caching the volatile variable and storing it in a local variable ([dotnet/runtime#34225](https://github.com/dotnet/runtime/pull/34225), [dotnet/runtime#36976](https://github.com/dotnet/runtime/pull/36976) and [dotnet/runtime#37081](https://github.com/dotnet/runtime/pull/37081)) outside the loop resulted in improved performance, as seen below. All the measurements are in `nanoseconds`.


| Method names      | Benchmarks                                                                                                                                                                                                                                | .NET Core 3.1    | .NET 5         | Improvements |
|-------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|------------------|----------------|---------------|
| `IsEmpty(string)` | [IsEmpty&lt;String&gt;.Dictionary(Size: 512)](https://github.com/dotnet/performance/blob/a5296dda39031ac84f40eeb5a0a136c89cde599b/src/benchmarks/micro/libraries/System.Collections/Concurrent/IsEmpty.cs#L37)                            | 30.11   | 19.38 | -36%           |
| `TryAdd()`        | [TryAddDefaultSize&lt;Int32&gt;.ConcurrentDictionary(Count: 512)](https://github.com/dotnet/performance/blob/a5296dda39031ac84f40eeb5a0a136c89cde599b/src/benchmarks/micro/libraries/System.Collections/Add/TryAddDefaultSize.cs#L39)     | 557564.35  | 398071.1 | -29%           |
| `IsEmpty(int)`    | [IsEmpty&lt;Int32&gt;.Dictionary(Size: 512)](https://github.com/dotnet/performance/blob/a5296dda39031ac84f40eeb5a0a136c89cde599b/src/benchmarks/micro/libraries/System.Collections/Concurrent/IsEmpty.cs#L37)                             | 28.48   | 20.87 | -27%           |
| `ctor()`          | [CtorFromCollection&lt;Int32&gt;.ConcurrentDictionary(Size: 512)](https://github.com/dotnet/performance/blob/a5296dda39031ac84f40eeb5a0a136c89cde599b/src/benchmarks/micro/libraries/System.Collections/Create/CtorFromCollection.cs#L60) | 497202.32  | 376048.69 | -24%           |
| `get_Count`       | [Count&lt;Int32&gt;.Dictionary(Size: 512)](https://github.com/dotnet/performance/blob/a5296dda39031ac84f40eeb5a0a136c89cde599b/src/benchmarks/micro/libraries/System.Collections/Concurrent/Count.cs#L37)                                 | 234404.62  | 185172.15 | -21%           |
| `Add(), Clear()`  | [CreateAddAndClear&lt;Int32&gt;.ConcurrentDictionary(Size: 512)](https://github.com/dotnet/performance/blob/a5296dda39031ac84f40eeb5a0a136c89cde599b/src/benchmarks/micro/libraries/System.Collections/CreateAddAndClear.cs#L168)         | 704458.54   | 581923.04 | -17%           |

We made similar optimization in `System.Threading.ThreadPool` as part of [dotnet/runtime#36697](https://github.com/dotnet/runtime/pull/36697) and in `System.Diagnostics.Tracing.EventCount` as part of [dotnet/runtime#37309](https://github.com/dotnet/runtime/pull/37309) classes. 


#### ARM memory model
ARM architecture has weakly ordered memory model. The processor can re-order the memory access instructions to improve performance. It can rearrange instructions to reduce the time processor takes to access memory. The order in which instructions are written is not guaranteed, and instead may be executed depending on the memory access cost of a given instruction. This approach does not impact single core machine but can negatively impact a multi-threaded program running on a multicore machine. In such situations, there are instructions to tell processors not to re-arrange memory access at a given point. The technical term for such instructions that restricts this re-arrangement is called "memory barriers". The `dmb` instruction in ARM64 acts as a barrier prohibiting the processor from moving an instruction across the fence. You can read more about it in [ARM developer docs](https://developer.arm.com/documentation/den0024/a/memory-ordering).

One of the way in which you can specify adding memory barrier in your code is by using  a [volatile variable](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/volatile). With `volatile`, it is guaranteed that the runtime, JIT, and the processor will not rearrange reads and writes to memory locations for performance. To make this happen, RyuJIT will emit `dmb` (data memory barrier) instruction for ARM64 every time there is an access (read/write) to a `volatile` variable.

For example, the following is code taken from [Perf_Volatile](https://github.com/dotnet/performance/blob/a5296dda39031ac84f40eeb5a0a136c89cde599b/src/benchmarks/micro/libraries/System.Threading/Perf.Volatile.cs#L17) microbenchmark. It does a volatile read of local field `_location`.

```csharp
public class Perf_Volatile
{
    private double _location = 0;
    
    [Benchmark]
    public double Read_double() => Volatile.Read(ref _location);
}
```

The generated relevant machine code of `Read_double` for ARM64 is:

```asm
; Read_double():double:this

        add     x0, x0, #8
        ldr     d0, [x0]
        dmb     ishld
```

The code first gets the address of `_location` field, loads the value in `d0` register and then execute `dmb ishld` that acts as a data memory barrier.

Although this guarantees the memory ordering, there is a cost associated with it. The processor must now guarantee that all the data access done before the memory barrier is visible to all the cores after the barrier instruction which could be time consuming. Hence, it is important to avoid or minimize the usage of such data access inside hot methods and loop as much as possible.

### ARM64 and big constants

In .NET 5, we did some improvements in the way we handled large constants present in user code. We started eliminating redundant loads of large constants in [dotnet/runtime#39096](https://github.com/dotnet/runtime/pull/39096) which gave us around **1%** (521K bytes to be precise) improvement in the size of ARM64 code we produced for all the .NET libraries.

It is worth noting that sometimes JIT improvements do not get reflected in the microbenchmark runs but are beneficial in overall code quality. In such cases, RyuJIT team reports the improvements that were made in terms of .NET libraries code size. RyuJIT is run on entire .NET library dlls before and after changes to understand how much impact the optimization has made, and which libraries got optimized more than others. As of preview 8, the emitted code size of entire .NET libraries for ARM64 target is 45 MB. **1%** improvement would mean we emit 450 KB less code in .NET 5, which is substantial. You can see the individual numbers of methods that were improved [here](https://github.com/dotnet/runtime/pull/39096#issuecomment-656859475).

#### Details
 
ARM64 has an Instruction set architecture (ISA) with fixed length encoding with each instruction exactly 32-bits long. Because of this, a move instruction `mov` have space only to encode up to 16-bits unsigned constant. To move a bigger constant value, we need to move the value in multiple steps using chunks of 16-bits (`movz/movk`). Due to this, multiple `mov` instructions are generated to construct a single bigger constant that need to be saved in a register. Alternately, in x64 a single `mov` can load bigger constant.

Now imagine code containing a couple constants (`2981231` and `2981235`).

```csharp
public static uint GetHashCode(uint a, uint b)
{
  return  ((a * 2981231) * b) + 2981235;
}
```

Before we optimized this pattern, we would generate code to construct each constant. So, if they are present in a loop, they would get constructed for every iteration.

```asm
        movz    w2, #0x7d6f
        movk    w2, #45 LSL #16  ; <-- loads 2981231 in w2
        mul     w0, w0, w2
        mul     w0, w0, w1
        movz    w1, #0x7d73
        movk    w1, #45 LSL #16  ; <-- loads 2981235 in w1
        add     w0, w0, w1
```

In .NET 5, we are now loading such constants once in a register and whenever possible, reusing them in the code. If there is more than one constant whose difference with the optimized constant is below a certain threshold, then we use the optimized constant that is already in a register to construct the other constant(s). Below, we used the value in register `w2` (`2981231` in this case) to calculate constant `2981235`.

```asm
        movz    w2, #0x7d6f
        movk    w2, #45 LSL #16  ; <-- loads 2981231
        mul     w0, w0, w2
        mul     w0, w0, w1
        add     w1, w2, #4       ; <-- loads 2981235
        add     w0, w0, w1
```

This optimization was helpful not just for loading constants but also for loading method addresses because they are 64-bits long on ARM64.

### C# structs

We made good progress in optimizing scenarios for ARM64 that returns C# struct and got **0.19%** code size improvement in .NET libraries. Before .NET 5, we always created a struct on stack before doing any operation on it. Any updates to its fields would do the update on stack. When returning, the fields had to be copied from the stack into the return register. Likewise, when a `struct` was returned from a method, we would store it on stack before operating on it. In .NET 5, we started enregistering structs that can be returned using multiple registers in [dotnet/runtime#36862](https://github.com/dotnet/runtime/pull/36862), meaning in certain cases, the structs won't be created on stack but will be directly created and manipulated using registers. With that, we omitted the expensive memory access in methods using structs. This was substantial work that improved scenarios that operate on stack.

The following measurements are in `nanoseconds` for [ReadOnlySpan&lt;T&gt; and Span&lt;T&gt; .ctor()](https://github.com/dotnet/performance/blob/bf83f43cc9fb262b45bd2200028f12c3e05d3155/src/benchmarks/micro/libraries/System.Memory/Constructors.cs#L15) microbenchmark that operate on `ReadOnlySpan<T>` and `Span<T>` structs.

| Benchmark                                                                         | .NET Core 3.1  | .NET 5       | Improvements |
|-----------------------------------------------------------------------------------|----------------|----------------|---------------|
| Constructors&lt;Byte&gt;.MemoryMarshalCreateSpan                    | 7.58 | 0.43 | -94%           |
| Constructors_ValueTypesOnly&lt;Int32&gt;.ReadOnlyFromPointerLength  | 7.22 | 0.43 | -94%           |
| Constructors&lt;Byte&gt;.ReadOnlySpanFromArray                      | 6.47 | 0.43  | -93%           |
| Constructors&lt;Byte&gt;.SpanImplicitCastFromArray                  | 4.26 | 0.41 | -90%           |
| Constructors_ValueTypesOnly&lt;Byte&gt;.ReadOnlyFromPointerLength   | 6.45 | 0.64 | -90%           |
| Constructors&lt;Byte&gt;.ArrayAsSpanStartLength                     | 4.02 | 0.4 | -90%           |
| Constructors&lt;String&gt;.ReadOnlySpanImplicitCastFromSpan         | 34.03 | 4.35  | -87%           |
| Constructors&lt;Byte&gt;.ArrayAsSpan                                | 8.34 | 1.48 | -82%           |
| Constructors&lt;Byte&gt;.ReadOnlySpanImplicitCastFromArraySegment   | 18.38 | 3.4 | -81%           |
| Constructors&lt;String&gt;.ReadOnlySpanImplicitCastFromArray        | 17.87 | 3.5 | -80%           |
| Constructors&lt;Byte&gt;.SpanImplicitCastFromArraySegment           | 18.62 | 3.88 | -79%           |
| Constructors&lt;String&gt;.SpanFromArrayStartLength                 | 50.9  | 14.27  | -72%           |
| Constructors&lt;String&gt;.MemoryFromArrayStartLength               | 54.31 | 16.23 | -70%           |
| Constructors&lt;String&gt;.ReadOnlySpanFromArrayStartLength         | 17.34 | 5.39 | -69%           |
| Constructors&lt;Byte&gt;.SpanFromMemory                             | 8.95 | 3.09 | -65%           |
| Constructors&lt;String&gt;.ArrayAsMemory                            | 53.56 | 18.54 | -65%           |
| Constructors&lt;Byte&gt;.ReadOnlyMemoryFromArrayStartLength         | 9.053 | 3.27 | -64%           |
| Constructors&lt;Byte&gt;.MemoryFromArrayStartLength                 | 9.060 | 3.3 | -64%           |
| Constructors&lt;String&gt;.ArrayAsMemoryStartLength                 | 53.00 | 19.31 | -64%           |
| Constructors&lt;String&gt;.SpanImplicitCastFromArraySegment         | 63.62 | 25.6 | -60%           |
| Constructors&lt;Byte&gt;.ArrayAsMemoryStartLength                   | 9.07 | 3.66 | -60%           |
| Constructors&lt;String&gt;.ReadOnlyMemoryFromArray                  | 9.06 | 3.7 | -59%           |
| Constructors&lt;Byte&gt;.SpanFromArray                              | 8.39 | 3.44 | -59%           |
| Constructors&lt;String&gt;.MemoryMarshalCreateSpan                  | 14.43 | 7.28 | -50%           |
| Constructors&lt;Byte&gt;.MemoryFromArray                            | 6.21 | 3.22 | -48%           |
| Constructors&lt;Byte&gt;.ReadOnlySpanFromMemory                     | 12.95 | 7.35 | -43%           |
| Constructors&lt;String&gt;.ReadOnlySpanImplicitCastFromArraySegment | 31.84 | 18.08  | -43%           |
| Constructors&lt;String&gt;.ReadOnlyMemoryFromArrayStartLength       | 9.06 | 5.52 | -39%           |
| Constructors&lt;Byte&gt;.ReadOnlyMemoryFromArray                    | 6.24 | 4.13  | -34%           |
| Constructors&lt;String&gt;.SpanFromMemory                           | 20.87 | 15.05 | -28%           |
| Constructors&lt;Byte&gt;.ReadOnlySpanImplicitCastFromArray          | 4.47 | 3.44  | -23%           |

<p/>

#### Details

In .NET Core 3.1, when a function created and returned a `struct` containing fields that can fit in a register like `float`, we were always creating and storing the `struct` on stack. Let us see an example:

```csharp
public struct MyStruct
{
  public float a;
  public float b;
}

[MethodImpl(MethodImplOptions.NoInlining)]
public static MyStruct GetMyStruct(float i, float j)
{
  MyStruct mys = new MyStruct();
  mys.a = i + j;
  mys.b = i - j;
  return mys;
}

public static float GetTotal(float i, float j)
{
  MyStruct mys = GetMyStruct(i, j);
  return mys.a + mys.b;
}

public static void Main()
{
  GetTotal(1.5f, 2.5f);
}
```

Here is the code we generated in .NET Core 3.1. If you see below, we created the `struct` on stack at location `[fp+24]` and then stored the `i+j` and `i-j` result in fields `a` and `b` located at `[fp+24]` and `[fp+28]` respectively. We finally loaded those fields from stack into the registers `s0` and `s1` to return the result. The caller `GetTotal()` would also save the returned `struct` on stack before operating on it.

```asm
; GetMyStruct(float,float):struct

        stp     fp, lr, [sp,#-32]!
        mov     fp, sp
        str     xzr, [fp,#24]	// [V02 loc0]
        add     x0, fp, #24	  ; <-- struct created on stack at [fp+24]
        str     xzr, [x0]
        fadd    s16, s0, s1
        str     s16, [fp,#24] ; <-- mys.a = i + j
        fsub    s16, s0, s1
        str     s16, [fp,#28] ; <-- mys.a = i - j
        ldr     s0, [fp,#24]  ; returning the struct field 'a' in s0
        ldr     s1, [fp,#28]  ; returning the struct field 'b' in s1
        ldp     fp, lr, [sp],#32
        ret     lr

; Total bytes of code 52, prolog size 12
; ============================================================

; GetTotal(float,float):float

        stp     fp, lr, [sp,#-32]!
        mov     fp, sp
        call    [GetMyStruct(float,float):MyStruct]
        str     s0, [fp,#24]   ; store mys.a on stack
        str     s1, [fp,#28]   ; store mys.b on stack
        add     x0, fp, #24    
        ldr     s0, [x0]       ; load again in register
        ldr     s16, [x0,#4]
        fadd    s0, s0, s16
        ldp     fp, lr, [sp],#32
        ret     lr

; Total bytes of code 44, prolog size 8

```

With the enregistration work, we do not create the `struct` on stack anymore in certain scenarios. With that, we do not have to load the field values from stack into the return registers. Here is the optimized code in .NET 5:

```asm
; GetMyStruct(float,float):MyStruct

        stp     fp, lr, [sp,#-16]!
        mov     fp, sp
        fadd    s16, s0, s1
        fsub    s1, s0, s1   ; s1 contains value of 'b'
        fmov    s0, s16      ; s0 contains value of 'a'
        ldp     fp, lr, [sp],#16
        ret     lr


; Total bytes of code 28, prolog size 8
; ============================================================

; GetTotal(float,float):float

        stp     fp, lr, [sp,#-16]!
        mov     fp, sp
        call    [GetMyStruct(float,float):MyStruct]
        fmov    s16, s1
        fadd    s0, s0, s16
        ldp     fp, lr, [sp],#16
        ret     lr

; Total bytes of code 28, prolog size 8
```

The code size has reduced by 43% and we have eliminated 10 memory accesses in `GetMyStruct()` and `GetTotal()` combined. The stack space needed for both the methods has also reduced from `32 bytes` to `16 bytes`.


 [dotnet/runtime#39326](https://github.com/dotnet/runtime/pull/39326) is a work in progress to similarly optimize fields of structs that are passed in registers, that we will ship in next release. We also found issues like [dotnet/runtime#35071](https://github.com/dotnet/runtime/issues/35071) where we do some redundant store and load when handling struct arguments or [HFA registers](https://docs.microsoft.com/en-us/cpp/build/arm64-windows-abi-conventions?view=vs-2019), or always push arguments on the stack before using them in a method as seen in [dotnet/runtime#35635](https://github.com/dotnet/runtime/issues/35635). We are hoping to address these issues in a future release.


### Array access with post-index addressing mode

ARM64 has various addressing modes that can be used to generate load/store instruction to compute the memory address an operation need to access. "Post-index" addressing mode is one of them. It is usually used in scenarios where consecutive access to memory location (from fixed base address) is needed. A typical example of it is array element access in a loop where the base address of an array is fixed and the elements are in consecutive memory at a fixed offset from one another. One of the issues we found out was that we were not using post-index addressing mode in our generated ARM64 code but instead generating a lot of instructions to calculate the address of array element. We will address [dotnet/runtime#34810](https://github.com/dotnet/runtime/issues/34810) in a future release.

#### Details

Consider a loop that stores a value in an array element.

```csharp
public int[] Test()
{
    int[] arr = new int[10];
    int i = 0;
    while (i < 9)
    {
        arr[i] = 1;  // <---- IG03
        i++;
    }
    return arr;
}
```

To store `1` inside `arr[i]`, we need to generate instructions to calculate address of `arr[i]` in every iteration. For example, on x64 this is as simple as:

```asm
...
M00_L00:
        movsxd   rcx, edx
        mov      dword ptr [rax+4*rcx+16], 1
        inc      edx
        cmp      edx, 9
        jl       SHORT M00_L00
...
```

`rax` stores the base address of array `arr`. `rcx` holds the value of `i` and since the array is of type `int`, we multiply it by `4`. `rax+4*rcx` forms the address of array element at `ith` index. `16` is the offset from base address at which elements are stored. All of this execute in a loop.

However, for ARM64, we generate longer code as seen below. We generate 3 instructions to calculate the array element address and 4th instruction to save the value. We do this calculation in every iteration of a loop.

```asm
...
M00_L00:
        sxtw    x2, w1        ; load 'i' from w1
        lsl     x2, x2, #2    ; x2 *= 4
        add     x2, x2, #16   ; x2 += 16
        mov     w3, #1        ; w3 = 1
        str     w3, [x0, x2]  ; store w3 in [x0 + x2]
        add     w1, w1, #1    ; w1++
        cmp     w1, #9        ; repeat while i < 9
        blt     M00_L00
...
```

With post-index addressing mode, much of the recalculation here can be simplified. With this addressing mode, we can auto increment the address present in a register to get the next array element. The code gets optimized as seen below. After every execution, contents of `x1` would be auto incremented by 4, and would get the address of the next array element.

```asm
; x1 contains <<base address of arr>>+16
; w0 contains value "1"
; w1 contains value of "i"

M00_L00:
        str     w0, [x1], 4  ; post-index addressing mode
        add     w1, w1, #1
        cmp     w1, #9
        blt     M00_L00
```

Fixing this issue will result in both performance and code size improvements.

### Mod operations

Modulo operations are crucial in many algorithms and currently we do not generate good quality code for certain scenarios.
In `a % b`, if `a` is an `unsigned int` and `b` is power of 2 and a constant, ARM64 code that is generated today is:

```asm
        lsr     w1, w0, #2
        lsl     w1, w1, #2
        sub     w0, w0, w1
```

But instead it can be optimized to generate:

```asm        
        and     w2, w0, <<b - 1>>
```

Another scenario that we could optimize is if `b` is a variable. Today, we generate:

```asm
        udiv    w2, w0, w1   ; sdiv if 'a' is signed int
        mul     w1, w2, w1
        sub     w0, w0, w1
```

The last two instructions can be combined into a single instruction to generate:
```asm
        udiv    w2, w0, w1
        msub    w3, w3, w1, w2
```

We will address [dotnet/runtime#34937](https://github.com/dotnet/runtime/issues/34937) in a future release.

-------------------------------------------------------
## Code size analysis

Understanding the size of ARM64 code that we produced and reducing it down was an important task for us in .NET 5. Not only does it improve the memory consumption of .NET runtime, it also reduces the disk footprint of R2R binaries that are compiled ahead-of-time.

We found some good areas where we could reduce the ARM64 code size and the results were astonishing. In addition to some of the work I mentioned above, after we optimized code generated for call indirects in [dotnet/runtime#35675](https://github.com/dotnet/runtime/pull/35675) and virtual call stub in [dotnet/runtime#36817](https://github.com/dotnet/runtime/pull/36817), we saw code size improvement of **13%** on .NET library R2R images. We also compared the ARM64 code produced in .NET Core 3.1 vs. .NET 5 for the [top 25 NuGet packages](https://www.nuget.org/stats). On average, we improved the code size of R2R images by **16.61%**. Below are the nuget package name and version along with the % improvement. All the measurements are in `bytes` (lower is better).

| Nuget package                        | Nuget version | .NET Core 3.1 | .NET 5  | Code size improvement |
|--------------------------------------|---------------|---------------|---------|-----------------------|
| Microsoft.EntityFrameworkCore   | 3.1.6         | 2414572       | 1944756 | -19.46%               |
| HtmlAgilityPack                 | 1.11.24       | 255700        | 205944  | -19.46%               |
| WebDriver                       | 3.141.0       | 330236        | 266116  | -19.42%               |
| System.Data.SqlClient           | 4.8.1         | 118588        | 96636   | -18.51%               |
| System.Web.Razor                | 3.2.7         | 474180        | 387296  | -18.32%               |
| Moq                             | 4.14.5        | 307540        | 251264  | -18.30%               |
| MongoDB.Bson                    | 2.11.0        | 863688        | 706152  | -18.24%               |
| AWSSDK.Core                     | 3.3.107.32    | 889712        | 728000  | -18.18%               |
| AutoMapper                      | 10.0.0        | 411132        | 338068  | -17.77%               |
| xunit.core                      | 2.4.1         | 41488         | 34192   | -17.59%               |
| Google.Protobuf                 | 3.12.4        | 643172        | 532372  | -17.23%               |
| xunit.execution.dotnet          | 2.4.1         | 313116        | 259212  | -17.22%               |
| nunit.framework                 | 3.12.0        | 722228        | 598976  | -17.07%               |
| Xamarin.Forms.Core              | 4.7.0.1239    | 1740552       | 1444740 | -17.00%               |
| Castle.Core                     | 4.4.1         | 389552        | 323892  | -16.86%               |
| Serilog                         | 2.9.0         | 167020        | 139308  | -16.59%               |
| MongoDB.Driver.Core             | 2.11.0        | 1281668       | 1069768 | -16.53%               |
| Newtonsoft.Json                 | 12.0.3        | 1056372       | 882724  | -16.44%               |
| polly                           | 7.2.1         | 353456        | 297120  | -15.94%               |
| StackExchange.Redis             | 2.1.58        | 1031668       | 867804  | -15.88%               |
| RabbitMQ.Client                 | 6.1.0         | 355372        | 299152  | -15.82%               |
| Grpc.Core.Api                   | 2.30.0        | 36488         | 30912   | -15.28%               |
| Grpc.Core                       | 2.30.0        | 190820        | 161764  | -15.23%               |
| ICSharpCode.SharpZipLib         | 1.2.0         | 306236        | 261244  | -14.69%               |
| Swashbuckle.AspNetCore.Swagger  | 5.5.1         | 5872          | 5112    | -12.94%               |
| JetBrains.Annotations           | 2020.1.0      | 7736          | 6824    | -11.79%               |
| Elasticsearch.Net               | 7.8.2         | 1904684       | 1702216 | -10.63%               |

Note that most of the above packages might not include R2R images, we picked these packages for our code size measurement because they are one of the most downloaded packages and written for wide variety of domains.

<p/>

### Inline heuristics tweaking
Currently, RyuJIT uses various heuristics to decide whether inlining a method will be beneficial or not. Among other heuristics, one of them is to check the code size of the caller in which the callee gets inlined. The code size heuristics is based upon [x64 code](https://github.com/dotnet/runtime/blob/e100d5ed292786284ef4f3ee678be5f7c43a0a53/src/coreclr/src/jit/inline.cpp#L1027) which has different characteristics than the ARM64 code. We explored some ways to fine tune it for ARM64 but did not see promising results. We will continue exploring these heuristics in future.

### Return address hijacking

While doing the code size analysis, we noticed that for small methods, ARM64 code includes [prologue](https://en.wikipedia.org/wiki/Function_prologue#Prologue) and [epilogue](https://en.wikipedia.org/wiki/Function_prologue#Epilogue) for every method, even though it is not needed. Often small methods get inlined inside the caller, but there may be scenarios where this might not happen. Consider a method `AdditionalCount()` that is marked as `NoInlining`. This method will not get inlined inside its caller. In this method, let us invoke the [Stack&lt;T&gt;.Count](https://github.com/dotnet/runtime/blob/8a2820e35c5ad841d4c3aae3af8b1ace37d22660/src/libraries/System.Collections/src/System/Collections/Generic/Stack.cs#L58) getter.

```csharp
[MethodImpl(MethodImplOptions.NoInlining)]
public static int AdditionalCount(Stack<string> a, int b)
{
    return a.Count + b;
}
```

Since there are no local variables in `AdditionalCount()`, nothing is retrieved from the stack and hence there is no need prepare and revert stack's state using prologue and epilogue. Below is the code generated for x64. If you notice, the x64 code for this method is 6 bytes long, with 0 bytes in prolog.

```asm
; AdditionalCount(System.Collections.Generic.Stack`1[[System.String, System.Private.CoreLib, Version=5.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]],int):int

        mov      eax, edx
        add      eax, dword ptr [rcx+16]
        ret

; Total bytes of code 6, prolog size 0
```

However, for ARM64, we generate prologue and epilogue even though nothing is stored or retrieved from stack. Also, if you see below, the code size is 24 bytes with 8 bytes in prologue which is bigger than x64 code size.

```asm
; AdditionalCount(System.Collections.Generic.Stack`1[[System.String, System.Private.CoreLib, Version=5.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]],int):int

        stp     fp, lr, [sp,#-16]!
        mov     fp, sp
        ldr     w0, [x0,#16]
        add     w0, w0, w1
        ldp     fp, lr, [sp],#16
        ret     lr

; Total bytes of code 24, prolog size 8
```

Our investigation showed that approximately **23%** of methods in the .NET libraries skip generating prologue/epilogue for x64, while for ARM64, we generate extra 16 bytes code for storing and retrieving `fp` and `lr` registers. We need to do this to support [return address hijacking](https://github.com/dotnet/runtime/blob/a21da8f6d945002bbb7cdb426c148867f60be528/docs/design/coreclr/jit/arm64-jit-frame-layout.md). If the .NET runtime needs to trigger garbage collection (GC), it needs to bring the user code execution to a safe point before it can start the GC. For ARM64, it has been done by generating prologue/epilogue in user's code to store the return address present in `lr` register on the stack and retrieve it back before returning. If the runtime decides to trigger GC while executing user code, it replaces the return address present on the stack with a runtime helper function address. When the method completes the execution, it retrieves the modified return address from the stack into `lr` and thus return to the runtime helper function so the runtime can perform GC. After GC is complete, control jumps back to the original return address of user code. All this is not needed for x64 code because the return address is already on stack and can be retrieved by the runtime. It may be possible to [optimize return address hijacking](https://github.com/dotnet/runtime/blob/d76ef042f8ead9d06a447ab2b1004ae626185ca2/src/coreclr/src/jit/codegencommon.cpp#L4886) for certain scenarios. In future release, we will do more investigation of [dotnet/runtime#35274](https://github.com/dotnet/runtime/issues/35274) to reduce the code size and improve speed of small methods.


### ARM64 code characteristics

Although there are various issues that we have identified and continue optimizing to improve the code size produced for ARM64, there are certain aspects of ARM ISA that cannot be changed and are worth mentioning here.

While x86 has [CISC](https://en.wikipedia.org/wiki/Complex_instruction_set_computer) and ARM is a [RISC](https://en.wikipedia.org/wiki/Reduced_instruction_set_computer) architecture, it is nearly impossible to have x86 and ARM target code size similar for the same method. ARM has fixed length encoding of 4-bytes in contrast to x86 which has variable length encoding. A return instruction `ret` on x86 can be as short as 1-byte, but on ARM64, it is always 4-bytes long. Because of fixed length encoding in ARM, there is a limited range of constant values that can be encoded inside an instruction as I mentioned in [ARM64 and big constants section](#arm64-and-big-constants). Any instruction that contains a constant bigger than 12-bits (sometimes 16-bits) must be moved to a register and operated through register. Basic arithmetic instructions like `add` and `sub` cannot operate on constant values that are bigger than 12-bits. Data cannot be transferred between memory to memory. It must be loaded in a register before transferring or operating on it. If there are any constants that need to be stored in memory, those constants must be moved in a register first before storing them to the memory. Even to do memory access using various addressing modes, the address has to be moved in a register before loading or storing data into it. Thus, at various places, there is a need to perform prerequisite or setup instructions to load the data in registers before performing actual operation. That all can lead to bigger code size on ARM64 targets.


-------------------------------------------------------

## Peephole analysis

The last topic that I would like to mention is our data-driven engineering approach in discovering and prioritizing some other important ARM64 code quality enhancements. When inspecting ARM64 code produced for .NET libraries with several benchmarks, we realized that there were several instruction patterns that could be replaced with better and more performant instructions. In compiler literature, ["peephole optimization"](https://en.wikipedia.org/wiki/Peephole_optimization) is the phase that does such optimizations. RyuJIT does not have peephole optimization phase currently. Adding a new compiler phase is a big task and can easily take a few months to get it right without impacting other metrics like JIT throughput. Additionally, we were not sure how much code size or speed up improvement such optimization would get us. Hence, we gathered data in an interesting way to discover and prioritize various opportunities in performing peephole optimization. We wrote a utility tool [AnalyzeAsm](https://github.com/dotnet/jitutils/tree/master/src/AnalyzeAsm) that would scan through approximately 1GB file containing ARM64 disassembly code of .NET library methods and report back the frequency of instruction patterns that we were interested in, along with methods in which they are present. With that information, it became easier for us to decide that a minimal implementation of peephole optimization phase was important. With `AnalyzeAsm`, we identified several peephole opportunities that would give us roughly **0.75%** improvement in the code size of the .NET libraries. In .NET 5, we optimized an instruction pattern by eliminating redundant opposite `mov` instructions in [dotnet/runtime#38179](https://github.com/dotnet/runtime/pull/38179) which gave us **0.28%** code size improvement. Percentage-wise, the improvements are not large, but they are meaningful in the context of the whole product.


### Details

I would like to highlight some of the peephole opportunities that we have found and hoping to address them in .NET 6.

#### Replace pair of "ldr" with "ldp"

If there are pair of consecutive load instructions `ldr` that loads data into a register from consecutive memory location, then the pair can be replaced by single load-pair instruction `ldp`.

So below pattern:

```asm
        ldr     x23, [x19,#16]
        ldr     x24, [x19,#24]
```

can be replaced with:

```asm
        ldp x1, x2, [x19, #16]
```

As seen in [dotnet/runtime#35130](https://github.com/dotnet/runtime/issues/35130) and [dotnet/runtime#35132](https://github.com/dotnet/runtime/issues/35132), `AnalyzeAsm` pointed out that this pattern occurs approximately **34,000** times in **16,000** methods.

#### Replace pair of "str" with "stp"

This is similar pattern as above, except that if there are pair of consecutive store instructions `str` that stores data from a register into consecutive memory location, then the pair can be replaced by single store-pair instruction `stp`.

So below pattern:

```asm
        str     x23, [x19,#16]
        str     x24, [x19,#24]
```

can be replaced with:

```asm
        stp x1, x2, [x19, #16]
```

As seen in [dotnet/runtime#35133](https://github.com/dotnet/runtime/issues/35133) and [dotnet/runtime#35134](https://github.com/dotnet/runtime/issues/35134), `AnalyzeAsm` pointed out that this pattern occurs approximately **35,000** times in **16,400** methods.

#### Replace pair of "str wzr" with "str xzr"

`wzr` is 4-byte zero register while `xzr` is an 8-byte zero register in ARM64. If there is a pair of consecutive instructions that stores `wzr` in consecutive memory location, then the pair can be replaced by single store of `xzr` value.
 
So below pattern:

```asm
        str     wzr, [x2, #8]
        str     wzr, [x2, #12]
```

can be replaced with:

```asm
        str     xzr, [x2, #8]
```

As seen in [dotnet/runtime#35136](https://github.com/dotnet/runtime/issues/35136), `AnalyzeAsm` pointed out that this pattern occurs approximately **450** times in **353** methods.

#### Remove redundant "ldr" and "str"

Another pattern that we were generating was loading a value from memory location into a register and then storing that value back from the register into same memory location. The second instruction was redundant and could be removed. Likewise, if there is a store followed by a load, it is safe to eliminate the second load instruction.

So below pattern:

```asm
        ldr     w0, [x19, #64]
        str     w0, [x19, #64]
```

can be optimized with:

```asm
        ldr     w0, [x19, #64]
```

As seen in [dotnet/runtime#35613](https://github.com/dotnet/runtime/issues/35613) and [dotnet/runtime#35614](https://github.com/dotnet/runtime/issues/35614) issues, `AnalyzeAsm` pointed out that this pattern occurs approximately **2570** times in **1750** methods. We are already in the process of addressing this optimization in [dotnet/runtime#39222](https://github.com/dotnet/runtime/pull/39222).

#### Replace "ldr" with "mov"

RyuJIT rarely generates code that will load two registers from same memory location, but we have seen that pattern in  library methods. The second load instruction can be converted to `mov` instruction which is cheaper and does not need memory access.

So below pattern:

```asm
        ldr     w1, [fp,#28]
        ldr     w0, [fp,#28]
```

can be optimized with:

```asm
        ldr     w1, [fp,#28]
        mov     w0, w1
```

As seen in [dotnet/runtime#35141](https://github.com/dotnet/runtime/issues/35141), `AnalyzeAsm `pointed out that this pattern occurs approximately **540** times in **300** methods.

#### Loading large constants using movz/movk 

Since large constants cannot be encoded in an ARM64 instruction as I [described above](#arm64-code-characteristics), we also found large number of occurrences of `movz/movk` pair (around **191028** of them in **4578** methods). In .NET 5, while some of these patterns are optimized by caching them as done in [dotnet/runtime#39096](https://github.com/dotnet/runtime/pull/39096), we are hoping to revisit other patterns and come up with a way to reduce them.

#### Call indirects and virtual stubs

Lastly, as I [mentioned above](#code-size-analysis), **14%** code size improvement in .NET libraries came from optimizing [call indirects](https://github.com/dotnet/runtime/pull/35675) and [virtual call stub](https://github.com/dotnet/runtime/pull/36817) in R2R code. It was possible to prioritize this from the data we obtained by using `AnalyzeAsm` on JIT disassembly of .NET libraries. It pointed out that the suboptimal pattern occurred approximately **615,700** times in **126,800** methods.

-------------------------------------------------------

## Techempower benchmarks

With all of the work that I described above and other work described in [this blog](https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-5/), we made significant improvement in ARM64 performance in Techempower benchmarks. The measurements below are for Requests / Second (higher is better)

| TechEmpower Platform Benchmark       | .NET Core 3.1 |  .NET 5 |    Improvements |
|--------------------------------------|---------------|----------|--------|
| JSON RPS              |       484,256 |   542,463 | +12.02% |
| Single Query RPS      |        49,663 |    53,392 | +7.51% |
| 20-Query RPS          |        10,730 |    11,114 | +3.58% |
| Fortunes RPS          |        61,164 |    71,528 | +16.95% |
| Updates RPS           |         9,154 |    10,217 | +11.61% |
| Plaintext RPS         |     6,763,328 | 7,415,041 | +9.64% |
| TechEmpower Performance Rating (TPR) |           484 |       538 | +11.16% |

-------------------------------------------------------

## Hardware

Here are the hardware details of machines we used to run the benchmarks I have covered in this blog.

### MicroBenchmarks

Our performance lab that runs microbenchmarks has following hardware configuration.

```
ARM64v8​
Memory:              96510MB ​
Architecture:        aarch64​
Byte Order:          Little Endian​
CPU(s):              46​
On-line CPU(s) list: 0-45​
Thread(s) per core:  1​
Core(s) per socket:  46​
Socket(s):           1​
NUMA node(s):        1​
Vendor ID:           Qualcomm​
Model:               1​
Model name:          Falkor​
Stepping:            0x0​
CPU max MHz:         2600.0000​
CPU min MHz:         600.0000​
BogoMIPS:            40.00​
L1d cache:           32K​
L1i cache:           64K​
L2 cache:            512K​
L3 cache:            58880K​
NUMA node0 CPU(s):   0-45​
Flags:               fp asimd evtstrm aes pmull sha1 sha2 crc32 cpuid asimdrdm
```
<p/>

### Techempower benchmarks

Our ASP.NET lab that runs techempower benchmarks has following hardware configuration.

```
Rack-Mount, 1U​
ThinkSystem HR330A​
1x 32-Core/3.0GHz eMAG CPU​
64GB DDR4 (8x8GB)​
1x 960GB NVMe M.2 SSD​
1x Single-Port 50GbE NIC​
2x Serial Ports​
1x 1GbE Management Port​
Ubuntu 18.04​
ARMv8​

Architecture:        aarch64​
Byte Order:          Little Endian​
CPU(s):              32​
On-line CPU(s) list: 0-31​
Thread(s) per core:  1​
Core(s) per socket:  32​
Socket(s):           1​
NUMA node(s):        1​
Vendor ID:           APM​
Model:               2​
Model name:          X-Gene​
Stepping:            0x3​
CPU max MHz:         3300.0000​
CPU min MHz:         363.9700​
BogoMIPS:            80.00​
L1d cache:           32K​
L1i cache:           32K​
L2 cache:            256K​
NUMA node0 CPU(s):   0-31
```
<p/>

## Conclusion

In .NET 5, we made great progress in improving the speed and code size for ARM64 target. Not only did we expose ARM64 intrinsics in .NET APIs, but also consumed them in our library code to optimize critical methods. With our data-driven engineering approach, we were able to prioritize high impacting work items in .NET 5. While doing performance investigation, we have also discovered several opportunities as summarized in [dotnet/runtime#35853](https://github.com/dotnet/runtime/issues/35853) that we plan to continue working for .NET 6. We had great partnership with [@TamarChristinaArm](https://github.com/TamarChristinaArm) from Arm Holdings who not only [implemented some of the ARM64 hardware intrinsics](https://github.com/dotnet/runtime/pulls?q=is%3Apr+author%3ATamarChristinaArm+is%3Aclosed), but also [gave valuable suggestions and feedback](https://github.com/issues?q=dotnet%2Fruntime+commenter%3ATamarChristinaArm+) to improve our code quality. We want to thank multiple contributors who made it possible to ship .NET 5 running on ARM64 target.

I would encourage you to download the latest bits of [.NET 5](https://dotnet.microsoft.com/download/dotnet/5.0) for ARM64 and let us know your feedback.

Happy coding on ARM64!