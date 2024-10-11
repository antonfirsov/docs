---
post_title: .NET 9 Networking Improvements
author1: mapichov
author2: knatalia
post_slug: dotnet-9-networking-improvements
microsoft_alias: mapichov
featured_image: dotnet9-networking-improvements.png
categories: .NET, Networking
tags: .net 9, http, http-client-factory, net-security
summary: Introducing new networking features in .NET 9 including HTTP space, HttpClientFactory, security and more!
post_date: 2024-12-05 10:05:00
---

As in the previous years, we publish a blog post about new interesting changes in networking space with new [.NET release](https://devblogs.microsoft.com/dotnet/announcing-dotnet-9/). This year, we're introducing changes in [HTTP](#http) space, new [`HttpClientFactory`](#httpclientfactory) APIs, .NET framework compatibility improvements and more.


// Mana
## HTTP
- multiple H/3 connections
- APIs with cancellation token (https://github.com/dotnet/runtime/pull/103991 community)
- The H1 connection pool contention change (https://github.com/dotnet/runtime/pull/99364)
- analyzer (https://github.com/dotnet/runtime/issues/75137 https://github.com/dotnet/roslyn-analyzers/pull/6796 community)
- proxy refresh (https://github.com/dotnet/runtime/pull/103364)

// Natalia
## WebSockets
- https://github.com/dotnet/runtime/pull/105841

// Mana
## .NET Framework Compatibility
- HttpWebRequest (https://github.com/dotnet/runtime/pull/95001, https://github.com/dotnet/runtime/pull/94664, https://github.com/dotnet/runtime/pull/97537)
- WebRequest impersonation (https://github.com/dotnet/runtime/pull/102038)
- ServicePoint obsoletion (https://github.com/dotnet/runtime/pull/103456)
- AuthenticationManager obsoletion (https://github.com/dotnet/runtime/pull/93171) community

// Anton
## Metrics / Diagnostics / Redaction
- https://github.com/dotnet/runtime/pull/103922
- ...

// Natalia
## HttpClientFactory

// Mana
## QUIC
- public API surface (https://github.com/dotnet/runtime/pull/104227)
- APIs for multiple connections (https://github.com/dotnet/runtime/pull/101531)
- new connection options (https://github.com/dotnet/runtime/pull/94211)
- connection TLS details (https://github.com/dotnet/runtime/pull/84976, https://github.com/dotnet/runtime/pull/106391)
- perf callback in TP thread (https://github.com/dotnet/runtime/pull/98361)
- perf configuration cache (https://github.com/dotnet/runtime/pull/99371)

// Mana
## Security

### SSLKEYLOGFILE Support
// Needs update: written last year, no need for Debug build, but we have AppContext switch instead, also works with QUIC.

The most upvoted issue in the security space was to support logging of pre-master secret ([dotnet/runtime#37915](https://github.com/dotnet/runtime/issues/37915)). The logged secret can be used by packet capturing tool Wireshark to decrypt the traffic. It's useful diagnostics tool when investigation networking issues. Moreover, the same functionality is provided by browsers like Firefox (via [NSS](https://udn.realityripple.com/docs/Mozilla/Projects/NSS/Key_Log_Format)) and Chrome and command line HTTP tools like [cURL](https://everything.curl.dev/usingcurl/tls/sslkeylogfile).

For now, we have implemented this feature for platforms on which we use OpenSSL as a security backend (in terms of officially released .NET runtime it means Linux). We track this feature for Windows in [dotnet/runtime#94843](https://github.com/dotnet/runtime/issues/94843) and we encourage upvoting the issue to let us know we should invest in doing the same for SChannel (Windows).

As this is rather advanced scenario with security implications, we decided to support it only in DEBUG builds of `System.Net.Security`. We do not officially ship DEBUG builds of .NET libraries anywhere. If you want to take advantage of this feature, you have to build it yourself. The documentation how to do that is in our [repository](https://github.com/dotnet/runtime/blob/main/docs/workflow/README.md). Note that corresponding branch of source code needs to be used, meaning [release/8.0](https://github.com/dotnet/runtime/tree/release/8.0) for .NET 8.

When you have the DEBUG build ready, you can publish your project as self contained:

```sh
dotnet publish --runtime linux-x64 --self-contained
```

Then, replace `System.Net.Security.dll` in the publish directory with the one you've built:

```sh
cp <runtime-repo>/artifacts/bin/System.Net.Security/Debug/net8.0-linux/* <project-repo>/bin/Release/net8.0/linux-x64/publish/

```

The last thing is to set up an environmental variable `SSLKEYLOGFILE` and run your program:

```sh
export SSLKEYLOGFILE=~/keylogfile

./<your-program>
```

At this point, `~/keylogfile` will contain pre-master secrets that can be used by Wireshark to decrypt the traffic, see [TLS Using the (Pre)-Master-Secret](https://wiki.wireshark.org/TLS#using-the-pre-master-secret) docs about how to configure it.

https://github.com/dotnet/runtime/pull/100665

- TLS Resume on Linux (https://github.com/dotnet/runtime/pull/102656)
- IntegrityCheck APIs (https://github.com/dotnet/runtime/pull/96712) - Filip community

// Mana
## Networking Primitives
- IEquatable on Uri (https://github.com/dotnet/runtime/pull/103511)
- More MediaTypeNames (https://github.com/dotnet/runtime/pull/103575)
- SSE parser as OOB (https://github.com/dotnet/runtime/pull/102238)
- Uri’s got span-based zero-alloc Escape/UnescapeDataString APIs (https://github.com/dotnet/runtime/pull/98074)
- new media types (https://github.com/dotnet/runtime/pull/103575)
