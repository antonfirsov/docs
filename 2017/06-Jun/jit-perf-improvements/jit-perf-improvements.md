# Performance Improvements in RyuJIT

RyuJIT is the just-in-time compiler used by .NET Core (on x64 [and now x86](https://github.com/dotnet/announcements/issues/10)) and the 64-bit .NET Framework to compile MSIL bytecode to native machine code when a managed assembly executes.  I'd like to point out some of the past year's improvements that have gone into RyuJIT, and how they make the generated code faster.  What follows is by no means a comprehensive list of RyuJIT optimization improvements, but rather a few hand-picked examples that should make for a fun read and point to some of the issues and pull requests on GitHub that highlight the great community interactions and contributions that have helped shape this work.  Be sure to also check out Stephen Toub's [recent post](https://blogs.msdn.microsoft.com/dotnet/2017/06/07/performance-improvements-in-net-core/) about performance improvements in the runtime and base class libraries, if you haven't already.

This post will be comparing the performance of RyuJIT in .NET Framework 4.6.2 to its performance in .NET Core 2.0 and .NET Framework 4.7.1. Note that .NET Framework 4.7.1 has not yet shipped and I am using an early private build of the product.  The same RyuJIT compiler sources are shared between .NET Core and .NET Framework, so the compiler changes discussed here are present in both .NET Core 2.0 and .NET Framework 4.7.1 builds.

*NOTE: Code examples included in this post use manual [`Stopwatch`](https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.stopwatch?view=netstandard-2.0) invocations, with arbitrarily fixed iteration counts and no statistical analysis, as a zero-dependency way to corroborate known large performance deltas.  The timings quoted below were collected on the same machine, with compared runs executed back-to-back, but even so it would be ill-advised to extrapolate quantitative results; they serve only to confirm that the optimizations improve the performance of the targeted code sequences rather than degrade it.  Active performance work, of course, demands real benchmarking, which comes with a whole host of subtle issues that it is well worth taking a dependency to manage properly.  Andrey Akinshin recently wrote a [great blog post](http://aakinshin.net/blog/post/stephen-toub-benchmarks-part1/) discussing this, with the snippets from Stephen's post as examples, for those interested.*

## Devirtualization

The machine code sequence that the just-in-time compiler emits for a virtual call necessarily involves some degree of indirection, so that the correct target override method can be determined when the machine code executes.  Compared to a direct call, this indirection imposes nontrivial overhead.  RyuJIT can now identify that certain virtual call sites will always have one particular target override, and replace those virtual calls with direct ones. This avoids the overhead of the virtual indirection and, better still, allows inlining the callee method into the callsite, eliminating call overhead entirely and giving optimizations better insight into the effects of the code. This can happen when the target object has sealed type, or when its allocation site is immediately apparent and thus its exact type is known.  This optimization was introduced to RyuJIT in [dotnet/coreclr#9230](https://github.com/dotnet/coreclr/pull/9230); was subsequently improved by [dotnet/coreclr#10192](https://github.com/dotnet/coreclr/pull/10192), [dotnet/coreclr#10432](https://github.com/dotnet/coreclr/pull/10432), and [dotnet/coreclr#10471](https://github.com/dotnet/coreclr/pull/10471); and has plenty more [room for improvement](https://github.com/dotnet/coreclr/issues/9908).
The PRs for the changes include some statistics (e.g. [7.3% of virtual calls in System.Private.CoreLib get devirtualized](https://github.com/dotnet/coreclr/pull/10471#issuecomment-289170334)) and real-world examples (e.g. [this diff in `ConcurrentStack.GetEnumerator()`](https://github.com/dotnet/coreclr/pull/10192#issuecomment-286827037) -- to see the code diff at that link you may have to scroll past the quoted output from `jit-diff`, which is a [tool](https://github.com/dotnet/jitutils/blob/master/doc/diffs.md) we use for assessing compiler change impact.  It reports any code size increase as a "regression", though in this case the code size increases are likely from enabling inlines, which is actually an improvement).  Here's a minimal example to illustrate the optimization in action:

<script src="https://gist.github.com/JosephTremoulet/57bdad096c6a0ee7d70d2ae527639169.js"></script>

Method `Operation.OperateTwice` takes an instance parameter of abstract type `Operation`, and makes two virtual calls to its `Operate` method.
When run with the version of the RyuJIT compiler included in .NET Framework 4.6.2, `OperateTwice` is inlined into `Test.PostDoubleIncrement`, leaving `PostDoubleIncrement` with two virtual calls:

<script src="https://gist.github.com/JosephTremoulet/378b0554b4f59b0d01f19b7200c8a7e0.js"></script>

When run with the version of RyuJIT included in .NET Core 2.0 and .NET Framework 4.7.1, `OperateTwice` is again inlined into `Test.PostDoubleIncrement`, but the JIT can now recognize that the instance argument to the two virtual calls pulled in by that inlining is `PostDoubleIncrement`'s parameter `inc`, which is of sealed type `Increment`.  This allows it to rewrite the virtual calls as direct calls to the `Incremement.Operate` override of `Operation.Operate`, and even inline those calls into `PostDoubleIncrement`, which in turn exposes the fact that this code sequence doesn't modify instance field `input`, allowing the redundant load of it for the return value to be eliminated:

<script src="https://gist.github.com/JosephTremoulet/0f3f0cd1c6dfb301899565ac15386b6a.js"></script>

The optimized version does of course run faster; here's what I see running locally on .NET Framework 4.6.2:

```
00:00:00.7389248
00:00:00.7390185
00:00:00.7343929
00:00:00.7355264
00:00:00.7350114
```

and here's what I see running locally on .NET Core 2.0:

```
00:00:00.4671669
00:00:00.4676545
00:00:00.4683338
00:00:00.4674685
00:00:00.4673269
```

## Enhanced Range Inference

One key goal of JIT compiler optimizations is to reduce the cost of run-time safety checks by eliding the code for them when it can prove they will succeed; this necessarily falls to the JIT since the checks are explicitly dictated by MSIL semantics.  RyuJIT's optimizer accordingly focuses some of its analysis on array index expressions, to see whether it can prove they are in-bounds.  [@mikedn](https://github.com/mikedn)'s change [dotnet/coreclr#9773](https://github.com/dotnet/coreclr/pull/9773) extended this analysis to recognize the common idiom of using an unsigned comparison to check upper and lower bounds in one check (`(uint)i < (uint)a.len` implies both `i >= 0` and `i < a.len` for signed `i`).  The PR [notes](https://github.com/dotnet/coreclr/pull/9773#issuecomment-283800896) how this trimmed the machine code generated for `List.Add` from 68 bytes to 48 bytes, and here's a minimal illustrative example:

<script src="https://gist.github.com/JosephTremoulet/28013a5892c842489df1075adc99232b.js"></script>

Method `Set` validates its `index` argument, and then stores to the array.  The IL generated by the C# compiler for this method looks like so:

<script src="https://gist.github.com/JosephTremoulet/146c00437e6b3ba62ed579d5fc43b0a4.js"></script>

The IL has a `blt.un` instruction for the argument validation, and a subsequent `stelem` instruction for the store that carries an implied bounds check of its own.  When run with the version of the RyuJIT compiler included in .NET Framework 4.6.2, machine instructions are generated for each of these checks; here's what the machine code for the inner loop from the `Main` method (into which `Set` gets inlined) looks like:

<script src="https://gist.github.com/JosephTremoulet/10959abed2788b141ec012a731b6cd44.js"></script>

When run with the version of RyuJIT included in .NET Core 2.0 and .NET Framework 4.7.1, on the other hand, the compiler recognizes that the explicit check for the argument validation ensures that the subsequent check for the `stelem` instruction will always succeed, and omits the redundant check, producing this machine code:

<script src="https://gist.github.com/JosephTremoulet/ad4b381f49dd0763481683bfc833156b.js"></script>

Importantly, this brings the machine code in line with what one might expect from looking at the source code -- a check for argument validation, followed by a store to the backing array.  Also, executing the code reports a speedup as expected -- running on .NET Framework 4.6.2 gives me output like this:

```
00:00:00.4313988
00:00:00.4313209
00:00:00.4320729
00:00:00.4319180
00:00:00.4316375
```

and running on .NET Core 2.0 gives me output like this:

```
00:00:00.3235982
00:00:00.3237021
00:00:00.3250067
00:00:00.3235947
00:00:00.3236944
```

## Finally Cloning

When it comes to exception handling and performance, one key goal is to minimize the cost that exception handling constructs impose on the non-exception path -- if no exceptions are actually raised when the program runs, then (as much as possible) it should run as fast as it would if it didn't have exception handlers at all.  This poses a challenge for `finally` clauses, which execute on both exception and non-exception paths.  In order to correctly support the exception path, the code of the `finally` must be bracketed by some set-up/tear-down code that facilitates being called from the runtime code that handles exception dispatch.  Let's look at an example:

<script src="https://gist.github.com/JosephTremoulet/9f5c885cfac85dd860df769ba6d4d9a8.js"></script>

Method `Update` has a `finally` clause that increments ref parameter `right`.  The actual increment boils down to a single machine instruction (`add      dword ptr [rax], 1`), but interaction with the runtime's exception dispatch mechanism requires 5 extra instructions prior and 3 instructions after.  The exception dispatch code invokes the `finally` handler by calling it, and with the version of RyuJIT included with .NET Framework 4.6.2, the non-exception path of method `Update` similarly uses a `call` instruction to transfer control to the `finally` code.  Here's what the machine code for method `Update` looks like with that version of the compiler:

<script src="https://gist.github.com/JosephTremoulet/cc728b7db5b511487d5a75acd91e0e84.js"></script>

Thanks to change [dotnet/coreclr#8551](https://github.com/dotnet/coreclr/pull/8551), the version of RyuJIT included with .NET Core 2.0 makes a separate copy of the `finally` handler body, which executes on the non-exception path, and only on the non-exception path, and therefore doesn't need any of the code that interacts with the exception dispatch code.  The result (for this simple `finally`) is a simple `inc      dword ptr [rax]` in lieu of the `call` to the `finally` handler code.  Here's what the machine code for method `Update` looks like on .NET Core 2.0:

<script src="https://gist.github.com/JosephTremoulet/5efb192634acff6eae66061c50f8b858.js"></script>

(Note: As mentioned above, .NET Core and .NET Framework share RyuJIT compiler sources.  In the case of this particular optimization, however, since the [`Thread.Abort`](https://docs.microsoft.com/en-us/dotnet/api/system.threading.thread.abort?view=netframework-4.7) mechanism that exists in .NET Framework but not .NET Core requires the optimization to perform [extra work](https://github.com/dotnet/coreclr/pull/8551#issuecomment-267692283) that's not yet implemented, the compiler includes a check that disables this optimization when running on .NET Framework.)

It's worth noting that, in terms of C# source code, this optimization applies not just to `finally` statements, but also to other constructs which are implemented using MSIL `finally` clauses, such as `using` statements and `foreach` statements involving enumerators that implement `IDisposable`.

As usual, the PR reports some stats (e.g. [3,000 affected methods in frameworks libraries](https://github.com/dotnet/coreclr/pull/8551#issuecomment-270448482)).  The example above gives me output like this running on .NET Framework 4.6.2:

```
00:00:00.8864647
00:00:00.8871649
00:00:00.8858654
00:00:00.8844547
00:00:00.8863496
```

and output like this running on .NET Core 2.0:

```
00:00:00.3945198
00:00:00.3943679
00:00:00.3954488
00:00:00.3944719
00:00:00.3948235
00:00:00.3942550
00:00:00.3943774
```

## Shift Count Mask Removal

Generating machine code for bit-shift operations is surprisingly nuanced.  Many software languages and hardware ISAs (and compiler intermediate representations) include bit-shift instructions, but in the case that the nominal shift amount is greater than or equal to the number of bits in the shifted value's type, they have differing conventions (as do different programmer's expectations): some interpret the shift amount modulo the number of bits, some produce the value zero (all bits "shifted out"), and some leave the result unspecified.  MSIL's `shl` and `shr.un` instructions' results are undefined in these cases (perhaps to allow the JIT compiler to simply lower these to corresponding target machine instructions regardless of the target's convention).  C#'s `<<` and `>>` operators, on the other hand, always have a defined result, interpreting the shift amount modulo the number of bits.  To ensure the correct semantics, therefore, the C# compiler must emit explicit MSIL instructions to perform the modulus/bit-mask operation on the shift amount before feeding it to `shl`/`shr`.  Since the x86, x64, and ARM64 ISAs' shift instructions likewise interpret the shift amount modulo the number of bits, on these targets a single hardware instruction can be used for the whole mask+shift sequence emitted by the C# compiler.  [@mikedn](https://github.com/mikedn)'s change [dotnet/coreclr#11594](https://github.com/dotnet/coreclr/pull/11594) taught RyuJIT to recognize these sequences and shrink them down appropriately.  The PR reports [stats](https://github.com/dotnet/coreclr/pull/11594#issuecomment-301268383) showing this firing in 20 different framework assemblies, and, as always, a minimal illustrative example follows:

<script src="https://gist.github.com/JosephTremoulet/952091ca03b43b20ebb20b61508680f1.js"></script>

Method `LeftShift` uses only C#'s `<<` operator, and its IL includes the modulo operation (as `ldc 31` + `and`) to ensure those semantics:

<script src="https://gist.github.com/JosephTremoulet/5a030998c62dc243702e04f78b48831b.js"></script>

Running this with the version of the RyuJIT compiler included with .NET Framework 4.6.2, these masking operations are mechanically translated to hardware `and` instructions; here's what the inner loop of method `Main` (into which `LeftShift` gets inlined) looks like:

<script src="https://gist.github.com/JosephTremoulet/edf58d575a852e32e9b0c896a9b91a9f.js"></script>

Running this with the RyuJIT compiler built from the current `master` branch (this particular change was merged after forking the .NET Core 2.0 release branch, and so will be included in versions after 2.0), on the other hand, the redundant masking is eliminated:

<script src="https://gist.github.com/JosephTremoulet/d168e983dfb83cd503db2bb5f7b7b1ba.js"></script>

Running this example code on .NET Framework 4.6.2, I see output like this:

```
00:00:00.8666592
00:00:00.8644551
00:00:00.8623416
00:00:00.8625029
00:00:00.8621675
```

Running it on .NET Core built from current `master` branch, I see output like this:

```
00:00:00.5767756
00:00:00.5747216
00:00:00.5753256
00:00:00.5747212
00:00:00.5751126
```

## Conclusion

There's a lot of work going on in the JIT.  I hope this small sampling has provided a fun read, and invite anyone interested to join the community pushing this work forward; there's some [documentation on RyuJIT](https://github.com/dotnet/coreclr/blob/master/Documentation/botr/ryujit-overview.md) available, and active work is typically labelled [codegen](https://github.com/dotnet/coreclr/labels/area-CodeGen) and/or [optimization](https://github.com/dotnet/coreclr/labels/optimization).  Performance is a constant focus in JIT work, and we've got some exciting improvements in the pipeline (like [tiered jitting](https://github.com/dotnet/coreclr/issues/4331)), so stay tuned, and let us know what would really help light up your scenarios!
