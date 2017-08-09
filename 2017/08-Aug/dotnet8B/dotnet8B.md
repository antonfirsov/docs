# .NET Framework August 2017 Security and Quality Rollup

Today, we are releasing the August 2017 Security and Quality Rollup and Security Only Update. 

This update applies to Windows 10 and Windows Server 2016.

## Security

There are no new security changes for the .NET Framework this month.

## Quality and Reliability

#### Networking

* Update [Token Binding](https://docs.microsoft.com/windows-server/security/token-binding/introducing-token-binding) to be conformant with [Token Binding v0.10 spec](https://datatracker.ietf.org/doc/draft-ietf-tokbind-protocol/01/). See [Announcing .NET Framework 4.6.1](https://blogs.msdn.microsoft.com/dotnet/2015/11/30/net-framework-4-6-1-is-now-available/#networking) for the introduction of Token Binding support.

## Getting the Update

The Security and Quality Rollup is available via Windows Update, Windows Server Update Services, Microsoft Update Catalog, and Docker. 

### Windows 10

You can get the update via the Windows 10 Monthly Update.

|Windows Version |.NET Version|Rollup KB |Security-only KB |
|:---------------|:-----------|:---------|:----------------|
|Windows 10 Update 1703 (Creators Update)|.NET Framework 3.5 and 4.7|[4034674](https://support.microsoft.com/kb/4034674) | N/A |
|Windows 10 Update 1607 (Anniversary Update) <br> Windows Server 2016|.NET Framework 3.5, 4.6.2 and 4.7|[4034658](https://support.microsoft.com/kb/4034658) | N/A |
|Windows 10 Update 1511 |.NET Framework 3.5 and 4.6.1|[4034660](https://support.microsoft.com/kb/4034660) | N/A |
|Windows 10 Update 1507|.NET Framework 3.5 and 4.6|[4034668](https://support.microsoft.com/kb/4034668) | N/A |

### Docker Images

The following Docker images have been updated with today's release.

Note: You must explicitly re-pull images to update your local Docker image cache, for example with `docker pull microsoft/dotnet-framework:4.7`.

#### .NET Framework

The .NET Framework Docker images have been updated to include the .NET Framework July Security and Quality Rollup and have been rebased on top of the latest [microsoft/windowsservercore](https://hub.docker.com/r/microsoft/windowsservercore/) base image (released today).

* [microsoft/aspnet](https://hub.docker.com/r/microsoft/aspnet/)
* [microsoft/dotnet-framework](https://hub.docker.com/r/microsoft/dotnet-framework/)
* [microsoft/dotnet-framework-samples](https://hub.docker.com/r/microsoft/dotnet-framework-samples/)
* [microsoft/wcf](https://hub.docker.com/r/microsoft/wcf/)

#### .NET Core

The .NET Core Docker images have been updated to rebase on top of the latest [microsoft/nanoserver](https://hub.docker.com/r/microsoft/nanoserver/) base image (released today).

* [microsoft/aspnetcore](https://hub.docker.com/r/microsoft/aspnetcore/)
* [microsoft/aspnetcore-build](https://hub.docker.com/r/microsoft/aspnetcore-build/)
* [microsoft/dotnet](https://hub.docker.com/r/microsoft/dotnet/)
* [microsoft/dotnet-nightly](https://hub.docker.com/r/microsoft/dotnet-nightly)
* [microsoft/dotnet-samples](https://hub.docker.com/r/microsoft/dotnet-samples/)

### Previous Monthly Rollups

The last few .NET Framework Monthly updates are listed below for your convenience:

* [July 2017 Cumulative Quality Update for Windows 10](https://blogs.msdn.microsoft.com/dotnet/2017/08/01/net-framework-july-2017-cumulative-quality-update-for-windows-10/)
* [July 2017 Quality Update for WPF](https://blogs.msdn.microsoft.com/dotnet/2017/07/25/net-framework-july-2017-quality-update)
* [July 2017 Preview of Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2017/07/24/net-framework-july-2017-preview-of-quality-rollup/)
* [July 2017 Security and Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2017/07/11/net-framework-july-2017-security-and-quality-rollup/)
