# .NET Framework April 2017 Monthly Rollup

Today, we are releasing a new Security and Quality Rollup and Security Only Update for the .NET Framework.

## Security

[Microsoft Common Vulnerabilities and Exposures CVE17-0160](https://portal.msrc.microsoft.com/security-guidance/advisory/CVE-2017-0160)

A remote code execution vulnerability exists when the Microsoft .NET Framework fails to properly validate input before loading libraries. An attacker who successfully exploited this vulnerability could take control of an affected system. An attacker could then install programs; view, change, or delete data; or create new accounts with full user rights. Users whose accounts are configured to have fewer user rights on the system could be less impacted than users who operate with administrative user rights. To exploit the vulnerability, an attacker would first need to access the local system with the ability to execute a malicious application.  The security update addresses the vulnerability by correcting how .NET validates input on library load.

## Quality and Reliability

There are no quality and reliability changes this month.

## Getting the Update

The Security and Quality Rollup is available via Windows Update, Windows Server Update Services and Microsoft Update Catalog. The Security Only Update is available via Windows Server Update Services and Microsoft Update Catalog. The [Windows 10 updates](https://support.microsoft.com/help/4018124) are integrated with the Windows 10 Monthly Update.

You can learn more about the releases from the table below.

|Windows Version |.NET Version|Rollup KB |Security-only KB |
|:---------------|:-----------|:---------|:----------------|
|Windows 10 Creators Update|.NET Framework 3.5 and 4.7|[4018483](https://support.microsoft.com/kb/4018483) | N/A |
|Windows 10 Anniversary Update and Windows Server 2016|.NET Framework 3.5 and 4.6.2|[4015217](https://support.microsoft.com/kb/4015217) | N/A |
|Windows 10 1511 Update|.NET Framework 3.5 and 4.6.1|[4015219](https://support.microsoft.com/kb/4015219) | N/A |
|Windows 10 RTM|.NET Framework 3.5 and 4.6|[4015221](https://support.microsoft.com/kb/4015221) | N/A |
|Windows 8.1 and Windows Server 2012 R2|.NET Framework 3.5, 4.5.2, 4.6, 4.6.1, and 4.6.2|[4014983](https://support.microsoft.com/kb/4014983) | [4014987](https://support.microsoft.com/kb/4014987) |
|Windows Server 2012|.NET Framework 3.5, 4.5.2, 4.6, 4.6.1, and 4.6.2|[4014982](https://support.microsoft.com/kb/4014982) | [4014986](https://support.microsoft.com/kb/4014986) |
|Windows 7 and Windows Server 2008 R2|.NET Framework 3.5, 4.5.2, 4.6, 4.6.1, and 4.6.2|[4014981](https://support.microsoft.com/kb/4014981) | [4014985](https://support.microsoft.com/kb/4014985) |
|Windows Vista SP2 and Windows Server 2008 SP2|.NET Framework 3.5, 4.5.2, and 4.6|[4014984](https://support.microsoft.com/kb/4014984) | [4014988](https://support.microsoft.com/kb/4014988) | 

### Docker Images

The [Windows ServerCore](https://hub.docker.com/r/microsoft/windowsservercore/) and [.NET Framework](https://hub.docker.com/r/microsoft/dotnet-framework/) Docker images will be updated today. Pulling the latest image will update your local Docker image cache.

### Previous Monthly Rollups

The last couple .NET Framework Rollup updates are listed below for your convenience:

- [December 2016](https://blogs.msdn.microsoft.com/dotnet/2016/12/13/net-framework-december-monthly-rollup-is-now-available/)
- [October 2016](https://blogs.msdn.microsoft.com/dotnet/2016/10/11/net-framework-monthly-rollup-october-2016/)

Note: Previously released security and quality updates are included in today's release.

### More Information

You can read the [.NET Framework Monthly Rollups Explained](https://blogs.msdn.microsoft.com/dotnet/2016/10/11/net-framework-monthly-rollups-explained/) to learn more about how the .NET Framework is updated.
