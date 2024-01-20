using Markdig.Extensions.Yaml;
using Markdig.Helpers;
using Markdig.Syntax;

namespace Microsoft.DotNetBlog;

public static class MarkdownExtensions
{
    public static LinePosition GetLinePosition(this MarkdownDocument document, int index)
    {
        if (document?.LineStartIndexes == null)
            throw new ArgumentNullException(nameof(document));

        for (var i = document.LineStartIndexes.Count - 1; i >= 0; i--)
        {
            var lineStart = document.LineStartIndexes[i];
            if (lineStart <= index)
            {
                var line = i;
                var column = index - lineStart;
                return new LinePosition(line, column);
            }
        }

        throw new ArgumentOutOfRangeException(nameof(index), index, null);
    }

    public static LinePositionSpan GetLinePosition(this MarkdownDocument document, SourceSpan span)
    {
        ArgumentNullException.ThrowIfNull(document);

        var start = document.GetLinePosition(span.Start);
        var end = document.GetLinePosition(span.End + 1);
        return new LinePositionSpan(start, end);
    }

    public static SourceSpan GetFrontMatterDiagnosticSpan(this MarkdownDocument document, string text)
    {
        ArgumentNullException.ThrowIfNull(document);

        if (document.FirstOrDefault() is not YamlFrontMatterBlock block)
            throw new ArgumentException("The document doesn't have any front matter", nameof(document));

        foreach (StringLine line in block.Lines)
        {
            var lineText = line.ToString();
            if (lineText.Contains(text, StringComparison.OrdinalIgnoreCase))
                return new SourceSpan(line.Position, line.Position + lineText.Length);
        }

        return block.Span;
    }
}
