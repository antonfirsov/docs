---
title: Profile-guided optimization in .NET Core 2.0
weblogName: .NET Blog
customFields:
  authors:
    key: authors
    value: Bertrand Le Roy, Daniel Podder
---
.NET Core 2.0 introduces many new optimizations that will make your code even faster. [A lot of work has been done in the base class library to improve performance](https://blogs.msdn.microsoft.com/dotnet/2017/06/07/performance-improvements-in-net-core/), but in this post, we'd like to talk about a specific category of optimization: profile-guided optimization (or PGO, pronounced "pogo").

## What is profile-guided optimization?

PGO is the usage of runtime profile data to optimize code compilation. By relying on runtime data, PGO can focus optimization work on those code paths that are most frequently used. Of course, the choice of the applications used for profiling matters, as it will determine what will get optimized. The further an application diverges from what was used to profile, the less likely it is to benefit from the optimizations. In the case of just-in-time compiled frameworks such as .NET, it's however possible to recompile code on-the-fly to optimize an application as it's running.

## PGO in .NET Core 2

*Details of the implementation, on Windows and Linux*
*What about .NET Framework?*

## Results

*results and caveats, with graphs*

## How to profile and optimize your own application?

*Is this something people will be reasonably able to do?*

## Conclusion

.NET Core 2.0 is an important performance release of [an already very fast platform](https://www.techempower.com/benchmarks/#section=data-r14&hw=ph&test=plaintext). We're committed to continuing on that trend, and make .NET the fastest general purpose development environment. PGO is an integral part of this, and an important tool that is going to significantly improve the performance of your .NET Core applications.