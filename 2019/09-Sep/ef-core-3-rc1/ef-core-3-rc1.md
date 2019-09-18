We previously [said that preview 9][1] would be your last chance to test EF Core 3.0 and EF 6.3 before general availability. But it turns out that we made enough improvements to our libraries and across the whole of .NET Core 3.0 to justify publishing a release candidate build. Hence the packages for [EF Core 3.0 RC1][2] and [EF 6.3 RC1][3] were uploaded to [nuget.org][4] today.

### Consider installing daily builds {#consider-installing-daily-builds}

Although RC1 builds contain several improvements, we took several more critical bug fixes after the RC1 branch was created.

Detailed instructions to install daily builds, including the necessary NuGet feeds, can be found in the [How to get daily builds of ASP.NET Core][5] article.

For the best experience with daily builds, install at least the 3.0 RC1 version of the [.NET Core SDK][6], and [ASP.NET Core][7], which where also published today.

Here are a couple of the most relevant improvements you may want to verify:

*   Work on the EF Core in-memory provider was finished and most query features should now be working (the majority of it went into RC1)
*   EF Core's compilation performance was improved significantly for complex queries

For other details on recent changes and how to install the packages, breaking changes and known workarounds, please refer to the information in the [preview 9 blog post][1].

### What happens next {#what-happens-next}

We intend to release the final versions of EF Core 3.0 and EF 6.3 on [September 23 at .NET Conf][8], so we are very close to done.

The team has switched focus to to updating [our documentation][9] for the new releases, and to the 3.1 release, which we plan to release later this year.

### Thank you {#thank-you}

Thank you for helping make this a better release, and please keep the feedback coming! Although any important bugs reported at this stage will likely not make it into 3.0, we will consider them for 3.1.

 [1]: https://devblogs.microsoft.com/dotnet/announcing-entity-framework-core-3-0-preview-9-and-entity-framework-6-3-preview-9/
 [2]: https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/3.0.0-rc1.19456.14
 [3]: https://www.nuget.org/packages/EntityFramework/6.3.0-rc1-19458-04
 [4]: https://nuget.org
 [5]: https://github.com/aspnet/AspNetCore/blob/master/docs/DailyBuilds.md
 [6]: https://devblogs.microsoft.com/dotnet/announcing-net-core-3-0-release-candidate-1/
 [7]: https://devblogs.microsoft.com/aspnet/asp-net-core-and-blazor-updates-in-net-core-3-0-release-candidate-1/
 [8]: https://devblogs.microsoft.com/dotnet/join-us-for-net-conf-2019-sept-23-25/
 [9]: http://docs.microsoft.com/ef/
