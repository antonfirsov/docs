namespace Microsoft.DotNetBlog;

internal sealed class VR28_PostDateMustBeValidDateTime : ValidationRule
{
    public override void Validate(ValidationContext context)
    {
        var postDate = context.FrontMatter?.PostDate;

        if (postDate != null && postDate.Value.TimeOfDay == TimeSpan.Zero)
        {
            string suggestion = $"{BlogFrontMatterFields.PostDate}: {context.FrontMatter.PostDateText!}  10:00:00";

            context.Error("VR28_PostDateMustBeValidDateTime", context.Document.GetFrontMatterDiagnosticSpan(BlogFrontMatterFields.PostDate), "The post_date must also include a time in this format (default is Pacific Time): yyyy-MM-dd HH:mm:ss", suggestion);
        }
    }
}
