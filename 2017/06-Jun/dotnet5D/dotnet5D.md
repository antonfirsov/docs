# .NET Framework June 2017 Cumulative Quality Update for Windows 10

Today, we are releasing a new Cumulative Quality Update for the .NET Framework. It is specific to Windows 10 Creators Update (1703). 

Previously released security and quality updates are included in this release, including the [NET Framework May 2017 Security and Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2017/05/09/net-framework-may-2017-monthly-rollup/) and the [May 2017 Cumulative Quality Update for Windows 10](https://blogs.msdn.microsoft.com/dotnet/2017/05/26/net-framework-may-2017-cumulative-quality-update-for-windows-10/). There was no Security and Quality Rollup released in June.

## Security

This release contains no new security improvements. 

## Quality and Reliability

The following improvements are included in this release.

### Windows Presentation Framework

**Issues 429046**

Resolves a reliability issue where a PimcContext is incorrectly used after it is released. 

This issue affects Visual Studio 2017. You are encouraged to install this updated if you use Visual Studio 2017.

**Issues 429047**

Resolves a reliability issue where a failure to query a tablet cursor is incorrectly handled from the WISP component.

**Issues 429048**

Resolves a reliability issue where a PenContext is incorrectly used after it is released.

## Getting the Update

The May 2017 Cumulative Quality Update is available via Windows Update, Windows Server Update Services and Microsoft Update Catalog. 

### Docker Images

The [Windows ServerCore](https://hub.docker.com/r/microsoft/windowsservercore/) and [.NET Framework](https://hub.docker.com/r/microsoft/dotnet-framework/) images have not been updated for this release.

### Downloading KBs from Microsoft Update Catalog

You can download patches from the table below. See [.NET Framework Monthly Rollups Explained](https://blogs.msdn.microsoft.com/dotnet/2016/10/11/net-framework-monthly-rollups-explained/) for an explanation on how to use this table to download patches from Microsoft Update Catalog.

<table>
<thead><tr>
<th>Product Version</th><th>Cumulative Quality Update KB</th>
</tr></thead>
<tr>
<td><strong>Windows 10 Creators Update</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4022723">Catalog</a><BR><a href="https://support.microsoft.com/kb/4022723">4022723</a></strong></td>
</tr>
<tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.7</td><td><a href="https://support.microsoft.com/kb/4022723">4022723</a></td>
</tr>
</table>

### Previous Monthly Rollups

The last few .NET Framework Monthly Rollups are listed below for your convenience:

- [May 2017 Cumulative Quality Update for Windows 10](https://blogs.msdn.microsoft.com/dotnet/2017/05/26/net-framework-may-2017-cumulative-quality-update-for-windows-10/)
- [May 2017 Preview of Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2017/05/17/net-framework-may-2017-preview-of-quality-rollup/)
- [May 2017 Security and Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2017/05/09/net-framework-may-2017-monthly-rollup/)
- [April 2017 Security and Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2017/04/11/net-framework-april-2017-monthly-rollup/)

### More Information

You can read the [.NET Framework Monthly Rollups Explained](https://blogs.msdn.microsoft.com/dotnet/2016/10/11/net-framework-monthly-rollups-explained/) to learn more about how the .NET Framework is updated.