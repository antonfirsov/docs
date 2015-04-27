ASP.NET Updates
===============

The ASP.NET team has been busy since Preview, with ASP.NET 4.6 and ASP.NET 5.

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

You can opt into this new feature by doing A, B and C.