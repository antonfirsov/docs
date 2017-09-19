# .NET Framework September 2017 Security and Quality Rollup

Today, we are releasing the September 2017 Security and Quality Rollup and Security Only Update.

This update applies to Windows 7 and later client versions and Windows Server 2008 and later server versions.

## Security

This release contains the following security changes.

#### CVE-2017-8759 | .NET Framework Remote Code Execution Vulnerability

A remote code execution vulnerability exists when Microsoft .NET Framework processes untrusted input. An attacker who successfully exploited this vulnerability in software using the .NET framework could take control of an affected system. Users whose accounts are configured to have fewer user rights on the system could be less impacted than users who operate with administrative user rights.

To exploit the vulnerability, an attacker would first need to convince the user to open a malicious document or application. 
The security update addresses the vulnerability by correcting how .NET validates untrusted input.

More Information: [CVE-2017-8759](https://portal.msrc.microsoft.com/en-US/security-guidance/advisory/CVE-2017-8759)

## Quality and Reliability

This release contains the following quality and reliability improvements.

#### ASP.NET

* Values added to System.Web.Cache expire immediately, with .NET Framework 4.7. [452228]
  * Also reported at [ASP.NET Forums #2123507](https://forums.asp.net/t/2123507.aspx)
* ASP.NET site running on Sitefinity broken, with .NET Framework 4.7. [457739]

#### CLR

* CRWLock::StaticAcquireWriterLock() never returns if Int32.MaxValue number of ReaderWriterLock objects are created, with .NET Framework 3.5. [242568]
* Crash in CLR assembly metadata reader. [367294]
  * Also reported at [ASP.NET Forums #2106799](https://forums.asp.net/t/2106799.aspx)
  * Also reported at [StackOverflow #40272099](https://stackoverflow.com/questions/40272099/executionengineexception-80131506-in-mscorlib-dll-when-processing-aspscriptma/40532065)
  * Also reported at [Connect #3111237](https://connect.microsoft.com/VisualStudio/feedback/details/3111237/access-violation-exception-in-blobtoattributeset-instruction-in-clr-dll)
* .NET remoting IPC listener thread exits and leaves an orphaned IPCServerchannel. [454409]
* Silent bad codegen when optimizing expression. [460765]
  * Also reported at [dotnet/coreclr #11574](https://github.com/dotnet/coreclr/issues/11574)
* Crash in Visual Studio due to race in CLR assembly loader. [462762]
* Runtime underallocates arrays by one element in rare cases when jitting large methods. [463604]
* AppContext feature opt-in/out not functioning correctly. [469020]
  * More information: [.NET Framework Update for AppContext](https://blogs.msdn.microsoft.com/dotnet/2017/08/18/net-framework-update-for-appcontext/)

#### Management

* Reboot method of Win32_OperatingSystem has Privilege not held exception [441901]

#### Networking

* HTTPWebRequest times out when switching to TLS after installing update KB4019112. [465796]

#### WCF

* NetTcp with X509Certificates using SslStream uses the default TLS version as the OS, with .NET Framework 4.7. [451528]

#### Windows Forms

* Excessive object creation in a performance-critical code-path leading to performance regressions and/or displaying empty UI and/or exhausting GDI+ handles. [452048]
* Multi-Mon support: Controls with non-default anchoring are moved around the screen when scaling is changed [462872].
  * Note: This fix will be made available for Windows 10 1607 (Anniversary Update) in October.

#### WPF

* WPF fails to load resources if two versions of the same assembly are loaded. [378607]
  * Note: This fix will be made available for Windows 10 1703 (Creators Update) in October.
* WPF consumes high % of CPU in Visual Studio when console session not active. [391184]
  * Note: This fix will be made available for Windows 10 in October.
* Visual Studio fails due to “Unable to load DLL ‘PenIMC.dll'” error. [452476]
  * Note: This fix will be made available for Windows 10 1703 (Creators Update) in October.
* Application crash due to call into DWrite. [453529]
  * Note: This fix will be made available for Windows 10 in October.
* TargetFrameworkName is null with mixed mode application. [425074]
  * Note: This fix will be made available for Windows 10 1703 (Creators Update) in October.
* Event leak with WPF application on touch screen monitors on Windows 10. [434946]
  * Note: This fix will be made available for Windows 10 1703 (Creators Update) in October.

Note: Additional information on these improvements is not available. The [VSTS](https://www.visualstudio.com/team-services/) bug number provided with each improvement is a unique ID that you can give [Microsoft Customer Support](https://support.microsoft.com/contactus/), include in [StackOverflow comments](https://stackoverflow.com/questions/tagged/.net)or use in web searches.

## Getting the Update

The Security and Quality Rollup is available via Windows Update, Windows Server Update Services, Microsoft Update Catalog, and Docker.

### Microsoft Update Catalog

You can get the update via the Microsoft Update Catalog. For Windows 10, .NET Framework updates are part of the Windows 10 Monthly Rollup.

<table>
<thead><tr>
<th>Product Version</th><th>Security and Quality Rollup KB</th>
<th>Security Rollup KB</th>
</tr></thead>
<tr>
<td><strong>Windows 10 1703 (Creators Update)</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4038788">Catalog</a><BR><a href="https://support.microsoft.com/kb/4038788">4038788</a></strong></td><td>N/A</td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.7</td><td><a href="https://support.microsoft.com/kb/4038788">4038788</a></td><td>N/A</td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5</td><td><a href="https://support.microsoft.com/kb/4038788">4038788</a></td><td>N/A</td>
</tr>
<tr>
<td><strong>Windows 10 1607 (Anniversary Update)<BR>Windows Server 2016</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4038782">Catalog</a><BR><a href="https://support.microsoft.com/kb/4038782">4038782</a></strong></td><td>N/A</td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6.2, 4.7</td><td><a href="https://support.microsoft.com/kb/4038782">4038782</a></td><td>N/A</td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5</td><td><a href="https://support.microsoft.com/kb/4038782">4038782</a></td><td>N/A</td>
</tr>
<tr>
<td><strong>Windows 10 1511</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4038783">Catalog</a><BR><a href="https://support.microsoft.com/kb/4038783">4038783</a></strong></td><td>N/A</td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6.1</td><td><a href="https://support.microsoft.com/kb/4038783">4038783</a></td><td>N/A</td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5</td><td><a href="https://support.microsoft.com/kb/4038783">4038783</a></td><td>N/A</td>
</tr>
<tr>
<td><strong>Windows 10 1507</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4038781">Catalog</a><BR><a href="https://support.microsoft.com/kb/4038781">4038781</a></strong></td><td>N/A</td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6</td><td><a href="https://support.microsoft.com/kb/4038781">4038781</a></td><td>N/A</td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5</td><td><a href="https://support.microsoft.com/kb/4038781">4038781</a></td><td>N/A</td>
</tr>
<tr>
<td><strong>Windows 8.1<BR>Windows RT 8.1<BR>Windows Server 2012 R2</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4041085">Catalog</a><BR><a href="https://support.microsoft.com/kb/4041085">4041085</a></strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4041092">Catalog</a><BR><a href="https://support.microsoft.com/kb/4041092">4041092</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5</td><td><a href="https://support.microsoft.com/kb/4040981">4040981</a></td><td><a href="https://support.microsoft.com/kb/4040967">4040967</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.5.2</td><td><a href="https://support.microsoft.com/kb/4040974">4040974</a></td><td><a href="https://support.microsoft.com/kb/4040958">4040958</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6, 4.6.1, 4.6.2, 4.7</td><td><a href="https://support.microsoft.com/kb/4040972">4040972</a></td><td><a href="https://support.microsoft.com/kb/4040956">4040956</a></td>
</tr>
<tr>
<td><strong>Windows Server 2012 R2</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4041084">Catalog</a><BR><a href="https://support.microsoft.com/kb/4041084">4041084</a></strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4041091">Catalog</a><BR><a href="https://support.microsoft.com/kb/4041091">4041091</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5</td><td><a href="https://support.microsoft.com/kb/4040979">4040979</a></td><td><a href="https://support.microsoft.com/kb/4040965">4040965</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.5.2</td><td><a href="https://support.microsoft.com/kb/4040975">4040975</a></td><td><a href="https://support.microsoft.com/kb/4040959">4040959</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6</td><td><a href="https://support.microsoft.com/kb/4040971">4040971</a></td><td><a href="https://support.microsoft.com/kb/4040955">4040955</a></td>
</tr>
<tr>
<td><strong>Windows 7<BR>Windows Server 2008 R2</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4041083">Catalog</a><BR><a href="https://support.microsoft.com/kb/4041083">4041083</a></strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4041090">Catalog</a><BR><a href="https://support.microsoft.com/kb/4041090">4041090</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5.1</td><td><a href="https://support.microsoft.com/kb/4040980">4040980</a></td><td><a href="https://support.microsoft.com/kb/4040966">4040966</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.5.2</td><td><a href="https://support.microsoft.com/kb/4040977">4040977</a></td><td><a href="https://support.microsoft.com/kb/4040960">4040960</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6, 4.6.1, 4.6.2, 4.7</td><td><a href="https://support.microsoft.com/kb/4040973">4040973</a></td><td><a href="https://support.microsoft.com/kb/4040957">4040957</a></td>
</tr>
<tr>
<td><strong>Windows Server 2008</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4041086">Catalog</a><BR><a href="https://support.microsoft.com/kb/4041086">4041086</a></strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4041093">Catalog</a><BR><a href="https://support.microsoft.com/kb/4041093">4041093</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 2.0</td><td><a href="https://support.microsoft.com/kb/4040978">4040978</a></td><td><a href="https://support.microsoft.com/kb/4040964">4040964</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.5.2</td><td><a href="https://support.microsoft.com/kb/4040977">4040977</a></td><td><a href="https://support.microsoft.com/kb/4040960">4040960</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6</td><td><a href="https://support.microsoft.com/kb/4040973">4040973</a></td><td><a href="https://support.microsoft.com/kb/4040957">4040957</a></td>
</tr>
</table>

### Docker Images

Docker images has not yet been updated as part of today's release. They will be updated in the shortly. This post will be updated at that time.

### Previous Monthly Rollups

The last few .NET Framework Monthly updates are listed below for your convenience:

* [August 2017 Preview of Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2017/08/16/net-framework-august-2017-preview-of-quality-rollup/)
* [August 2017 Security and Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2017/08/08/net-framework-august-2017-security-and-quality-rollup/)
* [July 2017 Cumulative Quality Update for Windows 10](https://blogs.msdn.microsoft.com/dotnet/2017/08/01/net-framework-july-2017-cumulative-quality-update-for-windows-10/)
* [July 2017 Quality Update for WPF](https://blogs.msdn.microsoft.com/dotnet/2017/07/25/net-framework-july-2017-quality-update)
* [July 2017 Preview of Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2017/07/24/net-framework-july-2017-preview-of-quality-rollup/)
* [July 2017 Security and Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2017/07/11/net-framework-july-2017-security-and-quality-rollup/)