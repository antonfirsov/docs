.NET Language Updates
=====================

C# 6 and VB 14
--------------

With the RC release of Visual Studio 2015 RC, both Visual Basic 14 and C# 6 are language complete. In this release we've made several improvements to the languages and IDE to reduce boilerplate and respond to top customer feedback. Some highlights include:
 
- String interpolation: An intuitive String.Format-like syntax for composing strings from templates with inline expressions.
- The Null-Conditional operator (?.): A streamlined syntax for conditionally accessing a member or invoking a method on a value if it's non-null and returning null if the object is null instead of throwing a NullReferenceException.
- The NameOf operator: A rename-safe way to refer to the name of a code element such as in PropertyChanged events and ArgumentExceptions.
- Read-only Auto-Properties: A concise syntax for declaring properties which may only be assigned in their initializers or inside of a constructor.
 
Read about these features and many more on the [VB](http://blogs.msdn.com/b/vbteam/archive/2014/12/09/new-language-features-in-visual-basic-14.aspx) and [C#](http://blogs.msdn.com/b/csharpfaq/archive/2014/11/20/new-features-in-c-6.aspx) team blogs.
 
We've also made several improvements to the debugging experience including support for using lambda and query expressions inside of the debugger windows and Edit and Continue support in and around lambda expressions, simple queries, as well as inside Async and iterator methods.
 
Lastly, this release includes the 1.0 release of the .NET Compiler Platform, formerly known as "Roslyn". A rich set of code analysis APIs on which many of the new Visual Studio experience improvements like new refactorings and live code analyzers are built. Read more about the .NET Compiler Platform ("Roslyn") on [GitHub](http://github.com/dotnet/roslyn).
 
Visual F# 4.0
-------------

F# is a collaboration between the F# community and Microsoft. Visual F# 4.0 RC represents the work of 38 contributors, over 75% of whom have no Microsoft affiliation. It was built completely in the open by F# community developers, in partnership with the Visual F# team at Microsoft. The F# bits that ship with today’s RC build map to commit [76ae08d](https://github.com/Microsoft/visualfsharp/commit/76ae08d427a41e4140c) in the [F# repo on GitHub](https://github.com/Microsoft/visualfsharp/).

F# 4.0 includes major new enhancements across the language, the runtime and the IDE experience. The following features are a small selection of what's included:

- constructors as first-class functions
- simplified mutable/ref values
- a normalized collections API
- Leading ‘Microsoft’ namespace optional
- Async extensions to WebClient
- Implicit quotation of method arguments
- Script debugging
- Intellisense in object initializers

Check out the more indepth blog posts from the F# team on F# 4.0 release, for [F# 4.0 RC](http://blogs.msdn.com/b/dotnet/archive/2015/04/28/rounding-out-visual-f-4-0-in-vs-2015-rc.aspx) and [F# 4.0 Preview](http://blogs.msdn.com/b/fsharpteam/archive/2014/11/12/announcing-a-preview-of-f-4-0-and-the-visual-f-tools-in-vs-2015.aspx). 

