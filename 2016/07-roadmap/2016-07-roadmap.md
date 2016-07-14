# .NET Core Roadmap

_This post was written by **Scott Hunter**._

It has been about two weeks since we shipped .NET Core / ASP.NET Core 1.0. The
team has used the past two weeks to take a deep breath, and is now getting
started on planning what is coming next. We have seen a lot of .NET Core SDK
downloads and significant feedback. Please keep the feedback coming.

Here is a rough timeline of how things might look going forward. Note that these
are the targeted dates that the team are currently working towards but may
change.

## 1.0.1 (~August 2016)

We are actively monitoring the 1.0 release for issues to include in a first
patch (1.0.1) release of the .NET Core SDK. There is no scheduled date for this
patch update but early August is likely. Here is a list of the top issues we are
investigating:

* Performance improvements in `dotnet build` which will improve ASP.NET Core
  publishing times.
* Updates to the `dotnet new` templates for F# so they build without changes.
* Miscellaneous fixes to the tools based on crash telemetry.

## Q4 2016 / Q1 2017

This will be the first minor update, mainly focused on replacing
.xproj/project.json with .csproj/MSBuild. Project format update should be
automatic. Just opening a 1.0 project will update it to the new project format.
There will also be new functionality and improvements in the runtime and
libraries.

As context, .NET Core 1.0 included a preview version of the .NET Core Tools,
referred to as "Preview 2". The tools were "preview" primary because we knew
that we would change the tools experience post 1.0. .NET Core and the .NET Core
Tools will both be "RTM quality" or "stable" with this release.

### .NET Core Tooling

* Support for .csproj/MSBuild project system
* `dotnet restore` improvements to not restore packages that are part of .NET Core
* New commands for managing the frameworks on the machine
* `dotnet publish` will publish only required dependencies, for optimal distribution size

### Languages (available for .NET Framework and .NET Core)

The next releases for the .NET languages will apply to all .NET platforms.
There's a lot of information out there about the features included in these
releases but here's a short summary:

* Bring functional programming concepts to .NET languages
    - Tuples
    - Pattern matching
* Performance and Code Quality
    - Value Tasks
    - Ref returns
    - Throw expressions
    - Binary literals
    - Digit separators
* Developer Productivity
    - Out vars
    - Local functions

These features will be all available in C# 7. VB 15 will also implement all the
features that impact language interop (tuples, ref returns, etc) but some
features will be available in the next language update (e.g. pattern matching)
or are not in the roadmap (e.g. local functions).

In addition to C# and VB we'll also release a new version for the F# language.
F# 4.1 will include things like:

* Full .NET Core support
* Better IDE experience with workspace support on the F# language service
* New language features such as ValueTuple interop, more support for annotating
  types as structs, better "low-level" programming support and more.

### ASP.NET Core

* Web Sockets
* URL Rewriting Middleware
* Azure
    - App Service startup time improvements
    - App Service Logging Provider
    - Azure Key Vault Provider
    - Azure AD B2C Support
* Containers and Microservices
    - Service Fabric support via WebListener based server
    - MVC & DI Startup Time Improvements
* Previews
    - SignalR
    - View Pages (Views without MVC Controllers)

### .NET Core Runtime

* ARM 32/64
* More Linux distributions (build from source)

### Entity Framework Core

* Azure
    - Transient fault handling (resiliency)
* Mapping
    - Custom type conversions
    - Complex types (value objects)
    - Entity entry APIs
* Update pipeline
    - CUD stored procedures
    - Better batching (TVPs)
    - Ambient transactions
* Query
    - Stability, performance.
* Migrations
    - Seed data
    - Stability
* Reverse engineer
    - Pluralization
    - VS item template (UX)

## Q1 2017 / Q2 2017

.NET Core will bring back many of the missing APIs in .NET Core, including
networking, serialization, data and more. These APIs are part of .NET Standard
2.0, which will be released at the same time, resulting in APIs being consistent
across .NET Framework, .NET Core and Xamarin. It will be much easier to write
portable code that can run on all the major .NET platforms, targeting .NET
Standard 2.0. Expect a preview of this work to start showing up after we ship
the Q4/Q1 release.

### Better Communication

Moving forward we want to be more transparent in what the team is doing. To do
this we are planning on updating this blog on a more frequently with updates on
the team. A rough list of upcoming topics is:

* .NET Core Roadmap (this blog post)
* ASP.NET Upcoming Highlights
* Entity Framework Upcoming Highlights
* .NET CLI Upcoming Highlights
* Support and Versioning .NET Core
* Telemetry in .NET Core
* .NET Standard
* API's Returning
* Project Conversion from project.json to .csproj

Next week we hope to show some of the first examples of what the conversion to
.csproj/MSBuild will look like and a deeper dive of the new functionality in one
of (ASP.NET, EF or .NET CLI).

Let us know what your think!
