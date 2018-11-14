# .NET Framework September 2018 Security and Quality Rollup

Today, we are releasing the September 2018 Security and Quality Rollup.

## Security

### CVE-2018-8421 – Windows Remote Code Execution Vulnerability

This security update resolves a vulnerability in Microsoft .NET Framework that could allow remote code execution when .NET Framework processes untrusted input. An attacker who successfully exploits this vulnerability in software by using .NET Framework could take control of an affected system. The attacker could then install programs; view, change, or delete data; or create new accounts that have full user rights. Users whose accounts are configured to have fewer user rights on the system could be less affected than users who operate with administrative user rights.

To exploit the vulnerability, an attacker would first have to convince the user to open a malicious document or application.

This security update addresses the vulnerability by correcting how .NET Framework validates untrusted input.

[CVE-2018-0765](https://portal.msrc.microsoft.com/security-guidance/advisory/CVE-2018-8421)

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
<td><strong>Windows 10 1803 (April 2018 Update)</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4457128">Catalog</a><BR><a href="https://support.microsoft.com/kb/4457128">4457128</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5</td><td><a href="https://support.microsoft.com/kb/4457128">4457128</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.7.2</td><td><a href="https://support.microsoft.com/kb/4457128">4457128</a></td>
</tr>
<tr>
<td><strong>Windows 10 1709 (Fall Creators Update)</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4457142">Catalog</a><BR><a href="https://support.microsoft.com/kb/4457142">4457142</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5</td><td><a href="https://support.microsoft.com/kb/4457142">4457142</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.7.1</td><td><a href="https://support.microsoft.com/kb/4457142">4457142</a></td>
</tr>
<tr>
<td><strong>Windows 10 1703 (Creators Update)</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4457138">Catalog</a><BR><a href="https://support.microsoft.com/kb/4457138">4457138</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5</td><td><a href="https://support.microsoft.com/kb/4457138">4457138</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.7, 4.7.1</td><td><a href="https://support.microsoft.com/kb/4457138">4457138</a></td>
</tr>
<tr>
<td><strong>Windows 10 1607 (Anniversary Update)<br> Windows Server 2016</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4457131">Catalog</a><BR><a href="https://support.microsoft.com/kb/4457131">4457131</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5</td><td><a href="https://support.microsoft.com/kb/4457131">4457131</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6.2, 4.7, 4.7.1</td><td><a href="https://support.microsoft.com/kb/4457131">4457131</a></td>
</tr>
<tr>
<td><strong>Windows 10 1507</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4457132">Catalog</a><BR><a href="https://support.microsoft.com/kb/4457132">4457132</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5</td><td><a href="https://support.microsoft.com/kb/4457132">4457132</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6, 4.6.1, 4.6.2</td><td><a href="https://support.microsoft.com/kb/4457132">4457132</a></td>
</tr>
</table>

The following table is for earlier Windows and Windows Server versions.

<table>
<thead><tr>
<th>Product Version</th><th>Security and Quality Rollup KB</th>
<th>Security Rollup KB</th>
</tr></thead>
<tr>
<td><strong>Windows 8.1<BR>Windows RT 8.1<BR>Windows Server 2012 R2</strong></td>
<td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4457920">Catalog</a><BR><a href="https://support.microsoft.com/kb/4457920">4457920</a></strong></td>
<td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4457916">Catalog</a><BR><a href="https://support.microsoft.com/kb/4457916">4457916</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5</td>
<td><a href="https://support.microsoft.com/kb/4457045">4457045</a></td>
<td><a href="https://support.microsoft.com/kb/4457056">4457056</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.5.2</td>
<td><a href="https://support.microsoft.com/kb/4457036">4457036</a></td>
<td><a href="https://support.microsoft.com/kb/4457028">4457028</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6, 4.6.1, 4.6.2, 4.7, 4.7.1</td>
<td><a href="https://support.microsoft.com/kb/4457034">4457034</a></td>
<td><a href="https://support.microsoft.com/kb/4457026">4457026</a></td>
</tr>
<tr>
<td><strong>Windows Server 2012</strong></td>
<td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4457919">Catalog</a><BR><a href="https://support.microsoft.com/kb/4457919">4457919</a></strong></td>
<td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4457915">Catalog</a><BR><a href="https://support.microsoft.com/kb/4457915">4457915</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5</td>
<td><a href="https://support.microsoft.com/kb/4457042">4457042</a></td>
<td><a href="https://support.microsoft.com/kb/4457053">4457053</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.5.2</td>
<td><a href="https://support.microsoft.com/kb/4457037 ">4457037</a></td>
<td><a href="https://support.microsoft.com/kb/4457029">4457029</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6, 4.6.1, 4.6.2, 4.7, 4.7.1</td>
<td><a href="https://support.microsoft.com/kb/4457033">4457033</a></td>
<td><a href="https://support.microsoft.com/kb/4457025">4457025</a></td>
</tr>
<tr>
<td><strong>Windows 7<BR>Windows Server 2008 R2</strong></td>
<td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4457918">Catalog</a><BR><a href="https://support.microsoft.com/kb/4457918">4457918</a></strong></td>
<td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4099637">Catalog</a><BR><a href="https://support.microsoft.com/kb/4099637">4457914</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5.1</td>
<td><a href="https://support.microsoft.com/kb/4457044">4457044</a></td>
<td><a href="https://support.microsoft.com/kb/4457055">4457055</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.5.2</td>
<td><a href="https://support.microsoft.com/kb/4457038">4457038</a></td>
<td><a href="https://support.microsoft.com/kb/4457030">4457030</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6, 4.6.1, 4.6.2, 4.7, 4.7.1</td>
<td><a href="https://support.microsoft.com/kb/4457035">4457035</a></td>
<td><a href="https://support.microsoft.com/kb/4457027">4457027</a></td>
</tr>
<tr>
<td><strong>Windows Server 2008</strong></td>
<td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4457921">Catalog</a><BR><a href="https://support.microsoft.com/kb/4457921">4457921</a></strong></td>
<td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4457917">Catalog</a><BR><a href="https://support.microsoft.com/kb/4457917">4457917</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 2.0, 3.0</td>
<td><a href="https://support.microsoft.com/kb/4457043">4457043</a></td>
<td><a href="https://support.microsoft.com/kb/4457054">4457054</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.5.2</td>
<td><a href="https://support.microsoft.com/kb/4457038">4457038</a></td>
<td><a href="https://support.microsoft.com/kb/4457030">4457030</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6</td>
<td><a href="https://support.microsoft.com/kb/4457035">4457035</a></td>
<td><a href="https://support.microsoft.com/kb/4457027">4457027</a></td>
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

* [August 2018 Preview of Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2018/08/30/net-framework-august-2018-preview-of-quality-rollup/)
* [August 2018 Security and Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2018/08/14/august-2018-security-and-quality-rollup/)
* [July 2018 Update](https://blogs.msdn.microsoft.com/dotnet/2018/07/30/net-framework-july-2018-update/)
* [June 2018 Security and Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2018/06/13/net-framework-june-2018-security-and-quality-rollup/)
