using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace Microsoft.DotNetBlog;

internal sealed class VR18_AvoidGif : ValidationRule
{
    public override void Validate(ValidationContext context)
    {
        var links = context.Document.Descendants<LinkInline>()
                                    .Where(i => i.IsImage);

        foreach (var link in links)
        {
            if (!UriHelper.TryGetRelativeOrAbsoluteUri(link.Url!, out var url))
                continue;

            var local = url.IsAbsoluteUri ? url.LocalPath : link.Url;
            var extension = Path.GetExtension(local);

            var isGif = string.Equals(extension, ".gif", StringComparison.OrdinalIgnoreCase);

            if (isGif)
                context.Warning(this.GetType().Name, link, "Avoid GIF for videos because they can't be stopped and thus aren't accessible. Use .mp4 instead.");
        }
    }
}
