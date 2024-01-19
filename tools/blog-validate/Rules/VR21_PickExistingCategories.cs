namespace Microsoft.DotNetBlog;

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

        var diagnosticSpan = context.Document.GetFrontMatterDiagnosticSpan(BlogFrontMatterFields.Categories);

        foreach (var unknownCategory in unknownCategories)
            context.Error(this.GetType().Name, diagnosticSpan, $"Category '{unknownCategory}' doesn't exist. If you need to create it, please add it to categories.txt in the repo root.");
    }
}
