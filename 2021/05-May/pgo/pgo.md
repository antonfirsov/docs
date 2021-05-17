---
post_title: Conversation about PGO
username: rlander@microsoft.com
microsoft_alias: rlander@microsoft.com
categories: .NET Core, .NET
desired_publication_date: 05/22/2021
summary: Conversation with .NET engineers about profile guided optimization (PGO).
---

[Profile guided optimization (PGO)](https://devblogs.microsoft.com/dotnet/announcing-net-6-preview-1/#dynamic-pgo) is an exciting area of investment in [.NET 6](https://devblogs.microsoft.com/dotnet/announcing-net-6-preview-1/) release. We're working on both static and dynamic variations of PGO, with the intention of using them together and offering the best of what they both have to offer.

You might have seen the [Conversation about crossgen2](https://devblogs.microsoft.com/dotnet/conversation-about-crossgen2) post. We decided to try that same form to talk about PGO. Today, I'm hosting a conversation with [Andy Ayers](https://github.com/AndyAyersMS) and [David Wrighton](https://github.com/davidwrighton), two of the runtime architects designing and implementing PGO for .NET. Let's see what they had to say.

## What is the problem that PGO solves? Aren’t compilers (like RyuJIT) pretty sophisticated and powerful as-is?

**David:** PGO improves the performance of applications by adding new information to the optimization process that is dependent on how the application executes, not just on the program's code.

**Andy:** Generally compilers have to assume that all possible behaviors that could happen at runtime will happen at runtime. But most of the time, the runtime behavior of programs only covers a small fraction of what could happen. So, PGO helps the compiler prioritize optimizations based on what is likely to happen.

## Give me an elevator pitch on how PGO works and the best case scenario for improving the performance of an app.

**Andy:** PGO is akin to the compiler having an oracle at each "choice" point in the program, pointing the compiler at the behavior that is most likely. And knowing this, the compiler can optimize the program to best fit how it's actually used.

**David:** PGO works by analyzing the programs behavior and optimizing it based on that information to prioritize behavior found in the past. This can make programs start up faster, run quicker, and have more predictable latency. The PGO data is used by several components of the system to achieve these goals.

## What dev platform is known for strong PGO? Is there a strong example for a similar runtime environment?

**David:** C++ is known for having strong ahead-of-time PGO technology, and both ECMAScript runtimes and the JVM are known for depending on a high quality dynamic PGO scheme for good performance.

**Andy:** PGO has been around for more than nearly 30 years now. I suspect [MSVC](https://docs.microsoft.com/cpp/build/reference/compiler-options) actually has the most mature implementation, given that multiple product groups at Microsoft, like Windows and Office, heavily rely on PGO to optimize their code. Java and JavaScript also have strong PGO implementations.

## There are both static and dynamic PGO systems. What are their strengths? Can you do both at the same time to capture the strengths of both?

**Andy:** Static PGO is good at summarizing the behavior of long running applications or for things like libraries that are shared by multiple programs. Dynamic PGO is good at pinpointing the current behavior of code in a single instance of an application. They are compatible and both can be used in together.

**David:** Static PGO systems are particularly strong for use in situations where the behavior of an application is both testable outside of production, and in scenarios where startup performance is an important concern. Dynamic PGO systems have the potential for the best throughput in performance, but tend to have performance problems and unpredictable behavior during the startup phases of applications. It is possible to combine the two approaches, and that is what we seek to do in .NET 6. We will provide a static PGO system which can reduce the startup time spent jitting, and draw out some of the throughput benefits of PGO, as well as building a dynamic PGO system that can optionally be enabled for developers seeking the best throughput performance.

## Are there downsides to PGO?

**David:** Yes. Static PGO systems rely on the ability to measure the common behavior of applications in a test environment and provide that feedback back to the compiler. This is often very difficult for application developers to use successfully. In contrast, dynamic PGO systems are typically very simple to enable, but will inject difficult to predict latency into the startup phase of applications, as well as potentially behave differently on different runs in a way which causes unpredictable application performance.

**Andy:** Traditionally PGO has been somewhat awkward to use. We're hoping some of our work reduces some of this and makes PGO accessible enough that many applications can benefit from it.

## What are the hard problems that need to be solved to make a best-in-class PGO solution for .NET?

**Andy:** One problem is that the JIT isn't (wasn't) really set up to take advantage of this information. While it seems simple enough, the jit is continually modifying its model for a program as it is compiling it, and making sure the PGO data remains accurate and consistent as compilation progresses is a challenge. Another is that with PGO data you actually want to alter your overall compilation strategy and enable optimizations you might not do without PGO, like guarded devirtualization. So we've been spending a fair amount of time working on up-levelling the capabilities of the JIT.

**David:** PGO relies on a data pipeline of interesting data about application execution as well as algorithms which take advantage of that data. Hard problems include:

* Building a data pipeline that moves data from one execution to the next. This may be in-process, in the case of dynamic PGO, and across multiple processes in the case of static PGO. The data collected comes in many subtly different forms, and is used for several purposes which makes this a need for a general purpose data format. To provide a best-in-class solution this pipeline needs to be generally useable for both models, and even for both models at once, as well as being resilient to changes in the application from build to build.
* In addition, there are many complex algorithms that need to exist to utilize this data effectively, as just having data is not useful.These algorithms include everything from effects on the register allocator and basic block layout in the JIT, to evaluation of which methods should be ahead-of-time compiled, to algorithms which adjust where in memory code is placed. These complex algorithms are a work in progress in .NET 6 and will continue to evolve for many years.

**Andy:** Another challenge is that for static PGO, the profile data must be collected, extracted to some persistent storage, and then reapplied to future runs of the application. The data is both voluminous and fragile, so we've spent a fair amount of effort finding the right representation and building appropriate processing.

**David:** Another really hard problem is building a model where developers can successfully collect data from their application in production as it really executes. We have some ideas on how to build this, but it is quite a difficult problem to make reliable and simple to use.

## Are we seeing a scenario where the average developer will use PGO on their desktop machine? Who do you see using the PGO tools?

**David:** In the near term, I do not expect the average developer to use the PGO tools on their desktop machine, but they should be able to enable our dynamic PGO support to achieve throughput wins. However, we do use the static PGO data from a number of representative applications to produce the binaries that are distributed by Microsoft. Thus while an average developer will not use our static PGO tools, they will receive some of the benefit they provide.

**Andy:** We are hoping to make PGO easy enough to use that average developers will be able to use it successfully. Dynamic PGO in particular will be easy to enable, as it is just a new mode of behavior for the runtime, and does not require any other changes.

## Is there a model where PGO will be easy, or possibly an “easy mode” that gets 80%+ of the benefit?

**Andy:** Yes. Dynamic PGO. It is an extension of the tiered compilation we added in the [.NET Core 3.0 release](https://devblogs.microsoft.com/dotnet/announcing-net-core-3-0/). Apps that use and benefit from tiered compilation will likely see further benefit from Dynamic PGO.

**David:** This is a hard question to answer, as what is the definition of benefit. Some developers will prioritize startup time, some will prioritize throughput, some are concerned about P99 latency, some are concerned with file size. PGO can be used to attempt to address any of these concerns. I expect that the dynamic PGO support will be used by many developers to achieve throughput wins, but the other concerns are often more difficult to address with an "easy mode".

## Are there scenarios that PGO data is super bad and your program grinds to a halt?

**David:** We don't expect bad PGO data to be able to have that effect in our system. It may slow the performance of an application down somewhat, but a program grinding to a halt should never happen.

**Andy:** Right, I don't see it causing a "bad" behavior ... the worst case is that your program behaves quite differently than it did when PGO was gathering its data. Our ambition is not to degrade performance if that happens.

## .NET has had PGO tools for a long time and they’ve always been closed source. Why open those tools up now? Does this have any relationship to crossgen2?

**David:** In more recent versions of .NET we've been experimenting with a number of new technologies. In particular, tiered compilation and crossgen2. With tiered compilation, our runtime has gained support for substantially more dynamic compilation behavior which opened up the possibilities for dynamic PGO, and with crossgen2, we've had a chance to revisit our entire ahead-of-time compiler stack. As both of these features are maturing it has given us a chance to revisit the PGO space, and provide a simpler, and more powerful set of PGO infrastructure. For many years we've wanted our customers to be able to use our PGO technology, but the previous incarnation was incredibly difficult to use, and attempts to commercialize it into something that a developer might use were unsuccessful. In addition, with the advent of .NET Core we've rekindled interest in the .NET community targeting high performance systems, so we believe there is more customer interest now than there ever was in the past.

**Andy:** The tools used in .NET in the past didn't realize the full potential of PGO. And they were proprietary in part because using them was difficult. With the advent of things like tiered compilation and Crossgen2 we're now able to address the usability issues and are able to realize more of the potential benefit.

## Closing

Thanks to Andy and David for sharing their insights on PGO. I'm looking forward to static and dynamic PGO being commonly used features that make .NET apps start and run faster. As both Andy and David suggest, PGO is an area that has lots of potential for investment in future .NET versions. It is one of the most promising projects for the runtime.
