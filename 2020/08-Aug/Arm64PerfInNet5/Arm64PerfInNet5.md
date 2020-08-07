# ARM64 performance in .NET 5
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


## Optimization of .NET libraries using ARM64 intrinsics

[Optimize library code using ARM64 intrinsics](https://github.com/dotnet/runtime/issues/33308). Goal was to pick library methods that are already optimized for SSE2 or AVX2. Wanted to optimize them for ARM64.

Picked methods in following namespace:
- System.Collections.BitArray
- System.Runtime.Intrinsics.Vector64
- System.Runtime.Intrinsics.Vector128
- System.Numerics.BitOperations
- System.Numerics.Matrix4x4
- System.Buffers
- System.SpanHelpers
- System.Text.ASCIIUtility
- System.Text.Unicode
- System.Text.Encodings.Web


Benchmark results from [here](https://pvscmdupload.blob.core.windows.net/reports/08_06_2020/report_Daily_ca=ARM64_cb=master_co=Ubuntu1804ARM_cr=dotnetcoresdk_cc=CompliationMode=tiered-RunKind=micro_Baseline_bb=release-3.1.2xx_2020-08-06.html):

- BitArray
- BitOperations
- IndexOf

Talk which area they get impacted above along with result.


#### Details

In case you are interested, this is how it is done.

Give an example of `LeadingZeroCount` and link all the possible PRs.


### AOT compilation for methods having ARM64 intrinsics

Gather more examples around https://github.com/dotnet/runtime/pull/38060

Talk which top methods got impacted which resulted in faster start up.

## Opened ended investigation for code quality

### Benchmark analysis

Started looking at [Microbenchmarks](https://github.com/dotnet/performance/tree/master/src/benchmarks/micro) that is based upon [Benchmark.NET](https://github.com/dotnet/benchmarkdotnet).

- In 1300 micro benchmarks, Speed ratio of x64/arm64 varied from 2X ~ 50X.

- Profiler can't be used to compare microbenchmarks to spot for code quality issues

- Started focusing on "Windows ARM64" because IR that RyuJIT operates on is platform agnostics and any findings will have impact on Windows/Ubuntu.

- Inspect ARM64 code of benchmark and compare against x64 to see any difference.

#### Memory barries in ARM64

- Quick introduction and then example of volatile variable
- Hoisting volatile variable gave ~30% win

https://github.com/dotnet/runtime/pull/36697
https://github.com/dotnet/runtime/pull/37309
https://github.com/dotnet/runtime/pull/36976
https://github.com/dotnet/runtime/pull/34225

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
