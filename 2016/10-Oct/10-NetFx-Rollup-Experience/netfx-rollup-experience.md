# Upcoming .NET Framework Security and Quality Rollup Changes

We [recently introduced](https://blogs.msdn.microsoft.com/dotnet/2016/08/15/introducing-the-net-framework-monthly-rollup/) the .NET Framework Security and Quality Rollup, a simplified way for you to obtain all quality and security updates. The first release of the Security and Quality Rollup is just around the corner on Tuesday, October 11th. We have gotten a few questions around the new model which we thought would be helpful to everyone.  

## The Three Rollup Types

There are three rollup types that may be available throughout the month. Not every month will yield a monthly rollup nor will every rollup apply to all supported versions of the .NET Framework.

### Security and Quality Rollup
As you may recall, the Security and Quality Rollup is a cumulative set of updates for all [supported](https://support.microsoft.com/en-us/gp/framework_faq/en-us) (and applicable) versions of the .NET Framework.  The Security and Quality Rollup will supersede the previous rollup, making it easy to catch up if you have missed any. They will never install a different version of the .NET Framework than what is currently on your computer. 

### Security Only Rollup
For those who prefer to take only the security updates, you can download the Security Only Update on Windows Server Update Services and Microsoft Update Catalog. It's important to note that the Security Only Update  will not supersede the previous Security Only Update or be superseded by the Security and Quality Rollup. This enables you fine tune the security updates that are applied.  

### Quality Preview Rollup
Lastly, you will be able to download a preview of upcoming .NET Framework quality changes that are scheduled to be included in the next .NET Framework Monthly Rollup. The Quality Preview Rollup will be available on Windows Update, Windows Server Update Services and Microsoft Update Catalog the third Tuesday of the month. The Quality Preview Rollup will always supersede previous Quality Preview Rollups and will be superseded by future Security and Quality Rollups.

## Installation

With the new model, there will no longer be induvial KB's available on Windows Update, Windows Server Update Services or Microsoft Update Catalog. Instead, you will see a single item per rollup type:
 
[IMG]

Even though the Security and Quality Rollup is a single installation, it is possible after the update has been applied to remove the rollup for a version of the .NET Framework. For example, if you installed the Security and Quality Rollup and you have .NET Framework 3.5 and 4.6.2 installed, you can uninstall the .NET Framework 3.5 Security and Quality Rollup, leaving the .NET Framework 4.6.2 Security and Quality Rollup on your computer. This can be done by removing the Security and Quality Rollup that appears in Add Remove Programs (ARP) as a KB:

[IMG]
