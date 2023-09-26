---
post_title: What's new in System.Text.Json in .NET 8
summary: An overview of all new .NET 8 features in System.Text.Json for developers.
featured_image: systemtextjsondotnet8.jpg
post_slug: system-text-json-in-dotnet-8
author1: eitsarpa
microsoft_alias: eitsarpa
categories: .NET, JSON
tags: system.text.json, .net 8
desired_publication_date: 2023-09-19
post_date: 2023-09-19 08:15:00
---

There are a lot of exciting updates for developers in System.Text.Json in .NET 8. In this release, we have substantially improved the user experience when using the library in Native AOT applications, as well as delivering a number of highly requested features and reliability enhancements. These include support for [populating read-only members](https://learn.microsoft.com/dotnet/core/whats-new/dotnet-8#read-only-properties), customizable [unmapped member handling](https://learn.microsoft.com/dotnet/standard/serialization/system-text-json/missing-members), support for [interface hierarchies](https://learn.microsoft.com/dotnet/core/whats-new/dotnet-8#interface-hierarchies), [snake case and kebab case naming policies](https://learn.microsoft.com/dotnet/core/whats-new/dotnet-8#naming-policies) and much more.

### Getting the latest bits

You can try out the new features by referencing the latest build of [System.Text.Json NuGet package](https://www.nuget.org/packages/System.Text.Json) or the [latest SDK for .NET 8](https://dotnet.microsoft.com/download/dotnet/8.0), which is currently RC 1.

## Source generator improvements

This release marks an important milestone in our investment to ensure compatibility with .NET libraries being used in Native AOT applications. System.Text.Json has evolved over the years to use reflection and runtime code generation for serialization and then in .NET 6 the option to use source generators that are compiled into your application. There have always been trade offs between these two options and in this release we have shortened the functional gap between these two options to ensure that ASP.NET Core and other apps have first-class support for source-generated JSON serialization.

### `required` and `init` member support

[Required members](https://learn.microsoft.com/dotnet/csharp/language-reference/keywords/required) and [init-only properties](https://learn.microsoft.com/dotnet/csharp/language-reference/proposals/csharp-9.0/init) are relatively recent C# language features that control how fields or properties are meant to be initialized. Even though these have been well-supported by the reflection serializer (the modifiers are compile-time guards that can be bypassed by reflection), this has not been the case with the source generator where referencing `required` or `init` members could result in generated code with compiler errors.

Starting in .NET 8, full support for `required` and `init` members has been added:

```csharp
JsonSerializer.Deserialize("""{"Real" : 0, "Im" : 1 }""", MyContext.Default.Complex);

public record Complex
{
    public required double Real { get; init; }
    public required double Im { get; init; }
}

[JsonSerializable(typeof(Complex))]
public partial class MyContext : JsonSerializerContext { }
```

### Combining source generators

The [contract customization](https://learn.microsoft.com/dotnet/standard/serialization/system-text-json/custom-contracts) feature introduced in .NET 7 added support for chaining source generators by means of the `JsonTypeInfoResolver.Combine` method. This makes it possible to combine contracts from multiple source generated contexts inside a single `JsonSerializerOptions` instance:

```csharp
var options = new JsonSerializerOptions
{
    TypeInfoResolver = JsonTypeInfoResolver.Combine(ContextA.Default, ContextB.Default, ContextC.Default);
};
```

Based on feedback we’ve received, this approach has a couple of usability issues:

1. It necessitates specifying all chained components at one call site — resolvers cannot be prepended or appended to the chain after the fact.
2. Because the chaining implementation is abstracted behind a `IJsonTypeInfoResolver` implementation, there is no way for users to introspect the chain or remove components from it.

The `JsonSerializerOptions` class now includes a `TypeInfoResolverChain` property that is complementary to `TypeInfoResolver`:

```csharp
namespace System.Text.Json;

public partial class JsonSerializerOptions
{
    public IJsonTypeInfoResolver? TypeInfoResolver { get; set; }
    public IList<IJsonTypeInfoResolver> TypeInfoResolverChain { get; }
}
```

This lets users both introspect and modify the chain of resolvers, as can be seen in the following ASP.NET Core configuration snippet:

```csharp
WebApplicationBuilder builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    // Prepend my source generated context so that it takes precedence over other registered contexts
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, MyContext.Default);
});
```

### Unspeakable type support

Compiler-generated or “unspeakable” types have been challenging to support in weakly typed source gen scenarios. In .NET 7, the following application will fail:

```csharp
object value = Test();
Stream stdout = Console.OpenStandardOutput();
await JsonSerializer.SerializeAsync(stdout, value, MyContext.Default.Options);

async IAsyncEnumerable<int> Test()
{
    for (int i = 0; i < 5; i++)
    {
        await Task.Delay(500);
        yield return i;
    }
}

[JsonSerializable(typeof(IAsyncEnumerable<int>))]
public partial class MyContext : JsonSerializerContext { }
```

producing the error message

```text
Metadata for type 'Program+<<<Main>$>g__Test|0_5>d' was not provided by TypeInfoResolver of type 'MyContext'
```

This happens because the compiler-generated type `Program+<<<Main>$>g__Test|0_5>d` cannot be explicitly specified by the source generator.

Starting with .NET 8, System.Text.Json will perform run-time nearest-ancestor resolution to determine the most appropriate supertype with which to serialize the value (in this case, `IAsyncEnumerable<int>`), making the above snippet output a JSON array as expected:

```text
[0,1,2,3,4]
```

### `JsonStringEnumConverter<TEnum>`

The [`JsonStringEnumConverter` class](https://learn.microsoft.com/dotnet/api/system.text.json.serialization.jsonstringenumconverter) is the API of choice for users looking to serialize enum types as string values. The type however requires run-time code generation and as such cannot be used in Native AOT applications.

The new release of System.Text.Json includes the generic `JsonStringEnumConverter<TEnum>` class which does not require run-time code generation. Users wishing to target Native AOT should annotate their enums as follows:

```csharp
[JsonConverter(typeof(JsonStringEnumConverter<MyEnum>))]
public enum MyEnum { Value1, Value2, Value3 }

[JsonSerializable(typeof(MyEnum))]
public partial class MyContext : JsonSerializerContext { }
```

The source generator will emit diagnostic [SYSLIB1034](https://learn.microsoft.com/dotnet/fundamentals/syslib-diagnostics/source-generator-overview) if it does encounter the unsupported `JsonStringEnumConverter` in the type graph, prompting users to replace it with the new generic type.

String enum conversion can also be applied as a blanket policy in the source generator using the `JsonSourceGenerationOptions` attribute; the following is equivalent to the previous code:

```csharp
public enum MyEnum { Value1, Value2, Value3 }

[JsonSourceGenerationOptions(UseStringEnumConverter = true)]
[JsonSerializable(typeof(MyEnum))]
public partial class MyContext : JsonSerializerContext { }
```

### Extended `JsonSourceGenerationOptionsAttribute` functionality

The [`JsonSourceGenerationOptions` attribute](https://learn.microsoft.com/dotnet/api/system.text.json.serialization.jsonsourcegenerationoptionsattribute) lets users specify compile-time configuration for a small subset of settings available in the `JsonSerializerOptions` class. Users looking to configure the source generator using settings beyond what was available on the attribute needed to manually create a `JsonSerializerContext` instance:

```csharp
var options = new JsonSerializerOptions(JsonSerializerDefaults.Web)
{
    AllowTrailingCommas = true,
    DefaultBufferSize = 10
};

var context = new MyContext(options);

public record MyPoco(int Id, string Title);

[JsonSerializable(typeof(MyPoco))]
public partial class MyContext : JsonSerializerContext { }
```

The attribute has now been augmented with most settings available in `JsonSerializerOptions`, so the above can now be rendered as follows:

```csharp
MyContext context = MyContext.Default;

public record MyPoco(int Id, string Title);

[JsonSourceGenerationOptions(
    JsonSerializerDefaults.Web, 
    AllowTrailingCommas = true, 
    DefaultBufferSize = 10)]
[JsonSerializable(typeof(MyPoco))]
public partial class MyContext : JsonSerializerContext {}
```

### Disabling reflection defaults

By default, System.Text.Json will use reflection when serializing. Running something as simple as

```csharp
JsonSerializer.Serialize(42);
```

either in your code or in a third-party dependency can break your Native AOT application since it requires unsupported reflection under the hood. This can be challenging to diagnose since you're most likely debugging your application in CoreCLR where the above works just fine.

It is now possible to disable default reflection in applications using the `System.Text.Json.JsonSerializer.IsReflectionEnabledByDefault` feature switch. This can be turned off using the `JsonSerializerIsReflectionEnabledByDefault` MSBuild property in your project configuration:

```xml
<JsonSerializerIsReflectionEnabledByDefault>false</JsonSerializerIsReflectionEnabledByDefault>
```

which makes the previous snippet fail with the following error:

```text
System.InvalidOperationException: Reflection-based serialization has been disabled for this application. Either use the source generator APIs or explicitly configure the 'JsonSerializerOptions.TypeInfoResolver' property.
```

It should be noted that this behavior is consistent regardless of runtime (i.e. both CoreCLR and Native AOT). If left unspecified, the feature switch will be turned off automatically in projects that enable the `PublishTrimmed` property.

The run-time value of the feature switch is reflected by the `JsonSerializer.IsReflectionEnabledByDefault` property. Library authors can consult that value when configuring their serializers:

```csharp
static JsonSerializerOptions CreateDefaultOptions()
{
    return new()
    {
        TypeInfoResolver = JsonSerializer.IsReflectionEnabledByDefault
            ? new DefaultJsonTypeInfoResolver()
            : MyContext.Default
    };
}
```

Because the property is treated as a link-time constant, the above method will avoid rooting the reflection-based resolver in applications that do run in Native AOT.

### Size reduction

We made a number of internal changes that contribute to size reduction of trimmed applications that use System.Text.Json. Consider the following minimal application:

```csharp
Todo value = new(1, "Walk the dog");
string json = JsonSerializer.Serialize(value, MyContext.Default.Todo);
JsonSerializer.Deserialize(json, MyContext.Default.Todo);

public record Todo(int Id, string Title);

[JsonSerializable(typeof(Todo))]
public partial class MyContext : JsonSerializerContext { }
```

Publishing as a self-contained Native AOT application in .NET 7 on Windows gives a 3.4 MB binary. On .NET 8 the same application is 2.6 MB, a 23% reduction.

We're seeing similar improvements in production apps&mdash;the Microsoft Store team [had this to share](https://twitter.com/SergioPedri/status/1701633787845546426):

> The Microsoft Store is now happily using System.Text.Json 8.0 RC1 builds, and the new version gave us a whopping 4.2 MB package size improvement compared to 7.0! We're also using source generators to make serialization code trim/AOT-friendly and super efficient 🚀

### Bugfixes

This release includes a large number of bug fixes, performance improvements, and reliability enhancements to the source generator:

* [dotnet/runtime#86121](https://github.com/dotnet/runtime/pull/86121) adds caching support to the incremental generator, improving IDE performance in large projects.
* [dotnet/runtime#86526](https://github.com/dotnet/runtime/pull/86526) and [dotnet/runtime#87557](https://github.com/dotnet/runtime/pull/87557) improve formatting of source generated code, including fixes to a number of indentation issues.
* [dotnet/runtime#87980](https://github.com/dotnet/runtime/pull/87980) adds a number of new diagnostic warnings.
* [dotnet/runtime#87136](https://github.com/dotnet/runtime/pull/87136) fixes a number of bugs related to accessibility modifier resolution.
* [dotnet/runtime#87383](https://github.com/dotnet/runtime/pull/87383) ensures that types of ignored or inaccessible properties are not included by the generator.
* [dotnet/runtime#87484](https://github.com/dotnet/runtime/pull/87484) fixes issues related to `JsonNumberHandling` support.
* [dotnet/runtime#87632](https://github.com/dotnet/runtime/pull/87632) fixes support for recursive collection types.
* [dotnet/runtime#84208](https://github.com/dotnet/runtime/pull/84208) fixes custom converter support for nullable structs.
* [dotnet/runtime#87796](https://github.com/dotnet/runtime/pull/87796) fixes a number of bugs in the compile-time attribute parsing implementation.
* [dotnet/runtime#87829](https://github.com/dotnet/runtime/pull/87829) adds support for nesting `JsonSerializerContext` declarations within arbitrary containing types.

## Populate read-only members

Consider the following example:

```csharp
MyPoco result = JsonSerializer.Deserialize<MyPoco>("""{ "Values" : [1,2,3] }""");
Console.WriteLine(result.Values.Count); // 0

public class MyPoco
{
    public IList<int> Values { get; } = new List<int>();
}
```

Because the `Values` property doesn't include a setter, the serializer will not attempt to bind any data to it, despite the fact that the getter returns a mutable collection that _could_ be populated by the serializer. Starting with System.Text.Json v8, this behavior can be configured via the `JsonObjectCreationHandling` attribute:

```csharp
MyPoco result = JsonSerializer.Deserialize<MyPoco>("""{ "Values" : [1,2,3] }""");
Console.WriteLine(result.Values.Count); // 3

public class MyPoco
{
    [JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
    public IList<int> Values { get; } = new List<int>();
}
```

Applying the attribute on the type sets the policy on all contained members, where applicable:

```csharp
MyPoco result = JsonSerializer.Deserialize<MyPoco>("""{ "Values" : [1,2,3], "Person" : { "Name" : "Brian" } }""");
Console.WriteLine(result.Values.Count); // 3
Console.WriteLine(result.Person.Name); // Brian

[JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
public class MyPoco
{
    public IList<int> Values { get; } = new List<int>();

    public Person Person { get; } = new();
}

public class Person
{
    public string Name { get; set; }
}
```

The policy can also be set globally via the `PreferredObjectCreationHandling` property on `JsonSerializerOptions`:

```csharp
var options = new JsonSerializerOptions 
{ 
    PreferredObjectCreationHandling = JsonObjectCreationHandling.Populate 
};

MyPoco result = JsonSerializer.Deserialize<MyPoco>("""{ "Values" : [1,2,3] }""", options);
Console.WriteLine(result.Values.Count); // 3

public class MyPoco
{
    public IList<int> Values { get; } = new List<int>();
}
```

or the corresponding property on `JsonSourceGenerationOptionsAttribute` if using the source generator:

```csharp
MyPoco result = JsonSerializer.Deserialize("""{ "Values" : [1,2,3] }""", MyContext.Default.MyPoco);
Console.WriteLine(result.Values.Count); // 3

public class MyPoco
{
    public IList<int> Values { get; } = new List<int>();
}

[JsonSourceGenerationOptions(PreferredObjectCreationHandling = JsonObjectCreationHandling.Populate)]
[JsonSerializable(typeof(MyPoco))]
public partial class MyContext : JsonSerializerContext { }
```

### Final notes

* Struct types can also be populated but this requires that the member is settable.
* Population of collections is additive; existing elements in a collection won't be cleared and deserialized values will be appended to it.
* The population policy can be configured via [contract customization](https://learn.microsoft.com/dotnet/standard/serialization/system-text-json/custom-contracts) using the `JsonPropertyInfo.ObjectCreationHandling` property.

## Missing member handling

It’s now possible to configure object deserialization behavior, whenever the underlying JSON payload includes properties that cannot be mapped to members of the deserialized POCO type. This can be controlled by applying a `JsonUnmappedMemberHandling` attribute on the POCO type itself, globally using the `UnmappedMemberHandling` property on `JsonSerializerOptions`/`JsonSourceGenerationOptionsAttribute` or programmatically by customizing the `JsonTypeInfo.UnmappedMemberHandling` property:

```csharp
JsonSerializer.Deserialize<MyPoco>("""{"Id" : 42, "AnotherId" : -1 }"""); 
// JsonException : The JSON property 'AnotherId' could not be mapped to any .NET member contained in type 'MyPoco'.

[JsonUnmappedMemberHandling(JsonUnmappedMemberHandling.Disallow)]
public class MyPoco
{
   public int Id { get; set; }
}
```

## Snake case and kebab case naming policies

The library now ships with naming policies for `snake_case` and `kebab-case` conversions:

```csharp
var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower };
JsonSerializer.Serialize(new { PropertyName = "value" }, options); // { "property_name" : "value" }

var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.KebabCaseUpper };
JsonSerializer.Serialize(new { PropertyName = "value" }, options); // { "PROPERTY-NAME" : "value" }
```

Here's a full list of all available naming policies:

```csharp
namespace System.Text.Json;

public class JsonNamingPolicy
{
  public static JsonNamingPolicy CamelCase { get; }
  public static JsonNamingPolicy KebabCaseLower { get; }
  public static JsonNamingPolicy KebabCaseUpper { get; }
  public static JsonNamingPolicy SnakeCaseLower { get; }
  public static JsonNamingPolicy SnakeCaseUpper { get; }
}
```

Many thanks to [@YohDeadfall](https://github.com/YohDeadfall) for contributing to the design and implementation of this feature!

## Interface hierarchy support

The new version fixes support for interface hierarchy serialization. The code:

```csharp
IDerived value = new Implementation { Base = 0, Derived = 1 };
string json = JsonSerializer.Serialize(value);
Console.WriteLine(json);

public interface IBase
{
    public int Base { get; set; }
}

public interface IDerived : IBase
{
    public int Derived { get; set; }
}

public class Implementation : IDerived
{
    public int Base { get; set; }
    public int Derived { get; set; }
}
```

Will output `{"Derived":1}` in .NET 7 and earlier versions. Starting with .NET 8 it will serialize all properties in the type hierarchy:

```json
{"Derived":1,"Base":0}
```

which is similar to how class hierarchies are handled.

It should be noted that interface hierarchies admit multiple inheritance, so in rare cases where there are diamond ambiguities:

```csharp
IDiamond value = new Implementation { Value = 0 };
string json = JsonSerializer.Serialize(value);
Console.WriteLine(json);

public interface IBase1
{
    public int Value { get; set; }
}

public interface IBase2
{
    public int Value { get; set; }
}

public interface IDiamond : IBase1, IBase2 { }

public class Implementation : IDiamond
{
    public int Value { get; set; }
}
```

the serializer will reject the type altogether:

```text
System.InvalidOperationException: The JSON property name for 'IDiamond.Value' collides with another property.
```

## Built-in support for `Half`, `Int128` and `UInt128`

The numeric types `System.Half`, `System.Int128` and `System.UInt128` are now supported out of the box:

```csharp
Console.WriteLine(JsonSerializer.Serialize(new object[] { Half.MaxValue, Int128.MaxValue, UInt128.MaxValue }));
// [65500,170141183460469231731687303715884105727,340282366920938463463374607431768211455]
```

## Built-in support for `Memory<T>` and `ReadOnlyMemory<T>`

`Memory<T>` and `ReadOnlyMemory<T>` are now supported out of the box, with semantics being equivalent to arrays:

* Serializes to Base64 encoded JSON strings for `Memory<byte>` and `ReadOnlyMemory<byte>` values.
* Serializes to JSON arrays for all other types.

```csharp
JsonSerializer.Serialize<ReadOnlyMemory<byte>>(new byte[] { 1, 2, 3 }); // "AQID"
JsonSerializer.Serialize<ReadOnlyMemory<int>>(new int[] { 1, 2, 3 }); // [1,2,3]
```

## Single-usage `JsonSerializerOptions` analyzer

A common source of performance issues in applications using System.Text.Json is single-use `JsonSerializerOptions` instances. Even though the type is nominally just an options type, in actuality it also encapsulates all caches used by the serializer. Writing something as simple as:

```csharp
JsonSerializer.Serialize<MyPoco>(value, new JsonSerializerOptions { WriteIndented = true });
```

will result in metadata caches being recomputed _on each serialization operation_. Even though we mitigated some of these performance issues in .NET 7 using a shared cache scheme, it is still the case that caching and reusing `JsonSerializerOptions` singletons in user code is the optimal course of action.

For this purpose, we shipped analyzer [CA1869](https://learn.microsoft.com/dotnet/fundamentals/code-analysis/quality-rules/ca1869) which will emit a relevant warning whenever it detects single-use options instances.

## Extend `JsonIncludeAttribute` and `JsonConstructorAttribute` support to non-public members

The `JsonIncludeAttribute` and `JsonConstructorAttribute` are annotations that let users opt specific members into the serialization contract for a given type (properties/fields and constructors respectively). Until now these were limited to public members, but this has now been relaxed to include non-public members:

```csharp
string json = JsonSerializer.Serialize(new MyPoco(42)); // {"X":42}
JsonSerializer.Deserialize<MyPoco>(json);

public class MyPoco
{
    [JsonConstructor]
    internal MyPoco(int x) => X = x;

    [JsonInclude]
    internal int X { get; }
}
```

An important caveat to the above is that the source generator is still restricted to members that are visible to generated code, for example the type

```csharp
public class MyPoco
{
    [JsonInclude]
    private int value;
}
```

works as expected in the reflection serializer, but is not supported by the source generator and will result in a [SYSLIB1038](https://learn.microsoft.com/dotnet/fundamentals/syslib-diagnostics/syslib1038) diagnostic warning being issued.

## `IJsonTypeInfoResolver.WithAddedModifier`

This new [extension method](https://learn.microsoft.com/dotnet/api/system.text.json.serialization.metadata.jsontypeinforesolver.withaddedmodifier) enables making modifications to serialization contracts of arbitrary `IJsonTypeInfoResolver` instances, including `JsonSerializerContext`:

```csharp
var options = new JsonSerializerOptions
{
    TypeInfoResolver = MyContext.Default
        .WithAddedModifier(static typeInfo =>
        {
            foreach (JsonPropertyInfo prop in typeInfo.Properties)
            {
                prop.Name = prop.Name.ToUpperInvariant();
            }
        })
};

JsonSerializer.Serialize(new MyPoco(42), options); // {"VALUE":42}

public record MyPoco(int value);

[JsonSerializable(typeof(MyPoco))]
public partial class MyContext : JsonSerializerContext { }
```

In effect, this extends the [`DefaultJsonTypeInfoResolver.Modifiers`](https://learn.microsoft.com/dotnet/api/system.text.json.serialization.metadata.defaultjsontypeinforesolver.modifiers) API to any `IJsonTypeInfoResolver` instance.

## `JsonSerializerOptions.MakeReadOnly()`

Since released, the `JsonSerializerOptions` type was designed to have freezable semantics. In other words, instances are mutable until the first serialization operation occurs, after which time they can no longer be modified.

The newly added `MakeReadOnly` methods make it possible to explicitly freeze the instance for further modification without requiring a full-blown serialization operation:

```csharp
static JsonSerializerOptions CreateDefaultOptions()
{
    var options = new JsonSerializerOptions 
    { 
        TypeInfoResolver = new DefaultJsonTypeInfoResolver(),
        WriteIndented = true
    };

    options.MakeReadOnly(); // prevent accidental modification outside the method
    return options;
}
```

The type now also comes with an `IsReadOnly` property reflecting its current state:

```csharp
JsonSerializerOptions options = new();
Console.WriteLine(options.IsReadOnly); // False

JsonSerializer.Serialize("value", options);
Console.WriteLine(options.IsReadOnly); // True
```

## Additional `JsonNode` functionality

The `JsonNode` APIs now come with the following new methods:

```csharp
namespace System.Text.Json.Nodes;

public partial class JsonNode
{
    // Creates a deep clone of the current node and all its descendants.
    public JsonNode DeepClone();

    // Returns true if the two nodes are equivalent JSON representations.
    public static bool DeepEquals(JsonNode? node1, JsonNode? node2);

    // Determines the JsonValueKind of the current node.
    public JsonValueKind GetValueKind(JsonSerializerOptions options = null);

    // If node is the value of a property in the parent object, returns its name.
    public string GetPropertyName();
   
    // If node is an element of a parent JsonArray, returns its index.
    public int GetElementIndex();

    // Replaces this instance with a new value, updating the parent node accordingly.
    public void ReplaceWith<T>(T value);
}

public partial class JsonArray
{
    // Returns an IEnumerable<T> view of the current array.
    public IEnumerable<T> GetValues<T>();
}
```

For example, deep cloning:

```csharp
JsonNode node = JsonNode.Parse("""{ "Prop" : { "NestedProp" : 42 }""");
JsonNode other = node.DeepClone();
bool same = JsonNode.DeepEquals(node, other); // true
```

Support for `IEnumerable`:

```csharp
JsonArray jsonArray = new JsonArray(1, 2, 3, 2);
IEnumerable<int> values = jsonArray.GetValues<int>().Where(i => i == 2);
```

## `JsonNode.ParseAsync` APIs

Adds support for parsing `JsonNode` instances from streams:

```csharp
using var stream = File.OpenRead("myFile.json");
JsonNode node = await JsonNode.ParseAsync(stream);
```

## `System.Net.Http.Json` improvements

We've shipped a number of new APIs for the separately bundled `System.Net.Http.Json` package.

### IAsyncEnumerable extensions

.NET 8 sees the inclusion of IAsyncEnumerable streaming deserialization extension methods:

```csharp
const string RequestUri = "https://api.contoso.com/books";
using var client = new HttpClient();
IAsyncEnumerable<Book> books = client.GetFromJsonAsAsyncEnumerable<Book>(RequestUri);

await foreach (Book book in books)
{
    Console.WriteLine($"Read book '{book.title}'");
}

public record Book(int id, string title, string author, int publishedYear);
```

### `JsonContent.Create` overloads accepting `JsonTypeInfo`

It is now possible to create `JsonContent` instances using trim safe/source generated contracts:

```csharp
var book = new Book(id: 42, "Title", "Author", publishedYear: 2023);
HttpContent content = JsonContent.Create(book, MyContext.Default.Book);

public record Book(int id, string title, string author, int publishedYear);

[JsonSerializable(typeof(Book))]
public partial class MyContext : JsonSerializerContext 
{ }
```

## `JsonConverter.Type` property

The new property allows users to look up the type of a non-generic `JsonConverter` instance:

```csharp
Dictionary<Type, JsonConverter> CreateDictionary(IEnumerable<JsonConverter> converters)
    => converters.Where(converter => converter.Type != null).ToDictionary(converter => converter.Type!);
```

The property is nullable since it returns `null` for `JsonConverterFactory` instances and `typeof(T)` for `JsonConverter<T>` instances.

## Performance improvements

For a detailed write-up of System.Text.Json performance improvements in .NET 8, please refer to the [relevant section](https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-8/#json) in Stephen Toub’s "Performance Improvements in .NET 8" article.

## Closing

.NET 8 sees a large number of new features, reliability enhancements and quality of life improvements with a focus on improved Native AOT support.
In total [135 pull requests](https://github.com/dotnet/runtime/pulls?q=is%3Apr+is%3Amerged+label%3Aarea-System.Text.Json+milestone%3A8.0.0) were contributed to System.Text.Json during .NET 8 development. We'd like you to try the new features and give us feedback on how it improves your applications, and any usability issues or bugs that you might encounter.

Community contributions are always welcome. If you'd like to contribute to System.Text.Json, check out our list of [`help wanted` issues](https://github.com/dotnet/runtime/issues?q=is%3Aopen+is%3Aissue+label%3A%22help+wanted%22+label%3Aarea-System.Text.Json) on GitHub.
