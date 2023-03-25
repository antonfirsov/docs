using Markdig.Syntax;

namespace Microsoft.DotNetBlog;

internal sealed class VR25_AuthorsShouldBeValid : ValidationRule
{
    public override async Task ValidateAsync(ValidationContext context)
    {
        await ValidateAuthor(context.FrontMatter?.Author1, context.Document.GetFrontMatterDiagnosticSpan(BlogFrontMatterFields.Author1));
        await ValidateAuthor(context.FrontMatter?.Author2, context.Document.GetFrontMatterDiagnosticSpan(BlogFrontMatterFields.Author2));
        await ValidateAuthor(context.FrontMatter?.Author3, context.Document.GetFrontMatterDiagnosticSpan(BlogFrontMatterFields.Author3));

        async Task ValidateAuthor(string? author, SourceSpan span)
        {
            if (string.IsNullOrWhiteSpace(author))
                return;

            try
            {
                var isValid = await context.AuthorValidator.IsValidAsync(author); 
                if (!isValid)
                    context.Warning("VR25", span, $"{author} may not be valid, please login to WordPress and validate your username in your profile.");
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
