# .NET Framework May 2018 Security and Quality Rollup

Today, we are releasing the May 2018 Security and Quality Rollup.

## Security

### CVE-2018-1039 – Windows Security Feature Bypass Vulnerability

A security feature bypass vulnerability exists in Windows which could allow an attacker to bypass Device Guard. An attacker who successfully exploited this vulnerability could circumvent a User Mode Code Integrity (UMCI) policy on the machine.  To exploit the vulnerability, an attacker would first have to access the local machine, and then run a malicious program.  The update addresses the vulnerability by correcting how Windows validates User Mode Code Integrity policies

[CVE-2018-1039](https://github.com/dotnet/announcements/issues/XXXXX)

### CVE-2018-0765 – .NET and .NET Core Denial Of Service Vulnerability

A Denial of Service vulnerability exists when .NET, and .NET core, improperly process XML documents. An attacker who successfully exploited this vulnerability could cause a denial of service against a .NET application. A remote unauthenticated attacker could exploit this vulnerability by issuing specially crafted requests to a .NET(or .NET core) application.

The update addresses the vulnerability by correcting how a .NET, and .NET core, applications handles XML document processing.


[CVE-2018-0765](https://github.com/dotnet/announcements/issues/XXXXX)

## Quality and Reliability

This release contains the following quality and reliability improvements. 

#### CLR
* Floating-point overflow in the thread pool’s hill climbing algorithm. [569602]* High CPU usage in a kernel lock ntoskrnl!ExpWaitForSpinLockExclusiveAndAcquire called by ntoskrnl!KiPageFault is resolved by CLR implemented write watch instead [568318]

Note: Additional information on these improvements is not available. The VSTS bug number provided with each improvement is a unique ID that you can give Microsoft Customer Support, include in StackOverflow commentsor use in web searches.

## Getting the Update

The Security and Quality Rollup is available via Windows Update, Windows Server Update Services, Microsoft Update Catalog, and Docker.

### Microsoft Update Catalog

You can get the update via the Microsoft Update Catalog. For Windows 10, .NET Framework updates are part of the Windows 10 Monthly Rollup.

The following table is for Windows 10 and Windows Server 2016+ versions.

<table>
<thead><tr>
<th>Product Version</th><th>Security and Quality Rollup KB</th>
</tr></thead>
<tr>
<td><strong>Windows 10 1803 (April 2018 Update)</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4103721">Catalog</a><BR><a href="https://support.microsoft.com/kb/4103721">4103721</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5</td><td><a href="https://support.microsoft.com/kb/4103721">4103721</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.7.2</td><td><a href="https://support.microsoft.com/kb/4103721">4103721</a></td>
</tr>
<tr>
<td><strong>Windows 10 1709 (Fall Creators Update)</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4103727">Catalog</a><BR><a href="https://support.microsoft.com/kb/4103727">4103727</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5</td><td><a href="https://support.microsoft.com/kb/4103727">4103727</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.7.1</td><td><a href="https://support.microsoft.com/kb/4103727">4103727</a></td>
</tr>
<tr>
<td><strong>Windows 10 1703 (Creators Update)</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4103731">Catalog</a><BR><a href="https://support.microsoft.com/kb/4103731">4103731</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5</td><td><a href="https://support.microsoft.com/kb/4103731">4103731</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.7, 4.7.1</td><td><a href="https://support.microsoft.com/kb/4103731">4103731</a></td>
</tr>
<tr>
<td><strong>Windows 10 1607 (Anniversary Update)<br> Windows Server 2016</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4103723">Catalog</a><BR><a href="https://support.microsoft.com/kb/4103723">4103723</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5</td><td><a href="https://support.microsoft.com/kb/4103723">4103723</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6.2, 4.7, 4.7.1</td><td><a href="https://support.microsoft.com/kb/4103723">4103723</a></td>
</tr>
<tr>
<td><strong>Windows 10 1507</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4103716">Catalog</a><BR><a href="https://support.microsoft.com/kb/4103716">4103716</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5</td><td><a href="https://support.microsoft.com/kb/4103716">4103716</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6, 4.6.1, 4.6.2</td><td><a href="https://support.microsoft.com/kb/4103716">4103716</a></td>
</tr>
</table>

The following table is for earlier Windows and Windows Server versions.

<table>
<thead><tr>
<th>Product Version</th><th>Security and Quality Rollup KB</th>
<th>Security Rollup KB</th>
</tr></thead>
<tr>
<td><strong>Windows 8.1<BR>Windows RT 8.1<BR>Windows Server 2012 R2</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4099635">Catalog</a><BR><a href="https://support.microsoft.com/kb/4099635">4099635</a></strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4099639">Catalog</a><BR><a href="https://support.microsoft.com/kb/4099639">4099639</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5</td><td><a href="https://support.microsoft.com/kb/4095875">4095875</a></td><td><a href="https://support.microsoft.com/kb/4095515">4095515</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.5.2</td><td><a href="https://support.microsoft.com/kb/4095876">4095876</a></td><td><a href="https://support.microsoft.com/kb/4095517">4095517</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6, 4.6.1, 4.6.2, 4.7, 4.7.1</td><td><a href="https://support.microsoft.com/kb/4096417">4096417</a></td><td><a href="https://support.microsoft.com/kb/4096236">4096236</a></td>
</tr>
<tr>
<td><strong>Windows Server 2012</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4099634">Catalog</a><BR><a href="https://support.microsoft.com/kb/4099634">4099634</a></strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4099638">Catalog</a><BR><a href="https://support.microsoft.com/kb/4099638">4099638</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5</td><td><a href="https://support.microsoft.com/kb/4095872">4095872</a></td><td><a href="https://support.microsoft.com/kb/4095512">4095512</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.5.2</td><td><a href="https://support.microsoft.com/kb/4096494">4096494</a></td><td><a href="https://support.microsoft.com/kb/4095518">4095518</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6, 4.6.1, 4.6.2, 4.7, 4.7.1</td><td><a href="https://support.microsoft.com/kb/4096416">4096416</a></td><td><a href="https://support.microsoft.com/kb/4096235">4096235</a></td>
</tr>
<tr>
<td><strong>Windows 7<BR>Windows Server 2008 R2</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4099633">Catalog</a><BR><a href="https://support.microsoft.com/kb/4099633">4099633</a></strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4099637">Catalog</a><BR><a href="https://support.microsoft.com/kb/4099637">4099637</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5.1</td><td><a href="https://support.microsoft.com/kb/4095874">4095874</a></td><td><a href="https://support.microsoft.com/kb/4095514">4095514</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.5.2</td><td><a href="https://support.microsoft.com/kb/4096495">4096495</a></td><td><a href="https://support.microsoft.com/kb/4095519">4095519</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6, 4.6.1, 4.6.2, 4.7, 4.7.1</td><td><a href="https://support.microsoft.com/kb/4096418">4096418</a></td><td><a href="https://support.microsoft.com/kb/4096237">4096237</a></td>
</tr>
<tr>
<td><strong>Windows Server 2008</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4099636">Catalog</a><BR><a href="https://support.microsoft.com/kb/4099636">4099636</a></strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4099640">Catalog</a><BR><a href="https://support.microsoft.com/kb/4099640">4099640</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 2.0, 3.0</td><td><a href="https://support.microsoft.com/kb/4095873">4095873</a></td><td><a href="https://support.microsoft.com/kb/4095513">4095513</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.5.2</td><td><a href="https://support.microsoft.com/kb/4096495">4096495</a></td><td><a href="https://support.microsoft.com/kb/4095519">4095519</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6</td><td><a href="https://support.microsoft.com/kb/4096418">4096418</a></td><td><a href="https://support.microsoft.com/kb/4096237">4096237</a></td>
</tr>
</table>

### Docker Images

We are updating the following .NET Framework Docker images for today's release:

* [microsoft/aspnet](https://hub.docker.com/r/microsoft/aspnet/)
* [microsoft/dotnet-framework](https://hub.docker.com/r/microsoft/dotnet-framework/)
* [microsoft/dotnet-framework-samples](https://hub.docker.com/r/microsoft/dotnet-framework-samples/)

Note: Look at the "Tags" view in each repository to see the updated Docker image tags.

Note: Significant changes have been made with Docker images recently. Please look at [.NET Docker Announcements](https://github.com/dotnet/announcements/labels/Docker) for more information.

### Previous Monthly Rollups

The last few .NET Framework Monthly updates are listed below for your convenience:

* [February 2018 Security and Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2018/02/13/net-framework-february-2018-security-and-quality-rollup/)
* [January 2018 Preview of Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2018/01/26/january-2018-preview-of-quality-rollup/)
* [January 2018 Security and Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2018/01/09/net-framework-january-2018-security-and-quality-rollup/)
* [November 2017 Security and Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2017/11/14/net-framework-november-2017-security-and-quality-rollup/)
