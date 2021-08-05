using System;
using System.IO;

namespace Microsoft.DotNetBlog
{
    internal sealed class VR20_FeaturedImageShouldExist : ValidationRule
    {
        public override void Validate(ValidationContext context)
        {
            if (context.FrontMatter?.FeaturedImage is string relativePath)
            {
                var diagnosticSpan = context.Document.GetFrontMatterDiagnosticSpan();

                if (!UriHelper.TryGetRelativeOrAbsoluteUri(relativePath, out var url))
                {
                    context.Warning("VR20", diagnosticSpan, $"'featured_image' must be a valid URL.");
                }
                else if (!url.IsAbsoluteUri)
                {
                    var markdownDirectory = Path.GetDirectoryName(context.FileName);
                    var fullPath = Path.Join(markdownDirectory, relativePath);
                    if (!File.Exists(fullPath))
                    {
                        context.Error("VR20", diagnosticSpan, $"'featured_image' must either be an absolute URL or refer to a file in the repository.");
                    }
                }
            }
        }
    }
}
