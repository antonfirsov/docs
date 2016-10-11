# .NET Framework Monthly Rollup

We recently introduced the [.NET Framework Monthly Rollup](https://blogs.msdn.microsoft.com/dotnet/2016/08/15/introducing-the-net-framework-monthly-rollup/). It's a new and easier way for you to install all applicable .NET Framework updates in a single step. 

This post describes the three monthly updates types that you can install. It shows you what the install process looks like. Last, it addresses some common questions that we have heard since introducing the new model.

The introduction of these new monthly releases aligns with a similar set of [monthly Windows releases](https://blogs.technet.microsoft.com/windowsitpro/2016/10/07/more-on-windows-7-and-windows-8-1-servicing-changes/) that you can also learn more about.

## Monthly Releases

There are three kinds of release that you can choose from. You can read the descriptions below to help you pick the best one for your situation.

### Security and Quality Rollup
The Security and Quality Rollup is recommended for consumer and developer machines. It includes both security  and quality improvements. It is cumulative, meaning that it contains all of the updates from previous rollups, making it easy to catch up if you have missed any of the previous updates. Security and Quality Rollup release will be made available on Windows Update and Windows Update Catalog.

- When?: Second Tuesday of the month (Patch Tuesday).
- Where?: Windows Update, Microsoft Update Catalog.
- Cumulative: Yes.
- Contents: Security and/or quality improvements.

### Security-Only Update
The Security-Only Update is recommended for production machines. It contains only security updates and only updates for that month. This enables you to fine-tune the security updates that are applied. If you have installed the Security and Quality Rollup for the month, then you are up to date and do not need to install the Security-Only Update. The Security-Only Update will be made available on Windows Server Update Services and Microsoft Update Catalog. 

- When?: Second Tuesday of the month (Patch Tuesday).
- Where?: Windows Server Update Services, Microsoft Update Catalog.
- Cumulative: No.
- Contents: Security improvements.

### Quality Rollup
The Quality Rollup is recommended for large businesses that want to use and/or preview quality improvements as soon as they become available. These same quality improvements will typically be included in the following Security and Quality Rollup, approxiately three weeks later. The Quality Rollup will be made available on Windows Update, Windows Server Update Services and Microsoft Update Catalog.

- When?: Third Tuesday of the Month (one week after Patch Tuesday).
- Where?: Windows Update, Windows Server Update Services and Microsoft Update Catalog.
- Cumulative: Yes.
- Contents: Quality improvements.

## More Information

The following information answers common questions.

### Supported Versions

These new releases apply to [supported .NET Framework versions](https://support.microsoft.com/en-us/gp/framework_faq/en-us). This means that you need to install a supported version of the .NET Framework to get these updates. At the time of writing, the supports versions are:

- .NET Framework 3.5 SP1
- .NET Framework 4.5.2 or later

## Cadence

We intend to ship updates monthly, however, we will only release updates if we have made changes. Also, some changes only apply to a subset of .NET Framework versions and/or Windows versions. You can always check this blog to see if an update has been released.

## Updating to a later .NET Framework

These updates include patch-level changes. They will not change the .NET Framework that is currently installed on your computer. For example, if you have .NET Framework 4.5.2, you will still have .NET Framework 4.5.2 after installing one of these updates. If you want to upgrade to a later .NET Framework version, you must install it manually.

## Installation

You will see a single item for each operating system:
 
![qualitysecurity](qualitysecurityrollup.png)
*Available Security and Quality Rollup on Windows Server 2008 SP2*

![securityOnly](securityonlyupdate.png)
*Available Security-Only Update on Windows Server 2008 SP2*

Even though the Security and Quality Rollup appears as a single installation, it is possible to remove the rollup or security-only update for a specific version of the .NET Framework after the update has been applied. For example, if you installed the Security and Quality Rollup and you have .NET Framework 3.5 and 4.6.2 installed, you can uninstall the .NET Framework 3.5 Security and Quality Rollup, leaving only the .NET Framework 4.6.2 Security and Quality Rollup on your computer. This can be done by removing the Security and Quality Rollup that appears in Add or Remove Programs (ARP):

![securityOnlyARP](SecurityOnlyARP.png)
*Installed Security-Only Update on WIndows Server 2008 SP2*
