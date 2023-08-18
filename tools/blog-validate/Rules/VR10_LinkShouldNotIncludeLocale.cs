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
                string suggestion = link.Url.Replace($"/{locale.Name}/", "/", StringComparison.OrdinalIgnoreCase);
                context.Error("VR10", link, $"The host '{url.Host} shouldn't use locales. Remove '{locale.Name}' from the URL.", suggestion);
            }
        }
    }
}
