# Help us improve Visual Studio tooling for .NET Core

We're currently planning our next set of investments in Visual Studio tooling for .NET Core (and .NET 5) and all other [.NET SDK-style projects](https://docs.microsoft.com/dotnet/core/project-sdk/overview). This tooling, called [a project system](https://github.com/dotnet/project-system), is open source and powers millions of .NET SDK-style projects every single day.

## What is the .NET project system

The .NET project system is a set of components that power nearly everything you do in Visual Studio and .NET. It's the "plumbing" that moves data to and from Solution Explorer, designer tooling, language services and IntelliSense, the debugger, build and publish actions, configuration, and more.

The .NET project system is also responsible for several UI experiences in Visual Studio. Solution Explorer, project property pages, the dependencies node, project files, various menus, and several tooling windows (such as the RESX and settings designers). It's very broad in scope.

The .NET project system is also a significant driver of performance metrics in Visual Studio, such as:

* Time until Solution Explorer is ready for you to interact with
* Time until you get full IntelliSense in C# files
* Build times for your solution
* Behavior of various tooling windows
* Behavior of various parts of Visual Studio when branching with source control
* and others

The team tracks metrics like this with the goal to improve them over time.

## How to help

As mentioned, we're doing some planning on what to tackle next. Quality and performance improvements are already on the table and you can engage with the team [on GitHub](https://github.com/dotnet/project-system). If you have an idea for a new feature or enhancement, feel free to [file an issue](https://github.com/dotnet/project-system/issues/new)!

Additionally, we'd love it if you could take 1-2 minutes and fill out a quick survey. It will help us prioritize certain areas of the .NET project system:

[Take the survey](https://www.surveymonkey.com/r/QW9BNYL)

![Survey illustration](survey-illustration-300x181.png)

Thanks, and happy coding!
