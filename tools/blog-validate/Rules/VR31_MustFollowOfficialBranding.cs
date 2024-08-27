using System.Text.RegularExpressions;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace Microsoft.DotNetBlog;

internal sealed partial class VR31_MustFollowOfficialBranding : ValidationRule
{
    public override void Validate(ValidationContext context)
    {
        var markdownText = context.Markdown;
        if (string.IsNullOrEmpty(markdownText)) return;

        foreach (var paragraph in context.Document.Descendants<ParagraphBlock>())
        {
            var paragraphText = markdownText.Substring(paragraph.Span.Start, paragraph.Span.Length);
            ValidatePattern(context, paragraph, paragraphText, MauiPattern(), ".NET MAUI", "The term 'MAUI' must be written in all caps and prefaced by '.NET'.");
            ValidatePattern(context, paragraph, paragraphText, AspirePattern(), ".NET Aspire", "The term 'Aspire' must be prefaced by '.NET'.");
            ValidatePattern(context, paragraph, paragraphText, IncorrectDotNetPattern(), ".NET", "The term '.NET' must be written in all caps.");
        }
    }

    private void ValidatePattern(ValidationContext context, MarkdownObject paragraph, string paragraphText, Regex pattern, string replacement, string message)
    {
        var matches = pattern.Matches(paragraphText);
        if (matches.Count > 0)
        {
            var correctedText = pattern.Replace(paragraphText, replacement);
            context.Error(this.GetType().Name, paragraph, message, correctedText);
        }
    }

    [GeneratedRegex(@"(?<!\.NET\s)(?<![a-zA-Z0-9/])Aspire", RegexOptions.IgnoreCase)]
    public static partial Regex AspirePattern();

    [GeneratedRegex(@"(?<![a-zA-Z0-9/])(?<!\.NET\s)MAUI", RegexOptions.IgnoreCase)]
    public static partial Regex MauiPattern();

    [GeneratedRegex(@"(?<![a-zA-Z0-9])\.Net\b")]
    public static partial Regex IncorrectDotNetPattern();
}