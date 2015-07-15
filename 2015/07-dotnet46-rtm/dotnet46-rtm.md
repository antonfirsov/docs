Announcing .NET Framework 4.6 RTM
=================================

We're excited to announce [.NET Framework 4.6](http://go.microsoft.com/fwlink/?LinkId=528259) and [Visual Studio 2015](http://go.microsoft.com/fwlink/?LinkId=517106), both as RTM releases, today. You can read about the new features or install these new versions to try them out. The quickest way to get started is to install the free Visual Studio 2015 Community version.

With the .NET Framework 4.6, you'll enjoy better performance with the new 64-bit "RyuJIT" JIT and high DPI support for WPF and Windows Forms. ASP.NET provides HTTP/2 support when running on Windows 10 and has more async task-returning APIs. There are also major updates in Visual Studio 2015 for .NET developers, many of which are built on top of the new Roslyn compiler framework. The .NET languages -- C# 6, F# 4, VB 15 -- get an update today, too. 

We're also announcing updates to .NET Core and ASP.NET 5, both releasing as beta 6 today. .NET Native??

You can download and try out the releases now:

- [Visual Studio 2015](http://go.microsoft.com/fwlink/?LinkId=517106)
- [.NET Framework 4.6](http://go.microsoft.com/fwlink/?LinkId=528259)

As a team, we're really excited to share everything we've been working on:

- .NET Framework 4.6
- Visual Studio Improvements for .NET Developers
- ASP.NET
- .NET Core - for device and cloud
- .NET Universal Windows Apps (including .NET Native)

You can check out the earlier [RC](http://blogs.msdn.com/b/dotnet/archive/2015/04/29/net-announcements-at-build-2015.aspx) and [Preview](http://blogs.msdn.com/b/dotnet/archive/2014/11/12/announcing-net-2015-preview-a-new-era-for-net.aspx) releases to see how the release has developed over the last year. In fact, it's only been 14 months since we released the [.NET Framework 4.5.2](http://blogs.msdn.com/b/dotnet/archive/2014/05/05/announcing-the-net-framework-4-5-2-release.aspx). 

.NET Framework 4.6
==================

There are many great features in the [.NET Framework 4.6](http://go.microsoft.com/fwlink/?LinkId=528259). Some of these features, like RyuJIT and the latest GC updates, can provide improvements by just installing the .NET Framework 4.6. Give it at try!

RyuJIT
------

RyuJIT is the next generation Just-In-Time (JIT) compiler for .NET. It uses a high-performance JIT architecture, focussed on high throughput JIT compilation. It is much faster than the existing _JIT64_ 64-bit JIT that has been used for the last 10 years (introduced in 2005 .NET 2.0 release). There was always a big gap in throughput between the 32- and 64-bit JITs. That gap has been closed, making it easier to exclusively target 64-bit architectures or migrate workloads from 32- to 64-bit.

RyuJIT is enabled for 64-bit processes running on top of the .NET Framework 4.6. Your app will run in a 64-bit process if it is compiled as 64-bit or AnyCPU, and run on a 64-bit operating system. RyuJIT is similarly integrated into .NET Core, as the 64-bit JIT.

We've used a transparent process over the last two years with RyuJIT. You've been able to read [RyuJIT blog posts](http://blogs.msdn.com/b/dotnet/archive/tags/ryujit/), try out several RyuJIT CTPs and (suprise!) can now even read and contribute to the [RyuJIT source code](https://github.com/dotnet/coreclr/tree/master/src/jit). Thanks to everyone who helped improve RyuJIT along the way to RTM. We fixed a lot of publicly-reported bugs and performance issues based on those CTP releases. It's been a pleasure for Microsoft engineers to adopt a more public development process with RyuJIT.

The project was initially targeted to improve high-scale 64-bit cloud workloads, although it has much broader applicability. We do expect to add 32-bit support in a later release.

Garbage Collector Updates
-------------------------

The garbage collector has a few important improvements that reduce latency and improve memory utilization. The GC update have already been deployed to large Microsoft cloud workloads, such as Office 365 and Bing. We've seen impressive improvements in the performance of those services. They've simply deployed the update, without any code changes.

The GC now handles pinned objects in a more optimized way. It is now possible for the GC to compact more memory around pinned objects. This change can provide a suprisingly impactful improvement for large-scale workloads with significant use of pinning.

Promotion of generation 1 objects to generation 2 has been updated to use memory more efficiently. The GC attempts to use free space in a given generation before allocating a new memory segment. A new algorithm has been adopted that uses a free space region to allocate an object that more closely matches the object size.

The Garbage Collector has a new mode that avoids garbage collection while certain memory-related conditions are met. This new mode is important for low-latency workloads that cannot afford interuptions.

The new mode enables you to [specify a certain amount of memory be available](https://msdn.microsoft.com/library/system.gc.trystartnogcregion.aspx) as a pre-requisite to enter a _No GC Region_. While in the region the GC will not collect. It will start collecting if a collection is explicitly requested (e.g. [GC.Collect](https://msdn.microsoft.com/library/system.gc.collect.aspx)) or if the initially specified memory size is exhausted.

The new mode exposes multiple points of configuration, including allowing you to specify the memory available for the small and large object heaps separately, for use within the No GC region. 

Cryptography Updates
--------------------

The team is updating the [System.Security.Cryptography APIs](https://msdn.microsoft.com/library/system.security.cryptography.aspx) to support the [Windows CNG cryptography APIs](https://msdn.microsoft.com/library/windows/desktop/aa376214.aspx). To date, the .NET Framework has use an earlier version of [Windows Cryptography APIs](https://msdn.microsoft.com/library/windows/desktop/aa380255.aspx) as the basis of the System.Security.Cryptography implementation. We have had requests to support the CNG API, since it supports [modern cryptography algorithms](https://msdn.microsoft.com/library/windows/desktop/bb204775.aspx#suite_b_support), which are important for certain categories of apps. In this update, the team has added support to use CNG certificate keys with the [X509Certificate class](https://msdn.microsoft.com/library/system.security.cryptography.x509certificates.x509certificate.aspx).
  
This update is the first step towards broader support for the Windows CNG API and for more modern cryptography algorithms generally. Note that team is still in the middle of building this new support, so expect the API to change for RTM.

Compatibility Switches
----------------------

AppContext is a new compatibility feature that enables library writers to provide a uniform opt-out mechanism for new functionality for their users. It established a loosley-coupled contract between components in order to communicate an opt-out request. This capability is typically important when a change is made to existing functionality. Conversely, there is already an implicit opt-in for new functionality.

With AppContext, libraries define and expose compatibility switches, while code that depends on them can set those switches, to affect the library behavior. By default libraries provide the new functionality and only alter it (e.g. provide the old behavior) if the switch is set.

An application (or a library) can declare the value (always boolean) of a switch that a dependent library defines. The switch is always implicity `false`. Setting the switch to `true` enables the switch. Explicity the switch to `false` provides the new behavior.

	AppContext.SetSwitch("Switch.AmazingLib.ThrowOnException”, true)

The library must check if a consumer has declared the value of the switch and then appropraitely act on it.

<script src="https://gist.github.com/richlander/7ab29fe9ffa71eabe9be.js"></script>

It's beneficial to use a consistent format for switches, since they are a formal contract exposed by libraries. The following are two obvious formats.  

- Switch.namespace.switchname
- Switch.library.switchname

This same infrastructure is used by the .NET Framework internally, to enable developers to opt out of updates to existing functionality.

WPF Improvements
----------------

The team has made key improvements to WPF in this release:

- Transparent child windows
- Multi-image cursor files
- Re-designed Blend experience
- New set of Visual Diagnostics tools
- Timeline tool in the Performance and Diagnostics hub


### HDPI Improvements

HDPI support in WPF is now better in the .NET Framework 4.6. Changes have been made to Layout rounding to reduce instances of clipping in controls with borders. By default, this feature is enabled if your Target Framework is .NET Framework 4.6 (".NETFramework,Version=v4.6") or higher. Applications that target earlier versions of the framework can opt in into the new behavior by adding the following line to the <runtime> section of the app.config file. The setting only takes effect when the application is running on the .NET Framework 4.6.

	<runtime>
		<AppContextSwitchOverrides value="Switch.MS.Internal.DoNotApplyLayoutRoundingToMarginsAndBorderThickness=false" />
	</runtime>

WPF windows straddling multiple monitors with different DPI settings (Multi-DPI setup) are now rendered without blacked out regions. You can opt out of this behavior by adding the following line to the <appSettings> section to disable this new Behavior:

	<appSettings>
		<add key="EnableMultiMonitorDisplayClipping" value="true"/>
	</appSettings>

Support for automatically loading the right cursor based on DPI setting has been added to System.Windows.Input.Cursor.

### Touch is better

The double tap threshold for Windows Store applications and WPF applications are now the same in Windows 8.1 and above. This 

https://connect.microsoft.com/VisualStudio/feedback/details/903760/wpf-touch-services-are-badly-broken

### Transparent Child Window support

WPF in .NET 4.6 supports transparent child windows in Windows 8.1 and above. You can enable this by setting the UsesPerPixelTransparency property to true in HwndSourceParameters



Windows Forms Updates for High DPI
----------------------------------

Windows Forms High DPI support has been updated to include more controls. The [.NET Framework 4.5.2(http://blogs.msdn.com/b/dotnet/archive/2014/05/05/announcing-the-net-framework-4-5-2-release.aspx) included high DPI support for an initial set of Windows Forms controls.

The following controls have High DPI support in the .NET Framework 4.6: ComboBox, Cursor, DataGridView, DataGridViewColumn, DataGridViewComboBoxColumn, DomainUpDown, NumericUpDown, ToolStripComboBox, ToolStripMenuItem and ToolStripSplitButton.

This is an opt-in feature. To enable it, set the EnableWindowsFormsHighDpiAutoResizing element to true in the application configuration (app.config) file:

	<appSettings>
		<add key="EnableWindowsFormsHighDpiAutoResizing" value="true" />
	</appSettings>