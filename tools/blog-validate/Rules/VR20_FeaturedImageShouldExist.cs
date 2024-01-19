namespace Microsoft.DotNetBlog;

internal sealed class VR20_FeaturedImageShouldExist : ValidationRule
{
    public override void Validate(ValidationContext context)
    {
        if (context.FrontMatter?.FeaturedImage is string relativePath)
        {
            var diagnosticSpan = context.Document.GetFrontMatterDiagnosticSpan(BlogFrontMatterFields.FeaturedImage);

            if (!UriHelper.TryGetRelativeOrAbsoluteUri(relativePath, out _))
            {
                context.Warning(this.GetType().Name, diagnosticSpan, $"'featured_image' must be a valid URL.");
            }
            else if (UriHelper.TryGetRelativeUri(relativePath, out var url))
            {
                var markdownDirectory = Path.GetDirectoryName(context.FileName);
                var fullPath = Path.Join(markdownDirectory, relativePath);
                if (!File.Exists(fullPath))
                {
                    context.Error(this.GetType().Name, diagnosticSpan, $"'featured_image' must either be an absolute URL or refer to a file in the repository.");
                }
            }
        }
    }
}
