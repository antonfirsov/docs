---
post_title: .NET Framework August 2023 Cumulative Update Preview
author1: salagarw
post_slug: dotnet-framework-august-2023-cumulative-update-preview
microsoft_alias: salagarw
featured_image: blogpst-NET.png
categories: .NET Framework
tags: .NET Framework
summary: August 2023 Cumulative Update Preview Updates for .NET Framework
post_date: 2023-08-24 10:05:00
---

Today, we are releasing the August 2023 Cumulative Update Preview Updates for .NET Framework.  


<h3> Quality and Reliability</h3>

This release contains the following quality and reliability improvements.

<h5> WPF<span style="font-size: 12pt;"><sup>1</sup></span></h5>
<ul>
<li>Addresses an issue where the layout of collapsed panels are affected by CollectionChanged event.</li>
</ul>
<h5> Runtime  <span style="font-size: 12pt;"></span></h5>
<ul>
<li>Addresses an issue where unpredictable crashes which could occur in multi-appdomain scenarios running on arm64.</li>
<li>Addresses an issue where AnchorInfo was miscalculated hen controls scaled on higher DPI settings and thus location may be calculated incorrect.</li>
</ul>
<h5> ASP.NET <span style="font-size: 12pt;"></span></h5>
<ul>
<li>Addresses an issue in the AspNetEnforceViewStateMac regkey logic.</li>
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
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5029718" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5029718" rel="nofollow">5029718</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows 11, version 21H2</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5029848" rel="nofollow">5029848</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5029715" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5029715" rel="nofollow">5029715</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8.1
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5029717" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5029717" rel="nofollow">5029717</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows 10, version 22H2</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5029847" rel="nofollow">5029847</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5029714" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5029714" rel="nofollow">5029714</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8.1
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5029716" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5029716" rel="nofollow">5029716</a>
      </td>
    </tr>    
  </tbody>
</table>






<br/>    
<h5> Previous Monthly Rollups </h5>
The last few .NET Framework Monthly updates are listed below for your convenience:  

<ul>
	<li><a href="https://devblogs.microsoft.com/dotnet/dotnet-framework-august-2023-security-and-quality-rollup-updates/" rel="nofollow">.NET Framework August 2023 Security and Quality Rollup Updates</a></li>
	<li><a href="https://devblogs.microsoft.com/dotnet/dotnet-framework-july-2023-cumulative-update-preview/" rel="nofollow">.NET Framework July 2023 Cumulative Update Preview</a></li>
	<li><a href="https://devblogs.microsoft.com/dotnet/dotnet-framework-july-2023-security-and-quality-rollup-updates/" rel="nofollow">.NET Framework July 2023 Security and Quality Rollup Updates</a></li>
</ul>
