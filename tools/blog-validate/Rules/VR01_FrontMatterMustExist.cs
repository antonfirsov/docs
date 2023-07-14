namespace Microsoft.DotNetBlog;

internal sealed class VR01_FrontMatterMustExist : ValidationRule
{
    public override void Validate(ValidationContext context)
    {
        if (context.FrontMatter == null)
            context.Error("VR01", context.Document, "Valid front matter is required, see /template/blank.md. Verify your front mater is valid YAML, quoting your title and summary if needed.");
    }
}
