# Making .NET Core easier to port to

In my last post, I [talked about porting to .NET Core][porting-to-core] and
requested feedback from our community on what their experience was and what we
could improve.

This sparked many great conversations with our users.

Based on these conversations as well our experience working with first- and
third-party partners, we've decided to drastically simplify the porting effort
by unifying the core APIs with other .NET platforms, specifically the .NET
Framework and Mono/Xamarin.

In this blog post, I'll talk about our plans, how and when this work will
happen, and what this means for existing .NET Core customers.

## Reflecting on .NET Core

The [.NET Core platform][net-core] evolved from a desire to create a modern,
modular, and app-local .NET stack. The business goals that drove its creation
were focused on providing a stack for brand new application types (such as
touch-based UWP apps) or modern cross-platform applications (such as ASP.NET
Core web sites and services).

We are about to ship .NET Core 1.0 and we have succeeded in creating a powerful
and cross platform development stack. .NET Core 1.0 is the beginning of a
journey to get .NET everywhere.

While .NET Core works well for the scenarios that we set out to address, it
provides access to fewer technologies than other .NET platforms, especially the
.NET Framework. Partly because not everything has been made cross platform, and
partly because we aggressively trimmed some features out of it.

It became clear that adopting .NET core would require existing .NET developers
to spend a considerable amount of time to port to it.

While there is certainly some value in presenting new customers with a cleaner
API, it disproportionately penalized our existing loyal customers who have
invested over many years in using the APIs and technologies we advertised to
them. We want to extend the reach of the .NET platform and gain new customers,
but we can’t do so at the expense of existing users.

Xamarin is a great role model in this regard. They allow .NET developers to
build mobile applications for the iOS and Android platforms with minimal effort.
Consider iOS. It shares many of the characteristics of the UWP platform, such
as the high-focus on end-user experience and the requirement of static
compilation. However, in contrast to .NET Core, Xamarin did not start with
reimagining the .NET stack. They took Mono as-is, removed the application models
components (Windows.Forms, ASP.NET), added a new one for iOS, and made minimal
changes to make it suitable embedded use. Since Mono is virtually identical to
the .NET Framework, the resulting API set is fairly comprehensive and makes
porting existing code to Xamarin substantially easier.

Since its inception the key promise of .NET has been to make developers more
productive and help them write robust code. It was designed from the start to
support developers on a wide range of areas and scenario, starting with desktop
and web applications to microservices, mobile applications and gaming.

For us to deliver on our promise, it is critical that we provide a unified core
API that is available everywhere. A unified core API allows developers to
easily share code across these workloads and allows them to focus their skills
where it matters the most - creating great services and user experiences.

## .NET Core moving forward

At Build 2016, [Scott Hunter presented the following slide][build-talk]:

![](NetStandard.png)

Here is the promise we want to make to you:

*Whether you need to build a desktop application, a mobile app, a web site, or a*
*micro service: you can rely on .NET to get you there. Code sharing is as easy*
*as possible because we provide a unified BCL. As a developer, you can focus on*
*the features and technologies that are specific to the user experiences and*
*platforms you're targeting.*

This is how we want to realize this promise: we will provide the source and
binary compatibility for application that target the core Base Class Libraries
(BCL) across all platforms with the same behavior across platforms. The Base
Class Libraries are those that existed in `mscorlib`, `System`, `System.Core`,
`System.Data`, `System.Xml` and that are not tied to a particular application
model and are not tied to a particular operating system implementation.

Whether you target .NET Core 1.0 surface (`System.Runtime`-based surface), or
the upcoming version of .NET Core with the expanded API (`mscorlib`-based
surface), your existing code will continue to work.

The promise of making it easier to bring existing code extends to libraries and
NuGet packages. Obviously this includes portable class libraries, regardless of
whether they used `mscorlib` or `System.Runtime`.

It is worth pointing out that we look at the convergence from the perspective of
a Mono/Xamarin developer, i.e. we focus on APIs that Mono has implemented that
we don’t have in .NET Core yet. The set of APIs in the .NET Framework is much
larger; we’re looking at Mono because it provides a starting point for what our
ecosystem needs outside of the Windows desktop scenarios. Of course, we can add
even more APIs later.

Here are a few examples of the additions that will make your life easier when
targeting to .NET Core:

* Reflection will become the same as the .NET Framework, no need for `GetTypeInfo()`,
  good old `.GetType()` is back.
* Types will no loner miss members we've removed for clean up reasons (`Clone()`,
  `Close()` vs `Dispose()`, old APM APIs)

A full list of the planned additions will be made available in our [corefx]
GitHub repo.

## What does this mean for .NET Core?

From talking to our community on social media it seems there is a concern that
these API additions degrade the .NET Core experience. Nothing could be further
from the truth. The vast majority of investments we made for .NET Core, be it
that it can be deployed in an app-local fashion, XCOPY deployment, that we have
an ahead-of-time (AOT) compiler tool chain, that it's open source and cross-
platform remain unchanged. The same is true for all the additional features and
performance improvements we made, such as the new networking component called
Kestrel.

Originally, when we designed .NET Core we've talked heavily about modularization
and pay for play, meaning you only have to consume the disk space for the
features that you end up using. We believe we can still realize these goals
without compromising so heavily on compatibility.

Initially, our plan to achieve minimum disk space usage relied on a manual
process of splitting the functionality in tiny libraries and we know that our
users liked this. We will now provide a linking tool that will be more precise
and provide better savings than any manual process could have provided. This is
similar to what Xamarin developers get today.

## Timelines and process

The process to extend the API surface of .NET Core will come after we ship .NET
Core 1.0 RTM. This way, those of you that have been following along .NET Core
will be able to deploy to production.

You can expect to see more details and plans over the next couple of weeks
published in our [corefx] GitHub repository. One of first thing we will do is to
publish a set of API refs that list which APIs we're planning to bring. So when
porting code you will be able to tell whether you want to jump to .NET Core 1.0
or wait for the new APIs to come. We'll also call out which APIs we don't plan
on bringing. Our desire is to provide a dashboard for our users to check on
the project status and goals.

This will be an improvement over the process that we followed in the lead up to
.NET Core 1.0, as we did not share enough of our internal processes with the
world.

Lastly, we're planning on releasing incremental updates to .NET Core on NuGet
that extends the set of available APIs. This way, you will not have to wait
until all the API additions are done in order to take advantage of them. This
also allows us to incorporate your feedback on behavioral compatibility.

Over the next weeks we publish more details in the [corefx] repo. You can expect
this blog to communicate the status and all major decisions.

Stay tuned for more details!

[porting-to-core]: https://blogs.msdn.microsoft.com/dotnet/2016/02/10/porting-to-net-core/
[net-core]: https://blogs.msdn.microsoft.com/dotnet/2014/12/04/introducing-net-core/
[build-talk]: https://channel9.msdn.com/Events/Build/2016/B891
[corefx]: https://github.com/dotnet/corefx


