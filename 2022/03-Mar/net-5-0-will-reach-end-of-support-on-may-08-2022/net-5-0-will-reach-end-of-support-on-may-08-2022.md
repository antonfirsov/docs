---
post_title: .NET 5.0 will reach End of Support on May 08, 2022
username: rbhanda@microsoft.com
microsoft_alias: rbhanda
featured_image: dotnet-bot_handybot.png
categories: .NET
summary: .NET 5.0 will reach end of support on May 08, 2022, this blog breaks down all the valuable information you need to know and how to update to .NET 6.0.
desired_publication_date: 2022-03-24
---

.NET 5.0 will reach end of support on May 08, 2022. After the .NET May updates, Microsoft will no longer provide servicing updates, including security fixes or technical support, for .NET 5.0. You'll need to update the version of .NET you’re using to a supported version (.NET 6.0) before this date in order to continue to receive updates.

## Support Policy 

.NET 5.0 is not an LTS release and is therefore supported for 18 months, or 6 months after the next release ships, whichever is longer. .NET 5.0 support will end on May 08, 2022.

When .NET 5.0 reaches end of support, applications that use this version will continue to run. That said, we won't issue security updates for .NET 5.0 starting in May 2022 when we issue security updates for supported versions of .NET, which will be .NET Core 3.1 and .NET 6.0. This means that if a computer has .NET 5.0 installed, it may be potentially unsecure. Additionally, if you run into any issues and need technical support, we may not be able to help you.


### Update your application
If you're an end user, we recommend reaching out to the vendor managing your software to confirm whether an updated version of the software is needed and available. The remainder of this post is applicable to software vendors and developers.

If your application uses NET 5.0, we strongly recommend you migrate your application to .NET 6.0 - a supported LTS version. You can download .NET 6.0 from the [.NET website](https://dotnet.microsoft.com/download/dotnet/6.0).


#### Upgrading to .NET 6.0

* Open the project file (the *.csproj, *.vbproj, or *.fsproj file).
* Change the target framework value from net5.0 to net6.0. The target framework is defined by the <TargetFramework> or <TargetFrameworks> element.
* For example, change <TargetFramework>net5.0</TargetFramework> to <TargetFramework>net6.0</TargetFramework>.
You may also want to review the [.NET 6 Compatibility Guide](https://docs.microsoft.com/dotnet/core/compatibility/6.0).
#### Update your development environment

In addition to the software you ship to your customers, the computer you use for development may have .NET 5.0 installed - either standalone or installed by Visual Studio.

You can check for stand-alone installations of .NET 5.0 from the command line. On a Windows computer, open a Command Prompt and go to %ProgramFiles%dotnet folder. On macOS or Linux, open a terminal window.

Then type the following command: dotnet --list-runtimes
  
<img width="548" alt="image" src="https://user-images.githubusercontent.com/30737530/159568592-466dbe73-b151-462d-a067-5e4e330ae38c.png">

If you use Visual Studio 2019 16.11 or 16.9 or 16.7, then based on the workloads installed, you may also have .NET 5.0 installed as a required component of Visual Studio and you need to be aware of some relevant changes that are coming.
  
<img width="667" alt="image" src="https://user-images.githubusercontent.com/30737530/159569685-4bdc0323-5933-48de-b41d-4d2de2c20840.png">

  <img width="535" alt="image" src="https://user-images.githubusercontent.com/30737530/159569752-ac64f61a-2c3f-4cc1-957b-c0862312fad8.png">

Starting with the June 2022 servicing update for Visual Studio 2019 16.11 and Visual Studio 2019 16.9, the .NET 5.0 component in Visual Studio will be changed to out of support and optional. This means that workloads in Visual Studio may be installed without installing .NET 5.0. Note that existing installations won’t be affected and any previously installed workload and component will remain installed until the component or workload is unselected in Visual Studio setup. While it's possible for you to re-select this optional component in Visual Studio and re-install this, we strongly recommend you use .NET 6.0 with Visual Studio 2022 to build apps that run on a supported .NET runtime.

Note: If you're migrating an app to .NET 6.0, some breaking changes might affect you. We recommend you to go through the [compatibility check](https://docs.microsoft.com/dotnet/core/compatibility/6.0).
  
Note: The .NET 5.0 SDK versions will continue to be supported in VS 16.11 until December of 2022 when .NET Core 3.1 goes out of support so that .NET Core 3.1 customers can continue to use 16.11 to developer their applications. This .NET 5.0 SDK will not use the .NET 5.0 runtime when running command line scenarios and will not be shipped as a stand-alone SDK.
## Useful Links
* [.NET downloads](https://dotnet.microsoft.com/download/dotnet)
* [.NET Compatibility](https://docs.microsoft.com/dotnet/core/compatibility/)
* [.NET Deployment](https://docs.microsoft.com/dotnet/core/deploying/)
* [.NET Support Policy](https://dotnet.microsoft.com/platform/support/policy/dotnet-core)

## Closing

.NET 5.0 will be reaching end of support on May 08 and after the .NET May 2022 updates we will no longer provide updates including security fixes, or technical support for this version. We strongly recommend you migrate your applications to .NET 6.0.
