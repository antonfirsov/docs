# .NET Framework May 2017 Cumulative Quality Update for Windows 10

Today, we are releasing a new Cumulative Quality Update for the .NET Framework. It is specific to Windows 10 Creators Update. 

Previously released security and quality updates are included in this release.

## Security

This release contains the security improvements included in the [NET Framework May 2017 Security and Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2017/05/09/net-framework-may-2017-monthly-rollup/).

## Quality and Reliability

The following improvements are included in this release.

### Common Language Runtime

**Issue 426799**

Some applications using the managed C++ memcpy intrinsic could execute incorrectly. The JIT compiler when optimizing code that contains a C++ memcpy intrinsic could generate incorrect code.

**Issue 426795**

Some applications terminate with an A/V in clrjit.dll.The JIT compiler when optimizing a block of unreachable code could hit a A/V which results in the unexpected early termination of the application.


### Windows Presentation Framework

**Issue 373366**

If two WPF applications that target Side by Side (SxS) .NET versions (3.5 and 4.X) are loaded in the same process issues can occur on touch/stylus enabled machines.  A common example of this is loading VSTO add-ins written in WPF.  This is due to an issue with choosing the correct PenIMC.dll version for each application.  This fix allows WPF to properly differentiate between both DLLs and function correctly.

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
<td><strong>Windows 10 Creators Update</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4020102">Catalog</a><BR><a href="https://support.microsoft.com/kb/4020102">4020102</a></strong></td>
</tr>
<tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.7</td><td><a href="https://support.microsoft.com/kb/4020102">4020102</a></td>
</tr>
</table>

### Previous Monthly Rollups

The last few .NET Framework Monthly Rollups are listed below for your convenience:

- [May 2017 December 2016 Preview of Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2017/05/17/net-framework-may-2017-preview-of-quality-rollup/)
- [May 2017 December 2016 Security and Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2017/05/09/net-framework-may-2017-monthly-rollup/)
- [April 2017 December 2016 Security and Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2017/04/11/net-framework-april-2017-monthly-rollup/)

### More Information

You can read the [.NET Framework Monthly Rollups Explained](https://blogs.msdn.microsoft.com/dotnet/2016/10/11/net-framework-monthly-rollups-explained/) to learn more about how the .NET Framework is updated.