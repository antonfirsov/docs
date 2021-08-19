---
post_title: 'New .NET 6 APIs driven by the developer community'
username: davifowl@microsoft.com
microsoft_alias: 'davifowl'
featured_image: ./wait.jpg
categories: .NET, ASP.NET, C#
summary: .NET 6 is on it's way, and I wanted to share some of my favorite new APIs that you are going to love.
desired_publication_date: '2021-08-23'
---

.NET 6 is on it's way, and I wanted to share some of my favorite new APIs in .NET and ASP.NET Core that you are going to love. Why are you going to love them? Well becuase they were directly driven by our fantastic .NET developer community! Let's get started!

## Reading & Writing Files

In .NET 6, there's a new low-level API to enable reading/writing of files without using a FileStream. It also supports scatter/gather IO (multiple buffers) and overlapping reads and writes at a given file offset.

```csharp
using var handle = File.OpenHandle("ConsoleApp128.exe");
var length = RandomAccess.GetLength(handle);

Console.WriteLine(length);
```

## Process Path & Id
There are a couple of new ways to access a process path and process id without allocating a new process object:

```csharp
var pid = Environment.ProcessId;
var path = Environment.ProcessPath;

Console.WriteLine(pid);
Console.WriteLine(path);
```

## CSPNG (Cryptographically Secure Pseudorandom Number Generator)
Generating random numbers from a CSPNG (Cryptographically Secure Pseudorandom Number Generator) is a easier than ever:

```csharp
// Give me 200 random bytes
var bytes = RandomNumberGenerator.GetBytes(200);
```

## Parallel.ForEachAsync
We finally added Parallel.ForEachAsync, a way to schedule asynchronous work that allows you to control the degree of parallelism:

```csharp
var urlsToDownload = new [] 
{
    "https://dotnet.microsoft.com",
    "https://www.microsoft.com",
    "https://twitter.com/davidfowl"
};

var client = new HttpClient();

await Parallel.ForEachAsync(urlsToDownload, async (url, token) =>
{
    var targetPath = Path.Combine(Path.GetTempPath(), "http_cache", url);
    
    var response = await client.GetAsync(url);
    
    if(response.IsSuccessStatusCode)
    {
        using var target = File.OpenWrite(targetPath);
        
        await response.Content.CopyToAsync(target);
    }
});
```

## Configuration Helpers
We added a helper to make it easier to throw if a required section of configuration is missing:

```csharp
var configuration = new ConfigurationManager();
var options = new MyOptions();

// This will throw if the section isn't configured
configuration.GetRequiredSection("MyOptions").Bind(options);

class MyOptions
{
    public string? SettingValue { get; set;}
}
```

## LINQ
There's a ton of new LINQ methods as well. It got lots of love in this release. Here's a new helper to chunk any IEnumerable<T> into batches:

```csharp
int chunkNumber = 1;
foreach (int[] chunk in Enumerable.Range(0, 9).Chunk(3))
{
    Console.WriteLine($"Chunk {chunkNumber++}");
    foreach (var item in chunk)
    {
        Console.WriteLine(item);
    }
}
```

## Even More LINQ!
More LINQ! There are now MaxBy and MinBy methods:

```csharp
var people = GetPeople();

var oldest = people.MaxBy(p => p.Age);
var youngest = people.MinBy(p => p.Age);

Console.WriteLine($"The oldest person is {oldest.Age}");
Console.WriteLine($"The youngest person is {youngest.Age}");

public record Person(string Name, int Age);
```
  
## Power of 2
Don't keep bit math in your head? Me neither, here are some new helpers for working with powers of 2:

```csharp
using System.Numerics;

uint bufferSize = 235;
if(!BitOperations.IsPow2(bufferSize))
{
    bufferSize = BitOperations.RoundUpToPower2(bufferSize);
}

Console.WriteLine(bufferSize);
```

## WaitAsync Improvements
There's now a much easier (and properly implemented) way to wait for task to complete asynchronously. The following code will yield the await if it hasn't completed in 10 seconds. The operation might still be running! This is for un-cancellable operations!
  
```csharp
var oprationTask = SomeLongRunningOperationAsync();

await operationTask.WaitAsync(TimeSpan.FromSeconds(10));
```

## ThrowIfNull
No more having to check for `null` in every method before you throw an exception. It is now just one line of code.

```csharp
void DoSomethingUseful(object obj)
{
    ArgumentNullException.ThrowIfNull(obj);
}
```

## Working with NativeMemory
If you feel like using C APIs to allocate memory because you're a l33t hacker or you need to need to allocate native memory, the look no further. Don't forget to free!

```csharp
using System.Runtime.InteropServices;

unsafe
{
    byte* buffer = (byte*)NativeMemory.Alloc(100);

    NativeMemory.Free(buffer);
}
```

## Posix Signal Handling
Native support for Posix signal handling is here and we also emulate a couple of signals on Windows.

```csharp
using System.Runtime.InteropServices;

var tcs = new TaskCompletionSource();

PosixSignalRegistration.Create(PosixSignal.SIGTERM, context =>
{
    Console.WriteLine($"{context.Signal} fired");
    tcs.TrySetResult();
});

await tcs.Task;
```

## New Metrics API

We added an entirely new metrics API based on @opentelemetry in .NET 6. It supports dimensions, is *super* efficient and will have exporters for popular metric sinks (prometheus etc).

```csharp
using System.Diagnostics.Metrics;

// This is how you produce metrics

var meter = new Meter("Microsoft.AspNetCore", "v1.0");
Counter<int> counter = meter.CreateCounter<int>("Requests");

var app = WebApplication.Create(args);

app.Use((context, next) =>
{
    counter.Add(1, KeyValuePair.Create<string, object?>("path", context.Request.Path.ToString()));
    return next(context);
});

app.MapGet("/", () => "Hello World");
```
  
You can even listen in and meter:

```csharp
var listener = new MeterListener();
listener.InstrumentPublished = (instrument, meterListener) =>
{
    if(intrument.Name == "Requests" && instrument.Meter.Name == "Microsoft.AspNetCore")
    {
        meterListener.EnableMeasurementEvents(instrument, null);
    })
};

listener.SetMeasurementEventCallback<int>((instrument, measurement, tags, state) =>
{
    Console.WriteLine($"Instrument: {instrument.Name} has recorded the measurement: {measurement}");
});

listener.Start();
```

## Modern Timer API
Last but not least, a modern timer API (I think this is the 5th timer API in .NET now). It's fully async and isn't plagued by the types of gotchas the other timers are plagued with like object lifetime issues, no asynchronous callbacks etc.

```csharp
var timer = new PeriodicTimer(TimeSpan.FromSeconds(1));

white(await timer.WaitForNextTickAsync())
{
    Console.WriteLine(DateTime.UtcNow);
}
```

## Summary

This is just a sampling of the new APIs coming in .NET 6, for more information take a look at the [.NET 6 release notes API diffs](https://github.com/dotnet/core/tree/main/release-notes/6.0/preview/api-diff). Also, Stephen just wrote a spectacular blog on [performance improvements in .NET6](https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-6/) so be sure to give that a read. Finally, donn't forget to [download .NET 6 Preview](https://dot.net/download) and try them out today.
