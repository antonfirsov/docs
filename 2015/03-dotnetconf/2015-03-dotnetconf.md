# .NET Confererence

## The .NET Voyage into Open Source

Format: Interview with questions vs. presentational? Email Hanselman

* Why open source?
    - Community
    - Cross-plat
* What we open sourced
    - .NET Core
    - ASP.NET
    - Roslyn
* The processes
    - We *do* accept changes!
    - The PR flow & process
    - Issue management
    - API Reviews
    - Breaking changes
    - CI System (AppVeyor now self-hosted Jenkins)
    - Mature vs. Agile
* Relationship to other .NET Platforms
    - Mono
    - .NET Framework
    - .NET Native / Universal Windows Apps
* Contributions
    - How many contributions?
    - What kind of contributions?
    - Examples
* Demos
    - Clone, Build & run tests (library)
    - Clone, Build & run hello world on Mac (runtime)

## .NET Core Deep Dive

* How is .NET Core different
    - Why do we need another .NET platform?
    - CLR: Smaller version of the .NET runtime (some heavyweight features are
      missing, e.g. app domains, remoting, security, ...)
    - Framework: factoring of asssemblies, pay-for-play, NuGet
    - Both optimized for XCOPY style deployment

* What are we doing for cross-plat?
    - Runtime: libunwinder, dtrace-like
    - Framework: PALs
    - CI System: validation for cross-platform, test coverage

* Whiteboarding
* Showing some PR (viewers can follow along)