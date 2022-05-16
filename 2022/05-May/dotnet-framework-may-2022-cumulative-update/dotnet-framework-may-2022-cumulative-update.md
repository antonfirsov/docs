---
post_title: .NET Framework May 2022 Cumulative Update
username: salagarw
microsoft_alias: salagarw
featured_image: blogpst-NET.png
categories: .NET Framework, WinForms, WPF
summary: Summary of your post, shown on the home page next to the featured image
desired_publication_date: 2022-05-24
---

We are releasing the May 2022 Cumulative Update Preview Updates for .NET Framework.  


<h3> Quality and Reliability</h3>

This release contains the following quality and reliability improvements.

<h5> WPF <span style="font-size: 12pt;"><sup>1</sup></span></h5>

<ul>
	<li>
    Addressed an issue where DWM failures can cause WPF's render thread to fail. An app can opt-in to the behavior of ignoring all DwmFlush errors by setting a regkey in HKCU\Software\Microsoft\Avalon.Graphics\IgnoreDwmFlushErrors or HKLM\Software\Microsoft\Avalon.Graphics\IgnoreDwmFlushErrors whose name is the full path to the .exe that wants to opt-in, and whose DWORD value is 1.
    </li>
    <li>
    Addressed an issue of WPF apps not working with "Text Cursor Indicator" enabled when using RichTextBox.
    </li>
</ul>

<h5> Winforms <span style="font-size: 12pt;"></span></h5>
<ul>
	<li>
    Improved the hardened rendering of ComboBox controls on 64 bit architectures.
    </li>
    <li>
    Improved the reliability of data-bound ComboBox controls under assistive technology.
    </li>
</ul>

<h5> .NET Runtime <span style="font-size: 12pt;"></span></h5>
<ul>
	<li>
    Addressed several issues that would cause too many garbage collections under high memory load. The part of the change that reduces the number of blocking generation 2 collections under high memory load is considered a tuning change and is only active if the GCConserveMemory setting is set to a non-zero value. The part of the change that reduces needless generation 0 collections is considered an improvement and is always active.
    </li>
    <li>
    Adjusted GC Heap Hard Limit configuration, as well as processor interpretation for .NET Framework container scenarios.
    </li>
</ul>

<h5> Workflow <span style="font-size: 12pt;"></span></h5>
<ul>
	<li>Addressed an issue when users interact with the Workflow Designer they might encounter incorrectly disabled context menu items when right clicking on a variable in the component variables list.
    </li>
</ul>

<sup>1 </sup>Windows Presentation Foundation (WPF)



<h3> <a id="user-content-getting-the-update" class="anchor" href="#getting-the-update"></a>Getting the Update</h3>

The Cumulative Update Preview is available via Windows Update, Windows Server Update Services, and Microsoft Update Catalog.

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
        <strong><a href="https://support.microsoft.com/kb/5014090" rel="nofollow">5014090</a></strong>
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
  </tbody>
</table>






<br/>    
<h5> Previous Monthly Rollups </h5>
The last few .NET Framework Monthly updates are listed below for your convenience:  

<ul>
	<li><a href="https://devblogs.microsoft.com/dotnet/framework-may-2022-updates/" rel="nofollow">.NET Framework May 2022 Security and Quality Rollup</a></li>
	<li><a href="https://devblogs.microsoft.com/dotnet/framework-april-2022-updates/" rel="nofollow">.NET Framework April 2022 Cumulative Update </a></li>
    <li><a href="https://devblogs.microsoft.com/dotnet/dotnet-framework-april-2022-updates/" rel="nofollow">.NET Framework April 2022 Security and Quality Rollup Updates</a></li>
	<li><a href="https://devblogs.microsoft.com/dotnet/net-framework-february-2022-cumulative-update-preview/" rel="nofollow">.NET Framework February 2022 Cumulative Update Preview</a></li>
</ul>
