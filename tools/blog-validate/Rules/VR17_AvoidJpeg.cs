using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace Microsoft.DotNetBlog;

internal sealed class VR17_AvoidJpeg : ValidationRule
{
    public override void Validate(ValidationContext context)
    {
        var links = context.Document.Descendants<LinkInline>()
                                    .Where(i => i.IsImage);

        foreach (var link in links)
        {
            if (!UriHelper.TryGetRelativeOrAbsoluteUri(link.Url, out var url))
                continue;

            var local = url.IsAbsoluteUri ? url.LocalPath : link.Url;
            var extension = Path.GetExtension(local);

            var isJpeg = string.Equals(extension, ".jpg", StringComparison.OrdinalIgnoreCase) ||
                         string.Equals(extension, ".jpeg", StringComparison.OrdinalIgnoreCase);

            if (isJpeg)
                context.Warning(this.GetType().Name, link, "Avoid JPEG files for screenshots and use PNG instead.");
        }
    }
}
