using Markdig;

namespace md2html;

internal static class Program
{
    private static void Main(string[] args)
    {
        string? fileName = null;

        if (args.Length == 1)
        {
            fileName = args[0];
        }
        else if (args.Length == 0)
        {
            var files = Directory.GetFiles(Environment.CurrentDirectory, "*.md");
            if (files.Length == 0)
            {
                Console.Error.WriteLine("error: no markdown files found");
                return;
            }

            if (files.Length > 1)
            {
                Console.Error.WriteLine("error: too many markdown files found");
                return;
            }

            fileName = files.Single();
        }
        else
        {
            Console.Error.WriteLine("error: too many arguments");
            return;
        }

        fileName = Path.GetFullPath(fileName);

        if (!File.Exists(fileName))
        {
            Console.Error.WriteLine($"error: file '{fileName}' not found found");
            return;
        }

        Console.WriteLine($"Reading '{fileName}'...");
        var markdown = File.ReadAllText(fileName);
        var pipeline = new MarkdownPipelineBuilder()
            .UsePipeTables()
            .UseYamlFrontMatter()
            .Build();

        var html = Markdown.ToHtml(markdown, pipeline);
        Console.Write(html);
    }
}
