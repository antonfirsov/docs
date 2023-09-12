---
post_title: .NET Framework September 2023 Security and Quality Rollup Updates
author1: salagarw
post_slug: dotnet-framework-september-2023-security-and-quality-rollup-updates
microsoft_alias: salagarw
featured_image: blogpst-NET.png
categories: .NET Framework, WPF, Maintenance & Updates
tags: .NET Framework
summary: September 2023 Security and Quality Rollup Updates for .NET Framework.
post_date: 2023-09-12 10:05:00
---

Today, we are releasing the September 2023 Security and Quality Rollup Updates for .NET Framework.  


<h3> Security</h3>
<h5>  CVE-2023-36796  - .NET Framework Remote Code Execution Vulnerability </h5>
This security update addresses a vulnerability in DiaSymReader.dll when reading a corrupted PDB file which can lead to Remote Code Execution. 
  
<li><a href="https://portal.msrc.microsoft.com/en-US/security-guidance/advisory/CVE-2023-36796" rel="nofollow">CVE 2023-36796.</a></li> 

<h5>CVE-2023-36792 - .NET Framework Remote Code Execution Vulnerability </h5>
This security update addresses a vulnerability in DiaSymReader.dll when reading a corrupted PDB file which can lead to Remote Code Execution.

<li><a href="https://portal.msrc.microsoft.com/en-US/security-guidance/advisory/CVE-2023-36792" rel="nofollow">CVE-2023-36792.</a></li> 

<h5>CVE-2023-36793 - .NET Framework Remote Code Execution Vulnerability </h5>
This security update addresses a vulnerability in DiaSymReader.dll when reading a corrupted PDB file which can lead to Remote Code Execution.

<li><a href="https://portal.msrc.microsoft.com/en-US/security-guidance/advisory/CVE-2023-36793" rel="nofollow">CVE-2023-36793.</a></li> 

<h5>CVE-2023-36794 - .NET Framework Remote Code Execution Vulnerability </h5>
This security update addresses a vulnerability in DiaSymReader.dll when reading a corrupted PDB file which can lead to Remote Code Execution.   

<li><a href="https://portal.msrc.microsoft.com/en-US/security-guidance/advisory/CVE-2023-36794" rel="nofollow">CVE-2023-36794.</a></li>

<h5>CVE-2023-36788  - .NET Framework Remote Code Execution Vulnerability </h5>
This security update addresses a vulnerability in the WPF XAML parser where an unsandboxed parser can lead to remote code execution.  

<li><a href="https://portal.msrc.microsoft.com/en-US/security-guidance/advisory/CVE-2023-36788" rel="nofollow">CVE 2023-36788.</a></li> 


<h3> Quality and Reliability</h3>

This release contains the following quality and reliability improvements.

<h5> Windows Presentation Foundation (WPF)<span style="font-size: 12pt;"></span></h5>
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


<h3> <a id="user-content-getting-the-update" class="anchor" href="#getting-the-update"></a>Getting the Update</h3>

The Security and Quality Rollup is available via Windows Update, Windows Server Update Services, and Microsoft Update Catalog.  

**Note**: Customers that rely on Windows Update and Windows Server Update Services will automatically receive the .NET Framework version-specific updates. Advanced system administrators can also take use of the below direct Microsoft Update Catalog download links to .NET Framework-specific updates. Before applying these updates, please ensure that you carefully review the .NET Framework version applicability, to ensure that you only install updates on systems where they apply.

The following table is for Windows 10, version 1507 and Windows Server 2016 versions and newer operating systems.


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
        <strong>Windows 11, version 22H2</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5031217" rel="nofollow">5031217</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8.1
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5029921" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5029921" rel="nofollow">5029921</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows 11, version 21H2</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5030181" rel="nofollow">5030181</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5029926" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5029926" rel="nofollow">5029926</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8.1
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5029920" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5029920" rel="nofollow">5029920</a>
      </td>
    </tr>    
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Microsoft server operating system, version 22H2</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5030177" rel="nofollow">5030177</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5029928" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5029928" rel="nofollow">5029928</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Microsoft server operating system version 21H2</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5030186" rel="nofollow">5030186</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5029928" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5029928" rel="nofollow">5029928</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8.1
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5029922" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5029922" rel="nofollow">5029922</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows 10, version 22H2</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5030180" rel="nofollow">5030180</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5029923" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5029923" rel="nofollow">5029923</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8.1
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5029919" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5029919" rel="nofollow">5029919</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows 10, version 21H2</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5030179" rel="nofollow">5030179</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5029923" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5029923" rel="nofollow">5029923</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8.1
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5029919" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5029919" rel="nofollow">5029919</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows 10 1809 (October 2018 Update) and Windows Server 2019</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5030178" rel="nofollow">5030178</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.7.2
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5029931" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5029931" rel="nofollow">5029931</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5029925" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5029925" rel="nofollow">5029925</a>
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
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5030213" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5030213" rel="nofollow">5030213</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5029924" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5029924" rel="nofollow">5029924</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows 10 1507</strong>
      </td>
      <td></td>
      <td></td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.6, 4.6.2
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5030220" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5030220" rel="nofollow">5030220</a>
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
      <th colspan="2">
        Security Only Update
      </th>
    </tr>
  </thead>
  <tbody>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows Server 2012 R2</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5030184" rel="nofollow">5030184</a></strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5030175" rel="nofollow">5030175</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5029915" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5029915" rel="nofollow">5029915</a>
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5029940" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5029940" rel="nofollow">5029940</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 4.6.2, 4.7, 4.7.1, 4.7.2
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5029916" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5029916" rel="nofollow">5029916</a>
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5029941" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5029941" rel="nofollow">5029941</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5029917" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5029917" rel="nofollow">5029917</a>
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5029942" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5029942" rel="nofollow">5029942</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows Server 2012</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5030183" rel="nofollow">5030183</a></strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5030174" rel="nofollow">5030174</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5030160" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5030160" rel="nofollow">5030160</a>
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5030030" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5030030" rel="nofollow">5030030</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 4.6.2, 4.7, 4.7.1, 4.7.2
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5029932" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5029932" rel="nofollow">5029932</a>
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5029945" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5029945" rel="nofollow">5029945</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5029927" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5029927" rel="nofollow">5029927</a>
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5029943" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5029943" rel="nofollow">5029943</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows Embedded 7 and Windows Server 2008 R2 SP1</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5030182" rel="nofollow">5030182</a></strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5030173" rel="nofollow">5030173</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5.1
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5029938" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5029938" rel="nofollow">5029938</a>
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5029948" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5029948" rel="nofollow">5029948</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 4.6.2, 4.7, 4.7.1, 4.7.2
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5029933" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5029933" rel="nofollow">5029933</a>
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5029946" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5029946" rel="nofollow">5029946</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5029929" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5029929" rel="nofollow">5029929</a>
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5029944" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5029944" rel="nofollow">5029944</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows Server 2008</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5030185" rel="nofollow">5030185</a></strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5030176" rel="nofollow">5030176</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 2.0, 3.0
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5029937" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5029937" rel="nofollow">5029937</a>
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5029947" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5029947" rel="nofollow">5029947</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 4.6.2
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5029933" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5029933" rel="nofollow">5029933</a>
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5029946" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5029946" rel="nofollow">5029946</a>
      </td>
    </tr>
  </tbody>
</table>



<br/>    
<h3> Previous Monthly Rollups </h3>
The last few .NET Framework Monthly updates are listed below for your convenience:  

<ul>
	<li><a href="https://devblogs.microsoft.com/dotnet/dotnet-framework-august-2023-cumulative-update-preview/" rel="nofollow">.NET Framework August 2023 Cumulative Update Preview</a></li>
	<li><a href="https://devblogs.microsoft.com/dotnet/dotnet-framework-august-2023-security-and-quality-rollup-updates/" rel="nofollow">.NET Framework August 2023 Security and Quality Rollup Updates</a></li>
	<li><a href="https://devblogs.microsoft.com/dotnet/dotnet-framework-july-2023-cumulative-update-preview/" rel="nofollow">.NET Framework July 2023 Cumulative Update Preview</a></li>
	<li><a href="https://devblogs.microsoft.com/dotnet/dotnet-framework-july-2023-security-and-quality-rollup-updates/" rel="nofollow">.NET Framework July 2023 Security and Quality Rollup Updates</a></li>
</ul>
