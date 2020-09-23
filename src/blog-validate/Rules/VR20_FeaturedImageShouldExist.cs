using System;
using System.IO;

namespace BlogValidator
{
    internal sealed class VR20_FeaturedImageShouldExist : ValidationRule
    {
        public override void Validate(ValidationContext context)
        {
            if (context.FrontMatter?.FeaturedImage is string relativePath)
            {
                var url = new Uri(relativePath, UriKind.RelativeOrAbsolute);
                if (!url.IsAbsoluteUri)
                {
                    var markdownDirectory = Path.GetDirectoryName(context.FileName);
                    var fullPath = Path.Join(markdownDirectory, relativePath);
                    if (!File.Exists(fullPath))
                    {
                        var diagnosticSpan = context.Document.GetFrontMatterDiagnosticSpan();
                        context.Warning("VR20", diagnosticSpan, $"'featured_image' must refer to a file that exists.");
                    }
                }
            }
        }
    }
}
