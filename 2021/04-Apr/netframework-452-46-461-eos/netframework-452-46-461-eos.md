---
post_title: '.NET Framework 4.5.2, 4.6, 4.6.1 will reach End of Support on April 26, 2022'
username: jamshedd@microsoft.com
microsoft_alias: jamshedd
categories: .NET, .NET Framework, Support, Lifecycle
summary: .NET Framework 4.5.2, 4.6, 4.6.1 will reach End of Support on April 26, 2022. You need to take action to update your .NET Framework runtime to a supported version of .NET Framework.
desired_publication_date: '2021-04-26'
---

.NET Framework 4.5.2, 4.6, and 4.6.1 will be reaching end of support* on April 26, 2022. After this date, we will no longer provide updates including security fixes or technical support for these versions.

__*Customers currently using .NET Framework 4.5.2, 4.6, or 4.6.1 need to update their deployed runtime to a more recent version - at least .NET Framework 4.6.2 before April 26, 2022 - in order to continue to receive updates and technical support.*__

> *Windows 10 Enterprise LTSC 2015 shipped with .NET Framework 4.6 built into the OS. This OS version is a long-term servicing channel (LTSC) release. We will continue to support .NET 4.6 on Windows 10 Enterprise LTSC 2015 through end of support of the OS version (October 2025).

There is no change to the support timelines for any other .NET Framework version, including .NET 3.5 SP1, which will continue to be supported as documented on our [.NET Framework Lifecycle FAQ](https://docs.microsoft.com/lifecycle/faq/dotnet-framework).

## Why are we doing this?

The .NET Framework was previously digitally signed using certificates that use the Secure Hash Algorithm 1 (SHA-1). SHA-1 is a legacy cryptographic hashing algorithm that is no longer deemed secure. We are retiring content that were signed using digital certificates that used SHA-1 to support evolving industry standards. 

After looking at download and usage data across the different versions of .NET Framework, we found that updating .NET Framework 4.6.2 and newer versions to support newer digital certificates (for the installers) would satisfy the vast majority (98%) of users without them needing to make a change. The small set of users using .NET Framework 4.5.2, 4.6, or 4.6.1 will need to upgrade to a later .NET Framework version to stay supported. Applications do not need to be recompiled. Given the nature of this change, we decided that targeting .NET Framework 4.6.2 and later was the best balance of support and effort on our part. 

See this support article on [retiring SHA-1 content](https://aka.ms/framework-sha1-retirement) for more information.

When .NET Framework 4.5.2, 4.6, and 4.6.1 reach end of support, applications that run on top of these versions will continue to run. Starting May 2022, we won't be issuing security updates for .NET Framework 4.5.2, 4.6, and 4.6.1 when we issue these security updates for .NET Framework 4.6.2 and later versions. This means that starting May 2022,    if a computer has .NET Framework 4.5.2, 4.6, or 4.6.1 installed, it may be unsecure. Additionally, if you run into any issue and need technical support, you will be asked to first upgrade to a supported version.

.NET Framework 4.6.2 shipped nearly 5 years ago, and .NET 4.8 shipped 2 years ago, so both versions are solid, stable runtimes for your applications. .NET Framework 4.6.2 and 4.8 are highly compatible in-place updates (replacements) for .NET 4.5.2, 4.6, and 4.6.1 and broadly deployed to hundreds of millions of   computers via Windows Update (WU). If your computer is configured to take the  latest updates from WU your application is likely already running on .NET Framework 4.8.

If you have not deployed .NET Framework 4.6.2 or a later version yet, you only need to update the runtime on which the application is running to a minimum version of 4.6.2 to stay supported. If your application was built to target .NET Framework 4 – 4.6.1, it should continue to run on .NET Framework 4.6.2 and later without any changes in most cases. There is no need for you to retarget or recompile against .NET 4.6.2. That said, we strongly recommend you validate that the functionality of your app is unaffected when running on the newer runtime version before you deploy the updated runtime in your production environment.

## Resources

Here are some other resources you may find helpful:

* [.NET Framework Downloads](https://dotnet.microsoft.com/download/dotnet-framework)
* [NET Framework Application Compatibility](https://docs.microsoft.com/dotnet/framework/migration-guide/application-compatibility)
* [Runtime changes between .NET 4.5.2 and .NET 4.6.2](https://docs.microsoft.com/dotnet/framework/migration-guide/runtime/4.5.2-4.6.2)
* [.NET Framework Migration Guide](https://docs.microsoft.com/dotnet/framework/migration-guide)

We are committed to help you ensure your apps work on the latest versions of our software. Should you have any questions that remain unanswered, we’re here to help. You should engage with Microsoft Support through your regular channels for a resolution.

Additionally, if you run into compatibility or app issues as you transition to .NET Framework 4.6.2 or later, there’s [App Assure](https://www.microsoft.com/fasttrack/microsoft-365/app-assure). We’ll help you resolve compatibility issues at no additional cost. You can [contact App Assure](https://fasttrack.microsoft.com/portal#/signin) for remediation support or by email if you experience any challenges submitting your request ([ACHELP@microsoft.com](mailto:ACHELP@microsoft.com)). 

You may also want to look at [this FAQ](https://aka.ms/framework-452-46-461-eos-faq) for more detailed answers or questions not covered in this post.

## Closing

.NET Framework 4.5.2, 4.6, and 4.6.1 will be reaching end of support on April 26, 2022 and after this date we will no longer provide updates including security fixes or technical support for these versions. We strongly recommend you migrate your applications to at least .NET Framework 4.6.2 or higher before this date.
