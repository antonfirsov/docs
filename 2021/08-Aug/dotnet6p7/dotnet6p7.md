---
post_title: Announcing .NET 6 Preview 7
username: rlander@microsoft.com
microsoft_alias: rlander@microsoft.com
categories: .NET Core, .NET
desired_publication_date: 08/10/2021
summary: .NET 6 Preview 7 is now available.
---

We are delighted to release .NET 6 Preview 7. It is the last preview before we enter the (two) RC period. The team has been burning the midnight oil and the candle at both ends getting the last set of features in before we slow the speed down on the release. It's also the release where you will see the last bit of polish on various features and large it-took-the-whole-release features come in all at once. From this point, the team will be focused on bringing all the features to uniform (high) quality so that .NET 6 is ready for your production workloads.

On the topic of production workloads, it's worth reminding everyone that both the [.NET website](https://dotnet.microsoft.com) and [Bing.com](https://bing.com) have been running on .NET 6 since Preview 1. We're in talks with various teams (Microsoft and otherwise) about going into production with the .NET 6 RCs. If you are interested in that and want guidance on how to approach that, please reach out at dotnet@microsoft.com. We're always happy to talk to early adopters.

You can [download .NET 6 Preview 7](https://dotnet.microsoft.com/download/dotnet/6.0) for Linux, macOS, and Windows.

* [Installers and binaries](https://dotnet.microsoft.com/download/dotnet/6.0)
* [Container images](https://hub.docker.com/_/microsoft-dotnet)
* [Linux packages](https://github.com/dotnet/core/blob/main/release-notes/6.0/install-linux.md)
* [Release notes](https://github.com/dotnet/core/blob/main/release-notes/6.0/README.md)
* [API diff](https://github.com/dotnet/core/tree/main/release-notes/6.0/preview/api-diff/preview7)
* [Known issues](https://github.com/dotnet/core/blob/main/release-notes/6.0/known-issues.md)
* [GitHub issue tracker](https://github.com/dotnet/core/issues/6141)

See the [.NET MAUI](https://devblogs.microsoft.com/dotnet), [ASP.NET Core](https://devblogs.microsoft.com/aspnet/), and [EF Core](https://devblogs.microsoft.com/dotnet) posts for more detail on what’s new for client, web, and data access application scenarios.

.NET 6 Preview 7 has been tested and is supported with [Visual Studio 2022 Preview 2](https://visualstudio.microsoft.com/vs/preview/vs2022/). Visual Studio 2022 enables you to leverage the Visual Studio tools developed for .NET 6 such as development in .NET MAUI, Hot Reload for C# apps, new Web Live Preview for WebForms, and other performance improvements in your IDE experience. .NET 6 is also supported with [Visual Studio for Mac 8.9](https://visualstudio.microsoft.com/vs/mac/).

Check out the new [conversations posts](https://devblogs.microsoft.com/dotnet/category/conversations/) for in-depth engineer-to-engineer discussions on the latest .NET features.

## .NET SDK: C# project templates modernized

We [updated .NET SDK templates](https://github.com/dotnet/templating/issues/3359) to use the latest C# language features and patterns. We hadn't revisited the templates in terms of new language features in a while. It was time to do that and we'll ensure that the templates use new and modern features going forward.

The following language features are used in the new templates:

- Top-level statements
- async Main
- Global using directives (via SDK driven defaults)
- File-scoped namespaces
- Target-typed new expressions
- Nullable reference types

You might wonder why we enable certain features via templates instead of enabling them by default when a project targets .NET 6. We're OK with requiring some amount of work on your part to upgrade applications to a new version of .NET as a tradeoff for improving the default behavior of the platform. This allows us improve the product without complicating project files over time. However, some features can be quite disruptive with that model, such as nullable reference types. We don't want to tie those features to the upgrade experience, but want to leave that choice to you, both when and if ever. The templates are a much lower risk pivot point, where we're able to set what the new "good default model" is for new code without nearly as much downstream consequence. By enabling these features via project templates, we're getting the best of both worlds: new code starts with these features enabled but existing code isn't impacted when you upgrade.

### Console template

The `console` template demonstrates the biggest change. It's now (effectively) a one-liner by virtue of top-level statements and global using directives.

```csharp
// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
```

The .NET 5 version of the same template includes several lines of familiar ceremony that provide the structure previously necessary for even a single line of actual code.

```csharp
using System;

namespace Company.ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }
    }
}
```

The project file for the `console` template has also changed, to enable the [nullable reference types](https://docs.microsoft.com/dotnet/csharp/nullable-references) feature, as you can see in the following example.

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net6.0</TargetFramework>
    <Nullable>enable</Nullable>
  </PropertyGroup>

</Project>
```

Other templates also enable nullability, implicit global usings, and file scoped namespaces, including ASP.NET Core and Class Library.

### ASP.NET web template

The `web` template is also similarly reduced in lines of code, using the same features.

```csharp
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.MapGet("/", () => "Hello World!");

app.Run();
```

### ASP.NET MVC template

The `mvc` template is similar in structure. In this case, we've merged `Program.cs` and `Startup.cs` into a single file (`Program.cs`), creating a further simplification.

```csharp
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
```

### Template compatibility

See the following documents for compatibility concerns with using the new templates.

* [C# code in templates not supported by earlier .NET versions](https://docs.microsoft.com/dotnet/core/compatibility/sdk/6.0/csharp-template-code)
* [Implicit namespace imports](https://docs.microsoft.com/dotnet/core/compatibility/sdk/6.0/implicit-namespaces)

## Libraries: Reflection APIs for nullability information

[Nullable reference types](https://docs.microsoft.com/dotnet/csharp/nullable-references) is an important feature for writing reliable code. It works great for writing code but not (until now) for inspecting it. [New Reflection APIs](https://github.com/dotnet/runtime/issues/29723) enable you to determine the nullability nature of parameters and return values for a given method. These new APIs will be critical for Reflection-based tools and serializers, for example.

For context, we added [nullable annotations to the .NET Libraries](https://devblogs.microsoft.com/dotnet/announcing-net-5-0/#nullability-annotation-improvements) in .NET 5 and are in the process of doing same with [ASP.NET Core](https://github.com/dotnet/aspnetcore/issues/27389) this release. We also see [developers adopting nullability](https://github.com/jellyfin/jellyfin/blob/c07e83fdf87e61f30e4cca4e458113ac315918ae/Directory.Build.props#L5) for their projects.

Nullability information is persisted in [metadata using custom attributes](https://github.com/dotnet/roslyn/blob/main/docs/features/nullable-metadata.md). In principle, anyone can already read the custom attributes, however, this is not ideal because the encoding is non-trivial to consume.

The following examples demonstrate using the new APIs for a couple different scenarios.

### Getting top-level nullability information

Imagine you're implementing a serializer. Using these new APIs the serializer can check whether a given property can be set to `null`:

```C#
private NullabilityInfoContext _nullabilityContext = new NullabilityInfoContext();

private void DeserializePropertyValue(PropertyInfo p, object instance, object? value)
{
    if (value is null)
    {
        var nullabilityInfo = _nullabilityContext.Create(p);
        if (nullabilityInfo.WriteState is not NullabilityState.Nullable)
        {
            throw new MySerializerException($"Property '{p.GetType().Name}.{p.Name}'' cannot be set to null.");
        }
    }

    p.SetValue(instance, value);
}
```

### Getting nested nullability information

Nullability has special treatment for objects that can (formally) hold other objects, like arrays and tuples. For example, you can specify that an array object (as a variable, or as part of a type member signature) must be non-null but that the elements can be null, or vice versa. This extra level of specificity is inspectable with the new Reflection APIs, as you see demonstrated in the following example.

```C#
class Data
{
    public string?[] ArrayField;
    public (string?, object) TupleField;
}
private void Print()
{
    Type type = typeof(Data);
    FieldInfo arrayField = type.GetField("ArrayField");
    FieldInfo tupleField = type.GetField("TupleField");

    NullabilityInfoContext context = new ();

    NullabilityInfo arrayInfo = context.Create(arrayField);
    Console.WriteLine(arrayInfo.ReadState);        // NotNull
    Console.WriteLine(arrayInfo.Element.State);    // Nullable

    NullabilityInfo tupleInfo = context.Create(tupleField);
    Console.WriteLine(tupleInfo.ReadState);                      // NotNull
    Console.WriteLine(tupleInfo.GenericTypeArguments [0].State); // Nullable
    Console.WriteLine(tupleInfo.GenericTypeArguments [1].State); // NotNull
}
```

## Libraries: ZipFile Respects Unix File Permissions

The `System.IO.Compression.ZipFile` class now captures Unix file permissions during create and set file permissions when extracting zip archives on Unix-like operating systems. This change allows for executable files to be round-tripped through a zip archive, which means you no longer have to modify file permissions to make files executable after extracting a zip archive. It also respects the read/write permissions for `user`, `group`, and `other` as well.

If a zip archive doesn't contain file permissions (because it was created on Windows, or using a tool which didn't capture the permissions, like an earlier version of .NET) extracted files get the default file permissions, just like any other newly created file.

The Unix file permissions work with other zip archive tools as well, including:

* [Info-ZIP](https://sourceforge.net/projects/infozip/)
* [7-Zip](https://www.7-zip.org/)

## Early .NET 7 Feature Preview: Generic Math

For .NET 6, we've built the capability to [mark APIs as "in preview"](https://github.com/dotnet/designs/blob/main/accepted/2021/preview-features/preview-features.md). This new approach will allow us to offer and evolve preview features across multiple major releases. In order to use preview APIs, projects needs to explicitly opt-into using preview features. If you use preview features without explicitly opting-in, you will see build errors with actionable messages, starting in .NET 6 RC1. Preview features are expected to change, likely in breaking ways, in later releases. That's why they are opt-in.

One of those features we're previewing in .NET 6 is static abstract interface members. Those allow you to define static abstract methods (including operators) in interfaces. For example, it is now possible to implement algebraic generic methods. For some folks, this feature will be the absolute standout improvement we're delivering this year. It is perhaps the most important new type system capability since `Span<T>`.

The following example takes an `IEnumerable<T>` and is able to sum all the values due to the `T` being constrained to `INumber<T>`, possibly an `INumber<int>`.

```csharp
public static T Sum<T>(IEnumerable<T> values)
    where T : INumber<T>
{
    T result = TSelf.Zero;

    foreach (var value in values)
    {
        result += value;
    }

    return result;
}
```

This works because [`INumber<T>`](https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/INumber.cs) defines various (static)[operator overloads](https://docs.microsoft.com/dotnet/csharp/language-reference/operators/operator-overloading) that must be satisfied by interface implementors. The [`IAdditionOperators`](https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/IAdditionOperators.cs) is perhaps the easiest new interface to understand, which `INumber<T>` itself is derived from.

This is all powered by a new feature which allows `static abstract` members to be declared in interfaces. This enables interfaces to expose operators and other static methods, such as `Parse` or `Create`, and for those to be implemented by a derived type. Please see our associated blog post for more details!

All of the features mentioned are in preview for .NET 6 and not supported for use in production. We would appreciate your feedback using them. We intend to continue evolving and improving the generic math features and the runtime and C# features that support them in .NET 7. We expect to make breaking changes to the current experience, and that's part of the reason why the new APIs are marked as "in preview".

## Libraries: NativeMemory APIs

We added new [native memory allocation APIs](https://github.com/dotnet/runtime/pull/54006) exposed via `System.Runtime.InteropServices.NativeMemory`. These APIs represent equivalents to the `malloc`, `free`, `realloc`, and `calloc` C APIs and also includes APIs for doing aligned allocations.

You may be wondering about how to think about these APIs. First, they are low-level APIs that are intended for low-level code and algorithms. Application developers would rarely if ever use these. Another way to think about these APIs is similarly to the [platform intrinsic](https://github.com/dotnet/designs/blob/main/accepted/2018/platform-intrinsics.md) APIs, which are low-level .NET APIs for chip instructions. These APIs are similar but expose low-level APIs for memory-related operations.

## Libraries: System.Text.Json serialization notifications

The System.Text.Json serializer now exposes notifications as part of (de)serialization operations. They are useful for defaulting values and validation. To use them, implement one or more of the interfaces `IJsonOnDeserialized`, `IJsonOnDeserializing`, `IJsonOnSerialized` or `IJsonOnSerializing` within the `System.Text.Json.Serialization` namespace.

Here's an example that validates during both `JsonSerializer.Serialize()` and `JsonSerializer.Deserialize()` to ensure a `FirstName` property is not `null`.

```cs
  public class Person : IJsonOnDeserialized, IJsonOnSerializing
  {
      public string FirstName{ get; set; }

      void IJsonOnDeserialized.OnDeserialized() => Validate(); // Call after deserialization
      void IJsonOnSerializing.OnSerializing() => Validate(); // Call before serialization

      private void Validate()
      {
          if (FirstName is null)
          {
              throw new InvalidOperationException("The 'FirstName' property cannot be 'null'.");
          }
      }
  }
```

Previously, you would need to implement a custom converter to achieve this functionality.

## Libraries: System.Text.Json serialization property ordering

We've also added the ability to control the serialization order of properties, with `System.Text.Json.Serialization.JsonPropertyOrderAttribute`. An integer specifies the order. Smaller integers are serialized first; properties that have no attribute have a default ordering value of 0.

Here's an example that specifies JSON should be serialized in the order `Id, City, FirstName, LastName`:

```cs
public class Person
{
    public string City { get; set; } // No order defined (has the default ordering value of 0)

    [JsonPropertyOrder(1)] // Serialize after other properties that have default ordering
    public string FirstName { get; set; }

    [JsonPropertyOrder(2)] // Serialize after FirstName
    public string LastName { get; set; }

    [JsonPropertyOrder(-1)] // Serialize before other properties that have default ordering
    public int Id { get; set; }
}
```

Previously, the serialization order was determined by reflection order which was neither deterministic nor resulting in a specific desired order.

## Libraries: "write raw" JSON with System.Text.Json.Utf8JsonWriter

There are times when you need to [integrate "raw" JSON when writing JSON payloads](https://github.com/dotnet/runtime/issues/1784) with Utf8JsonWriter.

For example:

- I have a deliberate sequence of bytes I want to write out on the wire, and I know what I'm doing (as demonstrated in the following example).
- I have a blob which I think represents JSON content and which I want to envelope, and I need to make sure the envelope & its inner contents remain well-formed

```cs
JsonWriterOptions writerOptions = new() { WriteIndented = true, };

using MemoryStream ms = new();
using UtfJsonWriter writer = new(ms, writerOptions);

writer.WriteStartObject();
writer.WriteString("dataType", "CalculationResults");

writer.WriteStartArray("data");

foreach (CalculationResult result in results)
{
    writer.WriteStartObject();
    writer.WriteString("measurement", result.Measurement);

    writer.WritePropertyName("value");
    // Write raw JSON numeric value using FormatNumberValue (not defined in the example)
    byte[] formattedValue = FormatNumberValue(result.Value);
    writer.WriteRawValue(formattedValue, skipValidation: true);

    writer.WriteEndObject();
}

writer.WriteEndArray();
writer.WriteEndObject();
```

The following is a description of what the code above -- particularly `FormatNumberValue` -- is doing. For performance, `System.Text.Json` omits the decimal points/values when the number is whole, like `1.0`. The rationale is that writing fewer bytes is good for perf. In some scenarios, it might be important to retain decimal values because the consumer treats numbers without decimals as integers, otherwise as doubles. This new "raw value" model allows you to have that control wherever you need it.

## Libraries: Synchronous stream overloads on `JsonSerializer`

We've added [new synchronous APIs](https://github.com/dotnet/runtime/issues/1574) to `JsonSerializer` for serializing and deserializing JSON data to/from a stream. You can see that demonstrated in the following example.

```cs
using MemoryStream ms = GetMyStream();
MyPoco poco = JsonSerializer.Deserialize<MyPoco>(ms);
```

These new synchronous APIs include overloads that are compatible and usable with the new [System.Text.Json source generator](https://devblogs.microsoft.com/dotnet/try-the-new-system-text-json-source-generator/), by accepting `JsonTypeInfo<T>` or `JsonSerializerContext` instances.

## Libraries: System.Text.Json.Nodes.JsonNode support for `dynamic` is removed

Support for the C# [`dynamic`](https://docs.microsoft.com/dotnet/csharp/language-reference/builtin-types/reference-types#the-dynamic-type) type in the JsonSerializer has been removed. We added `dynamic` support in Preview 4 but later decided to be a poor design choice, including making it a required dependency of the `JsonNode` type.

This change is considered a [breaking change](https://github.com/dotnet/docs/issues/25105) from a .NET 6 preview to preview standpoint but not from .NET 5 to 6.

## Libraries: System.Diagnostics Propagators

We've been improving support for [OpenTelemetry](https://devblogs.microsoft.com/dotnet/opentelemetry-net-reaches-v1-0/) over the last couple years. One of the key aspects of enabling great support is ensuring that all components that need to participate in telemetry production produce network headers in the right format. It's really hard to do that, particularly as the OpenTelemetry specification changes. OpenTelemetry defines the [propagation](https://opentelemetry.lightstep.com/core-concepts/context-propagation/) concept to help with this situation. We're in the process of adopting [propagation](https://github.com/dotnet/runtime/issues/50658) to enable a general model for header customization.

Context on the broader concepts:

* [OpenTelemetry](https://opentelemetry.io/) specification -- In-memory representation of the distributed tracing data structures.
* [OpenTelemetry Span](https://opentelemetry.lightstep.com/spans/) -- Building block for a trace, and represented by [System.Diagnostics.Activity](https://docs.microsoft.com/dotnet/api/system.diagnostics.activity) in .NET.
* [W3C TraceContext](https://www.w3.org/TR/trace-context/) -- Spec on how to propagate these distributed tracing data structures over well-know HTTP headers.

The following code demonstrates the general approach for using propagation.

```C#
DistributedContextPropagator propagator = DistributedContextPropagator.Current;
propagator.Inject(activity, carrier, (object theCarrier, string fieldName, string value) =>
{
   // Extract the context from the activity then inject it to the carrier.
});
```

You can also choose to use a different propagator.

```C#
// Set the current propagation behavior to not transmit any distributed context information in outbound network messages.
DistributedContextPropagator.Current = DistributedContextPropagator.CreateNoOutputPropagator();
```

The `DistributedContextPropagator` abstract class determines if and how distributed context information is encoded and decoded as it traverses the network. The encoding can be transported over any network protocol that supports key-value string pairs. `DistributedContextPropagator` inject values into and extracts values from carriers as key/value string pairs.
By adding support for propagators, we've enabled two things:

* You're no longer required  to use the [W3C TraceContext](https://www.w3.org/TR/trace-context/) headers. You can write a custom propagator (i.e., use your own headers names including not sending them at all) without the libraries HttpClient, ASP.NET Core having a priori knowledge of this custom format
* If you implement a library with a custom transport (e.g., message queue), you can now support various wire formats as long as you support sending and receiving a text map (e.g. `Dictionary<string, string>`)

Most application code does not need to directly use this feature, however, it is likely that you will see it in a call-stack if you use OpenTelemetry. Some library code will want to participate in this model if it cares about tracing and causality.

## Libraries: Simplified call patterns for cryptographic operations

The .NET encryption and decryption routines were designed around streaming, with no real concession for when the payload is already in memory.  The new Encrypt- and Decrypt- methods on `SymmetricAlgorithm` accelerate the already-in-memory case, and are intended to provide clarity to the caller and the code reviewer.  Additionally, they support reading from and writing to spans.

The new simplified methods offer a straightforward approach to using cryptographic APIs:

```C#
private static byte[] Decrypt(byte[] key, byte[] iv, byte[] ciphertext)
{
    using (Aes aes = Aes.Create())
    {
        aes.Key = key;

        return aes.DecryptCbc(ciphertext, iv);
    }
}
```

With the new Encrypt- and Decrypt-methods, only the key property is used from the SymmetricAlgorithm instance. The new DecryptCbc method supports choosing the padding algorithm, but PKCS#7 is used with CBC so often that it's a default argument.  If you like the clarity, just specify it:

```C#
private static byte[] Decrypt(byte[] key, byte[] iv, byte[] ciphertext)
{
    using (Aes aes = Aes.Create())
    {
        aes.Key = key;

        return aes.DecryptCbc(ciphertext, iv, PaddingMode.PKCS7);
    }
}
```

You can see that the existing pattern -- with .NET 5 -- required significantly more plumbing for the same outcome.

```C#
private static byte[] Decrypt(byte[] key, byte[] iv, byte[] ciphertext)
{
    using (Aes aes = Aes.Create())
    {
        aes.Key = key;
        aes.IV = iv;

        // These are the defaults, but let's set them anyways.
        aes.Padding = PaddingMode.PKCS7;
        aes.Mode = CipherMode.CBC;

        using (MemoryStream destination = new MemoryStream())
        using (ICryptoTransform transform = aes.CreateDecryptor())
        using (CryptoStream cryptoStream = new CryptoStream(destination, transform, CryptoStreamMode.Write))
        {
            cryptoStream.Write(ciphertext, 0, ciphertext.Length);
            cryptoStream.FlushFinalBlock();
            return destination.ToArray();
        }
    }
}
```

## Libraries: Full Case Mapping Support in Globalization Invariant Mode

[Globalization Invariant Mode](https://github.com/dotnet/runtime/blob/main/docs/design/features/globalization-invariant-mode.md) enables you to remove application dependencies on globalization data and behavior in exchange for smaller applications (primarily on Linux). We've [improved Globalization Invariant Mode](https://docs.microsoft.com/dotnet/core/compatibility/globalization/6.0/culture-creation-invariant-mode) to support case mapping of the full Unicode character set. Previously, this mode only supported ASCII range characters for operations like `String.ToUpper`, `String.ToLower`, and string comparisons and searching with the [IgnoreCase option](https://docs.microsoft.com/dotnet/api/system.globalization.compareoptions).

Alpine-based .NET container images are the only environment where we [enable globalization environment mode by default](https://github.com/dotnet/dotnet-docker/blob/b7eeae802bcf3b9793ae49992ce4bb8d16c504c8/src/runtime-deps/5.0/alpine3.13/arm32v7/Dockerfile#L20).

## Runtime: W^X (write xor execute) support for all platforms and architectures

The runtime now has a mode in which it doesn't create or use any memory pages that are writeable and executable at the same time. All executable memory is mapped as read-execute only. This feature was enabled on macOS -- for Apple Silicon -- earlier in the release. On Apple Silicon machines, memory mappings that are writeable and executable at the same time are prohibited.

This capability is now enabled and supported on all other platforms as an opt-in experience. On these platforms, executable code generation / modification is done via separate read-write memory mappings. This is true for both JIT'd code and runtime-generated helpers. These mappings are created at virtual memory addresses that are different from the executable code address and exist only for a very brief period of time when the writing is performed. For example, the JIT now generates code into a scratch buffer that is copied into the executable memory using a single memory copy function call after the whole method is jitted. And the writeable mapping lifetime spans only the time of the memory copy.

This new feature can be enabled by setting the environment variable `DOTNET_EnableWriteXorExecute` to `1`. This feature is opt-in in .NET 6 because it has a startup regression (except on Apple Silicon). The regression is ~10% in our ASP.Net benchmark tests when compiled with Ready To Run (R2R). However, the steady state performance was measured to be the same with and without the feature enabled. For applications where startup performance isn't critical, we recommend enabling this feature for the improved security that it offers. We intend to resolve the performance regression as part of .NET 7 and enable the feature by default at that time.

## Closing

We're at that point in the release where we consider new features and improvements done. Nice work, team. That's a wrap on another season of .NET previews.

We continue to want and rely on your feedback. We will focus the rest of .NET 6 on regressions (functional and performance) and bugs found in the new features. Functional improvements in most cases will need to wait for .NET 7. Please share any and all feedback you have and we'll be happy to categorize it.

Thanks for everyone who has contributed to making .NET 6 another great release.

Thanks for being a .NET developer.
