---
post_title: Introducing the New T4 Command-Line Tool for .NET
author1: mikecorsaro
post_slug: t4-command-line-tool-for-dotnet
microsoft_alias: mikecorsaro
featured_image: blogpst-NET.png
categories: .NET, .NET Core
tags: Visual Studio, T4, text templating, .NET Core
summary: Learn about the new T4 text template utility built with .NET 6
desired_publication_date: 2023-06-20
post_date: 2023-06-20 10:05:00
---

We're happy to announce that Visual Studio 2022 v17.6 now includes an updated Text Template Transformation Toolkit (T4) command-line tool built with .NET 6.

For those unfamiliar, T4 is a powerful framework that allows you to automate the creation of text files. It's perfect for automating the creation of HTML, XAML, or even code models from REST APIs. These template files can contain invokable .NET code and string literals, so it's critical that our users are able to utilize the latest .NET 6+ features and libraries.

## Try It

Using the new `TextTransformCore.exe` utility is simple: all arguments are the same as `TextTransform.exe`. The location of the new utility is also the same and can be found under `{VS_INSTALL_PATH}\Common7\IDE\TextTransformCore.exe`.

## Feedback

We want to hear from you! Please file feedback or issues in our [Developer Community](https://developercommunity.visualstudio.com/).

Additionally, there are a few questions we have about how best to support our T4 users who wish to use .NET (Core) libraries, so we've prepared a [short survey](https://www.surveymonkey.com/r/7HY8DV8).

## Limitations

We currently do not yet support in-IDE or MSBuild task file generators for .NET 6+. However, if your template does not rely on invoking in-IDE services you can work around this by using the new `TextTransformCore.exe` with the `Exec` command.

Here's an example, as a PreBuild step:

```xml
<Target Name="PreBuild" BeforeTargets="PreBuildEvent">
    <Exec Command="'$(DevEnvDir)TextTransformCore.exe' '$(ProjectDir)MyFile.tt'" />
</Target>
```

### Known issues

There's currently a known issue where transforms will fail when setting the attribute `hostSpecific` to true. If you do not use `this.Host` we recommend that you set the attribute `hostSpecific="false"`. This issue only applies to `TextTransformCore.exe`.

Thanks for your patience &mdash; we're excited to see how you utilize these new T4 capabilities.
