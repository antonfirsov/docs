# Advisory on July 2018 .NET Framework Updates

The [September 2018 Security and Quality Rollup](https://portal.msrc.microsoft.com/security-guidance/advisory/CVE-2018-8421) updates for .NET Framework was released earlier this month. We have received multiple customer reports of <START PART TO CHANGE> applications that fail to start or don’t run correctly <END PART TO CHANGE> after installing the September 2018 update. These reports are specific to applications that initialize a COM component and run with restricted permissions. You can reach out to Microsoft Support to get help.  

If you installed the September 2018 update and have not yet seen any negative behavior, we recommend that you leave your systems as-is but closely monitor them and ensure that you apply upcoming .NET Framework updates.

<PLACEHOLDER FOR NEW CONTENT PREVIOUS "BLUE" CONTENT IS HERE RIGHT NOW> As a team, we regret that this release was shipped with this flaw. This release was tested using our regular and extensive testing process. We discovered while investigating this issue that we have a test hole for the specific combination of COM activation and restricted permissions, including impersonation. We will be mitigating that gap going forward. Again, we are sorry for any inconvenience that this product flaw has caused.

We will continue to update this post and [dotnet/announcement #74](https://github.com/dotnet/announcements/issues/74) as we have new information

## Guidance

Temporarily uninstall the September 2018 Security and Quality Rollup updates for .NET Framework to restore functionality until a new update has been released to correct this problem.

## Workaround

<PLACEHOLDER FROM JIM> 

## Symptoms

Workflow 2010 workflows will fail to start and running workflows may fail to continue after installing the .Net patch.  Affected workflows include Out of the Box workflows, such as “Approval – SharePoint 2010” workflows, “Collect Signatures – SharePoint 2010” workflows and “Collect Feedback – SharePoint 2010” workflows (basically, all of the workflows enabled when you turn on the “Workflows” Site Collection Feature).  Workflows using custom code may also be affected, [with Nintex being probably the biggest issue] does Nintex want to be called out?.  This issue is limited to Workflow 2010 workflows; Workflow 2013 should be unaffected.  

The error they will see in ULS [do we know what ULS is?]  for the failed workflows is “Type System.CodeDom.CodeBinaryOperatorExpression is not marked as authorized in the application configuration file”, possibly with a different type name.

## Affected Products

The affected SharePoint versions are SharePoint 2010, 2013, 2016 and the 2019 preview.

### Previous .NET Monthly Rollups

The last few .NET Framework Monthly updates are listed below for your convenience:

* [September 2018 Security and Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2018/09/11/net-framework-september-2018-security-and-quality-rollup/)
* [August 2018 Preview of Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2018/08/30/net-framework-august-2018-preview-of-quality-rollup/)
* [August 2018 Security and Quality Rollup](https://blogs.msdn.microsoft.com/dotnet/2018/08/14/august-2018-security-and-quality-rollup/)
