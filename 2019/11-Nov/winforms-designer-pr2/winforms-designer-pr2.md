# Updates to .NET Core Windows Forms Designer in Visual Studio 16.5 Preview 1

We are happy to announce the new preview version of the .NET Core Windows Forms Designer that is available with the  Visual Studio 16.5 Preview 1! To use the designer you need to be on a ***preview*** version of [Visual Studio](https://visualstudio.microsoft.com/vs/preview/ "Visual Studio Preview download") and don't forget to enable the designer in Visual Studio **Tools -> Options -> Environment -> Preview Features** and check **Use the preview Windows Forms designer for .NET Core apps**. You might notice a gold bar in the upper part of your Visual Studio Preview suggesting you to enable the Windows Forms designer.

//ToDo: add new picture
![Gold bar in Visual Studio suggesting to enable the Windows Forms designer](goldbar.png)

Clicking on **Enable** link will take you to the same place in  **Tools -> Options -> Environment -> Preview Features** where you can enable the Designer.

## What's new

In this preview version of the designer we've added following features:

* Timer control support.
* Added Designer Actions to the available controls, like for example making a `TextBox` multiline or adding items to a `CheckedListBox`.
* Improved Undo/Redo actions.
* Scrollbars support. If a form is bigger than the document window, scrollbars are shown.
* Some container controls (`GroupBox` and `Panel`) and more are coming.
* Started Component Tray.
* Local resources support (?).
* Copy-paste between containers.
* .NET Core Windows Forms designer is now a part of Visual Studio and is released and available with each Visual Studio Preview version. 
* Various bug fixes and improvements.


![Windows Forms designer](designer.png)

## Under the hood of the new Windows Forms Core Designer (or why it takes us so much time)

We know that you’ve noticed: although the WinForms Core Designer Preview has basic functionalities, it is not mature enought for providing the full Windows Forms Designer experience and we need a little more time to get there. There are good reasons for this: Visual Studio is based on the classic .NET Framework. The Core Designer however should enable users to create the visual design for WinForms _Core_ apps. The WinForms team faces the same challenges here as all developer teams who "mix" the classic with the Core framework: Core assemblies cannot be integrated into the classic .NET Framework by definition. But this is just one of the reasons why the internal concept of the new Core Designer is: Whenever the Core Designer is started by double-clicking on a form, a second designer process starts under the hood almost independently of Visual Studio. And that process takes over the Core design part, or better said: it is responsible for instantiating the Core framework based objects that are then rendered on the monitor by the Core and not the Visual Studio process. So, if you are a developer and you put a Button on the design surface, then you "draw" the rubber band based on the classic Visual Studio framework all right. But as soon as you then release the mouse button, the classic designer part calls over to the Core designer part in the Core process: "Hey, please instantiate a (Core) Button and render it at the given position." As a result, the necessary code to create the user interface at runtime, which lives in the well known `InitializeComponent` method of a `Form` or a `UserControl`, is also created by the Core and not by the Visual Studio process.
Of course, the WinForms team could have made their work easy and simply "pretended" what is needed to be designed to be classic framework objects for the design phase. But that would only work up to a certain point: Already today there are features and functionalities in WinForms Core that do not exist in the classic framework or are different. Such examples are the `PlaceholderText` property of the `TextBox` control, which only exists in WinForms Core. Or the fact that a modern (and therefore different) default font is used in WinForms Core, or that the designed elements in Core are rendered in the SystemAware HighDPI mode. Any designer that would simply map the Core functionality to framework objects would not be able to also map the new objects or object's properties and features of WinForms Core, of which there will be much more in the future! 
In the new Visual Studio WinForms Core Designer, however, this will work: The Property Browser you see and use in Visual Studio is based on the classic framework, but thanks to the existence of TypeDescriptors, which allow an enormous flexibility of the extension of framework types at runtime, we are able to create ProxyObjects as a communication link between the two processes at design time to access the actual Core objects in the other (core) process via inter-process communication. That way, although the UI is still in Visual Studio and thus based on the classic framework, the users will still see and edit every single aspect of the WinForms Core objects’ functionality. 
The downside is that this detour has to be implemented anew for practically the entire existing Windows Forms Designer, and to do this correctly and with the performance and stability that you expect, we have set ourselves the corresponding schedule.

## Known issues - ToDo Merrie


## Breaking Changes - ToDo Olia
