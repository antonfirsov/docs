# .NET Framework Security and Quality Rollup - October 2016

We [recently introduced](https://blogs.msdn.microsoft.com/dotnet/2016/08/15/introducing-the-net-framework-monthly-rollup/) the .NET Framework Security and Quality Rollup, a simplified way for you to obtain all quality and security updates. The first release of the Security and Quality Rollup and Security-Only Update is now available and the details of what is included in the release are [below](xxx). We have also gotten a few questions around the new model and we thought the answers would be helpful for everyone.  

## The Three Rollup Types

There are three rollup types that may be available throughout the month. Not every month will yield a monthly rollup nor will every rollup apply to all supported versions of the .NET Framework.

### Security and Quality Rollup
The Security and Quality Rollup is a cumulative set of updates for all [supported](https://support.microsoft.com/en-us/gp/framework_faq/en-us) (and applicable) versions of the .NET Framework.  The Security and Quality Rollup will contain the updates from all the previous rollups, making it easy to catch up if you have missed any. They will never install a different version of the .NET Framework than what is currently on your computer.

### Security-Only Update
For those who prefer to take only the security updates, you can download the Security Only Update on Windows Server Update Services and Microsoft Update Catalog. It's important to note that the Security Only Update will not contain the updates from the previous Security Only Update or be those included in the Security and Quality Rollup. This enables you to fine-tune the security updates that are applied.  

### Quality Preview Rollup
Lastly, you will be able to download an optional preview of upcoming .NET Framework quality changes that are scheduled to be included in the next .NET Framework Monthly Rollup. The Quality Preview Rollup will be available on Windows Update, Windows Server Update Services and Microsoft Update Catalog typically on the third Tuesday of the month. The Quality Preview Rollup will always replace previous Quality Preview Rollups and will be replaced by future Security and Quality Rollups.

## Installation

With the new model, there will no longer be individual updates for each .NET Framework version available on Windows Update, Windows Server Update Services or Microsoft Update Catalog. Instead, you will see a single item for each operating system:
 
[IMG PENDING]

Even though the Security and Quality Rollup appears as a single installation, it is possible to remove the rollup for a specific version of the .NET Framework after the update has been applied. For example, if you installed the Security and Quality Rollup and you have .NET Framework 3.5 and 4.6.2 installed, you can uninstall the .NET Framework 3.5 Security and Quality Rollup, leaving only the .NET Framework 4.6.2 Security and Quality Rollup on your computer. This can be done by removing the Security and Quality Rollup that appears in Add or Remove Programs (ARP):

[IMG PENDING]

## October 2016 Release Notes
The following is a list of updates included in the .NET Framework Security and Quality Rollup and Security-Only Update. This month's Security and Quality Rollup contains all past hotfixes for .NET Framework 4.5.x and 4.6.x. It does not contain all past hotfixes for .NET Framework 3.5 as those will be introduced naturally if/when new changes are necessary for those components. Additional information can be found on the Knowledge Base [article](XXX). 

* TODO: Add when provided 