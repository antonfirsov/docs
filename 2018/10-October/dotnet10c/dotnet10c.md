# .NET Framework October 2018 Preview Quality Rollup

Today, we are releasing the October 2018 Preview of Quality Rollup.

## Quality and Reliability

This release contains the following quality and reliability improvements.

### CLR

* Updates Japanese dates that are formatted for the first year in an era and for which the format pattern uses “y年”. The format of the year together with the symbol "元" is supported instead of using year number 1. Also, formatting day numbers that include "元" is supported. [646179]
* Updating Venezuela currency information.
	•currency symbol changed to “Bs.S”
	•English currency name is changed to “Bolívar Soberano”
	•Native Currency name is changed to “bolívar soberano”
	•Intl Currency Code changed to “VES”
This will affect the culture of “es-VE” [616146]
* Address a situation where the System.Security.Cryptography.Algorithms reference was not correctly loaded on .NET Framework 4.7.1 after the 7B/8B patch. [673870]

### WF
* In some .NET Remoting scenarios, when using TransactionScopeAsyncFlowOption.Enabled, it was possible to have Transaction.Current reset to null after a remoting call. [669153]

### WPF

* Addressed an issue where application created numerous Windows Forms textboxes to a flowLayoutPanel, with only a few calls to comctl32.dll. [638365]
* Addressed a race condition involving temporary files and some anti-virus scanners.  This was causing crashes with the message "The process cannot access the file <name of temp file>". [638468]
* Addressed a crash due to TaskCanceledException that can occur during shutdown of some WPF apps.   Apps that continue to do work involving weak events or data binding after Application.Run() returns are known to be vulnerable to this crash. [655427]

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
<td><strong>Windows 10 1709 (Fall Creators Update)</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4459931">Catalog</a><BR><a href="https://support.microsoft.com/kb/4459931">4459931</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5, 4.7.1, 4.7.2</td><td><a href="https://support.microsoft.com/kb/4459931">4459931</a></td>
</tr>
<tr>
<td><strong>Windows 10 1703 (Creators Update)</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4459930">Catalog</a><BR><a href="https://support.microsoft.com/kb/4459930">4459930</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5, 4.7, 4.7.1, 4.7.2</td><td><a href="https://support.microsoft.com/kb/4459930">4459930</a></td>
</tr>
<tr>
<td><strong>Windows 10 1607 (Anniversary Update)</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4459929">Catalog</a><BR><a href="https://support.microsoft.com/kb/4459929">4459929</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5, 4.6.2, 4.7, 4.7.1, 4.7.2</td><td><a href="https://support.microsoft.com/kb/4459929">4459929</a></td>
</tr>
</table>

The following table is for earlier Windows and Windows Server versions.

<table>
<thead><tr>
<th>Product Version</th><th>Securty and Quality Rollup KB</th>
</tr></thead>
<tr>
<td><strong>Windows 8.1<BR>Windows RT 8.1<BR>Windows Server 2012 R2</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4462502">Catalog</a><BR><a href="https://support.microsoft.com/kb/4462502">4462502</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5</td><td><a href="https://support.microsoft.com/kb/4459935">4459935</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.5.2</td><td><a href="https://support.microsoft.com/kb/4459943">4459943</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6, 4.6.1, 4.6.2, 4.7, 4.7.1, 4.7.1, 4.7.2</td><td><a href="https://support.microsoft.com/kb/4459941">4459941</a></td>
</tr>
<tr>
<td><strong>Windows Server 2012</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4462501">Catalog</a><BR><a href="https://support.microsoft.com/kb/4462501">4462501</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5</td><td><a href="https://support.microsoft.com/kb/4459932">4459932</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.5.2</td><td><a href="https://support.microsoft.com/kb/4459944">4459944</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6, 4.6.1, 4.6.2, 4.7, 4.7.1, 4.7.1, 4.7.2</td><td><a href="https://support.microsoft.com/kb/4459940">4459940</a></td>
</tr>
<tr>
<td><strong>Windows 7<BR>Windows Server 2008 R2</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4462500">Catalog</a><BR><a href="https://support.microsoft.com/kb/4462500">4462500</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5.1</td><td><a href="https://support.microsoft.com/kb/4459934">4459934</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.5.2</td><td><a href="https://support.microsoft.com/kb/4459945">4459945</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6, 4.6.1, 4.6.2, 4.7, 4.7.1, 4.7.1, 4.7.2</td><td><a href="https://support.microsoft.com/kb/4459942">4459942</a></td>
</tr>
<tr>
<td><strong>Windows Server 2008</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4462503">Catalog</a><BR><a href="https://support.microsoft.com/kb/4462503">4462503</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 2.0, 3.0</td><td><a href="https://support.microsoft.com/kb/4459933">4459933</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.5.2</td><td><a href="https://support.microsoft.com/kb/4459945">4459945</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6</td><td><a href="https://support.microsoft.com/kb/4459942">4459942</a></td>
</tr>
</table>

### Previous Monthly Rollups

The last few .NET Framework Monthly updates are listed below for your convenience:

* [October 2018 Security and Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2018/10/09/net-framework-october-2018-security-and-quality-rollup/)
* [September 2018 Preview of Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2018/09/25/net-framework-september-2018-preview-of-quality-rollup/)
* [September 2018 Security and Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2018/09/11/net-framework-september-2018-security-and-quality-rollup/)

