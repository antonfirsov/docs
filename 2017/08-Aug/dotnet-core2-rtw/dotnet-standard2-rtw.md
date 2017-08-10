# Announcing .NET Standard 2.0

The [.NET Standard 2.0 specification][ns20] is
now complete. It is supported in [.NET Core 2.0][netcore20-post],
in the [.NET Framework 4.6.1 and later versions][netfx],
and in Visual Studio. You can start using .NET Standard 2.0 today.

## For the impatient: TL;DR

* **.NET Standard is for sharing code**. .NET Standard is a set of APIs that all .NET
  platforms have to implement. This unifies the .NET implementation and prevents future
  fragmentation. It replaces Portable Class Libraries (PCLs) as the tool for building
  .NET libraries that work everywhere.
* **Much bigger API Surface**: We have more than doubled the set of available APIs
  from **13k** in [.NET Standard 1.6][ns16]
  to **32k** in [.NET Standard 2.0][ns20].
  Most of the added APIs are .NET Framework APIs. These additions make it much easier to port
  existing code to .NET Standard, and, by extension, to any .NET implementation of .NET Standard,
  such as .NET Core 2.0 and the upcoming version of UWP.
* **.NET Framework compatibility mode**: The vast majority of NuGet packages are currently
  still targeting .NET Framework. Many projects are currently blocked from moving to .NET Standard
  because not all their dependencies are targeting .NET Standard yet. That's why we added a
  compatibility mode that allows .NET Standard projects to depend on .NET Framework libraries as
  if they were compiled for .NET Standard. Of course, this may not work in all cases (for instance,
  if the .NET Framework binaries uses WPF), but we found that
  [70% of all NuGet packages on nuget.org are API compatible][nsnuget]
  with .NET Standard 2.0, so in practice it unblocks many projects.
* **Broad platform support**. .NET Standard 2.0 is [supported on the following platforms][nsversions]:
    - .NET Framework 4.6.1
    - .NET Core 2.0
    - Mono 5.4
    - Xamarin.iOS 10.14
    - Xamarin.Mac 3.8
    - Xamarin.Android 7.5
    - UWP is work in progress and will ship later this year.

## Walkthrough

* How do you create a .NET Standard library project
    - dotnet new lib
    - File | New | Project | .NET Standard | Class Library (.NET Standard)
    - Adding a reference to .NET Framework-only NuGet package
    - Warning & Suppression

* Converting an existing class library to .NET Standard

## What about Portable Class Libraries?

* Screenshot of the now deprecated templates
* We no longer support using PCLs for targeting .NET Standard

## Summary

.NET Standard 2.0 has doubled the APIs since .NET Standard 1.x which means it's
now much easier to port existing code from .NET Framework to .NET Standard. It
also has a compatibility mode for referencing existing .NET Framework binaries
from .NET Standard. This allows you to get started although not all of your
dependencies have ported to .NET Standard yet.

Virtually all .NET implementations have support for .NET Standard 2.0, including
.NET Framework, .NET Core, and Xamarin. UWP support will come later this year.
All these platform benefit from the added APIs and the compatibility mode,
especially .NET Core and UWP which used to have a much more constrained API set.

**Are you building applications?** Then you should convert your business logic
and UI independent code to .NET Standard. This ensures now matter where you
business needs to go -- desktop, mobile, or cloud -- your code can largely come
across for the ride.
    
**Are you building NuGet packages**? Then move to .NET Standard 2.0. You get a
lot of APIs without compromising on reach. And your consumers will love it to!

Please let us know what you think in the comments below.

[ns16]: https://github.com/dotnet/standard/blob/master/docs/versions/netstandard1.6.md
[ns20]: https://github.com/dotnet/standard/blob/master/docs/versions/netstandard2.0.md
[nsversions]: https://github.com/dotnet/standard/blob/master/docs/versions.md
[nsnuget]: https://www.youtube.com/watch?v=iIlQer4LEac
[netfx]: https://github.com/Microsoft/dotnet/blob/master/releases/README.md
[netcore20-post]: https://blogs.msdn.microsoft.com/dotnet/2017/08/14/announcing-net-core-2-0/