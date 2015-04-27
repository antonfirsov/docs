.NET Framework 4.6
==================

Cryptography Updates
--------------------

The team is updating the [System.Security.Cryptography APIs](https://msdn.microsoft.com/library/system.security.cryptography.aspx) to support the [Windows CNG cryptography APIs](https://msdn.microsoft.com/library/windows/desktop/aa376214.aspx). To date, the .NET Framework has use an earlier version of [Windows Cryptography APIs](https://msdn.microsoft.com/library/windows/desktop/aa380255.aspx) as the basis of the System.Security.Cryptography implementation. We have had requests to support the CNG API, since it supports [modern cryptography algorithms](https://msdn.microsoft.com/library/windows/desktop/bb204775.aspx#suite_b_support), which are important for certain categories of apps. In this update, the team has added support to use CNG certificate keys with the [X509Certificate class](https://msdn.microsoft.com/library/system.security.cryptography.x509certificates.x509certificate.aspx). See the code below that demonstrates the new capability.
 
            Code here.
 
This update is the first step towards broader support for the Windows CNG API and for more modern cryptography algorithms generally. Note that team is still in the middle of building this new support, so expect the API to change for RTM.
