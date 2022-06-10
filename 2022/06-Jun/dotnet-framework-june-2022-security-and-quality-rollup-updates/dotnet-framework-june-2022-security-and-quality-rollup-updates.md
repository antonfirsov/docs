---
post_title: .NET Framework June 2022 Security and Quality Rollup Updates
author1: salagarw
post_slug: 
username: salagarw
microsoft_alias: salagarw
featured_image: blogpst-NET.png
categories: .NET Framework, WinForms, WPF
summary: June 2022 Security and Quality Rollup Updates for .NET Framework
desired_publication_date: 2022-06-14
---

We are releasing the June 2022 Security and Quality Rollup Updates for .NET Framework.  


<h3> Security</h3>
The June Security and Quality Rollup Update does not contain any new security fixes. See <a href="https://devblogs.microsoft.com/dotnet/framework-may-2022-updates/" rel="nofollow">May 2022 Security and Quality Rollup</a> for the latest security updates.    


<h3> Quality and Reliability</h3>

This release contains the following quality and reliability improvements.

<h5> NET Runtime<span style="font-size: 12pt;"></span></h5>
<ul>
    <li>Addresses several issues that would cause too many garbage collections under high memory load. The part of the change that reduces the number of blocking generation 2 collections under high memory load is considered a tuning change and is only active if the GCConserveMemory setting is set to a non-zero value. The part of the change that reduces needless generation 0 collections is considered an improvement and is always active.</li>
    <li>Adjusted GC Heap Hard Limit configuration, as well as processor interpretation for .NET Framework container scenarios.</li>
</ul>
	

<h5> WPF<span style="font-size: 12pt;"><sup>1</sup></span></h5>
<ul>
    <li> Addresses an issue where DWM failures can cause WPF's render thread to fail. An app can opt-in to the behavior of ignoring all DwmFlush errors by setting a regkey in HKCU\Software\Microsoft\Avalon.Graphics\IgnoreDwmFlushErrors or HKLM\Software\Microsoft\Avalon.Graphics\IgnoreDwmFlushErrors whose name is the full path to the .exe that wants to opt-in, and whose DWORD value is 1.</li>
    <li>Addresses an issue of WPF apps not working with "Text Cursor Indicator" enabled when using RichTextBox.</li>
</ul>

<h5> Winforms<span style="font-size: 12pt;"></span></h5>
<ul>
    <li>Improved the hardened rendering of ComboBox controls on 64 bit architectures.</li>
    <li>Improved the reliability of data-bound ComboBox controls under assistive technology.</li>
</ul>

<h5> Workflow<span style="font-size: 12pt;"></span></h5>
<ul>
    <li>Addresses an issue when users interact with the Workflow Designer they might encounter incorrectly disabled context menu items when right clicking on a variable in the component variables list.</li>
</ul>
<sup>1 </sup>Windows Presentation Foundation (WPF)


<h3> <a id="user-content-getting-the-update" class="anchor" href="#getting-the-update"></a>Getting the Update</h3>

The Security and Quality Rollup is available via Windows Update, Windows Server Update Services, and Microsoft Update Catalog.

<h5> <a id="user-content-microsoft-update-catalog" class="anchor" href="#microsoft-update-catalog"></a>Microsoft Update Catalog</h5>

You can get the update via the Microsoft Update Catalog. For Windows 10, NET Framework 4.8 updates are available via Windows Update, Windows Server Update Services, Microsoft Update Catalog. Updates for other versions of .NET Framework are part of the Windows 10 Monthly Cumulative Update.

**Note**: Customers that rely on Windows Update and Windows Server Update Services will automatically receive the .NET Framework version-specific updates. Advanced system administrators can also take use of the below direct Microsoft Update Catalog download links to .NET Framework-specific updates. Before applying these updates, please ensure that you carefully review the .NET Framework version applicability, to ensure that you only install updates on systems where they apply. 


The following table is for Windows 10 and Windows Server 2016+ versions.  
<table border="1" cellspacing="0" cellpadding="8px">
  <thead>
    <tr>
      <th>
        Product Version
      </th>
      <th colspan="2">
        Cumulative Update
      </th>
    </tr>
  </thead>
  <tbody>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows 11</strong>
      </td>
      <td></td>
      <td></td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5013889" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5013889" rel="nofollow">5013889</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Microsoft server operating systems version 21H2</strong>
      </td>
      <td></td>
      <td></td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5013890" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5013890" rel="nofollow">5013890</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows 10 21H2</strong>
      </td>
      <td></td>
      <td></td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5013887" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5013887" rel="nofollow">5013887</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows 10 21H1</strong>
      </td>
      <td></td>
      <td></td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5013887" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5013887" rel="nofollow">5013887</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows 10, version 20H2 and Windows Server, version 20H2</strong>
      </td>
      <td></td>
      <td></td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5013887" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5013887" rel="nofollow">5013887</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows 10 1809 (October 2018 Update) and Windows Server 2019</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5014805" rel="nofollow">5014805</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.7.2
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5013892" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5013892" rel="nofollow">5013892</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5013888" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5013888" rel="nofollow">5013888</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows 10 1607 (Anniversary Update) and Windows Server 2016</strong>
      </td>
      <td></td>
      <td></td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.6.2, 4.7, 4.7.1, 4.7.2
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5014702" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5014702" rel="nofollow">5014702</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5014630" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5014630" rel="nofollow">5014630</a>
      </td>
    </tr>
  </tbody>
</table>


The following table is for earlier Windows and Windows Server versions.  
<table border="1" cellspacing="0" cellpadding="8px">
  <thead>
    <tr>
      <th>
        Product Version
      </th>
      <th colspan="2">
        Security and Quality Rollup
      </th>
    </tr>
  </thead>
  <tbody>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows 8.1, Windows RT 8.1 and Windows Server 2012 R2</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5014808" rel="nofollow">5014808</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5013638" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5013638" rel="nofollow">5013638</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 4.6.2, 4.7, 4.7.1, 4.7.2
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5014637" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5014637" rel="nofollow">5014637</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5014633" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5014633" rel="nofollow">5014633</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows Server 2012</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5014807" rel="nofollow">5014807</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5013635" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5013635" rel="nofollow">5013635</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 4.6.2, 4.7, 4.7.1, 4.7.2
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5014636" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5014636" rel="nofollow">5014636</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5014632" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5014632" rel="nofollow">5014632</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows 7 SP1 and Windows Server 2008 R2 SP1</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5014806" rel="nofollow">5014806</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5.1
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5013637" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5013637" rel="nofollow">5013637</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 4.6.2, 4.7, 4.7.1, 4.7.2
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5014635" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5014635" rel="nofollow">5014635</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5014631" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5014631" rel="nofollow">5014631</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows Server 2008</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5014809" rel="nofollow">5014809</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 2.0, 3.0
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5013636" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5013636" rel="nofollow">5013636</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 4.6.2
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5014635" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5014635" rel="nofollow">5014635</a>
      </td>
    </tr>
  </tbody>
</table>


<br/>    
<h5> Previous Monthly Rollups </h5>
The last few .NET Framework Monthly updates are listed below for your convenience:  

<ul>
	<li><a href="https://devblogs.microsoft.com/dotnet/net-framework-may-2022-cumulative-update/" rel="nofollow">.NET Framework May 2022 Cumulative Update</a></li>
	<li><a href="https://devblogs.microsoft.com/dotnet/framework-may-2022-updates/" rel="nofollow">.NET Framework May 2022 Security and Quality Rollup</a></li>
	<li><a href="https://devblogs.microsoft.com/dotnet/framework-april-2022-updates/" rel="nofollow">.NET Framework April 2022 Cumulative Update </a></li>
    <li><a href="https://devblogs.microsoft.com/dotnet/dotnet-framework-april-2022-updates/" rel="nofollow">.NET Framework April 2022 Security and Quality Rollup Updates</a></li>
</ul>
