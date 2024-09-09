---
post_title: 'Why is F# code so robust and reliable?'
author1: shchurvladimir
post_slug: why-is-fsharp-code-so-robust-and-reliable
microsoft_alias: psemkin
featured_image: fsharprobust.png
categories: .NET, F#
tags: f#, clean code, customer experience
ai_note: hide
summary: F# is not just for math and big data, it's a general purpose language that will greatly reduce the amount of bugs in your code.
post_date: 2024-09-17 10:05:00
---

> This is a guest post by Vladimir Shchur, lead developer at [Access Softek](https://accesssoftek.com/). Vladimir is an active open-source contributor, the author of [Oxpecker](https://github.com/Lanayx/Oxpecker) and [Pulsar.Client](https://github.com/fsprojects/pulsar-client-dotnet) libraries. He has also published a series of posts on [Medium](https://medium.com/@lanayx) and [contributed to F# Core](https://github.com/dotnet/fsharp/pull/17277).

In Access Softek, we've been developing software for financial institutions using C# and .NET for two decades, at the same time suffering from lots of bugs. We struggled to implement the [Zero Bug Policy](https://www.ministryoftesting.com/articles/zero-bug-policy-the-myths-and-the-reality) and had the green light to build one of our new projects, namely [EasyCoin](https://accesssoftek.com/easycoin/), in F#, as it was claimed to be a very robust and effective tool.

The EasyCoin project was challenging, requiring the implementation of distributed transactions with all the possible unexpected outcomes that could happen along the way. The transaction flow included integration with several services, including some internal SOAP ones. The project also required creating a client-facing UI, a web API, an admin site, and a few other elements. The implementation took us about a year and a half for the team of 4 F# full-stack developers, a team lead, a PM and a QA. 

The product has been live for a year, and remarkably, only one user-facing bug has been found so far. I attribute this success to the chosen language. In this post, I will share the specific reasons F# allowed us to write robust code. Some of them may be not applicable to your projects and this also might not mention F# aspects which your company would want to leverage. The following are the particular features that made our company avoid bugs in the software we do and hence make our customers happy.

## Immutability by default

F#'s basic blocks — values and records — are immutable by default (as in, in F#, it's easier to write immutable code than mutable code). This is good for thread safety and general code predictability - since objects don't change state, components can rely on them without worrying about how other parts of the program might alter them:

```fsharp  
let x = 1
x <- 2                      // error, x is not mutable

type User = { Id: int }
let process (user: User) =
    user.Id <- 2            // error, Id field is not mutable
    user <- { Id: 2 }       // error, user argument is not mutable
```

## Discriminated unions with exhaustive check

[Discriminated unions (DU)](https://learn.microsoft.com/dotnet/fsharp/language-reference/discriminated-unions) is an F# key feature which lets types hold a closed set of arbitrary data. Importantly, DU `match` is exhaustive, meaning that adding a case would raise warnings in all not yet handled cases, preventing potential bugs:

```fsharp
open System

type LoginType =
    | Password of string
    | PinCode of int

let printLoginType loginType =
    match loginType with  // warning, not all cases are handled
    | Password password -> Console.WriteLine(password)
```

## No nulls by default

While many modern languages added some control for doing more null checks to prevent `NullReferenceException`, F# avoided them from the inception. This means such exceptions are nearly impossible in the end-to-end F# workflows (although, you still need to handle them during interop):

```fsharp
type User = { Id: int }
let user = { Id = null }     // error, null is not assignable to int
let user: User = null        // error, null is not assignable to record User

let s1: string = null        // allowed in F# 8 due to interop with C#, warning in F# 9 (when opted in)
let s2: string | null = null // allowed in F# 9 due to interop with C# and null reference type support (when opted in)
```

*Idiomatic* F# way to deal with missing values is using [`Option<'T>`](https://learn.microsoft.com/dotnet/fsharp/language-reference/options) type, so that the developer has to explicitly unwrap it and handle the missing value:

```fsharp
open System

let numberOption = [1; 2; 3] |> List.tryFind (fun x -> x % 2 = 0)

match numberOption with
| Some x -> Console.WriteLine($"Even number was found: {x}")
| None -> Console.WriteLine("Even number was not found")
```

## No exceptions in the business logic

Throwing custom exceptions is one of the popular ways to deal with errors in business logic, however it is also a source of bugs, since there is no way to ensure those exceptions are handled properly. While it is still reasonable to throw exceptions in unexpected cases (like when the database connection fails), you shouldn't do it in the middle of the business transaction. F# has a dedicated [`Result<'T>`](https://learn.microsoft.com/dotnet/fsharp/language-reference/results) type for handling errors, and you can also easily define your more specific DUs:

```fsharp
type TransactionStatus =
    | Ready
    | Pending

let executeTransaction transaction =
    if validate transaction then
        try
            let isReady = callExternalService transaction
            if isReady then Ok Ready else Ok Pending
        with ex ->
            Error $"Service is unavailable: {ex.Message}"
    else
        Error "Transaction is invalid"
```

## Strict dependency order

In F#, all variables, functions, types and files can only depend on variables, functions, types and files defined earlier. The benefits of this are the fact that a circular dependency is not possible by default and extra clarity with "what depends on what", which helps during code analysis and PR reviews:

```fsharp
open System

type Person = { BillingAddress: Address } // error, since the required type is defined below
type Address = { Street: string }

helloWorld() // error, the function is defined below
let helloWorld() = Console.WriteLine("Hello")
```

```xml
<ItemGroup>
    <Compile Include="Operations.fs" /> <!-- error, since operations use models defined below -->
    <Compile Include="Models.fs" />
</ItemGroup>
```

## Warnings on unused expression results

F# warnings will make you think twice about dangling values, where other popular languages would simply ignore that fact, unless they had an external analyzer:

```fsharp
open System.Collections.Generic

let people = Dictionary<string, int>()
people.Add("Jack", 1)
people.Remove("John") // warning, Remove returns bool value, you need to think about missing case
```

## Typed primitives

F# has a notion of [Units of Measure](https://learn.microsoft.com/dotnet/fsharp/language-reference/units-of-measure) which help the compiler to verify that arithmetic relationships have correct units. Among other things, this (together with [`FSharp.UMX`](https://github.com/fsprojects/FSharp.UMX) package), make it very easy to have your primitives strongly typed with zero cost at runtime:

```fsharp
open FSharp.UMX
  
[<Measure>] type private tenantId
[<Measure>] type private transactionId
  
type TenantId = string<tenantId>
type TransactionId = string<transactionId>
  
type Transaction = {
    TenantId: TenantId
    TransactionId: TransactionId
}
  
let getTransactions tenantId transactionId = 
    [
        { TenantId = tenantId; TransactionId = transactionId }
        { TenantId = tenantId; TransactionId = tenantId } // error, TenantId and TransactionId are different types
    ]
  
getTransactions %"myTenant" %"1234"
```

## Explicit conversions

Implicit type conversions are a common source of accidental errors. For example, this code will compile just fine in languages such as TypeScript or C#:

```cs
var x = 1 + "";       // compiles
```

F# compiler won't allow such implicit conversions and will produce an error:

```fsharp
let x = 1 + ""        // error, the type 'string' does not match the type 'int'
let y = string 1 + "" // ok
```

Another prominent example of accidental conversion bugs is string interpolation. Let's say we had this C# code:

```cs
var age = 21;
var displayAge = $"Your age is {age}";
```

Then someone refactored the code and used anonymous type, but forgot to update the interpolation:
```cs
var age = new { Age = 21 };
var displayAge = $"Your age is {age}"; // no errors or warnings, but wrongly evaluates to "Your age is { Age = 21 }"
```

With F#, you can [specify](https://learn.microsoft.com/dotnet/fsharp/language-reference/plaintext-formatting) the type of the "hole" to avoid such errors:

```fsharp
let age = 21
let displayAge = $"Your age is %i{age}"             // ok

let ageRecord = {| Age = 21 |}
let displayAgeRecord = $"Your age is %i{ageRecord}" // compilation error, ageRecord is not an integer
```

## Functional approach to concurrency
  
Reading and writing code with locks is difficult, so actors frameworks are there for help. [Actors](https://en.wikipedia.org/wiki/Actor_model) simplify reasoning and reduce shared data bugs by assigning a single owner to data and communicating with each other using messages. Each actor should be able to handle different types of message and they can be conveniently modeled as Discriminated Unions.

F# has a built-in actor support using [`MailboxProcessor<'Msg>`](https://fsharp.github.io/fsharp-core-docs/reference/fsharp-control-fsharpmailboxprocessor-1.html), but you can also easily switch to a more modern [Channels library](https://learn.microsoft.com/dotnet/core/extensions/channels):

```fsharp
open System
open System.Threading.Channels
  
type MyMessage =
    | Start
    | Print of string
    | Stop
    
let actor = Channel.CreateUnbounded<MyMessage>(UnboundedChannelOptions(SingleReader = true))
let task = backgroundTask {
    let mutable continueLoop = true
    while continueLoop do
        match! actor.Reader.ReadAsync() with
        | Start -> Console.WriteLine("Started")
        | Print s -> Console.WriteLine(s)
        | Stop -> continueLoop <- false
}


actor.Writer.TryWrite Start |> ignore                 // send 1st message to the actor
actor.Writer.TryWrite (Print "Hello world") |> ignore // send 2nd message to the actor

task.Wait()
```

## Explicit dependency injection

OOP projects usually use DI containers for injecting dependencies at runtime, thus dependency resolution bugs often reach production, because real dependencies are not covered by unit tests and missing registration can't be caught at compile time. With functional architecture, dependencies are passed explicitly as function arguments, which results in:
 - Compile time verification, no "unregistered dependency" errors are possible at runtime
 - Better separation of concerns, different functions only depend on their arguments, not on shared fields (like in constructor injection)
 - Easier testing, since all you need to do is to specify function arguments

Dependency injection in F# is fully described in [this article](https://medium.com/@lanayx/dependency-injection-in-f-the-missing-manual-d376e9cafd0f). Example code:

```fsharp
let notifyUser (env: #IGetUserSettings & #ISendEmail & #ISendSms)   
               userId message =  
    task {  
        let! userSettings = env.GetUserSettings(userId)  
        match userSettings.NotificationType with  
        | Email address ->   
            return! env.SendEmail(address, message)  
        | Sms phone ->  
            return! env.SendSms(phone, message)  
    }
```

## The bottom line

These are just some of the ways that F# helped us write robust and readable code that dropped our bug count to nearly zero. What's more, F# libraries let you write type-safe SQL (using [Type Providers](https://learn.microsoft.com/dotnet/fsharp/tutorials/type-providers/)), type-safe HTML or IaC (using [Computational Expressions](https://learn.microsoft.com/dotnet/fsharp/language-reference/computation-expressions)), type-safe ASP.NET route parameters (using [`printf` module](https://learn.microsoft.com/dotnet/fsharp/language-reference/plaintext-formatting)), and many more. And it is also worth mentioning that F# brings its goodness not just to .NET, but can run on other platforms with transpilers available to JavaScript and Python using the [Fable project](https://fable.io).

With that, I'm wrapping up, I hope this article has provided valuable insights and inspired you to explore F# if you haven't done so already. F# community will be always ready to help in [Amplifying F#](https://amplifyingfsharp.io/), [Slack](https://fsharp.org/guides/slack/), [Discord](https://discord.com/invite/R6n7c54), or [X](https://x.com/hashtag/fsharp?f=live).
