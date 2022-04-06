---
post_title: .NET Framework April 2022 Security and Quality Rollup Updates
username: salagarw
microsoft_alias: salagarw
featured_image: blogpst-NET.png
categories: .NET Framework, WinForms, ASP.NET
summary: Summary of your post, shown on the home page next to the featured image
desired_publication_date: 2022-04-12
---

We are releasing the April 2022 Security and Quality Rollup Updates for .NET Framework.  


<h3> Security</h3>
<h5>CVE-2022-26832 – .NET Framework Denial of Service</h5>
This security update addresses an issue where an unauthenticated attacker could cause a denial of service on an affected system.

<li><a href="https://msrc.microsoft.com/update-guide/vulnerability/CVE-2022-26832" rel="nofollow">CVE-2022-26832</a></li>  


<h3> Quality and Reliability</h3>

This release contains the following quality and reliability improvements.

<h5> NET Libraries <span style="font-size: 12pt;"></span></h5>
	<ul>
		<li>Addresses an issue when Ssl negotiation can hang indefinitely when client certificates are used when TLS 1.3 is negotiated. Before the change renegotiation (PostHandshakeAuthentiction) would fail and SslStream or HttpWebRequest would observe a timeout.</li>
	</ul>


<h5> Winforms <span style="font-size: 12pt;"></span></h5>
	<ul>
		<li>Addresses a leak of IRawElementProviderSimple objects which was introduced in .NET Framework 4.8. This is an opt-in fix, add the following compatibility switch to the app.config file in order to dispose the accessible objects:

`````` { .html }
        <runtime>
            <!-- AppContextSwitchOverrides values are in the form of 'key1=true|false;key2=true|false  -->
            <AppContextSwitchOverrides value="Switch.System.Windows.Forms.DisconnectUiaProvidersOnWmDestroy=true"/>
        </runtime>
``````

Note: that when the accessibility server application opts into this fix, the accessibility client will receive errors when accessing the disconnected provider. This is expected because the corresponding control window is destroyed. Previous behavior where the provider was returning information for destroyed controls was incorrect.</li>
</ul>


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
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5012121" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5012121" rel="nofollow">5012121</a>
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
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5012123" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5012123" rel="nofollow">5012123</a>
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
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5012117" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5012117" rel="nofollow">5012117</a>
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
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5012117" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5012117" rel="nofollow">5012117</a>
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
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5012117" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5012117" rel="nofollow">5012117</a>
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
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5012120" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5012120" rel="nofollow">5012120</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows 10 1809 (October 2018 Update) and Windows Server 2019</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5012328" rel="nofollow">5012328</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.7.2
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5012128" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5012128" rel="nofollow">5012128</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5, 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5012119" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5012119" rel="nofollow">5012119</a>
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
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5012596" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5012596" rel="nofollow">5012596</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5012118" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5012118" rel="nofollow">5012118</a>
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
       .NET Framework 3.5, 4.6, 4.6.1, 4.6.2
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5012653" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5012653" rel="nofollow">5012653</a>
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
        <strong><a href="https://support.microsoft.com/kb/5012331" rel="nofollow">5012331</a></strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5012326" rel="nofollow">5012326</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5012139" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5012139" rel="nofollow">5012139</a>
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5012152" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5012152" rel="nofollow">5012152</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 4.5.2
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5012142" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5012142" rel="nofollow">5012142</a>
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5012155" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5012155" rel="nofollow">5012155</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 4.6, 4.6.1, 4.6.2, 4.7, 4.7.1, 4.7.2
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5012130" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5012130" rel="nofollow">5012130</a>
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5012147" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5012147" rel="nofollow">5012147</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5012124" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5012124" rel="nofollow">5012124</a>
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5012144" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5012144" rel="nofollow">5012144</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows Server 2012</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5012330" rel="nofollow">5012330</a></strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5012325" rel="nofollow">5012325</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5012136" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5012136" rel="nofollow">5012136</a>
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5012149" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5012149" rel="nofollow">5012149</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 4.5.2
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5012140" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5012140" rel="nofollow">5012140</a>
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5012153" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5012153" rel="nofollow">5012153</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 4.6, 4.6.1, 4.6.2, 4.7, 4.7.1, 4.7.2
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5012129" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5012129" rel="nofollow">5012129</a>
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5012146" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5012146" rel="nofollow">5012146</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5012122" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5012122" rel="nofollow">5012122</a>
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5012143" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5012143" rel="nofollow">5012143</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows 7 SP1 and Windows Server 2008 R2 SP1</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5012329" rel="nofollow">5012329</a></strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5012324" rel="nofollow">5012324</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 3.5.1
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5012138" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5012138" rel="nofollow">5012138</a>
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5012151" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5012151" rel="nofollow">5012151</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 4.5.2
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5012141" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5012141" rel="nofollow">5012141</a>
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5012154" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5012154" rel="nofollow">5012154</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 4.6, 4.6.1, 4.6.2, 4.7, 4.7.1, 4.7.2
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5012131" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5012131" rel="nofollow">5012131</a>
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5012148" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5012148" rel="nofollow">5012148</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 4.8
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5012125" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5012125" rel="nofollow">5012125</a>
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5012145" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5012145" rel="nofollow">5012145</a>
      </td>
    </tr>
    <tr bgcolor="#F0F0F0">
      <td>
        <strong>Windows Server 2008</strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5012332" rel="nofollow">5012332</a></strong>
      </td>
      <td></td>
      <td>
        <strong><a href="https://support.microsoft.com/kb/5012327" rel="nofollow">5012327</a></strong>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 2.0, 3.0
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5012137" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5012137" rel="nofollow">5012137</a>
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5012150" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5012150" rel="nofollow">5012150</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 4.5.2
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5012141" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5012141" rel="nofollow">5012141</a>
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5012154" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5012154" rel="nofollow">5012154</a>
      </td>
    </tr>
    <tr>
      <td>
       .NET Framework 4.6
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5012131" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5012131" rel="nofollow">5012131</a>
      </td>
      <td>
       <a href="http://www.catalog.update.microsoft.com/Search.aspx?q=5012148" rel="nofollow">Catalog</a>
      </td>
      <td>
       <a href="https://support.microsoft.com/kb/5012148" rel="nofollow">5012148</a>
      </td>
    </tr>
  </tbody>
</table>



<br/>    
<h5> Previous Monthly Rollups </h5>
The last few .NET Framework Monthly updates are listed below for your convenience:  

<ul>
	<li><a href="https://devblogs.microsoft.com/dotnet/net-framework-february-2022-cumulative-update-preview/" rel="nofollow">.NET Framework February 2022 Cumulative Update Preview</a></li>
	<li><a href="https://devblogs.microsoft.com/dotnet/net-framework-february-2022-security-and-quality-rollup/" rel="nofollow">.NET Framework February 2022 Security and Quality Rollup</a></li>
	<li><a href="https://devblogs.microsoft.com/dotnet/net-framework-january-2022-cumulative-update-preview/" rel="nofollow">.NET Framework January 2022 Cumulative Update Preview</a></li>
	<li><a href="https://devblogs.microsoft.com/dotnet/net-framework-january-2022-security-and-quality-rollup-updates/" rel="nofollow">.NET Framework January 2022 Security and Quality Rollup Updates</a></li>
</ul>
