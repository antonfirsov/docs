---
post_title: 'Sharing code between ASP.NET and ASP.NET Core'
username: keschlob
microsoft_alias: keschlob
featured_image: images/mvc3MusicStore.png
categories: .NET, .NET Core
summary: You can begin your journey to ASP.NET Core by migrating one page at a time and do it by reusing the code you already have today. This article shows how to share controllers, models, and views from the 10-year-old MVC Music Store with a new ASP.NET Core project.
desired_publication_date: '2022-03-02'
---

With the release of .NET 6 there are even more benefits to using ASP.NET Core. But migrating existing code to ASP.NET Core often sounds like a big investment. Today we'll share how you can accelerate the migration to ASP.NET Core. There are minor changes you can make today that can make it easier to migrate to ASP.NET Core tomorrow.

Before we begin let's talk about a real scenario. Let's talk about how to modify code in a 10-year old application so that it can be shared with ASP.NET Core. In the next sections we'll give samples that migrate the `ShoppingCartController.cs` from the MVC Music Store app that was used to demo ASP.NET MVC3.

The first step to migrating this web app is to create a new **ASP.NET Core Web App (Model-View-Controller)** project. This template will add support for Controllers and map the default route for Controllers in the `Program.cs` file. Once we have the new project setup we'll remove the default `HomeController` and the view files for `Home/Index` and `Home/Privacy` so we can share content from the MVC3 Music Store web app without conflict.

## You can share Controllers

The first thing you can share between the two projects are Controllers. Many teams want the new website to work the same as the current one. And when we say "the same" we mean "the same". If you fix a bug in one project then you need that same fix to show up in both sites. One of the easiest ways to assure this behavior is to share the same file in both projects. Luckily ASP.NET Core uses the new SDK style project files. That means it's really easy to open the csproj file and add some changes because the files are very readable.

To start sharing a Controller class you'll need to create an `<ItemGroup>` and add a reference to the existing class. Here's a sample that shows how to share the `ShoppingCartController.cs` by updating the csproj file of the ASP.NET Core project.

```xml
    <ItemGroup>
        <Compile Include="..\MvcMusicStore\Controllers\ShoppingCartController.cs" LinkBase="Controllers" />
    </ItemGroup>
```
Okay, now the file is included in the project but you may have guessed that the ASP.NET Core project doesn't compile anymore. In ASP.NET Core the `Controller` class doesn't use **System.Web.Mvc** it uses **Microsoft.AspNetCore.Mvc**.

Here's a sample that shows how the `ShoppingCartController.cs` can use both namespaces to fix that compiler error.

```cs
#if NET
using Microsoft.AspNetCore.Mvc;
#else
using System.Web.Mvc;
#endif
```

There are other places in the `ShoppingCartController` that would need to be updated but the approach is the same. Using [C# preprocessor directives](https://aka.ms/AAf5yjv) we can make the class flexible enough to compile for both projects.

> For scenarios with large sections of code that work differently for ASP.NET Core you may want to create implementation specific files. A good approach is to create a partial class and extract those code blocks to new method(s) that are different between the two web app targets and use the csproj to control which files are included when building the project.

## You can share Models
Now that we can share Controllers we'll want to share the Models they return. In many scenarios this will just start working when we include them by adding another `<ItemGroup>` to the csproj file. But if your models also reference `System.Web` then we can use the same approach we just used for Controllers. Start by updating the namespaces so that the same class file can exist in both projects. Keep using the C# precompiler directives to add ASP.NET Core support.

Here's a sample that shows how to modify the `[Bind]` attribute.

```cs
#if !NET
    [Bind(Exclude="OrderId")]
#endif
    public partial class Order
    {
        [ScaffoldColumn(false)]
#if NET
    [BindNever]
#endif
        publicintOrderId{ get; set; }
   …
   …
```

## You can share Views
We can even share views. Using the same approach again we can edit the csproj file to share files like the `_Layout.cshtml`. And, inside the view you can keep using C# precompiler directives to make the file flexible enough to be used by both projects.

Here's what it looks like to have a master page with mixed support for Child Actions from ASP.NET and View Components from ASP.NET Core so that we can render the part of the page that knows how many items are in the shopping cart.

```cs
@{
    #if NET
		<text>@awaitComponent.InvokeAsync("CartSummary")</text>
	#else
		@Html.RenderAction("CartSummary", "ShoppingCart");
    #endif
}
```

## Wrapping up
The ability to share code also includes static content like CSS, JavaScript and images. Step-by-step you can build flexibility into your web app today to make your migration to ASP.NET Core easier.

If you would like more detailed guidance to migrate the entire `ShoppingCartController.cs` you can follow a full walkthrough with samples at [MvcMusicStoreMigration](https://aka.ms/AAf5wkc). The walkthrough will also demonstrate how you can run both ASP.NET and ASP.NET Core from the same IIS Application Pool to incrementally migrate your web app one controller at a time.

For those planning to start work on their ASP.NET Core migration we'll share a few more tips.

* Upgrade your NuGet packages so you can use netstandard.
* Change your class libraries to netstandard so you can share code between ASP.NET and ASP.NET Core.
* Find references to `System.Web` in your class libraries build interfaces replace them. Use dependency injection so you can easily switch between ASP.NET and ASP.NET Core features.

You can also find more guidance from our docs at [Migrate from ASP.NET to ASP.NET Core](https://docs.microsoft.com/aspnet/core/migration/proper-to-2x/).
