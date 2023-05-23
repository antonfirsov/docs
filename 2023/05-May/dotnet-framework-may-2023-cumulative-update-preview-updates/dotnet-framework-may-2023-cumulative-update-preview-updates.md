---
post_title: .NET Framework May 2023 Cumulative Update Preview Updates
author1: salagarw
post_slug: dotnet-framework-may-2023-cumulative-update-preview-updates
username: salagarw
microsoft_alias: salagarw
featured_image: blogpst-NET.png
categories: .NET Framework, WPF
tags: .NET Framework
summary: May 2023 Cumulative Update Preview Updates for .NET Framework
desired_publication_date: 2023-05-23
post_date: 2023-05-23 12:05:00
---

Today, we are releasing the May 2023 Cumulative Update Preview for .NET Framework.  

<h3> Quality and Reliability</h3>

This release contains the following quality and reliability improvements.

<h5> WPF<span style="font-size: 12pt;"><sup>1</sup></span></h5>
<ul>
<li>  Addresses an issue to avoid ArgumentOutOfRangeException when ControlTemplate has two or more ItemsPresenter sharing a single ItemsCollection.</li>
<li> Addresses ArgumentNullException that can arise in apps, or libraries, that directly set the IsOpen property on ToolTips or their Popups.</li>
</ul>
<h5> SQL Connectivity <span style="font-size: 12pt;"></span></h5>
<ul>
<li> Addresses an issue where SQL connection created is not terminated by the library when this error is thrown or is leaked in the client application.</li>
</ul>
<sup>1 </sup>Windows Presentation Foundation (WPF)


<h3> <a id="user-content-getting-the-update" class="anchor" href="#getting-the-update"></a>Getting the Update</h3>

The Cumulative Update Preview is available via Windows Update and <a id="user-content-microsoft-update-catalog" class="anchor" href="#microsoft-update-catalog"></a>Microsoft Update Catalog.

Customers using Windows 11, version 22H2, you will now find .NET Framework updates on the <strong> Settings > Windows Update > Advanced options > Optional updates </strong> page.   

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
    <!--- <tr bgcolor="#F0F0F0">
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
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5026515" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5026515" rel="nofollow">5026515</a>
      </td>
    </tr> -->
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows 11, version 21H2</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5026959" rel="nofollow">5026959</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5026514" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5026514" rel="nofollow">5026514</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8.1
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5026517" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5026517" rel="nofollow">5026517</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows 10, version 22H2</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5026958" rel="nofollow">5026958</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5026513" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5026513" rel="nofollow">5026513</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8.1
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5026516" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5026516" rel="nofollow">5026516</a>
      </td>
    </tr>    
  </tbody>
</table>

<br/>    
<h5> Previous Monthly Rollups </h5>
The last few .NET Framework Monthly updates are listed below for your convenience:  

<ul>
	<li><a href= "https://devblogs.microsoft.com/dotnet/dotnet-framework-april-2023-cumulative-update-preview-updates/" rel="nofollow"> .NET Framework April 2023 Cumulative Update Preview Updates</a> </li>
	<li><a href="https://devblogs.microsoft.com/dotnet/dotnet-framework-february-2023-security-and-quality-rollup-updates/" rel="nofollow">.NET Framework February 2023 Security and Quality Rollup Updates</a></li>	
</ul>
