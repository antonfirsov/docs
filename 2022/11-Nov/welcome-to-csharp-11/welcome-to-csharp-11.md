---
post_title: Welcome to C# 11
author1: madst@microsoft.com
post_slug: welcome-to-csharp-11
username: madst@microsoft.com
microsoft_alias: madst
featured_image: csharp11.png
categories: .NET, C#
tags: .net 7, C# 11
summary: C# 11 is here! Bringing with it some highly anticipated features including string literals, generic math, required members, and much more.
desired_publication_date: 2022-11-08
post_date: 2022-11-08 10:05:00
---

I am excited to announce that C# 11 is out! As always, C# opens some entirely new fronts, even while advancing several themes that have been in motion over past releases. There are many features and many details, which are beautifully covered under [What's new in C# 11](https://learn.microsoft.com/dotnet/csharp/whats-new/csharp-11) on our docs pages. What follows here is an appetizer of some of the highlights - small and big.

Before we dive in, let me just say how happy I am about the way this version of C# came into being! With every release there's more and more participation from the community, contributing everything from suggestions, insights and bug reports all the way up to entire feature implementations. This is really everyone's C#. **Thank you!** :heart:

## UTF-8 string literals

By default C# strings are hardcoded to UTF-16, whereas the prevailing string encoding on the internet is UTF-8. To minimize the hassle and performance overhead of converting, you can now simply append a `u8` suffix to your string literals to get them in UTF-8 right away:

``` c#
var u8 = "This is a UTF-8 string!"u8;
```

UTF-8 string literals simply give you back a chunk of bytes - in the form of a `ReadOnlySpan<byte>`. For the scenarios where it's important to have a UTF-8 encoding this is probably more useful than some dedicated new UTF-8 string type.

Read the docs on [UTF-8 string literals](https://learn.microsoft.com/dotnet/csharp/language-reference/builtin-types/reference-types#utf-8-string-literals).

## Raw string literals

A lot of what gets put in string literals is "code" of some sort - not just program text, but also JSON and XML data, HTML, regular expressions, SQL queries, etc. It's really unhelpful when many of the special characters that show up in such text have special meaning in C# string literals! Noteworthy examples include `\` and `"`, joined in interpolated strings by `{` and `}`. Having to escape all of those is a real bummer, and an ongoing source of pain an bugs.

Why not have a form of string literals that has no escape characters at all? That's what raw string literals are. Everything is content!

A raw string literal is delimited by at least three double-quotes:

``` c#
var raw1 = """This\is\all "content"!""";
Console.WriteLine(raw1);
```

This prints:

```cli
This\is\all "content"!
```

If you need three or more `"`s to be part of your content, you just use *more* `"`s on the outside. The beginning and end just have to match:

``` c#
var raw2 = """""I can do ", "", """ or even """" double quotes!""""";
```

This makes it really easy to paste in, maintain and read at a glance what the literal contains.

Multi-line raw string literals can also truncate leading white space: The position of the end quote determines where white space starts to be included in the output:

``` c#
var raw3 = """
    <element attr="content">
      <body>
        This line is indented by 4 spaces.
      </body>
    </element>
    """;
//  ^white space left of here is removed
```

Since there are four spaces to the left of the end quote, four spaces will be removed from the beginning of every line of content, yielding this output:

```html
<element attr="content">
  <body>
    This line is indented by 4 spaces.
  </body>
</element>
```

There's much more to raw string literals than this - for instance they support interpolation! Read more about [raw string literals](https://learn.microsoft.com/dotnet/csharp/programming-guide/strings/#raw-string-literals) in the docs. 

## Abstracting over static members

How do you abstract over operations that are inherently static - such as operators? The traditional answer is "poorly". In C# 11 we released support for static virtual members in interfaces, which was in preview in C# 10. With this you can now define a very simple mathematical interface:

``` c#
public interface IMonoid<TSelf> where TSelf : IMonoid<TSelf>
{
    public static abstract TSelf operator +(TSelf a, TSelf b);
    public static abstract TSelf Zero { get; }
}
```

Notice how the interface takes a type parameter for "itself". That's because static members don't have a `this`. 

Anyone can now implement this interface by providing implementations for the two static members, and passing themselves as the `TSelf` type argument:

``` c#
public struct MyInt : IMonoid<MyInt>
{
    int value;
    public MyInt(int i) => value = i;
    public static MyInt operator +(MyInt a, MyInt b) => new MyInt(a.value + b.value);
    public static MyInt Zero => new MyInt(0);
}
```

Importantly, how do you *consume* these abstract operations? How do you call virtual members when there is no instance to call them on? The answer is via generics. Here is what it looks like:

``` c#
T AddAll<T>(params T[] elements) where T : IMonoid<T>
{
    T result = T.Zero;
    foreach (var element in elements)
    {
        result += element;
    }
    return result;
}
```

The type parameter `T` is constrained by the `IMonoid<T>` interface, and that allows the static virtual members of that interface - `Zero` and `+` - to be called on `T` itself! 

Now we can call the generic method with some `MyInt`s, and the correct implementations of `+` and `Zero` are passed in through the type argument:

``` c#
MyInt sum = AddAll<MyInt>(new MyInt(3), new MyInt(4), new MyInt(5));
```

In fact .NET 7 comes with a new namespace `System.Numerics` chock-full of math interfaces, representing the different combinations of operators and other static members that you'd ever want to use: the "grown-up" versions of the little `IMonoid<T>` interface above. All the numeric types in .NET now implement these new interfaces - and you can add them for your own types too! So it's now easy to write numeric algorithms once and for all - abstracted from the concrete types they work on - instead of having forests of overloads containing essentially the same code.

It's also worth noting that static virtual members are useful for other things than math. For instance you can abstract over factory methods for a hierarchy of types. But we've covered enough for now - you might want to check out these tutorials in docs on [static abstract interface methods](https://learn.microsoft.com/dotnet/csharp/whats-new/tutorials/static-virtual-interface-members#static-abstract-interface-methods) and [generic math](https://learn.microsoft.com/dotnet/csharp/whats-new/tutorials/static-virtual-interface-members#generic-math).

Even if you do not create interfaces with static virtual members, you benefit from the improvements they make to .NET libraries, now and in the future.

## List patterns

Pattern matching is one of the ongoing stories in C# that we just keep filling out. Pattern matching was introduced in C# 7 and since then it has grown to become one of the most important and powerful control structures in the language.

C# 11 adds *list patterns* to the story. With list patterns you can apply patterns recursively to the individual elements of list-like input - or to a range of them. Let's jump right in with the generic algorithm from above, rewritten as a recursive method using list patterns:

``` c#
T AddAll<T>(params T[] elements) where T : IMonoid<T> => 
    elements switch
{
    [] => T.Zero,
    [var first, ..var rest] => first + AddAll<T>(rest),
};
```

There's a lot going on, but at the center is a switch expression with two cases. One case returns zero for an empty list `[]`, where `Zero` is defined by the interface. The other case extracts the first element into `first` with the `var first` pattern, and the remainder is extracted into `rest` using the `..` to slice out all the remaining elements into the `var rest` pattern.

Read more about [list patterns](https://learn.microsoft.com/dotnet/csharp/language-reference/operators/patterns#list-patterns) in the docs.

## Required members

Another ongoing theme that we've been working on for several releases is improving object creation and initialization.  C# 11 continues these improvements with *required members*.

When creating types that used object initializers, you used to be unable to specify that some properties must be initialized. Now, you can say that a property or field is `required`. This means that it *must* be initialized by an object initializer when an object of the type is created:

``` c#
public class Person
{
    public required string FirstName { get; init; }
    public string? MiddleName { get; init; }
    public required string LastName { get; init; }
}
```

It is now an error to create a `Person` without initializing both the required properties:

``` c#
var person = new Person { FirstName = "Ada" }; // Error: no LastName!
```

Check the docs for more on [required members](https://learn.microsoft.com/dotnet/csharp/language-reference/keywords/required).

## In closing ...

C# 11 also includes many other features. I hope this appetizer excites you to explore [What's new in C# 11](https://learn.microsoft.com/dotnet/csharp/whats-new/csharp-11) and that you have as much fun coding with C# 11 as we had making it! We strive to make the language ever more useful, not only by adding more expressive power (as with static virtual members) but also by simplifying, streamlining and removing boilerplate (as with e.g. raw string literals and list patterns) and making it safer (as with required members).
