# Announcing .NET Core 3.0 Release Candidate 1

Today, we're announcing [.NET Core 3.0 Release Candidate 1](https://dotnet.microsoft.com/download/dotnet-core/3.0). Just like with [Preview 9](https://devblogs.microsoft.com/dotnet/announcing-net-core-3-0-preview-9/), we've focused on polishing .NET Core 3.0 for a final release. We are now getting very, very close. We intend to release the final version on [September 23 at .NET Conf](https://devblogs.microsoft.com/dotnet/join-us-for-net-conf-2019-sept-23-25/).

[Download .NET Core 3.0 RC1](https://dotnet.microsoft.com/download/dotnet-core/3.0) right now on Windows, macOS, and Linux.

Details:

* [ASP.NET Core 3.0 RC1](https://devblogs.microsoft.com/aspnet)
* [.NET Core 3.0 release notes](https://github.com/dotnet/core/tree/master/release-notes/3.0)
* [GitHub release](https://github.com/dotnet/core/releases/tag/v3.0.0-rc1)

## Why RC1?

The [.NET Core 3.0 Preview 9 post](https://devblogs.microsoft.com/dotnet/announcing-net-core-3-0-preview-9/) stated that Preview 9 would be the last release before the final GA one. We, or at least the tireless writer of these blog posts, were mistaken. And now for the explanation.

For technical and historical reasons, the .NET toolset (compilers, NuGet client, MSBuild, ...) is duplicated between Visual Studio and the .NET Core SDK. Important changes were made in the toolset as part of Visual Studio 2019 16.3 Preview 4, also released today. It is critical that the .NET Core SDK version that is part of any Visual Studio release includes the same toolset in order to deliver a compatible experience in all scenarios.

We should have realized that there was a high likelihood that we might need to release changes to accomodate another Visual Studio preview. Making fixes in the .NET toolset like this is standard operating procedure. We could have released a new .NET Core SDK and only delivered it via Visual Studio, however, we've broken people in the (now distant) past with that approach. As a result, when we release a new .NET Core SDK, we make it available for everyone in all the places.

## Visual Studio Support

.NET Core 3.0 is supported with Visual Studio 2019 16.3 Preview 4 and Visual Studio for Mac 8.3, which were also released today. Please upgrade to it for the best (and supported) experience with .NET Core 3.0 Preview 9. See Visual Studio 2019 16.4 release notes for more information.

The C# Extension for Visual Studio Code is always updated to support new .NET Core versions. Make sure you have the latest version of the C# extension installed.

## Go Live

NET Core 3.0 Preview RC1 is supported by Microsoft and can be used in production. We strongly recommend that you test your app running on Preview RC1 before deploying into production. If you find an issue with .NET Core 3.0, please file a [GitHub issue](https://github.com/dotnet/core/issues) and/or contact [Microsoft support](https://support.microsoft.com/en-us/supportforbusiness/productselection?fltadd=sps-business-1&sapId=4fd4947b-15ea-ce01-080f-97f2ca3c76e8).

## Closing

The .NET Core 3.0 release is coming close to completion, and the team is solely focused on stability and reliability now that we’re no longer building new features. Please tell us about any issues you find, ideally as quickly as possible. We want to get as many fixes in as possible before we ship the final 3.0 release.
