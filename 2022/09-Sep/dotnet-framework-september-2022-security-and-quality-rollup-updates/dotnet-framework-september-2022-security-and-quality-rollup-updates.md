---
post_title: .NET Framework September 2022 Security and Quality Rollup
author1: salagarw
post_slug: dotnet-framework-september-2022-security-and-quality-rollup
username: salagarw
microsoft_alias: salagarw
featured_image: blogpst-NET.png
categories: .NET Framework
summary: September 2022 Security and Quality Rollup for .NET Framework
desired_publication_date: 2022-09-13
---

Today, we are releasing the September 2022 Security and Quality Rollup for .NET Framework.  


<h3> Security</h3>
<h5> CVE-2022-26929– .NET Framework Remote Code Execution Vulnerability </h5>
This security update addresses an issue where an attacker could convince a local user to open a specially crafted file which could execute malicious code on an affected system.


<li><a href="https://msrc.microsoft.com/update-guide/vulnerability/CVE-2022-26929" rel="nofollow"> CVE-2022-26929</a></li>  


<h3> Quality and Reliability</h3>

This release contains the following quality and reliability improvements.

<h5> WinForms <span style="font-size: 12pt;"></span></h5>
	<ul>
		<li>Addresses an issue regarding integer overflow exception in System.Windows.Forms.InputLanguage class.</li>
	</ul>



<h3> <a id="user-content-getting-the-update" class="anchor" href="#getting-the-update"></a>Getting the Update</h3>

The Security and Quality Rollup is available via Windows Update, Windows Server Update Services, and Microsoft Update Catalog.

<h5> <a id="user-content-microsoft-update-catalog" class="anchor" href="#microsoft-update-catalog"></a>Microsoft Update Catalog</h5>

You can get the update via the Microsoft Update Catalog. For Windows 10, version 1607 NET Framework 4.8 updates are available via Windows Update, Windows Server Update Services, Microsoft Update Catalog. Updates for other versions of .NET Framework are part of the Windows 10 Monthly Cumulative Update.  For Windows 10, version 1809 and newer all .NET Framwork versions re available via Windows Update, Windows Server Update Services, Microsoft Update Catalog.

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
      <td>
        <strong><a href="https://support.microsoft.com/kb/5017497" rel="nofollow">5017497</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5017024" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5017024" rel="nofollow">5017024</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8.1
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5017029" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5017029" rel="nofollow">5017029</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Microsoft server operating systems version 21H2</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5017501" rel="nofollow">5017501</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5017028" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5017028" rel="nofollow">5017028</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8.1
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5017030" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5017030" rel="nofollow">5017030</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows 10 21H2</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5017500" rel="nofollow">5017500</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5017022" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5017022" rel="nofollow">5017022</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8.1
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5017025" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5017025" rel="nofollow">5017025</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows 10 21H1</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5017499" rel="nofollow">5017499</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5017022" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5017022" rel="nofollow">5017022</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8.1
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5017025" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5017025" rel="nofollow">5017025</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows 10, version 20H2 and Windows Server, version 20H2</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5017498" rel="nofollow">5017498</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5017022" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5017022" rel="nofollow">5017022</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8.1
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5017025" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5017025" rel="nofollow">5017025</a>
      </td>
    </tr>
       <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows 10 1809 (October 2018 Update) and Windows Server 2019</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5017528" rel="nofollow">5017528</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.7.2
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5016713" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5016713" rel="nofollow">5016713</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5016593" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5016593" rel="nofollow">5016593</a>
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
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5017305" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5017305" rel="nofollow">5017305</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5017035" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5017035" rel="nofollow">5017035</a>
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
        <strong><a href="https://support.microsoft.com/kb/5017531" rel="nofollow">5017531</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5016268" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5016268" rel="nofollow">5016268</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 4.6.2, 4.7, 4.7.1, 4.7.2
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5016372" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5016372" rel="nofollow">5016372</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5017038" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5017038" rel="nofollow">5017038</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows Server 2012</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5017530" rel="nofollow">5017530</a></strong>
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
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5016371" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5016371" rel="nofollow">5016371</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5017037" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5017037" rel="nofollow">5017037</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows 7 SP1 and Windows Server 2008 R2 SP1</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5017529" rel="nofollow">5017529</a></strong>
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
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5016368" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5016368" rel="nofollow">5016368</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5017036" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5017036" rel="nofollow">5017036</a>
      </td>
    </tr>
     </tbody>
</table>





<br/>    
<h5> Previous Monthly Rollups </h5>
The last few .NET Framework Monthly updates are listed below for your convenience:  

<ul>
	<li><a href="https://devblogs.microsoft.com/dotnet/dotnet-framework-august-2022-cumulative-update-preview-updates/" rel="nofollow">.NET Framework August 2022 Cumulative Update Preview Updates</a></li>
    <li><a href="https://devblogs.microsoft.com/dotnet/dotnet-framework-august-2022-security-and-quality-rollup-updates/" rel="nofollow">.NET Framework August 2022 Security and Quality Rollup Updates</a></li>
	<li><a href="https://devblogs.microsoft.com/dotnet/dotnet-framework-july-2022-cumulative-update-preview/" rel="nofollow">.NET Framework July 2022 Cumulative Update Preview</a></li>
	<li><a href="https://devblogs.microsoft.com/dotnet/dotnet-framework-july-2022-security-and-quality-rollup-updates/" rel="nofollow">.NET Framework July 2022 Security and Quality Rollup Updates</a></li>
</ul>
