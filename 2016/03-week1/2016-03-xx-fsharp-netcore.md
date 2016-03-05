Getting Started with F# on .NET Core
===========================

With the help of the F# community, we've added support for .NET Core to F#. Now, we're looking for you to try it out and give us feedback.

To get started, [follow this guide on Github.](https://github.com/enricosada/fsharp-dotnet-cli-samples/wiki/Getting-Started)

## Known Issues
* Temporarily, this only works on Windows. There's a [blocking bug](https://github.com/Microsoft/visualfsharp/issues/997) on OS X and Linux. Should be fixed soon!
* No debugging support (no Portable PDBs) for F# on .NET Core, yet. We're working on it [on Github](TBD).
* F# Interactive on .NET Core is flaky and requires you to `#r` reference everything explicitly. We're working on it [on Github](TBD). 

## Ways to Contribute
Help us make F# even better on .NET Core:
* Help port the F# libraries you use to .NET Core. [This guide](https://blogs.msdn.microsoft.com/dotnet/2016/02/10/porting-to-net-core/) is a great place to start with porting
* File issues with the F# compiler [on Github](https://github.com/Microsoft/visualfsharp/issues?q=is%3Aopen+is%3Aissue+label%3AArea-CoreCLR), and contribute fixes
* File issues with the .NET CLI [on Github](https://github.com/dotnet/cli/issues), and contribute fixes.

## Learn More
* [.NET Core Documentation](http://dotnet.github.io/)
* [F# Documentation](https://msdn.microsoft.com/en-us/library/dd233154.aspx)


