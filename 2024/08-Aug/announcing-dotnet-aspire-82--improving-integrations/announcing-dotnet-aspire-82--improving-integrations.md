---
post_title: Announcing .NET Aspire 8.2 - Goodbye Components, Hello Integrations!
author1: maddy-leger
post_slug: announcing-dotnet-aspire-8-2
microsoft_alias: maleger
featured_image: aspire-8-2.jpg
categories: .NET Aspire
tags: aspire, dotnet, integrations
ai_note: hide
summary: .NET Aspire 8.2 is here with some nice updates for components... we mean integrations! Learn more about this rename, what it means for you, and what the teams has been adding to testing in .NET Aspire!
post_date: 2024-08-29 10:05:00
---

.NET Aspire 8.2 is shipping today, and you can download or update to [today's release](https://learn.microsoft.com/dotnet/aspire/fundamentals/setup-tooling) now. While this release doesn't introduce big new features like in [.NET Aspire 8.1](https://devblogs.microsoft.com/dotnet/whats-new-in-aspire-8-1/), it does improve onboarding, testing, and have other quality of life improvements. 

## Components have a new name!

One of the major changes you'll notice in .NET Aspire 8.2 is that .NET Aspire Components are now called "Integrations"! A .NET Aspire Integration is a package that you add to your app that streamlines the process of setting up, starting up, and communicating with prominent cloud services and platforms.

Integrations are used in two ways in project using .NET Aspire:
1. As a "Hosting" package added to your AppHost project, letting you easily spin up the resource and connect to it alongside your projects during local development.
1. As a package in your app code, for connecting to the resource created in your AppHost, as well as streamlining setup and defaults to lower the burden of adding a new cloud service.

We originally named these "components" because... well... they're components! But we've realized that it's such an overloaded term in development that we were actually just confusing people (and ourselves). Our [documentation](https://learn.microsoft.com/dotnet/aspire/fundamentals/integrations-overview) has been updated to reflect the change to "Integrations" and we will be using that terminology in our content moving forward.

## Testing, testing, and more testing

Another core part of this release has been strengthening our own system to handle how often Integrations are updated. Of course, one of the best ways to do that is... tests! The .NET Aspire team, along with some amazing community contributors, have been bolstering our suite of tests throughout this release so that we can quickly bump a version and make sure it isn't going to break anything in your apps. A special thank you to our top contributors from the past month - [@alirexaa](https://github.com/Alirexaa), [@davidebbo](https://github.com/davidebbo), and [more](https://github.com/dotnet/aspire/graphs/contributors) - for all the great work you brought into 8.2 for us.

## Catch up & join .NET Aspire events
Since the launch of .NET Aspire in May, together with the community we have had several event going deeper on development with .NET Aspire. 

* [Let's Learn .NET Aspire](https://www.youtube.com/@dotnet/search?query=let%27s%20learn%20.net%20aspire): A full 2 hour workshop broadcast live in 7 different languages around the world.
* [.NET Aspire Developers Day](https://www.youtube.com/playlist?list=PLdo4fOcmZ0oWMbEO7CiaDZh6cqSTU_lzJ): A full day of .NET Aspire with the product team and community covering all things .NET Aspire with 14 total sessions!

<iframe width="800" height="450" src="https://www.youtube.com/embed/videoseries?si=YNLACUvXidek9f7F&amp;list=PLdo4fOcmZ0oWMbEO7CiaDZh6cqSTU_lzJ" title="YouTube video player" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share" referrerpolicy="strict-origin-when-cross-origin" allowfullscreen></iframe>

Join us live on September 18th for **[Azure Developers - .NET Aspire Day](https://aka.ms/azuredevelopers/dotnetaspireday)** where teams across Azure will be highlighting the latest for .NET Aspire developers for cloud development. [Register for free today!](https://aka.ms/azuredevelopers/dotnetaspireday)

## Try it out and let us know your feedback 📣

We're now turning our focus towards .NET Aspire 9.0, which ships at the same time as [.NET 9](https://dotnetconf.net), and would love to hear what YOU want to see in upcoming releases. Feel free to engage on our [GitHub](https://github.com/dotnet/aspire), fill out our [survey](https://www.surveymonkey.com/r/WL9VC22?sessionId=[sessionId_value]), and visit our [What's New](https://learn.microsoft.com/dotnet/aspire/whats-new/) page in our docs to learn more and download today!
