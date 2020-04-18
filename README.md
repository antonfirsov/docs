# .NET Blog

This repository is for reviewing and authoring our blog posts. The blogs posts
should be authored in text, ideally Markdown.

The directory structure should look like this:

    2014
    └───08-Aug
        └───interns-at-microsoft
            interns-at-microsoft.md
            CharlesLovell.png
            ChristianSalgadoPacheco.png
            IanHays.png
            SantiagoFernandezMadero.png
            ShaunArora.png
            ZachMontoya.png

In other words:

* A top level folder per year
* One nested folder per month, with the two-digit month number, a hyphen, and the three letter abbreviation for the month.
* One nested folder per post. The folder name should reflect the post's title.
* The post folder should contain all assets, especially images

## Reviewing Posts

In order to get the post reviewed, you will need to submit a pull request
against the `master` branch. This also means that new posts should be authored
in their own branch.

Here is the workflow:

1. Create a branch for your post. Use the directory name as the branch name, e.g.

        git checkout -b 2014/08-interns-at-microsoft

2. Author your post and commit to this newly created branch

3. Publish your branch

        git push origin 2014/08-interns-at-microsoft

4. Create a pull request

## Instructions for bloggers

1. Request access to the [dotnet team in microsoft org](https://repos.opensource.microsoft.com/microsoft/teams/dotnet/join/)
2. Submit a PR to [.NET Blog](https://github.com/microsoft/dotnet-blog)
    * We author in Markdown
3. [Sign in to the .NET blog](https://devblogs.microsoft.com/dotnet/wp-login.php?redirect_to=https%3A%2F%2Fdevblogs.microsoft.com%2Fdotnet%2F)
    * Use your `alias@microsoft.com` email
    * This allows us to mark you as the author for the post
4. [Setup a profile picture](https://devblogs.microsoft.com/dotnet/wp-admin/profile.php)  
    * It's showcased at the top of every post
    * If you're not comfortable using an actual photograph, choose something else

## Instructions for reviewers

In order for folks to get access to this repo, they can request access via this
link:

1. Request access to the [dotnet team in microsoft org](https://repos.opensource.microsoft.com/microsoft/teams/dotnet/join/)
2. Leave feedback on the PR
