using LibGit2Sharp;

namespace Microsoft.DotNetBlog;

public static class BlogRepo
{
    public static string[] GetAffectedPosts(Repository repository, Commit beforeCommit, Commit? afterCommit = null)
    {
        TreeChanges changes;

        if (afterCommit == null)
        {
            var indexAndWorkingDirectory = DiffTargets.Index | DiffTargets.WorkingDirectory;
            changes = repository.Diff.Compare<TreeChanges>(beforeCommit.Tree, indexAndWorkingDirectory);
        }
        else
        {
            // In our case, before is going to be the target branch of the PR (usually current main) while after
            // is the latest commit in the PR.
            // 
            // In order to compute the changes of the PR, we need to do the equivalent of this:
            //
            //   $ git diff before...after
            //
            // Notice that there are three dots, not two. This will first find the merge base of before and after
            // (which is the common ancestor) and then do a diff between it and after.
            //
            // This ensures we only get the changes introduced in the PR, not any of the changes that were done to
            // main after the PR branched off.

            var historyDivergence = repository.ObjectDatabase.CalculateHistoryDivergence(beforeCommit, afterCommit);
            changes = repository.Diff.Compare<TreeChanges>(historyDivergence.CommonAncestor.Tree, afterCommit.Tree);
        }

        return changes.Where(c => c.Status != ChangeKind.Deleted)
                      .Select(c => Path.GetFullPath(Path.Combine(repository.Info.WorkingDirectory, c.Path)))
                      .Where(p => IsIncluded(repository.Info.WorkingDirectory, p))
                      .ToArray();
    }

    public static IEnumerable<string> GetPosts(string directory)
    {
        return Directory.GetFiles(directory, "*.md", SearchOption.AllDirectories)
                        .Where(p => IsIncluded(directory, p));
    }

    private static bool IsIncluded(string repoPath, string path)
    {
        if (!string.Equals(Path.GetExtension(path), ".md", StringComparison.OrdinalIgnoreCase))
            return false;

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
}
