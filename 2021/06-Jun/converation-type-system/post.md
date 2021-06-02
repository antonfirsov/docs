---
post_title: Conversation about the .NET type system
username: rlander@microsoft.com
microsoft_alias: rlander@microsoft.com
categories: .NET
desired_publication_date: 06/03/2021
summary: Conversation with .NET engineers about the .NET type system.
---

The .NET or Common Language Runtime (CLR) type system is the foundation of the .NET programming model. We often talk about `System.Object` being the base of the type system, but it's really the base of all (reference) types. The type system is (at least) one step lower than that. It defines that both reference and value types exist, that strings are immutable, that single-inheritence is allowed and multiple-inheritence is not, and that generics are a runtime concept. On the other hand, it doesn't define concepts like `IEnumerable<T>`, `IDisposable`, or `Task<T>`, but it enables those `T`s to exist. You can quickly see that the type system is one of the most pervasive concepts in .NET.

We’re using the [conversation format](https://devblogs.microsoft.com/dotnet/category/conversations/) again, this time with runtime engineers who work on the type system and related topics.

* [David Wrighton](https://github.com/davidwrighton)
* [Jared Parsons](https://github.com/jaredpar)

## What is the purpose of a type system and why do they vary so much across languages?

**David:** The purpose of a type system is to define and control the way in which data and code are arranged within an application. They vary so much across languages as there are many different ways to arrange code and data that make sense, and are useful.

**Jared:** The type system is the building blocks and rules on which the language features are built. They vary so much across languages because language differentiation is often achieved by starting from a different set of building blocks and rules on which it operates.

## What are the aspects of the CLR type system that you think are the most apparent to .NET developers?

**David:** I see two different aspects of the type system as most apparent to .NET developers.

1. Code and data are held in constructs like value types and classes, which encourages developers to build object oriented systems.
1. The CLR type system's smallest redistributable unit is the assembly, or .dll. This has profound implications on how applications are structured, and developed.

**Jared:** In general I think the split between `struct` and `class` is the aspect that becomes the most apparent to .NET developers. It's essentially two ways of defining very similar objects with similar capabilities but the "modifier" on the type significantly changes how the type is used by consumers. It's not common in other languages and runtimes

## The line between CLR and C# concepts can be murky. How do you think of the new record types? They are a class that behaves more like a struct. Is that good?

**David:** "Is that good" probably isn't the right question here. A better question would more like "Is that useful?" And I believe they are useful, as it makes it simple for one developer to communicate to others that some objects aren't an object with behaviors, but more just pure data. Also, the new record types are much less effort to use than building the equivalent logic would have been in past versions of C#.

One thing to remember about the line between CLR and C# concepts is that CLR concepts provide the possibility to make some logic work, and C# concepts provide an interface for actual developers to work with. The C# concepts are an opinionated view on the possible programs that can be written using CLR concepts, and over time, the developers of the C# language have found ways for programmers to more clearly and succinctly represent intent on a fairly regular cadence, while the fundamental capabilities provided by CLR concepts are typically much more slow to evolve.

**Jared:** I often divide types into whether they primarily provide data or behavior. In the case they provide data I often want a number of features to come along with it: immutability, equality, deconstruction, etc ... Essentially I want the data objects to fit into all of the C# features that allow me to explore data. Records are a declarative syntax for letting me define data objects that get all of these features for free. 
 
Having classes that behave like values has always been possible in C# and there are many types in the framework that already do this. Generally though these classes fall into the category of "data" style objects, `Tuple<>` for example. It's not good or bad to do this, it's instead an exercise in evaluating trade offs: heap vs. stack, cost of passing / returning, etc ...

In the case of records we wanted to explore classes first because that is what most of the customers who valued records were already using. In future versions of the language we will allow for them to be declared as structs as well though to help customers who need to make different trade off decisions.

## Value types and structs. Same thing, right?

**David:** In how I work with the type systems, yes. Value type is the CLR term for what is exposed in C# as a struct.

**Jared:** Yes they're the same thing. Except in the case of ValueType which is a fairly special value type. It's the base type of all value types even though value types can't inherit from other types.

## How about structs vs classes?

**Jared:** When discussing the difference between struct and class most people tend to focus on how structs default to allocated on the stack and classes are allocated on the heap. I tend to think about them a bit differently. A struct is in many ways a loose grouping of fields while a class is a firm container around a group of fields. When you assign structs together it essentially comes down to a field by field assignment where as assigning classes together is always a single pointer assignment. Understanding this gives you a better sense of the trade offs between the two types: whether or not assignments are atomic, how they are laid out in memory, what level of control the type has over it's contents, etc ... 

## There has been several low-level type system changes across CLR and C# in recent years, like ref structs and `Span<T>`, ref returns, covariant return types, default interface methods, and static interface methods. On the one hand, those are great because they are targeted at performance and other needs. On the other, they are not something that most developers will use. When is the next type system feature coming that the average developer uses?

**Jared:** I think it will be static virtual interface methods. That is a feature we will be previewing in C# 10 that allows customers to use static methods on type parameters inside generic methods. On the face this may seem like an advanced concept which wouldn't have broad user reach, however it opens the door to us defining generic math methods. Essentially allowing us to express mathmetical algorithms in terms of any numeric type vs. today where we have to limit to a specific type like `double`, `int`, etc ... This capability is present and popular in a number of other languages and I think we will see similar usages in .NET once the feature is available.

**David:** I don't know. One possibility is the static virtual interface methods feature we are working on in preview. Another possibility that I have been experimenting with is a form of specialized generic code, but we haven't seen much need for that in our community yet.

## Some of these new features press the bounds of safety. Is that OK?

**Jared:** They press the bounds of safety but in a way that doesn't push the burden to customers. The rules around `Span<T>` are quite involved and took many months to refine, verify and make workable with common coding patterns. The burden here though was primarily on the .NET team to stretch the boundaries here and see what we could achieve.

The result is the customer can consume `Span<T>` and get the performance without worrying about the safety issues: the language simply prevents you from doing unsafe operations as it also does for other features. The customer needs to learn a bit about the new rules but they don't have to worry about the safety.

**David:** Yes. The CLR and .NET ecosystem have always embraced code following a spectrum of safety. We strive to make normal code safe, and potentially unsafe code is something that generally has to be opted in using some mechanism, (P/Invoke, Unsafe APIs, Marshal APIs, unsafe code blocks, etc.) In the spectrum of type system provided safety rules .NET/C# exists in a fairly pragmatic place where unsafe code is possible, but we strive to make it difficult to accidentally invoke potentially unsafe behavior.

## .NET is often compared to Java, and the biggest differences are value types and reified/runtime generics. Why are those two type system features valuable? Would you repeat those choices if you could re-design .NET?

**David:** These two type system features are valuable as they allow the way a program is executed to be expressed in code instead of merely expressing the semantic meaning of the code. This is an interesting philosophical design difference between the .NET and Java type systems, where the Java type system less often expresses details on exactly how program semantics should be implemented. This has certain benefits and drawbacks, but I believe I would likely repeat those decisions if I could re-design .NET although I would probably change a few details.

**Jared:** The advantage of value types is avoiding heap allocations and by extension reducing the pressure on the garbage collector. That is an invaluable tool for high performance code which often needs to ensure a GC will not happen on a given code path. I would definitely want to keep them if we were starting .NET from scratch but I would likely invest in changing how they were expressed. Rather than expressing them at the declaration, essentially having a differentiation between classes and structs, I'd explore if we could express it at usage time. Essentially have a single kind of type and at the use case decide between heap or stack allocation. That is a really difficult problem to solve without pushing a lot of complextiy to the customer. Enough that I think such a redesign may end up where we are today because it's a very pragmatic trade off. 

## Let’s dive a little deeper. What’s an everyday scenario that’s made better by value types?


## How have value types been optimized and improved over the years?


## Same with runtime generics.


## We just had a conversation with the interop team. What CLR type system concepts do you think are the hardest to interop with other systems?


## I watched a video recently with Chris Lattner. He made it sound like Swift works as well with `int` in the standard library as it does with a user defined `quaternion` type. That's not true of .NET, right? Or is this more a C# topic?

https://www.youtube.com/watch?v=nWTvXbQHwWs&t=2545s
Also interesting: https://www.youtube.com/watch?v=nWTvXbQHwWs&t=1806s


## The team has done a lot of work to move the type system to C#, pulling it out the runtime. How is that even possible?


## Your role – at least in part – is working on the CLR type system. What does that entail?


## I want to talk about two big challenges I’ve observed with the type system over the years. The first relates to representation and description. It seems like it’s really challenging maintaining semantics while keeping file size small. That’s not an obvious problem to most people.


## The second – and this applies more to Native AOT – is closing the (infinite) set of generic instantiations that can exist. Aren’t generic wonderful and horrible at the same time? This strikes me as one the biggest challenges – more so than Reflection – for adopting native AOT pervasively in .NET.


## Closing

The type system is both the result of intentional design decisions made by the original architects of .NET and the outcome of years of organic change based on the needs of .NET users. You can likely see that the type system will continue to evolve as new scenerios and requirements present themselves. Only time will tell what those new type system capabilities will be.

Thanks again to Jared and David for sharing their insights on the type system.
