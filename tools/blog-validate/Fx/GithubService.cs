using GitHub;
using GitHub.Authentication;
using GitHub.Client;
using GitHub.Models;
using System.Text.RegularExpressions;

namespace Microsoft.DotNetBlog.Fx
{
    public partial class GitHubService
    {
        private const string ClientProductHeader = "blog-validate";
        private readonly string Owner;
        private readonly string Repo;
        private readonly int PullRequestNumber;
        private readonly GitHubClient client;

        public GitHubService(string token, string repo, string githubRef)
        {
            if (PullRequestRegex().Match(githubRef).Success)
            {
                int.TryParse(PullRequestRegex().Match(githubRef).Groups[1].Value, out PullRequestNumber);
            }

            Owner = repo.Split('/')[0];
            Repo = repo.Split('/')[1];

            var request = RequestAdapter.Create(new TokenAuthenticationProvider(ClientProductHeader, token));
            client = new GitHubClient(request);
        }

        public async Task TryAddSuggestion(string body, string message, string file, int position, string commit)
        {
            string formattedBody = $"""
                          ```suggestion
                          {body}
                          ```
                          {message}
                          """;
            try
            {
                var comment = new GitHub.Repos.Item.Item.Pulls.Item.Comments.CommentsPostRequestBody
                {
                    Body = formattedBody,
                    CommitId = commit,
                    Path = file,
                    Line = position
                };
                await client.Repos[Owner][Repo].Pulls[PullRequestNumber].Comments.PostAsync(comment);
            }
            catch (ValidationError ex)
            {
                Console.WriteLine(ex.MessageEscaped);
                foreach (var error in ex.Errors ?? Enumerable.Empty<ValidationError_errors>())
                {
                    Console.WriteLine(error.Message);
                }
                Console.WriteLine($"Exception details: {ex}");
            }
            catch (BasicError ex)
            {
                Console.WriteLine(ex.MessageEscaped);
                Console.WriteLine($"Exception details: {ex}");
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
            try
            {
                // Leaving a simple comment on a PR, rather than a pull request review comment
                // requires using the Issue Comment API using the PR # as Issue #
                // https://docs.github.com/en/rest/issues/comments#create-an-issue-comment
                var comment = await client.Repos[Owner][Repo].Issues[PullRequestNumber].Comments.PostAsync(
                    new GitHub.Repos.Item.Item.Issues.Item.Comments.CommentsPostRequestBody
                    {
                        Body = body,
                    });
            }
            catch (ValidationError ex)
            {
                Console.WriteLine(ex.MessageEscaped);
            }
            catch (BasicError ex)
            {
                Console.WriteLine(ex.MessageEscaped);
            }
        }

        [GeneratedRegex(@"refs\/pull\/(\d+)\/merge")]
        private static partial Regex PullRequestRegex();
    }
}