using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace Microsoft.DotNetBlog
{
    internal sealed class VR14_LinkShouldResolve : ValidationRule
    {
        public override void Validate(ValidationContext context)
        {
            var links = context.Document.Descendants<LinkInline>();

            var client = new HttpClient();

            // Some CDNs, such as Akamai, will return 404 unless a UserAgent is specified.
            var assemblyName = GetType().Assembly.GetName();
            var productName = assemblyName.Name;
            var productVersion = assemblyName.Version.ToString();
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(productName, productVersion));

            foreach (var link in links)
            {
                if (!UriHelper.TryGetRelativeOrAbsoluteUri(link.Url, out var url))
                    continue;

                if (url.IsAbsoluteUri)
                {
                    var isHttp = url.Scheme.StartsWith("http", StringComparison.OrdinalIgnoreCase);
                    if (!isHttp)
                        continue;

                    try
                    {
                        using var response = client.GetAsync(url).GetAwaiter().GetResult();

                        if (IsForwardLink(url) && !IsForwarded(response))
                            throw new Exception("The URL wasn't forwarded");

                        if (IsForwarded(response))
                            continue;

                        response.EnsureSuccessStatusCode();
                    }
                    catch (Exception ex)
                    {
                        context.Warning("VR14", link, $"URL '{url}' doesn't resolve: {ex.Message}");
                    }
                }
                else
                {
                    var markdownDirectory = Path.GetDirectoryName(context.FileName);
                    var fullPath = Path.Join(markdownDirectory, link.Url);
                    if (!File.Exists(fullPath))
                    {
                        context.Error("VR20", link, $"Relative URL '{url}' doesn't resolve to file in the repository");
                    }
                }
            }
        }      

        private bool IsForwardLink(Uri url)
        {
            return string.Equals(url.Host, "aka.ms", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(url.Host, "go.microsoft.com", StringComparison.OrdinalIgnoreCase);
        }

        private bool IsForwarded(HttpResponseMessage response)
        {
            return response.StatusCode == HttpStatusCode.Moved ||
                   response.StatusCode == HttpStatusCode.MovedPermanently;
        }
    }
}
