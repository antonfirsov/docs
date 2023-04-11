---
post_title: Check out new C# 12 preview features!
author1: kathleen.a.dollard
post_slug: check-out-csharp-12-preview
username: kathleen.a.dollard
microsoft_alias: kdollard
featured_image: csharp12.png
categories: .NET, C#
tags: .net 8, c# 12
summary: The first set of C# 12 features are here in preview including primary constructors, using aliases, and lambda expression parameters.
desired_publication_date: 2023-04-11
post_date: 2023-04-11 10:15:00
---

We're excited to preview three new features for C# 12:

* Primary constructors for non-record classes and structs
* Using aliases for any type
* Default values for lambda expression parameters

In addition to this overview, you can also find detailed documentation in the [What’s new in C# article on Microsoft Learn](https://learn.microsoft.com/dotnet/csharp/whats-new/csharp-12). 

To test out these features yourself, you can download the latest  [Visual Studio 17.6 preview](https://visualstudio.com/preview) or the latest [.NET 8 preview](https://dotnet.microsoft.com/download/dotnet/8.0). Find out what else is coming for developers in [Announcing .NET 8 Preview 3](https://devblogs.microsoft.com/dotnet/announcing-dotnet-8-preview-3) and other posts on the .NET blog.

## Primary constructors for non-record classes and structs

Primary constructors let you add parameters to the class declaration itself and use these values in the class body. For example, you could use the parameters to initialize properties or in the code of methods and property accessors. Primary constructors were introduced for records in C# 9 as part of the positional syntax for records. C# 12 extends them to all classes and structs.

The basic syntax and usage for a primary constructor is:

```csharp
public class Student(int id, string name, IEnumerable<decimal> grades)
{
    public Student(int id, string name) : this(id, name, Enumerable.Empty<decimal>()) { }
    public int Id => id;
    public string Name { get; set; } = name.Trim();
    public decimal GPA => grades.Any() ? grades.Average() : 4.0m;
} 
```

The primary constructor parameters in the `Student` class above are available throughout the body of the class. One way you can use them is to initialize properties. Unlike records, properties are not automatically created for primary constructor parameters in non-record classes and structs. This reflects that non-record classes and structs often have more complexity than records, combining data and behavior. As a result, they often need constructor parameters that should not be exposed. Explicitly creating properties makes it obvious which data is exposed, consistent with the common use of classes. Primary constructors help avoid the boilerplate of declaring private fields and having trivial constructor bodies assign parameter values to those fields.

When primary constructor parameters are used in methods or property accessors (the `grades` parameter in the `Student` class), they need to be _captured_ in order for them to stay around after the constructor is done executing. This is similar to how parameters and local variables are captured in lambda expressions. For primary constructor parameters, the capture is implemented by generating a private backing field on the class or struct itself. The field has an “unspeakable” name, which means it will not collide with other naming and is not obvious via reflection. Consider how you assign and use primary constructor parameters to avoid double storage. For example, `name` is used to initialize the auto-property `Name`, which has its own backing field. If another member referenced the parameter `name` directly, it would also be stored in its own backing field, leading to an unfortunate duplication.

A class with a primary constructor can have additional constructors. Additional constructors must use a `this(…)` initializer to call another constructor on the same class or struct. This ensures that the primary constructor is always called and all the all the data necessary to create the class is present. A struct type always has a parameterless constructor. The implicit parameterless constructor doesn’t use a `this()` initializer to call the primary constructor. In the case of a struct, you must write an explicit parameterless constructor to do if you want the primary constructor called.

Find out more in the [What's new in C# 12](https://learn.microsoft.com/dotnet/csharp/whats-new/csharp-12#primary-constructors) article with links to all new content for primary constructors.

You can leave feedback on primary constructors in the CSharpLang GitHub repository at [Preview Feedback: C# 12 Primary constructors](https://github.com/dotnet/csharplang/discussions/7109).

## Using directives for additional types

C# 12 extends using directive support to any type. Here are a few examples:

```csharp
using Measurement = (string, int);
using PathOfPoints = int[];
using DatabaseInt = int?;
```

You can now alias almost any type. You can alias nullable value types, although you cannot alias nullable reference types. Tuples are particularly exciting because you can include element names and types:
```csharp
using Measurement = (string Units, int Distance);
```

You can use aliases anywhere you would use a type. For example:

```csharp
public void F(Measurement x)
{ }
```

Aliasing types lets you abstract the actual types you are using and lets you give friendly names to confusing or long generic names. This can make it easier to read your code.

Find out more in the [What's new in C# 12](https://learn.microsoft.com/dotnet/csharp/whats-new/csharp-12#alias-any-type) article.

You can leave feedback on aliases for any type in the CSharpLang GitHub repository at [Preview Feedback: C# 12 Alias any type](https://github.com/dotnet/csharplang/discussions/7110).


## Default values for lambda expressions

C# 12 takes the next step in empowering lambda expressions by letting you specify default values for parameters. The syntax is the same as for other default parameters: 

```csharp
var addWithDefault = (int addTo = 2) => addTo + 1;
addWithDefault(); // 3
addWithDefault(5); // 6
```

Similar to other default values, the default value will be emitted in metadata and is available via reflection as the `DefaultValue` of the `ParameterInfo` of the lambda's `Method` property. For example:


```csharp
var addWithDefault = (int addTo = 2) => addTo + 1;
addWithDefault.Method.GetParameters()[0].DefaultValue; // 2
```

Prior to C# 12 you needed to use a local function or the unwieldy `DefaultParameterValue`
from the `System.Runtime.InteropServices` namespace to provide a default value for lambda expression parameters. These approaches still work but are harder to read and are inconsistent with default values on methods. With the new default values on lambdas you’ll have a consistent look for default parameter values on methods, constructors and lambda expressions.

Find out more in the article on [What's new in C# 12](https://learn.microsoft.com/dotnet/csharp/whats-new/csharp-12#default-lambda-parameters).

You can leave feedback on default values for lambda parameters in the CSharpLang GitHub repository at [Preview Feedback: C# 12 Default values in lambda expressions](https://github.com/dotnet/csharplang/discussions/7111).

## Next steps

We hope you will download the preview and check out these features. We are experimenting in C# 12 with a dedicated issue for each feature. We4 hope this will focus feedback and make it easier for you to upvote what other people are saying. You can find these at [Preview Feedback: C# 12 Primary constructors](https://github.com/dotnet/csharplang/discussions/7109), [Preview Feedback: C# 12 Alias any type](https://github.com/dotnet/csharplang/discussions/7110), and [Preview Feedback: C# 12 Default values in lambda expressions](https://github.com/dotnet/csharplang/discussions/7111). 


You can follow our implementation progress for C# 12 at [Roslyn Feature Status](https://github.com/dotnet/roslyn/blob/main/docs/Language%20Feature%20Status.md). You can also follow the design process at the [CSharpLang GitHub repository](https://github.com/dotnet/csharplang) where you will find proposals, discussions and meeting notes.

We look forward to hearing from you!