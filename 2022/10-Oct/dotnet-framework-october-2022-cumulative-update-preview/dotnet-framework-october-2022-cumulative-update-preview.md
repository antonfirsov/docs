---
post_title: .NET Framework October 2022 Cumulative Update Preview
author1: salagarw
post_slug: dotnet-framework-october-2022-cumulative-update-preview
username: salagarw
microsoft_alias: salagarw
featured_image: blogpst-NET.png
categories: .NET Framework, WPF
tags: .NET Framework
summary: October 2022 Cumulative Update Preview for .NET Framework
desired_publication_date: 2022-10-25
post_date: 2022-10-25 12:00:00
---

Today, we are releasing the October 2022 Cumulative Update Preview for .NET Framework.  


<h3> Quality and Reliability</h3>

This release contains the following quality and reliability improvements.

<h5> WPF<span style="font-size: 12pt;"><sup>1</sup></span></h5>
<ul>
<li>  Addresses an issue where a FailFast crash could occur when using WebBrowser.NavigateToString.</li>
<li>  Addresses an ArgumentOutOfRangeException that can arise when calling ListBox.ScrollIntoView while there are pending changes to the visual tree that will change or clear the underlying ItemsCollection.</li>
<li>  Addresses an ArgumentException "Width and Height must be non-negative" that can arise in an ItemsControl with grouping enabled, custom margins on the GroupItems, collapse/expand of GroupItems enabled, and run in high-DPI.</li>
<li>  Addresses an issue where the opt-out switch Switch.System.Windows.Controls.ToolTip.OptOutOfWCAG21ToolTipBehavior didn't quite restore the 4.8 behavior, in particular the way it honors Switch.UseLegacyToolTipDisplay (which controls whether keyboard tooltip behavior is enabled).</li>
</ul>
<h5> .NET Runtime <span style="font-size: 12pt;"></span></h5>
<ul>
<li>Address crashes that could occur if ilasm.exe failed to establish temporary PDB file alongside the output file.</li>
</ul>
<sup> 1 </sup>Windows Presentation Foundation (WPF)


<h3> <a id="user-content-getting-the-update" class="anchor" href="#getting-the-update"></a>Getting the Update</h3>

The Cumulative Update Preview is available via Windows Update and <a id="user-content-microsoft-update-catalog" class="anchor" href="#microsoft-update-catalog"></a>Microsoft Update Catalog.


**Note**: Customers that rely on Windows Update will automatically receive the .NET Framework version-specific updates. Advanced system administrators can also take use of the below direct Microsoft Update Catalog download links to .NET Framework-specific updates. Before applying these updates, please ensure that you carefully review the .NET Framework version applicability, to ensure that you only install updates on systems where they apply. 


The following table is for Windows 10, and Windows Server 2019+ versions.    
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
        <strong><a href="https://support.microsoft.com/kb/5018859" rel="nofollow">5018859</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5018330" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5018330" rel="nofollow">5018330</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8.1
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5018334" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5018334" rel="nofollow">5018334</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Microsoft server operating system, version 22H2</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5018855" rel="nofollow">5018855</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5018331" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5018331" rel="nofollow">5018331</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Microsoft server operating system version 21H2</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5018860" rel="nofollow">5018860</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5018331" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5018331" rel="nofollow">5018331</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8.1
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5018335" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5018335" rel="nofollow">5018335</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows 10 version 22H2</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5018858" rel="nofollow">5018858</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5018329" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5018329" rel="nofollow">5018329</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8.1
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5018332" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5018332" rel="nofollow">5018332</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows 10 version 21H2</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5018858" rel="nofollow">5018858</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5018329" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5018329" rel="nofollow">5018329</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8.1
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5018332" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5018332" rel="nofollow">5018332</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows 10 version 21H1</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5018857" rel="nofollow">5018857</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5018329" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5018329" rel="nofollow">5018329</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8.1
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5018332" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5018332" rel="nofollow">5018332</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows 10 Version 20H2</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5018856" rel="nofollow">5018856</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5018329" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5018329" rel="nofollow">5018329</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8.1
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5018332" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5018332" rel="nofollow">5018332</a>
      </td>
    </tr>
  </tbody>
</table>


<br/>    
<h3> Previous Monthly Rollups </h3>
The last few .NET Framework Monthly updates are listed below for your convenience:  

<ul>
	<li><a href="https://devblogs.microsoft.com/dotnet/dotnet-framework-october-2022-security-and-quality-rollup/" rel="nofollow">.NET Framework October 2022 Security and Quality Rollup</a></li>
	<li><a href="https://devblogs.microsoft.com/dotnet/dotnet-framework-september-2022-cumulative-update-preview/" rel="nofollow">.NET Framework September 2022 Cumulative Update Preview</a></li>
	<li><a href="https://devblogs.microsoft.com/dotnet/dotnet-framework-september-2022-security-and-quality-rollup/" rel="nofollow">.NET Framework September 2022 Security and Quality Rollup</a></li>
	<li><a href="https://devblogs.microsoft.com/dotnet/dotnet-framework-august-2022-cumulative-update-preview-updates/" rel="nofollow">.NET Framework August 2022 Cumulative Update Preview Updates</a></li>
</ul>
