namespace Microsoft.DotNetBlog;

internal sealed class AuthorValidator
{
    private const string AuthorsUrl = "https://dotnetdevblogids.blob.core.windows.net/blogauthors/blog-authors.csv?sp=r&st=2024-01-18T17:57:07Z&se=2025-02-01T01:57:07Z&spr=https&sv=2022-11-02&sr=b&sig=NEyCEfytvioZghOgu5YHfyP7ePRJLS3GIG8Fe30j%2F8E%3D";
    private string[]? authors;

    public async Task<bool> IsValidAsync(string author)
    {
        if (authors == null)
        {
            string content;
            var CacheFilePath = GetCacheFileName();
            bool isGitHubActions = Environment.GetEnvironmentVariable("GITHUB_ACTIONS") == "true";
            if (!isGitHubActions && File.Exists(CacheFilePath))
            {
                content = await File.ReadAllTextAsync(CacheFilePath);
            }
            else
            {
                var client = new HttpClient();
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