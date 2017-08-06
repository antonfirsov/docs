# .NET Framework July 2017 Cumulative Quality Update for Windows 10

Today, we are releasing a new Cumulative Quality Update for the .NET Framework. It is specific to Windows 10 build 1703 (Creators Update). 

Previously released security and quality updates are included in this release, including the [July 2017 Security and Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2017/07/11/net-framework-july-2017-security-and-quality-rollup/), the [July 2017 Preview of Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2017/07/24/net-framework-july-2017-preview-of-quality-rollup/) and the [June 2017 Cumulative Quality Update for Windows 10](https://blogs.msdn.microsoft.com/dotnet/2017/06/27/net-framework-june-2017-cumulative-quality-update-for-windows-10/).

Note: This release is considered a July release even though it is being released in August.

## Quality and Reliability

The following improvements are included in this release.

#### ASP.NET

* .NET Session State throttling change request when several requests for the same session object arrive on the server, with .NET Framework 3.5 [447143].

#### CLR

* CRWLock::StaticAcquireWriterLock() never returns if Int32.MaxValue number of ReaderWriterLock objects are created, with .NET Framework 3.5 [242568].
* Dynamic method compilation may fail due to out of memory if memory for the code is not available near its precode [394998].
* Memory leak in AppDomain::m_clsidHashMap [431586].
* EETypeHashTable lookups can deadlock with GC during comparison with unrestored persisted TypeHandles [435006]


#### Data

* SqlConnection object memory leak, with .NET Framework 4.7 [444016].
* System.Data.SqlClient: Errors due to undersized prelogin packet data buffer [445492].
* Performance improvements in Entity Framework [445511].

#### Management

* Reboot method of Win32_OperatingSystem has Privilege not held exception [441904].

#### Windows forms

* MdiWindowListItem remove MDI from list after closing cancellation [396476].
* Excessive object creation in a performance-critical code-path leading to performance regressions and/or displaying empty UI and/or exhausting GDI+ handles [452048].

#### Windows Presentation Foundation (WPF)

* NullReferenceException thrown from PresentationFramework.dll during SpellCheck operations after SpellCheck.CustomDictionaries.Clear() is called programmatically [432174].

#### XML

* Incorrect Validation logic is applied when handling the <xsd:whiteSpace value=”collapse”/> directive in System.Xml [227903, 440921, 440932]

## Getting the Update

The July 2017 Cumulative Quality Update is available via Windows Update, Windows Server Update Services and Microsoft Update Catalog. 

### Docker Images

The [.NET Framework](https://hub.docker.com/r/microsoft/dotnet-framework/) Docker images have not been updated for this release.

### Downloading KBs from Microsoft Update Catalog

You can download patches from the table below. See [.NET Framework Monthly Rollups Explained](https://blogs.msdn.microsoft.com/dotnet/2016/10/11/net-framework-monthly-rollups-explained/) for an explanation on how to use this table to download patches from Microsoft Update Catalog.

<table>
<thead><tr>
<th>Product Version</th><th>Cumulative Quality Update KB</th>
</tr></thead>
<tr>
<td><strong>Windows 10 Creators Update</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4032188">Catalog</a><BR><a href="https://support.microsoft.com/kb/4032188">4032188</a></strong></td>
</tr>
<tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.7</td><td><a href="https://support.microsoft.com/kb/4032188">4032188</a></td>
</tr>
</table>

### Previous Rollups and Updates

The last few .NET Framework Monthly updates are listed below for your convenience:

- [July 2017 Quality Update for WPF](https://blogs.msdn.microsoft.com/dotnet/2017/07/25/net-framework-july-2017-quality-update)
- [July 2017 Preview of Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2017/07/24/net-framework-july-2017-preview-of-quality-rollup/)
- [July 2017 Security and Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2017/07/11/net-framework-july-2017-security-and-quality-rollup/)
- [June 2017 Cumulative Quality Update for Windows 10](https://blogs.msdn.microsoft.com/dotnet/2017/06/27/net-framework-june-2017-cumulative-quality-update-for-windows-10/)
