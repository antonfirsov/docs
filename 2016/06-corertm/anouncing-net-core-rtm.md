Announcing .NET Core 1.0
========================

We are extremely proud to announce the release of .NET Core 1.0 and ASP.NET Core 1.0 today! .NET Core is a cross-platform, open source, and modular .NET platform for creating modern web apps, microservices, libraries and console applications. This release includes the .NET Core runtime, libraries and tools and the ASP.NET Core libraries. We are also releasing Visual Studio and Visual Studio Code extensions that enable you to create .NET Core projects. Get started at [https://dot.net/core](https://dot.net/core).

Today we are at the [Red Hat DevNation](http://www.devnation.org) conference showing off the release and our partnership with Red Hat. [Watch the live stream via Channel 9](https://aka.ms/redhatdotnet) where Scott Hanselman will demonstrate .NET Core 1.0. .NET Core is now available on Red Hat Enterprise Linux and OpenShift via certified containers. In addition, .NET Core is fully supported by Red Hat and extended via the integrated hybrid support partnership between Microsoft and Red Hat. See the Red Hat Blog for more details.

We'd also like to express our gratitude for everyone that has tried .NET Core and ASP.NET Core before today and given us feedback. We've received a lot of feedback about design choices, user experience, performance, community and other topics. We've tried our best to apply all of that feedback. The release is much better for it. Thanks!

A little bit of content on the 1.0 journey. Make reference to customers going live on RC1 an RC2 and how that feedback has been quite useful and "thanks!".

Make reference to the ASP.NET Core 1.0 release and usage. And talk about the performance work.

Getting .NET Core and ASP.NET Core
==================================

It's really easy to try out .NET Core and ASP.NET on Windows, macOS or Linux. You can have an app up and running in a few minutes. The you need to the .NET Core SDK to get started.

The best place to start is the [.NET Core](https://dot.net/core) home page. It will offer you the correct download for the Operating System (OS) that you are using and the 3-4 steps you need to following to get started. It's pretty straightforward.

more text here ...


Community Contribution
======================

This is a huge accomplishment for the entire ecosystem – with more than 18,000 developers representing more than 1,300 companies contributing to .NET Core 1.0. Nearly half of all pull requests for the central .NET Core projects (corefx & coreclr) came from outside of Microsoft - up from 20% one year ago, which felt like a milestone to us at the time. The momentum has been incredible. 

Customers have already been using preview versions of .NET Core in production to drive tremendous business impact. We thank you for the feedback and contributions that have been quite useful to get us to our 1.0 release. Illyriad Games, the team behind Age of Ascent, reported a [10-fold increase in performance](http://web.ageofascent.com/video-microsoft-cloud-age-ascent-browser-game-handle-50000-players/) using ASP.NET Core with Azure Service Fabric. We are also extreemely greatful for their code contributions to this performance. 

NetEase, a leading IT company in China that provides online services for content, gaming, social media, communications and commerce, needed to stay on the leading edge of the ever-evolving mobile games space and chose .NET Core for their back end services. When comparing to their previous Java back-end architecture, *“.NET core has reduced our release cycle by 20% and cost on engineering resource by 30%.”* When speaking about the throughput improvements and cost savings, *“Additionally, it has made it possible to reduce the number of VMs needed in production by half.”*

While benchmarks from TechEmpower are still pending, ASP.NET Core has proven over 8x faster than Node.js and almost 3x faster than Go [in our labs.](https://github.com/aspnet/benchmarks)

Increased interest in .NET Core has also driven deeper engagement in the .NET Foundation, which now manages more than 60 projects. In April, Red Hat, Jet Brains and Unity were welcomed to the .NET Foundation Technical Steering Group. Today we are announcing a new member, Samsung. 

*".NET is a great technology that dramatically boosts developer productivity. Samsung has been contributing to .NET Core on GitHub – especially in the area of ARM support – and we are looking forward to contributing further to the .NET open source community. Samsung is glad to join the .NET Foundation's Technical Steering Group and help more developers enjoy the benefits of .NET."* Hong-Seok Kim, Vice President, Samsung Electronics

See the [.NET Foundation blog](https://www.dotnetfoundation.org/blog) for more details. 

Using .NET Core 1.0
===================

Show the experience you get with .NET Core 1.0 via the CLI.

Using Visual Studio Code
========================

Show the experience using Visual Studio Code.

To get started with .NET Core on Visual Studio Code, make sure you have downloaded and installed:

* [.NET Core](http://dot.net/core)
* [Visual Studio Code](https://code.visualstudio.com)

You can verify that you have the latest version of .NET Core installed by opening a command prompt and typing `dotnet --version`.  Your output should look like this:

![](vscode-dotnet-version.png)

Next, you can create a new folder, scaffold a new "Hello World" C# application inside of it with the command line via `dotnet new`, then open Visual Studio Code in that directory with the `code .` command.  If you don't have `code` on your PATH, you'll have to set it.

If you don't have [C# language plugin for Visual Studio Code](https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp) it installed already, you'll want to do that.

Next, you'll need to create and configure the `launch.json` and `tasks.json` files.  Visual Studio Code will have asked if it can create these files for you.  If you didn't allow it to do that, you will have to create these files yourself.  Here's how:

1. Create a new folder at the root level called `.vscode` and create the `launch.json` and `tasks.json` files inside of it.

2. Open `launch.json` and configure it like this:

<script src="https://gist.github.com/cartermp/b2fea9ea7e0d2379e5a4d4a1bb0406cb.js"></script>

3. Open the `tasks.json` file and configure it like this:

<script src="https://gist.github.com/cartermp/2db7148c6618c758b59e34343741f0f1.js"></script>

4. Navigate to the Debug menu, click the Play icon, and now you can run your .NET Core applications!

IMAGE OR GIF HERE

Note that if you open Visual Studio code from a different directory, you may need to change the values of `cwd` and `program` in `launch.json` and `tasks.json` to point to your application output folders.

You can also debug your application by setting a breakpoint in the code and clicking the Play icon.

IMAGE OR GIF HERE

Using Visual Studio
===================

Show the experience using Visual Studio.

Roadmap
=======

We've shipped 1.0 and we've got a plan for future releases. Make reference to the blog posts. Point to the new .NET Core project.

Closing
=======

Thanks for all the feedback and usage.
