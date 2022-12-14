---
post_title: Polyglot Notebooks - Now with .NET 7, C# 11, and F# 7 Support!
author1: clregio@microsoft.com
post_slug: polyglot-notebooks--december-2022-release
username: clregio@microsoft.com
microsoft_alias: clregio
featured_image: polyglot-nb-ext.png
categories: .NET, .NET Core, C#, F#, Visual Studio Code
tags: notebooks, vs code, .net interactive, polyglot notebooks
summary: The December 2022 release of the Polyglot Notebooks extension for Visual Studio Code is now available. This release includes upgrades to .NET 7, support for C# 11 and F# 7, and improvements to the SQL and KQL kernel experience! 
desired_publication_date: 2022-12-12
post_date: 2022-12-12 10:05:00
---

We are pleased to announce the December 2022 improvements to Polyglot Notebooks for Visual Studio Code. Make sure to install the [Polyglot Notebooks extension](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.dotnet-interactive-vscode) from the VS Code Marketplace, or install it directly from the extension gallery in Visual Studio Code. _If you haven’t read about our latest announcement regarding the difference between .NET Interactive and Polyglot Notebooks, we recommend you [read this first](https://devblogs.microsoft.com/dotnet/dotnet-interactive-notebooks-is-now-polyglot-notebooks/)._

## Upgrade to .NET 7

Polyglot Notebooks has recently upgraded to depend on .NET 7. The extension now requires the .NET 7 SDK to work so [update today](https://dotnet.microsoft.com/download/dotnet/7.0)! To learn more about .NET 7, [read our announcement](https://devblogs.microsoft.com/dotnet/announcing-dotnet-7/)!  

## Support for C# 11 and F# 7

As part of the upgrade to .NET 7, Polyglot Notebooks now has support for C# 11 and F# 7. To see all the exciting new features you can use in Polyglot Notebooks, review the following resources: 
 - [Welcome to C# 11](https://devblogs.microsoft.com/dotnet/welcome-to-csharp-11/)
 - [What's new in C# 11](https://learn.microsoft.com/dotnet/csharp/whats-new/csharp-11)
 - [Announcing F# 7](https://devblogs.microsoft.com/dotnet/announcing-fsharp-7/)

## Improvements to SQL and KQL with the new dynamic kernel picker  

When establishing a connection with a Microsoft SQL Server or Kusto Cluster, you need to alias the connection using the `–kernel-name` parameter in the `#!connect` command. In order to then declare which database connection you want to query against, you previously would have had to add a magic command using the alias at the beginning of each cell.

Before the dynamic kernel picker: 

![image](https://user-images.githubusercontent.com/19276747/204938317-3432b45a-9621-420b-ab72-8dfc2910538e.png)

With the dynamic kernel picker, you can now declare which database connection you are querying in the bottom right of the cell instead of using a magic command. Simply add a new code cell after establishing your connection, click on the dynamic kernel picker, and select the alias you created for your connection. Then write your native SQL or Kusto code! 

With the dynamic kernel picker: 

![image](https://user-images.githubusercontent.com/19276747/204938475-276a9c1a-0f0c-4326-8d95-7725e6e32750.png)

## Summary

Be sure to [download the Polyglot Notebooks extension](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.dotnet-interactive-vscode) for Visual Studio Code now to try out the above improvements. If you run into any problems or have suggestions, [please file an issue](https://github.com/dotnet/interactive/issues/new/choose) on the [.NET Interactive GitHub](https://github.com/dotnet/interactive) page. 
