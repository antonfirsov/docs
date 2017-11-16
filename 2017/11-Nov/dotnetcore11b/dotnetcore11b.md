# .NET Core November 2017 Update

Today, we are releasing the .NET Core November Update. This includes .NET Core 1.0.8, 1.1.5 and 2.0.1 and .NET Core SDK 1.1.5 and 2.0.3.

Details regarding the security issues addressed by this release can be seen in the [Security Advisory announcement](https://github.com/dotnet/announcements/issues/44).

## Security

#### CVE-2017-8585 -- Malformed Certificate can cause Denial of Service

Microsoft is releasing this security advisory to provide information about a vulnerability in the public versions of .NET Core 1.0 and 1.1, and 2.0. This advisory also provides guidance on what developers can do to update their applications correctly.

Microsoft is aware of a security vulnerability in the public version of .NET Core where a malformed certificate or other ASN.1 formatted data could lead to a denial of service via an infinite loop on Linux and macOS.

System administrators are advised to update their .NET Core runtimes to versions 1.0.8, 1.1.5 and 2.0.1. Developers are advised to update their .NET Core SDK to version 2.0.3 or 1.1.5.

[CVE-2017-8585](https://cve.mitre.org/cgi-bin/cvename.cgi?name=CVE-2017-8585)

## Quality

See the release notes for a list of all the quality fixes in this release.

* [.NET Core 2.0.3](https://github.com/dotnet/core/blob/master/release-notes/2.0/2.0.3.md)
* [.NET Core 1.1.5](https://github.com/dotnet/core/blob/master/release-notes/1.1/1.1.5.md)
* [.NET Core 1.0.8](https://github.com/dotnet/core/blob/master/release-notes/1.0/1.0.8.md)

## Getting the Update

The .NET Core November 2017 Update is available from the [.NET Core download page](https://github.com/dotnet/core/blob/master/release-notes/download-archive.md).

You can always download the version of .NET Core at [.NET Downloads](https://www.microsoft.com/net/download).

### Docker Iamges

.NET Docker images have been updated for today's release. The following repos have been updated.

* [microsoft/dotnet](https://hub.docker.com/r/microsoft/dotnet/)

The following repos are in the process of being updated:

* [microsoft/aspnetcore/](https://hub.docker.com/r/microsoft/aspnetcore/)
* [microsoft/aspnetcore-build/](https://hub.docker.com/r/microsoft/aspnetcore-build/)

Note: Look at the “Tags” view in each repository to see the updated Docker image tags.

Note: You must re-pull base images in order to get updates. The Docker client does not pull updates automatically.

Note: The [Windows Docker tag scheme has changed](https://github.com/dotnet/announcements/issues/38).

## Previous .NET Core Updates

The last few .NET Core updates follow:

* [September 2017](https://blogs.msdn.microsoft.com/dotnet/2017/09/22/net-core-september-2017-update-macos-high-sierra-support/)
* [May 2017 Update](https://blogs.msdn.microsoft.com/dotnet/2017/05/09/net-core-may-2017-update/)
* [January 2017 Update](https://blogs.msdn.microsoft.com/dotnet/2017/01/30/january-2017-update-for-asp-net-core-1-1/)
* [December 2016 Update](https://blogs.msdn.microsoft.com/dotnet/2016/12/13/december-2016-update-net-core-1-0/)
* [September 2016 Update](https://blogs.msdn.microsoft.com/dotnet/2016/09/13/announcing-september-2016-updates-for-net-core-1-0/)
