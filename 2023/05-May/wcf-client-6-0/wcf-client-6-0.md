---
post_title: Client Support for Calling WCF/CoreWCF with System.ServiceModel 6.0 Is Here! 
author1: samsp@microsoft.com
post_slug: wcf-client-60-has-been-released
username: samsp@microsoft.com
microsoft_alias: samsp
featured_image: CoreWCFImage.png
categories: .NET, .NET Core, Support, WCF
tags: System.ServiceModel, WCF, CoreWCF, WCF Client
summary: The System.ServiceModel 6.0 packages have been released, they provide client suppport for calling into WCF and CoreWCF Services.
desired_publication_date: 2023-05-16
post_date: 2023-05-18 10:05:00
---

`System.ServiceModel.*` are a set of NuGet packages that provide the client functionality for calling into WCF or CoreWCF services from .NET applications. These packages, collectively known as the WCF Client, are developed and supported by Microsoft, and open-sourced at [https://github.com/dotnet/wcf](https://github.com/dotnet/wcf) 

Windows Communication Services (WCF) is a service communication framework from Microsoft, that supports SOAP and the WS-* protocol specifications, and was originally released as part of .NET 3.0 in 2006.

.NET Core 3.1 & .NET 5, 6, 7 & 8 (currently in preview) do not include WCF server support in the base SDK. A separate community project, [CoreWCF](https://github.com/corewcf/corewcf), provides a WCF-compatible server implementation, based on top of ASP.NET Core.

## Packages in this release

This 6.0 release includes the following packages:

| Name | Description |
| --- | --- |
| [System.ServiceModel.Primitives](https://www.nuget.org/packages/System.ServiceModel.Primitives) | Provides the common types used by all of the WCF libraries |
| [System.ServiceModel.Http](https://www.nuget.org/packages/System.ServiceModel.Http) | Provides the types that permit SOAP messages to be exchanged using Http (example: `BasicHttpBinding`) |
| [System.ServiceModel.NetTcp](https://www.nuget.org/packages/System.ServiceModel.NetTCP) | Provides the types that permit SOAP messages to be exchanged using TCP (example: `NetTcpBinding`) |
| [System.ServiceModel.Federation](https://www.nuget.org/packages/System.ServiceModel.Federation) | Provides the types that allow security communications with SOAP messages using WS-Federation |
| [System.ServiceModel.NetFramingBase](https://www.nuget.org/packages/System.ServiceModel.NetFramingBase) | Contain common types for NetFraming based transports such as `NetTcp` and `NetNamedPipe` |
| [System.ServiceModel.NetNamedPipe](https://www.nuget.org/packages/System.ServiceModel.NetNamedPipe) | Provides the types that permit SOAP messages to be exchanged using named pipes (example: `NetNamedPipeBinding`)|
| [System.Web.Services.Description](https://www.nuget.org/packages/System.Web.Services.Description) | Contains classes that enable you to publicly describe an XML Web service by using the Web Services Description Language (WSDL)|
| [System.ServiceModel.Security](https://www.nuget.org/packages/System.ServiceModel.Security) | [Deprecated](#deprecating-the-systemservicemodelduplex-and-systemservicemodelsecurity-packages). Provides the types that support additional security features  |
| [System.ServiceModel.Duplex](https://www.nuget.org/packages/System.ServiceModel.Duplex) | [Deprecated](#deprecating-the-systemservicemodelduplex-and-systemservicemodelsecurity-packages). Provides the types that permit 2-way ("duplex") exchanges of messages  |

## NetNamedPipe support has been added

Support for using named pipes is new to this release. It works for named pipes implemented by WCF and CoreWCF. `NetNamedPipeBinding` is a binding for enabling fast binary communication between processes on the same machine. 

Named Pipes is a feature of the Windows OS, so is not available on Linux or other non-windows platforms.

## Dropping .NET Standard and older .NET support

This WCF Client 6.0 package drops support for .NET Standard 2.0, and is only targetting .NET 6.0 and above. The reason for this change is so that we can take advantage of newer functionality included in .NET 6, which is not available for .NET Framework. This change reduces the size and complexity that was required for implementing support for both .NET and .NET Framework.

Targeting .NET standard enabled the same package to be used from either .NET Framwork or .NET applications. Applications and libraries that need .NET Standard support, should either:
- Continue to reference the version 4.x System.ServiceModel.* packages
- Or use a conditional assembly reference to System.ServiceModel.dll for .NET Framework and use these nuget packages for .NET 6 and above.   

## Deprecating the System.ServiceModel.Duplex and System.ServiceModel.Security packages

These packages are no longer required as the types have been merged into the `System.ServiceModel.Primitives` package. This 6.0 version of `System.ServiceModel.Duplex` and `System.ServiceModel.Security` now just includes type forwarders to the implementations in the `System.ServiceModel.Primitives` package.

This will be the final release of these two packages, as the type forwarders will always point to the version of the `Primitives` package referenced by the application.

## Versioning pattern

To make it easier to understand the versioning we will be following a new versioning pattern for this package where the library will be targetted against an LTS version of .NET, and will match the support lifecycle for that LTS release. This release is versioned as 6.0 as it targets the .NET 6 LTS. 

Support will be based on a *major*.*minor* versioning scheme:
- The major number will follow the same numbering as the .NET LTS versions, and will follow the same support lifecycle as the corresponding .NET LTS. 
  - Eg WCF Client 6.x will be supported for the same duration as .NET 6
- Breaking changes, and dependent runtime requirements will only be made as part of a new major version train.
  - For example the changes in this release to only support .NET 6 and above were only possible because of the major version change. No futher runtime dependency changes will be made as part of the 6.x version train.
- There may be multiple minor releases under each major number, eg 6.0, 6.1, 6.2 etc. 
  - Minor releases with the same major version will be API and behavior compatible with the previous minor releases.
  - When new minor versions are released, support for the previous minor release(s) will be for six months after their replacement date.
- Support will be primarily for the latest major.minor release of each supported major version.
  - When WCF Client 8.0 ships, support periods will overlap for both 6.x and 8.0. WCF Client 6.x support will continue thru Nov 2024, the same as .NET 6. 
- Security fixes will be released for all supported versions. 
  - During the six months overlap period of a previous release and new minor release, any security fix release will include both versions.  

