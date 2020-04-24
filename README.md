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
    * Add subject matter experts (SMEs) from your engineering team/partner team as reviewers
    * Don't merge until `dotnet-blog-owners` and your SMEs sign off
3. [Sign in to the .NET blog](https://devblogs.microsoft.com/dotnet/wp-login.php?redirect_to=https%3A%2F%2Fdevblogs.microsoft.com%2Fdotnet%2F)
    * Use your `alias@microsoft.com` email
    * [Send mail to netblogowners@microsoft.com](mailto:netblogowners@microsoft.com?subject=Request+access+to+blog&body=Hey+.NET+blog+owners%2C%0A%0APlease+give+me+author+permissions+for+the+.NET+blog.%0A%0AEmail+I+used+for+WordPress%3A+____%0A%0AThanks!)
4. [Setup a profile picture](https://devblogs.microsoft.com/dotnet/wp-admin/profile.php)
    * It's showcased at the top of every post
    * If you're not comfortable using an actual photograph, choose something else
5. [Create a new post](https://devblogs.microsoft.com/dotnet/wp-admin/post-new.php)
    * Copy & paste the Markdown
    * Upload pictures
    * Ensure all pictures are accessible by using the `alt` tag

## Instructions for reviewers

In order for folks to get access to this repo, they can request access via this
link:

1. Request access to the [dotnet team in microsoft org](https://repos.opensource.microsoft.com/microsoft/teams/dotnet/join/)
2. Leave feedback on the PR
