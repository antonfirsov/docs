Announcing September 2016 Updates for .NET Core 1.0 
===================================================

Today, we are releasing a set of reliability and quality updates for .NET Core 1.0. The quickest way to get the updates is to head over to [dot.net/core](https://dot.net/core) and follow the updated install instructions for your operating system. You can download and install the update as an MSI for Windows, a PKG for macOS and updated [zips](https://www.microsoft.com/net/download#core) and packages for Linux OSes. If you don't yet have .NET Core, you can start with this release. It contains everything you need to get started.

We are calling this release .NET Core 1.0.1. It is the first .NET Core 1.0 [Long Term Support (LTS)](https://blogs.msdn.microsoft.com/dotnet/2016/07/26/net-support-and-versioning/) update. We recommend that everyone move to it immediately. You need to be on the latest LTS release to get support from Microsoft.     

We are also releasing [updates for ASP.NET Core 1.0]() and [Entity Framework Core 1.0]() today. The ASP.NET Core release includes an update for a [security advisory](https://github.com/aspnet/Announcements/issues/203).

Updates in .NET Core 1.0.1
-------

The following is a summary of updates/fixes in the .NET Core 1.0.1 release, which you can get by [installing .NET Core 1.0.1](https://dot.net/core).

- Segfaults on Linux 4.6 - [coreclr 6016](https://github.com/dotnet/coreclr/issues/6016); [coreclr 5837](https://github.com/dotnet/coreclr/issues/5837)
- Access violation on Windows - [coreclr 6460](https://github.com/dotnet/coreclr/issues/6460)
- F# template has been updated for .NET Core 1.0 - [cli 3789](https://github.com/dotnet/cli/pull/3789)
- Update ASP.NET Core templates to reference ASP.NET Core 1.0.1 - [cli 3948](https://github.com/dotnet/cli/pull/3948)
- Update ASP.NET Core templates to correctly publish CSHTML files - [cli 3950](https://github.com/dotnet/cli/pull/3950)

The following is a summary of updates/fixes in the ASP.NET Core and Entity Framework Core 1.0.1 releases, which you can get by referencing updated NuGet packages.

- Microsoft Security Advisory 3181759 : Vulnerabilities in ASP.NET Core View Components Could Allow Elevation of Privilege - [aspnet 203](https://github.com/aspnet/Announcements/issues/203)
- MVC updates for FIPS compliance - [mvc 5103](https://github.com/aspnet/Mvc/issues/5103); [Antiforgery 95](https://github.com/aspnet/Antiforgery/issues/95)
- HTTP Verbs mapping error GET and DELETE - [mvc 5038](https://github.com/aspnet/Mvc/issues/5038)
- ResponseBody and the corresponding stream is replicated in future requests in some cases [KestrelHttpServer 1028](https://github.com/aspnet/KestrelHttpServer/issues/1028)
- Several Entity Framework Core updates

You can read the [release notes for .NET Core, ASP.NET Core and Entity Framework 1.0.1](https://go.microsoft.com/fwlink/?LinkID=827612) to learn about the specific changes that are included, including the commits that the release was built from.

We've talked about [future updates to .NET Core](https://blogs.msdn.microsoft.com/dotnet/2016/07/15/net-core-roadmap/), including adopting MSBuild and establishing .NET Standard 2.0. These changes will be included in an upcoming feature release and not in a patch version like the one we are releasing today.

Updating your Machine to use .NET Core 1.0.1
--------------------------------------------

.NET Core versions are installed side-by-side. After [installing .NET Core 1.0.1](https://dot.net/core), you will have two .NET Core versions on your machine, assuming that you already had .NET Core 1.0.0 installed. The installation of .NET Core 1.0.1 *does not* delete or otherwise update your .NET Core 1.0.0 installation.

.NET Core has a roll-forward policy for patch versions (the 3rd part of the version number, per [semver](http://semver.org/)). We only include important quality and reliability updates in patch versions. We do not include new features in patch versions. After you install .NET Core 1.0.1, apps that previously used .NET Core 1.0.0 will automatically use .NET Core 1.0.1. This policy results in apps running on a more secure and reliable version of .NET Core. The roll-forward policy only applies to patch versions. For example, an app built for .NET Core 1.0.0 will roll forward to 1.0.100, but not to 1.1.0 or 2.0.0.  

Updating your Application to use .NET Core 1.0.1
------------------------------------------------

You don't need to do anything to get your application to use .NET Core 1.0.1. It will roll-forward to .NET Core 1.0.1 by default, as discussed above. You can set .NET Core 1.0.1 as the minimum version for your app by updating your .NET Core metapackage reference to [Microsoft.NETCore.App 1.0.1](https://www.nuget.org/packages/Microsoft.NETCore.App/1.0.1). An app configured this way will no longer run on .NET Core 1.0.0, even if it's the only .NET Core version on a machine. 

You do need to make a change to update to ASP.NET Core 1.0.1 and Entity Framework 1.0.1, by referencing a newer NuGet package. You can see examples of that in the [ASP.NET Core 1.0.1 blog post]().

What you can do if you have trouble with .NET Core 1.0.1
--------------------------------------------------------

First, if you are having trouble, we want to know about it. Please report issues on GitHub issue 9999 or email us at [dotnet@microsoft.com](mailto:dotnet@microsoft.com). 

There are a few options to work around having trouble with .NET Core 1.0.1. 

You can opt specific apps out of the roll-forward policy. This approach is only encouraged as a short-term option, to determining why an app isn't working correctly and to fix it. Apps that are configured this way for an extended period of time may run in an insecure state without anyone realizing it. You can configure an app to not roll forward by setting `applyPatches` to false in [app.runtimeconfig.json](https://github.com/dotnet/cli/blob/rel/1.0.0/Documentation/specs/runtime-configuration-file.md#appnameruntimeconfigjson) file. Again, it's not recommended to use this approach for an extended period of time. 

The other approach is to uninstall .NET Core 1.0.1. Your machine will return to using .NET Core 1.0.0, assuming that you already had it installed. You can uninstall .NET Core 1.0.1 using the installer or delete the 1.0.1 Microsoft.NETCore.App directory manually. You can find out where .NET Core is installed on your machine by using `where` on Windows and `which` on macOS. Example: `where dotnet`.

- Windows: C:\Program Files\dotnet\shared\Microsoft.NETCore.App
- macOS: /usr/local/share/dotnet/shared/Microsoft.NETCore.App

On Linux, you can uninstall .NET Core 1.0.1 with your package manager if you installed it that way. If you installed it via a .zip or .tar.gz then you can use simple file operations to add or remove .NET Core versions. If you don't know how .NET Core was installed and don't know how to find it, you can use the following command to find it:

```
find -L / -maxdepth 5 -name "shared" 2>/dev/null | grep '/dotnet/shared'
```

Closing
-------

Please upgrade to today's release. We've included security and reliability updates that are important to ensure that your apps are secure, reliable and have great performance. Today's release is the first update for .NET Core. As we make future updates, the approach to updating your .NET Core installation and your apps will get more familiar and straightforward.

Thanks to everyone who has installed and used .NET Core, ASP.NET Core and Entity Framework Core. We appreciate all of the feedback that we've received from you and please keep it coming.
