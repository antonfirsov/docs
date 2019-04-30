# Migrating Delegate.BeginInvoke Calls for .NET Core
*By Mike Rousos*

I recently worked with a couple customers migrating applications to .NET Core that had to make code changes to workaround `BeginInvoke` and `EndInvoke` methods on delegates not being supported on .NET Core. In this post, we'll look at why these APIs aren't implemented for .NET Core, why their usage isn't caught by the [.NET API Portability Analyzer](https://github.com/Microsoft/dotnet-apiport), and how to fix code using them to work with .NET Core.

## About the APIs

As explained in [.NET documentation](https://docs.microsoft.com/dotnet/standard/asynchronous-programming-patterns/calling-synchronous-methods-asynchronously), the `BeginInvoke` method on delegate types allows them to be invoked asynchronously. `BeginInvoke` immediately (without waiting for the delegate to complete) returns an `IAsyncResult` object that can be used later (by calling `EndInvoke`) to wait for the call to finish and receive its return value.

For example, this code calls the `DoWork` method in the background:

```CSharp
delegate int WorkDelegate(int arg);
...
WorkDelegate del = DoWork;

// Calling del.BeginInvoke starts executing the delegate on a
// separate ThreadPool thread
Console.WriteLine("Starting with BeginInvoke");
var result = del.BeginInvoke(11, WorkCallback, null);

// This writes output to the console while DoWork is running in the background
Console.WriteLine("Waiting on work...");

// del.EndInvoke waits for the delegate to finish executing and 
// gets its return value
var ret = del.EndInvoke(result);
```

**The [Asynchronous Programming Model (APM)](https://docs.microsoft.com/dotnet/standard/asynchronous-programming-patterns/asynchronous-programming-model-apm) (using `IAsyncResult` and `BeginInvoke`) is no longer the preferred method of making asynchronous calls.** The [Task-based Asynchronous Pattern (TAP)](https://docs.microsoft.com/dotnet/standard/asynchronous-programming-patterns/task-based-asynchronous-pattern-tap) is the recommended async model as of .NET Framework 4.5. Because of this, and because the implementation of async delegates depends on remoting features not present in .NET Core, `BeginInvoke` and `EndInvoke` delegate calls are not supported in .NET Core. This is discussed in GitHub issue [dotnet/corefx #5940](https://github.com/dotnet/corefx/issues/5940).

Of course, existing .NET Framework code can continue to use `IAsyncResult` async patterns, but running that code on .NET Core will result in an exception similar to this at runtime:

```
Unhandled Exception: System.PlatformNotSupportedException: Operation is not supported on this platform.
   at BeginInvokeExploration.Program.WorkDelegate.BeginInvoke(Int32 arg, AsyncCallback callback, Object object)
```

## Why doesn't ApiPort catch this?

There are other APIs that are supported on .NET Framework that aren't supported on .NET Core, of course. What made this one especially confusing for the customers I worked with was that the [.NET API Portability Analyzer](https://github.com/Microsoft/dotnet-apiport) didn't mention the incompatibility in its report. Following our [migration guidance](https://docs.microsoft.com/dotnet/core/porting/), the customers had run the API Port tool to spot any APIs used by their projects that weren't available on .NET Core. `BeginInvoke` and `EndInvoke` weren't reported.

The reason for this is that `BeginInvoke` and `EndInvoke` methods on user-defined delegate types aren't actually defined in .NET Framework libraries. Instead, these methods are emitted by the compiler (see the 'Important' note in [Asynchronous Programming Using Delegates](https://docs.microsoft.com/dotnet/standard/asynchronous-programming-patterns/asynchronous-programming-using-delegates)) as part of building code that declares a delegate type.

In the code above, the `WorkDelegate` delegate type is declared in the C# code. The IL of the compiled library includes `BeginInvoke` and `EndInvoke` methods, added by the compiler:

```IL
.class auto ansi sealed nested private WorkDelegate
        extends [mscorlib]System.MulticastDelegate
{
.method public hidebysig specialname rtspecialname 
        instance void  .ctor(object 'object',
                                native int 'method') runtime managed
{
} // end of method WorkDelegate::.ctor

.method public hidebysig newslot virtual 
        instance int32  Invoke(int32 arg) runtime managed
{
} // end of method WorkDelegate::Invoke

.method public hidebysig newslot virtual 
        instance class [mscorlib]System.IAsyncResult 
        BeginInvoke(int32 arg,
                    class [mscorlib]System.AsyncCallback callback,
                    object 'object') runtime managed
{
} // end of method WorkDelegate::BeginInvoke

.method public hidebysig newslot virtual 
        instance int32  EndInvoke(class [mscorlib]System.IAsyncResult result) runtime managed
{
} // end of method WorkDelegate::EndInvoke

} // end of class WorkDelegate
```

The methods have no implementation because the CLR provides them at runtime.

The .NET Portability Analyzer only analyzes calls made to methods declared in .NET Framework assemblies, so it misses these methods, even though they may feel like .NET dependencies. Because the Portability Analyzer decides which APIs to analyze by looking at the name and public key token of the assembly declaring the API, the only way to analyze `BeginInvoke` and `EndInvoke` methods on user-defined delegates would be to analyze *all* API calls, which would require a large change to the portability analyzer and would have undesirable performance and privacy drawbacks.

## How to remove BeginInvoke/EndInvoke usage

The good news here is that calls to `BeginInvoke` and `EndInvoke` are usually easy to update so that they work with .NET Core. When fixing this type of error, there are a couple approaches.

First, if the API being invoked with the `BeginInvoke` call has a `Task`-based asynchronous alternative, call that instead. All delegates expose `BeginInvoke` and `EndInvoke` APIs, so there's no guarantee that the work is actually done asynchronously (`BeginInvoke` may just invoke a synchronous workflow on a different thread). If the API being called has an async alternative, using that API will probably be the easiest and most performant fix.

If there are no Task-based alternatives available, but offloading the call to a thread pool thread is still useful, this can be done by using `Task.Run` to schedule a task for running the method. If an `AsyncCallback` parameter was supplied when calling `BeginInvoke`, that can be replaced with a call to `Task.ContinueWith`.

Task-based Asynchronous Pattern (TAP) documentation has [guidance on how to wrap `IAsyncResult`-style patterns as `Task`s](https://docs.microsoft.com/dotnet/standard/asynchronous-programming-patterns/interop-with-other-asynchronous-patterns-and-types) using `TaskFactory`. Unfortunately, that solution doesn't work for .NET Core because the APM APIs (`BeginInvoke`, `EndInvoke`) are still used inside the wrapper. The TAP documentation guidance is useful for using older APM-style code in .NET Framework TAP scenarios, but for .NET Core migration, APM APIs like `BeginInvoke` and `EndInvoke` need to be replaced with synchronous calls (like `Invoke`) which can be run on a separate thread using `Task.Run`.

As an example, the code from earlier in this post can be replaced with the following:

```CSharp
delegate int WorkDelegate(int arg);
...
WorkDelegate del = DoWork;

// Schedule the work using a Task and 
// del.Invoke instead of del.BeginInvoke.
Console.WriteLine("Starting with Task.Run");
var workTask = Task.Run(() => del.Invoke(11));

// Optionally, we can specify a continuation delegate 
// to execute when DoWork has finished.
var followUpTask = workTask.ContinueWith(TaskCallback);

// This writes output to the console while DoWork is running in the background.
Console.WriteLine("Waiting on work...");

// We await the task instead of calling EndInvoke.
// Either workTask or followUpTask can be awaited depending on which
// needs to be finished before proceeding. Both should eventually
// be awaited so that exceptions that may have been thrown can be handled.
var ret = await workTask;
await followUpTask;
```

This code snippet provides the same functionality and works on .NET Core.

## Resources

* [Task-based asynchronous programming (TAP)](https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/task-based-asynchronous-programming) (the preferred alternative to [APM]((https://docs.microsoft.com/dotnet/standard/asynchronous-programming-patterns/asynchronous-programming-model-apm)))
* [Platform Compatibility Analyzers](https://github.com/dotnet/platform-compat)
* [API Portability Analyzer](https://github.com/Microsoft/dotnet-apiport)
