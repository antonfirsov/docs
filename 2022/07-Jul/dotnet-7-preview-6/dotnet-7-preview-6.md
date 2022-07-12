---
post_title: Announcing .NET 7 Preview 6
microsoft_alias: jodou
author1: jodou@microsoft.com.com
post_slug: announcing-dotnet-7-preview-6
categories: .NET
featured_image: dotnet7-preview6.png
desired_publication_date: 2022-07-12
summary: .NET 7 Preview 6 is now available with improvements to type converters, JSON contract customization, System.Formats.Tar API updates, constraints to .NET template authoring, and performance enhancements in the CodeGen area.
---

Today we released .NET 7 Preview 6. This preview of .NET 7 includes improvements to type converters, JSON contract customization, System.Formats.Tar API updates, constraints to .NET template authoring, and performance enhancements in the CodeGen area.

You can [download .NET 7 Preview 6](https://dotnet.microsoft.com/download/dotnet/7.0), for Windows, macOS, and Linux.

* [Installers and binaries](https://dotnet.microsoft.com/download/dotnet/7.0)
* [Container images](https://mcr.microsoft.com/catalog?search=dotnet/)
* [Linux packages](https://github.com/dotnet/core/blob/master/release-notes/7.0/)
* [Release notes](https://github.com/dotnet/core/tree/master/release-notes/7.0)
* [Known issues](https://github.com/dotnet/core/blob/main/release-notes/7.0/known-issues.md)
* [GitHub issue tracker](https://github.com/dotnet/core/issues)

.NET 7 Preview 6 has been tested with Visual Studio 17.3 Preview 3. We recommend you use the [preview channel builds](https://visualstudio.com/preview) if you want to try .NET 7 with Visual Studio family products. If you're on macOS, we recommend using the latest [Visual Studio 2022 for Mac preview](https://visualstudio.microsoft.com/vs/mac/preview/). Now, let's get into some of the latest updates in this release.

## Type converters

There are now exposed type converters for the newly added primitive types `DateOnly`, `TimeOnly`, `Int128`, `UInt128`, and `Half`.

```csharp
namespace System.ComponentModel
{
    public class DateOnlyConverter : System.ComponentModel.TypeConverter
    {
        public DateOnlyConverter() { }
    }

    public class TimeOnlyConverter : System.ComponentModel.TypeConverter
    {
        public TimeOnlyConverter() { }
    }

    public class Int128Converter : System.ComponentModel.BaseNumberConverter
    {
        public Int128Converter() { }
    }

    public class UInt128Converter : System.ComponentModel.BaseNumberConverter
    {
        public UInt128Converter() { }
    }

    public class HalfConverter : System.ComponentModel.BaseNumberConverter
    {
        public HalfConverter() { }
    }
}
```

### Usage Example

```csharp
TypeConverter dateOnlyConverter = TypeDescriptor.GetConverter(typeof(DateOnly));
// produce DateOnly value of DateOnly(1940, 10, 9)
DateOnly? date = dateOnlyConverter.ConvertFromString("1940-10-09") as DateOnly?;

TypeConverter timeOnlyConverter = TypeDescriptor.GetConverter(typeof(TimeOnly));
// produce TimeOnly value of TimeOnly(20, 30, 50)
TimeOnly? time = timeOnlyConverter.ConvertFromString("20:30:50") as TimeOnly?;

TypeConverter halfConverter = TypeDescriptor.GetConverter(typeof(Half));
// produce Half value of -1.2
Half? half = halfConverter.ConvertFromString(((Half)(-1.2)).ToString()) as Half?;

TypeConverter Int128Converter = TypeDescriptor.GetConverter(typeof(Int128));
// produce Int128 value of Int128.MaxValue which equal 170141183460469231731687303715884105727
Int128? int128 = Int128Converter.ConvertFromString("170141183460469231731687303715884105727") as Int128?;

TypeConverter UInt128Converter = TypeDescriptor.GetConverter(typeof(UInt128));
// produce UInt128 value of UInt128.MaxValue which equal 340282366920938463463374607431768211455
UInt128? uint128 = UInt128Converter.ConvertFromString("340282366920938463463374607431768211455") as UInt128?;
```

## JSON contract customization

In certain situations developers serializing or deserializing JSON find that they don't want to or cannot change types because they either come from external library or it would greatly pollute the code but need to make some changes which influence serialization like removing property, changing how numbers get serialized, how object is created etc. They frequently are forced to either write wrappers or custom converters which are not only a hassle but also make serialization slower.

JSON contract customization allows user for more control over what and how types get serialized or deserialized.

### Opting into customization

There are two basic ways developers can "plug" into the customization, they both end up assigning `JsonSerializerOptions.TypeInfoResolver` and require assigning resolver:

- Developer can use `DefaultJsonTypeInfoResolver` and add their modifier, all modifiers will be called serially:

```csharp
JsonSerializerOptions options = new()
{
    TypeInfoResolver = new DefaultJsonTypeInfoResolver()
    {
        Modifiers =
        {
            (JsonTypeInfo jsonTypeInfo) =>
            {
                // your modifications here, i.e.:
                if (jsonTypeInfo.Type == typeof(int))
                {
                    jsonTypeInfo.NumberHandling = JsonNumberHandling.AllowReadingFromString;
                }
            }
        }
    }
};

Point point = JsonSerializer.Deserialize<Point>(@"{""X"":""12"",""Y"":""3""}", options);
Console.WriteLine($"({point.X},{point.Y})"); // (12,3)

public class Point
{
    public int X { get; set; }
    public int Y { get; set; }
}
```

- Writing own custom resolver by implementing `System.Text.Json.Serialization.Metadata.IJsonTypeInfoResolver`.
- When type is not handled code should return `null`.
- `IJsonTypeInfoResolver` can be combined with others into effective resolver which will return first non-null answer. For example `JsonTypeInfoResolver.Combine(new MyResolver(), new DefaultJsonTypeInfoResolver())`.

### Customizations

`IJsonTypeInfoResolver` job is to provide `JsonTypeInfo` for any `Type` serializer requests - this will only happen once per type per options.
`JsonTypeInfo.Kind` will determine what knobs developer can change and is determined based on converter which is determined based on converters provided to options. For example `JsonTypeInfoKind.Object` means `Properties` can be added/modified while `JsonTypeInfoKind.None` means none of the knobs is guaranteed to be used - that can happen when type has custom converter.

`JsonTypeInfo` is either created by `DefaultJsonTypeInfoResolver` with pre-populated knobs coming from i.e. custom attributes or can be created from scratch by user: `JsonTypeInfo.CreateJsonTypeInfo` - creating from scratch means user will also need to set `JsonTypeInfo.CreateObject`.

### Customizing properties

Properties are only relevant when `JsonTypeInfo.Kind == JsonTypeInfoKind.Object` and in case of `DefaultJsonTypeInfoResolver` will be pre-populated.
They can be modified or created by using `JsonTypeInfo.CreateJsonPropertyInfo` and added to the list of properties, i.e. say you got a class from separate library which has weirdly designed APIs which you can't change:

```csharp
class MyClass
{
    private string _name = string.Empty;
    public string LastName { get; set; }

    public string GetName() => _name;
    public void SetName(string name)
    {
        _name = name;
    }
}
```

Before this feature existed you'd need to wrap your type hierarchy or create your own custom converter for that type. Now you can simply fix it:

```csharp
JsonSerializerOptions options = new()
{
    TypeInfoResolver = new DefaultJsonTypeInfoResolver()
    {
        Modifiers = { ModifyTypeInfo }
    }
};

MyClass obj = new()
{
    LastName = "Doe"
};

obj.SetName("John");

string serialized = JsonSerializer.Serialize(obj, options); // {"LastName":"Doe","Name":"John"}

static void ModifyTypeInfo(JsonTypeInfo ti)
{
    if (ti.Type != typeof(MyClass))
        return;

    JsonPropertyInfo property = ti.CreateJsonPropertyInfo(typeof(string), "Name");
    property.Get = (obj) =>
    {
        MyClass myClass = (MyClass)obj;
        return myClass.GetName();
    };

    property.Set = (obj, val) =>
    {
        MyClass myClass = (MyClass)obj;
        string value = (string)val;
        myClass.SetName(value);
    };

    ti.Properties.Add(property);
}
```

### Conditional serialization of properties

In some usage scenarios it's required that some default values don't get serialized. I.e. you don't want `0` to show up in JSON for certain properties. It was possible to get that scenario to work before by using [JsonIgnoreAttribute](https://docs.microsoft.com/dotnet/api/system.text.json.serialization.jsonignoreattribute?view=net-6.0) with `JsonIgnoreCondition.WhenWritingDefault`.
The problem occurs when your default value is not `0` and it's something different, i.e. `-1` or it depends on some external setting.

Now it's possible to set your own predicate `ShouldSerialize` with any condition you'd like. I.e. say you have `string` property and you'd want `N/A` to not show up in JSON:

```csharp
// string property you'd like to customize
JsonPropertyInfo property = ...;

property.ShouldSerialize = (obj, val) =>
{
    // in this specific example we don't use parent but it's available if needed
    MyClass parentObj = (MyClass)obj;
    string value = (string)val;
    return value != "N/A";
};
```

### Sample: Ignoring properties with specific name or type

```C#
var modifier = new IgnorePropertiesWithNameOrType();
modifier.IgnorePropertyWithType(typeof(SecretHolder));
modifier.IgnorePropertyWithName("IrrelevantDetail");

JsonSerializerOptions options = new()
{
    TypeInfoResolver = new DefaultJsonTypeInfoResolver()
    {
        Modifiers = { modifier.ModifyTypeInfo }
    }
};

ExampleClass obj = new()
{
    Name = "Test",
    Secret = new SecretHolder() { Value = "MySecret" },
    IrrelevantDetail = 15,
};

string output = JsonSerializer.Serialize(obj, options); // {"Name":"Test"}

class ExampleClass
{
    public string Name { get; set; }
    public SecretHolder Secret { get; set; }
    public int IrrelevantDetail { get; set; }
}

class SecretHolder
{
    public string Value { get; set; }
}

class IgnorePropertiesWithNameOrType
{
    private List<Type> _ignoredTypes = new List<Type>();
    private List<string> _ignoredNames = new List<string>();

    public void IgnorePropertyWithType(Type type)
    {
        _ignoredTypes.Add(type);
    }

    public void IgnorePropertyWithName(string name)
    {
        _ignoredNames.Add(name);
    }

    public void ModifyTypeInfo(JsonTypeInfo ti)
    {
        JsonPropertyInfo[] props = ti.Properties.Where((pi) => !_ignoredTypes.Contains(pi.PropertyType) && !_ignoredNames.Contains(pi.Name)).ToArray();
        ti.Properties.Clear();

        foreach (var pi in props)
        {
            ti.Properties.Add(pi);
        }
    }
}
```

## System.Formats.Tar API Updates

In Preview 4, the `System.Formats.Tar` assembly was introduced. It offers APIs for manipulating TAR archives.

In Preview 6, some changes were made to cover a few special cases:

### Global Extended Attributes specialized class

 The initial design was assuming that only PAX TAR archives could contain a single Global Extended Attributes (GEA) entry in the first position, but it was discovered that TAR archives can contain multiple GEA entries, which can affect all subsequent entries until encountering a new GEA entry or the end of the archive.

 It was also discovered that GEA entries should not be expected only in archives containing PAX entries exclusively: they can show up in archives that intermix entries of different formats. So a new class was added to describe a GEA entry:

```diff
+ public sealed partial class PaxGlobalExtendedAttributesTarEntry : PosixTarEntry
+ {
+     public PaxGlobalExtendedAttributesTarEntry(IEnumerable<KeyValuePair<string, string>> globalExtendedAttributes) { }
+     public IReadOnlyDictionary<string, string> GlobalExtendedAttributes { get { throw null; } }
+ }
```

### Entry format, not archive format

Since it was also discovered that entries of different formats can be intermixed in a single TAR archive, the `TarFormat` enum was renamed to `TarEntryFormat`:

```diff
-public enum TarFormat
+public enum TarEntryFormat
{
    ...
}
```

And a new property was added to `TarEntry` to expose the entry's format:

```diff
public abstract partial class TarEntry
{
    ...
+    public TarEntryFormat Format { get { throw null; } }
    ...
}
```

### Writing and reading changes

The `Format` property was removed from `TarReader` because no archive is expected to have all its entries in a single format.

Since GEA entries are now being described with their own specialized class, and multiple entries of this type can be found in a single archive, the dictionary property from the `TarReader` was also removed:

```diff
public sealed partial class TarReader : IDisposable
{
    ...
-    public TarFormat Format { get { throw null; } }
-    public IReadOnlyDictionary<string, string>? GlobalExtendedAttributes { get { throw null; } }
    ...
}
```

The addition of the specialized GEA class also affected `TarWriter`:

- The constructor that used to take the dictionary for a single first-position GEA entry was removed.
- A new constructor that takes only the stream and the `leaveOpen` boolean was added.
- The constructor that takes the `TarFormat` was kept, but the enum was renamed, and a default value was set to `Pax`. The method's documentation was changed to explain that the specified format parameter only applies to the `TarWriter.WriteEntry` method that adds an entry from a file.

```diff
public sealed partial class TarWriter : IDisposable
{
    ...
-    public TarWriter(Stream archiveStream, IEnumerable<KeyValuePair<string, string>>? globalExtendedAttributes = null, bool leaveOpen = false) { }
+    public TarWriter(Stream archiveStream, bool leaveOpen = false) { }
-    public TarWriter(Stream archiveStream, TarFormat archiveFormat, bool leaveOpen = false) { }
+    public TarWriter(Stream archiveStream, TarEntryFormat format = TarEntryFormat.Pax, bool leaveOpen = false) { }
     public void WriteEntry(string fileName, string? entryName) { }
    ...
}
```

## Template authoring

### Constraints

Preview 6 introduces the concept of *constraints* to .NET Templates. Constraints allow you to define the context in which your templates are allowed - which can help the template engine determine which templates it should show in commands like `dotnet new list`. For this release we have added support for three kinds of constraints:
* Operating System - which limits templates based on the Operating System of the user
* Template Engine Host - which limits templates based on which host is executing the Template Engine - this is usually the .NET CLI itself, or embedded scenarios like the New Project Dialog in Visual Studio/Visual Studio for Mac.
* Installed Workloads - requires that the specified .NET SDK workload is installed before the template will become available

In all cases, describing these constraints is as easy as adding a new `constraints` section to your template's configuration file:

```json
 "constraints": {
       "web-assembly": {
           "type": "workload",
           "args": "wasm-tools"
       },
   }
```

These templates can be named, and we'll use that name when informing the user _why_ they couldn't invoke your template.

Currently these constraints are supported in the .NET CLI, and we're working with our partners in the Visual Studio teams to incorporate them into the project and item creation experiences that you already know.

We hope that this feature will lead to a more consistent experience for users of the SDK, regardless of their editor choices, make it easier to guide users to necessary template prerequisites, and well as help us de-clutter the template list for common scenarios like `dotnet new list`.  In future previews of .NET 7 we plan to add support for constraints based on common MSBuild Properties!

For more examples see the [documentation for constraints](https://github.com/dotnet/templating/blob/main/docs/Constraints.md), and for discussion about new kinds of constraints please join the [discussion](https://github.com/dotnet/templating/discussions/4936) at the Template Engine Repository.


### Multi-choice parameters

Preview 6 also adds a new ability for `choice` parameters - the ability for a user to specify more than one value in a single selection. This can be used in the same way a `Flags`-style enum might be used. Common examples of this type of parameter might be:

* Opting into multiple forms of authentication on the `web` template
* Choosing multiple target platforms (ios, android, web) at once in the `maui` templates

Opting in to this behavior is as simple as adding `"allowMultipleValues": true` to the parameter definition in your template's configuration. Once you do, you'll get access to a number of helper functions to use in your template's content as well to help detect specific values that the user chose.

For a full explanation of the feature see the [documentation for multi-choice parameters](https://github.com/dotnet/templating/blob/main/docs/Reference-for-template.json.md#multichoice-symbols-specifics).

### Exit codes unification and reporting

Preview 6 also unified the exit codes reported by the Template Engine. This should help users that rely on scripting in their shell of choice to have a more consistent error-handling experience. In addition, errors reported by the .NET CLI now include a link to find detailed information about each exit code:

```shell
➜ dotnet new unknown-template
No templates found matching: 'unknown-template'.

To list installed templates, run:
   dotnet new list
To search for the templates on NuGet.org, run:
   dotnet new search unknown-template


For details on the exit code, refer to https://aka.ms/templating-exit-codes#103
```

## CodeGen

### Community PRs (Many thanks to JIT community contributors!)
- @am11 added a glossary for {M}IBC in https://github.com/dotnet/runtime/pull/68111.
- @aromaa contributed https://github.com/dotnet/runtime/pull/70655. In some cases, inlining can reveal that a pinned local refers to a stack location; this change detects that case and removes the pin, as it’s unnecessary and blocks some optimizations.
- @huoyaoyuan removed ternary workaround in order to remove redundant code when inlining string.IsNullOrEmpty in https://github.com/dotnet/runtime/pull/63095. 
- @singleaccretion made 41 PR contributions during Preview 6 ( https://github.com/dotnet/runtime/pulls?q=is%3Apr+is%3Aclosed+label%3Aarea-CodeGen-coreclr+closed%3A2022-05-24..2022-06-22+author%3Asingleaccretion). A lot of this work is streamlining the JITs IR and improving how the JIT represents struct values, in particular around calls and call arguments.
-- Enabling TYP_STRUCT LCL_VAR/LCL_FLD call args on Windows x64 ( https://github.com/dotnet/runtime/pull/70777 ) and Windows x86 (https://github.com/dotnet/runtime/pull/70779  ) showing much improved code-quality in many cases when structs are passed as arguments
-- Deleting field sequences from LCL_FLD and VNF_PtrToArrElem https://github.com/dotnet/runtime/pull/68986 which simplified the JITs IR and resulted in great code-quality and throughput improvements. This greatly improved codegen for some cases of struct copies.
- @skiFoD made an optimization for removing unnecessary range checks in https://github.com/dotnet/runtime/pull/70222 (for example, x <= 255 for a byte x) and transforming "~x + 1" to "-x" in https://github.com/dotnet/runtime/pull/69600. 
- @Wraith2 improved the JIT to remove a lot of unnecessary unconditional jumps in https://github.com/dotnet/runtime/pull/69041 and added blsmsk xarch instruction for the pattern XOR(x, x - 1) in https://github.com/dotnet/runtime/pull/66561.  

### Dynamic PGO
- https://github.com/dotnet/runtime/pull/68703 adds support for guarded devirtualization for delegate calls. When dynamic PGO is enabled this allows the JIT to specialize and inline delegate calls when it determines this might be profitable. This can greatly increase performance as demonstrated by the following microbenchmark where dynamic PGO is now roughly 5x faster (was roughly 2.5x before) than no PGO.

For now only delegates bound to instance methods are supported. We expect that support for static methods will come in early previews for .NET 8.

```csharp
public class Benchmark
{
    private readonly long[] _nums;
    public Benchmark()
    {
        _nums = Enumerable.Range(0, 100000).Select(i => (long)i).ToArray();
    }

    [Benchmark]
    public long Sum() => _nums.Sum(l => l * l);
}
```

| Method |        Job |                              Toolchain |      Mean |    Error |   StdDev | Ratio |
|------- |----------- |--------------------------------------- |----------:|---------:|---------:|------:|
|    Sum | Job-QWNDLL |                     \nopgo\corerun.exe | 406.65 us | 0.718 us | 0.560 us |  1.00 |
|    Sum | Job-PNPEDU |                 \tieredpgo_no_delegate_gdv\corerun.exe | 172.77 us | 0.819 us | 0.766 us |  0.42 |
|    Sum | Job-KFFWQK | \tieredpgo_delegate_gdv\corerun.exe |  91.38 us | 0.263 us | 0.219 us |  0.22 |

- We started to implement **hot and cold splitting** and https://github.com/dotnet/runtime/pull/69763 is the first part of it.  
  - Hot/Cold splitting on ARM64 has been implemented in the JIT ([PR](https://github.com/dotnet/runtime/pull/70708)). This work largely consisted of generating long pseudo-instructions for branching between hot/cold sections, and loading constants from the data section.
  - We've also added support for hot/cold splitting of functions with exception handling ([PR](https://github.com/dotnet/runtime/pull/71236)). Without PGO data, our heuristic moves all exception handling funclets to the cold section, and copies “finally” blocks to the hot section; we’re operating under the assumption that exceptions occur rarely, but finally blocks are executed regardless of the presence of exceptions.
  - When running various SuperPMI collections, the JIT split ~14% of functions on the low end (no PGO data), and ~26% of functions on the high end (with PGO data). See more metrics [here](https://github.com/dotnet/runtimelab/pull/1923#issuecomment-1172558978).

### Arm64  
- https://github.com/dotnet/runtime/pull/70600 enabled LSE atomics in Windows Arm64. It improves lock related operation performance by up to 78%. 

![image](https://user-images.githubusercontent.com/63486087/177864481-2df454a3-fdc8-4668-9919-1752ae885241.png)

- https://github.com/dotnet/runtime/pull/70749 enabled addressing mode for gc type on Arm64 to gain up to 45% of performance. 
- https://github.com/dotnet/runtime/pull/71044 aligned arm64 data section for 16 byte SIMD16.
- https://github.com/dotnet/runtime/pull/70599 optimizes i % 2 and gives up to 17% throughput improvement. 

### Loop Optimizations  
- Loop cloning driven by type tests: https://github.com/dotnet/runtime/pull/70377 enables loop cloning based on loop invariant type tests such as those added by GDV. This effectively allows the fast path loop to hoist the type check out of the loop, leading to improved performance. For example:  

![image](https://user-images.githubusercontent.com/63486087/177864549-0b1c57f8-610f-4895-8cfc-52dcae803355.png)

- Started hoisting invariants out of multi-level nested loop in https://github.com/dotnet/runtime/pull/68061.

![image](https://user-images.githubusercontent.com/63486087/177864617-92ca94af-9f5e-4a8a-8b42-6c9722f65bf2.png)

### General Optimizations
- PR https://github.com/dotnet/runtime/pull/68874 improved handling of vector constants in the JIT including support for value numbering, constant propagation, and other optimizations already available to other constants.

## Contributor spotlight: Pent Ploompuu

Pent has contributed to .NET long before becoming an employee at Microsoft working on Microsoft Teams. Whether that's [cleaning up CacheEntry](https://github.com/dotnet/runtime/pull/59110) or [fixing regressions in integer formatting](https://github.com/dotnet/runtime/pull/57400), we'd like to express our gratitude to Pent for contributing to .NET over the years. Thanks Pent!

![Pent Ploompuu](./pentploompuu.jpg)

In Pent's own words:

I learned to program when I was 7 years old and found it fascinating. Started with simple text based toy apps, now working on huge global scale backend systems.

I started using .NET Framework when beta 1 came out in late 2000 and I have since created innumerable applications based on it. I find it to be the most comprehensive and highest quality platform combined with an excellent language.

At one point I was frustrated with System.Decimal performance in an energy metering & billing app, so I started contributing to .NET Core to improve this. Over the years I have learned a lot from reading & contributing to the codebase, and the contribution workflow is great (compared to most smaller/internal repos).

## Targeting .NET 7

To target .NET 7, you need to use a .NET 7 Target Framework Moniker (TFM) in your project file. For example:

```xml
<TargetFramework>net7.0</TargetFramework>
```

The full set of .NET 7 TFMs, including operating-specific ones follows.

* `net7.0`
* `net7.0-android`
* `net7.0-ios`
* `net7.0-maccatalyst`
* `net7.0-macos`
* `net7.0-tvos`
* `net7.0-windows`

We expect that upgrading from .NET 6 to .NET 7 should be straightforward. Please report any breaking changes that you discover in the process of testing existing apps with .NET 7.

## Support

.NET 7 is a **Short Term Support (STS)** release, meaning it will receive free support and patches for 18 months from the release date. It's important to note that the quality of all releases is the same. The only difference is the length of support. For more about .NET support policies, see the [.NET and .NET Core official support policy](https://dotnet.microsoft.com/platform/support/policy/dotnet-core).

We recently recently changed the "Current" name to "Short Term Support (STS)". We're in the [process of rolling out that change](https://github.com/dotnet/core/pull/7517).

## Breaking changes

You can find the most recent list of breaking changes in .NET 7 by reading the [Breaking changes in .NET 7](https://docs.microsoft.com/dotnet/core/compatibility/7.0) document. It lists breaking changes by area and release with links to detailed explanations.

To see what breaking changes are proposed but still under review, follow the [Proposed .NET Breaking Changes GitHub issue](https://github.com/dotnet/core/issues/7131).

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

We appreciate and [thank you](https://dotnet.microsoft.com/thanks) for your all your support and contributions to .NET. Please [give .NET 7 Preview 6 a try](https://dotnet.microsoft.com/download/dotnet/7.0) and tell us what you think!
