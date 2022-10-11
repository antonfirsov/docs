---
post_title: .NET Framework October 2022 Security and Quality Rollup
author1: salagarw
post_slug: dotnet-framework-october-2022-security-and-quality-rollup
username: salagarw
microsoft_alias: salagarw
featured_image: blogpst-NET.png
categories: .NET Framework, WPF
tags: .NET Framework
summary: October 2022 Security and Quality Rollup for .NET Framework
desired_publication_date: 2022-10-11 
post_date: 2022-10-11 12:15:00
---

Today, we are releasing the October 2022 Security and Quality Rollup Updates for .NET Framework.  


<h3> Security</h3>
For Windows 11, version 22H2 and Microsoft server operating system, version 22H2 the February Security and Quality Rollup Update is cumulative and contains all previously released security improvements.
For all other windows versions The October Security and Quality Rollup Update does not contain any new security fixes. See <a href="https://devblogs.microsoft.com/dotnet/dotnet-framework-september-2022-security-and-quality-rollup/" rel="nofollow">September 2022 Security and Quality Rollup</a> for the latest security updates.    


<h3> Quality and Reliability</h3>

This release contains the following quality and reliability improvements.

<h5> WPF<span style="font-size: 12pt;"><sup>1</sup></span></h5>
<ul>
<li>Addresses an issue of WPF apps not working with "Text Cursor Indicator" enabled when using RichTextBox.</li>
<li> Addresses an issue to allow users to adjust column width for Datagrid and Gridview controls using keybord.</li>
<li> Addresses an issue where opening a tooltip causes an ArgumentOutOfRangeException, when the app has changed the floating point control word.</li>
<li> Addresses an issue where invoking a synchronization Wait on the UI thread can lead to a render-thread failure, due to unexpected re-entrancy.</li>
<li>Addresses issues arising in TreeView (or ListBox/DataGrid with grouping enabled) when changing the underlying collection(s) while also collapsing or expanding TreeView nodes. Scrolling actions, especially "short" actions like LineUp or MouseWheelUp, can result in poor outcomes: hangs, crashes, unexpected scrolling, gaps in the display, etc.</li>
<li>Addresses an issue where DWM failures can cause WPF's render thread to fail. An app can opt-in to the behavior of ignoring all DwmFlush errors by setting a regkey in </br>
&nbsp;&nbsp;&nbsp;&nbsp;<codeinline> HKCU\Software\Microsoft\Avalon.Graphics\IgnoreDwmFlushErrors</codeinline></br>
&nbsp;&nbsp;&nbsp;&nbsp;or</br>
&nbsp;&nbsp;&nbsp;&nbsp;<codeinline>HKLM\Software\Microsoft\Avalon.Graphics\IgnoreDwmFlushErrors</codeinline></br>
whose name is the full path to the .exe that wants to opt-in, and whose DWORD value is 1.</li>
</ul>
<h5> Winforms<span style="font-size: 12pt;"></span></h5>
<ul>
<li>Enabled Narrator to focus on System.Windows.Forms.DataGridView control even if it's empty.</li>
<li>Improved keyboard navigation in ToolStrip ComboBox and TextBox items.</li>
<li>Improved color contrast in System.Windows.Forms.DataGridView control's link cells.</li>
</ul>
<h5> Workflow <span style="font-size: 12pt;"></span></h5>
<ul>
<li>Addresses an issue when users interact with the Workflow Designer, they might encounter incorrectly disabled context menu items when right clicking on a variable in the component variables list.</li>
</ul>
<h5> .NET Runtime <span style="font-size: 12pt;"></span></h5>
<ul>
<li>Adjusted GC Heap Hard Limit configuration, as well as processor interpretation for .NET Framework container scenarios.</li>
<li>Improved reliability of managed debugging on arm64.</li>
<li>Improved security of code identified by internal compliance scans.</li>
<li>Improved deterministic output capability in ilasm.exe.</li>
</ul>
<h5> Networking<span style="font-size: 12pt;"></span></h5>
<ul>
<li>Addresses an issue when Ssl negotiation can hang indefinitely when client certificates are used when TLS 1.3 is negotiated. Before the change renegotiation (PostHandshakeAuthentiction) would fail and SslStream or HttpWebRequest would observe timeout. Possible workaround is disabling TLS 1.3 either via Switch.System.Net.DontEnableTls13 AppContext or via OS registry.</li>
</ul>
<sup>1 </sup>Windows Presentation Foundation (WPF)


<h3> <a id="user-content-getting-the-update" class="anchor" href="#getting-the-update"></a>Getting the Update</h3>

The Security and Quality Rollup is available via Windows Update, Windows Server Update Services, and Microsoft Update Catalog.

<h5> <a id="user-content-microsoft-update-catalog" class="anchor" href="#microsoft-update-catalog"></a>Microsoft Update Catalog</h5>

You can get the update via the Microsoft Update Catalog. For Windows 10, version 1607 NET Framework 4.8 updates are available via Windows Update, Windows Server Update Services, Microsoft Update Catalog. Updates for other versions of .NET Framework are part of the Windows 10 Monthly Cumulative Update.  For Windows 10, version 1809 and newer all .NET Framework versions are available via Windows Update, Windows Server Update Services, Microsoft Update Catalog

**Note**: Customers that rely on Windows Update and Windows Server Update Services will automatically receive the .NET Framework version-specific updates. Advanced system administrators can also take use of the below direct Microsoft Update Catalog download links to .NET Framework-specific updates. Before applying these updates, please ensure that you carefully review the .NET Framework version applicability, to ensure that you only install updates on systems where they apply. 


The following table is for Windows 10, version 1809 and Windows Server 2019+ versions. 
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
      <strong><a href="https://support.microsoft.com/kb/5017271" rel="nofollow">5017271</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8.1
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5017271" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5017271" rel="nofollow">5017271</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows 11, version 21H2</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5018546" rel="nofollow">5018546</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5017264" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5017264" rel="nofollow">5017264</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8.1
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5017267" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5017267" rel="nofollow">5017267</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Microsoft server operating system, version 22H2</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5018541" rel="nofollow">5018541</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5017265" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5017265" rel="nofollow">5017265</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Microsoft server operating system version 21H2</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5018551" rel="nofollow">5018551</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5017265" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5017265" rel="nofollow">5017265</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8.1
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5017268" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5017268" rel="nofollow">5017268</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows 10 22H2</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5017888" rel="nofollow">5017888</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5017262" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5017262" rel="nofollow">5017262</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8.1
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5017266" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5017266" rel="nofollow">5017266</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows 10 21H2</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5018545" rel="nofollow">5018545</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5017262" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5017262" rel="nofollow">5017262</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8.1
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5017266" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5017266" rel="nofollow">5017266</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows 10 21H1</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5018544" rel="nofollow">5018544</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5017262" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5017262" rel="nofollow">5017262</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8.1
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5017266" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5017266" rel="nofollow">5017266</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows 10 Version 20H2</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5018543" rel="nofollow">5018543</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5017262" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5017262" rel="nofollow">5017262</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8.1
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5017266" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5017266" rel="nofollow">5017266</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows 10 1809 (October 2018 Update) and Windows Server 2019</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5018542" rel="nofollow">5018542</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.7.2
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5017270" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5017270" rel="nofollow">5017270</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5017263" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5017263" rel="nofollow">5017263</a>
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
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5018411" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5018411" rel="nofollow">5018411</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5018515" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5018515" rel="nofollow">5018515</a>
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
        <strong><a href="https://support.microsoft.com/kb/5018549" rel="nofollow">5018549</a></strong>
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
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5018523" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5018523" rel="nofollow">5018523</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5018519" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5018519" rel="nofollow">5018519</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows Server 2012</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5018548" rel="nofollow">5018548</a></strong>
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
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5018522" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5018522" rel="nofollow">5018522</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5018518" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5018518" rel="nofollow">5018518</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows 7 SP1 and Windows Server 2008 R2 SP1</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5018547" rel="nofollow">5018547</a></strong>
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
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5018521" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5018521" rel="nofollow">5018521</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5018516" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5018516" rel="nofollow">5018516</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows Server 2008</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5018550" rel="nofollow">5018550</a></strong>
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
    </tr>
    <tr>
      <td>
       .NET Framework 4.6.2
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5018521" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5018521" rel="nofollow">5018521</a>
      </td>
    </tr>
  </tbody>
</table>



<br/>    
<h5> Previous Monthly Rollups </h5>
The last few .NET Framework Monthly updates are listed below for your convenience:  

<ul>
	<li><a href="https://devblogs.microsoft.com/dotnet/dotnet-framework-september-2022-cumulative-update-preview/" rel="nofollow">.NET Framework September 2022 Cumulative Update Preview</a></li>
	<li><a href="https://devblogs.microsoft.com/dotnet/dotnet-framework-september-2022-security-and-quality-rollup/" rel="nofollow">.NET Framework September 2022 Security and Quality Rollup</a></li>
	<li><a href="https://devblogs.microsoft.com/dotnet/dotnet-framework-august-2022-cumulative-update-preview-updates/" rel="nofollow">.NET Framework August 2022 Cumulative Update Preview Updates</a></li>
	<li><a href="https://devblogs.microsoft.com/dotnet/dotnet-framework-august-2022-security-and-quality-rollup-updates/" rel="nofollow">.NET Framework August 2022 Security and Quality Rollup Updates</a></li>
</ul>
