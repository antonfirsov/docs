# .NET Framework August 2017 Preview of Quality Rollup

Today, we are releasing the August 2017 [Preview of Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2016/10/11/net-framework-monthly-rollups-explained#preview-of-quality).

## Quality and Reliability

This release contains the following quality and reliability improvements.

### ASP.NET

* Values added to System.Web.Cache expire immediately, with .NET Framework 4.7. [452228]
  * Also reported at [ASP.NET Forums #2123507](https://forums.asp.net/t/2123507.aspx)
* ASP.NET site running on Sitefinity broken, with .NET Framework 4.7. [457739]

### CLR

* CRWLock::StaticAcquireWriterLock() never returns if Int32.MaxValue number of ReaderWriterLock objects are created, with .NET Framework 3.5. [242568]
* Crash in Visual Studio due to race in CLR assembly loader. [462762]
* .NET remoting IPC listener thread exits and leaves an orphaned IPCServerchannel. [454409]
* Crash in CLR assembly metadata reader. [367294]
  * Also reported at [ASP.NET Forums #2106799](https://forums.asp.net/t/2106799.aspx)
  * Also reported at [StackOverflow #40272099](https://stackoverflow.com/questions/40272099/executionengineexception-80131506-in-mscorlib-dll-when-processing-aspscriptma/40532065)
  * Also reported at [Connect #3111237](https://connect.microsoft.com/VisualStudio/feedback/details/3111237/access-violation-exception-in-blobtoattributeset-instruction-in-clr-dll)

### Management

* Reboot method of Win32_OperatingSystem has Privilege not held exception [441901]

### Windows Forms

* Excessive object creation in a performance-critical code-path leading to performance regressions and/or displaying empty UI and/or exhausting GDI+ handles. [452048]

### WCF

* NetTcp with X509Certificates using SslStream uses the default TLS version as the OS, with .NET Framework 4.7. [451528]

### WPF

* Handle(event) leaks with WPF application on touch screen monitors. [398137]
* TargetFrameworkName is null with mixed mode application. [425074]
* Visual Studio fails due to "Unable to load DLL 'PenIMC.dll'" error. [452476]
* WPF fails to load resources if two versions of the same assembly are loaded. [378607]

Note: Additional information on these improvements is not available. The [VSTS](https://www.visualstudio.com/team-services/) bug number provided with each improvement is a unique ID that you can give [Microsoft Customer Support](https://support.microsoft.com/contactus/), include in [StackOverflow comments](https://stackoverflow.com/questions/tagged/.net) or use in web searches. 

## Getting the Update

The Preview of Quality Rollup is available via Windows Update, Windows Server Update Services and Microsoft Update Catalog.

### Downloading KBs from Microsoft Update Catalog

You can download patches from the table below. See [.NET Framework Monthly Rollups Explained](https://blogs.msdn.microsoft.com/dotnet/2016/10/11/net-framework-monthly-rollups-explained/) for an explanation on how to use this table to download patches from Microsoft Update Catalog.

<table>
<thead><tr>
<th>Product Version</th><th>Preview of Quality Rollup KB</th>
</tr></thead>
<tr>
<td><strong>Windows 10 Update 1607<BR>Windows Server 2016</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4034661">Catalog</a><BR><a href="https://support.microsoft.com/kb/4034661">4034661</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6.2 and 4.7</td><td><a href="https://support.microsoft.com/kb/4034661">4034661</a></td>
</tr>
<tr>
<td><strong>Windows 8.1<BR>Windows RT 8.1<BR>Windows Server 2012 R2</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4035038">Catalog</a><BR><a href="https://support.microsoft.com/kb/4035038">4035038</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5</td><td><a href="https://support.microsoft.com/kb/4033997">4033997</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.5.2</td><td><a href="https://support.microsoft.com/kb/4033991">4033991</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6, 4.6.1, 4.6.2, 4.7</td><td><a href="https://support.microsoft.com/kb/4033989">4033989</a></td>
</tr>
<tr>
<td><strong>Windows Server 2012</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4035037">Catalog</a><BR><a href="https://support.microsoft.com/kb/4035037">4035037</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5</td><td><a href="https://support.microsoft.com/kb/4033995">4033995</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.5.2</td><td><a href="https://support.microsoft.com/kb/4033992">4033992</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6, 4.6.1, 4.6.2, 4.7</td><td><a href="https://support.microsoft.com/kb/4033988">4033988</a></td>
</tr>
<tr>
<td><strong>Windows 7<BR>Windows Server 2008 R2</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4035036">Catalog</a><BR><a href="https://support.microsoft.com/kb/4035036">4035036</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5.1</td><td><a href="https://support.microsoft.com/kb/4033996">4033996</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.5.2</td><td><a href="https://support.microsoft.com/kb/4033993">4033993</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6, 4.6.1, 4.6.2, 4.7</td><td><a href="https://support.microsoft.com/kb/4033990">4033990</a></td>
</tr>
<tr>
<td><strong>Windows Server 2008</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4035039">Catalog</a><BR><a href="https://support.microsoft.com/kb/4035039">4035039</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 2.0</td><td><a href="https://support.microsoft.com/kb/4033994">4033994</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.5.2</td><td><a href="https://support.microsoft.com/kb/4033993">4033993</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6</td><td><a href="https://support.microsoft.com/kb/4033990">4033990</a></td>
</tr>
</table>

## Update Notes

The `File Version` of the following files did not change in this update.

* System.Management.dll
* WMINet_Utils.dll

It is typical for both `File Version` and `Date Modified` fields to be unique in each update. The unique values helps for identification of patches.

You can see these fields in the file properties for System.Management.dll, for example, in the following image.

![File Properties for System.Management.dll](https://user-images.githubusercontent.com/2608468/29130468-ca498900-7cde-11e7-89ec-5a7c1bda8915.png)

### Previous Rollups and Updates

The last few .NET Framework Monthly updates are listed below for your convenience:

* [August 2017 Security and Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2017/08/08/net-framework-august-2017-security-and-quality-rollup/)
* [July 2017 Cumulative Quality Update for Windows 10](https://blogs.msdn.microsoft.com/dotnet/2017/08/01/net-framework-july-2017-cumulative-quality-update-for-windows-10/)
* [July 2017 Quality Update for WPF](https://blogs.msdn.microsoft.com/dotnet/2017/07/25/net-framework-july-2017-quality-update)
* [July 2017 Preview of Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2017/07/24/net-framework-july-2017-preview-of-quality-rollup/)
* [July 2017 Security and Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2017/07/11/net-framework-july-2017-security-and-quality-rollup/)
