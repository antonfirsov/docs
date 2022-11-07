---
post_title: Announcing F# 7
author1: vlza@microsoft.com
post_slug: announcing-fsharp-7
username: vlza@microsoft.com
microsoft_alias: vlza
featured_image: fsharp512.png
categories: F#, .NET
tags: .net 7, f# 7
summary: F# 7 is here, see what's new!
desired_publication_date: 2022-11-08
post_date: 2022-11-08 10:05:00
---

We’re happy to announce the availability of F# 7, shipping with .NET 7 and Visual Studio 2022. Check out this next step to making it easier for you to write robust, succinct and performant code. You can get F# 7 in the following ways:

* [Install the latest .NET 7 SDK](https://dotnet.microsoft.com/download/dotnet/7.0)
* [Install Visual Studio 2022 preview](https://visualstudio.microsoft.com/vs/preview/)
* [Use .NET Polyglot Notebooks in Jupyter or VS Code](https://github.com/dotnet/interactive#notebooks-with-net)

F# 7 continues the journey to make F# simpler and more performant as well as improving interop with new C# features. [Syntax for the F# specific feature SRTP is simplified](#making-working-with-srtps-easier). Support has been added for [Static abstract members support in interfaces](#static-abstract-members-support-in-interfaces) and consumption of [C# `required` members and `init` scope](#required-properties-checking). The F# compiler is improved to offer [reference assemblies](#reference-assemblies-support), [trimmability, better code gen, ARM64 support, performance enhancements and bug fixes](#other-changes).

To learn more about F#, see [the .NET guide to F#](https://dotnet.microsoft.com/languages/fsharp) and the excellent overview talks at the [.NET Conf Focus Day on F#](https://channel9.msdn.com/Events/dotnetConf/Focus-on-FSharp). The F# community provide resources through [the F# Software Foundation (fsharp.org)](https://fsharp.org). If you're interested in more advanced topics, see [Fast F#](https://www.youtube.com/c/FastFSharp) series by [Matthew Crews](https://twitter.com/FastFSharp), the [F# community compiler sessions](https://www.youtube.com/c/fsharporg), this interview with F# designer Don Syme ([part 1](https://www.youtube.com/watch?v=hNAb04V4liA&t=1s&ab_channel=ContextFree) / [part 2](https://www.youtube.com/watch?v=x4j7LQApfEY&t=25s&ab_channel=ContextFree)) and [Betatalks #66](https://www.youtube.com/watch?v=IHoasuxy1C0&ab_channel=Betabit) .

To stay up to date, check out the community-provided [F# Weekly](https://sergeytihon.com/fsharp-weekly/), follow [@fsharporg](https://twitter.com/fsharporg) on Twitter, and join the [F# Software Foundation](https://foundation.fsharp.org/about) to get the latest news and updates.

## Static abstract members support in interfaces

We are adding a way of declaring, calling and implementing interfaces with static abstract methods, described in [F# RFC 1124](https://github.com/fsharp/fslang-design/blob/main/FSharp-7.0/FS-1124-interfaces-with-static-abstract-members.md).
This is a new feature in .NET 7, matching the [corresponding C# feature](https://learn.microsoft.com/dotnet/csharp/whats-new/tutorials/static-virtual-interface-members). One notable application is [generic math](https://learn.microsoft.com/dotnet/csharp/whats-new/tutorials/static-virtual-interface-members#generic-math).

First, let's declare an interface with a static abstract member:

```fsharp
type IAddition<'T when 'T :> IAdditionOperator<'T>> =
    static abstract op_Addition: 'T * 'T -> 'T
```

> **Note** The code above will produce `FS3535` warning - `Declaring "interfaces with static abstract methods" is an advanced feature. See <https://aka.ms/fsharp-iwsams> for guidance. You can disable this warning by using '#nowarn "3535"' or '--nowarn:3535'.`

Next, we can implement it (`'T * 'T` is the F# signature for a function with two parameters of type `T):

```fsharp
type IAddition<'T when 'T :> IAddition<'T>> =
    static abstract op_Addition: 'T * 'T -> 'T

type Number<'T when IAddition<'T>>(value: 'T) =
    member _.Value with get() = value
    interface IAddition<Number<'T>> with
        static member op_Addition(a, b) = Number(a.Value + b.Value)
```

This will allow us to write generic functions that can be used with any type that implements `IAddition` interface:

```fsharp
let add<'T when IAddition<'T>>(x: 'T) (y: 'T) = 'T.op_Addition(x,y)
```

or in the operator form:

```fsharp
let add<'T when IAddition<'T>>(x: 'T) (y: 'T) = x + y
```

This is a runtime feature and can be used with any static methods or properties, you can see a few more examples below.

```fsharp
type ISinOperator<'T when 'T :> ISinOperator<'T>> =
    static abstract Sin: 'T -> 'T

let sin<'T when ISinOperator<'T>>(x: 'T) = 'T.Sin(x)
```

This feature can also be used with BCL built-in types (such as `INumber<'T>`), for example:

```fsharp
open System.Numerics

let sum<'T when INumber<'T>>(values: seq<'T>) =
    let mutable result = 'T.Zero
    for value in values do
        result <- result + value
    result
```

These simplified examples show how interfaces with static abstract methods work.

This is an advanced feature with potential serious methodological drawbacks if used inappropriately. To learn more about the pros and cons of using the feature in F# methodology, see the [drawbacks](https://github.com/fsharp/fslang-design/blob/main/FSharp-7.0/FS-1124-interfaces-with-static-abstract-members.md#drawbacks) and [guidance](https://github.com/fsharp/fslang-design/blob/main/FSharp-7.0/FS-1124-interfaces-with-static-abstract-members.md#guidance) sections of the RFC. In particular, you should not use this feature for application code, nor for library code whose specification may be subject to significant change.

Because of the potential drawbacks, we will emit a [warning when interfaces with static abstract methods are declared and implemented in F#](https://github.com/fsharp/fslang-design/blob/main/FSharp-7.0/FS-1124-interfaces-with-static-abstract-members.md#warning-when-declaring-iwsams). Additionally, a warning is emitted when [interfaces with static abstract methods are used as types, not type constraints](https://github.com/fsharp/fslang-design/blob/main/FSharp-7.0/FS-1124-interfaces-with-static-abstract-members.md#warning-when-using-iwsams-as-types). Both these warnings can be suppressed.

## Making working with SRTPs easier

F# SRTPs or [Statically Resolved Type Parameters](https://learn.microsoft.com/dotnet/fsharp/language-reference/generics/statically-resolved-type-parameters) are type parameters that are replaced with an actual type at compile time instead of at run time. F# 7 simplifies the syntax used for defining SRTPs through [F# RFC 1083](https://github.com/fsharp/fslang-design/blob/main/FSharp-7.0/FS-1083-srtp-type-no-whitespace.md) and [F# RFC 1124](https://github.com/fsharp/fslang-design/blob/main/FSharp-7.0/FS-1124-interfaces-with-static-abstract-members.md). This aligns SRTPs and interfaces with static abstract members and makes it easier to interconvert between these two approaches to generic math.

As a quick refreshment on what SRTPs are, and how they're used, let's declare a function named `average` that takes an array of type `T` where type `T` is a type that has at least the following members:

* An addition operator (`(+)` or `op_Addition`)
* A `DivideByInt` static method, which takes a `T` and an `int` and returns a `T`
* A static property named `Zero` that returns a `T`

```fsharp
let inline average< ^T
                   when ^T: (static member (+): ^T * ^T -> ^T)
                   and  ^T: (static member DivideByInt : ^T * int -> ^T)
                   and  ^T: (static member Zero : ^T)>
                   (xs: ^T array) =
    let mutable sum : ^T = (^T : (static member Zero: ^T) ())
    for x in xs do
        sum <- (^T : (static member op_Addition: ^T * ^T -> ^T) (sum, x))
    (^T : (static member DivideByInt: ^T * int -> ^T) (sum, xs.Length))
```

`^T` here is a type parameter, and `^T: (static member (+): ^T * ^T -> ^T)`, `^T: (static member DivideByInt : ^T * int -> ^T)`, and `^T: (static member Zero : ^T)` are constraints for it.

We are making some improvements here. First, you no longer need to use a dedicated type parameter character (`^`), a single tick character (`'`) can be used instead, the compiler will decide whether it's static or generic based on the context - whether the function is `inline` and if it has constraints.

Next, we are also adding a new simpler  syntax for calling constraints, which is more readable and easier to write:

```fsharp
let inline average<'T
                when 'T: (static member (+): 'T * 'T -> 'T)
                and  'T: (static member DivideByInt : 'T * int -> 'T)
                and  'T: (static member Zero : 'T)>
                (xs: 'T array) =
    let mutable sum = 'T.Zero
    for x in xs do
        sum <- sum + x
    'T.DivideByInt(sum, xs.Length)
```

Finally, we've added the ability to declare constraints in groups:

```fsharp
type AverageOps<'T when 'T: (static member (+): 'T * 'T -> 'T)
                   and  'T: (static member DivideByInt : 'T*int -> 'T)
                   and  'T: (static member Zero : 'T)> = 'T
```

And a simpler syntax for self-constraints, which are constraints that refer to the type parameter itself:

```fsharp
let inline average<'T when AverageOps<'T>>(xs: 'T array) =
    let mutable sum = 'T.Zero
    for x in xs do
        sum <- sum + x
    'T.DivideByInt(sum, xs.Length)
```

The simplified call syntax also works with instance members:

```fsharp
type Length<'T when 'T: (member Length: int)> = 'T
let inline len<'T when Length<'T>>(x: 'T) =
    x.Length
```

Together these changes make working with SRTPs easier and more readable.

## Required properties checking

C# 11 introduced new [required modifier for properties](https://learn.microsoft.com/dotnet/csharp/language-reference/keywords/required), F# 7 supports consuming classes with required properties and enforcing the constraint:

Consider the following data type, defined in the C# library:

```csharp
public sealed class Person
{
    public required string Name { get; set; }
    public required string Surname { get; set; }
}
```

When using from F# code, the F# compiler will make sure required properties are getting properly initialized:

```fsharp
let person = Person(Name = "John")
```

The code above will compile correctly, but if we try to omit any of the required properties, we will get a compile-time diagnostic:

```fsharp
FS3545: The following required properties have to be initalized:
    property Person.Surname: string with get, set
```

### Init scope and init-only properties

In F# 7 we are tightening the rules for `init-only` properties so that they can only be initialized within the `init` scope. This is a new compile-time check and is a breaking change. It will only be applied starting F# 7, see examples below.

Given the following C# data type:

```csharp
public sealed class Person
{
    public int Id { get; init; }
    public int Name { get; set; }
    public int Surname { get; set; }
    public Person Set() => this;
}
```

Before, in F# 6, the following code would've compiled and mutated the property `Id` of the `Person`:

```fsharp
let person = Person(Id = 42, Name = "John", Surname = "Doe")
person.Id <- 123
person.set_Id(123)
person.Set(Id=123)
```

In F# 7, we are changing this to be a compile-time error:

```csharp
Error FS0810 Init-only property 'Id' cannot be set outside the initialization code. See https://aka.ms/fsharp-assigning-values-to-properties-at-initialization
Error FS0810 Cannot call 'set_Id' - a setter for init-only property, please use object initialization instead. See https://aka.ms/fsharp-assigning-values-to-properties-at-initialization
Error FS0810 Init-only property 'Id' cannot be set outside the initialization code. See https://aka.ms/fsharp-assigning-values-to-properties-at-initialization
```

## Reference assemblies support

Starting F# 7, the F# compiler can generate and properly consume [reference assemblies](https://learn.microsoft.com/dotnet/standard/assembly/reference-assemblies).

You can generate reference assemblies by:

* Adding `ProduceReferenceAssembly` MSBuild project property to your `fsproj` (`<ProduceReferenceAssembly>true</ProduceReferenceAssembly>`) file or msbuild flags (`/p:ProduceReferenceAssembly=true`).
* Using `--refout` or `--refonly` compiler options.

We are looking forward to your feedback on this feature.

## F# self-contained deployments & Native AOT

While [F# apps can already be trimmed](https://learn.microsoft.com/dotnet/core/deploying/trimming/trim-self-contained) and [compiled to the native code](https://learn.microsoft.com/dotnet/core/deploying/native-aot/) in most cases, we are working on making this experience even better.

In F# 7 we are introducing number of improvements:

* A new codegen flag for F# compiler `--reflectionfree` - it will allow skipping emitting of automatic (`%A`, reflection-based) `ToString` implementation for records, unions and structs.
* Trimmabillity improvements for `FSharp.Core` - added `ILLink.LinkAttributes.xml` and `ILLink.LinkAttributes.xml` for `FSharp.Core`, which allows trimming compile-time resources and attributes for runtime-only FSharp.Core dependency.

We will continue to work on improving the experience of F# self-contained deployments and Native AOT for the next version of F# and core library.

## Other changes

Other changes in F# 7 include:

* Added support for N-d arrays up to rank 32.
* Result module functions parity with Option.
* Fixes in resumable state machines codegen for the tasks builds.
* Better codegen for compiler-generated side-effect-free property getters.
* ARM64 platform-specific compiler and ARM64 target support in F# compiler. Dependency manager `#r` caching support.
* Parallel type-checking and project-checking support (experimental, can be enabled via VS setting, or by tooling authors).
* Miscellaneous bugfixes and improvements.

RFCs for F# 7 can be found [here](https://github.com/fsharp/fslang-design/tree/main/FSharp-7.0).

## General Improvements in .NET 7

.NET 7 brought a myriad of improvements, which F# 7 will benefit from - [various arm64 performance improvements](https://devblogs.microsoft.com/dotnet/arm64-performance-improvements-in-dotnet-7/), and [general performance improvements](https://devblogs.microsoft.com/dotnet/performance_improvements_in_net_7/).

## Thanks and Acknowledgments

F# is developed as a collaboration between the .NET Foundation, the F# Software Foundation, their members and other contributors including Microsoft. The F# community is involved at all stages of innovation, design, implementation and delivery and we're proud to be a contributing part of this community.

Many people have contributed directly and indirectly to F# 7, including all who have contributed to the F# Language Design Process through [fsharp/fslang-suggestions](https://github.com/fsharp/fslang-suggestions/) and [fsharp/fslang-design](https://github.com/fsharp/fslang-design) repositories.

[Adam Boniecki](https://github.com/abonie), [Brett V. Forsgren](https://github.com/brettfo), [Don Syme](https://github.com/dsyme), [Jon Sequeira](https://github.com/jonsequitur) [Kevin Ransom](https://github.com/KevinRansom), [Petr Pokorný](https://github.com/0101), [Petr Semkin](https://github.com/psfinaki), [Tomáš Grošup](https://github.com/T-Gro), [Vlad Zarytovskii](https://github.com/vzarytovskii), [Will Smith](https://github.com/TIHan) contributed directly as part of F# team at Microsoft.

[Florian Verdonck](https://github.com/nojaf) contributed [72 PRs](https://github.com/dotnet/fsharp/commits?author=nojaf&since=2021-10-13&until=2022-10-15), including work on parallel type-checking improvements, tons of improvements to the syntax trees, which benefit **all** tooling in general and [Fantomas](https://github.com/fsprojects/fantomas#fantomas) source code formatter in particular.

[Eugene Auduchinok](https://github.com/auduchinok) contributed 26 [PRs](https://github.com/dotnet/fsharp/commits?author=auduchinok&since=2021-10-13&until=2022-10-15) including a bunch of improvements and optimizations in the parser, IL reading, completions, compiler service caching and more.

**Our new contributor** [Edgar Gonzalez](https://github.com/edgarfgp) 🥳 contributed [17 PRs](https://github.com/dotnet/fsharp/commits?author=edgarfgp&since=2021-10-13&until=2022-10-15) with tons of improvements and fixes around attributes processing, RequiredQualifiedAccess & lower-cased DU cases relaxations and whole bunch of bugfixes.

[kerams](https://github.com/kerams) contibuted [13 PRs](https://github.com/dotnet/fsharp/commits?author=kerams&since=2021-10-13&until=2022-10-15) for records, types, unions, enums, lambda variables completions, IL gen improvements, and more.

Other direct contributors to the [dotnet/fsharp](https://github.com/dotnet/fsharp) repository in the F# 7.0 time period include [teo-tsirpanis](https://github.com/dotnet/fsharp/commits?author=teo-tsirpanis&since=2021-10-13&until=2022-10-15), [DedSec256](https://github.com/dotnet/fsharp/commits?author=DedSec256&since=2021-10-13&until=2022-10-15), [baronfel](https://github.com/dotnet/fsharp/commits?author=baronfel&since=2021-10-13&until=2022-10-15), [leolorenzoluis](https://github.com/dotnet/fsharp/commits?author=leolorenzoluis&since=2021-10-13&until=2022-10-15), [dawedawe](https://github.com/dotnet/fsharp/commits?author=dawedawe&since=2021-10-13&until=2022-10-15), [tmat](https://github.com/dotnet/fsharp/commits?author=tmat&since=2021-10-13&until=2022-10-15), [marcin-krystianc](https://github.com/dotnet/fsharp/commits?author=marcin-krystianc&since=2021-10-13&until=2022-10-15), [cartermp](https://github.com/dotnet/fsharp/commits?author=cartermp&since=2021-10-13&until=2022-10-15), [pirrmann](https://github.com/dotnet/fsharp/commits?author=pirrmann&since=2021-10-13&until=2022-10-15), [safesparrow](https://github.com/dotnet/fsharp/commits?author=safesparrow&since=2021-10-13&until=2022-10-15), [omppye](https://github.com/dotnet/fsharp/commits?author=omppye&since=2021-10-13&until=2022-10-15), [Happypig375](https://github.com/dotnet/fsharp/commits?author=Happypig375&since=2021-10-13&until=2022-10-15), [ncave](https://github.com/dotnet/fsharp/commits?author=ncave&since=2021-10-13&until=2022-10-15), [thinkbeforecoding](https://github.com/dotnet/fsharp/commits?author=thinkbeforecoding&since=2021-10-13&until=2022-10-15), [MichaelSimons](https://github.com/dotnet/fsharp/commits?author=MichaelSimons&since=2021-10-13&until=2022-10-15), [tboby](https://github.com/dotnet/fsharp/commits?author=tboby&since=2021-10-13&until=2022-10-15), [mmitche](https://github.com/dotnet/fsharp/commits?author=mmitche&since=2021-10-13&until=2022-10-15), [nosami](https://github.com/dotnet/fsharp/commits?author=nosami&since=2021-10-13&until=2022-10-15), [jonfortescue](https://github.com/dotnet/fsharp/commits?author=jonfortescue&since=2021-10-13&until=2022-10-15), [smoothdeveloper](https://github.com/dotnet/fsharp/commits?author=smoothdeveloper&since=2021-10-13&until=2022-10-15), [abelbraaksma](https://github.com/dotnet/fsharp/commits?author=abelbraaksma&since=2021-10-13&until=2022-10-15), [uxsoft](https://github.com/dotnet/fsharp/commits?author=uxsoft&since=2021-10-13&until=2022-10-15).

Many other people are contributing to the rollout of F# 7 and .NET 7 in [Fable](https://fable.io/), [Ionide](https://ionide.io/), [Bolero](https://fsbolero.io/), [FSharp.Data](https://fsprojects.github.io/FSharp.Data/), [Giraffe](https://github.com/giraffe-fsharp/Giraffe#giraffe), [Saturn](https://github.com/SaturnFramework/Saturn), [SAFE Stack](https://safe-stack.github.io/), [WebSharper](https://websharper.com/), [FsCheck](https://fscheck.github.io/FsCheck/), [DiffSharp](https://diffsharp.github.io/), [Fantomas](https://github.com/fsprojects/fantomas#fantomas) and other community-delivered technologies.

## What's next?

We will continue working on improving both F# language as well as tooling, including, but not limited to various VS features and improvements, compiler performance, language simplicity, new language features and more. If you're interested in following our progress, you [check out our tracking issues](https://github.com/orgs/dotnet/projects/126/views/17). If contributing to the compiler and tooling is something that interests you more, please check out our ["help wanted" issues list](https://github.com/dotnet/fsharp/issues?q=is%3Aissue+is%3Aopen+sort%3Aupdated-desc+label%3A%22help+wanted%22), or feel free to drop a question in [discussions](https://github.com/dotnet/fsharp/discussions).

## Contributor Showcase

In this and future announcements, we will highlight some of the individuals who contribute to F#. This text is written in the contributors’ own words:

### Edgar Gonzalez

> I'm Edgar Gonzalez, and I live between Albacete(Spain) and London(Uk), where I'm a Mobile Developer at Fund Ourselves.
>
> I'm new to the .NET ecosystem. Previously I was a member of the Spanish Paratroopers Brigade "Almogávares" VI, where I served as a Corporal first class (2004-2017).
>
> I started programming five years ago, interested in mobile development with C#, and around the same time, I was introduced to F# by [Frank A. Krueger](https://twitter.com/praeclarum) using Xamarin.iOS
>
> Last year I moved to F#, and during my journey realized that there is room for improvement regarding compiler error reporting, so I decided to get involved and help make F# simple for newcomers like me.
>
> Why do I like F#? it has a clean and lightweight syntax, is expression-oriented, exhaustive pattern matching, and discriminated unions.
>
> I look forward to continuing to help F# to be even better, focusing on making it easy for newcomers.
>
> Thanks to [Timothé Larivière](https://twitter.com/Tim_Lariviere) and [Florian Verdonck](https://twitter.com/verdonckflorian) for helping me get started with open source.

![Contributor photo](EdgarGonzalez.jpg)

### Florian Verdonck

> I’m Florian Verdonck, an independent software consultant with a passion for open-source development. My F# open-source journey started in 2017, as part of the [F#
foundation mentorship program](https://fsharp.org/mentorship/). My mentor, [Anthony Lloyd](https://anthonylloyd.github.io/), and I started
contributing to the [Fantomas](https://fsprojects.github.io/fantomas/) project. Maintaining led to contributing, and later, I
was adding to F# editor and compiler tooling. Several great mentors helped me get
my contributions accepted and my pull requests merged in and I’m grateful to all of
them.
>
> Being inspired to contribute to the wonderful F# community is one thing. Having the
resources to do it properly is another. Luckily, I found customers that share my
vision of improving open source by active collaboration.
>
> I’ve been fortunate to work with the [open-source division](https://opensource.gresearch.co.uk/) of G-Research, a leading
quantitative finance research firm. It is actively contributing to the open-source
software it uses in-house. The leaders there believe that targeted open-source
efforts can improve the operational efficacy of their engineers and encourage me in
my work.

![Contributor photo](FlorianVerdonck.jpg)

### Janusz Wrobel

> I'm Janusz Wrobel, a software developer currently living in the UK.
>
> My programming journey, as for many, started with home projects in different technologies.
> My first encounter with professional software projects was in .NET, which became my primary development ecosystem.
>
> A few years ago I encountered F# and I really liked it. Besides obvious advantages like conciseness and low overhead coding, I think that the main benefit I got from being exposed to the language was the functional programming paradigm. It allowed me to think about the same problems in a completely different way, which can be applied in OOP languages as well as functional ones. Immutability and pure code are very powerful concepts that I think we will see more of in the future with the expansion of highly-parallel and distributed computations.
>
> Despite its advantages, F# can be further improved, and one of the areas for improvement I think is tooling performance. That's why I was excited to be able to contribute in this area. The prospect of seeing your changes have impact on the tooling you use everyday is an exciting one!
>
> When I'm not programming, you might find me trying my best at the football pitch, or watching my favourite football team play.
