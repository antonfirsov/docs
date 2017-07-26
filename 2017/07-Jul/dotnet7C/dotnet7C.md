# .NET Framework July 2017 Preview of Quality Rollup

We recently released a new Preview of Quality Rollup for the .NET Framework. 

[Preview of Quality Rollup releases](https://blogs.msdn.microsoft.com/dotnet/2016/10/11/net-framework-monthly-rollups-explained/) are recommended for businesses that want to use and/or preview quality improvements as soon as they become available. These same quality improvements will typically be included in the following [Security and Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2016/10/11/net-framework-monthly-rollups-explained/).

## Quality and Reliability

This release contains the following quality and reliability improvements.

### CLR

* Dynamic method compilation may fail due to out of memory if memory for the code is not available near its precode [394998, 394999]
* Memory leak in AppDomain::m_clsidHashMap growing [431586]
* EETypeHashTable lookups can deadlock with GC during comparison with unrestored persisted TypeHandles [435005, 435006]

### Data

* Performance improvements in Entity Framework [445511]
* System.Data.SqlClient: Errors due to undersized prelogin packet data buffer [445492]
* Handle leak with SqlConnection object [440114, 440116]

### Management

* Reboot method of Win32_OperatingSystem has Privilege not held exception [441902, 441903, 441904]

### Windows Forms

* MdiWindowListItem remove MDI from list after closing cancelation [396476]

### WPF

* Use after free/double free in PenIMC [429048]
* NullReferenceException thrown from PresentationFramework.dll during SpellCheck operations after SpellCheck.CustomDictionaries.Clear() is called programmatically [432174]
* Crash caused by HEAP_CORRUPTION_ACTIONABLE_BlockNotBusy_DOUBLE_FREE_c0000374_PenIMC.dll!CPimcTablet::ReleaseCursorInfo [429047]
* Crash caused by INVALID_POINTER_WRITE_c0000005_PenIMC.dll!CPimcContext::GetPenEventMultiple [429046]

### XML

* Incorrect Validation logic is applied when handling the &lt;xsd:whiteSpace value="collapse"/&gt; directive in System.Xml [227903, 440921, 440932]

## Getting the Update

The Security and Quality Rollup is available via Windows Update, Windows Server Update Services and Microsoft Update Catalog. 

### Docker Images

Docker images have not been updated for this release.

### Downloading KBs from Microsoft Update Catalog

You can download patches from the table below. See [.NET Framework Monthly Rollups Explained](https://blogs.msdn.microsoft.com/dotnet/2016/10/11/net-framework-monthly-rollups-explained/) for an explanation on how to use this table to download patches from Microsoft Update Catalog.

<table>
<thead><tr>
<th>Product Version</th><th>Preview of Quality Rollup KB</th>
</tr></thead>
<tr>
<td><strong>Windows 10 Update 1607 (Anniversary Update)<BR>Windows Server 2016</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4025334">Catalog</a><BR><a href="https://support.microsoft.com/kb/4025334">4025334</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6.2, 4.7</td><td><a href="https://support.microsoft.com/kb/4025334">4025334</a></td>
</tr>
<tr>
<td><strong>Windows 8.1<BR>Windows RT 8.1<BR>Windows Server 2012 R2</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4032115">Catalog</a><BR><a href="https://support.microsoft.com/kb/4032115">4032115</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5 (reship of May release)</td><td><a href="https://support.microsoft.com/kb/4014598">4014598</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.5.2</td><td><a href="https://support.microsoft.com/kb/4024843">4024843</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6, 4.6.1, 4.6.2, and 4.7</td><td><a href="https://support.microsoft.com/kb/4024847">4024847</a></td>
</tr>
<tr>
<td><strong>Windows Server 2012</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4032114">Catalog</a><BR><a href="https://support.microsoft.com/kb/4032114">4032114</a></strong></td>
</tr>
<tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5 (reship of May release)</td><td><a href="https://support.microsoft.com/kb/4014594">4014594</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.5.2</td><td><a href="https://support.microsoft.com/kb/4024844">4024844</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6, 4.6.1, 4.6.2, and 4.7</td><td><a href="https://support.microsoft.com/kb/4024846">4024846</a></td>
</tr>
<tr>
<td><strong>Windows 7<BR>Windows Server 2008 R2</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4032113">Catalog</a><BR><a href="https://support.microsoft.com/kb/4032113">4032113</a></strong></td>
</tr>
<tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5.1</td><td><a href="https://support.microsoft.com/kb/4014596">4014596</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.5.2</td><td><a href="https://support.microsoft.com/kb/4024845">4024845</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6, 4.6.1, 4.6.2, 4.7</td><td><a href="https://support.microsoft.com/kb/4024848">4024848</a></td>
</tr>
<tr>
<td><strong>Windows Server 2008</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4032116">Catalog</a><BR><a href="https://support.microsoft.com/kb/4032116">4032116</a></strong></td>
</tr>
<tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 2.0 (reship of May release)</td><td><a href="https://support.microsoft.com/kb/4014592">4014592</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.5.2</td><td><a href="https://support.microsoft.com/kb/4024845">4024845</a></td>
</tr>
<td style="padding-left:.5cm">.NET Framework 4.6</td><td><a href="https://support.microsoft.com/kb/4024848">4024848</a></td>
</tr>
</table>

### Previous Rollups and Updates

The last few .NET Framework Monthly updates are listed below for your convenience:

- [July 2017 Security and Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2017/07/11/net-framework-july-2017-security-and-quality-rollup/)
- [June 2017 Cumulative Quality Update for Windows 10](https://blogs.msdn.microsoft.com/dotnet/2017/06/27/net-framework-june-2017-cumulative-quality-update-for-windows-10/)
- [May 2017 Cumulative Quality Update for Windows 10](https://blogs.msdn.microsoft.com/dotnet/2017/05/26/net-framework-may-2017-cumulative-quality-update-for-windows-10/)
- [May 2017 Security and Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2017/05/09/net-framework-may-2017-monthly-rollup/)
