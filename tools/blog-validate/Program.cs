using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using LibGit2Sharp;

using Mono.Options;

namespace BlogValidator
{
    // Maira suggested these rules:
    //
    // TODO: Avoid non-English words(like i.e., e.g., etc.)
    //
    // Meenal suggested these rules:
    //
    // TODO: Minimum length for a post should be 300 words

    internal static class Program
    {
        static int Main(string[] args)
        {
            var exeName = Path.GetFileNameWithoutExtension(Environment.GetCommandLineArgs()[0]);
            var help = false;
            var baseReferenceText = "";
            var referenceText = "";
            var inputPath = "";

            var options = new OptionSet
            {
                $"usage: {exeName} <directory> [OPTIONS]+",
                { "base-ref=", "The branch the changes are merged into", v => baseReferenceText = v },
                { "ref=", "The ref of the PR that is being merged", v => referenceText = v },
                { "h|?|help", null, v => help = true, true },
                new ResponseFileSource()
            };

            try
            {
                var parameters = options.Parse(args).ToArray();

                if (help)
                {
                    options.WriteOptionDescriptions(Console.Error);
                    return 0;
                }

                if (parameters.Length == 1)
                {
                    inputPath = parameters[0];
                }
                else
                {
                    Console.Error.WriteLine("error: must specify an input path");
                    return 1;
                }

                var unprocessed = parameters.Skip(1);

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

            var directory = Path.GetFullPath(inputPath);
            if (!Directory.Exists(directory))
            {
                Console.Error.WriteLine($"error: directory '{directory}' does not exist");
                return 1;
            }

            var affectedFiles = (string[])null;

            if (!string.IsNullOrEmpty(baseReferenceText) || string.IsNullOrEmpty(referenceText))
            {
                if (string.IsNullOrEmpty(baseReferenceText))
                {
                    Console.Error.WriteLine($"error: must specify --base-ref when specifying --ref");
                    return 1;
                }

                if (string.IsNullOrEmpty(referenceText))
                {
                    Console.Error.WriteLine($"error: must specify --ref when specifying --base-ref");
                    return 1;
                }

                var repositoryPath = Repository.Discover(directory);
                if (repositoryPath == null)
                {
                    Console.Error.WriteLine($"error: '{directory}' is not inside a Git repository");
                    return 1;
                }

                var repository = new Repository(repositoryPath);

                repository.RevParse(referenceText, out var reference, out var gitReferenceObject);
                repository.RevParse(baseReferenceText, out var baseReference, out var gitBaseReferenceObject);

                var referenceCommit = gitReferenceObject as Commit;
                var baseReferenceCommit = gitBaseReferenceObject as Commit;

                if (referenceCommit == null)
                {
                    Console.Error.WriteLine($"error: reference '{referenceText}' isn't valid");
                    return 1;
                }

                if (baseReferenceCommit == null)
                {
                    Console.Error.WriteLine($"error: reference '{baseReferenceText}' isn't valid");
                    return 1;
                }

                var treeChanges = repository.Diff.Compare<TreeChanges>(baseReferenceCommit.Tree, referenceCommit.Tree);
                affectedFiles = treeChanges.Select(c => Path.GetFullPath(Path.Combine(repository.Info.WorkingDirectory, c.Path))).ToArray();
            }

            try
            {
                return Run(directory, affectedFiles) ? 0 : 1;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return 1;
            }
        }

        private static bool Run(string directory, string[] affectedFiles)
        {
            var files = FindMarkdownFiles(directory, affectedFiles);

            var diagnostics = Validate(files);

            var isInsideGitHubAction = Environment.GetEnvironmentVariable("GITHUB_ACTIONS") == "true";

            foreach (var d in diagnostics)
            {
                var path = Path.GetRelativePath(directory, d.FileName);
                var severity = d.IsWarning ? "warning" : "error";

                if (isInsideGitHubAction)
                {
                    var line = d.LinePositionSpan.Start.Line + 1;
                    var col = d.LinePositionSpan.Start.Column + 1;
                    Console.WriteLine($"::{severity} file={path},line={line},col={col}::{d.Id}: {d.Message}");
                }
                else
                {
                    if (d.IsWarning)
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    else
                        Console.ForegroundColor = ConsoleColor.Red;

                    Console.WriteLine($"{path}({d.LinePositionSpan}): {severity}: {d.Id}: {d.Message}");

                    Console.ResetColor();
                }
            }

            var hasErrors = diagnostics.Any(d => !d.IsWarning);
            return !hasErrors;
        }

        private static IEnumerable<string> FindMarkdownFiles(string directory, string[] affectedFiles)
        {
            if (affectedFiles != null)
                return affectedFiles.Where(p => string.Equals(Path.GetExtension(p), ".md", StringComparison.OrdinalIgnoreCase));

            return Directory.GetFiles(directory, "*.md", SearchOption.AllDirectories)
                            .Where(p => IsIncluded(directory, p));
        }

        private static bool IsIncluded(string repoPath, string path)
        {
            var relativePath = Path.GetRelativePath(repoPath, path);
            var segments = GetSegments(relativePath);

            if (segments.Length < 2 || segments[1].Length < 2)
                return false;

            if (!int.TryParse(segments[0], out var year))
                return false;

            if (!int.TryParse(segments[1].Substring(0, 2), out var month))
                return false;

            if (year < 2020 || year == 2020 && month < 9)
                return false;

            if (year == 2020 && month == 9)
            {
                if (segments.Length >= 3)
                {
                    var grandfathered = new[]
                    {
                        "announcing-entity-framework-5.0-rc1",
                        "Arm64PerfInNet5",
                        "debug-dotnet-in-wsl",
                        "dotnet5rc1",
                        "netstandard-update",
                        "mlnet-september-updates",
                    };

                    var isGrandfathered = grandfathered.Contains(segments[2], StringComparer.Ordinal);
                    if (isGrandfathered)
                        return false;
                }
            }

            return true;
        }

        private static string[] GetSegments(string path)
        {
            var list = new List<string>();
            while (path.Length > 0)
            {
                var last = Path.GetFileName(path);
                list.Add(last);
                path = Path.GetDirectoryName(path);
            }

            list.Reverse();

            return list.ToArray();
        }

        private static IReadOnlyList<Diagnostic> Validate(IEnumerable<string> fileNames)
        {
            var validator = new Validator();
            var result = new List<Diagnostic>();

            foreach (var fileName in fileNames)
            {
                var diagnostics = validator.Validate(fileName);
                result.AddRange(diagnostics);
            }

            return result;
        }
    }
}
