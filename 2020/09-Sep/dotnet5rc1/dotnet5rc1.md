# Announcing .NET 5.0 RC1

Today, we are shipping [.NET 5.0 Release Candidate 1 (RC1)](https://dotnet.microsoft.com/download/dotnet/5.0). It is a near-final release of .NET 5.0, and the first of two RCs before the official release in November. RC1 is a "go live" release; you are supported using it in production. At this point, we're looking for reports of any remaining critical bugs that should be fixed before the final release. We need your feedback to get .NET 5.0 across the finish line.

We also released new versions of ASP.NET Core and EF Core today.

You can [download .NET 5.0](https://dotnet.microsoft.com/download/dotnet/5.0), for Windows, macOS, and Linux:

* [Installers and binaries](https://dotnet.microsoft.com/download/dotnet/5.0)
* [Container images](https://hub.docker.com/_/microsoft-dotnet)
* [Snap installer](https://snapcraft.io/dotnet-sdk)
* [Release notes](https://github.com/dotnet/core/tree/master/release-notes/5.0)
* [Known issues](https://github.com/dotnet/core/blob/master/release-notes/5.0/5.0-known-issues.md)
* [GitHub issue tracker](https://github.com/dotnet/core/issues)

You need the latest preview version of [Visual Studio](https://visualstudio.microsoft.com/vs/preview/) (including [Visual Studio for Mac)](https://visualstudio.microsoft.com/vs/mac/) to use .NET 5.0.

.NET 5.0 includes [many improvements](https://github.com/dotnet/runtime/issues/37269), notably [single file applications](https://github.com/dotnet/runtime/issues/36590), [smaller container images](https://github.com/dotnet/dotnet-docker/issues/1814#issuecomment-625294750), more capable [JsonSerializer APIs](https://github.com/dotnet/runtime/issues/41313), a complete set of [nullable reference type annotations](https://twitter.com/terrajobst/status/1296566363880742917), new [target framework names](https://github.com/dotnet/designs/blob/master/accepted/2020/net5/net5.md),  and support for [Windows ARM64](https://github.com/dotnet/runtime/issues/36699). [Performance has been greatly improved](https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-5/), in the NET libraries, in the GC, and the JIT. [ARM64](https://github.com/dotnet/runtime/issues/35853) was a [key focus for performance investment](https://devblogs.microsoft.com/dotnet/arm64-performance-in-net-5/), resulting in much better throughput and smaller binaries. .NET 5.0 includes new language versions, [C# 9](https://devblogs.microsoft.com/dotnet/welcome-to-c-9-0/) and [F# 5.0](https://devblogs.microsoft.com/dotnet/f-5-update-for-august/).

We recently published a few deep-dive posts about new capabilities in 5.0 that you may want to check out:

* [F# 5 update for August](https://devblogs.microsoft.com/dotnet/f-5-update-for-august/)
* [ARM64 Performance in .NET 5](https://devblogs.microsoft.com/dotnet/arm64-performance-in-net-5/)
* [Improvements in native code interop in .NET 5.0](https://devblogs.microsoft.com/dotnet/improvements-in-native-code-interop-in-net-5-0/)
* [Introducing the Half type!](https://devblogs.microsoft.com/dotnet/introducing-the-half-type/)
* [App Trimming in .NET 5](https://devblogs.microsoft.com/dotnet/app-trimming-in-net-5/)
* [Customizing Trimming in .NET 5](https://devblogs.microsoft.com/dotnet/customizing-trimming-in-net-core-5/)
* [Automatically find latent bugs in your code with .NET 5](https://devblogs.microsoft.com/dotnet/automatically-find-latent-bugs-in-your-code-with-net-5/)

Just like I did for [.NET 5.0 Preview 8](https://devblogs.microsoft.com/dotnet/announcing-net-5-0-preview-8/) I've chosen a selection of features to look at in more depth and to give you a sense of how you'll use them in real-world usage. This post is dedicated to records in C# 9 and `System.Text.Json.JsonSerializer`. They are separate features, but also a very nice pairing, particularly if you spend a lot of time crafting [POCO](https://en.wikipedia.org/wiki/Plain_old_CLR_object) types for deserialized JSON objects.

## C# 9 -- Records

Records are perhaps the most important new feature in [C# 9](https://github.com/dotnet/roslyn/blob/master/docs/Language%20Feature%20Status.md#c-9). They offer a broad feature set (for a language type kind), some of which requires RC1 or later (like `record.ToString()`).

The easiest way to think of records is as immutable classes. Feature-wise, they are closest to tuples. One can think of them as custom tuples with properties and immutability. There are likely many cases where tuples are used today that would be better served by records.

If you are using C#, you will get the best experience if you are using named types (as opposed to a feature like tuples). Static typing is the primary design point of the language. Records make it easier to use small types, and take advantage of type safety throughout your app.

### Records are immutable data types

Records enable you to create immutable data types. This is great for defining types that store small amounts of data.

The following is an example of a record. It stores user information from a login screen.

```csharp
public record LoginResource(string Username, string Password, bool RememberMe);
```

It is semantically similar (almost identical) to the following class. I'll cover the differences shortly.

```csharp
public class LoginResource
{
    public LoginResource(string username, string password, bool rememberMe)
    {
        Username = username;
        Password = password;
        RememberMe = rememberMe;
    }

    public string Username { get; init; }
    public string Password { get; init; }
    public bool RememberMe { get; init; }
}
```

`init` is a new keyword that is an alternative to `set`. `set` allows you to assign a property at any time. `init` only allows you to assign a property during object construction. It's the building block that records rely on for immutability. Any type can use `init`. It isn't specific to records, as you can see in the previous class definition.

`private set` might seem similar to `init`; `private set` prevents other code (outside the type) from mutating data. `init` will generate compiler errors when a type mutates a property accidentally (after construction). `private set` isn't intended to model immutable data, so doesn't generate any compiler errors or warnings when the type mutates a property value after construction.

### Records are specialized classes

As I just covered, the record and the class variants of `LoginResource` are almost identical. The class definition is a semantically identical *subset* of the record. The record provides more, specialized, behavior.

Just so we're on the same page, the following comparison is between a `record`, and a `class` that uses `init` instead of `set` for properties, as demonstrated earlier.

What's the same?

- Construction
- Immutability
- Copy semantics (records are classes under the hood)

What's different?

- Record equality is based on content. Class equality based on object identity.
- Records provide a `GetHashCode()` implementation is that is based on record content.
- Records provide an `IEquatable<T>` implementation. It uses the unique `GetHashCode()` behavior as the mechanism to provide the content-based equality semantic for records.
- Record ToString() is overridden to print record content.

The differences between a record and a class (using `init`) can be seen in the disassembly for [LoginResource as a record](https://sharplab.io/#v2:EYLgtghgzgLgpgJwD4AEBMBGAsAKBQZgAIE4BjAewQBNCAZcgcwEsA7AJTinIFcFS4AFCgwAGQgFUoiFhDBwANIWFiACtCgB3SlUXBy5ADaEOcsMEQBZOAEoA3Llwy5UAA4R+SjADo23FjCY5LwBhcjAXJgNEAGVEADcmfihcAG9cQiU0QgBJKABRAA94BBkDbJYmGEIUwgBfXFqgA==) and [LoginResource as a class](https://sharplab.io/#v2:EYLgtghgzgLgpgJwD4AEBMBGAsAKFygZgAJ0iAZAewHMBLAOwCU4oKBXBAYzgGEAbaKLgDeuImJLFKtRszace/KFAAUKDAAYirKIjoQwcADQkNRAA4CA7hQQATY8AoVeRBHANhgiALJwAlKLiIjjioUQAqjoIegZEALxaUTFwANyBYUQAClY2tvHmOXZpIRlETB5eCL75bhU+qelEAL64jYQmmpG6+nBEQkRUcDApRPQ0w81txGqa2UrWdn0DQyNjEy0lYu2OzmXu7pXV/YMTayMbG7jJUBZcJgB0DKx0MDQG99wUYGY0vIgAyogAG40LiCHDBLZoIgASSgAFEAB7waIQXgwujjJYXIA).

I'll show you some code that demonstrates these differences.

```csharp
using System;
using System.Linq;
using static System.Console;

var user = "Lion-O";
var password = "jaga";
var rememberMe = true;
LoginResourceRecord lrr1 = new(user, password, rememberMe);
var lrr2 = new LoginResourceRecord(user, password, rememberMe);
var lrc1 = new LoginResourceClass(user, password, rememberMe);
var lrc2 = new LoginResourceClass(user, password, rememberMe);

WriteLine($"Test record equality -- lrr1 == lrr2 : {lrr1 == lrr2}");
WriteLine($"Test class equality  -- lrc1 == lrc2 : {lrc1 == lrc2}");
WriteLine($"Print lrr1 hash code -- lrr1.GetHashCode(): {lrr1.GetHashCode()}");
WriteLine($"Print lrr2 hash code -- lrr2.GetHashCode(): {lrr2.GetHashCode()}");
WriteLine($"Print lrc1 hash code -- lrc1.GetHashCode(): {lrc1.GetHashCode()}");
WriteLine($"Print lrc2 hash code -- lrc2.GetHashCode(): {lrc2.GetHashCode()}");
WriteLine($"{nameof(LoginResourceRecord)} implements IEquatable<T>: {lrr1 is IEquatable<LoginResourceRecord>} ");
WriteLine($"{nameof(LoginResourceClass)}  implements IEquatable<T>: {lrr1 is IEquatable<LoginResourceClass>}");
WriteLine($"Print {nameof(LoginResourceRecord)}.ToString -- lrr1.ToString(): {lrr1.ToString()}");
WriteLine($"Print {nameof(LoginResourceClass)}.ToString  -- lrc1.ToString(): {lrc1.ToString()}");

public record LoginResourceRecord(string Username, string Password, bool RememberMe);

public class LoginResourceClass
{
    public LoginResourceClass(string username, string password, bool rememberMe)
    {
        Username = username;
        Password = password;
        RememberMe = rememberMe;
    }

    public string Username { get; init; }
    public string Password { get; init; }
    public bool RememberMe { get; init; }
}
```

```html
<script src="https://gist.github.com/richlander/177aeb54839da4416dfe977a4136149d.js"></script>
```

Note: You will notice that the `LoginResource` types end in `Record` and `Class`. That pattern is not the indication of a new naming pattern. They are only named that way so that there can be a record and class variant of the same type in the sample. Please don't name your types that way.

This code produces the following output.

```console
rich@thundera records % dotnet run
Test record equality -- lrr1 == lrr2 : True
Test class equality  -- lrc1 == lrc2 : False
Print lrr1 hash code -- lrr1.GetHashCode(): -542976961
Print lrr2 hash code -- lrr2.GetHashCode(): -542976961
Print lrc1 hash code -- lrc1.GetHashCode(): 54267293
Print lrc2 hash code -- lrc2.GetHashCode(): 18643596
LoginResourceRecord implements IEquatable<T>: True
LoginResourceClass  implements IEquatable<T>: False
Print LoginResourceRecord.ToString -- lrr1.ToString(): LoginResourceRecord { Username = Lion-O, Password = jaga, RememberMe = True }
Print LoginResourceClass.ToString -- lrc1.ToString(): LoginResourceClass
```

### Record syntax

There are multiple patterns for declaring records that cater to different use cases. After playing with each one, you start to get a feel for the benefits of each pattern. You'll also see that they are not distinct syntax but a continuum of options.

The first pattern is the simplest one -- a one liner -- but offers the least flexibility. It's good for records with a small number of required properties.

Here is the LoginResource record, shown earlier, as an example of this pattern. That's it. That one line is the entire definition.

```csharp
public record LoginResource(string Username, string Password, bool RememberMe);
```

Construction follows the requirements of a constructor with parameters (including the allowance for optional parameters).

```csharp
var login = new LoginResource("Lion-O", "jaga", true);
```

You can also use target typing if you prefer.

```csharp
LoginResource login = new("Lion-O", "jaga", true);
```

The next syntax makes all the properties optional. There is an implicit parameterless constructor provided for the record.

```csharp
public record LoginResource
{
    public string Username {get; init;}
    public string Password {get; init;}
    public bool RememberMe {get; init;}
}
```

Construction uses object initializers and could look like the following:

```csharp
LoginResource login = new() 
{
    Username = "Lion-O", 
    TemperatureC = "jaga"
};
```

Maybe you want to make those two properties required, with the other one optional. This last pattern would look like the following.

```csharp
public record LoginResource(string Username, string Password)
{
    public bool RememberMe {get; init;}
}
```

Construction could look like the following, with `RememberMe` unspecified.

```csharp
LoginResource login = new("Lion-O", "jaga");
```

And with `RememberMe` specified.

```csharp
LoginResource login = new("Lion-O", "jaga")
{
    RememberMe = true
};
```

I want to make sure that you don't think that records are exclusively for immutable data. They can opt into mutable data, as you can see in the following example.

```csharp
using System;

Battery battery = new Battery("CR2032", 0.235)
{
    RemainingCapacityPercentage = 100
};

Console.WriteLine (battery);

for (int i = battery.RemainingCapacityPercentage; i >= 0; i--)
{
    battery.RemainingCapacityPercentage = i;
}

Console.WriteLine (battery);

public record Battery(string Model, double TotalCapacityAmpHours)
{
    public int RemainingCapacityPercentage {get;set;}
}
```

```html
<script src="https://gist.github.com/richlander/83d2ccde88218e56e6b93a55db3f986b.js"></script>
```

It produces the following output.

rich@thundera recordmutable % dotnet run
Battery { Model = CR2032, TotalCapacityAmpHours = 0.235, RemainingCapacityPercentage = 100 }
Battery { Model = CR2032, TotalCapacityAmpHours = 0.235, RemainingCapacityPercentage = 0 }

### Non-destructive record mutation

Immutability provides significant benefits, but you will quickly find a case where you need to mutate a record. How can you do that without giving up on immutability? The `with` expression satisfies this need. It enables creating a new record in terms of an existing record of the same type. You can specify the new values that you want to be different, and all other properties are copied from the existing record.

Let's transform the username to lower-case. That's how usernames are stored in our pretend user database. However, the original username casing is required for diagnostic purposes. It could look like the following, assuming the code from the previous example:

```csharp
LoginResource login = new("Lion-O", "jaga", true);
LoginResource loginLowercased = lrr1 with {Username = login.Username.ToLowerInvariant()};
```

The `login` record hasn't been changed. In fact, that's impossible. The transformation has only affected `loginLowercased`. Other than the lowercase transformation to `loginLowercased`, it's identical to `login`.

We can check that `with` has done what we expect using the built-in `ToString()` override.

```csharp
Console.WriteLine(login);
Console.WriteLine(loginLowercased);
```

This code produces the following output.

```console
LoginResource { Username = Lion-O, Password = jaga, RememberMe = True }
LoginResource { Username = lion-o, Password = jaga, RememberMe = True }
```

We can go one step further with understanding how `with` works. It copies all values from one record to the other. This isn't a delegation model where one record depends on another. In fact, after the `with` operation completes, there is no relationship between the two records. `with` only has meaning for record construction. That means  for reference types, the copy is just a copy of the reference. For value types, the value is copied.

You can see that semantic at play with the following code.

```csharp
Console.WriteLine($"Record equality: {login == loginLowercased}");
Console.WriteLine($"Property equality: Username == {login.Username == loginLowercased.Username}; Password == {login.Password == loginLowercased.Password}; RememberMe == {login.RememberMe == loginLowercased.RememberMe}");
```

It produces the following output.

```console
Record equality: False
Property equality: Username == False; Password == True; RememberMe == True
```

### Record inheritance

It's easy to extend a record. Let's assume a new `LastLoggedIn` property. It could be added directly to `LoginResource`. That's a fine idea. Records are not brittle like interfaces traditionally have been, unless you want to make new properties required constructor parameters. 

In this case, I want to make `LastLogin` required. Imagine the codebase is large, and it would be expensive to sprinkle knowledge of the `LastLoggedIn` property in all the places where a `LoginResource` is created. Instead, we're going to create a new record that extends `LoginResource` with this new property. Existing code will work in terms of `LoginResource` and new code will work in terms of a new record that can then assume that the `LastLoggedIn` property has been populated. Code that accepts a `LoginResource` will happily accept the new record, by virtue of regular inheritance rules.

This new record could be based on any of the `LoginResource` variants demonstrated earlier. It will be based on the following one.

```csharp
public record LoginResource(string Username, string Password)
{
    public bool RememberMe {get; init;}
}
```

The new record could look like the following.

```csharp
public record LoginWithUserDataResource(string Username, string Password, DateTime LastLoggedIn) : LoginResource(Username, Password)
{
    public int DiscountTier {get; init};
    public bool FreeShipping {get; init};
}
```

I've made `LastLoggedIn` a required property, and taken the opportunity to add additional, optional, properties that may or may not be set. The optional `RememberMe` property is also defined, by virtue of extending the `LoginResource` record.

### Modeling record construction helpers

One of the patterns that isn't necessarily intuitive is modeling helpers that you want to use as part of record construction. Let's switch examples, to weight measurements. Weight measurements come from an internet-connected scale. The weight is specified in Kilograms, however, there are some cases where the weight needs to be provided in pounds.

The following record declaration could be used.

```csharp
public record WeightMeasurement(DateTime Date, int Kilograms)
{
    public int Pounds {get; init;}

    public static int GetPounds(int kilograms) => kilograms * 2.20462262;
}
```

This is what construction would look like.

```csharp
var weight = 200;
WeightMeasurement measurement = new(DateTime.Now, weight)
{
    Pounds = WeightMeasurement.GetPounds(weight)
};
```

In this example, it is necessary to specify the weight as a local. It isn't possible to access the `Kilograms` property within an object initializer. It is also necessary to define `GetPounds` as a static method. It isn't possible to call instance methods (for the type being constructed) within an object initializer.

### Records and Nullability

You get nullability for free with records, right? Everything is immutable, so where would the nulls come from? Not quite. An immutable property can be null and will always be null in that case.

Let's look at another program without nullability enabled.

```csharp
using System;
using System.Collections.Generic;

Author author = new(null, null);

Console.WriteLine(author.Name.ToString());

public record Author(string Name, List<Book> Books)
{
    public string Website {get; init;}
    public string Genre {get; init;}
    public List<Author> RelatedAuthors {get; init;}
}

public record Book(string name, int Published, Author author);
```

This program compiles and will throw a `NullReference` exception, due to dereferencing `author.Name`, which is `null`.

To further drive home this point, the following will not compile. `author.Name` is initialized as `null` and then cannot be changed, since the property is immutable.

```csharp
Author author = new(null, null);
author.Name = "Colin Meloy";
```

I'm going to update my project file to enable nullability.

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net5.0</TargetFramework>
    <LangVersion>preview</LangVersion>
    <Nullable>enable</Nullable>
  </PropertyGroup>

</Project>
```

I'm now seeing a bunch of warnings like the following.

```console
/Users/rich/recordsnullability/Program.cs(8,21): warning CS8618: Non-nullable property 'Website' must contain a non-null value when exiting constructor. Consider declaring the property as nullable. [/Users/rich/recordsnullability/recordsnullability.csproj]
```

I updated the `Author` record with null annotations that describe my intended use of the record.

```csharp
public record Author(string Name, List<Book> Books)
{
    public string? Website {get; init;}
    public string? Genre {get; init;}
    public List<Author>? RelatedAuthors {get; init;}
}
```

I'm still getting warnings for the `null, null` construction of `Author` seen earlier.

```console
/Users/rich/recordsnullability/Program.cs(5,21): warning CS8625: Cannot convert null literal to non-nullable reference type. [/Users/rich/recordsnullability/recordsnullability.csproj]
```

That's good, since that's a scenario I want to protect against. I'll now show you an updated variant of the program that plays nicely with and enjoys the benefits of nullability.

```csharp
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

Author lord = new Author("Karen Lord")
{
    Website = "https://karenlord.wordpress.com/",
    RelatedAuthors = new()
};

lord.Books.AddRange(
    new Book[] 
    {
        new Book("The Best of All Possible Worlds", 2013, lord),
        new Book("The Galaxy Game", 2015, lord)
    }
);

lord.RelatedAuthors.AddRange(
    new Author[]
    {
        new ("Nalo Hopkinson"),
        new ("Ursula K. Le Guin"),
        new ("Orson Scott Card"),
        new ("Patrick Rothfuss")
    }
);

Console.WriteLine($"Author: {lord.Name}");
Console.WriteLine($"Books: {lord.Books.Count}");
Console.WriteLine($"Related authors: {lord.RelatedAuthors.Count}");


public record Author(string Name)
{
    private List<Book> _books = new();

    public List<Book> Books => _books;

    public string? Website {get; init;}
    public string? Genre {get; init;}
    public List<Author>? RelatedAuthors {get; init;}
}

public record Book(string name, int Published, Author author);
```

```html
<script src="https://gist.github.com/richlander/2c63df7e23d038145eb5d22c4b8dbdc2.js"></script>
```

This program compiles without nullable warnings.

You might be wondering about the following line:

```csharp
lord.RelatedAuthors.AddRange(
```

`Author.RelatedAuthors` can be null. The compiler can see that the `RelatedAuthors` property is set just a few lines earlier, so it knows that `RelatedAuthors` reference will be non-null. 

However, imagine the program instead looked like the following.

```csharp
Author GetAuthor()
{
    return new Author("Karen Lord")
    {
        Website = "https://karenlord.wordpress.com/",
        RelatedAuthors = new()
    };
}

Author lord = GetAuthor();
```

The compiler doesn't have the flow analysis smarts to know that `RelatedAuthors` will be non-null when type construction is within a separate method. In that case, one of two following patterns would be needed.

```csharp
lord.RelatedAuthors!.AddRange(
```

or

```csharp
if (lord.RelatedAuthors is object)
{
    lord.RelatedAuthors.AddRange( ...
}
```

This is a long demonstration of records nullability just to say that it doesn't change anything about the experience of using nullable reference types.

Separately, you may have noticed that I moved the `Books` property on the `Author` record to be an initialized get-only property, instead of being a required parameter in the record constructor. This was driven by there being a circular relationship between `Author` and `Books`. Immutability and circular references can cause headaches. It is OK in this case, and just means that all `Author` objects need to be created before `Book` objects. As a result, it isn't possible to provide a fully initialized set of `Book` objects as part of `Author` construction. The best we could ever expect as part of `Author` construction is an empty `List<Book>`. As a result, initializing an empty `List<Book>` as part of `Author` construction seem like the best choice. There is no rule that all of these properties need to be `init` style. I've chosen to do that to demonstrate the behavior when you do.

We're about to transition to talk about JSON serialization. This example, with circular references, relates to the **Preserving references in JSON object graphs** section coming shortly. `JsonSerializer` supports object graphs with circular references, but not with types with parameterized constructors. You can serialize the `Author` object to JSON, but not back to an `Author` object as it is currently defined. If `Author` wasn't a `record` or didn't have circular references, then both serialization and deserialization would work with `JsonSerializer`.

## System.Text.Json

`System.Text.Json` has been [significantly improved in .NET 5.0](https://github.com/dotnet/runtime/issues/41313) to improve performance, reliability, and to make it easier for people to adopt that are familiar with [Newtonsoft.Json](https://www.nuget.org/packages/Newtonsoft.Json/). It also includes [support for deserializing JSON objects to records](https://github.com/dotnet/runtime/issues/38539), the new C# feature covered earlier in this post.

If you are looking at using `System.Text.Json` as an alternative to `Newtonsoft.Json`, you should check out the [migration guide](https://docs.microsoft.com/dotnet/standard/serialization/system-text-json-migrate-from-newtonsoft-how-to). The guide clarifies the relationship between these two APIs. `System.Text.Json` is intended to cover many of the same scenarios as `Newtonsoft.Json`, but it's not intended to be a drop-in replacement for or achieve feature parity with the popular JSON library. We try to maintain a balance between performance and usability, and bias to performance in our design choices. 

### `HttpClient` extension methods

[`JsonSerializer` extension methods](https://github.com/dotnet/runtime/issues/32937) are now exposed on [`HttpClient`](https://docs.microsoft.com/dotnet/api/system.net.http.httpclient?view=net-5.0) and greatly simplify using these two APIs together. These extension methods remove complexity and take care of a variety of scenarios for you, including handling the content stream and validating the content media type. Steve Gordon does a great job of explaining the benefits in [Sending and receiving JSON using HttpClient with System.Net.Http.Json](https://www.stevejgordon.co.uk/sending-and-receiving-json-using-httpclient-with-system-net-http-json).

The following example deserializes weather forecast JSON data into a `Forecast` record, using the new [`GetFromJsonAsync<T>()`](https://docs.microsoft.com/dotnet/api/system.net.http.json.httpclientjsonextensions.getfromjsonasync?view=net-5.0#System_Net_Http_Json_HttpClientJsonExtensions_GetFromJsonAsync__1_System_Net_Http_HttpClient_System_String_System_Threading_CancellationToken_) extension method.

```csharp
using System;
using System.Net.Http;
using System.Net.Http.Json;

string serviceURL = "https://localhost:5001/WeatherForecast";
HttpClient client = new();
Forecast[] forecasts = await client.GetFromJsonAsync<Forecast[]>(serviceURL);

foreach(Forecast forecast in forecasts)
{
    Console.WriteLine($"{forecast.Date}; {forecast.TemperatureC}C; {forecast.Summary}");
}

// {"date":"2020-09-06T11:31:01.923395-07:00","temperatureC":-1,"temperatureF":31,"summary":"Scorching"}            
public record Forecast(DateTime Date, int TemperatureC, int TemperatureF, string Summary);
```

```html
<script src="https://gist.github.com/richlander/3b41d7496f2d8533b2d88896bd31e764.js"></script>
```

This code is compact! It is relying on top-level programs and records from C# 9 and the new `GetFromJsonAsync<T>()` extension method. The use of `foreach` and `await` in such close proximity might be make you wonder if we're going to [add support for streaming JSON objects](https://github.com/dotnet/runtime/issues/1570). Yes, in a future release. 

You can try this on your own machine. The following .NET SDK commands will create a weather forecast service using the WebAPI template. It will expose the service at the following URL by default: `https://localhost:5001/WeatherForecast`. This is the same URL used in the sample.

```console
rich@thundera ~ % dotnet new webapi -o webapi
rich@thundera ~ % cd webapi 
rich@thundera webapi % dotnet run
```

Make sure you've run `dotnet dev-certs https --trust` first or the handshake between client and server won't work. If you're having trouble, see [Trust the ASP.NET Core HTTPS development certificate](https://docs.microsoft.com/aspnet/core/security/enforcing-ssl?view=aspnetcore-5.0&tabs=visual-studio#trust-the-aspnet-core-https-development-certificate-on-windows-and-macos).

You can then run the previous sample.

```console
rich@thundera ~ % git clone https://gist.github.com/3b41d7496f2d8533b2d88896bd31e764.git weather-forecast
rich@thundera ~ % cd weather-forecast
rich@thundera weather-forecast % dotnet run
9/9/2020 12:09:19 PM; 24C; Chilly
9/10/2020 12:09:19 PM; 54C; Mild
9/11/2020 12:09:19 PM; -2C; Hot
9/12/2020 12:09:19 PM; 24C; Cool
9/13/2020 12:09:19 PM; 45C; Balmy
```

### Improved support for immutable types

There are multiple patterns for defining immutable types. Records are just the newest one. `JsonSerializer` now has support for immutable types.

In this example, you'll see the serialization with an immutable struct.

```csharp
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

var json = "{\"date\":\"2020-09-06T11:31:01.923395-07:00\",\"temperatureC\":-1,\"temperatureF\":31,\"summary\":\"Scorching\"} ";           
var options = new JsonSerializerOptions()
{
    PropertyNameCaseInsensitive = true,
    IncludeFields = true,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
};
var forecast = JsonSerializer.Deserialize<Forecast>(json, options);

Console.WriteLine(forecast.Date);
Console.WriteLine(forecast.TemperatureC);
Console.WriteLine(forecast.TemperatureF);
Console.WriteLine(forecast.Summary);

var roundTrippedJson = JsonSerializer.Serialize<Forecast>(forecast, options);

Console.WriteLine(roundTrippedJson);

public struct Forecast{
    public DateTime Date {get;}
    public int TemperatureC {get;}
    public int TemperatureF {get;}
    public string Summary {get;}
    [JsonConstructor]
    public Forecast(DateTime date, int temperatureC, int temperatureF, string summary) => (Date, TemperatureC, TemperatureF, Summary) = (date, temperatureC, temperatureF, summary);
}
```

Note: The `JsonConstructor` attribute is required to specify the constructor to use with structs. With classes, if there is only a single constructor, then the attribute is not required. Same with records.

It produces the following output:

```console
rich@thundera jsonserializerimmutabletypes % dotnet run
9/6/2020 11:31:01 AM
-1
31
Scorching
{"date":"2020-09-06T11:31:01.923395-07:00","temperatureC":-1,"temperatureF":31,"summary":"Scorching"}
```

### Support for records

`JsonSerializer` [support for records](https://github.com/dotnet/runtime/issues/38539) is almost the same as what I just showed you for immutable types. The difference I want to show here is deserializing a JSON object to a record that exposes a parameterized constructor and an optional init property.

Here's the program, including the record definition:

```csharp
using System;
using System.Text.Json;

Forecast forecast = new(DateTime.Now, 40)
{
    Summary = "Hot!"
};

string forecastJson = JsonSerializer.Serialize<Forecast>(forecast);
Console.WriteLine(forecastJson);
Forecast? forecastObj = JsonSerializer.Deserialize<Forecast>(forecastJson);
Console.Write(forecastObj);

public record Forecast (DateTime Date, int TemperatureC)
{
    public string? Summary {get; init;}
};
```

```html
<script src="https://gist.github.com/richlander/bcb881c897f65c3fd804499846fcdefc.js"></script>
```

It produces the following output:

```console
rich@thundera jsonserializerrecords % dotnet run
{"Date":"2020-09-12T18:24:47.053821-07:00","TemperatureC":40,"Summary":"Hot!"}
Forecast { Date = 9/12/2020 6:24:47 PM, TemperatureC = 40, Summary = Hot! }
```

### Improved `Dictionary<K,V>` support

`JsonSerializer` now [supports dictionaries with non-string keys](https://github.com/dotnet/runtime/issues/30618). You can see what this looks like in the following sample. With .NET Core 3.0, this code compiles but throws a `NotSupportedException`.

```csharp
using System;
using System.Collections.Generic;
using System.Text.Json;

Dictionary<int, string> numbers = new ()
{
    {0, "zero"},
    {1, "one"},
    {2, "two"},
    {3, "three"},
    {5, "five"},
    {8, "eight"},
    {13, "thirteen"},
    {21, "twenty one"},
    {34, "thirty four"},
    {55, "fifty five"},
};

var json = JsonSerializer.Serialize<Dictionary<int, string>>(numbers);

Console.WriteLine(json);

var dictionary = JsonSerializer.Deserialize<Dictionary<int, string>>(json);

Console.WriteLine(dictionary[55]);
```

```html
<script src="https://gist.github.com/richlander/622028863a10bd2051a5ba47b903d839.js"></script>
```

It produces the following output.

```console
rich@thundera jsondictionarykeys % dotnet run
{"0":"zero","1":"one","2":"two","3":"three","5":"five","8":"eight","13":"thirteen","21":"twenty one","34":"thirty four","55":"fifty five"}
fifty five
```

### Support for fields

`JsonSerializer` now [supports fields](https://github.com/dotnet/runtime/issues/876). This change was contributed by [@YohDeadfall](https://github.com/YohDeadfall). Thanks! 

You can see what this looks like in the following sample. With .NET Core 3.0, `JsonSerializer` fails to serialize or deserialize with types that use fields. This is a problem for existing types that have fields and cannot be changed. With this change, that's no longer an issue.

```csharp
using System;
using System.Text.Json;

var json = "{\"date\":\"2020-09-06T11:31:01.923395-07:00\",\"temperatureC\":-1,\"temperatureF\":31,\"summary\":\"Scorching\"} ";           
var options = new JsonSerializerOptions()
{
    PropertyNameCaseInsensitive = true,
    IncludeFields = true,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
};
var forecast = JsonSerializer.Deserialize<Forecast>(json, options);

Console.WriteLine(forecast.Date);
Console.WriteLine(forecast.TemperatureC);
Console.WriteLine(forecast.TemperatureF);
Console.WriteLine(forecast.Summary);

var roundTrippedJson = JsonSerializer.Serialize<Forecast>(forecast, options);

Console.WriteLine(roundTrippedJson);

public class Forecast{
    public DateTime Date;
    public int TemperatureC;
    public int TemperatureF;
    public string Summary;
}
```

```html
<script src="https://gist.github.com/richlander/ff14d3e4b78f110c3c72abe58d9b9716.js"></script>
```

Produces the following output:

```console
rich@thundera jsonserializerfields % dotnet run
9/6/2020 11:31:01 AM
-1
31
Scorching
{"date":"2020-09-06T11:31:01.923395-07:00","temperatureC":-1,"temperatureF":31,"summary":"Scorching"}
```

### Preserving references in JSON object graphs

`JsonSerializer` has added [support for preserving (circular) references](https://github.com/dotnet/runtime/issues/30820) within JSON object graphs. It does this by storing IDs that can be reconstituted when a JSON string is deserialized back to objects.

```csharp
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

Employee janeEmployee = new()
{
    Name = "Jane Doe",
    YearsEmployed = 10
};

Employee johnEmployee = new()
{
    Name = "John Smith"
};

janeEmployee.Reports = new List<Employee> { johnEmployee };
johnEmployee.Manager = janeEmployee;

JsonSerializerOptions options = new()
{
    // NEW: globally ignore default values when writing null or default
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
    // NEW: globally allow reading and writing numbers as JSON strings
    NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString,
    // NEW: globally support preserving object references when (de)serializing
    ReferenceHandler = ReferenceHandler.Preserve,
    IncludeFields = true, // NEW: globally include fields for (de)serialization
    WriteIndented = true
};

string serialized = JsonSerializer.Serialize(janeEmployee, options);
Console.WriteLine($"Jane serialized: {serialized}");

Employee janeDeserialized = JsonSerializer.Deserialize<Employee>(serialized, options);
Console.Write("Whether Jane's first report's manager is Jane: ");
Console.WriteLine(janeDeserialized.Reports[0].Manager == janeDeserialized);

public class Employee
{
    // NEW: Allows use of non-public property accessor.
    // Can also be used to include fields "per-field", rather than globally with JsonSerializerOptions.
    [JsonInclude]
    public string Name { get; internal set; }

    public Employee Manager { get; set; }

    public List<Employee> Reports;

    public int YearsEmployed { get; set; }

    // NEW: Always include when (de)serializing regardless of global options
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public bool IsManager => Reports?.Count > 0;
}
```

```html
<script src="https://gist.github.com/richlander/4aac4e24b706f929bcd056e4d8b22d7b.js"></script>
```

### Performance

`JsonSerializer` performance is significantly improved in .NET 5.0. Stephen Toub covered some [`JsonSerializer` improvements](https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-5/#json) in his [Performance Improvements in .NET 5](https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-5) post. I'll cover a few more here.

#### Collections (de)serialization

We made significant improvements for large collections (~1.15x-1.5x on deserialize, ~1.5x-2.4x+ on serialize). You can see these improvements characterized in much more detail [dotnet/runtime #2259](https://github.com/dotnet/runtime/pull/2259).

The improvements to `List<int>` (de)serialization is particularly impressive, comparing .NET 5.0 to .NET Core 3.1. Those changes are going to be show up as meaningful with high-performance apps.

|         Method |     Mean |    Error |   StdDev |   Median |      Min |      Max |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|--------------- |---------:|---------:|---------:|---------:|---------:|---------:|-------:|------:|------:|----------:|
| Deserialize before | 76.40 us | 0.392 us | 0.366 us | 76.37 us | 75.53 us | 76.87 us | 1.2169 |     - |     - |   8.25 KB |
| After ~1.5x faster | 50.05 us | 0.251 us | 0.235 us | 49.94 us | 49.76 us | 50.43 us | 1.3922 |     - |     - |   8.62 KB |
| Serialize before | 29.04 us | 0.213 us | 0.189 us | 29.00 us | 28.70 us | 29.34 us | 1.2620 |     - |     - |   8.07 KB |
| After ~2.4x faster | 12.17 us | 0.205 us | 0.191 us | 12.15 us | 11.97 us | 12.55 us | 1.3187 |     - |     - |   8.34 KB |

#### Property lookups -- naming convention

One of the most common issues with using JSON is a mismatch of [naming conventions](https://en.wikipedia.org/wiki/Naming_convention_(programming)#Letter_case-separated_words) with .NET design guidelines. JSON properties are often [camelCase](https://en.wikipedia.org/wiki/Camel_case) and .NET properties and fields are typically PascalCase. The json serializer you use is responsible for bridging between naming conventions. That doesn't come for free, at least not with .NET Core 3.1. That cost is now negligible with .NET 5.0.

The code that allows for missing properties and case insensitivity has been greatly improved in .NET 5.0. It is [~1.75x faster in some cases](https://github.com/dotnet/runtime/pull/35848). 

The following benchmarks for a simple 4-property test class that has property names > 7 bytes.

```
3.1 performance
|                            Method |       Mean |   Error |  StdDev |     Median |        Min |        Max |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|---------------------------------- |-----------:|--------:|--------:|-----------:|-----------:|-----------:|-------:|------:|------:|----------:|
| CaseSensitive_Matching            |   844.2 ns | 4.25 ns | 3.55 ns |   844.2 ns |   838.6 ns |   850.6 ns | 0.0342 |     - |     - |     224 B |
| CaseInsensitive_Matching          |   833.3 ns | 3.84 ns | 3.40 ns |   832.6 ns |   829.4 ns |   841.1 ns | 0.0504 |     - |     - |     328 B |
| CaseSensitive_NotMatching(Missing)| 1,007.7 ns | 9.40 ns | 8.79 ns | 1,005.1 ns |   997.3 ns | 1,023.3 ns | 0.0722 |     - |     - |     464 B |
| CaseInsensitive_NotMatching       | 1,405.6 ns | 8.35 ns | 7.40 ns | 1,405.1 ns | 1,397.1 ns | 1,423.6 ns | 0.0626 |     - |     - |     408 B |

5.0 performance
|                            Method |     Mean |   Error |  StdDev |   Median |      Min |      Max |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|---------------------------------- |---------:|--------:|--------:|---------:|---------:|---------:|-------:|------:|------:|----------:|
| CaseSensitive_Matching            | 799.2 ns | 4.59 ns | 4.29 ns | 801.0 ns | 790.5 ns | 803.9 ns | 0.0985 |     - |     - |     632 B |
| CaseInsensitive_Matching          | 789.2 ns | 6.62 ns | 5.53 ns | 790.3 ns | 776.0 ns | 794.4 ns | 0.1004 |     - |     - |     632 B |
| CaseSensitive_NotMatching(Missing)| 479.9 ns | 0.75 ns | 0.59 ns | 479.8 ns | 479.1 ns | 481.0 ns | 0.0059 |     - |     - |      40 B |
| CaseInsensitive_NotMatching       | 783.5 ns | 3.26 ns | 2.89 ns | 783.5 ns | 779.0 ns | 789.2 ns | 0.1004 |     - |     - |     632 B |
```

#### TechEmpower improvement

We've spent significant effort improving .NET performance on the TechEmpower benchmark. It made sense to validate these `JsonSerializer` improvements with the [TechEmpower JSON benchmark](https://www.techempower.com/benchmarks/#section=data-r19&hw=ph&test=json). Performance is now ~ 19% better, which should improve the placement of .NET on that benchmark once we update our entries to .NET 5.0. Our goal for the release was to be more competitive with `netty`, which is a common Java webserver.

These changes, and the performance measurements are covered in detail at [dotnet/runtime #37976](https://github.com/dotnet/runtime/pull/37976). There are two sets of benchmarks there. The first is validating performance with the [`JsonSerializer` performance benchmarks](https://github.com/dotnet/performance/tree/master/src/benchmarks/micro/libraries/System.Text.Json/Serializer) that the team maintains. There is an ~8% improvement observed. The next section is for TechEmpower. It measures three different approaches for satisfying the requirements of the TechEmpower JSON benchmark. `SerializeWithCachedBufferAndWriter` is the one we use in the [official benchmark](https://github.com/aspnet/Benchmarks/blob/4e58073ac06642b34c25119c73ef22932c1ece64/src/BenchmarksApps/Kestrel/PlatformBenchmarks/BenchmarkApplication.Json.cs#L29-L33).

|                             Method |     Mean |   Error |  StdDev |   Median |      Min |      Max |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|----------------------------------- |---------:|--------:|--------:|---------:|---------:|---------:|-------:|------:|------:|----------:|
| SerializeWithCachedBufferAndWriter (before)| 155.3 ns | 1.19 ns | 1.11 ns | 155.5 ns | 153.3 ns | 157.3 ns | 0.0038 |     - |     - |      24 B |
| SerializeWithCachedBufferAndWriter (after) | 130.8 ns | 1.50 ns | 1.40 ns | 130.9 ns | 128.6 ns | 133.0 ns | 0.0037 |     - |     - |      24 B |

If we look at `Min` column, we can do some simple math to calculate the improvement: `153.3/128.6 = ~1.19`. That's a 19% improvement.

## Closing

I hope you've enjoyed this deeper dive into records and `JsonSerializer`. They are just two of the many improvement in .NET 5.0. The [Preview 8 post](https://devblogs.microsoft.com/dotnet/announcing-net-5-0-preview-8/) covers a larger set of features, that provides a broader view of the value that's coming in 5.0.

As you know, we're not adding any new features in .NET 5.0 at this point. I'm using these late preview and RC posts to cover all the features we've built. Which ones would you like to see me cover in the RC2 release blog post? I'd like to know what I should focus on.

Please share your experience using RC1 in the comments. Thanks to everyone that has installed .NET 5.0. We appreciate all the engagement and feedback we've received so far.
