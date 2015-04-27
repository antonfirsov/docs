.NET Framework 4.6
==================

Cryptography Updates
--------------------

The team is updating the [System.Security.Cryptography APIs](https://msdn.microsoft.com/library/system.security.cryptography.aspx) to support the [Windows CNG cryptography APIs](https://msdn.microsoft.com/library/windows/desktop/aa376214.aspx). To date, the .NET Framework has use an earlier version of [Windows Cryptography APIs](https://msdn.microsoft.com/library/windows/desktop/aa380255.aspx) as the basis of the System.Security.Cryptography implementation. We have had requests to support the CNG API, since it supports [modern cryptography algorithms](https://msdn.microsoft.com/library/windows/desktop/bb204775.aspx#suite_b_support), which are important for certain categories of apps. In this update, the team has added support to use CNG certificate keys with the [X509Certificate class](https://msdn.microsoft.com/library/system.security.cryptography.x509certificates.x509certificate.aspx). See the code below that demonstrates the new capability.
 
            Code here.
 
This update is the first step towards broader support for the Windows CNG API and for more modern cryptography algorithms generally. Note that team is still in the middle of building this new support, so expect the API to change for RTM.

Compatibility Switches
----------------------

AppContext is a new compatibility feature that allows you to define and/or check for the existence of switches and to make decisions based on them. Libraries define and expose switches, while code that depends on them can set those switches, to affect the library behavior. This same infrastructure is used by the .NET Framework internally, to enable developers to opt in or out of certain behaviors.

An application (or a library) can declare the value (always boolean) of a switch that a dependent library defines:

	AppContext.SetSwitch(“Switch.MyLib.ThrowOnException”, true)

The library must check if a consumer has delcared the value of the switch and then appropraitely act on it.

	bool shouldThrow;

	if (!AppContext.TryGetSwitch(“MyLib.ThrowOnException”, out shouldThrow))
	{
	   	// This is the case where the switch value was not set by the application. 
	   	// The library can choose to get the value of shouldThrow by other means. 
		// The value for shouldThrow in this case would be ‘false’
	}
	
	// The library can use the value of shouldThrow to throw exceptions or not.
	if (shouldThrow)
	{
	 	// Library logic
	}

It's beneficial to use a consistent format for switches, since they are a formal contract exposed by libraries. The following are two obvious formats. The first one is used by the .NET Team, but both are good. 

- Switch.namespace.switchname
- Switch.library.switchname


