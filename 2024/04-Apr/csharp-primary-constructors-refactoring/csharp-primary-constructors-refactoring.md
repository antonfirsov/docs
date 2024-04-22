---
post_title: Refactor your C# code with primary constructors
author1: dapine@microsoft.com
post_slug: csharp-primary-constructors-refactoring
microsoft_alias: dapine
featured_image: csharp-primary-constructors-refactoring.png
categories: .NET, C#
tags: .net 8, c# 12
ai_note: hide
summary: Explore C# 12's primary constructors through incremental refactoring of a Worker service.
post_date: 2024-04-23 10:05:00
---

[C# 12 as part of .NET 8][csharp-12] introduced a compelling set of new features! In this post, we explore one of these features, specifically _primary constructors_, explaining its usage and relevance. We'll then demonstrate a sample refactoring to show how it can be applied in your code, discussing the benefits and potential pitfalls. This will help you understand the impact of the change and help influence your adoption of the feature.

## Primary Constructors 1️⃣

Primary constructors are considered an "Everyday C#" developer feature. They allow you to define a `class` or `struct` along with its constructor in a single concise declaration. This can help you reduce the amount of boilerplate code you need to write. If you've been following along with C# versions, you're likely familiar with `record` types, which included the first examples of primary constructors.

### Differentiating from `record` types

[Record types][records] were introduced as a type modifier of `class` or `struct` that simplifies syntax for building simple classes like data containers. Records can include a primary constructor. This constructor not only generates a backing field but also exposes a public property for each parameter. Unlike traditional `class` or `struct` types, where primary constructor parameters are accessible throughout the class definition, records are designed to be transparent data containers. They inherently support value-based equality, aligning with their intended role as data holders. Consequently, it's logical for their primary constructor parameters to be accessible as properties.

### Refactoring example✨

[.NET provides many templates][dotnet-new], and if you've ever created a [Worker Service][worker-service-docs], you've likely seen the following `Worker` class template code:

```csharp
namespace Example.Worker.Service
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
```

The preceding code is a simple `Worker` service that logs a message every second. Currently, the `Worker` class has a constructor that requires an `ILogger<Worker>` instance as a parameter and assigns it to a `readonly` field of the same type. This type information is in two places, in the definition of the constructor, but also on the field itself. This is a common pattern in C# code, but it can be simplified with primary constructors.

It's worth mentioning that the refactoring tooling for this specific feature isn't available in Visual Studio Code, but you can still refactor to primary constructors manually. To refactor this code using primary constructors in Visual Studio, you can use the `Use primary constructor (and remove fields)` refactoring option. Right-click on the `Worker` constructor, select `Quick Actions and Refactorings...` (or press <kbd>Ctrl</kbd> + <kbd>.</kbd>), and choose `Use primary constructor (and remove fields)`.

Consider the following video demonstrating _Use primary constructor_ refactoring functionality:

<!-- markdownlint-disable no-inline-html -->
<video autoplay="" loop="" class="responsive-video" poster="./refactor-primary-ctor-thumb.png">
   <source src="./refactor-primary-ctor.mp4" type="video/mp4">
</video>
<!-- markdownlint-enable no-inline-html -->

The resulting code now resembles the following C# code:

```csharp
namespace Example.Worker.Service
{
    public class Worker(ILogger<Worker> logger) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (logger.IsEnabled(LogLevel.Information))
                {
                    logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
```

That's it, you've successfully refactored the `Worker` class to use a primary constructor! The `ILogger<Worker>` field has been removed, and the constructor has been replaced with a primary constructor. This makes the code more concise and easier to read. The `logger` instance is now available throughout the class (as it's in scope), without the need for a separate field declaration.

## Additional considerations 🤔

Primary constructors can remove your hand-written field declarations that were assigned in the constructor, but with a caveat. They're not entirely functionally equivalent if you have defined your fields as `readonly` because primary constructor parameters for non-record types are mutable. So, when you're using this refactoring approach, be aware that you're changing the semantics of your code. If you want to maintain the `readonly` behavior, use a field declaration in place and assign the field using the primary constructor parameter:

```csharp
namespace Example.Worker.Service;

public class Worker(ILogger<Worker> logger) : BackgroundService
{
    private readonly ILogger<Worker> _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }
            await Task.Delay(1000, stoppingToken);
        }
    }
}
```

## Additional constructors 🆕

When you define a primary constructor, you can still define additional constructors. These constructors are required, however; to call the primary constructor. Calling the primary constructor ensures that the primary constructor parameters are initialized everywhere in the class declaration. If you need to define additional constructors, you must call the primary constructor using the `this` keyword.

```csharp
namespace Example.Worker.Service
{
    // Primary constructor
    public class Worker(ILogger<Worker> logger) : BackgroundService
    {
        private readonly int _delayDuration = 1_000;

        // Secondary constructor, calling the primary constructor
        public Worker(ILogger<Worker> logger, int delayDuration) : this(logger)
        {
            _delayDuration = delayDuration;
        }

        // Omitted for brevity...
    }
}
```

Additional constructors aren't always needed. Let's do some bonus refactoring to include a few other features!

## Bonus refactoring 🎉

Primary constructors are awesome, but there's more we can do to improve the code.

C# includes [file-scoped namespaces][namespaces]. They're a really nice feature that reduces a level of nesting and improves readability. Continuing with the previous example, place your cursor at the end of the namespace name, and press the <kbd>;</kbd> key (this isn't supported in Visual Studio Code, but again you can do this manually). This will convert the namespace to a file-scoped namespace.

Consider the following video demonstrating this functionality:

<!-- markdownlint-disable no-inline-html -->
<video autoplay="" loop="" class="responsive-video" poster="./refactor-file-scope-ns-thumb.png">
   <source src="./refactor-file-scope-ns.mp4" type="video/mp4">
</video>
<!-- markdownlint-enable no-inline-html -->

With a few additional edits, the final refactored code is as follows:

```csharp
namespace Example.Worker.Service;

public sealed class Worker(ILogger<Worker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }

            await Task.Delay(1_000, stoppingToken);
        }
    }
}
```

In addition to refactoring to file-scoped namespaces, I also added the `sealed` modifier, as there's a performance benefit in multiple situations. Finally, I've also updated the numeric literal passed into the `Task.Delay` using the [digit separator][digit-separator] feature, to improve the readability. Did you know there's a lot more to simplify your code? Check out [What's new in C#][csharp-what-is-new] to learn more!

## Next steps 🚀

Try this out in your own code! Look for opportunities to refactor your code to use primary constructors and see how it can simplify your codebase. If you're using Visual Studio, check out the refactoring tooling. If you're using Visual Studio Code, you can still refactor manually. To learn more, explore the following resources:

- [Primary constructors in C# 12][primary-ctors]
- [Visual Studio: Additional Quick Actions][quick-actions]
- [Visual Studio Code: C# Quick Actions and Refactorings][vscode-quick-actions]

[csharp-12]: https://learn.microsoft.com/dotnet/csharp/whats-new/csharp-12
[csharp-what-is-new]: https://learn.microsoft.com/dotnet/csharp/whats-new
[digit-separator]: https://learn.microsoft.com/dotnet/csharp/language-reference/builtin-types/integral-numeric-types#integer-literals
[dotnet-new]: https://learn.microsoft.com/dotnet/core/tools/dotnet-new
[namespaces]: https://learn.microsoft.com/dotnet/csharp/language-reference/keywords/namespace
[primary-ctors]: https://learn.microsoft.com/dotnet/csharp/whats-new/tutorials/primary-constructors
[quick-actions]: https://learn.microsoft.com/visualstudio/ide/quick-actions?view=vs-2022
[records]: https://learn.microsoft.com/dotnet/csharp/fundamentals/types/records
[vscode-quick-actions]: https://code.visualstudio.com/docs/csharp/refactoring
[worker-service-docs]: https://learn.microsoft.com/dotnet/core/extensions/workers
