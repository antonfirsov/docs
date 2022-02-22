---
post_title: Early peek at C# 11 features 
username: kathleen.a.dollard
microsoft_alias: kdollard
featured_image: csharp-image.png
categories: .NET, .NET Core, C#, Visual Studio
summary: Learn about the C# 11 preview features you can try out in Visual Studio 17.1 and .NET SDK 6.0.200
desired_publication_date: 2022-02-22
---

Visual Studio 17.1 (Visual Studio 2022 Update 1) and .NET SDK 6.0.200 include preview features for C# 11! You can update [Visual Studio](https://visualstudio.microsoft.com/vs/) or download the [latest .NET SDK](https://dotnet.microsoft.com/download) to get these features.  

Check out the post [Visual Studio 2022 17.1 is now available!](https://devblogs.microsoft.com/visualstudio/visual-studio-2022-17-1-is-now-available/) to find out what’s new in Visual Studio and the post [Announcing .NET 7 Preview 1](https://devblogs.microsoft.com/dotnet/announcing-net-7-preview-1/) to learn about more .NET 7 preview features.

## Designing C# 11

We love designing and developing in the open! You can find proposals for future C# features and notes from language design meetings in the [CSharpLang repo](https://github.com/dotnet/csharplang). The main page explains our design process and you can listen to [Mads Torgersen on the .NET Community Runtime and Languages Standup](https://www.youtube.com/watch?v=lzyWFew_w8Y&t=362s) where he talks about the design process.  

Once work for a feature is planned, work and tracking shifts to the Roslyn repo. You can find the status of upcoming features on the [Feature Status page](https://github.com/dotnet/roslyn/blob/main/docs/Language%20Feature%20Status.md). You can see what we are working on and what's merged into each preview. You can also look back at previous versions to check out features you may have overlooked. 

For this post I've distilled these sometimes complex and technical discussions to what each feature means in your code.

We hope you will try out these new preview features and [let us know what you think](https://github.com/dotnet/csharplang/discussions). To try out the C# 11 preview features, create a C# project and set the `LangVersion` to `Preview`. Your `.csproj` file might look like:

```xml
<Project Sdk="Microsoft.NET.Sdk">
	<PropertyGroup>
		<OutputType>Exe</OutputType>
		<TargetFramework>net6.0</TargetFramework>
		<ImplicitUsings>enable</ImplicitUsings>
		<Nullable>enable</Nullable>
		<LangVersion>preview</LangVersion>
	</PropertyGroup>
</Project>
```

## C# 11 Preview: Allow newlines in the “holes” of interpolated strings

Read more about this change in the proposal [Remove restriction that interpolations within a non-verbatim interpolated string cannot contain new-lines. #4935](https://github.com/dotnet/csharplang/issues/4935)

C# supports two styles of [interpolated strings](https://docs.microsoft.com/dotnet/csharp/language-reference/tokens/interpolated): verbatim and non-verbatim interpolated strings (`$@""` and `$""` respectively). A key difference between these is that a non-verbatim interpolated strings cannot contain newlines in its text segments, and must instead use escapes (like \r\n). A verbatim interpolated string can contain newlines in its text segments, and doesn’t escape newlines or other character (except for “” to escape a quote itself).
All of this behavior remains the same.

Previously, these restrictions extended to the holes of non-verbatim interpolated strings. _Holes_ is a shorthand way of saying _interpolation expressions_ and are the portions inside the curly braces that supply runtime values. The holes themselves are not text, and shouldn't be held to the escaping/newline rules of the interpolated string text segments.

For example, the following would have resulted in a compiler error in C# 10 and is legal in this C# 11 preview:

```c#
var v = $"Count is\t: { this.Is.Really.Something()
                            .That.I.Should(
                                be + able)[
                                    to.Wrap()] }.";
```

## C# 11 Preview: List patterns

Read more about this change in the proposal [List patterns](https://github.com/dotnet/csharplang/blob/main/proposals/list-patterns.md).

The new _list pattern_ allows you to match against lists and arrays. You can match elements and optionally include a _slice pattern_ that matches zero or more elements. Using slice patterns you can discard or capture zero or more elements. 

The syntax for list patterns are values surrounded by square brackets and for the slice pattern it is two dots. The slice pattern can be followed by another list pattern, such as the `var` pattern to capture the contents of the slice.

The pattern `[1, 2, .., 10]` matches all of the following:

```c#
int[] arr1 = { 1, 2, 10 };
int[] arr1 = { 1, 2, 5, 10 };
int[] arr1 = { 1, 2, 5, 6, 7, 8, 9, 10 };
```

To explore list patterns consider:

```c#
public static int CheckSwitch(int[] values)
    => values switch
    {
        [1, 2, .., 10] => 1,
        [1, 2] => 2,
        [1, _] => 3,
        [1, ..] => 4,
        [..] => 50
    };
```

When it is passed the following arrays, the results are as indicated:

```c#
WriteLine(CheckSwitch(new[] { 1, 2, 10 }));          // prints 1
WriteLine(CheckSwitch(new[] { 1, 2, 7, 3, 3, 10 })); // prints 1
WriteLine(CheckSwitch(new[] { 1, 2 }));              // prints 2
WriteLine(CheckSwitch(new[] { 1, 3 }));              // prints 3
WriteLine(CheckSwitch(new[] { 1, 3, 5 }));           // prints 4
WriteLine(CheckSwitch(new[] { 2, 5, 6, 7 }));        // prints 50
```

You can also capture the results of a slice pattern:

```c#
public static string CaptureSlice(int[] values)
    => values switch
    {
        [1, .. var middle, _] => $"Middle {String.Join(", ", middle)}",
        [.. var all] => $"All {String.Join(", ", all)}"
    };
```

List patterns work with any type that is countable and indexable — which means it has an accessible `Length` or `Count` property and with an indexer an `int` or `System.Index` parameter. Slice patterns work with any type that is countable and sliceable — which means it has an accessible indexer that takes a `Range` as an argument or has an accessible `Slice` method with two `int` parameters. 

We're considering adding support for list patterns on `IEnumerable` types. If you have a chance to play with this feature, let us know your thoughts on it.

## C# 11 Preview: Parameter null-checking

Read more about this change in the proposal [Parameter null checking](https://github.com/dotnet/csharplang/blob/main/proposals/param-nullchecking.md).	

We are putting this feature into this early preview to ensure we have time to get feedback. There have been discussions on a very succinct syntax vs. a more verbose one. We want to get customer feedback and from users that have had a chance to experiment with this feature.

It is quite common to validate whether method arguments are null with variations of boilerplate code like:

```c#
public static void M(string s)
{
    if (s is null)
    {
        throw new ArgumentNullException(nameof(s));
    }
    // Body of the method
}
```

With Parameter null checking, you can abbreviate your intent by adding `!!` to the parameter name:

```c#
public static void M(string s!!)
{
    // Body of the method
}
```

Code will be generated to perform the null check. The generated null check will execute before any of the code within the method. For constructors, the null check occurs before field initialization, calls to `base` constructors, and calls to `this` constructors.

This features is independent of Nullable Reference Types (NRT), although they work well together. NRT helps you know at design time whether a null is possible. Parameter null-checking makes it easier to check at runtime whether nulls have been passed to your code. This is particularly important when your code is interacting with external code that might not have NRT enabled.

The check is  equivalent `if (param is null) throw new ArgumentNullException(...)`. When multiple parameters contain the `!!` operator then the checks will occur in the same order as the parameters are declared. 

There are a few guidelines limiting where `!!` can be used:

* Null-checks can only be applied to parameters when there is an implementation. For example, an abstract method parameter cannot use `!!`. Other cases where it cannot be used include:
  * `extern` method parameters.
  * Delegate parameters.
  * Interface method parameters when the method is not a Default Interface Method (DIM).
* Null checking can only be applied to parameters that can be checked. 

An example of scenarios that are excluded based on the second rule are discards and `out` parameters. Null-checking can be done on `ref` and `in` parameters. 

Null-checking is allowed on indexer parameters, and the check is added to the `get` and `set` accessor. For example:

```c#
public string this[string key!!] { get { ... } set { ... } }
```

Null-checks can be used on lambda parameters, whether or not they are surrounded by parentheses:

```c#
// An identity lambda which throws on a null input
Func<string, string> s = x!! => x;
```

`async` methods can have null-checked parameters. The null check occurs when the method is invoked. 

The syntax is also valid on parameters to iterator methods. The null-check will occur when the iterator method is invoked, not when the underlying enumerator is walked. This is true for traditional or `async` iterators:

```c#
class Iterators {
    IEnumerable<char> GetCharacters(string s!!) {
        foreach (var c in s) {
            yield return c;
        }
    }

    void Use() {
        // The invocation of GetCharacters will throw
        IEnumerable<char> e = GetCharacters(null);
    }
}
```	
	
### Interaction with Nullable Reference Types

Any parameter which has a `!!` operator applied to its name will start with the nullable state being not-null. This is true even if the type of the parameter itself is potentially null. That can occur with an explicitly nullable type, such as say `string?`, or with an unconstrained type parameter.

When `!!` syntax on parameters is combined with an explicitly nullable type on the parameter, the compiler will issue a warning:

```c#
void WarnCase<T>(
    string? name!!,     // CS8995	Nullable type 'string?' is null-checked and will throw if null. 
    T value1!!        // Okay
)
```

### Constructors

There is a small, but observable change when you change from explicit null-checks in your code to null-checks using the null validation syntax (`!!`). Your explicit validation occurs after field initializers, base class constructors, and constructors called using `this`. Null-checks performed with the parameter null-check syntax will occur before any of these execute. Early testers found this order to be helpful and we think it will be very rare that this difference will adversely affect code. But check that it will not impact your program before shifting from explicit null-checks to the new syntax.

### Notes on design

You can hear [Jared Parsons in the .NET Languages and Runtime Community Standup](https://youtu.be/Fz4hViH5bGc?t=2893) on Feb. 9th, 2022. This clip starts about 45 minutes into the stream when Jared joins us to talk more about the decisions made to get this feature into preview, and responds to some of the common feedback.

Some folks learned about this feature when they saw PRs using this feature in the .NET Runtime. Other teams at Microsoft provide important dogfooding feedback on C#. It was exciting to learn that the .NET Runtime removed nearly 20,000 lines of code using this new null-check syntax.

The syntax is `!!` on the parameter name. It is on the name, not the type, because this is a feature of how that specific parameter will be treated in your code. We decided against attributes because of how it would impact code readability and because attributes very rarely impact how your program executes in the way this feature does.

We considered and rejected making a global setting that there would be null-checks on all nullable parameters. Parameter null checking forces a design choice about how null will be handled. There are many methods where a null argument is a valid value. Doing this everywhere a type is not null would be excessive and have a performance impact. It would be extremely difficult to limit only to methods that were vulnerable to nulls (such as public interfaces). We also know from the .NET Runtime work that there are many places the check is not appropriate, so a per parameter opt-out mechanism would be needed. We do not currently think that a global approach to runtime null checks is likely to be appropriate, and if we ever consider a global approach, it would be a different feature.

## Summary

Visual Studio 17.1 and .NET SDK 6.0.200 offer an early peek into C# 11. You can play with parameter null-checking, list patterns, and new lines within curly braces (the holes) of interpolated strings. 

We hope you’ll check out the C# 11 Preview features by updating [Visual Studio](https://visualstudio.microsoft.com/vs/) or downloading the [latest .NET SDK](https://dotnet.microsoft.com/download), and then setting the `LangVersion` to `preview`.

We look forward to hearing what you think, here or via [discussions in the CSharpLang](https://github.com/dotnet/csharplang/discussions)!
