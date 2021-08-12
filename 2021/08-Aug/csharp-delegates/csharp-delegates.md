---
post_title: Understanding the cost of C# delegates
username: pamorgad
microsoft_alias: pamorgad@microsoft.com
categories: .NET Core, .NET, C#
desired_publication_date: 2021-08-17
summary: Delegates are widely used in C# (and .NET, in general), but it's not always obvious to the developer what code they write ends up generating. In this post, I'll show the various forms to make you aware of their costs.
---

[Delegates](https://docs.microsoft.com/dotnet/csharp/delegates-overview "Delegates") are widely used in C# (and .NET, in general). Either as event handlers, callbacks, or as logic to be used by other code (as in [LINQ](https://docs.microsoft.com/dotnet/csharp/linq/ "Language Integrated Query (LINQ)")).

Despite their wide usage, it's not always obvious to the developer what [delegate instantiation](https://docs.microsoft.com/dotnet/csharp/language-reference/language-specification/delegates#delegate-instantiation "Delegate instantiation") will look like. In this post, I'm going to show various usages of delegates and what code they generate so that you can see the costs associated with using them in your code.

## Explicit instantiation

Throughout the evolution of the C# language, delegate invocation has evolved with new patterns without breaking the previously existing patterns.

Initially (versions 1.0 and 1.2), the only instantiation pattern available was the explicit invocation of the delegate type constructor with a method group:

```csharp
delegate void D(int x);

class C
{
    public static void M1(int i) {...}
    public void M2(int i) {...}
}

class Test
{
    static void Main() {
        D cd1 = new D(C.M1);        // static method
        C t = new C();
        D cd2 = new D(t.M2);        // instance method
        D cd3 = new D(cd2);         // another delegate
    }
}
```

## Implicit conversion

C# 2.0 introduced [method group conversions](https://docs.microsoft.com/dotnet/csharp/language-reference/language-specification/conversions#method-group-conversions) where an implicit conversion ([Implicit conversions](https://docs.microsoft.com/dotnet/csharp/language-reference/language-specification/conversions#implicit-conversions)) exists from a method group ([Expression classifications](https://docs.microsoft.com/dotnet/csharp/language-reference/language-specification/expressions#expression-classifications)) to a compatible delegate type.

This allowed for short-hand instantiation of delegates:

```csharp
delegate string D1(object o);

delegate object D2(string s);

delegate object D3();

delegate string D4(object o, params object[] a);

delegate string D5(int i);

class Test
{
    static string F(object o) {...}

    static void G() {
        D1 d1 = F;            // Ok
        D2 d2 = F;            // Ok
        D3 d3 = F;            // Error -- not applicable
        D4 d4 = F;            // Error -- not applicable in normal form
        D5 d5 = F;            // Error -- applicable but not compatible

    }
}
```

The assignment to `d1` implicitly converts the method group `F` to a value of type `D1`.

The assignment to `d2` shows how it is possible to create a delegate to a method that has less derived (contravariant) parameter types and a more derived (covariant) return type.

The assignment to `d3` shows how no conversion exists if the method is not applicable.

The assignment to `d4` shows how the method must be applicable in its normal form.

The assignment to `d5` shows how parameter and return types of the delegate and method are allowed to differ only for reference types.

The compiler will translate the above code to:

```csharp
delegate string D1(object o);

delegate object D2(string s);

delegate object D3();

delegate string D4(object o, params object[] a);

delegate string D5(int i);

class Test
{
    static string F(object o) {...}

    static void G() {
        D1 d1 = new D1(F);            // Ok
        D2 d2 = new D2(F);            // Ok
        D3 d3 = new D3(F);            // Error -- not applicable
        D4 d4 = new D4(F);            // Error -- not applicable in normal form
        D5 d5 = new D5(F);            // Error -- applicable but not compatible

    }
}
```

As with all other implicit and explicit conversions, the cast operator can be used to explicitly perform a method group conversion. Thus, this code:

```csharp
object obj = (EventHandler)myDialog.OkClick;
```

will be converted by the compiler to:

```csharp
object obj = new EventHandler(myDialog.OkClick);
```

This instantiation pattern might create a performance issue in loops or frequently invoke code.

This innocent looking code:

```csharp
static void Sort(string[] lines)
{
    Array.Sort(lines, comparison);
}

...
Sort(lines, StringComparer.OrdinalIgnoreCase.Compare);
...
```

Will be translated to:

```csharp
static void Sort(string[] lines)
{
    Array.Sort(lines, comparison);
}

...
Sort(lines, new Comparison<string>(StringComparer.OrdinalIgnoreCase.Compare));
...
```

Which means that an instance of the delegate will be created on every invocation. A delegate instance that will have to be later collected by the garbage collector (GC).

One way to avoid this repeated instantiation of delegates is to pre-instantiate it:

```csharp
static void Sort(string[] lines)
{
    Array.Sort(lines, comparison);
}

...
private static Comparison<string> OrdinalIgnoreCaseComparison = StringComparer.OrdinalIgnoreCase.Compare;
...

...
Sort(lines, OrdinalIgnoreCaseComparison);
...
```

Which will be translated by the compiler to:

```csharp
static void Sort(string[] lines)
{
    Array.Sort(lines, comparison);
}

...
private static Comparison<string> OrdinalIgnoreCaseComparison = new Comparison<string>(StringComparer.OrdinalIgnoreCase.Compare);
...

...
Sort(lines, OrdinalIgnoreCaseComparison);
...
```

Now, only one instance of the delegate will be created.

## Anonymous functions

C# 2.0 also introduced the concept of [anonymous method expressions](https://docs.microsoft.com/dotnet/csharp/programming-guide/statements-expressions-operators/anonymous-functions "Anonymous functions (C# Programming Guide)") as a way to write unnamed inline statement blocks that can be executed in a delegate invocation.

Like a method group, an [anonymous function expression](https://docs.microsoft.com/dotnet/csharp/language-reference/language-specification/expressions#anonymous-function-expressions "anonymous function expression") can be implicitly converted to a compatible delegate.

C# 3.0 introduced the possibility of declaring anonymous functions using lambda expressions.

Being a new language concept allowed the compiler designers to interpret the expressions in new ways.

The compiler can generate a static method and optimize the delegate creation if the expression has no external dependencies:

This code:

```csharp
int ExecuteOperation(int a, int b, Func<int, int, int> operation)
{
    return operation(a, b);
}

...
var r = ExecuteOperation(2, 3, (a, b) => a + b);
...
```

Will be translated to:

```csharp
[Serializable]
[CompilerGenerated]
private sealed class <>c
{
      public static readonly <>c <>9 = new <>c();

      public static Func<int, int, int> <>9__4_0;

      internal int <M>b__4_0(int a, int b)
      {
            return a + b;
      }
}
...

int ExecuteOperation(int a, int b, Func<int, int, int> operation)
{
    return operation(a, b);
}

...
var r = ExecuteOperation(2, 3, <>c.<>9__4_0 ?? (<>c.<>9__4_0 = new Func<int, int, int>(<>c.<>9.<M>b__4_0)));
...
```

The compiler is, now, “smart” enough to instantiate the delegate only on the first use.

As you can see, the member names generated by the C# compiler are not valid C# identifiers. They are valid IL identifiers, though. The reason the compiler generates names like this is to avoid name collisions with user code. There is no was to write C# source code that will have identifiers with `<` or `>`.

This optimization is only possible because the operation is a static function. If, instead, the code was like this:

```csharp
int ExecuteOperation(int a, int b, Func<int, int, int> operation)
{
    return operation(a, b);
}

int Add(int a, int b) => a + b;

...
var r = ExecuteOperation(2, 3, (a, b) => Add(a, b));
...
```

We’d be back to a delegate instantiation for every invocation:

```csharp
int ExecuteOperation(int a, int b, Func<int, int, int> operation)
{
    return operation(a, b);
}

int Add(int a, int b) => a + b;

int <M>b__4_0(int a, int b) => Add(a, b);

...
var r = ExecuteOperation (2, 3, new Func<int, int, int> (<M>b__4_0));
...
```

This is due to the operation being dependent on the instance invoking the operation.

On the other hand, if the operation is a static function:

```csharp
int ExecuteOperation(int a, int b, Func<int, int, int> operation)
{
    return operation(a, b);
}

static int Add(int a, int b) => a + b;

...
var r = ExecuteOperation(2, 3, (a, b) => Add(a, b));
...
```

The compiler is clever enough to optimize the code:

```csharp
[Serializable]
[CompilerGenerated]
private sealed class <>c
{
      public static readonly <>c <>9 = new <>c();

      public static Func<int, int, int> <>9__4_0;

      internal int <M>b__4_0(int a, int b)
      {
            return Add(a, b);
      }
}
...

int ExecuteOperation(int a, int b, Func<int, int, int> operation)
{
    return operation(a, b);
}

static int Add(int a, int b) => a + b;

...
var r = ExecuteOperation(2, 3, <>c.<>9__4_0 ?? (<>c.<>9__4_0 = new Func<int, int, int>(<>c.<>9.<M>b__4_0)));
...
```

## Closures

Whenever a lambda (or anonymous) expression references a value outside of the expression, a closure class will always be created to hold that value, even if the expression would, otherwise, be static.

This code:

```csharp
int ExecuteOperation(int a, int b, Func<int, int, int> operation)
{
    return operation(a, b);
}

static int Add(int a, int b) => a + b;

...
var o = GetOffset();
var r = ExecuteOperation(2, 3, (a, b) => Add(a, b) + o);
...
```

Wiil cause the compiler to generate this code:

```csharp
[CompilerGenerated]
private sealed class <>c__DisplayClass4_0
{
      public int o;

      internal int <N>b__0(int a, int b)
      {
            return Add(a, b) + o;
      }
}

int ExecuteOperation(int a, int b, Func<int, int, int> operation)
{
    return operation(a, b);
}

static int Add(int a, int b) => a + b;

...
<>c__DisplayClass4_0 <>c__DisplayClass4_ = new <>c__DisplayClass4_0();
<>c__DisplayClass4_.o = GetOffset();
ExecuteOperation(2, 3, new Func<int, int, int>(<>c__DisplayClass4_.<M>b__0));
...
```

Now, not only a new delegate will be instantiated, an instance of class to hold the dependent value. This compiler generated field to capture the variables is what is called in computer science a [closure](https://simple.wikipedia.org/wiki/Closure_%28computer_science%29).

Closures allow the generated function to access the variables in the scope where they were defined.

However, by capturing the local environment or context, closure can unexpectedly hold a reference to resources that would otherwise be collected sooner causing them to be promoted to highier generations and, thus, incur in more CPU load because of the work the [garbage collector (GC)](https://docs.microsoft.com/dotnet/standard/garbage-collection/ "Garbage collection") needs to perform to reclaim that memory.

## Static anonymous functions

Because it’s very easy to write a lambda expression that starts with the intention of being static and ends up being not static, C# 9.0 introduces [static anonymous functions](https://docs.microsoft.com/dotnet/csharp/language-reference/proposals/csharp-9.0/static-anonymous-functions "Static anonymous functions") by allowing the `static` modifier to be applied to a lambda (or anonymous) expression to ensure that the expression is static:

```csharp
var r = ExecuteOperation(2, 3, static (a, b) => Add(a, b));
```

If the same changes above are made, now the compiler will “complain”:

```csharp
var o = GetOffset();
var r = ExecuteOperation(2, 3, static (a, b) => Add(a, b) + o); // Error CS8820: A static anonymous function cannot contain a reference to 'o'
```

## Workarounds

What can a developer do to avoid these unwanted intantiations?

We’ve seen what the compiler does, so, we can do the same.

With this small change to the code:

```csharp
int ExecuteOperation(int a, int b, Func<int, int, int> operation)
{
    return operation(a, b);
}

int Add(int a, int b) => a + b;
Func<int, int, int> addDelegate;

...
var r = ExecuteOperation(2, 3, addDelegate ?? (addDelegate = (a, b) => Add(a, b));
...
```

The only thing the compiler will have to do now is add the delegate instantiation, but the same instance of the delegate will be used throughout the lifetime of the enclosing type.

```csharp
int ExecuteOperation(int a, int b, Func<int, int, int> operation)
{
    return operation(a, b);
}

int Add(int a, int b) => a + b;
Func<int, int, int> addDelegate;

...
var r = ExecuteOperation(2, 3, addDelegate ?? (addDelegate = new Func<int, int, int>((a, b) => Add(a, b)));
...
```

## Closing

We've seen the different ways of using delegates and the code generated by the compiler and its side effects.

Delegates have powerful features such as capturing local variables. And while these features can make you more productive, they aren't free. Being aware of the differences in the generated code allows making informed decisions on what you value more for a given part of your application.

Instantiating a delegate more frequently can incur performance penalties by allocating more memory which also increases CPU load because of the work the garbage collector (GC) needs to perform to reclaim that memory.

For that reason, we've seen how we can control the code generated by the compiler in a way the best suits our performance needs.
