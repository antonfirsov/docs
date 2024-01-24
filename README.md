# .NET Blog

[WordPress] | [Schedule] | [Local Tutorial] | [Codespaces Tutorial]

This repository is for reviewing and authoring our blog posts.

Blogging for the first time? Check out one of our tutorials:

* [Local Tutorial]
* [Codespaces Tutorial]

## Creating a new post

Create your own codespace that is pre-configured for you, or open one that you already created. No need to Fork.

[![Open in GitHub Codespaces](https://github.com/codespaces/badge.svg)](https://github.com/codespaces/new?hide_repo_select=true&ref=main&repo=microsoft/dotnet-blog)

The easiest way to get started is by running this command:

Windows:
```text
$ ./new-post
```

Mac/Linux/Codespaces:
```text
$ ./new-post.sh
```

This will ask you some questions and will create the necessary boilerplate for
the blog post.

Ensure to follow the [instructions](#instructions-for-bloggers) below.

## Format and Structure

The blogs posts must be authored in Markdown using [this template](templates/blank.md).

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
* One nested folder per month, with the two-digit month number, a hyphen, and
  the three letter abbreviation for the month.
* One nested folder per post. The folder name should reflect the post's title.
* The post folder should contain all assets, especially images

## Instructions for bloggers

1. [Sign in to the .NET blog](https://devblogs.microsoft.com/dotnet/wp-login.php?redirect_to=https%3A%2F%2Fdevblogs.microsoft.com%2Fdotnet%2F)
    * Use your `alias@microsoft.com` email
    * [Request being added as an author](mailto:netblogowners@microsoft.com?subject=Requesting%20access%20to%20the%20.NET%20blog&body=Hey%20.NET%20blog%20owners%2C%0A%0APlease%20add%20me%20to%20the%20.NET%20blog%20as%20an%20author.%0A%0AE-mail%20address%20I%20used%20in%20WordPress%3A%20______%0A%0AThanks%21)
1. [Setup a profile picture](https://devblogs.microsoft.com/dotnet/wp-admin/profile.php)
    * It's showcased at the top of every post
    * If you're not comfortable using an actual photograph, choose something else
1. Request access to the [dotnet team in the microsoft org](https://repos.opensource.microsoft.com/microsoft/teams/dotnet/join/)
1. Submit a PR to [dotnet-blog](https://github.com/microsoft/dotnet-blog)
    * Author post in Markdown. Run the `new-post` shell script in the root, it will create the boilerplate for you.
    * To pick a publication date, consult the [blogging schedule](https://tasks.office.com/microsoft.onmicrosoft.com/en-US/Home/Planner#/plantaskboard?planId=wHOgFOjggEyrykcunh6oQpUAARuD).
      We generally only publish one post per day per blog and we don't publish
      on Fridays. Also, you need to add buffering for review and SEO. Plan for a
      minimum of three days between PR creation and publication date.
    * Add subject matter experts (SMEs) from your engineering team/partner team as reviewers
    * We have an integration that will automatically stage the post in WordPress when the PR is merged. For more details, see this guide on [Drafting in GitHub](https://dev.azure.com/devdiv/DevDiv/_wiki/wikis/DevDiv.wiki/10339/Drafting-in-GitHub).
1. Coordinate review with the **@microsoft/dotnet-blog-owners**
    * All errors reported by the CI validation need to be addressed, otherwise
      your post won't be staged properly in WordPress
    * Once your post is merged, it will be automatically scheduled in WordPress
1. [Review staged post in WordPress](https://devblogs.microsoft.com/dotnet/wp-admin/edit.php)
    * Preview post
    * Ensure it looks right (title, author, categories, tags, and scheduling settings)

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

## AI Assisted Content

We're not opposed to using AI (such as Office Copilot, Bing Chat, or ChatGPT)
for generating parts of your blog post. However, as the author it is still your
responsibility to edit, check, and ultimately stand behind the text that is
published on our blog.

> [!CAUTION]
> If you're using AI technologies to generate content, even if only for parts of
> your blog post, you must display a disclaimer. This can be controlled in
> WordPress or via the YAML front matter using `ai_note: show`. See [DevDiv
> wiki][ai-note] for more details.

> [!NOTE]
> You only need to display the disclaimer if you include content generated by an
> AI (text or source code). If you merely used an AI in the process of
> understanding the domain, for example, by asking questions or finding
> documentation, then you don't need to add the disclaimer.

[ai-note]: https://dev.azure.com/devdiv/DevDiv/_wiki/wikis/DevDiv.wiki/27974/FAQ-Blog-post-draft-stage-review-publish?anchor=q%3A-...-show-that-my-post-may-have-some-ai-generated-content%3F

## Validation

We have a validation tool that automatically runs on CI builds. You can invoke
this from VS Code via <kbd>Ctrl</kbd> <kbd>Shift</kbd> <kbd>B</kbd> (or whatever
you bound the build command to) or via the command line by invoking
`validate.cmd`.

[NDA]: https://microsoft.sharepoint.com/:w:/r/teams/dd_vsblog/_layouts/15/Doc.aspx?sourcedoc=%7BFD4908C7-2B4A-476E-8C74-97C20CE4CD33%7D&file=NDA%20Process%20for%20External%20Blog%20Authors.docx&action=default&mobileredirect=true
[License]: https://microsoft.sharepoint.com/:w:/t/dd_vsblog/EdbLLTgQuLRGuPVzkLnn8ewBfpXexWoMA-bquVRbDMxMlQ?e=LZQdoB
[WordPress]: https://devblogs.microsoft.com/dotnet/
[Schedule]: https://tasks.office.com/microsoft.onmicrosoft.com/en-US/Home/Planner#/plantaskboard?groupId=fdff90ed-0b3b-4caa-a30a-efb4dd47665f&planId=wHOgFOjggEyrykcunh6oQpUAARuD
[Local Tutorial]: https://msit.microsoftstream.com/video/619b0840-98dc-b561-c7a4-f1ebf7e10ab0
[Codespaces Tutorial]: https://microsoft.sharepoint.com/:v:/t/DotNetTeam/ERYggqFwUWhFsdw_lROALbMBJRmI2NckeFaOGLJLUudzKw?e=jY9MsH

## Highlighting code blocks

As you probably know, you can wrap a snippet of code in triple back ticks, you can give hint at the syntax colorizer to use. Here's an example where we specified `csharp`:
````
```csharp
public static int Main()
{
    return 1;
}
```
````
and here's how it looks rendered:
```csharp
public static int Main()
{
    return 1;
}
```

If you put in no hint at all, or one that highlight.js doesn't recognize, it will attempt to infer a "best fit" colorizer, but it's better to be explicit. For example, it will typically recognize x86asm, despite some of our old posts using the hint 'assembler' which it doesn't recognize.

Below is a table of some common hints that are good to use. Some are synonyms of others, use whichever you prefer.

| good hints    |
| ------------- |
| armasm        |
| bash          |
| c             |
| c#            |
| c++           |
| cmd           |
| csharp        |
| cshtml        |
| fsharp        |
| groovy        |
| html          |
| http          |
| ini           |
| java          |
| javascript    |
| js            |
| json          |
| markdown      |
| md            |
| plaintext     |
| powershell    |
| razor         |
| sh            |
| shell         |
| sql           |
| text          |
| ts            |
| txt           |
| typescript    |
| vb            |
| x86asm        |
| xml           |
| yaml          |
| yml           |

Full information is [here](https://github.com/highlightjs/highlight.js/blob/main/SUPPORTED_LANGUAGES.md). It's subject to change, and the blog site may choose to exclude some languages in its build.

Until 12/2023, we used prettify.js, whose hints don't all map to highlight.js. Here's a mapping in case you're used to prettify hints or want to fix an old post.

| old           | new                                                            |
| ------------- | -------------------------------------------------------------- |
| asm           | x86asm (but should infer)                                      |
| aspx-csharp   | use 'csharp'                                                   |
| assembly      | x86asm (but should infer)                                      |
| bash          | OK                                                             |
| c             | OK                                                             |
| c#            | OK                                                             |
| c++           | OK                                                             |
| cli           | use 'dos' or 'sh' as appropriate                               |
| cmd           | OK                                                             |
| console       | OK, but prefer 'sh' as it doesn't mean 'cmd'                   |
| cs            | OK                                                             |
| csharp        | OK                                                             |
| cshtml        | OK                                                             |
| csproj        | use 'xml'                                                      |
| csv           | not supported by highlight.js, use 'text'                      |
| diff          | OK                                                             |
| dockerfile    | OK                                                             |
| f#            | OK                                                             |
| fsharp        | OK                                                             |
| groovy        | OK                                                             |
| html          | OK                                                             |
| http          | OK                                                             |
| il            | not supported by highlight.js, use 'text'                      |
| ini           | OK                                                             |
| java          | OK                                                             |
| javascript    | OK                                                             |
| js            | OK                                                             |
| json          | OK                                                             |
| log           | use 'text'                                                     |
| markdown      | OK                                                             |
| md            | OK                                                             |
| mermaid       | not supported by highlight.js                                  |
| none          | use 'text'                                                     |
| output        | use 'text'                                                     |
| plaintext     | OK                                                             |
| powershell    | OK                                                             |
| proto         | OK                                                             |
| qml           | OK                                                             |
| razor         | OK                                                             |
| sh            | OK                                                             |
| shell         | OK                                                             |
| sql           | OK                                                             |
| text          | OK                                                             |
| ts            | OK                                                             |
| txt           | OK                                                             |
| typescript    | OK                                                             |
| vb            | OK                                                             |
| visual basic  | use 'vb'                                                       |
| xaml          | oddly not known by highlight.js, use 'xml' if not autodetected |
| xml           | OK                                                             |
| yaml          | OK                                                             |
| yml           | OK                                                             |

