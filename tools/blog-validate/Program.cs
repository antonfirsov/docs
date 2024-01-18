using LibGit2Sharp;
using Microsoft.DotNetBlog.Fx;
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
        var errors = diagnostics.Where(d => !d.IsWarning).ToArray();
        var warnings = diagnostics.Where(d => d.IsWarning).ToArray();

        var isInsideGitHubAction = Environment.GetEnvironmentVariable("GITHUB_ACTIONS") == "true";

        string token = Environment.GetEnvironmentVariable("GITHUB_TOKEN") ?? string.Empty;
        string commit = Environment.GetEnvironmentVariable("COMMIT_ID") ?? Environment.GetEnvironmentVariable("GITHUB_SHA") ?? string.Empty;
        string fullRepo = Environment.GetEnvironmentVariable("GITHUB_REPOSITORY") ?? string.Empty;
        string githubRef = Environment.GetEnvironmentVariable("GITHUB_REF") ?? string.Empty;

        var gitHubService = isInsideGitHubAction ? new GitHubService(token, fullRepo, githubRef) : null;
        string validationSummary = "| Issue | File | Line | Message |\n| --- | --- | --- | --- |\n";

        Console.ForegroundColor = ConsoleColor.Red;

        foreach (var e in errors)
        {
            var path = Path.GetRelativePath(rootDirectory, e.FileName);

            if (gitHubService is not null)
            {
                var line = e.LinePositionSpan.Start.Line + 1;
                var col = e.LinePositionSpan.Start.Column + 1;
                Console.WriteLine($"::Error file={path},line={line},col={col}::{e.Id}: {e.Message}");
                validationSummary += $"| ❌ {e.Id} | {Path.GetFileName(path)} | {line} | {e.Message} |\n";
                if (!string.IsNullOrEmpty(e.Suggestion))
                {
                    await gitHubService.TryAddSuggestion(e.Suggestion, e.Message, path, line, commit);
                }
            }
            else
            {
                Console.WriteLine($"{path}({e.LinePositionSpan}): Error: {e.Id}: {e.Message}");
            }
        }

        if (gitHubService is not null && errors.Length > 0)
        {
            validationSummary = $"## {errors.Length} error(s)\n\n All errors must be fixed before this pull request can be merged. \n{validationSummary}";
            await gitHubService.AddComment(validationSummary);
        }

        validationSummary = "| Issue | File | Line | Message |\n| --- | --- | --- | --- |\n";
        Console.ForegroundColor = ConsoleColor.Yellow;
        foreach (var w in warnings)
        {
            var path = Path.GetRelativePath(rootDirectory, w.FileName);

            if (gitHubService is not null)
            {
                var line = w.LinePositionSpan.Start.Line + 1;
                var col = w.LinePositionSpan.Start.Column + 1;
                Console.WriteLine($"::Warning file={path},line={line},col={col}::{w.Id}: {w.Message}");
                validationSummary += $"| 👀 {w.Id} | {Path.GetFileName(path)} | {line} | {w.Message} |\n";
            }
            else
            {
                Console.WriteLine($"{path}({w.LinePositionSpan}): Warning: {w.Id}: {w.Message}");
            }
        }

        if (gitHubService is not null && warnings.Length > 0)
        {
            validationSummary = $"## {warnings.Length} warning(s)\n\n Warnings should be checked and corrected if necessary but will not block merging. \n{validationSummary}";
            await gitHubService.AddComment(validationSummary);
        }

        return errors.Length == 0;
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