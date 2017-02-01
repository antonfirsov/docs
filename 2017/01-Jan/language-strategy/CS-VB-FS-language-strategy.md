The .NET Language Strategy
==========================

I am constantly aware of the enormous impact our language investments have on so many people's daily lives. Our languages are a huge strength of the .NET platform, and a primary factor in people choosing to bet on it - and stay on it.

I've been here on the .NET languages team at Microsoft for more than a decade, and I've always seen us have developers' interests first and foremost in our minds as we moved the languages forward. The open source revolution (of not just the .NET languages but the [whole .NET stack](https://github.com/dotnet/)) has improved the conversation dramatically, and - I think - helped us to make better choices. However, we haven't always been good at sharing *how* we make those decisions: Our language *strategy*; the framework for how we think about each of our .NET languages and chart their evolution.

This post is meant to provide that additional context for the principles we use to make decisions for each language. You should consider it as guidance, not as a roadmap.


C#
==

***C#*** *is used by millions of people.* As one data point, this year’s Stack Overflow developer survey shows C# as one of the [most popular](http://stackoverflow.com/research/developer-survey-2016#technology-most-popular-technologies) programming languages, surpassed only by Java and of course JavaScript (not counting SQL as a programming language, but let's not have a fight about it). These numbers may well have some skew deriving from which language communities use Stack Overflow more, but it is beyond doubt that C# is among the most widespread programming languages on the planet. The diversity of target scenarios is staggering, ranging across games in Unity, mobile apps in Xamarin, web apps in ASP.NET, business applications on Windows, .NET Core microservices on Linux in Azure and AWS, and so much more. 

C# is also one of the few big mainstream languages to figure on the [most loved](http://stackoverflow.com/research/developer-survey-2016#technology-most-loved-dreaded-and-wanted) top 10 in the StackOverflow survey, joining Python as the only two programming languages occurring on both top 10s.  After all these years, people still love C#! Why? Anecdotally we've been good at evolving it tastefully and pragmatically, addressing new challenges while keeping the spirit of the language intact. C# is seen as productive, powerful and easy to use, and is often perceived as almost synonymous with .NET, neither having a future without the other.

Strategy for C#
---------------

*We will keep growing C# to meet the evolving needs of developers and remain a state of the art programming language. We will innovate aggressively, while being very careful to stay within the spirit of the language. Given the diversity of the developer base, we will prefer language and performance improvements that benefit all or most developers, avoiding over-focusing on a given segment. We will continue to empower the broader ecosystem and grow its role in C#'s future, while maintaining strong stewardship of design decisions to ensure continued coherence.*

Every new version of C# has come with major language evolution: generics in C# 2.0, language integrated queries (and much functional goodness) in C# 3.0, dynamic in C# 4.0, async in C# 5.0 and a whole slew of small but useful features in C# 6.0. Many of the features served emerging scenarios, and since C# 5.0 there’s been a strong focus on connected devices and services, the latency of those connections and working with the data that flows across them. C# 7.0 will be no exception, with tuples and pattern matching as the biggest features, transforming and streamlining the flow of data and control in code.

Since C# 6.0, language design notes have been public. The language has been increasingly shaped by conversation with the community, now to the point of taking language features as contributions from outside Microsoft.

The C# design process unfolds in the [dotnet/csharplang](https://github.com/dotnet/csharplang) GitHub repository, and C# design discussions happen on the [csharplang](https://lists.dot.net/mailman/listinfo/csharplang) mailing list. 


Visual Basic
============

***Visual Basic*** *is used by hundreds of thousands of people.* Most are using WinForms to build business applications in Windows, and a few are building websites, overwhelmingly using ASP.NET Web Forms. A majority are also C# users. For many this may simply be because of language requirements of different projects they work on. However, outside of VB's core scenarios many undoubtedly switch to C# even when VB *is* supported: The ecosystem, samples and community are often richer and more abundant in C#.

An interesting trend we see in Visual Studio is that VB has twice the share of *new* developers as it does of *all* developers. This suggests that VB continues to play a role as a good, simple and approachable entry language for people new to the platform and even to development.

The Stack Overflow survey is not kind to VB, which tops the [list of languages](http://stackoverflow.com/research/developer-survey-2016#technology-most-loved-dreaded-and-wanted) whose users would rather use another language in the future. I think this should be taken with several grains of salt: First of all this number may include VB6 developers, whom I cannot blame for wanting to move on. Also, Stack Overflow is not a primary hangout for VB developers, so if you're there to even *take* the survey to begin with, that may be because you're already roaming Stack Overflow as a fan of another language. Finally, a deeper look at the data reveals that the place most VB developers would rather be is C#, so this may be more about consolidating their .NET development on one language, and less of a rejection of the VB experience itself.

All that said, the statistic is eye opening. It does look like a lot of VB users feel left behind, or are uncertain about the future of the language. Let's take this opportunity to start addressing that!

Strategy for Visual Basic
-------------------------

*We will keep Visual Basic straightforward and approachable. We will do everything necessary to keep it a first class citizen of the .NET ecosystem: When API shapes evolve as a result of new C# features, for instance, consuming those APIs should feel natural in VB. We will keep a focus on the cross-language tooling experience, recognizing that many VB developers also use C#. We will focus innovation on the core scenarios and domains where VB is popular.*

This is a shift from the [co-evolution strategy](https://blogs.msdn.microsoft.com/scottwil/2010/03/09/vb-and-c-coevolution/) that we laid out in 2010, which set C# and VB on a shared course. For VB to follow C# in its aggressive evolution would not only miss the mark, but would actively undermine the straightforward approachability that is one of VB's key strengths.

In VS 2015, C# 6.0 and VB 14 were still largely co-evolved, and shared many new features: null-conditional operators `?.`, `NameOf`, etc. However, both C# and VB also each addressed a number of nuisances that were specific to the language; for instance VB added multi-line string literals, comments after implicit line continuation, and many more. C#, on the other hand, added expression-bodied members and other features that wouldn't address a need or fit naturally in VB.

VB 15 comes with a subset of C# 7.0's new features. Tuples are useful in and of themselves, but also ensure continued great interop, as API's will start to have tuples in their signatures. However, VB 15 does not get features such as is-expressions, out-variables and local functions, which would probably do more harm than good to VB's readability, and add significantly to its concept count.

The Visual Basic design process unfolds in the [dotnet/vblang](https://github.com/dotnet/vblang) GitHub repository, and VB design discussions happen on the [vblang](https://lists.dot.net/mailman/listinfo/vblang) mailing list. 


F#
==

***F#*** *is used by tens of thousands of people* and shows great actual and potential growth. As a general purpose language it does see quite broad and varied usage, but it certainly has a center of gravity around web and cloud services, tools and utilities, analytic workloads, and data manipulation.

F# is very high on the [most loved](http://stackoverflow.com/research/developer-survey-2016#technology-most-loved-dreaded-and-wanted) languages list: People simply love working in it! While it has fantastic tooling compared to most other languages on that list, it doesn't quite measure up to the rich and polished experience of C# and VB. Many recent initiatives do a lot to catch up, and an increasing share of the .NET ecosystem - inside and outside of Microsoft - are thinking of F# as a language to take into account, target and test for.

F# has a phenomenally engaged community, which takes a very active role in its evolution and constant improvement, not least through the entirely open language design process. It's been an absolute front runner for open-source .NET, and continues to have a large proportion of its contributions come from outside of Microsoft.

Strategy for F#
---------------

*We will enable and encourage strong community participation in F# by continuing to build the necessary infrastructure and tooling to complement community contributions. We will make F# the best-tooled functional language on the market, by improving the language and tooling experience, removing road blocks for contributions, and addressing pain points to narrow the experience gap with C# and VB. As new language features appear in C#, we will ensure that they also interoperate well with F#. F# will continue to target platforms that are important to its community.*

On top of the strong functional legacy from the ML family of languages and the deep integration with the .NET platform, F# has some truly groundbreaking language features. Type providers, active patterns, and computation expressions all offer astounding expressiveness to those who are willing to take the jump and learn the language. What F# needs more than anything is a focus on removing hurdles to adoption and productivity at all levels.

Thus, F# 4.1 sees vastly improved tooling in Visual Studio through integration with Roslyn's editor workspace abstraction, targeting of .NET Core and .NET Standard, and improved error messages from the compiler. Much of the improved Visual Studio tooling and especially the improved error messages are a direct product of the strong F# open source community. Looking down the road, we intend to work both with the F# community and other teams at Microsoft to ensure F# tooling is best-of-breed, with the intention of making F# the best-tooled functional programming language in the market.

F# language design unfolds in the [language suggestion](https://github.com/fsharp/fslang-suggestions) and [RFC](https://github.com/fsharp/fslang-suggestions) repositories.

Conclusion
==========

Hopefully this post has shed some light on our decision-making framework for the .NET languages. Whenever we make a certain choice, I'd like you to be able to see where that came from. If you're left to fill in the gaps, that easily leads to unnecessary fear or speculation. You have business decisions to make as well, and the better you can glean our intentions, the better informed those can be. 

Happy hacking!

Mads