using System.Diagnostics;

using Humanizer;

using Spectre.Console;

namespace Microsoft.DotNetBlog;

internal static class Program
{
    private static void Main()
    {
        var authorInformation = AuthorInformation.Load();
        if (authorInformation is null)
        {
            AnsiConsole.Render(new Rule { Title = "[gray]User information[/]", Alignment = Justify.Left });

            var microsoftAlias = AnsiConsole.Ask("What's your Microsoft [cyan]alias[/]?", GetDefaultMicrosoftAlias());

            if (!AnsiConsole.Confirm("Do you know your WordPress user name?"))
            {
                AnsiConsole.MarkupLine("Check the [cyan]Username[/] field in your WordPress profile.");

                var profileUrl = "https://devblogs.microsoft.com/dotnet/wp-admin/profile.php";

                if (!OperatingSystem.IsWindows())
                {
                    AnsiConsole.MarkupLine("[gray]Navigate to the following URL in your browser:[/]");
                    AnsiConsole.MarkupLine(profileUrl);
                }
                else
                {
                    AnsiConsole.MarkupLine("[gray]Hit any key to open browser[/]");
                    Console.ReadKey();
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = profileUrl,
                        UseShellExecute = true
                    });
                }
            }

            var wordPressUserName = AnsiConsole.Ask<string>("What's your [cyan]WordPress user name[/]?");

            authorInformation = new AuthorInformation(microsoftAlias, wordPressUserName);
            authorInformation.Save();
        }

        AnsiConsole.Render(new Rule { Title = "[gray]Create blog post[/]", Alignment = Justify.Left });

        var postTitle = AnsiConsole.Ask<string>("What's the post [cyan]title[/]?");
        var postName = AnsiConsole.Ask("What's the post [cyan]name[/] (used for md file name)?", GetDefaultPostName(postTitle));
        var postSlug = AnsiConsole.Ask("What's the post [cyan]slug[/] (used on blog in wordpress)?", GetDefaultPostName(postTitle));
        var desiredPublicationDate = AnsiConsole.Ask("What's the desired [cyan]publication date[/]? Please give several days for review and SEO optimization.", GetDefaultPublicationDate());

        var categories = AnsiConsole.Prompt(
            new MultiSelectionPrompt<string>()
                .Title("What are the post [cyan]categories[/]?")
                .NotRequired()
                .PageSize(20)
                .MoreChoicesText("[grey](Move up and down to reveal more categories)[/]")
                .InstructionsText("[grey](Press [blue]<space>[/] to toggle a categorie, [green]<enter>[/] to accept)[/]")
                .AddChoices(GetCategories()
            )
        );

        var path = $"{desiredPublicationDate.Year}/{desiredPublicationDate:MM-MMM}/{postName}/{postName}.md";

        if (File.Exists(path))
        {
            var overwrite = AnsiConsole.Confirm("Post already exists. [cyan]Overwrite[/]?", false);
            if (!overwrite)
                return;
        }

        Directory.CreateDirectory(Path.GetDirectoryName(path)!);

        var post = @$"---
post_title: {postTitle}
author1: {authorInformation.WordPressUserName}
post_slug: {postSlug}
username: {authorInformation.WordPressUserName}
microsoft_alias: {authorInformation.MicrosoftAlias}
featured_image: path/relative/to/your/post/image.png
categories: {string.Join(", ", categories)}
tags: tag1, tag2, tag3
summary: Summary of your post, shown on the home page next to the featured image
desired_publication_date: {desiredPublicationDate:yyyy-MM-dd}
post_date: {desiredPublicationDate:yyyy-MM-dd} 10:05:00
---

This is the introduction to your post. It's the post's first paragraph. Don't
put a heading in front of it. The title will be the post's first heading.

## First actual heading

Your content's first heading should be an H2 because the title is the page's
H1.

## Summary

Some summary or call to action.";

        File.WriteAllText(path, post);
    }

    private static string GetDefaultPostName(string postTitle)
    {
        return string.Concat(postTitle.Replace("#", "sharp")
                                      .Replace(".NET", "dotnet", StringComparison.OrdinalIgnoreCase)
                                      .Where(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c)))
                     .Kebaberize();
    }

    private static string GetDefaultMicrosoftAlias()
    {
        return Environment.UserName;
    }

    private static DateTime GetDefaultPublicationDate()
    {
        var numberOfDays = 4;
        var current = DateTime.Today;

        while (numberOfDays-- > 0)
            current = AdjustToNextWeekDay(current.AddDays(1));

        return current;

        static DateTime AdjustToNextWeekDay(DateTime dateTime)
        {
            return dateTime.DayOfWeek switch
            {
                DayOfWeek.Friday => dateTime.AddDays(3),
                DayOfWeek.Saturday => dateTime.AddDays(2),
                DayOfWeek.Sunday => dateTime.AddDays(1),
                _ => dateTime
            };
        }
    }

    private static string[] GetCategories()
    {
        var fileName = Path.GetFullPath("categories.txt");
        if (!File.Exists(fileName))
            return Array.Empty<string>();

        return File.ReadAllLines(fileName);
    }
}

internal sealed class AuthorInformation
{
    public AuthorInformation(string microsoftAlias, string wordPressUserName)
    {
        MicrosoftAlias = microsoftAlias;
        WordPressUserName = wordPressUserName;
    }

    public string MicrosoftAlias { get; }
    public string WordPressUserName { get; }

    private static string GetFileName()
    {
        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Microsoft", ".NET Blog", "user.txt");
    }

    public static bool Delete()
    {
        var fileName = GetFileName();
        if (File.Exists(fileName))
        {
            File.Delete(fileName);
            return true;
        }

        return false;
    }

    public static AuthorInformation? Load()
    {
        var fileName = GetFileName();
        if (File.Exists(fileName))
        {
            var lines = File.ReadAllLines(fileName);
            if (lines.Length == 2)
            {
                var microsoftAlias = lines[0];
                var wordPressUserName = lines[1];
                return new AuthorInformation(microsoftAlias, wordPressUserName);
            }
        }

        return null;
    }

    public void Save()
    {
        var fileName = GetFileName();
        Directory.CreateDirectory(Path.GetDirectoryName(fileName)!);
        File.WriteAllLines(fileName, new[] { MicrosoftAlias, WordPressUserName });
    }
}
