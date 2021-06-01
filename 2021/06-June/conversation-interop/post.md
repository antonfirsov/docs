---
post_title: Conversation about .NET interop
username: rlander@microsoft.com
microsoft_alias: rlander@microsoft.com
categories: .NET
desired_publication_date: 06/01/2021
summary: Conversation with .NET engineers about native, language, and operating system interop.
---

[Interop](https://docs.microsoft.com/dotnet/standard/native-interop/) is the subsystem in the runtime that enables interoperability with other systems, like native C libraries or Objective-C. In some cases, it includes a full interop implementation like via P/Invoke or COM interop. In other cases, like for WinRT, it provides the building block APIs such that another (external) component can provide an end-to-end implementation.

We're using the [conversation format](https://devblogs.microsoft.com/dotnet/category/conversations/) again, this time with four of the runtime engineers who work on interop and related topics.

* [Aaron Robinson](https://github.com/AaronRobinsonMSFT)
* [Elinor Fung](https://github.com/elinor-fung)
* [Jeremy Koritzinsky](https://github.com/jkoritzinsky)
* [Tanner Gooding](https://github.com/tannergooding)

## What is interop and what problem does it solve? What capability would be missing without it?

**Aaron:** It attempts to bridge the gap between disparate platforms. In this case between .NET and the underlying platform it is running on top of. User defined interaction with the platform.

**Tanner:** Interop is a way to "communicate" between two different languages or platforms. Without it, we wouldn't be able to easily depend on existing code produced by other platforms and languages and would need to rebuild it ourselves.

For example, WinForms is written in C# and is built by interoping with the Windows APIs written in C, without interop this would be much more difficult.

**Jeremy:** Interop enables .NET users to interact with non-.NET code. Without interop, users would not be able to use non-.NET libraries within their .NET applications. For example, the .NET platform would not be able to use networking, compression, or cryptography libraries written in C or C++ or provided by the platform without some form of interop.

**Elinor:** Interop allows interaction between different languages/platforms. Without it, we would be missing a way to use components defined outside of the current language - for example, a .NET application could not easily use a native component written in C and the .NET platform itself could not easily call operating system APIs in order to provide an abstraction over them.

## We’ve had programming languages for a long time now. Are they getting more similar or less so? Is the need for language interop becoming more or less important?

**Aaron:** From a feature set, I think they are becoming more similar. However, the way each language implements those features is different and therein lies the reason for interop support. Language interop is going to become increasingly important as people build systems that contain disparate languages and runtimes.

**Tanner:** I think this depends on the languages you are asking about. There are "families" that are similar (such C, C++, C#) but which are likewise very different at the same time. New languages tend to pick and choose the best parts of other languages while also exposing their own new ideas. I believe interop is becoming more prevalent since the complexities and internal differences in the languages are very hard to rationalize otherwise (two languages might support virtual dispatch but implement it very differently).

**Elinor:** It would depend on how we are determining similarity. I think in terms of general functionality, yes, but in terms of internal complexity, not necessarily. I'd say interop is becomming more important - as more and more languages and runtimes are used, the need for interaction between them becomes more important. Especially if people need to continue supporting and interacting  with previously used languages/runtimes while moving to newer ones.

**Jeremy:** I would say that newer programming languages are getting less similar as time goes on. We're seeing the rise of languages based around various different programming paradigms (for example Rust focuses on memory safety, Go focuses on concurrency, and Swift is more general purpose).  I would say that language interop is becoming more important as systems move to using the language most adept to their needs in particular scenarios while using a more general purpose language for other areas of their products. For example, some applications like Firefox experimented with using Rust in high-performance scenarios like rendering for memory safety, but they are still keeping most of the application in other languages.

## With programming languages and operating systems, you often hear about types systems, ABI, APIs, and calling conventions. Can you describe those things, explain their role in interop and why each system/language tends to be different?

**Aaron:** Type systems are merely abstractions over memory. They are entirely for convenience and little else.
 
An ABI represents the contract between the computing machine and the programming language. For example, what goes in what register during a function call?
 
An API is the programming contract at the software layer. I like the term contract because it is an agreement between software components.
 
All 3 are important because languages, runtimes, and platforms typically define them in their own way but sometimes just say "we follow the same convention as ...". The role of interop is to create the mappings when they don't agree.

**Tanner:** The ABI, or Application Binary Interface, is effectively the contract through which you can communicate with a platform or language. When talking about interop, you are often talking about something that allows one ABI to communicate with another ABI. The type system and calling conventions are extensions of the ABI. The type system defines the rules for things exposed by the platform/language while the calling convention defines how that data is passed around (at a high level). An API, or Application Programming Interface, is the contract exposed to consumers of the platform/language.

Different languages/platforms have different ABIs, type systems, calling conventions, and APIs because they all have their own opinions on what's the right way to do something or sometimes because they have their own additional functionality that requires additional metadata to exist or be passed around.

So if in C++ virtual dispatch is implemented by a virtual method table (an array of function pointers) but in .NET its via [virtual stubs](https://github.com/dotnet/runtime/blob/main/docs/design/coreclr/botr/virtual-stub-dispatch.md), then you need something to rationalize this difference for .NET to talk to C++.

**Jeremy:** The type system is sort of the "mental model" of how data is related within a programming language. This may or may not be related to the API or ABI of the language. For example, C's type system does not have the concept of "member functions", but it can still interact with C++ classes if the developer writes a wrapper to convert between the constants. In .NET, some of the built-in interop system does a mapping between .NET type system concepts (structs, classes, delegates) to C-style concepts (structures, function pointers).
 
The ABI is the platform definition of how to store and transfer structured data throughout a program. For some platforms or application models, this is just calling conventions. For other platforms, this can include concepts like discovering exception handlers, deciding who owns a reference to data, or even the layout of a structure. Interop tooling needs to know the specifics of the source and target ABIs to correctly translate data between each of them at the boundary.
 
An API represents an application of the type system for a specific library. This API describes what data the library takes and how to call into the library with the data. An API generally needs to be described on both sides of an interop boundary, and interop tooling translates from the high-level API to the low-level ABI of both sides of the boundary to enable interop. 
 
A calling convention defines the specific processor registers that data needs to be passed in and out through. This is generally defined at a platform level for easier compatibility and performance across the interop boundary between languages.

**Elinor:** The type system is the logical representation of objects in memory. An ABI is basically a contract that allows communicating with a specific language from a machine layer - an API is the equivalent from software.  The calling convention is about how information is passed between components.

These are all about the rules and boundaries of a system. For interop, these are essentially the contracts by which different languages/systems can communicate. In order for that communication to be successful, each side must understand and adhere to the expectation of the other.

## Let’s think of interop in terms of big buckets. There’s data types, marshalling and some form of method invocation. Is that it?

**Elinor:** Perhaps falling into the category of marshalling, but lifetime management can also be of interest.

**Jeremy:** Another big bucket for interop would be ownership. As part of the marshalling, any interop system must determine who owns the representation of the data in the "target" language. Some platforms define this in some standard mechanism as part of the "method invocation" bucket. Other platforms generally follow some convention to determine ownership. There are some platforms that have no defined convention (plain C being probably the best example), where the interop system either has to allow the user to describe the ownership or it has to make an educated guess.

**Aaron:** Those are the primary dimensions of interop for sure. Data types are about reconciling the different memory abstractions across different platforms and marshalling is all about the transformation between those data types, but there are distinct concerns in each. The invocation part is the tricky part because it always involves "letting go". We do the conversion and then let the other side work with it in the hopes it was done right and when it returns we try our part. One aspect of managed languages that can get tricky are global mechanisms that represent some sort of non-determinism like the GC - that makes "letting go" require serious thought.

**Tanner:** At a high level, there's the representation of the data on each side of the interop boundary, who owns the data (such as if it was allocated by the GC vs on the stack vs on the native heap), which side is invoking into the other side (C# calling into C is regular P/Invoke while C calling back into C# is Reverse P/Invoke).

## The .NET product is now known for leaps in performance with each release. Do we see the same thing with interop?

**Tanner:** There have been various improvements to interop performance, although I think most of the interesting case are from features that allow the runtime itself to do less. For example, it used to be that providing callbacks could be "expensive" due to allocations for the delegate type and then marshalling that as a function pointer. However, with C# 9 function pointers are now more generally available and can now avoid marshalling all together and be "blitted" (copied) across. Some other interesting improvements include `SupressGCTransition`, better inlining for the interop "stub" methods, and support for more types to be used in interop scenarios (such as blittable generic types).

**Aaron:** Yes, but most of this is removing abstractions and simplifying code to prune legacy paths that just consume CPU cycles. Pushing more details into the JIT so it can handle the platform specific details is a big win both in UX and performance - see the new .NET 6 type `CallConvMemberFunction`. The more the JIT knows the better it can optimize code and with tiering it can do more optimizations the more details it has access to. Similar to moving knowledge to the JIT is moving interop closer to build time (that is, managed code). Source generation is going to not only improve start up time but also make more of the system visible to the JIT at the right time and as the JIT improves so will interop. AOT also benefits tremendously from this source generation approach.

**Jeremy:** For interop in .NET, we've been doing quite a bit of work in the past few releases to expose lower-level primitives to enable building higher-performance interop solutions. These primitives have better performance, but can be less user-friendly or convenient than preexisting solutions. For example, the `UnmanagedCallersOnlyAttribute` as well as function pointers enable interop scenarios with lower overhead and less allocations, but they generally do not support the built-in marshalling support that traditional `Marshal.GetDelegateAsFunctionPointer` and P/Invoke experiences support. We've also released other experiences like `ComWrappers` to enable developers to build .NET-COM interop solutions with lower overhead or tailored experiences for a given user's specific use cases, but like `UnmanagedCallersOnly`, this may require the user to build up more of the experience from these low level primitives. We've also included advanced performance features like `SuppressGCTransition` that require careful usage as correct usage can increase performance, but incorrect usage can cause serious performance or reliability issues.

**Elinor:** There have definitely been some improvements in the interop system itself, but much focus has been on enabling users to improve performance in an opt-in way - for example, `UnmanagedCallersOnly` and `SuppressGCTransition` - as well as making sure best practices are known. Function pointers in C# (and the associated runtime support) are another great example of enabling users to write interop code in a more performant way. The runtime itself has largely benefitted from this as well.

## Value types are a key difference with Java. Are value types a strength of the .NET interop system, generally and as compared to JNI?

**Tanner:** Value types and pointers allow us to more easily define "blittable" data, that is data that has the same layout on both sides of the interop boundary. This is a strength in that it means we can have more explicit control over how data is passed around and therefore less overall cost where appropriate. There are tradeoffs, of course, including that value types may not be as intuitive to use in some cases or where they have limitations (in .NET) as compared to reference types.

**Jeremy:** Yes, value types are a strength of the .NET interop system. .NET's support of value types enables more direct conversion/marshalling between a C/C++ representation of a type and the .NET representation. In many cases, a developer can design their .NET value type to exactly match the layout of a given C or C++ value type (in this case we call the .NET type "blittable"), which can enable the .NET interop system to avoid having to generate a thunk to convert or move the data as part of emitting a call. In some of these cases, passing one of these value types by ref or out or in a single-dimensional array by value, .NET can even avoid making any copies and just pass down a pointer to the location where the .NET value type lives, significantly increasing performance over a JNI-style solution that requires extensive copying and custom conversion functions even for simple types.

**Aaron:** Yes they are. ValueTypes represent a large performance win in interop scenarios. It does make some interop hard though because of those ABI concerns mentioned previously.

**Elinor:** Having value types gives the .NET interop system an easy way to directly represent simple data in a blittable way. I consider this an advantage in that users have more options in terms of how they represent the data and it can provide a more 'natural' way to interop with some platforms.

## On Windows, we’ve had P/Invoke, COM Interop for a long time, and now CSWinRT. Is interop on Windows, as it relates to operating system APIs, a solved problem with .NET? If not, what’s left?

**Aaron:** The biggest missing feature is making a supportable cross-platform RPC mechanism. COM is great on Windows and continues to work - although we needed to make it a little more complicated when we exposed it in .NET Core 3.1. Following the same COM route on non-Windows is a non-starter so we need to look at Windows and other platforms holistically with respect to RPC and design a system that works well everywhere. Source generation is likely a key tool here.

**Jeremy:** Generally our direction for Windows is to continue to move our interop solutions into more of a build-time/pay-for-what-you-use model. The built-in COM interop support is very opinionated and hard to change, which is one of the reasons that we built the ComWrappers solution for CsWinRT. We're looking at other mechanisms for making interop more pay-for-play with source generation and the various primitives we've been introducing.

**Tanner:** With the introduction of function pointers in C# 9 and the CCW/RCW APIs .NET 5, users can now have more direct control over how they interop with things like COM since the foundational building blocks are now available. I do think that its largely "solved" in that users have a fairly well-defined approach to interoping with COM or other system APIs and it likely won't deviate much overall. However, I think there is a lot we can still do with regards to tooling (see [microsoft/win32metadata](https://github.com/microsoft/win32metadata), [microsoft/cswin32](https://github.com/microsoft/cswin32), [microsoft/cswinrt](https://github.com/microsoft/cswinrt), [microsoft/clangsharp](https://github.com/microsoft/clangsharp), etc) to make interop usage easier overall.

**Elinor:** COM and p/invokes definitely cover a lot on Windows for operating system APIs. Out-of-proc COM and remoting is definitely achievable, but we don't have a golden/recommended path for that kind of communication. The general (Windows and other) desire is to do a better job of enabling users to better control interop though - and that is not 'solved'.

## With CSWinRT, we moved to a build-time source generating system. What are the benefits of that system? If we were to do that project over again, would we use C# source generators?

**Tanner:** Source generators help automate the process of generating code and integrate it naturally into the build system. This helps ensure that the code is up to date and correct. For interop bindings, this helps automate the sometimes arduous process of dealing with and understanding the differences between two platforms/languages. I'm not involved enough with [cswinrt](https://github.com/microsoft/cswinrt) to say how we would approch things if the project were started today.

**Aaron:** The biggest benefit here is not performing code generation in the runtime at run-time and instead making startup as fast as the JIT can go. It does help with our AOT plans because all the .NET code is there and can be compiled ahead of time. Roslyn Source Generators are very cool and something that might be worth converting to in the future but there are UX issues around COM/WinRT scenarios that are difficult - particularly type sharing. We are working with the Roslyn team on how we can improve these UX issues in the future so perhaps some day it will be the best way for all interop scenarios.

**Jeremy:** With a build-time source generation system built on top of interop primitives, the behavior of the system can be more easily understood and more well-defined. In the pre-CsWinRT world, [WinRT support in .NET was built into the runtime](https://github.com/dotnet/runtime/issues/37672), which meant it was effectively a black box to users and something we couldn't change easily. With CsWinRT, nearly everything is in managed code, so developers can more easily understand and reason about what the interop system is doing under the hood to implement the translation/projection layer.
 
For CsWinRT in particular, the build-time source generation system to build the Windows SDK projections which are then shipped is the best solution for the scenario. With the combination of WinRT generics and backward-compatibility concerns, shipping CsWinRT as a generator but having the primary use case be through precompiled projections works best. 

**Elinor:** A big benefit was the separation of that interop layer from the runtime's built-in system, allowing for independent evolution/improvement as well as ability to use features like AOT and the IL linker/trimmer. I think the source generators would definitely have been worth considering if doing the project over again (but I wasn't around for much of the project).

## The team is working on adding interop support for Objective-C right now. What’s the purpose of that project and what’s interesting and unique about it?

**Aaron:** The primary purpose is to support Mac on the desktop. This has been a focus area for our Xamarin peers. We're still working on our plans, but one can imagine using CoreCLR for Mac desktop apps for Blazor desktop, .NET MAUI or even Visual Studio for Mac. The most interesting facet of Objective-C interop has been the learning how Objective-C interop is similar to existing .NET interop solutions and how it is different. COM and WinRT were designed and implemented at Microsoft so we can see all the little details and create agreements about "bugs" we rely upon. Objective-C is an Apple thing so we don't necessarily have that insight so that has been a learning experience.

**Jeremy:** The purpose of the Objective-C interop support is to bring parity between the MonoVM and CoreCLR runtimes' support for interop with Objective-C. Mono/Xamarin has had a model for interacting with Objective-C for years, but CoreCLR has not had a solution. Like COM and WinRT, Objective-C has a reference-counting memory model, which for correct handling needs to be integrated with the underlying runtime in some capacity. Unlike Mono, CoreCLR has a limited native embedding surface, so the Objective-C work has been to enable a managed API with which Xamarin could integrate their projections of Apple APIs while still having memory-safe handling and exception integration as is present on Mono Framework on MacOS and other Apple platforms.

**Elinor:** The Objective-C interop work enables CoreCLR to support Mac desktop apps. It would enable things like the ability to use UI components native to the platform. It is interesting/unique in that much of the other large interop support is focused on Windows (COM, WinRT), while this is very much stretching our cross-plat support (in a good way).

## Closing

The .NET runtime has had interop capabilities since .NET Framework 1.0. The interop subsystem has followed the general progression of .NET, addressing the needs of new scenarios (like support for Linux and Arm64) and taking advantage of new features in the runtime to make interop better in some way (mostly performance). It has also biased from full in-box implementations at the beginning and is now biased to external tool-based implementations.

Tool-based implementations (that generate source) mean that interop implementations are largely C#, and compiled, debugged, optimized, trimmed as C#/IL. That's really powerful. That means that interop is simpler, easier to maintain, and can take advantage of the rich capabilities available to managed code, including performance enhancements. .NET interop today is an exciting area of runtime development, in large part because much less of it is in the runtime.

Thanks again to Tanner, Jeremy, Elinor and Aaron for sharing your insights and context on interop. It was a great [conversation](https://devblogs.microsoft.com/dotnet/category/conversations/).
