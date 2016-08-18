PowerShell is now open-source, and cross-platform
=================================================

The PowerShell team made a few [announcements](https://blogs.msdn.microsoft.com/powershell/2016/08/18/powershell-on-linux-and-open-source-2/) today: it's going open-source, and it's now available on Windows, Mac, and Linux. This is excellent news in itself, but the .NET team wanted to take the time to analyze what this means for .NET developers.

First, the availability of PowerShell on Linux and macOS, while it doesn't aim at replacing the native shell experiences of those operating systems, will make collaboration easier in teams with mixed environments. Being able to run the same scripts on each OS without having to spin up virtual machines will facilitate development and reduce friction across developers who have made different choices of environment.

When deploying applications into production, being able to run PowerShell scripts on Linux means more flexibility in the choice of target environments, and easier migrations to and from Linux and Windows servers.

Of course, the transition to an open-source model means not only community contributions, but also more transparent design processes and bug tracking. Implementing PowerShell on new platforms and environments now becomes possible for anyone who needs it.

Finally, it's worth pointing out that PowerShell and PowerShell scripts can now run on .NET Core.

Check it out!

* [The announcement](https://blogs.msdn.microsoft.com/powershell/2016/08/18/powershell-on-linux-and-open-source-2/)
* [Download the bits](https://github.com/PowerShell/PowerShell/releases)
* [The source code](https://github.com/PowerShell/PowerShell)
* [The PowerShell team blog](https://blogs.msdn.microsoft.com/powershell/)