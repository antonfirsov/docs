namespace Microsoft.DotNetBlog;

internal sealed class VR26_SummaryIsRequired : ValidationRule
{
    public override void Validate(ValidationContext context)
    {
        if (context.FrontMatter != null)
        {
            var diagnosticSpan = context.Document.GetFrontMatterDiagnosticSpan(BlogFrontMatterFields.Summary);

            if (context.FrontMatter.Summary == "Summary of your post, shown on the home page next to the featured image")
                context.Error("VR26", diagnosticSpan, "'summary' must be set to a value other than the default");
        }
    }
}
