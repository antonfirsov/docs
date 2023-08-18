using Octokit;
using System.Text.RegularExpressions;

namespace Microsoft.DotNetBlog.Fx
{
    public class GitHubService
    {
        private const string ClientProductHeader = "blog-validate";
        private string Owner;
        private string Repo;
        private int PullRequestNumber;
        private string Commit;
        private GitHubClient client;

        public GitHubService()
        {
            string token = Environment.GetEnvironmentVariable("GITHUB_TOKEN") ?? string.Empty;
            Commit = Environment.GetEnvironmentVariable("GITHUB_COMMITID") ?? string.Empty;
            string fullRepo = Environment.GetEnvironmentVariable("GITHUB_REPOSITORY") ?? string.Empty;
            string githubRef = Environment.GetEnvironmentVariable("GITHUB_REF") ?? string.Empty;

            if (Regex.Match(githubRef, @"refs\/pull\/(\d+)\/merge").Success)
            {
                int.TryParse(Regex.Match(githubRef, @"refs\/pull\/(\d+)\/merge").Groups[1].Value, out PullRequestNumber);
            }

            Owner = fullRepo.Split('/')[0];
            Repo = fullRepo.Split('/')[1];

            Console.WriteLine($"Owner: {Owner}");
            Console.WriteLine($"Repo: {Repo}");
            Console.WriteLine($"PullRequestNumber: {PullRequestNumber}");
            Console.WriteLine($"Commit: {Commit}");

            client = new GitHubClient(new ProductHeaderValue(ClientProductHeader));
            var tokenAuth = new Credentials(token);
            client.Credentials = tokenAuth;
        }

        public async Task TryAddSuggestion(string body, string file, int position)
        {
            string formattedBody = """
                          ```
                          {body}
                          ```
                          """;
            var comment = new PullRequestReviewCommentCreate(formattedBody, Commit, file, position);
            try {
                await client.PullRequest.ReviewComment.Create(Owner, Repo, PullRequestNumber, comment);
             }
            catch (Exception ex)
            {
                Console.WriteLine("::group::{Suggestion failed}");
                Console.WriteLine($"Exception details: {ex}");
                Console.WriteLine("::endgroup::");
            }
        }

        public async Task AddComment(string body)
        {
            await client.Issue.Comment.Create(Owner, Repo, PullRequestNumber, body);
        }
    }
}