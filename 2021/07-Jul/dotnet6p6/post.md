---
post_title: Announcing .NET 6 Preview 6
username: rlander@microsoft.com
microsoft_alias: rlander@microsoft.com
categories: .NET Core, .NET
desired_publication_date: 07/14/2021
summary: .NET 6 Preview 6 is now available.
---

We are happy to release .NET 6 Preview 6. Preview 6 is the second to last preview before we enter the RC period. There will be two RCs. This release itself is relatively small, while Preview 7 will be bigger. After that, we're down to quality fixes until the final release in November. We're looking forward to a great new .NET release.

You can [download .NET 6 Preview 6](https://dotnet.microsoft.com/download/dotnet/6.0) for Linux, macOS, and Windows.

* [Installers and binaries](https://dotnet.microsoft.com/download/dotnet/6.0)
* [Container images](https://hub.docker.com/_/microsoft-dotnet)
* [Linux packages](https://github.com/dotnet/core/blob/main/release-notes/6.0/install-linux.md)
* [Release notes](https://github.com/dotnet/core/blob/main/release-notes/6.0/README.md)
* [API diff](https://github.com/dotnet/core/tree/main/release-notes/6.0/preview/api-diff/preview6)
* [Known issues](https://github.com/dotnet/core/blob/main/release-notes/6.0/known-issues.md)
* [GitHub issue tracker](https://github.com/dotnet/core/issues/6141)

See the [ASP.NET Core](https://devblogs.microsoft.com/aspnet/) and [EF Core](https://devblogs.microsoft.com/dotnet) posts for more detail on what’s new for web and data access scenarios.

Starting with .NET 6 Preview 6, we've tested and support [Visual Studio 2022 Preview 2](https://visualstudio.microsoft.com/vs/preview/vs2022/). Visual Studio 2022 enables you to leverage the Visual Studio tools developed for .NET 6 such as development in .NET MAUI, Hot Reload for C# apps, new Web Live Preview for WebForms, and other performance improvements in your IDE experience. .NET 6 has also been tested with [Visual Studio for Mac 8.9](https://visualstudio.microsoft.com/vs/mac/).

Check out the new [conversations posts](https://devblogs.microsoft.com/dotnet/category/conversations/) for in-depth engineer-to-engineer discussions of the latest .NET features.

## x64 emulation update

We're done adding support for Apple Silicon for macOS and Arm64 for Windows. What's left is [supporting x64 emulation](https://github.com/dotnet/designs/pull/217). We need to do two things to enable that.

* [Side-by-side capable installers](https://github.com/dotnet/designs/pull/217)
* [First-class architecture targeting with the .NET CLI](https://github.com/dotnet/designs/pull/233) to (primarily) enable using the native architecture SDK in all scenarios.

Until side-by-side capable installers are available (later in .NET 6), you'll either need to install all x64 builds or all Arm64 ones. If you want to switch, you need to uninstall/delete all .NET versions on your Arm64 machine. That's unfortunate, but where we are at at this point.

## Tools: .NET SDK Optional Workload improvements

We have added three new [workload](https://github.com/dotnet/designs/blob/main/accepted/2020/workloads/workloads.md) commands that enable better discovery and management.
 
* `dotnet workload search` -- list workloads available to install.
* `dotnet workload uninstall` -- remove the specified workload if you no longer require a workload. Also a good option for space saving.
* `dotnet workload repair` -- re-install all workloads you've previously installed.
  * This is useful if your install fails in the middle because of a dropped internet connection.
  * Optional workloads are made up of multiple workload packs and you may have gotten into a state where some installed successfully but others didn't.

In previous previews, we added the following commands:

* `dotnet workload install` -- installs a workload.
* `dotnet workload list` -- lists installed workloads.
* `dotnet workload update` -- updates installed workloads.

## Libraries: TLS support for `System.DirectoryServices.Protocols`

[TLS support has been enabled](https://github.com/dotnet/runtime/pull/52904) for [`System.DirectoryServices.Protocols`](https://docs.microsoft.com/dotnet/api/system.directoryservices.protocols) for Linux and macOS. It was already enabled for Windows. .NET users can now enjoy secure communications with LDAP servers.

Credit to [@iinuwa](https://github.com/iinuwa).

## Tools: Crossgen2 replaces crossgen

[Crossgen2](https://devblogs.microsoft.com/dotnet/conversation-about-crossgen2/) has been enabled for all existing crossgen scenarios. With that milestone behind us, we've also removed (the old) crossgen from the SDK. It is no longer accessible and cannot be used.

Crossgen (1 and 2) enables pre-compiling IL to native code as a publishing step. Pre-compilation is primarily beneficial for improving startup. Crossgen2 is a from-scratch implementation that is already proving to be a superior platform for code generation innovation. For example, crossgen2 can generate code for a broader set of IL patterns than crossgen1.

The following MSBuild properties demonstrate how to enable pre-compilation with crossgen2.

```xml
      <!-- Enable pre-compiling native code (in ready-to-run format) with crossgen2 -->
      <PublishReadyToRun>true</PublishReadyToRun> 
      <!-- Enable generating a composite R2R image -->
      <PublishReadyToRunComposite>true</PublishReadyToRunComposite>
```

## Libraries: Improved sync-over-async performance

[Sync-over-async](https://devblogs.microsoft.com/pfxteam/should-i-expose-synchronous-wrappers-for-asynchronous-methods/) is a common type of blocking work. It can lead to starvation when it happens on thread pool worker threads. Slow thread injection may delay other queued work from running, and may delay the starvation from being resolved.

This [change](https://github.com/dotnet/runtime/pull/53471) improved the rate of thread injection by default when sync-over-async is the only type of blocking work happening on thread pool worker threads. There are some [new `AppContext` config values](https://github.com/dotnet/runtime/blob/a7a2fd6543ff71cecbbfe901b81ee27a6cf428c0/src/libraries/System.Private.CoreLib/src/System/Threading/PortableThreadPool.Blocking.cs#L271-L314) that can be used to configure the rate of thread injection in response to sync-over-async.

## Runtime: W^X memory policy

We are [enabling support](https://github.com/dotnet/runtime/issues/50391) for [W^X memory protection](https://github.com/dotnet/designs/blob/main/accepted/2021/runtime-security-mitigations.md#wx). It is a requirement on Apple Silicon machines and a useful security measure on other operating systems.

This feature has an abnormal name. It should be read as "write exclusive execute". That means that a memory page can be marked for read/write or read/execute but never any combination that includes write and execute. Write/execute pages are subject to being exploited with buffer overrun attacks, for example.

This feature requires changes throughout the product, wherever write/execute pages are used. For example, preview 6 includes a change that requires the [JIT to cooperate with the W^X scheme we've adopted](https://github.com/dotnet/runtime/pull/53173).

W^X is a requirement of macOS on Apple Silicon machines, optional in all other environments for .NET 6 and will likely be the default mode in all environments for .NET 7.

Note: The W^X implementation has a startup regression with .NET 6 on all environments but Apple Silicon. It will be resolved as part of .NET 7. The Apple Silicon implementation has no such regression due to [operating system support for this scenario](https://developer.apple.com/documentation/apple-silicon/porting-just-in-time-compilers-to-apple-silicon).

## CodeGen changelog

The following codegen changes are included in Preview 6.

### Dynamic PGO https://github.com/dotnet/runtime/issues/43618

- Add option to choose guarded devirt class randomly https://github.com/dotnet/runtime/pull/53399 
- pgo/devirt diagnostic improvements https://github.com/dotnet/runtime/pull/53247

### LSRA

- Refactor [LSRA](https://github.com/dotnet/runtime/blob/main/docs/design/coreclr/jit/lsra-detail.md) heuristics selection https://github.com/dotnet/runtime/pull/52832 
  - Now, DEBUG mode includes a COMPlus variable that LsraOrdering will let the user set the heuristics ordering.  
 - Tune the heuristics for register to select optimal register candidate to spill. https://github.com/dotnet/runtime/pull/53853

The following improvements are based on these changes.

![image](https://user-images.githubusercontent.com/63486087/124963432-a6d57580-dfd4-11eb-9549-e7d5a8231448.png)
![image](https://user-images.githubusercontent.com/63486087/124963446-ac32c000-dfd4-11eb-9960-69832b72f7b2.png)
![image](https://user-images.githubusercontent.com/63486087/124963455-b05edd80-dfd4-11eb-82a0-06de64fcc3f9.png)

More improvements:

* [Windows x86](https://github.com/DrewScoggins/performance-2/issues/6612), 
* [Windows x64](https://github.com/DrewScoggins/performance-2/issues/6592) 
* [General](https://github.com/DrewScoggins/performance-2/issues/6593).

### Code quality

- Eliminate redundant “test” instruction https://github.com/dotnet/runtime/pull/53214 

## Closing

The release is quickly coming together and to an end. We'll soon be addressing only the most pressing feedback, approaching the same bug bar that we use for servicing releases. If you've been holding on to some feedback or have yet to try .NET 6, please do now. It's your last chance to influence the release.

Thanks for everyone who has contributed to making .NET 6 another great release.

Thanks for being a .NET developer.
