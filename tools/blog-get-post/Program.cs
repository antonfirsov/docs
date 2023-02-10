using System.Text.Json;

using LibGit2Sharp;

using Mono.Options;

namespace Microsoft.DotNetBlog;

internal static class Program
{
    private static int Main(string[] args)
    {
        var exeName = Path.GetFileNameWithoutExtension(Environment.GetCommandLineArgs()[0]);
        var help = false;
        var beforeText = "";
        var afterText = "";
        var pullRequestNumber = -1;
        var pullRequestIsClosed = false;
        var pullRequestIsMerged = false;

        var eventPath = Environment.GetEnvironmentVariable("GITHUB_EVENT_PATH");
        if (!string.IsNullOrEmpty(eventPath))
        {
            var eventJson = File.ReadAllText(eventPath);

            Console.WriteLine("::group::GitHub Event");
            Console.WriteLine(eventJson);
            Console.WriteLine("::endgroup::");

            var eventPayload = JsonSerializer.Deserialize<GitHubEventPayload>(eventJson)!;

            pullRequestNumber = eventPayload.number;

            if (eventPayload.pull_request is not null)
            {
                beforeText = eventPayload.pull_request.@base?.sha;
                afterText = eventPayload.pull_request.head?.sha;
                pullRequestIsClosed = eventPayload.pull_request.state == "closed";
                pullRequestIsMerged = eventPayload.pull_request.merged;

                if (pullRequestIsMerged)
                {
                    // Since this repo uses squash/merge we can't use base/head.
                    //
                    // We can't use head because that commit isn't merged and the
                    // way GitHub Actions clones this repo isn't even part of the
                    // cloned repo.
                    //
                    // We can't use base either because the squased PR is on top
                    // of current main, which means it might contain commits from
                    // PRs that were merged after this PR got created but before
                    // this PR was merged. IOW, base is too old and if we used it
                    // we'd also include commits from those other PRs.
                    //
                    // So instead we use the <merge commit's SHA> as the after and
                    // <merge commit's SHA>^ as the before.

                    afterText = eventPayload.pull_request.merge_commit_sha;
                    beforeText = afterText + "^";
                }
            }
            else if (eventPayload.before is not null && eventPayload.after is not null)
            {
                beforeText = eventPayload.before;
                afterText = eventPayload.after;
            }
        }

        Console.WriteLine($"pull_request_number = {pullRequestNumber}");
        Console.WriteLine($"pull_request_is_closed = {pullRequestIsClosed.ToString().ToLower()}");
        Console.WriteLine($"pull_request_is_merged = {pullRequestIsMerged.ToString().ToLower()}");
        Console.WriteLine($"before = {beforeText}");
        Console.WriteLine($"after = {afterText}");

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

        using var repository = new Repository(repositoryPath);

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

        var affectedFiles = BlogRepo.GetAffectedPosts(repository, beforeCommit, afterCommit);

        try
        {
            return Run(pullRequestNumber, pullRequestIsClosed, pullRequestIsMerged, affectedFiles);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex);
            return 1;
        }
    }

    private static Commit? ParseRev(Repository repository, string? text)
    {
        if (text is null)
            return null;

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

    private static int Run(int pullRequestNumber, bool pullRequestIsClosed, bool pullRequestIsMerged, string[] affectedFiles)
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

        var markdown = File.ReadAllText(markdownFiles[0]);
        var document = BlogMarkdown.Parse(markdown);

        if (!document.TryGetFrontMatter(out var frontMatter))
        {
            Console.Error.WriteLine("warning: no front matter found");
            return 0;
        }

        var githubOutputPath = Environment.GetEnvironmentVariable("GITHUB_OUTPUT");
        if (!string.IsNullOrEmpty(githubOutputPath))
        {
            File.AppendAllLines(
                githubOutputPath,
                new[]{
                    $"pull_request_number={pullRequestNumber}",
                    $"pull_request_is_closed={pullRequestIsClosed.ToString().ToLower()}",
                    $"pull_request_is_merged={pullRequestIsMerged.ToString().ToLower()}",
                    $"title={frontMatter.PostTitle}",
                    $"alias={frontMatter.MicrosoftAlias}",
                    $"date={frontMatter.DesiredPublicationDate?.ToString("yyyy-MM-dd")}",
                }
            );
        }

        Console.WriteLine($"pull_request_number = {pullRequestNumber}");
        Console.WriteLine($"pull_request_is_closed = {pullRequestIsClosed.ToString().ToLower()}");
        Console.WriteLine($"pull_request_is_merged = {pullRequestIsMerged.ToString().ToLower()}");
        Console.WriteLine($"title = {frontMatter.PostTitle}");
        Console.WriteLine($"alias = {frontMatter.MicrosoftAlias}");
        Console.WriteLine($"date = {frontMatter.DesiredPublicationDate?.ToString("yyyy-MM-dd")}");
        return 0;
    }
}

#nullable disable

internal sealed class GitHubEventPayload
{
    public string action { get; set; }
    public string before { get; set; }
    public string after { get; set; }
    public int number { get; set; }
    public PullRequestPayload pull_request { get; set; }
}

internal sealed class PullRequestPayload
{
    public RefPayload @base { get; set; }
    public bool draft { get; set; }
    public RefPayload head { get; set; }
    public int id { get; set; }
    public bool merged { get; set; }
    public string merge_commit_sha { get; set; }
    public int number { get; set; }
    public string state { get; set; }
    public string title { get; set; }
    public UserPayload user { get; set; }
}

internal sealed class RefPayload
{
    public string sha { get; set; }
    public string @ref { get; set; }
    public string label { get; set; }
}

internal sealed class UserPayload
{
    public string login { get; set; }
}
