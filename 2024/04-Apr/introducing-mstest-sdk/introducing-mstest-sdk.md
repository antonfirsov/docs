---
post_title: Introducing MSTest SDK - Improved Configuration & Flexibility
author1: mrossignoli
author2: amauryleve
post_slug: introducing-mstest-sdk
microsoft_alias: amauryleve
featured_image: mstestsdk.jpg
categories: .NET, C#
tags: mstest, testing
ai_note: hide
summary: The new MSTest SDK is here and provides an easier way to configure your MSTest projects. Learn all about this release, how to get started, and its great improvements when using MSTest.
post_date: 2024-04-11 10:05:00
---

We are excited to announce the new MSTest SDK built on top of the MSBuild Project SDK system. It is designed to give you a better experience for testing with MSTest by making project configuration easier through sensible defaults and flexible options.

This new experience was built on top of the recently introduced MSTest runner ([check out the announcement post](https://devblogs.microsoft.com/dotnet/introducing-ms-test-runner/)) to further simplify your experience. This new runner, a lightweight, reliable, and performant way to run MSTest tests, is shipped as dependency of `MSTest.TestAdapter` NuGet package. The runner and its extensions consist of multiple NuGet packages to provide an extensible, flexible, and configurable test running experience. However, being customizable can result in many questions: What are the recommended extensions? What are the correct defaults? How do I align the versions? This is where the MSTest SDK comes in.

## Getting started with MSTest SDK

Getting started using the new MSTest SDK is easy. Just create a MSTest project (or update an existing MSTest project) and replace the content of the `.csproj` file with the following:

```xml
<Project Sdk="MSTest.Sdk/3.3.1">

  <PropertyGroup> 
      <TargetFramework>net8.0</TargetFramework>
  </PropertyGroup>

  <!-- Additional properties and items for your tests. -->
</Project> 
```

Note that you can use any target framework supported by MSTest (i.e. `net462` and above).

## Advantages of MSTest SDK

This new SDK offers many advantages for you and your test projects, such as: 

* Better defaults
* Simplified usage
* Extensibility of the MSTest runner
* Easier opt-in for new features (e.g. native AOT tests).

### Better defaults

When using the MSTest SDK, you are aligning with the patterns that are provided by the main types of applications such as ASP.NET Core, Razor, Windows Desktop. It will use the default suggestions that the MSTest team makes for your test projects.

For example, we introduced some MSTest static code analyzers with v3.2 but these analyzers are defined in a new package that is not available by default so you would have to manually add this package to your test projects. Instead, by using MSTest SDK, you would simply bump the version and get all the new defaults.

We want to call out that we are following semantic versioning (see this [article about semantic versioning](https://learn.microsoft.com/dotnet/csharp/versioning#semantic-versioning)) principles closely when choosing the defaults and updating them between versions, so you have the guarantee of understandable and easy updates.

### Easier usage of MSTest Runner extensions

In addition to [MSTest runner](https://devblogs.microsoft.com/dotnet/introducing-ms-test-runner/), we are also shipping a set of extensions that you can install as NuGet packages to enhance your testing experience. To help you select the right defaults, the right extensions and to make sure you have easy update and alignments between the extensions, we are introducing a new concept of "profiles". We are currently providing the following 3 profiles: `Default`, `AllMicrosoft` and `None`, that you can configure and further customize. With `Default` profile being our recommendation.

The default profile contains:

- [Microsoft Code Coverage](https://www.nuget.org/packages/Microsoft.Testing.Extensions.CodeCoverage)
- [TRX report support](https://www.nuget.org/packages/Microsoft.Testing.Extensions.TrxReport)

Starting from any profile, you can then manually opt-in or opt-out any additional extension adding some additional properties to your project that would follow the pattern `Enable[NugetPackageNameWithoutDots]`. For example, to add crash dump support to the default profile you can simply add the following MSBuild property `EnableMicrosoftTestingExtensionsCrashDump` set to `true`.

```xml
<Project Sdk="MSTest.Sdk/3.3.1">

  <PropertyGroup>
      <TargetFramework>net8.0</TargetFramework>
      <!-- Enable Microsoft.Testing.Extensions.CrashDump package on top of the default profile -->
      <EnableMicrosoftTestingExtensionsCrashDump>true</EnableMicrosoftTestingExtensionsCrashDump>
  </PropertyGroup>

  <!-- Additional properties and items for your tests. -->
</Project> 
```

You can refer to our [MSTest SDK documentation](https://learn.microsoft.com/dotnet/core/testing/unit-testing-mstest-sdk#available-extensions) to get more information about these profiles and their defaults.

### Testing Native AOT

MSTest is the first .NET Test Framework and runner to support running tests in Native AOT mode. When using MSTest SDK, we will automatically detect if you are publishing to AOT and transparently swap all required test packages and configurations to match this specialized mode.

For more information about testing with Native AOT, please refer to this blog post: [Testing with Native AOT blog post](https://devblogs.microsoft.com/dotnet/testing-your-native-aot-dotnet-apps/).

Example of project setup *without* MSTest SDK:

```xml
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

The same project setup *with* MSTest SDK:

```xml
<Project Sdk="MSTest.Sdk/3.3.1">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <PublishAot>true</PublishAot>
  </PropertyGroup>

  <ItemGroup>
    <ProjectReference Include="..\ClassLibrary1\ClassLibrary1.csproj" />
  </ItemGroup>

  <ItemGroup>
    <Using Include="Microsoft.VisualStudio.TestTools.UnitTesting" />
  </ItemGroup>

</Project>
```

## What's next?

The MSTest SDK style, while still in development, stands as the cornerstone of our forthcoming evolutions and features. We strongly encourage all MSTest users to transition to this SDK style that will become our standard for the MSTest project template with .NET 9.

We are also planning to add more scenarios in upcoming releases such as Playwright and WinUI.

We welcome any feedback on how to improve and refine it further, ensuring it aligns with the diverse needs and expectations of our users. The best way to share feedback is to creat an issue in our [microsoft/testfx](https://github.com/microsoft/testfx) repository.
