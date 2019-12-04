# Updates to .NET Core Windows Forms Designer in Visual Studio 16.5 Preview 1

We are happy to announce the new preview version of the .NET Core Windows Forms Designer, which is available with the Visual Studio 16.5 Preview 1! To use the designer:

- You must be using a ***preview*** version of [Visual Studio](https://visualstudio.microsoft.com/vs/preview/ "Visual Studio Preview download").
- You need to enable the designer in Visual Studio. Go to **Tools** > **Options** > **Environment** > **Preview Features** and select the **Use the preview Windows Forms designer for .NET Core apps** option.

![Enabling .NET Core Windows Forms Designer in Visual Studio Settings](settings.png)

Just to clarify, since we get this question sometimes: ***installing the .NET Core Windows Forms Designer from separate VSIX is no longer needed***. It is now built into the Visual Studio Preview.


You might notice a yellow bar in the upper part of your Visual Studio Preview suggesting you to enable the Windows Forms designer.

![Gold bar in Visual Studio suggesting to enable the Windows Forms designer](goldbar.png)

Clicking on **Enable** link will take you to the same place in  **Tools -> Options -> Environment -> Preview Features** where you can enable the designer.

## What's new

In this preview version of the designer we've added following features:

* Timer control support
* Added Designer Actions to the available controls, like for example making a `TextBox` multiline or adding items to a `CheckedListBox`.
* Improved Undo/Redo actions
* Scrollbars support: if a form is bigger than the document window, scrollbars are shown
* Some container controls (`GroupBox` and `Panel`) and more are coming
* Started Component Tray
* Local resources support
* Copy-paste between containers
* Various bug fixes and improvements
* Shipping as a part of Visual Studio: you no longer need to install .NET Core Windows Forms designer separately, now it is a part of Visual Studio and is released and available with each Visual Studio Preview version

![Windows Forms designer](designer.png)

## Features currently under development

These features are not yet implemented:

* Localization of your own Windows Forms applications
* Data-related scenarios including data binding and data-related controls
* Tab order dialog
* Document outline
* Container controls: we have implementations for Panel Control and GroupBox Container and the rest are coming soon.
* MenuStrip and ToolStrip are expected in the next preview.
* Inherited forms
* Tools -> Options page for the designer
* Third-party controls and UserControls

## Known issues - ToDO Merrie



## Upgrade to .NET Core 3.1 LTS version!

In .NET Core 3.1 some outdated Windows Forms controls (DataGrid, ToolBar, ContextMenu, Menu, MainMenu, MenuItem and their child components) were removed. These controls were replaced with newer and more powerful ones in .NET Framework 2.0 in 2005 and have not been available by default in the designer Toolbox. Moving forward with .NET Core we finally had to cut them out from the runtime as well in order to be able to maintain support for areas like high DPI, accessibility, and reliability.

We recommend to upgrade to the .NET Core 3.1 version because this is the long term support version that has many improvement and bug fixes. In future versions of the Windows Forms Designer you might see some issues with .NET Core 3.0 applications because of the removed functionality related to the outdated controls. Except for this deleted controls the upgrade from .NET Core 3.0 to 3.1 should be very smooth. And even if you are using any of the old controls, it will be easy to upgrade your application to their new alternatives:

| Old Control  	| Recommended Replacement                  	| Other associated APIs removed    	|
|-------------------	|------------------------------------------	|----------------------------------	|
| DataGrid          	| DataGridView                             	| DataGridCell, DataGridRow, DataGridTableCollection, DataGridColumnCollection, DataGridTableStyle, DataGridColumnStyle, DataGridLineStyle, DataGridParentRowsLabel, DataGridParentRowsLabelStyle, DataGridBoolColumn, DataGridTextBox, GridColumnStylesCollection, GridTableStylesCollection, HitTestType        	|
| ToolBar           	| ToolStrip                                	| ToolBarAppearance             	|
| ToolBarButton        	| ToolStripButton                          	| ToolBarButtonClickEventArgs, ToolBarButtonClickEventHandler, ToolBarButtonStyle, ToolBarTextAlign             	|
| ContextMenu       	| ContextMenuStrip                         	|                                  	|
| Menu              	| ToolStripDropDown, ToolstripDropDownMenu 	| MenuItemCollection              	|
| MainMenu          	| MenuStrip                                	|                                  	|
| MenuItem          	| ToolstripMenuItem                        	|                                  	|

## Under the hood of the new Windows Forms Core Designer (or why it takes us so much time)

We know that you’ve noticed: although the Windows Forms .NET Core Designer Preview has basic functionalities, it is not mature enough for providing the full Windows Forms experience and we need a little more time to get there. In his chapter we wanted to give you a glance on how we are implementing the designer for .NET Core and explain some of the time frames.

Visual Studio is based on .NET Framework. The Windows Forms Core Designer however should enable users to create the visual design for *.NET Core* apps. If you've tried to "mix" .NET Framework and .NET Core projects, you probably know what the challenge is here: .NET Core assemblies cannot be integrated into .NET Framework projects. Because of this (and some other reasons that we don't want to overwhelm you with), we came up with the following  internal concept: whenever the .NET Core Designer is started (for example, by double-clicking on a form), a second designer process starts under the hood almost independently of Visual Studio. And that process takes over the .NET Core design part, or better to say - it is responsible for instantiating the .NET Core based objects that are then rendered on the monitor by the .NET Core process and not the Visual Studio process.

For example, when you drag a Button from the Toolbox onto a form - this action is handled by Visual Studio (`devenv.exe` process which is .NET Framework). But, once you release the mouse button to droop the Button on the form, all further actions (instantiating a Button, rendering it at a specific location, etc.,) are related to .NET Core. That means .NET Framework process can no longer handle it. Instead it calls to a .NET Core process which does the job and also creates the user interface code at runtime, which lives in the `InitializeComponent` method of a `Form` or a `UserControl`.

***Was there a better way?*** There was another approach we could take that would save us a lot of time. We could simply "map" the .NET Core objects, features, etc. to .NET Framework ones. But this approach has significant limitations. The new features, that are available only in .NET Core won't be available in this "mapped" designer. And we already have quite a few Core-only functionalities such as: the new `PlaceholderText` property of the `TextBox` control, the new default font, that is used in Windows Forms .NET Core. Ang going forward we expect more innovations that are critical to be available from the designer.

That's why we turned down this idea and proceeded with the described above "out-of-process approach", that handles new additions to .NET Core very well. This is how t works. The Property Browser in Visual Studio is based on the .NET Framework, but thanks to `TypeDescriptors`, which allow an enormous flexibility of the extension of .NET Framework types at runtime, we are able to create "proxy objects" as a communication link between the two processes at design time to access the actual .NET Core objects in the other (.NET Core) process via inter-process communication. That way, although the UI is still in Visual Studio and thus is .NET Framework, the users will still see and edit every single aspect of the Windows Forms .NET Core objects’ functionality.

The downside for this approach is that this implementation requires to re-write in a new manner basically the entire existing Windows Forms Designer. And to do this correctly, with the performance and stability that you expect, we had to set out some significant time. The WPF team started working on the designer prototype that would work well outside of the Visual Studio process more than 3 years ago and now the WPF designer is released and ready for .NET Core developers. Of course we cannot leave Windows Forms developers for another 3 years without the designer. The engineering team is working very hard to get the mature version of the .NET Core Windows Forms designer in May 2020 and we are looking at completing the work on it by the fourth quarter of 2020.

From all the team we want to say **THANK YOU!** to those who is already testing the Preview versions of the designer, reporting the issues and contributing to the [Windows Forms](https://github.com/dotnet/winforms)! We know the experience is "buggy" and appreciate your desire to help us! :)

## How to report issues

Your feedback and help is very welcome! You can report an issue or a feature request via Visual Studio Feedback channel. To do so, click on Send Feedback icon in Visual Studio top right corner as it is shown on the picture below and specify that it is related to the "WinForms .NET Core" area.

![Visual Studio Feedback channel](feedback.png)
