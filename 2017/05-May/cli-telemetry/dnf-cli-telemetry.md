# What we've learned from .NET Core SDK Telemetry

We are releasing .NET Core SDK usage data that has been collected by the .NET Core CLI. We have been using this data to determine the most common CLI scenarios, the distribution of operating systems and to answer other questions we've had, as described below.

As an open source application platform that collects usage data via an SDK, it is important that all developers that work on the project have access to usage data in order to fully participate in and understand design choices and propose product changes. This is now the case with .NET Core.

.NET Core telemetry was first announced in the [.NET Core 1.0 RC2](https://blogs.msdn.microsoft.com/dotnet/2016/05/16/announcing-net-core-rc2/#net-core-tools-telemetry) and [.NET Core 1.0 RTW](https://blogs.msdn.microsoft.com/dotnet/2016/06/27/announcing-net-core-1-0/#net-core-tools-telemetry) blog announcements. It is also documented in [.NET Core telemetry docs](https://docs.microsoft.com/dotnet/core/tools/telemetry).

We will release new data on a quarterly schedule going forward. The data is licensed with the [Open Data Commons Attribution License](https://opendatacommons.org/licenses/by/).

## .NET Core SDK Usage Data

.NET Core SDK usage data is available by quarter in TSV (tab-separated values) format:

* [2016 - Q3](https://dotnetcli.blob.core.windows.net/usagedata/dotnet-cli-usage-2016-q3.tsv)
* [2016 - Q4](https://dotnetcli.blob.core.windows.net/usagedata/dotnet-cli-usage-2016-q4.tsv)
* [2017 - Q1](https://dotnetcli.blob.core.windows.net/usagedata/dotnet-cli-usage-2017-q1.tsv)
* [2017 - Q2](https://dotnetcli.blob.core.windows.net/usagedata/dotnet-cli-usage-2017-q2.tsv)

## The Data

.NET Core has two primary distributions: the .NET Core SDK for development and build scenarios and the .NET Core Runtime for running apps in production. The .NET Core SDK [collects usage data](https://docs.microsoft.com/dotnet/core/tools/telemetry) while the .NET Core Runtime does not.

The SDK collects the following pieces of data:

* The command being used (for example, `build`, `restore`).
* The `ExitCode` of the command.
* The test runner being used, for test projects.
* The timestamp of invocation.
* Whether runtime IDs are present in the `runtimes` node.
* The CLI version being used.

The data collected does not contain personal information.

The data does not include Visual Studio usage since Visual Studio uses MSBuild directly and not the higher-level .NET Core CLI tools (which is where data collection is implemented).

You can opt-out of telemetry by setting the `DOTNET_CLI_TELEMETRY_OPTOUT` variable, as described in [.NET Core documentation](https://docs.microsoft.com/dotnet/core/tools/telemetry).

## Shape of the Data

The following is an example of the data you will find in the TSV (tab separated values) files.

```console
C:\dotnet-core-cli-data>more dotnet-cli-usage-2016-q3.tsv
Timestamp       Occurences      Command Geography       OSFamily        RuntimeID       OSVersion       SDKVersion
9/1/2016 12:00:00 AM    1       bulid   India   Windows win7-x86        6.1.7601        1.0.0-preview1-002702
9/8/2016 12:00:00 AM    1       bulid   Republic of Korea       Windows win81-x64       6.3.9600        1.0.0-preview2-003121
9/19/2016 12:00:00 AM   1       bulid   United States   Windows win81-x64       6.3.9600        1.0.0-preview2-003121
9/12/2016 12:00:00 AM   1       bulid   Ukraine Windows win81-x64       6.3.9600        1.0.0-preview2-003121
8/12/2016 12:00:00 AM   2       bulid   Netherlands     Windows win10-x64       10.0.10240      1.0.0-preview1-002702
9/14/2016 12:00:00 AM   1       debug   Hong Kong       Windows win10-x64       10.0.14393      1.0.0-preview2-003121
9/14/2016 12:00:00 AM   1       debug   United States   Linux   ubuntu.16.04-x64        16.04   1.0.0-preview2-003121
8/27/2016 12:00:00 AM   1       debug   Belarus Windows win10-x64       10.0.10586      1.0.0-preview2-003121
9/16/2016 12:00:00 AM   1       debug   India   Darwin  osx.10.11-x64   10.11   1.0.0-preview2-003131
8/31/2016 12:00:00 AM   1       debug   Sweden  Windows win10-x64       10.0.10586      1.0.0-preview2-003121
8/26/2016 12:00:00 AM   1       debug   Netherlands     Windows win10-x64       10.0.10586      1.0.0-preview2-003121
9/27/2016 12:00:00 AM   2       debug   United States   Windows win10-x64       10.0.10586      1.0.0-preview2-003121
8/2/2016 12:00:00 AM    1       debug   Ireland Linux   ubuntu.16.04-x64        16.04   1.0.0-preview2-003121
8/10/2016 12:00:00 AM   1       debug   United States   Windows win7-x64        6.1.7601        1.0.0-preview1-002702
8/18/2016 12:00:00 AM   1       debug   United States   Linux   ubuntu.16.04-x64        16.04   1.0.0-preview2-003121
```

## Data for .NET Core 2.0

The data that has been collected with the .NET Core SDK has demonstrated some important gaps in our understanding of how the product is being used. The following additional data points are planned for .NET Core SDK 2.0.

* `dotnet` command arguments and options -- Determine more detailed product usage. For example, for `dotnet new`, collect the template name. For `dotnet build --framework netstandard2.0`, collect the framework specified. Only known arguments and options will be collected (not arbitrary strings).
* Containers -- Determine if the SDK is running in a container. Useful to help prioritize container-related investments.
* Command duration --  Determine how long a command runs. Useful to identify performance problems that should be investigated.
* Target Framework(s) -- Determine which target frameworks are used and whether multiple are specified. Useful to understand which .NET Standard versions are the most popular and whether new guidance should be written, for example.
* Hashed MAC address -- Determine a crypographically (SHA256) anonymous and unique ID for a machine. Useful to determine the aggregate number of machines that use .NET Core.

## Product Findings and Decisions

This data has been very useful to the .NET Core team for a year now. In some cases, like looking at overall usage or at the usage of specific commands, we are very reliant on this data to make decisions. For more specific decisions, like the case of removing the [OpenSSL dependency on macOS](https://github.com/dotnet/announcements/issues/21), we used the data as secondary evidence to user feedback.

Here are some interesting findings that we have made based on this data:

* .NET Core usage is growing -- >10% month over month.
* .NET Core usage is geographically diverse -- used in 100s of countries and all continents.
* .NET Core CLI tools are a very important part of the overall .NET Core experience -- relative to .NET Framework, the CLI tools are novel.
* Developers do not use the .NET Core SDK the same way on Windows, macOS and Linux -- the popular commands are different per OS.
* The publishing model for .NET Core apps is likely confusing some people -- the difference in the popular commands suggests a use of .NET Core that differs from our guidance (more investigation needed).
* We have more work to do to reach out to the Linux and macOS communities -- we would like to see increased use of .NET Core on thoses OSes.
* Our approach to supporting Linux (one build per distro) isn't providing broad enough support -- .NET Core was used on high 10s of Linux distros yet it only works well on 10-20 distros.
* There are gaps in the data that limit our understanding -- we would like to know if the SDK is running in a container, for example.

We made the following changes in .NET Core 2.0 based on this data:

* .NET Core 2.0 ships with a single Linux build, making it easier to use .NET Core on Linux. .NET Core 1.x has nearly a dozen Linux builds for specific distros (for example, RHEL, Debian and Ubuntu are all separate) and limits support to those distros.
* .NET Core 2.0 does not require OpenSSL on macOS, with the intention of increasing adoption on macOS.
* .NET Core 2.0 will be easily buildable from source so that Linux distros can include .NET Core in their package repository/archive/collection. We are talking to distros about that now.
* We will attend and/or encourage local experts to participate in more conferences (globally) to talk about .NET Core.

More forward-looking:

* Fix the build and publishing model for .NET Core -- the differences between `run`, `build` and `publish` are likely confusing people.
* Enable more CLI scenarios -- enable distribution of tools, possibly like the way [NPM does global installs](https://docs.npmjs.com/cli/install).

## Data Insights

The data reveals interesting trends in addition to product development insights. Let's take a look at historical data (since April 2016):

Note: this data is just from direct use of the CLI. There is of course a significant amount of .NET Core usage via Visual Studio, as well.

### Command Variations by Operating System

![Commands by OS](cli-commands-by-os.png)

There are some interesting and surprising differences in command usage between operating systems. We can see that `build` is by far the leading command on Windows, `run` on Linux, and `restore` on macOS. I'd interpret this to say that we're seeing a lot of application development on Windows, maybe more "kicking the tires" applications on macOS scaffolded using Yeoman (since `dotnet new` usage is low), while Linux is primarily being used to host applications.

Note: The chart says "OSX", which is the old name for macOS.

### Weekly Trends

![Weekly cycle](cli-weekly-cycle.png)

You can see that there's an obvious cycle that follows the work week. Looking closer, it's clear that the `build` and `restore` commands drop off quite a bit on the weekend, while the `run` command doesn't quite as much.

We wonder if developers use `build` and `restore` while automation that doesn't take weekends off uses `run`.

### Geographic Distributions

![Geographic Distribution](cli-client-os-geo.png)

It's interesting to take a look at the geographic variations in operating system usage. Most geographies have a mix, but you can see that some areas run predominantly on a single operating system, at least as it relates to .NET Core usage.

This data and visualization is based on the IP address seen on the server. It is not collected by the CLI. The IP address is not stored, but converted to a 3-octet IP address, which is effectively a city-level representation of that data.

### Overall Operating System Distribution

![OS Distribution](cli-os-distribution-by-distinctip.png)

Given .NET's roots, it's not surprising to see a large Windows following. It's exciting to see substantial Linux and Darwin (macOS) usage as well.

### Operating System Version Distribution

![Operating System Version Distribution](cli-os-versions.png)

It looks like .NET Core is running mostly on the newest operating system versions at this point. This aligns with our expecation that .NET Core has been adopted mostly by "early adopters" to this point. In 2-3 years, we expect that the operating system distribution will be more varied.

## More to Come

We will continue to make this data available to you in a timely manner, and we're going to look into making it possible for you to visualize the kinds of trends we're seeing (like in the images above). For now, we're making the raw data available to you.

Thanks to everyone that has been using .NET Core. The community engagement on the project has been amazing and we are making a great product together. This information is helping us make the product better and will become even more useful in the future. We are now doing our part to make the data collected publicly available. This makes good on a statement that we made at the start of the project, that we would release the data. We now look forward to other developers reasoning about this data and using it as part of project decision making.
