Visual Studio Improvements for .NET
===================================

The Visual Studio Team has added some key improvements for .NET in the RC release. There were many additional [Visual Studio improvements for .NET in the Preview release](http://blogs.msdn.com/b/dotnet/archive/2014/11/12/announcing-net-2015-preview-a-new-era-for-net.aspx#_Visual_Studio_Improvements) that you can also try out. 

Xamarin Starter now Included in Visual Studio
---------------------------------------------

Visual Studio 2015 includes [Xamarin Starter Edition](http://xamarin.com/starter) as an optional feature. Xamarin is a great way to start building iOS and Android apps in C# or F# within Visual Studio. Many .NET developers are using Xamarin to increase the reach of their apps and development effort to iOS and Android. According to a recent blog post, Xamarin has been downloaded by [1 Million unique developers](http://blog.xamarin.com/xamarin-passes-1-million-developer-milestone/). That's a lot.

There are several additional application templates that are available for you to use after installing Xamarin Starter edition, for iOS (displayd below) and Android.

![VS Xamarin Experience](vs-xamarin-projects.png)

You can use Xamarin Starter editon as long as you want, build apps, test on devices and publish to app stores. You can start a [Xamarin Business trial](http://developer.xamarin.com/guides/cross-platform/getting_started/beginning_a_xamarin_trial/#Activating_a_Trial_in_Visual_Studio) to try out the richer experience. You can always return to Xamarin Starter Edition after that.

Debugger Improvements 
=====================

Visual Studio 2015 addresses many requests that you have made for improving your debugging life, such as lambda debugging, Edit and Continue (EnC) improvements, child-process debugging, as well revamp core experiences such as powerful breakpoint configuration and introduce a new Exceptions Settings toolwindow. We also pushed the state of the art by integrating performance tooling into the debugger with PerfTips and the all new Diagnostic Tools window which includes the redesigned IntelliTrace for historical debugging and the Memory Usage tool.

Moar EnC - Lambda and Async Task support
========================================

Edit and Continue (EnC) is a great productivity feature that everyone (wants to) use every day. It enables you to edit your code while you are debugging it. This is useful for a lot of reasons, particularly if your code needs to interact with state that isn't directly part of an API (e.g. processing JSON files) and can be most easily discovered at runtime.

You can now use EnC with lambdas, async methods, LINQ and a few other situations. Given today's coding patterns, that's a huge jump forward for EnC usability. You can check out the set of [EnC improvements the team already released](http://blogs.msdn.com/b/csharpfaq/archive/2015/02/23/edit-and-continue-and-make-object-id-improvements-in-ctp-6.aspx) in Visual Studio CTP 6.

The following screenshot demonstrates the new support. There are two separate lines that were typed using this new support, one in an async method and the other in a lamda.

![Visual Studio 2015 - EnC Support](vs2015-enc.png)

The following scenarios are now supported:

- Async functions
- Lambda expressions
- LINQ queries
- Iterator functions

EnC improvements are still in progress and we are working to support more scenarios (and providing more documentation). Please [file any issues](https://github.com/dotnet/roslyn/issues) you come across on our GitHub.

> **Note:** If you don't understand why an edit fails, try checking the Error List. There are explanations for errors there that should help clarify issues. Please file an issue if you find these messages confusing or if they do not exist for your error.

Read more about [earlier EnC improvements](http://blogs.msdn.com/b/csharpfaq/archive/2015/02/23/edit-and-continue-and-make-object-id-improvements-in-ctp-6.aspx) from Visual Studio 2015 CTP 6, such as modifying iterators, async/await, methods, etc.

WPF - Live Visual Tree 
======================

Visual Studio includes a new viewer and editor for the XAML Visual Tree - Live! - while debugging a WPF app. It enables you to navigate the visual tree as it exists at any point in the life cycle of your app. You can edit properties on the tree, for example button text, which are then displayed in the running app. You cannot change the composition of the tree.

You can also select visual components in the running app. These selections will update the Live Visual Tree in Visual Studio, enabling you to focus in on the parts of your app that might need investigation or updates.

The Live Visual Tree is also connected to the XAML source editing experience. As you select XAML nodes in the Live Visual Tree, the selected textual XAML in the IDE changes to match. You always know which text matches, making it easy to find the line of XAML to look at or change.

The following screenshot demonstrates the Live Visual Tree and an app that has a button selected with the new feature. 

![Visual Studio 2015 - Live Visual Tree](vs2015-live-visual-tree.png)

TODO: Get a better screenshot.
