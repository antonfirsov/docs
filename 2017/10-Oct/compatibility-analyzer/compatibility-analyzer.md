# Preview of Compatibility Analyzer

Today we released a preview of [Compatibility Analyzer](https://www.nuget.org/packages/) (TBD) on NuGet, a Roslyn tool that provides a very easy way of dealing with compatibility and deprecation issues.

## Reasons for Analyzer

Quite often some program elements (APIs, classes, etc.) become deprecated and there are new much better ways to achieve same results. How can we motivate developers to use new approach? Can an old member just be deleted? No, that will cause a disaster in already written code that is using it. Can we publish a documentation informing users that this member is deprecated? This approach is neither convenient nor effective since not a lot of people would be looking up the documentation unless they face unresolvable issues, and we want to avoid that situation happening at all. Another solution was to use [Obsolete] attribute. In this case there is only one error code (CS0612) for all members marked [Obsolete] that just informs it has being obsolete without any information on what should be used instead. If user decides to use the old code anyways there are no handy ways to suppress all the warnings, the code and Error List Window would be lit up like a Christmas tree making user experience not enjoyable. Another inconvenience related to [Obsolete] attribute is that when a member becomes obsolete, we need to update the code by adding the attribute and ship a new version, and users need to get this new version.

All this pain can be avoided by using Compatibility Analyzer that will highlight “dangerous” areas and provide recommended solution while the user is typing.

## What is Compatibility Analyzer

Compatibility Analyzer is a Roslyn appication that runs at edit-time and prompts user whenever deprecated or incompatible functionality is used. Each warning here has its own error code, so it is very easy to pop up the info about what is happening with this specific member and what should be used instead. There are no other steps required to run a "spell check" of your code. Once git package is added, you can forget about it and enjoy a benefit of “virtual API expert” that “looks over your shoulder and gives you feedback" as you code.

## Using the Analyzer

Let’s take a closer look at the Compatibility Analyzer. To start using it you just need to add a NuGet package (TBD).

Then once you are trying to use a member that is either deprecated or might have compatibility issue, you’ll see a green squiggle line and a pop up message informing what the problem is.

![](GreenSquiggle.jpg)

Right now the analyzer tracks following scenarios:
* APIs that are in .NET Standard (TBD version?) but don’t exist in .NET Framework 4.6.1 yet
* APIs in .NET Standard/.NET Core that will throw PlatformNotSupported on some OS (Linux, macOS, Windows)
* Deprecation

In the Error List window you’ll immediately get a warning with corresponding code for this specific case (in our example DE0004) and by clicking on it you will get to a webpage with detailed problem resolution.

![](Warnings.jpg)

If you decide to keep using the obsolete member and suppress warnings about it, right click on the highlighted member and choose "Quick Actions and Refactorings" to get two suppression options: locally (in Source) or globally (in Suppression File).

It is also possible to ignore certain platforms you don't plan to run your code on. Just edit your project file and add a property PlatformCompatIgnore that lists all platforms to be ignored:
```
<PropertyGroup>
    <PlatformCompatIgnore>Linux;MacOSX</PlatformCompatIgnore>
</PropertyGroup>
```
## Demo

(TBD)

## Summary

Compatibility Analyzer provides a nice and easy method to notify developer about compatibility and deprecation issues and allows to suppress warnings in one click. It works on its own, no need to run extra checks or tools. In future we are planning to ship it as part of Visual Studio. Please try it out and give us your feedback here. (TBD)