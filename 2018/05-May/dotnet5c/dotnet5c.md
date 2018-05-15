# .NET Framework May 2018 Preview of Quality Rollup

Today, we are releasing the May 2018 Preview of Quality Rollup.

## Quality and Reliability

This release contains the following quality and reliability improvements.

### CLR

* Resolves an issue in WindowsIdentity.Impersonate where handles were not being explicitly cleaned up. [581052]
* Resolves an issue in deserialization when using a collection, for example, ConcurrentDictionary by ignoring casing. [524135]
* Removes case where floating-point overflow occurs in the thread pool’s hill climbing algorithm. [568704]
* Resolves instances of high CPU usage with background garbage collection. This can be observed with the following two functions on the stack: `clr!*gc_heap::bgc_thread_function`, `ntoskrnl!KiPageFault`. Most of the CPU time is spent in the `ntoskrnl!ExpWaitForSpinLockExclusiveAndAcquire` function. This change updates background garbage collection to use the CLR implementation of write watch instead of the one in Windows. [574027]

### Networking

* Fixed a problem with connection limit when using HttpClient to send requests to loopback addresses. [539851]

### WPF

* A crash can occur during shutdown of an application that hosts WPF content in a separate AppDomain.  (A notable example of this is an Office application hosting a VSTO add-in that uses WPF.) [543980]
* Addresses an issue that caused XAML Browser Applications (XBAP’s) targeting .NET 3.5 to sometimes be loaded using .NET 4.x runtime incorrectly. [555344]
* A WPF application can crash due to a NullReferenceException if a Binding (or MultiBinding) used in a DataTrigger (or MultiDataTrigger) belonging to a Style (or Template, or ThemeStyle) reports a new value, but whose host element gets GC'd in a very narrow window of time during the reporting process. [562000]
* A WPF application can crash due to a spurious ElementNotAvailableException. This can arise if:
     1.Change TreeView.IsEnabled
     2.Remove an item X from the collection
     3.Re-insert the same item X back into the collection
     4.Remove one of X's subitems Y from its collection
(Step 4 can happen any time relative to steps 2 and 3, as long as it's after step 1.  Steps 2-4 must occur before the asynchronous call to UpdatePeer, posted by step 1;  this will happen if steps 1-4 all occur in the same button-click handler.) [555225]

Note: Additional information on these improvements is not available. The [VSTS](https://www.visualstudio.com/team-services/) bug number provided with each improvement is a unique ID that you can give [Microsoft Customer Support](https://support.microsoft.com/contactus/), include in [StackOverflow comments](https://stackoverflow.com/questions/tagged/.net) or use in web searches.

## Getting the Update

The Security and Quality Rollup is available via Windows Update, Windows Server Update Services, Microsoft Update Catalog, and Docker.

### Microsoft Update Catalog

You can get the update via the Microsoft Update Catalog.

<table>
<thead><tr>
<th>Product Version</th><th>Preview of Quality Rollup KB</th>
</tr></thead>
<tr>
<td><strong>Windows 10 1709 (Fall Creators Update)</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4103714">Catalog</a><BR><a href="https://support.microsoft.com/kb/4103714">4103714</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.7.1</td><td><a href="https://support.microsoft.com/kb/4103714">4103714</a></td>
</tr>
<tr>
<td><strong>Windows 10 1703 (Creators Update)</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4103722">Catalog</a><BR><a href="https://support.microsoft.com/kb/4103722">4103722</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.7, 4.7.1</td><td><a href="https://support.microsoft.com/kb/4103722">4103722</a></td>
</tr>
<tr>
<td><strong>Windows 10 1607 (Anniversary Update)</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4103720">Catalog</a><BR><a href="https://support.microsoft.com/kb/4103720">4103720</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6.2, 4.7, 4.7.1</td><td><a href="https://support.microsoft.com/kb/4103720">4103720</a></td>
</tr>
<tr>
<td><strong>Windows 8.1<BR>Windows RT 8.1<BR>Windows Server 2012 R2</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4103473">Catalog</a><BR><a href="https://support.microsoft.com/kb/4103473">4103473</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5</td><td><a href="https://support.microsoft.com/kb/4095875">4095875</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.5.2</td><td><a href="https://support.microsoft.com/kb/4098974">4098974</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6, 4.6.1, 4.6.2, 4.7, 4.7.1, 4.7.1</td><td><a href="https://support.microsoft.com/kb/4098972">4098972</a></td>
</tr>
<tr>
<td><strong>Windows Server 2012</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4098968">Catalog</a><BR><a href="https://support.microsoft.com/kb/4098968">4098968</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5</td><td><a href="https://support.microsoft.com/kb/4095872">4095872</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.5.2</td><td><a href="https://support.microsoft.com/kb/4098975">4098975</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6, 4.6.1, 4.6.2, 4.7, 4.7.1, 4.7.1</td><td><a href="https://support.microsoft.com/kb/4098971">4098971</a></td>
</tr>
<tr>
<td><strong>Windows 7<BR>Windows Server 2008 R2</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4103472">Catalog</a><BR><a href="https://support.microsoft.com/kb/4103472">4103472</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 3.5.1</td><td><a href="https://support.microsoft.com/kb/4095874">4095874</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.5.2</td><td><a href="https://support.microsoft.com/kb/4098976">4098976</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6, 4.6.1, 4.6.2, 4.7, 4.7.1, 4.7.1</td><td><a href="https://support.microsoft.com/kb/4096234">4096234</a></td>
</tr>
<tr>
<td><strong>Windows Server 2008</strong></td><td><strong><a href ="http://www.catalog.update.microsoft.com/Search.aspx?q=4103474">Catalog</a><BR><a href="https://support.microsoft.com/kb/4103474">4103474</a></strong></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 2.0, 3.0</td><td><a href="https://support.microsoft.com/kb/4095873">4095873</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.5.2</td><td><a href="https://support.microsoft.com/kb/4098976">4098976</a></td>
</tr>
<tr>
<td style="padding-left:.5cm">.NET Framework 4.6</td><td><a href="https://support.microsoft.com/kb/4096234">4096234</a></td>
</tr>
</table>

### Previous Monthly Rollups

The last few .NET Framework Monthly updates are listed below for your convenience:

* [May 2018 Security and Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2018/05/08/net-framework-may-2018-security-and-quality-rollup/)
* [February 2018 Security and Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2018/02/13/net-framework-february-2018-security-and-quality-rollup/)
* [January 2018 Preview of Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2018/01/26/january-2018-preview-of-quality-rollup/)
* [January 2018 Security and Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2018/01/09/net-framework-january-2018-security-and-quality-rollup/)
