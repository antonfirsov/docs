# .NET Framework January 2018 Preview of Quality Rollup

We recently released the January 2018 Preview of Quality Rollup.

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

# Getting the Update

The Security and Quality Rollup is available via Windows Update, Windows Server Update Services, Microsoft Update Catalog, and Docker.

### Microsoft Update Catalog

You can get the update via the Microsoft Update Catalog.

<table>
<thead><tr>
<th>Product Version</th><th>Security and Quality Rollup KB</th>
</tr></thead>
<tr>
<td><strong>Windows 10 1703 (Creators Update)</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4057144">Catalog</a><BR><a href="https://support.microsoft.com/kb/4057144">4057144</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.7.1</td><td><a href="https://support.microsoft.com/kb/4057144">4057144</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.7</td><td><a href="https://support.microsoft.com/kb/4057144">4057144</a></td>
</tr>
<tr>
<td><strong>Windows 10 1607 (Anniversary Update)</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4057142">Catalog</a><BR><a href="https://support.microsoft.com/kb/4057142">4057142</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.7.1</td><td><a href="https://support.microsoft.com/kb/4057142">4057142</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.7</td><td><a href="https://support.microsoft.com/kb/4057142">4057142</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6.2</td><td><a href="https://support.microsoft.com/kb/4057142">4057142</a></td>
</tr>
<tr>
<td><strong>Windows 8.1<BR>Windows RT 8.1<BR>Windows Server 2012 R2</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4057272">Catalog</a><BR><a href="https://support.microsoft.com/kb/4057272">4057272</a></strong></td>
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
<td><strong>Windows Server 2012</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4057271">Catalog</a><BR><a href="https://support.microsoft.com/kb/4057271">4057271</a></strong></td>
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
<td><strong>Windows 7<BR>Windows Server 2008 R2</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4057270">Catalog</a><BR><a href="https://support.microsoft.com/kb/4057270">4057270</a></strong></td>
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
<td><strong>Windows Server 2008</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4057273">Catalog</a><BR><a href="https://support.microsoft.com/kb/4057273">4057273</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6</td><td><a href="https://support.microsoft.com/kb/4054981">4054981</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.5.2</td><td><a href="https://support.microsoft.com/kb/4054992">4054992</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 2.0</td><td><a href="https://support.microsoft.com/kb/3.0">3.0</a></td>
</tr>
</table>

### Previous Monthly Rollups

The last few .NET Framework Monthly updates are listed below for your convenience:

* [January 2018 Security and Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2018/01/09/net-framework-january-2018-security-and-quality-rollup/)
* [November 2017 Security and Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2017/11/14/net-framework-november-2017-security-and-quality-rollup/)
* [October 2017 Preview of Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2017/10/17/net-framework-october-2017-preview-of-quality-rollup/)
* [October 2017 Security and Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2017/10/10/net-framework-october-2017-security-and-quality-rollup/)
