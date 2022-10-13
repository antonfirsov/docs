---
post_title: What's new in System.Text.Json in .NET 7
summary: An overview of all .NET 7 features in System.Text.Json
featured_image: dotnet-bot_scene_juggling.png
post_slug: system-text-json-in-dotnet-7
author1: eitsarpa
author2: kwicher
author3: laakinri
microsoft_alias: eitsarpa
categories: .NET, JSON, Performance
tags: system.text.json, .net 7
desired_publication_date: 2022-10-13
post_date: 2022-10-13 08:15:00
---

In .NET 7, our focus for System.Text.Json has been to substantially improve extensibility of the library, adding new performance-oriented features and addressing high impact reliability and consistency issues. More specifically, .NET 7 sees the release of [contract customization](https://learn.microsoft.com/dotnet/standard/serialization/system-text-json/custom-contracts), which gives you more control over how types are serialized or deserialized, polymorphic serialization for user-defined [type hierarchies](https://learn.microsoft.com/dotnet/standard/serialization/system-text-json/polymorphism#serialize-properties-of-derived-classes), [required member support](https://learn.microsoft.com/dotnet/standard/serialization/system-text-json/required-properties), and much more.

### Getting the latest bits

You can try out the new features by using the latest build of [System.Text.Json NuGet package](https://www.nuget.org/packages/System.Text.Json) or the [latest SDK for .NET 7](https://dotnet.microsoft.com/download/dotnet/7.0), which is currently RC.

## [Contract Customization](https://github.com/dotnet/runtime/issues/63686)

System.Text.Json determines how a given .NET type is meant to be serialized and deserialized by constructing a JSON contract for that type. The contract is derived from the type's shape -- such as its available constructors, properties and fields, and whether it implements `IEnumerable` or `IDictionary` -- either at runtime using reflection or at compile time using the source generator. In previous releases, users were able to make limited adjustments to the derived contract using [System.Text.Json attribute annotations](https://learn.microsoft.com/dotnet/api/system.text.json.serialization.jsonattribute), assuming they are able to modify the type declaration.

The contract metadata for a given type `T` is represented using [`JsonTypeInfo<T>`](https://learn.microsoft.com/dotnet/api/system.text.json.serialization.metadata.jsontypeinfo-1), which in previous versions served as an opaque token used exclusively in source generator APIs. Starting in .NET 7, most facets of the `JsonTypeInfo` contract metadata have been exposed and made user-modifiable. Contract customization allows users to write their own JSON contract resolution logic using implementations of the [`IJsonTypeInfoResolver`](https://learn.microsoft.com/dotnet/api/system.text.json.serialization.metadata.ijsontypeinforesolver) interface:

```csharp
public interface IJsonTypeInfoResolver
{
    JsonTypeInfo? GetTypeInfo(Type type, JsonSerializerOptions options);
}
```

A contract resolver returns a configured `JsonTypeInfo` instance for the given `Type` and `JsonSerializerOptions` combination. It can return `null` if the resolver does not support metadata for the specified input type.

Contract resolution performed by the default, reflection-based serializer is now exposed via the [DefaultJsonTypeInfoResolver](https://learn.microsoft.com/dotnet/api/system.text.json.serialization.metadata.defaultjsontypeinforesolver) class, which implements `IJsonTypeInfoResolver`. This class lets users extend the default reflection-based resolution with custom modifications or combine it with other resolvers (such as source-generated resolvers).

Starting from .NET 7 the [JsonSerializerContext](https://learn.microsoft.com/dotnet/api/system.text.json.serialization.jsonserializercontext) class used in source generation also implements `IJsonTypeInfoResolver`. To learn more about the source generator, see [How to use source generation in System.Text.Json](https://learn.microsoft.com/dotnet/standard/serialization/system-text-json/source-generation).

A `JsonSerializerOptions` instance can be configured with a custom resolver using the new [`TypeInfoResolver`](https://learn.microsoft.com/dotnet/api/system.text.json.jsonserializeroptions.typeinforesolver) property:

```C#
// configure to use reflection contracts
var reflectionOptions = new JsonSerializerOptions 
{ 
    TypeInfoResolver = new DefaultJsonTypeInfoResolver()
};

// configure to use source generated contracts
var sourceGenOptions = new JsonSerializerOptions 
{ 
    TypeInfoResolver = EntryContext.Default 
};

[JsonSerializable(typeof(MyPoco))]
public partial class EntryContext : JsonSerializerContext { }
```

### Modifying JSON contracts

The `DefaultJsonTypeInfoResolver` class is designed with contract customization in mind and can be extended in a couple of ways:

1. Inheriting from the class, overriding the [`GetTypeInfo`](https://learn.microsoft.com/dotnet/api/system.text.json.serialization.metadata.defaultjsontypeinforesolver.gettypeinfo) method, or
2. Using the [`Modifiers`](https://learn.microsoft.com/dotnet/api/system.text.json.serialization.metadata.defaultjsontypeinforesolver.modifiers) property to subscribe delegates that modify the default `JsonTypeInfo` results.

Here's how you could define a custom contract resolver that uses uppercase JSON properties, first using inheritance:

```C#
var options = new JsonSerializerOptions
{
    TypeInfoResolver = new UpperCasePropertyContractResolver()
};

JsonSerializer.Serialize(new { value = 42 }, options); // {"VALUE":42}

public class UpperCasePropertyContractResolver : DefaultJsonTypeInfoResolver
{
    public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
    {
        JsonTypeInfo typeInfo = base.GetTypeInfo(type, options);

        if (typeInfo.Kind == JsonTypeInfoKind.Object)
        {
            foreach (JsonPropertyInfo property in typeInfo.Properties)
            {
                property.Name = property.Name.ToUpperInvariant();
            }
        }

        return typeInfo;
    }
}
```

and the same implementation using the `Modifiers` property:

```C#
var options = new JsonSerializerOptions
{
    TypeInfoResolver = new DefaultJsonTypeInfoResolver
    {
        Modifiers = { UseUppercasePropertyNames }
    }
};

JsonSerializer.Serialize(new { value = 42 }, options); // {"VALUE":42}

static void UseUppercasePropertyNames(JsonTypeInfo jsonTypeInfo)
{
    if (typeInfo.Kind != JsonTypeInfoKind.Object)
        return;

    foreach (JsonPropertyInfo property in typeInfo.Properties)
    {
        property.Name = property.Name.ToUpperInvariant();
    }
}
```

Each `DefaultJsonTypeInfoResolver` can register multiple modifier delegates, which it runs sequentially on metadata resolved for each type:

```C#
var options = new JsonSerializerOptions
{
    TypeInfoResolver = new DefaultJsonTypeInfoResolver
    {
        Modifiers = { Modifier1, Modifier2, Modifier3 }
    }
};

JsonTypeInfo typeInfo = options.GetTypeInfo(typeof(string)); // Prints messages in sequence

static void Modifier1(JsonTypeInfo typeInfo) { Console.WriteLine("Runs first"); }
static void Modifier2(JsonTypeInfo typeInfo) { Console.WriteLine("Runs second"); }
static void Modifier3(JsonTypeInfo typeInfo) { Console.WriteLine("Runs third"); }
```

Modifier syntax affords a more concise and compositional API compared to inheritance. As such, subsequent examples will focus on that approach.

#### Notes on authoring contract modifiers

The `DefaultJsonTypeInfoResolver.Modifiers` property allows developers to specify a series of handlers to update the metadata contract for all types in the serialization type closure. Modifiers are consulted in the order that they were specified in the `DefaultJsonTypeInfoResolver.Modifiers` list. This makes it possible for modifiers to make changes that conflict with each other, possibly resulting in unintended serialization contracts.

For example, consider how the `UseUppercasePropertyNames` modifier above would interact with a modifier that filters out properties that are only used in a hypothetical "v0" version of a serialization schema:

```cs
JsonSerializerOptions options0 = new()
{
    TypeInfoResolver = new DefaultJsonTypeInfoResolver
    { 
        Modifiers = { ExcludeV0Members, UseUppercasePropertyNames } 
    }
};

JsonSerializerOptions options1 = new()
{
    TypeInfoResolver = new DefaultJsonTypeInfoResolver 
    {
        Modifiers = { UseUppercasePropertyNames, ExcludeV0Members }
    }
};

Employee employee = FetchEmployee();
JsonSerializer.Serialize(employee, options0); // {"NAME":"Jane Doe","ROLE":"Contractor"}
JsonSerializer.Serialize(employee, options1); // {"NAME":"Jane Doe","ROLE":"Contractor","ROLE_V0":"Temp"}

static void ExcludeV0Members(JsonTypeInfo jsonTypeInfo)
{
    if (typeInfo.Kind != JsonTypeInfoKind.Object)
        return;

    foreach (JsonPropertyInfo property in typeInfo.Properties)
    {
        if (property.Name.EndsWith("_v0"))
        {
            property.ShouldSerialize = false;
        }
    }
}

class Employee
{
    public string Name { get; set; }
    public Role Role { get; set; }
    public string Role_v0 => { get; set; }
}
```

Even though not always possible, it is a good idea to design modifiers with composability in mind. One way to do this is by ensuring that any modifications to the contract are append-only, rather than replacing or removing work done by previous modifiers. Consider the following example, where we define a modifier that appends a property with diagnostic information for every object. Such a modifier might want to confirm that a "Data" property does not already exist on a given type:

```cs
static void AddDiagnosticDataProperty(JsonTypeInfo typeInfo)
{
    if (typeInfo.Kind == JsonTypeInfoKind.Object && 
        typeInfo.Properties.All(prop => prop.Name != "Data"))
    {
        JsonPropertyInfo propertyInfo = typeInfo.CreateJsonPropertyInfo(string, "Data");
        propertyInfo.Get = obj => GetDiagnosticData(obj);
        typeInfo.Properties.Add(propertyInfo);
    }
}
```

### Example: Custom Attribute Support

Contract customization makes it possible to add support for attributes that are not inbox to `System.Text.Json`. Here is an example implementing support for `DataContractSerializer`'s [`IgnoreDataMemberAttribute`](https://learn.microsoft.com/dotnet/api/system.runtime.serialization.ignoredatamemberattribute):

```C#
var options = new JsonSerializerOptions
{
    TypeInfoResolver = new DefaultJsonTypeInfoResolver
    {
        Modifiers = { DetectIgnoreDataMemberAttribute }
    }
};

JsonSerializer.Serialize(new MyPoco(), options); // {"Value":3}

static void DetectIgnoreDataMemberAttribute(JsonTypeInfo typeInfo)
{
    if (typeInfo.Kind != JsonTypeInfoKind.Object)
        return;

    foreach (JsonPropertyInfo propertyInfo in typeInfo.Properties)
    {
        if (propertyInfo.AttributeProvider is ICustomAttributeProvider provider &&
            provider.IsDefined(typeof(IgnoreDataMemberAttribute), inherit: true))
        {
            // Disable both serialization and deserialization 
            // by unsetting getter and setter delegates
            propertyInfo.Get = null;
            propertyInfo.Set = null;
        }
    }
}

public class MyPoco
{
    [JsonIgnore]
    public int JsonIgnoreValue { get; } = 1;

    [IgnoreDataMember]
    public int IgnoreDataMemberValue { get; } = 2;

    public int Value { get; } = 3;
}
```

### Example: Conditional Serialization

You can use the [`JsonPropertyInfo.ShouldSerialize`](https://learn.microsoft.com/dotnet/api/system.text.json.serialization.metadata.jsonpropertyinfo.shouldserialize) delegate to determine dynamically whether a given property value should be serialized:

```C#
var options = new JsonSerializerOptions
{
    TypeInfoResolver = new DefaultJsonTypeInfoResolver
    {
        Modifiers = { IgnoreNegativeValues }
    }
};

JsonSerializer.Serialize(new MyPoco { IgnoreNegativeValues = false, Value = -1 }, options); // {"Value":-1}
JsonSerializer.Serialize(new MyPoco { IgnoreNegativeValues = true, Value = -1 }, options); // {}

static void IgnoreNegativeValues(JsonTypeInfo typeInfo)
{
    if (typeInfo.Type != typeof(MyPoco))
        return;

    foreach (JsonPropertyInfo propertyInfo in typeInfo.Properties)
    {
        if (propertyInfo.PropertyType == typeof(int))
        {
            propertyInfo.ShouldSerialize = static (obj, value) => 
                (int)value! >= 0 || !((MyPoco)obj).IgnoreNegativeValues;
        }
    }
}

public class MyPoco
{
    [JsonIgnore]
    public bool IgnoreNegativeValues { get; set; }

    public int Value { get; set; }
}
```

### Example: Serializing private fields

System.Text.Json does not support private field serialization, as doing that is generally not recommended. However, if really necessary it's possible to write a contract resolver that only serializes the fields of a given type:

```C#
var options = new JsonSerializerOptions
{
    TypeInfoResolver = new DefaultJsonTypeInfoResolver
    {
        Modifiers = { SerializeObjectFields }
    }
};

JsonSerializer.Serialize(new { value = 42 }, options); // {"\u003Cvalue\u003Ei__Field":42}

static void SerializeObjectFields(JsonTypeInfo typeInfo)
{
    if (typeInfo.Kind != JsonTypeInfoKind.Object)
        return;

    // Remove any properties included by the default resolver
    typeInfo.Properties.Clear();

    const BindingFlags Flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
    foreach (FieldInfo fieldInfo in typeInfo.Type.GetFields(Flags))
    {
        JsonPropertyInfo propertyInfo = typeInfo.CreateJsonPropertyInfo(fieldInfo.FieldType, fieldInfo.Name);
        propertyInfo.Get = obj => fieldInfo.GetValue(obj);
        propertyInfo.Set = (obj, value) => fieldInfo.SetValue(obj, value);

        typeInfo.Properties.Add(propertyInfo);
    }
}
```

Although as the serialized output would suggest, serializing private fields can be very brittle and error prone. This example has been shared for illustrative purposes only and is not recommended for most real-word applications.

### Combining resolvers

It is possible to combine contracts from multiple sources using the [`JsonTypeInfoResolver.Combine`](https://learn.microsoft.com/dotnet/api/system.text.json.serialization.metadata.jsontypeinforesolver.combine) method. This is commonly applicable to source generated `JsonSerializerContext` instances that can only generate contracts for a restricted subset of types:

```C#
var options = new JsonSerializerOptions
{
    TypeInfoResolver = JsonTypeInfoResolver.Combine(ContextA.Default, ContextB.Default)
};

// Successfully handles serialization for both types
JsonSerializer.Serialize(new PocoA(), options);
JsonSerializer.Serialize(new PocoB(), options);

// Types from Library A
[JsonSerializable(typeof(PocoA))]
public partial class ContextA : JsonSerializerContext { }
public class PocoA { }

// Types from Library B
[JsonSerializable(typeof(PocoB))]
public partial class ContextB : JsonSerializerContext { }
public class PocoB { }
```

The `Combine` method produces a contract resolver that sequentially queries each of its constituent resolvers in order of definition, returning the first result that is not `null`. The method supports chaining arbitrarily many resolvers, including `DefaultJsonTypeInfoResolver`:

```C#
var options = new JsonSerializerOptions
{
    TypeInfoResolver = JsonTypeInfoResolver.Combine(
        ContextA.Default, 
        ContextB.Default, 
        new DefaultJsonTypeInfoResolver())
};

// Uses reflection for the outer class, source gen for the property types
JsonSerializer.Serialize(new { A = new PocoA(), B = new PocoB() } , options);
```

### Metadata Kinds

`JsonTypeInfo` metadata should be thought of as configuration objects for System.Text.Json's built-in converters. As such, what metadata is configurable on each type is largely dependent on the underlying converter being used for the type: types using the built-in converter for objects can configure property metadata, whereas types using custom user-defined converters cannot be configured at all. What can be configured is determined by the value of the [`JsonTypeInfo.Kind`](https://learn.microsoft.com/dotnet/api/system.text.json.serialization.metadata.jsontypeinfo.kind) property. There are currently four kinds of metadata available to the user:

- `Object` - type is serialized as a JSON object using `JsonPropertyInfo` metadata; used for most `class` or `struct` types by default.
- `Enumerable` - type is serialized as a JSON array; used for most types implementing `IEnumerable`.
- `Dictionary` - type is serialized as a JSON object; used for most dictionary types or collections of key/value pairs.
- `None` - type uses a converter not configurable by metadata; typically applies to primitive types, `object`, `string` or types using custom user-defined converters.

Currently, the `Object` kind offers the most configurability, and we plan to add more functionality for `Enumerable` and `Dictionary` kinds in future releases.

## [Type Hierarchies](https://github.com/dotnet/runtime/issues/63747)

System.Text.Json now supports polymorphic serialization and deserialization of user-defined type hierarchies. This can be enabled by decorating the base class of a type hierarchy with the new [`JsonDerivedTypeAttribute`](https://docs.microsoft.com/dotnet/api/system.text.json.serialization.jsonderivedtypeattribute):

```csharp
[JsonDerivedType(typeof(Derived))]
public class Base
{
    public int X { get; set; }
}

public class Derived : Base
{
    public int Y { get; set; }
}
```

This configuration enables polymorphic serialization for `Base`, specifically when the run-time type is `Derived`:

```csharp
Base value = new Derived();
JsonSerializer.Serialize<Base>(value); // { "X" : 0, "Y" : 0 }
```

Note that this does not enable polymorphic _deserialization_ since the payload would be round tripped as `Base`:

```C#
Base value = JsonSerializer.Deserialize<Base>(@"{ ""X"" : 0, ""Y"" : 0 }");
value is Derived; // false
```

### Using Type Discriminators

To enable polymorphic _deserialization_, you need to specify a _type discriminator_ for the derived class:

```C#
[JsonDerivedType(typeof(Base), typeDiscriminator: "base")]
[JsonDerivedType(typeof(Derived), typeDiscriminator: "derived")]
public class Base
{
    public int X { get; set; }
}

public class Derived : Base
{
    public int Y { get; set; }
}
```

Now, type discriminator metadata is emitted in the JSON:

```C#
Base value = new Derived();
JsonSerializer.Serialize<Base>(value); // { "$type" : "derived", "X" : 0, "Y" : 0 }
```

The presence of the metadata enables the value to be polymorphically deserialized:

```C#
Base value = JsonSerializer.Deserialize<Base>(@"{ ""$type"" : ""derived"", ""X"" : 0, ""Y"" : 0 }");
value is Derived; // true
```

Type discriminator identifiers can also be integers, so the following form is valid:

```C#
[JsonDerivedType(typeof(Derived1), 0)]
[JsonDerivedType(typeof(Derived2), 1)]
[JsonDerivedType(typeof(Derived3), 2)]
public class Base { }

JsonSerializer.Serialize<Base>(new Derived2()); // { "$type" : 1, ... }
```

### Configuring Polymorphism

You can tweak aspects of how polymorphic serialization works using the [`JsonPolymorphicAttribute`](https://docs.microsoft.com/dotnet/api/system.text.json.serialization.jsonpolymorphicattribute). The following example changes the property name of the type discriminator:

```C#
[JsonPolymorphic(TypeDiscriminatorPropertyName = "$case")]
[JsonDerivedType(typeof(Derived), "derived")]
public class Base { }

JsonSerializer.Serialize<Base>(new Derived2()); // { "$case" : "derived", ... }
```

The `JsonPolymorphicAttribute` exposes a number of configuration parameters, including properties controlling how undeclared runtime types or type discriminators should be handled on serialization and deserialization, respectively.

### Polymorphism using Contract Customization

The `JsonTypeInfo` contract model exposes the [`PolymorphismOptions`](https://docs.microsoft.com/dotnet/api/system.text.json.serialization.metadata.jsontypeinfo.polymorphismoptions) property that can be used to programmatically control all configuration of a given type hierarchy:

```C#
var options = new JsonSerializerOptions
{
    TypeInfoResolver = new DefaultJsonTypeInfoResolver
    {
        Modifiers =
        {
            static typeInfo =>
            {
                if (typeInfo.Type != typeof(Base))
                    return;

                typeInfo.PolymorphismOptions = new()
                {
                    TypeDiscriminatorPropertyName = "__type",
                    DerivedTypes =
                    {
                        new JsonDerivedType(typeof(Derived), "__derived")
                    }
                };
            }
        }
    }
};

JsonSerializer.Serialize<Base>(new Derived(), options); // {"__type":"__derived", ... }
```

## [Required Members](https://github.com/dotnet/runtime/issues/29861)

C# 11 adds support for [`required` members](https://learn.microsoft.com/dotnet/csharp/whats-new/csharp-11#required-members), a language feature that lets class authors specify properties or fields that _must_ be populated on instantiation. Starting in .NET 7, the reflection serializer in System.Text.Json includes support for `required` members: if the member of a deserialized type is marked `required` and cannot bind to any property from the JSON payload, deserialization will fail with an exception:

```csharp
using System.Text.Json;

JsonSerializer.Deserialize<Person>("""{"Age": 42}"""); // throws JsonException

public class Person
{
    public required string Name { get; set; }
    public int Age { get; set; }
}
```

It should be noted that `required` properties are currently not supported by the source generator. If you're using the source generator, an earlier C# version, a different .NET language like F# or Visual Basic, or simply need to avoid the `required` keyword, you can alternatively use the [`JsonRequiredAttribute`](https://learn.microsoft.com/dotnet/api/system.text.json.serialization.jsonrequiredattribute) to achieve the same effect.

```csharp
using System.Text.Json;

JsonSerializer.Deserialize("""{"Age": 42}""", MyContext.Default.Person); // throws JsonException

[JsonSerializable(typeof(Person))]
public partial class MyContext : JsonSerializerContext { }

public class Person
{
    [JsonRequired]
    public string Name { get; set; }
    public int Age { get; set; }
}
```

It is also possible to control whether a property is required via the contract model using the [`JsonPropertyInfo.IsRequired`](https://learn.microsoft.com/dotnet/api/system.text.json.serialization.metadata.jsonpropertyinfo.isrequired) property:

```C#
var options = new JsonSerializerOptions
{
    TypeInfoResolver = new DefaultJsonTypeInfoResolver
    {
        Modifiers =
        {
            static typeInfo =>
            {
                if (typeInfo.Kind != JsonTypeInfoKind.Object)
                    return;

                foreach (JsonPropertyInfo propertyInfo in typeInfo.Properties)
                {
                    // strip IsRequired constraint from every property
                    propertyInfo.IsRequired = false;
                }
            }
        }
    }
};

JsonSerializer.Deserialize<Person>("""{"Age": 42}""", options); // serialization now succeeds
```

### [JsonSerializerOptions.Default](https://github.com/dotnet/runtime/issues/61093)

System.Text.Json maintains a default instance of `JsonSerializerOptions` to be used in cases where no `JsonSerializerOptions` argument has been passed by the user. This (read-only) instance can now be accessed by users via the `JsonSerializerOptions.Default` static property. It can be useful in cases where users need to query the default `JsonTypeInfo` or `JsonConverter` for a given type:

```C#
public class MyCustomConverter : JsonConverter<int>
{
    private readonly static JsonConverter<int> s_defaultConverter = 
        (JsonConverter<int>)JsonSerializerOptions.Default.GetConverter(typeof(int));

    // custom serialization logic
    public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
    {
        return writer.WriteStringValue(value.ToString());
    }

    // fall back to default deserialization logic
    public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return s_defaultConverter.Read(ref reader, typeToConvert, options);
    }
}
```

Or in cases where obtaining a `JsonSerializerOptions` is required:

```C#
class MyClass
{
    private readonly JsonSerializerOptions _options;

    public MyClass(JsonSerializerOptions? options = null) => _options = options ?? JsonSerializerOptions.Default;
}
```

## [Utf8JsonReader.CopyString](https://github.com/dotnet/runtime/issues/54410)

Until today, [`Utf8JsonReader.GetString()`](https://learn.microsoft.com/dotnet/api/system.text.json.utf8jsonreader.getstring?view=net-6.0) has been the only way users could consume decoded JSON strings. This will always allocate a new string, which might be unsuitable for certain performance-sensitive applications. The newly included `CopyString` methods allow copying the unescaped UTF-8 or UTF-16 strings to a buffer owned by the user:

```C#
int valueLength = reader.HasReadOnlySequence 
    ? checked((int)ValueSequence.Length) 
    : ValueSpan.Length;

char[] buffer = ArrayPool<char>.Shared.Rent(valueLength);
int charsRead = reader.CopyString(buffer);
ReadOnlySpan<char> source = buffer.Slice(0, charsRead);

ParseUnescapedString(source); // handle the unescaped JSON string
ArrayPool<char>.Shared.Return(buffer);
```

Or if handling UTF-8 is preferable:

```C#
ReadOnlySpan<byte> source = stackalloc byte[0];
if (!reader.HasReadOnlySequence && !reader.ValueIsEscaped)
{
    // No need to copy to an intermediate buffer if value is span without escape sequences
    source = reader.ValueSpan;
}
else
{
    int valueLength = reader.HasReadOnlySequence 
        ? checked((int)ValueSequence.Length) 
        : ValueSpan.Length;

    Span<byte> buffer = valueLength <= 256 ? stackalloc byte[256] : new byte[valueLength];
    int bytesRead = reader.CopyString(buffer);
    source = buffer.Slice(0, bytesRead);
}

ParseUnescapedBytes(source);
```

## Source generation improvements

The System.Text.Json source generator now adds built-in support for the following types:

* [`IAsyncEnumerable<T>`](https://github.com/dotnet/runtime/issues/59268),
* [`JsonDocument`](https://github.com/dotnet/runtime/issues/59954) and 
* [`DateOnly`/`TimeOnly`](https://github.com/dotnet/runtime/issues/53539)

The following example now works as expected:

```C#
Stream stdout = Console.OpenStandardOutput();
MyPoco value = new MyPoco();
await JsonSerializer.SerializeAsync(stdout, value, MyContext.Default.MyPoco);

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(MyPoco))]
public partial class MyContext : JsonSerializerContext
{
}

public class MyPoco
{
    public IAsyncEnumerable<DateOnly> Dates => GetDatesAsync();

    private async IAsyncEnumerable<DateOnly> GetDatesAsync()
    {
        DateOnly date = DateOnly.Parse("2022-09-01", CultureInfo.InvariantCulture);
        for (int i = 0; i < 10; i++)
        {
            await Task.Delay(1000);
            yield return date.AddDays(i);
        }
    }
}
```

## Performance improvements

.NET 7 sees the release of the fastest System.Text.Json yet. We have invested in a number of performance-oriented improvements concerning both the internal implementation and user-facing APIs. For a detailed write-up of System.Text.Json performance improvements in .NET 7, please refer to the [relevant section](https://devblogs.microsoft.com/dotnet/performance_improvements_in_net_7/#json) in Stephen Toub's Performance Improvements in .NET 7 article.

## Breaking changes

As part of our efforts to make System.Text.Json more reliable and consistent, the .NET 7 release includes a number of necessary breaking changes. These typically concern rectifying inconsistencies of components shipped in earlier releases of the library. We have documented each of the breaking changes, including potential workarounds should these impact migration of your apps to .NET 7:

* [JsonSerializerOptions copy constructor includes JsonSerializerContext](https://learn.microsoft.com/dotnet/core/compatibility/serialization/7.0/jsonserializeroptions-copy-constructor)
* [Polymorphic serialization for object types](https://learn.microsoft.com/dotnet/core/compatibility/serialization/7.0/polymorphic-serialization)
* [System.Text.Json source generator fallback](https://learn.microsoft.com/dotnet/core/compatibility/serialization/7.0/reflection-fallback)

## Closing

Our focus for System.Text.Json this year has been to make the library more extensible and improve its consistency and reliability, while also committing to continually improving performance year-on-year. We'd like you to try the new features and give us feedback on how it improves your applications, and any usability issues or bugs that you might encounter.

Community contributions are always welcome. If you'd like to contribute to System.Text.Json, check out our list of [`help wanted` issues](https://github.com/dotnet/runtime/issues?q=is%3Aopen+is%3Aissue+label%3A%22help+wanted%22) on GitHub.