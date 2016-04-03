.NET Core Tools Telemetry
=========================

We will ship the new .NET Core command-line tools experience (AKA ".NET CLI") with .NET Core RC2. We believe that we've got a good design and experience in place, but don't have any usage data that proves that. Similarly, we have plenty of ideas on what to do next with the .NET Core Tools, but are again lacking data on how to priortize those ideas. We are in the process of adding a telemetry feature to the tools to produce more insight on usage and make decision making more data-oriented.

Improving Software with Data
----------------------------

Microsoft teams have used usage data for a long time. This is very common practice for commercial software products. It's amazing how different usage can be than your intuition. You can build a great product for carpenters but it ends up being primary used by dress makers. Usage data gives you a view into the reality of how a product is used, enabling you to make it even better for the dress makers and then drill into why the carpenters were not well-served by the product.

With commercial software, there is a single relationship that needs to be defined: the one between the software vendor and the user. Microsoft's commercial distribution of .NET Core has an End User License Agreement (EULA), defining that relationship. We use the [MICROSOFT .NET LIBRARY EULA](http://go.microsoft.com/fwlink/?LinkId=329770) for the .NET Core Tools, which we also use for all .NET NuGet packages. We recently added a "DATA" section re-printed below, to enable us to collect telemetry from the tools. We only intend to collect data from the tools, not the runtime or libraries.


> 2. DATA. The software may collect information about you and your use of the software, and send that to Microsoft. Microsoft may use this information to improve our products and services. You can learn more about data collection and use in the help documentation and the privacy statement at http://go.microsoft.com/fwlink/?LinkId=528096.Your use of the software operates as your consent to these practices.

Sharing the Data
----------------

.NET Core is open source, so adding a telemetry feature and collecting data has a different dynamic than closed source commercial software. That's because there is a second relationship that exists with open source software, between the maintainers and the community. To make good on that relationship, we will share the data we collect in an aggegated form. It's important to share the data for two reasons: as an open source project, you deserve to see the data too, and; the data will give everyone the same insight for project decision making.

We will store the data with Azure Storage. We have not yet built infastructure for sharing the data. That's our next step. We'll make that software open source so that you can see what it is doing.

Scoping the Data Collected
--------------------------

The data collected will not have any personal data, such as usernames or emails or anything that can be used to identify the actual user. We will also not scan your code and we will not extract any project-level data that can be considered sensitive, such as name, repo or author (if you set those in your project.json). We want to now how the tools are used, not what you are using the tools to build. If you find sensitive data being collected, that's a bug. Please file an issue and it will be fixed.

Behavior
--------

The telemetry tracking is "on" by default for the .NET Core Tools for RC2. We will re-assess if that's the right behavior for RTM, largely based on feedback.

You can opt-out of the telemetry feature by setting an environment variable DOTNET_CLI_TELEMETRY_OPTOUT. Doing this will stop the collection process from running. In order to set the environment variable, please use the existing operating system mechanisms for this (e.g. `export` on OS X/Linux, `set` on Windows).

Data Points
-----------

We collect the following pieces of data:

- The command being used (e.g. "build", "restore")
- Arguments passed to the command
- ExitCode of the command
- For test projects, the test runner being used
- Timestamp of invocation
- Details about the project commands are invoked on
- Framework used
- If RIDs are present in the "runtimes" node
- The CLI version being used

Questions, feedback, comments
-----------------------------

Text here

