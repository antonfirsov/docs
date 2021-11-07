---
post_title: 'Welcome to C# 10' 
username: kdollard@microsoft.com
microsoft_alias: kdollard
featured_image: csharp-image.png
categories: C#, .NET 
summary: Check out the great new features in C# 10
desired_publication_date: '2021-11-8'
---

Today, we are happy to announce the release of C# 10 as part of .NET 6 and Visual Studio 2022. In this post, we're covering a lot of the new C# 10 features that make your code prettier, more expressive, and faster.

Read the [Visual Studio 2022 announcement](https://aka.ms/vs2022gablog) and the [.NET 6 announcement](https://aka.ms/dotnet6-GA) to find out more, including how to install.

## Global and implicit usings

`using` directives simplify how you work with namespaces. C# 10 includes a new `global using` directive and _implicit usings_ to reduce the number of usings you need to specify at the top of each file.

### Global using directives

If the keyword `global` appears prior to a `using` directive, that using applies to the entire project:

```c#
global using System;
```

You can use any feature of `using` within a `global using` directive. For example, adding `static` imports a type and makes the type's members and nested types available throughout your project. If you use an alias in your using directive, that alias will also affect your entire project:

```c#
global using static System.Console;
global using Env = System.Environment;
```

You can put global usings in any `.cs` file, including `Program.cs` or a specifically named file like `globalusings.cs`. The scope of global usings is the current compilation, which generally corresponds to the current project.

For more information, see [global using directives](https://docs.microsoft.com/dotnet/csharp/language-reference/keywords/using-directive#global-modifier).

### Implicit usings

The Implicit usings feature automatically adds common `global using` directives for the type of project you are building. To enable implicit usings set the `ImplicitUsings` property in your `.csproj` file:

```xml
<PropertyGroup>
    <!-- Other properties like OutputType and TargetFramework -->
    <ImplicitUsings>enable</ImplicitUsings>
</PropertyGroup>
```

Implicit usings are enabled in the new .NET 6 templates. Read more about the changes to the .NET 6 templates at this [blog post](https://devblogs.microsoft.com/dotnet/announcing-net-6-preview-7/#net-sdk-c-project-templates-modernized).

The specific set of `global using` directives included depend on the type of application you are building. For example, implicit usings for a console application or a class library are different than those for an ASP.NET application.

For more information, see this [implicit usings](https://aka.ms/csharp-implicit-usings) article.

### Combining using features

Traditional `using` directives at the top of your files, global `using` directives, and implicit usings work well together. Implicit usings let you include the .NET namespaces appropriate to the kind of project you're building with a single line in your project file. `global using` directives let you include additional namespaces to make them available throughout your project. The `using` directives at the top of your code files let you include namespaces used by just a few files in your project.

Regardless of how they are defined, extra `using` directives increase the possibility of ambiguity in name resolution. If you encounter this, consider adding an alias or reducing the number of namespaces you are importing. For example, you can replace `global using` directives with explicit using directives at the top of a subset of files.

If you need to remove namespaces that have been included via implicit usings, you can specify them in your project file:

```xml
<ItemGroup>
  <Using Remove="System.Threading.Tasks" />
</ItemGroup>
```

You can also add namespace that behave as though they were `global using` directives, you can add `Using` items to your project file, for example:

 ```xml
<ItemGroup>
  <Using Include="System.IO.Pipes" />
</ItemGroup>
 ```

## File-scoped namespaces

Many files contain code for a single namespace. Starting in C# 10, you can include a namespace as a statement, followed by a semi-colon and without the curly brackets:

```c#
namespace MyCompany.MyNamespace;

class MyClass // Note: no indentation
{ ... } 
```

This simplifies the code and removes a level of nesting. Only one file-scoped namespace declaration is allowed, and it must come before any types are declared.

For more information about file-scoped namespaces, see the [namespace keyword](https://docs.microsoft.com/dotnet/csharp/language-reference/keywords/namespace) article.

## Improvements for lambda expressions and method groups

We've made several improvements to both the types and the syntax surrounding lambdas. We expect these to be widely useful, and one of the driving scenarios has been to make [ASP.NET Minimal APIs](https://devblogs.microsoft.com/dotnet/announcing-aspnetcore-in-dotnet-6) even more straightforward.

### Natural types for lambdas

Lambda expressions now sometimes have a "natural" type. This means that the compiler can often infer the type of the lambda expression.

Up until now a lambda expression had to be converted to a delegate or an expression type. For most purposes you'd use one of the overloaded `Func<...>` or `Action<...>` delegate types in the BCL:

``` c#
Func<string, int> parse = (string s) => int.Parse(s);
```

Starting with C# 10, however, if a lambda does not have such a "target type" we will try to compute one for you:

``` c#
var parse = (string s) => int.Parse(s);
```

You can hover over `var parse` in your favorite editor and see that the type is still `Func<string, int>`. In general, the compiler will use an available `Func` or `Action` delegate, if a suitable one exists. Otherwise, it will synthesize a delegate type (for example, when you have `ref` parameters or have a large number of parameters).

Not all lambdas have natural types - some just don't have enough type information. For instance, leaving off parameter types will leave the compiler unable to decide which delegate type to use:

``` c#
var parse = s => int.Parse(s); // ERROR: Not enough type info in the lambda
```

The natural type of lambdas means that they can be assigned to a weaker type, such as `object` or `Delegate`:

``` c#
object parse = (string s) => int.Parse(s);   // Func<string, int>
Delegate parse = (string s) => int.Parse(s); // Func<string, int>
```

When it comes to expression trees we do a combination of "target" and "natural" typing. If the target type is `LambdaExpression` or the non-generic `Expression` (base type for all expression trees) *and* the lambda has a natural delegate type `D` we will instead produce an `Expression<D>`:

``` c#
LambdaExpression parseExpr = (string s) => int.Parse(s); // Expression<Func<string, int>>
Expression parseExpr = (string s) => int.Parse(s);       // Expression<Func<string, int>>
```

### Natural types for method groups

Method groups (that is, method names without argument lists) now also sometimes have a natural type. You have always been able to convert a method group to a compatible delegate type:

``` c#
Func<int> read = Console.Read;
Action<string> write = Console.Write;
```

Now, if the method group has just one overload it will have a natural type:

``` c#
var read = Console.Read; // Just one overload; Func<int> inferred
var write = Console.Write; // ERROR: Multiple overloads, can't choose
```

### Return types for lambdas

In the previous examples, the return type of the lambda expression was obvious and was just being inferred. That isn't always the case:

``` c#
var choose = (bool b) => b ? 1 : "two"; // ERROR: Can't infer return type
```

In C# 10, you can specify an explicit return type on a lambda expression, just like you do on a method or a local function. The return type goes right before the parameters. When you specify an explicit return type, the parameters must be parenthesized, so that it's not too confusing to the compiler or other developers:

``` c#
var choose = object (bool b) => b ? 1 : "two"; // Func<bool, object>
```

### Attributes on lambdas

Starting in C# 10, you can put attributes on lambda expressions in the same way you do for methods and local functions. They go right where you expect; at the beginning. Once again, the lambda's parameter list must be parenthesized when there are attributes:

``` c#
Func<string, int> parse = [Example(1)] (s) => int.Parse(s);
var choose = [Example(2)][Example(3)] object (bool b) => b ? 1 : "two";
```

Just like local functions, attributes can be applied to lambdas if they are valid on `AttributeTargets.Method`.

Lambdas are invoked differently than methods and local functions, and as a result attributes do not have any effect when the lambda is invoked. However, attributes on lambdas are still useful for code analysis, and they are also emitted on the methods that the compiler generates under the hood for lambdas, so they can be discovered via reflection.

## Improvements to structs

C# 10 introduces features for structs that provide better parity between structs and classes. These new features include parameterless constructors, field initializers, record structs and `with` expressions.

### Parameterless struct constructors and field initializers

Prior to C# 10, every struct had an implicit public parameterless constructor that set the struct's fields to `default`. It was an error for you to create a parameterless constructor on a struct.

Starting in C# 10, you can include your own parameterless struct constructors. If you don’t supply one, the implicit parameterless constructor will be supplied to set all fields to their default. Parameterless constructors you create in structs must be public and cannot be partial:

```c#
public struct Address
{
    public Address()
    {
        City = "<unknown>";
    }
    public string City { get; init; }
}

```

You can initialize fields in a parameterless constructor as above, or you can initialize them via field or property initializers:

```c#
public struct Address
{
    public string City { get; init; } = "<unknown>";
}
```

Structs that are created via `default` or as part of array allocation ignore explicit parameterless constructors, and always set struct members to their default values. For more information about parameterless constructors in structs, see the [struct type](https://docs.microsoft.com/dotnet/csharp/language-reference/builtin-types/struct#parameterless-constructors-and-field-initializers).

### record structs

Starting in C# 10, records can now be defined with `record struct`. These are similar to record classes that were introduced in C# 9:

```c#
public record struct Person
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
}
```

You can continue to define record classes with `record`, or you can use `record class` for clarity.

Structs already had value equality - when you compare them it is by value. Record structs add `IEquatable<T>` support and the `==` operator. Record structs provide a custom implementation of `IEquatable<T>` to avoid the performance issues of reflection, and they include record features like a `ToString()` override. 

Record structs can be _positional_, with a primary constructor implicitly declaring public members:

```c#
public record struct Person(string FirstName, string LastName);
```

The parameters of the primary constructor become public auto-implemented properties of the record struct. Unlike record classes, the implicitly created properties are read/write. This makes it easier to convert tuples to named types. Changing return types from a tuple like `(string FirstName, string LastName)` to a named type of `Person` can clean up your code and guarantee consistent member names. Declaring the positional record struct is easy and keeps the mutable semantics.

If you declare a property or field with the same name as a primary constructor parameter, no auto-property will be synthesized and yours will be used. 

To create an immutable record struct, add `readonly`  to the struct (as you can to any struct) or apply `readonly` to individual properties. Object initializers are part of the construction phase where readonly properties can be set. Here is just one of the ways you can work with immutable record structs:

```c#
var person = new Person { FirstName = "Mads", LastName = "Torgersen"};

public readonly record struct Person
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
}
```

Find out more about [record structs in this article](https://docs.microsoft.com/dotnet/csharp/language-reference/builtin-types/record).

### `sealed` modifier on `ToString()` in record classes

Record classes have also been improved. Starting in C# 10 the `ToString()` method can include the sealed modifier, which prevents the compiler from synthesizing a `ToString` implementation for any derived records.

Find out more about `ToString()` in records [in this article](https://docs.microsoft.com/dotnet/csharp/language-reference/builtin-types/record#built-in-formatting-for-display).

### `with` expressions on structs and anonymous types

C# 10 supports `with` expressions for all structs, including record structs, as well as for anonymous types:

```c#
var person2 = person with { LastName = "Kristensen" };
```

This returns a new instance with the new value. You can update any number of values. Values you do not set will retain the same value as the initial instance.

Learn more about `with` [in this article](https://docs.microsoft.com//dotnet/csharp/language-reference/operators/with-expression)

## Interpolated string improvements

When we added interpolated strings to C# we always felt that there was more that could be done with that syntax down the line, both for performance and expressiveness. With C# 10 that time has come!

### Interpolated string handlers

Today the compiler turns interpolated strings into a call to `string.Format`. This can lead to a lot of allocations - the boxing of arguments, allocation of an argument array, and of course the resulting string itself. Also, it leaves no wiggle room in the meaning of the actual interpolation.

In C# 10 we've added a library pattern that allows an API to "take over" the handling of an interpolated string argument expression. As an example, consider `StringBuilder.Append`:

``` c#
var sb = new StringBuilder();
sb.Append($"Hello {args[0]}, how are you?");
```

Up until now, this would call the `Append(string? value)` overload with a newly allocated and computed string, appending that to the `StringBuilder` in one chunk. However, `Append` now has a new overload `Append(ref StringBuilder.AppendInterpolatedStringHandler handler)` which takes precedence over the string overload when an interpolated string is used as argument.

In general, when you see parameter types of the form `SomethingInterpolatedStringHandler` the API author has done some work behind the scenes to handle interpolated strings more appropriately for their purposes. In the case of our `Append` example, the strings `"Hello "`, `args[0]` and `", how are you?"` will be individually appended to the `StringBuilder`, which is much more efficient and has the same outcome.

Sometimes you want to do the work of building the string only under certain conditions. An example is `Debug.Assert`:

``` c#
Debug.Assert(condition, $"{SomethingExpensiveHappensHere()");
```

In most cases, the condition will be true and the second parameter is unused. However, all of the arguments are computed on every call, needlessly slowing down execution. `Debug.Assert` now has an overload with a custom interpolated string builder, which ensures that the second argument isn't even evaluated unless the condition is false.

Finally, here's an example of actually changing the behavior of string interpolation in a given call: `String.Create()` lets you specify the `IFormatProvider` used to format the expressions in the holes of the interpolated string argument itself:

``` c#
String.Create(CultureInfo.InvariantCulture, $"The result is {result}");
```

You can learn more about interpolated string handlers, [in this article](https://docs.microsoft.com/dotnet/csharp/language-reference/tokens/interpolated#compilation-of-interpolated-strings) and this [tutorial](https://docs.microsoft.com/dotnet/csharp/whats-new/tutorials/interpolated-string-handler) on creating a custom handler.

### Constant interpolated strings

If all the holes of an interpolated string are constant strings, then the resulting string is now also constant. This lets you use string interpolation syntax in more places, like attributes:

``` c#
[Obsolete($"Call {nameof(Discard)} instead")]
```

Note that the holes must be filled with constant _strings_. Other types, like numeric or date values, cannot be used because they are sensitive to `Culture`, and can't be computed at compile time.

## Other improvements

C# 10 has a number of smaller improvements across the language. Some of these just make C# work in the way you expect.

### Mix declarations and variables in deconstruction

Prior to C# 10, deconstruction required all variables to be new, or all of them to be previously declared. In C# 10, you can mix:

```c#
int x2;
int y2;
(x2, y2) = (0, 1);       // Works in C# 9
(var x, var y) = (0, 1); // Works in C# 9
(x2, var y3) = (0, 1);   // Works in C# 10 onwards
```

Find out more in the article on [deconstruction]( https://docs.microsoft.com/dotnet/csharp/fundamentals/functional/deconstruct).

### Improved definite assignment

C# produces errors if you use a value that has not been definitely assigned. C# 10 understands your code better and produces less spurious errors. These same improvements also mean you'll see less spurious errors and warnings for null references.

Find out more about C# definite assignment in the [what's new in C# 10 article](https://docs.microsoft.com/dotnet/csharp/whats-new/csharp-10#improved-definite-assignment).

### Extended property patterns

C# 10 adds extended property patterns to make it easier to access nested property values in patterns. For example, if we add an address to the `Person` record above, we can pattern match in both of the ways shown here:

```c#
object obj = new Person
{
    FirstName = "Kathleen",
    LastName = "Dollard",
    Address = new Address { City = "Seattle" }
};

if (obj is Person { Address: { City: "Seattle" } })
    Console.WriteLine("Seattle");

if (obj is Person { Address.City: "Seattle" }) // Extended property pattern
    Console.WriteLine("Seattle");
```

The extended property pattern simplifies the code and makes it easier to read, particularly when matching against multiple properties.

Find out more about extended property patterns in the [pattern matching article](https://docs.microsoft.com/dotnet/csharp/language-reference/operators/patterns#property-pattern).

### Caller expression attribute

`CallerArgumentExpressionAttribute` supplies information about the context of a method call. Like the other CompilerServices attributes, this attribute is applied to an optional parameter. In this case, a string:

```c#
void CheckExpression(bool condition, 
    [CallerArgumentExpression("condition")] string? message = null )
{
    Console.WriteLine($"Condition: {message}");
}
```

The parameter name passed to `CallerArgumentExpression` is the name of a different parameter. The expression passed as the argument to that parameter will be contained in the string. For example,

```c#
var a = 6;
var b = true;
CheckExpression(true);
CheckExpression(b);
CheckExpression(a > 5);

// Output:
// Condition: true
// Condition: b
// Condition: a > 5
```
A good example of how this attribute can be used is ArgumentNullException.ThrowIfNull(). It avoids have to pass in the parameter name by defaulting it from the provided value:

```C#
void MyMethod(object value)
{
    ArgumentNullException.ThrowIfNull(value);
}
```

Find out more about [CallerArgumentExpressionAttribute](https://docs.microsoft.com/dotnet/csharp/language-reference/attributes/caller-information#argument-expressions)

## Preview features

C# 10 GA includes _static abstract members in interfaces_ as a preview feature. Rolling out a preview feature in GA allows us to get feedback on a feature that will take longer than a single release to create. _Static abstract members in interfaces_ is the basis for a new set of generic math constraints that allow you to abstract over which operators are available. You can read more about [generic math constraints in this article](https://devblogs.microsoft.com/dotnet/preview-features-in-net-6-generic-math/).

## Closing

Install .NET 6 or Visual Studio 2022, enjoy C# 10, and tell us what you think!

- Kathleen Dollard (PM for the .NET Languages) and Mads Torgersen (C# Lead Designer)
