# .NET Framework May 2019 Security and Quality Rollup

Today, we are releasing the May 2019 Security and Quality Rollup.

## Security

### CVE-2019-0820 – Denial of Service Vulnerability

A denial of service vulnerability exists when .NET Framework and .NET Core improperly process RegEx strings. An attacker who successfully exploited this vulnerability could cause a denial of service against a .NET application.  A remote unauthenticated attacker could exploit this vulnerability by issuing specially crafted requests to a .NET Framework (or .NET core) application.  The update addresses the vulnerability by correcting how .NET Framework and .NET Core applications handle RegEx string processing.

[CVE-2019-0820](https://github.com/dotnet/announcements/issues/XXXXX)

### CVE-2019-0980 – Denial of Service Vulnerability

A denial of service vulnerability exists when .NET Framework or .NET Core improperly handle web requests. An attacker who successfully exploited this vulnerability could cause a denial of service against a .NET Framework or .NET Core web application. The vulnerability can be exploited remotely, without authentication.  A remote unauthenticated attacker could exploit this vulnerability by issuing specially crafted requests to the .NET Framework or .NET Core application.  The update addresses the vulnerability by correcting how .NET Framework or .NET Core web applications handles web requests.

[CVE-2019-0980](https://github.com/dotnet/announcements/issues/XXXXX)

### CVE-2019-0981 – Denial of Service Vulnerability

A denial of service vulnerability exists when .NET Framework or .NET Core improperly handle web requests. An attacker who successfully exploited this vulnerability could cause a denial of service against a .NET Framework or .NET Core web application. The vulnerability can be exploited remotely, without authentication.  A remote unauthenticated attacker could exploit this vulnerability by issuing specially crafted requests to the .NET Framework or .NET Core application.  The update addresses the vulnerability by correcting how .NET Framework or .NET Core web applications handles web requests.

[CVE-2019-0981](https://github.com/dotnet/announcements/issues/XXXXX)

### CVE-2019-0864 – Denial of Service Vulnerability

A denial of service vulnerability exists when .NET Framework improperly handles objects in heap memory. An attacker who successfully exploited this vulnerability could cause a denial of service against a .NET application.  To exploit this vulnerability, an attacker would have to log on to an affected system and run a specially crafted application.  The security update addresses the vulnerability by correcting how .NET Framework handle objects in heap memory.

[CVE-2019-0864](https://github.com/dotnet/announcements/issues/XXXXX)

## Getting the Update

The Security and Quality Rollup is available via Windows Update, Windows Server Update Services, Microsoft Update Catalog, and Docker.

### Microsoft Update Catalog

You can get the update via the Microsoft Update Catalog. For Windows 10, .NET Framework updates are part of the Windows 10 Monthly Rollup.

The following table is for Windows 10 and Windows Server 2016+ versions.

<table>
<thead><tr>
<th>Product Version</th><th>Cumulative Rollup</th>
</tr></thead>
<tr>
<td><strong>Windows 10 1903 (May 2019 Update)</strong></td><td><strong>
<a href="https://support.microsoft.com/kb/4502507">4502507</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5, 4.8</td><td>
<a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4495620">Catalog</a><BR>
<a href="https://support.microsoft.com/kb/4495620">4495620</a></td>
</tr>
<tr>
<td><strong>Windows 10 1809 (October 2018 Update)<br> Windows Server 2019</strong></td><td><strong>
<a href="https://support.microsoft.com/kb/4466961">4466961</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5, 4.7.2</td><td>
<a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4499405">Catalog</a><BR>
<a href="https://support.microsoft.com/kb/4495590">4495590</a></td>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5, 4.8</td><td>
<a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4499405">Catalog</a><BR>
<a href="https://support.microsoft.com/kb/4495618">4495618</a></td>
</tr>
<tr>
<td><strong>Windows 10 1803 (April 2018 Update)</strong></td><td><strong>
<a href="https://support.microsoft.com/kb/4498144">4498144</a></strong></td>
</tr>
<td style="padding-left:.5cm">.NET Framework 3.5, 4.7.2</td><td>
<a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4499167">Catalog</a><BR>
<a href="https://support.microsoft.com/kb/4499167">4499167</a></td>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.8</td><td>
<a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4495616">Catalog</a><BR>
<a href="https://support.microsoft.com/kb/4495616">4495616</a></td>
</tr>
<tr>
<td><strong>Windows 10 1709 (Fall Creators Update)</strong></td><td><strong>
<a href="https://support.microsoft.com/kb/4498143">4498143</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5, 4.7.1, 4.7.2</td><td>
<a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4499179">Catalog</a><BR>
<a href="https://support.microsoft.com/kb/4499179">4499179</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.8</td><td>
<a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4495613">Catalog</a><BR>
<a href="https://support.microsoft.com/kb/4495613">4495613</a></td>
</tr>
<tr>
<td><strong>Windows 10 1703 (Creators Update)</strong></td><td><strong>
<a href="https://support.microsoft.com/kb/4498142">4498142</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5, 4.7, 4.7.1, 4.7.2</td><td>
<a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4499181">Catalog</a><BR>
<a href="https://support.microsoft.com/kb/4499181">4499181</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.8</td><td>
<a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4495611">Catalog</a><BR>
<a href="https://support.microsoft.com/kb/4495611">4495611</a></td>
</tr>
<tr>
<td><strong>Windows 10 1607 (Anniversary Update)<br> Windows Server 2016</strong></td><td><strong>
<a href="https://support.microsoft.com/kb/4498141">4498141</a></strong></td>
</tr>
<tr><td style="padding-left:.5cm">.NET Framework 3.5, 4.6.2, 4.7, 4.7.1, 4.7.2<</td><td>
<a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4494440">Catalog</a><BR>
<a href="https://support.microsoft.com/kb/4494440">4494440</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.8</td><td>
<a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4495610">Catalog</a><BR>
<a href="https://support.microsoft.com/kb/4495610">4495610</a></td>
</tr>
<tr>
<td><strong>Windows 10 1507</strong></td><td><strong>
<a href="https://support.microsoft.com/kb/4499154">4499154</a></strong></td>
</tr>
<tr><td style="padding-left:.5cm">.NET Framework 3.5, 4.6, 4.6.1, 4.6.2<</td><td>
<a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4499154">Catalog</a><BR>
<a href="https://support.microsoft.com/kb/4499154">4499154</a></td>
</tr>
</table>

The following table is for earlier Windows and Windows Server versions.

<table>
<thead><tr>
<th>Product Version</th><th>Security and Quality Rollup KB</th><th>Security Only Update KB</th>
</tr></thead>
<tr>
<td><strong>Windows 8.1<BR>Windows RT 8.1<BR>Windows Server 2012 R2</strong></td><td><strong>
<a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4481485">Catalog</a><BR>
<a href="https://support.microsoft.com/kb/4481485">4481485</a></strong></td><td><strong>
<a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4481484">Catalog</a><BR>
<a href="https://support.microsoft.com/kb/4481484">4481484</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5</td><td>
<a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4480064">Catalog</a><BR>
<a href="https://support.microsoft.com/kb/4480064">4480064</a></strong></td><td><strong>
<a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4480086">Catalog</a><BR>
<a href="https://support.microsoft.com/kb/4480086">4480086</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.5.2</td><td>
<a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4480057">Catalog</a><BR>
<a href="https://support.microsoft.com/kb/4480057">4480057</a></strong></td><td><strong>
<a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4480074">Catalog</a><BR>
<a href="https://support.microsoft.com/kb/4480074">4480074</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6, 4.6.1, 4.6.2, 4.7, 4.7.1, 4.7.2</td><td>
<a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4480054">Catalog</a><BR>
<a href="https://support.microsoft.com/kb/4480054">4480054</a></strong></td><td><strong>
<a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4480071">Catalog</a><BR>
<a href="https://support.microsoft.com/kb/4480071">4480071</a></td>
</tr>
<tr>
<td><strong>Windows Server 2012</strong></td>
<td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4481482">Catalog</a><BR>
<a href="https://support.microsoft.com/kb/4481482">4481482</a></strong></td>
<td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4481483">Catalog</a><BR>
<a href="https://support.microsoft.com/kb/4481483">4481483</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5</td><td>
<a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4480061">Catalog</a><BR>
<a href="https://support.microsoft.com/kb/4480061">4480061</a></strong></td><td><strong>
<a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4480083">Catalog</a><BR>
<a href="https://support.microsoft.com/kb/4480083">4480083</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.5.2</td><td>
<a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4480058">Catalog</a><BR>
<a href="https://support.microsoft.com/kb/4480058">4480058</a></strong></td><td><strong>
<a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4480075">Catalog</a><BR>
<a href="https://support.microsoft.com/kb/4480075">4480075</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6, 4.6.1, 4.6.2, 4.7, 4.7.1, 4.7.2</td><td>
<a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4480051">Catalog</a><BR>
<a href="https://support.microsoft.com/kb/4480051">4480051</a></strong></td><td><strong>
<a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4480070">Catalog</a><BR>
<a href="https://support.microsoft.com/kb/4480070">4480070</a></td>
</tr>
<tr>
<td><strong>Windows 7 SP1<BR>Windows Server 2008 R2 SP1</strong></td><td><strong>
<a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4481480">Catalog</a><BR>
<a href="https://support.microsoft.com/kb/4481480">4481480</a></strong></td><td><strong>
<a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4481481">Catalog</a><BR>
<a href="https://support.microsoft.com/kb/4481481">4481481</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5.1</td><td>
<a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4480063">Catalog</a><BR>
<a href="https://support.microsoft.com/kb/4480063">4480063</a></strong></td><td><strong>
<a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4480085">Catalog</a><BR>
<a href="https://support.microsoft.com/kb/4480085">4480085</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.5.2</td><td>
<a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4480059">Catalog</a><BR>
<a href="https://support.microsoft.com/kb/4480059">4480059</a></strong></td><td><strong>
<a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4480076">Catalog</a><BR>
<a href="https://support.microsoft.com/kb/4480076">4480076</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6, 4.6.1, 4.6.2, 4.7, 4.7.1, 4.7.2</td><td>
<a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4480055">Catalog</a><BR><a href="https://support.microsoft.com/kb/4480055">4480055</a></strong></td><td><strong>
<a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4480072">Catalog</a><BR>
<a href="https://support.microsoft.com/kb/4480072">4480072</a></td>
</tr>
<tr>
<td><strong>Windows Server 2008</strong></td><td><strong>
<a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4481486">Catalog</a><BR>
<a href="https://support.microsoft.com/kb/4481486">4481486</a></strong></td><td><strong>
<a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4481487">Catalog</a><BR>
<a href="https://support.microsoft.com/kb/4481487">4481487</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 2.0, 3.0</td><td>
<a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4480062">Catalog</a><BR>
<a href="https://support.microsoft.com/kb/4480062">4480062</a></strong></td><td><strong>
<a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4480084">Catalog</a><BR>
<a href="https://support.microsoft.com/kb/4480084">4480084</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.5.2</td><td>
<a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4480059">Catalog</a><BR>
<a href="https://support.microsoft.com/kb/4480059">4480059</a></strong></td><td><strong>
<a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4480076">Catalog</a><BR>
<a href="https://support.microsoft.com/kb/4480076">4480076</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6</td><td>
<a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4480055">Catalog</a><BR>
<a href="https://support.microsoft.com/kb/4480055">4480055</a></strong></td><td><strong>
<a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4480072">Catalog</a><BR>
<a href="https://support.microsoft.com/kb/4480072">4480072</a></td>
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

* [December 2018 Security and Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2018/12/11/net-framework-december-2018-security-and-quality-rollup/)
* [November 2018 Preview of Cumulative Update for Windows 10 version 1809 and Windows Server 2019](https://blogs.msdn.microsoft.com/dotnet/2018/12/05/net-framework-december-4-2018-preview-of-cumulative-update-for-windows-10-version-1809-and-windows-server-2019/)
* [November 2018 Preview of Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2018/11/27/net-framework-november-2018-preview-of-quality-rollup/)
* [November 2018 Security and Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2018/11/13/net-framework-november-2018-security-and-quality-rollup/)

