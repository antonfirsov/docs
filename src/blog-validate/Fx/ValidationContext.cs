using System;
using System.Collections.Generic;
using System.Linq;

using Markdig.Extensions.Yaml;
using Markdig.Syntax;

using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace BlogValidator
{
    internal sealed class ValidationContext
    {
        private readonly List<Diagnostic> _diagnostics = new List<Diagnostic>();

        public ValidationContext(MarkdownDocument document, string fileName)
        {
            if (document.FirstOrDefault() is YamlFrontMatterBlock frontMatter)
            {
                var yaml = string.Join(Environment.NewLine, frontMatter.Lines);
                var deserializer = new DeserializerBuilder()
                    .WithNamingConvention(UnderscoredNamingConvention.Instance)
                    .Build();

                FrontMatter = deserializer.Deserialize<FrontMatter>(yaml);
            }

            Document = document;
            FileName = fileName;
        }

        public FrontMatter FrontMatter { get; }
        public MarkdownDocument Document { get; }
        public string FileName { get; }
        public IReadOnlyList<Diagnostic> Diagnostics => _diagnostics;

        public void Report(bool isWarning, string id, int line, int column, string message)
        {
            _diagnostics.Add(new Diagnostic(isWarning, id, FileName, line, column, message));
        }

        public void Error(string id, MarkdownObject o, string message)
        {
            Report(isWarning: false, id, o.Line, o.Column, message);
        }

        public void Warning(string id, MarkdownObject o, string message)
        {
            Report(isWarning: true, id, o.Line, o.Column, message);
        }
    }
}
