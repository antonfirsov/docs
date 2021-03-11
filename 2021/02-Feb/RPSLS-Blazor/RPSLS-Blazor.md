---
post_title: 'The path to .NET 5 and Blazor WebAssembly with some fun sprinkled in'
username: islevin
microsoft_alias: islevin
featured_image: RPSLS-Title.png
categories: .NET, .NET Core, ASP.NET, Azure, C#
summary: Rock, Paper, Scissors, Lizard, Spock was recently upgraded to .NET 5 and uses Blazor WebAssembly. Here is a summary of our experience completing the upgrade.
desired_publication_date: '2021-03-08'
---

With the recent release of .NET 5 at .NET Conf 2020, users saw first-hand how to take advantage of the latest web technology using .NET. One extremely interesting announcement was the new release of Blazor WebAssembly. [Blazor](https://blazor.net) lets you build interactive web UI wuth C# instead of JavaScript. Blazor WebAssembly allows users to build a completely isolated application entirely in C# that can run in nearly every web environment, including environments that only support static sites (think only HTML, CSS and Javascript). Blazor WebAssembly does this my compiling all C# code needed to run your application (your code and .NET libraries) into native code that will execute on the browser, not a server somewhere. This is valuable in scenarios when your app needs to run offline, or completely decoupled from a server, and your app only needs a server if it requests data outside of the application (similar to how many Javascript-based applications work). Due to the fact that the application runs in the browser, render times of the UI are near instantaneous, allowing for a great end-user experience. To see these benefits for ourselves, we decided to port a heavily used application to .NET 5 and Blazor WebAssembly and not only reap these benefits, but document the process one would take moving their existing .NET Core application using Blazor Server to .NET 5 using Blazor WebAssembly. [Read more about the differences between Blazor Server and Blazor WebAssembly](https://docs.microsoft.com/aspnet/core/blazor/?view=aspnetcore-5.0#blazor-webassembly). 

At Ignite 2019, we unveiled Rock, Paper, Scissors, Lizard, Spock (RPSLS), a web game based on the [game created by Sam Kass and Karen Bryla](https://en.wikipedia.org/wiki/Rock_paper_scissors#Additional_weapons)
that ups the difficulty of the very well-known game Rock, Paper, Scissors. When we built RPSLS, the goal was to show that services built with any language can run in Azure at scale. When launched at Ignite, over 2,000 unique people played RPSLS in the first week, most multiple times. You can [find the code of RPSLS on GitHub](https://github.com/microsoft/RockPaperScissorsLizardSpock/).

## So, what is RPSLS exactly? 

RPSLS is a microservice architecture solution built inside of Azure Kubernetes Service. In the game, you can play against a friend using a unique URL, or you can play against a bot. What is a bot? 
A bot is a “mascot” representation of 5 of the many languages you can build apps with inside Azure. The ones we picked were: C#, Java, NodeJs, Python and PHP. 
When you play against a bot, you are actually playing against a microservice running in that particular language. For instance, when you choose C# (my favorite language) as an opponent, there is a service running in .NET that predicts my next move. Once the service "guesses my move", it attempts to pick a move to beat me. That prediction runs in .NET. Along with the [.NET bot](https://mod-dotnet-bot.net) microservice running in .NET, the web UI also runs in .NET, .NET Core 3.1 Blazor Server to be exact. 

### Updating the App to Use the Latest Bits  

With the recent release of the unified .NET 5 with Blazor, it made sense for RPSLS to be upgraded as well. This isn’t extremely challenging given 
the hard work the .NET team put into providing a clear upgrade path. Knowing upgrading the bot service would be easy, we turned to the 
front-end and thought it would be interesting to not just upgrade to .NET 5, but also port the app from Blazor Server to Blazor WebAssembly. Going through this upgrade was extremely interesting, as it was fairly easy. In essence, here are the things that we accomplished as part of the migration:

1. A new project was created using the Blazor WebAssembly hosted template. This creates 3 projects; a `Client` project for the UI, a backend `Server` project for the UI to call, and a `Shared` project which will hold shared models and services.

2. Move the file content from the Blazor Server `_Host.cshtml` page content to the `index.html` in the static folder of the newly created project and replace the existing `_framework/blazor.server.js` with the new `_framework/blazor.client.js` reference in that file. The newly referenced .js file will be the new entry point of the SPA.

3. Migrate all .razor files in the old project to the new Client project

4. Using the Options pattern in .NET 5 in the Client project to retrieve any configuration settings from the Server project using modern retrieval methods (browser fetch API)

5. Configure the Client project to use JSRuntime to initialize Google Analytics with the Api key fetched from the backend (the Blazor Analytics NuGet package allowed us to do it out of the box)

6. Implement a custom AuthenticationState provider that checks if the user is logged in before calling the server (the user can either log with Twitter, or provide a string that represents their username

7. gRPC calls made from Blazor components to the Game API are now executed from Client project and, as the game API is not exposed to internet, we created a game a factory service to forward the requests to the backend Server API.

### Our Experience

This is an oversimplification of the steps needed to complete the migration, but it was not very complicated. At the same time, it was not trivial, 
but as mentioned, the hard work of the .NET team to make these upgrades easier was on display. For reference, [the code changes for the WebAssembly migration is available as a PR on GitHub](https://github.com/microsoft/RockPaperScissorsLizardSpock/pull/35).

## Summary

Overall, the experience was great! Developing multiple layers of the app all in the same language made it feel like you aren’t 
building a client side app, but since the Client project gets compiled down to native WebAssembly, you are. Building a true client-side application, 
and still get support for Dependency Injection, the Options Pattern and Http/gRPC clients in the same way as the backend makes the migration very straightforward. 
One of the key points is that our application doesn't require persistence, so we were able to migrate to Blazor WebAssembly without big changes in the components. 
We want to thank the .NET team for making .NET 5 and Blazor more accessible for existing applications on previous versions of the platform when it comes to migrating. 
We encourage everyone to check out the [GitHub repo](https://github.com/microsoft/RockPaperScissorsLizardSpock) and see what we did to get to the latest version of the app, and of course play the game, for no other reason 
other than it is fun.
