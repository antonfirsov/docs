#.NET Networking APIs for UWP Apps

At Build 2015, we announced that .NET Core 5 is the new version of .NET for developers writing Universal Windows Platform (UWP) apps. The set of networking APIs available for developers in .NET Core 5 is an evolution from the set that was available for Windows Store app developers in Windows 8.1 ([API reference on MSDN][msdn-dotnet-store-profile]). As was highlighted at Build, porting your apps to .NET Core and UWP enables you to target a wide variety of devices including Xbox, Windows Phone, Windows and HoloLens with the same codebase. Of course, you can still use all the .NET networking APIs that you used in Windows 8.1 Store apps (no API surface has been removed/deprecated), and more.

Although most of the networking API surface in .NET Core is the same as previous .NET Framework versions, the underlying implementation for some of these APIs has undergone a significant change as we move from the .NET Framework to .NET Core. We have also taken this opportunity to modernize the implementation of our networking APIs and make it more lightweight in terms of memory consumption. In this post, we outline all the .NET networking APIs available to UWP developers and provide some insights into the implementation underneath the APIs. 

Note that all the APIs and changes discussed in this post are applicable only to .NET Core for UWP apps, and not to .NET Framework 4.6. We are also investing in networking API improvements in .NET Core for server platforms (through ASP.NET 5), but we will cover those in a separate blog post. Similarly, this post does not cover networking APIs outside of the .NET framework that are available to Windows apps developers.

##What’s New
These are the new APIs and features that we have added into .NET Core 5 for UWP app developers.
###System.Net.Sockets
With Windows 10 and .NET Core 5, **System.Net.Sockets has been added into the API surface for UWP app developers**. This was a [highly requested API][uservoice-sockets] for Windows Store apps (it was already available for Windows Phone Silverlight apps) and includes types such as System.Net.Sockets.Socket and System.Net.Sockets.SocketAsyncEventArgs, which are used by developers for asynchronous socket communication.
The current API surface of System.Net.Sockets in .NET Core is based on that of Phone 8.1 Silverlight and continues to support most of the types, properties and methods (some APIs that are considered obsolete have been removed). Moving forward, we plan to expand this surface to support more types from System.Net.Sockets namespace – please see the “Looking ahead” section below.
The implementation underneath the System.Net.Sockets API has been significantly rewritten due to the move from the full .NET Framework to .NET Core. Our goal is to ensure functional parity between the previous implementation and the new .NET Core version. Please send us your feedback on [GitHub][github-corefx] if you see any differences in behavior or performance as you port your Sockets code to UWP.

###System.Net.Http gets HTTP/2
Developers writing UWP apps on Windows 10 and .NET Core 5 will get **HTTP/2 support in System.Net.Http.HttpClient**. HTTP/2 is the latest version of the HTTP protocol and provides much lower latency in web access by minimizing the number of connections and round-trip messages. Adding this support into the HttpClient API means that server responses come back much faster, leading to an app that feels more fluid at the same network speed. And the best part is – this feature on by default, so there is zero code change required to leverage this. For more details on how HTTP/2 provides faster web access to apps, see [this talk][channel9-build-talk] from Build 2015. The talk also features a simple photo downloading app that shows approximately 200% improvement in latency upon switching to HTTP/2 ([demo video][channel9-talk-demo])

The following code snippet shows how to query the HTTP version preference on the client as well as the actual HTTP version being used for the connection:

```c#
var myClient = new HttpClient();
var myRequest = new HttpRequestMessage(HttpMethod.Get, "http://www.contoso.com");
Debug.WriteLine(myRequest.Version.ToString()); //this property represents the client preference for the HTTP protocol version. The default value for UWP apps is 2.0.
var response = await myClient.SendAsync(myRequest);
Debug.WriteLine(response.Version.ToString());// this tells if you if the client-server communication is actually using HTTP/2
```

**Notes:**
1. Setting the Request.Version property to 2.0 is not supported on other .NET platforms and will throw a System.ArgumentException when trying to send such a request. The default version on .NET platforms other than UWP is 1.1.
2. The Request.Version property represents the client API preference to use HTTP/2. The actual HTTP version used will depend on the client OS, server and intermediate proxies. HTTP/2 is a negotiated protocol that will automatically fall back to HTTP 1.1 if the server or intermediaries do not support HTTP/2.

##What’s Changed
In this section, we review some APIs that were already available to Windows Store developers but have undergone significant change in underlying implementation. Understanding this change may help you as a developer to gain insight into changes you may see in your code as you port it from a Windows 8.1 Store app to Windows 10 UWP.

###System.Net.Http
In Windows 8.1, the implementation of HttpClient was based on a managed HTTP stack comprising of types such as HttpWebRequest and ServicePointManager. In .NET Core for UWP apps, this has been replaced by a completely new, lightweight wrapper on top of native Windows OS HTTP components such as Windows.Web.Http, which is based on [WinINet][msdn-wininet]. This allows us to leverage all the latest features (e.g. HTTP/2) from the OS and we are able to provide these new features much faster to .NET developers than we previously could. It also helps lower the memory consumption of .NET apps running on Windows 10, thereby giving the user a more fluid experience running multiple apps simultaneously. The available set of APIs from System.Net.Http documented here remains unchanged.
The new implementation has been tested to ensure functional parity with the previous Windows 8.1 implementation so that you, the developer, do not see any differences in API behavior as you port your HTTP client code to UWP. However, if you do see any issues/bugs, please file a bug on GitHub.
System.Net.Requests
This library contains types related to System.Net.HttpWebRequest and System.Net.HttpWebResponse that allow developers to implement the client role of the HTTP protocol. The API surface for .NET Core 5 is the same as that available for Windows 8.1 apps and is very limited compared to the surface in the .NET Framework. This is intentional and we highly encourage switching to the HttpClient API instead – that is where our energy and innovation will be focused going forward. Other parts of .NET Core 5 such as Windows Communication Foundation (WCF) have already migrated to HttpClient in their .NET Core implementation as well, as outlined here.
This library is provided purely for backward compatibility and to unblock usage of .NET libraries that use these older APIs. For .NET Core, the implementation of HttpWebRequest is actually based on HttpClient (reversing the dependency order from .NET Framework). As mentioned above, the reason for this is to avoid usage of the managed .NET HTTP stack in a UWP app context and move towards HttpClient as a single HTTP client role API for .NET developers.
What’s the same
Other types from System.Net and System.Net.NetworkInformation namespaces that were supported for Windows 8.1 Store apps will continue to be supported in Windows 10 UWP. There have been some minor additions to this API surface, but no major changes in implementation.
Looking Ahead
In this post, we discussed the initial version of the set of .NET networking APIs that will be available to Windows 10 UWP developers. We will continue to build on this set and add more API surface to ensure that developers can write rich, full-featured UWP apps using .NET.
To ensure that we prioritize and focus on the right APIs, we need feedback from you – please send us feedback on which APIs are missing in .NET Core and are blocking you from delivering the best possible experience to your users in a UWP app. Please create or vote on an idea on Windows platform missing APIs uservoice or file an issue in  GitHub. We look forward to working with you to deliver awesome apps to the entire breadth of Windows devices.



[msdn-dotnet-store-profile]: https://msdn.microsoft.com/en-us/library/br230232.aspx
[uservoice-sockets]: https://wpdev.uservoice.com/forums/253374-missing-platform-apis/suggestions/6092670-system-net-sockets-and-system-threading-thread-for
[github-corefx]: https://github.com/dotnet/corefx/issues
[channel9-build-talk]: https://channel9.msdn.com/Events/Build/2015/2-83
[channel9-talk-demo]: https://channel9.msdn.com/Events/Build/2015/2-83#time=16m34s
[msdn-wininet]: https://msdn.microsoft.com/en-us/library/windows/desktop/aa383630(v=vs.85).aspx

