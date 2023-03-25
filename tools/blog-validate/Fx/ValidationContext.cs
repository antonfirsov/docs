using Markdig.Syntax;

namespace Microsoft.DotNetBlog;

internal sealed class ValidationContext : IDisposable
{
    private readonly List<Diagnostic> _diagnostics = new List<Diagnostic>();

    public ValidationContext(string rootDirectory, MarkdownDocument document, string fileName, IEnumerable<string> categories)
    {
        RootDirectory = rootDirectory;
        Document = document;
        FileName = fileName;
        Categories = new SortedSet<string>(categories, StringComparer.OrdinalIgnoreCase);

        if (document.TryGetFrontMatter(out var frontMatter))
            FrontMatter = frontMatter;

        AuthorValidator = new AuthorValidator();
    }

    public void Dispose()
    {
        AuthorValidator.Dispose();
    }

    public string RootDirectory { get; }
    public BlogFrontMatter? FrontMatter { get; }
    public MarkdownDocument Document { get; }
    public string FileName { get; }
    public SortedSet<string> Categories { get; }
    public AuthorValidator AuthorValidator { get; }

    public IReadOnlyList<Diagnostic> Diagnostics => _diagnostics;

    private void Report(bool isWarning, string id, SourceSpan span, string message)
    {
        _diagnostics.Add(new Diagnostic(isWarning, id, Document, FileName, span, message));
    }

    public void Error(string id, MarkdownObject o, string message)
    {
        Report(isWarning: false, id, o.Span, message);
    }

    public void Error(string id, SourceSpan span, string message)
    {
        Report(isWarning: false, id, span, message);
    }

    public void Warning(string id, MarkdownObject o, string message)
    {
        Report(isWarning: true, id, o.Span, message);
    }

    public void Warning(string id, SourceSpan span, string message)
    {
        Report(isWarning: true, id, span, message);
    }
}
