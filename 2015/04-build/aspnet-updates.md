ASP.NET Updates
===============

The ASP.NET team has been busy since Preview, with ASP.NET 4.6 and ASP.NET 5. You can see all of the ASP.NET updates in [Visual Studio 2015 RC on the webdev blog](http://blogs.msdn.com/b/webdev). You can also see the updates from the earlier [ASP.NET CTP 6 update](http://blogs.msdn.com/b/webdev/archive/2015/02/23/aspnet-5-updates-for-feb-2015.aspx).

ASP.NET 5 Project
-----------------

The team has a lot of focus on the ASP.NET 5 project. There are some important new updates in the RC described below and in the [webdev blog](http://blogs.msdn.com/b/webdev). At this point, most of the attention is on fit-and-finish, performance and reliability. We've talked to many customers that want to start deploying it on both Windows and Linux. There is also a lot of interest in OS X, particularly with the recently announced Visual Studio Code. It's our goal to make this incredible new ASP.NET scenario available to you as soon as we can.

The team also recently announced [support for Visual Basic](http://blogs.msdn.com/b/webdev/archive/2015/04/24/making-it-better-asp-net-with-visual-basic-14.aspx) in ASP.NET 5. 

Updated New Project Dialog
--------------------------

The addition of ASP.NET 5 as new separate version of ASP.NET motivated the team to re-work the ASP.NET _New ASP.NET Project_ dialog in Visual Studio. ASP.NET 4.6 and ASP.NET 5 are clearly divided, making it easy to choose which type of app you want to build. The ASP.NET 5 section has fewer choices since more of the scenarios are integrated now. For examoke, you can opt to make your Web API a Mobile service at any time. 

![New ASP.NET Project Dialog](aspnet-project-dialog.png)

Missing NuGet Packages - No Longer
----------------------------------

The transition from the monolithic .NET Framework to distributing all of .NET as NuGet packages has a lot of advantages, but has come with some challenges, including package discovery. You can now resolve NuGet package references in a similar way as you can resolve missing namespace references for types. 

In the example below, the XDocument type (just the text `XDocument`) is resolved to its type definition with a simple `CTRL .`. The using statement is added to the file and the System.Xml.XmlDocument NuGet package. 

![Resolve missing NuGet references](aspnet-missing-nuget.png)

We wanted to make it easy to copy some code from [StackOverflow](http://stackoverflow.com/questions/tagged/asp.net), for example, and resolve type and package references quickly and easily. Please tell us if we've achieved that goal.

HTTP/2 Support (Windows 10)
---------------------------

[HTTP/2](http://en.wikipedia.org/wiki/HTTP/2) support has been added to ASP.NET in the .NET Framework 4.6. New features were required in Windows, in IIS and in ASP.NET to enable HTTP/2 given that networking functionality exists at multiple layers. You must be running on Windows 10 to use HTTP/2 with ASP.NET. HTTP/2 has not yet been added to ASP.NET 5.

HTTP/2 is a new version of the HTTP protocol that provides much better connection utilization (fewer round-trips between client and server), resulting in lower latency web page loading for users.  Web pages (as opposed to services) benefit the most from HTTP/2, since the protocol optimizes for multiple artifacts being requested as part of a single experience. 

The browser and the webserver (IIS on Windows) do all the work. You don't have to do any heavy-lifting for your users. You can opt-in to HTTP/2 by doing A, B and C. You can see ASP.NET serving HTTP/2 traffic in the Fiddler screenshot below. 

Screen-shot of HTTP/2 traffic with fiddler.

Most of the [major browsers](http://en.wikipedia.org/wiki/HTTP/2#Browser_support) support HTTP/2, so it's likely that your users will benefit from HTTP/2 support if your server supports it. Give it a try with the RC update.

Support for Token Binding Protocol
----------------------------------

Microsoft and Google have been collaborating on a new approach to authentication, called the [Token Binding Protocol](https://github.com/TokenBinding/Internet-Drafts). The premise is that  authentication tokens (in your browser cache) can be stolen and used by criminals to access otherwise secure resources (e.g. your bank account) without the requirement of your password or any other priviliged knowledge. The new protocol aims to mitigate this problem.

The Token Binding Protocol will be implemented in Windows 10, as a browser feature. ASP.NET apps will participate in the protocol, such that authentication tokens are validated to be legitimate. The client and the server implementations establish the end-to-end protection specified by the protocol.