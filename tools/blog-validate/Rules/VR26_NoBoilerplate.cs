namespace Microsoft.DotNetBlog;

internal sealed class VR26_NoBoilerplate : ValidationRule
{
    private readonly (string FieldName, Func<BlogFrontMatter, string?> FieldGetter, string BoilerplateText)[] _boilerplate =
    {
        (FieldName: BlogFrontMatterFields.Summary, fm => fm.Summary, "Summary of your post, shown on the home page next to the featured image"),
        (FieldName: BlogFrontMatterFields.Tags, fm => fm.Tags, "tag1, tag2, tag3")
    };

    public override void Validate(ValidationContext context)
    {
        if (context.FrontMatter is null)
            return;

        foreach (var (fieldName, fieldGetter, boilerplateText) in _boilerplate)
        {
            var diagnosticSpan = context.Document.GetFrontMatterDiagnosticSpan(fieldName);
            var actualText = fieldGetter(context.FrontMatter);
            
            if (actualText == boilerplateText)
                context.Error("VR26", diagnosticSpan, $"'{fieldName}' must be set to a value other than the default");
        }
    }
}
