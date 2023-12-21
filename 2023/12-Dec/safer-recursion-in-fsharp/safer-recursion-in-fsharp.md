---
post_title: Safer recursion in F#
author1: schaeferdav
post_slug: safer-recursion-in-fsharp
microsoft_alias: psemkin
featured_image: fsharprecursionfeature.jpg
categories: .NET, F#
tags: f#, recursion, algorithms
summary: Tail recursion is a new F# compiler feature which helps to avoid stack overflows.
post_date: 2023-12-28 10:05:00
---

> This is a guest blog post by **David Schaefer**. David is a freelancing software developer with a focus on functional programming. He's a member of the [G-Research](https://opensource.gresearch.com) Open Source Team. There he works on improving the F# ecosystem, mainly developer tooling. Furthermore, he helps maintaining various F# Open Source projects.

It's very common to define algorithms in a recursive way in functional programming. It fits very well with the mindset of avoiding mutation, and often there's no performance downside. The compiler tries to rewrite recursive definitions into more efficient loops during its optimization phase. 

However, the compiler is not always able to do this transformation into loops. And here is where the danger starts.

## Stack frames vs. Production

Calling a function `g` from inside a function `f` normally creates a new stack frame on the call stack of the process. After the function `g` is done, its stack frame isn't needed anymore and its space can be reused.

Now, with recursive functions, this stack frame creation can be vital to understand. In general, for every recursive call, a new stack frame is created. During development time, when problem sizes are rather small, this often doesn't cause an issue. But later in production, with bigger problem sizes, the program crashes all of a sudden with a stack overflow.

What happened? There were so many stack frames created for the recursive calls, that all space for the stack was used up and the runtime decided to stop it. To demonstrate this, take a look at this recursive function:
```fsharp
let rec countDown1 n =
    if n = 0
    then 0
    else
        countDown1 (n - 1) + 1
```

The generated IL code for it looks like this:
```csharp
.method public static 
    int32 countDown1 (
        int32 n
    ) cil managed 
{
    // Method begins at RVA 0x2050
    // Header size: 1
    // Code size: 17 (0x11)
    .maxstack 8

    IL_0000: nop
    IL_0001: ldarg.0
    IL_0002: brtrue.s IL_0006

    IL_0004: ldc.i4.0
    IL_0005: ret

    IL_0006: ldarg.0
    IL_0007: ldc.i4.1
    IL_0008: sub
    IL_0009: call int32 Program::countDown1(int32)
    IL_000e: ldc.i4.1
    IL_000f: add
    IL_0010: ret
} // end of method Program::countDown1
```

You can see the recursive call at `IL_0009`. Calling `coundDown1` with `n = 1_000`, like one would casually do during development, terminates just fine, but calling it with `n = 1_000_000` crashes with a stack overflow.

Compare this to the following:
```fsharp
let rec countDown2 n =
    if n = 0
    then 0
    else
        countDown2 (n - 1)
```
which results in:
```csharp
.method public static 
    int32 countDown2 (
        int32 n
    ) cil managed 
{
    // Method begins at RVA 0x2064
    // Header size: 1
    // Code size: 13 (0xd)
    .maxstack 8

    // loop start
        IL_0000: nop
        IL_0001: ldarg.0
        IL_0002: brtrue.s IL_0006

        IL_0004: ldc.i4.0
        IL_0005: ret

        IL_0006: ldarg.0
        IL_0007: ldc.i4.1
        IL_0008: sub
        IL_0009: starg.s n
        IL_000b: br.s IL_0000
    // end loop
} // end of method Program::countDown2
```

The difference is that in `countDown1` the returned value of the recursive call was used to construct the final value of the function by adding 1 to it. In contrast, in `countDown2` the returned value of the recursive call is also the value of the function itself. Meaning, the recursive call is the last instruction in the function definition - a style known as [tail recursion](https://en.wikipedia.org/wiki/Tail_call). This style allowed the compiler to transform the function into a loop, thus eliminating the need to create new stack frames.

Besides the chance for the compiler to rewrite a recursive function into a loop, there's another escape-door that opens up through the use of tail recursion: the IL prefix `tail.` In [ECMA-335](https://ecma-international.org/publications-and-standards/standards/ecma-335/), it is explained like this:

```txt
It indicates that the current method’s stack frame is no longer required and thus can be removed before the call instruction is executed. Because the value returned by the call will be the value returned by this method, the call can be converted into a cross-method jump.
```

To demonstrate it, we use the example from [RFC-1011](https://github.com/fsharp/fslang-design/blob/main/FSharp-8.0/FS-1011-warn-on-recursive-without-tail-call.md), more on that later. Consider the mutually recursive F# functions:
```fsharp
let foo x =
    printfn "Foo: %x" x

[<TailCall>]
let rec bar x =
    match x with
    | 0 ->
        foo x           // OK: non-tail-recursive call to a function which doesn't share the current stack frame (i.e., 'bar' or 'baz').
        printfn "Zero"
        
    | 1 ->
        bar (x - 1)     // Warning: this call is not tail-recursive
        printfn "Uno"
        baz x           // OK: tail-recursive call.

    | x ->
        printfn "0x%08x" x
        bar (x - 1)     // OK: tail-recursive call.
        
and [<TailCall>] baz x =
    printfn "Baz!"
    bar (x - 1)         // OK: tail-recursive call.
```
Here we get the following IL code for case `1` in `bar`:
```csharp
    ...
    IL_0030: ldarg.0
    IL_0031: ldc.i4.1
    IL_0032: sub
    IL_0033: call void Program::bar(int32)
    IL_0038: nop
    IL_0039: ldstr "Uno"
    IL_003e: newobj instance void class [FSharp.Core]Microsoft.FSharp.Core.PrintfFormat`5<class [FSharp.Core]Microsoft.FSharp.Core.Unit, class [System.Runtime]System.IO.TextWriter, class [FSharp.Core]Microsoft.FSharp.Core.Unit, class [FSharp.Core]Microsoft.FSharp.Core.Unit, class [FSharp.Core]Microsoft.FSharp.Core.Unit>::.ctor(string)
    IL_0043: stloc.0
    IL_0044: call class [netstandard]System.IO.TextWriter [netstandard]System.Console::get_Out()
    IL_0049: ldloc.0
    IL_004a: call !!0 [FSharp.Core]Microsoft.FSharp.Core.PrintfModule::PrintFormatLineToTextWriter<class [FSharp.Core]Microsoft.FSharp.Core.Unit>(class [System.Runtime]System.IO.TextWriter, class [FSharp.Core]Microsoft.FSharp.Core.PrintfFormat`4<!!0, class [System.Runtime]System.IO.TextWriter, class [FSharp.Core]Microsoft.FSharp.Core.Unit, class [FSharp.Core]Microsoft.FSharp.Core.Unit>)
    IL_004f: pop
    IL_0050: ldarg.0
    IL_0051: tail.
    IL_0053: call void Program::baz(int32)
    IL_0058: ret
    ...
```

Because the call to `baz` happens in a tail recursive way, the compiler can use the `tail.` prefix for it in `IL_0051`. Compare this to the call to `bar` in  `IL_0033`.

## Let the compiler understand your intent

So, all we have to do to keep using elegant recursive functions is to define them in the tail recursive style, right?

Well, easier said than done. Making sure a function is defined in a tail recursive way can be a challenge as the complexity of the function increases and various programmers work on the code. So it would be desirable that the compiler warns about a function which isn't tail recursive but should be according to the stated intend of the developer.

This is exactly what happened with [RFC-1011](https://github.com/fsharp/fslang-design/blob/main/FSharp-8.0/FS-1011-warn-on-recursive-without-tail-call.md). A new attribute, `[<TailCall>]`, was implemented in the F# compiler. Developers can use it to make their intent clear - that a function should be tail recursive. Functions annotated with this attribute are checked if they are really tail recursive and if not, a warning is emitted. 

The analysis for the warning is made after the optimization phase of the compiler, so any rewrite of a recursive function has already been applied. Otherwise, a false warning could be emitted about a function that the compiler is able to rewrite into a loop, for example. During the analysis the typed abstract syntax tree (TAST) is traversed and recursive calls to functions with the new attribute are checked if they happen in a tail recursive manner. An example of a characteristic that would render a function call non-tail recursive would be the first position in a `Sequential` expression.

Applying this new attribute to `countDown1` would look like this:
```fsharp
[<TailCall>]
let rec countDown1 n =
    if n = 0
    then 0
    else
        countDown1 (n - 1) + 1
```
With F# 8, the compiler warns about the non-tail recursive definition of the function:
```txt
Program.fs(6,9): warning FS3569: The member or function 'countDown1' has the 'TailCallAttribute' attribute, but is not being used in a tail recursive way. [tailrec_blog.fsproj]
```

With this feature of the compiler, developers should be able to concentrate more on the domain of their work and
worry less about technical details of the compiled code.

## Acknowledgements

So far, implementing this warning was my biggest contribution to the F# compiler. I had to try different approaches until the [PR](https://github.com/dotnet/fsharp/pull/15503) was approved. A few bug fixes followed which didn't make the cut for .NET 8, but should be in the first patch release.

I want to thank everyone who helped me along the way and I want to thank [Avi Avni](https://github.com/AviAvni) for opening the first [PR](https://github.com/dotnet/fsharp/pull/1976) of this feature many years ago.