---
post_title: Announcing Polyglot Notebooks! Multi-language notebooks in Visual Studio Code
author1: clregio@microsoft.com
post_slug: announcing-polyglot-notebooks-harness-the-power-of-multilanguage-notebooks-in-visual-studio-code
username: clregio@microsoft.com
microsoft_alias: clregio
featured_image: polyglot_nb_ext.png
categories: .NET, .NET Core, C#, F#, Visual Studio Code
tags: notebooks, vs code, .net interactive, polyglot notebooks
summary: Polyglot Notebooks in VS Code is now generally available! Come try it out!
desired_publication_date: 2023-03-15
post_date: 2023-03-15 10:05:00
---

We are excited to announce that Polyglot Notebooks, Visual Studio Code's multi-language notebook extension, is now generally available in the [VS Code Marketplace](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.dotnet-interactive-vscode)! 

_Please note: While the Polyglot Notebooks extension in Visual Studio Code is now generally available, the .NET Interactive APIs that power it are still in preview._

## What are Notebooks? 

If you’re not familiar with notebooks – stop here! A notebook is an interactive programming and computational file that supports mixing executable code, visualizations, equations, and narrative text. Contrary to scripts that need to be run in their entirety, notebooks have code cells that allow code to be run in an incremental and segmented manner. Popularized by the open-source [Project Jupyter](https://jupyter.org/), you may have heard of these referred to as ‘Jupyter Notebooks’. The component responsible for running code in a notebook is a kernel, and traditionally, Jupyter Notebooks have been most commonly used with Python kernels. Their ability to quickly iterate on code and create visualizations with narrative text have led them to become the de facto tool for data science and great tools for teaching or learning a new programming language, and quick prototyping. 


![Classic Jupyter Notebook](https://user-images.githubusercontent.com/19276747/222538271-8c111f3d-6e5d-453e-966f-6ef8b02a956e.png)


## What are Polyglot Notebooks? 

Polyglot notebooks elevate notebooks to a whole new level. Polyglot Notebooks’ philosophy is that developers should always be able to choose the best language for the task at hand and today using multiple languages in a traditional notebook is quite cumbersome. You may have a few options through magic commands and wrapper libraries, but both fall victim to a poor editing experience as they lack syntax highlighting, autocompletion and signature help. With Polyglot Notebooks, not only can you use multiple languages natively within the same notebook with full language server support, but you can also share variables between them to maintain a continuous workflow. No need to bounce around from tool to tool and manually transfer data between tools to continue your work. 

_*It is important to note that Polyglot Notebooks in VS Code are powered by .NET Interactive, which is an innovative engine built using .NET technology that can run multiple languages and share variables between them. Since it is capable of behaving as a kernel in the context of notebooks, .NET Interactive lights up the Polyglot Notebooks experience._

The Polyglot Notebooks extension in VS Code currently supports the following languages with more to come: 
 - C# 
 - F#
 - PowerShell
 - JavaScript
 - HTML* 
 - Mermaid*
 - SQL
 - KQL (Kusto Query Language) 

*Variable sharing not available  

For example, developers using Polyglot Notebooks in VS Code today can connect to and query a Microsoft SQL Server database, share the tabular result to JavaScript, and create visualizations all within the same tool and the same notebook. 

## Polyglot Notebook Features

### _Connect to and query Microsoft SQL Server Databases and Kusto Clusters_

Polyglot Notebooks currently has support for connecting to and querying Microsoft SQL Server Databases and Kusto Clusters. After connecting, natively write your SQL or KQL (Kusto Query Language) code to run queries. 

![Connecting to MS SQL Server in Polyglot Notebooks](https://user-images.githubusercontent.com/19276747/222537939-3fc6ff93-ba57-403e-a000-6a2f23df6681.png)

### _Language Server support for all languages_

Language server support such as autocompletion, syntax highlighting, and signature help for all languages. Say goodbye to using wrapper libraries when wanting to use multiple languages in the same notebook, and say hello to natively writing code in your preferred language with a first-class editing experience. 

### _Variable Sharing_

Retire the archaic method of jumping from tool to tool and manually transferring data between them when completing one part of your workflow. When you’re done working with one language in Polyglot Notebooks, simply share any relevant variables to the next language and enjoy your continuous workflow in the same file. In the example below, raw SQL is being used to query a Microsoft SQL Server database, the tabular result is shared to JavaScript and HTML in order to create a custom interactive visualization.  

## Getting Started

To get started you will need the following: 

1. [Visual Studio Code](https://code.visualstudio.com/)
2. [.NET 7 SDK](https://dotnet.microsoft.com/download/dotnet/7.0)
3. [Polyglot Notebooks Extension](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.dotnet-interactive-vscode)

Create your first notebook by opening the command palette (Ctrl+Shift+P in Windows, Cmd+Shift+P on iOS) and selecting “Polyglot Notebook: Create new blank notebook”, select ‘.ipynb’, and select the language you’d like to start with. You should see “.NET Interactive” in the top right as this indicates the kernel being used. To change the language of a cell being used, simply click on the language picker in the bottom right of the cell and choose your desired language. Last but not least, get coding! 

![Polyglot Notebook](https://user-images.githubusercontent.com/19276747/222540791-a054da73-a111-454f-9e93-251d620a0c2d.png)



## Feedback

We are excited for you to try out Polyglot Notebooks! If you would like to provide any feedback, file any bugs, or request any features, please file an issue on the [.NET Interactive GitHub repository](https://github.com/dotnet/interactive/issues). To learn more, visit our [Polyglot Notebook Documentation](https://github.com/dotnet/interactive/tree/main/docs). 
