---
post_title: What's New in Our Code Coverage Tooling?
author1: jachocho
post_slug: whats-new-in-our-code-coverage-tooling
microsoft_alias: jachocho
featured_image: codecoverageimprovementsthumb.jpg
categories: .NET, .NET Core, .NET Framework, Performance
tags: .NET, .NET Core, .NET Framework, performance
ai_note: show
summary: Discover enhanced code coverage tools with better platform support, new report formats and improved performance.
post_date: 2023-12-21 10:05:00
---

Exciting news for developers! We've enhanced our code coverage tools, [Microsoft.CodeCoverage](https://www.nuget.org/packages/Microsoft.CodeCoverage) and [dotnet-coverage](https://aka.ms/dotnet-coverage), with some fantastic features. If you're new to our tools, check out our [Get Started](https://github.com/microsoft/codecoverage#get-started) guide. Let's dive into the changes that will simplify your coding experience.

## Support for All Platforms

Our tools can run on any platform supported by .NET, thanks to the addition of static instrumentation. Learn more about [static and dynamic instrumentation](https://github.com/microsoft/codecoverage/blob/main/docs/instrumentation.md), and discover [supported platforms](https://github.com/microsoft/codecoverage/blob/main/docs/supported-os.md).

## Fresh Report Formats

We've revamped our code coverage report formats to integrate smoothly with tools like ReportGenerator. While the default remains the familiar `.coverage` format, we've introduced some new ones:
- **Binary (Default):** .coverage (Microsoft's special format) - Open it in Visual Studio Enterprise. [Example](https://github.com/microsoft/codecoverage/blob/main/samples/Calculator/scenarios/scenario01/README.md)
- **Cobertura:** .cobertura.xml (Open-source XML format) - Open it in Visual Studio Enterprise, any text editor, or generate an HTML report with ReportGenerator. [Example](https://github.com/microsoft/codecoverage/blob/main/samples/Calculator/scenarios/scenario02/README.md)
- **XML:** .xml (Microsoft's XML Format) - Open it in Visual Studio Enterprise and any text editor. [Example](https://github.com/microsoft/codecoverage/blob/main/samples/Calculator/scenarios/scenario04/README.md)

## Meet dotnet-coverage

Introducing our new tool, dotnet-coverage! It performs following tasks:

- Collects code coverage for console applications. [Example](https://github.com/microsoft/codecoverage/blob/main/samples/Calculator/scenarios/scenario08/README.md)
- Collects code coverage for web applications. [Example](https://github.com/microsoft/codecoverage/blob/main/samples/Calculator/scenarios/scenario14/README.md)
- Merges coverage reports. [Example](https://github.com/microsoft/codecoverage/blob/main/samples/Calculator/scenarios/scenario18/README.md)
- Instruments binaries. [Example](https://github.com/microsoft/codecoverage/blob/main/samples/Calculator/scenarios/scenario10/README.md)
- Calculates code coverage for each test separately. [Example](https://github.com/microsoft/codecoverage/blob/main/samples/Calculator/scenarios/scenario16/README.md)

Visit [dotnet-coverage](https://aka.ms/dotnet-coverage) documentation to learn more.

## Auto-Merge for solutions

Running `dotnet test --collect "Code Coverage"` at the solution level now automatically merges code coverage for all your test projects. Visit [Scenario 24 Code coverage for solution](https://github.com/microsoft/codecoverage/blob/main/samples/Calculator/scenarios/scenario24/README.md) to see full example.

## Improved Documentation

Explore our fresh GitHub repository at [microsoft/codecoverage](https://github.com/microsoft/codecoverage) for all the [info](https://github.com/microsoft/codecoverage#readme) and [samples](https://github.com/microsoft/codecoverage/tree/main/samples/Calculator#readme) you need.

## Faster Performance

Prior to the 16.5 release, the collection of code coverage report significantly slowed down test execution. We addressed this issue, resulting in an impressive 80% performance gain. See [performance](https://github.com/microsoft/codecoverage/tree/main/docs/performance) section for detailed results and logs.

| Package                 | Time       | Ratio |
|-------------------------|------------|-------|
| Microsoft.CodeCoverage 16.5 | 03:52:53 | 1.00  |
| Microsoft.CodeCoverage 17.0 | 02:25:49 | 0.63  |
| Microsoft.CodeCoverage 17.5 | 01:27:52 | 0.38  |
| Microsoft.CodeCoverage 17.9 | 00:50:00 | 0.21  |

## What You Need to Do

To enjoy the latest features and speed up your builds, make sure to use our latest stable packages in your test projects:
```xml
<PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.8.0" />
<PackageReference Include="Microsoft.CodeCoverage" Version="17.8.0" />
```
If your solution doesn't have any C++ code, make it faster and more reliable by turning off native code coverage with these flags in runsettings:

```xml
<EnableStaticNativeInstrumentation>False</EnableStaticNativeInstrumentation>
<EnableDynamicNativeInstrumentation>False</EnableDynamicNativeInstrumentation>
```
Visit [configuration](https://github.com/microsoft/codecoverage/blob/main/docs/configuration.md) documenation to see other options and full example of our settings.

## Special Thanks

A big thank you to [Faisal Hafeez](https://github.com/fhnaseer), [Marco Rossignoli](https://twitter.com/MarcoRossignoli), [Mariam Abdullah](https://github.com/mariam-abdulla), [Codrin-Victor Poienaru](https://github.com/cvpoienaru) and [Pavel Horak](https://twitter.com/pavelhorak0) for their exceptional contributions to this project! 🙌🚀