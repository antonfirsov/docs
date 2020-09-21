using System.Linq;

using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace BlogValidator
{
    internal sealed class VR07_ImagesMustHaveAltTextRule : ValidationRule
    {
        public override void Validate(ValidationContext context)
        {
            var images = context.Document.Descendants<LinkInline>()
                                         .Where(i => string.IsNullOrWhiteSpace(i.FirstChild?.ToString()));

            foreach (var image in images)
                context.Error("VR07", image, "Image must have alt text");
        }
    }
}
