---
post_title: .NET Framework December 2022 Security and Quality Rollup Updates
author1: salagarw
post_slug: dotnet-framework-december-2022-security-and-quality-rollup-updates
username: salagarw
microsoft_alias: salagarw
featured_image: blogpst-NET.png
categories: .NET Framework, WPF
tags: .NET Framework
summary:  December 2022 Security and Quality Rollup Updates for .NET Framework
desired_publication_date: 2022-12-13
post_date: 2022-12-13 10:05:00
---

Today, we are releasing the December 2022 Security and Quality Rollup Updates for .NET Framework.  

<h3> Security</h3>
<h5>CVE-2022-41089 – .NET Framework Remote Code Execution Vulnerability</h5>
This security update addresses a vulnerability where restricted mode is triggered for the parsing of XPS files, preventing gadget chains which could allow remote code execution on an affected system. For more information please see CVE-2022-41089.

<li><a href="https://portal.msrc.microsoft.com/en-US/security-guidance/advisory/CVE-2022-41089" rel="nofollow">CVE-2022-41089</a></li>  


<h3> Quality and Reliability</h3>

There are no new Quality and Reliability Improvements in this update.

<h3> Known issues in this update</h3>
<table border="1" cellspacing="0" cellpadding="8px">
  <thead>
    <tr>
      <th>
        Symptom
      </th>
      <th >
        Workaround
      </th>      
    </tr>
  </thead>
  <tbody>
    <tr>
      <td>
        After installing this update, WPF apps may have a change in behavior. For more information about this issue, see <a href="https://support.microsoft.com/kb/5022083" rel="nofollow">KB5022083</a> 
      </td>
      <td>To mitigate this issue, see <a href="https://support.microsoft.com/kb/5022083" rel="nofollow">KB5022083</a> .</td>     
    </tr>    
  </tbody>
</table>

<h3> <a id="user-content-getting-the-update" class="anchor" href="#getting-the-update"></a>Getting the Update</h3>

The Security and Quality Rollup is available via Windows Update, Windows Server Update Services, and Microsoft Update Catalog.  The Security Only Update is available via Windows Server Update Services and Microsoft Update Catalog.

<h5> <a id="user-content-microsoft-update-catalog" class="anchor" href="#microsoft-update-catalog"></a>Microsoft Update Catalog</h5>

The Security and Quality Rollup is available via Windows Update, Windows Server Update Services, and Microsoft Update Catalog.  The Security Only Update is available via Windows Server Update Services and Microsoft Update Catalog.

**Note**: Customers that rely on Windows Update and Windows Server Update Services will automatically receive the .NET Framework version-specific updates. Advanced system administrators can also take use of the below direct Microsoft Update Catalog download links to .NET Framework-specific updates. Before applying these updates, please ensure that you carefully review the .NET Framework version applicability, to ensure that you only install updates on systems where they apply. 


The following table is for Windows 10, version 1607 and Windows Server 2016 versions and newer operating systems.
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
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5020880" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5020880" rel="nofollow">5020880</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows 11, version 21H2</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5021090" rel="nofollow">5021090</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5020875" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5020875" rel="nofollow">5020875</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8.1
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5020882" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5020882" rel="nofollow">5020882</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Microsoft server operating system, version 22H2</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5021084" rel="nofollow">5021084</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5020877" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5022127" rel="nofollow">5020877</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Microsoft server operating system version 21H2</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5021095" rel="nofollow">5021095</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5020877" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5020877" rel="nofollow">5020877</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8.1
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5020883" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5020883" rel="nofollow">5020883</a>
      </td>
    </tr>    
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows 10 22H2</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5021089" rel="nofollow">5021089</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5020872" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5020872" rel="nofollow">5020872</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8.1
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5020881" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5020881" rel="nofollow">5020881</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows 10 21H2</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5021088" rel="nofollow">5021088</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5020872" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5020872" rel="nofollow">5020872</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8.1
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5020881" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5020881" rel="nofollow">5020881</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows 10 21H1</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5021087" rel="nofollow">5021087</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5020872" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5020872" rel="nofollow">5020872</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8.1
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5020881" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5020881" rel="nofollow">5020881</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows 10 Version 20H2</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5021086" rel="nofollow">5021086</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5020872" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5020872" rel="nofollow">5020872</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8.1
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5020881" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5020881" rel="nofollow">5020881</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows 10 1809 (October 2018 Update) and Windows Server 2019</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5021085" rel="nofollow">5021085</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.7.2
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5020866" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5020866" rel="nofollow">5020866</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5020874" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5020874" rel="nofollow">5020874</a>
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
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5021235" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5021235" rel="nofollow">5021235</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5020873" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5020873" rel="nofollow">5020873</a>
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
       .NET Framework 3.5, 4.6.2
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5021243" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5021243" rel="nofollow">5021243</a>
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
        <strong>Windows 8.1, Windows RT 8.1 and Windows Server 2012 R2</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5021093" rel="nofollow">5021093</a></strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5021081" rel="nofollow">5021081</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5020862" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5020862" rel="nofollow">5020862</a>
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5020897" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5020897" rel="nofollow">5020897</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 4.6.2, 4.7, 4.7.1, 4.7.2
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5020868" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5020868" rel="nofollow">5020868</a>
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5020899" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5020899" rel="nofollow">5020899</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5020878" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5020878" rel="nofollow">5020878</a>
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5020902" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5020902" rel="nofollow">5020902</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows Server 2012</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5021092" rel="nofollow">5021092</a></strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5021080" rel="nofollow">5021080</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5020859" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5020859" rel="nofollow">5020859</a>
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5020894" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5020894" rel="nofollow">5020894</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 4.6.2, 4.7, 4.7.1, 4.7.2
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5020867" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5020867" rel="nofollow">5020867</a>
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5020898" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5020898" rel="nofollow">5020898</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5020876" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5020876" rel="nofollow">5020876</a>
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5020901" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5020901" rel="nofollow">5020901</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows 7 SP1 and Windows Server 2008 R2 SP1</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5021091" rel="nofollow">5021091</a></strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5021079" rel="nofollow">5021079</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5.1
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5020861" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5020861" rel="nofollow">5020861</a>
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5020896" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5020896" rel="nofollow">5020896</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 4.6.2, 4.7, 4.7.1, 4.7.2
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5020869" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5020869" rel="nofollow">5020869</a>
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5020900" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5020900" rel="nofollow">5020900</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5020879" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5020879" rel="nofollow">5020879</a>
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5020903" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5020903" rel="nofollow">5020903</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows Server 2008</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5021094" rel="nofollow">5021094</a></strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5021082" rel="nofollow">5021082</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 2.0, 3.0
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5020860" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5020860" rel="nofollow">5020860</a>
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5020895" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5020895" rel="nofollow">5020895</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 4.6.2
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5020869" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5020869" rel="nofollow">5020869</a>
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5020900" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5020900" rel="nofollow">5020900</a>
      </td>
    </tr>
  </tbody>
</table>



<br/>    
<h3> Previous Monthly Rollups </h3>
The last few .NET Framework Monthly updates are listed below for your convenience:  

<ul>
	<li><a href="https://devblogs.microsoft.com/dotnet/dotnet-framework-november-2022-security-and-quality-rollup-updates/" rel="nofollow">.NET Framework November 2022 Security and Quality Rollup Updates</a></li>
	<li><a href="https://devblogs.microsoft.com/dotnet/dotnet-framework-october-2022-cumulative-update-preview/" rel="nofollow">.NET Framework October 2022 Cumulative Update Preview</a></li>
	<li><a href="https://devblogs.microsoft.com/dotnet/dotnet-framework-october-2022-security-and-quality-rollup/" rel="nofollow">.NET Framework October 2022 Security and Quality Rollup</a></li>
	<li><a href="https://devblogs.microsoft.com/dotnet/dotnet-framework-september-2022-cumulative-update-preview/" rel="nofollow">.NET Framework September 2022 Cumulative Update Preview</a></li>
</ul>
