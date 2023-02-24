using LibGit2Sharp;

using Mono.Options;

namespace Microsoft.DotNetBlog;

// Maira suggested these rules:
//
// TODO: Avoid non-English words(like i.e., e.g., etc.)
//
// Meenal suggested these rules:
//
// TODO: Minimum length for a post should be 300 words

internal static class Program
{
    private static async Task<int> Main(string[] args)
    {
        var exeName = Path.GetFileNameWithoutExtension(Environment.GetCommandLineArgs()[0]);
        var help = false;
        var baseReferenceText = "main";
        var referenceText = "";
        var categoriesPath = "";
        var all = false;
        var inputPath = "";

        var options = new OptionSet
            {
                $"usage: {exeName} <directory> [OPTIONS]+",
                { "base-ref=", "The {branch} the changes are merged into", v => baseReferenceText = v },
                { "ref=", "The {sha} of the PR that is being merged", v => referenceText = v },
                { "categories=", "The {path} to a file with allowed categories", v => categoriesPath = v },
                { "all", "Validates all files", v => all = true },
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

            if (parameters.Length >= 1)
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

        var categories = Array.Empty<string>();

        if (string.IsNullOrEmpty(categoriesPath))
        {
            var defaultCategoriesPath = Path.Combine(inputPath, "categories.txt");
            if (File.Exists(defaultCategoriesPath))
                categoriesPath = defaultCategoriesPath;
        }

        if (!string.IsNullOrEmpty(categoriesPath))
        {
            categoriesPath = Path.GetFullPath(categoriesPath);
            if (!File.Exists(categoriesPath))
            {
                Console.Error.WriteLine($"error: file '{categoriesPath}' does not exist");
                return 1;
            }

            categories = File.ReadLines(categoriesPath)
                             .Select(l => l.Trim())
                             .Where(l => !string.IsNullOrEmpty(l))
                             .ToArray();
        }

        var affectedFiles = (string[]?)null;

        if (!all)
        {
            var repositoryPath = Repository.Discover(directory);
            if (repositoryPath != null)
            {
                using var repository = new Repository(repositoryPath);

                var baseReferenceCommit = ParseRev(repository, baseReferenceText) ?? ParseRev(repository, "origin/" + baseReferenceText);

                if (baseReferenceCommit == null)
                {
                    Console.Error.WriteLine($"error: reference '{baseReferenceText}' isn't valid");
                    return 1;
                }

                if (string.IsNullOrEmpty(referenceText))
                {
                    affectedFiles = BlogRepo.GetAffectedPosts(repository, baseReferenceCommit);
                }
                else
                {
                    var referenceCommit = ParseRev(repository, referenceText);

                    if (referenceCommit == null)
                    {
                        Console.Error.WriteLine($"error: reference '{referenceText}' isn't valid");
                        return 1;
                    }

                    affectedFiles = BlogRepo.GetAffectedPosts(repository, baseReferenceCommit, referenceCommit);
                }
            }
        }

        try
        {
            return await RunAsync(directory, affectedFiles, categories) ? 0 : 1;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex);
            return 1;
        }
    }

    private static Commit? ParseRev(Repository repository, string text)
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

    private static async Task<bool> RunAsync(string rootDirectory, string[]? affectedFiles, string[] categories)
    {
        var files = FindMarkdownFiles(rootDirectory, affectedFiles);

        var diagnostics = await ValidateAsync(rootDirectory, files, categories);

        var isInsideGitHubAction = Environment.GetEnvironmentVariable("GITHUB_ACTIONS") == "true";

        foreach (var d in diagnostics)
        {
            var path = Path.GetRelativePath(rootDirectory, d.FileName);
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

    private static IEnumerable<string> FindMarkdownFiles(string directory, string[]? affectedFiles)
    {
        if (affectedFiles != null)
            return affectedFiles;

        return BlogRepo.GetPosts(directory);
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

        return true;
    }

    private static string[] GetSegments(string? path)
    {
        var list = new List<string>();
        while (path?.Length > 0)
        {
            var last = Path.GetFileName(path);
            list.Add(last);
            path = Path.GetDirectoryName(path);
        }

        list.Reverse();

        return list.ToArray();
    }

    private static async Task<IReadOnlyList<Diagnostic>> ValidateAsync(string rootDirectory, IEnumerable<string> fileNames, IEnumerable<string> categories)
    {
        var validator = new Validator();
        var result = new List<Diagnostic>();

        foreach (var fileName in fileNames)
        {
            var diagnostics = await validator.ValidateAsync(rootDirectory, fileName, categories);
            result.AddRange(diagnostics);
        }

        return result;
    }
}
