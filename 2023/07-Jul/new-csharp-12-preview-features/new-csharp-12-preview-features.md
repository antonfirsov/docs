---
post_title: New C# 12 preview features
author1: kathleen.a.dollard
post_slug: new-csharp-12-preview-features
microsoft_alias: kdollard
featured_image: new-csharp-12-preview-features.png
categories: .NET, C#
tags: .net 8, c# 12
summary: ".NET 8 Preview 6 adds three new features for C# 12: interceptors, inline arrays, and enhancements to the nameof expression."
post_date: 2023-07-11 10:10:00
---

Visual Studio 17.7 Preview 3 and .NET 8 Preview 6 continue the evolution of C# 12. This preview includes features designed to lay the groundwork for future performance enhancements. Easy access to inline arrays will allow libraries to use them in more places without effort on your part. This preview debuts an experimental feature called *interceptors* that allow generators to reroute code, such as to provide context specific optimization. Finally, `nameof` is enhanced to work in more places. 

You can get C# 12 by [installing the latest Visual Studio preview](https://visualstudio.microsoft.com/vs/preview/) or [the latest version of the .NET SDK](https://dotnet.microsoft.com/download/dotnet/8.0). To check out C# 12 features, you'll need to set the language version of your project to preview:

```xml
<PropertyGroup>
   <LangVersion>preview</LangVersion>
</PropertyGroup>
```

Since they are experimental, [interceptors require an additional flag](#interceptors) in your project file.

## `nameof` accessing instance members

The `nameof` keyword now works with member names, including initializers, on static members, and in attributes: 

```c#
internal class NameOf
{
    public string S { get; } = "";
    public static int StaticField;
    public string NameOfLength { get; } = nameof(S.Length);
    public static void NameOfExamples()
    {
        Console.WriteLine(nameof(S.Length));
        Console.WriteLine(nameof(StaticField.MinValue));
    }
    [Description($"String {nameof(S.Length)}")]
    public int StringLength(string s)
    { return s.Length; }
}
```
You can learn more at [What's new in C# 12](https://learn.microsoft.com/dotnet/csharp/whats-new/csharp-12).

## Inline arrays

The [InlineArrayAttribute](https://github.com/dotnet/runtime/issues/61135) was introduced to the runtime in a previous .NET 8 preview. This is an advanced feature that will be used primarily by the compiler, .NET libraries and some other libraries. The attribute identifies a type that can be treated as a contiguous sequence of primitives for efficient, type-safe, overrun-safe indexable/sliceable inline data. The .NET libraries improve performance of your application and tools using inline arrays.

The compiler creates different IL to access inline arrays. This results in a few restrictions, such as list patterns not being supported. In most cases, you access inline arrays the same as other arrays. The different IL creates performance gains without changing your code:

```c#
private static void InlineArrayAccess(Buffer10<int> inlineArray)
{
    for (int i = 0; i < 10; i++)
    {
        inlineArray[i] = i * i;
    }
    foreach (int i in inlineArray)
    {
        Console.WriteLine(i);
    }
}
```

Most folks will consume inline arrays, rather than create them. But, it's nice to understand how things work. Inline arrays are fast because they rely on an exact layout of a specified length. An inline array is a type with a single field and marked with the `InlineArrayAttribute` that specifies the length of the array. In the type used in the previous example, the runtime creates storage for exactly ten elements in `Buffer10<T>` due to the attribute parameter:

```c#
[System.Runtime.CompilerServices.InlineArray(10)]
public struct Buffer10<T>
{
    private T _element0;
}
```

You can learn more at [What's new in C# 12](https://learn.microsoft.com/dotnet/csharp/whats-new/csharp-12#inline-arrays).

## Interceptors

This preview introduces an experimental feature called _interceptors_. It’s intended for advanced scenarios, particularly allowing better ahead of time compilation (AOT). As an experimental part of .NET 8, it may be changed or removed in a future version. Thus, it should not be used in production. 

Interceptors allow specific method calls to be rerouted to different code. Attributes specify the actual source code location so interceptors are generally appropriate only for source generators. You can read the [interceptors proposal](https://github.com/dotnet/csharplang/issues/7009) to learn more about how interceptors work.

Because interceptors are an experimental feature, you'll need to explicitly enable them in your project file:

```xml
<PropertyGroup>
   <Features>InterceptorsPreview<Features>
</PropertyGroup>
```

Interceptors enable exciting code patterns. A few examples are:

* Calls that are known at compile time, like `Regex.IsMatch(@"a+b+")` with a constant pattern can be intercepted to use statically-generated code for optimization that is friendly to AOT.

* ASP.NET Minimal API calls like `app.MapGet("/products", handler: (int? page, int? pageLength, MyDb db) => { ... })` can be intercepted to register a statically-generated thunk which calls the user's handler directly, skipping an allocation and indirection. 

* In vectorization, where foreach loops contain calls to user methods, the compiler can rewrite code to check for and use relevant intrinsics at runtime, but fall back to the original code if those intrinsics aren't available.

* Static dependency graph resolution for dependency injection, where  `provider.Register<MyService>()` can be intercepted.

* Calls to query providers could be intercepted to offer translation to another language (e.g. SQL) at compile time, rather than evaluating expression trees to translate at runtime.

* Serializers could generate type specific (de)serialization based on the concrete type of calls like `Serialize<MyType>()`, all at compile time.

Most programmers will not use interceptors directly, but we hope they will play a significant role in our journey to make your applications run faster and be easier to deploy. Interceptors are expected to remain experimental in the C# 12/.NET 8 release and may be included in a future version of C#.

## Summary

You can find more information about all of the features introduced so far at the [What's new in C# 12 page of Microsoft Learn](https://learn.microsoft.com/dotnet/csharp/whats-new/csharp-12) and track the evolution of C# 12 features at the  [Roslyn Feature Status page](https://github.com/dotnet/roslyn/blob/main/docs/Language%20Feature%20Status.md). 

You can check out the latest C# 12 features by [downloading the latest Visual Studio preview](https://visualstudio.microsoft.com/vs/preview/) or [the latest version of the .NET SDK](https://dotnet.microsoft.com/download/dotnet/8.0), and setting `LangVersion` to `preview` in your project file.

Let us know what you think!