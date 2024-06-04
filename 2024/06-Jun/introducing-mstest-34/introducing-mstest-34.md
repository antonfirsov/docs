---
post_title: MSTest 3.4 is here with WinUI support and new analyzers!
author1: amauryleve
author2: mrossignoli
post_slug: introducing-mstest-34
microsoft_alias: amauryleve
featured_image: mstest-34.png
categories: .NET, .NET Aspire, .NET Core, .NET Fundamentals, .NET Framework, C#, Developer Stories, F#, Lifecycle, Static Analysis
tags: mstest, testing
ai_note: show
summary: MSTest 3.4 is available. Learn all about the highlighted features and fixes that will make your testing experience always better.
post_date: 2024-06-05 10:05:00
---

We are excited to announce the new release of MSTest, the popular testing framework for .NET.
This release brings many enhancements and bug fixes to MSTest.Analyzers, new features and improvements for MSTest.Sdk, and adds support for WinUI applications to MSTest.Runner.

We've added `[Timeout]` support to all fixture (initialize and cleanup) methods, and STA thread support for UI tests. Both were long-standing issues that were reported by our users and community. We have also simplified testing with Playwright and Aspire by removing project boilerplate.

Here are some of the highlights of this release:

## MSTest.Analyzers

MSTest ships with a set of Roslyn based code analyzers to help you write better tests. In this release we have added 9 new rules that cover common best practices and pitfalls. These rules ensure correct usage of attributes and assertions, help enforce design preferences, and more.

* MSTEST0017: Assertion arguments should be passed in the correct order.
* MSTEST0019: Prefer `TestInitialize` over constructors. This rule suggests using `TestInitialize` methods over class constructors to ensure a consistent declaration and initialization across all the test classes, because `TestInitialize` method  allows async initialization, which class constructors don’t allow.
* MSTEST0020: Prefer constructors over `TestInitialize` methods. This rule recommends using constructors over `TestInitialize` methods for non-async initialization, because constructors allow for readonly fields and provide better compiler feedback
* MSTEST0021: Prefer Dispose over `TestCleanup` methods. This rule recommends using `Dispose` or `DisposeAsync` pattern over `TestCleanup` methods to provide easier readability for developers not used to MSTest.
* MSTEST0022: Prefer `TestCleanup` methods over `Dispose`. This rule is the opposite of `MSTEST0021` and suggests using `TestCleanup` methods for the cleanup phase, because it allows for the async pattern even in older versions of .NET. This ensures a consistent declaration no matter what the targeted framework is.
* MSTEST0023: Do not negate boolean assertions. This rule warns against negating boolean assertions, which can lead to less readable tests and make it harder to understand the test's intent.
* MSTEST0024: Do not store `TestContext` in static members. The `TestContext` instance passed to `AssemblyInitialize` and `ClassInitialize` method is tailored for these methods only and won’t be updated based on the current stage of the test execution. This means that the instance being stored will not provide any consistent state so it is better not to reuse it outside the scope of its method.
* MSTEST0025: Use `Assert.Fail` instead of an always-failing assert. This rule suggests using `Assert.Fail` to explicitly fail a test, which is clearer and more direct than using an assertion that is designed to always fail.

You can find the full list of rules and their descriptions in the [MSTest code analysis](https://learn.microsoft.com/dotnet/core/testing/mstest-analyzers/overview) documentation.
We have also fixed 3 rules that had false positives or incorrect messages.

If you have any ideas for a rule or code-fix that would make your tests easier, please feel free to open a feature request ticket on our [microsoft/testfx repository](https://github.com/microsoft/testfx).

## MSTest

We have fixed the long-standing request to add support for STA threads in MSTest for VSTest and MSTest.Runner for all the supported target frameworks, allowing for easier testing of UI elements. To enable STA thread support, set `<ExecutionThreadApartmentState>STA</ExecutionThreadApartmentState>` in your runsettings. Full runsettings below:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<RunSettings>
    <RunConfiguration>
        <ExecutionThreadApartmentState>STA</ExecutionThreadApartmentState>
    </RunConfiguration>
</RunSettings>
```

We also plan to improve the experience by allowing to have only some specific tests or test classes run in STA thread mode, please upvote and track <https://github.com/microsoft/testfx/issues/2688>.

Additionally, we have introduced the ability to define timeouts on fixture methods (AssemblyInitialize, AssemblyCleanup, ClassInitialize, ClassCleanup, TestInitialize and TestCleanup), giving you more control over the execution of your tests.
In this case I want my `[ClassCleanup]` to time out after 1 second:

![Exception dialog showing the message: Class cleanup method 'Timeout. UnitTest1. ClassCIeanup' timed out](https://github.com/microsoft/testfx/assets/5735905/517bd606-36f8-4096-9e30-e8b88580f46c)

Alternatively, timeouts can be specified through runsettings, to be applied as default value. For example, the following runsettings will add a default timeout of 1 second for all ClassCleanup methods:

```xml
<RunSettings>
  <MSTest>
    <ClassCleanupTimeout>1000</ClassCleanupTimeout>
  </MSTest>
</RunSettings>
```

Finally, we have fixed message and stack trace for exceptions thrown in fixture methods.

```cs
namespace TestProject80
{
    [TestClass]
    public class UnitTest1
    {
        [AssemblyInitialize]
        public static void Init(TestContext context)
        {
            // This will throw in ctor.
            InitHelper.A();
        }

        [TestMethod]
        public void TestMethod1()
        {
        }
    }

    public static class InitHelper
    {
        static InitHelper()
        {
            throw new Exception("AAAA");
        }

        public static void A() {  }
    }
}
```

Was producing the following message

```text
 Assembly Initialization method TestProject80.UnitTest1.Init threw exception. System.TypeInitializationException: The type initializer for 'TestProject80.InitHelper' threw an exception.. Aborting test execution.
Stack Trace:
    at TestProject80.InitHelper..cctor() in C:\src\temp\ConsoleApp4\TestProject1\UnitTest1.cs:line 29
--- End of inner exception stack trace ---
    at TestProject80.InitHelper.A() in C:\src\temp\ConsoleApp4\TestProject1\UnitTest1.cs:line 32
   at TestProject80.UnitTest1.Init(TestContext context) in C:\src\temp\ConsoleApp4\TestProject1\UnitTest1.cs:line 17
   at System.RuntimeMethodHandle.InvokeMethod(Object target, Void** arguments, Signature sig, Boolean isConstructor)
   at System.Reflection.MethodBaseInvoker.InvokeDirectByRefWithFewArgs(Object obj, Span`1 copyOfArgs, BindingFlags invokeAttr)
```

And is now producing

```text
  Message: 
Assembly Initialization method TestProject80.UnitTest1.Init threw exception. System.Exception: AAAA. Aborting test execution.
Stack Trace:
    at TestProject80.InitHelper..cctor() in C:\src\temp\ConsoleApp4\TestProject1\UnitTest1.cs:line 29
```

## MSTest.Sdk

It's great to see your positive response to MSTest.Sdk. In case you missed our announcement, please read our [Introducing MSTest SDK blog post](https://devblogs.microsoft.com/dotnet/introducing-mstest-sdk/).

We have been carefully listening to your feedback and we are now adding global using of MSTest namespace to simplify further your tests. So starting with MSTest 3.4, you no longer need to explicitly add `using Microsoft.VisualStudio.TestTools.UnitTesting` in your files. Note that `MSTest.Sdk` does respect your implicit usings preferences.

On top of that, we have enhanced our [sample codes of how to use the SDK](https://github.com/microsoft/testfx/tree/rel/3.4/samples/public/DemoMSTestSdk).

Lastly, we have made Playwright and Aspire testing easier.

### Playwright

Simply add `<EnablePlaywright>true</EnablePlaywright>` to your project to remove all the project boilerplate.

Playwright project *without* SDK:

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>

    <IsPackable>false</IsPackable>
    <IsTestProject>true</IsTestProject>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.10.0" />
    <PackageReference Include="Microsoft.Playwright.MSTest" Version="1.44.0" />
    <PackageReference Include="MSTest.Analyzers" Version="3.4.1" />    
    <PackageReference Include="MSTest.TestAdapter" Version="3.4.1" />
    <PackageReference Include="MSTest.TestFramework" Version="3.4.1" />
  </ItemGroup>

  <ItemGroup>
    <Using Include="Microsoft.Playwright.MSTest" />
    <Using Include="Microsoft.VisualStudio.TestTools.UnitTesting" />
    <Using Include="System.Text.RegularExpressions" />
    <Using Include="System.Threading.Tasks" />
  </ItemGroup>

</Project>
```

Playwright project with SDK:

```xml
<Project Sdk="MSTest.Sdk/3.4.1">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    
    <IsPackable>false</IsPackable>
    <EnablePlaywright>true</EnablePlaywright>
  </PropertyGroup>

  <ItemGroup>
    <Using Include="System.Text.RegularExpressions" />
    <Using Include="System.Threading.Tasks" />
  </ItemGroup>

</Project>
```

### Aspire

Simply add `<EnableAspireTesting>true</EnableAspireTesting>` to your project to remove all the project boilerplate.

Aspire test project *without* SDK:

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <IsPackable>false</IsPackable>
    <IsTestProject>true</IsTestProject>
    <EnableMSTestRunner>true</EnableMSTestRunner>
    <OutputType>Exe</OutputType>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Aspire.Hosting.Testing" Version="8.0.0-preview.6.24214.1" />
    <PackageReference Include="MSTest" Version="3.4.1" />
  </ItemGroup>

  <ItemGroup>
    <Using Include="Aspire.Hosting.Testing" />
    <Using Include="Microsoft.VisualStudio.TestTools.UnitTesting" />
  </ItemGroup>

</Project>
```

Aspire test project with SDK:

```xml
<Project Sdk="MSTest.Sdk/3.4.1">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    
    <IsPackable>false</IsPackable>
    <EnableAspireTesting>true</EnableAspireTesting>
  </PropertyGroup>

</Project>
```

## MSTest.Runner support for WinUI tests

You asked for it, so we made it possible to run WinUI tests with MSTest runner. Check-out our [project sample](https://github.com/microsoft/testfx/tree/rel/3.4/samples/public/mstest-runner/MSTestRunnerWinUI) and we are working to simplify [testing un-packaged applications](https://github.com/microsoft/testfx/issues/2784).

We have as well enhanced the runner's performance by using the built-in `System.Text.Json` for .NET instead of Jsonite and by caching command line options.

## What's next?

We hope you enjoy this new release of MSTest and find it useful. We would love to hear your feedback and suggestions on how we can make MSTest better [browse or create an issue in our repository](https://github.com/microsoft/testfx/issues).

A huge thank you to all the amazing contributors who helped make this release even better. Your hard work and awesome ideas are what make MSTest great. Thanks for being a part of this journey!

Thank you for using MSTest and happy testing!
