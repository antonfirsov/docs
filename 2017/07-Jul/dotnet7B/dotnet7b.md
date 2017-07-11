# .NET Framework July Security and Quality Rollup

Today, we are releasing a new Security and Quality Rollup and Security Only Update for the .NET Framework. Today's update applies to Windows 10 and Windows Server 2016.

## Security

**Microsoft Common Vulnerabilities and Exposures CVE-2017-8585**

A denial of service vulnerability exists when Microsoft Common Object Runtime Library improperly handles web requests. An attacker who successfully exploited this vulnerability could cause a denial of service against a .NET web application.

A remote unauthenticated attacker could exploit this vulnerability by issuing specially crafted requests to the .NET  application.

The update addresses the vulnerability by correcting how the .NET web application handles web requests.

More info: [Microsoft Common Vulnerabilities and Exposures CVE-2017-0248](https://portal.msrc.microsoft.com/en-US/security-guidance/advisory/CVE-2017-8585).

## Quality and Reliability

### Issue 431586 - Memory leak in COM Interop with multiple type libs with same GUID

Resolves a memory leak in the GUID->Type caching when multiple structs are mashalled with the same GUID inside a SAFEARRAY. For example, this situation can happen when marshaling multiple structs that both #include a common IDL that that contains dependent structs. Previously, the common dependent structs were cached multiple times leading to a memory leak.

## Getting the Update

The Security and Quality Rollup is available via Windows Update, Windows Server Update Services and Microsoft Update Catalog. The Security Only Update is available via Windows Server Update Services and Microsoft Update Catalog. The [updates for Windows 10](https://support.microsoft.com/en-us/help/4025338/windows-10-update-kb4025338) are integrated with the [Windows 10 Monthly Update](https://support.microsoft.com/help/4018124).

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

The following Docker images have been updated with today's release.

You must explicitly re-pull images to update your local Docker image cache, for example with `docker pull microsoft/dotnet-framework:4.7`. The Docker client does not pull updated base images automatically.

#### .NET Framework

The .NET Framework Docker images have been updated to include the .NET Framework July Security and Quality Rollup and have been rebased on top of the latest [microsoft/windowsservercore](https://hub.docker.com/r/microsoft/windowsservercore/) base image (released today).

- [microsoft/aspnet](https://hub.docker.com/r/microsoft/dotnet-framework/)
- [microsoft/dotnet-framework](https://hub.docker.com/r/microsoft/dotnet-framework/)

Note: The update to the [microsoft/wcf](https://hub.docker.com/r/microsoft/wcf/) image are still pending.

#### .NET Core

The .NET Core Docker images have been updated to rebase on top of the latest [microsoft/nanoserver](https://hub.docker.com/r/microsoft/nanoserver/) base image (released today). .NET Core has not been updated.

- [microsoft/aspnetcore](https://hub.docker.com/r/microsoft/aspnetcore/)
- [microsoft/aspnetcore-build](https://hub.docker.com/r/microsoft/aspnetcore-build/)
- [microsoft/dotnet](https://hub.docker.com/r/microsoft/dotnet-framework/)

### Previous Monthly Rollups

The last few .NET Framework Monthly updates are listed below for your convenience:

- [June 2017 Cumulative Quality Update for Windows 10](https://blogs.msdn.microsoft.com/dotnet/2017/06/27/net-framework-june-2017-cumulative-quality-update-for-windows-10/)
- [May 2017 Cumulative Quality Update for Windows 10](https://blogs.msdn.microsoft.com/dotnet/2017/05/26/net-framework-may-2017-cumulative-quality-update-for-windows-10/)
- [May 2017 Security and Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2017/05/09/net-framework-may-2017-monthly-rollup/)
- [April 2017 Security and Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2017/04/11/net-framework-april-2017-monthly-rollup/)

Previously released security and quality updates are included in today's release.You can read the [.NET Framework Monthly Rollups Explained](https://blogs.msdn.microsoft.com/dotnet/2016/10/11/net-framework-monthly-rollups-explained/) to learn more about how the .NET Framework is updated.