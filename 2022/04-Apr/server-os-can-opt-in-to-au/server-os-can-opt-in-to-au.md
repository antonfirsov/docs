---
post_title: Server operating systems can opt in to get automatic updates for .NET
username: jamshedd@microsoft.com
microsoft_alias: jamshedd
featured_image: dotnet-bot_handybot.png
categories: .NET
summary: Server operating systems can opt in to get automatic updates for .NET
desired_publication_date: 2022-04-12
---

We’re excited to announce that starting April 2022, we will be making monthly updates for modern .NET (.NET Core) available for server operating systems via Microsoft Update (MU) on an opt-in basis.  

If you do not want to have your servers updated automatically for you __*no action*__ is required. If on the other hand you do want to leverage this for your servers review continue reading below. 

There is no change for client operating systems which will continue to receive updates via Automatic Updates, WSUS, and MU Catalog as earlier.

## More Information
Updates for supported versions of .NET Core 3.1, .NET 5.0 and .NET 6.0 are currently offered to server operating systems via Windows Server Update Services (WSUS) and Microsoft Update Catalog but not the Automatic Updates (AU) channel.  You can now get your server operating system automatically updated by opting in for this behavior. 

### Opting in

You can opt in for automatic updates by setting one or more of these registry keys on your server. 

| .NET Version | Registry Key | Name | Value |
| -------------- | :--------- | :---------- | :---------- |
| Allow All .NET Updates | \[HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\.NET\] | "AllowAUOnServerOS" | dword:00000001 |
| Allow .NET 6.0 Updates | \[HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\.NET\\6.0\] | "AllowAUOnServerOS" | dword:00000001 |
| Allow .NET 5.0 Updates | \[HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\.NET\\5.0\] | "AllowAUOnServerOS" | dword:00000001 |
| Allow .NET 3.1 Updates | \[HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\.NET\\3.1\] | "AllowAUOnServerOS" | dword:00000001 |


Setting the registry key can be achieved by adding the entry (this is an example for enabling 6.0 updates) in a “.reg” file and running this.

```csharp
\[HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\.NET\\6.0\] "AllowAUOnServerOS"=dword:00000001
```

Alternatively, you can use [Group Policy](https://docs.microsoft.com/troubleshoot/windows-server/group-policy/group-policy-overview) to deploy the registry key to many computers at once.


### Why are we making this change?

From the [first time we started shipping updates for modern .NET via Microsoft Update](https://devblogs.microsoft.com/dotnet/net-core-updates-coming-to-microsoft-update/) we have continued to refine this delivery channel including the recent addition of updates for the Hosting Bundle. The vast majority of customers use servers in a managed environment and deployments are handled using a management tool such as Microsoft Intune, Microsoft Endpoint Manager (MEP), System Center Config Manager (SCCM) or Windows Server Update Services (WSUS) and these customers prefer their servers are not directly patched outside of this management environment so they can exercise control over scheduling potential service downtime, reboots, etc. So when we started offering updates via MU we excluded Automatic Updates (AU).

A small number of customers have told us they don't use a deployment management tool and would like to leverage AU to update their servers similar to clients. We believe the opt in approach we’re rolling out today will allow these customers to get the benefit of AU for their server operating systems without impacting the larger set of customers that do not want this.


## In Closing
We’re excited to start delivering updates for modern .NET to server operating systems via Microsoft Update on an opt-in basis and look forward to your feedback on this experience. If you do not want to have your server operating systems updated automatically for you __*no action*__ is required. If on the other hand you do want to leverage this for your servers review the guidance above.
