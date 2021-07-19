---
post_title: Conversation about skills and learning.
username: rlander@microsoft.com
microsoft_alias: rlander@microsoft.com
categories: .NET
desired_publication_date: 6/21/2021
summary: Conversation with .NET engineers about dev skills and learning. They share their perspectives on the skills and aptitudes needed to be successful on the team, and how important learning on the job is versus any specific skills that you walk in the door with. They also share the journeys they have taken on personal and career development.
---

It can be really hard to tell which skills are important to get hired on and to be successful on a capable and important team at a big tech company. Some of the stereotypes of what's required are true enough, but there is much more to the story. This post is a sort of inside take on the people working on .NET, about the skills they use on a daily basis, how they developed them, and how they've figured out their own individual ways to be successful.

We’re using the [conversation format](https://devblogs.microsoft.com/dotnet/category/conversations/) again, this time with .NET engineers on dev skills and approaches to learning. We've got a big and great cast this time.

* [Aditya Mandaleeka](https://github.com/adityamandaleeka)
* [Elinor Fung](https://github.com/elinor-fung)
* [Jeremy Kuhne](https://github.com/JeremyKuhne)
* [Jose Perez Rodriguez](https://github.com/joperezr)
* [Kunal Pathak](https://github.com/kunalspathak)
* [Maoni Stephens](https://github.com/Maoni0)
* [Maryam Ariyan](https://github.com/maryamariyan)
* [Rainer Sigwald](https://github.com/rainersigwald)
* [Tanner Gooding](https://github.com/tannergooding)
* [Thays Grazia](https://github.com/thaystg)

## How did you get hired on the .NET Team?

**Elinor:** Some people I had previously worked with (and enjoyed working with) were on the .NET team. They pointed out the team was hiring and suggested I might enjoy it more than what I was doing at the time - and here I am.

**Aditya:** In early 2015, I heard .NET was going cross-platform and open-source, which I thought was really exciting. At the time, I was working on Visual Studio and had been collaborating with some CLR folks on making profiling work for AOT-compiled code, so I took the opportunity to learn more about the plans/challenges, and soon found myself on the CLR team myself, working to make that cross-platform vision happen.

**Rainer:** I was part of the [1ES](https://azure.microsoft.com/solutions/devops/devops-at-microsoft/one-engineering-system/) team, working on engineering productivity in Microsoft in general and on other technologies (like what became [BuildXL](https://github.com/Microsoft/BuildXL). The primary maintainer of MSBuild left to try new things, and I was the person who knew MSBuild best and eventually took over ownership as development ramped up on .NET Core 1.0. Later, MSBuild was reorganized to be closer to our partners in the .NET SDK and Visual Studio teams and I was given the option to stay in 1ES, but I chose to come to .NET.

**Tanner:** I started out with a vendor (contracted work via another company) position in 2012 doing testing work for Roslyn and that eventually transitioned into getting hired on full time to the same team in 2015. As an FTE, I transitioned from C# Scripting, to Live Unit Testing, to the Project System and SDK, and finally to the .NET Libraries team where I am now.

**Thays:** A friend of mine worked on Mono team at that time, and I worked in Brazil in a company that has its own proprietary language, so I worked with compiler, runtime and debugger. My friend thought that work on mono runtime would makes sense to me, so he told me about this position and I applied. Then I was hired for Rodrigo Kumpera to work on wasm team.

**Jose:** I used to play soccer with a lot of folks that used to work on the team, one of which was Peter Marcu who used to lead the .NET libraries and runtime teams back then. When .NET started focusing on going cross-plat, they had some openings and I was lucky enough to apply and get a spot. 

**Jeremy:** When .NET Core was announced I saw the opportunity to address shortcomings, notably around long path names. I opened issues on GitHub then realized I might be able to actually join the team to work on them. I reached out for an informational and got hired by the libraries team.

**Kunal:** I have strong inclination towards compilers domain and in the past, have worked on a Javascript runtime team. I wanted to explore other runtimes in Microsoft which attracted me towards .NET team. It was an internal transfer, but I should admit, the interviews had a high bar.

**Maryam:** I was working at a different company prior to joining Microsoft which was transitioning to .NET Core and since we were clients of .NET I was very passionate about everything .NET. During a Microsoft Build conference I reached out to the .NET booth on the second day mentioning my interest and things started flowing from there.

**Maoni:** I wanted to make a difference in .NET perf and the perf team had a position open. It was a PM position and since I had never been a PM before I thought I'd try it. It turned out that I'm totally not a PM so after 9 months I switched back to being a dev but stayed on the team.

## What are the skills you rely on most?

**Jose:** The ability to unblock myself and be productive. When facing a tough problem, it is very important to me to be able to select an appropriate time to try to fix the issue myself, and if after that time has passed I haven't yet found the answer, to be able to ask to the right people the right questions in order to get help and unblock myself.

**Maoni:** I used to rely mostly on my problem solving skills; nowadays communication skills are also important.

**Kunal:** Reading and understanding the assembly code and mapping it back to the original source code is a crucial skill in codegen team. Apart from that, quickly ramping up on unfamiliar tools and learning to switch between them is highly recommended (one day I am using perfview on windows and next day I am using gdb on Linux Arm) while debugging cross platform issues. Lastly, since the .NET product is vast, some files are over 10,000+ lines of code, it is not practical to understand every method. Since .NET is developed over several years, often you end up working on an unfamiliar piece of code. An important skill to have is reading the legacy code and quickly understanding what purpose it solves. 

**Rainer:** Debugging and problem-narrowing: figuring out how to identify what's going on from the information we have and picking the next steps to figure new stuff out. Communication to be able to ask good questions (either "so I can get help", "so I can describe the problem to others", or "so I can get more information about things from the person who's asking me").

**Tanner:** This is a hard one as there are so many. I think I most rely not necessarily on my own skills but the skills of my co-workers. The .NET team is very diverse in skillset and backgrounds and we all help watch out for our own specializations to help make the product the best it can be.

**Thays:** I think I rely mostly on my persistence when trying to solve a problem.

**Rainer:** Persistence is a good one. I had a hard time getting convincing my students that it was critical when I was teaching high school kids through [TEALS](https://www.microsoft.com/teals), but it really is.

**Maryam:** normal debugging skills, keeping conversations flowing on github on issues and PRs (responsiveness and keeping stakeholders up to date), adding to domain knowledge, time management.

**Jeremy:** Troubleshooting and researching are the primary skills I rely on. Finding technical details and relevant people is critical.

**Aditya:** One of the challenges in working on something as vast and complex as the CLR (or .NET all up) is that it's hard to know everything about everything, so being able to focus on a smaller system within the larger system and build a mental model quickly is really helpful, both for debugging and problem solving, but also for being able to have a productive collaboration with someone whose areas of expertise are different from my own.

**Elinor:** Asking questions - not sure how to label it. I consider this to encompass things like asking for help, raising concerns I may have, pointing at issues that need more investigation (whether by me or someone else), and questioning why some process/design/implementation is a certain way and if it could be better. Perhaps this is really just communication and collaboration in general.

Reading and debugging things other people wrote. All my work is building on existing systems, so I rely heavily on the ability to read and debug through unfamiliar territory in order to understand some system and effectively contribute to it. Reading sounds basic but being able to read a doc, some code, or proposed change and reason about what it means and how it fits into a larger system is really useful.

## What are the skills you wish you were stronger at?

**Jose:** Being more organized with all the things I have to do. I'm usually easily distracted into some new task while in the middle of another one, and I find it hard to remember to come back and complete the task that I was originally doing after the interruption is done. I really need to work on developing and adhering to a system that keeps me focused on all tasks at hand.

**Elinor:** Going to limit myself to three here:

* Asking questions =)
* Sharing information - presenting, public speaking, technical writing
* Processing aural technical information (especially in fast-moving meetings)

**Maoni:** Being more organized. I have to do a ton of context switches in my work and sometimes I have trouble keeping track of things.

**Maryam:** Toggling between different issues especially when focused on a longer running task and making sure to allow myself to make progress on unrelated smaller scale tasks as well.

**Thays:** I would like to be a better C# programmer, I programmed for the last 15 years using C and C++. Since I started at Microsoft I improved a lot my C# skills, but I still would like to be better. I would like to communicate better, as I'm not a native English speaker, sometimes I have issues to express myself.

**Tanner:** Staying focused on areas outside my interest. I have a decent background in numerics and working low-level and I can get very passionate and driven around those things. I am not so skilled with higher level concepts like web development, services, or databases and so I tend to lose my focus for these areas

**Jeremy:** Technically I'm perpetually wishing I had a better understanding of assembly. Interpersonal skills is something I'm always trying to improve and never satisfied with.

**Rainer:** Communication and delegation. Every Connect I have an entry about "reducing randomization and doing better distribution of work amongst my teammates". I haven't gotten really good at distributing my knowledge to the other folks on the team, either, which doesn't help--and that goes for public-facing documentation as well.

**Kunal:** Because of the complexity involved, it is very easy to get lost and barking up the wrong tree. I need to learn to have a good balance on how much investigation I should do and when to stop and ask questions to the experts.

**Aditya:** Some of my colleagues are able to remember very specific details about systems, sometimes down to variable names and the like. This is a superpower I don't have, and it sometimes means that I have to take a beat at the start of a project to re-familiarize myself with the specifics of an area.

## Zooming out. There are lots of different roles on the .NET Team. Describe a broad and stereotypical set of skills that are required to be successful?

**Jose:** Usually just have a decent programming background, but more importantly to be able to learn new concepts, adapt and help others. Given the framework's footprint is so wide, this means that different parts of it may need, every now and then, for random devs from the team to jump up and help. Being able to learn and adapt quickly on the job is a great skill to have in this team.

**Rainer:** Curiosity, persistence, and communication skills are the key to success. Be interested enough to dig into a problem, persistent to figure out the right approach to it, and know how to write down what you learned and ask for help effectively.

**Kunal:** Critical thinking and problem solving is a must in .NET.  For soft skills, I would say good communication skill is very important; we need to know how to explain a complex problem in simple terms to the audience who is not familiar with compiler domain. That skill really helps in engaging with the community and educating others about the design decisions that are baked in .NET.

**Elinor:** I'm not sure I know how to define a specific set. At the really high level, I'd say communication, collaboration, critical thinking, and the willingness to learn/be wrong. I will give another shoutout to reading and debugging as being really helpful though.

**Aditya:** In no particular order:

- Debugging and problem-solving
- Communication
- The ability to zoom in deep about a technical issue and then zoom out really far to consider things like customer impact, usability, etc.

**Maoni:** Debugging skills and being persistent at it. The runtime system can be very complex and some problems take a long time to figure out. Another very useful skill is being able to break down large problems into smaller, more manageable pieces.

**Thays:**

- Persistence
- Debugging
- Desire to Learn

**Maryam:** on a day to day: debugging skills, writing/reviewing code with a critical eye on performance, API design, issue prioritization/management

**Tanner:** I think the stereotypical set would be listening, communicating, and problem solving in the .NET team. Particularly having those allow us all to build off our individual strengths

**Jeremy:** I think the ability to push through things is critical. The scope of things can be overwhelming and it is easy to feel that the challenges are insurmountable when you are starting any given task.

## Conversely, can you describe niche skills that people have on the team that you might not have and that are important for our shared success?

**Jose:** I think that what really sets us apart is that even though each individual dev has their own personal tasks and responsibilities, which most of the time are very different between each other, we all seem to have the same focus in mind which is building a great .NET product. That along with the characteristic that we always try to help each other, make a great collective team and is one key ingredient of any successful product.

**Maryam:** relying more on tooling to be more productive, in other words, automating as many work items as possible, with the goal of "working smarter not harder"

**Maoni:** Being able to generate energy in the community. I think David Fowler is really good at this, a skill I wish I had (or had the drive to get better at :D).

**Rainer:** [PerfView](https://github.com/Microsoft/perfview) wizards are always impressive to me. There are a lot of intermediate-to-advanced CS skills that I don't have (my schooling was in mechanical engineering), and a lot of areas that I don't have experience in, like networking or assembly or Linux syscalls.

**Tanner:** Some team members are much more extroverted and are able to push out tons of blog posts and do talks and raise interest in the community. This isn't really a niche skill, but its definitely one that I don't have and allows me to stay more in my comfort zone while still allowing for a great .NET product.

**Jeremy:** Having people that go deep technically on a wide range of topics and those that have good higher level grasps on various slices of what we cover is critical.

**Elinor:** There are some skills that aren't niche in general, but I think the extremely high level of proficiency that some folks have in them is uncommon and important for the team. For example:

* Community engagement - it is impressive just how involved some folks are with so many different areas of the industry
* Dump debugging - reconstructing incredibly complex sequences of steps and the corresponding machine state necessary to get some behavior.
* Deep understanding of a particular OS and/or architecture (and its quirks) - it is great to have people to turn to when fighting some behaviour on a specific OS/architecture that you might not have worked on a lot

## If you work on the runtime team, do you need to know assembly and know what `jmp` and `eax` are or speak in hex? If you work on the libraries team, do you need to know C? If you work on the x team, do you need y stereotypical skill?

**Kunal:** When a software engineer writes a program, they need to check and understand if the program generated correct output. For compiler teams (specially codegen), the output is the assembly code. So, being on compiler team, you cannot get away from assembly code. At a higher hierarchy, like runtime or type-system, it might not as much needed. Also, most of the runtimes are written in native C/C++, having good sense of understanding help you as well as our team to write better and efficient code.

**Aditya:** There's so much involved in building and shipping something as complex as .NET, and I think everyone brings some unique piece of the puzzle without which it wouldn't be complete. For instance, we have people in the org who are experts in specific technologies, people who are great about identifying what our customers need, people who actually know how to turn all our work into packaged releases that get shipped, and the list goes on and on.

**Jose:** I don't think there is any knowledge pre-requirement to work on either the runtime or the libraries. The more skills you have usually means you need less ramp up time since there is one less thing for you to learn, but in general I would say that the only real requirement is to be able to learn on the job, since we need to do that pretty often as new technologies and language features are developed.

**Aditya:** I can speak for the runtime side of things... it is not expected that you know assembly before you join the runtime team. There's a good chance you will become familiar with it after you join the team though :).

**Thays:** Depends on the part of the runtime that you work, but I think that know assembly is really useful. I think it's good to have some knowledge of C if you work on libraries team.

**Elinor:** Like most things, I believe knowledge and familiarity with a language is a 'learn it when you need it, retain it if you keep using it' situation. Knowing assembly can be helpful on the runtime team, but is certainly not necessary to join the team. Whether or not you end up learning/needing it depends on what exactly you work on.

**Tanner:** I work on the libraries team, but being one of the owners for `System.Runtime.Intrinsics` and `System.Numerics` means I do a lot of work that ends up crossing both sides. I tend to use a lot more C# when working on the library side and a lot more C/C++ and assembly (x86, x64, Arm32, and Arm64) when working on the runtime. Knowing both helps me to do my own work better, but that might not be true for everyone

**Maoni:** If you mean "is knowing what `jmp` does a prerequisite of working on the runtime team", then absolutely not; if you have worked on the runtime for a while I would think everyone would have built up some basic knowledge of assembly.

**Rainer:** The build and SDK space is super broad and demands a bunch of broad knowledge. You don't have to know any particular thing, but knowing macOS vs Linux filesystem quirks can sure help with some classes of problems. And since everybody builds, we get a lot of "my build is broken" type issues where we have to figure out what's going wrong, and it can be in the OS, the compilers, the runtime . . . or of course in our own code (MSBuild XML or C#).

**Jeremy:** I don't think you need those skills, but you should also not be afraid of learning them. You don't have to become an expert, but you should be able to follow along at least.

## The switch to a cross-platform focus must have seemed quick. How quickly did folks learn Linux and Arm?

**Jose:** I joined the team after this cross-platform focus had happened, but I did have to learn Arm architecture development when starting the dotnet/iot repository which primarily focuses on running on devices like a Raspberry Pi which usually have ARM processors. I'd say in general we all adapted quickly to the new focus, and being open source was a huge help since we had contributors which specialized in Linux and ARM which were instrumental reviewing PRs and making suggestions.

**Thays:** This question does not apply from people who come from mono team, I think. ;)

**Aditya:** I think every time we've taken .NET to a new OS or architecture, there was first a period of time when a small subset of the team dove in head-first and spent most of their time working on it, doing all the heavy lifting like getting it to actually build, run HelloWorld, and eventually run bigger workloads. The broader team gradually gets more and more involved as the new platform becomes more stable and eventually just like any other OS/arch we support.

**Rainer:** I switched from Windows to Linux in college and to Mac in grad school, then came to Microsoft and worked on the Windows team. Going cross-platform was a super exciting opportunity to get back to platforms I really like (and then discover that I had learned an awful lot about Windows and forgotten stuff about Linux). But since I'd been keeping up with a bunch of industry folks that are primarily Linux I didn't have too bad of a time.

**Tanner:** I think everyone has learned at a different pace and many of us are still learning as appropriate. It all depends on what you're working on and so sometimes you may need to have more in depth knowledge of a platform and other times you might not need to consider at all. I did a lot of additional learning for ARM64 assembly when we were shipping .NET 5 as it was necessary for the hardware intrinsic work, but I'm still learning it and likely will be for many years before its at the same level as my knowledge of x86/x64 assembly.

**Kunal:** Our code base and tooling infrastructure is pretty solid that we do not need other OS/architecture during development. However, when it comes to debugging the issues on cross-platform, it is a big context switch. Whenever I have to investigate an issue cross-platform, I have to refresh my memories around tooling bash commands, gdb, perf or read the Arm instruction manual.

**Thays:** When I started at Microsoft I switched from Windows to Mac, I think this was not a problem, 2 months were enough to make me feel comfortable with the new OS.

**Jeremy:** Being Windows focused for many, many years did make it a little extra challenging to pick up Unix skills, but it wasn't insurmountable. I also don't think it is necessary to understand them both equally well.

**Elinor:** I wasn't around for the switch, but I think everyone is constantly learning how to work best on the platforms with which they are less familiar - and relying heavily on other team members that are familiar with them. I will say that having contributors outside of Microsoft with deep knowledge of certain platforms is helpful for the team.

## How much of what you rely on are topics you learned at school versus on the job?

**Ranier:** 5% school, 40% hobby learning, mostly on-the-job.

**Jose:** I'd say that most of it has been on the job. In my whole 4 and a half years in the University while getting my Computer Science major, I never once heard anything related to automation or unit testing, nor did anything related to the concept of source control, and I ended up getting a Testing job right after college. School is great for getting the fundamentals of programming, but today it is simply not the only place to get the knowledge, and usually you can learn just as much or even more while on-the-job.

**Tanner:** I didn't go to school and did a lot of self-directed learning via books and asking questions/experimenting. I think I learned a lot more in my first 6 months on the .NET team than I had in all my years of learning before that, as what you learn vs how you apply it can differ a lot.

**Thays:** 10% school, 90% on-the-job.

**Jeremy:** Almost everything is learned on the job. I didn't complete my CompSci degree and what I got there doesn't address "real-world" programming.

**Elinor:** In terms of topics, the vast majority of what I rely on is something I learned on the job. For me, school was more helpful in terms of figuring out my preferred working/learning style and providing experience working as a team on assorted projects. I did work as a TA / grader for various courses, which was useful for learning how I could help others learn.

**Kunal:** I know many smart people who do not have CS degree but still doing great as a software engineer. Personally, I had CS degree and it builds a good foundation of the concepts I work on day-to-day basis. For example, I don't think I rely 100% on the ["Dragon book"](https://en.wikipedia.org/wiki/Compilers:_Principles,_Techniques,_and_Tools) of compilers, but being already read that in college, I know where to look in it if I am trying to find a solution for a niche topic like "register allocator".

**Maoni:** almost everything I use, I learned on the job.

**Aditya:** I think some of the work I've done builds on foundations that I built while in school, but very little of it is directly on topics I studied in school. Of the courses I took, operating systems and computer architecture are probably the ones that have translated most into my work.

## Can you be successful on the team without a University degree or not one in CS?

**Jose:** For sure. Don't have to look super far, my skip-manager (my manager's manager) doesn’t have one and he is very successful. Being successful on this team is more dependent on skills and abilities, and less on pre-existing knowledge.

**Rainer:** Yes, definitely. The trick is getting hired. I was very lucky to squeak by in my Microsoft interview to get my foot in the door.

**Kunal:** You can be successful without a University degree as long as you have passion to solve complex problems and have good communication and collaboration skills.

**Thays:** For sure. At Microsoft I know at least one excellent engineer that does not have a University degree, the same thing I observed in my past job.

**Jeremy:** You can absolutely be successful. Many team members (myself included) don't come from "traditional" CompSci backgrounds. Passion and interest are key and now with open source it is thankfully easier to demonstrate that.

**Tanner:** I hope so, since I didn't go to University and don't have a degree :) I think a lot of it comes down to passion and willingness to learn/adjust. It can be a lot more complicated than this in practice, of course, but it is certainly possible to not even have a typical high school diploma and then still end up being a co-owner for several areas of the .NET Libraries and making contributions to the runtime, which I would at least call some level of success.

**Maryam:** Perhaps for concepts I rely on learning at school for CS Degree. Maybe school helped doing projects here and there and get more comfortable with a programming language of choice. Data structures was probably one of the more important ones that always keeps coming back for work, but yes to get comfortable with day to day work many things can be learnt from teammates and learning through the job.

**Elinor:** Absolutely. I have worked with great people both without a university degree or with one not in CS. I do not have a degree in CS. I actually don't know the education background of most of my coworkers, as it hasn't had any bearing on our work together. I strongly believe that you mainly need the ability and willingness to learn and the good fortune of finding people that will give you the opportunity to do so.

**Aditya:** Yes. Depending on what area you're working on, the ramp-up might be a bit more challenging without any prior exposure to the area, but that's just a temporary challenge. The other skills we talked about (communication, collaboration, problem solving, etc.) are way more important than a specific degree.

**Maoni:** Absolutely. My degree is not in CS. Not having a degree could be an obstacle to getting an interview but nowadays you could have a portfolio with OSS projects which could also get you an interview (perhaps even better than having a degree in some cases).

## Is there a myth or gatekeeping meme in tech that you'd like to dispel?

**Tanner:** I certainly wish I could dispel the gatekeeping, but it does exist and happens regularly, especially for minorities (Women, People of Color, LGBTQ+, etc). I'd hope that we are more accepting/understanding on the .NET team and can only suggest to be pro-active and to use your own privilege, if you have it, to help ensure that the gatekeeping diminishes over time.

**Jeremy:** That you need a specific set of academic credentials to be successful.

**Jose:** Anybody can code :). You don't really need a degree, know English or even know how to use a Computer. If you have an analytical thought process, chances are you have already used your brain for "programming" in one form or another, the key is not being afraid to learn and trying it out. Coding is not only meant for people that understand what 1s and 0s displayed on the Matrix terminal screen mean.

**Thays:** That women cannot program as well as men. :)

**Elinor:** That it is particularly hard or you need to be particularly smart or a self-described 'computer nerd'. Everyone has the basics of the ability to learn and think through problems, but may simply not have had the opportunity to apply them to tech.

**Rainer:** I would like to reinforce the "don't need specific credentials"/"demographics shouldn't matter" stuff but don't have any particularly insightful comments.

## How do you stay current? Do you get time on job for learning or is that mostly on your own time?

**Jose:** Definitely, lately we have been getting some time during work hours that is defined "for learning" in which we get to decide how to make use of the time. In my case, for example, I have been spending the last couple into learning more about web technologies which is nothing that is very related with my day-to-day job but it still interests me.

**Rainer:** A lot of Twitter and reading blogs. Much love to \@b0rk (http://jvns.ca/). I take time in my work day to do these things but it's not super formal. I also try to make tools that I can use in my work using new-to-me technologies to learn.

**Kunal:** Often, I do research about the concepts that I am working on and through that I get to read some good blogs and articles. Lately I have been reading academic papers on "efficient register allocation" to improve our algorithm. That itself keeps me busy. To stay current, I rely on my personal time.

**Tanner:** I spend part of my day helping to do things like moderate the C# Community Discord and staying up to date on the latest news and tech. I'm lucky enough to be free to do so provided I still get my work done and am meeting my deadlines and so I don't need to spend my personal time doing the same. We also occasionally get "learning days" which are dedicated to focusing on things you want to do/learn without eating into a potentially otherwise busy schedule.

**Thays:** I usually use the work hours that is defined "for learning", I also have been learning a lot about Javascript and Node.js being a mentor of an intern. So my answers is: I use time on job to learn.

**Jeremy:** What I'm looking at day-to-day can change quite dramatically so I do, out of necessity, have to learn a lot on-the-job. For example, I've needed to do some work with OLE recently. I do pick up additional skills by following my whims on things outside of work.

**Maoni:** I do both on the job and on my own time. One of my favorite pastime activities is reading GC papers :)

**Elinor:** I have a hard time defining whether job-related learning is happening on the job or in my own time. Recently, the .NET team (and DevDiv as well) has been highlighting learning as a key part of our jobs, which has been great.

I don't specifically track tech news closely, so I often rely on other people to inform me about interesting developments that don't cross into general news and then I do however much investigation I want. Being on an open source project is also helpful in my opinion, since all sorts of issues/questions come through that point at things I might not be familiar with yet.

## How has the open source .NET project changes your work style and experience?

**Maoni:** I love the fact that we could put in a fix and give a customer a build right away. This is so powerful it's very satisfying.

**Rainer:** I had to use more text-based communication, forcing me to be super duper clear because I don't know the other person's experience and English levels.

**Tanner:** .NET being open source has allowed me to actively seek out opinions and contributions from developers interested in the areas I'm responsible for. This has allowed me to more directly address customer concerns and to sometimes get additional feature work in from devs that are particularly passionate about a given feature and who want it in even sooner. It's also allowed me to get earlier feedback to help validate a design is correct and will meet the needs of the target audience which in turn allows for a better product to be shipped.

**Jeremy:** There is a deeper feeling of connection to the community versus working in closed source (which I still do some work with). I definitely prefer working in the open.

**Maryam:** Really happy with the open source aspect. Most conversations can happen offline, e.g. over github, the team is easier scattered across time zones and still productive, we can gain expertise and feedback from devs in community outside our team and it's easier to communicate with clients as the feedback loop is more direct (between area owners and direct users of the features).

**Kunal:** Being working on OSS is a great experience especially because you can connect to so many external contributors who are equally passionate about solving the problems in that product. With OSS, we, the developers get chance to see the issues our customers are facing and can help the community by acknowledging and responding to their needs. One added advantage is that we can always go back and refer to our work anytime, even when we are not working on that team anymore.

**Thays:** This question does not apply for mono team also, but when mono repo was moved to dotnet repo I felt that I had a big responsibility, a PR can be seen by thousands of people. So I always try to write the best code that I can. Always trying to learn with others PR and with the reviews.

**Elinor:** I really like that we are open source. I think it enables much better access to the community's desires and pain points as well as a better understanding of the impact of our work. It is also great to see all the contributions from the community. I do think that it means I am a little more careful with exactly how I write or comment on things, since most people reading it won't actually know me or be able to understand the tone/intent of my message.

**Aditya:** Being open source has helped us be closer to the community, which has had a profound impact on what we do, when we do it, and how we do it. Another nice side-effect of working in the open is that it has helped us all get much better at getting ideas, plans, explanations, etc. in writing in a way that's clear and understandable to observers, even those who may not have much prior context. This is always a good thing, but especially so over the past year where we've all had to shift to remote work.

## The pandemic has been a defining experience of the last year. How has that been as a .NET developer?

**Jose:** I consider myself very lucky in general with regards to the pandemic. Being a developer means that you can continue your work remotely; being open source means that you can work from virtually any place with a network connection; and being at Microsoft means you have enough job security in order to focus on what really matters and be productive.

**Rainer:** I was full-time remote before the pandemic, but still had some big shifts as the rest of the team went remote and we all had to juggle our home and family lives. The team and company were very supportive and I feel super lucky to have been able to take the time I needed, keep my family safe, and work.

**Tanner:** The hardest part has been juggling home and work. There isn't a "natural" transition period between the two (such as commuting) and everyone was stuck inside but the .NET team and management has been very flexible in trying to allow employees to adjust for this. I think, in many ways, it helped bring better tooling and understanding of how to communicate effectively when not in person and I hope much of that will persist after we come back.

## Do devs on the team follow academic research? What are topics you find interesting?


## How do folks on the team stay up to date on other programming languages like Go and Rust, architectures like RISC-V and Wasm? Are those viewed as competitive threats?

**Rainer:** I follow a lot of Mac/iOS developers and a lot of Rustaceans on Twitter and in my RSS feeds, so I try to keep up on those ecosystems especially. I'm pretty excited about RISC-V and Wasm both, but I don't think they're threats: I look forward to someday running build extensions in a [WASI](https://github.com/bytecodealliance/wasmtime/blob/main/docs/WASI-intro.md) sandbox! The pressure from ecosystems with different design decisions will help push us to do well.

**Tanner:** I generally just stay up to date on the things I'm interested in, generally by having an RSS feed to the relevant blog, and pick up bits and pieces via social media otherwise. Personally, I don't view other languages as threats. Multiple language exists because everyone has different preferences and opinions on what is best. Other architectures like RISC-V and WASM represent opportunities to bring the power of .NET to a new area and other languages represent an opportunity for .NET to extend its interop support so users can pick the right tool for each job and still build a working and cohesive program, even if only part of the program needed .NET.

## The average tenure of an engineer on the .NET team is high. What's the allure?

**Kunal** I have seen that people in compilers team stay for long time just because they like the domain and are very passionate to get challenged every single day for solving hard problems.

**Aditya:** I think many of us came to the .NET team because of the opportunity to work on interesting technical areas that have a lot of real-world impact. Those things are true about the team, but perhaps even more important is the great people we get to work with every day.

**Tanner:** While I've only been an FTE (full time employee) for 6 years, I had been on the .NET team as a vendor for nearly 2-3 more years before that (nearly 9 years total). My own "allure" is that I get to work on areas I'm passionate about and have a high impact across the ecosystem, often indirectly. I'm not forced to work on just my areas and I'm free to delve into other areas (even the JIT). The general culture has also been inclusive and I was never given negative feedback or treated badly for being interested, asking questions, or discussing my point of view. This has remained true whether I was talking to another "entry level" coworker, a principal, or even an architect.

## Closing

Perseverance, curiosity, and a desire to improve (both oneself and the product) are key ingredients to success in almost any field, technical, art or otherwise. In many cases, the answers we seek are known and just need to be adapted to the particular needs of .NET and .NET developers. In still more cases, the answers are unknown or nebulous and that's the time that these personality attributes really kick in. It's easy to see that in play on a daily basis on the team.

Thanks to Thays, Tanner, Rainer, Maryam, Maoni, Kunal, Jose, Jeremy, Elinor, and Aditya for sharing their insights on learning and dev skills.
