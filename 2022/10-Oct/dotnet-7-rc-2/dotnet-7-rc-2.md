---
post_title: Announcing .NET 7 Release Candidate 2
microsoft_alias: jodou
author1: JonDouglas
post_slug: announcing-dotnet-7-rc-2
categories: .NET
featured_image: dotnet7rc2.png
desired_publication_date: 2022-10-11
post_date: 2022-10-11 11:00:00
summary: .NET 7 Release Candidate 2 is the second of two release candidates that developers can now use in production. This post recaps major features included in the fastest .NET version to date.
---

Today we announce .NET 7 Release Candidate 2. This is the final release candidate (RC) for .NET 7 and is supported in production.

You can [download .NET 7 Release Candidate 2](https://dotnet.microsoft.com/download/dotnet/7.0), for Windows, macOS, and Linux.

* [Installers and binaries](https://dotnet.microsoft.com/download/dotnet/7.0)
* [Container images](https://mcr.microsoft.com/catalog?search=dotnet/)
* [Linux packages](https://github.com/dotnet/core/blob/master/release-notes/7.0/)
* [Release notes](https://github.com/dotnet/core/tree/master/release-notes/7.0)
* [Known issues](https://github.com/dotnet/core/blob/main/release-notes/7.0/known-issues.md)
* [GitHub issue tracker](https://github.com/dotnet/core/issues)

.NET 7 Release Candidate 2 has been tested with Visual Studio 17.4 Preview 3. We recommend you use the [preview channel builds](https://visualstudio.com/preview) if you want to try .NET 7 with Visual Studio family products. If you're on macOS, we recommend using the latest [Visual Studio 2022 for Mac preview](https://visualstudio.microsoft.com/vs/mac/preview/).

Don't forget about [.NET Conf 2022](https://dotnetconf.net). Join us November 8-10, 2022 to celebrate the .NET 7 release!

In this blog, we'll highlight the core themes of .NET 7 and provide you with resources to dive deeper into the details.

For a more detailed recap of all the features and improvements included in .NET 7 Release Candidate 2, check out the previous .NET 7 blog posts:

- [Announcing .NET 7 Preview 1](https://devblogs.microsoft.com/dotnet/announcing-net-7-preview-1/)
- [Announcing .NET 7 Preview 2](https://devblogs.microsoft.com/dotnet/announcing-dotnet-7-preview-2/)
- [Announcing .NET 7 Preview 3](https://devblogs.microsoft.com/dotnet/announcing-dotnet-7-preview-3/)
- [Announcing .NET 7 Preview 4](https://devblogs.microsoft.com/dotnet/announcing-dotnet-7-preview-4/)
- [Announcing .NET 7 Preview 5](https://devblogs.microsoft.com/dotnet/announcing-dotnet-7-preview-5/)
- [Announcing .NET 7 Preview 6](https://devblogs.microsoft.com/dotnet/announcing-dotnet-7-preview-5/)
- [Announcing .NET 7 Preview 7](https://devblogs.microsoft.com/dotnet/announcing-dotnet-7-preview-7/)
- [Announcing .NET 7 Release Candidate 1](https://devblogs.microsoft.com/dotnet/announcing-dotnet-7-rc-1/)

*Note: Take a minute to re-visit the [Announcing .NET 7 Release Candidate 1](https://devblogs.microsoft.com/dotnet/announcing-dotnet-7-rc-1/) blog as there was an omission of some exciting features that came in last minute.*

## C# 11
C# 11 is the latest version of C# that is now available in .NET 7.

We design and develop C# in the open. You can join us on the [CSharpLang repo](https://github.com/dotnet/csharplang) to see the latest C# feature proposals and meeting notes. Once work is planned you can monitor progress through our [Feature Status page](https://github.com/dotnet/roslyn/blob/main/docs/Language%20Feature%20Status.md). To learn more about C# 11, check out the following:

- https://devblogs.microsoft.com/dotnet/early-peek-at-csharp-11-features/
- https://devblogs.microsoft.com/dotnet/csharp-11-preview-updates/
- https://devblogs.microsoft.com/dotnet/csharp-11-preview-august-update/
- https://learn.microsoft.com/dotnet/csharp/whats-new/csharp-11

*Note: To try out the C# 11 preview features, create a C# project and [set the LangVersion to Preview](https://learn.microsoft.com/dotnet/csharp/language-reference/compiler-options/language#langversion).*

## Libraries
The .NET libraries are constantly improving. New APIs containing brand new functionality is regularly added. Performance improvements are being made to existing APIs, providing you with benefits by simply upgrading. Completely new libraries are being made to assist your daily jobs-to-be-done. Many .NET library enhancements are included in these blogs, but don't forget to also check out these library deep dives:

- https://devblogs.microsoft.com/dotnet/preview-features-in-net-6-generic-math/
- https://devblogs.microsoft.com/dotnet/dotnet-7-generic-math/
- https://devblogs.microsoft.com/dotnet/regular-expression-improvements-in-dotnet-7/
- https://devblogs.microsoft.com/dotnet/announcing-rate-limiting-for-dotnet/

## SDK
Each .NET release has a plethora of improvements to the .NET SDK which includes the core functionality to create, build, and manage your .NET projects. Many enhancements were already included in previous preview blogs that you can read up on. To read more about new SDK features, see the following:

- https://devblogs.microsoft.com/dotnet/announcing-builtin-container-support-for-the-dotnet-sdk/
- https://devblogs.microsoft.com/nuget/introducing-central-package-management/

## How to re-enable reflection fallback for System.Text.Json source generation

.NET 7 introduced an [intentional breaking change](https://learn.microsoft.com/dotnet/core/compatibility/serialization/7.0/reflection-fallback) which removes silent fallback to reflection-based serialization in System.Text.Json source generators. Based on early feedback we have been receiving from customers, it appears that quite a few users have (mostly accidentally) taken a dependency on the fallback behavior.

Even though a workaround for the breaking change has been documented, it still requires a code change which might not be possible in certain cases. Starting in .NET 7 RC 2, you can re-enable reflection fallback globally using the provided AppContext compatibility switch. Add the following entry to your application's project file to re-enable reflection fallback for all source-generated contexts in your app:

```xml
<ItemGroup>
  <RuntimeHostConfigurationOption Include="System.Text.Json.Serialization.EnableSourceGenReflectionFallback" Value="true" />
</ItemGroup>
```
For more information on using AppContext switches, see the article on [.NET runtime configuration settings](https://learn.microsoft.com/dotnet/core/runtime-config/).

## New analyzers that help you consume APIs the right way

### Implement Generic Math interfaces correctly

[dotnet/runtime #69775](https://github.com/dotnet/runtime/issues/69775)

Ensures the .NET Generic Math interfaces that uses [Curiously Recurring Template Pattern](https://wikipedia.org/wiki/Curiously_recurring_template_pattern) (CRTP) are implemented correctly in user code. In particular it warns if a type that implements the .NET Generic Math interfaces (that implements CRTP pattern) do NOT fill the generic type parameter with the type itself.

For example:

```csharp
public readonly struct DateOnly : IParsable<DateOnly> // correct implementation of IParsable<TSelf> interface
{ ... }
public readonly struct MyDate : IParsable<DateOnly> // Warns: "The 'IParsable<TSelf>' requires the 'TSelf' type parameter to be filled with the derived type 'MyDate' " the type parameter TSelf
{ ... }
```

### Prevent behavioral change in built-in operators for IntPtr and UIntPtr

[dotnet/runtime #74022](https://github.com/dotnet/runtime/issues/74022)

Some built in operators added in .NET 7 for System.IntPtr and System.UIntPtr behave differently than the user defined operators in .NET 6 and below. Some operators that used to throw in unchecked context while overflowing now no longer throw unless wrapped within checked context, and some operators that not used to throw in checked context now would throw when overflow unless wrapped within unchecked context. The analyzer detects the code that could cause those behavioral changes and informs it.

For example:

```csharp
checked
{
    intPtr2 = intPtr1 + 2; // Warns: "Starting with .NET 7 the operator '+' will throw when overflowing in a checked context. Wrap the expression with an 'unchecked' statement to restore the .NET 6 behavior."

    intPtr2 = intPtr1 - 2; // Warns: "Starting with .NET 7 the operator '-' will throw when overflowing in a checked context. Wrap the expression with an 'unchecked' statement to restore the .NET 6 behavior."

    void* ptr = (void*)intPtr1; // Warns: "Starting with .NET 7 the explicit conversion '(void*)IntPtr' will throw when overflowing in a checked context. Wrap the expression with an 'unchecked' statement to restore the .NET 6 behavior."

    intPtr2 = (IntPtr)ptr; // Warns: "Starting with .NET 7 the explicit conversion '(IntPtr)void*' will throw when overflowing in a checked context. Wrap the expression with an 'unchecked' statement to restore the .NET 6 behavior."
}

intPtr1 = (IntPtr)longValue; // Warns: "Starting with .NET 7 the explicit conversion '(IntPtr)Int64' will not throw when overflowing in an unchecked context. Wrap the expression with a 'checked' statement to restore the .NET 6 behavior."

int a = (int)intPtr1; // Warns: "Starting with .NET 7 the explicit conversion '(Int32)IntPtr' will not throw when overflowing in an unchecked context. Wrap the expression with a 'checked' statement to restore the .NET 6 behavior."
```

## Contributor spotlight: Alan Hayward

A huge "Thank you" goes out to all our community members. We deeply appreciate your thoughtful contributions. We asked contributor [@a74nh](https://github.com/a74nh) to share his thoughts.

![Alan Hayward](./alanhayward.jpg)

In Alan's own words:

I'm Alan Hayward and I live just outside of Manchester in the UK.

I'm fairly new to .NET and its ecosystem, but I've come from a long history of working on compilation like technologies. Early in my career I worked for a startup that produced a DBT called QuickTransit - more famously known as Apple's Rosetta (back from when they moved from PPC to X86) - but I was more concerned with its less known applications across architectures: SPARC Solaris to X86/Itanium/PPC Linux and X86 Linux to MIPS/PPC/SPARC/Z Linux, and probably a few more combinations now lost the the mists of time.

In 2014 I joined Arm's new Manchester UK office and worked on adding SVE support to GCC and GDB, then becoming the Arm/AArch64 maintainer for GDB. Today I'm in Arm's Runtimes team, which looks at the performance and security of popular cloud and desktop Runtimes. For a while I worked on OpenJDK and Graal, including adding support for Arm's Pointer Authentication (PAC) security feature to protect control flow in OpenJDK.

More recently I've been working with some of that team on .Net - getting us up to speed, improving some of the library routines with intrinsics, and adding an If Conversion pass to Ryujit. I'm excited to see what improvements to the Arm64 port we can add.

What do I like about .Net? I love the simplicity of C# - you can get stuff done easily and quickly (everyone who can program, can program in C#). Similarly, the Jit itself is intuitive in the way it applies compiler techniques. Finally, a shoutout to SuperPMI - it's is the tool I wish I had on previous projects, being able to quickly get diffs across test suites is amazing.

When I'm not working, I can often be found with a small paintbrush trying to improve my miniature painting, listening to too many podcasts and audiobooks.

I'm very happy to be on board and many thanks to those in the community I've interacted with up to now and hope to work with more of you over time.

## Support

.NET 7 is released under **Standard Support**. Although this is the new name for what was formerly called **Current**, there is _no change_ to the support duration. Odd-numbered .NET releases ship under **Standard Support** which lasts 18 months. There are no changes to the name or duration of **Long-Term Support (LTS)**, which still lasts for 36 months. See our [.NET and .NET Core Support Lifecycle](https://dotnet.microsoft.com/platform/support/policy/dotnet-core) document for more details.

## Roadmaps

Releases of .NET include products, libraries, runtime, and tooling, and represent a collaboration across multiple teams inside and outside Microsoft. You can learn more about these areas by reading the product roadmaps:

* [ASP.NET Core 7 and Blazor Roadmap](https://github.com/dotnet/aspnetcore/issues/39504)
* [EF 7 Roadmap](https://docs.microsoft.com/ef/core/what-is-new/ef-core-7.0/plan)
* [ML.NET](https://github.com/dotnet/machinelearning/blob/main/ROADMAP.md)
* [.NET MAUI](https://github.com/dotnet/maui/wiki/Roadmap)
* [WinForms](https://github.com/dotnet/winforms/blob/main/docs/roadmap.md)
* [WPF](https://github.com/dotnet/wpf/blob/main/roadmap.md)
* [NuGet](https://github.com/NuGet/Home/issues/11571)
* [Roslyn](https://github.com/dotnet/roslyn/blob/main/docs/Language%20Feature%20Status.md)
* [Runtime](https://github.com/dotnet/core/blob/main/roadmap.md)

## Closing

We appreciate and [thank you](https://dotnet.microsoft.com/thanks) for your all your support and contributions to .NET. Please [give .NET 7 Release Candidate 2 a try](https://dotnet.microsoft.com/download/dotnet/7.0) and tell us what you think!
