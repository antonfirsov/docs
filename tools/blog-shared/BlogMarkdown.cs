using System;
using System.Linq;

using Markdig;
using Markdig.Extensions.Yaml;
using Markdig.Parsers;
using Markdig.Syntax;

using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Microsoft.DotNetBlog
{
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

        public static bool TryGetFrontMatter(this MarkdownDocument document, out BlogFrontMatter frontMatter)
        {
            if (document.FirstOrDefault() is YamlFrontMatterBlock frontMatterBlock)
            {
                var yaml = string.Join(Environment.NewLine, frontMatterBlock.Lines);
                var deserializer = new DeserializerBuilder()
                    .WithNamingConvention(UnderscoredNamingConvention.Instance)
                    .IgnoreUnmatchedProperties()
                    .Build();

                frontMatter = deserializer.Deserialize<BlogFrontMatter>(yaml);
                return true;
            }

            frontMatter = null;
            return false;
        }
    }
}
