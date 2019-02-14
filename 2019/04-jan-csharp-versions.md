# An update to C# versions and C# tooling

Starting with Visual Studio 2019 Preview 4, we'll be adjusting how C# versions are treated in .NET tooling.

## Summary of changes

Firstly, we're adding two new Language Version (LangVersion) values: `LatestMajor` and `Preview`. Here's how they stack up to the currently supported list of values:

| Language Version | Meaning |
|------------------|---------|
|**LatestMajor**|Latest supported major C# language version|
|**Preview**|Latest available preview C# language version|
|Latest|Latest supported C# language version (including minor version)|
|default|Depends on target framework|
| ISO-1 | C# 1.0/1.2 |
| ISO-2 | C# 2.0 |
| 3 | C# 3.0 |
| 4 | C# 4.0 |
| 5 | C# 5.0 |
| 6 | C# 6.0 |
| 7 | C# 7.0 |
| 7.1 | C# 7.1 |
| 7.2 | C# 7.2 |
| 7.3 | C# 7.3 |
| 8.0 | C# 8.0 |

When no LangVersion is specified, `default` is implied.

The meaning of `default` is now determined by the target framework of your project. When you target a preview framework that also has a corresponding preview C# version, that preview version is the default. If you do not target a preview framework, then `Latest` is chosen.

The following scenarios explain how this will work for .NET Core 3.0 preview and C# 8 preview:

### Targeting netcoreapp3.0 preview or netstandard2.1 preview

The default language version chosen in this scenario is `Preview`. The C# 8.0 features you have access to are based entirely on the version of the compiler (and thus the .NET SDK) that you are using. As you use future previews, you may get more (or slightly tweaked) features. When you build a project, the .NET SDK will emit a warning that this is all still in preview.

### Targeting .NET Framework

The default language version chosen in this scenario is `Latest`. Any use of a C# 8.0 feature is a compile error. Visual Studio tooling will prompt you with a quick fix that can change the language version for the project or solution when one of these errors is encountered. Because some features require underlying types or runtime features that are not available on .NET Framework, such as Default Interface Members, you may still get an error for those features when targeting .NET Framework.

### Multi-targeting netcoreapp3.0 preview or netstandard2.1 preview and .NET Framework

For the netcoreapp3.0/netstandard2.1 preview targets, the language version is `Preview`. A warning is emitted on build from the .NET SDK. For the .NET Framework target, the language version is `Latest`.

### Explicit LangVersion is used

If you explicitly set a LangVersion value, that will be respected and the previously mentioned default behavior is ignored.

## Experience when C# 8.0 and .NET Core 3.0 are GA

Eventually, C# 8.0 and .NET Core 3.0 will ship in a GA release. Here's what the relvant LangVersion values will map to at that time:

|LangVersion|Meaning|
|-----------|------|
|default|8.0|
|Latest|8.0|
|LatestMajor|8.0|
|Preview|Not yet determined|

Projects that do not specify a LangVersion will also be treated as if they are `default`.

If you created a project for C# 8.0 preview targeting .NET Core 3.0 preview or .NET Standard 2.1 preview, and also did not specify a LangVersion, it will be as if `default` is chosen. You will not be opted into `Preview` under any scenario once C# 8.0 and .NET Core 3.0 are GA.

## Rationale

Up until this point, the default C# version used in Visual Studio was equivalent to `LatestMajor`. This has proven to be awkward for two reasons:

1. C# now evolves between Visual Studio release cycles, but new projects in Visual Studio would still default to an older version.
2. The default C# language version is 7.3, despite C# 8.0 preview being a better choice for proejcts that target .NET Core 3.0 preview.

More generally, as we evolve C# and continue to release more features that align with a future .NET Core version, we want to make sure that you can use these features as seamlessly as possible. This also allows you to use features earlier in their development lifecycle, increasing the window of time that actionable feedback on a feature could influence its design.

## Support and compatibility for C# 8.0 preview features

The way to think about support is also a bit different. To allow the use of C# 8.0 preview features within a released Visual Studio 2019, support and compatibility concerns are distinguished by preview vs. released features:

* Any C# 7.3 and lower feature or behavior is fully supported and fully compatibile. No change from what currently exists today.
* Any C# 8.0 preview feature is unsupported.
* There is no compatibility guarantee from one C# 8.0 preview to another.

In short, if you use C# 8.0 preview in Visual Studio 2019, some features and behavior may change between now and when C# 8.0 fully releases.

Happy hacking!
