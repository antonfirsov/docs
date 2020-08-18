# .NET Blog

This repository is for reviewing and authoring our blog posts. The blogs posts
should be authored in Markdown.

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

## Instructions for bloggers

1. Request access to the [dotnet team in the microsoft org](https://repos.opensource.microsoft.com/microsoft/teams/dotnet/join/)
2. Submit a PR to [dotnet-blog](https://github.com/microsoft/dotnet-blog)
    * Author post in Markdown
    * Make sure you have front-matter with title & featured image. You can use [this template](templates/blank.md).
    * Ensure all pictures are accessible by using the `alt` tag
    * Add subject matter experts (SMEs) from your engineering team/partner team as reviewers
    * Don't merge until `dotnet-blog-owners` and your SMEs sign off
    * We have an integration that will automatically stage the post in WordPress when the PR is merged. For more details, see this guide on [Drafting in GitHub](https://dev.azure.com/devdiv/DevDiv/_wiki/wikis/DevDiv.wiki/10339/Drafting-in-GitHub).
3. [Sign in to the .NET blog](https://devblogs.microsoft.com/dotnet/wp-login.php?redirect_to=https%3A%2F%2Fdevblogs.microsoft.com%2Fdotnet%2F)
    * Use your `alias@microsoft.com` email
    * [Request being added as an author](mailto:netblogowners@microsoft.com?subject=Requesting%20access%20to%20the%20.NET%20blog&body=Hey%20.NET%20blog%20owners%2C%0A%0APlease%20add%20me%20to%20the%20.NET%20blog%20as%20an%20author.%0A%0AE-mail%20address%20I%20used%20in%20WordPress%3A%20______%0A%0AThanks%21)
4. [Setup a profile picture](https://devblogs.microsoft.com/dotnet/wp-admin/profile.php)
    * It's showcased at the top of every post
    * If you're not comfortable using an actual photograph, choose something else
5. [Review staged post in WordPress](https://devblogs.microsoft.com/dotnet/wp-admin/edit.php)
    * After merging the PR, the post should automatically be staged in WordPress
    * Review title, author, categories, and tags    
6. Coordinate publishing with [.NET Blog Owners](mailto:netblogowners@microsoft.com)

## Instructions for reviewers

In order for folks to get access to this repo, they can request access via this
link:

1. Request access to the [dotnet team in microsoft org](https://repos.opensource.microsoft.com/microsoft/teams/dotnet/join/)
2. Leave feedback on the PR
