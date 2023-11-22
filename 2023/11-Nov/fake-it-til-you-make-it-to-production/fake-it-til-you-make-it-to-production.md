---
post_title: Fake It Til You Make It...To Production
author1: mataille
post_slug: fake-it-til-you-make-it-to-production
microsoft_alias: mataille
featured_image: dotnet8_blog_image.png
categories: .NET
tags: r9, aspire, .net 8, cloud native
summary: Explores the new logging, metric, and time provider fakes introduced in .NET 8
post_date: 2023-11-21 10:05:00
---

A few years back, I started an internal Microsoft project called R9. Our
goal was to create a robust SDK to accelerate the development of
reliable high-scale services on .NET. Hundreds of microservices within
Microsoft now leverage this SDK and happily many of the technologies we
created have been folded into .NET 8 for the benefit of the entire .NET
ecosystem.

We knew starting the R9 project that asking service teams to adopt
our technology would be challenging. Teams have a very high quality bar
and if we didn't meet that bar, it might
be a long time before they gave us another look, if ever. We therefore
put a strong emphasis on quality, establishing a target of 100% code
coverage, embracing [mutation testing](https://stryker-mutator.io/), and
adopting a zero-bug policy.

## Why we Started Faking It

Since the R9 components generate a fair bit of telemetry, we decided it
was critical for us to test the quality of our telemetry. Service teams
depend on having the right signals coming from their applications to be
able to successfully deploy, monitor, and repair their services. We
needed to make sure our telemetry was part of the solution, not the
problem.

At first, we were timidly using generated mocks
to capture telemetry from our components in ad hoc ways. Although this
technically worked, it was ugly. Using this approach usually requires tests
to assume the internal structure of the components being tested, which
makes tests brittle as they become susceptible to break when the internal
structure of a component is refactored. Since it was clumsy to write, we
ended up with few tests validating telemetry throughout our code base.

After a while, it became clear that to achieve our quality objectives,
we would need to improve how telemetry is tested, which led me to
create the logging and metric fakes. The model was successful as R9
developers quickly embraced it and the quality of our telemetry tests
improved dramatically. The fakes model caught on within the team, we
created several other fakes to help improve both our tests and our
customer’s tests.

## Time Is a Problem

Anybody that’s been part of a large software engineering team knows the
inherent problems caused by flaky tests. Flaky tests are those that
generally work, but occasionally trigger a false negative. If you have
thousands of test cases, a handful of flaky ones can make it difficult
to have a fully successful test run, which in turn makes it difficult to
know if changes you’re making to the code base are causing test
failures, or it’s merely a flaky test doing mischief.

Most flaky tests are caused by some sort of race condition, and often
these races exist purely in the tests and not in the software under
test. This happens because the environment in which software is tested
is often hacked together, being different than how the software is
normally used.

To help tame the race conditions, I decided to create an `IClock`
abstraction. We’d use this throughout our SDK to do anything time
related. `IClock` let us use a fake clock in our tests to completely
control the passage of time. Once deployed and used, this dramatically
reduced the number of flaky tests in our code base.

At some point, we searched through a bunch of internal Microsoft code
bases and found literally hundreds of variations of `IClock` and
associated types to use in testing. Clearly, this was a common problem
that deserved some attention. This work ultimately led to the
introduction of the [`TimeProvider`](https://learn.microsoft.com/dotnet/api/system.timeprovider)
type in .NET 8.

## Fakes in .NET 8

.NET 8 introduces several fakes to simplify test authoring. In this
article, I’ll describe the logging fake, the metering fake, and the time
provider fake. We don’t have the space to cover those here, but be on
the lookout for other useful fakes such as the fake host, fake
redactor, and fake data taxonomy.

## Logging Fake

The idea behind the logging fake is to use a custom implementation of
[`ILogger`](https://learn.microsoft.com/dotnet/api/microsoft.extensions.logging.ilogger)
whose job is to capture and accumulate all state being logged in
an in-memory buffer such that it can be inspected from a test suite.

Getting started is easy, you can just create a [`FakeLogger`](https://learn.microsoft.com/dotnet/api/microsoft.extensions.logging.testing.fakelogger)
instance and pass it to any code that is asking for an `ILogger` or `ILogger<T>`:

```csharp
[Fact]
public static void TestLogging()
{
    // a plain logger
    var logger1 = new FakeLogger();
    var c1 = new ClassToTest1(logger1);

    // a logger with a specific logging category
    var logger2 = new FakeLogger<ClassToTest2>();
    var c2 = new ClassToTest2(logger2);
}
```

Sometimes, code wants an `ILoggerProvider` out of which it creates its own
logger. For those cases, you can use the [`FakeLoggerProvider`](https://learn.microsoft.com/dotnet/api/microsoft.extensions.logging.testing.fakeloggerprovider)
 type either directly or through dependency injection.

Once you’ve got a logger provider or logger wired-in, anything logged
will be captured in a `FakeLogCollector` object. If you manually created
the logger, you can get the collector via the `FakeLogger.Collector`
property. If you used `FakeLoggerProvider` instead and you don’t have
immediate access to the logger object in your code, you can then use the
`GetFakeLogCollector` method to extract the [`FakeLogCollector`](https://learn.microsoft.com/dotnet/api/microsoft.extensions.logging.testing.fakelogcollector)
used by the logger instances created by dependency injection.

Once you have the `FakeLogCollector` object, you can do one of these
things:

- Check the `Count` property to make sure the expected number of log
  records were produced.

- Call `GetSnapshot` to get a copy of all the log records collected in the
  test case.

- Use the `LatestRecord` property to get access to the last log record
  that was collected.

The most common pattern is to check that `Count` equals 1, and then
use `LatestRecord` to ensure the logged data is correct. The `FakeLogRecord`
class contains properties to let you access all the state captured with
every log record including the log level (debug, warning, error, etc),
the category, the message, and the full set of name/value tags.

```csharp
[Fact]
public static void TestLogging()
{
    // create a fake logger for this test
    var logger = new FakeLogger();
    
    // pass the fake logger in to the code being tested
    var c = new ClassToTest1(logger);

    // trigger the code to do some work
    c.DoSomethingThatFails();

    // verify that the DoSomethingThatFails method has logged what it should have

    // only one log record
    Assert.Equal(1, logger.Collector.Count);    

    var r = logger.LatestRecord;

    // should be logged as an error
    Assert.Equal(LogLevel.Error, r.Level);

    // the message should include the text “Could not do something”
    Assert.Contains("Could not do something", r.Message);

    // there should be a “cause” tag with the value “no_support”
    Assert.Contains(r.StructuredState, tag => tag.Key == "cause" && tag.Value == "no_support"); 
}
```

## Metric Fake

The metric fake uses a similar model of capturing all the reported
metric updates and making it easy for you to inspect the resulting
state. Since metrics integrate very differently into .NET than logging
does, the fake model is also different and simpler.

The [`MetricCollector<T>`](https://learn.microsoft.com/dotnet/api/microsoft.extensions.diagnostics.metrics.testing.metriccollector-1) object lets you record all updates to a metric
instrument or observable instrument. Once you’ve created the collector,
you can use the `LastMeasurement` property to query the last update to the
instrument or call `GetMeasurementSnapshot` to get a list of all the
measurements captured for the instrument.

There are several different constructors for the `MetricCollector` class,
each letting you specify the specific instrument to collect from in a
different way, which tailors to most use cases.

Here’s an example showing how to use the `MetricCollector` type:

```csharp
public sealed class ClassToTest : IDisposable
{
    private const string CounterName = "MyCounter";
    private readonly Meter _meter;

    public ClassToTest()
    {
        _meter = new Meter(Guid.NewGuid().ToString());
        Counter = _meter.CreateCounter<long>(CounterName);
    }

    public void Dispose() => _meter.Dispose();

    public void DoWork(string workType, int unitsOfWork)
    {
        Counter.Add(unitsOfWork,
            new KeyValuePair<string, object?>[]
            {
                new("workType", workType),
            });
     }

    internal Counter<long> Counter { get; }
}

[Fact]
public static void TestMetering()
{
    const string WorkType = "Bootstrap";
    const int WorkUnits = 3;

    using var classToTest = new ClassToTest();
    using var collector = new MetricCollector<long>(classToTest.Counter);

    // show the collector has no state
    Assert.Empty(collector.GetMeasurementSnapshot());
    Assert.Null(collector.LastMeasurement);

    // now do some work that should update the counter
    classToTest.DoWork(WorkType, WorkUnits);

    // now verify the right telemetry was updated
    var snap = collector.GetMeasurementSnapshot();
    Assert.Equal(1, snap.Count);

    // get the sole measurement taken
    var m = snap[0];

    // make sure the value is what it should be
    Assert.Equal(WorkUnits, m.Value);

    // make sure the tags are exactly what they should be
    Assert.True(m.MatchesTags(new KeyValuePair<string, object?>("workType", WorkType)));

    // make sure the exact set of tags are present, regardless of the specific values they carry
    Assert.True(m.MatchesTags("workType"));

    // make sure at least the given tags are present
    Assert.True(m.MatchesTags(new KeyValuePair<string, object?>("workType", WorkType)));

    // make sure at least the given tags are present, regardless of the specific values they carry
    Assert.True(m.ContainsTags("workType"));
}
```

## TimeProvider Fake

The [`TimeProvider`](https://learn.microsoft.com/dotnet/api/system.timeprovider) type is new to .NET 8 and it provides a simple set of
methods to get the current time, get the current time zone, and create
timers. This is like functionality already present in .NET, except for
the fact the methods are not static. Instead, they are instance methods
on a type that’s designed to have both real and fake implementations.

The really useful aspect of `TimeProvider` is that it’s been integrated
into core functionality of the framework. For example, there are now
constructors for `CancellationTokenSource` and `PeriodicTimer` which take a
provider, `Task.WaitAsync` and `Task.Delay` now have overloads that take a
provider.

If you provide a fake time provider in all those places, you can
completely control how time behaves in those APIs. So instead of having
race conditions all over your test suite, you can have fully
deterministic tests that execute much faster than before (since you can
avoid the nasty delays that typically get added to test suites to work
around the race conditions).

The fake time provider is quite different than the logging and metric
fakes we just discussed. Instead of recording events, the fake time
provider is there as a manually controlled clock. Whereas the system
provider, which you get from `TimeProvider.System`, tracks real-world
time, the apparent passage of time is instead entirely controlled by the
fake time provider.

The [`FakeTimeProvider`](https://learn.microsoft.com/dotnet/api/microsoft.extensions.time.testing.faketimeprovider) type lets you do the following special things:

- You can set the specific time that the provider returns to callers.

- You can increment the specific time that the provider returns to
  callers.

- You can set an amount by which the specific time that the provider
  returns is automatically incremented any time the provider’s time is read.

Controlling the provider’s time is like a superpower. If your code calls
`Task.Delay` with a fake provider and a delay of 1 second, `Task.Delay` will
not return until the provider’s time is manually advanced to 1 second.
This can in fact take 1 microsecond or 1 hour of real-world time, that
no longer matters.

```csharp
public sealed class KeepAlive
{
    private readonly TimeProvider _timeProvider;
    private readonly ILogger _logger;

    // the normal public constructor which uses the system time provider and so is subject to real-time
    public KeepAlive(ILogger logger)
        : this(logger, TimeProvider.System)
    {
    }

    // an internal constructor used by tests to supply a fake time provider to control the passage of time
    internal KeepAlive(ILogger logger, TimeProvider timeProvider)
    {
        _logger = logger;
        _timeProvider = timeProvider;

        // start a background task that logs a message every second
        _ = Task.Run(async () =>
        {
            await Task.Delay(TimeSpan.FromSeconds(1), _timeProvider);
            _logger.LogInformation("I'm Alive!");
        });
    }
}

[Fact]
public async Task TestTime()
{
    var logger = new FakeLogger();
    var timeProvider = new FakeTimeProvider();
    var classToTest = new KeepAlive(logger, timeProvider);

    // wait 2 real-world seconds
    await Task.Delay(2000);

    // confirm that nothing has been logged yet
    Assert.Equal(0, logger.Collector.Count);

    // now advance the clock by almost a whole second, but not quite
    timeProvider.Advance(TimeSpan.FromMilliseconds(999));

    // wait 2 real-world seconds
    await Task.Delay(2000);

    // confirm that nothing has been logged yet
    Assert.Equal(0, logger.Collector.Count);

    // now advance the clock by another millisecond
    timeProvider.Advance(TimeSpan.FromMilliseconds(1));

    // wait 2 real-world seconds
    await Task.Delay(2000);

    // confirm that one thing has been logged
    Assert.Equal(1, logger.Collector.Count);
}
```

As you can see, until the fake time provider’s time is explicitly
advanced by the test, the `Task.Delay` call in the `KeepAlive` class just sits
there and blocks.

Using this general model lets you finely control what happens in any
time-sensitive code, turning hard-to-test non-determinism into
rock-solid deterministic logic.

## Summary

I hope this has given you a taste of what it’s like to test with the new
functionality provided in .NET 8. These features are designed to make it
easier to write robust tests, which in turn should make it faster to get
your code into production.
