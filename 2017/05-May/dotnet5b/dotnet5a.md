Text to be inserted into the 4B Blog post

### Known Issue with the April 2017 Update

The [April 2017 Monthly Update](https://blogs.msdn.microsoft.com/dotnet/2017/04/11/net-framework-april-2017-monthly-rollup/) contained a bug that caused the [PowerShell Stop-Computer command to stop correctly functioning](https://support.microsoft.com/en-us/help/4020459/after-you-apply-april-2017-security-updates-from-cve-2017-0160). This bug has since been fixed. You can get the fix in the following ways:

**Using Windows 10**
- Install the [May 2017 Update](https://blogs.msdn.microsoft.com/dotnet/).

**Using an earlier version of Windows**
- Wait for the next .NET Framework monthly update, which will include this fix. This approach is recommended if you are not experiencing this problem.
- Install the specific fix for this issue, listed below.

|Windows Version |.NET Version|KB Number |
|:---------------|:-----------|:---------|
|Windows 8.1 <br> Windows Server 2012 R2|.NET Framework 4.6.2 |[4020499](https://support.microsoft.com/kb/4020499) | 
|Windows 8.1 <br> Windows Server 2012 R2|.NET Framework 4.6 and 4.6.1 |[4020502](https://support.microsoft.com/kb/4020502) | 
|Windows 8.1 <br> Windows Server 2012 R2|.NET Framework 4.5.2 |[4020505](https://support.microsoft.com/kb/4020505) | 
|Windows 8.1 <br> Windows Server 2012 R2|.NET Framework 3.5 |[4020514](https://support.microsoft.com/kb/4020514) | 
|Windows Server 2012|.NET Framework 4.6.2 |[4020498](https://support.microsoft.com/kb/4020498) | 
|Windows Server 2012|.NET Framework 4.6 and 4.6.1 |[4020501](https://support.microsoft.com/kb/4020501) | 
|Windows Server 2012|.NET Framework 4.5.2 |[4020506](https://support.microsoft.com/kb/4020506) | 
|Windows Server 2012|.NET Framework 3.5 |[4020512](https://support.microsoft.com/kb/4020512) | 
|Windows 7 <br> Windows Server 2008 R2|.NET Framework 4.6.2 |[4020500](https://support.microsoft.com/kb/4020500) | 
|Windows 7 <br> Windows Server 2008 R2|.NET Framework 4.6 and 4.6.1 |[4020503](https://support.microsoft.com/kb/4020503) | 
|Windows 7 <br> Windows Server 2008 R2|.NET Framework 4.5.2 |[4020507](https://support.microsoft.com/kb/4020507) | 
|Windows 7 <br> Windows Server 2008 R2|.NET Framework 3.5.1 |[4020513](https://support.microsoft.com/kb/4020513) | 
|Windows Server 2008|.NET Framework 4.6 |[4020503](https://support.microsoft.com/kb/4020503) | 
|Windows Server 2008|.NET Framework 4.5.2 |[4020507](https://support.microsoft.com/kb/4020507) | 
|Windows Server 2008|.NET Framework 2.0 Service Pack 2 |[4020511](https://support.microsoft.com/kb/4020511) | 

Note that the [.NET Framework 4.7](https://blogs.msdn.microsoft.com/dotnet/2017/05/02/announcing-the-net-framework-4-7-general-availability/) contains the fix. If you are using Windows 10 Creators Update, you will still need to install the [May 2017 Update](https://blogs.msdn.microsoft.com/dotnet/) to get this fix.
