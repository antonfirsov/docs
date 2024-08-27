using System.Text.RegularExpressions;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace Microsoft.DotNetBlog;

internal sealed partial class VR31_MustFollowOfficialBranding : ValidationRule
{
    public override void Validate(ValidationContext context)
    {
        foreach (var text in context.Document.Descendants<LeafInline>())
        {
            var textContent = text?.ToString();
            if (textContent == null) continue;

            if (MauiPattern().IsMatch(textContent))
            {
                var correctedText = MauiPattern().Replace(textContent, ".NET MAUI");
                context.Error(this.GetType().Name, text!, "The term 'MAUI' must be written in all caps and prefaced by '.NET'.", correctedText);
            }
            if (AspirePattern().IsMatch(textContent))
            {
                var correctedText = AspirePattern().Replace(textContent, ".NET Aspire");
                context.Error(this.GetType().Name, text!, "The term 'Aspire' must be prefaced by '.NET'.", correctedText);
            }
            if (IncorrectDotNetPattern().IsMatch(textContent))
            {
                var correctedText = IncorrectDotNetPattern().Replace(textContent, ".NET");
                context.Error(this.GetType().Name, text!, "The term '.NET' must be written in all caps.", correctedText);
            }
        }
    }

    [GeneratedRegex(@"(?<!\.NET\s)MAUI", RegexOptions.IgnoreCase)]
    public static partial Regex MauiPattern();

    [GeneratedRegex(@"(?<!\.NET\s)Aspire", RegexOptions.IgnoreCase)]
    public static partial Regex AspirePattern();

    [GeneratedRegex(@"(?<![a-zA-Z0-9])\.Net\b", RegexOptions.IgnoreCase)]
    public static partial Regex IncorrectDotNetPattern();
}
