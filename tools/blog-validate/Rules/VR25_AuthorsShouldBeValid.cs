using Markdig.Syntax;
using System.Net.Http.Json;

namespace Microsoft.DotNetBlog;

internal sealed class VR25_AuthorsShouldBeValid : ValidationRule
{
    const string url = "https://prod-137.westus.logic.azure.com:443/workflows/0945ba8cd3d742e58aed90e55f0b3473/triggers/manual/paths/invoke?api-version=2016-10-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=KHolQfWeIh04JdvuTQhzqsgliow6NqLqoCTgQ-dkWsc";
    
    public override async Task ValidateAsync(ValidationContext context)
    {
        var client = new HttpClient();

        await ValidateAuthor(context.FrontMatter?.Author1, context.Document.GetFrontMatterDiagnosticSpan(BlogFrontMatterFields.Author1));
        await ValidateAuthor(context.FrontMatter?.Author2, context.Document.GetFrontMatterDiagnosticSpan(BlogFrontMatterFields.Author2));
        await ValidateAuthor(context.FrontMatter?.Author3, context.Document.GetFrontMatterDiagnosticSpan(BlogFrontMatterFields.Author3));


        async Task ValidateAuthor(string? author, SourceSpan span)
        {
            if (string.IsNullOrWhiteSpace(author))
                return;
            Console.WriteLine($"Validating author {author}...");

            try
            {
                var response = await client.PostAsJsonAsync(url, new { Author = author });
                if (response.IsSuccessStatusCode)
                {
                    var count = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Validating author count: {count}...");
                    if (count == "0")
                        context.Warning("VR25", span, $"{author} may not be valid, please login to WordPress and validate your username in your profile.");
                }
                else
                {
                    context.Warning("VR25", span, $"Unable to validate author username");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("exception");
                Console.WriteLine(ex);
                context.Warning("VR25", span, $"Unable to validate author username. Visit https://devblogs.microsoft.com/dotnet/wp-admin/profile.php and check the user name field.");
            }

        }
    }
}
