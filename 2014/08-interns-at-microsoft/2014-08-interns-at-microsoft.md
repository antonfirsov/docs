# Being an intern on the .NET Team

This summer we had six amazing interns that joined the team to work on .NET.
Their projects ranged from internal tools, over shipping components to designing
forward looking aspects:

* **Shaun Arora** (program manager intern): Designing .NET for NuGet
* **Ian Hays** (developer intern): Building a MultiDictionary Collection for .NET
* **Charles Lowell** (developer intern): .NET Portability Analyzer
* **Santiago Fernandez Madero** (developer intern): An LLVM based optimizer for MSIL
* **Zach Montoya** (developer intern): Visual Studio designer for .NET Native
* **Christian Salgado Pacheco** (developer intern): Cataloging .NET APIs

If you're interested in interning with Microsoft, visit
[our recruiting web site](http://careers.microsoft.com/careers/en/us/collegehome.aspx).
Now let's dive right in and see what cool stuff they helped us building.

## Shaun Arora: Designing .NET for NuGet

[We're releasing more and more .NET framework functionality via NuGet][dotnetisnuget].
Moving forward, we intend to bring the two even closer together. Shaun spend a
lot of time thinking about this problem space and helped us shape our thoughs
and design some ideas.

Shaun is also a very talented developer and designer and helped us to build a
[catalog of all the .NET features][dotnetfeatures] we shipped since .NET 4.
Check it out!

To learn more about our thinking on the NuGet integration, watch this interview:

***TBD: URL***

![Shaun Arora](ShaunArora.png)

[dotnetfeatures]: http://microsoft.github.io/dotnetfeatures
[dotnetisnuget]: http://blogs.msdn.com/b/dotnet/archive/2013/10/16/nuget-is-a-net-framework-release-vehicle.aspx

## Ian Hays: Building a MultiDictionary Collection for .NET

Ian's favorite data structure is the dictionary:

> I love the near constant time operations, the huge number of use cases, the
> cleanliness! Although the dictionary has a wide variety of uses, there are
> times when I want to add multiple values per key and Dictionary just doesn't
> quite cut it. In those situations the solution is simple: just build a
> `Dictionary<TKey, List<TValue>>`!

If you're interested to learn more about the dictionary Ian worked on, take a
look the blog posts he wrote over the summer:

* [Would you like a MultiDictionary?](http://blogs.msdn.com/b/dotnet/archive/2014/06/20/would-you-like-a-multidictionary.aspx)
* [MultiDictionary becomes MultiValueDictionary](http://blogs.msdn.com/b/dotnet/archive/2014/08/05/multidictionary-becomes-multivaluedictionary.aspx)

<http://channel9.msdn.com/Blogs/Charles/Ian-Hays-Building-a-MultiDictionary-Collection-for-NET>

![Ian Hays](IanHays.png)

## Charles Lovell: .NET Portability Analyzer

Charles has been working on a cool Visual Studio extension called the .NET
Portability Analyzer. As developers need to target more and more platforms this
tool can be a big help in analyzing how portable your .NET code is. It gives you
a quick overview of the changes that you would need to make in order to be able
to port your code to a given platform.

More information about the .NET Portability Analyzer:

* [Leveraging existing code across .NET platforms](http://blogs.msdn.com/b/dotnet/archive/2014/08/06/leveraging-existing-code-across-net-platforms.aspx)
* [.NET Portability Analyzer: Visual Studio Extension](http://go.microsoft.com/fwlink/?LinkID=507467&clcid=0x409)

<http://channel9.msdn.com/Blogs/funkyonex/Fun-with-the-Interns-Charles-Lowell-on-the-NET-API-Portability-Analyzer>

![Charles Lovell](CharlesLovell.png)

## Santiago Fernandez Madero: An LLVM based optimizer for MSIL

Santiago really likes being close to the metal. So he investigated what it would
take to use [LLVM] in the [.NET Native] code generator. [LLVM] is a cross
platform, open source collection of modular and reusable compiler and toolchain
technologies.

If you like compilers and low level stuff, watch this interview where Beth
and Santiago geek out:

[LLVM]: http://llvm.org/
[.NET Native]: http://blogs.msdn.com/b/dotnet/archive/tags/dotnetnative/

***TBD: URL***

![Santiago Fernandez Madero](SantiagoFernandezMadero.png)

## Zach Montoya: Visual Studio designer for .NET Native

Speaking about [.NET Native]: how does .NET Native do dynamic things when it's
compiling your code statically? The answer is something called
[Runtime Directives]. Runtime directives are basically additional information
provided to the .NET Native tool chain that tell the compiler what APIs you
intend to call dynamically.

Zach built a Visual Studio extension that allows maintaining and configuring the
[runtime directives] right from Visual Studio. Check out the interview to learn
more about this extension (and on how Beth can tell developers and program
managers apart).

***TBD: URL***

[Runtime Directives]: http://blogs.msdn.com/b/dotnet/archive/2014/05/20/net-native-deep-dive-dynamic-features-in-static-code.aspx

![Zach Montoya](ZachMontoya.png)

## Christian Salgado Pacheco: Cataloging .NET APIs

Keeping track of all the .NET APIs across all the platforms can be challenging.
At Microsoft, we often build internal engineering tools to make our lives
easier. Christian worked on a tool that enables us to catalog the APIs and
record comments and design notes.

<http://channel9.msdn.com/Blogs/funkyonex/Fun-with-the-Interns-Christian-Salgado-Catalogs-NET-APIs>

![Christian Salgado Pacheco](ChristianSalgadoPacheco.png)
