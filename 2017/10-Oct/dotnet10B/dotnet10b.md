# .NET Framework October 2017 Security and Quality Rollup

Today, we are releasing the October 2017 Security and Quality Rollup.

This update applies to all supported Windows versions. It includes a known issue for Windows 10 1507 (see below).

## Security

This release contains no new security updates.

## Quality and Reliability

This release contains the following quality and reliability improvements.

#### WPF

* WPF fails to load resources if two versions of the same assembly are loaded. [378607]
* WPF consumes high % of CPU in Visual Studio when console session not active. [391184]
* Visual Studio fails due to “Unable to load DLL ‘PenIMC.dll'” error. [452476]
* Application crash due to call into DWrite. [453529]
* TargetFrameworkName is null with mixed mode application. [425074]
* Event leak with WPF application on touch screen monitors on Windows 10. [434946]
* Rendering of WPF UI Elements is disabled in Windows Services. [497334]

Note: Additional information on these improvements is not available. The [VSTS](https://www.visualstudio.com/team-services/) bug number provided with each improvement is a unique ID that you can give [Microsoft Customer Support](https://support.microsoft.com/contactus/), include in [StackOverflow comments](https://stackoverflow.com/questions/tagged/.net)or use in web searches.

## Getting the Update

The Security and Quality Rollup is available via Windows Update, Windows Server Update Services, Microsoft Update Catalog, and Docker.

### Microsoft Update Catalog

You can get the update via the Microsoft Update Catalog. For Windows 10, .NET Framework updates are part of the Windows 10 Monthly Rollup.

<table>
<thead><tr>
<th>Product Version</th><th>Security and Quality Rollup KB</th>
</tr></thead>
<tr>
<td><strong>Windows 10 1703 (Creators Update)</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4041676">Catalog</a><BR><a href="https://support.microsoft.com/kb/4041676">4041676</a></strong></td>
</tr>
<tr>
<td><strong>Windows 10 1607 (Anniversary Update)</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4041691">Catalog</a><BR><a href="https://support.microsoft.com/kb/4041691">4041691</a></strong></td>
</tr>
<tr>
<td><strong>Windows 10 1507</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4042895">Catalog</a><BR><a href="https://support.microsoft.com/kb/4042895">4042895</a></strong></td>
</tr>
<tr>
<td><strong>Windows 8.1<BR>Windows RT 8.1<BR>Windows Server 2012 R2</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4043767">Catalog</a><BR><a href="https://support.microsoft.com/kb/4043767">4043767</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6, 4.6.1, 4.6.2, 4.7</td><td><a href="https://support.microsoft.com/kb/4043763">4043763</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.5.2</td><td><a href="https://support.microsoft.com/kb/4040974">4040974</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5</td><td><a href="https://support.microsoft.com/kb/4040981">4040981</a></td>
</tr>
<tr>
<td><strong>Windows Server 2012</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4043769">Catalog</a><BR><a href="https://support.microsoft.com/kb/4043769">4043769</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6, 4.6.1, 4.6.2, 4.7</td><td><a href="https://support.microsoft.com/kb/4043762">4043762</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.5.2</td><td><a href="https://support.microsoft.com/kb/4040975">4040975</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5</td><td><a href="https://support.microsoft.com/kb/4040979">4040979</a></td>
</tr>
<tr>
<td><strong>Windows 7<BR>Windows Server 2008 R2</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4043766">Catalog</a><BR><a href="https://support.microsoft.com/kb/4043766">4043766</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6, 4.6.1, 4.6.2, 4.7</td><td><a href="https://support.microsoft.com/kb/4043764">4043764</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.5.2</td><td><a href="https://support.microsoft.com/kb/4040977">4040977</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5.1</td><td><a href="https://support.microsoft.com/kb/4040980">4040980</a></td>
</tr>
<tr>
<td><strong>Windows Server 2008</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4043768">Catalog</a><BR><a href="https://support.microsoft.com/kb/4043768">4043768</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6</td><td><a href="https://support.microsoft.com/kb/4043764">4043764</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.5.2</td><td><a href="https://support.microsoft.com/kb/4040977">4040977</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 2.0</td><td><a href="https://support.microsoft.com/kb/4040978">4040978</a></td>
</tr>
</table>

### Docker Images

Docker images has not yet been updated as part of today's release. They will be updated in the shortly. This post will be updated at that time.

### Known Issues

* [WPF crashes after the October 2017 Security and Monthly Quality Rollup is applied on Windows 10 version 1507 that has Microsoft .NET Framework 4.6.2 installed](https://support.microsoft.com/help/4048718).

### Previous Monthly Rollups

The last few .NET Framework Monthly updates are listed below for your convenience:

* [September 2017 Security and Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2017/09/12/net-framework-september-2017-security-and-quality-rollup/)
* [August 2017 Preview of Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2017/08/16/net-framework-august-2017-preview-of-quality-rollup/)
* [August 2017 Security and Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2017/08/08/net-framework-august-2017-security-and-quality-rollup/)
