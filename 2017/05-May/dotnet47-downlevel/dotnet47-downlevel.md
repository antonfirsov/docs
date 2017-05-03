# Announcing the .NET Framework 4.7 General Availability

Today, we are announcing the general availability of the .NET Framework 4.7. The .NET Framework 4.7 was released as part of Windows 10 Creators Update a month ago. You can now install the .NET Framework 4.7 on other versions of Windows.

You can download the .NET Framework 4.7:

- [Web installer](http://go.microsoft.com/fwlink/?LinkId=825299)
- [Offline installer](http://go.microsoft.com/fwlink/?LinkId=825303)

The [.NET Framework 4.7](https://docs.microsoft.com/dotnet/articles/framework/whats-new/index#v47) includes improvements in several areas:

- High DPI support for Windows Forms applications on Windows 10
- Touch support for WPF applications on Windows 10
- Enhanced cryptography support
- Support for C# 7 and VB 15, including ValueTuple
- Support for .NET Standard 1.6
- Performance and reliability improvements

Please see the [Announcing the .NET Framework 4.7](https://blogs.msdn.microsoft.com/dotnet/2017/04/05/announcing-the-net-framework-4-7/) blog post to learn more about each of these improvements.

You can see the complete list of improvements in the [.NET Framework 4.7 release notes](https://github.com/Microsoft/dotnet/tree/master/releases/net47/README.md).

## Supported Windows Versions

The .NET Framework 4.7 is supported on the following Windows versions:

- Windows 10 Creators Update (included in-box)
- Windows 10 Anniversary Update
- Windows 8.1
- Windows 7 SP1

The .NET Framework 4.7 is supported on the following Windows Server versions:

- Windows Server 2016
- Windows Server 2012 R2
- Windows Server 2012
- Windows Server 2008 R2 SP1

## DirectX Dependency

The .NET Framework 4.7 now uses DirectX 11 components for WPF. These components are available on more recent versions of Windows.

You must install an additional DirectX component in order to install the .NET Framework 4.7 on Windows 7 SP1, Windows 2008 R2 SP1, Windows 2012 and Windows 2012 R2. The installation includes a single dll that will get added to your system. It will only be used by WPF applications. It is not possible to install the .NET Framework 4.7 without installing this component.

You can install this component for your operating system:

- [Windows 7 SP1 x86](http://go.microsoft.com/fwlink/?LinkId=848159)
- [Windows 7 SP1 or Windows Server 2008 R2 x64](http://go.microsoft.com/fwlink/?LinkId=848158)
- [Windows Server 2012 x64](http://go.microsoft.com/fwlink/?LinkId=848160)

Please see the following for more information: [The .NET Framework 4.7 installation is blocked on Windows 7, Windows Server 2008 R2 and Windows Server 2012 because of a missing d3dcompiler update](https://support.microsoft.com/en-us/help/4020302)

## Closing

Thanks for trying out the .NET Framework 4.7. Please tell us what you think about the release and how it is working for you in your environment. Please share your feedback in the comments below or on [GitHub](https://github.com/Microsoft/dotnet/issues/393).

For more information on the release, please see [Announcing the .NET Framework 4.7](https://blogs.msdn.microsoft.com/dotnet/2017/04/05/announcing-the-net-framework-4-7/) and the [.NET Framework 4.7 release notes](https://github.com/Microsoft/dotnet/tree/master/releases/net47/README.md).