using System;
using System.Linq;

namespace BlogValidator
{
    internal sealed class VR21_PickExistingCategories : ValidationRule
    {
        public override void Validate(ValidationContext context)
        {
            if (string.IsNullOrEmpty(context.FrontMatter?.Categories))
                return;

            var categories = context.FrontMatter.Categories.Split(',')
                                                           .Select(c => c.Trim())
                                                           .ToArray();

            var unknownCategories = categories.Where(c => !context.Categories.Contains(c));

            foreach (var unknownCategory in unknownCategories)
                context.Error("VR21", context.Document.GetFrontMatterDiagnosticSpan(), $"Category '{unknownCategory}' doesn't exist. If you need to create it, please add it to categories.txt in the repo root.");
        }
    }
}
