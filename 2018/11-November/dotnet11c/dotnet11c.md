# .NET Framework November 2018 Preview Quality Rollup

Today, we are releasing the November 2018 Preview of Quality Rollup.

## Quality and Reliability

This release contains the following quality and reliability improvements.

### CLR
* Addressed an issue with KB4096417 where we switched to CLR-implemented write-watch for pages. The GC will no longer call VirtualAlloc when running under workstation GC mode. [685611]

### SQL 
* Provides an AppContext flag for making the default value of TransparentNetworkIPResolutionfalse in SqlClient connection strings. [690465]

### WCF
* Addressed a System.AccessViolationException due to accessing disposed X509Certificate2 instance in a rare race condition to defer the service certificate cleanup to GC. The impacted scenario is WCF NetTcp bindings using reliable sessions with certificate authentication. [657003]

Note: Additional information on these improvements is not available. The [VSTS](https://www.visualstudio.com/team-services/) bug number provided with each improvement is a unique ID that you can give [Microsoft Customer Support](https://support.microsoft.com/contactus/), include in [StackOverflow comments](https://stackoverflow.com/questions/tagged/.net) or use in web searches.

## Getting the Update

The Preview of Quality Rollup is available via Windows Update, Windows Server Update Services, and Microsoft Update Catalog.

### Microsoft Update Catalog

You can get the update via the Microsoft Update Catalog. For Windows 10, .NET Framework updates are part of the Windows 10 Monthly Rollup.

The following table is for Windows 10 and Windows Server 2016+ versions.

<table>
<thead><tr>
<th>Product Version</th><th>Preview Quality Rollup KB</th>
</tr></thead>
<tr>
<td><strong>Windows 10 1803 (April 2018 Update)</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4467682">Catalog</a><BR><a href="https://support.microsoft.com/kb/4467682">4467682</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5, 4.7.2</td><td><a href="https://support.microsoft.com/kb/4467682">4467682</a></td>
</tr>
<tr>
<td><strong>Windows 10 1709 (Fall Creators Update)</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4467681">Catalog</a><BR><a href="https://support.microsoft.com/kb/4467681">4467681</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5, 4.7.1, 4.7.2</td><td><a href="https://support.microsoft.com/kb/4467681">4467681</a></td>
</tr>
<tr>
<td><strong>Windows 10 1703 (Creators Update)</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4467699">Catalog</a><BR><a href="https://support.microsoft.com/kb/4467699">4467699</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5, 4.7, 4.7.1, 4.7.2</td><td><a href="https://support.microsoft.com/kb/4467699">4467699</a></td>
</tr>
<tr>
<td><strong>Windows 10 1607 (Anniversary Update)</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4467684">Catalog</a><BR><a href="https://support.microsoft.com/kb/4467684">4467684</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5, 4.6.2, 4.7, 4.7.1, 4.7.2</td><td><a href="https://support.microsoft.com/kb/4467684">4467684</a></td>
</tr>
</table>

The following table is for earlier Windows and Windows Server versions.

<table>
<thead><tr>
<th>Product Version</th><th>Preview of Quality Rollup KB</th>
</tr></thead>
<tr>
<td><strong>Windows 8.1<BR>Windows RT 8.1<BR>Windows Server 2012 R2</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4467226">Catalog</a><BR><a href="https://support.microsoft.com/kb/4467226">4467226</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5</td><td><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=KB4459935">Catalog</a><BR><a href="https://support.microsoft.com/kb/4459935">4459935</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.5.2</td><td><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=KB4459943">Catalog</a><BR><a href="https://support.microsoft.com/kb/4459943">4459943</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6, 4.6.1, 4.6.2, 4.7, 4.7.1, 4.7.1, 4.7.2</td><td><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=KB4467087">Catalog</a><BR><a href="https://support.microsoft.com/kb/4467087">4467087</a></td>
</tr>
<tr>
<td><strong>Windows Server 2012</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4467225">Catalog</a><BR><a href="https://support.microsoft.com/kb/4467225">4467225</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5</td><td><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=KB4459932">Catalog</a><BR><a href="https://support.microsoft.com/kb/4459932">4459932</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.5.2</td><td><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=KB4459944">Catalog</a><BR><a href="https://support.microsoft.com/kb/4459944">4459944</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6, 4.6.1, 4.6.2, 4.7, 4.7.1, 4.7.1, 4.7.2</td><td><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=KB4467086">Catalog</a><BR><a href="https://support.microsoft.com/kb/4467086">4467086</a></td>
</tr>
<tr>
<td><strong>Windows 7<BR>Windows Server 2008 R2</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4467224">Catalog</a><BR><a href="https://support.microsoft.com/kb/4467224">4467224</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5.1</td><td><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=KB4459934">Catalog</a><BR><a href="https://support.microsoft.com/kb/4459934">4459934</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.5.2</td><td><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=KB4459945">Catalog</a><BR><a href="https://support.microsoft.com/kb/4459945">4459945</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6, 4.6.1, 4.6.2, 4.7, 4.7.1, 4.7.1, 4.7.2</td><td><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=KB4467088">Catalog</a><BR><a href="https://support.microsoft.com/kb/4467088">4467088</a></td>
</tr>
<tr>
<td><strong>Windows Server 2008</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4467227">Catalog</a><BR><a href="https://support.microsoft.com/kb/4467227">4467227</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 2.0, 3.0</td><td><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=KB4459933">Catalog</a><BR><a href="https://support.microsoft.com/kb/4459933">4459933</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.5.2</td><td><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=KB4459945">Catalog</a><BR><a href="https://support.microsoft.com/kb/4459945">4459945</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6</td><td><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=KB4467088">Catalog</a><BR><a href="https://support.microsoft.com/kb/4467088">4467088</a></td>>
</tr>
</table>

### Previous Monthly Rollups

The last few .NET Framework Monthly updates are listed below for your convenience:

* [November 2018 Security and Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2018/11/13/net-framework-november-2018-security-and-quality-rollup/)
* [October 2018 Preview of Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2018/10/18/net-framework-october-2018-preview-of-quality-rollup/)
* [October 2018 Security and Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2018/10/09/net-framework-october-2018-security-and-quality-rollup/)
* [September 2018 Preview of Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2018/09/25/net-framework-september-2018-preview-of-quality-rollup/)
* [September 2018 Security and Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2018/09/11/net-framework-september-2018-security-and-quality-rollup/)

