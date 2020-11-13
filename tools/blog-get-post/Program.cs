using System;
using System.IO;
using System.Linq;
using System.Text.Json;

using LibGit2Sharp;

using Mono.Options;

namespace Microsoft.DotNetBlog
{
    internal static class Program
    {
        private static int Main(string[] args)
        {
            var exeName = Path.GetFileNameWithoutExtension(Environment.GetCommandLineArgs()[0]);
            var help = false;
            var beforeText = "";
            var afterText = "";

            var eventPath = Environment.GetEnvironmentVariable("GITHUB_EVENT_PATH");
            if (!string.IsNullOrEmpty(eventPath))
            {
                var eventJson = File.ReadAllText(eventPath);
                var eventPayload = JsonSerializer.Deserialize<PushPayload>(eventJson);
                beforeText = eventPayload.before;
                afterText = eventPayload.after;
            }

            var options = new OptionSet
            {
                $"usage: {exeName} <directory> [OPTIONS]+",
                { "before=", "The {before SHA}", v => beforeText = v },
                { "after=", "The {after sha}", v => afterText = v },
                { "h|?|help", null, v => help = true, true },
                new ResponseFileSource()
            };

            try
            {
                var unprocessed = options.Parse(args).ToArray();

                if (help)
                {
                    options.WriteOptionDescriptions(Console.Error);
                    return 0;
                }

                if (unprocessed.Any())
                {
                    foreach (var option in unprocessed)
                        Console.Error.WriteLine($"error: unrecognized argument {option}");
                    return 1;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());
                return 1;
            }

            var repositoryPath = Repository.Discover(".");
            if (repositoryPath == null)
            {
                Console.Error.Write("error: can't find Git repository");
                return 1;
            }

            var repository = new Repository(repositoryPath);

            var beforeCommit = ParseRev(repository, beforeText);
            if (beforeCommit == null)
            {
                Console.Error.WriteLine($"error: reference '{beforeText}' isn't valid");
                return 1;
            }

            var afterCommit = ParseRev(repository, afterText);
            if (afterCommit == null)
            {
                Console.Error.WriteLine($"error: reference '{afterText}' isn't valid");
                return 1;
            }

            var changes = repository.Diff.Compare<TreeChanges>(beforeCommit.Tree, afterCommit.Tree);
            var affectedFiles = changes.Select(c => Path.GetFullPath(Path.Combine(repository.Info.WorkingDirectory, c.Path))).ToArray();

            try
            {
                return Run(affectedFiles);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return 1;
            }
        }

        private static Commit ParseRev(Repository repository, string text)
        {
            try
            {
                repository.RevParse(text, out var _, out var obj);
                return obj as Commit;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static int Run(string[] affectedFiles)
        {
            var markdownFiles = affectedFiles.Where(p => string.Equals(Path.GetExtension(p), ".md", StringComparison.OrdinalIgnoreCase))
                                             .ToArray();

            if (markdownFiles.Length == 0)
            {
                Console.Error.WriteLine("warning: no Markdown files found");
                return 0;
            }
            else if (markdownFiles.Length > 1)
            {
                Console.Error.WriteLine("warning: too many markdown files in commit");
                return 0;
            }

            var file = markdownFiles[0];
            if (!TryGetTitle(file, out var title))
            {
                Console.Error.WriteLine("warning: can't find title");
                return 0;
            }

            Console.WriteLine($"::set-output name=title::{title}");
            return 0;
        }

        private static bool TryGetTitle(string fileName, out object title)
        {
            var markdown = File.ReadAllText(fileName);
            var document = BlogMarkdown.Parse(markdown);

            if (document.TryGetFrontMatter(out var frontMatter))
            {
                if (!string.IsNullOrEmpty(frontMatter?.PostTitle))
                {
                    title = frontMatter.PostTitle;
                    return true;
                }
            }

            title = null;
            return false;
        }
    }

    internal sealed class PushPayload
    {
        public string before { get; set; }
        public string after { get; set; }
    }
}
