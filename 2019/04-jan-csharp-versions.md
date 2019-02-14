# An update to C# versions and C# tooling

Starting with Visual Studio 2019 Preview 4, we'll be adjusting how C# versions are treated in .NET tooling.

## Summary of changes

Firstly, we're adding two new Language Version (LangVer) values: `LatestMajor` and `Preview`. Here's how they stack up to the currently-supported list of values:

| Language Version | Meaning |
|------------------|---------|
|**LatestMajor**|Latest supported major C# language version|
|**Preview**|Latest available preview C# language version|
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
| 8 | C# 8.0 |

The only value not mentioned in this table is `default`. This is the most impactful change.

When no LangVer is specified, `default` is implied. The meaning of `default` is now determined by the target framework of your project. When you target a preview framework that also has a corresponding preview C# version, that preview version is the default. If you do not target a preview framework, then 

The following scenarios explain how this will work for .NET Core 3.0 preview and C# 8 preview:

### Targeting netcoreapp3.0 or netstandard2.1 preview

The default language version chosen in this scenario is `Preview`. The C# 8.0 features you have access to are based entirely on the version of the compiler (and thus the .NET SDK) that you are using. As you use future previews, you may get more (or slightly tweaked) features. When you build a project, the .NET SDK will emit a warning that this is all still in preview.

### Targeting .NET Framework

The default language version chosen in this scenario is `Latest`. Any use of a C# 8.0 feature is a compile error. Visual Studio tooling will prompt you with a quick fix that can change the language version for the project or solution when one of these errors is encountered. Because some features require underlying types or runtime features that are not available on .NET Framework, such as Default Interface Members, you may still get an error for those features when targeting .NET Framework.

### Multitargeting netcoreapp3.0 or netstandard2.1 preview and .NET Framework

For the netcoreapp3.0/netstandard2.1 preview targets, the language version is `Preview`. A warning is emitted on build from the .NET SDK. For the .NET Framework target, the language version is `Latest`.

### Explicit LangVer is used

If you explicitly set a LangVer value, that will be respected and the previously-mentioned default behavior is ignored.

## Experience when C# 8 and .NET Core 3.0 are GA

When C# 8 and .NET Core 3.0 are complete, the default LangVer will be `Latest` for new projects. `Latest` will correspond to C# 8.0. This means that if you are using C# 8.0 preview with projects targeting .NET Core 3.0 preview, your projects will be treated as if you are using to `Latest` when C# 8 and .NET Core 3.0 are fully released. This transition should mean that you won't have to modify project files unless you've already done so for other reasons.

## Rationale

Up until this point, the default C# version used in Visual Studio was equivalent to `LatestMajor`. This has proven to be awkward for two reasons:

1. C# now evolves in the middle of a Visual Studio release cycle, but new projects in Visual Stio would still be defaulting to an older version
2. The default C# language version for .NET Core 3.0 preview is not C# 8 preview, even though C# 8 will ship with .NET Core 3.0

More generally, as we evolve C# and continue to do more features that align with a future .NET Core version, we want to make sure that you can use these features as seamlessly as possible. This also allows you to use features earlier in their development lifecycle, increasing the window of time that actionable feedback on a feature that could influence its design.

Happy hacking!
