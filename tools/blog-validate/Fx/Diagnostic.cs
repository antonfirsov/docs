using Markdig.Syntax;

namespace Microsoft.DotNetBlog;

internal class Diagnostic
{
    public Diagnostic(bool isWarning, string id, MarkdownDocument document, string fileName, SourceSpan span, string message, string suggestion = "")
    {
        IsWarning = isWarning;
        Id = id;
        FileName = fileName;
        Span = span;
        LinePositionSpan = document.GetLinePosition(span);
        Message = message;
        Suggestion = suggestion;
    }

    public bool IsWarning { get; }
    public string Id { get; }
    public string FileName { get; }
    public SourceSpan Span { get; }
    public LinePositionSpan LinePositionSpan { get; }
    public string Message { get; }
    public string Suggestion { get; set; }
}
