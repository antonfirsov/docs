# Porting to .NET Core

.NET Core is getting closer and closer to an RTM release. Only two months ago,
we announced the [RC of .NET Core and ASP.NET Core](http://bloinggs.msdn.com/b/dotnet/archive/2015/11/18/announcing-net-core-and-asp-net-5-rc.aspx).

As part of our validation, we're working with first party and third party customers to port their code to .NET Core. We received many requests from you asking us how you should go about migrating existing code to .NET Core and how you can continue to target .NET Framework. In this post, I want to provide you with an overview of what porting existing code to .NET Core looks like, what kind of apps are good candidates, what tools we offer, and what we do in general to help you with being more successful with targeting to .NET Core.

## We'd like to talk to you!

If you already started taking advantage of existing assets on .NET Core, I'd like to talk to you about your experience to understand what we can do to help. If you're interested, please contact me at immol at microsoft dot com and I'll arrange a phone call.

***Embedded Tweet***

```html
<blockquote class="twitter-tweet" data-lang="en"><p lang="en" dir="ltr">Who has tried to port apps and/or libraries to .NET Core and would be willing to talk to me about their experiences (good and bad)?</p>&mdash; Immo Landwerth (@terrajobst) <a href="https://twitter.com/terrajobst/status/691740807125037056">January 25, 2016</a></blockquote>
<script async src="//platform.twitter.com/widgets.js" charset="utf-8"></script>
```

## What do you want to port?

Before porting assets to .NET Core you should understand your current assets, their architecture as well as your external dependencies. Think about the value props that .NET Core has to offer and how you want to leverage them. Then you can reverse engineer which assets make sense to port, which assets shouldn't be ported, and which new assets you need to create specifically for .NET Core.

Let's start by looking at what .NET Core has to offer. The RTM version of [.NET Core will have](https://github.com/dotnet/corefx/blob/master/Documentation/project-docs/porting.md#prioritization) the following application models: 

* **ASP.NET Core apps and services**
* **Universal Windows Platform (UWP) apps**
* **Console apps**

Let's take a quick look at each of them to see what porting would mean in their context.

### ASP.NET Core applications and services

**Reasons to port?** The primary reason to migrate your existing ASP.NET app to run cross-platform. For instance, this enables developing  your web site on a Mac running OS X while you deploy your web site to a production Linux machine (we'd love this to be Azure, but it's really your choice). But even if you stay on Windows you may want to take a look at ASP.NET Core because it offers new features and doesn't require a machine wide framework installation, which avoids issues such as requiring machine changes, administrative privileges during deployment, and GAC policies.

**Good porting candidates?** The best foundation for ASP.NET Core are web sites using MVC and/or WebAPI.

**Less ideal for porting?** If the majority of your web application is using WebForms, moving to ASP.NET Core is equivalent to a reimplementation as WebForms isn't supported. However, if you want to use this as an opportunity to refactor your application anyways, this isn't a show stopper as MVC/WebAPI is simply the more modern way to write web applications.

### Universal Windows Applications (UWP)

**Reasons to port?** UWP unifies the Windows device family, ranging from PCs to tablets, phablets, phones, and the Xbox. It now also includes headless internet of things (IoT) devices. UWP provides many great facilities, such as an app store that allows monetizing your apps more easily. And the Windows Runtime (WinRT) offers much more usable yet fully native and powerful operating system APIs, such as XAML and DirectX composition.

**Good porting candidates?** If you're targeting Windows 8 or Windows Phone 8.1, you're already done: these are .NET Core applications. If you maintain Windows Phone Silverlight apps, you're pretty close, so are most Silverlight apps in general.

**Less ideal for porting?** Rich desktop applications that take advantage of Windows Forms or WPF are less ideal because neither technology is supported on .NET Core. However, WinRT offers a native XAML-based UI technology that is very similar to Silverlight and WPF. So if you were planning to redesign your application to be usable across various form factors, this shouldn't be a blocker for you.

### Console Applications

**Reasons to port?** One of the biggest reasons you should look into using .NET Core for console applications is because it allows targeting multiple operating systems (Windows, OS X, and Linux). Another strong reason is .NET Native for console apps as this enables producing self-contained, single file executables with minimal dependencies.

**Good porting candidates?** Pretty much any console application is fair game, depending on your dependencies. For example, if your console application automates Windows or Office using COM, it might be harder to port than, say, an application that parses a CSV file and calls a WCF service. As a data point, the C# and VB compilers are .NET Core console applications.

**Less ideal for porting?** There isn't a canonical example of a less ideal candidate; it's a function of your dependencies.

## Relationship of .NET Core and .NET Framework

Before we go into porting itself, it's useful to understand how .NET Core and .NET Framework relate, especially with respect to available APIs. This helps you in gaining a picture of how APIs are likely going to evolve and in turn enables you to plan the evolution of your applications and libraries.

Many people think of .NET Core as a subset of .NET Framework. It's important to understand that this isn't true. While it's true that .NET Core today is smaller in terms of available APIs, we also have -- and plan to continue to have -- certain APIs and technologies that are .NET Core only. This includes tooling, for instance .NET Native, but also includes libraries.

However, it's equally important to understand that the overwhelming majority of .NET Core APIs are shared with the .NET Framework. That makes it fairly easy to write class libraries that can simultaneously work on .NET Core as well as the .NET Framework. In fact, unless you target .NET Framework 4 or Silverlight all portable class libraries are .NET Core class libraries. If you want to understand how this is modeled and how .NET Core is essentially simply portable class libraries done right, take a look a this [blog post](http://blogs.msdn.com/b/dotnet/archive/2014/12/04/introducing-net-core.aspx).

Of course, the vast majority of existing code is targeting the .NET Framework. Converting an existing .NET Framework class library to .NET Core can be challenging, so let's take a look at the key differences and API gaps that exist between the two.

## Differences between .NET Core and .NET Framework

The differences between the two can be summarized in these three points:

1. **NuGet-based**. .NET Core is distributed as a set of NuGet packages that allow app-local deployments. In contrast, the .NET Framework is always installed in a system-wide location. This difference doesn't matter so much for class libraries; but it matters for applications as those are expected to deploy the closure of their dependencies. But we expect this model to change how quickly class library authors can take advantage of new functionality. Since the applications can simply deploy a new version (as opposed to having to wait until a given .NET Framework version is widely adopted), there is less of a penalty for component authors to take advantage of the latest features.

2. **Well layered** .NET Core was specifically designed to be layered. [The goal](http://blogs.msdn.com/b/dotnet/archive/2014/12/04/introducing-net-core.aspx) was to create a .NET stack that can accommodate a wide variety of capabilities and system constraints without forcing customers to recompile their binaries and/or produce new assets. This means that we had to remove certain APIs because they tied lower level components to higher level components. In those cases, we provide alternatives, often in the form of extension methods.

3. **Free of problematic tech**. .NET Core doesn't include certain technologies we decided to discontinue because we found them to be problematic. If the scenario still makes sense for .NET Core, our plan is to have replacements.

The first point means that we now fully embrace NuGet as a first class concept for the core development experience. We believe this to be a natural progression as many of you already use NuGet to acquire third party dependencies. Of course, there are differences when authoring NuGet packages but that's a topic for another day. 

The second and third point mean that there are certain APIs that aren't available when targeting .NET Core. Let's look at some areas you should be aware of.

### Reflection

With the advent of .NET Native we have a technology that allows us to statically link your application with the framework and third party dependencies. For the linking to be viable, it's important that it can identify the parts of the framework that you're not using. In other technologies, such as C++, this is somewhat straightforward as these systems don't have dynamisms such as reflection. Of course, .NET Native still supports reflection but we wanted to make the platform more pay-for-play friendly, meaning that you don't have to pay for features that you don't use. This is especially true for reflection, as it imposes significant constraints on what the runtime and compilers can do based on static information.

So ideally, reflection should be an optional component that your application might decide not to use at all. The tricky part is that `System.Object` has a dependency on reflection via `Object.GetType()`. In order to break that dependency, we decided that `System.Type` no longer represents the full-blown reflection type information but only the type name. This means that `System.Type` in .NET Core no longer contains APIs such as `GetMembers()`, but continues to expose APIs such as `Name`.

In order order to get access to the additional type information you've to call an extension method called `GetTypeInfo()` that lives in `System.Reflection`. It returns a new type called `TypeInfo` which is what `Type` used to be. In other words code like this:

```C#
var members = obj.GetType().GetMembers();
```

becomes

```C#
using System.Reflection;
...
var members = obj.GetType().GetTypeInfo().GetMembers();
```

As you'll see later, there is tooling to help you with migrating your code.

### Technologies discontinued for .NET Core

The .NET platform is a very mature stack that is almost 15 years old. We've built a large set of technologies into the platform. Over the years, we've learned a lot about them, how they are used, what architectures they create and what limitations they have. As a result, we've identified a set of technologies we no longer promote for authoring modern .NET applications and thus do not bring to .NET Core.

Of course, we haven't removed any technologies from .NET Framework. If you're currently using them, you don't have to change anything. We'll continue to support those in the context of .NET Framework. However, we encourage you to avoid taking dependencies on those for new applications, as this will make it harder for you to port these assets to .NET Core. Also, in those areas we'll generally not add new features, so you're better off using the preferred alternatives.

Let's take a look at some of those. I'll explain why they are problematic and what you should use instead. For more details and the full list of discontinued technologies, read [our porting guide](https://github.com/dotnet/corefx/blob/master/Documentation/project-docs/porting.md#unsupported-technologies).

#### App Domains

**Why was it discontinued?** AppDomains require runtime support and are generally quite expensive. While still implemented by CoreCLR it's not available in .NET Native and we don't plan on adding this capability there.

**What should I use instead?** AppDomains were used for different features; for isolation we recommend processes and/or containers. For dynamic loading of assemblies we recommend the new `AssemblyLoadContext`.

#### Remoting

**Why was it discontinued?** The idea of .NET remoting -- which is transparent remote procedure calls -- has been identified as a problematic architecture. Outside of that realm, it's also used for cross AppDomain communication. On top of that, remoting requires runtime support and is quite heavyweight.

**What should I use instead?** For communication across processes, inter-process communication (IPC) should be used, such as pipes or memory mapped files. Across machines, you should use a network based solution, preferably a low-overhead plain text protocol such as HTTP.

#### Binary serialization

**Why was it discontinued?** After a decade of servicing we've learned that serialization is incredibly complicated and a huge compatibility burden for the types supporting it. Thus, we made the decision that serialization should be a protocol implemented on top of the available public APIs. However, binary serialization requires intimate knowledge of the types as it allows to serialize object graphs, which includes private state.

**What should I use instead?** Choose the serialization technology that fits your goals for formatting and footprint. Popular choices include data contract serialization, XML serialization, JSON.NET, and protobuf-net.

#### Sandboxing

**Why was it discontinued?** Sandboxing, i.e. relying on the runtime or the framework to constrain which resources a managed application can access, is considered a non-goal for .NET Core. We've already stated previously that relying on sandboxing for security reasons isn't an approach we want to support moving forward. It also makes the implementation more complicated and often negatively affects performance of applications that don't use sandboxing.

**What should I use instead?** Use operating system provided security boundaries, such as user accounts for running processes with the least set of privileges.

### Considered for porting

Of course, just because something isn't available in .NET Core today doesn't mean we discontinued it. In most cases, it simply means we haven't had the time investigating whether porting would make sense or didn't think it was relevant to the application models .NET Core currently offers. Thus, this is an area we're highly interested in getting your feedback.

Many of you already [filed issues on GitHub](https://github.com/dotnet/corefx/issues?q=is%3Aopen+is%3Aissue+label%3Aport-to-core), asking for specific components to be ported. We're currently aware of these items:

* **System.Data**. While the base layer is already part of .NET Core, i.e. the provider model and SQL client, some features are currently not available, such as schema support and `DataTable`/`DataSet`.

* **System.DirectoryServices**. There is currently no support in .NET Core to communicate with LDAP or ActiveDirectory.

* **System.Drawing**. While strictly speaking a client API, many developers use the drawing API on servers to provide thumbnail generation or watermarking. We currently don't have support for this in .NET Core.

* **System.Transactions**. While ADO.NET supports transactions there is no support for distributed transactions, which includes the notion of ambient transactions and enlistment.

* **System.Xml.Xsl and System.Xml.Schema**. .NET Core has support for `XmlDocument` as well as Linq's `XDocument`, including XPath. However, currently there is no support for XSD (`XmlSchema`) or XSLT (`XslTransform`).

* **System.Net.Mail**. There is currently no support for sending emails from .NET Core.

* **System.IO.Ports**. .NET Core currently doesn't include the ability to communicate with a serial port.

* **System.Workflow**. The Windows Workflow Foundation (WF) is currently unavailable on .NET Core.

* **System.Xaml**. When creating UWP applications, developers will use the WinRT XAML APIs. Hence, .NET Core currently doesn't include the managed XAML framework, which includes the ability to parse XAML documents and instantiate the described object graph.

For a full list, take a look at [CoreFX issues marked as port-to-core](https://github.com/dotnet/corefx/issues?q=is%3Aopen+is%3Aissue+label%3Aport-to-core). Please note that this list doesn't represent a commitment from us to open source all these components or even bring them to .NET Core -- they are simply capturing the desire from the community to do so. That being said, if you care about any of the components listed above, consider participating in the discussions on GitHub so that your voice can be heard. And if you think something is missing, [file a new issue](http://github.com/dotnet/corefx/issues/new).

Also, we're actively looking into areas that customers find particularly challenging to port. For instance, we recently had several design meetings on minimizing the [differences in reflection](https://github.com/dotnet/apireviews/tree/master/2016-01-19-reflection).

## Understanding how portable your code is

Before you even attempt to port, you should run [API Port](http://dotnetstatus.azurewebsites.net) on your binaries. This will produce a report that provides two useful pieces of information:

* **High-level summary**. The summary gives you a percentage for each of your assemblies, telling you how much of your framework usage is portable to .NET Core. This shows you which of your components are harder to port and which ones are easier.

* **List of non-portable APIs**. It provides a table that lists all the usages of APIs that aren't portable. It also includes a list of recommended changes, calling out the replacements.

Sometimes, the high-level summary can be misleading. Make sure to take a look at the list of non-portable APIs -- sometimes many issues can have the same, fix. For instance, many reflection APIs have moved but the fix is pretty simple by inserting a call to `GetTypeInfo()`.

We highly encourage you to use API Port. Not only is this tool super useful to you, it also helps us to prioritize APIs because it sends us telemetry back. The data we receive is basically just the list of framework APIs you called. This way, we know which APIs are often used by customers that wish to target .NET Core. Our goal is to prioritize those. Don't worry -- we will not collect any information about your code. You don't have to take my word for it: API Port is open source and is [hosted on GitHub](https://github.com/microsoft/dotnet-apiport).

If you want to learn how API Port works you should take a look at this [interview with the API Port team [on Channel 9](https://channel9.msdn.com/Blogs/Seth-Juarez/A-Brief-Look-at-the-NET-Portability-Analyzer):

***Embedded Channel 9 Video***

```html
<iframe src="https://channel9.msdn.com/Blogs/Seth-Juarez/A-Brief-Look-at-the-NET-Portability-Analyzer/player" width="960" height="540" allowFullScreen frameBorder="0"></iframe>
```

##  Porting to .NET Core

Now that you've got a good understanding of the features .NET Core offers and how it differs from the .NET Framework, let's talk about porting.

Before you start porting, you should understand your application architecture and which parts you want to port. There are roughly three approaches you can take:

1. **Co-evolution**. In this case you want to keep your existing .NET Framework assets (such as a desktop application) but you also want to take advantage of the .NET Core app models, such as a UWP for targeting mobile devices. Another common approach is a .NET Framework based desktop application that communicates with an ASP.NET Core based service. In both cases, the goal is to share some functionality between .NET Core and .NET Framework.

2. **Migration**. In this case you've an existing application, for instance an ASP.NET 4 MVC app that you want to move holistically to ASP.NET Core. Thus the goal isn't continuing to target both, .NET Framework and .NET Core but being able to quickly adapt your code so that it can be compiled for .NET Core.

3. **Start from scratch**. In this case you don't really care about existing assets as you're starting an application from scratch for .NET Core. But eventually you most likely have to use samples or incorporate snippets of code that we were meant for the .NET Framework. So you still want to understand how you can tweak code to make it work for .NET Core.

I'll focus on the first and second approach as the third is merely a microscopic combination of the techniques discussed by the others.

Please keep in mind that every code base is different. Thus, porting is different and will highly depend on the state of your code base. So I can't provide you with a recipe that will work for all cases. The approaches and techniques I list below will probably not always work as described. You'll likely have to adapt them to fit your case.

### General approach

Here is a rough approach for porting:

1. Identify the projects that you want to move to .NET Core.

2. Understand the external dependencies these projects have and ensure they are either compatible with .NET Core, have equivalent alternatives, or can be factored out.

3. Change those projects to target .NET Framework 4.6.1. This ensures that you can use API alternatives we've introduced for cases where .NET Core couldn't support existing APIs. Make sure to also upgrade any consuming projects, otherwise you'll get compilation errors due to inconsistent .NET Framework versions.

4. Recompile

5. Run API Port

6. Change your code to address API Port issues

Repeat steps 4 - 6 until all API Port issues are addressed. Then, create the new .NET Core projects and move your code in. Unless there are more issues that API Port couldn't detect, you should be able to compile your code successfully.

### Co-evolving .NET Core and .NET Framework applications

In this scenario you want to share code between an existing .NET Framework application and a to-be-created .NET Core application. There are [two approaches](https://blogs.msdn.microsoft.com/dotnet/2014/04/21/sharing-code-across-platforms/) you can take to share code:

* **Share sources**. Shared projects allow you link source files at the project level: when referencing a shared project all these assets become part of the project they are referenced from. This allows you to use conditional compilation and partial classes to adapt your code to either platform.

* **Share binaries**. You can also factor the code you want to share into a portable class library that can then be referenced from both, .NET Framework as well as the .NET Core application.

Sharing source code is much easier in case you want to adapt your code by using `#if`. However, you want to make sure that you don't create spaghetti code by littering the source with `#if` conditions. Try to centralize code that handles differences as much as possible. Another neat trick is to use partial classes where one part of the class is in the shared project while another part is provided by the platform specific project.

Depending on the size of your team and the application, it might be a better and more sustainable solution to use shared binaries. The reason being that libraries are real building blocks that can be deployed and tested as individual artifacts. Enforcing layering in source is obviously also possible but requires more discipline because it's based on conventions.

To share binaries, you'll create libraries that are fully portable between .NET Framework and .NET Core. To do this, you need to identify the components you want to share. I recommend that you first make sure that components you want to share are not in the same project as components you don't want to share. In other words, a project is either shared entirely or not shared at all. Once that's done, create the portable class libraries and move the code there.

As a rule of thumb I'd say that the sharing binaries is ideal for your business logic and your core layers, while sharing sources is more suitable for components that highly depend on the target platform, i.e. you need to interact with UI components or call operating system specific APIs. It's worth pointing out that the intersection of .NET Core and .NET Framework is pretty large. In fact, almost all of the APIs .NET Core provides are also available in .NET Framework so the second approach isn't as limiting as it may sound.

For more details on how these two approaches compare, take a look at this [blog post](https://blogs.msdn.microsoft.com/dotnet/2014/04/21/sharing-code-across-platforms/).

### Migrating code to .NET Core

To make migration easier, I suggest you target the .NET Framework as long as possible during your migration, use API Port to identify porting issues, and only make the big switch when most are addressed. This way, you don't have the problem of having to deal with a code base that doesn't build for a long time. 

When planning your migration you should also think about your tests and treat them as a part of your product when porting. Migration often result in huge code churns so you want to make sure hat you didn't introduce bugs along the way.

The area that needs the most work is probably going to be application model, be that a desktop application or ASP.NET:

* For desktop applications, it might be worthwhile to use the co-evolution approach because it avoids not having any interim versions of your application.

* For ASP.NET, a good approach would be to compile the application as a .NET Framework application, run API Port and ignore all the issues reported to MVC/WebAPI first. Once all the other issues are fixed, replace ASP.NET 4 with ASP.NET Core and fix up the remaining issues, which hopefully should mostly be fixing up namespaces. Then, replace the .NET Framework ASP.NET application with an ASP.NET Core application.

But if your app is small, it might best to use the big hammer approach: simply create the new project, copy & paste the existing code and fix up all the compiler errors.

## Want more details?

For more details, take a look at these resources:

**.NET Core Progress**

* [Announcing .NET Core RC](http://blogs.msdn.com/b/dotnet/archive/2015/11/18/announcing-net-core-and-asp-net-5-rc.aspx)
* [Road to RTM for CoreFX](https://github.com/dotnet/corefx/blob/master/Documentation/project-docs/rtm.md)
* [Which API/Technologies do we port .NET Core?](https://github.com/dotnet/corefx/blob/master/Documentation/project-docs/porting.md)
* [Community requests for technologies being ported](https://github.com/dotnet/corefx/issues?q=is%3Aopen+is%3Aissue+label%3Aport-to-core)

**API Port**

* [Channel 9: A brief look at .NET Portability Analyzer (API Port)](https://channel9.msdn.com/Blogs/Seth-Juarez/A-Brief-Look-at-the-NET-Portability-Analyzer)
* [Download API Port](http://dotnetstatus.azurewebsites.net)
* [API Port on GitHub](https://github.com/microsoft/dotnet-apiport)

## Summary

In this post, I outlined the benefits that .NET Core brings and which apps and components are most likely benefiting from it. We also looked at ways you can leverage .NET Core from existing applications, by either adding a .NET Core based experience to your portfolio or by migrating your assets completely on top of .NET Core. I showed you how you can use [API Port](http://dotnetstatus.azurewebsites.net) that can help you to understand your components and how portable they are.

If you have any questions or concerns, let us know by leaving a comment. If you've experiences with porting code to .NET Core and would like to talk about them, I'd appreciate if you'd drop me a line at immol at microsoft dot com. I'd love to learn what we can do to improve your porting experience!

Happy porting!