# .NET Core CLI Usage Data Release

We are releasing anonymous .NET Core CLI usage data that has been collected as part of the .NET Core SDK. As an open source application platform, we feel that it is important to release this data for three key reasons:

- Provide context and motivation for product decisions that have been made.
- Enable you and other community members to participate in decision making based on this data.
- Highlight .NET Core CLI scenarios that are the most common and those that should be improved.

We will release new data on a regular schedule going forward. The data is licensed with the [Open Data Commons Attribution License](https://opendatacommons.org/licenses/by/).

Note: You can opt-out of telemetry by setting the `DOTNET_CLI_TELEMETRY_OPTOUT` variable, as described in [.NET Core documentation](https://docs.microsoft.com/dotnet/core/tools/telemetry).

## .NET Core SDK Usage Data

.NET Core SDK usage data is available, by month, in CSV format:

* [April 2016](https://dotnetcoredata.blob.core.windows.net/dotnetclidata/CLI_2016-04-01.csv)
* [May 2016](https://dotnetcoredata.blob.core.windows.net/dotnetclidata/CLI_2016-05-01.csv)
* [June 2016](https://dotnetcoredata.blob.core.windows.net/dotnetclidata/CLI_2016-06-01.csv)
* [July 2016](https://dotnetcoredata.blob.core.windows.net/dotnetclidata/CLI_2016-07-01.csv)
* [August 2016](https://dotnetcoredata.blob.core.windows.net/dotnetclidata/CLI_2016-08-01.csv)
* [September 2016](https://dotnetcoredata.blob.core.windows.net/dotnetclidata/CLI_2016-09-01.csv)
* [October 2016](https://dotnetcoredata.blob.core.windows.net/dotnetclidata/CLI_2016-10-01.csv)
* [November 2016](https://dotnetcoredata.blob.core.windows.net/dotnetclidata/CLI_2016-11-01.csv)
* [December 2016](https://dotnetcoredata.blob.core.windows.net/dotnetclidata/CLI_2016-12-01.csv)
* [January 2017](https://dotnetcoredata.blob.core.windows.net/dotnetclidata/CLI_2017-01-01.csv)
* [February 2017](https://dotnetcoredata.blob.core.windows.net/dotnetclidata/CLI_2017-02-01.csv)

Note to reviewers: More data still coming, before posting.

## Shape of the Data

The following is an example of the data you will find in the CSV files.

```csv
Occurrences,Date,EventName,CountryOrRegionISOLong,OSVersion,OSPlatform,RuntimeID,ProductVersion
5,2017-01-31,restore,BLR,8,Linux,debian.8-x64,1.0.0-preview2-1-003177
1,2017-01-30,restore,CZE,16.10,Linux,ubuntu.16.10-x64,1.0.0-preview2-1-003177
1,2017-02-01,projectmodel-server,AUS,10.0.14393,Windows,win10-x64,1.0.0-preview2-003131
1,2017-01-31,projectmodel-server,NLD,10.0.14393,Windows,win10-x64,1.0.0-preview2-003131
2,2017-02-01,restore,AUS,10.12,Darwin,osx.10.12-x64,1.0.0-preview2-1-003177
38,2017-02-01,build,AUS,10.0.14393,Windows,win10-x64,1.0.0-preview2-003131
27,2017-02-01,razor-tooling,CHN,10.0.14393,Windows,win10-x64,1.0.0-preview2-003131
3,2017-02-01,build,VNM,10.0.14393,Windows,win10-x64,1.0.0-preview2-003131
5,2017-02-01,restore,JPN,10.0.14393,Windows,win10-x64,1.0.0-preview2-003131
1,2017-02-01,publish-iis,CHN,10.0.14393,Windows,win10-x64,1.0.0-preview2-003131
1,2017-02-01,razor-tooling,VNM,10.0.14393,Windows,win10-x64,1.0.0-preview2-003131
1,2017-02-01,build,MYS,10.0.10586,Windows,win10-x64,1.0.0-preview2-1-003177
1,2017-02-01,aspnet-codegenerator,USA,10.0.14393,Windows,win10-x64,1.0.0-preview2-003131
102,2017-02-01,test,USA,10.0.14393,Windows,win10-x64,1.0.0-preview2-1-003177
```
 
 ## Product Findings and Decisions

This data has been very useful to the .NET Core team for a year now. In some cases, like looking at overall usage or at the usage of specific commands, we are very reliant on this data to make decisions. For more specific decisions, like the case of removing the OpenSSL dependency on macOS, we used the data as secondary evidence to user feedback.

Here are some interesting findings that we have made based on this data:

- .NET Core usage is growing -- >10% per month.
- .NET Core usage is geographically diverse -- used in n countries and all continents.
- .NET Core CLI tools are a very important part of the overall .NET Core experience -- relative to .NET Framework, the CLI tools are novel.
- Developers do not use the .NET Core SDK the same way on Windows, macOS and Linux -- the popular commands are different per OS. 
- The publishing model for .NET Core apps is likely confusing some people -- the difference in the popular commands suggests a use of .NET Core that differs from our guidance (more investigation needed). 
- We have more work to do to reach out to the Linux and macOS communities -- we would like to see increased use of .NET Core on thoses OSes.
- Our approach to supporting Linux (one build per distro) isn't providing broad enough support -- .NET Core was used on n Linux distros yet it only works well on y distros.
- There are gaps in the data that limit our understanding -- we would like to know if the SDK is running in a container, for example.

We have immediate and longer-lead plans based on this data:

- .NET Core 2.0 will ship with a single Linux build, making it easier to use .NET Core on Linux. .NET Core 1.x has nearly a dozen Linux builds for specific distros (for example, RHEL, Debian and Ubuntu are all separate) and limits support to those distros.
- .NET Core 2.0 will be easily buildable from source so that Linux distros can include .NET Core in their package repository/archive/collection. We are talking to distros about that now.
- .NET Core 2.0 will not require OpenSSL on macOS, with the intention of increasing adoption on macOS.
- .NET Core 2.0 will include more data points for the SDK. More on that below.
- We will attend and/or encourage local experts to participate in more conferences (globally) to talk about .NET Core.

More forward-looking:

- Fix the build and publishing model for .NET Core -- the differences between `run`, `build` and `publish` are likely confusing people.
- Enable more CLI scenarios -- enable distribution of tools, possibly like the way [NPM does global installs](https://docs.npmjs.com/cli/install).
 
The way that the data was used is different in each case. In some cases, like looking at overall usage or at the usage of specific commands, we are very reliant on this data to make decisions. In the case of removing the OpenSSL dependency on macOS, we used the data as secondary evidence to user feedback. The findings and decisions above were influenced in a significant way, and in some cases primarily, by the data we are releasing today.

## Data Insights
 
The data reveals interesting trends in addition to product development insights. Let's take a look at historical data (since April 2016):

Note: this data is just from direct use of the CLI. There is of course a significant amount of .NET Core usage via Visual Studio, as well.

### Command Variations by Operating System
![Commands by OS](cli-commands-by-os.png)

There are some interesting and surprising differences in command usage between operating systems. We can see that `build` is by far the leading command on Windows, `run` on Linux, and `restore` on macOS. I'd interpret this to say that we're seeing a lot of application development on Windows, maybe more "kicking the tires" scaffolding applications on macOS using Yeoman (since `dotnet new` usage is low), while Linux is primarily being used to host applications.

Note: The chart says "OSX", which is the old name for macOS.

### Weekly Trends
![Weekly cycle](cli-weekly-cycle.png)

You can see that there's an obvious cycle that follows the work week. Looking closer, it's clear that the `build` and `restore` commands drop off quite a bit on the weekend, while the `run` command doesn't quite as much.

We wonder if developers use `build` and `restore` while automation that doesn't take weekends off uses `run`.

### Geographic Distributions
![Geographic Distribution](cli-client-os-geo.png)

It's interesting to take a look at the geographic variations in operating system usage. Most geographies have a mix, but you can see that some areas run predominantly on a single operating system, at least as it relates to .NET Core usage.

This data and visualization is based on the IP address seen on the server. It is not collected by the CLI. The IP address is not stored, but converted to a city/country representation.

### Overall Operating System Distribution
![OS Distribution](cli-os-distribution-by-distinctip.png)

Given .NET's roots, it's not surprising to see a large Windows following. It's exciting to see   substantial Linux and Darwin (macOS) usage as well.

### Operating System Version Distribution

![Operating System Version Distribution](cli-os-versions.png)

It looks like .NET Core is running mostly on the newest operating system versions at this point. This aligns with our expecation that .NET Core has been adopted mostly by "early adopters" to this point. In 2-3 years, we expect that the operating system distribution will be more varied. 

## How the Data is Collected

.NET Core has two primary distributions: the .NET Core SDK for development and build scenarios and the .NET Core Runtime for running apps in production. The .NET Core SDK [collects usage data](https://docs.microsoft.com/dotnet/core/tools/telemetry) while the .NET Core Runtime does not.

The SDK collects the following pieces of data:
 - The command being used (for example, `build`, `restore`).
 - The `ExitCode` of the command.
 - For test projects, the test runner being used.
 - The timestamp of invocation.
 - Whether runtime IDs are present in the `runtimes` node.
 - The CLI version being used.

The data collected is anonymous. 

The data does not include Visual Studio usage since Visual Studio uses MSBuild directly and not the higher-level .NET Core CLI tools (which is where data collection is implemented).

You can opt-out of telemetry by setting the `DOTNET_CLI_TELEMETRY_OPTOUT` variable, as described in [.NET Core documentation](https://docs.microsoft.com/dotnet/core/tools/telemetry).

## Data for .NET Core 2.0

The data that has been collected the .NET Core SDK 1.0 has demonstrated some important gaps in our understanding of how the product is being used. The following additional data points are planned for .NET Core SDK 2.0.

- `dotnet` command arguments and options -- Determine more detailed product usage. For example, for `dotnet new`, collect the template name. For `dotnet build --framework netstandard2.0`, collect the framework specified. Only known arguments and options will be collected (not arbitrary strings).
- Containers -- Determine if the SDK is running in a container. Useful to help prioritize container-related investments.
- Command duration --  Determine how long a command runs. Useful to identify performance problems that should be investigated.
- Target Framework(s) -- Determine which target frameworks are used and whether multiple are specified. Useful to understand which .NET Standard versions are the most popular and whether new guidance should be written, for example.
- Hashed project file path -- Determine how much of .NET Core SDK usage is for "kicking the tires" apps verus more serious projects. Useful for prioritizing features like interactive template creation (like Yeoman).
- Hashed MAC address -- Determine a unique ID for the machine. Useful to determine the aggregate population of active users, for example.

Note: Any data that could be considered personally identifiable will not be publicly released.

## More to Come

We will continue to make this data available to you in a timely manner, and we're going to look into making it possible for you to visualize the kinds of trends we're seeing (like in the images above). For now, we're making the raw data available to you.

Thanks to everyone that has been using .NET Core. The community engagement on the project has been amazing and we are making a great product together. Thanks to everyone who has telemetry enabled. This information is helping us make the product better and will become even more useful in the future. We are now doing our part to make the data collected publicly available. This makes good on a promise that we made at the start of the project. We now look forward to other developers reasoning about this data and using it as part of project decision making.
