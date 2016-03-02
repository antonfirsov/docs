Introduction to .NET Framework Compatibility
============================================

This post was written by **Mike Rousos**, a software engineer on the .NET team.

Introduction
------------

Beginning with the .NET Framework 4.0, all versions of the .NET Framework with a major version number of 4 (called ‘4.x’ versions) install as in-place updates. This means that only one 4.x .NET Framework is installed on a computer at a time. Installing the .NET Framework 4.5 will replace version 4.0, the .NET Framework 4.5.1 will replace version 4.5, the .NET Framework 4.6 will replace version 4.5.1, and so on.

Because of the in-place nature of these updates, applications that originally ran on the .NET Framework 4.0, for example, may need to run on 4.6 after the .NET Framework installed on the computer is upgraded. The .NET Framework 4.x releases are highly-compatible with one another, so an app which works on one 4.x .NET Framework will usually work on newer versions of the .NET Framework. However, there are some changes between 4.x versions of the .NET Framework, so apps should be tested on any versions of the .NET Framework they are expected to run on.

This article gives an overview of best practices and tools to make supporting a new .NET Framework version easier.

What Has Changed and Why?
-------------------------

Compatibility with previous versions of the .NET Framework is a high priority for the .NET team. In fact, all changes in the .NET Framework are reviewed by experienced engineers who assess the impact of the changes on customer apps.

Despite that, compatibility issues still exist. One reason for this is that compatibility (important as it is) is not the only priority in updating the .NET Framework. Sometimes functionality has to change to address a security hole, or to support an industry standard.

Other compatibility issues occur unintentionally. The .NET Framework team conducts thorough compatibility testing to guard against these sorts of issues, but some bugs do slip through. Even more complicated, fixing a compatibility issue that was previously introduced as a bug is, itself, a compatibility-affecting change (since some users may be depending on the unintentional newer behavior)! In these situations (addressing unintentional behavioral changes), the .NET Framework team will often use a solution known as ‘quirking’.

### Quirking and Targeting ###
Quirking refers to the compatibility issue mitigation of having two separate code paths in the .NET Framework and choosing which path to take based on the .NET Framework version that the application is targeting. Because many .NET Framework compatibility issues are mitigated in this way, it’s possible to avoid some potential issues when running on newer .NET Framework versions by leaving the targeted .NET Framework version unchanged for an app. Quirking behavior is automatically determined based on the .NET Framework the app targets, but can be overridden by developers using application or machine configuration settings. Although many compatibility issues are mitigated through quirks, due to security considerations and technical limitations, not all compatibility issues can be quirked.

As an example, if an app targets the .NET Framework 4.5 but runs on a computer with the .NET Framework 4.5.2 installed, even though the app executes on the newer Framework, it will mimic some behaviors from 4.5 in order to minimize compatibility issues.

**With the .NET Framework 4.0, 4.5, and 4.5.1 now [out of support](https://support.microsoft.com/en-us/gp/framework_faq), it is worth noting that targeting these Frameworks while running on a newer .NET Framework version will continue to be supported as per the newer Framework’s support policy.**

The target version is determined by consulting the [TargetFramework attribute](https://msdn.microsoft.com/en-us/library/system.runtime.versioning.targetframeworkattribute%28v=vs.110%29.aspx) of the app’s main assembly at the time the application domain is created. This attribute can be set several different ways:

* The target framework for a project can be [specified in Visual Studio](https://msdn.microsoft.com/en-us/library/bb398202%28v=vs.140%29.aspx).
* The target framework for a project can be [specified directly in the project file](https://msdn.microsoft.com/en-us/library/hh264221.aspx).
* The target framework can be specified by [directly applying a TargetFramework attribute](https://msdn.microsoft.com/en-us/library/system.runtime.versioning.targetframeworkattribute%28v=vs.110%29.aspx) in the project’s source code.
  * Note that MSBuild automatically adds a TargetFramework attribute based on the project’s target framework moniker, so this attribute should only be applied directly in non-MSBuild scenarios. If MSBuild is used, adjust the target framework by using the project file settings linked previously.

For executables (such as WPF or console apps), the exe’s target Framework is used for quirking decisions.Libraries (dll’s) do not have control of the target Framework.

### Compatibility Switches ###
In addition to automatic quirking based on target .NET Framework, developers can enable or disable individual compatibility quirks (as well as some behaviors which are not automatically quirked) by setting compatibility switches to explicitly opt in or out of compatibility-affecting changes. These ‘compatibility switches’ can be useful for allowing a developer to target a newer .NET Framework version (in order to use new .NET functionality) while still opting out of some changes that are known to affect the app. Compatibility switches can be set in a variety of ways:

* Through configuration file settings
* Through environment variables
* Programmatically in the project’s source code

Although the details of how to set compatibility switches are out of scope for this introductory blog, look for a future follow-up that digs into the topic in more detail.

Compatibility issue documentation in [MSDN](https://msdn.microsoft.com/en-US/library/dn458358%28v=vs.110%29.aspx) will often mention compatibility switches, when available.

### Compatibility Issue Documentation ###
All of the known .NET Framework compatibility issues are [documented on MSDN](https://msdn.microsoft.com/en-us/library/dn458358%28v=vs.110%29.aspx).

Beginning with the .NET Framework 4.5.1, compatibility issues are categorized as either ‘runtime changes’ or ‘retargeting changes’.
 
* **Runtime changes** are those that affect any apps running on the newer .NET Framework version (they are not quirked). 
* **Retargeting changes**, on the other hand, are changes that only affect apps rebuilt to target the newer Framework. These are either quirked .NET Framework changes or changes in compilation tools. For changes between 4.0 and 4.5, the runtime/retargeting distinction is not highlighted as its own column in the tables of compatibility issues, but can be inferred from reading the descriptions.

In addition to the MSDN documentation, .NET Framework compatibility issues are available [as markdown files](https://github.com/Microsoft/dotnet-apiport/tree/master/docs/BreakingChanges) for consumption by compatibility tooling (discussed below). The markdown files can be read directly (or in [this](https://msdn.microsoft.com/en-US/library/mt661864%28v=vs.110%29.aspx) MSDN mirror of the list) to learn about compatibility issues. The markdown files are part of an open-source GitHub repository, so please submit pull requests or create issues for any corrections. The .NET team works to keep the information in the markdown files synchronized with the information available on MSDN.

### Compiler Compatibility Issues ###
In addition to the .NET Framework runtime and retargeting compatibility issues described above, there are also small sets of changes between versions of the C# and Visual Basic compilers that cannot be quirked, but also do not occur at runtime. For example, developers must be intentional when rebuilding apps with newer compilers, because of rare differences between how the C# 4.0 compiler generates IL and how the C# 5.0 compiler generates IL.

Because these compatibility issues only manifest themselves when re-building with a newer compiler, they won’t affect old, previously compiled binaries running on a new version of the .NET Framework. For that reason, MSDN documentation classifies these as retargeting changes. Compatibility issue markdown files will label compiler compatibility issues as a class of retargeting changes that are ‘build-time,’ which is different from a ‘quirked’ retargeting change.

Tools for Identifying Issues
----------------------------
The primary purpose of the compatibility issue markdown files published on GitHub is for consumption by compatibility tools. These tools ease the migration from one .NET Framework version to another. Today, there are two toolsets for compatibility analysis.

### API Portability Analyzer ###
[ApiPort](https://github.com/Microsoft/dotnet-apiport) (as the tool is called, for short) is a tool that scans a binary and identifies all .NET Framework APIs used. Then, it compares those APIs to the data stored in the compatibility issue markdown files and provides a report on any APIs that are used which have changed between one .NET Framework 4.x version and another. Command line options can narrow the scan (for example, by only considering changes between specified .NET Framework versions). For full documentation, please see [ApiPort’s breaking change scanning usage instructions](https://github.com/Microsoft/dotnet-apiport/blob/master/docs/HowTo/BreakingChanges.md). A couple caveats to bear in mind when working with ApiPort are:

1. Because ApiPort is only looking at which .NET APIs are called, it will report some ‘false positives’. Most .NET compatibility issues only affect very specific code paths of a given API. Simply using one of these APIs does not mean that an app will be affected by a compatibility issue. Read through the change descriptions to determine whether the reported issues are likely to actually manifest in your particular app.
2. Because ApiPort is only looking at which .NET APIs are called, some compatibility issues cannot be detected by the tool. For example, using a changed XAML control in a WPF app may not be visible when only scanning IL. ApiPort is a useful tool, but is not a substitute for compatibility testing.

### .NET Framework Compatibility Analyzers ###
The .NET Framework Compatibility analyzers are a set of [Roslyn diagnostic code analyzers](https://msdn.microsoft.com/en-us/magazine/dn879356.aspx) which use syntax trees and semantic models from source code to more intelligently decide whether a project is likely to encounter compatibility issues or not. They will still report some false positives, but should be more accurate than ApiPort. 

These are available on [NuGet.org](https://www.nuget.org/) as [Microsoft.DotNet.FrameworkCompatibilityDiagnostics](https://www.nuget.org/packages/Microsoft.DotNet.FrameworkCompatibilityDiagnostics/). The .NET Framework team is currently working to open-source these analyzers. Please watch this blog for more updates on the open-sourcing effort.

Reporting New Compatibility Issues
----------------------------------

While migrating applications from one .NET Framework version to another, you may occasionally encounter a compatibility issue that isn’t documented in MSDN or in the ApiPort markdown files. If this should happen, please let us know! The .NET team is continually working to keep compatibility documentation up-to-date.

Should you encounter an undocumented compatibility issue in the .NET Framework, please report it in one of these ways:

* Use Visual Studio’s “Send-a-Smile” feedback feature to send details on the change.
* Create an [issue](https://github.com/microsoft/dotnet-apiport/issues) in the ApiPort repository that there is a .NET Framework compatibility issue not recorded in the tool’s markdown files. The .NET team (or community members) will be able to investigate and add documentation and support, as appropriate.

Conclusion
----------

The .NET Framework strives to be highly-compatible with each new Framework release. Despite that, some compatibility issues are inevitable. Knowing about these changes, and knowing how to mitigate them, can help keep your applications running successfully on new versions of the .NET Framework.

Some compatibility best practices covered in this article include:

* Don’t re-target existing projects to newer .NET Framework versions unless necessary. This will minimize compatibility issues by allowing the app to take advantage of compatibility quirks.
* If you have any control over which .NET Framework versions are used to run your app, use newer .NET Framework versions instead of older ones. This is because many compatibility issues in 4.x versions of the .NET Framework have been fixed in subsequent versions. For example, there are fewer compatibility issues moving between 4.0 and 4.6 than between 4.0 and 4.5.
* Make sure to test thoroughly if an app is re-built using newer compilers. This could expose it to compiler compatibility issues (though these are rare).
* Use compatibility tools like the [API Portability Analyzer](https://github.com/Microsoft/dotnet-apiport) and [.NET Framework Compatibility Analyzers](https://www.nuget.org/packages/Microsoft.DotNet.FrameworkCompatibilityDiagnostics/) to identify potential problem areas.
* Test your app on any .NET Framework version it is expected to run on.

Using these techniques, apps should continue to function on new versions of the .NET Framework.

Resources
---------

* [MSDN .NET Migration Guide](https://msdn.microsoft.com/en-us/library/ff657133%28v=vs.110%29.aspx)
  * [MSDN Compatibility Issue Lists](https://msdn.microsoft.com/en-us/library/dn458358%28v=vs.110%29.aspx)
* [.NET API Portability Analyzer](https://github.com/microsoft/dotnet-apiport)
* [.NET Framework Compatibility Analyzers](https://www.nuget.org/packages/Microsoft.DotNet.FrameworkCompatibilityDiagnostics/)
* [.NET Framework Support Lifecycles](https://support.microsoft.com/en-us/lifecycle/search?sort=pn&alpha=.net%20framework)
