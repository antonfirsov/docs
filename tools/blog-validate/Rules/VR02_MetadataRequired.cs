namespace Microsoft.DotNetBlog;

internal sealed class VR02_MetadataRequired : ValidationRule
{
    public override void Validate(ValidationContext context)
    {
        if (context.FrontMatter != null)
        {
            if (string.IsNullOrEmpty(context.FrontMatter.PostTitle))
                MustSpecifyField(context, BlogFrontMatterFields.PostTitle);

            if (string.IsNullOrEmpty(context.FrontMatter.FeaturedImage))
                MustSpecifyField(context, BlogFrontMatterFields.FeaturedImage);

            if (string.IsNullOrEmpty(context.FrontMatter.Summary))
                MustSpecifyField(context, BlogFrontMatterFields.Summary);

            if (string.IsNullOrEmpty(context.FrontMatter.Author1))
                MustSpecifyField(context, BlogFrontMatterFields.Author1);

            if (string.IsNullOrEmpty(context.FrontMatter.Author2) && !string.IsNullOrEmpty(context.FrontMatter.Author3))
                MustSpecifyField(context, BlogFrontMatterFields.Author3, $"Must specify '{BlogFrontMatterFields.Author2}' when '{BlogFrontMatterFields.Author3}' is specified.");

            if (string.IsNullOrEmpty(context.FrontMatter.Categories))
                MustSpecifyField(context, BlogFrontMatterFields.Categories);

            if (string.IsNullOrEmpty(context.FrontMatter.MicrosoftAlias))
                MustSpecifyField(context, BlogFrontMatterFields.MicrosoftAlias);

            if (context.FrontMatter.PostDate == null)
                MustSpecifyField(context, BlogFrontMatterFields.PostDate);

            static void MustSpecifyField(ValidationContext context, string field, string? text = null)
            {
                text ??= $"Must specify '{field}'";
                context.Error("VR02", context.Document.GetFrontMatterDiagnosticSpan(field), text);
            }
        }
    }
}
