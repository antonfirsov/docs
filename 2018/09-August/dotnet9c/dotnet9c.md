# .NET Framework September 2018 Security and Quality Rollup

Today, we are releasing the September 2018 Security and Quality Rollup.

## Quality and Reliability

This release contains the following quality and reliability improvements. 

#### CLR
* Updated code to prevent errors regarding invalid date format when Japanese Era 4 is used with a future date [568291]
* Parsing Japanese dates having a year number exceeding the number of years in that date era will succeed instead of throwing errors [603100]
* When asynchronously reading a process output an IndexOutOfRangeException is thrown when less than a character's worth of bytes is read at the beginning of a line [621951]
* Fix in the JIT compiler for a rare case of struct field assignments, described here: https://github.com/Microsoft/dotnet/issues/779 [641182]
* DateTime.Now and DateTime.Utc will now always be synchronized with the system time,  DateTime and DateTimeOffset operations will continue to work as it used to work [645660]
* Spin-waits in several synchronization primitives were conditionally improved to perform better on Intel Skylake and more recent microarchitectures. To enable these improvements, set the new configuration variable COMPlus_Thread_NormalizeSpinWait to 1. [647729]
* Corrected JIT optimization which resulted in removal of interlocked Compare Exchange operation [653568]

#### WPF
* Under certain circumstances, WPF applications using the spell-checker that use custom dictionaries can throw unexpected excpetions and crash [622262]

Note: Additional information on these improvements is not available. The VSTS bug number provided with each improvement is a unique ID that you can give Microsoft Customer Support, include in StackOverflow commentsor use in web searches.

## Getting the Update

The Security and Quality Rollup is available via Windows Update, Windows Server Update Services, Microsoft Update Catalog, and Docker.

### Microsoft Update Catalog

You can get the update via the Microsoft Update Catalog. For Windows 10, .NET Framework updates are part of the Windows 10 Monthly Rollup.

The following table is for Windows 10 and Windows Server 2016+ versions.

<table>
<thead><tr>
<th>Product Version</th><th>Security and Quality Rollup KB</th>
</tr></thead>
<tr>
<td><strong>Windows 10 1803 (April 2018 Update)</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4458469">Catalog</a><BR><a href="https://support.microsoft.com/kb/4458469">4458469</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5</td><td><a href="https://support.microsoft.com/kb/4458469">4458469</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.7.2</td><td><a href="https://support.microsoft.com/kb/4458469">4458469</a></td>
</tr>
<tr>
<td><strong>Windows 10 1709 (Fall Creators Update)</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4457136">Catalog</a><BR><a href="https://support.microsoft.com/kb/4457136">4457136</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5</td><td><a href="https://support.microsoft.com/kb/4457136">4457136</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.7.1</td><td><a href="https://support.microsoft.com/kb/4457136">4457136</a></td>
</tr>
<tr>
<td><strong>Windows 10 1703 (Creators Update)</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4457141">Catalog</a><BR><a href="https://support.microsoft.com/kb/4457141">4457141</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5</td><td><a href="https://support.microsoft.com/kb/4457141">4457141</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.7, 4.7.1</td><td><a href="https://support.microsoft.com/kb/4457141">4457141</a></td>
</tr>
<tr>
<td><strong>Windows 10 1607 (Anniversary Update)<br> Windows Server 2016</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4457127">Catalog</a><BR><a href="https://support.microsoft.com/kb/4457127">4457127</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5</td><td><a href="https://support.microsoft.com/kb/4457127">4457127</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6.2, 4.7, 4.7.1</td><td><a href="https://support.microsoft.com/kb/4457127">4457127</a></td>
</tr>
</table>

The following table is for earlier Windows and Windows Server versions.

<table>
<thead><tr>
<th>Product Version</th><th>Preview of Quality Rollup KB</th>
</tr></thead>
<tr>
<td><strong>Windows 8.1<BR>Windows RT 8.1<BR>Windows Server 2012 R2</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4458613">Catalog</a><BR><a href="https://support.microsoft.com/kb/4458613">4458613</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5</td><td><a href="https://support.microsoft.com/kb/4457009">4457009</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.5.2</td><td><a href="https://support.microsoft.com/kb/4457017">4457017</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6, 4.6.1, 4.6.2, 4.7, 4.7.1</td><td><a href="https://support.microsoft.com/kb/4458612">4458612</a></td>
</tr>
<tr>
<td><strong>Windows Server 2012</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4458612">Catalog</a><BR><a href="https://support.microsoft.com/kb/4458612">4458612</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5</td><td><a href="https://support.microsoft.com/kb/4457008">4457008</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.5.2</td><td><a href="https://support.microsoft.com/kb/4457018">4457018</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6, 4.6.1, 4.6.2, 4.7, 4.7.1</td><td><a href="https://support.microsoft.com/kb/4457014">4457014</a></td>
</tr>
<tr>
<td><strong>Windows 7<BR>Windows Server 2008 R2</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4458611">Catalog</a><BR><a href="https://support.microsoft.com/kb/4458611">4458611</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5.1</td><td><a href="https://support.microsoft.com/kb/4457008">4457008</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.5.2</td><td><a href="https://support.microsoft.com/kb/4457019">4457019</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6, 4.6.1, 4.6.2, 4.7, 4.7.1</td><td><a href="https://support.microsoft.com/kb/4457014">4457014</a></td>
</tr>
<tr>
<td><strong>Windows Server 2008</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4458614">Catalog</a><BR><a href="https://support.microsoft.com/kb/4458614">4458614</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 2.0, 3.0</td><td><a href="https://support.microsoft.com/kb/4457007">4457007</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.5.2</td><td><a href="https://support.microsoft.com/kb/4457019">4457019</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6</td><td><a href="https://support.microsoft.com/kb/4457014">4457014</a></td>
</tr>
</table>

### Previous Monthly Rollups

The last few .NET Framework Monthly updates are listed below for your convenience:

* [September 2018 Security and Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2018/09/11/net-framework-september-2018-security-and-quality-rollup/)
* [August 2018 Preview of Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2018/08/30/net-framework-august-2018-preview-of-quality-rollup/)
* [August 2018 Security and Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2018/08/14/august-2018-security-and-quality-rollup/)
* [July 2018 Update](https://blogs.msdn.microsoft.com/dotnet/2018/07/30/net-framework-july-2018-update/)
