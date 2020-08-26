# F# 5 update for August

We’re excited to announce more updates to F# 5 today to go alongside [.NET 5 preview 8](https://devblogs.microsoft.com/dotnet/announcing-net-5-0-preview-8/)! We've shipped various updates since the beginning of this year:

* [F# 5 preview 1](https://devblogs.microsoft.com/dotnet/announcing-f-5-preview-1/)
* [F# 5 update for .NET 5 preview 5](https://devblogs.microsoft.com/dotnet/f-5-update-for-net-5-preview-4/)
* [F# 5 update for June](https://devblogs.microsoft.com/dotnet/f-5-and-f-tools-update-for-june/)

Today, we're please to announce the last of our F# 5 feature work. There is one minor enhancement planned for the .NET 5 RC, but other than that we are finished with F# 5! From this point forward, our journey to shipping F# 5 will be focused mostly on bug fixes and addressing feedback.

You can get the latest F# 5 in these ways

* [Install the latest .NET 5 preview SDK](https://dotnet.microsoft.com/download/dotnet-core/5.0)
* [Install .NET for Jupyter/nteract](https://github.com/dotnet/interactive/#jupyter-and-nteract)
* [Install .NET for VSCode Notebooks](https://github.com/dotnet/interactive/#visual-studio-code)

If you’re using Visual Studio on Windows, you’ll need both the .NET 5 preview SDK and [Visual Studio Preview installed](https://visualstudio.microsoft.com/vs/preview/).

## Using F# 5 preview

You can use F# 5 preview via the [.NET 5 preview SDK](https://dotnet.microsoft.com/download/dotnet-core/5.0), or through the [.NET and Jupyter Notebooks support](https://devblogs.microsoft.com/dotnet/net-interactive-is-here-net-notebooks-preview-2/).

If you’re using the .NET 5 preview SDK, check out a [sample repository](https://github.com/cartermp/fs5preview) showing off some of what you can do with F# 5. You can play with each of the features there instead of starting from scratch.

If you’d rather use F# 5 in your own project, you’ll need to add a `LangVersion` property with `preview` as the value. It should look something like this:

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net5.0</TargetFramework>
    <LangVersion>preview</LangVersion>
  </PropertyGroup>

  <ItemGroup>
    <Compile Include="Program.fs" />
  </ItemGroup>

</Project>
```

```html
<script src="https://gist.github.com/cartermp/3a12e552cc64918d697c430c7b5cfcf7.js"></script>
```

Alternatively, if you’re using Jupyter Notebooks and want a more interactive experience, check out a [sample notebook](https://gist.github.com/cartermp/6b91c3561c6a5efca4288dca37c15edc) that shows the same features, but has a more interactive output.

## String Interpolation

This preview adds String Interpolation, one of the most highly-requested language features and the very first feature that we had an initial design for in the [F# Language Design repository](https://github.com/fsharp/fslang-design). The design has undergone a lot of discussion over the years, but finally a breakthrough on how to best handle it was made by [Yatao Li](https://github.com/yatli), who also supplied an initial implementation.

F# interpolated strings are fairly similar to C# or JavaScript interpolated strings, in that they let you write code in "holes" inside of a string literal. Here's a basic example:

```fsharp
// Basic interpolated string
let name = "Phillip"
let age = 29
printfn $"Name: {name}, Age: {age}"

// Inline expressions
printfn $"I think {3.0 + 0.14} is close to {System.Math.PI}!"
```

```html
<script src="https://gist.github.com/cartermp/4ed6b98cf35c650bef637b82efd26dc6.js"></script>
```

However, F# interpolated strings also allow for typed interpolations, just like the `sprintf` function, to enforce that an expression inside of an interpolated context conforms to a particular type. It uses the same format specifiers.

```fsharp
// Typed interpolation
// '%s' requires the interpolation to be of type string
// '%d' requires the interpolation to be an integer
let name = "Phillip"
let age = 29

printfn $"Name: %s{name}, Age: %d{age}"

// This gives an error because the types don't match!
printfn $"Name: %s{age}, Age: %d{name}"
```

```html
<script src="https://gist.github.com/cartermp/af61c08e3b2b6bfb810d7302b50a0328.js"></script>
```

## nameof is now complete

In the [June update](https://devblogs.microsoft.com/dotnet/f-5-and-f-tools-update-for-june/), we mentioned that we were finishing up several design changes for `nameof`. These are now complete!

To recap, `nameof` resolves the symbol it's being used for and produces a name that represents what it's called in F# source. This is useful in various scenarios, such as logging, and protects your logging against changes in source code.

```fsharp
let months =
    [
        "January"; "February"; "March"; "April";
        "May"; "June"; "July"; "August"; "September";
        "October"; "November"; "December"
    ]

let lookupMonth month =
    if (month > 12 || month < 1) then
        invalidArg (nameof month) (sprintf "Value passed in was %d." month) // use 'nameof' on the parameter name

    months.[month-1]

printfn "%s" (lookupMonth 12)
printfn "%s" (lookupMonth 1)
printfn "%s" (lookupMonth 13) // Throws an exception!
```

```html
<script src="https://gist.github.com/cartermp/d3bb6ffadc7751b1616e42f3f96fbcb8.js"></script>
```

The last line will throw an exception and "month" will be shown in the error message.

You can take a name of nearly everything in F#:

```fsharp
module M =
    let f x = nameof x

printfn "%s" (M.f 12)
printfn "%s" (nameof M)
printfn "%s" (nameof M.f)
```

```html
<script src="https://gist.github.com/cartermp/678dc8cde08565cbbb9726a0e65ffbdd.js"></script>
```

Three final additions are changes to how operators work: the addition of the `nameof<'type-parameter>` form for generic type parameters, and the ability to use `nameof` as a pattern in a pattern match expression.

```fsharp

nameof(+) // gives '+'
nameof op_Addition // gives 'op_Addition'

type C<'TType> =
    member _.TypeName = nameof<'TType> // Nameof with a generic type parameter via 'nameof<>'

/// Simplified version of EventStore's API
type RecordedEvent = { EventType: string; Data: byte[] }

/// My concrete type:
type MyEvent =
    | A of AData
    | B of BData

// use 'nameof' instead of the string literal in the match expression
let deserialize (e: RecordedEvent) : MyEvent =
    match e.EventType with
    | nameof A -> A (JsonSerializer.Deserialize<AData> e.Data)
    | nameof B -> B (JsonSerializer.Deserialize<BData> e.Data)
    | t -> failwithf "Invalid EventType: %s" t
```

```html
<script src="https://gist.github.com/cartermp/4b633fb5ad214a2538d0dd93a7b794ad.js"></script>
```

The `nameof<'type-parameter>` form aligns with how `typeof` and `typedefof` work in F# today.

## Open Type declarations

This preview also adds Open Type Declarations. It's like Open Static Classes in C#, except with some different syntax and some slightly different behavior to fit F# semantics.

With Open Type Declarations, you can `open` any type to expose static contents inside of it. Additionally, you can `open` F#-defined unions and records to expose their contents. This can be useful if you have a union defined in a module and want to access its cases, but don't want to open the entire module.

```fsharp
open type System.Math

let x = Min(1.0, 2.0)

module M =
    type DU = A | B | C

    let someOtherFunction x = x + 1

// Open only the type inside the module
open type M.DU

printfn "%A" A
```

```html
<script src="https://gist.github.com/cartermp/b2a30361927427314ab781c1bb30be98.js"></script>
```

## Overloads of custom keywords in computation expressions

Computation expressions are a powerful feature for library and framework authors. They allow you to greatly improve the expressiveness of your components by letting you define well-known members and form a DSL for the domain you're working in.

We've enhanced computation expressions to allow for [Applicative forms] already. This time, [Diego Esmerio](https://github.com/Nhowka) and [Ryan Riley](https://github.com/panesofglass) contributed a design an implementation to allow for overloading custom keywords in computation expressions. This new feature allows code like the following to be written:

```fsharp
open System

type InputKind =
    | Text of placeholder:string option
    | Password of placeholder: string option

type InputOptions =
  { Label: string option
    Kind : InputKind
    Validators : (string -> bool) array }

type InputBuilder() =
    member t.Yield(_) =
      { Label = None
        Kind = Text None
        Validators = [||] }

    [<CustomOperation("text")>]
    member this.Text(io, ?placeholder) =
        { io with Kind = Text placeholder }

    [<CustomOperation("password")>]
    member this.Password(io, ?placeholder) =
        { io with Kind = Password placeholder }

    [<CustomOperation("label")>]
    member this.Label(io, label) =
        { io with Label = Some label }

    [<CustomOperation("with_validators")>]
    member this.Validators(io, [<ParamArray>] validators) =
        { io with Validators = validators }

let input = InputBuilder()

let name =
    input {
    label "Name"
    text
    with_validators
        (String.IsNullOrWhiteSpace >> not)
    }

let email =
    input {
    label "Email"
    text "Your email"
    with_validators
        (String.IsNullOrWhiteSpace >> not)
        (fun s -> s.Contains "@")
    }

let password =
    input {
    label "Password"
    password "Must contains at least 6 characters, one number and one uppercase"
    with_validators
        (String.exists Char.IsUpper)
        (String.exists Char.IsDigit)
        (fun s -> s.Length >= 6)
    }
```

```html
<script src="https://gist.github.com/cartermp/802d640dd1188100a30da93137d4fc6b.js"></script>
```

Prior to this change, you could write the `InputBuilder` type as it is, but you couldn't use it the way it's used in the previous example. Since overloads, optional parameters, and now `System.ParamArray` types are allowed, everything just works as you'd expect it to.

## Interfaces can be implemeneted at different generic instantiations

The final feature enabled in this preview is an enhancement to interfaces in F#. You can now implement the same interface at different generic instantiations. [Lukas Rieger](https://github.com/0x53A) contributed an initial design and implementation of this feature.

```fsharp
type IA<'T> =
    abstract member Get : unit -> 'T

type MyClass() =
    interface IA<int> with
        member x.Get() = 1
    interface IA<string> with
        member x.Get() = "hello"

let mc = MyClass()
let iaInt = mc :> IA<int>
let iaString = mc :> IA<string>

iaInt.Get() // 1
iaString.Get() // "hello"
```

```html
<script src="https://gist.github.com/cartermp/d25aede7921a36e88ee46b9b11386b4a.js"></script>
```

## Finishing F# 5

Now that we're feature complete for F# 5, minus a tweak here or there, we're going to shift our focus:

* Ensure F# 5 features are of high quality and do not introduce any problems
* Address bug fixes and high-priority feedback items for F# 5
* Improve our engineering system in the [F# development repository](https://github.com/dotnet/fsharp), particularly to improve our testing infrastructure so that it's easier for open source contributors to work there

After F# 5 ships alongside .NET 5, we'll also start our planning for the next wave of F# investments. We'd love for you to join us when we get there.

Cheers, and happy F# coding!
