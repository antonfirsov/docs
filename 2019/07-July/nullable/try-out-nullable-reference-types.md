# Try out Nullable Reference Types

With the release of .NET Core 3.0 Preview 7, C# 8.0 is considered "feature complete". That means that the biggest feature of them all, [Nullable Reference Types](https://docs.microsoft.com/dotnet/csharp/nullable-references), is also locked down behavior-wise for the .NET Core release. It will continue to improve after C# 8.0, but it is now considered stable with the rest of C# 8.0.

At this time, our aim is to collect as much feedback about the process of adopting nullability as possible, catch some issues, and collect feedback on further improvements to the feature that we can do after .NET Core 3.0. This is one of the largest features ever built for C#, and although we've done our best to get things right, we need your help!

It is at this junction that we especially call upon .NET library authors to try out the feature and begin annotating your libraries. We'd love to hear your feedback and help resolve any issues you come across.

## Familiarize yourself with the feature

We recommend reading some of the [Nullable Reference Types documentation](https://docs.microsoft.com/dotnet/csharp/nullable-references) before getting started with the feature. It covers essentials like:

* A conceptual overview
* How to specify a nullable reference type
* How to control compiler analysis or override compiler analysis

If you're unfamiliar with these concepts, please give the documentation a quick read before proceeding.

## Turn on Nullable Reference Types

The first step in adopting nullability for your library is to turn it on. Here's how:

### Make sure you're using C# 8.0

If your library explicitly targets `netcoreapp3.0`, you'll get C# 8.0 by default. If not, we recommend updating your TFM to `netcoreapp3.0`.

If you cannot update your TFM, you can set the `LangVersion` explicitly:

```xml
<PropertyGroup>
    <LangVersion>preview</LangVersion>
</PropertyGroup>
```

Just make sure to set this to `latest` when .NET Core 3.0 releases, or find a way to explicitly target `netcoreapp3.0` moving forward.

From here, we recommend two general approaches to adopting nullability.

### Opt in a project, opt out files

This approach is best for projects where you'll be adding new files over time. The process is straightforward:

1. Apply the following property to your project file:

```xml
<PropertyGroup>
    <Nullable>enable</Nullable>
</PropertyGroup>
```

1. Disable nullability in every file for that project by adding this to the top of every existing file in the project:

```csharp
#nullable disable
```

1. Pick a file, remove the `#nullable disable` directive, and fix the warnings. Repeat until all `#nullable disable` directives are gone.

This approach requires a bit more up front work, but it means that you can continue working in your library while you're porting and ensure that any new files are automatically opted into nullability. This is the approach we generally recommend, and we are currently using it in [some of our own codebases](https://github.com/dotnet/project-system/blob/master/src/Directory.Build.props#L28).

Note that you can also apply the `Nullable` property to a `Directory.build.props` file if that fits your workflow better.

### Opt in files one at a time

This approach is the inverse of the previous one.

1. Enable nullability in a file for a project by adding this to the top of the file:

```csharp
#nullable enable
```

1. Continue adding this to files until all files are annotated and all nullability warnings are addressed.

1. Apply the following property to your project file:

```xml
<PropertyGroup>
    <Nullable>enable</Nullable>
</PropertyGroup>
```

1. Remove all `#nullable enable` directives in source.

This approach requires more work at the end, but it allows you to start fixing nullability warnings immediately.

Note that you can also apply the `Nullable` property to a `Directory.build.props` file if that fits your workflow better.

## What's new in Nullable Reference Types

The most critical additions to the feature are tools for working with generics and more advanced API usage scenarios. These were derived from our experience annotating CoreFX.

### The notnull generic constraint

It is quite common to intend that a generic type is specifically not allowed to be nullable. For example, given the following interface:

```csharp
interface IDoStuff<TIn, TOut>
{
    TOut DoStuff(TIn input);
}
```

It may be desirable to only allow non-nullable types. So parameterizing with `string` should be fine, but parameterizing with `string?` should not.

This can be accomplished with the `notnull` constraint:

```csharp
#nullable enable

interface IDoStuff<TIn, TOut>
    where TIn : notnull
    where TOut : notnull
{
    TOut DoStuff(TIn input);
}
```

This will then generate a warning if any implementing class does not also apply the same `notnull` constraints:

```csharp
// Warnings!
class DoStuffer<TIn, TOut> : IDoStuff<TIn, TOut>
{
    TOut DoStuff(TIn input)
    {
        ...
    }
}
```

To fix it, we need to apply the same constraints:

```csharp
// No warnings!
class DoStuffer<TIn, TOut> : IDoStuff<TIn, TOut>
    where TIn : notnull
    where TOut : notnull
{
    TOut DoStuff(TIn input)
    {
        ...
    }
}
```

And when creating an instance of that class, if you parameterize it with a nullable reference type, a warning will also be generated:

```csharp
// warnings!
var doStuffer = new DoStuff<string?, string?>();

// No warnings!
var doStufferRight = new DoStuff<string, string>();
```

This constraint is useful for generic code where you want to ensure that only non-nullable reference types can be used. However, not all nullability problems with generics can be solved in this way. This is where we've added some new attributes to allow you to influence nullable analysis in the compiler.

### Nullable preconditions: AllowNull and DisallowNull

Consider the following example:

```csharp
#nullable enable

public interface IMyEqualityComparer<in T>
{
    bool Equals(T x, T y);
    int GetHashCode(T obj);
}
```

We'd like to allow `null` values for `Equals`, but disallow then for `GetHashCode`. We cannot solve this with the `notnull` constraint, nor can we specify a different generic type for each method (otherwise `Equals` and `GetHashCode` would not work on the same type!)

Our first, and perhaps most natural reaction would be to change the `T` on `GetHashCode` to be `T?`. However, this causes a compile error that makes me specialize `T` to be either a `class` or `struct`. That wouldn't be right, since we want `T` to apply for both classes and structs. Uh-oh!

Enter `[AllowNull]` and `[DisallowNull]`. These attributes let you get _fancy_ with the nullability of input types. We can modify the example as such:

```csharp
#nullable enable

using System.Diagnostics.CodeAnalysis;

public interface IMyEqualityComparer<in T>
{
    bool Equals([AllowNull] T x, [AllowNull] T y);
    int GetHashCode([DisallowNull] T obj);
}
```

Both attributes are specified for illustrative purposes. We could have solved this problem two other ways:

* Only specify `DisallowNull` on `GetHashCode`
* Only specify `AllowNull` on `Equals` input types and constraint `T` to be `notnull`

The end result is all the same: `null` values are allowed for `Equals`, and only non-`null` values are allowed for `GetHashCode`.

More formally:

The `AllowNull` attribute allows callers to pass `null` even if the type doesn't allow it. The `DisallowNull` attribute disallows callers to pass `null` even if the type allows it. They can be specified on anything that takes input:

* Input parameters
* `in` input parameters
* `ref` input parameters
* fields
* properties
* indexers

**Important:** These attributes only affect nullable analysis for the _callers_ of methods that are annotated. The bodies of annotated methods (including when implementing an interface) do not respect these attributes yet.

### Nullable postconditions: MaybeNull and NotNull

Consider the following example:

```csharp
public interface IMyArray<T>
{
    // Result is the default of T if no match is found 
    T Find(Predicate<T> match);

    // Never gives back a null when called
    void Resize(ref T[] array, int newSize)
}
```

Here we have another problem. We'd like `Find` to give back `default` if nothing is found, which is `null` for reference types. We'd like `Resize` to accept a possibly `null` input, but we want to ensure that after `Resize` is called, the `array` value passed by reference is always non-`null`. Again, applying the `notnull` constraint doesn't solve this. Uh-oh!

Enter `[MaybeNull]` and `[NotNull]`. Now we can get _fancy_ with the nullability of the outputs! We can modify the example as such:

```csharp
public interface IMyArray<T>
{
    // Result is the default of T if no match is found
    [return: MaybeNull]
    T Find(Predicate<T> match);

    // Never gives back a null when called
    void Resize([NotNull] ref T[]? array, int newSize)
}
```

The first method specifies that the `T` that is returned could be a `null` value. This means that callers of this method must check for `null` when using its result.

The second method has a trickier signature: `[NotNull] ref T[]? array`. This means that `array` could be `null` as an input, but when `Resize` is called, `array` will not be `null`. This means that if you "dot" into `array` after calling `Resize`, you will not get a warning.

More formally:

The `MaybeNull` attribute allows for a return type to be `null`, even if its type doesn't allow it. The `NotNull` attribute disallows `null` results even if the type allows it. They can be specified on anything that produces output:

* Method returns
* `out` input parameters
* `ref` input parameters
* fields
* properties
* indexers

**Important:** These attributes only affect nullable analysis for the _callers_ of methods that are annotated. The bodies of annotated methods (including when implementing an interface) do not respect these attributes yet.

### Conditional postconditions: MaybeNullWhen(bool) and NotNullWhen(bool)

Consider the following example:

```csharp
public class MyString
{
    // True when 'value' is null
    public static bool IsNullOrEmpty(string? value);
}

public class MyVersion
{
    // If it parses successfully, the Version will not be null.
    public static bool TryParse(string? input, out Version? version);
}

public class MyQueue<T>
{
    // 'result' could be null if we couldn't Dequeue it.
    public bool TryDequeue(out T result)
}
```

Methods like this are everywhere in .NET, where the return value of `true` or `false` corresponds to the nullability (or possible nullability) of a parameter. However, the C# compiler does not associate the meaning of a return value with the nullability of a given parameter! Uh-oh!

Enter `NotNullWhen(bool)` and `MaybeNullWhen(bool)`. Now we can get _even fancier_ with parameters:

```csharp
public class MyString
{
    // True when 'value' is null
    public static bool IsNullOrEmpty([NotNullWhen(false)] string? value);
}

public class MyVersion
{
    // If it parses successfully, the Version will not be null.
    public static bool TryParse(string? input, [NotNullWhen(true)] out Version? version);
}

public class MyQueue<T>
{
    // 'result' could be null if we couldn't Dequeue it.
    public bool TryDequeue([MaybeNullWhen(false)] out T result)
}
```

This enables callers to work with APIs using the same patterns that they've used before, without any spurious warnings from the compiler:

* If `IsNullOrEmpty` is true, it's safe to "dot" into `value`
* If `TryParse` is true, then `version` was parsed and is safe to "dot" into
* If `TryDequeue` is false, then `result` might be `null` and a check is needed (example: returning `false` when the type is a struct is non-`null`, but `false` for a reference type means it could be `null`)

More formally:

The `NotNullWhen(bool)` signifies that a parameter is not null even if the type disallows it, conditional on the `bool` returned value of the method. The `MaybeNullWhen(bool)` signifies that a parameter could be null even if the type disallows it, conditional on the `bool` returned value of the method. They can be specified on any parameter type.

### Nullness dependence between inputs and outputs: NotNullIfNotNull(string)

Consider the following example:

```csharp
class MyPath
{
    public static string? GetFileName(string? path);
}
```

In this case, we'd like to return a possibly `null` string, and we should also be able to accept a `null` value as input. So the signature accomplishes what I'd like to express.

However, if `path` is not `null`, we'd like to ensure that we always give back a string. That is, we want the return value of `GetFileName` to be non-null, conditional on the nullness of `path`. There's no way to express this as-is. Uh-oh!

Enter `NotNullIfNotNull(string)`. This attribute can make your code the _fanciest_, so use it with care! Here's how we'll use it in my API:

```csharp
class MyPath
{
    [return: NotNullIfNotNull("path")]
    public static string? GetFileName(string? path);
}
```

This will now affect all callers of `GetFileName`:

* If `GetFileName` is passed a non-`null` string for the `path` parameter, then it is safe to "dot" into the return of `GetFileName`
* If `GetFileName` is passed a `null` for the `path` parameter, then a warning will be emitted if someone doesn't check the return of `GetFileName`

They can be specified on the following constructs:

* Method returns
* `out` input parameters
* `ref` input parameters

More formally:

The `NotNullIfNotNull(string)` attribute signifies that any output value is non-`null` conditional on the nullability of a given parameter whose name is specified.

## Evolving your annotations

Once you annotate a public API, you'll want to consider the fact that updating an API can have downstream effects:

* Adding nullable annotations where there weren't any will introduce warnings to user code
* Removing nullable annotations can also introduce warnings (e.g., interface implementation)

Although introducing further warnings is certainly better than introducing errors over time, we recommend starting with a preview release where you solicit feedback, with aims to not change any annotations after a full release. This isn't always going to be possible, but we recommend it nonetheless.

## Current status of Microsoft frameworks and libraries

Because Nullable Reference Types are so new, the large majority of Microsoft-authored C# frameworks and libraries have not yet been appropriately annotated.

That said, the "CoreLib" part of CoreFX, which represents about ~20% of the .NET Core shared framework, has been fully updated. We're looking for feedback on our decisions so that we can make appropriate tweaks as soon as possible, and before their usage becomes widespread.

Although there is still ~80% CoreFX to still annotate, the most-used APIs are fully annotated.

## Roadmap for Nullable Reference Types

Over the coming year, we're going to continue to improve the feature and spread its use throughout Microsoft frameworks and libraries.

For the language, especially compiler analysis, we'll be making numerous enhancements so that we can minimize your need to do things like use the null-forgiveness (`!`) operator. Many of these enhancements are [already tracked on the Roslyn repo](https://github.com/dotnet/roslyn/issues?q=is%3Aissue+is%3Aopen+label%3A%22New+Language+Feature+-+Nullable+Reference+Types%22).

For CoreFX, we'll be annotating the remaining ~80% of APIs and making appropriate tweaks based on feedback.

For ASP.NET Core and Entity Framework, we'll be annotating public APIs once some new additions to CoreFX and the compiler are added.

We haven't yet planned how to annotate WinForms and WPF APIs, but we'd love to hear your feedback on what kinds of things matter!

Finally, we're going to continue enhancing C# tooling in Visual Studio. We have multiple ideas for features to help using the feature, but we'd love your input as well!

## Next steps

If you're still reading and haven't tried out the feature in your code, especially your library code, give it a try and please give us feedback on anything you feel ought to be different. The journey to make unanticipated `NullReferenceException`s in .NET go away will be lengthy, but we hope that in the long run, developers simply won't have to worry about `null` anymore.

Cheers, and happy hacking!
