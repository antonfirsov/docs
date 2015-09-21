What's new for .NET and UWP in Win10 Tools 1.1
================================================

> This post was written by Lucian Wischik, Program Manager on the Managed Languages team.

Last week we updated the [Visual Studio tools for Universal Windows Apps](http://blogs.msdn.com/b/visualstudio/archive/2015/09/16/wintools-1-1-typescript-1-6-rtm-and-tools-for-apache-cordova-updates.aspx). The easiest way to get the update is within Visual Studio, under *Tools > Extensions > Updates*.

The [release notes](https://social.msdn.microsoft.com/Forums/en-US/e9df01f6-1474-4a4e-98fc-2567591c764f/update-11-release-notes-and-installation-instructions?forum=Win10SDKToolsIssues) mention these improvements for .NET:

- Significant bug fixes for Universal Windows apps. 
- Improved globalization and localization, *optional multi-file support - helps reduce the size of app packages*.

*Reduced size of app package*. In this article I'll tell you the what and why and how of this changes. Then I'll tell you how this fits into the wider context -- about how improvements in .NET make their way into your UWP apps.

UWP apps include .NET app-locally
===================================

When you build a UWP app, it includes app-locally all the .NET APIs that it actually uses...

"... But doesn't this get a bit big?"


Here's a concrete example. I wrote a [Game of Life](https://en.wikipedia.org/wiki/Conway%27s_Game_of_Life) simulator that runs on Windows 10 devices, both desktop and mobile, and takes advantage of the [Win2d library](http://microsoft.github.io/Win2D/html/Introduction.htm) to run the simulation fast on each device's graphics accelerator. My source code is [here on github](https://github.com/ljw1004/blog/tree/master/Win2d/GameOfLife). This is what the simulator looks like:
 
![Game of life](game-of-life.gif)

And this is what I get when I build my app in Release mode: 
 
![AppSize](appsize.png) 

*What we're delivering with Win10 Tools 1.1 is the ability to shave off that extra grey segment.*

How to reduce size of app package, and improve build time
===========================================================

The ability to reduce build-time and app-size is still in pre-release. That's because there are caveats to its widespread adoption. We've made it an opt-in feature for now, but you can still submit apps to the store that use this flag.

**Instructions.** Within Visual Studio, right-click on your UWP project and *unload project*. Right-click once again and *Edit the .vbproj/.csproj*. Within this prof file, look for all three occurrences of `<UseDotNetNativeToolchain>` and add a new directive under them as follows. By default, the three places that use the .NET Native toolchain are Release|x86, Release|x64 and Release|Arm.
```xml
<UseDotNetNativeToolchain>true</UseDotNetNativeToolchain>
<UseDotNetNativeSharedAssemblyFrameworkPackage>true</UseDotNetNativeSharedAssemblyFrameworkPackage>
```

Next, right-click on your project once again, *Reload*, and rebuild. 

(Tip: you can also avoid the need to unload+reload by editing the proj file outside VS, for instance in Notepad. VS will automatically detect when it needs to reload the project.)

**What this flag does.** This configuration flag tells your .NET Native release builds to *not* include the bulk of .NET app-locally. That's how it delivers reduced app-size. Instead .NET is delivered through a *"Shared AppX Framework Package"*. That means:

1. When customers download your app from the Store, this download doesn't contain the bulk of .NET
2. Instead, the right version of .NET gets automatically downloaded on-demand from the Store, and is shared between all apps that use it.

You can see how it's accomplished inside your app's `bin\Release\x86\ilc\AppxManifest.xml` file. It has a line like this, which is what the Store app obeys when installing your app:
```
<PackageDependency
    Name="Microsoft.NET.Native.Framework.1.2"
    MinVersion="1.2.23231.0" />
```

**Caveats.** We hope to make "shared framework" the default experience soon, but for now in the Win10 Tools 1.1 release, the feature is opt-in...

- This is the first public release of the feature, and we want to give it some time and testing before switching everyone over.
- For most apps, the shared framework flag causes faster build times for Release builds. Some apps build 35% faster, some build 20% slower, but the typical change is ~20% faster. We are working on making the flag deliver a more consistent improvement to build times. 
- When you upgrade to VS2015 Update 1, if your project is using this flag, then you might need to upgrade your project in some way in order to open it. We haven't closed on this.
- Any apps you submit to the store right now, using this flag, will continue to work.


**Guidance.** Here are our recommendations for how to use this flag for now:

- **During development:** turn flag on if you observe that doing so improves build times for your particular app.
- **For store submission:** leave flag off, to reduce risk of churn.
- **For size-critical store submission:** turn flag on only if you concretely need to keep the size of your app small, e.g. to keep it under 50mb so it's easier and faster to download over cellular.
- **For sample code:** leave flag off, so people in the future have an easier time building your sample.
- **If you're using out-of-band updates to .NET:** leave the flag off. Read the next section for an explanation.


How to take advantage of fixes in .NET
========================================

We and the community make ongoing improvements to .NET. How do you as a developer take advantage of those fixes in your apps?

Let's motivate this with a specific example. Suppose you're writing a UWP app using VS2015 RTM and you write this code:
```cs
Expression<Func<int>> e = () => 1;
e = e.Update(Expression.Constant(2), null);
int i = e.Compile().Invoke(); // should return "2"
```

Don't worry if this code is unfamiliar -- it's a small corner of the .NET framework using [expression trees](https://msdn.microsoft.com/en-us/library/vstudio/bb397951.aspx) that's not commonly used. But for those people who do use it, they need it, and were blocked when they discovered a .NET bug which prevents the call to *Expression.Update* from working.

In the past if you found a bug in .NET, you were basically stuck -- you'd have to wait for the next public release of .NET, and even then you couldn't depend on all your users installing that update onto their machines.

But now with UWP things are different. This fix was identified in the public open-source version of .NET on github, and was fixed with github changeset [#2361](https://github.com/dotnet/corefx/commit/ebac6f555fc1f89da91f09c4f9a11bc123c99889), and is available in a new publically-available beta of the NuGet package [System.Linq.Expressions](https://www.nuget.org/packages/System.Linq.Expressions/4.0.11-beta-23225). To use this beta in your UWP app, do *Manage Nuget References*, turn on the *Include prerelease* checkbox, and pick up the latest prerelease version of the package. Most people won't need any of this. But those who do, who were blocked by this particular API, they now have a way forward.

![Manage NuGet References](manage-nuget.png)
 
What's important with UWP is that *you don't have to depend on your users installing .NET updates onto their machines*. That's because, when they download your app from the store, your app includes app-locally all the .NET APIs that it actually uses.


(Where can you find a list of all these pre-release packages and the fixes are inside them? Well, they're all still in beta. Most developers won't need to live at the "bleeding edge" and can simply wait until we gather up the changes and publish them, likely in the next VS Update. Those developers who do need these immediate fixes should follow them on the [corefx github](https://github.com/dotnet/corefx/commit/ebac6f555fc1f89da91f09c4f9a11bc123c99889).)
 
![CoreFX on github](corefx-github.png)


You can't combine out-of-band .NET fixes with Shared Framework
================================================================

This blog post has described two technologies:

- How to get the latest out-of-band fixes to .NET, so your app compiles with a slightly different version of the .NET framework from what everyone else does;
- How to use a shared version of the .NET framework delivered on-demand, the same version as other apps do, in order to reduce build-time and app size.

Obviously the two are incompatible! This is what you'll see if you try to combine a pre-release version of System.Linq.Expressions and the Shared Framework flag:
 
![Incomptabitble shared assembly](incompatible-sharedfx.png)
*"ILC1308: SharedAssembly is not applicable to you project configuration. Project dependencies don't match the assemblies that the shared assembly is built against. You may need to adjust your project dependencies to ensure they match the versions that the shared assembly is built against."*


Conclusions
=============

.NET Native is an important technology for the future of .NET. In this release we've improved it significantly.

We are eager for people to try the new feature. Please turn the flag on during development. Many of you will observe faster build-times. What we're hoping to get are reports from you of any problems you encounter -- either by email to [dotnetnative@microsoft.com](mailto:dotnetnative@microsoft.com) or via the *send-a-smile/frown* icon at the top of the Visual Studio 2015 titlebar (below). Thank you!
 
![Send-a-smile](send-a-smile.png)
