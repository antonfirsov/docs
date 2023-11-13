---
post_title: Announcing F# 8
author1: tomasgrosup
post_slug: announcing-fsharp-8
microsoft_alias: tomasgrosup
featured_image: announcingfsharp8.jpg
categories: .NET, F#
tags: F#, .net 8
summary: Read what is new in F# 8 - the language, compiler tooling and FSharp.Core standard library
post_date: 2023-11-14 10:05:00
---

F# 8 is released as part of .NET 8. It is included with new updates of Visual Studio 2022 and .NET 8 SDK.

 - [Download the latest version of .NET 8](https://dotnet.microsoft.com/download/dotnet/8.0)
 - [Install Visual Studio 2022 from](https://visualstudio.microsoft.com/vs/preview/)

F# 8 brings in many features to make F# programs simpler, more uniform and more performant.
Read more about language changes, new diagnostics, quality of life improvements, performance boosts for project compilation and upgrades given to the FSharp.Core standard library.

This announcement lists the major changes brought in by F# 8 developed at [F#'s open source code repository](https://github.com/dotnet/fsharp).

Do you want to learn more about .NET 8 in general? Join us and many other speakers at [.NET Conf 2023](https://dotnetconf.net/), an online event scheduled for November 14th-16th.

Are you new to F#? Start your journey at [the .NET guide to F# with learning materials, examples and YouTube videos](https://dotnet.microsoft.com/languages/fsharp).

Do you want to see how others are using F#? See recordings of [this year's fsharpConf](http://fsharpconf.com/), the F# Community Virtual Conference. For more advanced topics as well as recordings of live contributions to F# compiler and the F# library ecosystem, see the [sessions of Amplifying F#](https://amplifying-fsharp.github.io/), a community initiative to grow F#.

If you want to stay up-to-date with F#, follow @fsharponline on social media - [LinkedIn](https://www.linkedin.com/company/fsharponline/), [X](https://twitter.com/fsharponline) and [Hachyderm](https://hachyderm.io/@fsharponline).



## F# language changes

This section describes updates to the language itself, changes you will notice when writing or reading F# code the most.
Most of the code samples in this blog post are duplicated in the [FSharp 8 News repository](https://github.com/T-Gro/FSharp8_news) as a F# project. If you installed the latest .NET 8 tooling, you can check it out and experiment with the code already.

Do you have any interesting use cases for the new features you want to make public for others?
Let me know in [that repository](https://github.com/T-Gro/FSharp8_news) via a new issue or open a direct pull request straight away!

### _.Property shorthand for (fun x -> x.Property)

The first feature I want to introduce is a shorthand for defining simple lambda functions - useful for situations, when a lambda only does an atomic expression on the lambda argument. An atomic expression is an expression which has no whitespace unless enclosed in method call parentheses.

Let's have a look at a practical example before and after this feature.

**Before:**
<pre><code class="language-fsharp">type Person = {Name : string; Age : int}
let people = [ {Name = "Joe"; Age = 20} ; {Name = "Will"; Age = 30} ; {Name = "Joe"; Age = 51}]

let beforeThisFeature = 
    people 
    |> List.distinctBy (fun x -> x.Name)
    |> List.groupBy (fun x -> x.Age)
    |> List.map (fun (x,y) -> y)
    |> List.map (fun x -> x.Head.Name)
    |> List.sortBy (fun x -> x.ToString())
</code></pre>

**After:** 
<pre><code class="language-fsharp">type Person = {Name : string; Age : int}
let people = [ {Name = "Joe"; Age = 20} ; {Name = "Will"; Age = 30} ; {Name = "Joe"; Age = 51}]

let possibleNow = 
    people 
    |> List.distinctBy _.Name
    |> List.groupBy _.Age
    |> List.map snd
    |> List.map _.Head.Name
    |> List.sortBy _.ToString()
</code></pre>

As you can see, the snippet `(fun x -> x.)` got replaced with just `_.`, and the need for parantheses was eliminated.
This can come especially handy in a sequence of `|>` pipelined calls, as typically used for F#'s combinators in List, Option and many other modules.
The feature works for single property access, nested property access, method calls and even indexers. The example demonstrates this feature on a list of records, but it works on all values that you can use in a regular lambda function, too. So for example also covering objects, primitives, anonymous records or discriminated unions:
<pre><code class="language-fsharp">let getIdx5 : {| Foo : int array |} -> int = _.Foo[5]
</code></pre>


**Also:**

What's more, this feature can also be used outside of a function call to define a standalone lambda to future usage.
In the second example `getNameLength`, you can also see that the feature works without the need to annotate the definition.
<pre><code class="language-fsharp">let ageAccessor : Person -> int = _.Age
let getNameLength = _.Name.Length
</code></pre>


The same syntax can be used to define accessors via [SRTP syntax](https://learn.microsoft.com/dotnet/fsharp/language-reference/generics/statically-resolved-type-parameters), allowing the same binding to be used by all items having the same member, without the need to share a common interface.
<pre><code class="language-fsharp">let inline myPropertyGetter<'a when 'a:(member WhatANiceProperty: int)> (x: 'a) = x |> _.WhatANiceProperty
</code></pre>



There is a situation where this syntax is not suitable: when the surrounding scope already makes use of the `_` underscore symbol, typically to discard a parameter.
<pre><code class="language-fsharp">let a : string -> string = (fun _ -> 5 |> _.ToString())
</code></pre>
Such code will end up producing a warning FS3570, saying `"The meaning of _ is ambiguous here. It cannot be used for a discarded variable and a function shorthand in the same scope."`

### Nested record field copy and update

Next new feature is a [copy-and-update](https://learn.microsoft.com/dotnet/fsharp/language-reference/copy-and-update-record-expressions) enhancement for nested records.
Let's again demonstrate the feature with before and after examples.

**Before:**
<pre><code class="language-fsharp">type SteeringWheel = { Type: string }
type CarInterior = { Steering: SteeringWheel; Seats: int }
type Car = { Interior: CarInterior; ExteriorColor: string option }

let beforeThisFeature x = 
    { x with Interior = { x.Interior with 
                            Steering = {x.Interior.Steering with Type = "yoke"}
                            Seats = 5
                        }
    }
</code></pre>

**After:**
<pre><code class="language-fsharp">let withTheFeature x = { x with Interior.Steering.Type = "yoke"; Interior.Seats = 5 }
</code></pre>

The two blocks make the identical change. Instead of having to write multiple nested `with` keywords, the new language feature allows you to use the dot-notation to reach to lower levels of nested records and update those.
As seen in the example, the syntax still allows copy-and-updating multiple fields using the same expression. Each copy-and-update within the same expression can be on a different level of nesting (see `Interior.Steering.Type` and `Interior.Seats` being used in the same snippet above).

**Also works for anonymous records:**
The same syntax extension can be used on anonymous records, or when updating regular records into anonymous ones.
Using the same type definitions from the example above, we can update the field `Interior.Seats ` using the new feature as well as as add a brand new field `Price` within the same expression.
<pre><code class="language-fsharp">let alsoWorksForAnonymous (x:Car) = {| x with Interior.Seats = 7; Price = 99_999 |}
</code></pre>

**Beware of name clashes between types and record fields:**

When trying out this feature, you [might get into a conflict](https://github.com/dotnet/fsharp/issues/16180) when naming a field the same as an existing type.
Before F# 8, it was possible to qualify record updates using the `Type.Field` notation. For backwards compatibility, this behavior still works and has a higher priority over a new language feature.


<pre><code class="language-fsharp">type Author = {
    Name: string
    YearBorn: int
}
type Book = {
    Title: string
    Year: int
    Author: Author
}

let oneBook = { Title = "Book1"; Year = 2000; Author = { Name = "Author1"; YearBorn = 1950 } }
let codeWhichWorks = {oneBook with Book.Author.Name = "Author1Updated"}
let codeWhichLeadsToAnError = {oneBook with Author.Name = "Author1Updated"}
</code></pre>

The last example, since it has to prefer to pre-existing `Type.Field` notation for the existing `Author` type, leads to an error:

```This expression was expected to have type 'Book' but here has type 'Author'```

**Workaround:**
When this happens, it is sufficient to qualify the update with the right type, as in the `codeWhichWorks` sample: `Book.Author.Name` works fine here.


### while!

The `while!` (while bang) feature was announced when as part of an earlier preview. You can read in [blog post introducing while!](https://devblogs.microsoft.com/dotnet/simplifying-fsharp-computations-with-the-new-while-keyword/).

What does `while!` do?
It simplifies the usage of computation expressions when looping over a boolean condition that has to be evaluated by the computation expression first (e.g., inside an `async{}` block).

**Before this feature:**
<pre><code class="language-fsharp">let mutable count = 0
let asyncCondition = async {
    return count < 10
}

let doStuffBeforeThisFeature = 
    async {
       let! firstRead = asyncCondition
       let mutable read = firstRead
       while read do
         count <- count + 2
         let! nextRead = asyncCondition
         read <- nextRead
       return count
    }
</code></pre>

**With `while!`:**
<pre><code class="language-fsharp">let doStuffWithWhileBang =
    async {
        while! asyncCondition do
            count <- count + 2
        return count
    }
</code></pre>

The two code blocks are equivalent in their behavior.

The addition of `while!` means a reduction of boilerplate code needed to maintain the `mutable read` boolean variable which was being looped over. The regular `while` (no bang) can only loop over a bool value, and not over an `async<bool>` (or similar wrapper in a different computation expression). `while!` brings this possibility to the language.


The [separate blog post](https://devblogs.microsoft.com/dotnet/simplifying-fsharp-computations-with-the-new-while-keyword/) uses the `<LangVersion>preview</LangVersion>` project setting to enable it. That is no longer needed with .NET 8 being released. As a [reminder](https://learn.microsoft.com/dotnet/fsharp/whats-new/fsharp-47#language-version), newly developed language and compiler features can be tested using the `preview` value before the final version (in this case, 8) is released.

### Extended string interpolation syntax  
F# 8 took inspiration from [interpolated raw string literals in C#](https://learn.microsoft.com/dotnet/csharp/language-reference/tokens/interpolated#interpolated-raw-string-literals) and improved the support for existing [interpolated strings in F#](https://learn.microsoft.com/dotnet/fsharp/language-reference/interpolated-strings).

In interpolated strings, literal text output can be combined with values and expressions by wrapping them into a pair of braces `{}`.
Braces therefore are special symbols, and need to be escaped by doubling them (via `{{` and `}}` ) if they are meant to be a literal part of the output.

This gets in the way if the text naturally contains a lot of braces, e.g. in the context of embedding other languages in F# strings - JSON, CSS or HTML-based templating languages such as [mustache](https://mustache.github.io/).

**Embedding CSS in F# strings before:**
Notice how each brace for the literal output had to be doubled.
<pre><code class="language-fsharp">let classAttr = "item-panel"
let cssOld = $""".{classAttr}:hover {{background-color: #eee;}}"""
</code></pre>

The extended interpolation syntax introduces the ability to redefine that behaviour by entering multiple `$` dollar signs at the beginning of the interpolated string literal. The number of starting dollars then dictates the number of braces needed to enter interpolation mode. Any smaller number of braces just becomes part of the output, without any need to escape them.

**With new feature:**
<pre><code class="language-fsharp">let cssNew = $$""".{{classAttr}}:hover {background-color: #eee;}"""
</code></pre>

**HTML templating:**
In HTML-based templating languages, doubled braces are commonly used to render variables.
In the example below, you can see how F# strings compare with and without extended interpolation syntax.
<pre><code class="language-fsharp">let templateOld = $"""
&lt;div class="{classAttr}"&gt;
  &lt;p&gt;{{{{title}}}}&lt;/p&gt;
&lt;/div&gt;
"""
let templateNew = $$$"""
&lt;div class="{{{classAttr}}}"&gt;
  &lt;p&gt;{{title}}&lt;/p&gt;
&lt;/div&gt;
"""
</code></pre>

You can read more about the feature in the [blog post about new syntax for string interpolation in F# 8](https://devblogs.microsoft.com/dotnet/new-syntax-for-string-interpolation-in-fsharp/).


### Use and compose string literals for printf and related functions

String literals have received one more update in this release, for usages of built-in printing functions (printfn, sprintfn [and others](https://fsharpforfunandprofit.com/posts/printf/#the-printf-family-of-functions)).

**Before F# 8:**
<pre><code class="language-fsharp">let renderedCoordinatesOld = sprintf "(%f,%f)" 0.25 0.75
let renderedTextOld = sprintf "Person at coordinates(%f,%f)" 0.25 0.75
</code></pre>

Before F# 8, the format specified has to be a string literal typed directly at the place of usage.
With F# 8, string literals defined elsewhere are supported.
What's more, you can define a string literal using a concatenation of existing string literals.
That way, commonly repeating format specifiers can be reused as patterns, instead of repeating same string snippets throughout the codebase.

Why does it have to be a `[<Literal>]` string that is known at compile time? Since the compiler is doing typechecking on the format string in conjuction with other arguments, full content of it must be known at compile time and it cannot be a runtime value.

**String format reuse with F# 8:**
<pre><code class="language-fsharp">[&lt;Literal&gt;] 
let formatBody = "(%f,%f)"
[&lt;Literal&gt;] 
let formatPrefix = "Person at coordinates"
[&lt;Literal&gt;] 
let fullFormat = formatPrefix + formatBody

let renderedCoordinates = sprintf formatBody 0.25 0.75
let renderedText = sprintf fullFormat 0.25 0.75
</code></pre>




### Arithmetic operators in literals

Numeric literals have also received an update.
In the past, they had to be fully specified using constant values.

**Before:**
<pre><code class="language-fsharp">module ArithmeticLiteralsBefore =
    let [&lt;Literal&gt;] bytesInKB = 1024f
    let [&lt;Literal&gt;] bytesInMB = 1048576f
    let [&lt;Literal&gt;] bytesInGB = 1073741824
    let [&lt;Literal&gt;] customBitMask =  0b01010101uy
    let [&lt;Literal&gt;] inverseBitMask = 0b10101010uy
</code></pre>

With F# 8, numeric literals can also be expressed using existing operators and other literals.
The compiler evaluates the expression at compile time, and stores the resulting value in the produced assembly. You will notice this when you hover over a defined literal (or its usage) in Visual Studio - it will show you the calculated value.

- Supported for numeric types: `+,-,*, /, %, &&&, |||, <<<, >>>, ^^^, ~~~, **`
    - The operators have the same semantics as they have in non-literal expressions.
- Supported for bools: `not, &&, ||`

**Example using F# 8:**
<pre><code class="language-fsharp">
let [&lt;Literal&gt;] bytesInKB = 2f ** 10f
let [&lt;Literal&gt;] bytesInMB = bytesInKB * bytesInKB
let [&lt;Literal&gt;] bytesInGB = 1 <<< 30
let [&lt;Literal&gt;] customBitMask = 0b01010101uy
let [&lt;Literal&gt;] inverseBitMask = ~~~ customBitMask
</code></pre>

**Also works for enum values and literals:**

The feature can be used at places which require literal values, such as enum values or attribute parameters.

<pre><code class="language-fsharp">type MyEnum = 
    | A = (1 <<< 5)
    | B = (17 * 45 % 13)
    | C = bytesInGB

[&lt;System.Runtime.CompilerServices.MethodImplAttribute(enum(1+2+3))&gt;]
let doStuff = ()
</code></pre>

In the case of enum values, arithmetic expressions need to be wrapped in parentheses like in the example above.


### Type constraint intersection syntax

F# 8 brings in a new feature to simplify definition of multiple intersected generic constraints using [flexible types](https://learn.microsoft.com/dotnet/fsharp/language-reference/flexible-types).

**Code prior to F# 8:**
<pre><code class="language-fsharp">let beforeThis(arg1 : 't 
    when 't:>IDisposable 
    and 't:>IEx 
    and 't:>seq&lt;int&gt;) =
    arg1.h(arg1)
    arg1.Dispose()
    for x in arg1 do
        printfn "%i" x
</code></pre>


The definiton does require repeating the generic type argument `'t` for every clause and they have to be connected using the `and` keyword.
With F# 8, intersected constrains can be specified using the `&` characters and achieve the same result:

<pre><code class="language-fsharp">let withNewFeature (arg1: 't & #IEx & 
    #IDisposable & #seq<int>) =
    arg1.h(arg1)
    arg1.Dispose()
    for x in arg1 do
        printfn "%i" x
</code></pre>

The same syntax also works on specifying signatures, e.g. in signature files or for defining abstract functions:

<pre><code class="language-fsharp">type IEx =
    abstract h: #IDisposable & #seq<int> -> unit
</code></pre>

### Extended fixed bindings

[F# has the fixed keyword](https://learn.microsoft.com/dotnet/fsharp/language-reference/fixed), which can be used to pin memory and get its address as `nativeptr<int>`. It is used in low-level programming scenarios.

[F# 8 extends this feature](https://github.com/fsharp/fslang-design/blob/main/FSharp-8.0/FS-1081-extended-fixed-bindings.md) with additionally allowing it on:
- `byref<'t>`
- `inref<'t>`
- `outref<'t>`
- any type 'a when 'a has an instance/extension method `GetPinnableReference : unit -> byref<'t>`
- any type 'a when 'a has an instance/extension method `GetPinnableReference : unit -> inref<'t>`

The last two additions are especially relevant for the growing ecosystem and the types like [ReadOnlySpan](https://learn.microsoft.com/dotnet/api/system.readonlyspan-1.getpinnablereference?view=net-7.0) or [Span](https://learn.microsoft.com/dotnet/api/system.span-1.getpinnablereference?view=net-7.0).


**Possible now:**
<pre><code class="language-fsharp">open System
open FSharp.NativeInterop

#nowarn "9"
// "Warning no. 9 is a warning about using unsafe code with nativeint. We are disabling it here when we know we know what we are doing"
let pinIt (span: Span&lt;char&gt;, byRef: byref&lt;int&gt;, inRef: inref&lt;int&gt;) =
    // Calls span.GetPinnableReference()
    // The following lines wouldn't compile before
    use ptrSpan = fixed span
    use ptrByRef = fixed &byRef
    use ptrInref = fixed &inRef
    
    NativePtr.copyBlock ptrByRef ptrInref 1
</code></pre>


### Easier `[<Extension>]` method definition

The `[<Extension>]` attribute exists in order to define C#-style extension methods, which can be consumed both by F# and C#.
However, in order to satisfy the C# compiler, the attribute had to be applied both on the member as well as on the type:

**Before:**
<pre><code class="language-fsharp">open System.Runtime.CompilerServices
[&lt;Extension&gt;]
type Foo =
    [&lt;Extension&gt;]
    static member PlusOne (a:int) : int = a + 1
let f (b:int) = b.PlusOne()
</code></pre>

With F# 8, the compiler will only require the attribute on the extension method, and will automatically add the type-level attribute for you.

**After:**
<pre><code class="language-fsharp">open System.Runtime.CompilerServices
type Foo =
    [&lt;Extension&gt;]
    static member PlusOne (a:int) : int = a + 1
let f (b:int) = b.PlusOne()
</code></pre>


## Making F# more uniform

The following changes are making F# more consistent by allowing existing constructs at previously forbidden contexts.
This aims to reduce beginner confusion, reduce the need for workarounds and lead to a more succinct code.

### Static members in interfaces

This change allows to declare and implement static members in interfaces.
Not to confuse with [static abstract members in interfaces from F#7](https://devblogs.microsoft.com/dotnet/announcing-fsharp-7/#static-abstract-members-support-in-interfaces), the upcoming change is about concrete members in interfaces, with their implementation.

Today, the same would be typically accomplished by putting a module with implemented function(s) below the type.

**Before:**
<pre><code class="language-fsharp">[&lt;Interface&gt;]
type IDemoableOld =
    abstract member Show: string -> unit

module IDemoableOld =
    let autoFormat(a) = sprintf "%A" a
</code></pre>

With F# 8, interfaces can have concrete members on them and a standalone module is not needed.

**After:**
<pre><code class="language-fsharp">[&lt;Interface&gt;]
type IDemoable =
    abstract member Show: string -> unit
    static member AutoFormat(a) = sprintf "%A" a
</code></pre>


### Static let in discriminated unions, records, structs, and types without primary constructors

Second example of uniformity is enablement of static [bindings](https://learn.microsoft.com/dotnet/fsharp/language-reference/members/let-bindings-in-classes) in more F# types. We are talking about the following constructs:
- `static let`
- `static let mutable`
- `static do`
- `static member val`

Before F# 8, those were only possible in regular class definitions. With F# 8, they can be added to:
- Discriminated unions
- Records
- Structs
    - incl. `[<Struct>]` unions and records
- Types without primary constructors


This addition can again help with encapsulating data and logic within the type definition, as opposed to declaring a standalone `module` and putting the bindings in that module.



**Possible now:**
As an example, a simple lookup for converting from string to cases of a disriminated union can now be declared directly inside the type definiton.
<pre><code class="language-fsharp">open FSharp.Reflection

type AbcDU = A | B | C
    with   
        static let namesAndValues = 
            FSharpType.GetUnionCases(typeof<AbcDU>) 
            |> Array.map (fun c -> c.Name, FSharpValue.MakeUnion (c,[||]) :?> AbcDU)
        static let stringMap = namesAndValues |> dict
        static let mutable cnt = 0
        
        static do printfn "Init done! We have %i cases" stringMap.Count
        static member TryParse text = 
            let cnt = Interlocked.Increment(&cnt)
            stringMap.TryGetValue text, sprintf "Parsed %i" cnt
</code></pre>

The example also shows how a static mutable can be added, and how side effects can be triggered via [static do](https://learn.microsoft.com/dotnet/fsharp/language-reference/members/do-bindings-in-classes). This feature respects the declaration order within the file, as well as the declaration order of types and bindings within a single `module`.

**Also possible:**

The syntax also allows you to define [explicit static fields with automatic properties using the 'member val' keywords](https://learn.microsoft.com/dotnet/fsharp/language-reference/members/explicit-fields-the-val-keyword).

<pre><code class="language-fsharp">type AnotherDu = D | E
    with
        static member val X = 42 with get,set
</code></pre>

Active patterns can also be implemented in `static let` bindings. The active pattern is private to the type, but it can be used by following members within the same type definition.
<pre><code class="language-fsharp">type AB = 
    | A
    | B of int

    static let (|B0PatPrivate|_|) value = if value = 0 then Some (B 999) else None
    static member ParseUsingActivePattern x = match x with | B0PatPrivate x -> x | _ -> A

let testThis = AB.ParseUsingActivePattern 0
</code></pre>

**Good to know:**
This feature does not work for 'types' which are not real types at runtime. In particular type aliases and units of measure, which are being erased during compilation, and plain .NET enums.


As with other statics in generic types in .NET, all static values are created per generic instantiation and can make use of the generic parameters provided, e.g. via reflection.

The following example shows a `static let` in a generic type, in particular a type without primary constructor.
<pre><code class="language-fsharp">type EmptyT<'T> =
    static let cachedName = 
        let name = typeof<'T>.Name
        printfn "Accessing name for %s" name
        name
    static member Name = cachedName
</code></pre>

This might be useful to retrieve and cache information retrieved by reflection or by runtime-only features such as the `sizeof` instruction, per generic type instantiation.
<pre><code class="language-fsharp">[&lt;Struct&gt;]
type MyUnion<'A,'B> = 
    | A of aval:'A
    | B of bval:'B
    | C

    static let sizeOfTCached = 
        printfn "Creating cached val for %s * %s" (typeof<'A>.Name) (typeof<'B>.Name)
        sizeof<MyUnion<'A,'B>>
</code></pre>

### `try-with` within `seq{}`,`[]` and `[||]` collection expressions

Third addition in the 'uniformity' category is new support of the the try-with code construct within collection builders - `seq{}` for `IEnumerable` definitions, `[]` list builders, and `[||]` array builders.

With this change, exception handling in those expressions is possible.
The following combinations are possible:
- 'try' part can yield values, 'with' can just produce a side effect (e.g. logging)
- both 'try' and 'with' can yield values
- 'try' can be empty from the perspective of yielding values, and only 'with' produces data
   - This scenario is less likely to be meaningful in real code, but it is supported


In the examples below, exception throwing is simulated by dividing by 0, which produces a `DivideByZeroException` on .NET.

**Possible now:**
<pre><code class="language-fsharp">let sum =
    [ for x in [0;1] do       
            try          
                yield 1              
                yield (10/x)    
                yield 100  
            with _ ->
                yield 1000 ]
    |> List.sum
</code></pre>
When executed, the code yields a list `[1;1000;1;10;100]` which sums up to 1112.

**Also: recursive calls and `yield!`**

Recursive calls from within the exception handler are also supported. In the case of `seq{}`, the behavior still maintains the lazy semantics and only executes code one value at a time. The example below shows how the exception handler can "retry" by recursively `yield!`-ing the same calculation again.

Do note that exception handling (the `'with'` handler) has a non-trivial cost in .NET which will be noticable in case of repetitive exceptions being thrown and handled.


<pre><code class="language-fsharp">let rec f () = seq {
    try 
        yield 123    
        yield (456/0)
    with exn ->
        eprintfn "%s" exn.Message
        yield 789
        yield! f()
}

let first5 = 
    f() 
    |> Seq.take 5 
    |> Seq.toArray
</code></pre>

When executed, the code produces `[|123; 789; 123; 789; 123|]`.

## New diagnostics

A big part of new F# 8 development is dedicated to new and updated diagnostics.
Those are the errors, warnings and information messages which compiler reports when it encounters issues.
This section lists a few selected categories of diagnostics enhancements.
Between F# 7 and F# 8, 34 new diagnostic errors and messages have been added into the [F# definition of diagnostical messages](https://github.com/dotnet/fsharp/blame/main/src/Compiler/FSComp.txt).

### TailCall attribute

In F#, [recursive functions](https://learn.microsoft.com/dotnet/fsharp/language-reference/functions/recursive-functions-the-rec-keyword) are possible using the 'rec' keyword. An important aspect of recursive functions is the ability to do [tail recursion](https://learn.microsoft.com/dotnet/fsharp/language-reference/functions/recursive-functions-the-rec-keyword#tail-recursion) in order to prevent deep stacks and eventually StackOverflowException errors. 

You can also read [this Q&A](https://cs.stackexchange.com/questions/6230/what-is-tail-recursion) to learn what tail recursion means independent of the F# programming language.

Starting with F# 8.0, you can use the `TailCall` attribute to explicitly state your intention of defining a tail-recursive function to the compiler. The compiler will then warn you if your function makes non-tail recursive calls. You can use the attribute on methods and module-level functions.

The attribute is purely informational to the compiler and doesn't affect generated code, it ensures the compiler checks for tail call consistency and raises a warning if it is not met.


**Possible now:**
The first function, `factorialClassic` is the text book example for starting with functional programming and implementing the factorial function.
In this snippet, it is not tail recursive: When building the project, a `"warning FS3569: The member or function 'factorialClassic' has the 'TailCallAttribute' attribute, but is not being used in a tail recursive way."` is produced.

The second function, `factorialWithAcc`, uses the accummulator technique to implement the calculation in a tail-recursive way. Therefore, when the attribute is applied, no warning is being produced.

<pre><code class="language-fsharp">
[&lt;TailCall&gt;]
let rec factorialClassic n =
    match n with
    | 0u | 1u -> 1u
    | _ -> n * (factorialClassic (n - 1u))
// This produces a warning

[&lt;TailCall&gt;]
let rec factorialWithAcc n accumulator = 
    match n with
    | 0u | 1u -> accumulator
    | _ -> factorialWithAcc (n - 1u) (n * accumulator)
// This is a tail call and does NOT produce a warning
</code></pre>


### Diagnostics on static classes

F# does not have a dedicated set of keywords for creating a `static class`.
However, types that are sealed cannot be inherited, and types which are abstract cannot be instantiated.
This in practice means that only static members can be accessed on such type.

In order to eliminate runtime errors and dead code, a suite of new warnings for detecting invalid scenarios has been created.
The following diagnostics have been added as warnings enabled for F# 8:

**New warnings emitted:**

If a type uses both [&lt;Sealed&gt;] and [&lt;AbstractClass&gt;] attributes, it means it is static. This means:
- Instance let bindings are not allowed.
- Implementing interfaces is not allowed.
- Explicit field declarations are not allowed.
- Constructor with arguments is not allowed.
- Additional constructor is not allowed.
- Abstract member declarations are not allowed.


### Diagnostics on `[<Obsolete>]` usage

The `[<Obsolete>]` attribute can be used to mark types and members not recommended for usage.
The compiler should then emit a warning with a user-defined message (the message is taken from the attribute's content) when a usage is being detected.

The diagnostics around obsolete members have received the following support with F# 8:
- Detection on enum value usage
- Detection on an event
- Detection on record copy-and-update syntax (`with`) only when the obsolete field is part of the update


**Example of code which now produces a warning**
<pre><code class="language-fsharp">open System
type Color =
    | [&lt;Obsolete("Use B instead")&gt;] Red = 0
    | Green = 1
    
let c = Color.Red // warning "This construct is deprecated. Use B instead" at this line
</code></pre>


### Optional warning when `obj` is inferred

F# has a strong [type inference](https://learn.microsoft.com/dotnet/fsharp/language-reference/type-inference) on both parameter and return types.
For consumption of certain APIs, it can happen that [automatic generalization](https://learn.microsoft.com/dotnet/fsharp/language-reference/type-inference#automatic-generalization) fails to infer a value as a generic, and type is instead infered as `obj`.

Most of the times (not always), this is an indication of an unintended action.
F# 8 brings in a new optional information-level diagnostics with the number FS3559 and text:
```"A type has been implicitly inferred as 'obj', which may be unintended. Consider adding explicit type annotations. You can disable this warning by using '#nowarn \"3559\"' or '--nowarn:3559'."```

Example of code that can trigger it:
<pre><code class="language-fsharp">```([] = [])```
</code></pre>

This warning is off by default, meaning it has to be explicitely enabled using `<WarnOn>FS3559</WarnOn>` in your .fsproj project file.

### Optional warning when copy and update changes all fields
F# records offer a handy syntax to create a copy with selected fields modified, using the `with` keyword.
The project can be configured (via `<WarnOn>FS3560</WarnOn>`) to detect a situation in which copying syntax happens to change all fields of a record.
In such cases, it will be shorter and more efficient to create a new record from scratch, and populate all the fields directly.
The compiler will then report a warning saying:
```"This copy-and-update record expression changes all fields of record type '..name of your type..'. Consider using the record construction syntax instead."```

## Quality of life improvements

F# 8 also brings in many quality of life improvements, not related to new language features nor new diagnostics.
Here are a few selected ones:

### Trimmability for compiler-generated code
.NET platform supports [trimming](https://learn.microsoft.com/dotnet/core/deploying/trimming/trim-self-contained) since .NET 6. 3 improvements have been done to better support for trimming F#-compiler-generated code:
- Discriminated unions are now trimmable.
- Anonymous records are now trimmable.
- Code using `printfn "%A"` for trimmed records is now trimmable.

### Parser recovery

Parser recovery  comes to play when the F# parser encounters an invalid code construct, but should still attempt to continue parsing the rest of the file at a best-attempt basis.

This ensures that IDE features like coloring and navigation keep working even for code with syntactical errors or code which is being typed and is not finished yet. Mistakes like missing equals signs, unfinished declarations or incorrect indentation have received vastly improved parser recovery; bringing a better typing experience to F# users.

It very likely is the area of most improvements in F# 8 considering the number of pull requests targetting it.

[Watch this video session](https://amplifying-fsharp.github.io/sessions/2023/07/07/) to learn more about parser recovery in depth.

### Strict indentation rules

As part of work needed for improved parser recovery, F# 8 turns on a strict indentation mode. This mode respects the rules of the language for indentation and reports an error in invalid scenarios where the previous language versions reported a warning only.

Projects targetting F# language version 7 or lower keep the current behavior, projects for F# 8 and newer have the strict indentation rules turned on automatically.

The default selection can be configured using a compiler switch:

```--strict-indentation[+|-] Override indentation rules implied by the language version```

This can be also turned off in the project file, by specifying:

````<OtherFlags>--strict-indentation-</..>```` to turn this feature off or

````<OtherFlags>--strict-indentation+</..>```` to turn it on.


### Autocomplete improvements

The F# compiler provides a library which brings in autocomplete logic for F# code to your favourite editors, such as Visual Studio, Visual Studio Code or Rider.

The improvements cover both recall and precision - suggesting names which are needed in the given context, and not offering the entities which cannot be used there.

The following completion scenarios have been improved:
- Record completions in patterns
- Union fields in patterns
- Return type annotations
- Method completions for overrides
- Constant value completions in pattern matching (e.g. matching against constant in System.Double type)
- Expressions in enum values
- Suggesting names based on labels of union-case fields if they are defined
- Completions for collections of anonymous records
- Settable properties in attribute completions

### `[<Struct>]` unions can now have > 49 cases

Many issues have been resolved as part of F# 8 and they will not all be mentioned in this blog post. One I'd like to mention was a limiting factor for F# codebases:

Before F# 8, declarations of struct unions with more than 49 cases caused an unexpected runtime error. As a workaround, they typically had to be converted to regular class-based unions.

With F# 8, this limitation is removed and struct unions do not come with a size limitation - making them suitable for longer definitions of union cases. Examples might be unions of countries or phone codes.

## Compiler performance

Compiler performance is a big topic for the F# compiler and related tooling. The compiler is what powers independent builds, language features in IDEs as well as popular libraries built on top of it (e.g. [Fantomas](https://fsprojects.github.io/fantomas/docs/index.html), the F# code formatter).

Two areas have received special attention in this release of F# - incremental builds of large graphs of projects via the `Reference assemblies` feature, and CPU-parallelization of the compiler process.

### Reference assemblies

[Reference assemblies](https://learn.microsoft.com/dotnet/standard/assembly/reference-assemblies) are special assemblies in .NET which can be used in design-time and build-time scenarios.

They work by keeping the public shape of an API (surface area), but stripping out implementation details and replacing them with a `"throw null"` instruction. That way, they get smaller and, more importantly, more robust to change. If the implementation details in the real project change, chances are that the reference assembly remains the same as long as APIs have not changed.

This is an existing feature which was already working for F#, but not utilized to its potential due to F#'s compiler-generated resources. F# compiler enriches produces assemblies with embedded resources, binary data representing the F# signature information and F# optimizer for cross-assembly F# support and cross-assembly function inlining.

For the purpose of reference assemblies, optimization data has been reduced  to `"let inline"` definitions only  in DEBUG builds, and F# signature data has been replaced with a custom hash function covering the public surface of F# types, modules and namespaces.

With those two changes, changes of implementation details in a low-level F# project will not require a full rebuild of all the projects transitively depending on it. This can be a big factor especially in a large solutions with interconnected projects.

When using Visual Studio, the benefit of reference assemblies can go even further by avoiding the need to call into `msbuild.exe` alltogether, by replicating up-to-date checks in VisualStudio. This [blog about AccelerateBuildsInVisualStudio](https://devblogs.microsoft.com/visualstudio/vs-toolbox-accelerate-your-builds-of-sdk-style-net-projects/) explains the feature in bigger details.

If you want to give it a try, add the following to your .fsproj project properties:

- `<AccelerateBuildsInVisualStudio>true</..>`

### Switches for compiler parallelization

Second area of improvements I want to mention is compiler parallelization.
Historically, the F# compiler has been single-threaded. In recent F# versions, parallelization has been enabled for parsing and for typechecking implementation files backed by signature files.

F# 8 brings in 3 new experimental features that have been added to cover 3 other stages of the F# compilation process.
At the moment, they are not enabled by default and are only configurable via dedicated `--test:` flags to the command line compiler. They can be also passed to the compiler based on project file, using the following property syntax for [F# compiler options](https://learn.microsoft.com/dotnet/fsharp/language-reference/compiler-options):

- `<OtherFlags>--test:..</..>`

Graph-based typechecking has the potential of speeding up the build process the most. It works by deriving dependencies of F# files from the untyped syntax tree before the expensive typechecking phase, and then controlling which files can be typechecked in parallel by following the connected graph of files.

This feature has been demonstrated in an [Amplifying F#](https://amplifying-fsharp.github.io/) session on [Graph-based type-checking](https://amplifying-fsharp.github.io/sessions/2023/04/28/) and is described in [this blog post](https://devblogs.microsoft.com/dotnet/a-new-fsharp-compiler-feature-graphbased-typechecking/).

This can be turned on by the flag:
- --test:GraphBasedChecking

Next phase of the compilation process are the optimizations applied to F# code. The technical description of the chosen approach is well described in the [pull request implementing the feature](https://github.com/dotnet/fsharp/pull/14390).

Timings for `FSharp.Compiler.Service` showing the difference with (Parallel) or without (Sequential) this feature:
Test run on a 8-core/16-thread CPU.
It's worth noting that no more than 7 items are processed at a time.

| Optimize | Mode | Optimization Time | Total Time |
| -------- | ------ | ----- | ----- |
| + | Sequential | 12.9s | 31.0s |
| + | Parallel | 7.1s (-45%) | 25.5s (-18%) |
| - | Sequential | 5.6s | 24.3s |
| - | Parallel | 3.7s (-34%) | 23.6s (-3%) |

The feature can be enabled using the following flag:
- --test:ParallelOptimization



Last phase of the F# build process is the code generation of IL instructions and creation of an assembly.
In this phase, the parallelization happens by converting methods bodies into .NET IL in parallel. 
For the main test project in the F# repository, this change lead to an improvement from 0.6s to 0.4s to the IL conversion, therefore a -33% speed improvement.

The feature can be enabled using the following flag:
- --test:ParallelIlxGen



Alternatively, all of the features can be enabled  globally using an `FSHARP_EXPERIMENTAL_FEATURES` environment variable, for example like this in PowerShell: `$env:FSHARP_EXPERIMENTAL_FEATURES = '1`.


## Enhancements to the FSharp.Core standard library

### Inlining

F# already has a powerful feature called inlining.
Instead of calling a function directly, the compiler can decide to put the body of the called function (=inline it)  into the call site. That way, not only is a function call being eliminated, but the compiler can also optimize further based on type arguments and function arguments available at the call site. For example, a regular generic function for equality would have to go via standard interfaces and contracts (e.g., the `.Equals()` method). When inlining the generic usage in a call site that uses it on integers, it can instead replace it with a single instruction that compares two integers.

Furthermore, F# also has the `[<InlineIfLambda>]` attribute which allows to replace a lambda function with a direct call when inlining happens. On top of all the previous advantages, this also means a saved allocation for the closure of the lambda.


F# 8 comes with inlining changes to two standard modules in FSharp.Core:
- [Inlining](https://github.com/dotnet/fsharp/pull/14927) for functions in the `Option` module
- [Inlining](https://github.com/dotnet/fsharp/pull/15709) for functions in the `ValueOption` module


Lambda allocations have been removed, which in the case of `ValueOption` functions means zero allocations alltogether.
As an example of the reduced overhead, mapping of the `None` value now takes 0.17ns instead of 2.77ns, which is a 16x improvement in terms of time.


Another example is an equality-inlining issue which has been spotted at [this issue about List.contains performance](https://github.com/dotnet/fsharp/issues/15720).
The reason was a stopped inlining because of recursive scope - when a function is recursive, it cannot be inlined (inlining effectively means embedding the code of the inlined function at the call site - which for a recursive function would mean including it infinitely many times).
Once the equality check was moved outside of the recursive scope, equality check can be inlined and properly special-cased.

[This is the minimal code change](https://github.com/dotnet/fsharp/pull/15726/files#diff-3564806fd5c409bdb92242766794a120c6897330a21c0b12ce2138c2e6aa44ecR462) which improved it, leading up to an 16x improvement in certain scenarios.

|                                 Method |       Mean |     Error |    StdDev |     Median |      Gen0 |  Allocated |
|--------------------------------------- |-----------:|----------:|----------:|-----------:|----------:|-----------:|
|                  &#39;int - List.containsOld&#39; | 9,284.4 μs | 158.63 μs | 148.38 μs | 9,266.7 μs | 1906.2500 | 24024049 B |
|       &#39;int - List.containsNew&#39; |   548.4 μs |  10.35 μs |  15.17 μs |   541.9 μs |         - |       41 B |
|               &#39;string - List.containsOld&#39; | 2,393.9 μs |  43.59 μs |  83.98 μs | 2,359.5 μs |         - |       42 B |
|    &#39;string - List.containsNew&#39; |   838.6 μs |  21.62 μs |  62.38 μs |   824.1 μs |         - |       41 B |
|               &#39;record - List.containsOld&#39; | 3,796.8 μs |  66.80 μs | 113.44 μs | 3,753.0 μs |         - |       45 B |
|    &#39;record - List.containsNew&#39; |   691.4 μs |  13.61 μs |  16.20 μs |   688.9 μs |         - |       41 B |

### Improvements

### `Array.Parallel.*` APIs

Fsharp.Core offers many standard functions for major collection types - arrays, lists and sequences.
Array contains a nested module, Array.Parallel, intended for CPU-intensive tasks over large arrays. Before F# 8, that module was not on par with the non-parallel functions, and was missing many basic functions. With this release, many new APIs have been added to support multi-threaded programming.

Each of the functions will try to consume all the logical processors available to it (this can be fine-tuned via the `DOTNET_PROCESSOR_COUNT` environment variable if the default is not desired).
They are especially useful in CPU-intensive processing. That is, either when the arrays are very large, when the function parameter passed in is itself computationally expensive, or both.
They do carry the overhead of creating new tasks and coordinating them, therefore can be slower then their non-parallel counterparts for simple inputs.

 - exists
 - forAll
 - tryFindIndex
 - tryFind
 - tryPick
 - reduceBy
 - reduce
 - minBy
 - min
 - sumBy
 - sum
 - maxBy
 - max
 - averageBy
 - average
 - zip
 - groupBy
 - filter
 - sortInPlaceWith
 - sortInPlaceBy
 - sortInPlace
 - sortWith
 - sortBy
 - sort
 - sortByDescending
 - sortDescending

 #### Selected benchmarks

 To demonstrate the difference between `Array`, `Array.Parallel` and `PLINQ`, several benchmarks are included in this blog post.
 All of these were running for an array of **500.000 randomly generated structs** using the following definition of the struct, and an artificial function representing *"complex CPU-intensive business logic"*.


**Possible now:**
<pre><code class="language-fsharp">[&lt;Struct&gt;]
type SampleRecord = {Age : int; Balance : int64; Molecules : float; IsMiddle : bool}

let complexLogic (sr:SampleRecord) = 
    let mutable total = float sr.Balance
    total <- total + sin sr.Molecules
    total <- atan total
    for a=0 to sr.Age do
        total <- total + cos (float a)
    total <- total + float (hash sr)
    total
 </code></pre>

This was executed on a 11th Gen Intel Core i9-11950H 2.60GHz machine, with 16 logical and 8 physical cores.
The comparisons were done across three different approaches:
 - Array module from FSharp.Core
 - PLINQ (Parallel Enumerable via `AsParallel()`), a stable and robust implementation from .NET
 - Newly added functions to the `Array.Parallel` module

 The major outcome for the comparison is the classical *"it depends"* consulting advice.
 500.000 elements can still be processed sequentially very fast (notice we are in the `ms` time range at most) and in cases of trivial calculation (e.g. just a property/field accessor, and not an expensive lambda function), the non-parallelized functions from the `Array` module perform the best. See for example the 'GroupBy - field only', 'Sort - by int field' and 'SumBy(plain field access)' categories in the results.

 The parallel versions start to get an advantage when the calculation to be invoked on every element gets more complex, like the `complexLogic` function above. In those cases, the `Array.Parallel` module brings both faster speed compared to the classical version, as well as better allocation footprint compared to PLINQ.

 A very significant performance improvement occurs at the `MinBy` calculation, where the `Array.Parallel.minBy` version is 68% faster than `Array.minBy` and 70% faster than `AsParallel().MinBy(..)` from PLINQ. MinBy is an example of an aggregation function, just like max, sum or average. They are all built on a lower-level `reduce` function utilizing a map-reduce routine, and very similar performance is expected across them all.


| Method                | Categories                    | Mean         | Ratio    | Allocated | Alloc Ratio |
|---------------------- |------------------------------ |-------------:|---------:|----------:|------------:|
| ArrayGroupBy2         | GroupBy - calculation         | 169,024.8 us | baseline |  70.17 MB |             |
| PlinqGroupBy2         | GroupBy - calculation         |  74,683.8 us |     -56% | 103.93 MB |        +48% |
| ArrayParallelGroupBy2 | GroupBy - calculation         |  62,574.3 us |     -63% |  70.61 MB |         +1% |
|                       |                               |              |          |           |             |
| ArrayGroupBy          | GroupBy - field only          |  14,274.3 us | baseline |  57.28 MB |             |
| PlinqGroupBy          | GroupBy - field only          |  30,933.6 us |    +117% |  88.77 MB |        +55% |
| ArrayParallelGroupBy  | GroupBy - field only          |  18,318.6 us |     +29% |  47.72 MB |        -17% |
|                       |                               |              |          |           |             |
| ArrayMinBy            | MinBy(calculationFunction)    | 157,463.5 us | baseline |  11.44 MB |             |
| PlinqMinBy            | MinBy(calculationFunction)    | 160,243.5 us |      +2% |  11.44 MB |         +0% |
| ArrayParallelMinBy    | MinBy(calculationFunction)    |  48,768.7 us |     -68% |  11.45 MB |         +0% |
|                       |                               |              |          |           |             |
| ArraySort             | Sort - by int field           |  27,352.1 us | baseline |  17.17 MB |             |
| PlinqSort             | Sort - by int field           |  38,723.7 us |     +42% | 172.89 MB |       +907% |
| ArrayParallelSort     | Sort - by int field           |  76,744.8 us |    +179% | 112.76 MB |       +557% |
|                       |                               |              |          |           |             |
| ArraySortBy           | SortBy - calculation          | 214,042.4 us | baseline |  30.52 MB |             |
| PlinqSortBy           | SortBy - calculation          |  97,214.3 us |     -55% | 193.99 MB |       +536% |
| ArrayParallelSortBy   | SortBy - calculation          | 125,951.7 us |     -41% | 130.49 MB |       +328% |
|                       |                               |              |          |           |             |
| ArraySumBy            | SumBy(plain field access)     |     466.7 us | baseline |         - |          NA |
| PlinqSumBy            | SumBy(plain field access)     |     984.1 us |    +112% |   0.01 MB |          NA |
| ArrayParallelSumBy    | SumBy(plain field access)     |     687.6 us |     +47% |   0.01 MB |          NA |
|                       |                               |              |          |           |             |
| ArrayTryFind          | TryFind - calculationFunction |  76,509.7 us | baseline |   5.72 MB |             |
| PlinqTryFind          | TryFind - calculationFunction |  41,256.7 us |     -47% |  10.74 MB |        +88% |
| ArrayParallelTryFind  | TryFind - calculationFunction |  23,094.4 us |     -69% |   5.73 MB |         +0% |


### `async` improvements
- `Bind` of `Async<>` within `task{}` [now starts on the same thread](https://github.com/dotnet/fsharp/pull/14499)
    * This saves resources by keeping the computation on the same .NET thread and not starting a new one
- `MailBoxProcessor` [now comes](https://github.com/dotnet/fsharp/pull/14929) with a public `.Dispose()` member
    * Better discoverability that `MailboxProcessor` implements `IDisposable` and thus must be disposed
    * Removes need to manually cast to `IDisposable`, as in `(mailboxProcessor :> IDisposable).Dispose()` before disposing
- `MailBoxProcessor` [now comes](https://github.com/dotnet/fsharp/pull/14931) with `StartImmediate`
    * Existing `Start` method starts the execution on the thread pool. The new `StartImmediate` ensures starting on the same thread as the calling one.
    * This is useful for situations in which you want to force the MailboxProcessor to start on a given thread or even run on a single thread throughout its lifetime. For example, a `MailboxProcessor` may be desired to wrap some unmanaged state, such as a window or some communication session that is not thread safe, and this is not currently possible with `MailboxProcessor.Start`.
## Thanks and acknowledgements

F# is developed as a collaboration between the .NET Foundation, the F# Software Foundation, their members and other contributors including Microsoft. The F# community is involved at all stages of innovation, design, implementation and delivery and we’re proud to be a contributing part of this community.

Between October 2022 and October 2023, the [dotnet/fsharp](https://github.com/dotnet/fsharp/graphs/contributors?from=2022-10-01&to=2023-10-01&type=c) repository tracks 33 contributors with commits in that period. On top of that, there are numerous contributors filing [issues](https://github.com/dotnet/fsharp), [raising and voting on suggestions](https://github.com/fsharp/fslang-suggestions) and contributing to [feature design](https://github.com/fsharp/fslang-design). Thank you all!



Below is a wall created from the profile pictures listed at GitHub's [automated statistics](https://github.com/dotnet/fsharp/graphs/contributors?from=2022-10-01&to=2023-10-01&type=c) for the [dotnet/fsharp](https://github.com/dotnet/fsharp) repository over the F# 8 development period. Visit the link and see all the contributors including the changes they have brought to F#. Order of the profile pictures here is ASCII alphabetical.


![0101](https://avatars.githubusercontent.com/u/217092?s=60&v=4)
![DedSec256](https://avatars.githubusercontent.com/u/26364714?s=60&v=4)
![Happypig375](https://avatars.githubusercontent.com/u/19922066?s=60&v=4)
![KevinRansom](https://avatars.githubusercontent.com/u/5175830?s=60&v=4)
 <img src="https://avatars.githubusercontent.com/u/29605222?s=60&v=4" width="60">
![MattGal](https://avatars.githubusercontent.com/u/7862010?s=60&v=4)
![NikolaMilosavljevic](https://avatars.githubusercontent.com/u/9423618?s=60&v=4)
![NinoFloris](https://avatars.githubusercontent.com/u/4218809?s=60&v=4)
![Smaug123](https://avatars.githubusercontent.com/u/3138005?s=60&v=4)
![T-Gro](https://avatars.githubusercontent.com/u/46543583?s=60&v=4)
![abonie](https://avatars.githubusercontent.com/u/20281641?s=60&v=4)
![alfonsogarciacaro](https://avatars.githubusercontent.com/u/8275461?s=60&v=4)
![auduchinok](https://avatars.githubusercontent.com/u/3923587?s=60&v=4)
![bmitc](https://avatars.githubusercontent.com/u/65685447?s=60&v=4)
![cartermp](https://avatars.githubusercontent.com/u/6309070?s=60&v=4)
![dawedawe](https://avatars.githubusercontent.com/u/3221269?s=60&v=4)
![dsyme](https://avatars.githubusercontent.com/u/7204669?s=60&v=4)
![edgarfgp](https://avatars.githubusercontent.com/u/31915729?s=60&v=4)
![goswinr](https://avatars.githubusercontent.com/u/884885?s=60&v=4)
![gusty](https://avatars.githubusercontent.com/u/1261319?s=60&v=4)
![jwosty](https://avatars.githubusercontent.com/u/4031185?s=60&v=4)
![kant2002](https://avatars.githubusercontent.com/u/4257079?s=60&v=4)
![kerams](https://avatars.githubusercontent.com/u/5063478?s=60&v=4)
![majocha](https://avatars.githubusercontent.com/u/1760221?s=60&v=4)
![mmitche](https://avatars.githubusercontent.com/u/8725170?s=60&v=4)
![nojaf](https://avatars.githubusercontent.com/u/2621499?s=60&v=4)
![psfinaki](https://avatars.githubusercontent.com/u/5451366?s=60&v=4)
![rosskuehl](https://avatars.githubusercontent.com/u/94796738?s=60&v=4)
![safesparrow](https://avatars.githubusercontent.com/u/2478401?s=60&v=4)
<img src="https://avatars.githubusercontent.com/u/87944?s=60&v=4" width="60">
![tboby](https://avatars.githubusercontent.com/u/447391?s=60&v=4)
![teo-tsirpanis](https://avatars.githubusercontent.com/u/12659251?s=60&v=4)
![vzarytovskii](https://avatars.githubusercontent.com/u/1260985?s=60&v=4)


We want to thank all contributors, community members and users of F#. On top of that, we want to call out the following heavily contributing members and a selection of their recent work in the [F# repository](https://github.com/dotnet/fsharp) explicitly:

- [@kerams](https://github.com/kerams) for the majority of new language features, completion improvements and many other additions to F#.

- [@auduchinok](https://github.com/auduchinok) for parser recovery improvements, strict indentation mode and many performance improvements in the F# compiler itself.

- [@nojaf](https://github.com/nojaf) for syntax tree additions, code printing additions, signature files improvements and graph-based typechecking.

- [@safesparrow](https://github.com/safesparrow) for compiler observability and tracing, graph-based typechecking and parallel optimization.

- [@majocha](https://github.com/majocha) for improvements in the VS editor support which improve user experience, editor speed and code search experience.

- [@edgarfp](https://github.com/edgarfgp) for improvements in error handling, user friendliness of existing diagnostics and additions of many new F# diagnostics. 


## Contributor showcase

As in our last release, we want to highlight individuals who contribute to F#, and publish a short text about them using their own words.


### dawedawe
I'm David Schaefer, aka dawe, living near Cologne, Germany.

I fell in love with functional programming (FP) while being exposed to theoretical computer science during my time at the university.
Back then, I made my first stab at contributing to open source by maintaining some Haskell packages of OpenBSD.
After university, I wasn't able to find an FP job and fell out of package maintainership.
But a C# job at least allowed me to stay in touch with FP by using F# in my free time.

Motivated to use F# professionally, I joined an awesome small company (Rhein-Spree) which uses F# quite a lot.
That led to small contributions to [Fantomas](https://fsprojects.github.io/fantomas/) and with the great mentorship of Florian,
I worked my way up to co-maintainership of it.

The ride really got wild when I joined the G-Research Open Source team in March 2023 to work full time on the F# eco-system.
In parallel, we launched the Amplifying F# initiative. Watch our video of [fsharpconf 2023](https://www.youtube.com/watch?v=69VVSnng8TY) to learn more.
Today, I help maintaining various pieces in the eco-system. I'm proud I earned the needed trust for that.

With the [Data Science in F#](https://datascienceinfsharp.com/) conference still fresh on my mind, I want to emphasize how much the human side means to me.
Sharing a coffee or a beer with friends from all over the world,
discussing new ideas how to move things forward,
celebrating wins together and consoling each other after a loss - that's for a large part why I do this.
And all the green squares on GitHub, of course.

![Dawe](./david.jpg)

### auduchinok

I'm Eugene Auduchinok, I work on F# support in JetBrains Rider and currently live in Amsterdam.
I have first met with F# in the university as some of the courses used it. F# worked good for the tasks and was really fun to use.
When Rider was announced, I was happy since I loved working on a Mac and using IntelliJ. The problem was it didn't have any support for F#, so I've applied for an internship to try to make it happen. This worked out and now I use it daily and enjoy seeing other people using it too. :)
Since then I've had a chance to contribute to various parts of F# tooling, work with wonderful people, and even make some impact on the language design itself.

![Eugene](./eugene.jpg)

## What's next?

We continue working on F#: be it the language itself, compiler performance, Visual Studio features and improvements and many other aspects of F#.
- See the [overall work tracking issues](https://github.com/orgs/dotnet/projects/126/views/17)
- Do you want to help? There is a ["help wanted" issues list](https://github.com/dotnet/fsharp/issues?q=is%3Aissue+is%3Aopen+sort%3Aupdated-desc+label%3A%22help+wanted%22)
- Getting started with the compiler codebase? We have a curated list of [good first issues](https://github.com/dotnet/fsharp/issues?q=is%3Aopen+is%3Aissue+label%3A%22good+first+issue%22)
