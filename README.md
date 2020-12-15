# .NET Blog

This repository is for reviewing and authoring our blog posts. The blogs posts
must be authored in Markdown using this template](templates/blank.md).

The directory structure should look like this:

    2014
    └───08-Aug
        └───interns-at-microsoft
            ├───interns-at-microsoft.md
            ├───CharlesLovell.png
            ├───ChristianSalgadoPacheco.png
            ├───IanHays.png
            ├───SantiagoFernandezMadero.png
            ├───ShaunArora.png
            └───ZachMontoya.png

In other words:

* A top-level folder per year
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

## External authors

Have you explored the idea of inviting external bloggers onto DevBlogs but not
sure how to go about it? We now have a process in place to enable to do just
this:

1. **Waiver &/or NDA** (depending on the situation). For when external authors
   are granted "external author" role to come into our blog space to draft and
   publish their content. See [NDA for External Authors.docx][NDA].

2. **Waiver**. For when external authors grant us permission to publish their
   content on our site. See [License for External Authors.docx][License].

Additionally in terms of backend access and blog author list management, we now
have a new user role *external author*. Non-Microsoft authors should ideally be
given this role which is only slightly different from regular "author" roles:

1. In the media folder they can only see media files uploaded by them
2. In the "all posts" view they only see posts authored by them

This ensures that external authors don't accidentally see draft post titles or
media files for important announcements that have not yet been published.

Reach out to us on [devblogsplatform](mailto:devblogsplatform@microsoft.com) if
you have any questions.

## Validation

We have a validation tool that automatically runs on CI builds. You can invoke
this from VS Code via <kbd>Ctrl</kbd> <kbd>Shift</kbd> <kbd>B</kbd> (or whatever
you bound the build command to) or via the command line by invoking
`validate.cmd`.

[NDA]: https://microsoft.sharepoint.com/:w:/r/teams/dd_vsblog/_layouts/15/Doc.aspx?sourcedoc=%7BFD4908C7-2B4A-476E-8C74-97C20CE4CD33%7D&file=NDA%20Process%20for%20External%20Blog%20Authors.docx&action=default&mobileredirect=true
[License]: https://microsoft.sharepoint.com/:w:/t/dd_vsblog/EdbLLTgQuLRGuPVzkLnn8ewBfpXexWoMA-bquVRbDMxMlQ?e=LZQdoB