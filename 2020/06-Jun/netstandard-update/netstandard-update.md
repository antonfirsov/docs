# The future of .NET Standard

Since [.NET 5 was announced][net5-post], many of you have asked what this means
for .NET Standard and whether it will still be relevant. In this post, I'm going
to explain how .NET 5 improves code sharing and replaces .NET Standard. I'll
also cover the cases where you still need .NET Standard.

## For the impatient: TL;DR

.NET 5 will be a shared code base for .NET Core, Mono, Xamarin, and future .NET
implementations:

![.NET 5 vision](net5-vision.gif)

To better reflect this, we've updated the [target framework names (TFMs)][net5-tfms]:

* `net5.0`. This is for code that runs everywhere. It combines and replaces the
  `netcoreapp` and `netstandard` names. This TFM will generally only include
  technologies that work cross-platform (except for pragmatic concessions, like we
  already did in .NET Standard).

* `net5.0-android`, `net5.0-ios`, and `net5.0-windows`. These TFMs represent OS
  specific flavors of .NET 5 that include `net5.0` plus OS-specific bindings.

There isn't going to be a new version of .NET Standard, but .NET 5 and all future
versions will continue to support .NET Standard 2.1 and earlier. You should
think of `net5.0` (and future versions) as the foundation for sharing code
moving forward.

## Problems with .NET Standard

.NET Standard has made it much easier to create libraries that work on all .NET
platforms. But there are still three problems with .NET Standard:

1. **It [versions slowly][problem-1]**, which means you can't easily use the
   latest features.
2. **It needs a [decoder ring][problem-2]** to map versions to .NET
   implementations.
3. **It [exposes platform-specific APIs][problem-3]**, which means you can't
   statically validate whether your code is truly portable.

Let's see how .NET 5 will address all three issues.

## Problem 1: .NET Standard versions slowly

[.NET Standard was designed][ns-post] at a time where the .NET platforms weren't
converged at the implementation level. This made writing code that needs to work
in different environments hard, because different workloads used different .NET
implementations.

The goal of .NET Standard was to unify the API set of the base class library
(BCL), so that you can write a single library that can run everywhere. And this
has served us really well: .NET Standard is used by [over 30% of all NuGet
packages][ns-growth-post].

> **PENDING**. Requested updated data from Mark

![#Packages supporting .NET Standard](chart-all.png)

But standardizing the API set alone creates a tax. It requires coordination
whenever we're adding new APIs -- which happens all the time. Both us and the
.NET open-source community keep innovating in the BCL providing new language
features, usability improvements, new cross-cutting features such as `Span<T>`,
or supporting new data formats or networking protocols.

And while we can provide new types as NuGet packages, we can't provide new APIs
on existing types this way. So in the general sense, innovation in the BCL
requires shipping a new version of .NET Standard.

Up until .NET Standard 2.0 this wasn't really an issue because we only
standardized *existing* APIs. But in .NET Standard 2.1, we standardized brand new
APIs and that's where we saw quite a bit of friction.

Where does this friction come from?

.NET Standard is an API set that all .NET implementations have to support, so
there is an [editorial aspect][ns-process] to it in that all APIs must be
reviewed by the [.NET Standard review board][ns-board]. The board is comprised
of .NET platform implementers as well as representatives of the .NET community.
The goal is to only standardize APIs that we can truly implement in all current
*and* future .NET platforms. These reviews are necessary because there are
different implementations of the .NET stack, with different constraints.

We predicted this type of friction, which is why we said early on that .NET
Standard [will only standardize APIs][problem-1] that were already shipped in at
least one .NET implementation. This seems reasonable at first, but then you
realize that .NET Standard can't ship very frequently. So, if a feature
misses a particular release, you might have to wait for a couple of years before
it's even available and potentially even longer until this version of .NET
Standard is widely supported.

We felt for some features that opportunity loss was too high, so we did
unnatural acts to standardize APIs that weren't shipped yet (such as
`IAsyncEnumerable<T>`). Doing this for all BCL APIs was simply too expensive,
which is why quite a few features still missed the .NET Standard 2.1 train (such
as the new hardware intrinsics).

But what if there was a single code base? And what if that code base would have
to support all the aspects that make .NET implementations differ today, such
as supporting both just-in-time (JIT) compilation and ahead-of-time
(AOT) compilation?

Instead of doing these reviews as an after thought, we'd make all these aspects
part of the feature design, right from the start. In such a world, the
standardized API set is, by construction, the common API set. When a feature is
implemented, it would already be available for everyone because the code base is
shared.

## Problem 2: .NET Standard needs a decoder ring

Separating the API set from its implementation doesn't just slow down the
availability of APIs. It also means that we need to [map .NET Standard versions
to their implementations][ns-table]. As someone who had to explain this table to
many people over time I've come to appreciate just how complicated this
seemingly simple idea is. We've tried our best to make it easier, but in the
end, it's just inherent complexity because the API set and the implementations
are shipped independently.

We have unified the .NET platforms by adding yet another, synthetic, platform
below them all that represents the common API set. In a very real sense, this
[XKCD-inspired comic][xkcd] is spot on:

![How .NET platforms proliferate][xkcd-img]

We can't solve this problem without truly merging some rectangles in our layer
diagram, which is what .NET 5 does: it provides a unified implementation where
all parties build on the same foundation and thus get the same version number.

## Problem 3: .NET Standard exposes platform-specific APIs

When we designed .NET Standard, [we had to make pragmatic
concessions][problem-3] in order to avoid breaking the library ecosystem too
much. That is, we had to include some Windows-only APIs (such as file system
ACLs, the registry, WMI, and so on).

We didn't have a way to mark these APIs as Windows-only, nor did we have a TFM
to put them into (like we have now with `net5.0-windows`). Many of you have
complained that these feel like "landmines" - the code compiles without
errors and "look" like portable to any platform, but when running on a platform
that doesn't have an implementation for the given API, you get runtime
errors.

In the past, we have experimented with a [Roslyn analyzer][platform-compat] that
detects platform-specific APIs at compile time:

![Detecting platform-specific APIs with an analyzer][platform-compat-img]

However, this analyzer has a few shortcomings:

1. It's experimental so it doesn't ship with the SDK and is not enabled by
   default.
2. It essentially hard codes which APIs are platform-specific.
3. It's not smart enough to understand when APIs are called under a
   platform-guard.
4. It doesn't understand in which version a platform API is available.

Starting with .NET 5, we're going to ship analyzers and code fixers with the SDK
that are on by default. Among those, we're [planning to add one for platform specific
APIs][platform-compat-spec].

This analyzer will be especially valuable for app-models that have a large set
of OS-bindings, such as Android, iOS, and UWP. Technically, WinForms and WPF are
also OS bindings, but they work on all supported versions of Windows, so you never
encounter an API that needs a specific version of Windows (unlike the WinRT APIs
from UWP).

In .NET land, you compile against a specific framework version, which means at
compile-time you can only see APIs that exist in that version of .NET. If you
want to call APIs that are introduced in a later .NET version, you need to
either use reflection, retarget to a higher version (and thus no longer being
able to run on the older version), or use multi-targeting (meaning you produce
two separate binaries for the old and the new .NET version). You can't
just compile against the later version and guard the call at run time with an
`if` check -- the reason is that the runtime needs to be able to resolve the
types and methods you're using at run time which it can't do for the new APIs
when you run on the older version. That's irrespective of whether or not you
actually call the API.

Since OS APIs are native, this requirement doesn't exist. Thus, you can compile
against the latest Android, iOS, or Windows SDK and still run on older versions,
as long as you are only calling the APIs that are actually available at run time.

Let's look at an example. Say you're building an iOS application and you want to
run on iOS 13 while still being able to use the latest APIs if you're running on
the latest version of iOS. Your project file might look like this:

```XML
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net5.0-ios14.0</TargetFramework>
    <TargetPlatformMinVersion>13.0</TargetPlatformMinVersion>
  </PropertyGroup>

  ...

</Project>
```

In our example, Apple added a new API `NSFizzBuff` that provides some cool new
thing to your application, but it's entirely optional.

Our [goal for .NET 5][platform-compat-spec] is to ship the following experience.
If you call the API like this:

```C#
private static void ProvideExtraPop()
{
    NSFizzBuff();
}
```

you'll immediately get a warning such as:

> 'NSFizzBuff' requires iOS 14 or later.

You can then invoke a code fix from the light bulb menu that will add a platform
guard to your code:

```C#
private static void ProvideExtraPop()
{
    if (!RuntimeInformation.IsOSPlatformOrLater(OSPlatform.iOS, 14))
      return;

    NSFizzBuff();
}
```

The warning will then disappear because the analyzer understands that you
checked for the correct version. Alternatively, you can invoke a different code
fixer that will annotate your own method (type or even assembly) to be
platform-specific. This way, you can forward the requirements to your consumers.
This makes it possible to write wrappers around platform-specific APIs as well.

## .NET versioning

As a library author, you're probably wondering when .NET 5 will be widely
supported. Moving forward, we'll ship .NET every year in November, with every
other year being a long-term-support (LTS) release.

.NET 5 will ship in November 2020 and .NET 6 will ship in November 2021 as an
LTS. We created this fixed schedule to make it easier for you to plan your
updates (if you're an app developer) and predict the demand for supported .NET
versions (if you're a library developer).

Thanks to the ability to install .NET Core side-by-side, new versions are
adopted fairly fast with LTS versions being the most popular. In fact, .NET Core
3.1 was the fasted adopted .NET version ever.

![.NET 5 Schedule](net5-schedule.png)

The expectation is that every time we ship, we ship all framework names in
conjunction, for example, it might look something like this:

|.NET 5            | .NET 6            | .NET 7            |
|------------------|-------------------|-------------------|
|`net5.0`          | `net6.0`          | `net7.0`          |
|`net5.0-android`  | `net6.0-android`  | `net7.0-android`  |
|`net5.0-ios`      | `net6.0-ios`      | `net7.0-ios`      |
|`net5.0-windows`  | `net6.0-windows`  | `net7.0-windows`  |
|                  | `net6.0-somenewos`| `net7.0-somenewos`|
|`net5.0-someoldos`|                   |                   |

This means that you can generally expect that whatever innovation we did in the
BCL, you're going to be able to use it from all app models, no matter which
platform they run on. It also means that libraries shipped for the latest `net`
framework can always be consumed from all app models, as long as you run the latest
version of them.

This model removes the complexity around .NET Standard versioning because each
time we ship, you can assume that all platforms are going to support the new
version immediately and completely. And we cement this promise by using the
prefix naming convention.

However, we might add support for new platforms (illustrated by
`net6.0-somenewos`) and we might drop support for platforms that are no longer
relevant (illustrated by `net5.0-someoldos`). But dropping platforms will be a
big deal and we'll announce these decisions well in advance, so these changes
should never surprise you.

## .NET 5 as the combination of .NET Standard & .NET Core

.NET 5 and subsequent versions will be a single code base that supports desktop
apps, mobile apps, cloud services, web sites, and whatever environment .NET will
run on tomorrow.

You might think "hold on, this sounds great, but what if someone wants to create
a completely new implementation". That's fine too. But virtually nobody will
start one from scratch. Most likely, it will be a fork of the current code base
([dotnet/runtime]), and even that might not be necessary. For example, Tizen
(the Samsung platform for smart appliances) uses an unchanged .NET Core
runtime/framework with a Samsung-specific app model on top.

And, even in cases where changes at the runtime or framework-level are
necessary, forking preserves a merge relationship, which allows maintainers to
keep pulling in new changes from the [dotnet/runtime] repo, benefiting from BCL
innovations in areas unaffected by their changes.

Granted, there are cases where one might want to create a very different "kind"
of .NET, such as a minimal runtime without the current BCL. But that would
mean that it couldn't leverage the existing .NET library ecosystem anyway, which
means it wouldn't have implemented .NET Standard either. We're generally not
interested in pursuing this direction, but the convergence of .NET Standard and
.NET Core doesn't prevent that nor does it make it any harder.

## What you should target

.NET 5 and all future versions will always support .NET Standard 2.1 and
earlier. The only reason to retarget from .NET Standard to .NET 5 is to gain
access to more APIs. So you can think of .NET 5 as .NET Standard 2.2.

What about new code? Should you still start with .NET Standard 2.0 or should you
go straight to .NET 5? It depends.

* **App components**. If you're using libraries to break down your application
  into several components, my recommendation is to use `netX.Y` where `X.Y` is
  the lowest number of .NET that your application (or applications) are
  targeting. For simplicity, you probably want all projects that make up your
  application to be on the same version of .NET because it means you can assume
  the same BCL features everywhere.

* **Reusable libraries**. If you're building reusable libraries that you plan on
  shipping on NuGet, you'll want to consider the trade-off between reach and API
  set. .NET Standard 2.0 is the highest version of .NET Standard that is
  supported by .NET Framework, so it will give you the most reach, while also
  giving you a fairly large API set to work with. We'd generally recommend
  against targeting .NET Standard 1.x as it's not worth the hassle anymore. If
  you don't need to support .NET Framework, then you can either go with .NET
  Standard 2.1 or .NET 5. Most code can probably skip .NET Standard 2.1 and go
  straight to .NET 5.

So what should you do? My expectation is that widely used libraries will end up
multi-targeting for both .NET Standard 2.0 and .NET 5: supporting .NET Standard
2.0 gives you the most reach while supporting .NET 5 ensures you can leverage
the latest platform features for customers that are already on .NET 5.

In a couple of years, the choice for reusable libraries will only involve the
version number of `netX.Y`, which is basically how building libraries for .NET
has always worked -- you generally want to support some older version in order
to ensure you get the most reach.

To summarize:

* Use `netstandard2.0` to share code between .NET Framework and all other
  platforms.
* Use `netstandard2.1` to share code between Mono, Xamarin, and .NET Core 3.x.
* Use `net5.0` for code sharing moving forward.

## Summary

`net5.0` is for code that runs everywhere. It combines and replaces the
`netcoreapp` and `netstandard` names. We'll also have platform-specific
frameworks, such as `net5.0-android`, `net5.0-ios`, and `net5.0-windows`.

Since there is no difference between the standard and its implementation, you'll
be able to take advantage of new APIs much quicker than with .NET Standard. And
due to the naming convention, you'll be able to easily tell who can consume a
given library -- without having to consult the .NET Standard version table.

While .NET Standard 2.1 will be the last version of .NET Standard, .NET 5 and
all future versions will continue to support .NET Standard 2.1 and earlier. But
you should think of `netX.Y` as the foundation for sharing code moving forward.

Happy coding!

[problem-1]: https://github.com/dotnet/standard/tree/master/docs/governance#process
[problem-2]: https://dotnet.microsoft.com/platform/dotnet-standard#versions
[problem-3]: https://github.com/dotnet/standard/blob/master/docs/faq.md#why-do-you-include-apis-that-dont-work-everywhere
[net5-post]: https://devblogs.microsoft.com/dotnet/introducing-net-5/
[ns-post]: https://devblogs.microsoft.com/dotnet/introducing-net-standard/
[ns-growth-post]: https://devblogs.microsoft.com/dotnet/update-on-net-standard-adoption/
[ns-process]: https://github.com/dotnet/standard/tree/master/docs/governance#process
[ns-board]: https://github.com/dotnet/standard/blob/master/docs/governance/board.md
[ns-table]: https://dotnet.microsoft.com/platform/dotnet-standard#versions
[net5-tfms]: https://github.com/dotnet/designs/blob/master/accepted/2020/net5/net5.md
[dotnet/runtime]: https://github.com/dotnet/runtime
[xkcd]: https://xkcd.com/927
[xkcd-img]: https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2014/12/7725.Pic6_.png
[platform-compat]: https://github.com/dotnet/platform-compat
[platform-compat-img]: https://github.com/dotnet/platform-compat/raw/master/docs/screenshot1.png
[platform-compat-spec]: https://github.com/dotnet/designs/pull/110
