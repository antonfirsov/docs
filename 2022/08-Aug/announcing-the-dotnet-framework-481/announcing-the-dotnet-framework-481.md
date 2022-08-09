---
post_title: Announcing .NET Framework 4.8.1
author1: taraoverfield
post_slug: announcing-dotnet-framework-481
username: taraoverfield
microsoft_alias: tarao
featured_image: framework481.png
categories: .NET Framework, Maintenance & Updates
summary: Announcing the release of .NET Framework 4.8.1 that bring ARM64 support, WinForms accessibility enhancements, and more.
desired_publication_date: 2022-08-09
---

We are excited to announce the release of the .NET Framework 4.8.1 today. It’s included in the Visual Studio 2022 17.3 release and .NET Framework 4.8.1 is also available to download for Windows 10 Version 20H2+ and Windows Server 2022+.

You can install .NET Framework 4.8.1 from our<a href="https://dotnet.microsoft.com/download/dotnet-framework" target="_blank" rel="noopener"> .NET Framework Download site</a>. For building applications targeting .NET Framework 4.8.1, you can download the <a href="https://go.microsoft.com/fwlink/?linkid=2203306" target="_blank" rel="noopener">NET Framework 4.8.1 Developer Pack</a>. If you just want the runtime, you can use either:
<ul>
 	<li><a href="http://go.microsoft.com/fwlink/?LinkId=2203304" target="_blank" rel="noopener">.NET Framework 4.8.1 Web Installer </a>– requires an internet connection during installation</li>
 	<li><a href="https://go.microsoft.com/fwlink/?linkid=2203305" target="_blank" rel="noopener">.NET Framework 4.8.1 Offline installer</a> – can be downloaded and installed later in a disconnected state</li>
</ul>
Additionally, .NET Framework 4.8.1 is included in the latest version of Visual Studio, <a href="https://visualstudio.microsoft.com/vs/" target="_blank" rel="noopener">Visual Studio 2022 17.3</a>.
<br/><br/>
.NET Framework 4.8.1 includes native support for the Arm64 architecture (Windows 11+) and accessibility improvements as well as other improvements. You can see the complete list of improvements in the <a href="https://github.com/Microsoft/dotnet/tree/master/releases/net481/README.md" target="_blank" rel="noopener">.NET Framework 4.8.1 release notes</a>. 

<h3 id="supported-windows-versions" class="">Supported Windows Versions<i class="fabric-icon fabric-icon--Link" aria-hidden="true"></i></h3>
<p class=""><strong class="">Windows Client versions: </strong>Windows 11, Windows 10 version 21H2, Windows 10 version 21H1, Windows 10 version 20H2</p>
<p><strong>Windows Server versions:</strong> Windows Server 2022</p>

<h3 id="new-features-in-net-framework-4-8-1">New Features in .NET Framework 4.8.1</h3>
<h5 class="">Native support for Arm64</h5>
.NET Framework 4.8.1 adds native Arm64 support to the .NET Framework family. So, your investments in the vast ecosystem of .NET Framework apps and libraries can now leverage the benefits of running workloads natively on Arm64 for better performance when compared to running x64 code emulated on Arm64.
<h5 class="">WCAG2.1 compliant accessible tooltips</h5>
<p>
Microsoft has a commitment to providing products and platforms that are <a href = "https://www.microsoft.com/accessibility/">accessible to everyone</a>. .NET Framework 4.8.1 provides two Windows UI development platforms, both of which provide developers with the support necessary to create accessible applications for their users. Over the past several releases, both Windows Forms and WPF have added several features and fixed numerous reliability issues related to accessibility. You can read more about the details of what we fixed or added in each release by visiting <a href="https://docs.microsoft.com/dotnet/framework/whats-new/whats-new-in-accessibility">What's new in accessibility in .NET Framework</a>.
</p>
In this release, both Widows Forms and WPF have made improvements to the handling of tooltips to enable them to be more accessible. In both cases, tooltips now comply with the guidelines set 
forth in the <a href="https://www.w3.org/WAI/WCAG21/Understanding/content-on-hover-or-focus.html">WCAG2.1 content on Hover or Focus</a> guidance. The requirements for tooltips require the following:
<UL>
<LI>Tooltips must display either via mouse hover or by keyboard navigation to the control.</LI>
<LI>Tooltips should be <b>dismissable</b>. That is, a simple keyboard command like the ESC key should dismiss the tooltip. </LI> 
<LI>Tooltips should be <b>hoverable</b>. Users should be able to place their mouse cursor over the tooltip. This enables scenarios like using magnifier to be able to read the tooltip for low-vision users.</LI>
<LI>Tooltips should be <b>persistent</b>. Tooltips should not automatically disappear after a certain time has elapsed. Rather, the tooltips should be dismissed by the user moving their mouse to another control, or by dismissing the tooltip as described above. </LI>
</UL>
<p>
In WinForms, this support is only available on Windows 11 or higher operating system. WinForms is a thin managed wrapper around the Windows API, and the new tooltip behavior only became available in Windows 11. WPF has no operating system version dependencies for their accessible tooltips.
</p>
<p>
WPF had implemented most of the requirements for WCAG2.1 compliant tooltips in .NET Framework 4.8. In this release WPF improved the experience by ensuring that a tooltip in the current window can easily be dismissed by using the ESC key, the CTRL key (by itself), or by the combination Ctrl+Shift+F10. The scope of the Escape key was reduced in this release to apply only to the current window, when previously it would have been any open tooltip in the application.
</p>
<h5 class="">Windows Forms – Accessibility Improvements</h5>
<P>Windows Forms was the first Windows UI stack created for .NET Framework. As such, it was originally created to utilize legacy accessibility technology, which doesn't meet current accessibility requirements. In this release, WinForms has addressed a number of issues. For a complete list of the accessibility related changes, visit <a href="https://docs.microsoft.com/dotnet/framework/whats-new/whats-new-in-accessibility">What's new in accessibility in .NET Framework</a></P>. Here we'll focus on the highlights of what WinForms has done in .NET Framework 4.8.1.
<UL>
    <LI><u><A href="https://docs.microsoft.com/windows/win32/winauto/uiauto-implementingtextandtextrange">Text Pattern Support</u></A>-  In this release, WinForms added support for the UIA Text Pattern. This pattern enables assistive technology to traverse the content of a TextBox or similar text-based control letter by letter. It enables text to be selected within the control and changed, as well as new text inserted at the cursor. WinForms added this support for TextBox, DataGridView cells, ComboBox controls and more.</LI>
    <LI><u>Addressing Contrast issues</u>- We've addressed high contrast issues in several controls and have changed the contrast ratio of selection rectangles to be darker and more visible.</LI>
    <LI><u>Fixed several DataGridView issues</u>- In this release, we've updated the scrollbar names to be consistent. We've addressed an issue where Narrator was unable to focus on empty DataGridView cells. Developers are now able to set the localized control type property for Custom DataGridView cells. The link color for DataGridViewLink cells has been updated to have better contrast with the background.</LI>

</UL>

<h3></h3>
<h3>Known issues</h3>
<table style="width: 75.1578%; height: 152px;" border="1" cellspacing="0" cellpadding="8px">
<tbody>
<tr>
<td style="width: 22.0012%;" width="29%"><strong>Symptoms</strong></td>
<td style="width: 178.29%;" width="70%">The .NET Framework 4.x WCF optional components on Windows 11 ARM64 client machines will fail to be enabled either through the dism command or add/remove program UI. </td>
</tr>
<tr>
<td style="width: 22.0012%;" width="29%"><strong>Workaround</strong></td>
<td style="width: 178.29%;" width="70%"> No workaround available</td>
</tr>
<tr>
<td style="width: 22.0012%;" width="29%"><strong>Resolution</strong></td>
<td style="width: 178.29%;" width="70%"> A resolution to this issue will be included in an upcoming release
<p><Strong>Note</strong>: Message Queuing (MSMQ) Activation will remain disabled because MSMQ is not available on Windows 11 for ARM64 client machines</p>
</td>
</tr>
</tbody>
</table>
<h3></h3>
<h3>Frequently Asked Questions (FAQs)</h3>
<p>If I don't upgrade to .NET Framework 4.8.1 will anything change for how I receive Windows or .NET Framework updates?</p>
<ul>
<li>No. Updates for previous versions of .NET Framework and for Windows operating systems components remain the same.</li>
</ul>
<p>I am an IT Administrator managing updates for my organization, how do I ensure that my deployments include all existing versions of .NET Framework?</p>
<ul>
<li>As noted above, continue to rely on the same mechanisms for Windows and .NET Framework updates. Ensure that within your WSUS, SCCM or similar environment, you select updates that correspond to the <strong>“Windows” Product</strong> and continue to rely on the <strong>Classifications</strong> categories to select all applicable updates that align with your organization’s update criteria for Security and non-security content. This will ensure you continue to receive updates for all .NET Framework versions.</li>
</ul>
<p>Does anything change about the way updates to .NET Framework 3.5 get delivered once I upgrade to .NET Framework 4.8.1?</p>
<ul>
<li>The .NET Framework 3.5 updates are included in the .NET Framework Cumulative Update and will not be affected by the upgrade to .NET Framework 4.8.1. </li> 
</ul>
<br></br>
<p>Please try out these improvements in the .NET Framework 4.8.1 and share your feedback in the comments below or via <a href="https://github.com/Microsoft/dotnet/issues/" target="_blank" rel="noopener">GitHub</a>.</p>

<p>Thank you!</p>
