---
post_title: 'Announcing Open Source C# standardization'
username: wiwagn
microsoft_alias: wiwagn
categories: .NET, C#
summary: 'The C# standards committee moved its work into Open Source. ECMA worked with the .NET Foundation, enabling C# developers worldwide to participate in the standardization effort. Visit the C# standards repo for more information.'
desired_publication_date: '2021-04-05'
---

The C# compilers have been open source since 2014, now in the [dotnet/roslyn](https://github.com/dotnet/roslyn) repository. The [dotnet/csharplang](https://github.com/dotnet/csharplang) split off to provide a dedicate public space for the innovation and evolution of the C# language. Now, [dotnet/csharpstandard](https://github.com/dotnet/csharpstandard) completes the group, providing a public space for the ongoing work to document the standard for the latest C# language versions.

## Welcome dotnet/csharpstandard

Moving the standards work into the open, under the .NET Foundation, makes it easier for standardization work. Everything from language innovation and feature design through implementation and on to standardization now takes place in the open. It will be easier to ask questions among the language design team, the compiler implementers, and the standards committee. Even better, those conversations will be public.

The end result will be a more accurate standard for the latest versions of C#.

## Opening the process

The ECMA C# standards committee, [TC-49-TG2](https://www.ecma-international.org/task-groups/tc49-tg2/) is  still responsible for creating the proposed standard for the C# language. What changes is that work now happens in the open, under the [.NET Foundation](https://dotnetfoundation.org/projects/csharp-standard). You can see work in progress on the standard text for [C# 6](https://github.com/dotnet/csharpstandard). This work merges the draft spec currently hosted in the [`csharplang`](https://github.com/dotnet/csharplang/tree/main/spec) repository with the current [C# 5.0 standard text](https://www.ecma-international.org/publications-and-standards/standards/ecma-334/). Work on incorporating the C# 7 features is taking place as well. See the [C# 7 draft](https://github.com/dotnet/csharpstandard/tree/draft-v7) branch for progress.

The addition of `dotnet/csharpstandard` means there are now three different repositories related to the C# language. Each has a well-defined purpose:

- [`dotnet/csharplang`](https://github.com/dotnet/csharplang) is for language design and evolution efforts.
- [`dotnet/roslyn`](https://github.com/dotnet/roslyn) is for the implementation of the compilers and related tools.
- [`dotnet/csharpstandard`](https://github.com/dotnet/csharpstandard) is for the creation of the standard text that describes the C# language.

The addition of `dotnet/csharpstandard` to the .NET Foundation means we can direct work to the correct place more easily. You'll see the following changes over the coming months:

- Issues in [`dotnet/csharplang`](https://github.com/dotnet/csharplang/issues?q=is%3Aopen+is%3Aissue+label%3ASpec) and [`dotnet/docs`](https://github.com/dotnet/docs/issues?q=is%3Aopen+is%3Aissue+label%3Acsharp-spec%2Ftech+label%3Adotnet-csharp%2Fprod) for the spec text will move to the new [`dotnet/csharpstandard`](https://github.com/dotnet/csharpstandard/issues) repository.
  - This will take place during the next month or two.
- The [C# spec on docs.microsoft.com](https://docs.microsoft.com/dotnet/csharp/language-reference/language-specification/introduction) will be replaced with the version from the standards committee.
  - This will take place once all [C# 6 pull requests](https://github.com/dotnet/csharpstandard/pulls?q=is%3Apr+is%3Aopen+C%23+6) have been reviewed and merged in the standards repo.
- The [C# 6 draft spec](https://github.com/dotnet/csharplang/tree/main/spec) will be removed from the `dotnet/csharplang` repo.
  - This will take place once the proposed C# 6 draft is published on [docs.microsoft.com](https://docs.microsoft.com).

You can participate by reviewing the PRs, opening issues for changes that aren't covered, and helping refine the language in PRs.

## Thank you

Moving the standards work into a public repository took the cooperation of the members of the C# standards committee, the chair, vice-chair and secretary of [ECMA TG49](https://www.ecma-international.org/technical-committees/tc49/), and the .NET Foundation board. We invite you to participate by identifying issues, reviewing proposed text, and suggesting improvements. We're excited to move this work into the open, and invite all of you along.
