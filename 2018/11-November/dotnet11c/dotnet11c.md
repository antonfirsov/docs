# .NET Framework October 2018 Preview Quality Rollup

Today, we are releasing the October 2018 Preview of Quality Rollup.

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

The Security and Quality Rollup is available via Windows Update, Windows Server Update Services, and Microsoft Update Catalog.

### Microsoft Update Catalog

You can get the update via the Microsoft Update Catalog. For Windows 10, .NET Framework updates are part of the Windows 10 Monthly Rollup.

The following table is for Windows 10 and Windows Server 2016+ versions.

<table>
<thead><tr>
<th>Product Version</th><th>Security and Quality Rollup KB</th>
</tr></thead>
<tr>
<td><strong>Windows 10 1803 (April 2018 Update)</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4458469">Catalog</a><BR><a href="https://support.microsoft.com/kb/4458469">4458469</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5, 4.7.2</td><td><a href="https://support.microsoft.com/kb/4458469">4458469</a></td>
</tr>
<tr>
<td><strong>Windows 10 1709 (Fall Creators Update)</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4457136">Catalog</a><BR><a href="https://support.microsoft.com/kb/4457136">4457136</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5, 4.7.1, 4.7.2</td><td><a href="https://support.microsoft.com/kb/4457136">4457136</a></td>
</tr>
<tr>
<td><strong>Windows 10 1703 (Creators Update)</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4457141">Catalog</a><BR><a href="https://support.microsoft.com/kb/4457141">4457141</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5, 4.7, 4.7.1, 4.7.2</td><td><a href="https://support.microsoft.com/kb/4457141">4457141</a></td>
</tr>
<tr>
<td><strong>Windows 10 1607 (Anniversary Update)</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4457127">Catalog</a><BR><a href="https://support.microsoft.com/kb/4457127">4457127</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5, 4.6.2, 4.7, 4.7.1, 4.7.2</td><td><a href="https://support.microsoft.com/kb/4457127">4457127</a></td>
</tr>
</table>

The following table is for earlier Windows and Windows Server versions.

<table>
<thead><tr>
<th>Product Version</th><th>Securty and Quality Rollup KB</th>
</tr></thead>
<tr>
<td><strong>Windows 8.1<BR>Windows RT 8.1<BR>Windows Server 2012 R2</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4458613">Catalog</a><BR><a href="https://support.microsoft.com/kb/4458613">4458613</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5</td><td><a href="https://support.microsoft.com/kb/4457009">4457009</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.5.2</td><td><a href="https://support.microsoft.com/kb/4457017">4457017</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6, 4.6.1, 4.6.2, 4.7, 4.7.1, 4.7.1, 4.7.2</td><td><a href="https://support.microsoft.com/kb/4457015">4457015</a></td>
</tr>
<tr>
<td><strong>Windows Server 2012</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4458612">Catalog</a><BR><a href="https://support.microsoft.com/kb/4458612">4458612</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5</td><td><a href="https://support.microsoft.com/kb/4457006">4457006</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.5.2</td><td><a href="https://support.microsoft.com/kb/4457018">4457018</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6, 4.6.1, 4.6.2, 4.7, 4.7.1, 4.7.1, 4.7.2</td><td><a href="https://support.microsoft.com/kb/4457014">4457014</a></td>
</tr>
<tr>
<td><strong>Windows 7<BR>Windows Server 2008 R2</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4458611">Catalog</a><BR><a href="https://support.microsoft.com/kb/4458611">4458611</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5.1</td><td><a href="https://support.microsoft.com/kb/4457008">4457008</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.5.2</td><td><a href="https://support.microsoft.com/kb/4457019">4457019</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6, 4.6.1, 4.6.2, 4.7, 4.7.1, 4.7.1, 4.7.2</td><td><a href="https://support.microsoft.com/kb/4457016">4457016</a></td>
</tr>
<tr>
<td><strong>Windows Server 2008</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4458614">Catalog</a><BR><a href="https://support.microsoft.com/kb/4458614">4458614</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 2.0, 3.0</td><td><a href="https://support.microsoft.com/kb/4457007">4457007</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.5.2</td><td><a href="https://support.microsoft.com/kb/4457019">4457019</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6</td><td><a href="https://support.microsoft.com/kb/4457016">4457016</a></td>
</tr>
</table>

### Previous Monthly Rollups

The last few .NET Framework Monthly updates are listed below for your convenience:

* [October 2018 Security and Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2018/10/09/net-framework-october-2018-security-and-quality-rollup/)
* [September 2018 Preview of Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2018/09/25/net-framework-september-2018-preview-of-quality-rollup/)
* [September 2018 Security and Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2018/09/11/net-framework-september-2018-security-and-quality-rollup/)

