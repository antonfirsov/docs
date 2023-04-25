---
post_title: .NET Framework April 2023 Cumulative Update Preview Updates
author1: salagarw
post_slug: dotnet-framework-april-2023-cumulative-update-preview-updates
username: salagarw
microsoft_alias: salagarw
featured_image: blogpst-NET.png
categories: .NET Framework, WPF, Maintenance & Updates
tags: .NET Framework
summary:  April 2023 Cumulative Update Preview Updates for .NET Framework
desired_publication_date: 2023-04-25
post_date: 2023-04-25 12:05:00
---
 
Today, we are releasing the April 2023 Cumulative Update Preview for .NET Framework.  


<h3> Quality and Reliability</h3>

This release contains the following quality and reliability improvements.

<h5> WPF<span style="font-size: 12pt;"><sup>1</sup></span></h5>
<ul>
<li>  Addresses an issue where using IsReadOnly property of TextBox and RichTextBox in ControlTemplate.Triggers throws an exception.</li>
<li>  Addresses Null Reference Exception reloading XPS document after adjusting column width for Datagrid and Gridview controls.</li>
<li>  Addresses Null Reference Exception when ToolTip is visible property is overridden to be always be false.</li>
</ul>

<sup>1 </sup>Windows Presentation Foundation (WPF)

<h3> New .NET preview updates experience </h3>
For customers on Windows 11, version 22H2 we have improved .NET Framework update installing experience for optional Non sec updates. For more information visit <a href="https://devblogs.microsoft.com/dotnet/improvements-to-net-framework-updates-for-windows-11-22h2/ " rel="nofollow">Announcing the SV2 improvements </a> and <a href = " https://techcommunity.microsoft.com/t5/windows-it-pro-blog/improving-net-framework-updates-for-windows-11-version-22h2/ba-p/3741184"  rel="nofollow"> Improving .NET Framework updates for Windows 11, version 22H2 - Microsoft Community Hub. </a>


<h3> <a id="user-content-getting-the-update" class="anchor" href="#getting-the-update"></a>Getting the Update</h3>

The Cumulative Update Preview is available via Windows Update and <a id="user-content-microsoft-update-catalog" class="anchor" href="#microsoft-update-catalog"></a>Microsoft Update Catalog.

Customers using Windows 11, version 22H2, you will now find .NET Framework updates on the <strong> Settings > Windows Update > Advanced options > Optional updates </strong> page.  Customers using Windows 11, version 21H2 and below version that rely on Windows Update will automatically receive the .NET Framework version-specific updates.  

Advanced system administrators can also take use of the below direct Microsoft Update Catalog download links to .NET Framework-specific updates. Before applying these updates, please ensure that you carefully review the .NET Framework version applicability, to ensure that you only install updates on systems where they apply. 


The following table is for Windows 10+ versions.       
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
        <strong>Windows 11, version 22H2</strong>
      </td>
      <td></td>
      <td></td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8.1
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5025182" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5025182" rel="nofollow">5025182</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows 11, version 21H2</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5025368" rel="nofollow">5025368</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5025184" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5025184" rel="nofollow">5025184</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8.1
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5025186" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5025186" rel="nofollow">5025186</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows 10 22H2</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5025367" rel="nofollow">5025367</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5025183" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5025183" rel="nofollow">5025183</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8.1
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5025185" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5025185" rel="nofollow">5025185</a>
      </td>
    </tr>
  </tbody>
</table>
<br/> 

<br />
<h3> Previous Monthly Rollups </h3>
The last few .NET Framework Monthly updates are listed below for your convenience:  

<ul>
	<li><a href="https://devblogs.microsoft.com/dotnet/dotnet-framework-february-2023-security-and-quality-rollup-updates/" rel="nofollow">.NET Framework February 2023 Security and Quality Rollup Updates</a></li>
	<li><a href="https://devblogs.microsoft.com/dotnet/dotnet-framework-january-2023-update/" rel="nofollow">.NET Framework January 2023 Cumulative Update Preview</a></li>
</ul>
