---
post_title: Announcing .NET 6 Preview 3
username: rlander@microsoft.com
microsoft_alias: rlander@microsoft.com
categories: .NET Core, .NET
desired_publication_date: 04/08/2021
summary: .NET 6 Preview 3 is now available.
---

Today, we are glad to release .NET 6 Preview 3. This release is dedicated almost entirely to low-level performance features. These are the types of improvements that many folks don't necessarily fully appreciate or understand (including me), but they help a lot for many apps. Most of these improvements apply to the CLR type system directly, either making it function faster or better interplay with modern CPUs (think "hardware accelerate the type system"). In the last few years, there have been a few key performance trends, including: using structs more liberally in libraries, and moving runtime code to C#. Both of trends are visible (directly or indirectly) in these changes. It also demonstrates a continued effort on a focused set of performance strategies.

You will notice that two of these performance changes came [Ben Adams](https://github.com/benaadams/). He's well known for word play on his [first name](https://github.com/benaadams/System.Ben). In the spirit of April 1st (recently past), perhaps we can rename this release .NET 6 Preview B3n. Thanks B3n. You'll also see codegen contributions from [Nathan Moore](https://github.com/nathan-moore) and [SingleAccretion](https://github.com/SingleAccretion). Thanks!

You can [download .NET 6 Preview 3](https://dotnet.microsoft.com/download/dotnet/6.0), for Windows, macOS, and Linux.

* [Installers and binaries](https://dotnet.microsoft.com/download/dotnet/6.0)
* [Container images](https://hub.docker.com/_/microsoft-dotnet)
* [Linux packages](https://github.com/dotnet/core/blob/master/release-notes/6.0/)
* [Release notes](https://github.com/dotnet/core/tree/master/release-notes/6.0)
* [Known issues](https://github.com/dotnet/core/blob/master/release-notes/6.0/6.0-known-issues.md)
* [GitHub issue tracker](https://github.com/dotnet/core/issues)

See the [ASP.NET Core post](https://devblogs.microsoft.com/aspnet/asp-net-core-updates-in-net-6-preview-3/) for more detail on what’s new for web technology. For EF Core, [Preview 3](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/6.0.0-preview.3.21201.2) includes several bug fixes and ongoing improvements to infrastructure to support upcoming features including compiled models and temporal tables

.NET 6 has been tested with Visual Studio 16.10 Preview 1 and Visual Studio for Mac 8.9. We recommend you use those builds if you want to try .NET 6.

> For Linux users: .NET SDK 6 Preview 3 resolves an issue where [NuGet restore fails on Linux](https://devblogs.microsoft.com/nuget/net-5-nuget-restore-failures-on-linux-distributions-using-nss-or-ca-certificates/) due to expired NuGet certificates and unfortunate interactions with changes made to root certificates stores on Linux, carried by ca-certificates and nss packages. Please update your [.NET 5](https://devblogs.microsoft.com/dotnet/net-april-2021-updates/) and .NET 6 deployments.

## Support

.NET 6 will be will be released in November 2021, and will be supported for three years, as a [Long Term Support (LTS) release](https://github.com/dotnet/core/blob/master/release-policies.md). The [platform matrix](https://github.com/dotnet/core/blob/master/release-notes/6.0/6.0-supported-os.md) has been significantly expanded.

The additions are:

- Android.
- iOS.
- Mac and Mac Catalyst, for x64 and Apple Silicon (AKA "M1").
- Windows Arm64 (specifically Windows Desktop).

.NET 6 Debian container images are based on Debian 11 ("bullseye"), which is [currently in testing](https://wiki.debian.org/DebianBullseye).

## Libraries

The following APIs and improvements have been added to the .NET libraries.

## Faster handling of struts as Dictionary values

A new *unsafe* api -- [CollectionsMarshal.GetValueRef](https://github.com/dotnet/runtime/pull/49388) -- has been added that makes updating struct values in Dictionaries faster. The new API is intended for high performance scenarios, not for general purpose use. It returns a `ref` to the struct value which can then be updated in place with typical techniques.

Prior to this change, updating `struct` dictionary values can be expensive for high-performance scenarios, requiring a dictionary lookup and a copy to stack of the `struct`; then after changing the `struct`, it needs to be assigned to the dictionary key again resulting in another look up and copy operation. This improvement reduces the key hashing to 1 (from 2) and removes all the struct copy operations.

Example:

```cs
ref MyStruct value = CollectionsMarshal.GetValueRef(dictionary, key);
// Returns Unsafe.NullRef<TValue>() if it doesn't exist; check using Unsafe.IsNullRef(ref value)
if (!Unsafe.IsNullRef(ref value))
{
    // Mutate in-place
    value.MyInt++;
}
```

Credit to [Ben Adams](https://github.com/benaadams).

## Faster interface checking and casting

[Interface casting performance has been boosted](https://github.com/dotnet/runtime/pull/49257) by 16% - 38%. This improvement is particularly useful for C#'s pattern matching to and between interfaces.

![image](https://user-images.githubusercontent.com/1142958/112411698-3f413600-8d15-11eb-899f-5a0450524b08.png)

|           Method |  Implementation |     Mean | Version  | Ratio      | Improvement |
|----------------- |------------- |---------:|-------- |---------:|---------:|
|     IsInterface1 | Interfaces1  | 2.166 ns | .NET 6 Preview 2 | 1.000 | |
|     IsInterface1 | Interfaces1  | 1.629 ns | .NET 6 Preview 3      | **0.752** | x1.3 |
| IsNotImplemented | Interfaces1  | 2.166 ns | .NET 6 Preview 2 | 1.000 | |
| IsNotImplemented | Interfaces1  | 1.950 ns | .NET 6 Preview 3      | **0.900** | x1.1 |
|                  |              |          |         |       |
|     IsInterface1 | Interfaces6  | 2.155 ns | .NET 6 Preview 2 | 1.000 | |
|     IsInterface1 | Interfaces6  | 1.936 ns | .NET 6 Preview 3      | **0.898** | x1.1 |
|     IsInterface2 | Interfaces6  | 2.161 ns | .NET 6 Preview 2 | 1.000 | |
|     IsInterface2 | Interfaces6  | 1.908 ns | .NET 6 Preview 3      | **0.883** | x1.1 |
|     IsInterface3 | Interfaces6  | 2.188 ns | .NET 6 Preview 2 | 1.000 | |
|     IsInterface3 | Interfaces6  | 1.672 ns | .NET 6 Preview 3      | **0.764** | x1.3 |
|     IsInterface4 | Interfaces6  | 2.346 ns | .NET 6 Preview 2 | 1.000 | |
|     IsInterface4 | Interfaces6  | 1.965 ns | .NET 6 Preview 3      | **0.838** | x1.2 |
|     IsInterface5 | Interfaces6  | 3.122 ns | .NET 6 Preview 2 | 1.000 | |
|     IsInterface5 | Interfaces6  | 2.173 ns | .NET 6 Preview 3      | **0.696** | x1.4 |
|     IsInterface6 | Interfaces6  | 3.472 ns | .NET 6 Preview 2 | 1.000 | |
|     IsInterface6 | Interfaces6  | 2.687 ns | .NET 6 Preview 3      | **0.774** | x1.3 |
| IsNotImplemented | Interfaces6  | 3.644 ns | .NET 6 Preview 2 | 1.000 | |
| IsNotImplemented | Interfaces6  | 3.071 ns | .NET 6 Preview 3      | **0.843** | x1.2 |

One of the biggest advantages of moving parts of the .NET runtime from C++ to managed C# is that it lowers the barrier to contribution. This includes interface casting, which was moved to C# as an early .NET 6 change. Many more people in the .NET ecosystem are literate in C# than C++ (and the runtime isn't even regular C++). Just being able to read some of the code that composes the runtime is a major step to developing confidence in contributing, in its various forms.

Credit to [Ben Adams](https://github.com/benaadams).

## Runtime: Codegen

The following changes improve code generation in RyuJIT, either making the process more efficient or the resulting code faster to run. A recent post on [Loop Alignment](https://devblogs.microsoft.com/dotnet/loop-alignment-in-net-6/) does a great job of demonstrating the level of consideration and detail that is required for optimal performance.

### Optimizations:
- [Remove bounds checks with tests against length](https://github.com/dotnet/runtime/pull/40180) Credit: [Nathan Moore](https://github.com/nathan-moore)
- [Span bounds check elision with top-level range check nodes removal](https://github.com/dotnet/runtime/pull/49271) Credit [SingleAccretion](https://github.com/SingleAccretion)
- [Support loop cloning of byte array accesses](https://github.com/dotnet/runtime/pull/48894)
- [JIT: Non-void ThrowHelpers](https://github.com/dotnet/runtime/pull/48589)
- [CSE for floating-point constants](https://github.com/dotnet/runtime/pull/44419)
- [Invariant static readonly fields to enable CSE/Hoist-from-loops optimizations](https://github.com/dotnet/runtime/pull/44562)
- [Fold more with null checks for const string](https://github.com/dotnet/runtime/pull/49930)
- [Zero init elimination: Don’t init tracked temps without GC fields](https://github.com/dotnet/runtime/pull/49879)

### Dynamic PGO https://github.com/dotnet/runtime/issues/43618 
- JIT: profile updates for return merges and tail calls  https://github.com/dotnet/runtime/pull/48773
- Class profile: Use unknown placeholder for collectible class typehandle https://github.com/dotnet/runtime/pull/48707 

### Keep structs in register https://github.com/dotnet/runtime/issues/43867
- Completed struct improvement, part1: create more LCL_FLD https://github.com/dotnet/runtime/pull/48377
- Improve liveness for 'STORE_BLK(lcl_var)' https://github.com/dotnet/runtime/pull/48797 

### Completed .NET 6 EH Write Thru
- [JIT: Enable EH Write Thru by default](https://github.com/dotnet/runtime/issues/35923)
- [Enregister EH var that are single def](https://github.com/dotnet/runtime/pull/47307)
- [7 ~ 18% perf improvement](https://github.com/DrewScoggins/performance-2/issues/4275)

![performance improvement](https://user-images.githubusercontent.com/63486087/112660549-d213ca80-8e12-11eb-8f6b-6c692dd1824a.png)

## Tools: Initial .NET Hot Reload support now available for web apps

Early support for .NET Hot Reload is now available for ASP.NET Core & Blazor projects using `dotnet watch`. .NET Hot Reload applies code changes to your running app without restarting it and without losing any app state. Code changes that cannot be applied to the running app can still be applied by rebuilding and restarting the app.

This is just the first step in our more comprehensive plan to bring this technology to all .NET developers including desktop (WPF, WinUI, WinForms), cross-platform client scenarios in .NET MAUI, and more. .NET Hot Reload will be supported with these additional platforms in future preview releases of .NET 6. In addition, we will also make .NET Hot Reload available as an integrated experience in future Visual Studio releases.

For more details on trying out hot reload with web app projects, see [ASP.NET Core updates in .NET 6 Preview 3](https://devblogs.microsoft.com/aspnet/asp-net-core-updates-in-net-6-preview-3/.)

## Closing

We're now about half way through the .NET 6 release, at least in terms of feature development. You can read about other features that have been added, in the [Preview 2](https://devblogs.microsoft.com/dotnet/announcing-net-6-preview-2/) and [Preview 1](https://devblogs.microsoft.com/dotnet/announcing-net-6-preview-1/) releases. We expect some larger features to land in the next few previews before we start into focusing on quality in the release candidate builds.

I started the post raising visibility on some great community contributions. Some community folks are using the [GitHub Sponsors](https://github.com/sponsors) program to support spending more of their time on contributing to .NET. That's great. [Ben is an example](https://github.com/sponsors/benaadams). I encourage you to consider supporting community developers improving .NET, perhaps focused on a part of the product you depend on significantly. Its a good opportunity to invest in your supply chain.
