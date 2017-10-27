# Choose the right APIs from now on
# Introducing Compatibility Analyzer

Have you ever wondered which APIs are deprecated and which should you use instead? Or have you ever used an API and then found out it didn't work on Mac or Linux? Have that ever happened to you too late when a major part of your code is already implemented and refactoring turns into a living hell? All those problems can be avoided now with the new [Compatibility Analyzer](https://www.nuget.org/packages/Microsoft.DotNet.Analyzers.Compatibility/) that allows you to get a live feedback on API usage and warnings about potential problems with compatibility and deprecation.

# What is Compatibility Analyzer
It is a Roslyn analyzer that comes as [NuGet package](https://www.nuget.org/packages/Microsoft.DotNet.Analyzers.Compatibility/). After referencing it in your project it automatically starts monitoring your code and underscores "dangerous" areas with a squiggle. On the right click you can get information about possible solution and suppress some or all warnings by type or operating system. 

![](GreenSquiggle.jpg) !

Figuratively speaking Compatibility Analyzer is your virtual API expert who looks over your shoulder and gives you feedback as you code and then turns into your diligent assistant who does routine tasks and manages notifications.

Here is a demo of Compatibility Analyzer in action:
## Demo

And now we'll take a deeper dive into the topic...

Two main areas where the analyzer is helpful today are compatibility of APIs with different platforms and APIs deprecation. Let's talk about deprecation first.

## Discovering deprecated APIs
The .NET Framework is a large product that is getting constantly upgraded to better serve customers needs, implement innovative approaches and address clients feedback. So deprecation of some APIs and appearance of brand new ones is a naturals process. For example, we have built v2 and v3 versions of the .NET Framework networking stack, so new HttpClient is a very different and much better API than WebClient and HttpWebRequest. Being a developer how can I know that I should use HttpClient instead of WebClient? There is a documentation, but we usually look in documentation only after we face some problems, when a part of a code is already implemented with an old API and refactoring process becomes pretty annoying. So it is very beneficial to get prompted that you're about to use a deprecated API right at the moment of its first appearance in the code.

Another good example would be a developer who comes from Java experience where he was using ArrayList, he looks for the same name in C# and finds it, so he goes ahead and uses it in his code. Only later he would find out that ArrayList was a deprecated API and using List<T> was much better idea. Here being advised on which API to use right away would be a great learning experience.

So that is exactly what Compatibility Analyzer does - prompts your right away as you code if you should consider moving to a newer technology. And not only that! It also provides you with detailed information for each deprecation case. In the Error List window you’ll immediately get warnings with unique  diagnostic ID per each deprecated API (in our example DE004) and by clicking on it you will get to a webpage with detailed problem resolution.

![](Warnings.jpg)

If you still decide to keep using the old API and suppress warnings about only this specific case, it can be easily done by right click on the highlighted member and choosing "Quick Actions and Refactorings". Here you will get two suppression options: locally (in Source) or globally (in Suppression File). We encourage developers to use global suppression since if you have decided that it is ok to use some deprecated API it should be ok to use it everywhere in your project, so you'll suppress it only once in Suppression File for all the occurrences of this API and keep your code clean.

## Discovering cross-platform issues
Because we’re a cross platform stack, some APIs don’t work on all the platforms. There are also types that work everywhere but certain members of that type would not be supported by each platform.  A good example is Console.WindowWidth that works on Windows but does not on Linux and MacOSX. However, we still included Console in .Net Core because of its wide usage. This way if your code has Console.WindowWidth it will crash when you try to run it on Linux or MacOSX. With Compatibility Analyzer you will get notified that the API is not supported which will help you to address the problem right away on a code composition phase. Even if you are targeting only one platform at this moment, your business goals might change in future and you will have to spend a lot of time going through your code figuring out if there are any cross-platform problems, where Compatibility Analyzer will highlight all the “dangerous” parts right away.

The experience of analyzer handling compatibility issues is very similar to deprecation, you'll see a green squiggle, diagnostic ID in Error List window and will be able to suppress warning by right click and choosing "Quick Actions and Refactorings". Unlike deprecation cases where you have two options: either keep using deprecated member and suppress warnings or not use it at all, here if you are developing your code only for certain platform  you can suppress all warnings for all other  platforms you don't plan to run your code on. To do so you just need to edit your project file and add a property PlatformCompatIgnore that lists all platforms to be ignored:
```
<PropertyGroup>
    <PlatformCompatIgnore>Linux;MacOSX</PlatformCompatIgnore>
</PropertyGroup>
```
If your code targets multiple platforms and you want to take advantage of API not supported on some of them, you can guard that part of the code with conditional compilation directives so it will be executed only on supported platform. For example this code will be runned only on .NET Framework 4.6:
```
#if NET46
    var w = Console.WindowWidth;
#endif
```
In future we are considering adding a functionality to automatically generate this guarding code on a right click on the API that is not cross-platforms.

You can also conditionally compile on operating system but you need to do it manually.

## Supported diagnostics
Right now the analyzer handles following cases:
* Usage of a .NET Standard API that will throw PlatformNoSupportedException (PC001)
* Usage of a .NET Standard API that aren’t available on .NET Framework 4.6.1 (PC002)
* Usage of a native API that doesn’t exist in UWP (PC003)
* Usage of an API that is marked as deprecated (DEXXXX)

## CI machine
All these diagnostics are available not only in the IDE, but also on the command line as part of building your project, which includes the CI server.

## Configuration
It is up to a user to decide how the diagnostics should be treated: as warnings, errors, suggestions, or to be turned off. For example as an architect you can decide that compatibility issues are errors, some deprecation are warnings and some are only suggestions. You can configure this separately by diagnostic ID and by project. To do so in your project tree -> Dependencies ->Analyzers -> (TBD...) right click on the diagnostic ID and Set Rule Set Severity and pick a desired option. 

## Summary
Compatibility Analyzer helps developers to be prompted right away if they are about to use a deprecated or non cross-platform functionality. Being notified so fast eliminates a need of refactoring the code in future and results in a better quality applications. 

## Call for feedback
We are still working on this analyzer and would appreciate any feedback. What we did good, what we did bad, is it useful at all, is there anything else you'd like us to add into the analyzer. Please try [it] (https://www.nuget.org/packages/Microsoft.DotNet.Analyzers.Compatibility/) out and leave your feedback [here] (https://github.com/dotnet/platform-compat/issues/new).
