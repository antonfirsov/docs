The .NET Core tools include a [telemetry feature](https://docs.microsoft.com/en-us/dotnet/articles/core/tools/telemetry)  to allow the .NET team to understand how the tools are being used so they can improve them.
The telemetry feature collects the following pieces of data:
 - The command being used (for example, “build”, “restore”)
 - The ExitCode of the command
 - For test projects, the test runner being used
 - The timestamp of invocation
 - The framework used
 - Whether runtime IDs are present in the “runtimes” node
 - The CLI version being used
As described in the documentation, you're able to opt-out telemetry collection by setting the `DOTNET_CLI_TELEMETRY_OPTOUT` variable.
 
The data collected is anonymous, and we've promised that the it would be published in an aggregated form for use by both Microsoft and community engineers under the Creative Commons Attribution License.
Today, we're making that anonymous data available in CSV format:
 - <a href="https://netcoretelemetrydata.blob.core.windows.net/csv/CLI_2016-04-01.csv">CLI_2016-04-01.csv</a>
 - <a href="https://netcoretelemetrydata.blob.core.windows.net/csv/CLI_2016-05-01.csv">CLI_2016-05-01.csv</a>
 - <a href="https://netcoretelemetrydata.blob.core.windows.net/csv/CLI_2016-06-01.csv">CLI_2016-06-01.csv</a>
 - <a href="https://netcoretelemetrydata.blob.core.windows.net/csv/CLI_2016-07-01.csv">CLI_2016-07-01.csv</a>
 - <a href="https://netcoretelemetrydata.blob.core.windows.net/csv/CLI_2016-08-01.csv">CLI_2016-08-01.csv</a>
 - <a href="https://netcoretelemetrydata.blob.core.windows.net/csv/CLI_2016-09-01.csv">CLI_2016-09-01.csv</a>
 - <a href="https://netcoretelemetrydata.blob.core.windows.net/csv/CLI_2016-10-01.csv">CLI_2016-10-01.csv</a>
 - <a href="https://netcoretelemetrydata.blob.core.windows.net/csv/CLI_2016-11-01.csv">CLI_2016-11-01.csv</a>
 - <a href="https://netcoretelemetrydata.blob.core.windows.net/csv/CLI_2016-12-01.csv">CLI_2016-12-01.csv</a>
 - <a href="https://netcoretelemetrydata.blob.core.windows.net/csv/CLI_2017-01-01.csv">CLI_2017-01-01.csv</a>
 - <a href="https://netcoretelemetrydata.blob.core.windows.net/csv/CLI_2017-02-01.csv">CLI_2017-02-01.csv</a>
 
 
 
 This is a beginning, and a first effort. We will continue to make this data available to you in a timely manner, and we're going to look into making the data more accessible. But we don't want to hold things up, so we're starting by providing you data in raw format.
