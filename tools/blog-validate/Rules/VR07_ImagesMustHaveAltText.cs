using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace Microsoft.DotNetBlog;

internal sealed class VR07_ImagesMustHaveAltText : ValidationRule
{
    public override void Validate(ValidationContext context)
    {
        var images = context.Document.Descendants<LinkInline>()
                                     .Where(i => i.IsImage);

        foreach (var image in images)
        {
            if (string.IsNullOrWhiteSpace(image.FirstChild?.ToString()))
                context.Error(this.GetType().Name, image, "Image must have alt text");
        }
    }
}
