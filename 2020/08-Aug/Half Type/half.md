The IEEE 754 specification defines many floating point types, including: `binary16`, `binary32`, `binary64` and `binary128`. Most developers are familiar with `binary32` (equivalent to `float` in C#) and `binary64` (equivalent to `double` in C#). They provide a standard format to represent a wide range of values with a precision acceptable for many applications. .NET has always had `float` and `double` and with .NET 5 Preview 7, we've added a new `Half` type (equivalent to `binary16`)!

A `Half` is a binary floating-point number that occupies 16 bits. With half the number of bits as `float`, a `Half` number can represent values in the range ±65504. More formally, the `Half` type is defined as a base-2 16-bit interchange format meant to support the exchange of floating-point data between implementations. One of the primary use cases of the `Half` type is to save on storage space where the computed result does not need to be stored with full precision. Many computation workloads already take advantage of the `Half` type: machine learning, graphics cards, the latest processors, native SIMD libraries etc. With the new `Half` type, we expect to unlock many applications in these workloads.

## Let's explore the `Half` type

The 16 bits in the `Half` type are split into:
1. Sign bit: 1 bit
2. Exponent bits: 5 bits
3. Significand bits: 10 bits (with 1 implicit bit that is not stored)

Despite that fact that the significand is made up of 10 bits, the total precision is really 11 bits. The format is assumed to have an implicit leading bit of value 1 (unless the exponent field is all zeros, in which case the leading bit has a value 0). To represent the number `1` in the `Half` format, we'd use the bits:

`0 01111 0000000000` = `1`

The leading bit (our sign bit) is 0, indicating a positive number. The exponent bits are `01111`, or 15 in decimal. However, the exponent bits don't represent the exponent directly. Instead, an exponent bias is defined that lets the format represent both positive and negative exponents. For the `Half` type, that exponent bias is `15`. The true exponent is derived by subtracting `15` from the stored exponent. Therefore, `01111` represents the exponent `e = 01111 (in binary) - 15 (the exponent bias) = 0`. The significand is `0000000000`, which can be interpreted as the number `.significand(in base 2)` in base 2, `0` in our case. If, for example, the `significand` was `0000011010` (26 in decimal), we can divide its decimal value `26` by the number of values representable in 10 bits (`1 << 10`): so the significand `0000011010 (in binary)` is `26 / (1 << 10) = 26 / 1024 = 0.025390625` in decimal. Finally, because our stored exponent bits (`01111`) are not all `0`, we have an implicit leading bit of `1`. Therefore, 

`0 01111 0000000000` = `2^0 * (1 + 2^0)` = `1`

In general, the `16` bits of a `Half` value are interpreted as `-1^(sign bit) * 2^(storedExponent - 15) * (implicitBit + (significand/1024))`. A special case exists for the stored exponent `00000`. In this case, the bits are interpreted as `-1^(sign bit) * 2^(-14) * (0 + (significand/1024))`. Let's look at the bit representations of some other numbers in the `Half` format:

### Smallest positive non-zero value
`0 00000 0000000001` = `-1^(0) * 2^(-14) * (0 + 1/1024)` ≈ `0.000000059604645` (Note the implicit bit is `0` here because the stored exponents bits are all `0`)

### Largest normal number
`0 11110 1111111111` = `-1^(0) * 2^(15) * (1 + 1023/1024)` ≈ `65504`

### Negative Infinity
`1 11111 0000000000` = `-Infinity`

A peculiarity of the format is that it defines both positive and negative 0:

`1 00000 0000000000` = `-0`
 
`0 00000 0000000000` = `+0`


## Conversions to/from `float/double`

A `Half` can be converted to/from a `float/double` by simply casting it:

`float f = (float)half;`

`Half h = (Half)floatValue;`

Any `Half` value, because `Half` uses only 16 bits, can be represented as a `float/double` without loss of precision. However, the inverse is not true. Some precision may be lost when going from `float/double` to `Half`. In .NET 5.0, the `Half` type is primarily an interchange type with no arithmetic operators defined on it. It only supports parsing, formatting and comparison operators. All arithmetic operations will need an explicit conversion to a `float/double`. Future versions will consider adding arithmetic operators directly on `Half`.

As library authors, one of the points to consider is that a language can add support for a type in the future. It is conceivable that C# adds a `half` type in the future. Language support would enable an identifier such as `f16`(similar to the `f` that exists today) and implicit/explicit conversions. Thus, the library defined type `Half` needs to be defined in a manner that does not result in any breaking changes if `half` becomes a reality. Specifically, we needed to be careful about adding operators to the `Half` type. Implicit conversions to `float/double` could lead to potential breaking changes if language support is added. On the other hand, having a `Float/Double` property on the `Half` type felt less than ideal. In the end, we decided to add explicit operators to convert to/from `float/double`. If C# does add support for `half`, no user code would break, since all casts would be explicit.

## Adoption
We expect that `Half` will find its way into many codebases. The `Half` type plugs a gap in the .NET ecosystem and we expect many numerics libraries to take advantage of it. In the open source arena, ML.NET is expected to start using `Half`, the Apache Arrow project's C# implementation has an [open issue](https://issues.apache.org/jira/browse/ARROW-9048?jql=project%20%3D%20ARROW%20AND%20text%20~%20%22Half%20type%22) for it and the `DataFrame` library tracks a related issue [here](https://github.com/dotnet/corefxlab/issues/2930). As more intrinsics are unlocked in .NET for x86 and ARM processors, we expect that computation performance with `Half` can be accelerated and result in more efficient code!