using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

        public IReadOnlyList<Diagnostic> Validate(string fileName, IEnumerable<string> categories)
        {
            var markdown = File.ReadAllText(fileName);
            var document = BlogMarkdown.Parse(markdown);
            var context = new ValidationContext(document, fileName, categories);

            foreach (var rule in _rules)
                rule.Validate(context);

            return context.Diagnostics;
        }
    }
}
