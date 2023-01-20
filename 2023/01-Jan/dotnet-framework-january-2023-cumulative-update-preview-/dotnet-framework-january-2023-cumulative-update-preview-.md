---
post_title: .NET Framework January 2023 Cumulative Update Preview 
author1: salagarw
post_slug: dotnet-framework-january-2023-update
username: salagarw
microsoft_alias: salagarw
featured_image: blogpst-NET.png
categories: .NET Framework, WPF, Maintenance & Updates
tags: .NET Framework
summary: January 2023 Cumulative Update Preview Updates for .NET Framework.  
desired_publication_date: 2023-01-20
post_date: 2023-01-20 14:05:00
---

Today, we are releasing the January 2023 Cumulative Update Preview Updates for .NET Framework.  


<h3> Quality and Reliability</h3>

<h5> WPF<span style="font-size: 12pt;"><sup>1</sup></span></h5>
<ul>
<li>  Addresses an issue in propagation of ElementHost controls Visible property to underlying HwndWrapper.</li>
<li>  Addresses an issue that restores VirtualizingScrollPanel scrolling behavior for CollectionChange event.</li>
<li>  Addresses an issue to ignore Win32LastError when calling GetWindowText and GetWindowTextLength methods.</li>
<li>  Addresses Null Reference Exception when ToolTip is visible property is overridden to be always be false.</li>
</ul>
<h5> Networking <span style="font-size: 12pt;"></span></h5>
<ul>
<li>Addresses an issue in the Socket.EndReceiveFrom method that may lead to AccessViolationException in rare scenarios.</li>
</ul>
<sup> 1 </sup>Windows Presentation Foundation (WPF)

<h3> <a id="user-content-getting-the-update" class="anchor" href="#getting-the-update"></a>Getting the Update</h3>

The Cumulative Update Preview is available via Windows Update and <a id="user-content-microsoft-update-catalog" class="anchor" href="#microsoft-update-catalog"></a>Microsoft Update Catalog.


**Note**: Customers that rely on Windows Update will automatically receive the .NET Framework version-specific updates. Advanced system administrators can also take use of the below direct Microsoft Update Catalog download links to .NET Framework-specific updates. Before applying these updates, please ensure that you carefully review the .NET Framework version applicability, to ensure that you only install updates on systems where they apply. 


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
        <strong>Windows 11, version 21H2</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5022479" rel="nofollow">5022479</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5022406" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5022406" rel="nofollow">5022406</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8.1
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5022409" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5022409" rel="nofollow">5022409</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows 10 22H2</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5022478" rel="nofollow">5022478</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5022405" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5022405" rel="nofollow">5022405</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8.1
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5022408" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5022408" rel="nofollow">5022408</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows 10 21H2</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5022476" rel="nofollow">5022476</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5022405" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5022405" rel="nofollow">5022405</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8.1
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5022408" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5022408" rel="nofollow">5022408</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows 10 Version 20H2</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5022474" rel="nofollow">5022474</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5022405" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5022405" rel="nofollow">5022405</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8.1
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5022408" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5022408" rel="nofollow">5022408</a>
      </td>
    </tr>
  </tbody>
</table>



<br/>    
<h3> Previous Monthly Rollups </h3>
The last few .NET Framework updates are listed below for your convenience:  

<ul>
	<li><a href="https://devblogs.microsoft.com/dotnet/dotnet-framework-december-2022-security-and-quality-rollup-updates/" rel="nofollow">.NET Framework December 2022 Security and Quality Rollup Updates</a></li>
	<li><a href="https://devblogs.microsoft.com/dotnet/dotnet-framework-november-2022-security-and-quality-rollup-updates/" rel="nofollow">.NET Framework November 2022 Security and Quality Rollup Updates</a></li>
	<li><a href="https://devblogs.microsoft.com/dotnet/dotnet-framework-october-2022-cumulative-update-preview/" rel="nofollow">.NET Framework October 2022 Cumulative Update Preview</a></li>
	<li><a href="https://devblogs.microsoft.com/dotnet/dotnet-framework-october-2022-security-and-quality-rollup/" rel="nofollow">.NET Framework October 2022 Security and Quality Rollup</a></li>
</ul>
