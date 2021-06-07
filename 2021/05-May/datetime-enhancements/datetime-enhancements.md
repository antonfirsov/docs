---
post_title: Date, Time, and Time Zone Enhancements in .NET 6
username: mattjoh
microsoft_alias: mattjoh
categories: .NET Core, .NET
desired_publication_date: 06/01/2021
summary: .NET 6 Preview 4 introduces DateOnly and TimeOnly structs and improves time zone support.
---

I'm excited to share with you some of the improvements that have been made to .NET that are coming in .NET 6 in the area of dates, times, and time zones.  You can try out all of the following, starting with **[.NET 6 Preview 4](https://dotnet.microsoft.com/download/dotnet/6.0).**

In this blog post, I'm going to cover the following topics:

- [The new DateOnly and TimeOnly types](#introducing-the-dateonly-and-timeonly-types)
- [Time Zone Conversion APIs](#time-zone-conversion-apis)
- [Time Zone Display Names on Linux and macOS](#time-zone-display-names-on-linux-and-macos)
- [TimeZoneInfo.AdjustmentRule Improvements](#timezoneinfoadjustmentrule-improvements)

For even more details, you can also refer to [dotnet/runtime#45318](https://github.com/dotnet/runtime/issues/45318) on GitHub.

## Introducing the DateOnly and TimeOnly Types

If you've worked with dates and times in .NET, you've probably used `DateTime`, `DateTimeOffset`, `TimeSpan` and `TimeZoneInfo`.  With this release, we introduce two additional types: `DateOnly` and `TimeOnly`.  Both are in the `System` namespace and are built-in to .NET, just like the other date and time types.

### The DateOnly Type

The `DateOnly` type is a structure that is intended to represent *only* a date.  In other words, just a year, month, and day.  Here's a brief example:

```csharp
// Construction and properties
DateOnly d1 = new DateOnly(2021, 5, 31);
Console.WriteLine(d1.Year);      // 2021
Console.WriteLine(d1.Month);     // 5
Console.WriteLine(d1.Day);       // 31
Console.WriteLine(d1.DayOfWeek); // Monday

// Manipulation
DateOnly d2 = d1.AddMonths(1);  // You can add days, months, or years. Use negative values to subtract.
Console.WriteLine(d2);     // "6/30/2021"  notice no time

// You can use the DayNumber property to find out how many days are between two dates
int days = d2.DayNumber - d1.DayNumber;
Console.WriteLine($"There are {days} days between {d1} and {d2}");

// The usual parsing and string formatting tokens all work as expected
DateOnly d3 = DateOnly.ParseExact("31 Dec 1980", "dd MMM yyyy", CultureInfo.InvariantCulture);  // Custom format
Console.WriteLine(d3.ToString("o", CultureInfo.InvariantCulture));   // "1980-12-31"  (ISO 8601 format)

// You can combine with a TimeOnly to get a DateTime
DateTime dt = d3.ToDateTime(new TimeOnly(0, 0));
Console.WriteLine(dt);       // "12/31/1980 12:00:00 AM"

// If you want the current date (in the local time zone)
DateOnly today = DateOnly.FromDateTime(DateTime.Today);
```

A `DateOnly` is ideal for scenarios such as birth dates, anniversary dates, hire dates, and other business dates that are not typically associated with any particular time.  Another way to think about it is that a `DateOnly` represents the *whole* date, such as would be visualized by a given square of a printed wall calendar.  Until now, you may have used `DateTime` for this purpose, likely with the time set to midnight (`00:00:00.0000000`).  While that still works, there are several advantages to using a `DateOnly` instead.  These include:

- A `DateOnly` provides better type safety than a `DateTime` that is intended to represent just a date.  This matters when using APIs, as not every action that makes sense for a date and time also makes sense for a whole date.  For example, the `TimeZoneInfo.ConvertTime` method can be used to convert a `DateTime` from one time zone to another.  Passing it a whole date makes no sense, as only a single point in time on that date could possibly be converted.  With `DateTime`, these nonsensical operations can happen, and are partially to blame for bugs that might shift someone's birthday a day late or a day early.  Since no such time zone conversion API would work with a `DateOnly`, accidental misuse is prevented.
- A `DateTime` also contains a `Kind` property of type `DateTimeKind`, which can be either `Local`, `Utc` or `Unspecified`.  The kind affects behavior of conversion APIs as well as formatting and parsing of strings.  A `DateOnly` has no such kind - it is effectively `Unspecified`, always.
- When serializing a `DateOnly`, you only need to include the year, month, and day.  This makes your data clearer by preventing a bunch of zeros from being tacked on to the end.  It also makes it clear to any consumer of your API that the value represents a whole date, not the time at midnight on that date.
- When interacting with a database (such as SQL Server and others), whole dates are almost always stored in a `date` data type.  Until now, the APIs for storing and retrieving such data have been strongly tied to the `DateTime` type.  On storage, the time would be truncated, potentially leading to data loss.  On retrieval, the time would be set to zeros and would be indistinguishable from a date at midnight.  Having a `DateOnly` type allows a more exact matching type to a database's `date` type.  Note, there is [still work to do](https://github.com/dotnet/runtime/issues/49036#issuecomment-806444260) for the various data providers support this new type, but at least it is now possible.

A `DateOnly` has a range from `0001-01-01` through `9999-12-31`, just like `DateTime`.  We've also included a constructor that will accept any of the calendars supported by .NET.  However just like `DateTime`, a `DateOnly` object is *always* representing values of the [Proleptic Gregorian calendar](https://en.wikipedia.org/wiki/Proleptic_Gregorian_calendar), regardless of which calendar was used to construct it.  If you do pass a calendar to the constructor, it will only be used to *interpret* the year, month, and day values passed into the same constructor.  For example:

```csharp
Calendar hebrewCalendar = new HebrewCalendar();
DateOnly d4 = new DateOnly(5781, 9, 16, hebrewCalendar);                   // 16 Sivan 5781
Console.WriteLine(d4.ToString("d MMMM yyyy", CultureInfo.InvariantCulture)); // 27 May 2021
```

For more on this, see [*Working with calendars*](https://docs.microsoft.com/dotnet/standard/datetime/working-with-calendars).

### The TimeOnly Type

We also get a new `TimeOnly` type, which is a structure that is intended to represent *only* a time of day.  If `DateOnly` is one half of a `DateTime`, then `TimeOnly` is the other half.  Here's a brief example:

```csharp
// Construction and properties
TimeOnly t1 = new TimeOnly(16, 30);
Console.WriteLine(t1.Hour);      // 16
Console.WriteLine(t1.Minute);    // 30
Console.WriteLine(t1.Second);    // 0

// You can add hours, minutes, or a TimeSpan (using negative values to subtract).
TimeOnly t2 = t1.AddHours(10);
Console.WriteLine(t2);     // "2:30 AM"  notice no date, and we crossed midnight

// If desired, we can tell how many days were "wrapped" as the clock passed over midnight.
TimeOnly t3 = t2.AddMinutes(5000, out int wrappedDays);
Console.WriteLine($"{t3}, {wrappedDays} days later");  // "1:50 PM, 3 days later"

// You can subtract to find out how much time has elapsed between two times.
// Use "end time - start time".  The order matters, as this is a circular clock.  For example:
TimeOnly t4 = new TimeOnly(2, 0);  //  2:00  (2:00 AM)
TimeOnly t5 = new TimeOnly(21, 0); // 21:00  (9:00 PM)
TimeSpan x = t5 - t4;
TimeSpan y = t4 - t5;
Console.WriteLine($"There are {x.TotalHours} hours between {t4} and {t5}"); // 19 hours
Console.WriteLine($"There are {y.TotalHours} hours between {t5} and {t4}"); //  5 hours

// The usual parsing and string formatting tokens all work as expected
TimeOnly t6 = TimeOnly.ParseExact("5:00 pm", "h:mm tt", CultureInfo.InvariantCulture);  // Custom format
Console.WriteLine(t6.ToString("T", CultureInfo.InvariantCulture));   // "17:00:00"  (long time format)

// You can get an equivalent TimeSpan for use with previous APIs
TimeSpan ts = t6.ToTimeSpan();
Console.WriteLine(ts);      // "17:00:00"

// Or, you can combine with a DateOnly to get a DateTime
DateTime dt = new DateOnly(1970, 1, 1).ToDateTime(t6);
Console.WriteLine(dt);       // "1/1/1970 5:00:00 PM"

// If you want the current time (in the local time zone)
TimeOnly now = TimeOnly.FromDateTime(DateTime.Now);

// You can easily tell if a time is between two other times
if (now.IsBetween(t1, t2))
    Console.WriteLine($"{now} is between {t1} and {t2}.");
else
    Console.WriteLine($"{now} is NOT between {t1} and {t2}.");
```

A `TimeOnly` is ideal for scenarios such as recurring meeting times, daily alarm clock times, or the times that a business opens and closes each day of the week.  Because a `TimeOnly` isn't associated with any particular date, it is best visualized as a circular analog clock that might hang on your wall (albeit a 24-hour clock, not a 12-hour clock).  Until now, there have been two common ways that such values were represented, either using a `TimeSpan` type or a `DateTime` type.  While those approaches still work, there are several advantages to using a `TimeOnly` instead, including:

- A `TimeSpan` is primarily intended for *elapsed* time, such as you would measure with a stopwatch.  Its upper range is more than 29,000 *years*, and its values can also be *negative* to indicate moving backward in time.  Conversely, a `TimeOnly` is intended for a time-of-day value, so its range is from `00:00:00.0000000` to `23:59:59.9999999`, and is always positive.  When a `TimeSpan` is used as a time of day, there is a risk that it could be manipulated such that it is out of an acceptable range.  There is no such risk with a `TimeOnly`.
- Using a `DateTime` for a time-of-day value requires assigning some arbitrary date.  A common date picked is `DateTime.MinValue` (`0001-01-01`), but that sometimes leads to out of range exceptions during manipulation, if time is subtracted.  Picking some other arbitrary date still requires remembering to later disregard it - which can be a problem during serialization.
- `TimeOnly` is a true time-of-day type, and so it offers superior type safety for such values vs `DateTime` or `TimeSpan`, in the same way that using a `DateOnly` offers better type safety for date values than a `DateTime`.
- A common operation with time-of-day values is to add or subtract some period of elapsed time.  Unlike `TimeSpan`, a `TimeOnly` value will correctly handle such operations when crossing midnight.  For example, an employee's shift might start at `18:00` and last for 8 hours, ending at `02:00`.  `TimeOnly` will take care of that during the addition operation, and it also has an `InBetween` method that can easily be used to tell if any given time is within the worker's shift.

### Why are they named "Only"?

Naming things is always difficult and this was no different. Several different names were considered and debated at length, but ultimately `DateOnly` and `TimeOnly` were approved because they met several different constraints:

- They did not use any .NET language's reserved keywords.  `Date` would have been an ideal name, but it is a VB.Net language keyword and data type, which is an alias for `System.DateTime`, and thus could not be chosen.
- They would be easily discoverable in documentation and IntelliSense, by starting with "Date" or "Time".  We felt this was important, as many .NET developers are accustomed to using `DateTime` and `TimeSpan` types.  Other platforms use prefixed names for the same functionality, such as Java's `LocalDate` and `LocalTime` classes in the [java.time](https://docs.oracle.com/javase/8/docs/api/java/time/package-summary.html) package, or the `PlainDate` and `PlainTime` types in the upcoming [Temporal](https://tc39.es/proposal-temporal/docs/) proposal for JavaScript.  However, both of those counterexamples have all date and time types grouped in a specific namespace, where .NET has its date and time types in the much larger `System` namespace.
- They would avoid confusion with existing APIs as much as possible.  In particular, both the `DateTime` and `DateTimeOffset` types have properties that are named `Date` (which returns a `DateTime`) and `TimeOfDay` (which returns a `TimeSpan`).  We felt that it would be extremely confusing if we used the name `TimeOfDay` instead of `TimeOnly`, but the `DateTime.TimeOfDay` property returned a `TimeSpan` type instead of a `TimeOfDay` type.  If we could go back and do it all over from scratch then we would pick these as both the names of the properties *and* the names of the types they return, but such a breaking change is not possible now.
- They are easy to remember, and intuitively state what they are for.  Indeed, "date-only and time-only values" are good descriptions for how the `DateOnly` and `TimeOnly` types should be used.  Furthermore, they combine to make a `DateTime` so giving them similar names keeps them logically paired together.

In short, while the *best* names might have been `Date` and `TimeOfDay`, those names didn't fit the constraints.

### What about Noda Time?

In introducing these two types, many have asked about using [Noda Time](https://nodatime.org/) instead.  Indeed, Noda Time is a great example of a high-quality, community developed .NET open source library, and you can certainly use it if desired.  However, we didn't feel that implementing a Noda-like API in .NET itself was warranted.  After careful evaluation, it was decided that it would be better to *augment* the existing types to fill in the gaps rather than to overhaul and replace them.  After all, there are many .NET applications built using the existing `DateTime`, `DateTimeOffset`, `TimeSpan`, and `TimeZoneInfo` types.  The `DateOnly` and `TimeOnly` types should feel natural to use along side them.

Additionally, support for interchanging `DateOnly` and `TimeOnly` with their equivalent Noda Time types (`LocalDate` and `LocalTime`) [has been proposed](https://github.com/nodatime/nodatime/issues/1635).

## Time Zone Conversion APIs

First a bit of background and history.  Generally speaking, there are two sets of time zone data used in computing:

- The set of time zones created by Microsoft that ship with Windows.
  - Example ID:  `"AUS Eastern Standard Time"`
- The set of time zones that everyone else uses, which are currently maintained by [IANA](https://www.iana.org/time-zones).
  - Example ID:  `"Australia/Sydney"`

By *everyone* else, I don't just mean Linux and macOS, but also Java, Python, Perl, Ruby, Go, JavaScript, and many others.

Support for time zones in .NET is provided by the `TimeZoneInfo` class.  However, this class was designed original with .NET Framework 3.5, which only ran on Windows operating systems.  As such, `TimeZoneInfo` took its time zone data from Windows.  This quickly became a problem for those that wanted to reference time zones in data passed between systems.  When .NET Core came out, this problem was exacerbated because Windows time zone data was not available on non-Windows systems like Linux and macOS.

Previously, the `TimeZoneInfo.FindSystemTimeZoneById` method looked up time zones available *on the operating system*.  That means Windows time zones for Windows systems, and IANA time zones for everyone else.  That is problematic, especially if one is aiming for cross-platform portability of their code and data.  Until now, the way to deal with this issue has been to *manually* translate between one set of time zones to the other, preferably using the mappings established and maintained by the [Unicode CLDR Project](https://github.com/unicode-org/cldr).  These mappings are also surfaced by libraries such as [ICU](https://github.com/unicode-org/icu).  More commonly, .NET developers have used the [TimeZoneConverter](https://github.com/mattjohnsonpint/TimeZoneConverter) library which also uses these mappings.  While any of these approaches continue to work, there is now an easier way.

Starting with this release, the `TimeZoneInfo.FindSystemTimeZoneById` method will *automatically* convert its input to the opposite format if the requested time zone is not found on the system.  That means that you can now use either IANA or Windows time zone IDs on any operating system that has time zone data installed*.   It uses the same CLDR mappings, but gets them through [.NET's ICU globalization support](https://docs.microsoft.com/dotnet/standard/globalization-localization/globalization-icu), so you don't have to use a separate library.

A brief example:

```csharp
// Both of these will now work on any supported OS where ICU and time zone data are available.
TimeZoneInfo tzi1 = TimeZoneInfo.FindSystemTimeZoneById("AUS Eastern Standard Time");
TimeZoneInfo tzi2 = TimeZoneInfo.FindSystemTimeZoneById("Australia/Sydney");
```

On Unix, the Windows time zones are not actually installed on the OS but their identifiers are recognized through the conversions and data provided by ICU.  You can install `libicu` on your system, or you can use [.NET's App-Local ICU](https://docs.microsoft.com/dotnet/standard/globalization-localization/globalization-icu#app-local-icu) feature to bundle the data with your application.

**Note, some .NET Docker images such as for Alpine Linux do not come with the `tzdata` package pre-installed, but you can [easily add it](https://github.com/dotnet/dotnet-docker/issues/1366).*

Also with this release, we've added some new methods to the `TimeZoneInfo` class called `TryConvertIanaIdToWindowsId` and `TryConvertWindowsIdToIanaId`, for scenarios when you still need to manually convert from one form of time zone to another.

Some example usage:

```csharp
// Conversion from IANA to Windows
string ianaId1 = "America/Los_Angeles";
if (!TimeZoneInfo.TryConvertIanaIdToWindowsId(ianaId1, out string winId1))
    throw new TimeZoneNotFoundException($"No Windows time zone found for \"{ianaId1}\".");
Console.WriteLine($"{ianaId1} => {winId1}");  // "America/Los_Angeles => Pacific Standard Time"

// Conversion from Windows to IANA when a region is unknown
string winId2 = "Eastern Standard Time";
if (!TimeZoneInfo.TryConvertWindowsIdToIanaId(winId2, out string ianaId2))
    throw new TimeZoneNotFoundException($"No IANA time zone found for \"{winId2}\".");
Console.WriteLine($"{winId2} => {ianaId2}");  // "Eastern Standard Time => America/New_York"

// Conversion from Windows to IANA when a region is known
string winId3 = "Eastern Standard Time";
string region = "CA"; // Canada
if (!TimeZoneInfo.TryConvertWindowsIdToIanaId(winId3, region, out string ianaId3))
    throw new TimeZoneNotFoundException($"No IANA time zone found for \"{winId3}\" in \"{region}\".");
Console.WriteLine($"{winId3} + {region} => {ianaId3}");  // "Eastern Standard Time + CA => America/Toronto"
```

We've also added an instance property to `TimeZoneInfo` called `HasIanaId`, which returns `true` when the `Id` property is an IANA time zone identifier.  That should help you determine whether conversion is necessary, depending on your needs.  For example, perhaps you are using `TimeZoneInfo` objects loaded from a mix of either Windows or IANA identifiers, and then specifically need an IANA time zone identifier for some external API call.  You can define a helper method as follows:

```csharp
static string GetIanaTimeZoneId(TimeZoneInfo tzi)
{
    if (tzi.HasIanaId)
        return tzi.Id;  // no conversion necessary

    if (TimeZoneInfo.TryConvertWindowsIdToIanaId(tzi.Id, out string ianaId))
        return ianaId;  // use the converted ID

    throw new TimeZoneNotFoundException($"No IANA time zone found for \"{tzi.Id}\".");
}
```

Or conversely, perhaps you are needing a Windows time zone identifier.  For example, SQL Server's [`AT TIME ZONE`](https://docs.microsoft.com/sql/t-sql/queries/at-time-zone-transact-sql) function presently requires a Windows time zone identifier - even when using SQL Server on Linux.  You can define a helper method as follows:

```csharp
static string GetWindowsTimeZoneId(TimeZoneInfo tzi)
{
    if (!tzi.HasIanaId)
        return tzi.Id;  // no conversion necessary

    if (TimeZoneInfo.TryConvertIanaIdToWindowsId(tzi.Id, out string winId))
        return winId;   // use the converted ID

    throw new TimeZoneNotFoundException($"No Windows time zone found for \"{tzi.Id}\".");
}
```

## Time Zone Display Names on Linux and macOS

Another common operation with time zones it to get a list of them, usually for purposes of asking an end-user to choose one.  The `TimeZoneInfo.GetSystemTimeZones` method has always served this purpose well on Windows.  It returns a read-only collection of `TimeZoneInfo` objects, and such a list can be built using the `Id` and `DisplayName` properties of each object.

On Windows, .NET populates the display names using the resource files associated with the current OS display language.  On Linux and macOS, the [ICU globalization data](https://docs.microsoft.com/dotnet/standard/globalization-localization/globalization-icu) is used instead.  This is generally ok, except that one has to ensure that the `DisplayName` value is unambiguous with regard to the entire list of values, otherwise such a list becomes unusable.  For example, there were 13 different time zones returned that all had the same display name of `"(UTC-07:00) Mountain Standard Time"`, making it near impossible for a user to pick the one that belonged to them - and yes, there are differences!  For example, `America/Denver` represents most of Mountain Time in the US, but `America/Phoenix` is used in Arizona where daylight saving time is not observed.

With this release, additional algorithms were added internally to choose better values from ICU to be used for display names.  The lists are now much more usable.  For example, `America/Denver` is now displayed as `"(UTC-07:00) Mountain Time (Denver)"` while `America/Phoenix` is displayed as `"(UTC-07:00) Mountain Time (Phoenix)"`.  If you'd like to see how the rest of the list has changed, refer to the "Before" and "After" sections in [the GitHub pull request](https://github.com/dotnet/runtime/pull/48931).

Note that for now, the list of time zones and their display names on *Windows* remains mostly unchanged.  However, a minor but related fix is that previously the display name for the UTC time zone was hard-coded to the English `"Coordinated Universal Time"`, which was a problem for other languages.  It now correctly follows the same language as the rest of the time zone display names, on all operating systems.

## TimeZoneInfo.AdjustmentRule Improvements

The last improvement to cover is one that is slightly lesser used, but just as important.  The `TimeZoneInfo.AdjustmentRule` class is used as part of .NET's in-memory representation of a time zone.  A single `TimeZoneInfo` class can have zero to many adjustment rules.  These rules keep track of how a time zone's offset from UTC is adjusted over the course of history, so that the correct conversions can be made for a given point in time.  Such changes are extremely complicated, and mostly beyond the scope of this article.  However, I will describe some of the improvements that have been made.

In the original design of `TimeZoneInfo`, it was assumed that the `BaseUtcOffset` would be a fixed value and that all the adjustment rules would simply control when daylight saving time started or stopped.  Unfortunately, that design didn't take into account that time zones have changed their *standard* offset at different points in history, such as when Yukon Territory, Canada [recently decided](https://www.timeanddate.com/news/time/yukon-canada-permanent-dst.html) to stop going between UTC-8 and UTC-7, and instead stay at UTC-7 year-round.  To accommodate such changes, .NET added an *internal* property (a long time ago) to the `TimeZoneInfo.AdjustmentRule` class called `BaseUtcOffsetDelta`.  This value is used to keep track of how the `TimeZoneInfo.BaseUtcOffset` changes from one adjustment rule to the next.

However, there are some advanced scenarios that occasionally require gaining access to all the raw data, and keeping one piece of the data hidden internally didn't make much sense.  So with this release, the `BaseUtcOffsetDelta` property on the `TimeZoneInfo.AdjustmentRule`  class is made public.  For completeness, we also took an additional step and created an overload to the `CreateAdjustmentRule` method, that accepts a `baseUtcOffsetDelta` parameter - not that we expect most developers will need or want to create custom time zones or adjustment rules.

Two other minor improvements were made to how adjustment rules are populated from IANA data internally on non-Windows operating systems.  They don't affect external behavior, other than to ensure correctness in some edge cases.

If this all sounds confusing, don't worry you're not alone.  Thankfully, all of the logic to use the data correctly is already incorporated in the various methods on `TimeZoneInfo`, such as `GetUtcOffset` and `ConvertTime`.  One generally doesn't need to use adjustment rules.

## Conclusion

Overall, things are shaping up quite a bit for date, time, and time zones in .NET 6.  I'm excited to see how the new `DateOnly` and `TimeOnly` types make there way through the rest of the .NET ecosystem, especially with regard to serialization and databases.  I'm also excited to see that .NET continues to make improvements to localization and usability - even in the obscure area of time zones!

Please be sure to leave your feedback about these new features below.  Thanks!
