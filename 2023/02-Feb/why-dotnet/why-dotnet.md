---
post_title: "What is .NET, and why should you choose it?"
author1: dotnet
post_slug: why-dotnet
username: dotnet
microsoft_alias: rlander
featured_image: whatisdotnet.png
categories: .NET, .NET Fundamentals, .NET Internals, C#, Performance, Security
summary: .NET has changed a lot over the last few years. Learn why you should choose it for your next project.
desired_publication_date: 2023-02-14
post_date: 2023-02-14 08:00:00
---

.NET has changed a lot since we kicked off the fast-moving [.NET open-source and cross-platform project](https://github.com/dotnet/runtime). We've re-thought and refined the platform, adding new low-level capabilities designed for performance and safety, paired with higher-level productivity-focused features. [`Span<T>`](https://learn.microsoft.com/archive/msdn-magazine/2018/january/csharp-all-about-span-exploring-a-new-net-mainstay), [hardware intrinsics](https://devblogs.microsoft.com/dotnet/hardware-intrinsics-in-net-core/), and [nullable reference types](https://learn.microsoft.com/dotnet/csharp/nullable-references) are examples. We're kicking off a new ".NET Design Point" blog series to explore the fundamentals and design choices that define today's .NET platform, and how they benefit the code you are writing now.

This first post in the series provides a breadth overview of the pillars and the design-point of the platform. It describes "what you get" at a foundational level when you choose .NET and is intended to be a sufficient and facts-focused framing that you can use to describe the platform to others. Subsequent posts will go into more detail on these same topics since this post doesn't quite do any of these features justice. This post doesn't describe tools, like Visual Studio, nor does it cover higher-level libraries and application models like those provided by ASP.NET.

Before getting into the details, it is worth talking about .NET usage. It is used by millions of developers, to create cloud, client, and other apps on [multiple operating systems and chip architectures](https://github.com/dotnet/core/blob/main/release-notes/7.0/supported-os.md). It is also run in some well-known places, like [Azure](https://azure.microsoft.com/), [StackOverflow](https://wouterdekort.com/2022/05/25/the-stackoverflow-journey-to-dotnet6/), and [Unity](https://blog.unity.com/technology/unity-and-net-whats-next). It is common to find .NET used in companies of all sizes, but particularly larger ones. In many places, it is a good technology to know to get a job.

## .NET design point

> The .NET platform stands for **Productivity**, **Performance**, **Security**, and **Reliability**. The balance .NET strikes between these values is what makes it attractive.

The .NET design point can be boiled down to being effective and efficient in both the safe domain (where everything is productive) and in the unsafe domain (where tremendous functionality exists). .NET is perhaps the managed environment with the most built-in functionality, while also offering the lowest cost to interop with the outside world, with no tradeoff between the two. In fact, [many features exploit this seamless divide](https://devblogs.microsoft.com/dotnet/performance_improvements_in_net_7/), building safe managed APIs on the raw power and capability of the underlying OS and [CPU](https://devblogs.microsoft.com/dotnet/arm64-performance-improvements-in-dotnet-7/).

We can expand on the design point a bit more:

- **Productivity is full-stack** with runtime, libraries, language, and tools all contributing to developer user experience.
- **Safe code** is the primary compute model, while unsafe code enables additional manual optimizations.
- **Static and dynamic code** are both supported, enabling a broad set of distinct scenarios.
- **Native code interop and hardware intrinsics** are low cost and high-fidelity (raw API and instruction access).
- **Code is portable across platforms** (OS, chip architecture), while platform targeting enables specialization and optimization.
- **Adaptability across programming domains** (cloud, client, gaming) is enabled with [specialized implementations](https://github.com/dotnet/designs/blob/main/accepted/2020/form-factors.md) of the general-purpose programming model.
- **Industry standards** like [OpenTelemetry](https://opentelemetry.io/) and [gRPC](https://grpc.io/) are favored over bespoke solutions.

## The pillars of the .NET Stack

The runtime, libraries, and languages are the pillars of the .NET stack. Higher-level components, like .NET tools and app stacks like ASP.NET Core, build on top of these pillars. The pillars have a symbiotic relationship, having been designed and built together by a single group (Microsoft employees and the open source community), where individuals work on and inform multiple of these components.

C# is object-oriented and the runtime supports object orientation. C# requires garbage collection and the runtime provides a tracing garbage collector. In fact, it would be impossible to port C# (in its complete form) to a system without garbage collection. The libraries (and also the app stacks) shape those capabilities into concepts and object models that enable developers to productively write algorithms in intuitive workflows.

C# is a modern, safe, and general-purpose programming language that spans from high-level features such as [data-oriented records](https://learn.microsoft.com/dotnet/csharp/language-reference/builtin-types/record) to low-level features such as [function pointers](https://learn.microsoft.com/dotnet/csharp/language-reference/proposals/csharp-9.0/function-pointers). It offers static typing and type- and memory-safety as baseline capabilities, which simultaneously improves developer productivity and code safety.  The C# compiler is also extensible, supporting a plug-in model that enables developers to augment the system with additional diagnostics and compile-time code generation.

A number of C# features have influenced or were influenced by state of the art programming languages. For example, C# was the first mainstream language to introduce [`async` and `await`](https://learn.microsoft.com/dotnet/standard/parallel-programming/task-based-asynchronous-programming). At the same time, C# borrows concepts first introduced in other programming languages, for example by adopting functional approaches such as [pattern matching](https://learn.microsoft.com/dotnet/csharp/fundamentals/functional/pattern-matching) and [primary constructors](https://learn.microsoft.com/dotnet/csharp/language-reference/builtin-types/record#positional-syntax-for-property-definition).

The core libraries expose thousands of types, many of which integrate with and fuel the C# language. For example, C#'s [`foreach`](https://learn.microsoft.com/dotnet/csharp/language-reference/statements/iteration-statements#the-foreach-statement) enables enumerating arbitrary collections, with pattern-based optimizations that enable collections like [`List<T>`](https://learn.microsoft.com/dotnet/api/system.collections.generic.list-1) to be processed simply and efficiently. Resource management may be left up to garbage collection, but prompt cleanup is possible via [`IDisposable`](https://learn.microsoft.com/dotnet/api/system.idisposable) and direct language support in [`using`](https://learn.microsoft.com/dotnet/csharp/language-reference/keywords/using-statement).

[String interpolation](https://devblogs.microsoft.com/dotnet/string-interpolation-in-c-10-and-net-6/) in C# is both expressive and efficient, integrated with and powered by implementations across core library types like [`string`](https://learn.microsoft.com/dotnet/api/system.string), [`StringBuilder`](https://learn.microsoft.com/dotnet/api/system.text.stringbuilder), and [`Span<T>`](https://learn.microsoft.com/dotnet/api/system.span-1). And [language-integrated query (LINQ)](https://learn.microsoft.com/dotnet/csharp/programming-guide/concepts/linq/) features are powered by hundreds of sequence-processing routines in the libraries, like [`Where`](https://learn.microsoft.com/dotnet/api/system.linq.enumerable.where), [`Select`](https://learn.microsoft.com/dotnet/api/system.linq.enumerable.select), and [`GroupBy`](https://learn.microsoft.com/dotnet/api/system.linq.enumerable.groupby), with an extensible design and implementations that support both in-memory and remote data sources.  The list goes on, and what's integrated into the language directly only scratches the surface of the functionality exposed as part of the core .NET libraries, from [compression](https://learn.microsoft.com/dotnet/api/system.io.compression) to [cryptography](https://learn.microsoft.com/dotnet/api/system.security.cryptography) to [regular expressions](https://devblogs.microsoft.com/dotnet/regular-expression-improvements-in-dotnet-7). A comprehensive [networking stack](https://learn.microsoft.com/dotnet/api/system.net) is a domain of its own, spanning from [sockets ](https://learn.microsoft.com/dotnet/api/system.net.sockets) to [HTTP/3](https://learn.microsoft.com/dotnet/api/system.net.http). Similarly, the libraries support processing a myriad of formats and languages like [JSON](https://learn.microsoft.com/dotnet/api/system.text.json), [XML](https://learn.microsoft.com/dotnet/api/system.xml), and [tar](https://learn.microsoft.com/dotnet/api/system.formats.tar).

The .NET runtime was initially referred to as the "Common Language Runtime (CLR)". It continues to support multiple languages, some [maintained by Microsoft](https://devblogs.microsoft.com/dotnet/update-to-the-dotnet-language-strategy/) (e.g. C#, F#,  Visual Basic, C++/CLI, and PowerShell) and some by other organizations (e.g. Cobol, [Java](https://github.com/ikvm-revived/ikvm), [PHP](https://www.peachpie.io/), [Python](https://ironpython.net/), [Scheme](https://github.com/IronScheme/IronScheme)). Many improvements are language-agnostic, which raises all boats.

Next, we're going to look at the various platform characteristics that they deliver together. We could detail each of these components separately, but you'll soon see that they cooperate in delivering on the .NET design point. Let's start with the type system.

## Type system

The .NET type system offers significant breadth, catering somewhat equally to safety, descriptiveness, dynamism, and native interop.

First and foremost, the type system enables an object-oriented programming model. It includes types, (single base class) inheritance, interfaces (including default method implementations), and virtual method dispatch to provide a sensible behavior for all the type layering that object orientation allows.

[Generics](https://learn.microsoft.com/dotnet/csharp/fundamentals/types/generics) are a pervasive feature that allow specializing classes to one or more types. For example, [`List<T>`](https://learn.microsoft.com/dotnet/api/system.collections.generic.list-1) is an open generic class, while instantiations like `List<string>` and `List<int>` avoid the need for separate `ListOfString` and `ListOfInt` classes or relying on `object` and casting as was the case with [`ArrayList`](https://learn.microsoft.com/dotnet/api/system.collections.arraylist). Generics also enable creating useful systems across disparate types (and reducing the need for a lot of code), like with [Generic Math](https://learn.microsoft.com/dotnet/standard/generics/math).

[Delegates](https://learn.microsoft.com/dotnet/csharp/delegates-overview) and [lambdas](https://learn.microsoft.com/dotnet/csharp/language-reference/operators/lambda-expressions) enable passing methods as data, which makes it easy to integrate external code within a flow of operations owned by another system. They are a kind of "glue code" and their signatures are often generic to allow broad utility.

```csharp
app.MapGet("/Product/{id}", async (int id) =>
{
    if (await IsProductIdValid(id))
    {
        return await GetProductDetails(id);
    }

    return Products.InvalidProduct;
});
```

This use of lambdas is part of [ASP.NET Core Minimal APIs](https://learn.microsoft.com/aspnet/core/fundamentals/minimal-apis/overview). It enables providing an endpoint implementation directly to the routing system. In more recent versions, ASP.NET Core makes more extensive use of the type system.

[Value types](https://learn.microsoft.com/dotnet/csharp/language-reference/builtin-types/value-types) and [stack-allocated](https://learn.microsoft.com/dotnet/csharp/language-reference/operators/stackalloc) memory blocks offer more direct, low-level control over data and native platform interop, in contrast to .NET's GC-managed types. Most of the primitive types in .NET, like integer types, are value types, and users can define their own types with similar semantics.

Value types are fully supported through .NET's generics system, meaning that generic types like `List<T>` can provide flat, no-overhead memory representations of value type collections. In addition, .NET generics provide specialized compiled code when value types are substituted, meaning that those generic code paths can avoid expensive GC overhead.

```csharp
byte magicSequence = 0b1000_0001;
Span<byte> data = stackalloc byte[128];
DuplicateSequence(data[0..4], magicSequence);
```

This code results in stack-allocated values. The `Span<byte>` is a safe and richer version of what would otherwise be a `byte*`, providing a length value (with bounds checking) and convenient span slicing.

[Ref](https://learn.microsoft.com/dotnet/csharp/language-reference/keywords/ref) types and variables are a sort of mini-programming model that offers lower-level and lighter-weight abstractions over type system data. This includes [`Span<T>`](https://learn.microsoft.com/dotnet/api/system.span-1). This programming model is not general purpose, including significant restrictions to maintain safety.

```csharp
internal readonly ref T _reference;
```

[This use of `ref`](https://learn.microsoft.com/dotnet/csharp/language-reference/keywords/ref#ref-fields) results in copying a pointer to the underlying storage rather than copying the data referenced by that pointer. Value types are "copy by value" by default. `ref` provides a "copy by reference" behavior, which can provide significant performance benefits.

## Automatic memory management

The .NET runtime provides automatic memory management via a garbage collector (GC). For any language, its memory management model is likely its most defining characteristic. This is true for .NET languages.

Heap corruption bugs are notoriously hard to debug. It's not uncommon that engineers spend many weeks if not months tracking these down. Many languages use a garbage collector as a user friendly way of eliminating these bugs because the GC ensures correct object lifetimes. Typically, GCs free memory in batches to operate efficiently. This incurs pauses that may not be suitable if you have very tight latency requirements, and the memory usage would be higher. GCs tend to have better [memory locality](https://en.wikipedia.org/wiki/Locality_of_reference) and some are capable of compacting the heap making it less prone to [memory fragmentation](https://en.wikipedia.org/wiki/Fragmentation_(computing)).

.NET has a self-tuning, [tracing GC](https://en.wikipedia.org/wiki/Tracing_garbage_collection). It aims to deliver "hands off" operation in the general case while offering configuration options for more extreme workloads. The GC is the result of many years of investment, improving and learning from many kinds of workloads.

**Bump pointer allocation** &mdash; objects are allocated by incrementing an allocation pointer by the size needed (instead of finding space in segregated free blocks) so those allocated together tend to stay together. And since they are often accessed together this enables better [memory locality](https://en.wikipedia.org/wiki/Locality_of_reference) which is important for performance.

**Generational collections** &mdash; it's extremely common that object lifetimes follow the [generational hypothesis](https://en.wikipedia.org/wiki/Tracing_garbage_collection#Generational_GC_(ephemeral_GC)), that an object either lives for very long or dies very quickly. So it's much more efficient for a GC to only collect memory occupied by ephemeral objects most of time it runs (called *ephemeral GCs*), instead of having to collect the whole heap (called *full GCs*) every time it runs.

**Compaction** &mdash; the same amount of free space in larger and fewer chunks is more useful than in smaller and more chunks. During a compacting GC, survived objects are moved together so larger free spaces can be formed. This is harder to implement than a non-moving GC as it will need to update references to these moved objects. The .NET GC is dynamically tuned to perform compaction only when it determines the reclaimed memory is worth the GC cost. This means ephemeral collections are often compacting.

**Parallel** &mdash; GC work can run on a single thread or on multiple threads. The Workstation flavor does GC work on a single thread while the Server flavor does it on multiple GC threads so that it can finish much faster. The Server GC can also accommodate a larger allocation rate as there are multiple heaps the application can allocate on instead of only one, so it's very good for throughput.

**Concurrent** &mdash; doing GC work while user threads are paused &mdash; called [Stop-The-World](https://en.wikipedia.org/wiki/Tracing_garbage_collection#Stop-the-world_vs._incremental_vs._concurrent) &mdash; makes the implementation simpler but the length of these pauses may be unacceptable. .NET offers a [concurrent flavor](https://github.com/Maoni0/mem-doc/blob/master/doc/.NETMemoryPerformanceAnalysis.md#Concurrent-GCBackground-GC) to mitigate that issue.

**Pinning** &mdash; the .NET GC supports object pinning, which enables zero-copy interop with native code. This capability enables high-performance and high-fidelity native interop, with limited overhead for the GC.

**Standalone GC** &mdash; a standalone GC with a different implementation can be used (specified via config and satisfying [interface requirements](https://github.com/dotnet/runtime/blob/main/src/coreclr/gc/gcinterface.h)). This makes investigations and trying out new features much easier.

**Diagnostics** &mdash; The GC provides rich information about memory and collections, structured in a way that allows you to correlate data with the rest of the system. For example, you can evaluate the [GC impact of your tail latency](https://github.com/Maoni0/mem-doc/blob/master/doc/.NETMemoryPerformanceAnalysis.md#measure-the-impact-of-factors-that-likely-affect-your-perf-metrics) by capturing GC events and correlating them with other events like IO to calculate how much GC is contributing vs other factors, so you can direct your efforts to the right components.

## Safety

Programming safety has been one of the top topics of the last decade. It is an inherent component of a managed environment like .NET.

Forms of safety:

- [Type safety](https://en.wikipedia.org/wiki/Type_safety) &mdash; An arbitrary type cannot be used in place of another, avoiding undefined behavior.
- [Memory safety](https://en.wikipedia.org/wiki/Memory_safety) &mdash; Only allocated memory is ever used, for example a variable either references a live object or is `null`.
- [Concurrency or thread safety](https://en.wikipedia.org/wiki/Thread_safety) &mdash; Shared data cannot be accessed in a way that would result in undefined behavior.

Note: The US Federal government recently published guidance on the [importance of memory safety](https://www.nsa.gov/Press-Room/News-Highlights/Article/Article/3215760/nsa-releases-guidance-on-how-to-protect-against-software-memory-safety-issues/).

.NET was designed as a safe platform from its initial design. In particular, it was intended to enable a new generation of web servers, which inherently need to accept untrusted input in the world's most hostile computing environment (the Internet). It is now generally accepted that web programs should be written in safe languages.

Type safety is enforced by a combination of the language and the runtime. The compiler validates static invariants, such as assigning unlike types &mdash; for example, assigning `string` to `Stream` &mdash; which will produce compiler errors. The runtime validates dynamic invariants, such as casting between unlike types, which will produce an [InvalidCastException](https://learn.microsoft.com/dotnet/api/system.invalidcastexception).

Memory safety is provided largely by cooperation between a code generator (like a JIT) and a garbage collector. Variables either reference live objects, are `null`, or are out of scope. Memory is auto-initialized by default such that new objects do not use uninitialized memory. Bounds checking ensures that accessing an element with an invalid index will not allow reading undefined memory &mdash; often caused by off-by-one errors &mdash; but instead will result in a [IndexOutOfRangeException](https://learn.microsoft.com/dotnet/api/system.indexoutofrangeexception).

`null` handling is a specific form of memory safety. [Nullable reference types](https://learn.microsoft.com/dotnet/csharp/nullable-references) is a C# language and compiler feature that statically identifies code that is not safely handling `null`. In particular, the compiler warns you if you dereference a variable that might be null. You can also disallow `null` assignment so the compiler warns you if you assign a variable from a value that might be null. The runtime has a matching dynamic validation feature that prevents `null` references from being accessed, by throwing [NullReferenceException](https://learn.microsoft.com/dotnet/api/system.nullreferenceexception).

This feature relies on [nullable attributes](https://learn.microsoft.com/dotnet/csharp/language-reference/attributes/nullable-analysis) in the library. It also relies on their exhaustive application within the libraries and app stacks such that user code can be provided with accurate results from static analysis tools.

```csharp
string? SomeMethod() => null;
string value = SomeMethod() ?? "default string";
```

This code is considered null-safe by the C# compiler since `null` use is declared and handled, in part by `??`, the [null coalescing operator](https://learn.microsoft.com/dotnet/csharp/language-reference/operators/null-coalescing-operator). The `value` variable will always be non-null, matching its declaration.

There is no built-in concurrency safety in .NET. Instead, developers need to follow patterns and conventions to avoid undefined behavior. There are also analyzers and other tools in the .NET ecosystem that provide insight into concurrency issues. And the core libraries include a multitude of types and methods that are safe to be used concurrently, for example [concurrent collections](https://learn.microsoft.com/dotnet/api/system.collections.concurrent) that support any number of concurrent readers and writers without risking data structure corruption.

The runtime exposes safe and [unsafe code](https://learn.microsoft.com/dotnet/csharp/language-reference/unsafe-code) models. Safety is guaranteed for safe code, which is the default, while developers must opt-in to using unsafe code. Unsafe code is typically used to interop with the underlying platform, interact with hardware, or to implement manual optimizations for performance critical paths.


A [sandbox](https://en.wikipedia.org/wiki/Sandbox_(computer_security))  is a special form of safety that provides isolation and restricts access between components. We rely on standard isolation technologies, like processes (and CGroups), virtual machines, and Wasm (with their varying characteristics).

## Error handling

Exceptions are the primary error handling model in .NET. Exceptions have the benefit that error information does not need to be represented in method signatures or handled by every method.

The following code demonstrates a typical pattern:

```csharp
try
{
    var lines = await File.ReadAllLinesAsync(file);
    Console.WriteLine($"The {file} has {lines.Length} lines.");
}
catch (Exception e) when (e is FileNotFoundException or DirectoryNotFoundException)
{
    Console.WriteLine($"{file} doesn't exist.");
}
```

Proper exception handling is essential for application reliability. Expected exceptions can be intentionally handled in user code, otherwise an app will crash. A crashed app is more reliable and diagnosable than an app with undefined behavior.

Exceptions are thrown from the point of an error and automatically collect additional diagnostic information about the state of the program that is used with interactive debugging, application observability, and post-mortem debugging. Each of these diagnostic approaches rely on having access to rich error information and application state to diagnose problems.

Exceptions are intended for rare situations. This is, in part, because they have a relatively high performance cost. They are not intended to be used for control flow, even though they are sometimes used that way.

Exceptions are used (in part) for cancellation. They enable efficiently halting execution and unwinding a callstack that had work in progress once a cancellation request is observed.

```csharp
try 
{ 
    await source.CopyToAsync(destination, cancellationToken); 
} 
catch (OperationCanceledException) 
{ 
    Console.WriteLine("Operation was canceled"); 
}
```

.NET design patterns include alternative forms of error handling for situations when the performance cost of exceptions is prohibitive. For example, [`int.TryParse`](https://learn.microsoft.com/dotnet/api/system.int32.tryparse) returns a `bool`, with an `out` parameter containing the parsed valid integer upon success. [`Dictionary<TKey, TValue>.TryGetValue`](https://learn.microsoft.com/dotnet/api/system.collections.generic.dictionary-2.trygetvalue) offers a similar model, returning a valid `TValue` type as an `out` parameter in the `true` case.

Error handling, and diagnostics more generally, is implemented via low-level runtime APIs, [higher-level libraries](https://opentelemetry.io/docs/instrumentation/net/), and [tools](https://learn.microsoft.com/dotnet/core/diagnostics/#net-core-diagnostic-global-tools). These capabilities have been designed to support newer deployment options like containers. For example, [dotnet-monitor](https://learn.microsoft.com/dotnet/core/diagnostics/dotnet-monitor) can egress runtime data from an app to a listener via a built-in diagnostic-oriented web server.

## Concurrency

Support for doing multiple things at the same time is fundamental to practically all workloads, whether it be client applications doing background processing while keeping the UI responsive, services handling thousands upon thousands of simultaneous requests, devices responding to a multitude of simultaneous stimuli, or high-powered machines parallelizing the processing of compute-intensive operations. Operating systems provide support for such concurrency via threads, which enable multiple streams of instructions to be processed independently, with the operating system managing the execution of those threads on any available processor cores in the machine. Operating systems also provide support for doing I/O, with mechanisms provided for enabling I/O to be performed in a scalable manner with many I/O operations "in flight" at any particular time. Programming languages and frameworks can then provide various levels of abstraction on top of this core support.

.NET provides such concurrency and parallelization support at multiple levels of abstraction, both via libraries and deeply integrated into C#.  A [`Thread`](https://learn.microsoft.com/dotnet/api/system.threading.thread) class sits at the bottom of the hierarchy and represents an operating system thread, enabling developers to create new threads and subsequently join with them. [`ThreadPool`](https://learn.microsoft.com/dotnet/api/system.threading.threadpool) sits on top of threads, allowing developers to think in terms of work items that are scheduled asynchronously to run on a pool of threads, with the management of those threads (including the addition and removal of threads from the pool, and the assignment of work items to those threads) left up to the runtime. [`Task`](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task) then provides a unified representation for any operations performed asynchronously and that can be created and joined with in multiple ways; for example, `Task.Run` allows for scheduling a delegate to run on the `ThreadPool` and returns a `Task` to represent the eventual completion of that work, while `Socket.ReceiveAsync` returns a `Task<int>` (or `ValueTask<int>`) that represents the eventual completion of the asynchronous I/O to read pending or future data from a `Socket`. A vast array of synchronization primitives are provided for coordinating activities synchronously and asynchronously between threads and asynchronous operations, and a multitude of higher-level APIs are provided to ease the implementation of common concurrency patterns, e.g. [`Parallel.ForEach`](https://learn.microsoft.com/dotnet/api/system.threading.tasks.parallel) and `Parallel.ForEachAsync` make it easier to process all elements of a data sequence in parallel.

Asynchronous programming support is also a first-class feature of the C# programming language, which provides the `async` and `await` keywords that make it easy to write and compose asynchronous operations while still enjoying the full benefits of all the control flow constructs the language has to offer.

## Reflection

[Reflection](https://learn.microsoft.com/dotnet/framework/reflection-and-codedom/reflection) is a "programs as data" paradigm, allowing one part of a program to dynamically query and/or invoke another, in terms of [assemblies, types and members](https://learn.microsoft.com/dotnet/api/system.reflection). It is particularly useful for late-bound programming models and tools.

The following code uses reflection to find and invoke types.

```csharp
foreach (Type type in typeof(Program).Assembly.DefinedTypes)
{
    if (type.IsAssignableTo(typeof(IStory)) &&
        !type.IsInterface)
    {
        IStory? story = (IStory?)Activator.CreateInstance(type);
        if (story is not null)
        {
            var text = story.TellMeAStory();
            Console.WriteLine(text);
        }
    }
}

interface IStory
{
    string TellMeAStory();
}

class BedTimeStore : IStory
{
    public string TellMeAStory() => "Once upon a time, there was an orphan learning magic ...";
}

class HorrorStory : IStory
{
    public string TellMeAStory() => "On a dark and stormy night, I heard a strange voice in the cellar ...";
}
```

This code dynamically enumerates all of an assembly's types that implement a specific interface, instantiates an instance of each type, and invokes a method on the object via that interface.  The code could have been written statically instead, since it's only querying for types in an assembly it's referencing, but to do so it would need to be handed a collection of all of the instances to process, perhaps as a `List<IStory>`. This late-bound approach would be more likely to be used if this algorithm loaded arbitrary assemblies from an add-ins directory. Reflection is often used in scenarios like that, when assemblies and types are not known ahead of time.

Reflection is perhaps the most dynamic system offered in .NET. It is intended to enable developers to create their own binary code loaders and method dispatchers, with semantics that can match or diverge from static code policies (defined by the runtime). Reflection exposes a [rich object model](https://learn.microsoft.com/dotnet/api/system.reflection), which is straightforward to adopt for narrow use cases but requires a deeper understanding the .NET type system as scenarios get more complex.

Reflection also enables a separate mode where [generated IL byte code can be JIT-compiled](https://learn.microsoft.com/dotnet/framework/reflection-and-codedom/emitting-dynamic-methods-and-assemblies) at runtime, sometimes used to replace a general algorithm with a specialized one. It is often used in serializers or object relational mappers once the object model and other details are known.

## Compiled binary format

Apps and libraries are compiled to a [standardized](https://github.com/dotnet/runtime/blob/main/docs/project/dotnet-standards.md) cross-platform [bytecode](https://en.wikipedia.org/wiki/Common_Intermediate_Language) in [PE/COFF format](https://en.wikipedia.org/wiki/COFF). Binary distribution is foremost a performance feature. It enables apps to scale to larger and larger numbers of projects. Each library includes a  database of imported and exported types, referred to as [metadata](https://www.nuget.org/packages/System.Reflection.Metadata), which serves a significant role for both development operations and for running the app.

Compiled binaries include two main aspects:

- Binary bytecode &mdash; terse and regular format that skips the need to parse textual source after compilation by a high-level language compiler (like C#).
- Metadata &mdash; describes imported and exported types, including the location of the byte code for a given method.

For development, tools can efficiently read metadata to determine the set of types exposed by a given library and which of those types implement certain interfaces, for example. This process makes compilation fast and enables IDEs and other tools to accurately present lists of types and members for a given context.

For runtime, metadata enables libraries to be loaded lazily, and method bodies even more so. Reflection (discussed later) is the runtime API for metadata and IL. There are other more appropriate APIs for tools.

The IL format has remained backwards-compatible over time. The latest .NET version can still load and execute binaries produced with .NET Framework 1.0 compilers.

Shared libraries are typically distributed via [NuGet packages](https://www.nuget.org/). NuGet packages, with a single binary, can work on any operating system and architecture, by default, but can also be specialized to provide specific behavior in specific environments.

## Code generation

.NET bytecode is not a machine-executable format, but it needs to be made executable by some form of code generator. This can be achieved by ahead-of-time (AOT) compilation, just-in-time (JIT) compilation, interpretation, or transpilation. In fact, these are all used today in various scenarios.

.NET is most known for JIT compilation. JITs compile methods (and other members) to native code while the application is running and only as they are needed, hence the "just in time" name. For example, a program might only call one of several methods on a type at runtime. A JIT can also take advantage of information that is only available at runtime, like values of initialized readonly static variables or the exact CPU model that the program is running on, and can compile the same method multiple times in order to optimize each time for different goals and with learnings from previous compilations.

JITs produce code for a given operating system and chip architecture. .NET has JIT implementations that support, for example, Arm64 and x64 instruction sets, and Linux, macOS, and Windows operating systems. As a .NET developer, you don't have to worry about the differences between CPU instruction sets and operating system calling conventions. The JIT takes care of producing the code that the CPU wants. It also knows how to produce fast code for each CPU, and OS and CPU vendors often help us do exactly that.

AOT is similar except that the code is generated before the program is run. Developers choose this option because it can significantly improve startup time by eliminating the work done by a JIT. AOT-built apps are inherently operating system and architecture specific, which means that extra steps are required to make an app run in multiple environments. For example, if you want to support Linux and Windows and Arm64 and x64, then you need to build four variants (to allow for all the combinations). AOT code can provide valuable optimizations, too, but not as many as the JIT in general.

We'll cover interpretation and transpilation in a later post, however, they also play critical roles in our ecosystem.

One of the code-generator optimizations is intrinsics. [Hardware intrinsics](https://github.com/dotnet/designs/blob/main/accepted/2018/platform-intrinsics.md) are an example where [.NET APIs](https://learn.microsoft.com/dotnet/api/system.numerics) are directly translated into CPU instructions. This has been used pervasively throughout .NET libraries for [SIMD](https://en.wikipedia.org/wiki/Single_instruction,_multiple_data) instructions.

## Interop

.NET has been explicitly designed for low-cost interop with native libraries. .NET programs and libraries can seamlessly call low-level operating system APIs or tap into the vast ecosystem of C/C++ libraries. The modern .NET runtime is focused on providing low-level interop building blocks such as the ability to call native methods via function pointers, exposing managed methods as [unmanaged callbacks](https://learn.microsoft.com/dotnet/api/system.runtime.interopservices.unmanagedcallersonlyattribute) or [customized interface casting](https://learn.microsoft.com/dotnet/api/system.runtime.interopservices.idynamicinterfacecastable). .NET is also continually evolving in this area and in .NET 7 released [source generated solutions](https://learn.microsoft.com/dotnet/standard/native-interop/pinvoke-source-generation) that further reduced overhead and were AOT friendly.

The following demonstrates the efficiency of C# functions pointers with the `LibraryImport` source generator introduced in .NET 7 (this source generator support layers on top of the `DllImport` support that's existed since the beginning of .NET).
```csharp
// Using a function pointer avoids a delegate allocation.
// Equivalent to `void (*fptr)(int) = &RegisterCallback;` in C
delegate* unmanaged<int, void> fptr = &RegisterCallback;
RegisterCallback(fptr);

[UnmanagedCallersOnly]
static void Callback(int a) => Console.WriteLine($"Callback:  {a}");

[LibraryImport("...", EntryPoint = "RegisterCallback")]
static partial void RegisterCallback(delegate* unmanaged<int, void> fptr);
```

Independent packages provide higher-level domain-specific interop solutions by taking advantage of these low-level building blocks, for example [ClangSharp](https://github.com/dotnet/ClangSharp), [Xamarin.iOS & Xamarin.Mac](https://github.com/xamarin/xamarin-macios), [CsWinRT](https://github.com/microsoft/CsWinRT), [CsWin32](https://github.com/microsoft/CsWin32) and [DNNE](https://github.com/AaronRobinsonMSFT/DNNE).

These new features don't mean built-in interop solutions like built-in runtime managed/unmanaged marshalling or Windows COM interop aren't useful &mdash; we know they are and that people have come to rely upon them. Those features that have been historically built into the runtime continue to be supported in the .NET runtime. However, they are for backward compatibility only, with no plans to evolve them further. All future investments will be focused on the interop building blocks and in the domain-specific solutions that they enable.

## Binary distributions

The .NET Team at Microsoft maintains [several binary distributions](https://github.com/dotnet/core/blob/main/os-lifecycle-policy.md), more recently supporting Android, iOS, and [Web Assembly](https://learn.microsoft.com/aspnet/core/blazor/hosting-models#blazor-webassembly). The team uses a variety of techniques to specialize the codebase for each one of these environments. Most of the platform is written in C#, which enables porting to be focused on a relatively small set of components.

The community [maintains another set of distributions](https://github.com/dotnet/core/blob/main/linux.md), largely focused on Linux. For example,.NET is included in [Alpine Linux](https://pkgs.alpinelinux.org/packages?name=dotnet*), [Fedora](https://packages.fedoraproject.org/search?query=dotnet), [Red Hat Enterprise Linux](http://redhatloves.net/), and [Ubuntu](https://ubuntu.com/blog/install-dotnet-on-ubuntu).

The community has also extended .NET to run on other platforms. [Samsung ported .NET for their Arm-based Tizen platform](https://developer.samsung.com/tizen/About-Tizen.NET/Tizen.NET.html). [Red Hat](http://redhatloves.net/) and [IBM ported .NET to LinuxONE/s390x](https://community.ibm.com/community/user/ibmz-and-linuxone/blogs/elizabeth-k-joseph1/2021/11/10/net-6-comes-to-ibm-z-and-linuxone).  [Loongson Technology](https://www.loongson.cn/) ported [.NET to LoongArch](https://github.com/dotnet/runtime/issues/59561). We hope and expect that new partners will port .NET to other environments.

Unity Technologies has [started a multi-year initiative](https://blog.unity.com/technology/unity-and-net-whats-next) to modernize their .NET runtime.

The [.NET open source project](https://github.com/dotnet/dotnet) is maintained and structured to enable individuals, companies, and other organizations to collaborate together in a traditional [upstream model](https://www.redhat.com/en/blog/what-open-source-upstream). Microsoft is the steward of the platform, providing both project governance and project infrastructure (like CI pipelines). The Microsoft team collaborates with organizations to help make them successful using and/or [porting .NET](https://github.com/dotnet/runtime/blob/main/docs/design/coreclr/botr/guide-for-porting.md). The project has a broad upstreaming policy, which includes accepting changes that are unique to a given distribution.

A major focus is the [source-build project](https://github.com/dotnet/source-build), which [multiple organizations](https://github.com/dotnet/core/blob/main/linux.md) use to build .NET according to typical distro rules, for example [Canonical (Ubuntu)](https://devblogs.microsoft.com/dotnet/dotnet-6-is-now-in-ubuntu-2204/). This focus has expanded more recently with the addition of a [Virtual Mono Repo (VMR)](https://github.com/dotnet/dotnet). The .NET project is composed of many repos, which aids .NET developer efficiency but makes it harder to build the a complete product. The VMR solves that problem.

## Summary

We're several versions into the modern .NET era, having recently released [.NET 7](https://devblogs.microsoft.com/dotnet/announcing-dotnet-7/). We thought it would be useful if we summarized what we've been striving to build &mdash; at the lowest levels of the platform &mdash; since .NET Core 1.0. While we've clearly kept to the spirit of the original .NET, the result is a new platform that strikes a new path and offers new and considerably more value to developers.

Let's end where we started. .NET stands for four values: Productivity, Performance, Security and Reliability. We are big believers that developers are best served when different language platforms offer different approaches. As a team, we seek to offer high productivity to .NET developers while providing a platform that leads in performance, security and reliability.

We plan to add more posts in this series. Which topics would you like to see addressed first? Please tell us in the comments. Would you like more of this "big picture" content?

If you want more of this content, you might check out [Introduction to the Common Language Runtime (CLR)](https://github.com/dotnet/runtime/blob/main/docs/design/coreclr/botr/intro-to-clr.md).

This post was written by [Jan Kotas](https://github.com/jkotas), [Rich Lander](https://github.com/richlander), [Maoni Stephens](https://github.com/Maoni0), and [Stephen Toub](https://github.com/stephentoub), with the insight and review of our colleagues on the .NET team.
