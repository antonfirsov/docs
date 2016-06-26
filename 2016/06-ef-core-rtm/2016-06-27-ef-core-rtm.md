# Entity Framework Core 1.0.0 Available

Entity Framework (EF) Core is a lightweight, extensible, and cross-platform version of Entity Framework. Today we are making Entity Framework Core 1.0.0 available. This coincides with the release of [.NET Core](https://blogs.msdn.microsoft.com/dotnet/) and [ASP.NET Core](https://blogs.msdn.microsoft.com/webdev/).

## Upgrading from RC2 to RTM

If you are upgrading an application from RC2 to RTM, be sure to read our [Upgrading from RC2 to RTM](https://docs.efproject.net/en/latest/miscellaneous/rc2-rtm-upgrade.html) article.

## When to use EF Core

**We now have an [EF Core vs. EF6.x section in our documentation](https://docs.efproject.net/en/latest/efcore-vs-ef6/index.html). It includes guidance on when to use EF Core, feature comparisons, and information on porting to EF Core.**

These are the types of applications we would recommend using EF Core for. For all other applications, you should consider using EF6.x.
 * New applications that do not require features that are not yet implemented in EF Core.
 * Applications that target .NET Core, such as Universal Windows Platform (UWP) and ASP.NET Core applications.

## Getting started with EF Core

You can find the documentation for EF Core at [docs.efproject.net](http://docs.efproject.net). In particular, you probably want to start with a tutorial for the type of application you want to build.
 * [Full .NET (Console, WPF, WinForms, and ASP.NET 4)](https://docs.efproject.net/en/latest/platforms/full-dotnet/index.html)
 * [Universal Windows Platform (UWP)](https://docs.efproject.net/en/latest/platforms/uwp/index.html)
 * [ASP.NET Core](https://docs.efproject.net/en/latest/platforms/aspnetcore/index.html)
 * [.NET Core (Windows, OSX, Linux, etc.)](https://docs.efproject.net/en/latest/platforms/netcore/index.html)

## Supported databases

At the time of release, the following database providers are available for EF Core. See our [providers page](https://docs.efproject.net/en/latest/providers/index.html) for more information and links to getting started.
 * Microsoft SQL Server
 * SQLite
 * Postgres (Npgsql)
 * SQL Server Compact Edition
 * InMemory (for testing purposes)
 * DevArt has paid providers for MySQL, Oracle, and many other databases

We are continuing to work with other database vendors to have more providers available soon. At this stage, they do not have a publically available timeline for their releases.

As always, we'd like to thank Shay Rojansky and Erik Ejlskov Jensen for their collaboration to provide the Npgsql and SQL Compact providers and drive improvements in the core EF Core code base.

## Known Issues

Here are some specific known issues you should be aware of when using 1.0.0.
 * [EF Core: Some queries significantly slower when executed async release-note](https://github.com/aspnet/Home/releases/tag/1.0.0#issue142)
 * [EF Core: Pre-RTM migrations may need maxLength added to string columns release-note](https://github.com/aspnet/Home/releases/tag/1.0.0#issue141)
 * [EF Core: Commands on UWP projects require manually adding binding redirects release-note](https://github.com/aspnet/Home/releases/tag/1.0.0#issue139)

It is also worth noting that the EF Core LINQ provider is still maturing. LINQ providers are complex and take time to mature, and EF Core in no exception. Improving our LINQ provider will be one of the main focuses of our upcoming releases. The majority of issues result in an exception when you attempt to execute a LINQ query that contains a particular pattern. There are often ways to workaround these issues by expressing the same query using different patterns, or evaluating parts of the query client-side. We try to include these workarounds in the issue, when they are available.

## What We're Working on Now

### EF6.x

For the last while we've had to focus almost exclusively on EF Core to get 1.0.0 shipped. We are now transitioning to focus on the EF6.2 release in parallel with updates to EF Core. Shortly, we'll also have a short period of time where our team focuses almost exclusively on EF6.x, in order to catch up on some of our backlog. We anticipate having the first preview of EF6.2 available in the next couple of months.

### EF Core

We've already started work on updates to EF Core. We're working on bug fixes, improving the capabilities of our LINQ provider, and getting started on some of the features we want to add to EF Core (for example, we've already added DbSet.Find). We'll more share details of our upcoming releases in the near future.
