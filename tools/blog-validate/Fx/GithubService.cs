using GitHub;
using GitHub.Authentication;
using GitHub.Client;
using System.Text.RegularExpressions;

namespace Microsoft.DotNetBlog.Fx
{
    public class GitHubService
    {
        private const string ClientProductHeader = "blog-validate";
        private string Owner;
        private string Repo;
        private int PullRequestNumber;
        private GitHubClient client;

        public GitHubService(string token, string repo, string githubRef)
        {
            if (Regex.Match(githubRef, @"refs\/pull\/(\d+)\/merge").Success)
            {
                int.TryParse(Regex.Match(githubRef, @"refs\/pull\/(\d+)\/merge").Groups[1].Value, out PullRequestNumber);
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
            catch (GitHub.Models.ValidationError ex)
            {
                Console.WriteLine(ex.MessageEscaped);
                foreach (var error in ex.Errors)
                {
                    Console.WriteLine(error.Message);
                }
                Console.WriteLine($"Exception details: {ex}");
            }
            catch (GitHub.Models.BasicError ex)
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
            catch (GitHub.Models.ValidationError ex)
            {
                Console.WriteLine(ex.MessageEscaped);
            }
            catch (GitHub.Models.BasicError ex)
            {
                Console.WriteLine(ex.MessageEscaped);
            }
        }
    }
}