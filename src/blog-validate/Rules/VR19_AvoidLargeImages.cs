using System;
using System.IO;
using System.Linq;

using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace BlogValidator
{
    internal sealed class VR19_AvoidLargeImages : ValidationRule
    {
        public override void Validate(ValidationContext context)
        {
            var thresholdBytes = 100 * 1024;

            var links = context.Document.Descendants<LinkInline>()
                                        .Where(i => i.IsImage);

            foreach (var link in links)
            {
                var url = new Uri(link.Url, UriKind.RelativeOrAbsolute);
                if (url.IsAbsoluteUri)
                    continue;

                var local = link.Url;
                var markdownDirectory = Path.GetDirectoryName(context.FileName);
                var path = Path.Join(markdownDirectory, local);
                var info = new FileInfo(path);
                var size = info.Exists ? info.Length : 0;
                if (size > thresholdBytes)
                    context.Warning("VR19", link, $"This image is {size / 1024:N0}kb. You should avoid images larger than {thresholdBytes / 1024:N0}kb.");
            }
        }
    }
}
