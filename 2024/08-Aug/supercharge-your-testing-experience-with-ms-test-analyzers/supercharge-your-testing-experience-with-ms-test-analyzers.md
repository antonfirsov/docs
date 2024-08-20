---
post_title: Supercharge your testing experience with MSTest.Analyzers
author1: jajares
post_slug: supercharge-your-testing-experience-with-ms-test-analyzers
microsoft_alias: jajares
featured_image: mstestanalyzers.jpg
categories: .NET, C#
tags: mstest, testing
ai_note: hide
summary: MSTest ships with extensive suite of code analyzers to help you write well-formed, performant and error free tests.
post_date: 2024-08-21 10:05:00
---

MSTest ships with a set of analyzers that inspect your test code and point out common mistakes and pitfalls. These mistakes can be subtle and lead to your tests being completely ignored by the test framework.

We’ve been shipping these analyzers since 3.2.0, but in the latest 3.5.1, we’ve added some that we think you should not miss.

## The missing test

One common problem is when you forget to put `[TestClass]` on your class. MSTest won't know that there are tests in the class, and it won't run them:

```cs

public class MyTests

{
    [TestMethod]
    public async Task TestMethod1()
    {
      Assert.Fail();
    }
}
```

Without MSTest.Analyzers, this code builds without warning or info message. There is no test failure when running the tests either. Because there is no `[TestClass]` attribute on the class, MSTest will skip over the whole class for performance reasons, and your test will never be found.

But with analyzers you get an info message during build:

![Info message for MSTEST0030 shown in Visual Studio Error List](info-1.png)

We recommend, to upgrade this message to a warning, or even build error, one such way is by adding this line to your `.editorconfig`:

```txt
[*.cs]
# MSTEST0030: Type containing '[TestMethod]' should be marked with '[TestClass]'
dotnet_diagnostic.MSTEST0030.severity = warning
```

![Warning message for MSTEST0030 shown in Visual Studio Error List](warning-1.png)

### Why is this not a warning by default?

You might be asking why `MSTEST0030` is an info message and not warning by default. The reason is that we cannot introduce breaking changes in MSTest v3, and the code above is a common pattern for re-using tests from a base class.

```cs
public class MyTestsBase
{
    [TestMethod]
    public async Task CommonTestMethod()
    {
    }
}

[TestClass]
public class MyTests : MyTestsBase
{
    [TestMethod]
    public async Task TestMethod1()
    {
      Assert.Fail();
    }
}
```

In the example above, the test `CommonTestMethod` will not run from `MyTestsBase`, because it does not have `[TestClass]` attribute, but it will be inherited to `MyTests`, and will run there.


We **DO NOT** recommend this pattern. Instead, we recommend to always mark classes with `[TestClass]`, and make the base class abstract, if you don’t want to run tests from it.

```cs
[TestClass]
public abstract class MyTestsBase
{
    [TestMethod]
    public async Task CommonTestMethod()
    {
    }
}

[TestClass]
public class MyTests : MyTestsBase
{
    [TestMethod]
    public async Task TestMethod1()
    {
      Assert.Fail();
    }
}
```

This approach behaves identically to the one above, but you communicate it clearly to the analyzers and the testing framework that the `abstract` base class is holding a shared logic, and should not run by itself.

## Malformed AssemblyInitialize

Another example of a useful analyzer is fixing the signature of `[AssemblyInitialize]`, to do a one-time setup for all the tests in the assembly.

I don't know about you, but I can't remember the signature of this method. And when I get it wrong my tests don't run at all. This is especially annoying in Visual Studio, where the test simply remains blue, and I need to go into the `Tests` output to find the reason.

But with the analyzers, I can easily find out what is wrong, and there is even an automatic fix that I can apply to my code.

Here I write method Setup, and VisualStudio will underline it with a warning, and I press `Ctrl+.` to see the automatic fix and apply it:

```cs
[TestClass]
public class MyTests
{
  [AssemblyInitialize]
  public void Setup()
  {
  }
}
```

![Warning message for MSTEST0012 shown in Visual Studio Error List](warning-2.png)
![Fixing signature via lightbulb menu](code-fix.png)

Fixing my code to the correct shape that the testing framework can recognize:

```cs
 [TestClass]
 public class MyTests
 {
     [AssemblyInitialize]
     public static void Setup(TestContext context)
     {
     }
 }
```

## Installation

The recommended way to start using `MSTest.Analyzers` is by using the `MSTest` nuget package, or the MSTest project SDK, with version 3.2.0 or newer.

<https://learn.microsoft.com/dotnet/core/testing/unit-testing-mstest-getting-started>

The analyzers can also be installed separately by referencing the `MSTest.Analyzers` NuGet package.

We wholeheartedly recommend any project that is using MSTest to upgrade to version 3.2.0 and newer, and enable those analyzers.

## Summary

The two analyzers showcased in this article are the ones that I find the most useful in my day-to-day. But there is a whole lot more, such as:

- MSTEST0003, which ensures that your test method has correct signature, such as being `public`, and not being `async void`.
- MSTEST0001, which recommends enabling test parallelization, because we’ve seen multiple test bases dramatically reduce their test execution time by doing so.
- MSTEST0017, which ensures that you pass arguments to assertions in the correct order, to avoid confusing test failure messages.

And 32 more rules, that are split into Design, Performance, and Usage categories. Helping you to write well formed, performant and error free tests.

<https://learn.microsoft.com/dotnet/core/testing/mstest-analyzers/overview>

We are constantly looking for ways to improve these analyzers or new analyzers that our customers are missing. We welcome you to share your feedback, ideas for more analyzers, and your experience, on our repository [microsoft/testfx](https://github.com/microsoft/testfx/issues).
