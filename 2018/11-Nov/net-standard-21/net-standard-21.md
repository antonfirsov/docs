# Announcing .NET Standard 2.1

We [shipped .NET Standard 2.0][ns20-post] about a year ago. Since then, .NET
Core has shipped one minor version (.NET Core 2.1) and is about to ship another
one (.NET Core 2.2). So it's time to update the standard to include some new
concepts as well as a number of small improvements that make your life easier
across the various implementations of .NET.

## What's new in .NET Standard 2.1?

In total, about 3k APIs are planned to be added in .NET Standard 2.1. A good
chunk of them are brand-new APIs while others are existing APIs that we added to
the standard in order to converge the .NET implementations even further.

Here are the highlights:

* **`Span<T>`**. In .NET Core 2.1 we've added `Span<T>` which is an array-like
  type that allows representing managed and unmanaged memory in a uniform way
  and supports slicing without copying. It's at the heart of most
  performance-related improvements in .NET Core 2.1. Since it allows managing
  buffers in a more efficient way, it can help in reducing allocations and
  copying. We consider `Span<T>` to be a very fundamental type as it requires
  runtime and compiler support in order to be fully leveraged. If you want to
  learn more about this type, make sure to read [Stephen Toub's excellent
  article on `Span<T>`][span-article].

* **Foundational-APIs working with spans**. While `Span<T>` is available as a
  .NET Standard compatible NuGet package (`System.Memory`) already, adding this
  package cannot extend the members of .NET Standard types that deal with spans.
  For example, .NET Core 2.1 added many APIs that allow working with spans, such
  as `Stream.Read(Span<Byte>)`. Part of the value proposition to add span to
  .NET Standard is to add theses companion APIs as well.

* **Reflection emit**. To boost productivity, the .NET ecosystem has always made
  heavy use of dynamic features such as reflection and reflection emit. Emit is
  often used as a tool to optimize performance as well as a way to generate
  types on the fly for proxying interfaces. As a result, many of you asked for
  reflection emit to be included in the .NET Standard. Previously, we've tried
  to provide this via a NuGet package but we discovered that we cannot model
  such a core technology using a package. With .NET Standard 2.1, you'll have
  access to Lightweight Code Generation (LCG) as well as Reflection Emit. Of
  course, you might run on a runtime that doesn't support running IL via
  interpretation or compiling it with a JIT, so we also exposed two new
  capability APIs that allow you to check for the ability to generate code at
  all (`RuntimeFeature.IsDynamicCodeSupported`) as well as whether the generated
  code is interpreted or compiled (`RuntimeFeature.IsDynamicCodeCompiled`). This
  will make it much easier to write libraries that can exploit these
  capabilities in a portable fashion.

* **SIMD**. .NET Framework and .NET Core had [support for SIMD][simd-post] for a
  while now. We've leveraged them to speed up basic operations in the BCL, such
  as string comparisons. We've received quite a few requests to expose these
  APIs in .NET Standard as the functionality requires runtime support and thus
  cannot be provided meaningfully as a NuGet package.

* **`ValueTask` and `ValueTask<T>`**. In [.NET Core 2.1][netcore-21], the
  biggest feature was improvements in our fundamentals to support
  high-performance scenarios, which also included making `async`/`await` more
  efficient. `ValueTask<T>` already exists and allows to return results if the
  operation completed synchronously without having to allocate a new `Task<T>`.
  With .NET Core 2.1 we've improved this further which made it useful to have a
  corresponding non-generic `ValueTask` that allows reducing allocations even
  for cases where the operation has to be completed asynchronously, a feature
  that types like `Socket` and `NetworkStream` now utilize. Exposing these APIs
  in .NET Standard 2.1 enables library authors to benefit from these
  improvements both, as a consumer, as well as a producer.

* **DbProviderFactories**. In .NET Standard 2.0 we added almost all of the
  primitives in ADO<span></span>.NET to allow OR mappers and database
  implementers to communicate. Unfortuantely, `DbProviderFactories` didn't make
  the cut for 2.0 so we're adding it now. In a nutshell, `DbProviderFactories`
  allows OR mappers to instantiate a specific provider factory without having to
  accept an instance of `DbProviderFactory`, which enables them to select the
  appropriate provider automatically, by, for example, reading configuration
  settings.

* **General Goodness**. Since .NET Core was open sourced, we've added many small
  features across the base class libraries such as `System.HashCode` for
  combining hash codes or new overloads on `System.String`. There are about 800
  new members in .NET Core and virtually all of them got added in .NET Standard
  2.1.

For more details, you might want to check out the [full API diff][ns21-diff]
between .NET Standard 2.1 and .NET Standard 2.0. You can also use [apisof.net]
to quickly check whether a given API will be included with .NET Standard 2.1.

## .NET platform support

In [Update on .NET Core 3.0 and .NET Framework 4.8][hunters-post] we wrote the
following:

> **.NET Framework** is the implementation of .NET that's installed on over one
billion machines and thus needs to remain as compatible as possible. Because of
this, it moves at a slower pace than .NET Core. Even security and bug fixes can
cause breaks in applications because applications depend on the previous
behavior. We will make sure that .NET Framework always supports the latest
networking protocols, security standards, and Windows features.
>
> **.NET Core** is the open source, cross-platform, and fast-moving version of
.NET. Because of its side-by-side nature it can take changes that we can't risk
applying back to .NET Framework. This means that .NET Core will get new APIs and
language features over time that .NET Framework cannot. At Build we showed a
demo how the file APIs are faster on .NET Core. If we put those same changes
into .NET Framework we could break existing applications, and we don't want to
do that.

As you saw earlier, a large chunk of the API additions in .NET Standard 2.1
require runtime changes in order to be meaningful. Thus, our plan is that .NET
Framework 4.8 will not implement .NET Standard 2.1 but will remain on .NET
Standard 2.0. .NET Core 3.0 as well as upcoming versions of Xamarin, Mono, and
Unity will be updated to implement .NET Standard 2.1.

Library authors who need to support .NET Framework customers should stay on .NET
Standard 2.0. In fact, most libraries should be able to stay on .NET Standard
2.0, as the API additions are largely for advanced scenarios. However, this
doesn't mean that library authors cannot take advantage of these APIs even if
they have to support .NET Framework. In those cases they can use multi-targeting
to compile for both .NET Standard 2.0 as well as .NET Standard 2.1. This allows
writing code that can expose more features or provide a more efficient
implementation on runtimes that support .NET Standard 2.1 while not giving up on
the bigger reach that .NET Standard 2.0 offers.

For more recommendations on targeting, check out the brand new documentation on
[cross-platform targeting][cross-docs].

## Governance model

The .NET Standard 1.x and 2.0 releases were mostly about exposing existing
concepts. The bulk of the work was on the .NET Core side, as this platform
started with a much smaller API set. Moving forward, we'll often have to
standardize brand-new technologies, which means we need to consider the impact
on all .NET implementations, not just .NET Core. However, some of them are
managed outside of Microsoft, such as by the Mono community or by Unity. This
required creating a governance model that honors these constraints.

We've made two changes:

**We formed a .NET Standard review board**. To ensure we don't end up adding
large chunks of API surface that cannot be implemented, we have a review board
that has to sign-off on API additions to the .NET Standard. The board comprises
representatives from the following groups:

* **.NET platform**. The rationale here is that most, if not all, of the APIs
  that are part of .NET Standard are implemented and evolved by the .NET
  platform team.
* **Xamarin & Mono**. While Mono mostly copies code from the .NET Framework &
  .NET Core base class libraries, changes and extensions can impact their
  ability to support the .NET Standard. Thus, we need to coordinate any changes
  with Mono.
* **Unity**. Same rationale as for Xamarin & Mono.
* **.NET Foundation**. A set of people selected by the .NET Foundation that
  represent the interests of the .NET community at large, which also includes
  pure consumers of the .NET Standard.

The chair of the review board is Miguel de Icaza. For the most part, we strive
to make decisions based on consensus, but as it is with every group of
engineers, consensus can be impossible to achieve at times so we need to have an
ultimate tie breaker. And Miguel has a lot of expertise and experience building
.NET implementations that are supported by multiple parties.

**We created a formal approval process**. The .NET Standard 1.x and 2.0 version
were largely mechanically derived by computing which APIs existing .NET
implementations had in common, which means the API sets were effectively a
computational outcome. Moving forward, this won't be the case anymore so we
needed a process that allows an editorial approach:

* **Anybody can submit proposals for API additions to the .NET Standard**.
* **New members on standardized types are automatically considered**. To prevent
  accidental fragmentation, we'll automatically consider all members added by
  any .NET implementation on types that are already in the standard. The
  rationale here is that divergence at that the member level is not desirable
  and unless there is something wrong with the API it's likely a good addition.
* **Acceptance requires**
    - **A sponsorship from a review board member**. That person will be assigned
      the issue and is expected to shepherd the issue until it's either accepted
      or rejected. If no board member is willing to sponsor the proposal, it's
      considered rejected.
    - **A stable implementation in at least one .NET implementation**. The
      implementation must be licensed under an open source license that is
      compatible with MIT. This will allow other .NET implementations to jump-
      start their own implementations or simply take the feature as-is.
* **.NET Standard updates are planned and will generally follow a set of
  themes**. We avoid releases with a large number of tiny features that aren't
  part of a common set of scenarios. Instead, we try to define a set of goals
  that describe what kind of feature areas a particular .NET Standard version
  provides. This simplifies answering the question which .NET Standard a given
  library should depend on. It also makes it easier for .NET implementations to
  decide whether it's worth implementing a higher version of .NET Standard.
* **The version number** is subject to discussion and is generally a function of
  how significant the new version is. While we aren't planning on making
  breaking changes, we'll rev the major version if the new version adds large
  chunks of APIs (like when we doubled the number of APIs in .NET Standard 2.0)
  or has sizable changes in the overall developer experience (like the added
  compatibility mode for consuming .NET Framework libraries we added in .NET
  Standard 2.0).

For more information, take a look at the [.NET Standard governance model][gov]
and the [.NET Standard review board][review-board].

## Summary

The definition of .NET Standard 2.1 is ongoing. You can watch [our progress on
GitHub][ns21-ms] and still file requests.

If you want to quickly check whether a specific API is in .NET Standard (or any
other .NET platform), you can use [apisof.net]. You can also use the [.NET
Portability Analyzer][APIPort] to check whether an existing project or binary
can be ported to .NET Standard 2.1.

Happy coding!

[ns20-post]: https://blogs.msdn.microsoft.com/dotnet/2017/08/14/announcing-net-standard-2-0/
[ns21-diff]: https://github.com/dotnet/standard/blob/master/docs/versions/netstandard2.1.md
[ns21-planning]: https://github.com/dotnet/standard/tree/master/docs/planning/netstandard-2.1
[ns21-ms]: https://github.com/dotnet/standard/milestone/3
[apisof.net]: https://apisof.net/catalog/System.Span%3CT%3E
[span-article]: https://msdn.microsoft.com/en-us/magazine/mt814808.aspx
[simd-post]: Pworhttps://blogs.msdn.microsoft.com/dotnet/2014/04/07/the-jit-finally-proposed-jit-and-simd-are-getting-married/
[netcore-21]: https://blogs.msdn.microsoft.com/dotnet/2018/04/18/performance-improvements-in-net-core-2-1/
[hunters-post]: https://blogs.msdn.microsoft.com/dotnet/2018/10/04/update-on-net-core-3-0-and-net-framework-4-8/]
[cross-docs]: https://docs.microsoft.com/en-us/dotnet/standard/library-guidance/cross-platform-targeting
[gov]: https://github.com/dotnet/standard/tree/master/docs/governance/README.md
[review-board]: https://github.com/dotnet/standard/blob/master/docs/governance/board.md
[APIPort]: [https://docs.microsoft.com/en-us/dotnet/standard/analyzers/portability-analyzer]
