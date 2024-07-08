---
post_title: 'C# 13: Explore the latest preview features'
author1: kathleen.a.dollard
post_slug: csharp-13-explore-preview-features
microsoft_alias: kdollard
featured_image: csharp-13-preview-features.jpg
categories: C#
tags: .net 9, c# 13, featured-preview, csharp
ai_note: hide
summary: C# 13 focuses on flexibility and performance, with top features like params collections for added flexibility, lock object for improved performance, and partial properties to support generators.
post_date: 2024-07-09 10:05:00
---

C# 13 is starting to take shape with features that focus on flexibility, performance, and  making your favorite features even better for everyday use. We build C# [in the open](https://github.com/dotnet/roslyn/blob/main/docs/Language%20Feature%20Status.md) and at this year's [Microsoft Build](https://aka.ms/build2024-csharp) we gave you a peek into what was coming in C# 13. Today, we want to share the current status of what you can try today in C# 13 and provide updates on features planned for this release and beyond. Let's take a look at these new features in more detail.

* [`params` collection enhancements for greater flexibility](#params-collections)
* [`lock` object](#lock-object)
* [index operator improvements](#index-from-the-end-in-initializers)
* [Escape sequence `\e`](#escape-sequence-e)
* [Partial properties](#partial-properties)
* [Method group natural type improvements](#method-group-natural-type-improvements)
* [`ref` and `unsafe` in `async` methods and iterators](#ref-and-unsafe-in-async-methods-and-iterators)

There's also an [update on Extension Types](#update-on-extension-types).
## Try C# 13 today
Before we dive into each new features of C# 13 you may be wondering how do you try it out.

You can find the latest previews of C# 13 in latest [.NET 9 preview](https://dotnet.microsoft.com/download/dotnet/9.0) (Preview 6 at the time of writing) and in the latest preview of [Visual Studio 2022-17.11](https://visualstudio.microsoft.com/vs/preview/). To access preview features, set your language version to `preview` in your project file:

```xml
<Project Sdk="Microsoft.NET.Sdk">
   <PropertyGroup>
      <!--other settings-->
      <LangVersion>preview</LangVersion>
      <!--other settings-->
   </PropertyGroup>
</Project>
```

## `params` collections

C# 13 extends `params` to work with any type that can be constructed via a collection expression. This adds flexibility whether you are writing a method or calling it.

When the `params` keyword appears before a parameter, calls to the method can provide a comma delimited list of zero or more values. The following works in all versions of C#:

```csharp
public void WriteNames(params string[] names)
   => Console.WriteLine(String.Join(", ", names));

WriteNames("Mads", "Dustin", "Kathleen");
WriteNames(new string[] {"Mads", "Dustin", "Kathleen"});
// Both of these Would output: Mads, Dustin, Kathleen
```

Note that you can call the method with either a comma delimited list of values, or an object of the underlying type.

Starting in C# 13 `params` parameters can be of any of the types supported for collection expressions. For example:

```csharp
public void WriteNames(params List<string> names)
   => Console.WriteLine(String.Join(", ", names));
```

Whenever you call a method that has a parameter that is an `IEnumerable<T>`, you can pass the results of a LINQ expression. If the `IEnumerable<T>` parameter has the `params` modifier, you can also pass a comma delimited list. You can use a comma delimited list when you have constants and a LINQ expression when you need it:

```csharp
public void WriteNames(params IEnumerable<string> names)
   => Console.WriteLine(String.Join(", ", names));

var persons = new List<Person>
{
   new Person("Mads", "Torgersen"),
   new Person("Dustin", "Campbell"),
   new Person("Kathleen", "Dollard")
};

// All of the following output: Mads, Dustin, Kathleen
WriteNames("Mads", "Dustin", "Kathleen");
WriteNames(persons.Select(person => person.FirstName));
WriteNames(from p in persons select p.FirstName);
```

### Overload resolution

When authoring a method, you can supply multiple `params` overloads. For example, adding an `IEnumerable<T>` overload supports LINQ and adding a `ReadOnlySpan<T>` or `Span<T>` overload reduces allocations, which can improve performance.

```csharp
public void WriteNames(params string[] names)
   => Console.WriteLine(String.Join(", ", names));

public void WriteNames(params ReadOnlySpan<string> names)
   => Console.WriteLine(String.Join(", ", names));

public void WriteNames(params IEnumerable<string> names)
   => Console.WriteLine(String.Join(", ", names));
```

When one of the specified types is passed, that overload is used. When comma delimited values or no values, are passed, the best overload is selected. Using the overloads above:

```csharp
// IEnumerable overload is used
WriteNames(persons.Select(person => person.FirstName)); 
 
// array overload is used
WriteNames(new string[] {"Mads", "Dustin", "Kathleen"}); 

// most efficient overload is used: currently ReadOnlySpan
WriteNames("Mads", "Dustin", "Kathleen");                
```

Multiple overloads can add convenience and improve performance. Library authors should give all overloads the same semantics so that callers don't need to be concerned about which overload is used.

## `lock` object

.NET 9 includes a new `System.Threading.Lock` type for mutual exclusion that can be more efficient than locking on an arbitrary `System.Object` instance. The [`System.Threading.Lock` type proposal](https://github.com/dotnet/runtime/issues/34812) has more about this type and why it was created. Over time, this type is expected to become the primary mechanism used for most locking in C# code.

C# 13 makes it easy to use this type. When the compiler recognizes that the target of the `lock` statement is a `System.Threading.Lock` object, C# now generates calls to the `System.Threading.Lock` API and provides warnings for cases where an instance of a `Lock` might be incorrectly treated as a normal `object`.

This update means the familiar syntax for the lock statement leverages new features in the runtime. Familiar code gets better with minimal change. Just change your project's `TargetFramework` to .NET 9 and change the type of the lock from object to `System.Threading.Lock`:

```csharp
public class MyClass 
{
    private object myLock = new object();

    public void MyMethod() 
    {
        lock (myLock)
        {
           // Your code
        }          
    }
}

public class MyClass 
{
    // The following line is the only change
    private System.Threading.Lock myLock = new System.Threading.Lock();
    
    public void MyMethod() 
    {
        lock (myLock)
        {
            // Your code
        }     
    }
}
```

## Index from the end in initializers

The index operator `^` allows you to indicate a position in a countable collection relative to the end of the collection. This now works in initializers:

```csharp
class Program
{ 
    static void Main()
    {
        var x = new Numbers
        {
            Values = 
            {
                [1] = 111,
                [^1] = 999  // Works starting in C# 13
            }
            // x.Values[1] is 111
            // x.Values[9] is 999, since it is the last element
        };
    }
}

class Numbers
{
    public int[] Values { get; set; } = new int[10];
} 
```

## Escape sequence `\e`

C# 13 introduces a new escape sequence for the character you know as `ESCAPE` or `ESC`. You previously had to type this as a variation of `\u001b`. This new sequence is especially convenient when interacting with terminals with the VT100/ANSI escape codes to `System.Console`. For example:

```csharp
// Prior to C# 13
Console.WriteLine("\u001b[1mThis is a bold text\u001b[0m");

// With C# 13
Console.WriteLine("\e[1mThis is a bold text\e[0m");
```

This makes creating fancy terminal output easier and less prone to errors.

## Partial properties

C# 13 adds partial properties. Like partial methods their primary purpose is to support source generators. Partial methods have been available for many releases with additional improvements in C# 9. Partial properties are much like their partial method counterparts.

For example, starting with .NET 7 (C# 12), the regular expression source generator creates efficient code for methods:

```csharp
[GeneratedRegex("abc|def")]
private static partial Regex AbcOrDefMethod();

if (AbcOrDefMethod().IsMatch(text))
{
   // Take action with matching text
}
```

In .NET 9 (C# 13), the Regex source generator has been updated and if you prefer to use a property, you can also use:

```csharp
[GeneratedRegex("abc|def")]
private static partial Regex AbcOrDefProperty { get; };

if (AbcOrDefProperty.IsMatch(text))
{
   // Take action with matching text
}
```

Partial properties will make it easier for source generator designers to create natural feeling APIs.

## Method group natural type improvements

The _natural type_ of an expression is the type determined by the compiler, such as when the type is assigned to `var` or `Delegate`. That's straightforward when it's a simple type. In C# 10 we added support for method groups. Method groups are used when you include the name of a method without parentheses as a delegate:

```csharp
Todo GetTodo() => new(Id: 0, Name: "Name");
var f = GetTodo; // the type of f is Func<ToDo>
```

C# 13 refines the rules for determining the natural type to consider candidates by scope and to prune candidates that have no chance of succeeding. Updating these rules will mean less compiler errors when working with method groups.

## `allows ref struct`

C# 13 adds a new way to specify capabilities for generic type parameters. By default, type parameters cannot be `ref struct`. C# 13 lets you specify that a type parameter can be a `ref struct`, and applies the appropriate rules. While other generic constraints limit the set of types that can be used as the type parameter, this new specification expands the allowed types. We think of this as an _anti-constraint_ since it removes rather than adds a restriction. The syntax `allows ref struct` in the `where` clause, where `allows` indicates this expansion in usage:

```csharp
T Identity<T>(T p)
    where T : allows ref struct
    => p;

// Okay
Span<int> local = Identity(new Span<int>(new int[10]));
```

A type parameter specified with `allows ref struct` has all of the [behaviors and restrictions of a `ref struct` type](https://learn.microsoft.com/dotnet/csharp/language-reference/builtin-types/ref-struct).

## `ref` and `unsafe` in `async` methods and iterators

Prior to C# 13, iterator methods (methods that use `yield return`) and `async` methods couldn't declare local `ref` variables, nor could they have an `unsafe` context.

In C# 13, `async` methods can declare `ref` local variables, or local variables of a `ref struct` type. These variables can't be preserved across an await boundary or a yield return boundary.

In the same fashion, C# 13 allows `unsafe` contexts in iterator methods. However, all `yield return` and `await` statements must be in safe contexts.
These relaxed restrictions let you use `ref` local variables and `ref struct` types in more places. 

## Update on Extension Types

We are very excited about the Extension Types feature that [Mads and Dustin showed at Build](https://aka.ms/build2024-csharp). We also described Extension Types in the blog post [.NET announcements at Build](https://devblogs.microsoft.com/dotnet/dotnet-build-2024-announcements). At the time, we were aiming for key parts of the feature to be in C# 13, but the design and implementation are going to take more time. Look for Extension Types in early C# 14 (NET 10) previews.

## Summary

You can find more on these features at [What's new in C# 13](https://learn.microsoft.com/dotnet/csharp/whats-new/csharp-13). We're still working on features for C# 13, and you can check out what we're doing at the [Roslyn Feature Status page](https://github.com/dotnet/roslyn/blob/main/docs/Language%20Feature%20Status.md). Also, be sure to follow the [.NET 9 preview release notes](https://github.com/dotnet/core/tree/main/release-notes/9.0) where you can now find C# release notes for each release.

Download the latest preview of [Visual Studio 2022-17.11](https://visualstudio.microsoft.com/vs/preview/) with .NET 9, check out these new features, and let us know what you think!
