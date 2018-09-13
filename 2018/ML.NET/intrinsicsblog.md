# Accelerating machine learning: Summer internship project with the .NET Team

This summer, I was delighted to be given an opportunity to contribute to [ML.NET](https://www.microsoft.com/net/learn/apps/machine-learning-and-ai/ml-dotnet), an open-source initiative introduced in Microsoft Build 2018 to empower .NET developers with machine learning tools.  As a software engineering intern in the .NET team under my manager, Dan, and my mentors, Eric and Santi, I have two goals in my project:

1. **Enable [ML.NET](https://www.microsoft.com/net/learn/apps/machine-learning-and-ai/ml-dotnet) to run on more architectures** (x64, ARM32, ARM64, etc.) by replacing native dependencies of CPU math operations with managed code
2. **Accelerate machine learning** by enabling more efficient hardware vectorization using AVX (Advanced Vector Extensions) SIMD (single-instruction, multiple-data) hardware intrinsics on x86 architectures

By achieving the first goal, the machine learning task of classifying iris flowers in the [Getting Started with ML.NET](https://www.microsoft.com/net/learn/machine-learning-and-ai/get-started-with-ml-dotnet-tutorial) tutorial and many more can now run on Raspberry Pi.  By achieving the second goal, we see 
- a **+13.88%** end-to-end performance improvement on average in the training time of logistic regression and K-means clustering models, and 
- a **+17.78%** unit performance improvement on average in each function that calls hardware intrinsics to handle a CPU math operation.

With unit tests and performance tests implemented with [Benchmark.NET](https://benchmarkdotnet.org/index.html), the new code delivers correctness and the aforementioned performance improvement in both the high-level ML.NET user scenarios and the low-level CPU math operations, without compromising memory allocation.

Leveraging the [multitargeting](https://docs.microsoft.com/en-us/visualstudio/msbuild/msbuild-multitargeting-overview?view=vs-2017) feature of MSBuild, this project implements two independent code paths of CPU math operations that branch out from the same partial class (`CpuMathUtils`) on two target frameworks:
1. On **.NET Standard 2.0**, the system picks up the original native implementation with SSE (Streaming SIMD Extensions) hardware intrinsics, while
2. On **.NET Core App 3.0**, the system follows the new managed implementation with AVX hardware intrinsics.

In other words, since .NET Core App 3.0 has not been released as of the time of writing, the functionality of existing projects on .NET Standard 2.0 remains unchanged even after incorporating changes from this project.  Only on .NET Core App 3.0 can we see the performance improvement brought about by the managed code using AVX hardware intrinsics.

## Main challenges

Not only does porting existing native code to managed code expand the coverage of [ML.NET](https://www.microsoft.com/net/learn/apps/machine-learning-and-ai/ml-dotnet) support, it also saves future cost to maintain and improve the code for CPU math operations.  In addition, enabling the system to opt for the more efficient AVX hardware intrinsics whenever possible instead of SSE ones gives CPU math operations a performance boost.  Main challenges lie in
- leveraging the new data structure, `Span<T>`, introduced in C# 7.3, in the base-layer implementation of CPU math operations in C#,
- streamlining the switching between AVX and SSE technologies, as well as the services to overloaded public APIs,
- coping with fundamental differences between managed and native code in their ways to handle pointers, and
- removing alignment assumptions in the implementation of CPU math operations with AVX hardware intrinsics.


## SIMD instructions as vectorization

To speed up machine learning, we could apply the same operation to multiple elements of an array simultaneously instead of only one element.  This is called vectorization.  To implement vectorization in this project, SIMD (single-instruction, multi-data) instructions in the form of SSE and AVX hardware intrinsics are called in functions that implement CPU math operations on input array-like objects.

## What are SSE and AVX?

SSE (Streaming SIMD Extensions) and AVX (Advanced Vector Extensions) are SIMD instruction set extensions to the x86 architecture.  Most x86 architectures nowadays support both SSE and AVX.

AVX can be understood as being more advanced and efficient than SSE.  AVX handles 8 consecutive 32-bit elements in memory in one instruction, while SSE handles only 4.  As a result, there should be performance gains when the project upgrades the original SSE implementation in native code to the new AVX implementation in managed code, given that the transition from native code to managed code does not incur much of a cost.

When neither AVX nor SSE is supported, e.g. on Raspberry Pi, software fallbacks that implement CPU math operations in a naive approach should be provided to ensure correct functionality [Figure 1].

Figure 1: (Improving implementations of CPU math operations through transitions) Transitions.png

## How is the project implemented?

Originally, every trainer, learner, and transform used in machine learning ultimately calls a `SseUtils` wrapper method that performs a CPU math operation on input arrays, such as
- `MatMulDense`, which takes the matrix multiplication of two dense arrays interpreted as matrices, and 
- `SdcaL1UpdateSparse`, which performs the update step of the stochastic dual coordinate ascent for sparse arrays.

These wrapper methods assume preference for SSE SIMD instructions, and calls a corresponding method in another class `Thunk`, which serves as the interface between managed and native code and contains methods that directly call their native equivalents through [P/Invoke](https://msdn.microsoft.com/en-us/library/55d3thsc.aspx) calls.  These native methods in `.cpp` files in turn implement the CPU math operations with loops containing SSE hardware intrinsics that perform SIMD instructions.

The original implementation provides room for enhancement in the following areas:
1. Can we simplify the layers between the `SseUtils` wrapper class and the bottom-level hardware intrinsics?
2. Can we remove native dependencies to support more architectures by implementing hardware intrinsics in managed code?
3. Can we leverage the more efficient AVX hardware intrinsics whenever the AVX technology is supported?

Following these trains of thought, this project answers "yes" to all three questions by adding a new independent code path for CPU math operations that becomes active on .NET Core App 3.0, and by keeping the original code path running on .NET Standard 2.0. All previous call sites of `SseUtils` methods now call `CpuMathUtils` methods of the same name instead, thanks to an effort to keep the public API signatures of CPU math operations consistent [Figure 2].

Figure 2: (Change of top-most wrapper class for CPU math operations) Utils.png

`CpuMathUtils` is a new partial class that contains two definitions for each public API representing CPU math operation, one of which is compiled only on .NET Standard 2.0 while the other, only on .NET Core App 3.0.  This conditional compilation feature creates two independent code paths for `CpuMathUtils` methods.  Those function definitions compiled on .NET Standard 2.0 call their `SseUtils` counterparts directly, which essentially follow the original native code path.  On the other hand, the other function definitions compiled on .NET Core App 3.0 go through a switching paradigm between three implementations of the same CPU math operation through:
1. an `AvxIntrinsics` method which implements the operation with loops containing AVX hardware intrinsics,
2. a `SseIntrinsics` method which implements the operation with loops containing SSE hardware intrinsics, and
3. a software fallback in case neither AVX nor SSE is supported [Figure 3].

Figure 3: (Switching paradigm of `CpuMathUtils` methods) SwitchingParadigm.png

Since the `AvxIntrinsics` and `SseIntrinsics` methods in managed code directly implement the CPU math operations in analogy to the native methods originally in `.cpp` files, the code change not only removes native dependencies but also simplifies the levels of abstraction between public APIs and base-layer hardware intrinsics.  

With the replacement of native code in CPU math operations, it is shown in a real-time demonstration that the machine learning task of classifying iris flowers in the [Getting Started with ML.NET](https://www.microsoft.com/net/learn/machine-learning-and-ai/get-started-with-ml-dotnet-tutorial) tutorial can now run on Raspberry Pi, which is originally not possible with .NET Standard 2.0.  A more complex experiment also shows that we can now train models with stochastic dual coordinate ascent, conduct hyperparameter tuning, and perform cross validation to select the best model for the best prediction of the numbers of goals England and Sweden receive when they play against each other in World Cup 2018, all on Raspberry Pi.

Meanwhile, the switching paradigm allows the system to use the more efficient AVX implementation whenever possible.  Since most x86 architectures nowadays support AVX, we can expect a noticeable performance gain from these changes.

Figure 4: (Illustration of new code architecture) Code.png

## Performance improvements

As CPU math operations implemented with SSE hardware intrinsics originally in native code are re-written in managed code, the cost of this native-managed transition lies low at only 2.90% on average [Figure 5], while upgrading SSE to AVX in managed code delivers a significant performance boost of +20.23% on average [Figure 6].  Overall speaking, the entire transiton from the SSE implementation in native code to the AVX implementation in managed code gives a **+17.78%** performance boost on average to CPU math operations in [ML.NET](https://www.microsoft.com/net/learn/apps/machine-learning-and-ai/ml-dotnet) [Figure 7].

Figure 5: NativeManaged.png

Figure 6: SseAvx.png

Figure 7: Overall.png

In the best cases, some CPU math operations enjoy a +42.32% improvement in the running time, while some others involving sparse inputs have potential for further optimization.

Beyond unit performance changes at the API level, we are also interested in performance changes in end-to-end user scenarios of [ML.NET](https://www.microsoft.com/net/learn/apps/machine-learning-and-ai/ml-dotnet) in real life.  On .NET Core App 3.0, training models of K-means clustering and logistic regression benefits from a performance gain of **+13.88%** on average, without compromizng much of memory allocation (only less than 0.3% on average) [Figure 8].

Figure 8: TrainTime.png

This **+13.88%** performance improvement reflects the expected speed-up [ML.NET](https://www.microsoft.com/net/learn/apps/machine-learning-and-ai/ml-dotnet) users experience when they adopt changes from this project on .NET Core App 3.0.

## Epilogue

My summer internship experience with the .NET team has been exceptionally rewarding and inspiring to me.  My manager and mentors has given me a precious opportunity to go hands-on with a project that gets directly incorporated into existing products that are constantly updated and shipped.  As a bonus, this project ends up with tangible positive impacts, including the +13.88% average performance boost to [ML.NET](https://www.microsoft.com/net/learn/apps/machine-learning-and-ai/ml-dotnet) users and extended support to more architectures, which could be a testimony to 12 weeks of efficient and productive teamwork.  

There are numerous times when I get to work with other teams and external industry partners to optimize my project, which turns out to be a pleasant surprise.  Most importantly, as a software engineering intern with the .NET team, I am exposed to almost every step of the entire working cycle of a product enhancement, from idea generation to code review to product release with documentation.  This feature gives me not only a personal experience of real-life software engineering but also a great sense of responsbility for my project, which could be very satisfying when the project makes a positive contribution to [ML.NET](https://www.microsoft.com/net/learn/apps/machine-learning-and-ai/ml-dotnet) users in real life.