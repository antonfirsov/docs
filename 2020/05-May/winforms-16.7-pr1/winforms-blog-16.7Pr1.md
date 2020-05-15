# Updates on .NET Core Windows Forms designer

Today we are happy to announce that Windows Forms designer for .NET Core projects is now available in the releease channel of Visua Studio! And we also have even newer version of the designer available in the [Visual Studion 16.7 Preview 1](https://docs.microsoft.com/en-us/visualstudio/releases/2019/release-notes-preview#16.7.0-pre.1.0)!

**ToDo PICTURE**

![.NET Core Windows Forms designer in Visual Studio](designer.png)

As many of you may know, last year we open-sourced Windows Froms and anabled it on .NET Core. And finaly we have the designer to provide the full experience for developing Windows Forms on .NET Core. Our team still continues working on the designer, so you will see more improvements coming soon, but even now you already can use it in both release and preview channels of Visual Studio.

## How to use the designer

* Get [Visual Studio 16.6](https://visualstudio.microsoft.com/downloads/) or [Visual Studio 16.7 Preview 1](https://visualstudio.microsoft.com/vs/preview/).
* Enable the designer in Visual Studio. Go to **Tools** > **Options** > **Environment** > **Preview Features** and select the **Use the preview Windows Forms designer for .NET Core apps** option.

![Enabling .NET Core Windows Forms designer in Visual Studio Settings](settings.png)

Now, once you double-click on your form in the Solution Explorer, the designer will open automatically the same way it is for .NET Framework applications.

Improving the performance is our next goal after we complete the functionality work, so don't get upset if it's not as fast as you envisioned while the designer is in the preview, that's something we will improve in the future.  

# What's available in the designer

* All Windows Forms controls except DataGrid and ToolStripContainer, and these are coming soon
* User Controls (available only in Visual Studio 16.7 Preview 1 version for now)
* All designer functionality, such as 
    * drag-and-drop
    * selection, move and resize
    * cut/copy/paste/delete
    * integration with Properties Window
    * events generation and so on
 * New WebView2 control **ToDo: is there a designer experience?**

# What's coming next

* Data binding scenarios

    This work is in progress and you already can see some results in the Visual Studio 16.7 Preview 1 designer.

* Control vendors support, such as Progress Telerik, DevExpress, GrapeCity, and others
    
    We are closely working with the control vendorson supporting their controls in .NET Core and in future in .NET 5. One of our demos for Microsoft Build 2020 showed of Progress Telerik controls in Windows Forms application targeting .NET Core 3.1 and .NET 5. More controls from varios vendors are coming soon.

* Inherited dialogs support

* Resourses and localization

## New in 16.6 GA release
* **ToDo: which controls we added in 16.6 GA sisnce 16.6 Preview 1?**
* Drag-and-drop improvements
* Selection improvements
* Stability and bug fixes

## New in 16.7 Preview 1 release
* User controls
* TableLayoutPanel
* Fundamentals for third-party controls support
* Fundamentals for data binding support


## Give us your feedback!

Your feedback is important to us! Please report issues and send feature requests via the Visual Studio Feedback channel. Use the "Send Feedback" icon in Visual Studio top-right corner as shown below and specify that it is related to the "WinForms .NET Core" area.

![Visual Studio Feedback channel](feedback.png)