# .NET Framework February 2018 Security and Quality Rollup

Today, we are releasing the February 2018 Security and Quality Rollup. It includes the same quality improvements that were part of the [January 2018 Preview of Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2018/01/26/january-2018-preview-of-quality-rollup/).

## Security

No new security fixes. See [.NET Framework January 2018 Security and Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2018/01/09/net-framework-january-2018-security-and-quality-rollup/) for the latest security updates.

## Quality and Reliability

This release contains the following quality and reliability improvements.

### ASP.NET

* Behavior change of HttpCookieCollection.Set, in .NET Framework 4.5.2 and later. If the app uses Set-Cookie header and also use Cookies.Set to add new cookie, the cookie set via Set-Cookie will not be in the response. [513614]

### Identity

* UserPrincipal.GetAuthorizationGroups does not use Kerberos transition when it should, and LDAP operation fails if a group has an SID in its sidHistory, for .NET Framework 4.6.2. [484146]

### SQL

* SqlConnection can hang in Close/Dispose after getting network exception during SqlBulkCopy writes, for .NET Framework 4.6 and later. [523503]

### WCF

* Deadlock between SharedTcpTransportManager.OnClose and OnReceiveComplete causing hang. [454558]

Note: Additional information on these improvements is not available. The [VSTS](https://www.visualstudio.com/team-services/) bug number provided with each improvement is a unique ID that you can give [Microsoft Customer Support](https://support.microsoft.com/contactus/), include in [StackOverflow comments](https://stackoverflow.com/questions/tagged/.net)or use in web searches.

## Getting the Update

The Security and Quality Rollup is available via Windows Update, Windows Server Update Services, Microsoft Update Catalog, and Docker.

### Guidance

The February 2018 Rollup does not contain new security fixes. As a result, it is an optional update. If you want the latest fixes and improvements, you are recommended to install the appropriate updates in the following table.

If you want to install the minimum set of updates, you are recommended to:

* Install the [Windows 10 LCU](https://support.microsoft.com/en-us/help/4043454) if you are on Windows 10.
* Do not install February 2018 .NET Framework updates.

### Microsoft Update Catalog

You can get the update via the Microsoft Update Catalog. For Windows 10, .NET Framework updates are part of the Windows 10 Monthly Rollup.

<table>
<thead><tr>
<th>Product Version</th><th>Security and Quality Rollup KB</th>
</tr></thead>
<tr>
<td><strong>Windows 10 1607 (Anniversary Update)</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4087249">Catalog</a><BR><a href="https://support.microsoft.com/kb/4087249">4087249</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6, 4.6.1, 4.6.2</td><td><a href="https://support.microsoft.com/kb/4087249">4087249</a></td>
</tr>
<tr>
<td><strong>Windows 8.1<BR>Windows RT 8.1<BR>Windows Server 2012 R2</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4076494">Catalog</a><BR><a href="https://support.microsoft.com/kb/4076494">4076494</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6, 4.6.1, 4.6.2, 4.7, 4.7.1</td><td><a href="https://support.microsoft.com/kb/4054980">4054980</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.5.2</td><td><a href="https://support.microsoft.com/kb/4054990">4054990</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5</td><td><a href="https://support.microsoft.com/kb/4054999">4054999</a></td>
</tr>
<tr>
<td><strong>Windows Server 2012</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4076493">Catalog</a><BR><a href="https://support.microsoft.com/kb/4076493">4076493</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6, 4.6.1, 4.6.2, 4.7, 4.7.1</td><td><a href="https://support.microsoft.com/kb/4054979">4054979</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.5.2</td><td><a href="https://support.microsoft.com/kb/4054991">4054991</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5</td><td><a href="https://support.microsoft.com/kb/4054997">4054997</a></td>
</tr>
<tr>
<td><strong>Windows 7<BR>Windows Server 2008 R2</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4076492">Catalog</a><BR><a href="https://support.microsoft.com/kb/4076492">4076492</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6, 4.6.1, 4.6.2, 4.7, 4.7.1</td><td><a href="https://support.microsoft.com/kb/4054981">4054981</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.5.2</td><td><a href="https://support.microsoft.com/kb/4054992">4054992</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5.1</td><td><a href="https://support.microsoft.com/kb/4054998">4054998</a></td>
</tr>
<tr>
<td><strong>Windows Server 2008</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4076495">Catalog</a><BR><a href="https://support.microsoft.com/kb/4076495">4076495</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6</td><td><a href="https://support.microsoft.com/kb/4054981">4054981</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.5.2</td><td><a href="https://support.microsoft.com/kb/4054992">4054992</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 2.0, 3.0</td><td><a href="https://support.microsoft.com/kb/4054996">4054996</a></td>
</tr>
</table>

### Docker Images

Docker images have been updated as part of today's release. 

* [microsoft/aspnet](https://hub.docker.com/r/microsoft/aspnet/)
* [microsoft/dotnet-framework](https://hub.docker.com/r/microsoft/dotnet-framework/)
* [microsoft/dotnet-framework-build](https://hub.docker.com/r/microsoft/dotnet-framework-build/)
* [microsoft/dotnet-framework-samples](https://hub.docker.com/r/microsoft/dotnet-framework-samples/)
* [microsoft/wcf](https://hub.docker.com/r/microsoft/wcf/)

Note: Look at the "Tags" view in each repository to see the updated Docker image tags.

Note: Please look at [.NET Docker Announcements](https://github.com/dotnet/announcements/labels/Docker) for more information on .NET and Docker.

### Previous Monthly Rollups

The last few .NET Framework Monthly updates are listed below for your convenience:

* [January 2018 Preview of Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2018/01/26/january-2018-preview-of-quality-rollup/)
* [January 2018 Security and Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2018/01/09/net-framework-january-2018-security-and-quality-rollup/)
* [November 2017 Security and Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2017/11/14/net-framework-november-2017-security-and-quality-rollup/)
