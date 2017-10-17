# .NET Framework October 2017 Preview of Quality Rollup

Today, we are releasing the October 2017 [Preview of Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2016/10/11/net-framework-monthly-rollups-explained#preview-of-quality). This type of rollup is intended for businesses that want to the preview or use quality improvements as soon as they are available.

## Quality and Reliability

This release contains the following quality and reliability improvements.

#### CLR

* Code optimization bug for x64 C# code targeting .NET Framework 4.6.1 and running on .NET Framework 4.7. [484415]

#### WPF

* WPF touch stops working after many touch events due to reference counting issue. [460192]
* WPF touch generates a NullReferenceException in System.Windows.Input.StylusWisp.WispLogic.ProcessInputReport with .NET Framework 4.7. [480909]
* WPF crash caused by INVALID_POINTER_WRITE_c0000005_PenIMC_v0400.dll!CPimcContext::GetPenEventMultiple. [488390]

Note: Additional information on these improvements is not available. The [VSTS](https://www.visualstudio.com/team-services/) bug number provided with each improvement is a unique ID that you can give [Microsoft Customer Support](https://support.microsoft.com/contactus/), include in [StackOverflow comments](https://stackoverflow.com/questions/tagged/.net)or use in web searches.

## Getting the Update

The Preview of Quality Rollup is available via Windows Update, Windows Server Update Services, Microsoft Update Catalog, and Docker.

### Microsoft Update Catalog

You can get the update via the Microsoft Update Catalog. For Windows 10, .NET Framework updates are part of the Windows 10 Monthly Rollup.

<table>
<thead><tr>
<th>Product Version</th><th>Security and Quality Rollup KB</th>
</tr></thead>
<tr>
<td><strong>Windows 10 1607 (Anniversary Update)</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4042691">Catalog</a><BR><a href="https://support.microsoft.com/kb/4042691">4042691</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.7</td><td><a href="https://support.microsoft.com/kb/4042691">4042691</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6.2</td><td><a href="https://support.microsoft.com/kb/4042691">4042691</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6.1</td><td><a href="https://support.microsoft.com/kb/4042691">4042691</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6</td><td><a href="https://support.microsoft.com/kb/4042691">4042691</a></td>
</tr>
<tr>
<td><strong>Windows 8.1<BR>Windows RT 8.1<BR>Windows Server 2012 R2</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4042078">Catalog</a><BR><a href="https://support.microsoft.com/kb/4042078">4042078</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6, 4.6.1, 4.6.2, 4.7</td><td><a href="https://support.microsoft.com/kb/4041777">4041777</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.5, 4.5.1, 4.5.2</td><td><a href="https://support.microsoft.com/kb/4040974">4040974</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5.1</td><td><a href="https://support.microsoft.com/kb/4040981">4040981</a></td>
</tr>
<tr>
<td><strong>Windows Server 2012</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4042077">Catalog</a><BR><a href="https://support.microsoft.com/kb/4042077">4042077</a></strong></td>
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
<td><strong>Windows 7<BR>Windows Server 2008 R2</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4042076">Catalog</a><BR><a href="https://support.microsoft.com/kb/4042076">4042076</a></strong></td>
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
<td><strong>Windows Server 2008</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4042201">Catalog</a><BR><a href="https://support.microsoft.com/kb/4042201">4042201</a></strong></td>
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

### Known Issues

* [WPF crashes after the October 2017 Security and Monthly Quality Rollup is applied on Windows 10 version 1507 that has Microsoft .NET Framework 4.6.2 installed](https://support.microsoft.com/help/4048718).

### Previous Monthly Rollups

The last few .NET Framework Monthly updates are listed below for your convenience:

* [October 2017 Security and Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2017/10/10/net-framework-october-2017-security-and-quality-rollup/)
* [September 2017 Preview of Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2017/09/25/net-framework-september-2017-preview-of-quality-rollup/)
* [September 2017 Security and Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2017/09/12/net-framework-september-2017-security-and-quality-rollup/)
