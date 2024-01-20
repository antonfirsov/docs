using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace Microsoft.DotNetBlog;

internal sealed class VR22_LinksMustBeWellFormedUris : ValidationRule
{
    public override void Validate(ValidationContext context)
    {
        var links = context.Document.Descendants<LinkInline>();

        foreach (var link in links)
        {
            if (UriHelper.TryGetRelativeOrAbsoluteUri(link.Url!, out _))
                continue;

            context.Error(this.GetType().Name, link, $"'{link.Url}' is not a valid URI");
        }
    }
}
