##[LLILC](https://github.com/dotnet/llilc) : An LLVM based compiler for dotnet CoreCLR.

The LLILC project (we pronouce it "lilac") intends to produce MSIL code generators based 
on LLVM and targeting the open source [CoreCLR](https://github.com/dotnet/coreclr) for a 
number of different scenarios.  Our first tool is a JIT for CoreCLR that operates directly 
against the common JIT interface utilized by RyuJIT, the CoreCLR production JIT. Following 
on after the JIT we expect to produce an AOT compiler that will generate stand alone binaries.  

####Why a new JIT for CoreCLR?

While the CoreCLR already has RyuJIT, we saw an opportunity to provide a new code generator 
that has the potential to run across all the targets and platforms supported by LLVM. As part 
of our project we're providing an MSIL reader that operates directly against the same common 
JIT interface as the production RyuJIT. This new JIT will allow any C# program written for the 
.NET Core class libraries to run on any platform that CoreCLR can be ported to and that LLVM 
will target.

####There are several ongoing efforts to complie MSIL in the LLVM community, SharpLang springs to mind. Why build another one?

We think a new effort is useful mostly because of the CoreCLR.  The CoreCLr source base 
reflects years of development and includes a number of building-block components that we 
think the community can take advantage of. This fast bootstrap for C# across multiple platforms 
was the idea that was the genesis of this project and the compelling reason to start a new effort. 
That being said we think there are lots of areas where we can collaborate with current MSIL compliation 
efforts. 

####Why LLVM?

For us LLVM is about cross-platform, community, and time to market. The active community of 
LLVM - just trying to stay current with the dev alias was a revelation! - and its ability to 
operate as both a JIT and as an Ahead-Of-Time (AOT) compiler was especially attractive.  By 
bringing MSIL semantics to LLVM we plan to construct a number of tools that can work against 
CoreCLR or some sub set of its components. We also hope the community will produce tools what 
we haven't thought of yet. 

####Tool roadmap
- CoreCLR JIT
    - Just In Time - A classic JIT. 
	This is expected to be throughput-challenged but will be correct and usable for 
	bringup.  Also possible to use with more optimization enabled as a higher tier JIT
	- Install-time JIT - What .NET calls NGen. 
	 This will be suitable for install-time JITing (LLVM is still slow in a runtime 
	 configuration)
- Ahead of Time compiler.  A build lab compiler that utilizes some shared components from 
CoreCLR but will produce a stand alone executable.

## What's Actually Working

Today on Windows we have the MSIL reader & LLVM JIT implemented well enough to 
compile a significant number of methods in the JIT bringup tests included in 
CoreCLR. In these tests we compile about half the methods and then fall back 
to RyuJIT for cases we can't handle yet.  The testing experience is pretty 
decent for developers.  The tests we run can be seen in the CoreCLR 
[test repo](https://github.com/dotnet/coreclr/tree/master/tests/src/JIT/CodeGenBringUpTests)

On Linux and Mac OSX we've established a build and are putting together the 
mscorlib and test asset dependencies to get testing off-the-ground 
for those platforms.

All tests run against the CoreCLR GC in conservative mode - scans the frame 
for roots - rather than precise mode.  But we don't yet support Exception Handling cases.

##Architecture

Philosophically LLILC is to provide a lean interface between CoreCLR and 
LLVM.  Where posible we rely on preexisting technology from one side or the other.

![JitArch](.\JITArch.png)

For the JIT, when we are compiling on demand, we map the runtime types into
LLVM and then translate MSIL into bitcode.  From there compliation uses LLVM 
infrastructure.

![AOTArch](.\AOTArch.png)

Our AOT diagram is more tentative but the basic outline is that the code generator 
is driven using the same interface and the JIT but that there is a static type system 
behind it and we build up a whole program module with in LLVM and run in a LTO like mode. 
Required runtime components are emitted with the output objs and the platform linker 
then produces the target executable.  There are still a number of open questions 
around issues like generics that need resolution but this is our first stake in 
the ground.

##Using LLVM

In the few months we've been using LLVM we've had a really good experience but with a few caveats.
 Getting started with translating to BitCode has been a very straight forward experience 
and ramp-up time for someone with compiler experience has been very quick.  The MCJIT, which 
we started with for our JIT, was easy to configure and get code compiled and returned 
to the runtime. Outside of the COFF issue discussed below, we only had to make adjustments in 
configuration or straightforward overrides of classes, like EEMemoryManager, to 
enable working code.  Of the caveats, the first was simple, but the other two are 
going to require sustained work to bring up to the level we'd like.  The 
first issue was a problem with Windows support in the DynamicRuntime of the MCJIT 
infrastructure.  The last two, Precise Garbage Collection, and Exception 
Handling, arise because of the different semantics of managed languages.  Luckily 
for us, people in the community have already started working in these areas so we 
don't have to start from zero. 

####COFF dynamic loader support
One of the additions we made to LLVM to unblock ourselves was to implement a 
COFF dynamic loader.  (The patch to add RuntimeDyldCoff.{h,cpp} is 
through review and has been committed).  This was the only addition we 
directly had to make to LLVM to enable bring-up of the code generator.  Once 
this is in, we see a number of bugs in the database around Windows JIT support that 
should be easier to close.   

####Precise Garbage Collection
Precise GC is central to the CoreCLR approach to memory management.  Its 
intent is to keep the overhead of managed memory as low as possible. 
It is assumed by the runtime that the JIT will generate precise information about 
the GC ref lifetimes and provide it with the compiled code for execution. 
To support this we're beginning to use the 
[StatePoint](http://llvm.org/docs/Statepoints.html) approach, with 
additions to convert the standard output format to the custom format 
expected by CoreCLR.
We share some of the same concerns that Philip Reames echoed in the initial 
design of StatePoints.  E.g. preservation of "GCness" through the optimizer 
is critical, but must not block optimizer transformations.  Given this concern 
one of our open questions is how to enable testing to find GC holes that creep 
in, but also enable extra checking that can be opted into if the incoming 
IR contains GC pointers.

There is a more detailed document included in our repo that outlines our more-specific GC plans 
[here](https://github.com/dotnet/llilc/blob/master/Documentation/llilc-gc.md).

####Exception Handling
The MSIL EH model is specific to the CLR as you'd expect, but it descends 
in part conceptually from Windows Structured Exception Handling (SEH).  In 
particular, the implicit exception flow from memory accesses to implement null 
checks, and the use of filters and funclets in the handling of exceptions, 
mirrors SEH. ([here](https://msdn.microsoft.com/en-us/library/ms173162.aspx) 
is an outline of C# EH) Our plans at this point are to add all checks required 
by MSIL as explicit compare/branch/throw sequences to better match C++ EH as well 
as building on the SEH support currently being put into Clang. Then, once we have 
correctness, see if there is a reasonable way forward that improves 
performance.

Like GC, there's a detailed doc outlining our specific issues and plans in the 
repo 
[here](https://github.com/dotnet/llilc/blob/master/Documentation/llilc-jit-eh.md)  

##Future Work

* More platforms.  Today we're running on Windows and starting to build for
Linux and Mac OSX.  We'd like more.
* Complet JIT implementation
    * More MSIL opcodes supported
	* Precise GC support
	* EH support
* Specialized memory allocators for hosted solutions.  CoreCLR has been used as 
a hosted solution (run in process by other programs) but to support this we need 
a better memory allocation story.  The runtime should be able to provide a memory 
allocator that is used for all compilation.
* AOT - Fully flesh out the AOT story.

#### Links

[LLILC Wiki](https://github.com/dotnet/llilc/wiki)

[LLILC Issues](https://github.com/dotnet/llilc/issues)

[CoreCLR](https://github.com/dotnet/coreclr)
