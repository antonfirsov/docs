---
post_title: 'Automatically find latent bugs in your code with .NET 5'
username: jmarolf@microsoft.com
summary: Introduction to the new Analysis Level feature shipping in .NET 5 Preview 8.
---

In the past, we’ve been reluctant to add new warnings to C#. This is because adding new warnings is technically a breaking change for users who have warnings set as errors. However, there are a lot of cases we’ve come across over the years where we also _really_ want to warn people that something was wrong, ranging from common coding mistakes to common API misuse patterns.

Starting with .NET 5, we're introducing what we're calling "Analysis Levels" in the C# compiler to introduce warnings for these patterns in a safe way. The default Analysis Level for all projects targeting .NET 5 will be at the highest, meaning that more warnings (and suggestions to fix them) will be introduced.

## What are Analysis Levels?

TODO - explain the mechanism
TODO - give example of manually setting it in a project file

Unless specified explicitly, the default Analysis Level is tied to your target framework:

| Target Framework | Analysis Level |
|---------------------|-----------------|
| `netcoreapp3.1` or lower | ??? |
| `net5.0` or higher | 5 |
| `netstandard2.1 or lower | ??? |


Analysis Levels are tied to the target framework of your project so until you change what your code targets you will never change your default analysis level. You can also manually set your analysis level per project if you want (that’s discussed in more detail at the end).
Since all .NET 5 projects will be opted into _Analysis Level 5_, let’s take a look at some of the new warnings and suggestions that will be offered:


## Warnings for common mistakes

The first set of new warnings that we are going to talk about are intended to find latent bugs in your existing code. I can say from experience that without the compiler helping you out these can be very difficult to avoid introducing into your codebase.

### Warn when expression is always true or false

The first new warning that we are going to talk about has been a long time coming. Consider the following code:

```csharp
public void M(DateTime dateTime)
{
    if (dateTime == null) // warning CS8073
    {
        return;
    }
}
```

Is it obvious what the problem is? Admittedly this method is rather sparse on the details so to some it may stand out rather starkly: `DateTime` is a `struct` and `struct`s cannot be null, yet we check anyways. Starting in .NET 5 we will warn about this case with `CS80731`. Warning message will be:

```
Warning CS8073: The result of the expression is always ‘false’ since the value of type ‘DateTime’ is never equal to ‘null’ of type ‘DateTime?’
```

It might seem rather obvious what this code is doing is unnecessary in isolation but it can be hard to spot issues like this in codebases even though we are all good (?) programmers. To fix this you can remove the code (since its always false it’s not doing anything anyways) or change its type to `DateTime?`

```csharp
public void M(DateTime? dateTime) // We accept a null DateTime
{
    if (dateTime == null) // No Warnings
    {
        return;
    }
}
```

### Do not use ReferenceEquals with value types

Equality can be a tricky topic in .NET. This next warning strives to make accidentally comparing a `struct` by reference obvious. Consider the code below:

```csharp
int int1 = 1, int2 = 1;
Console.WriteLine(object.ReferenceEquals(int1, int2)); // warning CA2013
```

This will box the two ints and `ReferenceEquals` will always return false as a result. We will see this warning description:

```
Warning CA2013: Do not pass an argument with value type 'int' to 'ReferenceEquals'. Due to value boxing, this call to 'ReferenceEquals' will always return 'false'.
```

The fix for this error is to either use the equality operator `==` or `object.Equals` like so:

```csharp
int int1 = 1, int2 = 1;
Console.WriteLine(int1 == int2); // using the equality operator is fine
Console.WriteLine(object.Equals(int1, int2));  // so is object.Equals
```

### Track definite assignment of structs across assemblies

This next warning is something that a lot of people may be surprised to learn wasn't already a warning:

```csharp
using System.Collections.Immutable;

class P
{
    public void M(out ImmutableArray<int> immutableArray) // CS0177
    {
    }
}
```

This rule is all about [definite assignment](https://docs.microsoft.com/dotnet/csharp/language-reference/language-specification/variables#definite-assignment), a useful feature in C# that makes sure you don't forget to assign values to your variables.

```
Error CS0177: The out parameter 'immutableArray' must be assigned to before control leaves the current method
```

`CS0177` is already issued for several different situations today so why didn't we issue a warning for this obviously wrong case in the past? The history here is that this was a bug that traces itself all the way back to the original implementations of the C# compiler. Previously, the compiler ignored private fields of reference types in a value type imported from metadata when computing definite assignment. This very specific bug meant that a type like `ImmutableArray<T>` was able to escape definite assignment analysis. But no longer! Now the compiler will correctly warn you and you can fix it by simply ensuring that it is always assigned a value, like so:

```csharp
using System.Collections.Immutable;

class P
{
    public bool M(out ImmutableArray<int> immutableArray) // no warning
    {
        immutableArray = ImmutableArray<int>.Empty;
    }
}
```

## Correctly Using APIs

These next two warnings are about correctly using .NET libraries. While some of these apis might not be used by everyone today this is an important step in ensuring that the .NET team can release apis that we previously couldn’t because there was no way to tell the user that they were using the api wrong in one very specific case.

### Do not define finalizers for types derived from MemoryManager

`MemoryManager` is a useful class for when you want to implement your own `Memory<T>` type. Hopefully, this is not something most people find themselves doing a lot, but when you need it you really need it. This new warning triggers for cases like this:

```csharp
class DerivedClass <T> : MemoryManager<T>
{
    public override bool Dispose(bool disposing)
    {
        if (disposing)
        {
            _handle.Dispose();
        }
    }
  
    ~DerivedClass() => Dispose(false); // warning CA2015
}
```

Turns out adding a finalizer to this type can introduce GC holes, bad terrible things as it were. Instead you now get the warning, so you don’t do this accidentally.

```
Warning CA2015 Adding a finalizer to a type derived from MemoryManager<T> may permit memory to be freed while it is still in use by a Span<T>.
```

The fix is to remove this destructor as it will cause very subtle bugs in your program that will be hard to find and fix.

```csharp
class DerivedClass <T> : MemoryManager<T>
{
    public override bool Dispose(bool disposing)
    {
        if (disposing)
        {
            _handle.Dispose();
        }
    }
 // no warning
}
```

### Argument passed to TaskCompletionSource constructor should be TaskCreationOptions enum instead of TaskContinuationOptions

For the final entry in this category:  a warning that notifies us that we’ve used just _slightly_ the wrong enum.

```csharp
var tcs = new TaskCompletionSource<int>(TaskContinuationOptions.RunContinuationsAsynchronously); // warning CA2247
```

Unless you are already aware of the issue you may stare at this for a bit before you see it. The problem is that this constructor does not take a `TaskContinuationOptions` enum it takes a `TaskCreationOptions` enum. Considering how similar their names are and that they have very similar values this mistake is easy to make.

```
Warning CA2247: Argument contains TaskContinuationsOptions enum instead of TaskCreationOptions enum.
```

The fix is to pass in the correct enum type:
```csharp
var tcs = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously); // no warning
```

## Low level coding help

There are also a few warnings that are useful when writing high-performance applications. These next set of warnings ensure you don’t need to sacrifice safety for these cases.

### Do not use OutAttribute on string parameters for P/Invokes

In the course of human events you sometimes need to inter-operate with native code. .Net has the concept of platform invocations (P/Invokes) to make this process easier. However, there are a few gotchas in terms of sending data to and from native libraries in .NET.  consider the code below:

```csharp
[DllImport("MyLibrary")]
private static extern void Goo([Out] string s); // warning CA1417
```

Unless you are very familiar with writing P/Invokes its not obvious what is wrong here. You normally apply OutAttribute’s to types that the runtime doesn’t know about to indicate how the type should be marshaled. The OutAttribute implies that you are passing the data by value. It doesn’t make sense for strings to be passed by value and has the potential to destabilize the runtime.

```
Warning CA1417 Do not use the 'OutAttribute' for string parameter 's' which is passed by value. If marshalling of modified data back to the caller is required, use the 'out' keyword to pass the string by reference instead.
```

The fix for this is to either treat it as a normal out parameter (passing by reference).

```csharp
[DllImport("MyLibrary")]
private static extern void Goo(out string s); // no warning
```

or if you don’t need the string marshaled back to the caller you can just do this:

```csharp
[DllImport("MyLibrary")]
private static extern void Goo(string s); // no warning
```

### Use AsSpan instead of Range-based indexers for string when appropriate

This is all about making sure that you don’t accidentally allocate a string. 

```csharp
class Program
{
    public void TestMethod(string str)
    {
        ReadOnlySpan<char> slice = str[1..3]; // CA1831
    }
}
```

In the code above its clear the developers intent is to index a string using the new [range-based index](https://docs.microsoft.com/dotnet/csharp/language-reference/proposals/csharp-8.0/ranges) feature in C#. Unfortunately, this will actually allocate a string unless you convert that string to a span first. 

```
Warning CA1831 Use 'AsSpan' instead of the 'System.Range'-based indexer on 'string' to avoid creating unnecessary data copies
```

The fix is to just add AsSpan calls in this case:

```csharp
class Program
{
    public void TestMethod(string str)
    {
        ReadOnlySpan<char> slice = str.AsSpan()[1..3]; // no warning
    }
}
```

### Do not use stackalloc in loops

The [`stackalloc`](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/stackalloc) keyword is great for when you want to make sure the operations you are doing are easy on the garbage collector. In the past `stackalloc` was only allowed in unsafe code, but beginning in C# 8 its allowed as long as that variable is assigned to a `Span<T>` or a `ReadOnlySpan<T>`.
```csharp
class C
{
    public void TestMethod(string str)
    {
        int length = 3;
        for (int i = 0; i < length; i++)
        {
            Span<int> numbers = stackalloc int[length]; // CA2014
            numbers[i] = i;
        }
    }
}
```

Allocating a lot on the stack can lead to the famous stack overflow exception where we’ve allocated more memory on the stack than allowed. Allocating in a loop is especially perilous.

```
Warning CA2014 Potential stack overflow. Move the stackalloc out of the loop.
```

The fix is to move our `stackalloc` out of the loop.
```csharp
class C
{
    public void TestMethod(string str)
    {
        int length = 3;
        Span<int> numbers = stackalloc int[length]; // no warning
        for (int i = 0; i < length; i++)
        {
            numbers[i] = i;
        }
    }
}
```

## Configuring Analysis Levels

Now that you’ve seen how useful these warnings are you probably never want to go back to a world without them right? Well I know that the world doesn’t always work that way. As we’ve talked about these are breaking changes and you should be able to take them on in a schedule that makes sense to you. Part of the reason we want to get this new experience into Preview 8 is to get feedback from you on whether this (small) set of warnings is too disruptive.

### Going back to the .NET Core 3.1 analysis level:

If you just want to go back to the way things were before .NET 5 (meaning the warnings you got in .NET Core 3.1) all you need to do is set the analysis level to 4 in your project file. Here is an example:

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net5.0</TargetFramework>
    <!-- get the exact same warnings you used to -->
    <AnalysisLevel>4</AnalysisLevel>
  </PropertyGroup>

</Project>
```

### Turning off just a single rule

If there is a specific warning that you believe is not applicable to your codebase you can use an editorconfig file to turn it off for your codebase. You can do this by either setting the severity of the warning to ‘none’ from the error list.

![Set Severity From Error List](SetSeverityFromErrorList.png)

Or by selecting “none” from the lightbulb menu where the warning appears in the editor

![Set Severity From Lightbulb](SetSeverityFromLightbulb.png)
 
### Turning off a single instance of a warning

If you want a warning to be on almost all the time and only suppress it in a few instances you can use the lightbulb menu to either:

- Suppress it in source.

![Suppress in Source](SuppressInSource.png)
 
- Suppress it in a separate suppression file.
  
![Suppress in Suppression File](SuppressInSuppressionFile.png)
 
- Suppress it in source with an attribute.
 
![Suppress in Attribute](SuppressInAttribute.png)

I hope this has gotten you excited for all the improvements to code analysis that you can expect in .NET 5 and please give us feedback about this experience.
