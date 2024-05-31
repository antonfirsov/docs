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
                content = await System.IO.File.ReadAllTextAsync(CacheFilePath);
            }
            else
            {
                var client = new HttpClient();
                string AuthorsUrl = Environment.GetEnvironmentVariable("AUTHORS_LIST_URL")!;
                if(string.IsNullOrEmpty(AuthorsUrl))
                {
                    Console.WriteLine("AUTHORS_LIST_URL environment variable is not set");
                    return false;
                }
                var response = await client.GetAsync(AuthorsUrl);
                content = await response.Content.ReadAsStringAsync();
            }
            authors = content.Split(',');
        }
        return authors.Contains(author);
    }

    private static string GetCacheFileName()
    {
        // Since it's a cache roaming doesn't seem sensible
        var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        return Path.Join(localAppData, "Microsoft", ".NET Blog", "authors.csv");
    }
}