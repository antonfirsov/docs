# .NET for Apache® Spark™ In-Memory DataFrame Support

[.NET for Apache Spark][1] is aimed at making [Apache® Spark™][2], and thus the exciting world of big data analytics, accessible to .NET developers. .NET for Spark can be used for processing batches of data, real-time streams, machine learning, and ad-hoc query.

The [DataFrame][3] is one of the core data structures in Spark programming. A DataFrame is a distributed collection of data organized into named columns. In a Spark application, we typically start off by reading input data from a data source, storing it in a DataFrame, and then leveraging functionality like Spark SQL to transform and gain insights from our data. User-defined functions, or UDFs, are column-based functions that allow us to manipulate data stored in DataFrames.

In December 2019, the [.NET team announced][4] the preview of the [Microsoft.Data.Analysis.DataFrame][5] type to make data exploration easy in .NET. Now in March 2020, we have [introduced convenience APIs][6] to the .NET for Spark codebase for using Microsoft.Data.Analysis.DataFrame objects with UDFs in Spark. These convenience APIs make data manipulation and analysis with UDFs much more convenient and concise in .NET for Spark.

In this blog post, we’ll explore:
*   [Implementation goal and details][7]
*   [Coding examples and DataFrame comparisons][8]
*   [Wrap Up][9]

### <a id="implement"></a>Implementation goals and details

#### Context and Goals

Let's start off with some context about data-sharing in Spark UDFs. Apache Spark streams data to Arrow-based UDFs in the [Apache Arrow][10] format. Apache Arrow provides a standardized, language-independent format for working with data in-memory. It's designed for high-performance, efficient analysis through its columnar memory format, and it provides libraries and zero-copy messaging for communication across processes.

![Diagram of the benefits of Apache Arrow][logo]
###### Diagram credit: https://arrow.apache.org/.

Because Spark streams data to UDFs in the Arrow format, you need to understand the Arrow format when working with UDFs, such as how to read Arrow columns, write to Arrow columns, and unwrap a RecordBatch, which is a 2D data type in Arrow consisting of a set of rows of equal-length columns. 

The main goal of the work described in this blog post is to improve scalar and vector UDFs in .NET for Spark through a set of convenience APIs. You can now use Microsoft.Data.Analysis.DataFrame objects in your .NET for Spark apps, which take care of working with Arrow-formatted data for you behind-the-scenes of your UDFs.

#### Details

There are a few kinds of Spark UDFs: pickling, scalar, and vector. Our convenience APIs specifically apply to scalar and vector UDFs.

[Python pickling UDFs][16] are an older version of Spark UDFs. They leverage the Python pickling format of serialization, rather than Arrow, to convert data between the JVM and .NET for Spark processes. Once a column is specified for processing, a pickling UDF will take each of its rows, apply the given functionality, and then add a new column, resulting in quite a bit of overhead.

By contrast, scalar and vector UDFs leverage Arrow serialization, enabling them to reap the benefits of an in-memory columnar format and making data transfers more efficient. Once data is serialized to the Arrow format, it can be used directly in the Spark processes and doesn't need to be serialized or deserialized anymore, which is a major improvement over the Python pickling format. Arrow can create DataFrames using zero-copy methods across chunks of data (multiple rows and columns all at once) rather than row-by-row. 

Our new .NET for Apache Spark convenience APIs specifically apply to scalar and vector UDFs since they use Arrow.

Prior to these convenience APIs, you needed to enumerate an Arrow RecordBatch to work with columns in an Arrow-based UDF. RecordBatches are Arrow-based objects, not standard Spark objects, and can thus disrupt the flow or familiarity of code in your program.

Our new APIs automatically wrap data in a Microsoft.Data.Analysis.DataFrame instead of a RecordBatch. The wrapping doesn’t involve copying data, thus ensuring performance remains high as our ease of coding also improves. 

You can use both traditional Spark SQL and Microsoft.Data.Analysis.DataFrames in your programs. The traditional Spark DataFrame distributes data across your Spark cluster. It's used for the entire dataset in your Spark driver program. Once you create a UDF, the data in the traditional DataFrame will be streamed to the UDF on the worker machines in the Arrow format. Once inside the UDF, you’ll now work with the Microsoft.Data.Analysis.DataFrame (rather than RecordBatches)- it will be in-memory on a single machine. The concept of the Microsoft.Data.Analysis.DataFrame is similar to the [Python Pandas DataFrame][15]. 

### <a id="example"></a>Example

#### Simple Examples

Let’s start with a basic example. Suppose we have a vector UDF that adds 2 columns and returns the result. Traditionally, the UDF would take in 2 ArrowArrays (for example, DoubleArray) and return a new ArrowArray. You would unfortunately need to create the new array, and then loop over each row in the data. In addition to the loop, you'd also need to deal with Builder objects in the Arrow library to create the new array, resulting in more allocations than necessary.

If we instead used the Microsoft.Data.Analysis.DataFrame, we can write something along the lines of: `dataframe.ColumnA + dataframe.ColumnB`. Isn't that convenient!

As another example, we often create UDFs that return a set of columns. With the traditional Spark DataFrames, these columns must be returned as an Arrow RecordBatch. But with our new convenience APIs, we can just return a DataFrame, and everything else is handled internally!

#### Detailed Examples

<b><i>Vector Udfs</i></b>

Let’s take a look at a more detailed, concrete example. [VectorUdfs.cs][12] is a program using the traditional Spark DataFrame. It reads in a Json file with people’s names and ages as input and stores the data in a DataFrame. The program leverages Spark to group records by the same age, and then applies a custom UDF over each age group. 

Let's say you have 2 people with the same age:

<pre class="prettyprint">21 | John
21 | Sally</pre>

The UDF will count the number of characters in all the names that have the same age. So, in this case, you'd get 1 row back: `21 | 9`, where 9 is the result of "John".Length + "Sally".Length.

[VectorDataFrameUdfs.cs][13] is an updated program that accomplishes the same task with both a traditional DataFrame and the Microsoft.Data.Analysis.DataFrame. 

Both programs use a Grouped Map Vector UDF and apply it very similarly. In VectorUdfs.cs, the code is as follows:

<pre class="prettyprint">
df.GroupBy("age")
    .Apply(
    new StructType(new[]
    {
        new StructField("age", new IntegerType()),
        new StructField("nameCharCount", new IntegerType())
    }),
r => CountCharacters(r))
</pre>

`CountCharacters` is implemented differently in each program. In VectorUdfs.cs, the definition is:

<pre class="prettyprint">
private static RecordBatch CountCharacters(RecordBatch records)
{
    StringArray nameColumn = records.Column("name") as StringArray;

    int characterCount = 0;

    for (int i = 0; i < nameColumn.Length; ++i)
    {
        string current = nameColumn.GetString(i);
        characterCount += current.Length;
    }

    int ageFieldIndex = records.Schema.GetFieldIndex("age");
    Field ageField = records.Schema.GetFieldByIndex(ageFieldIndex);

    // Return 1 record, if we were given any. 0, otherwise.
    int returnLength = records.Length > 0 ? 1 : 0;

    return new RecordBatch(
        new Schema.Builder()
            .Field(ageField)
            .Field(f => f.Name("name_CharCount").DataType(Int32Type.Default))
            .Build(),
        new IArrowArray[]
        {
            records.Column(ageFieldIndex),
            new Int32Array.Builder().Append(characterCount).Build()
        },
        returnLength);
}
</pre>

In VectorDataFrameUdfs.cs, the method is:

<pre class="prettyprint">
private static FxDataFrame CountCharacters(FxDataFrame dataFrame)
{
    int characterCount = 0;

    var characterCountColumn = new PrimitiveDataFrameColumn<int>("nameCharCount");
    var ageColumn = new PrimitiveDataFrameColumn<int>("age");
    ArrowStringDataFrameColumn nameColumn = dataFrame["name"] as ArrowStringDataFrameColumn;
    for (long i = 0; i < dataFrame.Rows.Count; ++i)
    {
        characterCount += nameColumn[i].Length;
    }

    if (dataFrame.Rows.Count > 0)
    {
        characterCountColumn.Append(characterCount);
        ageColumn.Append((int?)dataFrame["age"][0]);
    }

    return new FxDataFrame(ageColumn, characterCountColumn);
}
</pre>

Note that the `FxDataFrame` type represents the Microsoft.Data.Analysis.DataFrame, while `DataFrame` represents the traditional Spark DataFrame. This is signified in the latter sample at the top of the program:

<pre class="prettyprint">
using DataFrame = Microsoft.Spark.Sql.DataFrame;
using FxDataFrame = Microsoft.Data.Analysis.DataFrame;
</pre>

As you can see, the latter `CountCharacters` implementation deals completely with DataFrames rather than RecordBatches. It is also almost half the length of the first implementation!

In this single comparison, we can see where these new APIs add a great deal of convenience to our .NET for Spark apps. VectorUdfs.cs requires us to convert to and from the RecordBatch type, requiring many extra lines of code. By contrast, the new VectorDataFrameUdfs.cs sample can dive immediately into our data processing. 

<b><i>TPC-H Vector Functions</i></b>

.NET for Apache Spark is designed for high performance and performs well on the [TPC-H benchmark][17]. The TPC-H benchmark consists of a suite of business-oriented ad hoc queries and concurrent data modifications. The queries and the data populating the database have been chosen to have broad industry-wide relevance.

[VectorFunctions.cs][18] and [VectorDataFrameFunctions.cs][19] both perform the same `ComputeTotal` TPC-H function where prices are calculated based on taxes and discounts. However, only the latter program leverages Microsoft.Data.Analysis.DataFrame. 

`ComputeTotal` in VectorFunctions.cs:

<pre class="prettyprint">
internal static DoubleArray ComputeTotal(DoubleArray price, DoubleArray discount, DoubleArray tax)
{
    if ((price.Length != discount.Length) || (price.Length != tax.Length))
    {
        throw new ArgumentException("Arrays need to be the same length");
    }

    int length = price.Length;
    var builder = new DoubleArray.Builder().Reserve(length);
    ReadOnlySpan<double> prices = price.Values;
    ReadOnlySpan<double> discounts = discount.Values;
    ReadOnlySpan<double> taxes = tax.Values;
    for (int i = 0; i < length; ++i)
    {
        builder.Append(prices[i] * (1 - discounts[i]) * (1 + taxes[i]));
    }

    return builder.Build();
}
</pre>

Whereas the logic in `ComputeTotal` in VectorDataFrameFunctions.cs (not including the initial array length check) is just 1 line!

<pre class="prettyprint">return price * (1 - discount) * (1 + tax);</pre>

Now we can harness the tremendous benefits of Apache Arrow without extra code overhead or confusion – awesome! 

### <a id="wrapup"></a>Wrap Up

Thank you to Prashanth Govindarajan, Eric Erhardt, Terry Kim, and the other members of the .NET and .NET for Apache Spark teams for their contributions to this outstanding work.

We’d love to help you get started with .NET for Apache Spark and hear your feedback. You can [Request a Demo][1] from our landing page and check out the [.NET for Spark GitHub repo][14] to get involved with our effort to make .NET a great tech stack for building big data applications.

[1]: https://dot.net/spark
[2]: https://spark.apache.org/
[3]: https://docs.microsoft.com/dotnet/api/microsoft.spark.sql.dataframe?view=spark-dotnet
[4]: https://devblogs.microsoft.com/dotnet/an-introduction-to-dataframe/
[5]: https://www.nuget.org/packages/Microsoft.Data.Analysis/
[6]: https://github.com/dotnet/spark/pull/277
[7]: #implement
[8]: #example
[9]: #wrapup
[10]: https://arrow.apache.org/
[11]: https://github.com/dotnet/spark/blob/master/benchmark/csharp/Tpch/VectorFunctions.intrinsics.cs
[12]: https://github.com/dotnet/spark/blob/master/examples/Microsoft.Spark.CSharp.Examples/Sql/Batch/VectorUdfs.cs
[13]: https://github.com/dotnet/spark/blob/master/examples/Microsoft.Spark.CSharp.Examples/Sql/Batch/VectorDataFrameUdfs.cs
[14]: https://github.com/dotnet/spark
[logo]: Arrow.png
[15]: https://pandas.pydata.org/pandas-docs/stable/getting_started/10min.html
[16]: https://docs.python.org/3/library/pickle.html
[17]: http://www.tpc.org/tpch/
[18]: https://github.com/dotnet/spark/blob/master/benchmark/csharp/Tpch/VectorFunctions.cs#L12
[19]: https://github.com/dotnet/spark/blob/master/benchmark/csharp/Tpch/VectorDataFrameFunctions.cs#L12
