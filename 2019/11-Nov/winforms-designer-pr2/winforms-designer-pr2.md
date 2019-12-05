# Updates to .NET Core Windows Forms designer in Visual Studio 16.5 Preview 1

We are happy to announce the new preview version of the .NET Core Windows Forms designer, which is available with the Visual Studio 16.5 Preview 1. 

The big news is that the designer is now part of Visual Studio!
This means that  ***installing the .NET Core Windows Forms designer from separate VSIX is no longer needed!*** 

To use the designer:

- You must be using a ***preview*** version of [Visual Studio](https://visualstudio.microsoft.com/vs/preview/ "Visual Studio Preview download") of at least version VS 16.5 Preview 1.
- You need to enable the designer in Visual Studio. Go to **Tools** > **Options** > **Environment** > **Preview Features** and select the **Use the preview Windows Forms designer for .NET Core apps** option.

![Enabling .NET Core Windows Forms designer in Visual Studio Settings](settings.png)

If you haven't enabled the Windows Forms designer, you might notice a yellow bar in the upper part of your Visual Studio Preview suggesting you to enable it:

![Gold bar in Visual Studio suggesting to enable the Windows Forms designer](goldbar.png)

Selecting the **Enable** link will take you to the same place in  **Tools -> Options -> Environment -> Preview Features** where you can enable the Windows Forms .NET Core designer preview.

## What's new

In this preview version of the designer, we have improved reliability and enhanced performance, fixed various bugs and added the following features:

* designer Actions for available controls, such as making a `TextBox` multiline or adding items to a `CheckedListBox`.
* Improved Undo/Redo actions to prevent hangs or incomplete undos.
* Scrollbars now appear when the form is larger than the visible document window and on Forms with the `AutoScroll` property set to `True` and controls outside the visible area of the form.
* Added `GroupBox` and `Panel` container control support.
* Copy-paste is supported between container controls.
* Limited Component Tray support.
* Local resources support.
* `Timer` control support.


![Windows Forms designer](designer.png)

## Features currently under development

Support for the following features is currently on our backlog and being actively addressed:

* Localization of your own Windows Forms applications.
* Data-related scenarios including data binding and data-related controls.
* Document Outline Window.
* The remaining container controls.
* `MenuStrip` and `ToolStrip` are expected in the next preview.
* Third-party controls and UserControls.
* Inherited Forms and UserControls.
* Tab Order.
* Tools -> Options page for the designer.
  
## Upgrade to .NET Core 3.1

We recommend that you upgrade your applications to .NET Core 3.1 before using the Windows Forms .NET Core designer. Visual Studio may not perform as expected if your project is targeting an earlier verion of .NET Core. 

In .NET Core 3.1 a few outdated Windows Forms controls (`DataGrid`, `ToolBar`, `ContextMenu`, `Menu`, `MainMenu`, `MenuItem`, and their child components) were removed. These controls were replaced with newer and more powerful ones in .NET Framework 2.0 in 2005 and haven't been available by default in the designer Toolbox. Moving forward with .NET Core, we had to cut them out of the runtime as well in order to maintain support for areas like high DPI, accessibility, and reliability. For more information please see the [announcement blog post](https://devblogs.microsoft.com/dotnet/announcing-net-core-3-1/). 


## Under the hood of the new Windows Forms Core designer (or why it takes us so much time)

We know that you’ve noticed: although the Windows Forms .NET Core designer Preview has basic functionalities, it is not mature enough for providing the full Windows Forms experience and we need a little more time to get there. In this chapter we wanted to give you a glimpse into how we are implementing the designer for .NET Core and explain some of the time frames.

### The concept

Visual Studio is based on .NET Framework. The Windows Forms Core designer however should enable users to create a visual design for *.NET Core* apps. If you've tried to "mix" .NET Framework and .NET Core projects, you probably know what the challenge is here: .NET Core assemblies cannot be integrated into .NET Framework projects. Because of this (and some other reasons that we don't want to overwhelm you with), we came up with the following internal concept: whenever the .NET Core designer is started (for example, by double-clicking on a form), a second designer process starts under the hood almost independently of Visual Studio. And that process takes over the .NET Core design part, or better to say - it is responsible for instantiating the .NET Core based objects that are then rendered on the monitor by the .NET Core process and not the Visual Studio process.

For example, when you drag a `Button` from the Toolbox onto a form - this action is handled by Visual Studio (`devenv.exe` process which is .NET Framework). But, once you release the mouse button to drop the `Button` on the form, all further actions (instantiating a `Button`, rendering it at a specific location, and so on) are related to .NET Core. That means .NET Framework process can no longer handle it. Instead it calls to a .NET Core process which does the job and also creates the user interface code at runtime, which lives in the `InitializeComponent` method of a `Form` or a `UserControl`. This is the same way the XAML designer works for UWP and .NET Core.

### Was there a better way?

There was another approach we could take that would save us a lot of time. We could simply "map" the .NET Core objects, features, and so on to .NET Framework ones. But this approach has significant limitations. The new features, that are available only in .NET Core won't be available in this "mapped" designer. And we already have quite a few Core-only functionalities such as: the new `PlaceholderText` property of the `TextBox` control, the new default font, that is used in Windows Forms .NET Core. Ang going forward we expect more innovations coming.

That's why we turned down that idea, and proceeded with the described above "out-of-process approach" that handles new additions to .NET Core very well.

### This is how it works

The Property Browser in Visual Studio is based on the .NET Framework, but thanks to `TypeDescriptors` we are able to create "proxy objects" as a communication link between the two processes at design time to access the actual .NET Core objects in the other (.NET Core) process via inter-process communication. That way, although the UI is still in Visual Studio and thus is .NET Framework, the users will still see and edit every single aspect of the Windows Forms .NET Core objects’ functionality.

The downside of this approach is that it requires us to re-write the significant portions of the Framework Windows Forms designer. To do this correctly, with the performance and stability that you expect, we had to set out a significant amount of time. The XAML designer team already developed an out-of-process model for UWP XAML designer support when UWP implemented .NET Standard 2.0. They were able to share much of that architecture with WPF running against .NET Core. This gave a head-start for .NET Core WPF designer, and now it is released and ready for .NET Core developers. The Windows Forms team started working on the designer with the .NET Core 3.0 announcement. We expect to get to feature parity by May 2020, and complete the work by the fourth quarter of 2020.

The Windows Forms team wants to say **THANK YOU!** to those who are already testing preview versions of the designer and reporting issues! We know the experience may not be 100% stable and appreciate your patience and a desire to help us! :)

## How to report issues

Your feedback and help are important to us! Please report issues or feature requests via the Visual Studio Feedback channel. To do so, select the **Send Feedback** icon in Visual Studio top-right corner as shown in the following picture and specify that it is related to the "WinForms .NET Core" area.

![Visual Studio Feedback channel](feedback.png)