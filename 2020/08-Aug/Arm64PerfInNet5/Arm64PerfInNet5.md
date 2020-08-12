# ARM64 performance work in .NET 5
**Kunal Pathak** (Kunal.Pathak@microsoft.com)

.NET Core team has done great amount of work to improve the performance of .NET 5. You can check it out in Stephen's [Performance Improvements in .NET5](https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-5/) blog. In the blog, I will describe the work our team has done to improve performance of .NET 5 for ARM64 and resulting outcome on some benchmarks.

## History of ARM and .NET

.NET Core has been working on ARM architecture for quite some time. .NET Core 2.0 was the first release to [announce support of ARM32](https://github.com/dotnet/announcements/issues/29). Back then, the legacy JIT engine of .NET was used to target ARM32, although the newer JIT engine "RyuJIT" was used to target x86 and x64. In December 2017, we made [RyuJIT as the default engine to generate ARM32 code](https://github.com/dotnet/coreclr/pull/15134). In .NET Core 2.1, we [added support of ARM32 on Linux](https://devblogs.microsoft.com/dotnet/announcing-net-core-2-1/) and had preview quality support for ARM64 on Linux .NET Core 3.0 was a big release for .NET Core in terms of ARM architecture. In that, not only did we extend ARM32 for Windows but also added [official support of ARM64 on Linux platform](https://devblogs.microsoft.com/dotnet/announcing-net-core-3-0/). In .NET 5, are working to target ARM64 for Windows. You can check out our progress [here](https://github.com/dotnet/runtime/issues/36699).


## Goals

While we have actually been working on ARM64 support in RyuJIT for over five years, most of the work that was done was to ensure that we generate functionally correct ARM64 code. We spent very little time in evaluating the quality of code RyuJIT produced for ARM64. As part of .NET 5, our focus was to perform investigation in this area and find out any obvious issues in RyuJIT that would improve the ARM64 code quality (CQ). Since Microsoft VC++ team already have ARM64 support, we also consulted them to understand the CQ issues that they encountered when doing similar exercise.

Although fixing CQ issues is crucial, sometimes its impact might not be noticable in an application. Hence we also wanted to make observable improvements in the performance of .NET Core applications that targets ARM64.

Here, I will describe our work to improve performance of ARM64 in these two areas:
- Planned optimizations done in .NET libraries.
- Evaluation of code quality produced by RyuJIT and resulting outcome.

## Implementation of hardware intrinsics in .NET libraries

In .NET Core 3.0, we introduced a new feature ["hardware intrinsics"](https://devblogs.microsoft.com/dotnet/hardware-intrinsics-in-net-core/) which gives access to various vectorized and non-vectorized hardware instructions that modern hardware support. .NET developers can access these instructions using set of APIs under namespace `System.Runtime.Intrinsics` ([msdn](https://docs.microsoft.com/en-us/dotNet/api/system.runtime.intrinsics?view=net-5.0)) and `System.Runtime.Intrinsics.X86` ([msdn](https://docs.microsoft.com/en-us/dotnet/api/system.runtime.intrinsics.x86?view=net-5.0)) for Intel x86/x64 architecture. In .NET Core 5.0, we added around 384 APIs under `System.Runtime.Intrinsics.Arm` ([msdn](https://docs.microsoft.com/en-us/dotnet/api/system.runtime.intrinsics.arm?view=net-5.0)) for ARM32/ARM64 architecture. This involved [implementing those APIs](https://github.com/dotnet/runtime/issues?q=is%3Aissue+label%3Aapi-approved+label%3Aarch-arm64+label%3Aarea-System.Runtime.Intrinsics+is%3Aclosed) and making RyuJIT aware of them so it can emit appropriate ARM32/ARM64 instruction. You can check the "hardware intrinsic" project progress [here](https://github.com/dotnet/runtime/projects/21).



## Improvements in .NET libraries using ARM64 hardware intrinsics

Today, we optimize many critical methods of .NET library using Intel x86/x64 intrinsics. Doing that improves the performance of such methods if running on Intel hardware supporting the intrinsic instructions. Code running on hardware that does not support the Intel intrinsics or running on other architectures like ARM, we fallback to slower implementation of those methods. In .NET 5, we [optimized most of those critical methods using ARM64 hardware intrinsics](https://github.com/dotnet/runtime/issues/33308) as well. So if your code uses any of those .NET library methods, they will see speed up boost running on ARM architecture. There might be several other methods that should be optimized, but in .NET 5 we focussed our attention to only those that are optimized using Intel intrinsics. In future, we can consider optimizing more methods using Intel/Arm64 intrinsics.

Here are list of classes whose methods we optimized (or will be optimized sooner) with ARM64 hardware intrinsics:

- [x] `System.Collections.BitArray`
- [x] `System.Runtime.Intrinsics.Vector64`
- [x] `System.Runtime.Intrinsics.Vector128`
- [x] `System.Numerics.BitOperations`
- [ ] `System.Numerics.Matrix4x4` (In progress)
- [ ] `System.Buffers`
- [x] `System.SpanHelpers`
- [ ] `System.Text.ASCIIUtility` (In progress)
- [x] `System.Text.Unicode`
- [x] `System.Text.Encodings.Web`

To see the improvements we get after we optimized these methods, here are some numbers for `System.Collections.BitArray`:

| Method name    | Benchmark                                                                                                                                                                                               | % improvement |
|----------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|---------------|
| `CopyTo()`     | [BitArrayCopyToIntArray(Size: 4)](https://github.com/dotnet/performance/blob/8aed638c9ee65c034fe0cca4ea2bdc3a68d2a6b5/src/benchmarks/micro/libraries/System.Collections/Perf.BitArray.cs#L142)          | -76%          |
| `Or()`         | [BitArrayOr(Size: 512)](https://github.com/dotnet/performance/blob/8aed638c9ee65c034fe0cca4ea2bdc3a68d2a6b5/src/benchmarks/micro/libraries/System.Collections/Perf.BitArray.cs#L100)                    | -72%          |
| `Not()`        | [BitArrayNot(Size: 512)](https://github.com/dotnet/performance/blob/8aed638c9ee65c034fe0cca4ea2bdc3a68d2a6b5/src/benchmarks/micro/libraries/System.Collections/Perf.BitArray.cs#L64)                    | -65%          |
| `CopyTo()`     | [BitArrayCopyToIntArray(Size: 512)](https://github.com/dotnet/performance/blob/8aed638c9ee65c034fe0cca4ea2bdc3a68d2a6b5/src/benchmarks/micro/libraries/System.Collections/Perf.BitArray.cs#L142)        | -65%          |
| `And()`        | [BitArrayAnd(Size: 512)](https://github.com/dotnet/performance/blob/8aed638c9ee65c034fe0cca4ea2bdc3a68d2a6b5/src/benchmarks/micro/libraries/System.Collections/Perf.BitArray.cs#L97)                    | -60%          |
| `Xor()`        | [BitArrayXor(Size: 512)](https://github.com/dotnet/performance/blob/8aed638c9ee65c034fe0cca4ea2bdc3a68d2a6b5/src/benchmarks/micro/libraries/System.Collections/Perf.BitArray.cs#L103)                   | -59%          |
| `.ctor()`      | [BitArrayIntArrayCtor(Size: 4)](https://github.com/dotnet/performance/blob/8aed638c9ee65c034fe0cca4ea2bdc3a68d2a6b5/src/benchmarks/micro/libraries/System.Collections/Perf.BitArray.cs#L55)             | -40%          |
| `.ctor()`      | [BitArrayBitArrayCtor(Size: 4)](https://github.com/dotnet/performance/blob/8aed638c9ee65c034fe0cca4ea2bdc3a68d2a6b5/src/benchmarks/micro/libraries/System.Collections/Perf.BitArray.cs#L37)             | -37%          |
| `.ctor()`      | [BitArrayBitArrayCtor(Size: 512)](https://github.com/dotnet/performance/blob/8aed638c9ee65c034fe0cca4ea2bdc3a68d2a6b5/src/benchmarks/micro/libraries/System.Collections/Perf.BitArray.cs#L37)           | -34%          |

Here are the numbers for `System.Numerics.BitOperations`:

| Method names          | Benchmarks                                                                                                                                                                                           | % improvement |
|-----------------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|---------------|
| `LeadingZeroCount()`  | [LeadingZeroCount_ulong](https://github.com/dotnet/performance/blob/454476401e17ed7f4d8b899ecf7661eb6cd63bad/src/benchmarks/micro/libraries/System.Numerics.BitOperations/Perf_BitOperations.cs#L32) | -90%          |
| `Log2()`              | [Log2_ulong](https://github.com/dotnet/performance/blob/454476401e17ed7f4d8b899ecf7661eb6cd63bad/src/benchmarks/micro/libraries/System.Numerics.BitOperations/Perf_BitOperations.cs#L56)             | -72%          |
| `PopCount()`          | [PopCount_ulong](https://github.com/dotnet/performance/blob/454476401e17ed7f4d8b899ecf7661eb6cd63bad/src/benchmarks/micro/libraries/System.Numerics.BitOperations/Perf_BitOperations.cs#L104)        | -64%          |
| `TrailingZeroCount()` | [TrailingZeroCount_uint](https://github.com/dotnet/performance/blob/454476401e17ed7f4d8b899ecf7661eb6cd63bad/src/benchmarks/micro/libraries/System.Numerics.BitOperations/Perf_BitOperations.cs#L68) | -51%          |

------> DO NOT MENTION Benchmark results from [here](https://pvscmdupload.blob.core.windows.net/reports/08_06_2020/report_Daily_ca=ARM64_cb=master_co=Ubuntu1804ARM_cr=dotnetcoresdk_cc=CompliationMode=tiered-RunKind=micro_Baseline_bb=release-3.1.2xx_2020-08-06.html):

Here are the numbers for `System.Numerics.Matrix4x4`:

| Method names          | Benchmarks                            | % improvement |
|-----------------------|---------------------------------------|---------------|
| `operator +()`        | [AddOperatorBenchmark]()              | -30%          |
| `operator ==()`       | [EqualityOperatorBenchmark]()         | -8%           |
| `operator !=()`       | [InequalityOperatorBenchmark]()       | -28%          |
| `operator *()`        | [MultiplyByMatrixOperatorBenchmark]() | -55%          |
| `operator *(scalar)`  | [MultiplyByScalarOperatorBenchmark]() | -16%          |
| `operator -()`        | [SubtractOperatorBenchmark]()         | -21%          |
| `operator negation()` | [NegationOperatorBenchmark]()         | -22%          |
| `Add()`               | [AddBenchmark]()                      | -22%          |
| `Lerp()`              | [LerpBenchmark]()                     | -41%          |
| `Multiply()`          | [MultiplyByMatrixBenchmark]()         | -46%          |
| `Multiply(scalar)`    | [MultiplyByScalarBenchmark]()         | -25%          |
| `Negate()`            | [NegateBenchmark]()                   | -21%          |
| `Subtract()`          | [SubtractBenchmark]()                 | -22%          |
| `Transpose()`         | [Transpose]()                         | -25%          |

TOOD: IndexOf, Encoding

### Details

Just to take an example of how bigger the impact from using ARM64 intrinsics is, lets take the following C# code which returns leading zero count of `value`.

```csharp
private int Test(uint value)
{
    return BitOperations.LeadingZeroCount(value);
}
```

Before optimization for ARM64, the code would execute the [software fallback](https://github.com/dotnet/runtime/blob/6072e4d3a7a2a1493f514cdf4be75a3d56580e84/src/libraries/System.Private.CoreLib/src/System/Numerics/BitOperations.cs#L205) of `LeadingZeroCount()`. If you see the ARM64 assembly code generated below, not only it is large, but RyuJIT had to JIT 2 methods - `Test(int)` and `Log2SoftwareFallback(int)`.

<details>
<summary>Suboptimal assembly code generated in .NET 3.1</summary>

```asm

; Assembly listing for method BitOperationsTest.TestClass:Test(int):int
; Emitting BLENDED_CODE for generic ARM64 CPU - Windows
; optimized code
;
; Lcl frame size = 0

G_M29785_IG01:
        A9BF7BFD          stp     fp, lr, [sp,#-16]!
        910003FD          mov     fp, sp

G_M29785_IG02:
        35000060          cbnz    w0, G_M29785_IG04

G_M29785_IG03:
        52800400          mov     w0, #32
        14000003          b       G_M29785_IG05

G_M29785_IG04:
        97FF9795          bl      System.Numerics.BitOperations:Log2SoftwareFallback(int):int
        52001000          eor     w0, w0, #31

G_M29785_IG05:
        A8C17BFD          ldp     fp, lr, [sp],#16
        D65F03C0          ret     lr

; Total bytes of code 28, prolog size 8, PerfScore 8.30, (MethodHash=2e738ba6) for method BitOperationsTest.TestClass:Test(int):int
; ============================================================


; Assembly listing for method System.Numerics.BitOperations:Log2SoftwareFallback(int):int
; Emitting BLENDED_CODE for generic ARM64 CPU - Windows
; optimized code
;
; Lcl frame size = 0

G_M25770_IG01:
        A9BF7BFD          stp     fp, lr, [sp,#-16]!
        910003FD          mov     fp, sp

G_M25770_IG02:
        53017C01          lsr     w1, w0, #1
        2A010000          orr     w0, w0, w1
        53027C01          lsr     w1, w0, #2
        2A010000          orr     w0, w0, w1
        53047C01          lsr     w1, w0, #4
        2A010000          orr     w0, w0, w1
        53087C01          lsr     w1, w0, #8
        2A010000          orr     w0, w0, w1
        53107C01          lsr     w1, w0, #16
        2A010000          orr     w0, w0, w1
        52959BA1          movz    w1, #0xacdd
        72A0F881          movk    w1, #0x7c4 LSL #16
        1B017C00          mul     w0, w0, w1
        531B7C00          lsr     w0, w0, #27
        93407C00          sxtw    x0, w0
        D2984921          movz    x1, #0xc249
        F2AA80A1          movk    x1, #0x5405 LSL #16
        F2CFFF81          movk    x1, #0x7ffc LSL #32
        38617800          ldrb    w0, [x0, x1]

G_M25770_IG03:
        A8C17BFD          ldp     fp, lr, [sp],#16
        D65F03C0          ret     lr


; Total bytes of code 92, prolog size 8, PerfScore 27.70, (MethodHash=172e9b55) for method System.Numerics.BitOperations:Log2SoftwareFallback(int):int
; ============================================================
```
</details>

After we optimized `LeadingZeroCount()` to use ARM64 intrinsics, generated code for ARM64 is just handful of instructions (including the crucial `clz`). In this case, RyuJIT didn't even JIT `Log2SoftwareFallback(int)` method because it was not called. Thus, we got improvement in code quality as well as JIT throughput.

<details>
<summary>Optimal assembly code generated in .NET 5</summary>

```asm
; Assembly listing for method BitOperationsTest.TestClass:Test(int):int
; Emitting BLENDED_CODE for generic ARM64 CPU - Windows
; optimized code
;
; Lcl frame size = 0

G_M29785_IG01:
        A9BF7BFD          stp     fp, lr, [sp,#-16]!
        910003FD          mov     fp, sp

G_M29785_IG02:
        5AC01000          clz     w0, w0

G_M29785_IG03:
        A8C17BFD          ldp     fp, lr, [sp],#16
        D65F03C0          ret     lr

; Total bytes of code 24, prolog size 8, PerfScore 6.90, (MethodHash=2e738ba6) for method BitOperationsTest.TestClass:Test(int):int
; ============================================================
```
</details>

</p>

### AOT compilation for methods having ARM64 intrinsics

In .NET, a program can be compiled to machine code during runtime using what we known as JIT (just-in-time). The target machine code produced is very efficient but has little disadvantage of having to do the compilation during execution and this might add some delay during the start-up. If the target platform is known in advance, some .NET developer prefer creating ready to run images for target platform using AOT (ahead-of-time)  compilation. It has an advantage of faster startup time because there is no need to produce machine code during execution. The target machine code is already present in the binary and can be run directly. AOT compiled code might be suboptimal sometimes, but get replaced by optimal code eventually.

Earlier, if a method (.NET framework library method or user defined method) had calls to ARM64 hardware intrinsic APIs (APIs under `System.Runtime.Intrinsics` and `System.Runtime.Intrinsics.Arm`), such methods were never compiled AOT and were always deferred to get compiled during runtime. This had an impact on start-up time of some .NET apps which used one of these methods in their startup code. We [addressed this problem](https://github.com/dotnet/runtime/pull/38060) in .NET 5 and now able to do the compilation of such methods AOT.

## Benchmark analysis

As mentioned earlier, apart from optimizing .NET library with intrinsics, we also wanted to evaluate CQ of ARM64. In order to do that, we wanted to pick benchmarks that can easily highlight underlying ARM64 CQ issues. [TechEmpower](https://www.techempower.com/) was a good starting point, but intially, we wanted something simpler to investigate and reason about ARM64 code. Hence, we picked [Microbenchmarks](https://github.com/dotnet/performance/tree/master/src/benchmarks/micro) that are based upon [Benchmark.NET](https://github.com/dotnet/benchmarkdotnet). It has around 1300 benchmarks and are run daily to do various comparisons. You can check the daily report at https://aka.ms/dotnetperfindex.

We decided to compare ARM64 performance of those benchmarks with x64. Improving ARM64 performance to match that of x64 was not our goal, but to understand the outliers and know which benchmarks are slower than others. Once we identified the slower benchmarks, we wanted to check why they run slow on ARM64 target. We tried using some profilers like [WPA](https://docs.microsoft.com/en-us/windows-hardware/test/wpt/windows-performance-analyzer) and [PerfView](https://github.com/microsoft/perfview) but they were not useful in this scenario. Those profilers would have pointed out the hottest method in given benchmark. But since MicroBenchmarks are tiny benchmarks with at most 1~2 method, the hottest method that the profiler pointed was mostly the benchmark method itself. Hence, to understand the ARM64 CQ issues, we decided to just inspect the assembly code produced for a given benchmark and compare it against that produced for x64. That would help us identify basic issues in RyuJIT's ARM64 code generator.

Below, I will describe some of the issues that we found out with this exercise.

### Memory barries in ARM64

Through some of the benchmarks, we noticed that we were accessing `volatile` variable in hot loop of critical methods of `System.Collections.Concurrent.ConcurrentDictionary` class. Accessing `volatile` variable for ARM64 is expensive because they introduce memory barrier instructions. By caching the volatile variable and storing it in a local variable ([here](https://github.com/dotnet/runtime/pull/34225), [here](https://github.com/dotnet/runtime/pull/36976) and [here](https://github.com/dotnet/runtime/pull/37081)) outside such loops gave us good performance wins as seen below.




| Method names      | Benchmarks                                                                       | % improvement |
|-------------------|----------------------------------------------------------------------------------|---------------|
| `get_Count`       | [Count<Int32>.Dictionary(Size: 512)](https://github.com/dotnet/performance/blob/a5296dda39031ac84f40eeb5a0a136c89cde599b/src/benchmarks/micro/libraries/System.Collections/Concurrent/Count.cs#L37)                 | -40%          |
| `TryGetValue()`   | [TryGetValueTrue<Int32, Int32>.ConcurrentDictionary(Size: 512)](https://github.com/dotnet/performance/blob/a5296dda39031ac84f40eeb5a0a136c89cde599b/src/benchmarks/micro/libraries/System.Collections/TryGetValue/TryGetValueTrue.cs#L95) | -37%          |
| `ctor()`          | [CtorFromCollection<Int32>.ConcurrentDictionary(Size: 512)](https://github.com/dotnet/performance/blob/a5296dda39031ac84f40eeb5a0a136c89cde599b/src/benchmarks/micro/libraries/System.Collections/Create/CtorFromCollection.cs#L60)   | -33%          |
| `IsEmpty(string)` | [IsEmpty<String>.Dictionary(Size: 512)](https://github.com/dotnet/performance/blob/a5296dda39031ac84f40eeb5a0a136c89cde599b/src/benchmarks/micro/libraries/System.Collections/Concurrent/IsEmpty.cs#L37)           | -33%          |
| `IsEmpty(int)`    | [IsEmpty<Int32>.Dictionary(Size: 512)](https://github.com/dotnet/performance/blob/a5296dda39031ac84f40eeb5a0a136c89cde599b/src/benchmarks/micro/libraries/System.Collections/Concurrent/IsEmpty.cs#L37)             | -28%          |
| `Add(), Clear()`  | [CreateAddAndClear<Int32>.ConcurrentDictionary(Size: 512)](https://github.com/dotnet/performance/blob/a5296dda39031ac84f40eeb5a0a136c89cde599b/src/benchmarks/micro/libraries/System.Collections/CreateAddAndClear.cs#L168)    | -28%          |
| `TryAdd()`        | [TryAddDefaultSize<Int32>.ConcurrentDictionary(Count: 512)](https://github.com/dotnet/performance/blob/a5296dda39031ac84f40eeb5a0a136c89cde599b/src/benchmarks/micro/libraries/System.Collections/Add/TryAddDefaultSize.cs#L39)   | -26%          |
| `TryAdd()`        | [TryAddGivenSize<Int32>.ConcurrentDictionary(Count: 512)](https://github.com/dotnet/performance/blob/a5296dda39031ac84f40eeb5a0a136c89cde599b/src/benchmarks/micro/libraries/System.Collections/Add/TryAddGivenSize.cs#L39)    | -10%          |
| `TryAdd()`        | [AddGivenSize<Int32>.ConcurrentDictionary(Size: 512)](https://github.com/dotnet/performance/blob/a5296dda39031ac84f40eeb5a0a136c89cde599b/src/benchmarks/micro/libraries/System.Collections/Add/AddGivenSize.cs#L116)         | -10%          |

Other places where we did similar optimization was in [System.Threading.ThreadPool](https://github.com/dotnet/runtime/pull/36697) and [System.Diagnostics.Tracing.EventCount](https://github.com/dotnet/runtime/pull/37309) classes. 



#### Details
ARM architecture has weakly ordered memory model. The processor can re-order the memory access instructions to improve performance of the processor. It can rearrange instructions to reduce the time processor takes to access memory. The order in which user has written the code is not guaranteed to be executed in same order and can be weakly defined depending on the memory access cost of given instruction. This approach doesn't impact single core machine but can impact adversely a multi-threaded program running on a multicore machine.
In such situations, there are instructions to tell processors not to re-arrange memory access at a given point inside code. The technical term for such instructions that restricts this re-arrangement is called "memory barriers". The `dmb` instruction in ARM64 acts as a barrier prohibiting the processor from moving an instructions across the fence. You can read more about it in [ARM developer docs](https://developer.arm.com/documentation/den0024/a/memory-ordering).

One of the way in which .NET developer can specify adding memory barrier in their code is by using [volatile variable](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/volatile) in C#. With `volatile` variable, it is guaranteed that the runtime, JIT or the processor will not rearrange reads and writes to memory locations for performance. To make this happen, RyuJIT would emit `dmb` (data memory barrier) instruction for ARM64 every time there is an access (read/write) to a `volatile` variable. 

For example, below is the C# code taken from [microbenchmarks](https://github.com/dotnet/performance/blob/a5296dda39031ac84f40eeb5a0a136c89cde599b/src/benchmarks/micro/libraries/System.Threading/Perf.Volatile.cs#L17). It does a volatile read of local field `_location`.

```csharp
public class Perf_Volatile
{
    private double _location = 0;
    
    [Benchmark]
    public double Read_double() => Volatile.Read(ref _location);
}
```

The generated relevant machine code of `Read_double` for ARM64 is:

```
; Assembly listing for method Program:Read_double():double:this
; Emitting BLENDED_CODE for generic ARM64 CPU - Windows

G_M49790_IG02:
        91002000          add     x0, x0, #8
        FD400000          ldr     d0, [x0]
        D50339BF          dmb     ishld
```

The code first gets the address of `_location` field, loads the value in `d0` register and then execute `dmb ishld` that acts as a data memory barrier.

Although this guarantees the memory ordering, there is a cost for it. The processor must now guarantee that all the data access done before the memory barrier is visible to all the cores after the barrier instruction. This means that the barrier requires all memory operations to complete before letting the cores cross the barrier instruction which could be time consuming. Hence, it is important to avoid or minimize the usage of such data access inside hot methods and loop as much as possible.




#### Mod operations
NOT DONE

#### C# structs
NOT DONE

#### ARM64 and big constants

#### Array access
NOT DONE

#### Range check elimination
NOT DONE

### Code size analysis

- We improved our Ready-to-run code size by approx. 15% on framework libraries.

- Nuget code size findings

#### Details

- Talk about ARM64 code characteristics
- Indirect and virtual stub calls
- Return address hijacking (NOT DONE)
- Talk about inline heuristics (NOT DONE)

### Peephole analysis

0.25% code size win

- AnalyzeAsm tool
- Describe several issues opened for peephole

## Conclusion

TODO:
- Link the epic issue and make sure to cover all the issues mentioned in it.
- Use "We" as much as possible instead of "I".
- Describe hardware
- Engagement with Tamar from ARM holdings and examples of his valuable feedback.
- Pick bechmarks from Stephen's blog
- Format (STAR)
  - Motive
  - How we approached
  - What was result?
