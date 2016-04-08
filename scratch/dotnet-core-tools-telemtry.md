.NET Core Tools Telemetry
=========================

We will ship the new .NET Core command-line tools experience (AKA ".NET CLI") with .NET Core RC2. We believe that we've got a good design and experience in place, but don't have any usage data that proves that. Similarly, we have plenty of ideas on what to do next with the .NET Core Tools, but are again lacking data on how to priortize those ideas. 

We just [added a telemetry feature](https://github.com/dotnet/cli/pull/2145) to the tools to produce more insight on usage and make decision making more data-oriented. The data generated from this feature will be aggregated and anonymized and published for use by both Microsoft and community engineers.

Behavior
--------

The telemetry feature is on by default for the .NET Core Tools for RC2. We will re-assess if that's the right behavior for RTM, based on your feedback.

You can opt-out of the telemetry feature by setting an environment variable DOTNET_CLI_TELEMETRY_OPTOUT. Doing this will stop the collection process from running. In order to set the environment variable, please use the existing operating system mechanisms for this (e.g. `export` on OS X/Linux, `set` on Windows). We may also add a different opt-out for RTM, again based on your feedback.

Data Points
-----------

The feature collects the following pieces of data:

- The command being used (e.g. "build", "restore")
- ExitCode of the command
- For test projects, the test runner being used
- Timestamp of invocation
- Framework used
- If RIDs are present in the "runtimes" node
- The CLI version being used

The feature will not collect any personal data, such as usernames or emails or anything that can be used to identify the actual user. It will not scan your code and  not extract any project-level data that can be considered sensitive, such as name, repo or author (if you set those in your project.json). We want to now how the tools are used, not what you are using the tools to build. If you find sensitive data being collected, that's a bug. Please file an issue and it will be fixed.

EULA
----

With commercial software, there is a single relationship that needs to be defined: the one between the software vendor and the user. Microsoft's commercial distribution of .NET Core has an End User License Agreement (EULA), defining that relationship. We use the [MICROSOFT .NET LIBRARY EULA](http://go.microsoft.com/fwlink/?LinkId=329770) for the .NET Core Tools, which we also use for all .NET NuGet packages. We recently added a "DATA" section re-printed below, to enable us to collect telemetry from the tools. We want to stay with one EULA for .NET Core and only intend to collect data from the tools, not the runtime or libraries.

> DATA. The software may collect information about you and your use of the software, and send that to Microsoft. Microsoft may use this information to improve our products and services. You can learn more about data collection and use in the help documentation and the privacy statement at http://go.microsoft.com/fwlink/?LinkId=528096. Your use of the software operates as your consent to these practices.

The EULA is different from the [MIT](https://github.com/dotnet/coreclr/blob/master/LICENSE.TXT) open source software license we use. The MIT license explains what you can do with the source code, while the EULA describes a two-way relationship between the software user and the software vendor.

.NET Core is open source, so adding a telemetry feature and collecting data has a different dynamic than closed source commercial software. That's because there is a second relationship that exists with open source software, between the maintainers and the community. To make good on that relationship, we will share the data we collect in an annonymized and aggegated form. It's important to share the data for two reasons: as an open source project, you deserve to see the data too, and; the data will give everyone the same insight for project decision making.

We have not yet built infastructure for sharing the data. That's our next step. We'll make that software open source so that you can see how the data is extracted. You can also look at the [initial PR](https://github.com/dotnet/cli/pull/2145) that added the telemetry feature to see how the data is collected and uploaded to Azure storage.

Improving Software with Data
----------------------------

Microsoft teams have used product usage data for a long time for Microsoft products. This is  common practice for commercial software products. It's amazing how different specific product usage can be than your intuition. For .NET Core, we've built this first version to power ASP.NET Core on multiple operating systems. While downloads of the various operating systems might end up being the same, usage across them might be very different. We'd want to understand that and then determine if there was some experience that needed to be improved on the lower usage OSes. That's all speculation but provides insight into the type of questions we might ask.

We are using .NET Core RC2 as an opportunity to validate and get feedback on our approach with product telemetry. Being transparent with our telemetry plans and with the data we collect is an important part of that. We'd like to hear about what you think about our approach to telemetry. Certainly once the data starts coming in and we've found a good way to publish it, I suspect that we'll have an opportunity for more interesting conversations on what the data means.
