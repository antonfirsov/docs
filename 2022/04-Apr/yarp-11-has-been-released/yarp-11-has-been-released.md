---
post_title: YARP 1.1 is here with new requested reverse proxy features 
username: samsp@microsoft.com
microsoft_alias: samsp
featured_image: dotnet-bot_delivery-bot.png
categories: .NET, .NET Core, ASP.NET Core, Networking
summary: YARP 1.1 has been released. YARP is a highly customizable open-source reverse proxy written for .NET. This release adds a few commonly requested features.
desired_publication_date: 2022-04-28
---

We are pleased to announce that YARP 1.1 has been released to [nuget.org](https://www.nuget.org/packages/Yarp.ReverseProxy). YARP 1.1 is an incremental release, adding some commonly requested features since the 1.0 release in November.

## What is YARP?

YARP is an open source project to create a highly customizable reverse proxy built on the Microsoft .NET platform. To learn more, see the GitHub repo at https://github.com/microsoft/reverse-proxy or the [1.0 Announcement post](https://devblogs.microsoft.com/dotnet/announcing-yarp-1-0-release/).

## What's new in 1.1?

- [Zero Byte Reads](https://github.com/microsoft/reverse-proxy/issues/1325) - this is a perf optimization which is especially valuable for proxying web socket and gRPC streams. A zero byte read is used to detect if data is waiting on the stream, and only then is a memory buffer allocated for the reads. This improves the memory scalability when handling large numbers of streaming requests which may be idle.

- [Multi-value header matching](https://microsoft.github.io/reverse-proxy/articles/header-routing.html#scenario-2---multiple-values) - Header matching rules for routes now support headers with multiple values, or multiple instances of the same header name.

- [HTTP/3 Support](https://microsoft.github.io/reverse-proxy/articles/http3.html) - On .NET 6, YARP supports inbound and outbound requests using HTTP/3. This is dependent on enabling the HTTP/3 functionality for .NET 6, which is a [preview feature](https://devblogs.microsoft.com/dotnet/http-3-support-in-dotnet-6/).

- [Multiple configuration sources](https://microsoft.github.io/reverse-proxy/articles/config-files.html) - YARP configuration can now be loaded from multiple sources, merging multiple route and cluster lists together. The sources can be a mix of config files and/or code-based providers, providing more flexibility with how dynamic configuration is handled.

- [Http.Sys delegation](https://microsoft.github.io/reverse-proxy/articles/httpsys-delegation.html) - If YARP is hosted by Http.sys, then it can route requests to other processes on the same machine also using Http.Sys. 

- [APIs for Middleware](https://microsoft.github.io/reverse-proxy/articles/ab-testing.html) - Added APIs for middleware giving them real-time access to clusters and routes, and the ability to modify the cluster after routing has already run. These are the key building blocks for creating custom A/B testing and rolling upgrade systems.

- [Documentation](https://microsoft.github.io/reverse-proxy/articles/getting-started.html) - In addition to documentation for the features above, articles have been added for [configuration filters](https://microsoft.github.io/reverse-proxy/articles/config-filters.html), [Lets Encrypt](https://microsoft.github.io/reverse-proxy/articles/lets-encrypt.html) and [Web Socket support](https://microsoft.github.io/reverse-proxy/articles/websockets.html). All the docs have been edited with a view to improving them for those not intimately familiar with the details of ASP.NET Core.

## Contributors

In addition to the contributions from the YARP team members, we are very grateful to have received a number of PRs and issues from the community. Thank you to those who contributed PRs to make this release happen – @dpbevin, @specialforest, @kahbazi, @stanvanrooy, @NGloreous, @ericmutta, @Steve-Fenton, @illay1994, 
@macsux, @rwkarg, @horse315, @kkbruce, @mmitche, @damienbod, @tomaustin700, @Henfau, @jerry-shao.

## Closing

Over the next couple of weeks we will be planning the feature set for the next YARP release. One of the big areas of investment is Kubernetes integration being led by @dpbevin. 

We really value the feedback and questions in the GitHub [issues](https://github.com/microsoft/reverse-proxy/issues) and [discussions](https://github.com/microsoft/reverse-proxy/discussions), so please keep them coming.
