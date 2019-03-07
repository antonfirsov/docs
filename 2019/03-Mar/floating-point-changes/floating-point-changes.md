# Floating-Point Parsing and Formatting improvements in .NET Core

Starting back with the .NET Core 2.1 release, we were making iterative improvements to the floating-point parsing and formatting code in .NET Core. Now, in .NET Core 3.0 Preview 3, we are nearing completion of this work and would like to share more details about these changes and some of the differences you might see in your applications.

The primary goals of this work were to ensure correctness and standards compliance with [IEEE 754-2008](https://en.wikipedia.org/wiki/IEEE_754-2008_revision). For those unfamiliar with the standard, it defines the underlying format, base operations, and behaviors for binary floating-point types such as [System.Single](https://docs.microsoft.com/en-us/dotnet/api/system.single?view=netcore-3.0) (`float`) and [System.Double](https://docs.microsoft.com/en-us/dotnet/api/system.double?view=netcore-3.0) (`double`). The majority of modern processors and programming languages support some version of this standard, so it is important to ensure it is implemented correctly. The standard does not impact the integer types, such as [System.Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32?view=netcore-3.0) (`int`), nor does it impact the other floating-point types, such as [System.Decimal] (https://docs.microsoft.com/en-us/dotnet/api/system.decimal?view=netcore-3.0) (`decimal`).

## Initial Work

We started with parsing changes, as part of the .NET Core 2.1 release. Initially, this was just an attempt to fix a perf difference between Windows and Unix and was done by [@mazong1123](https://github.com/mazong1123) in [dotnet/coreclr#12894](https://github.com/dotnet/coreclr/pull/12894), which implements the [Dragon4](http://www.ryanjuckett.com/programming/printing-floating-point-numbers/) algorithm. [@mazong1123](https://github.com/mazong1123) also made a follow up PR which improved perf even more by implementing the [Grisu3](https://www.cs.tufts.edu/~nr/cs257/archive/florian-loitsch/printf.pdf) algorithm in [dotnet/coreclr#14646](https://github.com/dotnet/coreclr/pull/14646). However, in reviewing the code we determined that existing infrastructure had a number of issues that prevented us from always doing the right thing and that it would require significantly more work to make correct.

## Porting to C#

The first step in fixing these underlying infrastructure issues was porting the code from native to managed. We did this work in [dotnet/coreclr#19999](https://github.com/dotnet/coreclr/pull/19999) and [dotnet/coreclr#20080](https://github.com/dotnet/coreclr/pull/20080). The result was we made the code more portable, allowed it to be shared with the other frameworks/runtimes (such as Mono and CoreRT), made it possible to easily debug the code with the .NET Debugger, and made it available through SourceLink.

## Making the parser IEEE compliant

We did some additional cleanup in [dotnet/coreclr#20619](https://github.com/dotnet/coreclr/pull/20619) by removing various bits of duplicated code that was shared between the different parsers. Finally, we made the `double` and `float` parsing logic mostly IEEE compliant in [dotnet/coreclr#20707](https://github.com/dotnet/coreclr/pull/20707) and this made available in the first .NET Core 3.0 Preview. 

These changes fixed three primary issues:
* [dotnet/coreclr#1316](https://github.com/dotnet/coreclr/issues/1316)
* [dotnet/coreclr#16783](https://github.com/dotnet/coreclr/issues/16783)
* [dotnet/coreclr#17467](https://github.com/dotnet/coreclr/issues/17467)

The fixes ensured that [double.Parse](https://docs.microsoft.com/en-us/dotnet/api/system.double.parse?view=netcore-3.0)/[float.Parse](https://docs.microsoft.com/en-us/dotnet/api/system.single.parse?view=netcore-3.0) would return the same result as the C#, VB, or F# compiler for the corresponding literal value. Producing the same result as the language compilers is important for determinism of runtime and compile time expressions. Up until the change, this was not the case.

To elaborate, every floating-point value requires between 1 and X significant digits (i.e. all digits that are not leading or trailing zeros) in order to roundtrip the value (that is, in order for `double.Parse(value.ToString())` to return exactly `value`). This is at most 17 digits for `double` and at most 9 digits for `float`. However, this only applies to strings that are first formatted from an existing floating-point value. When parsing from an arbitrary string, you may have to instead consider up to Y digits to ensure that you produce the "nearest" representable value. This is 768 digits for `double` and 113 digits for `float`. We have tests validating such strings parse correctly in [RealParserTestsBase.netcoreapp.cs](https://github.com/dotnet/corefx/blob/master/src/Common/tests/System/RealParserTestsBase.netcoreapp.cs) and [dotnet/corefx#35701](https://github.com/dotnet/corefx/pull/35701). More details on this can be found on Rick Regan's [Exploring Binary](https://www.exploringbinary.com/maximum-number-of-decimal-digits-in-binary-floating-point-numbers/) blog.

An example of such a string would be for [double.Epsilon](https://docs.microsoft.com/en-us/dotnet/api/system.double.epsilon?view=netcore-3.0) (which is the smallest value that is greater than zero). The shortest roundtrippable string for this value is only `5e-324`, but the exact string (i.e. the string that contains all significant digits available in the underlying value) for this value is exactly 1074 digits long, which is comprised of 323 leading zeros and 751 significant digits. You then need one additional digit to ensure that the string is rounded in the correct direction (should it be exactly `double.Epsilon` or the smallest value that is greater than `double.Epsilon`).

Some additional minor cleanup was done in [dotnet/coreclr#21036](https://github.com/dotnet/coreclr/pull/21036) to ensure that the remaining compliance issues were resolved. These ended up mostly about ensuring we handle `Infinity` and `NaN` case-insensitively and that we allowed an optional preceding sign.

## Making the formatter IEEE 754-2008 compliant

The formatting code required more significant changes and was primarily done in [dotnet/coreclr#22040](https://github.com/dotnet/coreclr/pull/22040) with some followup work fixing some remaining issues in [dotnet/coreclr#22522](https://github.com/dotnet/coreclr/pull/22522).

These changes fixed 5 primary issues:
 * [dotnet/corefx#26785](https://github.com/dotnet/corefx/issues/26785)
 * [dotnet/coreclr#3313](https://github.com/dotnet/coreclr/issues/3313)
 * [dotnet/coreclr#13106](https://github.com/dotnet/coreclr/issues/13106)
 * [dotnet/coreclr#13615](https://github.com/dotnet/coreclr/issues/13615)
 * [dotnet/coreclr#21272](https://github.com/dotnet/coreclr/issues/21272)

These changes are expected to have the largest potential impact to existing code.

The summary of these changes is that (for `double`/`float`):
* `ToString()`, `ToString("G")`, and `ToString("R")` will now return the shortest roundtrippable string. This ensures that users end up with something that just works by default. An example of where it was problematic was `Math.PI.ToString()` where the string that was previously being returned (for `ToString()` and `ToString("G")`) was `3.14159265358979`; instead, it should have returned `3.1415926535897931`. The previous result, when parsed, returned a value which was internally off by 7 ULP (units in last place) from the actual value of [Math.PI](https://docs.microsoft.com/en-us/dotnet/api/system.math.pi?view=netcore-3.0). This meant that it was very easy for users to get into a scenario where they would accidentally lose some precision on a floating-point value when the needed to serialize/deserialize it.
* For the `"G"` format specifier that takes a precision (e.g. `G3`), the precision specifier is now always respected. For `double` with precisions less than 15 (inclusive) and for `float` with precisions less than 6 (inclusive) this means you get the same string as before. For precisions greater than that, you will get up to that many significant digits, provided those digits are available (i.e. `(1.0).ToString("G17")` will still return `1` since the exact string only has one significant digit; but `Math.PI.ToString("G20")` will now return `3.141592653589793116`, since the exact string contains at least 20 significant digits).
* For the `"C"`, `"E"`, `"F"`, `"N"`, and `"P"` format specifiers the changes are similar. The difference is that these format specifiers treat the precision as the number of digits after the decimal point, in contrast to `"G"` which treats it as the number of significant digits. The previous implementation had a bug where, for strings that contained more than 15 significant digits, it would actually fill in the remaining digits with zero, regardless of whether they appeared before or after the decimal point. As an example, `(1844674407370955.25).ToString("F4")` would previously return `1844674407370960.0000`. The exact string, however, actually contains enough information to fill all the integral digits. With the changes made we instead fill out the available integral digits while still respecting the request for the 4 digits after the decimal point and instead return `1844674407370955.2500`.
* For custom format strings, they have the same behavior as before and will only print up to 15 significant digits, regardless of how many are requested. Fixing this to support an arbitrary number of digits would require more work to support and hasn't been done at this time.

## Potential impact to existing code

When picking up .NET Core 3.0, it is expected that you may encounter some of the differences described in this post in your application or library code. The general recommendation is that the code be updated to handle these changes. However, this may not be possible in all cases and a workaround may be required. Focused testing for floating-point specific code is recommended.

For differences in parsing, there is no mechanism to fallback to the old behavior. There were already differences across various operating systems (i.e. Linux, Windows, macOS, etc) and architectures (i.e. x86, x64, ARM, ARM64, etc). The new logic makes all of these consistent and ensures that the result returned is consistent with the corresponding language literal.

For differences in formatting, you can get the equivalent behavior by:
* For `ToString()` and `ToString("G")` you can use `G15` as the format specifier as this is what the previous logic would do internally.
* For `ToString("R")`, there is no mechanism to fallback to the old behavior. The previous behavior would first try "G15" and then using the internal buffer would see if it roundtrips; if that failed, it would instead return "G17".
* For the `"G"` format-specifier that takes a precision, you can force precisions greater than 15 (exclusive) to be exactly 17. For example, if your code is doing `ToString("G20")` you can instead change this to `ToString("G17")`.
* For the remaining format-specifiers that take a precision (`"C"`, `"E"`, `"F"`, `"N"`, and `"P"`), there is no mechanism to fallback to the old behavior. The previous behavior would clamp precisions greater than 14 (exclusive) to be 17 for `"E"` and 15 for the others. However, this only impacted the significant digits that would be displayed, the remaining digits (even if available) would be filled in as zero.
