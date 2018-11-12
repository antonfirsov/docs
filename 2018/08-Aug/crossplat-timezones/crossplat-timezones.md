# Cross-platform Time Zones with .NET Core #
*By Chris Roberts*

Developing applications that span multiple operating systems in .NET Core while working with Time Zone information can lead to unexpected results for developers not familiar with the differences in how operating systems manage Time Zones. In this post, we will explore those differences and the challenges they present.

## Reproducing the issue

Suppose you are writing a console application in .NET Core and you want to get information about a specific Time Zone.  You might write something like this

```csharp
static void Main(string[] args)
{
    TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
    Console.WriteLine(tzi.DisplayName);
}
```

Running this on my Windows 10 development environment, I see the following output

```
(UTC-06:00) Central Time (US & Canada)
```

If I take that same block of code over to my Ubuntu 18.04 development environment and run it, I instead see the following exception being thrown

```
Exception has occurred: CLR/System.TimeZoneNotFoundException
An unhandled exception of type 'System.TimeZoneNotFoundException' occurred in System.Private.CoreLib.dll: 'The time zone ID 'Central Standard Time' was not found on the local computer.'
```

What's going on here? Let's spend a little time digging into that and see what exactly is happening.

## Time Zone differences

Windows maintains its list of Time Zones in the Windows registry.  You can find a list of those values [here](https://docs.microsoft.com/en-us/windows-hardware/manufacture/desktop/default-time-zones).  

In contrast, Linux distributions use the Time Zone database curated by the Internet Assigned Numbers Authority (IANA). You can find the latest copy of that database on [IANA's website](https://www.iana.org/time-zones). Here's an example of what an IANA Time Zone looks like

```
America/New_York
```

The issue comes into play when you write your .NET Core code specifically using one of the two formats and then try to run the application on another operating system.  Because the runtime is deferring the Time Zone management to the underlying operating system you will need to handle the differences if that scenario applies to you.  

## How can we work around this?

There is an open source project available on GitHub that addresses these differences. Head over and check out the [source code](https://github.com/mj1856/TimeZoneConverter) contributed by the project [developer and maintainer](https://github.com/mj1856). You can grab the package via NuGet with the following command:

```
Install-Package TimeZoneConverter
```

Once you have it installed, you are able to work with different operating system Time Zone providers in a uniform way.

```csharp
TimeZoneInfo tzi = TZConvert.GetTimeZoneInfo("Central Standard Time");
TimeZoneInfo tzi = TZConvert.GetTimeZoneInfo("America/New_York");
```

Time zone data changes every so often, so as noted in the project documentation - be sure to keep this package up to date.


