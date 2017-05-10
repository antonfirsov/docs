# .NET Framework May 2017 Monthly Rollup

Today, we are releasing a new Security and Quality Rollup and Security Only Update for the .NET Framework.

Please see [.NET Core May 2017 Updates](https://blogs.msdn.microsoft.com/dotnet/) for the .NET Core updates being released today.

## Security

**Microsoft Common Vulnerabilities and Exposures CVE-2017-0248**

A security feature bypass vulnerability exists when Microsoft .NET Framework (and .NET Core) components do not completely validate certificates.

An attacker could present a certificate that is marked invalid for a specific use, but the component uses it for that purpose. This action disregards the Enhanced Key Usage extensions. 

The security update addresses the vulnerability by helping to ensure that .NET Framework (and .NET Core) components completely validate certificates.

To learn more about this vulnerability, see [Microsoft Common Vulnerabilities and Exposures CVE-2017-0248](https://portal.msrc.microsoft.com/security-guidance/advisory/CVE-2017-0248).

This update also contains security-enhancing fixes to the Windows Presentation Framework PackageDigitalSignatureManager component's ability to sign packages with the SHA256 hash algorithm.

## Quality and Reliability

There are no quality and reliability changes this month.

## Getting the Update

The Security and Quality Rollup is available via Windows Update, Windows Server Update Services and Microsoft Update Catalog. The Security Only Update is available via Windows Server Update Services and Microsoft Update Catalog. The [Windows 10 updates](https://support.microsoft.com/help/4018124) are integrated with the Windows 10 Monthly Update.

You can learn more about the releases from the table below.

|Windows Version |.NET Version|Rollup KB |Security-only KB |
|:---------------|:-----------|:---------|:----------------|
|Windows 10 Creators Update|.NET Framework 3.5 and 4.7|[4016871](https://support.microsoft.com/kb/4016871) | N/A |
|Windows 10 Anniversary Update <br> Windows Server 2016|.NET Framework 3.5, 4.6.2 and 4.7|[4019472](https://support.microsoft.com/kb/4019472) | N/A |
|Windows 10 1511 |.NET Framework 3.5 and 4.6.1|[4019473](https://support.microsoft.com/kb/4019473) | N/A |
|Windows 10 1507|.NET Framework 3.5 and 4.6|[4019474](https://support.microsoft.com/kb/4019474) | N/A |
|Windows 8.1 <br> Windows Server 2012 R2|.NET Framework 3.5, 4.5.2, 4.6, 4.6.1, 4.6.2 and 4.7|[4019114](https://support.microsoft.com/kb/4019114) | [4019111](https://support.microsoft.com/kb/4019111) |
|Windows Server 2012|.NET Framework 3.5, 4.5.2, 4.6, 4.6.1, 4.6.2 and 4.7|[4019113](https://support.microsoft.com/kb/4019113) | [4019110](https://support.microsoft.com/kb/4019110) |
|Windows 7 <br> Windows Server 2008 R2|.NET Framework 3.5, 4.5.2, 4.6, 4.6.1, 4.6.2 and 4.7|[4019112](https://support.microsoft.com/kb/4019112) | [4019108](https://support.microsoft.com/kb/4019108) |
|Windows Vista SP2 <br> Windows Server 2008 SP2 SP1|.NET Framework 3.5, 4.5.2, and 4.6|[4019115](https://support.microsoft.com/kb/4019115) | [4019109](https://support.microsoft.com/kb/4019109) | 

### Docker Images

The [Windows ServerCore](https://hub.docker.com/r/microsoft/windowsservercore/) and [.NET Framework](https://hub.docker.com/r/microsoft/dotnet-framework/) Docker images have also been updated. Pulling the latest image will update your local Docker image cache.

### Known Issue with the April 2017 Update

The [April 2017 Monthly Update](https://blogs.msdn.microsoft.com/dotnet/2017/04/11/net-framework-april-2017-monthly-rollup/) contained a bug that caused the [PowerShell Stop-Computer command to stop correctly functioning](https://support.microsoft.com/en-us/help/4020459/after-you-apply-april-2017-security-updates-from-cve-2017-0160). This bug has since been fixed. You can get the fix in the following ways:

**Using Windows 10**
- Install the May 2017 Update for Windows 10 (see link in the table above).

**Using an earlier version of Windows**
- Wait for the next .NET Framework monthly update, which will include this fix. This approach is recommended if you are not experiencing this problem.
- Install the specific fix for this issue, which you can find in the [April 2017 Monthly Update](https://blogs.msdn.microsoft.com/dotnet/2017/04/11/net-framework-april-2017-monthly-rollup/) post.

Note that the [.NET Framework 4.7](https://blogs.msdn.microsoft.com/dotnet/2017/05/02/announcing-the-net-framework-4-7-general-availability/) contains the fix. If you are using Windows 10 Creators Update, you will still need to install the May 2017 Update (see link in the table above) to get this fix.

### Previous Monthly Rollups

The last few .NET Framework Monthly updates are listed below for your convenience:

- [April 2017](https://blogs.msdn.microsoft.com/dotnet/2017/04/11/net-framework-april-2017-monthly-rollup/)
- [December 2016](https://blogs.msdn.microsoft.com/dotnet/2016/12/13/net-framework-december-monthly-rollup-is-now-available/)
- [October 2016](https://blogs.msdn.microsoft.com/dotnet/2016/10/11/net-framework-monthly-rollup-october-2016/)

Note: Previously released security and quality updates are included in today's release.

### More Information

You can read the [.NET Framework Monthly Rollups Explained](https://blogs.msdn.microsoft.com/dotnet/2016/10/11/net-framework-monthly-rollups-explained/) to learn more about how the .NET Framework is updated.
