---
post_title: .NET 6 will reach End of Support on November 12, 2024
post_slug: dotnet-6-end-of-support
author1: rbhanda
microsoft_alias: rbhanda
featured_image: dotnet-6-support.png
categories: .NET, Maintenance & Updates
summary: .NET 6 will reach end of support on November 12, 2024, this blog breaks down all the valuable information you need to know and how to update to .NET 8.
post_date: 2024-07-31 11:00:00
---

.NET 6 will reach [end of support on Nov 12, 2024](https://github.com/dotnet/core/blob/main/release-notes/6.0/README.md). After that, Microsoft will no longer provide updates for .NET 6. Security fixes and technical support will no longer be available for .NET 6. You'll need to update to [.NET 8](https://devblogs.microsoft.com/dotnet/announcing-dotnet-8/) before this date to stay supported.

[Commercial support](https://github.com/dotnet/core/blob/main/support.md#commercial-support) for .NET is also provided by enterprise Linux companies, which may have other policies (see later section).

## Support Policy

.NET 6 is an [LTS release](https://dotnet.microsoft.com/platform/support/policy/dotnet-core#release-types), supported for 36 months, ending on November 12, 2024.

![.NET Release Schedule](https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2024/03/release-schedule.svg)

November 12th is a patch Tuesday release day. .NET 6 may be updated one last time, on that day, if there is a known critical issue.

## What to expect

You can expect the following after .NET 6 reaches end of support:

- Applications that use this version **will** continue to run.
- No new security updates will be issued for .NET 6.
- Continuing to use an unsupported version will expose you to security vulnerabilities.
- You may not be able to access technical support for .NET 6 applications.
- You will start getting `NETSDK1138` build warnings when targeting .NET 6 from a later SDK.
- You will get "gold bar" warnings in Visual Studio when targeting .NET 6.

## Visual Studio Compatibility

Starting with the January 2025 servicing update for Visual Studio 2022 17.8, Visual Studio 2022 17.10, and Visual Studio 2022 17.12, the .NET 6 component in Visual Studio will be marked as out of support. Existing installations won’t be affected.

You must retarget to .NET 8 (or later) to stay supported.

You can use the "remove out of support components" option to remove .NET 6 from existing Visual Studio installations.

## Enterprise Linux Support

.NET is also supported by enterprise Linux companies, who have their own support policies.

- .NET provided by Red Hat is supported according to [.NET Life Cycle](https://access.redhat.com/support/policy/updates/net-core).
- .NET provided by Canonical in Ubuntu is supported according to the following statement.

"Under the [Ubuntu Pro](https://ubuntu.com/pro) support plans, all packages in Ubuntu LTS main and universe components get five years of standard support and ten years of security coverage support."

## Upgrading to .NET 8

You can upgrade your app to .NET 8 by changing the value of the `TargetFramework` property in your project file to `net8.0`. You will also need to update your development and hosting environments. This process is covered in more detail in [Upgrade to a new .NET version](https://learn.microsoft.com/dotnet/core/install/upgrade).

## Using .NET 6 apps

If you're using a .NET 6 app, we recommend reaching out to the software developer or vendor who produced it to ask if an updated version that uses .NET 8 is available.

## Resources

* [.NET downloads](https://dotnet.microsoft.com/download/dotnet)
* [.NET Deployment](https://docs.microsoft.com/dotnet/core/deploying/)
* [.NET Support Policy](https://dotnet.microsoft.com/platform/support/policy/dotnet-core)
* [.NET 7 Breaking Changes](https://docs.microsoft.com/dotnet/core/compatibility/7.0)
* [.NET 8 Breaking Changes](https://docs.microsoft.com/dotnet/core/compatibility/8.0)
* [.NET Upgrade Assistant](https://learn.microsoft.com/dotnet/core/porting/upgrade-assistant-overview)
* [Migrate from ASP.NET Core in .NET 6 to .NET 8](https://learn.microsoft.com/aspnet/core/migration/70-80?view=aspnetcore-8.0&tabs=visual-studio)
* [Upgrading .NET MAUI from .NET 6 to .NET 8](https://github.com/dotnet/maui/wiki/Upgrading-.NET-MAUI-from-.NET-7-to-.NET-8)

## Closing

.NET 6 will be reaching end of support on November 12, 2024.  After that date, no additional updates or technical support will be offered. We strongly recommend you start migrating your .NET 6 apps to .NET 8.
