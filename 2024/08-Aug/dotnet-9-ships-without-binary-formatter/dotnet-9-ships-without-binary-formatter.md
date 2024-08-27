---
post_title: BinaryFormatter removed from .NET 9
author1: terrajobst@web.de
post_slug: binaryformatter-removed-from-dotnet-9
microsoft_alias: immol
featured_image: binaryformatterdotnet9.jpg
categories: .NET, .NET Core, Security
ai_note: hide
summary: Starting with .NET 9, we no longer include an implementation of BinaryFormatter in the runtime. This post covers what options you have to move forward.
post_date: 2024-08-28 10:05:00
---

Starting with .NET 9, we no longer include an implementation of
`BinaryFormatter` in the runtime (.NET Framework remains unchanged). The APIs
are still present, but their implementation always throws an exception,
regardless of project type. Hence, setting the existing backwards compatibility
flag is no longer sufficient to use `BinaryFormatter`.

In this blog post, I'll explain why this change was made and what options you
have to move forward.

## TL;DR: What should I do?

You have two options to address the removal of `BinaryFormatter`'s
implementation:

1. **Migrate away from BinaryFormatter**. We strongly recommend that you
   investigate options to stop using `BinaryFormatter` due to the associated
   security risks. The [BinaryFormatter migration guide][migration-guide] lists
   several options.

2. **Keep using BinaryFormatter**. If you need to continue using
   `BinaryFormatter` in .NET 9, you need to depend on the unsupported
   [System.Runtime.Serialization.Formatters][compat-pack] NuGet package,
   which restores the unsafe legacy functionality and replaces the throwing implementation.

> [!NOTE]
> Please note that .NET Framework is unaffected by this change and continues to
> include an implementation of `BinaryFormatter`. However, we still strongly
> recommend to stop using `BinaryFormatter` from .NET Framework too, for the
> same reasons.

## What's the risk in using BinaryFormatter?

Any deserializer, binary or text, that allows its input to carry information
about the objects to be created is a security problem waiting to happen. There
is a common weakness enumeration (CWE) that describes the issue: [CWE-502
"Deserialization of Untrusted Data"][CWE502]. `BinaryFormatter`, included in the
the initial release of .NET Framework in 2002, is such a deserializer. We also
cover this in the [BinaryFormatter security guide][security-guide].

## Why we removed BinaryFormatter

We strongly believe that .NET should make it easy for customers to do the right
thing and hard, if not impossible, to do the wrong thing. We generally refer to
this as the "pit of success".

Shipping a technology that is widely regarded as unsafe is counter to this goal.
At the same time, we also have a responsibility to ensure customers can support
and move their existing code forward. We can't just remove widely used
components from a .NET release, even when communicated long in advance. We also
need a migration plan and stop gap options.

This removal was not a sudden change. Due to the known risks of using
`BinaryFormatter`, we excluded it from .NET Core 1.0. But without a clear
migration path to using something safer, customer demand led to
`BinaryFormatter` being included in .NET Core 2.0.

Since then, we have been on the path to removing `BinaryFormatter`, slowly
turning it off by default in multiple project types but letting consumers opt-in
via flags if still needed for backward compatibility:

* [2020: BinaryFormatter Obsoletion Plan](https://github.com/dotnet/designs/blob/main/accepted/2020/better-obsoletion/binaryformatter-obsoletion.md)
* [2023: .NET 8 Update](https://github.com/dotnet/announcements/issues/284): Implementation throws by default
* [2024: .NET 9 Update](https://github.com/dotnet/announcements/issues/293): Announced intention of removal early in the release cycle
* [2024: .NET 9 Update](https://github.com/dotnet/announcements/issues/317): Removal completed
* [2024: .NET 9 Breaking Change](https://learn.microsoft.com/dotnet/core/compatibility/serialization/9.0/binaryformatter-removal): In-box BinaryFormatter implementation removed and always throws

In .NET 9 we removed all remaining in-box dependencies on `BinaryFormatter` and
replaced the implementation with one that always throws.

## Options to move forward

New code should not take a dependency on `BinaryFormatter`. For existing code,
you should first investigate alternatives to `BinaryFormatter`. In case you
don't control the serializer but only perform deserialization, you can consider
only reading the `BinaryFormatter` payload, without performing any
deserialization. And if none of this works for you can bring the implementation
back by depending on an (unsupported) [compatibility package][compat-pack].

I'll explore these options in more detail below.

### Migrate-Away

You should first investigate whether you can replace `BinaryFormatter` with
another serializer. We have four recommendations:

* **Text-based**. If a binary serialization format is not a requirement, you can
  consider using JSON or XML serialization formats. These serializers are
  included in .NET and are supported by us. 
    - [JSON using System.Text.Json][migrate-json]
    - [XML using System.Runtime.Serialization.DataContractSerializer][migrate-dcs]

* **Binary** If a compact binary representation is important for your scenarios,
  the following serialization formats and open-source serializers are
  recommended: 
    - [MessagePack using MessagePack for C#][migrate-messagepack]
    - [Protocol Buffers using protobuf-net][migrate-probuf]

Since `DataContractSerializer` honors the same attribute and interface as
`BinaryFormatter` (namely `[Serializable]` and `ISerializable`), it's probably
the easiest one to migrate to. If your migration goals include adopting a
modern and performant serializer or a format with better cross-platform
interoperability, the other options should be considered.

### Read BinaryFormatter Payloads

If your code doesn't control the serialization but only the deserialization (for
use the new [`NrbfDecoder`][nrbf-decoder] to [read BinaryFormatter
payloads][read-nrbf]. This allows you to read the encoded data without any
deserialization. It's the equivalent of using a JSON/XML reader without the
deserializer:

```C#
using System.Formats.Nrbf;

void Read(Stream payload)
{
    SerializationRecord rootObject = NrbfDecoder.Decode(payload);

    if (rootObject is PrimitiveTypeRecord primitiveRecord)
    {
        Console.WriteLine($"It was a primitive value: '{primitiveRecord.Value}'");
    }
    else if (rootObject is ClassRecord classRecord)
    {
        Console.WriteLine($"It was a class record of '{classRecord.TypeName.AssemblyQualifiedName}' type name.");
    }
    else if (rootObject is SZArrayRecord<byte> arrayOfBytes)
    {
        Console.WriteLine($"It was an array of `{arrayOfBytes.Length}`-many bytes.");
    }
}
```

For more details, check out the [Nrbf documentation][read-nrbf].

> [!NOTE]
> At the time .NET 9 ships, the `NrbfDecoder` will likely still be in
> preview.

### BinaryFormatter compatibility package

If you have explored the options and determined you can't migrate away from
`BinaryFormatter`, you can also install the unsupported
[System.Runtime.Serialization.Formatters][compat-pack] NuGet package and set the
compatibility switch to true:

```XML
<PropertyGroup>
  <TargetFramework>net9.0</TargetFramework>
  <EnableUnsafeBinaryFormatterSerialization>true</EnableUnsafeBinaryFormatterSerialization>
</PropertyGroup>

<ItemGroup>
  <PackageReference Include="System.Runtime.Serialization.Formatters" Version="9.0.0" />
</ItemGroup>
```

The package replaces the in-box implementation of `BinaryFormatter` with a
functioning one, including its vulnerabilities and risks. It's meant as a
stopgap if you can't wait with migrating to .NET 9 while not having replaced the
usages of `BinaryFormatter` yet.

Since the `BinaryFormatter` API still exists and this package only replaces the
in-box implementation you only need to reference it from application projects.
Existing code that is compiled against `BinaryFormatter` will continue to work.

> [!CAUTION]
> The compatibility package is not supported and unsafe. We strongly recommend
> against taking a dependency on this package and to instead migrate away from
> `BinaryFormatter`.

## Summary

Since the start of .NET Core we have been on a path of deprecating
`BinaryFormatter`, due to [its security risks][security-guide].

Starting with .NET 9, we no longer ship an implementation with the runtime. We
recommend that you [migrate away from BinaryFormatter][migration-guide]. If that
doesn't work for you can either start [reading the binary payloads without
deserializing][read-nrbf] or you can take a dependency on the [unsupported
compatibility package][compat-pack].

[compat-pack]: https://learn.microsoft.com/dotnet/standard/serialization/binaryformatter-migration-guide/compatibility-package
[migration-guide]: https://learn.microsoft.com/dotnet/standard/serialization/binaryformatter-migration-guide/
[security-guide]: https://learn.microsoft.com/dotnet/standard/serialization/binaryformatter-security-guide
[CWE502]: https://cwe.mitre.org/data/definitions/502.html

[migrate-json]: https://learn.microsoft.com/dotnet/standard/serialization/binaryformatter-migration-guide/migrate-to-system-text-json
[migrate-dcs]: https://learn.microsoft.com/dotnet/standard/serialization/binaryformatter-migration-guide/migrate-to-datacontractserializer
[migrate-messagepack]: https://learn.microsoft.com/dotnet/standard/serialization/binaryformatter-migration-guide/migrate-to-messagepack
[migrate-probuf]: https://learn.microsoft.com/dotnet/standard/serialization/binaryformatter-migration-guide/migrate-to-protobuf-net
[read-nrbf]: https://learn.microsoft.com/dotnet/standard/serialization/binaryformatter-migration-guide/read-nrbf-payloads
[nrbf-decoder]: https://learn.microsoft.com/dotnet/api/system.formats.nrbf.nrbfdecoder