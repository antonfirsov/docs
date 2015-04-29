.NET Framework 4.6
==================

Garbage Collector Update
------------------------

The Garbage Collector has a new mode that attempts to avoid garbage collection while certain memory-related conditions are met. This new mode is important for workloads that require uninterupted computation (at least as it relates to GC CPU use).

The new mode enables you to [specify a certain amount of memory be available](https://msdn.microsoft.com/library/system.gc.trystartnogcregion.aspx) as a pre-requisite to enter a _No GC Region_. While in the region the GC will not collect. It will start collecting if a collection is  explicitly requested (e.g. [GC.Collect](https://msdn.microsoft.com/library/system.gc.collect.aspx)) or if the initially specified memory size is exhausted.

The new mode exposes multiple points of configuration, including allowing you to specify the memory available for the small and large object heaps separately, for use within the No GC Region. 

Cryptography Updates
--------------------

The team is updating the [System.Security.Cryptography APIs](https://msdn.microsoft.com/library/system.security.cryptography.aspx) to support the [Windows CNG cryptography APIs](https://msdn.microsoft.com/library/windows/desktop/aa376214.aspx). To date, the .NET Framework has use an earlier version of [Windows Cryptography APIs](https://msdn.microsoft.com/library/windows/desktop/aa380255.aspx) as the basis of the System.Security.Cryptography implementation. We have had requests to support the CNG API, since it supports [modern cryptography algorithms](https://msdn.microsoft.com/library/windows/desktop/bb204775.aspx#suite_b_support), which are important for certain categories of apps. In this update, the team has added support to use CNG certificate keys with the [X509Certificate class](https://msdn.microsoft.com/library/system.security.cryptography.x509certificates.x509certificate.aspx). See the code below that demonstrates the new capability.
  
This update is the first step towards broader support for the Windows CNG API and for more modern cryptography algorithms generally. Note that team is still in the middle of building this new support, so expect the API to change for RTM.

Compatibility Switches
----------------------

AppContext is a new compatibility feature that enables library writers to provide a uniform opt-out mechanism for new functionality for their users. It established a loosley-coupled contract between components in order to communicate an opt-out request. This capability is typically important when a change is made to existing functionality. Conversely, there is already an implicit opt-in for new functionality.

With AppContext, libraries define and expose compatibility switches, while code that depends on them can set those switches, to affect the library behavior. By default libraries provide the new functionality and only alter it (e.g. provide the old behavior) if the switch is set.

An application (or a library) can declare the value (always boolean) of a switch that a dependent library defines. The switch is always implicity `false`. Setting the switch to `true` enables the switch. Explicity the switch to `false` provides the new behavior.

	AppContext.SetSwitch("Switch.AmazingLib.ThrowOnException”, true)

The library must check if a consumer has declared the value of the switch and then appropraitely act on it.

	bool shouldThrow;

	if (!AppContext.TryGetSwitch(“Switch.AmazingLib.ThrowOnException”, out shouldThrow))
	{
	   	// The switch value was not set by the application. 
	   	// As a result, the value is 'false'. A false value implies the latest behavior.

	   	// The library can declare a default value for a switch based on a condition
	   	// Example: https://github.com/dotnet/coreclr/blob/master/src/mscorlib/src/System/AppContext/AppContextDefaultValues.Defaults.cs
	}
	
	// The library can use the value of shouldThrow to throw exceptions or not.
	if (shouldThrow)
	{
		// old code
	}
	else
	{
		//new code
	}

It's beneficial to use a consistent format for switches, since they are a formal contract exposed by libraries. The following are two obvious formats.  

- Switch.namespace.switchname
- Switch.library.switchname

This same infrastructure is used by the .NET Framework internally, to enable developers to opt out of updates to existing functionality.


