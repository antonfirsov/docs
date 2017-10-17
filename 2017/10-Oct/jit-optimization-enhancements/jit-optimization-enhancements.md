# RyuJIT Optimization Enhancements

I'd like to tell you about some of the recent changes we've made as part of
our ongoing work to extend the optimization capabilities of RyuJIT, the
MSIL-to-native code generator used by .NET Core and .NET Framework.  I hope it
will make for an interesting read, and offer some insight into the sorts of
optimization opportunities we have our eyes on.

*Note: The changes described here landed after the release fork for
.NET Core 2.0 was created, so they are available in [daily preview
builds](https://github.com/dotnet/core/blob/master/daily-builds.md)
but not the [released 2.0 bits](https://github.com/dotnet/core/blob/master/release-notes/download-archive.md).*

*Similarly, these changes landed after the fork for
[.NET Framework 4.7.1](https://blogs.msdn.microsoft.com/dotnet/2017/08/07/welcome-to-the-net-framework-4-7-1-early-access/)
was created.  The changes to struct argument passing and block layout, which are
purely JIT changes, will automatically propagate to subsequent .NET Framework
releases with the new JIT bits (the RyuJIT sources are shared between .NET Core
and .NET Framework); the other changes depend on their runtime components to
propagate to .NET Framework.*

## Improvements for Span<T>

Some of our work was motivated by [the introduction of `Span<T>`](https://github.com/dotnet/corefxlab/blob/master/docs/specs/span.md),
so that it and similar types could better deliver on their performance promises.

One such change was [#10910](https://github.com/dotnet/coreclr/pull/10910), which
made the JIT recognize the `Item` property getters of `Span<T>` and `ReadOnlySpan<T>`
as intrinsics -- the JIT now recognizes calls to these getters and, rather than
generate code for them the same way it would for other calls, it transforms them
directly into code sequences in its intermediate representation that are similar
to the sequences used for the `ldelem` MSIL opcode that fetches an element from
an array.  As noted in the PR's [performance assessment](https://github.com/dotnet/coreclr/pull/10910#issuecomment-293511168)
(n.b., if you follow that link, see also the [follow-up](https://github.com/dotnet/coreclr/pull/10910#issuecomment-294051549)
where the initially-discovered regressions were fixed with subsequent improvements
in [#10956](https://github.com/dotnet/coreclr/pull/10956) and [dotnet/roslyn#20548](https://github.com/dotnet/roslyn/pull/20548)),
this improved several benchmarks in the
[tests](https://github.com/dotnet/coreclr/tree/master/tests/src/JIT/Performance/CodeQuality/Span)
we added to track `Span<T>` performance, by allowing the existing JIT code that
optimized array bound checks that are redundant with prior checks, or that are
against arrays with known constant length, to kick in for `Span<T>` as well.  This
is what some of those improved benchmark methods look like, and their improvements:

<sscript src="https://gist.github.com/JosephTremoulet/e13e5385ffe7784612c77dad6ea6a79e.js">[link to gist](https://gist.github.com/JosephTremoulet/e13e5385ffe7784612c77dad6ea6a79e#file-indexerbench-snippets-cs)</sscript>

Building on that, change [#11521](https://github.com/dotnet/coreclr/pull/11521)
updated the analysis machinery the JIT uses to eliminate bounds checks for other
provably in-bounds array accesses, to similarly eliminate bounds checks for
provably in-bounds `Span<T>` accesses (in particular, bounds checks in `for` loops
bounded by `span.Length`).  As
[noted](https://github.com/dotnet/coreclr/pull/11521#issuecomment-300658768) in
the PR (numbers [here](https://github.com/dotnet/coreclr/pull/11521#issuecomment-301594380)),
this brought the codegen for four more microbenchmarks in the [`Span<T>` tests](https://github.com/dotnet/coreclr/tree/master/tests/src/JIT/Performance/CodeQuality/Span)
up to par with the codegen for equivalent patterns with arrays; here are two of them:

<sscript src="https://gist.github.com/JosephTremoulet/41548f0b3601199f75ab2dd32b679494.js">[link to gist](https://gist.github.com/JosephTremoulet/41548f0b3601199f75ab2dd32b679494#file-indexerbench-snippets-cs)</sscript>

One key fact that these bounds-check removal optimizations exploit is that array
lengths are immutable; any two loads of `a.Length`, if `a` refers to the same
array each time, will load the same length value.  It's common for the JIT to
encounter different accesses to the same array, where the reference to the
array is held in a local or parameter of type `T[]`, such that it can determine
that intervening code hasn't modified the local/parameter in question, even if
that intervening code has unknown side-effects.  The same isn't true for
parameters of type `ref T[]`, since intervening code with unknown side-effects
might change which array object is referenced.  Consider:

<sscript src="https://gist.github.com/JosephTremoulet/f31c6fc24ab04e2e9643adbcd8015dc0.js">[link to gist](https://gist.github.com/JosephTremoulet/f31c6fc24ab04e2e9643adbcd8015dc0#file-example-cs)</sscript>

Since `Span<T>` is a struct, some platforms' ABIs specify that passing an
argument of type `Span<T>` actually be done by creating a copy of the struct
in the caller's stack frame, and passing a pointer to that copy in to the
callee via the argument registers/stack.  The JIT's internal modeling of this
convention is to rewrite `Span<T>` parameters as `ref Span<T>` parameters.  That
internal rewrite at first caused problems for applying bounds-check removal
optimizations to spans passed as parameters.  The problem was that methods
written with by-value `Span<T>` parameters, which at source look analogous to
by-value array parameter `a` in the example above, when rewritten looked to the
JIT like by-reference parameters, analogous to by-reference array parameter `b`
above.  This caused the JIT to handle references to such parameters' `Length`
fields with the same conservativism needed for `b` above.  Change [#10453](https://github.com/dotnet/coreclr/pull/10453)
taught the JIT to make local copies of such parameters before doing that rewrite
(in beneficial cases), so that bounds-check removal optimizations can equally
apply to spans passed by value.  As [noted](https://github.com/dotnet/coreclr/pull/10453#issuecomment-297835320)
in the PR, this change allowed these optimizations to fire in 9 more of the
[`Span<T>` micro-benchmarks](https://github.com/dotnet/coreclr/tree/master/tests/src/JIT/Performance/CodeQuality/Span)
in our test suite; here are three of them:

<sscript src="https://gist.github.com/JosephTremoulet/10f80bd5a1cbd72e577d503148116d91.js">[link to gist](https://gist.github.com/JosephTremoulet/10f80bd5a1cbd72e577d503148116d91#file-indexerbench-snippets-cs)</sscript>

This last change applies more generally to any structs passed as parameters (not
just `Span<T>`); the JIT is now better able to analyze value propagation through
their fields.

## Enum.HasFlag Optimization

The [`Enum.HasFlag` method](https://docs.microsoft.com/en-us/dotnet/api/system.enum.hasflag?view=netstandard-2.0)
offers nice readability (compare
  `targets.HasFlag(AttributeTargets.Class | AttributeTargets.Struct)` vs
  `targets & (AttributeTargets.Class | AttributeTargets.Struct) == (AttributeTargets.Class | AttributeTargets.Struct)`),
but, since it needs to handle reflection cases where the exact enum type isn't
known until run-time, it is
[notoriously](https://stackoverflow.com/questions/7368652/what-is-it-that-makes-enum-hasflag-so-slow)
expensive.  Change [#13748](https://github.com/dotnet/coreclr/pull/13748) taught
the JIT to recognize when the enum type is known (and known to equal the argument
type) at JIT time, and generate the simple bit test rather than the expensive
`Enum.HasFlag` call.  Here's a micro-benchmark to demonstrate, comparing .NET
Core 2.0 (which doesn't have this change) to a recent daily preview build (which
does).  Much thanks to [@adamsitnik](https://github.com/adamsitnik) for
[making it easy](https://github.com/dotnet/BenchmarkDotNet/blob/master/docs/guide/Configs/Toolchains.md#custom-net-core-runtime)
to use [BenchmarkDotNet](http://www.benchmarkdotnet.org) with
[daily preview builds](https://github.com/dotnet/core/blob/master/daily-builds.md)
of .NET Core!

<sscript src="https://gist.github.com/JosephTremoulet/06f275ca02c1f78835d3c3d597a30c33.js">[link to gist](https://gist.github.com/JosephTremoulet/06f275ca02c1f78835d3c3d597a30c33#file-hasflagbench-cs)</sscript>

Output:

``` ini
BenchmarkDotNet=v0.10.9.313-nightly, OS=Windows 10 Redstone 2 [1703, Creators Update] (10.0.15063)
Processor=Intel Core i7-4790 CPU 3.60GHz (Haswell), ProcessorCount=8
Frequency=3507517 Hz, Resolution=285.1020 ns, Timer=TSC
.NET Core SDK=2.1.0-preview1-007228
  [Host]     : .NET Core 2.1.0-preview1-25719-04 (Framework 4.6.25718.02), 64bit RyuJIT
  Job-WFNGKY : .NET Core 2.0.0 (Framework 4.6.00001.0), 64bit RyuJIT
  Job-VIXUQP : .NET Core 2.1.0-preview1-25719-04 (Framework 4.6.25718.02), 64bit RyuJIT
```
 |  Method |                         Toolchain |        Mean |     Error |    StdDev |
 |-------- |---------------------------------- |------------:|----------:|----------:|
 | HasFlag |                     .NET Core 2.0 | 14,917.4 ns | 80.147 ns | 71.048 ns |
 | HasFlag | .NET Core 2.1.0-preview1-25719-04 |    449.3 ns |  1.239 ns |  1.034 ns |


With the cool new [BenchmarkDotNet DisassemblyDiagnoser](http://adamsitnik.com/Disassembly-Diagnoser/) (again
thanks to [@adamsitnik](https://github.com/adamsitnik)), we can see that the
optimized code really is a simple bit test:

<table>
<thead>
<tr><th colspan="2">Bench.HasFlag</th></tr>
<tr>
<th>RyuJIT x64 .NET Core 2.0</th>
<th>RyuJIT x64 .NET Core 2.1.0-preview1-25719-04</th>
</tr>
</thead>
<tbody>
<tr>
<td style="vertical-align:top;"><pre><code>HasFlagBench.Bench.HasFlag():
push    rdi
push    rsi
push    rbx
sub     rsp,20h
mov     rsi,rcx
xor     edi,edi
L1:
mov rcx, [[AttributeTargets type]]
call    [[box]]
mov     rbx,rax
mov rcx, [[AttributeTargets type]]
call    [[box]]
mov     ecx,dword ptr [rsi+8]
mov     dword ptr [rbx+8],ecx
mov     rcx,rbx
mov     dword ptr [rax+8],0Ch
mov     rdx,rax
call    [[System.Enum.HasFlag]]
mov     byte ptr [rsi+0Ch],al
inc     edi
cmp     edi,3E8h
jl      L1
add     rsp,20h
pop     rbx
pop     rsi
pop     rdi
ret</code></pre></td>
<td style="vertical-align:top;"><pre><code>HasFlagBench.Bench.HasFlag():
xor     eax,eax
mov     edx,dword ptr [rcx+8]
L1:
mov     r8d,edx
and     r8d,0Ch
cmp     r8d,0Ch
sete    r8b
mov     byte ptr [rcx+0Ch],r8b
inc     eax
cmp     eax,3E8h
jl      L1
ret</code></pre></td>
</tr>
</tbody>
</table>

What's more, implementing this optimization involved
[implementing](https://github.com/dotnet/coreclr/pull/13815) a new scheme
for recognizing intrinsics in the JIT, which is
[more flexible](https://github.com/dotnet/coreclr/issues/13813) than the previous
scheme, and which [is being leveraged](https://github.com/dotnet/coreclr/pull/14020)
in the implementation of [Intel SIMD intrinsics for.NET Core](https://github.com/dotnet/corefx/issues/22940).

## Block Layout for Search Loops

Outside of [profile-guided optimization](https://docs.microsoft.com/en-us/dotnet/framework/tools/mpgo-exe-managed-profile-guided-optimization-tool),
the JIT has traditionally been conservative about rearranging the basic blocks
of methods it compiles, leaving them in MSIL order except to segregate code it
identifies as "rarely-run" (e.g. blocks that throw or catch exceptions).  Of
course, MSIL order isn't always the most performant one; notably, in the case
of loops with conditional exits/returns, it's generally a good idea to keep
the in-loop code together, and move everything on the exit path after the
conditional branch out of the loop.  For particularly hot loops, this can cause
a significant enough difference that developers have
[been](https://github.com/dotnet/coreclr/issues/9692#issuecomment-307262693)
[using](https://github.com/dotnet/coreclr/pull/2667#discussion-diff-49820503)
[gotos](https://github.com/dotnet/coreclr/pull/9213#pullrequestreview-22413856)
to make the MSIL order reflect the desired machine code order.  Change
[#13314](https://github.com/dotnet/coreclr/pull/13314) updated the JIT's loop
detection to effect this layout automatically.  As usual, the PR included a
[performance assessment](https://github.com/dotnet/coreclr/pull/13314#issuecomment-321576769),
which noted speed-ups in 5 of the benchmarks in our
[performance test suite](https://github.com/dotnet/coreclr/tree/master/tests/src/JIT/Performance/CodeQuality).
Again comparing .NET Core 2.0 (which didn't have this change) to a recent daily
preview build (which does), let's look at the effect on the repro case from the
[GitHub issue](https://github.com/dotnet/coreclr/issues/9692) describing this
opportunity:

<sscript src="https://gist.github.com/JosephTremoulet/589f42fbdf7e71511b24aca259b56b81.js">[link to gist](https://gist.github.com/JosephTremoulet/589f42fbdf7e71511b24aca259b56b81#file-loopwithexit-cs)</sscript>

The results confirm that the new JIT brings the performance of the loop with the
in-place `return` in line with the performance of the loop with the `goto`, and
that doing so constituted a 15% speed-up:

``` ini

BenchmarkDotNet=v0.10.9.313-nightly, OS=Windows 10 Redstone 2 [1703, Creators Update] (10.0.15063)
Processor=Intel Core i7-4790 CPU 3.60GHz (Haswell), ProcessorCount=8
Frequency=3507517 Hz, Resolution=285.1020 ns, Timer=TSC
.NET Core SDK=2.1.0-preview1-007228
  [Host]     : .NET Core 2.0.0 (Framework 4.6.00001.0), 64bit RyuJIT
  Job-NHAVNC : .NET Core 2.0.0 (Framework 4.6.00001.0), 64bit RyuJIT
  Job-CTEHPT : .NET Core 2.1.0-preview1-25719-04 (Framework 4.6.25718.02), 64bit RyuJIT


```
 |     Method |                         Toolchain |     Mean |     Error |    StdDev |
 |----------- |---------------------------------- |---------:|----------:|----------:|
 | LoopReturn |                     .NET Core 2.0 | 61.97 ns | 0.1254 ns | 0.1111 ns |
 |   LoopGoto |                     .NET Core 2.0 | 53.63 ns | 0.5171 ns | 0.4837 ns |
 | LoopReturn | .NET Core 2.1.0-preview1-25719-04 | 53.75 ns | 0.5089 ns | 0.4511 ns |
 |   LoopGoto | .NET Core 2.1.0-preview1-25719-04 | 53.52 ns | 0.0999 ns | 0.0934 ns |

Disassembly confirms that the difference is entirely block placement:

<table>
<thead>
<tr><th colspan="2">LoopWithExit.LoopReturn</th></tr>
<tr>
<th>RyuJIT x64  .NET Core 2.0</th>
<th>RyuJIT x64  .NET Core 2.1.0-preview1-25719-04</th>
</tr>
</thead>
<tbody>
<tr>
<td style="vertical-align:top;"><pre><code>LoopLayoutBench.LoopWithExit.LoopReturn_(System.String, System.String):
sub     rsp,18h
xor     eax,eax
mov     qword ptr [rsp+10h],rax
mov     qword ptr [rsp+8],rax
mov     ecx,dword ptr [rdx+8]
mov     qword ptr [rsp+10h],rdx
mov     rax,rdx
test    rax,rax
je      L1
add     rax,0Ch
L1:
mov     qword ptr [rsp+8],r8
mov     rdx,r8
test    rdx,rdx
je      L2
add     rdx,0Ch
L2:
test    ecx,ecx
je      L5
L3:
movzx   r8d,word ptr [rax]
movzx   r9d,word ptr [rdx]
cmp     r8d,r9d
je      L4
xor     eax,eax
add     rsp,18h
ret
L4:
add     rax,2
add     rdx,2
dec     ecx
test    ecx,ecx
jne     L3
L5:
mov     eax,1
add     rsp,18h
ret</code></pre></td>
<td style="vertical-align:top;"><pre><code>LoopLayoutBench.LoopWithExit.LoopReturn_(System.String, System.String):
sub     rsp,18h
xor     eax,eax
mov     qword ptr [rsp+10h],rax
mov     qword ptr [rsp+8],rax
mov     eax,dword ptr [rdx+8]
mov     qword ptr [rsp+10h],rdx
test    rdx,rdx
je      L1
add     rdx,0Ch
L1:
mov     qword ptr [rsp+8],r8
mov     rcx,r8
test    rcx,rcx
je      L2
add     rcx,0Ch
L2:
test    eax,eax
je      L4
L3:
movzx   r8d,word ptr [rdx]
movzx   r9d,word ptr [rcx]
cmp     r8d,r9d
jne     L5
add     rdx,2
add     rcx,2
dec     eax
test    eax,eax
jne     L3
L4:
mov     eax,1
add     rsp,18h
ret
L5:
xor     eax,eax
add     rsp,18h
ret</code></pre></td>
</tr>
</tbody>
</table>
<table>
<thead>
<tr><th colspan="2">LoopWithExit.LoopGoto</th></tr>
<tr>
<th>RyuJIT x64  .NET Core 2.0</th>
<th>RyuJIT x64  .NET Core 2.1.0-preview1-25719-04</th>
</tr>
</thead>
<tbody>
<tr>
<td style="vertical-align:top;"><pre><code>LoopLayoutBench.LoopWithExit.LoopGoto_(System.String, System.String):
sub     rsp,18h
xor     eax,eax
mov     qword ptr [rsp+10h],rax
mov     qword ptr [rsp+8],rax
mov     eax,dword ptr [rcx+8]
mov     qword ptr [rsp+10h],rcx
test    rcx,rcx
je      L1
add     rcx,0Ch
L1:
mov     qword ptr [rsp+8],rdx
test    rdx,rdx
je      L2
add     rdx,0Ch
L2:
test    eax,eax
je      L4
L3:
movzx   r8d,word ptr [rcx]
movzx   r9d,word ptr [rdx]
cmp     r8d,r9d
jne     L5
add     rcx,2
add     rdx,2
dec     eax
test    eax,eax
jne     L3
L4:
mov     eax,1
add     rsp,18h
ret
L5:
xor     eax,eax
add     rsp,18h
ret</code></pre></td>
<td style="vertical-align:top;"><pre><code>LoopLayoutBench.LoopWithExit.LoopGoto_(System.String, System.String):
sub     rsp,18h
xor     eax,eax
mov     qword ptr [rsp+10h],rax
mov     qword ptr [rsp+8],rax
mov     eax,dword ptr [rcx+8]
mov     qword ptr [rsp+10h],rcx
test    rcx,rcx
je      L1
add     rcx,0Ch
L1:
mov     qword ptr [rsp+8],rdx
test    rdx,rdx
je      L2
add     rdx,0Ch
L2:
test    eax,eax
je      L4
L3:
movzx   r8d,word ptr [rcx]
movzx   r9d,word ptr [rdx]
cmp     r8d,r9d
jne     L5
add     rcx,2
add     rdx,2
dec     eax
test    eax,eax
jne     L3
L4:
mov     eax,1
add     rsp,18h
ret
L5:
xor     eax,eax
add     rsp,18h
ret</code></pre></td>
</tr>
</tbody>
</table>

## Conclusion

We're constantly pushing to improve our codegen, whether it's to enable new
scenarios/features (like `Span<T>`), or to ensure good performance for
natural/readable code (like calls to `HasFlag` and returns from loops).  As
always, we invite anyone interested to join the community pushing this work
forward.  RyuJIT documentation avilable online includes an
[overview](https://github.com/dotnet/coreclr/blob/master/Documentation/botr/ryujit-overview.md)
and a [recently](https://github.com/dotnet/coreclr/pull/13079) added
[tutorial](https://github.com/dotnet/coreclr/blob/master/Documentation/botr/ryujit-tutorial.md),
and our [GitHub issues](https://github.com/dotnet/coreclr/labels/area-CodeGen)
are open for (and full of) active discussions!
