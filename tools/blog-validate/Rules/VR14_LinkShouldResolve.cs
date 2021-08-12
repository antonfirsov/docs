using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;

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

            var validatedLinks = new Dictionary<string, ValidationResult>(StringComparer.Ordinal);

            foreach (var link in links)
            {
                if (!validatedLinks.TryGetValue(link.Url, out var validationResult))
                {
                    validationResult = Validate(client, context.FileName, link);
                    validatedLinks.Add(link.Url, validationResult);
                }

                if (validationResult is not null)
                {
                    if (validationResult.IsError)
                        context.Error(validationResult.Id, link, validationResult.Message);
                    else
                        context.Warning(validationResult.Id, link, validationResult.Message);
                }
            }
        }

        private static ValidationResult Validate(HttpClient client, string fileName, LinkInline link)
        {
            var retryCount = 3;
            Retry:

            if (retryCount-- == 0)
                return new ValidationResult(false, "VR14", "Couldn't validate URL.");

            if (UriHelper.TryGetAbsoluteUri(link.Url, out var url))
            {
                var isHttp = url.Scheme.StartsWith("http", StringComparison.OrdinalIgnoreCase);
                if (!isHttp)
                    return null;

                try
                {
                    using var response = client.GetAsync(url).GetAwaiter().GetResult();

                    if (response.StatusCode == HttpStatusCode.TooManyRequests)
                    {
                        var delay = TimeSpan.FromSeconds(10);

                        if (response.Headers.TryGetValues("Retry-After", out var retryAfterValues))
                        {
                            foreach (var value in retryAfterValues)
                            {
                                if (int.TryParse(value, out var seconds))
                                {
                                    delay = TimeSpan.FromSeconds(seconds);
                                }
                            }
                        }

                        Thread.Sleep(delay);

                        goto Retry;
                    }

                    if (IsForwardLink(url) && !IsForwarded(response))
                        throw new Exception("The URL wasn't forwarded");

                    if (IsForwarded(response))
                        return null;

                    response.EnsureSuccessStatusCode();
                }
                catch (Exception ex)
                {
                    return new ValidationResult(false, "VR14", $"URL '{url}' doesn't resolve: {ex.Message}");
                }
            }
            else if (UriHelper.TryGetRelativeUri(link.Url, out url))
            {
                var markdownDirectory = Path.GetDirectoryName(fileName);
                var fullPath = Path.Join(markdownDirectory, link.Url);
                if (!File.Exists(fullPath))
                    return new ValidationResult(true, "VR20", $"Relative URL '{url}' doesn't resolve to file in the repository");
            }

            return null;
        }

        private static bool IsForwardLink(Uri url)
        {
            return string.Equals(url.Host, "aka.ms", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(url.Host, "go.microsoft.com", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsForwarded(HttpResponseMessage response)
        {
            return response.StatusCode == HttpStatusCode.Moved ||
                   response.StatusCode == HttpStatusCode.MovedPermanently;
        }

        private sealed class ValidationResult
        {
            public ValidationResult(bool isError, string id, string message)
            {
                IsError = isError;
                Id = id;
                Message = message;
            }

            public bool IsError { get; }
            public string Id { get; }
            public string Message { get; }
        }
    }
}
