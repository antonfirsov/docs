---
post_title: 'Infer# v1.2: Interprocedural Memory Safety Analysis For C#'
username: xinshi
microsoft_alias: xinshi
featured_image: ./secure-authentication-code-browser-gears.png
categories: .NET, .NET Core, C#, Static Analysis
summary: 'Infer# v1.2 brings the first interprocedural race condition detection to .NET. Use it today locally in Windows via WSL2 or directly in continuous integration via Azure Pipelines or GitHub Actions.'
desired_publication_date: '2021-11-16'
---

Last December, we announced the [public preview release of Infer#](https://devblogs.microsoft.com/dotnet/infer-interprocedural-memory-safety-analysis-for-c/), which brings the interprocedural static analysis of [Infer](https://fbinfer.com/) to the .NET community. The project was [open sourced on GitHub](https://github.com/microsoft/infersharp) under an MIT license.

## New Feature Highlights

Infer# 1.2 brings race condition detection, improves performance, provides more ways to use, and expands analysis coverage. 

### Support for Infer# On Windows via WSL2 (Windows Subsystem for Linux)

As the first step in our initiative to provide Windows support for Infer#, you can now [run the analysis in WSL2](https://github.com/microsoft/infersharp/blob/main/RUNNING_INFERSHARP_ON_WINDOWS.md).

### Azure Pipelines Integration

We now support [Infer# as an Azure Pipelines plugin](https://github.com/microsoft/infersharp#azure-pipelines).

### Race Condition

Infer# now supports race condition detection via Infer's [RacerD](https://fbinfer.com/docs/all-issue-types/#thread_safety_violation) analyzer.

```csharp
public class RaceCondition
{
    private readonly object _object = new object();

    public void TestMethod()
    {
        int FirstLocal;
        FirstLocal = TestClass.StaticIntegerField;
    }

    public void FieldWrite()
    {
        lock (_object)
        {
            {
                TestClass.StaticIntegerField = 1;
            }
        }
    }
}
```

```text
Assets/RaceCondition.cs:12: warning: Thread Safety Violation
  Read/Write race. Non-private method `RaceCondition.TestMethod()` reads without synchronization from `Assets.TestClass.Cilsil.Test.Assets.TestClass.StaticIntegerField`. Potentially races with write in method `RaceCondition.FieldWrite()`.
 Reporting because another access to the same memory occurs on a background thread, although this access may not.
```

### Exception Code Coverage  

Infer# now reports warnings on methods with exception-handling constructs (for example, try-catch-finally, and lock).

```csharp
public void ResourceLeakExcepHandlingBad() {
    StreamWriter stream = AllocateStreamWriter();
    try
    {
        stream.WriteLine(12);
    }
    catch
    {
        console.log("Fail to write");
    }
    finally
    {
        // FIXME: should close the stream by calling stream.Close().
    }
}
```

```text
/.../Examples/Program.cs:39: error: Dotnet Resource Leak
  Leaked { %0 -> 1 } resource(s) at type(s) System.IO.StreamWriter.
```

The full list of improvements can be found on the [release page](https://github.com/microsoft/infersharp/releases). 

## Using Infer#
You can find the instructions for all supported scenarios on our [GitHub landing page](https://github.com/microsoft/infersharp#infersharp).

Please submit your feedback and feature requests to our [GitHub repository](https://github.com/microsoft/infersharp/issues). We're looking at all feature requests from the community and will prioritize next steps based on popularity.
