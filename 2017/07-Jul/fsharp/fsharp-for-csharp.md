# Get Started with F# as a C# developer

One of our previous posts, [Why You Should Use F#](https://blogs.msdn.microsoft.com/dotnet/2017/05/31/why-you-should-use-f/), listed a few reasons why F# is worth trying out today.  In this post, we'll cover some of the basics you'll need to know to be successful.  This post is intended for people who are coming from a C#, Java, or anoter object-oriented background.  The concepts covered here should seem very familiar to existing F# programmers.

This post will not make any attempt to show you how to "translate" C# code into F#. This is because C# and F# are fundamentally a part of different programming paradigms, which makes each a tool suited for their own purposes. We think you'll find that it expands your mind and helps you be a better programmer to learn functional programming concepts.  That value will come to you the fastest if you don't think about trying to translate from one paradigm to another.

Now is the time to be curious, inquisitive, and ready to learn brand-new things.  Let's get going!

## Four immediate differences

Before we dive deep into concepts, there are a few immediate ways that F# differs from C#.

1. Semicolons, braces (`{}`), and parentheses are few and far between.
2. Type inference instead of annotating types. Think C#'s `var`, but on steroids.
3. No `return` statement at the end of a function.
4. A few more operators (e.g., `|>`).

These are also surface-level differences in F#.  For example, here is some basic F# code with two functions and a printed result:

```fsharp
let square x = x * x

let sumOfSquares n =
    [1..n] // Create a new list with values 1 to n
    |> List.map square // Square each item
    |> List.sum // Sum it up!

printfn "Sum of the first 5 squares is %d" (sumOfSquares 12)
```

Note that there are no type definitions, semicolons, or braces.  The only parentheses are used to disambiguate `sumOfSquares` and `12`.  The pipeline operator (`|>`) is used much like a unix-style pipe.  `square` is a function passed directly as a parameter to `List.map`.  And so on!

Although there are many surface-level differences we could talk about, there are deeper things going on here which are key to understanding F#.

## Mapping core C# concepts to core F# concepts

The following table aims to provide a basic mapping of some of the core concepts from C# to F#:

|C# and Object-Oriented Programming|F# and Functional Programming|
|----------------------------------|------------------------------|
| Variables | Immutable values |
| Statements | Expressions |
| Objects with Methods | Types and function |
| Namespaces | Modules and Namespaces |

Here's a quick primer on some the above terminology:

* Variables are values which can change in-place, or _vary_.  It's in the name!
* Immutable values are values which cannot change after they are assigned.
* [Statements](https://en.wikipedia.org/wiki/Statement_(computer_science)) are units of work, performed imperatively, by a running program.
* [Expressions](https://en.wikipedia.org/wiki/Expression_(computer_science)) are units of code which evaluate to a value.
* [Types](https://en.wikipedia.org/wiki/Data_type) are classifications of data in a program.
* Modules in F# are groupings of values, types, and functions.

We'll assume that you're comfortable with objects, methods, functions, and namespaces.

It's worth noting that everything on the left-hand side is possible in F# and quite easy to accomplish.  There are also a things on the right-hand side which are possible in C#, though they are more difficult to accomplish.  It's also worth noting that items in the left-hand column are not "bad" for F#, either.  Objects with methods are perfectly valid to use in F#, and can often be the best approach for F# depending on your scenario.

## Immutability values instead of variables

One of the most transformative concepts in functional programming is immutability.  It's often overlooked and underrated in tutorials online, but this is often the first and biggest hump to get over if you've never used a language where immutability is the default.  Nearly all functional programming languages have immutability at their core.

```fsharp
let x = 1
```

In the above statement, the value of `1` is _bound_ to the name `x`.  `x` now always refers to the value `1` for its lifetime, and cannot be modified.  That is, the following code will not compile:

```fsharp
let x = 1
x = 2
```

We said that immutability was transformative, and that means that there are some very concrete differences in approaches to solving a problem.  For example, `for` loops and other basic imperative programming operations are not typically used in F#.

As a more concrete example, say you wish to compute the squares of an input array of numbers.  Here is an approach to that in F#:

```fsharp
// Define a function which squares a value
let square x = x * x

let getSquares items = items |> Array.map square

let arr = [| 1; 2; 3; 4; 5 |] // Create a new F# array

printfn "Squares of %A are %A" arr (getSquares arr)
```

Notice that there isn't a `for` loop to be seen.  At a conceptual level, this is very different from imperative code.  We're not squaring each item in the list.  We are mapping the `square` function over the input list to product a list of squared values.  This distinction is subtle in concept, but in practice it can lead to dramatically different code.  For starters, `getSquares` actually produces a whole other list.

Immutability does more than just change the way you manipulate data like lists.  The concept of [Referential Transparency](http://wiki.c2.com/?ReferentialTransparency) will come naturally in F#, and it is a driver in how systems are built and pieces of that system are composed.  Execution characteristics of a system become more predictable, because values cannot change when you did not anticipate them to change.

Furthermore, when values are immutable, concurrent programming becomes far simpler.  Because values cannot be changed due to immutability, some of the more difficult concurrency problems you can encounter in C# are not a concern in F#.

## Expressions instead of statements

As mentioned to above, F# makes use of *expressions*.  This is in contrast with C#, which uses *statements* for nearly everything.  The difference between the two can initially seem subtle, but there is one thing to always keep in mind: an expression produces a value.  Statements do not.

```fsharp
// 'getMessage' is a function, and `name` is an input parameter.
let getMessage name =
    if name = "Phillip" then   // 'if' is an expression.
        "Hello, Phillip!"      // This string is an expression.  It's the return value.
    else
        "Hello, other person!" // Same with this string.

let phillipMessage = getMessage "Phillip" // getMessage, when called, is an expression. Its value is bound ot 'phillipMessage'.
let alfMessage = getMessage "Alf" // This expression is bound to 'alfMessage'!
```

In the above code sample, you'll notice a few things which are very different from imperative laguages like C#:

* `if` is also an expression, not a statement!
* Each branch of the `if` expression is the return value of the `getMessage` function
* Each invocation of `getMessage` is an expression which takes a string and produces a string

Although this is very different from C#, you'll most likely find that it feels natural when writing code in F#.

Diving a bit deeper, F# actually uses expressions to model statements.  These return the [`unit`](https://docs.microsoft.com/en-us/dotnet/fsharp/language-reference/unit-type) type.  `unit` is roughly analagous to `void` in C#, although it is subtly different.

```fsharp
let names = [ "Alf"; "Vasily"; "Shreyans"; "Jin Sun"; "Moulaye" ]

// For loop.  The 'do' keyword enforces that the inner scope should have type 'unit'.
// If the inner scope does not, its result is implicitly ignored.
for name in names do
    printfn "My name is %s" name // printfn returns unit.
```

In the above sample `for` expression, everything is of type `unit`.  Unit expressions are expressions which return no value.

## F# arrays, lists, and sequences

The above code samples have used F# arrays and lists.  This section explains them a bit more.

F# comes with a few collection types, the most common of which are arrays, lists, and sequences.

* F# arrays are mutable .NET arrays
* F# lists are singly-linked lists
* F# sequences are a type alias for `IEnumerable<'T>`

F# Arrays behave just like arrays in C#.  They are mutable and their values can be changed in-place.  They are evaluated eagerly.  F# lists are singly-linked and immutable.  They can be used to form list patterns with F# pattern matching.  They are evaluated eagerly.  F# sequences are `IEnumerable<'T>` under the covers.  They are evaluated lazily.

F# arrays, lists, and sequences also have array, list, and sequence expression syntax.  This is very convenient for different scenarios where you can generate one programatically.

```fsharp
// Generate 100 square values as an F# list.
let first100Squares = [ for x in 1..100 -> x * x ]

// Same as above, but as an array!
let first100SquaresArray = [| for x in 1..100 -> x * x |]

// Function which generates an infinite sequence of all odd values.
//
// Call in conjunction with Seq.take!
let odds = 
    let rec loop x = // Uses a recursive inner function
        seq { yield x
              yield! loop (x + 2) }
    loop 1

printfn "First 3 odds: %A" (Seq.take 3 odds)
// Prints:  "First 10 odds: seq [1; 3; 5]
```

### Mapping common LINQ methods to F# functions

If you are familiar with LINQ methods, the following table should help you undertand analogous functions in F#.  This table is, in no way, intended to be exhaustive.

| LINQ | F# function |
|------|-------------|
| `Where` | `filter` |
| `Select` | `map` |
| `GroupBy` | `groupBy` |
| `SelectMany` | `collect` |
| `Aggregate` | `fold` or `reduce` |
| `Sum` | `sum` |

We highly encourage you to explore these functions to see what they do.  You can often see what they do by inspecting their type signature.  For example, here are the signatures for `fold` and `reduce`:

```fsharp
val fold: folder: ('State -> 'T -> 'State) -> source: seq<'T> -> 'State
val reduce: reduction: ('T -> 'T -> 'T) -> source: seq<'T> -> 'T
```

Although both are similar in that they will collapse a source sequence into a value, `fold` is more flexible because it can produce a type which isn't the same as the parameterized type of the source sequence.  In fact, `reduce` is merely a special case of `fold`!

You'll also notice that the same set of functions exist for the `Seq` module, `List` module, and `Array` module.  The `Seq` module functions can be used on F# sequences, lists, or arrays.  The array and list functions can only be used on F# arrays and F# lists, respectively.  Additionally, F# sequences are lazy, whereas F# lists and arrays use eager evaluation.  Using `Seq` functions on an F# list or F# array will incur lazy evaluation, and the type will then be an F# sequence.

Although the above paragraph may be a lot to unpack, it should feel intuitive as you write more F#.

## Functional pipelines

You may have noticed the `|>` used in many code samples above.  This operator is very similar to unix pipes: it takes something on the left-hand side, and makes that the input to something on the right-hand side.  This operator (called "pipe", or "pipeline") is used to form a functional pipeline.  Here's an example:

```fsharp
let square x = x * x
let isOdd x = x % 2 <> 0

let getOddSquares items =
    items
    |> Seq.filter isOdd
    |> Seq.map square
```

Walking through this code sample, `items` is passed as input to the `Seq.filer` function.  The ouput of `Seq.filter`, a sequence, is then passed as input to the `Seq.map` function.  The output of `Seq.map` is the output of `getOddSquares`.

Use of the pipeline operator is so much fun that it's rare to see F# code which _doesn't_ make use of it.  It's usually near the top of everyone's favorite F# feature list!

## F# Types

Because F# is a .NET language, it shares the same primitive types that C# does: `string`, `int`, etc.  It also has .NET objects, and supports the four main pillars of object-oriented programming.  F# also has two primary types not found in C#: [Records](https://docs.microsoft.com/en-us/dotnet/fsharp/language-reference/records) and [Discriminated Unions](https://docs.microsoft.com/en-us/dotnet/fsharp/language-reference/discriminated-unions).  There are some examples of these above, but we'll explain them a bit further here.

[Records](https://docs.microsoft.com/en-us/dotnet/fsharp/language-reference/records) are a named, ordered grouping of values which have equality baked in.  And by equality, we mean in the most literal sense.  There is no need to distinguish between reference equality or some custom definition of value equality between two objects.  Records are values, and values have equality.  They are Product Types for the category theorists out there.  They have a number of uses, but one of the most obvious ones is a replacement for POCO or POJO classes.

```fsharp
open System

// Here's how you define a record type.
// Note that you can use new lines to denote new labels
type Person =
    { Name: string
      Age: int
      Birth: DateTime }

// Creating a new Person is done like this
let p1 = { Name="Charles"; Age=27; Birth=DateTime(1990, 1, 1) }

// Here, we're using new lines to denote the labels as well.
let p2 =
    { Name="Moulaye"
      Age=22
      Birth=DateTime(1995, 1, 1) }

// Records have equality baked in.  No need to define Equals() and GetHashCode().
printfn "Are they equal? %b" (p1 = p2) // This will say 'false', because they are not the same.
```

The other foundational F# type is a [Discriminated Union, or DU](https://docs.microsoft.com/en-us/dotnet/fsharp/language-reference/discriminated-unions).  DUs are a type which could be one of a number of named cases.  These are Sum Types for the category theorists out there.  They can also be recursively-defined, which dramatically simplifies heirarchial data.

```fsharp
// Define a generic binary search tree.
//
// Note that generic type parameters have ' at the beginning.
type BST<'T> =
    | Empty
    | Node of 'T * BST<'T> * BST<'T> // Each node has a left and right BST<'T>

// Flip the BST using pattern matching!
let rec flip bst =
    match bst with
    | Empty -> bst
    | Node(item, left, right) -> Node(item, flip right, flip left)

// Define a sample BST
let tree = 
    Node(4, 
        Node(3, 
            Empty, 
            Node(12, 
                Empty, 
                Empty)),
        Node(55,
            Node(16, 
                Empty,
                Empty),
            Empty))

// Flip it!
printfn "%A" (flip tree)
```

Ta-da!  Armed with the power of Discriminated Unions and F#, you can pass any programming interview which requires you to flip a binary search tree.  Pretty easy to do.

You may have noticed a bit of funky syntax in the `Node` case of the tree definition.  This is actually a type signature for a tuple.  That means that a BST, as we've defined it, can either be empty, or a tuple of `(value, left subtree, right subtree)`.  Read more about [Signatures](https://fsharpforfunandprofit.com/posts/function-signatures/) to learn more.

## Putting it all together: F# Syntax in 60 seconds

The following snippet of code is presented with permission from [Scott Wlaschin](https://twitter.com/ScottWlaschin), an F# community hero who wrote this great overview of F# syntax.  You should be able to read through in about a minute.  It has been edited slightly.

```fsharp
// Single line comments use a double slash.
(* 
    Multi-line comments can be done this way (though double-slash is usually used).
*)

// ======== "Variables" (but not really) ==========
// The "let" keyword defines an (immutable) value
let myInt = 5
let myFloat = 3.14
let myString = "hello"   // note that no types needed

// ======== Lists ============
let twoToFive = [ 2; 3; 4; 5 ]        // Square brackets create a list with
                                     // semicolon delimiters.
let oneToFive = 1 :: twoToFive   // :: creates list with new 1st element
// The result is [1; 2; 3; 4; 5]

let zeroToFive = [0;1] @ twoToFive   // @ concats two lists

// IMPORTANT: commas are never used as delimiters, only semicolons!

// ======== Functions ========
// The "let" keyword also defines a named function.
let square x = x * x          // Note that no parens are used.
square 3                      // Now run the function. Again, no parens.

let add x y = x + y           // don't use add (x,y)! It means something
                              // completely different.
add 2 3                       // Now run the function.

// to define a multiline function, just use indents. No semicolons needed.
let evens list =
   let isEven x = x % 2 = 0     // Define "isEven" as an inner ("nested") function
   List.filter isEven list      // List.filter is a library function
                                // with two parameters: a boolean function
                                // and a list to work on
  
evens oneToFive               // Now run the function

// You can use parens to clarify precedence. In this example,
// do "map" first, with two args, then do "sum" on the result.
// Without the parens, "List.map" would be passed as an arg to List.sum
let sumOfSquaresTo100 =
   List.sum (List.map square [ 1 .. 100 ])

// You can pipe the output of one operation to the next using "|>"
// Here is the same sumOfSquares function written using pipes
let sumOfSquaresTo100piped =
   [ 1 .. 100 ] |> List.map square |> List.sum  // "square" was defined earlier

// you can define lambdas (anonymous functions) using the "fun" keyword
let sumOfSquaresTo100withFun =
   [ 1 .. 100 ] |> List.map (fun x -> x * x) |> List.sum

// In F# returns are implicit -- no "return" needed. A function always
// returns the value of the last expression used.

// ======== Pattern Matching ========
// Match..with.. is a supercharged case/switch statement.
let x = "a"
match x with
| "a" -> printfn "x is a"
| "b" -> printfn "x is b"
| _ -> printfn "x is something else"   // underscore matches anything

// Some(..) and None are roughly analogous to Nullable wrappers
let validValue = Some(99)
let invalidValue = None

// In this example, match..with matches the "Some" and the "None",
// and also unpacks the value in the "Some" at the same time.
let optionPatternMatch input =
   match input with
    | Some i -> printfn "input is an int=%d" i
    | None -> printfn "input is missing"

optionPatternMatch validValue
optionPatternMatch invalidValue

// ========= Complex Data Types =========

// Tuple types are pairs, triples, etc. Tuples use commas.
let twoTuple = (1, 2)
let threeTuple = ("a", 2, true)

// Record types have named fields. Semicolons are separators.
type Person = { First: string; Last: string }

let person1 = { First="John"; Last="Doe" }
// You can also use new lines to elide the semiclon.
let person2 =
    { First="Jane"
      Last="Doe" }

// Union types have choices. Vertical bars are separators.
type Temp = 
    | DegreesC of float
    | DegreesF of float

let temp = DegreesF 98.6

// Types can be combined recursively in complex ways.
// E.g. here is a union type that contains a list of the same type:
type Employee = 
  | Worker of Person
  | Manager of Employee list

let jdoe = { First="John"; Last="Doe" }
let worker = Worker jdoe

// ========= Printing =========
// The printf/printfn functions are similar to the
// Console.Write/WriteLine functions in C#.
printfn "Printing an int %i, a float %f, a bool %b" 1 2.0 true
printfn "A string %s, and something generic %A" "hello" [ 1; 2; 3; 4 ]

// all complex types have pretty printing built in
printfn "twoTuple=%A,\nPerson=%A,\nTemp=%A,\nEmployee=%A" 
         twoTuple person1 temp worker
```

Additionally, there is the [Tour of F#](https://docs.microsoft.com/en-us/dotnet/fsharp/tour) document in our official documentation for .NET and its languages.

## What you can do next

This post covered a lot of things, but it really only began to scratch the surface of F#.  We hope that after reading this post, you'll be able to dive further into F# and functional programming.  Here are a few things we recommend building as a way to learn F# even further:

* Use F# to build [beautiful fractal trees](https://github.com/sfsharp/dojo-fractal-forest)
* Use F# to explore and analyze [data on The Simpsons](https://www.kaggle.com/wcukierski/the-simpsons-by-the-data)
* Use F# and [Suave](https://suave.io/) to build [a web app](https://theimowski.gitbooks.io/suave-music-store/content/en/)
* Use F# to explore [Azure Functions](https://docs.microsoft.com/en-us/azure/azure-functions/functions-reference-fsharp)

There are many, many more things you can use F# for, so the above list is by no means exhaustive.  F# can be used from something as simple as [a build script](https://www.hanselman.com/blog/ExploringFAKEAnFBuildSystemForAllOfNET.aspx) to forming the backend of a [huge, powerful set of backend services](https://tech.jet.com/blog/tag/f/).  There is no shortage of projects you can use F# for.  Give it a shot!

## Additional resources

It's worth noting that there is also a wealth of information online about learning F#, including from a C# or Java background.  The following links will be helpful as you dive deeper into learning F#:

* [F# Guide](https://docs.microsoft.com/en-us/dotnet/fsharp/)
* [F# for Fun and Profit](https://swlaschin.gitbooks.io/fsharpforfunandprofit/content/)
* [F# Wiki](https://en.wikibooks.org/wiki/F_Sharp_Programming)
* [Learn X in Y Minutes: F#](https://learnxinyminutes.com/docs/fsharp/)

Additionally, the F# community is incredibly welcoming for beginners.  There is a very active slack run by the F# Software Foundation, with rooms for beginners, which you can access by [joining for free](http://foundation.fsharp.org/join).  We highly encourage you to do so!