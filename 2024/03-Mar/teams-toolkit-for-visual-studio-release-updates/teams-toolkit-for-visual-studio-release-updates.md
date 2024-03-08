---
post_title: Building AI Bots, Extending Copilot for Microsoft 365, and more with .NET and Teams Toolkit
author1: aycabas
post_slug: teams-toolkit-for-visual-studio-release-updates
username: aycabas
microsoft_alias: aycabas
featured_image: ttk_appTestTool.png
categories: .NET, ASP.NET Core, Blazor
tags: teams toolkit, microsoft teams, copilot
summary: Teams Toolkit for Visual Studio is packed with the new exciting capabilities for .NET developers including new AI Bot templates, CodeLens integration, and new Copilot preview templates.
post_date: 2024-03-07 10:00:00
---

[Teams Toolkit for Visual Studio](https://learn.microsoft.com/microsoftteams/platform/toolkit/toolkit-v4/teams-toolkit-fundamentals-vs) helps .NET developers build, debug, and publish apps for Microsoft Teams. We are thrilled to share that the 17.9 version of Teams Toolkit for Visual Studio 2022 is packed with exciting capabilities for .NET developers such as the new AI Bot template, Teams Bot Test tool, Adaptive Card previewer and more enhancements, bug fixes to improve your development experience. 

Let's explore the new features that this release is offering! 

## Overview of the new features
Teams Toolkit for Visual Studio 17.9 version release includes the new AI Bot template, Teams Bot test tool, Adaptive card previewer, CodeLens support in lifecycle steps and direct access to the documentation.

### The new AI bot template
The new Teams AI Bot template helps developers get started building intelligent chat bots that can process information and provide answers with the power of OpenAI. This template leverages the Teams AI library and AI components, which simplify creating bots that use an OpenAI API key or Azure OpenAI to provide an AI-driven conversational experience.

![Teams Toolkit AI Bot template](ttk_AItemplate.png)

### Teams Bot Test Tool 
Previously developers were required to launch Teams web client with credentials and custom permissions to preview and debug Teams bots. With the new Teams Bot Test Tool, developers can debug, preview and test your Teams Bot app in a simulated Teams chat environment, without logging in to Teams web client. To use Teams Bot Test Tool, select your debug profile as **Teams App Test Tool** to start debugging.

![Teams Bot Test Toolkit](ttk_appTestTool.png)

Learn more about the Teams Bot Test Tool by watching this video: [Debugging your Teams bot with Teams Toolkit](https://youtu.be/IiuAWrZYmoo?si=eWKshLc5-_bZG5je)

### Adaptive Card Previewer 
Teams Toolkit now integrates the Microsoft Adaptive Card Previewer to help developers preview and edit Adaptive Cards in a more intuitive way. To use the Adaptive Card Previewer, select the **Preview** button in the definition file of the Adaptive Card, and preview on the right side.

![Adaptive card previewer](ttk_ACPreviewer.png)

Learn more about the Adaptive card previewer by watching this video: [Create and live preview your adaptive cards for Teams](https://youtu.be/RzI1DINaSzM?si=yN24dR_fKIGIy6sA)

### CodeLens support for the lifecycle steps
In this release, Teams Toolkit configures lifecycle actions and settings in teamsapp.yml file for developers to view and run the lifecycle actions that are already in place using CodeLens of teamsapp.yml file.

![CodeLens in teamsapp.yml](ttk_CodeLens.png)

### Access to the Teams Toolkit documentation 
Finally, as a part of this release, developers can access Teams Toolkit documentation from menu **Project -> Teams Toolkit -> Teams Toolkit Documentation**.

![Teams Toolkit Docs](ttk_docs.png)

## What's new in Teams Toolkit preview?  
In addition to the generally available features, Teams Toolkit also provides several new features in preview such as the new Microsoft Copilot template, AI Assistant bot template and more. To test out the preview features, select **Tools -> Options**, then **Preview Features** in left panel, select the ones you prefer.

### Extend Copilot for Microsoft 365
Teams Toolkit provides a new template in preview for developers who are interested in extending Copilot for Microsoft 365 and bring their data in the search experience. To extend Copilot for Microsoft 365, developers can use the new **Custom Search Results** template in the Teams Toolkit preview version. 

To test out this feature, select **Tools -> Options**, then **Preview Features** in left panel, tick **Teams Toolkit: Develop Copilot Plugin**.

![Teams Toolkit Preview Features](ttk_PreviewFeatures.png)

After enabling the preview features and restarting Visual Studio, create a new project and select **Microsoft Teams App > Custom Search Results**. 

![Copilot app template](ttk_CopilotTemplate.png)

### Connect with the OpenAI Assistants API
Building intelligent chat bots is even simpler now using the AI Assistants Bot project template. Selecting this template creates a new project that uses the Teams AI Library to simplify connecting your Teams bot to the OpenAI Assistants API to build engaging conversational experiences – checkout the video on using custom functions to customize the AI responses with your own data and build your own copilot.

![New AI Assistant Bot](ttk_AssistantTemplate.png)

## Build with .NET 8
We’re happy to share that all the Microsoft Teams App project templates are updated to use .NET 8 by default and new Tab projects are using the new rendering options in Blazor.
We 💖 your feedback! Developers can [share feedback or issues with the Teams Toolkit product team on GitHub](https://github.com/OfficeDev/TeamsFx/issues), or email the product team directly at ttkfeedback@microsoft.com.

## See it in action
Want to see more? Checkout the recent episode of On .NET where I joined James to walk through live demos:

<iframe width="800" height="450" src="https://www.youtube.com/embed/DvqnTunnJkQ?si=QidM9sD2ETfJQX0z" title="YouTube video player" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share" allowfullscreen></iframe>
