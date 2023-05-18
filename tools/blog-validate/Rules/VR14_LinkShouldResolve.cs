using System.Collections.Concurrent;
using System.Net;

using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace Microsoft.DotNetBlog;

internal sealed class VR14_LinkShouldResolve : ValidationRule
{
    public override void Validate(ValidationContext context)
    {
        var links = context.Document.Descendants<LinkInline>();

        var validatedLinks = new ConcurrentDictionary<string, ValidationResult?>(StringComparer.Ordinal);

        var uniqueLinks = links.Select(l => l.Url)
                               .ToHashSet();

        var options = new ParallelOptions
        {
            MaxDegreeOfParallelism = 4
        };

        Parallel.ForEach(uniqueLinks, options, link =>
        {
            var validationResult = Validate(context.FileName, link);
            validatedLinks.TryAdd(link, validationResult);
        });

        foreach (var link in links)
        {
            var validationResult = validatedLinks[link.Url];

            if (validationResult is not null)
            {
                if (validationResult.IsError)
                    context.Error(validationResult.Id, link, validationResult.Message);
                else
                    context.Warning(validationResult.Id, link, validationResult.Message);
            }
        }
    }

    private static ValidationResult? Validate(string fileName, string link)
    {
        var client = new HttpClient();

        // Some CDNs, such as Akamai, will return 404 unless a User-Agent is specified.
        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.131 Safari/537.36 Edg/92.0.902.67");

        var retryCount = 5;
    Retry:

        if (retryCount-- == 0)
            return new ValidationResult(false, "VR14", "Couldn't validate URL.");

        if (UriHelper.TryGetAbsoluteUri(link, out var url))
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
        else if (UriHelper.TryGetRelativeUri(link, out url))
        {
            var markdownDirectory = Path.GetDirectoryName(fileName);
            var fullPath = Path.Join(markdownDirectory, link);
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
               response.StatusCode == HttpStatusCode.MovedPermanently ||
               response.IsSuccessStatusCode;
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
