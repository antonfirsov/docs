# .NET Core April 2018 Update

Today, we are releasing the [.NET Core April 2018 Update](https://github.com/dotnet/core/issues/1456). This update includes .NET Core [1.0.11](https://github.com/dotnet/core/blob/master/release-notes/1.0/1.0.11.md), [1.1.8](https://github.com/dotnet/core/blob/master/release-notes/1.1/1.1.8.md) and [2.0.7](https://github.com/dotnet/core/blob/master/release-notes/2.0/2.0.7.md). There are no new security fixes in this update.

## Notable Fixes and Commits

#### CoreFX

* [`[522373a]`](https://github.com/dotnet/corefx/pull/27614/commits/522373a4bf70f6ec69f41a2681394f4167341364) : Adding support for ncurses 6.1 TERM format on System.Console.

#### CoreCLR
* [`[45c57cc]`](https://github.com/dotnet/coreclr/pull/16577/commits/45c57cc0daf228856ae48d60ff25c76a6ce83983) : Fix detection of YMM regs presence
* [`[802ca8c]`](https://github.com/dotnet/coreclr/pull/16756/commits/802ca8cfa424838003c2a61a5d17f78fcabe042b) : Remove flock *usage from InternalCreateFile in PAL
* [`[d40ce91]`](https://github.com/dotnet/coreclr/pull/16152/commits/d40ce91ca58387b62456fb137aa829d8c3ceed6c) : Fix SIGSEGV in EventPipe on Shutdown
* [`[06a1cd1]`](https://github.com/dotnet/coreclr/pull/15444/commits/06a1cd1223df9dfc190fd74603dbb9119636f554) : Fix uaf in DestroyThread function

## Getting the Update

The .NET Core April 2018 Update is available from the [.NET Core download page](https://github.com/dotnet/core/blob/master/release-notes/download-archive.md).

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

* [March 2018 Update](https://github.com/dotnet/core/issues/1341)
* [January 2018 Update](https://blogs.msdn.microsoft.com/dotnet/2018/01/09/net-core-january-2018-update/)
* [November 2017 Update](https://blogs.msdn.microsoft.com/dotnet/2017/11/14/net-core-november-2017-update/)
