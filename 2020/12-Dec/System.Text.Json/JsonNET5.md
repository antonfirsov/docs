---
post_title: What's next for System.Text.Json?
username: laakinri@microsoft.com
microsoft_alias: laakinri
categories: .NET, .NET Core, JSON, Performance
summary: What's next for System.Text.Json?
desired_publication_date: 2020-12-16
---

[.NET 5.0 was released recently](https://devblogs.microsoft.com/dotnet/announcing-net-5-0/) and has come with many new features and performance improvements. `System.Text.Json` is no exception. We have improved performance, reliability, and made it easier to adopt for people who are familiar with `Newtonsoft.Json`. In this post, I'm going to talk about the progress that has been made with System.Text.Json, and what's going to come next.

## Getting the library

- If your project is targeting .NET 5, [install .NET 5](https://dotnet.microsoft.com/download/dotnet/5.0). This gives you `System.Text.Json` right out of the box, without any other pre-requisites.
- If your project is targeting .NET Core 3.x or .NET Framework, or if you're writing a .NET standard compatible library, install the latest [`System.Text.Json` NuGet package](https://www.nuget.org/packages/System.Text.Json/).
- If you're starting a new ASP.NET Core project or upgrading an existing one from ASP.NET Core 2.2 to 3.0 or 5.0, `System.Text.Json` is the default serialization library being used under the covers.

## What is `System.Text.Json`?

`System.Text.Json` is the built-in JSON serialization library in .NET for converting from .NET object types to a JSON string, and vice versa, supporting UTF-8 text encoding. It was first added in .NET Core 3.0. A popular type in the library is `JsonSerializer`, which provides the highest level of functionality for processing JSON data. Here's a simple example of how you use it to serialize and deserialize JSON to and from .NET object types:

```cs
using System;
using System.Text.Json;

MyType obj = new() { Message = "Hello World!" };

string json = JsonSerializer.Serialize(obj);
Console.WriteLine(json); // {"Message":"Hello World!"}

MyType anotherObject = JsonSerializer.Deserialize<MyType>("{\"Message\":\"Hello Again.\"}");
Console.WriteLine(anotherObject.Message); // "Hello Again."

public class MyType
{
    public string Message { get; set; }
}
```

## Why `System.Text.Json`? Looking back at .NET Core 3.x

Processing JSON data has become an essential part of many modern .NET applications, and in many cases, the format has even surpassed the usage of XML. However, .NET didn't have a great built-in way to deal with JSON. Instead, users relied on `Newtonsoft.Json`, which continues to serve the .NET ecosystem well. It became clear that customers would benefit from a JSON library that came as part of the framework. Therefore, we built a new JSON library with the following considerations:

- **Provide high-performance JSON APIs**. We needed a new set of JSON APIs that are highly tuned for performance, leverage modern additions to the framework like `Span<T>`, and can process UTF-8 directly without having to transcode to UTF-16 strings. These aspects are critical to ASP.NET Core, where throughput is a key requirement. We considered contributing changes to `Newtonsoft.Json`, but this was deemed infeasible without either breaking existing `Newtonsoft.Json` users or compromising on the performance we could achieve.

- **Remove the `Newtonsoft.Json` dependency from ASP.NET Core. Prior to .NET Core 3.x ASP.NET Core had a dependency on `Newtonsoft.Json`**. While this provided a tight integration between ASP.NET Core and `Newtonsoft.Json`, it also meant that the version of `Newtonsoft.Json` was dictated by the underlying platform. However, `Newtonsoft.Json` was frequently updated and application developers often needed to a use specific version. Thus, we wanted to remove the `Newtonsoft.Json` dependency from ASP.NET Core 3.0, so that customers could choose which version to use, without fearing they might accidentally break the underlying platform.

Given the ubiquity of `Newtonsoft.Json` in the .NET ecosystem, we did not want to remove it as a dependency from ASP.NET Core without providing an easy way to add it back as the serialization mechanism. The way to do this is the following:

- Install the [`Microsoft.AspNetCore.Mvc.Newtonsoft.Json`](https://www.nuget.org/packages/Microsoft.AspNetCore.Mvc.NewtonsoftJson/) NuGet package.
- Update `Startup.ConfigureServices()` to call `AddNewtonsoftJson()`.

```cs
services.AddMvc()
    .AddNewtonsoftJson();
```

For more information, see [Newtonsoft.Json (Json.NET) support](https://docs.microsoft.com/aspnet/core/migration/22-to-30?view=aspnetcore-5.0&tabs=visual-studio#newtonsoftjson-jsonnet-support).

### What we shipped in .NET Core 3.0

In .NET Core 3.0, we shipped the following types in the [`System.Text.Json` namespace](https://docs.microsoft.com/dotnet/api/system.text.json?view=net-5.0):

- [`JsonSerializer`](https://docs.microsoft.com/dotnet/api/system.text.json.jsonserializer?view=net-5.0): Provides functionality to serialize objects or value types to JSON and to deserialize JSON into objects representations.
- [`JsonDocument`](https://docs.microsoft.com/dotnet/api/system.text.json.jsondocument?view=net-5.0): Provides random access capabilities for examining the structural content of a JSON value without automatically instantiating data values. This type is immutable.
- [`JsonElement`](https://docs.microsoft.com/dotnet/api/system.text.json.jsonelement?view=net-5.0): Represents a specific JSON value within a `JsonDocument`.
- [`Utf8JsonWriter`](https://docs.microsoft.com/dotnet/api/system.text.json.utf8jsonwriter?view=net-5.0): Provides a high-performance API for forward-only, non-cached writing of UTF-8 encoded JSON text.
- [`Utf8JsonReader`](https://docs.microsoft.com/dotnet/api/system.text.json.utf8jsonreader?view=net-5.0): Provides a high-performance API for forward-only, token-by-token processing of UTF-8 encoded JSON text.

`JsonSerializer` provides the highest level of functionality for processing JSON data. Due to the wide feature breadth of implementing a serializer to convert between a persistent data format and .NET object types, `JsonSerializer` is undergoing far more development and receiving more community feedback than the other types in the JSON stack. As a result, I'll mostly cover `JsonSerializer` functionality in this post.

In the [`System.Text.Json.Serialization` namespace](https://docs.microsoft.com/dotnet/api/system.text.json?view=net-5.0), we shipped attributes and APIs for advanced scenarios and customization specific to serialization and deserialization with `JsonSerializer`. Popular among these is the `JsonConverter<T>` type which allows users to control the serialization and deserialization of a specific type, property or field.

In 3.0, we shipped `JsonSerializer` with the following functionality:

- Support for (de)serializing plain old CLR objects (POCOs), primitives, and collections
- Built-in asynchronous serialization and deserialization
- Case-insensitive deserialization (in addition to case-sensitive)
- Camel-case naming policy with `JsonNamingPolicy.CamelCase`
- Specifying custom naming policies with `JsonNamingPolicy`
- Escaped JSON data deserialization
- Optional minimal character escaping on serialization
- Ignoring null values on serialization
- Ignoring comments on deserialization
- Allowing trailing commas
- Custom converters
- Indicate a property overflow bag when deserializing using `[JsonExtensionData]` attribute
- Ignoring individual properties using `[JsonIgnore]` attribute
- Specifying a custom property name using `[JsonProperty]` attribute

See [JSON serialization and deserialization (marshalling and unmarshalling) in .NET](https://docs.microsoft.com/dotnet/standard/serialization/system-text-json-overview) for an overview of the functionality that `System.Text.Json` offers.

### Choosing between `System.Text.Json` and `Newtonsoft.Json`

#### Features

Now, you might be asking yourself, should I migrate from `Newtonsoft.Json` to use `System.Text.Json`? The answer, as you likely suspected is, it depends. In .NET Core 3.0, we made `System.Text.Json` the default serializer for ASP.NET Core because we believe it's good enough for most applications. However, if you have heavily customized the serialization behavior by, for example, using several attributes from `Newtonsoft.Json`, you might find it more difficult to migrate as opposed to, say, someone who mostly uses POCOs. For more details, check out the documentation on [How to migrate from Newtonsoft.Json to System.Text.Json](https://docs.microsoft.com/dotnet/standard/serialization/system-text-json-migrate-from-newtonsoft-how-to).

`System.Text.Json` focuses primarily on performance, security, and standards compliance, given it's the default JSON-processing stack for new .NET applications. It has some key differences in default behavior and doesn't aim to have feature parity with `Newtonsoft.Json`. For some scenarios, `System.Text.Json` has no built-in functionality, but there are recommended workarounds. For other scenarios, workarounds are impractical. If your application depends on a missing feature, consider [filing an issue](https://github.com/dotnet/runtime/issues/new/choose) to find out if support for your scenario can be added.

That's not to say that `System.Text.Json` doesn't have features or flexibility, or that `Newtonsoft.Json` is slow; it's just meant to illustrate the different design philosophies and how we resolve tension when features are in conflict with either performance or flexibility. The goal we have for `System.Text.Json` is to provide a fast built-in JSON stack that balances performance, security, and feature set. Every feature request is carefully weighed against these design principles. This may mean that new projects that align with these principles may more readily use `System.Text.Json`, whereas older projects may encounter more roadblocks during migration. However, there is nothing wrong with `Newtonsoft.Json`. If you're happy with it, you should continue to use it.

Let me provide two examples of features we don't plan to add to `System.Text.Json` for the foreseeable future:

1. Type conversion via `System.ComponentModel.TypeConverter`
2. Tweaking serialization behavior via `System.Runtime.Serialization` attributes

Both of these are considered legacy systems from older serialization stacks, and supporting them would go against the performance-first architecture of `System.Text.Json` due to additional reflection-based lookups on start-up, and the maintenance and size burden of more code in the System.Text.Json.dll. Additionally, any feature that implies going against our principle of standards compliance, such as allowing processing of malformed JSON on deserialization, will not be accepted.

We are designing `System.Text.Json` with extensibility in mind, meaning that even if the above features do not have native support within `JsonSerializer`, they can still be supported from user-provided configuration. For instance, users can write a wrapper around calls to `TypeConverter`s by using a custom `JsonConverter<T>`. We are also considering an `IContractResolver`-like mechanism (from `Newtonsoft.Json`) to allow programmatic control of setting metadata and serialization logic for types and members ([dotnet/runtime #31257](https://github.com/dotnet/runtime/issues/31257)), which would allow custom support for honoring `System.Runtime.Serialization` attributes.

`JsonSerializer` supports asynchronous serialization and deserialization of JSON data as a built-in, performant feature. This has been shown to provide significant value to customers writing highly scalable applications that need to be responsive and non-blocking, over `Newtonsoft.Json` which doesn't have a built-in mechanism to support async JSON processing.

The UTF-8 encoding is the defacto format for information transfer over the wire. `System.Text.Json` APIs natively process data with this encoding and do not need to transcode to and from UTF-16, unlike `Newtonsoft.Json`. Callers also do not need to manually transcode data before passing it as input to the `System.Text.Json` APIs. Avoiding this transcoding also helps yield better performance when processing JSON data. Note that `string`-based (UTF-16) APIs are also available as a convenience but incur the cost of transcoding.

#### Performance

During the .NET Core 3.0 development cycle, we [blogged](https://devblogs.microsoft.com/dotnet/try-the-new-system-text-json-apis/) about how `System.Text.Json` performance compares with `Newtonsoft.Json`:

| Scenario | Speed | Memory |
|---------------------|-----------------|-------------------------------------------------|
| Deserialization | 2x faster | Parity or lower |
| Serialization | 1.5x faster | Parity or lower |
| Document (read-only) | 3-5x faster | ~Allocation free for sizes < 1 MB |
| Reader | 2-3x faster | ~Allocation free (until you materialize values) |
| Writer | 1.3-1.6x faster | ~Allocation free |

As it is with any performance benchmarks, scenarios vary, and summarizing numbers is nuanced. The summary of that post read:

> The primary goal was performance and we see typical speedups of up to 2x over Json.NET, but it depends on your scenario and your payload, so make sure you measure what's important to you.

Some people took this to mean that `System.Text.Json` is always twice as fast, which wasn't the intended takeaway. The "up to" and "depends on your scenario and your payload" statements are key in that sentence.

There have been multiple performance comparisons between `JsonSerializer` and other serializers in the .NET ecosystem. You may have noticed that `JsonSerializer` is not always the fastest. You might wonder why that is when we have said that performance is a primary goal. In software, almost no capability can be maximized without hurting other capabilities, so all functionality is a trade-off between various aspects. And while performance of JSON serialization is very important to us, it's not the only goal we have. Other goals include security, reliability, standards compliance, and usability.

Here are a few examples of some trade-offs we made against better performance:

- **Using classes in some cases**. We made the decision to turn `Utf8JsonWriter` into a class. It used to be a `ref struct` which allowed for fewer allocations and faster access to the underlying span we're writing to. We changed it to a class to avoid problems where developers accidentally create copies of the writer (for example, when passing it to a helper method by value) and subsequent writes end up overwriting data in the buffer.

- **Validation**. We made the decision to validate JSON data when it's being processed, which includes both UTF-8 validation as well as JSON syntax validation. This is necessary to enable server and cloud scenarios where the JSON input is often coming from untrusted sources.

- **Extension points**. The serializer has the ability to register custom converters. These indirections, while relatively cheap, are costly when running in a tight loop; sometimes even when they aren't used (due to lookups).

## What's new in .NET 5.0?

### Features

In .NET 5.0, we have implemented some of the most sought after features from the community and made it easier to adopt for people who are familiar with `Newtonsoft.Json`. Here is an overview of the features that were added:

| GitHub Issue | Description |
|--------|-------------------------------------------------------------------------------|
| [#30820](https://github.com/dotnet/runtime/issues/30820) | Add mechanism to preserve object references when (de)serializing |
| [#32937](https://github.com/dotnet/runtime/issues/32937) | Add extension methods for `HttpClient` and `HttpContent` that allow (de)serializing JSON |
| [#30255](https://github.com/dotnet/runtime/issues/30255) | Support (de)serializing quoted numbers |
| [#29895](https://github.com/dotnet/runtime/issues/29895) | Support deserializing objects using parameterized constructors |
| [#876](https://github.com/dotnet/runtime/issues/876) | Support (de)serializing fields |
| [#779](https://github.com/dotnet/runtime/issues/779) | Support ignoring value-type defaults |
| [#30687](https://github.com/dotnet/runtime/issues/30687) | Support conditionally ignoring properties (always, never, when null/default) |
| [#30524](https://github.com/dotnet/runtime/issues/30524) | Support non-string dictionary keys |
| [#29743](https://github.com/dotnet/runtime/issues/29743) | Allow using non-public property accessors for (de)serialization |
| [#34439](https://github.com/dotnet/runtime/issues/34439) | Provide opt-in for custom converters to handle null |
| [#38539](https://github.com/dotnet/runtime/issues/38539) | Support new C# record types |
| [#30445](https://github.com/dotnet/runtime/issues/30445) | Add a copy constructor to JsonSerializerOptions |
| [#34626](https://github.com/dotnet/runtime/issues/34626) | Add constructor to JsonSerializerOptions that takes serialization defaults |
| [#31326](https://github.com/dotnet/runtime/issues/31326) | Enable JsonSerializer to work on Xamarin iOS/Android |

When we announced [.NET 5.0](https://devblogs.microsoft.com/dotnet/announcing-net-5-0/#system-text-json) and [.NET 5.0 RC 1](https://devblogs.microsoft.com/dotnet/announcing-net-5-0-rc-1/#system-text-json), we went over some of the new features, including improved support for immutable types and preserving references in JSON object graphs. In this section, I will go over a few more.

#### Support for quoted numbers

When .NET Core 3.0 shipped, deserializing number types encoded as JSON strings was not supported by default. The workaround was to add a custom converter for each applicable number type that would take control of the serialization and deserialization of the type, and include logic to handle quoted numbers. In .NET 5, we have added a convenient, opt-in feature to support serializing and deserializing quoted numbers, and named floating point literals (`NaN`, `Infinity`, and `-Infinity`). Here is an example that uses this feature:

```cs
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

var options = new JsonSerializerOptions
{
    NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString
};

string json = @"{""NumberOne"":1,""NumberTwo"":""2""}";

ClassWithInts @class = JsonSerializer.Deserialize<ClassWithInts>(json, options);
Console.WriteLine(@class.NumberOne); // 1
Console.WriteLine(@class.NumberTwo); // 2

json = JsonSerializer.Serialize(@class, options);
Console.WriteLine(json); // @"{""Number1"":""1"",""Number2"":""2""}";

public class ClassWithInts
{
    public int NumberOne { get; set; }
    public int NumberTwo { get; set; }
}
```

In addition to the globally applied [`JsonSerializerOptions.NumberHandling`](https://docs.microsoft.com/dotnet/api/system.text.json.jsonserializeroptions.numberhandling?view=net-5.0) option, we have also added [`JsonNumberHandlingAttribute`](https://docs.microsoft.com/dotnet/api/system.text.json.serialization.jsonnumberhandlingattribute?view=net-5.0) which enables specifying number handling settings for a type, property, or field.

#### Extended support for ignoring `default` values

When serializing in .NET Core 3.x, it was only possible to ignore `null` values for reference or nullable value-typed properties, and the [`JsonSerializerOptions.IgnoreNullValues`](https://docs.microsoft.com/dotnet/api/system.text.json.jsonserializeroptions.ignorenullvalues?view=net-5.0) setting was only applicable across an entire input object graph. In .NET 5, we added support to ignore `default` values for non-nullable values, such as `int` and `float`. Furthermore, it is now possible to cherry-pick properties and fields to be ignored when `default`. See the following example:

```cs
var options = new JsonSerializerOptions
{
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
};

var obj = new MyClass();
string json = JsonSerializer.Serialize(obj, options);
Console.WriteLine(json); // {"MyBool":false}

public class MyClass
{
    public int MyInt { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public bool MyBool { get; set; }

    public string MyString { get; set; }
}
```

Here we see a combination of the new features: we tell the serializer to ignore `default` values across the entire object graph, but indicate that one of the properties should never be ignored, regardless of the global option. To only ignore `null` values but not `default` values for value types, use `JsonIgnoreCondition.WhenWritingNull`.

With these augmentations to handling `default` values, the `JsonSerializerOptions.IgnoreNullValues` property has been made obsolete in the upcoming .NET 6.0 release. `IgnoreNullValues` is honored when deserializing as well as when serializing. This is not desired, as there is no good reason for ignoring `null` tokens in input payloads.

#### Utility constructors for `JsonSerializerOptions`

Copying `JsonSerializerOptions` settings from one instance to another, then making a few mutations, is a common operation. In .NET 5, we added a copy constructor to `JsonSerializerOptions` which makes this easier:

```cs
JsonSerializerOptions options = new()
{
    NumberHandling = JsonNumberHandling.AllowReadingFromString,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    Converters = { new JsonStringEnumConverter() }
};

JsonSerializerOptions newOptions = new(options)
{
    NumberHandling = JsonNumberHandling.Strict
};

Console.WriteLine(newOptions.NumberHandling); // Strict
Console.WriteLine(newOptions.DefaultIgnoreCondition); // WhenWritingNull
Console.WriteLine(newOptions.Converters[0]); // System.Text.Json.Serialization.JsonStringEnumConverter
```

A common set of options when processing JSON data in web contexts is to use the `camelCase` naming policy, specify case-insensitive matching when deserializing, and allow reading quoted numbers. The new [`JsonSerializerDefaults.Web`](https://docs.microsoft.com/dotnet/api/system.text.json.jsonserializerdefaults?view=net-5.0#fields) enum value, along with a new constructor that takes a `JsonSerializerDefaults` value, provides an easy way to reuse these options in a consistent way across multiple layers of an application, (e.g. client, shared, and server for Blazor scenarios). Let's take a look:

```cs
JsonSerializerOptions options = new(JsonSerializerDefaults.Web);
Console.WriteLine(options.PropertyNamingPolicy); // System.Text.Json.JsonCamelCaseNamingPolicy
Console.WriteLine(options.PropertyNameCaseInsensitive); // True
Console.WriteLine(options.NumberHandling); // AllowReadingFromString
```

For more information on what features `JsonSerializer` supports, check out the [migration guide](https://docs.microsoft.com/dotnet/standard/serialization/system-text-json-migrate-from-newtonsoft-how-to#table-of-differences-between-newtonsoftjson-and-systemtextjson).

![Migration Guide](migration-guide.png)

### Performance

Between .NET Core 3.1 and .NET 5 we have improved performance in the following areas:

- Improved serialization and deserialization performance for collections
- Improved serialization and deserialization performance for small types
- Improved deserialization performance for case-insensitive and missing-property cases
- Improved serialization performance for long JSON strings

These improvements are going to be especially meaningful for high-performance apps.

#### Improved performance for collections

We made significant improvements for large collections (~1.15x-1.5x on deserialize, ~1.5x-2.4x on serialize). You can see these improvements characterized in much more detail at [dotnet/runtime #2259](https://github.com/dotnet/runtime/pull/2259). The following numbers show performance numbers for processing collections with 1,024 elements.

**`Dictionary<string, string>`**

| Method | Mean | Error | StdDev | Median | Min | Max | Gen 0 | Gen 1 | Gen 2 | Allocated |
|---------------|---------:|--------:|--------:|---------:|---------:|---------:|--------:|-------:|------:|----------:|
| Deserialize Before | 190.4 us | 1.47 us | 1.38 us | 190.6 us | 188.5 us | 193.6 us | 26.5554 | 8.3460 | - | 163.69 KB |
| After **~1.2x faster** | 158.8 us | 1.27 us | 1.13 us | 158.8 us | 157.3 us | 161.3 us | 26.5991 | 8.8664 | - | 164.05 KB |
| Serialize Before | 109.7 us | 0.77 us | 0.72 us | 109.5 us | 108.5 us | 111.1 us | 3.4904 | - | - | 23.92 KB |
| After **~1.5x faster** | 74.53 us | 0.590 us | 0.552 us | 74.40 us | 73.57 us | 75.69 us | 3.8179 | 0.2937 | - | 24.17 KB |

**`List<int>`**

| Method | Mean | Error | StdDev | Median | Min | Max | Gen 0 | Gen 1 | Gen 2 | Allocated |
|--------------- |---------:|---------:|---------:|---------:|---------:|---------:|-------:|------:|------:|----------:|
Deserialize Before | 76.40 us | 0.392 us | 0.366 us | 76.37 us | 75.53 us | 76.87 us | 1.2169 | - | - | 8.25 KB |
| After **~1.5x faster** | 50.05 us | 0.251 us | 0.235 us | 49.94 us | 49.76 us | 50.43 us | 1.3922 | - | - | 8.62 KB |
| Serialize Before | 29.04 us | 0.213 us | 0.189 us | 29.00 us | 28.70 us | 29.34 us | 1.2620 | - | - | 8.07 KB |
| After **~2.4x faster** | 12.17 us | 0.205 us | 0.191 us | 12.15 us | 11.97 us | 12.55 us | 1.3187 | - | - | 8.34 KB |

#### Improved performance for small types (TechEmpower benchmarks)

In .NET 5.0, there was a significant effort to improve .NET performance in the [TechEmpower](https://www.techempower.com/benchmarks/) JSON benchmark. This work spanned across multiple areas, including the networking stack, Kestrel, and `JsonSerializer` itself. As a result, performance when observing work done within the serializer is now ~19% better.

These changes, and the performance measurements are covered in detail at [dotnet/runtime #37976](https://github.com/dotnet/runtime/pull/37976). There are two sets of benchmarks there. The first is validating performance with the [`JsonSerializer` performance benchmarks](https://github.com/dotnet/performance/tree/master/src/benchmarks/micro/libraries/System.Text.Json/Serializer) that the team maintains. There is an ~8% improvement observed. The next section is for TechEmpower. It measures three different approaches for satisfying the requirements of the TechEmpower JSON benchmark. `SerializeWithCachedBufferAndWriter` is the one we use in the [official benchmark](https://github.com/aspnet/Benchmarks/blob/4e58073ac06642b34c25119c73ef22932c1ece64/src/BenchmarksApps/Kestrel/PlatformBenchmarks/BenchmarkApplication.Json.cs#L29-L33).

| Method | Mean | Error | StdDev | Median | Min | Max | Gen 0 | Gen 1 | Gen 2 | Allocated |
|-----------------------------------|---------:|--------:|--------:|---------:|---------:|---------:|-------:|------:|------:|----------:|
| SerializeWithCachedBufferAndWriter (Before)| 155.3 ns | 1.19 ns | 1.11 ns | 155.5 ns | 153.3 ns | 157.3 ns | 0.0038 | - | - | 24 B |
| SerializeWithCachedBufferAndWriter (After) **~1.19x faster** | 130.8 ns | 1.50 ns | 1.40 ns | 130.9 ns | 128.6 ns | 133.0 ns | 0.0037 | - | - | 24 B |

All the work done across the stack will improve the placement of .NET on the JSON TechEmpower benchmark once we update our entries to .NET 5.0. We can compare the performance of the `aspcore` entry of the benchmark in .NET 3.1 ([Round 19, 2020-05-28](https://www.techempower.com/benchmarks/#section=data-r19&hw=ph&test=json)) to .NET 5.0 ([Unofficial continuous run, 2020-12-14](https://www.techempower.com/benchmarks/#section=test&runid=996d0239-a18a-4054-b496-3506868f0723&hw=ph&test=json&a=2)):

**Before**

![TechEmpower Before](techempower-before.png)

**After**

![TechEmpower After](techempower-after.png)

We can see that the responses per second (RPS) for JSON serialization increased from 904,846 to 1,190,245, which is a **~31% improvement**.

#### Improved deserialization performance for case-insensitive and extra-property cases

One of the most common issues with using JSON is a mismatch of [naming conventions](https://en.wikipedia.org/wiki/Naming_convention_(programming)#Letter_case-separated_words) with .NET design guidelines. JSON properties are often [camelCase](https://en.wikipedia.org/wiki/Camel_case) and .NET properties and fields are typically PascalCase. The JSON serializer you use is responsible for bridging between naming conventions. That doesn't come for free, at least not with .NET Core 3.1. That cost is now negligible with .NET 5.0.

The code that allows for case insensitivity and extra properties has been greatly improved in .NET 5.0. It is [**~1.75x faster** in some cases](https://github.com/dotnet/runtime/pull/35848).

The following benchmarks are for a simple 4-property test class that has property names which have more than 7 characters.

**3.1 performance**

| Method | Mean | Error | StdDev | Median | Min | Max | Gen 0 | Gen 1 | Gen 2 | Allocated |
|---------------------------------- |-----------:|--------:|--------:|-----------:|-----------:|-----------:|-------:|------:|------:|----------:|
| CaseSensitive_Matching | 844.2 ns | 4.25 ns | 3.55 ns | 844.2 ns | 838.6 ns | 850.6 ns | 0.0342 | - | - | 224 B |
| CaseInsensitive_Matching | 833.3 ns | 3.84 ns | 3.40 ns | 832.6 ns | 829.4 ns | 841.1 ns | 0.0504 | - | - | 328 B |
| CaseSensitive_NotMatching (Extra)| 1,007.7 ns | 9.40 ns | 8.79 ns | 1,005.1 ns | 997.3 ns | 1,023.3 ns | 0.0722 | - | - | 464 B |
| CaseInsensitive_NotMatching | 1,405.6 ns | 8.35 ns | 7.40 ns | 1,405.1 ns | 1,397.1 ns | 1,423.6 ns | 0.0626 | - | - | 408 B |

**5.0 performance**

| Method | Mean | Error | StdDev | Median | Min | Max | Gen 0 | Gen 1 | Gen 2 | Allocated |
|----------------------------------|---------:|--------:|--------:|---------:|---------:|---------:|-------:|------:|------:|----------:|
| CaseSensitive_Matching | 799.2 ns | 4.59 ns | 4.29 ns | 801.0 ns | 790.5 ns | 803.9 ns | 0.0985 | - | - | 632 B |
| CaseInsensitive_Matching | 789.2 ns | 6.62 ns | 5.53 ns | 790.3 ns | 776.0 ns | 794.4 ns | 0.1004 | - | - | 632 B |
| CaseSensitive_NotMatching (Extra)| 479.9 ns | 0.75 ns | 0.59 ns | 479.8 ns | 479.1 ns | 481.0 ns | 0.0059 | - | - | 40 B |
| CaseInsensitive_NotMatching | 783.5 ns | 3.26 ns | 2.89 ns | 783.5 ns | 779.0 ns | 789.2 ns | 0.1004 | - | - | 632 B |

#### Improve serialization performance for long JSON strings

[dotnet/corefx #41845](https://github.com/dotnet/corefx/pull/41845) utilized SSE2 intrinsics to make the security check to see if a JSON string needs escaping, faster. When serializing commonly found payloads, there is a **~10-20% improvement**. Also, when writing relatively large JSON strings using the writer directly, there is a **~30% improvement**. The performance characteristics of this change are discussed in greater detail in this [GitHub gist](https://gist.github.com/ahsonkhan/c566f5e7d65c1fde5a83a67be290c4ee). One interesting data point is a benchmark that shows improvements when serializing an instance of a POCO representing a NuGet search result:

**Before:**

| Method | Mean | Error | StdDev | Median | Min | Max | Gen 0 | Gen 1 | Gen 2 | Allocated |
|----------------------------------|---------:|---------:|---------:|---------:|---------:|---------:|------:|------:|------:|----------:|
| SerializeNugetPayload (Before) | 791.7 ms | 15.69 ms | 16.12 ms | 787.4 ms | 772.0 ms | 827.1 ms | - | - | - | 787.3 MB |
| SerializeNugetPayload (After) **~1.13x faster** | 698.4 ms | 6.63 ms | 5.53 ms | 699.5 ms | 690.9 ms | 708.4 ms | - | - | - | 787.3 MB |

### Breaking changes

We made a few behavioral breaking changes between .NET Core 3.x and .NET 5.0. Given that `System.Text.Json` is a platform component and adheres to a strict compatibility standard like the rest of .NET, these changes were made based on careful evaluation of their impacts, and serve to improve the library as a whole. Though important, these changes generally address edge cases, especially around consistency, and should have minimal impact to the vast majority of folks using the library. For more significant behavioral changes and new feature additions, we make them opt-in to avoid breaking existing code written against previous versions of the library. See the linked documents for more information on each breaking change.

- [Deserialization of `char` types requires a single-character string](https://docs.microsoft.com/dotnet/core/compatibility/serialization/5.0/deserializing-json-into-char-requires-single-character)
- [ASP.NET Core apps allow deserializing quoted numbers](https://docs.microsoft.com/dotnet/core/compatibility/serialization/5.0/jsonserializer-allows-reading-numbers-as-strings)
- [`JsonSerializer.Serialize` throws `ArgumentNullException` when `Type` parameter is null](https://docs.microsoft.com/dotnet/core/compatibility/serialization/5.0/jsonserializer-serialize-throws-argumentnullexception-for-null-type)
- [Non-`public`, parameterless constructors not used for deserialization](https://docs.microsoft.com/dotnet/core/compatibility/serialization/5.0/non-public-parameterless-constructors-not-used-for-deserialization)
- [PropertyNamingPolicy, PropertyNameCaseInsensitive, and Encoder options are honored when serializing and deserializing `KeyValuePair<TKey,TValue>`](https://docs.microsoft.com/dotnet/core/compatibility/serialization/5.0/options-honored-when-serializing-key-value-pairs)

## What's next for System.Text.Json?

We have started planning for .NET 6.0. We'll continue to address the most requested features that help drive `System.Text.Json` to be a viable choice for a JSON stack in more .NET applications, weighing each request against our design principles. See [dotnet/runtime 43620](https://github.com/dotnet/runtime/issues/43620) for an overview of what is being proposed. Here is a sneak peak of some of the top features.

### C# source generator for JSON serialization ([dotnet/runtime #1568](https://github.com/dotnet/runtime/issues/1568))

One area of investment is utilizing the new [C# source generators feature](https://devblogs.microsoft.com/dotnet/new-c-source-generator-samples/) to generate code that can help the serializer in the following areas:
- Improved start up perf
- Improved run-time throughput
- Reduced private bytes usage
- ILLinker friendliness due to avoiding runtime reflection
- Reduced application size: facilitating linker removal of unused reflection-based code-paths of the serializer and unused converters.

This effort is underway and is in a prototype stage. Action items and progress of this effort can be observed through the [JSON Code Gen project board](https://github.com/dotnet/runtimelab/projects/1) in dotnet/runtimelab.

The updated `System.Text.Json` with support for source generation can be consumed via an experimental [NuGet package](https://dev.azure.com/dnceng/public/_packaging?_a=package&feed=dotnet-experimental&view=versions&package=System.Text.Json&protocolType=NuGet). Please share with us any performance changes you observe when using this feature. Issues can be logged at https://github.com/dotnet/runtimelab/issues/new?labels=area-JsonCodeGen with the `area-JsonCodeGen` label.

### Extended polymorphic serialization and deserialization ([dotnet/runtime #45189](https://github.com/dotnet/runtime/issues/45189))

Polymorphic serialization and deserialization remain important scenarios for modern .NET applications. We expect feature work to happen in .NET 6.0 to enable these scenarios.

### `dynamic` and a mutable JSON DOM ([dotnet/runtime #45188](https://github.com/dotnet/runtime/issues/45188))

Enabling serializing and deserializing `dynamic` types and providing a mutable JSON document are two closely related and important features that will unblock many customers. We are tracking these as potential work items for .NET 6.0.

### Miscellaneous improvements ([dotnet/runtime #45190](https://github.com/dotnet/runtime/issues/45190))

There are multiple miscellaneous features and improvements that we would like to make to make in .NET 6.0. Some features would be innovative and unique to `System.Text.Json`, such as a feature to support asynchronously serializing and deserializing `IAsyncEnumerable<T>` instances ([dotnet/runtime #1570](https://github.com/dotnet/runtime/issues/1570)). Other features would be more familiar, such as adding snake_case support ([dotnet/runtime #782](https://github.com/dotnet/runtime/issues/782)), and being able to change the default `JsonSerializerOptions` settings ([dotnet/runtime #31094](https://github.com/dotnet/runtime/issues/31094)). Implementing these features would increase the number of apps for which `System.Text.Json` is a good fit.

## Closing

.NET 5.0 was a big release for System.Text.Json. If your project is not targeting .NET 5.0, you can still take advantage of the new improvements by installing the latest [`System.Text.Json` NuGet package](https://www.nuget.org/packages/System.Text.Json/).

A lot of the work we did in .NET 5.0 was driven by the community. [@YohDeadfall](https://github.com/YohDeadfall) contributed [field support](https://github.com/dotnet/runtime/pull/36986) and various [optimizations](https://github.com/dotnet/runtime/pulls?q=is%3Apr+author%3AYohDeadfall+is%3Aclosed+label%3Aarea-System.Text.Json) to `JsonSerializer`. [@devsko](https://github.com/devsko) contributed [fixes](https://github.com/dotnet/runtime/pulls?q=is%3Apr+is%3Aclosed+label%3Aarea-System.Text.Json+author%3Adevsko) to various issues with new .NET 5.0 improvements before we shipped. [@CodeBlanch](https://github.com/CodeBlanch) contributed [fixes](https://github.com/dotnet/runtime/pulls?q=is%3Apr+author%3ACodeBlanch+is%3Aclosed+label%3Aarea-System.Text.Json) for issues with `null` and nullability. [@Marusyk](https://github.com/Marusyk) contributed a [fix](https://github.com/dotnet/runtime/pull/31756) for a bug with reference equality for the new preserve-references feature. [@khellang](https://github.com/khellang) contributed a [fix](https://github.com/dotnet/runtime/pull/478) for a bug with the validation of `DateTime` and `DateTimeOffset` payloads when reading. [@alanisaac](https://github.com/alanisaac), [@thomaslevesque](https://github.com/thomaslevesque), [@marcusturewicz](https://github.com/marcusturewicz), [@madmir](https://github.com/madmir), [@NikiforovAll](https://github.com/NikiforovAll), [@JoshSchreuder](https://github.com/JoshSchreuder), [@Jacksondr5](https://github.com/Jacksondr5), and [@KimKiHyuk](https://github.com/KimKiHyuk) contributed changes to [improve test coverage for System.Text.Json](https://github.com/dotnet/runtime/issues/32341) in the continuous effort to get it close to 100%. We can't highlight all the contributions here, but the [.NET Thanks](https://dotnetthanks.azurewebsites.net/) page gives kudos to all contributors to the .NET runtime.

In .NET 6.0, we're continuing to make more improvements. Contributions to `System.Text.Json` are very welcome as we make progress. Simply visit this [GitHub query](https://github.com/dotnet/runtime/issues?q=is%3Aopen+is%3Aissue+label%3Aarea-System.Text.Json+label%3Aup-for-grabs) to find issues labeled with `area-System.Text.Json` and `up-for-grabs`. If you are feeling bold, check out the issues without the `up-for-grabs` label. As always, feedback is very welcome, especially now as we are currently planning for the next release.
