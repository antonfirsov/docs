---
post_title: Testing Your Native AOT Applications
author1: mrossignoli
author2: jajares
author3: jachocho
post_slug: testing-your-native-aot-dotnet-apps
microsoft_alias: jajares
featured_image: nativeaottestingpreview.jpg
categories: .NET, C#, Performance
tags: csharp, testing, mstest, dotnet
ai_note: hide
summary: MSTest introduces a Native AOT compatible test runner and engine for testing your Native AOT applications.
post_date: 2024-04-03 10:05:00
---

We are happy to announce that we just published an early preview of support for testing Native AOT with MSTest, and we welcome all of you to try it.

This new solution is powered by a completely new testing engine that we wrote from ground up, to allow running tests in Native AOT, and that works together with the existing MSTest runner (Microsoft.Testing.Platform). We also leveraged the power of source generators, to discover tests during compilation, which solved one of the big hurdles that prevented us from compiling and running tests in Native AOT.

## What is Native AOT

.NET applications can be [published as Native AOT](https://learn.microsoft.com/dotnet/core/deploying/native-aot), which means that the resulting application is compiled ahead-of-time (AOT) into native code. Such applications have faster startup time, smaller memory footprint, and no dependency on .NET runtime.

There are two main use cases for such applications. One is ASP.NET Core, which has added support for Native AOT in .NET 8, that takes advantage of the faster startup time. And the second use case is running applications directly on IoT devices that have limited resources and storage.

## Why should I test Native AOT apps?

Native AOT applications can run in environments that do not allow JITing code. Such as game consoles, or some IoT devices. Building your tests as Native AOT will allow running them even on such devices.

You may also be producing a Native AOT and non-native version of your application and use compiler conditions to take different paths in your code, e.g. to work around [code that RequiresDynamicCode](https://learn.microsoft.com/dotnet/core/deploying/native-aot/fixing-warnings#react-to-aot-warnings).

Native tests can help you test such use cases.

## How to test your apps in Native AOT mode?

In the example below we are creating a test project that enables PublishAot and has 2 new packages installed: `MSTest.Engine` and `MSTest.SourceGeneration`. These packages contain our new testing engine, and source generator. We are using this test project to test our class library (not shown), that also enables PublishAot.

```xml
<!-- file: UnitTestProject1.csproj -->
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>

    <OutputType>exe</OutputType>
    <PublishAot>true</PublishAot>
  </PropertyGroup>

  <ItemGroup>
    <!-- 
      Experimental MSTest Engine & source generator, 
      close sourced, licensed the same as our extensions 
      with Microsoft Testing Platform Tools license.
    -->
    <PackageReference Include="MSTest.Engine" Version="1.0.0-alpha.24163.4" />
    <PackageReference Include="MSTest.SourceGeneration" Version="1.0.0-alpha.24163.4" />

    <PackageReference Include="Microsoft.CodeCoverage.MSBuild" Version="17.10.4" />
    <PackageReference Include="Microsoft.Testing.Extensions.CodeCoverage" Version="17.10.4" />

    <PackageReference Include="Microsoft.Testing.Extensions.TrxReport" Version="1.0.2" />
    <PackageReference Include="Microsoft.Testing.Platform.MSBuild" Version="1.0.2" />
    <PackageReference Include="MSTest.TestFramework" Version="3.2.2" />
    <PackageReference Include="MSTest.Analyzers" Version="3.2.2" />
    
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\ClassLibrary1\ClassLibrary1.csproj" />
  </ItemGroup>

  <ItemGroup>
    <Using Include="Microsoft.VisualStudio.TestTools.UnitTesting" />
  </ItemGroup>

</Project>
```

```csharp
// file: UnitTest1.cs
using ClassLibrary1;

namespace TestProject1;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void TestMethod1()
    {
        Assert.AreEqual(3, new Class1().Add(1, 2));
    }

    [TestMethod]
    [DataRow(1, 2)]
    public void TestMethod2(int left, int right)
    {
        Assert.AreEqual(3, new Class1().Add(left, right));
    }
}
```

The complete example is available at [microsoft/testfx samples](https://github.com/microsoft/testfx/tree/main/samples/mstest-runner/NativeAotRunner).

To run your tests, you need to publish them, then run the published executable:

```bash
C:\t\NativeAotRunner> dotnet publish .\TestProject1 --runtime win-x64
C:\t\NativeAotRunner> .\TestProject1\bin\Release\net8.0\win-x64\publish\TestProject1.exe

Microsoft(R) Testing Platform Execution Command Line Tool
Copyright(c) Microsoft Corporation.  All rights reserved.

Passed! - Failed: 0, Passed: 6, Skipped: 0, Total: 6, Duration: 7ms - TestProject1.exe
```

## Code Coverage and TRX reports

You might notice that the example project is referencing the CodeCoverage and TRX report extension packages. They are, as their name suggests, providing Code Coverage and TRX (test results) reports. 

We think this is the minimum that you need to successfully add Native AOT tests to your existing pipelines, and that is why we are providing them with this early preview. Using the project above you can simply provide `--coverage --report-trx` on the commandline to collect TRX and CodeCoverage report.

```bash
C:\t\NativeAotRunner> .\TestProject1\bin\Release\net8.0\win-x64\publish\TestProject1.exe --coverage --report-trx

Microsoft(R) Testing Platform Execution Command Line Tool
Copyright(c) Microsoft Corporation.  All rights reserved.

Passed! - Failed: 0, Passed: 6, Skipped: 0, Total: 6, Duration: 1ms - TestProject1.exe

In process file artifacts produced:
- C:\t\NativeAotRunner\TestProject1\bin\Release\net8.0\win-x64\publish\TestResults\jajares_RDESKTOP_2024-03-27_12_18_20.727.trx
- C:\t\NativeAotRunner\TestProject1\bin\Release\net8.0\win-x64\publish\TestResults\8abd5db9-4d12-4778-b4f1-e24635c44df0.coverage
```

Here are the details of how this is set up:

### Running with TRX

To run with TRX report provide `--report-trx` on commandline.

`.\TestProject1\bin\Release\net8.0\win-x64\publish\TestProject1.exe --report-trx`

### Running with Code Coverage

To generate code coverage in Native AOT mode you need to instrument the executable during `dotnet publish`. This is done by providing `/p:AotMsCodeCoverageInstrumentation=true` MSBuild property:

`dotnet publish .\TestProject1 --runtime win-x64 /p:AotMsCodeCoverageInstrumentation=true`

And then during test run you will provide `--coverage` parameter to collect code coverage of the run:

`.\TestProject1\bin\Release\net8.0\win-x64\publish\TestProject1.exe --coverage`

This functionality is achieved by the two referenced NuGet packages:

- `Microsoft.CodeCoverage.MSBuild`: adds required MSBuild targets to instrument test project during build
- `Microsoft.Testing.Extensions.CodeCoverage`: adds coverage provider to test

For more information please visit [Code coverage for MSTest runner project in Native AOT mode](https://github.com/microsoft/codecoverage/blob/main/samples/Algorithms/scenarios/scenario06/README.md). 

To see how you can collect coverage for any Native AOT console app, using `Microsoft.CodeCoverage.MSBuild` and `dotnet-coverage` tool please see [this example](https://github.com/microsoft/codecoverage/blob/main/samples/Algorithms/scenarios/scenario05/README.md).

## Limitations

`MSTest.Engine` and `MSTest.SourceGeneration` are implementing the bare minimum to run tests:

- They detect `[TestClass]` and `[TestMethod]` attributes.
- Test methods can use `[DataRow]` and `[DynamicData]`.

All other features of MSTest are **not supported** yet. This includes:

- Inheriting from `TestClass` and `TestMethod` attributes is not supported.

- `AssemblyInitialize`, `ClassInitialize`, `TestInitialize`, as well as all the related Cleanups are not supported.

- There is no way to configure in-process parallelism, all tests will run in parallel, as if you defined `[assembly: Parallelize(Workers = 0, Scope = ExecutionScope.MethodLevel)]`.

## What’s next?

We are adding the various `Initialize` and `Cleanup` methods, which was a top request from the feature’s early adopters.

We are also working on improving the getting started experience via a new `MSTest.SDK`, we will announce this soon!

For more features that you would like to prioritize, please let us know in this issue GitHub issue, where we are collecting feedback [Add support for NativeAOT](https://github.com/microsoft/testfx/issues/1837).
