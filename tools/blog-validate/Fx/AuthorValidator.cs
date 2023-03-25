using System.Net.Http.Json;
using System.Text.Json;

namespace Microsoft.DotNetBlog;

internal sealed class AuthorValidator : IDisposable
{
    private const string Url = "https://prod-137.westus.logic.azure.com:443/workflows/0945ba8cd3d742e58aed90e55f0b3473/triggers/manual/paths/invoke?api-version=2016-10-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=KHolQfWeIh04JdvuTQhzqsgliow6NqLqoCTgQ-dkWsc";

    private readonly HttpClient _client;
    private readonly Dictionary<string, bool> _authorCache; 

    public AuthorValidator()
    {
        _client = new HttpClient();
        _authorCache = LoadCache();
    }

    public void Dispose()
    {
        _client.Dispose();
        StoreCache();
    }

    public async Task<bool> IsValidAsync(string author)
    {
        if (!_authorCache.TryGetValue(author, out var isValid))
        {
            isValid = await IsValidUncachedAsync(author);
            _authorCache.Add(author, isValid);
        }
        
        return isValid;
    }

    private async Task<bool> IsValidUncachedAsync(string author)
    {
        Console.WriteLine($"Validating author '{author}'...");
        var response = await _client.PostAsJsonAsync(Url, new {Author = author});
        response.EnsureSuccessStatusCode();
        var count = await response.Content.ReadAsStringAsync();
        return count != "0";
    }

    private static Dictionary<string, bool> LoadCache()
    {
        var fileName = GetCacheFileName();
        var cacheInfo = new FileInfo(fileName);
        var result = new Dictionary<string, bool>();
        if (cacheInfo.Exists && (DateTime.Now - cacheInfo.LastWriteTime).TotalDays < 1)
        {
            var json = File.ReadAllText(fileName);
            var entries = JsonSerializer.Deserialize<CacheEntry[]>(json)!;
            foreach (var entry in entries)
                result[entry.AuthorName] = entry.IsValid;
        }

        return result;
    }

    private void StoreCache()
    {
        var entries = _authorCache.Select(kv => new CacheEntry(kv.Key, kv.Value));
        var json = JsonSerializer.Serialize(entries);
        
        var cacheFileName = GetCacheFileName();
        var cacheDirectory = Path.GetDirectoryName(cacheFileName)!;
        Directory.CreateDirectory(cacheDirectory);
        File.WriteAllText(cacheFileName, json);
    }

    private static string GetCacheFileName()
    {
        // Since it's a cache roaming doesn't seem sensible
        var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        return Path.Join(localAppData, "Microsoft", ".NET Blog", "authors.json");
    }

    private sealed record CacheEntry(string AuthorName, bool IsValid);
}