---
post_title: Try the new System.Text.Json source generator
username: laakinri@microsoft.com
microsoft_alias: laakinri
categories: .NET, .NET Core, JSON, Performance
summary: Learn about the new System.Text.Json source generator
desired_publication_date: 2021-07-22
---

In .NET 6.0, we are shipping a new [C# source generator](https://devblogs.microsoft.com/dotnet/introducing-c-source-generators/) to help improve the performance of applications that use `System.Text.Json`. In this post, I'll go over why we built it, how it works, and what benefits you can experience in your application.

With the introduction of the `System.Text.Json` source generator, we now have a few models for JSON serialization in .NET to choose from, using `JsonSerializer`. There is the existing model which is backed by runtime reflection, and two new compile-time source generation modes; where the generator generates optimized serialization logic, a static data access model, or both. In both source-generation scenarios, the generated artifacts are passed directly to `JsonSerializer` as a performance optimization. Here's an overview of the functionality that each serialization model provides:

|                                          | `JsonSerializer` | `JsonSerializer` + pre-generating optimized serialization logic | `JsonSerializer` + pre-generating data access model  |
|------------------------------------------|------------------|-----------------------------------------------------------------|------------------------------------------------------|
| Increases serialization throughput       | No (baseline)    | Yes                                                             | No                                                   |
| Reduces start-up time                    | No (baseline)    | Yes                                                             | Yes                                                  |
| Reduces private memory usage             | No (baseline)    | Yes                                                             | Yes                                                  |
| Eliminates runtime reflection            | No               | Yes                                                             | Yes                                                  |
| Facilitates trim-safe app size reduction | No               | Yes                                                             | Yes                                                  |
| Supports all serialization features      | Yes              | No                                                              | Yes                                                  |

## Getting the source generator

The source generator can be consumed in any .NET C# project, including console applications, class libraries, web, and Blazor applications. You can try out the source generator by using the latest build of the [`System.Text.Json` NuGet package](https://dev.azure.com/dnceng/public/_packaging?_a=package&feed=dotnet6&package=System.Text.Json&protocolType=NuGet&view=versions). Starting with the upcoming .NET 6.0 Preview 7 this won't be necessary when targeting `net6.0`.

The source generator is compatible with other target framework monikers (TFMs) aside from .NET 6.0, that is .NET 5.0 and lower, .NET Framework, and .NET Standard. The API shape of the generated source code is consistent across the TFMs, but the implementation may vary based on the framework APIs that are available on each TFM.

The minimum version of the .NET SDK required for C# source generation is .NET 5.0.

## What is the `System.Text.Json` source generator?

The backbone of nearly all .NET serializers is reflection. Reflection provides great capability for certain scenarios, but not as the basis of high-performance cloud-native applications (which typically (de)serialize and process a lot of JSON documents). Reflection is a problem for startup, memory usage, and assembly trimming.

An alternative to runtime reflection is compile-time [source generation](https://devblogs.microsoft.com/dotnet/new-c-source-generator-samples/). Source generators generate C# source files that can be compiled as part of the library or application build. Generating source code at compile time can provide many benefits to .NET applications, including increased performance. In .NET 6, we are including a new source generator as part of `System.Text.Json`. The JSON source generator works in conjunction with `JsonSerializer`, and can be configured in multiple ways. Existing `JsonSerializer` functionality will continue to work as-is, so it's your decision whether you use the new source generator. The approach the JSON source generator takes to provide these benefits is to move the runtime inspection of JSON-serializable types to compile-time, where it generates a static model to access data on the types, optimized serialization logic using `Utf8JsonWriter` directly, or both.

To opt into source generation, you can define a partial class which derives from a new class called `JsonSerializerContext`, and indicate serializable types using a new `JsonSerializableAttribute` class.

For example, given a simple `Person` type to serialize:

```cs
namespace Test
{
    internal class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
```

We would specify the type to the source generator as follows:

```cs
using System.Text.Json.Serialization;

namespace Test
{
    [JsonSerializable(typeof(Person))]
    internal partial class MyJsonContext : JsonSerializerContext
    {
    }
}
```

As part of the build, the source generator will augment the `MyJsonContext` partial class with the following shape:

```cs
internal partial class MyJsonContext : JsonSerializerContext
{
    public static MyJsonContext Default { get; }

    public JsonTypeInfo<Person> Person { get; }

    public MyJsonContext(JsonSerializerOptions options) { }

    public override JsonTypeInfo GetTypeInfo(Type type) => ...;
}
```

The generated source code can be integrated into the compiling application by passing it directly to new overloads on `JsonSerializer`:

```cs
Person person = new() { FirstName = "Jane", LastName = "Doe" };
byte[] utf8Json = JsonSerializer.SerializeToUtf8Bytes(person, MyJsonContext.Default.Person);
person = JsonSerializer.Deserialize(utf8Json, MyJsonContext.Default.Person):
```

## Why do source generation?

`JsonSerializer` is a tool that serializes .NET objects into JSON strings, and deserializes .NET objects from JSON strings. To process a type, the serializer needs information about how to access its members. When serializing, the serializer needs access to the object's property and field getters. Similarly, when deserializing, the serializer needs access to a constructor to instantiate the type, and also to setters for the object's properties and fields.

`System.Text.Json` exposes mechanisms for influencing serialization and deserialization behavior when using `JsonSerializer`, via `JsonSerializerOptions` (which allows runtime configuration), and also via attributes like `[JsonPropertyName(string)]` and `[JsonIgnore]` (which allow design-time configuration). When serializing and deserializing instances of a type, the serializer needs information about this configuration so that it can be honored.

When processing JSON-serializable types, the serializer needs information about object members and feature configuration in a structured, optimized format. We can refer to this structured information as the serialization metadata required to serialize a type.

In previous versions of `System.Text.Json`, serialization metadata could only be computed at runtime, during the first serialization or deserialization routine of every type in any object graph passed to the serializer. After this metadata is generated, the serializer performs the actual serialization and deserialization. The result of this computation is cached for reuse during subsequent JSON processing routines. The generation phase is based on reflection, and is computationally expensive both in terms of throughput and allocations. We can refer to this phase as the serializer's "warm-up" phase.

The `System.Text.Json` source generator helps us to remove this warm-up phase by shifting the runtime inspection of serializable types using reflection to compile-time. The result of this inspection can be source code that initializes instances of structured serialization metadata. The generator can also generate highly-optimized serialization logic that can honor a set of [serialization features](https://github.com/dotnet/runtime/blob/779b79401648908279d0903b5a91f593c745efd0/src/libraries/System.Text.Json/Common/JsonSourceGenerationOptionsAttribute.cs) that are specified ahead-of-time. By default, the generator emits both kinds of source, but can be configured to generate only one of these kinds of outputs either across a set of types, or per serializable type.

This generated metadata is included in the compiled assembly, where it can be initialized and passed directly to `JsonSerializer` so that the serializer doesn't have to generate it at runtime. This helps reduce the costs of the first serialization or deserialization of each type. With these characteristics, using the source generator can provide the following benefits to applications that use `System.Text.Json`:

- Increased serialization throughput
- Reduced start-up time
- Reduced private memory usage
- Removed runtime use of `System.Reflection` and `System.Reflection.Emit`
- Trim-compatible serialization which reduces application size

## Introducing `JsonTypeInfo<T>`, `JsonTypeInfo`, and `JsonSerializerContext`

Implementations of the `JsonTypeInfo<T>`, `JsonTypeInfo`, and `JsonSerializerContext` types are the primary result of JSON source generation.

The `JsonTypeInfo<T>` type contains structured information about how to serialize and deserialize a single type. This information can contain metadata about how to access its members. This information is required when the serializer itself is performing the (de)serialization of the type, using the robust logic it has to support **all** the features that can be configured with `JsonSerializerOptions` or serialization attributes. This includes advanced features like async (de)serialization and reference handling. When only a limited set of features are needed, `JsonTypeInfo<T>` can contain optimized, pre-generated serialization logic (using `Utf8JsonWriter` directly) which the serializer can invoke instead of going through its own code-paths. Invoking this logic can lead to substantial serialization throughput improvements. A `JsonTypeInfo<T>` instance is tightly bound to a single instance of `JsonSerializerOptions`.

The `JsonTypeInfo` type provides an untyped abstraction over `JsonTypeInfo<T>`. `JsonSerializer` utilizes it to retrieve a `JsonTypeInfo<T>` instance from a `JsonSerializerContext` instance via `JsonSerializerContext.GetTypeInfo`.

The `JsonSerializerContext` type contains `JsonTypeInfo<T>` instances for multiple types. In addition to helping `JsonSerializer` retrieve instances of `JsonTypeInfo<T>` via `JsonSerializerContext.GetTypeInfo`, it also provides a mechanism for initializing all the type metadata using specific `JsonSerializerOptions` instances provided by users.

## Source generation modes

The `System.Text.Json` source generator has two modes: one that generates type-metadata initialization logic, and another that generates serialization logic. Users can configure the source generator to use one or both of these modes for JSON-serializable types in a project, depending on the (de)serialization scenario. Metadata generated for a type contains structured information in a format that can be optimally utilized by the serializer to serialize and deserialize instances of that type to and from JSON representations. Serialization logic uses direct calls to `Utf8JsonWriter` methods to write JSON representations of .NET objects, using a predetermined set of serialization options. By default, the source generator generates both metadata initialization logic and serialization logic, but can be configured to generate just one type of logic. To set the generation mode for the entire context (set of serializable types), use `JsonSourceGenerationOptionsAttribute.GenerationMode`, while to set the mode for a specific type, use `JsonSerializableAttribute.GenerationMode`.

### Generating optimized serialization logic

The first source generation mode we'll look at is `JsonSourceGenerationMode.Serialization`. It delivers much higher performance than using existing `JsonSerializer` methods by generating source code that uses `Utf8JsonWriter` directly. In short, source generators offer a way of giving you a different implementation at compile-time in order to make the runtime experience better.

`JsonSerializer` is a powerful tool that has many features that can influence the (de)serialization of .NET types from/into the JSON format. It is fast, but can have some performance overhead when only a subset of features are needed for a serialization routine. Going forward, we will update `JsonSerializer` and the new source generator together. Sometimes, a new `JsonSerializer` feature will have accompanying support for optimized serialization logic and sometimes not, depending on how feasible it is to generate logic to support the feature.

Given our `Person` type from above, the source generator can be configured to generate serialization logic for instances of the type, given some pre-defined serializer options. Note that the class name `MyJsonContext` is arbitrary. You can use whatever class name you want.

```cs
using System.Text.Json.Serialization;

namespace Test
{
    [JsonSerializerOptions(
        PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
        GenerationMode = JsonSourceGenerationMode.Serialization)]
    [JsonSerializable(typeof(Person))]
    internal partial class MyJsonContext : JsonSerializerContext
    {
    }
}
```

We have defined a [set of `JsonSerializer` features](https://github.com/dotnet/runtime/blob/779b79401648908279d0903b5a91f593c745efd0/src/libraries/System.Text.Json/Common/JsonSourceGenerationOptionsAttribute.cs) that are supported in this mode via `JsonSourceGenerationOptionsAttribute`. As shown above, these features can be specified to the source generator ahead of time, to avoid extra checks at runtime.

As part of the build, the source generator augments the `MyJsonContext` partial class with the following shape:

```cs
internal partial class MyJsonContext : JsonSerializerContext
{
    public static MyJsonContext Default { get; }

    public JsonTypeInfo<Person> Person { get; }

    public MyJsonContext(JsonSerializerOptions options) { }

    public override JsonTypeInfo GetTypeInfo(Type type) => ...;
}
```

The serializer invocation with this mode could look like the following example. This example allows us to trim out a lot of the `System.Text.Json` implementation since we don't call into `JsonSerializer`.

```cs
using MemoryStream ms = new();
using Utf8JsonWriter writer = new(ms);

MyJsonContext.Default.Person.Serialize(writer, new Person { FirstName = "Jane", LastName = "Doe" });
writer.Flush();

// Writer contains:
// {"firstName":"Jane","lastName":"Doe"}
```

Alternatively, you can continue to use `JsonSerializer`, and instead pass an instance of the generated code to it, with `MyJsonContext.Default.Person`.

```cs
JsonSerializer.Serialize(person, MyJsonContext.Default.Person);
```

Here's a similar use, with a different overload.

```cs
JsonSerializer.Serialize(person, typeof(Person), MyJsonContext.Default);
```

The difference between these two overloads is that the first is using the typed metadata implementation -- `JsonTypeInfo<T>` -- and the second one is using a more general `JsonSerializerContext` implementation that does type tests to determine if a typed implementation exists. It is a little slower (due to the type tests), as a result. If there is no source-generated implementation for a given type, then the serializer throws a `NotSupportedException`. It does not fallback to a reflection-based implementation (as an explicit design choice).

In the example above, the `MyJsonContext.Default.Person` property returns a `JsonTypeInfo<Person>`. The `Default` property returns a `MyJsonContext` instance whose backing `JsonSerializerOptions` instance matches the values set by the `JsonSourceGenerationOptionsAttribute` set on the `JsonSerializerContext`. If the attribute was not present, a default `JsonSerializerOptions` instance (i.e the result of `new JsonSerializerOptions(JsonSerializerDefaults.General)`) would be used.

This fastest and most optimized source generation mode -- based on `Utf8JsonWriter` -- is currently only available for serialization. Similar support for deserialization -- based on `Utf8JsonReader` -- will be considered for support in a future version of .NET. You will see in the following sections that there are other modes that benefit deserialization.

### Generating type-metadata initialization logic

The generator can be configured to generate type-metadata initialization logic -- with the `JsonSourceGenerationMode.Metadata` mode -- instead of the complete serialization logic. This mode provides a static data access model for the regular `JsonSerializer` code paths to invoke when executing serialization and deserailization logic. This mode is useful if you need features that are not supported by the `JsonSourceGenerationMode.Serialization` mode, such as reference handling and async serialization. This mode also provide benefits for deserialization, which the serialization-logic mode does not.

The `JsonSourceGenerationMode.Metadata` mode provides much of the benefits of source generation, with the exception of improved serialization throughput. With this mode, runtime metadata generation is shifted to compile-time. Like the previous mode, the required metadata is generated into the compiling assembly, where it can be initialized and passed to `JsonSerializer`. This approach reduces the costs of the first serialization or deserialization of each type. Most of the other benefits stated earlier also apply, such as lower memory usage.

This mode is configured in a similar way as the previous example, except that we don't specify feature options ahead of time, and we specify different generation mode:

```cs
using System.Text.Json.Serialization;

namespace Test
{
    [JsonSerializable(typeof(Person), GenerationMode = JsonSourceGenerationMode.Metadata)]
    internal partial class MyJsonContext : JsonSerializerContext
    {
    }
}
```

The generator augments the partial context class with the same shape as shown earlier.

The serializer invocation with this mode would look like the following example.

```cs
JsonSerializerOptions options = new()
{
    ReferenceHander = ReferenceHandler.Preserve,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
};

// Use your custom options to initialize a context instance.
MyJsonContext context = new(options);

string json = JsonSerializer.Serialize(person, context.Person);

// {"id":1,"firstName":"Jane","lastName":"Doe"}
```

Using this mode, you should see a significant performance improvement while enjoying the full range of (de)serializer features. This mode is a nice middle-ground option, depending on your needs.

### Generating both serialization logic and metadata initialization logic

The default configuration for the source generation mode is `JsonSourceGenerationMode.Default`. This is an "everything on" mode that enables both of the source generator modes that were just covered. For example, you might only need features compatible with `JsonSourceGenerationMode.Serialization` for serialization, but also want to improve deserialization performance. In this scenario, you should see faster initial serialization and deserialization, faster serialization throughput, lower memory use, and decreased app size with assembly trimming.

You'd configure the generator as follows. Note you don't have specify a generation mode since the default is to generate both kinds of logic.

```cs
using System.Text.Json.Serialization;

namespace Test
{
    [JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
    [JsonSerializable(typeof(Person))]
    internal partial class MyJsonContext : JsonSerializerContext
    {
    }
}
```

Again, the generated API shape remains the same. We can use the generated source as follows:

```cs
// Serializer invokes pre-generated serialization logic for increased throughput and other benefits.
string json = JsonSerializer.Serialize(person, MyJsonContext.Default.Person);

// Serializer uses pre-generated type-metadata and avoids warm-up stage for deserialization, alongside other benefits.
Person person = JsonSerializer.Deserialize(json, MyJsonContext.Default.Person);
```

## New APIs on `JsonSerializer` and in `System.Net.Http.Json`

In addition to the new APIs we've gone over to configure the source generator, we've added APIs to consume it's output. We've already seen some of the new APIs in the examples above, where we pass `JsonTypeInfo<T>` and `JsonSerializerContext` instances directly to `JsonSerializer` as a performance optimization. We've also added more overloads to `JsonSerializer` and also in the `System.Net.Http.Json` APIs which help optimize the processing of JSON data when interacting with `HttpClient` and `JsonContent`.

### New `JsonSerializer` APIs

```cs
namespace System.Text.Json
{
    public static class JsonSerializer
    {
        public static object? Deserialize(ReadOnlySpan<byte> utf8Json, Type returnType, JsonSerializerContext context) => ...;
        public static object? Deserialize(ReadOnlySpan<char> json, Type returnType, JsonSerializerContext context) => ...;
        public static object? Deserialize(string json, Type returnType, JsonSerializerContext context) => ...;
        public static object? Deserialize(ref Utf8JsonReader reader, Type returnType, JsonSerializerContext context) => ...;
        public static ValueTask<object?> DeserializeAsync(Stream utf8Json, Type returnType, JsonSerializerContext context, CancellationToken cancellationToken = default(CancellationToken)) => ...;
        public static ValueTask<TValue?> DeserializeAsync<TValue>(Stream utf8Json, JsonTypeInfo<TValue> jsonTypeInfo, CancellationToken cancellationToken = default(CancellationToken)) => ...;
        public static TValue? Deserialize<TValue>(ReadOnlySpan<byte> utf8Json, JsonTypeInfo<TValue> jsonTypeInfo) => ...;
        public static TValue? Deserialize<TValue>(string json, JsonTypeInfo<TValue> jsonTypeInfo) => ...;
        public static TValue? Deserialize<TValue>(ReadOnlySpan<char> json, JsonTypeInfo<TValue> jsonTypeInfo) => ...;
        public static TValue? Deserialize<TValue>(ref Utf8JsonReader reader, JsonTypeInfo<TValue> jsonTypeInfo) => ...;
        public static string Serialize(object? value, Type inputType, JsonSerializerContext context) => ...;
        public static void Serialize(Utf8JsonWriter writer, object? value, Type inputType, JsonSerializerContext context) { }
        public static Task SerializeAsync(Stream utf8Json, object? value, Type inputType, JsonSerializerContext context, CancellationToken cancellationToken = default(CancellationToken)) => ...;
        public static Task SerializeAsync<TValue>(Stream utf8Json, TValue value, JsonTypeInfo<TValue> jsonTypeInfo, CancellationToken cancellationToken = default(CancellationToken)) => ...;
        public static byte[] SerializeToUtf8Bytes(object? value, Type inputType, JsonSerializerContext context) => ...;
        public static byte[] SerializeToUtf8Bytes<TValue>(TValue value, JsonTypeInfo<TValue> jsonTypeInfo) => ...;
        public static void Serialize<TValue>(Utf8JsonWriter writer, TValue value, JsonTypeInfo<TValue> jsonTypeInfo) { }
        public static string Serialize<TValue>(TValue value, JsonTypeInfo<TValue> jsonTypeInfo) => ...;
    }
}
```

### New `System.Net.Http.Json` APIs

```cs
namespace System.Net.Http.Json
{
    public static partial class HttpClientJsonExtensions
    {
        public static Task<object?> GetFromJsonAsync(this HttpClient client, string? requestUri, Type type, JsonSerializerContext context, CancellationToken cancellationToken = default(CancellationToken)) => ...;
        public static Task<object?> GetFromJsonAsync(this HttpClient client, System.Uri? requestUri, Type type, JsonSerializerContext context, CancellationToken cancellationToken = default(CancellationToken)) => ...;
        public static Task<TValue?> GetFromJsonAsync<TValue>(this HttpClient client, string? requestUri, JsonTypeInfo<TValue> jsonTypeInfo, CancellationToken cancellationToken = default(CancellationToken)) => ...;
        public static Task<TValue?> GetFromJsonAsync<TValue>(this HttpClient client, System.Uri? requestUri, JsonTypeInfo<TValue> jsonTypeInfo, CancellationToken cancellationToken = default(CancellationToken)) => ...;
        public static Task<HttpResponseMessage> PostAsJsonAsync<TValue>(this HttpClient client, string? requestUri, TValue value, JsonTypeInfo<TValue> jsonTypeInfo, CancellationToken cancellationToken = default(CancellationToken)) => ...;
        public static Task<HttpResponseMessage> PostAsJsonAsync<TValue>(this HttpClient client, System.Uri? requestUri, TValue value, JsonTypeInfo<TValue> jsonTypeInfo, CancellationToken cancellationToken = default(CancellationToken)) => ...;
        public static Task<HttpResponseMessage> PutAsJsonAsync<TValue>(this HttpClient client, string? requestUri, TValue value, JsonTypeInfo<TValue> jsonTypeInfo, CancellationToken cancellationToken = default(CancellationToken)) => ...;
        public static Task<HttpResponseMessage> PutAsJsonAsync<TValue>(this HttpClient client, System.Uri? requestUri, TValue value, JsonTypeInfo<TValue> jsonTypeInfo, CancellationToken cancellationToken = default(CancellationToken)) => ...;
    }
    public static partial class HttpContentJsonExtensions
    {
        public static Task<object?> ReadFromJsonAsync(this HttpContent content, Type type, JsonSerializerContext context, CancellationToken cancellationToken = default(CancellationToken)) => ...;
        public static Task<T?> ReadFromJsonAsync<T>(this HttpContent content, JsonTypeInfo<TValue> jsonTypeInfo, CancellationToken cancellationToken = default(CancellationToken)) => ...;
    }
    public sealed partial class JsonContent : HttpContent
    {
        public static JsonContent<object?> Create(object? inputValue, Type inputType, JsonSerializerContext context, MediaTypeHeaderValue? mediaType = null) => ...;
        public static JsonContent<TValue> Create<TValue>(TValue? inputValue, JsonTypeInfo<TValue> jsonTypeInfo, MediaTypeHeaderValue? mediaType = null) => ...;
    }
    public sealed partial class JsonContent<TValue> : JsonContent
    {
        public TValue? Value { get }
    }
}
```

## How source generation provides benefits

### Improved serialization throughput

Pre-generating and using optimized serialization logic that honors only features needed in an app leads to increased performance over using `JsonSerializer`'s robust serialization logic. The serializer supports all features, which means there's more logic to tear through during serialization, which can show up during measurements.

#### Serializing POCOs

Given our `Person` type, we can [observe](https://gist.github.com/layomia/2c236f74b250ed06e0cac5435f78e2cc) that serialization is **~1.62x faster** when using the source generator.

|           Method |     Mean |   Error |  StdDev | Ratio | RatioSD | Gen 0 | Gen 1 | Gen 2 | Allocated |
|----------------- |---------:|--------:|--------:|------:|--------:|------:|------:|------:|----------:|
|       Serializer | 243.1 ns | 4.83 ns | 9.54 ns |  1.00 |    0.00 |     - |     - |     - |         - |
| SrcGenSerializer | 149.3 ns | 2.04 ns | 1.91 ns |  0.62 |    0.03 |     - |     - |     - |         - |

#### Serializing collections

Using the same `Person` type, we [observe](https://gist.github.com/layomia/2ff8c065f0f4933c4b22f7a277b03457) significant performance boosts when serializing arrays with different lengths, all while not allocating at all.

|           Method | Count |         Mean |       Error |      StdDev | Ratio |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|----------------- |------ |-------------:|------------:|------------:|------:|-------:|------:|------:|----------:|
|       Serializer |    10 |   2,392.5 ns |    17.42 ns |    13.60 ns |  1.00 | 0.0801 |     - |     - |     344 B |
| SrcGenSerializer |    10 |     989.4 ns |     6.74 ns |     5.62 ns |  0.41 |      - |     - |     - |         - |
|                  |       |              |             |             |       |        |       |       |           |
|       Serializer |   100 |  21,427.1 ns |   189.33 ns |   167.84 ns |  1.00 | 0.0610 |     - |     - |     344 B |
| SrcGenSerializer |   100 |  10,137.3 ns |   125.61 ns |   111.35 ns |  0.47 |      - |     - |     - |         - |
|                  |       |              |             |             |       |        |       |       |           |
|       Serializer |  1000 | 215,102.4 ns | 1,737.91 ns | 1,356.85 ns |  1.00 |      - |     - |     - |     344 B |
| SrcGenSerializer |  1000 | 104,970.5 ns |   345.48 ns |   288.49 ns |  0.49 |      - |     - |     - |         - |

#### TechEmpower caching benchmark

The [TechEmpower caching benchmark](https://www.techempower.com/benchmarks/#section=data-r20&hw=ph&test=cached-query) exercises a platform or framework's in-memory caching of information sourced from a database. In .NET's implementation of the benchmark, we perform JSON serialization of the cached data in order to send them as a response to the test harness. With JSON source generation, we've been able to significantly increase our performance in this benchmark.

|                          | Requests/sec |  Requests |
|--------------------------|--------------|-----------|
| net5.0                   |      243,000 | 3,669,151 |
| net6.0                   |      260,928 | 3,939,804 |
| net6.0 + JSON source gen |      364,224 | 5,499,468 |

We [observe](https://github.com/aspnet/Benchmarks/pull/1683) a ~100K RPS gain, which is a **~40% increase**.

### Faster startup & reduced private memory

Moving retrieval of type metadata from runtime to compile-time means that there is less work for the serializer to do on start-up, which leads to a reduction in the amount of time it takes to perform the first serialization or deserialization of each type. 

In previous versions of `System.Text.Json`, the serializer always used `Reflection.Emit` where possible to generate fast member accessors to constructors, properties, and fields. Generating these IL methods takes a non-trivial amount of time, but also consumes private memory. With source generators, we are able to generate code that that statically invokes these accessors. This eliminates time and allocation cost due to reflection.

Similarly, *all* serialization and deserialization of JSON data was was performed within `JsonConverter<T>` instances. The serializer would statically initialize several built-in converter instances to provide default functionality. User applications paid the cost of these allocations, even when only a few of these converters are needed given the input object graphs. With source generation, we can initialize only the converters that are needed by the types indicated to the generator (in the `JsonSourceGeneration.Metadata` mode), and also skip the use of converters entirely (in the `JsonSourceGeneration.Serialization` mode).

Using a simple `Message` type:

```cs
public class JsonMessage
{
    public string message { get; set; }
}
```

We can [observe](https://gist.github.com/layomia/77b91c8ea446b50efbcf358a19d73be1) startup improvements during serialization and deserialization.

#### Serialization

|                  | Elapsed time (ms) |  Allocated (KB) |
|------------------|-------------------|-----------------|
|       Serializer |             28.25 |         1110.00 |
| SrcGenSerializer |             12.75 |          563.00 |

#### Deserialization

|                  | Elapsed time (ms) |   Allocated (KB) |
|------------------|-------------------|------------------|
|       Serializer |             24.75 |          1457.00 |
| SrcGenSerializer |             15.50 |          1025.00 |

We see that that it takes substantially less time and lower allocations to perform the first (de)serialization for the type.

### Trim correctness

By eliminating runtime reflection, we avoid the primary coding pattern that is unfriendly to ILLinker analysis. Given that the reflection-based code is trimmed out, applications that use `System.Text.Json` go from having several ILLinker analysis warnings when trimming to having absolutely none. This means that applications that use the `System.Text.Json` source generator can be safely trimmed, provided there are no other warnings due to the user app itself, or other parts of the BCL.

### Reduced app size

By inspecting serializable types at compile-time instead of at runtime, two major things are done to reduce the size of the consuming application. First, we can detect which custom or built-in `JsonConverter<T>` types are needed in the application at runtime, and reference them statically in the generated metadata. This allows the ILLinker to trim out JSON converter types which will not be needed in the application at runtime. Similarly, inspecting input types at compile-time eliminates the need to do so at runtime. This removes the need for lots of `System.Reflection` APIs at runtime, so the ILLinker can trim code internal code in `System.Text.Json` which uses with those APIs. Unused source code further in the dependency graph is also trimmed out.

Size reductions are based on which `JsonSerializer` methods are used. If the new APIs that take pre-generated metadata are used, then you may observe size reductions in your app.

Using a simple console program that does a roundtrip serialization and deserialization of our `Message` type using JSON source generation, we can [observe](https://gist.github.com/layomia/7714dffe94d1d971332c57dc2d768462) a size reduction after trimming, when compared to the same program which does not use source generation.

|                  | App size (MB) | System.Text.Json.dll size (KB) |
|------------------|---------------|--------------------------------|
|       Serializer |          21.7 |                            989 |
| SrcGenSerializer |          20.8 |                            460 |

## Using source generated code in ASP.NET Core

### Blazor

In Blazor applications, pre-generated logic for serializable types can be forwarded to the serializer directly via the new APIs being added in the `System.Net.Http.Json` namespace. For example, to asynchronously deserialize a list of weather forecast objects from an `HttpClient`, you can use a new overload on the `HttpClient.GetFromJsonAsync` method:

```cs
[JsonSerializable(typeof(WeatherForecast[]))]
internal partial class MyJsonContext : JsonSerializerContext { }

@code {
    private WeatherForecast[] forecasts;

    private static JsonSerializerOptions Options = new(JsonSerializerDefaults.Web);
    private static MyJsonContext Context = new MyJsonContext(Options);

    protected override async Task OnInitializedAsync()
    {
        forecasts = await Http.GetFromJsonAsync("sample-data/weather.json", Context.WeatherForecastArray);
    }
}
```

### MVC, WebAPIs, SignalR, Houdini

These ASP.NET Core services can retrieve user-generated contexts with already-existing APIs to configure serialization options.

```cs
[JsonSerializable(typeof(WeatherForecast[]))]
internal partial class MyJsonContext : JsonSerializerContext { }

services.AddControllers().AddJsonOptions(options => options.AddContext<MyJsonContext>());
```

In the future, these services can expose APIs to directly take `JsonTypeInfo<T>` or `JsonSerializerContext` instances.

## Performing JSON serialization on behalf of others

Similar to the ASP.NET Core scenarios above, library or framework developers can provide new APIs that accept `JsonSerializerOptions` or `JsonSerializerContext` instances to forward to the serializer on behalf of the user.

## Versioning

You might be wondering why the output of the source generator is passed directly to `JsonSerializer` via new overloads, rather than say initialized implicitly on application start up and registered in a global cache where the serializer can retrieve it. One benefit is for app trimming. By providing dedicated entry-points for source-generated serializers and data access models, we are statically providing the serializer with all the information that it needs to serialize input types, which allows it to shed lots of unused code and dependencies (both reflection-related and otherwise). Another benefit is that it provides an opportunity for callers to consider versioning, that is whether the code generated for serializable types is in sync with the serializable types themselves. This is an important consideration when using source generation, particulaly for serialization.

Having source-generator output that does not match the corresponding serializable types can cause serious issues, including having data being accidentally included or excluded from serialization. These issues could be really hard to diagnose. The source generator design avoids patterns which can cause versioning issues such as app-global registration of generated artifacts. This sort of global registration can cause contention issues between serialization settings across different assemblies. Each assembly might use different versions of the source generator, potentially resulting in apps having different behavior in development and in production, depending on which set of generated artifacts (among the implementations in various assemblies) are selected by the serializer.

## Closing

Improving the performance of applications that use `System.Text.Json` has been a continuous process, and a primary goal of the library since its inception. Compile-time source generation has helped us increase performance and develop a new model of trim-safe serialization. We'd like you to try the source generator and give us feedback on how it affects the perf in your app, any usability issues, and any bugs you may find.

Community contributions are always welcome. If you'd like to contribute to `System.Text.Json`, check out our list of [`up-for-grabs` issues](https://github.com/dotnet/runtime/issues?q=is%3Aopen+is%3Aissue+label%3Aarea-System.Text.Json+label%3Aup-for-grabs) on GitHub.