using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace Microsoft.DotNetBlog
{
    internal sealed class Validator
    {
        private readonly IReadOnlyList<ValidationRule> _rules;

        public Validator()
        {
            _rules = GetRules();
        }

        private static IReadOnlyList<ValidationRule> GetRules()
        {
            return typeof(ValidationRule).Assembly
                                         .GetTypes()
                                         .Where(t => !t.IsAbstract && typeof(ValidationRule).IsAssignableFrom(t))
                                         .Select(t => (ValidationRule)Activator.CreateInstance(t))
                                         .ToArray();
        }

        public IReadOnlyList<Diagnostic> Validate(string rootDirectory, string fileName, IEnumerable<string> categories)
        {
            var markdown = File.ReadAllText(fileName);
            var document = BlogMarkdown.Parse(markdown);

            var context = new ValidationContext(rootDirectory, document, fileName, categories);
            var links = document.Descendants<LinkInline>().Select(l => l.Url).ToHashSet();

            Console.WriteLine($"Root       : {context.RootDirectory}");
            Console.WriteLine($"Path       : {context.FileName}");
            Console.WriteLine($"Title      : {context.FrontMatter?.PostTitle}");
            Console.WriteLine($"Summary    : {context.FrontMatter?.Summary}");
            Console.WriteLine($"Categories : {context.FrontMatter?.Categories}");
            Console.WriteLine($"Image      : {context.FrontMatter?.FeaturedImage}");
            Console.WriteLine($"Username   : {context.FrontMatter?.Username}");
            Console.WriteLine($"Alias      : {context.FrontMatter?.MicrosoftAlias}");
            Console.WriteLine($"#Links     : {links.Count:N0}");

            foreach (var rule in _rules)
                rule.Validate(context);

            return context.Diagnostics;
        }
    }
}
