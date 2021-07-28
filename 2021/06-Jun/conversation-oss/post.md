---
post_title: Conversation about the .NET open source project.
username: rlander@microsoft.com
microsoft_alias: rlander@microsoft.com
categories: .NET
desired_publication_date: 07/28/2021
summary: Conversation with .NET engineers about the .NET open source project, five+ years in.
---

Open source was an attractive and exciting idea when we first considered .NET open source. At the same time, GitHub was a largely unknown platform for many of us and we had a lot of questions about how everything would work. "What if someone forks the runtime on the first day? Is the project over?". We knew enough that we should take the time to learn the patterns that developers had already established and also what they expected of us. That said, we also had to host a project and org from day one, and it got busier much quicker than we expected. We're several years and versions in at this point, and the rest is now history. It's been a fun journey that we'll reflect on with folks that have been part of it in a variety of ways.

We’re using the [conversation format](https://devblogs.microsoft.com/dotnet/category/conversations/) again, this time with collection of .NET engineers who want to share their experiences working on GitHub and establishing .NET as an open source project.

* [Claire Novotny](https://github.com/clairernovotny)
* [Dan Moseley](https://github.com/danmoseley)
* [Immo Landswerth](https://github.com/terrajobst)
* [Kevin Pilch](https://github.com/Pilchie)
* [Matt Mitchell](https://github.com/mmitche)
* [Stephen Toub](https://github.com/stephentoub)

## Why is open source important for the .NET project?

**Immo:** A modern developer stack needs to be cross platform. Open source is the most sustainable way to build a stack that has very wide support, over an ever changing landscape of operating systems and architectures. It also allows us to engage with consumers in real time, which has altered the way we plan, build, and iterate on .NET in more ways than I can describe here. And lastly, it's now an expectation that fundamental technologies like developer stacks are available under an open source license.

**Claire:** Open Source enables anyone to view, debug, and contribute to the runtime that they use to build their application. They can address pain points that are important to them that might not be otherwise prioritized. Open Source helps ensure that the .NET project is available beyond a single vendor, Microsoft.

**Kevin:** I think Open Source is important for .NET for a few reasons:

* It is pretty common for language and runtime implementations to be Open Source, so we'd be conspicuous if we weren't.
* We have a huge surface area, and despite our best attempts to write documentations, it's really useful for people to be able to see and debug the implementation.
* It opens us up to interesting collaborations from both individuals and other companies in a manner that is a lot more straightforward than trying to do one-off agreements on a closed source system.

**Stephen:** A multitude of reasons, but closest to my heart is the ability for anyone anywhere to find something important to them and improve it. In the times prior to .NET Core, requests for improvements would have to find their way to the right people at Microsoft, be colated and triaged against other requests, be scheduled for a developer to handle improving, and so on, and eventually it might find its way into a release in the next couple of years. Now, someone sees something they want to fix, they put up a PR, it's reviewed, iterated on, merged, and it can be in a nightly build the next day. It's a whole different world.

**Dan:** Open Source is the best way to be successful cross platform - it's natural for targeting Linux.

**Matt:** I think it's important for Microsoft to show that it's invested in the OSS community in a meaningful way. We use OSS software as we develop, ship OSS software as part of our products, etc. OSS is a huge part of the entire software ecosystem these days. It's important for Microsoft to be part of that. To give back.

**Dan:** Because we get more eyes on what we're doing earlier, it helps us make sure we ship the right thing - before we ship. Also, the size of our community means there's experts in many domains - more than we can have on our engineering team - and that helps us do better work as well.

## The .NET team works directly on GitHub, right? There were some code bombs (the JIT and the GC) in the early days. There's none of that now, right?

**Immo:** Only when Stephen Toub has a long layover.

**Matt:** We are almost entirely on GitHub. We do initially make security patches in internal repositories, but this code is merged into the open on release day. Otherwise we develop, discuss, and track issues almost entirely on GitHub.

**Stephen:** It's much more rare these days. Every now and then there's some additional existing component that's brought into one of the dotnet/* repos, but it's very infrequent. The only one I can think of from the past year was the System.Speech library.

**Kevin:** Actually, I think I have one of the largest code-bombs as I was the account that owned the first commit in the
https://github.com/dotnet/rolsyn project (back in the days before we thought to use bots for things like that). That said, these days it's true that the vast majority of our work happens directly on GitHub, and you can see it incrementally appear. But I wouldn't rule out the possibility if we either decide to release the source for something other existing thing, or if we work on a prototype before deciding whether it makes sense to be Open Source or not.

**Dan:** We care a lot about working in the open - as well as being a commitment, it also gets more eyes on what we're doing. So generally, it's an anti-pattern for us to do significant non-public development.

## Share a fun story about the original .NET Core 1.0 open source project.

**Kevin:** I was working on Roslyn at the time .NET Core 1.0 was being developed, but one thing that we were pretty happy about was that after Anders announced the compiler was released on CodePlex at the time, we were in a chat with their ops folks watching load increase. Luckily it stayed up, but IIRC it was the highest load they'd seen to date.

**Immo:** For the dotnet org we created a dedicated user account (dotnet bot) that, among other things, we use as the author of the initial commits. We did this to make it clear that these commits represent existing code and wasn't written by any specific individual. As you can imagine, initial commits tend have a lot of lines code. To this day, dotnet bot receives a lot of job offers by people who mine GitHub stats.

**Stephen:** Years ago on a plane flight from London I'd written a Sudoku app for generating, solving, and playing. That's been my go-to app for porting to new platforms to try them out. It was originally written for Windows Forms. Then I ported it to WPF. When I was working on our parallel computing effort, I used it as a demo for parallelization. When Windows 8 came out, I ported it to WinRT. When we released an early tablet device, I ported it to run on that and light-up with ink. And when we were bringing up .NET Core on Linux, after "Hello, world" it was one of if not the first app to run on Linux on .NET Core (ported to have a console interface).

**Matt:** In the early days we used Jenkins as our public CI system. We had no idea what we should really be using. But this was a relatively old, monolithic version of Jenkins that was not designed to handle the scale we were trying to use it at (tens of thousands of jobs with a lot of orchestration). It got the job done, but managing a stable installation was a challenge. It ended up that we just created successively larger and larger VMs with larger amounts of memory (100s of GBs) and we still had to have someone watch to see when it would fall over so they could smash the restart button.

**Dan:** I'm constantly surprised at the engagement and expertise of the community. It's very much a partnership, and it's gratifying how many developers are so passionate about .NET that they have contributed such substantial features and changes that have made .NET better.

## What's the most surprising thing about the .NET open source project?

**Immo:** We have talked open sourcing for years. I'm still surprised how fast we went from having concerns to "why is it not done yet". This has changed the entire team culture. We moved pretty much in weeks from an internal TFS with TF VC to GitHub with Git and developing in the open. I'm stunned, amazed, and incredibly proud of being part of a team that adapted so quickly, with relatively few hiccups.

**Kevin:** It's getting normal at this point after doing it for the last 7 years, but I still get excited at the fact that my full time job is to work on Open Source technologies at Microsoft. It's something I had a really hard time imagining when I started back in 2002.

**Matt:** I am always surprised at how fast it moves and how it seems to be continually accelerating. I used to be able to keep in my head the current set of efforts were going on, the state of the various repositories and their relationship between one another. I've had more and more moments of 'wait...we do **that** now?' in the last year than I did in perhaps the first 3 combined. But then again, I might just be getting old.

**Dan:** Since .NET open sourced, we've been approached many times by internal teams asking for help and guidance to open source their own projects. They've in turn helped become role models for other Microsoft teams. It's really surprised me how much interest there has been in how we operate in the open and how they can learn from it.

**Immo:** One of the things that I didn't forsee is how much open source has helped out internal engagement with partner teams. Looking back, it shouldn't have been that surprising. Microsoft is a large company, with various different engineering systems, shipping cycles, and business goals. With open source, we try to make our code bases approachable by people who aren't working on our team, which makes it simpler for anyone -- including a different team at Microsoft.

## The project seems very popular and busy. Why do you think that is?

**Immo:** .NET has a reputation with being very productive, especially with C#. Before open source, I've heard from many people, some die hard "Microsoft is evil" people, how awesome C# is but that they refuse to use it because it's closed source and Windows-only. It seems many people finally had a chance to use it for their projects, even though they don't involve Windows at all.

**Kevin:** Partly I think it goes to show how much .NET is a part of our customer's lives. It's something that people use day in and day out, and having the ability to have direct engagement between the engineers and customers is highly valuable. There are also a variety of folks on the team that spend a bunch of time thinking about how to work well with the community, and how to make our OSS presence more inclusive and welcoming. Definitely want to highlight Dan's efforts in this space.

**Matt:** We've gotten the "core" of .NET solidified over the years. We now have the opportunities to branch out and try new and innovative things. Our engineering systems have gotten so much better compared to the early days. We can just move faster now.

**Stephen:** C# is a very approachable and well thought-out language, the .NET libraries are robust and sprawling, and the runtime is rock-solid. You can write very efficient and scalable apps and services, and you can do so productively.

**Claire:** For me, C#, .NET, and Visual Studio, has always been the "it just works" platform. I could get started and productive quickly and just start debugging without having to configure complicated things like class paths or manually specify garbage collector settings.

**Dan:** The .NET developer base is very large, plus as most of the .NET platform is written in C# it's relatively easy to contribute to it and make a real difference. Plus, we became relevant to all developers, not just Windows developers - no matter what platform you work on, .NET is relevant.

## In your view, are people happy with the project, open source wise?

**Immo:** From talking to other members in the .NET open source community it seems there are many things we got right. Specifically, when we started we told everyone that we'll likely make mistakes and given our track record we're grateful for honest feedback. I think that has set a great tone with our community engagement. That being said, it's also clear that being great in OSS isn't a destination -- it's a journey. And there is still a lot we can learn from other communities.

**Dan:** We know from surveys of our Github communities that for the most part, they're very happy. It varies by repo, and there's a lot we can do better: being more responsive and transparent, making it easier to get started and contribute. We can learn more from what other OSS projects do well. We've made improvements but there's so much scope to do better.

**Matt:** I think the core developers are generally happy with .NET as an open source project. At least, I don't see any fundamental push back to open-sourcing. Culturally within the .NET organization, it's just the default. It's just what we do. Disagreements we have these days are more on the nuts and bolts of working effectively. How can we best manage issues? What should our PR testing bar be?

**Kevin:** I think we're always trying to improve. There are always going to be people that are disappointed that their thing doesn't get fixed, but right now, it's not that easy to build/debug/run tests in our repos. Sometimes we don't do a good job of setting expectations of when or if issues will get addressed. And so on. So, I think we do okay, but I think we're always looking to improve.

**Dan:** A couple of examples of changes we've made to be more inclusive: we logged our .NET 6 plans in Github, and manage them there; we're also using the dotnet/designs repo extensively so from the start the community can help share and refine our designs and plans.

## There are kind of two levels of open source with the project, with "source build" being the second level. Can you explain that?


## Project maintainers often talk about burn out. They are working after hours. You are doing this as your primary job, so the experience likely isn't the same issue. What are the  primary challenging issues you experience?

**Dan:** Because the repo never sleeps, you have to let go of the idea that you can keep up with what's going on or that there's an obligation to respond outside of work hours. On the other hand, for folks that like flexibility in when they work, it can be an advantage - there's usually someone around to do a code review, for example.

**Kevin:** I think for me, it can still feel overwhelming trying to stay on top of what's going on. Several of the repos I manage get hundreds to thousands of new issues every month, let alone PRs, comments, etc. Even though it's a full-time job, it still doesn't feel like there is enough time to give everything the attention it deserves. One way I've seen people address that is by expanding their working hours to the point where they still end up in that burnout state.

**Stephen:** Not having enough time to do everything I'd like to do. For example, I try to review a significant percentage of the PRs that come through dotnet/runtime, but there's just so much volume, at the end of the day I have to prioritize.

**Matt:** One challenge is just avoiding being interrupt driven all the time. Since .NET moves so fast, I could spend my whole day responding to PR review requests, helping developers get unblocked, or fixing infrastructure bugs. The challenge is to stay focused on getting to the root of those issues I see to think strategically about how to solve whole classes of problems, rather than pounding individual ones all day.

**Dan:** There are many folks on the team who are really good at keeping strong boundaries on their availability. We are very clear that that's fine, and actually ideal.

There are a fixed number of committers, and an unlimited number of potential contributors. That can make it hard to keep up with issues and PR reviews. We keep up more or less but I think we are still trying to find the right model to manage this. There's likely opportunities we can offer to the community to help here.

**Claire:** There can be a never ending stream of issues posted given the global nature of the project and our contributors. I try to find a work/life balance by using the various "Do Not Disturb" features when after work hours.

## It's pretty obvious that the open source nature of the project goes well beyond the license and is mostly about the community. What has the team done to improve community engagement?

**Claire:** We're constantly working to ensure interactions with and between the community are respectful so that anyone can participate.

**Dan:** Many examples. We've been piloting community triagers -- several repos now have community members that can set labels and close duplicate issues. On the code side, we've in some cases brought our community contributors into our project standups: we did this for push on networking performance last year, and it was very successful. Where possible, when there are community members who are passionate about implementing a feature, we're happy to step out of the way - we've done this with Web Socket Compression and PriorityQueue for example.

**Kevin:** Dan and a few others have been running the survey for a while now, and we've been trying to apply learnings from that. As I mentioned above, trying to make expectations of about the state of issues clearer, making the repo more approachable and easier to build, putting systems in place to try to make sure that community PRs get reviewed in a timely fashion, etc.

**Immo:** In the past, we've had a track record of doing "source open", that is throw-over-fence OSS where we work behind close doors and publish code bombs as you mentioned earlier. With .NET Core 1.0, we started to adopt the model that we work off of GitHub. This allows people to see code reviews between team members and our real bug tracker. Many of us also joined social media and promoted .NET and our GitHup projects a lot more. We also started to live stream design discussions on YouTube. We now have various web sites to share more content with the community, such as issuesof.net, apisof.net, apireview.net or themesof.net.

**Matt:** On the infrastructure side, our philosophy has always been to try and give non-Microsoft .NET contributors the same tools and processes Microsoft developers have at their disposal. The same PR checks and public access to results, the same coding tools, etc.

**Dan:** Build on Matt's comment there, this has also made it easier for folks in our team who aren't located in the office -- they can work without having to connect a VPN.

## Imagine you decided to move to a new (work) project. Would open source be part of your criteria? How important is it to you?

**Stephen:** That's a sad thought; why would I want to leave? But yes, it's extremely important.

**Dan:** I worked on closed source projects all my career until .NET. Now I can't imagine working on a project that isn't open source, or destined to be open source. It's just so much more fun - more energy, more perspectives, more people to meet. It's also more productive.

**Claire:** Being able to participate and contribute to open source is very important to me. Being able to interact with the community, get feedback on new ideas and iterate on them in the open, is critical.

**Immo:** As a PM, I massively value the ability to engage with my customers in real time, which includes connecting them directly with the artifacts my engineering team produces right now. Not being able to do this anymore would be hard take back. It doesn't hurt that all my work is also visible publicly, should I ever leave Microsoft.

**Kevin:** I've been working on Open Source full-time for the last 7 years or so, and it was something I've wanted to do since I was in university. I have a hard time imagining being on a team that isn't Open Source.

**Matt:** I don't think OSS code would be a hard requirement for me. But certain OSS 'ways of thinking' are important. For example, transparency, diversity, integrity, community, public standards and debate, and OSS tooling are all important to me. It does happen that OSS projects often exhibit these attributes though.

## Has .NET adoption increased because it is open source?

**Claire:** Without a doubt. Prior to being open source, .NET was limited to Windows. Now that it's open source, it's runs in so many more locations.

**Immo:** I like to believe the answer is yes, but I think .NET Core has changed the scenarios a lot with cross-platform support, including devices. It's hard to tease apart which wins are due to what factor. But I think OSS is for sure a critical part of us being able to build .NET.

**Dan:** Open Source has made it much easier for us to be cross platform, because we can collaborate with Linux based communities that are naturally open source. And reaching Linux (and Mac) makes us an option for developers that are Linux based. Of course, there are also developers that just prefer open source stacks. It also makes it easier to trust .NET -- you can read the sources yourself.

**Matt:** I think so. I think it's a little hard to draw a direct line between the fact that .NET is OSS and adoption, but I think that OSS may have increased the visibility of the project in some ways, and also enables its use in some places (e.g. RedHat) that may not have been possible otherwise.

## Microsoft, at least historically, has primarily delivered closed-source products. Have there been any challenges with long-time Microsoft customers adopting .NET as open source?

**Kevin:** There are definitely some. For me, this has typically come up around support. While it's clear to most people that things published by Microsoft are supported, take an example like gRPC-dotnet, where Microsoft has written the vast majority of the code, but it's actually owned and released by the CNCF. What's our support story for that? (The answer is that we just got this straightened out and it is supported). But that comes up again and again for 3rd party libraries that we recommend, contribute to, etc.

**Dan:** Many .NET customers have historically composed their apps from Microsoft supplied libraries (which were historically closed source) and their own code, and are less comfortable depending on non-Microsoft libraries - which are typically open source. We'd like to make it easier for them to feel that they trust libraries that don't come from the .NET team, and make the case that often times, open source should make it easier to trust them.

**Matt:** At least internally, there is occasionally some confusion about how our OSS projects are developed and the relationship between that and how they can get their needs met (servicing, new features etc.). Occasionally they might be a little uncomfortable with the idea that a traditionally "Microsoft" project like .NET has code developed by someone outside Microsoft in it. But I think that is largely historical assumptions about what .NET is rather than actual discomfort.

## Describe the commitment to open source at Microsoft.

**Immo:** I feel like we have been pretty transparent about this from day one. Microsoft as a company is moving towards the cloud. Many of Microsoft's customers are using .NET and we think making .NET great for the cloud is critical. And we believe open source is the best way to make .NET great for the cloud.

**Dan:** The best answer is to look at what's been happening in recent years. The number of Microsoft maintained open source projects has continued to grow and now includes several of the most popular Github repositories. There is powerful momentum.

## What has the relationship with the .NET Foundation been like?

**Claire:** There has been confusion with ".NET the project" and the .NET Foundation. The .NET Foundation holds the IP of the project, but it does not control how the projects operate. Microsoft controls the .NET project, as each other member project controls itself. In my roles as Executive Director of the .NET Foundation and as a Program Manager on the .NET team, I wear two hats and am always aware of which I'm wearing for a given conversation. I think at first, the lines between the .NET project and the .NET Foundation were blurry; over the past couple of years we've worked to put a cleaner separation between the two.

## Do Microsoft lawyers like or loath open source? Do they understand it?

## Closing

At this point, open source is so totally assumed that we don't even really talk about working on an "OSS project". Working in the open on GitHub has totally changed the team and the product in such meaningful and wonderful ways forever. We're several years in at this point and the project and product is still growing. That's heartening an satisfying. Clearly, open source is the best way to develop an application platform. It's also a lot more fun. Thanks to everyone that has contributed to the project.

Thanks to Stephen, Matt, Kevin, Immo, Dan, and Claire for sharing your insights on the .NET open source project and what's it's been like to be part of it.
