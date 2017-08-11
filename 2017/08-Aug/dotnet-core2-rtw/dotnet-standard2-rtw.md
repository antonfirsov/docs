# Announcing .NET Standard 2.0

The [.NET Standard 2.0 specification][ns20] is
now complete. It is supported in [.NET Core 2.0][netcore20-post],
in the [.NET Framework 4.6.1 and later versions][netfx],
and in Visual Studio. You can start using .NET Standard 2.0 today.

## For the impatient: TL;DR

* **.NET Standard is for sharing code**. .NET Standard is a set of APIs that all
  .NET implementations have to provide. This unifies the .NET implementations
  and prevents future fragmentation. It replaces Portable Class Libraries (PCLs)
  as the tool for building .NET libraries that work everywhere.
* **Much bigger API Surface**: We have more than doubled the set of available APIs
  from **13k** in [.NET Standard 1.6][ns16]
  to **32k** in [.NET Standard 2.0][ns20].
  Most of them are existing .NET Framework APIs. These additions make it much
  easier to port existing code to .NET Standard, and, by extension, to any .NET
  implementation of .NET Standard, such as .NET Core 2.0 and the upcoming
  version of UWP.
* **.NET Framework compatibility mode**: The vast majority of NuGet packages are
  currently still targeting .NET Framework. Many projects are currently blocked
  from moving to .NET Standard because not all their dependencies are targeting
  .NET Standard yet. That's why we added a compatibility mode that allows .NET
  Standard projects to depend on .NET Framework libraries as if they were
  compiled for .NET Standard. Of course, this may not work in all cases (for
  instance, if the .NET Framework binaries uses WPF), but we found that 
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

## Creating a .NET Standard library

Let's see .NET Standard 2.0 in action by creating a new project. You can do this
in Visual Studio by invoking **File** | **New Project**. Choose **Class Library
(.NET Standard)** from the **.NET Standard** category:

![](net_standard_01_new_project.png)

From the command line, you can use `dotnet new` to create a new library (which
by default is targeting .NET Standard):

```
$ dotnet new lib -o mylibrary
```

## Reusing an existing .NET Framework library

Now let's add a reference to a NuGet package that doesn't target .NET Standard yet,
[Huitian.PowerCollections](https://www.nuget.org/packages/Huitian.PowerCollections).
In Visual Studio, right click your project and choose **Manage NuGet Packages**.
Then select **Browse** and search for **Huitian.PowerCollections**. Click on
**Install**.

As a command line user, you can achieve the same by by using `dotnet add package`:

```
$ dotnet add package Huitian.PowerCollections
```

You'll notice the following warning:

> NU1701: Package 'Huitian.PowerCollections 1.0.0' was restored using
> '.NETFramework,Version=v4.6.1' instead of the project target framework
> '.NETStandard,Version=v2.0'. This package may not be fully compatible with
> your project.

This warning will not just appear when installing the package, but every time
you build. This ensures you don't accidentally overlook it.

The reason for the warning is that NuGet has no way of knowing whether the .NET
Framework library will actually work. For example, it might depend on Windows
Forms. To make sure you don't waste your time troubleshooting something that
cannot work, NuGet lets you know that you're potentially going off the rails. Of
course, warnings you have to overlook are annoying. Thus, we recommend that you
test your application/library and if you're convinced everything is working
fine, you suppress the warning.

If you're using the command line, you'll need to the edit your project file and
add the `NoWarn` attribute on the `PackageReference` that you want to suppress
the warning for:

```xml
<ItemGroup>
  <PackageReference Include="Huitian.PowerCollections" Version="1.0.0" NoWarn="NU1701" />
</ItemGroup>
```

And in Visual Studio, you can simply select the package reference in **Solution
Explorer** and use **Properties** to add the suppression:

![](net_standard_02_suppression.png)

Building the project will now show zero warnings. Notice that the suppression
wasn't global but specific to the package reference. This ensures that just
using one library through the compatibility mode doesn't result in a free ride
for all future references. So if you install another library that needs the
compatibility mode, you'll get the warning again and you'll need to suppress it
for that package too.

## Producing a NuGet package

Once you're happy with your library, you can simply make it a NuGet package. To
do this, right click your project and choose **Properties**. On the **Package**
tab, check the box for **Generate NuGet package on build**:

![](net_standard_03_generatenupkg.png)

If you're using the command line, edit the project file and set the
`GeneratePackageOnBuild` property to `true`:

```xml
<PropertyGroup>
  <TargetFramework>netstandard2.0</TargetFramework>
  <GeneratePackageOnBuild>true</GeneratePackageOnBuild>
</PropertyGroup>
```

If you rebuild the project, you'll now find a NuGet package in the output
directory as well.

## What about Portable Class Libraries?

If you're sharing code between different .NET implementations today, you're
probably aware of Portable Class Libraries (PCLs). With the release of .NET
Standard 2.0, we're now officially deprecating PCLs:

![](net_standard_04_pcls.png)

## Summary

.NET Standard 2.0 has doubled the APIs since .NET Standard 1.x which means it's
now much easier to port existing code from .NET Framework to .NET Standard. It
also adds a compatibility mode for referencing existing .NET Framework binaries
from .NET Standard. This allows you to get started although not all of your
dependencies have ported to .NET Standard yet.

Virtually all .NET implementations have support for .NET Standard 2.0, including
.NET Framework, .NET Core, and Xamarin. UWP support will come later this year.
All these implementations benefit from the added APIs and the compatibility mode,
especially .NET Core and UWP which used to have a much more constrained API set.

**Are you building applications?** Then you should convert your business logic
and UI independent code to .NET Standard. This ensures now matter where you
business needs to go -- desktop, mobile, or cloud -- your code can come across
for the ride.
    
**Are you building NuGet packages**? Then move to .NET Standard 2.0. You get a
lot of APIs without compromising on reach. And your consumers will love it too!

Please let us know what you think in the comments below.

[ns16]: https://github.com/dotnet/standard/blob/master/docs/versions/netstandard1.6.md
[ns20]: https://github.com/dotnet/standard/blob/master/docs/versions/netstandard2.0.md
[nsversions]: https://github.com/dotnet/standard/blob/master/docs/versions.md
[nsnuget]: https://www.youtube.com/watch?v=iIlQer4LEac
[netfx]: https://github.com/Microsoft/dotnet/blob/master/releases/README.md
[netcore20-post]: https://blogs.msdn.microsoft.com/dotnet/2017/08/14/announcing-net-core-2-0/