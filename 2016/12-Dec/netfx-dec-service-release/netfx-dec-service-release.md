# .NET Framework December Monthly Rollup

Today we are releasing a new Security and Quality Rollup and Security Only update for the .NET Framework. This release resolves a security vulnerability and includes three new quality and reliability improvements.

You can read more about the recent changes to how the .NET Framework receives updates on the [.NET Framework Monthly Rollups Explained](https://blogs.msdn.microsoft.com/dotnet/2016/10/11/net-framework-monthly-rollups-explained/) post.

## Security
This release resolves a vulnerability in Microsoft .NET 4.6.2 Framework’s Data Provider for SQL Server. A security vulnerability exists in Microsoft .NET Framework 4.6.2 that could allow an attacker to access information that is defended by the [Always Encrypted](https://technet.microsoft.com/library/security/dn848375.aspx#AlwaysEncrypted) feature.  The security update addresses the vulnerability by correcting the way .NET Framework handles the developer-supplied key, and thus properly defends the data. This security update is rated Important for Microsoft .NET Framework 4.6.2. To learn more about the vulnerability, see [Microsoft Security Bulletin MS16-155](https://technet.microsoft.com/library/security/MS16-155).

## Quality and Reliability 

### Common Language Runtime

When an application uses unaligned block initialization, for example, from managed C++, the code generated on AVX2 hardware has an error. As a result, if the JIT uses a register other than xmm0 for the source, an incorrect encoding will be used. This improvement applies .NET Framework 4.6 and 4.6.1.

### Windows Presentation Foundation

A memory leak may occur for certain scenarios when an application includes a D3DImage control. For example, if you started an application, changed both the size and content of the image and then ran the application through Remote Desktop. This improvement applies .NET Framework 4.5.2, 4.6 and 4.6.1.

## More Information
Additional information on what is included in each of the rollups along with the applicable operating systems can be found on their associated knowledge base articles, listed below. 

### Security and Quality Rollup

|KB Article | .NET Version  | Operating System   |
|---|---|---|
| [3210142](https://support.microsoft.com/en-us/kb/3210142)  |.NET Frameworks 3.5, 4.5.2, and 4.6   |  Windows Vista SP2 and Windows Server 2008 SP2 |
| [3205402](https://support.microsoft.com/en-us/kb/3205402)  |.NET Frameworks 3.5, 4.5.2, 4.6, 4.6.1, and 4.6.2   |   Windows 7 and Windows Server 2008 R2 |
| [3205403](https://support.microsoft.com/en-us/kb/3205403)  |.NET Frameworks 3.5, 4.5.2, 4.6, 4.6.1, and 4.6.2   |  Windows Server 2012 |
| [3205404](https://support.microsoft.com/en-us/kb/3205404)  |.NET Frameworks 3.5, 4.5.2, 4.6, 4.6.1, and 4.6.2   |  Windows 8.1 and Windows Server 2012 R2 |

### Security Only Update

|KB Article | .NET Version  | Operating System   |
|---|---|---|
| [3205406](https://support.microsoft.com/en-us/kb/3205406)  |.NET Framework 4.6.2   |  Windows 7 and Windows Server 2008 R2 |
| [3205407](https://support.microsoft.com/en-us/kb/3205407)  |.NET Framework 4.6.2   |  Windows Server 2012 |
| [3205410](https://support.microsoft.com/en-us/kb/3205410)  |.NET Framework 4.6.2   |  Windows 8.1 and Windows Server 2012 R2 |
