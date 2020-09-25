using System;
using System.Linq;

namespace BlogValidator
{
    internal sealed class VR21_PickExistingCategories : ValidationRule
    {
        private static readonly string[] _knownCategories = new[]
        {
            ".NET",
            ".NET Core",
            ".NET Framework",
            ".NET Internals",
            "AI Machine Learning",
            "Apache",
            "ASP.NET",
            "Async",
            "Azure",
            "Big Data",
            "C#",
            "Code reviews",
            "Concurrency",
            "Docker",
            "Dot.Net",
            "Entity Framework",
            "ErrorProne.NET",
            "F#",
            "GC",
            "Lifecycle",
            "LOH",
            "Machine Learning",
            "Maoni",
            "ML.NET",
            "Performance",
            "Security",
            "Spark for .NET",
            "TPL",
            "Visual Studio",
            "WinForms",
            "WPF",
            "XAML"
        };

        public override void Validate(ValidationContext context)
        {
            if (string.IsNullOrEmpty(context.FrontMatter?.Categories))
                return;

            var categories = context.FrontMatter.Categories.Split(',')
                                                           .Select(c => c.Trim())
                                                           .ToArray();

            var unknownCategories = categories.Where(c => !_knownCategories.Contains(c, StringComparer.OrdinalIgnoreCase));
            var knownCategoryList = string.Join(", ", _knownCategories);

            foreach (var unknownCategory in unknownCategories)
                context.Error("VR21", context.Document.GetFrontMatterDiagnosticSpan(), $"Category '{unknownCategory}' doesn't exist. Valid categories are: {knownCategoryList}.");
        }
    }
}
