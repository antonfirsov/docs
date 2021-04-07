using System;
using System.Linq;

using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace Microsoft.DotNetBlog
{
    internal sealed class VR09_LinkMustBeHttps : ValidationRule
    {
        private static readonly string[] _knownHosts = new[]
        {
            "aka.ms",
            "dot.net",
            "dotnetfoundation.org",
            "github.com",
            "microsoft.com",
        };

        public override void Validate(ValidationContext context)
        {
            var links = context.Document.Descendants<LinkInline>();    

            foreach (var link in links)
            {              
                var url = new Uri(link.Url, UriKind.RelativeOrAbsolute);

                if (!url.IsAbsoluteUri)
                    continue;

                var isKnownHost = _knownHosts.Any(k => url.Host.Equals(k, StringComparison.OrdinalIgnoreCase) ||
                                                       url.Host.EndsWith("." + k, StringComparison.OrdinalIgnoreCase));
                if (!isKnownHost)
                    continue;

                var isHttp = url.Scheme.StartsWith("http", StringComparison.OrdinalIgnoreCase);
                var isSecure = string.Equals(url.Scheme, "https", StringComparison.OrdinalIgnoreCase);
                if (!isHttp || isSecure)
                    continue;

                context.Error("VR09", link, $"The host '{url.Host}' requires https");
            }
        }
    }
}
