---
post_title: Diagnostics improvements in .NET 5
username: soshir@microsoft.com
microsoft_alias: soshir@microsoft.com
categories: .NET Core
desired_publication_date: 1/13/2021
summary: Diagnostics improvements in .NET 5
---

Building upon the [diagnostics improvements we introduced in .NET Core 3.0](https://devblogs.microsoft.com/dotnet/introducing-diagnostics-improvements-in-net-core-3-0/), we've been hard at work further improving this space. I'm excited to introduce the next wave of diagnostics improvements.

### Diagnostics tool are available without the .NET SDK

Until recently, the .NET diagnostics suite of tools was available only as .NET SDK global tools. While this provided a convenient way to acquire and update the tools, this meant it was difficult to acquire them in environments where the full SDK was not present. We now provide a single-file distribution mechanism that only requires a runtime (3.1+) to be available on the target machine.

The latest version of the tools is always available at a link that follows the following schema:

```text
https://aka.ms/<tool-name>/<platform-runtime-identifier>
```

For example, if you're running .NET Core on x64 Ubuntu, you can acquire the `dotnet-trace` from `https://aka.ms/dotnet-trace/linux-x64`.

The list of supported platforms and their download links can be found on the documentation for each of the tools, e.g., [dotnet-counters documentation](https://docs.microsoft.com/dotnet/core/diagnostics/dotnet-counters). The list of all available tools and supported platform runtime identifiers can be found in [the diagnostics repo](https://github.com/dotnet/diagnostics/blob/master/documentation/single-file-tools.md).

### Analyze Linux memory dumps on Windows

Debugging managed code requires special knowledge of managed objects and constructs. The Data Access Component (DAC) is a subset of the runtime execution engine that has knowledge of these constructs and can access these managed objects without a runtime. In .NET Core 3.1.8+ and in .NET 5+, we’ve started to compile the Linux DAC against Windows. .NET Core process dumps collected on Linux can now be analyzed on Windows using WinDBG, `dotnet dump analyze`, and Visual Studio 2019 16.8.

More information on both how to collect .NET memory dumps and how to analyze them can be found on the [Visual Studio blog](https://devblogs.microsoft.com/visualstudio/linux-managed-memory-dump-debugging/).

### Startup tracing

The .NET diagnostics suite of tools work by connecting to the diagnostics port created by the runtime and then requesting the runtime to egress information using the [Diagnostics IPC Protocol](https://github.com/dotnet/diagnostics/blob/master/documentation/design-docs/ipc-protocol.md) over that channel. In .NET Core 3.1, it wasn't possible to perform startup tracing (via EventPipe; ETW was still possible) since events emitted before the tools could connect to the runtime would be lost. In .NET 5, it is now possible to configure the runtime to suspend itself during startup until a tool has connected (or have the runtime connect to the tool).

The 5.0 versions of `dotnet-counters` and `dotnet-trace` can now launch dotnet processes and collect diagnostics information from the process start. For example, the following command will start `mydotnetapp.exe` and begin monitoring counters.

```bash
dotnet counters monitor -- mydotnetapp.exe
```

More information on startup tracing can be found on documentation pages for [dotnet-counters](https://docs.microsoft.com/dotnet/core/diagnostics/dotnet-counters) and [dotnet-trace](https://docs.microsoft.com/dotnet/core/diagnostics/dotnet-trace).

### Assembly load diagnostics

in .NET 5, the runtime now emits events for assembly binds via `EventPipe`. This information can help you diagnose why the runtime cannot locate an assembly at runtime. This is the replacement for the Fusion Log Viewer ([fuslogvw.exe](https://docs.microsoft.com/dotnet/framework/tools/fuslogvw-exe-assembly-binding-log-viewer)) present in the .NET Framework.

You can use the following command to collect assembly load diagnostics:

```bash
dotnet-trace collect --providers Microsoft-Windows-DotNETRuntime:4:4 --process-id [process ID]
```

The resulting `.nettrace` file can be analyzed using [PerfView](https://github.com/microsoft/perfview).

## Closing

Thanks for trying out the updated diagnostics tools in .NET 5. Please continue to give us feedback, either in the comments or on [GitHub](https://github.com/dotnet/diagnostics). We are listening carefully and will continue to make changes based on your feedback. We have more upcoming changes to improve the diagnostics tools in .NET 5 that will be covered in a follow-up blog post.
