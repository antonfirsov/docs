---
post_title: 'Announcing the official OpenAI library for .NET'
author1: dotnet
post_slug: openai-dotnet-library
microsoft_alias: scaddie
featured_image: openai-dotnet-library-announcement.jpg
categories: .NET, AI, Azure
tags: ai, announcement, openai
ai_note: hide
summary: 'The initial beta release of the official OpenAI library for .NET is now available.'
post_date: 2024-06-06 12:30:00
---

At Microsoft Build 2024, [we announced](https://devblogs.microsoft.com/dotnet/dotnet-build-2024-announcements/#expanding-the-net-ai-ecosystem) new investments that expand the AI ecosystem for .NET developers. We're excited to share more detailed plans around Microsoft's collaboration with OpenAI on their *official* .NET library.

Today, the OpenAI team released their first beta, [version 2.0.0-beta.1](https://www.nuget.org/packages/OpenAI/2.0.0-beta.1), of the official OpenAI library for .NET. Features include:

- Support for the entire OpenAI API, including [Assistants v2](https://platform.openai.com/docs/assistants/overview) and [Chat Completions](https://platform.openai.com/docs/guides/text-generation/chat-completions-api)
- Support for GPT-4o, OpenAI's latest flagship model
- Extensibility to enable the community to build libraries on top
- Sync and async APIs for ease of use and efficiency
- Access to streaming completions via `IAsyncEnumerable<T>`

This official .NET library ensures a smooth and supported integration with OpenAI and Azure OpenAI. It also complements OpenAI's official libraries for Python and TypeScript/JavaScript developers.

The .NET library is developed and supported on [GitHub](https://github.com/openai/openai-dotnet) and will be kept up to date with the latest features from OpenAI. Work will continue over the next few months to gather feedback to improve the library and release a stable NuGet package.

## Thank you to the .NET community

We'd like to thank and recognize the work of [Roger Pincombe](https://github.com/OkGoDoIt) on his library that was published under the *OpenAI* v1.x NuGet package name. Roger initially published the library in June 2020, making it the first known OpenAI package for .NET. He volunteered countless hours of personal time ever since to maintain the project on GitHub. Roger has worked closely with OpenAI and Microsoft on our plans for the official .NET package for OpenAI. Roger is also helping with a migration guide from his package to the new official one.

Of course, developers may choose to continue using their favorite community libraries, like:

- [Betalgo.OpenAI](https://github.com/betalgo/openai) by [Betalgo](https://github.com/betalgo)
- [OpenAI-DotNet](https://github.com/RageAgainstThePixel/OpenAI-DotNet) by [RageAgainstThePixel](https://github.com/RageAgainstThePixel)

OpenAI and the .NET team also thank these project maintainers for their extraordinary efforts in filling a void within the community. Even with the release of the official package from OpenAI, there are opportunities for community libraries to add significant value on top. We look forward to collaborating with the community in this space.

## Next steps

Here's how you can get involved:

- **Try the library**: Install the [OpenAI .NET library](https://www.nuget.org/packages/OpenAI/2.0.0-beta.1) and start experimenting with its features.
- **Join the community**: Engage with us and other developers on [GitHub](https://github.com/openai/openai-dotnet). Share your experiences, report issues, and contribute to discussions.
- **Attend the live stream**: Join us live at 10:00 AM PDT on June 19 for the [.NET AI Community Standup](https://dotnet.microsoft.com/live/community-standup). Ask questions, learn more about the library, and see demos of its capabilities.
