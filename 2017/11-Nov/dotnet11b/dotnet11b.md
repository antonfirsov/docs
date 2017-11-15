# .NET Framework November 2017 Security and Quality Rollup

Today, we are releasing the November 2017 Security and Quality Rollup.

## Security

This release contains no new security updates. The most recent .NET security updates were shipped with the [September 2017 Security and Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2017/09/12/net-framework-september-2017-security-and-quality-rollup/).

## Quality and Reliability

This release contains the following quality and reliability improvements.

#### CLR

* Code optimization bug for x64 C# code targeting .NET Framework 4.6.1 and running on .NET Framework 4.7. [484415]

#### WPF

* WPF touch stops working after many touch events due to reference counting issue. [460192]
* WPF touch generates a NullReferenceException in System.Windows.Input.StylusWisp.WispLogic.ProcessInputReport with .NET Framework 4.7. [480909]
* WPF crash caused by INVALID_POINTER_WRITE_c0000005_PenIMC_v0400.dll!CPimcContext::GetPenEventMultiple. [488390]
* WPF rendering of UI Elements broken in Windows Services. [497604]

Note: Additional information on these improvements is not available. The [VSTS](https://www.visualstudio.com/team-services/) bug number provided with each improvement is a unique ID that you can give [Microsoft Customer Support](https://support.microsoft.com/contactus/), include in [StackOverflow comments](https://stackoverflow.com/questions/tagged/.net)or use in web searches.

## Security Compliance Guidance

This guidance is for companies that want to install the **minimum** set of security updates each month. If you want security and quality updates, do not follow this guidance.

Guidance for this month:

* Install Windows 10 updates (the [Windows 10 LCU](https://support.microsoft.com/en-us/help/4043454)).
* Do not install pre-Win10 .NET Framework updates.

Rationale: .NET Framework updates for Windows 10 are included with the Windows 10 LCU (which typically include Windows security updates), while pre-Windows 10 .NET Framework updates are separate updates (which only include quality updates this month).

Note: You can look at the Classification release attribute to see if a .NET Framework update falls under either “Security Updates” or “Updates” category. See this month's [Windows 10 1709](http://www.catalog.update.microsoft.com/Search.aspx?q=4048955) and [Windows 7](http://www.catalog.update.microsoft.com/Search.aspx?q=4049016) releases as examples.

## Getting the Update

The Security and Quality Rollup is available via Windows Update, Windows Server Update Services, Microsoft Update Catalog, and Docker.

### Microsoft Update Catalog

You can get the update via the Microsoft Update Catalog. For Windows 10, .NET Framework updates are part of the Windows 10 Monthly Rollup.

<table>
<thead><tr>
<th>Product Version</th><th>Security and Quality Rollup KB</th>
</tr></thead>
<tr>
<td><strong>Windows 10 1709 (Fall Creators Update)</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4048955">Catalog</a><BR><a href="https://support.microsoft.com/kb/4048955">4048955</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.7.1</td><td><a href="https://support.microsoft.com/kb/4048954">4048954</a></td>
</tr>
<tr>
<td><strong>Windows 10 1703 (Creators Update)</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4048954">Catalog</a><BR><a href="https://support.microsoft.com/kb/4048954">4048954</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.7, 4.7.1</td><td><a href="https://support.microsoft.com/kb/4048954">4048954</a></td>
</tr>
<tr>
<td><strong>Windows 8.1<BR>Windows RT 8.1<BR>Windows Server 2012 R2</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4049017">Catalog</a><BR><a href="https://support.microsoft.com/kb/4049017">4049017</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6, 4.6.1, 4.6.2, 4.7</td><td><a href="https://support.microsoft.com/kb/4041777">4041777</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.5.2</td><td><a href="https://support.microsoft.com/kb/4040974">4040974</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5</td><td><a href="https://support.microsoft.com/kb/4040981">4040981</a></td>
</tr>
<tr>
<td><strong>Windows Server 2012</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4049018">Catalog</a><BR><a href="https://support.microsoft.com/kb/4049018">4049018</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6, 4.6.1, 4.6.2, 4.7</td><td><a href="https://support.microsoft.com/kb/4041776">4041776</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.5.2</td><td><a href="https://support.microsoft.com/kb/4040975">4040975</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5</td><td><a href="https://support.microsoft.com/kb/4040979">4040979</a></td>
</tr>
<tr>
<td><strong>Windows 7<BR>Windows Server 2008 R2</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4049016">Catalog</a><BR><a href="https://support.microsoft.com/kb/4049016">4049016</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6, 4.6.1, 4.6.2, 4.7</td><td><a href="https://support.microsoft.com/kb/4041778">4041778</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.5.2</td><td><a href="https://support.microsoft.com/kb/4040977">4040977</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5.1</td><td><a href="https://support.microsoft.com/kb/4040980">4040980</a></td>
</tr>
<tr>
<td><strong>Windows Server 2008</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4049019">Catalog</a><BR><a href="https://support.microsoft.com/kb/4049019">4049019</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6</td><td><a href="https://support.microsoft.com/kb/4041778">4041778</a></td>
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

* [microsoft/aspnet](https://hub.docker.com/r/microsoft/aspnet/)
* [microsoft/dotnet-framework](https://hub.docker.com/r/microsoft/dotnet-framework/)
* [microsoft/wcf](https://hub.docker.com/r/microsoft/wcf/)

Note: Look at the "Tags" view in each repository to see the updated Docker image tags.

Note: Significant changes have been made with Docker images recently. Please look at [.NET Docker Announcements](https://github.com/dotnet/announcements/labels/Docker) for more information.

### Previous Monthly Rollups

The last few .NET Framework Monthly updates are listed below for your convenience:

* [October 2017 Preview of Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2017/10/17/net-framework-october-2017-preview-of-quality-rollup/)
* [October 2017 Security and Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2017/10/10/net-framework-october-2017-security-and-quality-rollup/)
* [September 2017 Preview of Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2017/09/25/net-framework-september-2017-preview-of-quality-rollup/)
* [September 2017 Security and Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2017/09/12/net-framework-september-2017-security-and-quality-rollup/)
