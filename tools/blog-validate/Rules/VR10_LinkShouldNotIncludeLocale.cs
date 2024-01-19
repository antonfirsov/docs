using System.Globalization;
using System.Runtime.CompilerServices;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace Microsoft.DotNetBlog;

internal sealed class VR10_LinkShouldNotIncludeLocale : ValidationRule
{
    public override void Validate(ValidationContext context)
    {
        const string host = "microsoft.com";
        var links = context.Document.Descendants<LinkInline>();

        foreach (var link in links)
        {
            if (!UriHelper.TryGetAbsoluteUri(link.Url, out var url))
                continue;

            var isMicrosoftDotCom = url.Host.Equals(host, StringComparison.OrdinalIgnoreCase) ||
                                    url.Host.EndsWith($".{host}", StringComparison.OrdinalIgnoreCase);

            var locale = CultureInfo.GetCultures(CultureTypes.AllCultures)
                                    .Where(c => !string.IsNullOrEmpty(c.Name))
                                    .FirstOrDefault(c => url.Segments.Any(s => s.Equals(c.Name + "/", StringComparison.OrdinalIgnoreCase)));

            if (isMicrosoftDotCom && locale != null)
            {
                var paragraph = link.Parent;
                string content = context.Markdown.Substring(paragraph.Span.Start, paragraph.Span.Length);

                // If the link is contained in a nested stucture like a bulleted list, get the parent markdown
                if (link?.Parent?.ParentBlock?.Parent?.Line == link.Line)
                {
                    var parentSpan = link.Parent.ParentBlock.Parent.Span;
                    content = context.Markdown.Substring(parentSpan.Start, parentSpan.Length);
                }

                string suggestion = content.Replace($"/{locale.Name}/", "/", StringComparison.OrdinalIgnoreCase);
                context.Error(this.GetType().Name, link, $"The host '{url.Host} shouldn't use locales. Remove '{locale.Name}' from the URL.", suggestion);
            }
        }
    }
}