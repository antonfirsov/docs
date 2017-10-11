# .NET Framework September 2017 Preview of Quality Rollup

Today, we are releasing the September 2017 [Preview of Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2016/10/11/net-framework-monthly-rollups-explained#preview-of-quality). This type of rollup is intended for businesses that want to the preview or use quality improvements as soon as they are available.

## Quality and Reliability

This release contains the following quality and reliability improvements.

#### CLR

* Silent bad codegen when optimizing expression. [460765]
  * Also reported at [dotnet/coreclr #11574](https://github.com/dotnet/coreclr/issues/11574)
* Crash in Visual Studio due to race in CLR assembly loader. [462762]
* Runtime underallocates arrays by one element in rare cases when jitting large methods. [463604]
* AppContext feature opt-in/out not functioning correctly. [469020]
  * More information: [.NET Framework Update for AppContext](https://blogs.msdn.microsoft.com/dotnet/2017/08/18/net-framework-update-for-appcontext/)

#### Networking

* HTTPWebRequest times out when switching to TLS after installing update KB4019112. [465796]

#### Windows Forms

* Multi-Mon support: Controls with non-default anchoring are moved around the screen when scaling is changed [462872].

#### WPF

* WPF consumes high % of CPU in Visual Studio when console session not active. [391184]
* Application crash due to call into DWrite. [453529]

Note: Additional information on these improvements is not available. The [VSTS](https://www.visualstudio.com/team-services/) bug number provided with each improvement is a unique ID that you can give [Microsoft Customer Support](https://support.microsoft.com/contactus/), include in [StackOverflow comments](https://stackoverflow.com/questions/tagged/.net)or use in web searches.

## Getting the Update

The Preview of Quality Rollup is available via Windows Update, Windows Server Update Services and Microsoft Update Catalog.

### Downloading KBs from Microsoft Update Catalog

You can download patches from the table below. See [.NET Framework Monthly Rollups Explained](https://blogs.msdn.microsoft.com/dotnet/2016/10/11/net-framework-monthly-rollups-explained/) for an explanation on how to use this table to download patches from Microsoft Update Catalog.

<table>
<thead><tr>
<th>Product Version</th><th>Preview of Quality Rollup KB</th>
</tr></thead>
<tr>
<td><strong>Windows 10 Update 1607<BR>Windows Server 2016</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4039425">Catalog</a><BR><a href="https://support.microsoft.com/kb/4039425">4039425</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6.2 and 4.7</td><td><a href="https://support.microsoft.com/kb/4039425">4039425</a></td>
</tr>
</table>

### Previous Rollups and Updates

The last few .NET Framework Monthly updates are listed below for your convenience:

* [September 2017 Security and Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2017/09/12/net-framework-september-2017-security-and-quality-rollup/)
* [August 2017 Preview of Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2017/08/16/net-framework-august-2017-preview-of-quality-rollup/)
* [August 2017 Security and Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2017/08/08/net-framework-august-2017-security-and-quality-rollup/)
