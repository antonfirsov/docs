namespace Microsoft.DotNetBlog;

internal sealed class VR16_TitleShouldBe60OrLess : ValidationRule
{
    public override void Validate(ValidationContext context)
    {
        if (context.FrontMatter != null)
        {
            var diagnosticSpan = context.Document.GetFrontMatterDiagnosticSpan(BlogFrontMatterFields.PostTitle);

            if (context.FrontMatter.PostTitle?.Length > 80)
                context.Warning("VR16", diagnosticSpan, "'post_title' should be ideally be 80 characters or less for SEO optimizations.");
        }
    }
}
