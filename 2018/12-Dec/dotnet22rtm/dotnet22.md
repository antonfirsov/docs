# Announcing .NET Core 2.2

We’re excited to announce the release of .NET Core 2.2. It includes diagnostic improvements to the runtime, support for ARM32 for Windows and Azure Active Directory for SQL Client. The biggest improvements in this release are in ASP.NET Core.

ASP.NET Core 2.2 and Entity Framework Core 2.2 are also releasing today.

You can download and get started with .NET Core 2.2, on Windows, macOS, and Linux:

* .NET Core 2.2 SDK (includes the runtime)
* .NET Core 2.2 Runtime

.NET Core 2.2 is supported by Visual Studio 15.8, Visual Studio for Mac and Visual Studio Code.

Docker images are available at [microsoft/dotnet](https://hub.docker.com/r/microsoft/dotnet/) for .NET Core and ASP.NET Core.

You can see complete details of the release in the [.NET Core 2.2 release notes](https://github.com/dotnet/core/blob/master/release-notes/2.2/2.2.0.md). Related instructions, known issues, and workarounds are included in the release notes. Please report any issues you find in the comments or at [dotnet/core #1614](https://github.com/dotnet/core/issues/1614).

Thanks for everyone that contributed to .NET Core 2.2. You’ve helped make .NET Core a better product!

# Tiered Compilation
Tiered compilation is a feature that enables the runtime to more adaptively use the Just-In-Time (JIT) compiler to get better performance, both at startup and to maximize throughput. It was added as an opt-in feature in .NET Core 2.1 and then was enabled by default in .NET Core 2.2 Preview 2. We decided that we were not quite ready to enable it by default in the final .NET Core 2.2 release, so we switched it back to opt-in, just like .NET Core 2.1. It is enabled by default in .NET Core 3.0 and we expect it to stay in that configuration.

## Runtime Events

It is often desirable to monitor runtime services such as the GC, JIT, and ThreadPool of the current process to understand how these services are behaving while running your application.  On Windows systems, this is commonly done using ETW and monitoring the ETW events of the current process.  While this continues to work well, it is not always easy or possible to use ETW.  Whether you’re running in a low-privilege environment or running on Linux or macOS, it may not be possible to use ETW.  

Starting with .NET Core 2.2, CoreCLR events can now be consumed using the EventListener class. These events describe the behavior of GC, JIT, ThreadPool, and interop. They are the same events that are exposed as part of the CoreCLR ETW provider on Windows. This allows for applications to consume these events or use a transport mechanism to send them to a telemetry aggregation service. 

You can see how to subscribe to events in the following code sample:

```csharp
 internal sealed class SimpleEventListener : EventListener
    {
        // Called whenever an EventSource is created.
        protected override void OnEventSourceCreated(EventSource eventSource)
        {
            // Watch for the .NET runtime EventSource and enable all of its events.
            if (eventSource.Name.Equals("Microsoft-Windows-DotNETRuntime"))
            {
                    EnableEvents(eventSource, EventLevel.Verbose, (EventKeywords)(-1));
            }
        }

        // Called whenever an event is written.
        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            // Write the contents of the event to the console.
            Console.WriteLine($"ThreadID = {eventData.OSThreadId} ID = {eventData.EventId} Name = {eventData.EventName}");
            for (int i = 0; i < eventData.Payload.Count; i++)
            {
                string payloadString = eventData.Payload[i] != null ? eventData.Payload[i].ToString() : string.Empty;
                Console.WriteLine($"\tName = \"{eventData.PayloadNames[i]}\" Value = \"{payloadString}\"");
            }
            Console.WriteLine("\n");
        }
    }
```

## Support for AccessToken in SqlConnection
The ADO.NET provider for SQL Server, SqlClient, now supports setting the AccessToken property to authenticate SQL Server connections using Azure Active Directory. In order to use the feature, you can obtain the access token value using Active Directory Authentication Library for .NET, contained in the Microsoft.IdentityModel.Clients.ActiveDirectory NuGet package.
 
The following sample shows how to authenticate SQL Server connections using Azure Active directory:
 
```csharp
// get access token using ADAL.NET
var authContext = new AuthenticationContext(authority);
var authResult = await authContext.AcquireTokenAsync(appUri, clientCredential);
// setup connection to SQL Server
var sqlConnection = new SqlConnection(connectionString);
sqlConnection.AccessToken = authResult.AccessToken;
await sqlConnection.OpenAsync();
```
 
For more information see [ADAL.NET]( https://github.com/AzureAD/azure-activedirectory-library-for-dotnet/wiki) and the [Azure Active Directory documentation](https://docs.microsoft.com/azure/active-directory/develop/).

## Injecting code prior to Main
.NET Core now enables injecting code prior to running an application main method via a Startup Hook. Startup hooks make it possible for a host to customize the behavior of applications after they have been deployed, without needing to recompile or change the application.

We expect hosting providers to define custom configuration and policy, including settings that potentially influence load behavior of the main entry point such as the AssemblyLoadContext behavior. The hook could be used to set up tracing or telemetry injection, to set up callbacks for handling, or other environment-dependent behavior. The hook is separate from the entry point, so that user code doesn't need to be modified.

See [Host startup hook](https://github.com/dotnet/core-setup/blob/master/Documentation/design-docs/host-startup-hook.md) for more information.

## Windows ARM32

We are adding support for Windows ARM32, similar to the Linux ARM32 support we added in .NET Core 2.1. Windows has had support for ARM32 with [Windows IoT Core](https://developer.microsoft.com/windows/iot) for some time. As part of the Windows Server 2019 release, ARM32 support was also added for Nanoserver. .NET Core can be used on both Nanoserver and IoT Core.

Docker will be provided for Nanoserver for ARM32 at [microsoft/dotnet](https://hub.docker.com/r/microsoft/dotnet/) on Docker Hub.

We ran into a late bug that prevented us from publishing .NET Core builds for Windows ARM32 today. We expect those builds to be in place for .NET Core 2.2.1, in January 2019.

## Platform Support

.NET Core 2.2 is supported on the following operating systems:

* Windows Client: 7, 8.1, 10 (1607+)
* Windows Server: 2008 R2 SP1+
* macOS: 10.12+
* RHEL: 6+
* Fedora: 26+
* Ubuntu: 16.04+
* Debian: 9+
* SLES: 12+
* openSUSE: 42.3+
* Alpine: 3.7+

Chip support follows:

* x64 on Windows, macOS, and Linux
* x86 on Windows
* ARM32 on Linux (Ubuntu 16.04+, Debian 9+)
* ARM32 on Windows (1809+; available in January)

# Closing

.NET Core 2.2 includes key improvements for the product. Please try them out and tell us what you think. Also make sure to check out the improvements in ASP.NET Core 2.2 and Entity Framework 2.2.