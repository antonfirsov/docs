using System;
using System.IO;
using System.Linq;

using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace BlogValidator
{
    internal sealed class VR17_AvoidJpeg : ValidationRule
    {
        public override void Validate(ValidationContext context)
        {
            var links = context.Document.Descendants<LinkInline>()
                                        .Where(i => i.IsImage);

            foreach (var link in links)
            {
                var url = new Uri(link.Url, UriKind.RelativeOrAbsolute);
                var local = url.IsAbsoluteUri ? url.LocalPath : link.Url;
                var extension = Path.GetExtension(local);

                var isJpeg = string.Equals(extension, ".jpg", StringComparison.OrdinalIgnoreCase) ||
                             string.Equals(extension, ".jpeg", StringComparison.OrdinalIgnoreCase);

                if (isJpeg)
                    context.Warning("VR17", link, "Avoid JPEG files for screenshots and use PNG instead.");
            }
        }
    }
}
