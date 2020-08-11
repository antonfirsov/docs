In computing, a `Half` is a binary floating-point number that occupies 16 bits. With half the number of bits as the more popular `single/float`, a `Half` number can represent values in the range ±65504. More formally, the `Half` type is defined in the IEEE754-2008 standard as a base-2 16-bit interchange format meant to support the exchange of floating-point data between implementations. The primary use case of the `Half` type is in applications where higher precision is not required for arithmetic computations and data sizes need to be kept small. Many computation workloads already take advantage of the `Half` type: graphics cards, the latest processors, native SIMD libraries etc. Now, .NET 5 has added support for an IEEE754 compliant `Half` type in the `System` namespace!

## Let's explore the `Half` type

The 16 bits in the `Half` type are split into 
1. Sign bit: 1 bit
2. Exponent bits: 5 bits
3. Significand bits: 10 bits (with 1 implicit bit that is not stored)

Despite that fact that the significand is made up of 10 bits, the total precision is really 11 bits. The format is assumed to have an implicit leading bit of value 1 (unless the exponent field is all zeros, in which case the leading bit has a value 0). In addition, the exponent bits don't represent the exponent directly. Instead, an exponent bias is defined that lets the format reprsent both positive and negative exponents. For the `Half` type, that exponent bias is `15`. The true exponent is derived by subtracting `15` from the stored exponent. Let's look at the bit representations of various numbers in the `Half` format:

### Smallest positive subnormal number (a.k.a precision)
0 00000 0000000001 ≈ 0.000000059604645

### Largest normal number
0 11110 1111111111 ≈ 65504

### The number 1
0 01111 0000000000 = 1

### Negative Infinity
1 11111 0000000000 = -Infinity

A pecularity of the format is that it defines both positive and negative 0:

1 00000 0000000000 = -0
 
0 00000 0000000000 = +0

Finally, because `Half` uses only 16 bits, any `Half` value can be represented as a `float/double` with loss of precision. However, the inverse is not true. Some precision may be lost when going from `float/double` to `Half`.


## Design Considerations
As library authors, one of the points to consider is that a language can add support for a type in the future. It is not inconceivable that C# adds a `half` type in the future.  Language support would enable checked/unchecked contexts, an identifer such as `f16`(similar to the `f` that exists today) and implicit/explicit conversions. Thus, the library defined type `Half` needs to be defined in a manner that does not result in any breaking changes if `half` becomes a reality. Specifically, we needed to be careful about adding operators to the `Half` type. Implicit conversions to `float/double` could lead to potential breaking changes when language support was added. On the other hand, having a `Float/Double` property on the `Half` type felt less than ideal. In the end, we decided to add explicit operators to convert to/from `float/double`. When C# does add support for `half`, no user code would break, since all casts would be explicit. The final consideration was the addition of arithmetic operators to `Half`. For version 1, the `Half` type is primarily an interchange type with no arithmetic operators defined on it. It only supports parsing, formatting and comparison operators. However, any arithmetic operation can still be performed with an explicit conversion to a `float/double`. Future versions will consider adding arithmetic operators directly on `Half`. 

We expect that `Half` will find it's way into many codebases. The `Half` type plugs a gap in the .NET ecosystem and we expect many numerics libraries to take advantage of it. In the open source arena, ML.NET is expected to start using `Half`, the Apache Arrow project's C# implementation has an [open issue](https://issues.apache.org/jira/browse/ARROW-9048?jql=project%20%3D%20ARROW%20AND%20text%20~%20%22Half%20type%22) for it and the `DataFrame` library tracks a related issue [here](https://github.com/dotnet/corefxlab/issues/2930). As more intrinsics are unlocked in .NET for Intel and ARM processors, we expect that computation performance with `Half` can be accelerated and result in more efficient code!