# .NET Core May 2018 Update

Today, we are releasing the .NET Core May 2018 Update. This update includes .NET Core [2.1.200](https://github.com/dotnet/core/blob/master/release-notes/download-archives/2.1.200-sdk-download.md) SDK and [ASP.NET Core 2.0.8](https://www.nuget.org/packages/Microsoft.AspNetCore.All).

# Security

Microsoft is releasing this security advisory to provide information about a vulnerability in .NET Core and .NET native version 2.0. This advisory also provides guidance on what developers can do to update their applications to remove this vulnerability.

Microsoft is aware of a denial of service vulnerability that exists when .NET Framework and .NET Core improperly process XML documents. An attacker who successfully exploited this vulnerability could cause a denial of service against a .NET Framework, .NET Core, or .NET native application.

The update addresses the vulnerability by correcting how .NET Framework, .NET Core, and .NET native applications handle XML document processing.

If your application is an ASP.NET Core application, developers are also advised to update to [ASP.NET Core 2.0.8](https://www.nuget.org/packages/Microsoft.AspNetCore.All).

[CVE-2018-0765: .NET Core Denial Of Service Vulnerability](https://github.com/dotnet/announcements/issues/67)

## Getting the Update

The .NET Core May 2018 Update is available from the [.NET Core download page](https://github.com/dotnet/core/blob/master/release-notes/download-archives/2.1.200-sdk-download.md) and the [Microsoft.AspNetCore.All](https://www.nuget.org/packages/Microsoft.AspNetCore.All) package on NuGet.

You can always download the latest version of .NET Core at [.NET Downloads](https://www.microsoft.com/net/download).

## Docker Images

.NET Docker images have been updated for today’s release. The following repos have been updated.

* [microsoft/dotnet](https://hub.docker.com/r/microsoft/dotnet/)
* [microsoft/dotnet-samples](https://hub.docker.com/r/microsoft/dotnet-samples/)
* [microsoft/aspnetcore](https://hub.docker.com/r/microsoft/aspnetcore/)
* [microsoft/aspnetcore-build](https://hub.docker.com/r/microsoft/aspnetcore-build/)

Note: Look at the “Tags” view in each repository to see the updated Docker image tags.

Note: You must re-pull base images in order to get updates. The Docker client does not pull updates automatically.

## Previous .NET Core Updates

The last few .NET Core updates follow:

* [April 2018 Update](https://blogs.msdn.microsoft.com/dotnet/2018/04/17/net-core-april-2018-update/)
* [March 2018 Update](https://github.com/dotnet/core/issues/1341)
* [January 2018 Update](https://blogs.msdn.microsoft.com/dotnet/2018/01/09/net-core-january-2018-update/)
* [November 2017 Update](https://blogs.msdn.microsoft.com/dotnet/2017/11/14/net-core-november-2017-update/)
