---
post_title: The convenience of System.IO
author1: rlander@microsoft.com
post_slug: the-convenience-of-system-io
microsoft_alias: rlander
featured_image: systemioconvenience.jpg
categories: .NET, C#, Performance
tags: Convenience of .NET
summary: File I/O APIs are used pervasively in apps. .NET has great API for reading and writing files. They are a great example of the convenience of .NET.
post_date: 2023-11-06 10:05:00
---

Reading and writing files is very common, just like other forms of I/O. File APIs are needed for reading configuration, caching data locally, and loading data (from disk) into memory to do some computation like (today's topic) word counting. `File`, `FileInfo`, and `Stream` types do a lot of the heavy lifting for .NET developers needing to access files. In this post, we’re going to look at the convenience and performance of reading files with [System.IO](https://learn.microsoft.com/dotnet/api/system.io).

We recently kicked off a series on the [Convenience of .NET](https://devblogs.microsoft.com/dotnet/the-convenience-of-dotnet/) that describes our approach for providing convenient solutions to common tasks. [The convenience of System.Text.Json](https://devblogs.microsoft.com/dotnet/the-convenience-of-system-text-json/) is another post in the series, about reading and writing JSON documents. [Why .NET?](https://devblogs.microsoft.com/dotnet/why-dotnet/) describes architectural choices that enable the solutions covered in these posts.

This post uses .NET file I/O APIs for counting line, words, and bytes in a large novel. It's a straightforward task that demonstrates the approachability and performance differences between the various file APIs. You'll also see how [native AOT](https://learn.microsoft.com/dotnet/core/deploying/native-aot) can count words even faster.

## The APIs

The following `File` APIs (with their companions) are used in the benchmarks that we're going to analyze.

1. [`File.OpenHandle`](https://learn.microsoft.com/dotnet/api/system.io.file.openhandle) with [`RandomAccess.Read`](https://learn.microsoft.com/dotnet/api/system.io.randomaccess.read#system-io-randomaccess-read(microsoft-win32-safehandles-safefilehandle-system-span((system-byte))-system-int64))
1. [`File.Open`](https://learn.microsoft.com/dotnet/api/system.io.file.open) with [`FileStream.Read`](https://learn.microsoft.com/dotnet/api/system.io.filestream.read#system-io-filestream-read(system-span((system-byte))))
1. [`File.OpenText`](https://learn.microsoft.com/dotnet/api/system.io.file.opentext) with [`StreamReader.ReadLine`](https://learn.microsoft.com/dotnet/api/system.io.streamreader.readline)
1. [`File.ReadLines`](https://learn.microsoft.com/dotnet/api/system.io.file.readlines) with [`IEnumerable<string>`](https://learn.microsoft.com/dotnet/api/system.collections.generic.ienumerable-1)
1. [`File.ReadAllLines`](https://learn.microsoft.com/dotnet/api/system.io.file.readalllines) with [`string[]`](https://learn.microsoft.com/dotnet/api/system.array)

The APIs are listed from highest-control to most convenient. It's OK if they are new to you. It should still be an interesting read.

I also used the new [`SearchValues`](https://learn.microsoft.com/dotnet/api/system.buffers.searchvalues) class to see if it provided a benefit instead of passing a `Span<byte>` to `Span<byte>.IndexOfAny`. It pre-computes the search strategy to avoid the upfront costs of [`.IndexOfAny()`](https://learn.microsoft.com/dotnet/api/system.memoryextensions.indexofany).

Next, we’ll look at an app that has been implemented multiple times — for each of those APIs — testing approachability and efficiency.

## The App

This [app](https://github.com/richlander/convenience/tree/wordcount/wordcount/wordcount) counts lines, words, and bytes in a text file. It is modeled on the behavior of [`wc`](https://en.wikipedia.org/wiki/Wc_(Unix)), a popular tool available on Unix-like systems. I assume "wc" stands for "word count".

I use `wc` a lot. The following use of `wc` tell me how many cores are available on my Linux machine. Each core gets its own line in the `/proc/cpuinfo` file and `-l` counts lines.

```bash
$ cat /proc/cpuinfo | grep "model name" | wc -l
8
$ cat /proc/cpuinfo | grep "model name" | head -n 1
model name	: Intel(R) Core(TM) i7-7700K CPU @ 4.20GHz
$ cat /etc/os-release | head -n 1
NAME="Manjaro Linux"
```

I used that machine for the performance testing in this post. You can see I'm using Manjaro Linux, which is part of the Arch Linux family.

Word counting is an algorithm that requires looking at every character in a file. The counting is done (primarily) by counting spaces.

> A word is a non-zero-length sequence of printable characters delimited by white space.

Credit: [`wc`](https://github.com/coreutils/coreutils/blob/c2173c0a52925245d4fe27890f9ced1b5d860372/src/wc.c#L223-L224).

So, we need to write some code that follows that recipe. First, let's look at the results of the various APIs.

## Results

Each implementation has been measured in terms of:

- Lines of code
- Speed of execution
- Memory use

I used [BenchmarkDotNet](https://benchmarkdotnet.org/index.html) for performance testing. It's a great tool if you've never used it. Writing a benchmark takes about a minute once you know the syntax. It's similar to unit testing in terms of how you write a test.

The benchmarks use [Clarissa Harlowe; or the history of a young lady](https://en.wikipedia.org/wiki/Clarissa) by Samuel Richardson. This text was chosen because it is apparently one of the longest books in the English language and is [freely available on Project Gutenberg](https://www.gutenberg.org/ebooks/author/1959). There's even a [BBC TV adaption](https://www.imdb.com/title/tt0101066/) of it from 1991.

### Lines of code

I love solutions that are easy and approchable. Lines of code is our best proxy metric for that.

<img title="File API lines of code metric" src ="file-api-loc.png" width="75%" />

There are two clusters in this chart, at ~35 lines and another at ~85 lines. You'll see that these benchmarks boil down to two algorithms with some small differences to accomodate the different APIs. In contrast, the [`wc` implementation](https://github.com/coreutils/coreutils/blob/master/src/wc.c) is quite a bit longer, nearing 1000 lines. It does more, however.

I used `wc` to calculate these numbers, again with `-l`.

```bash
$ wc -l *.cs
      34 BenchmarkData.cs
      31 BenchmarkTests.cs
      84 FileOpenBenchmark.cs
      87 FileOpenHandleBenchmark.cs
      85 FileOpenHandleSearchValuesBenchmark.cs
      85 FileOpenTextCharBenchmark.cs
      38 FileOpenTextReadLineBenchmark.cs
      32 FileReadAllLinesBenchmark.cs
      33 FileReadLinesBenchmark.cs
      27 Program.cs
      38 Runner.cs
     574 total
```

### Functional parity with `wc`

Let's validate that my C# implementation matches `wc` (called from the same directory).   

`wc`:

```bash
$  wc ../Clarissa_Harlowe/*
   11716  110023  610515 ../Clarissa_Harlowe/clarissa_volume1.txt
   12124  110407  610557 ../Clarissa_Harlowe/clarissa_volume2.txt
   11961  109622  606948 ../Clarissa_Harlowe/clarissa_volume3.txt
   12168  111908  625888 ../Clarissa_Harlowe/clarissa_volume4.txt
   12626  108593  614062 ../Clarissa_Harlowe/clarissa_volume5.txt
   12434  107576  607619 ../Clarissa_Harlowe/clarissa_volume6.txt
   12818  112713  628322 ../Clarissa_Harlowe/clarissa_volume7.txt
   12331  109787  611792 ../Clarissa_Harlowe/clarissa_volume8.txt
   11771  104934  598265 ../Clarissa_Harlowe/clarissa_volume9.txt
       9     153    1044 ../Clarissa_Harlowe/summary.md
  109958  985716 5515012 total
```

And with [`count`](https://github.com/richlander/convenience/tree/wordcount/wordcount/count), a standalone copy of [`FileOpenHandleSearchValuesBenchmark`](https://github.com/richlander/convenience/blob/wordcount/wordcount/wordcount/FileOpenHandleSearchValuesBenchmark.cs):

```bash
$ dotnet run ../Clarissa_Harlowe
    11716  110023  610515 ../Clarissa_Harlowe/clarissa_volume1.txt
    12124  110407  610557 ../Clarissa_Harlowe/clarissa_volume2.txt
    11961  109622  606948 ../Clarissa_Harlowe/clarissa_volume3.txt
    12168  111908  625888 ../Clarissa_Harlowe/clarissa_volume4.txt
    12626  108593  614062 ../Clarissa_Harlowe/clarissa_volume5.txt
    12434  107576  607619 ../Clarissa_Harlowe/clarissa_volume6.txt
    12818  112713  628322 ../Clarissa_Harlowe/clarissa_volume7.txt
    12331  109785  611792 ../Clarissa_Harlowe/clarissa_volume8.txt
    11771  104934  598265 ../Clarissa_Harlowe/clarissa_volume9.txt
        9     153    1044 ../Clarissa_Harlowe/summary.md
   109958  985714  5515012 total
```

The results are effectively identical. I found that there are one or two special characters in `clarissa_volume8.txt`. It results in a different word count (just 2 words) and I didn't know what the correct behavior was so left that difference alone.

### Speed: Read a page in 7 microseconds

I started by testing a [short summary](https://github.com/richlander/convenience/blob/wordcount/wordcount/Clarissa_Harlowe/summary.md) of the novel. It's just 1k (with 9 lines and 153 words).

```bash
$ dotnet run ../Clarissa_Harlowe/summary.md 
        9     153    1044 ../Clarissa_Harlowe/summary.md
```

Let's see how the fast our APIs can count those words.

<img src="file-api-speed-small-document.png" title="Speed metrics for reading small document using .NET File I/O APIs" width="75%" />

We see some clustering, but I'm going to call this result a tie. There are not a lot of apps where a 2.5 [microsecond](https://en.wikipedia.org/wiki/Unit_of_time#List) gap in performance matters. I wouldn't write 50 additional lines of code for (only) that win.

### Memory: Team `byte` vs Team `string`

Let's look at memory usage for the same small document.

<img src="file-api-memory-small-document.png" title="Memory metrics for reading small document using .NET File I/O APIs" width="75%" />

This difference is dramatic. We see the same clustering again, however it's definitely not a tie between them. You are seeing one cluster of APIs that return bytes and another that return heap-allocated strings.

For context, `1_048_576` bytes is 1 [megabyte (mebibyte)](https://en.wikipedia.org/wiki/Megabyte). `10_000` bytes is 1% of that.

I'm a fan of [integer literals](https://learn.microsoft.com/dotnet/csharp/language-reference/builtin-types/integral-numeric-types#integer-literals) like `1_000` and `0b_0010_1010`. That's why I've used that format, above.

## Speed: This book takes a millisecond to read

Let's see how long it takes to count lines, words, and bytes in [Clarissa_Harlowe volume one](https://github.com/richlander/convenience/blob/wordcount/wordcount/Clarissa_Harlowe/clarissa_volume1.txt).

```bash
$ dotnet run ../Clarissa_Harlowe/clarissa_volume1.txt 
    11716  110023  610515 ../Clarissa_Harlowe/clarissa_volume1.txt
```

Perhaps we'll see a larger separation in performance, grinding through `610_515` bytes of text.

<img src="file-api-speed-large-document.png" title="Speed metrics for reading large document using .NET File I/O APIs" width="75%" />

And we do. The first few APIs return counts in hundreds of microseconds and then we transition to thousands of microseconds, which is the same thing as milliseconds. We don't really have clusters anymore but more of a stair step of behavior and options.

I added a few more benchmarks to the mix to further explore this domain of APIs. There wasn't any value to doing that with the small doc (which I validated by running those tests).

- `FileOpen` and `FileOpenHandle` are both operating over a repeatedly updated `Span<byte>`, read from a stream. That's very efficient.
- `FileOpenHandleSearchValues` takes advantage of the `SearchValues` class. I didn't see a win with the large document, but it pulled ahead (of `FileOpenHandle`) for the small document. You may want to explore that. It is straightforward to use. I didn't use `SearchValues` for any other benchmarks.
- `FileOpenTextChar` is similar, but reads `Span<char>` instead of `Span<byte>`. Characters are potentially easier to work with and come with a little extra cost (of conversion).
- `FileOpenTextReadLine` uses a very common pattern with a `StreamReader`, to read one line of text as a `string` at a time.
- `FileReadLines` is one step higher-level, using an `IEnumerable<string>` to create the same outcome.
- `FileReadAllLines` is a sort of "relaxing lawn chair" API that reads the entire document at once, including paying the upfront cost of finding all the line endings to return a `string[]`.

In terms of usability, I've always gravitated to the code you'll see in `FileReadLines` and `FileOpenTextReadLine`. As we'll soon see, there is a time and a place for those APIs and also for the lower-level ones.

## Memory: It's best to read a page at a time

Based on the speed differences, we're likely to see big memory differences, too.

<img src="file-api-memory-large-document.png" title="Memory metrics for reading large document using .NET File I/O APIs" width="75%" />

This memory chart is similar to one we saw earlier, but even more dramatic. The low-level APIs have a small fixed cost, while the memory requirements of the `string` APIs scale with the size of the document.

Again, `1_048_576` bytes is 1 megabyte. `1_775_316` bytes is the largest size reported in the chart, between 1.5 and 1.75 MBs.

The `FileOpenTextChar` pattern is interesting since is a sort of middle ground option. You don't need to accept the additional complexity of dealing directly with bytes but retain most of the memory lean-ness of the lowest-level options. That might be an interesting option for some scenarios.

## Performance parity with `wc`

I continue to talk about `wc` and how much I like it. I've demonstrated that `System.IO` can be used to produce the same results. Clearly, I should share performance numbers, too! Here, I'll use `time`, another favorite tool.

Let's start with `wc`.

```bash
$ time wc ../Clarissa_Harlowe/clarissa_volume1.txt 
 11716 110023 610515 ../Clarissa_Harlowe/clarissa_volume1.txt

real	0m0.009s
user	0m0.006s
sys	0m0.003s
$ time wc ../Clarissa_Harlowe/*            
  11716  110023  610515 ../Clarissa_Harlowe/clarissa_volume1.txt
  12124  110407  610557 ../Clarissa_Harlowe/clarissa_volume2.txt
  11961  109622  606948 ../Clarissa_Harlowe/clarissa_volume3.txt
  12168  111908  625888 ../Clarissa_Harlowe/clarissa_volume4.txt
  12626  108592  614062 ../Clarissa_Harlowe/clarissa_volume5.txt
  12434  107576  607619 ../Clarissa_Harlowe/clarissa_volume6.txt
  12818  112713  628322 ../Clarissa_Harlowe/clarissa_volume7.txt
  12331  109784  611792 ../Clarissa_Harlowe/clarissa_volume8.txt
  11771  104933  598265 ../Clarissa_Harlowe/clarissa_volume9.txt
      9     153    1044 ../Clarissa_Harlowe/summary.md
 109958  985711 5515012 total

real	0m0.047s
user	0m0.042s
```

That's pretty fast. That's 9 and 47 milliseconds. I'm a bit worried.

Let's try with .NET, using my `FileOpenHandleSearchValuesBenchmark` implementation.

```bash
$ time ./app/count ../Clarissa_Harlowe/clarissa_volume1.txt 
   11716   110023  610515  ../Clarissa_Harlowe/clarissa_volume1.txt

real	0m0.106s
user	0m0.061s
sys	0m0.018s
$ time ./app/count ../Clarissa_Harlowe     
   11716   110023  610515  ../Clarissa_Harlowe/clarissa_volume1.txt
   12124   110407  610557  ../Clarissa_Harlowe/clarissa_volume2.txt
   11961   109622  606948  ../Clarissa_Harlowe/clarissa_volume3.txt
   12168   111908  625888  ../Clarissa_Harlowe/clarissa_volume4.txt
   12626   108593  614062  ../Clarissa_Harlowe/clarissa_volume5.txt
   12434   107576  607619  ../Clarissa_Harlowe/clarissa_volume6.txt
   12818   112713  628322  ../Clarissa_Harlowe/clarissa_volume7.txt
   12331   109785  611792  ../Clarissa_Harlowe/clarissa_volume8.txt
   11771   104934  598265  ../Clarissa_Harlowe/clarissa_volume9.txt
   9       153     1044    ../Clarissa_Harlowe/summary.md
Totals: 109958 985714 5515012

real	0m0.114s
user	0m0.088s
sys	0m0.007s
```

That's no good! Wasn't even close.

That's 106 milliseconds compared to 9 and a 114 compared to 47. Hmmm. It's really interesting that there is almost no difference for .NET for the two scenarios. The runtime startup cost is clearly dominant.

Everyone knows that a managed language runtime cannot keep up with native code on startup. The numbers validate that. If only we had a _native_ managed runtime ...

Oh! We do. That's right. We have native AOT. Let's try it. I published the app as native AOT to another directory.

```bash
$ time ./napp/count ../Clarissa_Harlowe/clarissa_volume1.txt 
    11716  110023  610515 ../Clarissa_Harlowe/clarissa_volume1.txt

real	0m0.007s
user	0m0.005s
sys	0m0.003s
$ time ./napp/count ../Clarissa_Harlowe
    11716  110023  610515 ../Clarissa_Harlowe/clarissa_volume1.txt
    12124  110407  610557 ../Clarissa_Harlowe/clarissa_volume2.txt
    11961  109622  606948 ../Clarissa_Harlowe/clarissa_volume3.txt
    12168  111908  625888 ../Clarissa_Harlowe/clarissa_volume4.txt
    12626  108593  614062 ../Clarissa_Harlowe/clarissa_volume5.txt
    12434  107576  607619 ../Clarissa_Harlowe/clarissa_volume6.txt
    12818  112713  628322 ../Clarissa_Harlowe/clarissa_volume7.txt
    12331  109785  611792 ../Clarissa_Harlowe/clarissa_volume8.txt
    11771  104934  598265 ../Clarissa_Harlowe/clarissa_volume9.txt
        9     153    1044 ../Clarissa_Harlowe/summary.md
   109958  985714  5515012 total

real	0m0.015s
user	0m0.019s
sys	0m0.006s
```

That's 7 milliseconds compared to 9 and 15 compared to 47. That's incredibly competitive!

<img src="native-code-results.png" title="Results of comparing wc and native aot" width="75%" />

Note: I [configured my app](https://learn.microsoft.com/dotnet/core/deploying/native-aot/optimizing#optimize-for-size-or-speed) with `<OptimizationPreference>Speed</OptimizationPreference>`. It provided a benefit.

Let's look at some code.

## `File.ReadLines` and `File.ReadAllLines`

The following benchmarks implement a high-level algorithm based on `string` lines:

- [`FileReadLines`](https://github.com/richlander/convenience/blob/wordcount/wordcount/wordcount/FileReadLinesBenchmark.cs)
- [`FileReadAllLinesBenchmark`](https://github.com/richlander/convenience/blob/wordcount/wordcount/wordcount/FileReadAllLinesBenchmark.cs)

The following code comes from the first benchmark, which uses `foreach` over an `IEnumerable<string>`.

```csharp
public static Count Count(string path)
{
   int wordCount = 0, lineCount = 0, charCount = 0;

   foreach (var line in File.ReadLines(path))
   {
      lineCount++;
      charCount += line.Length;
      bool wasSpace = true;

      foreach (var c in line)
      {
            bool isSpace = Char.IsWhiteSpace(c);

            if (!isSpace && wasSpace)
            {
               wordCount++;
            }

            wasSpace = isSpace;
      }
   }

   return new(lineCount, wordCount, charCount, path);
}
```

The code counts lines and character counts via the outer `foreach`. The inner `foreach` counts words after spaces, looking at every character in the line. It uses `Char.IsWhiteSpace` to determine is a character is whitespace.

This algorithm was the simplest I could come up with. It largely matches `wc` results. The byte counts don't match since this code works on characters not bytes. That means that all the line termination character(s) have been hidden from view. I could have added +1 to the `charCount` per line, but that didn't seem useful to me. Count characters or bytes. Pick one and do it accurately.

## `FileOpenText`

The following benchmark implements a lower-level approach, still based on `string` lines:

- [`FileOpenTextReadLineBenchmark`](https://github.com/richlander/convenience/blob/wordcount/wordcount/wordcount/FileOpenTextReadLineBenchmark.cs)

The `StreamReader` API is a good bit lower-level than `IEnumerable<string>` and `string[]` so I took the opportunity to optimize the algorithm by about the same amount. 

```csharp
public static Count Count(string path)
{
   int wordCount = 0, lineCount = 0, charCount = 0;
   using StreamReader stream = File.OpenText(path);
   char[] space = [' '];

   string? line = null;
   while ((line = stream.ReadLine()) is not null)
   {
      lineCount++;
      charCount += line.Length;
      ReadOnlySpan<char> text = line.TrimStart();

      if (text.Length is 0)
      {
            continue;
      }

      int index = 0;
      while ((index = text.IndexOfAny(space)) > 0)
      {
            wordCount++;
            text = text.Slice(index).TrimStart();
      }

      wordCount++;
   }

   return new(lineCount, wordCount, charCount, path);
}
```

As you can see, the algorithm is just a tad longer. To be fair, I could have used this same approach with the previous two benchmarks, however, I think it makes sense to match the algorithm to the complexity of the primary API being used (if you have multiple to choose from).

`StreamReader.ReadLine` returns a `string?` (AKA, a nullable `string`). It returns `null` when its done reading. You can see how I've handled that with the condition for the `while` loop.

```csharp
char[] space = [' '];
```

That's a [collection expression](https://learn.microsoft.com/dotnet/csharp/whats-new/csharp-12#collection-expressions). I use them as much as I can. In my opinion, they are the best C# feature since records. 

The real win here is that I'm using `IndexOfAny` to skip over as much (non-whitespace) text as possible and then `TrimStart` to do the same with whitespace. My colleagues have invested a lot of effort in making those APIs go fast (by using vector APIs). It delivers a significant speed boost (for the large document).

The core of this algorithm is arguably just five lines (the inner while loop), even shorter than the "simple" algorithm we looked at earlier. However, there is more going on and more concepts to understand, so I consider it more advanced.

## `File.Open` and friends

The following benchmarks implement the lowest-level algorithm, primarily based on bytes.

- [`FileOpenTextCharBenchmark`](https://github.com/richlander/convenience/blob/wordcount/wordcount/wordcount/FileOpenTextCharBenchmark.cs)
- [`FileOpenBenchmark`](https://github.com/richlander/convenience/blob/wordcount/wordcount/wordcount/FileOpenBenchmark.cs)
- [`FileOpenHandleBenchmark`](https://github.com/richlander/convenience/blob/wordcount/wordcount/wordcount/FileOpenHandleBenchmark.cs)
- [`FileOpenHandleSearchValuesBenchmark`](https://github.com/richlander/convenience/blob/wordcount/wordcount/wordcount/FileOpenHandleSearchValuesBenchmark.cs)

These APIs offer a lot more control and are required to match the byte counts that `wc` reports. In particular, these APIs don't hide the line termination characters. The concept of lines is now gone and replaced with a long series of bytes. Bytes might sound scary to work with. For this style of coding, they are pretty tame. You can save your bitmasks for Halloween.

```csharp
public static Count Count(string path)
{
   const byte NEWLINE = (byte)'\n';
   const byte CARRIAGE_RETURN = (byte)'\r';
   const byte SPACE = (byte)' ';
   ReadOnlySpan<byte> searchValues = [SPACE, NEWLINE];

   int wordCount = 0, lineCount = 0, byteCount = 0;
   bool wasSpace = true;

   byte[] buffer = ArrayPool<byte>.Shared.Rent(BenchmarkValues.Size);
   using var stream = File.Open(path, FileMode.Open, FileAccess.Read);

   int count = 0;
   while ((count = stream.Read(buffer)) > 0)
   {                
      byteCount += count;
      Span<byte> bytes = buffer.AsSpan(0, count);

      while (bytes.Length > 0)
      {
            if (bytes[0] is SPACE)
            {
               wasSpace = true;
               bytes = bytes.Slice(1);
               continue;
            }
            else if (bytes[0] is CARRIAGE_RETURN)
            {
               bytes = bytes.Slice(1);
               continue;
            }
            else if (bytes[0] is NEWLINE)
            {
               wasSpace = true;
               bytes = bytes.Slice(1);
               lineCount++;
               continue;
            }
            else if (wasSpace)
            {
               wasSpace = false;
               wordCount++;
            }

            int nextIndex = 0;
            int indexOf = bytes.IndexOfAny(searchValues);

            if (indexOf > -1)
            {
               wasSpace = true;
               nextIndex = indexOf + 1;

               if (bytes[indexOf] is NEWLINE)
               {
                  lineCount++;       
               }
            }
            else
            {
               if (wasSpace)
               {
                  wordCount++;
               }

               wasSpace = false;
               nextIndex = bytes.Length;
            }

            bytes = bytes.Slice(nextIndex);
      }
   }

   ArrayPool<byte>.Shared.Return(buffer);
   return new(lineCount, wordCount, byteCount, path);
}
```

In actuality, this algorithm is really a mix of the two algorithms I showed earlier plus handling the challenge of needing to watch for line termination characters.

The crux of the algorithm are these lines:

```csharp
const byte NEWLINE = (byte)'\n';
const byte SPACE = (byte)' ';
ReadOnlySpan<byte> searchValues = [SPACE, NEWLINE];
```

and this one:

```csharp
int indexOf = bytes.IndexOfAny(searchValues);
```

Here, we're searching a series of bytes for space or newline characters. `IndexOfAny` is able to search for those characters very efficiently and then returns an index to where it found a match. It doesn't tell you which one it found, which is fine since you can just check what the character at that index is. It's critical to not skip over spaces or newline characters. Otherwise, the line and wordcounts would be wrong.

```csharp
ReadOnlySpan<byte> searchValues = [SPACE, NEWLINE];
```

That's another collection expression, being assigned to `searchValues`.

You'll see a number of lines like:

```csharp
bytes = bytes.Slice(1);
```

Visual Studio will suggest that this code can be simplified. I discovered that the [simplication isn't equivalent](https://github.com/dotnet/roslyn/issues/47629) and shows up as a performance regression in benchmarks. It's much less likely to be a problem in apps. FYI.

```csharp
byte[] buffer = ArrayPool<byte>.Shared.Rent(BenchmarkValues.Size);
```

I've used the array pool to avoid creating multiple arrays (each time this method is called). It helps to further reduce the burden on the GC. As you can see from the memory numbers, this algorithm uses very little heap memory.

The `FileOpenTextCharBenchmark` benchmark is effectively identical, but uses characters instead of bytes. It is a little slower and uses a little more memory as a result, but is miles ahead of `FileOpenTextReadLineBenchmark`.

I also tried using the `SearchValues` class, in `FileOpenHandleSearchValuesBenchmark`. It is intended to pay the cost of generating optimal search code up front. Make sure to store it in a static as I've done. I found that it provided a benefit for the smaller document, but not the larger one. I believe that intuitively makes sense. Its value is effectively erased with a larger document.

## Summary

`System.IO` provides effective APIs that cover many use cases. I like how easy it is to create straightforward algorithms with `File.ReadLines`. It works very well for content that that is line-based. `File.Open` and `File.OpenHandle` are great for getting access to the binary content of files and to enable writing the most high-performance algorithms.

Thanks to [David Fowler](https://github.com/davidfowl), [Jan Kotas](https://github.com/jkotas), and [Stephen Toub](https://github.com/stephentoub) for their help contributing to this series.
