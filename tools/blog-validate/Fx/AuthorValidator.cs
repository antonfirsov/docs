using static System.Net.WebRequestMethods;

namespace Microsoft.DotNetBlog;

internal sealed class AuthorValidator
{
    private string[]? authors;

    public async Task<bool> IsValidAsync(string author)
    {
        if (authors == null)
        {
            string content;
            var CacheFilePath = GetCacheFileName();
            bool isGitHubActions = Environment.GetEnvironmentVariable("GITHUB_ACTIONS") == "true";
            if (!isGitHubActions && System.IO.File.Exists(CacheFilePath))
            {
                Console.WriteLine($"Reading authors from cache file: {CacheFilePath}");
                content = await System.IO.File.ReadAllTextAsync(CacheFilePath);
            }
            else
            {
                var client = new HttpClient();
                string AuthorsUrl = Environment.GetEnvironmentVariable("AUTHORS_LIST_URL")!;
                if (string.IsNullOrEmpty(AuthorsUrl))
                {
                    Console.WriteLine("AUTHORS_LIST_URL environment variable is not set");
                    return false;
                }
                content = await FetchAuthorsWithRetryAsync(client, AuthorsUrl);
                if (string.IsNullOrWhiteSpace(content))
                {
                    Console.WriteLine("Failed to fetch authors list");
                    return false;
                }
            }

            authors = content.Split(',');
            Console.WriteLine($"Authors list loaded. {authors.Length} authors found.");
        }

        bool isValid = authors.Contains(author);
        return isValid;
    }

    private static string GetCacheFileName()
    {
        var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        return Path.Join(localAppData, "Microsoft", ".NET Blog", "authors.csv");
    }

    private async Task<string> FetchAuthorsWithRetryAsync(HttpClient client, string url, int maxRetries = 3)
    {
        int attempt = 0;
        while (attempt < maxRetries)
        {
            try
            {
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                attempt++;
                Console.WriteLine($"Author Validation: Attempt {attempt} failed: {ex.Message}");
                if (attempt >= maxRetries)
                {
                    throw;
                }
                await Task.Delay(1000); // Wait for 1 second before retrying
            }
        }
        Console.WriteLine($"Failed to fetch authors after {maxRetries} attempts.");
        return string.Empty;
    }
}
