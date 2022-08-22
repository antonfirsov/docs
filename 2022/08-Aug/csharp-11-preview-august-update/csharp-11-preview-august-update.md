---
post_title: "C# 11 preview: generic math, required members, and more"
author1: wiwagn
post_slug: csharp-11-preview-august-update
username: wiwagn
microsoft_alias: wiwagn
featured_image: csharp-image.png
categories: .NET Core, C#
summary: Most of the C# 11 features are available for you to try. Learn about the latest updates shipping with Visual Studio 17.3, try them out, and give us feedback.
desired_publication_date: 2022-08-22
---

[C# 11](https://docs.microsoft.com/dotnet/csharp/whats-new/csharp-11) is nearing completion. This post covers features that are new in 17.3 or were not covered in our [April update on Visual Studio 17.2](https://devblogs.microsoft.com/dotnet/csharp-11-preview-updates/) and our [February update on Visual Studio 17.1](https://devblogs.microsoft.com/dotnet/early-peek-at-csharp-11-features/).

The new features in this preview follow on three themes of investment for C# 11:

- [Object initialization improvements](#improved-object-initialization): You can support constructors and object initializers in your type easier, independent of the rules you want to enforce for mutable and immutable members. Features include:
  - Required members
  - `ref` fields
- [Generic math support](#generic-math-support): You can write algorithms once for multiple numeric types. These features make it easier to use C# and .NET for statistics, machine learning, and other math-intensive applications. Features include: 
  - Static abstract and static virtual members in interfaces
  - Relaxed right-shift requirements
  - Unsigned right shift operator
  - Numeric `IntPtr`]
- [Developer productivity](#developer-productivity): We've added more language features to make you more productive. The extended `nameof` scope feature is new.

The sections below provide an overview of each feature and links in [Microsoft Docs](https://docs.microsoft.com/dotnet/csharp) where you can read more. To try these features, you'll need to enable preview features in your project. That's explained in the [What's new in C# 11](https://docs.microsoft.com/dotnet/csharp/whats-new/csharp-11) article in docs.

## Improved object initialization

[Required members](https://docs.microsoft.com/dotnet/csharp/language-reference/keywords/required) let you write class and struct types that *require* callers to set certain properties.  Consider this `Person` type:

```csharp
public class Person
{
    public string FirstName { get; init; }
    public string LastName {get; init; }
}
```

Callers should use object initializers to set the values of the `FirstName` and `LastName` property. But prior to 17.3, the compiler can't enforce that callers must set those properties. A constructor that requires parameters is the only way to ensure the user sets the `FirstName` and `LastName` properties. Required members communicates to the compiler and callers that they must set those properties. Add the `required` modifier to the member declarations:

```csharp
public class Person
{
    public required string FirstName { get; init; }
    public required string LastName {get; init; }
}
```

All callers must include object initializers for the `FirstName` and `LastName` properties or the compiler emits an error. The compiler informs callers that required members weren't initialized. The developer must fix the problem immediately. 

If the `Person` type was written for an earlier release and includes a constructor that sets properties, you can still use required members. You should annotate any existing constructors with the `SetsRequiredMembers` attribute:

```csharp
public class Person
{
    public required string FirstName { get; init; }
    public required string LastName {get; init; }

    [SetsRequiredMembers]
    public Person(string firstName, string lastName)
    {
        this.FirstName = firstName;
        this.LastName = lastName;
    }
    
    public Person() {}
}
```

The `SetsRequiredMembers` attribute indicates that a constructor sets all required members. The compiler knows that callers using the `Person(string firstName, string lastName)` constructor have set the required members. The parameterless constructor doesn't include that attribute, so callers using that constructor must initialize all required members using object initializers.

The examples above used properties, but you can apply required members to field declarations as well.

This preview also contains an initial implementation of [`ref` fields and `scoped` values](https://github.com/dotnet/csharplang/blob/main/proposals/low-level-struct-improvements.md#provide-ref-fields-and-scoped). These changes provide the capability for `ref` fields in `ref struct` types. You can also use the `scoped` keyword to limit the lifetime of `ref` parameters. The feature proposal and [updated changes](https://github.com/dotnet/csharplang/issues/6337) provide the best documentation on this feature right now. We discovered some scenarios that required language changes to be used safely. The updated changes will be available in a later preview, and the documentation will reflect the final design.

## Generic math support

We've added features where the motivating scenario was [generic math](../../06-Jun/dotnet-7-generic-math/dotnet-7-generic-math.md). You'll only use these features directly in advanced scenarios, such as writing mathematics algorithms that work on multiple number types. Otherwise, you'll benefit indirectly because the runtime uses these features:

- [Static abstract and static virtual members in interfaces](https://docs.microsoft.com/dotnet/csharp/whats-new/csharp-11#generic-math-support)
- [Relaxed right-shift requirements](https://docs.microsoft.com/dotnet/csharp/whats-new/csharp-11#generic-math-support)
- [Unsigned right shift operator](https://docs.microsoft.com/dotnet/csharp/whats-new/csharp-11#generic-math-support)
- [Numeric `IntPtr`](https://docs.microsoft.com/dotnet/csharp/whats-new/csharp-11#numeric-intptr-and-uintptr)

The addition of [static abstract and virtual members in interfaces](https://docs.microsoft.com/dotnet/csharp/language-reference/keywords/interface#static-abstract-and-virtual-members) provides much of the important infrastructure for generic math. This feature allows interfaces to declare operators, or other static methods. Classes that Implement an interface must provide the implementation of `static abstract` methods, just like other methods declared in interfaces. The compiler resolves calls to `static` methods, including operators, at compile time. There's no runtime dispatch mechanism as there is with instance methods. The [docs](https://docs.microsoft.com/dotnet/csharp/language-reference/keywords/interface#static-abstract-and-virtual-members) provide more details on the specific language rules required to make this feature work.

Other language features smooth out some differences in numeric types to make it easier to write generic mathematics algorithms. The right-shift operator no longer requires the second operand to be an `int`. Any integral type will do! The `nint` and `nuint` types are synonyms for `System.IntPtr` and `System.UIntPtr`, respectively. These keywords can be used in place of those types. In fact, new analyzers will gently nudge you to prefer the keywords to the type names. Finally, the unsigned right-shift operator (`>>>`) avoids casts when you perform an unsigned shift.

Combined, these changes and other changes like [checked operators](https://docs.microsoft.com/dotnet/csharp/language-reference/operators/arithmetic-operators#user-defined-checked-operators) support the generic math runtime changes. The language improvements mean the runtime team can provide improvements across all numeric types in .NET. You can also leverage the features when your types implement contracts using operators, or other static methods.

## Developer productivity

The `nameof` operator now can be used with [method parameters](https://docs.microsoft.com/dotnet/csharp/whats-new/csharp-11#extended-nameof-scope). This feature enables you to use the `nameof` operator in attribute declarations on methods, like the following example shows:

```csharp
[return: NotNullIfNotNull(nameof(url))]
string? GetTopLevelDomainFromFullUrl(string? url)
```

## Give it a try


Please download the latest [Visual Studio 2022 Preview](https://visualstudio.microsoft.com/vs/preview) and install the .NET 7 preview or you can separately install latest preview of [.NET 7](https://dotnet.microsoft.com/download/dotnet/7.0). Once you have it installed, you can try out the new features by creating or opening a C# project and setting the `LangVersion` to `Preview`.

This Visual Studio preview gets us closer to the complete feature set for C# 11. We've continued to invest across multiple themes in this release. We've made corrections along the way based on the feedback you've already given us. Now is a great time to download the preview, try all the new features, and give us feedback. We're listening and making final updates for C# 11 and .NET 7.
