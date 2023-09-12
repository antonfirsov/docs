---
post_title: Announcing .NET 8 RC1
author1: lerich
post_slug: announcing-dotnet-8-rc1
microsoft_alias: lerich
featured_image: dotnet-8-rc1.png
categories: .NET, .NET Core
tags: .net 8, featured-preview
summary: ".NET 8 RC1 is now available with improvements to System.Text.Json, a new AOT mode for Android and WASM, Azure Managed Identity support for containers, and more!"
post_date: 2023-09-12 10:05:00
---

.NET 8 RC1 is now available. This is our first of two release candidates. This release includes a new AOT mode for both Android and WASM, System.Text.Json improvements, and Azure Managed Identity support for containers. Now is great time to pick up and test .NET 8 if you haven't yet. 

The dates for [.NET Conf 2023](https://dotnetconf.net/) have been announced! Join us November 14-16, 2023 to celebrate the .NET 8 release!

[Download .NET 8 RC1](https://dotnet.microsoft.com/download/dotnet/8.0) for Linux, macOS, and Windows.

* [Installers and binaries](https://dotnet.microsoft.com/download/dotnet/8.0)
* [Container images](https://hub.docker.com/_/microsoft-dotnet-aspnet/)
* [Release notes](https://github.com/dotnet/core/tree/main/release-notes/8.0)
* [Breaking changes](https://learn.microsoft.com/dotnet/core/compatibility/8.0?toc=%2Fdotnet%2Ffundamentals%2Ftoc.json&bc=%2Fdotnet%2Fbreadcrumb%2Ftoc.json)
* [Known issues](https://github.com/dotnet/core/blob/main/release-notes/8.0/known-issues.md)
* [GitHub issue tracker](https://github.com/dotnet/core/issues)

There are several exciting posts you should check out as well:

- [ASP.NET Core Updates in .NET 8 RC1](https://devblogs.microsoft.com/dotnet/asp-net-core-updates-in-dotnet-8-rc-1)
- [.NET MAUI Updates in .NET 8 RC1](https://devblogs.microsoft.com/dotnet/announcing-dotnet-maui-in-dotnet-8-rc-1)
- [Visual Studio 2022 17.8 Preview 2](https://aka.ms/vs/v178P2)
- [Entity Framework Updates in .NET 8 RC1](https://devblogs.microsoft.com/dotnet/announcing-ef8-rc1)
- [What's New in .NET 8](https://learn.microsoft.com/dotnet/core/whats-new/dotnet-8) describes all the new features in .NET 8. For a broader view of the platform, read [Why .NET?](https://devblogs.microsoft.com/dotnet/why-dotnet/).

## System.Text.Json Improvements

### `System.Net.Http.Json` extensions  for IAsyncEnumerable

https://github.com/dotnet/runtime/pull/89258

RC1 sees the inclusion of IAsyncEnumerable streaming deserialization extension methods:

```C#
const string RequestUri = "https://api.contoso.com/books";
using var client = new HttpClient();
IAsyncEnumerable<Book> books = await client.GetFromJsonAsAsyncEnumerable<Book>(RequestUri);

await foreach (Book book in books)
{
    Console.WriteLine($"Read book '{book.title}'");
}

public record Book(int id, string title, string author, int publishedYear);
```

Credit to @IEvangelist for contributing the implementation.

### `JsonContent.Create` overloads accepting `JsonTypeInfo`

https://github.com/dotnet/runtime/pull/89614

It is now possible to create `JsonContent` instances using trim safe/source generated contracts:

```C#
var book = new Book(id: 42, "Title", "Author", publishedYear: 2023);
HttpContent content = JsonContent.Create(book, MyContext.Default.Book);

public record Book(int id, string title, string author, int publishedYear);

[JsonSerializable(typeof(Book))]
public partial class MyContext : JsonSerializerContext 
{ }
```

Credit to @brantburnett for contributing the implementation.

### `JsonNode.ParseAsync` APIs

https://github.com/dotnet/runtime/pull/90006

Adds support for parsing `JsonNode` instances from streams:

```C#
using var stream = File.OpenRead("myFile.json");
JsonNode node = await JsonNode.ParseAsync(stream);
```

Credit to @DoctorKrolic for contributing the implementation.

### `JsonSerializerOptions.MakeReadOnly(bool populateMissingResolver)`

https://github.com/dotnet/runtime/pull/90013

The existing parameterless `JsonSerializerOptions.MakeReadOnly()` method was designed to be trim-safe and will therefore throw an exception in cases where the options instance hasn't been configured with a resolver.

Calling the new overload with
```C#
options.MakeReadOnly(populateMissingResolver: true);
```
will populate the options instance with the default reflection resolver if one is missing. This emulates the initialisation logic employed by the `JsonSerializer` methods that accept `JsonSerializerOptions`. A side-effect of that behavior is that the new overload is marked `RequiresUnreferenceCode`/`RequiresDynamicCode` and is therefore unsuitable for Native AOT applications.

## `AndroidStripILAfterAOT` mode on Android

https://github.com/xamarin/xamarin-android/pull/8172

We try to choose the best the default configuration for .NET and .NET MAUI applications out of the box. In .NET 6 and higher, specifically, these applications now utilize profiled ahead-of-time (AOT) compilation mode by default when they are built in `Release` mode. AOT compilation results in faster startup time and runtime performance improvements at the expense of a larger app size. Profiled AOT, only AOT compiles a portion of your application's startup path -- improving startup time, with a minimal increase to application size. The new `AndroidStripILAfterAOT` setting removes unused IL that was AOT-compiled, reducing the apk size by at least 0 - 3.5% for a dotnet template application.

### When to consider using `AndroidStripILAfterAOT`:

If performance is more of a concern for your Android application than app size, AOT-compiling all code is a great way to achieve the best startup and runtime performance. `$(AndroidStripILAfterAOT)` goes a step further to remove the unused IL, making the app size increase from AOT-compilation more reasonable. The removed IL will also no longer be present, making it more difficult (but not impossible) to reverse engineer your application's .NET code.

### How to use `AndroidStripILAfterAOT`:

Set the following MsBuild property for your Android app:
```xml
<PropertyGroup Condition=" '$(Configuration)' == 'Release' ">
  <AndroidStripILAfterAOT>true</AndroidStripILAfterAOT>
</PropertyGroup>
```

By default setting `AndroidStripILAfterAOT` to true will *override* the default `AndroidEnableProfiledAot` setting, allowing all trimmable AOT'd methods to be removed.  Profiled AOT and IL stripping can be used together by explicitly setting both:
```xml
<PropertyGroup Condition=" '$(Configuration)' == 'Release' ">
  <AndroidStripILAfterAOT>true</AndroidStripILAfterAOT>
  <AndroidEnableProfiledAot>true</AndroidEnableProfiledAot>
</PropertyGroup>
```

`.apk` size results for a `dotnet new android` app:

| `$(AndroidStripILAfterAOT)` | `$(AndroidEnableProfiledAot)` | `.apk` size   |
| --------------------------- | ----------------------------- | ------------- |
| true                        | true                          | 7.7MB         |
| false                       | true                          | 7.7MB         |
| true                        | false                         | 8.1MB         |
| false                       | false                         | 8.4MB         |

Note that `$(AndroidStripILAfterAOT)`=false and `$(AndroidEnableProfiledAot)`=true is the *default* Release configuration environment, for 7.7MB.

### Limitations:

Not all of the AOT-compiled methods are able to be removed. At runtime, there are various scenarios where we have to switch from AOT to JIT to produce the correct result. With that said, we will be continuously working on reducing gaps in the upcoming .NET 9 release.

### Final note

We would like to invite everyone to try out this new feature and file any discovered issues to help us improve the user experience further. Issues can be filed directly to [dotnet/runtime repository](https://github.com/dotnet/runtime).

## Configuration Binding Generator breaking change

The [configuration binding generator](https://devblogs.microsoft.com/dotnet/announcing-dotnet-8-preview-6/) has switched to using the compiler interceptors preview feature to emit binding logic - https://github.com/dotnet/runtime/pull/90835.

In non Web SDK scenarios (i.e. apps that require a `Microsoft.Extensions.Configuration.Binder` package reference for binding, like console apps), the following additional MSBuild property is now required to enable the generator:

```diff
<PropertyGroup>
  <EnableConfigurationBindingGenerator>true</EnableConfigurationBindingGenerator>
+ <Features>$(Features);InterceptorsPreview</Features>
</PropertyGroup>
```

This is temporary. In RC-2, enabling the generator will switch back to only requiring the one gesture. The compiler inceptors feature will be enabled implicitly in a reliable way.

```diff
<PropertyGroup>
  <EnableConfigurationBindingGenerator>true</EnableConfigurationBindingGenerator>
- <Features>$(Features);InterceptorsPreview</Features>
</PropertyGroup>
```

## Containers switch to non-preview tagging pattern

Issue: https://github.com/dotnet/dotnet-docker/issues/4772
PR: https://github.com/dotnet/dotnet-docker/pull/4817

In preparation for the GA release of .NET 8, the .NET container images have switched to a new tagging pattern for RC 1 that removes "preview" from the tag name.

In previous preview releases of .NET 8, floating tags were published with the name of `8.0-preview` and `8.0-preview-<OS>`. These will no longer be maintained starting with the RC 1 release. Instead, you should migrate your tag references to `8.0` or `8.0-<OS>`. Making this change will allow for a seamless transition upon the GA release of .NET 8 as these will be the permanent tags that will be maintained throughout the lifetime of .NET 8.

## Cross-building Windows apps with Win32 resources on non-Windows

Issue: https://github.com/dotnet/runtime/issues/3828
PR: https://github.com/dotnet/runtime/pull/89303

Many thanks to [@anatawa12](https://github.com/anatawa12) for this great contribution!

When building applications targeting Windows on non-Windows platforms, the resulting executable is now updated with any specified Win32 resources - for example, application icon, manifest, version information.

Previously, applications had to be built on Windows in order to have such resources. Fixing this gap in cross-building support has been a popular request, as it was a significant pain point affecting both infrastructure complexity and resource usage.

## SDK: Container publishing now supports Azure Managed Identity

* [Azure Container Registry auth failure when using Managed Identity](https://github.com/dotnet/sdk-container-builds/issues/425)

The SDK Container publish feature is an easy way to [package .NET applications into containers](https://learn.microsoft.com/dotnet/core/docker/publish-as-container) and push them to container registries like Docker Hub, Azure Container Registry, or other popular registries. Pushing to these remote registries often requires authentication, which is usually handled by the `docker login` command. Some registries, like Azure Container Registry, don't use a standard username/password setup and instead rely on an OAuth token exchange. The SDK Container publishing tools didn't know how to handle this token exchange, and so users that used Managed Identity on Azure Container Registry (or used any other registry that used Identity Token exchange) would encounter authentication errors when pushing their containers. With .NET 8.0.100 RC1 we're happy to announce that we now support the OAuth token exchange authentication method, so ACR and all other registries that use it now just work. A typical publishing flow might now look like:

```shell
> az acr login -n <your registry name>
> dotnet publish -r linux-x64 -p PublishProfile=DefaultContainer
```

Doesn't get much simpler than that! You can learn more about registry authentication in [our docs](https://github.com/dotnet/sdk-container-builds/blob/main/docs/RegistryAuthentication.md), and you learn how to [get started containerizing your apps as well](https://github.com/dotnet/sdk-container-builds/blob/main/docs/GettingStarted.md).

## `WasmStripILAfterAOT` mode on WASM

https://github.com/dotnet/runtime/pull/88926

Blazor WebAssembly and WASM Browser support ahead-of-time (AOT) compilation, where you can compile your .NET code directly into WebAssembly. AOT compilation results in runtime performance improvements at the expense of a larger app size. This new stripping mode reduces the size of _framework folder by 1.7% - 4.2%, based on the testing we've done mentioned in the above github issue.

### When to consider using `WasmStripILAfterAOT`:

This feature is ideal any time you enable AOT compilation, so give it a try for a smaller application!

### How to use `WasmStripILAfterAOT`:

Set the following MsBuild property for your Blazor WebAssembly or WASM Browser app:
```c#
<PropertyGroup>
  <RunAOTCompilation>true</RunAOTCompilation>
  <WasmStripILAfterAOT>true</WasmStripILAfterAOT>
</PropertyGroup>
```
It will trim away the IL code for most of the compiled methods, including methods from the libraries and the ones authored by the app developers.

### Limitations:

Not all of the compiled methods are trimmable. At runtime, there are various scenarios where we have to switch from AOT to interpreter to produce the correct result. With that said, we will be continuously working on reducing the gap in the upcoming .NET9 release.

### Final note

We would like to invite everyone to try out this new feature and file any discovered issues to help us improve the user experience further. Issues can be filed directly to [dotnet/runtime repository](https://github.com/dotnet/runtime).

## Community Contributor
This month's community contributer is Jakub Majocha.  Here's a little about him in his own words:

<img src="community-spotlight.png" width="250px" />

*I'm Jakub Majocha. I live in Cracow, Poland where I work in a field unrelated to software. I have a lifelong interest in computer programming but my experience is limited to small one-off scripts, some tools for in-house use and solving programming puzzles like Advent Of Code. I like to chill tinkering with code and I have found F# perfect for this.*

*The fact that I could just jump in and contribute to F# shows how accommodating the community is. It's also thanks to the language itself. F#, with its type inference, concise syntax and low ceremony, allows for quick experimentation, refactoring and trying things out. It's just fun and relaxing.*

## Summary

The team has transitioned to working on quality and polish as we near the final release in November. Now is a great time to report any issues that you've noticed in RC1 and earlier builds. We'd also love to hear your reactions to using these features.
