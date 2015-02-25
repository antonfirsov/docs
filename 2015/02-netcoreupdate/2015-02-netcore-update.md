.NET Core Open Source Update
============================

It has been a couple weeks since we last reported on the .NET Core open source project. It's been a very fun time, watching more people get involved in the project and to see progress on a daily basis. It's amazing watching my GitHub news feed. I have to scroll through several page views just to get through the last hour (on a weekday) of [corefx](https://github.com/dotnet/corefx), [coreclr](https://github.com/dotnet/coreclr) and [roslyn](https://github.com/dotnet/roslyn) repo activity. Today, we're going to focus on the CoreCLR repo.

In addition to the growing community activity, there have been important product improvements that have been committed. Some of the key changes are coming from the community. That's very impressive. Wow and Thanks.

Work-in-progress
================

We published the CoreCLR source earlier this month, but did not immediately transition our work-in-progress feature work to be open and viewable. We received community encouragement to adopt a more transparent approach, and are following through on that.

There are two parts to that:

- Publish issues, as both self-assign and _up-for-grabs_.
- Do work on a publicly visible branch (either coreclr or a fork).

Here are a few of the issues that we published using both the self-assign and up-for-grabs patterns. The act of publishing issues enables transparent ownership of issues, which is beneficial for everyone. 

- [Implement calling convention for structs passing in JIT for Linux and Mac](https://github.com/dotnet/coreclr/issues/200)
- [Implement a plugin for loading CoreCLR debugger extensions into LLDB](https://github.com/dotnet/coreclr/issues/202)
- [Port assembly code for JIT_MemSet and JIT_MemCpy to Linux and Mac](https://github.com/dotnet/coreclr/issues/199)

Doing feature work in the open means that anyone can:

- Watch changes as they are made, to understand and give feedback on the approach for a given feature, _before_ it is submitted as a PR.
- Collaborate as a group on the actual feature work.

Here's a [good example](https://github.com/dotnet/coreclr/pull/282) of one of the first features that was developed in the open. 

![Implement native stack unwinding for Linux](unix-issue177-branch.png)

You can see that this feature was developed in a central branch on the coreclr repo. We will make use of both central branches and personal forks for feature work. We choose the branch location based on the level of discoverability and engagement that makes sense for a given feature. 

Mac OS X Support
================

One of the biggest additions since we published the CoreCLR repo is the initial implementation of Mac OS X support. The internal Microsoft team has been focussed on Linux support, making Mac OS X support a great community-led project, in order to bring it up in parallel with Linux. 

[@kangaroo](https://github.com/kangaroo) has been leading the charge on Mac OS X. Thanks! Check out these two PRs if you want to look at the changes: [Mac OS X Support](https://github.com/dotnet/coreclr/pull/105) and [Initial Mac OS X Support](https://github.com/dotnet/coreclr/pull/117). There have been lots of changes since then.

![@kangaroo PR #117](kangaroo-pull-117.png)

You can see from the community response to the initial support announcement that there are a lot of folks that would like to see .NET Core on Mac OS X. Me, too!

![@kangaroo initial Mac support](kangaroo-init-mac-support.png)

[@praeclarum](https://twitter.com/praeclarum/status/564846894837272576) wrote a great set of instructions, [Building and Running .NET’s CoreCLR on OS X](http://praeclarum.org/post/110552954728/building-and-running-nets-coreclr-on-os-x), to help you follow along at home. Check it out.

![@praeclarum Tweet](praeclarum-blog-tweet.png)

You'll quickly see that the Mac OS X experience is still pretty raw. We'll publish an official set of instructions when the experience is a bit further along. For now, please do refer to @praeclarum's instructions.

Linux Support
=============

There is also a lot of great work going into Linux. [@mikem8361](https://github.com/mikem8361) is working on [getting SOS working on Linux](https://github.com/dotnet/coreclr/pull/304). I already mentioned the [stack unwinding feature](https://github.com/dotnet/coreclr/pull/282), based on libunwind. 

Like Mac OS X support, it is still "early days" in the project. That said, we have an enterprising community that likes to try things out. We'll publish official Linux instructions when we've got a good experience in place.

@zerosignal00 was one of the first to try out [.NET Core on Linux](https://twitter.com/zerosignal00/status/565323160477007874). 

![.NET Core on Linux](netcore-on-linux.png)

Gitter Chat
===========

We received a PR - [Add a Gitter chat badge to README.md](https://github.com/dotnet/coreclr/pull/153/files) - to publicize a Gitter room created by [@migueldeicaza](https://github.com/migueldeicaza). We've seen a lot of people using it. 

We have two Gitter rooms now. Please drop in and join the crowd.

- [CoreFX](https://gitter.im/dotnet/corefx)
- [CoreCLR](https://gitter.im/dotnet/coreclr)

Gitter is an impressive product. I like using it. It's the integration with GitHub (obviously) that makes it a pleasure to use. That said, if someone likes a different chat room service, we're happy to publicize it, provided you can create an active community around it.

dotnet.github.io
================

Microsoft created the [.NET Foundation](http://www.dotnetfoundation.org) last year. We contributed .NET Core and other .NET components, like Roslyn, ASP.NET and Orleans, to it. Several other companies and individuals have contributed components. 

We've been spending a lot of time on GitHub, since most of the Microsoft projects are now there. There is a trend of hosting org.github.io pages to make it easier to discover repos. The [Netflix](http://netflix.github.io) and [Twitter](http://twitter.github.io) are great examples. We shamelessly borrowed some of their ideas.

Check out the [dotnet.github.io](http://dotnet.github.io) page we built for the .NET Foundation. It helps you discover [.NET Foundation project](http://www.dotnetfoundation.org/projects) repos. Repos are ordered in terms of  [_OSS Awesomeness_](http://dotnet.github.io/about.html), which is a concept we borrowed from Twitter.

Community Thanks!
=================

We were impressed to see the Roslyn team [recognize recent committers](https://twitter.com/khyperia/status/569992447708819456) to the Roslyn repo. We like to refer to these as sha-cups. That's quite classy.

![Roslyn sha-cup](roslyn-sha-cup.png)

We haven't gotten to that level of awesome community appreciation just yet. That said, we're equally appreciative for the level of support that we're seeing on a daily basis, almost 24 hours a day. Thanks to everyone who has contributed to the .NET Core project. If you haven't yet, I encourage you to get involved.