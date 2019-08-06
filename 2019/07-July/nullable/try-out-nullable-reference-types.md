# Try out Nullable Reference Types

With the release of .NET Core 3.0 Preview 7, C# 8.0 is considered "feature complete". That means that the biggest feature of them all, [Nullable Reference Types](https://docs.microsoft.com/dotnet/csharp/nullable-references), is also locked down behavior-wise for the .NET Core release. It will continue to improve after C# 8.0, but it is now considered stable with the rest of C# 8.0.

At this time, our aim is to collect as much feedback about the process of adopting nullability as possible, catch any issues, and collect feedback on further improvements to the feature that we can do after .NET Core 3.0. This is one of the largest features ever built for C#, and although we've done our best to get things right, we need your help!

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

If your library explicitly targets `netcoreapp3.0`, you'll get C# 8.0 by default. When we ship Preview 8, you'll get C# 8.0 by default if you target `netstandard2.1` too.

.NET Standard itself doesn't have any nullable annotations yet. If you're targeting .NET Standard, then you can use multi-targeting for .NET Standard and `netcoreapp3.0`, even if you don't need .NET Core specific APIs. The benefit is that the compiler will use the nullable annotations from CoreFX to help you get your own annotations right.

If you cannot update your TFM for some reason, you can set the `LangVersion` explicitly:

```xml
<PropertyGroup>
    <LangVersion>8.0</LangVersion>
</PropertyGroup>
```

Note that C# 8.0 is not meant for older targets, such as .NET Core 2.x or .NET Framework 4.x. So some additional language features may not work unless you are targeting .NET Core 3.0 or .NET Standard 2.1

From here, we recommend two general approaches to adopting nullability.

### Opt in a project, opt out files

This approach is best for projects where you'll be adding new files over time. The process is straightforward:

01. Apply the following property to your project file:

    ```xml
    <PropertyGroup>
        <Nullable>enable</Nullable>
    </PropertyGroup>
    ```

02. Disable nullability in every file for that project by adding this to the top of every existing file in the project:

    ```csharp
    #nullable disable
    ```

03. Pick a file, remove the `#nullable disable` directive, and fix the warnings. Repeat until all `#nullable disable` directives are gone.

This approach requires a bit more up front work, but it means that you can continue working in your library while you're porting and ensure that any new files are automatically opted-in to nullability. This is the approach we generally recommend, and we are currently using it in [some of our own codebases](https://github.com/dotnet/project-system/blob/master/src/Directory.Build.props#L28).

Note that you can also apply the `Nullable` property to a `Directory.build.props` file if that fits your workflow better.

### Opt in files one at a time

This approach is the inverse of the previous one.

01. Enable nullability in a file for a project by adding this to the top of the file:

    ```csharp
    #nullable enable
    ```

02. Continue adding this to files until all files are annotated and all nullability warnings are addressed.

03. Apply the following property to your project file:

    ```xml
    <PropertyGroup>
        <Nullable>enable</Nullable>
    </PropertyGroup>
    ```

04. Remove all `#nullable enable` directives in source.

This approach requires more work at the end, but it allows you to start fixing nullability warnings immediately.

Note that you can also apply the `Nullable` property to a `Directory.build.props` file if that fits your workflow better.

## What's new in Nullable Reference Types for Preview 7

The most critical additions to the feature are tools for working with generics and more advanced API usage scenarios. These were derived from our experience in beginning to annotate .NET Core.

### The `notnull` generic constraint

It is quite common to intend that a generic type is specifically not allowed to be nullable. For example, given the following interface:

```csharp
interface IDoStuff<TIn, TOut>
{
    TOut DoStuff(TIn input);
}
```

It may be desirable to only allow non-nullable reference and value types. So substituting with `string` or `int` should be fine, but substituting with `string?` or `int?` should not.

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
// Warning: CS8714 - Nullability of type argument 'TIn' doesn't match 'notnull' constraint.
// Warning: CS8714 - Nullability of type argument 'TOut' doesn't match 'notnull' constraint.
public class DoStuffer<TIn, TOut> : IDoStuff<TIn, TOut>
{
    public TOut DoStuff(TIn input)
    {
        ...
    }
}
```

To fix it, we need to apply the same constraints:

```csharp
// No warnings!
public class DoStuffer<TIn, TOut> : IDoStuff<TIn, TOut>
    where TIn : notnull
    where TOut : notnull
{
    TOut DoStuff(TIn input)
    {
        ...
    }
}
```

And when creating an instance of that class, if you substitute it with a nullable reference type, a warning will also be generated:

```csharp
// Warning: CS8714 - Nullability of type argument 'string?' doesn't match 'notnull' constraint
var doStuffer = new DoStuff<string?, string?>();

// No warnings!
var doStufferRight = new DoStuff<string, string>();
```

It also works for value types:

```csharp
// Warning: CS8714 - Nullability of type argument 'int?' doesn't match 'notnull' constraint
var doStuffer = new DoStuff<int?, int?>();

// No warnings!
var doStufferRight = new DoStuff<int, int>();
```

This constraint is useful for generic code where you want to ensure that only non-nullable reference types can be used. One prominent example is `Dictionary<TKey, TValue`, where `TKey` is now constrained to be `notnull`, which disallows using `null` as a key:

```csharp
// Warning: CS8714 - Nullability of type argument 'string?' doesn't match 'notnull' constraint
var d1 = new Dictionary<string?, string>(10);

// And as expected, using 'null' as a key for a non-nullable key type is a warning...
var d2 = new Dictionary<string, string>(10);

// Warning: CS8625 - Cannot convert to non-nullable reference type.
var nothing = d2[null];
```

However, not all nullability problems with generics can be solved in this way. This is where we've added some new attributes to allow you to influence nullable analysis in the compiler.

### The issue with `T?`

So you have have wondered: why not "just" allow `T?` when specifying a generic type that could be substituted with a nullable reference or value type? The answer is, unfortunately, complicated.

A natural definition of `T?` would mean, "any nullable type". However, this would imply that `T` would mean "any non-nullable type", and that is not true! It is possible to substitute a `T` with a nullable value type today (such as `bool?`). This is because `T` is already an unconstrained generic type. This change in semantics would likely be unexpected and cause some grief for the vast amount of existing code that uses `T` as an unconstrained generic type.

Next, it's important to note that a nullable reference type is _not_ the same thing as a nullable value type. Nullable value types map to a concrete class type in .NET. So `int?` is actually `Nullable<int>`. But for `string?`, it's actually the same `string` but with a compiler-generated attribute annotating it. This is done for backwards compatibility. In other words, `string?` is kind of a "fake type", whereas `int?` is not.

This distinction between nullable value types and nullable reference types comes up in a pattern such as this:

```csharp
void M<T>(T? t) where T: notnull
```

This would mean that the parameter is the nullable version of `T`, and `T` is constrained to be `notnull`. If `T` where a `string`, then the actual signature of `M` would be `M<string>([NullableAttribute] T t)`, but if `T` were an `int`, then `M` would be `M<int>(Nullable<int> t)`. These two signatures are fundamentally different, and this difference is not reconcilable.

Because of this issue between the concrete representations of nullable reference types and nullable value types, any use of `T?` must also require you to constrain the `T` to be either `class` or `struct`.

Finally, the existence of a `T?` that worked for both nullable reference types and nullable value types does not address every issue with generics. You may want to allow for nullable types in a single direction (i.e., as only an input or only an output) and that is not expressible with either `notnull` nor a `T` and `T?` split unless you artificially add separate generic types for inputs and outputs.

### Nullable preconditions: AllowNull and DisallowNull

Consider the following example:

```csharp
public class MyClass
{
    public string MyValue { get; set; }
}
```

This might have been an API that we supported prior to C# 8.0. However, the meaning of `string` now means non-nullable `string`! We may wish to actually still allow `null` values, but always give back some `string` value with the `get`. Here's where `AllowNull` can come in and let you get fancy:

```
public class MyClass
{
    private string _innerValue = string.Empty;

    [AllowNull]
    public string MyValue
    {
        get
        {
            return _innerValue;
        }
        set
        {
            _innerValue = value ?? string.Empty;
        }
    }
}
```

Since we always make sure that we get no `null` value with the getter, I'd like the type to remain `string`. But we want to still accept `null` values for backwards compatibility. The `AllowNull` attribute lets you specify that the setter accepts `null` values. Callers are then affected as you'd expect:

```csharp
void M1(MyClass mc)
{
    mc.MyValue = null; // Allowed because of AllowNull
}

void M2(MyClass mc)
{
    Console.WriteLine(mc.MyValue.Length); // Also allowed, note there is no warning
}
```

Note: there is currently [a bug](https://github.com/dotnet/roslyn/issues/37313) where assignment of `null` conflicts with nullable analysis. This will be addressed in a future update of the compiler.

Consider another API:

```csharp
public static HandleMethods
{
    public static void DisposeAndClear(ref MyHandle handle)
    {
        ...
    }
}
```

In this case, `MyHandle` refers to some handle to a resource. Typical use for this API is that we have a non-`null` instance that we pass by reference, but when it is cleared, the reference is `null`. We can get fancy and represent this with `DisallowNull`:

```csharp
public static HandleMethods
{
    public static void DisposeAndClear([DisallowNull] ref MyHandle? handle)
    {
        ...
    }
}
```

This will affect any caller by emitting a warning if they pass `null`, but will warn if you attempt to "dot" into the `handle` after the method is called:

```csharp
void M(MyHandle handle)
{
    MyHandle? local = null; // Create a null value here
    HandleMethods.DisposeAndClear(ref local); // Warning: CS8601 - Possible null reference assignment
    
    // Now pass the non-null handle
    HandleMethods.DisposeAndClear(ref handle); // No warning! ... But the value could be null now
    
    Console.WriteLine(handle.SomeProperty); // Warning: CS8602 - Dereference of a possibly null reference
}
```

These two attributes allow us single-direction nullability or non-nullability for those cases where we need them.

More formally:

The `AllowNull` attribute allows callers to pass `null` even if the type doesn't allow it. The `DisallowNull` attribute disallows callers to pass `null` even if the type allows it. They can be specified on anything that takes input:

* Value parameters
* `in` parameters
* `ref` parameters
* fields
* properties
* indexers

**Important:** These attributes only affect nullable analysis for the _callers_ of methods that are annotated with them. The bodies of annotated methods  and things like interface implementation do not respect these attributes. We may add support for that in the future.

### Nullable postconditions: MaybeNull and NotNull

Consider the following example API:

```csharp
public class MyArray
{
    // Result is the default of T if no match is found
    public static T Find<T>(T[] array, Func<T, bool> match)
    {
        ...
    }

    // Never gives back a null when called
    public static void Resize<T>(ref T[] array, int newSize)
    {
        ...
    }
}
```

Here we have another problem. We'd like `Find` to give back `default` if nothing is found, which is `null` for reference types. We'd like `Resize` to accept a possibly `null` input, but we want to ensure that after `Resize` is called, the `array` value passed by reference is always non-`null`. Again, applying the `notnull` constraint doesn't solve this. Uh-oh!

Enter `[MaybeNull]` and `[NotNull]`. Now we can get fancy with the nullability of the outputs! We can modify the example as such:

```csharp
public class MyArray
{
    // Result is the default of T if no match is found
    [return: MaybeNull]
    public static T Find<T>(T[] array, Func<T, bool> match)
    {
        ...
    }

    // Never gives back a null when called
    public static void Resize<T>([NotNull] ref T[]? array, int newSize)
    {
        ...
    }
}
```

And these can now affect call sites:

```csharp
void M(string[] testArray)
{
    var value = MyArray.Find<string>(testArray, s => s == "Hello!");
    Console.WriteLine(value.Length); // Warning: Dereference of a possibly null reference.

    MyArray.Resize<string>(ref testArray, 200);
    Console.WriteLine(testArray.Length); // Safe!
}
```

The first method specifies that the `T` that is returned could be a `null` value. This means that callers of this method must check for `null` when using its result.

The second method has a trickier signature: `[NotNull] ref T[]? array`. This means that `array` could be `null` as an input, but when `Resize` is called, `array` will not be `null`. This means that if you "dot" into `array` after calling `Resize`, you will not get a warning. But after `Resize` is called, `array` will no longer be `null`.

More formally:

The `MaybeNull` attribute allows for a return type to be `null`, even if its type doesn't allow it. The `NotNull` attribute disallows `null` results even if the type allows it. They can be specified on anything that produces output:

* Method returns
* `out` parameters (after a method is called)
* `ref` parameters (after a method is called)
* fields
* properties
* indexers

**Important:** These attributes only affect nullable analysis for the _callers_ of methods that are annotated with them. The bodies of annotated methods  and things like interface implementation do not respect these attributes. We may add support for that in the future.

### Conditional postconditions: MaybeNullWhen(bool) and NotNullWhen(bool)

Consider the following example:

```csharp
public class MyString
{
    // True when 'value' is null
    public static bool IsNullOrEmpty(string? value)
    {
        ...
    }
}

public class MyVersion
{
    // If it parses successfully, the Version will not be null.
    public static bool TryParse(string? input, out Version? version)
    {
        ...
    }
}

public class MyQueue<T>
{
    // 'result' could be null if we couldn't Dequeue it.
    public bool TryDequeue(out T result)
    {
        ...
    }
}
```

Methods like this are everywhere in .NET, where the return value of `true` or `false` corresponds to the nullability (or possible nullability) of a parameter. The `MyQueue` case is also a bit special, since it's generic. `TryDequeue` should give a `null` for `result` if the result is `false`, but only if `T` is a reference type. If `T` is a struct, then it won't be `null`.

So, we want to do three things:

1. Signal that if `IsNullOrEmpty` returns `true`, then `value` is non-`null`
1. Signal that if `TryParse` returns `true`, then `version` is non-`null`
1. Signal that if `TryDequeue` returns `false`, then `result` _could_ be `null`, provided it's a reference type

Unfortunately, the C# compiler does not associate the return value of a method with the nullability of one of its parameters! Uh-oh!

Enter `NotNullWhen(bool)` and `MaybeNullWhen(bool)`. Now we can get _even fancier_ with parameters:

```csharp
public class MyString
{
    // True when 'value' is null
    public static bool IsNullOrEmpty([NotNullWhen(false)] string? value)
    {
        ...
    }
}

public class MyVersion
{
    // If it parses successfully, the Version will not be null.
    public static bool TryParse(string? input, [NotNullWhen(true)] out Version? version)
    {
        ...
    }
}

public class MyQueue<T>
{
    // 'result' could be null if we couldn't Dequeue it.
    public bool TryDequeue([MaybeNullWhen(false)] out T result)
    {
        ...
    }
}
```

And these can now affect call sites:

```csharp
void StringTest(string? s)
{
    if (MyString.IsNullOrEmpty(s))
    {
        // This would generate a warning:
        // Console.WriteLine(s.Length);
        return;
    }

    Console.WriteLine(s.Length); // Safe!
}

void VersionTest(string? s)
{
    if (!MyVersion.TryParse(s, out var version))
    {
        // This would generate a warning:
        // Console.WriteLine(version.Major);
        return;
    }

    Console.WriteLine(version.Major); // Safe!
}

void QueueTest(MyQueue<string> q)
{
    if (!q.TryDequeue(out var s))
    {
        // This would generate a warning:
        // Console.WriteLine(s.Length);
        return;
    }

    Console.WriteLine(s.Length); // Safe!
}
```

This enables callers to work with APIs using the same patterns that they've used before, without any spurious warnings from the compiler:

* If `IsNullOrEmpty` is true, it's safe to "dot" into `value`
* If `TryParse` is true, then `version` was parsed and is safe to "dot" into
* If `TryDequeue` is false, then `result` might be `null` and a check is needed (example: returning `false` when the type is a struct is non-`null`, but `false` for a reference type means it could be `null`)

More formally:

The `NotNullWhen(bool)` signifies that a parameter is not null even if the type allows it, conditional on the `bool` returned value of the method. The `MaybeNullWhen(bool)` signifies that a parameter could be null even if the type disallows it, conditional on the `bool` returned value of the method. They can be specified on any parameter type.

### Nullness dependence between inputs and outputs: NotNullIfNotNull(string)

Consider the following example:

```csharp
class MyPath
{
    public static string? GetFileName(string? path)
    {
        ...
    }
}
```

In this case, we'd like to return a possibly `null` string, and we should also be able to accept a `null` value as input. So the signature accomplishes what I'd like to express.

However, if `path` is not `null`, we'd like to ensure that we always give back a string. That is, we want the return value of `GetFileName` to be non-null, conditional on the nullness of `path`. There's no way to express this as-is. Uh-oh!

Enter `NotNullIfNotNull(string)`. This attribute can make your code the _fanciest_, so use it with care! Here's how we'll use it in my API:

```csharp
class MyPath
{
    [return: NotNullIfNotNull("path")]
    public static string? GetFileName(string? path)
    {
        ...
    }
}
```

And this can now affect call sites:

```csharp
void PathTest(string? path)
{
    var possiblyNullPath = MyPath.GetFileName(path);
    Console.WriteLine(possiblyNullPath.Length); // Warning: Dereference of a possibly null reference
    
    if (!string.IsNullOrEmpty(path))
    {
        var goodPath = MyPath.GetFileName(path);
        Console.WriteLine(goodPath.Length); // Safe!
    }
}
```

More formally:

The `NotNullIfNotNull(string)` attribute signifies that any output value is non-`null` conditional on the nullability of a given parameter whose name is specified. They can be specified on the following constructs:

* Method returns
* `ref` parameters

### Flow attributes: `DoesNotReturn` and `DoesNotReturnIf(bool)`

You may work with multiple methods that affect control flow of your program. For example, an exception helper method that will throw an exception if called, or an assertion method that will throw an exception if an input is `true` or `false`.

You may wish to do something like _assert_ that a value is non-null, and we think you'd also like it if the compiler could understand that.

Enter `DoesNotReturn` and `DoesNotReturnIf(bool)`. Here's an example of how you could use either:

```csharp
internal static class ThrowHelper
{
    [DoesNotReturn]
    public static void ThrowArgumentNullException(ExceptionArgument arg)
    {
        ...
    }
}

public static class MyAssertionLibrary
{
    public static void MyAssert([DoesNotReturnIf(false)] bool condition)
    {
        ...
    }
}
```

When `ThrowArgumentNullException` is called in a method, it throws an exception. The `DoesNotReturn` it is annotated with will signal to the compiler that no nullable analysis needs to happen after that point, since that code would be unreachable.

When `MyAssert` is called and the condition passed to it is `false`, it throws an exception. The `DoesNotReturnIf(false)` that annotates the `condition` parameter lets the compiler know that program flow will not continue if that condition is false. This is helpful if you want to assert the nullability of a value. In the code path following `MyAssert(value != null);` the compiler can assume `value` is not null.

`DoesNotReturn` can be used on methods. `DoesNotReturnIf(bool)` can be used on input parameters.

## Evolving your annotations

Once you annotate a public API, you'll want to consider the fact that updating an API can have downstream effects:

* Adding nullable annotations where there weren't any may introduce warnings to user code
* Removing nullable annotations can also introduce warnings (e.g., interface implementation)

Nullable annotations are an integral part of your public API. Adding or removing annotations introduce new warnings. We recommend starting with a preview release where you solicit feedback, with aims to not change any annotations after a full release. This isn't always going to be possible, but we recommend it nonetheless.

## Current status of Microsoft frameworks and libraries

Because Nullable Reference Types are so new, the large majority of Microsoft-authored C# frameworks and libraries have not yet been appropriately annotated.

That said, the "Core Lib" part of .NET Core, which represents about ~20% of the .NET Core shared framework, has been fully updated. It includes namespaces like `System`, `System.IO`, and `System.Collections.Generic`. We're looking for feedback on our decisions so that we can make appropriate tweaks as soon as possible, and before their usage becomes widespread.

Although there is still ~80% CoreFX to still annotate, the most-used APIs are fully annotated.

## Roadmap for Nullable Reference Types

Currently, we view the _full_ Nullable Reference Types experience as being in preview. It's stable, but the feature involves spreading nullable annotations throughout our own technologies and the greater .NET ecosystem. This will take some time to complete.

That said, we're encouraging library authors to start annotating their libraries now. The feature will only get better as more libraries adopt nullability, helping .NET become a more `null`-safe place.

Over the coming year or so, we're going to continue to improve the feature and spread its use throughout Microsoft frameworks and libraries.

For the language, especially compiler analysis, we'll be making numerous enhancements so that we can minimize your need to do things like use the null-forgiveness (`!`) operator. Many of these enhancements are [already tracked on the Roslyn repo](https://github.com/dotnet/roslyn/issues?q=is%3Aissue+is%3Aopen+label%3A%22New+Language+Feature+-+Nullable+Reference+Types%22).

For CoreFX, we'll be annotating the remaining ~80% of APIs and making appropriate tweaks based on feedback.

For ASP.NET Core and Entity Framework, we'll be annotating public APIs once some new additions to CoreFX and the compiler are added.

We haven't yet planned how to annotate WinForms and WPF APIs, but we'd love to hear your feedback on what kinds of things matter!

Finally, we're going to continue enhancing C# tooling in Visual Studio. We have multiple ideas for features to help using the feature, but we'd love your input as well!

## Next steps

If you're still reading and haven't tried out the feature in your code, especially your library code, give it a try and please give us feedback on anything you feel ought to be different. The journey to make unanticipated `NullReferenceException`s in .NET go away will be lengthy, but we hope that in the long run, developers simply won't have to worry about `null` anymore. You can help us. Try out the feature and begin annotating your libraries. Feedback on your experience will help shorten that journey.

Cheers, and happy hacking!
