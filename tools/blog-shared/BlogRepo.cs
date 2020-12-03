using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using LibGit2Sharp;

namespace Microsoft.DotNetBlog
{
    public static class BlogRepo
    {
        public static string[] GetAffectedPosts(Repository repository, Commit beforeCommit, Commit afterCommit = null)
        {
            TreeChanges changes;

            if (afterCommit != null)
            {
                changes = repository.Diff.Compare<TreeChanges>(beforeCommit.Tree, afterCommit.Tree);
            }
            else
            {

                var indexAndWorkingDirectory = DiffTargets.Index | DiffTargets.WorkingDirectory;
                changes = repository.Diff.Compare<TreeChanges>(beforeCommit.Tree, indexAndWorkingDirectory);
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
    }
}
