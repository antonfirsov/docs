using Markdig.Syntax;
using System.Text.RegularExpressions;

namespace Microsoft.DotNetBlog;

internal sealed partial class VR29_YouTubeShouldBeFullWidth : ValidationRule
{
    private static readonly Regex widthRegex = widthRegexGen();
    private static readonly Regex heightRegex = heightRegexGen();

    public override void Validate(ValidationContext context)
    {
        // Filter this to HTML blocks that are iframe elements that link to youtube
        var links = context.Document.Descendants<HtmlBlock>()
            .Where(block =>
            {
                if (block.Lines.Lines.Length == 0)
                    return false;

                var line = block.Lines.Lines[0].Slice.ToString();
                return line.StartsWith("<iframe") && line.Contains("youtube.com");
            });

        foreach (var link in links)
        {
            var block = link as HtmlBlock;
            var text = block.Lines.Lines[0].Slice.ToString();

            // Check if width is not set to 800 and height is not set to 450. If they are, warn and recommend those
            if (!text.Contains("width=\"800\"") || !text.Contains("height=\"450\""))
            {
                // Create a string suggestion that will replace the entire line with the correct width and height
                var suggestion = widthRegex.Replace(text, "width=\"800\"");
                suggestion = heightRegex.Replace(suggestion, "height=\"450\"");

                context.Warning(this.GetType().Name, link, "YouTube videos should be full width (800x450)", suggestion);
            }
        }
    }

    [GeneratedRegex("width=\"\\d+\"", RegexOptions.Compiled)]
    private static partial Regex widthRegexGen();
    [GeneratedRegex("height=\"\\d+\"", RegexOptions.Compiled)]
    private static partial Regex heightRegexGen();
}
