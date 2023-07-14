using System.Diagnostics.CodeAnalysis;

using Markdig;
using Markdig.Extensions.Yaml;
using Markdig.Parsers;
using Markdig.Syntax;

using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Microsoft.DotNetBlog;

public static class BlogMarkdown
{
    private static readonly MarkdownPipeline _pipeline = new MarkdownPipelineBuilder()
        .UsePipeTables()
        .UseYamlFrontMatter()
        .UsePreciseSourceLocation()
        .Build();

    public static MarkdownDocument Parse(string markdown)
    {
        return MarkdownParser.Parse(markdown, _pipeline);
    }

    public static bool TryGetFrontMatter(this MarkdownDocument document, [MaybeNullWhen(false)] out BlogFrontMatter frontMatter)
    {
        if (document.FirstOrDefault() is YamlFrontMatterBlock frontMatterBlock)
        {
            var yaml = string.Join(Environment.NewLine, frontMatterBlock.Lines);
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(UnderscoredNamingConvention.Instance)
                .IgnoreUnmatchedProperties()
                .Build();

            try
            {
                frontMatter = deserializer.Deserialize<BlogFrontMatter>(yaml);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        frontMatter = null;
        return false;
    }
}
