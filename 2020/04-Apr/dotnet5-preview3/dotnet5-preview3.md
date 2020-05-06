# Announcing .NET 5.0 Preview 3

Today, we're releasing .NET 5.0 Preview 3. It contains a set of new features and performance improvements. We're continuing to work on the bigger features that will define the 5.0 release. The [.NET 5.0 Preview 1 post](https://devblogs.microsoft.com/dotnet/announcing-net-5-0-preview-1/) covers what we are planning on building for .NET 5.0. Please take a look at the post and the [dotnet/designs](https://github.com/dotnet/designs) repository and share any feedback you have. And, of course, please install Preview 3, and test your workloads with it.

You can [download .NET 5.0 Preview 3](https://dotnet.microsoft.com/download/dotnet-core/5.0), for Windows, macOS, and Linux:

* [.NET 5.0 Preview 3 and Runtime](https://dotnet.microsoft.com/download/dotnet-core/5.0)
* [Docker images](https://hub.docker.com/_/microsoft-dotnet-core)
* [Snap installer](https://snapcraft.io/dotnet-sdk)

ASP.NET Core and EF Core are also being released today.

You need to use Visual Studio 2019 16.5 to use .NET 5.0. Install the latest version of the [C# extension](https://code.visualstudio.com/Docs/languages/csharp), to use .NET 5.0 with Visual Studio Code. .NET 5.0 isn't yet supported with Visual Studio for Mac.

Release notes:

* .NET 5.0 release notes
* .NET 5.0 known issues
* .NET Core 3.1 -> .NET 5.0 API diff
* GitHub release
* GitHub tracking issue

Let's look at some of the improvements in Preview 3.

## Code quality improvements in RyuJIT

Every release includes a set of changes that improve the machine code that the JIT generates (we call this "code quality"). Better code quality means better application performance.

* [Vectorise BitArray for ARM64 - dotnet/runtime #33749](https://github.com/dotnet/runtime/pull/33749) -- The [BitArray class](https://docs.microsoft.com/dotnet/api/system.collections.bitarray?view=netframework-4.8) was updated to include a [hardware-accelerated implementation for ARM64](https://github.com/dotnet/runtime/blob/8511b5b9cc957ce824fc88c0780249fc0edbef15/src/libraries/System.Collections/src/System/Collections/BitArray.cs#L181) using [ARM64 intrinisics](https://github.com/dotnet/runtime/tree/master/src/libraries/System.Private.CoreLib/src/System/Runtime/Intrinsics/Arm). The [performance improvements](https://github.com/dotnet/runtime/pull/33749#issuecomment-603597689) for BitArray are very significant. Credit to [@Gnbrkm41](https://github.com/Gnbrkm41).
* [Implement simple version of On Stack Replacement (OSR) - dotnet/rintime #32969](https://github.com/dotnet/runtime/pull/32969). [On-stack replacement (OSR)](https://github.com/dotnet/runtime/blob/master/docs/design/features/OnStackReplacement.md) is a new capability that allows the code executed by currently running methods to be changed in the middle of method execution, while those methods are active "on stack". This feature is currently experimental and opt-in (on x64), and targeted at improving the more challenging performance characteristics of tiered compilation. Please try it out and give us feedback.
* [Dynamic generic dictionary expansion feature dotnet/runtime #32270](https://github.com/dotnet/runtime/pull/32270) - Some (maybe most?) uses of generics now have better performance ([initial performance findings](https://github.com/dotnet/runtime/pull/32270#issuecomment-586459130)), based on improving the implementation of low-level (native code) dictionaries used by the runtime to store information about generic types and methods. See [Perf: Collection Count() is Slower in Core than the CLR](https://github.com/dotnet/runtime/issues/11971#issuecomment-462183168) for more information. Credit to [@RealDotNetDave](https://github.com/RealDotNetDave) for the bug report.
* [Implement Vector.Ceiling / Vector.Floor dotnet/runtime #31993](https://github.com/dotnet/runtime/pull/31993) -  Implement Vector.Ceiling / Vector.Floor using x64 and ARM64 intrinsics, per [API proposal](https://github.com/dotnet/runtime/issues/20509). Credit to [@Gnbrkm41](https://github.com/Gnbrkm41).
* [JIT: allow CORINFO_HELP_READYTORUN_GENERIC_HANDLE to be optimized dotnet/runtime #34221](https://github.com/dotnet/runtime/pull/34221). Improves code quality for generics in Ready2Run images.
* [JIT: enable tail calls and copy omission for implicit byref structs #33004](https://github.com/dotnet/runtime/pull/33004). Improves code quality for structs as arguments in "tail call" position calls.

## System.Text.Json improvements

* [Add support for preserve references on JSON dotnet/runtime #655](https://github.com/dotnet/runtime/pull/655) - [Enables `ReferenceLoopHandling`](https://github.com/dotnet/runtime/issues/29900), which is one of the key features of JSON.NET serialization.
* [Add new System.Net.Http.Json project/namespace dotnet/runtime #33459](https://github.com/dotnet/runtime/pull/33459) - Adds [new extension methods for HttpClient that allow serialization from/to JSON](https://github.com/dotnet/runtime/issues/32937).
* [Add `JsonConstructor` and support for deserializing with parameterized ctors dotnet/runtime #33444](https://github.com/dotnet/runtime/pull/33444) -- Adds support for immutable classes and structs to JsonSerializer.
* [Add JsonIgnoreCondition & per-property ignore logic #34049](https://github.com/dotnet/runtime/pull/34049) - Adds support for null value handling, which is another feature of JSON.NET serialization.

## .NET SDK Support for .NET Framework Assemblies

The [.NET SDK will now auto-reference](https://github.com/dotnet/sdk/issues/4009) the [Microsoft.NETFramework.ReferenceAssemblies](https://www.nuget.org/packages/Microsoft.NETFramework.ReferenceAssemblies/) NuGet package given a .NET Framework target framework in a project file. This change enables building .NET Framework projects on a machine without the required .NET Framework targeting pack installed. This improvement is specific to targeting packs, and doesn't account for other dependencies that a project may have.

## Closing

Please take a moment to try out Preview 2, possibly in a container, a VM. We'd like your feedback on the quality of the release. There is a lot more coming, over the next several months, leading up a November release.
