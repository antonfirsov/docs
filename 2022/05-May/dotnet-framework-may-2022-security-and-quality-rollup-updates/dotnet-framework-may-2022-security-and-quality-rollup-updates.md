---
post_title: .NET Framework May 2022 Security and Quality Rollup
username: salagarw
microsoft_alias: salagarw
featured_image: blogpst-NET.png
categories: .NET Framework
summary: Summary of your post, shown on the home page next to the featured image
desired_publication_date: 2022-05-10
---

We are releasing the May 2022 Security and Quality Rollup Updates for .NET Framework.  


<h3> Security</h3>
<h5>CVE-2022-30130 - .NET Framework Denial of Service</h5>
This security update addresses an issue where a local user opening a specially crafted file could cause a denial of service condition on an affected system.
<li><a href="https://msrc.microsoft.com/update-guide/vulnerability/CVE-2022-30130" rel="nofollow">CVE-2022-30130</a></li>  

<h3> Quality and Reliability</h3>

This release contains the following quality and reliability improvements.

<h5> DS<span style="font-size: 12pt;"><sup>1</sup></span></h5>
	<ul>
		<li>Addresses an issue where 3rd party .NET apps using certain System.DirectoryServices APIs crash with an Access Violation (0xC0000005).</li>
	</ul>

<sup>1 </sup>Directory Services (DS)



<h3> <a id="user-content-getting-the-update" class="anchor" href="#getting-the-update"></a>Getting the Update</h3>

The Security and Quality Rollup is available via Windows Update, Windows Server Update Services, and Microsoft Update Catalog.  The Security Only Update is available via Windows Server Update Services and Microsoft Update Catalog.

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
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5013628" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5013628" rel="nofollow">5013628</a>
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
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5013630" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5013630" rel="nofollow">5013630</a>
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
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5013624" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5013624" rel="nofollow">5013624</a>
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
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5013624" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5013624" rel="nofollow">5013624</a>
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
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5013624" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5013624" rel="nofollow">5013624</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows 10 1909</strong>
      </td>
      <td></td>
      <td></td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5013627" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5013627" rel="nofollow">5013627</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows 10 1809 (October 2018 Update) and Windows Server 2019</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5013868" rel="nofollow">5013868</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.7.2
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5013641" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5013641" rel="nofollow">5013641</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5013626" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5013626" rel="nofollow">5013626</a>
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
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5013952" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5013952" rel="nofollow">5013952</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5013625" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5013625" rel="nofollow">5013625</a>
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
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5013963" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5013963" rel="nofollow">5013963</a>
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
        <strong><a href="https://support.microsoft.com/kb/5013872" rel="nofollow">5013872</a></strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5013839" rel="nofollow">5013839</a></strong>
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
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5013621" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5013621" rel="nofollow">5013621</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 4.6.2, 4.7, 4.7.1, 4.7.2
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5013643" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5013643" rel="nofollow">5013643</a>
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5013623" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5013623" rel="nofollow">5013623</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5013631" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5013631" rel="nofollow">5013631</a>
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5013616" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5013616" rel="nofollow">5013616</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows Server 2012</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5013871" rel="nofollow">5013871</a></strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5013838" rel="nofollow">5013838</a></strong>
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
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5013618" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5013618" rel="nofollow">5013618</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 4.6.2, 4.7, 4.7.1, 4.7.2
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5013642" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5013642" rel="nofollow">5013642</a>
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5013622" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5013622" rel="nofollow">5013622</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5013629" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5013629" rel="nofollow">5013629</a>
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5013615" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5013615" rel="nofollow">5013615</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows 7 SP1 and Windows Server 2008 R2 SP1</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5013870" rel="nofollow">5013870</a></strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5013837" rel="nofollow">5013837</a></strong>
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
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5013620" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5013620" rel="nofollow">5013620</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 4.6.2, 4.7, 4.7.1, 4.7.2
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5013644" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5013644" rel="nofollow">5013644</a>
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5013612" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5013612" rel="nofollow">5013612</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5013632" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5013632" rel="nofollow">5013632</a>
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5013617" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5013617" rel="nofollow">5013617</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows Server 2008</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5013873" rel="nofollow">5013873</a></strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5013840" rel="nofollow">5013840</a></strong>
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
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5013619" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5013619" rel="nofollow">5013619</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 4.6.2
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5013644" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5013644" rel="nofollow">5013644</a>
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5013612" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5013612" rel="nofollow">5013612</a>
      </td>
    </tr>
  </tbody>
</table>



<br/>    
<h5> Previous Monthly Rollups </h5>
The last few .NET Framework Monthly updates are listed below for your convenience:  

<ul>
	<li><a href="https://devblogs.microsoft.com/dotnet/dotnet-framework-april-2022-updates/" rel="nofollow">.NET Framework April 2022 Security and Quality Rollup Updates</a></li>
	<li><a href="https://devblogs.microsoft.com/dotnet/framework-april-2022-updates/" rel="nofollow">.NET Framework April 2022 Cumulative Update </a></li>
	<li><a href="https://devblogs.microsoft.com/dotnet/net-framework-february-2022-cumulative-update-preview/" rel="nofollow">.NET Framework February 2022 Cumulative Update Preview</a></li>
	<li><a href="https://devblogs.microsoft.com/dotnet/net-framework-february-2022-security-and-quality-rollup/" rel="nofollow">.NET Framework February 2022 Security and Quality Rollup</a></li>
</ul>
