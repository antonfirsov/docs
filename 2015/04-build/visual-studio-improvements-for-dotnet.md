Visual Studio Improvements for .NET
===================================

The Visual Studio Team has added some key improvements for .NET in the RC release. There were many additional [Visual Studio improvements for .NET in the Preview release](http://blogs.msdn.com/b/dotnet/archive/2014/11/12/announcing-net-2015-preview-a-new-era-for-net.aspx#_Visual_Studio_Improvements) that you can also try out. 

Moar EnC - Lambda and Async Task support
========================================

Edit and Continue (EnC) is a great productivity feature that everyone (wants to) use every day. It enables you to edit your code while you are debugging it. This is useful for a lot of reasons, particularly if your code needs to interact with state that isn't directly part of an API (e.g. processing JSON files) and can be most easily discovered at runtime.

You can now use EnC with lambdas, async methods, LINQ and a few other situations. Given today's coding patterns, that's a huge jump forward for EnC usability. The following screenshot demonstrates the new support. There are two separate lines that were typed using this new support, one in an async method and the other in a lamda.

![Visual Studio 2015 - EnC Support](vs2015-enc.png)

The following scenarios are now supported:

- Async functions
- Lambda expressions
- LINQ queries
- Functions with yield return expressions

In some cases, the combination of these features are not yet supported with EnC (e.g. async lamdas).