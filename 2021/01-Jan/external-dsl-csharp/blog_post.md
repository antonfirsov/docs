---
post_title: 'Using C# Source Generators to create an external DSL'
username: lucabol@microsoft.com
microsoft_alias: lucabol@microsoft.com
featured_image:
categories: .NET, .NET Core, C#
summary: This post looks at creating an external DSL to represent mathematical expressions using C# generators.
desired_publication_date: '2021-01-25'
---

This post looks at how to use C# Source Generators to build an [external DSL](https://en.wikipedia.org/wiki/Domain-specific_language) to represent mathematical expressions.

The code for this post is on the [roslyn-sdk repository](https://github.com/dotnet/roslyn-sdk/blob/master/samples/CSharp/SourceGenerators/SourceGeneratorSamples/MathsGenerator.cs).

## A recap of C# Source Generators

There are two other articles describing C# Source Generators on this blog, [Introducing C# Source Generators](https://devblogs.microsoft.com/dotnet/introducing-c-source-generators/) and [New C# Source Generator Samples](https://devblogs.microsoft.com/dotnet/new-c-source-generator-samples/). If you're new to generators, you might want to read them first.

Let's just remind ourselves of what they are. You can think of a Source Generator as a function that runs at compile time. It takes some inputs and produces C# code.

```dotnetcli
Program Parse Tree -> Additional Files -> File Specific Options -> C# Code
```

This conceptual view is implemented in the `ISourceGenerator` interface.

```csharp
    public interface ISourceGenerator {
        void Execute(GeneratorExecutionContext context);
        void Initialize(GeneratorInitializationContext context);
}
```

You implement the `Execute` method and get the inputs through the `context` object. The `Initialize` function is more rarely used.


The `context` parameter to `Execute` contains the inputs.

* `context.Compilation` is the parse tree for the program and everything else needed by the compiler (settings, references, etc.).
* `context.AdditionalFiles` gives you the additional files in the project.
* `context.AnalyzerConfigOptions.GetOptions` provides the options for each additional file.

The additional files are added to the project file using this syntax. Also, notice the file specific options that you can retrieve in your generator code.

```xml
<AdditionalFiles Include="Cars.csv" CsvLoadType="OnDemand" CacheObjects="true" />
```

You are not limited to these inputs. A C# generator is just a bit of code that runs at compile time. The code can do whatever it pleases. For example, it could download information from a website (not a good idea). But the three inputs above are the most logical ones as they are part of the project. It is the recommended way to do it.

As a side note, a different source generators' metaphor is the anthropomorphization of the compiler. Mrs. Compiler goes about her business of generating the parse tree and then she stops and asks you: "Do you have anything to add to what I have done so far?"

## The scenario

You work for an engineering company that employes many mathematicians. The formulas that underpin the business are spread out through the large C# codebase. The company would like to centralize them and make them easy to write and understand for their mathematicians.

They would like the calculations to be written in pure math, but have the same performance as C# code. For example, they would like the code to end up being inlined at the point of usage. Here is an example of what they would like to write:

```dotnetcli
AreaSquare(l)       = pow(l, 2)
AreaRectangle(w, h) = w * h
AreaCircle(r)       = pi * r * r
Quadratic(a, b, c)  = {-b + sqrt(pow(b,2) - 4 * a * c)} / (2 * a)

GoldenRatio         = 1.61803
GoldHarm(n)         = GoldenRatio + 1 * ∑(i, 1, n, 1 / i)

D(x', x'', y', y'') = sqrt(pow([x'-x''],2) + pow([y'-y''], 2))
```

You notice several things that differentiate this language from C#:

1. No type-annotations.
2. Different kinds of parenthesis.
3. Invalid C# characters in identifiers.
4. Special syntax for the summation symbol (`∑`).

Despite the differences, the language structure is similar to C# methods and properties. You think you should be able to translate each line of the language to a snippet of valid C# code.

You decide to use Source Generators for this task because they plug directly into the normal compiler workflow and because in the future the code might need to access the parse tree for the enclosing program.

One could use Regex substitutions to go from this language to C#, but that approach is problematic for two reasons.

1. The language structure is not completely identical to C# (i.e., you need to generate special code for `∑`)
2. More importantly, you expose yourself to [code injection attack](https://owasp.org/www-community/attacks/Code_Injection). A disgruntled mathematician could write code to mint bitcoins inside your language. By properly parsing the language you can whitelist the available functions.

## Hooking up the inputs

Here is the implementation of the `Execute` method for the `ISourceGenerator` interface.

```c#
        public void Execute(GeneratorExecutionContext context)
        {
            
            foreach (AdditionalText file in context.AdditionalFiles)
            {
                if (Path.GetExtension(file.Path).Equals(".math", StringComparison.OrdinalIgnoreCase))
                {
                    if(!libraryIsAdded)
                    {
                        context.AddSource("___MathLibrary___.cs", SourceText.From(libraryCode, Encoding.UTF8));
                        libraryIsAdded = true;
                    }
                    // Load formulas from .math files
                    var mathText = file.GetText();
                    var mathString = "";

                    if(mathText != null)
                    {
                        mathString = mathText.ToString();
                    } else
                    {
                        throw new Exception($"Cannot load file {file.Path}");
                    }

                    // Get name of generated namespace from file name
                    string fileName = Path.GetFileNameWithoutExtension(file.Path);

                    // Parse and gen the formulas functions
                    var tokens = Lexer.Tokenize(mathString);
                    var code = Parser.Parse(tokens);

                    var codeFileName = $@"{fileName}.cs";
    
                    context.AddSource(codeFileName, SourceText.From(code, Encoding.UTF8));
                }
            }
        }
```

The code scans the additional files from the project file and operates on the ones with the extension `.math`.

Firstly, it adds to the project a C# library file containing some utility functions. Then it gets the text for the Math file (aka the formulas), parses the language, and generates C# code for it.

This snippet is the minimum code to hook up a new language into your C# project. You can do more here. You can inspect the parse tree or gather more options to influence the way the language is parsed and generated, but this is not necessary in this case.

## Writing the parser

This section is standard compiler fare. If you are familiar with lexing, parsing, and generating code, you can jump directly to the next section. If you are curious, read on.

We are implementing the following two lines from the code above.

```c#
var tokens = Lexer.Tokenize(mathString);
var code = Parser.Parse(tokens);
```

The goal of these lines is to take the Math language and generate the following valid C# code. You can then call any of the generated functions from your existing code.

```c#
using static System.Math;
using static ___MathLibrary___.Formulas; // For the __MySum__ function

namespace Maths {

    public static partial class Formulas {

        public static double  AreaSquare (double  l ) => Pow ( l , 2 ) ;
        public static double  AreaRectangle (double  w ,double  h ) => w * h ;
        public static double  AreaCircle (double  r ) => PI * r * r ;
        public static double  Quadratic (double  a ,double  b ,double  c ) => ( - b + Sqrt ( Pow ( b , 2 ) - 4 * a * c ) ) / ( 2 * a ) ;

        public static double  GoldenRatio => 1.61803 ;
        public static double  GoldHarm (double  n ) => GoldenRatio + 1 * ___MySum___ ((int) 1 ,(int) n ,i =>  1 / i ) ;

        public static double  D (double  xPrime ,double  xSecond ,double  yPrime ,double  ySecond ) => Sqrt ( Pow ( ( xPrime - xSecond ) , 2 ) + Pow ( ( yPrime - ySecond ) , 2 ) ) ;

    }
}
```

I just touch on the most important points of the implementation, the full code is [here](https://github.com/dotnet/roslyn-sdk/blob/master/samples/CSharp/SourceGenerators/SourceGeneratorSamples/MathsGenerator.cs).

This is not production code. For the sake of simplicity, I had to fit it in one sample file without external dependencies. It is probably wiser to use a [parser generator](https://en.wikipedia.org/wiki/Comparison_of_parser_generators) to future-proof the implementation and avoid errors.

With such caveats out of the way, the lexer is Regex based. It uses the following `Token` definition and Regexps.

```c#
    public enum TokenType {
        Number,
        Identifier,
        Operation,
        OpenParens,
        CloseParens,
        Equal,
        EOL,
        EOF,
        Spaces,
        Comma,
        Sum,
        None
    }

    public struct Token {
        public TokenType Type;
        public string Value;
        public int Line;
        public int Column;
    }

/// ... More code not shown

        static (TokenType, string)[] tokenStrings = {
            (TokenType.EOL,         @"(\r\n|\r|\n)"),
            (TokenType.Spaces,      @"\s+"),
            (TokenType.Number,      @"[+-]?((\d+\.?\d*)|(\.\d+))"),
            (TokenType.Identifier,  @"[_a-zA-Z][`'""_a-zA-Z0-9]*"),
            (TokenType.Operation,   @"[\+\-/\*]"),
            (TokenType.OpenParens,  @"[([{]"),
            (TokenType.CloseParens, @"[)\]}]"),
            (TokenType.Equal,       @"="),
            (TokenType.Comma,       @","),
            (TokenType.Sum,         @"∑")
        };
```

The `Tokenize` function just goes from the source text to a list of tokens.

```c#

        using Tokens = System.Collections.Generic.IEnumerable<MathsGenerator.Token>;

        static public Tokens Tokenize(string source) {
```

It is too long to show here. Follow the link above for the gory details.

The parser's grammar is described below.

```c#
    /* EBNF for the language
        lines   = {line} EOF
        line    = {EOL} identifier [lround args rround] equal expr EOL {EOL}
        args    = identifier {comma identifier}
        expr    = [plus|minus] term { (plus|minus) term }
        term    = factor { (times|divide) factor };
        factor  = number | var | func | sum | matrix | lround expr rround;
        var     = identifier;
        func    = identifier lround expr {comma expr} rround;
        sum     = ∑ lround identifier comma expr comma expr comma expr rround;
    */
```

It is implemented as a [recursive descendent parser](https://en.wikipedia.org/wiki/Recursive_descent_parser).

The `Parse` function is below and illustrates a few of the design decisions.

```c
        public static string Parse(Tokens tokens) {
            var globalSymbolTable   = new SymTable();
            var symbolTable         = new SymTable();
            var buffer              = new StringBuilder();

            var en = tokens.GetEnumerator();
            en.MoveNext();

            buffer = Lines(new Context {
                tokens = en,
                globalSymbolTable = globalSymbolTable,
                symbolTable = symbolTable,
                buffer = buffer
                });
            return buffer.ToString();

        }

```

* `globalSymbolTable` is used to store the symbols that are whitelisted and the global symbols that are generated during the parsing of the language.
* `symbolTable` is for the parameters to a function and gets cleared at the start of each new line.
* `buffer` contains the C# code that is generated while parsing.
* `Lines` is the first mutually recursive function and maps to the first line of the grammar.

A typical example of one of such recursive functions is below.

```c#
        private static void Line(Context ctx) {
            // line    = {EOL} identifier [lround args rround] equal expr EOL {EOL}

            ctx.symbolTable.Clear();

            while(Peek(ctx, TokenType.EOL))
                Consume(ctx, TokenType.EOL);

            ctx.buffer.Append("\tpublic static double ");

            AddGlobalSymbol(ctx);
            Consume(ctx, TokenType.Identifier);

            if(Peek(ctx, TokenType.OpenParens, "(")) {
                Consume(ctx, TokenType.OpenParens, "("); // Just round parens
                Args(ctx);
                Consume(ctx, TokenType.CloseParens, ")");
            }

            Consume(ctx, TokenType.Equal);
            Expr(ctx);
            ctx.buffer.Append(" ;");

            Consume(ctx, TokenType.EOL);

            while(Peek(ctx, TokenType.EOL))
                Consume(ctx, TokenType.EOL);
        }

```

This shows the manipulation of both symbol tables, the utility functions to advance the tokens stream, the call to the other recursive functions, and emitting the C# code.

Not very elegant, but it gets the job done.

We whitelist all the functions in the `Math` class.

```c#
        static HashSet<string> validFunctions =
            new HashSet<string>(typeof(System.Math).GetMethods().Select(m => m.Name.ToLower()));
```

For most Tokens, there is a straightforward translation to C#.

```c#
        private static StringBuilder Emit(Context ctx, Token token) => token.Type switch
        {
            TokenType.EOL           => ctx.buffer.Append("\n"),
            TokenType.CloseParens   => ctx.buffer.Append(')'), // All parens become rounded
            TokenType.OpenParens    => ctx.buffer.Append('('),
            TokenType.Equal         => ctx.buffer.Append("=>"),
            TokenType.Comma         => ctx.buffer.Append(token.Value),

            // Identifiers are normalized and checked for injection attacks
            TokenType.Identifier    => EmitIdentifier(ctx, token),
            TokenType.Number        => ctx.buffer.Append(token.Value),
            TokenType.Operation     => ctx.buffer.Append(token.Value),
            TokenType.Sum           => ctx.buffer.Append("MySum"),
            _                       => Error(token, TokenType.None)
        };
```

But identifiers need special treatment to check the whitelisted symbols and replace invalid C# characters with valid strings.

```C#
        private static StringBuilder EmitIdentifier(Context ctx, Token token) {
            var val = token.Value;

            if(val == "pi") {
                ctx.buffer.Append("PI"); // Doesn't follow pattern
                return ctx.buffer;
            }

            if(validFunctions.Contains(val)) {
                ctx.buffer.Append(char.ToUpper(val[0]) + val.Substring(1));
                return ctx.buffer;
            }

            string id = token.Value;
            if(ctx.globalSymbolTable.Contains(token.Value) ||
                          ctx.symbolTable.Contains(token.Value)) {
                foreach (var r in replacementStrings) {
                    id = id.Replace(r.Key, r.Value);
                }
                return ctx.buffer.Append(id);
            } else {
                throw new Exception($"{token.Value} not a known identifier or function.");
            }
        }
```

There is a lot more that could be said about the parser. In the end, the implementation is not important. This one is far from perfect.

## Practical advice

As you build your own Source Generators, there are a few things that make the process smoother.

* Write most code in a standard `Console` project. When you are happy with the result, copy and paste it to your source generator. This gives you a good developer experience (i.e., step line by line) for most of your work.
* Once you have copied your code to the source generator, and if you still have problems, use `Debug.Launch` to launch the debugger at the start of the `Execute` function.
* Visual Studio currently has no ability to unload a source generator once loaded. Modifications to the generator itself will only take effect after you closed and reopened your solution.

These are teething problems that hopefully will be fixed in new releases of Visual Studio. For now, you can use the above workarounds.

## Conclusion

Source generators allow you to embed external DSLs into your C# project. This post shows how to do this for a simple mathematical language.
