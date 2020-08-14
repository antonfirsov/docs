# New C# Source Generator Samples

Phillip introduced C# Source Generators [here](https://devblogs.microsoft.com/dotnet/introducing-c-source-generators/).
This post describes two new generators that we added to the [samples project](https://github.com/dotnet/roslyn-sdk/tree/master/samples/CSharp/SourceGenerators) in the Roslyn SDK github repo.

The first generator gives you strongly typed access to [CSV](https://en.wikipedia.org/wiki/Comma-separated_values) data. The second one creates string constants based on [Mustache](https://mustache.github.io/mustache.5.html) specifications.

## Source Generators Overview

It is important to have a good mental picture of how source generators operate. Conceptually, a generator is a function that takes some input (more on that later) and generates C# code as output. This 'function' runs *before* the code for the main project is compiled. In fact, its output becomes part of the project.

The inputs to a generator must be available at compile time, because that's when generators run. In this post we explore two different ways to provide it.

You use a generator in your project by either referencing a generator project or by referencing the generator assembly directly. In the samples project this is achieved by the following instruction in the project file:

```xml
<ItemGroup>
    <ProjectReference Include="..\SourceGeneratorSamples\SourceGeneratorSamples.csproj"
                            OutputItemType="Analyzer" ReferenceOutputAssembly="false" />
</ItemGroup>
```

## CSV Generator Usage

The CSV Generator takes as an input CSV files and returns strongly typed C# representations of them as output. You specify the CSV files with the following lines in the project file:

```xml
<ItemGroup>
    <AdditionalFiles Include="People.csv" CsvLoadType="Startup" />
    <AdditionalFiles Include="Cars.csv" CsvLoadType="OnDemand" CacheObjects="true" />
</ItemGroup>
```

Where the `People.csv` file looks like so:

```csv
Name, address, 11Age
"Luca Bol", "23 Bell Street", 90
"john doe", "32 Carl street", 45
```

There are two additional arguments that get passed as part of the input in the project file `AdditionalFiles` tag: `CsvLoadType` and `CacheObjects`. `CsvLoadType` can take the value of `Startup` or `OnDemand`: the former instruct the code to load the objects representing the CSV file when the program starts; the latter loads them at first usage. `CacheObjects` is a `bool` indicating if the objects need to be cached after creation.

It can be a little confusing to keep straight when exactly every phase runs. The generation of classes representing the shape of the CSV file happens at *compile time*, while the creation of the objects for each row of the file happens at *run time* according to the policy specified by `CsvLoadType` and `CacheObjects`.

BTW: the `11Age` column name came about as a way to test that the C# generation is correct in case of columns starting with a number.

Given such input, the generator creates a `CSV` namespace that you can import in your code with:

```c#
using CSV;
```

In the namespace there is one class for each CSV file. Each class contains an `All` static property that can be used like so:

```c#
WriteLine("## CARS");
Cars.All.ToList().ForEach(c => WriteLine($"{c.Brand}\t{c.Model}\t{c.Year}\t{c.Cc}"));
WriteLine("\n## PEOPLE");
People.All.ToList().ForEach(p => WriteLine($"{p.Name}\t{p.Address}\t{p._11Age}"));
```

So that's how you use the generator. Let's now look at how it is implemented.

## CSV Generator Implementation

Inside the [generator project](https://github.com/dotnet/roslyn-sdk/tree/master/samples/CSharp/SourceGenerators/SourceGeneratorSamples) you need a class implementing the `ISourceGenerator` interface with a `Generator` attribute.

```c#
[Generator]
public class CSVGenerator : ISourceGenerator
```

The `Execute` method is the entry point. It gets called by the compiler to start the generation process. Ours looks like this:

```c#
public void Execute(SourceGeneratorContext context)
{
    IEnumerable<(CsvLoadType, bool, AdditionalText)> options = GetLoadOptions(context);
    IEnumerable<(string, string)> nameCodeSequence = SourceFilesFromAdditionalFiles(options);
    foreach ((string name, string code) in nameCodeSequence)
        context.AddSource($"Csv_{name}", SourceText.From(code, Encoding.UTF8));
}
```

We first get the options - `CsvLoadType` and `CacheObjects` from the project file - we then generate the source files by reading the additional files and add them to the project.

Getting the options is just a few easy calls to the analyzer apis:

```c#
static IEnumerable<(CsvLoadType, bool, AdditionalText)> GetLoadOptions(SourceGeneratorContext context)
{
    foreach (AdditionalText file in context.AdditionalFiles)
    {
        if (Path.GetExtension(file.Path).Equals(".csv", StringComparison.OrdinalIgnoreCase))
        {
            // are there any options for it?
            context.AnalyzerConfigOptions.GetOptions(file)
                .TryGetValue("build_metadata.additionalfiles.CsvLoadType", out string? loadTimeString);
            Enum.TryParse(loadTimeString, ignoreCase: true, out CsvLoadType loadType);

            context.AnalyzerConfigOptions.GetOptions(file)
                .TryGetValue("build_metadata.additionalfiles.CacheObjects", out string? cacheObjectsString);
            bool.TryParse(cacheObjectsString, out bool cacheObjects);

            yield return (loadType, cacheObjects, file);
        }
    }
}
```

Once the options are retrieved, the process of generating C# source files to represent the CSV data can start.

```c#
static IEnumerable<(string, string)> SourceFilesFromAdditionalFile(CsvLoadType loadTime,
    bool cacheObjects, AdditionalText file)
{
    string className = Path.GetFileNameWithoutExtension(file.Path);
    string csvText = file.GetText()!.ToString();
    return new (string, string)[] { (className, GenerateClassFile(className, csvText, loadTime, cacheObjects)) };
}

static IEnumerable<(string, string)> SourceFilesFromAdditionalFiles(IEnumerable<(CsvLoadType loadTime,
    bool cacheObjects, AdditionalText file)> pathsData)
    => pathsData.SelectMany(d => SourceFilesFromAdditionalFile(d.loadTime, d.cacheObjects, d.file));
```

We iterate over all the CSV files and generate a class file for each one of them by calling `GenerateClassFile`. This is where the magic happens: we look at the csv content and we generate the correct class file to add to the project.

But this is a long function ([code](https://github.com/dotnet/roslyn-sdk/blob/master/samples/CSharp/SourceGenerators/SourceGeneratorSamples/CsvGenerator.cs)), so let's just look at the start and the end of it to get the flavour.

```c#
public static string GenerateClassFile(string className, string csvText, CsvLoadType loadTime,
    bool cacheObjects)
{
    StringBuilder sb = new StringBuilder();
    using CsvTextFieldParser parser = new CsvTextFieldParser(new StringReader(csvText));

    //// Usings
    sb.Append(@"
#nullable enable
namespace CSV {
using System.Collections.Generic;

");
    //// Class Definition
    sb.Append($"    public class {className} {{\n");
```

First we add a new class to the `CSV` namespace. The name of the class corresponds to the CSV file name. Then we generate the code for the class and return it.

```c#
    // CODE TO GENERATE C# FROM THE CSV FILE ...

    sb.Append("            }\n        }\n    }\n}\n");
    return sb.ToString();
}
```

In the end, the compiler adds to our project
a file called `Csv_People.cs` containing the code below.

```c#
#nullable enable
namespace CSV {
    using System.Collections.Generic;

    public class People {

        static People() { var x = All; }
        public string Name { get; set;} = default!;
        public string Address { get; set;} = default!;
        public int _11Age { get; set;} = default!;

        static IEnumerable<People>? _all = null;

        public static IEnumerable<People> All {
            get {

                List<People> l = new List<People>();
                People c;
                c = new People();
                c.Name = "Luca Bol";
                c.Address = "23 Bell Street";
                c._11Age =  90;
                l.Add(c);
                c = new People();
                c.Name = "john doe";
                c.Address = "32 Carl street";
                c._11Age =  45;
                l.Add(c);
                _all = l;
                return l;
            }
        }
    }
}
```

This is what gets compiled into your project, so that you can reference it from the code.

## Mustache Generator Usage

For the Mustage Generator, we use a different way to pass input arguments compared to the CSV Generator above.
We embed our input in assembly attributes and then, in the generator code, we fish them out of the assembly to
drive the generation process.

In our client code, we pass the inputs to the generator as below:

```c#
using Mustache;

[assembly: Mustache("Lottery", t1, h1)]
[assembly: Mustache("HR", t2, h2)]
[assembly: Mustache("HTML", t3, h3)]
[assembly: Mustache("Section", t4, h4)]
[assembly: Mustache("NestedSection", t5, h5)]
```

The first argument to the `Mustache` attribute is the name of a static property that gets generated in the `Mustache.Constants` class.

The second argument represents the mustache template to use. In the demo we use the templates from the [manual](https://mustache.github.io/mustache.5.html).
For example:

```c#
public const string t1 = @"
Hello {{name}}
You have just won {{value}} dollars!
{{#in_ca}}
Well, {{taxed_value}} dollars, after taxes.
{{/in_ca}}
";
```

The third argument is the hash to use with the template.

```c#
public const string h1 = @"
{
""name"": ""Chris"",
""value"": 10000,
""taxed_value"": 5000,
""in_ca"": true
}
";
```

Each attribute instance is a named pair (template, hash). Our generator uses it to generate a string constant that you can access like this:

```c#
WriteLine(Mustache.Constants.Lottery);
```

The resulting output is good for Chris, as expected:

```shell
Hello Chris
You have just won 10000 dollars!
Well, 6000.0 dollars, after taxes.
```

## Mustache Generator Implementation

The input to this generator is quite different from the previous one, but the implementation is similar. Or at least it has a familiar 'shape'. As before there is a class implementing `ISourceGenerator` with an `Execute` method:

```c#
[Generator]
public class MustacheGenerator : ISourceGenerator
{
    public void Execute(SourceGeneratorContext context)
    {
        string attributeSource = @"
[System.AttributeUsage(System.AttributeTargets.Assembly, AllowMultiple=true)]
internal sealed class MustacheAttribute: System.Attribute
{
    public string Name { get; }
    public string Template { get; }
    public string Hash { get; }
    public MustacheAttribute(string name, string template, string hash)
        => (Name, Template, Hash) = (name, template, hash);
}
";
        context.AddSource("Mustache_MainAttributes__", SourceText.From(attributeSource, Encoding.UTF8));
```

First we need to add a source file to the project to define the Mustache attribute that will be used by the clients to specify the inputs.

Then we inspect the assembly to fish out all the usages of the `Mustache` attribute.

```c#
        Compilation compilation = context.Compilation;

        IEnumerable<(string, string, string)> options = GetMustacheOptions(compilation);
```

The code to do so is in the `GetMustacheOptions` function, that you can inspect [here](https://github.com/dotnet/roslyn-sdk/blob/master/samples/CSharp/SourceGenerators/SourceGeneratorSamples/MustacheGenerator.cs).

Once you have the options, it is time to generate the source files:

```c#
static string SourceFileFromMustachePath(string name, string template, string hash)
{
    Func<object, string> tree = HandlebarsDotNet.Handlebars.Compile(template);
    object @object = Newtonsoft.Json.JsonConvert.DeserializeObject(hash);
    string mustacheText = tree(@object);

    return GenerateMustacheClass(name, mustacheText);
}
```

First we use [Handlebars.net](https://github.com/rexm/Handlebars.Net) to create the string constant text (first 3 lines above).
We then move on to the task of generating the property to contain it.

```c#
private static string GenerateMustacheClass(string className, string mustacheText)
{
    StringBuilder sb = new StringBuilder();
    sb.Append($@"
namespace Mustache {{

public static partial class Constants {{

public const string {className} = @""{mustacheText.Replace("\"", "\"\"")}"";
}}
}}
");
    return sb.ToString();

}
```

That was easy, mainly thanks to C# partial classes. We generate a single class from multiple source files.

## Conclusion

C# Source Generators are a great addition to the compiler. The ability to interject yourself in the middle of the compilation process and have access to the source tree, makes it possible, even simple, to enable all sorts of scenarios (i.e. domain languages, code interpolation, automatic optimizations ...). We look forward to you surprising us with your own Source Generators!
