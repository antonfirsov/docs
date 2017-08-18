# .NET Framework Update for AppContext Configuration

Today, we are releasing an update for the [AppContext class](https://docs.microsoft.com/dotnet/api/system.appcontext). The behavior of the AppContext class was recently regressed. The update returns the class to the correct behavior. This update affects the .NET Framework 4.6 and later. The update is not required on Windows 10.

The [AppContext class](https://docs.microsoft.com/dotnet/api/system.appcontext) was introduced in the [.NET Framework 4.6](https://blogs.msdn.microsoft.com/dotnet/2015/07/20/announcing-net-framework-4-6/#appcontext). It's primary use is to enable developers to opt into new behavior in the .NET Framework that is not enabled by default. The regression prevented developers from opting into the new behavior.

## Getting the Update

This update is available via the Microsoft Update Catalog. It will be made available in the regular broader releases over the next one to two months.

### Downloading KBs from Microsoft Update Catalog

You can download patches from the table below. See [.NET Framework Monthly Rollups Explained](https://blogs.msdn.microsoft.com/dotnet/2016/10/11/net-framework-monthly-rollups-explained/) for an explanation on how to use this table to download patches from Microsoft Update Catalog.

<table>
<thead><tr>
<th>Product Version</th><th>Preview of Quality Rollup KB</th>
</tr></thead>
<tr>
<td><strong>Windows 8.1<BR>Windows RT 8.1<BR>Windows Server 2012 R2</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4038922">Catalog</a><BR><a href="https://support.microsoft.com/kb/4038922">4038922</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6, 4.6.1, 4.6.2, 4.7</td><td><a href="https://support.microsoft.com/kb/4038922">4038922</a></td>
</tr>
<tr>
<td><strong>Windows Server 2012</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4038921">Catalog</a><BR><a href="https://support.microsoft.com/kb/4038921">4038921</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6, 4.6.1, 4.6.2, 4.7</td><td><a href="https://support.microsoft.com/kb/4038921">4038921</a></td>
</tr>
<tr>
<td><strong>Windows 7<BR>Windows Server 2008 R2 SP1</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4038923">Catalog</a><BR><a href="https://support.microsoft.com/kb/4038923">4038923</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6, 4.6.1, 4.6.2, 4.7</td><td><a href="https://support.microsoft.com/kb/4038923">4038923</a></td>
</tr>
<tr>
<td><strong>Windows Server 2008 SP2</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4038923">Catalog</a><BR><a href="https://support.microsoft.com/kb/4038923">4038923</a></strong></td>
</tr>
<tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6</td><td><a href="https://support.microsoft.com/kb/4038923">4038923</a></td>
</tr>
</table>

### Previous Rollups and Updates

The last few .NET Framework Monthly updates are listed below for your convenience:

* [August 2017 Preview of Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2017/08/16/net-framework-august-2017-preview-of-quality-rollup/)
* [August 2017 Security and Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2017/08/08/net-framework-august-2017-security-and-quality-rollup/)
* [July 2017 Cumulative Quality Update for Windows 10](https://blogs.msdn.microsoft.com/dotnet/2017/08/01/net-framework-july-2017-cumulative-quality-update-for-windows-10/)
* [July 2017 Quality Update for WPF](https://blogs.msdn.microsoft.com/dotnet/2017/07/25/net-framework-july-2017-quality-update)
