# Introducing .NET Core Windows Forms Designer - Preview 2

We are happy to announce the new preview version of the .NET Core Windows Forms Designer - Preview 2! Starting from this version the designer is available with the ***preview*** version of [Visual Studio](https://visualstudio.microsoft.com/vs/preview/ "Visual Studio Preview download") 16.4 Preview 3 and later. To enable the designer in your Visual Studio Preview, go to **Tools -> Options -> Environment -> Preview Features** and check **Use the preview Windows Forms designer for .NET Core apps**. You might notice a gold bar in the upper part of your Visual Studio Preview suggesting you to enable the Windows Forms designer.

![Gold bar in Visual Studio suggesting to enable the Windows Forms designer](goldbar.png)

Clicking on **Enable** link will take you to the same place in  **Tools -> Options -> Environment -> Preview Features** where you can enable the designer. And yes, we make mistakes, the missing space in the gold bar will be fixed in the next Preview version :).

## New in Preview 2

* Timer control support.
* Added more features in the Component Tray. Now it responds to user actions.
* Added Designer Actions to the available controls, like for example making a `TextBox` multiline or adding items to a `CheckedListBox`.
* Improved Undo/Redo actions.
* Scrollbars support. If a form is bigger than the document window, scrollbars are shown.
* Some container controls (GroupBox and Panel) and more are coming.

![Windows Forms designer](designer.png)

## Klaus chapter - Some info about the intrinsic of the designer implementations

## Known issues
