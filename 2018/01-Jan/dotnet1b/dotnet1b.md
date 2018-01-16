# .NET Framework January 2018 Security and Quality Rollup

Today, we are releasing the November 2018 Security and Quality Rollup.

## Security

### CVE-2018-0786 – Security Feature Bypass in X509 Certificate Validation

Microsoft is aware of a security vulnerability in the public versions of .NET Core where an attacker could present a certificate that is marked invalid for a specific use, but a component uses it for that purpose. This action disregards the Enhanced Key Usage tagging.

The security update addresses the vulnerability by ensuring that .NET Core components completely validate certificates.

[CVE-2018-0786](https://github.com/dotnet/announcements/issues/51)

### CVE-2018-0764 – Denial of Service when parsing XML documents

Microsoft is aware of a Denial of Service vulnerability in all public versions of .NET core due to improper processing of XML documents. An attacker who successfully exploited this vulnerability could cause a denial of service against a .NET application. A remote unauthenticated attacker could exploit this vulnerability by issuing specially crafted requests to a .NET Core application.

The update addresses the vulnerability by correcting how .NET core handles XML document processing.

[CVE-2018-0764](https://github.com/dotnet/announcements/issues/52)

## Quality and Reliability

This release contains no new following quality and reliability improvements.

## Getting the Update

The Security and Quality Rollup is available via Windows Update, Windows Server Update Services, Microsoft Update Catalog, and Docker.

### Microsoft Update Catalog

You can get the update via the Microsoft Update Catalog. For Windows 10, .NET Framework updates are part of the Windows 10 Monthly Rollup.

<table>
<thead><tr>
<th>Product Version</th><th>Security and Quality Rollup KB</th>
<th>Security Rollup KB</th>
</tr></thead>
<tr>
<td><strong>Windows 10 1709 (Fall Creators Update)</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4056892">Catalog</a><BR><a href="https://support.microsoft.com/kb/4056892">4056892</a></strong></td><td>N/A</td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5</td><td><a href="https://support.microsoft.com/kb/4056892">4056892</a></td><td>N/A</td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.7.1</td><td><a href="https://support.microsoft.com/kb/4056892">4056892</a></td><td>N/A</td>
</tr>
<tr>
<td><strong>Windows 10 1703 (Creators Update)</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4056891">Catalog</a><BR><a href="https://support.microsoft.com/kb/4056891">4056891</a></strong></td><td>N/A</td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5</td><td><a href="https://support.microsoft.com/kb/4056891">4056891</a></td><td>N/A</td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.7</td><td><a href="https://support.microsoft.com/kb/4056891">4056891</a></td><td>N/A</td>
</tr>
<tr>
<td><strong>Windows 10 1607 (Anniversary Update)</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4056890">Catalog</a><BR><a href="https://support.microsoft.com/kb/4056890">4056890</a></strong></td><td>N/A</td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5</td><td><a href="https://support.microsoft.com/kb/4056890">4056890</a></td><td>N/A</td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6.2, 4.7</td><td><a href="https://support.microsoft.com/kb/4056890">4056890</a></td><td>N/A</td>
</tr>
<tr>
<td><strong>Windows 10 1511</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4056888">Catalog</a><BR><a href="https://support.microsoft.com/kb/4056888">4056888</a></strong></td><td>N/A</td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5</td><td><a href="https://support.microsoft.com/kb/4056888">4056888</a></td><td>N/A</td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6.1</td><td><a href="https://support.microsoft.com/kb/4056888">4056888</a></td><td>N/A</td>
</tr>
<tr>
<td><strong>Windows 10 1507</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4056893">Catalog</a><BR><a href="https://support.microsoft.com/kb/4056893">4056893</a></strong></td><td>N/A</td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5</td><td><a href="https://support.microsoft.com/kb/4056893">4056893</a></td><td>N/A</td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6</td><td><a href="https://support.microsoft.com/kb/4056893">4056893</a></td><td>N/A</td>
</tr>
<tr>
<td><strong>Windows 8.1<BR>Windows RT 8.1<BR>Windows Server 2012 R2</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4055266">Catalog</a><BR><a href="https://support.microsoft.com/kb/4055266">4055266</a></strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4055271">Catalog</a><BR><a href="https://support.microsoft.com/kb/4055271">4055271</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5</td><td><a href="https://support.microsoft.com/kb/4054999">4054999</a></td><td><a href="https://support.microsoft.com/kb/4054177">4054177</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.5.2</td><td><a href="https://support.microsoft.com/kb/4054993">4054993</a></td><td><a href="https://support.microsoft.com/kb/4054170">4054170</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6, 4.6.1, 4.6.2, 4.7, 4.7.1</td><td><a href="https://support.microsoft.com/kb/4055001">4055001</a></td><td><a href="https://support.microsoft.com/kb/4054182">4054182</a></td>
</tr>
<tr>
<td><strong>Windows Server 2012</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4055265">Catalog</a><BR><a href="https://support.microsoft.com/kb/4055265">4055265</a></strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4055270">Catalog</a><BR><a href="https://support.microsoft.com/kb/4055270">4055270</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5</td><td><a href="https://support.microsoft.com/kb/4054997">4054997</a></td><td><a href="https://support.microsoft.com/kb/4054175">4054175</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.5.2</td><td><a href="https://support.microsoft.com/kb/4054994">4054994</a></td><td><a href="https://support.microsoft.com/kb/4054171">4054171</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6, 4.6.1, 4.6.2, 4.7, 4.7.1</td><td><a href="https://support.microsoft.com/kb/4055000">4055000</a></td><td><a href="https://support.microsoft.com/kb/4054181">4054181</a></td>
</tr>
<tr>
<td><strong>Windows Server 2008</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4055267">Catalog</a><BR><a href="https://support.microsoft.com/kb/4055267">4055267</a></strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4055272">Catalog</a><BR><a href="https://support.microsoft.com/kb/4055272">4055272</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 2.0, 3.0</td><td><a href="https://support.microsoft.com/kb/4054996">4054996</a></td><td><a href="https://support.microsoft.com/kb/4054174">4054174</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.5.2</td><td><a href="https://support.microsoft.com/kb/4054995">4054995</a></td><td><a href="https://support.microsoft.com/kb/4054172">4054172</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6</td><td><a href="https://support.microsoft.com/kb/4055002">4055002</a></td><td><a href="https://support.microsoft.com/kb/4054183">4054183</a></td>
</tr>
</table>


### Docker Images

Docker images have been updated as part of today's release (actually, a few days ago). 

* [microsoft/aspnet](https://hub.docker.com/r/microsoft/aspnet/)
* [microsoft/dotnet-framework](https://hub.docker.com/r/microsoft/dotnet-framework/)
* [microsoft/dotnet-framework-samples](https://hub.docker.com/r/microsoft/dotnet-framework-samples/)
* [microsoft/wcf](https://hub.docker.com/r/microsoft/wcf/)

Note: Look at the "Tags" view in each repository to see the updated Docker image tags.

Note: Significant changes have been made with Docker images recently. Please look at [.NET Docker Announcements](https://github.com/dotnet/announcements/labels/Docker) for more information.

### Previous Monthly Rollups

The last few .NET Framework Monthly updates are listed below for your convenience:

* [November 2017 Security and Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2017/11/14/net-framework-november-2017-security-and-quality-rollup/)
* [October 2017 Preview of Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2017/10/17/net-framework-october-2017-preview-of-quality-rollup/)
* [October 2017 Security and Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2017/10/10/net-framework-october-2017-security-and-quality-rollup/)
* [September 2017 Preview of Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2017/09/25/net-framework-september-2017-preview-of-quality-rollup/)
* [September 2017 Security and Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2017/09/12/net-framework-september-2017-security-and-quality-rollup/)
